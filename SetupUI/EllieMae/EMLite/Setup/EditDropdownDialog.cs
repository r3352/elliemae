// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.EditDropdownDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class EditDropdownDialog : Form
  {
    private Label label3;
    private Button okBtn;
    private Button cancelBtn;
    private TextBox textBoxOption;
    private System.ComponentModel.Container components;
    private string[] existingValues;
    private FieldFormat fieldFormat;
    private string newOption = string.Empty;

    public EditDropdownDialog(string[] values, FieldFormat fieldFormat)
    {
      this.fieldFormat = fieldFormat;
      this.existingValues = values;
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public string NewOption => this.newOption;

    private void InitializeComponent()
    {
      this.label3 = new Label();
      this.textBoxOption = new TextBox();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.SuspendLayout();
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 16);
      this.label3.Name = "label3";
      this.label3.Size = new Size(93, 13);
      this.label3.TabIndex = 2;
      this.label3.Text = "Dropdown Option:";
      this.textBoxOption.Location = new Point(14, 35);
      this.textBoxOption.Name = "textBoxOption";
      this.textBoxOption.Size = new Size(228, 20);
      this.textBoxOption.TabIndex = 0;
      this.textBoxOption.KeyUp += new KeyEventHandler(this.textField_KeyUp);
      this.okBtn.Location = new Point(88, 68);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 15;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(168, 68);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 16;
      this.cancelBtn.Text = "Cancel";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(260, 106);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.textBoxOption);
      this.Controls.Add((Control) this.label3);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EditDropdownDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Dropdown Option";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string strB = this.textBoxOption.Text.Trim();
      if (this.fieldFormat == FieldFormat.DATE)
      {
        DateTime date = Utils.ParseDate((object) strB);
        if (date == DateTime.MinValue)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The field format is a date format. Please enter a valid date value.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.textBoxOption.Focus();
          return;
        }
        strB = date.ToShortDateString();
      }
      if (strB == "")
        strB = "<Clear>";
      for (int index = 0; index < this.existingValues.Length; ++index)
      {
        if (string.Compare(this.existingValues[index], strB, true) == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The dropdown list already contains option '" + strB + "'.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.newOption = strB;
      this.DialogResult = DialogResult.OK;
    }

    private void numberField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (!char.IsDigit(e.KeyChar))
      {
        char keyChar = e.KeyChar;
        if (!keyChar.Equals('.'))
        {
          keyChar = e.KeyChar;
          if (!keyChar.Equals('-'))
          {
            e.Handled = true;
            return;
          }
        }
      }
      e.Handled = false;
    }

    private void textField_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, this.fieldFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }
  }
}
