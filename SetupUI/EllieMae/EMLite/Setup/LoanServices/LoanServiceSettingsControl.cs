// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanServices.LoanServiceSettingsControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.LoanServices;
using System.ComponentModel;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.LoanServices
{
  public class LoanServiceSettingsControl : SettingsUserControl
  {
    private ISettingsUserControl settingsControl;
    private IContainer components;

    public LoanServiceSettingsControl(SetUpContainer setupContainer, Control settingsControl)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.settingsControl = (ISettingsUserControl) settingsControl;
      this.Controls.Add(settingsControl);
      settingsControl.Dock = DockStyle.Fill;
    }

    public override void Save() => this.settingsControl.Save();

    public override void Reset() => this.settingsControl.Reset();

    public override bool IsDirty => this.settingsControl.IsDirty;

    public override bool IsValid => this.settingsControl.IsValid;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AutoScaleMode = AutoScaleMode.Font;
    }
  }
}
