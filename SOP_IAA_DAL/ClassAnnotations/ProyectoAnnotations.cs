using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(proyectoMetaData))]
    public partial class proyecto
    {
    }

    public class proyectoMetaData
    {
        [Required]
        [DisplayName("Nombre de Proyecto")]
        public string nombre { get; set; }

    }
}
