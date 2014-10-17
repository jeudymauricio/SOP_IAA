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
    public partial class IngenieroController : Controller
    {
        // GET: Ingeniero/Create
        public ActionResult Create()
        {
            var personas = db.persona
                  .Select(persona => new SelectListItem
                  {
                      Value = persona.id.ToString(),
                      Text = persona.nombre + " " + persona.apellido1 + " " + persona.apellido2
                  });
            var ingenieros = db.ingeniero.Select(i => new SelectListItem
            {
                Value = i.idPersona.ToString(),
                Text = i.persona.nombre + " " + i.persona.apellido1 + " " + i.persona.apellido2
            });

            ViewBag.idPersona = new SelectList(personas.Except(ingenieros), "Value", "Text");
            return View();
        }

        // POST: ingeniero/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona")] ingeniero ingeniero)
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

            var personas = db.persona
                  .Select(persona => new SelectListItem
                  {
                      Value = persona.id.ToString(),
                      Text = persona.nombre + " " + persona.apellido1 + " " + persona.apellido2
                  });
            var ingenieros = db.ingeniero.Select(i => new SelectListItem
            {
                Value = i.idPersona.ToString(),
                Text = i.persona.nombre + " " + i.persona.apellido1 + " " + i.persona.apellido2
            });

            ViewBag.idPersona = new SelectList(personas.Except(ingenieros), "Value", "Text");

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
