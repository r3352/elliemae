// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.AssignLOCompDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class AssignLOCompDialog : Form
  {
    private Sessions.Session session;
    private string currentBranchOrUserID = string.Empty;
    private LoanCompHistoryList loanCompHistoryList;
    private LoanCompHistory newLoanCompHistory;
    private IContainer components;
    private Button btnCancel;
    private Button btnSelect;
    private Panel panelAll;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panelPlanList;
    private Panel panelTitle;
    private Label label1;
    private Panel panelPlanDetails;
    private GroupContainer grpDetails;
    private GroupContainer grpPlans;
    private GridView gridViewPlans;
    private TextBox txtEarliestChangeDate;
    private Label label2;
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
    private Label lblActivation;
    private Label lblName;
    private DatePicker datePickerStartDate;

    public AssignLOCompDialog(Sessions.Session session, string currentBranchOrUserID)
    {
      this.session = session;
      this.currentBranchOrUserID = currentBranchOrUserID;
      this.InitializeComponent();
    }

    public bool RefreshForm(LoanCompHistoryList loanCompHistoryList, int compType = 1)
    {
      this.loanCompHistoryList = loanCompHistoryList;
      this.gridViewPlans.Items.Clear();
      List<LoanCompPlan> allCompPlans = this.session.ConfigurationManager.GetAllCompPlans(true, compType);
      if (allCompPlans == null || allCompPlans.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are no activated LO Compensation Plans available.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      Cursor.Current = Cursors.WaitCursor;
      this.gridViewPlans.BeginUpdate();
      for (int index = 0; index < allCompPlans.Count; ++index)
        this.gridViewPlans.Items.Add(new GVItem(allCompPlans[index].Name)
        {
          SubItems = {
            !(allCompPlans[index].ActivationDate != DateTime.MaxValue) || !(allCompPlans[index].ActivationDate != DateTime.MinValue) ? (object) "" : (object) allCompPlans[index].ActivationDate.ToString("MM/dd/yyyy")
          },
          Tag = (object) allCompPlans[index]
        });
      this.gridViewPlans.EndUpdate();
      this.gridViewPlans_SelectedIndexChanged((object) null, (EventArgs) null);
      Cursor.Current = Cursors.Default;
      return true;
    }

    private void gridViewPlans_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gridViewPlans.SelectedItems == null || this.gridViewPlans.SelectedItems.Count == 0)
      {
        this.changeFieldStatus(true);
      }
      else
      {
        LoanCompPlan tag = (LoanCompPlan) this.gridViewPlans.SelectedItems[0].Tag;
        this.txtName.Text = tag.Name;
        DatePicker datePickerStartDate = this.datePickerStartDate;
        DateTime dateTime = this.session.SessionObjects.Session.ServerTime;
        dateTime = dateTime.Date;
        DateTime date = dateTime.AddDays(1.0).Date;
        datePickerStartDate.Value = date;
        this.datePickerStartDate.ReadOnly = false;
        this.txtMinTerm.Text = tag.MinTermDays > 0 ? tag.MinTermDays.ToString("") : "";
        this.txtEarliestChangeDate.Text = this.datePickerStartDate.Value.Date.AddDays((double) tag.MinTermDays).ToString("MM/dd/yyyy");
        TextBox txtAmount = this.txtAmount;
        Decimal num;
        string str1;
        if (!(tag.PercentAmt != 0M))
        {
          str1 = "";
        }
        else
        {
          num = tag.PercentAmt;
          str1 = num.ToString("N5");
        }
        txtAmount.Text = str1;
        TextBox txtAmount2 = this.txtAmount2;
        string str2;
        if (!(tag.DollarAmount != 0M))
        {
          str2 = "";
        }
        else
        {
          num = tag.DollarAmount;
          str2 = num.ToString("N2");
        }
        txtAmount2.Text = str2;
        TextBox txtMinAmt = this.txtMinAmt;
        string str3;
        if (!(tag.MinDollarAmount != 0M))
        {
          str3 = "";
        }
        else
        {
          num = tag.MinDollarAmount;
          str3 = num.ToString("N2");
        }
        txtMinAmt.Text = str3;
        TextBox txtMaxAmt = this.txtMaxAmt;
        string str4;
        if (!(tag.MaxDollarAmount != 0M))
        {
          str4 = "";
        }
        else
        {
          num = tag.MaxDollarAmount;
          str4 = num.ToString("N2");
        }
        txtMaxAmt.Text = str4;
      }
      this.btnSelect.Enabled = this.gridViewPlans.SelectedItems.Count == 1 && this.datePickerStartDate.Value != DateTime.MinValue;
    }

    private void changeFieldStatus(bool readOnly)
    {
      if (readOnly)
      {
        this.txtName.Text = this.txtMinTerm.Text = this.txtAmount.Text = this.txtAmount2.Text = this.txtEarliestChangeDate.Text = this.txtMaxAmt.Text = this.txtMinAmt.Text = "";
        this.datePickerStartDate.Text = string.Empty;
      }
      this.datePickerStartDate.ReadOnly = readOnly;
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.datePickerStartDate.Text == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The Start Date cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (Utils.ParseDate((object) this.datePickerStartDate.Text) < this.session.SessionObjects.Session.ServerTime.Date)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The Start Date cannot be earlier than today's date.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        LoanCompPlan tag = (LoanCompPlan) this.gridViewPlans.SelectedItems[0].Tag;
        this.newLoanCompHistory = new LoanCompHistory(this.currentBranchOrUserID, tag.Name, tag.Id, Utils.ParseDate((object) this.datePickerStartDate.Text), Utils.ParseDate((object) this.datePickerStartDate.Text));
        this.newLoanCompHistory.NewRecord = true;
        this.newLoanCompHistory.MinTermDays = tag.MinTermDays;
        this.newLoanCompHistory.PercentAmt = tag.PercentAmt;
        this.newLoanCompHistory.DollarAmount = tag.DollarAmount;
        this.newLoanCompHistory.MinDollarAmount = tag.MinDollarAmount;
        this.newLoanCompHistory.MaxDollarAmount = tag.MaxDollarAmount;
        this.newLoanCompHistory.PercentAmtBase = tag.PercentAmtBase;
        this.newLoanCompHistory.RoundingMethod = tag.RoundingMethod;
        if (this.loanCompHistoryList != null && this.loanCompHistoryList.Count > 0)
        {
          if (!this.loanCompHistoryList.IsNewPlanStartDateValid(this.newLoanCompHistory))
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The Start Date is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.datePickerStartDate.Focus();
            return;
          }
          if (!this.loanCompHistoryList.IsNewPlanStartDateValidWithMinimumTermDays(this.newLoanCompHistory, this.newLoanCompHistory.StartDate.Date) && Utils.Dialog((IWin32Window) this, "The Start Date entered is prior to the Earliest Change Date for the previous plan. Would you like to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
          {
            this.datePickerStartDate.Focus();
            return;
          }
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    private void datePickerStartDate_ValueChanged(object sender, EventArgs e)
    {
      this.btnSelect.Enabled = this.datePickerStartDate.Text != string.Empty && Utils.ParseDate((object) this.datePickerStartDate.Text) != DateTime.MinValue && this.gridViewPlans.SelectedItems.Count == 1;
      if (this.gridViewPlans.SelectedItems.Count == 0)
        return;
      LoanCompPlan tag = (LoanCompPlan) this.gridViewPlans.SelectedItems[0].Tag;
      TextBox earliestChangeDate = this.txtEarliestChangeDate;
      DateTime dateTime = this.datePickerStartDate.Value;
      dateTime = dateTime.Date;
      dateTime = dateTime.AddDays((double) tag.MinTermDays);
      string str = dateTime.ToString("MM/dd/yyyy");
      earliestChangeDate.Text = str;
    }

    public LoanCompHistory NewLoanCompHistory => this.newLoanCompHistory;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.btnCancel = new Button();
      this.btnSelect = new Button();
      this.panelAll = new Panel();
      this.panelPlanDetails = new Panel();
      this.grpDetails = new GroupContainer();
      this.datePickerStartDate = new DatePicker();
      this.txtEarliestChangeDate = new TextBox();
      this.label2 = new Label();
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
      this.lblActivation = new Label();
      this.lblName = new Label();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelPlanList = new Panel();
      this.grpPlans = new GroupContainer();
      this.gridViewPlans = new GridView();
      this.panelTitle = new Panel();
      this.label1 = new Label();
      this.panelAll.SuspendLayout();
      this.panelPlanDetails.SuspendLayout();
      this.grpDetails.SuspendLayout();
      this.panelPlanList.SuspendLayout();
      this.grpPlans.SuspendLayout();
      this.panelTitle.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(452, 548);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 0;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnSelect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnSelect.Location = new Point(371, 548);
      this.btnSelect.Name = "btnSelect";
      this.btnSelect.Size = new Size(75, 23);
      this.btnSelect.TabIndex = 1;
      this.btnSelect.Text = "&Select";
      this.btnSelect.UseVisualStyleBackColor = true;
      this.btnSelect.Click += new EventHandler(this.btnSelect_Click);
      this.panelAll.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panelAll.Controls.Add((Control) this.panelPlanDetails);
      this.panelAll.Controls.Add((Control) this.collapsibleSplitter1);
      this.panelAll.Controls.Add((Control) this.panelPlanList);
      this.panelAll.Controls.Add((Control) this.panelTitle);
      this.panelAll.Location = new Point(5, 8);
      this.panelAll.Name = "panelAll";
      this.panelAll.Size = new Size(530, 533);
      this.panelAll.TabIndex = 2;
      this.panelPlanDetails.Controls.Add((Control) this.grpDetails);
      this.panelPlanDetails.Dock = DockStyle.Fill;
      this.panelPlanDetails.Location = new Point(0, 303);
      this.panelPlanDetails.Name = "panelPlanDetails";
      this.panelPlanDetails.Size = new Size(530, 230);
      this.panelPlanDetails.TabIndex = 3;
      this.grpDetails.Controls.Add((Control) this.datePickerStartDate);
      this.grpDetails.Controls.Add((Control) this.txtEarliestChangeDate);
      this.grpDetails.Controls.Add((Control) this.label2);
      this.grpDetails.Controls.Add((Control) this.txtMaxAmt);
      this.grpDetails.Controls.Add((Control) this.lblMaxAmount);
      this.grpDetails.Controls.Add((Control) this.txtMinAmt);
      this.grpDetails.Controls.Add((Control) this.lblMinAmount);
      this.grpDetails.Controls.Add((Control) this.txtAmount2);
      this.grpDetails.Controls.Add((Control) this.lblAmount2);
      this.grpDetails.Controls.Add((Control) this.txtAmount);
      this.grpDetails.Controls.Add((Control) this.txtMinTerm);
      this.grpDetails.Controls.Add((Control) this.txtName);
      this.grpDetails.Controls.Add((Control) this.lblAmount);
      this.grpDetails.Controls.Add((Control) this.lblMinTerm);
      this.grpDetails.Controls.Add((Control) this.lblActivation);
      this.grpDetails.Controls.Add((Control) this.lblName);
      this.grpDetails.Dock = DockStyle.Fill;
      this.grpDetails.HeaderForeColor = SystemColors.ControlText;
      this.grpDetails.Location = new Point(0, 0);
      this.grpDetails.Name = "grpDetails";
      this.grpDetails.Size = new Size(530, 230);
      this.grpDetails.TabIndex = 1;
      this.grpDetails.Text = "Details";
      this.datePickerStartDate.BackColor = SystemColors.Window;
      this.datePickerStartDate.Location = new Point(131, 57);
      this.datePickerStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.datePickerStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.datePickerStartDate.Name = "datePickerStartDate";
      this.datePickerStartDate.Size = new Size(100, 21);
      this.datePickerStartDate.TabIndex = 48;
      this.datePickerStartDate.ToolTip = "";
      this.datePickerStartDate.Value = new DateTime(0L);
      this.datePickerStartDate.ValueChanged += new EventHandler(this.datePickerStartDate_ValueChanged);
      this.txtEarliestChangeDate.Enabled = false;
      this.txtEarliestChangeDate.Location = new Point(131, 105);
      this.txtEarliestChangeDate.Name = "txtEarliestChangeDate";
      this.txtEarliestChangeDate.ReadOnly = true;
      this.txtEarliestChangeDate.Size = new Size(100, 20);
      this.txtEarliestChangeDate.TabIndex = 47;
      this.txtEarliestChangeDate.TabStop = false;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 108);
      this.label2.Name = "label2";
      this.label2.Size = new Size(107, 13);
      this.label2.TabIndex = 46;
      this.label2.Text = "Earliest Change Date";
      this.txtMaxAmt.Location = new Point(131, 201);
      this.txtMaxAmt.MaxLength = 15;
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.ReadOnly = true;
      this.txtMaxAmt.Size = new Size(100, 20);
      this.txtMaxAmt.TabIndex = 42;
      this.txtMaxAmt.TabStop = false;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.lblMaxAmount.AutoSize = true;
      this.lblMaxAmount.Location = new Point(11, 204);
      this.lblMaxAmount.Name = "lblMaxAmount";
      this.lblMaxAmount.Size = new Size(60, 13);
      this.lblMaxAmount.TabIndex = 45;
      this.lblMaxAmount.Text = "Maximum $";
      this.txtMinAmt.Location = new Point(131, 177);
      this.txtMinAmt.MaxLength = 15;
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.ReadOnly = true;
      this.txtMinAmt.Size = new Size(100, 20);
      this.txtMinAmt.TabIndex = 41;
      this.txtMinAmt.TabStop = false;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.lblMinAmount.AutoSize = true;
      this.lblMinAmount.Location = new Point(11, 180);
      this.lblMinAmount.Name = "lblMinAmount";
      this.lblMinAmount.Size = new Size(57, 13);
      this.lblMinAmount.TabIndex = 44;
      this.lblMinAmount.Text = "Minimum $";
      this.txtAmount2.Location = new Point(131, 153);
      this.txtAmount2.MaxLength = 15;
      this.txtAmount2.Name = "txtAmount2";
      this.txtAmount2.ReadOnly = true;
      this.txtAmount2.Size = new Size(100, 20);
      this.txtAmount2.TabIndex = 40;
      this.txtAmount2.TabStop = false;
      this.txtAmount2.TextAlign = HorizontalAlignment.Right;
      this.lblAmount2.AutoSize = true;
      this.lblAmount2.Location = new Point(11, 156);
      this.lblAmount2.Name = "lblAmount2";
      this.lblAmount2.Size = new Size(52, 13);
      this.lblAmount2.TabIndex = 43;
      this.lblAmount2.Text = "$ Amount";
      this.txtAmount.Location = new Point(131, 129);
      this.txtAmount.MaxLength = 15;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.ReadOnly = true;
      this.txtAmount.Size = new Size(100, 20);
      this.txtAmount.TabIndex = 36;
      this.txtAmount.TabStop = false;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMinTerm.Location = new Point(131, 81);
      this.txtMinTerm.MaxLength = 5;
      this.txtMinTerm.Name = "txtMinTerm";
      this.txtMinTerm.ReadOnly = true;
      this.txtMinTerm.Size = new Size(100, 20);
      this.txtMinTerm.TabIndex = 30;
      this.txtMinTerm.TabStop = false;
      this.txtMinTerm.TextAlign = HorizontalAlignment.Right;
      this.txtName.Location = new Point(131, 33);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new Size(391, 20);
      this.txtName.TabIndex = 25;
      this.txtName.TabStop = false;
      this.lblAmount.AutoSize = true;
      this.lblAmount.Location = new Point(11, 132);
      this.lblAmount.Name = "lblAmount";
      this.lblAmount.Size = new Size(54, 13);
      this.lblAmount.TabIndex = 37;
      this.lblAmount.Text = "% Amount";
      this.lblMinTerm.AutoSize = true;
      this.lblMinTerm.Location = new Point(11, 84);
      this.lblMinTerm.Name = "lblMinTerm";
      this.lblMinTerm.Size = new Size(112, 13);
      this.lblMinTerm.TabIndex = 35;
      this.lblMinTerm.Text = "Minimum Term # Days";
      this.lblActivation.AutoSize = true;
      this.lblActivation.Location = new Point(11, 60);
      this.lblActivation.Name = "lblActivation";
      this.lblActivation.Size = new Size(55, 13);
      this.lblActivation.TabIndex = 32;
      this.lblActivation.Text = "Start Date";
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(11, 36);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(35, 13);
      this.lblName.TabIndex = 26;
      this.lblName.Text = "Name";
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panelPlanDetails;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 296);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 2;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelPlanList.Controls.Add((Control) this.grpPlans);
      this.panelPlanList.Dock = DockStyle.Top;
      this.panelPlanList.Location = new Point(0, 24);
      this.panelPlanList.Name = "panelPlanList";
      this.panelPlanList.Size = new Size(530, 272);
      this.panelPlanList.TabIndex = 1;
      this.grpPlans.Controls.Add((Control) this.gridViewPlans);
      this.grpPlans.Dock = DockStyle.Fill;
      this.grpPlans.HeaderForeColor = SystemColors.ControlText;
      this.grpPlans.Location = new Point(0, 0);
      this.grpPlans.Name = "grpPlans";
      this.grpPlans.Size = new Size(530, 272);
      this.grpPlans.TabIndex = 0;
      this.grpPlans.Text = "Plans";
      this.gridViewPlans.AllowMultiselect = false;
      this.gridViewPlans.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 420;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SpringToFit = true;
      gvColumn2.Text = "Activation Date";
      gvColumn2.TextAlignment = ContentAlignment.MiddleCenter;
      gvColumn2.Width = 108;
      this.gridViewPlans.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gridViewPlans.Dock = DockStyle.Fill;
      this.gridViewPlans.Location = new Point(1, 26);
      this.gridViewPlans.Name = "gridViewPlans";
      this.gridViewPlans.Size = new Size(528, 245);
      this.gridViewPlans.TabIndex = 0;
      this.gridViewPlans.SelectedIndexChanged += new EventHandler(this.gridViewPlans_SelectedIndexChanged);
      this.panelTitle.Controls.Add((Control) this.label1);
      this.panelTitle.Dock = DockStyle.Top;
      this.panelTitle.Location = new Point(0, 0);
      this.panelTitle.Name = "panelTitle";
      this.panelTitle.Size = new Size(530, 24);
      this.panelTitle.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(8, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(333, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Select an LO compensation plan to assign to the company or branch.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(539, 583);
      this.Controls.Add((Control) this.panelAll);
      this.Controls.Add((Control) this.btnSelect);
      this.Controls.Add((Control) this.btnCancel);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AssignLOCompDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "LO Compensation Plans";
      this.panelAll.ResumeLayout(false);
      this.panelPlanDetails.ResumeLayout(false);
      this.grpDetails.ResumeLayout(false);
      this.grpDetails.PerformLayout();
      this.panelPlanList.ResumeLayout(false);
      this.grpPlans.ResumeLayout(false);
      this.panelTitle.ResumeLayout(false);
      this.panelTitle.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
