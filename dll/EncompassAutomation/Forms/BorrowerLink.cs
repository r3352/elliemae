// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.BorrowerLink
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using mshtml;
using System.ComponentModel;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Borrower Link")]
  public class BorrowerLink : RuntimeControl
  {
    private bool strongLink;

    public BorrowerLink()
    {
    }

    internal BorrowerLink(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Browsable(true)]
    public BorrowerType BorrowerType
    {
      get
      {
        switch (this.GetAttribute("isCoborrower"))
        {
          case "":
            this.SetAttribute("isCoborrower", false.ToString());
            return BorrowerType.Borrower;
          case "True":
            return BorrowerType.CoBorrower;
          default:
            return BorrowerType.Borrower;
        }
      }
      set
      {
        if (value == BorrowerType.Borrower)
          this.SetAttribute("isCoborrower", false.ToString());
        else
          this.SetAttribute("isCoborrower", true.ToString());
      }
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.DisplayImage(this.strongLink);
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      ((IHTMLImgElement) this.HTMLElement).src = this.getIconImageSource(interactive);
    }

    internal override string RenderHTML()
    {
      return "<img src=\"" + this.getIconImageSource(true) + "\" width=\"16\" height=\"16\" tabIndex=\"-1\" emimport=\"0\" alt=\"BorrowerLink\"" + this.GetBaseAttributes() + " onmouseover=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('.png', '-over.png'); }\" onmouseout=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('-over.png', '.png'); }\"/>";
    }

    private string getIconImageSource(bool enabled)
    {
      return this.strongLink ? (enabled ? Control.ResolveInternalImagePath("contact-link.png") : Control.ResolveInternalImagePath("contact-link-disabled.png")) : (enabled ? Control.ResolveInternalImagePath("contact-unlink.png") : Control.ResolveInternalImagePath("contact-unlink-over.png"));
    }

    public void DisplayImage(bool strongLink)
    {
      this.strongLink = strongLink;
      ((IHTMLImgElement) this.HTMLElement).src = this.getIconImageSource(true);
      this.HTMLElement.title = BorrowerLink.getHoverText(strongLink);
    }

    private static string getHoverText(bool strongLink)
    {
      int num = strongLink ? 1 : 0;
      return "Link to a Contact";
    }
  }
}
