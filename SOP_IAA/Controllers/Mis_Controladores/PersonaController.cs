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
            try
            {
                // Primero se eliminan sus relaciones como ingeniero
                // Se elimina el ingeniero de todos los contratos donde exista
                if (persona.ingeniero != null)
                {
                    db.ingenieroContrato.RemoveRange(persona.ingeniero.ingenieroContrato);

                    // Se elimina el ingeniero del sistema
                    db.ingeniero.Remove(persona.ingeniero);
                }

                // Segundo se eliminan sus relaciones como inspector
                // Se eliminan los items de cada una de las boletas del inpector
                if (persona.inspector != null)
                {
                    foreach (var b in persona.inspector.boleta)
                    {
                        db.boletaItem.RemoveRange(b.boletaItem);
                    }

                    // Se remueven todas las boletas asociadas al inspector
                    db.boleta.RemoveRange(persona.inspector.boleta);

                    // Finalmente se elimina el inspector de la BD
                    db.inspector.Remove(persona.inspector);
                }

                // Finalmente se elimina la persona de la BD
                db.persona.Remove(persona);

                db.SaveChanges();
            }
            catch (Exception e)
            {
                // Notify error
            }

            return RedirectToAction("Index");
        }
    }
}
