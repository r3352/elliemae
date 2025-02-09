// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompCurrentControl
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LOCompCurrentControl : UserControl
  {
    private Sessions.Session session;
    private LoanCompHistoryList loanCompHistoryList;
    private LoanCompHistory selectedLoanCompHistory;
    private string parentID = string.Empty;
    private string currentID = string.Empty;
    private bool forUser;
    private bool forExternal;
    private bool forExternalLender;
    private DateTime originalStartDate;
    private IContainer components;
    private GroupContainer grpCurrent;
    private TextBox txtMaxAmt;
    private Label lblMaxAmount;
    private TextBox txtMinAmt;
    private Label lblMinAmount;
    private TextBox txtAmount2;
    private Label lblAmount2;
    private TextBox txtAmount;
    private TextBox txtMinTerm;
    private TextBox txtName;
    private Label lblAmount;
    private Label lblMinTerm;
    private Label lblStartDate;
    private Label lblName;
    private DatePicker datePickerStartDate;

    public event EventHandler StartDateChanged;

    public LOCompCurrentControl(Sessions.Session session, bool forUser, bool forExternal)
      : this(session, forUser, forExternal, false)
    {
    }

    public LOCompCurrentControl(
      Sessions.Session session,
      bool forUser,
      bool forExternal,
      bool forExternalLender)
    {
      this.session = session;
      this.forUser = forUser;
      this.forExternal = forExternal;
      this.forExternalLender = forExternalLender;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
    }

    public void RefreshDate(
      LoanCompHistoryList loanCompHistoryList,
      string parentID,
      string currentID)
    {
      this.loanCompHistoryList = loanCompHistoryList;
      this.parentID = parentID;
      this.currentID = currentID;
      this.RefreshPlanDetails(this.loanCompHistoryList.GetCurrentPlan(DateTime.Today.Date));
    }

    public void RefreshPlanDetails(LoanCompHistory loanCompHistory)
    {
      this.txtName.Text = loanCompHistory != null ? loanCompHistory.PlanName : "";
      DateTime dateTime;
      if (loanCompHistory != null)
      {
        this.datePickerStartDate.ValueChanged -= new EventHandler(this.datePickerStartDate_ValueChanged);
        this.datePickerStartDate.Value = loanCompHistory.StartDate.Date;
        this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
        dateTime = loanCompHistory.StartDate;
        this.originalStartDate = dateTime.Date;
        this.selectedLoanCompHistory = loanCompHistory;
      }
      else
      {
        this.datePickerStartDate.Text = string.Empty;
        this.txtName.Focus();
        this.selectedLoanCompHistory = (LoanCompHistory) null;
      }
      this.txtMinTerm.Text = loanCompHistory == null || loanCompHistory.MinTermDays <= 0 ? "" : loanCompHistory.MinTermDays.ToString("");
      TextBox txtAmount = this.txtAmount;
      Decimal num;
      string str1;
      if (loanCompHistory == null || !(loanCompHistory.PercentAmt != 0M))
      {
        str1 = "";
      }
      else
      {
        num = loanCompHistory.PercentAmt;
        str1 = num.ToString("N5");
      }
      txtAmount.Text = str1;
      TextBox txtAmount2 = this.txtAmount2;
      string str2;
      if (loanCompHistory == null || !(loanCompHistory.DollarAmount != 0M))
      {
        str2 = "";
      }
      else
      {
        num = loanCompHistory.DollarAmount;
        str2 = num.ToString("N2");
      }
      txtAmount2.Text = str2;
      TextBox txtMinAmt = this.txtMinAmt;
      string str3;
      if (loanCompHistory == null || !(loanCompHistory.MinDollarAmount != 0M))
      {
        str3 = "";
      }
      else
      {
        num = loanCompHistory.MinDollarAmount;
        str3 = num.ToString("N2");
      }
      txtMinAmt.Text = str3;
      TextBox txtMaxAmt = this.txtMaxAmt;
      string str4;
      if (loanCompHistory == null || !(loanCompHistory.MaxDollarAmount != 0M))
      {
        str4 = "";
      }
      else
      {
        num = loanCompHistory.MaxDollarAmount;
        str4 = num.ToString("N2");
      }
      txtMaxAmt.Text = str4;
      if (!(this.txtName.Text == string.Empty))
      {
        if (loanCompHistory != null)
        {
          dateTime = loanCompHistory.StartDate;
          DateTime date1 = dateTime.Date;
          dateTime = DateTime.Today;
          DateTime date2 = dateTime.Date;
          if (date1 <= date2)
            goto label_19;
        }
        if (!this.loanCompHistoryList.UseParentInfo)
        {
          this.datePickerStartDate.ReadOnly = false;
          return;
        }
      }
label_19:
      this.datePickerStartDate.ReadOnly = true;
    }

    public void SetReadOnly(bool readOnly) => this.datePickerStartDate.ReadOnly = readOnly;

    private void datePickerStartDate_ValueChanged(object sender, EventArgs e)
    {
      DateTime date = this.datePickerStartDate.Value.Date;
      if (this.datePickerStartDate.Text == "" && this.txtName.Text.Trim() != string.Empty)
      {
        this.datePickerStartDate.ValueChanged -= new EventHandler(this.datePickerStartDate_ValueChanged);
        this.datePickerStartDate.Value = this.originalStartDate;
        this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
        int num = (int) Utils.Dialog((IWin32Window) this, "The Start Date cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (date.Date < DateTime.Today.Date && this.txtName.Text.Trim() != string.Empty)
      {
        this.datePickerStartDate.ValueChanged -= new EventHandler(this.datePickerStartDate_ValueChanged);
        this.datePickerStartDate.Value = this.originalStartDate;
        this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
        int num = (int) Utils.Dialog((IWin32Window) this, "The Start Date cannot be earlier than today's date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.loanCompHistoryList.IsNewPlanStartDateValid(this.selectedLoanCompHistory, date) && this.txtName.Text.Trim() != string.Empty)
      {
        this.datePickerStartDate.ValueChanged -= new EventHandler(this.datePickerStartDate_ValueChanged);
        this.datePickerStartDate.Value = this.originalStartDate;
        this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
        int num = (int) Utils.Dialog((IWin32Window) this, "The Start Date is invalid in current Assigned Comp Plan list.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (!this.loanCompHistoryList.IsNewPlanStartDateValidWithMinimumTermDays(this.selectedLoanCompHistory, date) && this.txtName.Text.Trim() != string.Empty && Utils.Dialog((IWin32Window) this, "The Start Date entered is prior to the Earliest Change Date for the previous plan. Would you like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
      {
        this.datePickerStartDate.ValueChanged -= new EventHandler(this.datePickerStartDate_ValueChanged);
        this.datePickerStartDate.Value = this.originalStartDate;
        this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
      }
      else
      {
        this.selectedLoanCompHistory.StartDate = this.datePickerStartDate.Value.Date;
        this.loanCompHistoryList.SortPlans(true);
        if (this.StartDateChanged == null)
          return;
        this.StartDateChanged((object) this.selectedLoanCompHistory, EventArgs.Empty);
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
      this.grpCurrent = new GroupContainer();
      this.datePickerStartDate = new DatePicker();
      this.txtMaxAmt = new TextBox();
      this.lblMaxAmount = new Label();
      this.txtMinAmt = new TextBox();
      this.lblMinAmount = new Label();
      this.txtAmount2 = new TextBox();
      this.lblAmount2 = new Label();
      this.txtAmount = new TextBox();
      this.txtMinTerm = new TextBox();
      this.txtName = new TextBox();
      this.lblAmount = new Label();
      this.lblMinTerm = new Label();
      this.lblStartDate = new Label();
      this.lblName = new Label();
      this.grpCurrent.SuspendLayout();
      this.SuspendLayout();
      this.grpCurrent.Controls.Add((Control) this.datePickerStartDate);
      this.grpCurrent.Controls.Add((Control) this.txtMaxAmt);
      this.grpCurrent.Controls.Add((Control) this.lblMaxAmount);
      this.grpCurrent.Controls.Add((Control) this.txtMinAmt);
      this.grpCurrent.Controls.Add((Control) this.lblMinAmount);
      this.grpCurrent.Controls.Add((Control) this.txtAmount2);
      this.grpCurrent.Controls.Add((Control) this.lblAmount2);
      this.grpCurrent.Controls.Add((Control) this.txtAmount);
      this.grpCurrent.Controls.Add((Control) this.txtMinTerm);
      this.grpCurrent.Controls.Add((Control) this.txtName);
      this.grpCurrent.Controls.Add((Control) this.lblAmount);
      this.grpCurrent.Controls.Add((Control) this.lblMinTerm);
      this.grpCurrent.Controls.Add((Control) this.lblStartDate);
      this.grpCurrent.Controls.Add((Control) this.lblName);
      this.grpCurrent.Dock = DockStyle.Fill;
      this.grpCurrent.HeaderForeColor = SystemColors.ControlText;
      this.grpCurrent.Location = new Point(0, 0);
      this.grpCurrent.Name = "grpCurrent";
      this.grpCurrent.Size = new Size(407, 190);
      this.grpCurrent.TabIndex = 0;
      this.grpCurrent.Text = "LO Comp Plan Details";
      this.datePickerStartDate.BackColor = SystemColors.Window;
      this.datePickerStartDate.Location = new Point(101, 54);
      this.datePickerStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datePickerStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datePickerStartDate.Name = "datePickerStartDate";
      this.datePickerStartDate.Size = new Size(100, 21);
      this.datePickerStartDate.TabIndex = 3;
      this.datePickerStartDate.ToolTip = "";
      this.datePickerStartDate.Value = new DateTime(0L);
      this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
      this.txtMaxAmt.Location = new Point(101, 165);
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.ReadOnly = true;
      this.txtMaxAmt.Size = new Size(100, 20);
      this.txtMaxAmt.TabIndex = 13;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.lblMaxAmount.AutoSize = true;
      this.lblMaxAmount.Location = new Point(8, 168);
      this.lblMaxAmount.Name = "lblMaxAmount";
      this.lblMaxAmount.Size = new Size(60, 13);
      this.lblMaxAmount.TabIndex = 12;
      this.lblMaxAmount.Text = "Maximum $";
      this.txtMinAmt.Location = new Point(101, 143);
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.ReadOnly = true;
      this.txtMinAmt.Size = new Size(100, 20);
      this.txtMinAmt.TabIndex = 11;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.lblMinAmount.AutoSize = true;
      this.lblMinAmount.Location = new Point(8, 146);
      this.lblMinAmount.Name = "lblMinAmount";
      this.lblMinAmount.Size = new Size(57, 13);
      this.lblMinAmount.TabIndex = 10;
      this.lblMinAmount.Text = "Minimum $";
      this.txtAmount2.Location = new Point(101, 121);
      this.txtAmount2.Name = "txtAmount2";
      this.txtAmount2.ReadOnly = true;
      this.txtAmount2.Size = new Size(100, 20);
      this.txtAmount2.TabIndex = 9;
      this.txtAmount2.TextAlign = HorizontalAlignment.Right;
      this.lblAmount2.AutoSize = true;
      this.lblAmount2.Location = new Point(8, 123);
      this.lblAmount2.Name = "lblAmount2";
      this.lblAmount2.Size = new Size(52, 13);
      this.lblAmount2.TabIndex = 8;
      this.lblAmount2.Text = "$ Amount";
      this.txtAmount.Location = new Point(101, 99);
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.ReadOnly = true;
      this.txtAmount.Size = new Size(100, 20);
      this.txtAmount.TabIndex = 7;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMinTerm.Location = new Point(101, 77);
      this.txtMinTerm.Name = "txtMinTerm";
      this.txtMinTerm.ReadOnly = true;
      this.txtMinTerm.Size = new Size(100, 20);
      this.txtMinTerm.TabIndex = 5;
      this.txtMinTerm.TextAlign = HorizontalAlignment.Right;
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(101, 32);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(301, 20);
      this.txtName.TabIndex = 1;
      this.lblAmount.AutoSize = true;
      this.lblAmount.Location = new Point(8, 101);
      this.lblAmount.Name = "lblAmount";
      this.lblAmount.Size = new Size(54, 13);
      this.lblAmount.TabIndex = 6;
      this.lblAmount.Text = "% Amount";
      this.lblMinTerm.AutoSize = true;
      this.lblMinTerm.Location = new Point(8, 79);
      this.lblMinTerm.Name = "lblMinTerm";
      this.lblMinTerm.Size = new Size(88, 13);
      this.lblMinTerm.TabIndex = 4;
      this.lblMinTerm.Text = "Min Term # Days";
      this.lblStartDate.AutoSize = true;
      this.lblStartDate.Location = new Point(8, 57);
      this.lblStartDate.Name = "lblStartDate";
      this.lblStartDate.Size = new Size(55, 13);
      this.lblStartDate.TabIndex = 2;
      this.lblStartDate.Text = "Start Date";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(8, 35);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(35, 13);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpCurrent);
      this.Name = nameof (LOCompCurrentControl);
      this.Size = new Size(407, 190);
      this.grpCurrent.ResumeLayout(false);
      this.grpCurrent.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
