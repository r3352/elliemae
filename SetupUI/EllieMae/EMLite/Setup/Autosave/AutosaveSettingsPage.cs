// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.Autosave.AutosaveSettingsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup.Autosave
{
  public class AutosaveSettingsPage : SettingsUserControl
  {
    private bool autosaveEnabled;
    private Decimal autosaveInterval;
    private CheckBox chkBoxEnableAutosave;
    private Label label3;
    private NumericUpDown upDownInterval;
    private Label label4;
    private GroupContainer groupContainer1;
    private System.ComponentModel.Container components;

    public AutosaveSettingsPage(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      bool serverSettingEnabled = AutosaveConfigManager.ServerSettingEnabled;
      this.autosaveEnabled = AutosaveConfigManager.IsEnabledOnClient();
      Decimal num = (Decimal) AutosaveConfigManager.GetInterval() / 60M;
      this.autosaveInterval = !(num < this.upDownInterval.Minimum) ? (!(num > this.upDownInterval.Maximum) ? num : this.upDownInterval.Maximum) : this.upDownInterval.Minimum;
      this.upDownInterval.Value = this.autosaveInterval;
      this.chkBoxEnableAutosave.Checked = serverSettingEnabled && this.autosaveEnabled;
      this.upDownInterval.Enabled = this.chkBoxEnableAutosave.Checked;
      this.chkBoxEnableAutosave.Enabled = serverSettingEnabled;
      this.setupContainer.ButtonResetEnabled = false;
      if (!serverSettingEnabled || num >= this.upDownInterval.Minimum && num <= this.upDownInterval.Maximum)
        this.setupContainer.ButtonResetEnabled = false;
      this.setDirtyFlag(false);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label4 = new Label();
      this.upDownInterval = new NumericUpDown();
      this.label3 = new Label();
      this.chkBoxEnableAutosave = new CheckBox();
      this.groupContainer1 = new GroupContainer();
      this.upDownInterval.BeginInit();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label4.Location = new Point(220, 58);
      this.label4.Name = "label4";
      this.label4.Size = new Size(71, 23);
      this.label4.TabIndex = 100;
      this.label4.Text = "minutes";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.upDownInterval.DecimalPlaces = 1;
      this.upDownInterval.Location = new Point(162, 61);
      this.upDownInterval.Maximum = new Decimal(new int[4]
      {
        60,
        0,
        0,
        0
      });
      this.upDownInterval.Minimum = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
      this.upDownInterval.Name = "upDownInterval";
      this.upDownInterval.Size = new Size(56, 20);
      this.upDownInterval.TabIndex = 2;
      this.upDownInterval.Value = new Decimal(new int[4]
      {
        20,
        0,
        0,
        65536
      });
      this.upDownInterval.ValueChanged += new EventHandler(this.upDownInterval_ValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(27, 63);
      this.label3.Name = "label3";
      this.label3.Size = new Size(126, 13);
      this.label3.TabIndex = 100;
      this.label3.Text = "Time between autosaves";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.chkBoxEnableAutosave.Checked = true;
      this.chkBoxEnableAutosave.CheckState = CheckState.Checked;
      this.chkBoxEnableAutosave.Location = new Point(10, 35);
      this.chkBoxEnableAutosave.Name = "chkBoxEnableAutosave";
      this.chkBoxEnableAutosave.Size = new Size(196, 16);
      this.chkBoxEnableAutosave.TabIndex = 1;
      this.chkBoxEnableAutosave.Text = "Enable autosave for this computer";
      this.chkBoxEnableAutosave.CheckedChanged += new EventHandler(this.chkBoxEnableAutosave_CheckedChanged);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.upDownInterval);
      this.groupContainer1.Controls.Add((Control) this.chkBoxEnableAutosave);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(380, 205);
      this.groupContainer1.TabIndex = 103;
      this.groupContainer1.Text = "Autosave Configuration";
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (AutosaveSettingsPage);
      this.Size = new Size(380, 205);
      this.upDownInterval.EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void chkBoxEnableAutosave_CheckedChanged(object sender, EventArgs e)
    {
      this.upDownInterval.Enabled = this.chkBoxEnableAutosave.Checked;
      this.setDirtyFlag(true);
    }

    private void upDownInterval_ValueChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    public override void Reset()
    {
      this.chkBoxEnableAutosave.Checked = this.autosaveEnabled;
      this.upDownInterval.Value = this.autosaveInterval;
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      AutosaveConfigManager.SaveSettings(this.chkBoxEnableAutosave.Checked, (int) (this.upDownInterval.Value * 60M));
      this.autosaveEnabled = this.chkBoxEnableAutosave.Checked;
      this.autosaveInterval = this.upDownInterval.Value;
      this.setDirtyFlag(false);
    }
  }
}
