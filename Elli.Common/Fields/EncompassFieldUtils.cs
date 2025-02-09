// Decompiled with JetBrains decompiler
// Type: Elli.Common.Fields.EncompassFieldUtils
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Common.Fields
{
  public static class EncompassFieldUtils
  {
    public static bool HasModelPath(this EncompassField encompassField)
    {
      return encompassField != null && !string.IsNullOrWhiteSpace(encompassField.ModelPath);
    }

    public static string ApplyFieldFormatting(
      object value,
      FieldFormat format,
      bool formatEmptyValue = true)
    {
      return Utils.ApplyFieldFormatting(value.ToString(), format, formatEmptyValue);
    }

    public static object ConvertToNativeValue(object value, FieldFormat format, bool throwOnError = false)
    {
      return Utils.ConvertToNativeValue(value.ToString(), format, throwOnError);
    }

    public static object ConvertToNativeValue(
      object value,
      FieldFormat format,
      object defaultValue)
    {
      return Utils.ConvertToNativeValue(value.ToString(), format, defaultValue);
    }

    public static string FormatFieldValue(
      object value,
      EncompassField field,
      ref string fieldFormat)
    {
      FieldFormat fieldFormatFromName = FieldFormatEnumUtil.GetFieldFormatFromName(field.Format);
      switch (fieldFormatFromName)
      {
        case FieldFormat.YN:
          fieldFormat = "bool";
          break;
        case FieldFormat.INTEGER:
          fieldFormat = "integer";
          break;
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          fieldFormat = "decimal";
          break;
        case FieldFormat.DATE:
          fieldFormat = "date";
          break;
        case FieldFormat.DATETIME:
          fieldFormat = "datetime";
          break;
        default:
          fieldFormat = "string";
          break;
      }
      bool needsUpdate = false;
      return Utils.FormatInput(value.ToString(), fieldFormatFromName, ref needsUpdate);
    }

    public static string FormatFieldValue(
      object value,
      string fieldFormat,
      out string fieldInternalType)
    {
      FieldFormat fieldFormatFromName = FieldFormatEnumUtil.GetFieldFormatFromName(fieldFormat);
      switch (fieldFormatFromName)
      {
        case FieldFormat.DATE:
          fieldInternalType = "date";
          break;
        case FieldFormat.DATETIME:
          fieldInternalType = "datetime";
          break;
        default:
          fieldInternalType = "other";
          break;
      }
      bool needsUpdate = false;
      return Utils.FormatInput(value.ToString(), fieldFormatFromName, ref needsUpdate);
    }

    public static string RemoveSpecialCharFromFieldId(string fieldId, out FieldFormat format)
    {
      format = FieldFormat.UNDEFINED;
      if (fieldId.StartsWith("#") && fieldId.EndsWith("#"))
      {
        format = FieldFormat.DECIMAL;
        return fieldId.Substring(1, fieldId.Length - 2);
      }
      if (fieldId.StartsWith("#"))
      {
        format = FieldFormat.DECIMAL;
        return fieldId.Substring(1);
      }
      if (fieldId.StartsWith("@"))
      {
        format = FieldFormat.DATE;
        return fieldId.Substring(1);
      }
      if (!fieldId.StartsWith("+") && !fieldId.StartsWith("-"))
        return fieldId;
      format = FieldFormat.STRING;
      return fieldId.Substring(1);
    }

    public static object GetFormatedValue(
      object value,
      FieldFormat format,
      Dictionary<bool, string> fieldOptions = null)
    {
      if (value == null || value.Equals((object) string.Empty))
        return (object) string.Empty;
      if (format == FieldFormat.UNDEFINED)
        return value;
      try
      {
        if (format <= FieldFormat.YN)
        {
          if (format != FieldFormat.STRING)
          {
            if (format == FieldFormat.YN)
              return value is bool flag1 ? (object) EncompassFieldUtils.ParseYN(flag1) : (object) string.Concat(value);
          }
          else
            return value is bool flag2 ? (object) EncompassFieldUtils.ParseBooleanToString(flag2, fieldOptions) : (object) string.Concat(value);
        }
        else
        {
          switch (format - 201)
          {
            case FieldFormat.NONE:
              try
              {
                return (object) Utils.ParseInt(value);
              }
              catch
              {
                return (object) 0;
              }
            case (FieldFormat) 1:
            case (FieldFormat) 2:
            case (FieldFormat) 3:
            case (FieldFormat) 4:
            case (FieldFormat) 6:
            case (FieldFormat) 7:
            case (FieldFormat) 8:
            case (FieldFormat) 9:
            case (FieldFormat) 10:
              return (object) Utils.ParseDecimal(value);
            case (FieldFormat) 5:
              break;
            default:
              switch (format - 301)
              {
                case FieldFormat.NONE:
                case (FieldFormat) 3:
                  DateTime date = Utils.ParseDate(value);
                  return (object) (format == FieldFormat.DATE ? date.Date : date);
                case (FieldFormat) 2:
                  return (object) Utils.ParseMonthDay(value);
              }
              break;
          }
        }
        return value;
      }
      catch (Exception ex)
      {
        return value;
      }
    }

    private static string ParseYN(bool value) => !value ? "N" : "Y";

    private static string ParseBooleanToString(bool value, Dictionary<bool, string> fieldOptions)
    {
      if (fieldOptions == null || !fieldOptions.Any<KeyValuePair<bool, string>>())
        return EncompassFieldUtils.ParseYN(value);
      string str;
      return fieldOptions.TryGetValue(value, out str) ? str : string.Empty;
    }

    public static object GetNativeValue(object value, FieldFormat format)
    {
      if (format == FieldFormat.UNDEFINED || value == null)
        return value;
      if (value is string && string.IsNullOrEmpty((string) value))
        return (object) null;
      switch (format)
      {
        case FieldFormat.X:
          return (object) float.Parse(value.ToString());
        case FieldFormat.INTEGER:
          return (object) Utils.ParseInt(value, true);
        case FieldFormat.DECIMAL_1:
        case FieldFormat.DECIMAL_2:
        case FieldFormat.DECIMAL_3:
        case FieldFormat.DECIMAL_4:
        case FieldFormat.DECIMAL_6:
        case FieldFormat.DECIMAL_5:
        case FieldFormat.DECIMAL_7:
        case FieldFormat.DECIMAL:
        case FieldFormat.DECIMAL_10:
          return (object) Utils.ParseDecimal(value, true);
        case FieldFormat.DATE:
        case FieldFormat.DATETIME:
          return Utils.ConvertToDateTime(value, format, true);
        case FieldFormat.MONTHDAY:
          return (object) Utils.ParseMonthDay(value, true);
        default:
          return value;
      }
    }

    public static object GetNativeValueWithDefault(
      object value,
      FieldFormat format,
      string fieldId,
      Dictionary<bool, string> fieldOptions = null)
    {
      if (string.IsNullOrEmpty(fieldId))
        return (object) string.Empty;
      if (fieldId.StartsWith("#"))
        return Utils.GetNumeric(value, format);
      if (fieldId.StartsWith("+") || fieldId.StartsWith("-"))
      {
        string stringValue = Utils.GetStringValue(value);
        return !fieldId.StartsWith("+") ? (object) Utils.UnformatValue(stringValue, format) : (object) Utils.ApplyFieldFormatting(stringValue, format);
      }
      return fieldId.StartsWith("@") ? Utils.GetDate(value, format) : EncompassFieldUtils.GetFormatedValue(value, format, fieldOptions);
    }
  }
}
