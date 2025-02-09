// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Panel
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents a simple <see cref="T:EllieMae.Encompass.Forms.ContainerControl" /> with a single border.
  /// </summary>
  [ToolboxControl("Panel")]
  public class Panel : ContainerControl
  {
    /// <summary>Constructor for a new Panel control.</summary>
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
