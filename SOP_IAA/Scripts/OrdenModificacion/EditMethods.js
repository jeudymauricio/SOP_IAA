/*
 *---------------------------------------------- Métodos de la vista 'Edit' del OrdenModificaciónController ---------------------------------------------- 
 */
var counter = 1;

$(document).ready(

    function () {

        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        // Función del DatePicker en los campos de Fecha sencillos
        //$("#txtFecha").datetimepicker({
        //    language: 'es',
        //    autoclose: true,
        //    format: "dd/mm/yyyy",
        //    startView: 'month',
        //    minView: 'month'/*,
        //    startDate: fechaInicio*/
        //});

        //Antes de ir a la acción Post del submit, se agrega la lista de ítems
        $("#formEdit").submit(function (eventObj) {

            $("input[name='edit']").toggleClass('disabled', true)
            $("#loadingGif").show();

            // Array JSON que contendrá los idContrato y las cantidads de la OM
            var items = [];

            // Bandera que indicará si debe hacerse el submit o no
            var doSubmit = true;

            // Se listan todos los items de la tabla
            $('#tbItems > tbody > tr').each(function () {

                // Objeto simple que contendrá los detalles de cada ítem (idContrato y cantidad)
                var singleObj = {}

                // Se ubica el input de cantidad
                var txtCantidad = $(this).children("td").eq(6).find("input:eq(0)");

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

                // Se convierte a decimal
                cantidad = new Decimal(cantidad);

                // Se agregan los detalles al objeto
                singleObj['idContratoItem'] = $(this).attr('id');
                singleObj['idOrdenModificacion'] = $(this).children("td").eq(6).attr('id');
                singleObj['cantidad'] = cantidad.toString();

                // Se agrega el objeto con los detalles del ítem a la lista a enviar en el submit
                items.push(singleObj);
            });

            // Se verifica si debe hacerse el submit o no
            if (!doSubmit) {
                $("input[name='edit']").toggleClass('disabled', false)
                $("#loadingGif").hide();
                return false;
            }

            // Si no hay problemas, Se crea el Json con la lista de ítems y si idContratoItem
            var jsonItems = { "Items": items };

            // Se adjunta al submit el Json de los items a reajustar
            $('<input />').attr('type', 'hidden')
                .attr('name', 'jsonItems')
                .attr('value', $.toJSON(jsonItems))
                .appendTo('#formEdit');

            return true;
        })

        // Se recorre la tabla de items para colocar validadciones y formato en los números
        $('#tbItems > tbody > tr').each(function () {
            // Se ubican los input de Autorizado, Realizado, disponible y Aumento/Disminución(OM)
            var txtOriginal = $(this).children("td").eq(3).find("input:eq(0)");
            var txtResumenOMs = $(this).children("td").eq(4).find("input:eq(0)");
            var txtRealizado = $(this).children("td").eq(5).find("input:eq(0)");
            var txtOM = $(this).children("td").eq(6).find("input:eq(0)");
            var txtAutorizado = $(this).children("td").eq(7).find("input:eq(0)");
            var txtDisponible = $(this).children("td").eq(8).find("input:eq(0)");

            // Se obtienen los valores de Autorizado, Realizado, disponible y Aumento/Disminución(OM)
            var original = txtOriginal.val();
            var resumenOMs = txtResumenOMs.val();
            var realizado = txtRealizado.val();
            var disponible = txtDisponible.val();
            var om = txtOM.val();
            var autorizado = txtAutorizado.val();

            //Se agregan las validaciones de cantidad
            txtOM.rules('add', {
                number: true, // Validación de números
                isNumberDecimal: true,
                //range: [-999, 999],
                required: true, // Validación de campos vacíos
                messages: {
                    required: "Debe ingresar una cantidad.",
                    number: "Ingrese una cantidad válida.",
                    //range: "Ingrese un numero entre -999,999 y 999,999",
                    isNumberDecimal: "Ingrese una cantidad válida." // Validación propia declarada en el inicio del document.ready()
                }
            });

            //console.info(original, resumenOMs, realizado, disponible, om, autorizado);
            // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos)
            original = removeCurrency(original);
            resumenOMs = removeCurrency(resumenOMs);
            realizado = removeCurrency(realizado);
            disponible = removeCurrency(disponible);
            om = removeCurrency(om);
            autorizado = removeCurrency(autorizado);

            if (new Decimal(disponible).toDP(3).lessThan(0)) {
                $(this).children("td").eq(8).addClass('danger');
            }
            else {
                $(this).children("td").eq(8).addClass('success');
            }

            txtOriginal.val(numberFormatCR(new Decimal(original).toDP(3).toFormat('', 3).toString()));
            txtResumenOMs.val(numberFormatCR(new Decimal(resumenOMs).toDP(3).toFormat('', 3).toString()));
            txtRealizado.val(numberFormatCR(new Decimal(realizado).toDP(3).toFormat('', 3).toString()));
            txtOM.val(numberFormatCR(new Decimal(om).toDP(3).toFormat('', 3).toString()));
            txtAutorizado.val(numberFormatCR(autorizado));
            txtDisponible.val(numberFormatCR(new Decimal(disponible).toDP(3).toFormat('', 3).toString()));
        });

    }); // .\document.ready

function clearTable() {
    $("#tbItems tbody tr").remove();
}

function cargarItems(_idContrato) {

    // Se almacena el id del contrato
    idContrato = _idContrato;
    var _fecha = document.getElementById('txtFecha').value;

    // Este ajax realiza una acción de controlador donde envía el id del ítem a buscar y recibe como retorno un JSON con los detalles del ítem
    $.ajax({
        url: '/OrdenModificacion/CargarCantidades/',
        type: "GET",
        dataType: "json",
        data: {
            idContrato: _idContrato,
            fecha: _fecha
        },
        success: function (data) {
            var json = $.toJSON(data);
            
            $('#formCreate').validate({
                errorPlacement: function(error, element) {
                    error.appendTo( element.parent("td").children("span") );
                }
            });

            $.each($.parseJSON(json), function (idx, obj) {
                // <idContratoItem, CodigoItem, Descripcion, unidad, Original, OMs, Realizado>
                // <   Item1,         Item2,       Item3,     Item4,  Item5,  Item6,  Item7  >

                var fila = '<tr id="' + obj.Item1+ '"><td>' + obj.Item2 + '</td>';
                fila += '<td>' + obj.Item3 + '</td>';
                fila += '<td align="center">' + obj.Item4 + '</td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right;min-width:100px" disabled="disabled" value="' + numberFormatCR(new Decimal(obj.Item5).toDP(3).toFormat('', 3).toString()) + '"></td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right;min-width:100px" disabled="disabled" value="' + numberFormatCR(new Decimal(obj.Item6).toDP(3).toFormat('', 3).toString()) + '"></td>';
                fila += '<td align="center"><input class="form-control" style="text-align:right;min-width:100px" disabled="disabled" value="' + numberFormatCR(new Decimal(obj.Item7).toDP(3).toFormat('', 3).toString()) + '"></td>';


                fila += '<td align="center"><input class="form-control" style="text-align:right;min-width:100px;" placeholder="+/-0,000" value="0,000" onchange="alpha($(this))" id="txtCantidad' + counter + '" name="txtCantidad' + counter + '">';
                fila += '<span class="text-danger field-validation-error" data-valmsg-for="txtCantidad' + counter + '" data-valmsg-replace="true"></span></td>';

                var autorizado = new Decimal(obj.Item5).toDP(3).plus(new Decimal(obj.Item6).toDP(3)).toDP(3);
                fila += '<td align="center"><input class="form-control" style="text-align:right;min-width:100px" disabled="disabled" value="' + numberFormatCR(autorizado.toFormat('', 3).toString()) + '"></td>';

                var disponible = new Decimal(obj.Item5).toDP(3).minus(new Decimal(obj.Item7).toDP(3)).toDP(3);
                fila += '<td align="center"><input class="form-control" style="text-align:right;min-width:100px;background-color: initial;" disabled="disabled" value="' + numberFormatCR(disponible.toFormat('', 3).toString()) + '"></td>';

                fila += '</tr>';

                //Agrega el item a la tabla htlm
                $('#tbItems > tbody:last').append(fila);

                if (disponible.lessThanOrEqualTo(0)) {
                    $('#tbItems > tbody > tr:last').children("td").eq(8).addClass('danger');
                }
                else {
                    $('#tbItems > tbody > tr:last').children("td").eq(8).addClass('success');
                }

                //Se agregan las validaciones de cantidad
                $('#txtCantidad' + counter).rules('add', {
                    number: true, // Validación de números
                    isNumberDecimal: true,
                    //range: [-999, 999],
                    required: true, // Validación de campos vacíos
                    messages: {
                        required: "Debe ingresar una cantidad.",
                        number: "Ingrese una cantidad válida.",
                        //range: "Ingrese un numero entre -999,999 y 999,999",
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

// Función de los input "Cantidad" que cuando se escribe un número se realizan cálculos con otros campos de la fila
function alpha(_this) {

    // Almacena la columna donde se escribió algo
    var td = _this;

    // Se ubican los input de Autorizado, Realizado, disponible y Aumento/Disminución(OM)
    var txtOriginal = td.parents("tr").children("td").eq(3).find("input:eq(0)");
    var txtResumenOMs = td.parents("tr").children("td").eq(4).find("input:eq(0)");
    var txtRealizado = td.parents("tr").children("td").eq(5).find("input:eq(0)");

    var txtOM = td.parent().find("input:eq(0)");
    var txtAutorizado = td.parents("tr").children("td").eq(7).find("input:eq(0)");
    var txtDisponible = td.parents("tr").children("td").eq(8).find("input:eq(0)");

    // Se obtienen los valores de Autorizado, Realizado, disponible y Aumento/Disminución(OM)
    var original = txtOriginal.val();
    var resumenOMs = txtResumenOMs.val();
    var realizado = txtRealizado.val();
    var disponible = txtDisponible.val();
    var om = txtOM.val();
    var autorizado = txtAutorizado.val();

    // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos)
    original = removeCurrency(original);
    resumenOMs = removeCurrency(resumenOMs);
    realizado = removeCurrency(realizado);
    disponible = removeCurrency(disponible);
    om = removeCurrency(om);
    autorizado = removeCurrency(autorizado);

    // Se verifica que el cantidad sea numérica
    if (!(txtOM.valid())) {
        return false;
    }

    if (isNaN(om)) {
        alert('La cantidad debe ser númerica y se usa la , como separador de decimales');
        return false;
    }
    else {
        // Se pasa el número a formato de numero de CR y se muestra en el textbox
        txtOM.val(numberFormatCR(new Decimal(om).toDP(3).toFormat('', 3).toString()));
    }
    
    // Se trata de hacer las operaciones
    try {

        // Se convierten los valores a decimal
        original = new Decimal(original).toDP(3);
        resumenOMs = new Decimal(resumenOMs).toDP(3);
        realizado = new Decimal(realizado).toDP(3);
        om = new Decimal(om).toDP(3);
        autorizado = new Decimal(autorizado).toDP(3);
        disponible = new Decimal(disponible).toDP(3);
        
        // Se obtiene el nuevo autorizado
        autorizado =  original.plus(resumenOMs).plus(om);
        disponible = autorizado.minus(realizado);

        if (disponible.lessThan(0)) {
            td.parents("tr").children("td").eq(8).attr('class', '');
            td.parents("tr").children("td").eq(8).addClass('danger');
        }
        else {
            td.parents("tr").children("td").eq(8).attr('class', '');
            td.parents("tr").children("td").eq(8).addClass('success');
        }

        txtAutorizado.val(numberFormatCR(autorizado.toFormat('', 3).toString()));
        txtDisponible.val(numberFormatCR(disponible.toFormat('', 3).toString()));
    }
    catch (err) {
        if (err instanceof Error && err.name == 'Decimal Error') {
            txtAutorizado.val("--- Error ---");
        }
        else {
            alert(err.message);
            txtAutorizado.val("--- Error ---");
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
 * Funcion que devuelve un numero separando los miles
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
