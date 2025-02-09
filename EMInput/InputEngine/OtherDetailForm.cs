// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.OtherDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
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
  public class OtherDetailForm : Form
  {
    private DateTime createdOn;
    private OtherTransactionLog otherTransLog;
    private IContainer components;
    private TextBox boxAccount;
    private TextBox boxRouting;
    private TextBox boxInstitution;
    private TextBox boxAmount;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private Label label2;
    private Label label22;
    private TextBox boxComment;
    private Button btnCancel;
    private Button btnSave;
    private TextBox boxReference;
    private Label label7;
    private Label labelModifiedBy;
    private Label labelCreatedBy;
    protected EMHelpLink emHelpLink1;
    private DatePicker boxDate;
    private GroupContainer groupContainer1;

    public OtherDetailForm(OtherTransactionLog otherTransLog, int transNo, bool viewOnly)
    {
      this.otherTransLog = otherTransLog;
      this.InitializeComponent();
      this.initForm();
      this.groupContainer1.Text = "T" + transNo.ToString("00") + " Other";
      DateTime dateTime;
      if (this.otherTransLog != null)
      {
        Label labelCreatedBy = this.labelCreatedBy;
        string createdByName = this.otherTransLog.CreatedByName;
        dateTime = this.otherTransLog.CreatedDateTime;
        string str1 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        string str2 = "Created by " + createdByName + " on " + str1;
        labelCreatedBy.Text = str2;
      }
      else
      {
        this.createdOn = DateTime.Now;
        this.labelCreatedBy.Text = "Created by " + Session.UserInfo.FullName + " on " + this.createdOn.ToString("MM/dd/yyyy hh:mm tt");
      }
      if (this.otherTransLog != null && this.otherTransLog.ModifiedByName != "")
      {
        Label labelModifiedBy = this.labelModifiedBy;
        string modifiedByName = this.otherTransLog.ModifiedByName;
        dateTime = this.otherTransLog.ModifiedDateTime;
        string str3 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        string str4 = "Last modified by " + modifiedByName + " on " + str3;
        labelModifiedBy.Text = str4;
      }
      else
        this.labelModifiedBy.Text = "";
      if (!viewOnly)
        return;
      this.btnSave.Visible = false;
      this.btnCancel.Text = "OK";
      this.boxDate.ReadOnly = true;
      this.boxAmount.ReadOnly = true;
      this.boxInstitution.ReadOnly = true;
      this.boxRouting.ReadOnly = true;
      this.boxAccount.ReadOnly = true;
      this.boxReference.ReadOnly = true;
      this.boxComment.ReadOnly = true;
      this.boxDate.BackColor = Color.WhiteSmoke;
      this.boxAmount.BackColor = Color.WhiteSmoke;
      this.boxInstitution.BackColor = Color.WhiteSmoke;
      this.boxRouting.BackColor = Color.WhiteSmoke;
      this.boxAccount.BackColor = Color.WhiteSmoke;
      this.boxReference.BackColor = Color.WhiteSmoke;
      this.boxComment.BackColor = Color.WhiteSmoke;
      this.Text = "View Transaction";
    }

    private void initForm()
    {
      if (this.otherTransLog == null)
        return;
      if (this.otherTransLog.TransactionDate != DateTime.MinValue)
        this.boxDate.Text = this.otherTransLog.TransactionDate.ToString("MM/dd/yyyy");
      else
        this.boxDate.Text = "";
      if (this.otherTransLog.TransactionAmount != 0.0)
        this.boxAmount.Text = this.otherTransLog.TransactionAmount.ToString("N2");
      else
        this.boxAmount.Text = "";
      this.boxInstitution.Text = this.otherTransLog.InstitutionName;
      this.boxRouting.Text = this.otherTransLog.InstitutionRouting;
      this.boxAccount.Text = this.otherTransLog.AccountNumber;
      this.boxReference.Text = this.otherTransLog.Reference;
      this.boxComment.Text = this.otherTransLog.Comments;
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

    public OtherTransactionLog OtherTransLog => this.otherTransLog;

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.boxDate.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The Date field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.boxAmount.Text.Trim() == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The Amount field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.otherTransLog == null)
        {
          this.otherTransLog = new OtherTransactionLog();
          this.otherTransLog.CreatedByID = Session.UserInfo.Userid;
          this.otherTransLog.CreatedByName = Session.UserInfo.FullName;
          this.otherTransLog.CreatedDateTime = this.createdOn;
        }
        else
        {
          this.otherTransLog.ModifiedByID = Session.UserInfo.Userid;
          this.otherTransLog.ModifiedByName = Session.UserInfo.FullName;
          this.otherTransLog.ModifiedDateTime = DateTime.Now;
        }
        this.otherTransLog.TransactionDate = Utils.ParseDate((object) this.boxDate.Text.Trim());
        this.otherTransLog.TransactionAmount = Utils.ParseDouble((object) this.boxAmount.Text.Trim());
        this.otherTransLog.InstitutionName = this.boxInstitution.Text.Trim();
        this.otherTransLog.InstitutionRouting = this.boxRouting.Text.Trim();
        this.otherTransLog.AccountNumber = this.boxAccount.Text.Trim();
        this.otherTransLog.Reference = this.boxReference.Text.Trim();
        this.otherTransLog.Comments = this.boxComment.Text.Trim();
        this.DialogResult = DialogResult.OK;
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
      this.boxAccount = new TextBox();
      this.boxRouting = new TextBox();
      this.boxInstitution = new TextBox();
      this.boxAmount = new TextBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label22 = new Label();
      this.boxComment = new TextBox();
      this.btnCancel = new Button();
      this.btnSave = new Button();
      this.boxReference = new TextBox();
      this.label7 = new Label();
      this.labelModifiedBy = new Label();
      this.labelCreatedBy = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.boxDate = new DatePicker();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.boxAccount.Location = new Point(162, 124);
      this.boxAccount.Name = "boxAccount";
      this.boxAccount.Size = new Size(100, 20);
      this.boxAccount.TabIndex = 5;
      this.boxAccount.Tag = (object) "PaymentDepositedDate";
      this.boxRouting.Location = new Point(162, 102);
      this.boxRouting.Name = "boxRouting";
      this.boxRouting.Size = new Size(100, 20);
      this.boxRouting.TabIndex = 4;
      this.boxRouting.Tag = (object) "PaymentReceivedDate";
      this.boxInstitution.Location = new Point(162, 80);
      this.boxInstitution.Name = "boxInstitution";
      this.boxInstitution.Size = new Size(100, 20);
      this.boxInstitution.TabIndex = 3;
      this.boxInstitution.Tag = (object) "LatePaymentDate";
      this.boxAmount.Location = new Point(162, 58);
      this.boxAmount.Name = "boxAmount";
      this.boxAmount.Size = new Size(100, 20);
      this.boxAmount.TabIndex = 2;
      this.boxAmount.Tag = (object) "PaymentDueDate";
      this.boxAmount.TextAlign = HorizontalAlignment.Right;
      this.boxAmount.Leave += new EventHandler(this.decimal_FieldLeave);
      this.boxAmount.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, (int) sbyte.MaxValue);
      this.label6.Name = "label6";
      this.label6.Size = new Size(87, 13);
      this.label6.TabIndex = 20;
      this.label6.Text = "Account Number";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 105);
      this.label5.Name = "label5";
      this.label5.Size = new Size(132, 13);
      this.label5.TabIndex = 19;
      this.label5.Text = "Institution Routing Number";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(10, 83);
      this.label4.Name = "label4";
      this.label4.Size = new Size(52, 13);
      this.label4.TabIndex = 18;
      this.label4.Text = "Institution";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 61);
      this.label3.Name = "label3";
      this.label3.Size = new Size(43, 13);
      this.label3.TabIndex = 17;
      this.label3.Text = "Amount";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(10, 39);
      this.label2.Name = "label2";
      this.label2.Size = new Size(30, 13);
      this.label2.TabIndex = 14;
      this.label2.Text = "Date";
      this.label22.AutoSize = true;
      this.label22.Location = new Point(8, 171);
      this.label22.Name = "label22";
      this.label22.Size = new Size(56, 13);
      this.label22.TabIndex = 47;
      this.label22.Text = "Comments";
      this.boxComment.Location = new Point(10, 190);
      this.boxComment.Multiline = true;
      this.boxComment.Name = "boxComment";
      this.boxComment.ScrollBars = ScrollBars.Vertical;
      this.boxComment.Size = new Size(385, 153);
      this.boxComment.TabIndex = 7;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(321, 351);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 9;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSave.BackColor = SystemColors.Control;
      this.btnSave.Location = new Point(241, 351);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 23);
      this.btnSave.TabIndex = 8;
      this.btnSave.Text = "&Save";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.boxReference.Location = new Point(162, 146);
      this.boxReference.Name = "boxReference";
      this.boxReference.Size = new Size(100, 20);
      this.boxReference.TabIndex = 6;
      this.boxReference.Tag = (object) "PaymentDepositedDate";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 149);
      this.label7.Name = "label7";
      this.label7.Size = new Size(97, 13);
      this.label7.TabIndex = 51;
      this.label7.Text = "Reference Number";
      this.labelModifiedBy.AutoSize = true;
      this.labelModifiedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelModifiedBy.ForeColor = SystemColors.ControlText;
      this.labelModifiedBy.Location = new Point(10, 397);
      this.labelModifiedBy.Name = "labelModifiedBy";
      this.labelModifiedBy.Size = new Size(65, 13);
      this.labelModifiedBy.TabIndex = 77;
      this.labelModifiedBy.Text = "(ModifiedBy)";
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(10, 377);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 76;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Interum Other Detail";
      this.emHelpLink1.Location = new Point(14, 764);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 113;
      this.boxDate.BackColor = SystemColors.Window;
      this.boxDate.Location = new Point(162, 36);
      this.boxDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.boxDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.boxDate.Name = "boxDate";
      this.boxDate.Size = new Size(100, 21);
      this.boxDate.TabIndex = 1;
      this.boxDate.Tag = (object) "StatementDate";
      this.boxDate.Value = new DateTime(0L);
      this.groupContainer1.Controls.Add((Control) this.boxDate);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.emHelpLink1);
      this.groupContainer1.Controls.Add((Control) this.boxRouting);
      this.groupContainer1.Controls.Add((Control) this.boxAccount);
      this.groupContainer1.Controls.Add((Control) this.labelModifiedBy);
      this.groupContainer1.Controls.Add((Control) this.boxInstitution);
      this.groupContainer1.Controls.Add((Control) this.boxComment);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.boxAmount);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label22);
      this.groupContainer1.Controls.Add((Control) this.boxReference);
      this.groupContainer1.Controls.Add((Control) this.btnSave);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(405, 447);
      this.groupContainer1.TabIndex = 0;
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(405, 447);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (OtherDetailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
