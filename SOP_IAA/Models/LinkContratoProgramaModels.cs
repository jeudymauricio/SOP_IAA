using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        [DisplayName("Año")]
        public short ano { get; set; }

        [Required]
        [DisplayName("Trimestre")]
        public byte trimestre { get; set; }

        // Elementos de ProgProy
        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> fechaInicio { get; set; }

        [Display(Name = "Fecha Fin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> fechaFin { get; set; }
        public int monto { get; set; }

    }
}