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
    public class Proyecto_EstructuraController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Proyecto_Estructura
        public ActionResult Index(int? idRuta)
        {
            if (idRuta == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ruta ruta = db.ruta.Find(idRuta);
            if (ruta == null)
            {
                return HttpNotFound();
            }

            return View(ruta);
        }

        // GET: Proyecto_Estructura/Create
        public ActionResult Create(int? idRuta)
        {
            if (idRuta == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre");
            proyecto_estructura pe = new proyecto_estructura();
            pe.idRuta = idRuta.Value;

            return View(pe);
        }

        // POST: Proyecto_Estructura/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idRuta,descripcion")] proyecto_estructura proyecto_estructura)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.proyecto_estructura.Add(proyecto_estructura);
                    
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idRuta = proyecto_estructura.idRuta });
                }
                catch
                {
                    ModelState.AddModelError("", "Ocurrió un error al agregar el Proyecto/Estructura, verifique que no haya un duplicado");
                }
            }

            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto_estructura.idRuta);
            return View(proyecto_estructura);
        }

        // GET: Proyecto_Estructura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto_estructura proyecto_estructura = db.proyecto_estructura.Find(id);
            if (proyecto_estructura == null)
            {
                return HttpNotFound();
            }
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto_estructura.idRuta);
            return View(proyecto_estructura);
        }

        // POST: Proyecto_Estructura/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idRuta,descripcion")] proyecto_estructura proyecto_estructura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proyecto_estructura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idRuta = proyecto_estructura.idRuta });
            }
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto_estructura.idRuta);
            return View(proyecto_estructura);
        }

        // GET: Proyecto_Estructura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto_estructura proyecto_estructura = db.proyecto_estructura.Find(id);
            if (proyecto_estructura == null)
            {
                return HttpNotFound();
            }
            return View(proyecto_estructura);
        }

        // POST: Proyecto_Estructura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            proyecto_estructura proyecto_estructura = db.proyecto_estructura.Find(id);
            db.proyecto_estructura.Remove(proyecto_estructura);
            db.SaveChanges();
            return RedirectToAction("Index", new { idRuta = proyecto_estructura.idRuta });
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
