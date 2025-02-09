// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ContactSearchUtilBase
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public abstract class ContactSearchUtilBase
  {
    protected string description = string.Empty;
    protected ArrayList criteria = new ArrayList();
    protected RelatedLoanMatchType loanMatchType;
    protected ContactType contactType;

    public ContactSearchUtilBase()
    {
    }

    public ContactSearchUtilBase(RelatedLoanMatchType loanMatchType, ContactType contactType)
    {
      this.loanMatchType = loanMatchType;
      this.contactType = contactType;
    }

    public abstract ContactCustomFieldInfoCollection getCustomFields(ContactType contactType);

    public abstract Hashtable getCategoryIdToNameTable();

    public abstract UserInfo getContactOwner(string userId);

    [CLSCompliant(false)]
    public string Description => this.description;

    [CLSCompliant(false)]
    public ArrayList Criteria => this.criteria;

    [CLSCompliant(false)]
    public RelatedLoanMatchType LoanMatchType
    {
      get => this.loanMatchType;
      set => this.loanMatchType = value;
    }

    public static string GetDescription(ContactQuery query, ContactType contactType)
    {
      string description = "";
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      foreach (ContactQueryItem contactQueryItem in query.Items)
      {
        switch (contactQueryItem.GroupName)
        {
          case "LoanInfo":
            ++num1;
            break;
          case "ContactInfo":
            ++num2;
            break;
          case "MoreChoices":
            ++num3;
            break;
        }
      }
      string str1 = contactType == ContactType.Borrower ? "borrowers" : "business contacts";
      switch (query.LoanMatchType)
      {
        case RelatedLoanMatchType.None:
          description = "Showing " + str1 + " whose\n";
          break;
        case RelatedLoanMatchType.AnyClosed:
          description = "Showing " + str1 + " whose closed loans have\n";
          break;
        case RelatedLoanMatchType.LastClosed:
          description = "Showing " + str1 + " whose last closed loan has\n";
          break;
        case RelatedLoanMatchType.AnyOriginated:
          description = "Showing " + str1 + " whose originated loans have\n";
          break;
        case RelatedLoanMatchType.LastOriginated:
          description = "Showing " + str1 + " whose last originated loan has\n";
          break;
      }
      if (num1 == 0)
      {
        if (query.LoanMatchType == RelatedLoanMatchType.LastClosed | query.LoanMatchType == RelatedLoanMatchType.AnyClosed)
        {
          string str2 = "Showing " + (object) contactType + " with at least one closed loan";
          description = num2 + num3 <= 0 ? str2 + "\n" : str2 + " and whose\n";
        }
        else if (query.LoanMatchType == RelatedLoanMatchType.LastOriginated | query.LoanMatchType == RelatedLoanMatchType.AnyOriginated)
        {
          string str3 = "Showing " + (object) contactType + " with at least one originated loan";
          description = num2 + num3 <= 0 ? str3 + "\n" : str3 + " and whose\n";
        }
      }
      return description;
    }

    private string translateQueryItemValue1(ContactQueryItem item)
    {
      string nameInLoan = item.Value1;
      switch (item.FieldName)
      {
        case "History.ContactSource":
          nameInLoan = ContactSourceEnumUtil.NameToValue(item.Value1).ToString();
          break;
        case "RelatedLoan.Amortization":
          nameInLoan = AmortizationTypeEnumUtil.ValueToNameInLoan(AmortizationTypeEnumUtil.NameInContactSearchToValue(item.Value1));
          break;
        case "RelatedLoan.Purpose":
          nameInLoan = LoanPurposeEnumUtil.ValueToNameInLoan(LoanPurposeEnumUtil.NameInContactSearchToValue(item.Value1));
          break;
        case "RelatedLoan.LoanType":
          nameInLoan = LoanTypeEnumUtil.ValueToNameInLoan(LoanTypeEnumUtil.NameInContactSearchToValue(item.Value1));
          break;
      }
      return nameInLoan;
    }

    public void FlushSearchObjectsToSql(ContactQuery query, ContactType contactType)
    {
      if (query.Items == null)
        return;
      ContactCustomFieldInfo[] items = this.getCustomFields(contactType).Items;
      Hashtable hashtable = new Hashtable(items.Length);
      for (int index = 0; index < items.Length; ++index)
        hashtable.Add((object) items[index].Label, (object) items[index].FieldType);
      foreach (ContactQueryItem contactQueryItem in query.Items)
      {
        string condition = contactQueryItem.Condition;
        if (contactQueryItem.FieldName == "RelatedLoan.ClosedDate")
        {
          contactQueryItem.FieldName = "RelatedLoan.DateCompleted";
          contactQueryItem.FieldDisplayName = "Date Completed";
        }
        if (contactQueryItem.FieldName == "RelatedLoan.DateStarted")
          contactQueryItem.FieldName = "RelatedLoan.DateCompleted";
        bool bIsCustomMonthDay = false;
        if (contactQueryItem.FieldName.StartsWith("Custom") && hashtable.Contains((object) contactQueryItem.FieldDisplayName) && (FieldFormat) hashtable[(object) contactQueryItem.FieldDisplayName] == FieldFormat.MONTHDAY)
          bIsCustomMonthDay = true;
        if (contactQueryItem.ValueType == "System.DateTime")
          this.AddDateValueQuery(contactQueryItem.FieldDisplayName, contactQueryItem.FieldName, condition, contactQueryItem.Value1, contactQueryItem.Value2, bIsCustomMonthDay);
        else if (contactQueryItem.ValueType == "System.String")
        {
          string str = this.translateQueryItemValue1(contactQueryItem);
          this.AddStringValueQuery(contactQueryItem.FieldDisplayName, contactQueryItem.FieldName, condition, str, this.getStringMatchTypeFromString(contactQueryItem.Condition), contactQueryItem.Value1);
        }
        else if (contactQueryItem.ValueType == "EllieMae.EMLite.Common.FieldFormat.X")
          this.AddXValueQuery(contactQueryItem.FieldDisplayName, contactQueryItem.FieldName, contactQueryItem.Value1, this.getStringMatchTypeFromString(contactQueryItem.Condition));
        else
          this.AddOrdinalValueQuery(contactQueryItem.FieldDisplayName, contactQueryItem.FieldName, condition, contactQueryItem.Value1, contactQueryItem.Value2, Type.GetType(contactQueryItem.ValueType));
      }
    }

    protected void AddOrdinalValueQuery(
      string name,
      string fieldName,
      string condition,
      string value1,
      string value2,
      Type valueType)
    {
      if (value1 == string.Empty && valueType != Type.GetType("System.Boolean") || condition == "Between" && value2 == string.Empty)
        return;
      int count = this.criteria.Count;
      object key;
      if (valueType == Type.GetType("System.Boolean"))
      {
        key = !(value1.ToLower() == "x") ? (object) 0 : (object) 1;
        valueType = Type.GetType("System.Int32");
      }
      else
        key = Convert.ChangeType((object) value1, valueType);
      if (name == "Category")
      {
        Hashtable categoryIdToNameTable = this.getCategoryIdToNameTable();
        if (!categoryIdToNameTable.Contains(key))
          return;
        value1 = (string) categoryIdToNameTable[key];
      }
      string str;
      switch (condition)
      {
        case "Is":
          this.criteria.Add((object) new OrdinalValueCriterion(fieldName, key, OrdinalMatchType.Equals));
          str = name + " is " + value1;
          break;
        case "Before":
        case "Less Than":
          this.criteria.Add((object) new OrdinalValueCriterion(fieldName, key, OrdinalMatchType.LessThan));
          str = name + " is " + condition.ToLower() + " " + value1;
          break;
        case "After":
        case "Greater Than":
          this.criteria.Add((object) new OrdinalValueCriterion(fieldName, key, OrdinalMatchType.GreaterThan));
          str = name + " is " + condition.ToLower() + " " + value1;
          break;
        default:
          object obj = Convert.ChangeType((object) value2, valueType);
          this.criteria.Add((object) new OrdinalValueCriterion(fieldName, key, OrdinalMatchType.GreaterThan));
          this.criteria.Add((object) new OrdinalValueCriterion(fieldName, obj, OrdinalMatchType.LessThan));
          str = name + " is between " + value1 + " and " + value2;
          break;
      }
      this.description = this.description + (count == 0 ? " " : " and ") + str;
    }

    private string addCustomDateValueQuery(
      string name,
      string fieldName,
      string condition,
      string value1,
      string value2,
      bool bIsCustomMonthDay,
      DateMatchPrecision precision)
    {
      DateTime today1 = DateTime.Today;
      DateTime today2 = DateTime.Today;
      if (value1 == string.Empty)
        return (string) null;
      DateTime dateTime1 = DateTime.Parse(value1);
      if (condition == "Between")
      {
        if (value2 == string.Empty)
          return (string) null;
        today2 = DateTime.Parse(value2);
      }
      string str;
      if (condition == "Is")
      {
        this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.Equals, precision));
        str = name + " is " + value1;
      }
      else if (condition == "Before" || condition == "Less Than")
      {
        this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.LessThan, precision));
        str = name + " is " + condition.ToLower() + " " + value1;
      }
      else if (condition == "After" || condition == "Greater Than")
      {
        this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.GreaterThan, precision));
        str = name + " is " + condition.ToLower() + " " + value1;
      }
      else
      {
        if (dateTime1.Month > today2.Month)
        {
          DateTime dateTime2 = new DateTime(dateTime1.Year, 12, 31);
          BinaryOperation lhs = new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.GreaterThan, precision), (QueryCriterion) new DateValueCriterion(fieldName, dateTime2, OrdinalMatchType.LessThanOrEquals, precision));
          DateTime dateTime3 = new DateTime(today2.Year, 1, 1);
          BinaryOperation rhs = new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime3, OrdinalMatchType.GreaterThanOrEquals, precision), (QueryCriterion) new DateValueCriterion(fieldName, today2, OrdinalMatchType.LessThan, precision));
          this.criteria.Add((object) new BinaryOperation(BinaryOperator.Or, (QueryCriterion) lhs, (QueryCriterion) rhs));
        }
        else
        {
          this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.GreaterThan, precision));
          this.criteria.Add((object) new DateValueCriterion(fieldName, today2, OrdinalMatchType.LessThan, precision));
        }
        str = name + " is between " + value1 + " and " + value2;
      }
      return str;
    }

    protected void AddDateValueQuery(
      string name,
      string fieldName,
      string condition,
      string value1,
      string value2,
      bool bIsCustomMonthDay)
    {
      DateTime dateTime1 = DateTime.Today;
      DateTime dateTime2 = DateTime.Today;
      int count = this.criteria.Count;
      DateMatchPrecision precision = DateMatchPrecision.Day;
      if (((name == "Birthday" ? 1 : (name == "Anniversary" ? 1 : 0)) | (bIsCustomMonthDay ? 1 : 0)) != 0)
        precision = DateMatchPrecision.Recurring;
      string str;
      if (condition == "Is")
      {
        switch (value1)
        {
          case "Exact Date":
            if (value2 == string.Empty)
              return;
            DateTime dateTime3 = DateTime.Parse(value2);
            this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime3, OrdinalMatchType.Equals, precision));
            str = name + " is " + value2;
            break;
          case "Current Month":
            ref DateTime local1 = ref dateTime1;
            DateTime today1 = DateTime.Today;
            int year1 = today1.Year;
            today1 = DateTime.Today;
            int month1 = today1.Month;
            local1 = new DateTime(year1, month1, 1);
            today1 = DateTime.Today;
            int year2 = today1.Year;
            today1 = DateTime.Today;
            int month2 = today1.Month;
            int num = DateTime.DaysInMonth(year2, month2);
            ref DateTime local2 = ref dateTime2;
            today1 = DateTime.Today;
            int year3 = today1.Year;
            today1 = DateTime.Today;
            int month3 = today1.Month;
            int day1 = num;
            local2 = new DateTime(year3, month3, day1);
            this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.GreaterThanOrEquals, precision));
            this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime2, OrdinalMatchType.LessThanOrEquals, precision));
            str = name + " is " + value1;
            break;
          case "Current Week":
            int dayDiffFromSunday1 = this.getDayDiffFromSunday(DateTime.Today.DayOfWeek);
            DateTime dateTime4 = DateTime.Today;
            dateTime4 = dateTime4.AddDays((double) -dayDiffFromSunday1);
            DateTime dateTime5 = DateTime.Today;
            dateTime5 = dateTime5.AddDays((double) (6 - dayDiffFromSunday1));
            if (dateTime4.Month > dateTime5.Month)
            {
              DateTime dateTime6 = new DateTime(dateTime4.Year, 12, 31);
              BinaryOperation lhs = new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime4, OrdinalMatchType.GreaterThanOrEquals, precision), (QueryCriterion) new DateValueCriterion(fieldName, dateTime6, OrdinalMatchType.LessThanOrEquals, precision));
              DateTime dateTime7 = new DateTime(dateTime5.Year, 1, 1);
              BinaryOperation rhs = new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime7, OrdinalMatchType.GreaterThanOrEquals, precision), (QueryCriterion) new DateValueCriterion(fieldName, dateTime5, OrdinalMatchType.LessThanOrEquals, precision));
              this.criteria.Add((object) new BinaryOperation(BinaryOperator.Or, (QueryCriterion) lhs, (QueryCriterion) rhs));
            }
            else
            {
              this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime4, OrdinalMatchType.GreaterThanOrEquals, precision));
              this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime5, OrdinalMatchType.LessThanOrEquals, precision));
            }
            str = name + " is " + value1;
            break;
          case "Next Month":
            ref DateTime local3 = ref dateTime1;
            DateTime today2 = DateTime.Today;
            int year4 = today2.Year;
            today2 = DateTime.Today;
            int month4 = today2.Month;
            local3 = new DateTime(year4, month4, 1);
            dateTime1 = dateTime1.AddMonths(1);
            int day2 = DateTime.DaysInMonth(dateTime1.Year, dateTime1.Month);
            dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, day2);
            this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime1, OrdinalMatchType.GreaterThanOrEquals, precision));
            this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime2, OrdinalMatchType.LessThanOrEquals, precision));
            str = name + " is " + value1;
            break;
          case "Next Week":
            int dayDiffFromSunday2 = this.getDayDiffFromSunday(DateTime.Today.DayOfWeek);
            DateTime dateTime8 = DateTime.Today;
            dateTime8 = dateTime8.AddDays((double) (-dayDiffFromSunday2 + 7));
            DateTime dateTime9 = DateTime.Today.AddDays((double) (13 - dayDiffFromSunday2));
            if (dateTime8.Month > dateTime9.Month)
            {
              DateTime dateTime10 = new DateTime(dateTime8.Year, 12, 31);
              BinaryOperation lhs = new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime8, OrdinalMatchType.GreaterThanOrEquals, precision), (QueryCriterion) new DateValueCriterion(fieldName, dateTime10, OrdinalMatchType.LessThanOrEquals, precision));
              DateTime dateTime11 = new DateTime(dateTime9.Year, 1, 1);
              BinaryOperation rhs = new BinaryOperation(BinaryOperator.And, (QueryCriterion) new DateValueCriterion(fieldName, dateTime11, OrdinalMatchType.GreaterThanOrEquals, precision), (QueryCriterion) new DateValueCriterion(fieldName, dateTime9, OrdinalMatchType.LessThanOrEquals, precision));
              this.criteria.Add((object) new BinaryOperation(BinaryOperator.Or, (QueryCriterion) lhs, (QueryCriterion) rhs));
            }
            else
            {
              this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime8, OrdinalMatchType.GreaterThanOrEquals, precision));
              this.criteria.Add((object) new DateValueCriterion(fieldName, dateTime9, OrdinalMatchType.LessThanOrEquals, precision));
            }
            str = name + " is " + value1;
            break;
          default:
            str = this.addCustomDateValueQuery(name, fieldName, condition, value1, value2, bIsCustomMonthDay, precision);
            break;
        }
      }
      else
        str = this.addCustomDateValueQuery(name, fieldName, condition, value1, value2, bIsCustomMonthDay, precision);
      if (str == null)
        return;
      this.description = this.description + (count == 0 ? " " : " and ") + str;
    }

    private int getDayDiffFromSunday(DayOfWeek day)
    {
      switch (day)
      {
        case DayOfWeek.Sunday:
          return 0;
        case DayOfWeek.Monday:
          return 1;
        case DayOfWeek.Tuesday:
          return 2;
        case DayOfWeek.Wednesday:
          return 3;
        case DayOfWeek.Thursday:
          return 4;
        case DayOfWeek.Friday:
          return 5;
        case DayOfWeek.Saturday:
          return 6;
        default:
          return 0;
      }
    }

    protected void AddStringValueQuery(
      string name,
      string fieldName,
      string condition,
      string value,
      StringMatchType matchType,
      string valueToDisplay)
    {
      if (value == string.Empty)
        return;
      bool include = true;
      if (string.Compare("Is Not", condition, true) == 0)
      {
        matchType = StringMatchType.Exact;
        include = false;
      }
      int count = this.criteria.Count;
      this.criteria.Add((object) new StringValueCriterion(fieldName, value, matchType, include));
      if (name == "")
        return;
      if (fieldName.ToLower() == "contact.ownerid")
      {
        string str = value;
        UserInfo contactOwner = this.getContactOwner(value);
        if (contactOwner != (UserInfo) null)
          str = contactOwner.FullName;
        this.description = this.description + (count == 0 ? " " : " and ") + name + " " + condition + " " + str;
      }
      else
        this.description = this.description + (count == 0 ? " " : " and ") + name + " " + condition + " " + valueToDisplay;
    }

    protected void AddXValueQuery(
      string name,
      string fieldName,
      string value,
      StringMatchType matchType)
    {
      int count = this.criteria.Count;
      this.criteria.Add((object) new StringValueCriterion(fieldName, value, matchType));
      if (value == string.Empty)
        value = "empty";
      this.description = this.description + (count == 0 ? " " : " and ") + name + " is " + value;
    }

    private StringMatchType getStringMatchTypeFromString(string strValue)
    {
      if (string.Compare("Starts With", strValue, true) == 0)
        return StringMatchType.StartsWith;
      if (string.Compare("Contains", strValue, true) == 0)
        return StringMatchType.Contains;
      return string.Compare("Is Not", strValue, true) == 0 ? StringMatchType.NotEquals : StringMatchType.Exact;
    }
  }
}
