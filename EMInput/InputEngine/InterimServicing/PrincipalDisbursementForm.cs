// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InterimServicing.PrincipalDisbursementForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine.InterimServicing
{
  public class PrincipalDisbursementForm : Form
  {
    private DateTime createdOn;
    private GroupContainer groupContainer1;
    private DatePicker boxDate;
    private Label label1;
    protected EMHelpLink emHelpLink1;
    private TextBox boxAmount;
    private Button btnOK;
    private Label labelModifiedBy;
    private Button btnCancel;
    private Label labelCreatedBy;
    private Label label7;
    private Label label21;
    private Label label6;
    private Label label4;
    private TextBox boxComments;
    private TextBox boxInstitution;
    private LoanData loan;
    private PrincipalDisbursementLog disbursementLog;

    public PrincipalDisbursementForm(
      PrincipalDisbursementLog disbursementLog,
      int transNo,
      LoanData loan,
      bool viewOnly)
    {
      this.loan = loan;
      this.disbursementLog = disbursementLog;
      this.InitializeComponent();
      this.initForm();
      this.groupContainer1.Text = "T" + transNo.ToString("00") + " Principal Disbursement";
      DateTime dateTime;
      if (this.disbursementLog != null)
      {
        Label labelCreatedBy = this.labelCreatedBy;
        string createdByName = this.disbursementLog.CreatedByName;
        dateTime = this.disbursementLog.CreatedDateTime;
        string str1 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        string str2 = "Created by " + createdByName + " on " + str1;
        labelCreatedBy.Text = str2;
      }
      else
      {
        this.createdOn = DateTime.Now;
        this.labelCreatedBy.Text = "Created by " + Session.UserInfo.FullName + " on " + this.createdOn.ToString("MM/dd/yyyy hh:mm tt");
      }
      if (this.disbursementLog != null && this.disbursementLog.ModifiedByName != "")
      {
        Label labelModifiedBy = this.labelModifiedBy;
        string modifiedByName = this.disbursementLog.ModifiedByName;
        dateTime = this.disbursementLog.ModifiedDateTime;
        string str3 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        string str4 = "Last modified by " + modifiedByName + " on " + str3;
        labelModifiedBy.Text = str4;
      }
      else
        this.labelModifiedBy.Text = "";
      if (!viewOnly)
        return;
      this.btnOK.Visible = false;
      this.btnCancel.Text = "OK";
      this.boxDate.ReadOnly = true;
      this.boxAmount.ReadOnly = true;
      this.boxInstitution.ReadOnly = true;
      this.boxComments.ReadOnly = true;
      this.Text = "View Transaction";
      this.boxDate.BackColor = Color.WhiteSmoke;
      this.boxAmount.BackColor = Color.WhiteSmoke;
      this.boxInstitution.BackColor = Color.WhiteSmoke;
      this.boxComments.BackColor = Color.WhiteSmoke;
    }

    public PrincipalDisbursementLog DisbursementLog => this.disbursementLog;

    private void InitializeComponent()
    {
      this.groupContainer1 = new GroupContainer();
      this.boxDate = new DatePicker();
      this.label1 = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.boxAmount = new TextBox();
      this.btnOK = new Button();
      this.labelModifiedBy = new Label();
      this.btnCancel = new Button();
      this.labelCreatedBy = new Label();
      this.label7 = new Label();
      this.label21 = new Label();
      this.label6 = new Label();
      this.label4 = new Label();
      this.boxComments = new TextBox();
      this.boxInstitution = new TextBox();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.boxDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.emHelpLink1);
      this.groupContainer1.Controls.Add((Control) this.boxAmount);
      this.groupContainer1.Controls.Add((Control) this.btnOK);
      this.groupContainer1.Controls.Add((Control) this.labelModifiedBy);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label21);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.boxComments);
      this.groupContainer1.Controls.Add((Control) this.boxInstitution);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(396, 362);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "T01 Principal Disbursement";
      this.boxDate.BackColor = SystemColors.Window;
      this.boxDate.Location = new Point(162, 59);
      this.boxDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.boxDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.boxDate.Name = "boxDate";
      this.boxDate.Size = new Size(100, 21);
      this.boxDate.TabIndex = 2;
      this.boxDate.ToolTip = "";
      this.boxDate.Value = new DateTime(0L);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(146, 87);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 61;
      this.label1.Text = "$";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Interum Escrow Disbursement";
      this.emHelpLink1.Location = new Point(13, 330);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 112;
      this.boxAmount.Location = new Point(162, 83);
      this.boxAmount.Name = "boxAmount";
      this.boxAmount.Size = new Size(100, 20);
      this.boxAmount.TabIndex = 3;
      this.boxAmount.TextAlign = HorizontalAlignment.Right;
      this.boxAmount.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.boxAmount.Leave += new EventHandler(this.decimal_FieldLeave);
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Location = new Point(230, 241);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "&Save";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.labelModifiedBy.AutoSize = true;
      this.labelModifiedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelModifiedBy.ForeColor = SystemColors.ControlText;
      this.labelModifiedBy.Location = new Point(10, 303);
      this.labelModifiedBy.Name = "labelModifiedBy";
      this.labelModifiedBy.Size = new Size(65, 13);
      this.labelModifiedBy.TabIndex = 79;
      this.labelModifiedBy.Text = "(ModifiedBy)";
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(310, 241);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(10, 275);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 78;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 105);
      this.label7.Name = "label7";
      this.label7.Size = new Size(56, 13);
      this.label7.TabIndex = 63;
      this.label7.Text = "Comments";
      this.label21.AutoSize = true;
      this.label21.Location = new Point(10, 39);
      this.label21.Name = "label21";
      this.label21.Size = new Size(83, 13);
      this.label21.TabIndex = 66;
      this.label21.Text = "Institution Name";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 83);
      this.label6.Name = "label6";
      this.label6.Size = new Size(110, 13);
      this.label6.TabIndex = 55;
      this.label6.Text = "Disbursement Amount";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 61);
      this.label4.Name = "label4";
      this.label4.Size = new Size(97, 13);
      this.label4.TabIndex = 53;
      this.label4.Text = "Disbursement Date";
      this.boxComments.Location = new Point(10, (int) sbyte.MaxValue);
      this.boxComments.Multiline = true;
      this.boxComments.Name = "boxComments";
      this.boxComments.Size = new Size(374, 108);
      this.boxComments.TabIndex = 4;
      this.boxInstitution.Location = new Point(162, 36);
      this.boxInstitution.Name = "boxInstitution";
      this.boxInstitution.Size = new Size(209, 20);
      this.boxInstitution.TabIndex = 1;
      this.boxInstitution.Tag = (object) "InstitutionName";
      this.ClientSize = new Size(396, 362);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (PrincipalDisbursementForm);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.boxDate.Text.Trim() != string.Empty && !Utils.IsDate((object) this.boxDate.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Disbursement Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxDate.Focus();
      }
      else if (this.boxAmount.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Disbursement Amount cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxAmount.Focus();
      }
      else
      {
        if (this.disbursementLog == null)
        {
          this.disbursementLog = new PrincipalDisbursementLog();
          this.disbursementLog.CreatedByID = Session.UserInfo.Userid;
          this.disbursementLog.CreatedByName = Session.UserInfo.FullName;
          this.disbursementLog.CreatedDateTime = this.createdOn;
        }
        else
        {
          this.disbursementLog.ModifiedByID = Session.UserInfo.Userid;
          this.disbursementLog.ModifiedByName = Session.UserInfo.FullName;
          this.disbursementLog.ModifiedDateTime = DateTime.Now;
        }
        if (this.boxDate.Text.Trim() != string.Empty)
          this.disbursementLog.TransactionDate = Utils.ParseDate((object) this.boxDate.Text.Trim());
        else
          this.disbursementLog.TransactionDate = DateTime.MinValue;
        this.disbursementLog.InstitutionName = this.boxInstitution.Text.Trim();
        this.disbursementLog.TransactionType = ServicingTransactionTypes.PrincipalDisbursement;
        this.disbursementLog.TransactionAmount = Utils.ParseDouble((object) this.boxAmount.Text.Trim());
        this.disbursementLog.Comments = this.boxComments.Text;
        this.disbursementLog.ModifiedByID = Session.UserInfo.Userid;
        this.disbursementLog.ModifiedByName = Session.UserInfo.FullName;
        this.disbursementLog.ModifiedDateTime = DateTime.Now;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void decimal_KeyUp(object sender, KeyEventArgs e)
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

    private void decimal_FieldLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      double num = Utils.ParseDouble((object) textBox.Text);
      textBox.Text = num.ToString("N2");
    }

    public void initForm()
    {
      if (this.disbursementLog == null)
        return;
      if (this.disbursementLog.TransactionDate != DateTime.MinValue)
        this.boxDate.Text = this.disbursementLog.TransactionDate.ToString("MM/dd/yyyy");
      else
        this.boxDate.Text = "";
      if (this.disbursementLog.TransactionAmount != 0.0)
        this.boxAmount.Text = this.disbursementLog.TransactionAmount.ToString("N2");
      else
        this.boxAmount.Text = "";
      this.boxInstitution.Text = this.disbursementLog.InstitutionName;
      this.boxComments.Text = this.disbursementLog.Comments;
    }
  }
}
