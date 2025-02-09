// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.PipelineSettingsPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class PipelineSettingsPage : SettingsUserControl
  {
    private IContainer components;
    private GroupContainer groupContainer1;
    private Label label4;
    private Label label3;
    private NumericUpDown upDownInterval;
    private CheckBox chkBoxEnableRefresh;

    public PipelineSettingsPage(SetUpContainer container)
      : base(container)
    {
      this.InitializeComponent();
      this.upDownInterval.Minimum = (Decimal) PipelineSettings.MinimumRefreshInterval / 60M;
      this.Reset();
    }

    private void chkBoxEnableRefresh_CheckedChanged(object sender, EventArgs e)
    {
      this.upDownInterval.Enabled = this.chkBoxEnableRefresh.Checked;
      this.setDirtyFlag(true);
    }

    private void upDownInterval_ValueChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    public override void Reset()
    {
      int refreshInterval = PipelineSettings.RefreshInterval;
      if (refreshInterval <= 0)
      {
        this.chkBoxEnableRefresh.Checked = false;
      }
      else
      {
        this.chkBoxEnableRefresh.Checked = true;
        this.upDownInterval.Value = (Decimal) refreshInterval / 60M;
      }
      this.setDirtyFlag(false);
    }

    public override void Save()
    {
      PipelineSettings.RefreshInterval = !this.chkBoxEnableRefresh.Checked ? -1 : (int) (this.upDownInterval.Value * 60M);
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
      this.groupContainer1 = new GroupContainer();
      this.label4 = new Label();
      this.label3 = new Label();
      this.upDownInterval = new NumericUpDown();
      this.chkBoxEnableRefresh = new CheckBox();
      this.groupContainer1.SuspendLayout();
      this.upDownInterval.BeginInit();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.upDownInterval);
      this.groupContainer1.Controls.Add((Control) this.chkBoxEnableRefresh);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(497, 328);
      this.groupContainer1.TabIndex = 104;
      this.groupContainer1.Text = "Pipeline Configuration";
      this.label4.Location = new Point(204, 68);
      this.label4.Name = "label4";
      this.label4.Size = new Size(71, 23);
      this.label4.TabIndex = 100;
      this.label4.Text = "minutes";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.Location = new Point(9, 68);
      this.label3.Name = "label3";
      this.label3.Size = new Size(134, 23);
      this.label3.TabIndex = 100;
      this.label3.Text = "Refresh the pipeline every";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.upDownInterval.DecimalPlaces = 1;
      this.upDownInterval.Increment = new Decimal(new int[4]
      {
        5,
        0,
        0,
        65536
      });
      this.upDownInterval.Location = new Point(145, 71);
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
      this.chkBoxEnableRefresh.Checked = true;
      this.chkBoxEnableRefresh.CheckState = CheckState.Checked;
      this.chkBoxEnableRefresh.Location = new Point(12, 39);
      this.chkBoxEnableRefresh.Name = "chkBoxEnableRefresh";
      this.chkBoxEnableRefresh.Size = new Size(196, 16);
      this.chkBoxEnableRefresh.TabIndex = 1;
      this.chkBoxEnableRefresh.Text = "Enable auto-refresh for the Pipeline";
      this.chkBoxEnableRefresh.CheckedChanged += new EventHandler(this.chkBoxEnableRefresh_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (PipelineSettingsPage);
      this.Size = new Size(497, 328);
      this.groupContainer1.ResumeLayout(false);
      this.upDownInterval.EndInit();
      this.ResumeLayout(false);
    }
  }
}
