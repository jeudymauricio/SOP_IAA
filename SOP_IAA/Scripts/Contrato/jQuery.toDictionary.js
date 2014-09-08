   1:  /*!
   2:   * jQuery toDictionary() plugin
   3:   *
   4:   * Version 1.2 (11 Apr 2011)
   5:   *
   6:   * Copyright (c) 2011 Robert Koritnik
   7:   * Licensed under the terms of the MIT license
   8:   * http://www.opensource.org/licenses/mit-license.php
   9:   */
  10:   
  11:  (function ($) {
  12:   
  13:      // #region String.prototype.format
  14:      // add String prototype format function if it doesn't yet exist
  15:      if ($.isFunction(String.prototype.format) === false)
  16:      {
  17:          String.prototype.format = function () {
  18:              var s = this;
  19:              var i = arguments.length;
  20:              while (i--)
  21:              {
  22:                  s = s.replace(new RegExp("\\{" + i + "\\}", "gim"), arguments[i]);
  23:              }
  24:              return s;
  25:          };
  26:      }
  27:      // #endregion
  28:   
  29:      // #region Date.prototype.toISOString
  30:      // add Date prototype toISOString function if it doesn't yet exist
  31:      if ($.isFunction(Date.prototype.toISOString) === false)
  32:      {
  33:          Date.prototype.toISOString = function () {
  34:              var pad = function (n, places) {
  35:                  n = n.toString();
  36:                  for (var i = n.length; i < places; i++)
  37:                  {
  38:                      n = "0" + n;
  39:                  }
  40:                  return n;
  41:              };
  42:              var d = this;
  43:              return "{0}-{1}-{2}T{3}:{4}:{5}.{6}Z".format(
  44:                  d.getUTCFullYear(),
  45:                  pad(d.getUTCMonth() + 1, 2),
  46:                  pad(d.getUTCDate(), 2),
  47:                  pad(d.getUTCHours(), 2),
  48:                  pad(d.getUTCMinutes(), 2),
  49:                  pad(d.getUTCSeconds(), 2),
  50:                  pad(d.getUTCMilliseconds(), 3)
  51:              );
  52:          };
  53:      }
  54:      // #endregion
  55:   
  56:      var _flatten = function (input, output, prefix, includeNulls) {
  57:          if ($.isPlainObject(input))
  58:          {
  59:              for (var p in input)
  60:              {
  61:                  if (includeNulls === true || typeof (input[p]) !== "undefined" && input[p] !== null)
  62:                  {
  63:                      _flatten(input[p], output, prefix.length > 0 ? prefix + "." + p : p, includeNulls);
  64:                  }
  65:              }
  66:          }
  67:          else
  68:          {
  69:              if ($.isArray(input))
  70:              {
  71:                  $.each(input, function (index, value) {
  72:                      _flatten(value, output, "{0}[{1}]".format(prefix, index));
  73:                  });
  74:                  return;
  75:              }
  76:              if (!$.isFunction(input))
  77:              {
  78:                  if (input instanceof Date)
  79:                  {
  80:                      output.push({ name: prefix, value: input.toISOString() });
  81:                  }
  82:                  else
  83:                  {
  84:                      var val = typeof (input);
  85:                      switch (val)
  86:                      {
  87:                          case "boolean":
  88:                          case "number":
  89:                              val = input;
  90:                              break;
  91:                          case "object":
  92:                              // this property is null, because non-null objects are evaluated in first if branch
  93:                              if (includeNulls !== true)
  94:                              {
  95:                                  return;
  96:                              }
  97:                          default:
  98:                              val = input || "";
  99:                      }
 100:                      output.push({ name: prefix, value: val });
 101:                  }
 102:              }
 103:          }
 104:      };
 105:   
 106:      $.extend({
 107:          toDictionary: function (data, prefix, includeNulls) {
 108:              /// <summary>Flattens an arbitrary JSON object to a dictionary that Asp.net MVC default model binder understands.</summary>
 109:              /// <param name="data" type="Object">Can either be a JSON object or a function that returns one.</data>
 110:              /// <param name="prefix" type="String" Optional="true">Provide this parameter when you want the output names to be prefixed by something (ie. when flattening simple values).</param>
 111:              /// <param name="includeNulls" type="Boolean" Optional="true">Set this to 'true' when you want null valued properties to be included in result (default is 'false').</param>
 112:   
 113:              // get data first if provided parameter is a function
 114:              data = $.isFunction(data) ? data.call() : data;
 115:   
 116:              // is second argument "prefix" or "includeNulls"
 117:              if (arguments.length === 2 && typeof (prefix) === "boolean")
 118:              {
 119:                  includeNulls = prefix;
 120:                  prefix = "";
 121:              }
 122:   
 123:              // set "includeNulls" default
 124:              includeNulls = typeof (includeNulls) === "boolean" ? includeNulls : false;
 125:   
 126:              var result = [];
 127:              _flatten(data, result, prefix || "", includeNulls);
 128:   
 129:              return result;
 130:          }
 131:      });
 132:  })(jQuery);