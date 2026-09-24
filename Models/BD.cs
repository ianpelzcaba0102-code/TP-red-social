using Dapper;
using Microsoft.Data.SqlClient;
using TP05_SESSIONS.Models;

namespace TP05_SESSIONS
{
    public class BD
    {
        private string connectionString = "Server=localhost;Database=session;Trusted_Connection=True;TrustServerCertificate=True;";

        public Usuario BuscarUsuario(string nombreUsuario, string contraseña)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuario WHERE nombreUsuario = @nombreUsuario AND contraseña = @contraseña";
                return connection.QueryFirstOrDefault<Usuario>(query, new { nombreUsuario, contraseña });
            }
        }

        public Usuario BuscarUsuarioPorNombre(string nombreUsuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Usuario WHERE nombreUsuario = @nombreUsuario";
                return connection.QueryFirstOrDefault<Usuario>(query, new { nombreUsuario });
            }
        }

        public void RegistrarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Usuario (nombre, apellido, nombreUsuario, contraseña, tipousuario) VALUES (@nombre, @apellido, @nombreUsuario, @contraseña, @tipousuario)";
                connection.Execute(query, usuario);
            }
        }

        public void CrearPublicacion(Publicacion publicacion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Publicacion (Imagen, Titulo, Descripcion, IdUsuario, FechaPublicacion)
                                 VALUES (@Imagen, @Titulo, @Descripcion, @IdUsuario, GETDATE())";
                connection.Execute(query, publicacion);
            }
        }

        public List<Publicacion> ObtenerPublicaciones(int desde, int cantidad, int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT p.IdPublicacion, p.Imagen, p.Titulo, p.Descripcion,
                           p.IdUsuario, u.nombreUsuario AS NombreUsuario, p.FechaPublicacion,
                           (SELECT COUNT(*) FROM [Like] l WHERE l.IdPublicacion = p.IdPublicacion) AS CantidadLikes,
                           CAST(CASE WHEN EXISTS (
                               SELECT 1 FROM [Like] l2
                               WHERE l2.IdPublicacion = p.IdPublicacion AND l2.IdUsuario = @idUsuario
                           ) THEN 1 ELSE 0 END AS bit) AS UsuarioDioLike
                    FROM Publicacion p
                    INNER JOIN Usuario u ON u.IdUsuario = p.IdUsuario
                    ORDER BY p.FechaPublicacion DESC, p.IdPublicacion DESC
                    OFFSET @desde ROWS FETCH NEXT @cantidad ROWS ONLY";

                List<Publicacion> publicaciones = connection.Query<Publicacion>(query, new { desde, cantidad, idUsuario }).ToList();

                foreach (Publicacion publicacion in publicaciones)
                {
                    publicacion.Comentarios = ObtenerComentarios(publicacion.IdPublicacion, connection);
                }

                return publicaciones;
            }
        }

        private List<Comentario> ObtenerComentarios(int idPublicacion, SqlConnection connection)
        {
            string query = @"SELECT c.IdComentario, c.IdPublicacion, c.IdUsuario,
                                    u.nombreUsuario AS NombreUsuario, c.Texto, c.FechaComentario
                             FROM Comentario c
                             INNER JOIN Usuario u ON u.IdUsuario = c.IdUsuario
                             WHERE c.IdPublicacion = @idPublicacion
                             ORDER BY c.FechaComentario ASC, c.IdComentario ASC";

            return connection.Query<Comentario>(query, new { idPublicacion }).ToList();
        }

        public bool PublicacionExiste(int idPublicacion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Publicacion WHERE IdPublicacion = @idPublicacion";
                return connection.ExecuteScalar<int>(query, new { idPublicacion }) > 0;
            }
        }

        public bool UsuarioDioLike(int idPublicacion, int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM [Like] WHERE IdPublicacion = @idPublicacion AND IdUsuario = @idUsuario";
                return connection.ExecuteScalar<int>(query, new { idPublicacion, idUsuario }) > 0;
            }
        }

        public int CambiarLike(int idPublicacion, int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (UsuarioDioLike(idPublicacion, idUsuario))
                {
                    connection.Execute("DELETE FROM [Like] WHERE IdPublicacion = @idPublicacion AND IdUsuario = @idUsuario", new { idPublicacion, idUsuario });
                }
                else
                {
                    connection.Execute("INSERT INTO [Like] (IdPublicacion, IdUsuario) VALUES (@idPublicacion, @idUsuario)", new { idPublicacion, idUsuario });
                }

                return connection.ExecuteScalar<int>("SELECT COUNT(*) FROM [Like] WHERE IdPublicacion = @idPublicacion", new { idPublicacion });
            }
        }

        public Comentario CrearComentario(Comentario comentario)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Comentario (IdPublicacion, IdUsuario, Texto, FechaComentario)
                                 VALUES (@IdPublicacion, @IdUsuario, @Texto, GETDATE());
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";
                comentario.IdComentario = connection.ExecuteScalar<int>(query, comentario);

                return connection.QueryFirstOrDefault<Comentario>(@"SELECT c.IdComentario, c.IdPublicacion, c.IdUsuario,
                    u.nombreUsuario AS NombreUsuario, c.Texto, c.FechaComentario
                    FROM Comentario c INNER JOIN Usuario u ON u.IdUsuario = c.IdUsuario
                    WHERE c.IdComentario = @IdComentario", new { comentario.IdComentario });
            }
        }
    }
}
