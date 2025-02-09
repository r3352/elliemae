// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeAssignmentByTradeListScreenBase
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeAssignmentByTradeListScreenBase : UserControl
  {
    protected static string assignedAmountName = "TradeAssignmentByTrade.AssignedAmount";
    protected static string sw = Tracing.SwOutsideLoan;
    private string className = nameof (TradeAssignmentByTradeListScreenBase);
    private TableLayout tableLayout;
    protected GridViewLayoutManager gvLayoutManager;
    private string priorValue = string.Empty;
    private List<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem> assignmentActionItems = new List<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>();
    private ITradeEditorBase tradeEditor;
    private bool readOnly;
    private bool modified;
    private bool useByDialog;
    private FieldFilterList filters;
    protected Dictionary<string, Decimal> originalAssignedAmounts = new Dictionary<string, Decimal>();

    protected virtual string GetAssignedAmountDisplayName() => "Allocated Pool Amount";

    public event EventHandler ModifiedEvent;

    public ITradeEditorBase TradeEditor => this.tradeEditor;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.hideShowButtons();
      }
    }

    public bool DataModified => !this.readOnly && this.modified;

    public bool UseByDialog
    {
      get => this.useByDialog;
      set => this.useByDialog = value;
    }

    public FieldFilterList Filters => this.filters;

    public TradeAssignmentByTradeListScreenBase()
    {
      this.DealocatedTrades = new List<TradeAssignmentByTradeBase>();
    }

    public TradeAssignmentByTradeListScreenBase(ITradeEditorBase tradeEditor)
      : this()
    {
      this.tradeEditor = tradeEditor;
    }

    public TradeAssignmentByTradeListScreenBase(ITradeEditorBase tradeEditor, bool useByDialog)
      : this(tradeEditor)
    {
      this.UseByDialog = useByDialog;
    }

    public void RefreshData(
      TradeAssignmentByTradeBase[] currentAssignments,
      TradeAssignmentByTradeBase[] eligibleAssignments,
      FieldFilterList filters = null)
    {
      this.initParams(currentAssignments, eligibleAssignments, filters);
      this.loadPersonalLayout(filters);
      this.hideShowButtons();
    }

    public List<TradeAssignmentByTradeBase> DealocatedTrades { get; private set; }

    public virtual void Save()
    {
      TradeAssignmentByTradeBase[] source1 = (TradeAssignmentByTradeBase[]) null;
      TradeAssignmentByTradeBase[] source2 = (TradeAssignmentByTradeBase[]) null;
      if (this.TradeEditor.CurrentTradeInfo.TradeType == TradeType.MbsPool)
      {
        source1 = SecurityTradeAssignmentListScreen.GetUnassignedAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
        source2 = SecurityTradeAssignmentListScreen.GetAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      }
      else if (this.TradeEditor.CurrentTradeInfo.TradeType == TradeType.SecurityTrade)
      {
        source1 = MbsPoolAssignmentListScreen.GetUnassignedAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
        source2 = MbsPoolAssignmentListScreen.GetAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      }
      else if (this.TradeEditor.CurrentTradeInfo.TradeType == TradeType.GSECommitment)
      {
        source1 = MbsPoolAssignmentListScreen.GetUnassignedAssigmentsByGseCommitment(this.TradeEditor.CurrentTradeInfo.TradeID);
        source2 = MbsPoolAssignmentListScreen.GetAssigmentsByGseCommitment(this.TradeEditor.CurrentTradeInfo.TradeID);
      }
      int? nullable;
      foreach (TradeAssignmentByTradeBase eligibleAssignment in this.GetEligibleAssignments())
      {
        TradeAssignmentByTradeBase assignment = eligibleAssignment;
        if (((IEnumerable<TradeAssignmentByTradeBase>) source2).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
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
          nullable = assignment.TradeID;
          int tradeId = nullable.Value;
          nullable = assignment.AssigneeTradeID;
          int assigneeTradeId = nullable.Value;
          mbsPoolManager.UnassignSecurityTradeToTrade(tradeId, assigneeTradeId);
        }
      }
      foreach (TradeAssignmentByTradeBase currentAssignment in this.GetCurrentAssignments())
      {
        TradeAssignmentByTradeBase assignment = currentAssignment;
        if (((IEnumerable<TradeAssignmentByTradeBase>) source1).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
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
          nullable = assignment.TradeID;
          int tradeId = nullable.Value;
          nullable = assignment.AssigneeTradeID;
          int assigneeTradeId = nullable.Value;
          Decimal assignedAmount = assignment.AssignedAmount;
          mbsPoolManager.AssignSecurityTradeToTrade(tradeId, assigneeTradeId, assignedAmount);
        }
        if (((IEnumerable<TradeAssignmentByTradeBase>) source2).Any<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
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
          nullable = assignment.TradeID;
          int tradeId = nullable.Value;
          nullable = assignment.AssigneeTradeID;
          int assigneeTradeId = nullable.Value;
          Decimal assignedAmount = assignment.AssignedAmount;
          mbsPoolManager.UpdateAssignedAmountToTrade(tradeId, assigneeTradeId, assignedAmount);
        }
      }
      this.setModified(false);
      this.refreshDataAfterSave();
    }

    protected virtual void refreshDataAfterSave()
    {
      TradeAssignmentByTradeBase[] eligibleAssignments = (TradeAssignmentByTradeBase[]) null;
      TradeAssignmentByTradeBase[] currentAssignments = (TradeAssignmentByTradeBase[]) null;
      if (this.TradeEditor.CurrentTradeInfo.TradeType == TradeType.MbsPool)
      {
        eligibleAssignments = SecurityTradeAssignmentListScreen.GetUnassignedAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
        currentAssignments = SecurityTradeAssignmentListScreen.GetAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      }
      else if (this.TradeEditor.CurrentTradeInfo.TradeType == TradeType.SecurityTrade)
      {
        eligibleAssignments = MbsPoolAssignmentListScreen.GetUnassignedAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
        currentAssignments = MbsPoolAssignmentListScreen.GetAssigments(this.TradeEditor.CurrentTradeInfo.TradeID);
      }
      this.Cursor = Cursors.WaitCursor;
      try
      {
        this.originalAssignedAmounts.Clear();
        this.RefreshData(currentAssignments, eligibleAssignments);
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

    public TradeAssignmentByTradeBase[] GetCurrentAssignments()
    {
      List<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem> assignmentActionItems = this.getCurrentAssignmentActionItems();
      TradeAssignmentByTradeBase[] currentAssignments = new TradeAssignmentByTradeBase[assignmentActionItems.Count];
      for (int index = 0; index < assignmentActionItems.Count; ++index)
        currentAssignments[index] = assignmentActionItems[index].Assignment;
      return currentAssignments;
    }

    public TradeAssignmentByTradeBase[] GetEligibleAssignments()
    {
      List<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem> assignmentActionItems = this.getEligibleAssignmentActionItems();
      TradeAssignmentByTradeBase[] eligibleAssignments = new TradeAssignmentByTradeBase[assignmentActionItems.Count];
      for (int index = 0; index < assignmentActionItems.Count; ++index)
        eligibleAssignments[index] = assignmentActionItems[index].Assignment;
      return eligibleAssignments;
    }

    public void AppendCurrentAssignments(TradeAssignmentByTradeBase assignment)
    {
      if (((IEnumerable<TradeAssignmentByTradeBase>) this.GetCurrentAssignments()).Where<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
      {
        int? tradeId1 = p.TradeID;
        int? tradeId2 = assignment.TradeID;
        if (!(tradeId1.GetValueOrDefault() == tradeId2.GetValueOrDefault() & tradeId1.HasValue == tradeId2.HasValue))
          return false;
        int? assigneeTradeId1 = p.AssigneeTradeID;
        int? assigneeTradeId2 = assignment.AssigneeTradeID;
        return assigneeTradeId1.GetValueOrDefault() == assigneeTradeId2.GetValueOrDefault() & assigneeTradeId1.HasValue == assigneeTradeId2.HasValue;
      })).FirstOrDefault<TradeAssignmentByTradeBase>() != null)
        return;
      this.RemoveEligibleAssignments(assignment);
    }

    public void AppendEligibleAssignments(TradeAssignmentByTradeBase assignment)
    {
      if (((IEnumerable<TradeAssignmentByTradeBase>) this.GetEligibleAssignments()).Where<TradeAssignmentByTradeBase>((Func<TradeAssignmentByTradeBase, bool>) (p =>
      {
        int? tradeId1 = p.TradeID;
        int? tradeId2 = assignment.TradeID;
        if (!(tradeId1.GetValueOrDefault() == tradeId2.GetValueOrDefault() & tradeId1.HasValue == tradeId2.HasValue))
          return false;
        int? assigneeTradeId1 = p.AssigneeTradeID;
        int? assigneeTradeId2 = assignment.AssigneeTradeID;
        return assigneeTradeId1.GetValueOrDefault() == assigneeTradeId2.GetValueOrDefault() & assigneeTradeId1.HasValue == assigneeTradeId2.HasValue;
      })).FirstOrDefault<TradeAssignmentByTradeBase>() != null)
        return;
      this.RemoveCurrentAssignments(assignment);
    }

    public void RemoveCurrentAssignments(TradeAssignmentByTradeBase assignment)
    {
      if (this.assignmentActionItems.RemoveAll((Predicate<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>) (x =>
      {
        int? tradeId1 = x.Assignment.TradeID;
        int? tradeId2 = assignment.TradeID;
        if (tradeId1.GetValueOrDefault() == tradeId2.GetValueOrDefault() & tradeId1.HasValue == tradeId2.HasValue)
        {
          int? assigneeTradeId1 = x.Assignment.AssigneeTradeID;
          int? assigneeTradeId2 = assignment.AssigneeTradeID;
          if (assigneeTradeId1.GetValueOrDefault() == assigneeTradeId2.GetValueOrDefault() & assigneeTradeId1.HasValue == assigneeTradeId2.HasValue)
            return x.ActionStatus == TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.CurrentAppended;
        }
        return false;
      })) <= 0)
        return;
      if (this.originalAssignedAmounts.ContainsKey(this.getHashKey(assignment)))
      {
        assignment.AssignedAmount = this.originalAssignedAmounts[this.getHashKey(assignment)];
        this.DealocatedTrades.Add(assignment);
      }
      this.assignmentActionItems.Add(new TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem(assignment, TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.EligibleAppended));
      this.setModified(true);
    }

    public void RemoveEligibleAssignments(TradeAssignmentByTradeBase assignment)
    {
      if (this.assignmentActionItems.RemoveAll((Predicate<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>) (x =>
      {
        int? tradeId1 = x.Assignment.TradeID;
        int? tradeId2 = assignment.TradeID;
        if (tradeId1.GetValueOrDefault() == tradeId2.GetValueOrDefault() & tradeId1.HasValue == tradeId2.HasValue)
        {
          int? assigneeTradeId1 = x.Assignment.AssigneeTradeID;
          int? assigneeTradeId2 = assignment.AssigneeTradeID;
          if (assigneeTradeId1.GetValueOrDefault() == assigneeTradeId2.GetValueOrDefault() & assigneeTradeId1.HasValue == assigneeTradeId2.HasValue)
            return x.ActionStatus == TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.EligibleAppended;
        }
        return false;
      })) <= 0)
        return;
      this.assignmentActionItems.Add(new TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem(assignment, TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.CurrentAppended));
      this.setModified(true);
    }

    public bool ValidateChanges()
    {
      bool flag = this.validateAndCalculateProfits();
      if (flag)
      {
        foreach (TradeAssignmentByTradeBase currentAssignment in this.GetCurrentAssignments())
        {
          flag = this.validateAssignedAmountChange(currentAssignment, currentAssignment.AssignedAmount);
          if (!flag)
            break;
        }
      }
      return flag;
    }

    protected virtual GridView getAssignmentGrid() => throw new NotImplementedException();

    protected virtual TradeAssignmentByTradeProfitControl getProfitControl()
    {
      throw new NotImplementedException();
    }

    protected virtual Button getOpenDialogButton() => throw new NotImplementedException();

    protected virtual StandardIconButton getExportButton() => throw new NotImplementedException();

    protected virtual Button getAssignButton() => throw new NotImplementedException();

    protected virtual Button getUnassignButton() => throw new NotImplementedException();

    protected virtual string getTableLayoutFileName() => throw new NotImplementedException();

    protected virtual void loadAssignments(FieldFilterList filters = null, bool isShowWarning = false)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        if (this.UseByDialog)
          this.loadAssignments(this.GetEligibleAssignments(), filters);
        else
          this.loadAssignments(this.GetCurrentAssignments(), filters);
        this.validateAndCalculateProfits(isShowWarning);
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

    protected virtual void loadAssignments(
      TradeAssignmentByTradeBase[] currentAssignments,
      TradeAssignmentByTradeBase[] eligibleAssignments,
      FieldFilterList filters = null,
      bool isShowWarning = true)
    {
      this.initParams(currentAssignments, eligibleAssignments, filters);
      this.loadAssignments(currentAssignments, filters);
      this.validateAndCalculateProfits(isShowWarning);
    }

    protected virtual void loadAssignments(
      TradeAssignmentByTradeBase[] assignments,
      FieldFilterList filters = null)
    {
      throw new NotImplementedException();
    }

    protected virtual ReportFieldDefs getFieldDefs() => throw new NotImplementedException();

    protected void setModified(bool flag)
    {
      this.modified = flag;
      if (this.ModifiedEvent == null)
        return;
      this.ModifiedEvent((object) this, new EventArgs());
    }

    protected virtual void hideShowButtons()
    {
      bool flag = false;
      if (this.TradeEditor.CurrentTradeInfo != null)
        flag = this.TradeEditor.CurrentTradeInfo.Locked;
      if (this.getOpenDialogButton() != null)
        this.getOpenDialogButton().Visible = false;
      if (this.getAssignButton() != null)
        this.getAssignButton().Visible = false;
      if (this.getUnassignButton() != null)
        this.getUnassignButton().Visible = false;
      if (!(!flag & !this.ReadOnly))
        return;
      if (!this.UseByDialog && this.getOpenDialogButton() != null)
        this.getOpenDialogButton().Visible = true;
      if (this.UseByDialog && this.getAssignButton() != null)
        this.getAssignButton().Visible = true;
      if (this.UseByDialog || this.getUnassignButton() == null)
        return;
      this.getUnassignButton().Visible = true;
    }

    protected virtual GVItem createAssignmentGridItem(
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
        TradeReportFieldDef fieldByCriterionName = (TradeReportFieldDef) this.getFieldDefs().GetFieldByCriterionName(tag1.ColumnID);
        if (fieldByCriterionName != null && tradeInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) tradeInfo, (EventHandler) null);
        else if (this.getAssignmentGrid().Columns[index].Text == this.GetAssignedAmountDisplayName() && assignment != null)
          obj = (object) assignment.AssignedAmount.ToString("#,##0;;\"\"");
        assignmentGridItem.SubItems[index].Value = obj;
      }
      return assignmentGridItem;
    }

    protected virtual void onAssignedAmountChange(object tag, Decimal amount)
    {
      TradeAssignmentByTradeBase assignment = (TradeAssignmentByTradeBase) tag;
      TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem assignmentActionItem = this.assignmentActionItems.Where<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>((Func<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem, bool>) (p =>
      {
        int? tradeId1 = p.Assignment.TradeID;
        int? tradeId2 = assignment.TradeID;
        if (!(tradeId1.GetValueOrDefault() == tradeId2.GetValueOrDefault() & tradeId1.HasValue == tradeId2.HasValue))
          return false;
        int? assigneeTradeId1 = p.Assignment.AssigneeTradeID;
        int? assigneeTradeId2 = assignment.AssigneeTradeID;
        return assigneeTradeId1.GetValueOrDefault() == assigneeTradeId2.GetValueOrDefault() & assigneeTradeId1.HasValue == assigneeTradeId2.HasValue;
      })).FirstOrDefault<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>();
      if (assignmentActionItem != null)
        assignmentActionItem.Assignment.AssignedAmount = amount;
      if (!this.validateAssignedAmountChange(assignment, amount))
        return;
      if (assignmentActionItem.Assignment.AssignedAmount > 0M)
        this.validateAndCalculateProfits();
      else
        this.validateAndCalculateProfits(false);
    }

    protected virtual bool validateAssignedAmountChange(
      TradeAssignmentByTradeBase assigment,
      Decimal newAmount)
    {
      return true;
    }

    protected bool isCurrentTradeSaved()
    {
      return this.TradeEditor.CurrentTradeInfo != null && this.TradeEditor.CurrentTradeInfo.TradeID > 0;
    }

    private List<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem> getCurrentAssignmentActionItems()
    {
      return this.assignmentActionItems.Where<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>((Func<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem, bool>) (q => q.ActionStatus == TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.CurrentAppended)).ToList<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>();
    }

    private List<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem> getEligibleAssignmentActionItems()
    {
      return this.assignmentActionItems.Where<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>((Func<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem, bool>) (q => q.ActionStatus == TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.EligibleAppended)).ToList<TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem>();
    }

    private void initParams(
      TradeAssignmentByTradeBase[] currentAssignments,
      TradeAssignmentByTradeBase[] eligibleAssignments,
      FieldFilterList filters = null)
    {
      this.filters = filters;
      this.assignmentActionItems.Clear();
      if (currentAssignments != null)
      {
        foreach (TradeAssignmentByTradeBase currentAssignment in currentAssignments)
        {
          this.assignmentActionItems.Add(new TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem(currentAssignment, TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.CurrentAppended));
          if (!this.originalAssignedAmounts.ContainsKey(this.getHashKey(currentAssignment)))
            this.originalAssignedAmounts.Add(this.getHashKey(currentAssignment), currentAssignment.AssignedAmount);
        }
      }
      if (eligibleAssignments == null)
        return;
      foreach (TradeAssignmentByTradeBase eligibleAssignment in eligibleAssignments)
      {
        this.assignmentActionItems.Add(new TradeAssignmentByTradeListScreenBase.TradeAssignmentActionItem(eligibleAssignment, TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus.EligibleAppended));
        if (!this.originalAssignedAmounts.ContainsKey(this.getHashKey(eligibleAssignment)))
          this.originalAssignedAmounts.Add(this.getHashKey(eligibleAssignment), eligibleAssignment.AssignedAmount);
      }
    }

    private string getHashKey(TradeAssignmentByTradeBase assignment)
    {
      int? nullable = assignment.TradeID;
      string str1 = nullable.ToString();
      nullable = assignment.AssigneeTradeID;
      string str2 = nullable.ToString();
      return str1 + "_" + str2;
    }

    private bool validateAndCalculateProfits(bool isShowWarning = true)
    {
      if (this.getProfitControl() == null)
        return true;
      this.tradeEditor.CurrentTradeInfo.TradeAmount = this.tradeEditor.TradeAmount;
      int num = this.getProfitControl().ValidateAndCalculate(this.tradeEditor.CurrentTradeInfo, this.GetCurrentAssignments(), isShowWarning) ? 1 : 0;
      if (!(this.tradeEditor.CurrentTradeInfo is MbsPoolInfo))
        return num != 0;
      if (!(((MbsPoolInfo) this.tradeEditor.CurrentTradeInfo).TBAOpenAmount != this.getProfitControl().OpenAmount))
        return num != 0;
      ((MbsPoolInfo) this.tradeEditor.CurrentTradeInfo).TBAOpenAmount = this.getProfitControl().OpenAmount;
      return num != 0;
    }

    private void loadPersonalLayout(FieldFilterList filters = null)
    {
      try
      {
        BinaryObject userSettings = Session.User.GetUserSettings(this.getTableLayoutFileName());
        this.setLayout(userSettings == null ? this.getDemoTableLayout() : userSettings.ToObject<TableLayout>(), filters);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeAssignmentByTradeListScreenBase.sw, this.className, TraceLevel.Error, "Error loading layout: " + (object) ex);
      }
    }

    private void setLayout(TableLayout layOut, FieldFilterList filters = null)
    {
      this.tableLayout = layOut;
      this.TradeEditor.SuspendEvents = true;
      this.applyTableLayout(layOut);
      this.TradeEditor.SuspendEvents = false;
      this.loadAssignments(filters);
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.validateTableLayout(layout);
      layout.InsertColumn(0, new TableLayout.Column(TradeAssignmentByTradeListScreenBase.assignedAmountName, this.GetAssignedAmountDisplayName(), HorizontalAlignment.Left, 100));
      this.gvLayoutManager.ApplyLayout(layout, false);
      foreach (GVColumn column in this.getAssignmentGrid().Columns)
      {
        if (column.Text == this.GetAssignedAmountDisplayName())
        {
          column.ActivatedEditorType = this.tradeEditor.CurrentTradeInfo.Status == TradeStatus.Archived ? GVActivatedEditorType.None : GVActivatedEditorType.TextBox;
          break;
        }
      }
      this.getAssignmentGrid().SelectedIndexChanged += new EventHandler(this.gvAssignment_SelectedIndexChanged);
      this.getAssignmentGrid().EditorOpening += new GVSubItemEditingEventHandler(this.gvAssignment_EditorOpening);
      this.getAssignmentGrid().EditorClosing += new GVSubItemEditingEventHandler(this.gvAssignment_EditorClosing);
    }

    private void validateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = this.getFieldDefs().GetFieldByCriterionName(column.ColumnID);
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
      foreach (ReportFieldDef fieldDef in (ReportFieldDefContainer) this.getFieldDefs())
      {
        if (fullTableLayout.GetColumnByID(fieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(fieldDef.ToTableLayoutColumn());
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private GridViewLayoutManager createLayoutManager()
    {
      TableLayout nonCustomizableColumnLayout = new TableLayout();
      nonCustomizableColumnLayout.InsertColumn(0, new TableLayout.Column(TradeAssignmentByTradeListScreenBase.assignedAmountName, this.GetAssignedAmountDisplayName(), HorizontalAlignment.Left, 100));
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.getAssignmentGrid(), this.getFullTableLayout(), this.getDemoTableLayout(), nonCustomizableColumnLayout);
      layoutManager.LayoutChanged += new EventHandler(this.onLayoutChanged);
      return layoutManager;
    }

    private void onLayoutChanged(object sender, EventArgs e)
    {
      if (this.TradeEditor.SuspendEvents)
        return;
      this.tableLayout = this.gvLayoutManager.GetCurrentLayout();
      using (BinaryObject data = new BinaryObject((IXmlSerializable) this.tableLayout))
        Session.User.SaveUserSettings(this.getTableLayoutFileName(), data);
      this.applyTableLayout(this.tableLayout);
      this.loadAssignments();
    }

    protected virtual TableLayout getDemoTableLayout() => new TableLayout();

    protected virtual string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        TradeReportFieldDef fieldByCriterionName = (TradeReportFieldDef) this.getFieldDefs().GetFieldByCriterionName(column.ColumnID);
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

    private void gvAssignment_SelectedIndexChanged(object sender, EventArgs e)
    {
      int count = this.getAssignmentGrid().SelectedItems.Count;
      this.getExportButton().Enabled = count > 0;
    }

    private void gvAssignment_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      string str = e.EditorControl.Text.Trim();
      TradeAssignmentByTradeBase tag = (TradeAssignmentByTradeBase) e.SubItem.Item.Tag;
      if (tag == null)
        return;
      if (Utils.IsDecimal((object) str))
      {
        Decimal amount = Utils.ParseDecimal((object) str, 0M);
        if (!(amount != tag.AssignedAmount))
          return;
        e.EditorControl.Text = amount.ToString("#,##0;;\"\"");
        this.onAssignedAmountChange(e.SubItem.Item.Tag, amount);
        this.setModified(true);
      }
      else if (!string.IsNullOrEmpty(str))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Assigned amount must be a number.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (!(tag.AssignedAmount > 0M))
          return;
        this.onAssignedAmountChange(e.SubItem.Item.Tag, 0M);
        this.setModified(true);
      }
    }

    private void gvAssignment_EditorOpening(object source, GVSubItemEditingEventArgs e)
    {
      TextBoxFormatter.Attach((TextBox) e.EditorControl, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      this.priorValue = e.EditorControl.Text.Trim();
    }

    protected bool exportTrades()
    {
      if (this.getAssignmentGrid().Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You trade/pool list cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.getAssignmentGrid().SelectedItems.Count > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You trade/pool list cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.exportSelectedRowsToExcel();
      return true;
    }

    private void exportSelectedRowsToExcel()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.getAssignmentGrid(), this.getFieldDefs(), true);
      excelHandler.CreateExcel();
    }

    private enum TradeAssignmentActionStatus
    {
      CurrentAppended,
      EligibleAppended,
    }

    private class TradeAssignmentActionItem
    {
      public TradeAssignmentByTradeBase Assignment { get; set; }

      public TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus ActionStatus { get; set; }

      public TradeAssignmentActionItem(
        TradeAssignmentByTradeBase assignment,
        TradeAssignmentByTradeListScreenBase.TradeAssignmentActionStatus actionStatus)
      {
        this.Assignment = assignment;
        this.ActionStatus = actionStatus;
      }
    }
  }
}
