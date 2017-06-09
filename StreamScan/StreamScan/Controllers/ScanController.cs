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
    /// <summary>
    /// Controller de la page "Scan"
    /// </summary>
    [Authorize]
    public class ScanController : Controller
    {
        private IDal dal;

        public ScanController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Affiche la page de scan avec la combobox des entreprises
        /// </summary>
        public ActionResult Index()
        {
            List<Enterprise> enterprises = dal.GetEnterprises();
            enterprises.Insert(0, new Enterprise { Name = "Select an enterprise...", Id = 0 });

            return View(new ScanViewModel { Enterprises = enterprises });
        }

        /// <summary>
        /// Récupère les ouvrages de l'entreprise spécifiée et remplit la combobox 
        /// avec ceux-ci puis affiche le formulaire de connexion à la machine.
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise dont on récupère les ouvrages</param>
        public ActionResult GetFacilities(int enterprise)
        {
            if (enterprise == 0)
            {
                //throw new Exception("Please select an enterprise");
                TempData["Exception_Message"] = "Please select an enterprise";
                return Redirect("/Error/AjaxError");
            }
            List<Facility> facilities = dal.GetFacilities(enterprise);
            facilities.Insert(0, new Facility { Name = "Select a facility...", Id = 0 });

            return PartialView("ScanConnect", new ScanViewModel { Facilities = facilities });
        }

        /// <summary>
        /// Connecte l'utilisateur à la machine dont l'adresse est spécifiée
        /// </summary>
        /// <param name="facility">L'ID de l'ouvrage</param>
        /// <param name="MachineAddress">L'adresse de la machine à laquelle on se connecte</param>
        public ActionResult Connect(int facility, string MachineAddress)
        {
            if (facility == 0)
            {
                //throw new Exception("Please select a facility");
                TempData["Exception_Message"] = "Please select a facility";
                return Redirect("/Error/AjaxError");
            }
            if (MachineAddress == "")
            {
                //throw new Exception("Please enter an address");
                TempData["Exception_Message"] = "Please enter an address";
                return Redirect("/Error/AjaxError");
            }
            Session["facilityId"] = facility;
            bool ok = ClientWCF.CheckStatus(String.Format("http://{0}:{1}/{2}", MachineAddress, ClientWCF.defaultPort, ClientWCF.defaultServiceName));
            if (!ok)
            {
               // throw new Exception("No server has been found to this address");
                TempData["Exception_Message"] = "No server has been found to this address";
                return Redirect("/Error/AjaxError");
            }
            ClientWCF wcf = new ClientWCF();
            wcf.InitClient(MachineAddress);
            Session["wcf"] = wcf;
            return PartialView("ScanInfosButton");
        }

        /// <summary>
        /// Récupère les infos de la machine et les affiche
        /// </summary>
        public ActionResult GetInfos()
        {
            ClientWCF wcf = (ClientWCF)Session["wcf"];
            if (wcf == null)
            {
                //throw new Exception("The connection has ended. Please reconnect to the server");
                TempData["Exception_Message"] = "The connection has ended. Please reconnect to the server";
                return Redirect("/Error/AjaxError");
            }
            Object infos = wcf.SendMessage("GetInfos");
            if (infos.GetType() == typeof(string))
                return PartialView("ScanInfos", new InfosViewModel { Error = (string)infos });
            int facility = (int)Session["facilityId"];
            Session["infos"] = (Info)infos;
            return PartialView("ScanInfos", new InfosViewModel { Infos = (Info)infos, Machines = dal.GetFacilityMachines(facility) });
        }

        /// <summary>
        /// Insert la machine dans la base de données
        /// </summary>
        [HttpPost]
        public ActionResult InsertMachine()
        {
            Info infos = (Info)Session["infos"];
            if (infos == null)
            {
                TempData["Exception_Message"] = "The connection has ended. Please reconnect to the server";
                return Redirect("/Error");
            }
            int facility = (int)Session["facilityId"];
            MySqlReturn sqlR = dal.InsertMachine(facility, infos);
            if (!sqlR.IsOk)
            {
                if (sqlR.ErrorMessage.Contains("Cannot add or update a child row"))
                    TempData["Error"] = "Can't add the machine : the enterprise or the facility in which the machine should be inserted has been deleted.";
                else
                    TempData["Error"] = sqlR.ErrorMessage;
            }
            else
                TempData["Message"] = "The machine has been inserted !";
            return Redirect("/");
        }

        /// <summary>
        /// Met à jour la machine dans la base de données
        /// </summary>
        /// <param name="machineId">L'ID de la machine</param>
        [HttpPost]
        public ActionResult UpdateMachine(int? version, int? systemId)
        {
            Info infos = (Info)Session["infos"];
            if (infos == null)
            {
                TempData["Exception_Message"] = "The connection has ended. Please re-scan the machine to update it correctly.";
                return Redirect("/Error");
            }
            MySqlReturn sqlR;
            if (systemId != null)
            {
                sqlR = dal.UpdateMachine(version.GetValueOrDefault(), systemId.GetValueOrDefault(), infos);
            }
            else
            {
                sqlR = new MySqlReturn { IsOk = false, ErrorMessage = "Can't update the machine : No machine selected" };
            }

            if (!sqlR.IsOk)
            {
                if (sqlR.ErrorMessage.Contains("Cannot add or update a child row"))
                    TempData["Error"] = "Can't add the machine : the enterprise or the facility in which the machine should be inserted has been deleted.";
                else
                    TempData["Error"] = sqlR.ErrorMessage;
            }
            else
                TempData["Message"] = "The machine has been updated !";
            return Redirect("/");
        }
    }
}