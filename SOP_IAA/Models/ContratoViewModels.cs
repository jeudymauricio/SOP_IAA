using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOP_IAA_DAL;
using System.Web.Mvc;

namespace SOP_IAA.Models
{
    public class ContratoViewModels
    {
        public IEnumerable<SelectListItem> ListContratoIdContratista { get; set; }
        public IEnumerable<SelectListItem> ListContratoIdZona { get; set; }
        public IEnumerable<SelectListItem> ListContratoIdFondo { get; set; }
        public IEnumerable<SelectListItem> ListIngeniero { get; set; }
        //public IEnumerable<SelectListItem> IdContratista { get; set; }

        public int id { get; set; }
        public int idContratista { get; set; }
        public string licitacion { get; set; }
        public short lineaContrato { get; set; }
        public short idZona { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public short plazo { get; set; }
        public string lugar { get; set; }
        public short idFondo { get; set; }

        public Contrato contrato { get; set; }
        public List<ingeniero> ingeniero { get; set; }
        public List<laboratorioCalidad> laboratorios { get; set; }



    }
}