// Array que contendrá los id de los ingenieros del proyecto
var ingenieros = [];
var ingenierosAnteriores = [];

// Array que contendrá los id de los laboratorios del proyecto
var laboratorios = [];
var laboratoriosAnteriores = [];

$(document).ready(
    
    // Función del DatePicker en los campos de Fecha
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
    $.datepicker.setDefaults($.datepicker.regional['es']),
    $("#txtFechaInicio").datepicker({ dateFormat: 'dd/mm/yy' }),

    // Función del boton 'Agregar Ingeniero'
    $('#btnAgregarIngeniero').click(function () {
        var dd = document.getElementById('ddlIngenieros')
        var _id = dd.options[dd.selectedIndex].value;

        // Este ajax se realiza una acción de cobtrolador donde envía el id del ingeniero a buscar y recibe como retorno un JSON con los detalles del Ingeniero
        $.ajax({
            url: '/ContratoView/IngenieroDetalles/',
            type: "GET",
            dataType: "json",
            data: {
                id: _id
            },
            success: function (data) {
                var json = $.parseJSON(data);
                ingenieros.push(_id.toString());

                var fila = '<tr id=' + json.persona.id + '><td>' + json.persona.nombre + ' ' + json.persona.apellido1 + ' ' + json.persona.apellido2 + '</td> ';
                fila += '<td>' + json.rol + '</td>';
                fila += '<td>' + json.descripcion + '</td>';
                fila += '<td>' + json.departamento + '</td>';
                fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarIngeniero(' + json.persona.id + ')">Quitar Ingeniero</button> </td></tr>';

                //Agrega el ingeniero a la tabla htlm
                $('#tbIngenieros > tbody:last').append(fila);

                //Elimina el ingeniero del dropdownlist
                $("#ddlIngenieros option:selected").remove();

            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        })
    }),

    $('#btnAgregarLaboratorio').click(function () {
        var dd = document.getElementById('ddlLaboratorios')
        var _id = dd.options[dd.selectedIndex].value;

        // Este ajax se realiza una acción de cobtrolador donde envía el id del laboratorio a buscar y recibe como retorno un JSON con los detalles del laboratorio
        $.ajax({
            url: '/ContratoView/LaboratorioDetalles/',
            type: "GET",
            dataType: "json",
            data: {
                id: _id
            },

            success: function (data) {
                var json = $.parseJSON(data);
                laboratorios.push(_id.toString());
                //console.log("array -> " + laboratorios.toString());

                var fila = '<tr id=' + json.id + '><td>' + json.nombre + '</td> ';
                fila += '<td>' + json.tipo + '</td>';
                fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarLaboratorio(' + json.id + ')">Quitar Laboratorio</button> </td></tr>';

                //Agrega el ingeniero a la tabla htlm
                $('#tbLaboratorios > tbody:last').append(fila);

                //Elimina el ingeniero del dropdownlist
                $("#ddlLaboratorios option:selected").remove();

            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        })
    }),

    //Antes de ir a la acción Post del submit, se agregan los ingenieros y labs modificados
    $("#formEditContract").submit(function (eventObj) {
        var jsonDatosIngenieros = { "Ingenieros": ingenieros };
        var jsonDatosIngenierosAnteriores = { "Ingenieros": ingenierosAnteriores };
        var jsonDatosLaboratorios = { "Laboratorios": laboratorios };
        var jsonDatosLaboratoriosAnteriores = { "Laboratorios": laboratoriosAnteriores };

        // Json de los ingenieros que va a tener el contrato
        $('<input />').attr('type', 'hidden')
            .attr('name', "jsonIng")
            .attr('value', $.toJSON(jsonDatosIngenieros))
            .appendTo('#formEditContract');

        // Json de los antiguos ingenieros del contrato
        $('<input />').attr('type', 'hidden')
            .attr('name', "jsonIngAnt")
            .attr('value', $.toJSON(jsonDatosIngenierosAnteriores))
            .appendTo('#formEditContract');

        // Json de los laboratorios que va a tener el contrato
        $('<input />').attr('type', 'hidden')
            .attr('name', "jsonLab")
            .attr('value', $.toJSON(jsonDatosLaboratorios))
            .appendTo('#formEditContract');

        // Json de los antiguos laboratorios del contrato
        $('<input />').attr('type', 'hidden')
            .attr('name', "jsonLabAnt")
            .attr('value', $.toJSON(jsonDatosLaboratoriosAnteriores))
            .appendTo('#formEditContract');

        return true;
    }),


    // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros
    $(document).on("click", "#tbIngenieros button.remove", function () {
        $(this).parents("tr").remove();
    }),

    // Función que permite quitar una fila con los detalles del laboratorio seleccionado en la sección Laboratorios
    $(document).on("click", "#tbLaboratorios button.remove", function () {
        $(this).parents("tr").remove();
    }),

    // Función que recorre la tabla de ingenieros al cargar.
    $('#tbIngenieros > tbody  > tr').each(function () {
        //Se obtiene el id de cada tr que corresponde al id de los ingenieros para eliminar al ingeniero del dropdownlist
        $("#ddlIngenieros option[value='" + $(this).attr('id') + "']").remove();

        //Se agrega el ingeniero a la lista global
        ingenieros.push($(this).attr('id'));
        ingenierosAnteriores.push($(this).attr('id'));
        
        //console.log($(this).find("td:first").text());
    }),

    // Función que recorre la tabla de laboratorios al cargar.
    $('#tbLaboratorios > tbody  > tr').each(function () {
        //Se obtiene el id de cada tr que corresponde al id de los lab para eliminar al lab del dropdownlist
        $("#ddlLaboratorios option[value='" + $(this).attr('id') + "']").remove();

        //Se agrega el lab a la lista global
        laboratorios.push($(this).attr('id'));
        laboratoriosAnteriores.push($(this).attr('id'));
    })
);

function eliminarIngeniero(_id) {
    $.ajax({
        url: '/ContratoView/IngenieroDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = $.parseJSON(data);

            // Saca el ingeniero de la lista global
            for (var i = ingenieros.length - 1; i >= 0; i--) {
                if (ingenieros[i] == _id) {
                    ingenieros.splice(i, 1);
                }
            }
            $("<option value=" + json.persona.id + ">" + json.persona.nombre + " " + json.persona.apellido1 + " " + json.persona.apellido2 + "</option>").appendTo("#ddlIngenieros");
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

function eliminarLaboratorio(_id) {
    $.ajax({
        url: '/ContratoView/LaboratorioDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = $.parseJSON(data);
            
            //Saca el laboratorio de la lista global
            for (var i = laboratorios.length - 1; i >= 0; i--) {
                if (laboratorios[i] == _id) {
                    laboratorios.splice(i, 1);
                }
            }
            $("<option value=" + json.id + ">" + json.nombre + "</option>").appendTo("#ddlLaboratorios");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}
