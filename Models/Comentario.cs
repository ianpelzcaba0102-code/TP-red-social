namespace TP05_SESSIONS.Models
{
    public class Comentario
    {
        public int IdComentario { get; set; }
        public int IdPublicacion { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string Texto { get; set; }
        public DateTime FechaComentario { get; set; }
    }
}
