using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(rutaMetaData))]
    public partial class ruta
    {
    }

    public class rutaMetaData
    {
        [Required]
        [DisplayName("Ruta")]
        public string nombre { get; set; }
    }
}
