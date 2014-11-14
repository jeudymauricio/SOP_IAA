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
    public class OrdenModificacionController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: OrdenModificacion
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            //var ordenModificacion = db.ordenModificacion.Include(o => o.Contrato);
            //var ordenModificacion = contrato.ordenModificacion;

            return View(contrato/*ordenModificacion.ToList()*/);
        }

        // GET: OrdenModificacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.idContrato = idContrato;
            ordenModificacion om = new ordenModificacion();
            
            om.idContrato = idContrato.Value;
            om.fecha = DateTime.Now;
            
            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            return View(om);
        }

        // POST: OrdenModificacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM")] ordenModificacion ordenModificacion)
        {
            if (ModelState.IsValid)
            {
                db.ordenModificacion.Add(ordenModificacion);
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = ordenModificacion.idContrato });
            }

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            ViewBag.idContrato = ordenModificacion.idContrato;
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM")] ordenModificacion ordenModificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordenModificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            db.ordenModificacion.Remove(ordenModificacion);
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
