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
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Collections;

namespace SOP_IAA.Controllers
{
    public partial class ContratoController : Controller
    {

        // GET: Contrato/Create
        public ActionResult Create()
        {
            //var model = new Contrato();
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre");
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre");
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre");

            var mquery = (from p in db.persona
                          join i in db.ingeniero
                          on p.id equals i.idPersona
                          select new SelectListItem
                          {
                              Value = p.id.ToString(),
                              Text = p.nombre + " " + p.apellido1 + " " + p.apellido2
                          }
                );


            ViewBag.idIngeniero = new SelectList(mquery, "Value", "Text");
            ViewBag.idLaboratorio = new SelectList(db.laboratorioCalidad, "id", "nombre");
            return View();
        }

        // POST: Contrato/Create
        [HttpPost]
        public ActionResult Create(
            [Bind(Include = "id,idContratista,licitacion,lineaContrato,idZona,fechaInicio,plazo,lugar,idFondo")] Contrato contrato,
            [Bind(Include = "jsonIng")] string jsonIng,
            [Bind(Include = "jsonLab")] string jsonLab
            )
        {
            if (ModelState.IsValid)
            {
                //Obtener los laboratios
                dynamic jObj = JsonConvert.DeserializeObject(jsonLab);
                foreach (var child in jObj.Laboratorios.Children())
                {
                    contrato.laboratorioCalidad.Add(db.laboratorioCalidad.Find((int)child));
                }

                //inserción del contrato a la DB
                db.Contrato.Add(contrato);
                db.SaveChanges();
                //obtención del id del contrato
                var idContrato = contrato.id;

                //Obtener ingenieros.
                jObj = JsonConvert.DeserializeObject(jsonIng);
                foreach (var child in jObj.Ingenieros.Children())
                {
                    //Creación del ingeniero-contrato.
                    ingenieroContrato ing_contrato = new ingenieroContrato();
                    ing_contrato.idContrato = idContrato;
                    ing_contrato.idIngeniero = (int)child;
                    db.ingenieroContrato.Add(ing_contrato);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            var mquery = (from p in db.persona
                          join i in db.ingeniero
                          on p.id equals i.idPersona
                          select new SelectListItem
                          {
                              Value = p.id.ToString(),
                              Text = p.nombre + " " + p.apellido1 + " " + p.apellido2
                          }
                );


            ViewBag.idIngeniero = new SelectList(mquery, "Value", "Text");
            ViewBag.idLaboratorio = new SelectList(db.laboratorioCalidad, "id", "nombre");

            return View(contrato);
        }

        // GET: Obtener los detalles de un ingeniero específico
        public ActionResult IngenieroDetalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // SE busca el id de ingeniero en la lista de ingenieros
            ingeniero ingeniero = db.ingeniero.Find(id);

            if (ingeniero == null)
            {
                //Si no se encuetra una coincidencia se retorna un NotFound
                return HttpNotFound();
            }

            // Se crea un objeto con las propiedades de ingeniero y persona
            var obj = new ingeniero
            {
                idPersona = ingeniero.idPersona,
                persona = new persona
                {
                    id = ingeniero.persona.id,
                    nombre = ingeniero.persona.nombre,
                    apellido1 = ingeniero.persona.apellido1,
                    apellido2 = ingeniero.persona.apellido2
                },
                rol = ingeniero.rol,
                descripcion = ingeniero.descripcion,
                departamento = ingeniero.departamento
            };

            //Se procede a convertir a JSON el objeto recien creado
            var json = new JavaScriptSerializer().Serialize(obj);

            //Se retorna el JSON
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: Obtener los detalles de un laboratorios específico
        public ActionResult LaboratorioDetalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // SE busca el id de ingeniero en la lista de ingenieros
            laboratorioCalidad laboratorio = db.laboratorioCalidad.Find(id);

            if (laboratorio == null)
            {
                //Si no se encuetra una coincidencia se retorna un NotFound
                return HttpNotFound();
            }

            laboratorioCalidad lab = new laboratorioCalidad
            {
                id = laboratorio.id,
                nombre = laboratorio.nombre,
                tipo = laboratorio.tipo
            };

            //Se procede a convertir a JSON el objeto recien creado
            var json = new JavaScriptSerializer().Serialize(lab);

            //Se retorna el JSON
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: Contrato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrato contrato = db.Contrato.Find(id);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            var mquery = (from p in db.persona
                          join i in db.ingeniero
                          on p.id equals i.idPersona
                          select new SelectListItem
                          {
                              Value = p.id.ToString(),
                              Text = p.nombre + " " + p.apellido1 + " " + p.apellido2
                          }
                );

            ViewBag.idIngeniero = new SelectList(mquery, "Value", "Text");
            ViewBag.idLaboratorio = new SelectList(db.laboratorioCalidad, "id", "nombre");
            return View(contrato);
        }

        // POST: Contrato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "id,idContratista,licitacion,lineaContrato,idZona,fechaInicio,plazo,lugar,idFondo")] Contrato contrato,
            [Bind(Include = "jsonIng")] string jsonIng,
            [Bind(Include = "jsonLab")] string jsonLab
            )
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Contrato contratoEditar = db.Contrato.Find(contrato.id);

                    contratoEditar.laboratorioCalidad.Clear();

                    //Obtener los laboratorios
                    dynamic jObj = JsonConvert.DeserializeObject(jsonLab);
                    laboratorioCalidad lab;

                    foreach (var child in jObj.Laboratorios.Children())
                    {
                        lab = db.laboratorioCalidad.Find((int)child);
                        contratoEditar.laboratorioCalidad.Add(lab);
                    }

                    db.ingenieroContrato.RemoveRange(contratoEditar.ingenieroContrato);

                    //Obtener ingenieros.
                    jObj = JsonConvert.DeserializeObject(jsonIng);
                    foreach (var child in jObj.Ingenieros.Children())
                    {
                        ingenieroContrato ing_contrato = new ingenieroContrato();
                        ing_contrato.idContrato = contrato.id;
                        ing_contrato.idIngeniero = (int)child;
                        db.ingenieroContrato.Add(ing_contrato);
                        db.SaveChanges();
                    }

                    db.SaveChanges();

                    // Actualización del contrato
                    Repositorio<Contrato> rep = new Repositorio<Contrato>();
                    rep.Actualizar(contrato);

                    return RedirectToAction("MiContrato", new {contrato.id});
                }
                catch (Exception ex)
                {
                    ViewBag.Error = true;
                    ViewBag.MensajeError = Utilerias.ValorRecurso(Utilerias.ArchivoRecurso.UtilRecursos, "ERROR_CONTRATO_ACTUALIZAR") + ex.Message;
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            ViewBag.idContratista = new SelectList(db.contratista, "id", "nombre", contrato.idContratista);
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", contrato.idFondo);
            ViewBag.idZona = new SelectList(db.zona, "id", "nombre", contrato.idZona);
            var mquery = (from p in db.persona
                          join i in db.ingeniero
                          on p.id equals i.idPersona
                          select new SelectListItem
                          {
                              Value = p.id.ToString(),
                              Text = p.nombre + " " + p.apellido1 + " " + p.apellido2
                          }
                );
            ViewBag.idIngeniero = new SelectList(mquery, "Value", "Text");
            ViewBag.idLaboratorio = new SelectList(db.laboratorioCalidad, "id", "nombre");

            return View(contrato);
        }

        // POST: Contrato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Contrato contrato = db.Contrato.Find(id);

            try
            {
                db.ingenieroContrato.RemoveRange(contrato.ingenieroContrato);
                contrato.laboratorioCalidad.Clear();
                foreach (programa prog in contrato.programa)
                {
                    /* hay que eliminar los proyectos de cada progproy */
                    db.progProy.Remove(prog.progProy);
                }
                contrato.programa.Clear();
                db.Contrato.Remove(contrato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            //return View();
        }

        // Vista que corresponde al actionlink "Ir al Contrato" que despliega el contrato con su respectivo menú de acciones
        public ActionResult MiContrato(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(id);

            return View(contrato);
        }
    }
}
