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
    
    public partial class itemReajuste
    {
        public int id { get; set; }
        public int idContratoItem { get; set; }
        public System.DateTime fecha { get; set; }
        public Nullable<int> mes { get; set; }
        public Nullable<int> ano { get; set; }
        public decimal reajuste { get; set; }
        public decimal precioReajustado { get; set; }
    
        public virtual contratoItem contratoItem { get; set; }
    }
}
