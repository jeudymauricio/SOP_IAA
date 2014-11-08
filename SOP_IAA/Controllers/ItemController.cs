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
    public partial class ItemController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Item
        public ActionResult Index()
        {
            if (access())
            {
                return View(db.item.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Item/Details/5
        public ActionResult Details(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                item item = db.item.Find(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return View(item);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Item/Create
        public ActionResult Create()
        {
            if (access())
            {
                return View();
            } 
            return RedirectToAction("Login", "Account");
        }


        // GET: Item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                item item = db.item.Find(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return View(item);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                item item = db.item.Find(id);
                if (item == null)
                {
                    return HttpNotFound();
                }
                return View(item);
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
