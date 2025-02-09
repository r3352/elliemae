// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSEcmtAssignmentListScreen
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
  public class GSEcmtAssignmentListScreen : TradeAssignmentByTradeListScreenBase
  {
    private IContainer components;
    private TableContainer grpGSECommitment;
    private GridView gvEligible;
    private FlowLayoutPanel flpGSECommitments;
    private Button btnOpenDialog;
    private Button btnUnassign;
    private Button btnAssign;
    private TradeAssignmentByTradeProfitControl ctlProfit;
    private StandardIconButton siBtnExcel;

    public event EventHandler OpenDialogClicked;

    public event EventHandler AssignedClicked;

    public event EventHandler UnassignedClicked;

    public GSEcmtAssignmentListScreen()
    {
      this.InitializeComponent();
      this.getProfitControl().lblOpenAmtCaption.Text = "Open Cmt Amount:";
    }

    public GSEcmtAssignmentListScreen(ITradeEditorBase tradeEditor)
      : base(tradeEditor)
    {
      this.InitializeComponent();
      this.getProfitControl().lblOpenAmtCaption.Text = "Open Cmt Amount:";
    }

    public GSEcmtAssignmentListScreen(ITradeEditorBase tradeEditor, bool useByDialog)
      : base(tradeEditor, useByDialog)
    {
      this.InitializeComponent();
      this.getProfitControl().lblOpenAmtCaption.Text = "Open Cmt Amount:";
    }

    protected override GridView getAssignmentGrid() => this.gvEligible;

    protected override TradeAssignmentByTradeProfitControl getProfitControl() => this.ctlProfit;

    protected override StandardIconButton getExportButton() => this.siBtnExcel;

    protected override Button getOpenDialogButton() => this.btnOpenDialog;

    protected override Button getAssignButton() => this.btnAssign;

    protected override Button getUnassignButton() => this.btnUnassign;

    protected override string getTableLayoutFileName() => "GSECommitmentsListSmallScreenView";

    protected override ReportFieldDefs getFieldDefs()
    {
      return (ReportFieldDefs) TradeAssignedGSECmtFieldDefs.GetFieldDefs();
    }

    protected override string GetAssignedAmountDisplayName() => "Allocated Commitment Amount";

    public static TradeAssignmentByTradeBase[] GetAssigments(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.GseCommitmentManager.GetTradeAssigmentsByMbsPool(tradeId);
    }

    public static TradeAssignmentByTradeBase[] GetUnassignedAssigments(int tradeId)
    {
      return (TradeAssignmentByTradeBase[]) Session.GseCommitmentManager.GetUnassignedTradeAssigmentsByMbsPool(tradeId);
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
          criteria[index] = (QueryCriterion) new OrdinalValueCriterion("GseCommitmentDetails.TradeID", (object) ((GSECommitmentAssignment) assignments[index]).Trade.TradeID);
        if (filters != null)
          queryCriterion.And(QueryCriterion.Join(criteria, BinaryOperator.Or));
        else
          queryCriterion = QueryCriterion.Join(criteria, BinaryOperator.Or);
      }
      else if (filters != null)
        queryCriterion.And((QueryCriterion) new OrdinalValueCriterion("GseCommitmentDetails.TradeID", (object) -1));
      else
        queryCriterion = (QueryCriterion) new OrdinalValueCriterion("GseCommitmentDetails.TradeID", (object) -1);
      ICursor cursor = Session.GseCommitmentManager.OpenTradeCursor(new QueryCriterion[1]
      {
        queryCriterion
      }, (SortField[]) null, this.generateFieldList(), true, false);
      foreach (GSECommitmentViewModel commitmentViewModel in (GSECommitmentViewModel[]) cursor.GetItems(0, cursor.GetItemCount()))
      {
        GSECommitmentViewModel tv = commitmentViewModel;
        TradeAssignmentByTradeBase assignment = ((IEnumerable<TradeAssignmentByTradeBase>) assignments).Where<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
        {
          int? tradeId1 = p.TradeID;
          int tradeId2 = tv.TradeID;
          return tradeId1.GetValueOrDefault() == tradeId2 & tradeId1.HasValue;
        })).FirstOrDefault<TradeAssignmentByTradeBase>();
        this.getAssignmentGrid().Items.Add(this.createAssignmentGridItem((TradeViewModel) tv, assignment));
      }
      this.validateColumnSortMethod();
      if (this.UseByDialog)
        this.grpGSECommitment.Text = "GSE Commitments (" + cursor.GetItemCount().ToString() + ")";
      else
        this.grpGSECommitment.Text = "Commitments (" + cursor.GetItemCount().ToString() + ")";
      this.hideShowButtons();
    }

    protected override TableLayout getDemoTableLayout()
    {
      TableLayout demoTableLayout = new TableLayout();
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentDate", "Commitment Date", HorizontalAlignment.Left, 100));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.Name", "Commitment ID", HorizontalAlignment.Left, 120));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.ContractNumber", "Contract #", HorizontalAlignment.Left, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.CommitmentAmount", "Commitment Amount", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.OutstandingBalance", "Outstanding Balance", HorizontalAlignment.Right, 110));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentDetails.PairOffAmount", "Total Pair-Off Amount", HorizontalAlignment.Right, 130));
      demoTableLayout.AddColumn(new TableLayout.Column("GseCommitmentSummary.CompletionPercent", "Completion Percent", HorizontalAlignment.Left, 110));
      return demoTableLayout;
    }

    protected override bool validateAssignedAmountChange(
      TradeAssignmentByTradeBase assigment,
      Decimal newAmount)
    {
      if (this.TradeEditor.TradeAmount < newAmount)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The allocated commitment amount cannot be greater than the Fannie Mae PE MBS Pool Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (!(Session.GseCommitmentManager.GetTrade(assigment.TradeID.Value).CommitmentAmount < newAmount))
        return true;
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The allocated commitment amount cannot be greater than the Commitment Amount.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return false;
    }

    protected override string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        GSECommitmentReportFieldDef fieldByCriterionName = (GSECommitmentReportFieldDef) this.getFieldDefs().GetFieldByCriterionName(column.ColumnID);
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

    protected override GVItem createAssignmentGridItem(
      TradeViewModel tradeInfo,
      TradeAssignmentByTradeBase assignment)
    {
      GVItem assignmentGridItem = new GVItem();
      assignmentGridItem.Tag = (object) assignment;
      for (int index = 0; index < this.getAssignmentGrid().Columns.Count; ++index)
      {
        TableLayout.Column tag1 = (TableLayout.Column) this.getAssignmentGrid().Columns[index].Tag;
        string tag2 = tag1.Tag;
        string columnId = tag1.ColumnID;
        object obj = (object) null;
        GSECommitmentReportFieldDef fieldByCriterionName = (GSECommitmentReportFieldDef) this.getFieldDefs().GetFieldByCriterionName(tag1.ColumnID);
        if (fieldByCriterionName != null && tradeInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
        else if (this.getAssignmentGrid().Columns[index].Text == this.GetAssignedAmountDisplayName() && assignment != null)
          obj = (object) assignment.AssignedAmount.ToString("#,##0;;\"\"");
        assignmentGridItem.SubItems[index].Value = obj;
      }
      return assignmentGridItem;
    }

    public override void Save()
    {
      TradeAssignmentByTradeBase[] unassignedAssigments = GSEcmtAssignmentListScreen.GetUnassignedAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      TradeAssignmentByTradeBase[] assigments = GSEcmtAssignmentListScreen.GetAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      int? nullable;
      foreach (TradeAssignmentByTradeBase eligibleAssignment in this.GetEligibleAssignments())
      {
        TradeAssignmentByTradeBase assignment = eligibleAssignment;
        if (((IEnumerable<TradeAssignmentByTradeBase>) assigments).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
        {
          int? tradeId1 = p.TradeID;
          int? tradeId2 = assignment.TradeID;
          if (!(tradeId1.GetValueOrDefault() == tradeId2.GetValueOrDefault() & tradeId1.HasValue == tradeId2.HasValue))
            return false;
          int? assigneeTradeId1 = p.AssigneeTradeID;
          int? assigneeTradeId2 = assignment.AssigneeTradeID;
          return assigneeTradeId1.GetValueOrDefault() == assigneeTradeId2.GetValueOrDefault() & assigneeTradeId1.HasValue == assigneeTradeId2.HasValue;
        })))
        {
          IMbsPoolManager mbsPoolManager = Session.MbsPoolManager;
          nullable = assignment.AssigneeTradeID;
          int tradeId = nullable.Value;
          nullable = assignment.TradeID;
          int assigneeTradeId = nullable.Value;
          mbsPoolManager.UnassignGSECommitmentToTrade(tradeId, assigneeTradeId);
        }
      }
      foreach (TradeAssignmentByTradeBase currentAssignment in this.GetCurrentAssignments())
      {
        TradeAssignmentByTradeBase assignment = currentAssignment;
        if (((IEnumerable<TradeAssignmentByTradeBase>) unassignedAssigments).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
        {
          int? tradeId3 = p.TradeID;
          int? tradeId4 = assignment.TradeID;
          if (!(tradeId3.GetValueOrDefault() == tradeId4.GetValueOrDefault() & tradeId3.HasValue == tradeId4.HasValue))
            return false;
          int? assigneeTradeId3 = p.AssigneeTradeID;
          int? assigneeTradeId4 = assignment.AssigneeTradeID;
          return assigneeTradeId3.GetValueOrDefault() == assigneeTradeId4.GetValueOrDefault() & assigneeTradeId3.HasValue == assigneeTradeId4.HasValue;
        })))
        {
          IMbsPoolManager mbsPoolManager = Session.MbsPoolManager;
          nullable = assignment.AssigneeTradeID;
          int tradeId = nullable.Value;
          nullable = assignment.TradeID;
          int assigneeTradeId = nullable.Value;
          Decimal assignedAmount = assignment.AssignedAmount;
          mbsPoolManager.AssignGSECommitmentToTrade(tradeId, assigneeTradeId, assignedAmount);
        }
        if (((IEnumerable<TradeAssignmentByTradeBase>) assigments).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
        {
          int? tradeId5 = p.TradeID;
          int? tradeId6 = assignment.TradeID;
          if (tradeId5.GetValueOrDefault() == tradeId6.GetValueOrDefault() & tradeId5.HasValue == tradeId6.HasValue)
          {
            int? assigneeTradeId5 = p.AssigneeTradeID;
            int? assigneeTradeId6 = assignment.AssigneeTradeID;
            if (assigneeTradeId5.GetValueOrDefault() == assigneeTradeId6.GetValueOrDefault() & assigneeTradeId5.HasValue == assigneeTradeId6.HasValue)
              return p.AssignedAmount != assignment.AssignedAmount;
          }
          return false;
        })))
        {
          IMbsPoolManager mbsPoolManager = Session.MbsPoolManager;
          nullable = assignment.AssigneeTradeID;
          int tradeId = nullable.Value;
          nullable = assignment.TradeID;
          int assigneeTradeId = nullable.Value;
          Decimal assignedAmount = assignment.AssignedAmount;
          mbsPoolManager.UpdateAssignedAmountToTrade(tradeId, assigneeTradeId, assignedAmount);
        }
      }
      this.setModified(false);
      this.refreshDataAfterSave();
    }

    protected override void refreshDataAfterSave()
    {
      TradeAssignmentByTradeBase[] unassignedAssigments = GSEcmtAssignmentListScreen.GetUnassignedAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      TradeAssignmentByTradeBase[] assigments = GSEcmtAssignmentListScreen.GetAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      this.Cursor = Cursors.WaitCursor;
      try
      {
        this.originalAssignedAmounts.Clear();
        this.RefreshData(assigments, unassignedAssigments);
      }
      catch
      {
        throw;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private void btnOpenDialog_Click(object sender, EventArgs e)
    {
      if (!this.isCurrentTradeSaved() && Utils.Dialog((IWin32Window) this, "The MBS Pool has to be saved first, do you want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
        this.TradeEditor.SaveTrade();
      if (!this.isCurrentTradeSaved())
        return;
      using (GSECmtAssignmentDialog assignmentDialog = new GSECmtAssignmentDialog(this.TradeEditor))
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
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one GSE Commitments.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more GSE Commitments will be assigned to this MBS Pool. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
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
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one GSE Commitment.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "One or more GSE Commitments assigned to this pool will be removed. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
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
        TradeAssignedGSECmtFieldDefs fieldDefs = TradeAssignedGSECmtFieldDefs.GetFieldDefs();
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
      this.grpGSECommitment = new TableContainer();
      this.gvEligible = new GridView();
      this.flpGSECommitments = new FlowLayoutPanel();
      this.btnOpenDialog = new Button();
      this.btnUnassign = new Button();
      this.btnAssign = new Button();
      this.siBtnExcel = new StandardIconButton();
      this.ctlProfit = new TradeAssignmentByTradeProfitControl();
      this.grpGSECommitment.SuspendLayout();
      this.flpGSECommitments.SuspendLayout();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      this.SuspendLayout();
      this.grpGSECommitment.Controls.Add((Control) this.gvEligible);
      this.grpGSECommitment.Controls.Add((Control) this.flpGSECommitments);
      this.grpGSECommitment.Dock = DockStyle.Fill;
      this.grpGSECommitment.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpGSECommitment.Location = new Point(0, 0);
      this.grpGSECommitment.Margin = new Padding(0);
      this.grpGSECommitment.Name = "grpGSECommitment";
      this.grpGSECommitment.Size = new Size(769, 185);
      this.grpGSECommitment.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.grpGSECommitment.TabIndex = 3;
      this.grpGSECommitment.Text = "GSE Commitments";
      this.gvEligible.AllowColumnReorder = true;
      this.gvEligible.BorderStyle = BorderStyle.None;
      this.gvEligible.Dock = DockStyle.Fill;
      this.gvEligible.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvEligible.Location = new Point(1, 26);
      this.gvEligible.Name = "gvEligible";
      this.gvEligible.Size = new Size(767, 158);
      this.gvEligible.TabIndex = 4;
      this.flpGSECommitments.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flpGSECommitments.BackColor = Color.Transparent;
      this.flpGSECommitments.Controls.Add((Control) this.btnOpenDialog);
      this.flpGSECommitments.Controls.Add((Control) this.btnUnassign);
      this.flpGSECommitments.Controls.Add((Control) this.btnAssign);
      this.flpGSECommitments.Controls.Add((Control) this.siBtnExcel);
      this.flpGSECommitments.FlowDirection = FlowDirection.RightToLeft;
      this.flpGSECommitments.Location = new Point(209, 2);
      this.flpGSECommitments.Margin = new Padding(0);
      this.flpGSECommitments.Name = "flpGSECommitments";
      this.flpGSECommitments.Size = new Size(558, 22);
      this.flpGSECommitments.TabIndex = 3;
      this.btnOpenDialog.Location = new Point(458, 0);
      this.btnOpenDialog.Margin = new Padding(0);
      this.btnOpenDialog.Name = "btnOpenDialog";
      this.btnOpenDialog.Size = new Size(100, 23);
      this.btnOpenDialog.TabIndex = 0;
      this.btnOpenDialog.Text = "Allocate Cmt";
      this.btnOpenDialog.UseVisualStyleBackColor = true;
      this.btnOpenDialog.Click += new EventHandler(this.btnOpenDialog_Click);
      this.btnUnassign.Location = new Point(358, 0);
      this.btnUnassign.Margin = new Padding(0);
      this.btnUnassign.Name = "btnUnassign";
      this.btnUnassign.Size = new Size(100, 23);
      this.btnUnassign.TabIndex = 1;
      this.btnUnassign.Text = "Deallocate Cmt";
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
      this.Controls.Add((Control) this.grpGSECommitment);
      this.Controls.Add((Control) this.ctlProfit);
      this.Name = nameof (GSEcmtAssignmentListScreen);
      this.Size = new Size(769, 204);
      this.grpGSECommitment.ResumeLayout(false);
      this.flpGSECommitments.ResumeLayout(false);
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      this.ResumeLayout(false);
    }
  }
}
