using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ingenieroContratoMetaData))]
    public partial class ingenieroContrato
    {
    }

    public class ingenieroContratoMetaData
    {
        [DisplayName("Contrato")]
        public int idContrato { get; set; }

        [DisplayName("Ingeniero")]
        public int idIngeniero { get; set; }

        [Required]
        [DisplayName("Descripción")]
        [StringLength(150)]
        public string descripcion { get; set; }

        [Required]
        [DisplayName("Departamento")]
        [StringLength(150)]
        public string departamento { get; set; }

        [Required]
        [DisplayName("Rol")]
        [StringLength(150)]
        public string rol { get; set; }

        [DisplayName("Fecha de Inicio")]
        public Nullable<System.DateTime> fechaInicio { get; set; }

        [DisplayName("Fecha Fin")]
        public Nullable<System.DateTime> fechaFin { get; set; }
    }
}
