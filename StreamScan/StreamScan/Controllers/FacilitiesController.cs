using QueriesManager.Bean;
using StreamScan.Models;
using StreamScan.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.Controllers
{

    /// <summary>
    /// Controller de la page "Facilities"
    /// </summary>
    [Authorize]
    public class FacilitiesController : Controller
    {

        private IDal dal;

        public FacilitiesController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Liste les ouvrages de l'entreptise spécifiée
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        public ActionResult Index(int enterprise)
        {
            List<Facility> facilities = dal.GetFacilities(enterprise);
            Enterprise enterpriseObj = null;
            try
            {
                enterpriseObj = dal.GetEnterprise(enterprise);
            }
            catch (ArgumentOutOfRangeException)
            {
                TempData["Error"] = "The enterprise you request doesn't exist anymore.";
                return Redirect("/Enterprises/Index");
            }
            return View(new FacilitiesViewModel { Enterprise = enterpriseObj, Facilities = facilities });
        }

        /// <summary>
        /// Affiche la page d'ajout d'un ouvrage
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise à laquelle on ajoute l'ouvrage</param>
        public ActionResult New(int enterprise)
        {
            return View(new NewFacilityViewModel { Enterprise = enterprise });
        }

        /// <summary>
        /// Ajoute l'ouvrage spécifié
        /// </summary>
        /// <param name="facility">L'ouvrage à ajouter</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult New(NewFacilityViewModel model)
        {
            model.Facility.Fk_Enterprise = model.Enterprise;
            if (ModelState.IsValid)
            {
                MySqlReturn sqlR = dal.InsertFacility(model.Facility);
                if (sqlR.ErrorMessage != "")
                {
                    if (sqlR.ErrorMessage.Contains("Cannot add or update a child row"))
                        TempData["Error"] = "Can't add the machine : the enterprise or the facility in which the machine should be inserted has been deleted.";
                    else
                    {
                        TempData["Exception_Message"] = sqlR.ErrorMessage;
                        return Redirect("/Error");
                    }
                }
                if (sqlR.IsOk)
                {
                    TempData["Message"] = "The facility has been added successfully";
                    return Redirect("/Facilities/Index/" + model.Enterprise);
                }
                else
                {
                    TempData["Error"] = "An error occured during the adding. If this error still coming please check that the enterprise in which you are trying to insert the facility still exists.";
                }
            }
            return View(model);
        }

        /// <summary>
        /// Affiche la page de mise à jour
        /// </summary>
        /// <param name="facility">L'ID de l'ouvrage à mettre à jour</param>
        public ActionResult Update(int facility)
        {
            Facility model = dal.GetFacility(facility);
            return View(model);
        }

        /// <summary>
        /// Met à jour l'ouvrage spécifié
        /// </summary>
        /// <param name="model">Les infos de l'ouvrage à mettre à jour</param>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        /// <param name="facility">L'ID de l'ouvrage</param>
        [HttpPost]
        public ActionResult Update(Facility model, int enterprise, int facility)
        {
            if (ModelState.IsValid)
            {
                model.Id = facility;
                model.Fk_Enterprise = enterprise;
                MySqlReturn sqlR = dal.UpdateFacility(model);
                if (sqlR.ErrorMessage != "")
                {
                    TempData["Exception_Message"] = sqlR.ErrorMessage;
                    return Redirect("/Error");
                }
                if (sqlR.IsOk)
                {
                    TempData["Message"] = "The facility has been updated successfully";
                    return Redirect("/Facilities/Index/" + enterprise);
                }
                else
                {
                    TempData["Error"] = "An error occured during the update. If this error still coming please go back to the facilities list and re-update this facility.";
                }
            }
            return View(model);
        }

        /// <summary>
        /// Supprime l'ouvrage possédant l'ID spécifié
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise utilisé pour la redirection</param>
        /// <param name="facility">L'ID de l'ouvrage à supprimer</param>
        [HttpPost]
        public ActionResult Delete(int enterprise, int facility)
        {
            MySqlReturn sqlR = dal.DeleteFacility(facility);
            if (sqlR.ErrorMessage != "")
            {
                TempData["Exception_Message"] = sqlR.ErrorMessage;
                return Redirect("/Error");
            }
            if (sqlR.IsOk)
            {
                TempData["Message"] = "The facility has been deleted successfully";
                return Redirect("/Facilities/Index/" + enterprise);
            }
            else
            {
                TempData["Error"] = "An error occured during the deletion";
                return Redirect("/Facilities/Index/" + enterprise);
            }
        }

    }

}