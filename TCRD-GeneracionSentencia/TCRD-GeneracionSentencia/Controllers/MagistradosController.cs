using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCRD_GeneracionSentencia.Models;

namespace TCRD_GeneracionSentencia.Controllers
{
    public class MagistradosController : Controller
    {
        // GET: Magistrados
        public ActionResult Index()
        {
            var nombre = Session["nombre"] != null ? Session["nombre"].ToString() : "";

            if (!string.IsNullOrEmpty(nombre))
            {
                var magistrados = Helpper.MagistradoHelpper.GetMagistrados();

                return View(magistrados);
            }
            else return RedirectToAction("Login", "Auth");


          
        }

        [HttpPost]
        public ActionResult Index(Magistrado magistrado, string  accion)
        {

            bool result = false;
            switch (accion)
            {
                case "guardar":
                   result = Helpper.MagistradoHelpper.AddMagistrado(magistrado);
                    break;
                case "editar":
                    result = Helpper.MagistradoHelpper.UpdateMagistrado(magistrado);
                    break;
                case "eliminar":
                    result = Helpper.MagistradoHelpper.DeleteMagistrado(magistrado);
                    break;
            }

            if (result)
            {
               return Json(new
                {
                    success = true,
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    
                });
            }

        }

        // GET: Magistrados/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Magistrados/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Magistrados/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Magistrados/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Magistrados/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Magistrados/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
