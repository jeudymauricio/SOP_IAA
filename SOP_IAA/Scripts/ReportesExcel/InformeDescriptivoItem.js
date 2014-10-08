$(document).ready(function () {

    // Funcionalidad de la tabla
    $('#tbReportes').dataTable({
        "language": {
            "url": "/Scripts/plugins/dataTables/Spanish.txt"
        }
    });

})

// Función que llama a la acción de controlador 'ExportarInformeDescriptivoItem' para exportar el informe descriptivo de ítem a excel
function ExportarInformeDescriptivoItem(_id) {
    $.ajax({
        url: '/Reportes/ExportarInformeDescriptivoItem/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = data;
            if (json.status == "ok") {
                alert(json.Correcto);
            }
            else {
                alert(json.Error);
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status == 400) {
                // Bad request
                alert('Error: Consulta inválida.\nRecargue la página e intente de nuevo.');
            }
            else if (xhr.status === 401) {
                // Unauthorized error
                alert('Error: Acceso denegado.\n Verifique que tenga privilegios para realizar la operación.');
            }
            else if (xhr.status == 404) {
                // Not found
                alert('Error: no se encontraron los detalles del reporte.\nVerifique que el reporte existe y puede acceder a él.');
            }
            else if (xhr.status == 500) {
                // Server side error
                alert('Error del servidor.\n Espere unos segundos y vuelva a reitentar.');
            }
            else {
                alert('Error: \n ' + errorThrown + 'Reitente de nuevo.');
            }
        }
    })
}