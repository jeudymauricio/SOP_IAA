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
    
    public partial class programa
    {
        public programa()
        {
            this.proyecto = new HashSet<proyecto>();
        }
    
        public int id { get; set; }
        public int idContrato { get; set; }
        public short ano { get; set; }
        public byte trimestre { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaFin { get; set; }
        public decimal monto { get; set; }
    
        public virtual Contrato Contrato { get; set; }
        public virtual ICollection<proyecto> proyecto { get; set; }
    }
}
