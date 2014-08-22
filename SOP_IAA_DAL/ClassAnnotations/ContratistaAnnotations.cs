using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(contratistaMetaData))]
    public partial class contratista
    {
    }

    public class contratistaMetaData
    {
        [Required]
        [DisplayName("Contratista")]
        [StringLength(50)]
        public string nombre { get; set; }

        [DisplayName("Descripción")]
        [StringLength(60)]
        public string descripcion { get; set; }
    }
}
