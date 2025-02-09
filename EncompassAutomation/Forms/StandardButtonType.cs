// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.StandardButtonType
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Enumeration of the button types supported by the <see cref="T:EllieMae.Encompass.Forms.StandardButton" />.
  /// </summary>
  public enum StandardButtonType
  {
    /// <summary>Button is to display an Edit window.</summary>
    Edit = 1,
    /// <summary>Button is to display the lookup window.</summary>
    Lookup = 2,
    /// <summary>Button is to clear/delete a value.</summary>
    Clear = 3,
    /// <summary>Button is to refresh content of one or multiple fields depending on the action taken.</summary>
    Refresh = 4,
    /// <summary>Button is to display help content.</summary>
    Help = 5,
    /// <summary>Button displays a calendar.</summary>
    Calendar = 6,
    /// <summary>Button showing the alert icon.</summary>
    Alert = 7,
  }
}
