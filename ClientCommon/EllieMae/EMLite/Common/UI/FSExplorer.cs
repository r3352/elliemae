// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.FSExplorer
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.RemotingServices.Acl;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class FSExplorer : UserControl
  {
    private Sessions.Session session;
    private string title = "";
    private Hashtable buttonMenuItemMap = new Hashtable();
    private int maxFileNameLength = 260;
    private FSExplorer.TemplateSettingsType templateType;
    private FSExplorer.DialogMode dialogMode;
    private bool displayFoldersOnly;
    private bool menuItemImportVisible;
    private bool disableAllMenuItemsExceptRefresh;
    private bool displayExtension = true;
    private FSExplorer.RootType currRoot = FSExplorer.RootType.PublicOnly;
    private bool hasPublicRight = true;
    private FileSystemEntry _currentFolder = FileSystemEntry.PublicRoot;
    private FileSystemEntry[] currFSEntries = new FileSystemEntry[0];
    private IFSExplorerBase _ifsExplorer;
    private bool displayFolderOperationButtonsOnly;
    private FSExplorer.FileTypes fileType;
    private int rightMouseX;
    private int rightMouseY;
    private bool isCut;
    private FSExplorer.RootType cutCopyRoot;
    private FileSystemEntry cutCopyFolder;
    private FileSystemEntry[] cutCopyEntries;
    private string preSelected = string.Empty;
    private string preDescription = string.Empty;
    private FileSystemEntry[] entries;
    private EllieMae.EMLite.ContactUI.ContactType contactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
    private ToolTip tipFSExplorer;
    private bool canCreateEdit = true;
    private TextBoxFormatter editorFormatter;
    private IContainer components;
    private GroupContainer gcListView;
    private StandardIconButton btnAdd;
    private StandardIconButton btnDelete;
    private StandardIconButton btnNewFolder;
    private StandardIconButton btnOpen;
    private StandardIconButton btnPaste;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnCopy;
    private StandardIconButton btnCut;
    private VerticalSeparator verticalSeparator1;
    private Button btnExport;
    private Button btnDeploy;
    private Button btnRename;
    private Button btnImport;
    private StandardIconButton stdIconBtnUpFolder;
    private Label lblFolder;
    private ComboBoxEx cmbBoxFolder;
    private ContextMenu ctxMenuLstView1;
    private MenuItem menuItemDeploy;
    private MenuItem menuItemExport;
    private MenuItem menuItemPaste;
    private MenuItem menuItemRefresh;
    private MenuItem menuItemOpen;
    private MenuItem menuItemCut;
    private MenuItem menuItemCopy;
    private MenuItem menuItemDelete;
    private MenuItem menuItemRename;
    private MenuItem menuItemCreateFolder;
    private MenuItem menuItemNew;
    private MenuItem menuItemImport;
    private MenuItem menuItemDuplicate;
    private MenuItem menuItemSetAsDefault;
    private GridView gvDirectory;
    private GVColumn nameHeader;
    private GVColumn descHeader;
    private ImageList imgsListView;
    private Button btnSetAsDefault;
    private GradientPanel pnlExPurchaseAdvice;
    private Label lblAdditionalDescription;
    private GradientPanel pnlFolder;
    public bool FileActionConfirmed;
    private bool hideAllButtons;
    private FSExplorer.RESPAFilter respaMode;
    private Hashtable userFeatureRights;

    public event EventHandler SetAsDefaultButtonClick;

    public event EventHandler FileChanged;

    public event EventHandler FolderChanged;

    public event EventHandler SelectedEntryChanged;

    public event EventHandler SelectedCurrentFile;

    public event EventHandler BeforeFileDeleted;

    public event EventHandler BeforeFolderRenamed;

    public event EventHandler BeforeFileRenamed;

    public event EventHandler AfterFileRenamed;

    public event EventHandler AfterFolderRenamed;

    public event EventHandler AfterFolderDeleted;

    public event EventHandler EntryDoubleClicked;

    public event EventHandler AfterFileDeleted;

    public event EventHandler BeforeFileMoved;

    public event EventHandler AfterFileMoved;

    public event EventHandler AfterFileCopied;

    public event EventHandler RefreshedClicked;

    private FileSystemEntry currentFolder
    {
      get => this._currentFolder;
      set
      {
        this._currentFolder = value;
        if (this.currRoot == FSExplorer.RootType.Top || FSExplorer.RootType.PublicOnly == this.currRoot && "\\" == this._currentFolder.Path || FSExplorer.RootType.Public == this.currRoot && "\\" == this._currentFolder.Path || FSExplorer.RootType.Private == this.currRoot && "\\" == this._currentFolder.Path)
          this.stdIconBtnUpFolder.Enabled = false;
        else
          this.stdIconBtnUpFolder.Enabled = true;
      }
    }

    public FSExplorer()
      : this(Session.DefaultInstance)
    {
    }

    public FSExplorer(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.init();
    }

    public Sessions.Session GetSession() => this.session;

    public void SetSession(Sessions.Session session)
    {
      this.session = session;
      this.init();
      this.HasPublicRight = true;
    }

    private void init()
    {
      if (this.session == null || !this.session.IsConnected)
        return;
      this.nameHeader = this.gvDirectory.Columns[0];
      this.descHeader = this.gvDirectory.Columns[1];
      this.buttonMenuItemMap.Add((object) this.btnAdd, (object) this.menuItemNew);
      this.buttonMenuItemMap.Add((object) this.btnCut, (object) this.menuItemCut);
      this.buttonMenuItemMap.Add((object) this.btnCopy, (object) this.menuItemCopy);
      this.buttonMenuItemMap.Add((object) this.btnDuplicate, (object) this.menuItemDuplicate);
      this.buttonMenuItemMap.Add((object) this.btnPaste, (object) this.menuItemPaste);
      this.buttonMenuItemMap.Add((object) this.btnOpen, (object) this.menuItemOpen);
      this.buttonMenuItemMap.Add((object) this.btnNewFolder, (object) this.menuItemCreateFolder);
      this.buttonMenuItemMap.Add((object) this.btnDelete, (object) this.menuItemDelete);
      this.buttonMenuItemMap.Add((object) this.btnRename, (object) this.menuItemRename);
      this.buttonMenuItemMap.Add((object) this.btnDeploy, (object) this.menuItemDeploy);
      this.buttonMenuItemMap.Add((object) this.btnImport, (object) this.menuItemImport);
      this.buttonMenuItemMap.Add((object) this.btnExport, (object) this.menuItemExport);
      this.buttonMenuItemMap.Add((object) this.btnSetAsDefault, (object) this.menuItemSetAsDefault);
    }

    private void gvDirectory_SortItems(object source, GVColumnSortEventArgs e)
    {
      GVColumnSort[] sortOrder = this.gvDirectory.Columns.GetSortOrder();
      SortOrder order = SortOrder.Ascending;
      if (sortOrder != null && sortOrder.Length != 0 && sortOrder[0].Column == e.Column)
        order = this.gvDirectory.Columns[e.Column].SortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
      this.gvDirectory.SortOption = GVSortOption.Auto;
      this.gvDirectory.Sort(e.Column, order);
      if (e.Column == 0)
      {
        List<GVItem> gvItemList1 = new List<GVItem>();
        List<GVItem> gvItemList2 = new List<GVItem>();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
        {
          if (gvItem.Tag.GetType() == typeof (FileSystemEntry))
          {
            FileSystemEntry tag = (FileSystemEntry) gvItem.Tag;
            if (tag.Type == FileSystemEntry.Types.Folder)
              gvItemList1.Add(gvItem);
            else if (tag.Type == FileSystemEntry.Types.File)
              gvItemList2.Add(gvItem);
          }
          else if (gvItem.Tag.GetType() == typeof (MilestoneTemplate) || gvItem.Tag.GetType() == typeof (SyncTemplate))
            gvItemList2.Add(gvItem);
        }
        this.gvDirectory.SortOption = GVSortOption.None;
        this.gvDirectory.Items.Clear();
        this.gvDirectory.BeginUpdate();
        if (order == SortOrder.Ascending)
        {
          foreach (GVItem gvItem in gvItemList1)
            this.gvDirectory.Items.Add(gvItem);
          foreach (GVItem gvItem in gvItemList2)
            this.gvDirectory.Items.Add(gvItem);
        }
        else
        {
          foreach (GVItem gvItem in gvItemList2)
            this.gvDirectory.Items.Add(gvItem);
          foreach (GVItem gvItem in gvItemList1)
            this.gvDirectory.Items.Add(gvItem);
        }
        this.gvDirectory.EndUpdate();
      }
      this.gvDirectory.SortOption = GVSortOption.Owner;
    }

    private void setTitle()
    {
      this.gcListView.Text = this.title;
      if (this.fileType == FSExplorer.FileTypes.PurchaseAdvice)
        return;
      GroupContainer gcListView = this.gcListView;
      gcListView.Text = gcListView.Text + " (" + (object) this.gvDirectory.Items.Count + ")";
    }

    public FSExplorer.FileTypes FileType
    {
      set
      {
        this.fileType = value;
        switch (this.fileType)
        {
          case FSExplorer.FileTypes.PrintGroups:
            this.title = "Print Form Groups";
            break;
          case FSExplorer.FileTypes.CustomForms:
            this.title = "Custom Print Forms";
            break;
          case FSExplorer.FileTypes.CustomLetters:
            this.title = "Custom Letters";
            break;
          case FSExplorer.FileTypes.Reports:
            this.title = "Reports";
            break;
          case FSExplorer.FileTypes.LoanTemplates:
            this.title = "Loan Template Sets";
            break;
          case FSExplorer.FileTypes.DataTemplates:
            this.title = "Data Templates";
            break;
          case FSExplorer.FileTypes.LoanPrograms:
            this.title = "Loan Programs";
            break;
          case FSExplorer.FileTypes.ClosingCosts:
            this.title = "Closing Costs";
            break;
          case FSExplorer.FileTypes.DocumentSets:
            this.title = "Document Sets";
            break;
          case FSExplorer.FileTypes.FormLists:
            this.title = "Input Form Sets";
            break;
          case FSExplorer.FileTypes.StackingOrderSets:
            this.title = "Stacking Templates";
            break;
          case FSExplorer.FileTypes.CampaignTemplates:
            this.title = "Campaign Templates";
            break;
          case FSExplorer.FileTypes.PurchaseAdvice:
            this.title = "Purchase Advice Template";
            break;
          case FSExplorer.FileTypes.FundingTemplate:
            this.title = "Funding Templates";
            this.lblAdditionalDescription.Text = "Create and edit templates of the fees typically deducted on the Funding Worksheet.";
            this.lblAdditionalDescription.Left = 6;
            this.lblAdditionalDescription.Top = 6;
            break;
          case FSExplorer.FileTypes.DashboardTemplate:
            this.title = "Snapshot";
            break;
          case FSExplorer.FileTypes.DashboardViewTemplate:
            this.title = "Views";
            break;
          case FSExplorer.FileTypes.TaskSets:
            this.title = "Task Sets";
            break;
          case FSExplorer.FileTypes.ConditionalLetter:
            this.title = "Condition Forms";
            break;
          case FSExplorer.FileTypes.SettlementServiceProviders:
            this.title = "Settlement Service Providers";
            break;
          case FSExplorer.FileTypes.LoanDuplicationTemplate:
            this.title = "Loan Duplication Template";
            break;
          case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
            this.title = "Affiliates";
            break;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public bool HideAllButtons
    {
      set
      {
        this.hideAllButtons = value;
        this.initializeButtons();
      }
    }

    private void initializeButtons()
    {
      this.btnSetAsDefault.Visible = false;
      this.btnExport.Visible = false;
      this.btnImport.Visible = false;
      this.btnDeploy.Visible = false;
      this.btnRename.Visible = false;
      this.verticalSeparator1.Visible = false;
      this.btnAdd.Visible = false;
      this.btnCut.Visible = false;
      this.btnCopy.Visible = false;
      this.btnDuplicate.Visible = false;
      this.btnPaste.Visible = false;
      this.btnOpen.Visible = false;
      this.btnNewFolder.Visible = false;
      this.btnDelete.Visible = false;
      this.menuItemSetAsDefault.Visible = false;
      this.menuItemExport.Visible = false;
      this.menuItemImport.Visible = false;
      this.menuItemDeploy.Visible = false;
      this.menuItemRename.Visible = false;
      this.menuItemDelete.Visible = false;
      this.menuItemCreateFolder.Visible = false;
      this.menuItemOpen.Visible = false;
      this.menuItemPaste.Visible = false;
      this.menuItemDuplicate.Visible = false;
      this.menuItemCopy.Visible = false;
      this.menuItemCut.Visible = false;
      this.menuItemNew.Visible = false;
    }

    private void arrangeButtons(params object[] buttons)
    {
      this.initializeButtons();
      if (this.hideAllButtons)
        return;
      int num = this.gcListView.Width - (buttons[buttons.Length - 1] is Button ? 3 : 1);
      for (int index = buttons.Length - 1; index >= 0; --index)
      {
        if (!(buttons[index] is VerticalSeparator) && this.buttonMenuItemMap[buttons[index]] != null && this.buttonMenuItemMap[buttons[index]] is MenuItem)
          ((MenuItem) this.buttonMenuItemMap[buttons[index]]).Visible = true;
        if (buttons[index] is Button)
        {
          Button button = buttons[index] as Button;
          button.Location = new Point(num - 2 - button.Width, 2);
          num = button.Location.X;
          button.Visible = true;
        }
        else if (buttons[index] is StandardIconButton)
        {
          StandardIconButton button = buttons[index] as StandardIconButton;
          button.Location = new Point(num - 4 - button.Width, 5);
          num = button.Location.X;
          button.Visible = true;
        }
        else if (buttons[index] is VerticalSeparator)
        {
          VerticalSeparator button = buttons[index] as VerticalSeparator;
          button.Location = new Point(num - 4 - button.Width, 5);
          num = button.Location.X;
          button.Visible = true;
        }
      }
    }

    private void suspendFolderComboIdxChangedEvent()
    {
      this.cmbBoxFolder.SelectedIndexChanged -= new EventHandler(this.cmbBoxFolder_SelectedIndexChanged);
    }

    private void resumeFolderComboIdxChangedEvent()
    {
      this.cmbBoxFolder.SelectedIndexChanged -= new EventHandler(this.cmbBoxFolder_SelectedIndexChanged);
      this.cmbBoxFolder.SelectedIndexChanged += new EventHandler(this.cmbBoxFolder_SelectedIndexChanged);
    }

    public void SetCustomLetterProperties(FSExplorer.DialogMode dialogMode)
    {
      this.displayFoldersOnly = false;
      this.fileType = FSExplorer.FileTypes.CustomLetters;
      this.templateType = FSExplorer.TemplateSettingsType.Nothing;
      this.displayExtension = true;
      switch (dialogMode)
      {
        case FSExplorer.DialogMode.SelectFiles:
          throw new NotImplementedException();
        case FSExplorer.DialogMode.SaveFiles:
          this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnImport);
          break;
        case FSExplorer.DialogMode.ManageFiles:
          throw new NotImplementedException();
      }
      this.gvDirectory.Sort(0, SortOrder.Ascending);
      this.setTitle();
    }

    public void SetCampaignTemplateProperties(FSExplorer.DialogMode dialogMode)
    {
      this.FileType = FSExplorer.FileTypes.CampaignTemplates;
      this.displayFoldersOnly = false;
      this.fileType = FSExplorer.FileTypes.CampaignTemplates;
      this.templateType = FSExplorer.TemplateSettingsType.Campaign;
      this.displayExtension = true;
      switch (dialogMode)
      {
        case FSExplorer.DialogMode.SelectFiles:
          this.disableAllMenuItemsExceptRefresh = true;
          this.initializeButtons();
          break;
        case FSExplorer.DialogMode.SaveFiles:
          this.arrangeButtons((object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnExport);
          this.displayFolderOperationButtonsOnly = true;
          break;
        case FSExplorer.DialogMode.ManageFiles:
          if (this.hasCampaignTemplatePublicRight())
          {
            this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnImport, (object) this.btnExport, (object) this.btnDeploy);
            this.btnDeploy.Enabled = this.menuItemDeploy.Enabled = false;
          }
          else
            this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnImport, (object) this.btnExport);
          this.btnImport.Enabled = this.menuItemImport.Enabled = false;
          this.btnImport.Visible = this.menuItemImportVisible = true;
          this.btnExport.Enabled = this.menuItemExport.Enabled = false;
          break;
      }
      this.gvDirectory.Sort(0, SortOrder.Ascending);
      this.setTitle();
    }

    public FSExplorer.RESPAFilter RESPAMode
    {
      set => this.respaMode = value;
      get => this.respaMode;
    }

    public void AddNewHUDColumn()
    {
      this.gvDirectory.Columns.Insert(1, new GVColumn("newHUD")
      {
        Text = this.fileType == FSExplorer.FileTypes.DataTemplates ? "RESPA-TILA Form Version" : (this.fileType == FSExplorer.FileTypes.FundingTemplate ? "2010 Itemization" : "2015 Itemization"),
        Width = this.fileType == FSExplorer.FileTypes.DataTemplates ? 150 : 110,
        TextAlign = HorizontalAlignment.Center
      });
      this.descHeader = this.gvDirectory.Columns[2];
      this.nameHeader = this.gvDirectory.Columns[0];
    }

    public void AddCustomFormIntendedForColumn()
    {
      this.gvDirectory.Columns.Insert(1, new GVColumn("intendedFor")
      {
        Text = "Intended For",
        Width = 110,
        TextAlign = HorizontalAlignment.Left
      });
      this.descHeader = this.gvDirectory.Columns[2];
      this.nameHeader = this.gvDirectory.Columns[0];
    }

    public void PopulateMilestoneTemplate(IEnumerable<MilestoneTemplate> templateList)
    {
      List<MilestoneTemplate> milestoneTemplateList = new List<MilestoneTemplate>();
      foreach (MilestoneTemplate template in templateList)
        milestoneTemplateList.Add(template);
      this.pnlFolder.Hide();
      this.pnlExPurchaseAdvice.Hide();
      this.gcListView.Text = "Milestones (" + (object) milestoneTemplateList.Count + ")";
      milestoneTemplateList.ForEach((Action<MilestoneTemplate>) (item => this.gvDirectory.Items.Add(new GVItem()
      {
        Tag = (object) item,
        SubItems = {
          (object) item.Name
        }
      })));
    }

    public void PopulateSyncTemplate(List<SyncTemplate> syncTemplates)
    {
      this.pnlFolder.Hide();
      this.pnlExPurchaseAdvice.Hide();
      this.gcListView.Text = "Templates (" + (object) (syncTemplates != null ? syncTemplates.Count : 0) + ")";
      this.gvDirectory.Items.Clear();
      if (syncTemplates == null)
        return;
      this.gvDirectory.BeginUpdate();
      foreach (SyncTemplate syncTemplate in syncTemplates)
        this.gvDirectory.Items.Add(new GVItem()
        {
          SubItems = {
            (object) syncTemplate.TemplateName,
            (object) syncTemplate.TemplateDescription
          },
          Tag = (object) syncTemplate
        });
      this.gvDirectory.EndUpdate();
    }

    public void SetDashboardViewTemplateProperties(FSExplorer.DialogMode dialogMode)
    {
      this.FileType = FSExplorer.FileTypes.DashboardViewTemplate;
      this.templateType = FSExplorer.TemplateSettingsType.DashboardViewTemplate;
      this.dialogMode = dialogMode;
      this.maxFileNameLength = 50;
      if (this.gvDirectory.Columns.Contains(this.descHeader))
        this.gvDirectory.Columns.Remove(this.descHeader);
      this.gvDirectory.Columns[0].SpringToFit = true;
      this.btnRename.Visible = false;
      this.menuItemRename.Visible = false;
      this.btnCut.Visible = false;
      this.menuItemCut.Visible = false;
      this.btnCopy.Visible = false;
      this.menuItemCopy.Visible = false;
      this.btnPaste.Visible = false;
      this.menuItemPaste.Visible = false;
      this.displayFoldersOnly = false;
      this.menuItemImportVisible = false;
      this.displayExtension = true;
      this.SingleSelection = true;
      this.initializeButtons();
      switch (dialogMode)
      {
        case FSExplorer.DialogMode.SelectFiles:
          this.initializeButtons();
          this.disableAllMenuItemsExceptRefresh = true;
          break;
        case FSExplorer.DialogMode.SaveFiles:
          this.arrangeButtons((object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnNewFolder, (object) this.verticalSeparator1, (object) this.btnRename);
          this.displayFolderOperationButtonsOnly = true;
          break;
        case FSExplorer.DialogMode.ManageFiles:
          this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.btnSetAsDefault);
          this.menuItemCut.Visible = true;
          this.menuItemCopy.Visible = true;
          this.menuItemPaste.Visible = true;
          this.menuItemRename.Visible = true;
          this.menuItemSetAsDefault.Visible = true;
          break;
      }
      this.gvDirectory.Sort(0, SortOrder.Ascending);
      this.setTitle();
    }

    public void SetDashboardTemplateProperties(FSExplorer.DialogMode dialogMode)
    {
      this.FileType = FSExplorer.FileTypes.DashboardTemplate;
      this.templateType = FSExplorer.TemplateSettingsType.DashboardTemplate;
      this.dialogMode = dialogMode;
      this.maxFileNameLength = 50;
      if (this.gvDirectory.Columns.Contains(this.descHeader))
        this.gvDirectory.Columns.Remove(this.descHeader);
      this.gvDirectory.Columns[0].SpringToFit = true;
      this.btnRename.Visible = false;
      this.btnCut.Visible = false;
      this.btnCopy.Visible = false;
      this.btnPaste.Visible = false;
      this.displayFoldersOnly = false;
      this.btnImport.Visible = false;
      this.menuItemImportVisible = false;
      this.btnDeploy.Visible = false;
      this.displayExtension = true;
      this.SingleSelection = true;
      switch (dialogMode)
      {
        case FSExplorer.DialogMode.SelectFiles:
          this.initializeButtons();
          this.disableAllMenuItemsExceptRefresh = true;
          break;
        case FSExplorer.DialogMode.SaveFiles:
          this.arrangeButtons((object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnNewFolder, (object) this.verticalSeparator1, (object) this.btnRename);
          this.displayFolderOperationButtonsOnly = true;
          break;
        case FSExplorer.DialogMode.ManageFiles:
          this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnNewFolder, (object) this.btnDelete);
          this.menuItemCut.Visible = true;
          this.menuItemCopy.Visible = true;
          this.menuItemPaste.Visible = true;
          this.menuItemRename.Visible = true;
          break;
      }
      this.gvDirectory.Sort(0, SortOrder.Ascending);
      this.setTitle();
    }

    public void EnableSetAsDefaultButton(bool enabled)
    {
      this.btnSetAsDefault.Enabled = enabled;
      this.menuItemSetAsDefault.Enabled = enabled;
    }

    public void SetProperties(
      bool displayFoldersOnly,
      bool menuItemImportVisible,
      bool disableAllMenuItemsExceptRefresh,
      bool displayExtension)
    {
      this.SetProperties(displayFoldersOnly, menuItemImportVisible, disableAllMenuItemsExceptRefresh);
      this.templateType = FSExplorer.TemplateSettingsType.Nothing;
      this.displayExtension = displayExtension;
      if (this.fileType == FSExplorer.FileTypes.Reports)
        this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename);
      this.gvDirectory.Sort(0, SortOrder.Ascending);
      this.setTitle();
    }

    public void SetProperties(
      bool displayFoldersOnly,
      bool menuItemImportVisible,
      bool disableAllMenuItemsExceptRefresh)
    {
      if (this._ifsExplorer != null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "FSExplorer already initialized!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (this.gvDirectory.Columns.Contains(this.descHeader))
          this.gvDirectory.Columns.Remove(this.descHeader);
        this.gvDirectory.Columns[0].SpringToFit = true;
        if (this.fileType == FSExplorer.FileTypes.LoanTemplates)
        {
          if (menuItemImportVisible)
            this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnImport, (object) this.btnSetAsDefault);
          else
            this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnSetAsDefault);
        }
        else if (menuItemImportVisible)
          this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnImport);
        else
          this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename);
        this.displayFoldersOnly = displayFoldersOnly;
        this.menuItemImportVisible = menuItemImportVisible;
        this.disableAllMenuItemsExceptRefresh = disableAllMenuItemsExceptRefresh;
        this.templateType = FSExplorer.TemplateSettingsType.Nothing;
        this.displayExtension = true;
        this.setTitle();
      }
    }

    public void SetProperties(
      bool displayFoldersOnly,
      bool menuItemImportVisible,
      bool disableAllMenuItemsExceptRefresh,
      int templateSettingsType,
      bool displayExtension)
    {
      this.SetProperties(displayFoldersOnly, menuItemImportVisible, disableAllMenuItemsExceptRefresh);
      this.gvDirectory.Columns.Add(this.descHeader);
      this.gvDirectory.Columns[0].SpringToFit = false;
      this.templateType = (FSExplorer.TemplateSettingsType) templateSettingsType;
      this.displayExtension = displayExtension;
      this.gvDirectory.Sort(0, SortOrder.Ascending);
      this.setTitle();
    }

    public void InitCampaignTemplate(IFSExplorerBase ifsExplorer, FileSystemEntry defaultFolder)
    {
      this.hasPublicRight = this.hasCampaignTemplatePublicRight();
      this.init(ifsExplorer, defaultFolder, !this.hasCampaignTemplatePrivateRight());
    }

    public void InitDashboardTemplate(
      IFSExplorerBase ifsExplorer,
      FileSystemEntry defaultFolder,
      bool showPublicOnly)
    {
      this.hasPublicRight = this.hasDashboardTemplatePublicRight();
      if (showPublicOnly)
        this.init(ifsExplorer, defaultFolder, true);
      else
        this.init(ifsExplorer, defaultFolder, !this.hasDashboardTemplatePrivateRight());
    }

    public void InitDashboardViewTemplate(
      IFSExplorerBase ifsExplorer,
      FileSystemEntry defaultFolder)
    {
      this.hasPublicRight = this.hasDashboardViewTemplatePublicRight();
      this.init(ifsExplorer, defaultFolder, !this.hasDashboardViewTemplatePrivateRight());
    }

    public void Init(IFSExplorerBase ifsExplorer, FileSystemEntry defaultFolder, bool publicOnly)
    {
      this.init(ifsExplorer, defaultFolder, publicOnly);
    }

    public void Init(IFSExplorerBase ifsExplorer, FileSystemEntry defaultFolder)
    {
      this.Init(ifsExplorer, defaultFolder, false);
    }

    private void init(IFSExplorerBase ifsExplorer, FileSystemEntry defaultFolder, bool publicOnly)
    {
      if (ifsExplorer == null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Parameter ifsExplorer cannot be null!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (this._ifsExplorer != null)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "FSExplorer already initialized!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this._ifsExplorer = ifsExplorer;
        this.currRoot = defaultFolder != null || publicOnly ? (!publicOnly ? (defaultFolder.Owner == null ? FSExplorer.RootType.Public : FSExplorer.RootType.Private) : FSExplorer.RootType.PublicOnly) : FSExplorer.RootType.Top;
        this.changeFolder(this.normalizeFolder(defaultFolder.Path, this.currRoot), this.currRoot);
        this.updateButtons();
        this.setTitle();
        if (!this.doNotHaveRightToPersonalFolder() || this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.FundingTemplate)
          return;
        this.MakeEmptySelectionReadWrite();
        this.MakeFileReadOnly();
        this.MakeFolderReadOnly();
      }
    }

    public EllieMae.EMLite.ContactUI.ContactType setContactType
    {
      get => this.contactType;
      set => this.contactType = value;
    }

    public void Reset()
    {
      this.displayFoldersOnly = false;
      this.menuItemImportVisible = true;
      this.disableAllMenuItemsExceptRefresh = false;
      this.displayExtension = true;
      this.templateType = FSExplorer.TemplateSettingsType.Nothing;
      this._ifsExplorer = (IFSExplorerBase) null;
    }

    private bool hasCampaignTemplatePublicRight()
    {
      return UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.CampaignTemplate);
    }

    private bool hasCampaignTemplatePrivateRight()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.Cnt_Campaign_PersonalTemplates);
    }

    private bool hasDashboardTemplatePublicRight()
    {
      return UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.DashboardTemplate);
    }

    private bool hasDashboardViewTemplatePublicRight()
    {
      return UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.DashboardViewTemplate);
    }

    private bool hasDashboardTemplatePrivateRight()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.DashboardTab_ManagePersonalTemplate);
    }

    private bool hasDashboardViewTemplatePrivateRight()
    {
      return ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.DashboardTab_ManagePersonalViewTemplate);
    }

    private string normalizeFolder(string folder, FSExplorer.RootType rootType)
    {
      if (folder == null || folder.Length == 0)
        return "\\";
      while (folder.IndexOf("\\\\") >= 0)
        folder = folder.Replace("\\\\", "\\");
      if (folder.EndsWith("\\"))
        folder = folder.Substring(0, folder.Length - 1);
      if (!folder.StartsWith("\\"))
        folder = "\\" + folder;
      return folder;
    }

    private string getPathPrefix(FSExplorer.RootType rootType)
    {
      if (rootType == FSExplorer.RootType.Top)
        return "\\";
      if (rootType == FSExplorer.RootType.PublicOnly)
        return "\\\\" + this.getRootName(true);
      string empty = string.Empty;
      return rootType != FSExplorer.RootType.Public ? "\\\\" + this.getRootName(false) : "\\\\" + this.getRootName(true);
    }

    private string getPathPrefix(string path) => this.getPathPrefix(this.getRootType(path));

    private FSExplorer.RootType getRootType(string path)
    {
      if (this.currRoot == FSExplorer.RootType.PublicOnly)
        return FSExplorer.RootType.PublicOnly;
      if (path == "\\\\")
        return FSExplorer.RootType.Top;
      path = path.ToLower();
      if (path.StartsWith("\\\\personal"))
        return FSExplorer.RootType.Private;
      if (path.StartsWith("\\\\public"))
        return FSExplorer.RootType.Public;
      throw new ApplicationException("Invalid path");
    }

    private string getFolder(string path)
    {
      FSExplorer.RootType rootType = this.getRootType(path);
      if (rootType == FSExplorer.RootType.Top)
        return "\\";
      int startIndex = 0;
      string empty = string.Empty;
      if (rootType == FSExplorer.RootType.Private)
        startIndex = ("\\\\" + this.getRootName(false)).Length;
      else if (rootType == FSExplorer.RootType.Public || rootType == FSExplorer.RootType.PublicOnly)
        startIndex = ("\\\\" + this.getRootName(true)).Length;
      return this.normalizeFolder(path.Substring(startIndex, path.Length - startIndex), rootType);
    }

    private FileSystemEntry setFakeFolderProperty(FileSystemEntry newFolder)
    {
      AclFileType fileType = AclFileType.LoanProgram;
      string uri = "";
      switch (this.fileType)
      {
        case FSExplorer.FileTypes.PrintGroups:
          fileType = AclFileType.PrintGroups;
          uri = "Public:\\Public Forms Groups\\";
          break;
        case FSExplorer.FileTypes.CustomForms:
          fileType = AclFileType.CustomPrintForms;
          uri = "Public:\\Public Custom Forms\\";
          break;
        case FSExplorer.FileTypes.CustomLetters:
          fileType = this.contactType != EllieMae.EMLite.ContactUI.ContactType.BizPartner ? AclFileType.BorrowerCustomLetters : AclFileType.BizCustomLetters;
          uri = "Public:\\Public Custom Letters\\";
          break;
        case FSExplorer.FileTypes.Reports:
          fileType = AclFileType.Reports;
          uri = "Public:\\Public Reports\\";
          break;
        case FSExplorer.FileTypes.LoanTemplates:
          fileType = AclFileType.LoanTemplate;
          uri = "Public:\\Public Loan Templates\\";
          break;
        case FSExplorer.FileTypes.DataTemplates:
          fileType = AclFileType.MiscData;
          uri = "Public:\\Public Data Templates\\";
          break;
        case FSExplorer.FileTypes.LoanPrograms:
          fileType = AclFileType.LoanProgram;
          uri = "Public:\\Public Loan Programs\\";
          break;
        case FSExplorer.FileTypes.ClosingCosts:
          fileType = AclFileType.ClosingCost;
          uri = "Public:\\Public Closing Cost Templates\\";
          break;
        case FSExplorer.FileTypes.DocumentSets:
          fileType = AclFileType.DocumentSet;
          uri = "Public:\\Public Document Sets\\";
          break;
        case FSExplorer.FileTypes.FormLists:
          fileType = AclFileType.FormList;
          uri = "Public:\\Public Form Lists\\";
          break;
        case FSExplorer.FileTypes.CampaignTemplates:
          fileType = AclFileType.CampaignTemplate;
          uri = "Public:\\Public Campaign Templates\\";
          break;
        case FSExplorer.FileTypes.DashboardTemplate:
          fileType = AclFileType.DashboardTemplate;
          uri = "Public:\\Public Dashboard Templates\\";
          break;
        case FSExplorer.FileTypes.DashboardViewTemplate:
          fileType = AclFileType.DashboardViewTemplate;
          uri = "Public:\\Public DashboardView Templates\\";
          break;
        case FSExplorer.FileTypes.TaskSets:
          fileType = AclFileType.TaskSet;
          uri = "Public:\\Public Task Sets\\";
          break;
        case FSExplorer.FileTypes.SettlementServiceProviders:
          fileType = AclFileType.SettlementServiceProviders;
          uri = "Public:\\Public Settlement Service Providers\\";
          break;
        case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
          fileType = AclFileType.AffiliatedBusinessArrangements;
          uri = "Public:\\Public Affiliates\\";
          break;
      }
      if (uri == "")
        return newFolder;
      AclResourceAccess fileFolderAccess = this.session.AclGroupManager.GetUserFileFolderAccess(fileType, FileSystemEntry.Parse(uri));
      if (fileFolderAccess == AclResourceAccess.ReadWrite)
        newFolder.Access = fileFolderAccess;
      return newFolder;
    }

    private string ConstructFilePath(
      string name,
      FileSystemEntry.Types type,
      string path,
      string extension)
    {
      string str1 = "Public:" + path;
      string str2;
      if (type == FileSystemEntry.Types.Folder)
      {
        str2 = str1 + name + "\\";
      }
      else
      {
        str2 = str1 + name;
        if (extension != "")
          str2 += extension;
      }
      return str2;
    }

    private AclFileType GetAclFileType()
    {
      AclFileType aclFileType = AclFileType.LoanProgram;
      switch (this.fileType)
      {
        case FSExplorer.FileTypes.PrintGroups:
          aclFileType = AclFileType.PrintGroups;
          break;
        case FSExplorer.FileTypes.CustomForms:
          aclFileType = AclFileType.CustomPrintForms;
          break;
        case FSExplorer.FileTypes.CustomLetters:
          aclFileType = this.contactType != EllieMae.EMLite.ContactUI.ContactType.BizPartner ? AclFileType.BorrowerCustomLetters : AclFileType.BizCustomLetters;
          break;
        case FSExplorer.FileTypes.Reports:
          aclFileType = AclFileType.Reports;
          break;
        case FSExplorer.FileTypes.LoanTemplates:
          aclFileType = AclFileType.LoanTemplate;
          break;
        case FSExplorer.FileTypes.DataTemplates:
          aclFileType = AclFileType.MiscData;
          break;
        case FSExplorer.FileTypes.LoanPrograms:
          aclFileType = AclFileType.LoanProgram;
          break;
        case FSExplorer.FileTypes.ClosingCosts:
          aclFileType = AclFileType.ClosingCost;
          break;
        case FSExplorer.FileTypes.DocumentSets:
          aclFileType = AclFileType.DocumentSet;
          break;
        case FSExplorer.FileTypes.FormLists:
          aclFileType = AclFileType.FormList;
          break;
        case FSExplorer.FileTypes.CampaignTemplates:
          aclFileType = AclFileType.CampaignTemplate;
          break;
        case FSExplorer.FileTypes.DashboardTemplate:
          aclFileType = AclFileType.DashboardTemplate;
          break;
        case FSExplorer.FileTypes.DashboardViewTemplate:
          aclFileType = AclFileType.DashboardViewTemplate;
          break;
        case FSExplorer.FileTypes.TaskSets:
          aclFileType = AclFileType.TaskSet;
          break;
        case FSExplorer.FileTypes.SettlementServiceProviders:
          aclFileType = AclFileType.SettlementServiceProviders;
          break;
        case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
          aclFileType = AclFileType.AffiliatedBusinessArrangements;
          break;
      }
      return aclFileType;
    }

    private void changeFolder(string folder, FSExplorer.RootType rootType)
    {
      if (this.fileType == FSExplorer.FileTypes.SyncTemplates)
        return;
      this.preDescription = string.Empty;
      this.preSelected = string.Empty;
      string empty = string.Empty;
      try
      {
        if (rootType == FSExplorer.RootType.Top)
        {
          this.entries = new FileSystemEntry[2];
          this.entries[0] = new FileSystemEntry("\\Personal\\", FileSystemEntry.Types.Folder, this.session.UserID);
          this.entries[1] = new FileSystemEntry("\\Public\\", FileSystemEntry.Types.Folder, (string) null);
          this.currentFolder = FileSystemEntry.PublicRoot;
        }
        else
        {
          FileSystemEntry fileSystemEntry = new FileSystemEntry(folder, FileSystemEntry.Types.Folder, this.isRootTypePublic(rootType) ? (string) null : this.session.UserID);
          this.entries = this.currFSEntries;
          if (fileSystemEntry.IsPublic && (this.entries == null || this.entries.Length == 0 || this.currentFolder.Path == fileSystemEntry.Path || this.currentFolder.ParentFolder != null && this.currentFolder.ParentFolder.Path == fileSystemEntry.Path || !this.currentFolder.IsPublic) && fileSystemEntry.ParentFolder != null)
            this.entries = this.IfsExplorer.GetFileSystemEntries(fileSystemEntry.ParentFolder);
          if (this.entries != null && fileSystemEntry.IsPublic)
          {
            foreach (FileSystemEntry entry in this.entries)
            {
              if (fileSystemEntry.Path == entry.Path)
              {
                fileSystemEntry = entry;
                break;
              }
            }
          }
          if (fileSystemEntry.Path == "\\" && fileSystemEntry.IsPublic)
          {
            fileSystemEntry = this.setFakeFolderProperty(fileSystemEntry);
            if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
              fileSystemEntry.Access = AclResourceAccess.ReadWrite;
          }
          this.entries = this.IfsExplorer.GetFileSystemEntries(fileSystemEntry);
          if (this.entries == null)
            return;
          this.currentFolder = fileSystemEntry;
        }
        if (this.currRoot != FSExplorer.RootType.PublicOnly)
          this.currRoot = rootType;
        this.ResetOption();
        if (this.doNotHaveRightToPersonalFolder())
        {
          this.MakeFileReadOnly();
          this.MakeFolderReadOnly();
        }
        else if (!this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite)
          this.MakeEmptySelectionReadWrite();
        else if (this.currentFolder.IsPublic)
        {
          if (this.currentFolder.Access == AclResourceAccess.ReadOnly)
            this.MakeFolderReadOnly();
          else
            this.MakeFolderReadWrite();
        }
        this.gvDirectory.Items.Clear();
        if (this.entries == null)
        {
          this.currFSEntries = new FileSystemEntry[0];
        }
        else
        {
          this.currFSEntries = this.entries;
          this.gvDirectory.BeginUpdate();
          for (int index = 0; index < this.entries.Length; ++index)
          {
            if (this.currRoot == FSExplorer.RootType.Top)
            {
              string rootName = this.getRootName(this.entries[index].IsPublic);
              GVItem gvItem = this.gvDirectory.Items.Add(rootName);
              gvItem.ImageIndex = this.getFolderImageIcon(rootName);
              gvItem.Tag = (object) this.entries[index];
            }
            else if (this.entries[index].Type == FileSystemEntry.Types.Folder)
            {
              if (this.templateType == FSExplorer.TemplateSettingsType.Nothing)
              {
                GVItem gvItem = this.gvDirectory.Items.Add(this.getDisplayName(this.entries[index]));
                gvItem.ImageIndex = this.getFolderImageIcon(this.entries[index].Name);
                gvItem.Tag = (object) this.entries[index];
              }
              else
              {
                GVItem gvItem = new GVItem(this.getDisplayName(this.entries[index]));
                gvItem.SubItems.Add((object) "");
                if (this.fileType == FSExplorer.FileTypes.FundingTemplate || this.fileType == FSExplorer.FileTypes.ClosingCosts || this.fileType == FSExplorer.FileTypes.DataTemplates)
                  gvItem.SubItems.Add((object) "");
                gvItem.ImageIndex = this.getFolderImageIcon(this.entries[index].Name);
                gvItem.Tag = (object) this.entries[index];
                this.gvDirectory.Items.Add(gvItem);
              }
            }
            else if (this.entries[index].Type == FileSystemEntry.Types.File && !this.displayFoldersOnly)
            {
              if (this.templateType == FSExplorer.TemplateSettingsType.Nothing)
              {
                GVItem gvItem = this.gvDirectory.Items.Add(this.getDisplayName(this.entries[index]));
                gvItem.ImageIndex = this.getFileImageIcon();
                gvItem.Tag = (object) this.entries[index];
              }
              else
              {
                string respa = this.IfsExplorer.GetRESPA(this.entries[index]);
                if (this.respaMode == FSExplorer.RESPAFilter.All || (this.respaMode != FSExplorer.RESPAFilter.Respa2015 || !(respa != "2015")) && (this.respaMode != FSExplorer.RESPAFilter.Respa2010 || !(respa != "2010")) && (this.respaMode != FSExplorer.RESPAFilter.Respa2009 || !(respa != "2009")) || this.templateType == FSExplorer.TemplateSettingsType.MiscData && (this.templateType != FSExplorer.TemplateSettingsType.MiscData || !(respa != "")))
                {
                  if (this.fileType == FSExplorer.FileTypes.FundingTemplate && this.gvDirectory.Columns.Count < 3 && respa == "2009")
                    return;
                  GVItem gvItem = new GVItem(this.getDisplayName(this.entries[index]));
                  if (this.gvDirectory.Columns.Count > 2)
                  {
                    if (this.fileType == FSExplorer.FileTypes.DataTemplates)
                      gvItem.SubItems.Add(respa == "2009" ? (object) "Old" : (object) respa);
                    else if (this.fileType == FSExplorer.FileTypes.ClosingCosts)
                      gvItem.SubItems.Add(respa == "2015" ? (object) "Yes" : (object) "No");
                    else if (this.fileType == FSExplorer.FileTypes.FundingTemplate)
                      gvItem.SubItems.Add(respa == "2010" ? (object) "Yes" : (object) "No");
                  }
                  string description = this.IfsExplorer.GetDescription(this.entries[index]);
                  gvItem.SubItems.Add((object) description);
                  gvItem.ImageIndex = this.getFileImageIcon();
                  gvItem.Tag = (object) this.entries[index];
                  this.gvDirectory.Items.Add(gvItem);
                }
              }
            }
          }
          this.gvDirectory.EndUpdate();
          this.gvDirectory.Columns[0].ActivatedEditorType = this.currRoot != FSExplorer.RootType.Top ? GVActivatedEditorType.TextBox : GVActivatedEditorType.None;
          this.refreshFolderCombo(SystemUtil.CombinePath(this.getPathPrefix(this.currRoot), this.currentFolder.Path));
          this.rightMouseX = -1;
          this.rightMouseY = -1;
          if (this.FolderChanged == null)
            return;
          this.FolderChanged((object) this, new EventArgs());
        }
      }
      catch (Exception ex)
      {
        Tracing.Log((string) null, nameof (FSExplorer), TraceLevel.Error, "Error in Change Folder method with exception : " + ex.Message + ". Folder : " + folder);
        throw;
      }
      finally
      {
        if (this.currRoot == FSExplorer.RootType.Top || FSExplorer.RootType.PublicOnly == this.currRoot && "\\" == this._currentFolder.Path || FSExplorer.RootType.Public == this.currRoot && "\\" == this._currentFolder.Path || FSExplorer.RootType.Private == this.currRoot && "\\" == this._currentFolder.Path)
          this.stdIconBtnUpFolder.Enabled = false;
        else
          this.stdIconBtnUpFolder.Enabled = true;
        this.gvDirectory.ClearSort();
        this.setTitle();
      }
    }

    private string removeFileExtension(string filename)
    {
      int length = filename.LastIndexOf('.');
      return length > 0 ? filename.Substring(0, length) : filename;
    }

    private int getFolderImageIcon(string itemName)
    {
      return this.currRoot == FSExplorer.RootType.Top && itemName == this.getRootName(false) || this.currRoot != FSExplorer.RootType.Public && this.currRoot != FSExplorer.RootType.PublicOnly && !(itemName == this.getRootName(true)) ? 2 : 0;
    }

    private int getFileImageIcon()
    {
      return FSExplorer.FileTypes.PrintGroups == this.fileType ? (FSExplorer.RootType.Public == this.currRoot ? 3 : 4) : (FSExplorer.FileTypes.Reports == this.fileType ? 5 : 1);
    }

    private void ctxMenuLstView1_Popup(object sender, EventArgs e)
    {
      if (this.displayFolderOperationButtonsOnly || this.hideAllButtons)
        return;
      GVItem itemAt = this.gvDirectory.GetItemAt(this.rightMouseX, this.rightMouseY);
      if (itemAt != null && itemAt.Tag.ToString() == "")
        return;
      if (this.displayFoldersOnly || this.disableAllMenuItemsExceptRefresh)
        this.menuItemNew.Visible = this.btnAdd.Visible;
      this.menuItemCreateFolder.Visible = this.fileType != FSExplorer.FileTypes.ConditionalLetter && !this.disableAllMenuItemsExceptRefresh;
      this.menuItemOpen.Visible = this.menuItemCut.Visible = this.menuItemCopy.Visible = this.menuItemPaste.Visible = !this.disableAllMenuItemsExceptRefresh;
      this.menuItemDelete.Visible = this.menuItemRename.Visible = !this.disableAllMenuItemsExceptRefresh;
      this.menuItemNew.Visible = !this.disableAllMenuItemsExceptRefresh;
      this.menuItemImport.Visible = this.menuItemImportVisible;
      if (this.btnExport.Visible)
      {
        this.menuItemExport.Visible = true;
        this.menuItemExport.Enabled = this.btnExport.Enabled;
      }
      if (this.btnDeploy.Visible)
      {
        this.menuItemDeploy.Visible = true;
        this.menuItemDeploy.Enabled = this.btnDeploy.Enabled;
      }
      this.menuItemOpen.Enabled = this.btnOpen.Enabled;
      if (itemAt == null)
      {
        this.menuItemDuplicate.Visible = this.menuItemCut.Visible = this.menuItemCopy.Visible = this.menuItemDelete.Visible = false;
        this.menuItemOpen.Visible = this.menuItemRename.Visible = false;
        if (this.currRoot != FSExplorer.RootType.Top)
        {
          if (this.doNotHaveRightToPersonalFolder())
          {
            this.ResetOption();
            this.MakeFileReadOnly();
            this.MakeFolderReadOnly();
          }
          else if (!this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite || this.fileType == FSExplorer.FileTypes.FundingTemplate || this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate)
            this.MakeEmptySelectionReadWrite();
        }
      }
      else
      {
        this.menuItemDuplicate.Visible = this.menuItemCut.Visible = this.menuItemCopy.Visible = this.menuItemDelete.Visible = !this.disableAllMenuItemsExceptRefresh;
        this.menuItemOpen.Visible = this.menuItemRename.Visible = !this.disableAllMenuItemsExceptRefresh;
        FileSystemEntry tag = (FileSystemEntry) itemAt.Tag;
        if (this.doNotHaveRightToPersonalFolder() && this.fileType != FSExplorer.FileTypes.PurchaseAdvice && this.fileType != FSExplorer.FileTypes.FundingTemplate)
        {
          this.MakeEmptySelectionReadWrite();
          this.MakeFileReadOnly();
          this.MakeFolderReadOnly();
        }
        else if (tag.Type == FileSystemEntry.Types.Folder)
        {
          this.btnDeploy.Enabled = this.menuItemDeploy.Enabled = false;
          if (!this.disableAllMenuItemsExceptRefresh)
          {
            if (!this.currentFolder.IsPublic || tag.Access == AclResourceAccess.ReadWrite)
              this.MakeFolderReadWrite();
            else if (this.currentFolder.IsPublic)
              this.MakeFolderReadOnly();
          }
        }
        else if (!this.disableAllMenuItemsExceptRefresh)
        {
          if (!this.currentFolder.IsPublic || tag.Access == AclResourceAccess.ReadWrite)
            this.MakeFileReadWrite();
          else if (this.currentFolder.IsPublic)
            this.MakeFileReadOnly();
        }
      }
      if (this.fileType == FSExplorer.FileTypes.StackingOrderSets)
        this.menuItemDuplicate.Visible = true;
      if (this.fileType != FSExplorer.FileTypes.StackingOrderSets && this.fileType != FSExplorer.FileTypes.PurchaseAdvice && this.fileType != FSExplorer.FileTypes.FundingTemplate && this.fileType != FSExplorer.FileTypes.LoanDuplicationTemplate)
        return;
      this.menuItemCopy.Visible = false;
      this.menuItemCreateFolder.Visible = false;
      this.menuItemCut.Visible = false;
      this.menuItemImport.Visible = false;
      this.menuItemPaste.Visible = false;
      this.menuItemRefresh.Visible = false;
      this.menuItemDuplicate.Index = 0;
      this.menuItemRename.Index = 1;
      this.menuItemOpen.Index = 2;
      this.menuItemDelete.Index = 3;
    }

    private void picFolderUp_Click(object sender, EventArgs e)
    {
      if (this.currRoot == FSExplorer.RootType.Top || FSExplorer.RootType.PublicOnly == this.currRoot && "\\" == this.currentFolder.Path || FSExplorer.RootType.Public == this.currRoot && "\\" == this.currentFolder.Path || FSExplorer.RootType.Private == this.currRoot && "\\" == this.currentFolder.Path)
        return;
      this.changeFolder(this.currentFolder.ParentFolder.Path, this.currRoot);
      this.ResetOption();
      if (!this.currentFolder.IsPublic && AclResourceAccess.ReadWrite != this.currentFolder.Access)
      {
        this.MakeFileReadOnly();
        this.MakeFolderReadOnly();
      }
      else if (!this.currentFolder.IsPublic || AclResourceAccess.ReadWrite == this.currentFolder.Access)
        this.MakeEmptySelectionReadWrite();
      this.setTitle();
      this.setButtonsForReports();
    }

    private string getParentDirectory(string path)
    {
      int length = path.LastIndexOf('\\');
      return length <= 0 ? "\\" : path.Substring(0, length);
    }

    private void menuItemRefresh_Click(object sender, EventArgs e)
    {
      this.changeFolder(this.currentFolder.Path, this.currRoot);
      if (this.RefreshedClicked == null)
        return;
      this.RefreshedClicked((object) this, new EventArgs());
    }

    private void open(GVItem lvItem)
    {
      FileSystemEntry tag = (FileSystemEntry) lvItem.Tag;
      if (tag.Type == FileSystemEntry.Types.File)
      {
        if (this.fileType == FSExplorer.FileTypes.CustomForms)
          this.IfsExplorer.OpenFile(tag, lvItem);
        else if (this.templateType == FSExplorer.TemplateSettingsType.Nothing)
          this.IfsExplorer.OpenFile(tag);
        else
          this.IfsExplorer.OpenFile(tag, lvItem);
        if (this.fileType != FSExplorer.FileTypes.StackingOrderSets)
        {
          foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
            selectedItem.Selected = false;
        }
        this.preSelected = "";
        if (this.FileChanged == null)
          return;
        this.FileChanged((object) this, new EventArgs());
      }
      else
      {
        string folder = tag.Path;
        if (this.currRoot == FSExplorer.RootType.Top)
        {
          string empty = string.Empty;
          string rootName = this.getRootName(false);
          this.currRoot = !(lvItem.Text.ToLower() == rootName.ToLower()) ? FSExplorer.RootType.Public : FSExplorer.RootType.Private;
          folder = "\\";
        }
        this.changeFolder(folder, this.currRoot);
      }
    }

    private void menuItemOpen_Click(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0 || this.gvDirectory.SelectedItems[0].Tag.ToString() == "")
        return;
      FileSystemEntry fileSystemEntry = (FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag;
      if (fileSystemEntry.Type != FileSystemEntry.Types.Folder && !this.hasPublicRight && this.IsRootPublic && (this.fileType == FSExplorer.FileTypes.CustomLetters || this.fileType == FSExplorer.FileTypes.CustomForms))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the require access right to open this form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        GVItem selectedItem = this.gvDirectory.SelectedItems[0];
        if (selectedItem == null)
          return;
        this.open(selectedItem);
        if (this.gvDirectory.SelectedItems.Count == 0)
          fileSystemEntry = this.currentFolder;
        this.ResetOption();
        if (fileSystemEntry.Type == FileSystemEntry.Types.Folder)
        {
          if (this.currentFolder.IsPublic && this.currentFolder.Access != AclResourceAccess.ReadWrite)
            return;
          this.MakeEmptySelectionReadWrite();
        }
        else
        {
          if (this.currentFolder.IsPublic && fileSystemEntry.Access != AclResourceAccess.ReadWrite)
            return;
          this.MakeFileReadWrite();
        }
      }
    }

    private void menuItemDelete_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh || this.gvDirectory.SelectedItems.Count == 0 || this.gvDirectory.SelectedItems[0].Tag.ToString() == "" && this.gvDirectory.SelectedItems.Count == 1)
        return;
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The root folder cannot be deleted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (FSExplorer.FileTypes.DashboardTemplate != this.fileType && Utils.Dialog((IWin32Window) this, this.gvDirectory.SelectedItems.Count != 1 ? "Are you sure you want to delete " + (object) this.gvDirectory.SelectedItems.Count + " items?" : "Are you sure you want to delete '" + this.gvDirectory.SelectedItems[0].Text + "'?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        for (int index = 0; index < this.gvDirectory.SelectedItems.Count; ++index)
        {
          if (!(this.gvDirectory.SelectedItems[index].Tag.ToString() == ""))
          {
            FileSystemEntry tag = (FileSystemEntry) this.gvDirectory.SelectedItems[index].Tag;
            try
            {
              this.FileActionConfirmed = true;
              if (tag.Type == FileSystemEntry.Types.Folder)
              {
                if (this.IfsExplorer.GetFileSystemEntries(tag).Length != 0)
                {
                  int num2 = (int) Utils.Dialog((IWin32Window) this, "The folder '" + tag.Name + "' cannot be deleted because it is not empty.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  return;
                }
                if (this.AfterFolderDeleted != null)
                  this.AfterFolderDeleted((object) this, e);
              }
              else if (this.BeforeFileDeleted != null && tag.Type == FileSystemEntry.Types.File && tag.IsPublic)
                this.BeforeFileDeleted((object) new object[2]
                {
                  (object) this,
                  (object) tag
                }, EventArgs.Empty);
              if (this.FileActionConfirmed)
              {
                this.IfsExplorer.DeleteEntry(tag);
                if (tag.Type == FileSystemEntry.Types.File)
                {
                  if (this.AfterFileDeleted != null)
                    this.AfterFileDeleted((object) tag, EventArgs.Empty);
                }
              }
            }
            catch (Exception ex)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
          }
        }
        this.menuItemRefresh_Click((object) this, (EventArgs) null);
        this.ResetOption();
        FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
        if (aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionalApprovalLetter) || !this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite || this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate)
          this.MakeEmptySelectionReadWrite();
        if (Session.IsBrokerEdition())
          return;
        if (this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate && this.gvDirectory.Items.Count <= 0)
        {
          aclManager.DisablePermission(AclFeature.LoanMgmt_Duplicate);
        }
        else
        {
          int[] array = ((IEnumerable<string>) aclManager.GetPersonaListByFeature(new AclFeature[1]
          {
            AclFeature.LoanMgmt_Duplicate
          }, AclTriState.True)).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
          LoanDuplicationTemplateAclInfo[] duplicationTemplates = ((LoanDuplicationAclManager) this.session.ACL.GetAclManager(AclCategory.LoanDuplicationTemplates)).GetAccessibleLoanDuplicationTemplates(array);
          foreach (int num4 in array)
          {
            int personaID = num4;
            if (duplicationTemplates == null || ((IEnumerable<LoanDuplicationTemplateAclInfo>) duplicationTemplates).Where<LoanDuplicationTemplateAclInfo>((Func<LoanDuplicationTemplateAclInfo, bool>) (p => p.PersonaID == personaID)).Count<LoanDuplicationTemplateAclInfo>() <= 0)
              aclManager.SetPermission(AclFeature.LoanMgmt_Duplicate, personaID, AclTriState.False);
          }
        }
      }
    }

    private bool selectedCustomPrintFormHasDocs()
    {
      this.session.ConfigurationManager.GetDocumentTrackingSetup();
      for (int index = 0; index < this.gvDirectory.SelectedItems.Count; ++index)
      {
        if (!(this.gvDirectory.SelectedItems[index].Tag.ToString() == "") && this.customPrintFormHasDocs((FileSystemEntry) this.gvDirectory.SelectedItems[index].Tag))
          return true;
      }
      return false;
    }

    private bool cutCustomPrintFormHasDocs()
    {
      this.session.ConfigurationManager.GetDocumentTrackingSetup();
      for (int index = 0; index < this.cutCopyEntries.Length; ++index)
      {
        if (this.customPrintFormHasDocs(this.cutCopyEntries[index]))
          return true;
      }
      return false;
    }

    private bool customPrintFormHasDocs(FileSystemEntry entry)
    {
      foreach (DocumentTemplate documentTemplate in this.session.ConfigurationManager.GetDocumentTrackingSetup())
      {
        if ((documentTemplate.SourceType == "Custom Form" || documentTemplate.SourceType == "Borrower Specific Custom Form") && (string.Compare(documentTemplate.Source, entry.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(documentTemplate.SourceBorrower, entry.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(documentTemplate.SourceCoborrower, entry.ToString(), StringComparison.OrdinalIgnoreCase) == 0))
          return true;
      }
      return false;
    }

    private void menuItemCreateFolder_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh && !this.displayFolderOperationButtonsOnly)
        return;
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "New folder cannot be added to the current folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string str = "New Folder";
        FileSystemEntry entry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, str), FileSystemEntry.Types.Folder, this.isRootPublic ? (string) null : this.session.UserID);
        int num2 = 2;
        while (this.IfsExplorer.EntryExistsOfAnyType(entry))
        {
          str = "New Folder (" + (object) num2 + ")";
          entry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, str), FileSystemEntry.Types.Folder, this.isRootPublic ? (string) null : this.session.UserID);
          ++num2;
        }
        this.IfsExplorer.CreateFolder(entry);
        this.currFSEntries = this.IfsExplorer.GetFileSystemEntries(this.currentFolder);
        foreach (FileSystemEntry currFsEntry in this.currFSEntries)
        {
          if (currFsEntry.Path == entry.Path)
          {
            entry = currFsEntry;
            break;
          }
        }
        GVItem gvItem = new GVItem(str);
        gvItem.ImageIndex = this.getFolderImageIcon(str);
        gvItem.Tag = (object) entry;
        this.gvDirectory.Items.Add(gvItem);
        gvItem.BeginEdit();
        this.setTitle();
      }
    }

    private void gvDirectory_EditorClosing(object sender, GVSubItemEditingEventArgs e)
    {
      GVItem parent = e.SubItem.Parent;
      FileSystemEntry tag1 = (FileSystemEntry) parent.Tag;
      FileSystemEntry tag2 = (FileSystemEntry) parent.Tag;
      if (!tag1.IsPublic || tag1.Access == AclResourceAccess.ReadWrite)
      {
        if (tag1.Type == FileSystemEntry.Types.Folder)
          this.MakeFolderReadWrite();
        else
          this.MakeFileReadWrite();
      }
      if (e.EditorControl.Text == e.SubItem.Text)
        return;
      if (e.EditorControl.Text.IndexOf("\\") > -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot contain the \"\\\" character.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
        e.EditorControl.Text = tag2.Name;
      }
      else if (tag1.Type == FileSystemEntry.Types.Folder && (this.fileType == FSExplorer.FileTypes.DashboardTemplate || this.fileType == FSExplorer.FileTypes.DashboardViewTemplate || this.fileType == FSExplorer.FileTypes.Reports || this.fileType == FSExplorer.FileTypes.CustomForms || this.fileType == FSExplorer.FileTypes.PrintGroups || this.fileType == FSExplorer.FileTypes.LoanPrograms || this.fileType == FSExplorer.FileTypes.ClosingCosts || this.fileType == FSExplorer.FileTypes.FormLists || this.fileType == FSExplorer.FileTypes.SettlementServiceProviders || this.fileType == FSExplorer.FileTypes.AffiliatedBusinessArrangements || this.fileType == FSExplorer.FileTypes.DocumentSets || this.fileType == FSExplorer.FileTypes.TaskSets || this.fileType == FSExplorer.FileTypes.DataTemplates || this.fileType == FSExplorer.FileTypes.LoanTemplates) && e.EditorControl.Text.IndexOf("/") > -1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A folder name cannot contain the \"/\" character.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        e.Cancel = true;
        e.EditorControl.Text = tag2.Name;
      }
      else
      {
        int length = e.EditorControl.Text.IndexOf(".");
        if (length > -1 && e.EditorControl.Text.Substring(0, length).Trim() == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must type a file name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          e.EditorControl.Text = tag2.Name;
        }
        else if (FileSystemEntry.Types.File == ((FileSystemEntry) parent.Tag).Type && this.maxFileNameLength < e.EditorControl.Text.Trim().Length)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "File name is limited to " + (object) this.maxFileNameLength + " characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
          e.EditorControl.Text = tag2.Name;
        }
        else
        {
          string name = tag2.Name;
          string str = e.EditorControl.Text.Trim();
          string oldName = "Public:" + tag2.Path;
          string newName = name.IndexOf(".") <= 0 ? this.ConstructFilePath(str, tag2.Type, tag2.ParentFolder.Path, "") : this.ConstructFilePath(str, tag2.Type, tag2.ParentFolder.Path, name.Substring(name.IndexOf(".")));
          if (str == "")
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "A file or folder name cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            e.Cancel = true;
            e.EditorControl.Text = tag2.Name;
          }
          else
          {
            if (!this.displayExtension && tag2.Type == FileSystemEntry.Types.File)
              str += this.getFileExtension(name);
            if (string.Compare(name, str, false) == 0)
              return;
            if (!this.hasPublicRight && this.IsRootPublic)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to rename this item.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              e.EditorControl.Text = tag2.Name;
            }
            else
            {
              string lower = e.EditorControl.Text.Trim().ToLower();
              for (int index = 0; index < this.currFSEntries.Length; ++index)
              {
                if (this.currFSEntries[index] != tag2 && this.getDisplayName(this.currFSEntries[index]).ToLower() == lower)
                {
                  int num = (int) Utils.Dialog((IWin32Window) this, "The selected item cannot be renamed because an item in this folder already exists with the specified name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                  e.Cancel = true;
                  e.EditorControl.Text = tag2.Name;
                  return;
                }
              }
              this.cutCopyEntries = (FileSystemEntry[]) null;
              this.cutCopyFolder = (FileSystemEntry) null;
              FileSystemEntry targetEntry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, str), tag2.Type, tag2.Owner);
              targetEntry.Access = tag2.Access;
              try
              {
                this.IfsExplorer.MoveEntry(tag2, targetEntry);
              }
              catch (Exception ex)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Handled = true;
                return;
              }
              if (targetEntry.Properties.Count == 0)
                targetEntry.Properties = tag2.Properties;
              parent.Tag = (object) targetEntry;
              ArrayList arrayList = new ArrayList((ICollection) this.currFSEntries);
              arrayList.Remove((object) tag2);
              arrayList.Add((object) targetEntry);
              this.currFSEntries = (FileSystemEntry[]) arrayList.ToArray(typeof (FileSystemEntry));
              if (targetEntry.Type == FileSystemEntry.Types.Folder && this.AfterFolderRenamed != null)
                this.AfterFolderRenamed((object) this, (EventArgs) e);
              if (this.AfterFileRenamed != null && tag2.IsPublic)
                this.AfterFileRenamed((object) new FileSystemEntry[2]
                {
                  tag2,
                  targetEntry
                }, EventArgs.Empty);
              if (!this.currentFolder.IsPublic)
                return;
              if (tag2.Type == FileSystemEntry.Types.Folder)
                this.session.AclGroupManager.UpdateFileResource(oldName, newName, (int) this.GetAclFileType(), true);
              else
                this.session.AclGroupManager.UpdateFileResource(oldName, newName, (int) this.GetAclFileType(), false);
            }
          }
        }
      }
    }

    private string getDisplayName(FileSystemEntry entry)
    {
      return entry.Type == FileSystemEntry.Types.Folder || this.displayExtension ? entry.Name : this.removeFileExtension(entry.Name);
    }

    private void menuItemRename_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh && !this.displayFolderOperationButtonsOnly || this.gvDirectory.SelectedItems.Count == 0)
        return;
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected folder cannot be renamed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        GVItem selectedItem = this.gvDirectory.SelectedItems[0];
        if (selectedItem == null || selectedItem.Tag.ToString() == "")
          return;
        if (((FileSystemEntry) selectedItem.Tag).Type != FileSystemEntry.Types.Folder && this.displayFolderOperationButtonsOnly)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You only can rename folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          selectedItem.BeginEdit();
          this.ResetOption();
        }
      }
    }

    private void gvDirectory_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button != MouseButtons.Right)
        return;
      this.rightMouseX = e.X;
      this.rightMouseY = e.Y;
    }

    private void saveSelectedItems()
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
        this.cutCopyEntries = (FileSystemEntry[]) null;
      this.cutCopyRoot = this.currRoot;
      this.cutCopyFolder = this.currentFolder;
      this.cutCopyEntries = !(this.gvDirectory.SelectedItems[0].Tag.ToString() == "") ? new FileSystemEntry[this.gvDirectory.SelectedItems.Count] : new FileSystemEntry[this.gvDirectory.SelectedItems.Count - 1];
      this.cutCopyEntries = new FileSystemEntry[this.gvDirectory.SelectedItems.Count];
      for (int index = 0; index < this.gvDirectory.SelectedItems.Count; ++index)
      {
        if (!(this.gvDirectory.SelectedItems[index].Tag.ToString() == ""))
        {
          FileSystemEntry tag = (FileSystemEntry) this.gvDirectory.SelectedItems[index].Tag;
          this.cutCopyEntries[index] = tag;
        }
      }
    }

    private void menuItemCut_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh)
        return;
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected folder cannot be moved.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.gvDirectory.SelectedItems.Count < 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select one file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.isCut = true;
        this.saveSelectedItems();
        this.ResetOption();
        if (!this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite)
          this.MakeEmptySelectionReadWrite();
        this.setTitle();
      }
    }

    private void menuItemCopy_Click(object sender, EventArgs e)
    {
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "The selected folder cannot be copied.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (this.gvDirectory.SelectedItems.Count < 1)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "You have to select one file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.isCut = false;
        this.saveSelectedItems();
        if (this.doNotHaveRightToPersonalFolder() && this.fileType != FSExplorer.FileTypes.PurchaseAdvice && this.fileType != FSExplorer.FileTypes.FundingTemplate)
        {
          this.MakeEmptySelectionReadWrite();
          this.MakeFileReadOnly();
          this.MakeFolderReadOnly();
        }
        else
        {
          if (!this.hasPublicRight && this.IsRootPublic)
            this.btnPaste.Enabled = this.menuItemPaste.Enabled = false;
          else
            this.btnPaste.Enabled = this.menuItemPaste.Enabled = true;
          this.updateButtons();
        }
      }
    }

    private bool copyOrCut(FileSystemEntry source, FileSystemEntry target, bool isCut)
    {
      if (!this.IfsExplorer.EntryExistsOfAnyType(source))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot find the source file " + source.Name + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.btnPaste.Enabled = this.menuItemPaste.Enabled = false;
        return false;
      }
      target = new FileSystemEntry(target.Path, source.Name, source.Type, target.Owner);
      if (source.Equals((object) target) && this.isCut)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to perform paste operation because the destination is the same as the source.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if (source.Equals((object) target))
      {
        FileSystemEntry entry = new FileSystemEntry(target.ParentFolder.Path + "Copy of " + target.Name, target.Type, target.Owner);
        int num = 2;
        while (this.IfsExplorer.EntryExistsOfAnyType(entry))
        {
          entry = new FileSystemEntry(target.ParentFolder.Path + "Copy of (" + (object) num + ") " + target.Name, target.Type, target.Owner);
          ++num;
        }
        target = entry;
      }
      if (this.IfsExplorer.EntryExistsOfAnyType(target))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot create or replace '" + target.Name + "'. There is already a file or folder with the same name.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (source.Type == FileSystemEntry.Types.Folder && target.ToString().ToLower().IndexOf(source.ToString().ToLower()) >= 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Cannot move or copy the folder '" + source.Name + "' because the destination is a subdirectory of the source.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        if (this.isCut)
        {
          this.FileActionConfirmed = true;
          if (this.BeforeFileMoved != null)
            this.BeforeFileMoved((object) new object[3]
            {
              (object) this,
              (object) source,
              (object) target
            }, EventArgs.Empty);
          if (this.FileActionConfirmed)
          {
            this.IfsExplorer.MoveEntry(source, target);
            if (this.AfterFileMoved != null)
              this.AfterFileMoved((object) new FileSystemEntry[2]
              {
                source,
                target
              }, EventArgs.Empty);
          }
        }
        else
        {
          this.IfsExplorer.CopyEntry(source, target);
          if (this.AfterFileCopied != null)
            this.AfterFileCopied((object) new FileSystemEntry[2]
            {
              source,
              target
            }, EventArgs.Empty);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.setTitle();
      return true;
    }

    private void menuItemPaste_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh)
        return;
      if (this.cutCopyEntries == null || this.cutCopyFolder == null)
      {
        this.btnPaste.Enabled = this.menuItemPaste.Enabled = false;
      }
      else
      {
        FileSystemEntry target = (FileSystemEntry) null;
        GVItem gvItem = (GVItem) null;
        if (this.gvDirectory.SelectedItems.Count == 0)
        {
          gvItem = this.gvDirectory.GetItemAt(this.rightMouseX, this.rightMouseY);
        }
        else
        {
          target = (FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag;
          if (target.Type == FileSystemEntry.Types.File)
            target = this.currentFolder;
        }
        if (target == null)
          target = this.currentFolder;
        if (true)
        {
          int index = 0;
          while (index < this.cutCopyEntries.Length && this.copyOrCut(this.cutCopyEntries[index], target, this.isCut))
            ++index;
        }
        if (this.isCut)
        {
          this.cutCopyEntries = (FileSystemEntry[]) null;
          this.cutCopyFolder = (FileSystemEntry) null;
          this.btnPaste.Enabled = this.menuItemPaste.Enabled = false;
        }
        this.menuItemRefresh_Click((object) this, (EventArgs) null);
        this.ResetOption();
        if (((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionalApprovalLetter) || !this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite)
          this.MakeEmptySelectionReadWrite();
        this.setTitle();
      }
    }

    private void menuItemNew_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh)
        return;
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "New files or folders cannot be added to the current folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string newDocBaseName = this.IfsExplorer.NewDocBaseName;
        string newDocExtension = this.displayExtension ? "" : this.IfsExplorer.NewDocExtension;
        FileSystemEntry entry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, newDocBaseName + newDocExtension), FileSystemEntry.Types.File, this.isRootPublic ? (string) null : this.session.UserID);
        int num2 = 2;
        while (this.IfsExplorer.EntryExistsOfAnyType(entry))
        {
          entry = new FileSystemEntry(SystemUtil.CombinePath(this.currentFolder.Path, newDocBaseName + " (" + (object) num2 + ")" + newDocExtension), FileSystemEntry.Types.File, this.isRootPublic ? (string) null : this.session.UserID);
          ++num2;
        }
        if (!this.IfsExplorer.CreateNew(entry) || FSExplorer.FileTypes.CampaignTemplates == this.fileType)
          return;
        this.currFSEntries = this.IfsExplorer.GetFileSystemEntries(this.currentFolder);
        foreach (FileSystemEntry currFsEntry in this.currFSEntries)
        {
          if (currFsEntry.Path == entry.Path)
          {
            entry = currFsEntry;
            break;
          }
        }
        GVItem gvItem = new GVItem(this.getDisplayName(entry));
        if (this.templateType != FSExplorer.TemplateSettingsType.Nothing)
        {
          if ((this.templateType == FSExplorer.TemplateSettingsType.ClosingCost || this.templateType == FSExplorer.TemplateSettingsType.MiscData || this.fileType == FSExplorer.FileTypes.FundingTemplate) && this.gvDirectory.Columns.Count > 2)
          {
            if (this.templateType == FSExplorer.TemplateSettingsType.MiscData)
            {
              if (entry.Properties.ContainsKey((object) "RESPAVERSION"))
                gvItem.SubItems.Add(entry.Properties[(object) "RESPAVERSION"].ToString() == "2009" ? (object) "Old" : (object) entry.Properties[(object) "RESPAVERSION"].ToString());
              else
                gvItem.SubItems.Add((object) "");
            }
            else if (this.templateType == FSExplorer.TemplateSettingsType.ClosingCost && entry.Properties.ContainsKey((object) "RESPAVERSION"))
              gvItem.SubItems.Add(entry.Properties[(object) "RESPAVERSION"].ToString() == "2015" ? (object) "Yes" : (object) "No");
            else
              gvItem.SubItems.Add((object) string.Concat(entry.Properties[(object) "For2010GFE"]));
          }
          else
            gvItem.SubItems.Add((object) "");
        }
        gvItem.ImageIndex = this.getFileImageIcon();
        gvItem.Tag = (object) entry;
        this.gvDirectory.Items.Add(gvItem);
        gvItem.BeginEdit();
        this.setTitle();
      }
    }

    private void cmbBoxFolder_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxFolder.SelectedItem == null)
        return;
      string fullPath = ((CmbBoxItem) this.cmbBoxFolder.SelectedItem).FullPath;
      try
      {
        FSExplorer.RootType rootType = this.getRootType(fullPath);
        this.changeFolder(this.getFolder(fullPath), rootType);
      }
      catch (ApplicationException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      this.ResetOption();
      if (this.doNotHaveRightToPersonalFolder())
      {
        this.MakeFileReadOnly();
        this.MakeFolderReadOnly();
      }
      else if (!this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite)
        this.MakeEmptySelectionReadWrite();
      else if (this.currentFolder.IsPublic && this.currentFolder.Access == AclResourceAccess.ReadOnly)
        this.MakeFolderReadOnly();
      else
        this.MakeFolderReadWrite();
      this.setButtonsForReports();
    }

    private void setButtonsForUserRight()
    {
      if (this.session == null || !this.session.IsConnected)
        return;
      this.ResetOption();
      if (!UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) && (this.fileType == FSExplorer.FileTypes.CustomForms || this.fileType == FSExplorer.FileTypes.CustomLetters || this.fileType == FSExplorer.FileTypes.PrintGroups))
        this.btnOpen.Enabled = this.menuItemOpen.Enabled = false;
      else
        this.btnOpen.Enabled = this.menuItemOpen.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      if (this.currentFolder.IsPublic && this.fileType != FSExplorer.FileTypes.FundingTemplate && this.fileType != FSExplorer.FileTypes.PurchaseAdvice && this.fileType != FSExplorer.FileTypes.LoanDuplicationTemplate)
      {
        if (this.currentFolder.Type != FileSystemEntry.Types.Folder)
        {
          if (this.currentFolder.Access != AclResourceAccess.ReadWrite)
            return;
          this.MakeFileReadWrite();
        }
        else
        {
          if (this.currentFolder.Type != FileSystemEntry.Types.Folder || this.currentFolder.Access != AclResourceAccess.ReadWrite)
            return;
          this.MakeFolderReadWrite();
        }
      }
      else
      {
        this.MakeFileReadWrite();
        this.MakeFolderReadWrite();
      }
    }

    private void ResetOption()
    {
      this.btnOpen.Enabled = false;
      this.menuItemOpen.Enabled = false;
      this.btnCut.Enabled = false;
      this.menuItemCut.Enabled = false;
      this.btnCopy.Enabled = false;
      this.menuItemCopy.Enabled = false;
      this.btnPaste.Enabled = false;
      this.menuItemPaste.Enabled = false;
      this.btnDelete.Enabled = false;
      this.menuItemDelete.Enabled = false;
      this.btnRename.Enabled = false;
      this.menuItemRename.Enabled = false;
      this.btnDuplicate.Enabled = false;
      this.menuItemDuplicate.Enabled = false;
      this.btnImport.Enabled = false;
      this.menuItemImport.Enabled = false;
      this.btnDeploy.Enabled = false;
      this.menuItemDeploy.Enabled = false;
      this.btnExport.Enabled = false;
      this.menuItemExport.Enabled = false;
    }

    private void MakeFolderReadWrite()
    {
      if (this.currentFolder.Access == AclResourceAccess.ReadWrite)
      {
        this.btnAdd.Enabled = this.menuItemNew.Enabled = true;
        this.btnDuplicate.Enabled = this.menuItemDuplicate.Enabled = true;
      }
      if ((this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate) && !this.canCreateEdit)
        this.btnOpen.Enabled = this.menuItemOpen.Enabled = false;
      else
        this.btnOpen.Enabled = this.menuItemOpen.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      this.btnCut.Enabled = this.menuItemCut.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      this.btnCopy.Enabled = this.menuItemCopy.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      this.btnRename.Enabled = this.menuItemRename.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      if (this.cutCopyEntries != null)
      {
        this.btnPaste.Enabled = true;
        this.menuItemPaste.Enabled = true;
      }
      switch (this.fileType)
      {
        case FSExplorer.FileTypes.Reports:
        case FSExplorer.FileTypes.DashboardTemplate:
        case FSExplorer.FileTypes.DashboardViewTemplate:
        case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
          this.btnDelete.Enabled = this.menuItemDelete.Enabled = this.gvDirectory.SelectedItems.Count == 1;
          break;
        case FSExplorer.FileTypes.LoanDuplicationTemplate:
          this.btnDelete.Enabled = this.menuItemDelete.Enabled = this.gvDirectory.SelectedItems.Count == 1;
          break;
        default:
          this.btnDelete.Enabled = this.menuItemDelete.Enabled = this.gvDirectory.SelectedItems.Count > 0;
          break;
      }
    }

    private void MakeEmptySelectionReadWrite()
    {
      this.btnDelete.Enabled = this.btnCut.Enabled = this.btnCopy.Enabled = this.btnRename.Enabled = false;
      this.menuItemDelete.Enabled = this.menuItemCut.Enabled = this.menuItemCopy.Enabled = this.menuItemRename.Enabled = false;
      if ((this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate) && !this.canCreateEdit)
      {
        this.btnAdd.Enabled = false;
        this.menuItemNew.Enabled = false;
      }
      else
      {
        this.btnAdd.Enabled = true;
        this.menuItemNew.Enabled = true;
      }
      if (this.cutCopyEntries != null)
      {
        this.btnPaste.Enabled = true;
        this.menuItemPaste.Enabled = true;
      }
      this.btnNewFolder.Enabled = true;
      this.menuItemCreateFolder.Enabled = true;
      this.btnImport.Enabled = true;
      this.menuItemImport.Enabled = true;
    }

    private void MakeFileReadWrite()
    {
      if ((this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate) && !this.canCreateEdit)
      {
        this.btnOpen.Enabled = false;
        this.menuItemOpen.Enabled = false;
      }
      else
        this.btnOpen.Enabled = this.menuItemOpen.Enabled = this.btnOpen.Visible && this.gvDirectory.SelectedItems.Count == 1;
      this.btnCut.Enabled = this.menuItemCut.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      this.btnCopy.Enabled = this.menuItemCopy.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      switch (this.fileType)
      {
        case FSExplorer.FileTypes.Reports:
        case FSExplorer.FileTypes.DashboardTemplate:
        case FSExplorer.FileTypes.DashboardViewTemplate:
        case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
          this.btnDelete.Enabled = this.menuItemDelete.Enabled = this.gvDirectory.SelectedItems.Count == 1;
          break;
        case FSExplorer.FileTypes.LoanDuplicationTemplate:
          this.btnDelete.Enabled = this.menuItemDelete.Enabled = this.gvDirectory.SelectedItems.Count == 1;
          break;
        default:
          this.btnDelete.Enabled = this.menuItemDelete.Enabled = this.gvDirectory.SelectedItems.Count > 0;
          break;
      }
      this.btnRename.Enabled = this.menuItemRename.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      this.btnImport.Enabled = true;
      this.menuItemImport.Enabled = true;
      if (this.btnAdd.Enabled)
        this.btnDuplicate.Enabled = this.menuItemDuplicate.Enabled = this.gvDirectory.SelectedItems.Count == 1;
      this.btnDeploy.Enabled = true;
      this.menuItemDeploy.Enabled = true;
      this.btnExport.Enabled = true;
      this.menuItemExport.Enabled = true;
    }

    private void MakeFileReadOnly()
    {
      if (this.fileType == FSExplorer.FileTypes.CustomForms || this.fileType == FSExplorer.FileTypes.CustomLetters)
      {
        this.btnOpen.Enabled = false;
        this.menuItemOpen.Enabled = false;
      }
      if (this.gvDirectory.SelectedItems.Count <= 0)
        return;
      this.btnCopy.Enabled = this.menuItemCopy.Enabled = true;
    }

    private void MakeFolderReadOnly()
    {
      this.btnAdd.Enabled = this.btnNewFolder.Enabled = this.btnDelete.Enabled = this.btnDuplicate.Enabled = this.btnCut.Enabled = this.btnPaste.Enabled = false;
      this.menuItemNew.Enabled = this.menuItemCreateFolder.Enabled = this.menuItemDelete.Enabled = this.menuItemDuplicate.Enabled = this.menuItemCut.Enabled = this.menuItemPaste.Enabled = false;
      if (this.gvDirectory.SelectedItems.Count <= 0)
        return;
      this.btnCopy.Enabled = this.menuItemCopy.Enabled = true;
    }

    private void addCmbBoxFolderItems(string startFullPath, string startSpaces, bool isPublic)
    {
      string fullPath = startFullPath;
      string str = startSpaces;
      string[] strArray = this.currentFolder.Path.Split('\\');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (!(strArray[index] == ""))
        {
          fullPath = SystemUtil.CombinePath(fullPath, strArray[index]);
          str += "    ";
          this.cmbBoxFolder.Items.Add((object) new CmbBoxItem(fullPath, strArray[index], isPublic, str.Length * 4));
        }
      }
    }

    private void refreshFolderCombo(string selectedFullPath)
    {
      this.suspendFolderComboIdxChangedEvent();
      try
      {
        this.cmbBoxFolder.Items.Clear();
        if (this.currRoot == FSExplorer.RootType.PublicOnly)
        {
          string str = "\\\\" + this.getRootName(true);
          this.cmbBoxFolder.Items.Add((object) new CmbBoxItem(str, this.getRootName(true), true, 0));
          this.addCmbBoxFolderItems(str, "", true);
        }
        else
        {
          string str1 = "\\\\" + this.getRootName(true);
          this.cmbBoxFolder.Items.Add((object) new CmbBoxItem(str1, this.getRootName(true), true, 0));
          if (this.currRoot == FSExplorer.RootType.Public)
            this.addCmbBoxFolderItems(str1, "", true);
          string str2 = "\\\\" + this.getRootName(false);
          this.cmbBoxFolder.Items.Add((object) new CmbBoxItem(str2, this.getRootName(false), false, 0));
          if (this.currRoot == FSExplorer.RootType.Private)
            this.addCmbBoxFolderItems(str2, "", false);
        }
        if (selectedFullPath == null)
          return;
        for (int index = 0; index < this.cmbBoxFolder.Items.Count; ++index)
        {
          string trimedFullPath = ((CmbBoxItem) this.cmbBoxFolder.Items[index]).TrimedFullPath;
          selectedFullPath = selectedFullPath.TrimEnd('\\');
          if (string.Compare(trimedFullPath, selectedFullPath, StringComparison.OrdinalIgnoreCase) == 0)
          {
            this.cmbBoxFolder.SelectedIndex = index;
            break;
          }
        }
      }
      finally
      {
        this.resumeFolderComboIdxChangedEvent();
      }
    }

    private void gvDirectory_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar == '\u0001')
      {
        for (int nItemIndex = 0; nItemIndex < this.gvDirectory.Items.Count; ++nItemIndex)
          this.gvDirectory.Items[nItemIndex].Selected = true;
      }
      else if (e.KeyChar == '\u0003' && this.menuItemCopy.Enabled)
        this.menuItemCopy_Click((object) null, (EventArgs) null);
      else if (e.KeyChar == '\u0004')
      {
        if (!this.currentFolder.IsPublic)
          return;
        bool flag = true;
        foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
        {
          if (((FileSystemEntry) selectedItem.Tag).Access == AclResourceAccess.ReadOnly)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights for the selected files to perform this function.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.menuItemDelete_Click((object) null, (EventArgs) null);
      }
      else if (e.KeyChar == '\u0016' && this.menuItemPaste.Enabled)
      {
        if ((!this.hasPublicRight && !this.IsRootPublic || this.hasPublicRight) && !this.disableAllMenuItemsExceptRefresh)
        {
          this.menuItemPaste_Click((object) null, (EventArgs) null);
        }
        else
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to perform this function.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else
      {
        if (e.KeyChar != '\u0018' || !this.currentFolder.IsPublic)
          return;
        bool flag = true;
        foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
        {
          if (((FileSystemEntry) selectedItem.Tag).Access == AclResourceAccess.ReadOnly)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights for the selected files to perform this function.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.menuItemCut_Click((object) null, (EventArgs) null);
      }
    }

    private void menuItemImport_Click(object sender, EventArgs e)
    {
      if (this.currRoot == FSExplorer.RootType.Top)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You cannot import to the current folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        FileSystemEntry fileEntry = (FileSystemEntry) null;
        GVItem gvItem = (GVItem) null;
        if (this.gvDirectory.SelectedItems.Count != 0)
        {
          gvItem = this.gvDirectory.SelectedItems[0];
          fileEntry = (FileSystemEntry) gvItem.Tag;
          if (fileEntry.Type == FileSystemEntry.Types.File)
            gvItem = (GVItem) null;
        }
        if (gvItem == null)
          fileEntry = this.currentFolder;
        this.IfsExplorer.Import(fileEntry);
        if (gvItem == null)
          this.menuItemRefresh_Click((object) this, (EventArgs) null);
        this.updateButtons();
      }
    }

    private void updateButtons()
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
      {
        this.btnRename.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnCopy.Enabled = false;
        this.btnPaste.Enabled = this.cutCopyEntries != null && (this.hasPublicRight || !this.IsRootPublic);
        this.btnCut.Enabled = false;
        this.btnOpen.Enabled = false;
        this.btnDuplicate.Enabled = false;
        this.btnDeploy.Enabled = false;
        this.btnExport.Enabled = false;
      }
      else
        this.setButtonsForUserRight();
    }

    public void SelectGridViewFormGroup(string itemText)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvDirectory.Items.Count; ++nItemIndex)
      {
        GVItem gvItem = this.gvDirectory.Items[nItemIndex];
        if ((gvItem.ImageIndex == 3 || gvItem.ImageIndex == 4) && itemText == gvItem.Text)
        {
          if (!gvItem.Selected)
          {
            gvItem.Selected = true;
            break;
          }
          this.gvDirectory_SelectedIndexChanged((object) this, (EventArgs) null);
          break;
        }
      }
    }

    private void gvDirectory_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.fileType == FSExplorer.FileTypes.MilestoneTemplate || this.fileType == FSExplorer.FileTypes.SyncTemplates)
        return;
      this.ResetOption();
      bool flag1 = false;
      bool flag2 = false;
      if (this.currentFolder.IsPublic)
      {
        bool flag3 = true;
        foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
        {
          FileSystemEntry tag = (FileSystemEntry) selectedItem.Tag;
          if (tag.Access == AclResourceAccess.ReadOnly)
            flag3 = false;
          if (tag.Type == FileSystemEntry.Types.File)
            flag1 = true;
          else if (tag.Type == FileSystemEntry.Types.Folder)
            flag2 = true;
        }
        if (flag3)
        {
          if (this.gvDirectory.SelectedItems.Count > 0)
          {
            if (flag2 && !flag1)
              this.MakeFolderReadWrite();
            else if (flag1 && !flag2)
            {
              this.MakeFileReadWrite();
            }
            else
            {
              this.MakeFileReadWrite();
              this.MakeFolderReadWrite();
            }
          }
          else if (this.currentFolder.Access == AclResourceAccess.ReadWrite || this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.FundingTemplate || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate)
            this.MakeEmptySelectionReadWrite();
        }
        else if (this.gvDirectory.SelectedItems.Count > 0)
        {
          if (flag2 && !flag1)
            this.MakeFolderReadOnly();
          else if (flag1 && !flag2)
          {
            this.MakeFileReadOnly();
          }
          else
          {
            this.MakeFolderReadWrite();
            this.MakeFileReadWrite();
          }
        }
        else if (this.currentFolder.Access == AclResourceAccess.ReadWrite)
          this.MakeEmptySelectionReadWrite();
      }
      else
      {
        if (this.doNotHaveRightToPersonalFolder() && this.fileType != FSExplorer.FileTypes.PurchaseAdvice && this.fileType != FSExplorer.FileTypes.FundingTemplate)
        {
          this.MakeEmptySelectionReadWrite();
          this.MakeFileReadOnly();
          this.MakeFolderReadOnly();
          if (this.SelectedEntryChanged == null)
            return;
          this.SelectedEntryChanged(sender, e);
          return;
        }
        if (this.gvDirectory.SelectedItems.Count > 0)
        {
          foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
          {
            FileSystemEntry tag = (FileSystemEntry) selectedItem.Tag;
            if (tag.Type == FileSystemEntry.Types.File)
              flag1 = true;
            else if (tag.Type == FileSystemEntry.Types.Folder)
              flag2 = true;
          }
          if (flag2 && !flag1)
            this.MakeFolderReadWrite();
          else if (flag1 && !flag2)
          {
            this.MakeFileReadWrite();
          }
          else
          {
            this.MakeFileReadWrite();
            this.MakeFolderReadWrite();
          }
        }
        else
          this.MakeEmptySelectionReadWrite();
      }
      if (this.SelectedEntryChanged == null)
        return;
      this.SelectedEntryChanged(sender, e);
    }

    private void gvDirectory_Resize(object sender, EventArgs e)
    {
      try
      {
        if (this.templateType != FSExplorer.TemplateSettingsType.Nothing)
        {
          if (this.gvDirectory.Columns.Contains(this.descHeader))
          {
            this.gvDirectory.Columns[0].Width = (int) ((double) this.gvDirectory.Width * 0.7);
            if (this.gvDirectory.Columns.Count > 2)
              this.gvDirectory.Columns[2].Width = (int) ((double) this.gvDirectory.Width * 0.5);
            else
              this.gvDirectory.Columns[1].Width = (int) ((double) this.gvDirectory.Width * 0.5);
          }
          else
            this.gvDirectory.Columns[0].Width = (int) ((double) this.gvDirectory.Width * 0.9);
        }
        else
          this.gvDirectory.Columns[0].Width = (int) ((double) this.gvDirectory.Width * 0.8);
      }
      catch
      {
      }
    }

    private string getRootName(bool isPublic)
    {
      string str = string.Empty;
      switch (this.fileType)
      {
        case FSExplorer.FileTypes.PrintGroups:
          str = " Forms Groups";
          break;
        case FSExplorer.FileTypes.CustomForms:
          str = " Custom Forms";
          break;
        case FSExplorer.FileTypes.CustomLetters:
          str = " Custom Letters";
          break;
        case FSExplorer.FileTypes.Reports:
          str = " Reports";
          break;
        case FSExplorer.FileTypes.LoanTemplates:
          str = " Loan Templates";
          break;
        case FSExplorer.FileTypes.DataTemplates:
          str = " Data Templates";
          break;
        case FSExplorer.FileTypes.LoanPrograms:
          str = " Loan Programs";
          break;
        case FSExplorer.FileTypes.ClosingCosts:
          str = " Closing Cost Templates";
          break;
        case FSExplorer.FileTypes.DocumentSets:
          str = " Document Sets";
          break;
        case FSExplorer.FileTypes.FormLists:
          str = " Form Lists";
          break;
        case FSExplorer.FileTypes.StackingOrderSets:
          str = " Stacking Order Templates";
          break;
        case FSExplorer.FileTypes.CampaignTemplates:
          str = " Campaign Templates";
          break;
        case FSExplorer.FileTypes.DashboardTemplate:
          str = " Dashboard Templates";
          break;
        case FSExplorer.FileTypes.TaskSets:
          str = " Task Sets";
          break;
        case FSExplorer.FileTypes.SettlementServiceProviders:
          str = " Settlement Service Providers";
          break;
        case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
          str = " Affiliates";
          break;
      }
      return isPublic ? "Public" + str : "Personal" + str;
    }

    private bool isRootTypePublic(FSExplorer.RootType type) => type != FSExplorer.RootType.Private;

    private bool isRootPublic => this.isRootTypePublic(this.currRoot);

    private bool isCutCopyRootPublic => this.isRootTypePublic(this.cutCopyRoot);

    public IFSExplorerBase IfsExplorer
    {
      get
      {
        if (this._ifsExplorer == null)
          this._ifsExplorer = (IFSExplorerBase) this.Parent;
        return this._ifsExplorer;
      }
    }

    private string getFileExtension(string file)
    {
      int startIndex = file.LastIndexOf(".");
      return startIndex == -1 ? string.Empty : file.Substring(startIndex);
    }

    public void RenameFormGroupItem(GVItem lvItem) => lvItem?.BeginEdit();

    public void DeleteFormGroupItem() => this.menuItemDelete_Click((object) null, (EventArgs) null);

    public void MakeItemSelected(AclFileType type, string[] fileTypePathList)
    {
      List<string> stringList = new List<string>((IEnumerable<string>) fileTypePathList);
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
      {
        FileSystemEntry tag = (FileSystemEntry) gvItem.Tag;
        AclFileResource aclFileResource = new AclFileResource(-1, tag.ToString(), type, tag.Type == FileSystemEntry.Types.Folder, tag.Owner);
        gvItem.Selected = stringList.Contains(aclFileResource.FileTypePath);
      }
    }

    public void RefreshFormGroup(string groupName)
    {
      this.menuItemRefresh_Click((object) null, (EventArgs) null);
      if (!(groupName != string.Empty))
        return;
      for (int nItemIndex = 0; nItemIndex < this.gvDirectory.Items.Count; ++nItemIndex)
      {
        FileSystemEntry tag = (FileSystemEntry) this.gvDirectory.Items[nItemIndex].Tag;
        if (tag.Type == FileSystemEntry.Types.File && string.Compare(groupName, tag.Name, StringComparison.OrdinalIgnoreCase) == 0)
        {
          this.gvDirectory.Items[nItemIndex].Selected = true;
          break;
        }
      }
    }

    public void SetFolder(FileSystemEntry target)
    {
      FSExplorer.RootType rootType = FSExplorer.RootType.Public;
      if (!target.IsPublic)
        rootType = FSExplorer.RootType.Private;
      if (target.Type == FileSystemEntry.Types.File)
      {
        this.changeFolder(target.ParentFolder.Path, rootType);
        for (int nItemIndex = 0; nItemIndex < this.gvDirectory.Items.Count; ++nItemIndex)
        {
          if (((FileSystemEntry) this.gvDirectory.Items[nItemIndex].Tag).Equals((object) target))
          {
            this.gvDirectory.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
      else
        this.changeFolder(target.Path, rootType);
    }

    public GVSelectedItemCollection SelectedItems => this.gvDirectory.SelectedItems;

    public bool IsTopFolder => this.currRoot == FSExplorer.RootType.Top;

    public bool IsRootPublic => this.currRoot != FSExplorer.RootType.Private;

    public FileSystemEntry CurrentFolder
    {
      get => this.IsTopFolder ? (FileSystemEntry) null : this.currentFolder;
    }

    public FileSystemEntry[] CurrFSEntries => this.currFSEntries;

    public void SetFocusToFileListView() => this.gvDirectory.Focus();

    public bool HideDescription
    {
      set
      {
        if (value)
        {
          if (this.gvDirectory.Columns.Contains(this.descHeader))
            this.gvDirectory.Columns.Remove(this.descHeader);
          this.gvDirectory.Columns[0].SpringToFit = true;
        }
        else
        {
          if (!this.gvDirectory.Columns.Contains(this.descHeader))
            this.gvDirectory.Columns.Add(this.descHeader);
          this.gvDirectory.Columns[0].SpringToFit = false;
        }
        this.gvDirectory_Resize((object) null, (EventArgs) null);
      }
    }

    public void AdjustLastColumnSprintToFit(bool springToFit)
    {
      this.gvDirectory.Columns[this.gvDirectory.Columns.Count - 1].SpringToFit = springToFit;
    }

    public void HideContextMenu() => this.gvDirectory.ContextMenu = (ContextMenu) null;

    public void HideOpenMenu()
    {
      this.btnOpen.Visible = false;
      this.btnOpen.Enabled = false;
      this.menuItemOpen.Visible = false;
      this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnCut, (object) this.btnCopy, (object) this.btnPaste, (object) this.btnOpen, (object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename, (object) this.btnDeploy, (object) this.btnImport, (object) this.btnExport);
    }

    public bool DisplayFolderOperationButtonsOnly
    {
      set
      {
        this.displayFolderOperationButtonsOnly = value;
        if (!this.displayFolderOperationButtonsOnly)
          return;
        this.showOnlyFolderOperationButtons();
      }
    }

    private void showOnlyFolderOperationButtons()
    {
      this.initializeButtons();
      this.arrangeButtons((object) this.btnNewFolder, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename);
      this.menuItemCreateFolder.Visible = true;
      this.menuItemRename.Visible = true;
      this.menuItemNew.Visible = false;
      this.menuItemOpen.Visible = false;
      this.menuItemCut.Visible = false;
      this.menuItemCopy.Visible = false;
      this.menuItemPaste.Visible = false;
      this.menuItemImport.Visible = false;
    }

    public void Menu_Click(FSExplorer.MenuFunctionTypes func)
    {
      switch (func)
      {
        case FSExplorer.MenuFunctionTypes.CreateFolder:
          this.menuItemCreateFolder_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.AddNewFile:
          this.menuItemNew_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.OpenFolderOrFile:
          this.menuItemOpen_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.CutFolderOrFile:
          this.menuItemCut_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.CopyFolderOrFile:
          this.menuItemCopy_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.PasteFolderOrFile:
          this.menuItemPaste_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.DeleteFolderOrFile:
          this.menuItemDelete_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.RenameFolderOrFile:
          this.menuItemRename_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.ImportFile:
          this.menuItemImport_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.RefreshFolder:
          this.menuItemRefresh_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.DuplicateFile:
          this.menuItemDuplicate_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.DeployFile:
          this.menuItemDeploy_Click((object) null, (EventArgs) null);
          break;
        case FSExplorer.MenuFunctionTypes.ExportFile:
          this.menuItemExport_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public bool IsMenuItemEnabled(FSExplorer.MenuFunctionTypes func)
    {
      bool flag = false;
      switch (func)
      {
        case FSExplorer.MenuFunctionTypes.CreateFolder:
          flag = this.btnNewFolder.Enabled || this.menuItemCreateFolder.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.AddNewFile:
          flag = this.btnAdd.Enabled || this.menuItemNew.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.OpenFolderOrFile:
          flag = this.btnOpen.Enabled || this.menuItemOpen.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.CutFolderOrFile:
          flag = this.btnCut.Enabled || this.menuItemCut.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.CopyFolderOrFile:
          flag = this.btnCopy.Enabled || this.menuItemCopy.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.PasteFolderOrFile:
          flag = this.btnPaste.Enabled || this.menuItemPaste.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.DeleteFolderOrFile:
          flag = this.btnDelete.Enabled || this.menuItemDelete.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.RenameFolderOrFile:
          flag = this.btnRename.Enabled || this.menuItemRename.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.ImportFile:
          flag = this.btnImport.Enabled || this.menuItemImport.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.RefreshFolder:
          flag = this.menuItemRefresh.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.DuplicateFile:
          flag = this.btnDuplicate.Enabled || this.menuItemDuplicate.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.DeployFile:
          flag = this.btnDeploy.Enabled || this.menuItemDeploy.Enabled;
          break;
        case FSExplorer.MenuFunctionTypes.ExportFile:
          flag = this.btnExport.Enabled || this.menuItemExport.Enabled;
          break;
      }
      return flag;
    }

    private void buttons_Click(object sender, EventArgs e)
    {
      Button button = (Button) null;
      if (!(sender is StandardIconButton standardIconButton))
        button = (Button) sender;
      if (standardIconButton != null)
      {
        if (standardIconButton == this.btnNewFolder)
          this.menuItemCreateFolder_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnAdd)
          this.menuItemNew_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnDuplicate)
          this.menuItemDuplicate_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnOpen)
          this.menuItemOpen_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnCut)
          this.menuItemCut_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnCopy)
          this.menuItemCopy_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnPaste)
          this.menuItemPaste_Click((object) null, (EventArgs) null);
        else if (standardIconButton == this.btnDelete)
          this.menuItemDelete_Click((object) null, (EventArgs) null);
      }
      else if (button != null)
      {
        if (button == this.btnRename)
          this.menuItemRename_Click((object) null, (EventArgs) null);
        else if (button == this.btnImport)
          this.menuItemImport_Click((object) null, (EventArgs) null);
        else if (button == this.btnDeploy)
          this.menuItemDeploy_Click((object) null, (EventArgs) null);
        else if (button == this.btnExport)
          this.menuItemExport_Click((object) null, (EventArgs) null);
      }
      this.setTitle();
    }

    private void gvDirectory_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        this.menuItemOpen_Click((object) null, (EventArgs) null);
      }
      else
      {
        if (e.KeyCode != Keys.Delete || !this.currentFolder.IsPublic)
          return;
        bool flag = true;
        foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
        {
          if (((FileSystemEntry) selectedItem.Tag).Access == AclResourceAccess.ReadOnly)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights for the selected files to perform this function.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          this.menuItemDelete_Click((object) null, (EventArgs) null);
      }
    }

    private void gvDirectory_EditorOpening(object sender, GVSubItemEditingEventArgs e)
    {
      if (this.editorFormatter != null)
        this.editorFormatter.Dispose();
      this.editorFormatter = (TextBoxFormatter) new FileSystemEntryFormatter(e.EditorControl as TextBox);
      if (this.gvDirectory.SelectedItems.Count > 0 && this.gvDirectory.SelectedItems[0].Tag != null && this.gvDirectory.SelectedItems[0].Tag.ToString() == "")
      {
        e.Cancel = true;
      }
      else
      {
        if (this.currentFolder.IsPublic)
        {
          if (this.gvDirectory.SelectedItems.Count > 0)
          {
            FileSystemEntry tag = (FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag;
            if (tag != null && tag.Access == AclResourceAccess.ReadOnly)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to rename this file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
              return;
            }
          }
        }
        else if (this.doNotHaveRightToPersonalFolder())
        {
          e.Cancel = true;
          return;
        }
        this.ResetOption();
        if (this.gvDirectory.SelectedItems.Count == 0)
          return;
        FileSystemEntry tag1 = (FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag;
        if (tag1.Type == FileSystemEntry.Types.Folder)
        {
          if (this.BeforeFolderRenamed == null)
            return;
          this.BeforeFolderRenamed((object) this, (EventArgs) e);
        }
        else
        {
          if (tag1.Type != FileSystemEntry.Types.File || this.BeforeFileRenamed == null)
            return;
          this.BeforeFileRenamed((object) tag1, (EventArgs) e);
        }
      }
    }

    private void cmbBoxFolder_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      Form parentForm = this.ParentForm;
      if (!(parentForm is IHelp))
        return;
      ((IHelp) parentForm).ShowHelp();
    }

    public void SetupForConditionalLetter(bool canCreateEdit)
    {
      this.canCreateEdit = canCreateEdit;
      this.pnlFolder.Visible = false;
      this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnOpen, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename);
      if (!this.canCreateEdit)
        this.btnAdd.Enabled = false;
      else
        this.btnAdd.Enabled = true;
      this.menuItemNew.Text = "New";
      this.menuItemOpen.Text = "Edit";
      this.menuItemNew.Enabled = true;
      this.HideDescription = true;
      this.nameHeader.Width = this.gcListView.Width - 10;
    }

    public void SetupForCustomForm() => this.nameHeader.Width = this.gcListView.Width - 1000;

    public void SetupForPurchaseAdvice(bool canCreateEdit)
    {
      this.canCreateEdit = canCreateEdit;
      this.pnlFolder.Visible = false;
      if (this.fileType != FSExplorer.FileTypes.LoanDuplicationTemplate)
        this.pnlExPurchaseAdvice.Visible = true;
      this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnOpen, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnRename);
      if ((this.fileType == FSExplorer.FileTypes.PurchaseAdvice || this.fileType == FSExplorer.FileTypes.LoanDuplicationTemplate) && !this.canCreateEdit)
      {
        this.btnAdd.Enabled = false;
        this.SingleSelection = true;
        this.disableAllMenuItemsExceptRefresh = true;
        this.HideAllButtons = true;
        this.initializeButtons();
      }
      else
      {
        this.btnAdd.Enabled = true;
        this.menuItemOpen.Text = "Edit";
        this.menuItemNew.Text = "New";
        this.menuItemNew.Enabled = true;
      }
    }

    public void SetupForStackingOrder()
    {
      this.pnlFolder.Visible = false;
      this.arrangeButtons((object) this.btnAdd, (object) this.btnDuplicate, (object) this.btnDelete, (object) this.verticalSeparator1, (object) this.btnSetAsDefault);
      this.menuItemOpen.Text = "Edit";
      this.menuItemNew.Text = "New";
    }

    public void EnableDeployAction()
    {
      this.btnDeploy.Top = this.btnNewFolder.Top + 2 * (this.btnOpen.Top - this.btnAdd.Top);
      this.btnDeploy.Visible = true;
      this.btnDeploy.Enabled = false;
    }

    private void menuItemDuplicate_Click(object sender, EventArgs e)
    {
      if (this.disableAllMenuItemsExceptRefresh)
        return;
      if (this.fileType != FSExplorer.FileTypes.Reports || this.currentFolder.IsPublic || this.gvDirectory.SelectedItems.Count > 0 || this.fileType == FSExplorer.FileTypes.Reports && this.cutCopyEntries == null)
        this.menuItemCopy_Click((object) null, (EventArgs) null);
      this.gvDirectory.SelectedItems.Clear();
      this.menuItemPaste_Click((object) null, (EventArgs) null);
    }

    public bool HasPublicRight
    {
      get => this.hasPublicRight;
      set
      {
        this.hasPublicRight = value;
        this.setButtonsForUserRight();
      }
    }

    public bool SingleSelection
    {
      set
      {
        if (value)
          this.gvDirectory.AllowMultiselect = false;
        else
          this.gvDirectory.AllowMultiselect = true;
      }
    }

    private void menuItemDeploy_Click(object sender, EventArgs e)
    {
      if (1 != this.gvDirectory.SelectedItems.Count || this.gvDirectory.SelectedItems[0].Tag == null)
        return;
      FileSystemEntry tag = (FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag;
      if (FileSystemEntry.Types.File != tag.Type)
        return;
      this.IfsExplorer.Deploy(tag);
      this.gvDirectory.SelectedItems.Clear();
      this.updateButtons();
    }

    private void menuItemExport_Click(object sender, EventArgs e)
    {
      List<FileSystemEntry> fsEntryList = new List<FileSystemEntry>();
      foreach (GVItem selectedItem in this.gvDirectory.SelectedItems)
      {
        if (selectedItem.Tag is FileSystemEntry tag && FileSystemEntry.Types.File == tag.Type)
          fsEntryList.Add((FileSystemEntry) selectedItem.Tag);
      }
      if (0 >= fsEntryList.Count)
        return;
      this.IfsExplorer.Export(fsEntryList);
      this.updateButtons();
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FSExplorer));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      this.ctxMenuLstView1 = new ContextMenu();
      this.menuItemNew = new MenuItem();
      this.menuItemDuplicate = new MenuItem();
      this.menuItemCut = new MenuItem();
      this.menuItemCopy = new MenuItem();
      this.menuItemPaste = new MenuItem();
      this.menuItemOpen = new MenuItem();
      this.menuItemCreateFolder = new MenuItem();
      this.menuItemDelete = new MenuItem();
      this.menuItemRename = new MenuItem();
      this.menuItemDeploy = new MenuItem();
      this.menuItemImport = new MenuItem();
      this.menuItemExport = new MenuItem();
      this.menuItemSetAsDefault = new MenuItem();
      this.menuItemRefresh = new MenuItem();
      this.imgsListView = new ImageList(this.components);
      this.lblFolder = new Label();
      this.cmbBoxFolder = new ComboBoxEx();
      this.pnlFolder = new GradientPanel();
      this.stdIconBtnUpFolder = new StandardIconButton();
      this.tipFSExplorer = new ToolTip(this.components);
      this.btnDelete = new StandardIconButton();
      this.btnNewFolder = new StandardIconButton();
      this.btnOpen = new StandardIconButton();
      this.btnPaste = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnCopy = new StandardIconButton();
      this.btnCut = new StandardIconButton();
      this.btnAdd = new StandardIconButton();
      this.gcListView = new GroupContainer();
      this.btnSetAsDefault = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnRename = new Button();
      this.btnExport = new Button();
      this.btnDeploy = new Button();
      this.btnImport = new Button();
      this.gvDirectory = new GridView();
      this.pnlExPurchaseAdvice = new GradientPanel();
      this.lblAdditionalDescription = new Label();
      this.pnlFolder.SuspendLayout();
      ((ISupportInitialize) this.stdIconBtnUpFolder).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnNewFolder).BeginInit();
      ((ISupportInitialize) this.btnOpen).BeginInit();
      ((ISupportInitialize) this.btnPaste).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnCopy).BeginInit();
      ((ISupportInitialize) this.btnCut).BeginInit();
      ((ISupportInitialize) this.btnAdd).BeginInit();
      this.gcListView.SuspendLayout();
      this.pnlExPurchaseAdvice.SuspendLayout();
      this.SuspendLayout();
      this.ctxMenuLstView1.MenuItems.AddRange(new MenuItem[14]
      {
        this.menuItemNew,
        this.menuItemDuplicate,
        this.menuItemCut,
        this.menuItemCopy,
        this.menuItemPaste,
        this.menuItemOpen,
        this.menuItemCreateFolder,
        this.menuItemDelete,
        this.menuItemRename,
        this.menuItemDeploy,
        this.menuItemImport,
        this.menuItemExport,
        this.menuItemSetAsDefault,
        this.menuItemRefresh
      });
      this.ctxMenuLstView1.Popup += new EventHandler(this.ctxMenuLstView1_Popup);
      this.menuItemNew.Index = 0;
      this.menuItemNew.Text = "Add";
      this.menuItemNew.Click += new EventHandler(this.menuItemNew_Click);
      this.menuItemDuplicate.Index = 1;
      this.menuItemDuplicate.Text = "Duplicate";
      this.menuItemDuplicate.Click += new EventHandler(this.menuItemDuplicate_Click);
      this.menuItemCut.Index = 2;
      this.menuItemCut.Text = "Cut";
      this.menuItemCut.Click += new EventHandler(this.menuItemCut_Click);
      this.menuItemCopy.Index = 3;
      this.menuItemCopy.Text = "Copy";
      this.menuItemCopy.Click += new EventHandler(this.menuItemCopy_Click);
      this.menuItemPaste.Index = 4;
      this.menuItemPaste.Text = "Paste";
      this.menuItemPaste.Click += new EventHandler(this.menuItemPaste_Click);
      this.menuItemOpen.Index = 5;
      this.menuItemOpen.Text = "Open";
      this.menuItemOpen.Click += new EventHandler(this.menuItemOpen_Click);
      this.menuItemCreateFolder.Index = 6;
      this.menuItemCreateFolder.Text = "New Folder";
      this.menuItemCreateFolder.Click += new EventHandler(this.menuItemCreateFolder_Click);
      this.menuItemDelete.Index = 7;
      this.menuItemDelete.Text = "Delete";
      this.menuItemDelete.Click += new EventHandler(this.menuItemDelete_Click);
      this.menuItemRename.Index = 8;
      this.menuItemRename.Text = "Rename";
      this.menuItemRename.Click += new EventHandler(this.menuItemRename_Click);
      this.menuItemDeploy.Index = 9;
      this.menuItemDeploy.Text = "Deploy";
      this.menuItemDeploy.Visible = false;
      this.menuItemDeploy.Click += new EventHandler(this.menuItemDeploy_Click);
      this.menuItemImport.Index = 10;
      this.menuItemImport.Text = "Import";
      this.menuItemImport.Click += new EventHandler(this.menuItemImport_Click);
      this.menuItemExport.Index = 11;
      this.menuItemExport.Text = "Export";
      this.menuItemExport.Click += new EventHandler(this.menuItemExport_Click);
      this.menuItemSetAsDefault.Index = 12;
      this.menuItemSetAsDefault.Text = "Set as Default";
      this.menuItemSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.menuItemRefresh.Index = 13;
      this.menuItemRefresh.Text = "Refresh";
      this.menuItemRefresh.Click += new EventHandler(this.menuItemRefresh_Click);
      this.imgsListView.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgsListView.ImageStream");
      this.imgsListView.TransparentColor = Color.Transparent;
      this.imgsListView.Images.SetKeyName(0, "share-folder.png");
      this.imgsListView.Images.SetKeyName(1, "tempate.png");
      this.imgsListView.Images.SetKeyName(2, "folder.png");
      this.imgsListView.Images.SetKeyName(3, "document-group-public.png");
      this.imgsListView.Images.SetKeyName(4, "document-group-private.png");
      this.imgsListView.Images.SetKeyName(5, "report.png");
      this.lblFolder.AutoSize = true;
      this.lblFolder.BackColor = Color.Transparent;
      this.lblFolder.Location = new Point(6, 9);
      this.lblFolder.Name = "lblFolder";
      this.lblFolder.Size = new Size(37, 14);
      this.lblFolder.TabIndex = 1;
      this.lblFolder.Text = "Folder";
      this.lblFolder.TextAlign = ContentAlignment.MiddleLeft;
      this.cmbBoxFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxFolder.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxFolder.Location = new Point(45, 5);
      this.cmbBoxFolder.Name = "cmbBoxFolder";
      this.cmbBoxFolder.SelectedBGColor = SystemColors.Highlight;
      this.cmbBoxFolder.Size = new Size(654, 21);
      this.cmbBoxFolder.TabIndex = 0;
      this.cmbBoxFolder.SelectedIndexChanged += new EventHandler(this.cmbBoxFolder_SelectedIndexChanged);
      this.cmbBoxFolder.KeyDown += new KeyEventHandler(this.cmbBoxFolder_KeyDown);
      this.pnlFolder.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlFolder.Controls.Add((Control) this.stdIconBtnUpFolder);
      this.pnlFolder.Controls.Add((Control) this.lblFolder);
      this.pnlFolder.Controls.Add((Control) this.cmbBoxFolder);
      this.pnlFolder.Dock = DockStyle.Top;
      this.pnlFolder.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlFolder.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlFolder.Location = new Point(0, 0);
      this.pnlFolder.Name = "pnlFolder";
      this.pnlFolder.Size = new Size(733, 31);
      this.pnlFolder.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.pnlFolder.TabIndex = 16;
      this.stdIconBtnUpFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnUpFolder.BackColor = Color.Transparent;
      this.stdIconBtnUpFolder.Location = new Point(705, 7);
      this.stdIconBtnUpFolder.Name = "stdIconBtnUpFolder";
      this.stdIconBtnUpFolder.Size = new Size(16, 16);
      this.stdIconBtnUpFolder.StandardButtonType = StandardIconButton.ButtonType.UpFolderButton;
      this.stdIconBtnUpFolder.TabIndex = 2;
      this.stdIconBtnUpFolder.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.stdIconBtnUpFolder, "Up One Level");
      this.stdIconBtnUpFolder.Click += new EventHandler(this.picFolderUp_Click);
      this.tipFSExplorer.ShowAlways = true;
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(361, 5);
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 26;
      this.btnDelete.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnDelete, "Delete");
      this.btnDelete.Click += new EventHandler(this.buttons_Click);
      this.btnNewFolder.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNewFolder.BackColor = Color.Transparent;
      this.btnNewFolder.Location = new Point(339, 5);
      this.btnNewFolder.Name = "btnNewFolder";
      this.btnNewFolder.Size = new Size(16, 16);
      this.btnNewFolder.StandardButtonType = StandardIconButton.ButtonType.NewFolderButton;
      this.btnNewFolder.TabIndex = 25;
      this.btnNewFolder.TabStop = false;
      this.btnNewFolder.Text = "&New Folder";
      this.tipFSExplorer.SetToolTip((Control) this.btnNewFolder, "New Folder");
      this.btnNewFolder.Click += new EventHandler(this.buttons_Click);
      this.btnOpen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOpen.BackColor = Color.Transparent;
      this.btnOpen.Location = new Point(317, 5);
      this.btnOpen.Name = "btnOpen";
      this.btnOpen.Size = new Size(16, 16);
      this.btnOpen.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnOpen.TabIndex = 24;
      this.btnOpen.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnOpen, "Edit");
      this.btnOpen.Click += new EventHandler(this.buttons_Click);
      this.btnPaste.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPaste.BackColor = Color.Transparent;
      this.btnPaste.Location = new Point(295, 5);
      this.btnPaste.Name = "btnPaste";
      this.btnPaste.Size = new Size(16, 16);
      this.btnPaste.StandardButtonType = StandardIconButton.ButtonType.PasteButton;
      this.btnPaste.TabIndex = 23;
      this.btnPaste.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnPaste, "Paste");
      this.btnPaste.Click += new EventHandler(this.buttons_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(229, 5);
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 22;
      this.btnDuplicate.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnDuplicate, "Duplicate");
      this.btnDuplicate.Click += new EventHandler(this.buttons_Click);
      this.btnCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCopy.BackColor = Color.Transparent;
      this.btnCopy.Location = new Point(273, 5);
      this.btnCopy.Name = "btnCopy";
      this.btnCopy.Size = new Size(16, 16);
      this.btnCopy.StandardButtonType = StandardIconButton.ButtonType.CopyButton;
      this.btnCopy.TabIndex = 21;
      this.btnCopy.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnCopy, "Copy");
      this.btnCopy.Click += new EventHandler(this.buttons_Click);
      this.btnCut.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCut.BackColor = Color.Transparent;
      this.btnCut.Location = new Point(251, 5);
      this.btnCut.Name = "btnCut";
      this.btnCut.Size = new Size(16, 16);
      this.btnCut.StandardButtonType = StandardIconButton.ButtonType.CutButton;
      this.btnCut.TabIndex = 20;
      this.btnCut.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnCut, "Cut");
      this.btnCut.Click += new EventHandler(this.buttons_Click);
      this.btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdd.BackColor = Color.Transparent;
      this.btnAdd.Location = new Point(207, 5);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(16, 16);
      this.btnAdd.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnAdd.TabIndex = 19;
      this.btnAdd.TabStop = false;
      this.tipFSExplorer.SetToolTip((Control) this.btnAdd, "New");
      this.btnAdd.Click += new EventHandler(this.buttons_Click);
      this.gcListView.Controls.Add((Control) this.btnSetAsDefault);
      this.gcListView.Controls.Add((Control) this.verticalSeparator1);
      this.gcListView.Controls.Add((Control) this.btnRename);
      this.gcListView.Controls.Add((Control) this.btnExport);
      this.gcListView.Controls.Add((Control) this.btnDeploy);
      this.gcListView.Controls.Add((Control) this.btnImport);
      this.gcListView.Controls.Add((Control) this.btnDelete);
      this.gcListView.Controls.Add((Control) this.btnNewFolder);
      this.gcListView.Controls.Add((Control) this.btnOpen);
      this.gcListView.Controls.Add((Control) this.btnPaste);
      this.gcListView.Controls.Add((Control) this.btnDuplicate);
      this.gcListView.Controls.Add((Control) this.btnCopy);
      this.gcListView.Controls.Add((Control) this.btnCut);
      this.gcListView.Controls.Add((Control) this.btnAdd);
      this.gcListView.Controls.Add((Control) this.gvDirectory);
      this.gcListView.Controls.Add((Control) this.pnlExPurchaseAdvice);
      this.gcListView.Dock = DockStyle.Fill;
      this.gcListView.Location = new Point(0, 31);
      this.gcListView.Name = "gcListView";
      this.gcListView.Size = new Size(733, 451);
      this.gcListView.TabIndex = 19;
      this.gcListView.Text = "<title>";
      this.btnSetAsDefault.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSetAsDefault.BackColor = SystemColors.Control;
      this.btnSetAsDefault.Location = new Point(637, 2);
      this.btnSetAsDefault.Name = "btnSetAsDefault";
      this.btnSetAsDefault.Size = new Size(88, 22);
      this.btnSetAsDefault.TabIndex = 28;
      this.btnSetAsDefault.Text = "Set as Default";
      this.btnSetAsDefault.UseVisualStyleBackColor = true;
      this.btnSetAsDefault.Click += new EventHandler(this.btnSetAsDefault_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(382, 5);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 27;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnRename.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRename.BackColor = SystemColors.Control;
      this.btnRename.Location = new Point(389, 2);
      this.btnRename.Name = "btnRename";
      this.btnRename.Size = new Size(62, 22);
      this.btnRename.TabIndex = 9;
      this.btnRename.Text = "Rena&me";
      this.btnRename.UseVisualStyleBackColor = true;
      this.btnRename.Click += new EventHandler(this.buttons_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = SystemColors.Control;
      this.btnExport.Location = new Point(575, 2);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(62, 22);
      this.btnExport.TabIndex = 15;
      this.btnExport.Text = "&Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new EventHandler(this.buttons_Click);
      this.btnDeploy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDeploy.BackColor = SystemColors.Control;
      this.btnDeploy.Location = new Point(451, 2);
      this.btnDeploy.Name = "btnDeploy";
      this.btnDeploy.Size = new Size(62, 22);
      this.btnDeploy.TabIndex = 14;
      this.btnDeploy.Text = "Deploy";
      this.btnDeploy.UseVisualStyleBackColor = true;
      this.btnDeploy.Click += new EventHandler(this.buttons_Click);
      this.btnImport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnImport.BackColor = SystemColors.Control;
      this.btnImport.Location = new Point(513, 2);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(62, 22);
      this.btnImport.TabIndex = 11;
      this.btnImport.Text = "&Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.buttons_Click);
      this.gvDirectory.AllowDrop = true;
      this.gvDirectory.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Name";
      gvColumn1.Width = 280;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Description";
      gvColumn2.Width = 400;
      this.gvDirectory.Columns.AddRange(new GVColumn[2]
      {
        gvColumn1,
        gvColumn2
      });
      this.gvDirectory.ContextMenu = this.ctxMenuLstView1;
      this.gvDirectory.Dock = DockStyle.Fill;
      this.gvDirectory.ImageList = this.imgsListView;
      this.gvDirectory.Location = new Point(1, 57);
      this.gvDirectory.Name = "gvDirectory";
      this.gvDirectory.Size = new Size(731, 393);
      this.gvDirectory.SortOption = GVSortOption.Owner;
      this.gvDirectory.TabIndex = 2;
      this.gvDirectory.UseCompatibleEditingBehavior = true;
      this.gvDirectory.Resize += new EventHandler(this.gvDirectory_Resize);
      this.gvDirectory.EditorOpening += new GVSubItemEditingEventHandler(this.gvDirectory_EditorOpening);
      this.gvDirectory.SelectedIndexChanged += new EventHandler(this.gvDirectory_SelectedIndexChanged);
      this.gvDirectory.DoubleClick += new EventHandler(this.gvDirectory_DoubleClick);
      this.gvDirectory.SortItems += new GVColumnSortEventHandler(this.gvDirectory_SortItems);
      this.gvDirectory.MouseDown += new MouseEventHandler(this.gvDirectory_MouseDown);
      this.gvDirectory.KeyPress += new KeyPressEventHandler(this.gvDirectory_KeyPress);
      this.gvDirectory.KeyUp += new KeyEventHandler(this.gvDirectory_KeyUp);
      this.gvDirectory.EditorClosing += new GVSubItemEditingEventHandler(this.gvDirectory_EditorClosing);
      this.gvDirectory.ItemDoubleClick += new GVItemEventHandler(this.gvDirectory_ItemDoubleClick);
      this.pnlExPurchaseAdvice.Borders = AnchorStyles.Bottom;
      this.pnlExPurchaseAdvice.Controls.Add((Control) this.lblAdditionalDescription);
      this.pnlExPurchaseAdvice.Dock = DockStyle.Top;
      this.pnlExPurchaseAdvice.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlExPurchaseAdvice.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlExPurchaseAdvice.Location = new Point(1, 26);
      this.pnlExPurchaseAdvice.Name = "pnlExPurchaseAdvice";
      this.pnlExPurchaseAdvice.Size = new Size(731, 31);
      this.pnlExPurchaseAdvice.TabIndex = 29;
      this.pnlExPurchaseAdvice.Visible = false;
      this.lblAdditionalDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lblAdditionalDescription.BackColor = Color.Transparent;
      this.lblAdditionalDescription.Location = new Point(11, 3);
      this.lblAdditionalDescription.Name = "lblAdditionalDescription";
      this.lblAdditionalDescription.Size = new Size(709, 24);
      this.lblAdditionalDescription.TabIndex = 0;
      this.lblAdditionalDescription.Text = "Create and edit templates that populate payout descriptions and expected payout amounts on the Purchase Advice Form.";
      this.Controls.Add((Control) this.gcListView);
      this.Controls.Add((Control) this.pnlFolder);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (FSExplorer);
      this.Size = new Size(733, 482);
      this.pnlFolder.ResumeLayout(false);
      this.pnlFolder.PerformLayout();
      ((ISupportInitialize) this.stdIconBtnUpFolder).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnNewFolder).EndInit();
      ((ISupportInitialize) this.btnOpen).EndInit();
      ((ISupportInitialize) this.btnPaste).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnCopy).EndInit();
      ((ISupportInitialize) this.btnCut).EndInit();
      ((ISupportInitialize) this.btnAdd).EndInit();
      this.gcListView.ResumeLayout(false);
      this.pnlExPurchaseAdvice.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void btnSetAsDefault_Click(object sender, EventArgs e)
    {
      if (this.SetAsDefaultButtonClick == null)
        return;
      this.SetAsDefaultButtonClick(sender, e);
    }

    public CmbBoxItem[] FolderComboItems
    {
      get
      {
        if (this.cmbBoxFolder.Items == null)
          return (CmbBoxItem[]) null;
        CmbBoxItem[] folderComboItems = new CmbBoxItem[this.cmbBoxFolder.Items.Count];
        for (int index = 0; index < this.cmbBoxFolder.Items.Count; ++index)
          folderComboItems[index] = (CmbBoxItem) this.cmbBoxFolder.Items[index];
        return folderComboItems;
      }
    }

    public int FolderComboSelectedIndex
    {
      get => this.cmbBoxFolder.SelectedIndex;
      set
      {
        if (this.cmbBoxFolder.SelectedIndex != value)
          this.cmbBoxFolder.SelectedIndex = value;
        else
          this.cmbBoxFolder_SelectedIndexChanged((object) this, (EventArgs) null);
      }
    }

    public GVItemCollection GVItems => this.gvDirectory.Items;

    public bool UpFolderIconEnabled => this.stdIconBtnUpFolder.Enabled;

    public void UpOneLevel() => this.picFolderUp_Click((object) null, (EventArgs) null);

    public void GVItemDoubleClicked(string gvItemText)
    {
      for (int nItemIndex = 0; nItemIndex < this.gvDirectory.Items.Count; ++nItemIndex)
      {
        if (gvItemText == this.gvDirectory.Items[nItemIndex].Text)
        {
          foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvDirectory.Items)
            gvItem.Selected = false;
          this.gvDirectory.Items[nItemIndex].Selected = true;
          break;
        }
      }
      this.gvDirectory_ItemDoubleClick((object) this, (GVItemEventArgs) null);
    }

    private void gvDirectory_ItemDoubleClick(object sender, GVItemEventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0)
        return;
      if (this.gvDirectory.SelectedItems[0].Tag.ToString() == "")
      {
        this.picFolderUp_Click((object) null, (EventArgs) null);
      }
      else
      {
        if (this.fileType != FSExplorer.FileTypes.MilestoneTemplate && this.fileType != FSExplorer.FileTypes.SyncTemplates)
        {
          FileSystemEntry tag1 = (FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag;
          if (tag1.Type == FileSystemEntry.Types.Folder)
          {
            if (this.currRoot == FSExplorer.RootType.Top)
              this.changeFolder("\\", tag1.IsPublic ? FSExplorer.RootType.Public : FSExplorer.RootType.Private);
            else
              this.changeFolder(tag1.Path, this.currRoot);
            this.ResetOption();
            if (this.doNotHaveRightToPersonalFolder())
            {
              this.MakeFileReadOnly();
              this.MakeFolderReadOnly();
            }
            else if (!this.currentFolder.IsPublic || this.currentFolder.Access == AclResourceAccess.ReadWrite)
              this.MakeEmptySelectionReadWrite();
          }
          else if (tag1.Type == FileSystemEntry.Types.File)
          {
            if ((FSExplorer.TemplateSettingsType.DashboardViewTemplate == this.templateType || FSExplorer.TemplateSettingsType.DashboardTemplate == this.templateType) && FSExplorer.DialogMode.SelectFiles == this.dialogMode)
            {
              this.open(this.gvDirectory.SelectedItems[0]);
              return;
            }
            if (this.disableAllMenuItemsExceptRefresh && !this.btnOpen.Enabled)
            {
              if (this.SelectedCurrentFile == null)
                return;
              this.SelectedCurrentFile(sender, (EventArgs) e);
              return;
            }
            if (!this.hasPublicRight && this.IsRootPublic && (this.fileType == FSExplorer.FileTypes.CustomForms || this.fileType == FSExplorer.FileTypes.CustomLetters))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You do not have the necessary access rights to open this form.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
            GVItem selectedItem = this.gvDirectory.SelectedItems[0];
            FileSystemEntry tag2 = (FileSystemEntry) selectedItem.Tag;
            if (this.btnOpen.Enabled)
            {
              this.open(selectedItem);
              this.ResetOption();
              if (!this.currentFolder.IsPublic || tag1.Access == AclResourceAccess.ReadWrite)
                this.MakeFileReadWrite();
            }
          }
        }
        this.setTitle();
        this.setButtonsForReports();
      }
    }

    private void setButtonsForReports()
    {
      if (this.fileType != FSExplorer.FileTypes.Reports || this.currentFolder.IsPublic)
        return;
      this.btnNewFolder.Enabled = this.menuItemCreateFolder.Enabled = true;
      this.btnAdd.Enabled = this.menuItemNew.Enabled = true;
      if (this.cutCopyEntries == null)
        return;
      this.btnDuplicate.Enabled = this.menuItemDuplicate.Enabled = true;
      this.btnPaste.Enabled = this.menuItemPaste.Enabled = true;
    }

    private void gvDirectory_DoubleClick(object sender, EventArgs e)
    {
      if (this.gvDirectory.SelectedItems.Count == 0 || this.fileType != FSExplorer.FileTypes.MilestoneTemplate && this.fileType != FSExplorer.FileTypes.SyncTemplates && ((FileSystemEntry) this.gvDirectory.SelectedItems[0].Tag).Type != FileSystemEntry.Types.File || this.EntryDoubleClicked == null)
        return;
      this.EntryDoubleClicked(sender, e);
    }

    public Size RenameButtonSize
    {
      get => this.btnRename.Size;
      set => this.btnRename.Size = value;
    }

    private bool doNotHaveRightToPersonalFolder()
    {
      if (this.currentFolder.IsPublic || UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
        return false;
      if (this.userFeatureRights == null)
      {
        FeaturesAclManager aclManager = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
        if (aclManager == null)
          return false;
        this.userFeatureRights = aclManager.CheckPermissions(FeatureSets.SettingsTabCompanyFeatures, this.session.UserInfo);
      }
      if (this.userFeatureRights == null)
        return false;
      switch (this.fileType)
      {
        case FSExplorer.FileTypes.LoanTemplates:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_LoanTemplateSets];
        case FSExplorer.FileTypes.DataTemplates:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_MiscDataTemplates];
        case FSExplorer.FileTypes.LoanPrograms:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_LoanPrograms];
        case FSExplorer.FileTypes.ClosingCosts:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_ClosingCosts];
        case FSExplorer.FileTypes.DocumentSets:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_DocumentSets];
        case FSExplorer.FileTypes.FormLists:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_InputFormSets];
        case FSExplorer.FileTypes.TaskSets:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_TaskSets];
        case FSExplorer.FileTypes.SettlementServiceProviders:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_SettlementServiceProvider];
        case FSExplorer.FileTypes.AffiliatedBusinessArrangements:
          return !(bool) this.userFeatureRights[(object) AclFeature.SettingsTab_Personal_Affiliate];
        default:
          return false;
      }
    }

    public enum FileTypes
    {
      Ignore,
      PrintGroups,
      CustomForms,
      CustomLetters,
      Reports,
      LoanTemplates,
      DataTemplates,
      LoanPrograms,
      ClosingCosts,
      DocumentSets,
      FormLists,
      StackingOrderSets,
      CampaignTemplates,
      PurchaseAdvice,
      FundingTemplate,
      DashboardTemplate,
      DashboardViewTemplate,
      TaskSets,
      ConditionalLetter,
      SettlementServiceProviders,
      LoanDuplicationTemplate,
      MilestoneTemplate,
      AffiliatedBusinessArrangements,
      SyncTemplates,
    }

    public enum MenuFunctionTypes
    {
      CreateFolder = 1,
      AddNewFile = 2,
      OpenFolderOrFile = 3,
      CutFolderOrFile = 4,
      CopyFolderOrFile = 5,
      PasteFolderOrFile = 6,
      DeleteFolderOrFile = 7,
      RenameFolderOrFile = 8,
      ImportFile = 9,
      RefreshFolder = 10, // 0x0000000A
      DuplicateFile = 11, // 0x0000000B
      DeployFile = 12, // 0x0000000C
      ExportFile = 13, // 0x0000000D
    }

    private enum RootType
    {
      Top,
      Private,
      Public,
      PublicOnly,
    }

    private enum TemplateSettingsType
    {
      Nothing,
      LoanProgram,
      ClosingCost,
      MiscData,
      FormList,
      DocumentSet,
      LoanTemplate,
      StackingOrder,
      Campaign,
      DashboardTemplate,
      DashboardViewTemplate,
      TaskSet,
      SettlementServiceProviders,
      AffiliatedBusinessArrangements,
    }

    public enum DialogMode
    {
      Unspecified,
      SelectFiles,
      SaveFiles,
      ManageFiles,
    }

    public enum RESPAFilter
    {
      All,
      Respa2010,
      Respa2009,
      Respa2015,
    }
  }
}
