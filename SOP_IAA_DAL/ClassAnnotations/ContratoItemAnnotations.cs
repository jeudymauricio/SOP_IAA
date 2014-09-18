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
        [Required]
        [DisplayName("Precio Unitario")]
        public int precioUnitario { get; set; }

    }
}
