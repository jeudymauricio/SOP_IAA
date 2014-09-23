using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ProyectoItemMetaData))]
    public partial class proyectoItem
    {
    }

    public class ProyectoItemMetaData
    {

        [Required]
        [DisplayName("Proyecto")]
        public int idProyecto { get; set; }

        [Required]
        [DisplayName("Item")]
        public int idItem { get; set; }

        [Required]
        [DisplayName("Fecha Inicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Fecha de Fin")]
        public System.DateTime fechaFin { get; set; }

        [Required]
        [DisplayName("Costo Estimado")]
        public int costoEstimado { get; set; }
    }
}
