// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanXDBPopulateOptionsDialog
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.UI;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanXDBPopulateOptionsDialog : Form
  {
    private IContainer components;
    private RadioButton radPendingOnly;
    private Label label1;
    private RadioButton radAll;
    private DialogButtons dialogButtons1;

    public LoanXDBPopulateOptionsDialog() => this.InitializeComponent();

    public bool PendingFieldsOnly => this.radPendingOnly.Checked;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.radPendingOnly = new RadioButton();
      this.label1 = new Label();
      this.radAll = new RadioButton();
      this.dialogButtons1 = new DialogButtons();
      this.SuspendLayout();
      this.radPendingOnly.Checked = true;
      this.radPendingOnly.Location = new Point(16, 34);
      this.radPendingOnly.Name = "radPendingOnly";
      this.radPendingOnly.Size = new Size(346, 18);
      this.radPendingOnly.TabIndex = 1;
      this.radPendingOnly.TabStop = true;
      this.radPendingOnly.Text = "Populate only fields that are known to be out-of-date";
      this.radPendingOnly.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(307, 14);
      this.label1.TabIndex = 2;
      this.label1.Text = "Select the option for populating the Reporting Database tables:";
      this.radAll.AutoSize = true;
      this.radAll.Location = new Point(16, 56);
      this.radAll.Name = "radAll";
      this.radAll.Size = new Size(288, 18);
      this.radAll.TabIndex = 3;
      this.radAll.Text = "Re-populate all data (this will take considerably longer)";
      this.radAll.UseVisualStyleBackColor = true;
      this.dialogButtons1.DialogResult = DialogResult.OK;
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 95);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(376, 44);
      this.dialogButtons1.TabIndex = 4;
      this.AcceptButton = (IButtonControl) this.dialogButtons1;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(376, 139);
      this.Controls.Add((Control) this.dialogButtons1);
      this.Controls.Add((Control) this.radAll);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.radPendingOnly);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanXDBPopulateOptionsDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Populate Reporting Database";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
