using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace SOP_IAA_DAL
{
    public interface IRepositorio<TEntidad> : 
        IDisposable where TEntidad:class
    {
        TEntidad Insertar(TEntidad entidadAgregar);

        bool Eliminar(TEntidad entidadEliminar);

        bool Actualizar(TEntidad entidadActualizar);

        TEntidad SeleccionarUno(Expression<Func<TEntidad, bool>> criterio);

        List<TEntidad> SeleccionarTodos();

        List<TEntidad> Buscar(Expression<Func<TEntidad, bool>> criterio);
    }
}
