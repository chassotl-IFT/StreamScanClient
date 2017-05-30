using StreamScan.Models;
using StreamScan.ViewModels;
using StreamScanCommon.Infos;
using StreamScanConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.Controllers
{
    public class ScanController : Controller
    {
        private IDal dal;

        public ScanController()
        {
            dal = new Dal();
        }

        public ActionResult Index()
        {
            List<Enterprise> enterprises = dal.GetEnterprises();
            Dictionary<string, string> arr = new Dictionary<string, string>();
            arr.Add("Select an enterprise...", "-1");
            foreach (Enterprise enterprise in enterprises)
            {
                arr.Add(enterprise.name, "" + enterprise.id);
            }
            ScanViewModel vm = new ScanViewModel { Enterprises = arr };
            return View(vm);
        }

        public ActionResult GetFacilities(int enterprise)
        {
            if (enterprise == -1)
                throw new Exception("Please select an enterprise");
            if (ModelState.IsValid)
            {
                List<Facility> facilities = dal.GetFacilities(enterprise);
                Dictionary<string, string> arr = new Dictionary<string, string>();
                arr.Add("Select a facility...", "-1");
                foreach (Facility facility in facilities)
                {
                    arr.Add(facility.name, "" + facility.id);
                }
                return PartialView("ScanConnect", new ScanViewModel { Facilities = arr });
            }
            throw new Exception("Please enter correct informations");
        }

        public ActionResult Connect(int facility, string MachineAddress)
        {
            if (facility == -1)
                throw new Exception("Please select a facility");
            if (MachineAddress == "")
                throw new Exception("Please enter an address");
            if (ModelState.IsValid)
            {
                bool ok = ClientWCF.CheckStatus(String.Format("http://{0}:{1}/{2}", MachineAddress, ClientWCF.defaultPort, ClientWCF.defaultServiceName));
                if (!ok)
                    throw new Exception("No server has been found to this address");
                ClientWCF wcf = new ClientWCF();
                wcf.InitClient(MachineAddress);
                Session["wcf"] = wcf;
                return PartialView("ScanInfosButton");
            }
            throw new Exception("Please enter correct informations");
        }

        public ActionResult GetInfos()
        {
            ClientWCF wcf = (ClientWCF)Session["wcf"];
            Info iInfos = null;
            string sInfos = "";
            Object infos = wcf.SendMessage("GetInfos");
            if (infos.GetType() == typeof(Info))
            {
                iInfos = (Info)infos;
                sInfos = iInfos.InfosMachine.ToString();
            }
            else if (infos.GetType() == typeof(String))
            {
                sInfos = (string)infos;
                Console.WriteLine(sInfos);
            } else
            {
                sInfos = (string)infos;
            }
            return PartialView("ScanInfos", sInfos);
        }
    }
}