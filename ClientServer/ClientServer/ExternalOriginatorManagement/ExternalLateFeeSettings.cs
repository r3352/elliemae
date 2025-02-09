// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagement.ExternalLateFeeSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ExternalOriginatorManagement
{
  [Serializable]
  public class ExternalLateFeeSettings
  {
    public int LateFeeSettingID { get; set; }

    public int ExternalOrgID { get; set; }

    public int GracePeriodDays { get; set; }

    public int GracePeriodCalendar { get; set; }

    public int GracePeriodStarts { get; set; }

    public int GracePeriodLaterOf { get; set; }

    public string OtherDate { get; set; }

    public int StartOnWeekend { get; set; }

    public int IncludeDay { get; set; }

    public int FeeHandledAs { get; set; }

    public double LateFee { get; set; }

    public int LateFeeBasedOn { get; set; }

    public double Amount { get; set; }

    public int CalculateAs { get; set; }

    public int MaxLateDays { get; set; }

    public int DayCleared { get; set; }

    public string DayClearedOtherDate { get; set; }

    public ExternalLateFeeSettings(
      int externalOrgID,
      int gracePeriodDays,
      int gracePeriodCalendar,
      int gracePeriodStarts,
      int gracePeriodLaterOf,
      string otherDate,
      int startOnWeekend,
      int includeDay,
      int feeHandledAs,
      int lateFee,
      int lateFeeBasedOn,
      int amount,
      int calculateAs,
      int maxLateDays,
      int dayCleared = 0,
      string dayClearedOtherDate = null)
    {
      this.ExternalOrgID = externalOrgID;
      this.GracePeriodDays = gracePeriodDays;
      this.GracePeriodCalendar = gracePeriodCalendar;
      this.GracePeriodStarts = gracePeriodStarts;
      this.GracePeriodLaterOf = gracePeriodLaterOf;
      this.OtherDate = otherDate;
      this.StartOnWeekend = startOnWeekend;
      this.IncludeDay = includeDay;
      this.FeeHandledAs = feeHandledAs;
      this.LateFee = (double) lateFee;
      this.LateFeeBasedOn = lateFeeBasedOn;
      this.Amount = (double) amount;
      this.CalculateAs = calculateAs;
      this.MaxLateDays = maxLateDays;
      this.DayCleared = dayCleared;
      this.DayClearedOtherDate = dayClearedOtherDate;
    }

    public ExternalLateFeeSettings()
    {
    }
  }
}
