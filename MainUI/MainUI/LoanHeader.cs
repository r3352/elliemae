// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoanHeader
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LoanHeader : UserControl, IRefreshContents
  {
    private static readonly string[] fieldsToMonitor = new string[19]
    {
      "2",
      "3",
      "11",
      "12",
      "14",
      "15",
      "353",
      "364",
      "420",
      "740",
      "742",
      "762",
      "763",
      "976",
      "1540",
      "4527",
      "4528",
      "4529",
      "5016"
    };
    private Panel[] headerPanels;
    private LoanDataMgr loanDataMgr;
    private Routine loanDataChangeRoutine;
    private AlertConfig rateLockAlertConfig;
    private IContainer components;
    private GradientPanel gradientPanel1;
    private PictureBox pbLienPosition;
    private Panel pnlAddress;
    private Panel pnlLoanAmt;
    private Label lblLoanNumber;
    private Label label1;
    private Label lblLoanAmount;
    private Label label4;
    private Panel pnlLTV;
    private Label label7;
    private Label lblLTV;
    private Label label6;
    private Label lblCityStateZip;
    private Label lblStreet;
    private Label lblDTI;
    private Panel pnlRate;
    private Label lblRate;
    private Label label8;
    private FormattedLabel lblLockInfo;
    private Panel pnlClosing;
    private Label lblClosingDate;
    private Label lblClosingDateCaption;
    private ComboBoxEx cboAssociates;
    private ElementControl elmProperty;
    private ElementControl elmRateLock;
    private ElementControl elmLoanAssociate;
    private ToolTip toolTip1;
    private ElementControl elmLinkedLoan;
    private Panel pnlarchive;
    private CheckBox chkarchived;

    private LoanData loanData => this.loanDataMgr?.LoanData;

    public LoanHeader()
    {
      this.InitializeComponent();
      this.headerPanels = new Panel[6]
      {
        this.pnlAddress,
        this.pnlLoanAmt,
        this.pnlLTV,
        this.pnlRate,
        this.pnlClosing,
        this.pnlarchive
      };
      this.loanDataChangeRoutine = new Routine(this.onLoanDataChanged);
    }

    public void AttachToLoan(LoanDataMgr loanMgr)
    {
      if (this.loanData != null)
        this.unsubscribeToEvents();
      this.loanDataMgr = loanMgr;
      this.subscribeToEvents();
      foreach (AlertConfig alertConfig in loanMgr.SystemConfiguration.AlertSetupData.AlertConfigList)
      {
        if (alertConfig.AlertID == 10)
        {
          this.rateLockAlertConfig = alertConfig;
          break;
        }
      }
      this.elmLinkedLoan.Element = (object) new LinkedGUIDLink((Control) this, this.loanData, this.loanDataMgr);
      this.RefreshContents();
    }

    public void ApplyPRBRRules()
    {
      if (this.loanDataMgr == null)
        return;
      BizRule.FieldAccessRight fieldAccessRights = this.loanDataMgr.GetFieldAccessRights("5016");
      if (fieldAccessRights == BizRule.FieldAccessRight.Hide)
        this.chkarchived.Visible = false;
      else
        this.chkarchived.Enabled = fieldAccessRights != BizRule.FieldAccessRight.ViewOnly;
    }

    public void ReleaseLoan()
    {
      if (this.loanData != null)
        this.unsubscribeToEvents();
      this.loanDataMgr = (LoanDataMgr) null;
    }

    public void RefreshContents()
    {
      this.elmProperty.Element = (object) new PropertyLink((Control) this);
      this.lblStreet.Text = this.loanData.GetField("11");
      this.lblCityStateZip.Text = this.getCityStateZipText();
      this.pbLienPosition.Image = this.loanData.GetField("420") == "SecondLien" ? (Image) Resources.loan_position_2 : (Image) Resources.loan_position_1;
      this.lblLoanNumber.Text = this.loanData.GetField("364");
      string field1 = this.loanData.GetField("2");
      this.lblLoanAmount.Text = field1 == "" ? "" : "$" + field1;
      this.elmLinkedLoan.Visible = this.loanData.LinkGUID != "";
      this.lblLTV.Text = this.loanData.GetField("353") + "/" + this.loanData.GetField("976") + "/" + this.loanData.GetField("1540");
      this.lblDTI.Text = this.loanData.GetField("740") + "/" + this.loanData.GetField("742");
      string field2 = this.loanData.GetField("3");
      this.lblRate.Text = field2 == "" ? "" : field2 + "%";
      this.elmRateLock.Element = (object) new RateLockLink((Control) this);
      this.chkarchived.Checked = this.loanData.GetField("5016").ToLower() == "y";
      this.populateRateLockInfo();
      this.populateClosingDateInfo();
      this.populateLoanAssociates();
    }

    public void RefreshLoanContents() => this.RefreshContents();

    private void populateClosingDateInfo()
    {
      LogList logList = this.loanData.GetLogList();
      if (logList.GetMilestone("Completion").Done)
      {
        this.lblClosingDateCaption.Text = "Date Completed:";
        this.lblClosingDate.Text = logList.GetMilestone("Completion").Date.ToShortDateString();
      }
      else
      {
        this.lblClosingDateCaption.Text = "Est Closing Date:";
        this.lblClosingDate.Text = this.loanData.GetField("763");
      }
    }

    private void populateLoanAssociates()
    {
      RoleInfo[] allRoles = this.loanDataMgr.SystemConfiguration.AllRoles;
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (RoleInfo roleInfo in allRoles)
        insensitiveHashtable[(object) roleInfo.Name] = (object) roleInfo.RoleAbbr;
      string.Concat(this.cboAssociates.SelectedItem);
      this.cboAssociates.Items.Clear();
      foreach (LoanAssociateLog allLoanAssociate in this.loanData.GetLogList().GetAllLoanAssociates())
      {
        if ((allLoanAssociate.LoanAssociateID ?? "") != "")
        {
          string abbrev = string.Concat(insensitiveHashtable[(object) allLoanAssociate.RoleName]);
          if (allLoanAssociate.RoleID == RoleInfo.FileStarter.ID)
            abbrev = RoleInfo.FileStarter.RoleAbbr;
          if (abbrev != "")
            this.cboAssociates.Items.Add((object) new LoanHeader.LoanAssociateLogListItem(abbrev, allLoanAssociate));
        }
      }
      if (this.cboAssociates.Items.Count > 0)
        this.cboAssociates.SelectedIndex = 0;
      else
        this.elmLoanAssociate.Element = (object) new ImageElement((Image) Resources.business_contact_disabled);
    }

    private void onLoanDataChanged(string id, string val) => this.RefreshContents();

    private string getCityStateZipText()
    {
      string field1 = this.loanData.GetField("12");
      string field2 = this.loanData.GetField("14");
      string field3 = this.loanData.GetField("15");
      string cityStateZipText = !(field1 != "") || !(field2 != "") ? field1 + field2 : field1 + ", " + field2;
      if (field3 != "")
        cityStateZipText = cityStateZipText + " " + field3;
      return cityStateZipText;
    }

    private void populateRateLockInfo()
    {
      LockConfirmLog confirmForCurrentLock = this.loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
      bool flag = false;
      if (confirmForCurrentLock != null)
        flag = confirmForCurrentLock.CommitmentTermEnabled;
      DateTime dateTime = string.Equals(this.loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) & flag ? Utils.ParseDate((object) this.loanData.GetField("4529")) : Utils.ParseDate((object) this.loanData.GetField("762"));
      string field1 = this.loanData.GetField("LOCKRATE.RATESTATUS");
      string field2 = this.loanData.GetField("LOCKRATE.REQUESTED");
      string field3 = this.loanData.GetField("LOCKRATE.EXTENSIONREQUESTPENDING");
      int lockAlertDaysBefore = this.getRateLockAlertDaysBefore();
      this.loanData.GetField("LOCKRATE.RATEREQUESTSTATUS");
      string str1 = ControlDraw.ColorToString(EncompassColors.Primary4);
      string str2 = ControlDraw.ColorToString(EncompassColors.Alert2);
      string caption;
      if (field1 == "Cancelled")
        caption = "<C Value=\"Black\">Lock Cancelled</C>";
      else if (field1 != "NotLocked" && field1 != "Voided")
      {
        int totalDays = (int) (dateTime - DateTime.Today).TotalDays;
        if (dateTime == DateTime.MinValue)
          caption = !(field3 != "Y") ? "<C value=\"" + str1 + "\">Locked, Extension Requested</C>" : "<C value=\"" + str1 + "\">Rate is locked</C>";
        else if (totalDays == 0)
          caption = "<C value=\"" + (totalDays <= lockAlertDaysBefore ? str2 : str1) + "\"><B>Expires today!</B></C>";
        else if (totalDays > 0)
        {
          string str3 = totalDays <= lockAlertDaysBefore ? str2 : str1;
          if (totalDays == 1)
            caption = "<C value=\"" + str3 + "\"><B>1 day</B> remaining</C>";
          else
            caption = "<C value=\"" + str3 + "\"><B>" + totalDays.ToString() + " days</B> remaining</C>";
        }
        else
          caption = !(field3 == "Y") ? "<C value=\"" + str2 + "\"><B>Expired!</B></C>" : "<C value=\"" + str2 + "\"><B>Expired! Extension Requested</B></C>";
      }
      else
        caption = !(field2 == "Y") ? "<C Value=\"Black\">Not Locked</C>" : "<B>Lock Requested</B>";
      this.lblLockInfo.Text = caption;
      this.toolTip1.SetToolTip((Control) this.lblLockInfo, caption);
    }

    private int getRateLockAlertDaysBefore()
    {
      if (this.rateLockAlertConfig == null || !this.rateLockAlertConfig.AlertEnabled)
        return -1;
      MilestoneLog completedMilestone = this.loanData.GetLogList().GetLastCompletedMilestone();
      return completedMilestone == null || !this.rateLockAlertConfig.MilestoneGuidList.Contains(completedMilestone.MilestoneID) ? -1 : this.rateLockAlertConfig.DaysBefore;
    }

    private void subscribeToEvents()
    {
      foreach (string id in LoanHeader.fieldsToMonitor)
        this.loanData.RegisterCustomFieldValueChangeEventHandler(id, this.loanDataChangeRoutine);
    }

    private void unsubscribeToEvents()
    {
      foreach (string id in LoanHeader.fieldsToMonitor)
        this.loanData.UnregisterCustomFieldValueChangeEventHandler(id, this.loanDataChangeRoutine);
    }

    public void ResubscribeToEvents()
    {
      this.unsubscribeToEvents();
      this.subscribeToEvents();
    }

    private void LoanHeader_Resize(object sender, EventArgs e)
    {
      Decimal[] numArray = new Decimal[6]
      {
        0.24M,
        0.21M,
        0.165M,
        0.185M,
        0.2M,
        0.24M
      };
      if (this.headerPanels == null)
        return;
      for (int index = 0; index < this.headerPanels.Length; ++index)
      {
        Panel headerPanel = this.headerPanels[index];
        int num = (int) Math.Min(numArray[index] * (Decimal) this.ClientRectangle.Width, (Decimal) headerPanel.MaximumSize.Width);
        headerPanel.Width = num;
        headerPanel.Left = index == 0 ? 0 : this.headerPanels[index - 1].Right + 1;
      }
    }

    private void cboAssociates_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboAssociates.SelectedIndex < 0)
        return;
      LoanHeader.LoanAssociateLogListItem selectedItem = (LoanHeader.LoanAssociateLogListItem) this.cboAssociates.SelectedItem;
      this.elmLoanAssociate.Element = (object) new UserLink((Control) this, selectedItem.LogItem.LoanAssociateID, selectedItem.LogItem.LoanAssociateName);
    }

    private void chkarchived_CheckedChanged(object sender, EventArgs e)
    {
      this.loanData.SetField("5016", this.chkarchived.Checked ? "Y" : "N");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanHeader));
      this.gradientPanel1 = new GradientPanel();
      this.pnlarchive = new Panel();
      this.chkarchived = new CheckBox();
      this.pnlClosing = new Panel();
      this.cboAssociates = new ComboBoxEx();
      this.lblClosingDate = new Label();
      this.lblClosingDateCaption = new Label();
      this.elmLoanAssociate = new ElementControl();
      this.pnlRate = new Panel();
      this.lblLockInfo = new FormattedLabel();
      this.lblRate = new Label();
      this.label8 = new Label();
      this.elmRateLock = new ElementControl();
      this.pnlLTV = new Panel();
      this.lblDTI = new Label();
      this.label7 = new Label();
      this.lblLTV = new Label();
      this.label6 = new Label();
      this.pnlLoanAmt = new Panel();
      this.elmLinkedLoan = new ElementControl();
      this.lblLoanAmount = new Label();
      this.label4 = new Label();
      this.lblLoanNumber = new Label();
      this.label1 = new Label();
      this.pbLienPosition = new PictureBox();
      this.pnlAddress = new Panel();
      this.lblCityStateZip = new Label();
      this.lblStreet = new Label();
      this.elmProperty = new ElementControl();
      this.toolTip1 = new ToolTip(this.components);
      this.gradientPanel1.SuspendLayout();
      this.pnlarchive.SuspendLayout();
      this.pnlClosing.SuspendLayout();
      this.pnlRate.SuspendLayout();
      this.pnlLTV.SuspendLayout();
      this.pnlLoanAmt.SuspendLayout();
      ((ISupportInitialize) this.pbLienPosition).BeginInit();
      this.pnlAddress.SuspendLayout();
      this.SuspendLayout();
      this.gradientPanel1.Controls.Add((Control) this.pnlarchive);
      this.gradientPanel1.Controls.Add((Control) this.pnlClosing);
      this.gradientPanel1.Controls.Add((Control) this.pnlRate);
      this.gradientPanel1.Controls.Add((Control) this.pnlLTV);
      this.gradientPanel1.Controls.Add((Control) this.pnlLoanAmt);
      this.gradientPanel1.Controls.Add((Control) this.pnlAddress);
      this.gradientPanel1.Dock = DockStyle.Fill;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1104, 52);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.LoanHeader;
      this.gradientPanel1.TabIndex = 0;
      this.pnlarchive.BackColor = Color.Transparent;
      this.pnlarchive.Controls.Add((Control) this.chkarchived);
      this.pnlarchive.Location = new Point(1003, 0);
      this.pnlarchive.MaximumSize = new Size(199, 52);
      this.pnlarchive.Name = "pnlarchive";
      this.pnlarchive.Size = new Size(160, 52);
      this.pnlarchive.TabIndex = 11;
      this.chkarchived.AutoSize = true;
      this.chkarchived.Location = new Point(7, 24);
      this.chkarchived.Name = "chkarchived";
      this.chkarchived.Size = new Size(161, 36);
      this.chkarchived.TabIndex = 12;
      this.chkarchived.Text = "Archived";
      this.chkarchived.UseVisualStyleBackColor = true;
      this.chkarchived.CheckedChanged += new EventHandler(this.chkarchived_CheckedChanged);
      this.pnlClosing.BackColor = Color.Transparent;
      this.pnlClosing.Controls.Add((Control) this.cboAssociates);
      this.pnlClosing.Controls.Add((Control) this.lblClosingDate);
      this.pnlClosing.Controls.Add((Control) this.lblClosingDateCaption);
      this.pnlClosing.Controls.Add((Control) this.elmLoanAssociate);
      this.pnlClosing.Location = new Point(805, 0);
      this.pnlClosing.MaximumSize = new Size(199, 52);
      this.pnlClosing.Name = "pnlClosing";
      this.pnlClosing.Size = new Size(199, 52);
      this.pnlClosing.TabIndex = 10;
      this.cboAssociates.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboAssociates.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAssociates.FormattingEnabled = true;
      this.cboAssociates.Location = new Point(25, 26);
      this.cboAssociates.Name = "cboAssociates";
      this.cboAssociates.SelectedBGColor = SystemColors.Highlight;
      this.cboAssociates.Size = new Size(167, 40);
      this.cboAssociates.TabIndex = 12;
      this.cboAssociates.SelectedIndexChanged += new EventHandler(this.cboAssociates_SelectedIndexChanged);
      this.lblClosingDate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblClosingDate.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblClosingDate.Location = new Point(93, 8);
      this.lblClosingDate.Name = "lblClosingDate";
      this.lblClosingDate.Size = new Size(98, 17);
      this.lblClosingDate.TabIndex = 11;
      this.lblClosingDate.Text = "<Closing Date>";
      this.lblClosingDate.TextAlign = ContentAlignment.MiddleLeft;
      this.lblClosingDateCaption.Location = new Point(4, 8);
      this.lblClosingDateCaption.Name = "lblClosingDateCaption";
      this.lblClosingDateCaption.Size = new Size(94, 17);
      this.lblClosingDateCaption.TabIndex = 10;
      this.lblClosingDateCaption.Text = "Est. Closing Date:";
      this.lblClosingDateCaption.TextAlign = ContentAlignment.MiddleLeft;
      this.elmLoanAssociate.Element = (object) null;
      this.elmLoanAssociate.Location = new Point(4, 28);
      this.elmLoanAssociate.Name = "elmLoanAssociate";
      this.elmLoanAssociate.Size = new Size(16, 16);
      this.elmLoanAssociate.TabIndex = 16;
      this.pnlRate.BackColor = Color.Transparent;
      this.pnlRate.Controls.Add((Control) this.lblLockInfo);
      this.pnlRate.Controls.Add((Control) this.lblRate);
      this.pnlRate.Controls.Add((Control) this.label8);
      this.pnlRate.Controls.Add((Control) this.elmRateLock);
      this.pnlRate.Location = new Point(617, 0);
      this.pnlRate.MaximumSize = new Size(188, 52);
      this.pnlRate.Name = "pnlRate";
      this.pnlRate.Size = new Size(188, 52);
      this.pnlRate.TabIndex = 9;
      this.lblLockInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblLockInfo.AutoSize = false;
      this.lblLockInfo.ForeColor = Color.FromArgb(29, 110, 174);
      this.lblLockInfo.Location = new Point(24, 27);
      this.lblLockInfo.Name = "lblLockInfo";
      this.lblLockInfo.Size = new Size(163, 17);
      this.lblLockInfo.TabIndex = 13;
      this.lblLockInfo.Text = "<Lock Info>";
      this.lblRate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblRate.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblRate.Location = new Point(33, 8);
      this.lblRate.Name = "lblRate";
      this.lblRate.Size = new Size(150, 17);
      this.lblRate.TabIndex = 9;
      this.lblRate.Text = "<Note Rate>";
      this.lblRate.TextAlign = ContentAlignment.MiddleLeft;
      this.label8.Location = new Point(4, 8);
      this.label8.Name = "label8";
      this.label8.Size = new Size(38, 17);
      this.label8.TabIndex = 8;
      this.label8.Text = "Rate:";
      this.label8.TextAlign = ContentAlignment.MiddleLeft;
      this.elmRateLock.Element = (object) null;
      this.elmRateLock.Location = new Point(5, 27);
      this.elmRateLock.Name = "elmRateLock";
      this.elmRateLock.Size = new Size(16, 16);
      this.elmRateLock.TabIndex = 15;
      this.elmRateLock.Text = "elementControl1";
      this.pnlLTV.BackColor = Color.Transparent;
      this.pnlLTV.Controls.Add((Control) this.lblDTI);
      this.pnlLTV.Controls.Add((Control) this.label7);
      this.pnlLTV.Controls.Add((Control) this.lblLTV);
      this.pnlLTV.Controls.Add((Control) this.label6);
      this.pnlLTV.Location = new Point(450, 0);
      this.pnlLTV.MaximumSize = new Size(167, 52);
      this.pnlLTV.Name = "pnlLTV";
      this.pnlLTV.Size = new Size(167, 52);
      this.pnlLTV.TabIndex = 8;
      this.lblDTI.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblDTI.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDTI.Location = new Point(32, 27);
      this.lblDTI.Name = "lblDTI";
      this.lblDTI.Size = new Size(130, 17);
      this.lblDTI.TabIndex = 11;
      this.lblDTI.Text = "<DTI>";
      this.lblDTI.TextAlign = ContentAlignment.MiddleLeft;
      this.label7.Location = new Point(4, 27);
      this.label7.Name = "label7";
      this.label7.Size = new Size(30, 17);
      this.label7.TabIndex = 10;
      this.label7.Text = "DTI:";
      this.label7.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLTV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblLTV.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLTV.Location = new Point(32, 8);
      this.lblLTV.Name = "lblLTV";
      this.lblLTV.Size = new Size(130, 17);
      this.lblLTV.TabIndex = 9;
      this.lblLTV.Text = "<LTV>";
      this.lblLTV.TextAlign = ContentAlignment.MiddleLeft;
      this.label6.Location = new Point(4, 8);
      this.label6.Name = "label6";
      this.label6.Size = new Size(30, 17);
      this.label6.TabIndex = 8;
      this.label6.Text = "LTV:";
      this.label6.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlLoanAmt.BackColor = Color.Transparent;
      this.pnlLoanAmt.Controls.Add((Control) this.elmLinkedLoan);
      this.pnlLoanAmt.Controls.Add((Control) this.lblLoanAmount);
      this.pnlLoanAmt.Controls.Add((Control) this.label4);
      this.pnlLoanAmt.Controls.Add((Control) this.lblLoanNumber);
      this.pnlLoanAmt.Controls.Add((Control) this.label1);
      this.pnlLoanAmt.Controls.Add((Control) this.pbLienPosition);
      this.pnlLoanAmt.Location = new Point(240, 0);
      this.pnlLoanAmt.MaximumSize = new Size(210, 52);
      this.pnlLoanAmt.Name = "pnlLoanAmt";
      this.pnlLoanAmt.Size = new Size(210, 52);
      this.pnlLoanAmt.TabIndex = 7;
      this.elmLinkedLoan.Element = (object) null;
      this.elmLinkedLoan.Location = new Point(3, 8);
      this.elmLinkedLoan.Name = "elmLinkedLoan";
      this.elmLinkedLoan.Size = new Size(16, 16);
      this.elmLinkedLoan.TabIndex = 13;
      this.elmLinkedLoan.Text = "elementControl1";
      this.lblLoanAmount.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblLoanAmount.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLoanAmount.Location = new Point(116, 27);
      this.lblLoanAmount.Name = "lblLoanAmount";
      this.lblLoanAmount.Size = new Size(131, 17);
      this.lblLoanAmount.TabIndex = 9;
      this.lblLoanAmount.Text = "<Loan Amt>";
      this.lblLoanAmount.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.Location = new Point(45, 27);
      this.label4.Name = "label4";
      this.label4.Size = new Size(88, 17);
      this.label4.TabIndex = 8;
      this.label4.Text = "Loan Amount:";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLoanNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblLoanNumber.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblLoanNumber.Location = new Point(85, 8);
      this.lblLoanNumber.Name = "lblLoanNumber";
      this.lblLoanNumber.Size = new Size(138, 17);
      this.lblLoanNumber.TabIndex = 7;
      this.lblLoanNumber.Text = "<Loan Number>";
      this.lblLoanNumber.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.Location = new Point(45, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(48, 17);
      this.label1.TabIndex = 6;
      this.label1.Text = "Loan #:";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.pbLienPosition.BackColor = Color.Transparent;
      this.pbLienPosition.Image = (Image) componentResourceManager.GetObject("pbLienPosition.Image");
      this.pbLienPosition.Location = new Point(24, 8);
      this.pbLienPosition.Name = "pbLienPosition";
      this.pbLienPosition.Size = new Size(19, 16);
      this.pbLienPosition.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pbLienPosition.TabIndex = 3;
      this.pbLienPosition.TabStop = false;
      this.pnlAddress.BackColor = Color.Transparent;
      this.pnlAddress.Controls.Add((Control) this.lblCityStateZip);
      this.pnlAddress.Controls.Add((Control) this.lblStreet);
      this.pnlAddress.Controls.Add((Control) this.elmProperty);
      this.pnlAddress.Location = new Point(0, 0);
      this.pnlAddress.MaximumSize = new Size(240, 52);
      this.pnlAddress.Name = "pnlAddress";
      this.pnlAddress.Size = new Size(240, 52);
      this.pnlAddress.TabIndex = 6;
      this.lblCityStateZip.AutoEllipsis = true;
      this.lblCityStateZip.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCityStateZip.Location = new Point(27, 27);
      this.lblCityStateZip.Name = "lblCityStateZip";
      this.lblCityStateZip.Size = new Size(207, 17);
      this.lblCityStateZip.TabIndex = 10;
      this.lblCityStateZip.Text = "<City, State, ZIP>";
      this.lblCityStateZip.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStreet.AutoEllipsis = true;
      this.lblStreet.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblStreet.Location = new Point(27, 8);
      this.lblStreet.Name = "lblStreet";
      this.lblStreet.Size = new Size(207, 17);
      this.lblStreet.TabIndex = 9;
      this.lblStreet.Text = "<Street Address>";
      this.lblStreet.TextAlign = ContentAlignment.MiddleLeft;
      this.lblStreet.UseMnemonic = false;
      this.elmProperty.Element = (object) null;
      this.elmProperty.Location = new Point(8, 8);
      this.elmProperty.Name = "elmProperty";
      this.elmProperty.Size = new Size(16, 16);
      this.elmProperty.TabIndex = 12;
      this.elmProperty.Text = "elementControl1";
      this.AutoScaleDimensions = new SizeF(16f, 32f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (LoanHeader);
      this.Size = new Size(1104, 52);
      this.Resize += new EventHandler(this.LoanHeader_Resize);
      this.gradientPanel1.ResumeLayout(false);
      this.pnlarchive.ResumeLayout(false);
      this.pnlarchive.PerformLayout();
      this.pnlClosing.ResumeLayout(false);
      this.pnlRate.ResumeLayout(false);
      this.pnlLTV.ResumeLayout(false);
      this.pnlLoanAmt.ResumeLayout(false);
      this.pnlLoanAmt.PerformLayout();
      ((ISupportInitialize) this.pbLienPosition).EndInit();
      this.pnlAddress.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private class LoanAssociateLogListItem
    {
      public readonly string RoleAbbrev;
      public readonly LoanAssociateLog LogItem;

      public LoanAssociateLogListItem(string abbrev, LoanAssociateLog log)
      {
        this.RoleAbbrev = abbrev;
        this.LogItem = log;
      }

      public override string ToString() => this.RoleAbbrev + ": " + this.LogItem.LoanAssociateName;
    }
  }
}
