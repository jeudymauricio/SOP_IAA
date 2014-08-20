//-----------------------------------------------------------------------------------------------------------
// TEC - Instituto Tecnologico de Costa Rica
//     Implementación del patrón repositorio descrito en MSDN
//
//     La declaración debe ser: var nombreVariable = Repositorio<TEndidad>
//     Las entidades deben encontrarse definidas en el proyecto de Entidades
//     
//     con esto el objeto repositorio tendrá los métodos:
///    - Insertar
///    - Actualizar
///    - Eliminar
///    - SeleccionarTodos
///    - SeleccionarUno  (criterio Linq)
///    - Buscar (criterio Linq)
//-----------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    public class Repositorio<TEndidad> : IRepositorio<TEndidad> where TEndidad:class
    {
        public readonly Proyecto_IAAEntities ContextoEF = null;

        public Repositorio()
        {
            ContextoEF = new Proyecto_IAAEntities();
        }

        private  DbSet<TEndidad> Entidades
        {
            get
            {
                return ContextoEF.Set<TEndidad>();
            }
    }

        public TEndidad Insertar(TEndidad entidadAgregar)
        {
            TEndidad resultado = null;
            try
            {
                //throw new Exception("Error de prueba");
                Entidades.Add(entidadAgregar);
                ContextoEF.SaveChanges();
                resultado = entidadAgregar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultado;
        }

        public bool Eliminar(TEndidad entidadEliminar)
        {
            bool resultado = false;
            try
            {
                Entidades.Attach(entidadEliminar);
                Entidades.Remove(entidadEliminar);
                resultado = ContextoEF.SaveChanges() > 0;
            }
            catch
            {

            }
            return resultado;
        }

        public bool Actualizar(TEndidad entidadActualizar)
        {
            bool resultado = false;
            try
            {
                Entidades.Attach(entidadActualizar);
                ContextoEF.Entry(entidadActualizar).State = EntityState.Modified;
                resultado = ContextoEF.SaveChanges() > 0;

            }
            catch
            {

            }
            return resultado;
        }

        public TEndidad SeleccionarUno(System.Linq.Expressions.Expression<Func<TEndidad, bool>> criterio)
        {
            TEndidad resultado = null;
            try
            {
                resultado = Entidades.FirstOrDefault(criterio);
            }
            catch
            {
            }
            return resultado;
        }

        public List<TEndidad> SeleccionarTodos()
        {
            List<TEndidad> resultado = null;
            try
            {
                resultado = Entidades.ToList();
            }
            catch
            {
            }
            return resultado;
        }

        public List<TEndidad> Buscar(System.Linq.Expressions.Expression<Func<TEndidad, bool>> criterio)
        {
            List<TEndidad> resultado = null;
            try
            {
                resultado = Entidades.Where(criterio).ToList();
            }
            catch
            {
            }
            return resultado;
        }

        public void Dispose()
        {
            if (ContextoEF != null)
                ContextoEF.Dispose();
        }
    }
}
