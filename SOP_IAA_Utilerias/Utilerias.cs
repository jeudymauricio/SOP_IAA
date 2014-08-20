using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection; 

namespace SOP_IAA_Utilerias
{
    public class Utilerias
    {
        /// <summary>
        /// Se utiliza para gestionar los nombre de los diversos archivos de recursos
        /// </summary>
        public enum ArchivoRecurso
        {
            /// <summary>
            /// Archivo de recursos para los mensajes
            /// </summary>
            UtilRecursos,
            /// <summary>
            /// Archivo de recursos para mensajes en el módulo de seguridad
            /// </summary>
            UtilResSeguridad,
        }

        /// <summary>
        /// Retorna el valor de una llave almacenado en un archivo de recursos
        /// </summary>
        /// <param name="strArchivo">Nombre del archivo de recursos</param>
        /// <param name="strLlave">Nombre de la Llave</param>
        /// <returns>Valor de la llave</returns>
        public static string ValorRecurso(ArchivoRecurso strArchivo, string strLlave)
        {
            // Dato a retornar
            string resourceValue = string.Empty;
            try
            {
                // Crear una variable tipo "resourcemanager" para leer el arhivo de recursos
                ResourceManager resourceManager =
                new ResourceManager(
                Assembly.GetExecutingAssembly().GetName().Name +
                '.' +
                strArchivo,
                Assembly.GetExecutingAssembly());
                // Obtiene el valor a partir de la llave sumistrada
                resourceValue = resourceManager.GetString(strLlave);
            }
            catch (Exception)
            {
                resourceValue = string.Empty;
            }
            return resourceValue;
        }
    }
}
