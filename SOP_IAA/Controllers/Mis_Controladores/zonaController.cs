using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;
using SOP_IAA_Utilerias;
using System.Web.Script.Serialization;

namespace SOP_IAA.Controllers
{
    public partial class ZonaController : Controller
    {
        // POST: zona/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] zona zona)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<zona> rep = new Repositorio<zona>();
                    rep.Insertar(zona);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_ZONA_AGREGAR") + ex.Message;
                }
            }

            return View(zona);
        }

        // POST: zona/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] zona zona)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<zona> rep = new Repositorio<zona>();
                    rep.Actualizar(zona);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_ZONA_ACTUALIZAR") + ex.Message;
                }
            }
            return View(zona);
        }

        // POST: zona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            zona zona = db.zona.Find(id);
            db.zona.Remove(zona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RutasAsociadas(int? id)
        {
            zona zona = db.zona.Find(id);
            //var rutas = db.ruta.Include(rut => rut.nombre).Include(rut => rut.descripcion).Where(rut => rut.zona == id);
            return View(zona);
        }

        public ActionResult RutasAgregar(int id)
        {
            ViewBag.rutas = new SelectList(db.ruta, "id", "nombre");
            zona zona = db.zona.Find(id);
            
            return View(zona);
        }

        [HttpPost]
        public ActionResult RutasAgregar(string jsonRutas)
        {
            

            return View();
        }

        public ActionResult RutasEliminar()
        {
            return View();
        }

        // Muestra los detalles de una ruta específica
        public ActionResult RutasDetalles(int? id)
        {
            return RedirectToAction("Details", "Ruta", new {id});
        }

        // Acción invocada desde un ajax y que retorna los detalles de una ruta por su id
        public ActionResult RutaDetalles(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // SE busca el id de la ruta en la lista de rutas
            ruta ruta = db.ruta.Find(id);

            if (ruta == null)
            {
                //Si no se encuetra una coincidencia se retorna un NotFound
                return HttpNotFound();
            }

            //Se procede a convertir a JSON el objeto ruta
            var json = new JavaScriptSerializer().Serialize(ruta);

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}
