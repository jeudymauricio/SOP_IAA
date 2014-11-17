using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;

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
        /// <returns>JSON(List<Tuple<int, string, string, string, decimal, decimal, decimal>>) -> <idContratoItem, CodigoItem, Descripcion, unidad, programado, ejecutado, reajustar> </returns>
        public ActionResult CargarPeriodo(int? idContrato, string fecha)
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

            //Lista con los balances de programado, ejecuta y a reajustar por item
            // <idContratoItem, CodigoItem, Descripcion, unidad, programado, ejecutado, reajustar>
            List<Tuple<int, string, string, string, decimal, decimal, decimal>> listaItems = new List<Tuple<int, string, string, string, decimal, decimal, decimal>>();

            // Se recorren cada uno de los ítems del contrato
            foreach (var item in contrato.contratoItem)
            {
                // Se seleccionan las boletas hasta la fecha
                var bo = item.boletaItem.Where(b => (b.boleta.fecha <= fecha2));
                // Se seleccionan las ordenes de modificación válidas hasta la fecha
                var om = item.oMCI.Where(o => (o.ordenModificacion.fecha <= fecha2));

                decimal cantidadAutorizada = 0;

                cantidadAutorizada = item.cantidadAprobada;
                // Se recorren las OM correspondientes a la fecha
                foreach (var ordenModificacion in om)
                {
                    cantidadAutorizada += ordenModificacion.cantidad;
                }

                decimal cantidadLaborada = 0;
                // Se recorren todas las boletas hasta la fecha para sumar sus cantidades
                foreach (var boleta in bo)
                {
                    cantidadLaborada += boleta.cantidad;
                }

                // Se guardan los detalles en una tupla
                var detalleItem = new Tuple<int, string, string, string, decimal, decimal, decimal>(item.id, item.item.codigoItem, item.item.descripcion, item.item.unidadMedida, cantidadAutorizada, cantidadLaborada, decimal.Round(cantidadAutorizada - cantidadLaborada, 3));

                // Se agrega la tupla a la lista de items
                listaItems.Add(detalleItem);
            }

            // Retorna un JSON con los detalles del ítem
            return Json(listaItems, JsonRequestBehavior.AllowGet);
        }




        // POST: OrdenModificacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM")] ordenModificacion ordenModificacion)
        {
            if (ModelState.IsValid)
            {
                db.ordenModificacion.Add(ordenModificacion);
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = ordenModificacion.idContrato });
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
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM")] ordenModificacion ordenModificacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordenModificacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            db.ordenModificacion.Remove(ordenModificacion);
            db.SaveChanges();
            return RedirectToAction("Index");
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
