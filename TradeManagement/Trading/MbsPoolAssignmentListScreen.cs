// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolAssignmentListScreen
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.ReportFieldDefinitions;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MbsPoolAssignmentListScreen : TradeAssignmentByTradeListScreenBase
  {
    private string poolName = "MBS Pool";
    private string tableLayoutFileName = "MbsPoolListSmallScreenView";
    private string tradeType = "trade";
    private IContainer components;
    private TableContainer grpSecurityTrades;
    private GridView gvEligible;
    private FlowLayoutPanel flpSecurityTrades;
    private Button btnOpenDialog;
    private Button btnAssign;
    private Button btnUnassign;
    private TradeAssignmentByTradeProfitControl ctlProfit;
    private StandardIconButton siBtnExcel;

    public event EventHandler OpenDialogClicked;

    public event EventHandler AssignedClicked;

    public event EventHandler UnassignedClicked;

    public MbsPoolAssignmentListScreen() => this.init();

    public MbsPoolAssignmentListScreen(ITradeEditorBase tradeEditor)
      : base(tradeEditor)
    {
      this.init();
    }

    public MbsPoolAssignmentListScreen(ITradeEditorBase tradeEditor, bool useByDialog)
      : base(tradeEditor, useByDialog)
    {
      this.init();
    }

    private void init()
    {
      this.InitializeComponent();
      this.ctlProfit.HideOpenAmount();
      this.ctlProfit.ShowAllocatedPoolAmount();
      this.grpSecurityTrades.Text = this.poolName + "s";
    }

    public string PoolName
    {
      set
      {
        this.poolName = value;
        this.grpSecurityTrades.Text = this.poolName + "s";
      }
      get => this.poolName;
    }

    protected string TableLayoutFileName
    {
      set => this.tableLayoutFileName = value;
    }

    public string TradeType
    {
      set => this.tradeType = value;
    }

    protected override GridView getAssignmentGrid() => this.gvEligible;

    protected override TradeAssignmentByTradeProfitControl getProfitControl() => this.ctlProfit;

    protected override StandardIconButton getExportButton() => this.siBtnExcel;

    protected override Button getOpenDialogButton() => this.btnOpenDialog;

    protected override Button getAssignButton() => this.btnAssign;

    protected override Button getUnassignButton() => this.btnUnassign;

    protected override string getTableLayoutFileName() => this.tableLayoutFileName;

    protected override ReportFieldDefs getFieldDefs()
    {
      return (ReportFieldDefs) TradeAssignedMbsPoolFieldDefs.GetFieldDefs();
    }

    public static TradeAssignmentByTradeBase[] GetAssigments(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.MbsPoolManager.GetTradeAssigmentsBySecurityTrade(tradeId);
    }

    public static TradeAssignmentByTradeBase[] GetUnassignedAssigments(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.MbsPoolManager.GetUnassignedTradeAssigmentsBySecurityTrade(tradeId);
    }

    public static TradeAssignmentByTradeBase[] GetAssigmentsByGseCommitment(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.MbsPoolManager.GetTradeAssigmentsByGseCommitment(tradeId);
    }

    public static TradeAssignmentByTradeBase[] GetUnassignedAssigmentsByGseCommitment(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.MbsPoolManager.GetUnassignedTradeAssigmentsByGseCommitment(tradeId);
    }

    protected override void loadAssignments(
      TradeAssignmentByTradeBase[] assignments,
      FieldFilterList filters = null)
    {
      this.getAssignmentGrid().Items.Clear();
      if (assignments == null)
        return;
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (filters != null)
        queryCriterion = filters.CreateEvaluator().ToQueryCriterion(FilterEvaluationOption.None);
      if (assignments.Length != 0)
      {
        QueryCriterion[] criteria = new QueryCriterion[assignments.Length];
        for (int index = 0; index < assignments.Length; ++index)
          criteria[index] = (QueryCriterion) new OrdinalValueCriterion("MbsPoolDetails.TradeID", (object) ((MbsPoolAssignment) assignments[index]).Trade.TradeID);
        if (filters != null)
          queryCriterion.And(QueryCriterion.Join(criteria, BinaryOperator.Or));
        else
          queryCriterion = QueryCriterion.Join(criteria, BinaryOperator.Or);
      }
      else if (filters != null)
        queryCriterion.And((QueryCriterion) new OrdinalValueCriterion("MbsPoolDetails.TradeID", (object) -1));
      else
        queryCriterion = (QueryCriterion) new OrdinalValueCriterion("MbsPoolDetails.TradeID", (object) -1);
      ICursor cursor = Session.MbsPoolManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, this.generateFieldList(), true, false);
      foreach (MbsPoolViewModel mbsPoolViewModel in (MbsPoolViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
      {
        MbsPoolViewModel tv = mbsPoolViewModel;
        TradeAssignmentByTradeBase assignment = ((IEnumerable<TradeAssignmentByTradeBase>) assignments).Where<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
        {
          int? tradeId1 = p.TradeID;
          int tradeId2 = tv.TradeID;
          return tradeId1.GetValueOrDefault() == tradeId2 & tradeId1.HasValue;
        })).FirstOrDefault<TradeAssignmentByTradeBase>();
        this.getAssignmentGrid().Items.Add(this.createAssignmentGridItem((TradeViewModel) tv, assignment));
      }
      this.validateColumnSortMethod();
      if (!this.grpSecurityTrades.Text.Contains("("))
        this.grpSecurityTrades.Text = this.grpSecurityTrades.Text + " (" + cursor.GetItemCount().ToString() + ")";
      else
        this.grpSecurityTrades.Text = this.grpSecurityTrades.Text.Substring(0, this.grpSecurityTrades.Text.IndexOf(" (")) + " (" + cursor.GetItemCount().ToString() + ")";
      this.hideShowButtons();
    }

    protected override TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.Name", "Pool ID", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.PoolNumber", "Pool Number", HorizontalAlignment.Left, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.TradeAmount", "Pool Amount", HorizontalAlignment.Right, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.AmortizationType", "Amortization Type", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("MbsPoolDetails.Term", "Term", HorizontalAlignment.Left, 90));
      demoTableLayout.AddColumn(new TableLayout.Column("TradesMasterContract1.ContractNumber", "Master #", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("TradeMbsPoolSummary.TotalAmount", "Assigned Amount", HorizontalAlignment.Right, 110));
      return demoTableLayout;
    }

    protected override bool validateAssignedAmountChange(
      TradeAssignmentByTradeBase assigment,
      Decimal newAmount)
    {
      MbsPoolInfo trade = Session.MbsPoolManager.GetTrade(assigment.TradeID.Value);
      if (trade == null)
        return false;
      if (!(trade.TradeAmount < newAmount))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "The allocated pool amount cannot be greater than the " + this.poolName + " Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void btnOpenDialog_Click(object sender, EventArgs e)
    {
      if (!this.isCurrentTradeSaved() && Utils.Dialog((IWin32Window) this, string.Format("The {0} has to be saved first, do you want to proceed?", this.TradeEditor is GseCommitmentEditor ? (object) "GSE Commitment" : (object) "trade"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.TradeEditor.SaveTrade();
      if (!this.isCurrentTradeSaved())
        return;
      using (MbsPoolAssignmentDialog assignmentDialog = new MbsPoolAssignmentDialog(this.TradeEditor))
      {
        assignmentDialog.RefreshData(this.GetCurrentAssignments(), this.GetEligibleAssignments());
        if (assignmentDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          this.loadAssignments(assignmentDialog.GetCurrentAssignments(), assignmentDialog.GetEligibleAssignments());
          this.setModified(assignmentDialog.DataModified);
        }
      }
      if (this.OpenDialogClicked == null)
        return;
      this.OpenDialogClicked((object) this, new EventArgs());
    }

    private void btnAssign_Click(object sender, EventArgs e)
    {
      if (this.getAssignmentGrid().SelectedItems == null || this.getAssignmentGrid().SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one " + this.poolName + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more " + this.poolName + "s will be assigned to this " + this.tradeType + ". Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        foreach (GVItem selectedItem in this.getAssignmentGrid().SelectedItems)
        {
          if (selectedItem.Tag is TradeAssignmentByTradeBase tag)
            this.AppendCurrentAssignments(tag);
        }
        if (this.AssignedClicked == null)
          return;
        this.AssignedClicked((object) this, new EventArgs());
      }
    }

    private void btnUnassign_Click(object sender, EventArgs e)
    {
      if (!this.isCurrentTradeSaved() && Utils.Dialog((IWin32Window) this, string.Format("The {0} has to be saved first, do you want to proceed?", this.TradeEditor is GseCommitmentEditor ? (object) "GSE Commitment" : (object) "trade"), MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.TradeEditor.SaveTrade();
      if (!this.isCurrentTradeSaved())
        return;
      if (this.getAssignmentGrid().SelectedItems == null || this.getAssignmentGrid().SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one " + this.poolName + ".", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more " + this.poolName + "s assigned to this " + this.tradeType + " will be removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        foreach (GVItem selectedItem in this.getAssignmentGrid().SelectedItems)
        {
          if (selectedItem.Tag is TradeAssignmentByTradeBase tag)
            this.RemoveCurrentAssignments(tag);
        }
        this.loadAssignments(this.Filters);
        if (this.UnassignedClicked == null)
          return;
        this.UnassignedClicked((object) this, new EventArgs());
      }
    }

    private void siBtnExcel_Click(object sender, EventArgs e) => this.exportTrades();

    public void validateColumnSortMethod()
    {
      for (int nColumnIndex = 0; nColumnIndex < this.gvEligible.Columns.Count; ++nColumnIndex)
      {
        TableLayout.Column tag = (TableLayout.Column) this.gvEligible.Columns[nColumnIndex].Tag;
        TradeAssignedMbsPoolFieldDefs fieldDefs = TradeAssignedMbsPoolFieldDefs.GetFieldDefs();
        if (tag.ColumnID == "TradeAssignmentByTrade.AssignedAmount" || fieldDefs.GetFieldByCriterionName(tag.ColumnID).FieldType == FieldTypes.IsNumeric)
          this.gvEligible.Columns[nColumnIndex].SortMethod = GVSortMethod.Numeric;
        else if (tag.ColumnID == "MbsPoolDetails.SettlementMonth")
          this.gvEligible.Columns[nColumnIndex].SortMethod = GVSortMethod.Month;
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
      this.grpSecurityTrades = new TableContainer();
      this.gvEligible = new GridView();
      this.flpSecurityTrades = new FlowLayoutPanel();
      this.btnOpenDialog = new Button();
      this.btnUnassign = new Button();
      this.btnAssign = new Button();
      this.siBtnExcel = new StandardIconButton();
      this.ctlProfit = new TradeAssignmentByTradeProfitControl();
      this.grpSecurityTrades.SuspendLayout();
      this.flpSecurityTrades.SuspendLayout();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      this.SuspendLayout();
      this.grpSecurityTrades.Controls.Add((Control) this.gvEligible);
      this.grpSecurityTrades.Controls.Add((Control) this.flpSecurityTrades);
      this.grpSecurityTrades.Dock = DockStyle.Fill;
      this.grpSecurityTrades.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpSecurityTrades.Location = new Point(0, 0);
      this.grpSecurityTrades.Margin = new Padding(0);
      this.grpSecurityTrades.Name = "grpSecurityTrades";
      this.grpSecurityTrades.Size = new Size(755, 218);
      this.grpSecurityTrades.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpSecurityTrades.TabIndex = 3;
      this.grpSecurityTrades.Text = "MBS Pools";
      this.gvEligible.AllowColumnReorder = true;
      this.gvEligible.BorderStyle = BorderStyle.None;
      this.gvEligible.Dock = DockStyle.Fill;
      this.gvEligible.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvEligible.Location = new Point(1, 26);
      this.gvEligible.Name = "gvEligible";
      this.gvEligible.Size = new Size(753, 191);
      this.gvEligible.TabIndex = 4;
      this.flpSecurityTrades.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpSecurityTrades.BackColor = Color.Transparent;
      this.flpSecurityTrades.Controls.Add((Control) this.btnOpenDialog);
      this.flpSecurityTrades.Controls.Add((Control) this.btnUnassign);
      this.flpSecurityTrades.Controls.Add((Control) this.btnAssign);
      this.flpSecurityTrades.Controls.Add((Control) this.siBtnExcel);
      this.flpSecurityTrades.FlowDirection = FlowDirection.RightToLeft;
      this.flpSecurityTrades.Location = new Point(209, 2);
      this.flpSecurityTrades.Margin = new Padding(0);
      this.flpSecurityTrades.Name = "flpSecurityTrades";
      this.flpSecurityTrades.Size = new Size(544, 22);
      this.flpSecurityTrades.TabIndex = 3;
      this.btnOpenDialog.Location = new Point(444, 0);
      this.btnOpenDialog.Margin = new Padding(0);
      this.btnOpenDialog.Name = "btnOpenDialog";
      this.btnOpenDialog.Size = new Size(100, 23);
      this.btnOpenDialog.TabIndex = 0;
      this.btnOpenDialog.Text = "Allocate Pool";
      this.btnOpenDialog.UseVisualStyleBackColor = true;
      this.btnOpenDialog.Click += new EventHandler(this.btnOpenDialog_Click);
      this.btnUnassign.Location = new Point(344, 0);
      this.btnUnassign.Margin = new Padding(0);
      this.btnUnassign.Name = "btnUnassign";
      this.btnUnassign.Size = new Size(100, 23);
      this.btnUnassign.TabIndex = 1;
      this.btnUnassign.Text = "Deallocate Pool";
      this.btnUnassign.UseVisualStyleBackColor = true;
      this.btnUnassign.Click += new EventHandler(this.btnUnassign_Click);
      this.btnAssign.Location = new Point(269, 0);
      this.btnAssign.Margin = new Padding(0);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(75, 23);
      this.btnAssign.TabIndex = 2;
      this.btnAssign.Text = "Allocate";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Enabled = false;
      this.siBtnExcel.Location = new Point(251, 4);
      this.siBtnExcel.Margin = new Padding(3, 4, 2, 3);
      this.siBtnExcel.MouseDownImage = (Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(16, 16);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 10;
      this.siBtnExcel.TabStop = false;
      this.siBtnExcel.Click += new EventHandler(this.siBtnExcel_Click);
      this.ctlProfit.BackColor = SystemColors.Control;
      this.ctlProfit.Dock = DockStyle.Bottom;
      this.ctlProfit.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlProfit.Location = new Point(0, 218);
      this.ctlProfit.Name = "ctlProfit";
      this.ctlProfit.Size = new Size(755, 19);
      this.ctlProfit.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpSecurityTrades);
      this.Controls.Add((Control) this.ctlProfit);
      this.Name = nameof (MbsPoolAssignmentListScreen);
      this.Size = new Size(755, 237);
      this.grpSecurityTrades.ResumeLayout(false);
      this.flpSecurityTrades.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      this.ResumeLayout(false);
    }
  }
}
