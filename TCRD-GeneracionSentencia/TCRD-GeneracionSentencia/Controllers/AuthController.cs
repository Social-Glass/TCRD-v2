using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TCRD_GeneracionSentencia.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var usuarios = Helpper.AuthHelpper.GetUsuarios();

                var usuario = usuarios.Find(x => x.email.Equals(email.Trim()));

                if (BCrypt.Net.BCrypt.Verify(password, usuario.password))
                {
                    Session["usuario"] = usuario.usuario;
                    Session["email"] = usuario.email;
                    Session["nombre"] = usuario.nombre;

                    return RedirectToAction("Dashboard", "Home");
                }   
                else
                {
                    return View();
                }
            }
            else
              {
                return View();
            }

            
        }

         
    }


}
