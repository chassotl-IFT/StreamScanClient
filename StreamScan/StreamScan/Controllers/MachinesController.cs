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
    /// <summary>
    /// Controller de la page "StreamX Machines"
    /// </summary>
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
        /// Les paramètres sont nullables car ils ne sont pas obligatoires à l'affichage de la page
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        /// <param name="facility">L'ID de l'ouvrage</param>
        /// <returns></returns>
        public ActionResult Index(int? enterprise, int? facility)
        {
            int facilityId = facility.GetValueOrDefault();
            int enterpriseId = enterprise.GetValueOrDefault();

            MachinesViewModel vm = new MachinesViewModel { enterprise = enterpriseId, facility = facilityId };

            List<Enterprise> enterprises = dal.GetEnterprises();
            enterprises.Insert(0, new Enterprise { Name = "Select an enterprise...", Id = 0 });
            vm.Enterprises = enterprises;
            if (enterpriseId != 0)
            {
                List<Facility> facilities = dal.GetFacilities(enterpriseId);
                facilities.Insert(0, new Facility { Name = "Select a facility...", Id = 0 });
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
    }
}