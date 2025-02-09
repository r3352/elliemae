// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CoverageDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CoverageDialog : Form
  {
    private Label labelDesc;
    private Button buttonOK;
    private Button buttonCancel;
    private TextBox textBoxFee;
    private System.ComponentModel.Container components;
    private LoanData loan;
    private bool forLender = true;
    private bool readOnly;
    private Label label1;
    private IHtmlInput ccTemp;

    public CoverageDialog(IHtmlInput ccTemp, bool forLender, bool readOnly)
    {
      this.loan = (LoanData) null;
      this.forLender = forLender;
      this.readOnly = readOnly;
      this.ccTemp = ccTemp;
      this.InitializeComponent();
      if (!this.forLender)
      {
        this.Text = "Owner's Coverage";
        this.labelDesc.Text = "Owner's Coverage";
        this.textBoxFee.Text = this.getFieldValue("2410");
      }
      else
        this.textBoxFee.Text = this.getFieldValue("2409");
    }

    public CoverageDialog(LoanData loan, bool forLender, bool readOnly)
    {
      this.ccTemp = (IHtmlInput) null;
      this.loan = loan;
      this.forLender = forLender;
      this.readOnly = readOnly;
      this.InitializeComponent();
      if (!this.forLender)
      {
        this.Text = "Owner's Coverage";
        this.labelDesc.Text = "Owner's Coverage";
        this.textBoxFee.Text = this.getFieldValue("2410");
      }
      else
        this.textBoxFee.Text = this.getFieldValue("2409");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.labelDesc = new Label();
      this.textBoxFee = new TextBox();
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.label1 = new Label();
      this.SuspendLayout();
      this.labelDesc.AutoSize = true;
      this.labelDesc.Location = new Point(12, 16);
      this.labelDesc.Name = "labelDesc";
      this.labelDesc.Size = new Size(96, 13);
      this.labelDesc.TabIndex = 0;
      this.labelDesc.Text = "Lender's Coverage";
      this.textBoxFee.Location = new Point(136, 12);
      this.textBoxFee.MaxLength = 15;
      this.textBoxFee.Name = "textBoxFee";
      this.textBoxFee.Size = new Size(100, 20);
      this.textBoxFee.TabIndex = 1;
      this.textBoxFee.TextAlign = HorizontalAlignment.Right;
      this.textBoxFee.Leave += new EventHandler(this.field_Leave);
      this.textBoxFee.KeyUp += new KeyEventHandler(this.num_KeyUp);
      this.textBoxFee.KeyPress += new KeyPressEventHandler(this.num_KeyPress);
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(80, 44);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(75, 23);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "&OK";
      this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(161, 44);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "&Cancel";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(123, 16);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "$";
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(248, 82);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOK);
      this.Controls.Add((Control) this.textBoxFee);
      this.Controls.Add((Control) this.labelDesc);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CoverageDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Lender's Coverage";
      this.KeyPress += new KeyPressEventHandler(this.CoverageDialog_KeyPress);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void num_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void num_KeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void field_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text.Trim());
      textBox.Text = num.ToString("N2");
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      double num = Utils.ParseDouble((object) this.textBoxFee.Text.Trim());
      if (this.forLender)
      {
        this.setFieldValue("2409", num.ToString("N2"));
        if (num != 0.0)
          this.setFieldValue("652", "Lender's coverage: $" + num.ToString("N2"));
        else
          this.setFieldValue("652", "Lender's coverage: $0.00");
      }
      else
      {
        this.setFieldValue("2410", num.ToString("N2"));
        if (num != 0.0)
          this.setFieldValue("1633", "Owner's coverage: $" + num.ToString("N2"));
        else
          this.setFieldValue("1633", "Owner's coverage: $0.00");
      }
    }

    private string getFieldValue(string id)
    {
      return this.ccTemp != null ? this.ccTemp.GetField(id) : this.loan.GetField(id);
    }

    private void setFieldValue(string id, string val)
    {
      if (this.ccTemp != null)
        this.ccTemp.SetCurrentField(id, val);
      else
        this.loan.SetCurrentField(id, val);
    }

    private void CoverageDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
