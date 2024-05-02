using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Controllers
{
    public class DashboardController : Controller
    {
 
        [Authorize]
        public ActionResult Index()
        {
            var datos = Helpper.DashboardHelpper.GetDatos();

            return View(datos);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult InsertarDatos(Datos_Dashboard datos)
        {
            if (ModelState.IsValid)
            {

                return Json(new
                {
                    success = Helpper.DashboardHelpper.CreateDatos(datos)

                });


            }
            else
            {
                return Json(false);
            }
        }


    }
}
