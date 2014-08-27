using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ContratoMetaData))]
    public partial class Contrato
    {
    }

    public class ContratoMetaData
    {
        [Required]
        [DisplayName("Contratista")]
        public int idContratista { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Contratación")]
        public string licitacion { get; set; }

        [Required]
        [DisplayName("Linea de Contrato")]
        public short lineaContrato { get; set; }

        [DisplayName("Zona")]
        public short idZona { get; set; }

        [Required]
        [DisplayName("Fecha de Inicio")]
        public System.DateTime fechaInicio { get; set; }

        [Required]
        [DisplayName("Plazo (días)")]
        public short plazo { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Lugar")]
        public string lugar { get; set; }

        [Required]
        [DisplayName("Fondo")]
        public short idFondo { get; set; }
    }
}
