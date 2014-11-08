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

    /// <summary>
    /// Clase parcial de Ingenieros
    /// EL resto de los métodos estan en el IngenieroController dentro de la carpeta "Mis_Controladores"
    /// </summary>
    public partial class IngenieroController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: Ingeniero
        public ActionResult Index()
        {
            if (access())
            {
                var ingeniero = db.ingeniero.Include(i => i.persona);
                return View(ingeniero.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: Ingeniero/Delete/5
        public ActionResult Delete(int? id)
        {
            if (access())
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
