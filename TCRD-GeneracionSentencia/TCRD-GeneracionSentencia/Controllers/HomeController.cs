using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using TCRD_GeneracionSentencia.Models;



namespace TCRD_GeneracionSentencia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var nombre = Session["nombre"] != null ? Session["nombre"].ToString() : "";

            if (!string.IsNullOrEmpty(nombre))
            {
                return View();
            }
            else return RedirectToAction("Login", "Auth");


        }

        public ActionResult Dashboard()
        {

            var nombre = Session["nombre"] != null ? Session["nombre"].ToString() : "";

            if (!string.IsNullOrEmpty(nombre))
            {
                var datos = Helpper.CamposDashboardHelpper.GetRegistros();

                return View(datos);
            }
            else return RedirectToAction("Login", "Auth");

        }




    }
}