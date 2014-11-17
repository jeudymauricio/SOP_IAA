using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(contratoItemMetaData))]
    public partial class contratoItem
    {
    }

    public class contratoItemMetaData
    {
        [DisplayName("Contrato")]
        public int idContrato { get; set; }

        [DisplayName("Item")]
        public int idItem { get; set; }

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:C4}")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.####}")]
        //[DataType(DataType.Currency)]
        [DisplayName("Precio Unitario")]
        public decimal precioUnitario { get; set; }

        [Required]
        [DisplayName("Cantidad Aprobada")]
        public decimal cantidadAprobada { get; set; }
    }
}
