using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;
using System.Data.SqlClient;
using Newtonsoft.Json;

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

            return View(contrato.planInversion.ToList().OrderByDescending(c => c.fecha));
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
        public ActionResult Create(
            [Bind(Include = "id,idContrato,fecha,mes,ano")] planInversion planInversion,
            [Bind(Include = "jsonRutas")] string jsonRutas)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.planInversion.Add(planInversion);
                    db.SaveChanges();

                    // Obtener las rutas.
                    dynamic jObj = JsonConvert.DeserializeObject(jsonRutas);

                    foreach (var child in jObj.rutas.Children())
                    {
                        // por cada ruta se obtiene sus ítems
                        foreach (var item in child.items)
                        {
                            pICI planRutaItem = new pICI();
                            planRutaItem.idRuta = (int)child.idRuta;
                            planRutaItem.idPlanInversion = planInversion.id;
                            planRutaItem.idContratoItem = (int)item.idContratoItem;
                            planRutaItem.cantidad = (decimal)item.cantidad;

                            db.pICI.Add(planRutaItem);
                        }

                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { idContrato = planInversion.idContrato });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Ocurrió un error al ingresar el plan, reintente en un momento");

                    Contrato contrato = db.Contrato.Find(planInversion.idContrato);
                    if (contrato == null)
                    {
                        return HttpNotFound();
                    }
                    
                    ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "Nombre");
                    ViewBag.idItem = new SelectList(contrato.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");

                    return View(planInversion);
                }
            }

            // Si el modelo no es válido se vuelve a pedir los datos
            Contrato c = db.Contrato.Find(planInversion.idContrato);
            if (c == null)
            {
                return HttpNotFound();
            }

            ViewBag.idRuta = new SelectList(c.zona.ruta, "id", "Nombre");
            ViewBag.idItem = new SelectList(c.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");

            return View(planInversion);
        }

        /// <summary>
        /// Retorna el plan de inversión para una fecha específica
        /// </summary>
        /// <param name="fecha">Fecha del periodo a buscar</param>
        /// <param name="idContrato">id del contrato</param>
        /// <returns>Plan de inversión para la fecha recibida</returns>
        public ActionResult Periodo(DateTime fecha, int? idContrato)
        {
            if (idContrato == null || fecha == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                ViewBag.idContrato = idContrato;

                var pi = db.planInversion.Where(p => (p.ano == fecha.Year && p.mes == fecha.Month)).Where(i => i.idContrato == idContrato);

                if (pi == null)
                {
                    return HttpNotFound();
                }

                return View(pi.First());
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { idContrato = idContrato });
            }
        }

        // GET: PlanInversion/Edit/5
        public ActionResult Edit(int? idPlan)
        {
            if (idPlan == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            planInversion planInversion = db.planInversion.Find(idPlan);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            Contrato contrato = db.Contrato.Find(planInversion.idContrato);
            ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "Nombre");
            ViewBag.idItem = new SelectList(contrato.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
            ViewBag.idContrato = planInversion.idContrato;//new SelectList(db.Contrato, "id", "licitacion", planInversion.idContrato);
            
            return View(planInversion);
        }

        // POST: PlanInversion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "id,idContrato,fecha,mes,ano")] planInversion planInversion,
            [Bind(Include = "jsonRutas")] string jsonRutas)
        {
            Contrato contrato;
            
            if (ModelState.IsValid)
            {
                try
                {
                    Repositorio<planInversion> rep = new Repositorio<SOP_IAA_DAL.planInversion>();
                    if (! rep.Actualizar(planInversion))
                    {
                        throw new Exception();
                    }
                    /*db.Entry(planInversion).State = EntityState.Modified;
                    db.SaveChanges();*/

                    // Se busca el plan de inversión a modificar
                    planInversion planInversion2 = db.planInversion.Find(planInversion.id);

                    // Se eliminan los elementos de la base de datos
                    db.pICI.RemoveRange(planInversion2.pICI);
                    db.SaveChanges();

                    // Objeto dinámico para las rutas y sus items
                    dynamic jObj = JsonConvert.DeserializeObject(jsonRutas);

                    foreach (var child in jObj.rutas.Children())
                    {
                        // por cada ruta se obtiene sus ítems
                        foreach (var item in child.items)
                        {
                            pICI planRutaItem = new pICI();
                            planRutaItem.idRuta = (int)child.idRuta;
                            planRutaItem.idPlanInversion = planInversion.id;
                            planRutaItem.idContratoItem = (int)item.idContratoItem;
                            planRutaItem.cantidad = (decimal)item.cantidad;

                            db.pICI.Add(planRutaItem);
                        }

                        db.SaveChanges();
                    }

                    return RedirectToAction("Periodo", new { fecha = planInversion.fecha, idContrato = planInversion.idContrato });
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar el plan, verifique que no exista un plan para esa fecha");

                    planInversion = db.planInversion.Find(planInversion.id);
                    if (planInversion == null)
                    {
                        return HttpNotFound();
                    }
                    contrato = db.Contrato.Find(planInversion.idContrato);
                    ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "Nombre");
                    ViewBag.idItem = new SelectList(contrato.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
                    ViewBag.idContrato = planInversion.idContrato;

                    return View(planInversion);
                }
            }

            planInversion = db.planInversion.Find(planInversion.id);
            if (planInversion == null)
            {
                return HttpNotFound();
            }
            contrato = db.Contrato.Find(planInversion.idContrato);
            ViewBag.idRuta = new SelectList(contrato.zona.ruta, "id", "Nombre");
            ViewBag.idItem = new SelectList(contrato.contratoItem.Where(x => x.item.id == x.idItem), "id", "item.codigoItem");
            ViewBag.idContrato = planInversion.idContrato;

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

        /// <summary>
        /// Acción que elimina un plan y sus detalles de la base de datos
        /// </summary>
        /// <param name="idPlan">id del plan a eliminar (int)</param>
        /// <returns>Si tuvo éxito retorna al index del contrato al que pertenecía el plan</returns>
        public ActionResult DeleteAllConfirmed(int? idPlan)
        {
            // Se busca el plan en la base de datos
            planInversion pi = db.planInversion.Find(idPlan);

            if (pi == null){
                return HttpNotFound();
            }

            int idContrato = pi.idContrato;
            try
            {
                db.pICI.RemoveRange(pi.pICI);
                db.planInversion.Remove(pi);
                db.SaveChanges();

                return RedirectToAction("index", new { idContrato = idContrato });
            }
            catch (Exception e)
            {
                return RedirectToAction("Periodo", new { fecha = pi.fecha, idContrato = idContrato });
            }
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
