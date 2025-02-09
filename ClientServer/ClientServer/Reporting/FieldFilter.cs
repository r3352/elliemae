// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.FieldFilter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Trading;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  [Serializable]
  public class FieldFilter : TradeEntity, IXmlSerializable, IFilterEvaluator
  {
    private static OperatorTypesEnumNameProvider opTypeProvider = new OperatorTypesEnumNameProvider();
    private FieldTypes fieldType = FieldTypes.IsNothing;
    private string fieldID = "";
    private string criterionName = "";
    private OperatorTypes opType;
    private string fieldDescription = "";
    private string valueFrom = "";
    private string valueTo = "";
    private JointTokens jointToken;
    private int leftParentheses;
    private int rightParentheses;
    private string valueDescription;
    private bool isVolatile;
    private bool forceDataConversion;
    private FilterDataSource dataSource;

    public FieldFilter()
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom)
      : this(fieldType, fieldID, fieldID, fieldDescription, opType, valueFrom, "")
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom)
      : this(fieldType, fieldID, criterionName, fieldDescription, opType, valueFrom, "")
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom,
      bool isVolatile)
      : this(fieldType, fieldID, criterionName, fieldDescription, opType, valueFrom, "", isVolatile)
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string fieldDescription,
      OperatorTypes opType,
      string[] values)
      : this(fieldType, fieldID, fieldID, fieldDescription, opType, string.Join(";", values))
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string[] values)
      : this(fieldType, fieldID, criterionName, fieldDescription, opType, string.Join(";", values))
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string[] values,
      string valueDescription)
      : this(fieldType, fieldID, criterionName, fieldDescription, opType, values, valueDescription, false)
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string[] values,
      string valueDescription,
      bool isVolatile)
      : this(fieldType, fieldID, criterionName, fieldDescription, opType, values)
    {
      this.valueDescription = valueDescription;
      this.isVolatile = isVolatile;
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom,
      string valueTo,
      int leftParenthesis,
      int rightParenthesis)
      : this(fieldType, fieldID, fieldID, fieldDescription, opType, valueFrom, valueTo)
    {
      this.leftParentheses = leftParenthesis;
      this.rightParentheses = rightParenthesis;
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom,
      string valueTo)
      : this(fieldType, fieldID, fieldID, fieldDescription, opType, valueFrom, valueTo)
    {
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom,
      string valueTo)
      : this(fieldType, fieldID, criterionName, fieldDescription, opType, valueFrom, valueTo, false)
    {
    }

    public FieldFilter(DataRow r)
    {
      this.fieldType = (FieldTypes) r["QueryType"];
      this.fieldID = (string) r["QueryField"];
      this.OperatorType = (OperatorTypes) Enum.Parse(typeof (OperatorTypes), r["QueryOperators"].ToString());
      this.valueFrom = (string) r[nameof (ValueFrom)];
      this.valueTo = (string) r[nameof (ValueTo)];
      this.jointToken = (JointTokens) r[nameof (JointToken)];
      this.leftParentheses = Utils.ParseInt((object) r[nameof (LeftParentheses)].ToString());
      this.rightParentheses = Utils.ParseInt((object) r[nameof (RightParentheses)].ToString());
      this.fieldDescription = r["QueryDescription"].ToString();
    }

    public FieldFilter(
      FieldTypes fieldType,
      string fieldID,
      string criterionName,
      string fieldDescription,
      OperatorTypes opType,
      string valueFrom,
      string valueTo,
      bool isVolatile)
    {
      this.fieldType = fieldType;
      this.fieldID = fieldID;
      this.criterionName = criterionName;
      this.fieldDescription = fieldDescription;
      this.OperatorType = opType;
      this.valueFrom = valueFrom;
      this.valueTo = valueTo;
      this.isVolatile = isVolatile;
    }

    public FieldFilter(FieldFilter filter)
    {
      this.Id = filter.Id;
      this.fieldType = filter.FieldType;
      this.fieldID = filter.FieldID;
      this.criterionName = filter.criterionName;
      this.opType = filter.opType;
      this.valueFrom = filter.ValueFrom;
      this.valueTo = filter.valueTo;
      this.valueDescription = filter.valueDescription;
      this.jointToken = filter.JointToken;
      this.leftParentheses = filter.LeftParentheses;
      this.rightParentheses = filter.RightParentheses;
      this.fieldDescription = filter.fieldDescription;
      this.isVolatile = filter.isVolatile;
      this.dataSource = filter.dataSource;
    }

    public FieldFilter(XmlSerializationInfo info)
    {
      this.ReadId(info);
      this.fieldType = (FieldTypes) info.GetValue(nameof (fieldType), typeof (FieldTypes));
      this.fieldID = info.GetString(nameof (fieldID));
      this.criterionName = info.GetString(nameof (criterionName));
      this.fieldDescription = info.GetString(nameof (fieldDescription));
      this.OperatorType = (OperatorTypes) info.GetValue(nameof (opType), typeof (OperatorTypes));
      this.valueFrom = info.GetString(nameof (valueFrom));
      this.valueTo = info.GetString(nameof (valueTo));
      this.jointToken = (JointTokens) info.GetValue(nameof (jointToken), typeof (JointTokens));
      this.leftParentheses = info.GetInteger(nameof (leftParentheses));
      this.rightParentheses = info.GetInteger(nameof (rightParentheses));
      this.valueDescription = info.GetString(nameof (valueDescription));
      this.isVolatile = info.GetBoolean("volatile", false);
      this.forceDataConversion = info.GetBoolean(nameof (forceDataConversion), false);
      this.dataSource = (FilterDataSource) info.GetValue(nameof (dataSource), typeof (FilterDataSource), (object) FilterDataSource.Unknown);
      if (string.Compare(this.criterionName, "Loan.CreditScore", true) != 0)
        return;
      this.fieldID = "VASUMM.X23";
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      this.WriteId(info);
      info.AddValue("fieldType", (object) this.fieldType);
      info.AddValue("fieldID", (object) this.fieldID);
      info.AddValue("criterionName", (object) this.criterionName);
      info.AddValue("fieldDescription", (object) this.fieldDescription);
      info.AddValue("opType", (object) this.opType);
      info.AddValue("valueFrom", (object) this.valueFrom);
      info.AddValue("valueTo", (object) this.valueTo);
      info.AddValue("jointToken", (object) this.jointToken);
      info.AddValue("leftParentheses", (object) this.leftParentheses);
      info.AddValue("rightParentheses", (object) this.rightParentheses);
      info.AddValue("valueDescription", (object) this.valueDescription);
      info.AddValue("volatile", (object) this.isVolatile);
      info.AddValue("forceDataConversion", (object) this.forceDataConversion);
      info.AddValue("dataSource", (object) this.dataSource);
    }

    public FieldTypes FieldType
    {
      set => this.fieldType = value;
      get => this.fieldType;
    }

    public string FieldID
    {
      set => this.fieldID = value;
      get => this.fieldID;
    }

    public string CriterionName
    {
      get => this.criterionName;
      set => this.criterionName = value;
    }

    public string FieldDescription
    {
      set => this.fieldDescription = value;
      get => this.fieldDescription;
    }

    public bool ForceDataConversion
    {
      get => this.forceDataConversion;
      set => this.forceDataConversion = value;
    }

    public FilterDataSource DataSource
    {
      get => this.dataSource;
      set => this.dataSource = value;
    }

    public OperatorTypes OperatorType
    {
      get => this.opType;
      set => this.opType = FieldTypeUtilities.ConvertForFieldType(value, this.fieldType);
    }

    public string OperatorTypeAsString
    {
      get => FieldFilter.opTypeProvider.GetName((object) this.OperatorType);
    }

    public string ValueFrom
    {
      set => this.valueFrom = value;
      get => this.valueFrom;
    }

    public string ValueDescription
    {
      get
      {
        if (this.valueDescription == null)
          this.valueDescription = this.getDefaultValueDescription();
        return this.valueDescription;
      }
      set => this.valueDescription = value;
    }

    private string getDefaultValueDescription()
    {
      if (this.fieldType == FieldTypes.IsOptionList)
        return string.Join(", ", this.GetOptionsList());
      if (this.fieldType == FieldTypes.IsCheckbox)
        return "Checked";
      if (this.fieldType == FieldTypes.IsYesNo)
        return this.opType != OperatorTypes.IsYes ? "No" : "Yes";
      OperatorTypesEnumNameProvider enumNameProvider = new OperatorTypesEnumNameProvider();
      switch (enumNameProvider.GetParameterCount(this.opType))
      {
        case 1:
          return this.valueFrom;
        case 2:
          return this.valueFrom + " and " + this.valueTo;
        default:
          return enumNameProvider.GetName((object) this.opType);
      }
    }

    public string OperatorDescription
    {
      get
      {
        if (FieldFilter.opTypeProvider.GetParameterCount(this.OperatorType) == 0 && this.fieldType != FieldTypes.IsCheckbox || this.OperatorType == OperatorTypes.IsExact)
          return "Is";
        if (this.fieldType == FieldTypes.IsOptionList)
          return this.OperatorType == OperatorTypes.IsAnyOf ? (this.GetOptionsList().Length <= 1 ? "Is" : FieldFilter.opTypeProvider.GetName((object) this.OperatorType)) : (this.GetOptionsList().Length <= 1 ? "Is not" : FieldFilter.opTypeProvider.GetName((object) this.OperatorType));
        if (this.fieldType == FieldTypes.IsYesNo)
          return "Is";
        if (this.fieldType != FieldTypes.IsCheckbox)
          return FieldFilter.opTypeProvider.GetName((object) this.OperatorType);
        return this.OperatorType != OperatorTypes.IsChecked ? "Is not" : "Is";
      }
    }

    public bool IsVolatile
    {
      get => this.isVolatile;
      set => this.isVolatile = value;
    }

    public DateTime DateFrom
    {
      get
      {
        return this.fieldType != FieldTypes.IsDate && this.fieldType != FieldTypes.IsMonthDay && this.fieldType != FieldTypes.IsDateTime ? DateTime.MinValue : this.toDate(this.opType, true);
      }
    }

    public DateTime DateTo
    {
      get
      {
        return this.fieldType != FieldTypes.IsDate && this.fieldType != FieldTypes.IsMonthDay && this.fieldType != FieldTypes.IsDateTime ? DateTime.MinValue : this.toDate(this.opType, false);
      }
    }

    private DateTime parseDate(string value)
    {
      try
      {
        return Utils.ParseDate((object) value, true);
      }
      catch
      {
      }
      return Utils.ParseMonthDay((object) value, DateTime.MinValue);
    }

    private DateTime toDate(OperatorTypes dateType, bool beginDate)
    {
      DateTime today = DateTime.Today;
      DateTime dateValue = DateTime.MinValue;
      switch (dateType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
        case OperatorTypes.NotEqual:
        case OperatorTypes.GreaterThan:
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.LessThan:
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
        case OperatorTypes.DateOnOrBefore:
        case OperatorTypes.DateAfter:
        case OperatorTypes.DateBefore:
          dateValue = !beginDate ? DateTime.MinValue : this.parseDate(this.valueFrom);
          break;
        case OperatorTypes.Between:
        case OperatorTypes.DateBetween:
          dateValue = !beginDate ? this.parseDate(this.valueTo).AddDays(1.0) : this.parseDate(this.valueFrom);
          break;
        case OperatorTypes.CurrentWeek:
          if (beginDate)
          {
            int dayOfWeek = this.getDayOfWeek(today.DayOfWeek);
            dateValue = today.AddDays((double) dayOfWeek);
            break;
          }
          int dayOfWeek1 = this.getDayOfWeek(today.DayOfWeek);
          dateValue = today.AddDays((double) dayOfWeek1);
          dateValue = dateValue.AddDays(7.0);
          break;
        case OperatorTypes.CurrentMonth:
          if (beginDate)
          {
            int num = today.Month;
            string str1 = num.ToString("00");
            num = today.Year;
            string str2 = num.ToString("0000");
            dateValue = Utils.ParseDate((object) (str1 + "/01/" + str2));
            break;
          }
          int num1 = today.Month;
          string str3 = num1.ToString("00");
          num1 = today.Year;
          string str4 = num1.ToString("0000");
          dateValue = Utils.ParseDate((object) (str3 + "/01/" + str4));
          dateValue = dateValue.AddMonths(1);
          break;
        case OperatorTypes.YearToDate:
          dateValue = !beginDate ? today.AddDays(1.0) : Utils.ParseDate((object) ("01/01/" + today.Year.ToString("0000")));
          break;
        case OperatorTypes.PreviousWeek:
          if (beginDate)
          {
            int dayOfWeek2 = this.getDayOfWeek(today.DayOfWeek);
            dateValue = today.AddDays((double) (dayOfWeek2 - 7));
            break;
          }
          int dayOfWeek3 = this.getDayOfWeek(today.DayOfWeek);
          dateValue = today.AddDays((double) dayOfWeek3);
          break;
        case OperatorTypes.PreviousMonth:
          if (beginDate)
          {
            DateTime dateTime = today.AddMonths(-1);
            dateValue = Utils.ParseDate((object) (dateTime.Month.ToString("00") + "/01/" + dateTime.Year.ToString("0000")));
            break;
          }
          int num2 = today.Month;
          string str5 = num2.ToString("00");
          num2 = today.Year;
          string str6 = num2.ToString("0000");
          dateValue = Utils.ParseDate((object) (str5 + "/01/" + str6));
          break;
        case OperatorTypes.PreviousYear:
          dateValue = !beginDate ? Utils.ParseDate((object) ("01/01/" + today.Year.ToString("0000"))) : Utils.ParseDate((object) ("01/01/" + (today.Year - 1).ToString("0000")));
          break;
        case OperatorTypes.Last7Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-6.0);
          break;
        case OperatorTypes.Last30Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-29.0);
          break;
        case OperatorTypes.Last90Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-89.0);
          break;
        case OperatorTypes.Last365Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-364.0);
          break;
        case OperatorTypes.DateNotBetween:
          dateValue = !beginDate ? this.parseDate(this.valueTo).AddDays(1.0) : this.parseDate(this.valueFrom);
          break;
        case OperatorTypes.Today:
          dateValue = !beginDate ? today.AddDays(1.0) : today;
          break;
        case OperatorTypes.NextWeek:
          if (beginDate)
          {
            DateTime dateTime = today.AddDays(7.0);
            int dayOfWeek4 = this.getDayOfWeek(dateTime.DayOfWeek);
            dateValue = dateTime.AddDays((double) dayOfWeek4);
            break;
          }
          DateTime dateTime1 = today.AddDays(7.0);
          int dayOfWeek5 = this.getDayOfWeek(dateTime1.DayOfWeek);
          dateValue = dateTime1.AddDays((double) dayOfWeek5).AddDays(7.0);
          break;
        case OperatorTypes.NextMonth:
          if (beginDate)
          {
            DateTime dateTime2 = today.AddMonths(1);
            dateValue = DateTime.Parse(dateTime2.Month.ToString("00") + "/01/" + dateTime2.Year.ToString("0000"));
            break;
          }
          DateTime dateTime3 = today.AddMonths(1);
          dateValue = DateTime.Parse(dateTime3.Month.ToString("00") + "/01/" + dateTime3.Year.ToString("0000")).AddMonths(1);
          break;
        case OperatorTypes.NextYear:
          DateTime dateTime4 = today.AddYears(1);
          dateValue = !beginDate ? DateTime.Parse("01/01/" + dateTime4.AddYears(1).Year.ToString("0000")) : DateTime.Parse("01/01/" + dateTime4.Year.ToString("0000"));
          break;
        case OperatorTypes.Last15Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-14.0);
          break;
        case OperatorTypes.Last60Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-59.0);
          break;
        case OperatorTypes.Last180Days:
          dateValue = !beginDate ? today.AddDays(1.0) : today.AddDays(-179.0);
          break;
        case OperatorTypes.Next7Days:
          dateValue = !beginDate ? today.AddDays(8.0) : today.AddDays(1.0);
          break;
        case OperatorTypes.Next15Days:
          dateValue = !beginDate ? today.AddDays(16.0) : today.AddDays(1.0);
          break;
        case OperatorTypes.Next30Days:
          dateValue = !beginDate ? today.AddDays(31.0) : today.AddDays(1.0);
          break;
        case OperatorTypes.Next60Days:
          dateValue = !beginDate ? today.AddDays(61.0) : today.AddDays(1.0);
          break;
        case OperatorTypes.Next90Days:
          dateValue = !beginDate ? today.AddDays(91.0) : today.AddDays(1.0);
          break;
        case OperatorTypes.Next180Days:
          dateValue = !beginDate ? today.AddDays(181.0) : today.AddDays(1.0);
          break;
        case OperatorTypes.Next365Days:
          dateValue = !beginDate ? today.AddDays(366.0) : today.AddDays(1.0);
          break;
      }
      return this.normalizeDate(dateValue);
    }

    private DateTime normalizeDate(DateTime dateValue)
    {
      return dateValue == DateTime.MinValue || this.fieldType != FieldTypes.IsMonthDay ? dateValue : new DateTime(2000, dateValue.Month, dateValue.Day);
    }

    private int getDayOfWeek(DayOfWeek dow)
    {
      switch (dow)
      {
        case DayOfWeek.Sunday:
          return 0;
        case DayOfWeek.Monday:
          return -1;
        case DayOfWeek.Tuesday:
          return -2;
        case DayOfWeek.Wednesday:
          return -3;
        case DayOfWeek.Thursday:
          return -4;
        case DayOfWeek.Friday:
          return -5;
        case DayOfWeek.Saturday:
          return -6;
        default:
          return 0;
      }
    }

    public string ValueTo
    {
      set => this.valueTo = value;
      get => this.valueTo;
    }

    public JointTokens JointToken
    {
      set => this.jointToken = value;
      get => this.jointToken;
    }

    public string JointTokenToString
    {
      get
      {
        if (this.jointToken == JointTokens.And)
          return "and";
        return this.jointToken == JointTokens.Or ? "or" : "";
      }
    }

    public int LeftParentheses
    {
      set
      {
        this.leftParentheses = value;
        if (this.leftParentheses >= 0)
          return;
        this.leftParentheses = 0;
      }
      get => this.leftParentheses;
    }

    public string LeftParenthesesToString
    {
      get
      {
        string parenthesesToString = "";
        for (int index = 1; index <= this.leftParentheses; ++index)
          parenthesesToString += "(";
        return parenthesesToString;
      }
    }

    public int RightParentheses
    {
      set
      {
        this.rightParentheses = value;
        if (this.rightParentheses >= 0)
          return;
        this.rightParentheses = 0;
      }
      get => this.rightParentheses;
    }

    public string RightParenthesesToString
    {
      get
      {
        string parenthesesToString = "";
        for (int index = 1; index <= this.rightParentheses; ++index)
          parenthesesToString += ")";
        return parenthesesToString;
      }
    }

    public string[] GetOptionsList() => this.valueFrom.Split(';');

    public void SetOptionsList(string[] values) => this.valueFrom = string.Join(";", values);

    public void SetOptionsList(string[] values, string[] names)
    {
      this.SetOptionsList(values);
      this.valueDescription = string.Join(", ", names);
    }

    public object Clone() => (object) new FieldFilter((FieldFilter) this.MemberwiseClone());

    public string GetScriptCommands(string fieldValue)
    {
      bool flag;
      if (this.fieldType == FieldTypes.IsNumeric)
        flag = this.numericFilterPassed((double) Utils.ParseDecimal((object) fieldValue));
      else if (this.fieldType == FieldTypes.IsDate || this.fieldType == FieldTypes.IsMonthDay || this.fieldType == FieldTypes.IsDateTime)
      {
        if (!Utils.IsDate((object) fieldValue) || fieldValue == "/")
          flag = this.OperatorType == OperatorTypes.EmptyDate;
        else if (this.OperatorType == OperatorTypes.NotEmptyDate)
        {
          flag = true;
        }
        else
        {
          DateTime date = Utils.ParseDate((object) fieldValue);
          if (this.fieldType == FieldTypes.IsMonthDay)
            fieldValue = DateTime.Today.Year.ToString("0000") + "/" + date.Month.ToString("00") + "/" + date.Day.ToString("00");
          else
            fieldValue = date.ToString("yyyy/MM/dd");
          flag = this.dateFilterPassed(fieldValue);
        }
      }
      else
        flag = this.fieldType != FieldTypes.IsOptionList ? this.stringFilterPassed(fieldValue) : this.listFilterPassed(fieldValue);
      string str = "";
      if (this.leftParentheses > 0)
        str = str + this.LeftParenthesesToString + " ";
      string scriptCommands = !flag ? str + "False" : str + "True";
      if (this.rightParentheses > 0)
        scriptCommands = scriptCommands + this.RightParenthesesToString + " ";
      if (this.jointToken == JointTokens.And)
        scriptCommands += " And ";
      else if (this.JointToken == JointTokens.Or)
        scriptCommands += " Or ";
      return scriptCommands;
    }

    public bool Evaluate(object value) => this.Evaluate(value, FilterEvaluationOption.None);

    public bool Evaluate(object value, FilterEvaluationOption options)
    {
      if (this.isVolatile && (options & FilterEvaluationOption.NonVolatile) != FilterEvaluationOption.None)
        return true;
      try
      {
        switch (this.fieldType)
        {
          case FieldTypes.IsString:
          case FieldTypes.IsPhone:
            return this.stringFilterPassed(string.Concat(value));
          case FieldTypes.IsNumeric:
            return this.numericFilterPassed(Utils.ParseDouble((object) string.Concat(value)));
          case FieldTypes.IsDate:
          case FieldTypes.IsDateTime:
            return this.dateFilterPassed(string.Concat(value));
          case FieldTypes.IsMonthDay:
            return this.monthDayFilterPassed(string.Concat(value));
          case FieldTypes.IsOptionList:
            return this.listFilterPassed(string.Concat(value));
          case FieldTypes.IsYesNo:
            return this.yesNoFilterPassed(string.Concat(value));
          case FieldTypes.IsCheckbox:
            return this.checkboxFilterPassed(string.Concat(value));
          default:
            return false;
        }
      }
      catch
      {
        return false;
      }
    }

    private bool checkboxFilterPassed(string value)
    {
      return this.OperatorType == OperatorTypes.IsChecked ? value == "X" : value != "X";
    }

    private bool yesNoFilterPassed(string value)
    {
      return this.OperatorType == OperatorTypes.IsYes ? value == "Y" : value != "Y";
    }

    private bool numericFilterPassed(double doubleValue)
    {
      double num1 = (double) Utils.ParseDecimal((object) this.valueFrom);
      double num2 = (double) Utils.ParseDecimal((object) this.valueTo);
      bool flag = false;
      switch (this.opType)
      {
        case OperatorTypes.Equals:
          if (doubleValue == num1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.NotEqual:
          if (doubleValue != num1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.GreaterThan:
          if (doubleValue > num1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.NotGreaterThan:
          if (doubleValue <= num1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.LessThan:
          if (doubleValue < num1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.NotLessThan:
          if (doubleValue >= num1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.Between:
          if (doubleValue >= num1 && doubleValue <= num2)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.NotBetween:
          if (doubleValue < num1 || doubleValue > num2)
          {
            flag = true;
            break;
          }
          break;
      }
      return flag;
    }

    private bool stringFilterPassed(string stringValue)
    {
      if (stringValue == null)
        stringValue = "";
      bool flag = false;
      switch (this.opType)
      {
        case OperatorTypes.IsExact:
          if (stringValue.ToLower() == this.valueFrom.ToLower())
          {
            flag = true;
            break;
          }
          if (this.fieldID == "ContactGroup.GroupName" && (";" + stringValue.ToLower() + ";").IndexOf(";" + this.valueFrom.ToLower() + ";") > -1)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.IsNot:
          if (this.fieldID == "ContactGroup.GroupName")
          {
            if ((";" + stringValue.ToLower() + ";").IndexOf(";" + this.valueFrom.ToLower() + ";") == -1)
            {
              flag = true;
              break;
            }
            break;
          }
          if (stringValue.ToLower() != this.valueFrom.ToLower())
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.StartsWith:
          if (!(this.valueFrom == ""))
          {
            flag = stringValue.ToLower().StartsWith(this.valueFrom.ToLower());
            break;
          }
          goto case OperatorTypes.IsExact;
        case OperatorTypes.DoesnotStartWith:
          if (!(this.valueFrom == ""))
          {
            if (!stringValue.ToLower().StartsWith(this.valueFrom.ToLower()))
            {
              flag = true;
              break;
            }
            break;
          }
          goto case OperatorTypes.IsNot;
        case OperatorTypes.Contains:
          if ((stringValue == string.Empty || stringValue == null) && this.valueFrom == string.Empty)
          {
            flag = true;
            break;
          }
          if (stringValue.ToLower().IndexOf(this.valueFrom.ToLower()) != -1 && this.valueFrom != string.Empty && this.valueFrom != null)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.DoesnotContain:
          if (stringValue != string.Empty && stringValue != null && this.valueFrom == string.Empty)
          {
            flag = true;
            break;
          }
          if (stringValue.ToLower().IndexOf(this.valueFrom.ToLower()) == -1 && this.valueFrom != string.Empty && this.valueFrom != null)
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.IsYes:
          if (stringValue.ToLower() == "y" || stringValue.ToLower() == "yes")
          {
            flag = true;
            break;
          }
          break;
        case OperatorTypes.IsNotYes:
          if (stringValue.ToLower() != "y" && stringValue.ToLower() != "yes")
          {
            flag = true;
            break;
          }
          break;
      }
      return flag;
    }

    private bool listFilterPassed(string stringValue)
    {
      foreach (string options in this.GetOptionsList())
      {
        if (string.Compare(stringValue, options, true) == 0)
          return this.OperatorType == OperatorTypes.IsAnyOf || this.OperatorType == OperatorTypes.IsExact;
      }
      return this.OperatorType != OperatorTypes.IsAnyOf && this.OperatorType != OperatorTypes.IsExact;
    }

    private bool dateFilterPassed(string dateValue)
    {
      bool flag1 = Utils.IsDate((object) dateValue);
      if (this.opType == OperatorTypes.EmptyDate && !flag1)
        return true;
      if (this.opType == OperatorTypes.EmptyDate & flag1 || this.opType == OperatorTypes.NotEmptyDate && !flag1)
        return false;
      if (this.opType == OperatorTypes.NotEmptyDate & flag1)
        return true;
      DateTime dateTime1 = DateTime.MinValue;
      DateTime dateTime2 = DateTime.MinValue;
      DateTime today = DateTime.Today;
      switch (this.opType)
      {
        case OperatorTypes.Equals:
        case OperatorTypes.NotEqual:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom).Date;
          DateTime dateTime3 = Utils.ParseDate((object) this.valueFrom);
          dateTime3 = dateTime3.Date;
          dateTime3 = dateTime3.AddDays(1.0);
          dateTime2 = dateTime3.AddSeconds(-1.0);
          break;
        case OperatorTypes.GreaterThan:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          DateTime date1 = Utils.ParseDate((object) this.valueFrom);
          date1 = date1.Date;
          dateTime1 = date1.AddDays(1.0);
          dateTime2 = DateTime.MaxValue;
          break;
        case OperatorTypes.LessThan:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = DateTime.MinValue;
          DateTime date2 = Utils.ParseDate((object) this.valueFrom);
          date2 = date2.Date;
          dateTime2 = date2.AddSeconds(-1.0);
          break;
        case OperatorTypes.Between:
        case OperatorTypes.DateBetween:
          if (!Utils.IsDate((object) this.valueFrom) || !Utils.IsDate((object) this.valueTo))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom);
          dateTime2 = Utils.ParseDate((object) this.valueTo);
          break;
        case OperatorTypes.CurrentWeek:
          int dayOfWeek1 = this.getDayOfWeek(today.DayOfWeek);
          dateTime1 = today.AddDays((double) dayOfWeek1);
          dateTime2 = dateTime1.AddDays(6.0);
          break;
        case OperatorTypes.CurrentMonth:
          dateTime1 = Utils.ParseDate((object) (today.Month.ToString("00") + "/01/" + today.Year.ToString("0000")));
          dateTime2 = dateTime1.AddMonths(1);
          dateTime2 = dateTime2.AddDays(-1.0);
          break;
        case OperatorTypes.YearToDate:
          dateTime1 = DateTime.Parse("01/01/" + today.Year.ToString("0000"));
          dateTime2 = today;
          break;
        case OperatorTypes.PreviousWeek:
          DateTime dateTime4 = today.AddDays(-7.0);
          int dayOfWeek2 = this.getDayOfWeek(dateTime4.DayOfWeek);
          DateTime dateTime5 = dateTime4.AddDays((double) dayOfWeek2);
          dateTime1 = dateTime5;
          dateTime2 = dateTime5.AddDays(6.0);
          break;
        case OperatorTypes.PreviousMonth:
          DateTime dateTime6 = today.AddMonths(-1);
          dateTime1 = DateTime.Parse(dateTime6.Month.ToString("00") + "/01/" + dateTime6.Year.ToString("0000"));
          DateTime dateTime7 = dateTime1.AddMonths(1).AddDays(-1.0);
          dateTime2 = DateTime.Parse(dateTime7.Month.ToString("00") + "/" + dateTime7.Day.ToString("00") + "/" + dateTime7.Year.ToString("0000"));
          break;
        case OperatorTypes.PreviousYear:
          DateTime dateTime8 = today.AddYears(-1);
          int year1 = dateTime8.Year;
          dateTime1 = DateTime.Parse("01/01/" + year1.ToString("0000"));
          year1 = dateTime8.Year;
          dateTime2 = DateTime.Parse("12/31/" + year1.ToString("0000"));
          break;
        case OperatorTypes.Last7Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-6.0);
          break;
        case OperatorTypes.Last30Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-29.0);
          break;
        case OperatorTypes.Last90Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-89.0);
          break;
        case OperatorTypes.Last365Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-364.0);
          break;
        case OperatorTypes.DateNotBetween:
          if (!Utils.IsDate((object) this.valueFrom) || !Utils.IsDate((object) this.valueTo))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom);
          dateTime2 = Utils.ParseDate((object) this.valueTo);
          break;
        case OperatorTypes.DateOnOrAfter:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom);
          dateTime2 = DateTime.MaxValue;
          break;
        case OperatorTypes.DateOnOrBefore:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = DateTime.MinValue;
          DateTime dateTime9 = Utils.ParseDate((object) this.valueFrom);
          dateTime9 = dateTime9.Date;
          dateTime9 = dateTime9.AddDays(1.0);
          dateTime2 = dateTime9.AddSeconds(-1.0);
          break;
        case OperatorTypes.Today:
          dateTime1 = today;
          dateTime2 = today.AddDays(1.0).AddSeconds(-1.0);
          break;
        case OperatorTypes.NextWeek:
          DateTime dateTime10 = today.AddDays(7.0);
          int dayOfWeek3 = this.getDayOfWeek(dateTime10.DayOfWeek);
          DateTime dateTime11 = dateTime10.AddDays((double) -dayOfWeek3);
          dateTime1 = dateTime11;
          dateTime2 = dateTime11.AddDays(6.0);
          break;
        case OperatorTypes.NextMonth:
          DateTime dateTime12 = today.AddMonths(1);
          dateTime1 = DateTime.Parse(dateTime12.Month.ToString("00") + "/01/" + dateTime12.Year.ToString("0000"));
          DateTime dateTime13 = dateTime1.AddMonths(1).AddDays(-1.0);
          dateTime2 = DateTime.Parse(dateTime13.Month.ToString("00") + "/" + dateTime13.Day.ToString("00") + "/" + dateTime13.Year.ToString("0000"));
          break;
        case OperatorTypes.NextYear:
          DateTime dateTime14 = today.AddYears(1);
          int year2 = dateTime14.Year;
          dateTime1 = DateTime.Parse("01/01/" + year2.ToString("0000"));
          year2 = dateTime14.Year;
          dateTime2 = DateTime.Parse("12/31/" + year2.ToString("0000"));
          break;
        case OperatorTypes.Last15Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-15.0);
          break;
        case OperatorTypes.Last60Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-59.0);
          break;
        case OperatorTypes.Last180Days:
          dateTime2 = today;
          dateTime1 = today.AddDays(-179.0);
          break;
        case OperatorTypes.Next7Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(6.0);
          break;
        case OperatorTypes.Next15Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(14.0);
          break;
        case OperatorTypes.Next30Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(29.0);
          break;
        case OperatorTypes.Next60Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(59.0);
          break;
        case OperatorTypes.Next90Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(89.0);
          break;
        case OperatorTypes.Next180Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(179.0);
          break;
        case OperatorTypes.Next365Days:
          dateTime1 = today.AddDays(1.0);
          dateTime2 = dateTime1.AddDays(364.0);
          break;
        case OperatorTypes.DateAfter:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          DateTime date3 = Utils.ParseDate((object) this.valueFrom);
          date3 = date3.Date;
          dateTime1 = date3.AddDays(1.0);
          dateTime2 = DateTime.MaxValue;
          break;
        case OperatorTypes.DateBefore:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = DateTime.MinValue;
          DateTime date4 = Utils.ParseDate((object) this.valueFrom);
          date4 = date4.Date;
          dateTime2 = date4.AddSeconds(-1.0);
          break;
      }
      DateTime date5 = Utils.ParseDate((object) dateValue);
      bool flag2 = false;
      if (dateTime1 <= date5 && date5 <= dateTime2)
        flag2 = true;
      if (this.opType.ToString().IndexOf("Not") > -1)
        flag2 = !flag2;
      return flag2;
    }

    private bool monthDayFilterPassed(string dateValue)
    {
      if (!Utils.IsMonthDay((object) dateValue))
        return this.opType == OperatorTypes.EmptyDate;
      if (this.opType == OperatorTypes.EmptyDate)
        return false;
      if (this.opType == OperatorTypes.NotEmptyDate)
        return true;
      DateTime dateTime1 = DateTime.MinValue;
      DateTime dateTime2 = DateTime.MinValue;
      DateTime dateTime3 = DateTime.Today;
      DateTime date1 = Utils.ParseDate((object) dateValue).Date;
      string[] strArray1 = new string[5];
      int num = date1.Month;
      strArray1[0] = num.ToString("00");
      strArray1[1] = "/";
      num = date1.Day;
      strArray1[2] = num.ToString("00");
      strArray1[3] = "/";
      num = dateTime3.Year;
      strArray1[4] = num.ToString("0000");
      DateTime dateTime4 = DateTime.Parse(string.Concat(strArray1));
      switch (this.opType)
      {
        case OperatorTypes.Equals:
        case OperatorTypes.NotEqual:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom);
          dateTime2 = Utils.ParseDate((object) this.valueFrom);
          break;
        case OperatorTypes.GreaterThan:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          DateTime date2 = Utils.ParseDate((object) this.valueFrom);
          date2 = date2.Date;
          dateTime1 = date2.AddDays(1.0);
          dateTime2 = DateTime.MaxValue;
          break;
        case OperatorTypes.LessThan:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = DateTime.MinValue;
          DateTime date3 = Utils.ParseDate((object) this.valueFrom);
          date3 = date3.Date;
          dateTime2 = date3.AddSeconds(-1.0);
          break;
        case OperatorTypes.Between:
        case OperatorTypes.DateBetween:
        case OperatorTypes.DateNotBetween:
          if (!Utils.IsDate((object) this.valueFrom) || !Utils.IsDate((object) this.valueTo))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom);
          dateTime2 = Utils.ParseDate((object) this.valueTo);
          break;
        case OperatorTypes.CurrentWeek:
          int dayOfWeek1 = this.getDayOfWeek(dateTime3.DayOfWeek);
          dateTime1 = dateTime3.AddDays((double) dayOfWeek1);
          dateTime2 = dateTime1.AddDays(6.0);
          break;
        case OperatorTypes.CurrentMonth:
          num = dateTime3.Month;
          string str1 = num.ToString("00");
          num = dateTime3.Year;
          string str2 = num.ToString("0000");
          dateTime1 = Utils.ParseDate((object) (str1 + "/01/" + str2));
          dateTime2 = dateTime1.AddMonths(1).AddDays(-1.0);
          break;
        case OperatorTypes.PreviousWeek:
          dateTime3 = dateTime3.AddDays(-7.0);
          int dayOfWeek2 = this.getDayOfWeek(dateTime3.DayOfWeek);
          dateTime3 = dateTime3.AddDays((double) dayOfWeek2);
          dateTime1 = dateTime3;
          dateTime2 = dateTime3.AddDays(6.0);
          break;
        case OperatorTypes.PreviousMonth:
          dateTime3 = dateTime3.AddMonths(-1);
          num = dateTime3.Month;
          string str3 = num.ToString("00");
          num = dateTime3.Year;
          string str4 = num.ToString("0000");
          dateTime1 = DateTime.Parse(str3 + "/01/" + str4);
          dateTime3 = dateTime1.AddMonths(1).AddDays(-1.0);
          string[] strArray2 = new string[5];
          num = dateTime3.Month;
          strArray2[0] = num.ToString("00");
          strArray2[1] = "/";
          num = dateTime3.Day;
          strArray2[2] = num.ToString("00");
          strArray2[3] = "/";
          num = dateTime3.Year;
          strArray2[4] = num.ToString("0000");
          dateTime2 = DateTime.Parse(string.Concat(strArray2));
          break;
        case OperatorTypes.DateOnOrAfter:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom);
          dateTime2 = DateTime.MaxValue;
          break;
        case OperatorTypes.DateOnOrBefore:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = DateTime.MinValue;
          dateTime2 = Utils.ParseDate((object) this.valueFrom);
          break;
        case OperatorTypes.DateAfter:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = Utils.ParseDate((object) this.valueFrom).AddDays(1.0);
          dateTime2 = DateTime.MaxValue;
          break;
        case OperatorTypes.DateBefore:
          if (!Utils.IsDate((object) this.valueFrom))
            return false;
          dateTime1 = DateTime.MinValue;
          dateTime2 = Utils.ParseDate((object) this.valueFrom).AddDays(-1.0);
          break;
      }
      string[] strArray3 = new string[5];
      num = dateTime1.Month;
      strArray3[0] = num.ToString("00");
      strArray3[1] = "/";
      num = dateTime1.Day;
      strArray3[2] = num.ToString("00");
      strArray3[3] = "/";
      num = dateTime3.Year;
      strArray3[4] = num.ToString("0000");
      dateTime1 = DateTime.Parse(string.Concat(strArray3));
      string[] strArray4 = new string[5];
      num = dateTime2.Month;
      strArray4[0] = num.ToString("00");
      strArray4[1] = "/";
      num = dateTime2.Day;
      strArray4[2] = num.ToString("00");
      strArray4[3] = "/";
      num = dateTime3.Year;
      strArray4[4] = num.ToString("0000");
      DateTime dateTime5 = DateTime.Parse(string.Concat(strArray4));
      bool flag = false;
      if (dateTime1 <= dateTime4 && dateTime4 <= dateTime5)
        flag = true;
      if (this.opType.ToString().IndexOf("Not") > -1)
        flag = !flag;
      return flag;
    }

    public string GetSimpleString(DisplayTypes dtype)
    {
      string simpleString = "";
      if (dtype == DisplayTypes.AllAndFieldID)
        simpleString = simpleString + "[" + this.fieldID + "]";
      else if (dtype == DisplayTypes.DisplayField || dtype == DisplayTypes.All)
        simpleString = !(this.fieldDescription == string.Empty) ? simpleString + this.fieldDescription : simpleString + this.fieldID;
      if (dtype == DisplayTypes.DisplayField)
        return simpleString;
      string str1 = "";
      string str2 = "";
      if (dtype == DisplayTypes.DisplayValue || dtype == DisplayTypes.All || dtype == DisplayTypes.AllAndFieldID)
      {
        switch (this.opType)
        {
          case OperatorTypes.IsExact:
          case OperatorTypes.IsNot:
          case OperatorTypes.StartsWith:
          case OperatorTypes.DoesnotStartWith:
          case OperatorTypes.Contains:
          case OperatorTypes.DoesnotContain:
          case OperatorTypes.Equals:
          case OperatorTypes.NotEqual:
          case OperatorTypes.GreaterThan:
          case OperatorTypes.NotGreaterThan:
          case OperatorTypes.LessThan:
          case OperatorTypes.NotLessThan:
          case OperatorTypes.DateOnOrAfter:
          case OperatorTypes.DateOnOrBefore:
          case OperatorTypes.IsAnyOf:
          case OperatorTypes.IsNotAnyOf:
          case OperatorTypes.DateAfter:
          case OperatorTypes.DateBefore:
            str1 = this.valueFrom;
            break;
          case OperatorTypes.Between:
          case OperatorTypes.NotBetween:
          case OperatorTypes.DateBetween:
          case OperatorTypes.DateNotBetween:
            str1 = this.valueFrom;
            str2 = this.valueTo;
            break;
          default:
            str1 = new OperatorTypesEnumNameProvider().GetName((object) this.opType);
            break;
        }
        if (str1 == "")
          str1 = "Nothing";
        if (str2 == "")
          str2 = "Nothing";
        if (dtype == DisplayTypes.DisplayValue)
          return this.opType.ToString().ToLower().IndexOf("between") > -1 ? str1 + " and " + str2 : str1;
      }
      string str3;
      string str4;
      switch (this.opType)
      {
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          str3 = simpleString + " <> " + str1;
          str4 = "<>";
          break;
        case OperatorTypes.StartsWith:
          str3 = simpleString + " starts with \"" + str1 + "\"";
          str4 = "starts with";
          break;
        case OperatorTypes.DoesnotStartWith:
          str3 = simpleString + " does not start with \"" + str1 + "\"";
          str4 = "does not start with";
          break;
        case OperatorTypes.Contains:
          str3 = simpleString + " contains \"" + str1 + "\"";
          str4 = "contains";
          break;
        case OperatorTypes.DoesnotContain:
          str3 = simpleString + " does not contain \"" + str1 + "\"";
          str4 = "does not contain";
          break;
        case OperatorTypes.GreaterThan:
          str3 = simpleString + " > " + str1;
          str4 = ">";
          break;
        case OperatorTypes.NotGreaterThan:
          str3 = simpleString + " <= " + str1;
          str4 = "<=";
          break;
        case OperatorTypes.LessThan:
          str3 = simpleString + " < " + str1;
          str4 = "<";
          break;
        case OperatorTypes.NotLessThan:
          str3 = simpleString + " >= " + str1;
          str4 = ">=";
          break;
        case OperatorTypes.Between:
        case OperatorTypes.DateBetween:
          str3 = simpleString + " between " + str1 + " and " + str2;
          str4 = "between";
          break;
        case OperatorTypes.NotBetween:
        case OperatorTypes.DateNotBetween:
          str3 = simpleString + " not between " + str1 + " and " + str2;
          str4 = "not between";
          break;
        case OperatorTypes.DateOnOrAfter:
          str3 = simpleString + " >= " + str1;
          str4 = "on or after";
          break;
        case OperatorTypes.DateOnOrBefore:
          str3 = simpleString + " <= " + str1;
          str4 = "on or before";
          break;
        case OperatorTypes.IsAnyOf:
          str3 = this.GetOptionsList().Length != 1 ? simpleString + " is any of " + str1 : simpleString + " = " + str1;
          str4 = "is any of";
          break;
        case OperatorTypes.IsNotAnyOf:
          str3 = this.GetOptionsList().Length != 1 ? simpleString + " is not any of (" + str1 + ")" : simpleString + " <> " + str1;
          str4 = "is not any of";
          break;
        case OperatorTypes.IsYes:
          str3 = simpleString + " = Yes";
          str4 = "=";
          break;
        case OperatorTypes.IsNotYes:
          str3 = simpleString + " = No";
          str4 = "=";
          break;
        case OperatorTypes.IsChecked:
          str3 = simpleString + " is Checked";
          str4 = "is";
          break;
        case OperatorTypes.IsNotChecked:
          str3 = simpleString + " is Unchecked";
          str4 = "is";
          break;
        case OperatorTypes.DateAfter:
          str3 = simpleString + " > " + str1;
          str4 = "after";
          break;
        case OperatorTypes.DateBefore:
          str3 = simpleString + " < " + str1;
          str4 = "before";
          break;
        default:
          str3 = simpleString + " = " + str1;
          str4 = "=";
          break;
      }
      return dtype == DisplayTypes.DisplayOperator ? str4 : str3;
    }

    public QueryCriterion ToQueryCriterion() => this.ToQueryCriterion(FilterEvaluationOption.None);

    public QueryCriterion ToQueryCriterion(FilterEvaluationOption options)
    {
      if (this.isVolatile && (options & FilterEvaluationOption.NonVolatile) != FilterEvaluationOption.None)
        return (QueryCriterion) null;
      switch (this.fieldType)
      {
        case FieldTypes.IsString:
        case FieldTypes.IsPhone:
          return this.toStringValueCriteria();
        case FieldTypes.IsNumeric:
          return this.toOrdinalValueCriteria();
        case FieldTypes.IsDate:
        case FieldTypes.IsDateTime:
          return this.toDateValueCriteria();
        case FieldTypes.IsMonthDay:
          return this.toMonthDayValueCriteria();
        case FieldTypes.IsOptionList:
          return this.toOptionListCriteria();
        case FieldTypes.IsYesNo:
          return this.toYesNoCriteria();
        case FieldTypes.IsCheckbox:
          return this.toCheckboxCriteria();
        default:
          throw new Exception("Invalid field type specified");
      }
    }

    private QueryCriterion toMonthDayValueCriteria()
    {
      QueryCriterion queryCriterion = (QueryCriterion) new StringValueCriterion(this.criterionName, "/", StringMatchType.Exact, false);
      switch (this.opType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.Equals, DateMatchPrecision.Recurring, this.forceDataConversion);
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.NotEquals, DateMatchPrecision.Recurring, this.forceDataConversion);
        case OperatorTypes.GreaterThan:
        case OperatorTypes.DateAfter:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.GreaterThan, DateMatchPrecision.Recurring, this.forceDataConversion);
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.DateOnOrBefore:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Recurring, this.forceDataConversion);
        case OperatorTypes.LessThan:
        case OperatorTypes.DateBefore:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.LessThan, DateMatchPrecision.Recurring, this.forceDataConversion);
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Recurring, this.forceDataConversion);
        case OperatorTypes.Between:
        case OperatorTypes.CurrentWeek:
        case OperatorTypes.CurrentMonth:
        case OperatorTypes.PreviousWeek:
        case OperatorTypes.PreviousMonth:
        case OperatorTypes.Last7Days:
        case OperatorTypes.Last30Days:
        case OperatorTypes.DateBetween:
        case OperatorTypes.NextWeek:
        case OperatorTypes.NextMonth:
          return this.DateFrom < this.DateTo ? new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Recurring, this.forceDataConversion).And((QueryCriterion) new DateValueCriterion(this.criterionName, this.DateTo, OrdinalMatchType.LessThan, DateMatchPrecision.Recurring, this.forceDataConversion)) : new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Recurring, this.forceDataConversion).Or((QueryCriterion) new DateValueCriterion(this.criterionName, this.DateTo, OrdinalMatchType.LessThan, DateMatchPrecision.Recurring, this.forceDataConversion));
        case OperatorTypes.DateNotBetween:
          return new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.LessThan, DateMatchPrecision.Recurring, this.forceDataConversion).Or((QueryCriterion) new DateValueCriterion(this.criterionName, this.DateTo, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Recurring, this.forceDataConversion));
        case OperatorTypes.EmptyDate:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, DateValueCriterion.NullValue, OrdinalMatchType.Equals, this.forceDataConversion);
        case OperatorTypes.NotEmptyDate:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, DateValueCriterion.NonNullValue, OrdinalMatchType.Equals, this.forceDataConversion);
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for a string field");
      }
    }

    private QueryCriterion toDateValueCriteria()
    {
      switch (this.opType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.Equals, DateMatchPrecision.Day, this.forceDataConversion);
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.NotEquals, DateMatchPrecision.Day, this.forceDataConversion);
        case OperatorTypes.GreaterThan:
        case OperatorTypes.DateAfter:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom.Date.AddDays(1.0), OrdinalMatchType.GreaterThanOrEquals, this.forceDataConversion);
        case OperatorTypes.NotGreaterThan:
        case OperatorTypes.DateOnOrBefore:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom.Date.AddDays(1.0), OrdinalMatchType.LessThan, this.forceDataConversion);
        case OperatorTypes.LessThan:
        case OperatorTypes.DateBefore:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.LessThan, this.forceDataConversion);
        case OperatorTypes.NotLessThan:
        case OperatorTypes.DateOnOrAfter:
          return (QueryCriterion) new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.GreaterThanOrEquals, this.forceDataConversion);
        case OperatorTypes.Between:
        case OperatorTypes.CurrentWeek:
        case OperatorTypes.CurrentMonth:
        case OperatorTypes.YearToDate:
        case OperatorTypes.PreviousWeek:
        case OperatorTypes.PreviousMonth:
        case OperatorTypes.PreviousYear:
        case OperatorTypes.Last7Days:
        case OperatorTypes.Last30Days:
        case OperatorTypes.Last90Days:
        case OperatorTypes.Last365Days:
        case OperatorTypes.DateBetween:
        case OperatorTypes.Today:
        case OperatorTypes.NextWeek:
        case OperatorTypes.NextMonth:
        case OperatorTypes.NextYear:
        case OperatorTypes.Last15Days:
        case OperatorTypes.Last60Days:
        case OperatorTypes.Last180Days:
        case OperatorTypes.Next7Days:
        case OperatorTypes.Next15Days:
        case OperatorTypes.Next30Days:
        case OperatorTypes.Next60Days:
        case OperatorTypes.Next90Days:
        case OperatorTypes.Next180Days:
        case OperatorTypes.Next365Days:
          return new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.GreaterThanOrEquals, this.forceDataConversion).And((QueryCriterion) new DateValueCriterion(this.criterionName, this.DateTo, OrdinalMatchType.LessThan, this.forceDataConversion));
        case OperatorTypes.DateNotBetween:
          return new DateValueCriterion(this.criterionName, this.DateFrom, OrdinalMatchType.LessThan, this.forceDataConversion).Or((QueryCriterion) new DateValueCriterion(this.criterionName, this.DateTo, OrdinalMatchType.GreaterThanOrEquals, this.forceDataConversion));
        case OperatorTypes.EmptyDate:
          return (QueryCriterion) new NullValueCriterion(this.criterionName, true);
        case OperatorTypes.NotEmptyDate:
          return (QueryCriterion) new NullValueCriterion(this.criterionName, false);
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for a date field");
      }
    }

    private QueryCriterion toYesNoCriteria()
    {
      switch (this.opType)
      {
        case OperatorTypes.IsYes:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, "Y");
        case OperatorTypes.IsNotYes:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, "Y", StringMatchType.Exact, false);
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for a Yes/No field");
      }
    }

    private QueryCriterion toCheckboxCriteria()
    {
      switch (this.opType)
      {
        case OperatorTypes.IsChecked:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, "X");
        case OperatorTypes.IsNotChecked:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, "X", StringMatchType.Exact, false);
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for a Checkbox field");
      }
    }

    private QueryCriterion toOptionListCriteria()
    {
      string[] optionsList = this.GetOptionsList();
      switch (this.opType)
      {
        case OperatorTypes.IsAnyOf:
          return (QueryCriterion) new ListValueCriterion(this.criterionName, (Array) optionsList);
        case OperatorTypes.IsNotAnyOf:
          return (QueryCriterion) new ListValueCriterion(this.criterionName, (Array) optionsList, false);
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for an option field");
      }
    }

    private QueryCriterion toStringValueCriteria()
    {
      switch (this.opType)
      {
        case OperatorTypes.IsExact:
        case OperatorTypes.Equals:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, this.valueFrom, StringMatchType.CaseInsensitive);
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, this.valueFrom, StringMatchType.CaseInsensitive, false);
        case OperatorTypes.StartsWith:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, this.valueFrom, StringMatchType.StartsWith);
        case OperatorTypes.DoesnotStartWith:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, this.valueFrom, StringMatchType.StartsWith, false);
        case OperatorTypes.Contains:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, this.valueFrom, StringMatchType.Contains);
        case OperatorTypes.DoesnotContain:
          return (QueryCriterion) new StringValueCriterion(this.criterionName, this.valueFrom, StringMatchType.Contains, false);
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for a string field");
      }
    }

    private QueryCriterion toOrdinalValueCriteria()
    {
      double num1 = (double) Utils.ParseDecimal((object) this.valueFrom);
      double num2 = (double) Utils.ParseDecimal((object) this.valueTo);
      switch (this.opType)
      {
        case OperatorTypes.IsNot:
        case OperatorTypes.NotEqual:
          return (QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.NotEquals, this.forceDataConversion);
        case OperatorTypes.Equals:
          return (QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.Equals, this.forceDataConversion);
        case OperatorTypes.GreaterThan:
          return (QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.GreaterThan, this.forceDataConversion);
        case OperatorTypes.NotGreaterThan:
          return (QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.LessThanOrEquals, this.forceDataConversion);
        case OperatorTypes.LessThan:
          return (QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.LessThan, this.forceDataConversion);
        case OperatorTypes.NotLessThan:
          return (QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.GreaterThanOrEquals, this.forceDataConversion);
        case OperatorTypes.Between:
          return new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.GreaterThanOrEquals, this.forceDataConversion).And((QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num2, OrdinalMatchType.LessThanOrEquals, this.forceDataConversion));
        case OperatorTypes.NotBetween:
          return new OrdinalValueCriterion(this.criterionName, (object) num1, OrdinalMatchType.LessThan, this.forceDataConversion).Or((QueryCriterion) new OrdinalValueCriterion(this.criterionName, (object) num2, OrdinalMatchType.GreaterThan, this.forceDataConversion));
        default:
          throw new Exception("The specified operator (" + (object) this.opType + ") is invalid for a numeric field");
      }
    }

    private object toEnum(string value, Type enumType)
    {
      try
      {
        return Enum.Parse(enumType, value, true);
      }
      catch
      {
        return (object) 0;
      }
    }
  }
}
