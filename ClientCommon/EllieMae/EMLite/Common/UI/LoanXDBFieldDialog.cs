// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.LoanXDBFieldDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class LoanXDBFieldDialog : Form
  {
    private TextBox textBoxID;
    private Label label1;
    private TextBox textBoxDescription;
    private Label label3;
    private Button btnCancel;
    private System.ComponentModel.Container components;
    private Button btnOK;
    private Label label2;
    private Label label5;
    private TextBox textBoxSize;
    private TextBox textBoxType;
    private Panel panel1;
    private Panel panel3;
    private Label label4;
    private TextBox textBoxTableName;
    private ComboBox cboComortgagor;
    private Label label6;
    private LoanXDBField dbField;

    public LoanXDBFieldDialog(LoanXDBField dbField)
    {
      this.InitializeComponent();
      this.dbField = dbField;
      this.textBoxID.Text = dbField.FieldID;
      this.textBoxDescription.Text = dbField.Description;
      this.textBoxTableName.Text = dbField.TableName;
      if (this.dbField.FieldType == LoanXDBTableList.TableTypes.IsDate)
      {
        this.textBoxType.Text = "Date";
        this.textBoxSize.Text = "4";
      }
      else if (this.dbField.FieldType == LoanXDBTableList.TableTypes.IsNumeric)
      {
        this.textBoxType.Text = "Numeric";
        this.textBoxSize.Text = "9";
      }
      else
      {
        this.textBoxSize.ReadOnly = false;
        this.textBoxType.Text = "String";
        this.textBoxSize.Text = this.dbField.FieldSizeToString;
      }
      FieldDefinition field = EncompassFields.GetField(dbField.FieldID);
      int num = dbField.ComortgagorPair - 1;
      if (num < 0)
        num = 0;
      if (field == null || field.Category == FieldCategory.Common)
        this.cboComortgagor.Enabled = false;
      else
        this.cboComortgagor.SelectedIndex = num;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public LoanXDBField DBField => this.dbField;

    private void InitializeComponent()
    {
      this.textBoxID = new TextBox();
      this.label1 = new Label();
      this.textBoxDescription = new TextBox();
      this.label3 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.textBoxSize = new TextBox();
      this.label2 = new Label();
      this.label5 = new Label();
      this.textBoxType = new TextBox();
      this.panel1 = new Panel();
      this.panel3 = new Panel();
      this.label4 = new Label();
      this.textBoxTableName = new TextBox();
      this.cboComortgagor = new ComboBox();
      this.label6 = new Label();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.SuspendLayout();
      this.textBoxID.Location = new Point(91, 10);
      this.textBoxID.Name = "textBoxID";
      this.textBoxID.ReadOnly = true;
      this.textBoxID.Size = new Size(240, 20);
      this.textBoxID.TabIndex = 6;
      this.textBoxID.TabStop = false;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(41, 14);
      this.label1.TabIndex = 7;
      this.label1.Text = "Field ID";
      this.textBoxDescription.Location = new Point(91, 32);
      this.textBoxDescription.MaxLength = 100;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.Size = new Size(240, 20);
      this.textBoxDescription.TabIndex = 0;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(9, 35);
      this.label3.Name = "label3";
      this.label3.Size = new Size(61, 14);
      this.label3.TabIndex = 11;
      this.label3.Text = "Description";
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(178, 140);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(258, 140);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.textBoxSize.Location = new Point(248, 78);
      this.textBoxSize.MaxLength = 4;
      this.textBoxSize.Name = "textBoxSize";
      this.textBoxSize.ReadOnly = true;
      this.textBoxSize.Size = new Size(64, 20);
      this.textBoxSize.TabIndex = 2;
      this.textBoxSize.KeyPress += new KeyPressEventHandler(this.keypress);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(188, 81);
      this.label2.Name = "label2";
      this.label2.Size = new Size(53, 14);
      this.label2.TabIndex = 13;
      this.label2.Text = "Field Size";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(9, 81);
      this.label5.Name = "label5";
      this.label5.Size = new Size(55, 14);
      this.label5.TabIndex = 16;
      this.label5.Text = "Field Type";
      this.textBoxType.Location = new Point(91, 78);
      this.textBoxType.MaxLength = 50;
      this.textBoxType.Name = "textBoxType";
      this.textBoxType.ReadOnly = true;
      this.textBoxType.Size = new Size(92, 20);
      this.textBoxType.TabIndex = 17;
      this.textBoxType.TabStop = false;
      this.panel1.Controls.Add((Control) this.cboComortgagor);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Controls.Add((Control) this.label6);
      this.panel1.Controls.Add((Control) this.textBoxID);
      this.panel1.Controls.Add((Control) this.textBoxDescription);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.textBoxType);
      this.panel1.Controls.Add((Control) this.textBoxSize);
      this.panel1.Controls.Add((Control) this.label5);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(342, 103);
      this.panel1.TabIndex = 21;
      this.panel3.Controls.Add((Control) this.label4);
      this.panel3.Controls.Add((Control) this.textBoxTableName);
      this.panel3.Dock = DockStyle.Top;
      this.panel3.Location = new Point(0, 103);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(342, 32);
      this.panel3.TabIndex = 23;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(9, 6);
      this.label4.Name = "label4";
      this.label4.Size = new Size(62, 14);
      this.label4.TabIndex = 17;
      this.label4.Text = "Table Name";
      this.textBoxTableName.Location = new Point(91, 2);
      this.textBoxTableName.MaxLength = 50;
      this.textBoxTableName.Name = "textBoxTableName";
      this.textBoxTableName.ReadOnly = true;
      this.textBoxTableName.Size = new Size(240, 20);
      this.textBoxTableName.TabIndex = 16;
      this.textBoxTableName.TabStop = false;
      this.cboComortgagor.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboComortgagor.FormattingEnabled = true;
      this.cboComortgagor.Items.AddRange(new object[6]
      {
        (object) "1st",
        (object) "2nd",
        (object) "3rd",
        (object) "4th",
        (object) "5th",
        (object) "6th"
      });
      this.cboComortgagor.Location = new Point(91, 54);
      this.cboComortgagor.Name = "cboComortgagor";
      this.cboComortgagor.Size = new Size(92, 22);
      this.cboComortgagor.TabIndex = 1;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(9, 58);
      this.label6.Name = "label6";
      this.label6.Size = new Size(75, 14);
      this.label6.TabIndex = 20;
      this.label6.Text = "Borrower Pair";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(342, 171);
      this.Controls.Add((Control) this.panel3);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoanXDBFieldDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Database Field";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      int num1 = Utils.ParseInt((object) this.textBoxSize.Text);
      if (num1 > 8000 && this.dbField.FieldType == LoanXDBTableList.TableTypes.IsString)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The Field Size must not exceed 8,000.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxSize.Focus();
      }
      else if (num1 < 1 && this.dbField.FieldType == LoanXDBTableList.TableTypes.IsString)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The field size must be from 1 to 8000.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.textBoxDescription.Text.Trim() == "")
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "Please enter a field description", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textBoxDescription.Focus();
      }
      else
      {
        this.dbField.Description = this.textBoxDescription.Text.Trim();
        this.dbField.FieldSize = num1;
        FieldDefinition field = EncompassFields.GetField(this.dbField.FieldID);
        this.dbField.ComortgagorPair = field == null || field.Category == FieldCategory.Common ? 1 : this.cboComortgagor.SelectedIndex + 1;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar))
        e.Handled = false;
      else
        e.Handled = true;
    }
  }
}
