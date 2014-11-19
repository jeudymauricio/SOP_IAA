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
    
    public partial class Contrato
    {
        public Contrato()
        {
            this.boleta = new HashSet<boleta>();
            this.contratoItem = new HashSet<contratoItem>();
            this.ingenieroContrato = new HashSet<ingenieroContrato>();
            this.ordenModificacion = new HashSet<ordenModificacion>();
            this.planInversion = new HashSet<planInversion>();
            this.laboratorioCalidad = new HashSet<laboratorioCalidad>();
        }
    
        public int id { get; set; }
        public int idContratista { get; set; }
        public string licitacion { get; set; }
        public short lineaContrato { get; set; }
        public short idZona { get; set; }
        public System.DateTime fechaInicio { get; set; }
        public short plazo { get; set; }
        public string lugar { get; set; }
        public short idFondo { get; set; }
    
        public virtual ICollection<boleta> boleta { get; set; }
        public virtual contratista contratista { get; set; }
        public virtual ICollection<contratoItem> contratoItem { get; set; }
        public virtual ICollection<ingenieroContrato> ingenieroContrato { get; set; }
        public virtual ICollection<ordenModificacion> ordenModificacion { get; set; }
        public virtual ICollection<planInversion> planInversion { get; set; }
        public virtual fondo fondo { get; set; }
        public virtual zona zona { get; set; }
        public virtual ICollection<laboratorioCalidad> laboratorioCalidad { get; set; }
    }
}
