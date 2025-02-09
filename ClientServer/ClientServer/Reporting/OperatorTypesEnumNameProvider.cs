// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Reporting.OperatorTypesEnumNameProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Reporting
{
  public class OperatorTypesEnumNameProvider : IEnumNameProvider
  {
    private static Hashtable nameToValueMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private static Hashtable valueToNameMap = new Hashtable();

    static OperatorTypesEnumNameProvider()
    {
      OperatorTypesEnumNameProvider.nameToValueMap[(object) ""] = (object) OperatorTypes.None;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "None"] = (object) OperatorTypes.None;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is (Exact)"] = (object) OperatorTypes.IsExact;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is not"] = (object) OperatorTypes.IsNot;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Starts with"] = (object) OperatorTypes.StartsWith;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Doesn't start with"] = (object) OperatorTypes.DoesnotStartWith;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Contains"] = (object) OperatorTypes.Contains;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Doesn't contain"] = (object) OperatorTypes.DoesnotContain;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is"] = (object) OperatorTypes.Equals;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Equals"] = (object) OperatorTypes.Equals;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Equal to"] = (object) OperatorTypes.Equals;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Does not equal"] = (object) OperatorTypes.NotEqual;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Not equal to"] = (object) OperatorTypes.NotEqual;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Greater than"] = (object) OperatorTypes.GreaterThan;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Not greater than"] = (object) OperatorTypes.NotGreaterThan;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Less than"] = (object) OperatorTypes.LessThan;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Not less than"] = (object) OperatorTypes.NotLessThan;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Between"] = (object) OperatorTypes.Between;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Not between"] = (object) OperatorTypes.NotBetween;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Today"] = (object) OperatorTypes.Today;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Current week"] = (object) OperatorTypes.CurrentWeek;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Current month"] = (object) OperatorTypes.CurrentMonth;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next week"] = (object) OperatorTypes.NextWeek;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next month"] = (object) OperatorTypes.NextMonth;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next year"] = (object) OperatorTypes.NextYear;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Year-to-date"] = (object) OperatorTypes.YearToDate;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Previous week"] = (object) OperatorTypes.PreviousWeek;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Previous month"] = (object) OperatorTypes.PreviousMonth;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Previous year"] = (object) OperatorTypes.PreviousYear;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 7 days"] = (object) OperatorTypes.Last7Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 15 days"] = (object) OperatorTypes.Last15Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 30 days"] = (object) OperatorTypes.Last30Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 60 days"] = (object) OperatorTypes.Last60Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 90 days"] = (object) OperatorTypes.Last90Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 180 days"] = (object) OperatorTypes.Last180Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Last 365 days"] = (object) OperatorTypes.Last365Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 7 days"] = (object) OperatorTypes.Next7Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 15 days"] = (object) OperatorTypes.Next15Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 30 days"] = (object) OperatorTypes.Next30Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 60 days"] = (object) OperatorTypes.Next60Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 90 days"] = (object) OperatorTypes.Next90Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 180 days"] = (object) OperatorTypes.Next180Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Next 365 days"] = (object) OperatorTypes.Next365Days;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Date between"] = (object) OperatorTypes.DateBetween;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Date not between"] = (object) OperatorTypes.DateNotBetween;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Occurs between"] = (object) OperatorTypes.DateBetween;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Does not occur between"] = (object) OperatorTypes.DateNotBetween;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "empty date field"] = (object) OperatorTypes.EmptyDate;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "non-empty date field"] = (object) OperatorTypes.NotEmptyDate;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "No date specified"] = (object) OperatorTypes.EmptyDate;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Date is specified"] = (object) OperatorTypes.NotEmptyDate;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "On or after"] = (object) OperatorTypes.DateOnOrAfter;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "On or before"] = (object) OperatorTypes.DateOnOrBefore;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "After"] = (object) OperatorTypes.DateAfter;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Before"] = (object) OperatorTypes.DateBefore;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is any of"] = (object) OperatorTypes.IsAnyOf;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is not any of"] = (object) OperatorTypes.IsNotAnyOf;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is Yes"] = (object) OperatorTypes.IsYes;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is No"] = (object) OperatorTypes.IsNotYes;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Yes"] = (object) OperatorTypes.IsYes;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "No"] = (object) OperatorTypes.IsNotYes;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is checked"] = (object) OperatorTypes.IsChecked;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is not checked"] = (object) OperatorTypes.IsNotChecked;
      OperatorTypesEnumNameProvider.nameToValueMap[(object) "Is unchecked"] = (object) OperatorTypes.IsNotChecked;
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.None] = (object) "";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsExact] = (object) "Is (Exact)";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsNot] = (object) "Is not";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.StartsWith] = (object) "Starts with";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DoesnotStartWith] = (object) "Doesn't start with";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Contains] = (object) "Contains";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DoesnotContain] = (object) "Doesn't contain";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Equals] = (object) "Is";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NotEqual] = (object) "Is not";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.GreaterThan] = (object) "Greater than";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NotGreaterThan] = (object) "Not greater than";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.LessThan] = (object) "Less than";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NotLessThan] = (object) "Not less than";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Between] = (object) "Between";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NotBetween] = (object) "Not between";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Today] = (object) "Today";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.CurrentWeek] = (object) "Current week";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.CurrentMonth] = (object) "Current month";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NextWeek] = (object) "Next week";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NextMonth] = (object) "Next month";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NextYear] = (object) "Next year";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.YearToDate] = (object) "Year-to-date";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.PreviousWeek] = (object) "Previous week";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.PreviousMonth] = (object) "Previous month";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.PreviousYear] = (object) "Previous year";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last7Days] = (object) "Last 7 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last15Days] = (object) "Last 15 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last30Days] = (object) "Last 30 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last60Days] = (object) "Last 60 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last90Days] = (object) "Last 90 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last180Days] = (object) "Last 180 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Last365Days] = (object) "Last 365 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next7Days] = (object) "Next 7 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next15Days] = (object) "Next 15 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next30Days] = (object) "Next 30 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next60Days] = (object) "Next 60 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next90Days] = (object) "Next 90 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next180Days] = (object) "Next 180 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.Next365Days] = (object) "Next 365 days";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DateBetween] = (object) "Date between";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DateNotBetween] = (object) "Date not between";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.EmptyDate] = (object) "Empty Date Field";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.NotEmptyDate] = (object) "Non-empty Date Field";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DateAfter] = (object) "After";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DateOnOrAfter] = (object) "On or after";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DateBefore] = (object) "Before";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.DateOnOrBefore] = (object) "On or before";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsAnyOf] = (object) "Is any of";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsNotAnyOf] = (object) "Is not any of";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsYes] = (object) "Is Yes";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsNotYes] = (object) "Is No";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsChecked] = (object) "Is checked";
      OperatorTypesEnumNameProvider.valueToNameMap[(object) OperatorTypes.IsNotChecked] = (object) "Is not checked";
    }

    public string GetName(object value)
    {
      return OperatorTypesEnumNameProvider.valueToNameMap.Contains(value) ? (string) OperatorTypesEnumNameProvider.valueToNameMap[value] : throw new Exception("Invalid enum value specified");
    }

    public string[] GetNames()
    {
      ArrayList arrayList = new ArrayList();
      foreach (string str in (IEnumerable) OperatorTypesEnumNameProvider.valueToNameMap.Values)
        arrayList.Add((object) str);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public object GetValue(string name)
    {
      return OperatorTypesEnumNameProvider.nameToValueMap.Contains((object) name) ? (object) (OperatorTypes) OperatorTypesEnumNameProvider.nameToValueMap[(object) name] : throw new Exception("Invalid name specified");
    }

    public bool IsDynamic(OperatorTypes op)
    {
      switch (op)
      {
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
          return true;
        default:
          if ((uint) (op - 37) > 13U)
            return false;
          goto case OperatorTypes.CurrentWeek;
      }
    }

    public int GetParameterCount(OperatorTypes op)
    {
      switch (op)
      {
        case OperatorTypes.Between:
        case OperatorTypes.NotBetween:
        case OperatorTypes.DateBetween:
        case OperatorTypes.DateNotBetween:
          return 2;
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
        case OperatorTypes.EmptyDate:
        case OperatorTypes.NotEmptyDate:
        case OperatorTypes.IsYes:
        case OperatorTypes.IsNotYes:
        case OperatorTypes.IsChecked:
        case OperatorTypes.IsNotChecked:
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
          return 0;
        default:
          return 1;
      }
    }
  }
}
