

using MiWepAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace MiWepAPI.DATA
{
    public class EmpleadoData
    {
        private readonly string conexion;

        public EmpleadoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }
        public async Task<List<Empleado>> Lista()
        {
            List<Empleado> lista = new List<Empleado>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("listaEmpleado", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using(var reader = await cmd.ExecuteReaderAsync())
                {
                    while ( await reader.ReadAsync())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["idEmpleado"]),
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            Fecha = reader["Fecha"].ToString()
                            
                        });
                    }
                }
            }
            return lista;
        }

        public async Task<Empleado> Obtener(int id)
        {
            Empleado objeto = new Empleado();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();
                SqlCommand cmd = new SqlCommand("obtenerEmpleado", con);
                cmd.Parameters.AddWithValue("@idempleado", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        objeto = new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["idEmpleado"]),
                            Nombre = reader["Nombre"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            Fecha = reader["Fecha"].ToString()

                        };
                    }
                }
            }
            return objeto;
        }

        public async Task<bool> Crear(Empleado objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {
                
                SqlCommand cmd = new SqlCommand("crearEmpleado", con);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@Fecha", objeto.Fecha);

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() >0 ? true: false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Empleado objeto)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("actualizarEmpleado", con);
                cmd.Parameters.AddWithValue("@idEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("@Nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@Correo", objeto.Correo);
                cmd.Parameters.AddWithValue("@Sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@Fecha", objeto.Fecha);

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;

            using (var con = new SqlConnection(conexion))
            {

                SqlCommand cmd = new SqlCommand("EliminarEmpleado", con);
                cmd.Parameters.AddWithValue("@idEmpleado", id);
         

                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await con.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
