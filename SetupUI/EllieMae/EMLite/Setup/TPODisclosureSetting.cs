// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TPODisclosureSetting
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TPODisclosureSetting : SettingsUserControl, IOnlineHelpTarget
  {
    private bool access;
    private IContainer components;
    private CheckBox chkCreditOverride;
    private Label label1;
    private GroupContainer gcStatementOfDenial;

    public TPODisclosureSetting(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      Session.ServerManager.GetServerSetting("Policies.CreditorOverride");
      this.access = (bool) Session.ServerManager.GetServerSetting("Policies.CreditorOverride");
      this.reset();
    }

    private void reset()
    {
      this.resetAll();
      this.partialAccess();
      this.setDirtyFlag(false);
    }

    private void resetAll() => this.chkCreditOverride.Checked = false;

    private void partialAccess()
    {
      if (!Convert.ToBoolean(this.access))
        return;
      this.chkCreditOverride.Checked = true;
    }

    private void enableSaveReset(object sender, EventArgs e) => this.setDirtyFlag(true);

    public override void Reset()
    {
      this.reset();
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      if (!this.IsDirty)
      {
        this.Dispose();
      }
      else
      {
        Session.ServerManager.UpdateServerSetting("Policies.CreditorOverride", (object) (bool) (this.chkCreditOverride.Checked ? (true ? 1 : 0) : (false ? 1 : 0)));
        this.access = (bool) Session.ServerManager.GetServerSetting("Policies.CreditorOverride");
        this.setDirtyFlag(false);
      }
    }

    string IOnlineHelpTarget.GetHelpTargetName() => "Setup\\TPO Disclosure Settings";

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkCreditOverride = new CheckBox();
      this.label1 = new Label();
      this.gcStatementOfDenial = new GroupContainer();
      this.gcStatementOfDenial.SuspendLayout();
      this.SuspendLayout();
      this.chkCreditOverride.AutoSize = true;
      this.chkCreditOverride.Location = new Point(11, 31);
      this.chkCreditOverride.Name = "chkCreditOverride";
      this.chkCreditOverride.Size = new Size(216, 17);
      this.chkCreditOverride.TabIndex = 0;
      this.chkCreditOverride.Text = "Creditor Override on Statement of Denial";
      this.chkCreditOverride.UseVisualStyleBackColor = true;
      this.chkCreditOverride.CheckedChanged += new EventHandler(this.enableSaveReset);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(58, 53);
      this.label1.MaximumSize = new Size(400, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(383, 26);
      this.label1.TabIndex = 1;
      this.label1.Text = "Always use the Correspondent Buyer as the Creditor on the Statement of Denial form when Channel is “Correspondent” and Underwriting is not Delegated.";
      this.gcStatementOfDenial.AutoSize = true;
      this.gcStatementOfDenial.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.gcStatementOfDenial.Borders = AnchorStyles.Top | AnchorStyles.Left;
      this.gcStatementOfDenial.Controls.Add((Control) this.label1);
      this.gcStatementOfDenial.Controls.Add((Control) this.chkCreditOverride);
      this.gcStatementOfDenial.Dock = DockStyle.Fill;
      this.gcStatementOfDenial.HeaderForeColor = SystemColors.ControlText;
      this.gcStatementOfDenial.Location = new Point(0, 0);
      this.gcStatementOfDenial.Name = "gcStatementOfDenial";
      this.gcStatementOfDenial.Size = new Size(514, 324);
      this.gcStatementOfDenial.TabIndex = 1;
      this.gcStatementOfDenial.TabStop = true;
      this.gcStatementOfDenial.Text = "Statement of Denial";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcStatementOfDenial);
      this.Name = nameof (TPODisclosureSetting);
      this.Size = new Size(514, 324);
      this.gcStatementOfDenial.ResumeLayout(false);
      this.gcStatementOfDenial.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
