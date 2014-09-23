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

        // GET: Boleta
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.idContrato = idContrato;

            var boleta = db.boleta.Where(b => b.idContrato == idContrato).Include(b => b.fondo).Include(b => b.inspector).Include(b => b.proyecto_estructura).Include(b => b.ruta);
            //var boleta = db.boleta.Include(b => b.fondo).Include(b => b.inspector).Include(b => b.proyecto_estructura).Include(b => b.ruta);
            return View(boleta.ToList());
        }

        // GET: Boleta/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Se busca el contrato, esto para cargar el fondo y las rutas correspondientes al contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            
            // Si no existe el contrato se retorna un 'No encontrado'
            if (contrato == null)
            {
                return HttpNotFound();
            }

            // Selecciona solo los fondos que existen el el contrato, siempre va a ser uno
            var fondoContrato = db.fondo.Where(f => f.id == contrato.fondo.id);
            ViewBag.idFondo = new SelectList (fondoContrato, "id", "nombre"); // new SelectList(db.fondo, "id", "nombre")

            // Se carga la lista de inspectores del sistema
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona");

            // Se cargan las rutas correspondientes a la zona del contrato
            ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "nombre"); // new SelectList(db.ruta, "id", "nombre");

            // 
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion");

            // Se manda una boleta con los datos iniciales que debe ser llenada en la vista
            boleta boleta = new boleta
            {
                idContrato = idContrato.Value,
                idFondo = contrato.idFondo
            };

            return View(boleta);
        }

        // POST: Boleta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContrato,id,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta)
        {
            if (ModelState.IsValid)
            {
                db.boleta.Add(boleta);
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = boleta.idContrato });
            }

            ViewBag.idContrato = boleta.idContrato;
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", boleta.idFondo);
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona", boleta.idInspector);
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion", boleta.idProyecto_Estructura);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", boleta.idRuta);
            return View(boleta);
        }

        // GET: Boleta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            boleta boleta = db.boleta.Find(id);
            if (boleta == null)
            {
                return HttpNotFound();
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

        // GET: Boleta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            boleta boleta = db.boleta.Find(id);
            if (boleta == null)
            {
                return HttpNotFound();
            }
            return View(boleta);
        }

        // GET: Boleta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            boleta boleta = db.boleta.Find(id);
            if (boleta == null)
            {
                return HttpNotFound();
            }
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
