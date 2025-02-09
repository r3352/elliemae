// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.LateFeeSettings
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents late fee settings.</summary>
  public class LateFeeSettings : IExternalLateFees
  {
    private ExternalLateFeeSettings externalLateFee;

    internal LateFeeSettings(ExternalLateFeeSettings externalLateFee)
    {
      this.externalLateFee = externalLateFee;
    }

    /// <summary>Gets the LateFeeSetting Id</summary>
    public int LateFeeSettingID => this.externalLateFee.LateFeeSettingID;

    /// <summary>Gets the external organization id</summary>
    public int ExternalOrgID => this.externalLateFee.ExternalOrgID;

    /// <summary>Gets or sets the number of grace period days</summary>
    public int GracePeriodDays
    {
      get => this.externalLateFee.GracePeriodDays;
      set => this.externalLateFee.GracePeriodDays = value;
    }

    /// <summary>Gets or sets the GracePeriodCalendar object</summary>
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

    /// <summary>Gets or sets the value when grace period starts</summary>
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

    /// <summary>
    /// Method to check if a particular Grace Period Later Of is allowed
    /// </summary>
    /// <param name="gracePeriodLaterOf">The Grace Period Later Of to check.</param>
    /// <returns>Indicates if a particular Grace Period Later Of is selected.</returns>
    public bool ContainsGracePeriodLaterOf(EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf gracePeriodLaterOf)
    {
      return ((EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf) this.GracePeriodLaterOf & gracePeriodLaterOf) == gracePeriodLaterOf;
    }

    private int GracePeriodLaterOf
    {
      get => this.externalLateFee.GracePeriodLaterOf;
      set => this.externalLateFee.GracePeriodLaterOf = value;
    }

    /// <summary>Gets or sets the Other date</summary>
    public string OtherDate
    {
      get
      {
        return this.ContainsGracePeriodLaterOf(EllieMae.Encompass.BusinessObjects.ExternalOrganization.GracePeriodLaterOf.OtherDate) ? this.externalLateFee.OtherDate : "";
      }
      set => this.externalLateFee.OtherDate = value;
    }

    /// <summary>
    /// Gets or sets the boolean value to indicate if the late fee starts on weekend
    /// </summary>
    public bool StartOnWeekend
    {
      get => this.externalLateFee.StartOnWeekend == 0;
      set => this.externalLateFee.StartOnWeekend = value ? 0 : 1;
    }

    /// <summary>
    /// Gets or sets the boolean value if the late fee starts including day
    /// </summary>
    public bool IncludeDay
    {
      get => this.externalLateFee.IncludeDay == 0;
      set => this.externalLateFee.IncludeDay = value ? 0 : 1;
    }

    /// <summary>Gets or sets the value as to how the fee is handled</summary>
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

    /// <summary>Gets or sets the Late fee percentage value</summary>
    public double LateFeePercent
    {
      get => this.externalLateFee.LateFee;
      set => this.externalLateFee.LateFee = value;
    }

    /// <summary>
    /// Gets or sets the option on which the late fee is based
    /// </summary>
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

    /// <summary>Gets or sets the amount of the external late fee</summary>
    public double Amount
    {
      get => this.externalLateFee.Amount;
      set => this.externalLateFee.Amount = value;
    }

    /// <summary>
    /// Gets or sets the option as the how the external late will be calculated
    /// </summary>
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

    /// <summary>Gets or sets the value for the maximum delay period</summary>
    public int MaxLateDays
    {
      get => this.externalLateFee.MaxLateDays;
      set => this.externalLateFee.MaxLateDays = value;
    }
  }
}
