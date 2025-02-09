// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.MonthWeekEnumUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class MonthWeekEnumUtil
  {
    private static Hashtable _NameToValue = new Hashtable();
    private static Hashtable _ValueToName;

    static MonthWeekEnumUtil()
    {
      MonthWeekEnumUtil._NameToValue.Add((object) "", (object) MonthWeekEnum.Blank);
      MonthWeekEnumUtil._NameToValue.Add((object) "Current Week", (object) MonthWeekEnum.CurrentWeek);
      MonthWeekEnumUtil._NameToValue.Add((object) "Current Month", (object) MonthWeekEnum.CurrentMonth);
      MonthWeekEnumUtil._NameToValue.Add((object) "Next Week", (object) MonthWeekEnum.NextWeek);
      MonthWeekEnumUtil._NameToValue.Add((object) "Next Month", (object) MonthWeekEnum.NextMonth);
      MonthWeekEnumUtil._NameToValue.Add((object) "Exact Date", (object) MonthWeekEnum.ExactDate);
      MonthWeekEnumUtil._ValueToName = new Hashtable();
      MonthWeekEnumUtil._ValueToName.Add((object) MonthWeekEnum.Blank, (object) "");
      MonthWeekEnumUtil._ValueToName.Add((object) MonthWeekEnum.CurrentMonth, (object) "Current Month");
      MonthWeekEnumUtil._ValueToName.Add((object) MonthWeekEnum.CurrentWeek, (object) "Current Week");
      MonthWeekEnumUtil._ValueToName.Add((object) MonthWeekEnum.NextMonth, (object) "Next Month");
      MonthWeekEnumUtil._ValueToName.Add((object) MonthWeekEnum.NextWeek, (object) "Next Week");
      MonthWeekEnumUtil._ValueToName.Add((object) MonthWeekEnum.ExactDate, (object) "Exact Date");
    }

    public static IDictionary ValueToNameMap => (IDictionary) MonthWeekEnumUtil._ValueToName;

    public static object[] GetDisplayNames()
    {
      MonthWeekEnum[] values = (MonthWeekEnum[]) Enum.GetValues(typeof (MonthWeekEnum));
      object[] displayNames = new object[values.Length];
      for (int index = 0; index < values.Length; ++index)
        displayNames[index] = (object) MonthWeekEnumUtil.ValueToName(values[index]);
      return displayNames;
    }

    public static string[] GetDisplayNamesNoExactDate()
    {
      MonthWeekEnum[] values = (MonthWeekEnum[]) Enum.GetValues(typeof (MonthWeekEnum));
      ArrayList arrayList = new ArrayList(values.Length);
      for (int index = 0; index < values.Length; ++index)
      {
        if (values[index] != MonthWeekEnum.ExactDate)
          arrayList.Add((object) MonthWeekEnumUtil.ValueToName(values[index]));
      }
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public static string ValueToName(MonthWeekEnum val)
    {
      return (string) MonthWeekEnumUtil._ValueToName[(object) val];
    }

    public static MonthWeekEnum NameToValue(string name)
    {
      return MonthWeekEnumUtil._NameToValue.Contains((object) name) ? (MonthWeekEnum) MonthWeekEnumUtil._NameToValue[(object) name] : throw new ArgumentException("The name " + name + " is not a valid value for MonthDateEnum.");
    }
  }
}
