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
    public partial class ingenieroController : Controller
    {

        // POST: ingeniero/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona,descripcion,departamento,rol")] ingeniero ingeniero)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<ingeniero> rep = new Repositorio<ingeniero>();
                    rep.Insertar(ingeniero);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Construir el string a guardar en la bitácora en caso de error.
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_INGENIERO_AGREGAR") + ex.Message;
                }
            }

            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }

        // POST: ingeniero/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona,descripcion,departamento,rol")] ingeniero ingeniero)
        {
            try
            {
                Repositorio<ingeniero> rep = new Repositorio<ingeniero>();
                rep.Actualizar(ingeniero);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //Construir el string a guardar en la bitácora en caso de error.
                ViewBag.Error = true;
                ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_INGENIERO_ACTUALIZAR") + ex.Message;
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }

        // POST: ingeniero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ingeniero ingeniero = db.ingeniero.Find(id);
            db.ingeniero.Remove(ingeniero);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
