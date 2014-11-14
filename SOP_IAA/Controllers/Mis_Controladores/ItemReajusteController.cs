﻿using System;
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
    public partial class ItemReajusteController : Controller
    {
        public ActionResult Periodo(DateTime fecha, int? idContrato)
        {
            if (idContrato == null || fecha == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                ViewBag.idContrato = idContrato;

                var ir = db.itemReajuste.Where(p => p.ano == fecha.Year).Where(i => i.mes == fecha.Month).Where(n => n.contratoItem.idContrato == idContrato);
                if (ir == null)
                {
                    return HttpNotFound();
                }

                // Se cargan los precios con reajuste
                ViewBag.reajustesActuales = obtenerPrecios(fecha, (int)idContrato);

                // Se almacenan los precios base de cada item
                //ViewBag.reajustesAnteriores = new SelectList(db.contratoItem.Where(ci => ci.idContrato ==idContrato), "id", "precioUnitario");//obtenerPrecios(fecha.AddMonths(-1), (int)idContrato);

                return View(ir);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { idContrato = idContrato });
            }
        }

        /// <summary>
        /// Con base a una fecha, retorna el precio TODOS los ítems en esa fecha (con reajuste, o precio base si no hay reajustes)
        /// </summary>
        /// <param name="fecha">Fecha a buscar</param>
        /// <param name="idContrato">Contrato al que pertenecen los reajustes</param>
        /// <returns>Lista de precios de cada item en el mes</returns>
        public Dictionary<int, decimal> obtenerPrecios(DateTime fecha, int idContrato)
        {
            // Lista de items del contrato
            var contratoItem = db.contratoItem.Where(ci => ci.idContrato == idContrato);

            // Diccionario que contendrá el id del item en el contrato y su respectivo reajuste o precio base  <int> idContratoItem  <decimal> Precio
            Dictionary<int, decimal> precios = new Dictionary<int, decimal>();

            // Se procede a guardar los precios correspondientes a cada item del contrato
            foreach (contratoItem ci in contratoItem)
            {
                // Precio base del item
                decimal precio = ci.precioUnitario;

                // Se busca si hay reajuste para ese mes
                var itemReajustado = db.itemReajuste.Where(ir => (ir.ano == fecha.Year) && (ir.mes == fecha.Month) && (ir.idContratoItem == ci.id));

                // Si hay reajuste se aplica
                if (itemReajustado.Count() > 0)
                {
                    // Reajuste del mes
                    decimal reajuste = itemReajustado.First().reajuste;

                    // Se aplica el reajuste
                    precio = decimal.Round(precio * reajuste + precio, 4);
                }

                // Se almacena el precio
                precios.Add(ci.id, precio);
            }

            // Se retorna el diccionario con los precios correspondientes a los items segun la fecha anterior
            return precios;
        }

        // GET: itemReajustes/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.idContratoItem = new SelectList(db.contratoItem.Where(p => p.idContrato == idContrato)
            //    .Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
            ViewBag.idContrato = idContrato;
            ViewBag.FechaInicio = Json(new { fechaInicio = db.Contrato.Find(idContrato).fechaInicio.ToShortDateString() });

            return View();
        }

        /// <summary>
        /// Funció que basada en un idContrato y fecha, devuelve los items y sus respectivos precios para esa fecha
        /// </summary>
        /// <param name="idContrato">Contrato al que pertenecen los items</param>
        /// <param name="fecha">Fecha a buscar</param>
        /// <returns>Json con los detalles del item y su precio al mes</returns>
        public ActionResult cargarItems(int? idContrato, string fecha)
        {
            if ((idContrato == null)||(string.IsNullOrWhiteSpace(fecha)) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Diccionario que contendrá los detalles de cada item consultado.
            Dictionary<string,string> jObj;

            // Lista que contendrá los detalles de cada item en tuplas
            List<Tuple<string, string, string, string, string>> itemList = new List<Tuple<string, string, string, string,string>>();

            // Se selecciona el contrato de la base de datos
            Contrato contrato = db.Contrato.Find(idContrato);

            // Por cada item del contrato se debe cargar su precio a la fecha
            foreach (contratoItem ci in contrato.contratoItem)
            {
                // Obtener los detalles del item.
                jObj = getItemDetail(ci.id, fecha);
                
                // Se guardan los detalles en una tupla
                var itemDetalle = new Tuple<string, string, string, string, string>(jObj["codigoItem"], jObj["descripcion"], jObj["unidadMedida"], jObj["precioReajustado"], jObj["idContratoItem"]);

                // Se agrega la tupla a la lista de items
                itemList.Add(itemDetalle);
            }

            // Retorna un JSON con los detalles de cada ítem
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Devuelve los detalles de un item específico en un diccionario tipo<string,string>
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <param name="fecha">fecha de la boleta</param>
        /// <returns>Diccionario con los detalles del item del contrato(incluido su reajuste al mes si lo hay)</returns>
        public Dictionary<string,string> getItemDetail(int? id, string fecha)
        {
            // Si el id o la fecha están vacíos se retorna un badrequest
            if ((id == null) || (string.IsNullOrWhiteSpace(fecha)))
            {
                return null;
            }

            // Especifica el formato en que está la fecha, en este caso Costa Rica (es-CR)
            IFormatProvider culture = new System.Globalization.CultureInfo("es-CR", true);

            DateTime fecha2 = new DateTime();
            // convierte el string en datetime
            try
            {
                fecha2 = DateTime.Parse(fecha, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            }
            catch (Exception e)
            {
                return null;
            }

            // Selecciona de la base de datos el contratoItem correspondiente
            contratoItem ci = db.contratoItem.Find(id);

            // Si está nulo, quiere decir que no existe el item en el contrato
            if (ci == null)
            {
                return null;
            }

            /// Se inicia con la creacion de un diccionario con toda la información pertinente del item
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("idContratoItem", ci.id.ToString());
            result.Add("codigoItem", ci.item.codigoItem);
            result.Add("descripcion", ci.item.descripcion);
            result.Add("unidadMedida", ci.item.unidadMedida);

            // Se almacena el precio
            result.Add("precioReajustado", ci.precioUnitario.ToString());

            // Retorna un Diccionario con los detalles del ítem
            return result;
        }
        
        // POST: itemReajustes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            //[Bind(Include = "id,idContratoItem,fecha,mes,ano,reajuste")] itemReajuste itemReajuste,
            [Bind(Include = "jsonItems")] string jsonItems,
            [Bind(Include = "strFechaReajuste")] string strFechaReajuste,
            [Bind(Include = "idContrato")] string idContrato)
        {

            // Especifica el formato en que está la fecha, en este caso Costa Rica (es-CR)
            IFormatProvider cultureDate = new System.Globalization.CultureInfo("es-CR", true);

            DateTime fecha2 = new DateTime();
            // convierte el string en datetime
            try
            {
                fecha2 = DateTime.Parse(strFechaReajuste, cultureDate, System.Globalization.DateTimeStyles.AssumeLocal);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(99, e.Message.ToString());
            }

            try
            {
                // Obtener items con su reajuste.
                dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                // Se indica que los valores numéricos están en formato inglés(en-US)
                IFormatProvider cultureNumber = new System.Globalization.CultureInfo("en-US", true);

                foreach (var child in jObj.Items.Children())
                {
                    //Creación del boleta-item.
                    itemReajuste ir = new itemReajuste();

                    ir.idContratoItem = int.Parse(child.idContratoItem.Value);
                    ir.fecha = fecha2;
                    ir.reajuste = decimal.Parse(child.reajuste.Value, cultureNumber);

                    // Se agrega el item-reajuste a la base de datos
                    db.itemReajuste.Add(ir);
                }
                db.SaveChanges();
                // Si no hubo problemas se guardan los cambios
                return RedirectToAction("Index", new { idContrato = idContrato });
            }
            catch(Exception){
                ViewBag.idContrato = idContrato;
                itemReajuste ir = new itemReajuste();
                ModelState.AddModelError("", "No fue posible agregar los reajustes, verifique que no hay un reajuste para ese mes");
                return View(ir);
            }
        }

        // GET: itemReajustes/Edit/5
        public ActionResult Edit(DateTime fecha, int? idContrato) /*int? id*/
        {
            if (idContrato == null || fecha == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                ViewBag.idContrato = idContrato;

                // Se busca el reajuste en la base de datos
                var ir = db.itemReajuste.Where(p => p.ano == fecha.Year).Where(i => i.mes == fecha.Month).Where(n => n.contratoItem.idContrato == idContrato);
                if (ir == null)
                {
                    return HttpNotFound();
                }

                // Se cargan los precios con reajuste
                ViewBag.reajustesActuales = obtenerPrecios(fecha, (int)idContrato);

                return View(ir);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { idContrato = idContrato });
            }
        }

        // POST: itemReajustes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            //[Bind(Include = "id,idContratoItem,fecha,mes,ano,reajuste")] itemReajuste itemReajuste)
            [Bind(Include = "jsonItems")] string jsonItems,
            [Bind(Include = "strFechaReajuste")] string strFechaReajuste,
            [Bind(Include = "idContrato")] string idContrato)
        {

            // Especifica el formato en que está la fecha, en este caso Costa Rica (es-CR)
            IFormatProvider cultureDate = new System.Globalization.CultureInfo("es-CR", true);

            DateTime fecha2 = new DateTime();
            int nIdContrato;

            // convierte el string en datetime
            try
            {
                fecha2 = DateTime.Parse(strFechaReajuste, cultureDate, System.Globalization.DateTimeStyles.AssumeLocal);
                nIdContrato = int.Parse(idContrato);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(99, e.Message.ToString());
            }

            try
            {
                // Obtener items con su reajuste.
                dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                // Se indica que los valores numéricos están en formato inglés(en-US)
                IFormatProvider cultureNumber = new System.Globalization.CultureInfo("en-US", true);

                List<itemReajuste> irEditados = new List<itemReajuste>();
                foreach (var child in jObj.Items.Children())
                {
                    //
                    string h =child.idReajuste.Value;
                    itemReajuste ir = db.itemReajuste.Find(int.Parse(h));

                    ir.reajuste = decimal.Parse(child.reajuste.Value, cultureNumber);

                    // Se marca la modificación
                    db.Entry(ir).State = EntityState.Modified;
                }

                //Se guardan los cambios
                db.SaveChanges();

                return RedirectToAction("Periodo", new { fecha = fecha2, idContrato = idContrato });
            }
            catch (Exception)
            {
                ViewBag.idContrato = idContrato;

                var ir = db.itemReajuste.Where(p => p.ano == fecha2.Year).Where(i => i.mes == fecha2.Month).Where(n => n.contratoItem.idContrato == nIdContrato);

                ViewBag.reajustesActuales = obtenerPrecios(fecha2, nIdContrato);

                // Se almacenan los precios anteriores de cada item
                ViewBag.reajustesAnteriores = obtenerPrecios(fecha2.AddMonths(-1), nIdContrato);
                ModelState.AddModelError("", "No fue posible editar los reajustes");
                return View(ir);
            }
        }
        
        /// <summary>
        /// Elimina todos los reajustes de un mes y año específicos
        /// </summary>
        /// <param name="fecha">Fecha de la cual se extraerá el año y mes</param>
        /// <param name="idContrato">Contrato al que pertenecen los reajustes (int id)</param>
        /// <returns>Si fue correcto se redirecciona al index, de lo contrario se muestra un error</returns>
        public ActionResult DeleteAllConfirmed(DateTime fecha, int idContrato)
        {

            // Se ubica el contrato en la base de datos
            Contrato contrato = db.Contrato.Find(idContrato);

            // Lista que contendrá todos los reajustes del contrato
            var reajustes = new List<itemReajuste>();

            // Se recorren todos los item del contrato y se agregan los reajustes a la lista de reajustes
            foreach (contratoItem ci in contrato.contratoItem)
            {
                reajustes.AddRange(ci.itemReajuste);
            }

            // Se marcan los reajustes de la fecha especificada por parámetro
            var borrar = reajustes.Where(x => x.fecha == fecha);

            try
            {
                // Se borran los reajustes seleccionados de la base de datos
                db.itemReajuste.RemoveRange(borrar);

                // Se guardan los cambios en la base de datos
                db.SaveChanges();

                // Se retorna al index de reajustes
                return RedirectToAction("Index", new { idContrato = idContrato });
            }
            catch
            {
                return RedirectToAction("Periodo", new { mes = fecha.Month, ano = fecha.Year, idContrato = idContrato});
            }
        }
    }
}
