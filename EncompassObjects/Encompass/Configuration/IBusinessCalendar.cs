// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.IBusinessCalendar
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>Represents the interface for the DataObject object.</summary>
  /// <exclude />
  [Guid("0D999359-45BA-457c-A6DD-565B90B1FCA8")]
  public interface IBusinessCalendar
  {
    BusinessCalendarType CalendarType { get; }

    DaysOfWeek WorkDays { get; }

    bool IsWeekendDay(DateTime date);

    bool IsBusinessDay(DateTime date);

    BusinessCalendarDayType GetDayType(DateTime date);

    DateTime AddBusinessDays(DateTime date, int count, bool startFromClosestBusinessDay);

    DateTime GetNextClosestBusinessDay(DateTime date);
  }
}
