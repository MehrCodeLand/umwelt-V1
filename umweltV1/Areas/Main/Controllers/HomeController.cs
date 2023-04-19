using Microsoft.AspNetCore.Mvc;
using umweltV1.Data.ViewModels;

namespace umweltV1.Areas.Main.Controllers
{
    [Area(nameof(Main))]
    public class HomeController : Controller
    {
        public IActionResult HomeMain() => View();

        [Route("SignUpUser")]
        public IActionResult SignUpUser() => View();

        [HttpPost]
        [Route("SignUpUser")]
        public IActionResult SignUpUser(SignUpUserVm upUserVm)
        {
            int x = -23  ;
            if(x > 0)
            {
                TempData["success"] = "signUpUser";
                return View();
            }

            TempData["error"] = "ErrorUser";
            return View();
        }
    }
}
