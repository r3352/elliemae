// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.ISystemSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Represents the interface for the SystemSettings object.
  /// </summary>
  /// <exclude />
  [Guid("8F0978F8-C230-49a6-AF29-A69C1E1FA19D")]
  public interface ISystemSettings
  {
    BusinessCalendar GetBusinessCalendar(BusinessCalendarType calendarType);
  }
}
