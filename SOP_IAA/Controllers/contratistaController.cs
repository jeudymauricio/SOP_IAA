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
    public partial class contratistaController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: contratista
        public ActionResult Index()
        {
            return View(db.contratista.ToList());
        }

        // GET: contratista/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratista contratista = db.contratista.Find(id);
            if (contratista == null)
            {
                return HttpNotFound();
            }
            return View(contratista);
        }

        // GET: contratista/Create
        public ActionResult Create()
        {
            return View();
        }

        /*
        // POST: contratista/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion")] contratista contratista)
        {
            if (ModelState.IsValid)
            {
                db.contratista.Add(contratista);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contratista);
        }*/

        // GET: contratista/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratista contratista = db.contratista.Find(id);
            if (contratista == null)
            {
                return HttpNotFound();
            }
            return View(contratista);
        }

        /*
        // POST: contratista/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion")] contratista contratista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contratista).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contratista);
        }*/

        // GET: contratista/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratista contratista = db.contratista.Find(id);
            if (contratista == null)
            {
                return HttpNotFound();
            }
            return View(contratista);
        }

        /*
        // POST: contratista/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contratista contratista = db.contratista.Find(id);
            db.contratista.Remove(contratista);
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
