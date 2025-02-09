// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationTimelineDetailForm
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationTimelineDetailForm : Form, IOnlineHelpTarget, IHelp
  {
    private const string className = "VerificationTimelineDetailForm";
    private VerificationTimelineType editMode = VerificationTimelineType.Employment;
    private IVerificationDetails iBorVerifDetails;
    private bool readOnly;
    private VerificationTimelineLog verificationLog;
    private IContainer components;
    private Label label1;
    private GroupContainer groupContainer1;
    private TextBox txtCompletedBy;
    private TextBox txtHow;
    private Label label9;
    private Label label8;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label4;
    private Label label3;
    private ComboBox cboBorType;
    private Button btnCancel;
    private Button btnOK;
    private CheckBox chkAttached;
    private TextBox txtReviewedBy;
    private DatePicker dpUploaded;
    private DatePicker dpReviewed;
    private DatePicker dpCompleted;
    private Panel panelBor;
    private TextBox txtWhat;
    private Label label2;
    private EMHelpLink emHelpLink1;

    public VerificationTimelineDetailForm(
      VerificationTimelineLog verificationLog,
      VerificationTimelineType editMode,
      bool readOnly)
    {
      this.verificationLog = verificationLog;
      this.editMode = editMode;
      this.readOnly = readOnly;
      this.InitializeComponent();
      this.Text = "Verification Timeline";
      if (this.editMode == VerificationTimelineType.Employment)
      {
        this.groupContainer1.Text = "Employment Status Verification Timeline";
        VerificationEmploymentControl employmentControl = new VerificationEmploymentControl(VerificationDetailsControl.BorrowerEditMode.Borrower);
        employmentControl.OnStatusChanged += new EventHandler(this.borVerifDetails_OnStatusChanged);
        this.iBorVerifDetails = (IVerificationDetails) employmentControl;
        this.panelBor.Height = 120;
      }
      else if (this.editMode == VerificationTimelineType.Income)
      {
        this.groupContainer1.Text = "Income Verification Timeline";
        VerificationIncomeControl verificationIncomeControl = new VerificationIncomeControl(VerificationDetailsControl.BorrowerEditMode.Borrower);
        verificationIncomeControl.OnStatusChanged += new EventHandler(this.borVerifDetails_OnStatusChanged);
        this.iBorVerifDetails = (IVerificationDetails) verificationIncomeControl;
        this.panelBor.Height = 260;
      }
      else if (this.editMode == VerificationTimelineType.Asset)
      {
        this.groupContainer1.Text = "Asset Verification Timeline";
        VerificationAssetControl verificationAssetControl = new VerificationAssetControl(VerificationDetailsControl.BorrowerEditMode.Borrower);
        verificationAssetControl.OnStatusChanged += new EventHandler(this.borVerifDetails_OnStatusChanged);
        this.iBorVerifDetails = (IVerificationDetails) verificationAssetControl;
        this.panelBor.Height = 140;
      }
      else if (this.editMode == VerificationTimelineType.Obligation)
      {
        this.groupContainer1.Text = "Monthly Obligation Verification Timeline";
        VerificationObligationControl obligationControl = new VerificationObligationControl(VerificationDetailsControl.BorrowerEditMode.Borrower);
        obligationControl.OnStatusChanged += new EventHandler(this.borVerifDetails_OnStatusChanged);
        this.iBorVerifDetails = (IVerificationDetails) obligationControl;
      }
      this.Height = this.panelBor.Top + this.panelBor.Height + 70;
      if (this.verificationLog != null)
      {
        this.cboBorType.Text = verificationLog.BorrowerType == LoanBorrowerType.Coborrower ? "Co-Borrower" : "Borrower";
        this.txtHow.Text = verificationLog.HowCompleted;
        this.txtCompletedBy.Text = verificationLog.CompletedBy;
        this.dpCompleted.Text = verificationLog.DateCompleted == DateTime.MinValue ? "" : verificationLog.DateCompleted.ToString("MM/dd/yyyy");
        this.txtReviewedBy.Text = verificationLog.ReviewedBy;
        this.dpReviewed.Text = verificationLog.DateReviewed == DateTime.MinValue ? "" : verificationLog.DateReviewed.ToString("MM/dd/yyyy");
        this.chkAttached.Checked = verificationLog.EFolderAttached;
        this.dpUploaded.Text = verificationLog.DateUploaded == DateTime.MinValue ? "" : verificationLog.DateUploaded.ToString("MM/dd/yyyy");
        this.iBorVerifDetails.SetDetails(verificationLog);
        this.txtWhat.Text = this.iBorVerifDetails.BuildWhatVerified();
      }
      this.panelBor.Controls.Add((Control) this.iBorVerifDetails);
      if (!this.readOnly)
        return;
      this.btnOK.Visible = false;
      this.btnCancel.Text = "&Close";
      this.cboBorType.Enabled = false;
      this.txtHow.ReadOnly = true;
      this.txtCompletedBy.ReadOnly = true;
      this.dpCompleted.ReadOnly = true;
      this.txtReviewedBy.ReadOnly = true;
      this.dpReviewed.ReadOnly = true;
      this.chkAttached.Enabled = false;
      this.dpUploaded.ReadOnly = true;
      this.iBorVerifDetails.SetReadOnly();
      this.txtWhat.ReadOnly = true;
    }

    private void borVerifDetails_OnStatusChanged(object sender, EventArgs e)
    {
      this.txtWhat.Text = this.iBorVerifDetails.BuildWhatVerified();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      if (this.cboBorType.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select borrower type.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cboBorType.Focus();
      }
      else
      {
        if (this.verificationLog == null)
          this.verificationLog = new VerificationTimelineLog();
        this.verificationLog.BorrowerType = this.cboBorType.Text == "Co-Borrower" ? LoanBorrowerType.Coborrower : LoanBorrowerType.Borrower;
        this.verificationLog.HowCompleted = this.txtHow.Text;
        this.verificationLog.CompletedBy = this.txtCompletedBy.Text;
        this.verificationLog.DateCompleted = this.dpCompleted.Text == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) this.dpCompleted.Text);
        this.verificationLog.ReviewedBy = this.txtReviewedBy.Text;
        this.verificationLog.DateReviewed = this.dpReviewed.Text == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) this.dpReviewed.Text);
        this.verificationLog.EFolderAttached = this.chkAttached.Checked;
        this.verificationLog.DateUploaded = this.dpUploaded.Text == string.Empty ? DateTime.MinValue : Utils.ParseDate((object) this.dpUploaded.Text);
        this.iBorVerifDetails.GetDetails(this.verificationLog);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void cboBorType_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setTitleInBorrowerSection();
    }

    private void setTitleInBorrowerSection()
    {
      this.iBorVerifDetails.SetBorrowerType(this.cboBorType.Text == "Co-Borrower" ? VerificationDetailsControl.BorrowerEditMode.CoBorrower : VerificationDetailsControl.BorrowerEditMode.Borrower);
    }

    public VerificationTimelineLog VerificationLog => this.verificationLog;

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, this.GetHelpTargetName());
    }

    public string GetHelpTargetName() => nameof (VerificationTimelineDetailForm);

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.groupContainer1 = new GroupContainer();
      this.txtWhat = new TextBox();
      this.label2 = new Label();
      this.dpUploaded = new DatePicker();
      this.dpReviewed = new DatePicker();
      this.dpCompleted = new DatePicker();
      this.chkAttached = new CheckBox();
      this.txtReviewedBy = new TextBox();
      this.txtCompletedBy = new TextBox();
      this.txtHow = new TextBox();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label3 = new Label();
      this.cboBorType = new ComboBox();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.panelBor = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.groupContainer1.SuspendLayout();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(6, 38);
      this.label1.Name = "label1";
      this.label1.Size = new Size(76, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Borrower Type";
      this.groupContainer1.BackColor = Color.White;
      this.groupContainer1.Controls.Add((Control) this.txtWhat);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.dpUploaded);
      this.groupContainer1.Controls.Add((Control) this.dpReviewed);
      this.groupContainer1.Controls.Add((Control) this.dpCompleted);
      this.groupContainer1.Controls.Add((Control) this.chkAttached);
      this.groupContainer1.Controls.Add((Control) this.txtReviewedBy);
      this.groupContainer1.Controls.Add((Control) this.txtCompletedBy);
      this.groupContainer1.Controls.Add((Control) this.txtHow);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.label8);
      this.groupContainer1.Controls.Add((Control) this.label7);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.cboBorType);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Dock = DockStyle.Top;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(567, 249);
      this.groupContainer1.TabIndex = 1;
      this.groupContainer1.Text = "Employment Status Verification";
      this.txtWhat.Location = new Point(197, 57);
      this.txtWhat.Name = "txtWhat";
      this.txtWhat.ReadOnly = true;
      this.txtWhat.Size = new Size(358, 20);
      this.txtWhat.TabIndex = 10;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(6, 60);
      this.label2.Name = "label2";
      this.label2.Size = new Size(92, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "What was verified";
      this.dpUploaded.BackColor = SystemColors.Window;
      this.dpUploaded.Location = new Point(197, 210);
      this.dpUploaded.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpUploaded.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpUploaded.Name = "dpUploaded";
      this.dpUploaded.Size = new Size(85, 21);
      this.dpUploaded.TabIndex = 9;
      this.dpUploaded.Tag = (object) "3859";
      this.dpUploaded.ToolTip = "";
      this.dpUploaded.Value = new DateTime(0L);
      this.dpReviewed.BackColor = SystemColors.Window;
      this.dpReviewed.Location = new Point(197, 168);
      this.dpReviewed.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpReviewed.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpReviewed.Name = "dpReviewed";
      this.dpReviewed.Size = new Size(85, 21);
      this.dpReviewed.TabIndex = 7;
      this.dpReviewed.Tag = (object) "3859";
      this.dpReviewed.ToolTip = "";
      this.dpReviewed.Value = new DateTime(0L);
      this.dpCompleted.BackColor = SystemColors.Window;
      this.dpCompleted.Location = new Point(197, 123);
      this.dpCompleted.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpCompleted.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCompleted.Name = "dpCompleted";
      this.dpCompleted.Size = new Size(85, 21);
      this.dpCompleted.TabIndex = 5;
      this.dpCompleted.Tag = (object) "3859";
      this.dpCompleted.ToolTip = "";
      this.dpCompleted.Value = new DateTime(0L);
      this.chkAttached.AutoSize = true;
      this.chkAttached.Location = new Point(197, 193);
      this.chkAttached.Name = "chkAttached";
      this.chkAttached.Size = new Size(15, 14);
      this.chkAttached.TabIndex = 8;
      this.chkAttached.UseVisualStyleBackColor = true;
      this.txtReviewedBy.Location = new Point(197, 146);
      this.txtReviewedBy.Name = "txtReviewedBy";
      this.txtReviewedBy.Size = new Size(358, 20);
      this.txtReviewedBy.TabIndex = 6;
      this.txtCompletedBy.Location = new Point(197, 101);
      this.txtCompletedBy.Name = "txtCompletedBy";
      this.txtCompletedBy.Size = new Size(358, 20);
      this.txtCompletedBy.TabIndex = 3;
      this.txtHow.Location = new Point(197, 79);
      this.txtHow.Name = "txtHow";
      this.txtHow.Size = new Size(358, 20);
      this.txtHow.TabIndex = 2;
      this.label9.AutoSize = true;
      this.label9.Location = new Point(6, 213);
      this.label9.Name = "label9";
      this.label9.Size = new Size((int) sbyte.MaxValue, 13);
      this.label9.TabIndex = 9;
      this.label9.Text = "Date uploaded to eFolder";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(6, 192);
      this.label8.Name = "label8";
      this.label8.Size = new Size(152, 13);
      this.label8.TabIndex = 8;
      this.label8.Text = "Document Attached to eFolder";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(6, 171);
      this.label7.Name = "label7";
      this.label7.Size = new Size(81, 13);
      this.label7.TabIndex = 7;
      this.label7.Text = "Date Reviewed";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(6, 149);
      this.label6.Name = "label6";
      this.label6.Size = new Size(130, 13);
      this.label6.TabIndex = 6;
      this.label6.Text = "Verifications Reviewed By";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(6, (int) sbyte.MaxValue);
      this.label5.Name = "label5";
      this.label5.Size = new Size(138, 13);
      this.label5.TabIndex = 5;
      this.label5.Text = "Date Verification Completed";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(6, 104);
      this.label4.Name = "label4";
      this.label4.Size = new Size((int) sbyte.MaxValue, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Verification Completed By";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(6, 82);
      this.label3.Name = "label3";
      this.label3.Size = new Size(177, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "How was the Verification Completed";
      this.cboBorType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorType.FormattingEnabled = true;
      this.cboBorType.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Borrower",
        (object) "Co-Borrower"
      });
      this.cboBorType.Location = new Point(197, 34);
      this.cboBorType.Name = "cboBorType";
      this.cboBorType.Size = new Size(121, 21);
      this.cboBorType.TabIndex = 1;
      this.cboBorType.SelectedIndexChanged += new EventHandler(this.cboBorType_SelectedIndexChanged);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(480, 581);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 11;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(402, 581);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 10;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.panelBor.Dock = DockStyle.Top;
      this.panelBor.Location = new Point(0, 249);
      this.panelBor.Name = "panelBor";
      this.panelBor.Size = new Size(567, 322);
      this.panelBor.TabIndex = 12;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = nameof (VerificationTimelineDetailForm);
      this.emHelpLink1.Location = new Point(9, 588);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 71;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(567, 611);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.panelBor);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.groupContainer1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (VerificationTimelineDetailForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Timeline";
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
