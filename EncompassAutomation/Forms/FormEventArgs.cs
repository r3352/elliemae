// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.FormEventArgs
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using mshtml;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>Event arguments for the Form's events.</summary>
  /// <exclude />
  public class FormEventArgs : EventArgs
  {
    /// <summary>Represents an empty set of event arguments.</summary>
    public static readonly FormEventArgs Empty = new FormEventArgs();
    private Form form;
    private IHTMLEventObj eventObj;
    private Control sourceControl;
    private bool cancel;
    private Point scrollOffset = Point.Empty;

    internal FormEventArgs(Form form, IHTMLEventObj eventObj)
    {
      this.form = form;
      this.eventObj = eventObj;
      this.scrollOffset = form.ScrollOffset;
    }

    internal FormEventArgs(Control control)
    {
      this.sourceControl = control;
      this.form = this.sourceControl.Form;
      this.scrollOffset = this.form.ScrollOffset;
    }

    private FormEventArgs()
    {
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Forms.FormEventArgs.Control" /> which is affected by this event.
    /// </summary>
    public Control Control
    {
      get
      {
        if (this.sourceControl == null && this.eventObj.srcElement != null)
          this.sourceControl = this.form.FindControlForElement(this.eventObj.srcElement);
        return this.sourceControl;
      }
    }

    /// <summary>
    /// Gets or sets a flag that allows the event to be cancelled.
    /// </summary>
    public bool Cancel
    {
      get => this.cancel;
      set => this.cancel = value;
    }

    /// <summary>
    /// Returns information on the mouse button clicked by the user.
    /// </summary>
    public int Button => this.eventObj.button;

    /// <summary>
    /// Gets the position of the mouse within the visible region.
    /// </summary>
    public Point MousePositionInVisibleRegion
    {
      get => this.eventObj == null ? Point.Empty : new Point(this.eventObj.x, this.eventObj.y);
    }

    /// <summary>Gets the absolute position of the mouse on the Form.</summary>
    public Point MousePosition
    {
      get
      {
        return this.eventObj == null ? Point.Empty : new Point(this.eventObj.clientX + this.scrollOffset.X, this.eventObj.clientY + this.scrollOffset.Y);
      }
    }
  }
}
