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
    public class SubProyectoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: SubProyecto
        public ActionResult Index(int? idContrato)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subProyecto = db.subProyecto.Where(s=>s.idContrato == idContrato).Include(s => s.Contrato);
            ViewBag.idContrato = idContrato;

            return View(subProyecto.ToList());
        }

        // GET: SubProyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subProyecto subProyecto = db.subProyecto.Find(id);
            if (subProyecto == null)
            {
                return HttpNotFound();
            }
            return View(subProyecto);
        }

        // GET: SubProyecto/Create
        public ActionResult Create(int? idContrato)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(idContrato);
            if(contrato == null){
                return HttpNotFound();
            }

            subProyecto sp = new subProyecto();
            sp.idContrato = idContrato.Value;

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            //var itemGlobal = db.item.ToList();
            List<item> itemGlobal = new List<item>();

            foreach (var item in contrato.contratoItem)
            {
                itemGlobal.Add(item.item);
            }

            ViewBag.idItem = new SelectList(itemGlobal, "id", "codigoItem");

            return View(sp);
        }

        // POST: SubProyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContrato,nombre")] subProyecto subProyecto)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                db.subProyecto.Add(subProyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", subProyecto.idContrato);
            return View(subProyecto);
        }

        // GET: SubProyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subProyecto subProyecto = db.subProyecto.Find(id);
            if (subProyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", subProyecto.idContrato);
            return View(subProyecto);
        }

        // POST: SubProyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,nombre")] subProyecto subProyecto)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                db.Entry(subProyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", subProyecto.idContrato);
            return View(subProyecto);
        }

        // GET: SubProyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subProyecto subProyecto = db.subProyecto.Find(id);
            if (subProyecto == null)
            {
                return HttpNotFound();
            }
            return View(subProyecto);
        }

        // POST: SubProyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            subProyecto subProyecto = db.subProyecto.Find(id);
            db.subProyecto.Remove(subProyecto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private Boolean access()
        {
            if (Session["CurrentSession"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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
