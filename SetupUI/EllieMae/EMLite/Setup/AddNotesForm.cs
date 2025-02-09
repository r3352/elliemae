// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AddNotesForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AddNotesForm : Form
  {
    private bool readOnly;
    private IContainer components;
    private TextBox txtNotes;
    private Button btnOK;
    private Button btnCancel;

    public AddNotesForm(string notesDetails)
      : this()
    {
      this.txtNotes.Text = notesDetails.Replace("\n", "\r\n");
      this.readOnly = true;
      this.btnOK.Visible = false;
      this.txtNotes.ReadOnly = true;
      this.btnCancel.Text = "&Close";
      this.Text = "View Notes";
    }

    public AddNotesForm()
    {
      this.InitializeComponent();
      this.btnOK.Enabled = false;
    }

    private void btnOK_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void txtNotes_TextChanged(object sender, EventArgs e)
    {
      if (this.readOnly)
        return;
      string text = this.txtNotes.Text;
      if (this.txtNotes.SelectionStart >= this.txtNotes.Text.Length)
        this.txtNotes.SelectionStart = this.txtNotes.Text.Length;
      if (this.txtNotes.SelectionStart == 0 && !this.txtNotes.Text.EndsWith("\n"))
        this.txtNotes.SelectionStart = 1;
      this.btnOK.Enabled = this.txtNotes.Text.Trim() != string.Empty;
    }

    public string NotesDetails
    {
      get
      {
        this.txtNotes.TextChanged -= new EventHandler(this.txtNotes_TextChanged);
        this.txtNotes.Text = this.txtNotes.Text.Replace("\t", " ");
        this.txtNotes.Text = this.txtNotes.Text.Replace("\n", "\r\n");
        this.txtNotes.TextChanged += new EventHandler(this.txtNotes_TextChanged);
        string[] strArray = this.txtNotes.Text.Trim().Split('\r');
        string notesDetails = "";
        foreach (string str in strArray)
        {
          if (str != "")
            notesDetails += str;
        }
        return notesDetails;
      }
    }

    private void txtNotes_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!this.readOnly)
        return;
      e.Handled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtNotes = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.txtNotes.Location = new Point(12, 12);
      this.txtNotes.MaxLength = 4096;
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = ScrollBars.Both;
      this.txtNotes.Size = new Size(655, 344);
      this.txtNotes.TabIndex = 0;
      this.txtNotes.TextChanged += new EventHandler(this.txtNotes_TextChanged);
      this.txtNotes.KeyPress += new KeyPressEventHandler(this.txtNotes_KeyPress);
      this.btnOK.Location = new Point(511, 366);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(592, 366);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(679, 397);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtNotes);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddNotesForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Notes";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
