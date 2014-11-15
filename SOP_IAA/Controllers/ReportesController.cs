using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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
        public ActionResult Reports(int? id)
        {
            if ((id == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contratoItem = db.contratoItem.Find(id);
            return View(contratoItem);
        }

        public ActionResult Export(int? id)
        {
            if ((id == null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            contratoItem contraroItem = db.contratoItem.Find(id);
            // Load Excel application
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            // Create empty workbook
            excel.Workbooks.Add();

            // Create Worksheet from active sheet
            Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;

            // I created Application and Worksheet objects before try/catch,
            // so that i can close them in finnaly block.
            // It's IMPORTANT to release these COM objects!!
            try
            {
                // ------------------------------------------------
                // Creation of header cells
                // ------------------------------------------------
                workSheet.Cells[1, "A"] = "Número de boleta";
                workSheet.Cells[1, "B"] = "Fecha";
                workSheet.Cells[1, "C"] = "Ruta";
                workSheet.Cells[1, "D"] = "Sección de control";
                workSheet.Cells[1, "E"] = "Estacionamiento inicial";
                workSheet.Cells[1, "F"] = "Estacionamiento final";
                workSheet.Cells[1, "G"] = "Cantidad";
                workSheet.Cells[1, "H"] = "Nombre";
                workSheet.Cells[1, "I"] = "Proyecto/Estructura";

                // ------------------------------------------------
                // Populate sheet with some real data from "cars" list
                // ------------------------------------------------
                int row = 2; // start row (in row 1 are header cells)
                foreach (var item in contraroItem.boletaItem)
                {
                    workSheet.Cells[row, "A"] = item.boleta.numeroBoleta;
                    workSheet.Cells[row, "B"] = item.boleta.fecha;
                    workSheet.Cells[row, "C"] = item.boleta.ruta.nombre;
                    workSheet.Cells[row, "D"] = item.boleta.seccionControl;
                    workSheet.Cells[row, "E"] = item.boleta.estacionamientoInicial;
                    workSheet.Cells[row, "F"] = item.boleta.estacionamientoFinal;
                    workSheet.Cells[row, "G"] = item.cantidad;
                    workSheet.Cells[row, "H"] = item.boleta.inspector.persona.nombre + " " 
                        + item.boleta.inspector.persona.apellido1 + " " 
                        + item.boleta.inspector.persona.apellido2;
                    workSheet.Cells[row, "I"] = item.boleta.proyecto_estructura.descripcion;

                    row++;
                }

                // Apply some predefined styles for data to look nicely :)
                workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

                // Define filename
                string fileName = string.Format(@"{0}\ExcelData.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

                // Save this data as a file
                workSheet.SaveAs(fileName);

                // Display SUCCESS message
                //MessageBox.Show(string.Format("The file '{0}' is saved successfully!", fileName));
            }
            catch (Exception exception)
            {
                //MessageBox.Show("Exception",
                  //  "There was a PROBLEM saving Excel file!\n" + exception.Message,
                   // MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                // Quit Excel application
                excel.Quit();

                // Release COM objects (very important!)
                if (excel != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

                if (workSheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

                // Empty variables
                excel = null;
                workSheet = null;

                // Force garbage collector cleaning
                GC.Collect();
            }
            return RedirectToAction("Reports", new { id = contraroItem.id });
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