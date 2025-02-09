// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.SQL
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  internal sealed class SQL
  {
    private const string className = "Query�";
    private static readonly string sw = Tracing.SwQuery;

    private SQL()
    {
    }

    public static string ToFastStringMatchClauseForContains(
      string fieldName,
      string subSelect,
      string value)
    {
      return fieldName + " in (" + subSelect + " like '%" + SQL.Escape(value) + "%')";
    }

    public static string ToStringMatchClause(
      string fieldName,
      string value,
      StringMatchType matchType)
    {
      if (fieldName == "")
        return "(1=0)";
      if (value == string.Empty && matchType == StringMatchType.Exact)
        return fieldName + " = '' or " + fieldName + " is NULL";
      if (value == null)
        return SQL.ToNullMatchClause(fieldName, matchType == StringMatchType.Exact);
      if (fieldName.ToLower() == "[loan].[isarchived]")
        return fieldName + " = " + (object) SQL.EncodeYNToBit(value);
      switch (matchType)
      {
        case StringMatchType.StartsWith:
          return fieldName + " like '" + SQL.Escape(value) + "%'";
        case StringMatchType.Contains:
          return fieldName + " like '%" + SQL.Escape(value) + "%'";
        case StringMatchType.GreaterThan:
          return fieldName + " > '" + SQL.Escape(value) + "'";
        case StringMatchType.GreaterThanOrEquals:
          return fieldName + " >= '" + SQL.Escape(value) + "'";
        case StringMatchType.LessThan:
          return fieldName + " < '" + SQL.Escape(value) + "'";
        case StringMatchType.LessThanOrEquals:
          return fieldName + " <= '" + SQL.Escape(value) + "'";
        case StringMatchType.NotEquals:
          return fieldName + " <> " + SQL.EncodeString(value);
        case StringMatchType.MultiValue:
          return fieldName + " in (" + value + ")";
        default:
          return fieldName + " = " + SQL.EncodeString(value);
      }
    }

    public static string ToOrdinalMatchClause(
      string fieldName,
      object value,
      OrdinalMatchType matchType)
    {
      if (value == null)
        return SQL.ToNullMatchClause(fieldName, matchType == OrdinalMatchType.Equals);
      string str = SQL.OrdinalMatchTypeToString(matchType);
      return fieldName + str + SQL.Encode(value);
    }

    public static string ToNullMatchClause(string fieldName, bool include)
    {
      return fieldName + " is " + (include ? "" : "not ") + " NULL";
    }

    public static string ToDateMatchClause(
      string fieldName,
      DateTime value,
      OrdinalMatchType matchType,
      DateMatchPrecision precision)
    {
      if (precision == DateMatchPrecision.Recurring)
        return fieldName.StartsWith("[LoanExternalFieldsDateValues].") ? SQL.ToRecurringDateMatchOffsetClause(fieldName, value, matchType) : SQL.ToRecurringDateMatchClause(fieldName, value, matchType);
      string str1 = SQL.DatePrecisionToString(precision);
      string str2 = SQL.OrdinalMatchTypeToString(matchType);
      if (precision == DateMatchPrecision.Exact)
        return fieldName + str2 + SQL.EncodeDateTime(value);
      return "DateDiff(" + str1 + ", " + SQL.EncodeDateTime(value) + ", " + fieldName + ")" + str2 + "0";
    }

    public static string ToRecurringDateMatchClause(
      string fieldName,
      DateTime value,
      OrdinalMatchType matchType)
    {
      switch (matchType)
      {
        case OrdinalMatchType.NotEquals:
          return "((Month(" + fieldName + ") <> " + (object) value.Month + ") or (Day(" + fieldName + ") <> " + (object) value.Day + "))";
        case OrdinalMatchType.GreaterThan:
          return "((Month(" + fieldName + ") > " + (object) value.Month + ") or ((Month(" + fieldName + ") = " + (object) value.Month + ") and (Day(" + fieldName + ") > " + (object) value.Day + ")))";
        case OrdinalMatchType.GreaterThanOrEquals:
          return "((Month(" + fieldName + ") > " + (object) value.Month + ") or ((Month(" + fieldName + ") = " + (object) value.Month + ") and (Day(" + fieldName + ") >= " + (object) value.Day + ")))";
        case OrdinalMatchType.LessThan:
          return "((Month(" + fieldName + ") < " + (object) value.Month + ") or ((Month(" + fieldName + ") = " + (object) value.Month + ") and (Day(" + fieldName + ") < " + (object) value.Day + ")))";
        case OrdinalMatchType.LessThanOrEquals:
          return "((Month(" + fieldName + ") < " + (object) value.Month + ") or ((Month(" + fieldName + ") = " + (object) value.Month + ") and (Day(" + fieldName + ") <= " + (object) value.Day + ")))";
        default:
          return "((Month(" + fieldName + ") = " + (object) value.Month + ") and (Day(" + fieldName + ") = " + (object) value.Day + "))";
      }
    }

    public static string ToRecurringDateMatchOffsetClause(
      string fieldName,
      DateTime value,
      OrdinalMatchType matchType)
    {
      DateTime universalTime = value.ToUniversalTime();
      DateTime dateTime = universalTime.AddDays(1.0).AddSeconds(-0.01);
      switch (matchType)
      {
        case OrdinalMatchType.NotEquals:
          return fieldName + " <> '" + (object) universalTime + "'))";
        case OrdinalMatchType.GreaterThan:
          return fieldName + " > '" + (object) universalTime + "'";
        case OrdinalMatchType.GreaterThanOrEquals:
          return fieldName + " >= '" + (object) universalTime + "'";
        case OrdinalMatchType.LessThan:
          return fieldName + " < '" + (object) universalTime + "'";
        case OrdinalMatchType.LessThanOrEquals:
          return fieldName + " <= '" + (object) universalTime + "'";
        default:
          return "((" + fieldName + " >= '" + (object) universalTime + "') and (" + fieldName + " <= '" + (object) dateTime + "'))";
      }
    }

    public static string Escape(string value)
    {
      value = value.Replace("'", "''");
      value = value.Replace("[", "[[]");
      value = value.Replace("%", "[%]");
      value = value.Replace("_", "[_]");
      return value;
    }

    public static string Encode(object value)
    {
      switch (value)
      {
        case null:
          return "NULL";
        case string _:
          return SQL.EncodeString((string) value);
        case DateTime dateTime:
          return SQL.EncodeDateTime(dateTime);
        case bool flag:
          return SQL.EncodeBoolean(flag);
        case Array _:
          return SQL.EncodeArray((Array) value);
        default:
          return value.ToString();
      }
    }

    public static int EncodeYNToBit(string value) => !(value.ToUpper() == "Y") ? 0 : 1;

    public static string EncodeString(string value) => "'" + value.Replace("'", "''") + "'";

    public static string EncodeBoolean(bool value) => !value ? "'F'" : "'T'";

    public static string EncodeFlag(bool value) => !value ? "0" : "1";

    public static string EncodeDateTime(DateTime value)
    {
      return "'" + string.Format("{0:0000}{1:00}{2:00} {3:00}:{4:00}", (object) value.Year, (object) value.Month, (object) value.Day, (object) value.Hour, (object) value.Minute) + "'";
    }

    public static string EncodeArray(Array data)
    {
      if (data.Length > 500)
      {
        StackTrace stackTrace = new StackTrace();
        string str = "";
        for (int index = 0; index < stackTrace.FrameCount && index < 10; ++index)
        {
          StackFrame frame = stackTrace.GetFrame(index);
          str = str + "stackframe(" + (object) index + "): " + frame.GetMethod().DeclaringType.FullName + "." + frame.GetMethod().Name + Environment.NewLine;
        }
        Tracing.Log(SQL.sw, "Query", TraceLevel.Warning, "number of elements passing into array exceeds 500" + Environment.NewLine + str);
      }
      StringBuilder stringBuilder = new StringBuilder();
      int lowerBound = data.GetLowerBound(0);
      int upperBound = data.GetUpperBound(0);
      for (int index = lowerBound; index <= upperBound; ++index)
      {
        stringBuilder.Append(SQL.Encode(data.GetValue(index)));
        if (index < upperBound)
          stringBuilder.Append(",");
      }
      return stringBuilder.ToString();
    }

    public static string DatePrecisionToString(DateMatchPrecision precision)
    {
      switch (precision)
      {
        case DateMatchPrecision.Exact:
          return "s";
        case DateMatchPrecision.Day:
          return "d";
        case DateMatchPrecision.Month:
          return "m";
        case DateMatchPrecision.Year:
          return "yy";
        case DateMatchPrecision.Hour:
          return "hh";
        case DateMatchPrecision.Minute:
          return "mi";
        default:
          return "s";
      }
    }

    public static string OrdinalMatchTypeToString(OrdinalMatchType matchType)
    {
      switch (matchType)
      {
        case OrdinalMatchType.Equals:
          return " = ";
        case OrdinalMatchType.NotEquals:
          return " <> ";
        case OrdinalMatchType.GreaterThan:
          return " > ";
        case OrdinalMatchType.GreaterThanOrEquals:
          return " >= ";
        case OrdinalMatchType.LessThan:
          return " < ";
        case OrdinalMatchType.LessThanOrEquals:
          return " <= ";
        default:
          return "=";
      }
    }

    public static string BinaryOperatorToString(BinaryOperator op)
    {
      return op == BinaryOperator.And || op != BinaryOperator.Or ? " and " : " or ";
    }
  }
}
