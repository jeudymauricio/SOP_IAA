using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SOP_IAA
{
    public class ReporteBoleta
    {
        public int numeroBoleta { get; set; }
        public DateTime fecha { get; set; }
        public string nombre { get; set; }
        public int seccionControl { get; set; }
        public string estacionamientoInicial { get; set; }
        public string estacionamientoFinal { get; set; }
        public decimal cantidad { get; set; }
        public string observaciones { get; set; }
    }
}