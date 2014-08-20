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
    public partial class FondoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Fondo
        public ActionResult Index()
        {
            return View(db.fondo.ToList());
        }

        // GET: Fondo/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fondo fondo = db.fondo.Find(id);
            if (fondo == null)
            {
                return HttpNotFound();
            }
            return View(fondo);
        }

        // GET: Fondo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fondo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,nombre")] fondo fondo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.fondo.Add(fondo);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(fondo);
        //}

        // GET: Fondo/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fondo fondo = db.fondo.Find(id);
            if (fondo == null)
            {
                return HttpNotFound();
            }
            return View(fondo);
        }

        // POST: Fondo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,nombre")] fondo fondo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(fondo).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(fondo);
        //}

        // GET: Fondo/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            fondo fondo = db.fondo.Find(id);
            if (fondo == null)
            {
                return HttpNotFound();
            }
            return View(fondo);
        }

        // POST: Fondo/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(short id)
        //{
        //    fondo fondo = db.fondo.Find(id);
        //    db.fondo.Remove(fondo);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
