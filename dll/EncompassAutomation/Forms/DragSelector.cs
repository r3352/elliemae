// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.DragSelector
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using mshtml;
using System;
using System.Collections;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
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

    public bool Active => this.dragContainer != null;

    public override bool AllowDesign => false;

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

    public void EndDrag()
    {
      this.dragContainer = (ContainerControl) null;
      this.HTMLElement.style.display = "none";
    }

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
