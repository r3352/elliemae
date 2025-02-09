// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.LockDeskSettings.LockDeskChannelSetting
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.LockDeskSettings
{
  /// <summary>Represents lock desk setting.</summary>
  public class LockDeskChannelSetting
  {
    /// <summary>Get or Set the setting of All Channels Indicator</summary>
    public string IsAllChannelsEnabled;
    /// <summary>
    /// Get or Set the setting of Lock Desk Start Time - Weekday (relabel?)
    /// </summary>
    public string WeekdayStartDateTime;
    /// <summary>
    /// Get or Set the setting of Lock Desk End Time - Weekday (relabel?)
    /// </summary>
    public string WeekdayEndDateTime;
    /// <summary>Get or Set the setting of Saturday Hours Indicator</summary>
    public string IsSaturdayHoursEnabled;
    /// <summary>
    /// Get or Set the setting of Lock Desk Start Time - Saturday
    /// </summary>
    public string SaturdayStartDateTime;
    /// <summary>
    /// Get or Set the setting of Lock Desk End Time - Saturday
    /// </summary>
    public string SaturdayEndDateTime;
    /// <summary>Get or Set the setting of Sunday Hours Indicator</summary>
    public string IsSundayHoursEnabled;
    /// <summary>
    /// Get or Set the setting of Lock Desk Start Time - Sunday
    /// </summary>
    public string SundayStartDateTime;
    /// <summary>Get or Set the setting of Lock Desk End Time - Sunday</summary>
    public string SundayEndDateTime;
    /// <summary>Get or Set the setting of Lock Desk Hours Message</summary>
    public string LockDeskHoursMessage;
    /// <summary>Get or Set the setting of Lock Desk Shutdown Message</summary>
    public string LockDeskShutDownMessage;
    /// <summary>
    /// Get or Set the setting of Lock Desk Shutdown Indicator
    /// </summary>
    public string IsLockDeskShutDown;
    /// <summary>
    /// Get or Set the setting to allow Active Relock Requests during lock down shutdown
    /// </summary>
    public string AllowActiveRelockRequests;
  }
}
