using CTBPruebaLibros.Models;
using CTBPruebaLibros.Repositorios.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace CTBPruebaLibros.Repositorios.Implementacion
{
    public class LibroRepository: IGenericoRepositorio<Libro>
    {
        private readonly string _sqlConexion = "";

        public LibroRepository(IConfiguration configuracion)
        {
            _sqlConexion = configuracion.GetConnectionString("LibroBD");
        }

        public async Task<bool> Editar(Libro modelo)
        {
            using (var conexion = new SqlConnection(_sqlConexion))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_EditarLibro", conexion);
                cmd.Parameters.AddWithValue("@Id", modelo.id);
                cmd.Parameters.AddWithValue("@Titulo", modelo.titulo);
                cmd.Parameters.AddWithValue("@Autor", modelo.autor);
                cmd.Parameters.AddWithValue("@Descripcion", modelo.descripcion);
                cmd.Parameters.AddWithValue("@FechaPublicacion", modelo.fechaPublicacion);
                cmd.Parameters.AddWithValue("@Genero", modelo.genero);
                cmd.Parameters.AddWithValue("@Imagen", modelo.imagen);
                cmd.Parameters.AddWithValue("@Cantidad", modelo.cantidad);

                cmd.CommandType = CommandType.StoredProcedure;

                int filas = await cmd.ExecuteNonQueryAsync();

                return filas > 0;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            using (var conexion = new SqlConnection(_sqlConexion))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_EliminarLibro", conexion);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.CommandType = CommandType.StoredProcedure;

                int filas = await cmd.ExecuteNonQueryAsync();

                return filas > 0;
            }
        }

        public async Task<bool> Guardar(Libro modelo)
        {
            using (var conexion = new SqlConnection(_sqlConexion))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_GuardarLibro", conexion);
                cmd.Parameters.AddWithValue("@Titulo", modelo.titulo);
                cmd.Parameters.AddWithValue("@Autor", modelo.autor);
                cmd.Parameters.AddWithValue("@Descripcion", modelo.descripcion);
                cmd.Parameters.AddWithValue("@FechaPublicacion", modelo.fechaPublicacion);
                cmd.Parameters.AddWithValue("@Genero", modelo.genero);
                cmd.Parameters.AddWithValue("@Imagen", modelo.imagen );
                cmd.Parameters.AddWithValue("@Cantidad", modelo.cantidad);

                cmd.CommandType = CommandType.StoredProcedure;

                int filas = await cmd.ExecuteNonQueryAsync();

                return filas > 0;
            }
        }

        public async Task<List<Libro>> Listar()
        {
            List<Libro> listaLibros = new List<Libro>();
            using(var conexion = new SqlConnection(_sqlConexion))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_ListarLibros", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        listaLibros.Add(
                            new Libro
                            {
                                id = Convert.ToInt32(reader["Id"]),
                                titulo = reader["Titulo"].ToString(),
                                autor = reader["Autor"].ToString(),
                                descripcion = reader["Descripcion"] != DBNull.Value ? reader["Descripcion"].ToString() : null,
                                fechaPublicacion = Convert.ToDateTime(reader["FechaPublicacion"]),
                                genero = reader["Genero"].ToString(),
                                imagen = reader["Imagen"] != DBNull.Value ? reader["Imagen"].ToString() : null,
                                cantidad = Convert.ToInt32(reader["Cantidad"])
                            }
                        );
                    }
                }
            }

            return listaLibros;
        }

        public async Task<bool> Prestar(int idLibro, DateTime fechaSalida)
        {
            using (var conexion = new SqlConnection(_sqlConexion))
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand("SP_PrestarLibro", conexion);
                cmd.Parameters.AddWithValue("@Id", idLibro);
                cmd.Parameters.AddWithValue("@FechaPublicacion", fechaSalida);

                cmd.CommandType = CommandType.StoredProcedure;

                object? respuesta = await cmd.ExecuteScalarAsync();
                int resultado = (int)(respuesta ?? 0);

                return resultado == 1;
            }
        }
    }
}
