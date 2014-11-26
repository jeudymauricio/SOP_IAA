using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    [MetadataType(typeof(usuarioMetaData))]
    public partial class usuario
    {
    }

    public class usuarioMetaData
    {
        [Required]
        [DisplayName("Persona")]
        public int idPersona { get; set; }

        [Required]
        [DisplayName("Nombre de Usuario")]
        public string nombreUsuario { get; set; }

        [Required(ErrorMessage = "Se requiere una contraseña")]
        [StringLength(30, ErrorMessage = "La contraseña es de mínimo 5 caracteres y máximo 30", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string contrasena { get; set; }

        /*[Required(ErrorMessage = "Se requiere una contraseña")]
        [StringLength(30, ErrorMessage = "La contraseña es de mínimo 5 caracteres y máximo 30", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("contrasena")]
        public string ConfirmContrasena { get; set; }*/


        [DisplayName("Nivel de Privilegios")]
        public byte tipo { get; set; }
    }
}
