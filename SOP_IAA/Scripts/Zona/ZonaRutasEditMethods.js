// Array que contendrá los id de las rutas nuevas de la zona
var rutas = [];

$(document).ready(

    function () {

        // Función que permite agregar una fila con los detalles de una ruta a la tabla de rutas de la zona
        $('#btnAgregarRuta').click(function () {
            var dd = document.getElementById('ddlRutas')
            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Este ajax se realiza una acción de cobtrolador donde envía el id de la ruta a buscar y recibe como retorno un JSON con los detalles de la ruta
            $.ajax({
                url: '/Zona/RutaDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id
                },
                success: function (data) {
                    var json = $.parseJSON(data);
                    rutas.push(_id.toString());

                    var fila = '<tr id=' + json.id + '><td>' + json.nombre + '</td> ';
                    fila += '<td>' + json.descripcion + '</td>';
                    fila += '<td> <button class="remove btn btn-danger" onclick="eliminarRuta(' + json.id + ', \'' + (json.nombre) + '\')">Quitar Ruta</button> </td></tr>';

                    //Agrega la ruta a la tabla htlm
                    $('#tbRutas > tbody:last').append(fila);

                    //Elimina la ruta del dropdownlist
                    $("#ddlRutas option:selected").remove();

                    // Actualiza el dropdown
                    try {
                        $("#ddlRutas").parent().find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
                    }
                    catch (error) {
                        $("#ddlRutas").parent().find('span.custom-combobox').find('input:text').val('');
                    }

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        }),

        // Función que permite quitar una fila con los detalles de la ruta especificada
        $(document).on("click", "#tbRutas button.remove", function () {
            $(this).parents("tr").remove();
        }),

        // Función que recorre la tabla de ingenieros al cargar.
        $('#tbRutas > tbody  > tr').each(function () {

            //Se obtiene el id de cada tr que corresponde al id de los ingenieros para eliminar al ingeniero del dropdownlist
            $("#ddlRutas option[value='" + $(this).attr('id') + "']").remove();

            //Se agrega el ingeniero a la lista global
            rutas.push($(this).attr('id'));
        }),

        //Antes de ir a la acción Post del submit, se agrega la lista de rutas
        $("#formRutasAgregar").submit(function (eventObj) {
            var jsonDatosRutas = { "Rutas": rutas };

            // Json de las rutas que va a tener la zona
            $('<input />').attr('type', 'hidden')
                .attr('name', "jsonRutas")
                .attr('value', $.toJSON(jsonDatosRutas))
                .appendTo('#formRutasAgregar');

            return true;
        })
    });


function eliminarRuta(id, nombre) {
    console.log(id);
    console.log(nombre);
    for (var i = rutas.length - 1; i >= 0; i--) {
        if (rutas[i] == id) {
            rutas.splice(i, 1);
        }
    }
    $("<option value=" + id + ">" + nombre + "</option>").appendTo("#ddlRutas");
}
