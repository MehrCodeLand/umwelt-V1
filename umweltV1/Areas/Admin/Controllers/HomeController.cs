using Microsoft.AspNetCore.Mvc;
using umweltV1.Core.Interfaces;
using umweltV1.Data.Models.Structs;
using umweltV1.Data.ViewModels;

namespace umweltV1.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class HomeController : Controller
    {
        private readonly IAdminService _admin;
        public HomeController(IAdminService admin)
        {
            _admin = admin;
        }


        public IActionResult AdminHome() => View();

        [HttpGet]
        [Route("CreateRole")]
        public IActionResult CreateRole() => View();
        
        
        [HttpPost]
        [Route("CreateRole")]
        public IActionResult CreateRole(CreateRoleVm createRole )
        {
            var message = _admin.CreatateRole(createRole);
            if(message.ErrorId < 0)
            {
                TempData["error"] = message.Message.ToString();
                return View();
            }

            TempData["success"] = message.Message.ToString();
            return View();
        }
    }
}
