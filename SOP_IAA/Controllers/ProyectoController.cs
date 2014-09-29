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
    public class ProyectoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Proyecto
        public ActionResult Index(int? _id)
        {
            //var proyecto = db.proyecto.Include(p => p.programa).Include(p => p.ruta).Include(p => p.tipoProyecto);
            //return View(proyecto.ToList());
            if ((_id == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(_id);
            return View(programa);
        }

        // GET: Proyecto/Details/5
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

        // GET: Proyecto/Create
        public ActionResult Create(int? _id)
        {
            if (_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idPrograma = _id;//new SelectList(db.programa, "id", "id");
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre");
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre");
            return View();
        }

        // POST: Proyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idPrograma,idTipoProyecto,idRuta,nombre")] proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.proyecto.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index", new { _id = proyecto.idPrograma });
            }

            ViewBag.idPrograma = new SelectList(db.programa, "id", "id", proyecto.idPrograma);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // GET: Proyecto/Edit/5
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
            //ViewBag.idPrograma = new SelectList(db.programa, "id", "id", proyecto.idPrograma);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // POST: Proyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idPrograma,idTipoProyecto,idRuta,nombre")] proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { _id = proyecto.idPrograma });
            }
            ViewBag.idPrograma = new SelectList(db.programa, "id", "id", proyecto.idPrograma);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // GET: Proyecto/Delete/5
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

        // POST: Proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proyecto proyecto = db.proyecto.Find(id);
            db.proyecto.Remove(proyecto);
            db.SaveChanges();
            return RedirectToAction("Index", new { _id = proyecto.idPrograma });
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
