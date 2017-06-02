using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.Controllers
{
    [Authorize]
    public class FacilitiesController : Controller
    {

        private IDal dal;

        public FacilitiesController()
        {
            dal = new Dal();
        }
        
        public ActionResult Delete(int id)
        {

            return View();
        }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult New()
        {

            return View();
        }
        
        public ActionResult New(Facility facility)
        {

            return View();
        }
        
        public ActionResult Update(int id)
        {

            return View();
        }
        
        public ActionResult Update(Enterprise enterprise)
        {

            return View();
        }

    }

}