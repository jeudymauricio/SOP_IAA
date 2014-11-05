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
    public class PlanInversionController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: PlanInversion
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            ViewBag.contratacion = contrato.licitacion;
            ViewBag.idContrato = contrato.id;

            // Se seleccionan solo los Planes de inversión de un contrato específico
            //var planInversion = db.planInversion.Where(p => p.idContrato == idContrato.Value).Include(p => p.Contrato);

            return View(contrato.planInversion.ToList());
        }

        // GET: PlanInversion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            return View(planInversion);
        }

        // GET: PlanInversion/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }
            planInversion pi = new planInversion();
            pi.idContrato = idContrato.Value;
            pi.fecha = DateTime.Now.AddMonths(1);
            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "Nombre");
            ViewBag.idItem = new SelectList(contrato.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
            return View(pi);
        }

        /// <summary>
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <returns>Json con los detalles del item del contrato(precio base de contrato)</returns>
        public ActionResult ItemDetalles(int? id)
        {
            // Si el id está vacío se retorna un badrequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

            // Se almacena el precio
            result.Add("precio", precio.ToString());

            // Retorna un JSON con los detalles del ítem
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: PlanInversion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idContrato,fecha,mes,ano")] planInversion planInversion)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.planInversion.Add(planInversion);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idContrato = planInversion.idContrato });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("","Ocurrió un error al ingresar el plan, verifique que no existe un plan para esa fecha");
                    return View(planInversion);
                }
            }

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            return View(planInversion);
        }

        // GET: PlanInversion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            return View(planInversion);
        }

        // POST: PlanInversion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idContrato,fecha,mes,ano")] planInversion planInversion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(planInversion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = planInversion.idContrato });
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            return View(planInversion);
        }

        // GET: PlanInversion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            return View(planInversion);
        }

        // POST: PlanInversion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            planInversion planInversion = db.planInversion.Find(id);
            db.planInversion.Remove(planInversion);
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
