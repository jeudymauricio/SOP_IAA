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
    public partial class ItemReajusteController : Controller
    {
        //GET: itemReajuste/Periodo
        public ActionResult Periodo(int? mes, int? ano, int? idContrato)
        {
            if (idContrato == null || mes==null || ano==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ir = db.itemReajuste.Where(p => p.ano == ano).Where(i => i.mes==mes);
            return View(ir);
        }

        // POST: itemReajustes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContratoItem,fecha,mes,ano,reajuste,precioReajustado")] itemReajuste itemReajuste)
        {
            if (ModelState.IsValid)
            {
                db.itemReajuste.Add(itemReajuste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id", itemReajuste.idContratoItem);
            return View(itemReajuste);
        }

        // POST: itemReajustes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContratoItem,fecha,mes,ano,reajuste,precioReajustado")] itemReajuste itemReajuste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemReajuste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id", itemReajuste.idContratoItem);
            return View(itemReajuste);
        }

        // POST: itemReajustes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            itemReajuste itemReajuste = db.itemReajuste.Find(id);
            db.itemReajuste.Remove(itemReajuste);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
