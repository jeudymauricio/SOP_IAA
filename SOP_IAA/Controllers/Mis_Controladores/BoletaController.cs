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
    public partial class BoletaController : Controller
    {

        // GET: Boleta
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.idContrato = idContrato;

            var boleta = db.boleta.Where(b => b.idContrato == idContrato).Include(b => b.fondo).Include(b => b.inspector).Include(b => b.proyecto_estructura).Include(b => b.ruta);

            return View(boleta.ToList());
        }

        // GET: Boleta/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Se busca el contrato, esto para cargar el fondo y las rutas correspondientes al contrato
            Contrato contrato = db.Contrato.Find(idContrato);

            // Si no existe el contrato se retorna un 'No encontrado'
            if (contrato == null)
            {
                return HttpNotFound();
            }

            // Selecciona solo los fondos que existen el el contrato, siempre va a ser uno
            var fondoContrato = db.fondo.Where(f => f.id == contrato.fondo.id);
            ViewBag.idFondo = new SelectList(fondoContrato, "id", "nombre");

            // Se carga la lista de inspectores del sistema
            var mquery = (
                from p in db.persona
                join i in db.inspector
                on p.id equals i.idPersona
                select new SelectListItem
                {
                    Value = p.id.ToString(),
                    Text = p.nombre + " " + p.apellido1 + " " + p.apellido2
                }
            );
            ViewBag.idInspector = new SelectList(mquery, "Value", "Text");

            // Se cargan las rutas correspondientes a la zona del contrato
            var rutasContrato = new SelectList(contrato.zona.ruta, "id", "nombre");
            ViewBag.idRuta = rutasContrato;


            if (rutasContrato.ToList().Count != 0)
            {
                // Se selecciona de la bd los proyectos estructuras y se convierten en una lista
                ViewBag.idProyecto_Estructura = db.proyecto_estructura.ToList()
                    // Se selecciona de la lista sólo los de la primer ruta que se carga al dropdown
                    .Where(pe => pe.idRuta == int.Parse(rutasContrato.ToList().ElementAt(0).Value))
                    // Se convirte a lista donde se toman solamente el id y la descripción del pe
                    .Select(c => new SelectListItem
                    {
                        Value = c.id.ToString(),
                        Text = c.descripcion.ToString()
                    });
            }
            else
            {
                ViewBag.idProyecto_Estructura = new List<SelectListItem> { new SelectListItem { Text = " - ", Value = "-1" } };
            }

            // Se manda una boleta con los datos iniciales que debe ser llenada en la vista
            boleta boleta = new boleta
            {
                idContrato = idContrato.Value,
                idFondo = contrato.idFondo
            };

            return View(boleta);
        }

        /// <summary>
        /// Acción invocada por ajax y que según la ruta seleccionada en el dropdown, devuelve los pe de la ruta
        /// </summary>
        /// <param name="idRuta">id de la ruta para buscar sus pe</param>
        /// <returns>Json con un lista de id y descripción de los pe de la ruta</returns>
        public ActionResult ObtenerProyectosEstructuras(int? idRuta)
        {
            // Si el idRuta está vacío se retorna un badrequest
            if (idRuta == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Obtiene todos los proyectos y estructuras de la ruta
            var proyectosEstructuras = db.proyecto_estructura.Where(pe => pe.idRuta == idRuta);

            // Selecciona sólo el id y descripción de cada proyecto-estructura de la ruta
            var result = (from pe in proyectosEstructuras
                          select new
                          {
                              id = pe.id,
                              descripcion = pe.descripcion
                          }).ToList();

            if (result.Count < 1)
            {
                // Retornar vacío
            }

            // Retorna un JSON con la lista de proyecto-estructuras de la ruta
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // POST: Boleta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idContrato,id,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.boleta.Add(boleta);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { idContrato = boleta.idContrato });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error al ingresar la boleta, verifique que no sea un duplicado");
                }
            }

            ViewBag.idContrato = boleta.idContrato;
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", boleta.idFondo);
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona", boleta.idInspector);
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion", boleta.idProyecto_Estructura);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", boleta.idRuta);
            return View(boleta);
        }

        // GET: Boleta/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            boleta boleta = db.boleta.Find(id);
            if (boleta == null)
            {
                return HttpNotFound();
            }

            // Se busca el contrato, esto para cargar el fondo y las rutas correspondientes al contrato
            Contrato contrato = db.Contrato.Find(boleta.idContrato);

            // Si no existe el contrato se retorna un 'No encontrado'
            if (contrato == null)
            {
                return HttpNotFound();
            }

            // Selecciona solo los fondos que existen el el contrato, siempre va a ser uno
            var fondoContrato = db.fondo.Where(f => f.id == contrato.fondo.id);
            ViewBag.idFondo = new SelectList(fondoContrato, "id", "nombre", boleta.idFondo);

            // Se carga la lista de inspectores del sistema
            var mquery = (
                from p in db.persona
                join i in db.inspector
                on p.id equals i.idPersona
                select new SelectListItem
                {
                    Value = p.id.ToString(),
                    Text = p.nombre + " " + p.apellido1 + " " + p.apellido2
                }
            );
            ViewBag.idInspector = new SelectList(mquery, "Value", "Text", boleta.idInspector);

            // Se cargan las rutas correspondientes a la zona del contrato
            var rutasContrato = new SelectList(contrato.zona.ruta, "id", "nombre", boleta.idRuta);
            ViewBag.idRuta = rutasContrato;


            if (rutasContrato.ToList().Count != 0)
            {
                // Se selecciona de la bd los proyectos estructuras y se convierten en una lista
                ViewBag.idProyecto_Estructura = db.proyecto_estructura.ToList()
                    // Se selecciona de la lista sólo los de la primer ruta que se carga al dropdown
                    .Where(pe => pe.idRuta == int.Parse(rutasContrato.ToList().ElementAt(0).Value))
                    // Se convirte a lista donde se toman solamente el id y la descripción del pe
                    .Select(c => new SelectListItem
                    {
                        Value = c.id.ToString(),
                        Text = c.descripcion.ToString()
                    });
            }
            else
            {
                ViewBag.idProyecto_Estructura = new List<SelectListItem> { new SelectListItem { Text = " - ", Value = "-1" } };
            }

            return View(boleta);
        }

        // POST: Boleta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(boleta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idFondo = new SelectList(db.fondo, "id", "nombre", boleta.idFondo);
            ViewBag.idInspector = new SelectList(db.inspector, "idPersona", "idPersona", boleta.idInspector);
            ViewBag.idProyecto_Estructura = new SelectList(db.proyecto_estructura, "id", "descripcion", boleta.idProyecto_Estructura);
            ViewBag.idRuta = new SelectList(db.ruta, "id", "nombre", boleta.idRuta);
            return View(boleta);
        }

        // GET: Boleta/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            boleta boleta = db.boleta.Find(id);
            if (boleta == null)
            {
                return HttpNotFound();
            }
            return View(boleta);
        }

        // GET: Boleta/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            boleta boleta = db.boleta.Find(id);
            if (boleta == null)
            {
                return HttpNotFound();
            }
            return View(boleta);
        }

        // POST: Boleta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            boleta boleta = db.boleta.Find(id);
            db.boleta.Remove(boleta);
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
