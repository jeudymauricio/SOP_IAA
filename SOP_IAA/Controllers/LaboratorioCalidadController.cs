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
    public partial class LaboratorioCalidadController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: LaboratorioCalidad
        public ActionResult Index()
        {
            return View(db.laboratorioCalidad.ToList());
        }

        // GET: LaboratorioCalidad/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            laboratorioCalidad laboratorioCalidad = db.laboratorioCalidad.Find(id);
            if (laboratorioCalidad == null)
            {
                return HttpNotFound();
            }
            return View(laboratorioCalidad);
        }

        // GET: LaboratorioCalidad/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LaboratorioCalidad/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,nombre,tipo")] laboratorioCalidad laboratorioCalidad)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.laboratorioCalidad.Add(laboratorioCalidad);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(laboratorioCalidad);
        //}

        // GET: LaboratorioCalidad/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            laboratorioCalidad laboratorioCalidad = db.laboratorioCalidad.Find(id);
            if (laboratorioCalidad == null)
            {
                return HttpNotFound();
            }
            return View(laboratorioCalidad);
        }

        // POST: LaboratorioCalidad/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,nombre,tipo")] laboratorioCalidad laboratorioCalidad)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(laboratorioCalidad).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(laboratorioCalidad);
        //}

        // GET: LaboratorioCalidad/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            laboratorioCalidad laboratorioCalidad = db.laboratorioCalidad.Find(id);
            if (laboratorioCalidad == null)
            {
                return HttpNotFound();
            }
            return View(laboratorioCalidad);
        }

        // POST: LaboratorioCalidad/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(short id)
        //{
        //    laboratorioCalidad laboratorioCalidad = db.laboratorioCalidad.Find(id);
        //    db.laboratorioCalidad.Remove(laboratorioCalidad);
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
