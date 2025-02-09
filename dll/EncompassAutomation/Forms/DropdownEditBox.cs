// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DropdownEditBox
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

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

    [Category("Appearance")]
    public event FormatEventHandler Format
    {
      add
      {
        if (value == null)
          return;
        this.format.Add(new ScopedEventHandler<FormatEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.format.Remove(new ScopedEventHandler<FormatEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public DropdownEditBox()
    {
    }

    internal DropdownEditBox(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

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

    [Browsable(false)]
    public override Color BackColor
    {
      get => Color.White;
      set
      {
        throw new NotSupportedException("This property is not supported on the current control.");
      }
    }

    public override bool SupportsBackColor => false;

    [Browsable(false)]
    public override Color ForeColor
    {
      get => Color.Black;
      set
      {
        throw new NotSupportedException("This property is not supported on the current control.");
      }
    }

    public override bool SupportsForeColor => false;

    [Browsable(false)]
    public override HTMLFont Font
    {
      get => new HTMLFont("Arial", 10, false, false, false);
      set
      {
        throw new NotSupportedException("This property is not supported on the current control.");
      }
    }

    public override bool SupportsFont => false;

    [Browsable(false)]
    public string Text
    {
      get => this.GetObject().Text;
      set => this.ApplyValueToControl(value);
    }

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

    public override string[] SupportedEvents
    {
      get => Control.MergeEvents(base.SupportedEvents, DropdownEditBox.supportedEvents);
    }

    public bool InvokeFormat(ref string value)
    {
      FormatEventArgs e = new FormatEventArgs(value);
      this.OnFormat(e);
      value = e.Value;
      return !e.Cancel;
    }

    protected virtual void OnFormat(FormatEventArgs e) => this.format((object) this, e);

    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      this.HTMLElement.style.height = (object) "22px";
    }

    internal override void OnStartEditing()
    {
      try
      {
        IHTMLElement htmlElement = this.HTMLElement;
        if (htmlElement.tagName.ToUpper() != "OBJECT")
          return;
        IHTMLElement element = ((DispHTMLDocument) this.GetHTMLDocument()).createElement("select");
        this.duplicateAttributes(htmlElement, element);
        element.setAttribute("emid", (object) this.findParameterOfObject("emid"), 1);
        DropdownOption[] objectOptionsList = this.parseObjectOptionsList();
        IHTMLDOMNode ihtmldomNode1 = htmlElement as IHTMLDOMNode;
        IHTMLDOMNode ihtmldomNode2 = element as IHTMLDOMNode;
        ihtmldomNode1.parentNode.replaceChild(ihtmldomNode2, ihtmldomNode1);
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
        IHTMLElement htmlElement = this.HTMLElement;
        if (htmlElement.tagName.ToUpper() == "OBJECT")
          return;
        IHTMLDOMNode ihtmldomNode1 = htmlElement as IHTMLDOMNode;
        IHTMLElement element = ((DispHTMLDocument) this.GetHTMLDocument()).createElement("OBJECT");
        IHTMLDOMNode ihtmldomNode2 = element as IHTMLDOMNode;
        this.duplicateAttributes(htmlElement, element);
        element.setAttribute("classid", (object) "CLSID:8B79EBAC-6294-4B7E-BD98-334297D59945", 1);
        this.addParameterToObject(element, "listonly", "no");
        if (htmlElement.getAttribute("emid", 1) != null)
          this.addParameterToObject(element, "emid", string.Concat(htmlElement.getAttribute("emid", 1)));
        ArrayList arrayList = new ArrayList();
        foreach (DropdownOption options in this.ParseOptionsList())
          arrayList.Add((object) ("<" + this.parameterEncode(options.Value) + "!" + this.parameterEncode(options.Text) + ">"));
        string paramValue = string.Join(",", (string[]) arrayList.ToArray(typeof (string)));
        this.addParameterToObject(element, "ValNames", paramValue);
        this.addParameterToObject(element, "TEXT", "");
        ihtmldomNode1.parentNode.replaceChild(ihtmldomNode2, ihtmldomNode1);
        this.AttachToElement(this.Form, element);
        this.PrepareForDisplay();
      }
      finally
      {
        base.OnStopEditing();
      }
    }

    private void duplicateAttributes(IHTMLElement curElement, IHTMLElement newElement)
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

    private void copyAttribute(IHTMLElement newElement, IHTMLElement curElement, string attrName)
    {
      string str = string.Concat(curElement.getAttribute(attrName, 1));
      if (!((str ?? "") != ""))
        return;
      newElement.setAttribute(attrName, (object) str, 1);
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
      IHTMLElement objectElement,
      string paramName,
      string paramValue)
    {
      HTMLParamElement element = ((DispHTMLDocument) this.GetHTMLDocument()).createElement("PARAM") as HTMLParamElement;
      ((DispHTMLParamElement) element).name = paramName;
      ((DispHTMLParamElement) element).value = paramValue;
      ((DispHTMLObjectElement) (objectElement as HTMLObjectElement)).appendChild(element as IHTMLDOMNode);
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

    internal static bool IsDropdownEditBox(IHTMLElement theElement)
    {
      return !(theElement.tagName.ToLower() != "object") && !(string.Concat(theElement.getAttribute("classid", 1)).ToUpper() != "CLSID:8B79EBAC-6294-4B7E-BD98-334297D59945");
    }

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
