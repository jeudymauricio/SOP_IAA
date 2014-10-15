using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

        // GET: Items del contrato
        public ActionResult Index(int? idContrato)
        {
            if ((idContrato == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
            // Se busca dentro de los reajustes de precio el que le corresponde a la fecha
            var itemReajustado = db.itemReajuste.Where(ir => ir.idContratoItem == ci.id);
            if (itemReajustado.Count() > 0)
            {
                itemReajustado = itemReajustado.OrderByDescending(ir => ir.fecha);

                // Si la fecha es menor que la del primer reajuste, se asigna el precio establecido en el contrato sin reajuste
                if (fecha < itemReajustado.ToList().Last().fecha)
                {
                    return ci.precioUnitario;
                }
                else // Si la fecha es mayor que la del primer reajuste, se busca el reajuste o en su defecto el mas cercano
                {
                    // Se asigna el precio del reajuste mas cercano a ese mes (o el de ese mes)
                    decimal precio = itemReajustado.First().precioReajustado;
                    foreach (var ir in itemReajustado)
                    {
                        if (ir.fecha < fecha)
                        {
                            precio = ir.precioReajustado;
                            break;
                        }
                    }
                    return precio;
                }

            }
            else // Si no hay reajustes se procede a poner el precio estipulado en el contrato.
            {
                return ci.precioUnitario;
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