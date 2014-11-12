using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(seccionControlMetaData))]
    public partial class seccionControl
    {
    }

    public class seccionControlMetaData
    {
        [Required]
        [DisplayName("Ruta")]
        public int idRuta { get; set; }

        [Required]
        [DisplayName("Número de Sección")]
        public int seccion { get; set; }

        [Required]
        [DisplayName("Descripción")]
        public string descripcion { get; set; }
    }
}
