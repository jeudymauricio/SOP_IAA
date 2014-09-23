using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(proyecto_estructuraMetaData))]
    public partial class proyecto_estructura
    {
    }

    public class proyecto_estructuraMetaData
    {

        [DisplayName("Ruta")]
        public int idRuta { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }

    }
}
