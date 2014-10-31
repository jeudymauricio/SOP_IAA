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
    public class SeccionControlController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: SeccionControl
        public ActionResult Index()
        {
            var seccionControl = db.seccionControl.Include(s => s.ruta);
            return View(seccionControl.ToList());
        }

        // GET: SeccionControl/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seccionControl seccionControl = db.seccionControl.Find(id);
            if (seccionControl == null)
            {
                return HttpNotFound();
            }
            return View(seccionControl);
        }

        // GET: SeccionControl/Create
        public ActionResult Create()
        {
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre");
            return View();
        }

        // POST: SeccionControl/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idRuta,seccion,descripcion")] seccionControl seccionControl)
        {
            if (ModelState.IsValid)
            {
                db.seccionControl.Add(seccionControl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", seccionControl.idRuta);
            return View(seccionControl);
        }

        // GET: SeccionControl/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seccionControl seccionControl = db.seccionControl.Find(id);
            if (seccionControl == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", seccionControl.idRuta);
            return View(seccionControl);
        }

        // POST: SeccionControl/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idRuta,seccion,descripcion")] seccionControl seccionControl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(seccionControl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", seccionControl.idRuta);
            return View(seccionControl);
        }

        // GET: SeccionControl/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            seccionControl seccionControl = db.seccionControl.Find(id);
            if (seccionControl == null)
            {
                return HttpNotFound();
            }
            return View(seccionControl);
        }

        // POST: SeccionControl/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            seccionControl seccionControl = db.seccionControl.Find(id);
            db.seccionControl.Remove(seccionControl);
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
