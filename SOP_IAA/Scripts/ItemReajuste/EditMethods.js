/*
 *---------------------------------------------- Métodos de la vista 'Edit' del ItemReajusteController ---------------------------------------------- 
 */

// Varible contador que permite asignar nombre único a cada textBox agregado
var counter = 1;
var idContrato;

$(document).ready(

    function () {
        
        // Regla para los números (verificar que se trata de un número y que no hay letras en el)
        jQuery.validator.addMethod("isNumberDecimal", function (value, element) {
            return this.optional(element) || !isNaN(removeCurrency(value));
        }, "El valor no es un número correcto");

        $('#formEdit').validate({
            errorPlacement: function (error, element) {
                error.appendTo(element.parent("td").children("span"));
            }
        });

        $('#tbReajustes > tbody  > tr').each(function () {
            $(  this).children("td").eq(4).find('input:eq(0)').rules('add', {
                number: true, // Validación de números
                isNumberDecimal: true,
                range: [0, 999],
                required: true, // Validación de campos vacíos
                messages: {
                    required: "Debe ingresar un reajuste.",
                    number: "Ingrese un reajuste válido.",
                    range: "Ingrese un numero menor o igual a 999,9999",
                    isNumberDecimal: "Ingrese un reajuste válido." // Validación propia declarada en el inicio del document.ready()
                }
            });
        });

        /*$("input[name='save']").on("click", function () {
            $(this).replaceWith('<img class="submit-form" src=http://www.techgig.com/Themes/Release/images/loading.gif />');
        })*/
    }); // .\document.ready

function makeSubmit(idContrato, strFechaReajuste) {

    $("input[name='save']").toggleClass('disabled', true)
    $("#loadingGif").show();
    
    // Array JSON que contendrá los id de los reajustes que se agreguen al contrato
    var items = [];
    
    // Bandera que indicará si debe hacerse el submit o no
    var doSubmit = true;

    // Se listan todos los items de la tabla
    $('#tbReajustes > tbody > tr').each(function () {

        // Objeto simple que contendrá los detalles de cada ítem de la boleta
        var singleObj = {}

        // Se ubican los input de Reajuste y Precio reajustado
        var txtReajuste = $(this).children("td").eq(4).find("input:eq(0)");
        var txtPrecioReajustado = $(this).children("td").eq(5).find("input:eq(0)");

        // Se obtienen los valores de reajuste y precio reajustado
        var reajuste = txtReajuste.val();
        var precioReajustado = txtPrecioReajustado.val();

        // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos) al precio actual y reajuste
        reajuste = removeCurrency(reajuste);
        precioReajustado = removeCurrency(precioReajustado);

        // Se verifica que el reajuste sea numérico
        if (!(txtReajuste.valid())) {
            // Se indica que no debe hacerse el submit
            doSubmit = false;
            // Se detiene el ciclo
            return false;
        }

        // Se verifica que el precio reajustado no tenga errores
        if (!(/[0-9]$/.test(precioReajustado))) {
            // Se indica el error
            alert("Algunos campos son incorrectos, verifique los reajustes");
            // Se indica que no debe hacerse el submit
            doSubmit = false;
            // Se detiene el ciclo
            return false;
        }

        // Se convierte el procentaje a decimal
        reajuste = (new Decimal(reajuste)).dividedBy(100);
        
        // Se agregan los detalles al objeto
        singleObj['idReajuste'] = $(this).attr('id');
        singleObj['idContratoItem'] = $(this).children("td").eq(0).attr('id'); 
        singleObj['reajuste'] = reajuste.toString();
        singleObj['precioReajustado'] = precioReajustado

        // Se agrega el objeto con los detalles del ítem a la lista a enviar en el submit
        items.push(singleObj);
    })

    // Se verifica si debe hacerse el submit o no
    if (!doSubmit) {
        $("input[name='save']").toggleClass('disabled', false);
        $("#loadingGif").hide();
        return false;
    }

    // Si no hay problemas, Se crea el Json con la lista de ítems y si idContratoItem
    var jsonItems = { "Items": items };

    // Se adjunta al submit el Json de los items a reajustar
    $('<input />').attr('type', 'hidden')
        .attr('name', 'jsonItems')
        .attr('value', $.toJSON(jsonItems))
        .appendTo('#formEdit');

    // Se adjunta al submit la fecha de los reajustes
    $('<input />').attr('type', 'hidden')
        .attr('name', 'strFechaReajuste')
        .attr('value', strFechaReajuste)
        .appendTo('#formEdit');

    $('<input />').attr('type', 'hidden')
        .attr('name', 'idContrato')
        .attr('value', idContrato)
        .appendTo('#formEdit');

    $("form").submit();
};

// Función que limpia los elementos no numéricos de los precios y establece el formato de CR
function removeCurrency(num) {

    // Se quita el simbolo de moneda
    num = num.replace(/₡/g, "");

    // Se remueve el separador de miles
    num = num.replace(/\./g, "");

    // Se intercambia , por . como separador de decimales (formato interno)
    num = num.replace(/,/g, ".");

    return num;
}

// Función de los input "Cantidad" que cuando se escribe un número se realizan cálculos con otros campos de la fila
function alpha(_this) {

    // Almacena la columna donde se escribió algo
    var td = _this;

    // Se ubican los input de precio actual, Reajuste y Precio reajustado
    var txtPrecioActual = td.parents("tr").children("td").eq(3).find("input:eq(0)");
    var txtReajuste = td.parent().find("input:eq(0)");
    var txtPrecioReajustado = td.parents("tr").children("td").eq(5).find("input:eq(0)");

    // Se obtienen los valores de precio actual y reajuste
    var precioActual = txtPrecioActual.val();
    var reajuste = txtReajuste.val();

    // Se remueve el signo de ₡ y los separadores de miles (en caso de tenerlos) al precio actual y reajuste
    precioActual = removeCurrency(precioActual)
    reajuste = removeCurrency(reajuste);

    // Se verifica que el reajuste sea numérico
    if (!(txtReajuste.valid())) {
        return false;
    }

    if(isNaN(reajuste)){
        //alert('El reajuste debe ser númerico y se usa la , como separador de decimales');
        txtPrecioReajustado.val("--- Error ---");
        return false;
    }
    else {
        // Se pasa el número a formato de numero de CR y se muestra en el textbox
        txtReajuste.val(numberFormatCR(new Decimal(reajuste).toDP(4).toFormat('',4).toString()));
    }
    
    // Se trata de hacer las operaciones
    try {

        // Se convierten los valores a decimal
        precioActual = new Decimal(precioActual);
        reajuste = new Decimal(reajuste);

        // Se pasa a valor decimal el porcentaje de reajuste
        var _reajusteDecimal = new Decimal(reajuste.dividedBy(100));

        // Se suma el reajuste al precio actual
        var _precioReajuste =  new Decimal((_reajusteDecimal.times(precioActual)).plus(precioActual)).toDP(4);

        txtPrecioReajustado.val("₡" + numberFormatCR(_precioReajuste.toFormat('',4).toString()));
    }
    catch (err) {
        if (err instanceof Error && err.name == 'Decimal Error') {
            txtPrecioReajustado.val("--- Error ---");
        }
        else {
            alert(err.message);
            txtPrecioReajustado.val("--- Error ---");
        }
    }

}

/**
 * Funcion que devuelve un numero separando los separadores de miles
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
