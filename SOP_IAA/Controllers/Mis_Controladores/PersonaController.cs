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
    public partial class PersonaController : Controller
    {
        // POST: persona/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,apellido1,apellido2,cedula")] persona persona)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<persona> rep = new Repositorio<persona>();
                    rep.Insertar(persona);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_PERSONA_AGREGAR") + ex.Message;
                }
            }

            ViewBag.id = new SelectList(db.ingeniero, "idPersona", "descripcion", persona.id);
            ViewBag.id = new SelectList(db.inspector, "idPersona", "idPersona", persona.id);
            ViewBag.id = new SelectList(db.usuario, "idPersona", "nombreUsuario", persona.id);
            return View(persona);
        }

        // POST: persona/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,apellido1,apellido2,cedula")] persona persona)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<persona> rep = new Repositorio<persona>();
                    rep.Actualizar(persona);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_PERSONA_ACTUALIZAR") + ex.Message;
                }
            }
            ViewBag.id = new SelectList(db.ingeniero, "idPersona", "descripcion", persona.id);
            ViewBag.id = new SelectList(db.inspector, "idPersona", "idPersona", persona.id);
            ViewBag.id = new SelectList(db.usuario, "idPersona", "nombreUsuario", persona.id);
            return View(persona);
        }

        // POST: persona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            persona persona = db.persona.Find(id);
            db.persona.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
