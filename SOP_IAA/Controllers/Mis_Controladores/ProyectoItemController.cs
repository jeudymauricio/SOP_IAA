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
        //private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: ProyectoItem
        public ActionResult Index(/*int? _id,*/int? _id)
        {
            if (_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            proyecto proyecto = db.proyecto.Find(_id);
            // var item = db.proyectoItem.Include(pr => pr.item).Where(pr => pr.idProyecto == proyecto.id);
            // var item = db.item.Include(i => i.contratoItem).Where(i => i.codigoItem == item.)

            return View(proyecto);

        }

       /* public ActionResult Index()
        {
            var proyectoItem = db.proyectoItem.Include(p => p.contratoItem).Include(p => p.proyecto);
            return View(proyectoItem.ToList());
        }*/

        // GET: ProyectoItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyectoItem proyectoItem = db.proyectoItem.Find(id);
            if (proyectoItem == null)
            {
                return HttpNotFound();
            }
            return View(proyectoItem);
        }


        // GET: ProyectoItem/Create
        public ActionResult Create()
        {
            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id");
            ViewBag.idProyecto = new SelectList(db.proyecto, "id", "nombre");
            return View();
        }

        // POST: ProyectoItem/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idProyecto,idContratoItem,fechaInicio,fechaFin,costoEstimado")] proyectoItem proyectoItem)
        {
            if (ModelState.IsValid)
            {
                db.proyectoItem.Add(proyectoItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idItem = new SelectList(db.item, "id", "id", proyectoItem.idContratoItem);
            ViewBag.idProyecto = new SelectList(db.proyecto, "id", "nombre", proyectoItem.idProyecto);
            return View(proyectoItem);
        }

 

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyectoItem proyectoItem = db.proyectoItem.Find(id);
            if (proyectoItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.idItem = new SelectList(db.item, "id", "id", proyectoItem.idContratoItem);
            ViewBag.idProyecto = new SelectList(db.proyecto, "id", "nombre", proyectoItem.idProyecto);
            return View(proyectoItem);
        }

        // POST: ProyectoItem/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idProyecto,idContratoItem,fechaInicio,fechaFin,costoEstimado")] proyectoItem proyectoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyectoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idItem = new SelectList(db.item, "id", "id", proyectoItem.idContratoItem);
            ViewBag.idProyecto = new SelectList(db.proyecto, "id", "nombre", proyectoItem.idProyecto);
            return View(proyectoItem);
        }

        // GET: ProyectoItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyectoItem proyectoItem = db.proyectoItem.Find(id);
            if (proyectoItem == null)
            {
                return HttpNotFound();
            }
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
