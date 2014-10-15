/*
 *---------------------------------------------- Métodos de la vista 'Create' del ContratoController ---------------------------------------------- 
 */

$(document).ready(

    function () {
        // Funciones de autocomplete en los dropdown
        $("#ddlIdContratista").combobox();
        $("#ddlIdZona").combobox();
        $("#ddlIdFondo").combobox();
        $("#ddlIngenieros").combobox();
        $("#ddlLaboratorios").combobox();

        // Función del Wizard
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
                //alert('Utilice los botones de Siguiente, Anterior para desplazarse');
                return false;
            }
        }),

        // Función del DatePicker en los campos de Fecha
        $("#txtFechaInicio").datepicker({ dateFormat: 'dd/mm/yy' }),
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

        // Función que permite agregar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $('#btnAgregarIngeniero').click(function () {
            var dd = document.getElementById('ddlIngenieros');
            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Se deshabilita el boton mientras se realiza la acción
            $(this).toggleClass('disabled', true);

            // Este ajax se realiza una acción de controlador donde envía el id del ingeniero a buscar y recibe como retorno un JSON con los detalles del Ingeniero
            $.ajax({
                url: '/Contrato/IngenieroDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id
                },
                success: function (data) {
                    var json = $.parseJSON(data);
                    var nombreIngeniero = json.persona.nombre + ' ' + json.persona.apellido1 + ' ' + json.persona.apellido2;

                    var fila = '<tr id=' + json.persona.id + '><td>' + nombreIngeniero + '</td> ';
                    fila += '<td>' + json.rol + '</td>';
                    fila += '<td>' + json.descripcion + '</td>';
                    fila += '<td>' + json.departamento + '</td>';
                    fila += '<td> <button class="remove btn btn-danger" onclick="eliminarIngeniero(' + json.persona.id + ',\'' + nombreIngeniero + '\')">Quitar Ingeniero</button> </td></tr>';

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbIngenieros > tbody:last').append(fila);

                    //Elimina el ingeniero del dropdownlist
                    $("#ddlIngenieros option:selected").remove();

                    // Actualiza el dropdown
                    try {
                        $("#ddlIngenieros").parent().find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
                    }
                    catch (error) {
                        $("#ddlIngenieros").parent().find('span.custom-combobox').find('input:text').val('');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(errorThrown);
                }
            });

            // Se habilita nuevamente el botón
            $(this).toggleClass('disabled', false);
        }),

        $('#btnAgregarLaboratorio').click(function () {
            var dd = document.getElementById('ddlLaboratorios')
            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Se deshabilita el boton mientras se realiza la acción
            $(this).toggleClass('disabled', true);

            // Este ajax se realiza una acción de cobtrolador donde envía el id del laboratorio a buscar y recibe como retorno un JSON con los detalles del laboratorio
            $.ajax({
                url: '/Contrato/LaboratorioDetalles/',
                type: "GET",
                dataType: "json",
                data: {
                    id: _id
                },

                success: function (data) {
                    var json = $.parseJSON(data);

                    var fila = '<tr id=' + json.id + '><td>' + json.nombre + '</td> ';
                    fila += '<td>' + json.tipo + '</td>';
                    fila += '<td> <button class="remove btn btn-danger" onclick=" eliminarLaboratorio(' + json.id + ',\'' + json.nombre + '\')">Quitar Laboratorio</button> </td></tr>';

                    //Agrega el ingeniero a la tabla htlm
                    $('#tbLaboratorios > tbody:last').append(fila);

                    //Elimina el ingeniero del dropdownlist
                    $("#ddlLaboratorios option:selected").remove();

                    // Actualiza el dropdown
                    try {
                        $("#ddlLaboratorios").parent().find('span.custom-combobox').find('input:text').val(dd.options[dd.selectedIndex].text);
                    }
                    catch (error) {
                        $("#ddlLaboratorios").parent().find('span.custom-combobox').find('input:text').val('');
                    }

                },
                error: function (xhr, textStatus, errorThrown) {
                    if (xhr.status == 400) {
                        // Bad request
                        alert('Error: Consulta inválida.\nVerifique que seleccionó un ingeniero.');
                    }
                    else if (xhr.status === 401) {
                        // Unauthorized error
                        alert('Error: Acceso denegado.\n Verifique que tenga privilegios para realizar la operación.');
                    }
                    else if (xhr.status == 404) {
                        // Not found
                        alert('Error: no se encontraron los detalles del ingeniero.\nVerifique que existe el ingeniero.');
                    }
                    else if (xhr.status == 500) {
                        // Server side error
                        alert('Error del servidor.\n Espere unos segundos y vuelva a reitentar.');
                    }
                    else {
                        alert('Error: \n' + errorThrown + 'Reitente de nuevo.');
                    }
                }
            });

            // Se habilita nuevamente el botón
            $(this).toggleClass('disabled', false);
        }),

        // Función que permite quitar una fila con los detalles del ingeniero seleccionado en la sección Ingenieros del Wizard
        $(document).on("click", "#tbIngenieros button.remove", function () {
            $(this).parents("tr").remove();
        }),

        // Función que permite quitar una fila con los detalles del laboratorio seleccionado en la sección Laboratorios del Wizard
        $(document).on("click", "#tbLaboratorios button.remove", function () {
            $(this).parents("tr").remove();
        }),

        //Antes de ir a la acción Post del submit, se agregan los ingenieros y labs modificados
        $("#formCreateContract").submit(function (eventObj) {
            // Array que contendrá los id de los ingenieros del proyecto
            var ingenieros = [];

            // Array que contendrá los id de los laboratorios del proyecto
            var laboratorios = [];

            // Se agregan a la lista todos los ingenieros de la tabla ingenieros
            $('#tbIngenieros > tbody > tr').each(function () {
                ingenieros.push($(this).attr('id').toString());
            });

            // Se agregan a la lista todos los laboratorios de la tabla laboratorios
            $('#tbLaboratorios > tbody > tr').each(function () {
                laboratorios.push($(this).attr('id').toString());
            });

            // Se preparan las listas antes de convertirlas a JSON
            var jsonDatosIngenieros = { "Ingenieros": ingenieros };
            var jsonDatosLaboratorios = { "Laboratorios": laboratorios };

            // Json de los ingenieros que va a tener el contrato
            $('<input />').attr('type', 'hidden')
                .attr('name', "jsonIng")
                .attr('value', $.toJSON(jsonDatosIngenieros))
                .appendTo('#formCreateContract');

            // Json de los laboratorios que va a tener el contrato
            $('<input />').attr('type', 'hidden')
                .attr('name', "jsonLab")
                .attr('value', $.toJSON(jsonDatosLaboratorios))
                .appendTo('#formCreateContract');

            return true;
        })
    });

// Devuelve el nombre e id del ingeniero eliminado al dropdown de ingenieros
function eliminarIngeniero(_id, _nombreIngeniero) {
    $("<option value=" + _id + ">" + _nombreIngeniero.toString() + "</option>").appendTo("#ddlIngenieros");
}

// Devuelve el nombre e id del laboratorio eliminado al dropdown de laboratorios
function eliminarLaboratorio(_id, _nombre) {
    $("<option value=" + _id + ">" + _nombre + "</option>").appendTo("#ddlLaboratorios");
}
