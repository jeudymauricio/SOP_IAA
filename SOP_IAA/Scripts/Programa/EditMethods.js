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
    })
)