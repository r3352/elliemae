// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ActivityNoteDialog
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
  public class ActivityNoteDialog : Form
  {
    private IContainer components;
    private TextBox txtNote;
    private Button btnClose;

    public string Note
    {
      get => this.txtNote.Text.Trim();
      set
      {
        this.txtNote.Text = value == null || !(string.Empty != value.Trim()) ? string.Empty : value.Trim();
      }
    }

    public ActivityNoteDialog() => this.InitializeComponent();

    private void btnClose_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void ActivityNoteDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnClose.PerformClick();
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
      this.btnClose = new Button();
      this.SuspendLayout();
      this.txtNote.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtNote.Location = new Point(10, 10);
      this.txtNote.MaxLength = 3700;
      this.txtNote.Multiline = true;
      this.txtNote.Name = "txtNote";
      this.txtNote.ReadOnly = true;
      this.txtNote.ScrollBars = ScrollBars.Both;
      this.txtNote.Size = new Size(552, 182);
      this.txtNote.TabIndex = 1;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(487, 202);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 23);
      this.btnClose.TabIndex = 0;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.AcceptButton = (IButtonControl) this.btnClose;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(574, 233);
      this.Controls.Add((Control) this.btnClose);
      this.Controls.Add((Control) this.txtNote);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ActivityNoteDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Notes";
      this.KeyUp += new KeyEventHandler(this.ActivityNoteDialog_KeyUp);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
