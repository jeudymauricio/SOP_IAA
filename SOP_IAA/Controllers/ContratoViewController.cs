using SOP_IAA.Models;
using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;

namespace SOP_IAA.Controllers
{
    public class ContratoViewController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: ContratoParent
        public ActionResult Index()
        {
            var model = new ContratoViewModels();

            var contrato = db.Contrato.Include(c => c.contratista).Include(c => c.fondo).Include(c => c.zona);
            var ingeniero = db.ingeniero.Include(i => i.persona);

            model.ListContratoIdContratista = new SelectList(db.contratista, "id", "nombre");
            model.ListContratoIdFondo = new SelectList(db.fondo, "id", "nombre");
            model.ListContratoIdZona = new SelectList(db.zona, "id", "nombre");
            model.ListIngeniero = new SelectList(db.persona, "id", "nombre");

            return View(model);
        }

        public ActionResult MyEditActionOne(ContratoViewModels model)
        {
            if (ModelState.IsValid)
            {
                return View("Index", model);
            }

            throw new Exception("My Model state is not valid");
        }
    }
}