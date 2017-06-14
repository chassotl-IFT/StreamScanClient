using StreamScan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StreamScan.Controllers
{
    public class AccountController : Controller
    {

        private IDal dal;

        public AccountController()
        {
            dal = new Dal();
        }

        /// <summary>
        /// Affiche le formulaire de connexion
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Connecte l'utilisateur
        /// </summary>
        /// <param name="login">Object représentant les identifiants de l'utilisateur</param>
        /// <param name="returnUrl">L'URL depuis lequel l'utilisateur a été redirigé</param>
        [HttpPost]
        public ActionResult Index(Login login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string name = dal.Login(login);
                if (name != null)
                {
                    //Connecte l'utilisateur
                    FormsAuthentication.SetAuthCookie(name, false);
                    TempData["Message"] = "You are now connected !";
                    //Contrôle s'il faut rediriger vers le "returnUrl"
                    if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return Redirect("~/");
                }
                @TempData["Error"] = "Wrong username or password";
            }
            return View(login);
        }

        /// <summary>
        /// Déconnecte l'utilisateur
        /// </summary>
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }
    }
}