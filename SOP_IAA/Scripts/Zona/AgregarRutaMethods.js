// Array que contendrá los id de las rutas nuevas de la zona
var rutas = [];

$(document).ready(

    function () {
        
        // Función que permite agregar una fila con los detalles de una ruta a la tabla de rutas de la zona
        $('#btnAgregarRuta').click(function () {
            var dd = document.getElementById('ddlRutas')
            var _id = dd.options[dd.selectedIndex].value;

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
                    fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarRuta(' + json.id + ')">Quitar Ruta</button> </td></tr>';

                    //Agrega la ruta a la tabla htlm
                    $('#tbRutas > tbody:last').append(fila);

                    //Elimina la ruta del dropdownlist
                    $("#tbRutas option:selected").remove();

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

        //Antes de ir a la acción Post del submit, se agregan la lista de rutas
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


function eliminarRuta(_id) {
    $.ajax({
        url: '/ContratoView/RutaDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = $.parseJSON(data);

            for (var i = rutas.length - 1; i >= 0; i--) {
                if (rutas[i] == _id) {
                    rutas.splice(i, 1);
                }
            }
            
            $("<option value=" + json.id + ">" + json.nombre + "</option>").appendTo("#ddlRutas");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}
