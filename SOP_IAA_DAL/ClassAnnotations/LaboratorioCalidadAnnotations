using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(laboratorioCalidadMetaData))]
    public partial class laboratorioCalidad
    {
    }

    public class laboratorioCalidadMetaData
    {
        [Required]
        [StringLength(30)]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Required]
        [DisplayName("Tipo")]
        [StringLength(30)]
        public string tipo { get; set; }
    }
}
