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
            var message = _admin.CreateRole(createRole);
            if(message.ErrorId < 0)
            {
                TempData["error"] = message.Message.ToString();
                return View();
            }

            TempData["success"] = message.Message.ToString();
            return View();
        }


        [HttpGet]
        [Route("CreatePermission")]
        public IActionResult CreatePermission()
        {
            CreatePermisionVm createPermision = new CreatePermisionVm();
            createPermision.ParentTitle = _admin.GetAllPermissionTitle();
            if(createPermision.ParentTitle.Count < 1 )
            {
                return NotFound();
            }

            return View(createPermision);
        }

        [HttpPost]
        [Route("CreatePermission")]
        public IActionResult CreatePermission(CreatePermisionVm createPermision )
        {
            var resultMessage = _admin.CreatePermission(createPermision);
            if(resultMessage.ErrorId < 0)
            {
                createPermision.ParentTitle = _admin.GetAllPermissionTitle();
                TempData["error"] = resultMessage.Message.ToString();
                return View(createPermision);
            }

            return RedirectToAction("AdminHome");
        }
    }
}
