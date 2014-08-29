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
    public partial class ContratoController : Controller
    {

        // POST: Contrato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContratista,licitacion,lineaContrato,idZona,fechaInicio,plazo,lugar,idFondo")] Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<Contrato> rep = new Repositorio<Contrato>();
                    rep.Insertar(contrato);
                    var idNuevo = contrato.id;

                    //aqui se agregarian los ingenieros


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_CONTRATO_AGREGAR") + ex.Message;
                }
            }

            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            return View(contrato);
        }

        // POST: Contrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContratista,licitacion,lineaContrato,idZona,fechaInicio,plazo,lugar,idFondo")] Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<Contrato> rep = new Repositorio<Contrato>();
                    rep.Actualizar(contrato);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_CONTRATO_ACTUALIZAR") + ex.Message;
                }
            }
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            return View(contrato);
        }

        // POST: Contrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contrato contrato = db.Contrato.Find(id);
            db.Contrato.Remove(contrato);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
