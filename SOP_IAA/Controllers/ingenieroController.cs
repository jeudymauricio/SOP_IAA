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
    public partial class IngenieroController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: ingeniero
        public ActionResult Index()
        {
            var ingeniero = db.ingeniero.Include(i => i.persona);
            return View(ingeniero.ToList());
        }

        // GET: ingeniero/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingeniero ingeniero = db.ingeniero.Find(id);
            if (ingeniero == null)
            {
                return HttpNotFound();
            }
            return View(ingeniero);
        }

        // GET: ingeniero/Create
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
        /*
        // POST: ingeniero/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona,descripcion,departamento,rol")] ingeniero ingeniero)
        {
            if (ModelState.IsValid)
            {
                db.ingeniero.Add(ingeniero);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }*/

        // GET: ingeniero/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingeniero ingeniero = db.ingeniero.Find(id);
            if (ingeniero == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }
        /*
        // POST: ingeniero/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona,descripcion,departamento,rol")] ingeniero ingeniero)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingeniero).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }*/

        // GET: ingeniero/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingeniero ingeniero = db.ingeniero.Find(id);
            if (ingeniero == null)
            {
                return HttpNotFound();
            }
            return View(ingeniero);
        }
        /*
        // POST: ingeniero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ingeniero ingeniero = db.ingeniero.Find(id);
            db.ingeniero.Remove(ingeniero);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

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
