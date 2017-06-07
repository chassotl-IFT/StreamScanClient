using StreamScan.Models;
using StreamScan.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /// <summary>
        /// Affiche la page avec une combobox d'entreprises
        /// </summary>
        public ActionResult Index(int? enterprise, int? facility)
        {
            int facilityId = facility.GetValueOrDefault();
            int enterpriseId = enterprise.GetValueOrDefault();

            MachinesViewModel vm = new MachinesViewModel { enterprise = enterpriseId, facility = facilityId };

            List<Enterprise> enterprises = dal.GetEnterprises();
            enterprises.Insert(0, new Enterprise { Name = "Select an enterprise...", Id = null });
            vm.Enterprises = enterprises;
            if (enterpriseId != 0)
            {
                List<Facility> facilities = dal.GetFacilities(enterpriseId);
                facilities.Insert(0, new Facility { Name = "Select a facility...", Id = null });
                vm.Facilities = facilities;
            }
            if (enterpriseId != 0 && facilityId != 0)
            {
                List<Machine> facilityMachines = dal.GetFacilityMachines(facilityId);
                vm.Machines = new Dictionary<int, List<Machine>> { { -1, facilityMachines } };
            }
            else if (enterpriseId != 0)
            {
                Dictionary<int, List<Machine>> enterprisesMachines = dal.GetEnterpriseMachines(enterpriseId);
                vm.Machines = enterprisesMachines;
            }

            return View(vm);
        }
        /// <summary>
        /// Affiche la page avec la liste des ouvrages et des machines de l'entreprise spécifiée
        /// </summary>
        /// <param name="enterprise">L'entreprise</param>
       
        public ActionResult Enterprises(int enterprise)
        {
           /* List<Enterprise> enterprises = dal.GetEnterprises();
            Dictionary<string, string> cmxEnterprises = new Dictionary<string, string>();
            cmxEnterprises.Add("Select an enterprise...", "");
            foreach (Enterprise enterprise in enterprises)
            {
                cmxEnterprises.Add(enterprise.Name, "" + enterprise.Id);
            }

            List<Facility> facilities = dal.GetFacilities(enterprise);
            Dictionary<string, string> cmxFacilities = new Dictionary<string, string>();
            cmxFacilities.Add("Select an enterprise...", "");
            foreach (Facility facility in facilities)
            {
                cmxFacilities.Add(facility.Name, "" + facility.Id);
            }
            Dictionary<int, List<Machine>> machines = dal.GetEnterpriseMachines(enterprise);*/
            
            return View();
        }
    }
}