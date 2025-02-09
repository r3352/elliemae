// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.Wizard.WizardItem
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI.Wizard
{
  public class WizardItem : UserControl
  {
    public static readonly WizardItem Finished = new WizardItem();
    public static readonly WizardItem Cancelled = new WizardItem();
    private WizardItem prevItem;
    private bool scaled;
    private System.ComponentModel.Container components;

    public event EventHandler ControlsChange;

    public WizardItem()
      : this((WizardItem) null)
    {
    }

    public WizardItem(WizardItem previousItem)
    {
      this.InitializeComponent();
      this.prevItem = previousItem;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.Name = nameof (WizardItem);
      this.Size = new Size(496, 314);
    }

    protected void OnControlsChange()
    {
      if (this.ControlsChange == null)
        return;
      this.ControlsChange((object) this, new EventArgs());
    }

    public virtual bool NextVisible => true;

    public virtual bool BackVisible => true;

    public virtual bool CancelVisible => true;

    public virtual bool NextEnabled => true;

    public virtual bool BackEnabled => this.prevItem != null;

    public virtual bool CancelEnabled => true;

    public virtual string NextLabel => "&Next >";

    public virtual string BackLabel => "< &Back";

    public virtual string CancelLabel => "&Cancel";

    public virtual WizardItem Next() => (WizardItem) null;

    public virtual WizardItem Back() => this.prevItem;

    public virtual WizardItem Cancel() => WizardItem.Cancelled;

    protected override void ScaleCore(float dx, float dy)
    {
      if (this.scaled)
        return;
      base.ScaleCore(dx, dy);
      this.scaled = true;
    }
  }
}
