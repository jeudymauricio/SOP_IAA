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
    public partial class ContratoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Contrato
        public ActionResult Index()
        {
            if(access())
            {
                var contrato = db.Contrato.Include(c => c.contratista).Include(c => c.fondo).Include(c => c.zona);
                return View(contrato.ToList());
            }

            return RedirectToAction("Login", "Account");
        }

        // GET: Contrato/Details/5
        public ActionResult Details(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Contrato contrato = db.Contrato.Find(id);
                if (contrato == null)
                {
                    return HttpNotFound();
                }
                return View(contrato);
            }
            return RedirectToAction("Login", "Account");
        }
        
        // GET: Contrato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Contrato contrato = db.Contrato.Find(id);
                if (contrato == null)
                {
                    return HttpNotFound();
                }
                return View(contrato);
            }
         return RedirectToAction("Login", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private Boolean access() {
            if (Session["CurrentSession"] == null)
            {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
