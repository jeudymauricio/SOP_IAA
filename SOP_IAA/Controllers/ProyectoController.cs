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
        //static int? __id;
        static int? __idContrato;
        static Int32? __ano;
        static Int16? __trimestre;

        static int __idProgProy;
       // static int? __ProgProy;

        // GET: proyecto
        public ActionResult Index(/*int? _id,*/int? _idContrato, Int32? _ano, Int16? _trimestre)
        {
            if ((_idContrato == null) || (_ano == null) || (_trimestre == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var proyecto = db.proyecto.Include(p => p.progProy).Include(p => p.ruta).Include(p => p.tipoProyecto);
            programa programa = db.programa.Find(_idContrato, _ano, _trimestre);
            var proyecto = db.proyecto.Include(pr => pr.progProy).Include(pr => pr.ruta).Include(pr => pr.tipoProyecto).Where(pr => pr.idProgProy == programa.idProgProy);
            //ViewBag.idP = _id;
            //ViewBag.idP = programa.id;
            ViewBag.idCont = _idContrato;
            ViewBag.ano = _ano;
            ViewBag.tri = _trimestre;
            ViewBag.idProgProy = programa.idProgProy;

            //__id = _id;
            __idContrato = _idContrato;
            __ano = _ano;
            __trimestre = _trimestre;
            
            __idProgProy = programa.idProgProy;

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

            //Varibles para volver al Index
            //ViewBag.idP = __id;
            ViewBag.idCont = __idContrato;
            ViewBag.ano = __ano;
            ViewBag.tri = __trimestre;

            return View(proyecto);
        }

        // GET: proyecto/Create
        public ActionResult Create(int? _idPP)
        {
            if (_idPP == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //ViewBag.idProgProy = new SelectList(db.progProy, "id", "id");
            ViewBag.idProgProy = _idPP;
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre");
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre");

            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre");

            //Varibles para volver al Index
            //ViewBag.idP = __id;
            ViewBag.idCont = __idContrato;
            ViewBag.ano = __ano;
            ViewBag.tri = __trimestre;

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
               // int? _id = __id;
                int? _idContrato = __idContrato;
                Int32? _ano = __ano;
                Int16? _trimestre = __trimestre;
                return RedirectToAction("Index", new { /*_id, */_idContrato, _ano, _trimestre = _trimestre });
            }

            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", proyecto.idProgProy);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", proyecto.idRuta);
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "nombre", proyecto.idTipoProyecto);
            return View(proyecto);
        }

        // GET: proyecto/Edit/5
        public ActionResult Edit(int? id, int? _id, int? _idContrato, Int32? _ano, Int16? _trimestre)
        {
            if ((id == null) || (_id == null) || (_idContrato == null) || (_ano == null) || (_trimestre == null))
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

            //Variables del programa
            ViewBag.idP = _id;
            ViewBag.idCont = _idContrato;
            ViewBag.ano = _ano;
            ViewBag.tri = _trimestre;

            ViewBag.idProgProy = __idProgProy;

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


                //int? _id = __id;
                int? _idContrato = __idContrato;
                Int32? _ano = __ano;
                Int16? _trimestre = __trimestre;


                return RedirectToAction("Index", new {_idContrato, _ano, _trimestre = _trimestre });
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

            //Variables del programa
            //ViewBag.idP = __id;
            ViewBag.idCont = __idContrato;
            ViewBag.ano = __ano;
            ViewBag.tri = __trimestre;

            ViewBag.idProgProy = __idProgProy;

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

            //Para regresar al Index
            //int? _id = __id;
            int? _idContrato = __idContrato;
            Int32? _ano = __ano;
            Int16? _trimestre = __trimestre;


            return RedirectToAction("Index", new { /*_id, */_idContrato, _ano, _trimestre = _trimestre });
        }

        public ActionResult ItemsProyecto(int? id)
        {
            return RedirectToAction("Index", "ProyectoItem", new { /*_id = id,*/ _id = id });
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
