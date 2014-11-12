using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(zonaMetaData))]
    public partial class zona
    {
    }

    public class zonaMetaData
    {
        [Required]
        [DisplayName("Zona")]
        [StringLength(10)]
        public string nombre { get; set; }
    }
}
