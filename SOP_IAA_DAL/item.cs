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
    
    public partial class item
    {
        public item()
        {
            this.contratoItem = new HashSet<contratoItem>();
            this.proyectoItem = new HashSet<proyectoItem>();
        }
    
        public int id { get; set; }
        public string codigoItem { get; set; }
        public string descripcion { get; set; }
        public string unidadMedida { get; set; }
    
        public virtual ICollection<contratoItem> contratoItem { get; set; }
        public virtual ICollection<proyectoItem> proyectoItem { get; set; }
    }
}
