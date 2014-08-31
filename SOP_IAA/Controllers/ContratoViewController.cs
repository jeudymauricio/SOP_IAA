using SOP_IAA.Models;
using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Data;
using System.Net;
using System.Web.Script.Serialization;

namespace SOP_IAA.Controllers
{
    public class ContratoViewController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        public ActionResult CreateContractEngineer()
        {
            var model = new ContratoViewModels();
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

            model.ListIngeniero = new SelectList(mquery, "Value", "Text");

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContratoViewModels contratoView)
        {
            if (ModelState.IsValid)
            {
                Contrato miContrato = new Contrato();
                miContrato.idContratista = contratoView.idContratista;
                miContrato.idFondo = contratoView.idFondo;
                miContrato.licitacion = contratoView.contrato.licitacion;
                miContrato.lineaContrato = contratoView.contrato.lineaContrato;
                miContrato.idZona = contratoView.idZona;
                miContrato.fechaInicio = contratoView.contrato.fechaInicio;
                miContrato.plazo = contratoView.contrato.plazo;
                miContrato.lugar = contratoView.contrato.lugar;

                db.Contrato.Add(miContrato);
                db.SaveChanges();
                return RedirectToAction("Index","Contrato");
            }

            return View();
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
    }
}