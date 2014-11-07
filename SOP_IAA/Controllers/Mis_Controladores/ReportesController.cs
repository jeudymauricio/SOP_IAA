using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace SOP_IAA.Controllers
{
    public class ReportesController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();
        static int? id = 0;
        static int? idI = 0; 
        // GET: Items del contrato
        public ActionResult Index(int? idContrato)
        {
            if ((idContrato == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            id = idContrato;
            
            Contrato contrato = db.Contrato.Find(idContrato);
            return View(contrato);
        }

        //GET: 
        public ActionResult InformeDescriptivoItem(int? id)
        {
            if ((id == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);
            idI = contratoItem.idItem;
            return View(contratoItem);
        }

        // Acción de exportar las boletas de un ítem a excel (Informe Descriptivo de ítem)
        public ActionResult exportarInformeDescriptivoItem(int? id)
        {
            // Se verifica que venga un id
             if ((id == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);

            // Se verifica que el ítem exista en el contrato
            if (contratoItem == null)
            {
                return new HttpNotFoundResult();
            }

            // Se indica la plantilla a utilizar
            var template = new FileInfo(Server.MapPath("/Plantillas/Informe_Descriptivo_Item.xlsx"));

            using (ExcelPackage package = new ExcelPackage(template, true))
            {
                // 
                ExcelWorkbook workBook = package.Workbook;
                try
                {
                    if (workBook != null)
                    {
                        if (workBook.Worksheets.Count > 0)
                        {

                            // Se crea la hoja de excel y se le coloca como nombre el código de ítem
                            ExcelWorksheet worksheet = workBook.Worksheets["Hoja1"];

                            // Se rempieza a llenar los campos con los datos correspondientes
                            worksheet.Cells["D4"].Value = "ESTIMACIÓN DESCRIPTIVA N°  FONDO " + contratoItem.Contrato.fondo.nombre;
                            worksheet.Cells["D5"].Value = contratoItem.Contrato.lugar;
                            worksheet.Cells["D6"].Value = "Licitación Pública " + contratoItem.Contrato.licitacion;
                            worksheet.Cells["D7"].Value = contratoItem.Contrato.contratista.nombre;
                            worksheet.Cells["D11"].Value = contratoItem.item.codigoItem;
                            worksheet.Cells["E11"].Value = contratoItem.item.descripcion;

                            const int startRow = 14;
                            int row = startRow;

                            // Se insertan las filas ncecesarias al excel
                            worksheet.InsertRow(startRow, contratoItem.boletaItem.Count -2, startRow);

                            foreach (var item in contratoItem.boletaItem)
                            {
                                int col = 4;
                                worksheet.Cells[row, col].Value = item.boleta.numeroBoleta;
                                col++;
                                worksheet.Cells[row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                                worksheet.Cells[row, col++].Value = item.boleta.fecha;
                                worksheet.Cells[row, col++].Value = item.boleta.ruta.nombre;
                                worksheet.Cells[row, col++].Value = item.boleta.seccionControl;
                                worksheet.Cells[row, col++].Value = int.Parse(item.boleta.estacionamientoInicial);
                                worksheet.Cells[row, col++].Value = int.Parse(item.boleta.estacionamientoFinal);
                                worksheet.Cells[row, col++].Value = decimal.Parse(@String.Format("{0:N3}", (item.cantidad)));
                                worksheet.Cells[row, col++].Value = item.boleta.inspector.persona.nombre + " "
                                    + item.boleta.inspector.persona.apellido1 + " "
                                    + item.boleta.inspector.persona.apellido2;
                                worksheet.Cells[row, col++].Value = item.boleta.proyecto_estructura.descripcion;
                                worksheet.Cells[row, col++].Value = item.boleta.observaciones;  // Observaciones de la boleta
                                row++;
                            }

                            row++;

                            // Sumatoria de la boleta
                            worksheet.Cells[row, 10].Formula = "SUM(" + worksheet.Cells[startRow, 10].Address + ":" + worksheet.Cells[row - 2, 10].Address + ")";

                            // Se coloca la unidad de medida del ítem
                            worksheet.Cells[row++, 11].Value = contratoItem.item.unidadMedida;
                            worksheet.Cells[row, 11].Value = "/" + contratoItem.item.unidadMedida;
                            
                            // Se coloca el precio unitario
                            worksheet.Cells[row++, 10].Value = precioALaFecha(contratoItem, DateTime.Now);

                            // Fórmula de total a pagar
                            worksheet.Cells[row, 10].Formula = "(" + worksheet.Cells[row-1, 10].Address + "*" + worksheet.Cells[row - 2, 10].Address + ")";

                            // Se cambia el nombre a la hoja
                            worksheet.Name = contratoItem.item.codigoItem;
                        }
                    }

                    
                }
                catch
                {
                    // Se indica que ocurrió un error en la operación
                    return JavaScript("alert('Hubo un error en la operación, reintente mas tarde');");
                }

                // Nombre que va a tener el archivo
                string nombreArchivo = "Informe Descriptivo del Item " + contratoItem.item.codigoItem + ".xlsx";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
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

            // Se retorna el precio a la fecha
            return precio;
        }

        [HttpPost]
        public ActionResult exportarInformesItems(/*int? id, */DateTime fecha1, DateTime fecha2)
        {
            if ((id == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contrato contrato = db.Contrato.Find(id);

            // Se verifica que el ítem exista en el contrato
            if (contrato == null)
            {
                return new HttpNotFoundResult();
            }
            ExcelPackage pckTemplate = new ExcelPackage();
            // Se indica la plantilla a utilizar
            var template = new FileInfo(Server.MapPath("/Plantillas/Informe_Descriptivo_Item.xlsx"));
            FileInfo newFile = new FileInfo(@"Sample2.xlsx");
            ExcelPackage pkg = new ExcelPackage(newFile);
            int sheetIndex = 1;
            
            using (ExcelPackage package = new ExcelPackage(newFile,template))
            {

                ExcelWorkbook workBook = package.Workbook;
                int cont = 2;
                try
                {
                    if (workBook != null)
                    {
                        
                        if (workBook.Worksheets.Count > 0)
                        {  

                            int cantidad = contrato.contratoItem.Count;
                            for (int i = 0; i < cantidad;i++ )
                            {
                                // Se crea la hoja de excel 
                                ExcelWorksheet worksheet2;
                                worksheet2 = package.Workbook.Worksheets[sheetIndex];
                                package.Workbook.Worksheets.Add("First worksheet "+i, worksheet2);
                            }
                            sheetIndex++;
                            foreach(var contItem in contrato.contratoItem)
                            {

                                ExcelWorksheet worksheet;
                                
                                worksheet = package.Workbook.Worksheets[sheetIndex];
                                string _SheetName = string.Format("Hoja{0}", sheetIndex.ToString());

                                // Se rempieza a llenar los campos con los datos correspondientes
                                worksheet.Cells["D3"].Value = "ESTIMACIÓN DESCRIPTIVA N°  FONDO " + contItem.Contrato.fondo.nombre;
                                worksheet.Cells["D4"].Value = "Periodo del " + fecha1.ToShortDateString() + " al " + fecha2.ToShortDateString();
                                worksheet.Cells["D5"].Value = contItem.Contrato.lugar;
                                worksheet.Cells["D6"].Value = "Licitación Pública " + contItem.Contrato.licitacion;
                                worksheet.Cells["D7"].Value = contItem.Contrato.contratista.nombre;
                                worksheet.Cells["D11"].Value = contItem.item.codigoItem;
                                worksheet.Cells["E11"].Value = contItem.item.descripcion;

                                const int startRow = 14;
                                int row = startRow;
                                int rowBoletaItem = startRow;

                                foreach (var item in contItem.boletaItem)
                                {
                                   
                                    //int rowInicio = startRow;
                                    cont++;
                                    if (item.boleta.fecha >= fecha1 && item.boleta.fecha <= fecha2)
                                    {
                                        // Se insertan las filas ncecesarias al excel
                                        worksheet.InsertRow(rowBoletaItem, 1, rowBoletaItem);
                                        rowBoletaItem++;
                                        int col = 4;
                                        worksheet.Cells[row, col].Value = item.boleta.numeroBoleta;
                                        col++;
                                        worksheet.Cells[row, col].Style.Numberformat.Format = "dd/MM/yyyy";
                                        worksheet.Cells[row, col++].Value = item.boleta.fecha;
                                        worksheet.Cells[row, col++].Value = item.boleta.ruta.nombre;
                                        worksheet.Cells[row, col++].Value = item.boleta.seccionControl;
                                        worksheet.Cells[row, col++].Value = int.Parse(item.boleta.estacionamientoInicial);
                                        worksheet.Cells[row, col++].Value = int.Parse(item.boleta.estacionamientoFinal);
                                        worksheet.Cells[row, col++].Value = decimal.Parse(@String.Format("{0:N3}", (item.cantidad)));
                                        worksheet.Cells[row, col++].Value = item.boleta.inspector.persona.nombre + " "
                                            + item.boleta.inspector.persona.apellido1 + " "
                                            + item.boleta.inspector.persona.apellido2;
                                        worksheet.Cells[row, col++].Value = item.boleta.proyecto_estructura.descripcion;
                                        worksheet.Cells[row, col++].Value = item.boleta.observaciones;  // Observaciones de la boleta
                                        row++;

                                    }
                                }//foreach interno

                                row+=3;

                                // Sumatoria de la boleta
                                worksheet.Cells[row, 10].Formula = "SUM(" + worksheet.Cells[startRow, 10].Address + ":" + worksheet.Cells[row - 2, 10].Address + ")";
                                
                                // Se coloca la unidad de medida del ítem                  
                                worksheet.Cells[row, 11].Value = "/" + contItem.item.unidadMedida;

                                // Se coloca el precio unitario
                                row++;
                                worksheet.Cells[row, 10].Value = precioALaFecha(contItem, DateTime.Now);

                                // Fórmula de total a pagar
                                row++;
                                worksheet.Cells[row, 10].Formula = "(" + worksheet.Cells[row - 1, 10].Address + "*" + worksheet.Cells[row - 2, 10].Address + ")";

                                // Se cambia el nombre a la hoja
                                worksheet.Name = contItem.item.codigoItem;

                                sheetIndex++;
                                
                            }//foreach externo

                            package.Workbook.Worksheets.Delete(1);
                        }
                    }


                }
                catch(Exception e)
                {
                    // Se indica que ocurrió un error en la operación
                    return JavaScript("alert('Hubo un error en la operación, reintente mas tarde');");
                }

                // Nombre que va a tener el archivo
                string nombreArchivo = "Informe Descriptivo del contarto " + contrato.licitacion + ".xlsx";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreArchivo);
            }
        }

      /*  [HttpPost]
        public ActionResult exportarInformesItems(DateTime fecha1, DateTime fecha2)
        {

        }
       * */
        [HttpPost]
        public ActionResult InformeDescriptivoItemRango(DateTime fecha1, DateTime fecha2)
        {
            if ((idI == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            
            //contratoItem contratoItem = db.contratoItem.Find(idI);
            //var contratoItem = db.contratoItem.Select(ci => ci.boletaItem.Where(bi => bi.idContratoItem == idI && bi.boleta.fecha > fecha1 && bi.boleta.fecha < fecha2));
            var boletas = db.boletaItem.Include(b => b.boleta).Where(bi => bi.idContratoItem == idI && bi.boleta.fecha > fecha1 && bi.boleta.fecha < fecha2);
            ViewBag.fecha1 = fecha1;
            ViewBag.fecha2 = fecha2;
            ViewBag.id = idI;
            //var contratoItem = db.contratoItem.Include(ci => ci.boletaItem);

            //var boleta = db.boleta.Include(b => b.boletaItem.Where( b => b.idContratoItem == idI).Include(b => b.ruta).Include(b => b.proyecto_estructura).Include(b => b.inspector)
            //    .Where(b => b.idContrato == id && b.fecha < fecha2 && b.fecha > fecha1);
           
            /* var mquery = (from ci in db.contratoItem
                          join bi in db.boletaItem
                          on ci.idContrato equals bi.idContratoItem
                          join b in db.boleta
                          on bi.idBoleta equals b.id
                          where ci.idItem == id && b.fecha < fecha2 && b.fecha > fecha1
                          join r in db.ruta
                          on b.idRuta equals r.id
                          select new ReporteBoleta()
                          {
                              numeroBoleta = b.numeroBoleta,
                              fecha = b.fecha,
                              nombre = r.nombre,
                              seccionControl = b.seccionControl,
                              estacionamientoInicial = b.estacionamientoInicial,
                              estacionamientoFinal = b.estacionamientoFinal,
                              cantidad = bi.cantidad,
                              observaciones = b.observaciones
                          }
               );
            SelectList rb = new SelectList(mquery);
            
            ViewBag.reportes = new SelectList(mquery);*/
            return View(boletas.ToList());
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