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
    public class proyectoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: proyecto
        public ActionResult Index()
        {
            var proyecto = db.proyecto.Include(p => p.progProy).Include(p => p.ruta).Include(p => p.tipoProyecto);
            return View(proyecto.ToList());
        }

        // GET: proyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // GET: proyecto/Create
        public ActionResult Create()
        {
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id");
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre");
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre");
            return View();
        }

        // POST: proyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idProgProy,idTipoProyecto,idRuta,nombre")] proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.proyecto.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", proyecto.idProgProy);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // GET: proyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", proyecto.idProgProy);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // POST: proyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idProgProy,idTipoProyecto,idRuta,nombre")] proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", proyecto.idProgProy);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // GET: proyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // POST: proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proyecto proyecto = db.proyecto.Find(id);
            db.proyecto.Remove(proyecto);
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
