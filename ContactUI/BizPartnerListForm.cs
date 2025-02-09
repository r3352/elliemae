// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BizPartnerListForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using Elli.Metrics.Client;
using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BizPartnerListForm : Form, IBizContacts
  {
    private EllieMae.EMLite.ClientServer.TemplateSettingsType templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.PublicBizPartnerView;
    private string standardViewName = "Standard View";
    private FileSystemEntry fsViewEntry;
    private ICursor contactCursor;
    private PageChangedEventArgs currentPagingArgument;
    private bool suspendEvents;
    private bool suspendRefresh;
    private ContactView currentView;
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private FieldFilterList advFilter;
    private BizPartnerReportFieldDefs contactFieldDefsWoHistory;
    private BizPartnerLoanReportFieldDefs contactLoanFieldDefs;
    private static string className = nameof (BizPartnerListForm);
    private static string sw = Tracing.SwContact;
    private BizPartnerTabForm contactDetailTab;
    private int currentContactId = -1;
    private BizCategoryUtil catUtil;
    private Hashtable idMap = new Hashtable();
    private ContactListState listState;
    private ContactType contactType = ContactType.BizPartner;
    private int idForState = -1;
    private string nameForState = string.Empty;
    private bool mailMergeDocMgmtOnly;
    private FeaturesAclManager aclMgr;
    private Button btnMailMerge;
    private Panel panelBottom;
    private Panel panelBody;
    private Button btnAddToGroup;
    private Button btnRemoveFromGroup;
    private Button btnEditGroups;
    private IContainer components;
    private ContactMainForm parentForm;
    private GridView gvContactList;
    private StandardIconButton btnRefresh;
    private StandardIconButton btnSave;
    private Panel panel1;
    private GradientPanel gradientPanel1;
    private StandardIconButton btnRefreshView;
    private StandardIconButton btnEditView;
    private StandardIconButton btnSaveView;
    private ComboBoxEx cboView;
    private Label label3;
    private GradientPanel gradientPanel3;
    private Button btnAdvSearch;
    private Label label4;
    private GroupContainer groupContainer1;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel pnlDetail;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnPrint;
    private StandardIconButton btnExport;
    private StandardIconButton btnDelete;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnNew;
    private PageListNavigator navContacts;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem menuItemNewContact;
    private ToolStripMenuItem menuItemPrint;
    private ToolStripMenuItem menuItemCopy;
    private ToolStripMenuItem menuItemDelete;
    private ToolStripMenuItem menuItemSelectAll;
    private ToolStripMenuItem menuItemMailMerge;
    private ToolStripMenuItem menuItemAddToGroup;
    private ToolStripMenuItem menuItemRemoveFromGroup;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripSeparator toolStripSeparator3;
    private ToolTip toolTip1;
    private Button btnClearSearch;
    private VerticalSeparator verticalSeparator1;
    private VerticalSeparator verticalSeparator2;
    private Label lblFilter;
    private GroupContainer groupContainer2;
    private StandardIconButton siBtnRefresh;
    private Button btnSync;
    private BizPartnerReportFieldDefs allContactFieldDefs;
    private ToolStripMenuItem menuItemPrintSelected;
    private ToolStripMenuItem menuItemPrintAll;
    private ToolStripMenuItem menuItemMailMergeSelected;
    private ToolStripMenuItem menuItemMailMergeAll;
    private Dictionary<AclFeature, bool> personaSettings = new Dictionary<AclFeature, bool>();

    public BizPartnerListForm(ContactMainForm parentForm)
      : this(parentForm, ContactType.BizPartner)
    {
    }

    public BizPartnerListForm(ContactMainForm parentForm, ContactType contactType)
    {
      this.InitializeComponent();
      Session.Application.RegisterService((object) this, typeof (IBizContacts));
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.parentForm = parentForm;
      this.contactType = contactType;
      this.templateType = this.contactType != ContactType.PublicBiz ? EllieMae.EMLite.ClientServer.TemplateSettingsType.BizPartnerView : EllieMae.EMLite.ClientServer.TemplateSettingsType.PublicBizPartnerView;
      this.gatherPersonaSettings();
      this.Init();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    public bool isDirty() => false;

    private void gatherPersonaSettings()
    {
      this.personaSettings.Add(AclFeature.Cnt_Biz_Copy, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Copy));
      this.personaSettings.Add(AclFeature.Cnt_Biz_CreateNew, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_CreateNew));
      this.personaSettings.Add(AclFeature.Cnt_Biz_Delete, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Delete));
      this.personaSettings.Add(AclFeature.Cnt_Biz_Export, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Export));
      this.personaSettings.Add(AclFeature.Cnt_Biz_Import, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Import));
      this.personaSettings.Add(AclFeature.Cnt_Biz_LoansTab, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_LoansTab));
      this.personaSettings.Add(AclFeature.Cnt_Biz_MailMerge, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_MailMerge));
      this.personaSettings.Add(AclFeature.Cnt_Biz_Print, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Print));
      this.personaSettings.Add(AclFeature.Cnt_Synchronization, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Synchronization));
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.panelBody = new Panel();
      this.panel1 = new Panel();
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.menuItemNewContact = new ToolStripMenuItem();
      this.menuItemPrint = new ToolStripMenuItem();
      this.menuItemPrintSelected = new ToolStripMenuItem();
      this.menuItemPrintAll = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.menuItemCopy = new ToolStripMenuItem();
      this.menuItemDelete = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.menuItemSelectAll = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.menuItemMailMerge = new ToolStripMenuItem();
      this.menuItemMailMergeSelected = new ToolStripMenuItem();
      this.menuItemMailMergeAll = new ToolStripMenuItem();
      this.menuItemAddToGroup = new ToolStripMenuItem();
      this.menuItemRemoveFromGroup = new ToolStripMenuItem();
      this.toolTip1 = new ToolTip(this.components);
      this.groupContainer1 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnEditGroups = new Button();
      this.btnRemoveFromGroup = new Button();
      this.btnAddToGroup = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnSync = new Button();
      this.btnMailMerge = new Button();
      this.verticalSeparator2 = new VerticalSeparator();
      this.btnPrint = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.siBtnRefresh = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.navContacts = new PageListNavigator();
      this.gvContactList = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlDetail = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.btnSave = new StandardIconButton();
      this.panelBottom = new Panel();
      this.btnRefresh = new StandardIconButton();
      this.gradientPanel3 = new GradientPanel();
      this.lblFilter = new Label();
      this.btnClearSearch = new Button();
      this.btnAdvSearch = new Button();
      this.label4 = new Label();
      this.gradientPanel1 = new GradientPanel();
      this.btnRefreshView = new StandardIconButton();
      this.btnEditView = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.cboView = new ComboBoxEx();
      this.label3 = new Label();
      this.panelBody.SuspendLayout();
      this.panel1.SuspendLayout();
      this.contextMenuStrip1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.siBtnRefresh).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.pnlDetail.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      this.gradientPanel3.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRefreshView).BeginInit();
      ((ISupportInitialize) this.btnEditView).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      this.SuspendLayout();
      this.panelBody.Controls.Add((Control) this.panel1);
      this.panelBody.Dock = DockStyle.Fill;
      this.panelBody.Location = new Point(0, 0);
      this.panelBody.Name = "panelBody";
      this.panelBody.Size = new Size(983, 618);
      this.panelBody.TabIndex = 2;
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Controls.Add((Control) this.gradientPanel3);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(983, 618);
      this.panel1.TabIndex = 5;
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[11]
      {
        (ToolStripItem) this.menuItemNewContact,
        (ToolStripItem) this.menuItemPrint,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.menuItemCopy,
        (ToolStripItem) this.menuItemDelete,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.menuItemSelectAll,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.menuItemMailMerge,
        (ToolStripItem) this.menuItemAddToGroup,
        (ToolStripItem) this.menuItemRemoveFromGroup
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.ShowImageMargin = false;
      this.contextMenuStrip1.Size = new Size(158, 198);
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.menuItemNewContact.Name = "menuItemNewContact";
      this.menuItemNewContact.Size = new Size(157, 22);
      this.menuItemNewContact.Text = "New Contact";
      this.menuItemNewContact.Click += new EventHandler(this.menuItemNewContact_Click);
      this.menuItemPrint.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.menuItemPrintSelected,
        (ToolStripItem) this.menuItemPrintAll
      });
      this.menuItemPrint.Name = "menuItemPrint";
      this.menuItemPrint.Size = new Size(157, 22);
      this.menuItemPrint.Text = "Print";
      this.menuItemPrintSelected.Name = "menuItemPrintSelected";
      this.menuItemPrintSelected.Size = new Size(206, 22);
      this.menuItemPrintSelected.Text = "Selected Contacts Only...";
      this.menuItemPrintSelected.Click += new EventHandler(this.btnPrint_Click);
      this.menuItemPrintAll.Name = "menuItemPrintAll";
      this.menuItemPrintAll.Size = new Size(206, 22);
      this.menuItemPrintAll.Text = "All Contacts on All Pages";
      this.menuItemPrintAll.Click += new EventHandler(this.btnPrintAll_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(154, 6);
      this.menuItemCopy.Name = "menuItemCopy";
      this.menuItemCopy.Size = new Size(157, 22);
      this.menuItemCopy.Text = "Duplicate";
      this.menuItemCopy.Click += new EventHandler(this.btnCopy_Click);
      this.menuItemDelete.Name = "menuItemDelete";
      this.menuItemDelete.Size = new Size(157, 22);
      this.menuItemDelete.Text = "Delete";
      this.menuItemDelete.Click += new EventHandler(this.btnDelete_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(154, 6);
      this.menuItemSelectAll.Name = "menuItemSelectAll";
      this.menuItemSelectAll.Size = new Size(157, 22);
      this.menuItemSelectAll.Text = "Select All";
      this.menuItemSelectAll.Click += new EventHandler(this.menuItemSelectAll_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(154, 6);
      this.menuItemMailMerge.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.menuItemMailMergeSelected,
        (ToolStripItem) this.menuItemMailMergeAll
      });
      this.menuItemMailMerge.Name = "menuItemMailMerge";
      this.menuItemMailMerge.Size = new Size(157, 22);
      this.menuItemMailMerge.Text = "Mail Merge";
      this.menuItemMailMergeSelected.Name = "menuItemMailMergeSelected";
      this.menuItemMailMergeSelected.Size = new Size(215, 22);
      this.menuItemMailMergeSelected.Text = "Selected Contacts Only...";
      this.menuItemMailMergeSelected.Click += new EventHandler(this.btnMailMerge_Click);
      this.menuItemMailMergeAll.Name = "menuItemMailMergeAll";
      this.menuItemMailMergeAll.Size = new Size(215, 22);
      this.menuItemMailMergeAll.Text = "All Contacts on All Pages...";
      this.menuItemMailMergeAll.Click += new EventHandler(this.btnMailMergeAll_Click);
      this.menuItemAddToGroup.Name = "menuItemAddToGroup";
      this.menuItemAddToGroup.Size = new Size(157, 22);
      this.menuItemAddToGroup.Text = "Add to Group";
      this.menuItemAddToGroup.Click += new EventHandler(this.menuItemAddToGroup_Click);
      this.menuItemRemoveFromGroup.Name = "menuItemRemoveFromGroup";
      this.menuItemRemoveFromGroup.Size = new Size(157, 22);
      this.menuItemRemoveFromGroup.Text = "Remove from Group";
      this.menuItemRemoveFromGroup.Click += new EventHandler(this.menuItemRemoveFromGroup_Click);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.navContacts);
      this.groupContainer1.Controls.Add((Control) this.gvContactList);
      this.groupContainer1.Controls.Add((Control) this.collapsibleSplitter1);
      this.groupContainer1.Controls.Add((Control) this.pnlDetail);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 62);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(983, 556);
      this.groupContainer1.TabIndex = 6;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEditGroups);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemoveFromGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddToGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSync);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMailMerge);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator2);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnPrint);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnExport);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnRefresh);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDuplicate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(228, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(754, 22);
      this.flowLayoutPanel1.TabIndex = 28;
      this.btnEditGroups.BackColor = SystemColors.Control;
      this.btnEditGroups.Location = new Point(673, 0);
      this.btnEditGroups.Margin = new Padding(0, 0, 5, 0);
      this.btnEditGroups.Name = "btnEditGroups";
      this.btnEditGroups.Padding = new Padding(2, 0, 0, 0);
      this.btnEditGroups.Size = new Size(76, 22);
      this.btnEditGroups.TabIndex = 23;
      this.btnEditGroups.Text = "Edit Groups";
      this.btnEditGroups.UseVisualStyleBackColor = true;
      this.btnEditGroups.Click += new EventHandler(this.btnEditGroups_Click);
      this.btnRemoveFromGroup.BackColor = SystemColors.Control;
      this.btnRemoveFromGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnRemoveFromGroup.Location = new Point(557, 0);
      this.btnRemoveFromGroup.Margin = new Padding(0);
      this.btnRemoveFromGroup.Name = "btnRemoveFromGroup";
      this.btnRemoveFromGroup.Padding = new Padding(2, 0, 0, 0);
      this.btnRemoveFromGroup.Size = new Size(116, 22);
      this.btnRemoveFromGroup.TabIndex = 3;
      this.btnRemoveFromGroup.Text = "Remove from Group";
      this.btnRemoveFromGroup.UseVisualStyleBackColor = true;
      this.btnRemoveFromGroup.Click += new EventHandler(this.btnRemoveFromGroup_Click);
      this.btnAddToGroup.BackColor = SystemColors.Control;
      this.btnAddToGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAddToGroup.Location = new Point(473, 0);
      this.btnAddToGroup.Margin = new Padding(0);
      this.btnAddToGroup.Name = "btnAddToGroup";
      this.btnAddToGroup.Size = new Size(84, 22);
      this.btnAddToGroup.TabIndex = 8;
      this.btnAddToGroup.Text = "Add to Group";
      this.btnAddToGroup.UseVisualStyleBackColor = true;
      this.btnAddToGroup.Click += new EventHandler(this.btnAddToGroup_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(467, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 4, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 27;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnSync.BackColor = SystemColors.Control;
      this.btnSync.Location = new Point(367, 0);
      this.btnSync.Margin = new Padding(0);
      this.btnSync.Name = "btnSync";
      this.btnSync.Padding = new Padding(2, 0, 2, 0);
      this.btnSync.Size = new Size(97, 22);
      this.btnSync.TabIndex = 26;
      this.btnSync.Text = "Synchronize";
      this.btnSync.UseVisualStyleBackColor = true;
      this.btnSync.Click += new EventHandler(this.btnSync_Click);
      this.btnMailMerge.BackColor = SystemColors.Control;
      this.btnMailMerge.Location = new Point(269, 0);
      this.btnMailMerge.Margin = new Padding(0);
      this.btnMailMerge.Name = "btnMailMerge";
      this.btnMailMerge.Padding = new Padding(2, 0, 0, 0);
      this.btnMailMerge.Size = new Size(98, 22);
      this.btnMailMerge.TabIndex = 6;
      this.btnMailMerge.Text = "Mail/Email Mer&ge";
      this.btnMailMerge.UseVisualStyleBackColor = true;
      this.btnMailMerge.Click += new EventHandler(this.btnMailMerge_Click);
      this.verticalSeparator2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator2.Location = new Point(263, 3);
      this.verticalSeparator2.Margin = new Padding(3, 3, 4, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 28;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(241, 3);
      this.btnPrint.MouseDownImage = (Image) null;
      this.btnPrint.Name = "btnPrint";
      this.btnPrint.Size = new Size(16, 16);
      this.btnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.btnPrint.TabIndex = 18;
      this.btnPrint.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnPrint, "Print Details");
      this.btnPrint.Click += new EventHandler(this.btnPrint_Click);
      this.btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExport.BackColor = Color.Transparent;
      this.btnExport.Location = new Point(219, 3);
      this.btnExport.Margin = new Padding(2, 3, 3, 3);
      this.btnExport.MouseDownImage = (Image) null;
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new Size(16, 16);
      this.btnExport.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExport.TabIndex = 17;
      this.btnExport.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnExport, "Export to Excel");
      this.btnExport.Click += new EventHandler(this.btnExport_Click);
      this.siBtnRefresh.BackColor = Color.Transparent;
      this.siBtnRefresh.Location = new Point(198, 3);
      this.siBtnRefresh.Margin = new Padding(2, 3, 3, 3);
      this.siBtnRefresh.MouseDownImage = (Image) null;
      this.siBtnRefresh.Name = "siBtnRefresh";
      this.siBtnRefresh.Size = new Size(16, 16);
      this.siBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.siBtnRefresh.TabIndex = 29;
      this.siBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnRefresh, "Refresh");
      this.siBtnRefresh.Click += new EventHandler(this.siBtnRefresh_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(177, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 16);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 19;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Contact");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Location = new Point(156, 3);
      this.btnDuplicate.Margin = new Padding(2, 3, 2, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 16);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 20;
      this.btnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicate, "Duplicate Contact");
      this.btnDuplicate.Click += new EventHandler(this.btnCopy_Click);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(135, 3);
      this.btnNew.Margin = new Padding(2, 3, 3, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 21;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Contact");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.navContacts.BackColor = Color.Transparent;
      this.navContacts.Font = new Font("Arial", 8f);
      this.navContacts.Location = new Point(0, 2);
      this.navContacts.Name = "navContacts";
      this.navContacts.NumberOfItems = 0;
      this.navContacts.Size = new Size(253, 22);
      this.navContacts.TabIndex = 27;
      this.navContacts.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContacts_PageChangedEvent);
      this.gvContactList.AllowColumnReorder = true;
      this.gvContactList.BorderStyle = BorderStyle.None;
      this.gvContactList.ContextMenuStrip = this.contextMenuStrip1;
      this.gvContactList.Dock = DockStyle.Fill;
      this.gvContactList.FilterVisible = true;
      this.gvContactList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvContactList.Location = new Point(1, 26);
      this.gvContactList.Name = "gvContactList";
      this.gvContactList.Size = new Size(981, 67);
      this.gvContactList.SortOption = GVSortOption.Owner;
      this.gvContactList.TabIndex = 2;
      this.gvContactList.SelectedIndexChanged += new EventHandler(this.gvContactList_SelectedIndexChanged);
      this.gvContactList.ColumnReorder += new GVColumnEventHandler(this.gvContactList_ColumnReorder);
      this.gvContactList.ColumnResize += new GVColumnEventHandler(this.gvContactList_ColumnResize);
      this.gvContactList.SortItems += new GVColumnSortEventHandler(this.gvContactList_SortItems);
      this.gvContactList.DoubleClick += new EventHandler(this.gvContacts_DoubleClick);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlDetail;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 93);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 25;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.pnlDetail.Controls.Add((Control) this.groupContainer2);
      this.pnlDetail.Dock = DockStyle.Bottom;
      this.pnlDetail.Location = new Point(1, 100);
      this.pnlDetail.Name = "pnlDetail";
      this.pnlDetail.Size = new Size(981, 455);
      this.pnlDetail.TabIndex = 26;
      this.groupContainer2.Borders = AnchorStyles.Top;
      this.groupContainer2.Controls.Add((Control) this.btnSave);
      this.groupContainer2.Controls.Add((Control) this.panelBottom);
      this.groupContainer2.Controls.Add((Control) this.btnRefresh);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(981, 455);
      this.groupContainer2.TabIndex = 25;
      this.groupContainer2.Text = "Contact Details";
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(939, 4);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 3;
      this.btnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSave, "Save Changes");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelBottom.BackColor = Color.Transparent;
      this.panelBottom.Dock = DockStyle.Fill;
      this.panelBottom.Location = new Point(0, 26);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Padding = new Padding(1, 0, 0, 0);
      this.panelBottom.Size = new Size(981, 429);
      this.panelBottom.TabIndex = 0;
      this.btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRefresh.BackColor = Color.Transparent;
      this.btnRefresh.Location = new Point(960, 4);
      this.btnRefresh.MouseDownImage = (Image) null;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(16, 16);
      this.btnRefresh.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefresh.TabIndex = 4;
      this.btnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRefresh, "Reset");
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.gradientPanel3.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.lblFilter);
      this.gradientPanel3.Controls.Add((Control) this.btnClearSearch);
      this.gradientPanel3.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 32);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(983, 30);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 5;
      this.lblFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(38, 9);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(756, 14);
      this.lblFilter.TabIndex = 9;
      this.btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearSearch.Enabled = false;
      this.btnClearSearch.Location = new Point(922, 5);
      this.btnClearSearch.Name = "btnClearSearch";
      this.btnClearSearch.Padding = new Padding(2, 0, 0, 0);
      this.btnClearSearch.Size = new Size(56, 22);
      this.btnClearSearch.TabIndex = 8;
      this.btnClearSearch.Text = "&Clear";
      this.btnClearSearch.UseVisualStyleBackColor = true;
      this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAdvSearch.Location = new Point(802, 5);
      this.btnAdvSearch.Name = "btnAdvSearch";
      this.btnAdvSearch.Size = new Size(120, 22);
      this.btnAdvSearch.TabIndex = 4;
      this.btnAdvSearch.Text = "Advanced &Search";
      this.btnAdvSearch.UseVisualStyleBackColor = true;
      this.btnAdvSearch.Click += new EventHandler(this.btnAdvSearch_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(7, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(33, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Filter:";
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Controls.Add((Control) this.btnRefreshView);
      this.gradientPanel1.Controls.Add((Control) this.btnEditView);
      this.gradientPanel1.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.cboView);
      this.gradientPanel1.Controls.Add((Control) this.label3);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(983, 32);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 4;
      this.btnRefreshView.BackColor = Color.Transparent;
      this.btnRefreshView.Location = new Point(362, 8);
      this.btnRefreshView.MouseDownImage = (Image) null;
      this.btnRefreshView.Name = "btnRefreshView";
      this.btnRefreshView.Size = new Size(16, 16);
      this.btnRefreshView.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefreshView.TabIndex = 7;
      this.btnRefreshView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRefreshView, "Reset View");
      this.btnRefreshView.Click += new EventHandler(this.btnRefreshView_Click);
      this.btnEditView.BackColor = Color.Transparent;
      this.btnEditView.Location = new Point(384, 8);
      this.btnEditView.MouseDownImage = (Image) null;
      this.btnEditView.Name = "btnEditView";
      this.btnEditView.Size = new Size(16, 16);
      this.btnEditView.StandardButtonType = StandardIconButton.ButtonType.ManageButton;
      this.btnEditView.TabIndex = 6;
      this.btnEditView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnEditView, "Manage View");
      this.btnEditView.Click += new EventHandler(this.btnEditView_Click);
      this.btnSaveView.BackColor = Color.Transparent;
      this.btnSaveView.Location = new Point(340, 8);
      this.btnSaveView.MouseDownImage = (Image) null;
      this.btnSaveView.Name = "btnSaveView";
      this.btnSaveView.Size = new Size(16, 16);
      this.btnSaveView.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSaveView.TabIndex = 5;
      this.btnSaveView.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSaveView, "Save View");
      this.btnSaveView.Click += new EventHandler(this.btnSaveView_Click);
      this.cboView.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboView.FormattingEnabled = true;
      this.cboView.Location = new Point(111, 5);
      this.cboView.Name = "cboView";
      this.cboView.SelectedBGColor = SystemColors.Highlight;
      this.cboView.Size = new Size(219, 21);
      this.cboView.TabIndex = 2;
      this.cboView.SelectedIndexChanged += new EventHandler(this.cboView_SelectedIndexChanged);
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(7, 8);
      this.label3.Name = "label3";
      this.label3.Size = new Size(98, 16);
      this.label3.TabIndex = 1;
      this.label3.Text = "Contacts View";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(983, 618);
      this.Controls.Add((Control) this.panelBody);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BizPartnerListForm);
      this.Text = nameof (BizPartnerListForm);
      this.Closing += new CancelEventHandler(this.BizPartnerListForm_Closing);
      this.SizeChanged += new EventHandler(this.BizPartnerListForm_SizeChanged);
      this.panelBody.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.contextMenuStrip1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.siBtnRefresh).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.pnlDetail.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnRefresh).EndInit();
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnRefreshView).EndInit();
      ((ISupportInitialize) this.btnEditView).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      this.ResumeLayout(false);
    }

    private void resetFieldDefs()
    {
      this.allContactFieldDefs = BizPartnerReportFieldDefs.GetFieldDefs(false, this.contactType);
      this.contactFieldDefsWoHistory = this.allContactFieldDefs.ExtractFields((BizPartnerReportFieldFlags) 0);
      this.contactLoanFieldDefs = BizPartnerLoanReportFieldDefs.GetFieldDefs(this.allContactFieldDefs);
    }

    private void Init()
    {
      this.initWithoutSecurity();
      this.resetFieldDefs();
      this.loadViewList(this.contactType != ContactType.BizPartner ? Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.PublicBizPartnerView, FileSystemEntry.PrivateRoot(Session.UserID)) : Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.BizPartnerView, FileSystemEntry.PrivateRoot(Session.UserID)));
      string privateProfileString = Session.GetPrivateProfileString("BizPartnerContact", "DefaultView");
      if (privateProfileString != "" && privateProfileString != this.standardViewName)
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString)), false);
      if (this.cboView.SelectedIndex < 0)
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse("Personal:\\" + Session.UserID + "\\" + this.standardViewName)), false);
      this.enforceSecurity();
      if (this.CurrentContactID != -1)
        return;
      this.CurrentContactID = -100;
    }

    private void enforceSecurity()
    {
      bool flag = false;
      if (Session.UserInfo.IsSuperAdministrator())
        flag = true;
      else if (this.gvContactList.SelectedItems.Count == 0)
        flag = false;
      else if (this.gvContactList.SelectedItems.Count == 1)
      {
        BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
        flag = tag.AccessLevel == ContactAccess.Private || Session.AclGroupManager.GetBizContactAccessRight(Session.UserInfo, tag.ContactID) == AclTriState.True;
      }
      else
      {
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
          if (tag.AccessLevel == ContactAccess.Private)
            flag = true;
          else if (Session.AclGroupManager.GetBizContactAccessRight(Session.UserInfo, tag.ContactID) != AclTriState.True)
          {
            flag = false;
            break;
          }
        }
      }
      this.btnNew.Visible = this.personaSettings[AclFeature.Cnt_Biz_CreateNew];
      this.btnDuplicate.Visible = this.personaSettings[AclFeature.Cnt_Biz_Copy];
      this.btnDelete.Visible = this.personaSettings[AclFeature.Cnt_Biz_Delete];
      this.btnExport.Visible = this.personaSettings[AclFeature.Cnt_Biz_Export];
      this.btnPrint.Visible = this.personaSettings[AclFeature.Cnt_Biz_Print];
      this.btnMailMerge.Visible = this.personaSettings[AclFeature.Cnt_Biz_MailMerge];
      this.btnSync.Visible = this.personaSettings[AclFeature.Cnt_Synchronization];
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        this.btnDuplicate.Enabled = false;
        this.btnDelete.Enabled = false;
        this.btnExport.Enabled = false;
        this.btnPrint.Enabled = false;
        this.btnMailMerge.Enabled = false;
        this.btnAddToGroup.Enabled = false;
        this.btnRemoveFromGroup.Enabled = false;
        this.btnSave.Enabled = false;
        this.btnRefresh.Enabled = false;
        this.contactDetailTab.disableControls();
        this.mailMergeDocMgmtOnly = true;
      }
      else if (this.gvContactList.SelectedItems.Count == 1)
      {
        this.btnDuplicate.Enabled = true;
        this.btnDelete.Enabled = flag;
        this.btnExport.Enabled = true;
        this.btnPrint.Enabled = true;
        this.btnMailMerge.Enabled = true;
        this.btnAddToGroup.Enabled = true;
        this.btnRemoveFromGroup.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnRefresh.Enabled = true;
        if (flag)
          this.contactDetailTab.enableControls();
        else
          this.contactDetailTab.disableControls();
        this.mailMergeDocMgmtOnly = false;
      }
      else
      {
        this.btnDuplicate.Enabled = false;
        this.btnDelete.Enabled = flag;
        this.btnExport.Enabled = true;
        this.btnPrint.Enabled = true;
        this.btnMailMerge.Enabled = true;
        this.btnAddToGroup.Enabled = true;
        this.btnRemoveFromGroup.Enabled = true;
        this.btnSave.Enabled = false;
        this.btnRefresh.Enabled = false;
        this.contactDetailTab.disableControls();
        this.mailMergeDocMgmtOnly = false;
      }
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        this.btnSync.Enabled = false;
      if (!this.personaSettings[AclFeature.Cnt_Biz_MailMerge] && !this.personaSettings[AclFeature.Cnt_Synchronization])
        this.verticalSeparator2.Visible = false;
      else
        this.verticalSeparator2.Visible = true;
    }

    private void initWithoutSecurity()
    {
      this.contactDetailTab = new BizPartnerTabForm(this, this.contactType);
      this.contactDetailTab.TopLevel = false;
      this.contactDetailTab.Visible = true;
      this.contactDetailTab.Dock = DockStyle.Fill;
      this.contactDetailTab.DataChanged += new BizPartnerSummaryChangeEventHandler(this.contactDetailTab_DataChanged);
      this.contactDetailTab.RequireContactListRefresh += new EventHandler(this.contactDetailTab_RequireContactListRefresh);
      this.contactDetailTab.ContactDeleted += new ContactDeletedEventHandler(this.contactDeletedHandler);
      this.panelBottom.Controls.Clear();
      this.panelBottom.Controls.Add((Control) this.contactDetailTab);
      this.RefreshBizCategories();
    }

    private void contactDetailTab_DataChanged()
    {
      this.btnRefresh.Enabled = true;
      this.btnSave.Enabled = true;
    }

    private void contactDetailTab_RequireContactListRefresh(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
        return;
      BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
      if (!this.saveChanges(true))
        return;
      this.refreshContactInList(tag.ContactID, true);
    }

    public void ShowContacts(
      QueryCriterion criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields,
      string description)
    {
      this.retrieveContactData(criteria, matchType, sortFields);
      this.lblFilter.Text = description;
    }

    public bool IsMenuItemEnabled(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          flag = this.btnSync.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_NewContact:
          flag = this.btnNew.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_DuplicateContact:
          flag = this.btnDuplicate.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_DeleteContact:
          flag = this.btnDelete.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExport:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected:
          flag = this.btnExport.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportAll:
          flag = this.gvContactList.Items.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_PrintDetails:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails:
          flag = this.btnPrint.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails:
          flag = this.gvContactList.Items.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_MailMerge:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected:
          flag = this.btnMailMerge.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeAll:
          flag = this.gvContactList.Items.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_AddToGroup:
          flag = this.btnAddToGroup.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup:
          flag = this.btnRemoveFromGroup.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_EditGroup:
          flag = this.btnEditGroups.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ImportContact:
          flag = this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Import);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_SaveView:
          flag = this.btnSaveView.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ResetView:
          flag = this.btnRefreshView.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ManageView:
          flag = this.btnEditView.Enabled;
          break;
      }
      return flag;
    }

    public bool IsMenuItemVisible(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          flag = this.btnSync.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_NewContact:
          flag = this.btnNew.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_DuplicateContact:
          flag = this.btnDuplicate.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_DeleteContact:
          flag = this.btnDelete.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExport:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected:
          flag = this.btnExport.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_PrintDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails:
          flag = this.btnPrint.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_MailMerge:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeAll:
          flag = this.btnMailMerge.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_AddToGroup:
          flag = this.btnAddToGroup.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup:
          flag = this.btnRemoveFromGroup.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_EditGroup:
          flag = this.btnEditGroups.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ImportContact:
          flag = this.personaSettings[AclFeature.Cnt_Biz_Import];
          break;
        case ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_SaveView:
          flag = this.btnSaveView.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ResetView:
          flag = this.btnRefreshView.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ManageView:
          flag = this.btnEditView.Visible;
          break;
      }
      return flag;
    }

    public void TriggerContactAction(ContactMainForm.ContactsActionEnum action)
    {
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          this.btnSync.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_NewContact:
          this.btnNew_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_DuplicateContact:
          this.btnCopy_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_DeleteContact:
          this.btnDelete_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ExportSelectedExcel:
          this.btnExport_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ExportAllExcel:
          this.exportContactsToExcel(true);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_PrintSelectedDetails:
          this.btnPrint_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_PrintAllDetails:
          this.PrintContactBusiness(true);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeSelected:
          this.btnMailMerge.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_MailMergeAll:
          this.mailMerge(true);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_AddToGroup:
          this.btnAddToGroup.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_RemoveFromGroup:
          this.btnRemoveFromGroup.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_EditGroup:
          this.btnEditGroups.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ImportContact:
          this.ImportContacts();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportAll:
          this.ExportContacts(true);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_CSVExportSelected:
          this.ExportContacts(false);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_CustomizeColumns:
          this.gvLayoutManager.CustomizeColumns();
          break;
        case ContactMainForm.ContactsActionEnum.Biz_SaveView:
          this.btnSaveView_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ResetView:
          this.btnRefreshView_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Biz_ManageView:
          this.btnEditGroups.PerformClick();
          break;
      }
    }

    private void loadViewList(FileSystemEntry[] fsEntries)
    {
      this.suspendEvents = true;
      try
      {
        this.cboView.Items.Clear();
        this.cboView.Dividers.Clear();
        foreach (FileSystemEntry fsEntry in fsEntries)
          this.cboView.Items.Add((object) new FileSystemEntryListItem(fsEntry));
        this.cboView.Dividers.Add(this.cboView.Items.Count);
        this.cboView.Items.Add((object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.standardViewName, FileSystemEntry.Types.File, Session.UserID)));
        if (this.currentView == null)
          return;
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, Session.UserID)), false);
      }
      catch (Exception ex)
      {
        Tracing.Log(BizPartnerListForm.sw, BizPartnerListForm.className, TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    public void GoToContact(int contactID)
    {
      this.btnClearSearch_Click((object) null, (EventArgs) null);
      this.retrieveContactData((QueryCriterion) new OrdinalValueCriterion("Contact.ContactID", (object) contactID), RelatedLoanMatchType.None, (SortField[]) null);
      this.displayCurrentPage(false);
      if (this.gvContactList.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access right to the selected contact.");
      }
      else
        this.gvContactList.Items[0].Selected = true;
    }

    private void buildQuerySummary(bool populateLabel)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      this.lblFilter.Text = fieldFilterList.ToString(true);
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    private void retrieveContactData()
    {
      this.retrieveContactData((QueryCriterion) null, RelatedLoanMatchType.None, (SortField[]) null);
    }

    private void retrieveContactData(
      QueryCriterion filter,
      RelatedLoanMatchType matchType,
      SortField[] sortFields)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.gvContactList.SelectedItems.Count == 1)
          this.saveChanges(true);
        string[] fieldList = this.generateFieldList();
        if (filter == null)
        {
          filter = this.generateQueryCriteria();
          matchType = this.advFilter == null ? RelatedLoanMatchType.None : this.advFilter.RelatedLoanMatchType;
        }
        if (sortFields == null)
          sortFields = this.generateSortFields();
        this.suspendEvents = true;
        if (this.contactCursor != null)
        {
          this.contactCursor.Dispose();
          this.contactCursor = (ICursor) null;
        }
        if (filter != null)
        {
          if (this.contactType != ContactType.PublicBiz)
            this.contactCursor = Session.ContactManager.OpenBizPartnerCursor(new QueryCriterion[1]
            {
              filter
            }, matchType, sortFields, fieldList, true);
          else
            this.contactCursor = Session.ContactManager.OpenPublicBizPartnerCursor(new QueryCriterion[1]
            {
              filter
            }, matchType, sortFields, fieldList, true);
        }
        else
          this.contactCursor = this.contactType == ContactType.PublicBiz ? Session.ContactManager.OpenPublicBizPartnerCursor(new QueryCriterion[0], RelatedLoanMatchType.None, sortFields, fieldList, true) : Session.ContactManager.OpenBizPartnerCursor(new QueryCriterion[0], RelatedLoanMatchType.None, sortFields, fieldList, true);
        this.navContacts.NumberOfItems = this.contactCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Contacts: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
        this.suspendEvents = false;
      }
    }

    private QueryCriterion generateQueryCriteria()
    {
      QueryCriterion criterion = (QueryCriterion) null;
      if (this.advFilter != null)
        criterion = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriterion = this.gvFilterManager.ToQueryCriterion();
      QueryCriterion queryCriteria = (QueryCriterion) null;
      if (criterion != null)
        queryCriteria = queryCriteria == null ? criterion : queryCriteria.And(criterion);
      if (queryCriterion != null)
        queryCriteria = queryCriteria == null ? queryCriterion : queryCriteria.And(queryCriterion);
      return queryCriteria;
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (EllieMae.EMLite.ClientServer.TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefsWoHistory.GetFieldByCriterionName(column.ColumnID);
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

    private SortField[] generateSortFields()
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumn column in this.gvContactList.Columns)
      {
        if (column.SortOrder != SortOrder.None)
        {
          EllieMae.EMLite.ClientServer.TableLayout.Column tag = (EllieMae.EMLite.ClientServer.TableLayout.Column) column.Tag;
          sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == column.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
        }
      }
      return sortFieldList.ToArray();
    }

    private ContactView getDefaultContactView()
    {
      ContactView defaultContactView = new ContactView(this.standardViewName);
      EllieMae.EMLite.ClientServer.TableLayout tableLayout = new EllieMae.EMLite.ClientServer.TableLayout();
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("ContactGroupCount.GroupCount", "Groups", HorizontalAlignment.Left, 70));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.CategoryID", "Category", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.CompanyName", "Company", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.FirstName", "Contact First Name", HorizontalAlignment.Left, 105));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.LastName", "Contact Last Name", HorizontalAlignment.Left, 105));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.JobTitle", "Title", HorizontalAlignment.Left, 95));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.WorkPhone", "Work Phone", HorizontalAlignment.Left, 91));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.MobilePhone", "Cell Phone", HorizontalAlignment.Left, 91));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.BizEmail", "Work Email", HorizontalAlignment.Left, 91));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.AccessLevel", "Public", HorizontalAlignment.Left, 90));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("NextAppointment.StartDateTime", "Next Appointment Date/Time", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new EllieMae.EMLite.ClientServer.TableLayout.Column("Contact.LastModified", "Last Modification", HorizontalAlignment.Left, 90));
      defaultContactView.Layout = tableLayout;
      return defaultContactView;
    }

    private string categoryIdToName(int catId)
    {
      string name = this.catUtil.CategoryIdToName(catId);
      if (catId >= 0 && name == "")
      {
        this.catUtil = new BizCategoryUtil(Session.SessionObjects);
        name = this.catUtil.CategoryIdToName(catId);
      }
      return name;
    }

    public bool SaveChanges() => this.saveChanges(false);

    private bool saveChanges(bool prompt)
    {
      return prompt ? this.saveChanges(prompt, false) : this.saveChanges(prompt, true);
    }

    private bool saveChanges(bool prompt, bool makeSelected)
    {
      if (this.currentContactId == -1)
        return false;
      this.suspendEvents = true;
      bool flag = true;
      if (this.contactDetailTab.isDirty())
      {
        flag = this.contactDetailTab.SaveChanges(prompt);
        if (flag)
          this.refreshContactInList(this.currentContactId, makeSelected);
      }
      this.btnRefresh.Enabled = !flag;
      this.btnSave.Enabled = !flag;
      this.suspendEvents = !flag;
      return flag;
    }

    public void InsertContactIntoList(int contactID)
    {
      this.gvContactList.Items.Add(this.getGVItemForContact(contactID));
      this.gvContactList.EnsureVisible(this.gvContactList.Items.Count - 1);
    }

    private void refreshContactInList(int contactID, bool makeSelect)
    {
      int num = -1;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
      {
        if (((BizPartnerSummaryInfo) gvItem.Tag).ContactID == contactID)
        {
          num = gvItem.Index;
          break;
        }
      }
      if (num < 0)
        return;
      GVItem gvItemForContact = this.getGVItemForContact(contactID);
      this.gvContactList.Items.RemoveAt(num);
      this.gvContactList.Items.Insert(num, gvItemForContact);
      if (!makeSelect)
        return;
      this.gvContactList.Items[num].Selected = true;
    }

    private GVItem getGVItemForContact(int contactID)
    {
      GVItem gvItem = new GVItem();
      using (ICursor cursor = Session.ContactManager.OpenBizPartnerCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new OrdinalValueCriterion("Contact.contactID", (object) contactID, OrdinalMatchType.Equals)
      }, RelatedLoanMatchType.None, (SortField[]) null, this.generateFieldList(), true))
        return this.createGVItem(cursor.GetItem(0) as BizPartnerSummaryInfo);
    }

    private void setContactID(int contactId)
    {
      if (this.gvContactList.SelectedItems.Count == 1)
      {
        BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
        if (Session.ContactManager.GetBizPartner(tag.ContactID) == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Unable to retrieve business contact '" + tag.LastName + ", " + tag.FirstName + "'. The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.contactDeletedHandler(tag.ContactID);
          return;
        }
        this.enforceSecurity();
      }
      if (this.currentContactId == contactId)
        return;
      this.contactDetailTab.CurrentContactID = contactId;
      this.currentContactId = contactId;
    }

    public int CurrentContactID
    {
      get => this.currentContactId;
      set
      {
        this.setContactID(value);
        this.btnRefresh.Enabled = false;
        this.btnSave.Enabled = false;
      }
    }

    public void RefreshDataSet()
    {
      this.SaveChanges();
      this.contactDetailTab.InitDynamicTabs();
      this.RefreshBizCategories();
      this.RefreshList();
    }

    public void RefreshDataSetWOSave()
    {
      this.contactDetailTab.InitDynamicTabs();
      this.RefreshBizCategories();
      if (this.btnSave.Enabled)
        return;
      this.RefreshList();
    }

    public void RefreshList() => this.RefreshList(false);

    public void RefreshList(bool preserverSelection)
    {
      if (this.suspendRefresh)
        return;
      using (CursorActivator.Wait())
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("BizContacts.Refresh", "Refresh the Business Contact screen data", true, 1704, nameof (RefreshList), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\BizPartnerListForm.cs"))
        {
          this.retrieveContactData();
          this.displayCurrentPage(preserverSelection);
          this.refreshFilterDescription();
          performanceMeter.AddVariable("ContactCount", (object) this.navContacts.NumberOfItems);
          performanceMeter.AddVariable("Columns", (object) this.gvContactList.Columns.Count);
          performanceMeter.AddVariable("Filter", (object) this.lblFilter.Text);
          GVColumnSort[] sortOrder = this.gvContactList.Columns.GetSortOrder();
          if (sortOrder.Length == 0)
            return;
          performanceMeter.AddVariable("Sort", this.gvContactList.Columns[sortOrder[0].Column].Tag);
        }
      }
    }

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      return fieldFilterList;
    }

    private void refreshFilterDescription()
    {
      FieldFilterList currentFilter = this.GetCurrentFilter();
      if (currentFilter.Count == 0)
      {
        this.lblFilter.Text = "None";
        this.btnClearSearch.Enabled = false;
      }
      else
      {
        this.lblFilter.Text = currentFilter.ToString(true);
        this.btnClearSearch.Enabled = true;
      }
    }

    private string truncateString(string value, int length)
    {
      if (value == null)
        return string.Empty;
      return value.Length > length ? value.Substring(0, length) : value;
    }

    private void btnNew_Click(object sender, EventArgs e) => this.CreateNew();

    public void CreateNew()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        BizPartnerInfo info = new BizPartnerInfo();
        info.CompanyName = "<New Contact>";
        int id = this.catUtil.CategoryNameToId("No Category");
        if (id >= 0)
          info.CategoryID = id;
        if (this.contactType == ContactType.PublicBiz)
        {
          if (this.listState == ContactListState.Group)
          {
            if (this.nameForState != "All Contacts")
            {
              info.AccessLevel = ContactAccess.Public;
              info.OwnerID = "";
            }
            else
            {
              info.AccessLevel = ContactAccess.Private;
              info.OwnerID = Session.UserID;
            }
          }
          else
          {
            info.AccessLevel = ContactAccess.Private;
            info.OwnerID = Session.UserID;
          }
        }
        else
        {
          info.AccessLevel = ContactAccess.Private;
          info.OwnerID = Session.UserID;
        }
        int bizPartner1 = Session.ContactManager.CreateBizPartner(info, DateTime.Now, ContactSource.Entered);
        BizPartnerInfo bizPartner2 = Session.ContactManager.GetBizPartner(bizPartner1);
        if (this.listState == ContactListState.Group && this.nameForState != "All Contacts")
        {
          ContactGroupInfo contactGroup = Session.ContactGroupManager.GetContactGroup(this.idForState);
          if (contactGroup != (ContactGroupInfo) null)
          {
            contactGroup.AddedContactIds = new int[1]
            {
              bizPartner1
            };
            Session.ContactGroupManager.SaveContactGroup(contactGroup);
          }
        }
        this.gvContactList.SelectedItems.Clear();
        GVItem gvItem = this.createGVItem(new BizPartnerSummaryInfo(bizPartner2));
        this.gvContactList.Items.Add(gvItem);
        gvItem.Selected = true;
        this.gvContactList.EnsureVisible(gvItem.Index);
        this.contactDetailTab.ActivateDetailTab();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.getSelectedItemsCount() == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You need to select a contact to delete.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        bool flag = true;
        DialogResult dialogResult = DialogResult.No;
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
          if (tag != null)
          {
            if (tag.AccessLevel == ContactAccess.Public && !this.checkDeleteRight(tag.ContactID, tag.AccessLevel))
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "You can not delete this public contact '" + tag.FirstName + " " + tag.LastName + "'.  You do not have edit rights to one or more groups this contact belongs to.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              if (flag)
              {
                ConfirmDialog confirmDialog = new ConfirmDialog("Delete Contact", "Are you sure you want to delete contact '" + tag.LastName + ", " + tag.FirstName + "'?", (this.gvContactList.SelectedItems.Count > 1 ? 1 : 0) != 0);
                dialogResult = confirmDialog.ShowDialog();
                flag = !confirmDialog.ApplyToAll;
              }
              if (dialogResult == DialogResult.Yes)
              {
                try
                {
                  Session.ContactManager.DeleteBizPartner(tag.ContactID);
                }
                catch (Exception ex)
                {
                  int num3 = (int) Utils.Dialog((IWin32Window) this, "Unable to delete business contact '" + tag.LastName + ", " + tag.FirstName + "'. The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                selectedItem.Selected = false;
              }
            }
          }
        }
        this.RefreshList();
      }
    }

    private bool checkDeleteRight(int contactID, ContactAccess accessLevel)
    {
      if (Session.UserInfo.IsSuperAdministrator())
        return true;
      ContactGroupInfo[] contactGroupInfoArray = accessLevel != ContactAccess.Private ? Session.ContactGroupManager.GetContactGroupsForContact(ContactType.PublicBiz, contactID) : Session.ContactGroupManager.GetContactGroupsForContact(this.contactType, contactID);
      if ((contactGroupInfoArray == null || contactGroupInfoArray.Length == 0) && Session.UserInfo.IsTopLevelAdministrator())
        return true;
      BizGroupRef[] contactGroupRefs = Session.AclGroupManager.GetBizContactGroupRefs(Session.UserID, true);
      foreach (ContactGroupInfo contactGroupInfo in contactGroupInfoArray)
      {
        bool flag = false;
        foreach (BizGroupRef bizGroupRef in contactGroupRefs)
        {
          if (bizGroupRef.BizGroupID == contactGroupInfo.GroupId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
      }
      return true;
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
      if (this.getSelectedItemsCount() == 0)
      {
        int num1 = (int) MessageBox.Show("Select a contact to duplicate.", "Copy Contact");
      }
      else if (this.getSelectedItemsCount() > 1)
      {
        int num2 = (int) MessageBox.Show("You can only select one contact at a time to duplicate.", "Copy Contact");
      }
      else
      {
        BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
        if (tag == null)
        {
          int num3 = (int) MessageBox.Show("The selected Contact no longer exists.", "Copy Contact");
        }
        else
        {
          BizPartnerInfo bizPartner1 = Session.ContactManager.GetBizPartner(tag.ContactID);
          ContactGroupInfo[] groupsForContact = Session.ContactGroupManager.GetContactGroupsForContact(ContactType.BizPartner, this.currentContactId);
          if (bizPartner1 == null)
          {
            int num4 = (int) MessageBox.Show("The selected Contact no longer exists.", "Copy Contact");
          }
          else
          {
            bizPartner1.AccessLevel = ContactAccess.Private;
            bizPartner1.OwnerID = Session.UserID;
            int bizPartner2 = Session.ContactManager.CreateBizPartner(bizPartner1, DateTime.Now, ContactSource.Entered);
            ContactCustomField[] fieldsForContact = Session.ContactManager.GetCustomFieldsForContact(bizPartner1.ContactID, ContactType.BizPartner);
            if (fieldsForContact != null)
            {
              for (int index = 0; index < fieldsForContact.Length; ++index)
                fieldsForContact[index].ContactID = bizPartner2;
              Session.ContactManager.UpdateCustomFieldsForContact(bizPartner2, ContactType.BizPartner, fieldsForContact);
            }
            else
              Tracing.Log(BizPartnerListForm.sw, TraceLevel.Error, BizPartnerListForm.className, "Cannot get custom fields for business partner id " + (object) bizPartner1.ContactID);
            for (int index = 0; index < groupsForContact.Length; ++index)
            {
              groupsForContact[index].AddedContactIds = new int[1]
              {
                bizPartner2
              };
              Session.ContactGroupManager.SaveContactGroup(groupsForContact[index]);
            }
            CustomFieldValueCollection fieldValueCollection1 = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(bizPartner1.ContactID, bizPartner1.CategoryID));
            if (fieldValueCollection1 != null && fieldValueCollection1.Count > 0)
            {
              CustomFieldValueCollection fieldValueCollection2 = CustomFieldValueCollection.GetCustomFieldValueCollection(Session.SessionObjects, new CustomFieldValueCollection.Criteria(bizPartner2, bizPartner1.CategoryID));
              foreach (CustomFieldValue customFieldValue1 in (CollectionBase) fieldValueCollection1)
              {
                CustomFieldValue customFieldValue2 = CustomFieldValue.NewCustomFieldValue(customFieldValue1.FieldId, bizPartner2, customFieldValue1.FieldFormat);
                customFieldValue2.FieldValue = customFieldValue1.FieldValue;
                fieldValueCollection2.Add(customFieldValue2);
              }
              fieldValueCollection2.Save();
            }
            this.RefreshList(false);
          }
        }
      }
    }

    private BizPartnerInfo[] getSelectedContacts(bool excludeNoSpam, bool includeAllContacts)
    {
      this.SaveChanges();
      ArrayList arrayList1 = new ArrayList((ICollection) this.getSelectedContactIDs(excludeNoSpam, includeAllContacts));
      if (arrayList1.Count == 0)
        return new BizPartnerInfo[0];
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < arrayList1.Count; index += 100)
      {
        int count = Math.Min(100, arrayList1.Count - index);
        BizPartnerInfo[] bizPartners = Session.ContactManager.GetBizPartners((int[]) arrayList1.GetRange(index, count).ToArray(typeof (int)));
        arrayList2.AddRange((ICollection) bizPartners);
      }
      return (BizPartnerInfo[]) arrayList2.ToArray(typeof (BizPartnerInfo));
    }

    private int[] getSelectedContactIDs(bool excludeNoSpam, bool selectAllContacts)
    {
      List<int> intList1 = new List<int>();
      int[] selectedSpamContactIds = this.getSelectedSpamContactIDs(selectAllContacts);
      List<int> intList2 = new List<int>();
      intList2.AddRange((IEnumerable<int>) selectedSpamContactIds);
      if (!selectAllContacts)
      {
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
          if (tag != null && (!excludeNoSpam || intList2.Contains(tag.ContactID)))
            intList1.Add(tag.ContactID);
        }
      }
      else
      {
        foreach (BizPartnerSummaryInfo partnerSummaryInfo in this.contactCursor.GetItems(0, this.contactCursor.GetItemCount()))
        {
          if (!excludeNoSpam || intList2.Contains(partnerSummaryInfo.ContactID))
            intList1.Add(partnerSummaryInfo.ContactID);
        }
      }
      return intList1.ToArray();
    }

    private BizPartnerInfo[] getSelectedSpamContactsWithNoEmail(bool selectAllContacts)
    {
      List<BizPartnerInfo> bizPartnerInfoList = new List<BizPartnerInfo>();
      int[] selectedSpamContactIds = this.getSelectedSpamContactIDs(selectAllContacts);
      List<int> intList = new List<int>();
      intList.AddRange((IEnumerable<int>) selectedSpamContactIds);
      if (!selectAllContacts)
      {
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
          if (tag != null && intList.Contains(tag.ContactID))
          {
            BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(tag.ContactID);
            if (bizPartner == null || bizPartner.BizEmail.Trim() != string.Empty)
              bizPartnerInfoList.Add(bizPartner);
          }
        }
      }
      else
      {
        foreach (BizPartnerSummaryInfo allContact in this.getAllContacts())
        {
          if (intList.Contains(allContact.ContactID))
          {
            BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(allContact.ContactID);
            if (bizPartner != null && !(bizPartner.BizEmail.Trim() != string.Empty))
              bizPartnerInfoList.Add(bizPartner);
          }
        }
      }
      return bizPartnerInfoList.ToArray();
    }

    private int[] getSelectedSpamContactIDs(bool selectAllContacts)
    {
      QueryCriterion queryCriterion = (QueryCriterion) null;
      List<int> intList1 = new List<int>();
      if (!selectAllContacts)
      {
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
          intList1.Add(tag.ContactID);
        }
      }
      else
      {
        foreach (BizPartnerSummaryInfo allContact in this.getAllContacts())
          intList1.Add(allContact.ContactID);
      }
      if (intList1.Capacity > 0)
        queryCriterion = (QueryCriterion) new ListValueCriterion("Contact.ContactID", (Array) intList1.ToArray());
      string[] valueList = new string[1]{ "N" };
      BizPartnerSummaryInfo[] partnerSummaryInfoArray = Session.ContactManager.QueryBizPartnerSummaries(new QueryCriterion[1]
      {
        queryCriterion.And((QueryCriterion) new ListValueCriterion("Contact.DoNotSpam", (Array) valueList, true))
      }, RelatedLoanMatchType.None);
      List<int> intList2 = new List<int>();
      foreach (BizPartnerSummaryInfo partnerSummaryInfo in partnerSummaryInfoArray)
        intList2.Add(partnerSummaryInfo.ContactID);
      return intList2.ToArray();
    }

    private void LogInfo(string msg)
    {
      Tracing.Log(BizPartnerListForm.sw, TraceLevel.Info, BizPartnerListForm.className, msg);
    }

    private void btnMailMerge_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessMailMergeUsageCounter", (SFxTag) new SFxUiTag());
      this.mailMerge(false);
    }

    private void btnMailMergeAll_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessMailMergeAllUsageCounter", (SFxTag) new SFxUiTag());
      this.mailMerge(true);
    }

    private void mailMerge(bool includeAllContacts)
    {
      this.SaveChanges();
      SystemSettings.DeleteTempFiles();
      MailMergeForm mailMergeForm = new MailMergeForm(false, ContactType.BizPartner, this.mailMergeDocMgmtOnly);
      int num1 = (int) mailMergeForm.ShowDialog();
      if (mailMergeForm.LetterFile == null)
        return;
      this.LogInfo("Custom letter " + (object) mailMergeForm.LetterFile + " is selected.");
      if (!includeAllContacts && this.getSelectedItemsCount() <= 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please select the contacts that you want to send letters to.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LogInfo("No contacts are selected. MailMerge ends.");
      }
      else
      {
        int[] contactIds;
        if (mailMergeForm.Action == "MailMerge")
        {
          contactIds = this.getSelectedContactIDs(true, includeAllContacts);
        }
        else
        {
          if (!(mailMergeForm.Action == "EmailMerge"))
            throw new ApplicationException("Mail merge action is not valid.");
          contactIds = this.getSelectedSpamContactIDs(includeAllContacts);
        }
        string str = "";
        bool flag = false;
        string[] emailOptions = (string[]) null;
        if (mailMergeForm.Action == "EmailMerge")
        {
          BizPartnerInfo[] selectedContacts = this.getSelectedContacts(true, includeAllContacts);
          if (selectedContacts.Length != 0)
          {
            ContactsEmailSelection contactsEmailSelection = new ContactsEmailSelection((object[]) selectedContacts, ContactType.BizPartner, true);
            if (DialogResult.Cancel == contactsEmailSelection.ShowDialog((IWin32Window) this))
              return;
            contactIds = contactsEmailSelection.SelectedContactIds;
            emailOptions = contactsEmailSelection.EmailOptions;
          }
          if (contactIds.Length != 0)
            flag = true;
        }
        if (flag)
        {
          EmailInfoForm emailInfoForm = new EmailInfoForm();
          if (emailInfoForm.ShowDialog() == DialogResult.Cancel)
            return;
          str = emailInfoForm.EmailSubject;
        }
        if (contactIds.Length == 0 & flag)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "No custom letters need to be sent to the contacts you selected. They either have empty email addresses or are marked as not to receive emails.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          OutlookServices outlookServices = (OutlookServices) null;
          try
          {
            if (flag)
            {
              if (ContactUtils.GetCurrentMailDeliveryMethod() == EmailDeliveryMethod.Outlook)
                outlookServices = new OutlookServices();
              this.performEmailMerge(contactIds, mailMergeForm.LetterFile, str, emailOptions, Session.UserID);
              Cursor.Current = Cursors.WaitCursor;
              ContactUtils.addEmailMergeHistory(contactIds, ContactType.BizPartner, mailMergeForm.LetterFile.Name, str);
            }
            else
            {
              if (!ContactUtils.DoMailMerge(contactIds, ContactType.BizPartner, mailMergeForm.LetterFile, mailMergeForm.IsPrintPreview, false, "", (string[]) null, Session.UserID))
                return;
              ContactUtils.addMailMergeHistory(contactIds, ContactType.BizPartner, mailMergeForm.LetterFile.Name);
            }
          }
          catch (Exception ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Tracing.Log(BizPartnerListForm.sw, TraceLevel.Error, BizPartnerListForm.className, "Error occurred in MailMerge.");
            Tracing.Log(BizPartnerListForm.sw, TraceLevel.Error, BizPartnerListForm.className, ex.StackTrace);
          }
          finally
          {
            outlookServices?.Dispose();
          }
        }
      }
    }

    private void performEmailMerge(
      int[] contactIds,
      FileSystemEntry letterEntry,
      string emailSubject,
      string[] emailOptions,
      string senderUserID)
    {
      ContactUtils.ContactIDs = contactIds;
      ContactUtils.TypeOfContacts = ContactType.BizPartner;
      ContactUtils.LetterEntry = letterEntry;
      ContactUtils.IsPrintPreview = false;
      ContactUtils.IsEmailMerge = true;
      ContactUtils.EmailSubject = emailSubject;
      ContactUtils.EmailAddressOption = emailOptions;
      ContactUtils.SenderUserID = senderUserID;
      if (new ProgressDialog("Sending Emails to Contacts", new AsynchronousProcess(ContactUtils.DoAsyncMailMerge), (object) null, false).ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Custom letters were successfully sent to the contacts you selected.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public void ImportContacts()
    {
      this.SaveChanges();
      ContactImportWizard contactImportWizard = new ContactImportWizard(ContactType.BizPartner);
      contactImportWizard.ContactImported += new ContactImportedEventHandler(this.onContactImported);
      if (contactImportWizard.ShowDialog((IWin32Window) this.ParentForm) != DialogResult.OK)
        return;
      this.RefreshList();
    }

    public void ExportContacts(bool exportAll)
    {
      if (!exportAll && this.getSelectedItemsCount() <= 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select the contacts you want to export.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (exportAll && this.gvContactList.Items.Count == 0)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "There are no items in the list to be exported.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int[] selectedContactIds = this.getSelectedContactIDs(false, exportAll);
        this.SaveChanges();
        ContactExportWizard contactExportWizard = new ContactExportWizard(ContactType.BizPartner, selectedContactIds);
        contactExportWizard.ContactExported += new ContactExportedEventHandler(this.onContactExported);
        contactExportWizard.ShowDialog((IWin32Window) this.ParentForm);
      }
    }

    public void RefreshBizCategories()
    {
      this.contactDetailTab.ContactInfoForm.RefreshData();
      this.catUtil = new BizCategoryUtil(Session.SessionObjects);
    }

    private void onContactExported(object contact)
    {
    }

    private void onContactImported(object contact)
    {
    }

    private void gvContactList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      try
      {
        this.Cursor = Cursors.WaitCursor;
        if (this.gvContactList.SelectedItems.Count == 1)
        {
          BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
          if (tag != null)
          {
            if (tag.ContactID == this.currentContactId)
              return;
            this.saveChanges(true, false);
            this.enforceSecurity();
            this.CurrentContactID = tag.ContactID;
          }
          else
          {
            this.saveChanges(true, false);
            this.enforceSecurity();
            this.CurrentContactID = -1;
          }
        }
        else
        {
          this.saveChanges(true);
          this.enforceSecurity();
          this.CurrentContactID = -1;
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.saveChanges(true);
        this.enforceSecurity();
        this.CurrentContactID = -1;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
      this.RefreshHelpText();
    }

    private void BizPartnerListForm_Closing(object sender, CancelEventArgs e) => this.SaveChanges();

    private void contactDeletedHandler(int contactId)
    {
      this.contactDetailTab.ClearChanges();
      this.currentContactId = -1;
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.refreshListCallback));
    }

    private void refreshListCallback(object notUsed)
    {
      this.Invoke((Delegate) new MethodInvoker(this.RefreshList));
    }

    private int getSelectedItemsCount() => this.gvContactList.SelectedItems.Count;

    private bool canDelete()
    {
      bool flag1 = Session.UserInfo.IsSuperAdministrator();
      bool flag2 = true;
      foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
      {
        BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
        if (tag != null)
        {
          ContactAccess accessLevel = tag.AccessLevel;
          if (!flag1 && accessLevel == ContactAccess.Public)
          {
            flag2 = false;
            break;
          }
        }
      }
      return flag2;
    }

    public void PrintContactBusiness(bool includeAllContacts)
    {
      BizPartnerInfo[] selectedContacts = this.getSelectedContacts(false, includeAllContacts);
      if (selectedContacts.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one contact in order to print.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        ContactPrintDialog contactPrintDialog = new ContactPrintDialog(ContactType.BizPartner);
        if (contactPrintDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        new PdfFormFacade().ProcessContactPrint(selectedContacts, contactPrintDialog.PrintSummary, contactPrintDialog.PrintPages, contactPrintDialog.PrintPreview);
        this.Focus();
      }
    }

    private void btnPrint_Click(object sender, EventArgs e) => this.PrintContactBusiness(false);

    private void btnPrintAll_Click(object sender, EventArgs e) => this.PrintContactBusiness(true);

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
      this.SetCurrentFilter((FieldFilterList) null);
    }

    public void RefreshHelpText(ItemCheckEventArgs e)
    {
      int num1 = 0;
      if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
        num1 = 1;
      else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
        num1 = -1;
      int num2 = this.getSelectedItemsCount() + num1;
      IStatusDisplay service = Session.Application.GetService<IStatusDisplay>();
      if (num2 <= 0)
        service.DisplayHelpText("Press F1 for Help");
      else
        service.DisplayHelpText(num2.ToString() + " contacts selected");
    }

    private void gvContacts_DoubleClick(object sender, EventArgs e)
    {
      if (!this.collapsibleSplitter1.IsCollapsed)
        return;
      this.collapsibleSplitter1.ToggleState();
    }

    private void btnAddToGroup_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessAddToGroupUsageCounter", (SFxTag) new SFxUiTag());
      if (this.getSelectedItemsCount() == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the contacts you want to add to a group.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(this.contactType, true);
        if (groupSelectionDlg.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
        {
          this.resetFieldDefs();
          this.gvFilterManager.RefreshFilterContent();
          this.RefreshList();
        }
        else
        {
          ContactGroupInfo[] selectedGroups = groupSelectionDlg.SelectedGroups;
          if (selectedGroups != null && selectedGroups.Length != 0)
          {
            foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
            {
              BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
              BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(tag.ContactID);
              if (bizPartner != null && bizPartner.AccessLevel == ContactAccess.Private && this.contactType == ContactType.PublicBiz)
              {
                bizPartner.AccessLevel = ContactAccess.Public;
                tag.AccessLevel = ContactAccess.Public;
                bizPartner.OwnerID = "";
                Session.ContactManager.UpdateBizPartner(bizPartner);
                if (this.gvContactList.SelectedItems.Count == 1)
                  this.contactDetailTab.SynchronizeBizPartnerInfo(bizPartner);
              }
            }
            for (int index = 0; index < selectedGroups.Length; ++index)
            {
              ISet a = (ISet) new HashedSet((ICollection) selectedGroups[index].ContactIds);
              ISet set = new HashedSet((ICollection) this.getSelectedContactIDs(false, false)).Minus(a);
              selectedGroups[index].AddedContactIds = new int[set.Count];
              set.CopyTo((Array) selectedGroups[index].AddedContactIds, 0);
              Session.ContactGroupManager.SaveContactGroup(selectedGroups[index]);
            }
          }
          if (this.gvContactList.SelectedItems.Count == 1)
          {
            if (this.contactType == ContactType.PublicBiz)
              this.contactDetailTab.ContactInfoForm.MakePublic();
            this.contactDetailTab.SaveChanges();
          }
          this.resetFieldDefs();
          this.gvFilterManager.RefreshFilterContent();
          this.RefreshList();
        }
      }
    }

    private void btnRemoveFromGroup_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessRemoveFromGroupUsageCounter", (SFxTag) new SFxUiTag());
      if (this.getSelectedItemsCount() == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the contacts you want to remove from a group(s).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(this.contactType, false);
        if (groupSelectionDlg.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        ContactGroupInfo[] selectedGroups = groupSelectionDlg.SelectedGroups;
        if (selectedGroups != null && selectedGroups.Length != 0)
        {
          foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
          {
            BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) selectedItem.Tag;
            BizPartnerInfo bizPartner = Session.ContactManager.GetBizPartner(tag.ContactID);
            if (bizPartner != null && bizPartner.AccessLevel == ContactAccess.Private && this.contactType == ContactType.PublicBiz)
            {
              bizPartner.AccessLevel = ContactAccess.Public;
              tag.AccessLevel = ContactAccess.Public;
              bizPartner.OwnerID = "";
              Session.ContactManager.UpdateBizPartner(bizPartner);
              if (this.gvContactList.SelectedItems.Count == 1)
                this.contactDetailTab.SynchronizeBizPartnerInfo(bizPartner);
            }
          }
          for (int index = 0; index < selectedGroups.Length; ++index)
          {
            selectedGroups[index].DeletedContactIds = this.getSelectedContactIDs(false, false);
            Session.ContactGroupManager.SaveContactGroup(selectedGroups[index]);
          }
        }
        this.RefreshList();
      }
    }

    public void RefreshHelpText()
    {
      IStatusDisplay service = Session.Application.GetService<IStatusDisplay>();
      int selectedItemsCount = this.getSelectedItemsCount();
      if (selectedItemsCount <= 0)
        service.DisplayHelpText("Press F1 for Help");
      else
        service.DisplayHelpText(selectedItemsCount.ToString() + " contacts selected");
    }

    private void SetContactListState(ContactListState state, int id, string name)
    {
      this.listState = state;
      this.idForState = id;
      this.nameForState = name;
      switch (state)
      {
        case ContactListState.Search:
          this.btnRemoveFromGroup.Enabled = false;
          break;
        case ContactListState.Group:
          if (name == "My Contacts" | name == "All Contacts")
          {
            this.btnRemoveFromGroup.Enabled = false;
            break;
          }
          this.btnRemoveFromGroup.Enabled = true;
          break;
        default:
          this.btnRemoveFromGroup.Enabled = false;
          break;
      }
    }

    private void btnEditGroups_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessEditGroupsUsageCounter", (SFxTag) new SFxUiTag());
      int num = (int) new ContactGroupSetupDlg(this.contactType).ShowDialog((IWin32Window) this);
      this.resetFieldDefs();
      this.gvFilterManager.RefreshFilterContent();
      this.RefreshList();
    }

    private void menuItemNewContact_Click(object sender, EventArgs e)
    {
      this.btnNew_Click((object) null, (EventArgs) null);
    }

    private void menuItemPrint_Click(object sender, EventArgs e)
    {
      this.btnPrint_Click((object) null, (EventArgs) null);
    }

    private void menuItemSelectAll_Click(object sender, EventArgs e)
    {
      this.gvContactList.Items.SelectAll();
    }

    private void menuItemMailMerge_Click(object sender, EventArgs e)
    {
      this.btnMailMerge_Click((object) null, (EventArgs) null);
    }

    private void menuItemAddToGroup_Click(object sender, EventArgs e)
    {
      this.btnAddToGroup_Click((object) null, (EventArgs) null);
    }

    private void menuItemRemoveFromGroup_Click(object sender, EventArgs e)
    {
      this.btnRemoveFromGroup_Click((object) null, (EventArgs) null);
    }

    private void navContacts_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      if (!this.suspendEvents)
        this.loadGVContactList(e.ItemIndex, e.ItemCount);
      this.currentPagingArgument = e;
    }

    private void loadGVContactList(int itemIndex, int itemCount)
    {
      this.Cursor = Cursors.WaitCursor;
      this.gvContactList.BeginUpdate();
      try
      {
        int contactId = 1 == this.gvContactList.SelectedItems.Count ? ((BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag).ContactID : 0;
        this.gvContactList.Items.Clear();
        if (-1 == itemIndex || itemCount == 0)
          return;
        object[] items = this.contactCursor.GetItems(itemIndex, itemCount);
        if (items.Length == 0)
          return;
        GVItem gvItem1 = (GVItem) null;
        foreach (object obj in items)
        {
          if (obj is BizPartnerSummaryInfo contactInfo)
          {
            GVItem gvItem2 = this.createGVItem(contactInfo);
            if (gvItem1 == null)
              gvItem1 = gvItem2;
            if (contactId == ((BizPartnerSummaryInfo) gvItem2.Tag).ContactID)
              gvItem1 = gvItem2;
            this.gvContactList.Items.Add(gvItem2);
          }
        }
        if (gvItem1 == null)
          return;
        gvItem1.Selected = true;
        this.gvContactList.EnsureVisible(gvItem1.Index);
      }
      finally
      {
        this.gvContactList.EndUpdate();
        this.Cursor = Cursors.Default;
      }
    }

    private GVItem createGVItem(BizPartnerSummaryInfo contactInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) contactInfo;
      for (int index = 0; index < this.gvContactList.Columns.Count; ++index)
      {
        string columnId = ((EllieMae.EMLite.ClientServer.TableLayout.Column) this.gvContactList.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefsWoHistory.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && contactInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) contactInfo, new EventHandler(this.LinkClickEvent));
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private void LinkClickEvent(object sender, EventArgs e)
    {
      if (this.currentContactId <= 0)
        return;
      this.Cursor = Cursors.WaitCursor;
      switch (sender)
      {
        case ContactEmailLink _:
          ContactEmailLink contactEmailLink = (ContactEmailLink) sender;
          this.contactDetailTab.ActivateNotesTab();
          int newNote1 = this.contactDetailTab.ContactNotesForm.CreateNewNote();
          SystemUtil.ShellExecute("mailto:" + contactEmailLink.EmailAddress);
          ContactUtils.addEmailHistory(new int[1]
          {
            this.currentContactId
          }, this.contactType, newNote1);
          break;
        case ContactPhoneLink _:
          ContactPhoneLink contactPhoneLink = (ContactPhoneLink) sender;
          this.contactDetailTab.ActivateNotesTab();
          int newNote2 = this.contactDetailTab.ContactNotesForm.CreateNewNote();
          if (contactPhoneLink.ContactPhoneType == PhoneImageLink.PhoneType.Fax)
          {
            ContactUtils.addFaxHistory(new int[1]
            {
              this.currentContactId
            }, this.contactType, newNote2);
            break;
          }
          ContactUtils.addCallHistory(new int[1]
          {
            this.currentContactId
          }, this.contactType, newNote2);
          break;
      }
      this.Cursor = Cursors.Default;
    }

    public void RefreshContactData() => this.RefreshList();

    public void RefreshContactList() => this.RefreshList();

    private void btnSave_Click(object sender, EventArgs e) => this.saveChanges(false);

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      if (this.contactDetailTab.isDirty() && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected contact? All changes to the contact will be lost.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        return;
      this.contactDetailTab.ClearChanges();
      this.contactDetailTab.CurrentContactID = this.currentContactId;
      this.btnSave.Enabled = false;
      this.btnRefresh.Enabled = false;
    }

    private void btnExport_Click(object sender, EventArgs e) => this.exportContactsToExcel(false);

    private bool exportContactsToExcel(bool exportAll)
    {
      if (this.gvContactList.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Your contacts cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if ((!exportAll ? this.gvContactList.SelectedItems.Count : this.contactCursor.GetItemCount()) > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Your selected contacts cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (exportAll)
        this.exportAllContactsToExcel();
      else
        this.exportSelectedRowsToExcel();
      return true;
    }

    private void exportSelectedRowsToExcel()
    {
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.gvContactList, (ReportFieldDefs) this.contactLoanFieldDefs, true);
      excelHandler.CreateExcel();
    }

    private void exportAllContactsToExcel()
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn c in this.gvContactList.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(c.Text, excelHandler.GetColumnFormat(c, (ReportFieldDefs) this.contactLoanFieldDefs));
        foreach (BizPartnerSummaryInfo allContact in this.getAllContacts())
        {
          GVItem gvItem = this.createGVItem(allContact);
          string[] data = new string[this.gvContactList.Columns.Count];
          for (int index = 0; index < this.gvContactList.Columns.Count; ++index)
            data[index] = gvItem.SubItems[this.gvContactList.Columns.DisplaySequence[index].Index].Text;
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
    }

    private BizPartnerSummaryInfo[] getAllContacts()
    {
      List<BizPartnerSummaryInfo> partnerSummaryInfoList = new List<BizPartnerSummaryInfo>();
      foreach (object obj in this.contactCursor.GetItems(0, this.contactCursor.GetItemCount()))
        partnerSummaryInfoList.Add((BizPartnerSummaryInfo) obj);
      return partnerSummaryInfoList.ToArray();
    }

    private void cboView_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.SaveChanges();
      this.SetCurrentView(((FileSystemEntryListItem) this.cboView.SelectedItem).Entry);
    }

    public void SetCurrentView(FileSystemEntry fsEntry)
    {
      try
      {
        if (fsEntry.Name == this.standardViewName)
        {
          ContactView defaultContactView = this.getDefaultContactView();
          this.fsViewEntry = fsEntry;
          this.setCurrentView(defaultContactView);
        }
        else
        {
          ContactView view = this.contactType != ContactType.BizPartner ? (ContactView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.PublicBizPartnerView, fsEntry) : (ContactView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.BizPartnerView, fsEntry);
          if (view == null)
            throw new ArgumentException();
          this.btnEditView.Enabled = !fsEntry.IsPublic;
          this.fsViewEntry = fsEntry;
          this.setCurrentView(view);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(BizPartnerListForm.sw, BizPartnerListForm.className, TraceLevel.Error, "Error opening view: " + (object) ex);
        ErrorDialog.Display(ex);
      }
    }

    private void setCurrentView(ContactView view)
    {
      this.currentView = view;
      this.suspendEvents = true;
      this.validateTableLayout(view.Layout);
      this.applyTableLayout(view.Layout);
      this.advFilter = view.Filter;
      this.gvFilterManager.ClearColumnFilters();
      this.setViewChanged(false);
      this.suspendEvents = false;
      this.SetCurrentFilter(view.Filter);
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
    }

    private void validateTableLayout(EllieMae.EMLite.ClientServer.TableLayout layout)
    {
      List<EllieMae.EMLite.ClientServer.TableLayout.Column> columnList = new List<EllieMae.EMLite.ClientServer.TableLayout.Column>();
      foreach (EllieMae.EMLite.ClientServer.TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) this.contactFieldDefsWoHistory.GetFieldByCriterionName(column.ColumnID);
        if (fieldByCriterionName != null)
          column.Title = fieldByCriterionName.Description;
        else
          columnList.Add(column);
      }
      foreach (EllieMae.EMLite.ClientServer.TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    private void setViewChanged(bool modified)
    {
      this.btnSaveView.Enabled = modified;
      this.btnRefreshView.Enabled = modified;
    }

    private void applyTableLayout(EllieMae.EMLite.ClientServer.TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvContactList);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefsWoHistory);
    }

    public void RefreshGVContactList()
    {
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvContactList, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.gvFilterManager != null)
        this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefsWoHistory);
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefsWoHistory);
      this.setViewChanged(true);
      this.RefreshList(true);
    }

    private EllieMae.EMLite.ClientServer.TableLayout getFullTableLayout()
    {
      EllieMae.EMLite.ClientServer.TableLayout fullTableLayout = new EllieMae.EMLite.ClientServer.TableLayout();
      foreach (BizPartnerReportFieldDef field in (ReportFieldDefContainer) this.allContactFieldDefs.ExtractFields(BizPartnerReportFieldFlags.IncludeBasicFields))
      {
        if (fullTableLayout.GetColumnByID(field.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(field));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void btnSaveView_Click(object sender, EventArgs e) => this.saveCurrentView();

    private void saveCurrentView()
    {
      ContactView contactView = new ContactView(this.currentView.Name);
      contactView.Layout = this.gvLayoutManager.GetCurrentLayout();
      contactView.Filter = this.GetCurrentFilter();
      using (SaveViewTemplateDialog viewTemplateDialog = new SaveViewTemplateDialog(this.templateType, (object) contactView, this.getViewNameList(), this.currentView.Name != this.standardViewName))
      {
        if (viewTemplateDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        if (!viewTemplateDialog.SelectedEntry.Equals((object) this.fsViewEntry))
          this.updateCurrentView(contactView, viewTemplateDialog.SelectedEntry);
      }
      this.currentView = contactView;
      this.btnRefreshView.Enabled = false;
      this.btnSaveView.Enabled = false;
    }

    private string[] getViewNameList()
    {
      List<string> stringList = new List<string>();
      foreach (object obj in this.cboView.Items)
        stringList.Add(obj.ToString());
      return stringList.ToArray();
    }

    private void updateCurrentView(ContactView view, FileSystemEntry e)
    {
      this.suspendEvents = true;
      this.currentView = view;
      ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(e), true);
      this.suspendEvents = false;
    }

    private void btnRefreshView_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected view?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.setCurrentView(this.currentView);
    }

    private void btnEditView_Click(object sender, EventArgs e)
    {
      EllieMae.EMLite.ClientServer.TemplateSettingsType templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.BizPartnerView;
      if (this.contactType == ContactType.PublicBiz)
        templateType = EllieMae.EMLite.ClientServer.TemplateSettingsType.PublicBizPartnerView;
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(templateType, false, "BizPartnerContact.DefaultView"))
      {
        managementDialog.AddStaticView((BinaryConvertibleObject) this.getDefaultContactView());
        int num = (int) managementDialog.ShowDialog((IWin32Window) this);
      }
      this.RefreshViews();
    }

    private void deleteCurrentView()
    {
      if (this.currentView.Name == this.standardViewName)
        throw new Exception("Standard view can not be deleted");
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to permanently delete this Contact View?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      try
      {
        Session.ConfigurationManager.DeleteTemplateSettingsObject(this.templateType, this.fsViewEntry);
      }
      catch (Exception ex)
      {
        Tracing.Log(BizPartnerListForm.sw, BizPartnerListForm.className, TraceLevel.Error, "Error deleting view: " + (object) ex);
      }
      this.RefreshViews();
    }

    public void RefreshViews()
    {
      this.loadViewList(Session.ConfigurationManager.GetTemplateDirEntries(this.templateType, FileSystemEntry.PrivateRoot(Session.UserID)));
      if (this.cboView.Items.Count <= 0 || this.cboView.SelectedIndex >= 0)
        return;
      this.cboView.SelectedIndex = 0;
    }

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      if (this.contactLoanFieldDefs == null)
        this.contactLoanFieldDefs = BizPartnerLoanReportFieldDefs.GetFieldDefs(this.contactType);
      using (ContactAdvSearchDialog contactAdvSearchDialog = new ContactAdvSearchDialog((ReportFieldDefs) this.contactLoanFieldDefs, fieldFilterList))
      {
        if (contactAdvSearchDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        this.SetCurrentFilter(contactAdvSearchDialog.GetSelectedFilter());
      }
    }

    public void SetCurrentFilter(FieldFilterList filter)
    {
      this.advFilter = filter;
      this.gvFilterManager.ClearColumnFilters();
      this.refreshFilterDescription();
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    private void btnSync_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBusinessSynchronizeUsageCounter", (SFxTag) new SFxUiTag());
      this.parentForm.ContactMenu_Click("Synchronize");
    }

    private void gvContactList_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
      {
        this.retrieveContactData((QueryCriterion) null, RelatedLoanMatchType.None, this.getSortFieldsForColumnSort(e.ColumnSorts));
        this.displayCurrentPage(true);
        this.setViewChanged(true);
      }
    }

    private void displayCurrentPage(bool preserveSelections)
    {
      int currentPageItemIndex = this.navContacts.CurrentPageItemIndex;
      int currentPageItemCount = this.navContacts.CurrentPageItemCount;
      BizPartnerSummaryInfo[] partnerSummaryInfoArray = new BizPartnerSummaryInfo[0];
      if (currentPageItemCount > 0)
        partnerSummaryInfoArray = (BizPartnerSummaryInfo[]) this.contactCursor.GetItems(currentPageItemIndex, currentPageItemCount);
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
        {
          if (gvItem.Selected)
            dictionary[((BizPartnerSummaryInfo) gvItem.Tag).ContactID] = true;
        }
      }
      this.gvContactList.Items.Clear();
      for (int index = 0; index < partnerSummaryInfoArray.Length; ++index)
      {
        GVItem gvItem = this.createGVItem(partnerSummaryInfoArray[index]);
        this.gvContactList.Items.Add(gvItem);
        if (dictionary.ContainsKey(partnerSummaryInfoArray[index].ContactID))
          gvItem.Selected = true;
      }
      if (this.gvContactList.Items.Count <= 0 || this.gvContactList.SelectedItems.Count != 0)
        return;
      this.gvContactList.Items[0].Selected = true;
    }

    private SortField[] getSortFieldsForColumnSort(GVColumnSort[] sortOrder)
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumnSort gvColumnSort in sortOrder)
      {
        SortField sortFieldForColumn = this.getSortFieldForColumn((EllieMae.EMLite.ClientServer.TableLayout.Column) this.gvContactList.Columns[gvColumnSort.Column].Tag, gvColumnSort.SortOrder);
        if (sortFieldForColumn != null)
          sortFieldList.Add(sortFieldForColumn);
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(EllieMae.EMLite.ClientServer.TableLayout.Column colInfo, SortOrder sortOrder)
    {
      BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefsWoHistory.GetFieldByCriterionName(colInfo.ColumnID);
      return fieldByCriterionName != null ? new SortField(fieldByCriterionName.SortTerm, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion) : (SortField) null;
    }

    private void siBtnRefresh_Click(object sender, EventArgs e)
    {
      this.gvFilterManager.RefreshFilterContent();
      this.RefreshList(true);
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      this.menuItemNewContact.Visible = this.btnNew.Visible;
      this.menuItemNewContact.Enabled = this.btnNew.Enabled;
      this.menuItemCopy.Visible = this.btnDuplicate.Visible;
      this.menuItemCopy.Enabled = this.btnDuplicate.Enabled;
      this.menuItemDelete.Visible = this.btnDelete.Visible;
      this.menuItemDelete.Enabled = this.btnDelete.Enabled;
      this.menuItemPrint.Visible = this.btnPrint.Visible;
      if (this.gvContactList.Items.Count > 0)
        this.menuItemPrintAll.Enabled = true;
      else
        this.menuItemPrintAll.Enabled = false;
      if (this.gvContactList.SelectedItems.Count > 0)
        this.menuItemPrintSelected.Enabled = true;
      else
        this.menuItemPrintSelected.Enabled = false;
      this.menuItemMailMerge.Visible = this.btnMailMerge.Visible;
      if (this.gvContactList.Items.Count > 0)
        this.menuItemMailMergeAll.Enabled = true;
      else
        this.menuItemMailMergeAll.Enabled = false;
      if (this.gvContactList.SelectedItems.Count > 0)
        this.menuItemMailMergeSelected.Enabled = true;
      else
        this.menuItemMailMergeSelected.Enabled = false;
      this.menuItemAddToGroup.Visible = this.btnAddToGroup.Visible;
      this.menuItemAddToGroup.Enabled = this.btnAddToGroup.Enabled;
      this.menuItemRemoveFromGroup.Visible = this.btnRemoveFromGroup.Visible;
      this.menuItemRemoveFromGroup.Enabled = this.btnRemoveFromGroup.Enabled;
      if (!this.btnNew.Visible && !this.btnPrint.Visible)
        this.toolStripSeparator1.Visible = false;
      else
        this.toolStripSeparator1.Visible = true;
      if (!this.btnDuplicate.Visible && !this.btnDelete.Visible)
        this.toolStripSeparator2.Visible = false;
      else
        this.toolStripSeparator2.Visible = true;
    }

    private void gvContactList_ColumnReorder(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void gvContactList_ColumnResize(object source, GVColumnEventArgs e)
    {
      this.setViewChanged(true);
    }

    private void BizPartnerListForm_SizeChanged(object sender, EventArgs e)
    {
      IntPtr handle = this.Handle;
      this.BeginInvoke((Delegate) new MethodInvoker(this.ChangeSize));
    }

    protected void ChangeSize()
    {
      this.SuspendLayout();
      foreach (Control control in (ArrangedElementCollection) this.Controls)
        control.Size = new Size(control.Width - 1, control.Height - 1);
      this.ResumeLayout();
    }

    private delegate void ContactMethod(int contactId);
  }
}
