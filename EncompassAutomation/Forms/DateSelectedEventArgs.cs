// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DateSelectedEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Class representing the arguments for the DateSelectedEvent event
  /// </summary>
  public class DateSelectedEventArgs : EventArgs
  {
    private DateTime selectedDate;

    internal DateSelectedEventArgs(DateTime selectedDate) => this.selectedDate = selectedDate;

    /// <summary>Gets the DateTime selected by the user.</summary>
    public DateTime SelectedDate => this.selectedDate;
  }
}
