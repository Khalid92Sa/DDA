using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DDA.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error()
        {
            ViewBag.ExceptionMessage = (string)this.RouteData.Values["Description"];
            ViewBag.ExceptionName = (string)this.RouteData.Values["Name"];

            return View();
        }
    }
}