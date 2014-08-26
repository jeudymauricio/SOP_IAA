using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ingenieroMetaData))]
    public partial class ingeniero
    {
    }

    public class ingenieroMetaData
    {
        [Required]
        [DisplayName("Descripción")]
        [StringLength(100)]
        public string descripcion { get; set; }
        [Required]
        [DisplayName("Departamento")]
        [StringLength(100)]
        public string departamento { get; set; }
        [Required]
        [DisplayName("Rol")]
        [StringLength(100)]
        public string rol { get; set; }
    }
}
