using Microsoft.AspNetCore.Mvc;

namespace umweltV1.Areas.Main.Controllers
{
    [Area(nameof(Main))]
    public class HomeController : Controller
    {
        public IActionResult HomeMain() => View();

        [Route("SignUpUser")]
        public IActionResult SignUpUser() => View();
    }
}
