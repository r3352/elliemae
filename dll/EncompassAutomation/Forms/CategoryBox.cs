// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.CategoryBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Category Box")]
  public class CategoryBox : SectionControl
  {
    public CategoryBox()
    {
    }

    internal CategoryBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string str = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(str))
        return;
      controlElement.setAttribute("contentEditable", (object) str, 1);
    }

    internal override IHTMLElement FindContainerElement(IHTMLElement controlElement)
    {
      if (controlElement.tagName.ToLower() == "span")
      {
        IHTMLTable elementWithTagName = (IHTMLTable) HTMLHelper.FindElementWithTagName(controlElement, "table");
        if (elementWithTagName != null && elementWithTagName.rows.length >= 3)
        {
          IHTMLElement containerElement = this.FindContainerElement((IHTMLElement) elementWithTagName.rows.item((object) 2, (object) null), false);
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
          IHTMLElement headerElement = base.FindHeaderElement((IHTMLElement) elementWithTagName.rows.item((object) null, (object) 0));
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
