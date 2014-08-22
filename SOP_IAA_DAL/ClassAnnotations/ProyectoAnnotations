using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ProyectoMetaData))]
    public partial class Proyecto
    {
    }

    public class ProyectoMetaData
    {
        [Required]
        [DisplayName("Nombre y número de contratación")]
        [StringLength(30)]
        public string contratacion { get; set; }

        [Required]
        [DisplayName("Línea del contrato")]
        [Range(0, Int16.MaxValue, ErrorMessage = "La línea debe estar en un rango de 0 a 32700")]
        public short? lineaContrato { get; set; }
        
        [Required]
        [DisplayName("Fecha Inicio Proyecto")]
        public System.DateTime fechaInicio { get; set; }
        
        [Required]
        [DisplayName("Plazo (días)")]
        [Range(0, Int16.MaxValue, ErrorMessage = "Plazo debe ser en días y estar en un rango de 0 a 32700")]
        public short plazo { get; set; }
        
        [Required]
        [DisplayName("Lugar")]
        public string lugar { get; set; }

        [Required]
        [DisplayName("Nombre Fondo")]
        public short idFondo { get; set; }
    }
}
