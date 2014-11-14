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
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <param name="fecha">fecha de la boleta</param>
        /// <returns>Json con los detalles del item del contrato(incluido su reajuste si lo hay)</returns>
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
            //result.Add("idContratoItem", ci.id.ToString());
            result.Add("codigoItem", ci.item.codigoItem);
            result.Add("descripcion", ci.item.descripcion);
            result.Add("unidadMedida", ci.item.unidadMedida);

            // Precio base del item
            decimal precio = ci.precioUnitario;

            // Se busca si hay reajuste para ese mes
            var itemReajustado = db.itemReajuste.Where(ir => (ir.ano == fecha2.Year) && (ir.mes == fecha2.Month) && (ir.idContratoItem == ci.id));

            // Si hay reajuste se aplica
            if (itemReajustado.Count() > 0)
            {
                // Reajuste del mes
                decimal reajuste = itemReajustado.First().reajuste;

                // Se aplica el reajuste
                precio = decimal.Round(precio * reajuste + precio, 4);
            }

            // Se almacena el precio
            result.Add("precioReajustado", precio.ToString());

            // Retorna un JSON con los detalles del ítem
            return Json(result, JsonRequestBehavior.AllowGet);
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
