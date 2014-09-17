using SOP_IAA_DAL;
using SOP_IAA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace SOP_IAA.Controllers
{
    public class LinkContratoProgramaController : Controller
    {
        // Conexión a la base de datos
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // Acción que despliega la lista de programas de un contrato específico
        public ActionResult MisProgramas(int? _id)
        {
            // Se busca el contrato específico
             Contrato contrato = db.Contrato.Find(_id);

            return View(contrato);
        }

        // GET: Vista inicial que le permite al usuario crear un nuevo programa para el contrato
        public ActionResult Create(int id)
        {
            LinkContratoProgramaModels ContratoPrograma = new LinkContratoProgramaModels();
            Contrato contrato = db.Contrato.Find(id);

            ContratoPrograma.idContrato = contrato.id;
            ContratoPrograma.licitacion = contrato.licitacion;
            
            return View(ContratoPrograma);
        }

        // POST: Crea y enlaza el programa al contrato.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "idContrato, licitacion, ano, trimestre, fechaInicio, fechaFin, monto")] LinkContratoProgramaModels ContratoPrograma)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    progProy progproy = new progProy
                    {
                        fechaInicio = ContratoPrograma.fechaInicio,
                        fechaFin = ContratoPrograma.fechaFin,
                        monto = ContratoPrograma.monto
                    };

                    db.progProy.Add(progproy);

                    programa program = new programa
                    {
                        idContrato = ContratoPrograma.idContrato,
                        idProgProy = progproy.id,
                        ano = ContratoPrograma.ano,
                        trimestre = ContratoPrograma.trimestre
                    };

                    db.programa.Add(program);
                    db.SaveChanges();

                    return RedirectToAction("MisProgramas", new { _id = ContratoPrograma.idContrato });
                }
                catch(Exception){
                    ModelState.AddModelError("", "Ocurrió un error al agregar el programa, verifique que no haya otro programa para ese año y trimestre");
                }
            }

            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id");
            ViewBag.idTipoProyecto = new SelectList(db.tipoProyecto, "id", "Nombre");

            return View(ContratoPrograma);
        }

        // GET: Programa/Edit/5
        public ActionResult Edit(int? idContrato, Int32? ano, Int16? trimestre)
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
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);
            return View(programa);
        }

        // POST: Programa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,ano,trimestre,ProgProy")] programa programa)
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

        // Acción que redirecciona a una lista de proyectos de un programa específico
        public ActionResult AgregarProyecto(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            
            return RedirectToAction("MisProyectos", "Proyecto", new { _id = id, _idContrato = idContrato, _ano = ano, _trimestre = trimestre });
            
        }

    }
}