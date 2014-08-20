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
    
    public partial class Proyecto
    {
        public Proyecto()
        {
            this.contratistaProyecto = new HashSet<contratistaProyecto>();
            this.ingenieroProyecto = new HashSet<ingenieroProyecto>();
            this.programa = new HashSet<programa>();
            this.laboratorioCalidad = new HashSet<laboratorioCalidad>();
            this.zona = new HashSet<zona>();
        }
    
        public int id { get; set; }
        public string contratacion { get; set; }
        public short lineaContrato { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public short plazo { get; set; }
        public string lugar { get; set; }
        public short idFondo { get; set; }
    
        public virtual ICollection<contratistaProyecto> contratistaProyecto { get; set; }
        public virtual fondo fondo { get; set; }
        public virtual ICollection<ingenieroProyecto> ingenieroProyecto { get; set; }
        public virtual ICollection<programa> programa { get; set; }
        public virtual ICollection<laboratorioCalidad> laboratorioCalidad { get; set; }
        public virtual ICollection<zona> zona { get; set; }
    }
}
