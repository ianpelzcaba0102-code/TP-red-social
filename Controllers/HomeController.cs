using Microsoft.AspNetCore.Mvc;

namespace TP05_SESSIONS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                return RedirectToAction("Index", "Publicacion");
            }

            return RedirectToAction("Login", "Usuario");
        }
    }
}
