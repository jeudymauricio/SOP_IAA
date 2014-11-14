using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SOP_IAA.Controllers
{
    public class ControlInversionController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: ControlInversion
        public ActionResult Index(int? idContrato, string error)
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
            
            // LIsta de fechas desde el inicio del contrato hasta la fecha
            var dates = new List<DateTime>();
            for (var date = contrato.fechaInicio; date <= DateTime.Now; date = date.AddMonths(1))
            {
                dates.Add(date);
            }

            ViewBag.fechas = dates.OrderByDescending(x => x).ToList();
            ViewBag.contratacion = contrato.licitacion;
            ViewBag.idContrato = contrato.id;

            // En caso de que haya un error (esto en una vista hija) se le muestra al usuario
            if ((!String.IsNullOrEmpty(error)) ||(!String.IsNullOrWhiteSpace(error)))
            {
                ModelState.AddModelError("", error.ToString());
            }
            
            return View(contrato);
        }

        /// <summary>
        /// Retorna la comparación de las cantidades programadas vs las cantidades realizadas(según las boletas)
        /// y todo según una fecha específica
        /// </summary>
        /// <param name="idContrato"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public ActionResult Periodo(int? idContrato, DateTime fecha)
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

            //Lista con los balances de programado, ejecuta y a reajustar por item
            // <CodigoItem, Descripcion, unidad, programado, ejecutado, reajustar>
            List<Tuple<string, string, string, decimal, decimal, decimal>> listaItems = new List<Tuple<string, string, string, decimal, decimal, decimal>>();

            // Se recorren cada uno de los ítems del contrato
            foreach(var item in contrato.contratoItem){
                // Se selecciona del plan de inversion de la fecha, todos las menciones del ítem(porque el item puede estar planeado en mas de una ruta)
                var pi = item.pICI.Where(p => (p.planInversion.fecha.Month == fecha.Month && p.planInversion.fecha.Year == fecha.Year));
                // Se seleccionan las boletas del mes y año
                var bo = item.boletaItem.Where(b => (b.boleta.fecha.Month == fecha.Month && b.boleta.fecha.Year == fecha.Year));

                decimal cantidadProgramada = 0;
                
                // Si estaba programado se suman las cantidades
                foreach ( var planInversionSubItem in pi)
                {
                    cantidadProgramada += planInversionSubItem.cantidad;
                }

                decimal cantidadLaborada = 0;
                // Se recorren todas las boletas para sumar sus cantidades
                foreach (var boleta in item.boletaItem)
                {
                    cantidadLaborada += boleta.cantidad;
                }

                // Se guardan los detalles en una tupla
                var detalleItem = new Tuple<string, string, string, decimal, decimal, decimal>(item.item.codigoItem, item.item.descripcion, item.item.unidadMedida, cantidadProgramada, cantidadLaborada, decimal.Round(cantidadProgramada - cantidadLaborada, 3));

                // Se agrega la tupla a la lista de items
                listaItems.Add(detalleItem);
            }

            ViewBag.listaItems = listaItems;
            ViewBag.idContrato = idContrato;
            ViewBag.fecha = fecha;

            return View();
        }
    }
}