using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL.ClassAnnotations
{
    [MetadataType(typeof(telefonoMetaData))]
    public partial class telefono
    {
    }

    public class telefonoMetaData
    {
        [Required]
        [DisplayName("Teléfono")]
        public string nombre { get; set; }
    }
}
