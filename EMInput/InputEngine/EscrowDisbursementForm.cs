// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.EscrowDisbursementForm
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class EscrowDisbursementForm : Form
  {
    private DateTime createdOn;
    private LoanData loan;
    private EscrowDisbursementLog disbursementLog;
    private IContainer components;
    private Label label1;
    private TextBox boxAmount;
    private TextBox boxNo;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Button btnCancel;
    private Button btnOK;
    private ComboBox boxType;
    private Label label7;
    private TextBox boxComments;
    private Label label21;
    private TextBox boxInstitution;
    private Label labelModifiedBy;
    private Label labelCreatedBy;
    protected EMHelpLink emHelpLink1;
    private DatePicker boxDate;
    private DatePicker boxDueDate;
    private GroupContainer groupContainer1;

    public EscrowDisbursementForm(
      EscrowDisbursementLog disbursementLog,
      int transNo,
      int nextDisbursementNo,
      LoanData loan,
      bool viewOnly)
    {
      this.loan = loan;
      this.disbursementLog = disbursementLog;
      this.InitializeComponent();
      this.initForm();
      if (disbursementLog == null)
        this.boxNo.Text = nextDisbursementNo.ToString();
      this.groupContainer1.Text = "T" + transNo.ToString("00") + " Escrow Disbursement";
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
      this.boxNo.ReadOnly = true;
      this.boxDate.ReadOnly = true;
      this.boxDueDate.ReadOnly = true;
      this.boxType.Enabled = false;
      this.boxAmount.ReadOnly = true;
      this.boxInstitution.ReadOnly = true;
      this.boxComments.ReadOnly = true;
      this.Text = "View Transaction";
      this.boxDate.BackColor = Color.WhiteSmoke;
      this.boxDueDate.BackColor = Color.WhiteSmoke;
      this.boxType.BackColor = Color.WhiteSmoke;
      this.boxAmount.BackColor = Color.WhiteSmoke;
      this.boxInstitution.BackColor = Color.WhiteSmoke;
      this.boxComments.BackColor = Color.WhiteSmoke;
    }

    public void initForm()
    {
      this.boxType.Items.AddRange((object[]) ServicingEnum.DisbursementTypes);
      if (this.disbursementLog == null)
        return;
      this.boxNo.Text = this.disbursementLog.DisbursementNo.ToString();
      DateTime dateTime;
      if (this.disbursementLog.TransactionDate != DateTime.MinValue)
      {
        DatePicker boxDate = this.boxDate;
        dateTime = this.disbursementLog.TransactionDate;
        string str = dateTime.ToString("MM/dd/yyyy");
        boxDate.Text = str;
      }
      else
        this.boxDate.Text = "";
      if (this.disbursementLog.DisbursementDueDate != DateTime.MinValue)
      {
        DatePicker boxDueDate = this.boxDueDate;
        dateTime = this.disbursementLog.DisbursementDueDate;
        string str = dateTime.ToString("MM/dd/yyyy");
        boxDueDate.Text = str;
      }
      else
        this.boxDueDate.Text = "";
      this.boxType.Text = ServicingEnum.DisbursementTypesToUI(this.disbursementLog.DisbursementType);
      if (this.disbursementLog.TransactionAmount != 0.0)
        this.boxAmount.Text = this.disbursementLog.TransactionAmount.ToString("N2");
      else
        this.boxAmount.Text = "";
      this.boxInstitution.Text = this.disbursementLog.InstitutionName;
      this.boxComments.Text = this.disbursementLog.Comments;
    }

    public EscrowDisbursementLog DisbursementLog => this.disbursementLog;

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.boxDueDate.Text.Trim() != string.Empty && !Utils.IsDate((object) this.boxDueDate.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Disbursement Due Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxDueDate.Focus();
      }
      else if (this.boxDate.Text.Trim() != string.Empty && !Utils.IsDate((object) this.boxDate.Text.Trim()))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Disbursement Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxDate.Focus();
      }
      else if (this.boxType.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Disbursement Type cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.boxType.Focus();
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
          this.disbursementLog = new EscrowDisbursementLog();
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
        this.disbursementLog.DisbursementNo = Utils.ParseInt((object) this.boxNo.Text);
        this.disbursementLog.DisbursementType = ServicingEnum.DisbursementTypesToEnum(this.boxType.Text.Trim());
        this.disbursementLog.DisbursementDueDate = !(this.boxDueDate.Text.Trim() != string.Empty) ? DateTime.MinValue : Utils.ParseDate((object) this.boxDueDate.Text.Trim());
        if (this.boxDate.Text.Trim() != string.Empty)
          this.disbursementLog.TransactionDate = Utils.ParseDate((object) this.boxDate.Text.Trim());
        else
          this.disbursementLog.TransactionDate = DateTime.MinValue;
        this.disbursementLog.InstitutionName = this.boxInstitution.Text.Trim();
        this.disbursementLog.TransactionType = ServicingTransactionTypes.EscrowDisbursement;
        this.disbursementLog.TransactionAmount = Utils.ParseDouble((object) this.boxAmount.Text.Trim());
        this.disbursementLog.Comments = this.boxComments.Text;
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

    private void boxType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.disbursementLog != null)
        return;
      for (int index = 1; index < ServicingEnum.DisbursementTypes.Length; ++index)
      {
        if (string.Compare(this.boxType.Text, ServicingEnum.DisbursementTypes[index], true) == 0)
        {
          int num1 = (index - 1) * 2 + 58;
          int num2 = num1 + 1;
          if (index == 9)
          {
            num1 = 105;
            num2 = 106;
          }
          string field1 = this.loan.GetField("SERVICE.X" + num2.ToString());
          string field2 = this.loan.GetField("SERVICE.X" + num1.ToString());
          if (Utils.ParseDouble((object) field2) != 0.0)
            this.boxAmount.Text = field2;
          else
            this.boxAmount.Text = string.Empty;
          if ((field1 == string.Empty || field1 == "//") && field2 == string.Empty)
            break;
          if (field1 != string.Empty && field1 != "//")
            this.boxDueDate.Text = field1;
          else
            this.boxDueDate.Text = "";
        }
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
      this.label1 = new Label();
      this.boxAmount = new TextBox();
      this.boxNo = new TextBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.boxType = new ComboBox();
      this.label7 = new Label();
      this.boxComments = new TextBox();
      this.label21 = new Label();
      this.boxInstitution = new TextBox();
      this.labelModifiedBy = new Label();
      this.labelCreatedBy = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.boxDueDate = new DatePicker();
      this.boxDate = new DatePicker();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(146, 151);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 61;
      this.label1.Text = "$";
      this.boxAmount.Location = new Point(162, 147);
      this.boxAmount.Name = "boxAmount";
      this.boxAmount.Size = new Size(100, 20);
      this.boxAmount.TabIndex = 6;
      this.boxAmount.TextAlign = HorizontalAlignment.Right;
      this.boxAmount.Leave += new EventHandler(this.decimal_FieldLeave);
      this.boxAmount.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.boxNo.Location = new Point(162, 36);
      this.boxNo.Name = "boxNo";
      this.boxNo.ReadOnly = true;
      this.boxNo.Size = new Size(100, 20);
      this.boxNo.TabIndex = 1;
      this.boxNo.TabStop = false;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 151);
      this.label6.Name = "label6";
      this.label6.Size = new Size(110, 13);
      this.label6.TabIndex = 55;
      this.label6.Text = "Disbursement Amount";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 62);
      this.label5.Name = "label5";
      this.label5.Size = new Size(98, 13);
      this.label5.TabIndex = 54;
      this.label5.Text = "Disbursement Type";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 129);
      this.label4.Name = "label4";
      this.label4.Size = new Size(97, 13);
      this.label4.TabIndex = 53;
      this.label4.Text = "Disbursement Date";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 107);
      this.label3.Name = "label3";
      this.label3.Size = new Size(120, 13);
      this.label3.TabIndex = 52;
      this.label3.Text = "Disbursement Due Date";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(81, 13);
      this.label2.TabIndex = 51;
      this.label2.Text = "Disbursement #";
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(310, 305);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Location = new Point(230, 305);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 8;
      this.btnOK.Text = "&Save";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.boxType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.boxType.FormattingEnabled = true;
      this.boxType.Location = new Point(162, 58);
      this.boxType.Name = "boxType";
      this.boxType.Size = new Size(209, 21);
      this.boxType.TabIndex = 2;
      this.boxType.SelectedIndexChanged += new EventHandler(this.boxType_SelectedIndexChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 174);
      this.label7.Name = "label7";
      this.label7.Size = new Size(56, 13);
      this.label7.TabIndex = 63;
      this.label7.Text = "Comments";
      this.boxComments.Location = new Point(10, 191);
      this.boxComments.Multiline = true;
      this.boxComments.Name = "boxComments";
      this.boxComments.Size = new Size(374, 108);
      this.boxComments.TabIndex = 7;
      this.label21.AutoSize = true;
      this.label21.Location = new Point(10, 85);
      this.label21.Name = "label21";
      this.label21.Size = new Size(83, 13);
      this.label21.TabIndex = 66;
      this.label21.Text = "Institution Name";
      this.boxInstitution.Location = new Point(162, 81);
      this.boxInstitution.Name = "boxInstitution";
      this.boxInstitution.Size = new Size(209, 20);
      this.boxInstitution.TabIndex = 3;
      this.boxInstitution.Tag = (object) "InstitutionName";
      this.labelModifiedBy.AutoSize = true;
      this.labelModifiedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelModifiedBy.ForeColor = SystemColors.ControlText;
      this.labelModifiedBy.Location = new Point(10, 351);
      this.labelModifiedBy.Name = "labelModifiedBy";
      this.labelModifiedBy.Size = new Size(65, 13);
      this.labelModifiedBy.TabIndex = 79;
      this.labelModifiedBy.Text = "(ModifiedBy)";
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(10, 331);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 78;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Interum Escrow Disbursement";
      this.emHelpLink1.Location = new Point(13, 382);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 112;
      this.boxDueDate.BackColor = SystemColors.Window;
      this.boxDueDate.Location = new Point(162, 103);
      this.boxDueDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.boxDueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.boxDueDate.Name = "boxDueDate";
      this.boxDueDate.Size = new Size(100, 21);
      this.boxDueDate.TabIndex = 4;
      this.boxDueDate.Value = new DateTime(0L);
      this.boxDate.BackColor = SystemColors.Window;
      this.boxDate.Location = new Point(162, 125);
      this.boxDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.boxDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.boxDate.Name = "boxDate";
      this.boxDate.Size = new Size(100, 21);
      this.boxDate.TabIndex = 5;
      this.boxDate.Value = new DateTime(0L);
      this.groupContainer1.Controls.Add((Control) this.boxDate);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.boxDueDate);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.emHelpLink1);
      this.groupContainer1.Controls.Add((Control) this.boxAmount);
      this.groupContainer1.Controls.Add((Control) this.btnOK);
      this.groupContainer1.Controls.Add((Control) this.labelModifiedBy);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.boxType);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.boxNo);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label21);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.boxComments);
      this.groupContainer1.Controls.Add((Control) this.boxInstitution);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(396, 414);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "T01 Escrow Disbursement";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(396, 414);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EscrowDisbursementForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
