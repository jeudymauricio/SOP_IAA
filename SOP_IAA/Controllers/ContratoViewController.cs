﻿using SOP_IAA.Models;
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
using System.IO;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SOP_IAA.Controllers
{
    public class ContratoViewController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Contrato
        public ActionResult Index()
        {
            var contrato = db.Contrato.Include(c => c.contratista).Include(c => c.fondo).Include(c => c.zona);
            return View(contrato.ToList());
        }

        // GET
        public ActionResult Create()
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


            ViewBag.idIngeniero = new SelectList(mquery, "Value", "Text");
            ViewBag.idLaboratorio = new SelectList(db.laboratorioCalidad, "id", "nombre");
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Create(string jsonContrato, string jsonLaboratorios, string jsonIngenieros)
        {
            if (ModelState.IsValid)
            {
                List<int> listLaboratios = new List<int>();
                List<int> listIngenieros = new List<int>();
                Contrato contrato = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Contrato>(jsonContrato);

                //Obtener los laboratios
                dynamic jObj = JsonConvert.DeserializeObject(jsonLaboratorios);
                foreach (var child in jObj.Laboratorios.Children())
                {
                    //listLaboratios.Add((int)child);
                    contrato.laboratorioCalidad.Add(db.laboratorioCalidad.Find((int)child));
                }

                //inserción del contrato a la DB
                db.Contrato.Add(contrato);
                db.SaveChanges();
                //obtención del id del contrato
                var idContrato = contrato.id;
                
                //Obtener ingenieros.
                jObj = JsonConvert.DeserializeObject(jsonIngenieros);
                foreach (var child in jObj.Ingenieros.Children())
                {
                    //Creación del ingeniero-contrato.
                    ingenieroContrato ing_contrato = new ingenieroContrato();
                    ing_contrato.idContrato = idContrato;
                    ing_contrato.idIngeniero = (int)child;
                    db.ingenieroContrato.Add(ing_contrato);
                    db.SaveChanges();
                }
                
            }
            //RedirectToAction("Index", "Contrato");
            return View("Index");
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

            laboratorioCalidad lab = new laboratorioCalidad { 
                id=laboratorio.id,
                nombre = laboratorio.nombre,
                tipo= laboratorio.tipo
            };

            //Se procede a convertir a JSON el objeto recien creado
            var json = new JavaScriptSerializer().Serialize(lab);

            //Se retorna el JSON
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: Obtener los detalles de un contrato específico
        public ActionResult Details(int? id)
        {
            Contrato contrato = db.Contrato.Find(id);
            return View();
        }
    }
}