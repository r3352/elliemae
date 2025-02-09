// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.GroupNameDuplicate
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class GroupNameDuplicate : Form
  {
    private Label label1;
    private TextBox textBox1;
    private Button btnSave;
    private Button btnCancel;
    private System.ComponentModel.Container components;

    public GroupNameDuplicate(string selectedName, bool rename)
    {
      this.InitializeComponent();
      if (selectedName == "")
        this.label1.Text = "Please enter a group name.";
      else if (selectedName.Length > 64)
      {
        this.label1.Text = "Group name can not be longer than 64 characters.  Please enter another group name.";
      }
      else
      {
        if (rename)
          this.label1.Text = "Please enter a new group name.";
        else
          this.label1.Text = "The group name '" + selectedName + "' already exists. Please enter another group name.";
        this.textBox1.Text = selectedName;
      }
      this.btnSave.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string GetNewName => this.textBox1.Text.Trim();

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.textBox1 = new TextBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(276, 56);
      this.label1.TabIndex = 0;
      this.textBox1.Location = new Point(4, 68);
      this.textBox1.MaxLength = 64;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new Size(280, 20);
      this.textBox1.TabIndex = 1;
      this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
      this.textBox1.KeyUp += new KeyEventHandler(this.textBox1_KeyUp);
      this.btnSave.DialogResult = DialogResult.OK;
      this.btnSave.Location = new Point(128, 92);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 2;
      this.btnSave.Text = "OK";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(212, 92);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(292, 126);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.textBox1);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.Name = nameof (GroupNameDuplicate);
      this.Text = "Contact Group Name";
      this.KeyUp += new KeyEventHandler(this.GroupNameDuplicate_KeyUp);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {
      if (this.textBox1.Text.Trim() != "")
        this.btnSave.Enabled = true;
      else if (this.textBox1.Text.Length > 64)
        this.btnSave.Enabled = false;
      else
        this.btnSave.Enabled = false;
    }

    private void textBox1_KeyUp(object sender, KeyEventArgs e)
    {
      if (!this.btnSave.Enabled || e.KeyValue != 13)
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void GroupNameDuplicate_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
