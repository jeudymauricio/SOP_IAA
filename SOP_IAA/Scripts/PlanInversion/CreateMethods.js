/*
 *---------------------------------------------- Métodos de la vista 'Create' del PlanInversionController ---------------------------------------------- 
 */

// Varible contador que permite asignar nombre único a cada elemento agregado
var panelCounter = 1;
var rowCounter = 1;

// Variable contador que indica el número de panels agregados
var panels = 0;

$(document).ready(

    function () {
        panelCounter = 1;
        rowCounter = 1;
        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        // Función de autocompletar de los dropdown
        //$('#ddlItems').combobox();

        // Función del DatePicker en los campos de Fecha
        $("#txtFecha").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'MM yy',
            onClose: function (dateText, inst) {
                var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(year, month, 1));
            }
        });
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
        $.datepicker.setDefaults($.datepicker.regional['es']);

        $('#txtFecha').rules('remove');
        
        // Función que permite agregar una ruta al panel principal
        $('#btnAgregarRuta').click(function () {
            var dd = document.getElementById('ddlRutas');
            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Se deshabilita el boton mientras se realiza la acción
            $(this).toggleClass('disabled', true);

            // Creación del panel group
            var panel = '<div class="panel panel-default" id="panel' + _id + '">';
            panel += '<div class="panel-heading" min-height: 37px;>';
            panel += '<h4 class="panel-title">';

            // Este id identificará al div que contendrá tanto el dropdown como la tabla con les detalles de item por ruta
            panel += '<a data-toggle="collapse" data-parent="#accordion" href="#collapse' + panelCounter + '" class="collapsed" aria-expanded="false"> Ruta ' + dd.options[dd.selectedIndex].text + '</a>';
            panel += '<button type="button" class="btn btn-warning btn-xs pull-right" onclick="eliminarRuta(' + _id + ',' + dd.options[dd.selectedIndex].text + ')"> Excluir Ruta </button>';
            panel += '</h4></div>';
            panel += '<div id="collapse' + panelCounter + '" class="panel-collapse collapse" aria-expanded="false">';
            panel += '<div class="panel-body">';

            //Cuerpo del panel -- Donde se cargarán los ítems
            panel += '<div class="row">';
            panel += '<div class="col-sm-12">';

            // El dropdown donde está la lista de items, posee el mismo número de id que el panel global donde está
            panel += '<select id = "ddlItems' + panelCounter + '">';
            
            // Se carga el dropdown con los items (almacenados en la variable global del create.cshtml)
            for (var x = 0; x < itemList.length; x++) {
                panel += '<option value="' + itemList[x].Value + '">' + itemList[x].Text + '</option>';
            }
            panel += '</select>';

            panel += '<input class="btn btn-primary" type="button" id="btnAgregarItem' + panelCounter + '" value="Incluir Ítem" onclick="agregarItem(' + panelCounter + ')"/>';
            panel += '<img src="/Content/Images/loading.gif" id="loadingGif' + panelCounter + '" style="display: none;" />';
            panel += '</div>';
            panel += '</div>';
            panel += '<div class="table-responsive">';

            // La tabla dinámica donde se almacenarán los items de cada ruta, posee el mismo número de id que el panel global donde está
            panel += '<table id="tbItems' + panelCounter + '" class="table">';
            panel += '<thead>';
            panel += '<tr>';
            panel += '<th>Item</th>';
            panel += '<th style="text-align:center">Descripción</th> <th style="text-align:center">Unidad de Medida</th> <th style="text-align:center">Precio(₡)</th> <th style="text-align:center">Cantidad</th>';
            panel += '<th style="text-align:center">Monto(₡)</th> <th style="text-align:center"> </th>';
            panel += '<th></th></tr>';
            panel += '</thead>';
            panel += '<tbody>';
                
            panel += '</tbody> </table> </div>';

            panel += '</div></div></div>';
            
            // Se agrega el panel recien creado al panel de tipo acordeón
            $('#rutasAccordion').append(panel);

            // Se procede a marcar como autocomplete el dropdown items del panel
            var ddl = 'ddlItems' + panelCounter;
            var dd2 = document.getElementById(ddl);
            $(dd2).combobox();

            //Elimina la ruta del dropdownlist global
            $("#ddlRutas option:selected").remove();

            // Se indica la agregación de un panel
            //panels++;

            // Se aumenta el contador
            panelCounter += 1;

            // Se habilita nuevamente el botón
            $(this).toggleClass('disabled', false);
        });

    }); // .\document.ready

// Función que elimina un panel del acordeón
function eliminarRuta(_id, _ruta) {
    // Se agrega nuevamente la ruta al dropdown
    $("<option value=" + _id + ">" + _ruta + "</option>").appendTo("#ddlRutas");

    // Puesto que el panel comparte el id con la ruta, se elimina de la siguiente forma
    document.getElementById("panel"+_id).remove();
}

// Función que elimina la fila de un ítem de la lista
function eliminarItem(_panelCounter, _id, _codigoItem) {

    debugger;
    // Se agrega nuevamente el item al dropdown
    $("<option value=" + _id + ">" + _codigoItem + "</option>").appendTo('#ddlItems' + _panelCounter);

    // Actualiza el dropdown
    var dd = document.getElementById('ddlItems' + _panelCounter);
    try {
        $('#collapse' + _panelCounter + '').find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
    }
    catch (error) {
        $('#collapse' + _panelCounter + '').find('span.custom-combobox').find('input:text').val('');
    }

}


function agregarItem(_panelCounter) {

    // Se deshabilita el boton mientras se realiza la acción
    var btn = '#btnAgregarItem' + _panelCounter;
    $(btn).toggleClass('disabled', true);

    var dd = document.getElementById('ddlItems' + _panelCounter);
    $("#loadingGif" + _panelCounter + "").show();
    try {
        // Se trata de obtener el valor del dropdown
        var _id = dd.options[dd.selectedIndex].value;

    } catch (error) {
        $("#loadingGif" + _panelCounter + "").hide();
        // Se habilita nuevamente el botón
        $(btn).toggleClass('disabled', false);
        return false;
    }

    // Este ajax realiza una acción de controlador donde envía el id del ítem a buscar y recibe como retorno un JSON con los detalles del ítem
    $.ajax({
        url: '/PlanInversion/ItemDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = data;
            //debugger;

            var fila = '<tr id=' + _id + '><td>' + json.codigoItem + '</td> ';
            fila += '<td>' + json.descripcion + '</td>';
            fila += '<td align="center">' + json.unidadMedida + '</td>';
            // Columna de Precio
            fila += '<td align="center"><input class="form-control" style="min-width: 100px; text-align:right" disabled="disabled" value="₡' + numberFormatCR(json.precio) + '"></td>';
            // Columna de cantidad
            fila += '<td align="center"><input class="form-control" style="min-width: 100px; text-align:right"  onchange="alpha($(this))" id="txtCantidad' + rowCounter + '" name="txtCantidad' + rowCounter + '" type="text" value="0,000">'; /*</td>*/
            fila += '<span class="text-danger field-validation-error" data-valmsg-for="txtCantidad' + rowCounter + '" data-valmsg-replace="true"></span> </td>';
            // Columna de monto total
            fila += '<td align="center"><input class="form-control" style="min-width: 100px; text-align:right" disabled="disabled" value="₡0,0000"></td>';
            // Botón de excluir
            fila += '<td align="center"><button type="button" class="btn btn-warning" onclick="eliminarItem(' + _panelCounter + ',' + _id + ',\'' + json.codigoItem + '\')">Excluir Item</button> </td></tr>';



            //Agrega el ingeniero a la tabla htlm
            $('#tbItems' + _panelCounter + ' > tbody:last').append(fila);

            // Se agregan las validaciones de precio
            /*$('#tbItems > tbody > tr:last').find('input:eq(0)').rules('add', {
                number: true, // Validación de números
                required: true, // Validación de campos vacíos
                messages: {
                    required: "Debe ingresar un precio.",
                    number: "Ingrese un precio válido."
                }
            });*/
            
            // Aumenta el Contador
            rowCounter += 1;

            //Elimina el item del dropdownlist
            $('#ddlItems' + _panelCounter + ' option:selected').remove();

            // Actualiza el dropdown
            try {
                $('#collapse'+_panelCounter+'').find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
            }
            catch (error) {
                $('#collapse' + _panelCounter + '').find('span.custom-combobox').find('input:text').val('');
            }

            $("#loadingGif" + _panelCounter + "").hide();
            // Se habilita nuevamente el botón
            $(btn).toggleClass('disabled', false);
        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status == 400) {
                // Bad request
                alert('Error: Consulta inválida.\nVerifique que digitó datos correctos.');
            }
            else if (xhr.status === 401) {
                // Unauthorized error
                alert('Error: Acceso denegado.\n Verifique que tenga privilegios para realizar la operación.');
            }
            else if (xhr.status == 404) {
                // Not found
                alert('Error: no se encontraron los detalles del ítem.\nVerifique que el item pertenece al contrato.');
            }
            else if (xhr.status == 500) {
                // Server side error
                alert('Error del servidor.\n Espere unos segundos y vuelva a reitentar.');
            }
            else {
                alert('Error: \n ' + errorThrown + 'Reitente de nuevo.');
            }

            $("#loadingGif" + _panelCounter + "").hide();
            // Se habilita nuevamente el botón
            $(btn).toggleClass('disabled', false);
        }
    });
    
}

// Función de los input "Cantidad" que cuando se escribe un número se realizan cálculos con otros campos de la fila
function alpha(_this) {

    // Almacena la columna donde se escribió algo
    var td = _this;

    // Se ubican los input de precio actual, Reajuste y Precio reajustado
    var txtPrecioActual = td.parents("tr").children("td").eq(3).find("input:eq(0)");
    var txtReajuste = td.parent().find("input:eq(0)");
    var txtPrecioReajustado = td.parents("tr").children("td").eq(5).find("input:eq(0)");

    // Se obtienen los valores de precio actual y reajuste
    var precioActual = txtPrecioActual.val();
    var reajuste = txtReajuste.val();

    // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos) al precio actual y reajuste
    precioActual = removeCurrency(precioActual)
    reajuste = removeCurrency(reajuste);

    // Se verifica que el reajuste sea numérico
    if (!(txtReajuste.valid())) {
        return false;
    }

    if(isNaN(reajuste)){
        //alert('El reajuste debe ser númerico y se usa la , como separador de decimales');
        txtPrecioReajustado.val("--- Error ---");
        return false;
    }
    else {
        // Se pasa el número a formato de numero de CR y se muestra en el textbox
        txtReajuste.val(numberFormatCR(new Decimal(reajuste).toDP(4).toFormat('',4).toString()));
    }
    
    // Se trata de hacer las operaciones
    try {

        // Se convierten los valores a decimal
        precioActual = new Decimal(precioActual);
        reajuste = new Decimal(reajuste);

        // Se pasa a valor decimal el porcentaje de reajuste
        var _reajusteDecimal = new Decimal(reajuste.dividedBy(100));

        // Se suma el reajuste al precio actual
        var _precioReajuste =  new Decimal((_reajusteDecimal.times(precioActual)).plus(precioActual)).toDP(4);

        txtPrecioReajustado.val("₡" + numberFormatCR(_precioReajuste.toFormat('', 4).toString()));
    }
    catch (err) {
        if (err instanceof Error && err.name == 'Decimal Error') {
            txtPrecioReajustado.val("--- Error ---");
        }
        else {
            alert(err.message);
            txtPrecioReajustado.val("--- Error ---");
        }
    }

}

// Función que limpia los elementos no numéricos de los precios y establece el formato de CR
function removeCurrency(num) {

    // Se quita el simbolo de moneda
    num = num.replace(/₡/g, "");

    // Se remueve el separador de miles
    num = num.replace(/\./g, "");

    // Se intercambia , por . como separador de decimales (formato interno)
    num = num.replace(/,/g, ".");

    return num;
}

/**
 * Funcion que devuelve un numero separando los separadores de miles
 * Puede recibir valores negativos y con decimales
 */
function numberFormatCR(numero) {
    // Se coloca la , en lugar del . como separador decimal
    numero = numero.replace(/\./g, ',');

    // Variable que contendra el resultado final
    var resultado = "";

    // Si el numero empieza por el valor "-" (numero negativo)
    if (numero[0] == "-") {
        // Cogemos el numero eliminando los posibles puntos que tenga, y sin
        // el signo negativo
        nuevoNumero = numero.replace(/\./g, '').substring(1);
    } else {
        // Cogemos el numero eliminando los posibles puntos que tenga
        nuevoNumero = numero.replace(/\./g, '');
    }

    // Si tiene decimales, se los quitamos al numero
    if (numero.indexOf(",") >= 0)
        nuevoNumero = nuevoNumero.substring(0, nuevoNumero.indexOf(","));

    // Ponemos un punto cada 3 caracteres
    for (var j, i = nuevoNumero.length - 1, j = 0; i >= 0; i--, j++)
        resultado = nuevoNumero.charAt(i) + ((j > 0) && (j % 3 == 0) ? "." : "") + resultado;

    // Si tiene decimales, se lo añadimos al numero una vez formateado con 
    // los separadores de miles
    if (numero.indexOf(",") >= 0)
        resultado += numero.substring(numero.indexOf(","));

    if (numero[0] == "-") {
        // Devolvemos el valor añadiendo al inicio el signo negativo
        return "-" + resultado;
    } else {
        return resultado;
    }
}