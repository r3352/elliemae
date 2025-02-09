// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.BorrowerListForm
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
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.ContactUI.Leads;
using EllieMae.EMLite.ContactUI.LoanServices;
using EllieMae.EMLite.ContactUI.WebServices;
using EllieMae.EMLite.ePass.Messaging;
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
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class BorrowerListForm : Form, IBindingForm, IBorrowerContacts
  {
    private Sessions.Session session;
    private string standardViewName = "Standard View";
    private FileSystemEntry fsViewEntry;
    private bool suspendEvents;
    private bool suspendRefresh;
    private static string className = nameof (BorrowerListForm);
    private static string sw = Tracing.SwContact;
    private BorrowerInfo currentContact;
    private BorrowerTabForm contactDetailTab;
    private int currentContactId = -2;
    private Hashtable userNameCache = new Hashtable();
    private Hashtable idMap = new Hashtable();
    private ContactListState listState = ContactListState.Group;
    private int idForState = -1;
    private string nameForState = string.Empty;
    private bool mailMergeDocMgmtOnly;
    private FieldFilterList advFilter;
    private BorrowerGroupListController groupListController;
    private Button btnMailMerge;
    private Panel panelBottom;
    private Panel panel1;
    private Button btnAddToGroup;
    private Button btnRemoveFromGroup;
    private IContainer components;
    private ContactMainForm parentForm;
    private GradientPanel gradientPanel1;
    private Label label1;
    private ComboBoxEx cboView;
    private StandardIconButton btnRefreshView;
    private StandardIconButton btnEditView;
    private StandardIconButton btnSaveView;
    private Button btnAdvSearch;
    private Label label4;
    private GroupContainer groupContainer1;
    private StandardIconButton btnPrint;
    private StandardIconButton btnExport;
    private StandardIconButton btnDelete;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnNew;
    private StandardIconButton btnRefresh;
    private StandardIconButton btnSave;
    private Button btnOrderCredit;
    private Button btnOriginateLoan;
    private Button btnProductPricing;
    private GridView gvContactList;
    private FeaturesAclManager aclMgr;
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private TableLayout borrowerTableLayout;
    private ICursor contactCursor;
    private PageListNavigator navContacts;
    private BorrowerReportFieldDefs contactFieldDefsWOHistory;
    private BorrowerLoanReportFieldDefs contactLoanReportFields;
    private BorrowerReportFieldDefs allContactFieldDefs;
    private ContextMenuStrip gvMenuStrip;
    private ToolStripMenuItem miNewContact;
    private ToolStripMenuItem miDuplicateContact;
    private ToolStripMenuItem miDeleteContact;
    private ToolStripMenuItem miExport;
    private ToolStripMenuItem miPrint;
    private ToolStripMenuItem miMailMerge;
    private ToolStripMenuItem miAddToGroup;
    private ToolStripMenuItem miRemoveFromGroup;
    private ToolStripMenuItem miOriginateLoan;
    private ToolStripMenuItem miOrderCredit;
    private ToolStripMenuItem miProductAndPricing;
    private ToolStripMenuItem miNewAppointment;
    private ToolStripMenuItem miReassign;
    private ToolStripMenuItem miSelectAll;
    private PageChangedEventArgs currentPagingArgument;
    private FlowLayoutPanel flowLayoutPanel1;
    private Button btnBuyLead;
    private Button btnSync;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panel2;
    private VerticalSeparator vs1;
    private VerticalSeparator vs2;
    private VerticalSeparator verticalSeparator3;
    private FlowLayoutPanel flowLayoutPanel2;
    private VerticalSeparator vs3;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripSeparator toolStripSeparator2;
    private ToolStripSeparator toolStripSeparator3;
    private ToolStripSeparator toolStripSeparator4;
    private ToolTip toolTip1;
    private Button btnClearSearch;
    private GradientPanel gradientPanel4;
    private GroupContainer groupContainer2;
    private Button btnEditGroup;
    private Label lblFilter;
    private StandardIconButton siBtnRefresh;
    private ContactView currentView;
    private Button btnImportLeads;
    private ImageList imageList1;
    private ToolStripMenuItem miExportSelectedOnly;
    private ToolStripMenuItem miExportAll;
    private ToolStripMenuItem miPrintSelectedOnly;
    private ToolStripMenuItem miPrintAll;
    private ToolStripMenuItem miMailMergeSelectedOnly;
    private ToolStripMenuItem miMailMergeAll;
    private Dictionary<AclFeature, bool> personaSettings = new Dictionary<AclFeature, bool>();

    public BorrowerListForm(Sessions.Session session, ContactMainForm parentForm)
    {
      this.session = session;
      this.InitializeComponent();
      this.session.Application.RegisterService((object) this, typeof (IBorrowerContacts));
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.parentForm = parentForm;
      this.gatherPersonaSettings();
      this.init();
    }

    public bool isDirty() => false;

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

    private void btnClearSearch_Click(object sender, EventArgs e)
    {
      this.SetCurrentFilter((FieldFilterList) null);
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
        if (filter == null)
          this.contactCursor = this.session.ContactManager.OpenBorrowerCursor(new QueryCriterion[0], matchType, sortFields, fieldList, true);
        else
          this.contactCursor = this.session.ContactManager.OpenBorrowerCursor(new QueryCriterion[1]
          {
            filter
          }, matchType, sortFields, fieldList, true);
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
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        BorrowerReportFieldDef fieldByCriterionName = this.contactFieldDefsWOHistory.GetFieldByCriterionName(column.ColumnID);
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
          TableLayout.Column tag = (TableLayout.Column) column.Tag;
          sortFieldList.Add(new SortField(tag.ColumnID, SortOrder.Ascending == column.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending));
        }
      }
      return sortFieldList.ToArray();
    }

    private ContactView getDefaultContactView()
    {
      ContactView defaultContactView = new ContactView(this.standardViewName);
      TableLayout tableLayout = new TableLayout();
      tableLayout.AddColumn(new TableLayout.Column("ContactGroupCount.GroupCount", "Groups", HorizontalAlignment.Left, 70));
      tableLayout.AddColumn(new TableLayout.Column("ContactOwner.UserName", "Contact Owner", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("Contact.FirstName", "Borrower First Name", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column("Contact.LastName", "Borrower Last Name", HorizontalAlignment.Left, 110));
      tableLayout.AddColumn(new TableLayout.Column("Contact.HomePhone", "Home Phone", HorizontalAlignment.Left, 90));
      tableLayout.AddColumn(new TableLayout.Column("Contact.MobilePhone", "Cell Phone", HorizontalAlignment.Left, 90));
      tableLayout.AddColumn(new TableLayout.Column("Contact.PersonalEmail", "Home Email", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("Contact.ContactType", "Contact Type", HorizontalAlignment.Left, 91));
      tableLayout.AddColumn(new TableLayout.Column("Contact.Status", "Status", HorizontalAlignment.Left, 100));
      tableLayout.AddColumn(new TableLayout.Column("NextAppointment.StartDateTime", "Next Appointment Date/Time", HorizontalAlignment.Left, 150));
      tableLayout.AddColumn(new TableLayout.Column("Contact.LastModified", "Last Modification", HorizontalAlignment.Left, 90));
      defaultContactView.Layout = tableLayout;
      return defaultContactView;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (BorrowerListForm));
      this.panel1 = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnEditGroup = new Button();
      this.btnRemoveFromGroup = new Button();
      this.btnAddToGroup = new Button();
      this.vs1 = new VerticalSeparator();
      this.btnSync = new Button();
      this.btnMailMerge = new Button();
      this.vs2 = new VerticalSeparator();
      this.btnImportLeads = new Button();
      this.btnBuyLead = new Button();
      this.verticalSeparator3 = new VerticalSeparator();
      this.btnPrint = new StandardIconButton();
      this.btnExport = new StandardIconButton();
      this.siBtnRefresh = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.navContacts = new PageListNavigator();
      this.gvContactList = new GridView();
      this.gvMenuStrip = new ContextMenuStrip(this.components);
      this.miNewContact = new ToolStripMenuItem();
      this.miDuplicateContact = new ToolStripMenuItem();
      this.miDeleteContact = new ToolStripMenuItem();
      this.toolStripSeparator1 = new ToolStripSeparator();
      this.miExport = new ToolStripMenuItem();
      this.miExportSelectedOnly = new ToolStripMenuItem();
      this.miExportAll = new ToolStripMenuItem();
      this.miPrint = new ToolStripMenuItem();
      this.miPrintSelectedOnly = new ToolStripMenuItem();
      this.miPrintAll = new ToolStripMenuItem();
      this.miMailMerge = new ToolStripMenuItem();
      this.miMailMergeSelectedOnly = new ToolStripMenuItem();
      this.miMailMergeAll = new ToolStripMenuItem();
      this.toolStripSeparator2 = new ToolStripSeparator();
      this.miAddToGroup = new ToolStripMenuItem();
      this.miRemoveFromGroup = new ToolStripMenuItem();
      this.toolStripSeparator3 = new ToolStripSeparator();
      this.miOriginateLoan = new ToolStripMenuItem();
      this.miOrderCredit = new ToolStripMenuItem();
      this.miProductAndPricing = new ToolStripMenuItem();
      this.miNewAppointment = new ToolStripMenuItem();
      this.miReassign = new ToolStripMenuItem();
      this.toolStripSeparator4 = new ToolStripSeparator();
      this.miSelectAll = new ToolStripMenuItem();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panel2 = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnProductPricing = new Button();
      this.btnOrderCredit = new Button();
      this.btnOriginateLoan = new Button();
      this.vs3 = new VerticalSeparator();
      this.btnRefresh = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.panelBottom = new Panel();
      this.gradientPanel4 = new GradientPanel();
      this.lblFilter = new Label();
      this.btnClearSearch = new Button();
      this.label4 = new Label();
      this.btnAdvSearch = new Button();
      this.gradientPanel1 = new GradientPanel();
      this.btnRefreshView = new StandardIconButton();
      this.btnEditView = new StandardIconButton();
      this.btnSaveView = new StandardIconButton();
      this.cboView = new ComboBoxEx();
      this.label1 = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.imageList1 = new ImageList(this.components);
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnPrint).BeginInit();
      ((ISupportInitialize) this.btnExport).BeginInit();
      ((ISupportInitialize) this.siBtnRefresh).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.gvMenuStrip.SuspendLayout();
      this.panel2.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      ((ISupportInitialize) this.btnRefresh).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.gradientPanel4.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      ((ISupportInitialize) this.btnRefreshView).BeginInit();
      ((ISupportInitialize) this.btnEditView).BeginInit();
      ((ISupportInitialize) this.btnSaveView).BeginInit();
      this.SuspendLayout();
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Controls.Add((Control) this.gradientPanel4);
      this.panel1.Controls.Add((Control) this.gradientPanel1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(1050, 505);
      this.panel1.TabIndex = 10;
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.navContacts);
      this.groupContainer1.Controls.Add((Control) this.gvContactList);
      this.groupContainer1.Controls.Add((Control) this.collapsibleSplitter1);
      this.groupContainer1.Controls.Add((Control) this.panel2);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 62);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(1050, 443);
      this.groupContainer1.TabIndex = 2;
      this.groupContainer1.Paint += new PaintEventHandler(this.groupContainer1_Paint);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnEditGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnRemoveFromGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddToGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.vs1);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSync);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnMailMerge);
      this.flowLayoutPanel1.Controls.Add((Control) this.vs2);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnImportLeads);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnBuyLead);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator3);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnPrint);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnExport);
      this.flowLayoutPanel1.Controls.Add((Control) this.siBtnRefresh);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDuplicate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(265, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(784, 22);
      this.flowLayoutPanel1.TabIndex = 2;
      this.btnEditGroup.BackColor = SystemColors.Control;
      this.btnEditGroup.Location = new Point(704, 0);
      this.btnEditGroup.Margin = new Padding(0, 0, 5, 0);
      this.btnEditGroup.Name = "btnEditGroup";
      this.btnEditGroup.Padding = new Padding(2, 0, 0, 0);
      this.btnEditGroup.Size = new Size(75, 22);
      this.btnEditGroup.TabIndex = 7;
      this.btnEditGroup.Text = "Edit Groups";
      this.btnEditGroup.UseVisualStyleBackColor = true;
      this.btnEditGroup.Click += new EventHandler(this.btnEditGroups_Click);
      this.btnRemoveFromGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRemoveFromGroup.BackColor = SystemColors.Control;
      this.btnRemoveFromGroup.Enabled = false;
      this.btnRemoveFromGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnRemoveFromGroup.Location = new Point(588, 0);
      this.btnRemoveFromGroup.Margin = new Padding(0);
      this.btnRemoveFromGroup.Name = "btnRemoveFromGroup";
      this.btnRemoveFromGroup.Padding = new Padding(2, 0, 0, 0);
      this.btnRemoveFromGroup.Size = new Size(116, 22);
      this.btnRemoveFromGroup.TabIndex = 6;
      this.btnRemoveFromGroup.Text = "Remove from Group";
      this.btnRemoveFromGroup.UseVisualStyleBackColor = true;
      this.btnRemoveFromGroup.Click += new EventHandler(this.btnRemoveFromGroup_Click);
      this.btnAddToGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddToGroup.BackColor = SystemColors.Control;
      this.btnAddToGroup.Enabled = false;
      this.btnAddToGroup.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAddToGroup.Location = new Point(504, 0);
      this.btnAddToGroup.Margin = new Padding(0);
      this.btnAddToGroup.Name = "btnAddToGroup";
      this.btnAddToGroup.Size = new Size(84, 22);
      this.btnAddToGroup.TabIndex = 5;
      this.btnAddToGroup.Text = "Add to Group";
      this.btnAddToGroup.UseVisualStyleBackColor = true;
      this.btnAddToGroup.Click += new EventHandler(this.btnAddToGroup_Click);
      this.vs1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.vs1.Location = new Point(499, 3);
      this.vs1.MaximumSize = new Size(2, 16);
      this.vs1.MinimumSize = new Size(2, 16);
      this.vs1.Name = "vs1";
      this.vs1.Size = new Size(2, 16);
      this.vs1.TabIndex = 26;
      this.vs1.Text = "verticalSeparator1";
      this.btnSync.BackColor = SystemColors.Control;
      this.btnSync.Location = new Point(414, 0);
      this.btnSync.Margin = new Padding(0);
      this.btnSync.Name = "btnSync";
      this.btnSync.Padding = new Padding(2, 0, 0, 0);
      this.btnSync.Size = new Size(82, 22);
      this.btnSync.TabIndex = 4;
      this.btnSync.Text = "Synchronize";
      this.btnSync.UseVisualStyleBackColor = true;
      this.btnSync.Click += new EventHandler(this.btnSync_Click);
      this.btnMailMerge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnMailMerge.BackColor = SystemColors.Control;
      this.btnMailMerge.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnMailMerge.Location = new Point(319, 0);
      this.btnMailMerge.Margin = new Padding(0);
      this.btnMailMerge.Name = "btnMailMerge";
      this.btnMailMerge.Padding = new Padding(2, 0, 0, 0);
      this.btnMailMerge.Size = new Size(95, 22);
      this.btnMailMerge.TabIndex = 3;
      this.btnMailMerge.Text = "Mail/Email Mer&ge";
      this.btnMailMerge.UseVisualStyleBackColor = true;
      this.btnMailMerge.Click += new EventHandler(this.btnMailMerge_Click);
      this.vs2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.vs2.Location = new Point(314, 3);
      this.vs2.MaximumSize = new Size(2, 16);
      this.vs2.MinimumSize = new Size(2, 16);
      this.vs2.Name = "vs2";
      this.vs2.Size = new Size(2, 16);
      this.vs2.TabIndex = 27;
      this.vs2.Text = "verticalSeparator2";
      this.btnImportLeads.BackColor = SystemColors.Control;
      this.btnImportLeads.ImageAlign = ContentAlignment.MiddleLeft;
      this.btnImportLeads.Location = new Point(211, 0);
      this.btnImportLeads.Margin = new Padding(0);
      this.btnImportLeads.Name = "btnImportLeads";
      this.btnImportLeads.Size = new Size(100, 22);
      this.btnImportLeads.TabIndex = 2;
      this.btnImportLeads.Text = "Import Leads";
      this.btnImportLeads.UseVisualStyleBackColor = true;
      this.btnImportLeads.Click += new EventHandler(this.btnImportLeads_Click);
      this.btnBuyLead.BackColor = SystemColors.Control;
      this.btnBuyLead.Location = new Point(136, 0);
      this.btnBuyLead.Margin = new Padding(0);
      this.btnBuyLead.Name = "btnBuyLead";
      this.btnBuyLead.Padding = new Padding(2, 0, 0, 0);
      this.btnBuyLead.Size = new Size(75, 22);
      this.btnBuyLead.TabIndex = 1;
      this.btnBuyLead.Text = "Buy Leads";
      this.btnBuyLead.UseVisualStyleBackColor = true;
      this.btnBuyLead.Click += new EventHandler(this.btnBuyLead_Click);
      this.verticalSeparator3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator3.Location = new Point(131, 3);
      this.verticalSeparator3.MaximumSize = new Size(2, 16);
      this.verticalSeparator3.MinimumSize = new Size(2, 16);
      this.verticalSeparator3.Name = "verticalSeparator3";
      this.verticalSeparator3.Size = new Size(2, 16);
      this.verticalSeparator3.TabIndex = 28;
      this.verticalSeparator3.Text = "verticalSeparator3";
      this.btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPrint.BackColor = Color.Transparent;
      this.btnPrint.Location = new Point(109, 3);
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
      this.btnExport.Location = new Point(87, 3);
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
      this.siBtnRefresh.Location = new Point(66, 3);
      this.siBtnRefresh.Margin = new Padding(2, 3, 3, 3);
      this.siBtnRefresh.MouseDownImage = (Image) null;
      this.siBtnRefresh.Name = "siBtnRefresh";
      this.siBtnRefresh.Size = new Size(16, 16);
      this.siBtnRefresh.StandardButtonType = StandardIconButton.ButtonType.RefreshButton;
      this.siBtnRefresh.TabIndex = 30;
      this.siBtnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnRefresh, "Refresh");
      this.siBtnRefresh.Click += new EventHandler(this.siBtnRefresh_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Location = new Point(45, 3);
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
      this.btnDuplicate.Location = new Point(24, 3);
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
      this.btnNew.Location = new Point(3, 3);
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
      this.navContacts.Size = new Size(254, 22);
      this.navContacts.TabIndex = 1;
      this.navContacts.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContacts_PageChangedEvent);
      this.gvContactList.AllowColumnReorder = true;
      this.gvContactList.BorderStyle = BorderStyle.None;
      this.gvContactList.ContextMenuStrip = this.gvMenuStrip;
      this.gvContactList.Dock = DockStyle.Fill;
      this.gvContactList.FilterVisible = true;
      this.gvContactList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvContactList.Location = new Point(1, 26);
      this.gvContactList.Name = "gvContactList";
      this.gvContactList.Size = new Size(1048, 73);
      this.gvContactList.SortOption = GVSortOption.Owner;
      this.gvContactList.TabIndex = 3;
      this.gvContactList.SelectedIndexChanged += new EventHandler(this.lvwContacts_SelectedIndexChanged);
      this.gvContactList.ColumnReorder += new GVColumnEventHandler(this.gvContactList_ColumnReorder);
      this.gvContactList.ColumnResize += new GVColumnEventHandler(this.gvContactList_ColumnResize);
      this.gvContactList.SortItems += new GVColumnSortEventHandler(this.gvContactList_SortItems);
      this.gvContactList.DoubleClick += new EventHandler(this.gvContacts_DoubleClick);
      this.gvMenuStrip.Items.AddRange(new ToolStripItem[18]
      {
        (ToolStripItem) this.miNewContact,
        (ToolStripItem) this.miDuplicateContact,
        (ToolStripItem) this.miDeleteContact,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.miExport,
        (ToolStripItem) this.miPrint,
        (ToolStripItem) this.miMailMerge,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.miAddToGroup,
        (ToolStripItem) this.miRemoveFromGroup,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.miOriginateLoan,
        (ToolStripItem) this.miOrderCredit,
        (ToolStripItem) this.miProductAndPricing,
        (ToolStripItem) this.miNewAppointment,
        (ToolStripItem) this.miReassign,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.miSelectAll
      });
      this.gvMenuStrip.Name = "gvMenuStrip";
      this.gvMenuStrip.ShowImageMargin = false;
      this.gvMenuStrip.Size = new Size(168, 336);
      this.gvMenuStrip.Opening += new CancelEventHandler(this.gvMenuStrip_Opening);
      this.miNewContact.Name = "miNewContact";
      this.miNewContact.Size = new Size(167, 22);
      this.miNewContact.Text = "New Contact";
      this.miNewContact.Click += new EventHandler(this.menuItemNewContact_Click);
      this.miDuplicateContact.Name = "miDuplicateContact";
      this.miDuplicateContact.Size = new Size(167, 22);
      this.miDuplicateContact.Text = "Duplicate Contact";
      this.miDuplicateContact.Click += new EventHandler(this.btnCopy_Click);
      this.miDeleteContact.Name = "miDeleteContact";
      this.miDeleteContact.Size = new Size(167, 22);
      this.miDeleteContact.Text = "Delete Contact";
      this.miDeleteContact.Click += new EventHandler(this.btnDelete_Click);
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new Size(164, 6);
      this.miExport.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.miExportSelectedOnly,
        (ToolStripItem) this.miExportAll
      });
      this.miExport.Name = "miExport";
      this.miExport.Size = new Size(167, 22);
      this.miExport.Text = "Export to Excel...";
      this.miExportSelectedOnly.Name = "miExportSelectedOnly";
      this.miExportSelectedOnly.Size = new Size(215, 22);
      this.miExportSelectedOnly.Text = "Selected Contacts Only...";
      this.miExportSelectedOnly.Click += new EventHandler(this.btnExport_Click);
      this.miExportAll.Name = "miExportAll";
      this.miExportAll.Size = new Size(215, 22);
      this.miExportAll.Text = "All Contacts on All Pages...";
      this.miExportAll.Click += new EventHandler(this.btnExportAll_Click);
      this.miPrint.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.miPrintSelectedOnly,
        (ToolStripItem) this.miPrintAll
      });
      this.miPrint.Name = "miPrint";
      this.miPrint.Size = new Size(167, 22);
      this.miPrint.Text = "Print Details...";
      this.miPrintSelectedOnly.Name = "miPrintSelectedOnly";
      this.miPrintSelectedOnly.Size = new Size(215, 22);
      this.miPrintSelectedOnly.Text = "Selected Contacts Only...";
      this.miPrintSelectedOnly.Click += new EventHandler(this.btnPrint_Click);
      this.miPrintAll.Name = "miPrintAll";
      this.miPrintAll.Size = new Size(215, 22);
      this.miPrintAll.Text = "All Contacts on All Pages...";
      this.miPrintAll.Click += new EventHandler(this.btnPrintAll_Click);
      this.miMailMerge.DropDownItems.AddRange(new ToolStripItem[2]
      {
        (ToolStripItem) this.miMailMergeSelectedOnly,
        (ToolStripItem) this.miMailMergeAll
      });
      this.miMailMerge.Name = "miMailMerge";
      this.miMailMerge.Size = new Size(167, 22);
      this.miMailMerge.Text = "Mail Merge...";
      this.miMailMergeSelectedOnly.Name = "miMailMergeSelectedOnly";
      this.miMailMergeSelectedOnly.Size = new Size(215, 22);
      this.miMailMergeSelectedOnly.Text = "Selected Contacts Only...";
      this.miMailMergeSelectedOnly.Click += new EventHandler(this.btnMailMerge_Click);
      this.miMailMergeAll.Name = "miMailMergeAll";
      this.miMailMergeAll.Size = new Size(215, 22);
      this.miMailMergeAll.Text = "All Contacts on All Pages...";
      this.miMailMergeAll.Click += new EventHandler(this.btnMailMergeAll_Click);
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new Size(164, 6);
      this.miAddToGroup.Name = "miAddToGroup";
      this.miAddToGroup.Size = new Size(167, 22);
      this.miAddToGroup.Text = "Add to Group";
      this.miAddToGroup.Click += new EventHandler(this.menuItemAddToGroup_Click);
      this.miRemoveFromGroup.Name = "miRemoveFromGroup";
      this.miRemoveFromGroup.Size = new Size(167, 22);
      this.miRemoveFromGroup.Text = "Remove From Group";
      this.miRemoveFromGroup.Click += new EventHandler(this.menuItemRemoveFromGroup_Click);
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new Size(164, 6);
      this.miOriginateLoan.Name = "miOriginateLoan";
      this.miOriginateLoan.Size = new Size(167, 22);
      this.miOriginateLoan.Text = "Originate Loan";
      this.miOriginateLoan.Click += new EventHandler(this.menuItemOriginateLoan_Click);
      this.miOrderCredit.Name = "miOrderCredit";
      this.miOrderCredit.Size = new Size(167, 22);
      this.miOrderCredit.Text = "Order Credit...";
      this.miOrderCredit.Click += new EventHandler(this.menuItemOrderCredit_Click);
      this.miProductAndPricing.Name = "miProductAndPricing";
      this.miProductAndPricing.Size = new Size(167, 22);
      this.miProductAndPricing.Text = "Product and Pricing";
      this.miProductAndPricing.Click += new EventHandler(this.menuItemProductPricing_Click);
      this.miNewAppointment.Name = "miNewAppointment";
      this.miNewAppointment.Size = new Size(167, 22);
      this.miNewAppointment.Text = "New Appointment";
      this.miNewAppointment.Click += new EventHandler(this.miNewAppointment_Click);
      this.miReassign.Name = "miReassign";
      this.miReassign.Size = new Size(167, 22);
      this.miReassign.Text = "Reassign...";
      this.miReassign.Click += new EventHandler(this.btnReassign_Click);
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new Size(164, 6);
      this.miSelectAll.Name = "miSelectAll";
      this.miSelectAll.Size = new Size(167, 22);
      this.miSelectAll.Text = "Select All on This Page";
      this.miSelectAll.Click += new EventHandler(this.menuItemSelectAll_Click);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.panel2;
      this.collapsibleSplitter1.Cursor = Cursors.HSplit;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 99);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 27;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panel2.Controls.Add((Control) this.groupContainer2);
      this.panel2.Dock = DockStyle.Bottom;
      this.panel2.Location = new Point(1, 106);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(1048, 336);
      this.panel2.TabIndex = 28;
      this.groupContainer2.Borders = AnchorStyles.Top;
      this.groupContainer2.Controls.Add((Control) this.flowLayoutPanel2);
      this.groupContainer2.Controls.Add((Control) this.panelBottom);
      this.groupContainer2.Dock = DockStyle.Fill;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(1048, 336);
      this.groupContainer2.TabIndex = 4;
      this.groupContainer2.Text = "Contact Details";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnProductPricing);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnOrderCredit);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnOriginateLoan);
      this.flowLayoutPanel2.Controls.Add((Control) this.vs3);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnRefresh);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(526, 3);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(520, 22);
      this.flowLayoutPanel2.TabIndex = 0;
      this.btnProductPricing.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnProductPricing.BackColor = SystemColors.Control;
      this.btnProductPricing.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnProductPricing.Location = new Point(404, 0);
      this.btnProductPricing.Margin = new Padding(0, 0, 5, 0);
      this.btnProductPricing.Name = "btnProductPricing";
      this.btnProductPricing.Padding = new Padding(2, 0, 0, 0);
      this.btnProductPricing.Size = new Size(111, 22);
      this.btnProductPricing.TabIndex = 3;
      this.btnProductPricing.Text = "Product and Pricing";
      this.btnProductPricing.UseVisualStyleBackColor = true;
      this.btnProductPricing.Click += new EventHandler(this.menuItemProductPricing_Click);
      this.btnOrderCredit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOrderCredit.BackColor = SystemColors.Control;
      this.btnOrderCredit.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOrderCredit.Location = new Point(322, 0);
      this.btnOrderCredit.Margin = new Padding(0);
      this.btnOrderCredit.Name = "btnOrderCredit";
      this.btnOrderCredit.Padding = new Padding(2, 0, 0, 0);
      this.btnOrderCredit.Size = new Size(82, 22);
      this.btnOrderCredit.TabIndex = 2;
      this.btnOrderCredit.Text = "Order Credit";
      this.btnOrderCredit.UseVisualStyleBackColor = true;
      this.btnOrderCredit.Click += new EventHandler(this.menuItemOrderCredit_Click);
      this.btnOriginateLoan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOriginateLoan.BackColor = SystemColors.Control;
      this.btnOriginateLoan.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnOriginateLoan.Location = new Point(231, 0);
      this.btnOriginateLoan.Margin = new Padding(0);
      this.btnOriginateLoan.Name = "btnOriginateLoan";
      this.btnOriginateLoan.Padding = new Padding(2, 0, 0, 0);
      this.btnOriginateLoan.Size = new Size(91, 22);
      this.btnOriginateLoan.TabIndex = 1;
      this.btnOriginateLoan.Text = "Originate Loan";
      this.btnOriginateLoan.UseVisualStyleBackColor = true;
      this.btnOriginateLoan.Click += new EventHandler(this.menuItemOriginateLoan_Click);
      this.vs3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.vs3.Location = new Point(225, 3);
      this.vs3.Margin = new Padding(3, 3, 4, 3);
      this.vs3.MaximumSize = new Size(2, 16);
      this.vs3.MinimumSize = new Size(2, 16);
      this.vs3.Name = "vs3";
      this.vs3.Size = new Size(2, 16);
      this.vs3.TabIndex = 29;
      this.vs3.Text = "verticalSeparator4";
      this.btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnRefresh.BackColor = Color.Transparent;
      this.btnRefresh.Location = new Point(203, 3);
      this.btnRefresh.MouseDownImage = (Image) null;
      this.btnRefresh.Name = "btnRefresh";
      this.btnRefresh.Size = new Size(16, 16);
      this.btnRefresh.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.btnRefresh.TabIndex = 4;
      this.btnRefresh.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnRefresh, "Reset");
      this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(181, 3);
      this.btnSave.MouseDownImage = (Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 3;
      this.btnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSave, "Save Changes");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.panelBottom.AutoScroll = true;
      this.panelBottom.BackColor = Color.Transparent;
      this.panelBottom.Dock = DockStyle.Fill;
      this.panelBottom.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.panelBottom.Location = new Point(0, 26);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Padding = new Padding(2, 2, 0, 0);
      this.panelBottom.Size = new Size(1048, 310);
      this.panelBottom.TabIndex = 0;
      this.gradientPanel4.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel4.Controls.Add((Control) this.lblFilter);
      this.gradientPanel4.Controls.Add((Control) this.btnClearSearch);
      this.gradientPanel4.Controls.Add((Control) this.label4);
      this.gradientPanel4.Controls.Add((Control) this.btnAdvSearch);
      this.gradientPanel4.Dock = DockStyle.Top;
      this.gradientPanel4.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel4.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel4.Location = new Point(0, 31);
      this.gradientPanel4.Name = "gradientPanel4";
      this.gradientPanel4.Size = new Size(1050, 31);
      this.gradientPanel4.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel4.TabIndex = 1;
      this.lblFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(38, 9);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(822, 14);
      this.lblFilter.TabIndex = 10;
      this.btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnClearSearch.BackColor = SystemColors.Control;
      this.btnClearSearch.Enabled = false;
      this.btnClearSearch.Location = new Point(988, 5);
      this.btnClearSearch.Name = "btnClearSearch";
      this.btnClearSearch.Padding = new Padding(2, 0, 0, 0);
      this.btnClearSearch.Size = new Size(56, 22);
      this.btnClearSearch.TabIndex = 2;
      this.btnClearSearch.Text = "&Clear";
      this.btnClearSearch.UseVisualStyleBackColor = true;
      this.btnClearSearch.Click += new EventHandler(this.btnClearSearch_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(7, 9);
      this.label4.Name = "label4";
      this.label4.Size = new Size(33, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Filter:";
      this.btnAdvSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAdvSearch.BackColor = SystemColors.Control;
      this.btnAdvSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnAdvSearch.Location = new Point(868, 5);
      this.btnAdvSearch.Margin = new Padding(3, 3, 5, 3);
      this.btnAdvSearch.Name = "btnAdvSearch";
      this.btnAdvSearch.Size = new Size(120, 22);
      this.btnAdvSearch.TabIndex = 1;
      this.btnAdvSearch.Text = "Advanced &Search";
      this.btnAdvSearch.UseVisualStyleBackColor = true;
      this.btnAdvSearch.Click += new EventHandler(this.btnAdvSearch_Click);
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel1.Controls.Add((Control) this.btnRefreshView);
      this.gradientPanel1.Controls.Add((Control) this.btnEditView);
      this.gradientPanel1.Controls.Add((Control) this.btnSaveView);
      this.gradientPanel1.Controls.Add((Control) this.cboView);
      this.gradientPanel1.Controls.Add((Control) this.label1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(0, 0);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1050, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 0;
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
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(7, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(97, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "Contacts View";
      this.imageList1.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList1.ImageStream");
      this.imageList1.TransparentColor = Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "download-available-over.png");
      this.imageList1.Images.SetKeyName(1, "download-available.png");
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1050, 505);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (BorrowerListForm);
      this.Text = nameof (BorrowerListForm);
      this.Closing += new CancelEventHandler(this.BorrowerListForm_Closing);
      this.SizeChanged += new EventHandler(this.BorrowerListForm_SizeChanged);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.btnPrint).EndInit();
      ((ISupportInitialize) this.btnExport).EndInit();
      ((ISupportInitialize) this.siBtnRefresh).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.gvMenuStrip.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      ((ISupportInitialize) this.btnRefresh).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      this.gradientPanel4.ResumeLayout(false);
      this.gradientPanel4.PerformLayout();
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      ((ISupportInitialize) this.btnRefreshView).EndInit();
      ((ISupportInitialize) this.btnEditView).EndInit();
      ((ISupportInitialize) this.btnSaveView).EndInit();
      this.ResumeLayout(false);
    }

    private BorrowerInfo[] getSelectedContacts(bool excludeNoSpam, bool selectAllContacts)
    {
      this.SaveChanges();
      ArrayList arrayList1 = new ArrayList((ICollection) this.getSelectedContactIDs(excludeNoSpam, selectAllContacts));
      if (arrayList1.Count == 0)
        return new BorrowerInfo[0];
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < arrayList1.Count; index += 100)
      {
        int count = Math.Min(100, arrayList1.Count - index);
        BorrowerInfo[] borrowers = this.session.ContactManager.GetBorrowers((int[]) arrayList1.GetRange(index, count).ToArray(typeof (int)));
        arrayList2.AddRange((ICollection) borrowers);
      }
      return (BorrowerInfo[]) arrayList2.ToArray(typeof (BorrowerInfo));
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
          BorrowerSummaryInfo tag = (BorrowerSummaryInfo) selectedItem.Tag;
          if (tag != null && (!excludeNoSpam || intList2.Contains(tag.ContactID)))
            intList1.Add(tag.ContactID);
        }
      }
      else
      {
        foreach (BorrowerSummaryInfo allContact in this.getAllContacts())
        {
          if (!excludeNoSpam || intList2.Contains(allContact.ContactID))
            intList1.Add(allContact.ContactID);
        }
      }
      return intList1.ToArray();
    }

    private int[] getSelectedSpamContactIDs(bool selectAllContacts)
    {
      QueryCriterion queryCriterion = (QueryCriterion) null;
      List<int> intList1 = new List<int>();
      if (!selectAllContacts)
      {
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BorrowerSummaryInfo tag = (BorrowerSummaryInfo) selectedItem.Tag;
          intList1.Add(tag.ContactID);
        }
      }
      else
      {
        foreach (BorrowerSummaryInfo allContact in this.getAllContacts())
          intList1.Add(allContact.ContactID);
      }
      if (intList1.Capacity > 0)
        queryCriterion = (QueryCriterion) new ListValueCriterion("Contact.ContactID", (Array) intList1.ToArray());
      string[] valueList = new string[1]{ "N" };
      BorrowerSummaryInfo[] borrowerSummaryInfoArray = this.session.ContactManager.QueryBorrowerSummaries(new QueryCriterion[1]
      {
        queryCriterion.And((QueryCriterion) new ListValueCriterion("Contact.DoNotSpam", (Array) valueList, true))
      }, RelatedLoanMatchType.None);
      List<int> intList2 = new List<int>();
      foreach (BorrowerSummaryInfo borrowerSummaryInfo in borrowerSummaryInfoArray)
        intList2.Add(borrowerSummaryInfo.ContactID);
      return intList2.ToArray();
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

    public int CurrentContactID
    {
      get => this.currentContactId;
      set
      {
        this.setContactID(value);
        this.btnSave.Enabled = false;
        this.btnRefresh.Enabled = false;
      }
    }

    public object CurrentContact
    {
      get => (object) this.currentContact;
      set
      {
        this.currentContact = (BorrowerInfo) value;
        if (value == null)
          this.setContactID(-1);
        else
          this.setContactID(this.currentContact.ContactID);
        this.btnSave.Enabled = false;
        this.btnRefresh.Enabled = false;
      }
    }

    private void setContactID(int contactId)
    {
      this.enforceSecurity();
      if (this.currentContactId == contactId)
        return;
      this.contactDetailTab.CurrentContactID = contactId;
      this.currentContactId = contactId;
    }

    public void RefreshDataSet()
    {
      this.SaveChanges();
      this.contactDetailTab.InitDynamicTabs();
      this.RefreshList();
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

    public void RefreshList() => this.RefreshList(false);

    public void RefreshList(bool preserveSelections)
    {
      if (this.suspendRefresh)
        return;
      using (CursorActivator.Wait())
      {
        using (PerformanceMeter performanceMeter = PerformanceMeter.StartNew("BorContacts.Refresh", "Refresh the Borrower Contact screen data", true, 1563, nameof (RefreshList), "D:\\ws\\24.3.0.0\\EmLite\\ContactUI\\BorrowerListForm.cs"))
        {
          this.retrieveContactData();
          this.displayCurrentPage(preserveSelections);
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

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
      {
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
        if (this.advFilter.RelatedLoanMatchType != RelatedLoanMatchType.None)
          fieldFilterList.RelatedLoanMatchType = this.advFilter.RelatedLoanMatchType;
      }
      return fieldFilterList;
    }

    private void resetFieldDefs()
    {
      this.allContactFieldDefs = BorrowerReportFieldDefs.GetFieldDefs(false);
      this.contactFieldDefsWOHistory = this.allContactFieldDefs.ExtractFields((BorrowerReportFieldFlags) 0);
      this.contactLoanReportFields = BorrowerLoanReportFieldDefs.GetFieldDefs(this.allContactFieldDefs);
    }

    private void init()
    {
      this.initWithoutSecurity();
      this.enforceSecurity();
      this.resetFieldDefs();
      this.loadViewList(this.session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, FileSystemEntry.PrivateRoot(this.session.UserID)));
      string privateProfileString = this.session.GetPrivateProfileString("BorrowerContact", "DefaultView");
      if (privateProfileString != "" && privateProfileString != this.standardViewName)
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse(privateProfileString)), false);
      if (this.cboView.SelectedIndex < 0)
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(FileSystemEntry.Parse("Personal:\\" + this.session.UserID + "\\" + this.standardViewName)), false);
      EPassMessages.MessageActivity += new EPassMessageEventHandler(this.onEPassMessageActivity);
      this.refreshLeadNotification();
      if (this.CurrentContactID != -2)
        return;
      this.CurrentContactID = -100;
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
        this.cboView.Items.Add((object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.standardViewName, FileSystemEntry.Types.File, this.session.UserID)));
        if (this.currentView == null)
          return;
        ClientCommonUtils.PopulateDropdown((ComboBox) this.cboView, (object) new FileSystemEntryListItem(new FileSystemEntry("\\" + this.currentView.Name, FileSystemEntry.Types.File, this.session.UserID)), false);
      }
      catch (Exception ex)
      {
        Tracing.Log(BorrowerListForm.sw, BorrowerListForm.className, TraceLevel.Error, "Error loading view list: " + (object) ex);
        ErrorDialog.Display(ex);
      }
      finally
      {
        this.suspendEvents = false;
      }
    }

    private void gatherPersonaSettings()
    {
      this.personaSettings.Add(AclFeature.Cnt_Borrower_Copy, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Copy));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_CreatBlankLoan, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_CreatBlankLoan));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_CreateNew, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_CreateNew));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_CreatLoanFrmTemplate, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_CreatLoanFrmTemplate));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_Delete, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Delete));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_Export, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Export));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_Import, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Import));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_LoansTab, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_LoansTab));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_MailMerge, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_MailMerge));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_OrderCredit, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_OrderCredit));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_ProductAndPricing, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_ProductAndPricing));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_Print, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Print));
      this.personaSettings.Add(AclFeature.Cnt_Borrower_Reassign, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Reassign));
      this.personaSettings.Add(AclFeature.Cnt_Synchronization, this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Synchronization));
    }

    private void enforceSecurity()
    {
      bool flag = false;
      if (this.session.UserInfo.IsSuperAdministrator())
        flag = true;
      else if (this.gvContactList.SelectedItems.Count == 0)
        flag = false;
      else if (this.gvContactList.SelectedItems.Count == 1)
      {
        BorrowerSummaryInfo tag = (BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
        flag = tag.OwnerID == this.session.UserID || this.session.AclGroupManager.GetBorrowerContactAccessRight(this.session.UserInfo, tag.OwnerID) == AclResourceAccess.ReadWrite;
      }
      else
      {
        foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
        {
          BorrowerSummaryInfo tag = (BorrowerSummaryInfo) selectedItem.Tag;
          if (tag.OwnerID == this.session.UserID)
            flag = true;
          else if (this.session.AclGroupManager.GetBorrowerContactAccessRight(this.session.UserInfo, tag.OwnerID) != AclResourceAccess.ReadWrite)
          {
            flag = false;
            break;
          }
        }
      }
      this.btnNew.Visible = this.personaSettings[AclFeature.Cnt_Borrower_CreateNew];
      this.btnDuplicate.Visible = this.personaSettings[AclFeature.Cnt_Borrower_Copy];
      this.btnDelete.Visible = this.personaSettings[AclFeature.Cnt_Borrower_Delete];
      this.btnExport.Visible = this.personaSettings[AclFeature.Cnt_Borrower_Export];
      this.btnPrint.Visible = this.personaSettings[AclFeature.Cnt_Borrower_Print];
      this.btnMailMerge.Visible = this.personaSettings[AclFeature.Cnt_Borrower_MailMerge];
      this.btnOriginateLoan.Visible = this.personaSettings[AclFeature.Cnt_Borrower_CreatBlankLoan] || this.personaSettings[AclFeature.Cnt_Borrower_CreatLoanFrmTemplate];
      this.btnOrderCredit.Visible = this.personaSettings[AclFeature.Cnt_Borrower_OrderCredit] && (this.personaSettings[AclFeature.Cnt_Borrower_CreatBlankLoan] || this.personaSettings[AclFeature.Cnt_Borrower_CreatLoanFrmTemplate]);
      this.btnProductPricing.Visible = this.personaSettings[AclFeature.Cnt_Borrower_ProductAndPricing] && (this.personaSettings[AclFeature.Cnt_Borrower_CreatBlankLoan] || this.personaSettings[AclFeature.Cnt_Borrower_CreatLoanFrmTemplate]);
      this.miReassign.Visible = this.personaSettings[AclFeature.Cnt_Borrower_Reassign];
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
        this.btnOriginateLoan.Enabled = false;
        this.btnOrderCredit.Enabled = false;
        this.btnProductPricing.Enabled = false;
        this.btnSave.Enabled = false;
        this.btnRefresh.Enabled = false;
        this.miReassign.Enabled = false;
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
        this.btnOriginateLoan.Enabled = true;
        this.btnOrderCredit.Enabled = true;
        this.btnProductPricing.Enabled = true;
        this.btnSave.Enabled = true;
        this.btnRefresh.Enabled = true;
        this.miReassign.Enabled = flag;
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
        this.btnOriginateLoan.Enabled = false;
        this.btnOrderCredit.Enabled = false;
        this.btnProductPricing.Enabled = false;
        this.btnSave.Enabled = false;
        this.btnRefresh.Enabled = false;
        this.miReassign.Enabled = flag;
        this.contactDetailTab.disableControls();
        this.mailMergeDocMgmtOnly = false;
      }
      if (EnConfigurationSettings.GlobalSettings.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        this.btnSync.Enabled = false;
      if (!this.personaSettings[AclFeature.Cnt_Borrower_MailMerge] && !this.personaSettings[AclFeature.Cnt_Synchronization])
        this.vs2.Visible = false;
      else
        this.vs2.Visible = true;
      if (!this.personaSettings[AclFeature.Cnt_Borrower_CreatBlankLoan] && !this.personaSettings[AclFeature.Cnt_Borrower_CreatLoanFrmTemplate])
        this.vs3.Visible = false;
      else
        this.vs3.Visible = true;
    }

    private void initWithoutSecurity()
    {
      this.contactDetailTab = new BorrowerTabForm((ContactGroupListController) this.groupListController);
      this.contactDetailTab.TopLevel = false;
      this.contactDetailTab.Visible = true;
      this.contactDetailTab.Dock = DockStyle.Fill;
      this.contactDetailTab.RequireContactListRefresh += new EventHandler(this.contactDetailTab_RequireContactListRefresh);
      this.contactDetailTab.SummaryChanged += new BorrowerSummaryChangedEventHandler(this.contactDetailTab_SummaryChanged);
      this.contactDetailTab.ContactDeleted += new ContactDeletedEventHandler(this.contactDeletedHandler);
      this.panelBottom.Controls.Clear();
      this.panelBottom.Controls.Add((Control) this.contactDetailTab);
    }

    private void contactDetailTab_SummaryChanged()
    {
      this.btnSave.Enabled = true;
      this.btnRefresh.Enabled = true;
    }

    private void contactDetailTab_RequireContactListRefresh(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0 || sender == null)
        return;
      BorrowerSummaryInfo tag = (BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
      bool prompt = true;
      if (typeof (bool) == sender.GetType())
        prompt = (bool) sender;
      if (!this.saveChanges(prompt))
        return;
      this.refreshContactInList(tag.ContactID, true);
    }

    private void LogInfo(string msg)
    {
      Tracing.Log(BorrowerListForm.sw, TraceLevel.Info, BorrowerListForm.className, msg);
    }

    private void btnNew_Click(object sender, EventArgs e) => this.CreateNew();

    public void CreateNew()
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        BorrowerInfo borrower = this.session.ContactManager.GetBorrower(this.session.ContactManager.CreateBorrower(new BorrowerInfo()
        {
          OwnerID = this.session.UserInfo.Userid,
          AccessLevel = ContactAccess.Private,
          LastName = "<New Contact>",
          Salutation = "",
          PrimaryContact = true
        }, DateTime.Now, ContactSource.Entered));
        this.gvContactList.SelectedItems.Clear();
        GVItem gvItem = this.createGVItem(new BorrowerSummaryInfo(borrower));
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

    private void btnCopy_Click(object sender, EventArgs e)
    {
      if (this.getSelectedItemsCount() == 0)
      {
        int num1 = (int) MessageBox.Show("Select a contact to duplicate.", "Copy Contact");
      }
      else if (this.getSelectedItemsCount() > 1)
      {
        int num2 = (int) MessageBox.Show("You may only select one contact at a time to duplicate.", "Copy Contact");
      }
      else
      {
        BorrowerSummaryInfo tag = (BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
        if (tag == null)
        {
          int num3 = (int) MessageBox.Show("The selected Contact no longer exists.", "Copy Contact");
        }
        else
        {
          BorrowerInfo borrower1 = this.session.ContactManager.GetBorrower(tag.ContactID);
          Opportunity opportunityByBorrowerId = this.session.ContactManager.GetOpportunityByBorrowerId(tag.ContactID);
          ContactGroupInfo[] groupsForContact = this.session.ContactGroupManager.GetContactGroupsForContact(ContactType.Borrower, this.currentContactId);
          if (borrower1 == null)
          {
            int num4 = (int) MessageBox.Show("The selected Contact no longer exists.", "Copy Contact");
          }
          else
          {
            borrower1.OwnerID = this.session.UserInfo.Userid;
            int borrower2 = this.session.ContactManager.CreateBorrower(borrower1, DateTime.Now, ContactSource.Entered);
            if (opportunityByBorrowerId != null)
            {
              opportunityByBorrowerId.ContactID = borrower2;
              this.session.ContactManager.CreateBorrowerOpportunity(opportunityByBorrowerId);
            }
            ContactCustomField[] fieldsForContact = this.session.ContactManager.GetCustomFieldsForContact(borrower1.ContactID, ContactType.Borrower);
            if (fieldsForContact != null)
            {
              for (int index = 0; index < fieldsForContact.Length; ++index)
                fieldsForContact[index].ContactID = borrower2;
              this.session.ContactManager.UpdateCustomFieldsForContact(borrower2, ContactType.Borrower, fieldsForContact);
            }
            else
              Tracing.Log(BorrowerListForm.sw, TraceLevel.Error, BorrowerListForm.className, "Cannot get custom fields for borrower id " + (object) borrower1.ContactID);
            for (int index = 0; index < groupsForContact.Length; ++index)
            {
              groupsForContact[index].AddedContactIds = new int[1]
              {
                borrower2
              };
              this.session.ContactGroupManager.SaveContactGroup(groupsForContact[index]);
            }
            this.RefreshList(false);
          }
        }
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
          BorrowerSummaryInfo tag = (BorrowerSummaryInfo) selectedItem.Tag;
          if (tag != null)
          {
            if (flag)
            {
              ConfirmDialog confirmDialog = new ConfirmDialog("Delete Contact", "Are you sure you want to delete contact '" + tag.LastName + ", " + tag.FirstName + "'?", (this.gvContactList.SelectedItems.Count > 1 ? 1 : 0) != 0);
              dialogResult = confirmDialog.ShowDialog();
              flag = !confirmDialog.ApplyToAll;
            }
            if (DialogResult.Yes == dialogResult)
            {
              try
              {
                this.session.ContactManager.DeleteOpportunityByBorrowerId(tag.ContactID);
                this.session.ContactManager.DeleteBorrower(tag.ContactID);
              }
              catch (Exception ex)
              {
                int num2 = (int) Utils.Dialog((IWin32Window) this, "Unable to delete contact '" + tag.LastName + ", " + tag.FirstName + "'. The contact has been deleted by another user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              }
              selectedItem.Selected = false;
            }
          }
        }
        this.RefreshList();
      }
    }

    private void btnMailMerge_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerMailMergeUsageCounter", (SFxTag) new SFxUiTag());
      this.LogInfo("Enter btnMailMerge_Click.");
      this.mailMerge(false);
    }

    private void btnMailMergeAll_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerMailMergeAllUsageCounter", (SFxTag) new SFxUiTag());
      this.LogInfo("Enter btnMailMergeAll_Click.");
      this.mailMerge(true);
    }

    private void mailMerge(bool includeAllContacts)
    {
      this.SaveChanges();
      SystemSettings.DeleteTempFiles();
      MailMergeForm mailMergeForm = new MailMergeForm(false, ContactType.Borrower, this.mailMergeDocMgmtOnly);
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
        bool bEmailMerge = false;
        string[] emailAddressOption = (string[]) null;
        if (mailMergeForm.Action == "EmailMerge")
        {
          BorrowerInfo[] selectedContacts = this.getSelectedContacts(true, includeAllContacts);
          if (selectedContacts.Length != 0)
          {
            ContactsEmailSelection contactsEmailSelection = new ContactsEmailSelection((object[]) selectedContacts, ContactType.Borrower, true);
            if (DialogResult.Cancel == contactsEmailSelection.ShowDialog((IWin32Window) this))
              return;
            contactIds = contactsEmailSelection.SelectedContactIds;
            emailAddressOption = contactsEmailSelection.EmailOptions;
          }
          if (contactIds.Length != 0)
            bEmailMerge = true;
        }
        if (bEmailMerge)
        {
          EmailInfoForm emailInfoForm = new EmailInfoForm();
          if (emailInfoForm.ShowDialog() == DialogResult.Cancel)
            return;
          str = emailInfoForm.EmailSubject;
        }
        if (contactIds.Length == 0 & bEmailMerge)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "No custom letters need to be sent to the contacts you selected. They either have empty email addresses or are marked as not to receive emails.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          OutlookServices outlookServices = (OutlookServices) null;
          try
          {
            if (bEmailMerge)
            {
              if (ContactUtils.GetCurrentMailDeliveryMethod() == EmailDeliveryMethod.Outlook)
                outlookServices = new OutlookServices();
              this.performEmailMerge(contactIds, mailMergeForm.LetterFile, str, emailAddressOption, this.session.UserID);
              Cursor.Current = Cursors.WaitCursor;
              ContactUtils.addEmailMergeHistory(contactIds, ContactType.Borrower, mailMergeForm.LetterFile.Name, str);
            }
            else
            {
              if (!ContactUtils.DoMailMerge(contactIds, ContactType.Borrower, mailMergeForm.LetterFile, mailMergeForm.IsPrintPreview, bEmailMerge, str, emailAddressOption, this.session.UserID))
                return;
              ContactUtils.addMailMergeHistory(contactIds, ContactType.Borrower, mailMergeForm.LetterFile.Name);
            }
          }
          catch (Exception ex)
          {
            int num4 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            Tracing.Log(BorrowerListForm.sw, TraceLevel.Error, BorrowerListForm.className, "Error occurred in MailMerge.");
            Tracing.Log(BorrowerListForm.sw, TraceLevel.Error, BorrowerListForm.className, ex.StackTrace);
          }
          finally
          {
            outlookServices?.Dispose();
            Cursor.Current = Cursors.Default;
          }
        }
      }
    }

    private void performEmailMerge(
      int[] contactIds,
      FileSystemEntry letterEntry,
      string emailSubject,
      string[] emailAddressOption,
      string senderUserID)
    {
      ContactUtils.ContactIDs = contactIds;
      ContactUtils.TypeOfContacts = ContactType.Borrower;
      ContactUtils.LetterEntry = letterEntry;
      ContactUtils.IsPrintPreview = false;
      ContactUtils.IsEmailMerge = true;
      ContactUtils.EmailSubject = emailSubject;
      ContactUtils.EmailAddressOption = emailAddressOption;
      ContactUtils.SenderUserID = senderUserID;
      if (new ProgressDialog("Sending Emails to Contacts", new AsynchronousProcess(ContactUtils.DoAsyncMailMerge), (object) null, false).ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Custom letters were successfully sent to the contacts you selected.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public bool IsMenuItemVisible(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          flag = this.btnSync.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_NewContact:
          flag = this.btnNew.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact:
          flag = this.btnDuplicate.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_DeleteContact:
          flag = this.btnDelete.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExport:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected:
          flag = this.btnExport.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_PrintDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails:
        case ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails:
          flag = this.btnPrint.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_MailMerge:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected:
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll:
          flag = this.btnMailMerge.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_AddToGroup:
          flag = this.btnAddToGroup.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup:
          flag = this.btnRemoveFromGroup.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_EditGroup:
          flag = this.btnEditGroup.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan:
          flag = this.btnOriginateLoan.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_OrderCredit:
          flag = this.btnOrderCredit.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ProductPricing:
          flag = this.btnProductPricing.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_BuyLead:
          flag = this.btnBuyLead.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ImportLead:
          flag = this.btnImportLeads.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ImportContact:
          flag = this.personaSettings[AclFeature.Cnt_Borrower_Import];
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_Reassign:
          flag = this.personaSettings[AclFeature.Cnt_Borrower_Reassign];
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_SaveView:
          flag = this.btnSaveView.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ResetView:
          flag = this.btnRefreshView.Visible;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ManageView:
          flag = this.btnEditView.Visible;
          break;
      }
      return flag;
    }

    public bool IsMenuItemEnabled(ContactMainForm.ContactsActionEnum action)
    {
      bool flag = false;
      switch (action)
      {
        case ContactMainForm.ContactsActionEnum.Synchronization:
          flag = this.btnSync.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_NewContact:
          flag = this.btnNew.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact:
          flag = this.btnDuplicate.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_DeleteContact:
          flag = this.btnDelete.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ExportExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExport:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected:
          flag = this.btnExport.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel:
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll:
          flag = this.gvContactList.Items.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_PrintDetails:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails:
          flag = this.btnPrint.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails:
          flag = this.gvContactList.Items.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_MailMerge:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected:
          flag = this.btnMailMerge.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll:
          flag = this.gvContactList.Items.Count > 0;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_AddToGroup:
          flag = this.btnAddToGroup.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup:
          flag = this.btnRemoveFromGroup.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_EditGroup:
          flag = this.btnEditGroup.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan:
          flag = this.btnOriginateLoan.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_OrderCredit:
          flag = this.btnOrderCredit.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ProductPricing:
          flag = this.btnProductPricing.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_BuyLead:
          flag = this.btnBuyLead.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ImportLead:
          flag = this.btnImportLeads.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ImportContact:
          flag = this.personaSettings[AclFeature.Cnt_Borrower_Import];
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_Reassign:
          flag = this.miReassign.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns:
          flag = true;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_SaveView:
          flag = this.btnSaveView.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ResetView:
          flag = this.btnRefreshView.Enabled;
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ManageView:
          flag = this.btnEditView.Enabled;
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
        case ContactMainForm.ContactsActionEnum.Borrower_NewContact:
          this.btnNew_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_DuplicateContact:
          this.btnCopy_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_DeleteContact:
          this.btnDelete_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ExportSelectedExcel:
          this.btnExport_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ExportAllExcel:
          this.exportContactsToExcel(true);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_PrintSelectedDetails:
          this.btnPrint_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_PrintAllDetails:
          this.PrintContactBorrower(true);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeSelected:
          this.btnMailMerge_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_MailMergeAll:
          this.mailMerge(true);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_AddToGroup:
          this.btnAddToGroup_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_RemoveFromGroup:
          this.btnRemoveFromGroup_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_EditGroup:
          this.btnEditGroups_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_OriginateLoan:
          this.btnOriginateLoan.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_OrderCredit:
          this.btnOrderCredit.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ProductPricing:
          this.btnProductPricing.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_BuyLead:
          this.btnBuyLead.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ImportLead:
          this.btnImportLeads.PerformClick();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ImportContact:
          this.ImportContacts();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportAll:
          this.ExportContacts(true);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_CSVExportSelected:
          this.ExportContacts(false);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_Reassign:
          this.btnReassign_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_CustomizeColumns:
          this.gvLayoutManager.CustomizeColumns();
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_SaveView:
          this.btnSaveView_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ResetView:
          this.btnRefreshView_Click((object) null, (EventArgs) null);
          break;
        case ContactMainForm.ContactsActionEnum.Borrower_ManageView:
          this.btnEditView_Click((object) null, (EventArgs) null);
          break;
      }
    }

    public void ImportContacts()
    {
      this.SaveChanges();
      ContactImportWizard contactImportWizard = new ContactImportWizard(ContactType.Borrower);
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
        ContactExportWizard contactExportWizard = new ContactExportWizard(ContactType.Borrower, selectedContactIds);
        contactExportWizard.ContactExported += new ContactExportedEventHandler(this.onContactExported);
        contactExportWizard.ShowDialog((IWin32Window) this.ParentForm);
      }
    }

    private void onContactExported(object contact)
    {
    }

    private void onContactImported(object contact)
    {
    }

    private void lvwContacts_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      this.Cursor = Cursors.WaitCursor;
      try
      {
        if (this.gvContactList.SelectedItems.Count == 1)
        {
          BorrowerSummaryInfo tag = (BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag;
          if (tag == null)
          {
            this.saveChanges(true, false);
            this.CurrentContactID = -1;
          }
          else
          {
            this.saveChanges(true, false);
            this.CurrentContactID = tag.ContactID;
          }
        }
        else
        {
          this.saveChanges(true, false);
          this.CurrentContactID = -1;
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        this.CurrentContactID = -1;
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
      this.RefreshHelpText();
    }

    private void BorrowerListForm_Closing(object sender, CancelEventArgs e) => this.SaveChanges();

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

    private void btnReassign_Click(object sender, EventArgs e)
    {
      this.SaveChanges();
      if (this.getSelectedItemsCount() == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select a contact to reassign.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        using (ContactAssignment contactAssignment = new ContactAssignment(this.session, AclFeature.Cnt_Borrower_Reassign, ""))
        {
          if (contactAssignment.ShowDialog() == DialogResult.Cancel)
            return;
          string[] reassignedContacts = this.changeOwnerOfSelectedContacts(contactAssignment.AssigneeID);
          this.sendContactReassignmentEmail(contactAssignment.AssigneeID, reassignedContacts);
        }
      }
    }

    private void sendContactReassignmentEmail(string newOwnerId, string[] reassignedContacts)
    {
      if (reassignedContacts.Length == 0)
        return;
      UserInfo userInfo = this.session.UserInfo;
      string fullName1 = userInfo.FullName;
      UserInfo user = this.session.OrganizationManager.GetUser(newOwnerId);
      if (user == (UserInfo) null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Unable to send an email to notify the new owner of the contacts assigned to him because the new owner is not a valid Encompass user.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (user.Email == null || user.Email == string.Empty)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Unable to send an email to notify " + user.FullName + " of the contacts assigned to him because he does not have a valid email address.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        string fullName2 = user.FullName;
        string email = user.Email;
        string subject = "Encompass Contacts Reassignment";
        StringBuilder stringBuilder = new StringBuilder(userInfo.FullName + " reassigned " + reassignedContacts.Length.ToString() + " Encompass contact(s) to " + fullName2 + ":\r\n\r\n");
        for (int index = 0; index < reassignedContacts.Length; ++index)
          stringBuilder.Append(reassignedContacts[index] + "\r\n");
        try
        {
          new LoanCenterServiceProxy(Session.SessionObjects?.StartupInfo?.ServiceUrls?.LoanCenterServiceUrl).SendEmail("encompass.email@elliemae.com", email, string.Empty, string.Empty, subject, stringBuilder.ToString());
        }
        catch (Exception ex)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Because there was no Internet connection, Encompass could not notify " + user.FullName + " of the reassigned contacts.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          Tracing.Log(BorrowerListForm.sw, TraceLevel.Error, BorrowerListForm.className, "Encompass could not send an email to notify " + user.FullName + " of the contacts assigned to him because: " + ex.Message);
          return;
        }
        int num4 = (int) Utils.Dialog((IWin32Window) this, "An email was sent to notify " + user.FullName + " of the contact reassignment.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private string[] changeOwnerOfSelectedContacts(string newOwnerId)
    {
      ArrayList arrayList = new ArrayList();
      foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
      {
        BorrowerSummaryInfo tag = (BorrowerSummaryInfo) selectedItem.Tag;
        if (tag != null && !(tag.OwnerID == newOwnerId))
        {
          BorrowerInfo borrower = this.session.ContactManager.GetBorrower(tag.ContactID);
          if (borrower != null)
          {
            borrower.OwnerID = newOwnerId;
            this.session.ContactManager.UpdateBorrower(borrower);
            this.session.ContactManager.QueryBorrowers(new QueryCriterion[1]
            {
              (QueryCriterion) new OrdinalValueCriterion("Contact.ContactID", (object) tag.ContactID)
            });
            arrayList.Add((object) (borrower.FirstName + " " + borrower.LastName));
          }
        }
      }
      this.RefreshList();
      return (string[]) arrayList.ToArray(typeof (string));
    }

    public void RefreshHelpText(ItemCheckEventArgs e)
    {
      int num1 = 0;
      if (e.CurrentValue == CheckState.Unchecked && e.NewValue == CheckState.Checked)
        num1 = 1;
      else if (e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
        num1 = -1;
      int num2 = this.getSelectedItemsCount() + num1;
      IStatusDisplay service = this.session.Application.GetService<IStatusDisplay>();
      if (num2 <= 0)
        service.DisplayHelpText("Press F1 for Help");
      else
        service.DisplayHelpText(num2.ToString() + " contacts selected");
    }

    public void RefreshHelpText()
    {
      IStatusDisplay service = this.session.Application.GetService<IStatusDisplay>();
      int selectedItemsCount = this.getSelectedItemsCount();
      if (selectedItemsCount <= 0)
        service.DisplayHelpText("Press F1 for Help");
      else
        service.DisplayHelpText(selectedItemsCount.ToString() + " contacts selected");
    }

    public void PrintContactBorrower(bool includeAllContacts)
    {
      BorrowerInfo[] selectedContacts = this.getSelectedContacts(false, includeAllContacts);
      if (selectedContacts.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select at least one contact in order to print.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        ContactPrintDialog contactPrintDialog = new ContactPrintDialog(ContactType.Borrower);
        if (contactPrintDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        new PdfFormFacade().ProcessContactPrint(selectedContacts, contactPrintDialog.PrintSummary, contactPrintDialog.PrintPages, contactPrintDialog.PrintPreview);
        this.Focus();
      }
    }

    private void btnPrint_Click(object sender, EventArgs e) => this.PrintContactBorrower(false);

    private void btnPrintAll_Click(object sender, EventArgs e) => this.PrintContactBorrower(true);

    private void btnAddToGroup_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerAddToGroupUsageCounter", (SFxTag) new SFxUiTag());
      if (this.getSelectedItemsCount() == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the contacts you want to add to a group.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(ContactType.Borrower, true);
        if (DialogResult.Cancel == groupSelectionDlg.ShowDialog((IWin32Window) this))
        {
          this.resetFieldDefs();
          this.gvFilterManager.RefreshFilterContent();
          this.RefreshList();
        }
        else
        {
          ContactGroupInfo[] selectedGroups = groupSelectionDlg.SelectedGroups;
          for (int index = 0; index < selectedGroups.Length; ++index)
          {
            ISet a = (ISet) new HashedSet((ICollection) selectedGroups[index].ContactIds);
            ISet set = new HashedSet((ICollection) this.getSelectedContactIDs(false, false)).Minus(a);
            selectedGroups[index].AddedContactIds = new int[set.Count];
            set.CopyTo((Array) selectedGroups[index].AddedContactIds, 0);
            this.session.ContactGroupManager.SaveContactGroup(selectedGroups[index]);
          }
          this.resetFieldDefs();
          this.gvFilterManager.RefreshFilterContent();
          this.RefreshList();
        }
      }
    }

    private void btnRemoveFromGroup_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerRemoveFromGroupUsageCounter", (SFxTag) new SFxUiTag());
      if (this.getSelectedItemsCount() == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You need to select the contacts you want to remove from the " + this.nameForState + " group.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(ContactType.Borrower, false);
        if (DialogResult.Cancel == groupSelectionDlg.ShowDialog((IWin32Window) this))
          return;
        ContactGroupInfo[] selectedGroups = groupSelectionDlg.SelectedGroups;
        for (int index = 0; index < selectedGroups.Length; ++index)
        {
          selectedGroups[index].DeletedContactIds = this.getSelectedContactIDs(false, false);
          this.session.ContactGroupManager.SaveContactGroup(selectedGroups[index]);
        }
        this.RefreshList();
      }
    }

    private void gvContacts_DoubleClick(object sender, EventArgs e)
    {
      if (!this.collapsibleSplitter1.IsCollapsed)
        return;
      this.collapsibleSplitter1.ToggleState();
    }

    public void InsertContactIntoList(int contactID)
    {
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
      {
        if (((BorrowerSummaryInfo) gvItem.Tag).ContactID == contactID)
        {
          gvItem.Selected = true;
          this.gvContactList.EnsureVisible(gvItem.Index);
          return;
        }
      }
      GVItem gvItemForContact = this.getGVItemForContact(contactID);
      this.gvContactList.Items.Add(gvItemForContact);
      gvItemForContact.Selected = true;
      this.gvContactList.EnsureVisible(this.gvContactList.Items.Count - 1);
    }

    private void refreshContactInList(int contactID, bool makeSelect)
    {
      int num = -1;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
      {
        if (((BorrowerSummaryInfo) gvItem.Tag).ContactID == contactID)
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
      using (ICursor cursor = this.session.ContactManager.OpenBorrowerCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new OrdinalValueCriterion("Contact.contactID", (object) contactID, OrdinalMatchType.Equals)
      }, RelatedLoanMatchType.None, (SortField[]) null, this.generateFieldList(), true))
        return this.createGVItem(cursor.GetItem(0) as BorrowerSummaryInfo);
    }

    private void SetContactListState(ContactListState state, int id, string name)
    {
      this.listState = state;
      this.idForState = id;
      this.nameForState = name;
      if (state != ContactListState.Search)
      {
        if (state == ContactListState.Group)
          this.btnRemoveFromGroup.Enabled = true;
        else
          this.btnRemoveFromGroup.Enabled = false;
      }
      else
        this.btnRemoveFromGroup.Enabled = false;
    }

    private void btnEditGroups_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerEditGroupsUsageCounter", (SFxTag) new SFxUiTag());
      int num = (int) new ContactGroupSetupDlg(ContactType.Borrower).ShowDialog((IWin32Window) this);
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

    private void menuItemOriginateLoan_Click(object sender, EventArgs e)
    {
      this.contactDetailTab.getDetailsPage().CurrentContactID = this.currentContactId;
      this.contactDetailTab.getDetailsPage().btnOriginateLoan_Click((object) null, (EventArgs) null);
    }

    private void menuItemOrderCredit_Click(object sender, EventArgs e)
    {
      this.contactDetailTab.getDetailsPage().CurrentContactID = this.currentContactId;
      this.contactDetailTab.getDetailsPage().btnOrderCredit_Click((object) null, (EventArgs) null);
    }

    private void menuItemProductPricing_Click(object sender, EventArgs e)
    {
      this.contactDetailTab.getDetailsPage().CurrentContactID = this.currentContactId;
      this.contactDetailTab.getDetailsPage().btnPricing_Click((object) null, (EventArgs) null);
    }

    private void groupContainer1_Paint(object sender, PaintEventArgs e)
    {
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
        int contactId = 1 == this.gvContactList.SelectedItems.Count ? ((BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag).ContactID : 0;
        this.gvContactList.Items.Clear();
        if (-1 == itemIndex || itemCount == 0)
          return;
        object[] items = this.contactCursor.GetItems(itemIndex, itemCount);
        if (items.Length == 0)
          return;
        GVItem gvItem1 = (GVItem) null;
        foreach (object obj in items)
        {
          if (obj is BorrowerSummaryInfo contactInfo)
          {
            GVItem gvItem2 = this.createGVItem(contactInfo);
            if (gvItem1 == null)
              gvItem1 = gvItem2;
            if (contactId == ((BorrowerSummaryInfo) gvItem2.Tag).ContactID)
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

    private GVItem createGVItem(BorrowerSummaryInfo contactInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) contactInfo;
      for (int index = 0; index < this.gvContactList.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvContactList.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        BorrowerReportFieldDef fieldByCriterionName = this.contactFieldDefsWOHistory.GetFieldByCriterionName(columnId);
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
          }, ContactType.Borrower, newNote1);
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
            }, ContactType.Borrower, newNote2);
            break;
          }
          ContactUtils.addCallHistory(new int[1]
          {
            this.currentContactId
          }, ContactType.Borrower, newNote2);
          break;
      }
      this.Cursor = Cursors.Default;
    }

    private void btnSave_Click(object sender, EventArgs e) => this.saveChanges(false);

    private void btnRefresh_Click(object sender, EventArgs e)
    {
      if (this.contactDetailTab.isDirty() && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Are you sure you want to reset the selected contact? All changes to the contact will be lost.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
        return;
      this.contactDetailTab.ClearChanges();
      this.contactDetailTab.CurrentContactID = this.CurrentContactID;
      this.btnSave.Enabled = false;
      this.btnRefresh.Enabled = false;
    }

    private void btnExport_Click(object sender, EventArgs e) => this.exportContactsToExcel(false);

    private void btnExportAll_Click(object sender, EventArgs e) => this.exportContactsToExcel(true);

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
      excelHandler.AddDataTable(this.gvContactList, (ReportFieldDefs) this.contactLoanReportFields, true);
      excelHandler.CreateExcel();
    }

    private void exportAllContactsToExcel()
    {
      using (CursorActivator.Wait())
      {
        ExcelHandler excelHandler = new ExcelHandler();
        foreach (GVColumn c in this.gvContactList.Columns.DisplaySequence)
          excelHandler.AddHeaderColumn(c.Text, excelHandler.GetColumnFormat(c, (ReportFieldDefs) this.contactLoanReportFields));
        foreach (BorrowerSummaryInfo allContact in this.getAllContacts())
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

    private BorrowerSummaryInfo[] getAllContacts()
    {
      List<BorrowerSummaryInfo> borrowerSummaryInfoList = new List<BorrowerSummaryInfo>();
      foreach (object obj in this.contactCursor.GetItems(0, this.contactCursor.GetItemCount()))
        borrowerSummaryInfoList.Add((BorrowerSummaryInfo) obj);
      return borrowerSummaryInfoList.ToArray();
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
          ContactView templateSettings = (ContactView) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, fsEntry);
          if (templateSettings == null)
            throw new ArgumentException();
          this.btnEditView.Enabled = !fsEntry.IsPublic;
          this.fsViewEntry = fsEntry;
          this.setCurrentView(templateSettings);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(BorrowerListForm.sw, BorrowerListForm.className, TraceLevel.Error, "Error opening view: " + (object) ex);
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

    private void validateTableLayout(TableLayout layout)
    {
      List<TableLayout.Column> columnList = new List<TableLayout.Column>();
      foreach (TableLayout.Column column in layout)
      {
        ReportFieldDef fieldByCriterionName = (ReportFieldDef) this.contactFieldDefsWOHistory.GetFieldByCriterionName(column.ColumnID);
        if (fieldByCriterionName != null)
          column.Title = fieldByCriterionName.Description;
        else
          columnList.Add(column);
      }
      foreach (TableLayout.Column column in columnList)
        layout.Remove(column);
    }

    private void setViewChanged(bool modified)
    {
      this.btnSaveView.Enabled = modified;
      this.btnRefreshView.Enabled = modified;
    }

    private void applyTableLayout(TableLayout layout)
    {
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      this.gvLayoutManager.ApplyLayout(layout, false);
      if (this.gvFilterManager == null)
      {
        this.gvFilterManager = new GridViewReportFilterManager(this.session, this.gvContactList);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefsWOHistory);
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
        this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefsWOHistory);
      this.borrowerTableLayout = this.gvLayoutManager.GetCurrentLayout();
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefsWOHistory);
      this.setViewChanged(true);
      this.RefreshList(true);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (BorrowerReportFieldDef field in (ReportFieldDefContainer) this.allContactFieldDefs.ExtractFields(BorrowerReportFieldFlags.IncludeBasicFields))
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
      using (SaveViewTemplateDialog viewTemplateDialog = new SaveViewTemplateDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, (object) contactView, this.getViewNameList(), this.currentView.Name != this.standardViewName))
      {
        if (viewTemplateDialog.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          return;
        if (!viewTemplateDialog.SelectedEntry.Equals((object) this.fsViewEntry))
          this.updateCurrentView(contactView, viewTemplateDialog.SelectedEntry);
      }
      this.currentView = contactView;
      this.btnSaveView.Enabled = false;
      this.btnRefreshView.Enabled = false;
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
      using (ViewManagementDialog managementDialog = new ViewManagementDialog(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, false, "BorrowerContact.DefaultView"))
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
        this.session.ConfigurationManager.DeleteTemplateSettingsObject(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, this.fsViewEntry);
      }
      catch (Exception ex)
      {
        Tracing.Log(BorrowerListForm.sw, BorrowerListForm.className, TraceLevel.Error, "Error deleting view: " + (object) ex);
      }
      this.RefreshViews();
    }

    public void RefreshViews()
    {
      this.loadViewList(this.session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, FileSystemEntry.PrivateRoot(this.session.UserID)));
      if (this.cboView.Items.Count <= 0 || this.cboView.SelectedIndex >= 0)
        return;
      this.cboView.SelectedIndex = 0;
    }

    private void btnAdvSearch_Click(object sender, EventArgs e)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.Merge(this.advFilter);
      if (this.contactLoanReportFields == null)
        this.contactLoanReportFields = BorrowerLoanReportFieldDefs.GetFieldDefs();
      using (ContactAdvSearchDialog contactAdvSearchDialog = new ContactAdvSearchDialog((ReportFieldDefs) this.contactLoanReportFields, fieldFilterList))
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
      MetricsFactory.IncrementCounter("ContactsBorrowerSynchronizeUsageCounter", (SFxTag) new SFxUiTag());
      this.parentForm.ContactMenu_Click("Synchronize");
    }

    private void btnBuyLead_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerBuyLeadUsageCounter", (SFxTag) new SFxUiTag());
      this.ShowLeadCenter();
    }

    private void onEPassMessageActivity(object sender, EPassMessageEventArgs eventArgs)
    {
      if (eventArgs.EventType == EPassMessageEventType.MessagesSynced)
      {
        this.refreshLeadNotification();
      }
      else
      {
        if (eventArgs.Message == null || !(eventArgs.Message.MessageType == "LC_LEADS"))
          return;
        this.refreshLeadNotification();
      }
    }

    private void refreshLeadNotification()
    {
      if (this.InvokeRequired)
        this.Invoke((Delegate) new MethodInvoker(this.refreshLeadNotification));
      else if (this.session.ConfigurationManager.GetEPassMessageCountForUser(this.session.UserID, new string[1]
      {
        "LC_LEADS"
      }) > 0)
      {
        this.btnImportLeads.ImageList = this.imageList1;
        this.btnImportLeads.ImageIndex = 1;
        this.btnImportLeads.TextImageRelation = TextImageRelation.ImageBeforeText;
      }
      else
        this.btnImportLeads.ImageList = (ImageList) null;
    }

    private void btnImportLeads_Click(object sender, EventArgs e)
    {
      MetricsFactory.IncrementCounter("ContactsBorrowerImportLeadsUsageCounter", (SFxTag) new SFxUiTag());
      this.ShowLeadMailbox();
    }

    public void ShowContacts(
      QueryCriterion criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields,
      string description)
    {
      this.retrieveContactData(criteria, matchType, sortFields);
      this.displayCurrentPage(true);
      this.lblFilter.Text = description;
    }

    public void ShowContacts(FieldFilterList filter, SortField[] sortFields)
    {
      this.suspendEvents = true;
      this.gvFilterManager.ClearColumnFilters();
      this.advFilter = filter;
      this.suspendEvents = false;
      this.RefreshList(false);
      this.setViewChanged(true);
    }

    public void ShowLeadCenter()
    {
      using (LeadCenterDialog leadCenterDialog = new LeadCenterDialog())
      {
        int num = (int) leadCenterDialog.ShowDialog((IWin32Window) this);
      }
    }

    public void ShowLeadMailbox()
    {
      this.saveChanges(true);
      if (this.session.Application.GetService<IEncompassApplication>().IsModalDialogOpen())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Lead Center cannot be opened because other screens are being displayed. You must first close all pop-up windows and then open the Lead Center.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        using (LeadCenterDialog leadCenterDialog = new LeadCenterDialog())
        {
          leadCenterDialog.ImportLeads();
          this.RefreshList(true);
        }
      }
    }

    public void ShowLeads()
    {
      string str = this.session.GetPrivateProfileString("LeadCenter", "SearchAfterImport");
      if (string.IsNullOrEmpty(str))
      {
        using (ViewLeadsDialog viewLeadsDialog = new ViewLeadsDialog())
        {
          str = viewLeadsDialog.ShowDialog() != DialogResult.Yes ? "N" : "Y";
          if (viewLeadsDialog.DoNotPrompt)
            this.session.WritePrivateProfileString("LeadCenter", "SearchAfterImport", str);
        }
      }
      if (!(str == "Y"))
        return;
      this.session.Application.GetService<IEncompassApplication>().SetCurrentActivity(EncompassActivity.BorrowerContacts);
      FieldFilterList filter = new FieldFilterList();
      filter.Add(new FieldFilter(FieldTypes.IsOptionList, "History.First Contact.ContactSource", "FirstContact.ContactSource", "Data Source", OperatorTypes.IsAnyOf, new string[1]
      {
        "LeadCenter"
      }, "Imported from Lead Center"));
      filter.Add(new FieldFilter(FieldTypes.IsDate, "History.First Contact.TimeOfHistory", "FirstContact.TimeOfHistory", "First Contact Date", OperatorTypes.DateOnOrAfter, DateTime.Today.ToShortDateString()), JointTokens.And);
      this.ShowContacts(filter, (SortField[]) null);
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
      BorrowerSummaryInfo[] borrowerSummaryInfoArray = new BorrowerSummaryInfo[0];
      if (currentPageItemCount > 0)
        borrowerSummaryInfoArray = (BorrowerSummaryInfo[]) this.contactCursor.GetItems(currentPageItemIndex, currentPageItemCount);
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      if (preserveSelections)
      {
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
        {
          if (gvItem.Selected)
            dictionary[((BorrowerSummaryInfo) gvItem.Tag).ContactID] = true;
        }
      }
      this.gvContactList.Items.Clear();
      for (int index = 0; index < borrowerSummaryInfoArray.Length; ++index)
      {
        GVItem gvItem = this.createGVItem(borrowerSummaryInfoArray[index]);
        this.gvContactList.Items.Add(gvItem);
        if (dictionary.ContainsKey(borrowerSummaryInfoArray[index].ContactID))
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
        SortField sortFieldForColumn = this.getSortFieldForColumn((TableLayout.Column) this.gvContactList.Columns[gvColumnSort.Column].Tag, gvColumnSort.SortOrder);
        if (sortFieldForColumn != null)
          sortFieldList.Add(sortFieldForColumn);
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(TableLayout.Column colInfo, SortOrder sortOrder)
    {
      BorrowerReportFieldDef fieldByCriterionName = this.contactFieldDefsWOHistory.GetFieldByCriterionName(colInfo.ColumnID);
      return fieldByCriterionName != null ? new SortField(fieldByCriterionName.SortTerm, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion) : (SortField) null;
    }

    public void NavigateCustomerLoyalty(string url)
    {
      using (CustomerLoyaltyDialog customerLoyaltyDialog = new CustomerLoyaltyDialog(url))
      {
        int num = (int) customerLoyaltyDialog.ShowDialog((IWin32Window) Form.ActiveForm);
      }
    }

    private void miNewAppointment_Click(object sender, EventArgs e)
    {
      this.contactDetailTab.AddAppointment();
    }

    private void siBtnRefresh_Click(object sender, EventArgs e)
    {
      this.gvFilterManager.RefreshFilterContent();
      this.RefreshList(true);
    }

    private void gvMenuStrip_Opening(object sender, CancelEventArgs e)
    {
      this.miNewContact.Visible = this.btnNew.Visible;
      this.miNewContact.Enabled = this.btnNew.Enabled;
      this.miDuplicateContact.Visible = this.btnDuplicate.Visible;
      this.miDuplicateContact.Enabled = this.btnDuplicate.Enabled;
      this.miDeleteContact.Visible = this.btnDelete.Visible;
      this.miDeleteContact.Enabled = this.btnDelete.Enabled;
      this.miExport.Visible = this.btnExport.Visible;
      if (this.gvContactList.Items.Count > 0)
        this.miExportAll.Enabled = true;
      else
        this.miExportAll.Enabled = false;
      if (this.gvContactList.SelectedItems.Count > 0)
        this.miExportSelectedOnly.Enabled = true;
      else
        this.miExportSelectedOnly.Enabled = false;
      this.miPrint.Visible = this.btnPrint.Visible;
      if (this.gvContactList.Items.Count > 0)
        this.miPrintAll.Enabled = true;
      else
        this.miPrintAll.Enabled = false;
      if (this.gvContactList.SelectedItems.Count > 0)
        this.miPrintSelectedOnly.Enabled = true;
      else
        this.miPrintSelectedOnly.Enabled = false;
      this.miMailMerge.Visible = this.btnMailMerge.Visible;
      if (this.gvContactList.Items.Count > 0)
        this.miMailMergeAll.Enabled = true;
      else
        this.miMailMergeAll.Enabled = false;
      if (this.gvContactList.SelectedItems.Count > 0)
        this.miMailMergeSelectedOnly.Enabled = true;
      else
        this.miMailMergeSelectedOnly.Enabled = false;
      this.miAddToGroup.Visible = this.btnAddToGroup.Visible;
      this.miAddToGroup.Enabled = this.btnAddToGroup.Enabled;
      this.miRemoveFromGroup.Visible = this.btnRemoveFromGroup.Visible;
      this.miRemoveFromGroup.Enabled = this.btnRemoveFromGroup.Enabled;
      this.miOriginateLoan.Visible = this.btnOriginateLoan.Visible;
      this.miOriginateLoan.Enabled = this.btnOriginateLoan.Enabled;
      this.miOrderCredit.Visible = this.btnOrderCredit.Visible;
      this.miOrderCredit.Enabled = this.btnOrderCredit.Enabled;
      this.miProductAndPricing.Visible = this.btnProductPricing.Visible;
      this.miProductAndPricing.Enabled = this.btnProductPricing.Enabled;
      this.miNewAppointment.Visible = true;
      this.miNewAppointment.Enabled = this.gvContactList.SelectedItems.Count == 1;
      if (!this.btnNew.Visible && !this.btnDuplicate.Visible && !this.btnDelete.Visible)
        this.toolStripSeparator1.Visible = false;
      else
        this.toolStripSeparator1.Visible = true;
      if (!this.btnExport.Visible && !this.btnPrint.Visible && !this.btnMailMerge.Visible)
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

    private void BorrowerListForm_SizeChanged(object sender, EventArgs e)
    {
    }

    protected void ChangeSize()
    {
    }

    private delegate void ContactMethod(int contactId);
  }
}
