using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;
using SOP_IAA.Models;

namespace SOP_IAA.Controllers
{
    public partial class ProgramaController : Controller
    {
        // Acción que despliega la lista de programas de un contrato específico
        public static int id;
        public ActionResult MisProgramas(int? _id)
        {
            if (_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Se busca el contrato específico
            Contrato contrato = db.Contrato.Find(_id);
            id = _id.Value;
            ViewBag.id = _id;
            return View(contrato);
        }
        
        // GET: Vista inicial que le permite al usuario crear un nuevo programa para el contrato
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
                catch (Exception)
                {
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
        public ActionResult Edit([Bind(Include = "idContrato,ano,trimestre,ProgProy")] programa programa, [Bind(Include = "anoAnt")] string anoAnt, [Bind(Include = "triAnt")] string triAnt)
        {
            if (ModelState.IsValid)
            {
                // Busca el programa en la base de datos
                
                programa programaEditar = db.programa.Find(programa.idContrato, int.Parse(anoAnt), int.Parse(triAnt));
                progProy progproyEditar = db.progProy.Find(programa.progProy.id);

                programaEditar.ano = programa.ano;
                programaEditar.trimestre = programa.trimestre;
                
                // Guarda los cambios en el programa
                db.Entry(programaEditar).State = EntityState.Modified;

                progproyEditar.fechaInicio = programa.progProy.fechaInicio;
                progproyEditar.fechaFin = programa.progProy.fechaFin;
                progproyEditar.monto = programa.progProy.monto;
                
                db.Entry(progproyEditar).State = EntityState.Modified;

                // Guarda los cambios en la base de datos
                db.SaveChanges();
                return RedirectToAction("MisProgramas", new { _id = programa.idContrato });
            }

            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", programa.idContrato);
            ViewBag.idProgProy = new SelectList(db.progProy, "id", "id", programa.idProgProy);
            return View(programa);
        }

        // POST: Programa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "idContrato,ano,trimestre,ProgProy")] programa programa)
        {
            // Se busca el programa en la base de datos
            programa prog = db.programa.Find(programa.idContrato, programa.ano, programa.trimestre);

            // Se almacena el id del contrato para una vez eliminado el programa regresar al contrato
            int idContrato = prog.idContrato;
            
            // Se eliminan todos los proyectos del programa
            db.proyecto.RemoveRange(prog.progProy.proyecto);

            // Eliminar de.....

            // Se elimina la relación de los proyectos al programa
            db.progProy.Remove(prog.progProy);

            // Se elimina el programa de la base de datos
            db.programa.Remove(prog);

            // Se guardan los cambios en la base de datos
            db.SaveChanges();

            return RedirectToAction("MisProgramas", new { _id = programa.idContrato });
        }

        public ActionResult AddProject(int? id, int? idContrato, Int32? ano, Int16? trimestre)
        {
            if ((id == null) || (idContrato == null) || (ano == null) || (trimestre == null))
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

        public ActionResult AgregarProyecto(/*int? id, */int? idContrato, Int32? ano, Int16? trimestre)
        {
            //programa programa = db.programa.Find(id, idContrato, ano, trimestre);



            //var proyecto = db.proyecto.Include(pr => pr.progProy).Include(pr => pr.ruta).Include(pr => pr.tipoProyecto).Where(pr => pr.idProgProy == programa.idProgProy);

            //  var programa = db.programa.Include(p => p.Contrato).Include(p => p.progProy);

            return RedirectToAction("Index", "Proyecto", new { /*_id = id,*/ _idContrato = idContrato, _ano = ano, _trimestre = trimestre });
            //return View("/proyecto/Index",proyecto.ToList());
        }
    }
}
