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
    
    public partial class ruta
    {
        public ruta()
        {
            this.boleta = new HashSet<boleta>();
            this.proyecto = new HashSet<proyecto>();
            this.proyecto_estructura = new HashSet<proyecto_estructura>();
            this.zona = new HashSet<zona>();
        }
    
        public int id { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<boleta> boleta { get; set; }
        public virtual ICollection<proyecto> proyecto { get; set; }
        public virtual ICollection<proyecto_estructura> proyecto_estructura { get; set; }
        public virtual ICollection<zona> zona { get; set; }
    }
}
