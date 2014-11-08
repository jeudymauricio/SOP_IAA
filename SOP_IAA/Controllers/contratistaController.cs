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
    public partial class contratistaController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: contratista
        public ActionResult Index()
        {
            if (access())
            {
                return View(db.contratista.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: contratista/Details/5
        public ActionResult Details(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                contratista contratista = db.contratista.Find(id);
                if (contratista == null)
                {
                    return HttpNotFound();
                }
                return View(contratista);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: contratista/Create
        public ActionResult Create()
        {
            if (access())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        // GET: contratista/Edit/5
        public ActionResult Edit(int? id)
        {
            if(access()){
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                contratista contratista = db.contratista.Find(id);
                if (contratista == null)
                {
                    return HttpNotFound();
                }
                return View(contratista);
            }
            return RedirectToAction("Login", "Account");
        }



        // GET: contratista/Delete/5
        public ActionResult Delete(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                contratista contratista = db.contratista.Find(id);
                if (contratista == null)
                {
                    return HttpNotFound();
                }
                return View(contratista);
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
