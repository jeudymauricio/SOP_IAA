using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(ingenieroMetaData))]
    public partial class ingeniero
    {
    }

    public class ingenieroMetaData
    {
        [DisplayName("Persona")]
        public int idPersona { get; set; }
    }
}
