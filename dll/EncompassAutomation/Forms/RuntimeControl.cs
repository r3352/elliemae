// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RuntimeControl
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll

using EllieMae.EMLite.Common;
using mshtml;
using System;
using System.ComponentModel;
using System.Drawing;

#nullable disable
namespace EllieMae.Encompass.Forms
{
  public abstract class RuntimeControl : Control
  {
    private bool isContainerVisible = true;

    protected RuntimeControl()
    {
    }

    internal RuntimeControl(Form parentForm, IHTMLElement controlElement)
      : base(parentForm, controlElement)
    {
    }

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

    public override void MoveTo(Location location)
    {
      if (!this.AllowPositioning)
        throw new InvalidOperationException("Operation is only valid for Positioned controls");
      base.MoveTo(location);
    }

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

    [Category("Behavior")]
    [Description("Sets the initial state of the field as enabled or disabled.")]
    public virtual bool Enabled
    {
      get => string.Concat(this.GetEnablingTargetElement().getAttribute("emdisabled", 1)) != "1";
      set
      {
        if (value == this.Enabled)
          return;
        this.GetEnablingTargetElement().setAttribute("emdisabled", value ? (object) "0" : (object) "1", 1);
        this.ApplyInteractiveState();
        this.NotifyPropertyChange();
      }
    }

    public void EnsureVisible() => this.HTMLElement.scrollIntoView((object) false);

    [Browsable(false)]
    public virtual bool Interactive
    {
      get
      {
        return !((IHTMLElement3) this.GetEnablingTargetElement()).disabled && this.Visible && this.isContainerVisible;
      }
    }

    internal override void OnStartEditing() => this.ChangeControlVisibilityState(true);

    internal virtual bool AllowInteractivity => this.Enabled;

    internal bool IsContainerVisible => this.isContainerVisible;

    internal virtual void OnContainerInteractiveStateChanged(bool interactive)
    {
      this.ChangeControlInteractiveState(interactive && this.AllowInteractivity);
    }

    internal virtual void OnContainerVisibilityStateChanged(bool visible)
    {
      this.isContainerVisible = visible;
    }

    internal virtual void ChangeControlVisibilityState(bool visible)
    {
      using (PerformanceMeter.Current.BeginOperation("RuntimeControl.ChangeControlVisibilityState"))
        this.HTMLElement.style.visibility = visible ? "inherit" : "hidden";
    }

    internal virtual void ChangeControlInteractiveState(bool interactive)
    {
      using (PerformanceMeter.Current.BeginOperation("RuntimeControl.ChangeControlInteractiveState"))
        ((IHTMLElement3) this.GetEnablingTargetElement()).disabled = !interactive;
    }

    internal virtual IHTMLElement GetEnablingTargetElement() => this.HTMLElement;

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

    internal override void OnContainerChanged()
    {
      base.OnContainerChanged();
      this.ApplyInteractiveState();
    }

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
