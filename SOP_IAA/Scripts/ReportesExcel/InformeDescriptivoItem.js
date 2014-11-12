$(document).ready(function () {

    // Funcionalidad de la tabla
    $('#tbReportes').dataTable({
        "language": {
            "url": "/Scripts/plugins/dataTables/Spanish.txt"
        }
    });

    //Date range picker
    $('#fechaInicio').datetimepicker({
        language:  'es',
        autoclose: true,
        format: "dd/mm/yyyy",
        startView: 'month',
        minView: 'month'
    });
    //.daterangepicker({
    //    format: 'DD/MM/YYYY',
    //    showDropdowns: true,
    //    locale: {
    //        applyLabel: 'Aplicar',
    //        cancelLabel: 'Cancelar',
    //        fromLabel: 'Desde',
    //        toLabel: 'Hasta',
    //        weekLabel: 'W',
    //        customRangeLabel: 'Rango Personalizado',
    //        daysOfWeek: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
    //        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    //        firstDay: 0
    //    },
    //    ranges: {
    //        'Hoy': [moment(), moment()],
    //        'Ayer': [moment().subtract('days', 1), moment().subtract('days', 1)],
    //        'Últimos 7 Días': [moment().subtract('days', 6), moment()],
    //        'Últimos 30 Días': [moment().subtract('days', 29), moment()],
    //        'Este Mes': [moment().startOf('month'), moment().endOf('month')],
    //        'Mes Pasado': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
    //    },
    //    startDate: moment().subtract('days', 29),
    //    endDate: moment()
    //});

    $('#fechaFin').datetimepicker({
        language:  'es',
        autoclose: true,
        format: "dd/mm/yyyy",
        startView: 'month',
        minView: 'month'
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