using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using umweltV1.Core.Interfaces;
using umweltV1.Data.ViewModels;
using umweltV1.Security.Sender.EmailSender;
using static umweltV1.Security.Sender.EmailSender.RenderFile.ViewToString;

namespace umweltV1.Areas.Main.Controllers
{
    [Area(nameof(Main))]
    public class HomeController : Controller
    {
        private readonly IMainService _main;
        private readonly IViewRenderService _render;
        public HomeController( IMainService main , IViewRenderService render )
        {
            _render = render;
            _main = main;
        }
        public IActionResult HomeMain() => View();




        #region Sign Up


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


            // create new view model 
            var confirmUserVm = _main.CreateConfirmModel(signUpUserVm.Email.ToLower());

            confirmUserVm.Username = signUpUserVm.Username.ToLower();
            confirmUserVm.Email = signUpUserVm.Email.ToLower();
            
            string body = _render.RenderToStringAsync("SignUpView", confirmUserVm);
            EmailSend.Send(signUpUserVm.Email , "Confirm Code-Acticve" , body);

            TempData["success"] = result.Message.ToString();
            return View();
        }


        #endregion


        public IActionResult ConfirmAccount(  string id )
        {
            var data = id;
            // time to validate somthings
            return View();
        }
    }
}
