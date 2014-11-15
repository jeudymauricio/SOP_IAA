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
    public class ReportesItemController : Controller
    {
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();
      
        [HttpPost]
        public ActionResult exportarInformesItems(int? idContrato)
        {
            if ((idContrato == null))
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

                            ExcelWorksheet worksheet;

                            worksheet = package.Workbook.Worksheets[sheetIndex];
                            string _SheetName = string.Format("Hoja{0}", sheetIndex.ToString());

                            foreach(var contItem in contrato.contratoItem)
                            {

                                const int startRow = 14;
                                int row = startRow;
                                int rowBoletaItem = startRow;
                                int col = 4;
                                worksheet.Cells[row, col].Value = contItem.item.codigoItem;

                               /* foreach (var item in contItem.boletaItem)
                                {
                                   
                                    //int rowInicio = startRow;
                                    cont++;
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
                                }*///foreach interno

                                row++;

                                // Sumatoria de la boleta
                                //worksheet.Cells[row, 10].Formula = "SUM(" + worksheet.Cells[startRow, 10].Address + ":" + worksheet.Cells[row - 2, 10].Address + ")";
                                
                                // Se coloca la unidad de medida del ítem                  
                                //worksheet.Cells[row, 11].Value = "/" + contItem.item.unidadMedida;

                                // Se coloca el precio unitario
                                //row++;
                               // worksheet.Cells[row, 10].Value = precioALaFecha(contItem, DateTime.Now);

                                // Fórmula de total a pagar
                                //row++;
                               // worksheet.Cells[row, 10].Formula = "(" + worksheet.Cells[row - 1, 10].Address + "*" + worksheet.Cells[row - 2, 10].Address + ")";

                                // Se cambia el nombre a la hoja
                               // worksheet.Name = contItem.item.codigoItem;

                               // sheetIndex++;
                                
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