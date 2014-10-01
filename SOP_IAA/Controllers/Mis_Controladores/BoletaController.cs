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

            // Se carga la lista de Items del contrato
            List<object> listItem = new List<object>();
            foreach (var ci in contrato.contratoItem)
            {
                listItem.Add(new Tuple<int, string>(ci.id, ci.item.codigoItem));
            }
            ViewBag.idItem = new SelectList(listItem, "item1", "item2");

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

        /// <summary>
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <param name="fecha">fecha de la boleta</param>
        /// <returns>Json con los detalles del item del contrato(incluido su reajuste mas cercano)</returns>
        public ActionResult ItemDetalles(int? id, string fecha)
        {
            // Si el id o la fecha están vacíos se retorna un badrequest
            if ((id == null) || (string.IsNullOrWhiteSpace(fecha)))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Especifica el formato en que está la fecha, en este caso Costa Rica (es-CR)
            IFormatProvider culture = new System.Globalization.CultureInfo("es-CR", true);

            DateTime fecha2 = new DateTime();
            // convierte el string en datetime
            try
            {
                fecha2 = DateTime.Parse(fecha, culture, System.Globalization.DateTimeStyles.AssumeLocal);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(99, e.Message.ToString());
            }

            // Selecciona de la base de datos el contratoItem correspondiente
            contratoItem ci = db.contratoItem.Find(id);

            // Si está nulo, quiere decir que no existe el item en el contrato
            if (ci == null)
            {
                return HttpNotFound();
            }

            /// Se inicia con la creacion de un diccionario con toda la información pertinente del item
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add("codigoItem", ci.item.codigoItem);
            result.Add("descripcion", ci.item.descripcion);
            result.Add("unidadMedida", ci.item.unidadMedida);

            // Se busca dentro de los reajustes de precio el que le corresponde a la fecha de la boleta
            //var itemReajustado = db.itemReajuste.Where(ir => (ir.ano == fecha2.Year) && (ir.mes == fecha2.Month) && (ir.idContratoItem == id));
            var itemReajustado = db.itemReajuste.Where(ir => ir.idContratoItem == id);
            if (itemReajustado.Count() > 0)
            {
                itemReajustado = itemReajustado.OrderByDescending(ir => ir.fecha);

                // Si la fecha es menor que la del primer reajuste, se asigna el precio establecido en el contrato sin reajuste
                if (fecha2 < itemReajustado.ToList().Last().fecha)
                {
                    result.Add("precioReajustado", ci.precioUnitario.ToString("C3", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR")));
                }
                else // Si la fecha es mayor que la del primer reajuste, se busca el reajuste o en su defecto el mas cercano
                {
                    // Se asigna el precio del reajuste mas cercano a ese mes (o el de ese mes)
                    decimal precio = itemReajustado.First().precioReajustado;
                    foreach (var ir in itemReajustado)
                    {
                        if (ir.fecha < fecha2)
                        {
                            precio = ir.precioReajustado;
                            break;
                        }
                    }
                    result.Add("precioReajustado", precio.ToString("C3", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR")));
                }

            }
            else // Si no hay reajustes se procede a poner el precio estipulado en el contrato.
            {
                result.Add("precioReajustado", ci.precioUnitario.ToString("C3", System.Globalization.CultureInfo.CreateSpecificCulture("es-CR")));
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

    }
}
