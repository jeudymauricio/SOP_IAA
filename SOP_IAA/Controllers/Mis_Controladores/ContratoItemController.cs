using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;
using Newtonsoft.Json;

namespace SOP_IAA.Controllers
{
    public partial class ContratoItemController : Controller
    {
        // GET: ContratoItem
        public ActionResult Index(int? idContrato)
        {/*
            var contratoItem = db.contratoItem.Include(c => c.Contrato).Include(c => c.item);
            return View(contratoItem.ToList());*/
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(idContrato);
            return View(contrato);
        }

        // GET: ContratoItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);
            if (contratoItem == null)
            {
                return HttpNotFound();
            }
            return View(contratoItem);
        }

        // GET: ContratoItem/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            contratoItem contrItem = new contratoItem
            {
                idContrato = idContrato.Value
            };


            var itemGlobal = db.item.ToList();
            List<item> itemContrato = new List<item>();

            foreach (var item in db.Contrato.Find(idContrato).contratoItem)
            {
                itemContrato.Add(item.item);
            }

            ViewBag.idItem = new SelectList(itemGlobal.Except(itemContrato), "id", "codigoItem");

            //ViewBag.idItem = new SelectList(db.item, "id", "codigoItem");
            /*ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");*/
            return View(contrItem);
        }

        /// <summary>
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <returns>Json con los detalles del item del contrato(incluido su reajuste mas cercano)</returns>
        public ActionResult ItemDetalles(int? id)
        {
            // Si el id o la fecha están vacíos se retorna un badrequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Selecciona de la base de datos el contratoItem correspondiente
            item ci = db.item.Find(id);

            // Si está nulo, quiere decir que no existe el item en el contrato
            if (ci == null)
            {
                return HttpNotFound();
            }

            /// Se inicia con la creacion de un diccionario con toda la información pertinente del item
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("codigoItem", ci.codigoItem);
            result.Add("descripcion", ci.descripcion);
            result.Add("unidadMedida", ci.unidadMedida);

            // Retorna un JSON con los detalles del ítem
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: ContratoItem/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContrato,idItem,precioUnitario,cantidadAprobada")] contratoItem contratoItem, [Bind(Include="jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Como el formato de números utiliza la , como separador de decimales(es-CR), se debe convertir a formato inglés(en-US)
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                    dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                    foreach (var child in jObj.Items.Children())
                    {
                        //Creación del contrato-item.
                        contratoItem ci = new contratoItem();
                        ci.idContrato = contratoItem.idContrato;
                        ci.idItem = int.Parse(child.idItem.Value);
                        ci.precioUnitario = decimal.Parse(child.precio.Value, culture);
                        ci.cantidadAprobada = decimal.Parse(child.cantidadAprobada.Value, culture);
                        
                        // Se agrega el boleta-item a la base de datos
                        db.contratoItem.Add(ci);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { idContrato = contratoItem.idContrato });
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "Error al ingresar el item al proyecto, verifique si está duplicado");
                }
            }

            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", contratoItem.idContrato);
            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", contratoItem.idItem);
            return View(contratoItem);
        }

        // GET: ContratoItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);
            if (contratoItem == null)
            {
                return HttpNotFound();
            }
            /*ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", contratoItem.idContrato);
            ViewBag.idItem = new SelectList(db.item, "id", "codigoItem", contratoItem.idItem);*/
            return View(contratoItem);
        }

        // POST: ContratoItem/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id, idContrato, idItem, precioUnitario, cantidadAprobada")] contratoItem contratoItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contratoItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = contratoItem.idContrato });
            }
            
            return View(db.contratoItem.Find(contratoItem.id));
        }

        // GET: ContratoItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);
            if (contratoItem == null)
            {
                return HttpNotFound();
            }
            return View(contratoItem);
        }

        // POST: ContratoItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            contratoItem contratoItem = db.contratoItem.Find(id);

            try
            {    
                db.boletaItem.RemoveRange(contratoItem.boletaItem);
                db.pICI.RemoveRange(contratoItem.pICI);
                db.oMCI.RemoveRange(contratoItem.oMCI);
                db.subproyectoContratoItem.RemoveRange(contratoItem.subproyectoContratoItem);
                db.itemReajuste.RemoveRange(contratoItem.itemReajuste);
                //db.itemReajuste.RemoveRange(contratoItem.itemReajuste);
                db.contratoItem.Remove(contratoItem);
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = contratoItem.idContrato });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { idContrato = contratoItem.idContrato });
            }
        }
    }
}
