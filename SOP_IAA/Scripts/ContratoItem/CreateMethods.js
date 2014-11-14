/*
 *---------------------------------------------- Métodos de la vista 'Create' del ContratoItemController ---------------------------------------------- 
 */

// Varible contador que permite asignar nombre único a cada textBox agregado
var counter = 1;

$(document).ready(

    function () {
        $('#tbItems').dataTable({
            "language": {
                "url": "/Scripts/plugins/dataTables/Spanish.txt"
            }
        });

        // Función de autocomplete en los dropdown
        $("#ddlItems").combobox();

        //Antes de ir a la acción Post del submit, se agrega la lista de ítems
        $("#formCreate").submit(function (eventObj) {
            // Array JSON que contendrá los id de los contratoItem que se agreguen a la boleta
            var items = [];

            // Bandera que indicará si debe hacerse el submit o no
            var doSubmit = true;

            // Se quita la propiedad de dataTable(paginación)
            $('#tbItems').dataTable().fnDestroy();

            // Se listan todos los items de la tabla
            $('#tbItems > tbody > tr').each(function () {

                // Objeto simple que contendrá los detalles de cada ítem de la boleta
                var singleObj = {}
                singleObj['idItem'] = $(this).attr('id');
                singleObj['precio'] = removeCurrency($(this).children("td").eq(3).find("input:eq(0)").val());

                // Se verifica que la cantidad sea numérica
                if (!(/[0-9]$/.test(singleObj.precio))) {
                    // Se indica el error
                    alert("Algunos campos son incorrectos, verifique los precios");
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

            // Si no hay problemas, Se crea el Json con la lista de ítems
            var jsonItems = { "Items": items };

            // Se adjunta al submit el Json de los items de la boleta
            $('<input />').attr('type', 'hidden')
                .attr('name', "jsonItems")
                .attr('value', $.toJSON(jsonItems))
                .appendTo('#formCreate');

            return true;
        }),

        // Función que permite agregar una fila con los detalles del item seleccionado en la sección items del Wizard
        $('#btnAgregarItem').click(function () {
            var dd = document.getElementById('ddlItems');
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
                url: '/ContratoItem/ItemDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id
                },
                success: function (data) {
                    var json = data;

                    var fila = '<tr id=' + _id + '><td>' + json.codigoItem + '</td> ';
                    fila += '<td>' + json.descripcion + '</td>';
                    fila += '<td align="center">' + json.unidadMedida + '</td>';
                    fila += '<td align="center"><input class="form-control" style="text-align:right"  onchange="addCurrency($(this))" id="txtPrecio' + counter + '" name="txtPrecio' + counter + '" type="text" >'; /*</td>*/
                    fila += '<span class="text-danger field-validation-error" data-valmsg-for="txtPrecio' + counter + '" data-valmsg-replace="true"></span> </td>';
                    fila += '<td align="center"><button class="remove btn btn-danger" onclick="eliminarItem(' + _id + ', \' ' + json.codigoItem + '\')">Quitar Item</button> </td></tr>';

                    // Aumenta el Contador
                    counter += 1;

                    // Se quita la propiedad de dataTable(paginación)
                    $('#tbItems').dataTable().fnDestroy();

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbItems > tbody:last').append(fila);
                    
                    // Se agregan las validaciones de precio
                    $('#tbItems > tbody > tr:last').find('input:eq(0)').rules('add', {
                        number: true, // Validación de números
                        required: true, // Validación de campos vacíos
                        messages: {
                            required: "Debe ingresar un precio.",
                            number: "Ingrese un precio válido."
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

// Funcion que le da formato a un numero ejemplo: console.info(formatNumber(1240.5));    // 1,240.5
function addCurrency(_this) {
    // Almacena la columna donde se escribió algo
    var num = _this.parent().find("input:eq(0)").val();
    if ((num == '') || (num == '₡')) {
        _this.parent().find("input:eq(0)").val('');
        return false;
    }
    if (!isNumber(num)) {
        return false;
    }

    num = numberFormat(num);
    //num = '₡' + num; // Se agrega el símbolo de colón
   
    _this.parent().find("input:eq(0)").val(num);
}

// Esta función recibe un valor e indica si es un número
function isNumber(num){
    return (/[0-9]$/.test(num));
}

/**
 * Funcion que devuelve un numero separando los separadores de miles
 * Puede recibir valores negativos y con decimales
 */
function numberFormat(numero) {
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