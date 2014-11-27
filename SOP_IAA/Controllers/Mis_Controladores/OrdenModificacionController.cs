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
using System.Web.Script.Serialization;
using System.IO;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Drawing;

namespace SOP_IAA.Controllers
{
    public class OrdenModificacionController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // GET: OrdenModificacion
        public ActionResult Index(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            contrato.ordenModificacion = contrato.ordenModificacion.OrderByDescending(om => om.fecha).ToList();
            //var ordenModificacion = db.ordenModificacion.Include(o => o.Contrato);
            //var ordenModificacion = contrato.ordenModificacion;

            return View(contrato/*ordenModificacion.ToList()*/);
        }

        // GET: OrdenModificacion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }

            // Se cargan los items
            ViewBag.items = CargarCantidadesEdit(ordenModificacion.idContrato, ordenModificacion.fecha);

            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Create
        public ActionResult Create(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var contrato = db.Contrato.Find(idContrato);

            // Se carga la lista de Items del contrato
            List<object> listItem = new List<object>();
            foreach (var ci in contrato.contratoItem)
            {
                listItem.Add(new Tuple<int, string>(ci.id, ci.item.codigoItem));
            }
            ViewBag.idItem = new SelectList(listItem, "item1", "item2");

            ViewBag.idContrato = idContrato;
            ordenModificacion om = new ordenModificacion();

            om.idContrato = idContrato.Value;
            om.fecha = DateTime.Now;

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion");
            return View(om);
        }

        /// <summary>
        /// Retorna un JSON con la comparación de las cantidades programadas vs las cantidades realizadas(según las boletas)
        /// y todo según una fecha específica
        /// </summary>
        /// <param name="idContrato">id del contrato a consultar</param>
        /// <param name="fecha">fecha a consultar</param>
        /// <returns>JSON(List<Tuple<int, string, string, string, decimal, decimal, decimal>>) -> <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
        public ActionResult CargarCantidades(int? idContrato, string fecha)
        {
            if (idContrato == null)
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

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return HttpNotFound();
            }

            // Lista con las cantidades originales, autorizadas, ejecutadas y disponibles por item
            // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
            List<Tuple<int, string, string, string, decimal, decimal, decimal>> listaItems = new List<Tuple<int, string, string, string, decimal, decimal, decimal>>(); //decimal.Round(cantidadOriginal - cantidadRealizada, 3)

            // Se recorren cada uno de los ítems del contrato
            foreach (var item in contrato.contratoItem)
            {
                // Se seleccionan las boletas hasta la fecha
                var bo = item.boletaItem.Where(b => (b.boleta.fecha <= fecha2));
                // Se seleccionan las ordenes de modificación válidas hasta la fecha
                var om = item.oMCI.Where(o => (o.ordenModificacion.fecha < fecha2));

                decimal cantidadOriginal = 0;

                cantidadOriginal = item.cantidadAprobada;

                decimal cantidadOMs = 0;
                // Se recorren las OM correspondientes a la fecha
                foreach (var ordenModificacion in om)
                {
                    cantidadOMs += ordenModificacion.cantidad;
                }

                decimal cantidadRealizada = 0;
                // Se recorren todas las boletas hasta la fecha para sumar sus cantidades
                foreach (var boleta in bo)
                {
                    cantidadRealizada += boleta.cantidad;
                }

                // Se guardan los detalles en una tupla // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
                var detalleItem = new Tuple<int, string, string, string, decimal, decimal, decimal>(item.id, item.item.codigoItem, item.item.descripcion, item.item.unidadMedida, cantidadOriginal, cantidadOMs, cantidadRealizada);

                // Se agrega la tupla a la lista de items
                listaItems.Add(detalleItem);
            }

            // Retorna un JSON con los detalles del ítem
            return Json(listaItems, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna una lista de tuplas con la comparación de las cantidades programadas vs las cantidades realizadas(según las boletas)
        /// y todo hasta una fecha específica
        /// </summary>
        /// <param name="idContrato">id del contrato a consultar</param>
        /// <param name="fecha">fecha a consultar</param>
        /// <returns>List<Tuple<int, string, string, string, decimal, decimal, decimal>> -> <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
        public List<Tuple<int, string, string, string, decimal, decimal, decimal>> CargarCantidadesEdit(int idContrato, DateTime fecha)
        {

            // Se selecciona el contrato
            Contrato contrato = db.Contrato.Find(idContrato);
            if (contrato == null)
            {
                return null;
            }

            // Lista con las cantidades originales, autorizadas, ejecutadas y disponibles por item
            // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
            List<Tuple<int, string, string, string, decimal, decimal, decimal>> listaItems = new List<Tuple<int, string, string, string, decimal, decimal, decimal>>(); //decimal.Round(cantidadOriginal - cantidadRealizada, 3)

            // Se recorren cada uno de los ítems del contrato
            foreach (var item in contrato.contratoItem)
            {
                // Se seleccionan las boletas hasta la fecha
                var bo = item.boletaItem.Where(b => (b.boleta.fecha <= fecha));
                // Se seleccionan las ordenes de modificación válidas hasta la fecha
                var om = item.oMCI.Where(o => (o.ordenModificacion.fecha < fecha));

                decimal cantidadOriginal = 0;

                cantidadOriginal = item.cantidadAprobada;

                decimal cantidadOMs = 0;
                // Se recorren las OM correspondientes a la fecha
                foreach (var ordenModificacion in om)
                {
                    cantidadOMs += ordenModificacion.cantidad;
                }

                decimal cantidadRealizada = 0;
                // Se recorren todas las boletas hasta la fecha para sumar sus cantidades
                foreach (var boleta in bo)
                {
                    cantidadRealizada += boleta.cantidad;
                }

                // Se guardan los detalles en una tupla // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
                var detalleItem = new Tuple<int, string, string, string, decimal, decimal, decimal>(item.id, item.item.codigoItem, item.item.descripcion, item.item.unidadMedida, cantidadOriginal, cantidadOMs, cantidadRealizada);

                // Se agrega la tupla a la lista de items
                listaItems.Add(detalleItem);
            }

            // Retorna los detalles del ítem
            return listaItems;
        }

        // POST: OrdenModificacion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM,AumentoPlazo")] ordenModificacion ordenModificacion,
            [Bind(Include = "jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                // Obtener items.
                dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                // Formato en que están los decimales
                IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);

                try
                {
                    foreach (var child in jObj.Items.Children())
                    {
                        //Creación del contratoItem-OM.
                        oMCI om = new oMCI();
                        om.idContratoItem = int.Parse(child.idContratoItem.Value);
                        om.cantidad = decimal.Parse(child.cantidad.Value, culture);

                        // Se agrega la om de item a la OM global
                        ordenModificacion.oMCI.Add(om); // This will auto-fill the Foreign Key during commit.
                    }
                    // Se agrega la OM a la BD
                    db.ordenModificacion.Add(ordenModificacion);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { idContrato = ordenModificacion.idContrato });
                }
                catch (Exception ex)
                {
                    try
                    {
                        var sqlException = ex.InnerException.InnerException as SqlException;
                        if (sqlException != null && sqlException.Errors.OfType<SqlError>()
                        .Any(se => se.Number == 2601 || se.Number == 2627 /* PK/UKC violation */))
                        {
                            ModelState.AddModelError("", "No se pudo agregar la OM porque ya existe una para esa fecha");
                            // it's a dupe... do something about it
                        }
                        else
                        {
                            // it's something else...
                            ModelState.AddModelError("", "No se pudo agregar la OM");
                        }
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "No se pudo agregar la OM");
                    }
                }
            }

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            ViewBag.idContrato = ordenModificacion.idContrato;
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);

            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }

            // Se cargan las cantidades
            ViewBag.items = CargarCantidadesEdit(ordenModificacion.idContrato, ordenModificacion.fecha);

            //ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "id,idContrato,numeroOficio,fecha,objetoOM,AumentoPlazo")] ordenModificacion ordenModificacion,
            [Bind(Include = "jsonItems")] string jsonItems)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener items.
                    dynamic jObj = JsonConvert.DeserializeObject(jsonItems);

                    // Formato en que están los decimales
                    IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);


                    foreach (var child in jObj.Items.Children())
                    {
                        int idContratoItem = int.Parse(child.idContratoItem.Value);
                        int idOrdenModificacion = int.Parse(child.idOrdenModificacion.Value);

                        var omci = db.oMCI.Find(idOrdenModificacion, idContratoItem);
                        if (omci == null)
                        {
                            continue;
                        }

                        omci.cantidad = decimal.Parse(child.cantidad.Value, culture);
                        db.Entry(omci).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    db.Entry(ordenModificacion).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index", new { idContrato = ordenModificacion.idContrato });
                }
                catch (Exception)
                {
                    //Notify error
                    ModelState.AddModelError("", "No se pudo modificar la OM ");
                }
            }
            ViewBag.idContrato = new SelectList(db.Contrato, "id", "licitacion", ordenModificacion.idContrato);
            return View(ordenModificacion);
        }

        // GET: OrdenModificacion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            if (ordenModificacion == null)
            {
                return HttpNotFound();
            }
            return View(ordenModificacion);
        }

        // POST: OrdenModificacion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ordenModificacion ordenModificacion = db.ordenModificacion.Find(id);
            int _idContrato = ordenModificacion.idContrato;
            try
            {
                db.oMCI.RemoveRange(ordenModificacion.oMCI);
                db.ordenModificacion.Remove(ordenModificacion);
                db.SaveChanges();
            }
            catch (Exception)
            {
                //Notify Error
            }
            return RedirectToAction("Index", new { idContrato = _idContrato });
        }

        public ActionResult exportarOM(int? idContrato)
        {
            if (idContrato == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(idContrato);

            // Se verifica que el ítem exista en el contrato
            if (contrato == null)
            {
                return new HttpNotFoundResult();
            }

            ExcelPackage pckTemplate = new ExcelPackage();
            // Se indica la plantilla a utilizar
            var template = new FileInfo(Server.MapPath("/Plantillas/Orden_Modificacion.xlsx"));
            FileInfo newFile = new FileInfo(@"Sample2.xlsx");
            ExcelPackage pkg = new ExcelPackage(newFile);
            int sheetIndex = 1;
            string nombreArchivo;

            try
            {
                using (ExcelPackage package = new ExcelPackage(newFile, template))
                {
                    ExcelWorkbook workBook = package.Workbook;

                    if (workBook != null)
                    {

                        if (workBook.Worksheets.Count > 0)
                        {
                            int cantidad = contrato.contratoItem.Count;

                            ExcelWorksheet worksheet;

                            worksheet = package.Workbook.Worksheets[sheetIndex];
                            string _SheetName = string.Format("Hoja{0}", sheetIndex.ToString());

                            //Se comprueban que existan OMs
                            if (contrato.ordenModificacion.Count < 1)
                            {
                                // Nombre que va a tener el archivo
                                nombreArchivo = "OMs del Contrato " + contrato.licitacion + ".xlsx";
                                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                            }

                            // Se coloca los datos de contrato
                            worksheet.Cells["B4"].Value = contrato.contratista.nombre;
                            worksheet.Cells["B5"].Value = contrato.licitacion;
                            worksheet.Cells["B6"].Value = contrato.lugar;
                            worksheet.Cells["B7"].Value = contrato.lineaContrato;
                            worksheet.Cells["B8"].Value = contrato.zona.nombre;
                            worksheet.Cells["B11"].Value = contrato.fechaInicio;
                            worksheet.Cells["B12"].Value = contrato.fechaInicio.AddDays(contrato.plazo); // Tomar en cuenta los aumentos de plazo de las OM
                            worksheet.Cells["B13"].Value = contrato.plazo;

                            // Se indica desde donde empezar a llenar los datos
                            const int startRow = 20;
                            int row = startRow;

                            // Se obtiene los estilos de las celdas
                            int estiloOMTitulo = worksheet.Cells["K14"].StyleID;
                            int estiloOMFecha = worksheet.Cells["K16"].StyleID;
                            int estiloOMOficio = worksheet.Cells["K17"].StyleID;
                            int estiloOMVariacion = worksheet.Cells["K18"].StyleID;
                            int estiloOMDiv = worksheet.Cells["K19"].StyleID;
                            int estiloOMHistorial = worksheet.Cells["K20"].StyleID;
                            int estiloCondicional = worksheet.Cells["G20"].StyleID;
                            int estiloAumentoDisminucion = worksheet.Cells[25, 11].StyleID;
                            int estiloSumaProductoOM = worksheet.Cells[24, 11].StyleID;

                            // Se insertan las filas necesarias al excel con el mismo formato que la primera
                            if (cantidad > 2)
                            {
                                worksheet.InsertRow(startRow, cantidad - 2, startRow);
                            }

                            // Se obtiene el item con el mayor número de OMs
                            var itemOrdenados = contrato.contratoItem.OrderByDescending(ci => ci.oMCI.Count);

                            // Se crea una lista con el total de las fechas y se colocan las fechas en el excel
                            List<DateTime> l = new List<DateTime>();

                            int col = 11;
                            int fila1 = startRow + cantidad - 1;
                            foreach (var i in itemOrdenados.First().oMCI)
                            {
                                // Se agrega la fecha a la lista
                                l.Add(i.ordenModificacion.fecha);

                                //Titulo de la OM -1,2,3,4...
                                worksheet.Cells[14, col].StyleID = estiloOMTitulo;
                                worksheet.Cells[14, col].Value = "OM" + (col - 10).ToString();

                                //Objeto de la OM
                                worksheet.Cells[15, col].Value = i.ordenModificacion.objetoOM;

                                //Fecha de la OM
                                worksheet.Cells[16, col].StyleID = estiloOMFecha;
                                worksheet.Cells[16, col].Value = i.ordenModificacion.fecha;

                                // Oficio de aprobación
                                worksheet.Cells[17, col].StyleID = estiloOMOficio;
                                worksheet.Cells[17, col].Value = i.ordenModificacion.numeroOficio;

                                //Variacion de plazo
                                worksheet.Cells[18, col].StyleID = estiloOMVariacion;
                                worksheet.Cells[18, col].Value = i.ordenModificacion.AumentoPlazo;

                                // Division de celdas (por estetica)
                                worksheet.Cells[19, col].StyleID = estiloOMDiv;

                                // Fórmula para montos de cada OM (SUMAProducto en cada OM)
                                worksheet.Cells[fila1 + 3, col].StyleID = estiloSumaProductoOM;
                                worksheet.Cells[fila1 + 3, col].Formula = "SUMPRODUCT(" + worksheet.Cells[startRow, 4].Address + ":" + worksheet.Cells[fila1, 4].Address + "," + worksheet.Cells[startRow, col].Address + ":" + worksheet.Cells[fila1, col].Address + ")";

                                // Fórmula total de aumentos
                                worksheet.Cells[fila1 + 4, col].StyleID = estiloAumentoDisminucion;
                                worksheet.Cells[fila1 + 4, col].Formula = "SUMIF(" + worksheet.Cells[startRow, col].Address + ":" + worksheet.Cells[fila1, col].Address + ",\">0\"," + worksheet.Cells[startRow, col].Address + ":" + worksheet.Cells[fila1, col].Address + ")";
                                // Fórmula total de rebajas
                                worksheet.Cells[fila1 + 5, col].StyleID = estiloAumentoDisminucion;
                                worksheet.Cells[fila1 + 5, col].Formula = "SUMIF(" + worksheet.Cells[startRow, col].Address + ":" + worksheet.Cells[fila1, col].Address + ",\"<0\"," + worksheet.Cells[startRow, col].Address + ":" + worksheet.Cells[fila1, col].Address + ")";
                                col++;
                            }

                            // Total de aumentos y total de disminuciones
                            worksheet.Cells[fila1 + 4, 10].Formula = "sum(" + worksheet.Cells[fila1 + 4, 11].Address + ":" + worksheet.Cells[fila1 + 4, col-1].Address + ")";
                            worksheet.Cells[fila1 + 5, 10].Formula = "sum(" + worksheet.Cells[fila1 + 5, 11].Address + ":" + worksheet.Cells[fila1 + 5, col-1].Address + ")";

                            // Variación de plazo
                            worksheet.Cells["B14"].Formula = "sum(" + worksheet.Cells["K18"].Address + ":" + worksheet.Cells[18, col - 2].Address + ")";

                            foreach (var ci in itemOrdenados)
                            {
                                worksheet.Cells["A" + row].Value = ci.item.codigoItem;
                                worksheet.Cells["B" + row].Value = ci.item.descripcion;
                                worksheet.Cells["C" + row].Value = ci.item.unidadMedida;
                                worksheet.Cells["D" + row].Value = ci.precioUnitario;
                                worksheet.Cells["E" + row].Value = cantidadObraRealizada(ci, DateTime.Now);

                                // Se empieza a colocar el historial de OMs siguiente el orden de la fecha
                                // Se recorre la lista de fechas y se buscan los reajustes correspondientes
                                col = 11;
                                foreach (var fechaOM in l)
                                {
                                    var om = ci.oMCI.Where(x => x.ordenModificacion.fecha == fechaOM).FirstOrDefault();

                                    //Se coloca el estilo
                                    worksheet.Cells[row, col].StyleID = estiloOMHistorial;
                                    //Si no hay OMs para esa fecha se coloca 0, de lo contrario se coloca la cantidad de la OM
                                    if (om == null)
                                    {
                                        worksheet.Cells[row, col++].Value = 0;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col++].Value = om.cantidad;
                                    }
                                }
                                // Se coloca el monto original especificado en el contrato
                                worksheet.Cells["I" + row].Value = ci.cantidadAprobada;

                                // Se coloca la fórmula para obtener el resumen de OM
                                worksheet.Cells[row, 10].Formula = "SUM(" + worksheet.Cells[row, 11].Address + ":" + worksheet.Cells[row, col - 1].Address + ")";
                                // Se coloca la fórmula de Autorizado (Original + ResumenOM)
                                worksheet.Cells["F" + row].Formula = worksheet.Cells[row, 9].Address + "+" + worksheet.Cells[row, 10].Address;
                                // Fórmula para Disponible (Autorizado - Realizado)
                                worksheet.Cells[row, 8].Formula = worksheet.Cells[row, 6].Address + "-" + worksheet.Cells[row, 5].Address;

                                // Fórmula de Status
                                //worksheet.Cells["G" + row].StyleID = estiloCondicional;
                                worksheet.Cells["G" + row].Formula = "IF(" + worksheet.Cells["F" + row].Address + "<" + worksheet.Cells["E" + row].Address + ",\"Error\",\"OK\")";

                                // Se cambia de Fila para el próximo item
                                row++;

                            }// foreach itemOrdenados

                            // Monto total original (Original * PrecioUnitario)
                            worksheet.Cells["I" + (row + 2).ToString()].Formula = "SUMPRODUCT(" + worksheet.Cells["D" + startRow].Address + ":" + worksheet.Cells["D" + (row - 1).ToString()].Address + "," + worksheet.Cells["I" + startRow].Address + ":" + worksheet.Cells["I" + (row - 1).ToString()].Address + ")";

                            // Monto total Actual (Autorizado * PrecioUnitario)
                            worksheet.Cells["F" + (row + 2).ToString()].Formula = "SUMPRODUCT(" + worksheet.Cells["D" + startRow].Address + ":" + worksheet.Cells["D" + (row - 1).ToString()].Address + "," + worksheet.Cells["F" + startRow].Address + ":" + worksheet.Cells["F" + (row - 1).ToString()].Address + ")";
                        }
                    }

                    // Nombre que va a tener el archivo
                    nombreArchivo = "OMs del Contrato " + contrato.licitacion + ".xlsx";
                    return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
                }

            }
            catch (Exception e)
            {
                // Se indica que ocurrió un error en la operación
                return JavaScript("alert('Hubo un error en la operación, reintente mas tarde');");
            }
        }

        public decimal cantidadObraRealizada(contratoItem ci, DateTime fecha)
        {
            // Se seleccionan las boletas hasta la fecha (dia, mes, año)
            var bo = ci.boletaItem.Where(b => (b.boleta.fecha.Month <= fecha.Month && b.boleta.fecha.Year <= fecha.Year && b.boleta.fecha.Day <= fecha.Day));

            decimal cantidadLaborada = 0;
            // Se recorren todas las boletas hasta la fecha para sumar sus cantidades
            foreach (var boleta in bo)
            {
                cantidadLaborada += boleta.cantidad;
            }
            return cantidadLaborada;
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
