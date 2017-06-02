using QueriesManager.Bean;
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
    [Authorize]
    public class ScanController : Controller
    {
        private IDal dal;

        public ScanController()
        {
            dal = new Dal();
        }

        public ActionResult Index()
        {
            /*try
            {
            }
            catch (Exception ex)
            {
                TempData["Exception_Message"] = ex.Message;
                return Redirect("/Error");
            }*/
            List<Enterprise> enterprises = dal.GetEnterprises();
            Dictionary<string, string> arr = new Dictionary<string, string>();
            arr.Add("Select an enterprise...", "-1");
            foreach (Enterprise enterprise in enterprises)
            {
                arr.Add(enterprise.Name, "" + enterprise.Id);
            }
            return View(arr);
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
                    arr.Add(facility.Name, "" + facility.Id);
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
                Session["facilityId"] = facility;
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
            if (wcf == null)
                throw new Exception("The connection has ended. Please reconnect to the server");
            Object infos = wcf.SendMessage("GetInfos");
            if (infos.GetType() == typeof(string))
                return PartialView("ScanInfos", new InfosViewModel { Error = (string)infos });
            int facility = (int)Session["facilityId"];
            Session["infos"] = (Info)infos;
            return PartialView("ScanInfos", new InfosViewModel { Infos = (Info)infos, Machines = dal.GetFacilityMachines(facility) });
        }

        [HttpPost]
        public ActionResult InsertMachine()
        {
            Info infos = (Info)Session["infos"];
            if (infos == null)
                throw new Exception("The connection has ended. Please reconnect to the server");
            int facility = (int)Session["facilityId"];
            MySqlReturn sqlR = dal.InsertMachine(facility, infos);
            if (!sqlR.IsOk)
                TempData["Error"] = sqlR.ErrorMessage;
            else
                TempData["Message"] = "The machine has been inserted !";
            return Redirect("/");
        }

        [HttpPost]
        public ActionResult UpdateMachine(int? machineId)
        {
            Info infos = (Info)Session["infos"];
            if (infos == null)
                throw new Exception("The connection has ended. Please reconnect to the server");
            MySqlReturn sqlR;
            if (machineId != null)
            {
                sqlR = dal.UpdateMachine(machineId.GetValueOrDefault(), infos);
            }
            else
            {
                sqlR = new MySqlReturn { IsOk = false, ErrorMessage = "Can't update the machine : No machine selected" };
            }

            if (!sqlR.IsOk)
                TempData["Error"] = sqlR.ErrorMessage;
            else
                TempData["Message"] = "The machine has been updated !";
            return Redirect("/");
        }
    }
}