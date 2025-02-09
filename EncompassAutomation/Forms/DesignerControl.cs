// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DesignerControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Designer controls are used for design-time support only and do not appear at run time.
  /// </summary>
  public abstract class DesignerControl : Control
  {
    /// <summary>Constructor for a new DesignerControl.</summary>
    public DesignerControl()
    {
    }

    internal DesignerControl(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.HTMLElement.style.display = this.Form.EditingEnabled ? "block" : "none";
    }

    internal override void OnStartEditing() => this.HTMLElement.style.display = "block";

    internal override void OnStopEditing() => this.HTMLElement.style.display = "none";
  }
}
