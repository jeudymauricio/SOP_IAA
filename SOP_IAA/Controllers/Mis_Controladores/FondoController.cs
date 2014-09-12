using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;
using SOP_IAA_Utilerias;

namespace SOP_IAA.Controllers
{
    public partial class FondoController : Controller
    {

        // POST: fondo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] fondo fondo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<fondo> rep = new Repositorio<fondo>();
                    rep.Insertar(fondo);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_FONDO_AGREGAR") + ex.Message;
                }
            }

            return View(fondo);
        }

       // POST: fondo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] fondo fondo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<fondo> rep = new Repositorio<fondo>();
                    rep.Actualizar(fondo);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_PROYECTO_ACTUALIZAR") + ex.Message;
                }
            }
            return View(fondo);
        }

        // POST: fondo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            fondo fondo = db.fondo.Find(id);
            db.fondo.Remove(fondo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
