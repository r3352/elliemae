// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CompleteTasksDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CompleteTasksDialog : Form
  {
    private IContainer components;
    private TextBox txtNote;
    private Button btnComplete;
    private Button btnCancel;
    private Label lblDescription;

    public string Note
    {
      get => this.txtNote.Text.Trim();
      set
      {
        this.txtNote.Text = value == null || !(string.Empty != value.Trim()) ? string.Empty : value.Trim();
      }
    }

    public CompleteTasksDialog() => this.InitializeComponent();

    private void btnComplete_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void CompleteTasksDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtNote = new TextBox();
      this.btnComplete = new Button();
      this.btnCancel = new Button();
      this.lblDescription = new Label();
      this.SuspendLayout();
      this.txtNote.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNote.Location = new Point(10, 34);
      this.txtNote.MaxLength = 3700;
      this.txtNote.Multiline = true;
      this.txtNote.Name = "txtNote";
      this.txtNote.ScrollBars = ScrollBars.Both;
      this.txtNote.Size = new Size(552, 182);
      this.txtNote.TabIndex = 2;
      this.btnComplete.Location = new Point(429, 226);
      this.btnComplete.Name = "btnComplete";
      this.btnComplete.Size = new Size(67, 23);
      this.btnComplete.TabIndex = 0;
      this.btnComplete.Text = "&Complete";
      this.btnComplete.UseVisualStyleBackColor = true;
      this.btnComplete.Click += new EventHandler(this.btnComplete_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(506, 226);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(56, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "C&ancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(10, 10);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(479, 14);
      this.lblDescription.TabIndex = 1;
      this.lblDescription.Text = "The selected task(s) will be marked as 'Completed'. Add notes to be recorded in History (optional):";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(574, 259);
      this.Controls.Add((Control) this.txtNote);
      this.Controls.Add((Control) this.btnComplete);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblDescription);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CompleteTasksDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Complete Tasks";
      this.KeyUp += new KeyEventHandler(this.CompleteTasksDialog_KeyUp);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
