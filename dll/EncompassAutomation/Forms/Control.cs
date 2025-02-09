// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.Control
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

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
  public abstract class Control : Component, IComparable
  {
    internal static readonly Control Empty = (Control) new EmptyControl();
    private static readonly Hashtable controlTypeCache = new Hashtable();
    private bool forceComRelease;
    private IHTMLElement controlElement;
    private string controlId;
    private Point contextMenuPosition = Point.Empty;
    private Form parentForm;

    public Control()
    {
    }

    internal Control(Form form, IHTMLElement controlElement)
    {
      this.AttachToElement(form, controlElement);
    }

    internal Control(string controlId) => this.controlId = controlId;

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

    [Browsable(false)]
    public int Left
    {
      get => this.Position.X;
      set => this.Position = new Point(value, this.Position.Y);
    }

    [Browsable(false)]
    public int Right
    {
      get => this.Position.X + this.Size.Width - 1;
      set => this.Position = new Point(value - this.Size.Width + 1, this.Position.Y);
    }

    [Browsable(false)]
    public int Top
    {
      get => this.Position.Y;
      set => this.Position = new Point(this.Position.X, value);
    }

    [Browsable(false)]
    public int Bottom
    {
      get => this.Position.Y + this.Size.Height - 1;
      set => this.Position = new Point(this.Position.X, value - this.Size.Height + 1);
    }

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

    [Browsable(false)]
    public Rectangle Bounds => new Rectangle(this.AbsolutePosition, this.Size);

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

    public virtual string GetCustomAttribute(string name) => this.GetAttribute(name);

    public bool HitTest(Point p) => this.Bounds.Contains(p);

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

    public virtual void Refresh()
    {
      if (this.Form.FormScreen == null)
        return;
      this.Form.FormScreen.RefreshControl(this.ControlID);
    }

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

    [Browsable(false)]
    public virtual bool AllowCutCopyDelete => true;

    [Browsable(false)]
    public virtual bool AllowPositioning => true;

    [Browsable(false)]
    public virtual bool AllowDesign => true;

    [Browsable(false)]
    public IHTMLElement HTMLElement
    {
      get
      {
        this.EnsureAttached();
        return this.controlElement;
      }
    }

    internal IHTMLElement2 HTMLElement2 => this.HTMLElement as IHTMLElement2;

    internal Point ContextMenuPosition => this.contextMenuPosition;

    public HTMLDocument GetHTMLDocument() => this.parentForm.GetSafeBaseDocument();

    [Browsable(false)]
    public Form Form => this.parentForm;

    public virtual void MoveTo(Location location)
    {
      Point availableLocation = this.getNearestAvailableLocation(this.getMovementOrigin(location), location);
      this.Position = new Point(Math.Max(0, availableLocation.X), Math.Max(0, availableLocation.Y));
    }

    public virtual void Select() => this.Form.SelectedControls.Add(this);

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

    public virtual void RefreshProperties()
    {
    }

    public override bool Equals(object obj)
    {
      Control objA = obj as Control;
      return !object.Equals((object) objA, (object) null) && objA.ControlID == this.ControlID;
    }

    public override int GetHashCode() => this.ControlID.GetHashCode();

    public override string ToString() => this.ControlID;

    public int CompareTo(object o) => string.Compare(this.ToString(), o.ToString(), true);

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

    internal virtual void OnStartEditing()
    {
    }

    internal virtual void OnStopEditing()
    {
    }

    internal void EnsureEditing()
    {
      if (this.Form != null && !this.Form.EditingEnabled)
        throw new InvalidOperationException("The specified operation is only valid when the form is being edited");
    }

    internal virtual string RenderHTML() => throw new NotSupportedException();

    internal string GetAttribute(string name)
    {
      return string.Concat(this.HTMLElement.getAttribute(name, 1));
    }

    internal void SetAttribute(string name, string value)
    {
      this.HTMLElement.setAttribute(name, (object) value, 1);
      this.NotifyPropertyChange();
    }

    internal string GetBaseAttributes()
    {
      return " id=\"" + this.ControlID + "\" controlType=\"" + this.GetType().Name + "\" style=\"position: absolute;\" ";
    }

    internal string GetBaseAttributes(bool contentEditable)
    {
      return this.GetBaseAttributes() + "contentEditable=\"" + (contentEditable ? "" : "false") + "\"";
    }

    internal void NotifyPropertyChange()
    {
      if (!this.Form.EditingEnabled)
        return;
      this.Form.OnPropertyChange(new FormEventArgs(this));
    }

    internal void EnsureAttached()
    {
      if (this.controlElement == null)
        throw new InvalidOperationException("Control has not been inserted into a region");
      if (!this.ReattachRequired())
        return;
      this.ReattachToElement();
    }

    internal virtual bool ReattachRequired()
    {
      return Control.IsDetached(this.controlElement) || this.controlElement.document != this.Form.GetHTMLDocument();
    }

    internal virtual void ReattachToElement()
    {
      this.AttachToElement(this.Form, Control.FindChildByControlID(this.parentForm.HTMLElement, this.controlId, this.controlElement.tagName) ?? throw new InvalidOperationException("Object has been deleted or no longer exists"));
    }

    internal virtual void PrepareForDisplay()
    {
      using (PerformanceMeter.Current.BeginOperation("Control.PrepareForDisplay"))
        this.RefreshProperties();
    }

    internal virtual Size AdjustSize(Size size) => size;

    internal virtual void OnContainerChanged()
    {
    }

    internal virtual void ChangeControlID(string oldValue, string newValue)
    {
      this.Form.ControlCache.Remove(this.controlId);
      Control.SetControlIDForElement(this.controlElement, newValue);
      this.AttachToElement(this.Form, this.controlElement);
    }

    private void deleteEvents()
    {
      if (!(this is ISupportsEvents supportsEvents))
        return;
      foreach (string supportedEvent in supportsEvents.SupportedEvents)
        this.Form.ControlEvents.Remove(this.ControlID, supportedEvent);
    }

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

    private static double norm(Point p) => Math.Sqrt((double) (p.X * p.X + p.Y * p.Y));

    internal static Point GetAbsoluteElementPosition(IHTMLElement element)
    {
      Point point = new Point(0, 0);
      if (element.offsetParent != null && element.offsetParent != element)
        point = Control.GetAbsoluteElementPosition(element.offsetParent);
      return new Point(point.X + element.offsetLeft, point.Y + element.offsetTop);
    }

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

    internal static void SetControlTypeForElement(IHTMLElement controlElement, System.Type t)
    {
      if (t == (System.Type) null)
        controlElement.setAttribute("controlType", (object) "", 1);
      else
        controlElement.setAttribute("controlType", (object) t.Name, 1);
    }

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

    internal static void SetControlIDForElement(IHTMLElement controlElement, string controlId)
    {
      controlElement.id = controlId;
    }

    internal static bool IsValidControlID(string controlId)
    {
      return new Regex("^[A-Z][A-Z0-9_]*$", RegexOptions.IgnoreCase).IsMatch(controlId);
    }

    internal static bool IsControl(IHTMLElement element)
    {
      return (Control.GetControlIDForElement(element) ?? "") != "" && Control.GetControlTypeForElement(element) != (System.Type) null;
    }

    internal static IHTMLElement FindChildByControlID(
      IHTMLElement parentElement,
      string controlId,
      bool recurse)
    {
      IHTMLElementCollection elementCollection = recurse ? (IHTMLElementCollection) parentElement.all : (IHTMLElementCollection) parentElement.children;
      for (int index = 0; index < elementCollection.length; ++index)
      {
        IHTMLElement controlElement = (IHTMLElement) elementCollection.item((object) index, (object) null);
        if (Control.GetControlIDForElement(controlElement) == controlId)
          return controlElement;
      }
      return (IHTMLElement) null;
    }

    internal static IHTMLElement FindChildByControlID(
      IHTMLElement parentElement,
      string controlId,
      string tagName)
    {
      IHTMLElementCollection elementsByTagName = ((IHTMLElement2) parentElement).getElementsByTagName(tagName);
      for (int index = 0; index < elementsByTagName.length; ++index)
      {
        IHTMLElement controlElement = (IHTMLElement) elementsByTagName.item((object) index, (object) null);
        if (Control.GetControlIDForElement(controlElement) == controlId)
          return controlElement;
      }
      return (IHTMLElement) null;
    }

    internal static bool IsDetached(IHTMLElement e)
    {
      return e != null && ((IHTMLElement2) e).currentStyle == null;
    }

    internal static string[] MergeEvents(string[] listA, string[] listB)
    {
      ArrayList arrayList = new ArrayList((ICollection) listA);
      arrayList.AddRange((ICollection) listB);
      return (string[]) arrayList.ToArray(typeof (string));
    }

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
