using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.Controllers
{
    [Authorize]
    public class MachinesController : Controller
    {

        private IDal dal;

        public MachinesController()
        {
            dal = new Dal();
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int id)
        {

            return View();
        }

        [HttpPost]
        public ActionResult Index(int id, int facility)
        {

            return View();
        }
    }
}