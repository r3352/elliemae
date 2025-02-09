// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Documents.DocumentTrackingControl
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientCommon.AIQCapsilon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.eFolder.Files;
using EllieMae.EMLite.eFolder.Properties;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.Documents
{
  public class DocumentTrackingControl : UserControl, IRefreshContents
  {
    private LoanDataMgr loanDataMgr;
    private DocumentGroupSetup groupSetup;
    private DocumentTrackingSetup docSetup;
    private DocumentGroup allGroup;
    private DocumentGroup closingGroup;
    private DocumentGroup preClosingGroup;
    private DocumentGroup disclosuresGroup;
    private DocumentGroup epassGroup;
    private GridViewDataManager gvDocumentsMgr;
    private StackingOrderSetTemplate stackingTemplate;
    private DocumentTrackingView currentView;
    private bool canAddNewDocumentName;
    private DocumentLog docSelectedForRename;
    private int _colIndexRenameDoc = -1;
    private Sessions.Session session;
    private static readonly string sw = Tracing.SwEFolder;
    private AIQButtonHelper aiqHelper;
    private LoanDataMgr currentLoan;
    private IContainer components;
    private GroupContainer gcDocuments;
    private GridView gvDocuments;
    private ImageList imageList;
    private StandardIconButton btnEdit;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnNew;
    private StandardIconButton btnDelete;
    private VerticalSeparator separator1;
    private StandardIconButton btnPrint;
    private StandardIconButton btnExcel;
    private Button btnRequest;
    private Button btnFiles;
    private Button btnRetrieve;
    private ContextMenuButton btnSend;
    private FlowLayoutPanel pnlToolbar;
    private ToolTip tooltip;
    private ContextMenuStrip mnuContext;
    private ToolStripMenuItem mnuItemNew1;
    private ToolStripMenuItem mnuItemEdit1;
    private ToolStripMenuItem mnuItemRename1;
    private ToolStripMenuItem mnuItemDuplicate1;
    private ToolStripMenuItem mnuItemDelete1;
    private ToolStripSeparator mnuItemSeparator1;
    private ToolStripMenuItem mnuItemExcel1;
    private ToolStripMenuItem mnuItemPrint1;
    private ToolStripMenuItem mnuItemAccess1;
    private ToolStripSeparator mnuItemSeparator2;
    private ToolStripMenuItem mnuItemSelectAll;
    private ContextMenuStrip mnuDocuments;
    private ToolStripMenuItem mnuItemNew2;
    private ToolStripMenuItem mnuItemDuplicate2;
    private ToolStripMenuItem mnuItemEdit2;
    private ToolStripMenuItem mnuItemRename2;
    private ToolStripMenuItem mnuItemDelete2;
    private ToolStripSeparator mnuItemSeparator3;
    private ToolStripMenuItem mnuItemExcel2;
    private ToolStripMenuItem mnuItemPrint2;
    private ToolStripSeparator mnuItemSeparator4;
    private ToolStripMenuItem mnuItemAccess2;
    private ToolStripMenuItem mnuItemRequest;
    private ToolStripMenuItem mnuItemDisclosures;
    private ToolStripMenuItem mnuItemRetrieve;
    private ToolStripMenuItem mnuItemFiles;
    private ToolStripSeparator mnuItemSeparator5;
    private ToolStripMenuItem mnuItemSend;
    private ToolStripMenuItem mnuItemCoversheet;
    private ToolStripMenuItem mnuItemSave1;
    private ToolStripMenuItem mnuItemSave2;
    private ToolStripMenuItem mnuItemSendFiles1;
    private ToolStripMenuItem mnuItemSendLender1;
    private IconButton btnAccess;
    private Button btnDisclosures;
    private ContextMenuStrip mnuSend;
    private ToolStripMenuItem mnuItemSendFiles2;
    private ToolStripMenuItem mnuItemSendLender2;
    private GradientPanel gradStackingOrder;
    private ComboBox cboDocumentGroup;
    private Label lblDocumentGroup;
    private ComboBox cboStackingOrder;
    private Label lblStackingOrder;
    private GradientPanel gradDocumentsView;
    private StandardIconButton btnResetDocumentView;
    private StandardIconButton btnManageDocumentView;
    private StandardIconButton btnSaveDocumentView;
    private ComboBox cboDocumentsView;
    private Label lblDocumentsView;
    private Button btnDocuments;
    private Button btnConsent;
    private Button btnAIQ;

    public DocumentTrackingControl(LoanDataMgr loanDataMgr, Sessions.Session session)
    {
      this.InitializeComponent();
      this.session = session;
      this.loanDataMgr = loanDataMgr;
      this.initGroupList();
      this.initializeStackingOrder();
      this.initDocumentList();
      this.initDocumentViewList();
      this.aiqHelper = new AIQButtonHelper((IWin32Window) this, this.btnAIQ);
      this.applySecurity();
      this.aiqHelper.EnableAIQLaunchButton(this.loanDataMgr);
      this.applyFieldAccessRights();
      if (this.loanDataMgr == this.currentLoan)
        return;
      if (this.currentLoan != null)
        this.currentLoan.AccessRightsChanged -= new EventHandler(this.onAccessRightsChanged);
      this.currentLoan = this.loanDataMgr;
      if (this.currentLoan == null)
        return;
      this.currentLoan.AccessRightsChanged += new EventHandler(this.onAccessRightsChanged);
    }

    private void applyFieldAccessRights() => this.setButtonAccess(this.btnDisclosures);

    private void onAccessRightsChanged(object sender, EventArgs e) => this.applyFieldAccessRights();

    private void setButtonAccess(Button button)
    {
      button.Enabled = true;
      switch (Session.LoanDataMgr.GetFieldAccessRights("Button_" + button.Text))
      {
        case BizRule.FieldAccessRight.Hide:
          button.Visible = false;
          break;
        case BizRule.FieldAccessRight.ViewOnly:
          button.Enabled = false;
          break;
      }
    }

    public void CloseDetailInstances() => DocumentDetailsDialog.CloseInstances();

    public FileSystemEntry SelectedStackingOrder
    {
      get
      {
        return this.cboStackingOrder.SelectedItem is FileSystemEntryListItem ? ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry : (FileSystemEntry) null;
      }
    }

    private void initGroupList()
    {
      this.groupSetup = this.loanDataMgr.SystemConfiguration.DocumentGroupSetup;
      this.docSetup = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      this.allGroup = new DocumentGroup(GridViewDataManager.AllDocumentGroupName);
      this.closingGroup = new DocumentGroup("(Closing Documents)");
      this.preClosingGroup = new DocumentGroup("(PreClosing Documents)");
      this.disclosuresGroup = new DocumentGroup("(eDisclosures)");
      this.epassGroup = new DocumentGroup("(Settlement Services)");
      this.cboDocumentGroup.Items.Add((object) this.allGroup);
      this.cboDocumentGroup.Items.Add((object) this.closingGroup);
      this.cboDocumentGroup.Items.Add((object) this.preClosingGroup);
      this.cboDocumentGroup.Items.Add((object) this.disclosuresGroup);
      this.cboDocumentGroup.Items.Add((object) this.epassGroup);
      foreach (DocumentGroup documentGroup in (CollectionBase) this.groupSetup)
        this.cboDocumentGroup.Items.Add((object) documentGroup);
    }

    private void initDocumentList()
    {
      this.gvDocumentsMgr = new GridViewDataManager(this.gvDocuments, this.loanDataMgr);
      this.gvDocumentsMgr.FilterChanged += new EventHandler(this.gvDocumentsMgr_FilterChanged);
      this.gvDocumentsMgr.LayoutChanged += new EventHandler(this.gvDocumentsMgr_LayoutChanged);
      this.gvDocumentsMgr.CreateLayout(new TableLayout.Column[38]
      {
        GridViewDataManager.AccessedByColumn,
        GridViewDataManager.AccessedDateColumn,
        GridViewDataManager.ArchivedDateColumn,
        GridViewDataManager.AvailableExternallyColumn,
        GridViewDataManager.BorrowerColumn,
        GridViewDataManager.ClosingDocumentColumn,
        GridViewDataManager.DateColumn,
        GridViewDataManager.DescriptionColumn,
        GridViewDataManager.DaysTillDueColumn,
        GridViewDataManager.DaysTillExpireColumn,
        GridViewDataManager.DocAccessColumn,
        GridViewDataManager.DocStatusColumn,
        GridViewDataManager.DocTypeColumn,
        GridViewDataManager.ExpectedDateColumn,
        GridViewDataManager.ExpirationDateColumn,
        GridViewDataManager.HasAttachmentsColumn,
        GridViewDataManager.HasConditionsColumn,
        GridViewDataManager.MilestoneColumn,
        GridViewDataManager.NameColumn,
        GridViewDataManager.OpeningDocumentColumn,
        GridViewDataManager.LastAttachmentColumn,
        GridViewDataManager.ShippingReadyByColumn,
        GridViewDataManager.ShippingReadyDateColumn,
        GridViewDataManager.UnderwritingReadyByColumn,
        GridViewDataManager.UnderwritingReadyDateColumn,
        GridViewDataManager.ReceivedByColumn,
        GridViewDataManager.ReceivedDateColumn,
        GridViewDataManager.RequestedByColumn,
        GridViewDataManager.RequestedDateColumn,
        GridViewDataManager.RequestedFromColumn,
        GridViewDataManager.RerequestedByColumn,
        GridViewDataManager.RerequestedDateColumn,
        GridViewDataManager.ReviewedByColumn,
        GridViewDataManager.ReviewedDateColumn,
        GridViewDataManager.FileSizeColumn,
        GridViewDataManager.TotalFileSizeColumn,
        GridViewDataManager.LastUpdatedColumn,
        GridViewDataManager.MarkedAsFinalColumn
      }, true);
    }

    private void gvDocumentsMgr_FilterChanged(object sender, EventArgs e)
    {
      this.gcDocuments.Text = "Documents (" + this.gvDocuments.VisibleItems.Count.ToString() + ")";
    }

    private void gvDocumentsMgr_LayoutChanged(object sender, EventArgs e)
    {
      this.loadDocumentList();
    }

    private void loadDocumentList()
    {
      DocumentLog[] documentLogArray = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      if (this.stackingTemplate != null && this.stackingTemplate.FilterDocuments)
      {
        List<DocumentLog> documentLogList = new List<DocumentLog>();
        foreach (string docName in this.stackingTemplate.DocNames)
        {
          foreach (DocumentLog doc in documentLogArray)
          {
            if (this.docMatchesStackingItem(doc, docName))
              documentLogList.Add(doc);
          }
        }
        documentLogArray = documentLogList.ToArray();
      }
      DocumentGroup selectedItem = (DocumentGroup) this.cboDocumentGroup.SelectedItem;
      if (selectedItem == null)
        return;
      List<DocumentLog> documentLogList1 = new List<DocumentLog>();
      if (selectedItem == this.allGroup)
      {
        foreach (DocumentLog documentLog in documentLogArray)
        {
          if (!documentLogList1.Contains(documentLog))
            documentLogList1.Add(documentLog);
        }
      }
      else if (selectedItem == this.closingGroup)
      {
        foreach (DocumentLog documentLog in documentLogArray)
        {
          if (documentLog.ClosingDocument && !documentLogList1.Contains(documentLog))
            documentLogList1.Add(documentLog);
        }
      }
      else if (selectedItem == this.preClosingGroup)
      {
        foreach (DocumentLog documentLog in documentLogArray)
        {
          if (documentLog.PreClosingDocument && !documentLogList1.Contains(documentLog))
            documentLogList1.Add(documentLog);
        }
      }
      else if (selectedItem == this.disclosuresGroup)
      {
        foreach (DocumentLog documentLog in documentLogArray)
        {
          if (documentLog.OpeningDocument && !documentLogList1.Contains(documentLog))
            documentLogList1.Add(documentLog);
        }
      }
      else if (selectedItem == this.epassGroup)
      {
        foreach (DocumentLog documentLog in documentLogArray)
        {
          if ((documentLog.IsePASS || Epass.IsEpassDoc(documentLog.Title)) && !documentLogList1.Contains(documentLog))
            documentLogList1.Add(documentLog);
        }
      }
      else
      {
        DocumentTemplate[] documents = selectedItem.GetDocuments(this.docSetup);
        List<string> stringList = new List<string>();
        foreach (DocumentTemplate documentTemplate in documents)
          stringList.Add(documentTemplate.Name);
        foreach (DocumentLog documentLog in documentLogArray)
        {
          if (stringList.Contains(documentLog.Title) && !documentLogList1.Contains(documentLog))
            documentLogList1.Add(documentLog);
        }
      }
      int[] usersAssignedRoles = this.loanDataMgr.AccessRules.GetUsersAssignedRoles();
      foreach (DocumentLog doc in documentLogList1)
      {
        Font font = (Font) null;
        if (doc.Comments.HasUnreviewedEntry(usersAssignedRoles))
          font = EncompassFonts.Normal2.Font;
        GVItem gvItem = this.gvDocuments.Items.GetItemByTag((object) doc);
        if (gvItem == null)
          gvItem = this.gvDocumentsMgr.AddItem(doc);
        else
          this.gvDocumentsMgr.RefreshItem(gvItem, doc);
        foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) gvItem.SubItems)
          subItem.Font = font;
      }
      List<GVItem> gvItemList = new List<GVItem>();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDocuments.Items)
      {
        if (!documentLogList1.Contains(gvItem.Tag as DocumentLog))
          gvItemList.Add(gvItem);
      }
      foreach (GVItem gvItem in gvItemList)
        this.gvDocuments.Items.Remove(gvItem);
      this.gvDocumentsMgr.ApplyFilter();
      this.refreshToolbar();
    }

    private void loadDocumentList(DocumentLog doc)
    {
      this.loadDocumentList(new DocumentLog[1]{ doc });
    }

    private void loadDocumentList(DocumentLog[] docList)
    {
      this.loadDocumentList();
      this.gvDocuments.SelectedItems.Clear();
      foreach (object doc in docList)
      {
        GVItem itemByTag = this.gvDocuments.Items.GetItemByTag(doc);
        if (itemByTag != null)
          itemByTag.Selected = true;
      }
    }

    private bool docMatchesStackingItem(DocumentLog doc, string stackingName)
    {
      return doc.Title.ToLower() == stackingName.ToLower() || this.stackingTemplate.NDEDocGroups.Contains((object) stackingName) && doc.GroupName.ToLower() == stackingName.ToLower();
    }

    public DocumentLog[] getSelectedDocuments()
    {
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (GVItem selectedItem in this.gvDocuments.SelectedItems)
        documentLogList.Add((DocumentLog) selectedItem.Tag);
      return documentLogList.ToArray();
    }

    private void gvDocuments_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      this.btnEdit_Click(source, EventArgs.Empty);
    }

    private void gvDocuments_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.refreshToolbar();
    }

    public ToolStripDropDown Menu => (ToolStripDropDown) this.mnuDocuments;

    private void refreshToolbar()
    {
      int count = this.gvDocuments.SelectedItems.Count;
      this.btnDuplicate.Enabled = count == 1;
      this.btnEdit.Enabled = count > 0;
      this.btnDelete.Enabled = count > 0;
      this.btnPrint.Enabled = count > 0;
      this.btnAccess.Enabled = count > 0;
      this.mnuItemDuplicate1.Enabled = count == 1;
      this.mnuItemDuplicate2.Enabled = count == 1;
      this.mnuItemEdit1.Enabled = count > 0;
      this.mnuItemEdit2.Enabled = count > 0;
      this.mnuItemDelete1.Enabled = count > 0;
      this.mnuItemDelete2.Enabled = count > 0;
      this.mnuItemPrint1.Enabled = count > 0;
      this.mnuItemPrint2.Enabled = count > 0;
      this.mnuItemSave1.Enabled = count > 0;
      this.mnuItemSave2.Enabled = count > 0;
      this.mnuItemAccess1.Enabled = count > 0;
      this.mnuItemAccess2.Enabled = count > 0;
      if (this.docSelectedForRename == null && this.getSelectedDocuments().Length != 0)
        this.docSelectedForRename = this.getSelectedDocuments()[0];
      eFolderAccessRights folderAccessRights = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) this.docSelectedForRename);
      if (this.gvDocuments.Columns.GetColumnByName("Column3") == null || !folderAccessRights.CanEditDocument)
      {
        this.mnuItemRename1.Enabled = false;
        this.mnuItemRename2.Enabled = false;
      }
      else
      {
        this.mnuItemRename1.Enabled = count == 1;
        this.mnuItemRename2.Enabled = count == 1;
      }
    }

    private void renameOnClick(object sender, EventArgs e)
    {
      if (this.getSelectedDocuments().Length != 1)
        return;
      this.docSelectedForRename = this.getSelectedDocuments()[0];
      GVColumn columnByName = this.gvDocuments.Columns.GetColumnByName("Column3");
      if (columnByName == null)
        return;
      this._colIndexRenameDoc = columnByName.Index;
      this.gvDocuments.Columns[this._colIndexRenameDoc].ActivatedEditorType = GVActivatedEditorType.ComboBox;
      this.gvDocuments.SelectedItems[0].SubItems[this._colIndexRenameDoc].BeginEdit();
    }

    private ArrayList LoadPreDefinedDocumentNames()
    {
      ArrayList arrayList = new ArrayList();
      this.docSetup = this.loanDataMgr.SystemConfiguration.DocumentTrackingSetup;
      foreach (DocumentTemplate documentTemplate in this.docSetup)
        arrayList.Add((object) documentTemplate.Name);
      return arrayList;
    }

    private void gvDocuments_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      ComboBox editorControl = (ComboBox) e.EditorControl;
      editorControl.Items.Clear();
      editorControl.DropDownStyle = !this.canAddNewDocumentName ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
      editorControl.Items.AddRange(this.LoadPreDefinedDocumentNames().ToArray());
      if (!editorControl.Items.Contains((object) this.docSelectedForRename.Title))
        editorControl.Items.Add((object) this.docSelectedForRename.Title);
      editorControl.Text = (e.SubItem.Text ?? "") == "" ? this.docSelectedForRename.Title : e.SubItem.Text;
    }

    private void gvDocuments_EditorClosing(object source, GVSubItemEditingEventArgs e)
    {
      DocumentLog documentLog = (DocumentLog) null;
      string str = e.EditorControl.Text.Trim();
      this.gvDocuments.CancelEditing();
      if (str != e.SubItem.Text)
        documentLog = (DocumentLog) e.SubItem.Item.Tag;
      if (str != e.SubItem.Text && str != string.Empty)
        documentLog.Title = str;
      else if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
        documentLog.Title = "Untitled";
      this.docSelectedForRename.MarkAsAccessed(DateTime.Now, Session.UserID);
      this.gvDocuments.Columns[this._colIndexRenameDoc].ActivatedEditorType = GVActivatedEditorType.None;
    }

    private void btnAccess_Click(object sender, EventArgs e)
    {
      using (DocumentAccessDialog documentAccessDialog = new DocumentAccessDialog(this.loanDataMgr, this.getSelectedDocuments()))
      {
        int num = (int) documentAccessDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnConsent_Click(object sender, EventArgs e)
    {
      Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
      Session.Application.GetService<IEFolder>().SendConsentRequest(this.loanDataMgr);
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (!this.loanDataMgr.LockLoanWithExclusiveA())
        return;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      foreach (DocumentLog selectedDocument in this.getSelectedDocuments())
      {
        if (!this.loanDataMgr.LoanData.AccessRules.IsLogEntryDeletable((LogRecordBase) selectedDocument))
          arrayList1.Add((object) selectedDocument);
        else if (this.loanDataMgr.LoanData.AccessRules.IsLogEntryProtected((LogRecordBase) selectedDocument))
          arrayList2.Add((object) selectedDocument);
        else
          arrayList3.Add((object) selectedDocument);
      }
      if (arrayList1.Count > 0)
      {
        string str = string.Empty;
        foreach (DocumentLog documentLog in arrayList1)
          str = str + documentLog.Title + "\r\n";
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following document(s) cannot be deleted:\r\n\r\n" + str, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      if (arrayList2.Count > 0)
      {
        string str = string.Empty;
        foreach (DocumentLog documentLog in arrayList2)
          str = str + documentLog.Title + "\r\n";
        if (Utils.Dialog((IWin32Window) Form.ActiveForm, "The following document(s) are protected:\r\n\r\n" + str + "\r\nAre you sure that you want to delete them?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          foreach (DocumentLog rec in arrayList2)
            this.loanDataMgr.LoanData.GetLogList().RemoveRecord((LogRecordBase) rec);
        }
      }
      if (arrayList3.Count <= 0)
        return;
      string str1 = string.Empty;
      foreach (DocumentLog documentLog in arrayList3)
        str1 = str1 + documentLog.Title + "\r\n";
      if (Utils.Dialog((IWin32Window) Form.ActiveForm, "Are you sure that you want to delete the following document(s):\r\n\r\n" + str1, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      foreach (DocumentLog rec in arrayList3)
        this.loanDataMgr.LoanData.GetLogList().RemoveRecord((LogRecordBase) rec);
    }

    private void updateBusinessRule()
    {
      try
      {
        if (this.loanDataMgr.LoanData == null || this.loanDataMgr.LoanData.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }

    private void btnDisclosures_Click(object sender, EventArgs e)
    {
      Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
      if (this.loanDataMgr.SessionObjects.StartupInfo.EnableSendDisclosureSmartClient && this.loanDataMgr.IsPlatformLoan(setCCSiteId: true))
        Session.Application.GetService<IEFolder>().LaunchEDisclosures(Session.LoanDataMgr, Convert.ToInt32((double) this.Height * 0.9), Convert.ToInt32((double) this.Width * 0.9));
      else if (!string.IsNullOrEmpty(this.loanDataMgr.WCNotAllowedMessage))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, this.loanDataMgr.WCNotAllowedMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("DocTrCtrBtnDiscClk", "DOCS NDE-13455 Open Plancode Dialog", true, 758, nameof (btnDisclosures_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Documents\\DocumentTrackingControl.cs"))
        {
          try
          {
            performanceMeter.AddVariable("SessionId", (object) this.session.SessionID);
            performanceMeter.AddVariable("UserId", (object) this.session.UserID);
          }
          catch
          {
          }
          if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
          {
            PerformanceMeter.Current.AddCheckpoint("Before Early Exit, not IsModuleAvailableForUser", 770, nameof (btnDisclosures_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Documents\\DocumentTrackingControl.cs");
          }
          else
          {
            if ((bool) this.session.StartupInfo.PolicySettings[(object) "Policies.VALIDATESYSRULEPRIORORDERINGDOC"] && ((IEnumerable<BizRuleInfo>) ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldRules)).GetAllActiveRules()).Any<BizRuleInfo>((Func<BizRuleInfo, bool>) (b => ((FieldRuleInfo) b).RequiredFields.ContainsKey((object) "BUTTON_EDISCLOSURES"))) | ((IEnumerable<BizRuleInfo>) ((BpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldAccess)).GetAllActiveRules()).SelectMany<BizRuleInfo, FieldAccessRights>((Func<BizRuleInfo, IEnumerable<FieldAccessRights>>) (far => (IEnumerable<FieldAccessRights>) ((FieldAccessRuleInfo) far).FieldAccessRights)).Any<FieldAccessRights>((Func<FieldAccessRights, bool>) (b => b.FieldID.ToUpper() == "BUTTON_EDISCLOSURES")) && this.loanDataMgr.LoanData.Dirty)
            {
              if (Utils.Dialog((IWin32Window) this, "You must save the loan before you can add a Disclosure Tracking entry. Would you like to save the loan now?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                return;
              WaitDialog waitDialog = new WaitDialog("Encompass is currently checking validation rules to determine if there are known discrepancies before Documents can be drawn", (WaitCallback) null, (object) null);
              waitDialog.Show((IWin32Window) this);
              waitDialog.Top = this.Top + this.Height / 2 - waitDialog.Height / 2;
              waitDialog.Left = this.Left + this.Width / 2 - waitDialog.Width / 2;
              if (!Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false))
              {
                waitDialog.Close();
                return;
              }
              this.updateBusinessRule();
              waitDialog.Close();
              this.applyFieldAccessRights();
              if (!this.btnDisclosures.Visible || !this.btnDisclosures.Enabled)
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "This action cannot be performed at this time due to system rules. Please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
            }
            if (new BusinessRuleCheck().HasPrerequiredFields(this.loanDataMgr.LoanData, "BUTTON_EDISCLOSURES", true, (Hashtable) null))
            {
              PerformanceMeter.Current.AddCheckpoint("Before Early Exit, not HasPrerequiredFields", 823, nameof (btnDisclosures_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Documents\\DocumentTrackingControl.cs");
            }
            else
            {
              this.loanDataMgr.DDMTriggerExecute(DDMStartPopulationTrigger.LoanSave, false);
              try
              {
                this.loanDataMgr.LoanData.Calculator.CalcOnDemand();
                bool skipLockRequestSync = this.loanDataMgr.LoanData.Calculator.SkipLockRequestSync;
                this.loanDataMgr.LoanData.Calculator.SkipLockRequestSync = true;
                this.loanDataMgr.LoanData.Calculator.CalculateAll(false);
                this.loanDataMgr.LoanData.Calculator.SkipLockRequestSync = skipLockRequestSync;
              }
              catch (Exception ex)
              {
              }
              PerformanceMeter.Current.AddCheckpoint("Calcs Complete", 844, nameof (btnDisclosures_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Documents\\DocumentTrackingControl.cs");
              BorrowerPair currentBorrowerPair = this.loanDataMgr.LoanData.CurrentBorrowerPair;
              Session.Application.GetService<ILoanServices>();
              IEPass service = Session.Application.GetService<IEPass>();
              PerformanceMeter.Current.AddCheckpoint("Before ProcessURL", 856, nameof (btnDisclosures_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Documents\\DocumentTrackingControl.cs");
              service.ProcessURL("_EPASS_SIGNATURE;ENCOMPASSDOCS;EDISCLOSURES2", false);
              PerformanceMeter.Current.AddCheckpoint("After ProcessURL", 858, nameof (btnDisclosures_Click), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\Documents\\DocumentTrackingControl.cs");
              if (!(currentBorrowerPair.Id != this.loanDataMgr.LoanData.PairId))
                return;
              this.loanDataMgr.LoanData.SetBorrowerPair(currentBorrowerPair);
            }
          }
        }
      }
    }

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      DocumentLog selectedDocument = this.getSelectedDocuments()[0];
      DocumentLog documentLog = new DocumentLog(Session.UserID, selectedDocument.PairId);
      documentLog.Title = selectedDocument.Title;
      documentLog.Description = selectedDocument.Description;
      documentLog.Stage = selectedDocument.Stage;
      documentLog.DaysDue = selectedDocument.DaysDue;
      documentLog.DaysTillExpire = selectedDocument.DaysTillExpire;
      this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) documentLog);
      this.loadDocumentList(documentLog);
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      if (selectedDocuments.Length < 1)
        return;
      if (selectedDocuments.Length == 1)
        DocumentDetailsDialog.ShowInstance(this.loanDataMgr, selectedDocuments[0]);
      if (selectedDocuments.Length <= 1)
        return;
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
        AllDocumentsDialog.ShowInstance(this.loanDataMgr, selectedDocuments, ConditionType.Enhanced);
      else
        AllDocumentsDialog.ShowInstance(this.loanDataMgr, selectedDocuments, ConditionType.Underwriting);
    }

    private void btnExcel_Click(object sender, EventArgs e)
    {
      List<GVItem> gvItemList = this.gvDocuments.SelectedItems.Count <= 0 ? new List<GVItem>((IEnumerable<GVItem>) this.gvDocuments.VisibleItems) : new List<GVItem>((IEnumerable<GVItem>) this.gvDocuments.SelectedItems);
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn gvColumn in this.gvDocuments.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(gvColumn.Text);
        foreach (GVItem gvItem in gvItemList)
        {
          string[] data = new string[this.gvDocuments.Columns.Count];
          for (int index1 = 0; index1 < data.Length; ++index1)
          {
            int index2 = this.gvDocuments.Columns.DisplaySequence[index1].Index;
            data[index1] = gvItem.SubItems[index2].Text;
          }
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private void btnDocuments_Click(object sender, EventArgs e)
    {
      if (this.loanDataMgr.LoanData.EnableEnhancedConditions)
        AllDocumentsDialog.ShowInstance(this.loanDataMgr, this.getSelectedDocuments(), ConditionType.Enhanced);
      else
        AllDocumentsDialog.ShowInstance(this.loanDataMgr, this.getSelectedDocuments(), ConditionType.Underwriting);
    }

    private void btnFiles_Click(object sender, EventArgs e)
    {
      AllFilesDialog.ShowInstance(this.loanDataMgr, this.session);
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      using (AddDocumentDialog addDocumentDialog = new AddDocumentDialog(this.loanDataMgr))
      {
        if (addDocumentDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
          return;
        if (this.cboDocumentGroup.SelectedItem != this.allGroup)
          this.cboDocumentGroup.SelectedItem = (object) this.allGroup;
        this.loadDocumentList(addDocumentDialog.Documents);
      }
    }

    private void btnPrint_Click(object sender, EventArgs e)
    {
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      FileSystemEntry defaultStackingEntry = (FileSystemEntry) null;
      if (this.cboStackingOrder.SelectedItem is FileSystemEntryListItem)
        defaultStackingEntry = ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry;
      using (PrintDocumentsDialog printDocumentsDialog = new PrintDocumentsDialog(this.loanDataMgr, selectedDocuments, defaultStackingEntry))
      {
        int num = (int) printDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void btnRequest_Click(object sender, EventArgs e)
    {
      Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      Session.Application.GetService<IEFolder>().Request(this.loanDataMgr, selectedDocuments);
    }

    private void btnRetrieve_Click(object sender, EventArgs e)
    {
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      Session.Application.GetService<IEFolder>().Retrieve(this.loanDataMgr, selectedDocuments, this.session);
    }

    private void mnuItemSendFiles_Click(object sender, EventArgs e)
    {
      Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      DocumentLog[] allDocuments = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      List<DocumentLog> documentLogList = new List<DocumentLog>();
      foreach (DocumentLog documentLog in allDocuments)
      {
        if (documentLog.IsWebcenter)
          documentLogList.Add(documentLog);
      }
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      string settingFromCache = Session.SessionObjects.GetCompanySettingFromCache("StackingOrder", "Default");
      FileSystemEntry defaultStackingEntry = (FileSystemEntry) null;
      try
      {
        defaultStackingEntry = FileSystemEntry.Parse(settingFromCache);
      }
      catch
      {
      }
      using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(this.loanDataMgr, documentLogList.ToArray(), selectedDocuments, defaultStackingEntry, SelectDocumentsReasonType.SendToBorrower))
      {
        if (selectDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          return;
        Session.Application.GetService<IEFolder>().SendDocuments(this.loanDataMgr, selectDocumentsDialog.Documents);
      }
    }

    private void mnuItemSendLender_Click(object sender, EventArgs e)
    {
      Session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      Session.Application.GetService<IEPass>().ProcessURL("_EPASS_SIGNATURE;EDM_SUBMIT;2", false);
    }

    private void mnuItemSave_Click(object sender, EventArgs e)
    {
      DocumentLog[] allDocuments = this.loanDataMgr.LoanData.GetLogList().GetAllDocuments();
      DocumentLog[] selectedDocuments = this.getSelectedDocuments();
      FileSystemEntry defaultStackingEntry = (FileSystemEntry) null;
      if (this.cboStackingOrder.SelectedItem is FileSystemEntryListItem)
        defaultStackingEntry = ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry;
      using (SelectDocumentsDialog selectDocumentsDialog = new SelectDocumentsDialog(this.loanDataMgr, allDocuments, selectedDocuments, defaultStackingEntry, SelectDocumentsReasonType.PrintSave))
      {
        if (selectDocumentsDialog.ShowDialog((IWin32Window) Form.ActiveForm) == DialogResult.Cancel)
          return;
        using (PdfFileBuilder pdfFileBuilder = new PdfFileBuilder(this.loanDataMgr))
        {
          string file = pdfFileBuilder.CreateFile(selectDocumentsDialog.Documents);
          if (file == null)
            return;
          using (SaveFileDialog saveFileDialog = new SaveFileDialog())
          {
            saveFileDialog.Filter = "Adobe PDF Documents (*.pdf)|*.pdf|All Files (*.*)|*.*";
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
              return;
            File.Copy(file, saveFileDialog.FileName, true);
          }
        }
      }
    }

    private void mnuItemCoversheet_Click(object sender, EventArgs e)
    {
      if (!Modules.IsModuleAvailableForUser(EncompassModule.EDM, true))
        return;
      string pdfFile = new FormExport(this.loanDataMgr).ExportFaxCoversheet();
      if (pdfFile == null)
        return;
      using (PdfPrintDialog pdfPrintDialog = new PdfPrintDialog(pdfFile))
      {
        int num = (int) pdfPrintDialog.ShowDialog((IWin32Window) this);
      }
    }

    private void mnuItemSelectAll_Click(object sender, EventArgs e)
    {
      this.gvDocuments.Items.SelectAll();
    }

    private void applySecurity()
    {
      eFolderAccessRights folderAccessRights1 = new eFolderAccessRights(this.loanDataMgr);
      this.btnNew.Visible = folderAccessRights1.CanAddDocuments;
      this.btnDuplicate.Visible = folderAccessRights1.CanAddDocuments;
      this.btnDelete.Visible = folderAccessRights1.CanDeleteDocuments;
      this.btnAccess.Visible = folderAccessRights1.CanSetDocumentAccess;
      this.btnConsent.Visible = folderAccessRights1.CanSendConsent;
      this.btnRequest.Visible = folderAccessRights1.CanRequestDocuments || folderAccessRights1.CanRequestServices;
      this.btnDisclosures.Visible = folderAccessRights1.CanSendDisclosures;
      this.btnRetrieve.Visible = folderAccessRights1.CanRetrieveDocuments || folderAccessRights1.CanRetrieveServices;
      this.btnSend.Visible = folderAccessRights1.CanSendDocuments || folderAccessRights1.CanSubmitDocuments;
      this.mnuItemNew1.Visible = folderAccessRights1.CanAddDocuments;
      this.mnuItemNew2.Visible = folderAccessRights1.CanAddDocuments;
      this.mnuItemDuplicate1.Visible = folderAccessRights1.CanAddDocuments;
      this.mnuItemDuplicate2.Visible = folderAccessRights1.CanAddDocuments;
      this.mnuItemDelete1.Visible = folderAccessRights1.CanDeleteDocuments;
      this.mnuItemDelete2.Visible = folderAccessRights1.CanDeleteDocuments;
      this.mnuItemRequest.Visible = folderAccessRights1.CanRequestDocuments || folderAccessRights1.CanRequestServices;
      this.mnuItemDisclosures.Visible = folderAccessRights1.CanSendDisclosures;
      this.mnuItemRetrieve.Visible = folderAccessRights1.CanRetrieveDocuments || folderAccessRights1.CanRetrieveServices;
      this.mnuItemAccess1.Visible = folderAccessRights1.CanSetDocumentAccess;
      this.mnuItemAccess2.Visible = folderAccessRights1.CanSetDocumentAccess;
      this.mnuItemSend.Visible = folderAccessRights1.CanSendDocuments || folderAccessRights1.CanSubmitDocuments;
      this.mnuItemSendFiles1.Visible = folderAccessRights1.CanSendDocuments;
      this.mnuItemSendFiles2.Visible = folderAccessRights1.CanSendDocuments;
      this.mnuItemSendLender1.Visible = folderAccessRights1.CanSubmitDocuments;
      this.mnuItemSendLender2.Visible = folderAccessRights1.CanSubmitDocuments;
      if (this.docSelectedForRename == null && this.getSelectedDocuments().Length != 0)
        this.docSelectedForRename = this.getSelectedDocuments()[0];
      eFolderAccessRights folderAccessRights2 = new eFolderAccessRights(this.loanDataMgr, (LogRecordBase) this.docSelectedForRename);
      if (this.gvDocuments.Columns.GetColumnByName("Column3") != null || !folderAccessRights1.CanEditDocument)
      {
        this.mnuItemRename1.Enabled = folderAccessRights2.CanEditDocument;
        this.mnuItemRename2.Enabled = folderAccessRights2.CanEditDocument;
        if (folderAccessRights2.CanAddNewDocumentName)
          this.canAddNewDocumentName = folderAccessRights2.CanAddNewDocumentName;
      }
      else
      {
        this.mnuItemRename1.Enabled = false;
        this.mnuItemRename2.Enabled = false;
      }
      if (this.loanDataMgr.LoanData == null || this.loanDataMgr.LoanData.LinkSyncType != LinkSyncType.ConstructionLinked)
        return;
      this.btnDisclosures.Enabled = false;
      this.btnConsent.Enabled = false;
    }

    public void RefreshContents() => this.loadDocumentList();

    public void RefreshLoanContents() => this.loadDocumentList();

    private void initializeStackingOrder()
    {
      this.cboStackingOrder.Items.Add((object) GridViewDataManager.DefaultStackingOrderName);
    }

    private void cboStackingOrder_DropDown(object sender, EventArgs e)
    {
      foreach (FileSystemEntry templateDirEntry in Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, FileSystemEntry.PublicRoot))
      {
        FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(templateDirEntry);
        if (!this.cboStackingOrder.Items.Contains((object) systemEntryListItem))
          this.cboStackingOrder.Items.Add((object) systemEntryListItem);
      }
      this.cboStackingOrder.DropDown -= new EventHandler(this.cboStackingOrder_DropDown);
    }

    private void cboStackingOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cboStackingOrder.SelectedItem is FileSystemEntryListItem)
      {
        this.stackingTemplate = (StackingOrderSetTemplate) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.StackingOrder, ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry);
        this.gvDocuments.SortOption = GVSortOption.Owner;
        this.gvDocuments.SortIconVisible = false;
        if (this.gvDocuments.Items.Count > 0)
          this.gvDocuments.ReSort();
      }
      else
      {
        this.stackingTemplate = (StackingOrderSetTemplate) null;
        this.gvDocuments.SortOption = GVSortOption.Auto;
        this.gvDocuments.SortIconVisible = true;
      }
      this.loadDocumentList();
    }

    private void cboDocumentGroup_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.loadDocumentList();
    }

    private void gvDocuments_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.stackingTemplate != null)
        this.gvDocuments.Items.Sort((IComparer<GVItem>) new DocumentSortComparer(this.loanDataMgr.LoanData, this.stackingTemplate));
      e.Cancel = true;
    }

    private void cboDocumentsView_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cboDocumentsView.SelectedItem == null)
        return;
      this.currentView = this.session.EfolderDocTrackViewManager.GetView(this.cboDocumentsView.SelectedValue.ToString());
      this.setCurrentView(this.currentView);
    }

    private void btnSaveDocumentView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void btnManageDocumentView_Click(object sender, EventArgs e)
    {
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView, false, "DocumentTrackingView.DefaultView"))
      {
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
        this.initDocumentViewList();
      }
    }

    private void btnResetDocumentView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentView(this.currentView);
    }

    private void initDocumentViewList()
    {
      List<ViewSummary> viewsSummary = this.session.EfolderDocTrackViewManager.GetViewsSummary();
      this.cboDocumentsView.DataSource = (object) viewsSummary;
      this.cboDocumentsView.DisplayMember = "Name";
      this.cboDocumentsView.ValueMember = "Id";
      this.cboDocumentsView.SelectedItem = (object) viewsSummary.Where<ViewSummary>((Func<ViewSummary, bool>) (v => v.IsDefault)).FirstOrDefault<ViewSummary>();
      if (this.cboDocumentsView.SelectedItem == null)
        return;
      this.currentView = this.session.EfolderDocTrackViewManager.GetView(this.cboDocumentsView.SelectedValue.ToString());
      this.setCurrentView(this.currentView);
    }

    private void setCurrentView(DocumentTrackingView view)
    {
      try
      {
        if (view == null)
          return;
        this.gvDocumentsMgr.ApplyView(view);
        if (!string.IsNullOrEmpty(view.StackingOrder))
        {
          if (string.Compare(view.StackingOrder, GridViewDataManager.DefaultStackingOrderName, true) == 0)
          {
            this.cboStackingOrder.SelectedItem = (object) GridViewDataManager.DefaultStackingOrderName;
          }
          else
          {
            FileSystemEntryListItem systemEntryListItem = new FileSystemEntryListItem(FileSystemEntry.Parse(view.StackingOrder));
            if (!this.cboStackingOrder.Items.Contains((object) systemEntryListItem))
              this.cboStackingOrder.Items.Add((object) systemEntryListItem);
            this.cboStackingOrder.SelectedItem = (object) systemEntryListItem;
          }
        }
        else
          this.cboStackingOrder.SelectedItem = (object) GridViewDataManager.DefaultStackingOrderName;
        if (!string.IsNullOrEmpty(view.DocGroup))
        {
          int num = this.cboDocumentGroup.FindString(view.DocGroup);
          if (-1 != num)
            this.cboDocumentGroup.SelectedIndex = num;
          else
            this.cboDocumentGroup.SelectedItem = (object) this.allGroup;
        }
        else
          this.cboDocumentGroup.SelectedItem = (object) this.allGroup;
        this.loadDocumentList();
      }
      catch (Exception ex)
      {
        ErrorDialog.Display(ex);
      }
    }

    private void saveCurrentView()
    {
      if (this.currentView == null)
        return;
      DocumentTrackingView currentView = this.currentView;
      currentView.Layout = this.gvDocumentsMgr.GetCurrentLayout();
      currentView.Filter = this.gvDocumentsMgr.GetCurrentFilter();
      currentView.DocGroup = ((DocumentGroup) this.cboDocumentGroup.SelectedItem).ToString();
      currentView.StackingOrder = string.Compare(this.cboStackingOrder.SelectedItem.ToString(), GridViewDataManager.DefaultStackingOrderName) == 0 ? GridViewDataManager.DefaultStackingOrderName : ((FileSystemEntryListItem) this.cboStackingOrder.SelectedItem).Entry.ToString();
      using (SaveViewDialog saveViewDialog = new SaveViewDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentTrackingView, (BinaryConvertibleObject) currentView, this.getViewNameList(), this.currentView.Name != GridViewDataManager.StandardViewName))
      {
        if (saveViewDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        this.initDocumentViewList();
      }
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (object obj in this.cboDocumentsView.Items)
        stringList.Add(obj.ToString());
      return stringList.ToArray();
    }

    private void btnAIQ_Click(object sender, EventArgs e)
    {
      this.aiqHelper.btnClick_action(this.loanDataMgr.LoanData.GUID);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DocumentTrackingControl));
      this.imageList = new ImageList(this.components);
      this.tooltip = new ToolTip(this.components);
      this.btnSend = new ContextMenuButton();
      this.mnuSend = new ContextMenuStrip(this.components);
      this.mnuItemSendFiles2 = new ToolStripMenuItem();
      this.mnuItemSendLender2 = new ToolStripMenuItem();
      this.btnFiles = new Button();
      this.btnDocuments = new Button();
      this.btnRetrieve = new Button();
      this.btnDisclosures = new Button();
      this.btnRequest = new Button();
      this.btnConsent = new Button();
      this.btnPrint = new StandardIconButton();
      this.btnExcel = new StandardIconButton();
      this.btnAccess = new IconButton();
      this.btnDelete = new StandardIconButton();
      this.btnEdit = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.btnResetDocumentView = new StandardIconButton();
      this.btnManageDocumentView = new StandardIconButton();
      this.btnSaveDocumentView = new StandardIconButton();
      this.mnuContext = new ContextMenuStrip(this.components);
      this.mnuItemNew1 = new ToolStripMenuItem();
      this.mnuItemEdit1 = new ToolStripMenuItem();
      this.mnuItemRename1 = new ToolStripMenuItem();
      this.mnuItemDuplicate1 = new ToolStripMenuItem();
      this.mnuItemDelete1 = new ToolStripMenuItem();
      this.mnuItemSeparator1 = new ToolStripSeparator();
      this.mnuItemExcel1 = new ToolStripMenuItem();
      this.mnuItemPrint1 = new ToolStripMenuItem();
      this.mnuItemSave1 = new ToolStripMenuItem();
      this.mnuItemAccess1 = new ToolStripMenuItem();
      this.mnuItemSeparator2 = new ToolStripSeparator();
      this.mnuItemSelectAll = new ToolStripMenuItem();
      this.mnuDocuments = new ContextMenuStrip(this.components);
      this.mnuItemNew2 = new ToolStripMenuItem();
      this.mnuItemDuplicate2 = new ToolStripMenuItem();
      this.mnuItemEdit2 = new ToolStripMenuItem();
      this.mnuItemRename2 = new ToolStripMenuItem();
      this.mnuItemDelete2 = new ToolStripMenuItem();
      this.mnuItemSeparator3 = new ToolStripSeparator();
      this.mnuItemAccess2 = new ToolStripMenuItem();
      this.mnuItemExcel2 = new ToolStripMenuItem();
      this.mnuItemPrint2 = new ToolStripMenuItem();
      this.mnuItemSave2 = new ToolStripMenuItem();
      this.mnuItemSeparator4 = new ToolStripSeparator();
      this.mnuItemRequest = new ToolStripMenuItem();
      this.mnuItemDisclosures = new ToolStripMenuItem();
      this.mnuItemRetrieve = new ToolStripMenuItem();
      this.mnuItemFiles = new ToolStripMenuItem();
      this.mnuItemSend = new ToolStripMenuItem();
      this.mnuItemSendFiles1 = new ToolStripMenuItem();
      this.mnuItemSendLender1 = new ToolStripMenuItem();
      this.mnuItemSeparator5 = new ToolStripSeparator();
      this.mnuItemCoversheet = new ToolStripMenuItem();
      this.gcDocuments = new GroupContainer();
      this.btnAIQ = new Button();
      this.gvDocuments = new GridView();
      this.pnlToolbar = new FlowLayoutPanel();
      this.separator1 = new VerticalSeparator();
      this.gradStackingOrder = new GradientPanel();
      this.cboDocumentGroup = new ComboBox();
      this.lblDocumentGroup = new Label();
      this.cboStackingOrder = new ComboBox();
      this.lblStackingOrder = new Label();
      this.gradDocumentsView = new GradientPanel();
      this.cboDocumentsView = new ComboBox();
      this.lblDocumentsView = new Label();
      this.mnuSend.SuspendLayout();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnExcel).BeginInit();
      ((ISupportInitialize) this.btnAccess).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnEdit).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnResetDocumentView).BeginInit();
      ((ISupportInitialize) this.btnManageDocumentView).BeginInit();
      ((ISupportInitialize) this.btnSaveDocumentView).BeginInit();
      this.mnuContext.SuspendLayout();
      this.mnuDocuments.SuspendLayout();
      this.gcDocuments.SuspendLayout();
      this.pnlToolbar.SuspendLayout();
      this.gradStackingOrder.SuspendLayout();
      this.gradDocumentsView.SuspendLayout();
      this.SuspendLayout();
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "document-group-private.png");
      this.btnSend.BackColor = SystemColors.Control;
      this.btnSend.ButtonMenuStrip = this.mnuSend;
      this.btnSend.Location = new Point(674, 0);
      this.btnSend.Margin = new Padding(0);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(56, 22);
      this.btnSend.TabIndex = 10;
      this.btnSend.TabStop = false;
      this.btnSend.Text = "Send";
      this.tooltip.SetToolTip((Control) this.btnSend, "Send files to borrower, partners, and lenders");
      this.btnSend.UseVisualStyleBackColor = true;
      this.mnuSend.Items.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuItemSendFiles2,
        (ToolStripItem) this.mnuItemSendLender2
      });
      this.mnuSend.Name = "mnuSend";
      this.mnuSend.Size = new Size(189, 48);
      this.mnuItemSendFiles2.Name = "mnuItemSendFiles2";
      this.mnuItemSendFiles2.Size = new Size(188, 22);
      this.mnuItemSendFiles2.Text = "Send Files...";
      this.mnuItemSendFiles2.Click += new EventHandler(this.mnuItemSendFiles_Click);
      this.mnuItemSendLender2.Name = "mnuItemSendLender2";
      this.mnuItemSendLender2.Size = new Size(188, 22);
      this.mnuItemSendLender2.Text = "Send Files to Lender...";
      this.mnuItemSendLender2.Click += new EventHandler(this.mnuItemSendLender_Click);
      this.btnFiles.BackColor = SystemColors.Control;
      this.btnFiles.Location = new Point(592, 0);
      this.btnFiles.Margin = new Padding(0);
      this.btnFiles.Name = "btnFiles";
      this.btnFiles.Size = new Size(82, 22);
      this.btnFiles.TabIndex = 9;
      this.btnFiles.TabStop = false;
      this.btnFiles.Text = "File Manager";
      this.tooltip.SetToolTip((Control) this.btnFiles, "Attach files and assign to documents");
      this.btnFiles.UseVisualStyleBackColor = true;
      this.btnFiles.Click += new EventHandler(this.btnFiles_Click);
      this.btnDocuments.BackColor = SystemColors.Control;
      this.btnDocuments.Location = new Point(482, 0);
      this.btnDocuments.Margin = new Padding(0);
      this.btnDocuments.Name = "btnDocuments";
      this.btnDocuments.Size = new Size(110, 22);
      this.btnDocuments.TabIndex = 14;
      this.btnDocuments.TabStop = false;
      this.btnDocuments.Text = "Document Manager";
      this.tooltip.SetToolTip((Control) this.btnDocuments, "Edit selected documents");
      this.btnDocuments.UseVisualStyleBackColor = true;
      this.btnDocuments.Click += new EventHandler(this.btnDocuments_Click);
      this.btnRetrieve.BackColor = SystemColors.Control;
      this.btnRetrieve.Location = new Point(421, 0);
      this.btnRetrieve.Margin = new Padding(0);
      this.btnRetrieve.Name = "btnRetrieve";
      this.btnRetrieve.Size = new Size(61, 22);
      this.btnRetrieve.TabIndex = 8;
      this.btnRetrieve.TabStop = false;
      this.btnRetrieve.Text = "Retrieve";
      this.tooltip.SetToolTip((Control) this.btnRetrieve, "Retrieve faxes and documents from borrower and service provider");
      this.btnRetrieve.UseVisualStyleBackColor = true;
      this.btnRetrieve.Click += new EventHandler(this.btnRetrieve_Click);
      this.btnDisclosures.BackColor = SystemColors.Control;
      this.btnDisclosures.Location = new Point(339, 0);
      this.btnDisclosures.Margin = new Padding(0);
      this.btnDisclosures.Name = "btnDisclosures";
      this.btnDisclosures.Size = new Size(82, 22);
      this.btnDisclosures.TabIndex = 7;
      this.btnDisclosures.TabStop = false;
      this.btnDisclosures.Text = "eDisclosures";
      this.tooltip.SetToolTip((Control) this.btnDisclosures, "Send documents to borrower to sign and request needed documents");
      this.btnDisclosures.UseVisualStyleBackColor = true;
      this.btnDisclosures.Click += new EventHandler(this.btnDisclosures_Click);
      this.btnRequest.BackColor = SystemColors.Control;
      this.btnRequest.Location = new Point(278, 0);
      this.btnRequest.Margin = new Padding(0);
      this.btnRequest.Name = "btnRequest";
      this.btnRequest.Size = new Size(61, 22);
      this.btnRequest.TabIndex = 6;
      this.btnRequest.TabStop = false;
      this.btnRequest.Text = "Request";
      this.tooltip.SetToolTip((Control) this.btnRequest, "Send documents to borrower to sign and request needed documents");
      this.btnRequest.UseVisualStyleBackColor = true;
      this.btnRequest.Click += new EventHandler(this.btnRequest_Click);
      this.btnConsent.BackColor = SystemColors.Control;
      this.btnConsent.Location = new Point(192, 0);
      this.btnConsent.Margin = new Padding(0);
      this.btnConsent.Name = "btnConsent";
      this.btnConsent.Size = new Size(86, 22);
      this.btnConsent.TabIndex = 15;
      this.btnConsent.TabStop = false;
      this.btnConsent.Text = "eConsent";
      this.tooltip.SetToolTip((Control) this.btnConsent, "Request borrower consent to receive documents electronically");
      this.btnConsent.UseVisualStyleBackColor = true;
      this.btnConsent.Click += new EventHandler(this.btnConsent_Click);
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(167, 3);
      this.btnPrint.Margin = new Padding(4, 3, 0, 3);
      this.btnPrint.MouseDownImage = (Image) null;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 6;
      this.btnPrint.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnPrint, "Print Document");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExcel.BackColor = Color.Transparent;
      this.btnExcel.Location = new Point(147, 3);
      this.btnExcel.Margin = new Padding(4, 3, 0, 3);
      this.btnExcel.MouseDownImage = (Image) null;
      this.btnExcel.Name = "btnExcel";
      this.btnExcel.Size = new Size(16, 16);
      this.btnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExcel.TabIndex = 5;
      this.btnExcel.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnExcel, "Export to Excel");
      this.btnExcel.Click += new EventHandler(this.btnExcel_Click);
      this.btnAccess.BackColor = Color.Transparent;
      this.btnAccess.DisabledImage = (Image) Resources.concurrent_editing_block_disabled;
      this.btnAccess.Image = (Image) Resources.concurrent_editing_block;
      this.btnAccess.Location = new Point((int) sbyte.MaxValue, 3);
      this.btnAccess.Margin = new Padding(4, 3, 0, 3);
      this.btnAccess.MouseDownImage = (Image) null;
      this.btnAccess.MouseOverImage = (Image) Resources.concurrent_editing_block_over;
      this.btnAccess.Name = "btnAccess";
      this.btnAccess.Size = new Size(16, 16);
      this.btnAccess.TabIndex = 13;
      this.btnAccess.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnAccess, "Set Document Access");
      this.btnAccess.Click += new EventHandler(this.btnAccess_Click);
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(107, 3);
      this.btnDelete.Margin = new Padding(4, 3, 0, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 4;
      this.btnDelete.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDelete, "Delete Document");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnEdit.BackColor = Color.Transparent;
      this.btnEdit.Location = new Point(87, 3);
      this.btnEdit.Margin = new Padding(4, 3, 0, 3);
      this.btnEdit.MouseDownImage = (Image) null;
      this.btnEdit.Name = "btnEdit";
      this.btnEdit.Size = new Size(16, 16);
      this.btnEdit.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEdit.TabIndex = 3;
      this.btnEdit.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnEdit, "Edit Document");
      this.btnEdit.Click += new EventHandler(this.btnEdit_Click);
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(67, 3);
      this.btnDuplicate.Margin = new Padding(4, 3, 0, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 2;
      this.btnDuplicate.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnDuplicate, "Duplicate Document");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(47, 3);
      this.btnNew.Margin = new Padding(4, 3, 0, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 1;
      this.btnNew.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnNew, "New Document");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnResetDocumentView.BackColor = Color.Transparent;
      this.btnResetDocumentView.Location = new Point(374, 8);
      this.btnResetDocumentView.MouseDownImage = (Image) null;
      this.btnResetDocumentView.Name = "btnResetDocumentView";
      this.btnResetDocumentView.Size = new Size(16, 16);
      this.btnResetDocumentView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnResetDocumentView.TabIndex = 7;
      this.btnResetDocumentView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnResetDocumentView, "Reset View");
      this.btnResetDocumentView.Click += new EventHandler(this.btnResetDocumentView_Click);
      this.btnManageDocumentView.BackColor = Color.Transparent;
      this.btnManageDocumentView.Location = new Point(396, 8);
      this.btnManageDocumentView.MouseDownImage = (Image) null;
      this.btnManageDocumentView.Name = "btnManageDocumentView";
      this.btnManageDocumentView.Size = new Size(16, 16);
      this.btnManageDocumentView.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnManageDocumentView.TabIndex = 6;
      this.btnManageDocumentView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnManageDocumentView, "Manage View");
      this.btnManageDocumentView.Click += new EventHandler(this.btnManageDocumentView_Click);
      this.btnSaveDocumentView.BackColor = Color.Transparent;
      this.btnSaveDocumentView.Location = new Point(352, 8);
      this.btnSaveDocumentView.MouseDownImage = (Image) null;
      this.btnSaveDocumentView.Name = "btnSaveDocumentView";
      this.btnSaveDocumentView.Size = new Size(16, 16);
      this.btnSaveDocumentView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveDocumentView.TabIndex = 5;
      this.btnSaveDocumentView.TabStop = false;
      this.tooltip.SetToolTip((Control) this.btnSaveDocumentView, "Save View");
      this.btnSaveDocumentView.Click += new EventHandler(this.btnSaveDocumentView_Click);
      this.mnuContext.Items.AddRange(new ToolStripItem[12]
      {
        (ToolStripItem) this.mnuItemNew1,
        (ToolStripItem) this.mnuItemEdit1,
        (ToolStripItem) this.mnuItemRename1,
        (ToolStripItem) this.mnuItemDuplicate1,
        (ToolStripItem) this.mnuItemDelete1,
        (ToolStripItem) this.mnuItemSeparator1,
        (ToolStripItem) this.mnuItemExcel1,
        (ToolStripItem) this.mnuItemPrint1,
        (ToolStripItem) this.mnuItemSave1,
        (ToolStripItem) this.mnuItemAccess1,
        (ToolStripItem) this.mnuItemSeparator2,
        (ToolStripItem) this.mnuItemSelectAll
      });
      this.mnuContext.Name = "mnuDocuments";
      this.mnuContext.ShowImageMargin = false;
      this.mnuContext.Size = new Size(209, 236);
      this.mnuItemNew1.Name = "mnuItemNew1";
      this.mnuItemNew1.Size = new Size(208, 22);
      this.mnuItemNew1.Text = "New Document...";
      this.mnuItemNew1.Click += new EventHandler(this.btnNew_Click);
      this.mnuItemEdit1.Name = "mnuItemEdit1";
      this.mnuItemEdit1.Size = new Size(208, 22);
      this.mnuItemEdit1.Text = "Edit Document...";
      this.mnuItemEdit1.Click += new EventHandler(this.btnEdit_Click);
      this.mnuItemRename1.Name = "mnuItemRename1";
      this.mnuItemRename1.Size = new Size(208, 22);
      this.mnuItemRename1.Text = "Rename Document...";
      this.mnuItemRename1.Click += new EventHandler(this.renameOnClick);
      this.mnuItemDuplicate1.Name = "mnuItemDuplicate1";
      this.mnuItemDuplicate1.Size = new Size(208, 22);
      this.mnuItemDuplicate1.Text = "Duplicate Document...";
      this.mnuItemDuplicate1.Click += new EventHandler(this.btnDuplicate_Click);
      this.mnuItemDelete1.Name = "mnuItemDelete1";
      this.mnuItemDelete1.Size = new Size(208, 22);
      this.mnuItemDelete1.Text = "Delete Document";
      this.mnuItemDelete1.Click += new EventHandler(this.btnDelete_Click);
      this.mnuItemSeparator1.Name = "mnuItemSeparator1";
      this.mnuItemSeparator1.Size = new Size(205, 6);
      this.mnuItemExcel1.Name = "mnuItemExcel1";
      this.mnuItemExcel1.Size = new Size(208, 22);
      this.mnuItemExcel1.Text = "Export to Excel...";
      this.mnuItemExcel1.Click += new EventHandler(this.btnExcel_Click);
      this.mnuItemPrint1.Name = "mnuItemPrint1";
      this.mnuItemPrint1.Size = new Size(208, 22);
      this.mnuItemPrint1.Text = "Print Document...";
      this.mnuItemPrint1.Click += new EventHandler(this.btnPrint_Click);
      this.mnuItemSave1.Name = "mnuItemSave1";
      this.mnuItemSave1.Size = new Size(208, 22);
      this.mnuItemSave1.Text = "Save Document As...";
      this.mnuItemSave1.Click += new EventHandler(this.mnuItemSave_Click);
      this.mnuItemAccess1.Name = "mnuItemAccess1";
      this.mnuItemAccess1.Size = new Size(208, 22);
      this.mnuItemAccess1.Text = "Set Document Access Rights...";
      this.mnuItemAccess1.Click += new EventHandler(this.btnAccess_Click);
      this.mnuItemSeparator2.Name = "mnuItemSeparator2";
      this.mnuItemSeparator2.Size = new Size(205, 6);
      this.mnuItemSelectAll.Name = "mnuItemSelectAll";
      this.mnuItemSelectAll.Size = new Size(208, 22);
      this.mnuItemSelectAll.Text = "Select All on This Page";
      this.mnuItemSelectAll.Click += new EventHandler(this.mnuItemSelectAll_Click);
      this.mnuDocuments.Items.AddRange(new ToolStripItem[18]
      {
        (ToolStripItem) this.mnuItemNew2,
        (ToolStripItem) this.mnuItemDuplicate2,
        (ToolStripItem) this.mnuItemEdit2,
        (ToolStripItem) this.mnuItemRename2,
        (ToolStripItem) this.mnuItemDelete2,
        (ToolStripItem) this.mnuItemSeparator3,
        (ToolStripItem) this.mnuItemAccess2,
        (ToolStripItem) this.mnuItemExcel2,
        (ToolStripItem) this.mnuItemPrint2,
        (ToolStripItem) this.mnuItemSave2,
        (ToolStripItem) this.mnuItemSeparator4,
        (ToolStripItem) this.mnuItemRequest,
        (ToolStripItem) this.mnuItemDisclosures,
        (ToolStripItem) this.mnuItemRetrieve,
        (ToolStripItem) this.mnuItemFiles,
        (ToolStripItem) this.mnuItemSend,
        (ToolStripItem) this.mnuItemSeparator5,
        (ToolStripItem) this.mnuItemCoversheet
      });
      this.mnuDocuments.Name = "mnuDocuments";
      this.mnuDocuments.Size = new Size(234, 374);
      this.mnuItemNew2.Image = (Image) Resources._new;
      this.mnuItemNew2.Name = "mnuItemNew2";
      this.mnuItemNew2.ShortcutKeys = Keys.N | Keys.Control;
      this.mnuItemNew2.Size = new Size(233, 22);
      this.mnuItemNew2.Text = "&New Document...";
      this.mnuItemNew2.Click += new EventHandler(this.btnNew_Click);
      this.mnuItemDuplicate2.Image = (Image) Resources.duplicate;
      this.mnuItemDuplicate2.Name = "mnuItemDuplicate2";
      this.mnuItemDuplicate2.Size = new Size(233, 22);
      this.mnuItemDuplicate2.Text = "D&uplicate Document...";
      this.mnuItemDuplicate2.Click += new EventHandler(this.btnDuplicate_Click);
      this.mnuItemEdit2.Image = (Image) Resources.edit;
      this.mnuItemEdit2.Name = "mnuItemEdit2";
      this.mnuItemEdit2.Size = new Size(233, 22);
      this.mnuItemEdit2.Text = "&Edit Document...";
      this.mnuItemEdit2.Click += new EventHandler(this.btnEdit_Click);
      this.mnuItemRename2.Image = (Image) Resources.edit;
      this.mnuItemRename2.Name = "mnuItemRename2";
      this.mnuItemRename2.ShortcutKeys = Keys.N | Keys.Control;
      this.mnuItemRename2.Size = new Size(233, 22);
      this.mnuItemRename2.Text = "&Rename Document...";
      this.mnuItemRename2.Click += new EventHandler(this.renameOnClick);
      this.mnuItemDelete2.Image = (Image) Resources.delete;
      this.mnuItemDelete2.Name = "mnuItemDelete2";
      this.mnuItemDelete2.ShortcutKeyDisplayString = "";
      this.mnuItemDelete2.ShortcutKeys = Keys.D | Keys.Alt;
      this.mnuItemDelete2.Size = new Size(233, 22);
      this.mnuItemDelete2.Text = "&Delete Document";
      this.mnuItemDelete2.Click += new EventHandler(this.btnDelete_Click);
      this.mnuItemSeparator3.Name = "mnuItemSeparator3";
      this.mnuItemSeparator3.Size = new Size(230, 6);
      this.mnuItemAccess2.Image = (Image) Resources.concurrent_editing_block;
      this.mnuItemAccess2.Name = "mnuItemAccess2";
      this.mnuItemAccess2.Size = new Size(233, 22);
      this.mnuItemAccess2.Text = "Set Document &Access Rights...";
      this.mnuItemAccess2.Click += new EventHandler(this.btnAccess_Click);
      this.mnuItemExcel2.Image = (Image) Resources.excel;
      this.mnuItemExcel2.Name = "mnuItemExcel2";
      this.mnuItemExcel2.Size = new Size(233, 22);
      this.mnuItemExcel2.Text = "E&xport to Excel...";
      this.mnuItemExcel2.Click += new EventHandler(this.btnExcel_Click);
      this.mnuItemPrint2.Image = (Image) Resources.print;
      this.mnuItemPrint2.Name = "mnuItemPrint2";
      this.mnuItemPrint2.ShortcutKeys = Keys.P | Keys.Control;
      this.mnuItemPrint2.Size = new Size(233, 22);
      this.mnuItemPrint2.Text = "&Print Document...";
      this.mnuItemPrint2.Click += new EventHandler(this.btnPrint_Click);
      this.mnuItemSave2.Name = "mnuItemSave2";
      this.mnuItemSave2.Size = new Size(233, 22);
      this.mnuItemSave2.Text = "Save D&ocument As...";
      this.mnuItemSave2.Click += new EventHandler(this.mnuItemSave_Click);
      this.mnuItemSeparator4.Name = "mnuItemSeparator4";
      this.mnuItemSeparator4.Size = new Size(230, 6);
      this.mnuItemRequest.Name = "mnuItemRequest";
      this.mnuItemRequest.Size = new Size(233, 22);
      this.mnuItemRequest.Text = "&Request Documents...";
      this.mnuItemRequest.Click += new EventHandler(this.btnRequest_Click);
      this.mnuItemDisclosures.Name = "mnuItemDisclosures";
      this.mnuItemDisclosures.Size = new Size(233, 22);
      this.mnuItemDisclosures.Text = "eD&isclosures...";
      this.mnuItemDisclosures.Click += new EventHandler(this.btnDisclosures_Click);
      this.mnuItemRetrieve.Name = "mnuItemRetrieve";
      this.mnuItemRetrieve.Size = new Size(233, 22);
      this.mnuItemRetrieve.Text = "Retrie&ve Documents...";
      this.mnuItemRetrieve.Click += new EventHandler(this.btnRetrieve_Click);
      this.mnuItemFiles.Name = "mnuItemFiles";
      this.mnuItemFiles.Size = new Size(233, 22);
      this.mnuItemFiles.Text = "File &Manager...";
      this.mnuItemFiles.Click += new EventHandler(this.btnFiles_Click);
      this.mnuItemSend.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.mnuItemSendFiles1,
        (ToolStripItem) this.mnuItemSendLender1
      });
      this.mnuItemSend.Name = "mnuItemSend";
      this.mnuItemSend.Size = new Size(233, 22);
      this.mnuItemSend.Text = "&Send";
      this.mnuItemSendFiles1.Name = "mnuItemSendFiles1";
      this.mnuItemSendFiles1.Size = new Size(188, 22);
      this.mnuItemSendFiles1.Text = "&Send Files...";
      this.mnuItemSendFiles1.Click += new EventHandler(this.mnuItemSendFiles_Click);
      this.mnuItemSendLender1.Name = "mnuItemSendLender1";
      this.mnuItemSendLender1.Size = new Size(188, 22);
      this.mnuItemSendLender1.Text = "Send Files to &Lender...";
      this.mnuItemSendLender1.Click += new EventHandler(this.mnuItemSendLender_Click);
      this.mnuItemSeparator5.Name = "mnuItemSeparator5";
      this.mnuItemSeparator5.Size = new Size(230, 6);
      this.mnuItemCoversheet.Name = "mnuItemCoversheet";
      this.mnuItemCoversheet.Size = new Size(233, 22);
      this.mnuItemCoversheet.Text = "Print &Fax Coversheet...";
      this.mnuItemCoversheet.Click += new EventHandler(this.mnuItemCoversheet_Click);
      this.gcDocuments.Controls.Add((Control) this.btnAIQ);
      this.gcDocuments.Controls.Add((Control) this.gvDocuments);
      this.gcDocuments.Controls.Add((Control) this.pnlToolbar);
      this.gcDocuments.Dock = DockStyle.Fill;
      this.gcDocuments.HeaderForeColor = SystemColors.ControlText;
      this.gcDocuments.Location = new Point(0, 62);
      this.gcDocuments.Name = "gcDocuments";
      this.gcDocuments.Size = new Size(895, 262);
      this.gcDocuments.TabIndex = 3;
      this.gcDocuments.Text = "Documents";
      this.btnAIQ.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAIQ.Location = new Point(99, 2);
      this.btnAIQ.Name = "btnAIQ";
      this.btnAIQ.Size = new Size(56, 23);
      this.btnAIQ.TabIndex = 15;
      this.btnAIQ.Text = "DDA";
      this.btnAIQ.UseVisualStyleBackColor = true;
      this.btnAIQ.Visible = false;
      this.btnAIQ.Click += new EventHandler(this.btnAIQ_Click);
      this.gvDocuments.AllowColumnReorder = true;
      this.gvDocuments.BorderStyle = BorderStyle.None;
      this.gvDocuments.ContextMenuStrip = this.mnuContext;
      this.gvDocuments.Dock = DockStyle.Fill;
      this.gvDocuments.FilterVisible = true;
      this.gvDocuments.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvDocuments.Location = new Point(1, 26);
      this.gvDocuments.Name = "gvDocuments";
      this.gvDocuments.Size = new Size(893, 235);
      this.gvDocuments.SortingType = SortingType.AlphaNumeric;
      this.gvDocuments.TabIndex = 14;
      this.gvDocuments.TextTrimming = StringTrimming.EllipsisCharacter;
      this.gvDocuments.SelectedIndexChanged += new EventHandler(this.gvDocuments_SelectedIndexChanged);
      this.gvDocuments.SortItems += new GVColumnSortEventHandler(this.gvDocuments_SortItems);
      this.gvDocuments.ItemDoubleClick += new GVItemEventHandler(this.gvDocuments_ItemDoubleClick);
      this.gvDocuments.EditorOpening += new GVSubItemEditingEventHandler(this.gvDocuments_EditorOpening);
      this.gvDocuments.EditorClosing += new GVSubItemEditingEventHandler(this.gvDocuments_EditorClosing);
      this.pnlToolbar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.pnlToolbar.BackColor = Color.Transparent;
      this.pnlToolbar.Controls.Add((Control) this.btnSend);
      this.pnlToolbar.Controls.Add((Control) this.btnFiles);
      this.pnlToolbar.Controls.Add((Control) this.btnDocuments);
      this.pnlToolbar.Controls.Add((Control) this.btnRetrieve);
      this.pnlToolbar.Controls.Add((Control) this.btnDisclosures);
      this.pnlToolbar.Controls.Add((Control) this.btnRequest);
      this.pnlToolbar.Controls.Add((Control) this.btnConsent);
      this.pnlToolbar.Controls.Add((Control) this.separator1);
      this.pnlToolbar.Controls.Add((Control) this.btnPrint);
      this.pnlToolbar.Controls.Add((Control) this.btnExcel);
      this.pnlToolbar.Controls.Add((Control) this.btnAccess);
      this.pnlToolbar.Controls.Add((Control) this.btnDelete);
      this.pnlToolbar.Controls.Add((Control) this.btnEdit);
      this.pnlToolbar.Controls.Add((Control) this.btnDuplicate);
      this.pnlToolbar.Controls.Add((Control) this.btnNew);
      this.pnlToolbar.FlowDirection = FlowDirection.RightToLeft;
      this.pnlToolbar.Location = new Point(161, 2);
      this.pnlToolbar.Name = "pnlToolbar";
      this.pnlToolbar.Size = new Size(730, 22);
      this.pnlToolbar.TabIndex = 4;
      this.separator1.Location = new Point(187, 3);
      this.separator1.Margin = new Padding(4, 3, 3, 3);
      this.separator1.MaximumSize = new Size(2, 16);
      this.separator1.MinimumSize = new Size(2, 16);
      this.separator1.Name = "separator1";
      this.separator1.Size = new Size(2, 16);
      this.separator1.TabIndex = 5;
      this.separator1.TabStop = false;
      this.gradStackingOrder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradStackingOrder.Controls.Add((Control) this.cboDocumentGroup);
      this.gradStackingOrder.Controls.Add((Control) this.lblDocumentGroup);
      this.gradStackingOrder.Controls.Add((Control) this.cboStackingOrder);
      this.gradStackingOrder.Controls.Add((Control) this.lblStackingOrder);
      this.gradStackingOrder.Dock = DockStyle.Top;
      this.gradStackingOrder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradStackingOrder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradStackingOrder.Location = new Point(0, 31);
      this.gradStackingOrder.Name = "gradStackingOrder";
      this.gradStackingOrder.Padding = new Padding(8, 0, 0, 0);
      this.gradStackingOrder.Size = new Size(895, 31);
      this.gradStackingOrder.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradStackingOrder.TabIndex = 13;
      this.cboDocumentGroup.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocumentGroup.FormattingEnabled = true;
      this.cboDocumentGroup.Location = new Point(99, 5);
      this.cboDocumentGroup.Name = "cboDocumentGroup";
      this.cboDocumentGroup.Size = new Size(351, 22);
      this.cboDocumentGroup.TabIndex = 15;
      this.cboDocumentGroup.TabStop = false;
      this.cboDocumentGroup.SelectionChangeCommitted += new EventHandler(this.cboDocumentGroup_SelectionChangeCommitted);
      this.lblDocumentGroup.AutoSize = true;
      this.lblDocumentGroup.BackColor = Color.Transparent;
      this.lblDocumentGroup.Location = new Point(8, 9);
      this.lblDocumentGroup.Name = "lblDocumentGroup";
      this.lblDocumentGroup.Size = new Size(88, 14);
      this.lblDocumentGroup.TabIndex = 14;
      this.lblDocumentGroup.Text = "Document Group";
      this.lblDocumentGroup.TextAlign = ContentAlignment.MiddleLeft;
      this.cboStackingOrder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboStackingOrder.FormattingEnabled = true;
      this.cboStackingOrder.Location = new Point(540, 5);
      this.cboStackingOrder.Name = "cboStackingOrder";
      this.cboStackingOrder.Size = new Size(351, 22);
      this.cboStackingOrder.TabIndex = 13;
      this.cboStackingOrder.TabStop = false;
      this.cboStackingOrder.DropDown += new EventHandler(this.cboStackingOrder_DropDown);
      this.cboStackingOrder.SelectedIndexChanged += new EventHandler(this.cboStackingOrder_SelectedIndexChanged);
      this.lblStackingOrder.AutoSize = true;
      this.lblStackingOrder.BackColor = Color.Transparent;
      this.lblStackingOrder.Location = new Point(456, 9);
      this.lblStackingOrder.Name = "lblStackingOrder";
      this.lblStackingOrder.Size = new Size(79, 14);
      this.lblStackingOrder.TabIndex = 12;
      this.lblStackingOrder.Text = "Stacking Order";
      this.lblStackingOrder.TextAlign = ContentAlignment.MiddleLeft;
      this.gradDocumentsView.BackColorGlassyStyle = true;
      this.gradDocumentsView.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradDocumentsView.Controls.Add((Control) this.btnResetDocumentView);
      this.gradDocumentsView.Controls.Add((Control) this.btnManageDocumentView);
      this.gradDocumentsView.Controls.Add((Control) this.btnSaveDocumentView);
      this.gradDocumentsView.Controls.Add((Control) this.cboDocumentsView);
      this.gradDocumentsView.Controls.Add((Control) this.lblDocumentsView);
      this.gradDocumentsView.Dock = DockStyle.Top;
      this.gradDocumentsView.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradDocumentsView.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradDocumentsView.Location = new Point(0, 0);
      this.gradDocumentsView.Name = "gradDocumentsView";
      this.gradDocumentsView.Size = new Size(895, 31);
      this.gradDocumentsView.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradDocumentsView.TabIndex = 14;
      this.cboDocumentsView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboDocumentsView.FormattingEnabled = true;
      this.cboDocumentsView.Location = new Point(123, 5);
      this.cboDocumentsView.Name = "cboDocumentsView";
      this.cboDocumentsView.Size = new Size(219, 21);
      this.cboDocumentsView.TabIndex = 2;
      this.cboDocumentsView.SelectionChangeCommitted += new EventHandler(this.cboDocumentsView_SelectionChangeCommitted);
      this.lblDocumentsView.AutoSize = true;
      this.lblDocumentsView.BackColor = Color.Transparent;
      this.lblDocumentsView.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblDocumentsView.Location = new Point(6, 7);
      this.lblDocumentsView.Name = "lblDocumentsView";
      this.lblDocumentsView.Size = new Size(114, 16);
      this.lblDocumentsView.TabIndex = 1;
      this.lblDocumentsView.Text = "Documents View";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.gcDocuments);
      this.Controls.Add((Control) this.gradStackingOrder);
      this.Controls.Add((Control) this.gradDocumentsView);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (DocumentTrackingControl);
      this.Size = new Size(895, 324);
      this.mnuSend.ResumeLayout(false);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnExcel).EndInit();
      ((ISupportInitialize) this.btnAccess).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnEdit).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnResetDocumentView).EndInit();
      ((ISupportInitialize) this.btnManageDocumentView).EndInit();
      ((ISupportInitialize) this.btnSaveDocumentView).EndInit();
      this.mnuContext.ResumeLayout(false);
      this.mnuDocuments.ResumeLayout(false);
      this.gcDocuments.ResumeLayout(false);
      this.pnlToolbar.ResumeLayout(false);
      this.gradStackingOrder.ResumeLayout(false);
      this.gradStackingOrder.PerformLayout();
      this.gradDocumentsView.ResumeLayout(false);
      this.gradDocumentsView.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
