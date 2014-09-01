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
        public List<int> ingenieros { get; set; }
        public List<int> laboratorios { get; set; }

        //Para usar en el dropdownlist de ContratoView/CreateContractEngineer
        //public IEnumerable<SelectListItem> ListIngeniero { get; set; }

    }
}