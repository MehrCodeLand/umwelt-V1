using Microsoft.AspNetCore.Mvc;

namespace umweltV1.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class HomeController : Controller
    {
        public IActionResult AdminHome() => View();
    }
}
