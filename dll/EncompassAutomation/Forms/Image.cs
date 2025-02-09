// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Image
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.Encompass.ComponentModel;
using EllieMae.Encompass.Forms.Design;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing.Design;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  [ToolboxControl("Image")]
  public class Image : RuntimeControl
  {
    private HTMLImg imageElement;

    public Image()
    {
    }

    internal Image(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
      string str = Control.SpanContentEditable();
      if (string.IsNullOrWhiteSpace(str))
        return;
      controlElement.setAttribute("contentEditable", (object) str, 1);
    }

    [Browsable(false)]
    public override bool Enabled
    {
      get => base.Enabled;
      set => base.Enabled = value;
    }

    [Category("Appearance")]
    [Editor(typeof (FileOpenTypeEditor), typeof (UITypeEditor))]
    public string Source
    {
      get => string.Concat(((IHTMLElement) this.ImageElement).getAttribute("src", 2));
      set
      {
        ((DispHTMLImg) this.ImageElement).src = value;
        this.NotifyPropertyChange();
      }
    }

    [Category("Appearance")]
    public bool Stretch
    {
      get
      {
        return this.HTMLElement == this.ImageElement || string.Concat(((DispHTMLImg) this.ImageElement).style.width) == "100%";
      }
      set
      {
        if (this.HTMLElement == this.ImageElement)
          return;
        ((DispHTMLImg) this.ImageElement).style.width = value ? (object) "100%" : (object) "";
        ((DispHTMLImg) this.ImageElement).style.height = value ? (object) "100%" : (object) "";
        this.NotifyPropertyChange();
      }
    }

    internal HTMLImg ImageElement
    {
      get
      {
        if (Control.IsDetached(this.imageElement as IHTMLElement))
          this.imageElement = this.FindImageElement();
        return this.imageElement;
      }
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      this.imageElement = this.FindImageElement();
    }

    internal virtual HTMLImg FindImageElement()
    {
      if (this.HTMLElement.tagName.ToLower() == "img")
        return this.HTMLElement as HTMLImg;
      foreach (IHTMLElement child in (IHTMLElementCollection) this.HTMLElement.children)
      {
        if (child.tagName.ToLower() == "img")
          return child as HTMLImg;
      }
      throw new InvalidOperationException("Image element not found");
    }

    internal override string RenderHTML()
    {
      return "<span" + this.GetBaseAttributes() + "style=\"width: 50px; height: 50px\"><img src=\"\" style=\"width: 100%; height: 100%\"/></span>";
    }
  }
}
