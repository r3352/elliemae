// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.OccupancyIntent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>The enumeration of the different Occupancy Intent</summary>
  [Guid("F1814031-B90E-4AC8-953F-7CB580385E40")]
  public enum OccupancyIntent
  {
    /// <summary>OccupancyIntent - Will Occupy</summary>
    WillOccupy,
    /// <summary>OccupnacyIntent - Will Not Occupy</summary>
    WillNotOccupy,
    /// <summary>Occupancy Intent - Currently Occupy</summary>
    CurrentlyOccupy,
    /// <summary>No Occupancy Intent Selected</summary>
    None,
  }
}
