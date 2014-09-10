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
    public partial class ContratoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Contrato
        public ActionResult Index()
        {
            var contrato = db.Contrato.Include(c => c.contratista).Include(c => c.fondo).Include(c => c.zona);
            return View(contrato.ToList());
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrato contrato = db.Contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }
        /*
        // GET: Contrato/Create
        public ActionResult Create()
        {
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre");
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre");
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre");
            return View();
        }
        
        // POST: Contrato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContratista,licitacion,lineaContrato,idZona,fechaInicio,plazo,lugar,idFondo")] Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                db.Contrato.Add(contrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            return View(contrato);
        }*/
        /*
        // GET: Contrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrato contrato = db.Contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            return View(contrato);
        }*/
        /*
        // POST: Contrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContratista,licitacion,lineaContrato,idZona,fechaInicio,plazo,lugar,idFondo")] Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contrato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            return View(contrato);
        }*/

        // GET: Contrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrato contrato = db.Contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            return View(contrato);
        }
        /*
        // POST: Contrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contrato contrato = db.Contrato.Find(id);
            db.Contrato.Remove(contrato);
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
