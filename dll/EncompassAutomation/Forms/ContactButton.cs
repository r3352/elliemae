// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ContactButton
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Contact Button")]
  public class ContactButton : RuntimeControl
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Click"
    };
    private ScopedEventHandler<EventArgs> click = new ScopedEventHandler<EventArgs>(nameof (ContactButton), "Click");

    [Category("Behavior")]
    public event EventHandler Click
    {
      add
      {
        if (value == null)
          return;
        this.click.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.click.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public ContactButton()
    {
    }

    internal ContactButton(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    [Category("Behavior")]
    public ContactMethod ContactMethod
    {
      get => this.stringToContactMethod(string.Concat(this.HTMLElement.getAttribute("emid", 1)));
      set
      {
        this.getImageElement().src = ContactButton.getIconImageSource(value, this.Interactive);
        this.HTMLElement.setAttribute("emid", (object) this.contactMethodToString(value), 1);
        this.HTMLElement.title = ContactButton.getHoverText(value);
        this.NotifyPropertyChange();
      }
    }

    [Category("Data")]
    [Description("The field that will be used to populate both the Company information of the contact record.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor CompanyField
    {
      get => this.getAssociatedField("comp");
      set => this.setAssociatedField("comp", value);
    }

    [Category("Data")]
    [Description("The field that will be used to populate the Last Name information of the contact record.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor LastNameField
    {
      get => this.getAssociatedField("lastname");
      set => this.setAssociatedField("lastname", value);
    }

    [Category("Data")]
    [Description("The field that will be used to populate the First Name information of the contact record.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor FirstNameField
    {
      get => this.getAssociatedField("firstname");
      set => this.setAssociatedField("firstname", value);
    }

    [Category("Data")]
    [Description("The field that will be used to populate the phone number of the contact record.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor PhoneField
    {
      get => this.getAssociatedField("phone");
      set => this.setAssociatedField("phone", value);
    }

    [Category("Data")]
    [Description("The field that will be used to populate the email address of the contact record.")]
    [Editor(typeof (FieldEditor), typeof (UITypeEditor))]
    public FieldDescriptor EmailField
    {
      get => this.getAssociatedField("email");
      set => this.setAssociatedField("email", value);
    }

    [Category("Appearance")]
    [Description("The text that appears when the user hovers the mouse over the control.")]
    [Browsable(false)]
    public virtual string HoverText
    {
      get => this.HTMLElement.title ?? "";
      set
      {
      }
    }

    [Browsable(false)]
    public string[] SupportedEvents => ContactButton.supportedEvents;

    public bool InvokeClick()
    {
      this.OnClick(EventArgs.Empty);
      return true;
    }

    protected virtual void OnClick(EventArgs e) => this.click((object) this, e);

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.getImageElement().src = ContactButton.getIconImageSource(this.ContactMethod, this.Interactive);
      this.HTMLElement.title = ContactButton.getHoverText(this.ContactMethod);
    }

    private FieldDescriptor getAssociatedField(string attrName)
    {
      return this.Form.GetFieldDescriptor(this.GetAttribute(attrName));
    }

    private void setAssociatedField(string attrName, FieldDescriptor field)
    {
      this.HTMLElement.setAttribute(attrName, (object) field.FieldID, 1);
      this.NotifyPropertyChange();
    }

    internal override string RenderHTML()
    {
      return "<button" + this.GetBaseAttributes(false) + " tabIndex=\"-1\" hideFocus=\"true\" class=\"inputButtonImage\"><img src=\"" + ContactButton.getIconImageSource(ContactMethod.Phone, true) + "\" width=\"16\" height=\"16\" emimport=\"0\" onmouseover=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('.png', '-over.png'); }\" onmouseout=\"if (this.src.indexOf('-disabled') < 0) { this.src = this.src.replace('-over.png', '.png'); }\"></button>";
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      this.getImageElement().src = ContactButton.getIconImageSource(this.ContactMethod, interactive);
    }

    private IHTMLImgElement getImageElement()
    {
      return (IHTMLImgElement) (HTMLHelper.GetChildWithTagName(this.HTMLElement, "img") ?? throw new Exception("The interior image element could not be found"));
    }

    private static string getIconImageSource(ContactMethod method, bool enabled)
    {
      string imageName;
      switch (method)
      {
        case ContactMethod.Phone:
          imageName = enabled ? "phone.png" : "phone-disabled.png";
          break;
        case ContactMethod.Email:
          imageName = enabled ? "email.png" : "email-disabled.png";
          break;
        case ContactMethod.Cell:
          imageName = enabled ? "cell-phone.png" : "cell-phone-disabled.png";
          break;
        case ContactMethod.Fax:
          imageName = enabled ? "fax.png" : "fax-disabled.png";
          break;
        default:
          throw new ArgumentException("Invalid method specified");
      }
      return Control.ResolveInternalImagePath(imageName);
    }

    private static string getHoverText(ContactMethod method)
    {
      return method == ContactMethod.Email ? "Open the Conversation Log and Compose an Email" : "Open the Conversation Log";
    }

    private string contactMethodToString(ContactMethod method)
    {
      switch (method)
      {
        case ContactMethod.Email:
          return "email";
        case ContactMethod.Cell:
          return "cell";
        case ContactMethod.Fax:
          return "fax";
        default:
          return "phone";
      }
    }

    private ContactMethod stringToContactMethod(string method)
    {
      switch (method.ToLower())
      {
        case "email":
          return ContactMethod.Email;
        case "fax":
          return ContactMethod.Fax;
        case "cell":
          return ContactMethod.Cell;
        default:
          return ContactMethod.Phone;
      }
    }
  }
}
