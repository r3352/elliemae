// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.ReportFieldDefinitions;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class SecurityTradeEditor : UserControl, IMenuProvider, ITradeEditorBase
  {
    private string className = nameof (SecurityTradeEditor);
    private const int ControlPadding = 5;
    private static SecurityLoanTradeStatusEnumNameProvider tradeStatusNameProvider = new SecurityLoanTradeStatusEnumNameProvider();
    private static string sw = Tracing.SwOutsideLoan;
    public static Color AlertColor = Color.FromArgb(204, 51, 51);
    public static Color HighlightColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 231);
    private SecurityTradeInfo trade;
    private bool modified;
    private string originalTradeName;
    private MbsPoolAssignmentListScreen ctlMbsPoolList;
    private Dictionary<int, LoanTradeViewModel> removedTrade = new Dictionary<int, LoanTradeViewModel>();
    private bool readOnly;
    private bool suspendEvents;
    private TableLayout tradeLoanTradeTableLayout;
    private GridViewLayoutManager gvLayoutManager;
    private string tradeLoanTradeTableLayoutFileName = "TradeLoanTradeView";
    private TradeAssignedLoanTradeFieldDefs tradeAssLoanTradeFieldDefs;
    private IContainer components;
    private TabControl tabTrade;
    private TabPage tpDetails;
    private TabPage tpHistory;
    private GroupContainer grpTradeInfo;
    private Label label8;
    private Label label4;
    private Label label3;
    private TextBox txtName;
    private Label label1;
    private ComboBox cboCommitmentType;
    private TextBox txtTolerance;
    private TextBox txtAmount;
    private Label label10;
    private Button btnDateStamp;
    private TextBox txtNotes;
    private GridView gvHistory;
    private ToolTip toolTips;
    private BorderPanel grpEditor;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnSave;
    private Panel pnlDetails;
    private Panel pnlHistory;
    private GroupContainer grpHistory;
    private GroupContainer grpNotes;
    private StandardIconButton btnExportHistory;
    private StandardIconButton btnList;
    private GradientPanel gradientPanel1;
    private Label lblTradeName;
    private TextBox txtMaxAmt;
    private Label label20;
    private TextBox txtMinAmt;
    private Label label12;
    private DatePicker dpCommitmentDate;
    private Label label24;
    private GridView gvEligible;
    private TableContainer grpLoanTrades;
    private CollapsibleSplitter collapsibleSplitter1;
    private TableContainer grpPairOffs;
    private Label label13;
    private ComboBox cboTradeDescription;
    private Label label14;
    private ComboBox cboSecurityType;
    private Label label15;
    private ComboBox cboProgramType;
    private Label label16;
    private TextBox txtTerm1;
    private Label label17;
    private TextBox txtCoupon;
    private TextBox txtPrice;
    private Label label18;
    private TextBox txtTerm2;
    private Label label19;
    private Label label21;
    private DatePicker dpConfirmDate;
    private Label label2;
    private Label label6;
    private DatePicker dpNotificationDate;
    private Label label5;
    private DatePicker dpSettlementDate;
    private SecurityTradeProfitControl ctlTradeProfit;
    private Button btnAssignLoanTrade;
    private FlowLayoutPanel flpEligible;
    private GridView gvPairOffs;
    private FlowLayoutPanel flpPairOffs;
    private StandardIconButton btnEditPairOff;
    private CollapsibleSplitter collapsibleSplitter2;
    private Button btnUnassignLoanTrade;
    private Panel pnlTradeTop;
    private DealerEditorControl ctlDealer;
    private CollapsibleSplitter collapsibleSplitter3;
    private GroupContainer groupContainer1;
    private CollapsibleSplitter collapsibleSplitter4;
    private Panel pnlMbsPools;
    private Label label7;
    private TextBox txtOptionPremium;

    public bool DataModified
    {
      get
      {
        return !this.readOnly && (this.modified || this.ctlDealer.DataModified || this.ctlMbsPoolList.DataModified);
      }
    }

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    public TradeInfoObj CurrentTradeInfo
    {
      get => this.trade != null ? (TradeInfoObj) this.trade : (TradeInfoObj) null;
    }

    public bool SuspendEvents
    {
      get => this.suspendEvents;
      set => this.suspendEvents = value;
    }

    public Decimal TradeAmount
    {
      get
      {
        return !string.IsNullOrEmpty(this.txtAmount.Text) ? Utils.ParseDecimal((object) this.txtAmount.Text) : 0M;
      }
    }

    public SecurityTradeEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtTerm1, TextBoxContentRule.NonNegativeInteger, "##0");
      TextBoxFormatter.Attach(this.txtTerm2, TextBoxContentRule.NonNegativeInteger, "##0");
      TextBoxFormatter.Attach(this.txtAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtOptionPremium, TextBoxContentRule.NonNegativeDecimal, "#,##0.000000;;\"\"");
      TextBoxFormatter.Attach(this.txtMinAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtMaxAmt, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtTolerance, TextBoxContentRule.NonNegativeDecimal, "#,##0.00;;\"\"");
      TextBoxFormatter.Attach(this.txtCoupon, TextBoxContentRule.NonNegativeDecimal, "#,##0.00000;;\"\"");
      TextBoxFormatter.Attach(this.txtPrice, TextBoxContentRule.NonNegativeDecimal, "#,##0.0000000;;\"\"");
      this.btnEditPairOff.Enabled = false;
      this.resetFieldDefs();
      this.loadPersonalLayout();
      this.refreshConfigurableFieldOptions();
      this.ctlMbsPoolList = new MbsPoolAssignmentListScreen((ITradeEditorBase) this);
      this.ctlMbsPoolList.ModifiedEvent += new EventHandler(this.CtlMbsPoolListOnModifiedEvent);
      this.pnlMbsPools.Controls.Clear();
      this.pnlMbsPools.Controls.Add((Control) this.ctlMbsPoolList);
      this.ctlMbsPoolList.Dock = DockStyle.Fill;
    }

    private void CtlMbsPoolListOnModifiedEvent(object sender, EventArgs eventArgs)
    {
      this.recalculateProfitability();
    }

    public void RefreshData() => this.RefreshData(new SecurityTradeInfo());

    public void RefreshData(SecurityTradeInfo trade) => this.RefreshData(trade, (string[]) null);

    public void RefreshData(SecurityTradeInfo trade, string[] tradeGuids)
    {
      this.modified = false;
      this.readOnly = false;
      this.originalTradeName = (string) null;
      this.suspendEvents = false;
      this.trade = trade;
      this.removedTrade.Clear();
      this.loadTradeData();
    }

    public bool SaveTrade()
    {
      bool flag = true;
      if (this.readOnly)
        return true;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (!this.prevalidateCommit())
          return false;
        this.commitChanges();
        if (!this.validateTradeData())
          return false;
        this.saveTradeInfo();
        this.loadTradeData();
        return flag;
      }
      catch (ObjectNotFoundException ex)
      {
        Tracing.Log(SecurityTradeEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        if (ex.ObjectType == ObjectType.Trade)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "The current trade has been deleted and cannot be saved. All changes made to this trade will be lost.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "The trade could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.ReadOnly = true;
        this.modified = false;
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(SecurityTradeEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The trade could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.ReadOnly = true;
        this.modified = false;
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void commitChanges()
    {
      this.modified = this.DataModified;
      this.ctlDealer.CommitChanges();
      this.trade.Name = this.txtName.Text.Trim();
      this.trade.CommitmentType = this.cboCommitmentType.Text;
      this.trade.TradeDescription = this.cboTradeDescription.Text;
      this.trade.SecurityType = this.cboSecurityType.Text;
      this.trade.ProgramType = this.cboProgramType.Text;
      this.trade.Term1 = Utils.ParseInt((object) this.txtTerm1.Text) < 0 ? 0 : Utils.ParseInt((object) this.txtTerm1.Text);
      this.trade.Term2 = Utils.ParseInt((object) this.txtTerm2.Text) < 0 ? 0 : Utils.ParseInt((object) this.txtTerm2.Text);
      this.trade.Coupon = Utils.ParseDecimal((object) this.txtCoupon.Text);
      this.trade.Price = Utils.ParseDecimal((object) this.txtPrice.Text);
      this.trade.DealerName = this.ctlDealer.Dealer.EntityName;
      this.trade.Dealer = this.ctlDealer.Dealer;
      this.trade.TradeAmount = Utils.ParseDecimal((object) this.txtAmount.Text);
      this.trade.OptionPremium = Utils.ParseDecimal((object) this.txtOptionPremium.Text);
      this.trade.Tolerance = Utils.ParseDecimal((object) this.txtTolerance.Text);
      this.trade.Notes = this.txtNotes.Text;
      this.trade.CommitmentDate = this.dpCommitmentDate.Value;
      this.trade.ConfirmDate = this.dpConfirmDate.Value;
      this.trade.SettlementDate = this.dpSettlementDate.Value;
      this.trade.NotificationDate = this.dpNotificationDate.Value;
      this.trade.PairOffAmount = this.ctlTradeProfit.TotalPairOffAmount;
      this.trade.OpenAmount = this.ctlTradeProfit.OpenAmount;
      this.trade.TotalPairOffGainLoss = this.ctlTradeProfit.TotalPairOffGainLoss;
    }

    private void saveTradeInfo()
    {
      int tradeId = this.trade.TradeID;
      if (tradeId < 0)
        tradeId = Session.SecurityTradeManager.CreateTrade(this.trade);
      else
        Session.SecurityTradeManager.UpdateTrade(this.trade);
      if (this.removedTrade.Count > 0)
      {
        SecurityTradeAssignment[] tradeAssigments = Session.SecurityTradeManager.GetTradeAssigments(this.trade.TradeID);
        foreach (KeyValuePair<int, LoanTradeViewModel> keyValuePair in this.removedTrade)
        {
          if (((IEnumerable<bool>) ((IEnumerable<SecurityTradeAssignment>) tradeAssigments).Select<SecurityTradeAssignment, bool>((System.Func<SecurityTradeAssignment, bool>) (p => this.removedTrade.ContainsKey(Utils.ParseInt((object) p.AssigneeTradeID, -1)))).ToArray<bool>()).Count<bool>() > 0)
            Session.SecurityTradeManager.UnassignLoanTradeToTrade(this.trade.TradeID, keyValuePair.Value.TradeID);
        }
        Session.SecurityTradeManager.UpdateTradeAfterAssignLoanTrade(this.trade);
      }
      this.removedTrade.Clear();
      this.ctlMbsPoolList.Save();
      this.trade = Session.SecurityTradeManager.GetTrade(tradeId);
      this.resetOriginalTradeData();
    }

    private bool prevalidateCommit()
    {
      this.tabTrade.SelectedTab = this.tpDetails;
      return this.ctlMbsPoolList.ValidateChanges();
    }

    private bool validateTradeData()
    {
      string str = this.txtName.Text.Trim();
      if (str.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a name/number for this trade before saving.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (string.Compare(str, this.originalTradeName, true) != 0 && Session.SecurityTradeManager.GetTradeByName(str) != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A trade with the name/number '" + str + "' already exists. You must enter a unique name for this trade.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      return !this.areIneligibleLoanTradeAssigned() || Utils.Dialog((IWin32Window) this, "One or more loan trades assigned to this trade do not meet the eligibility criteria. Do you want to continue to save this trade?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
    }

    public void RefreshContents()
    {
      this.resetFieldDefs();
      this.loadPersonalLayout();
      this.loadTradeData();
    }

    private void refreshConfigurableFieldOptions()
    {
      this.cboCommitmentType.Items.Clear();
      ArrayList secondaryFields1 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.CommitmentTypeOption);
      if (secondaryFields1 != null)
      {
        foreach (string str in secondaryFields1)
          this.cboCommitmentType.Items.Add((object) str);
      }
      this.cboTradeDescription.Items.Clear();
      ArrayList secondaryFields2 = Session.ConfigurationManager.GetSecondaryFields(SecondaryFieldTypes.TradeDescriptionOption);
      if (secondaryFields2 != null)
      {
        foreach (string str in secondaryFields2)
          this.cboTradeDescription.Items.Add((object) str);
      }
      this.cboSecurityType.Items.Clear();
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
        this.cboSecurityType.Items.Add(row["Name"]);
    }

    private void resetFieldDefs()
    {
      this.tradeAssLoanTradeFieldDefs = TradeAssignedLoanTradeFieldDefs.GetFieldDefs();
    }

    private void setReadOnly()
    {
      this.txtName.ReadOnly = this.readOnly;
      this.cboCommitmentType.Enabled = !this.readOnly;
      this.cboTradeDescription.Enabled = !this.readOnly;
      this.cboSecurityType.Enabled = !this.readOnly;
      this.cboProgramType.Enabled = !this.readOnly;
      this.txtTerm1.ReadOnly = this.readOnly;
      this.txtTerm2.ReadOnly = this.readOnly;
      this.txtCoupon.ReadOnly = this.readOnly;
      this.txtPrice.ReadOnly = this.readOnly;
      this.ctlDealer.ReadOnly = this.readOnly;
      this.txtAmount.ReadOnly = this.readOnly;
      this.txtOptionPremium.ReadOnly = this.readOnly;
      this.txtTolerance.ReadOnly = this.readOnly;
      this.txtMinAmt.ReadOnly = this.readOnly;
      this.txtMaxAmt.ReadOnly = this.readOnly;
      this.dpCommitmentDate.ReadOnly = this.readOnly;
      this.dpConfirmDate.ReadOnly = this.readOnly;
      this.dpSettlementDate.ReadOnly = this.readOnly;
      this.dpNotificationDate.ReadOnly = this.readOnly;
      this.txtNotes.ReadOnly = this.readOnly;
      this.btnDateStamp.Visible = !this.readOnly;
      if (this.trade.Locked)
        this.btnEditPairOff.Enabled = false;
      else
        this.btnEditPairOff.Enabled = !this.readOnly;
      if (this.trade.Locked)
        this.btnAssignLoanTrade.Visible = false;
      else
        this.btnAssignLoanTrade.Visible = !this.readOnly;
      this.btnSave.Enabled = !this.readOnly;
    }

    private void resetOriginalTradeData()
    {
      if (string.IsNullOrEmpty(this.trade.Name))
        return;
      this.originalTradeName = this.trade.Name.Trim();
    }

    private bool areIneligibleLoanTradeAssigned() => false;

    private static string getTradeStatusDescription(SecurityTradeAssignment assignment)
    {
      return SecurityTradeEditor.tradeStatusNameProvider.GetName((object) assignment.AssignedStatus);
    }

    private void recalculatePairOffFees() => this.loadPairOffs();

    private void recalculateProfitability() => this.loadProfitabilityData();

    private void btnSave_Click(object sender, EventArgs e) => this.SaveTrade();

    private void onFieldValueChanged(object sender, EventArgs e)
    {
      Decimal num1 = 0M;
      try
      {
        num1 = Utils.ParseDecimal((object) this.txtAmount.Text);
      }
      catch
      {
      }
      Decimal num2 = 0M;
      try
      {
        num2 = Utils.ParseDecimal((object) this.txtTolerance.Text);
      }
      catch
      {
      }
      this.txtMinAmt.Text = (num1 - num1 * num2 / 100M).ToString("#,##0;;\"\"");
      this.txtMaxAmt.Text = (num1 + num1 * num2 / 100M).ToString("#,##0;;\"\"");
      if (sender == this.txtPrice && Utils.ParseDecimal((object) ((Control) sender).Text) != this.trade.Price)
      {
        this.commitChanges();
        this.recalculatePairOffFees();
        this.recalculateProfitability();
      }
      if (sender == this.txtAmount && Utils.ParseDecimal((object) ((Control) sender).Text) != this.trade.TradeAmount)
      {
        this.commitChanges();
        this.recalculateProfitability();
      }
      this.modified = true;
    }

    private void tabTrade_SelectedIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.tabTrade.SelectedTab == this.tpDetails)
        this.recalculateProfitability();
      else
        this.commitChanges();
      Cursor.Current = Cursors.Default;
    }

    private void btnDateStamp_Click(object sender, EventArgs e)
    {
      string str = Utils.CreateTimestamp(true) + " " + Session.UserInfo.FullName + "> ";
      if (this.txtNotes.Text.Length != 0)
        this.txtNotes.Text += Environment.NewLine;
      this.txtNotes.Text += str;
      this.txtNotes.SelectionStart = this.txtNotes.Text.Length;
      this.txtNotes.Focus();
    }

    private void tpHistory_Resize(object sender, EventArgs e)
    {
      int num = Math.Max(0, this.pnlHistory.Width - 5);
      this.grpNotes.Top = this.grpHistory.Top = 0;
      this.grpNotes.Height = this.grpHistory.Height = this.pnlHistory.Height;
      this.grpNotes.Left = 0;
      this.grpNotes.Width = num / 2;
      this.grpHistory.Width = Math.Max(0, num - this.grpNotes.Width);
      this.grpHistory.Left = this.grpNotes.Right + 5;
    }

    private void btnExportHistory_Click(object sender, EventArgs e)
    {
      try
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddDataTable(this.gvHistory, false);
        excelHandler.CreateExcel();
      }
      catch (Exception ex)
      {
        Tracing.Log(SecurityTradeEditor.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void btnList_Click(object sender, EventArgs e)
    {
      TradeManagementConsole.Instance.CloseSecurityTrade();
    }

    private void gvEligible_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!(e.Item.Tag is LoanTradeViewModel tag))
        return;
      TradeManagementConsole.Instance.OpenTrade(tag.TradeID, false);
    }

    private void gvPairOffs_ItemClick(object source, GVItemEventArgs e)
    {
      if (this.gvPairOffs.SelectedItems.Count <= 0)
        return;
      this.btnEditPairOff.Enabled = true;
    }

    private void gvPairOffs_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      PairOff tag = e.Item.Tag as PairOff;
      this.editPairOff(e.Item.Index, tag);
    }

    private void btnEditPairOff_Click(object sender, EventArgs e)
    {
      if (this.gvPairOffs.SelectedItems.Count < 1)
        return;
      this.editPairOff(this.gvPairOffs.SelectedItems[0].Index, this.gvPairOffs.SelectedItems[0].Tag as PairOff);
    }

    private void editPairOff(int index, PairOff pairOff)
    {
      using (SecurityTradePairOffDialog tradePairOffDialog = new SecurityTradePairOffDialog(pairOff, this.trade.Price))
      {
        tradePairOffDialog.ReadOnly = this.ReadOnly;
        if (!this.ReadOnly && this.trade.Locked)
          tradePairOffDialog.ReadOnly = true;
        if (tradePairOffDialog.ShowDialog((IWin32Window) this) != DialogResult.OK || !tradePairOffDialog.DataModified)
          return;
        tradePairOffDialog.CommitChanges();
        this.trade.PairOffs[tradePairOffDialog.PairOff.Index - 1] = tradePairOffDialog.PairOff;
        this.modified = true;
        this.loadPairOffs();
        this.recalculateProfitability();
      }
    }

    private void btnAssignLoanTrade_Click(object sender, EventArgs e)
    {
      if (!this.SaveTrade())
        return;
      TradeManagementConsole.Instance.OpenTrade(this.trade, 0);
    }

    private void btnUnassignLoanTrade_Click(object sender, EventArgs e)
    {
      if (this.gvEligible.SelectedItems == null || this.gvEligible.SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one loan trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more loan trades assigned to this trade will be removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        foreach (GVItem selectedItem in this.gvEligible.SelectedItems)
        {
          LoanTradeViewModel tag = selectedItem.Tag as LoanTradeViewModel;
          if (!this.removedTrade.ContainsKey(tag.TradeID))
          {
            this.removedTrade.Add(tag.TradeID, tag);
            this.gvEligible.Items.Remove(selectedItem);
          }
        }
        this.recalculateProfitability();
        this.modified = true;
        this.grpLoanTrades.Text = "Loan Trades (" + (object) this.gvEligible.Items.Count + ")";
      }
    }

    private void cboSecurityType_SelectedIndexChanged(object sender, EventArgs e)
    {
      foreach (DataRow row in (InternalDataCollectionBase) Session.ConfigurationManager.GetSecondarySecurityTypes().Rows)
      {
        if (this.cboSecurityType.Text == (string) row["Name"])
        {
          this.cboProgramType.Text = (string) row["ProgramType"];
          this.txtTerm1.Text = (int) row["Term1"] != -1 ? row["Term1"].ToString() : string.Empty;
          this.txtTerm2.Text = (int) row["Term2"] != -1 ? row["Term2"].ToString() : string.Empty;
        }
      }
    }

    private void loadTradeData()
    {
      SecurityTradeEditorScreenData editorScreenData = Session.SecurityTradeManager.GetTradeEditorScreenData(this.trade.TradeID);
      if (editorScreenData.Trade != null)
        this.trade = editorScreenData.Trade;
      this.lblTradeName.Text = this.trade.TradeID > 0 ? "Trade " + this.trade.Name : "New Trade";
      this.txtName.Text = this.trade.Name;
      this.cboCommitmentType.Text = this.trade.CommitmentType;
      this.cboTradeDescription.Text = this.trade.TradeDescription;
      this.cboProgramType.Text = this.trade.ProgramType;
      this.cboSecurityType.Text = this.trade.SecurityType;
      this.txtTerm1.Text = this.trade.Term1 > 0 ? this.trade.Term1.ToString() : "";
      this.txtTerm2.Text = this.trade.Term2 > 0 ? this.trade.Term2.ToString() : "";
      this.txtCoupon.Text = this.trade.Coupon.ToString("#,##0.00000;;\"\"");
      this.txtPrice.Text = this.trade.Price.ToString("#,##0.0000000;;\"\"");
      TextBox txtAmount = this.txtAmount;
      Decimal num = this.trade.TradeAmount;
      string str1 = num.ToString("#,##0;;\"\"");
      txtAmount.Text = str1;
      TextBox txtOptionPremium = this.txtOptionPremium;
      num = this.trade.OptionPremium;
      string str2 = num.ToString("#,##0.000000;;\"\"");
      txtOptionPremium.Text = str2;
      TextBox txtTolerance = this.txtTolerance;
      string str3;
      if (!(this.trade.Tolerance == 0M))
      {
        num = this.trade.Tolerance;
        str3 = num.ToString("#,##0.00;;\"\"");
      }
      else
        str3 = "";
      txtTolerance.Text = str3;
      this.dpCommitmentDate.Value = this.trade.CommitmentDate;
      this.dpConfirmDate.Value = this.trade.ConfirmDate;
      this.dpSettlementDate.Value = this.trade.SettlementDate;
      this.dpNotificationDate.Value = this.trade.NotificationDate;
      this.txtNotes.Text = this.trade.Notes;
      this.txtMinAmt.ReadOnly = true;
      this.txtMaxAmt.ReadOnly = true;
      this.loadDealerData();
      this.loadAssignments(editorScreenData.Assignments);
      this.loadTradeAssignments();
      this.loadPairOffs();
      this.loadHistory(editorScreenData.TradeHistory);
      this.loadProfitabilityData();
      this.resetOriginalTradeData();
      this.ReadOnly = this.trade.Status == TradeStatus.Archived;
      this.modified = false;
      if (!this.trade.IsCloned)
        return;
      this.modified = this.trade.TradeID <= 0;
    }

    private void loadDealerData() => this.ctlDealer.Init(this.trade.Dealer);

    private void loadPairOffs()
    {
      if (this.trade.PairOffs == null)
        this.trade.InitTradeObjects();
      for (int count = this.trade.PairOffs.Count; count < 4; ++count)
        this.trade.PairOffs.Add(new PairOff());
      if (this.trade.PairOffs == null)
        return;
      this.gvPairOffs.Items.Clear();
      for (int index = 0; index < this.trade.PairOffs.Count; ++index)
        this.gvPairOffs.Items.Add(this.createPairOffListItem(index, this.trade.PairOffs[index]));
      if (!this.trade.Locked)
        return;
      this.btnEditPairOff.Enabled = false;
    }

    private GVItem createPairOffListItem(int index, PairOff pairOff)
    {
      GVItem pairOffListItem = new GVItem();
      pairOffListItem.Text = "Pair-Off " + (index + 1).ToString();
      if (pairOff.Date > DateTime.MinValue)
        pairOffListItem.SubItems.Add((object) pairOff.Date.ToString("MM/dd/yyyy"));
      else
        pairOffListItem.SubItems.Add((object) "");
      if (pairOff.UndeliveredAmount > 0M)
      {
        Decimal num = pairOff.UndeliveredAmount * -1M;
        pairOffListItem.SubItems.Add((object) num.ToString("#,##0"));
      }
      else
        pairOffListItem.SubItems.Add((object) "");
      if (pairOff.Fee > 0M)
        pairOffListItem.SubItems.Add((object) pairOff.Fee.ToString("#,##0.#######;;\"\""));
      else
        pairOffListItem.SubItems.Add((object) "");
      Decimal pairOffGainLoss = SecurityTradeCalculation.CalculatePairOffGainLoss(this.trade.Price, pairOff.Fee, pairOff.UndeliveredAmount);
      pairOffListItem.SubItems.Add((object) pairOffGainLoss.ToString("#,##0"));
      pairOffListItem.Tag = (object) pairOff;
      return pairOffListItem;
    }

    public void AssignLoanTradeToTrade(int assigneeTradeId)
    {
      if (this.trade == null)
        this.SaveTrade();
      Session.SecurityTradeManager.AssignLoanTradeToTrade(this.trade.TradeID, assigneeTradeId);
      this.loadAssignments();
      this.modified = true;
    }

    private void loadAssignments()
    {
      if (this.trade == null)
        return;
      this.loadAssignments(Session.SecurityTradeManager.GetTradeAssigments(this.trade.TradeID));
    }

    private void loadAssignments(SecurityTradeAssignment[] assignments)
    {
      this.gvEligible.Items.Clear();
      if (assignments == null)
      {
        this.grpLoanTrades.Text = "Loan Trades (0)";
      }
      else
      {
        List<string> stringList = new List<string>();
        foreach (SecurityTradeAssignment assignment in assignments)
          stringList.Add(assignment.AssigneeTrade.TradeID.ToString());
        try
        {
          ICursor cursor = Session.LoanTradeManager.OpenTradeCursor(new QueryCriterion[1]
          {
            (QueryCriterion) new OrdinalValueCriterion("LoanTradeDetails.SecurityTradeID", (object) this.trade.TradeID)
          }, (SortField[]) null, this.generateFieldList(), true, false);
          foreach (LoanTradeViewModel tradeInfo in (LoanTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
          {
            if (!this.removedTrade.ContainsKey(tradeInfo.TradeID))
              this.gvEligible.Items.Add(this.createAssignedLoanTradeListItem(tradeInfo));
          }
        }
        catch
        {
        }
        this.grpLoanTrades.Text = "Loan Trades (" + assignments.Length.ToString() + ")";
        if (!this.trade.Locked)
          return;
        this.btnAssignLoanTrade.Visible = false;
      }
    }

    private GVItem createAssignedLoanTradeListItem(LoanTradeViewModel tradeInfo)
    {
      GVItem loanTradeListItem = new GVItem();
      loanTradeListItem.Tag = (object) tradeInfo;
      for (int index = 0; index < this.gvEligible.Columns.Count; ++index)
      {
        TableLayout.Column tag1 = (TableLayout.Column) this.gvEligible.Columns[index].Tag;
        string tag2 = tag1.Tag;
        string columnId = tag1.ColumnID;
        object obj = (object) null;
        TradeReportFieldDef fieldByCriterionName = (TradeReportFieldDef) this.tradeAssLoanTradeFieldDefs.GetFieldByCriterionName(tag1.ColumnID);
        if (fieldByCriterionName != null && tradeInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
        loanTradeListItem.SubItems[index].Value = obj;
      }
      return loanTradeListItem;
    }

    private void loadHistory()
    {
      this.loadHistory(Session.SecurityTradeManager.GetTradeHistory(this.trade.TradeID));
    }

    private void loadHistory(SecurityTradeHistoryItem[] tradeHistory)
    {
      this.btnExportHistory.Enabled = false;
      this.gvHistory.Items.Clear();
      if (tradeHistory == null)
        return;
      foreach (SecurityTradeHistoryItem historyItem in tradeHistory)
        this.gvHistory.Items.Add(this.createTradeHistoryListItem(historyItem));
      this.btnExportHistory.Enabled = this.gvHistory.Items.Count > 0;
    }

    private GVItem createTradeHistoryListItem(SecurityTradeHistoryItem historyItem)
    {
      return new GVItem()
      {
        Text = historyItem.Timestamp.ToString("MM/dd/yyyy h:mm tt"),
        SubItems = {
          (object) historyItem.Description,
          (object) historyItem.UserName
        },
        Tag = (object) historyItem
      };
    }

    private void loadProfitabilityData()
    {
      this.ctlTradeProfit.RemovedAssignment(this.removedTrade.Select<KeyValuePair<int, LoanTradeViewModel>, int>((System.Func<KeyValuePair<int, LoanTradeViewModel>, int>) (s => s.Value.TradeID)).ToList<int>());
      this.ctlTradeProfit.Calculate(this.trade, MbsPoolAssignment.Convert(this.ctlMbsPoolList.GetCurrentAssignments()));
    }

    private void loadTradeAssignments()
    {
      if (this.trade == null)
        return;
      this.ctlMbsPoolList.RefreshData(MbsPoolAssignmentListScreen.GetAssigments(this.trade.TradeID), MbsPoolAssignmentListScreen.GetUnassignedAssigments(this.trade.TradeID));
    }

    private void loadPersonalLayout()
    {
      try
      {
        BinaryObject userSettings = Session.User.GetUserSettings(this.tradeLoanTradeTableLayoutFileName);
        this.setLayout(userSettings == null ? this.getDemoTableLayout() : userSettings.ToObject<TableLayout>());
      }
      catch (Exception ex)
      {
        Tracing.Log(SecurityTradeEditor.sw, this.className, TraceLevel.Error, "Error loading layout: " + (object) ex);
      }
    }

    private void setLayout(TableLayout layOut)
    {
      this.tradeLoanTradeTableLayout = layOut;
      this.suspendEvents = true;
      this.applyTableLayout(layOut);
      this.suspendEvents = false;
      this.loadAssignments();
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.validateTableLayout(layout);
      this.gvLayoutManager.ApplyLayout(layout, false);
    }

    private void validateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = this.tradeAssLoanTradeFieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (fieldByCriterionName != null)
          column.Title = fieldByCriterionName.Description;
        else
          columnList.Add(column);
      }
      foreach (TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (TradeReportFieldDef loanTradeFieldDef in (ReportFieldDefContainer) this.tradeAssLoanTradeFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(loanTradeFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(loanTradeFieldDef));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvEligible, this.getFullTableLayout(), this.getDemoTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.onLayoutChanged);
      return layoutManager;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.loadAssignments();
      this.tradeLoanTradeTableLayout = this.gvLayoutManager.GetCurrentLayout();
      using (BinaryObject data = new BinaryObject((IXmlSerializable) this.tradeLoanTradeTableLayout))
        Session.User.SaveUserSettings(this.tradeLoanTradeTableLayoutFileName, data);
    }

    private TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.AssignedStatusDate", "Assigned Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.Name", "Trade ID", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.TradeDescription", "Trade Description", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.InvestorName", "Investor", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.InvestorCommitmentNum", "Investor Commitment #", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.TradeAmount", "Trade Amount", HorizontalAlignment.Right, 90));
      demoTableLayout.AddColumn(new TableLayout.Column("TradeLoanTradeSummary.TotalAmount", "Assigned Amount", HorizontalAlignment.Right, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("LoanTradeDetails.NetProfit", "Net Profit", HorizontalAlignment.Right, 100));
      return demoTableLayout;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        TradeReportFieldDef fieldByCriterionName = (TradeReportFieldDef) this.tradeAssLoanTradeFieldDefs.GetFieldByCriterionName(column.ColumnID);
        if (!stringList.Contains(column.ColumnID))
          stringList.Add(column.ColumnID);
        if (fieldByCriterionName != null)
        {
          foreach (string relatedField in fieldByCriterionName.RelatedFields)
          {
            if (!stringList.Contains(relatedField))
              stringList.Add(relatedField);
          }
        }
      }
      return stringList.ToArray();
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "STE_SaveTrade":
          this.SaveTrade();
          break;
        case "STE_ExitTrade":
          TradeManagementConsole.Instance.CloseSecurityTrade();
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "STE_SaveTrade":
          stateControl = (Control) this.btnSave;
          break;
        case "STR_ExitTrade":
          stateControl = (Control) this.btnList;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
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
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      this.toolTips = new ToolTip(this.components);
      this.btnExportHistory = new StandardIconButton();
      this.btnList = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.grpEditor = new BorderPanel();
      this.tabTrade = new TabControl();
      this.tpDetails = new TabPage();
      this.pnlDetails = new Panel();
      this.grpLoanTrades = new TableContainer();
      this.flpEligible = new FlowLayoutPanel();
      this.btnAssignLoanTrade = new Button();
      this.btnUnassignLoanTrade = new Button();
      this.gvEligible = new GridView();
      this.collapsibleSplitter4 = new CollapsibleSplitter();
      this.pnlMbsPools = new Panel();
      this.collapsibleSplitter2 = new CollapsibleSplitter();
      this.grpPairOffs = new TableContainer();
      this.flpPairOffs = new FlowLayoutPanel();
      this.btnEditPairOff = new StandardIconButton();
      this.gvPairOffs = new GridView();
      this.ctlTradeProfit = new SecurityTradeProfitControl();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpTradeInfo = new GroupContainer();
      this.groupContainer1 = new GroupContainer();
      this.ctlDealer = new DealerEditorControl();
      this.collapsibleSplitter3 = new CollapsibleSplitter();
      this.pnlTradeTop = new Panel();
      this.label1 = new Label();
      this.label10 = new Label();
      this.label6 = new Label();
      this.txtName = new TextBox();
      this.dpNotificationDate = new DatePicker();
      this.label3 = new Label();
      this.label5 = new Label();
      this.label4 = new Label();
      this.dpSettlementDate = new DatePicker();
      this.label8 = new Label();
      this.dpConfirmDate = new DatePicker();
      this.label2 = new Label();
      this.txtAmount = new TextBox();
      this.label21 = new Label();
      this.txtTolerance = new TextBox();
      this.label19 = new Label();
      this.cboCommitmentType = new ComboBox();
      this.label18 = new Label();
      this.txtTerm2 = new TextBox();
      this.label12 = new Label();
      this.txtPrice = new TextBox();
      this.txtMinAmt = new TextBox();
      this.label17 = new Label();
      this.label20 = new Label();
      this.txtCoupon = new TextBox();
      this.txtMaxAmt = new TextBox();
      this.label16 = new Label();
      this.label24 = new Label();
      this.txtTerm1 = new TextBox();
      this.dpCommitmentDate = new DatePicker();
      this.label15 = new Label();
      this.cboTradeDescription = new ComboBox();
      this.cboProgramType = new ComboBox();
      this.label13 = new Label();
      this.label14 = new Label();
      this.cboSecurityType = new ComboBox();
      this.tpHistory = new TabPage();
      this.pnlHistory = new Panel();
      this.grpHistory = new GroupContainer();
      this.gvHistory = new GridView();
      this.grpNotes = new GroupContainer();
      this.txtNotes = new TextBox();
      this.btnDateStamp = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.lblTradeName = new Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.txtOptionPremium = new TextBox();
      this.label7 = new Label();
      ((ISupportInitialize) this.btnExportHistory).BeginInit();
      ((ISupportInitialize) this.btnList).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.grpEditor.SuspendLayout();
      this.tabTrade.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.grpLoanTrades.SuspendLayout();
      this.flpEligible.SuspendLayout();
      this.grpPairOffs.SuspendLayout();
      this.flpPairOffs.SuspendLayout();
      ((ISupportInitialize) this.btnEditPairOff).BeginInit();
      this.grpTradeInfo.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.pnlTradeTop.SuspendLayout();
      this.tpHistory.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.grpHistory.SuspendLayout();
      this.grpNotes.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.btnExportHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExportHistory.BackColor = Color.Transparent;
      this.btnExportHistory.Location = new Point(422, 5);
      this.btnExportHistory.MouseDownImage = (Image) null;
      this.btnExportHistory.Name = "btnExportHistory";
      this.btnExportHistory.Size = new Size(16, 16);
      this.btnExportHistory.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExportHistory.TabIndex = 3;
      this.btnExportHistory.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnExportHistory, "Export to Excel");
      this.btnExportHistory.Click += new EventHandler(this.btnExportHistory_Click);
      this.btnList.BackColor = Color.Transparent;
      this.btnList.Location = new Point(595, 3);
      this.btnList.Margin = new Padding(2, 3, 0, 3);
      this.btnList.MouseDownImage = (Image) null;
      this.btnList.Name = "btnList";
      this.btnList.Size = new Size(16, 16);
      this.btnList.StandardButtonType = StandardIconButton.ButtonType.CloseButton;
      this.btnList.TabIndex = 7;
      this.btnList.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnList, "Exit Trade");
      this.btnList.Click += new EventHandler(this.btnList_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(574, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSave, "Save Trade");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.grpEditor.BackColor = Color.Transparent;
      this.grpEditor.Borders = AnchorStyles.None;
      this.grpEditor.Controls.Add((Control) this.tabTrade);
      this.grpEditor.Dock = DockStyle.Fill;
      this.grpEditor.Location = new Point(0, 31);
      this.grpEditor.Name = "grpEditor";
      this.grpEditor.Padding = new Padding(2, 2, 0, 0);
      this.grpEditor.Size = new Size(1053, 717);
      this.grpEditor.TabIndex = 5;
      this.grpEditor.Text = "<Trade Name>";
      this.tabTrade.Controls.Add((Control) this.tpDetails);
      this.tabTrade.Controls.Add((Control) this.tpHistory);
      this.tabTrade.Dock = DockStyle.Fill;
      this.tabTrade.ItemSize = new Size(44, 20);
      this.tabTrade.Location = new Point(2, 2);
      this.tabTrade.Name = "tabTrade";
      this.tabTrade.Padding = new Point(11, 3);
      this.tabTrade.SelectedIndex = 0;
      this.tabTrade.Size = new Size(1051, 715);
      this.tabTrade.TabIndex = 1;
      this.tabTrade.SelectedIndexChanged += new EventHandler(this.tabTrade_SelectedIndexChanged);
      this.tpDetails.Controls.Add((Control) this.pnlDetails);
      this.tpDetails.Location = new Point(4, 24);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(1043, 687);
      this.tpDetails.TabIndex = 0;
      this.tpDetails.Tag = (object) "Details";
      this.tpDetails.Text = "Details";
      this.tpDetails.UseVisualStyleBackColor = true;
      this.pnlDetails.Controls.Add((Control) this.grpLoanTrades);
      this.pnlDetails.Controls.Add((Control) this.collapsibleSplitter4);
      this.pnlDetails.Controls.Add((Control) this.pnlMbsPools);
      this.pnlDetails.Controls.Add((Control) this.collapsibleSplitter2);
      this.pnlDetails.Controls.Add((Control) this.grpPairOffs);
      this.pnlDetails.Controls.Add((Control) this.ctlTradeProfit);
      this.pnlDetails.Controls.Add((Control) this.collapsibleSplitter1);
      this.pnlDetails.Controls.Add((Control) this.grpTradeInfo);
      this.pnlDetails.Dock = DockStyle.Fill;
      this.pnlDetails.Location = new Point(0, 2);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(1041, 683);
      this.pnlDetails.TabIndex = 6;
      this.grpLoanTrades.Controls.Add((Control) this.flpEligible);
      this.grpLoanTrades.Controls.Add((Control) this.gvEligible);
      this.grpLoanTrades.Dock = DockStyle.Fill;
      this.grpLoanTrades.Location = new Point(310, 0);
      this.grpLoanTrades.Name = "grpLoanTrades";
      this.grpLoanTrades.Size = new Size(731, 290);
      this.grpLoanTrades.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpLoanTrades.TabIndex = 1;
      this.grpLoanTrades.Text = "Loan Trades";
      this.flpEligible.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpEligible.BackColor = Color.Transparent;
      this.flpEligible.Controls.Add((Control) this.btnAssignLoanTrade);
      this.flpEligible.Controls.Add((Control) this.btnUnassignLoanTrade);
      this.flpEligible.FlowDirection = FlowDirection.RightToLeft;
      this.flpEligible.Location = new Point(208, 1);
      this.flpEligible.Name = "flpEligible";
      this.flpEligible.Size = new Size(519, 22);
      this.flpEligible.TabIndex = 2;
      this.btnAssignLoanTrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAssignLoanTrade.Location = new Point(416, 0);
      this.btnAssignLoanTrade.Margin = new Padding(0);
      this.btnAssignLoanTrade.Name = "btnAssignLoanTrade";
      this.btnAssignLoanTrade.Size = new Size(103, 22);
      this.btnAssignLoanTrade.TabIndex = 1;
      this.btnAssignLoanTrade.Text = "Assign Trade";
      this.btnAssignLoanTrade.UseVisualStyleBackColor = true;
      this.btnAssignLoanTrade.Click += new EventHandler(this.btnAssignLoanTrade_Click);
      this.btnUnassignLoanTrade.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUnassignLoanTrade.Location = new Point(313, 0);
      this.btnUnassignLoanTrade.Margin = new Padding(0);
      this.btnUnassignLoanTrade.Name = "btnUnassignLoanTrade";
      this.btnUnassignLoanTrade.Size = new Size(103, 22);
      this.btnUnassignLoanTrade.TabIndex = 2;
      this.btnUnassignLoanTrade.Text = "Unassign Trade";
      this.btnUnassignLoanTrade.UseVisualStyleBackColor = true;
      this.btnUnassignLoanTrade.Click += new EventHandler(this.btnUnassignLoanTrade_Click);
      this.gvEligible.AllowColumnReorder = true;
      this.gvEligible.BorderStyle = BorderStyle.None;
      this.gvEligible.Dock = DockStyle.Fill;
      this.gvEligible.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvEligible.Location = new Point(1, 26);
      this.gvEligible.Name = "gvEligible";
      this.gvEligible.Size = new Size(729, 263);
      this.gvEligible.TabIndex = 0;
      this.gvEligible.ItemDoubleClick += new GVItemEventHandler(this.gvEligible_ItemDoubleClick);
      this.collapsibleSplitter4.AnimationDelay = 20;
      this.collapsibleSplitter4.AnimationStep = 20;
      this.collapsibleSplitter4.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter4.ControlToHide = (Control) this.pnlMbsPools;
      this.collapsibleSplitter4.Dock = DockStyle.Bottom;
      this.collapsibleSplitter4.ExpandParentForm = false;
      this.collapsibleSplitter4.Location = new Point(310, 290);
      this.collapsibleSplitter4.Name = "collapsibleSplitter4";
      this.collapsibleSplitter4.TabIndex = 12;
      this.collapsibleSplitter4.TabStop = false;
      this.collapsibleSplitter4.UseAnimations = false;
      this.collapsibleSplitter4.VisualStyle = VisualStyles.Encompass;
      this.pnlMbsPools.Dock = DockStyle.Bottom;
      this.pnlMbsPools.Location = new Point(310, 297);
      this.pnlMbsPools.Name = "pnlMbsPools";
      this.pnlMbsPools.Size = new Size(731, 227);
      this.pnlMbsPools.TabIndex = 11;
      this.collapsibleSplitter2.AnimationDelay = 20;
      this.collapsibleSplitter2.AnimationStep = 20;
      this.collapsibleSplitter2.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter2.ControlToHide = (Control) this.grpPairOffs;
      this.collapsibleSplitter2.Cursor = Cursors.HSplit;
      this.collapsibleSplitter2.Dock = DockStyle.Bottom;
      this.collapsibleSplitter2.ExpandParentForm = false;
      this.collapsibleSplitter2.Location = new Point(310, 524);
      this.collapsibleSplitter2.Name = "collapsibleSplitter2";
      this.collapsibleSplitter2.TabIndex = 9;
      this.collapsibleSplitter2.TabStop = false;
      this.collapsibleSplitter2.UseAnimations = false;
      this.collapsibleSplitter2.VisualStyle = VisualStyles.Encompass;
      this.grpPairOffs.Controls.Add((Control) this.flpPairOffs);
      this.grpPairOffs.Controls.Add((Control) this.gvPairOffs);
      this.grpPairOffs.Dock = DockStyle.Bottom;
      this.grpPairOffs.Location = new Point(310, 531);
      this.grpPairOffs.Name = "grpPairOffs";
      this.grpPairOffs.Size = new Size(731, 134);
      this.grpPairOffs.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpPairOffs.TabIndex = 0;
      this.grpPairOffs.Text = "Pair-Offs";
      this.flpPairOffs.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpPairOffs.BackColor = Color.Transparent;
      this.flpPairOffs.Controls.Add((Control) this.btnEditPairOff);
      this.flpPairOffs.FlowDirection = FlowDirection.RightToLeft;
      this.flpPairOffs.Location = new Point(209, 2);
      this.flpPairOffs.Name = "flpPairOffs";
      this.flpPairOffs.Size = new Size(519, 22);
      this.flpPairOffs.TabIndex = 3;
      this.btnEditPairOff.BackColor = Color.Transparent;
      this.btnEditPairOff.Location = new Point(500, 3);
      this.btnEditPairOff.MouseDownImage = (Image) null;
      this.btnEditPairOff.Name = "btnEditPairOff";
      this.btnEditPairOff.Size = new Size(16, 16);
      this.btnEditPairOff.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditPairOff.TabIndex = 0;
      this.btnEditPairOff.TabStop = false;
      this.btnEditPairOff.Click += new EventHandler(this.btnEditPairOff_Click);
      this.gvPairOffs.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Pair-Off #";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Pair-Off Date";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Pair-Off Amount";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Buy Price";
      gvColumn4.Width = 100;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Gain/Loss";
      gvColumn5.Width = 100;
      this.gvPairOffs.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.gvPairOffs.Dock = DockStyle.Fill;
      this.gvPairOffs.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvPairOffs.Location = new Point(1, 26);
      this.gvPairOffs.Name = "gvPairOffs";
      this.gvPairOffs.Size = new Size(729, 107);
      this.gvPairOffs.TabIndex = 0;
      this.gvPairOffs.ItemClick += new GVItemEventHandler(this.gvPairOffs_ItemClick);
      this.gvPairOffs.ItemDoubleClick += new GVItemEventHandler(this.gvPairOffs_ItemDoubleClick);
      this.ctlTradeProfit.BackColor = SystemColors.Control;
      this.ctlTradeProfit.Dock = DockStyle.Bottom;
      this.ctlTradeProfit.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlTradeProfit.Location = new Point(310, 665);
      this.ctlTradeProfit.Name = "ctlTradeProfit";
      this.ctlTradeProfit.Size = new Size(731, 18);
      this.ctlTradeProfit.TabIndex = 8;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpTradeInfo;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(303, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 6;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.grpTradeInfo.Controls.Add((Control) this.groupContainer1);
      this.grpTradeInfo.Controls.Add((Control) this.collapsibleSplitter3);
      this.grpTradeInfo.Controls.Add((Control) this.pnlTradeTop);
      this.grpTradeInfo.Dock = DockStyle.Left;
      this.grpTradeInfo.HeaderForeColor = SystemColors.ControlText;
      this.grpTradeInfo.Location = new Point(0, 0);
      this.grpTradeInfo.Name = "grpTradeInfo";
      this.grpTradeInfo.Size = new Size(303, 683);
      this.grpTradeInfo.TabIndex = 1;
      this.grpTradeInfo.Text = "Trade Info ";
      this.groupContainer1.Controls.Add((Control) this.ctlDealer);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(1, 439);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(301, 243);
      this.groupContainer1.TabIndex = 183;
      this.groupContainer1.Text = "Dealer";
      this.ctlDealer.BackColor = Color.WhiteSmoke;
      this.ctlDealer.Dock = DockStyle.Fill;
      this.ctlDealer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlDealer.Location = new Point(1, 26);
      this.ctlDealer.Name = "ctlDealer";
      this.ctlDealer.ReadOnly = false;
      this.ctlDealer.Size = new Size(299, 216);
      this.ctlDealer.TabIndex = 0;
      this.collapsibleSplitter3.AnimationDelay = 20;
      this.collapsibleSplitter3.AnimationStep = 20;
      this.collapsibleSplitter3.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter3.ControlToHide = (Control) this.pnlTradeTop;
      this.collapsibleSplitter3.Dock = DockStyle.Top;
      this.collapsibleSplitter3.ExpandParentForm = false;
      this.collapsibleSplitter3.Location = new Point(1, 432);
      this.collapsibleSplitter3.Name = "collapsibleSplitter3";
      this.collapsibleSplitter3.TabIndex = 182;
      this.collapsibleSplitter3.TabStop = false;
      this.collapsibleSplitter3.UseAnimations = false;
      this.collapsibleSplitter3.VisualStyle = VisualStyles.Encompass;
      this.pnlTradeTop.AutoScroll = true;
      this.pnlTradeTop.Controls.Add((Control) this.label1);
      this.pnlTradeTop.Controls.Add((Control) this.label10);
      this.pnlTradeTop.Controls.Add((Control) this.label6);
      this.pnlTradeTop.Controls.Add((Control) this.txtName);
      this.pnlTradeTop.Controls.Add((Control) this.dpNotificationDate);
      this.pnlTradeTop.Controls.Add((Control) this.label3);
      this.pnlTradeTop.Controls.Add((Control) this.label5);
      this.pnlTradeTop.Controls.Add((Control) this.label7);
      this.pnlTradeTop.Controls.Add((Control) this.label4);
      this.pnlTradeTop.Controls.Add((Control) this.dpSettlementDate);
      this.pnlTradeTop.Controls.Add((Control) this.label8);
      this.pnlTradeTop.Controls.Add((Control) this.dpConfirmDate);
      this.pnlTradeTop.Controls.Add((Control) this.label2);
      this.pnlTradeTop.Controls.Add((Control) this.txtOptionPremium);
      this.pnlTradeTop.Controls.Add((Control) this.txtAmount);
      this.pnlTradeTop.Controls.Add((Control) this.label21);
      this.pnlTradeTop.Controls.Add((Control) this.txtTolerance);
      this.pnlTradeTop.Controls.Add((Control) this.label19);
      this.pnlTradeTop.Controls.Add((Control) this.cboCommitmentType);
      this.pnlTradeTop.Controls.Add((Control) this.label18);
      this.pnlTradeTop.Controls.Add((Control) this.txtTerm2);
      this.pnlTradeTop.Controls.Add((Control) this.label12);
      this.pnlTradeTop.Controls.Add((Control) this.txtPrice);
      this.pnlTradeTop.Controls.Add((Control) this.txtMinAmt);
      this.pnlTradeTop.Controls.Add((Control) this.label17);
      this.pnlTradeTop.Controls.Add((Control) this.label20);
      this.pnlTradeTop.Controls.Add((Control) this.txtCoupon);
      this.pnlTradeTop.Controls.Add((Control) this.txtMaxAmt);
      this.pnlTradeTop.Controls.Add((Control) this.label16);
      this.pnlTradeTop.Controls.Add((Control) this.label24);
      this.pnlTradeTop.Controls.Add((Control) this.txtTerm1);
      this.pnlTradeTop.Controls.Add((Control) this.dpCommitmentDate);
      this.pnlTradeTop.Controls.Add((Control) this.label15);
      this.pnlTradeTop.Controls.Add((Control) this.cboTradeDescription);
      this.pnlTradeTop.Controls.Add((Control) this.cboProgramType);
      this.pnlTradeTop.Controls.Add((Control) this.label13);
      this.pnlTradeTop.Controls.Add((Control) this.label14);
      this.pnlTradeTop.Controls.Add((Control) this.cboSecurityType);
      this.pnlTradeTop.Dock = DockStyle.Top;
      this.pnlTradeTop.Location = new Point(1, 26);
      this.pnlTradeTop.Name = "pnlTradeTop";
      this.pnlTradeTop.Size = new Size(301, 406);
      this.pnlTradeTop.TabIndex = 181;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(3, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(59, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Security ID";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(185, 244);
      this.label10.Name = "label10";
      this.label10.Size = new Size(17, 14);
      this.label10.TabIndex = 0;
      this.label10.Text = "%";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(3, 380);
      this.label6.Name = "label6";
      this.label6.Size = new Size(85, 14);
      this.label6.TabIndex = 0;
      this.label6.Text = "Notification Date";
      this.txtName.Location = new Point(126, 10);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(161, 20);
      this.txtName.TabIndex = 10;
      this.txtName.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.dpNotificationDate.BackColor = SystemColors.Window;
      this.dpNotificationDate.Location = new Point(126, 379);
      this.dpNotificationDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpNotificationDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpNotificationDate.Name = "dpNotificationDate";
      this.dpNotificationDate.Size = new Size(104, 22);
      this.dpNotificationDate.TabIndex = 180;
      this.dpNotificationDate.ToolTip = "";
      this.dpNotificationDate.Value = new DateTime(0L);
      this.dpNotificationDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(3, 242);
      this.label3.Name = "label3";
      this.label3.Size = new Size(54, 14);
      this.label3.TabIndex = 3;
      this.label3.Text = "Tolerance";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(3, 357);
      this.label5.Name = "label5";
      this.label5.Size = new Size(82, 14);
      this.label5.TabIndex = 0;
      this.label5.Text = "Settlement Date";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(3, 196);
      this.label4.Name = "label4";
      this.label4.Size = new Size(74, 14);
      this.label4.TabIndex = 4;
      this.label4.Text = "Trade Amount";
      this.dpSettlementDate.BackColor = SystemColors.Window;
      this.dpSettlementDate.Location = new Point(126, 356);
      this.dpSettlementDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpSettlementDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpSettlementDate.Name = "dpSettlementDate";
      this.dpSettlementDate.Size = new Size(104, 22);
      this.dpSettlementDate.TabIndex = 170;
      this.dpSettlementDate.ToolTip = "";
      this.dpSettlementDate.Value = new DateTime(0L);
      this.dpSettlementDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(3, 35);
      this.label8.Name = "label8";
      this.label8.Size = new Size(90, 14);
      this.label8.TabIndex = 8;
      this.label8.Text = "Commitment Type";
      this.dpConfirmDate.BackColor = SystemColors.Window;
      this.dpConfirmDate.Location = new Point(126, 333);
      this.dpConfirmDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpConfirmDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpConfirmDate.Name = "dpConfirmDate";
      this.dpConfirmDate.Size = new Size(104, 22);
      this.dpConfirmDate.TabIndex = 160;
      this.dpConfirmDate.ToolTip = "";
      this.dpConfirmDate.Value = new DateTime(0L);
      this.dpConfirmDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(3, 334);
      this.label2.Name = "label2";
      this.label2.Size = new Size(69, 14);
      this.label2.TabIndex = 0;
      this.label2.Text = "Confirm Date";
      this.txtAmount.Location = new Point(126, 195);
      this.txtAmount.MaxLength = 12;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new Size(93, 20);
      this.txtAmount.TabIndex = 110;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label21.AutoSize = true;
      this.label21.Location = new Point(3, 173);
      this.label21.Name = "label21";
      this.label21.Size = new Size(31, 14);
      this.label21.TabIndex = 0;
      this.label21.Text = "Price";
      this.txtTolerance.Location = new Point(126, 241);
      this.txtTolerance.MaxLength = 6;
      this.txtTolerance.Name = "txtTolerance";
      this.txtTolerance.Size = new Size(59, 20);
      this.txtTolerance.TabIndex = 120;
      this.txtTolerance.TextAlign = HorizontalAlignment.Right;
      this.txtTolerance.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label19.AutoSize = true;
      this.label19.Location = new Point(258, 129);
      this.label19.Name = "label19";
      this.label19.Size = new Size(30, 14);
      this.label19.TabIndex = 0;
      this.label19.Text = "mths";
      this.cboCommitmentType.FormattingEnabled = true;
      this.cboCommitmentType.ItemHeight = 14;
      this.cboCommitmentType.Location = new Point(126, 32);
      this.cboCommitmentType.Name = "cboCommitmentType";
      this.cboCommitmentType.Size = new Size(161, 22);
      this.cboCommitmentType.TabIndex = 20;
      this.cboCommitmentType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label18.AutoSize = true;
      this.label18.Location = new Point(186, 128);
      this.label18.Name = "label18";
      this.label18.Size = new Size(11, 14);
      this.label18.TabIndex = 0;
      this.label18.Text = "-";
      this.txtTerm2.Location = new Point(197, 126);
      this.txtTerm2.MaxLength = 12;
      this.txtTerm2.Name = "txtTerm2";
      this.txtTerm2.Size = new Size(59, 20);
      this.txtTerm2.TabIndex = 70;
      this.txtTerm2.TextAlign = HorizontalAlignment.Right;
      this.txtTerm2.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label12.AutoSize = true;
      this.label12.Location = new Point(3, 265);
      this.label12.Name = "label12";
      this.label12.Size = new Size(86, 14);
      this.label12.TabIndex = 0;
      this.label12.Text = "Minimum Amount";
      this.txtPrice.Location = new Point(126, 172);
      this.txtPrice.MaxLength = 12;
      this.txtPrice.Name = "txtPrice";
      this.txtPrice.Size = new Size(93, 20);
      this.txtPrice.TabIndex = 90;
      this.txtPrice.TextAlign = HorizontalAlignment.Right;
      this.txtPrice.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtMinAmt.Location = new Point(126, 264);
      this.txtMinAmt.MaxLength = 12;
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.ReadOnly = true;
      this.txtMinAmt.Size = new Size(93, 20);
      this.txtMinAmt.TabIndex = 130;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.txtMinAmt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(3, 150);
      this.label17.Name = "label17";
      this.label17.Size = new Size(44, 14);
      this.label17.TabIndex = 0;
      this.label17.Text = "Coupon";
      this.label20.AutoSize = true;
      this.label20.Location = new Point(3, 288);
      this.label20.Name = "label20";
      this.label20.Size = new Size(90, 14);
      this.label20.TabIndex = 0;
      this.label20.Text = "Maximum Amount";
      this.txtCoupon.Location = new Point(126, 149);
      this.txtCoupon.MaxLength = 12;
      this.txtCoupon.Name = "txtCoupon";
      this.txtCoupon.Size = new Size(93, 20);
      this.txtCoupon.TabIndex = 80;
      this.txtCoupon.TextAlign = HorizontalAlignment.Right;
      this.txtCoupon.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.txtMaxAmt.Location = new Point(126, 287);
      this.txtMaxAmt.MaxLength = 12;
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.ReadOnly = true;
      this.txtMaxAmt.Size = new Size(93, 20);
      this.txtMaxAmt.TabIndex = 140;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.txtMaxAmt.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label16.AutoSize = true;
      this.label16.Location = new Point(3, (int) sbyte.MaxValue);
      this.label16.Name = "label16";
      this.label16.Size = new Size(30, 14);
      this.label16.TabIndex = 0;
      this.label16.Text = "Term";
      this.label24.AutoSize = true;
      this.label24.Location = new Point(3, 311);
      this.label24.Name = "label24";
      this.label24.Size = new Size(89, 14);
      this.label24.TabIndex = 0;
      this.label24.Text = "Commitment Date";
      this.txtTerm1.Location = new Point(126, 126);
      this.txtTerm1.MaxLength = 12;
      this.txtTerm1.Name = "txtTerm1";
      this.txtTerm1.Size = new Size(59, 20);
      this.txtTerm1.TabIndex = 60;
      this.txtTerm1.TextAlign = HorizontalAlignment.Right;
      this.txtTerm1.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.dpCommitmentDate.BackColor = SystemColors.Window;
      this.dpCommitmentDate.Location = new Point(126, 310);
      this.dpCommitmentDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpCommitmentDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpCommitmentDate.Name = "dpCommitmentDate";
      this.dpCommitmentDate.Size = new Size(104, 22);
      this.dpCommitmentDate.TabIndex = 150;
      this.dpCommitmentDate.ToolTip = "";
      this.dpCommitmentDate.Value = new DateTime(0L);
      this.dpCommitmentDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(3, 104);
      this.label15.Name = "label15";
      this.label15.Size = new Size(73, 14);
      this.label15.TabIndex = 0;
      this.label15.Text = "Program Type";
      this.cboTradeDescription.FormattingEnabled = true;
      this.cboTradeDescription.Location = new Point(126, 56);
      this.cboTradeDescription.Name = "cboTradeDescription";
      this.cboTradeDescription.Size = new Size(161, 22);
      this.cboTradeDescription.TabIndex = 30;
      this.cboTradeDescription.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.cboProgramType.FormattingEnabled = true;
      this.cboProgramType.Items.AddRange(new object[2]
      {
        (object) "Fixed",
        (object) "Adjustable"
      });
      this.cboProgramType.Location = new Point(126, 102);
      this.cboProgramType.Name = "cboProgramType";
      this.cboProgramType.Size = new Size(161, 22);
      this.cboProgramType.TabIndex = 50;
      this.cboProgramType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label13.AutoSize = true;
      this.label13.Location = new Point(3, 58);
      this.label13.Name = "label13";
      this.label13.Size = new Size(92, 14);
      this.label13.TabIndex = 0;
      this.label13.Text = "Trade Description";
      this.label14.AutoSize = true;
      this.label14.Location = new Point(3, 81);
      this.label14.Name = "label14";
      this.label14.Size = new Size(73, 14);
      this.label14.TabIndex = 0;
      this.label14.Text = "Security Type";
      this.cboSecurityType.FormattingEnabled = true;
      this.cboSecurityType.Location = new Point(126, 79);
      this.cboSecurityType.Name = "cboSecurityType";
      this.cboSecurityType.Size = new Size(161, 22);
      this.cboSecurityType.TabIndex = 40;
      this.cboSecurityType.SelectedIndexChanged += new EventHandler(this.cboSecurityType_SelectedIndexChanged);
      this.cboSecurityType.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.tpHistory.Controls.Add((Control) this.pnlHistory);
      this.tpHistory.Location = new Point(4, 24);
      this.tpHistory.Name = "tpHistory";
      this.tpHistory.Padding = new Padding(0, 2, 2, 2);
      this.tpHistory.Size = new Size(1043, 623);
      this.tpHistory.TabIndex = 4;
      this.tpHistory.Tag = (object) "History";
      this.tpHistory.Text = "Notes/History";
      this.tpHistory.UseVisualStyleBackColor = true;
      this.tpHistory.Resize += new EventHandler(this.tpHistory_Resize);
      this.pnlHistory.Controls.Add((Control) this.grpHistory);
      this.pnlHistory.Controls.Add((Control) this.grpNotes);
      this.pnlHistory.Dock = DockStyle.Fill;
      this.pnlHistory.Location = new Point(0, 2);
      this.pnlHistory.Name = "pnlHistory";
      this.pnlHistory.Size = new Size(1041, 619);
      this.pnlHistory.TabIndex = 4;
      this.grpHistory.Controls.Add((Control) this.btnExportHistory);
      this.grpHistory.Controls.Add((Control) this.gvHistory);
      this.grpHistory.HeaderForeColor = SystemColors.ControlText;
      this.grpHistory.Location = new Point(460, 0);
      this.grpHistory.Name = "grpHistory";
      this.grpHistory.Size = new Size(444, 388);
      this.grpHistory.TabIndex = 4;
      this.grpHistory.Text = "History";
      this.gvHistory.BorderStyle = BorderStyle.None;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column1";
      gvColumn6.Text = "Event Time";
      gvColumn6.Width = 125;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.SpringToFit = true;
      gvColumn7.Text = "Event";
      gvColumn7.Width = 192;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column3";
      gvColumn8.Text = "By";
      gvColumn8.Width = 125;
      this.gvHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(442, 361);
      this.gvHistory.TabIndex = 2;
      this.grpNotes.Controls.Add((Control) this.txtNotes);
      this.grpNotes.Controls.Add((Control) this.btnDateStamp);
      this.grpNotes.HeaderForeColor = SystemColors.ControlText;
      this.grpNotes.Location = new Point(0, 0);
      this.grpNotes.Name = "grpNotes";
      this.grpNotes.Size = new Size(452, 452);
      this.grpNotes.TabIndex = 3;
      this.grpNotes.Text = "Notes";
      this.txtNotes.BorderStyle = BorderStyle.None;
      this.txtNotes.Dock = DockStyle.Fill;
      this.txtNotes.Location = new Point(1, 26);
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = ScrollBars.Both;
      this.txtNotes.Size = new Size(450, 425);
      this.txtNotes.TabIndex = 2;
      this.txtNotes.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.btnDateStamp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDateStamp.BackColor = SystemColors.Control;
      this.btnDateStamp.Location = new Point(358, 2);
      this.btnDateStamp.Name = "btnDateStamp";
      this.btnDateStamp.Size = new Size(89, 22);
      this.btnDateStamp.TabIndex = 1;
      this.btnDateStamp.Text = "&Date Stamp";
      this.btnDateStamp.UseVisualStyleBackColor = true;
      this.btnDateStamp.Click += new EventHandler(this.btnDateStamp_Click);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblTradeName);
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1053, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 6;
      this.lblTradeName.AutoSize = true;
      this.lblTradeName.BackColor = Color.Transparent;
      this.lblTradeName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeName.Location = new Point(8, 8);
      this.lblTradeName.Name = "lblTradeName";
      this.lblTradeName.Size = new Size(84, 14);
      this.lblTradeName.TabIndex = 6;
      this.lblTradeName.Text = "<Trade Name>";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnList);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(436, 4);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(611, 22);
      this.flowLayoutPanel1.TabIndex = 5;
      this.txtOptionPremium.Location = new Point(126, 218);
      this.txtOptionPremium.MaxLength = 12;
      this.txtOptionPremium.Name = "txtOptionPremium";
      this.txtOptionPremium.Size = new Size(93, 20);
      this.txtOptionPremium.TabIndex = 115;
      this.txtOptionPremium.TextAlign = HorizontalAlignment.Right;
      this.txtOptionPremium.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 219);
      this.label7.Name = "label7";
      this.label7.Size = new Size(81, 14);
      this.label7.TabIndex = 4;
      this.label7.Text = "Option Premium";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpEditor);
      this.Controls.Add((Control) this.gradientPanel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (SecurityTradeEditor);
      this.Size = new Size(1053, 748);
      ((ISupportInitialize) this.btnExportHistory).EndInit();
      ((ISupportInitialize) this.btnList).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.grpEditor.ResumeLayout(false);
      this.tabTrade.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.grpLoanTrades.ResumeLayout(false);
      this.flpEligible.ResumeLayout(false);
      this.grpPairOffs.ResumeLayout(false);
      this.flpPairOffs.ResumeLayout(false);
      ((ISupportInitialize) this.btnEditPairOff).EndInit();
      this.grpTradeInfo.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.pnlTradeTop.ResumeLayout(false);
      this.pnlTradeTop.PerformLayout();
      this.tpHistory.ResumeLayout(false);
      this.pnlHistory.ResumeLayout(false);
      this.grpHistory.ResumeLayout(false);
      this.grpNotes.ResumeLayout(false);
      this.grpNotes.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
