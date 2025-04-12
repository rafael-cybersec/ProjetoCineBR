using Microsoft.AspNetCore.Mvc;

namespace CineBR.Controllers
{
    public class AdminPagesController : Controller
    {
        public IActionResult CadastroFilmes()
        {
            return View();
        }
        public IActionResult DashboardFilmes()
        {
            return View();
        }
    }
}
