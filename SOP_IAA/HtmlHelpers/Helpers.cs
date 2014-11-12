using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace SOP_IAA.HTML_Helpers
{
    public static class Helpers
    {

        // Helper para el tipo de moneda
        public static IHtmlString CurrencyDisplayFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            double value = double.Parse(expression.Compile().Invoke(helper.ViewData.Model).ToString());
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var prop = typeof(TModel).GetProperty(metadata.PropertyName);

            var attribute = prop.GetCustomAttribute(typeof(CurrencyDisplayAttribute)) as CurrencyDisplayAttribute;

            // this should be whatever html element you want to create
            TagBuilder tagBuilder = new TagBuilder("span");
            tagBuilder.SetInnerText(value.ToString("c", CultureInfo.CreateSpecificCulture(attribute.Culture)));

            return MvcHtmlString.Create(tagBuilder.ToString());
        }


        // Segundo try
        public static string Currency(this HtmlHelper helper, decimal data, string locale = "es-CR", bool woCurrency = false)
        {
            var culture = new System.Globalization.CultureInfo(locale);

            if (woCurrency || (helper.ViewData["woCurrency"] != null && (bool)helper.ViewData["woCurrency"]))
                return data.ToString(culture);

            return data.ToString("C", culture);
        }

    }
}