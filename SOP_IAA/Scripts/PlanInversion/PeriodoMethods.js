$(document).ready(function () {
    $('#tbPlanInversion').dataTable({
        "order": [],
        "language": {
            "url": "/Scripts/plugins/dataTables/Spanish.txt"
        }
    });

    // Suma total de todas las rutas
    var montoPlan = new Decimal('0').toDP(4);

    // Se recorren los paneles para sacar los montos totales
    $('#rutasAcordeon > div').each(function () {

        // Suma parcial de cada ruta
        var montoRuta = new Decimal('0').toDP(4);

        // Se recorre la tabla interna que contiene los ítems de la ruta
        $(this).children('div:last').find('.table').children('tbody').children('tr').each(function () {

            // Se suman los montos de cada item
            montoRuta = montoRuta.plus(new Decimal(removeCurrency($(this).children("td").eq(5).find('input:eq(0)').val())));
        }); // 

        // Se actualiza el monto total de la ruta
        $(this).children('div:first').find('.btn-xs').val('₡' + numberFormatCR(montoRuta.toFormat('', 4).toString()));

        //Se agrega el monto parcial a la suma global
        montoPlan = montoPlan.plus(montoRuta);
        $('#txtMontoPlan').val('₡' + numberFormatCR(montoPlan.toFormat('', 4).toString()));

    }); // Fin de #rutasAcordeon.each

});

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