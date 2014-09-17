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
    public partial class ProgramaController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Programa
        public ActionResult Index()
        {
            var programa = db.programa.Include(p => p.Contrato).Include(p => p.progProy);
            return View(programa.ToList());
        }

        // GET: Programa/Details/5
        public ActionResult Details(int? idContrato, Int32? ano, Int16? trimestre)
        {
            if ((idContrato == null) || (ano == null) || (trimestre == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(idContrato, ano, trimestre);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }
        /*
        // GET: Programa/Create
        public ActionResult Create()
        {
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id");

            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "Nombre");
            return View();
        }*/

        //// GET: Programa/Edit/5
        //public ActionResult Edit(int? idContrato, Int32? ano, Int16? trimestre)
        //{
        //    if ((idContrato == null) || (ano == null) || (trimestre == null))
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    programa programa = db.programa.Find(idContrato, ano, trimestre);
        //    if (programa == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
        //    ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);
        //    return View(programa);
        //}

        // POST: Programa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.

        //public ActionResult Create([Bind(Include = "id,idContrato,ano,trimestre,idProgProy")] programa programa)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.programa.Add(programa);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
        //    ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);
        //    return View(programa);
        //}
        
        // GET: Programa/Delete/5
        public ActionResult Delete(int? idContrato, Int32? ano, Int16? trimestre)
        {
            if ((idContrato == null) || (ano == null) || (trimestre == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(idContrato, ano, trimestre);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
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
