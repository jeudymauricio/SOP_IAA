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
    public class ProgramaController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Programa
        public ActionResult Index()
        {
            var programa = db.programa.Include(p => p.Contrato).Include(p => p.progProy);
            return View(programa.ToList());
        }

        // GET: Programa/Details/5
        public ActionResult Details(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id,idContrato,ano,trimestre);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        // GET: Programa/Create
        public ActionResult Create()
        {
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id");

            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "Nombre");
            return View();
        }

        // POST: Programa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        public ActionResult Create([Bind(Include = "id,idContrato,ano,trimestre,idProgProy")] programa programa, [Bind(Include = "id,fechaInicio,fechaFin,monto")] progProy progProy)
        {
            if (ModelState.IsValid)
            {
                /*progProy progProy = new progProy();
                 progProy.fechaInicio = programa.progProy.fechaInicio;
                 progProy.fechaFin = programa.progProy.fechaFin;
                 progProy.monto = programa.progProy.monto;*/

                db.progProy.Add(progProy);
                db.SaveChanges();
                programa.idProgProy = progProy.id;

                db.programa.Add(programa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(programa);
        }

        // GET: Programa/Edit/5
        public ActionResult Edit(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id, idContrato, ano, trimestre);
            if (programa == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);
            return View(programa);
        }

        // POST: Programa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,ano,trimestre,idProgProy")] programa programa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(programa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);
            return View(programa);
        }

        // GET: Programa/Delete/5
        public ActionResult Delete(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id, idContrato, ano, trimestre);
            if (programa == null)
            {
                return HttpNotFound();
            }
            return View(programa);
        }

        // POST: Programa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            programa programa = db.programa.Find(id);
            db.programa.Remove(programa);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddProject(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            programa programa = db.programa.Find(id, idContrato, ano, trimestre);
  
            if (programa == null)
            {
                return HttpNotFound();
            }
            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            //ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);

            return View("Index", db.programa);
        }

        public ActionResult AgregarProyecto(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            //programa programa = db.programa.Find(id, idContrato, ano, trimestre);

         

            //var proyecto = db.proyecto.Include(pr => pr.progProy).Include(pr => pr.ruta).Include(pr => pr.tipoProyecto).Where(pr => pr.idProgProy == programa.idProgProy);
            
          //  var programa = db.programa.Include(p => p.Contrato).Include(p => p.progProy);

            return RedirectToAction("Index", "Proyecto", new { _id = id, _idContrato = idContrato, _ano = ano, _trimestre = trimestre });
            //return View("/proyecto/Index",proyecto.ToList());

         
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
