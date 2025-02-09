// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.GroupBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// A <see cref="T:EllieMae.Encompass.Forms.ContainerControl" /> used for second-level sections in Encompass input forms.
  /// </summary>
  [ToolboxControl("Group Box")]
  public class GroupBox : SectionControl
  {
    /// <summary>Constructor for a new GroupBox control.</summary>
    public GroupBox()
    {
    }

    internal GroupBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string AttributeValue = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(AttributeValue))
        return;
      controlElement.setAttribute("contentEditable", (object) AttributeValue);
    }

    /// <summary>
    /// Overrides the header background color of a GroupBox control. Because this control's header
    /// contains a backgroun image, this property is ineffective.
    /// </summary>
    [Browsable(false)]
    public override Color HeaderBackColor
    {
      get => base.HeaderBackColor;
      set => base.HeaderBackColor = value;
    }

    internal override IHTMLElement FindContainerElement(IHTMLElement controlElement)
    {
      if (controlElement.tagName.ToLower() == "span")
      {
        IHTMLTable elementWithTagName = (IHTMLTable) HTMLHelper.FindElementWithTagName(controlElement, "table");
        if (elementWithTagName != null && elementWithTagName.rows.length >= 2)
        {
          IHTMLElement containerElement = this.FindContainerElement((IHTMLElement) elementWithTagName.rows.item((object) 1), false);
          if (containerElement != null)
            return containerElement;
        }
      }
      return base.FindContainerElement(controlElement);
    }

    internal override IHTMLElement FindHeaderElement(IHTMLElement controlElement)
    {
      if (controlElement.tagName.ToLower() == "span")
      {
        IHTMLTable elementWithTagName = (IHTMLTable) HTMLHelper.FindElementWithTagName(controlElement, "table");
        if (elementWithTagName != null && elementWithTagName.rows.length >= 1)
        {
          IHTMLElement headerElement = base.FindHeaderElement((IHTMLElement) elementWithTagName.rows.item(index: (object) 0));
          if (headerElement != null)
            return headerElement;
        }
      }
      return base.FindHeaderElement(controlElement);
    }

    internal override string RenderHTML()
    {
      return "<SPAN" + this.GetBaseAttributes() + "style=\"width: 200; height: 100px;\"><TABLE cellSpacing=0 cellPadding=0 width=\"100%\" height=\"100%\" border=0>" + Environment.NewLine + "  <TR>" + Environment.NewLine + "    <TD class=\"contentHeaderWhite\"" + this.GetBaseHeaderAttributes() + ">Your Group Header Goes Here</TD>" + Environment.NewLine + "  </TR>" + Environment.NewLine + "  <TR>" + Environment.NewLine + "    <TD height=\"100%\"><span" + this.GetBaseContainerAttributes() + " style=\"width: 100%; height: 100%;\"></span></TD>" + Environment.NewLine + "  </TR>" + Environment.NewLine + "</TABLE></SPAN>";
    }
  }
}
