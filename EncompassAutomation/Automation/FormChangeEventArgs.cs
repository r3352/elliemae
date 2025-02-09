// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Automation.FormChangeEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.Forms;
using System;

#nullable disable
namespace EllieMae.Encompass.Automation
{
  /// <summary>
  /// Event argument class for the LoansScreen's FormLoaded and FormUnloading events.
  /// </summary>
  public class FormChangeEventArgs : EventArgs
  {
    private Form form;

    internal FormChangeEventArgs(Form form) => this.form = form;

    /// <summary>Gets the Form which is being loaded or unloaded.</summary>
    public Form Form => this.form;
  }
}
