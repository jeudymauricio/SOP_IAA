/*
 *---------------------------------------------- Métodos de la vista 'Create' del OrdenModificaciónController ---------------------------------------------- 
 */
var counter = 1;

$(document).ready(

    function () {
        $("#ddlItems").combobox();

        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        // Función del DatePicker en los campos de Fecha sencillos
        $("#txtFecha").datetimepicker({
            language: 'es',
            autoclose: true,
            format: "dd/mm/yyyy",
            startView: 'month',
            minView: 'month'/*,
            startDate: fechaInicio*/
        });
        //$("#txtFechaCargar").datetimepicker({
        //    language: 'es',
        //    autoclose: true,
        //    format: "MM yyyy",
        //    startView: 'year',
        //    minView: 'year'/*,
        //    startDate: fechaInicio*/
        //});

        // Función del Wizard
        $('#rootwizard').bootstrapWizard({
            onTabShow: function (tab, navigation, index) {

                clearTable();

                // Dynamically change percentage completion on progress bar
                var tabCount = navigation.find('li').length;
                var current = index + 1;
                var percentDone = (current / tabCount) * 100;
                $('#rootwizard').find('#progressBar').css({ width: percentDone + '%' });

                // Optional: Show Done button when on last tab;
                // It is invisible by default.
                $('#rootwizard').find('.last').toggle(current >= tabCount);

                // Optional: Hide Next button if on last tab;
                // otherwise it shows but is disabled
                $('#rootwizard').find('.next').toggle(current < tabCount);
            },
            onTabClick: function (tab, navigation, index) {
                //alert('Utilice los botones de Siguiente, Anterior para desplazarse');
                //return false;
            }
        });

        // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $(document).on("click", "#tbItems button.remove", function () {
            $(this).parents("tr").remove();
        });

        //Antes de ir a la acción Post del submit, se agrega la lista de ítems
        $("#formCreate").submit(function (eventObj) {

            $("input[name='create']").toggleClass('disabled', true)
            $("#loadingGif").show();

            // Array JSON que contendrá los id de las cantidads que se agreguen al contrato
            var items = [];

            // Bandera que indicará si debe hacerse el submit o no
            var doSubmit = true;

            // Se listan todos los items de la tabla
            $('#tbItems > tbody > tr').each(function () {

                // Objeto simple que contendrá los detalles de cada ítem de la boleta
                var singleObj = {}

                // Se ubica el input de cantidad
                var txtCantidad = $(this).children("td").eq(4).find("input:eq(0)");

                // Se obtienen los valores de cantidad
                var cantidad = txtCantidad.val();

                // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos) al cantidad
                cantidad = removeCurrency(cantidad);

                // Se verifica que el cantidad sea numérico
                if (!(txtCantidad.valid())) {
                    // Se indica que no debe hacerse el submit
                    doSubmit = false;
                    // Se detiene el ciclo
                    return false;
                }

                // Se convierte el procentaje a decimal
                cantidad = (new Decimal(cantidad)).dividedBy(100);

                // Se agregan los detalles al objeto
                singleObj['idContratoItem'] = $(this).attr('id');
                singleObj['cantidad'] = cantidad.toString();

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

            var strFechaCantidad = document.getElementById('txtFechaCantidad').value.toString();
            console.log(strFechaCantidad);

            // Se adjunta al submit la fecha de los cantidads
            $('<input />').attr('type', 'hidden')
                .attr('name', 'strFechaCantidad')
                .attr('value', strFechaCantidad)
                .appendTo('#formCreate');

            $('<input />').attr('type', 'hidden')
                .attr('name', 'idContrato')
                .attr('value', idContrato)
                .appendTo('#formCreate');

            return true;
        })

    }); // .\document.ready

function clearTable() {
    $("#tbItems tbody tr").remove();
}

function cargarItems(_idContrato) {

    // Se almacena el id del contrato
    idContrato = _idContrato;
    var _fecha = document.getElementById('txtFecha').value;

    $('#txtFechaCargar').attr('disabled', 'disabled');
    $('#btnCargarItems').toggleClass('disabled', true);

    // Este ajax realiza una acción de controlador donde envía el id del ítem a buscar y recibe como retorno un JSON con los detalles del ítem
    $.ajax({
        url: '/OrdenModificacion/CargarPeriodo/',
        type: "GET",
        dataType: "json",
        data: {
            idContrato: _idContrato,
            fecha: _fecha
        },
        success: function (data) {
            var json = $.toJSON(data);
            //var fila;
            $('#formCreate').validate({
                errorPlacement: function(error, element) {
                    error.appendTo( element.parent("td").children("span") );
                }
            });

            $.each($.parseJSON(json), function (idx, obj) {

                var fila = '<tr id="' + obj.Item1+ '"><td>' + obj.Item2 + '</td>';
                fila += '<td>' + obj.Item3 + '</td>';
                fila += '<td align="center">' + obj.Item4 + '</td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right" disabled="disabled" value="' + numberFormatCR(new Decimal(obj.Item5).toDP(3).toFormat('', 3).toString()) + '"></td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right" disabled="disabled" value="' + numberFormatCR(new Decimal(obj.Item6).toDP(3).toFormat('', 3).toString()) + '"></td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right" disabled="disabled" value="' + numberFormatCR(new Decimal(obj.Item7).toDP(3).toFormat('', 3).toString()) + '"></td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right" value="0,000" onchange="alpha($(this))" id="txtCantidad' + counter + '" name="txtCantidad' + counter + '" type="text" >';
                fila += '<span class="text-danger field-validation-error" data-valmsg-for="txtCantidad' + counter + '" data-valmsg-replace="true"></span>';
                fila += '</td> </tr>';

                //Agrega el item a la tabla htlm
                $('#tbItems > tbody:last').append(fila);

                if (obj.Item7 >= 0) {
                    $('#tbItems > tbody > tr:last').addClass('success');
                }
                else {
                    $('#tbItems > tbody > tr:last').addClass('danger');
                }

                //Se agregan las validaciones de números
                $('#tbItems > tbody > tr:last').children("td").eq(4).find('input:eq(0)').rules('add', {
                    number: true, // Validación de números
                    isNumberDecimal: true,
                    range: [0, 999],
                    required: true, // Validación de campos vacíos
                    messages: {
                        required: "Debe ingresar una cantidad.",
                        number: "Ingrese una cantidad válida.",
                        range: "Ingrese un numero menor o igual a 999,999",
                        isNumberDecimal: "Ingrese una cantidad válida." // Validación propia declarada en el inicio del document.ready()
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
                alert('Error: Consulta inválida.\n');
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

    // Se ubican los input de precio actual, cantidad y Precio reajustado
    var txtPrecioActual = td.parents("tr").children("td").eq(3).find("input:eq(0)");
    var txtCantidad = td.parent().find("input:eq(0)");
    var txtPrecioReajustado = td.parents("tr").children("td").eq(5).find("input:eq(0)");

    // Se obtienen los valores de precio actual y cantidad
    var precioActual = txtPrecioActual.val();
    var cantidad = txtCantidad.val();

    // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos) al precio actual y cantidad
    precioActual = removeCurrency(precioActual)
    cantidad = removeCurrency(cantidad);

    // Se verifica que el cantidad sea numérico
    if (!(txtCantidad.valid())) {
        return false;
    }

    if(isNaN(cantidad)){
        //alert('El cantidad debe ser númerico y se usa la , como separador de decimales');
        txtPrecioReajustado.val("--- Error ---");
        return false;
    }
    else {
        // Se pasa el número a formato de numero de CR y se muestra en el textbox
        txtCantidad.val(numberFormatCR(new Decimal(cantidad).toDP(3).toFormat('',3).toString()));
    }
    
    // Se trata de hacer las operaciones
    try {

        // Se convierten los valores a decimal
        precioActual = new Decimal(precioActual).toDP(4);
        cantidad = new Decimal(cantidad).toDP(3);
        
        // Se obtiene el monto
        var monto =  new Decimal(precioActual.times(cantidad)).toDP(4);

        txtPrecioReajustado.val("₡" + numberFormatCR(monto.toFormat('', 4).toString()));
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
