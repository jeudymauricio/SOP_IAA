var startDate;
var endDate;

$(document).ready(function () {

    $('#tbItemsContrato').dataTable({
        "language": {
            "url": "/Scripts/plugins/dataTables/Spanish.txt"
        }
    });

    //$('#txtDateRange').daterangepicker({
    //    format: 'D/MM/YYYY',
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
    //},
    //function (start, end, label) {
    //    startDate = start.format('DD/MM/YYYY');
    //    endDate = end.format('DD/MM/YYYY');
    //    //alert('A date range was chosen: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD'));
    //});

    $('#btnRangoFechas').click(function () {
        var inputFechaInicio = document.getElementById('fechaInicio');
        var inputFechaFin = document.getElementById('fechaFin');

        var fecha1 = inputFechaInicio.value;
        var fecha2 = inputFechaFin.value;
        //var fecha1 = startDate;
        //var fecha2 = endDate;

        if (fecha1 == "" || fecha2 == "") {
            alert("No deje los campos vacíos");
            return false;
        }
        else if (fecha1 > fecha2) {
            alert("La fecha de inicio debe de ser menor que la final");
            return false;
        }
        else {
            return true;
        }
    });
    //// Función del DatePicker en los campos de Fecha
    $("#fechaInicio").datetimepicker({
        language: 'es',
        autoclose: true,
        format: "dd/mm/yyyy",
        startView: 'month',
        minView: 'month'
    });
    $("#fechaFin").datetimepicker({
        language: 'es',
        autoclose: true,
        format: "dd/mm/yyyy",
        startView: 'month',
        minView: 'month'
    });
});