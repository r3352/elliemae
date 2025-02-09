// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.TemplateAppendValueConfig
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class TemplateAppendValueConfig : SettingsUserControl
  {
    private IContainer components;
    private CheckBox chbClosingCost;
    private CheckBox chbLoanProgram;
    private CheckBox chbNewLoan;
    private GroupContainer groupContainer1;

    public TemplateAppendValueConfig(SetUpContainer setupContainer)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.initialPageValue();
      this.setDirtyFlag(false);
    }

    private void initialPageValue()
    {
      this.chbNewLoan.Checked = (bool) Session.ServerManager.GetServerSetting("Policies.AppendNewLoanTemplate");
      this.chbLoanProgram.Checked = (bool) Session.ServerManager.GetServerSetting("Policies.AppendLoanProgram");
      this.chbClosingCost.Checked = (bool) Session.ServerManager.GetServerSetting("Policies.AppendClosingCost");
    }

    public override void Save()
    {
      Session.ServerManager.UpdateServerSetting("Policies.AppendNewLoanTemplate", (object) this.chbNewLoan.Checked);
      Session.ServerManager.UpdateServerSetting("Policies.AppendLoanProgram", (object) this.chbLoanProgram.Checked);
      Session.ServerManager.UpdateServerSetting("Policies.AppendClosingCost", (object) this.chbClosingCost.Checked);
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      this.initialPageValue();
      this.setDirtyFlag(false);
    }

    private void chkBox_Click(object sender, EventArgs e) => this.setDirtyFlag(true);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chbClosingCost = new CheckBox();
      this.chbLoanProgram = new CheckBox();
      this.chbNewLoan = new CheckBox();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.chbClosingCost.AutoSize = true;
      this.chbClosingCost.Location = new Point(11, 75);
      this.chbClosingCost.Name = "chbClosingCost";
      this.chbClosingCost.Size = new Size(131, 17);
      this.chbClosingCost.TabIndex = 4;
      this.chbClosingCost.Text = "Closing Cost Template";
      this.chbClosingCost.UseVisualStyleBackColor = true;
      this.chbClosingCost.CheckedChanged += new EventHandler(this.chkBox_Click);
      this.chbLoanProgram.AutoSize = true;
      this.chbLoanProgram.Location = new Point(11, 53);
      this.chbLoanProgram.Name = "chbLoanProgram";
      this.chbLoanProgram.Size = new Size(139, 17);
      this.chbLoanProgram.TabIndex = 3;
      this.chbLoanProgram.Text = "Loan Program Template";
      this.chbLoanProgram.UseVisualStyleBackColor = true;
      this.chbLoanProgram.CheckedChanged += new EventHandler(this.chkBox_Click);
      this.chbNewLoan.AutoSize = true;
      this.chbNewLoan.Location = new Point(11, 31);
      this.chbNewLoan.Name = "chbNewLoan";
      this.chbNewLoan.Size = new Size(122, 17);
      this.chbNewLoan.TabIndex = 2;
      this.chbNewLoan.Text = "New Loan Template";
      this.chbNewLoan.UseVisualStyleBackColor = true;
      this.chbNewLoan.CheckedChanged += new EventHandler(this.chkBox_Click);
      this.groupContainer1.Controls.Add((Control) this.chbClosingCost);
      this.groupContainer1.Controls.Add((Control) this.chbNewLoan);
      this.groupContainer1.Controls.Add((Control) this.chbLoanProgram);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(506, 360);
      this.groupContainer1.TabIndex = 6;
      this.groupContainer1.Text = "Select the templates that will only apply the fields that contain a value.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.groupContainer1);
      this.Name = nameof (TemplateAppendValueConfig);
      this.Size = new Size(506, 360);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
