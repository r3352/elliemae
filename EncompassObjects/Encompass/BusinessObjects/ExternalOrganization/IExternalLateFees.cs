// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalLateFees
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>
  /// Interface for IExternalLateFees class to support External Late Fees
  /// </summary>
  /// <exclude />
  public interface IExternalLateFees
  {
    int LateFeeSettingID { get; }

    int ExternalOrgID { get; }

    int GracePeriodDays { get; set; }

    GracePeriodCalendar GracePeriodCalendar { get; set; }

    GracePeriodStarts GracePeriodStarts { get; set; }

    bool ContainsGracePeriodLaterOf(GracePeriodLaterOf gracePeriodLaterOf);

    string OtherDate { get; set; }

    bool StartOnWeekend { get; set; }

    bool IncludeDay { get; set; }

    FeeHandledAs FeeHandledAs { get; set; }

    double LateFeePercent { get; set; }

    LateFeeBasedOn LateFeeBasedOn { get; set; }

    double Amount { get; set; }

    CalculateAs CalculateAs { get; set; }

    int MaxLateDays { get; set; }
  }
}
