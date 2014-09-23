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
    
    public partial class boleta
    {
        public boleta()
        {
            this.boletaItem = new HashSet<boletaItem>();
        }
    
        public int id { get; set; }
        public int numeroBoleta { get; set; }
        public short idFondo { get; set; }
        public int idRuta { get; set; }
        public int idInspector { get; set; }
        public System.DateTime fecha { get; set; }
        public short seccionControl { get; set; }
        public string estacionamientoInicial { get; set; }
        public string estacionamientoFinal { get; set; }
        public byte periodo { get; set; }
        public int idProyecto_Estructura { get; set; }
        public string observaciones { get; set; }
        public int idContrato { get; set; }
    
        public virtual ICollection<boletaItem> boletaItem { get; set; }
        public virtual fondo fondo { get; set; }
        public virtual inspector inspector { get; set; }
        public virtual proyecto_estructura proyecto_estructura { get; set; }
        public virtual ruta ruta { get; set; }
        public virtual Contrato Contrato { get; set; }
    }
}
