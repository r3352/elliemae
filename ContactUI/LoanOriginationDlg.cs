// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.LoanOriginationDlg
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class LoanOriginationDlg : Form
  {
    private string loanFolder = string.Empty;
    private LoanTemplateSelection loanTemplate;
    private int borrowerId = -1;
    private bool allowBlankLoan;
    private bool allowTemplatedLoan;
    private BorrowerInfo coborrower;
    private System.ComponentModel.Container components;
    private Label label1;
    private Label lblContactDescription;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label lblTemplateDescription;
    private Label lblFolderDescription;
    private Label label8;
    private Label label9;
    private Label label10;
    private Label label11;
    private Label label12;
    private Label label13;
    private TextBox txtBoxBorrower;
    private TextBox txtBoxCoBorrower;
    private Button btnAddCoBorrower;
    private TextBox txtBoxLoanTemplate;
    private Button btnSelectTemplate;
    private ComboBox cmbBoxLoanFolder;
    private Button btnContinue;
    private Button btnCancel;

    public LoanOriginationDlg(int borrowerId, string borrowerName, string source)
    {
      this.InitializeComponent();
      this.borrowerId = borrowerId;
      this.txtBoxBorrower.Text = borrowerName;
      this.Text = source;
      this.init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void init()
    {
      this.reloadLoanFolders();
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.allowBlankLoan = aclManager.GetUserApplicationRight(AclFeature.Cnt_Borrower_CreatBlankLoan);
      if (!this.allowBlankLoan)
      {
        this.lblTemplateDescription.Text = "The loan will be originated using the following template";
        this.btnContinue.Enabled = false;
      }
      this.allowTemplatedLoan = aclManager.GetUserApplicationRight(AclFeature.Cnt_Borrower_CreatLoanFrmTemplate);
      if (!this.allowTemplatedLoan)
        this.btnSelectTemplate.Enabled = false;
      if (this.cmbBoxLoanFolder.Items.Count != 0)
        return;
      this.btnContinue.Enabled = false;
    }

    public string LoanFolder => this.loanFolder;

    public LoanTemplateSelection SelectedTemplate => this.loanTemplate;

    private void reloadLoanFolders()
    {
      LoanFolderInfo[] loanFolderInfoArray = LoanFolderUtil.LoanFolderNames2LoanFolderInfos(((LoanFolderRuleManager) Session.BPM.GetBpmManager(BpmCategory.LoanFolder)).GetLoanFoldersForAction(LoanFolderAction.Originate));
      this.cmbBoxLoanFolder.Items.Clear();
      foreach (LoanFolderInfo loanFolderInfo in loanFolderInfoArray)
      {
        if (loanFolderInfo.Type == LoanFolderInfo.LoanFolderType.Regular)
          this.cmbBoxLoanFolder.Items.Add((object) loanFolderInfo);
      }
      if (this.cmbBoxLoanFolder.Items.Count <= 0)
        return;
      for (int index = 0; index < this.cmbBoxLoanFolder.Items.Count; ++index)
      {
        if (((LoanFolderInfo) this.cmbBoxLoanFolder.Items[index]).Name == Session.WorkingFolder)
        {
          this.cmbBoxLoanFolder.SelectedIndex = index;
          return;
        }
      }
      this.cmbBoxLoanFolder.SelectedIndex = 0;
    }

    public BorrowerInfo CoBorrower => this.coborrower;

    private void btnAddCoBorrower_Click(object sender, EventArgs e)
    {
      BorrowerContactPickList borrowerContactPickList = new BorrowerContactPickList(this.borrowerId, string.Empty, false);
      if (DialogResult.Cancel == borrowerContactPickList.ShowDialog())
        return;
      BorrowerInfo selectedBorrowerContact = borrowerContactPickList.GetSelectedBorrowerContact();
      if (selectedBorrowerContact == null)
        return;
      this.coborrower = selectedBorrowerContact;
      this.txtBoxCoBorrower.Text = this.coborrower.FirstName + " " + this.coborrower.LastName;
    }

    private void btnSelectTemplate_Click(object sender, EventArgs e)
    {
      LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(Session.DefaultInstance, false, this.allowBlankLoan, this.allowTemplatedLoan);
      if (templateSelectDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || templateSelectDialog.SelectedItem == null)
        return;
      this.loanTemplate = new LoanTemplateSelection(templateSelectDialog.SelectedItem, templateSelectDialog.AppendData);
      this.txtBoxLoanTemplate.Text = this.loanTemplate.TemplateEntry.Name;
    }

    private void txtBoxLoanTemplate_TextChanged(object sender, EventArgs e)
    {
      if (this.allowBlankLoan)
        return;
      this.btnContinue.Enabled = !(string.Empty == this.txtBoxLoanTemplate.Text);
    }

    private void btnContinue_Click(object sender, EventArgs e)
    {
      if (!this.allowBlankLoan && string.Empty == this.txtBoxLoanTemplate.Text)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a loan template", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.loanFolder = ((LoanFolderInfo) this.cmbBoxLoanFolder.SelectedItem).Name;
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.lblContactDescription = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.txtBoxBorrower = new TextBox();
      this.txtBoxCoBorrower = new TextBox();
      this.btnAddCoBorrower = new Button();
      this.label5 = new Label();
      this.lblTemplateDescription = new Label();
      this.lblFolderDescription = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.txtBoxLoanTemplate = new TextBox();
      this.btnSelectTemplate = new Button();
      this.label12 = new Label();
      this.label13 = new Label();
      this.cmbBoxLoanFolder = new ComboBox();
      this.btnContinue = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(120, 16);
      this.label1.TabIndex = 0;
      this.label1.Text = "Borrower Contact";
      this.lblContactDescription.Location = new Point(12, 32);
      this.lblContactDescription.Name = "lblContactDescription";
      this.lblContactDescription.Size = new Size(336, 20);
      this.lblContactDescription.TabIndex = 1;
      this.lblContactDescription.Text = "The loan will be originated using the following borrower contact(s)";
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
      this.txtBoxBorrower.Size = new Size(172, 20);
      this.txtBoxBorrower.TabIndex = 4;
      this.txtBoxCoBorrower.Enabled = false;
      this.txtBoxCoBorrower.Location = new Point(84, 84);
      this.txtBoxCoBorrower.Name = "txtBoxCoBorrower";
      this.txtBoxCoBorrower.Size = new Size(172, 20);
      this.txtBoxCoBorrower.TabIndex = 5;
      this.btnAddCoBorrower.Location = new Point(264, 84);
      this.btnAddCoBorrower.Name = "btnAddCoBorrower";
      this.btnAddCoBorrower.Size = new Size(100, 22);
      this.btnAddCoBorrower.TabIndex = 6;
      this.btnAddCoBorrower.Text = "Add Co-Borrower";
      this.btnAddCoBorrower.Click += new EventHandler(this.btnAddCoBorrower_Click);
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(12, 128);
      this.label5.Name = "label5";
      this.label5.Size = new Size(96, 16);
      this.label5.TabIndex = 7;
      this.label5.Text = "Loan Template";
      this.lblTemplateDescription.Location = new Point(12, 148);
      this.lblTemplateDescription.Name = "lblTemplateDescription";
      this.lblTemplateDescription.Size = new Size(336, 20);
      this.lblTemplateDescription.TabIndex = 8;
      this.lblTemplateDescription.Text = "A blank loan will be created if you have not selected a template";
      this.lblFolderDescription.Location = new Point(12, 236);
      this.lblFolderDescription.Name = "lblFolderDescription";
      this.lblFolderDescription.Size = new Size(336, 20);
      this.lblFolderDescription.TabIndex = 10;
      this.lblFolderDescription.Text = "The loan will be originated in the following Loan Folder";
      this.label8.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label8.Location = new Point(12, 216);
      this.label8.Name = "label8";
      this.label8.Size = new Size(96, 16);
      this.label8.TabIndex = 9;
      this.label8.Text = "Loan Folder";
      this.label9.BorderStyle = BorderStyle.Fixed3D;
      this.label9.Location = new Point(8, 116);
      this.label9.Name = "label9";
      this.label9.Size = new Size(352, 2);
      this.label9.TabIndex = 11;
      this.label10.BorderStyle = BorderStyle.Fixed3D;
      this.label10.Location = new Point(8, 204);
      this.label10.Name = "label10";
      this.label10.Size = new Size(352, 2);
      this.label10.TabIndex = 12;
      this.label11.Location = new Point(12, 176);
      this.label11.Name = "label11";
      this.label11.Size = new Size(72, 16);
      this.label11.TabIndex = 13;
      this.label11.Text = "Template:";
      this.txtBoxLoanTemplate.Enabled = false;
      this.txtBoxLoanTemplate.Location = new Point(84, 172);
      this.txtBoxLoanTemplate.Name = "txtBoxLoanTemplate";
      this.txtBoxLoanTemplate.Size = new Size(172, 20);
      this.txtBoxLoanTemplate.TabIndex = 14;
      this.txtBoxLoanTemplate.TextChanged += new EventHandler(this.txtBoxLoanTemplate_TextChanged);
      this.btnSelectTemplate.Location = new Point(264, 172);
      this.btnSelectTemplate.Name = "btnSelectTemplate";
      this.btnSelectTemplate.Size = new Size(100, 22);
      this.btnSelectTemplate.TabIndex = 15;
      this.btnSelectTemplate.Text = "Select Template";
      this.btnSelectTemplate.Click += new EventHandler(this.btnSelectTemplate_Click);
      this.label12.BorderStyle = BorderStyle.Fixed3D;
      this.label12.Location = new Point(8, 296);
      this.label12.Name = "label12";
      this.label12.Size = new Size(352, 2);
      this.label12.TabIndex = 16;
      this.label13.Location = new Point(12, 264);
      this.label13.Name = "label13";
      this.label13.Size = new Size(72, 16);
      this.label13.TabIndex = 17;
      this.label13.Text = "Loan Folder:";
      this.cmbBoxLoanFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLoanFolder.Location = new Point(84, 260);
      this.cmbBoxLoanFolder.Name = "cmbBoxLoanFolder";
      this.cmbBoxLoanFolder.Size = new Size(172, 21);
      this.cmbBoxLoanFolder.TabIndex = 18;
      this.btnContinue.Location = new Point(220, 312);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(68, 22);
      this.btnContinue.TabIndex = 19;
      this.btnContinue.Text = "Continue";
      this.btnContinue.Click += new EventHandler(this.btnContinue_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(296, 312);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(68, 22);
      this.btnCancel.TabIndex = 20;
      this.btnCancel.Text = "Cancel";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(374, 347);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.cmbBoxLoanFolder);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.btnSelectTemplate);
      this.Controls.Add((Control) this.txtBoxLoanTemplate);
      this.Controls.Add((Control) this.txtBoxCoBorrower);
      this.Controls.Add((Control) this.txtBoxBorrower);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.lblFolderDescription);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.lblTemplateDescription);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.btnAddCoBorrower);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.lblContactDescription);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (LoanOriginationDlg);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
