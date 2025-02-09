// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Control
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using mshtml;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Represents a Control used on an Encompass Input Form.</summary>
  /// <remarks>
  /// <p>The Control class represents the base class for all controls on an Encompass input form.
  /// It provides the basic functionality common to all control types, including both
  /// runtime controls (such as textboxes, labels, etc.) and designer controls (those used to
  /// help design the form but invisible at runtime).</p>
  /// <p>When a control is constructed at runtime, its properties are generally inaccessible until
  /// the control has been added to the Form. At that point you can set the control's properties
  /// to have the desired style and binding to the underlying loan.</p>
  /// </remarks>
  public abstract class Control : Component, IComparable
  {
    internal static readonly Control Empty = (Control) new EmptyControl();
    private static readonly Hashtable controlTypeCache = new Hashtable();
    private bool forceComRelease;
    private IHTMLElement controlElement;
    private string controlId;
    private Point contextMenuPosition = Point.Empty;
    private Form parentForm;

    /// <summary>Default constructor for a control.</summary>
    public Control()
    {
    }

    internal Control(Form form, IHTMLElement controlElement)
    {
      this.AttachToElement(form, controlElement);
    }

    internal Control(string controlId) => this.controlId = controlId;

    /// <summary>Gets or sets the unique ID for the Control.</summary>
    /// <remarks>The control ID must be unique on the Form. If you do not assign a control ID
    /// to a new control, one will be automatically assigned when the control is added to the Form.
    /// </remarks>
    [Category("Control")]
    public virtual string ControlID
    {
      get => this.controlId;
      set
      {
        if (value == this.controlId)
          return;
        if (!Control.IsValidControlID(value))
          throw new ArgumentException("A control ID may only contain numbers, letters and the underscore character, and must begin with a letter.");
        if (this.Form == null)
        {
          this.controlId = value;
        }
        else
        {
          string controlId = this.controlId;
          Control control = this.Form.FindControl(value);
          if (control != null && control != this)
            throw new ArgumentException("A control with the specified ID already exists.");
          this.ChangeControlID(this.controlId, value);
          this.Form.OnControlIDChanged(this, controlId);
        }
      }
    }

    /// <summary>
    /// Controls the relative position of the control within its parent container.
    /// </summary>
    [Category("Layout")]
    [Description("Controls the relative position of the control within its parent control's container region.")]
    public virtual Point Position
    {
      get
      {
        Point absolutePosition = this.AbsolutePosition;
        ContainerControl container = this.GetContainer();
        return container == null ? absolutePosition : container.PointToClient(absolutePosition);
      }
      set
      {
        ContainerControl container = this.GetContainer();
        if (container != null)
        {
          Point containerLayoutOffset = container.GetContainerLayoutOffset(this.HTMLElement.offsetParent);
          value.Offset(containerLayoutOffset.X, containerLayoutOffset.Y);
        }
        this.HTMLElement.style.left = (object) (value.X.ToString() + "px");
        this.HTMLElement.style.top = (object) (value.Y.ToString() + "px");
        this.Form.OnMove(new FormEventArgs(this));
      }
    }

    /// <summary>
    /// Gets or sets the position of the left edge of the control relative to its container's
    /// client area.
    /// </summary>
    [Browsable(false)]
    public int Left
    {
      get => this.Position.X;
      set => this.Position = new Point(value, this.Position.Y);
    }

    /// <summary>
    /// Gets or sets the position of the right edge of the control relative to its container's
    /// client area.
    /// </summary>
    [Browsable(false)]
    public int Right
    {
      get => this.Position.X + this.Size.Width - 1;
      set => this.Position = new Point(value - this.Size.Width + 1, this.Position.Y);
    }

    /// <summary>
    /// Gets or sets the position of the top edge of the control relative to its container's
    /// client area.
    /// </summary>
    [Browsable(false)]
    public int Top
    {
      get => this.Position.Y;
      set => this.Position = new Point(this.Position.X, value);
    }

    /// <summary>
    /// Gets or sets the position of the bottom edge of the control relative to its container's
    /// client area.
    /// </summary>
    [Browsable(false)]
    public int Bottom
    {
      get => this.Position.Y + this.Size.Height - 1;
      set => this.Position = new Point(this.Position.X, value - this.Size.Height + 1);
    }

    /// <summary>
    /// Controls the absolute position of the element within the document.
    /// </summary>
    [Browsable(false)]
    [Description("Controls the absolute position of the control within the form.")]
    public Point AbsolutePosition
    {
      get => Control.GetAbsoluteElementPosition(this.HTMLElement);
      set
      {
        ContainerControl container = this.GetContainer();
        if (container != null)
        {
          Point absolutePosition = container.GetContainerAbsolutePosition();
          value.Offset(-absolutePosition.X, -absolutePosition.Y);
        }
        this.Position = value;
      }
    }

    /// <summary>Gets or sets the size of a control.</summary>
    /// <remarks>The size specified by this property is measured in pixels.</remarks>
    [Category("Layout")]
    [Description("The width and height of the control, in pixels.")]
    public virtual Size Size
    {
      get => new Size(this.HTMLElement.offsetWidth, this.HTMLElement.offsetHeight);
      set
      {
        Size size = this.AdjustSize(value);
        this.HTMLElement.style.width = (object) (Math.Max(size.Width, 1).ToString() + "px");
        this.HTMLElement.style.height = (object) (Math.Max(size.Height, 1).ToString() + "px");
        this.Form.OnResize(new FormEventArgs(this));
      }
    }

    /// <summary>Gets the Rectangle that bounds this control.</summary>
    [Browsable(false)]
    public Rectangle Bounds => new Rectangle(this.AbsolutePosition, this.Size);

    /// <summary>
    /// Controls the stacking order of the control. Controls with high ZIndex appear on top
    /// over controls with lower ZIndex.
    /// </summary>
    [Browsable(false)]
    [Description("Controls the stacking order of the control. Controls with high ZIndex appear on top over controls with lower ZIndex.")]
    public string ZIndex
    {
      get
      {
        try
        {
          return int.Parse(this.HTMLElement2.currentStyle.zIndex.ToString()).ToString();
        }
        catch
        {
          return "";
        }
      }
      set
      {
        try
        {
          this.HTMLElement.style.zIndex = (object) int.Parse(value);
        }
        catch
        {
          this.HTMLElement.style.zIndex = (object) "auto";
        }
        this.NotifyPropertyChange();
      }
    }

    /// <summary>Retrieves the value for a custom attribute</summary>
    /// <param name="name">The name of the custom attribute to retrieve</param>
    /// <returns>The value of the attribute, or an empty string if it does not exist.</returns>
    public virtual string GetCustomAttribute(string name) => this.GetAttribute(name);

    /// <summary>
    /// Indicates if an absolute position on the form is contained within the bounding
    /// rectangle of this control.
    /// </summary>
    /// <param name="p">The Point to test</param>
    /// <returns>Returns <c>true</c> if the point is within the control,
    /// <c>false</c> otherwise.</returns>
    public bool HitTest(Point p) => this.Bounds.Contains(p);

    /// <summary>
    /// Returns the container control for the current element.
    /// </summary>
    /// <returns>Null if the current element is the Form.</returns>
    public ContainerControl GetContainer()
    {
      try
      {
        return this.HTMLElement.parentElement == null ? (ContainerControl) null : (ContainerControl) this.Form.FindControlForElement(this.HTMLElement.parentElement);
      }
      catch (InvalidCastException ex)
      {
        throw new Exception("The control '" + this.controlId + "' has an invalid parent container object");
      }
    }

    /// <summary>Forces the control to reload it's state.</summary>
    public virtual void Refresh()
    {
      if (this.Form.FormScreen == null)
        return;
      this.Form.FormScreen.RefreshControl(this.ControlID);
    }

    /// <summary>Deletes the current control.</summary>
    public virtual void Delete()
    {
      if (!this.AllowCutCopyDelete)
        throw new InvalidOperationException("Control cannot be deleted");
      if (this.Form.EditingEnabled)
        this.deleteEvents();
      Control control = this.Form.FindControl(this.ControlID);
      this.GetContainer().Controls.Remove(control);
      this.controlElement = (IHTMLElement) null;
    }

    /// <summary>
    /// Indicates if this control can be cut, copied or deleted.
    /// </summary>
    [Browsable(false)]
    public virtual bool AllowCutCopyDelete => true;

    /// <summary>
    /// Indicates if this control can be cut, copied or deleted.
    /// </summary>
    [Browsable(false)]
    public virtual bool AllowPositioning => true;

    /// <summary>
    /// Indicates if the control should show up in the designer so it's properties can
    /// be manipulated directly.
    /// </summary>
    [Browsable(false)]
    public virtual bool AllowDesign => true;

    /// <summary>
    /// Returns the underlying HTML element for the control. This method is intended
    /// for use within Encompass only.
    /// </summary>
    [Browsable(false)]
    public IHTMLElement HTMLElement
    {
      get
      {
        this.EnsureAttached();
        return this.controlElement;
      }
    }

    /// <summary>
    /// Returns the underlying HTML element for the control using the IHTMLElement2 interface
    /// </summary>
    internal IHTMLElement2 HTMLElement2 => this.HTMLElement as IHTMLElement2;

    /// <summary>
    /// Returns the ContextMenu position from the last context menu event. This is
    /// an absolute position within the document.
    /// </summary>
    internal Point ContextMenuPosition => this.contextMenuPosition;

    /// <summary>
    /// Retrieves the parent HTMLDocument object for the control.
    /// </summary>
    /// <returns></returns>
    public HTMLDocument GetHTMLDocument() => this.parentForm.GetSafeBaseDocument();

    /// <summary>Returns the parent Form control.</summary>
    /// <returns></returns>
    [Browsable(false)]
    public Form Form => this.parentForm;

    /// <summary>Moves the control to a new location.</summary>
    /// <param name="location"></param>
    public virtual void MoveTo(Location location)
    {
      Point availableLocation = this.getNearestAvailableLocation(this.getMovementOrigin(location), location);
      this.Position = new Point(Math.Max(0, availableLocation.X), Math.Max(0, availableLocation.Y));
    }

    /// <summary>Adds the control to the current control selection.</summary>
    public virtual void Select() => this.Form.SelectedControls.Add(this);

    /// <summary>
    /// Brings the control to the front of the ZIndex order so it is on top of other controls
    /// in the same container.
    /// </summary>
    public virtual void BringToFront()
    {
      int val1 = 0;
      foreach (Control control in this.GetContainer().Controls)
      {
        if (!control.Equals((object) this))
        {
          string zindex = control.ZIndex;
          if (zindex != "")
            val1 = Math.Max(val1, int.Parse(zindex));
        }
      }
      this.ZIndex = (val1 + 1).ToString();
    }

    /// <summary>
    /// Sends the control to the back of the ZIndex order so it is behind other controls
    /// in the same container.
    /// </summary>
    public virtual void SendToBack()
    {
      int val1 = 0;
      foreach (Control control in this.GetContainer().Controls)
      {
        if (!control.Equals((object) this))
        {
          string zindex = control.ZIndex;
          if (zindex != "")
            val1 = Math.Min(val1, int.Parse(zindex));
        }
      }
      this.ZIndex = (val1 - 1).ToString();
    }

    /// <summary>
    /// Refreshes the properties of the control to ensure they are up-to-date. This method
    /// is intended for use within the Encompass application only.
    /// </summary>
    public virtual void RefreshProperties()
    {
    }

    /// <summary>
    /// Overrides the default notion of equivalence for two controls. The comparison is based
    /// on the controls' IDs.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      Control objA = obj as Control;
      return !object.Equals((object) objA, (object) null) && objA.ControlID == this.ControlID;
    }

    /// <summary>
    /// Overrides the default hash code implementation for the current control.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => this.ControlID.GetHashCode();

    /// <summary>Provides a string representation for the control.</summary>
    /// <returns>Returns the <see cref="P:EllieMae.Encompass.Forms.Control.ControlID" /> of the control.</returns>
    public override string ToString() => this.ControlID;

    /// <summary>
    /// Compares the control to another control for sorting purposes.
    /// </summary>
    /// <param name="o">The Control to which to compare.</param>
    /// <remarks>This method produces a sort based on Control ID.</remarks>
    public int CompareTo(object o) => string.Compare(this.ToString(), o.ToString(), true);

    /// <summary>
    /// Attaches the control to an HTML element. This method is invoked when an element is
    /// rendered to the document or any time the element has become invalid.
    /// </summary>
    /// <param name="form"></param>
    /// <param name="controlElement"></param>
    internal virtual void AttachToElement(Form form, IHTMLElement controlElement)
    {
      this.parentForm = form;
      this.controlElement = controlElement;
      this.controlId = Control.GetControlIDForElement(controlElement);
      if ((this.controlId ?? "") == "")
      {
        this.controlId = form.NewControlID(this.GetType().Name);
        Control.SetControlIDForElement(controlElement, this.controlId);
      }
      Control.SetControlTypeForElement(controlElement, this.GetType());
      this.Form.ControlCache.Add(this);
    }

    /// <summary>
    /// Notifies the control that the form is going into edit mode
    /// </summary>
    internal virtual void OnStartEditing()
    {
    }

    /// <summary>Notifies the control that form is exiting edit mode</summary>
    internal virtual void OnStopEditing()
    {
    }

    /// <summary>
    /// Ensures that the current form is in edit mode, or throws an exception
    /// </summary>
    internal void EnsureEditing()
    {
      if (this.Form != null && !this.Form.EditingEnabled)
        throw new InvalidOperationException("The specified operation is only valid when the form is being edited");
    }

    /// <summary>Generates the HTML for a new control.</summary>
    /// <returns></returns>
    internal virtual string RenderHTML() => throw new NotSupportedException();

    /// <summary>Gets an attribute from the element</summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal string GetAttribute(string name)
    {
      return string.Concat(this.HTMLElement.getAttribute(name, 1));
    }

    /// <summary>Saves an attribute onto the underlying HTML element</summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    internal void SetAttribute(string name, string value)
    {
      this.HTMLElement.setAttribute(name, (object) value);
      this.NotifyPropertyChange();
    }

    /// <summary>
    /// Returns the base set of attributes for a new control. This gets called during the
    /// override RenderHTML() call.
    /// </summary>
    /// <returns></returns>
    internal string GetBaseAttributes()
    {
      return " id=\"" + this.ControlID + "\" controlType=\"" + this.GetType().Name + "\" style=\"position: absolute;\" ";
    }

    /// <summary>
    /// Returns the base set of attributes and sets the contentEditable flag for a control.
    /// </summary>
    /// <param name="contentEditable"></param>
    /// <returns></returns>
    internal string GetBaseAttributes(bool contentEditable)
    {
      return this.GetBaseAttributes() + "contentEditable=\"" + (contentEditable ? "" : "false") + "\"";
    }

    /// <summary>Raise the Change event in the parent Form object.</summary>
    internal void NotifyPropertyChange()
    {
      if (!this.Form.EditingEnabled)
        return;
      this.Form.OnPropertyChange(new FormEventArgs(this));
    }

    /// <summary>
    /// Ensures that the object is properly attached to its underlying HTML element.
    /// </summary>
    internal void EnsureAttached()
    {
      if (this.controlElement == null)
        throw new InvalidOperationException("Control has not been inserted into a region");
      if (!this.ReattachRequired())
        return;
      this.ReattachToElement();
    }

    /// <summary>
    /// Checks is the current control needs to be reattached to its base element
    /// </summary>
    /// <returns></returns>
    internal virtual bool ReattachRequired()
    {
      return Control.IsDetached(this.controlElement) || this.controlElement.document != this.Form.GetHTMLDocument();
    }

    /// <summary>Reattaches the object to its underlying HTML element.</summary>
    internal virtual void ReattachToElement()
    {
      this.AttachToElement(this.Form, Control.FindChildByControlID(this.parentForm.HTMLElement, this.controlId, this.controlElement.tagName) ?? throw new InvalidOperationException("Object has been deleted or no longer exists"));
    }

    /// <summary>
    /// Performs all of the final pre-render activities before the control is displayed.
    /// When this method is called, you can assume the entire control tree has already been
    /// established. This method will be invoked starting at the lowest-level controls
    /// and working its way up the control tree.
    /// </summary>
    internal virtual void PrepareForDisplay()
    {
      using (PerformanceMeter.Current.BeginOperation("Control.PrepareForDisplay"))
        this.RefreshProperties();
    }

    /// <summary>
    /// Internal methed used to adjust the size of a control when it's size has certain contraints.
    /// </summary>
    internal virtual Size AdjustSize(Size size) => size;

    /// <summary>
    /// Invoked when the control's container changes, allowing it to update is visible state
    /// </summary>
    internal virtual void OnContainerChanged()
    {
    }

    /// <summary>Invoked when the control ID is changed on the object.</summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    internal virtual void ChangeControlID(string oldValue, string newValue)
    {
      this.Form.ControlCache.Remove(this.controlId);
      Control.SetControlIDForElement(this.controlElement, newValue);
      this.AttachToElement(this.Form, this.controlElement);
    }

    /// <summary>
    /// Deletes the control's event from the form's Event collection
    /// </summary>
    private void deleteEvents()
    {
      if (!(this is ISupportsEvents supportsEvents))
        return;
      foreach (string supportedEvent in supportsEvents.SupportedEvents)
        this.Form.ControlEvents.Remove(this.ControlID, supportedEvent);
    }

    /// <summary>
    /// Returns the origin for movement commands on the current control.
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private Point getMovementOrigin(Location location)
    {
      ContainerControl container = this.GetContainer();
      Size size1 = container == null ? this.Size : container.ClientSize;
      switch (location)
      {
        case Location.TopLeft:
          return new Point(0, 0);
        case Location.Top:
          return new Point(this.Position.X, 0);
        case Location.TopRight:
          return new Point(size1.Width - this.Size.Width, 0);
        case Location.Left:
          return new Point(0, this.Position.Y);
        case Location.Right:
          return new Point(size1.Width - this.Size.Width, this.Position.Y);
        case Location.BottomLeft:
          return new Point(0, size1.Height - this.Size.Height);
        case Location.Bottom:
          return new Point(this.Position.X, size1.Height - this.Size.Height);
        case Location.BottomRight:
          int width1 = size1.Width;
          Size size2 = this.Size;
          int width2 = size2.Width;
          int x = width1 - width2;
          int height1 = size1.Height;
          size2 = this.Size;
          int height2 = size2.Height;
          int y = height1 - height2;
          return new Point(x, y);
        default:
          return new Point(0, 0);
      }
    }

    /// <summary>
    /// Finds the nearest point at which the current control could be positioned
    /// so as not to overlap with an other controls in the same container.
    /// </summary>
    /// <param name="currentPosition"></param>
    /// <param name="location"></param>
    /// <returns></returns>
    private Point getNearestAvailableLocation(Point currentPosition, Location location)
    {
      Rectangle a1 = new Rectangle(currentPosition, this.Size);
      Rectangle a2 = Rectangle.Empty;
      foreach (Control control in this.GetContainer().Controls)
      {
        if (!control.Equals((object) this))
        {
          Rectangle b = new Rectangle(control.Position, control.Size);
          if (Rectangle.Intersect(a1, b) != Rectangle.Empty)
            a2 = Rectangle.Union(a2, b);
        }
      }
      if (a2 == Rectangle.Empty)
        return a1.Location;
      Point point1 = Point.Empty;
      Point point2 = Point.Empty;
      if (location == Location.TopLeft || location == Location.Top || location == Location.TopRight)
        point1 = new Point(a1.Left, a2.Bottom + 1);
      else if (location == Location.BottomLeft || location == Location.Bottom || location == Location.BottomRight)
        point1 = new Point(a1.Left, a2.Top - 1);
      if (location == Location.TopLeft || location == Location.Left || location == Location.BottomLeft)
        point2 = new Point(a2.Right + 1, a1.Top);
      else if (location == Location.TopRight || location == Location.Right || location == Location.BottomRight)
        point2 = new Point(a1.Left, a2.Top - 1);
      if (point1 != Point.Empty)
        point1 = this.getNearestAvailableLocation(point1, location);
      if (point2 != Point.Empty)
        point2 = this.getNearestAvailableLocation(point2, location);
      return point1 == Point.Empty || !(point2 == Point.Empty) && Control.norm(point2) < Control.norm(point1) ? point2 : point1;
    }

    /// <summary>Computes the 2-norm of a point</summary>
    /// <param name="p"></param>
    /// <returns></returns>
    private static double norm(Point p) => Math.Sqrt((double) (p.X * p.X + p.Y * p.Y));

    /// <summary>
    /// Returns the absolute postion of an element on the form.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    internal static Point GetAbsoluteElementPosition(IHTMLElement element)
    {
      Point point = new Point(0, 0);
      if (element.offsetParent != null && element.offsetParent != element)
        point = Control.GetAbsoluteElementPosition(element.offsetParent);
      return new Point(point.X + element.offsetLeft, point.Y + element.offsetTop);
    }

    /// <summary>Returns the control type for an HTML element.</summary>
    /// <param name="controlElement"></param>
    /// <returns></returns>
    internal static System.Type GetControlTypeForElement(IHTMLElement controlElement)
    {
      try
      {
        string key = string.Concat(controlElement.getAttribute("controlType", 1));
        if (key == "")
          return (System.Type) null;
        System.Type type = (System.Type) Control.controlTypeCache[(object) key];
        if (type == (System.Type) null)
        {
          type = System.Type.GetType("EllieMae.Encompass.Forms." + key);
          Control.controlTypeCache.Add((object) key, (object) type);
        }
        return type;
      }
      catch
      {
        return (System.Type) null;
      }
    }

    /// <summary>Sets the control type on an HTML element.</summary>
    /// <param name="controlElement"></param>
    /// <param name="t"></param>
    internal static void SetControlTypeForElement(IHTMLElement controlElement, System.Type t)
    {
      if (t == (System.Type) null)
        controlElement.setAttribute("controlType", (object) "");
      else
        controlElement.setAttribute("controlType", (object) t.Name);
    }

    /// <summary>Gets the Control ID for an HTML element.</summary>
    /// <param name="controlElement"></param>
    /// <returns></returns>
    internal static string GetControlIDForElement(IHTMLElement controlElement)
    {
      try
      {
        return controlElement.id;
      }
      catch
      {
        return (string) null;
      }
    }

    /// <summary>Sets the Control ID on an HTML element.</summary>
    /// <param name="controlElement"></param>
    /// <param name="controlId"></param>
    internal static void SetControlIDForElement(IHTMLElement controlElement, string controlId)
    {
      controlElement.id = controlId;
    }

    /// <summary>
    /// Verifies that a string is a valid control ID. Control IDs can contain only
    /// letters, numbers or the underscore character and must start with a letter.
    /// </summary>
    /// <param name="controlId"></param>
    /// <returns></returns>
    internal static bool IsValidControlID(string controlId)
    {
      return new Regex("^[A-Z][A-Z0-9_]*$", RegexOptions.IgnoreCase).IsMatch(controlId);
    }

    /// <summary>
    /// Returns a flag indicating if an HTML element represents a control.
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    internal static bool IsControl(IHTMLElement element)
    {
      return (Control.GetControlIDForElement(element) ?? "") != "" && Control.GetControlTypeForElement(element) != (System.Type) null;
    }

    /// <summary>Finds a child HTML element based on the control ID.</summary>
    /// <param name="parentElement"></param>
    /// <param name="controlId"></param>
    /// <param name="recurse"></param>
    /// <returns></returns>
    internal static IHTMLElement FindChildByControlID(
      IHTMLElement parentElement,
      string controlId,
      bool recurse)
    {
      IHTMLElementCollection elementCollection = recurse ? (IHTMLElementCollection) parentElement.all : (IHTMLElementCollection) parentElement.children;
      for (int name = 0; name < elementCollection.length; ++name)
      {
        IHTMLElement controlElement = (IHTMLElement) elementCollection.item((object) name);
        if (Control.GetControlIDForElement(controlElement) == controlId)
          return controlElement;
      }
      return (IHTMLElement) null;
    }

    /// <summary>
    /// Locates a child control using the underlying element's tag name as a hint.
    /// </summary>
    /// <param name="parentElement"></param>
    /// <param name="controlId"></param>
    /// <param name="tagName"></param>
    /// <returns></returns>
    internal static IHTMLElement FindChildByControlID(
      IHTMLElement parentElement,
      string controlId,
      string tagName)
    {
      IHTMLElementCollection elementsByTagName = ((IHTMLElement2) parentElement).getElementsByTagName(tagName);
      for (int name = 0; name < elementsByTagName.length; ++name)
      {
        IHTMLElement controlElement = (IHTMLElement) elementsByTagName.item((object) name);
        if (Control.GetControlIDForElement(controlElement) == controlId)
          return controlElement;
      }
      return (IHTMLElement) null;
    }

    /// <summary>
    /// Determines if an element is "Attached", i.e. is attached to an underlying document
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    internal static bool IsDetached(IHTMLElement e)
    {
      return e != null && ((IHTMLElement2) e).currentStyle == null;
    }

    /// <summary>Merges two list of event names into a single list.</summary>
    /// <param name="listA"></param>
    /// <param name="listB"></param>
    /// <returns></returns>
    internal static string[] MergeEvents(string[] listA, string[] listB)
    {
      ArrayList arrayList = new ArrayList((ICollection) listA);
      arrayList.AddRange((ICollection) listB);
      return (string[]) arrayList.ToArray(typeof (string));
    }

    /// <summary>
    /// Produces a mangled version of a control ID which should be unique
    /// </summary>
    /// <param name="controlId"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    internal static string MangleControlID(string controlId, string key)
    {
      return "__cid_" + controlId + "_" + key;
    }

    internal static string ResolveInternalImagePath(string imageName)
    {
      return (imageName ?? "") == "" ? "" : AssemblyResolver.GetResourceFileFullPath(Path.Combine(SystemSettings.FormRelDir, "images\\" + imageName), SystemSettings.LocalAppDir);
    }

    internal static string SpanContentEditable()
    {
      string appSetting = ConfigurationManager.AppSettings[nameof (SpanContentEditable)];
      if (!string.IsNullOrWhiteSpace(appSetting))
      {
        string lower = appSetting.ToLower();
        if (string.Compare(lower, "true", true) == 0 || string.Compare(lower, "false", true) == 0)
          return lower;
      }
      return (string) null;
    }

    /// <summary>
    /// Dispose of this control and all its underlying resources.
    /// </summary>
    public new void Dispose()
    {
      if (this.controlElement != null && this.forceComRelease)
      {
        Marshal.FinalReleaseComObject((object) this.controlElement);
        this.controlElement = (IHTMLElement) null;
      }
      base.Dispose();
    }
  }
}
