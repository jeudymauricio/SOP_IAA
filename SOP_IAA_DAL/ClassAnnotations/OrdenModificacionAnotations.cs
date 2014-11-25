using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ordenModificacionMetaData))]
    public partial class ordenModificacion
    {
    }

    public class ordenModificacionMetaData
    {

        [DisplayName("Contrato")]
        public int idContrato { get; set; }

        [Required]
        [DisplayName("Número de Oficio de Aprobación")]
        public string numeroOficio { get; set; }

        [Required]
        [DisplayName("Fecha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fecha { get; set; }

        [Required]
        [DisplayName("Objeto de la Orden de Modificación")]
        [StringLength(200)]
        public string objetoOM { get; set; }

        [DisplayName("Aumenta de plazo (Días)")]
        public short AumentoPlazo { get; set; }
    }
}
