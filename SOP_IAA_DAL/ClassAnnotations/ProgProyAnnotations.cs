using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(progProyMetaData))]
    public partial class progProy
    {
    }

    public class progProyMetaData
    {
        [Required]
        [DisplayName("Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> fechaInicio { get; set; }

        [Required]
        [DisplayName("Fecha de Fin")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> fechaFin { get; set; }

        [Required]
        [DisplayName("Monto Estimado")]
        public int monto { get; set; }

  
    }
}
