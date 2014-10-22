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
    public partial class ItemReajusteController : Controller
    {
        //GET: itemReajuste/Periodo
        public ActionResult Periodo(int? mes, int? ano, int? idContrato)
        {
            if (idContrato == null || mes==null || ano==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.idContrato = idContrato;
            var ci = db.contratoItem.Where(cii => cii.idContrato == idContrato);
            var ir = db.itemReajuste.Where(p => p.ano == ano).Where(i => i.mes==mes).Where(n => n.contratoItem.idContrato == idContrato);

            // Se almacenan los precios anteriores de cada item
            ViewBag.reajustesAnteriores = obtenerPrecioAnterior(ir.First().fecha, (int)idContrato);

            return View(ir);
        }

        /// <summary>
        /// Con base a una fecha, retorna los reajustes del mes anterior de cada item o en su defecto el precio de contrato
        /// </summary>
        /// <param name="fecha">Fecha inicial a la que se le debe restar un mes</param>
        /// <param name="idContrato">Contrato al que pertenecen los reajustes</param>
        /// <returns>Lista de reajustes de cada item en el mes</returns>
        public Dictionary<int, decimal> obtenerPrecioAnterior(DateTime fecha, int idContrato)
        {
            // Fecha a buscar
            DateTime fechaAnterior = fecha.AddMonths(-1);
            
            // Lista de items del contrato
            var contratoItem = db.contratoItem.Where(ci => ci.idContrato == idContrato);

            // Diccionario que contendrá el id del item en el contrato y su respectivo reajuste o precio base  <int> idContratoItem  <decimal> Precio
            Dictionary<int, decimal> preciosAnteriores = new Dictionary<int, decimal>();

            // Se procede a guardar los precios correspondientes a cada item del contrato
            foreach (contratoItem ci in contratoItem)
            {   
                // Se verifica si el item posee reajustes
                if (ci.itemReajuste.Count() > 0)
                {
                    // Si tiene reajustes se procede a colocar el precio del mas cercano
                    var temp = ci.itemReajuste.OrderByDescending(ir => ir.fecha);
                    
                    // Si la fecha es menor que la del primer reajuste, se asigna el precio establecido en el contrato sin reajuste
                    if (fechaAnterior < temp.ToList().Last().fecha)
                    {
                        preciosAnteriores.Add(ci.id, ci.precioUnitario);
                    }
                    else // Si la fecha es mayor que la del primer reajuste, se busca el reajuste o en su defecto el mas cercano
                    {
                        // Se asigna el precio del reajuste mas cercano a ese mes (o el de ese mes)
                        decimal precio = temp.First().precioReajustado;
                        foreach (var ri in temp)
                        {
                            if (ri.fecha <= fechaAnterior)
                            {
                                precio = ri.precioReajustado;
                                break;
                            }
                        }
                        preciosAnteriores.Add(ci.id, precio);
                    }
                }
                else
                {
                    // Si no hay reajutes se coloca el precio base del contrato
                    preciosAnteriores.Add(ci.id, ci.precioUnitario);
                }
            }

            // Se retorna el diccionario con los precios correspondientes a los items segun la fecha anterior
            return preciosAnteriores;
        }

        // GET: itemReajustes/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.idContratoItem = new SelectList(db.contratoItem.Where(p => p.idContrato == idContrato)
                .Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
            ViewBag.idContrato = idContrato;

            return View();
        }

        /// <summary>
        /// Funció que basada en un idContrato y fecha, devuelve los items y sus respectivos precios para esa fecha
        /// </summary>
        /// <param name="idContrato"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public ActionResult cargarItems(int? idContrato, string fecha)
        {
            if ((idContrato == null)||(string.IsNullOrWhiteSpace(fecha)) )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Diccionario que contendrá los detalles de cada item consultado.
            Dictionary<string,string> jObj;

            // Lista que contendrá los detalles de cada item en tuplas
            List<Tuple<string, string, string, decimal,string>> itemList = new List<Tuple<string, string, string, decimal,string>>();

            // Se selecciona el contrato de la base de datos
            Contrato contrato = db.Contrato.Find(idContrato);

            // Por cada item del contrato se debe cargar su precio a la fecha
            foreach (contratoItem ci in contrato.contratoItem)
            {
                // Obtener los detalles del item.
                jObj = getItemDetail(ci.id, fecha);
                
                // Se guardan los detalles en una tupla
                var itemDetalle = new Tuple<string, string, string, decimal, string>(jObj["codigoItem"], jObj["descripcion"], jObj["unidadMedida"], Decimal.Parse(jObj["precioReajustado"]), jObj["idContratoItem"]);

                // Se agrega la tupla a la lista de items
                itemList.Add(itemDetalle);
            }

            // Retorna un JSON con los detalles de cada ítem
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <param name="fecha">fecha de la boleta</param>
        /// <returns>Json con los detalles del item del contrato(incluido su reajuste mas cercano)</returns>
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

            // Se busca dentro de los reajustes de precio el que le corresponde a la fecha de la boleta
            //var itemReajustado = db.itemReajuste.Where(ir => (ir.ano == fecha2.Year) && (ir.mes == fecha2.Month) && (ir.idContratoItem == id));
            var itemReajustado = db.itemReajuste.Where(ir => ir.idContratoItem == id);
            if (itemReajustado.Count() > 0)
            {
                itemReajustado = itemReajustado.OrderByDescending(ir => ir.fecha);

                // Si la fecha es menor que la del primer reajuste, se asigna el precio establecido en el contrato sin reajuste
                if (fecha2 < itemReajustado.ToList().Last().fecha)
                {
                    result.Add("precioReajustado", ci.precioUnitario.ToString());
                }
                else // Si la fecha es mayor que la del primer reajuste, se busca el reajuste o en su defecto el mas cercano
                {
                    // Se asigna el precio del reajuste mas cercano a ese mes (o el de ese mes)
                    decimal precio = itemReajustado.First().precioReajustado;
                    foreach (var ir in itemReajustado)
                    {
                        if (ir.fecha <= fecha2)
                        {
                            precio = ir.precioReajustado;
                            break;
                        }
                    }
                    result.Add("precioReajustado", precio.ToString());
                }

            }
            else // Si no hay reajustes se procede a poner el precio estipulado en el contrato.
            {
                result.Add("precioReajustado", ci.precioUnitario.ToString());
            }

            // Retorna un JSON con los detalles del ítem
            return result;
        }

        /// <summary>
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <param name="fecha">fecha de la boleta</param>
        /// <returns>Json con los detalles del item del contrato(incluido su reajuste mas cercano)</returns>
        public ActionResult ItemDetalles(int? id, string fecha)
        {
            // Si el id o la fecha están vacíos se retorna un badrequest
            if ((id == null) || (string.IsNullOrWhiteSpace(fecha)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                return new HttpStatusCodeResult(99, e.Message.ToString());
            }

            // Selecciona de la base de datos el contratoItem correspondiente
            contratoItem ci = db.contratoItem.Find(id);

            // Si está nulo, quiere decir que no existe el item en el contrato
            if (ci == null)
            {
                return HttpNotFound();
            }

            /// Se inicia con la creacion de un diccionario con toda la información pertinente del item
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("codigoItem", ci.item.codigoItem);
            result.Add("descripcion", ci.item.descripcion);
            result.Add("unidadMedida", ci.item.unidadMedida);

            // Se busca dentro de los reajustes de precio el que le corresponde a la fecha de la boleta
            //var itemReajustado = db.itemReajuste.Where(ir => (ir.ano == fecha2.Year) && (ir.mes == fecha2.Month) && (ir.idContratoItem == id));
            var itemReajustado = db.itemReajuste.Where(ir => ir.idContratoItem == id);
            if (itemReajustado.Count() > 0)
            {
                itemReajustado = itemReajustado.OrderByDescending(ir => ir.fecha);

                // Si la fecha es menor que la del primer reajuste, se asigna el precio establecido en el contrato sin reajuste
                if (fecha2 < itemReajustado.ToList().Last().fecha)
                {
                    result.Add("precioReajustado", ci.precioUnitario.ToString("C4", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR")));
                }
                else // Si la fecha es mayor que la del primer reajuste, se busca el reajuste o en su defecto el mas cercano
                {
                    // Se asigna el precio del reajuste mas cercano a ese mes (o el de ese mes)
                    decimal precio = itemReajustado.First().precioReajustado;
                    foreach (var ir in itemReajustado)
                    {
                        if (ir.fecha <= fecha2)
                        {
                            precio = ir.precioReajustado;
                            break;
                        }
                    }
                    result.Add("precioReajustado", precio.ToString("C4", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR")));
                }

            }
            else // Si no hay reajustes se procede a poner el precio estipulado en el contrato.
            {
                result.Add("precioReajustado", ci.precioUnitario.ToString("C4", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR")));
            }

            // Retorna un JSON con los detalles del ítem
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: itemReajustes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "id,idContratoItem,fecha,mes,ano,reajuste")] itemReajuste itemReajuste,
            [Bind(Include = "jsonItems")] string jsonItems,
            [Bind(Include = "strFechaReajuste")] string strFechaReajuste)
        {
            if (ModelState.IsValid)
            {
                var itemReajustado = db.itemReajuste.Where(ir => ir.idContratoItem == itemReajuste.idContratoItem);
                var contratoItem = db.contratoItem.Where(ci => ci.id == itemReajuste.idContratoItem);
                if (itemReajustado.Count() > 0)
                {
                    itemReajustado = itemReajustado.OrderByDescending(ir => ir.fecha);
                    itemReajuste _temp = itemReajustado.First();

                    decimal precioBase = _temp.precioReajustado;
                    decimal reajustado = (precioBase * itemReajuste.reajuste) + precioBase;
                    itemReajuste.precioReajustado = reajustado;
                }
                else // Si no hay reajustes se procede a poner el precio estipulado en el contrato.
                {
                    decimal precioBase = contratoItem.First().precioUnitario;
                    decimal reajustado = (precioBase * itemReajuste.reajuste) + precioBase;
                    itemReajuste.precioReajustado = reajustado;
                }

                db.itemReajuste.Add(itemReajuste);
                db.SaveChanges();

                // Obtener items.
                /*dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                foreach (var child in jObj.Items.Children())
                {
                    //Creación del boleta-item.
                    boletaItem bi = new boletaItem();

                    bi.idBoleta = boleta.id;
                    bi.idContratoItem = int.Parse(child.idItemContrato.Value);
                    bi.cantidad = decimal.Parse(child.cantidad.Value, culture);
                    bi.costoTotal = decimal.Parse(child.costoTotal.Value, culture);
                    bi.redimientos = decimal.Parse(child.redimientos.Value, culture);
                    bi.precioUnitarioFecha = decimal.Parse(child.precio.Value, culture);

                    // Se agrega el boleta-item a la base de datos
                    db.boletaItem.Add(bi);
                    db.SaveChanges();
                }*/

                //return RedirectToAction("Index/?id"+itemReajuste.contratoItem.idContrato);
                return RedirectToAction("Index", new {id= contratoItem.First().idContrato });
            }

            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id", itemReajuste.idContratoItem);
            return View(itemReajuste);
        }

        // POST: itemReajustes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContratoItem,fecha,mes,ano,reajuste")] itemReajuste itemReajuste)
        {
            if (ModelState.IsValid)
            {

                db.Entry(itemReajuste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idContratoItem = new SelectList(db.contratoItem, "id", "id", itemReajuste.idContratoItem);
            return View(itemReajuste);
        }

        // POST: itemReajustes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            itemReajuste itemReajuste = db.itemReajuste.Find(id);
            db.itemReajuste.Remove(itemReajuste);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
