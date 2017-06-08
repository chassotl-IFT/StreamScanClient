using QueriesManager.Bean;
using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StreamScan.Controllers
{
    [Authorize]
    public class EnterprisesController : Controller
    {
        private IDal dal;

        public EnterprisesController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Liste les entreprises
        /// </summary>
        public ActionResult Index()
        {
            List<Enterprise> enterprises = dal.GetEnterprises();

            return View(enterprises);
        }

        /// <summary>
        /// Affiche la page d'ajout d'une entreprise
        /// </summary>
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Ajoute l'entreprise spécifiée
        /// </summary>
        /// <param name="enterprise">L'entreprise à ajouter</param>
        [HttpPost]
        public ActionResult New(Enterprise model)
        {
            if (ModelState.IsValid)
            {
                MySqlReturn sqlR = dal.InsertEnterprise(model);
                if (sqlR.ErrorMessage != "")
                {
                    TempData["Exception_Message"] = sqlR.ErrorMessage;
                    return Redirect("/Error");
                }
                if (sqlR.IsOk)
                {
                    TempData["Message"] = "The enterprise has been added successfully";
                    return Redirect("/Enterprises");
                }
                else
                {
                    TempData["Error"] = "An error occured during the adding";
                }
            }
            return View(model);
        }

        /// <summary>
        /// Affiche la page de mise à jour
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise</param>
        public ActionResult Update(int enterprise)
        {
            Enterprise model = dal.GetEnterprise(enterprise);
            return View(model);
        }

        /// <summary>
        /// Met à jour l'entreprise spécifiée
        /// </summary>
        /// <param name="enterprise">L'entreprise</param>
        [HttpPost]
        public ActionResult Update(int enterprise, Enterprise model)
        {
            model.Id = enterprise;
            if (ModelState.IsValid)
            {
                MySqlReturn sqlR = dal.UpdateEnterprise(model);
                if (sqlR.ErrorMessage != "")
                {
                    TempData["Exception_Message"] = sqlR.ErrorMessage;
                    return Redirect("/Error");
                }
                if (sqlR.IsOk)
                {
                    TempData["Message"] = "The enterprise has been updated successfully";
                    return Redirect("/Enterprises");
                }
                else
                {
                    TempData["Error"] = "An error occured during the update";
                }
            }
            return View(model);
        }

        /// <summary>
        /// Supprime l'entreprise possédant l'ID spécifié
        /// </summary>
        /// <param name="enterprise">L'ID de l'entreprise à supprimer</param>
        [HttpPost]
        public ActionResult Delete(int enterprise)
        {
            MySqlReturn sqlR = dal.DeleteEnterprise(enterprise);
            if (sqlR.ErrorMessage != "")
            {
                TempData["Exception_Message"] = sqlR.ErrorMessage;
                return Redirect("/Error");
            }
            if (sqlR.IsOk)
            {
                TempData["Message"] = "The enterprise has been deleted successfully";
                return Redirect("/Enterprises");
            }
            else
            {
                TempData["Error"] = "An error occured during the deletion";
                return Redirect("/Enterprises");
            }
        }
    }
}