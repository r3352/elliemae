// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DesignerControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public abstract class DesignerControl : Control
  {
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
