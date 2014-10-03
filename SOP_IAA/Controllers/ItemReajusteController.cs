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
    public partial class ItemReajusteController :  Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: itemReajustes
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ir = db.itemReajuste.Where(p => p.contratoItem.idContrato == idContrato);//.GroupBy(m => new { m.mes, m.ano });
            var v = ir.GetType();
            return View(ir);
        }

        // GET: itemReajustes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itemReajuste itemReajuste = db.itemReajuste.Find(id);
            if (itemReajuste == null)
            {
                return HttpNotFound();
            }
            return View(itemReajuste);
        }

        // GET: itemReajustes/Create
        public ActionResult Create()
        {
            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id");
            return View();
        }

        // GET: itemReajustes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itemReajuste itemReajuste = db.itemReajuste.Find(id);
            if (itemReajuste == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id", itemReajuste.idContratoItem);
            return View(itemReajuste);
        }

        // GET: itemReajustes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            itemReajuste itemReajuste = db.itemReajuste.Find(id);
            if (itemReajuste == null)
            {
                return HttpNotFound();
            }
            return View(itemReajuste);
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
