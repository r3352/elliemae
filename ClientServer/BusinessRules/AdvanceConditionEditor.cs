// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.AdvanceConditionEditor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class AdvanceConditionEditor
  {
    private FieldFilterList filters;

    public AdvanceConditionEditor(string xmlCondition)
    {
      if (!((xmlCondition ?? "") != ""))
        return;
      this.loadCondition(xmlCondition);
    }

    private void loadCondition(string xml)
    {
      this.filters = (FieldFilterList) new XmlSerializer().Deserialize(xml, typeof (FieldFilterList));
    }

    public void ClearFilters() => this.filters.Clear();

    public void UpdateOptionValueList()
    {
      foreach (FieldFilter filter in (List<FieldFilter>) this.filters)
      {
        if (filter.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList)
        {
          List<string> values = new List<string>();
          FieldDefinition field = (FieldDefinition) StandardFields.GetField(filter.FieldID);
          string valueFrom = filter.ValueFrom;
          char[] separator = new char[1]{ ';' };
          foreach (string strA in valueFrom.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            foreach (FieldOption option in field.Options)
            {
              if (string.Compare(strA, option.ReportingDatabaseValue, true) == 0)
              {
                if (!string.IsNullOrEmpty(option.Value) || filter.FieldID == "1393")
                  values.Add(option.Value);
                if (!values.Contains(option.ReportingDatabaseValue))
                  values.Add(option.ReportingDatabaseValue);
              }
            }
          }
          filter.ValueFrom = string.Join(";", (IEnumerable<string>) values);
        }
      }
    }

    public string GetConditionXml() => new XmlSerializer().Serialize((object) this.filters);

    public string GetConditionScript()
    {
      return AdvanceConditionEditor.GetConditionScriptForFilter(this.filters);
    }

    public static string GetConditionScriptForFilter(FieldFilterList filters)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (FieldFilter filter in (List<FieldFilter>) filters)
      {
        stringBuilder.Append(filter.LeftParenthesesToString);
        stringBuilder.Append(AdvanceConditionEditor.createFilterExpression(filter));
        stringBuilder.Append(filter.RightParenthesesToString);
        if (filter.JointToken != JointTokens.Nothing)
          stringBuilder.Append(" " + filter.JointTokenToString + " ");
      }
      return stringBuilder.ToString();
    }

    private static string createFilterExpression(FieldFilter filter)
    {
      switch (filter.FieldType)
      {
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric:
          return AdvanceConditionEditor.createNumericExpression(filter);
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDate:
          return AdvanceConditionEditor.createDateExpression(filter, false);
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsMonthDay:
          return AdvanceConditionEditor.createMonthDayExpression(filter);
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsOptionList:
          return AdvanceConditionEditor.createOptionListExpression(filter);
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsYesNo:
          return AdvanceConditionEditor.createYesNoExpression(filter);
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsCheckbox:
          return AdvanceConditionEditor.createCheckboxExpression(filter);
        case EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsDateTime:
          return AdvanceConditionEditor.createDateExpression(filter, true);
        default:
          return AdvanceConditionEditor.createStringExpression(filter);
      }
    }

    private static string createCheckboxExpression(FieldFilter filter)
    {
      return filter.OperatorType == OperatorTypes.IsChecked ? "[" + filter.FieldID + "] = \"X\"" : "[" + filter.FieldID + "] <> \"X\"";
    }

    private static string createYesNoExpression(FieldFilter filter)
    {
      return filter.OperatorType == OperatorTypes.IsYes ? "[" + filter.FieldID + "] = \"Y\"" : "[" + filter.FieldID + "] <> \"Y\"";
    }

    private static string createOptionListExpression(FieldFilter filter)
    {
      string[] optionsList = filter.GetOptionsList();
      if (optionsList.Length == 0)
        return "";
      if (optionsList.Length == 1)
        return "[" + filter.FieldID + "] " + (filter.OperatorType == OperatorTypes.IsAnyOf ? "=" : "<>") + " \"" + optionsList[0] + "\"";
      string str = "(";
      if (filter.OperatorType == OperatorTypes.IsNotAnyOf)
        str = "NOT " + str;
      for (int index = 0; index < optionsList.Length; ++index)
      {
        if (index > 0)
          str += " OR ";
        str = str + "[" + filter.FieldID + "] = \"" + optionsList[index] + "\"";
      }
      return str + ")";
    }

    private static string createDateExpression(FieldFilter filter, bool includeTime)
    {
      string str = "[@" + filter.FieldID + "]";
      string format = "M/d/yyyy";
      if (includeTime)
        format += " hh:mm";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return str + " = #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.IsNot:
          return str + " <> #" + Utils.ParseDate((object) filter.ValueFrom).ToString(format) + "#";
        case OperatorTypes.NotEqual:
          return str + " <> #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.GreaterThan:
        case OperatorTypes.DateAfter:
          return str + " > #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.DateOnOrBefore:
          return str + " <= #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.LessThan:
        case OperatorTypes.DateBefore:
          return str + " < #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
          return str + " >= #" + filter.DateFrom.ToString(format) + "#";
        case OperatorTypes.DateBetween:
          string[] strArray1 = new string[9];
          strArray1[0] = "(";
          strArray1[1] = str;
          strArray1[2] = " >= #";
          DateTime dateTime1 = filter.DateFrom;
          strArray1[3] = dateTime1.ToString(format);
          strArray1[4] = "# AND ";
          strArray1[5] = str;
          strArray1[6] = " <= #";
          dateTime1 = filter.DateTo;
          strArray1[7] = dateTime1.ToString(format);
          strArray1[8] = "#)";
          return string.Concat(strArray1);
        case OperatorTypes.DateNotBetween:
          string[] strArray2 = new string[9];
          strArray2[0] = "(";
          strArray2[1] = str;
          strArray2[2] = " < #";
          DateTime dateTime2 = filter.DateFrom;
          strArray2[3] = dateTime2.ToString(format);
          strArray2[4] = "# OR ";
          strArray2[5] = str;
          strArray2[6] = " > #";
          dateTime2 = filter.DateTo;
          strArray2[7] = dateTime2.ToString(format);
          strArray2[8] = "#)";
          return string.Concat(strArray2);
        case OperatorTypes.EmptyDate:
          return "IsEmpty([" + filter.FieldID + "])";
        case OperatorTypes.NotEmptyDate:
          return "Not IsEmpty([" + filter.FieldID + "])";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a date field");
      }
    }

    private static string createMonthDayExpression(FieldFilter filter)
    {
      string str = "[@" + filter.FieldID + "]";
      string format = "M/d";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return str + " = XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.IsNot:
          return str + " <> XMonthDay(\"" + Utils.ParseDate((object) filter.ValueFrom).ToString(format) + "\")";
        case OperatorTypes.NotEqual:
          return str + " <> XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.GreaterThan:
        case OperatorTypes.DateAfter:
          return str + " > XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.DateOnOrBefore:
          return str + " <= XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.LessThan:
        case OperatorTypes.DateBefore:
          return str + " < XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
          return str + " >= XMonthDay(\"" + filter.DateFrom.ToString(format) + "\")";
        case OperatorTypes.CurrentMonth:
        case OperatorTypes.DateBetween:
          return "(" + str + " >= XMonthDay(\"" + filter.DateFrom.ToString(format) + "\") AND " + str + " <= XMonthDay(\"" + filter.DateTo.ToString(format) + "\"))";
        case OperatorTypes.DateNotBetween:
          return "(" + str + " < XMonthDay(\"" + filter.DateFrom.ToString(format) + "\") OR " + str + " > XMonthDay(\"" + filter.DateTo.ToString(format) + "\"))";
        case OperatorTypes.EmptyDate:
          return "IsEmpty([" + filter.FieldID + "])";
        case OperatorTypes.NotEmptyDate:
          return "Not IsEmpty([" + filter.FieldID + "])";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a date field");
      }
    }

    private static string createNumericExpression(FieldFilter filter)
    {
      string str = "[#" + filter.FieldID + "]";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return str + " <> " + filter.ValueFrom;
        case OperatorTypes.Equals:
          return str + " = " + filter.ValueFrom;
        case OperatorTypes.GreaterThan:
          return str + " > " + filter.ValueFrom;
        case OperatorTypes.NotGreaterThan:
          return str + " <= " + filter.ValueFrom;
        case OperatorTypes.LessThan:
          return str + " < " + filter.ValueFrom;
        case OperatorTypes.NotLessThan:
          return str + " >= " + filter.ValueFrom;
        case OperatorTypes.Between:
          return "(" + str + " >= " + filter.ValueFrom + " AND " + str + " <= " + filter.ValueTo + ")";
        case OperatorTypes.NotBetween:
          return "(" + str + " < " + filter.ValueFrom + " OR " + str + " > " + filter.ValueTo + ")";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a numeric field");
      }
    }

    private static string createStringExpression(FieldFilter filter)
    {
      string str = "[" + filter.FieldID + "]";
      switch (filter.OperatorType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return str + " = \"" + filter.ValueFrom + "\"";
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return str + " <> \"" + filter.ValueFrom + "\"";
        case OperatorTypes.StartsWith:
          return str + ".StartsWith(\"" + filter.ValueFrom + "\")";
        case OperatorTypes.DoesnotStartWith:
          return "NOT " + str + ".StartsWith(\"" + filter.ValueFrom + "\")";
        case OperatorTypes.Contains:
          return str + ".Contains(\"" + filter.ValueFrom + "\")";
        case OperatorTypes.DoesnotContain:
          return "NOT " + str + ".Contains(\"" + filter.ValueFrom + "\")";
        default:
          throw new Exception("The specified operator (" + (object) filter.OperatorType + ") is invalid for a string field");
      }
    }
  }
}
