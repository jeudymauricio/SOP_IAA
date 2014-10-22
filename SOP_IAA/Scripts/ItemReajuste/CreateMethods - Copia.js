/*
 *---------------------------------------------- Métodos de la vista 'Create' del ItemReajusteController ---------------------------------------------- 
 */

// Varible contador que permite asignar nombre único a cada textBox agregado
var counter = 1;

$(document).ready(

    function () {
        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        // DataTable de los ítems
        $('#tbItems').dataTable({
            "language": {
                "url": "/Scripts/plugins/dataTables/Spanish.txt"
            }
        });

        // Función de autocompletar de los dropdown
        $('#ddlItems').combobox();

        // Función del DatePicker en los campos de Fecha
        $("#txtFechaReajuste").datepicker({
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

        //Antes de ir a la acción Post del submit, se agrega la lista de ítems
        $("#formCreate").submit(function (eventObj) {
            // Array JSON que contendrá los id de los reajustes que se agreguen al contrato
            var items = [];

            // Bandera que indicará si debe hacerse el submit o no
            var doSubmit = true;

            // Se listan todos los items de la tabla
            $('#tbItems > tbody > tr').each(function () {

                // Se verifica que el reajuste sea numérico
                if (!($(this).children("td").eq(4).find("input:eq(0)").valid())) {
                    // Se indica que no debe hacerse el submit
                    doSubmit = false;
                    // Se detiene el ciclo
                    return false;
                }

                // Objeto simple que contendrá los detalles de cada ítem de la boleta
                var singleObj = {}
                singleObj['idItem'] = $(this).attr('id');
                singleObj['reajuste'] = removeCurrency($(this).children("td").eq(4).find("input:eq(0)").val());
                singleObj['precioReajustado'] = removeCurrency($(this).children("td").eq(5).find("input:eq(0)").val());

                // Se verifica que la cantidad sea numérica
                if (!(/[0-9]$/.test(singleObj.precioReajustado))) {
                    // Se indica el error
                    alert("Algunos campos son incorrectos, verifique los reajustes");
                    // Se indica que no debe hacerse el submit
                    doSubmit = false;
                    // Se detiene el ciclo
                    return false;
                }

                // Se agrega el objeto con los detalles del ítem a la lista a enviar en el submit
                items.push(singleObj);
            })

            // Se verifica si debe hacerse el submit o no
            if (!doSubmit) {
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

            return true;
        }),

        // Función que permite agregar una fila con los detalles del item seleccionado en la sección items del Wizard
        $('#btnAgregarItem').click(function () {
            var dd = document.getElementById('ddlItems');
            var _fecha = document.getElementById('txtFechaReajuste').value;

            $('#txtFechaReajuste').attr('disabled', 'disabled');

            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Se deshabilita el boton mientras se realiza la acción
            $(this).toggleClass('disabled', true);

            // Este ajax realiza una acción de controlador donde envía el id del ítem a buscar y recibe como retorno un JSON con los detalles del ítem
            $.ajax({
                url: '/ItemReajuste/ItemDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id,
                    fecha: _fecha
                },
                success: function (data) {
                    var json = data;

                    var fila = '<tr id=' + _id + '><td>' + json.codigoItem + '</td> ';
                    fila += '<td>' + json.descripcion + '</td>';
                    fila += '<td align="center">' + json.unidadMedida + '</td>';
                    fila += '<td align="center"><input class="form-control" style="text-align:right" value="' + json.precioReajustado + '" disabled="disabled"></td>';
                    fila += '<td align="center"><input class="form-control" style="text-align:right"  onchange="alpha($(this))" id="txtReajuste' + counter + '" name="txtReajuste' + counter + '" type="text" >'; /*</td>*/
                    fila += '<span class="text-danger field-validation-error" data-valmsg-for="txtReajuste' + counter + '" data-valmsg-replace="true"></span> </td>';
                    fila += '<td align="center"><input class="form-control" style="text-align:right" disabled="disabled"></td>';
                    fila += '<td align="center" style="max-width:100px"><button class="remove btn btn-danger" onclick="eliminarItem(' + _id + ', \' ' + json.codigoItem + '\')">Quitar Item</button> </td></tr>';

                    // Aumenta el Contador
                    counter += 1;

                    // Se quita la propiedad de dataTable(paginación)
                    $('#tbItems').dataTable().fnDestroy();

                    //Agrega el item a la tabla htlm
                    $('#tbItems > tbody:last').append(fila);
                    
                    // Se agregan las validaciones de números
                    $('#tbItems > tbody > tr:last').children("td").eq(4).find('input:eq(0)').rules('add', {
                        number: true, // Validación de números
                        isNumberDecimal: true,
                        required: true, // Validación de campos vacíos
                        messages: {
                            required: "Debe ingresar un reajuste.",
                            number: "Ingrese un reajuste válido.",
                            isNumberDecimal: "Ingrese un reajuste válido." // Validación propia declarada en el inicio del document.ready()
                        }
                    });

                    // Se agrega la propiedad de dataTable(paginación)
                    $('#tbItems').dataTable({
                        "language": {
                            "url": "/Scripts/plugins/dataTables/Spanish.txt"
                        }
                    });

                    //Elimina el ingeniero del dropdownlist
                    $("#ddlItems option:selected").remove();

                    // Actualiza el dropdown
                    try {
                        $('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
                    }
                    catch (error) {
                        $('span.custom-combobox').find('input:text').val('');
                    }

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

            
            // Se habilita nuevamente el botón
            $(this).toggleClass('disabled', false);
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
        // Se pasa el número a formato de numero de CR
        txtReajuste.val(numberFormatCR(reajuste));
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

        txtPrecioReajustado.val("₡" + numberFormatCR(_precioReajuste.toString()));
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