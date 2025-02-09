// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.StatusPrintOptionForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class StatusPrintOptionForm : Form
  {
    private bool dateOnly;
    private IContainer components;
    private DialogButtons dialogButtons1;
    private RadioButton rdoDate;
    private RadioButton rdoName;
    private RadioButton rdoDateName;

    public StatusPrintOptionForm(bool dateOnly, string currentOption)
    {
      this.dateOnly = dateOnly;
      this.InitializeComponent();
      if (dateOnly)
      {
        this.rdoDateName.Visible = false;
        this.rdoName.Text = "No Date";
        if (currentOption == "Date")
          this.rdoDate.Checked = true;
        else
          this.rdoName.Checked = true;
      }
      else
      {
        switch (currentOption)
        {
          case "Date":
            this.rdoDate.Checked = true;
            break;
          case "Name":
            this.rdoName.Checked = true;
            break;
          default:
            this.rdoDateName.Checked = true;
            break;
        }
      }
    }

    private void dialogButtons1_OK(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
    }

    public int PrintOption
    {
      get
      {
        if (this.dateOnly && this.rdoName.Checked)
          return 0;
        if (this.rdoDate.Checked)
          return 1;
        return this.rdoName.Checked ? 2 : 3;
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
      this.dialogButtons1 = new DialogButtons();
      this.rdoDate = new RadioButton();
      this.rdoName = new RadioButton();
      this.rdoDateName = new RadioButton();
      this.SuspendLayout();
      this.dialogButtons1.Dock = DockStyle.Bottom;
      this.dialogButtons1.Location = new Point(0, 92);
      this.dialogButtons1.Name = "dialogButtons1";
      this.dialogButtons1.Size = new Size(209, 44);
      this.dialogButtons1.TabIndex = 0;
      this.dialogButtons1.OK += new EventHandler(this.dialogButtons1_OK);
      this.rdoDate.AutoSize = true;
      this.rdoDate.Location = new Point(42, 12);
      this.rdoDate.Name = "rdoDate";
      this.rdoDate.Size = new Size(48, 17);
      this.rdoDate.TabIndex = 1;
      this.rdoDate.TabStop = true;
      this.rdoDate.Text = "Date";
      this.rdoDate.UseVisualStyleBackColor = true;
      this.rdoName.AutoSize = true;
      this.rdoName.Location = new Point(42, 35);
      this.rdoName.Name = "rdoName";
      this.rdoName.Size = new Size(53, 17);
      this.rdoName.TabIndex = 2;
      this.rdoName.TabStop = true;
      this.rdoName.Text = "Name";
      this.rdoName.UseVisualStyleBackColor = true;
      this.rdoDateName.AutoSize = true;
      this.rdoDateName.Location = new Point(42, 58);
      this.rdoDateName.Name = "rdoDateName";
      this.rdoDateName.Size = new Size(100, 17);
      this.rdoDateName.TabIndex = 3;
      this.rdoDateName.TabStop = true;
      this.rdoDateName.Text = "Date and Name";
      this.rdoDateName.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(209, 136);
      this.Controls.Add((Control) this.rdoDateName);
      this.Controls.Add((Control) this.rdoName);
      this.Controls.Add((Control) this.rdoDate);
      this.Controls.Add((Control) this.dialogButtons1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (StatusPrintOptionForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Status Print Options";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
