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

        public Contrato contrato { get; set; }
        
        public List<ingeniero>  ingenieros { get; set; }
    }
}