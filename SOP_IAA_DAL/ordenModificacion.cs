//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SOP_IAA_DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class ordenModificacion
    {
        public ordenModificacion()
        {
            this.oMCI = new HashSet<oMCI>();
        }
    
        public int id { get; set; }
        public int idContrato { get; set; }
        public string numeroOficio { get; set; }
        public System.DateTime fecha { get; set; }
        public string objetoOM { get; set; }
    
        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<oMCI> oMCI { get; set; }
    }
}
