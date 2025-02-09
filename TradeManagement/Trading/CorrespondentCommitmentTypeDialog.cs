// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentCommitmentTypeDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentCommitmentTypeDialog : Form
  {
    private IContainer components;
    private RadioButton rbtnBestEffort;
    private RadioButton rbtnMandatory;
    private Button btnCancel;
    private Button btnOk;

    public MasterCommitmentType CommitmentType { get; set; }

    public CorrespondentCommitmentTypeDialog() => this.InitializeComponent();

    private void btnOk_Click(object sender, EventArgs e)
    {
      this.CommitmentType = MasterCommitmentType.None;
      if (this.rbtnBestEffort.Checked)
        this.CommitmentType = MasterCommitmentType.BestEfforts;
      else if (this.rbtnMandatory.Checked)
      {
        this.CommitmentType = MasterCommitmentType.Mandatory;
      }
      else
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select Commitment Type.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.None;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rbtnBestEffort = new RadioButton();
      this.rbtnMandatory = new RadioButton();
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.SuspendLayout();
      this.rbtnBestEffort.AutoSize = true;
      this.rbtnBestEffort.Location = new Point(42, 13);
      this.rbtnBestEffort.Name = "rbtnBestEffort";
      this.rbtnBestEffort.Size = new Size(74, 17);
      this.rbtnBestEffort.TabIndex = 0;
      this.rbtnBestEffort.TabStop = true;
      this.rbtnBestEffort.Text = "Best Effort";
      this.rbtnBestEffort.UseVisualStyleBackColor = true;
      this.rbtnMandatory.AutoSize = true;
      this.rbtnMandatory.Location = new Point(42, 37);
      this.rbtnMandatory.Name = "rbtnMandatory";
      this.rbtnMandatory.Size = new Size(75, 17);
      this.rbtnMandatory.TabIndex = 1;
      this.rbtnMandatory.TabStop = true;
      this.rbtnMandatory.Text = "Mandatory";
      this.rbtnMandatory.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(117, 65);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 22;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Location = new Point(36, 65);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(75, 23);
      this.btnOk.TabIndex = 21;
      this.btnOk.Text = "OK";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(228, 97);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.rbtnMandatory);
      this.Controls.Add((Control) this.rbtnBestEffort);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "CorrespondentMasterCommitmentType";
      this.ShowIcon = false;
      this.Text = "Select New Master Commitment";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
