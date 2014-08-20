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
    public partial class ProyectoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Proyecto
        public ActionResult Index()
        {
            var proyecto = db.Proyecto.Include(p => p.fondo);
            return View(proyecto.ToList());
        }

        // GET: Proyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // GET: Proyecto/Create
        public ActionResult Create()
        {
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre");
            return View();
        }

        // POST: Proyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,contratacion,lineaContrato,fechaInicio,plazo,lugar,idFondo")] Proyecto proyecto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Proyecto.Add(proyecto);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", proyecto.idFondo);
        //    return View(proyecto);
        //}

        // GET: Proyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", proyecto.idFondo);
            return View(proyecto);
        }

        // POST: Proyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,contratacion,lineaContrato,fechaInicio,plazo,lugar,idFondo")] Proyecto proyecto)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(proyecto).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", proyecto.idFondo);
        //    return View(proyecto);
        //}

        // GET: Proyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // POST: Proyecto/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Proyecto proyecto = db.Proyecto.Find(id);
        //    db.Proyecto.Remove(proyecto);
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
