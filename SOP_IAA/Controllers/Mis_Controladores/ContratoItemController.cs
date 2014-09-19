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
    public partial class ContratoItemController : Controller
    {
        // GET: ContratoItem
        public ActionResult Index(int? idContrato)
        {/*
            var contratoItem = db.contratoItem.Include(c => c.Contrato).Include(c => c.item);
            return View(contratoItem.ToList());*/
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(idContrato);
            return View(contrato);
        }

        // GET: ContratoItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);
            if (contratoItem == null)
            {
                return HttpNotFound();
            }
            return View(contratoItem);
        }

        // GET: ContratoItem/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            contratoItem contrItem = new contratoItem
            {
                idContrato = idContrato.Value
            };


            var itemGlobal = db.item.ToList();
            List<item> itemContrato = new List<item>();

            foreach (var item in db.Contrato.Find(idContrato).contratoItem)
            {
                itemContrato.Add(item.item);
            }

            ViewBag.idItem = new SelectList(itemGlobal.Except(itemContrato), "id", "codigoItem");

            //ViewBag.idItem = new SelectList(db.item, "id", "codigoItem");
            /*ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");*/
            return View(contrItem);
        }

        // POST: ContratoItem/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContrato,idItem,precioUnitario")] contratoItem contratoItem)
        {
            if (ModelState.IsValid)
            {
                db.contratoItem.Add(contratoItem);
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = contratoItem.idContrato });
            }

            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", contratoItem.idContrato);
            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", contratoItem.idItem);
            return View(contratoItem);
        }

        // GET: ContratoItem/Edit/5
        public ActionResult Edit(int? idContrato, int? idItem)
        {
            if ((idContrato == null) || (idItem == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(idContrato, idItem);
            if (contratoItem == null)
            {
                return HttpNotFound();
            }
            /*ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", contratoItem.idContrato);
            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", contratoItem.idItem);*/
            return View(contratoItem);
        }

        // POST: ContratoItem/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idContrato,idItem,precioUnitario")] contratoItem contratoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contratoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = contratoItem.idContrato, idItem = contratoItem.idItem });
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", contratoItem.idContrato);
            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", contratoItem.idItem);
            return View(contratoItem);
        }

        // GET: ContratoItem/Delete/5
        public ActionResult Delete(int? idContrato, int? idItem)
        {
            if ((idContrato == null) || (idItem == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(idContrato, idItem);
            if (contratoItem == null)
            {
                return HttpNotFound();
            }
            return View(contratoItem);
        }

        // POST: ContratoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int idContrato, int idItem)
        {
            contratoItem contratoItem = db.contratoItem.Find(idContrato, idItem);
            db.contratoItem.Remove(contratoItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
