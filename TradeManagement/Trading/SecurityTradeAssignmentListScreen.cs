// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeAssignmentListScreen
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
  public class SecurityTradeAssignmentListScreen : TradeAssignmentByTradeListScreenBase
  {
    private IContainer components;
    private TableContainer grpSecurityTrades;
    private GridView gvEligible;
    private FlowLayoutPanel flpSecurityTrades;
    private Button btnOpenDialog;
    private Button btnUnassign;
    private Button btnAssign;
    private TradeAssignmentByTradeProfitControl ctlProfit;
    private StandardIconButton siBtnExcel;

    public event EventHandler OpenDialogClicked;

    public event EventHandler AssignedClicked;

    public event EventHandler UnassignedClicked;

    public SecurityTradeAssignmentListScreen() => this.InitializeComponent();

    public SecurityTradeAssignmentListScreen(ITradeEditorBase tradeEditor)
      : base(tradeEditor)
    {
      this.InitializeComponent();
    }

    public SecurityTradeAssignmentListScreen(ITradeEditorBase tradeEditor, bool useByDialog)
      : base(tradeEditor, useByDialog)
    {
      this.InitializeComponent();
    }

    protected override GridView getAssignmentGrid() => this.gvEligible;

    protected override TradeAssignmentByTradeProfitControl getProfitControl() => this.ctlProfit;

    protected override StandardIconButton getExportButton() => this.siBtnExcel;

    protected override Button getOpenDialogButton() => this.btnOpenDialog;

    protected override Button getAssignButton() => this.btnAssign;

    protected override Button getUnassignButton() => this.btnUnassign;

    protected override string getTableLayoutFileName() => "SecurityTradeListSmallScreenView";

    protected override ReportFieldDefs getFieldDefs()
    {
      return (ReportFieldDefs) TradeAssignedSecurityTradeFieldDefs.GetFieldDefs();
    }

    public static TradeAssignmentByTradeBase[] GetAssigments(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.MbsPoolManager.GetTradeAssigments(tradeId);
    }

    public static TradeAssignmentByTradeBase[] GetUnassignedAssigments(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.MbsPoolManager.GetUnassignedTradeAssigments(tradeId);
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
          criteria[index] = (QueryCriterion) new OrdinalValueCriterion("SecurityTradeDetails.TradeID", (object) ((MbsPoolAssignment) assignments[index]).AssigneeTrade.TradeID);
        if (filters != null)
          queryCriterion.And(QueryCriterion.Join(criteria, BinaryOperator.Or));
        else
          queryCriterion = QueryCriterion.Join(criteria, BinaryOperator.Or);
      }
      else if (filters != null)
        queryCriterion.And((QueryCriterion) new OrdinalValueCriterion("SecurityTradeDetails.TradeID", (object) -1));
      else
        queryCriterion = (QueryCriterion) new OrdinalValueCriterion("SecurityTradeDetails.TradeID", (object) -1);
      ICursor cursor = Session.SecurityTradeManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, this.generateFieldList(), true, false);
      foreach (SecurityTradeViewModel securityTradeViewModel in (SecurityTradeViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
      {
        SecurityTradeViewModel tv = securityTradeViewModel;
        TradeAssignmentByTradeBase assignment = ((IEnumerable<TradeAssignmentByTradeBase>) assignments).Where<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
        {
          int? assigneeTradeId = p.AssigneeTradeID;
          int tradeId = tv.TradeID;
          return assigneeTradeId.GetValueOrDefault() == tradeId & assigneeTradeId.HasValue;
        })).FirstOrDefault<TradeAssignmentByTradeBase>();
        this.getAssignmentGrid().Items.Add(this.createAssignmentGridItem((TradeViewModel) tv, assignment));
      }
      this.validateColumnSortMethod();
      this.grpSecurityTrades.Text = "Security Trades (" + cursor.GetItemCount().ToString() + ")";
      this.hideShowButtons();
    }

    protected override TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.Name", "Security ID", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.SecurityType", "Security Type", HorizontalAlignment.Left, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.Coupon", "Coupon", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.SettlementDate", "Settlement Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.DealerName", "Dealer", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.TradeAmount", "Trade Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.MinAmount", "Min. Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeDetails.MaxAmount", "Max. Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeSummary.TotalAmount", "Assigned Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("SecurityTradeSummary.CompletionPercent", "Completion", HorizontalAlignment.Left, 110));
      return demoTableLayout;
    }

    protected override bool validateAssignedAmountChange(
      TradeAssignmentByTradeBase assigment,
      Decimal newAmount)
    {
      SecurityTradeInfo trade = Session.SecurityTradeManager.GetTrade(assigment.AssigneeTradeID.Value);
      if (trade == null)
        return false;
      if (!(trade.TradeAmount < newAmount))
        return true;
      int num = (int) Utils.Dialog((IWin32Window) this, "The allocated MBS Pool amount cannot be greater than the Trade Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    private void btnOpenDialog_Click(object sender, EventArgs e)
    {
      if (!this.isCurrentTradeSaved() && Utils.Dialog((IWin32Window) this, "The MBS Pool has to be saved first, do you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.TradeEditor.SaveTrade();
      if (!this.isCurrentTradeSaved())
        return;
      using (SecurityTradeAssignmentDialog assignmentDialog = new SecurityTradeAssignmentDialog(this.TradeEditor))
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
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one security trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more security trades will be assigned to this MBS Pool. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
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
      if (this.getAssignmentGrid().SelectedItems == null || this.getAssignmentGrid().SelectedItems.Count < 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one security trade.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more security trades assigned to this MBS Pool will be removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
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
        TradeAssignedSecurityTradeFieldDefs fieldDefs = TradeAssignedSecurityTradeFieldDefs.GetFieldDefs();
        if (tag.ColumnID == "TradeAssignmentByTrade.AssignedAmount" || fieldDefs.GetFieldByCriterionName(tag.ColumnID).FieldType == FieldTypes.IsNumeric)
          this.gvEligible.Columns[nColumnIndex].SortMethod = GVSortMethod.Numeric;
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
      this.grpSecurityTrades.Size = new Size(769, 185);
      this.grpSecurityTrades.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpSecurityTrades.TabIndex = 3;
      this.grpSecurityTrades.Text = "Security Trades";
      this.gvEligible.AllowColumnReorder = true;
      this.gvEligible.BorderStyle = BorderStyle.None;
      this.gvEligible.Dock = DockStyle.Fill;
      this.gvEligible.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvEligible.Location = new Point(1, 26);
      this.gvEligible.Name = "gvEligible";
      this.gvEligible.Size = new Size(767, 158);
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
      this.flpSecurityTrades.Size = new Size(558, 22);
      this.flpSecurityTrades.TabIndex = 3;
      this.btnOpenDialog.Location = new Point(458, 0);
      this.btnOpenDialog.Margin = new Padding(0);
      this.btnOpenDialog.Name = "btnOpenDialog";
      this.btnOpenDialog.Size = new Size(100, 23);
      this.btnOpenDialog.TabIndex = 0;
      this.btnOpenDialog.Text = "Allocate TBA";
      this.btnOpenDialog.UseVisualStyleBackColor = true;
      this.btnOpenDialog.Click += new EventHandler(this.btnOpenDialog_Click);
      this.btnUnassign.Location = new Point(358, 0);
      this.btnUnassign.Margin = new Padding(0);
      this.btnUnassign.Name = "btnUnassign";
      this.btnUnassign.Size = new Size(100, 23);
      this.btnUnassign.TabIndex = 1;
      this.btnUnassign.Text = "Deallocate TBA";
      this.btnUnassign.UseVisualStyleBackColor = true;
      this.btnUnassign.Click += new EventHandler(this.btnUnassign_Click);
      this.btnAssign.Location = new Point(283, 0);
      this.btnAssign.Margin = new Padding(0);
      this.btnAssign.Name = "btnAssign";
      this.btnAssign.Size = new Size(75, 23);
      this.btnAssign.TabIndex = 2;
      this.btnAssign.Text = "Allocate";
      this.btnAssign.UseVisualStyleBackColor = true;
      this.btnAssign.Click += new EventHandler(this.btnAssign_Click);
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Enabled = false;
      this.siBtnExcel.Location = new Point(265, 4);
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
      this.ctlProfit.Location = new Point(0, 185);
      this.ctlProfit.Name = "ctlProfit";
      this.ctlProfit.Size = new Size(769, 19);
      this.ctlProfit.TabIndex = 4;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.grpSecurityTrades);
      this.Controls.Add((Control) this.ctlProfit);
      this.Name = nameof (SecurityTradeAssignmentListScreen);
      this.Size = new Size(769, 204);
      this.grpSecurityTrades.ResumeLayout(false);
      this.flpSecurityTrades.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      this.ResumeLayout(false);
    }
  }
}
