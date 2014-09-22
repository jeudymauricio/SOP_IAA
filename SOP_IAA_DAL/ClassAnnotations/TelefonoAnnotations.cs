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

        [DisplayName("Teléfono")]
        [StringLength(9)]
        [RegularExpression("[1-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]")]
        public string telefono1 { get; set; }
    }
}
