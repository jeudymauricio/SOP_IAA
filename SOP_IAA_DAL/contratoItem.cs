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
    
    public partial class contratoItem
    {
        public contratoItem()
        {
            this.boletaItem = new HashSet<boletaItem>();
            this.itemReajuste = new HashSet<itemReajuste>();
            this.subproyectoContratoItem = new HashSet<subproyectoContratoItem>();
            this.pICI = new HashSet<pICI>();
        }
    
        public int id { get; set; }
        public int idContrato { get; set; }
        public int idItem { get; set; }
        public decimal precioUnitario { get; set; }
    
        public virtual ICollection<boletaItem> boletaItem { get; set; }
        public virtual Contrato Contrato { get; set; }
        public virtual item item { get; set; }
        public virtual ICollection<itemReajuste> itemReajuste { get; set; }
        public virtual ICollection<subproyectoContratoItem> subproyectoContratoItem { get; set; }
        public virtual ICollection<pICI> pICI { get; set; }
    }
}
