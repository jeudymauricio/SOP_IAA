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
    
    public partial class fondo
    {
        public fondo()
        {
            this.boleta = new HashSet<boleta>();
            this.Contrato = new HashSet<Contrato>();
        }
    
        public short id { get; set; }
        public string nombre { get; set; }
    
        public virtual ICollection<boleta> boleta { get; set; }
        public virtual ICollection<Contrato> Contrato { get; set; }
    }
}
