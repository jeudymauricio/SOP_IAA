using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(telefonoMetaData))]
    public partial class telefono
    {
    }

    public class telefonoMetaData
    {
        [Required]
        [RegularExpression("[1-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]")]
        [DisplayName("Teléfono")]
        public string telefono1 { get; set; }
    }
}
