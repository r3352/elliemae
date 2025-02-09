// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.EscrowInterestForm
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
  public class EscrowInterestForm : Form
  {
    private DateTime createdOn;
    private EscrowInterestLog interestLog;
    private IContainer components;
    private Button btnCancel;
    private Button btnOK;
    private TextBox boxAmount;
    private Label label3;
    private Label label2;
    private Label label1;
    private TextBox boxComments;
    private Label label7;
    private Label labelModifiedBy;
    private Label labelCreatedBy;
    protected EMHelpLink emHelpLink1;
    private DatePicker boxDate;
    private GroupContainer groupContainer1;

    public EscrowInterestForm(EscrowInterestLog interestLog, int transNo, bool viewOnly)
    {
      this.interestLog = interestLog;
      this.InitializeComponent();
      this.initForm();
      this.groupContainer1.Text = "T" + transNo.ToString("00") + " Escrow Interest";
      DateTime dateTime;
      if (this.interestLog != null)
      {
        Label labelCreatedBy = this.labelCreatedBy;
        string createdByName = this.interestLog.CreatedByName;
        dateTime = this.interestLog.CreatedDateTime;
        string str1 = dateTime.ToString("MM/dd/yyyy hh:mm tt");
        string str2 = "Created by " + createdByName + " on " + str1;
        labelCreatedBy.Text = str2;
      }
      else
      {
        this.createdOn = DateTime.Now;
        this.labelCreatedBy.Text = "Created by " + Session.UserInfo.FullName + " on " + this.createdOn.ToString("MM/dd/yyyy hh:mm tt");
      }
      if (this.interestLog != null && this.interestLog.ModifiedByName != "")
      {
        Label labelModifiedBy = this.labelModifiedBy;
        string modifiedByName = this.interestLog.ModifiedByName;
        dateTime = this.interestLog.ModifiedDateTime;
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
      this.boxComments.ReadOnly = true;
      this.Text = "View Transaction";
      this.boxDate.BackColor = Color.WhiteSmoke;
      this.boxAmount.BackColor = Color.WhiteSmoke;
      this.boxComments.BackColor = Color.WhiteSmoke;
    }

    private void initForm()
    {
      if (this.interestLog == null)
        return;
      if (this.interestLog.TransactionDate != DateTime.MinValue)
        this.boxDate.Text = this.interestLog.TransactionDate.ToString("MM/dd/yyyy");
      else
        this.boxDate.Text = "";
      if (this.interestLog.TransactionAmount != 0.0)
        this.boxAmount.Text = this.interestLog.TransactionAmount.ToString("N2");
      else
        this.boxAmount.Text = "";
      this.boxComments.Text = this.interestLog.Comments;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.boxDate.Text.Trim() == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The Interest Incurred Date field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.boxAmount.Text.Trim() == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The Interest Amount field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.interestLog == null)
        {
          this.interestLog = new EscrowInterestLog();
          this.interestLog.CreatedByID = Session.UserInfo.Userid;
          this.interestLog.CreatedByName = Session.UserInfo.FullName;
          this.interestLog.CreatedDateTime = this.createdOn;
        }
        else
        {
          this.interestLog.ModifiedByID = Session.UserInfo.Userid;
          this.interestLog.ModifiedByName = Session.UserInfo.FullName;
          this.interestLog.ModifiedDateTime = DateTime.Now;
        }
        this.interestLog.TransactionDate = Utils.ParseDate((object) this.boxDate.Text.Trim());
        this.interestLog.TransactionAmount = Utils.ParseDouble((object) this.boxAmount.Text.Trim());
        this.interestLog.Comments = this.boxComments.Text;
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

    public EscrowInterestLog InterestLog => this.interestLog;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.boxAmount = new TextBox();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.boxComments = new TextBox();
      this.label7 = new Label();
      this.labelModifiedBy = new Label();
      this.labelCreatedBy = new Label();
      this.emHelpLink1 = new EMHelpLink();
      this.boxDate = new DatePicker();
      this.groupContainer1 = new GroupContainer();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(321, 231);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.BackColor = SystemColors.Control;
      this.btnOK.Location = new Point(241, 231);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&Save";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.boxAmount.Location = new Point(157, 58);
      this.boxAmount.Name = "boxAmount";
      this.boxAmount.Size = new Size(100, 20);
      this.boxAmount.TabIndex = 2;
      this.boxAmount.TextAlign = HorizontalAlignment.Right;
      this.boxAmount.Leave += new EventHandler(this.decimal_FieldLeave);
      this.boxAmount.KeyUp += new KeyEventHandler(this.decimal_KeyUp);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(5, 62);
      this.label3.Name = "label3";
      this.label3.Size = new Size(81, 13);
      this.label3.TabIndex = 65;
      this.label3.Text = "Interest Amount";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 40);
      this.label2.Name = "label2";
      this.label2.Size = new Size(110, 13);
      this.label2.TabIndex = 64;
      this.label2.Text = "Interest Incurred Date";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(141, 61);
      this.label1.Name = "label1";
      this.label1.Size = new Size(13, 13);
      this.label1.TabIndex = 66;
      this.label1.Text = "$";
      this.boxComments.Location = new Point(10, 105);
      this.boxComments.Multiline = true;
      this.boxComments.Name = "boxComments";
      this.boxComments.Size = new Size(385, 120);
      this.boxComments.TabIndex = 3;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(5, 86);
      this.label7.Name = "label7";
      this.label7.Size = new Size(56, 13);
      this.label7.TabIndex = 68;
      this.label7.Text = "Comments";
      this.labelModifiedBy.AutoSize = true;
      this.labelModifiedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelModifiedBy.ForeColor = SystemColors.ControlText;
      this.labelModifiedBy.Location = new Point(9, 286);
      this.labelModifiedBy.Name = "labelModifiedBy";
      this.labelModifiedBy.Size = new Size(65, 13);
      this.labelModifiedBy.TabIndex = 81;
      this.labelModifiedBy.Text = "(ModifiedBy)";
      this.labelCreatedBy.AutoSize = true;
      this.labelCreatedBy.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.labelCreatedBy.ForeColor = SystemColors.ControlText;
      this.labelCreatedBy.Location = new Point(9, 266);
      this.labelCreatedBy.Name = "labelCreatedBy";
      this.labelCreatedBy.Size = new Size(62, 13);
      this.labelCreatedBy.TabIndex = 80;
      this.labelCreatedBy.Text = "(CreatedBy)";
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "Interum Escrow Interest";
      this.emHelpLink1.Location = new Point(19, 364);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 115;
      this.boxDate.BackColor = SystemColors.Window;
      this.boxDate.Location = new Point(157, 36);
      this.boxDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.boxDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.boxDate.Name = "boxDate";
      this.boxDate.Size = new Size(100, 21);
      this.boxDate.TabIndex = 1;
      this.boxDate.Value = new DateTime(0L);
      this.groupContainer1.Controls.Add((Control) this.boxDate);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.emHelpLink1);
      this.groupContainer1.Controls.Add((Control) this.boxAmount);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.labelModifiedBy);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.labelCreatedBy);
      this.groupContainer1.Controls.Add((Control) this.btnCancel);
      this.groupContainer1.Controls.Add((Control) this.btnOK);
      this.groupContainer1.Controls.Add((Control) this.boxComments);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(405, 331);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "T01 Escrow Interest";
      this.AutoScaleMode = AutoScaleMode.Inherit;
      this.ClientSize = new Size(405, 331);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (EscrowInterestForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Create/Edit Transaction";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
