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
    public class IngenieroContratoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: ingenieroContrato
        public ActionResult Index()
        {
            var ingenieroContrato = db.ingenieroContrato.Include(i => i.Contrato).Include(i => i.ingeniero);
            return View(ingenieroContrato.ToList());
        }

        // GET: ingenieroContrato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingenieroContrato ingenieroContrato = db.ingenieroContrato.Find(id);
            if (ingenieroContrato == null)
            {
                return HttpNotFound();
            }
            return View(ingenieroContrato);
        }

        // GET: ingenieroContrato/Create
        public ActionResult Create()
        {
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            ViewBag.idIngeniero = new SelectList(db.ingeniero, "idPersona", "descripcion");
            return View();
        }

        // POST: ingenieroContrato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContrato,idIngeniero,fechaInicio,fechaFin")] ingenieroContrato ingenieroContrato)
        {
            if (ModelState.IsValid)
            {
                db.ingenieroContrato.Add(ingenieroContrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ingenieroContrato.idContrato);
            ViewBag.idIngeniero = new SelectList(db.ingeniero, "idPersona", "descripcion", ingenieroContrato.idIngeniero);
            return View(ingenieroContrato);
        }

        // GET: ingenieroContrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingenieroContrato ingenieroContrato = db.ingenieroContrato.Find(id);
            if (ingenieroContrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ingenieroContrato.idContrato);
            ViewBag.idIngeniero = new SelectList(db.ingeniero, "idPersona", "descripcion", ingenieroContrato.idIngeniero);
            return View(ingenieroContrato);
        }

        // POST: ingenieroContrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idContrato,idIngeniero,fechaInicio,fechaFin")] ingenieroContrato ingenieroContrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingenieroContrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ingenieroContrato.idContrato);
            ViewBag.idIngeniero = new SelectList(db.ingeniero, "idPersona", "descripcion", ingenieroContrato.idIngeniero);
            return View(ingenieroContrato);
        }

        // GET: ingenieroContrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingenieroContrato ingenieroContrato = db.ingenieroContrato.Find(id);
            if (ingenieroContrato == null)
            {
                return HttpNotFound();
            }
            return View(ingenieroContrato);
        }

        // POST: ingenieroContrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ingenieroContrato ingenieroContrato = db.ingenieroContrato.Find(id);
            db.ingenieroContrato.Remove(ingenieroContrato);
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
