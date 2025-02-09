// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.ConditionTrackingControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Documents;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class ConditionTrackingControl : UserControl, IRefreshContents
  {
    private LoanDataMgr loanDataMgr;
    private ConditionType condType;
    private GridViewDataManager gvConditionsMgr;
    private ConditionTrackingView currentView;
    private ConditionTrackingView currentStandardView;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType currentTemplateType;
    private FileSystemEntry currentFileSystemEntry;
    private Sessions.Session session;
    private Form frmEFolderDialog;
    private IContainer components;
    private Button btnDocuments;
    private Button btnRetrieve;
    private Button btnRequest;
    private VerticalSeparator separator;
    private StandardIconButton btnPrint;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnNew;
    private GridView gvConditions;
    private StandardIconButton btnEdit;
    private StandardIconButton btnExcel;
    private GroupContainer gcConditions;
    private StandardIconButton btnDelete;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem mnuItemNew1;
    private ToolStripMenuItem mnuItemEdit1;
    private ToolStripMenuItem mnuItemDuplicate1;
    private ToolStripMenuItem mnuItemDelete1;
    private ToolStripSeparator mnuItemSeparator1;
    private ToolStripMenuItem mnuItemExcel1;
    private ToolStripMenuItem mnuItemPrint1;
    private ToolStripMenuItem mnuItemInternal;
    private ToolStripSeparator mnuItemSeparator2;
    private ToolStripMenuItem mnuItemSelectAll;
    private ContextMenuStrip mnuConditions;
    private ToolStripMenuItem mnuItemNew2;
    private ToolStripMenuItem mnuItemDuplicate2;
    private ToolStripMenuItem mnuItemEdit2;
    private ToolStripMenuItem mnuItemDelete2;
    private ToolStripSeparator mnuItemSeparator3;
    private ToolStripMenuItem mnuItemExcel2;
    private ToolStripMenuItem mnuItemPrint2;
    private ToolStripSeparator mnuItemSeparator4;
    private ToolStripMenuItem mnuItemRequest;
    private ToolStripMenuItem mnuItemRetrieve;
    private ToolStripMenuItem mnuItemDocuments;
    private ToolStripMenuItem mnuItemExternal;
    private GradientPanel gradConditionsView;
    private StandardIconButton btnResetConditionView;
    private StandardIconButton btnManageConditionView;
    private StandardIconButton btnSaveConditionView;
    private ComboBoxEx cboConditionsView;
    private Label lblConditionsView;
    private Button btnDeliveryStatus;
    private Button btnCondResponses;

    public void CloseDetailInstances()
    {
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          UnderwritingDetailsDialog.CloseInstances();
          break;
        case ConditionType.PostClosing:
          PostClosingDetailsDialog.CloseInstances();
          break;
        case ConditionType.Preliminary:
          PreliminaryDetailsDialog.CloseInstances();
          break;
        case ConditionType.Sell:
          SellDetailsDialog.CloseInstances();
          break;
      }
    }

    public ConditionTrackingControl(
      LoanDataMgr loanDataMgr,
      ConditionType condType,
      Sessions.Session session,
      Form eFolderDialog = null)
    {
      this.InitializeComponent();
      this.session = session;
      this.frmEFolderDialog = eFolderDialog;
      this.loanDataMgr = loanDataMgr;
      this.condType = condType;
      this.initConditionList();
      this.initConditionViewList();
      this.applySecurity();
      if (condType != ConditionType.Sell)
      {
        if (this.pnlToolbar.Controls.Contains((Control) this.btnCondResponses))
          this.pnlToolbar.Controls.Remove((Control) this.btnCondResponses);
        if (!this.pnlToolbar.Controls.Contains((Control) this.btnDeliveryStatus))
          return;
        this.pnlToolbar.Controls.Remove((Control) this.btnDeliveryStatus);
      }
      else
      {
        eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
        if (folderAccessRights.CanUseDeliveryConditionResponse)
        {
          if (!this.pnlToolbar.Controls.Contains((Control) this.btnCondResponses))
            this.pnlToolbar.Controls.Add((Control) this.btnCondResponses);
        }
        else if (this.pnlToolbar.Controls.Contains((Control) this.btnCondResponses))
          this.pnlToolbar.Controls.Remove((Control) this.btnCondResponses);
        if (folderAccessRights.CanUseConditionDeliveryStatus)
        {
          if (this.pnlToolbar.Controls.Contains((Control) this.btnDeliveryStatus))
            return;
          this.pnlToolbar.Controls.Add((Control) this.btnDeliveryStatus);
        }
        else
        {
          if (!this.pnlToolbar.Controls.Contains((Control) this.btnDeliveryStatus))
            return;
          this.pnlToolbar.Controls.Remove((Control) this.btnDeliveryStatus);
        }
      }
    }

    private void initConditionList()
    {
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.initUnderwritingList();
          this.currentStandardView = GridViewDataManager.GetStandardUnderwritingConditionTrackingView(this.session);
          break;
        case ConditionType.PostClosing:
          this.initPostClosingList();
          this.currentStandardView = GridViewDataManager.StandardPostClosingConditionTrackingView;
          break;
        case ConditionType.Preliminary:
          this.initPreliminaryList();
          this.currentStandardView = GridViewDataManager.StandardPreliminaryConditionTrackingView;
          break;
        case ConditionType.Sell:
          this.initSellList();
          this.currentStandardView = GridViewDataManager.StandardSellConditionTrackingView;
          this.loanDataMgr.GetUpdatedSellConditionSetup();
          break;
      }
      this.currentTemplateType = TemplateSettingsTypeConverter.GetConditionTrackingViewType(this.condType);
    }

    private void gvConditionsMgr_FilterChanged(object sender, EventArgs e)
    {
      string str = "(" + this.gvConditions.VisibleItems.Count.ToString() + ")";
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.gcConditions.Text = "Underwriting Conditions " + str;
          break;
        case ConditionType.PostClosing:
          this.gcConditions.Text = "Post-Closing Conditions " + str;
          break;
        case ConditionType.Preliminary:
          this.gcConditions.Text = "Preliminary Conditions " + str;
          break;
        case ConditionType.Sell:
          this.gcConditions.Text = "Delivery Conditions " + str;
          break;
      }
    }

    private void gvConditionsMgr_LayoutChanged(object sender, EventArgs e)
    {
      this.loadConditionList();
    }

    private void loadConditionList()
    {
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      ConditionLog[] allConditions = logList.GetAllConditions(this.condType);
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      int[] usersAssignedRoles = this.loanDataMgr.AccessRules.GetUsersAssignedRoles();
      foreach (ConditionLog cond in allConditions)
      {
        Font font = (Font) null;
        if (cond.Comments.HasUnreviewedEntry(usersAssignedRoles))
          font = EncompassFonts.Normal2.Font;
        GVItem gvItem = this.gvConditions.Items.GetItemByTag((object) cond);
        if (gvItem == null)
          gvItem = this.gvConditionsMgr.AddItem(cond, allDocuments);
        else
          this.gvConditionsMgr.RefreshItem(gvItem, cond, allDocuments);
        foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
          subItem.Font = font;
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditions.Items)
      {
        if (Array.IndexOf<object>((object[]) allConditions, gvItem.Tag) < 0)
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvConditions.Items.Remove(gvItem);
      this.gvConditionsMgr.ApplyFilter();
      this.refreshToolbar();
    }

    private void loadConditionList(ConditionLog cond)
    {
      this.loadConditionList(new ConditionLog[1]{ cond });
    }

    private void loadConditionList(ConditionLog[] condList)
    {
      this.loadConditionList();
      this.gvConditions.SelectedItems.Clear();
      foreach (object cond in condList)
      {
        GVItem itemByTag = this.gvConditions.Items.GetItemByTag(cond);
        if (itemByTag != null)
          itemByTag.Selected = true;
      }
    }

    private ConditionLog[] getSelectedConditions()
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
        arrayList.Add(selectedItem.Tag);
      return (ConditionLog[]) arrayList.ToArray(typeof (ConditionLog));
    }

    private void gvConditions_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshToolbar();
    }

    private void initPreliminaryList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.FilterChanged += new EventHandler(this.gvConditionsMgr_FilterChanged);
      this.gvConditionsMgr.LayoutChanged += new EventHandler(this.gvConditionsMgr_LayoutChanged);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[23]
      {
        GridViewDataManager.AddedByColumn,
        GridViewDataManager.AddedDateColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.ExpectedDateColumn,
        GridViewDataManager.ReceivedByColumn,
        GridViewDataManager.ReceivedDateColumn,
        GridViewDataManager.RequestedByColumn,
        GridViewDataManager.RequestedDateColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.RerequestedByColumn,
        GridViewDataManager.RerequestedDateColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.FulfilledByColumn,
        GridViewDataManager.FulfilledDateColumn,
        GridViewDataManager.HasDocumentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.UnderwriterAccessColumn
      }, true);
    }

    private void addPreliminaryCondition()
    {
      using (AddPreliminaryDialog preliminaryDialog = new AddPreliminaryDialog(this.loanDataMgr))
      {
        if (preliminaryDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK || preliminaryDialog.Conditions == null || preliminaryDialog.Conditions.Length == 0)
          return;
        if (preliminaryDialog.Conditions[0].ConditionType != ConditionType.Preliminary)
          this.switchTabsandRefreshContent(preliminaryDialog.IsImportConditions, preliminaryDialog.Conditions);
        else
          this.loadConditionList(preliminaryDialog.Conditions);
      }
    }

    private void duplicatePreliminaryCondition()
    {
      PreliminaryConditionLog selectedCondition = (PreliminaryConditionLog) this.getSelectedConditions()[0];
      PreliminaryConditionLog preliminaryConditionLog = new PreliminaryConditionLog(Session.UserID, selectedCondition.PairId);
      preliminaryConditionLog.Title = selectedCondition.Title;
      preliminaryConditionLog.Description = selectedCondition.Description;
      preliminaryConditionLog.Source = selectedCondition.Source;
      preliminaryConditionLog.Category = selectedCondition.Category;
      preliminaryConditionLog.PriorTo = selectedCondition.PriorTo;
      preliminaryConditionLog.DaysTillDue = selectedCondition.DaysTillDue;
      preliminaryConditionLog.UnderwriterAccess = selectedCondition.UnderwriterAccess;
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) preliminaryConditionLog);
      this.loadConditionList((ConditionLog) preliminaryConditionLog);
    }

    private void editPreliminaryCondition()
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      if (selectedConditions.Length != 1)
        return;
      PreliminaryDetailsDialog.ShowInstance(this.loanDataMgr, (PreliminaryConditionLog) selectedConditions[0]);
    }

    private void initUnderwritingList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.FilterChanged += new EventHandler(this.gvConditionsMgr_FilterChanged);
      this.gvConditionsMgr.LayoutChanged += new EventHandler(this.gvConditionsMgr_LayoutChanged);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[35]
      {
        GridViewDataManager.AddedByColumn,
        GridViewDataManager.AddedDateColumn,
        GridViewDataManager.AllowToClearColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.ClearedByColumn,
        GridViewDataManager.ClearedDateColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.ExpirationDateColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.ExpectedDateColumn,
        GridViewDataManager.FulfilledByColumn,
        GridViewDataManager.FulfilledDateColumn,
        GridViewDataManager.RequestedByColumn,
        GridViewDataManager.RequestedDateColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.RerequestedByColumn,
        GridViewDataManager.RerequestedDateColumn,
        GridViewDataManager.HasDocumentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.PrintExternallyColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.ReceivedByColumn,
        GridViewDataManager.ReceivedDateColumn,
        GridViewDataManager.RejectedByColumn,
        GridViewDataManager.RejectedDateColumn,
        GridViewDataManager.ReviewedByColumn,
        GridViewDataManager.ReviewedDateColumn,
        GridViewDataManager.WaivedByColumn,
        GridViewDataManager.WaivedDateColumn
      }, true);
    }

    private void addUnderwritingCondition()
    {
      using (AddUnderwritingDialog underwritingDialog = new AddUnderwritingDialog(this.loanDataMgr))
      {
        if (underwritingDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK || underwritingDialog.Conditions == null || underwritingDialog.Conditions.Length == 0)
          return;
        if (underwritingDialog.Conditions[0].ConditionType != ConditionType.Underwriting)
          this.switchTabsandRefreshContent(underwritingDialog.IsImportConditions, underwritingDialog.Conditions);
        else
          this.loadConditionList(underwritingDialog.Conditions);
      }
    }

    private void duplicateUnderwritingCondition()
    {
      UnderwritingConditionLog selectedCondition = (UnderwritingConditionLog) this.getSelectedConditions()[0];
      UnderwritingConditionLog underwritingConditionLog = new UnderwritingConditionLog(Session.UserID, selectedCondition.PairId);
      underwritingConditionLog.Title = selectedCondition.Title;
      underwritingConditionLog.Description = selectedCondition.Description;
      underwritingConditionLog.Source = selectedCondition.Source;
      underwritingConditionLog.Category = selectedCondition.Category;
      underwritingConditionLog.PriorTo = selectedCondition.PriorTo;
      underwritingConditionLog.ForRoleID = selectedCondition.ForRoleID;
      underwritingConditionLog.AllowToClear = selectedCondition.AllowToClear;
      underwritingConditionLog.DaysTillDue = selectedCondition.DaysTillDue;
      underwritingConditionLog.IsInternal = selectedCondition.IsInternal;
      underwritingConditionLog.IsExternal = selectedCondition.IsExternal;
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) underwritingConditionLog);
      this.loadConditionList((ConditionLog) underwritingConditionLog);
    }

    private void editUnderwritingCondition()
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      if (selectedConditions.Length != 1)
        return;
      UnderwritingDetailsDialog.ShowInstance(this.loanDataMgr, (UnderwritingConditionLog) selectedConditions[0]);
    }

    private void initPostClosingList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.FilterChanged += new EventHandler(this.gvConditionsMgr_FilterChanged);
      this.gvConditionsMgr.LayoutChanged += new EventHandler(this.gvConditionsMgr_LayoutChanged);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[23]
      {
        GridViewDataManager.AddedByColumn,
        GridViewDataManager.AddedDateColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.ClearedByColumn,
        GridViewDataManager.ClearedDateColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.ExpectedDateColumn,
        GridViewDataManager.HasDocumentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.ReceivedByColumn,
        GridViewDataManager.ReceivedDateColumn,
        GridViewDataManager.RecipientColumn,
        GridViewDataManager.RequestedByColumn,
        GridViewDataManager.RequestedDateColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.RerequestedByColumn,
        GridViewDataManager.RerequestedDateColumn,
        GridViewDataManager.SentByColumn,
        GridViewDataManager.SentDateColumn
      }, true);
    }

    private void addPostClosingCondition()
    {
      using (AddPostClosingDialog postClosingDialog = new AddPostClosingDialog(this.loanDataMgr))
      {
        if (postClosingDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK || postClosingDialog.Conditions == null || postClosingDialog.Conditions.Length == 0)
          return;
        if (postClosingDialog.Conditions[0].ConditionType != ConditionType.PostClosing)
          this.switchTabsandRefreshContent(postClosingDialog.IsImportConditions, postClosingDialog.Conditions);
        else
          this.loadConditionList(postClosingDialog.Conditions);
      }
    }

    private void duplicatePostClosingCondition()
    {
      PostClosingConditionLog selectedCondition = (PostClosingConditionLog) this.getSelectedConditions()[0];
      PostClosingConditionLog closingConditionLog = new PostClosingConditionLog(Session.UserID, selectedCondition.PairId);
      closingConditionLog.Title = selectedCondition.Title;
      closingConditionLog.Description = selectedCondition.Description;
      closingConditionLog.Source = selectedCondition.Source;
      closingConditionLog.Recipient = selectedCondition.Recipient;
      closingConditionLog.DaysTillDue = selectedCondition.DaysTillDue;
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) closingConditionLog);
      this.loadConditionList((ConditionLog) closingConditionLog);
    }

    private void editPostClosingCondition()
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      if (selectedConditions.Length != 1)
        return;
      PostClosingDetailsDialog.ShowInstance(this.loanDataMgr, (PostClosingConditionLog) selectedConditions[0]);
    }

    private void initSellList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.FilterChanged += new EventHandler(this.gvConditionsMgr_FilterChanged);
      this.gvConditionsMgr.LayoutChanged += new EventHandler(this.gvConditionsMgr_LayoutChanged);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[36]
      {
        GridViewDataManager.AddedByColumn,
        GridViewDataManager.AddedDateColumn,
        GridViewDataManager.AllowToClearColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.ClearedByColumn,
        GridViewDataManager.ClearedDateColumn,
        GridViewDataManager.CondSourceColumn,
        GridViewDataManager.CondStatusColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.ExpirationDateColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.ExpectedDateColumn,
        GridViewDataManager.FulfilledByColumn,
        GridViewDataManager.FulfilledDateColumn,
        GridViewDataManager.RequestedByColumn,
        GridViewDataManager.RequestedDateColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.RerequestedByColumn,
        GridViewDataManager.RerequestedDateColumn,
        GridViewDataManager.HasDocumentsColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.OwnerColumn,
        GridViewDataManager.PrintExternallyColumn,
        GridViewDataManager.PrintInternallyColumn,
        GridViewDataManager.PriorToColumn,
        GridViewDataManager.ReceivedByColumn,
        GridViewDataManager.ReceivedDateColumn,
        GridViewDataManager.RejectedByColumn,
        GridViewDataManager.RejectedDateColumn,
        GridViewDataManager.ReviewedByColumn,
        GridViewDataManager.ReviewedDateColumn,
        GridViewDataManager.WaivedByColumn,
        GridViewDataManager.WaivedDateColumn,
        GridViewDataManager.ConditionCodeColumn
      }, true);
    }

    private void addSellCondition()
    {
      using (AddSellDialog addSellDialog = new AddSellDialog(this.loanDataMgr))
      {
        if (addSellDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK || addSellDialog.Conditions == null || addSellDialog.Conditions.Length == 0)
          return;
        if (addSellDialog.Conditions[0].ConditionType != ConditionType.Sell)
          this.switchTabsandRefreshContent(addSellDialog.IsImportConditions, addSellDialog.Conditions);
        else
          this.loadConditionList(addSellDialog.Conditions);
      }
    }

    private void duplicateSellCondition()
    {
      SellConditionLog selectedCondition = (SellConditionLog) this.getSelectedConditions()[0];
      SellConditionLog sellConditionLog = new SellConditionLog(Session.UserID, selectedCondition.PairId);
      sellConditionLog.Title = selectedCondition.Title;
      sellConditionLog.Description = selectedCondition.Description;
      sellConditionLog.Source = selectedCondition.Source;
      sellConditionLog.Category = selectedCondition.Category;
      sellConditionLog.PriorTo = selectedCondition.PriorTo;
      sellConditionLog.ForRoleID = selectedCondition.ForRoleID;
      sellConditionLog.AllowToClear = selectedCondition.AllowToClear;
      sellConditionLog.DaysTillDue = selectedCondition.DaysTillDue;
      sellConditionLog.IsInternal = selectedCondition.IsInternal;
      sellConditionLog.IsExternal = selectedCondition.IsExternal;
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) sellConditionLog);
      this.loadConditionList((ConditionLog) sellConditionLog);
    }

    private void editSellCondition()
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      if (selectedConditions.Length != 1)
        return;
      SellDetailsDialog.ShowInstance(this.loanDataMgr, (SellConditionLog) selectedConditions[0]);
    }

    public ToolStripDropDown Menu => (ToolStripDropDown) this.mnuConditions;

    private void refreshToolbar()
    {
      int count = this.gvConditions.SelectedItems.Count;
      this.btnDuplicate.Enabled = count == 1;
      this.btnEdit.Enabled = count == 1;
      this.btnDelete.Enabled = count > 0;
      this.btnPrint.Enabled = count > 0;
      this.btnRequest.Enabled = count == 1;
      this.mnuItemDuplicate1.Enabled = count == 1;
      this.mnuItemDuplicate2.Enabled = count == 1;
      this.mnuItemEdit1.Enabled = count == 1;
      this.mnuItemEdit2.Enabled = count == 1;
      this.mnuItemDelete1.Enabled = count > 0;
      this.mnuItemDelete2.Enabled = count > 0;
      this.mnuItemPrint1.Enabled = count > 0;
      this.mnuItemPrint2.Enabled = count > 0;
      this.mnuItemRequest.Enabled = count > 0;
      this.mnuItemInternal.Enabled = count > 0;
      this.mnuItemExternal.Enabled = count > 0;
    }

    private void btnDocuments_Click(object sender, EventArgs e)
    {
      AllDocumentsDialog.ShowInstance(this.loanDataMgr, (DocumentLog[]) null, this.condType);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      string str = string.Empty;
      foreach (ConditionLog conditionLog in selectedConditions)
        str = str + conditionLog.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the following condition(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      LogList logList = this.loanDataMgr.LoanData.GetLogList();
      foreach (ConditionLog rec in selectedConditions)
        logList.RemoveRecord((LogRecordBase) rec);
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.duplicateUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.duplicatePostClosingCondition();
          break;
        case ConditionType.Preliminary:
          this.duplicatePreliminaryCondition();
          break;
        case ConditionType.Sell:
          this.duplicateSellCondition();
          break;
      }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvConditions.SelectedItems.Count != 1)
        return;
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.editUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.editPostClosingCondition();
          break;
        case ConditionType.Preliminary:
          this.editPreliminaryCondition();
          break;
        case ConditionType.Sell:
          this.editSellCondition();
          break;
      }
    }

    private void btnExcel_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = this.gvConditions.SelectedItems.Count <= 0 ? new List<GVItem>((IEnumerable<GVItem>) this.gvConditions.VisibleItems) : new List<GVItem>((IEnumerable<GVItem>) this.gvConditions.SelectedItems);
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn gvColumn in this.gvConditions.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(gvColumn.Text);
        foreach (GVItem gvItem in gvItemList)
        {
          string[] data = new string[this.gvConditions.Columns.Count];
          for (int index1 = 0; index1 < data.Length; ++index1)
          {
            int index2 = this.gvConditions.Columns.DisplaySequence[index1].Index;
            data[index1] = gvItem.SubItems[index2].Text;
          }
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.addUnderwritingCondition();
          break;
        case ConditionType.PostClosing:
          this.addPostClosingCondition();
          break;
        case ConditionType.Preliminary:
          this.addPreliminaryCondition();
          break;
        case ConditionType.Sell:
          this.addSellCondition();
          break;
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      using (PrintConditionsDialog conditionsDialog = new PrintConditionsDialog(this.loanDataMgr, this.getSelectedConditions(), eFolderDialog.SelectedStackingOrder()))
      {
        int num = (int) conditionsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnRequest_Click(object sender, EventArgs e)
    {
      ConditionLog selectedCondition = this.getSelectedConditions()[0];
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog linkedDocument in selectedCondition.GetLinkedDocuments())
        documentLogList.Add(linkedDocument);
      ConditionTrackingSetup conditionTrackingSetup = (ConditionTrackingSetup) null;
      switch (selectedCondition.ConditionType)
      {
        case ConditionType.Underwriting:
          conditionTrackingSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.UnderwritingConditionTrackingSetup;
          break;
        case ConditionType.PostClosing:
          conditionTrackingSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.PostClosingConditionTrackingSetup;
          break;
        case ConditionType.Preliminary:
          conditionTrackingSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.UnderwritingConditionTrackingSetup;
          break;
        case ConditionType.Sell:
          conditionTrackingSetup = (ConditionTrackingSetup) this.loanDataMgr.SystemConfiguration.SellConditionTrackingSetup;
          break;
      }
      ConditionTemplate byName = conditionTrackingSetup.GetByName(selectedCondition.Title);
      if (byName != null)
      {
        foreach (DocumentTemplate document in byName.GetDocuments(this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup))
        {
          bool flag = false;
          foreach (DocumentLog documentLog in documentLogList)
          {
            if (document.Name == documentLog.Title)
              flag = true;
          }
          if (!flag)
          {
            DocumentLog logEntry = document.CreateLogEntry(Session.UserID, selectedCondition.PairId);
            documentLogList.Add(logEntry);
          }
        }
      }
      new eFolderManager().Request(this.loanDataMgr, documentLogList.ToArray(), selectedCondition);
    }

    private void btnRetrieve_Click(object sender, EventArgs e)
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (ConditionLog conditionLog in selectedConditions)
      {
        foreach (DocumentLog linkedDocument in conditionLog.GetLinkedDocuments())
        {
          if (!documentLogList.Contains(linkedDocument))
            documentLogList.Add(linkedDocument);
        }
      }
      new eFolderManager().Retrieve(this.loanDataMgr, documentLogList.ToArray(), this.session);
    }

    private void mnuItemInternal_Click(object sender, EventArgs e)
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          foreach (UnderwritingConditionLog underwritingConditionLog in selectedConditions)
          {
            if (!underwritingConditionLog.IsInternal)
              underwritingConditionLog.IsInternal = true;
          }
          break;
        case ConditionType.Sell:
          foreach (SellConditionLog sellConditionLog in selectedConditions)
          {
            if (!sellConditionLog.IsInternal)
              sellConditionLog.IsInternal = true;
          }
          break;
      }
    }

    private void mnuItemExternal_Click(object sender, EventArgs e)
    {
      ConditionLog[] selectedConditions = this.getSelectedConditions();
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          foreach (UnderwritingConditionLog underwritingConditionLog in selectedConditions)
          {
            if (!underwritingConditionLog.IsExternal)
              underwritingConditionLog.IsExternal = true;
          }
          break;
        case ConditionType.Sell:
          foreach (SellConditionLog sellConditionLog in selectedConditions)
          {
            if (!sellConditionLog.IsExternal)
              sellConditionLog.IsExternal = true;
          }
          break;
      }
    }

    private void mnuItemSelectAll_Click(object sender, EventArgs e)
    {
      this.gvConditions.Items.SelectAll();
    }

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr);
      switch (this.condType)
      {
        case ConditionType.Underwriting:
          this.btnNew.Visible = folderAccessRights.CanAddUnderwritingConditions;
          this.btnDuplicate.Visible = folderAccessRights.CanAddUnderwritingConditions;
          this.btnDelete.Visible = folderAccessRights.CanDeleteUnderwritingConditions;
          this.btnRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.btnRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemNew1.Visible = folderAccessRights.CanAddUnderwritingConditions;
          this.mnuItemNew2.Visible = folderAccessRights.CanAddUnderwritingConditions;
          this.mnuItemDuplicate1.Visible = folderAccessRights.CanAddUnderwritingConditions;
          this.mnuItemDuplicate2.Visible = folderAccessRights.CanAddUnderwritingConditions;
          this.mnuItemDelete1.Visible = folderAccessRights.CanDeleteUnderwritingConditions;
          this.mnuItemDelete2.Visible = folderAccessRights.CanDeleteUnderwritingConditions;
          this.mnuItemRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.mnuItemRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemInternal.Visible = Session.IsBankerEdition() && folderAccessRights.CanEditUnderwritingCondition;
          this.mnuItemExternal.Visible = folderAccessRights.CanEditUnderwritingCondition;
          break;
        case ConditionType.PostClosing:
          this.btnNew.Visible = folderAccessRights.CanAddPostClosingConditions;
          this.btnDuplicate.Visible = folderAccessRights.CanAddPostClosingConditions;
          this.btnDelete.Visible = folderAccessRights.CanDeletePostClosingConditions;
          this.btnRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.btnRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemNew1.Visible = folderAccessRights.CanAddPostClosingConditions;
          this.mnuItemNew2.Visible = folderAccessRights.CanAddPostClosingConditions;
          this.mnuItemDuplicate1.Visible = folderAccessRights.CanAddPostClosingConditions;
          this.mnuItemDuplicate2.Visible = folderAccessRights.CanAddPostClosingConditions;
          this.mnuItemDelete1.Visible = folderAccessRights.CanDeletePostClosingConditions;
          this.mnuItemDelete2.Visible = folderAccessRights.CanDeletePostClosingConditions;
          this.mnuItemRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.mnuItemRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemInternal.Visible = false;
          this.mnuItemExternal.Visible = false;
          break;
        case ConditionType.Preliminary:
          this.btnNew.Visible = folderAccessRights.CanAddPreliminaryConditions;
          this.btnDuplicate.Visible = folderAccessRights.CanAddPreliminaryConditions;
          this.btnDelete.Visible = folderAccessRights.CanDeletePreliminaryConditions;
          this.btnRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.btnRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemNew1.Visible = folderAccessRights.CanAddPreliminaryConditions;
          this.mnuItemNew2.Visible = folderAccessRights.CanAddPreliminaryConditions;
          this.mnuItemDuplicate1.Visible = folderAccessRights.CanAddPreliminaryConditions;
          this.mnuItemDuplicate2.Visible = folderAccessRights.CanAddPreliminaryConditions;
          this.mnuItemDelete1.Visible = folderAccessRights.CanDeletePreliminaryConditions;
          this.mnuItemDelete2.Visible = folderAccessRights.CanDeletePreliminaryConditions;
          this.mnuItemRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.mnuItemRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemInternal.Visible = false;
          this.mnuItemExternal.Visible = false;
          break;
        case ConditionType.Sell:
          this.btnNew.Visible = folderAccessRights.CanAddSellConditions;
          this.btnDuplicate.Visible = folderAccessRights.CanAddSellConditions;
          this.btnDelete.Visible = folderAccessRights.CanDeleteSellConditions;
          this.btnRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.btnRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemNew1.Visible = folderAccessRights.CanAddSellConditions;
          this.mnuItemNew2.Visible = folderAccessRights.CanAddSellConditions;
          this.mnuItemDuplicate1.Visible = folderAccessRights.CanAddSellConditions;
          this.mnuItemDuplicate2.Visible = folderAccessRights.CanAddSellConditions;
          this.mnuItemDelete1.Visible = folderAccessRights.CanDeleteSellConditions;
          this.mnuItemDelete2.Visible = folderAccessRights.CanDeleteSellConditions;
          this.mnuItemRequest.Visible = folderAccessRights.CanRequestDocuments || folderAccessRights.CanRequestServices;
          this.mnuItemRetrieve.Visible = folderAccessRights.CanRetrieveDocuments || folderAccessRights.CanRetrieveServices;
          this.mnuItemInternal.Visible = Session.IsBankerEdition();
          this.mnuItemExternal.Visible = folderAccessRights.CanEditSellCondition;
          break;
      }
      if (!Session.IsBrokerEdition())
        return;
      this.mnuItemExternal.Text = "Print in MLC";
    }

    public void RefreshContents()
    {
      ConditionLog importedCondition = ((eFolderDialog) this.frmEFolderDialog).ImportedCondition;
      if (importedCondition != null)
      {
        this.loadConditionList(importedCondition);
        ((eFolderDialog) this.frmEFolderDialog).ImportedCondition = (ConditionLog) null;
        this.btnEdit_Click((object) this.btnEdit, EventArgs.Empty);
      }
      else
        this.loadConditionList();
    }

    public void RefreshLoanContents() => this.loadConditionList();

    private void switchTabsandRefreshContent(bool isImportConditions, ConditionLog[] conditions)
    {
      if (isImportConditions)
      {
        if (conditions.Length == 1)
          ((eFolderDialog) this.frmEFolderDialog).ImportedCondition = conditions[0];
        ((eFolderDialog) this.frmEFolderDialog).SetSelected(conditions[0].ConditionType);
      }
      else
        this.loadConditionList(conditions);
    }

    private void cboConditionsView_DropDown(object sender, EventArgs e)
    {
      this.refreshConditionViewList(Session.ConfigurationManager.GetTemplateDirEntries(this.currentTemplateType, FileSystemEntry.PrivateRoot(Session.UserID)));
      this.cboConditionsView.DropDown -= new EventHandler(this.cboConditionsView_DropDown);
    }

    private void cboConditionsView_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.setCurrentView(((FileSystemEntryListItem) this.cboConditionsView.SelectedItem).Entry);
    }

    private void btnSaveConditionView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void btnManageConditionView_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(this.currentTemplateType, false, this.currentTemplateType.ToString() + ".DefaultView"))
      {
        managementDialog.AddStaticView((BinaryConvertibleObject) this.currentStandardView);
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
        this.refreshConditionViewList(managementDialog.FileSystemEntries);
      }
    }

    private void btnResetConditionView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentView(this.currentView);
    }

    private void initConditionViewList()
    {
      int num = -1;
      FileSystemEntryListItem systemEntryListItem1 = new FileSystemEntryListItem(GridViewDataManager.StandardViewFileSystemEntry);
      if (!this.cboConditionsView.Items.Contains((object) systemEntryListItem1))
        num = this.cboConditionsView.Items.Add((object) systemEntryListItem1);
      string privateProfileString = Session.GetPrivateProfileString(this.currentTemplateType.ToString(), "DefaultView");
      if (!string.IsNullOrEmpty(privateProfileString) && string.Compare(privateProfileString, GridViewDataManager.StandardViewName, true) != 0)
      {
        FileSystemEntry e = FileSystemEntry.Parse(privateProfileString);
        ConditionTrackingView conditionTrackingView = (ConditionTrackingView) null;
        if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView)
          conditionTrackingView = Session.StartupInfo.DefaultPreliminaryConditionTrackingView;
        else if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView)
          conditionTrackingView = Session.StartupInfo.DefaultPostClosingConditionTrackingView;
        else if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView)
          conditionTrackingView = Session.StartupInfo.DefaultUnderwritingConditionTrackingView;
        else if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView)
          conditionTrackingView = Session.StartupInfo.DefaultSellConditionTrackingView;
        if (conditionTrackingView != null)
        {
          this.currentView = conditionTrackingView;
          this.currentFileSystemEntry = e;
          FileSystemEntryListItem systemEntryListItem2 = new FileSystemEntryListItem(e);
          if (!this.cboConditionsView.Items.Contains((object) systemEntryListItem2))
            num = this.cboConditionsView.Items.Add((object) systemEntryListItem2);
        }
      }
      this.cboConditionsView.SelectedIndex = num;
      this.setCurrentView(((FileSystemEntryListItem) this.cboConditionsView.SelectedItem).Entry);
    }

    private void setCurrentView(FileSystemEntry fsEntry)
    {
      try
      {
        if (fsEntry.Equals((object) GridViewDataManager.StandardViewFileSystemEntry))
        {
          this.currentView = this.currentStandardView;
          this.currentFileSystemEntry = fsEntry;
          this.setCurrentView(this.currentView);
        }
        else
        {
          if (!fsEntry.Equals((object) this.currentFileSystemEntry))
          {
            string privateProfileString = Session.GetPrivateProfileString(this.currentTemplateType.ToString(), "DefaultView");
            if (!string.IsNullOrEmpty(privateProfileString) && string.Compare(privateProfileString, GridViewDataManager.StandardViewName, true) != 0)
            {
              FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(privateProfileString);
              if (fsEntry.Equals((object) fileSystemEntry))
              {
                if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PreliminaryConditionTrackingView)
                  this.currentView = Session.StartupInfo.DefaultPreliminaryConditionTrackingView;
                else if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.PostClosingConditionTrackingView)
                  this.currentView = Session.StartupInfo.DefaultPostClosingConditionTrackingView;
                else if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.UnderwritingConditionTrackingView)
                  this.currentView = Session.StartupInfo.DefaultUnderwritingConditionTrackingView;
                else if (this.currentTemplateType == EllieMae.EMLite.ClientServer.TemplateSettingsType.SellConditionTrackingView)
                  this.currentView = Session.StartupInfo.DefaultSellConditionTrackingView;
              }
              else
                this.currentView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(this.currentTemplateType, fsEntry);
            }
            else
              this.currentView = (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(this.currentTemplateType, fsEntry);
            this.currentFileSystemEntry = fsEntry;
          }
          if (this.currentView == null)
            throw new ArgumentException();
          this.setCurrentView(this.currentView);
        }
      }
      catch (Exception ex)
      {
        ErrorDialog.Display(ex);
      }
    }

    private void setCurrentView(ConditionTrackingView view)
    {
      this.gvConditionsMgr.ApplyView(view);
      this.loadConditionList();
    }

    private void saveCurrentView()
    {
      ConditionTrackingView view = new ConditionTrackingView(this.currentView.Name);
      view.Layout = this.gvConditionsMgr.GetCurrentLayout();
      view.Filter = this.gvConditionsMgr.GetCurrentFilter();
      using (SaveViewDialog saveViewDialog = new SaveViewDialog(this.currentTemplateType, (BinaryConvertibleObject) view, this.getViewNameList(), this.currentView.Name != GridViewDataManager.StandardViewName))
      {
        if (saveViewDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.updateConditionViewList(saveViewDialog.SelectedEntry);
      }
      this.currentView = view;
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (object obj in this.cboConditionsView.Items)
        stringList.Add(obj.ToString());
      return stringList.ToArray();
    }

    private void updateConditionViewList(FileSystemEntry fsEntry)
    {
      FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(fsEntry);
      if (this.cboConditionsView.Items.Contains((object) systemEntryListItem))
        return;
      this.cboConditionsView.SelectedIndex = this.cboConditionsView.Items.Add((object) systemEntryListItem);
    }

    private void refreshConditionViewList(FileSystemEntry[] viewEntries)
    {
      int num = -1;
      this.cboConditionsView.Items.Clear();
      FileSystemEntryListItem systemEntryListItem1 = new FileSystemEntryListItem(GridViewDataManager.StandardViewFileSystemEntry);
      if (!this.cboConditionsView.Items.Contains((object) systemEntryListItem1))
        num = this.cboConditionsView.Items.Add((object) systemEntryListItem1);
      foreach (FileSystemEntry viewEntry in viewEntries)
        this.cboConditionsView.Items.Add((object) new FileSystemEntryListItem(viewEntry));
      if (this.currentFileSystemEntry != null)
      {
        FileSystemEntryListItem systemEntryListItem2 = new FileSystemEntryListItem(this.currentFileSystemEntry);
        if (this.cboConditionsView.Items.Contains((object) systemEntryListItem2))
          this.cboConditionsView.SelectedItem = (object) systemEntryListItem2;
      }
      if (this.cboConditionsView.SelectedIndex >= 0)
        return;
      this.cboConditionsView.SelectedIndex = num;
    }

    private void btnCondResponses_Click(object sender, EventArgs e)
    {
      using (Form condResponseForm = DeliverConditionResponseFactory.GetDeliverCondResponseForm(this.loanDataMgr))
        condResponseForm?.ShowDialog((IWin32Window) Form.ActiveForm);
    }

    private void btnDeliveryStatus_Click(object sender, EventArgs e)
    {
      using (Form deliveryStatusForm = ConditionDeliveryStatusFactory.GetConditionDeliveryStatusForm(this.loanDataMgr))
        deliveryStatusForm?.ShowDialog((IWin32Window) Form.ActiveForm);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
      {
        this.components.Dispose();
        this.components = (IContainer) null;
        this.tooltip = (ToolTip) null;
        this.loanDataMgr = (LoanDataMgr) null;
      }
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.btnDocuments = new Button();
      this.btnRetrieve = new Button();
      this.btnRequest = new Button();
      this.separator = new VerticalSeparator();
      this.btnPrint = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.gvConditions = new GridView();
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuItemNew1 = new ToolStripMenuItem();
      this.mnuItemEdit1 = new ToolStripMenuItem();
      this.mnuItemDuplicate1 = new ToolStripMenuItem();
      this.mnuItemDelete1 = new ToolStripMenuItem();
      this.mnuItemSeparator1 = new ToolStripSeparator();
      this.mnuItemExcel1 = new ToolStripMenuItem();
      this.mnuItemPrint1 = new ToolStripMenuItem();
      this.mnuItemInternal = new ToolStripMenuItem();
      this.mnuItemExternal = new ToolStripMenuItem();
      this.mnuItemSeparator2 = new ToolStripSeparator();
      this.mnuItemSelectAll = new ToolStripMenuItem();
      this.tooltip = new ToolTip(this.components);
      this.btnEdit = new StandardIconButton();
      this.btnExcel = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnResetConditionView = new StandardIconButton();
      this.btnManageConditionView = new StandardIconButton();
      this.btnSaveConditionView = new StandardIconButton();
      this.btnCondResponses = new Button();
      this.btnDeliveryStatus = new Button();
      this.gcConditions = new GroupContainer();
      this.pnlToolbar = new FlowLayoutPanel();
      this.mnuConditions = new ContextMenuStrip(this.components);
      this.mnuItemNew2 = new ToolStripMenuItem();
      this.mnuItemDuplicate2 = new ToolStripMenuItem();
      this.mnuItemEdit2 = new ToolStripMenuItem();
      this.mnuItemDelete2 = new ToolStripMenuItem();
      this.mnuItemSeparator3 = new ToolStripSeparator();
      this.mnuItemExcel2 = new ToolStripMenuItem();
      this.mnuItemPrint2 = new ToolStripMenuItem();
      this.mnuItemSeparator4 = new ToolStripSeparator();
      this.mnuItemRequest = new ToolStripMenuItem();
      this.mnuItemRetrieve = new ToolStripMenuItem();
      this.mnuItemDocuments = new ToolStripMenuItem();
      this.gradConditionsView = new GradientPanel();
      this.cboConditionsView = new ComboBoxEx();
      this.lblConditionsView = new Label();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.mnuContext.SuspendLayout();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnExcel).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnResetConditionView).BeginInit();
      ((ISupportInitialize) this.btnManageConditionView).BeginInit();
      ((ISupportInitialize) this.btnSaveConditionView).BeginInit();
      this.gcConditions.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.mnuConditions.SuspendLayout();
      this.gradConditionsView.SuspendLayout();
      this.SuspendLayout();
      this.btnDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDocuments.BackColor = SystemColors.Control;
      this.btnDocuments.Location = new Point(319, 0);
      this.btnDocuments.Margin = new Padding(0);
      this.btnDocuments.Name = "btnDocuments";
      this.btnDocuments.Size = new Size(114, 22);
      this.btnDocuments.TabIndex = 5;
      this.btnDocuments.TabStop = false;
      this.btnDocuments.Text = "Document Manager";
      this.tooltip.SetToolTip((Control) this.btnDocuments, "Assign documents to conditions");
      this.btnDocuments.UseVisualStyleBackColor = true;
      this.btnDocuments.Click += new EventHandler(this.btnDocuments_Click);
      this.btnRetrieve.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRetrieve.BackColor = SystemColors.Control;
      this.btnRetrieve.Location = new Point(228, 0);
      this.btnRetrieve.Margin = new Padding(0);
      this.btnRetrieve.Name = "btnRetrieve";
      this.btnRetrieve.Size = new Size(91, 22);
      this.btnRetrieve.TabIndex = 4;
      this.btnRetrieve.TabStop = false;
      this.btnRetrieve.Text = "Retrieve Docs";
      this.tooltip.SetToolTip((Control) this.btnRetrieve, "Retrieve faxes and documents from borrower and service provider");
      this.btnRetrieve.UseVisualStyleBackColor = true;
      this.btnRetrieve.Click += new EventHandler(this.btnRetrieve_Click);
      this.btnRequest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRequest.BackColor = SystemColors.Control;
      this.btnRequest.Location = new Point(136, 0);
      this.btnRequest.Margin = new Padding(0);
      this.btnRequest.Name = "btnRequest";
      this.btnRequest.Size = new Size(92, 22);
      this.btnRequest.TabIndex = 3;
      this.btnRequest.TabStop = false;
      this.btnRequest.Text = "Request Docs";
      this.tooltip.SetToolTip((Control) this.btnRequest, "Send documents to borrower to sign and request needed documents");
      this.btnRequest.UseVisualStyleBackColor = true;
      this.btnRequest.Click += new EventHandler(this.btnRequest_Click);
      this.separator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator.Location = new Point(131, 3);
      this.separator.Margin = new Padding(4, 3, 3, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 2;
      this.separator.TabStop = false;
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(111, 3);
      this.btnPrint.Margin = new Padding(4, 3, 0, 3);
      this.btnPrint.MouseDownImage = (Image) null;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 6;
      this.btnPrint.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnPrint, "Print Condition");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(31, 3);
      this.btnDuplicate.Margin = new Padding(4, 3, 0, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 2;
      this.btnDuplicate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDuplicate, "Duplicate Condition");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(11, 3);
      this.btnNew.Margin = new Padding(4, 3, 0, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 1;
      this.btnNew.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnNew, "New Condition");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.gvConditions.AllowColumnReorder = true;
      this.gvConditions.BorderStyle = BorderStyle.None;
      this.gvConditions.ContextMenuStrip = this.mnuContext;
      this.gvConditions.Dock = DockStyle.Fill;
      this.gvConditions.FilterVisible = true;
      this.gvConditions.Location = new Point(1, 26);
      this.gvConditions.Name = "gvConditions";
      this.gvConditions.Size = new Size(714, 266);
      this.gvConditions.TabIndex = 6;
      this.gvConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditions.SelectedIndexChanged += new EventHandler(this.gvConditions_SelectedIndexChanged);
      this.gvConditions.ItemDoubleClick += new GVItemEventHandler(this.gvConditions_ItemDoubleClick);
      this.mnuContext.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.mnuItemNew1,
        (ToolStripItem) this.mnuItemEdit1,
        (ToolStripItem) this.mnuItemDuplicate1,
        (ToolStripItem) this.mnuItemDelete1,
        (ToolStripItem) this.mnuItemSeparator1,
        (ToolStripItem) this.mnuItemExcel1,
        (ToolStripItem) this.mnuItemPrint1,
        (ToolStripItem) this.mnuItemInternal,
        (ToolStripItem) this.mnuItemExternal,
        (ToolStripItem) this.mnuItemSeparator2,
        (ToolStripItem) this.mnuItemSelectAll
      });
      this.mnuContext.Name = "mnuDocuments";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(168, 214);
      this.mnuItemNew1.Name = "mnuItemNew1";
      this.mnuItemNew1.Size = new Size(167, 22);
      this.mnuItemNew1.Text = "New Condition...";
      this.mnuItemNew1.Click += new EventHandler(this.btnNew_Click);
      this.mnuItemEdit1.Name = "mnuItemEdit1";
      this.mnuItemEdit1.Size = new Size(167, 22);
      this.mnuItemEdit1.Text = "Edit Condition...";
      this.mnuItemEdit1.Click += new EventHandler(this.btnEdit_Click);
      this.mnuItemDuplicate1.Name = "mnuItemDuplicate1";
      this.mnuItemDuplicate1.Size = new Size(167, 22);
      this.mnuItemDuplicate1.Text = "Duplicate Condition...";
      this.mnuItemDuplicate1.Click += new EventHandler(this.btnDuplicate_Click);
      this.mnuItemDelete1.Name = "mnuItemDelete1";
      this.mnuItemDelete1.Size = new Size(167, 22);
      this.mnuItemDelete1.Text = "Delete Condition";
      this.mnuItemDelete1.Click += new EventHandler(this.btnDelete_Click);
      this.mnuItemSeparator1.Name = "mnuItemSeparator1";
      this.mnuItemSeparator1.Size = new Size(164, 6);
      this.mnuItemExcel1.Name = "mnuItemExcel1";
      this.mnuItemExcel1.Size = new Size(167, 22);
      this.mnuItemExcel1.Text = "Export to Excel...";
      this.mnuItemExcel1.Click += new EventHandler(this.btnExcel_Click);
      this.mnuItemPrint1.Name = "mnuItemPrint1";
      this.mnuItemPrint1.Size = new Size(167, 22);
      this.mnuItemPrint1.Text = "Print Condition...";
      this.mnuItemPrint1.Click += new EventHandler(this.btnPrint_Click);
      this.mnuItemInternal.Name = "mnuItemInternal";
      this.mnuItemInternal.Size = new Size(167, 22);
      this.mnuItemInternal.Text = "Print Internally";
      this.mnuItemInternal.Click += new EventHandler(this.mnuItemInternal_Click);
      this.mnuItemExternal.Name = "mnuItemExternal";
      this.mnuItemExternal.Size = new Size(167, 22);
      this.mnuItemExternal.Text = "Print Externally";
      this.mnuItemExternal.Click += new EventHandler(this.mnuItemExternal_Click);
      this.mnuItemSeparator2.Name = "mnuItemSeparator2";
      this.mnuItemSeparator2.Size = new Size(164, 6);
      this.mnuItemSelectAll.Name = "mnuItemSelectAll";
      this.mnuItemSelectAll.Size = new Size(167, 22);
      this.mnuItemSelectAll.Text = "Select All on This Page";
      this.mnuItemSelectAll.Click += new EventHandler(this.mnuItemSelectAll_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(51, 3);
      this.btnEdit.Margin = new Padding(4, 3, 0, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 3;
      this.btnEdit.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEdit, "Edit Condition");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExcel.BackColor = Color.Transparent;
      this.btnExcel.Location = new Point(91, 3);
      this.btnExcel.Margin = new Padding(4, 3, 0, 3);
      this.btnExcel.MouseDownImage = (Image) null;
      this.btnExcel.Name = "btnExcel";
      this.btnExcel.Size = new Size(16, 16);
      this.btnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExcel.TabIndex = 5;
      this.btnExcel.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnExcel, "Export to Excel");
      this.btnExcel.Click += new EventHandler(this.btnExcel_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(71, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 4;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete Condition");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnResetConditionView.BackColor = Color.Transparent;
      this.btnResetConditionView.Location = new Point(374, 8);
      this.btnResetConditionView.MouseDownImage = (Image) null;
      this.btnResetConditionView.Name = "btnResetConditionView";
      this.btnResetConditionView.Size = new Size(16, 16);
      this.btnResetConditionView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetConditionView.TabIndex = 7;
      this.btnResetConditionView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnResetConditionView, "Reset View");
      this.btnResetConditionView.Click += new EventHandler(this.btnResetConditionView_Click);
      this.btnManageConditionView.BackColor = Color.Transparent;
      this.btnManageConditionView.Location = new Point(396, 8);
      this.btnManageConditionView.MouseDownImage = (Image) null;
      this.btnManageConditionView.Name = "btnManageConditionView";
      this.btnManageConditionView.Size = new Size(16, 16);
      this.btnManageConditionView.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnManageConditionView.TabIndex = 6;
      this.btnManageConditionView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnManageConditionView, "Manage View");
      this.btnManageConditionView.Click += new EventHandler(this.btnManageConditionView_Click);
      this.btnSaveConditionView.BackColor = Color.Transparent;
      this.btnSaveConditionView.Location = new Point(352, 8);
      this.btnSaveConditionView.MouseDownImage = (Image) null;
      this.btnSaveConditionView.Name = "btnSaveConditionView";
      this.btnSaveConditionView.Size = new Size(16, 16);
      this.btnSaveConditionView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveConditionView.TabIndex = 5;
      this.btnSaveConditionView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSaveConditionView, "Save View");
      this.btnSaveConditionView.Click += new EventHandler(this.btnSaveConditionView_Click);
      this.btnCondResponses.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCondResponses.BackColor = SystemColors.Control;
      this.btnCondResponses.Location = new Point(433, 0);
      this.btnCondResponses.Margin = new Padding(0);
      this.btnCondResponses.Name = "btnCondResponses";
      this.btnCondResponses.Size = new Size(156, 22);
      this.btnCondResponses.TabIndex = 7;
      this.btnCondResponses.TabStop = false;
      this.btnCondResponses.Text = "Deliver Condition Responses";
      this.btnCondResponses.UseVisualStyleBackColor = true;
      this.btnCondResponses.Click += new EventHandler(this.btnCondResponses_Click);
      this.btnDeliveryStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeliveryStatus.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.btnDeliveryStatus.BackColor = SystemColors.Control;
      this.btnDeliveryStatus.Location = new Point(589, 0);
      this.btnDeliveryStatus.Margin = new Padding(0);
      this.btnDeliveryStatus.Name = "btnDeliveryStatus";
      this.btnDeliveryStatus.Size = new Size(142, 22);
      this.btnDeliveryStatus.TabIndex = 8;
      this.btnDeliveryStatus.TabStop = false;
      this.btnDeliveryStatus.Text = " Condition Delivery Status";
      this.btnDeliveryStatus.UseVisualStyleBackColor = true;
      this.btnDeliveryStatus.Click += new EventHandler(this.btnDeliveryStatus_Click);
      this.gcConditions.Controls.Add((Control) this.pnlToolbar);
      this.gcConditions.Controls.Add((Control) this.gvConditions);
      this.gcConditions.Dock = DockStyle.Fill;
      this.gcConditions.HeaderForeColor = SystemColors.ControlText;
      this.gcConditions.Location = new Point(0, 31);
      this.gcConditions.Name = "gcConditions";
      this.gcConditions.Size = new Size(716, 293);
      this.gcConditions.TabIndex = 0;
      this.gcConditions.Text = "Conditions";
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnDeliveryStatus);
      this.pnlToolbar.Controls.Add((Control) this.btnCondResponses);
      this.pnlToolbar.Controls.Add((Control) this.btnDocuments);
      this.pnlToolbar.Controls.Add((Control) this.btnRetrieve);
      this.pnlToolbar.Controls.Add((Control) this.btnRequest);
      this.pnlToolbar.Controls.Add((Control) this.separator);
      this.pnlToolbar.Controls.Add((Control) this.btnPrint);
      this.pnlToolbar.Controls.Add((Control) this.btnExcel);
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnEdit);
      this.pnlToolbar.Controls.Add((Control) this.btnDuplicate);
      this.pnlToolbar.Controls.Add((Control) this.btnNew);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(-15, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(731, 22);
      this.pnlToolbar.TabIndex = 1;
      this.mnuConditions.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.mnuItemNew2,
        (ToolStripItem) this.mnuItemDuplicate2,
        (ToolStripItem) this.mnuItemEdit2,
        (ToolStripItem) this.mnuItemDelete2,
        (ToolStripItem) this.mnuItemSeparator3,
        (ToolStripItem) this.mnuItemExcel2,
        (ToolStripItem) this.mnuItemPrint2,
        (ToolStripItem) this.mnuItemSeparator4,
        (ToolStripItem) this.mnuItemRequest,
        (ToolStripItem) this.mnuItemRetrieve,
        (ToolStripItem) this.mnuItemDocuments
      });
      this.mnuConditions.Name = "mnuDocuments";
      this.mnuConditions.Size = new Size(206, 214);
      this.mnuItemNew2.Image = (Image) Resources._new;
      this.mnuItemNew2.Name = "mnuItemNew2";
      this.mnuItemNew2.ShortcutKeys = Keys.N | Keys.Control;
      this.mnuItemNew2.Size = new Size(205, 22);
      this.mnuItemNew2.Text = "&New Condition...";
      this.mnuItemNew2.Click += new EventHandler(this.btnNew_Click);
      this.mnuItemDuplicate2.Image = (Image) Resources.duplicate;
      this.mnuItemDuplicate2.Name = "mnuItemDuplicate2";
      this.mnuItemDuplicate2.Size = new Size(205, 22);
      this.mnuItemDuplicate2.Text = "D&uplicate Condition...";
      this.mnuItemDuplicate2.Click += new EventHandler(this.btnDuplicate_Click);
      this.mnuItemEdit2.Image = (Image) Resources.edit;
      this.mnuItemEdit2.Name = "mnuItemEdit2";
      this.mnuItemEdit2.Size = new Size(205, 22);
      this.mnuItemEdit2.Text = "&Edit Condition...";
      this.mnuItemEdit2.Click += new EventHandler(this.btnEdit_Click);
      this.mnuItemDelete2.Image = (Image) Resources.delete;
      this.mnuItemDelete2.Name = "mnuItemDelete2";
      this.mnuItemDelete2.ShortcutKeyDisplayString = "";
      this.mnuItemDelete2.ShortcutKeys = Keys.D | Keys.Alt;
      this.mnuItemDelete2.Size = new Size(205, 22);
      this.mnuItemDelete2.Text = "&Delete Condition";
      this.mnuItemDelete2.Click += new EventHandler(this.btnDelete_Click);
      this.mnuItemSeparator3.Name = "mnuItemSeparator3";
      this.mnuItemSeparator3.Size = new Size(202, 6);
      this.mnuItemExcel2.Image = (Image) Resources.excel;
      this.mnuItemExcel2.Name = "mnuItemExcel2";
      this.mnuItemExcel2.Size = new Size(205, 22);
      this.mnuItemExcel2.Text = "E&xport to Excel...";
      this.mnuItemExcel2.Click += new EventHandler(this.btnExcel_Click);
      this.mnuItemPrint2.Image = (Image) Resources.print;
      this.mnuItemPrint2.Name = "mnuItemPrint2";
      this.mnuItemPrint2.ShortcutKeys = Keys.P | Keys.Control;
      this.mnuItemPrint2.Size = new Size(205, 22);
      this.mnuItemPrint2.Text = "&Print Condition...";
      this.mnuItemPrint2.Click += new EventHandler(this.btnPrint_Click);
      this.mnuItemSeparator4.Name = "mnuItemSeparator4";
      this.mnuItemSeparator4.Size = new Size(202, 6);
      this.mnuItemRequest.Name = "mnuItemRequest";
      this.mnuItemRequest.Size = new Size(205, 22);
      this.mnuItemRequest.Text = "&Request Documents...";
      this.mnuItemRequest.Click += new EventHandler(this.btnRequest_Click);
      this.mnuItemRetrieve.Name = "mnuItemRetrieve";
      this.mnuItemRetrieve.Size = new Size(205, 22);
      this.mnuItemRetrieve.Text = "Retrie&ve Documents...";
      this.mnuItemRetrieve.Click += new EventHandler(this.btnRetrieve_Click);
      this.mnuItemDocuments.Name = "mnuItemDocuments";
      this.mnuItemDocuments.Size = new Size(205, 22);
      this.mnuItemDocuments.Text = "Document &Manager...";
      this.mnuItemDocuments.Click += new EventHandler(this.btnDocuments_Click);
      this.gradConditionsView.BackColorGlassyStyle = true;
      this.gradConditionsView.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradConditionsView.Controls.Add((Control) this.btnResetConditionView);
      this.gradConditionsView.Controls.Add((Control) this.btnManageConditionView);
      this.gradConditionsView.Controls.Add((Control) this.btnSaveConditionView);
      this.gradConditionsView.Controls.Add((Control) this.cboConditionsView);
      this.gradConditionsView.Controls.Add((Control) this.lblConditionsView);
      this.gradConditionsView.Dock = DockStyle.Top;
      this.gradConditionsView.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradConditionsView.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradConditionsView.Location = new Point(0, 0);
      this.gradConditionsView.Name = "gradConditionsView";
      this.gradConditionsView.Size = new Size(716, 31);
      this.gradConditionsView.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradConditionsView.TabIndex = 15;
      this.cboConditionsView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionsView.FormattingEnabled = true;
      this.cboConditionsView.Location = new Point(123, 5);
      this.cboConditionsView.Name = "cboConditionsView";
      this.cboConditionsView.SelectedBGColor = SystemColors.Highlight;
      this.cboConditionsView.Size = new Size(219, 21);
      this.cboConditionsView.TabIndex = 2;
      this.cboConditionsView.SelectionChangeCommitted += new EventHandler(this.cboConditionsView_SelectionChangeCommitted);
      this.cboConditionsView.DropDown += new EventHandler(this.cboConditionsView_DropDown);
      this.lblConditionsView.AutoSize = true;
      this.lblConditionsView.BackColor = Color.Transparent;
      this.lblConditionsView.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblConditionsView.Location = new Point(6, 7);
      this.lblConditionsView.Name = "lblConditionsView";
      this.lblConditionsView.Size = new Size(111, 16);
      this.lblConditionsView.TabIndex = 1;
      this.lblConditionsView.Text = "Conditions View";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcConditions);
      this.Controls.Add((Control) this.gradConditionsView);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (ConditionTrackingControl);
      this.Size = new Size(716, 324);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.mnuContext.ResumeLayout(false);
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnExcel).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnResetConditionView).EndInit();
      ((ISupportInitialize) this.btnManageConditionView).EndInit();
      ((ISupportInitialize) this.btnSaveConditionView).EndInit();
      this.gcConditions.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.mnuConditions.ResumeLayout(false);
      this.gradConditionsView.ResumeLayout(false);
      this.gradConditionsView.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
