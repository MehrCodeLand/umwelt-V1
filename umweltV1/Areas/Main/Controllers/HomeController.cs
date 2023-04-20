using Microsoft.AspNetCore.Mvc;
using umweltV1.Core.Interfaces;
using umweltV1.Data.ViewModels;

namespace umweltV1.Areas.Main.Controllers
{
    [Area(nameof(Main))]
    public class HomeController : Controller
    {
        private readonly IMainService _main;
        public HomeController( IMainService main )
        {
            _main = main;
        }
        public IActionResult HomeMain() => View();



        [Route("SignUpUser")]
        public IActionResult SignUpUser() => View();

        [HttpPost]
        [Route("SignUpUser")]
        public IActionResult SignUpUser(SignUpUserVm signUpUserVm)
        {
            var result = _main.SignUpUser(signUpUserVm);
            if(result.ErrorId != 0)
            {
                TempData["error"] = result.Message.ToString();
                return View();
            }
            TempData["success"] = "Its Work bitch";
            return View();
        }
    }
}
