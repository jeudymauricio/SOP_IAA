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
    public partial class ProyectoController : Controller
    {

        // POST: Proyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,contratacion,lineaContrato,fechaInicio,plazo,lugar,idFondo")] Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<Proyecto> rep = new Repositorio<Proyecto>();
                    rep.Insertar(proyecto);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_PROYECTO_AGREGAR") + ex.Message;
                }

            }

            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", proyecto.idFondo);
            return View(proyecto);
        }

        // POST: Proyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,contratacion,lineaContrato,fechaInicio,plazo,lugar,idFondo")] Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<Proyecto> rep = new Repositorio<Proyecto>();
                    rep.Actualizar(proyecto);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_PROYECTO_ACTUALIZAR") + ex.Message;
                }
            }
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", proyecto.idFondo);
            return View(proyecto);
        }

        // POST: Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = db.Proyecto.Find(id);
            db.Proyecto.Remove(proyecto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
