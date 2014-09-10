using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(tipoProyectoMetaData))]
    public partial class tipoProyecto
    {
    }

    public class tipoProyectoMetaData
    {
        [Required]
        [DisplayName("Tipo de proyecto")]
        public string nombre { get; set; }
    }
}
