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
    
    public partial class proyecto_estructura
    {
        public proyecto_estructura()
        {
            this.boleta = new HashSet<boleta>();
        }
    
        public int id { get; set; }
        public int idRuta { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<boleta> boleta { get; set; }
        public virtual ruta ruta { get; set; }
    }
}
