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
    public partial class ItemController : Controller
    {
        
        // POST: Item/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigoItem,descripcion,unidadMedida")] item item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.item.Add(item);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View(item);
                }
            }

            return View(item);
        }

        // POST: Item/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigoItem,descripcion,unidadMedida")] item item)
        {
            if (ModelState.IsValid)
            {
                Repositorio<item> rep = new Repositorio<item>();
                rep.Actualizar(item);
                /*db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();*/
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // POST: Item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            item item = db.item.Find(id);
            try
            {
                foreach (var ci in item.contratoItem)
                {
                    db.boletaItem.RemoveRange(ci.boletaItem);
                    db.itemReajuste.RemoveRange(ci.itemReajuste);
                    db.oMCI.RemoveRange(ci.oMCI);
                    db.pICI.RemoveRange(ci.pICI);
                    //db.subproyectoContratoItem.RemoveRange(ci.subproyectoContratoItem);
                }

                db.contratoItem.RemoveRange(item.contratoItem);
                db.item.Remove(item);
                db.SaveChanges();
            }
            catch (Exception)
            {
                // Notify error
            }

            return RedirectToAction("Index");
        }

    }
}
