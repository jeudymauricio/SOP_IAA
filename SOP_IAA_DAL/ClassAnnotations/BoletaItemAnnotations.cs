using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(boletaItemMetaData))]
    public partial class boletaItem
    {
    }

    public class boletaItemMetaData
    {
        [Required]
        [DisplayName("Contrato")]
        public int idContratoItem { get; set; }

        [Required]
        [DisplayName("Boleta")]
        public int idBoleta { get; set; }

        [Required]
        [DisplayName("Cantidad")]
        public decimal cantidad { get; set; }

        [Required]
        [DisplayName("Costo Total")]
        public decimal costoTotal { get; set; }

        [Required]
        [DisplayName("Redimientos")]
        public decimal redimientos { get; set; }

    }
}
