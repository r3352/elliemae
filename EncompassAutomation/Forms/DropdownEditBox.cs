// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownEditBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.ComponentModel;
using JEDCOMBO2Lib;
using mshtml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Provides a control from which the user may select a pre-defined option or enter their own value.
  /// </summary>
  /// <remarks>The DropdownEditBox is similar to a DropdownBox but allows the user to type in a custom
  /// value which is not in the options list.</remarks>
  [ToolboxControl("Dropdown Edit Box")]
  public class DropdownEditBox : DropdownBox, ISupportsFormatEvent, ISupportsEvents
  {
    private static readonly string[] supportedEvents = new string[1]
    {
      "Format"
    };
    private ScopedEventHandler<FormatEventArgs> format = new ScopedEventHandler<FormatEventArgs>(nameof (DropdownEditBox), "Format");
    private bool isInteractive = true;
    private PickList boundList;

    /// <summary>
    /// The Format event is fired as the user types into the control to allow on-the-fly formatting
    /// of the entered value.
    /// </summary>
    [Category("Appearance")]
    public event FormatEventHandler Format
    {
      add
      {
        if (value == null)
          return;
        this.format.Add(new ScopedEventHandler<FormatEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.format.Remove(new ScopedEventHandler<FormatEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>Constructor for a new DropdownEditBox control.</summary>
    public DropdownEditBox()
    {
    }

    internal DropdownEditBox(Form form, mshtml.IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>
    /// Gets or sets the loan field associated with the control.
    /// </summary>
    public override FieldDescriptor Field
    {
      get => base.Field;
      set
      {
        base.Field = value;
        if (!value.Options.RequireValueFromList)
          return;
        this.Enabled = false;
      }
    }

    /// <summary>
    /// Gets or sets whether the control is enabled and permits user interaction.
    /// </summary>
    public override bool Enabled
    {
      get => base.Enabled;
      set
      {
        if (value == this.Enabled)
          return;
        if (value && this.Field.Options.RequireValueFromList)
          throw new ArgumentException("This control cannot be enabled because the underlying field can only take on pre-defined values.");
        base.Enabled = value;
      }
    }

    /// <summary>Override the background color of the control.</summary>
    /// <remarks>The DropdownEdit box does not support custom background colors. Attempting
    /// to set this property will result in an exception.</remarks>
    [Browsable(false)]
    public override Color BackColor
    {
      get => Color.White;
      set
      {
        throw new NotSupportedException("This property is not supported on the current control.");
      }
    }

    /// <summary>
    /// Indicates if the control supports setting the BackColor property.
    /// </summary>
    /// <remarks>This property will always return <c>false</c> for this object.</remarks>
    public override bool SupportsBackColor => false;

    /// <summary>Override the text color of the control.</summary>
    /// <remarks>The DropdownEdit box does not support custom foreground colors. Attempting
    /// to set this property will result in an exception.</remarks>
    [Browsable(false)]
    public override Color ForeColor
    {
      get => Color.Black;
      set
      {
        throw new NotSupportedException("This property is not supported on the current control.");
      }
    }

    /// <summary>
    /// Indicates if the control supports setting the ForeColor property.
    /// </summary>
    /// <remarks>This property will always return <c>false</c> for this object.</remarks>
    public override bool SupportsForeColor => false;

    /// <summary>Override the font used within the control.</summary>
    /// <remarks>The DropdownEdit box does not support custom font selections. Attempting
    /// to set this property will result in an exception.</remarks>
    [Browsable(false)]
    public override HTMLFont Font
    {
      get => new HTMLFont("Arial", 10, false, false, false);
      set
      {
        throw new NotSupportedException("This property is not supported on the current control.");
      }
    }

    /// <summary>
    /// Indicates if the control supports setting the Font property.
    /// </summary>
    /// <remarks>This property will always return <c>false</c> for this object.</remarks>
    public override bool SupportsFont => false;

    /// <summary>
    /// Gets the current value of the checkbox based on the checked state of the object.
    /// </summary>
    [Browsable(false)]
    public string Text
    {
      get => this.GetObject().Text;
      set => this.ApplyValueToControl(value);
    }

    /// <summary>
    /// Gets the current value of the checkbox based on the checked state of the object.
    /// </summary>
    [Browsable(false)]
    public override string Value
    {
      get
      {
        int curSel = (int) this.GetObject().CurSel;
        string text = this.GetObject().Text;
        if (text == null)
          return "";
        if (curSel < 0)
          return text;
        return curSel < this.Options.Count ? this.Options[curSel].Value : "";
      }
    }

    internal override void DisplayValue(string value) => this.GetObject().Text = value ?? "";

    internal override void HideValue() => this.DisplayValue("*");

    /// <summary>Returns the list of events supported by this control.</summary>
    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, DropdownEditBox.supportedEvents);
    }

    /// <summary>Invokes the Format event on the control.</summary>
    /// <param name="value">The value to be formatted.</param>
    /// <returns>Returns <c>true</c> if auto-formatting should be applied, <c>false</c> otherwise.</returns>
    /// <remarks>This method is meant for use by the Encompass Input Engine only.</remarks>
    /// <exclude />
    public bool InvokeFormat(ref string value)
    {
      FormatEventArgs e = new FormatEventArgs(value);
      this.OnFormat(e);
      value = e.Value;
      return !e.Cancel;
    }

    /// <summary>
    /// Raises the <see cref="E:EllieMae.Encompass.Forms.DropdownEditBox.Format" /> event.
    /// </summary>
    /// <param name="e">The event parameters passed to the event handlers.</param>
    protected virtual void OnFormat(FormatEventArgs e) => this.format.Invoke((object) this, e);

    /// <summary>Performs the pre-display processing for the control</summary>
    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.HTMLElement.style.height = (object) "22px";
    }

    internal override void OnStartEditing()
    {
      try
      {
        mshtml.IHTMLElement htmlElement = this.HTMLElement;
        if (htmlElement.tagName.ToUpper() != "OBJECT")
          return;
        mshtml.IHTMLElement element = this.GetHTMLDocument().createElement("select");
        this.duplicateAttributes(htmlElement, element);
        element.setAttribute("emid", (object) this.findParameterOfObject("emid"));
        DropdownOption[] objectOptionsList = this.parseObjectOptionsList();
        mshtml.IHTMLDOMNode oldChild = htmlElement as mshtml.IHTMLDOMNode;
        mshtml.IHTMLDOMNode newChild = element as mshtml.IHTMLDOMNode;
        oldChild.parentNode.replaceChild(newChild, oldChild);
        this.AttachToElement(this.Form, element);
        this.PrepareForDisplay();
        this.RenderOptionsList(objectOptionsList);
        this.Enabled = this.Enabled;
      }
      finally
      {
        base.OnStartEditing();
      }
    }

    internal override void OnStopEditing()
    {
      try
      {
        mshtml.IHTMLElement htmlElement = this.HTMLElement;
        if (htmlElement.tagName.ToUpper() == "OBJECT")
          return;
        mshtml.IHTMLDOMNode oldChild = htmlElement as mshtml.IHTMLDOMNode;
        mshtml.IHTMLElement element = this.GetHTMLDocument().createElement("OBJECT");
        mshtml.IHTMLDOMNode newChild = element as mshtml.IHTMLDOMNode;
        this.duplicateAttributes(htmlElement, element);
        element.setAttribute("classid", (object) "CLSID:8B79EBAC-6294-4B7E-BD98-334297D59945");
        this.addParameterToObject(element, "listonly", "no");
        if (htmlElement.getAttribute("emid", 1) != null)
          this.addParameterToObject(element, "emid", string.Concat(htmlElement.getAttribute("emid", 1)));
        ArrayList arrayList = new ArrayList();
        foreach (DropdownOption options in this.ParseOptionsList())
          arrayList.Add((object) ("<" + this.parameterEncode(options.Value) + "!" + this.parameterEncode(options.Text) + ">"));
        string paramValue = string.Join(",", (string[]) arrayList.ToArray(typeof (string)));
        this.addParameterToObject(element, "ValNames", paramValue);
        this.addParameterToObject(element, "TEXT", "");
        oldChild.parentNode.replaceChild(newChild, oldChild);
        this.AttachToElement(this.Form, element);
        this.PrepareForDisplay();
      }
      finally
      {
        base.OnStopEditing();
      }
    }

    private void duplicateAttributes(mshtml.IHTMLElement curElement, mshtml.IHTMLElement newElement)
    {
      this.copyAttribute(newElement, curElement, "tabIndex");
      this.copyAttribute(newElement, curElement, "height");
      this.copyAttribute(newElement, curElement, "width");
      this.copyAttribute(newElement, curElement, "id");
      this.copyAttribute(newElement, curElement, "fieldId");
      this.copyAttribute(newElement, curElement, "controlType");
      this.copyAttribute(newElement, curElement, "emdisabled");
      this.copyAttribute(newElement, curElement, "emvisible");
      this.copyAttribute(newElement, curElement, "emsrc");
      IHTMLStyle2 style1 = curElement.style as IHTMLStyle2;
      IHTMLStyle2 style2 = newElement.style as IHTMLStyle2;
      if (style1.position != null)
        style2.position = style1.position;
      if (curElement.style.left != null)
        newElement.style.left = curElement.style.left;
      if (curElement.style.width != null)
        newElement.style.width = curElement.style.width;
      newElement.style.height = curElement.style.height == null ? (object) "22px" : curElement.style.height;
      if (curElement.style.top == null)
        return;
      newElement.style.top = curElement.style.top;
    }

    private void copyAttribute(mshtml.IHTMLElement newElement, mshtml.IHTMLElement curElement, string attrName)
    {
      string AttributeValue = string.Concat(curElement.getAttribute(attrName, 1));
      if (!((AttributeValue ?? "") != ""))
        return;
      newElement.setAttribute(attrName, (object) AttributeValue);
    }

    private string findParameterOfObject(string paramName)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(Html2XHtml.Convert("<html>" + this.HTMLElement.innerHTML + "</html>", false, false));
      foreach (XmlElement selectNode in xmlDocument.SelectNodes("//param"))
      {
        if ((selectNode.GetAttribute("name") ?? "").ToLower() == paramName.ToLower())
          return selectNode.GetAttribute("value") ?? "";
      }
      return "";
    }

    private void addParameterToObject(
      mshtml.IHTMLElement objectElement,
      string paramName,
      string paramValue)
    {
      HTMLParamElement element = this.GetHTMLDocument().createElement("PARAM") as HTMLParamElement;
      element.name = paramName;
      element.value = paramValue;
      (objectElement as HTMLObjectElement).appendChild(element as mshtml.IHTMLDOMNode);
    }

    internal override string RenderHTML()
    {
      return "<select size=\"1\" style=\"width: 200px; height: 22px\" height=\"22\" class=\"inputSelect\"" + this.GetBaseAttributes() + this.GetBaseFieldAttributes() + ">" + Environment.NewLine + "</select>";
    }

    internal override DropdownOptionCollection CreateOptionsCollection()
    {
      return new DropdownOptionCollection((IDropdownOptionContainer) new DropdownBox.DropdownBoxOptionContainer((DropdownBox) this), false);
    }

    internal override DropdownOption[] ParseOptionsList()
    {
      return this.Form.EditingEnabled ? base.ParseOptionsList() : this.parseObjectOptionsList();
    }

    internal override void RenderOptionsList(DropdownOption[] options)
    {
      if (!this.Form.EditingEnabled)
        throw new InvalidOperationException("The DropdownEditBox control does not support changes to its options list at runtime");
      base.RenderOptionsList(options);
    }

    /// <summary>
    /// Refreshes the properties of the control based on the current field information.
    /// </summary>
    public override void RefreshProperties()
    {
      if (!this.Form.EditingEnabled)
        return;
      base.RefreshProperties();
    }

    private DropdownOption[] parseObjectOptionsList()
    {
      Match match = new Regex("<([^!]*)!([^>]*)>").Match(this.findParameterOfObject("ValNames"));
      ArrayList arrayList = new ArrayList();
      for (; match.Success; match = match.NextMatch())
        arrayList.Add((object) new DropdownOption(this.parameterDecode(match.Groups[2].Value), this.parameterDecode(match.Groups[1].Value)));
      return (DropdownOption[]) arrayList.ToArray(typeof (DropdownOption));
    }

    private string parameterDecode(string text)
    {
      return new Regex("%([0-9a-f]{2})", RegexOptions.IgnoreCase).Replace(text, new MatchEvaluator(this.onDecoderMatch));
    }

    private string onDecoderMatch(Match m)
    {
      try
      {
        return Encoding.ASCII.GetChars(new byte[1]
        {
          byte.Parse(m.Groups[1].Value, NumberStyles.HexNumber)
        })[0].ToString() ?? "";
      }
      catch
      {
        return m.Value;
      }
    }

    private string parameterEncode(string text)
    {
      text = text.Replace("%", "%25");
      text = text.Replace(",", "%2C");
      text = text.Replace("!", "%21");
      text = text.Replace("<", "%3C");
      text = text.Replace(">", "%3E");
      return text == "" ? " " : text;
    }

    internal static bool IsDropdownEditBox(mshtml.IHTMLElement theElement)
    {
      return !(theElement.tagName.ToLower() != "object") && !(string.Concat(theElement.getAttribute("classid", 1)).ToUpper() != "CLSID:8B79EBAC-6294-4B7E-BD98-334297D59945");
    }

    /// <summary>
    /// Gets a flag indicating if the current control is interactive.
    /// </summary>
    /// <remarks>An interactive control is one that accepts input from a user. A control can be enabled
    /// but not interactive if it is contained in a <see cref="T:EllieMae.Encompass.Forms.ContainerControl" /> which is disabled.
    /// </remarks>
    public override bool Interactive => base.Interactive && this.isInteractive;

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      if (this.Form.EditingEnabled)
      {
        base.ChangeControlInteractiveState(interactive);
      }
      else
      {
        if (interactive == this.isInteractive)
          return;
        if (System.Windows.Forms.Form.ActiveForm != null)
          ThreadPool.QueueUserWorkItem(new WaitCallback(this.enableDisableControl), (object) interactive);
        else
          this.enableDisableControl((object) interactive);
        this.isInteractive = interactive;
      }
    }

    private void enableDisableControl(object newValue)
    {
      if (System.Windows.Forms.Form.ActiveForm != null)
      {
        if (System.Windows.Forms.Form.ActiveForm.InvokeRequired)
        {
          try
          {
            System.Windows.Forms.Form.ActiveForm.Invoke((Delegate) new WaitCallback(this.enableDisableControl), newValue);
            return;
          }
          catch
          {
          }
        }
      }
      try
      {
        if (this.Form.EditingEnabled)
          base.ChangeControlInteractiveState((bool) newValue);
        else if ((bool) newValue)
          this.GetObject().Enable();
        else
          this.GetObject().Disable();
      }
      catch
      {
      }
    }

    /// <summary>
    /// Creates a PickList control which is bound to the current textbox.
    /// </summary>
    /// <param name="listItems">The list of items to appear in the PickList.</param>
    /// <returns>The PickList object created for the control.</returns>
    public PickList CreatePickList(string[] listItems)
    {
      if (this.boundList == null)
      {
        this.boundList = new PickList();
        this.boundList.UseForEditableDropdownField = true;
        this.GetContainer().Controls.Insert((Control) this.boundList);
        this.boundList.BindToControl((FieldControl) this);
        if (this.Field != FieldDescriptor.Empty)
          this.boundList.Title = "Field: " + this.Field.FieldID;
        Point absolutePosition = this.AbsolutePosition;
        Size size = this.Size;
        this.Size = new Size(Math.Max(size.Width - this.boundList.Size.Width - 1, 0), size.Height);
        this.boundList.AbsolutePosition = new Point(this.Bounds.Right, absolutePosition.Y - 2);
      }
      this.Enabled = false;
      this.boundList.Options.Clear();
      DropdownOption[] optionList = new DropdownOption[listItems.Length];
      for (int index = 0; index < listItems.Length; ++index)
        optionList[index] = new DropdownOption(listItems[index]);
      this.boundList.Options.AddRange((ICollection) optionList);
      return this.boundList;
    }

    private IComboFull GetObject() => (IComboFull) ((IHTMLObjectElement) this.HTMLElement).@object;
  }
}
