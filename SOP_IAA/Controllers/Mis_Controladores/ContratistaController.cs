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
    public partial class contratistaController : Controller // El namespace no debe incluir .Mis_Controladores
    {
        
        // POST: contratista/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion")] contratista contratista)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<contratista> rep = new Repositorio<contratista>();
                    rep.Insertar(contratista);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_CONTRATISTA_AGREGAR") + ex.Message;
                }
            }

            return View(contratista);
        }

        // POST: contratista/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion")] contratista contratista)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<contratista> rep = new Repositorio<contratista>();
                    rep.Actualizar(contratista);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_CONTRATISTA_ACTUALIZAR") + ex.Message;
                }
            }
            return View(contratista);
        }

        // POST: contratista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contratista contratista = db.contratista.Find(id);
            db.contratista.Remove(contratista);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
