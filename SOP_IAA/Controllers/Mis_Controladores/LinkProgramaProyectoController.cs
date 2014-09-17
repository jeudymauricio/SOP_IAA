using SOP_IAA_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SOP_IAA.Controllers.Mis_Controladores
{
    public class LinkProgramaProyectoController : Controller
    {
        // Conexión a la base de datos
        private Proyecto_IAAEntities db = new Proyecto_IAAEntities();

        // Acción que desplige la lista de proyectos de un programa específico
        public ActionResult MisProyectos()
        {

            return View();
        }
    }
}