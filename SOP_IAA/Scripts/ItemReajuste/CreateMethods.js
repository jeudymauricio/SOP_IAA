/*
 *---------------------------------------------- Métodos de la vista 'Create' del ItemReajusteController ---------------------------------------------- 
 */

// Varible contador que permite asignar nombre único a cada textBox agregado
var counter = 1;
var idContrato;

$(document).ready(

    function () {
        
        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        // Función de autocompletar de los dropdown
        $('#ddlItems').combobox();

        // Función del DatePicker en los campos de Fecha sencillos
        $("#txtFechaReajuste").datetimepicker({
            language: 'es',
            autoclose: true,
            format: "MM yyyy",
            startView: 'year',
            minView: 'year',
            startDate: fechaInicio
        });

        //Antes de ir a la acción Post del submit, se agrega la lista de ítems
        $("#formCreate").submit(function (eventObj) {

            $("input[name='create']").toggleClass('disabled', true)
            $("#loadingGif").show();

            // Array JSON que contendrá los id de los reajustes que se agreguen al contrato
            var items = [];

            // Bandera que indicará si debe hacerse el submit o no
            var doSubmit = true;

            // Se listan todos los items de la tabla
            $('#tbItems > tbody > tr').each(function () {

                // Objeto simple que contendrá los detalles de cada ítem de la boleta
                var singleObj = {}

                // Se ubica el input de Reajuste
                var txtReajuste = $(this).children("td").eq(4).find("input:eq(0)");

                // Se obtienen los valores de reajuste
                var reajuste = txtReajuste.val();

                // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos) al reajuste
                reajuste = removeCurrency(reajuste);

                // Se verifica que el reajuste sea numérico
                if (!(txtReajuste.valid())) {
                    // Se indica que no debe hacerse el submit
                    doSubmit = false;
                    // Se detiene el ciclo
                    return false;
                }

                // Se convierte el procentaje a decimal
                reajuste = (new Decimal(reajuste)).dividedBy(100);

                // Se agregan los detalles al objeto
                singleObj['idContratoItem'] = $(this).attr('id');
                singleObj['reajuste'] = reajuste.toString();

                // Se agrega el objeto con los detalles del ítem a la lista a enviar en el submit
                items.push(singleObj);
            })

            // Se verifica si debe hacerse el submit o no
            if (!doSubmit) {
                $("input[name='create']").toggleClass('disabled', false)
                $("#loadingGif").hide();
                return false;
            }

            // Si no hay problemas, Se crea el Json con la lista de ítems y si idContratoItem
            var jsonItems = { "Items": items };

            // Se adjunta al submit el Json de los items a reajustar
            $('<input />').attr('type', 'hidden')
                .attr('name', 'jsonItems')
                .attr('value', $.toJSON(jsonItems))
                .appendTo('#formCreate');

            var strFechaReajuste = document.getElementById('txtFechaReajuste').value.toString();
            console.log(strFechaReajuste);

            // Se adjunta al submit la fecha de los reajustes
            $('<input />').attr('type', 'hidden')
                .attr('name', 'strFechaReajuste')
                .attr('value', strFechaReajuste)
                .appendTo('#formCreate');

            $('<input />').attr('type', 'hidden')
                .attr('name', 'idContrato')
                .attr('value', idContrato)
                .appendTo('#formCreate');

            return true;
        }),

        // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $(document).on("click", "#tbItems button.remove", function () {

            // Se quita la propiedad de dataTable(paginación)
            $('#tbItems').dataTable().fnDestroy();

            $(this).parents("tr").remove();

            // Se agrega la propiedad de dataTable(paginación)
            $('#tbItems').dataTable({
                "language": {
                    "url": "/Scripts/plugins/dataTables/Spanish.txt"
                }
            });
        })
    }); // .\document.ready

function cargarItems(_idContrato) {

    // Se almacena el id del contrato
    idContrato = _idContrato;
    //var _fecha = document.getElementById('txtFechaReajuste').value;

    $('#txtFechaReajuste').attr('disabled', 'disabled');
    $('#btnCargarItems').toggleClass('disabled', true);

    // Este ajax realiza una acción de controlador donde envía el id del ítem a buscar y recibe como retorno un JSON con los detalles del ítem
    $.ajax({
        url: '/ItemReajuste/cargarItems/',
        type: "GET",
        dataType: "json",
        data: {
            idContrato: _idContrato
        },
        success: function (data) {
            var json = $.toJSON(data);
            //var fila;
            $('#formCreate').validate({
                errorPlacement: function(error, element) {
                    error.appendTo( element.parent("td").children("span") );
                }
            });

            //console.info(data);

            $.each($.parseJSON(json), function (idx, obj) {
                
                var fila = '<tr id="' + obj.Item5+ '"><td>' + obj.Item1 + '</td>';
                fila += '<td>' + obj.Item2 + '</td>';
                fila += '<td align="center">' + obj.Item3 + '</td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right" disabled="disabled" value="₡' + numberFormatCR(obj.Item4.toString()) + '"></td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right" value="0,0000" onchange="alpha($(this))" id="txtReajuste' + counter + '" name="txtReajuste' + counter + '" type="text" >'; /*</td>*/
                fila += '<span class="text-danger field-validation-error" data-valmsg-for="txtReajuste' + counter + '" data-valmsg-replace="true"></span> </td>';
                fila += '<td align="center" style="max-width:100px"><input class="form-control" style="text-align:right" disabled="disabled" value="₡' + numberFormatCR(obj.Item4.toString()) + '"></td> </tr>';

                //Agrega el item a la tabla htlm
                $('#tbItems > tbody:last').append(fila);

                //Se agregan las validaciones de números
                $('#tbItems > tbody > tr:last').children("td").eq(4).find('input:eq(0)').rules('add', {
                    number: true, // Validación de números
                    isNumberDecimal: true,
                    range: [-999, 999],
                    required: true, // Validación de campos vacíos
                    messages: {
                        required: "Debe ingresar un reajuste.",
                        number: "Ingrese un reajuste válido.",
                        range: "Ingrese un numero entre -999,9999 y 999,9999",
                        isNumberDecimal: "Ingrese un reajuste válido." // Validación propia declarada en el inicio del document.ready()
                    }
                });

                // Aumenta el Contador
                counter += 1;
            });
            
            // Se agrega la propiedad de dataTable(paginación)
           /* $('#tbItems').dataTable({
                "language": {
                    "url": "/Scripts/plugins/dataTables/Spanish.txt"
                }
            });*/

        },
        error: function (xhr, textStatus, errorThrown) {
            if (xhr.status == 400) {
                // Bad request
                alert('Error: Consulta inválida.\nVerifique que ingresó una fecha de reajuste.');
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
        }
    });
}

// Función que elimina la fila de un ítem de la lista
function eliminarItem(_id, _codigoItem) {
    // Se agrega nuevamente el item al dropdown
    $("<option value=" + _id + ">" + _codigoItem + "</option>").appendTo("#ddlItems");

    // Actualiza el dropdown
    var dd = document.getElementById('ddlItems');
    try {
        $('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
    }
    catch (error) {
        $('span.custom-combobox').find('input:text').val('');
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
        precioActual = new Decimal(precioActual).toDP(4);
        reajuste = new Decimal(reajuste).toDP(4);

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

/*$.extend({
    error: function (msg) { throw msg; },
    parseJSON: function (data) {
        if (typeof data !== "string" || !data) {
            return null;
        }
        data = jQuery.trim(data);
        if (/^[\],:{}\s]*$/.test(data.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, "@")
            .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, "]")
            .replace(/(?:^|:|,)(?:\s*\[)+/g, ""))) {
            return window.JSON && window.JSON.parse ?
                window.JSON.parse(data) :
                (new Function("return " + data))();
        } else {
            jQuery.error("Invalid JSON: " + data);
        }
    }
});*/