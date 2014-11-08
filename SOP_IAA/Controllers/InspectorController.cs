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
    public class InspectorController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Inspector
        public ActionResult Index()
        {
            if (access())
            {
                var inspector = db.inspector.Include(i => i.persona);
                return View(inspector.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Inspector/Details/5
        public ActionResult Details(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                inspector inspector = db.inspector.Find(id);
                if (inspector == null)
                {
                    return HttpNotFound();
                }
                return View(inspector);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Inspector/Create
        public ActionResult Create()
        {
            if (access())
            {
                var personas = db.persona
                      .Select(persona => new SelectListItem
                      {
                          Value = persona.id.ToString(),
                          Text = persona.nombre + " " + persona.apellido1 + " " + persona.apellido2
                      });
                var inspectores = db.inspector.Select(i => new SelectListItem
                {
                    Value = i.idPersona.ToString(),
                    Text = i.persona.nombre + " " + i.persona.apellido1 + " " + i.persona.apellido2
                });

                ViewBag.idPersona = new SelectList(personas.Except(inspectores), "Value", "Text");

                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Inspector/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idPersona")] inspector inspector)
        {
            if (access())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        db.inspector.Add(inspector);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Error al crear el inspector, Verifique que la persona no sea ya un inspector");
                    }
                }

                var personas = db.persona
                      .Select(persona => new SelectListItem
                      {
                          Value = persona.id.ToString(),
                          Text = persona.nombre + " " + persona.apellido1 + " " + persona.apellido2
                      });
                var inspectores = db.inspector.Select(i => new SelectListItem
                {
                    Value = i.idPersona.ToString(),
                    Text = i.persona.nombre + " " + i.persona.apellido1 + " " + i.persona.apellido2
                });

                ViewBag.idPersona = new SelectList(personas.Except(inspectores), "Value", "Text");

                return View(inspector);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Inspector/Edit/5
        public ActionResult Edit(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                inspector inspector = db.inspector.Find(id);
                if (inspector == null)
                {
                    return HttpNotFound();
                }
                ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", inspector.idPersona);
                return View(inspector);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Inspector/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idPersona")] inspector inspector)
        {
            if (access())
            {
                if (ModelState.IsValid)
                {
                    db.Entry(inspector).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.idPersona = new SelectList(db.persona, "id", "nombre", inspector.idPersona);
                return View(inspector);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Inspector/Delete/5
        public ActionResult Delete(int? id)
        {
            if (access())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                inspector inspector = db.inspector.Find(id);
                if (inspector == null)
                {
                    return HttpNotFound();
                }
                return View(inspector);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: Inspector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (access())
            {
                inspector inspector = db.inspector.Find(id);
                db.inspector.Remove(inspector);
                db.SaveChanges();
                return RedirectToAction("Index");
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
