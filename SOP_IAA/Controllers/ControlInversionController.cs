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

            ViewBag.contratacion = contrato.licitacion;
            ViewBag.idContrato = contrato.id;

            // En caso de que haya un error (esto en una vista hija) se le muestra al usuario
            if (String.IsNullOrEmpty(error) || String.IsNullOrWhiteSpace(error))
            {
                ModelState.AddModelError("", error.ToString());
            }
            
            return View(contrato);
        }

        //
        //public ActionResult 
        // 
        public ActionResult BalanceInversion(int? idContrato, DateTime fecha)
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

            // Se recorren cada uno de los ítems del contrato
            foreach(var item in contrato.contratoItem){
                // Se selecciona el plan de inversion de la ficha si existe
                var pi = item.pICI.Where(p => p.planInversion.fecha == fecha).FirstOrDefault();
                // Se seleccionan las boletas del mes y año
                var bo = item.boletaItem.Where(b => (b.boleta.fecha.Month == fecha.Month && b.boleta.fecha.Year == fecha.Year));


            }





            // Se selecciona el plan de inversión de la fecha
            planInversion pI = contrato.planInversion.Where(pi => pi.fecha == fecha).FirstOrDefault();
            if (pI == null)
            {
                return RedirectToAction("Index", new { idContrato = idContrato, error = "No existe un plan de inversión para la fecha" });
            }

            // Se seleccionan todas las boletas dentro del rango de fecha
            var boletas = contrato.boleta.Where(b => b.fecha == fecha);

            //Lista con los balances de programado, ejecuta y a reajustar por item
            List<Tuple<string,string,string,decimal,decimal,decimal>> listaItems = new List<Tuple<string,string,string,decimal,decimal,decimal>>();

            // Se recorren cada uno de los ítems programados
            foreach (var item in pI.pICI)
            {

            }

            return View();
        }
    }
}