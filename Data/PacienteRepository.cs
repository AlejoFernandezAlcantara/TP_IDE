using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data
{
    
    public class PacienteRepository : IPacienteRepository
    {
        private const int ErrorViolacionFk = 547;

        private const string MensajeConReservas =
            "No se puede eliminar el paciente porque tiene reservas registradas. " +
            "Las reservas atendidas no se pueden borrar para conservar el historial.";

        // El orden de estas columnas debe coincidir con los índices de Map().
        private const string ColumnasPaciente =
            "NroPaciente, Nombre, Apellido, NroDni, TipoDni, Direccion, Telefono, Email, PasswordHash, Id, Activo";

        private readonly string _connectionString;

        public PacienteRepository(IConfiguration configuration)
        {
            // Misma clave que usa Program.cs para AppDbContext.
            _connectionString = configuration.GetConnectionString("Default")
                ?? throw new InvalidOperationException("No se encontró la cadena de conexión 'Default'.");
        }

        public async Task<List<Paciente>> GetAllAsync()
        {
            var pacientes = new List<Paciente>();

            await using var cn = await AbrirAsync();
            await using var cmd = new SqlCommand($"SELECT {ColumnasPaciente} FROM Pacientes", cn);
            await using var rd = await cmd.ExecuteReaderAsync();

            while (await rd.ReadAsync())
                pacientes.Add(Map(rd));

            return pacientes;
        }

        public async Task<Paciente?> GetByNroPacienteAsync(int nroPaciente)
        {
            await using var cn = await AbrirAsync();
            await using var cmd = new SqlCommand(
                $"SELECT TOP (1) {ColumnasPaciente} FROM Pacientes WHERE NroPaciente = @NroPaciente", cn);
            cmd.Parameters.AddWithValue("@NroPaciente", nroPaciente);

            await using var rd = await cmd.ExecuteReaderAsync();
            return await rd.ReadAsync() ? Map(rd) : null;
        }

        public async Task AddAsync(Paciente paciente)
        {
            const string sql = @"
                INSERT INTO Pacientes (Nombre, Apellido, NroDni, TipoDni, Direccion, Telefono,
                                       Email, PasswordHash, Id, Activo)
                OUTPUT INSERTED.NroPaciente
                VALUES (@Nombre, @Apellido, @NroDni, @TipoDni, @Direccion, @Telefono,
                        @Email, @PasswordHash, @Id, @Activo);";

            await using var cn = await AbrirAsync();
            await using var cmd = new SqlCommand(sql, cn);
            AgregarParametros(cmd, paciente);

           
            paciente.NroPaciente = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task UpdateAsync(Paciente paciente)
        {
           
            var existente = await GetByNroPacienteAsync(paciente.NroPaciente)
                ?? throw new InvalidOperationException("Paciente no encontrado.");

            existente.SetNombre(paciente.Nombre);
            existente.SetApellido(paciente.Apellido);
            existente.SetDireccion(paciente.Direccion);
            existente.SetTelefono(paciente.Telefono);

            
            const string sql = @"
                UPDATE Pacientes
                   SET Nombre = @Nombre,
                       Apellido = @Apellido,
                       Direccion = @Direccion,
                       Telefono = @Telefono
                 WHERE NroPaciente = @NroPaciente;";

            await using var cn = await AbrirAsync();
            await using var cmd = new SqlCommand(sql, cn);
            cmd.Parameters.AddWithValue("@NroPaciente", existente.NroPaciente);
            cmd.Parameters.AddWithValue("@Nombre", existente.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", existente.Apellido);
            cmd.Parameters.AddWithValue("@Direccion", existente.Direccion);
            cmd.Parameters.AddWithValue("@Telefono", existente.Telefono);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int nroPaciente)
        {
            await using var cn = await AbrirAsync();

            if (!await ExisteAsync(cn, nroPaciente))
                throw new InvalidOperationException("Paciente no encontrado.");

            // Las reservas bloquean el borrado para conservar el historial.
            if (await TieneReservasAsync(cn, nroPaciente))
                throw new InvalidOperationException(MensajeConReservas);

            try
            {
                await using var cmd = new SqlCommand(
                    "DELETE FROM Pacientes WHERE NroPaciente = @NroPaciente", cn);
                cmd.Parameters.AddWithValue("@NroPaciente", nroPaciente);
                await cmd.ExecuteNonQueryAsync();
            }
            catch (SqlException ex) when (ex.Number == ErrorViolacionFk)
            {
                // Por si una reserva se crea entre la comprobación y el DELETE.
                throw new InvalidOperationException(MensajeConReservas, ex);
            }
        }

       

        private async Task<SqlConnection> AbrirAsync()
        {
            var cn = new SqlConnection(_connectionString);
            await cn.OpenAsync();
            return cn;
        }

        private static async Task<bool> ExisteAsync(SqlConnection cn, int nroPaciente)
        {
            await using var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Pacientes WHERE NroPaciente = @NroPaciente", cn);
            cmd.Parameters.AddWithValue("@NroPaciente", nroPaciente);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        private static async Task<bool> TieneReservasAsync(SqlConnection cn, int nroPaciente)
        {
            await using var cmd = new SqlCommand(
                "SELECT COUNT(1) FROM Reservas WHERE PacienteId = @NroPaciente", cn);
            cmd.Parameters.AddWithValue("@NroPaciente", nroPaciente);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync()) > 0;
        }

        private static void AgregarParametros(SqlCommand cmd, Paciente p)
        {
            cmd.Parameters.AddWithValue("@Nombre", p.Nombre);
            cmd.Parameters.AddWithValue("@Apellido", p.Apellido);
            cmd.Parameters.AddWithValue("@NroDni", p.NroDni);
            cmd.Parameters.AddWithValue("@TipoDni", (int)p.TipoDni);
            cmd.Parameters.AddWithValue("@Direccion", p.Direccion);
            cmd.Parameters.AddWithValue("@Telefono", p.Telefono);
            cmd.Parameters.AddWithValue("@Email", p.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", p.PasswordHash);
            cmd.Parameters.AddWithValue("@Id", p.Id);
            cmd.Parameters.AddWithValue("@Activo", p.Activo);
        }

       
        private static Paciente Map(SqlDataReader rd)
        {
            var paciente = new Paciente(
                nombre: rd.GetString(1),
                apellido: rd.GetString(2),
                direccion: rd.GetString(5),
                telefono: rd.GetString(6),
                email: rd.GetString(7),
                nroDni: rd.GetInt32(3),
                tipoDni: (tiposEnumerados)rd.GetInt32(4),
                passwordHash: rd.GetString(8))
            {
                NroPaciente = rd.GetInt32(0)
            };

           
            SetProtegida(paciente, nameof(Usuario.Id), rd.GetInt32(9));
            SetProtegida(paciente, nameof(Usuario.Activo), rd.GetBoolean(10));

            return paciente;
        }

        private static void SetProtegida<T>(Usuario usuario, string propiedad, T valor)
        {
            typeof(Usuario)
                .GetProperty(propiedad)!
                .GetSetMethod(nonPublic: true)!
                .Invoke(usuario, new object?[] { valor });
        }
    }
}