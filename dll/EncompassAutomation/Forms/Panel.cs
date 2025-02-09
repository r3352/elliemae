// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Panel
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Panel")]
  public class Panel : ContainerControl
  {
    public Panel()
    {
    }

    internal Panel(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    internal override IHTMLElement FindContainerElement(IHTMLElement controlElement)
    {
      return controlElement;
    }

    internal override string RenderHTML()
    {
      return "<div" + this.GetBaseAttributes() + this.GetBaseContainerAttributes() + " style=\"width: 150px; height: 150px; border: 1px dashed gray;\"/>";
    }
  }
}
