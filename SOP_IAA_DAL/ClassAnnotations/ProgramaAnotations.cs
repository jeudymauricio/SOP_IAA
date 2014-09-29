using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(programaMetaData))]
    public partial class programa
    {
    }

    public class programaMetaData
    {

        [DisplayName("Contrato")]
        public int idContrato { get; set; }

        [DisplayName("Año")]
        public short ano { get; set; }

        [DisplayName("Trimestre")]
        public byte trimestre { get; set; }

        [DisplayName("Fecha de Inicio")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fechaInicio { get; set; }

        [DisplayName("Fecha de Fin")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime fechaFin { get; set; }

        [DisplayName("Monto del Programa")]
        public decimal monto { get; set; }
  
    }
}
