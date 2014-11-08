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
    public partial class ItemReajusteController :  Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: itemReajustes
        public ActionResult Index(int? idContrato)
        {
            if (access())
            {
                if (idContrato == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var ir = db.itemReajuste.Where(p => p.contratoItem.idContrato == idContrato).OrderByDescending(f => f.fecha);
                var v = ir.GetType();
                ViewBag.contrato = idContrato;
                return View(ir);
            }
             return RedirectToAction("Login", "Account");
        }

        private Boolean access()
        {
            if (Session["CurrentSession"] == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
