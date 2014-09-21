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
    public partial class BoletaController : Controller
    {

        // POST: Boleta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta)
        {
            if (ModelState.IsValid)
            {
                db.boleta.Add(boleta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", boleta.idFondo);
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona", boleta.idInspector);
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion", boleta.idProyecto_Estructura);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", boleta.idRuta);
            return View(boleta);
        }

       
        // POST: Boleta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boleta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", boleta.idFondo);
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona", boleta.idInspector);
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion", boleta.idProyecto_Estructura);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", boleta.idRuta);
            return View(boleta);
        }

        
        // POST: Boleta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            boleta boleta = db.boleta.Find(id);
            db.boleta.Remove(boleta);
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
