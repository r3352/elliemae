// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanAmountRoundPanel
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanAmountRoundPanel : UserControl
  {
    private IContainer components;
    private Label label1;
    private CheckBox chkBoxNoRounding;

    public LoanAmountRoundPanel()
    {
      this.InitializeComponent();
      object serverSetting = Session.ServerManager.GetServerSetting("Policies.LoanAmountRounding");
      if (serverSetting == null || (EnableDisableSetting) serverSetting != EnableDisableSetting.Enabled)
        return;
      this.chkBoxNoRounding.Checked = true;
    }

    private void chkBoxNoRounding_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkBoxNoRounding.Checked)
        Session.ServerManager.UpdateServerSetting("Policies.LoanAmountRounding", (object) EnableDisableSetting.Enabled);
      else
        Session.ServerManager.UpdateServerSetting("Policies.LoanAmountRounding", (object) EnableDisableSetting.Disabled);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.chkBoxNoRounding = new CheckBox();
      this.SuspendLayout();
      this.label1.Location = new Point(28, 24);
      this.label1.Name = "label1";
      this.label1.Size = new Size(324, 19);
      this.label1.TabIndex = 4;
      this.label1.Text = "Select the check box to enable loan amount rounding.";
      this.chkBoxNoRounding.Location = new Point(31, 46);
      this.chkBoxNoRounding.Name = "chkBoxNoRounding";
      this.chkBoxNoRounding.Size = new Size(324, 24);
      this.chkBoxNoRounding.TabIndex = 3;
      this.chkBoxNoRounding.Text = "Enables Loan Amount Rounding";
      this.chkBoxNoRounding.CheckedChanged += new EventHandler(this.chkBoxNoRounding_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkBoxNoRounding);
      this.Name = nameof (LoanAmountRoundPanel);
      this.Size = new Size(578, 423);
      this.ResumeLayout(false);
    }
  }
}
