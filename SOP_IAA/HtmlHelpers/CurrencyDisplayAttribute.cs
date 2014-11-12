using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SOP_IAA.HTML_Helpers
{
    public class CurrencyDisplayAttribute : DataTypeAttribute
    {
        public string Culture { get; set; }

        public CurrencyDisplayAttribute(string culture)
            : base(DataType.Currency)
        {
            Culture = culture;
        }
    }  
}