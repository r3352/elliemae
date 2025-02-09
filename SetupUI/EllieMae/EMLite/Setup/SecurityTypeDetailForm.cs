// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SecurityTypeDetailForm
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SecurityTypeDetailForm : Form
  {
    private DataTable securityTypesTable;
    private int? editItemIndex;
    private IContainer components;
    private TextBox txtName;
    private TextBox txtTerm1;
    private TextBox txtTerm2;
    private ComboBox cboProgramType;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Button buttonOk;
    private Button buttonCancel;
    private Label label6;
    private Label label7;

    public SecurityTypeDetailForm(DataTable securityTypesTable, int? editItemIndex)
    {
      this.InitializeComponent();
      this.securityTypesTable = securityTypesTable;
      this.editItemIndex = editItemIndex;
      if (!editItemIndex.HasValue)
        return;
      DataRow row = securityTypesTable.Rows[editItemIndex.Value];
      this.txtName.Text = (string) row["Name"];
      this.cboProgramType.Text = (string) row[nameof (ProgramType)];
      this.txtTerm1.Text = (int) row[nameof (Term1)] == -1 ? "" : row[nameof (Term1)].ToString();
      this.txtTerm2.Text = (int) row[nameof (Term2)] == -1 ? "" : row[nameof (Term2)].ToString();
    }

    private bool ValidateForm(out string validationMessage)
    {
      bool flag = false;
      validationMessage = "";
      if (this.txtName.Text.Trim() == "")
      {
        validationMessage = "Please enter a Security Type.";
        return false;
      }
      for (int index = 0; index < this.securityTypesTable.Rows.Count; ++index)
      {
        if ((string) this.securityTypesTable.Rows[index]["Name"] == this.txtName.Text.Trim())
        {
          if (this.editItemIndex.HasValue)
          {
            int num = index;
            int? editItemIndex = this.editItemIndex;
            int valueOrDefault = editItemIndex.GetValueOrDefault();
            if (num == valueOrDefault & editItemIndex.HasValue)
              continue;
          }
          flag = true;
        }
      }
      if (flag)
      {
        validationMessage = "The Security Type Name is already in use.";
        return false;
      }
      if (this.Term1 <= this.Term2)
        return true;
      validationMessage = "This minimum term must be less than or equal to the maximum.";
      return false;
    }

    private bool Save()
    {
      string validationMessage;
      if (this.ValidateForm(out validationMessage))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, validationMessage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      if (!this.Save())
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void numericField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }

    public string SecurityTypeName => this.txtName.Text.Trim();

    public string ProgramType => this.cboProgramType.Text.Trim();

    public int Term1
    {
      get => !(this.txtTerm1.Text.Trim() == "") ? Convert.ToInt32(this.txtTerm1.Text) : -1;
    }

    public int Term2
    {
      get => !(this.txtTerm2.Text.Trim() == "") ? Convert.ToInt32(this.txtTerm2.Text) : -1;
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtName = new TextBox();
      this.txtTerm1 = new TextBox();
      this.txtTerm2 = new TextBox();
      this.cboProgramType = new ComboBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.buttonOk = new Button();
      this.buttonCancel = new Button();
      this.label6 = new Label();
      this.label7 = new Label();
      this.SuspendLayout();
      this.txtName.Location = new Point(103, 12);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(168, 20);
      this.txtName.TabIndex = 0;
      this.txtTerm1.Location = new Point(103, 65);
      this.txtTerm1.Name = "txtTerm1";
      this.txtTerm1.Size = new Size(50, 20);
      this.txtTerm1.TabIndex = 2;
      this.txtTerm1.KeyPress += new KeyPressEventHandler(this.numericField_KeyPress);
      this.txtTerm2.Location = new Point(174, 65);
      this.txtTerm2.Name = "txtTerm2";
      this.txtTerm2.Size = new Size(50, 20);
      this.txtTerm2.TabIndex = 3;
      this.txtTerm2.KeyPress += new KeyPressEventHandler(this.numericField_KeyPress);
      this.cboProgramType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboProgramType.FormattingEnabled = true;
      this.cboProgramType.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Fixed",
        (object) "Adjustable"
      });
      this.cboProgramType.Location = new Point(103, 38);
      this.cboProgramType.Name = "cboProgramType";
      this.cboProgramType.Size = new Size(168, 21);
      this.cboProgramType.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 15);
      this.label1.Name = "label1";
      this.label1.Size = new Size(72, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Security Type";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(14, 41);
      this.label2.Name = "label2";
      this.label2.Size = new Size(73, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Program Type";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(14, 69);
      this.label3.Name = "label3";
      this.label3.Size = new Size(31, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Term";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(284, 69);
      this.label4.Name = "label4";
      this.label4.Size = new Size(50, 13);
      this.label4.TabIndex = 7;
      this.label4.Text = "(optional)";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(230, 69);
      this.label5.Name = "label5";
      this.label5.Size = new Size(29, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "mths";
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.Location = new Point(174, 104);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(75, 23);
      this.buttonOk.TabIndex = 4;
      this.buttonOk.Text = "OK";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(259, 104);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(158, 69);
      this.label6.Name = "label6";
      this.label6.Size = new Size(10, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "-";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(284, 41);
      this.label7.Name = "label7";
      this.label7.Size = new Size(50, 13);
      this.label7.TabIndex = 12;
      this.label7.Text = "(optional)";
      this.AcceptButton = (IButtonControl) this.buttonOk;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(346, 139);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOk);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cboProgramType);
      this.Controls.Add((Control) this.txtTerm2);
      this.Controls.Add((Control) this.txtTerm1);
      this.Controls.Add((Control) this.txtName);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SecurityTypeDetailForm);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Create/Edit Security Type Field Values";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
