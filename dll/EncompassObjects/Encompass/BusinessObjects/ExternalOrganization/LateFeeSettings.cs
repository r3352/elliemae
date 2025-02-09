// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.LateFeeSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  public class LateFeeSettings : IExternalLateFees
  {
    private ExternalLateFeeSettings externalLateFee;

    internal LateFeeSettings(ExternalLateFeeSettings externalLateFee)
    {
      this.externalLateFee = externalLateFee;
    }

    public int LateFeeSettingID => this.externalLateFee.LateFeeSettingID;

    public int ExternalOrgID => this.externalLateFee.ExternalOrgID;

    public int GracePeriodDays
    {
      get => this.externalLateFee.GracePeriodDays;
      set => this.externalLateFee.GracePeriodDays = value;
    }

    public GracePeriodCalendar GracePeriodCalendar
    {
      get
      {
        switch (this.externalLateFee.GracePeriodCalendar)
        {
          case 0:
            return GracePeriodCalendar.WeekDays;
          case 1:
            return GracePeriodCalendar.CalendarDays;
          case 2:
            return GracePeriodCalendar.CompanyCalendar;
          default:
            return GracePeriodCalendar.WeekDays;
        }
      }
      set
      {
        switch (value)
        {
          case GracePeriodCalendar.WeekDays:
            this.externalLateFee.GracePeriodCalendar = 0;
            break;
          case GracePeriodCalendar.CalendarDays:
            this.externalLateFee.GracePeriodCalendar = 1;
            break;
          case GracePeriodCalendar.CompanyCalendar:
            this.externalLateFee.GracePeriodCalendar = 2;
            break;
          default:
            this.externalLateFee.GracePeriodCalendar = 0;
            break;
        }
      }
    }

    public GracePeriodStarts GracePeriodStarts
    {
      get
      {
        switch (this.externalLateFee.GracePeriodStarts)
        {
          case 0:
            return GracePeriodStarts.On;
          case 1:
            return GracePeriodStarts.DayAfter;
          default:
            return GracePeriodStarts.On;
        }
      }
      set
      {
        if (value != GracePeriodStarts.On)
        {
          if (value == GracePeriodStarts.DayAfter)
            this.externalLateFee.GracePeriodStarts = 1;
          else
            this.externalLateFee.GracePeriodStarts = 0;
        }
        else
          this.externalLateFee.GracePeriodStarts = 0;
      }
    }

    public bool ContainsGracePeriodLaterOf(EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf gracePeriodLaterOf)
    {
      return ((EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf) this.GracePeriodLaterOf & gracePeriodLaterOf) == gracePeriodLaterOf;
    }

    private int GracePeriodLaterOf
    {
      get => this.externalLateFee.GracePeriodLaterOf;
      set => this.externalLateFee.GracePeriodLaterOf = value;
    }

    public string OtherDate
    {
      get
      {
        return this.ContainsGracePeriodLaterOf(EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf.OtherDate) ? this.externalLateFee.OtherDate : "";
      }
      set => this.externalLateFee.OtherDate = value;
    }

    public bool StartOnWeekend
    {
      get => this.externalLateFee.StartOnWeekend == 0;
      set => this.externalLateFee.StartOnWeekend = value ? 0 : 1;
    }

    public bool IncludeDay
    {
      get => this.externalLateFee.IncludeDay == 0;
      set => this.externalLateFee.IncludeDay = value ? 0 : 1;
    }

    public FeeHandledAs FeeHandledAs
    {
      get
      {
        switch (this.externalLateFee.FeeHandledAs)
        {
          case 0:
            return FeeHandledAs.Fee;
          case 1:
            return FeeHandledAs.PriceAdjustment;
          default:
            return FeeHandledAs.Fee;
        }
      }
      set
      {
        if (value != FeeHandledAs.Fee)
        {
          if (value == FeeHandledAs.PriceAdjustment)
            this.externalLateFee.FeeHandledAs = 1;
          else
            this.externalLateFee.FeeHandledAs = 0;
        }
        else
          this.externalLateFee.FeeHandledAs = 0;
      }
    }

    public double LateFeePercent
    {
      get => this.externalLateFee.LateFee;
      set => this.externalLateFee.LateFee = value;
    }

    public LateFeeBasedOn LateFeeBasedOn
    {
      get
      {
        switch (this.externalLateFee.LateFeeBasedOn)
        {
          case 0:
            return LateFeeBasedOn.Total;
          case 1:
            return LateFeeBasedOn.BaseLoanAmount;
          default:
            return LateFeeBasedOn.Total;
        }
      }
      set
      {
        if (value != LateFeeBasedOn.Total)
        {
          if (value == LateFeeBasedOn.BaseLoanAmount)
            this.externalLateFee.LateFeeBasedOn = 1;
          else
            this.externalLateFee.LateFeeBasedOn = 0;
        }
        else
          this.externalLateFee.LateFeeBasedOn = 0;
      }
    }

    public double Amount
    {
      get => this.externalLateFee.Amount;
      set => this.externalLateFee.Amount = value;
    }

    public CalculateAs CalculateAs
    {
      get
      {
        switch (this.externalLateFee.CalculateAs)
        {
          case 0:
            return CalculateAs.Flat;
          case 1:
            return CalculateAs.DailyFee;
          default:
            return CalculateAs.Flat;
        }
      }
      set
      {
        if (value != CalculateAs.Flat)
        {
          if (value == CalculateAs.DailyFee)
            this.externalLateFee.CalculateAs = 1;
          else
            this.externalLateFee.CalculateAs = 0;
        }
        else
          this.externalLateFee.CalculateAs = 0;
      }
    }

    public int MaxLateDays
    {
      get => this.externalLateFee.MaxLateDays;
      set => this.externalLateFee.MaxLateDays = value;
    }
  }
}
