
// Array que contendrá los id de los ingenieros del proyecto
var ingenieros = [];

// Array que contendrá los id de los laboratorios del proyecto
var laboratorios = [];

$(document).ready(

    function () {


        // Función que le da funcionalidad al Wizard
        $('#rootwizard').bootstrapWizard({
            onTabShow: function (tab, navigation, index) {
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
                @*alert('Utilice los botones de Siguiente, Anterior para desplazarse');*@
                return false;
            }
        });

        // Función del DatePicker en los campos de Fecha
        $(function () {
            $("#txtFechaInicio").datepicker({ dateFormat: 'dd/mm/yy' });
        });
        $(function ($) {
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
            };
            $.datepicker.setDefaults($.datepicker.regional['es']);
        });

        // Función que permite agregar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $('#btnAgregarIngeniero').click(function () {
            var dd = document.getElementById('ddlIngenieros')
            var _id = dd.options[dd.selectedIndex].value;

            // Este ajax se realiza una acción de cobtrolador donde envía el id del ingeniero a buscar y recibe como retorno un JSON con los detalles del Ingeniero
            $.ajax({
                url: '/ContratoView/IngenieroDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id
                },
                success: function (data) {
                    var json = $.parseJSON(data);
                    ingenieros.push(_id.toString());
                    //console.log("array -> " + ingenieros.toString());
                    var fila = '<tr id=' + json.persona.id + '><td>' + json.persona.nombre + ' ' + json.persona.apellido1 + ' ' + json.persona.apellido2 + '</td> ';
                    fila += '<td>' + json.rol + '</td>';
                    fila += '<td>' + json.descripcion + '</td>';
                    fila += '<td>' + json.departamento + '</td>';
                    fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarIngeniero(' + json.persona.id + ')">Quitar Ingeniero</button> </td></tr>';

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbIngenieros > tbody:last').append(fila);

                    //Elimina el ingeniero del dropdownlist
                    $("#ddlIngenieros option:selected").remove();

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        });

        $('#btnAgregarLaboratorio').click(function () {
            var dd = document.getElementById('ddlLaboratorios')
            var _id = dd.options[dd.selectedIndex].value;

            // Este ajax se realiza una acción de cobtrolador donde envía el id del laboratorio a buscar y recibe como retorno un JSON con los detalles del laboratorio
            $.ajax({
                url: '/ContratoView/LaboratorioDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id
                },

                success: function (data) {
                    var json = $.parseJSON(data);
                    laboratorios.push(_id.toString());
                    //console.log("array -> " + laboratorios.toString());

                    var fila = '<tr id=' + json.id + '><td>' + json.nombre + '</td> ';
                    fila += '<td>' + json.tipo + '</td>';
                    fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarLaboratorio(' + json.id + ')">Quitar Laboratorio</button> </td></tr>';

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbLaboratorios > tbody:last').append(fila);

                    //Elimina el ingeniero del dropdownlist
                    $("#ddlLaboratorios option:selected").remove();

                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            })
        });

        // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $(document).on("click", "#tbIngenieros button.remove", function () {
            $(this).parents("tr").remove();
        });

        // Función que permite quitar una fila con los detalles del laboratorio seleccionado en la sección Laboratorios del Wizard
        $(document).on("click", "#tbLaboratorios button.remove", function () {
            $(this).parents("tr").remove();
        });



    });


function eliminarIngeniero(_id) {
    $.ajax({
        url: '/ContratoView/IngenieroDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = $.parseJSON(data);
            //console.log("id ->" + _id);
            //var index = ingenieros.indexOf(_id);
            //console.log(index);
            for (var i = ingenieros.length - 1; i >= 0; i--) {
                if (ingenieros[i] == _id) {
                    ingenieros.splice(i, 1);
                }
            }
            // ingenieros.splice(ingenieros.indexOf(_id), 1);
            $("<option value=" + json.persona.id + ">" + json.persona.nombre + " " + json.persona.apellido1 + " " + json.persona.apellido2 + "</option>").appendTo("#ddlIngenieros");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

function eliminarLaboratorio(_id) {
    $.ajax({
        url: '/ContratoView/LaboratorioDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },
        success: function (data) {
            var json = $.parseJSON(data);
            console.log("id ->" + _id);
            //var index = ingenieros.indexOf(_id);
            //console.log(index);
            for (var i = laboratorios.length - 1; i >= 0; i--) {
                if (laboratorios[i] == _id) {
                    laboratorios.splice(i, 1);
                }
            }
            // ingenieros.splice(ingenieros.indexOf(_id), 1);
            $("<option value=" + json.id + ">" + json.nombre + "</option>").appendTo("#ddlLaboratorios");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

// Toma los datos generales, los ingenieros y laboratorios, forma un json con ellos y los envía al controlador para realizar la inserción a la base de datos
function CrearContrato() {

    var _idContratista = document.getElementById('ddlIdContratista').options[document.getElementById('ddlIdContratista').selectedIndex].value;
    var _licitacion = document.getElementById('txtLicitacion').value;
    var _lineacontrato = document.getElementById('txtLineaContrato').value;
    var _idZona = document.getElementById('ddlIdZona').options[document.getElementById('ddlIdZona').selectedIndex].value;
    var _FechaInicio = document.getElementById('txtFechaInicio').value;
    var _Plazo = document.getElementById('txtPlazo').value;
    var _Lugar = document.getElementById('txtLugar').value;
    var _idFondo = _idContratista = document.getElementById('ddlIdFondo').options[document.getElementById('ddlIdFondo').selectedIndex].value;

    var jsonDatosContrato = { 'idContratista': _idContratista, 'licitacion': _licitacion, 'lineaContrato': _lineacontrato, 'idZona': _idZona, 'fechaInicio': _FechaInicio, 'plazo': _Plazo, 'lugar': _Lugar, 'idFondo': _idFondo };
    var jsonDatosLaboratorios = { "Laboratorios": laboratorios };
    var jsonDatosIngenieros = { "Ingenieros": ingenieros };


    var jsonContrato = JSON.stringify(jsonDatosContrato);
    var jsonLaboratorios = JSON.stringify(jsonDatosLaboratorios);
    var jsonIngenieros = JSON.stringify(jsonDatosIngenieros);

    //console.log(jsonContrato);
    //console.log(jsonIngenieros);
    //console.log(jsonLaboratorios);

    //debugger;
    $.ajax({
        url: '/ContratoView/Create',
        type: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        data: {
            _jsonContrato: $.toDictionary(jsonContrato)//,
            //_jsonLaboratorios: jsonLaboratorios,
            //_jsonIngenieros: jsonIngenieros
        },
        async: false,
        success: function (data) {
            console.log("json Enviado");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log(errorThrown);
        }
    })
}