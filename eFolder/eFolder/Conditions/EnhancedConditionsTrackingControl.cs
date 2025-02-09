// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Conditions.EnhancedConditionsTrackingControl
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
using EllieMae.EMLite.eFolder.ThinThick;
using EllieMae.EMLite.LoanUtils.EnhancedConditions;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.UI.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Conditions
{
  public class EnhancedConditionsTrackingControl : UserControl, IRefreshContents
  {
    private LoanDataMgr loanDataMgr;
    private eFolderManager eFolderMgr;
    private GridViewDataManager gvConditionsMgr;
    private ConditionTrackingView currentView;
    private ConditionTrackingView currentStandardView;
    private EllieMae.EMLite.ClientServer.TemplateSettingsType currentTemplateType;
    private FileSystemEntry currentFileSystemEntry;
    private Sessions.Session session;
    private Form frmEFolderDialog;
    private LogList logList;
    private EnhancedConditionLog[] condList;
    private EnhancedConditionType[] enhancedConditionTypes;
    private EnhancedConditionTemplate[] conditionTemplates;
    private eFolderAccessRights rights;
    private List<ComboBoxItem> savedCheckedCondTypeItems = new List<ComboBoxItem>();
    private List<ComboBoxItem> allCondTypeItems = new List<ComboBoxItem>();
    private string allConditionsName = "<All Conditions>";
    private readonly StringEnum sourceOfCondStringEnum = new StringEnum(typeof (SourceOfCondition));
    private bool displayingInvestorDeliveryDialog;
    private RoleInfo[] rolesInfo;
    private bool formLoading = true;
    private IContainer components;
    private Button btnConditionStatus;
    private Button btnDeliverResponses;
    private Button btnDocuments;
    private Button btnRetrieve;
    private Button btnRequest;
    private VerticalSeparator separator;
    private StandardIconButton btnPrint;
    private StandardIconButton btnDuplicate;
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
    private GradientPanel gradConditionsView;
    private StandardIconButton btnResetConditionView;
    private StandardIconButton btnManageConditionView;
    private StandardIconButton btnSaveConditionView;
    private ComboBoxEx cboConditionsView;
    private Label lblConditionsView;
    private GridView gvConditions;
    private GradientPanel gradConditionType;
    private CheckedComboBox cboCondType;
    private ToolStripMenuItem mnuAssignDocument2;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem mnuAssignDocument;
    private StandardIconButton btnNew;
    private Label lblMessage;
    private Label lblConditionType;

    public void CloseDetailInstances() => EnhancedDetailsDialog.CloseInstances();

    public EnhancedConditionsTrackingControl(
      LoanDataMgr loanDataMgr,
      Sessions.Session session,
      Form eFolderDialog = null)
    {
      this.InitializeComponent();
      this.loanDataMgr = loanDataMgr;
      this.session = session;
      this.frmEFolderDialog = eFolderDialog;
      this.eFolderMgr = new eFolderManager();
      this.cboCondType.ItemList.ItemCheck += new ItemCheckEventHandler(this.cboCondType_ItemCheck);
      this.logList = this.loanDataMgr.LoanData.GetLogList();
      this.condList = this.logList.GetAllEnhancedConditions();
      this.enhancedConditionTypes = this.getConditionTypes();
      this.rolesInfo = ((IEnumerable<RoleInfo>) ((WorkflowManager) Session.BPM.GetBpmManager(BpmCategory.Workflow)).GetAllRoleFunctions()).OrderBy<RoleInfo, string>((Func<RoleInfo, string>) (r => r.RoleName)).ToArray<RoleInfo>();
      this.rights = new eFolderAccessRights(this.loanDataMgr);
      if (this.enhancedConditionTypes != null)
        this.initConditionTypeList();
      this.initConditionList();
      this.initConditionViewList();
      this.btnDeliverResponses.Visible = this.rights.CanDeliverConditionResponses();
      this.btnConditionStatus.Visible = this.rights.CanViewConditionDeliveryStatus();
    }

    private void initConditionList()
    {
      this.initConditionsList();
      this.currentStandardView = GridViewDataManager.StandardEnhancedConditionTrackingView;
      this.currentTemplateType = TemplateSettingsTypeConverter.GetConditionTrackingViewType(ConditionType.Enhanced);
    }

    private void gvConditionsMgr_LayoutChanged(object sender, EventArgs e)
    {
      this.loadConditionList();
    }

    private void loadConditionList() => this.loadConditionList((string) null);

    private void loadConditionList(string newConditionGUID)
    {
      this.logList = this.loanDataMgr.LoanData.GetLogList();
      this.condList = this.logList.GetAllEnhancedConditions();
      if (((IEnumerable<EnhancedConditionLog>) this.condList).Count<EnhancedConditionLog>() == 0)
        this.lblMessage.BringToFront();
      else
        this.lblMessage.SendToBack();
      DocumentLog[] allDocuments = this.logList.GetAllDocuments();
      List<ComboBoxItem> condTypeListItems = this.getCheckedCondTypeListItems();
      int[] usersAssignedRoles = this.loanDataMgr.AccessRules.GetUsersAssignedRoles();
      List<EnhancedConditionLog> enhancedConditionLogList = new List<EnhancedConditionLog>();
      foreach (EnhancedConditionLog cond in this.condList)
      {
        if (this.rights.CanAccessEnhancedCondition(cond.EnhancedConditionType) && this.isCondTypeSelected(cond, condTypeListItems))
        {
          Font font = (Font) null;
          if (cond.Comments.HasUnreviewedEntry(usersAssignedRoles))
            font = EncompassFonts.Normal2.Font;
          GVItem gvItem = this.gvConditions.Items.GetItemByTag((object) cond);
          if (gvItem == null)
            gvItem = this.gvConditionsMgr.AddItem((ConditionLog) cond, allDocuments);
          else
            this.gvConditionsMgr.RefreshItem(gvItem, cond, allDocuments);
          foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
            subItem.Font = font;
          enhancedConditionLogList.Add(cond);
        }
      }
      if (newConditionGUID != null)
        this.gvConditions.SelectedItems.Clear();
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvConditions.Items)
      {
        if (!enhancedConditionLogList.Contains(gvItem.Tag as EnhancedConditionLog))
          gvItemList.Add(gvItem);
        else if (newConditionGUID != null && ((LogRecordBase) gvItem.Tag).Guid == newConditionGUID)
          gvItem.Selected = true;
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvConditions.Items.Remove(gvItem);
      this.gvConditionsMgr.ApplyEmptyFilters = false;
      this.gvConditionsMgr.ApplyFilter();
      this.adjustFilters();
      if (this.formLoading)
      {
        this.setCurrentView(this.currentView, false);
        this.formLoading = false;
      }
      this.setAddOptions(condTypeListItems);
      this.refreshToolbar();
    }

    private bool isCondTypeSelected(EnhancedConditionLog cond, List<ComboBoxItem> checkedCondTypes)
    {
      bool flag = false;
      if (checkedCondTypes.Count == 0)
        return true;
      foreach (ComboBoxItem checkedCondType in checkedCondTypes)
      {
        if (checkedCondType.Name == this.allConditionsName)
        {
          flag = true;
          break;
        }
        if (cond.EnhancedConditionType == checkedCondType.Name)
        {
          flag = true;
          break;
        }
      }
      return flag;
    }

    private EnhancedConditionLog[] getSelectedConditions()
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
        arrayList.Add(selectedItem.Tag);
      return (EnhancedConditionLog[]) arrayList.ToArray(typeof (EnhancedConditionLog));
    }

    private void gvConditions_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvConditions_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshToolbar();
    }

    private void gvConditions_SubItemEnter(object source, GVSubItemEventArgs e)
    {
      if (e.SubItem.SortValue.ToString() == "Satisfied")
      {
        ToolTip tooltip = this.tooltip;
        Rectangle textRegion = e.SubItem.TextRegion;
        int x = textRegion.X + 30;
        textRegion = e.SubItem.TextRegion;
        int y = textRegion.Y + 60;
        Point point = new Point(x, y);
        tooltip.Show("Satisfied", (IWin32Window) this, point);
      }
      else
      {
        if (!(e.SubItem.SortValue.ToString() == "Open"))
          return;
        ToolTip tooltip = this.tooltip;
        Rectangle textRegion = e.SubItem.TextRegion;
        int x = textRegion.X + 30;
        textRegion = e.SubItem.TextRegion;
        int y = textRegion.Y + 60;
        Point point = new Point(x, y);
        tooltip.Show("Open", (IWin32Window) this, point);
      }
    }

    private void gvConditions_SubItemLeave(object source, GVSubItemEventArgs e)
    {
      this.tooltip.RemoveAll();
    }

    private void initConditionsList()
    {
      this.gvConditionsMgr = new GridViewDataManager(this.gvConditions, this.loanDataMgr);
      this.gvConditionsMgr.LayoutChanged += new EventHandler(this.gvConditionsMgr_LayoutChanged);
      this.gvConditionsMgr.CreateLayout(new TableLayout.Column[29]
      {
        GridViewDataManager.AddedDateColumn,
        GridViewDataManager.AddedByColumn,
        GridViewDataManager.CondStatusDateColumn,
        GridViewDataManager.AvailableExternallyColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.CategoryColumn,
        GridViewDataManager.CommentCountColumn,
        GridViewDataManager.CondNameColumn,
        GridViewDataManager.CondSourceEnhancedColumn,
        GridViewDataManager.CondLatestStatusColumn,
        GridViewDataManager.CondStatusDateTimeColumn,
        GridViewDataManager.CondStatusUserColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.DispositionColumn,
        GridViewDataManager.DocumentCountColumn,
        GridViewDataManager.DocumentReceiptDateColumn,
        GridViewDataManager.EffectiveEndDateColumn,
        GridViewDataManager.EffectiveStartDateColumn,
        GridViewDataManager.ExternalDescriptionColumn,
        GridViewDataManager.ExternalIdColumn,
        GridViewDataManager.InternalDescriptionColumn,
        GridViewDataManager.InternalIdColumn,
        GridViewDataManager.PrintInternalEnhancedColumn,
        GridViewDataManager.PriorToEnhancedColumn,
        GridViewDataManager.RecipientEnhancedColumn,
        GridViewDataManager.SourceOfConditionColumn,
        GridViewDataManager.PartnerColumn,
        GridViewDataManager.OwnerNameColumn,
        GridViewDataManager.RequestedFromColumn
      }, true);
    }

    private void duplicateEnhancedCondition()
    {
      EnhancedConditionLog selectedCondition = this.getSelectedConditions()[0];
      EnhancedConditionLog enhancedConditionLog = new EnhancedConditionLog(selectedCondition.EnhancedConditionType, selectedCondition.Title, this.session.UserID, selectedCondition.PairId, selectedCondition.Definitions, (StatusTrackingList) null);
      enhancedConditionLog.Category = selectedCondition.Category;
      enhancedConditionLog.DaysToReceive = selectedCondition.DaysToReceive;
      enhancedConditionLog.ExternalDescription = selectedCondition.ExternalDescription;
      enhancedConditionLog.ExternalId = selectedCondition.ExternalId;
      enhancedConditionLog.InternalDescription = selectedCondition.InternalDescription;
      enhancedConditionLog.InternalId = selectedCondition.InternalId;
      enhancedConditionLog.InternalPrint = selectedCondition.InternalPrint;
      enhancedConditionLog.PriorTo = selectedCondition.PriorTo;
      enhancedConditionLog.Recipient = selectedCondition.Recipient;
      enhancedConditionLog.RequestedFrom = selectedCondition.RequestedFrom;
      enhancedConditionLog.Source = selectedCondition.Source;
      enhancedConditionLog.DocumentReceiptDate = selectedCondition.DocumentReceiptDate;
      enhancedConditionLog.StartDate = selectedCondition.StartDate;
      enhancedConditionLog.EndDate = selectedCondition.EndDate;
      enhancedConditionLog.SourceOfCondition = selectedCondition.SourceOfCondition;
      enhancedConditionLog.Owner = selectedCondition.Owner;
      foreach (EnhancedConditionTemplate conditionTemplate in this.getConditionTemplates())
      {
        if (conditionTemplate.Title == selectedCondition.Title)
        {
          this.assignDocuments(conditionTemplate, this.logList, enhancedConditionLog);
          break;
        }
      }
      this.logList.AddRecord((LogRecordBase) enhancedConditionLog);
      enhancedConditionLog.ExternalPrint = selectedCondition.ExternalPrint;
      this.loadConditionList();
    }

    private void assignDocuments(
      EnhancedConditionTemplate template,
      LogList logList,
      EnhancedConditionLog cond)
    {
      if (template.AssignedTo == null)
        return;
      DocumentTrackingSetup documentTrackingSetup = Session.ConfigurationManager.GetDocumentTrackingSetup();
      DocumentLog[] allDocuments = logList.GetAllDocuments();
      foreach (EntityReferenceContract referenceContract in template.AssignedTo)
        this.assignDocumentToCondition(documentTrackingSetup.GetByID(referenceContract.entityId), allDocuments, cond);
    }

    private void assignDocumentToCondition(
      DocumentTemplate docTemplate,
      DocumentLog[] docList,
      EnhancedConditionLog cond)
    {
      if (docTemplate == null)
        return;
      foreach (DocumentLog doc in docList)
      {
        if (doc.Title == docTemplate.Name)
          doc.Conditions.Add((ConditionLog) cond);
      }
    }

    private void editEnhancedCondition() => this.editEnhancedCondition((EnhancedConditionLog) null);

    private void editEnhancedCondition(EnhancedConditionLog cond)
    {
      if (cond == null)
      {
        EnhancedConditionLog[] selectedConditions = this.getSelectedConditions();
        if (selectedConditions.Length != 1)
          return;
        cond = selectedConditions[0];
      }
      EnhancedConditionType conditionType = (EnhancedConditionType) null;
      if (this.enhancedConditionTypes != null)
      {
        foreach (EnhancedConditionType enhancedConditionType in this.enhancedConditionTypes)
        {
          if (enhancedConditionType.title == cond.EnhancedConditionType)
            conditionType = enhancedConditionType;
        }
      }
      EnhancedDetailsDialog.ShowInstance(this.loanDataMgr, cond, conditionType);
    }

    private void adjustFilters()
    {
      this.adjustFilter(GridViewDataManager.CategoryColumn.Title);
      this.adjustFilter(GridViewDataManager.CondSourceEnhancedColumn.Title);
      this.adjustFilter(GridViewDataManager.CondStatusDateColumn.Title);
      this.adjustFilter(GridViewDataManager.CondLatestStatusColumn.Title);
      this.adjustFilter(GridViewDataManager.DispositionColumn.Title);
      this.adjustFilter(GridViewDataManager.ExternalIdColumn.Title);
      this.adjustFilter(GridViewDataManager.InternalIdColumn.Title);
      this.adjustFilter(GridViewDataManager.PriorToEnhancedColumn.Title);
      this.adjustFilter(GridViewDataManager.RecipientEnhancedColumn.Title);
      this.adjustFilter(GridViewDataManager.SourceOfConditionColumn.Title);
      this.adjustFilter(GridViewDataManager.OwnerNameColumn.Title);
      this.adjustFilter(GridViewDataManager.PartnerColumn.Title);
    }

    private void adjustFilter(string columnHeader)
    {
      GVColumn gvColumn = this.gvConditions.Columns.Where<GVColumn>((Func<GVColumn, bool>) (x => x.Text == columnHeader)).DefaultIfEmpty<GVColumn>((GVColumn) null).ToList<GVColumn>()[0];
      if (gvColumn == null)
        return;
      string empty = string.Empty;
      if (((ComboBox) gvColumn.FilterControl).SelectedItem != null)
        empty = ((ComboBox) gvColumn.FilterControl).SelectedItem.ToString();
      FieldOption fieldOption1 = new FieldOption(string.Empty, string.Empty);
      if (!((ComboBox) gvColumn.FilterControl).Items.Contains((object) fieldOption1))
        ((ComboBox) gvColumn.FilterControl).Items.Add((object) fieldOption1);
      foreach (EnhancedConditionLog cond in this.condList)
      {
        if (this.rights.CanAccessEnhancedCondition(cond.EnhancedConditionType))
        {
          string text = string.Empty;
          if (columnHeader == GridViewDataManager.CategoryColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.Category))
              text = cond.Category;
          }
          else if (columnHeader == GridViewDataManager.CondSourceEnhancedColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.Source))
              text = cond.Source;
          }
          else if (columnHeader == GridViewDataManager.CondStatusDateColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.Status))
            {
              string status = cond.Status;
              DateTime? statusDate = cond.StatusDate;
              ref DateTime? local = ref statusDate;
              string str = local.HasValue ? local.GetValueOrDefault().ToString("MM/dd/yyyy") : (string) null;
              text = status + " on " + str;
            }
          }
          else if (columnHeader == GridViewDataManager.CondLatestStatusColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.Status))
              text = cond.Status;
          }
          else if (columnHeader == GridViewDataManager.DispositionColumn.Title)
            text = cond.StatusOpen ? "Open" : "Satisfied";
          else if (columnHeader == GridViewDataManager.ExternalIdColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.ExternalId))
              text = cond.ExternalId;
          }
          else if (columnHeader == GridViewDataManager.InternalIdColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.InternalId))
              text = cond.InternalId;
          }
          else if (columnHeader == GridViewDataManager.PriorToEnhancedColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.PriorTo))
              text = cond.PriorTo;
          }
          else if (columnHeader == GridViewDataManager.RecipientEnhancedColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.Recipient))
              text = cond.Recipient;
          }
          else if (columnHeader == GridViewDataManager.SourceOfConditionColumn.Title)
          {
            SourceOfCondition? sourceOfCondition = cond.SourceOfCondition;
            if (sourceOfCondition.HasValue)
            {
              StringEnum ofCondStringEnum = this.sourceOfCondStringEnum;
              sourceOfCondition = cond.SourceOfCondition;
              string valueName = sourceOfCondition.ToString();
              text = ofCondStringEnum.GetStringValue(valueName);
            }
          }
          else if (columnHeader == GridViewDataManager.PartnerColumn.Title)
          {
            if (!string.IsNullOrEmpty(cond.Partner))
              text = cond.Partner;
          }
          else if (columnHeader == GridViewDataManager.OwnerNameColumn.Title && cond.Owner.HasValue && this.rolesInfo.Length != 0)
          {
            foreach (RoleInfo roleInfo in this.rolesInfo)
            {
              int? owner = cond.Owner;
              int id = roleInfo.ID;
              if (owner.GetValueOrDefault() == id & owner.HasValue)
                text = roleInfo.RoleName;
            }
          }
          if (!string.IsNullOrEmpty(text))
          {
            FieldOption fieldOption2 = new FieldOption(text, text);
            if (!((ComboBox) gvColumn.FilterControl).Items.Contains((object) fieldOption2))
            {
              ((ComboBox) gvColumn.FilterControl).Items.Add((object) fieldOption2);
              if (text == empty)
                ((ComboBox) gvColumn.FilterControl).SelectedItem = (object) fieldOption2;
            }
          }
        }
      }
      ((ComboBox) gvColumn.FilterControl).Sorted = true;
    }

    public ToolStripDropDown Menu => (ToolStripDropDown) this.mnuConditions;

    private void setAddOptions(List<ComboBoxItem> checkedCondTypes)
    {
      this.mnuItemNew1.Enabled = false;
      this.mnuItemNew2.Enabled = false;
      this.btnNew.Enabled = false;
      if (!this.loanDataMgr.Writable)
        return;
      bool flag = false;
      if (checkedCondTypes.Count == 0)
        flag = true;
      foreach (ComboBoxItem checkedCondType in checkedCondTypes)
      {
        if (checkedCondType.Name == this.allConditionsName)
        {
          flag = true;
          break;
        }
        if (this.rights.CanAddEnhancedCondition(checkedCondType.Name))
        {
          this.mnuItemNew1.Enabled = true;
          this.mnuItemNew2.Enabled = true;
          this.btnNew.Enabled = true;
          return;
        }
      }
      if (!flag || this.enhancedConditionTypes == null)
        return;
      foreach (EnhancedConditionType enhancedConditionType in this.enhancedConditionTypes)
      {
        if (this.rights.CanAddEnhancedCondition(enhancedConditionType.title))
        {
          this.mnuItemNew1.Enabled = true;
          this.mnuItemNew2.Enabled = true;
          this.btnNew.Enabled = true;
          break;
        }
      }
    }

    private void refreshToolbar()
    {
      this.btnDuplicate.Enabled = false;
      this.mnuItemDuplicate1.Enabled = false;
      this.mnuItemDuplicate2.Enabled = false;
      this.btnEdit.Enabled = false;
      this.mnuItemEdit1.Enabled = false;
      this.mnuItemEdit2.Enabled = false;
      this.mnuAssignDocument2.Enabled = false;
      int count = this.gvConditions.SelectedItems.Count;
      this.btnPrint.Enabled = count > 0;
      this.btnRequest.Enabled = count == 1;
      this.mnuItemPrint1.Enabled = count > 0;
      this.mnuItemPrint2.Enabled = count > 0;
      this.mnuItemRequest.Enabled = count > 0;
      if (count > 0)
      {
        this.btnDelete.Enabled = true;
        this.mnuItemDelete1.Enabled = true;
        this.mnuItemDelete2.Enabled = true;
        foreach (GVItem selectedItem in this.gvConditions.SelectedItems)
        {
          if (!this.rights.CanDeleteEnhancedCondition(((EnhancedConditionLog) selectedItem.Tag).EnhancedConditionType))
          {
            this.btnDelete.Enabled = false;
            this.mnuItemDelete1.Enabled = false;
            this.mnuItemDelete2.Enabled = false;
            break;
          }
        }
      }
      else
      {
        this.btnDelete.Enabled = false;
        this.mnuItemDelete1.Enabled = false;
        this.mnuItemDelete2.Enabled = false;
      }
      if (count != 1)
        return;
      this.btnEdit.Enabled = true;
      this.mnuItemEdit1.Enabled = true;
      this.mnuItemEdit2.Enabled = true;
      EnhancedConditionLog tag = (EnhancedConditionLog) this.gvConditions.SelectedItems[0].Tag;
      if (this.rights.CanAddEnhancedCondition(tag.EnhancedConditionType))
      {
        this.btnDuplicate.Enabled = this.isDuplicateAllowed(tag);
        this.mnuItemDuplicate1.Enabled = this.btnDuplicate.Enabled;
        this.mnuItemDuplicate2.Enabled = this.btnDuplicate.Enabled;
      }
      if (!this.rights.CanAssignEnhancedConditionDocuments(tag.EnhancedConditionType))
        return;
      this.mnuAssignDocument2.Enabled = true;
    }

    private bool isDuplicateAllowed(EnhancedConditionLog cond)
    {
      bool flag = true;
      this.conditionTemplates = this.getConditionTemplates();
      if (this.conditionTemplates != null)
      {
        foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
        {
          if (conditionTemplate.Title == cond.Title)
          {
            if (conditionTemplate.AllowDuplicate.HasValue && !conditionTemplate.AllowDuplicate.Value)
            {
              flag = false;
              break;
            }
            break;
          }
        }
      }
      return flag;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      List<EnhancedConditionType> enhancedConditionTypeList = new List<EnhancedConditionType>();
      foreach (ComboBoxItem condTypeListItem in this.getCheckedCondTypeListItems())
      {
        if (condTypeListItem.Name != this.allConditionsName && ((EnhancedConditionType) condTypeListItem.Tag).active)
          enhancedConditionTypeList.Add((EnhancedConditionType) condTypeListItem.Tag);
      }
      if (enhancedConditionTypeList.Count == 0)
      {
        foreach (EnhancedConditionType enhancedConditionType in this.enhancedConditionTypes)
        {
          if (enhancedConditionType.active)
            enhancedConditionTypeList.Add(enhancedConditionType);
        }
      }
      this.conditionTemplates = this.getConditionTemplates();
      if (this.conditionTemplates == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "No Condition Templates found for Add.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        string[] array = ((IEnumerable<EnhancedConditionLog>) this.condList).Select<EnhancedConditionLog, string>((Func<EnhancedConditionLog, string>) (c => c.UniqueKey)).Distinct<string>().ToArray<string>();
        EnhancedConditionLog cond = (EnhancedConditionLog) null;
        using (AddEnhancedConditionsDialog conditionsDialog = new AddEnhancedConditionsDialog(this.loanDataMgr, this.conditionTemplates, enhancedConditionTypeList.ToArray(), array))
        {
          if (conditionsDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.OK)
          {
            if (conditionsDialog.Conditions != null)
            {
              if (conditionsDialog.Conditions.Length != 0)
              {
                cond = conditionsDialog.NewCondition;
                this.loadConditionList(cond != null ? cond.Guid : conditionsDialog.NewConditionGUID);
                this.gvConditions.SelectedItems.Clear();
                if (!conditionsDialog.IsAdHocCondition)
                {
                  foreach (object condition in conditionsDialog.Conditions)
                  {
                    GVItem itemByTag = this.gvConditions.Items.GetItemByTag(condition);
                    if (itemByTag != null)
                      itemByTag.Selected = true;
                  }
                }
              }
            }
          }
        }
        if (cond == null)
          return;
        this.editEnhancedCondition(cond);
      }
    }

    private void btnDocuments_Click(object sender, EventArgs e)
    {
      AllDocumentsDialog.ShowInstance(this.loanDataMgr, (DocumentLog[]) null, ConditionType.Enhanced);
    }

    private void btnDeliverResponses_Click(object sender, EventArgs e)
    {
      if (this.displayingInvestorDeliveryDialog)
        return;
      try
      {
        this.displayingInvestorDeliveryDialog = true;
        this.eFolderMgr.LaunchInvestorDeliveryConditions(this.loanDataMgr, ThinThickType.DeliverConditionResponses);
      }
      finally
      {
        this.displayingInvestorDeliveryDialog = false;
      }
    }

    private void btnConditionStatus_Click(object sender, EventArgs e)
    {
      if (this.displayingInvestorDeliveryDialog)
        return;
      try
      {
        this.displayingInvestorDeliveryDialog = true;
        this.eFolderMgr.LaunchInvestorDeliveryConditions(this.loanDataMgr, ThinThickType.ConditionDeliveryStatus);
      }
      finally
      {
        this.displayingInvestorDeliveryDialog = false;
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      EnhancedConditionLog[] selectedConditions = this.getSelectedConditions();
      string str = string.Empty;
      foreach (EnhancedConditionLog enhancedConditionLog in selectedConditions)
        str = str + enhancedConditionLog.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the following condition(s):\r\n\r\n" + str, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
        return;
      foreach (LogRecordBase rec in selectedConditions)
        this.logList.RemoveRecord(rec);
      this.loadConditionList();
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      this.duplicateEnhancedCondition();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (this.gvConditions.SelectedItems.Count != 1 || !this.btnEdit.Enabled)
        return;
      this.editEnhancedCondition();
    }

    private void mnuAssignDocument_Click(object sender, EventArgs e)
    {
      using (AssignDocumentsDialog assignDocumentsDialog = new AssignDocumentsDialog(this.loanDataMgr, (ConditionLog) this.getSelectedConditions()[0]))
      {
        if (assignDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        this.loadConditionList();
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

    private void btnPrint_Click(object sender, EventArgs e)
    {
      using (PrintConditionsDialog conditionsDialog = new PrintConditionsDialog(this.loanDataMgr, (ConditionLog[]) this.getSelectedConditions(), eFolderDialog.SelectedStackingOrder()))
      {
        int num = (int) conditionsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnRequest_Click(object sender, EventArgs e)
    {
      this.conditionTemplates = this.getConditionTemplates();
      if (this.conditionTemplates == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "No Condition Templates found for Request.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      EnhancedConditionLog tag = (EnhancedConditionLog) this.gvConditions.SelectedItems[0].Tag;
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog linkedDocument in tag.GetLinkedDocuments(false))
        documentLogList.Add(linkedDocument);
      foreach (EnhancedConditionTemplate conditionTemplate in this.conditionTemplates)
      {
        if (conditionTemplate.Title == tag.Title && conditionTemplate.AssignedTo != null)
        {
          DocumentTrackingSetup documentTrackingSetup = this.session.ConfigurationManager.GetDocumentTrackingSetup();
          using (List<EntityReferenceContract>.Enumerator enumerator = conditionTemplate.AssignedTo.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              EntityReferenceContract current = enumerator.Current;
              bool flag = false;
              foreach (LogRecordBase logRecordBase in documentLogList)
              {
                if (logRecordBase.Guid == current.entityId)
                  flag = true;
              }
              if (!flag)
              {
                DocumentLog logEntry = documentTrackingSetup.GetByID(current.entityId).CreateLogEntry(this.session.UserID, tag.PairId);
                documentLogList.Add(logEntry);
              }
            }
            break;
          }
        }
      }
      this.eFolderMgr.Request(this.loanDataMgr, documentLogList.ToArray(), (ConditionLog) tag);
    }

    private void btnRetrieve_Click(object sender, EventArgs e)
    {
      EnhancedConditionLog[] selectedConditions = this.getSelectedConditions();
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (EnhancedConditionLog enhancedConditionLog in selectedConditions)
      {
        foreach (DocumentLog linkedDocument in enhancedConditionLog.GetLinkedDocuments(false))
        {
          if (!documentLogList.Contains(linkedDocument))
            documentLogList.Add(linkedDocument);
        }
      }
      this.eFolderMgr.Retrieve(this.loanDataMgr, documentLogList.ToArray(), this.session);
    }

    private void mnuItemSelectAll_Click(object sender, EventArgs e)
    {
      this.gvConditions.Items.SelectAll();
    }

    private EnhancedConditionTemplate[] getConditionTemplates()
    {
      if (this.conditionTemplates != null)
        return this.conditionTemplates;
      this.conditionTemplates = new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTemplates(true);
      return this.conditionTemplates;
    }

    public void RefreshContents() => this.loadConditionList();

    public void RefreshLoanContents() => this.loadConditionList();

    private void cboConditionsView_DropDown(object sender, EventArgs e)
    {
      this.refreshConditionViewList(this.session.ConfigurationManager.GetTemplateDirEntries(this.currentTemplateType, FileSystemEntry.PrivateRoot(this.session.UserID)));
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
      string privateProfileString = this.session.GetPrivateProfileString(this.currentTemplateType.ToString(), "DefaultView");
      if (!string.IsNullOrEmpty(privateProfileString) && string.Compare(privateProfileString, GridViewDataManager.StandardViewName, true) != 0)
      {
        FileSystemEntry e = FileSystemEntry.Parse(privateProfileString);
        ConditionTrackingView conditionTrackingView = Session.StartupInfo.DefaultEnhancedConditionTrackingView;
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
            string privateProfileString = this.session.GetPrivateProfileString(this.currentTemplateType.ToString(), "DefaultView");
            if (!string.IsNullOrEmpty(privateProfileString) && string.Compare(privateProfileString, GridViewDataManager.StandardViewName, true) != 0)
            {
              FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(privateProfileString);
              this.currentView = !fsEntry.Equals((object) fileSystemEntry) ? (ConditionTrackingView) Session.ConfigurationManager.GetTemplateSettings(this.currentTemplateType, fsEntry) : Session.StartupInfo.DefaultEnhancedConditionTrackingView;
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

    private void setCurrentView(ConditionTrackingView view, bool loadConditions = true)
    {
      this.gvConditionsMgr.ApplyView(view);
      if (this.cboCondType.SelectedIndex < 0)
      {
        this.checkConditionType(this.cboCondType, this.allConditionsName, CheckState.Checked);
        this.cboCondType.SelectedItem = (object) this.allConditionsName;
        this.cboCondType.ComboBoxText = this.allConditionsName;
      }
      if (!loadConditions)
        return;
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

    private void cboCondType_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      this.cboCondType.ItemList.ItemCheck -= new ItemCheckEventHandler(this.cboCondType_ItemCheck);
      if (e.NewValue == CheckState.Checked)
      {
        if (e.Index == 0)
          this.checkConditionTypes(this.cboCondType, this.getCheckedCondTypeListItems(), CheckState.Unchecked);
        else
          this.checkConditionType(this.cboCondType, this.allConditionsName, CheckState.Unchecked);
      }
      this.cboCondType.ItemList.ItemCheck += new ItemCheckEventHandler(this.cboCondType_ItemCheck);
    }

    private void checkConditionTypes(
      CheckedComboBox box,
      List<ComboBoxItem> items,
      CheckState checkState)
    {
      foreach (ComboBoxItem comboBoxItem in items)
        this.checkConditionType(box, comboBoxItem.Name, checkState);
    }

    private void checkConditionType(
      CheckedComboBox box,
      string condTypeName,
      CheckState checkState)
    {
      ComboBoxItem comboBoxItem = (ComboBoxItem) null;
      foreach (object obj in (ListBox.ObjectCollection) box.Items)
      {
        if (string.Compare(((ComboBoxItem) obj).Name, condTypeName, true) == 0)
        {
          comboBoxItem = (ComboBoxItem) obj;
          break;
        }
      }
      if (comboBoxItem == null)
        return;
      box.SetItemChecked(comboBoxItem, checkState);
    }

    private List<ComboBoxItem> getCheckedCondTypeListItems()
    {
      List<ComboBoxItem> condTypeListItems = new List<ComboBoxItem>();
      if (string.IsNullOrEmpty(this.cboCondType.ComboBoxText) && this.cboCondType.ItemList.CheckedItems.Count == 0)
        return condTypeListItems;
      foreach (object checkedItem in this.cboCondType.ItemList.CheckedItems)
        condTypeListItems.Add((ComboBoxItem) checkedItem);
      return condTypeListItems;
    }

    private void initConditionTypeList()
    {
      int num1 = 0;
      foreach (EnhancedConditionType enhancedConditionType in this.enhancedConditionTypes)
      {
        int num2 = 0;
        if (this.rights.CanAccessEnhancedCondition(enhancedConditionType.title))
        {
          foreach (EnhancedConditionLog cond in this.condList)
          {
            if (cond.EnhancedConditionType == enhancedConditionType.title)
              ++num2;
          }
          if (enhancedConditionType.active || num2 > 0)
          {
            ComboBoxItem comboBoxItem = new ComboBoxItem(enhancedConditionType.title, enhancedConditionType.title + " (" + (object) num2 + ")", (object) enhancedConditionType);
            this.cboCondType.Items.Add((object) comboBoxItem);
            this.allCondTypeItems.Add(comboBoxItem);
          }
          num1 += num2;
        }
      }
      ComboBoxItem comboBoxItem1 = new ComboBoxItem(this.allConditionsName, this.allConditionsName + " (" + (object) num1 + ")");
      comboBoxItem1.Checked = true;
      this.cboCondType.Items.Insert(0, (object) comboBoxItem1);
      this.savedCheckedCondTypeItems.Add(comboBoxItem1);
      this.allCondTypeItems.Add(comboBoxItem1);
    }

    private void cboCondType_DropDownClosed(object sender, EventArgs e)
    {
      if (!(e is CCBoxEventArgs ccBoxEventArgs))
        ccBoxEventArgs = new CCBoxEventArgs(false, e);
      if (ccBoxEventArgs.AssignValues)
      {
        if (this.cboCondType.ItemList.CheckedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select at least one item from the list.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          List<ComboBoxItem> condTypeListItems = this.getCheckedCondTypeListItems();
          this.savedCheckedCondTypeItems = condTypeListItems;
          this.cboCondType.ComboBoxText = condTypeListItems.Count != 1 ? condTypeListItems.Count.ToString() + " types selected" : condTypeListItems[0].Name;
          this.loadConditionList();
        }
      }
      else
      {
        if (this.savedCheckedCondTypeItems.Count <= 0)
          return;
        this.checkConditionTypes(this.cboCondType, this.allCondTypeItems, CheckState.Unchecked);
        this.checkConditionTypes(this.cboCondType, this.savedCheckedCondTypeItems, CheckState.Checked);
      }
    }

    private EnhancedConditionType[] getConditionTypes()
    {
      return new EnhancedConditionsRestClient(this.loanDataMgr).GetEnhancedConditionTypes(false, false);
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
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuItemNew1 = new ToolStripMenuItem();
      this.mnuItemDuplicate1 = new ToolStripMenuItem();
      this.mnuItemEdit1 = new ToolStripMenuItem();
      this.mnuItemDelete1 = new ToolStripMenuItem();
      this.mnuItemSeparator1 = new ToolStripSeparator();
      this.mnuItemExcel1 = new ToolStripMenuItem();
      this.mnuItemPrint1 = new ToolStripMenuItem();
      this.mnuItemSeparator2 = new ToolStripSeparator();
      this.mnuAssignDocument2 = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.mnuItemSelectAll = new ToolStripMenuItem();
      this.tooltip = new ToolTip(this.components);
      this.btnConditionStatus = new Button();
      this.btnDeliverResponses = new Button();
      this.btnDocuments = new Button();
      this.btnRetrieve = new Button();
      this.btnRequest = new Button();
      this.btnPrint = new StandardIconButton();
      this.btnExcel = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnResetConditionView = new StandardIconButton();
      this.btnManageConditionView = new StandardIconButton();
      this.btnSaveConditionView = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.mnuConditions = new ContextMenuStrip(this.components);
      this.mnuItemNew2 = new ToolStripMenuItem();
      this.mnuItemDuplicate2 = new ToolStripMenuItem();
      this.mnuItemEdit2 = new ToolStripMenuItem();
      this.mnuItemDelete2 = new ToolStripMenuItem();
      this.mnuItemSeparator3 = new ToolStripSeparator();
      this.mnuItemExcel2 = new ToolStripMenuItem();
      this.mnuItemPrint2 = new ToolStripMenuItem();
      this.mnuItemSeparator4 = new ToolStripSeparator();
      this.mnuAssignDocument = new ToolStripMenuItem();
      this.mnuItemRequest = new ToolStripMenuItem();
      this.mnuItemRetrieve = new ToolStripMenuItem();
      this.mnuItemDocuments = new ToolStripMenuItem();
      this.gcConditions = new GroupContainer();
      this.cboCondType = new CheckedComboBox(this.components);
      this.gvConditions = new GridView();
      this.lblMessage = new Label();
      this.pnlToolbar = new FlowLayoutPanel();
      this.separator = new VerticalSeparator();
      this.gradConditionType = new GradientPanel();
      this.gradConditionsView = new GradientPanel();
      this.cboConditionsView = new ComboBoxEx();
      this.lblConditionsView = new Label();
      this.lblConditionType = new Label();
      this.mnuContext.SuspendLayout();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnExcel).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnResetConditionView).BeginInit();
      ((ISupportInitialize) this.btnManageConditionView).BeginInit();
      ((ISupportInitialize) this.btnSaveConditionView).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.mnuConditions.SuspendLayout();
      this.gcConditions.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.gradConditionType.SuspendLayout();
      this.gradConditionsView.SuspendLayout();
      this.SuspendLayout();
      this.mnuContext.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.mnuItemNew1,
        (ToolStripItem) this.mnuItemDuplicate1,
        (ToolStripItem) this.mnuItemEdit1,
        (ToolStripItem) this.mnuItemDelete1,
        (ToolStripItem) this.mnuItemSeparator1,
        (ToolStripItem) this.mnuItemExcel1,
        (ToolStripItem) this.mnuItemPrint1,
        (ToolStripItem) this.mnuItemSeparator2,
        (ToolStripItem) this.mnuAssignDocument2,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.mnuItemSelectAll
      });
      this.mnuContext.Name = "mnuDocuments";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(168, 198);
      this.mnuItemNew1.Name = "mnuItemNew1";
      this.mnuItemNew1.Size = new Size(167, 22);
      this.mnuItemNew1.Text = "Add Condition...";
      this.mnuItemNew1.Click += new EventHandler(this.btnNew_Click);
      this.mnuItemDuplicate1.Name = "mnuItemDuplicate1";
      this.mnuItemDuplicate1.Size = new Size(167, 22);
      this.mnuItemDuplicate1.Text = "Duplicate Condition...";
      this.mnuItemDuplicate1.Click += new EventHandler(this.btnDuplicate_Click);
      this.mnuItemEdit1.Name = "mnuItemEdit1";
      this.mnuItemEdit1.Size = new Size(167, 22);
      this.mnuItemEdit1.Text = "Edit Condition...";
      this.mnuItemEdit1.Click += new EventHandler(this.btnEdit_Click);
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
      this.mnuItemSeparator2.Name = "mnuItemSeparator2";
      this.mnuItemSeparator2.Size = new Size(164, 6);
      this.mnuAssignDocument2.Name = "mnuAssignDocument2";
      this.mnuAssignDocument2.Size = new Size(167, 22);
      this.mnuAssignDocument2.Text = "Assign Document...";
      this.mnuAssignDocument2.Click += new EventHandler(this.mnuAssignDocument_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(164, 6);
      this.mnuItemSelectAll.Name = "mnuItemSelectAll";
      this.mnuItemSelectAll.Size = new Size(167, 22);
      this.mnuItemSelectAll.Text = "Select All on This Page";
      this.mnuItemSelectAll.Click += new EventHandler(this.mnuItemSelectAll_Click);
      this.btnConditionStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnConditionStatus.BackColor = SystemColors.Control;
      this.btnConditionStatus.Location = new Point(683, 0);
      this.btnConditionStatus.Margin = new Padding(0);
      this.btnConditionStatus.Name = "btnConditionStatus";
      this.btnConditionStatus.Size = new Size(140, 22);
      this.btnConditionStatus.TabIndex = 7;
      this.btnConditionStatus.TabStop = false;
      this.btnConditionStatus.Text = "Condition Delivery Status";
      this.tooltip.SetToolTip((Control) this.btnConditionStatus, "Condition delivery status");
      this.btnConditionStatus.UseVisualStyleBackColor = true;
      this.btnConditionStatus.Click += new EventHandler(this.btnConditionStatus_Click);
      this.btnDeliverResponses.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeliverResponses.BackColor = SystemColors.Control;
      this.btnDeliverResponses.Location = new Point(523, 0);
      this.btnDeliverResponses.Margin = new Padding(0);
      this.btnDeliverResponses.Name = "btnDeliverResponses";
      this.btnDeliverResponses.Size = new Size(160, 22);
      this.btnDeliverResponses.TabIndex = 6;
      this.btnDeliverResponses.TabStop = false;
      this.btnDeliverResponses.Text = "Deliver Condition Responses";
      this.tooltip.SetToolTip((Control) this.btnDeliverResponses, "Deliver condition responses");
      this.btnDeliverResponses.UseVisualStyleBackColor = true;
      this.btnDeliverResponses.Click += new EventHandler(this.btnDeliverResponses_Click);
      this.btnDocuments.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDocuments.BackColor = SystemColors.Control;
      this.btnDocuments.Location = new Point(409, 0);
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
      this.btnRetrieve.Location = new Point(318, 0);
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
      this.btnRequest.Location = new Point(226, 0);
      this.btnRequest.Margin = new Padding(0);
      this.btnRequest.Name = "btnRequest";
      this.btnRequest.Size = new Size(92, 22);
      this.btnRequest.TabIndex = 3;
      this.btnRequest.TabStop = false;
      this.btnRequest.Text = "Request Docs";
      this.tooltip.SetToolTip((Control) this.btnRequest, "Send documents to borrower to sign and request needed documents");
      this.btnRequest.UseVisualStyleBackColor = true;
      this.btnRequest.Click += new EventHandler(this.btnRequest_Click);
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(201, 3);
      this.btnPrint.Margin = new Padding(4, 3, 0, 3);
      this.btnPrint.MouseDownImage = (Image) null;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 6;
      this.btnPrint.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnPrint, "Print Condition");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExcel.BackColor = Color.Transparent;
      this.btnExcel.Location = new Point(181, 3);
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
      this.btnDelete.Location = new Point(161, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 4;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete Condition");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(141, 3);
      this.btnEdit.Margin = new Padding(4, 3, 0, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 3;
      this.btnEdit.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEdit, "Edit Condition");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(121, 3);
      this.btnDuplicate.Margin = new Padding(4, 3, 0, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 2;
      this.btnDuplicate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDuplicate, "Duplicate Condition");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
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
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(101, 3);
      this.btnNew.Margin = new Padding(4, 3, 0, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 1;
      this.btnNew.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnNew, "New Condition");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.mnuConditions.Items.AddRange(new ToolStripItem[12]
      {
        (ToolStripItem) this.mnuItemNew2,
        (ToolStripItem) this.mnuItemDuplicate2,
        (ToolStripItem) this.mnuItemEdit2,
        (ToolStripItem) this.mnuItemDelete2,
        (ToolStripItem) this.mnuItemSeparator3,
        (ToolStripItem) this.mnuItemExcel2,
        (ToolStripItem) this.mnuItemPrint2,
        (ToolStripItem) this.mnuItemSeparator4,
        (ToolStripItem) this.mnuAssignDocument,
        (ToolStripItem) this.mnuItemRequest,
        (ToolStripItem) this.mnuItemRetrieve,
        (ToolStripItem) this.mnuItemDocuments
      });
      this.mnuConditions.Name = "mnuDocuments";
      this.mnuConditions.Size = new Size(206, 236);
      this.mnuItemNew2.Image = (Image) Resources._new;
      this.mnuItemNew2.Name = "mnuItemNew2";
      this.mnuItemNew2.ShortcutKeys = Keys.N | Keys.Control;
      this.mnuItemNew2.Size = new Size(205, 22);
      this.mnuItemNew2.Text = "&Add Condition...";
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
      this.mnuAssignDocument.Name = "mnuAssignDocument";
      this.mnuAssignDocument.Size = new Size(205, 22);
      this.mnuAssignDocument.Text = "Assign Document...";
      this.mnuAssignDocument.Click += new EventHandler(this.mnuAssignDocument_Click);
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
      this.gcConditions.Controls.Add((Control) this.gvConditions);
      this.gcConditions.Controls.Add((Control) this.lblMessage);
      this.gcConditions.Controls.Add((Control) this.pnlToolbar);
      this.gcConditions.Dock = DockStyle.Fill;
      this.gcConditions.HeaderForeColor = SystemColors.ControlText;
      this.gcConditions.Location = new Point(0, 62);
      this.gcConditions.Name = "gcConditions";
      this.gcConditions.Size = new Size(1028, 262);
      this.gcConditions.TabIndex = 0;
      this.cboCondType.CheckOnClick = true;
      this.cboCondType.ComboBoxText = "";
      this.cboCondType.Display = false;
      this.cboCondType.DisplayMember = "DisplayName";
      this.cboCondType.DropDownHeight = 1;
      this.cboCondType.FormattingEnabled = true;
      this.cboCondType.IntegralHeight = false;
      this.cboCondType.Location = new Point(96, 4);
      this.cboCondType.MaxDropDownItems = 16;
      this.cboCondType.Name = "cboCondType";
      this.cboCondType.Size = new Size(150, 22);
      this.cboCondType.TabIndex = 35;
      this.cboCondType.DropDownClosed += new EventHandler(this.cboCondType_DropDownClosed);
      this.gvConditions.AllowColumnReorder = true;
      this.gvConditions.BorderStyle = BorderStyle.None;
      this.gvConditions.ContextMenuStrip = this.mnuContext;
      this.gvConditions.Dock = DockStyle.Fill;
      this.gvConditions.FilterVisible = true;
      this.gvConditions.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvConditions.Location = new Point(1, 26);
      this.gvConditions.Name = "gvConditions";
      this.gvConditions.Size = new Size(1026, 235);
      this.gvConditions.TabIndex = 7;
      this.gvConditions.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvConditions.SelectedIndexChanged += new EventHandler(this.gvConditions_SelectedIndexChanged);
      this.gvConditions.SubItemEnter += new GVSubItemEventHandler(this.gvConditions_SubItemEnter);
      this.gvConditions.SubItemLeave += new GVSubItemEventHandler(this.gvConditions_SubItemLeave);
      this.gvConditions.ItemDoubleClick += new GVItemEventHandler(this.gvConditions_ItemDoubleClick);
      this.lblMessage.BackColor = SystemColors.AppWorkspace;
      this.lblMessage.Dock = DockStyle.Fill;
      this.lblMessage.ForeColor = SystemColors.Window;
      this.lblMessage.Location = new Point(1, 26);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(1026, 235);
      this.lblMessage.TabIndex = 8;
      this.lblMessage.Text = "No Conditions Yet";
      this.lblMessage.TextAlign = ContentAlignment.MiddleCenter;
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnConditionStatus);
      this.pnlToolbar.Controls.Add((Control) this.btnDeliverResponses);
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
      this.pnlToolbar.Location = new Point(198, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(823, 22);
      this.pnlToolbar.TabIndex = 1;
      this.separator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.separator.Location = new Point(221, 3);
      this.separator.Margin = new Padding(4, 3, 3, 3);
      this.separator.MaximumSize = new Size(2, 16);
      this.separator.MinimumSize = new Size(2, 16);
      this.separator.Name = "separator";
      this.separator.Size = new Size(2, 16);
      this.separator.TabIndex = 2;
      this.separator.TabStop = false;
      this.gradConditionType.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradConditionType.Controls.Add((Control) this.lblConditionType);
      this.gradConditionType.Controls.Add((Control) this.cboCondType);
      this.gradConditionType.Dock = DockStyle.Top;
      this.gradConditionType.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradConditionType.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradConditionType.Location = new Point(0, 31);
      this.gradConditionType.Name = "gradConditionType";
      this.gradConditionType.Padding = new Padding(8, 0, 0, 0);
      this.gradConditionType.Size = new Size(1028, 31);
      this.gradConditionType.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradConditionType.TabIndex = 13;
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
      this.gradConditionsView.Size = new Size(1028, 31);
      this.gradConditionsView.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradConditionsView.TabIndex = 15;
      this.cboConditionsView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboConditionsView.FormattingEnabled = true;
      this.cboConditionsView.Location = new Point(123, 5);
      this.cboConditionsView.Name = "cboConditionsView";
      this.cboConditionsView.SelectedBGColor = SystemColors.Highlight;
      this.cboConditionsView.Size = new Size(219, 21);
      this.cboConditionsView.TabIndex = 2;
      this.cboConditionsView.DropDown += new EventHandler(this.cboConditionsView_DropDown);
      this.cboConditionsView.SelectionChangeCommitted += new EventHandler(this.cboConditionsView_SelectionChangeCommitted);
      this.lblConditionsView.AutoSize = true;
      this.lblConditionsView.BackColor = Color.Transparent;
      this.lblConditionsView.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblConditionsView.Location = new Point(6, 7);
      this.lblConditionsView.Name = "lblConditionsView";
      this.lblConditionsView.Size = new Size(111, 16);
      this.lblConditionsView.TabIndex = 1;
      this.lblConditionsView.Text = "Conditions View";
      this.lblConditionType.AutoSize = true;
      this.lblConditionType.BackColor = Color.Transparent;
      this.lblConditionType.Location = new Point(6, 7);
      this.lblConditionType.Name = "lblConditionType";
      this.lblConditionType.Size = new Size(77, 14);
      this.lblConditionType.TabIndex = 36;
      this.lblConditionType.Text = "Condition Type";
      this.lblConditionType.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcConditions);
      this.Controls.Add((Control) this.gradConditionType);
      this.Controls.Add((Control) this.gradConditionsView);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (EnhancedConditionsTrackingControl);
      this.Size = new Size(1028, 324);
      this.mnuContext.ResumeLayout(false);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnExcel).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnResetConditionView).EndInit();
      ((ISupportInitialize) this.btnManageConditionView).EndInit();
      ((ISupportInitialize) this.btnSaveConditionView).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.mnuConditions.ResumeLayout(false);
      this.gcConditions.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.gradConditionType.ResumeLayout(false);
      this.gradConditionType.PerformLayout();
      this.gradConditionsView.ResumeLayout(false);
      this.gradConditionsView.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
