using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SOP_IAA_DAL;

namespace SOP_IAA.Models
{
    public class LinkPersonaTelefono
    {
        public persona persona { get; set; }
        public telefono telefono { get; set; }
    }
}