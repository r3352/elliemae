// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.MultilineTextBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Multi-line Text Box")]
  public class MultilineTextBox : TextBox
  {
    public MultilineTextBox()
    {
    }

    internal MultilineTextBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Browsable(false)]
    public override string Text
    {
      get => ((IHTMLTextAreaElement) this.FieldElement).value ?? "";
      set => this.ApplyValueToControl(value);
    }

    [Browsable(false)]
    public override int MaxLength
    {
      get => -1;
      set => throw new NotSupportedException();
    }

    public override bool SupportsMaxLength => false;

    internal override bool ReadOnly
    {
      get => ((IHTMLTextAreaElement) this.HTMLElement).readOnly;
      set => ((IHTMLTextAreaElement) this.HTMLElement).readOnly = value;
    }

    internal override void DisplayValue(string value)
    {
      ((IHTMLTextAreaElement) this.FieldElement).value = value;
    }

    internal override string RenderHTML()
    {
      return "<textarea" + this.GetBaseAttributes() + this.GetBaseFieldAttributes() + " style=\"width: 100px; height: 40px\" wrap></textarea>";
    }
  }
}
