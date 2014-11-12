﻿/*
 *---------------------------------------------- Métodos de la vista 'Edit' del ContratoController ---------------------------------------------- 
 */

$(document).ready(
    function () {

        $("#txtFechaInicio").daterangepicker({
            singleDatePicker: true,
            format: 'DD/MM/YYYY',
            showDropdowns: true,
            locale: {
                applyLabel: 'Aplicar',
                cancelLabel: 'Cancelar',
                fromLabel: 'Desde',
                toLabel: 'Hasta',
                weekLabel: 'W',
                customRangeLabel: 'Rango Personalizado',
                daysOfWeek: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                firstDay: 0
            }
        });

        //Antes de ir a la acción Post del submit, se agregan los ingenieros y labs modificados
        $("#formEditContract").submit(function (eventObj) {
            // Array que contendrá los id de los ingenieros del proyecto
            var ingenieros = [];

            // Array que contendrá los id de los laboratorios del proyecto
            var laboratorios = [];

            // Bandera que indicará si debe hacerse el submit o no
            var doSubmit = true;

            // Se agregan a la lista todos los ingenieros de la tabla ingenieros
            $('#tbIngenieros > tbody > tr').each(function () {
                // Objeto simple que contendrá los detalles de cada ingeniero
                var singleObj = {}
                singleObj['idIngeniero'] = $(this).attr('id');
                singleObj['Cargo'] = $(this).children("td").eq(1).find('textarea:eq(0)').val();
                singleObj['Descripcion'] = $(this).children("td").eq(2).find('textarea:eq(0)').val();
                singleObj['DeptEmpr'] = $(this).children("td").eq(3).find('textarea:eq(0)').val();

                if ((singleObj.Cargo == "") || (singleObj.Descripcion == "") || (singleObj.DeptEmpr == "")) {
                    alert("No deje espacios vacíos");
                    doSubmit = false;
                    return false;
                }

                // Se agrega el objeto con los detalles del ingeniero
                ingenieros.push(singleObj);
            });

            // Si ha un error se cancela el submit
            if (!doSubmit) {
                return false;
            }

            // Se agregan a la lista todos los laboratorios de la tabla laboratorios
            $('#tbLaboratorios > tbody > tr').each(function () {
                laboratorios.push($(this).attr('id').toString());
            });

            // Se preparan las listas antes de convertirlas a JSON
            var jsonDatosIngenieros = { "Ingenieros": ingenieros };
            var jsonDatosLaboratorios = { "Laboratorios": laboratorios };

            // Json de los ingenieros que va a tener el contrato
            $('<input />').attr('type', 'hidden')
                .attr('name', "jsonIng")
                .attr('value', $.toJSON(jsonDatosIngenieros))
                .appendTo('#formEditContract');

            // Json de los laboratorios que va a tener el contrato
            $('<input />').attr('type', 'hidden')
                .attr('name', "jsonLab")
                .attr('value', $.toJSON(jsonDatosLaboratorios))
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
        }),

        // Función que recorre la tabla de laboratorios al cargar.
        $('#tbLaboratorios > tbody  > tr').each(function () {
            //Se obtiene el id de cada tr que corresponde al id de los lab para eliminar al lab del dropdownlist
            $("#ddlLaboratorios option[value='" + $(this).attr('id') + "']").remove();
        });

        // Funciones de autocomplete en los dropdown
        $("#ddlIdContratista").combobox();
        $("#ddlIdZona").combobox();
        $("#ddlIdFondo").combobox();
        $("#ddlIngenieros").combobox();
        $("#ddlLaboratorios").combobox();
    });

// Función del boton 'Agregar Ingeniero'
$('#btnAgregarIngeniero').click(function () {
    var dd = document.getElementById('ddlIngenieros')
    try {
        // Se trata de obtener el valor del dropdown
        var _id = dd.options[dd.selectedIndex].value;
        var _nombreIngeniero = $("#ddlIngenieros option:selected").text();
    } catch (error) {
        return false;
    }

    // Se deshabilita el boton mientras se realiza la acción
    $(this).toggleClass('disabled', true);

    // 
    var fila = '<tr id=' + _id + '><td>' + _nombreIngeniero + '</td> ';
    fila += '<td style="text-align:center"><textarea rows="2" class="form-control" id="txaRol' + _id + '" name="txaRol' + _id + '" style="resize:vertical; min-width:150px" placeholder="Máximo 150 caracteres"></textarea>';
    fila += '<span class="text-danger field-validation-error" data-valmsg-for="txaRol' + _id + '" data-valmsg-replace="true"></span> </td>';

    fila += '<td style="text-align:center"><textarea rows="2" class="form-control" id="txaDes' + _id + '" name="txaDes' + _id + '" style="resize:vertical; min-width:150px" placeholder="Máximo 150 caracteres"></textarea>';
    fila += '<span class="text-danger field-validation-error" data-valmsg-for="txaDes' + _id + '" data-valmsg-replace="true"></span> </td>';

    fila += '<td style="text-align:center"><textarea rows="2" class="form-control" id="txaDep' + _id + '" name="txaDep' + _id + '" style="resize:vertical; min-width:150px" placeholder="Máximo 150 caracteres"></textarea>';
    fila += '<span class="text-danger field-validation-error" data-valmsg-for="txaDep' + _id + '" data-valmsg-replace="true"></span> </td>';

    fila += '<td> <button class="remove btn btn-danger" onclick="eliminarIngeniero(' + _id + ',\'' + _nombreIngeniero + '\')">Quitar Ingeniero</button> </td></tr>';

    //Agrega el ingeniero a la tabla htlm
    $('#tbIngenieros > tbody:last').append(fila);

    //Elimina el ingeniero del dropdownlist
    $("#ddlIngenieros option:selected").remove();

    // Actualiza el dropdown
    try {
        $("#ddlIngenieros").parent().find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
    }
    catch (error) {
        $("#ddlIngenieros").parent().find('span.custom-combobox').find('input:text').val('');
    }

    // Se habilita nuevamente el botón
    $(this).toggleClass('disabled', false);
});

$('#btnAgregarLaboratorio').click(function () {
    var dd = document.getElementById('ddlLaboratorios')
    try {
        // Se trata de obtener el valor del dropdown
        var _id = dd.options[dd.selectedIndex].value;
    } catch (error) {
        return false;
    }

    // Se deshabilita el boton mientras se realiza la acción
    $(this).toggleClass('disabled', true);

    // Este ajax se realiza una acción de cobtrolador donde envía el id del laboratorio a buscar y recibe como retorno un JSON con los detalles del laboratorio
    $.ajax({
        url: '/Contrato/LaboratorioDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },

        success: function (data) {
            var json = $.parseJSON(data);

            var fila = '<tr id=' + json.id + '><td>' + json.nombre + '</td> ';
            fila += '<td>' + json.tipo + '</td>';
            fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarLaboratorio(' + json.id + ',\'' + json.nombre + '\')">Quitar Laboratorio</button> </td></tr>';

            //Agrega el ingeniero a la tabla htlm
            $('#tbLaboratorios > tbody:last').append(fila);

            //Elimina el ingeniero del dropdownlist
            $("#ddlLaboratorios option:selected").remove();

            // Actualiza el dropdown
            try {
                $("#ddlLaboratorios").parent().find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
            }
            catch (error) {
                $("#ddlLaboratorios").parent().find('span.custom-combobox').find('input:text').val('');
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status == 400) {
                // Bad request
                alert('Error: Consulta inválida.\nVerifique que seleccionó un laboratorio.');
            }
            else if (xhr.status === 401) {
                // Unauthorized error
                alert('Error: Acceso denegado.\n Verifique que tenga privilegios para realizar la operación.');
            }
            else if (xhr.status == 404) {
                // Not found
                alert('Error: no se encontraron los detalles del ingeniero.\nVerifique que existe el laboratorio.');
            }
            else if (xhr.status == 500) {
                // Server side error
                alert('Error del servidor.\n Espere unos segundos y vuelva a reitentar.');
            }
            else {
                alert('Error: \n' + errorThrown + 'Reitente de nuevo.');
            }
        }
    });

    // Se habilita nuevamente el botón
    $(this).toggleClass('disabled', false);
});

// Devuelve el nombre e id del ingeniero eliminado al dropdown de ingenieros
function eliminarIngeniero(_id, _nombreIngeniero) {
    $("<option value=" + _id + ">" + _nombreIngeniero.toString() + "</option>").appendTo("#ddlIngenieros");
}

// Devuelve el nombre e id del laboratorio eliminado al dropdown de laboratorios
function eliminarLaboratorio(_id, _nombre) {
    $("<option value=" + _id + ">" + _nombre.toString() + "</option>").appendTo("#ddlLaboratorios");
}
