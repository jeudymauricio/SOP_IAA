using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(itemReajusteMetaData))]
    public partial class itemReajuste
    {
    }

    public class itemReajusteMetaData
    {
        [Required]
        [DisplayName("Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fecha { get; set; }

        [DisplayName("Mes")]
        public Nullable<int> mes { get; set; }

        [DisplayName("Año")]
        public Nullable<int> ano { get; set; }

        [Required]
        [DisplayName("Reajuste")]
        public decimal reajuste { get; set; }

        [Required]
        [DisplayName("Precio Reajustado")]
        public decimal precioReajustado { get; set; }

        public virtual contratoItem contratoItem { get; set; }

    }
}
