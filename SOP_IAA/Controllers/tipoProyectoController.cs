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
    public class TipoProyectoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: tipoProyecto
        public ActionResult Index()
        {
            return View(db.tipoProyecto.ToList());
        }

        // GET: tipoProyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoProyecto tipoProyecto = db.tipoProyecto.Find(id);
            if (tipoProyecto == null)
            {
                return HttpNotFound();
            }
            return View(tipoProyecto);
        }

        // GET: tipoProyecto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipoProyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] tipoProyecto tipoProyecto)
        {
            if (ModelState.IsValid)
            {
                db.tipoProyecto.Add(tipoProyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoProyecto);
        }

        // GET: tipoProyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoProyecto tipoProyecto = db.tipoProyecto.Find(id);
            if (tipoProyecto == null)
            {
                return HttpNotFound();
            }
            return View(tipoProyecto);
        }

        // POST: tipoProyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] tipoProyecto tipoProyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoProyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoProyecto);
        }

        // GET: tipoProyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoProyecto tipoProyecto = db.tipoProyecto.Find(id);
            if (tipoProyecto == null)
            {
                return HttpNotFound();
            }
            return View(tipoProyecto);
        }

        // POST: tipoProyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipoProyecto tipoProyecto = db.tipoProyecto.Find(id);
            db.tipoProyecto.Remove(tipoProyecto);
            db.SaveChanges();
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
