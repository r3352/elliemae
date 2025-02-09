// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LockedLoanDialog
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LockedLoanDialog : Form
  {
    private IContainer components;
    private Label lblHeader;
    private PictureBox pictureBox1;
    private Label label2;
    private Label lblN;
    private Label lblE;
    private Label lblP;
    private Label lblName;
    private Label lblEmail;
    private Label lblPhone;
    private Label label5;
    private Button btnCancel;
    private Button btnSkip;
    private Button btnRetry;
    private Button btnSkipAll;
    private ListView listViewUsers;
    private ColumnHeader colHdrName;
    private ColumnHeader colHdrEmail;
    private ColumnHeader colHdrPhone;

    public LockedLoanDialog(LoanDataMgr loanMgr, LockException ex, LockInfo[] lockInfoList)
    {
      this.InitializeComponent();
      this.lblHeader.Text = this.lblHeader.Text.Replace("%BORROWER%", Utils.JoinName(loanMgr.LoanData.GetField("36"), loanMgr.LoanData.GetField("37")));
      if (lockInfoList != null && lockInfoList.Length != 0)
        this.initCE(loanMgr, lockInfoList);
      else
        this.init(loanMgr, ex);
    }

    private void init(LoanDataMgr loanMgr, LockException ex)
    {
      this.listViewUsers.Visible = false;
      this.lblName.Text = this.lblEmail.Text = this.lblPhone.Text = "(Not Available)";
      UserInfo user = loanMgr.SessionObjects.OrganizationManager.GetUser(ex.LockInfo.LockedBy);
      if (!(user != (UserInfo) null))
        return;
      if (user.FullName != "")
        this.lblName.Text = user.FullName;
      if (user.Email != "")
        this.lblEmail.Text = user.Email;
      if (!(user.Phone != ""))
        return;
      this.lblPhone.Text = user.Phone;
    }

    private void initCE(LoanDataMgr loanMgr, LockInfo[] lockInfoList)
    {
      this.lblN.Visible = this.lblName.Visible = false;
      this.lblE.Visible = this.lblEmail.Visible = false;
      this.lblP.Visible = this.lblPhone.Visible = false;
      this.listViewUsers.Items.Clear();
      foreach (LockInfo lockInfo in lockInfoList)
      {
        this.lblName.Text = this.lblEmail.Text = this.lblPhone.Text = "(Not Available)";
        UserInfo user = loanMgr.SessionObjects.OrganizationManager.GetUser(lockInfo.LockedBy);
        if (user != (UserInfo) null)
        {
          if (user.FullName != "")
            this.lblName.Text = user.FullName;
          if (user.Email != "")
            this.lblEmail.Text = user.Email;
          if (user.Phone != "")
            this.lblPhone.Text = user.Phone;
        }
        this.listViewUsers.Items.Add(new ListViewItem(new string[3]
        {
          this.lblName.Text,
          this.lblEmail.Text,
          this.lblPhone.Text
        }));
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LockedLoanDialog));
      this.lblHeader = new Label();
      this.pictureBox1 = new PictureBox();
      this.label2 = new Label();
      this.lblN = new Label();
      this.lblE = new Label();
      this.lblP = new Label();
      this.lblName = new Label();
      this.lblEmail = new Label();
      this.lblPhone = new Label();
      this.label5 = new Label();
      this.btnCancel = new Button();
      this.btnSkip = new Button();
      this.btnRetry = new Button();
      this.btnSkipAll = new Button();
      this.listViewUsers = new ListView();
      this.colHdrName = new ColumnHeader();
      this.colHdrPhone = new ColumnHeader();
      this.colHdrEmail = new ColumnHeader();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.lblHeader.Location = new Point(58, 12);
      this.lblHeader.Name = "lblHeader";
      this.lblHeader.Size = new Size(300, 34);
      this.lblHeader.TabIndex = 0;
      this.lblHeader.Text = "The loan '%BORROWER%' is currently locked by the following user(s):";
      this.pictureBox1.Image = (Image) componentResourceManager.GetObject("pictureBox1.Image");
      this.pictureBox1.Location = new Point(10, 10);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(31, 32);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(58, 50);
      this.label2.Name = "label2";
      this.label2.Size = new Size(0, 14);
      this.label2.TabIndex = 2;
      this.lblN.AutoSize = true;
      this.lblN.Location = new Point(102, 72);
      this.lblN.Name = "lblN";
      this.lblN.Size = new Size(37, 14);
      this.lblN.TabIndex = 3;
      this.lblN.Text = "Name:";
      this.lblE.AutoSize = true;
      this.lblE.Location = new Point(102, 91);
      this.lblE.Name = "lblE";
      this.lblE.Size = new Size(34, 14);
      this.lblE.TabIndex = 4;
      this.lblE.Text = "Email:";
      this.lblP.AutoSize = true;
      this.lblP.Location = new Point(102, 111);
      this.lblP.Name = "lblP";
      this.lblP.Size = new Size(40, 14);
      this.lblP.TabIndex = 5;
      this.lblP.Text = "Phone:";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(156, 72);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(116, 14);
      this.lblName.TabIndex = 6;
      this.lblName.Text = "(User First/Last Name)";
      this.lblEmail.AutoSize = true;
      this.lblEmail.Location = new Point(156, 91);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(110, 14);
      this.lblEmail.TabIndex = 7;
      this.lblEmail.Text = "(User Email Address)";
      this.lblPhone.AutoSize = true;
      this.lblPhone.Location = new Point(156, 111);
      this.lblPhone.Name = "lblPhone";
      this.lblPhone.Size = new Size(80, 14);
      this.lblPhone.TabIndex = 8;
      this.lblPhone.Text = "(User Phone #)";
      this.label5.Location = new Point(58, 148);
      this.label5.Name = "label5";
      this.label5.Size = new Size(302, 17);
      this.label5.TabIndex = 9;
      this.label5.Text = "To update this loan, it must not be in use by another user.";
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(271, 173);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 10;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSkip.DialogResult = DialogResult.Ignore;
      this.btnSkip.Location = new Point(25, 173);
      this.btnSkip.Name = "btnSkip";
      this.btnSkip.Size = new Size(75, 22);
      this.btnSkip.TabIndex = 11;
      this.btnSkip.Text = "&Skip";
      this.btnSkip.UseVisualStyleBackColor = true;
      this.btnRetry.DialogResult = DialogResult.Retry;
      this.btnRetry.Location = new Point(189, 173);
      this.btnRetry.Name = "btnRetry";
      this.btnRetry.Size = new Size(75, 22);
      this.btnRetry.TabIndex = 12;
      this.btnRetry.Text = "&Retry";
      this.btnRetry.UseVisualStyleBackColor = true;
      this.btnSkipAll.DialogResult = DialogResult.Abort;
      this.btnSkipAll.Location = new Point(107, 173);
      this.btnSkipAll.Name = "btnSkipAll";
      this.btnSkipAll.Size = new Size(75, 22);
      this.btnSkipAll.TabIndex = 13;
      this.btnSkipAll.Text = "Skip &All";
      this.btnSkipAll.UseVisualStyleBackColor = true;
      this.listViewUsers.Columns.AddRange(new ColumnHeader[3]
      {
        this.colHdrName,
        this.colHdrEmail,
        this.colHdrPhone
      });
      this.listViewUsers.Location = new Point(10, 50);
      this.listViewUsers.Name = "listViewUsers";
      this.listViewUsers.Size = new Size(358, 95);
      this.listViewUsers.TabIndex = 14;
      this.listViewUsers.UseCompatibleStateImageBehavior = false;
      this.listViewUsers.View = View.Details;
      this.colHdrName.Text = "Name";
      this.colHdrName.Width = 89;
      this.colHdrPhone.Text = "Phone";
      this.colHdrPhone.Width = 115;
      this.colHdrEmail.Text = "Email";
      this.colHdrEmail.Width = 142;
      this.AcceptButton = (IButtonControl) this.btnSkip;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(371, 211);
      this.Controls.Add((Control) this.btnSkipAll);
      this.Controls.Add((Control) this.btnRetry);
      this.Controls.Add((Control) this.btnSkip);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.lblPhone);
      this.Controls.Add((Control) this.lblEmail);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.lblP);
      this.Controls.Add((Control) this.lblE);
      this.Controls.Add((Control) this.lblN);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.pictureBox1);
      this.Controls.Add((Control) this.lblHeader);
      this.Controls.Add((Control) this.listViewUsers);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LockedLoanDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Trade Management - Locked Loan";
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
