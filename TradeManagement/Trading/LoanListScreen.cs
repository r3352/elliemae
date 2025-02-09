// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanListScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.ReportFieldDefinitions;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.InputEngine.LockRequest;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class LoanListScreen : UserControl
  {
    private string className = nameof (LoanListScreen);
    private const int ControlPadding = 5;
    private static string sw = Tracing.SwOutsideLoan;
    private static string[] requiredEligibilityFields = new string[1]
    {
      "Loan.InvestorStatus"
    };
    private string standardViewName = "Standard View";
    public static Color AlertColor = Color.FromArgb(204, 51, 51);
    public static Color HighlightColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 231);
    public static Color AutoSelected = Color.FromArgb(0, 0, (int) byte.MaxValue);
    private ITradeEditor tradeEditor;
    private LoanReportFieldDefs fieldDefs;
    private TradeAssignedLoanFieldDefs tradeAssLoanFieldDefs;
    private GridViewLayoutManager eligibleLayoutMngr;
    private TableLayout tradeLoansTableLayout;
    private FileSystemEntry fsViewEntry;
    private GridViewLayoutManager gvLayoutManager;
    private TradeView currentView;
    private string[] eligibleSkipList;
    private bool withdrawnLoanMessageDisplayed;
    private bool readOnly;
    private bool showViewEligible = true;
    public bool ViewModified;
    private bool modified;
    private IContainer components;
    private Panel pnlLoans;
    private TableContainer grpLoans;
    private TradeProfitControl ctlProfit;
    private GridView gvLoans;
    private FlowLayoutPanel flpLoans;
    private Button btnMarkAsShipped;
    private ButtonEx btnUpdateLoans;
    private VerticalSeparator verticalSeparator2;
    private StandardIconButton btnExportLoans;
    private StandardIconButton btnRemoveLoans;
    private GradientPanel gradientPanel2;
    private StandardIconButton btnEditView;
    private StandardIconButton btnRefreshView;
    private StandardIconButton btnSaveView;
    private Label label22;
    private ComboBoxEx cBoxView;
    private Panel pnlEligibleLoans;
    private TableContainer tableContainer1;
    private TradeProfitControl ctlEligibleProfit;
    private FlowLayoutPanel flowLayoutPanel2;
    private Button btnViewFilter;
    private Button btnAddLoans;
    private GridView gvEligible;
    private CheckBox chkImport;
    private CheckBox chkViewEligible;
    private Button btnMarkAsPurchased;
    private ButtonEx btnUpdateSelectedLoans;
    private ButtonEx btnExtendSelectedLoan;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.gvLoans.SelectedItems.Clear();
        if (this.readOnly)
        {
          this.chkViewEligible.Checked = false;
          this.chkImport.Checked = false;
        }
        this.chkViewEligible.Visible = !this.readOnly && !this.tradeEditor.CurrentTradeInfo.Locked && this.showViewEligible;
        this.chkImport.Visible = !this.readOnly && !this.tradeEditor.CurrentTradeInfo.Locked;
        if (!(this.tradeEditor is CorrespondentTradeEditor))
          this.btnMarkAsShipped.Enabled = this.btnMarkAsPurchased.Enabled = !this.readOnly && this.gvLoans.SelectedItems.Count > 0;
        this.btnRemoveLoans.Visible = !this.readOnly;
        if (this.tradeEditor is CorrespondentTradeEditor)
        {
          this.SetCorrespondentUpdateButtonAttributes();
        }
        else
        {
          this.btnUpdateLoans.Enabled = !this.readOnly;
          this.btnUpdateSelectedLoans.Enabled = !this.readOnly && this.gvLoans.SelectedItems.Count > 0;
          this.btnExtendSelectedLoan.Enabled = !this.readOnly && this.gvLoans.SelectedItems.Count > 0;
        }
      }
    }

    public bool DataModified
    {
      get => !this.readOnly && this.modified;
      set => this.modified = value;
    }

    public int CurrentInTradeCount => this.gvLoans.Items.Count;

    public Decimal GetAssignedAmount => this.ctlProfit.GetAssignedAmount();

    public Dictionary<string, GVItem> LoansToAssign { get; set; }

    public LoanListScreen() => this.InitializeComponent();

    public LoanListScreen(ITradeEditor tradeEditor)
      : this()
    {
      this.tradeEditor = tradeEditor;
      this.btnExtendSelectedLoan.Enabled = false;
      this.pnlEligibleLoans.Height = 0;
      this.resetFieldDefs();
      this.eligibleLayoutMngr = new GridViewLayoutManager(this.gvEligible, this.getFullEligibleTableLayout(), this.getDefaultEligibleTableLayout());
      this.eligibleLayoutMngr.LayoutChanged += new EventHandler(this.eligibleLayoutMngr_LayoutChanged);
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeAssignedLoanView, FileSystemEntry.PrivateRoot(Session.UserID)));
      string privateProfileString = Session.GetPrivateProfileString("TradeAssignedLoans", "DefaultView");
      this.eligibleSkipList = new string[2]
      {
        "TradeAssignment.Status",
        "Trade.Name"
      };
      if (privateProfileString != "" && privateProfileString != this.standardViewName)
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString)), false);
      if (this.tradeEditor is MbsPoolEditor || this.tradeEditor is FannieMaePEPoolEditor)
      {
        this.grpLoans.Text = "Currently in MBS Pool";
        this.flpLoans.Controls.Remove((Control) this.btnExtendSelectedLoan);
      }
      else if (this.tradeEditor is TradeEditor)
      {
        this.grpLoans.Text = "Currently in Trade";
        this.flpLoans.Controls.Remove((Control) this.btnExtendSelectedLoan);
      }
      else if (this.tradeEditor is CorrespondentTradeEditor)
      {
        this.grpLoans.Text = "Currently in Correspondent Trade";
        this.btnMarkAsShipped.Visible = false;
        this.btnMarkAsPurchased.Visible = false;
      }
      else if (this.tradeEditor is GseCommitmentEditor)
      {
        this.grpLoans.Text = "Currently in GSE Commitment";
        this.grpLoans.Controls.Remove((Control) this.chkViewEligible);
        this.grpLoans.Controls.Remove((Control) this.chkImport);
        this.flpLoans.Controls.Remove((Control) this.btnRemoveLoans);
        this.flpLoans.Controls.Remove((Control) this.btnUpdateLoans);
        this.flpLoans.Controls.Remove((Control) this.btnMarkAsShipped);
        this.flpLoans.Controls.Remove((Control) this.btnMarkAsPurchased);
        this.flpLoans.Controls.Remove((Control) this.verticalSeparator2);
        this.flpLoans.Controls.Remove((Control) this.btnExtendSelectedLoan);
      }
      this.tradeEditor.ToolTip.SetToolTip((Control) this.btnEditView, "Manage View");
    }

    public void RefreshData()
    {
      this.modified = false;
      this.chkViewEligible.Checked = false;
      this.chkImport.Checked = false;
      this.pnlEligibleLoans.Height = 0;
    }

    public void RefreshData(PipelineInfo[] pinfos)
    {
      this.RefreshData();
      this.loadLoans(pinfos);
    }

    public void DisplayWithdrawnLoanMessage()
    {
      if (this.withdrawnLoanMessageDisplayed)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The highlighted loan(s) below have been withdrawn. Please note and take appropriate action.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.withdrawnLoanMessageDisplayed = false;
    }

    public void PlaceAssignedLoans(string[] loanGuids) => this.assignLoansToTrade(loanGuids);

    private void resetFieldDefs()
    {
      this.fieldDefs = LoanReportFieldDefs.GetFieldDefs(Session.DefaultInstance, LoanReportFieldFlags.DatabaseFieldsNoAudit);
      this.tradeAssLoanFieldDefs = TradeAssignedLoanFieldDefs.GetFieldDefs();
    }

    private void loadLoans(PipelineInfo[] pinfos)
    {
      if (pinfos == null)
        pinfos = new PipelineInfo[0];
      this.refreshAssignedLoans();
      if (this.tradeEditor.CurrentTradeInfo.Locked)
      {
        this.chkViewEligible.Checked = false;
        this.chkViewEligible.Visible = false;
        this.chkImport.Checked = false;
        this.chkImport.Visible = false;
      }
      else if (this.chkViewEligible.Checked)
        this.refreshEligibleLoans();
      if (this.tradeEditor is CorrespondentTradeEditor)
        return;
      this.btnUpdateLoans.Enabled = pinfos.Length != 0 && !this.readOnly;
    }

    private List<TradeLoanUpdateError> assignLoansToTrade(string[] guids)
    {
      TradeType tradeType = TradeType.None;
      if (this.tradeEditor is MbsPoolEditor || this.tradeEditor is FannieMaePEPoolEditor)
        tradeType = TradeType.MbsPool;
      else if (this.tradeEditor is TradeEditor)
        tradeType = TradeType.LoanTrade;
      else if (this.tradeEditor is CorrespondentTradeEditor)
        tradeType = TradeType.CorrespondentTrade;
      List<TradeLoanUpdateError> trade = this.tradeEditor.AssignLoanToTrade(Session.LoanManager.GetPipeline(guids, this.GetAssignedLoanListFields(), PipelineData.Trade, false, tradeType));
      this.refreshAssignedLoans();
      this.modified = true;
      return trade;
    }

    private bool assignLoanToTrade(PipelineInfo pinfo)
    {
      if (this.tradeEditor.CurrentTradeInfo.Pricing.SimplePricingItems.Count > 0 && !this.tradeEditor.IsNoteRateAllowed(pinfo))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The loan '" + pinfo.LoanNumber + "' cannot be assigned to this trade because its Note Rate does not fall within the range specified on the Pricing tab.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return true;
      }
      if (string.Concat(pinfo.GetField("InvestorStatus")) == "Purchased")
      {
        switch (Utils.Dialog((IWin32Window) this, "The loan '" + pinfo.LoanNumber + "' is marked as purchased. Are you sure you want to assign this loan to the " + this.GetNameOfTradeOrPool() + "?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        {
          case DialogResult.Cancel:
            return false;
          case DialogResult.No:
            return true;
        }
      }
      this.assignLoansToTrade(new string[1]{ pinfo.GUID });
      return true;
    }

    private void refreshAssignedLoans()
    {
      this.gvLoans.Items.Clear();
      if (this.tradeEditor.CurrentTradeInfo == null || this.tradeEditor.CurrentTradeInfo.Filter == null)
        return;
      IFilterEvaluator evaluator = (IFilterEvaluator) null;
      if (this.tradeEditor.CurrentTradeInfo.TradeType == TradeType.LoanTrade)
        evaluator = this.tradeEditor.CurrentTradeInfo.Filter.CreateEvaluator(typeof (LoanTradeFilterEvaluator));
      else if (this.tradeEditor.CurrentTradeInfo.TradeType == TradeType.MbsPool)
        evaluator = this.tradeEditor.CurrentTradeInfo.Filter.CreateEvaluator(typeof (MbsPoolFilterEvaluator));
      List<PipelineInfo> loans = new List<PipelineInfo>();
      foreach (LoanToTradeAssignmentBase assignedPendingLoan in this.tradeEditor.GetAllAssignedPendingLoans())
      {
        loans.Add(assignedPendingLoan.PipelineInfo);
        GVItem assignedLoanListItem = this.createAssignedLoanListItem(assignedPendingLoan, evaluator);
        if (this.tradeEditor.CurrentTradeInfo.TradeType == TradeType.CorrespondentTrade && this.IsAnyLoanPending(assignedPendingLoan))
        {
          assignedLoanListItem.BackColor = SystemColors.Highlight;
          assignedLoanListItem.ForeColor = SystemColors.HighlightText;
          if (!this.btnUpdateSelectedLoans.Enabled)
            this.btnUpdateSelectedLoans.Enabled = true;
        }
        this.gvLoans.Items.Add(assignedLoanListItem);
      }
      this.ctlProfit.Calculate(this.tradeEditor.CurrentTradeInfo, (IEnumerable<PipelineInfo>) loans);
      if (this.tradeEditor.CurrentTradeInfo.TradeType == TradeType.CorrespondentTrade)
        this.setUpdateLoansButtonState(this.btnUpdateSelectedLoans);
      else
        this.setUpdateLoansButtonState(this.btnUpdateLoans);
      if (this.tradeEditor is CorrespondentTradeEditor)
        this.SetCorrespondentUpdateButtonAttributes();
      else
        this.btnUpdateSelectedLoans.Enabled = this.gvLoans.SelectedItems.Count > 0 && !this.readOnly;
    }

    private bool IsAnyLoanPending(LoanToTradeAssignmentBase assignment)
    {
      return this.tradeEditor.GetTradeStatusDescription(assignment).IndexOf("Pending") > 0;
    }

    private bool IsAnyLoanPending()
    {
      foreach (LoanToTradeAssignmentBase assignedPendingLoan in this.tradeEditor.GetAllAssignedPendingLoans())
      {
        if (this.IsAnyLoanPending(assignedPendingLoan))
          return true;
      }
      return false;
    }

    public void RefreshCurrentInTradeView(bool refreshLoans)
    {
      if (!refreshLoans)
        return;
      if (this.isPipelineDataRefreshRequired())
        this.refreshAssignedLoanPipelineData();
      this.refreshAssignedLoans();
    }

    public void RefreshLoanList(bool refreshLoans)
    {
      if (refreshLoans)
      {
        if (this.isPipelineDataRefreshRequired())
          this.refreshAssignedLoanPipelineData();
        this.refreshAssignedLoans();
      }
      if (!(this.chkViewEligible.Checked & refreshLoans))
        return;
      this.refreshEligibleLoans();
    }

    private void loadEligibleLoans()
    {
      this.performEligibleLoanSearch(this.getSortOrder(this.gvEligible.Columns.GetSortOrder()));
    }

    private string[] getLoanListFields(GridView listView)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.getRequiredFields());
      foreach (GVColumn column in listView.Columns)
      {
        TableLayout.Column tag = (TableLayout.Column) column.Tag;
        if (!tag.Tag.StartsWith("TradeEligibility") && tag.Tag != "" && !stringList.Contains(tag.Tag))
          stringList.Add(tag.Tag);
      }
      return stringList.ToArray();
    }

    private string[] getRequiredFields()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.tradeEditor.GetPricingAndEligibilityFields());
      for (int index = 0; index < LoanListScreen.requiredEligibilityFields.Length; ++index)
      {
        if (!stringList.Contains(LoanListScreen.requiredEligibilityFields[index]))
          stringList.Add(LoanListScreen.requiredEligibilityFields[index]);
      }
      for (int index = 0; index < this.ctlProfit.RequiredFields.Length; ++index)
      {
        if (!stringList.Contains(this.ctlProfit.RequiredFields[index]))
          stringList.Add(this.ctlProfit.RequiredFields[index]);
      }
      if (this.tradeEditor is TradeEditor)
      {
        foreach (string requiredField in TradeAssignmentManager.RequiredFields)
        {
          if (!stringList.Contains(requiredField))
            stringList.Add(requiredField);
        }
      }
      else if (this.tradeEditor is MbsPoolEditor)
      {
        foreach (string requiredField in MbsPoolLoanAssignmentManager.RequiredFields)
        {
          if (!stringList.Contains(requiredField))
            stringList.Add(requiredField);
        }
      }
      if (!stringList.Contains("Loan.CurrentMilestoneID"))
        stringList.Add("Loan.CurrentMilestoneID");
      if (!stringList.Contains("Loan.CurrentCoreMilestoneName"))
        stringList.Add("Loan.CurrentCoreMilestoneName");
      if (!stringList.Contains("Loan.LockAndRequestStatus"))
        stringList.Add("Loan.LockAndRequestStatus");
      if (!stringList.Contains("Loan.NextMilestoneID"))
        stringList.Add("Loan.NextMilestoneID");
      if (!stringList.Contains("Loan.NextMilestoneName"))
        stringList.Add("Loan.NextMilestoneName");
      return stringList.ToArray();
    }

    private bool isPipelineDataRefreshRequired()
    {
      PipelineInfo pipelineInfo = (PipelineInfo) null;
      using (List<LoanToTradeAssignmentBase>.Enumerator enumerator = this.tradeEditor.GetLoanToTradeAssignments().GetEnumerator())
      {
        if (enumerator.MoveNext())
          pipelineInfo = enumerator.Current.PipelineInfo;
      }
      if (pipelineInfo == null)
        return false;
      foreach (string assignedLoanListField in this.GetAssignedLoanListFields())
      {
        if (!pipelineInfo.Info.ContainsKey((object) assignedLoanListField))
          return true;
      }
      return false;
    }

    private void refreshAssignedLoanPipelineData()
    {
      PipelineInfo[] pipeline = Session.LoanManager.GetPipeline(this.tradeEditor.GetLoanToTradeAssignmentAllLoanGuids(), this.GetAssignedLoanListFields(), PipelineData.Trade, false);
      Dictionary<string, PipelineInfo> dictionary = new Dictionary<string, PipelineInfo>();
      foreach (PipelineInfo pipelineInfo in pipeline)
      {
        if (pipelineInfo != null)
          dictionary[pipelineInfo.GUID] = pipelineInfo;
      }
      foreach (LoanToTradeAssignmentBase toTradeAssignment in this.tradeEditor.GetLoanToTradeAssignments())
      {
        if (dictionary.ContainsKey(toTradeAssignment.Guid))
          toTradeAssignment.PipelineInfo = dictionary[toTradeAssignment.Guid];
      }
    }

    private void setUpdateLoansButtonState(ButtonEx updateButton)
    {
      string text = updateButton != this.btnUpdateLoans ? "Update Selected Loans" : "Update All Loans";
      this.tradeEditor.ToolTip.SetToolTip((Control) this.btnUpdateLoans, "");
      updateButton.Value = (Element) new MultiValueElement((IEnumerable<Element>) new Element[1]
      {
        (Element) new TextElement(text)
      });
      int pendingAssignmentCount = this.tradeEditor.GetLoanToTradePendingAssignmentCount();
      int pendingShipmentCount = this.tradeEditor.GetLoanToTradePendingShipmentCount();
      int pendingRemovalCount = this.tradeEditor.GetLoanToTradePendingRemovalCount();
      int pendingPurchaseCount = this.tradeEditor.GetLoanToTradePendingPurchaseCount();
      if (pendingAssignmentCount + pendingShipmentCount + pendingRemovalCount + pendingPurchaseCount > 0 && !this.readOnly)
      {
        updateButton.Value = (Element) new MultiValueElement((Element) new AlertMessageLabel(AlertMessageLabel.AlertMessageStyle.Alert, "!"), (Element) new TextElement(text), 5, LayoutDirection.Horizontal);
        this.tradeEditor.ToolTip.SetToolTip((Control) updateButton, this.getPendingStatusDescription(pendingAssignmentCount, pendingShipmentCount, pendingRemovalCount, pendingPurchaseCount));
        updateButton.Enabled = !this.readOnly;
      }
      else
      {
        this.tradeEditor.ToolTip.SetToolTip((Control) updateButton, "");
        updateButton.Enabled = this.gvLoans.Items.Count > 0 && !this.readOnly;
      }
      if (this.tradeEditor is GseCommitmentEditor)
        this.btnUpdateSelectedLoans.Visible = false;
      else
        this.btnUpdateSelectedLoans.Visible = true;
    }

    private string getPendingStatusDescription(
      int pendingAssignment,
      int pendingShipment,
      int pendingRemoval,
      int pendingPurchase)
    {
      List<string> stringList = new List<string>();
      if (pendingAssignment > 0)
        stringList.Add(pendingAssignment.ToString() + " Assigned - Pending");
      if (pendingShipment > 0)
        stringList.Add(pendingShipment.ToString() + " Shipped - Pending");
      if (pendingRemoval > 0)
        stringList.Add(pendingRemoval.ToString() + " Removed - Pending");
      if (pendingPurchase > 0)
        stringList.Add(pendingPurchase.ToString() + " Purchased - Pending");
      return string.Join(Environment.NewLine, stringList.ToArray());
    }

    private bool isAssignedLoanEligible(IFilterEvaluator evaluator, PipelineInfo pinfo)
    {
      if (!evaluator.Evaluate((object) pinfo, FilterEvaluationOption.NonVolatile))
        return false;
      return this.tradeEditor.CurrentTradeInfo.Pricing.SimplePricingItems.Count <= 0 || this.tradeEditor.IsNoteRateAllowed(pinfo);
    }

    private void highlightIneligibleFields(
      GVItem item,
      IFilterEvaluator evaluator,
      PipelineInfo pinfo)
    {
      for (int index = 0; index < item.SubItems.Count; ++index)
      {
        TableLayout.Column tag = (TableLayout.Column) this.gvLoans.Columns[index].Tag;
        bool flag = false;
        if ((tag.Tag ?? "") != "")
        {
          if (!((TradeFilterEvaluator) evaluator).MatchesAllCriteria(tag.Tag, pinfo.Info[(object) tag.Tag], FilterEvaluationOption.NonVolatile))
            flag = true;
          else if (tag.Tag == "Loan.LoanRate" && this.tradeEditor.CurrentTradeInfo.Pricing.SimplePricingItems.Count > 0 && !this.tradeEditor.IsNoteRateAllowed(pinfo))
            flag = true;
        }
        if (flag)
          this.displayItemAlert(item, index);
      }
    }

    private void loadViewList(FileSystemEntry[] fsEntries)
    {
      this.tradeEditor.SuspendEvents = true;
      try
      {
        this.cBoxView.Items.Clear();
        this.cBoxView.Dividers.Clear();
        foreach (FileSystemEntry fsEntry in fsEntries)
          this.cBoxView.Items.Add((object) new FileSystemEntryListItem(fsEntry));
        this.cBoxView.Dividers.Add(this.cBoxView.Items.Count);
        this.cBoxView.Items.Add((object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.standardViewName, FileSystemEntry.Types.File, Session.UserID)));
        if (this.currentView != null)
          ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, Session.UserID)), false);
        if (this.cBoxView.SelectedIndex != -1)
          return;
        this.cBoxView.SelectedIndex = 0;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanListScreen.sw, this.className, TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.tradeEditor.SuspendEvents = false;
      }
    }

    private TradeView getDefaultTradeAssignedLoansView()
    {
      TradeView assignedLoansView = new TradeView(this.standardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("TradeAssignment.Status", "Trade Assignment Status", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("TradeAssignment.StatusDate", "Date", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103));
      if (this.tradeEditor is FannieMaePEPoolEditor)
      {
        tableLayout.AddColumn(new TableLayout.Column("TradeAssignment.CommitmentContractNumber", "Commitment Contract #", HorizontalAlignment.Left, 150));
        tableLayout.AddColumn(new TableLayout.Column("TradeAssignment.ProductName", "Product Name", HorizontalAlignment.Left, 104));
      }
      tableLayout.AddColumn(new TableLayout.Column("Loan.TotalBuyPrice", "Loan Trade Total Buy Price", HorizontalAlignment.Right, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.TotalSellPrice", "Loan Trade Total Sell Price", HorizontalAlignment.Right, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.NetProfit", "Gain/Loss", HorizontalAlignment.Right, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LoanProgram", "Loan Program", HorizontalAlignment.Left, 94));
      tableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Last Finished Milestone", HorizontalAlignment.Left, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.TotalLoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Note Rate", HorizontalAlignment.Right, 68));
      tableLayout.AddColumn(new TableLayout.Column("Loan.Term", "Term", HorizontalAlignment.Right, 56));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Right, 52));
      tableLayout.AddColumn(new TableLayout.Column("Loan.CLTV", "CLTV", HorizontalAlignment.Right, 52));
      tableLayout.AddColumn(new TableLayout.Column("Loan.DTITop", "Top", "Top Ratio", HorizontalAlignment.Right, 50));
      tableLayout.AddColumn(new TableLayout.Column("Loan.DTIBottom", "Bottom", "Bottom Ratio", HorizontalAlignment.Right, 50));
      tableLayout.AddColumn(new TableLayout.Column("Loan.CreditScore", "FICO", HorizontalAlignment.Right, 56));
      tableLayout.AddColumn(new TableLayout.Column("Loan.OccupancyStatus", "Occupancy Type", HorizontalAlignment.Left, 101));
      tableLayout.AddColumn(new TableLayout.Column("Loan.PropertyType", "Property Type", HorizontalAlignment.Left, 82));
      tableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 56));
      tableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Expiration Date", HorizontalAlignment.Left, 124));
      tableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerLastName", "Last Name", HorizontalAlignment.Left, 104));
      assignedLoansView.Layout = tableLayout;
      return assignedLoansView;
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
    }

    public void SetCurrentView(FileSystemEntry fsEntry)
    {
      try
      {
        if (fsEntry.Name == this.standardViewName)
        {
          TradeView assignedLoansView = this.getDefaultTradeAssignedLoansView();
          this.fsViewEntry = fsEntry;
          this.setCurrentView(assignedLoansView);
        }
        else
        {
          TradeView templateSettings = (TradeView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeAssignedLoanView, fsEntry);
          if (templateSettings == null)
            throw new ArgumentException();
          this.btnEditView.Enabled = !fsEntry.IsPublic;
          this.fsViewEntry = fsEntry;
          this.setCurrentView(templateSettings);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanListScreen.sw, this.className, TraceLevel.Error, "Error opening view: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    private void setCurrentView(TradeView view)
    {
      this.currentView = view;
      this.tradeEditor.SuspendEvents = true;
      this.applyTableLayout(view.Layout);
      this.RefreshCurrentInTradeView(true);
      this.gvLoans.Sort(this.generateLoanSortFields());
      this.setViewChanged(false);
      this.ViewModified = false;
      this.tradeEditor.SuspendEvents = false;
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    private void setViewChanged(bool modified)
    {
      this.btnSaveView.Enabled = this.ViewModified;
      this.btnRefreshView.Enabled = modified;
      if (!modified)
        return;
      this.tradeEditor.ToolTip.SetToolTip((Control) this.btnSaveView, "Save View");
      this.tradeEditor.ToolTip.SetToolTip((Control) this.btnRefreshView, "Reset View");
    }

    private void saveCurrentView()
    {
      TradeView tradeView = new TradeView(this.currentView.Name);
      tradeView.Layout = this.gvLayoutManager.GetCurrentLayout();
      using (SaveViewTemplateDialog viewTemplateDialog = new SaveViewTemplateDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeAssignedLoanView, (object) tradeView, this.getViewNameList(), this.currentView.Name != this.standardViewName))
      {
        if (viewTemplateDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        if (!viewTemplateDialog.SelectedEntry.Equals((object) this.fsViewEntry))
          this.updateCurrentView(tradeView, viewTemplateDialog.SelectedEntry);
      }
      this.currentView = tradeView;
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (object obj in this.cBoxView.Items)
        stringList.Add(obj.ToString());
      return stringList.ToArray();
    }

    private void updateCurrentView(TradeView view, FileSystemEntry e)
    {
      this.tradeEditor.SuspendEvents = true;
      this.currentView = view;
      ClientCommonUtils.PopulateDropdown((ComboBox) this.cBoxView, (object) new FileSystemEntryListItem(e), true);
      this.tradeEditor.SuspendEvents = false;
    }

    public void RefreshViews()
    {
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeAssignedLoanView, FileSystemEntry.PrivateRoot(Session.UserID)));
      if (this.cBoxView.Items.Count <= 0 || this.cBoxView.SelectedIndex >= 0)
        return;
      this.cBoxView.SelectedIndex = 0;
    }

    public void ValidateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        if (!((IEnumerable<string>) this.eligibleSkipList).Contains<string>(column.ColumnID))
        {
          LoanReportFieldDef fieldByCriterionName = this.fieldDefs.GetFieldByCriterionName(column.ColumnID);
          if (fieldByCriterionName != null)
            column.Title = fieldByCriterionName.Description;
          else
            columnList.Add(column);
        }
      }
      foreach (TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    public void AdjustLoanPanelDisplay()
    {
      if (this.chkViewEligible.Checked)
        this.pnlEligibleLoans.Height = this.tradeEditor.LoanToTradeAssignmentTab.ClientSize.Height / 2;
      else
        this.pnlEligibleLoans.Height = 0;
    }

    private TableLayout getDefaultAssignedTableLayout()
    {
      TableLayout assignedTableLayout = new TableLayout();
      assignedTableLayout.AddColumn(new TableLayout.Column("Trade.Eligibility", "", "Eligibility Indicator", "", HorizontalAlignment.Left, 25, true));
      assignedTableLayout.AddColumn(new TableLayout.Column("TradeAssignment.Status", "Status", HorizontalAlignment.Left, 103));
      assignedTableLayout.AddColumn(new TableLayout.Column("TradeAssignment.StatusDate", "Date", HorizontalAlignment.Left, 103));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.TotalBuyPrice", "Loan Trade Total Buy Price", HorizontalAlignment.Right, 94));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.TotalSellPrice", "Loan Trade Total Sell Price", HorizontalAlignment.Right, 94));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.NetProfit", "Gain/Loss", HorizontalAlignment.Right, 94));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.LoanProgram", "Loan Program", HorizontalAlignment.Left, 94));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Last Finished Milestone", HorizontalAlignment.Left, 103));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.TotalLoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Note Rate", HorizontalAlignment.Right, 68));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.Term", "Term", HorizontalAlignment.Right, 56));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Right, 52));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.CLTV", "CLTV", HorizontalAlignment.Right, 52));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.DTITop", "Top", "Top Ratio", HorizontalAlignment.Right, 50));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.DTIBottom", "Bottom", "Bottom Ratio", HorizontalAlignment.Right, 50));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.CreditScore", "FICO", HorizontalAlignment.Right, 56));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.OccupancyStatus", "Occupancy Type", HorizontalAlignment.Left, 101));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.PropertyType", "Property Type", HorizontalAlignment.Left, 82));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 56));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Expiration Date", HorizontalAlignment.Left, 124));
      assignedTableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerLastName", "Last Name", HorizontalAlignment.Left, 104));
      return assignedTableLayout;
    }

    public TableLayout.Column[] GetFixedEligibleLoanColumns()
    {
      return new List<TableLayout.Column>()
      {
        new TableLayout.Column("TradeAssignment.Status", "Status", HorizontalAlignment.Left, 103),
        new TableLayout.Column("Trade.Name", "Trade ID", HorizontalAlignment.Left, 103),
        new TableLayout.Column("Loan.LoanNumber", "Loan Number", HorizontalAlignment.Left, 103),
        new TableLayout.Column("Loan.TotalBuyPrice", "Loan Trade Total Buy Price", HorizontalAlignment.Right, 94),
        new TableLayout.Column("Loan.TotalSellPrice", "Loan Trade Total Sell Price", HorizontalAlignment.Right, 94),
        new TableLayout.Column("Loan.NetProfit", "Gain/Loss", HorizontalAlignment.Right, 94)
      }.ToArray();
    }

    private TableLayout getDefaultEligibleTableLayout()
    {
      TableLayout eligibleTableLayout = new TableLayout();
      foreach (TableLayout.Column eligibleLoanColumn in this.GetFixedEligibleLoanColumns())
        eligibleTableLayout.AddColumn(eligibleLoanColumn);
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LoanProgram", "Loan Program", HorizontalAlignment.Left, 94));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.CurrentMilestoneName", "Last Finished Milestone", HorizontalAlignment.Left, 103));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.TotalLoanAmount", "Loan Amount", HorizontalAlignment.Right, 103));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LoanRate", "Note Rate", HorizontalAlignment.Right, 68));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.Term", "Term", HorizontalAlignment.Right, 56));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LTV", "LTV", HorizontalAlignment.Right, 52));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.CLTV", "CLTV", "Combined LTV", HorizontalAlignment.Right, 52));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.DTITop", "Top", "Top Ratio", HorizontalAlignment.Right, 50));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.DTIBottom", "Bottom", "Bottom Ratio", HorizontalAlignment.Right, 50));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.CreditScore", "FICO", HorizontalAlignment.Right, 56));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.OccupancyStatus", "Occupancy Type", HorizontalAlignment.Left, 101));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.PropertyType", "Property Type", HorizontalAlignment.Left, 82));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.State", "State", HorizontalAlignment.Left, 56));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.LockExpirationDate", "Lock Expiration Date", HorizontalAlignment.Left, 124));
      eligibleTableLayout.AddColumn(new TableLayout.Column("Loan.BorrowerLastName", "Last Name", HorizontalAlignment.Left, 104));
      return eligibleTableLayout;
    }

    private TableLayout getFullEligibleTableLayout()
    {
      TableLayout eligibleTableLayout = this.getDefaultEligibleTableLayout();
      foreach (LoanReportFieldDef fieldDef in (ReportFieldDefContainer) this.fieldDefs)
      {
        if (fieldDef.IsDatabaseField && eligibleTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          eligibleTableLayout.AddColumn(new TableLayout.Column(fieldDef.CriterionFieldName, fieldDef.Name, fieldDef.Description, fieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric ? HorizontalAlignment.Right : HorizontalAlignment.Left, 100));
      }
      eligibleTableLayout.SortByDescription();
      return eligibleTableLayout;
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (ReportFieldDef tradeAssLoanFieldDef in (ReportFieldDefContainer) this.tradeAssLoanFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(tradeAssLoanFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(tradeAssLoanFieldDef.ToTableLayoutColumn());
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private GVColumnSort[] generateLoanSortFields()
    {
      List<GVColumnSort> gvColumnSortList = new List<GVColumnSort>();
      foreach (TableLayout.Column column1 in this.currentView.Layout.GetSortColumnsByPriority())
      {
        foreach (GVColumn column2 in this.gvLoans.Columns)
        {
          if (((TableLayout.Column) column2.Tag).ColumnID == column1.ColumnID)
          {
            gvColumnSortList.Add(new GVColumnSort(column2.Index, column1.SortOrder));
            break;
          }
        }
      }
      return gvColumnSortList.ToArray();
    }

    private SortField[] getSortOrder(GVColumnSort[] sortColumns)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort sortColumn in sortColumns)
      {
        TableLayout.Column tag1 = (TableLayout.Column) this.gvEligible.Columns[sortColumn.Column].Tag;
        string tag2 = tag1.Tag;
        this.fieldDefs.GetFieldByCriterionName(tag2);
        DataConversion conversion = DataConversion.None;
        if (tag1.ColumnID == "TradeAssignment.Status")
          conversion = DataConversion.Numeric;
        else if (tag1.ColumnID == "TradeAssignment.StatusDate")
          conversion = DataConversion.DateTime;
        FieldSortOrder sortOrder = sortColumn.SortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending;
        SortField sortField = new SortField(tag2, sortOrder, conversion);
        sortFieldList.Add(sortField);
      }
      return sortFieldList.ToArray();
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvLoans, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    public void ApplyLayout(TableLayout layout, bool raiseLayoutEvent)
    {
      if (layout == null)
        return;
      this.eligibleLayoutMngr.ApplyLayout(layout, raiseLayoutEvent);
    }

    public TableLayout GetCurrentLayout() => this.eligibleLayoutMngr.GetCurrentLayout();

    private GVItem createAssignedLoanListItem(
      LoanToTradeAssignmentBase loanInfo,
      IFilterEvaluator evaluator)
    {
      bool flag = true;
      GVItem assignedLoanListItem = new GVItem();
      assignedLoanListItem.Tag = (object) loanInfo;
      foreach (GVColumn column in this.gvLoans.Columns)
      {
        TableLayout.Column tag1 = (TableLayout.Column) column.Tag;
        string tag2 = tag1.Tag;
        Decimal num;
        if (tag2 == "TradeAssignment.Status")
          assignedLoanListItem.SubItems[column.Index].Text = this.tradeEditor.GetTradeStatusDescription(loanInfo);
        else if (tag2 == "Loan.TotalBuyPrice" && this.tradeEditor is CorrespondentTradeEditor)
        {
          if (((CorrespondentTradeEditor) this.tradeEditor).IsBulkPricing())
          {
            Decimal loanTotalPrice = ((CorrespondentTradeEditor) this.tradeEditor).GetLoanTotalPrice((CorrespondentTradeLoanAssignment) loanInfo);
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo, loanTotalPrice);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
          else if (!this.tradeEditor.CurrentTradeInfo.Pricing.IsAdvancedPricing)
          {
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
        }
        else if (tag2 == "Loan.TotalSellPrice" && !(this.tradeEditor is CorrespondentTradeEditor))
        {
          if (this.tradeEditor is TradeEditor && ((TradeEditor) this.tradeEditor).IsBulkPricing())
          {
            Decimal loanTotalPrice = ((TradeEditor) this.tradeEditor).GetLoanTotalPrice((LoanTradeAssignment) loanInfo);
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo, loanTotalPrice);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
          else if (!this.tradeEditor.CurrentTradeInfo.Pricing.IsAdvancedPricing)
          {
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
          else if (this.tradeEditor is MbsPoolEditor)
          {
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo, ((MbsPoolInfo) this.tradeEditor.CurrentTradeInfo).WeightedAvgPrice);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
          else if (this.tradeEditor is FannieMaePEPoolEditor)
          {
            ((FannieMaePEPoolEditor) this.tradeEditor).CommitmentContractNumberLoanCal = ((MbsPoolLoanAssignment) loanInfo).CommitmentContractNumber;
            ((FannieMaePEPoolEditor) this.tradeEditor).ProductNameLoanCal = ((MbsPoolLoanAssignment) loanInfo).ProductName;
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo, ((MbsPoolInfo) this.tradeEditor.CurrentTradeInfo).WeightedAvgPrice);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
          else if (this.tradeEditor is TradeEditor || this.tradeEditor is CorrespondentTradeEditor)
          {
            SecurityTradeInfo securityTradeInfo = (SecurityTradeInfo) null;
            if (this.tradeEditor is TradeEditor && ((LoanTradeInfo) this.tradeEditor.CurrentTradeInfo).SecurityTradeID > 0)
              securityTradeInfo = Session.SecurityTradeManager.GetTrade(((LoanTradeInfo) this.tradeEditor.CurrentTradeInfo).SecurityTradeID);
            GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
            num = this.tradeEditor.CalculatePriceIndex(loanInfo.PipelineInfo, securityTradeInfo == null ? 0M : securityTradeInfo.Price);
            string str = num.ToString("0.000;;\"\"");
            subItem.Text = str;
          }
        }
        else
        {
          switch (tag2)
          {
            case "Loan.NetProfit":
              if (this.tradeEditor is MbsPoolEditor)
              {
                GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
                num = this.tradeEditor.CalculateProfit(loanInfo.PipelineInfo, ((MbsPoolInfo) this.tradeEditor.CurrentTradeInfo).WeightedAvgPrice);
                string str = num.ToString("#,###");
                subItem.Text = str;
                continue;
              }
              if (this.tradeEditor is TradeEditor || this.tradeEditor is CorrespondentTradeEditor || this.tradeEditor is GseCommitmentEditor)
              {
                GVSubItem subItem = assignedLoanListItem.SubItems[column.Index];
                num = this.tradeEditor.CalculateProfit(loanInfo.PipelineInfo, 0M);
                string str = num.ToString("#,###");
                subItem.Text = str;
                continue;
              }
              continue;
            case "TradeAssignment.StatusDate":
              assignedLoanListItem.SubItems[column.Index].Text = this.tradeEditor.CurrentTradeInfo.TradeID <= 0 || Utils.ParseInt(loanInfo.PipelineInfo.Info[(object) "TradeAssignment.TradeID"]) != this.tradeEditor.CurrentTradeInfo.TradeID ? "" : Utils.ParseDate(loanInfo.PipelineInfo.Info[(object) "TradeAssignment.StatusDate"]).ToString("MM/dd/yyyy");
              continue;
            default:
              if (tag1.ColumnID == "Trade.Eligibility")
              {
                if (!this.isAssignedLoanEligible(evaluator, loanInfo.PipelineInfo))
                {
                  assignedLoanListItem.SubItems[column.Index].Text = "!";
                  this.displayItemAlert(assignedLoanListItem, column.Index);
                  flag = false;
                  continue;
                }
                continue;
              }
              if (tag1.ColumnID == "TradeAssignment.CommitmentContractNumber")
              {
                if (this.tradeEditor is FannieMaePEPoolEditor || this.tradeEditor is GseCommitmentEditor)
                {
                  assignedLoanListItem.SubItems[column.Index].Text = ((MbsPoolLoanAssignment) loanInfo).CommitmentContractNumber;
                  continue;
                }
                continue;
              }
              if (tag1.ColumnID == "TradeAssignment.ProductName")
              {
                if (this.tradeEditor is FannieMaePEPoolEditor || this.tradeEditor is GseCommitmentEditor)
                {
                  assignedLoanListItem.SubItems[column.Index].Text = ((MbsPoolLoanAssignment) loanInfo).ProductName;
                  continue;
                }
                continue;
              }
              assignedLoanListItem.SubItems[column.Index].Value = this.translateFieldValue(loanInfo.PipelineInfo, tag2, string.Concat(loanInfo.PipelineInfo.Info[(object) tag2]), (Control) this.gvLoans);
              continue;
          }
        }
      }
      if (!flag)
        this.highlightIneligibleFields(assignedLoanListItem, evaluator, loanInfo.PipelineInfo);
      if (this.tradeEditor.IsLoanToTradeAssignmentPending(loanInfo))
        assignedLoanListItem.BackColor = LoanListScreen.HighlightColor;
      if (!(this.tradeEditor is GseCommitmentEditor) && loanInfo.PipelineInfo.Info[(object) "Loan.WithdrawnDate"] != null && Utils.ToDate(loanInfo.PipelineInfo.Info[(object) "Loan.WithdrawnDate"].ToString()) != DateTime.MinValue)
      {
        assignedLoanListItem.BackColor = EncompassColors.Alert3;
        this.withdrawnLoanMessageDisplayed = true;
      }
      return assignedLoanListItem;
    }

    public PipelineInfo[] GetSelectedAndPendingLoans()
    {
      List<PipelineInfo> pipelineInfoList = new List<PipelineInfo>();
      if (this.gvLoans.Items.Count > 0)
      {
        if (this.gvLoans.Items[0].Tag is LoanTradeAssignment)
        {
          foreach (GVItem gvItem in this.gvLoans.Items.Where<GVItem>((Func<GVItem, bool>) (t => t.Selected)).ToArray<GVItem>())
          {
            PipelineInfo pipelineInfo = ((LoanToTradeAssignmentBase) gvItem.Tag).PipelineInfo;
            pipelineInfoList.Add(((LoanToTradeAssignmentBase) gvItem.Tag).PipelineInfo);
          }
        }
        if (this.gvLoans.Items[0].Tag is MbsPoolLoanAssignment)
        {
          foreach (GVItem gvItem in this.gvLoans.Items.Where<GVItem>((Func<GVItem, bool>) (t => t.Selected)).ToArray<GVItem>())
          {
            PipelineInfo pipelineInfo = ((LoanToTradeAssignmentBase) gvItem.Tag).PipelineInfo;
            pipelineInfoList.Add(((LoanToTradeAssignmentBase) gvItem.Tag).PipelineInfo);
          }
        }
        if (this.gvLoans.Items[0].Tag is CorrespondentTradeLoanAssignment)
        {
          foreach (GVItem gvItem in this.gvLoans.Items.Where<GVItem>((Func<GVItem, bool>) (t => t.Selected || t.BackColor == SystemColors.Highlight)).ToArray<GVItem>())
          {
            PipelineInfo pipelineInfo = ((LoanToTradeAssignmentBase) gvItem.Tag).PipelineInfo;
            pipelineInfoList.Add(((LoanToTradeAssignmentBase) gvItem.Tag).PipelineInfo);
          }
        }
      }
      return pipelineInfoList.ToArray();
    }

    public string[] GetAssignedLoanListFields() => this.getLoanListFields(this.gvLoans);

    private void refreshEligibleLoans()
    {
      this.performEligibleLoanSearch(this.getSortOrder(this.gvEligible.Columns.GetSortOrder()));
    }

    private string[] getEligibleLoanListFields() => this.getLoanListFields(this.gvEligible);

    private void performEligibleLoanSearch(SortField[] sortFields)
    {
      if (this.tradeEditor.Assignments == null)
        return;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        string[] eligibleLoanListFields = this.getEligibleLoanListFields();
        string[] rejectedLoanGuids = this.tradeEditor.GetLoanToTradeAssignedAndRejectedLoanGuids();
        ICursor eligibleLoanCursor = this.tradeEditor.GetEligibleLoanCursor(eligibleLoanListFields, PipelineData.Trade, sortFields, rejectedLoanGuids);
        foreach (GVColumn column in this.gvEligible.Columns)
        {
          TableLayout.Column tag = (TableLayout.Column) column.Tag;
          if (tag.ColumnID == "Loan.NetProfit")
            tag.Tag = this.tradeEditor.CurrentTradeInfo.CreateEligiblityDataKey("Profit");
          else if (tag.ColumnID == "Loan.TotalSellPrice")
            tag.Tag = this.tradeEditor.CurrentTradeInfo.CreateEligiblityDataKey("TotalPrice");
          else if (tag.ColumnID == "Loan.NetSellPrice")
            tag.Tag = this.tradeEditor.CurrentTradeInfo.CreateEligiblityDataKey("NetPrice");
        }
        this.gvEligible.DataProvider = (IGVDataProvider) new CursorGVDataProvider(eligibleLoanCursor, new PopulateGVItemEventHandler(this.gvEligible_PopulateItem));
        this.ctlEligibleProfit.Calculate(this.tradeEditor.CurrentTradeInfo, (IEnumerable<PipelineInfo>) this.tradeEditor.GetLoanToTradeAssignedPipelineData());
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    public void ClearCurrentEligibilityCursor()
    {
      if (this.gvEligible.DataProvider == null)
        return;
      this.gvEligible.DataProvider = (IGVDataProvider) null;
    }

    private void displayItemAlert(GVItem item, int subitemIndex)
    {
      item.SubItems[subitemIndex].Font = new Font(this.Font, FontStyle.Bold);
      item.SubItems[subitemIndex].ForeColor = LoanListScreen.AlertColor;
    }

    private object translateFieldValue(
      PipelineInfo pInfo,
      string fieldName,
      string fieldValue,
      Control parentControl)
    {
      ReportFieldDef fieldByCriterionName = this.tradeAssLoanFieldDefs.GetFieldByCriterionName(fieldName);
      switch (fieldByCriterionName)
      {
        case null:
          return (object) fieldValue;
        case LoanReportFieldDef _:
          return ((LoanReportFieldDef) fieldByCriterionName).ToDisplayElement(fieldName, pInfo, parentControl);
        case TradeReportFieldDef _:
          return ((TradeReportFieldDef) fieldByCriterionName).ToDisplayElement(fieldName, pInfo.Info[(object) fieldName], (EventHandler) null);
        default:
          return (object) fieldByCriterionName.ToDisplayValue(fieldValue);
      }
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
      if (this.tradeEditor.SuspendEvents)
        return;
      this.tradeLoansTableLayout = this.gvLayoutManager.GetCurrentLayout();
      this.setViewChanged(true);
      this.RefreshLoanList(true);
    }

    private void eligibleLayoutMngr_LayoutChanged(object sender, EventArgs e)
    {
      if (!this.chkViewEligible.Checked || this.tradeEditor.Loading)
        return;
      this.modified = true;
      this.loadEligibleLoans();
    }

    private void gvEligible_PopulateItem(object sender, PopulateGVItemEventArgs e)
    {
      PipelineInfo dataItem = (PipelineInfo) e.DataItem;
      foreach (GVColumn column in this.gvEligible.Columns)
      {
        int index = column.Index;
        TableLayout.Column tag1 = (TableLayout.Column) column.Tag;
        string tag2 = tag1.Tag;
        object obj1 = (object) string.Concat(dataItem.Info[(object) tag2]);
        object obj2;
        if (tag1.ColumnID == "Loan.NetProfit")
          obj2 = (object) Utils.ParseDecimal(obj1).ToString("#,###");
        else if (tag1.ColumnID == "Loan.TotalSellPrice")
          obj2 = (object) Utils.ParseDecimal(obj1).ToString("0.000;;\"\"");
        else if (tag1.ColumnID == "Loan.NetSellPrice")
        {
          obj2 = (object) Utils.ParseDecimal(obj1).ToString("0.000;;\"\"");
        }
        else
        {
          switch (tag2)
          {
            case "TradeAssignment.Status":
              obj2 = (object) this.tradeEditor.GetLoanStatusDescription(obj1);
              break;
            case "TradeAssignment.StatusDate":
              obj2 = string.Concat(obj1) == "" ? (object) "" : (object) Utils.ParseDate(obj1).ToString("MM/dd/yyyy");
              break;
            default:
              obj2 = this.translateFieldValue(dataItem, tag2, string.Concat(obj1), (Control) this.gvEligible);
              break;
          }
        }
        e.ListItem.SubItems[index].Value = obj2;
        e.ListItem.Tag = (object) dataItem;
      }
    }

    private void gvEligible_SortItems(object sender, GVColumnSortEventArgs e)
    {
      if (e.ColumnSorts.Length != 0)
        this.performEligibleLoanSearch(this.getSortOrder(new GVColumnSort[1]
        {
          e.ColumnSorts[0]
        }));
      else
        this.performEligibleLoanSearch(this.getSortOrder(e.ColumnSorts));
    }

    private void gvEligible_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      PipelineInfo tag = e.Item.Tag as PipelineInfo;
      Session.Application.GetService<ILoanConsole>()?.OpenLoan(tag.GUID);
    }

    private void gvLoans_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      LoanToTradeAssignmentBase tag = (LoanToTradeAssignmentBase) e.Item.Tag;
      Session.Application.GetService<ILoanConsole>()?.OpenLoan(tag.Guid);
    }

    private void gvLoans_ColumnReorder(object source, GVColumnEventArgs e)
    {
      this.ViewModified = true;
      this.setViewChanged(true);
    }

    private void gvLoans_ColumnResize(object source, GVColumnEventArgs e)
    {
      this.ViewModified = true;
      this.setViewChanged(true);
    }

    private void gvLoans_ColumnClick(object source, GVColumnClickEventArgs e)
    {
      this.ViewModified = true;
      this.setViewChanged(true);
    }

    private void chkViewEligible_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkViewEligible.Checked)
      {
        if (!this.ValidateLoanAssignment())
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This delivery type does not allow more than one loan to be assigned to the trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          this.chkViewEligible.Checked = false;
          this.chkImport.Checked = false;
          this.AdjustLoanPanelDisplay();
          return;
        }
        this.loadEligibleLoans();
        this.chkImport.Checked = false;
      }
      this.AdjustLoanPanelDisplay();
    }

    private void chkImport_CheckedChanged(object sender, EventArgs e)
    {
      if (!this.chkImport.Checked)
        return;
      if (!this.ValidateLoanAssignment())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "This delivery type does not allow more than one loan to be assigned to the trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.chkViewEligible.Checked = false;
        this.chkImport.Checked = false;
        this.AdjustLoanPanelDisplay();
      }
      else
      {
        this.chkViewEligible.Checked = false;
        this.AdjustLoanPanelDisplay();
        LoanAssignmentFromFileImport assignmentFromFileImport = new LoanAssignmentFromFileImport();
        if (this.tradeEditor is CorrespondentTradeEditor && ((CorrespondentTradeInfo) this.tradeEditor.CurrentTradeInfo).IsForIndividualLoan())
          assignmentFromFileImport.MaxLoanCount = 1;
        if (this.tradeEditor is FannieMaePEPoolEditor)
          assignmentFromFileImport.FileType = FileImportType.LoanNumberContractNumberProductName;
        if (this.tradeEditor is CorrespondentTradeEditor && ((CorrespondentTradeEditor) this.tradeEditor).IsCalculatedWABP() || this.tradeEditor is TradeEditor && ((TradeEditor) this.tradeEditor).IsCalculatedWABP())
          assignmentFromFileImport.FileType = FileImportType.LoanNumberPrice;
        if (assignmentFromFileImport.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.assignLoansFromLoanNumbers(assignmentFromFileImport.LoanNumberList);
          this.RefreshWeightedAvgBulkPrice();
        }
        this.chkImport.Checked = false;
      }
    }

    private void btnMarkAsShipped_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "If the shipping date for the selected loan(s) has not been assigned, today's date will be used.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
          return;
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
          this.tradeEditor.MarkLoanToTradeAssignmentStatusToShipped(((LoanToTradeAssignmentBase) selectedItem.Tag).Guid);
        this.refreshAssignedLoans();
      }
    }

    private void btnUpdateLoans_Click(object sender, EventArgs e)
    {
      if (this.tradeEditor.DataModified)
      {
        if (Utils.Dialog((IWin32Window) this, "The current " + this.GetNameOfTradeOrPool() + " has unsaved changes." + Environment.NewLine + "You must save these changes before you update loans." + Environment.NewLine + "Do you want to save the " + this.GetNameOfTradeOrPool() + " and update loans now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.tradeEditor.SaveTrade(true, false);
      }
      else
      {
        this.tradeEditor.CommitLoanToTradeAssignments(true, false);
        this.refreshAssignedLoans();
      }
    }

    private void btnExportLoans_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select at least one loan to be exported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        try
        {
          ExcelHandler excelHandler = new ExcelHandler();
          excelHandler.AddDataTable(this.gvLoans, (ReportFieldDefs) this.fieldDefs, true);
          excelHandler.CreateExcel();
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanListScreen.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
          int num2 = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
    }

    private void gvLoans_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.gvLoans.SelectedItems.Count;
      this.btnRemoveLoans.Enabled = count > 0 && !this.tradeEditor.CurrentTradeInfo.Locked;
      if (this.btnRemoveLoans.Enabled)
        this.tradeEditor.ToolTip.SetToolTip((Control) this.btnRemoveLoans, "Remove Loan");
      this.btnMarkAsShipped.Enabled = this.btnMarkAsPurchased.Enabled = !this.readOnly && count > 0;
      this.btnExportLoans.Enabled = count > 0;
      if (this.btnExportLoans.Enabled)
        this.tradeEditor.ToolTip.SetToolTip((Control) this.btnExportLoans, "Export to Excel");
      if (this.tradeEditor is CorrespondentTradeEditor)
      {
        this.SetCorrespondentUpdateButtonAttributes();
      }
      else
      {
        this.btnUpdateSelectedLoans.Enabled = this.gvLoans.SelectedItems.Count > 0 && !this.readOnly;
        this.btnExtendSelectedLoan.Enabled = this.gvLoans.SelectedItems.Count > 0 && !this.readOnly;
      }
    }

    private void btnRemoveLoans_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans to be removed from this " + this.GetNameOfTradeOrPool() + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.tradeEditor.CurrentTradeInfo.Status == TradeStatus.Purchased)
        {
          if (Utils.Dialog((IWin32Window) this, "This " + this.GetNameOfTradeOrPool() + " has been marked as purchased. Are you sure you want to remove loans from the " + this.GetNameOfTradeOrPool() + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
        }
        List<LoanToTradeAssignmentBase> selectedAssignments = new List<LoanToTradeAssignmentBase>();
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
        {
          LoanToTradeAssignmentBase tag = (LoanToTradeAssignmentBase) selectedItem.Tag;
          selectedAssignments.Add(tag);
        }
        foreach (LoanToTradeAssignmentBase assignment in selectedAssignments)
        {
          string errMsg = "";
          if (!this.tradeEditor.ValidateLoanToTradeAssignment(assignment, out errMsg))
          {
            if (Utils.Dialog((IWin32Window) this, errMsg, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
              return;
            break;
          }
        }
        bool rejected = false;
        string investorName = "";
        TradeType tradeType = TradeType.LoanTrade;
        if (this.tradeEditor is TradeEditor)
        {
          tradeType = TradeType.LoanTrade;
          investorName = ((TradeEditor) this.tradeEditor).GetInvestorText().Trim();
          if (this.tradeEditor.CurrentTradeInfo.TradeID <= 0)
            investorName = "";
        }
        else if (this.tradeEditor is MbsPoolEditor)
        {
          tradeType = TradeType.MbsPool;
          investorName = ((MbsPoolEditor) this.tradeEditor).GetInvestorText().Trim();
          if (this.tradeEditor.CurrentTradeInfo.TradeID <= 0)
            investorName = "";
        }
        else if (this.tradeEditor is CorrespondentTradeEditor)
          tradeType = TradeType.CorrespondentTrade;
        using (RemoveLoansOptionsDialog loansOptionsDialog = new RemoveLoansOptionsDialog(investorName, selectedAssignments, tradeType))
        {
          if (loansOptionsDialog.ShowDialog((IWin32Window) this) != DialogResult.Yes)
            return;
          rejected = loansOptionsDialog.MarkAsRejected;
        }
        List<string> stringList = new List<string>();
        if (rejected && this.tradeEditor is CorrespondentTradeEditor)
        {
          stringList = Session.LoanSummaryExtensionManager.GetSummariesByTradeId(this.tradeEditor.CurrentTradeInfo.TradeID).Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (s => s.SubmittedForReviewDate != DateTime.MinValue)).Select<LoanSummaryExtension, string>((Func<LoanSummaryExtension, string>) (s => s.Guid)).ToList<string>();
          if (stringList.Count != 0)
          {
            List<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError> errors = new List<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>();
            foreach (LoanToTradeAssignmentBase tradeAssignmentBase in selectedAssignments)
            {
              if (stringList.Contains(tradeAssignmentBase.Guid))
                errors.Add(new EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError(string.Empty, (PipelineInfo) null, string.Format("Loan {0} was not voided because Loan {0} was submitted for review. Please remove the submitted for review date before it can be voided and removed.", (object) tradeAssignmentBase.PipelineInfo.LoanNumber)));
            }
            if (errors.Count > 0)
            {
              string detailMessage = LoanListScreen.buildCompletionMessage(errors, selectedAssignments.Count - errors.Count, errors.Count, this.tradeEditor.CurrentTradeInfo.Name);
              int num2 = (int) new TradeLoanUpdateCompleteDialog(selectedAssignments.Count - errors.Count, errors.Count, detailMessage, ProcessType.Void).ShowDialog();
            }
          }
        }
        foreach (LoanToTradeAssignmentBase tradeAssignmentBase in selectedAssignments)
        {
          if (!rejected || rejected && !stringList.Contains(tradeAssignmentBase.Guid))
            this.tradeEditor.RemoveLoanFromTrade(tradeAssignmentBase.Guid, rejected);
        }
        this.refreshAssignedLoans();
        if (this.chkViewEligible.Checked)
          this.refreshEligibleLoans();
        this.RefreshWeightedAvgBulkPrice();
      }
    }

    private void btnEditView_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.TradeAssignedLoanView, false, "TradeAssignedLoans.DefaultView"))
      {
        managementDialog.AddStaticView((BinaryConvertibleObject) this.getDefaultTradeAssignedLoansView());
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
      }
      this.RefreshViews();
    }

    private void btnRefreshView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentView(this.currentView);
    }

    private void btnSaveView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void cBoxView_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetCurrentView(((FileSystemEntryListItem) this.cBoxView.SelectedItem).Entry);
    }

    private void btnViewFilter_Click(object sender, EventArgs e)
    {
      string text = AdvancedSearchControl.GetFilterSummary(this.tradeEditor.CurrentTradeInfo.Filter.FilterType != TradeFilterType.Advanced ? this.tradeEditor.CurrentTradeInfo.Filter.GetSimpleFilter().ConvertToFilterList() : this.tradeEditor.CurrentTradeInfo.Filter.GetAdvancedFilter());
      if (text == "")
        text = "No criteria has been defined for the search filter.";
      int num = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.None);
    }

    private bool ValidateLoanAssignment(bool checkEligibleCount = false)
    {
      return !(this.tradeEditor is CorrespondentTradeEditor) || !((CorrespondentTradeInfo) this.tradeEditor.CurrentTradeInfo).IsForIndividualLoan() || this.tradeEditor.GetLoanToTradeAssignedLoans().Count <= 0 && this.tradeEditor.GetLoanToTradePendingAssignmentCount() <= 0 && (!checkEligibleCount || this.gvEligible.SelectedItems.Count <= 1);
    }

    private void btnAddLoans_Click(object sender, EventArgs e)
    {
      if (!this.ValidateLoanAssignment(true))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This delivery type does not allow more than one loan to be assigned to the trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.gvEligible.SelectedItems.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans to be assigned to this " + this.GetNameOfTradeOrPool() + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.tradeEditor.CurrentTradeInfo.Status == TradeStatus.Purchased)
        {
          if (Utils.Dialog((IWin32Window) this, "This " + this.GetNameOfTradeOrPool() + " has been marked as purchased. Are you sure you want to assign new loans to the " + this.GetNameOfTradeOrPool() + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
        }
        bool flag1 = false;
        foreach (GVItem selectedItem in this.gvEligible.SelectedItems)
        {
          PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
          bool flag2 = false;
          if (tag.Info[(object) "Trade.TradeType"] != null && (this.tradeEditor is TradeEditor && ((int) tag.Info[(object) "Trade.TradeType"] == 2 || (int) tag.Info[(object) "Trade.TradeType"] == 3) || this.tradeEditor is MbsPoolEditor && ((int) tag.Info[(object) "Trade.TradeType"] == 2 || (int) tag.Info[(object) "Trade.TradeType"] == 3) || this.tradeEditor is FannieMaePEPoolEditor && ((int) tag.Info[(object) "Trade.TradeType"] == 2 || (int) tag.Info[(object) "Trade.TradeType"] == 3) || this.tradeEditor is CorrespondentTradeEditor && (int) tag.Info[(object) "Trade.TradeType"] == 4))
            flag2 = true;
          if (((tag.AssignedTrade == null ? 0 : (tag.AssignedTrade.TradeID != this.tradeEditor.CurrentTradeInfo.TradeID ? 1 : 0)) & (flag2 ? 1 : 0)) != 0)
          {
            flag1 = true;
            break;
          }
        }
        if (flag1)
        {
          if (Utils.Dialog((IWin32Window) this, "One or more of the selected loans is already assigned to another trade or pool. Do you want to remove those loans from their existing trades/pools and assign them to this " + this.GetNameOfTradeOrPool() + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
          this.tradeEditor.RemovePendingLoanFromOtherTrades = true;
        }
        if (this.tradeEditor is CorrespondentTradeEditor && this.ctlEligibleProfit.GetAssignedAmount() > ((CorrespondentTradeEditor) this.tradeEditor).CurrentCorrespondentTradeInfo.MaxAmount)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The assigned amount cannot be more than the maximum amount for the correspondent trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          string str1 = string.Empty;
          string str2 = string.Empty;
          if (this.tradeEditor is FannieMaePEPoolEditor)
          {
            FannieMaePEPoolEditor tradeEditor = (FannieMaePEPoolEditor) this.tradeEditor;
            using (CommitmentAssignLoanToPoolDialog loanToPoolDialog = new CommitmentAssignLoanToPoolDialog(this.gvEligible.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (p => ((PipelineInfo) p.Tag).LoanNumber)).ToList<string>(), tradeEditor.GetFannieMaeProducts(), ((FannieMaePEPoolEditor) this.tradeEditor).GseCommitmentAssignments))
            {
              if (loanToPoolDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              str1 = loanToPoolDialog.CommitmentContractNumber;
              str2 = loanToPoolDialog.ProductName;
            }
          }
          foreach (GVItem selectedItem in this.gvEligible.SelectedItems)
          {
            PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
            this.LoansToAssign = new Dictionary<string, GVItem>();
            this.LoansToAssign.Add(tag.LoanNumber, new GVItem()
            {
              SubItems = {
                (object) tag.LoanNumber,
                (object) str1,
                (object) str2
              }
            });
            if (!this.assignLoanToTrade(tag))
              break;
          }
          this.refreshAssignedLoans();
          this.refreshEligibleLoans();
        }
      }
    }

    private void gvEligible_SelectedIndexChanged(object sender, EventArgs e)
    {
      Dictionary<string, PipelineInfo> dictionary = new Dictionary<string, PipelineInfo>();
      foreach (PipelineInfo pipelineInfo in this.tradeEditor.GetLoanToTradeAssignedPipelineData())
        dictionary[pipelineInfo.GUID] = pipelineInfo;
      int num = 0;
      foreach (GVItem selectedItem in this.gvEligible.SelectedItems)
      {
        PipelineInfo tag = (PipelineInfo) selectedItem.Tag;
        if (!dictionary.ContainsKey(tag.GUID))
          dictionary[tag.GUID] = tag;
        ++num;
      }
      this.ctlEligibleProfit.Calculate(this.tradeEditor.CurrentTradeInfo, (IEnumerable<PipelineInfo>) dictionary.Values);
      this.btnAddLoans.Enabled = num > 0;
    }

    public bool ViewEligibleChecked
    {
      set => this.chkViewEligible.Checked = value;
      get => this.chkViewEligible.Checked;
    }

    private void assignLoansFromLoanNumbers(GVItemCollection loanNumbers)
    {
      if (this.tradeEditor.Assignments == null)
        return;
      bool flag = false;
      List<string> successLoans = new List<string>();
      List<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError> errs = new List<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>();
      string empty = string.Empty;
      List<string> stringList = new List<string>();
      List<PipelineInfo> validPipelineInfos = (List<PipelineInfo>) null;
      try
      {
        string[] rejectedLoanNumbers = this.tradeEditor.GetLoanToTradeAssignedAndRejectedLoanNumbers();
        Dictionary<string, GVItem> dictionary = new Dictionary<string, GVItem>();
        List<string[]> addedLoans = new List<string[]>();
        this.LoansToAssign = new Dictionary<string, GVItem>();
        foreach (GVItem loanNumber in (IEnumerable<GVItem>) loanNumbers)
        {
          if (this.tradeEditor is FannieMaePEPoolEditor)
          {
            string key = loanNumber.SubItems[0].Value as string;
            if (!stringList.Contains(key))
              stringList.Add(loanNumber.SubItems[0].Value as string);
            if (!dictionary.ContainsKey(key))
              dictionary.Add(loanNumber.SubItems[0].Value as string, loanNumber);
            string[] strArray = new string[3]
            {
              loanNumber.SubItems[0].Value as string,
              loanNumber.SubItems[1].Value as string,
              loanNumber.SubItems[2].Value as string
            };
            addedLoans.Add(strArray);
          }
          else if (this.tradeEditor is CorrespondentTradeEditor && ((CorrespondentTradeEditor) this.tradeEditor).IsCalculatedWABP() || this.tradeEditor is TradeEditor && ((TradeEditor) this.tradeEditor).IsCalculatedWABP())
          {
            string upper = (loanNumber.SubItems[0].Value as string).ToUpper();
            if (!stringList.Contains(upper))
              stringList.Add(upper);
            if (!dictionary.ContainsKey(upper))
              dictionary.Add(upper, loanNumber);
            string[] strArray = new string[2]
            {
              upper,
              loanNumber.SubItems[1].Value as string
            };
            addedLoans.Add(strArray);
          }
          else
            stringList.Add(loanNumber.Value as string);
        }
        if (this.tradeEditor is TradeEditor)
        {
          errs = ((LoanToTradeAssignmentManagerBase) this.tradeEditor.Assignments).ValidateLoanBeforeLoanAssignment(stringList.ToArray(), rejectedLoanNumbers, this.tradeEditor.CurrentTradeInfo.InvestorName, string.Empty, false, false, out validPipelineInfos);
          if (((TradeEditor) this.tradeEditor).IsCalculatedWABP())
            errs.AddRange((IEnumerable<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>) ((LoanToTradeAssignmentManagerBase) this.tradeEditor.Assignments).ValidateLoansTotalPriceBeforeLoanAssignment(addedLoans, ref validPipelineInfos));
        }
        else if (this.tradeEditor is MbsPoolEditor || this.tradeEditor is FannieMaePEPoolEditor)
        {
          errs = ((LoanToTradeAssignmentManagerBase) this.tradeEditor.Assignments).ValidateLoanBeforeLoanAssignment(stringList.ToArray(), rejectedLoanNumbers, this.tradeEditor.CurrentTradeInfo.InvestorName, string.Empty, false, false, out validPipelineInfos);
          if (this.tradeEditor is FannieMaePEPoolEditor)
            errs.AddRange((IEnumerable<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>) ((MbsPoolLoanAssignmentManager) this.tradeEditor.Assignments).ValidateCommitmentBeforeLoanAssignment(addedLoans, ((FannieMaePEPoolEditor) this.tradeEditor).GseCommitmentAssignments, ((FannieMaePEPoolEditor) this.tradeEditor).CurrentTradeInfo.ProductNames, ref validPipelineInfos));
        }
        else if (this.tradeEditor is CorrespondentTradeEditor)
        {
          CorrespondentTradeInfo correspondentTradeInfo = ((CorrespondentTradeEditor) this.tradeEditor).CurrentCorrespondentTradeInfo;
          errs = ((LoanToTradeAssignmentManagerBase) this.tradeEditor.Assignments).ValidateLoanBeforeLoanAssignment(stringList.ToArray(), rejectedLoanNumbers, (string) null, correspondentTradeInfo.TPOID, correspondentTradeInfo.IsForIndividualLoan(), correspondentTradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk || correspondentTradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT || correspondentTradeInfo.DeliveryType == CorrespondentMasterDeliveryType.CoIssue, out validPipelineInfos);
          if (((CorrespondentTradeEditor) this.tradeEditor).IsCalculatedWABP())
            errs.AddRange((IEnumerable<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>) ((LoanToTradeAssignmentManagerBase) this.tradeEditor.Assignments).ValidateLoansTotalPriceBeforeLoanAssignment(addedLoans, ref validPipelineInfos));
        }
        successLoans.Clear();
        successLoans.AddRange(validPipelineInfos.Select<PipelineInfo, string>((Func<PipelineInfo, string>) (p => p.LoanNumber)));
        Decimal num = validPipelineInfos.Sum<PipelineInfo>((Func<PipelineInfo, Decimal>) (p => (Decimal) p.Info[(object) "TotalLoanAmount"]));
        if (validPipelineInfos.Count <= 0)
          return;
        if (this.tradeEditor is CorrespondentTradeEditor)
        {
          CorrespondentTradeInfo correspondentTradeInfo = ((CorrespondentTradeEditor) this.tradeEditor).CurrentCorrespondentTradeInfo;
          if (this.ctlProfit.GetOpenAmount() < 0M || num > this.ctlProfit.GetOpenAmount() + (correspondentTradeInfo.MaxAmount - correspondentTradeInfo.TradeAmount))
          {
            string str = "No loan was assigned because the assigned amount cannot be more than the maximum amount of the correspondent trade";
            errs.Clear();
            errs.Add(new EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError(string.Empty, (PipelineInfo) null, str));
            flag = true;
            LoanListScreen.executeComplete(str, validPipelineInfos.Count<PipelineInfo>(), this.GetNameOfTradeOrPool());
            return;
          }
        }
        if (this.tradeEditor is FannieMaePEPoolEditor)
        {
          foreach (string key in validPipelineInfos.Select<PipelineInfo, string>((Func<PipelineInfo, string>) (p => p.LoanNumber)))
          {
            if (dictionary.ContainsKey(key))
              this.LoansToAssign.Add(key, dictionary[key]);
          }
        }
        if (this.tradeEditor is CorrespondentTradeEditor && ((CorrespondentTradeEditor) this.tradeEditor).IsCalculatedWABP() || this.tradeEditor is TradeEditor && ((TradeEditor) this.tradeEditor).IsCalculatedWABP())
        {
          foreach (string key in validPipelineInfos.Select<PipelineInfo, string>((Func<PipelineInfo, string>) (p => p.LoanNumber)))
          {
            if (dictionary.ContainsKey(key.ToUpper()))
              this.LoansToAssign.Add(key, dictionary[key.ToUpper()]);
          }
        }
        List<TradeLoanUpdateError> trade = this.assignLoansToTrade(validPipelineInfos.Select<PipelineInfo, string>((Func<PipelineInfo, string>) (p => p.GUID)).ToArray<string>());
        errs.AddRange(trade.Select<TradeLoanUpdateError, EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>((Func<TradeLoanUpdateError, EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError>) (err => new EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError()
        {
          LoanGuid = err.LoanGuid,
          LoanInfo = err.LoanInfo,
          Message = err.Message
        })));
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        if (!flag)
          LoanListScreen.executeComplete(successLoans, errs, this.GetNameOfTradeOrPool());
      }
    }

    private static void executeComplete(
      List<string> successLoans,
      List<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError> errs,
      string nameOfTradeOrPool)
    {
      string detailMessage = LoanListScreen.buildCompletionMessage(errs, successLoans.Count, errs.Count, nameOfTradeOrPool);
      int num = (int) new TradeLoanUpdateCompleteDialog(successLoans.Count, errs.Count, detailMessage, ProcessType.Assignment).ShowDialog();
    }

    private static void executeComplete(string error, int loanCount, string nameOfTradeOrPool)
    {
      int num = (int) new TradeLoanUpdateCompleteDialog(loanCount, error, ProcessType.Assignment).ShowDialog();
    }

    private static string buildCompletionMessage(
      List<EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError> errors,
      int successCount,
      int errorCount,
      string nameOfTradeOrPool)
    {
      string str = "";
      if (errors.Count > 0)
      {
        foreach (EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateError error in errors)
        {
          if (error.LoanInfo == null && string.IsNullOrEmpty(error.LoanGuid))
            str = str + error.Message + Environment.NewLine + Environment.NewLine;
          else
            str = str + "Loan " + (error.LoanInfo == null ? error.LoanGuid : error.LoanInfo.LoanNumber) + ": " + error.Message + Environment.NewLine + Environment.NewLine;
        }
      }
      else
        str = "All loans were assigned to the " + nameOfTradeOrPool + " successfully.";
      return str;
    }

    private string GetNameOfTradeOrPool()
    {
      if (this.tradeEditor is MbsPoolEditor)
        return "MBS Pool";
      if (this.tradeEditor is TradeEditor)
        return "Loan Trade";
      if (this.tradeEditor is CorrespondentTradeEditor)
        return "Correspondent Trade";
      return this.tradeEditor is FannieMaePEPoolEditor ? "Fannie Mae PE MBS Pool" : "trade/pool";
    }

    private void btnMarkAsPurchased_Click(object sender, EventArgs e)
    {
      if (this.gvLoans.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more loans from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "If the purchase date for the selected loan(s) has not been assigned, today's date will be used.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) != DialogResult.OK)
          return;
        foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
          this.tradeEditor.MarkLoanToTradeAssignmentStatusToPurchasedPending(((LoanToTradeAssignmentBase) selectedItem.Tag).Guid);
        this.refreshAssignedLoans();
      }
    }

    private void btnExtendSelectedLoan_Click(object sender, EventArgs e)
    {
      string str1 = string.Empty;
      if (this.tradeEditor.LoanUpdatesRequired)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, string.Format("Loans cannot be extended when there are unsaved changes for the correspondent trade."), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        foreach (GVItem gvItem in this.gvLoans.Items.Where<GVItem>((Func<GVItem, bool>) (t => t.Selected || t.BackColor == SystemColors.Highlight)).ToArray<GVItem>())
        {
          CorrespondentTradeLoanAssignment tag = (CorrespondentTradeLoanAssignment) gvItem.Tag;
          if (tag != null && (tag.PendingStatus != CorrespondentTradeLoanStatus.None || tag.AssignedStatus != CorrespondentTradeLoanStatus.Assigned))
            str1 = str1 + tag.PipelineInfo.LoanNumber + ",";
        }
        if (str1.Length > 0)
        {
          string str2 = str1.Substring(0, str1.Length - 1);
          if (str2.IndexOf(',') > 0)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, string.Format("Loans {0} were not extended because the loans are in a Pending assignment status.", (object) str2), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
          else
          {
            int num3 = (int) Utils.Dialog((IWin32Window) this, string.Format("Loan {0} was not extended because the loan is in a Pending assignment status.", (object) str2), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        else
        {
          ExtensionRequestDialog extensionRequestDialog = new ExtensionRequestDialog(((CorrespondentTradeEditor) this.tradeEditor).CurrentCorrespondentTradeInfo.ExpirationDate, Session.DefaultInstance);
          int num4 = (int) extensionRequestDialog.ShowDialog((IWin32Window) this);
          if (extensionRequestDialog.DialogResult != DialogResult.OK)
            return;
          foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
            ((CorrespondentTradeLoanAssignment) selectedItem.Tag).TradeExtensionInfo = extensionRequestDialog.TradeExtensionInfo;
          if (this.tradeEditor.DataModified)
          {
            if (Utils.Dialog((IWin32Window) this, "The current " + this.GetNameOfTradeOrPool() + " has unsaved changes." + Environment.NewLine + "You must save these changes before you update loans." + Environment.NewLine + "Do you want to save the " + this.GetNameOfTradeOrPool() + " and extend selected loans now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
              return;
            this.tradeEditor.SaveTrade(false, false);
          }
          ((CorrespondentTradeEditor) this.tradeEditor).CommitSelectedLoansToTradeExtension(extensionRequestDialog.TradeExtensionInfo);
        }
      }
    }

    private void btnUpdateSelectedLoans_Click(object sender, EventArgs e)
    {
      if (this.tradeEditor.DataModified)
      {
        if (Utils.Dialog((IWin32Window) this, "The current " + this.GetNameOfTradeOrPool() + " has unsaved changes." + Environment.NewLine + "You must save these changes before you update loans." + Environment.NewLine + "Do you want to save the " + this.GetNameOfTradeOrPool() + " and update loans now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.tradeEditor.SaveTrade(true, true);
      }
      else
      {
        this.tradeEditor.CommitLoanToTradeAssignments(true, true);
        this.refreshAssignedLoans();
      }
    }

    private void RefreshWeightedAvgBulkPrice()
    {
      if (this.tradeEditor is CorrespondentTradeEditor)
        ((CorrespondentTradeEditor) this.tradeEditor).RefreshWeightedAvgBulkPrice();
      if (!(this.tradeEditor is TradeEditor))
        return;
      ((TradeEditor) this.tradeEditor).RefreshWeightAvgBulkPrice();
    }

    public void DisableViewEligible()
    {
      this.chkViewEligible.Visible = false;
      this.chkViewEligible.Enabled = false;
      this.showViewEligible = false;
      this.chkViewEligible.Checked = false;
      this.AdjustLoanPanelDisplay();
    }

    public void EnableViewEligible()
    {
      this.chkViewEligible.Visible = true;
      this.chkViewEligible.Enabled = true;
      this.showViewEligible = true;
    }

    public void ResetWithdrawnLoanMessageFlag() => this.withdrawnLoanMessageDisplayed = false;

    private void SetCorrespondentUpdateButtonAttributes()
    {
      this.btnUpdateSelectedLoans.Enabled = !this.readOnly && (this.IsAnyLoanPending() || this.gvLoans.SelectedItems.Count<GVItem>() > 0 && this.tradeEditor.LoanUpdatesRequired);
      this.btnUpdateLoans.Enabled = !this.readOnly && this.tradeEditor.LoanUpdatesRequired;
      this.btnExtendSelectedLoan.Enabled = !this.readOnly && this.gvLoans.SelectedItems.Count<GVItem>() > 0;
      foreach (GVItem selectedItem in this.gvLoans.SelectedItems)
      {
        LoanToTradeAssignmentBase tag = (LoanToTradeAssignmentBase) selectedItem.Tag;
        this.btnExtendSelectedLoan.Enabled = !this.readOnly && ((IEnumerable<string>) ((CorrespondentTradeLoanAssignmentManager) this.tradeEditor.Assignments).GetAssignedStatusLoanNumbers()).Contains<string>(tag.PipelineInfo.LoanNumber);
        if (this.btnExtendSelectedLoan.Enabled)
          break;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      GVColumn gvColumn18 = new GVColumn();
      GVColumn gvColumn19 = new GVColumn();
      GVColumn gvColumn20 = new GVColumn();
      GVColumn gvColumn21 = new GVColumn();
      GVColumn gvColumn22 = new GVColumn();
      GVColumn gvColumn23 = new GVColumn();
      GVColumn gvColumn24 = new GVColumn();
      GVColumn gvColumn25 = new GVColumn();
      GVColumn gvColumn26 = new GVColumn();
      GVColumn gvColumn27 = new GVColumn();
      GVColumn gvColumn28 = new GVColumn();
      GVColumn gvColumn29 = new GVColumn();
      GVColumn gvColumn30 = new GVColumn();
      GVColumn gvColumn31 = new GVColumn();
      GVColumn gvColumn32 = new GVColumn();
      GVColumn gvColumn33 = new GVColumn();
      GVColumn gvColumn34 = new GVColumn();
      GVColumn gvColumn35 = new GVColumn();
      GVColumn gvColumn36 = new GVColumn();
      GVColumn gvColumn37 = new GVColumn();
      GVColumn gvColumn38 = new GVColumn();
      GVColumn gvColumn39 = new GVColumn();
      this.pnlLoans = new Panel();
      this.grpLoans = new TableContainer();
      this.chkImport = new CheckBox();
      this.chkViewEligible = new CheckBox();
      this.ctlProfit = new TradeProfitControl();
      this.gvLoans = new GridView();
      this.flpLoans = new FlowLayoutPanel();
      this.btnMarkAsShipped = new Button();
      this.btnMarkAsPurchased = new Button();
      this.btnExtendSelectedLoan = new ButtonEx();
      this.btnUpdateLoans = new ButtonEx();
      this.btnUpdateSelectedLoans = new ButtonEx();
      this.verticalSeparator2 = new VerticalSeparator();
      this.btnExportLoans = new StandardIconButton();
      this.btnRemoveLoans = new StandardIconButton();
      this.gradientPanel2 = new GradientPanel();
      this.btnEditView = new StandardIconButton();
      this.btnRefreshView = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.label22 = new Label();
      this.cBoxView = new ComboBoxEx();
      this.pnlEligibleLoans = new Panel();
      this.tableContainer1 = new TableContainer();
      this.ctlEligibleProfit = new TradeProfitControl();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnViewFilter = new Button();
      this.btnAddLoans = new Button();
      this.gvEligible = new GridView();
      this.pnlLoans.SuspendLayout();
      this.grpLoans.SuspendLayout();
      this.flpLoans.SuspendLayout();
      ((ISupportInitialize) this.btnExportLoans).BeginInit();
      ((ISupportInitialize) this.btnRemoveLoans).BeginInit();
      this.gradientPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnEditView).BeginInit();
      ((ISupportInitialize) this.btnRefreshView).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      this.pnlEligibleLoans.SuspendLayout();
      this.tableContainer1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.SuspendLayout();
      this.pnlLoans.Controls.Add((Control) this.grpLoans);
      this.pnlLoans.Controls.Add((Control) this.gradientPanel2);
      this.pnlLoans.Controls.Add((Control) this.pnlEligibleLoans);
      this.pnlLoans.Dock = DockStyle.Fill;
      this.pnlLoans.Location = new Point(0, 0);
      this.pnlLoans.Name = "pnlLoans";
      this.pnlLoans.Size = new Size(1282, 726);
      this.pnlLoans.TabIndex = 10;
      this.grpLoans.Controls.Add((Control) this.chkImport);
      this.grpLoans.Controls.Add((Control) this.chkViewEligible);
      this.grpLoans.Controls.Add((Control) this.ctlProfit);
      this.grpLoans.Controls.Add((Control) this.gvLoans);
      this.grpLoans.Controls.Add((Control) this.flpLoans);
      this.grpLoans.Dock = DockStyle.Fill;
      this.grpLoans.Location = new Point(0, 31);
      this.grpLoans.Name = "grpLoans";
      this.grpLoans.Size = new Size(1282, 370);
      this.grpLoans.TabIndex = 6;
      this.grpLoans.Text = "Currently in Correspondent Trade";
      this.chkImport.AutoSize = true;
      this.chkImport.BackColor = Color.Transparent;
      this.chkImport.Location = new Point(319, 6);
      this.chkImport.Name = "chkImport";
      this.chkImport.Size = new Size(198, 17);
      this.chkImport.TabIndex = 7;
      this.chkImport.Text = "Assign loans from a file/manual input";
      this.chkImport.UseVisualStyleBackColor = false;
      this.chkImport.Click += new EventHandler(this.chkImport_CheckedChanged);
      this.chkViewEligible.AutoSize = true;
      this.chkViewEligible.BackColor = Color.Transparent;
      this.chkViewEligible.Location = new Point(201, 6);
      this.chkViewEligible.Name = "chkViewEligible";
      this.chkViewEligible.Size = new Size(112, 17);
      this.chkViewEligible.TabIndex = 6;
      this.chkViewEligible.Text = "View eligible loans";
      this.chkViewEligible.UseVisualStyleBackColor = false;
      this.chkViewEligible.Click += new EventHandler(this.chkViewEligible_CheckedChanged);
      this.ctlProfit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlProfit.BackColor = Color.Transparent;
      this.ctlProfit.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlProfit.Location = new Point(9, 348);
      this.ctlProfit.Name = "ctlProfit";
      this.ctlProfit.Size = new Size(1268, 18);
      this.ctlProfit.TabIndex = 0;
      this.gvLoans.AllowColumnReorder = true;
      this.gvLoans.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "";
      gvColumn1.Width = 28;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Tag = (object) "TradeAssignment.Status";
      gvColumn2.Text = "Status";
      gvColumn2.Width = 79;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Tag = (object) "TradeAssignment.StatusDate";
      gvColumn3.Text = "Date";
      gvColumn3.Width = 83;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Tag = (object) "Loan.LoanNumber";
      gvColumn4.Text = "Loan Number";
      gvColumn4.Width = 150;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Tag = (object) "Loan.BorrowerLastName";
      gvColumn5.Text = "Last Name";
      gvColumn5.Width = 104;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Tag = (object) "Loan.TotalBuyPrice";
      gvColumn6.Text = "Loan Trade Total Buy Price";
      gvColumn6.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn6.Width = 95;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Tag = (object) "";
      gvColumn7.Text = "Loan Trade Total Sell Price";
      gvColumn7.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn7.Width = 94;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column8";
      gvColumn8.Text = "Gain/Loss";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 71;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column9";
      gvColumn9.Tag = (object) "Loan.LoanProgram";
      gvColumn9.Text = "Loan Program";
      gvColumn9.Width = 94;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column10";
      gvColumn10.Tag = (object) "Loan.TotalLoanAmount";
      gvColumn10.Text = "Loan Amount";
      gvColumn10.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn10.Width = 81;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column11";
      gvColumn11.Tag = (object) "Loan.LoanRate";
      gvColumn11.Text = "Note Rate";
      gvColumn11.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn11.Width = 68;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column12";
      gvColumn12.Tag = (object) "Loan.Term";
      gvColumn12.Text = "Term";
      gvColumn12.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn12.Width = 56;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column13";
      gvColumn13.Tag = (object) "Loan.LTV";
      gvColumn13.Text = "LTV";
      gvColumn13.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn13.Width = 52;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column14";
      gvColumn14.Tag = (object) "Loan.CLTV";
      gvColumn14.Text = "CLTV";
      gvColumn14.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn14.Width = 100;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column15";
      gvColumn15.Tag = (object) "Loan.DTITop";
      gvColumn15.Text = "Top";
      gvColumn15.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn15.Width = 50;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "Column16";
      gvColumn16.Tag = (object) "Loan.DTIBottom";
      gvColumn16.Text = "Bottom";
      gvColumn16.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn16.Width = 100;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "Column17";
      gvColumn17.Tag = (object) "Loan.CreditScore";
      gvColumn17.Text = "FICO";
      gvColumn17.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn17.Width = 100;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "Column18";
      gvColumn18.Tag = (object) "Loan.OccupancyStatus";
      gvColumn18.Text = "Occupancy Type";
      gvColumn18.Width = 101;
      gvColumn19.ImageIndex = -1;
      gvColumn19.Name = "Column19";
      gvColumn19.Tag = (object) "Loan.PropertyType";
      gvColumn19.Text = "Property Type";
      gvColumn19.Width = 82;
      gvColumn20.ImageIndex = -1;
      gvColumn20.Name = "Column20";
      gvColumn20.Tag = (object) "Loan.State";
      gvColumn20.Text = "State";
      gvColumn20.Width = 100;
      gvColumn21.ImageIndex = -1;
      gvColumn21.Name = "Column21";
      gvColumn21.Tag = (object) "Loan.LockExpirationDate";
      gvColumn21.Text = "Lock Expiration Date";
      gvColumn21.Width = 124;
      this.gvLoans.Columns.AddRange(new GVColumn[21]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15,
        gvColumn16,
        gvColumn17,
        gvColumn18,
        gvColumn19,
        gvColumn20,
        gvColumn21
      });
      this.gvLoans.Dock = DockStyle.Fill;
      this.gvLoans.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvLoans.Location = new Point(1, 26);
      this.gvLoans.Name = "gvLoans";
      this.gvLoans.Size = new Size(1280, 318);
      this.gvLoans.TabIndex = 5;
      this.gvLoans.SelectedIndexChanged += new EventHandler(this.gvLoans_SelectedIndexChanged);
      this.gvLoans.ColumnClick += new GVColumnClickEventHandler(this.gvLoans_ColumnClick);
      this.gvLoans.ColumnReorder += new GVColumnEventHandler(this.gvLoans_ColumnReorder);
      this.gvLoans.ColumnResize += new GVColumnEventHandler(this.gvLoans_ColumnResize);
      this.gvLoans.ItemDoubleClick += new GVItemEventHandler(this.gvLoans_ItemDoubleClick);
      this.flpLoans.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpLoans.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.flpLoans.BackColor = Color.Transparent;
      this.flpLoans.Controls.Add((Control) this.btnMarkAsPurchased);
      this.flpLoans.Controls.Add((Control) this.btnMarkAsShipped);
      this.flpLoans.Controls.Add((Control) this.btnExtendSelectedLoan);
      this.flpLoans.Controls.Add((Control) this.btnUpdateLoans);
      this.flpLoans.Controls.Add((Control) this.btnUpdateSelectedLoans);
      this.flpLoans.Controls.Add((Control) this.verticalSeparator2);
      this.flpLoans.Controls.Add((Control) this.btnExportLoans);
      this.flpLoans.Controls.Add((Control) this.btnRemoveLoans);
      this.flpLoans.FlowDirection = FlowDirection.RightToLeft;
      this.flpLoans.Location = new Point(502, 2);
      this.flpLoans.Name = "flpLoans";
      this.flpLoans.Size = new Size(774, 23);
      this.flpLoans.TabIndex = 0;
      this.btnMarkAsShipped.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMarkAsShipped.BackColor = SystemColors.Control;
      this.btnMarkAsShipped.Enabled = false;
      this.btnMarkAsShipped.Location = new Point(513, 0);
      this.btnMarkAsShipped.Margin = new Padding(0);
      this.btnMarkAsShipped.Name = "btnMarkAsShipped";
      this.btnMarkAsShipped.Padding = new Padding(2, 0, 0, 0);
      this.btnMarkAsShipped.Size = new Size(143, 23);
      this.btnMarkAsShipped.TabIndex = 3;
      this.btnMarkAsShipped.Text = "Mark As &Shipped";
      this.btnMarkAsShipped.UseVisualStyleBackColor = true;
      this.btnMarkAsShipped.Click += new EventHandler(this.btnMarkAsShipped_Click);
      this.btnMarkAsPurchased.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMarkAsPurchased.BackColor = SystemColors.Control;
      this.btnMarkAsPurchased.Enabled = false;
      this.btnMarkAsPurchased.Location = new Point(656, 0);
      this.btnMarkAsPurchased.Margin = new Padding(0);
      this.btnMarkAsPurchased.Name = "btnMarkAsPurchased";
      this.btnMarkAsPurchased.Padding = new Padding(2, 0, 0, 0);
      this.btnMarkAsPurchased.Size = new Size(118, 22);
      this.btnMarkAsPurchased.TabIndex = 7;
      this.btnMarkAsPurchased.Text = "Mark As &Purchased";
      this.btnMarkAsPurchased.UseVisualStyleBackColor = true;
      this.btnMarkAsPurchased.Click += new EventHandler(this.btnMarkAsPurchased_Click);
      this.btnExtendSelectedLoan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExtendSelectedLoan.BackColor = SystemColors.Control;
      this.btnExtendSelectedLoan.Location = new Point(359, 0);
      this.btnExtendSelectedLoan.Margin = new Padding(0);
      this.btnExtendSelectedLoan.Name = "btnExtendSelectedLoan";
      this.btnExtendSelectedLoan.Padding = new Padding(2, 0, 0, 0);
      this.btnExtendSelectedLoan.Size = new Size(154, 23);
      this.btnExtendSelectedLoan.TabIndex = 9;
      this.btnExtendSelectedLoan.Text = "Extend Selected Loans";
      this.btnExtendSelectedLoan.UseVisualStyleBackColor = true;
      this.btnExtendSelectedLoan.Click += new EventHandler(this.btnExtendSelectedLoan_Click);
      this.btnUpdateLoans.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpdateLoans.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.btnUpdateLoans.BackColor = SystemColors.Control;
      this.btnUpdateLoans.Location = new Point(235, 0);
      this.btnUpdateLoans.Margin = new Padding(0);
      this.btnUpdateLoans.Name = "btnUpdateLoans";
      this.btnUpdateLoans.Padding = new Padding(2, 0, 0, 0);
      this.btnUpdateLoans.Size = new Size(124, 22);
      this.btnUpdateLoans.TabIndex = 3;
      this.btnUpdateLoans.Text = "Update All Loans";
      this.btnUpdateLoans.UseVisualStyleBackColor = true;
      this.btnUpdateLoans.Click += new EventHandler(this.btnUpdateLoans_Click);
      this.btnUpdateSelectedLoans.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpdateSelectedLoans.BackColor = SystemColors.Control;
      this.btnUpdateSelectedLoans.Location = new Point(81, 0);
      this.btnUpdateSelectedLoans.Margin = new Padding(0);
      this.btnUpdateSelectedLoans.Name = "btnUpdateSelectedLoans";
      this.btnUpdateSelectedLoans.Padding = new Padding(2, 0, 0, 0);
      this.btnUpdateSelectedLoans.Size = new Size(154, 22);
      this.btnUpdateSelectedLoans.TabIndex = 8;
      this.btnUpdateSelectedLoans.Text = "Update Selected Loans";
      this.btnUpdateSelectedLoans.UseVisualStyleBackColor = true;
      this.btnUpdateSelectedLoans.Click += new EventHandler(this.btnUpdateSelectedLoans_Click);
      this.verticalSeparator2.Location = new Point(76, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 4;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.btnExportLoans.BackColor = Color.Transparent;
      this.btnExportLoans.Enabled = false;
      this.btnExportLoans.Location = new Point(55, 3);
      this.btnExportLoans.Margin = new Padding(3, 3, 2, 3);
      this.btnExportLoans.MouseDownImage = (Image) null;
      this.btnExportLoans.Name = "btnExportLoans";
      this.btnExportLoans.Size = new Size(16, 16);
      this.btnExportLoans.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExportLoans.TabIndex = 5;
      this.btnExportLoans.TabStop = false;
      this.btnExportLoans.Click += new EventHandler(this.btnExportLoans_Click);
      this.btnRemoveLoans.BackColor = Color.Transparent;
      this.btnRemoveLoans.Enabled = false;
      this.btnRemoveLoans.Location = new Point(35, 3);
      this.btnRemoveLoans.Margin = new Padding(3, 3, 1, 3);
      this.btnRemoveLoans.MouseDownImage = (Image) null;
      this.btnRemoveLoans.Name = "btnRemoveLoans";
      this.btnRemoveLoans.Size = new Size(16, 16);
      this.btnRemoveLoans.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnRemoveLoans.TabIndex = 6;
      this.btnRemoveLoans.TabStop = false;
      this.btnRemoveLoans.Click += new EventHandler(this.btnRemoveLoans_Click);
      this.gradientPanel2.BackColorGlassyStyle = true;
      this.gradientPanel2.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel2.Controls.Add((Control) this.btnEditView);
      this.gradientPanel2.Controls.Add((Control) this.btnRefreshView);
      this.gradientPanel2.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel2.Controls.Add((Control) this.label22);
      this.gradientPanel2.Controls.Add((Control) this.cBoxView);
      this.gradientPanel2.Dock = DockStyle.Top;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel2.Location = new Point(0, 0);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(1282, 31);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel2.TabIndex = 9;
      this.btnEditView.BackColor = Color.Transparent;
      this.btnEditView.Location = new Point(319, 9);
      this.btnEditView.MouseDownImage = (Image) null;
      this.btnEditView.Name = "btnEditView";
      this.btnEditView.Size = new Size(16, 16);
      this.btnEditView.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnEditView.TabIndex = 10;
      this.btnEditView.TabStop = false;
      this.btnEditView.Click += new EventHandler(this.btnEditView_Click);
      this.btnRefreshView.BackColor = Color.Transparent;
      this.btnRefreshView.Enabled = false;
      this.btnRefreshView.Location = new Point(297, 9);
      this.btnRefreshView.MouseDownImage = (Image) null;
      this.btnRefreshView.Name = "btnRefreshView";
      this.btnRefreshView.Size = new Size(16, 16);
      this.btnRefreshView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefreshView.TabIndex = 9;
      this.btnRefreshView.TabStop = false;
      this.btnRefreshView.Click += new EventHandler(this.btnRefreshView_Click);
      this.btnSaveView.BackColor = Color.Transparent;
      this.btnSaveView.Enabled = false;
      this.btnSaveView.Location = new Point(275, 9);
      this.btnSaveView.MouseDownImage = (Image) null;
      this.btnSaveView.Name = "btnSaveView";
      this.btnSaveView.Size = new Size(16, 16);
      this.btnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveView.TabIndex = 8;
      this.btnSaveView.TabStop = false;
      this.btnSaveView.Click += new EventHandler(this.btnSaveView_Click);
      this.label22.AutoSize = true;
      this.label22.BackColor = Color.Transparent;
      this.label22.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label22.Location = new Point(14, 9);
      this.label22.Name = "label22";
      this.label22.Size = new Size(72, 14);
      this.label22.TabIndex = 5;
      this.label22.Text = "Loans View";
      this.cBoxView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cBoxView.FormattingEnabled = true;
      this.cBoxView.Items.AddRange(new object[2]
      {
        (object) "Current Contracts",
        (object) "Archived Contracts"
      });
      this.cBoxView.Location = new Point(110, 6);
      this.cBoxView.Name = "cBoxView";
      this.cBoxView.SelectedBGColor = SystemColors.Highlight;
      this.cBoxView.Size = new Size(158, 21);
      this.cBoxView.TabIndex = 7;
      this.cBoxView.SelectedIndexChanged += new EventHandler(this.cBoxView_SelectedIndexChanged);
      this.pnlEligibleLoans.Controls.Add((Control) this.tableContainer1);
      this.pnlEligibleLoans.Dock = DockStyle.Bottom;
      this.pnlEligibleLoans.Location = new Point(0, 401);
      this.pnlEligibleLoans.Name = "pnlEligibleLoans";
      this.pnlEligibleLoans.Padding = new Padding(0, 5, 0, 0);
      this.pnlEligibleLoans.Size = new Size(1282, 325);
      this.pnlEligibleLoans.TabIndex = 8;
      this.tableContainer1.Controls.Add((Control) this.ctlEligibleProfit);
      this.tableContainer1.Controls.Add((Control) this.flowLayoutPanel2);
      this.tableContainer1.Controls.Add((Control) this.gvEligible);
      this.tableContainer1.Dock = DockStyle.Fill;
      this.tableContainer1.Location = new Point(0, 5);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(1282, 320);
      this.tableContainer1.TabIndex = 7;
      this.tableContainer1.Text = "Eligible Loans";
      this.ctlEligibleProfit.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.ctlEligibleProfit.BackColor = Color.Transparent;
      this.ctlEligibleProfit.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlEligibleProfit.Location = new Point(9, 298);
      this.ctlEligibleProfit.Name = "ctlEligibleProfit";
      this.ctlEligibleProfit.Size = new Size(1268, 18);
      this.ctlEligibleProfit.TabIndex = 0;
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnViewFilter);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnAddLoans);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(96, 2);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(1180, 22);
      this.flowLayoutPanel2.TabIndex = 15;
      this.btnViewFilter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnViewFilter.BackColor = SystemColors.Control;
      this.btnViewFilter.Location = new Point(1095, 0);
      this.btnViewFilter.Margin = new Padding(0);
      this.btnViewFilter.Name = "btnViewFilter";
      this.btnViewFilter.Size = new Size(85, 22);
      this.btnViewFilter.TabIndex = 16;
      this.btnViewFilter.Text = "&View Filter";
      this.btnViewFilter.UseVisualStyleBackColor = true;
      this.btnViewFilter.Click += new EventHandler(this.btnViewFilter_Click);
      this.btnAddLoans.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddLoans.BackColor = SystemColors.Control;
      this.btnAddLoans.Enabled = false;
      this.btnAddLoans.Location = new Point(1010, 0);
      this.btnAddLoans.Margin = new Padding(0);
      this.btnAddLoans.Name = "btnAddLoans";
      this.btnAddLoans.Size = new Size(85, 22);
      this.btnAddLoans.TabIndex = 15;
      this.btnAddLoans.Text = "&Assign Loans";
      this.btnAddLoans.UseVisualStyleBackColor = true;
      this.btnAddLoans.Click += new EventHandler(this.btnAddLoans_Click);
      this.gvEligible.AllowColumnReorder = true;
      this.gvEligible.BorderStyle = BorderStyle.None;
      gvColumn22.ImageIndex = -1;
      gvColumn22.Name = "Column1";
      gvColumn22.Tag = (object) "TradeAssignment.Status";
      gvColumn22.Text = "Status";
      gvColumn22.Width = 79;
      gvColumn23.ImageIndex = -1;
      gvColumn23.Name = "Column2";
      gvColumn23.Tag = (object) "Trade.Name";
      gvColumn23.Text = "Trade #";
      gvColumn23.Width = 83;
      gvColumn24.ImageIndex = -1;
      gvColumn24.Name = "Column3";
      gvColumn24.SpringToFit = true;
      gvColumn24.Tag = (object) "Loan.LoanNumber";
      gvColumn24.Text = "Loan Number";
      gvColumn24.Width = 3;
      gvColumn25.ImageIndex = -1;
      gvColumn25.Name = "Column4";
      gvColumn25.Tag = (object) "Loan.BorrowerLastName";
      gvColumn25.Text = "Last Name";
      gvColumn25.Width = 104;
      gvColumn26.ImageIndex = -1;
      gvColumn26.Name = "Column5";
      gvColumn26.Tag = (object) "Loan.TotalBuyPrice";
      gvColumn26.Text = "Loan Trade Total Buy Price";
      gvColumn26.Width = 150;
      gvColumn27.ImageIndex = -1;
      gvColumn27.Name = "Column6";
      gvColumn27.Tag = (object) "";
      gvColumn27.Text = "Loan Trade Total Sell Price";
      gvColumn27.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn27.Width = 150;
      gvColumn28.ImageIndex = -1;
      gvColumn28.Name = "Column7";
      gvColumn28.Text = "Gain/Loss";
      gvColumn28.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn28.Width = 71;
      gvColumn29.ImageIndex = -1;
      gvColumn29.Name = "Column8";
      gvColumn29.Tag = (object) "Loan.LoanProgram";
      gvColumn29.Text = "Loan Program";
      gvColumn29.Width = 94;
      gvColumn30.ImageIndex = -1;
      gvColumn30.Name = "Column9";
      gvColumn30.Tag = (object) "Loan.TotalLoanAmount";
      gvColumn30.Text = "Loan Amount";
      gvColumn30.Width = 81;
      gvColumn31.ImageIndex = -1;
      gvColumn31.Name = "Column10";
      gvColumn31.Tag = (object) "Loan.LoanRate";
      gvColumn31.Text = "Note Rate";
      gvColumn31.Width = 68;
      gvColumn32.ImageIndex = -1;
      gvColumn32.Name = "Column11";
      gvColumn32.Tag = (object) "Loan.Term";
      gvColumn32.Text = "Term";
      gvColumn32.Width = 56;
      gvColumn33.ImageIndex = -1;
      gvColumn33.Name = "Column12";
      gvColumn33.Tag = (object) "Loan.LTV";
      gvColumn33.Text = "LTV";
      gvColumn33.Width = 52;
      gvColumn34.ImageIndex = -1;
      gvColumn34.Name = "Column13";
      gvColumn34.Tag = (object) "Loan.DTITop";
      gvColumn34.Text = "DTI";
      gvColumn34.Width = 50;
      gvColumn35.ImageIndex = -1;
      gvColumn35.Name = "Column14";
      gvColumn35.Tag = (object) "Loan.CreditScore";
      gvColumn35.Text = "FICO";
      gvColumn35.Width = 100;
      gvColumn36.ImageIndex = -1;
      gvColumn36.Name = "Column15";
      gvColumn36.Tag = (object) "Loan.OccupancyStatus";
      gvColumn36.Text = "Occupancy Type";
      gvColumn36.Width = 101;
      gvColumn37.ImageIndex = -1;
      gvColumn37.Name = "Column16";
      gvColumn37.Tag = (object) "Loan.PropertyType";
      gvColumn37.Text = "Property Type";
      gvColumn37.Width = 82;
      gvColumn38.ImageIndex = -1;
      gvColumn38.Name = "Column17";
      gvColumn38.Tag = (object) "Loan.State";
      gvColumn38.Text = "State";
      gvColumn38.Width = 100;
      gvColumn39.ImageIndex = -1;
      gvColumn39.Name = "Column18";
      gvColumn39.Tag = (object) "Loan.LockExpirationDate";
      gvColumn39.Text = "Lock Expiration Date";
      gvColumn39.Width = 124;
      this.gvEligible.Columns.AddRange(new GVColumn[18]
      {
        gvColumn22,
        gvColumn23,
        gvColumn24,
        gvColumn25,
        gvColumn26,
        gvColumn27,
        gvColumn28,
        gvColumn29,
        gvColumn30,
        gvColumn31,
        gvColumn32,
        gvColumn33,
        gvColumn34,
        gvColumn35,
        gvColumn36,
        gvColumn37,
        gvColumn38,
        gvColumn39
      });
      this.gvEligible.Dock = DockStyle.Fill;
      this.gvEligible.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvEligible.Location = new Point(1, 26);
      this.gvEligible.Name = "gvEligible";
      this.gvEligible.Size = new Size(1280, 268);
      this.gvEligible.SortOption = GVSortOption.Owner;
      this.gvEligible.TabIndex = 13;
      this.gvEligible.SelectedIndexChanged += new EventHandler(this.gvEligible_SelectedIndexChanged);
      this.gvEligible.SortItems += new GVColumnSortEventHandler(this.gvEligible_SortItems);
      this.gvEligible.ItemDoubleClick += new GVItemEventHandler(this.gvEligible_ItemDoubleClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.pnlLoans);
      this.Name = nameof (LoanListScreen);
      this.Size = new Size(1282, 726);
      this.pnlLoans.ResumeLayout(false);
      this.grpLoans.ResumeLayout(false);
      this.grpLoans.PerformLayout();
      this.flpLoans.ResumeLayout(false);
      this.flpLoans.PerformLayout();
      ((ISupportInitialize) this.btnExportLoans).EndInit();
      ((ISupportInitialize) this.btnRemoveLoans).EndInit();
      this.gradientPanel2.ResumeLayout(false);
      this.gradientPanel2.PerformLayout();
      ((ISupportInitialize) this.btnEditView).EndInit();
      ((ISupportInitialize) this.btnRefreshView).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      this.pnlEligibleLoans.ResumeLayout(false);
      this.tableContainer1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
