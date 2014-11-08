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
    public partial class ZonaController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: zona
        public ActionResult Index()
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            return View(db.zona.ToList());
        }

        // GET: zona/Details/5
        public ActionResult Details(short? id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zona zona = db.zona.Find(id);
            if (zona == null)
            {
                return HttpNotFound();
            }
            return View(zona);
        }

        // GET: zona/Create
        public ActionResult Create()
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        /*
        // POST: zona/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre")] zona zona)
        {
            if (ModelState.IsValid)
            {
                db.zona.Add(zona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(zona);
        }*/

        // GET: zona/Edit/5
        public ActionResult Edit(short? id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zona zona = db.zona.Find(id);
            if (zona == null)
            {
                return HttpNotFound();
            }
            return View(zona);
        }

        /*
        // POST: zona/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nombre")] zona zona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(zona);
        }*/

        // GET: zona/Delete/5
        public ActionResult Delete(short? id)
        {
            if (!access())
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            zona zona = db.zona.Find(id);
            if (zona == null)
            {
                return HttpNotFound();
            }
            return View(zona);
        }

        /*
        // POST: zona/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            zona zona = db.zona.Find(id);
            db.zona.Remove(zona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
