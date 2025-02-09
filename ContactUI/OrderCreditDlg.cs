// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.OrderCreditDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class OrderCreditDlg : Form
  {
    private int borrowerId = -1;
    private BorrowerInfo coborrower;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label12;
    private TextBox txtBoxBorrower;
    private TextBox txtBoxCoBorrower;
    private Button btnAddCoBorrower;
    private Button btnContinue;
    private Button btnCancel;
    private Label label6;
    private Label label7;
    private Label lblLoanFolder;
    private System.ComponentModel.Container components;

    public OrderCreditDlg(int borrowerId, string borrowerName)
    {
      this.InitializeComponent();
      this.borrowerId = borrowerId;
      this.lblLoanFolder.Text = this.lblLoanFolder.Text.Replace("<folder name>", Session.WorkingFolder);
      this.txtBoxBorrower.Text = borrowerName;
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
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.txtBoxBorrower = new TextBox();
      this.txtBoxCoBorrower = new TextBox();
      this.btnAddCoBorrower = new Button();
      this.label12 = new Label();
      this.btnContinue = new Button();
      this.btnCancel = new Button();
      this.lblLoanFolder = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Borrower Contact";
      this.label2.Location = new Point(12, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(336, 20);
      this.label2.TabIndex = 1;
      this.label2.Text = "A credit report will be ordered for the following borrower contact(s)";
      this.label3.Location = new Point(12, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(72, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "Borrower:";
      this.label4.Location = new Point(12, 84);
      this.label4.Name = "label4";
      this.label4.Size = new Size(72, 16);
      this.label4.TabIndex = 3;
      this.label4.Text = "Co-Borrower:";
      this.txtBoxBorrower.Enabled = false;
      this.txtBoxBorrower.Location = new Point(84, 56);
      this.txtBoxBorrower.Name = "txtBoxBorrower";
      this.txtBoxBorrower.Size = new Size(216, 20);
      this.txtBoxBorrower.TabIndex = 4;
      this.txtBoxBorrower.Text = "";
      this.txtBoxCoBorrower.Enabled = false;
      this.txtBoxCoBorrower.Location = new Point(84, 84);
      this.txtBoxCoBorrower.Name = "txtBoxCoBorrower";
      this.txtBoxCoBorrower.Size = new Size(216, 20);
      this.txtBoxCoBorrower.TabIndex = 5;
      this.txtBoxCoBorrower.Text = "";
      this.btnAddCoBorrower.Location = new Point(308, 84);
      this.btnAddCoBorrower.Name = "btnAddCoBorrower";
      this.btnAddCoBorrower.Size = new Size(100, 22);
      this.btnAddCoBorrower.TabIndex = 6;
      this.btnAddCoBorrower.Text = "Add Co-Borrower";
      this.btnAddCoBorrower.Click += new EventHandler(this.btnAddCoBorrower_Click);
      this.label12.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.label12.BorderStyle = BorderStyle.Fixed3D;
      this.label12.Location = new Point(8, 164);
      this.label12.Name = "label12";
      this.label12.Size = new Size(396, 2);
      this.label12.TabIndex = 16;
      this.btnContinue.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnContinue.DialogResult = DialogResult.OK;
      this.btnContinue.Location = new Point(264, 180);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(68, 22);
      this.btnContinue.TabIndex = 19;
      this.btnContinue.Text = "Continue";
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(340, 180);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(68, 22);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.lblLoanFolder.Location = new Point(48, 120);
      this.lblLoanFolder.Name = "lblLoanFolder";
      this.lblLoanFolder.Size = new Size(356, 16);
      this.lblLoanFolder.TabIndex = 21;
      this.lblLoanFolder.Text = "A prequal file will be created in the <folder name> folder.";
      this.label6.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(12, 120);
      this.label6.Name = "label6";
      this.label6.Size = new Size(37, 16);
      this.label6.TabIndex = 22;
      this.label6.Text = "Note:";
      this.label7.Location = new Point(48, 136);
      this.label7.Name = "label7";
      this.label7.Size = new Size(356, 16);
      this.label7.TabIndex = 23;
      this.label7.Text = "You can access the credit report in this prequal file.";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(418, 215);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.lblLoanFolder);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.btnAddCoBorrower);
      this.Controls.Add((Control) this.txtBoxCoBorrower);
      this.Controls.Add((Control) this.txtBoxBorrower);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (OrderCreditDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Order Credit";
      this.ResumeLayout(false);
    }

    public BorrowerInfo CoBorrower => this.coborrower;

    private void btnAddCoBorrower_Click(object sender, EventArgs e)
    {
      BorrowerContactPickList borrowerContactPickList = new BorrowerContactPickList(this.borrowerId, string.Empty, true);
      if (DialogResult.Cancel == borrowerContactPickList.ShowDialog())
        return;
      BorrowerInfo selectedBorrowerContact = borrowerContactPickList.GetSelectedBorrowerContact();
      if (selectedBorrowerContact == null)
        return;
      this.coborrower = selectedBorrowerContact;
      this.txtBoxCoBorrower.Text = this.coborrower.FirstName + " " + this.coborrower.LastName;
    }
  }
}
