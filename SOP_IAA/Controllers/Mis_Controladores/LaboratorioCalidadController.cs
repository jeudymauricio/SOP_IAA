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
    public partial class LaboratorioCalidadController : Controller
    {
        
        // POST: LaboratorioCalidad/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,tipo")] laboratorioCalidad laboratorioCalidad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<laboratorioCalidad> rep = new Repositorio<laboratorioCalidad>();
                    rep.Insertar(laboratorioCalidad);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_LABORATORIO_AGREGAR") + ex.Message;
                }
            }

            return View(laboratorioCalidad);
        }

        // POST: LaboratorioCalidad/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,tipo")] laboratorioCalidad laboratorioCalidad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<laboratorioCalidad> rep = new Repositorio<laboratorioCalidad>();
                    rep.Actualizar(laboratorioCalidad);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_LABORATORIO_ACTUALIZAR") + ex.Message;
                }
            }
            return View(laboratorioCalidad);
        }

        // POST: LaboratorioCalidad/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            laboratorioCalidad laboratorioCalidad = db.laboratorioCalidad.Find(id);
            db.laboratorioCalidad.Remove(laboratorioCalidad);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
