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
    public partial class ProgramaController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Programa
        public ActionResult Index()
        {
            var programa = db.programa.Include(p => p.Contrato);
            return View(programa.ToList());
        }

        // GET: Programa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        // Acción que despliega la lista de programas de un contrato específico
        public static int id;
        public ActionResult MisProgramas(int? _id)
        {
            if (_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Se busca el contrato específico
            Contrato contrato = db.Contrato.Find(_id);
            id = _id.Value;
            ViewBag.id = _id;
            return View(contrato);
        }

        // GET: Programa/Create
        public ActionResult Create()
        {
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            return View();
        }

        // POST: Programa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ano,trimestre")] programa programa)
        {
            if (ModelState.IsValid)
            {
                db.programa.Add(programa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            return View(programa);
        }

        // GET: Programa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            return View(programa);
        }

        // POST: Programa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,ano,trimestre")] programa programa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            return View(programa);
        }

        // GET: Programa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        // POST: Programa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            programa programa = db.programa.Find(id);
            db.programa.Remove(programa);
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
