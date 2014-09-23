using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(boletaMetaData))]
    public partial class boleta
    {
    }

    public class boletaMetaData
    {
        //public int id { get; set; }
        [Required]
        [DisplayName("Número de Boleta")]
        public int numeroBoleta { get; set; }

        [Required]
        [DisplayName("Fondo")]
        public short idFondo { get; set; }

        [Required]
        [DisplayName("Ruta")]
        public int idRuta { get; set; }

        [Required]
        [DisplayName("Inspector")]
        public int idInspector { get; set; }

        [Required]
        [DisplayName("Fecha")]
        [DataType(DataType.Date)]
        public System.DateTime fecha { get; set; }

        [Required]
        [DisplayName("Sección de Control")]
        public short seccionControl { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Estacionamiento Inicial")]
        public string estacionamientoInicial { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Estacionamiento Final")]
        public string estacionamientoFinal { get; set; }

        [Required]
        [DisplayName("Periodo")]
        public byte periodo { get; set; }

        [Required]
        [DisplayName("Proyecto/Estructura")]
        public int idProyecto_Estructura { get; set; }
        

        [Required]
        [StringLength(100)]
        [DisplayName("Observaciones")]
        public string observaciones { get; set; }

        [Required]
        [DisplayName("Contrato")]
        public int idContrato { get; set; }
        /*
        public virtual ICollection<boletaItem> boletaItem { get; set; }
        public virtual fondo fondo { get; set; }
        public virtual inspector inspector { get; set; }
        public virtual proyecto_estructura proyecto_estructura { get; set; }
        public virtual ruta ruta { get; set; }*/
    }
}
