// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.CategoryBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// A CategoryBox represents a <see cref="T:EllieMae.Encompass.Forms.ContainerControl" /> which is formatted to appear
  /// stylistically consistent with the top-level sections of an Encompass input form.
  /// </summary>
  [ToolboxControl("Category Box")]
  public class CategoryBox : SectionControl
  {
    /// <summary>Constructor for a new Categorybox control.</summary>
    public CategoryBox()
    {
    }

    internal CategoryBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string AttributeValue = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(AttributeValue))
        return;
      controlElement.setAttribute("contentEditable", (object) AttributeValue);
    }

    internal override IHTMLElement FindContainerElement(IHTMLElement controlElement)
    {
      if (controlElement.tagName.ToLower() == "span")
      {
        IHTMLTable elementWithTagName = (IHTMLTable) HTMLHelper.FindElementWithTagName(controlElement, "table");
        if (elementWithTagName != null && elementWithTagName.rows.length >= 3)
        {
          IHTMLElement containerElement = this.FindContainerElement((IHTMLElement) elementWithTagName.rows.item((object) 2), false);
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
      return "<SPAN" + this.GetBaseAttributes() + "style=\"height: 200px; width:500px; margin: 0px 0px 4px 0px\"><TABLE cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" height=\"100%\" border=\"0\">" + Environment.NewLine + "  <TR>" + Environment.NewLine + "    <TD class=\"SectionHeader\"" + this.GetBaseHeaderAttributes() + " style=\"padding-top: 2px;\">Your Section Header Goes Here</TD>" + Environment.NewLine + "  </TR>" + Environment.NewLine + "  <TR>" + Environment.NewLine + "    <TD class=\"SectionHeaderRule\"></TD>" + Environment.NewLine + "  </TR>" + Environment.NewLine + "  <TR height=\"100%\">" + Environment.NewLine + "    <TD><span" + this.GetBaseContainerAttributes() + " style=\"width: 100%; height: 100%; background-color: #efefef;\"></span></TD>" + Environment.NewLine + "  </TR>" + Environment.NewLine + "</TABLE></SPAN>";
    }
  }
}
