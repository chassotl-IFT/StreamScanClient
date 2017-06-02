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
        /// Supprime l'entreprise poss�dant l'ID sp�cifi�
        /// </summary>
        /// <param name="id">L'ID de l'entreprise � supprimer</param>
        public ActionResult Delete(int id)
        {

            return View();
        }

        /// <summary>
        /// Liste des entreprises
        /// </summary>
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// Page d'ajout d'une entreprise
        /// </summary>
        public ActionResult New()
        {

            return View();
        }
        
        /// <summary>
        /// Ajoute l'entreprise sp�cifi�e
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public ActionResult New(Enterprise enterprise)
        {

            return View();
        }
        
        /// <summary>
        /// Affiche la page de mise � jour
        /// </summary>
        /// <param name="id">L'ID de l'entreprise</param>
        public ActionResult Update(int id)
        {

            return null;
        }
        
        /// <summary>
        /// Met � jour l'entreprise sp�cifi�e
        /// </summary>
        /// <param name="enterprise">L'entreprise</param>
        public ActionResult Update(Enterprise enterprise)
        {

            return null;
        }
    }
}