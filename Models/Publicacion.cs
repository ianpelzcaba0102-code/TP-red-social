namespace TP05_SESSIONS.Models
{
    public class Publicacion
    {
        public int IdPublicacion { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int CantidadLikes { get; set; }
        public bool UsuarioDioLike { get; set; }
        public List<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}
