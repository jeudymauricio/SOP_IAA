/*
 *---------------------------------------------- Métodos de la vista 'Create' del ItemReajusteController ---------------------------------------------- 
 */

// Varible contador que permite asignar nombre único a cada elemento agregado
var counter = 1;

$(document).ready(

    function () {
        
        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        // Función de autocompletar de los dropdown
        //$('#ddlItems').combobox();

        // Función del DatePicker en los campos de Fecha
        $("#txtFecha").datepicker({
            dateFormat: 'dd/mm/yy',
            changeMonth: true,
            changeYear: true,
            showButtonPanel: true,
            dateFormat: 'MM yy',
            onClose: function (dateText, inst) {
                var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(year, month, 1));
            }
        });
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
        $.datepicker.setDefaults($.datepicker.regional['es']);

        $('#txtFecha').rules('remove');
        
        // Función que permite agregar una ruta al panel principal
        $('#btnAgregarRuta').click(function () {
            var dd = document.getElementById('ddlItems');
            try {
                // Se trata de obtener el valor del dropdown
                var _id = dd.options[dd.selectedIndex].value;
            } catch (error) {
                return false;
            }

            // Se deshabilita el boton mientras se realiza la acción
            $(this).toggleClass('disabled', true);

            // Creación del panel group
            var panel = '<div class="panel panel-default" id=' + _id + '>';
            panel += '<div class="panel-heading" min-height: 37px;>';
            panel += '<h4 class="panel-title">';
            panel += '<a data-toggle="collapse" data-parent="#accordion" href="#collapse' + counter + '" class="collapsed" aria-expanded="false"> Ruta ' + dd.options[dd.selectedIndex].text + '</a>';
            panel += '<button type="button" class="btn btn-danger btn-xs pull-right"> Excluir Ruta </button>';
            panel += '</h4></div>';
            panel += '<div id="collapse' + counter + '" class="panel-collapse collapse" aria-expanded="false">';
            panel += '<div class="panel-body">';

            //Cuerpo del panel -- Donde se cargarán los ítems
            panel += '<div class="row">';
            panel += '<div class="col-sm-12">';

            panel += '<select id = "ddlItem' + counter + '" class = "form-control">';

            //console.log(itemList);
            
            for (var x = 0; x < itemList.length; x++) {
                panel += '<option value="' + itemList[x].Value + '">' + itemList[x].Text + '</option>';
            }

            panel += '</select>';


            panel += '<input class="btn btn-primary" type="button" id="btnAgregarItem" value="Incluir Ítem" />';
            panel += '</div>';
            panel += '</div>';
            panel += '<div class="table-responsive">';
            panel += '<table id="tbItems" class="table">';
            panel += '<thead>';
            panel += '<tr>';
            panel += '<th>Item</th>';
            panel += '<th style="text-align:center">Descripción</th> <th style="text-align:center">Unidad de Medida</th> <th style="text-align:center">Precio(₡)</th> <th style="text-align:center">Cantidad</th>';
            panel += '<th style="text-align:center">Monto(₡)</th> <th style="text-align:center"> </th>';
            panel += '</tr>';
            panel += '</thead>';
            panel += '<tbody>';
                
            panel += '</tbody> </table> </div>';

            panel += '</div></div></div>';
            
            $('#rutasAccordion').append(panel);

            //Elimina el ingeniero del dropdownlist
            $("#ddlItems option:selected").remove();

            counter += 1;

            // Se habilita nuevamente el botón
            $(this).toggleClass('disabled', false);
        });

    }); // .\document.ready

