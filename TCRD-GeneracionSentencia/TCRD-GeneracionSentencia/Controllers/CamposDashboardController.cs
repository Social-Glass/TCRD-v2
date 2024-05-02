using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Controllers
{
    public class CamposDashboardController : Controller
    {
        // GET: CamposDashboard
        public ActionResult Index()
        {
            var campos = Helpper.CamposDashboardHelpper.GetCampos();

            return View(campos);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Campos_Dashboard campo)
        {
            if (ModelState.IsValid)
            {

                return Json(new
                {
                    success = Helpper.CamposDashboardHelpper.CreateCampo(campo)

                });


            }
            else
            {
                return Json(false);
            }
        }

        public ActionResult Registros()
        {

            var registrosDashboard = Helpper.CamposDashboardHelpper.GetRegistros();

            return View(registrosDashboard);
        }

        public ActionResult CreateRegistro()
        {
             var campos = Helpper.CamposDashboardHelpper.GetCampos();
            return View(campos);
        }

        [HttpPost]
        public ActionResult CreateRegistro(List<RegistrosDashboard> registros)
        {
            var success = false;
            
            if (ModelState.IsValid)
            {

                foreach (var registro in registros)
                {
                    success = Helpper.CamposDashboardHelpper.CreateRegistro(registro);
                }

                return Json(success);

            }
            else
            {
                return Json(success);
            }
        }


    }
}
