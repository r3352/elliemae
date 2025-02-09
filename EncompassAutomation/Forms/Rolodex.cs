// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Rolodex
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Provides the ability for users to pop open the Rolodex dialog and select a contact for one
  /// or more fields on the Form.
  /// </summary>
  [ToolboxControl("Rolodex")]
  public class Rolodex : RuntimeControl
  {
    private RolodexGroup rolodexGroup;

    /// <summary>Constructor for a new Rolodex control.</summary>
    public Rolodex()
    {
    }

    internal Rolodex(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>
    /// The <see cref="P:EllieMae.Encompass.Forms.Rolodex.BusinessCategory" /> that will be opened by default when the rolodex is launched.
    /// </summary>
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

    /// <summary>
    /// Overrides the Delete method for the control, ensuring all Rolodex data is cleaned up.
    /// </summary>
    public override void Delete()
    {
      this.rolodexGroup.Delete();
      base.Delete();
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.Forms.RolodexGroup" /> associated with this control.
    /// </summary>
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
