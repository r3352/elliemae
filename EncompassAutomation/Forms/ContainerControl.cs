// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.ContainerControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Provides a base class for all controls which can contain other controls.
  /// </summary>
  /// <remarks>A Container control is used to group and maniupate sets of controls.
  /// Controls are added or removed from a container control via its <see cref="P:EllieMae.Encompass.Forms.ContainerControl.Controls" />
  /// property. Every control on the input form must belong to a container, with the exception
  /// of the <see cref="T:EllieMae.Encompass.Forms.Form" /> control, which is the top-most container control in the control
  /// hierarchy.
  /// </remarks>
  public abstract class ContainerControl : ContentControl
  {
    private ControlCollection controls;
    private IHTMLElement containerElement;
    private BorderStyle borderStyle;
    private Color borderColor = Color.Empty;
    private int borderWidth = -1;

    /// <summary>Constructor for a new Contaienr Control.</summary>
    protected ContainerControl()
    {
    }

    internal ContainerControl(Form form, IHTMLElement controlElement)
      : base(form, controlElement)
    {
    }

    /// <summary>
    /// Gets the collection of controls which are held within the container.
    /// </summary>
    [Browsable(false)]
    public ControlCollection Controls => this.controls;

    /// <summary>
    /// Gets or sets the relative position of the control within its parent container.
    /// </summary>
    [Category("Layout")]
    [Description("Determines if the control is placed within the flow of the document or can be positioned absolutely.")]
    public virtual LayoutMethod Layout
    {
      get
      {
        return !((this.HTMLElement2.currentStyle.position ?? "").ToLower() == "absolute") ? LayoutMethod.Flow : LayoutMethod.Positioned;
      }
      set
      {
        ((IHTMLStyle2) this.HTMLElement.style).position = value == LayoutMethod.Positioned ? "absolute" : "static";
        this.Form.OnMove(new FormEventArgs((Control) this));
      }
    }

    /// <summary>
    /// Indicates if this control can be cut, copied or deleted.
    /// </summary>
    public override bool AllowPositioning => this.Layout == LayoutMethod.Positioned;

    /// <summary>
    /// Gets the absolute position of the client area of the container control.
    /// </summary>
    [Browsable(false)]
    public Point ClientOffset
    {
      get
      {
        Point absolutePosition = this.AbsolutePosition;
        Point clientOffset = this.GetContainerAbsolutePosition();
        clientOffset = new Point(clientOffset.X - absolutePosition.X, clientOffset.Y - absolutePosition.Y);
        return clientOffset;
      }
    }

    /// <summary>
    /// Gets the size of the client area of the container control.
    /// </summary>
    [Browsable(false)]
    public Size ClientSize
    {
      get => new Size(this.ContainerElement.offsetWidth, this.ContainerElement.offsetHeight);
    }

    /// <summary>Gets the client rectangle for the control.</summary>
    [Browsable(false)]
    public Rectangle ClientRectangle => new Rectangle(this.ClientOffset, this.ClientSize);

    /// <summary>
    /// Forces the control and all if its child controls to reload their state.
    /// </summary>
    public override void Refresh()
    {
      foreach (Control control in this.Controls)
        control.Refresh();
      base.Refresh();
    }

    /// <summary>
    /// Tests if a point is within the client area of a control.
    /// </summary>
    /// <param name="pt">The point to be tested, expressed in absolute form coordinates.</param>
    /// <returns>Returns <c>true</c> if the point is within the client area,
    /// <c>false</c> otherwise.</returns>
    public bool ClientHitTest(Point pt) => this.ClientRectangle.Contains(this.PointToClient(pt));

    /// <summary>
    /// Converts a Point object expressed in coordinates relative to this control's
    /// client region to an absolute position in the form.
    /// </summary>
    /// <param name="pt">The point to convert.</param>
    /// <returns></returns>
    public Point PointToAbsolute(Point pt)
    {
      Point absolutePosition = this.GetContainerAbsolutePosition();
      return new Point(pt.X + absolutePosition.X, pt.Y + absolutePosition.Y);
    }

    /// <summary>
    /// Converts a Point expressed in absolute form coordinates to a point relative to
    /// the client area of the control.
    /// </summary>
    /// <param name="pt">The point to convert.</param>
    /// <returns></returns>
    public Point PointToClient(Point pt)
    {
      Point absolutePosition = this.GetContainerAbsolutePosition();
      return new Point(pt.X - absolutePosition.X, pt.Y - absolutePosition.Y);
    }

    /// <summary>
    /// Converts a Point object expressed in coordinates relative to this control
    /// to an absolute position in the form.
    /// </summary>
    /// <param name="rect">The Rectangle to convert.</param>
    /// <returns></returns>
    public Rectangle RectangleToAbsolute(Rectangle rect)
    {
      return new Rectangle(this.PointToAbsolute(rect.Location), rect.Size);
    }

    /// <summary>
    /// Converts a Point expressed in absolute form coordinates to a point relative to
    /// the client area of the control.
    /// </summary>
    /// <param name="rect">The Rectangle to convert.</param>
    /// <returns></returns>
    public Rectangle RectangleToClient(Rectangle rect)
    {
      return new Rectangle(this.PointToClient(rect.Location), rect.Size);
    }

    /// <summary>
    /// Locates a child container that includes a specified point
    /// </summary>
    /// <param name="pt">The point is an absolute position on the form.</param>
    /// <returns>Returns the most-nested container control at the specified point.</returns>
    public virtual ContainerControl GetContainerAtPoint(Point pt)
    {
      return this.GetContainerAtPointEx(pt, (IDictionary) new Hashtable());
    }

    /// <summary>
    /// Check if the form is in edit mode and make sure our setting matches
    /// </summary>
    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      if (this.Form.EditingEnabled && !this.ContentEditable)
      {
        this.OnStartEditing();
      }
      else
      {
        if (this.Form.EditingEnabled || !this.ContentEditable)
          return;
        this.OnStopEditing();
      }
    }

    internal virtual ContainerControl GetContainerAtPointEx(Point pt, IDictionary exclusionList)
    {
      foreach (Control control in this.controls)
      {
        if (control is ContainerControl containerControl && !exclusionList.Contains((object) containerControl.ControlID))
        {
          ContainerControl containerAtPointEx = containerControl.GetContainerAtPointEx(pt, exclusionList);
          if (containerAtPointEx != null)
            return containerAtPointEx;
        }
      }
      return exclusionList.Contains((object) this.ControlID) || !this.HitTest(pt) ? (ContainerControl) null : this;
    }

    internal Point GetContainerLayoutOffset(IHTMLElement offsetParent)
    {
      Point absolutePosition = this.GetContainerAbsolutePosition();
      Point absoluteElementPosition = Control.GetAbsoluteElementPosition(offsetParent);
      return new Point(absolutePosition.X - absoluteElementPosition.X, absolutePosition.Y - absoluteElementPosition.Y);
    }

    internal Point GetContainerAbsolutePosition()
    {
      return Control.GetAbsoluteElementPosition(this.ContainerElement);
    }

    internal virtual void Highlight()
    {
      this.borderStyle = this.BorderStyle;
      this.borderColor = this.BorderColor;
      this.borderWidth = this.BorderWidth;
      this.BorderStyle = BorderStyle.Solid;
      this.BorderColor = Color.DarkRed;
      this.BorderWidth = Math.Max(2, this.BorderWidth + 1);
    }

    internal virtual void RemoveHighlight()
    {
      if (this.borderWidth < 0)
        return;
      this.BorderStyle = this.borderStyle;
      this.BorderColor = this.borderColor;
      this.BorderWidth = this.borderWidth;
      this.borderWidth = -1;
    }

    internal override void OnStartEditing()
    {
      base.OnStartEditing();
      ((IHTMLElement3) this.ContainerElement).contentEditable = "true";
      foreach (Control control in this.Controls)
        control.OnStartEditing();
    }

    internal override void OnStopEditing()
    {
      ((IHTMLElement3) this.ContainerElement).contentEditable = "false";
      foreach (Control control in this.Controls)
        control.OnStopEditing();
      base.OnStopEditing();
    }

    /// <summary>Gets the ContentEditable property of the control</summary>
    internal bool ContentEditable
    {
      get => ((IHTMLElement3) this.ContainerElement).contentEditable == "true";
    }

    internal IHTMLElement ContainerElement
    {
      get
      {
        this.EnsureAttached();
        return this.containerElement;
      }
    }

    internal IHTMLElement2 ContainerElement2 => this.containerElement as IHTMLElement2;

    internal override bool ReattachRequired()
    {
      return base.ReattachRequired() || Control.IsDetached(this.containerElement);
    }

    internal override void ReattachToElement()
    {
      this.containerElement = (IHTMLElement) null;
      base.ReattachToElement();
    }

    internal override IHTMLElement FindContentElement(IHTMLElement controlElement)
    {
      if (this.containerElement != null)
        return this.containerElement;
      this.containerElement = this.FindContainerElement(controlElement);
      return this.containerElement;
    }

    internal string GetBaseContainerAttributes()
    {
      return " containerId=\"" + this.ControlID + "\" class=\"controlContainer\" contentEditable=\"true\" ";
    }

    internal override void ChangeControlID(string oldValue, string newValue)
    {
      this.ContainerElement.setAttribute("containerId", (object) newValue);
      base.ChangeControlID(oldValue, newValue);
    }

    internal override void AttachToElement(Form form, IHTMLElement controlElement)
    {
      base.AttachToElement(form, controlElement);
      if (this.containerElement == null || Control.IsDetached(this.containerElement))
        this.containerElement = this.FindContainerElement(controlElement);
      if (this.containerElement == null)
        throw new InvalidOperationException("Container element for container control " + this.ControlID + " not found.");
      this.controls = new ControlCollection(this);
    }

    internal virtual IHTMLElement FindContainerElement(IHTMLElement controlElement)
    {
      return this.FindContainerElement(controlElement, true);
    }

    internal IHTMLElement FindContainerElement(
      IHTMLElement controlElement,
      bool useControlElementAsDefault)
    {
      IHTMLElement elementWithAttribute = HTMLHelper.FindElementWithAttribute(controlElement, "containerId", this.ControlID);
      return elementWithAttribute == null & useControlElementAsDefault ? this.HTMLElement : elementWithAttribute;
    }

    internal override void ChangeControlInteractiveState(bool interactive)
    {
      base.ChangeControlInteractiveState(interactive);
      if (this.controls == null)
        return;
      foreach (Control control in this.controls)
      {
        if (control is RuntimeControl)
          ((RuntimeControl) control).OnContainerInteractiveStateChanged(interactive);
      }
    }

    internal override void ChangeControlVisibilityState(bool visible)
    {
      base.ChangeControlVisibilityState(visible);
      if (this.controls == null || !this.IsContainerVisible)
        return;
      foreach (Control control in this.controls)
      {
        if (control is RuntimeControl)
          ((RuntimeControl) control).OnContainerVisibilityStateChanged(visible);
      }
    }

    internal override void OnContainerVisibilityStateChanged(bool visible)
    {
      base.OnContainerVisibilityStateChanged(visible);
      if (this.controls == null || !this.Visible)
        return;
      foreach (Control control in this.controls)
      {
        if (control is RuntimeControl)
          ((RuntimeControl) control).OnContainerVisibilityStateChanged(visible);
      }
    }
  }
}
