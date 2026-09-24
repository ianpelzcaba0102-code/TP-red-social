using Microsoft.AspNetCore.Mvc;
using TP05_SESSIONS.Models;

namespace TP05_SESSIONS.Controllers
{
    public class PublicacionController : Controller
    {
        BD bd = new BD();

        private Usuario UsuarioLogueado()
        {
            string nombreUsuario = HttpContext.Session.GetString("Usuario");
            if (nombreUsuario == null)
            {
                return null;
            }
            return bd.BuscarUsuarioPorNombre(nombreUsuario);
        }

        public IActionResult Index()
        {
            Usuario usuario = UsuarioLogueado();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            List<Publicacion> publicaciones = bd.ObtenerPublicaciones(0, 10, usuario.IdUsuario);
            return View(publicaciones);
        }

        [HttpPost]
        public IActionResult Crear(Publicacion publicacion)
        {
            Usuario usuario = UsuarioLogueado();
            if (usuario == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            if (string.IsNullOrWhiteSpace(publicacion.Titulo) || string.IsNullOrWhiteSpace(publicacion.Descripcion) || string.IsNullOrWhiteSpace(publicacion.Imagen))
            {
                TempData["Mensaje"] = "Complete todos los campos de la publicación";
                return RedirectToAction("Index");
            }

            publicacion.IdUsuario = usuario.IdUsuario;
            bd.CrearPublicacion(publicacion);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CambiarLike(int idPublicacion)
        {
            Usuario usuario = UsuarioLogueado();
            if (usuario == null)
            {
                return Json(new { ok = false, mensaje = "Debe iniciar sesión" });
            }

            if (!bd.PublicacionExiste(idPublicacion))
            {
                return Json(new { ok = false, mensaje = "La publicación no existe" });
            }

            bool teniaLike = bd.UsuarioDioLike(idPublicacion, usuario.IdUsuario);
            int cantidadLikes = bd.CambiarLike(idPublicacion, usuario.IdUsuario);

            return Json(new { ok = true, cantidadLikes, dioLike = !teniaLike });
        }

        [HttpPost]
        public IActionResult Comentar(int idPublicacion, string texto)
        {
            Usuario usuario = UsuarioLogueado();
            if (usuario == null)
            {
                return Json(new { ok = false, mensaje = "Debe iniciar sesión" });
            }

            if (!bd.PublicacionExiste(idPublicacion))
            {
                return Json(new { ok = false, mensaje = "La publicación no existe" });
            }

            if (string.IsNullOrWhiteSpace(texto))
            {
                return Json(new { ok = false, mensaje = "El comentario no puede estar vacío" });
            }

            Comentario comentario = new Comentario
            {
                IdPublicacion = idPublicacion,
                IdUsuario = usuario.IdUsuario,
                Texto = texto.Trim()
            };

            comentario = bd.CrearComentario(comentario);

            return Json(new
            {
                ok = true,
                comentario.IdComentario,
                comentario.NombreUsuario,
                comentario.Texto,
                fechaComentario = comentario.FechaComentario.ToString("dd/MM/yyyy HH:mm")
            });
        }

        [HttpGet]
        public IActionResult ObtenerMas(int desde)
        {
            Usuario usuario = UsuarioLogueado();
            if (usuario == null)
            {
                return Json(new { ok = false, mensaje = "Debe iniciar sesión" });
            }

            List<Publicacion> publicaciones = bd.ObtenerPublicaciones(desde, 10, usuario.IdUsuario);
            return Json(new { ok = true, publicaciones });
        }
    }
}
