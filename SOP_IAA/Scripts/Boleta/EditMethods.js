// Array JSON que contendrá los id de los contratoItem que se agreguen a la boleta
var items = [];

$(document).ready(

    function () {

        // Funcion del dropdown ruta que según la seleccionada, carga los PE en el dropdown de proyecto estructura
        $("#ddlRuta").change(function () {
            var selectedItem = $(this).val();
            var ddlProyectoEstructura = $("#ddlProyectoEstructura");
            document.getElementById('ddlProyectoEstructura').disabled = false;
            ddlProyectoEstructura.html('');
            ddlProyectoEstructura.append($('<option></option>').val(0).html('- - - Cargando - - -'));

            $.ajax({
                cache: false,
                type: "GET",
                url: "/Boleta/ObtenerProyectosEstructuras/",
                data: { "idRuta": selectedItem },
                success: function (data) {
                    ddlProyectoEstructura.html('');
                    if (data.length < 1) {
                        document.getElementById('ddlProyectoEstructura').disabled = true;
                    }
                    else {
                        $.each(data, function (id, option) {
                            ddlProyectoEstructura.append($('<option></option>').val(option.id).html(option.descripcion));
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Fallo al obtener los proyectos/estructuras.');
                    ddlProyectoEstructura.html('');
                    document.getElementById('ddlProyectoEstructura').disabled = true;
                }
            });
        });

        // Función del DatePicker en los campos de Fecha
        $("#txtFecha").daterangepicker({
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

        // Función que permite agregar una fila con los detalles del item seleccionado en la sección items del Wizard
        $('#btnAgregarItem').click(function () {
            var dd = document.getElementById('ddlItems')

            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Se extrae la fecha seleccionada
            var _fecha = document.getElementById('txtFecha')

            // Este ajax realiza una acción de controlador donde envía el id del ítem a buscar y recibe como retorno un JSON con los detalles del ítem
            $.ajax({
                url: '/Boleta/ItemDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id,
                    fecha: _fecha.value
                },
                success: function (data) {
                    var json = data;

                    var fila = '<tr id=' + _id + '><td>' + json.codigoItem + '</td> ';
                    fila += '<td>' + json.descripcion + '</td>';
                    fila += '<td align="center">' + json.unidadMedida + '</td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" type="text" disabled="" value="₡' + numberFormatCR(removeCurrency(json.precioReajustado)) + '"></td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" onchange="alpha($(this))"></td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" type="text" disabled=""></td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" type="text" disabled=""></td>';
                    fila += '<td align="center"> <button class="remove btn btn-danger" onclick="eliminarItem(' + _id + ', \' ' + json.codigoItem + '\')">Quitar Item</button> </td></tr>';

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbItems > tbody:last').append(fila);

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
                    if (xhr.status == 99) {
                        // error de fecha (error establecido manualmente)
                        alert('Error de fecha.\nVerifique que indico un formato de fecha correcto.');
                    }
                    else if (xhr.status == 400) {
                        // Bad request
                        alert('Error: Consulta inválida.\nVerifique que ingresó una fecha en la boleta.');
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
            })
        }),

        // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $(document).on("click", "#tbItems button.remove", function () {
            $(this).parents("tr").remove();
        });

        $('#txtEstInicial').change(function () {
            recalcularRedimientos();
        });

        $('#txtEstFinal').change(function () {
            recalcularRedimientos();
        });
    }
)

//Antes de ir a la acción Post del submit, se agregan los ingenieros y labs modificados
$("#formEdit").submit(function (eventObj) {

    // Bandera que indicará si debe hacerse el submit o no
    var doSubmit = true;

    // Se listan todos los items de la tabla
    $('#tbItems > tbody > tr').each(function () {

        var singleObj = {}
        singleObj['idItemContrato'] = $(this).attr('id');
        singleObj['precio'] = removeCurrency($(this).children("td").eq(3).find("input:eq(0)").val());
        singleObj['cantidad'] = removeCurrency($(this).children("td").eq(4).find("input:eq(0)").val());
        singleObj['costoTotal'] = removeCurrency($(this).children("td").eq(5).find("input:eq(0)").val());
        singleObj['redimientos'] = removeCurrency($(this).children("td").eq(6).find("input:eq(0)").val());

        // Se verifica que la cantidad sea numérica
        if (!((/[0-9]$/.test(singleObj.cantidad)) && (/[0-9]$/.test(singleObj.costoTotal)))) {
            // Se indica el error
            alert("Algunos campos son incorrectos, verifique las cantidades");
            // Se indica que no debe hacerse el submit
            doSubmit = false;
            // Se detiene el ciclo
            return false;
        }

        // Se verifica que la cantidad sea numérica
        if (!(/[0-9]$/.test(singleObj.redimientos))) {
            // Se informa del error
            alert("Algunos campos son incorrectos, verifique los estacionamientos");
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

    // Se crea el Json con la lista de ítems
    var jsonItems = { "Items": items };

    // Se adjunta al submit el Json de los items de la boleta
    $('<input />').attr('type', 'hidden')
        .attr('name', "jsonItems")
        .attr('value', $.toJSON(jsonItems))
        .appendTo('#formEdit');

    return true;
})

// Función que recorre la tabla de ítems al cargar, para eliminar los items agregados del dropdown.
$('#tbItems > tbody  > tr').each(function () {
    //Se obtiene el id de cada tr que corresponde al id de los ingenieros para eliminar al ingeniero del dropdownlist
    $("#ddlItems option[value='" + $(this).attr('id') + "']").remove();
})

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

// Esta función se activa cuando se modifican los input de estacionamientos y actualiza los cambios en los items de la tabla en la pestaña items
function recalcularRedimientos() {

    // Se obtiene el estacionamiento inicial
    var estInicial = document.getElementById("txtEstInicial").value;

    // Se obtiene el estacionamiento final
    var estFinal = document.getElementById("txtEstFinal").value;

    // Se pasan a decimal los estacionamientos
    try {
        estFinal = new Decimal(estFinal);
        estInicial = new Decimal(estInicial);
    }
    catch (error) {
        $('#tbItems > tbody > tr').each(function () {
            $(this).children("td").eq(6).find("input:eq(0)").val("--- Error ---");
        });
        return false;
    }

    // Se modifican los redimientos de todos los items de la tabla
    $('#tbItems > tbody > tr').each(function () {

        // input de redimientos
        var txtRedimientos = $(this).children("td").eq(6).find("input:eq(0)");

        // Se obtiene la cantidad
        var cantidad = removeCurrency($(this).children("td").eq(4).find("input:eq(0)").val());

        // Se verifica que la cantidad es numérica
        try {
            cantidad = new Decimal(cantidad);
        }
        catch (error) {
            txtRedimientos.val("--- Error ---");
            return false;
        }

        // 
        var _rd = estFinal.minus(estInicial);

        if (_rd == 0) {
            txtRedimientos.val("0");
        }
        else if (_rd < 0) {
            txtRedimientos.val("--- Error ---");
        }
        else {
            txtRedimientos.val(numberFormatCR(cantidad.dividedBy(_rd).toFormat('', 3).toString()));
        }

    });
}

// Función de los input "Cantidad" que cuando se escribe un número se realizan cálculos con otros campos de la fila
function alpha(_this) {

    // Almacena la columna donde se escribió algo
    var td = _this;

    // Se ubican los input de cantidad, precio, redimientos y costo total
    var txtCantidad = td.parent().find("input:eq(0)");
    var txtPrecio = td.parents("tr").children("td").eq(3).find("input:eq(0)");
    var txtCostoTotal = td.parents("tr").children("td").eq(5).find("input:eq(0)");
    var txtRedimientos = td.parents("tr").children("td").eq(6).find("input:eq(0)");

    // Se extraen los valores
    var cantidad = txtCantidad.val();

    // Se ubica el input de costo total
    var costoTotal = td.parents("tr").children("td").eq(5);

    // Se cambia el formato de número
    cantidad = removeCurrency(cantidad);

    // Verifica que la cantidad sea numérica
    if (isNaN(cantidad)) {
        txtCostoTotal.val("--- Error ---");
        return false;
    }
    else {
        // Se pasa el número a formato de numero de CR y se muestra en el textbox
        txtCantidad.val(numberFormatCR(new Decimal(cantidad).toDP(3).toFormat('', 3).toString()));
    }

    // Se obtiene el precio del ítem
    var precio = txtPrecio.val();
    precio = removeCurrency(precio);

    // Se obtiene el estacionamiento inicial
    var estInicial = document.getElementById("txtEstInicial").value;

    // Se obtiene el estacionamiento final
    var estFinal = document.getElementById("txtEstFinal").value;

    // Se verifican los estacionamientos
    try {
        estFinal = new Decimal(estFinal);
        estInicial = new Decimal(estInicial);
    }
    catch (error) {
        txtRedimientos.val("--- Error ---");
        return false;
    }

    // Se trata de hacer las operaciones
    try {

        // Se convierten los valores a decimal
        cantidad = new Decimal(cantidad);
        precio = new Decimal(precio);

        var _ct = new Decimal(precio.times(cantidad)).toDP(4);
        var _rd = estFinal.minus(estInicial);

        txtCostoTotal.val("₡" + numberFormatCR(_ct.toFormat('', 4).toString()));

        if (_rd == 0) {
            txtRedimientos.val("0");
        }
        else if ((_rd.toString() == "NaN") || (_rd < 0)) {
            txtRedimientos.val("--- Error ---");
        }
        else {
            txtRedimientos.val(numberFormatCR(cantidad.dividedBy(_rd).toFormat('', 3).toString()));
        }
    }
    catch (err) {
        alert(err.message);
    }

}

/**
 * Funcion que devuelve un numero con separador de miles
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

// Función que limpia los elementos no numéricos de los precios y establece el formato de CR
function removeCurrency(num) {

    // Se quita el simbolo de moneda
    num = num.replace(/₡/g, "");

    // Se remueve el separador de miles
    num = num.replace(/\./g, "");

    // Se intercambia , por . como separador de deciamles (formato interno)
    num = num.replace(/,/g, ".");

    return num;
}