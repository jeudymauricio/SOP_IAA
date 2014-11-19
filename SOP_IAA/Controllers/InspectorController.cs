using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;

namespace SOP_IAA.Controllers
{
    public class InspectorController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Inspector
        public ActionResult Index()
        {
            var inspector = db.inspector.Include(i => i.persona);
            return View(inspector.ToList());
        }

        // GET: Inspector/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inspector inspector = db.inspector.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            return View(inspector);
        }

        // GET: Inspector/Create
        public ActionResult Create()
        {
            var personas = db.persona
                  .Select(persona => new SelectListItem
                  {
                      Value = persona.id.ToString(),
                      Text = persona.nombre + " " + persona.apellido1 + " " + persona.apellido2
                  });
            var inspectores = db.inspector.Select(i => new SelectListItem
            {
                Value = i.idPersona.ToString(),
                Text = i.persona.nombre + " " + i.persona.apellido1 + " " + i.persona.apellido2
            });

            ViewBag.idPersona = new SelectList(personas.Except(inspectores), "Value", "Text");

            return View();
        }

        // POST: Inspector/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona")] inspector inspector)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.inspector.Add(inspector);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception){
                    ModelState.AddModelError("", "Error al crear el inspector, Verifique que la persona no sea ya un inspector");
                }
            }

            var personas = db.persona
                  .Select(persona => new SelectListItem
                  {
                      Value = persona.id.ToString(),
                      Text = persona.nombre + " " + persona.apellido1 + " " + persona.apellido2
                  });
            var inspectores = db.inspector.Select(i => new SelectListItem
            {
                Value = i.idPersona.ToString(),
                Text = i.persona.nombre + " " + i.persona.apellido1 + " " + i.persona.apellido2
            });

            ViewBag.idPersona = new SelectList(personas.Except(inspectores), "Value", "Text");

            return View(inspector);
        }

        // GET: Inspector/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inspector inspector = db.inspector.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", inspector.idPersona);
            return View(inspector);
        }

        // POST: Inspector/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona")] inspector inspector)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inspector).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", inspector.idPersona);
            return View(inspector);
        }

        // GET: Inspector/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inspector inspector = db.inspector.Find(id);
            if (inspector == null)
            {
                return HttpNotFound();
            }
            return View(inspector);
        }

        // POST: Inspector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inspector inspector = db.inspector.Find(id);
            try
            {
                // Se eliminan los items de cada una de las boletas del inpector
                foreach(var b in inspector.boleta){
                    db.boletaItem.RemoveRange(b.boletaItem);
                }
                
                // Se remueven todas las boletas asociadas al inspector
                db.boleta.RemoveRange(inspector.boleta);

                // Finalmente se elimina el inspector de la BD
                db.inspector.Remove(inspector);

                // Se guardan cambios en la BD
                db.SaveChanges();
            }
            catch (Exception)
            {
                // Notity error
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
