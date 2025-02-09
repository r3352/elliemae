// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TrusteeDatabaseSettingForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.InputEngine;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TrusteeDatabaseSettingForm : UserControl
  {
    private TrusteeDatabaseControl trusteeControl;
    private IContainer components;

    public TrusteeDatabaseSettingForm(SetUpContainer setupContainer)
    {
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.trusteeControl = new TrusteeDatabaseControl((IWin32Window) setupContainer, (TrusteeRecord) null, true);
      this.Controls.Add((Control) this.trusteeControl);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.SuspendLayout();
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Name = nameof (TrusteeDatabaseSettingForm);
      this.Size = new Size(707, 485);
      this.ResumeLayout(false);
    }
  }
}
