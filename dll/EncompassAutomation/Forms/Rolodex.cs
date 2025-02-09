// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Rolodex
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Rolodex")]
  public class Rolodex : RuntimeControl
  {
    private RolodexGroup rolodexGroup;

    public Rolodex()
    {
    }

    internal Rolodex(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    [Category("Data")]
    [Editor(typeof (BizCategorySelector), typeof (UITypeEditor))]
    [TypeConverter(typeof (NonExpandableControlTypeConverter))]
    public BizCategory BusinessCategory
    {
      get => this.rolodexGroup.BusinessCategory;
      set
      {
        this.rolodexGroup.BusinessCategory = value;
        this.NotifyPropertyChange();
      }
    }

    public override void Delete()
    {
      this.rolodexGroup.Delete();
      base.Delete();
    }

    [Browsable(false)]
    public RolodexGroup Group => this.rolodexGroup;

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      RolodexGroups rolodexGroups = this.Form.RolodexGroups;
      this.rolodexGroup = rolodexGroups.GetGroupByName(this.ControlID);
      if (this.rolodexGroup != null)
        return;
      this.rolodexGroup = rolodexGroups.CreateNew(this.ControlID);
    }

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      ((IHTMLImgElement) this.HTMLElement).src = this.getIconImageSource(this.Interactive);
      this.HTMLElement.title = "Select a Business Contact";
    }

    internal override void ChangeControlID(string oldValue, string newValue)
    {
      this.rolodexGroup.ChangeRolodexName(newValue);
      base.ChangeControlID(oldValue, newValue);
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      ((IHTMLImgElement) this.HTMLElement).src = this.getIconImageSource(interactive);
    }

    internal override string RenderHTML()
    {
      return "<img src=\"" + this.getIconImageSource(true) + "\" width=\"16\" height=\"16\" tabIndex=\"-1\" emimport=\"0\" alt=\"Rolodex\"" + this.GetBaseAttributes() + " onmouseover=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('.png', '-over.png'); }\" onmouseout=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('-over.png', '.png'); }\"/>";
    }

    private string getIconImageSource(bool enabled)
    {
      return enabled ? Control.ResolveInternalImagePath("address-book.png") : Control.ResolveInternalImagePath("address-book-disabled.png");
    }
  }
}
