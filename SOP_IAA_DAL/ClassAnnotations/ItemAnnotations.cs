using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(itemMetaData))]
    public partial class item
    {
    }

    public class itemMetaData
    {
        [Required]
        [DisplayName("Código del Item")]
        [StringLength(25)]
        public string codigoItem { get; set; }

        [DisplayName("Descripción")]
        [StringLength(100)]
        public string descripcion { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Unidad de Medida")]
        public string unidadMedida { get; set; }

    }
}
