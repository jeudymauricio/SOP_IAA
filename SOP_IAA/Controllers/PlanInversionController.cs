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
    public class PlanInversionController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: PlanInversion
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            ViewBag.contratacion = contrato.licitacion;
            ViewBag.idContrato = contrato.id;

            // Se seleccionan solo los Planes de inversión de un contrato específico
            //var planInversion = db.planInversion.Where(p => p.idContrato == idContrato.Value).Include(p => p.Contrato);

            return View(contrato.planInversion.ToList());
        }

        // GET: PlanInversion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            return View(planInversion);
        }

        // GET: PlanInversion/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            planInversion pi = new planInversion();
            pi.idContrato = idContrato.Value;
            pi.fecha = DateTime.Now.AddMonths(1);
            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "Nombre");
            ViewBag.idItem = new SelectList(contrato.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
            return View(pi);
        }

        // POST: PlanInversion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContrato,fecha,mes,ano")] planInversion planInversion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.planInversion.Add(planInversion);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idContrato = planInversion.idContrato });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("","Ocurrió un error al ingresar el plan, verifique que no existe un plan para esa fecha");
                    return View(planInversion);
                }
            }

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            return View(planInversion);
        }

        // GET: PlanInversion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            return View(planInversion);
        }

        // POST: PlanInversion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,fecha,mes,ano")] planInversion planInversion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planInversion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = planInversion.idContrato });
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            return View(planInversion);
        }

        // GET: PlanInversion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            return View(planInversion);
        }

        // POST: PlanInversion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            planInversion planInversion = db.planInversion.Find(id);
            db.planInversion.Remove(planInversion);
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
