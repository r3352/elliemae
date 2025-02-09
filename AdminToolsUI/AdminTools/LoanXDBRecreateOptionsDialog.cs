// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanXDBRecreateOptionsDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanXDBRecreateOptionsDialog : Form
  {
    private IContainer components;
    private DialogButtons dialogButtons1;
    private Label labelRetain;
    private Label labelRecreate;
    private RadioButton radKeep;
    private Label label2;
    private RadioButton radDelete;

    public LoanXDBRecreateOptionsDialog()
    {
      this.InitializeComponent();
      this.radKeep.Checked = true;
      this.radDelete.Checked = false;
    }

    public bool KeepTables => this.radKeep.Checked;

    private void labelRetain_Click(object sender, EventArgs e) => this.radKeep.Checked = true;

    private void labelRecreate_Click(object sender, EventArgs e) => this.radDelete.Checked = true;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.dialogButtons1 = new DialogButtons();
      this.labelRetain = new Label();
      this.labelRecreate = new Label();
      this.radKeep = new RadioButton();
      this.label2 = new Label();
      this.radDelete = new RadioButton();
      this.SuspendLayout();
      this.dialogButtons1.DialogResult = DialogResult.OK;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 140);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(391, 44);
      this.dialogButtons1.TabIndex = 8;
      this.labelRetain.AutoSize = true;
      this.labelRetain.Location = new Point(32, 51);
      this.labelRetain.Name = "labelRetain";
      this.labelRetain.Size = new Size(298, 13);
      this.labelRetain.TabIndex = 13;
      this.labelRetain.Text = "(Recommended if data is being used by external applications.)";
      this.labelRetain.Click += new EventHandler(this.labelRetain_Click);
      this.labelRecreate.Location = new Point(32, 88);
      this.labelRecreate.Name = "labelRecreate";
      this.labelRecreate.Size = new Size(347, 31);
      this.labelRecreate.TabIndex = 12;
      this.labelRecreate.Text = "(Recommended if many changes have been made to database fields and data is NOT being used by external applications.)";
      this.labelRecreate.Click += new EventHandler(this.labelRecreate_Click);
      this.radKeep.AutoSize = true;
      this.radKeep.Checked = true;
      this.radKeep.Location = new Point(16, 34);
      this.radKeep.Name = "radKeep";
      this.radKeep.Size = new Size(237, 17);
      this.radKeep.TabIndex = 11;
      this.radKeep.TabStop = true;
      this.radKeep.Text = "Retain fields within the current table structure";
      this.radKeep.UseVisualStyleBackColor = true;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 10);
      this.label2.Name = "label2";
      this.label2.Size = new Size(299, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Select an option for recreating the Reporting Database tables:";
      this.radDelete.Location = new Point(16, 72);
      this.radDelete.Name = "radDelete";
      this.radDelete.Size = new Size(346, 18);
      this.radDelete.TabIndex = 9;
      this.radDelete.Text = "Recreate table structure";
      this.radDelete.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(391, 184);
      this.Controls.Add((Control) this.labelRetain);
      this.Controls.Add((Control) this.labelRecreate);
      this.Controls.Add((Control) this.radKeep);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.radDelete);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanXDBRecreateOptionsDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Recreate Reporting Database";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
