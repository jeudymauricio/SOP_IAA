using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SOP_IAA_DAL;
using Newtonsoft.Json;

namespace SOP_IAA.Controllers
{
    public partial class BoletaController : Controller // El namespace no debe incluir .Mis_Controladores
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

            return View(boleta.OrderByDescending(b => b.fecha).ToList());
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

            // Se verifica si existen rutas asociadas a la zona del contrato
            if (rutasContrato.ToList().Count > 0)
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
                // Se envía una lista vacía
                ViewBag.idProyecto_Estructura = Enumerable.Empty<SelectListItem>();
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
        /// Con base en una fecha y un item del contrato, busca el precio reajustado a la fecha
        /// </summary>
        /// <param name="ci">Item del contrato</param>
        /// <param name="fecha">Fecha que se desea consultar</param>
        /// <returns></returns>
        private decimal precioALaFecha(contratoItem ci, DateTime fecha)
        {
            // Precio base del item
            decimal precio = ci.precioUnitario;

            /// Para el primer entregable se omite esta sección y se deje el precio como viene en el contrato
            #region reajustes_al_precio
            /*
            // Se busca si hay reajuste para ese mes
            var itemReajustado = db.itemReajuste.Where(ir => (ir.ano == fecha.Year) && (ir.mes == fecha.Month) && (ir.idContratoItem == ci.id));

            // Si hay reajuste se aplica
            if (itemReajustado.Count() > 0)
            {
                // Reajuste del mes
                decimal reajuste = itemReajustado.First().reajuste;

                // Se aplica el reajuste
                precio = decimal.Round(precio * reajuste + precio, 4);
            }
            */ 
            #endregion

            // Se retorna el precio a la fecha
            return precio;
        }

        /// <summary>
        /// Acción invocada por ajax y que devuelve los detalles de un item específico
        /// </summary>
        /// <param name="id"> id del item de contrato a buscar</param>
        /// <param name="fecha">fecha de la boleta</param>
        /// <returns>Json con los detalles del item del contrato(incluido su reajuste si lo hay)</returns>
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
            //result.Add("idContratoItem", ci.id.ToString());
            result.Add("codigoItem", ci.item.codigoItem);
            result.Add("descripcion", ci.item.descripcion);
            result.Add("unidadMedida", ci.item.unidadMedida);

            // Precio base del item
            decimal precio = precioALaFecha(ci, fecha2);

            // Se almacena el precio
            result.Add("precioReajustado", precio.ToString());

            // Retorna un JSON con los detalles del ítem
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Con base a una fecha, retorna el precio TODOS los ítems en esa fecha (con reajuste, o precio base si no hay reajustes)
        /// </summary>
        /// <param name="fecha">Fecha a buscar</param>
        /// <param name="idContrato">Contrato al que pertenecen los reajustes</param>
        /// <returns>Lista de precios de cada item en el mes</returns>
        public Dictionary<int, decimal> obtenerPrecios(DateTime fecha, int idContrato)
        {
            // Lista de items del contrato
            var contratoItem = db.contratoItem.Where(ci => ci.idContrato == idContrato);

            // Diccionario que contendrá el id del item en el contrato y su respectivo reajuste o precio base  <int> idContratoItem  <decimal> Precio
            Dictionary<int, decimal> precios = new Dictionary<int, decimal>();

            // Se procede a guardar los precios correspondientes a cada item del contrato
            foreach (contratoItem ci in contratoItem)
            {
                // Precio base del item
                decimal precio = precioALaFecha(ci, fecha);

                // Se almacena el precio
                precios.Add(ci.id, precio);
            }

            // Se retorna el diccionario con los precios correspondientes a los items segun la fecha anterior
            return precios;
        }

        // POST: Boleta/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "idContrato,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta,
            [Bind(Include = "jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Como el formato de números utiliza la , como separador de decimales(es-CR), se debe convertir a formato inglés(en-US)
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                    // Se agrega la boleta a la tabla boleta
                    db.boleta.Add(boleta);
                    db.SaveChanges();

                    // Obtener items.
                    dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                    foreach (var child in jObj.Items.Children())
                    {
                        //Creación del boleta-item.
                        boletaItem bi = new boletaItem();

                        bi.idBoleta = boleta.id;
                        bi.idContratoItem = int.Parse(child.idItemContrato.Value);
                        bi.cantidad = decimal.Parse(child.cantidad.Value, culture);
                        //bi.costoTotal = decimal.Parse(child.costoTotal.Value, culture);
                        bi.redimientos = decimal.Parse(child.redimientos.Value, culture);
                        //bi.precioUnitarioFecha = decimal.Parse(child.precio.Value, culture);

                        // Se agrega el boleta-item a la base de datos
                        db.boletaItem.Add(bi);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index", new { idContrato = boleta.idContrato });
                }
                catch (Exception)
                {
                    // Se verifica que al menos hay una ruta seleccionada
                    if (boleta.idRuta == 0)
                    {
                        ModelState.AddModelError("", "Debe seleccionar una ruta válida");
                    }
                    // Se verifica que al menos hay un PE
                    else if (boleta.idProyecto_Estructura == 0)
                    {
                        ModelState.AddModelError("", "Debe seleccionar un Proyecto/Estructura válidos");
                    }
                    //
                    else
                    {
                        ModelState.AddModelError("", "Ocurrió un error al ingresar la boleta, verifique que no sea un duplicado");
                    }
                }
            }

            // Se busca el contrato, esto para cargar el fondo y las rutas correspondientes al contrato
            Contrato contrato = db.Contrato.Find(boleta.idContrato);

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

            // Se selecciona de la bd los proyectos estructuras y se convierten en una lista
            ViewBag.idProyecto_Estructura = db.proyecto_estructura.ToList()
                // Se selecciona de la lista sólo los PEde la ruta seleccionada en la boleta
                .Where(pe => pe.idRuta == boleta.idRuta)
                // Se convirte a lista donde se toman solamente el id y la descripción del pe
                .Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.descripcion.ToString()
                });

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

            // Se carga la lista de Items del contrato
            List<object> listItem = new List<object>();
            foreach (var ci in contrato.contratoItem)
            {
                listItem.Add(new Tuple<int, string>(ci.id, ci.item.codigoItem));
            }
            ViewBag.idItem = new SelectList(listItem, "item1", "item2");

            // Se cargan las rutas correspondientes a la zona del contrato
            var rutasContrato = new SelectList(contrato.zona.ruta, "id", "nombre", boleta.idRuta);
            ViewBag.idRuta = rutasContrato;

            // Se selecciona de la bd los proyectos estructuras y se convierten en una lista
            var peAux = db.proyecto_estructura.ToList()
                // Se selecciona de la lista sólo los PEde la ruta seleccionada en la boleta
                .Where(pe => pe.idRuta == boleta.idRuta)
                // Se convirte a lista donde se toman solamente el id y la descripción del pe
                .Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.descripcion.ToString()
                });

            ViewBag.idProyecto_Estructura = new SelectList(peAux, "Value", "Text", boleta.idProyecto_Estructura);

            // Se cargan los precios a la fecha
            ViewBag.precios = obtenerPrecios(boleta.fecha, boleta.idContrato);

            return View(boleta);
        }

        // POST: Boleta/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "id,idContrato,numeroBoleta,idFondo,idRuta,idInspector,fecha,seccionControl,estacionamientoInicial,estacionamientoFinal,periodo,idProyecto_Estructura,observaciones")] boleta boleta,
            [Bind(Include = "jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                /// Primero se actualizan los datos de ítems
                //Se encuentra la boleta
                boleta boletaEditar = db.boleta.Find(boleta.id);

                // Se remueven los anteriores items de la boleta
                db.boletaItem.RemoveRange(boletaEditar.boletaItem);
                db.SaveChanges();

                // Se agregan los nuevos ítems
                // Obtener items.
                dynamic jObj = JsonConvert.DeserializeObject(jsonItems);
                // Como el formato de números utiliza la , como separador de decimales(es-CR), se debe convertir a formato inglés(en-US)
                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                foreach (var child in jObj.Items.Children())
                {
                    //Creación del boleta-item.
                    boletaItem bi = new boletaItem();

                    bi.idBoleta = boleta.id;
                    bi.idContratoItem = int.Parse(child.idItemContrato.Value);
                    bi.cantidad = decimal.Parse(child.cantidad.Value, culture);
                    //bi.costoTotal = decimal.Parse(child.costoTotal.Value, culture);
                    bi.redimientos = decimal.Parse(child.redimientos.Value, culture);
                    //bi.precioUnitarioFecha = decimal.Parse(child.precio.Value, culture);

                    // Se agrega el boleta-item a la base de datos
                    db.boletaItem.Add(bi);

                    // Se guardan los cambios de la relacion boleta-item   
                    db.SaveChanges();
                }

                Repositorio<boleta> rep = new Repositorio<boleta>();
                rep.Actualizar(boleta);

                return RedirectToAction("Index", new { idContrato = boleta.idContrato });
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

            ViewBag.precios = obtenerPrecios(boleta.fecha, boleta.idContrato);
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

            try
            {

                db.boletaItem.RemoveRange(boleta.boletaItem);
                db.boleta.Remove(boleta);
                db.SaveChanges();
                return RedirectToAction("Index", new { idContrato = boleta.idContrato });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message.ToString());
            }
            return View(boleta);
        }

    }
}
