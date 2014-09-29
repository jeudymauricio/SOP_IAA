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
        $.datepicker.setDefaults($.datepicker.regional['es'])
    }
)