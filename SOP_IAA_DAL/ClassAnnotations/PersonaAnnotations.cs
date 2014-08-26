using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(personaMetaData))]
    public partial class persona
    {
    }

    public class personaMetaData
    {

        [Required]
        [DisplayName("Nombre")]
        [StringLength(50)]
        public string nombre { get; set; }
        
        [Required]
        [DisplayName("Primer Apellido")]
        [StringLength(50)]
        public string apellido1 { get; set; }
        
        [Required]
        [DisplayName("Segundo Apellido")]
        [StringLength(50)]
        public string apellido2 { get; set; }
       
        [Required]
        [DisplayName("Cédula")]
        [StringLength(50)]
        public string cedula { get; set; }
    }
}
