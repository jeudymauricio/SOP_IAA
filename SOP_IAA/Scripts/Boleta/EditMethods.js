// Array JSON que contendrá los id de los contratoItem que se agreguen a la boleta
var items = [];

$(document).ready(

    function () {
        // Funcion del dropdown ruta que según la seleccionada, carga los PE en el dropdown de proyecto estructura
        $("#ddlRuta").change(function () {
            var selectedItem = $(this).val();
            var ddlProyectoEstructura = $("#ddlProyectoEstructura");
            //var statesProgress = $("#states-loading-progress");
            //statesProgress.show();
            $.ajax({
                cache: false,
                type: "GET",
                url: "/Boleta/ObtenerProyectosEstructuras/",
                data: { "idRuta": selectedItem },
                success: function (data) {
                    ddlProyectoEstructura.html('');
                    $.each(data, function (id, option) {
                        ddlProyectoEstructura.append($('<option></option>').val(option.id).html(option.descripcion));
                    });
                    //statesProgress.hide();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Fallo al obtener los proyectos/estructuras.');
                    statesProgress.hide();
                }
            });
        });

        // Función del DatePicker en los campos de Fecha
        $("#txtFecha").datepicker({ dateFormat: 'dd/mm/yy' }),
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

        // Función que permite agregar una fila con los detalles del item seleccionado en la sección items del Wizard
        $('#btnAgregarItem').click(function () {
            var dd = document.getElementById('ddlItems')
            var _id = dd.options[dd.selectedIndex].value;
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
                    fila += '<td align="right"><input class="form-control" style="text-align:right" type="text" disabled="" value="' + json.precioReajustado + '"></td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" onchange="alpha($(this))"></td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" type="text" disabled=""></td>';
                    fila += '<td align="right"><input class="form-control" style="text-align:right" type="text" disabled=""></td>';
                    fila += '<td align="center"> <button class="remove btn btn-danger" onclick="eliminarItem(' + _id + ', \' ' + json.codigoItem + '\')">Quitar Item</button> </td></tr>';

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbItems > tbody:last').append(fila);

                    //Elimina el ingeniero del dropdownlist
                    $("#ddlItems option:selected").remove();

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
        })
    }
)

//Antes de ir a la acción Post del submit, se agregan los ingenieros y labs modificados
$("#formEdit").submit(function (eventObj) {


    // Se listan todos los items de la tabla
    $('#tbItems > tbody > tr').each(function () {

        var singleObj = {}
        singleObj['idItemContrato'] = $(this).attr('id');
        singleObj['precio'] = removeCurrency($(this).children("td").eq(3).find("input:eq(0)").val());
        singleObj['cantidad'] = removeCurrency($(this).children("td").eq(4).find("input:eq(0)").val());
        singleObj['costoTotal'] = removeCurrency($(this).children("td").eq(5).find("input:eq(0)").val());
        singleObj['redimientos'] = removeCurrency($(this).children("td").eq(6).find("input:eq(0)").val());
        items.push(singleObj);

    })

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
}

// Función de los input "Cantidad" que cuando se escribe un número se realizan cálculos con otros campos de la fila
function alpha(_this) {

    // Almacena la columna donde se escribió algo
    var td = _this;

    // Se ubica el input de costo total
    var costoTotal = td.parents("tr").children("td").eq(5);

    // Se ubica el imput de redimientos
    var redimientos = td.parents("tr").children("td").eq(6);

    // Se obtiene la Cantidad del input
    var cantidad = td.parent().find("input:eq(0)").val();
    // Se cambia el formato de número
    cantidad = removeCurrency(cantidad);

    // Se verifica que la cantidad sea numérica
    if (!(/[0-9]$/.test(cantidad))) {
        costoTotal.find("input:eq(0)").val(" --- Error --- ");
        return false;
    }

    // Se obtiene el precio del ítem
    var precio = td.parents("tr").children("td").eq(3).find("input:eq(0)").val();
    // Se cambia el formato de número
    precio = removeCurrency(precio);

    // Se obtiene el estacionamiento inicial
    var estInicial = document.getElementById("txtEstInicial").value;

    // Se obtiene el estacionamiento final
    var estFinal = document.getElementById("txtEstFinal").value;

    // Se trata de hacer las operaciones
    try {
        _ct = parseFloat(cantidad) * parseFloat(precio);
        _rd = parseFloat(estFinal) - parseFloat(estInicial);

        costoTotal.find("input:eq(0)").val("₡" + formatNumber(_ct));

        if (_rd == 0) {
            redimientos.find("input:eq(0)").val("0");
        }
        else if ((_rd.toString() == "NaN") || (_rd < 0)) {
            redimientos.find("input:eq(0)").val("--- Error ---");
        }
        else {
            redimientos.find("input:eq(0)").val(formatNumber((parseFloat(cantidad) / _rd)));
        }
    }
    catch (err) {
        alert(err.message);
    }

}

// Funcion que le da formato a un numero ejemplo: console.info(formatNumber(1240.5));    // 1,240.5
function formatNumber(num) {
    return num
       .toFixed(3) // 3 decimales
       .replace(".", ",") // Se reemplaza el . por la , como separador de decimales
       .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1.") // Se usa el . como separador de miles
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