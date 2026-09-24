using Microsoft.AspNetCore.Mvc;
using TP05_SESSIONS.Models;

namespace TP05_SESSIONS.Controllers
{
    public class UsuarioController : Controller
    {
        BD bd = new BD();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                return RedirectToAction("Index", "Publicacion");
            }

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario usuario)
        {
            string mensaje = usuario.ValidarLogin();
            if (mensaje != "")
            {
                ViewBag.Mensaje = mensaje;
                return View(usuario);
            }

            Usuario usuarioEncontrado = bd.BuscarUsuario(usuario.nombreUsuario, usuario.contraseña);
            if (usuarioEncontrado != null)
            {
                HttpContext.Session.SetString("Usuario", usuarioEncontrado.nombreUsuario);
                HttpContext.Session.SetString("IdUsuario", usuarioEncontrado.IdUsuario.ToString());
                return RedirectToAction("Index", "Publicacion");
            }

            ViewBag.Mensaje = "Usuario o contraseña incorrectos";
            return View(usuario);
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            string mensaje = usuario.ValidarRegistro();

            if (mensaje != "")
            {
                ViewBag.Mensaje = mensaje;
                return View(usuario);
            }

            Usuario usuarioEncontrado = bd.BuscarUsuarioPorNombre(usuario.nombreUsuario);

            if (usuarioEncontrado != null)
            {
                ViewBag.Mensaje = "El usuario ya existe";
                return View(usuario);
            }

            bd.RegistrarUsuario(usuario);

            return RedirectToAction("Login");
        }

        public IActionResult Bienvenida()
        {
            string nombreUsuario = HttpContext.Session.GetString("Usuario");

            if (nombreUsuario == null)
            {
                return RedirectToAction("Login");
            }
            Usuario usuario = bd.BuscarUsuarioPorNombre(nombreUsuario);
            return View(usuario);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}