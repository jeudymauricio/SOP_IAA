//$(document).ready(
            function onInit() {
                // Array que contendrá los id de los ingenieros del proyecto
                var ingenieros = [];

                // Array que contendrá los id de los laboratorios del proyecto
                var laboratorios = [];

                // Función que le da funcionalidad al Wizard
                $('#rootwizard').bootstrapWizard({
                    onTabShow: function (tab, navigation, index) {
                        /*if (index == 1) {
                            $.ajax({
                                url: '/ingeniero/Create',
                                contentType: 'application/html; charset=utf-8',
                                type: 'GET',
                                datatype:'html'
                            })
                            .success(function(result){
                                $('#hola').html(result);
                            })
                            .error(function (xhr, status) {
                                alert(status)
                            })
                        }*/
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
                        //@*alert('Utilice los botones de Siguiente, Anterior para desplazarse');*@
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

                            var fila = '<tr id='+json.persona.id+'><td>' + json.persona.nombre + ' ' + json.persona.apellido1 + ' ' + json.persona.apellido2 + '</td> ';
                            fila += '<td>' + json.rol + '</td>';
                            fila += '<td>' + json.descripcion + '</td>';
                            fila += '<td>' + json.departamento + '</td>';
                            fila += '<td> <button class="remove btn btn-danger" onclick=" eliminar(' + json.persona.id + ')">Quitar Ingeniero</button> </td></tr>';

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

                // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
                $(document).on("click", "#tbIngenieros button.remove", function () {
                    $(this).parents("tr").remove();
                });

            }
            //);

// Función que herada cada boton de la tabla ingenieros que permite quitar el ingeniero de la lista y regresarlo al dropdown
function eliminar(_id) {
    $.ajax({
        url: '/ContratoView/IngenieroDetalles/',
        type: "GET",
        dataType: "json",
        data: {
            id: _id
        },

        success: function (data) {
            var json = $.parseJSON(data);
            $("<option value=" + json.persona.id + ">"+json.persona.nombre+" "+ json.persona.apellido1+" "+json.persona.apellido2+"</option>").appendTo("#ddlIngenieros");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    })
}

// Toma los datos generales, los ingenieros y laboratorios, forma un json con ellos y los envía al controlador para realizar la inserción a la base de datos
function CrearContrato() {

}