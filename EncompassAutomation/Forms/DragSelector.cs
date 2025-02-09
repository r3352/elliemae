// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DragSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Represents the drag selection box used when the user shift-clicks in the Form Builder.
  /// </summary>
  /// <remarks>This class is intended for use within the Encompass Form Builder only.</remarks>
  public class DragSelector : DesignerControl
  {
    private Point dragOrigin = Point.Empty;
    private ContainerControl dragContainer;
    private Rectangle currentBounds = Rectangle.Empty;

    internal DragSelector()
    {
    }

    internal DragSelector(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    /// <summary>
    /// Indicates if the control is curently active and being dragged.
    /// </summary>
    public bool Active => this.dragContainer != null;

    /// <summary>
    /// Prevents the control from being manipulated in the Form Builder.
    /// </summary>
    public override bool AllowDesign => false;

    /// <summary>Starts the drag operation on the control.</summary>
    /// <param name="origin">The origin of the drag operation.</param>
    public void BeginDrag(Point origin)
    {
      if (!this.Form.EditingEnabled)
        throw new InvalidOperationException("This control can only be activated in design mode");
      this.Form.SelectedControls.Clear();
      this.dragContainer = this.Form.GetContainerAtPoint(origin);
      this.dragOrigin = origin;
      this.HTMLElement.style.display = "block";
      this.BringToFront();
    }

    /// <summary>Stops the drag operation of the control.</summary>
    public void EndDrag()
    {
      this.dragContainer = (ContainerControl) null;
      this.HTMLElement.style.display = "none";
    }

    /// <summary>
    /// Drags the control to the specified absolute position on the Form.
    /// </summary>
    /// <param name="dragPoint">The position to expand the drag region to.</param>
    public void DragTo(Point dragPoint)
    {
      if (this.dragContainer == null)
        throw new InvalidOperationException("A drag operation is not current in progress");
      if (!this.Form.EditingEnabled)
        throw new InvalidOperationException("This control can only be activated in design mode");
      Point location = new Point(Math.Min(dragPoint.X, this.dragOrigin.X), Math.Min(dragPoint.Y, this.dragOrigin.Y));
      Point point = new Point(Math.Max(dragPoint.X, this.dragOrigin.X), Math.Max(dragPoint.Y, this.dragOrigin.Y));
      Size size = new Size(point.X - location.X, point.Y - location.Y);
      if (this.currentBounds.Location != location)
        this.AbsolutePosition = location;
      if (this.currentBounds.Size != size)
        this.Size = size;
      this.currentBounds = new Rectangle(location, size);
      Rectangle rect = new Rectangle(location, size);
      ArrayList controls = new ArrayList();
      foreach (Control control in this.dragContainer.Controls)
      {
        if (control != this && control.Bounds.IntersectsWith(rect))
          controls.Add((object) control);
      }
      this.Form.SelectedControls.Clear();
      this.Form.SelectedControls.AddRange((IEnumerable) controls);
    }

    internal override void OnStartEditing()
    {
    }

    internal override string RenderHTML()
    {
      return "<div " + this.GetBaseAttributes(false) + " style=\"border: dashed black 1px; display: none;\" ></div>";
    }
  }
}
