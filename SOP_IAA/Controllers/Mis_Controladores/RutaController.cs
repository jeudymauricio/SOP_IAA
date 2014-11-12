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

namespace SOP_IAA.Controllers
{
    public partial class RutaController : Controller
    {
        
        // POST: Ruta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,descripcion")] ruta ruta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<ruta> rep = new Repositorio<ruta>();
                    rep.Insertar(ruta);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_RUTA_AGREGAR") + ex.Message;
                }
            }

            return View(ruta);
        }

        // POST: Ruta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre,descripcion")] ruta ruta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<ruta> rep = new Repositorio<ruta>();
                    rep.Actualizar(ruta);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_RUTA_ACTUALIZAR") + ex.Message;
                }
            }
            return View(ruta);
        }

        // POST: Ruta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Busca la ruta en la base de datos
            ruta ruta = db.ruta.Find(id);

            // Elimina los proyecto-Estructura asociados
            db.proyecto_estructura.RemoveRange(ruta.proyecto_estructura);

            // Elimina las relaciones a la tabla zona
            ruta.zona.Clear();
            
            // Elimina la ruta de la base de datos
            db.ruta.Remove(ruta);

            // Guarda los cambios en la base de datos
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
