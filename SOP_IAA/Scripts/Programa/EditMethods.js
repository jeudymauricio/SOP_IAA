// antiguo año del programa
var anoAnt;

// antiguo trimestre
var triAnt;

$(document).ready(

    anoAnt = $('#editorAno').val(),
    triAnt = $('#editorTri').val(),

    //Antes de ir a la acción Post del submit, se agregan los ingenieros y labs modificados
    $("#formEditProgram").submit(function (eventObj) {

        // Antiguo año del programa
        $('<input />').attr('type', 'hidden')
            .attr('name', "anoAnt")
            .attr('value', anoAnt.toString())
            .appendTo('#formEditProgram');

        // Antiguo trimestre del programa
        $('<input />').attr('type', 'hidden')
            .attr('name', "triAnt")
            .attr('value', triAnt.toString())
            .appendTo('#formEditProgram');

        return true;
    }),

    // Función del DatePicker en los campos de Fecha
    $("#txtFechaInicio").datepicker({ dateFormat: 'dd/mm/yy' }),
    $("#txtFechaFin").datepicker(),
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: 'Anterior',
        nextText: 'Siguiente',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    },
    $.datepicker.setDefaults($.datepicker.regional['es'])
)