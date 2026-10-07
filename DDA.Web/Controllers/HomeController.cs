using DDA.Business.Interfaces;
using DDA.Business.Security;
using DDA.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDA.Web.Controllers
{
    public class HomeController : BaseController
    {
        IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        //[RolePermission(UserPermission.Full)]
        public ActionResult Index()
        {
            return View();
        }
    }
}