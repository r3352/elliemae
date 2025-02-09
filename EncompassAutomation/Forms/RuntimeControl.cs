// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RuntimeControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

using EllieMae.EMLite.Common;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Provides the base class for all controls which are used to affect the runtime behavior or
  /// appearance of the form.
  /// </summary>
  /// <remarks>Runtime controls are distinguished from Designer controls in that they are visible
  /// to the ens user at runtime. Designer controls are used to help the form designer build the form
  /// or add custom behaviors but have no physical appearance when the form is displayed at runetime.
  /// </remarks>
  public abstract class RuntimeControl : Control
  {
    private bool isContainerVisible = true;

    /// <summary>Default constructor for the RuntimeControl.</summary>
    protected RuntimeControl()
    {
    }

    internal RuntimeControl(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

    /// <summary>
    /// Override of the base Control class Position property to ensure that the
    /// control doesn't have Flow layout.
    /// </summary>
    public override Point Position
    {
      get => base.Position;
      set
      {
        if (!this.AllowPositioning)
          throw new InvalidOperationException("Position cannot be changed for elements with Flow layout.");
        base.Position = value;
      }
    }

    /// <summary>Moves the control to a new location on the form.</summary>
    /// <param name="location"></param>
    public override void MoveTo(Location location)
    {
      if (!this.AllowPositioning)
        throw new InvalidOperationException("Operation is only valid for Positioned controls");
      base.MoveTo(location);
    }

    /// <summary>Controls the visibility of the control</summary>
    [Category("Behavior")]
    public virtual bool Visible
    {
      get => this.GetAttribute("emvisible") != "0";
      set
      {
        this.SetAttribute("emvisible", value ? "1" : "0");
        if (this.Form.EditingEnabled)
          return;
        this.ChangeControlVisibilityState(value);
      }
    }

    /// <summary>
    /// Gets or sets whether the control will respond to user input.
    /// </summary>
    [Category("Behavior")]
    [Description("Sets the initial state of the field as enabled or disabled.")]
    public virtual bool Enabled
    {
      get => string.Concat(this.GetEnablingTargetElement().getAttribute("emdisabled", 1)) != "1";
      set
      {
        if (value == this.Enabled)
          return;
        this.GetEnablingTargetElement().setAttribute("emdisabled", value ? (object) "0" : (object) "1");
        this.ApplyInteractiveState();
        this.NotifyPropertyChange();
      }
    }

    /// <summary>Ensures that the control is visible on the screen.</summary>
    public void EnsureVisible() => this.HTMLElement.scrollIntoView((object) false);

    /// <summary>
    /// Gets a value which indicates if the control is in an interactive state.
    /// </summary>
    /// <remarks>A control is interactive if both it and all container
    /// controls in which it resides are enabled.</remarks>
    [Browsable(false)]
    public virtual bool Interactive
    {
      get
      {
        return !((IHTMLElement3) this.GetEnablingTargetElement()).disabled && this.Visible && this.isContainerVisible;
      }
    }

    /// <summary>
    /// Notifies the control that the form is going into edit mode
    /// </summary>
    internal override void OnStartEditing() => this.ChangeControlVisibilityState(true);

    /// <summary>
    /// Determines if the control can be set into an interactive state
    /// </summary>
    internal virtual bool AllowInteractivity => this.Enabled;

    /// <summary>Determines if the control's container is visible</summary>
    internal bool IsContainerVisible => this.isContainerVisible;

    /// <summary>
    /// Invoked when the control's container's enabled state changes. This allows
    /// the control to perform some custom code in place of or in addition to
    /// disabling itself.
    /// </summary>
    internal virtual void OnContainerInteractiveStateChanged(bool interactive)
    {
      this.ChangeControlInteractiveState(interactive && this.AllowInteractivity);
    }

    /// <summary>
    /// Invoked when the control's container's enabled state changes. This allows
    /// the control to perform some custom code in place of or in addition to
    /// disabling itself.
    /// </summary>
    internal virtual void OnContainerVisibilityStateChanged(bool visible)
    {
      this.isContainerVisible = visible;
    }

    /// <summary>Sets the visiblity state of a control.</summary>
    /// <param name="visible"></param>
    internal virtual void ChangeControlVisibilityState(bool visible)
    {
      using (PerformanceMeter.Current.BeginOperation("RuntimeControl.ChangeControlVisibilityState"))
        this.HTMLElement.style.visibility = visible ? "inherit" : "hidden";
    }

    /// <summary>
    /// Override this method to perform whatever control-specific functionality
    /// must be performed when the control is enabled/disabled.
    /// </summary>
    /// <param name="interactive"></param>
    internal virtual void ChangeControlInteractiveState(bool interactive)
    {
      using (PerformanceMeter.Current.BeginOperation("RuntimeControl.ChangeControlInteractiveState"))
        ((IHTMLElement3) this.GetEnablingTargetElement()).disabled = !interactive;
    }

    /// <summary>
    /// Returns the HTML element that should be used to enable/disable the control.
    /// </summary>
    internal virtual IHTMLElement GetEnablingTargetElement() => this.HTMLElement;

    /// <summary>
    /// Performs all of the final pre-render activities before the control is displayed.
    /// When this method is called, you can assume the entire control tree has already been
    /// established. This method will be invoked starting at the lowest-level controls
    /// and working its way up the control tree.
    /// </summary>
    internal override void PrepareForDisplay()
    {
      base.PrepareForDisplay();
      using (PerformanceMeter.Current.BeginOperation("RuntimeControl.PrepareForDisplay"))
      {
        if (!this.Form.EditingEnabled && !this.Visible)
          this.ChangeControlVisibilityState(false);
        if (this.AllowInteractivity)
          return;
        this.ChangeControlInteractiveState(false);
      }
    }

    /// <summary>
    /// When the control's container changes, we need to update the interactivity state of the control
    /// </summary>
    internal override void OnContainerChanged()
    {
      base.OnContainerChanged();
      this.ApplyInteractiveState();
    }

    /// <summary>
    /// Enables or disables the control using the control hierarchy rules
    /// </summary>
    internal void ApplyInteractiveState()
    {
      ContainerControl container = this.GetContainer();
      if (container != null)
        this.OnContainerInteractiveStateChanged(container.Interactive);
      else
        this.OnContainerInteractiveStateChanged(true);
    }
  }
}
