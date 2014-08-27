using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(fondoMetaData))]
    public partial class fondo
    {
    }

    public class fondoMetaData
    {
        [Required]
        [DisplayName("Fondo")]
        [StringLength(20)]
        public string nombre { get; set; }

    }
}
