// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.CopySafeHarborForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class CopySafeHarborForm : Form
  {
    private IHtmlInput inputData;
    private IContainer components;
    private Label label1;
    private ComboBox cboOptions;
    private Label label2;
    private Button btnOK;
    private Button btnCancel;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;

    public CopySafeHarborForm(IHtmlInput inputData)
    {
      this.inputData = inputData;
      this.InitializeComponent();
      this.cboOptions.SelectedIndex = 0;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      LoanData inputData = (LoanData) this.inputData;
      switch (this.cboOptions.Text)
      {
        case "Loan Option 1":
          inputData.Calculator.CopyLoanToSafeHarbor(1);
          break;
        case "Loan Option 2":
          inputData.Calculator.CopyLoanToSafeHarbor(2);
          break;
        case "Loan Option 3":
          inputData.Calculator.CopyLoanToSafeHarbor(3);
          break;
        case "Loan Option 4":
          inputData.Calculator.CopyLoanToSafeHarbor(4);
          break;
      }
      Cursor.Current = Cursors.Default;
      this.DialogResult = DialogResult.OK;
    }

    private void cboOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.btnOK.Enabled = this.cboOptions.Text != string.Empty;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.cboOptions = new ComboBox();
      this.label2 = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.label3 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(247, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select a loan option you want to copy loan data to:";
      this.cboOptions.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboOptions.FormattingEnabled = true;
      this.cboOptions.Items.AddRange(new object[4]
      {
        (object) "Loan Option 1",
        (object) "Loan Option 2",
        (object) "Loan Option 3",
        (object) "Loan Option 4"
      });
      this.cboOptions.Location = new Point(15, 35);
      this.cboOptions.Name = "cboOptions";
      this.cboOptions.Size = new Size(160, 21);
      this.cboOptions.TabIndex = 1;
      this.cboOptions.SelectedIndexChanged += new EventHandler(this.cboOptions_SelectedIndexChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 69);
      this.label2.Name = "label2";
      this.label2.Size = new Size(307, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "If you copy loan data, the following field data will be overwritten:";
      this.btnOK.Location = new Point(332, 224);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(413, 224);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 92);
      this.label3.Name = "label3";
      this.label3.Size = new Size(58, 13);
      this.label3.TabIndex = 5;
      this.label3.Text = "Loan Type";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 112);
      this.label4.Name = "label4";
      this.label4.Size = new Size(58, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Loan Term";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 132);
      this.label5.Name = "label5";
      this.label5.Size = new Size(68, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Interest Rate";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 152);
      this.label6.Name = "label6";
      this.label6.Size = new Size(156, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Initial Fixed Interest Rate Period";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 172);
      this.label7.Name = "label7";
      this.label7.Size = new Size((int) sbyte.MaxValue, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Origination Points or Fees";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 192);
      this.label8.Name = "label8";
      this.label8.Size = new Size(81, 13);
      this.label8.TabIndex = 10;
      this.label8.Text = "Discount Points or Fees";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(500, 259);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.cboOptions);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CopySafeHarborForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Copy from Loan Data";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
