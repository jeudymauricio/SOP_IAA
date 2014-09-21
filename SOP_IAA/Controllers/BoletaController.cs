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
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Boleta
        public ActionResult Index()
        {
            var boleta = db.boleta.Include(b => b.fondo).Include(b => b.inspector).Include(b => b.proyecto_estructura).Include(b => b.ruta);
            return View(boleta.ToList());
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

        // GET: Boleta/Create
        public ActionResult Create()
        {
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre");
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona");
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion");
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre");
            return View();
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

    }
}
