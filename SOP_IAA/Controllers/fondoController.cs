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
    public partial class FondoController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: fondo
        public ActionResult Index()
        {
            if (access())
            {
                return View(db.fondo.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: fondo/Details/5
        public ActionResult Details(short? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                fondo fondo = db.fondo.Find(id);
                if (fondo == null)
                {
                    return HttpNotFound();
                }
                return View(fondo);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: fondo/Create
        public ActionResult Create()
        {
            if (access())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        // GET: fondo/Edit/5
        public ActionResult Edit(short? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                fondo fondo = db.fondo.Find(id);
                if (fondo == null)
                {
                    return HttpNotFound();
                }
                return View(fondo);
            }
            return RedirectToAction("Login", "Account");
        }


        // GET: fondo/Delete/5
        public ActionResult Delete(short? id)
        {
            if (access()) { 
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                fondo fondo = db.fondo.Find(id);
                if (fondo == null)
                {
                    return HttpNotFound();
            }
            return View(fondo);
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
