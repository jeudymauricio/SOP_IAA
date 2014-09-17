using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOP_IAA.Models
{
    public class LinkContratoProgramaModels
    {
        
        // Elementos de Contrato
        public int idContrato { get; set; }
        public string licitacion { get; set; }

        // Elementos de Programa
        public short ano { get; set; }
        public byte trimestre { get; set; }

        // Elementos de ProgProy

        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> fechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> fechaFin { get; set; }
        public int monto { get; set; }

    }
}