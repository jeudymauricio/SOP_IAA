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
    
    public partial class proyectoItemReajuste
    {
        public int id { get; set; }
        public int idProyecto { get; set; }
        public int idContratoItem { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public System.DateTime fechaFin { get; set; }
        public decimal costoEstimado { get; set; }
    
        public virtual contratoItem contratoItem { get; set; }
        public virtual proyecto proyecto { get; set; }
    }
}
