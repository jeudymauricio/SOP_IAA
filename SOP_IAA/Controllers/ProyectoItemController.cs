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
    public partial class ProyectoItemController : Controller
    {

        //public ActionResult Index(/*int? _id,*/int? _id)
        //{

        //    proyecto proyecto = db.proyecto.Find(_id);
        //   // var item = db.proyectoItem.Include(pr => pr.item).Where(pr => pr.idProyecto == proyecto.id);
        //   // var item = db.item.Include(i => i.contratoItem).Where(i => i.codigoItem == item.)
            
        //    return View(proyecto);

        //}

        // POST: ProyectoItem/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idProyecto,idItem,fechaInicio,fechaFin,costoEstimado")] proyectoItem proyectoItem)
        {
            if (ModelState.IsValid)
            {
                db.proyectoItem.Add(proyectoItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", proyectoItem.idItem);
            ViewBag.idProyecto = new SelectList(db.proyecto, "id", "nombre", proyectoItem.idProyecto);
            return View(proyectoItem);
        }


        // POST: ProyectoItem/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idProyecto,idItem,fechaInicio,fechaFin,costoEstimado")] proyectoItem proyectoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyectoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", proyectoItem.idItem);
            ViewBag.idProyecto = new SelectList(db.proyecto, "id", "nombre", proyectoItem.idProyecto);
            return View(proyectoItem);
        }

        

        // POST: ProyectoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proyectoItem proyectoItem = db.proyectoItem.Find(id);
            db.proyectoItem.Remove(proyectoItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
