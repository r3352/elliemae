// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FormatEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Class representing the arguments for the Format event.
  /// </summary>
  /// <remarks>The Format event is used to perform on-the-fly formatting for text as the
  /// user types. Use the <see cref="P:EllieMae.Encompass.Forms.FormatEventArgs.Value" /> property to obserrve what the user has typed and,
  /// if necessary to change it.</remarks>
  public class FormatEventArgs : EventArgs
  {
    private string value = "";
    private bool cancel;

    internal FormatEventArgs(string value) => this.value = value;

    /// <summary>Gets or sets the value displayed in the control.</summary>
    /// <remarks>When the Format event is raised, the Value property represents the contents of the control
    /// including the user's latest modifications. Your custom code should apply whatever filtering or
    /// formatting logic is appropriate and set the result back into the Value property.
    /// </remarks>
    public string Value
    {
      get => this.value;
      set => this.value = value;
    }

    /// <summary>
    /// Gets or sets whether the default formatting should be applied.
    /// </summary>
    /// <remarks>By setting this property to <c>true</c> you prevent the automatic formatting based
    /// on the control's field type from being applied to the value. The automtic formatting is
    /// always applied after the Format event is raised, so any changes made to the <see cref="P:EllieMae.Encompass.Forms.FormatEventArgs.Value" />
    /// property will be auto-formatted unless this property is set to <c>false</c>.</remarks>
    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }
  }
}
