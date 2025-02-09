// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.RxBorrowerSSN
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class RxBorrowerSSN : Form
  {
    private bool isCoborrower;
    private BorrowerInfo borObj;
    private bool loanDataOnly;
    private string currentUIValue = "NULL";
    private bool forceOptoutLogic;
    private FeaturesAclManager aclMgr;
    private IContainer components;
    private Label label1;
    private RadioButton rdoLoan;
    private RadioButton rdoContactManager;
    private Button btnOK;
    private Button btnCancel;
    private Label lblLoan;
    private Label lblBorMgr;
    private FlowLayoutPanel flowLayoutPanel1;

    public RxBorrowerSSN(
      bool isCoborrower,
      BorrowerInfo borObj,
      bool loanDataOnly,
      string currentUIValue)
      : this(isCoborrower, borObj, loanDataOnly, currentUIValue, false)
    {
    }

    public RxBorrowerSSN(bool isCoborrower, BorrowerInfo borObj, bool loanDataOnly)
      : this(isCoborrower, borObj, loanDataOnly, false)
    {
    }

    public RxBorrowerSSN(
      bool isCoborrower,
      BorrowerInfo borObj,
      bool loanDataOnly,
      bool forceOptOutLogic)
    {
      this.InitializeComponent();
      this.isCoborrower = isCoborrower;
      this.borObj = borObj;
      this.loanDataOnly = loanDataOnly;
      this.forceOptoutLogic = forceOptOutLogic;
      this.initPage();
      this.DialogResult = DialogResult.Cancel;
    }

    public RxBorrowerSSN(
      bool isCoborrower,
      BorrowerInfo borObj,
      bool loanDataOnly,
      string currentUIValue,
      bool forceOptOutLogic)
    {
      this.InitializeComponent();
      this.isCoborrower = isCoborrower;
      this.borObj = borObj;
      this.loanDataOnly = loanDataOnly;
      this.currentUIValue = currentUIValue;
      this.forceOptoutLogic = forceOptOutLogic;
      this.initPage();
      this.DialogResult = DialogResult.Cancel;
    }

    private void initPage()
    {
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      if (this.forceOptoutLogic)
      {
        if (this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Contacts_Update))
        {
          this.btnCancel.Visible = true;
        }
        else
        {
          this.btnCancel.Visible = false;
          this.ControlBox = false;
        }
      }
      if (this.currentUIValue != "NULL")
      {
        this.lblLoan.Text = this.currentUIValue;
        this.lblLoan.Tag = (object) this.currentUIValue;
      }
      else if (this.isCoborrower)
      {
        this.lblLoan.Text = Session.LoanData.GetField("97");
        this.lblLoan.Tag = (object) Session.LoanData.GetField("97");
      }
      else
      {
        this.lblLoan.Text = Session.LoanData.GetField("65");
        this.lblLoan.Tag = (object) Session.LoanData.GetField("65");
      }
      this.lblBorMgr.Text = this.borObj.SSN;
      this.enforceSecurity(this.borObj.OwnerID);
      if (this.currentUIValue != "")
        this.label1.Text = "The Social Security Number in the loan file does not match the Social Security Number in the linked borrower contact.  To synchronize the values, select which Social Security Number to match the files to (Loan or Borrower Contact), and then click OK.";
      if (this.loanDataOnly || this.currentUIValue != "")
        this.btnCancel.Text = "Cancel";
      if (this.lblLoan.Text.Trim() == "")
        this.lblLoan.Text = "Empty";
      if (!(this.lblBorMgr.Text.Trim() == ""))
        return;
      this.lblBorMgr.Text = "Empty";
    }

    private void enforceSecurity(string ownerID)
    {
      if (UserInfo.IsSuperAdministrator(Session.UserID, Session.UserInfo.UserPersonas) || ownerID == Session.UserID || Session.AclGroupManager.GetBorrowerContactAccessRight(Session.UserInfo, ownerID) == AclResourceAccess.ReadWrite)
      {
        this.rdoLoan.Enabled = true;
      }
      else
      {
        this.rdoLoan.Enabled = false;
        this.rdoLoan.Checked = false;
      }
      if (this.loanDataOnly && this.rdoLoan.Enabled)
      {
        this.rdoLoan.Enabled = false;
        this.rdoLoan.Checked = true;
        this.rdoContactManager.Enabled = false;
      }
      else
      {
        if (!this.loanDataOnly)
          return;
        this.rdoContactManager.Enabled = false;
      }
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (!this.rdoLoan.Checked && !this.rdoContactManager.Checked)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select an option.");
      }
      else
      {
        if (this.rdoLoan.Checked)
        {
          this.borObj.SSN = string.Concat(this.lblLoan.Tag);
          Session.ContactManager.UpdateBorrower(this.borObj);
        }
        else if (this.isCoborrower)
          Session.LoanData.SetField("97", this.borObj.SSN);
        else
          Session.LoanData.SetField("65", this.borObj.SSN);
        this.DialogResult = DialogResult.OK;
      }
    }

    public BorrowerInfo BorrowerObj => this.borObj;

    public bool HasConflict => this.borObj.SSN != string.Concat(this.lblLoan.Tag);

    private void RxBorrowerSSN_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult == DialogResult.OK || this.btnCancel.Visible)
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RxBorrowerSSN));
      this.label1 = new Label();
      this.rdoLoan = new RadioButton();
      this.rdoContactManager = new RadioButton();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblLoan = new Label();
      this.lblBorMgr = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(423, 47);
      this.label1.TabIndex = 0;
      this.label1.Text = componentResourceManager.GetString("label1.Text");
      this.rdoLoan.AutoSize = true;
      this.rdoLoan.Checked = true;
      this.rdoLoan.Location = new Point(13, 76);
      this.rdoLoan.Name = "rdoLoan";
      this.rdoLoan.Size = new Size(52, 17);
      this.rdoLoan.TabIndex = 1;
      this.rdoLoan.TabStop = true;
      this.rdoLoan.Text = "Loan:";
      this.rdoLoan.UseVisualStyleBackColor = true;
      this.rdoContactManager.AutoSize = true;
      this.rdoContactManager.Location = new Point(174, 76);
      this.rdoContactManager.Name = "rdoContactManager";
      this.rdoContactManager.Size = new Size(110, 17);
      this.rdoContactManager.TabIndex = 2;
      this.rdoContactManager.TabStop = true;
      this.rdoContactManager.Text = "Borrower Contact:";
      this.rdoContactManager.UseVisualStyleBackColor = true;
      this.btnOK.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOK.Location = new Point(12, 0);
      this.btnOK.Margin = new Padding(0);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "Select";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(92, 0);
      this.btnCancel.Margin = new Padding(5, 0, 0, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(120, 23);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel, Do Not Link";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblLoan.AutoSize = true;
      this.lblLoan.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLoan.Location = new Point(71, 78);
      this.lblLoan.Name = "lblLoan";
      this.lblLoan.Size = new Size(56, 13);
      this.lblLoan.TabIndex = 5;
      this.lblLoan.Text = "loanSSN";
      this.lblBorMgr.AutoSize = true;
      this.lblBorMgr.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblBorMgr.Location = new Point(295, 78);
      this.lblBorMgr.Name = "lblBorMgr";
      this.lblBorMgr.Size = new Size(50, 13);
      this.lblBorMgr.TabIndex = 6;
      this.lblBorMgr.Text = "borSSN";
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnOK);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(212, 106);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(212, 22);
      this.flowLayoutPanel1.TabIndex = 7;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(448, 140);
      this.Controls.Add((Control) this.flowLayoutPanel1);
      this.Controls.Add((Control) this.lblBorMgr);
      this.Controls.Add((Control) this.lblLoan);
      this.Controls.Add((Control) this.rdoContactManager);
      this.Controls.Add((Control) this.rdoLoan);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RxBorrowerSSN);
      this.Text = " SSN Match";
      this.FormClosing += new FormClosingEventHandler(this.RxBorrowerSSN_FormClosing);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
