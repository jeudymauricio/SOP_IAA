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
using System.Web.Script.Serialization;

namespace SOP_IAA.Controllers
{
    public class OrdenModificacionController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: OrdenModificacion
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            //var ordenModificacion = db.ordenModificacion.Include(o => o.Contrato);
            //var ordenModificacion = contrato.ordenModificacion;

            return View(contrato/*ordenModificacion.ToList()*/);
        }

        // GET: OrdenModificacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }

            // Se cargan los items
            ViewBag.items = CargarCantidadesEdit(ordenModificacion.idContrato, ordenModificacion.fecha);

            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contrato = db.Contrato.Find(idContrato);
            
            // Se carga la lista de Items del contrato
            List<object> listItem = new List<object>();
            foreach (var ci in contrato.contratoItem)
            {
                listItem.Add(new Tuple<int, string>(ci.id, ci.item.codigoItem));
            }
            ViewBag.idItem = new SelectList(listItem, "item1", "item2");

            ViewBag.idContrato = idContrato;
            ordenModificacion om = new ordenModificacion();
            
            om.idContrato = idContrato.Value;
            om.fecha = DateTime.Now;
            
            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            return View(om);
        }

        /// <summary>
        /// Retorna un JSON con la comparación de las cantidades programadas vs las cantidades realizadas(según las boletas)
        /// y todo según una fecha específica
        /// </summary>
        /// <param name="idContrato">id del contrato a consultar</param>
        /// <param name="fecha">fecha a consultar</param>
        /// <returns>JSON(List<Tuple<int, string, string, string, decimal, decimal, decimal>>) -> <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
        public ActionResult CargarCantidades(int? idContrato, string fecha)
        {
            if (idContrato == null)
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

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            // Lista con las cantidades originales, autorizadas, ejecutadas y disponibles por item
            // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
            List<Tuple<int, string, string, string, decimal, decimal, decimal>> listaItems = new List<Tuple<int, string, string, string, decimal, decimal, decimal>>(); //decimal.Round(cantidadOriginal - cantidadRealizada, 3)

            // Se recorren cada uno de los ítems del contrato
            foreach (var item in contrato.contratoItem)
            {
                // Se seleccionan las boletas hasta la fecha
                var bo = item.boletaItem.Where(b => (b.boleta.fecha < fecha2));
                // Se seleccionan las ordenes de modificación válidas hasta la fecha
                var om = item.oMCI.Where(o => (o.ordenModificacion.fecha < fecha2));

                decimal cantidadOriginal = 0;

                cantidadOriginal = item.cantidadAprobada;

                decimal cantidadOMs = 0;
                // Se recorren las OM correspondientes a la fecha
                foreach (var ordenModificacion in om)
                {
                    cantidadOMs += ordenModificacion.cantidad;
                }

                decimal cantidadRealizada = 0;
                // Se recorren todas las boletas hasta la fecha para sumar sus cantidades
                foreach (var boleta in bo)
                {
                    cantidadRealizada += boleta.cantidad;
                }

                // Se guardan los detalles en una tupla // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
                var detalleItem = new Tuple<int, string, string, string, decimal, decimal, decimal>(item.id, item.item.codigoItem, item.item.descripcion, item.item.unidadMedida, cantidadOriginal, cantidadOMs, cantidadRealizada);

                // Se agrega la tupla a la lista de items
                listaItems.Add(detalleItem);
            }

            // Retorna un JSON con los detalles del ítem
            return Json(listaItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna una lista de tuplas con la comparación de las cantidades programadas vs las cantidades realizadas(según las boletas)
        /// y todo hasta una fecha específica
        /// </summary>
        /// <param name="idContrato">id del contrato a consultar</param>
        /// <param name="fecha">fecha a consultar</param>
        /// <returns>List<Tuple<int, string, string, string, decimal, decimal, decimal>> -> <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
        public List<Tuple<int, string, string, string, decimal, decimal, decimal>> CargarCantidadesEdit(int idContrato, DateTime fecha)
        {

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return null;
            }

            // Lista con las cantidades originales, autorizadas, ejecutadas y disponibles por item
            // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
            List<Tuple<int, string, string, string, decimal, decimal, decimal>> listaItems = new List<Tuple<int, string, string, string, decimal, decimal, decimal>>(); //decimal.Round(cantidadOriginal - cantidadRealizada, 3)

            // Se recorren cada uno de los ítems del contrato
            foreach (var item in contrato.contratoItem)
            {
                // Se seleccionan las boletas hasta la fecha
                var bo = item.boletaItem.Where(b => (b.boleta.fecha < fecha));
                // Se seleccionan las ordenes de modificación válidas hasta la fecha
                var om = item.oMCI.Where(o => (o.ordenModificacion.fecha < fecha));

                decimal cantidadOriginal = 0;

                cantidadOriginal = item.cantidadAprobada;

                decimal cantidadOMs = 0;
                // Se recorren las OM correspondientes a la fecha
                foreach (var ordenModificacion in om)
                {
                    cantidadOMs += ordenModificacion.cantidad;
                }

                decimal cantidadRealizada = 0;
                // Se recorren todas las boletas hasta la fecha para sumar sus cantidades
                foreach (var boleta in bo)
                {
                    cantidadRealizada += boleta.cantidad;
                }

                // Se guardan los detalles en una tupla // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
                var detalleItem = new Tuple<int, string, string, string, decimal, decimal, decimal>(item.id, item.item.codigoItem, item.item.descripcion, item.item.unidadMedida, cantidadOriginal, cantidadOMs, cantidadRealizada);

                // Se agrega la tupla a la lista de items
                listaItems.Add(detalleItem);
            }

            // Retorna los detalles del ítem
            return listaItems;
        }

        // POST: OrdenModificacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM")] ordenModificacion ordenModificacion,
            [Bind(Include = "jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                // Obtener items.
                dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                // Formato en que están los decimales
                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                try
                {
                    foreach (var child in jObj.Items.Children())
                    {
                        //Creación del contratoItem-OM.
                        oMCI om = new oMCI();
                        om.idContratoItem = int.Parse(child.idContratoItem.Value);
                        om.cantidad = decimal.Parse(child.cantidad.Value, culture);

                        // Se agrega la om de item a la OM global
                        ordenModificacion.oMCI.Add(om); // This will auto-fill the Foreign Key during commit.
                    }
                    // Se agrega la OM a la BD
                    db.ordenModificacion.Add(ordenModificacion);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { idContrato = ordenModificacion.idContrato });
                }
                catch (Exception e)
                {
                    // Notify Error
                    ModelState.AddModelError("", "No se pudo agregar la OM");
                }
            }

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            ViewBag.idContrato = ordenModificacion.idContrato;
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);

            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }

            // Se cargan las cantidades
            ViewBag.items = CargarCantidadesEdit(ordenModificacion.idContrato, ordenModificacion.fecha);
            
            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM")] ordenModificacion ordenModificacion,
            [Bind(Include = "jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener items.
                    dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                    // Formato en que están los decimales
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);


                    foreach (var child in jObj.Items.Children())
                    {
                        int idContratoItem = int.Parse(child.idContratoItem.Value);
                        int idOrdenModificacion = int.Parse(child.idOrdenModificacion.Value);

                        var omci = db.oMCI.Find(idOrdenModificacion, idContratoItem);
                        if (omci == null)
                        {
                            continue;
                        }

                        omci.cantidad = decimal.Parse(child.cantidad.Value, culture);
                        db.Entry(omci).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    db.Entry(ordenModificacion).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index", new { idContrato = ordenModificacion.idContrato });
                }
                catch (Exception)
                {
                    //Notify error
                    ModelState.AddModelError("", "No se pudo modificar la OM ");
                }
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            int _idContrato = ordenModificacion.idContrato;
            try
            {
                db.oMCI.RemoveRange(ordenModificacion.oMCI);
                db.ordenModificacion.Remove(ordenModificacion);
                db.SaveChanges();
            }
            catch (Exception)
            {
                //Notify Error
            }
            return RedirectToAction("Index", new { idContrato = _idContrato });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
