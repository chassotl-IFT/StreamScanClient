using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.Controllers
{
    /// <summary>
    /// Controller des pages d'erreurs
    /// </summary>
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AjaxError()
        {
            return View();
        }
        public ActionResult Error403()
        {
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }
    }
}