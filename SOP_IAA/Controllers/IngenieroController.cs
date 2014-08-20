﻿using System;
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
    public class IngenieroController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Ingeniero
        public ActionResult Index()
        {
            var ingeniero = db.ingeniero.Include(i => i.persona);
            return View(ingeniero.ToList());
        }

        // GET: Ingeniero/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingeniero ingeniero = db.ingeniero.Find(id);
            if (ingeniero == null)
            {
                return HttpNotFound();
            }
            return View(ingeniero);
        }

        // GET: Ingeniero/Create
        public ActionResult Create()
        {
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre");
            return View();
        }

        // POST: Ingeniero/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona,descripcion,departamento,rol")] ingeniero ingeniero)
        {
            if (ModelState.IsValid)
            {
                db.ingeniero.Add(ingeniero);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }

        // GET: Ingeniero/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingeniero ingeniero = db.ingeniero.Find(id);
            if (ingeniero == null)
            {
                return HttpNotFound();
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }

        // POST: Ingeniero/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona,descripcion,departamento,rol")] ingeniero ingeniero)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ingeniero).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", ingeniero.idPersona);
            return View(ingeniero);
        }

        // GET: Ingeniero/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ingeniero ingeniero = db.ingeniero.Find(id);
            if (ingeniero == null)
            {
                return HttpNotFound();
            }
            return View(ingeniero);
        }

        // POST: Ingeniero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ingeniero ingeniero = db.ingeniero.Find(id);
            db.ingeniero.Remove(ingeniero);
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
