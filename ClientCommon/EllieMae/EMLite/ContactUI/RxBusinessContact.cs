// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.RxBusinessContact
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class RxBusinessContact : Form
  {
    private FeaturesAclManager aclMgr;
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private TableLayout bizPartnerTableLayout;
    private ICursor contactCursor;
    private BizPartnerReportFieldDefs contactFieldDefs;
    private FieldFilterList advFilter;
    private ContactView currentView;
    private PageChangedEventArgs currentPagingArgument;
    private static string sw = Tracing.SwContact;
    private BizPartnerInfo bizObj;
    private string currentAssContactGuid = "";
    private bool multiSelect;
    private bool forEPass;
    private bool bMatchMortgageClause;
    private string category = "";
    private string companyName = "";
    private string contactLastName = "";
    private RxContactInfo rxContact;
    private bool bMatchCompany;
    private bool bMatchOnlyCompany;
    private BizCategoryUtil catUtil;
    private CRMRoleType crmRoleType = CRMRoleType.NotFound;
    private string selectedXML = "";
    private RxContactInfo contactInfo;
    private CategoryCustomFieldsControl ctlCategoryStandardFields;
    private bool goToContact;
    private ContactInfo selectedContact;
    private RxBusinessContact.ActionMode currentMode;
    private bool initialLoad;
    private bool deleteBackKey;
    private bool suspendEvents;
    private string assMappingID = "";
    private bool forExternal;
    private bool forExternalLender;
    private Sessions.Session session;
    private int parent;
    private int depth;
    private string hierarchyPath;
    private List<FetchContacts> BusinessContacts = new List<FetchContacts>();
    private Dictionary<string, string> alreadyExists = new Dictionary<string, string>();
    private Dictionary<int, string> hierarchyNodes = new Dictionary<int, string>();
    private IContainer components;
    private GroupContainer groupContainer1;
    private GridView gvContactList;
    private Button btnNewLink;
    private Button btnLink;
    private Button btnCancel;
    private PictureBox picLinked;
    private PageListNavigator navContacts;
    private GradientPanel gradientPanel3;
    private Button btnClear;
    private Label label4;
    private Button btnGoto;
    private StandardIconButton siBtnSave;
    private StandardIconButton btnDelete;
    private StandardIconButton btnDuplicate;
    private StandardIconButton btnNew;
    private FlowLayoutPanel flowLayoutPanel1;
    private FlowLayoutPanel flowLayoutPanel2;
    private CollapsibleSplitter collapsibleSplitter1;
    private GroupContainer gcDetail;
    private Panel panel1;
    private TabControl tabControl1;
    private TabPage tpDetails;
    private GroupContainer groupContainer3;
    private TextBox txtBoxJobTitle;
    private Label lblTitle;
    private Label lblWebUrl;
    private TextBox txtBoxBizWebUrl;
    private TextBox txtBoxFees;
    private Label lblFees;
    private TextBox txtBoxBizState;
    private TextBox txtBoxBizZip;
    private Label lblBizZip;
    private Label lblBizState;
    private TextBox txtBoxBizCity;
    private TextBox txtBoxBizAddress2;
    private Label lblBizCity;
    private Label lblBizAddress2;
    private Label lblBizAddress1;
    private TextBox txtBoxBizAddress1;
    private ComboBox cmbBoxCategoryID;
    private TextBox txtBoxCompanyName;
    private Label lblCategory;
    private Label lblCompany;
    private GroupContainer groupContainer2;
    private CheckBox chkPublic;
    private TextBox txtBoxLastName;
    private CheckBox chkBoxNoSpam;
    private Label lblWorkEmail;
    private TextBox txtBoxBizEmail;
    private Label lblFaxNumber;
    private TextBox txtBoxSalutation;
    private Label lblCellPhone;
    private Label label2;
    private TextBox txtBoxWorkPhone;
    private TextBox txtBoxPersonalEmail;
    private Label lblWorkPhone;
    private TextBox txtBoxFaxNumber;
    private Label lblHomeEmail;
    private TextBox txtBoxMobilePhone;
    private Label lblHomePhone;
    private TextBox txtBoxHomePhone;
    private TextBox txtBoxFirstName;
    private Label lblFirstName;
    private Label lblLastName;
    private TabPage tabCategoryStandardFields;
    private ToolTip toolTip1;
    private BorderPanel borderPanel1;
    private FlowLayoutPanel flowLayoutPanel3;
    private CheckBox chkLink;
    private Label lblFilter;
    private VerticalSeparator verticalSeparator1;
    private Label label8;
    private Label label9;
    private Label label10;
    private Label label11;
    private Label label7;
    private Label label6;
    private Label label5;
    private Label label3;
    private Label label1;
    private TextBox txtBoxBizInfoLicAuthName;
    private Label lblLicenseNum;
    private TextBox txtBoxLicenseNumber;
    private TextBox txtBoxPersonalInfoLicAuthNAme;
    private TextBox txtBoxPersonalInfoLicNo;
    private DatePicker dpBizLicIssueDate;
    private DatePicker dpPersonalInfoLicIssueDate;
    private ComboBox comboBoxBizInfoLicState;
    private ComboBox comboBoxPersonalInfoLicState;
    private ComboBox comboBoxBizInfoLicAuthType;
    private ComboBox comboBoxPersonalInfoLicAuthType;

    public Dictionary<int, string> HierarchyNodes => this.hierarchyNodes;

    public RxBusinessContact(string category, bool multiSelect, bool allowGoto)
      : this(category, "", "", (RxContactInfo) null, CRMRoleType.NotFound, allowGoto, "")
    {
      this.multiSelect = multiSelect;
      this.gvContactList.AllowMultiselect = multiSelect;
      this.forEPass = true;
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      CRMRoleType crmRoleType,
      bool allowGoto,
      string associateMappingID,
      Sessions.Session session)
      : this(category, companyName, contactLastName, rxContact, true, false, false, crmRoleType, allowGoto, crmRoleType == CRMRoleType.NotFound ? RxBusinessContact.ActionMode.SelectMode : RxBusinessContact.ActionMode.LinkMode, associateMappingID, session)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      CRMRoleType crmRoleType,
      bool allowGoto,
      string associateMappingID)
      : this(category, companyName, contactLastName, rxContact, true, crmRoleType, allowGoto, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      bool bMatchCompany,
      CRMRoleType crmRoleType,
      bool allowGoto,
      string associateMappingID)
      : this(category, companyName, contactLastName, rxContact, bMatchCompany, false, crmRoleType, allowGoto, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      bool bMatchCompany,
      CRMRoleType crmRoleType,
      bool allowGoto,
      RxBusinessContact.ActionMode actionMode,
      string associateMappingID)
      : this(category, companyName, contactLastName, rxContact, bMatchCompany, false, crmRoleType, allowGoto, actionMode, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      RxContactInfo rxContact,
      CRMRoleType crmRoleType,
      bool allowGoto,
      string associateMappingID)
      : this(category, companyName, (string) null, rxContact, true, true, true, crmRoleType, allowGoto, crmRoleType == CRMRoleType.NotFound ? RxBusinessContact.ActionMode.SelectMode : RxBusinessContact.ActionMode.LinkMode, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      RxContactInfo rxContact,
      CRMRoleType crmRoleType,
      bool allowGoto,
      RxBusinessContact.ActionMode actionMode,
      string associateMappingID)
      : this(category, companyName, (string) null, rxContact, true, true, true, crmRoleType, allowGoto, actionMode, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      bool bMatchCompany,
      bool bMatchOnlyCompany,
      CRMRoleType crmRoleType,
      bool allowGoto,
      string associateMappingID)
      : this(category, companyName, contactLastName, rxContact, bMatchCompany, bMatchOnlyCompany, false, crmRoleType, allowGoto, crmRoleType == CRMRoleType.NotFound ? RxBusinessContact.ActionMode.SelectMode : RxBusinessContact.ActionMode.LinkMode, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      bool bMatchCompany,
      bool bMatchOnlyCompany,
      CRMRoleType crmRoleType,
      bool allowGoto,
      RxBusinessContact.ActionMode actionMode,
      string associateMappingID)
      : this(category, companyName, contactLastName, rxContact, bMatchCompany, bMatchOnlyCompany, false, crmRoleType, allowGoto, actionMode, associateMappingID)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      bool bMatchCompany,
      bool bMatchOnlyCompany,
      bool bMatchMortgageClause,
      CRMRoleType crmRoleType,
      bool allowGoto,
      RxBusinessContact.ActionMode actionMode,
      string associateMappingID)
      : this(category, companyName, contactLastName, rxContact, bMatchCompany, bMatchOnlyCompany, bMatchMortgageClause, crmRoleType, allowGoto, actionMode, associateMappingID, Session.DefaultInstance)
    {
    }

    public RxBusinessContact(
      string category,
      string companyName,
      string contactLastName,
      RxContactInfo rxContact,
      bool bMatchCompany,
      bool bMatchOnlyCompany,
      bool bMatchMortgageClause,
      CRMRoleType crmRoleType,
      bool allowGoto,
      RxBusinessContact.ActionMode actionMode,
      string associateMappingID,
      Sessions.Session session)
    {
      this.session = session;
      this.bMatchMortgageClause = bMatchMortgageClause;
      this.category = category;
      this.companyName = companyName;
      this.contactLastName = contactLastName;
      this.rxContact = rxContact;
      this.contactInfo = rxContact;
      this.bMatchCompany = bMatchCompany;
      this.bMatchOnlyCompany = bMatchOnlyCompany;
      this.crmRoleType = crmRoleType;
      this.currentMode = actionMode;
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.InitializeComponent();
      this.picLinked.Visible = false;
      this.assMappingID = associateMappingID;
      if (!allowGoto)
        this.btnGoto.Visible = false;
      this.init();
    }

    public RxBusinessContact(bool allowGoto)
      : this(RxBusinessContact.ActionMode.ManageMode, allowGoto)
    {
    }

    public RxBusinessContact(RxBusinessContact.ActionMode selectedMode, bool allowGoto)
      : this("", "", "", (RxContactInfo) null, CRMRoleType.NotFound, true, "")
    {
      this.multiSelect = false;
      this.btnGoto.Visible = allowGoto;
      this.gvContactList.AllowMultiselect = false;
      this.setCurrentMode(selectedMode);
    }

    public RxBusinessContact(
      string category,
      RxBusinessContact.ActionMode selectedMode,
      bool allowGoto)
      : this(category, "", "", (RxContactInfo) null, false, false, false, CRMRoleType.NotFound, allowGoto, selectedMode, "")
    {
    }

    public RxBusinessContact(
      Sessions.Session session,
      int parent,
      int depth,
      string hierarchyPath)
    {
      this.InitializeComponent();
      this.forExternal = true;
      if (hierarchyPath.StartsWith("Lenders"))
        this.forExternalLender = true;
      this.gvContactList.AllowMultiselect = true;
      this.session = session;
      this.parent = parent;
      this.depth = depth;
      this.hierarchyPath = hierarchyPath;
      this.multiSelect = true;
      this.btnGoto.Visible = false;
      this.picLinked.Visible = false;
      this.gvContactList.AllowMultiselect = true;
      this.setCurrentMode(RxBusinessContact.ActionMode.SelectMode);
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.init();
      this.StartPosition = FormStartPosition.CenterScreen;
    }

    public void SetReadOnlyMode()
    {
      this.btnLink.Visible = false;
      this.btnNewLink.Visible = false;
      this.chkLink.Enabled = false;
      this.gvContactList.DoubleClick -= new EventHandler(this.gvContactList_DoubleClick);
    }

    private void init()
    {
      this.initialLoad = true;
      this.catUtil = new BizCategoryUtil(this.session.SessionObjects);
      this.ctlCategoryStandardFields = new CategoryCustomFieldsControl(CustomFieldsType.BizCategoryStandard, this.session);
      this.ctlCategoryStandardFields.Dock = DockStyle.Fill;
      this.tabCategoryStandardFields.Controls.Add((Control) this.ctlCategoryStandardFields);
      this.ctlCategoryStandardFields.IsReadOnly = true;
      this.ctlCategoryStandardFields.DataChangedEvent += new EventHandler(this.dataChanged);
      this.enforceFeatureSecurity();
      this.initialGVContactList();
      this.cmbBoxCategoryID.Items.Clear();
      this.comboBoxBizInfoLicState.Items.Clear();
      this.comboBoxPersonalInfoLicState.Items.Clear();
      this.comboBoxBizInfoLicState.Items.AddRange((object[]) Utils.GetStates());
      this.comboBoxPersonalInfoLicState.Items.AddRange((object[]) Utils.GetStates());
      this.cmbBoxCategoryID.Items.AddRange((object[]) this.session.ContactManager.GetBizCategories());
      ContactView view = new ContactView("DefaultView");
      view.Layout = this.getDefaultTableLayout();
      LoanData loanData = this.session.LoanData;
      view.Filter = new FieldFilterList();
      if (this.category != null && this.category != "")
        view.Filter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "ContactCategory.CategoryName", "Category", OperatorTypes.Equals, this.category), JointTokens.And);
      if (this.companyName != null && this.companyName != "")
        view.Filter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "Contact.CompanyName", "Company Name", OperatorTypes.Contains, this.companyName), JointTokens.And);
      if (this.contactLastName != null && this.contactLastName != "")
        view.Filter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "Contact.LastName", "Contact Last Name", OperatorTypes.Contains, this.contactLastName), JointTokens.And);
      this.setCurrentView(view);
      if (this.session.LoanData != null && this.crmRoleType != CRMRoleType.NotFound && this.session.LoanData.GetLogList().GetCRMMapping(this.assMappingID) != null)
        this.currentAssContactGuid = this.session.LoanData.GetLogList().GetCRMMapping(this.assMappingID).ContactGuid;
      int num = -1;
      if (this.bMatchMortgageClause && (this.companyName ?? "") != string.Empty)
        num = this.session.ContactManager.QueryBizPartnerMortgageClause(this.rxContact.CategoryID, this.companyName);
      for (int nItemIndex = 0; nItemIndex < this.gvContactList.Items.Count; ++nItemIndex)
      {
        BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.Items[nItemIndex].Tag;
        if (this.bMatchOnlyCompany)
        {
          if (this.bMatchMortgageClause)
          {
            if (num != -1)
            {
              if (tag.ContactID == num)
              {
                this.gvContactList.Items[nItemIndex].Selected = true;
                break;
              }
            }
            else
              break;
          }
          else if (this.companyName != "" && tag.CompanyName == this.companyName)
          {
            this.gvContactList.Items[nItemIndex].Selected = true;
            break;
          }
        }
        else if (this.contactLastName != "" && tag.LastName == this.contactLastName)
        {
          if (this.bMatchCompany)
          {
            if (this.companyName != "" && tag.CompanyName == this.companyName)
            {
              this.gvContactList.Items[nItemIndex].Selected = true;
              break;
            }
          }
          else
          {
            this.gvContactList.Items[nItemIndex].Selected = true;
            break;
          }
        }
      }
      if (this.gvContactList.SelectedItems != null && this.gvContactList.SelectedItems.Count > 0)
        this.gvContactList.EnsureVisible(this.gvContactList.SelectedItems[0].Index);
      if (this.session.EncompassEdition == EncompassEdition.Broker)
        this.chkPublic.Visible = false;
      this.setCurrentMode(this.currentMode);
      this.initialLoad = false;
    }

    private void setCurrentMode(RxBusinessContact.ActionMode mode)
    {
      this.currentMode = mode;
      switch (this.currentMode)
      {
        case RxBusinessContact.ActionMode.SelectMode:
          this.btnLink.Visible = true;
          this.btnNewLink.Visible = false;
          this.siBtnSave.Visible = false;
          this.verticalSeparator1.Visible = false;
          this.btnNew.Visible = false;
          this.btnDuplicate.Visible = false;
          this.btnDelete.Visible = false;
          this.chkLink.Checked = false;
          this.chkLink.Visible = false;
          break;
        case RxBusinessContact.ActionMode.LinkMode:
          this.btnLink.Visible = true;
          this.btnNewLink.Visible = true;
          this.siBtnSave.Visible = false;
          this.verticalSeparator1.Visible = false;
          this.btnNew.Visible = false;
          this.btnDuplicate.Visible = false;
          this.btnDelete.Visible = false;
          this.chkLink.Checked = true;
          this.chkLink.Visible = true;
          break;
        case RxBusinessContact.ActionMode.ManageMode:
          this.btnLink.Visible = false;
          this.btnNewLink.Visible = false;
          this.siBtnSave.Visible = true;
          this.verticalSeparator1.Visible = true;
          this.btnNew.Visible = true;
          this.btnDuplicate.Visible = true;
          this.btnDelete.Visible = true;
          this.chkLink.Checked = false;
          this.chkLink.Visible = false;
          break;
      }
      if (this.forExternal)
        return;
      this.enforceFeatureSecurity();
    }

    private void setCurrentView(ContactView view)
    {
      this.suspendEvents = true;
      this.currentView = view;
      this.applyTableLayout(view.Layout);
      this.suspendEvents = false;
      this.advFilter = (FieldFilterList) null;
      this.SetCurrentFilter(view.Filter);
    }

    public void SetCurrentFilter(FieldFilterList filter)
    {
      this.advFilter = filter;
      this.gvFilterManager.ClearColumnFilters();
      this.populateContacts(this.generateCriterion());
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
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvContactList, this.getFullTableLayout(), this.getDefaultTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
      if (this.suspendEvents)
        return;
      if (this.gvFilterManager != null)
        this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
      this.bizPartnerTableLayout = this.gvLayoutManager.GetCurrentLayout();
      this.populateContacts(this.generateCriterion());
    }

    private void enforceFeatureSecurity()
    {
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_CreateNew))
      {
        this.btnNewLink.Visible = false;
        this.btnNew.Visible = false;
        this.btnDuplicate.Visible = false;
      }
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Delete))
        this.btnDelete.Visible = false;
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.GlobalTab_Contacts) || !this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Access))
        this.btnGoto.Visible = false;
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Biz_Access))
        this.siBtnSave.Visible = false;
      if (this.session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.tabControl1.TabPages.Remove(this.tabCategoryStandardFields);
    }

    private void initialGVContactList()
    {
      this.contactFieldDefs = BizPartnerReportFieldDefs.GetFieldDefs(this.session, true, ContactType.BizPartner);
      if (this.gvLayoutManager == null)
        this.gvLayoutManager = this.createLayoutManager();
      if (this.bizPartnerTableLayout != null)
        this.gvLayoutManager.ApplyLayout(this.bizPartnerTableLayout, false);
      this.gvFilterManager = new GridViewReportFilterManager(this.session, this.gvContactList);
      this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
      this.buildQuerySummary(true);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (BizPartnerReportFieldDef selectableFieldDef in (ReportFieldDefContainer) BizPartnerReportFieldDefs.GetSelectableFieldDefs(this.session, true, ContactType.BizPartner))
      {
        if (fullTableLayout.GetColumnByID(selectableFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(ReportFieldClientExtension.ToTableLayoutColumn(selectableFieldDef));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.populateContacts(this.generateCriterion());
    }

    private void buildQuerySummary(bool populateLabel)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      if (fieldFilterList.ToString(true) == "")
      {
        this.btnClear.Enabled = false;
        this.lblFilter.Text = "None";
      }
      else
      {
        this.btnClear.Enabled = true;
        this.lblFilter.Text = fieldFilterList.ToString(true);
      }
    }

    private QueryCriterion[] generateCriterion()
    {
      QueryCriterion queryCriterion1 = (QueryCriterion) null;
      if (this.advFilter != null)
        queryCriterion1 = this.advFilter.CreateEvaluator().ToQueryCriterion();
      QueryCriterion queryCriterion2 = this.gvFilterManager.ToQueryCriterion();
      if (queryCriterion2 != null)
        queryCriterion1 = queryCriterion1 != null ? queryCriterion1.And(queryCriterion2) : queryCriterion2;
      return new QueryCriterion[1]{ queryCriterion1 };
    }

    private void populateContacts(QueryCriterion[] advCri)
    {
      this.populateContacts(advCri, (SortField[]) null);
    }

    private void populateContacts(QueryCriterion[] advCri, SortField[] sortFields)
    {
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.contactCursor != null)
          this.contactCursor.Dispose();
        if (sortFields == null)
          sortFields = this.generateSortFields();
        this.buildQuerySummary(true);
        this.contactCursor = advCri == null || advCri.Length == 0 || advCri[0] == null || !(advCri[0].ToSQLClause() != "") ? this.session.ContactManager.OpenBizPartnerCursor(new QueryCriterion[0], RelatedLoanMatchType.None, sortFields, this.generateFieldList(), true) : this.session.ContactManager.OpenBizPartnerCursor(advCri, RelatedLoanMatchType.None, sortFields, this.generateFieldList(), true);
        this.navContacts.NumberOfItems = this.contactCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Contacts: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private GVItem getGVItemForContact(int contactID)
    {
      GVItem gvItem = new GVItem();
      using (ICursor cursor = this.session.ContactManager.OpenBizPartnerCursor(new QueryCriterion[1]
      {
        (QueryCriterion) new OrdinalValueCriterion("Contact.contactID", (object) contactID, OrdinalMatchType.Equals)
      }, RelatedLoanMatchType.None, (SortField[]) null, this.generateFieldList(), true))
        return this.createGVItem(cursor.GetItem(0) as BizPartnerSummaryInfo);
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

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(column.ColumnID);
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

    private TableLayout getDefaultTableLayout()
    {
      TableLayout defaultTableLayout = new TableLayout();
      defaultTableLayout.AddColumn(new TableLayout.Column("ContactCategory.CategoryName", "Category", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.CompanyName", "Company", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.FirstName", "First Name", HorizontalAlignment.Left, 95));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.LastName", "Last Name", HorizontalAlignment.Left, 90));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.WorkPhone", "Work Phone", HorizontalAlignment.Left, 91));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.BizState", "State", HorizontalAlignment.Left, 91));
      return defaultTableLayout;
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
        string columnId = ((TableLayout.Column) this.gvContactList.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && contactInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) contactInfo, (EventHandler) null);
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private int getSelectedItemsCount() => this.gvContactList.SelectedItems.Count;

    public void RefreshList()
    {
      this.populateContacts(this.generateCriterion());
      if (this.gvContactList.SelectedItems.Count != 0 || this.gvContactList.Items.Count <= 0)
        return;
      this.gvContactList.Items[0].Selected = true;
    }

    private void gvContactList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        if (this.isDirty && this.siBtnSave.Visible)
        {
          if (DialogResult.OK == Utils.Dialog((IWin32Window) this, "Do you want to save your changes before selecting another contact?"))
            this.saveContact(false);
          this.isDirty = false;
        }
        this.siBtnSave.Enabled = false;
        this.isEditable(false);
        this.bizObj = (BizPartnerInfo) null;
        this.btnGoto.Enabled = false;
        this.btnDuplicate.Enabled = false;
        this.btnDelete.Enabled = false;
        this.bizObj = new BizPartnerInfo();
        this.cmbBoxCategoryID.SelectedIndex = -1;
        this.populateBizContacts();
        this.isDirty = false;
      }
      else
      {
        this.btnDuplicate.Enabled = true;
        this.btnDelete.Enabled = true;
        if (this.isDirty && this.siBtnSave.Visible)
        {
          if (DialogResult.OK == Utils.Dialog((IWin32Window) this, "Do you want to save your changes before selecting another contact?"))
            this.saveContact(false);
          this.isDirty = false;
        }
        if (this.gvContactList.SelectedItems.Count > 1 && this.multiSelect && this.forExternal)
        {
          this.bizObj = new BizPartnerInfo();
          this.cmbBoxCategoryID.SelectedIndex = -1;
        }
        else
        {
          this.bizObj = this.session.ContactManager.GetBizPartner(((BizPartnerSummaryInfo) this.gvContactList.SelectedItems[0].Tag).ContactID);
          this.cmbBoxCategoryID.SelectedIndexChanged -= new EventHandler(this.cmbBoxCategoryID_SelectedIndexChanged);
          this.cmbBoxCategoryID.Text = this.catUtil.CategoryIdToName(this.bizObj.CategoryID);
          this.cmbBoxCategoryID.SelectedIndexChanged += new EventHandler(this.cmbBoxCategoryID_SelectedIndexChanged);
        }
        this.populateBizContacts();
        this.contactSelected();
        this.enforceSecurity();
        this.isDirty = false;
        this.btnGoto.Enabled = true;
      }
    }

    private void populateBizContacts()
    {
      this.txtBoxFirstName.Text = this.bizObj.FirstName;
      this.txtBoxLastName.Text = this.bizObj.LastName;
      this.txtBoxSalutation.Text = this.bizObj.Salutation;
      this.txtBoxHomePhone.Text = this.bizObj.HomePhone;
      this.txtBoxWorkPhone.Text = this.bizObj.WorkPhone;
      this.txtBoxMobilePhone.Text = this.bizObj.MobilePhone;
      this.txtBoxFaxNumber.Text = this.bizObj.FaxNumber;
      this.txtBoxPersonalEmail.Text = this.bizObj.PersonalEmail;
      this.txtBoxBizEmail.Text = this.bizObj.BizEmail;
      this.chkBoxNoSpam.Checked = this.bizObj.NoSpam;
      this.txtBoxCompanyName.Text = this.bizObj.CompanyName;
      this.txtBoxJobTitle.Text = this.bizObj.JobTitle;
      this.txtBoxLicenseNumber.Text = this.bizObj.LicenseNumber;
      this.txtBoxFees.Text = this.bizObj.Fees >= 0 ? this.bizObj.Fees.ToString() : "";
      if (this.bizObj.BizAddress != null)
      {
        this.txtBoxBizAddress1.Text = this.bizObj.BizAddress.Street1;
        this.txtBoxBizAddress2.Text = this.bizObj.BizAddress.Street2;
        this.txtBoxBizCity.Text = this.bizObj.BizAddress.City;
        this.txtBoxBizState.Text = this.bizObj.BizAddress.State;
        this.txtBoxBizZip.Text = this.bizObj.BizAddress.Zip;
      }
      if (this.bizObj.BizContactLicense != null)
      {
        this.txtBoxBizInfoLicAuthName.Text = this.bizObj.BizContactLicense.LicenseAuthName;
        if (this.bizObj.BizContactLicense.LicenseAuthType == "")
          this.comboBoxBizInfoLicAuthType.SelectedIndex = -1;
        else
          this.comboBoxBizInfoLicAuthType.Text = this.bizObj.BizContactLicense.LicenseAuthType;
        this.dpBizLicIssueDate.Value = !(this.bizObj.BizContactLicense.LicenseIssueDate != DateTime.MinValue) ? new DateTime() : this.bizObj.BizContactLicense.LicenseIssueDate;
        this.txtBoxLicenseNumber.Text = this.bizObj.BizContactLicense.LicenseNumber;
        if (this.bizObj.BizContactLicense.LicenseStateCode == "")
          this.comboBoxBizInfoLicState.SelectedIndex = -1;
        else
          this.comboBoxBizInfoLicState.Text = this.bizObj.BizContactLicense.LicenseStateCode;
      }
      if (this.bizObj.PersonalInfoLicense != null)
      {
        this.txtBoxPersonalInfoLicAuthNAme.Text = this.bizObj.PersonalInfoLicense.LicenseAuthName;
        if (this.bizObj.PersonalInfoLicense.LicenseAuthType == "")
          this.comboBoxPersonalInfoLicAuthType.SelectedIndex = -1;
        else
          this.comboBoxPersonalInfoLicAuthType.Text = this.bizObj.PersonalInfoLicense.LicenseAuthType;
        this.dpPersonalInfoLicIssueDate.Value = !(this.bizObj.PersonalInfoLicense.LicenseIssueDate != DateTime.MinValue) ? new DateTime() : this.bizObj.PersonalInfoLicense.LicenseIssueDate;
        this.txtBoxPersonalInfoLicNo.Text = this.bizObj.PersonalInfoLicense.LicenseNumber;
        if (this.bizObj.PersonalInfoLicense.LicenseStateCode == "")
          this.comboBoxPersonalInfoLicState.SelectedIndex = -1;
        else
          this.comboBoxPersonalInfoLicState.Text = this.bizObj.PersonalInfoLicense.LicenseStateCode;
      }
      this.txtBoxBizWebUrl.Text = this.bizObj.BizWebUrl;
      if (this.bizObj.ContactGuid.ToString() == this.currentAssContactGuid && !this.forExternal)
        this.picLinked.Visible = true;
      else
        this.picLinked.Visible = false;
      this.chkPublic.Checked = this.bizObj.AccessLevel == ContactAccess.Public;
      this.ctlCategoryStandardFields.CurrentContactID = this.bizObj.ContactID;
    }

    private void enforceSecurity()
    {
      if (this.currentMode != RxBusinessContact.ActionMode.ManageMode)
        this.isEditable(false);
      else if (this.session.UserInfo.IsSuperAdministrator())
        this.isEditable(true);
      else if (this.bizObj.OwnerID == this.session.UserID)
        this.isEditable(true);
      else if (this.session.AclGroupManager.GetBizContactAccessRight(this.session.UserInfo, this.bizObj.ContactID) == AclTriState.True)
        this.isEditable(true);
      else
        this.isEditable(false);
    }

    private void addExternalCompany(BizPartnerInfo contact)
    {
      string hierarchyPath = this.hierarchyPath + "\\" + contact.CompanyName;
      int depth = this.depth + 1;
      if (this.session.ConfigurationManager.GetOidByBusinessId(this.forExternalLender, contact.ContactID) == 0)
      {
        string address = contact.BizAddress != null ? (contact.BizAddress.Street1 != "" ? contact.BizAddress.Street1 + " " : "") + contact.BizAddress.Street2 : "";
        this.hierarchyNodes.Add(this.session.ConfigurationManager.AddBusinessContact(this.forExternalLender, contact.ContactID, contact.CompanyName, contact.CompanyName, address, contact.BizAddress.City, contact.BizAddress.State, contact.BizAddress.Zip, this.parent, depth, hierarchyPath), contact.CompanyName);
      }
      else
        this.alreadyExists.Add(string.Concat((object) contact.ContactID), contact.CompanyName);
    }

    public event EventHandler AlreadyExistsOnSelect;

    private void btnLink_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a contact.");
      }
      else
      {
        if (this.forExternal)
        {
          foreach (GVItem selectedItem in this.gvContactList.SelectedItems)
          {
            this.bizObj = this.session.ContactManager.GetBizPartner(((BizPartnerSummaryInfo) selectedItem.Tag).ContactID);
            if (this.bizObj.CompanyName == string.Empty)
            {
              int num2 = (int) Utils.Dialog((IWin32Window) this, "You cannot add a contact (id: " + (object) this.bizObj.ContactID + ") without company name.");
            }
            else if (this.hierarchyPath.Contains(this.bizObj.CompanyName))
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "You cannot add an existing contact to its descendant contacts.");
            }
            else
              this.addExternalCompany(this.bizObj);
          }
          if (this.alreadyExists.Count > 0 && this.AlreadyExistsOnSelect != null)
            this.AlreadyExistsOnSelect((object) this.alreadyExists, EventArgs.Empty);
          this.DialogResult = DialogResult.OK;
        }
        if (this.chkLink.Visible && this.chkLink.Checked)
          this.session.LoanData.GetLogList().AddCRMMapping(this.assMappingID, CRMLogType.BusinessContact, this.bizObj.ContactGuid.ToString(), this.crmRoleType);
        this.isDirty = false;
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnNewLink_Click(object sender, EventArgs e)
    {
      this.bizObj = this.session.ContactManager.GetBizPartner(this.createContact());
      this.session.LoanData.GetLogList().AddCRMMapping(this.assMappingID, CRMLogType.BusinessContact, this.bizObj.ContactGuid.ToString(), this.crmRoleType);
      this.isDirty = false;
      this.DialogResult = DialogResult.OK;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.currentView.Filter.Clear();
      this.gvFilterManager.ClearColumnFilters();
      this.populateContacts(this.generateCriterion());
    }

    private void contactSelected()
    {
      if (this.forEPass)
      {
        this.selectedXML = "";
        if (this.gvContactList.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement xmlElement1;
        if (this.multiSelect)
        {
          xmlElement1 = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("SelectedAppraisers"));
          for (int index = 0; index < this.gvContactList.SelectedItems.Count; ++index)
          {
            BizPartnerSummaryInfo tag = (BizPartnerSummaryInfo) this.gvContactList.SelectedItems[index].Tag;
            if (tag != null)
            {
              XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Appraiser"));
              xmlElement2.SetAttribute("ContactID", tag.ContactID.ToString());
              xmlElement2.SetAttribute("Name", tag.FirstName + " " + tag.LastName);
              xmlElement2.SetAttribute("Email", tag.BizEmail);
              xmlElement2.SetAttribute("Phone", tag.WorkPhone);
            }
          }
        }
        else
        {
          xmlElement1 = (XmlElement) xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("SelectedRealtors"));
          try
          {
            XmlElement xmlElement3 = (XmlElement) xmlElement1.AppendChild((XmlNode) xmlDocument.CreateElement("Realtor"));
            xmlElement3.SetAttribute("ContactID", this.bizObj.ContactID.ToString());
            xmlElement3.SetAttribute("Name", this.bizObj.FullName);
            xmlElement3.SetAttribute("HomePhone", this.bizObj.HomePhone);
            xmlElement3.SetAttribute("WorkPhone", this.bizObj.WorkPhone);
            xmlElement3.SetAttribute("MobilePhone", this.bizObj.MobilePhone);
            xmlElement3.SetAttribute("BizEmail", this.bizObj.BizEmail);
            xmlElement3.SetAttribute("PersonalEmail", this.bizObj.PersonalEmail);
            xmlElement3.SetAttribute("CategoryName", this.catUtil.CategoryIdToName(this.bizObj.CategoryID));
          }
          catch (Exception ex)
          {
          }
        }
        this.selectedXML = xmlElement1.OuterXml;
      }
      this.prepareRxContactInfo(true);
    }

    private void prepareRxContactInfo(bool bSelectContact)
    {
      if (!bSelectContact || this.bizObj == null)
        return;
      BizPartnerInfo bizPartner = this.session.ContactManager.GetBizPartner(this.bizObj.ContactID);
      if (bizPartner == null)
        return;
      this.contactInfo = new RxContactInfo();
      this.contactInfo.ContactID = bizPartner.ContactID;
      this.contactInfo.FirstName = bizPartner.FirstName;
      this.contactInfo.LastName = bizPartner.LastName;
      this.contactInfo.CompanyName = bizPartner.CompanyName;
      this.contactInfo.BizAddress1 = bizPartner.BizAddress.Street1;
      this.contactInfo.BizAddress2 = bizPartner.BizAddress.Street2;
      this.contactInfo.BizCity = bizPartner.BizAddress.City;
      this.contactInfo.BizState = bizPartner.BizAddress.State;
      this.contactInfo.BizZip = bizPartner.BizAddress.Zip;
      this.contactInfo.BizEmail = bizPartner.BizEmail;
      this.contactInfo.WorkPhone = bizPartner.WorkPhone;
      this.contactInfo.FaxNumber = bizPartner.FaxNumber;
      this.contactInfo.CellPhone = bizPartner.MobilePhone;
      this.contactInfo.LicenseNumber = bizPartner.LicenseNumber;
      this.contactInfo.WebSite = bizPartner.BizWebUrl;
      this.contactInfo.ContactGuid = bizPartner.ContactGuid;
      this.contactInfo.HomePhone = bizPartner.HomePhone;
      this.contactInfo.CompanyLicAuthName = bizPartner.BizContactLicense.LicenseAuthName;
      this.contactInfo.CompanyLicAuthStateCode = bizPartner.BizContactLicense.LicenseStateCode;
      this.contactInfo.CompanyLicAuthType = bizPartner.BizContactLicense.LicenseAuthType;
      this.contactInfo.CompanyLicIssDate = bizPartner.BizContactLicense.LicenseIssueDate == DateTime.MinValue ? "" : Convert.ToString(bizPartner.BizContactLicense.LicenseIssueDate);
      this.contactInfo.ContactLicAuthName = bizPartner.PersonalInfoLicense.LicenseAuthName;
      this.contactInfo.ContactLicAuthStateCode = bizPartner.PersonalInfoLicense.LicenseStateCode;
      this.contactInfo.ContactLicAuthType = bizPartner.PersonalInfoLicense.LicenseAuthType;
      this.contactInfo.ContactLicIssDate = bizPartner.PersonalInfoLicense.LicenseIssueDate == DateTime.MinValue ? "" : Convert.ToString(bizPartner.PersonalInfoLicense.LicenseIssueDate);
      this.contactInfo.ContactLicNo = bizPartner.PersonalInfoLicense.LicenseNumber;
      this.contactInfo.CategoryID = bizPartner.CategoryID;
      this.contactInfo.CategoryName = this.catUtil.CategoryIdToName(bizPartner.CategoryID);
      this.contactInfo.Aba = this.ctlCategoryStandardFields.GetCustomFieldValue("ABA Number");
      this.contactInfo.AccountName = this.ctlCategoryStandardFields.GetCustomFieldValue("Account Number");
      this.contactInfo.MortgageeClauseCompany = this.ctlCategoryStandardFields.GetCustomFieldValue("Company Name");
      this.contactInfo.MortgageeClauseName = this.ctlCategoryStandardFields.GetCustomFieldValue("Contact Name");
      this.contactInfo.MortgageeClauseAddressLine = this.ctlCategoryStandardFields.GetCustomFieldValue("Address");
      this.contactInfo.MortgageeClauseCity = this.ctlCategoryStandardFields.GetCustomFieldValue("City");
      this.contactInfo.MortgageeClauseState = this.ctlCategoryStandardFields.GetCustomFieldValue("State");
      this.contactInfo.MortgageeClauseZipCode = this.ctlCategoryStandardFields.GetCustomFieldValue("Zip");
      this.contactInfo.MortgageeClausePhone = this.ctlCategoryStandardFields.GetCustomFieldValue("Contact Phone");
      this.contactInfo.MortgageeClauseFax = this.ctlCategoryStandardFields.GetCustomFieldValue("Contact Fax");
      this.contactInfo.MortgageeClauseText = this.ctlCategoryStandardFields.GetCustomFieldValue("Mortgagee Clause");
      this.contactInfo.MortgageeClauseLocationCode = this.ctlCategoryStandardFields.GetCustomFieldValue("Location Code");
      this.contactInfo.MortgageeClauseInvestorCode = this.ctlCategoryStandardFields.GetCustomFieldValue("Investor Code");
      this.contactInfo.OrganizationID = this.ctlCategoryStandardFields.GetCustomFieldValue("Organization ID");
    }

    private int createContact()
    {
      if (this.rxContact == (RxContactInfo) null)
        return -1;
      BizPartnerInfo info = new BizPartnerInfo();
      info.BizAddress.Street1 = this.rxContact.BizAddress1;
      info.BizAddress.Street2 = this.rxContact.BizAddress2;
      info.BizAddress.City = this.rxContact.BizCity;
      info.BizAddress.State = this.rxContact.BizState;
      info.BizAddress.Zip = this.rxContact.BizZip;
      info.BizEmail = this.rxContact.BizEmail;
      info.CategoryID = this.rxContact.CategoryID <= 0 ? (!(this.category != "") || this.catUtil.CategoryNameToId(this.category) <= -1 ? this.catUtil.CategoryNameToId("No Category") : this.catUtil.CategoryNameToId(this.category)) : this.rxContact.CategoryID;
      info.CompanyName = this.rxContact.CompanyName;
      info.FaxNumber = this.rxContact.FaxNumber;
      info.MobilePhone = this.rxContact.CellPhone;
      info.FirstName = this.rxContact.FirstName;
      info.LastName = this.rxContact.LastName;
      info.BizWebUrl = this.rxContact.WebSite;
      info.WorkPhone = this.rxContact.WorkPhone;
      info.AccessLevel = ContactAccess.Private;
      info.OwnerID = this.session.UserID;
      info.PersonalInfoLicense = new BizContactLicenseInfo(this.rxContact.ContactLicNo, this.rxContact.ContactLicAuthName, this.rxContact.ContactLicAuthType, this.rxContact.ContactLicAuthStateCode, !string.IsNullOrEmpty(this.rxContact.ContactLicIssDate) ? Utils.ParseDate((object) this.rxContact.ContactLicIssDate) : DateTime.MinValue);
      info.BizContactLicense = new BizContactLicenseInfo(this.rxContact.LicenseNumber, this.rxContact.CompanyLicAuthName, this.rxContact.CompanyLicAuthType, this.rxContact.CompanyLicAuthStateCode, !string.IsNullOrEmpty(this.rxContact.CompanyLicIssDate) ? Utils.ParseDate((object) this.rxContact.CompanyLicIssDate) : DateTime.MinValue);
      int bizPartner = this.session.ContactManager.CreateBizPartner(info);
      this.rxContact.ContactID = bizPartner;
      this.rxContact.ContactGuid = info.ContactGuid;
      this.ctlCategoryStandardFields.CurrentContactID = bizPartner;
      this.ctlCategoryStandardFields.UpdateMortgageClauseFields(this.rxContact);
      this.ctlCategoryStandardFields.setDirty = true;
      this.ctlCategoryStandardFields.SaveChanges();
      return bizPartner;
    }

    public RxContactInfo RxContactRecord => this.contactInfo;

    private void btnSelect_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact.");
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    public string SelectedXML => this.selectedXML;

    private void btnGoto_Click(object sender, EventArgs e)
    {
      this.goToContact = true;
      this.selectedContact = new ContactInfo(string.Concat((object) this.bizObj.ContactID), CategoryType.BizPartner);
      this.DialogResult = DialogResult.OK;
    }

    public bool GoToContact => this.goToContact;

    public ContactInfo SelectedContactInfo => this.selectedContact;

    public BizPartnerInfo SelectedPartnerInfo => this.bizObj;

    private void isEditable(bool editable)
    {
      this.txtBoxFirstName.ReadOnly = !editable;
      this.txtBoxLastName.ReadOnly = !editable;
      this.txtBoxSalutation.ReadOnly = !editable;
      this.txtBoxHomePhone.ReadOnly = !editable;
      this.txtBoxWorkPhone.ReadOnly = !editable;
      this.txtBoxMobilePhone.ReadOnly = !editable;
      this.txtBoxFaxNumber.ReadOnly = !editable;
      this.txtBoxPersonalEmail.ReadOnly = !editable;
      this.txtBoxBizEmail.ReadOnly = !editable;
      this.chkBoxNoSpam.Enabled = editable;
      this.cmbBoxCategoryID.Enabled = editable;
      this.txtBoxCompanyName.ReadOnly = !editable;
      this.txtBoxJobTitle.ReadOnly = !editable;
      this.txtBoxLicenseNumber.ReadOnly = !editable;
      this.txtBoxFees.ReadOnly = !editable;
      this.txtBoxBizAddress1.ReadOnly = !editable;
      this.txtBoxBizAddress2.ReadOnly = !editable;
      this.txtBoxBizCity.ReadOnly = !editable;
      this.txtBoxBizState.ReadOnly = !editable;
      this.txtBoxBizZip.ReadOnly = !editable;
      this.txtBoxBizWebUrl.ReadOnly = !editable;
      this.ctlCategoryStandardFields.IsReadOnly = !editable;
      this.comboBoxBizInfoLicAuthType.Enabled = editable;
      this.txtBoxBizInfoLicAuthName.ReadOnly = !editable;
      this.comboBoxBizInfoLicState.Enabled = editable;
      this.dpPersonalInfoLicIssueDate.Enabled = editable;
      this.txtBoxPersonalInfoLicAuthNAme.ReadOnly = !editable;
      this.comboBoxPersonalInfoLicAuthType.Enabled = editable;
      this.dpBizLicIssueDate.Enabled = editable;
      this.txtBoxPersonalInfoLicNo.ReadOnly = !editable;
      this.comboBoxPersonalInfoLicState.Enabled = editable;
    }

    private void createNew(BizPartnerInfo sourceObj)
    {
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        BizPartnerInfo info = new BizPartnerInfo();
        if (sourceObj != null)
        {
          info.CompanyName = sourceObj.CompanyName;
          info.CategoryID = sourceObj.CategoryID;
          info.AccessLevel = ContactAccess.Private;
          info.OwnerID = this.session.UserID;
          info.BizAddress.City = sourceObj.BizAddress.City;
          info.BizAddress.State = sourceObj.BizAddress.State;
          info.BizAddress.Street1 = sourceObj.BizAddress.Street1;
          info.BizAddress.Street2 = sourceObj.BizAddress.Street2;
          info.BizAddress.Zip = sourceObj.BizAddress.Zip;
          info.BizEmail = sourceObj.BizEmail;
          info.BizWebUrl = sourceObj.BizWebUrl;
          info.FaxNumber = sourceObj.FaxNumber;
          info.Fees = sourceObj.Fees;
          info.FirstName = sourceObj.FirstName;
          info.HomePhone = sourceObj.HomePhone;
          info.JobTitle = sourceObj.JobTitle;
          info.LastName = sourceObj.LastName;
          info.LicenseNumber = sourceObj.LicenseNumber;
          info.MobilePhone = sourceObj.MobilePhone;
          info.NoSpam = sourceObj.NoSpam;
          info.PersonalEmail = sourceObj.PersonalEmail;
          info.PrimaryEmail = sourceObj.PrimaryEmail;
          info.PrimaryPhone = sourceObj.PrimaryPhone;
          info.Salutation = sourceObj.Salutation;
          info.WorkPhone = sourceObj.WorkPhone;
          info.BizContactLicense.LicenseAuthName = sourceObj.BizContactLicense.LicenseAuthName;
          info.BizContactLicense.LicenseAuthType = sourceObj.BizContactLicense.LicenseAuthType;
          info.BizContactLicense.LicenseIssueDate = sourceObj.BizContactLicense.LicenseIssueDate;
          info.BizContactLicense.LicenseStateCode = sourceObj.BizContactLicense.LicenseStateCode;
          info.BizContactLicense.LicenseNumber = sourceObj.BizContactLicense.LicenseNumber;
          info.PersonalInfoLicense.LicenseAuthName = sourceObj.PersonalInfoLicense.LicenseAuthName;
          info.PersonalInfoLicense.LicenseAuthType = sourceObj.PersonalInfoLicense.LicenseAuthType;
          info.PersonalInfoLicense.LicenseIssueDate = sourceObj.PersonalInfoLicense.LicenseIssueDate;
          info.PersonalInfoLicense.LicenseStateCode = sourceObj.PersonalInfoLicense.LicenseStateCode;
          info.PersonalInfoLicense.LicenseNumber = sourceObj.PersonalInfoLicense.LicenseNumber;
        }
        else
        {
          info.CompanyName = "<New Contact>";
          int id = this.catUtil.CategoryNameToId("No Category");
          if (id >= 0)
            info.CategoryID = id;
          info.AccessLevel = ContactAccess.Private;
          info.OwnerID = this.session.UserID;
        }
        this.bizObj = this.session.ContactManager.GetBizPartner(this.session.ContactManager.CreateBizPartner(info, DateTime.Now, ContactSource.Entered));
        if (sourceObj != null)
        {
          CustomFieldValueCollection fieldValueCollection1 = CustomFieldValueCollection.GetCustomFieldValueCollection(this.session.SessionObjects, new CustomFieldValueCollection.Criteria(sourceObj.ContactID, sourceObj.CategoryID));
          if (fieldValueCollection1 != null && fieldValueCollection1.Count > 0)
          {
            CustomFieldValueCollection fieldValueCollection2 = CustomFieldValueCollection.GetCustomFieldValueCollection(this.session.SessionObjects, new CustomFieldValueCollection.Criteria(this.bizObj.ContactID, this.bizObj.CategoryID));
            foreach (CustomFieldValue customFieldValue1 in (CollectionBase) fieldValueCollection1)
            {
              CustomFieldValue customFieldValue2 = CustomFieldValue.NewCustomFieldValue(customFieldValue1.FieldId, this.bizObj.ContactID, customFieldValue1.FieldFormat);
              customFieldValue2.FieldValue = customFieldValue1.FieldValue;
              fieldValueCollection2.Add(customFieldValue2);
            }
            fieldValueCollection2.Save();
          }
        }
        GVItem gvItem = this.createGVItem(new BizPartnerSummaryInfo(this.bizObj));
        this.gvContactList.SelectedItems.Clear();
        this.gvContactList.Items.Add(gvItem);
        gvItem.Selected = true;
        this.gvContactList.EnsureVisible(gvItem.Index);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void btnNew_Click(object sender, EventArgs e) => this.createNew((BizPartnerInfo) null);

    private void btnDuplicate_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact to dulicate.");
      }
      else
      {
        if (this.bizObj == null)
          return;
        this.createNew(this.bizObj);
      }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a contact to delete.");
      }
      else
      {
        if (this.bizObj == null)
          return;
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete contact '" + this.bizObj.LastName + ", " + this.bizObj.FirstName + "'?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          return;
        this.session.ContactManager.DeleteBizPartner(this.bizObj.ContactID);
        this.RefreshList();
      }
    }

    private void siBtnSave_Click(object sender, EventArgs e) => this.saveContact(true);

    private void saveContact(bool keepSelection)
    {
      this.bizObj.FirstName = this.txtBoxFirstName.Text;
      this.bizObj.LastName = this.txtBoxLastName.Text;
      this.bizObj.Salutation = this.txtBoxSalutation.Text;
      this.bizObj.HomePhone = this.txtBoxHomePhone.Text;
      this.bizObj.WorkPhone = this.txtBoxWorkPhone.Text;
      this.bizObj.MobilePhone = this.txtBoxMobilePhone.Text;
      this.bizObj.FaxNumber = this.txtBoxFaxNumber.Text;
      this.bizObj.PersonalEmail = this.txtBoxPersonalEmail.Text;
      this.bizObj.BizEmail = this.txtBoxBizEmail.Text;
      this.bizObj.NoSpam = this.chkBoxNoSpam.Checked;
      this.bizObj.CompanyName = this.txtBoxCompanyName.Text;
      this.bizObj.JobTitle = this.txtBoxJobTitle.Text;
      this.bizObj.LicenseNumber = this.txtBoxLicenseNumber.Text;
      this.bizObj.Fees = Utils.ParseInt((object) this.txtBoxFees.Text);
      if (this.bizObj.BizAddress != null)
      {
        this.bizObj.BizAddress.Street1 = this.txtBoxBizAddress1.Text;
        this.bizObj.BizAddress.Street2 = this.txtBoxBizAddress2.Text;
        this.bizObj.BizAddress.City = this.txtBoxBizCity.Text;
        this.bizObj.BizAddress.State = this.txtBoxBizState.Text;
        this.bizObj.BizAddress.Zip = this.txtBoxBizZip.Text ?? "";
      }
      if (this.bizObj.BizContactLicense != null)
      {
        this.bizObj.BizContactLicense.LicenseAuthName = this.txtBoxBizInfoLicAuthName.Text;
        this.bizObj.BizContactLicense.LicenseAuthType = this.comboBoxBizInfoLicAuthType.Text;
        this.bizObj.BizContactLicense.LicenseIssueDate = this.dpBizLicIssueDate.Value;
        this.bizObj.BizContactLicense.LicenseNumber = this.txtBoxLicenseNumber.Text;
        this.bizObj.BizContactLicense.LicenseStateCode = this.comboBoxBizInfoLicState.Text;
      }
      if (this.bizObj.PersonalInfoLicense != null)
      {
        this.bizObj.PersonalInfoLicense.LicenseAuthName = this.txtBoxPersonalInfoLicAuthNAme.Text;
        this.bizObj.PersonalInfoLicense.LicenseAuthType = this.comboBoxPersonalInfoLicAuthType.Text;
        this.bizObj.PersonalInfoLicense.LicenseIssueDate = this.dpPersonalInfoLicIssueDate.Value;
        this.bizObj.PersonalInfoLicense.LicenseNumber = this.txtBoxPersonalInfoLicNo.Text;
        this.bizObj.PersonalInfoLicense.LicenseStateCode = this.comboBoxPersonalInfoLicState.Text;
      }
      this.bizObj.BizWebUrl = this.txtBoxBizWebUrl.Text;
      this.bizObj.CategoryID = ((BizCategory) this.cmbBoxCategoryID.SelectedItem).CategoryID;
      this.session.ContactManager.UpdateBizPartner(this.bizObj);
      this.ctlCategoryStandardFields.SaveChanges();
      this.isDirty = false;
      this.bizObj = this.session.ContactManager.GetBizPartner(this.bizObj.ContactID);
      this.refreshContactInList(this.bizObj.ContactID, keepSelection);
    }

    private void refreshContactInList(int contactID, bool makeSelect)
    {
      int num = 0;
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContactList.Items)
      {
        if (((BizPartnerSummaryInfo) gvItem.Tag).ContactID == contactID)
        {
          num = gvItem.Index;
          break;
        }
      }
      GVItem gvItemForContact = this.getGVItemForContact(contactID);
      this.gvContactList.Items.RemoveAt(num);
      this.gvContactList.Items.Insert(num, gvItemForContact);
      if (!makeSelect)
        return;
      this.gvContactList.Items[num].Selected = makeSelect;
    }

    private bool isDirty
    {
      get => this.siBtnSave.Enabled;
      set => this.siBtnSave.Enabled = value;
    }

    private void dataChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
      this.isDirty = true;
    }

    private void txtBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (this.initialLoad || e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    public void formatText(object sender, EventArgs e)
    {
      if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat dataFormat;
        if (sender == this.txtBoxHomePhone || sender == this.txtBoxFaxNumber || sender == this.txtBoxWorkPhone || sender == this.txtBoxMobilePhone)
          dataFormat = FieldFormat.PHONE;
        else if (sender == this.txtBoxBizZip)
        {
          dataFormat = FieldFormat.ZIPCODE;
        }
        else
        {
          if (sender != this.txtBoxBizState)
            return;
          dataFormat = FieldFormat.STATE;
        }
        TextBox textBox = (TextBox) sender;
        bool needsUpdate = false;
        int newCursorPos = 0;
        string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate, textBox.SelectionStart, ref newCursorPos);
        if (!needsUpdate)
          return;
        textBox.Text = str;
        textBox.SelectionStart = newCursorPos;
      }
    }

    private bool validateCategoryChange(out int categoryId)
    {
      categoryId = this.catUtil.CategoryNameToId(this.cmbBoxCategoryID.Text);
      if (-1 == categoryId)
      {
        this.catUtil = new BizCategoryUtil(this.session.SessionObjects);
        categoryId = this.catUtil.CategoryNameToId(this.cmbBoxCategoryID.Text);
      }
      return categoryId == this.contactInfo.CategoryID || 0 >= CustomFieldValueCollection.GetCustomFieldValueCollection(this.session.SessionObjects, new CustomFieldValueCollection.Criteria(this.contactInfo.ContactID, this.contactInfo.CategoryID)).Count || DialogResult.No != Utils.Dialog((IWin32Window) this, "Changing the category for this contact will result in the loss of category custom field values. Do you wish to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
    }

    private void RxBusinessContact_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!this.isDirty || Utils.Dialog((IWin32Window) this, "Do you want to save your changes to the currently selected contact first?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      this.saveContact(false);
    }

    private void cmbBoxCategoryID_SelectedIndexChanged(object sender, EventArgs e)
    {
      int categoryId = -1;
      if (this.contactInfo != (RxContactInfo) null)
      {
        this.dataChanged((object) null, (EventArgs) null);
        if (!this.validateCategoryChange(out categoryId))
          return;
      }
      if (categoryId <= -1)
        return;
      this.ctlCategoryStandardFields.ResetCategoryID(categoryId);
    }

    private void gvContactList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!this.btnLink.Visible)
        return;
      this.btnLink.PerformClick();
    }

    private void collapsibleSplitter1_SplitterMoved(object sender, SplitterEventArgs e)
    {
    }

    private void gvContactList_DoubleClick(object sender, EventArgs e)
    {
      this.btnLink_Click(sender, e);
    }

    private void gvContactList_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.suspendEvents)
        return;
      using (CursorActivator.Wait())
        this.populateContacts(this.generateCriterion(), this.getSortFieldsForColumnSort(e.ColumnSorts));
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
      BizPartnerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(colInfo.ColumnID);
      return fieldByCriterionName != null ? new SortField(fieldByCriterionName.SortTerm, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion) : (SortField) null;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RxBusinessContact));
      this.toolTip1 = new ToolTip(this.components);
      this.siBtnSave = new StandardIconButton();
      this.btnDelete = new StandardIconButton();
      this.btnDuplicate = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvContactList = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.gcDetail = new GroupContainer();
      this.flowLayoutPanel3 = new FlowLayoutPanel();
      this.verticalSeparator1 = new VerticalSeparator();
      this.btnGoto = new Button();
      this.tabControl1 = new TabControl();
      this.tpDetails = new TabPage();
      this.groupContainer3 = new GroupContainer();
      this.comboBoxBizInfoLicAuthType = new ComboBox();
      this.comboBoxBizInfoLicState = new ComboBox();
      this.dpBizLicIssueDate = new DatePicker();
      this.txtBoxBizInfoLicAuthName = new TextBox();
      this.lblLicenseNum = new Label();
      this.txtBoxLicenseNumber = new TextBox();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.txtBoxJobTitle = new TextBox();
      this.lblTitle = new Label();
      this.lblWebUrl = new Label();
      this.txtBoxBizWebUrl = new TextBox();
      this.txtBoxFees = new TextBox();
      this.lblFees = new Label();
      this.txtBoxBizState = new TextBox();
      this.txtBoxBizZip = new TextBox();
      this.lblBizZip = new Label();
      this.lblBizState = new Label();
      this.txtBoxBizCity = new TextBox();
      this.txtBoxBizAddress2 = new TextBox();
      this.lblBizCity = new Label();
      this.lblBizAddress2 = new Label();
      this.lblBizAddress1 = new Label();
      this.txtBoxBizAddress1 = new TextBox();
      this.cmbBoxCategoryID = new ComboBox();
      this.txtBoxCompanyName = new TextBox();
      this.lblCategory = new Label();
      this.lblCompany = new Label();
      this.groupContainer2 = new GroupContainer();
      this.comboBoxPersonalInfoLicAuthType = new ComboBox();
      this.comboBoxPersonalInfoLicState = new ComboBox();
      this.dpPersonalInfoLicIssueDate = new DatePicker();
      this.txtBoxPersonalInfoLicAuthNAme = new TextBox();
      this.txtBoxPersonalInfoLicNo = new TextBox();
      this.label7 = new Label();
      this.label6 = new Label();
      this.label5 = new Label();
      this.label3 = new Label();
      this.label1 = new Label();
      this.chkPublic = new CheckBox();
      this.txtBoxLastName = new TextBox();
      this.chkBoxNoSpam = new CheckBox();
      this.lblWorkEmail = new Label();
      this.txtBoxBizEmail = new TextBox();
      this.lblFaxNumber = new Label();
      this.txtBoxSalutation = new TextBox();
      this.lblCellPhone = new Label();
      this.label2 = new Label();
      this.txtBoxWorkPhone = new TextBox();
      this.txtBoxPersonalEmail = new TextBox();
      this.lblWorkPhone = new Label();
      this.txtBoxFaxNumber = new TextBox();
      this.lblHomeEmail = new Label();
      this.txtBoxMobilePhone = new TextBox();
      this.lblHomePhone = new Label();
      this.txtBoxHomePhone = new TextBox();
      this.txtBoxFirstName = new TextBox();
      this.lblFirstName = new Label();
      this.lblLastName = new Label();
      this.tabCategoryStandardFields = new TabPage();
      this.picLinked = new PictureBox();
      this.panel1 = new Panel();
      this.chkLink = new CheckBox();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.btnCancel = new Button();
      this.btnNewLink = new Button();
      this.btnLink = new Button();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.navContacts = new PageListNavigator();
      this.gradientPanel3 = new GradientPanel();
      this.lblFilter = new Label();
      this.btnClear = new Button();
      this.label4 = new Label();
      ((ISupportInitialize) this.siBtnSave).BeginInit();
      ((ISupportInitialize) this.btnDelete).BeginInit();
      ((ISupportInitialize) this.btnDuplicate).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.gcDetail.SuspendLayout();
      this.flowLayoutPanel3.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      ((ISupportInitialize) this.picLinked).BeginInit();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.SuspendLayout();
      this.siBtnSave.BackColor = Color.Transparent;
      this.siBtnSave.Enabled = false;
      this.siBtnSave.Location = new Point(110, 3);
      this.siBtnSave.Margin = new Padding(0, 3, 3, 3);
      this.siBtnSave.MouseDownImage = (Image) null;
      this.siBtnSave.Name = "siBtnSave";
      this.siBtnSave.Size = new Size(16, 17);
      this.siBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.siBtnSave.TabIndex = 2;
      this.siBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.siBtnSave, "Save Contact");
      this.siBtnSave.Click += new EventHandler(this.siBtnSave_Click);
      this.btnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDelete.BackColor = Color.Transparent;
      this.btnDelete.Enabled = false;
      this.btnDelete.Location = new Point(72, 3);
      this.btnDelete.MouseDownImage = (Image) null;
      this.btnDelete.Name = "btnDelete";
      this.btnDelete.Size = new Size(16, 17);
      this.btnDelete.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDelete.TabIndex = 29;
      this.btnDelete.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDelete, "Delete Contact");
      this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
      this.btnDuplicate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDuplicate.BackColor = Color.Transparent;
      this.btnDuplicate.Enabled = false;
      this.btnDuplicate.Location = new Point(51, 3);
      this.btnDuplicate.Margin = new Padding(2, 3, 2, 3);
      this.btnDuplicate.MouseDownImage = (Image) null;
      this.btnDuplicate.Name = "btnDuplicate";
      this.btnDuplicate.Size = new Size(16, 17);
      this.btnDuplicate.StandardButtonType = StandardIconButton.ButtonType.DuplicateButton;
      this.btnDuplicate.TabIndex = 30;
      this.btnDuplicate.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnDuplicate, "Duplicate Contact");
      this.btnDuplicate.Click += new EventHandler(this.btnDuplicate_Click);
      this.btnNew.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(30, 3);
      this.btnNew.Margin = new Padding(2, 3, 3, 3);
      this.btnNew.MouseDownImage = (Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 17);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 31;
      this.btnNew.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnNew, "New Contact");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.Controls.Add((Control) this.collapsibleSplitter1);
      this.groupContainer1.Controls.Add((Control) this.gcDetail);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.flowLayoutPanel1);
      this.groupContainer1.Controls.Add((Control) this.navContacts);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 30);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(664, 718);
      this.groupContainer1.TabIndex = 8;
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.gvContactList);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(662, 212);
      this.borderPanel1.TabIndex = 36;
      this.gvContactList.AllowColumnReorder = true;
      this.gvContactList.AllowMultiselect = false;
      this.gvContactList.BorderStyle = BorderStyle.None;
      this.gvContactList.Dock = DockStyle.Fill;
      this.gvContactList.FilterVisible = true;
      this.gvContactList.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvContactList.Location = new Point(0, 0);
      this.gvContactList.Name = "gvContactList";
      this.gvContactList.Size = new Size(662, 211);
      this.gvContactList.SortOption = GVSortOption.Owner;
      this.gvContactList.TabIndex = 28;
      this.gvContactList.SelectedIndexChanged += new EventHandler(this.gvContactList_SelectedIndexChanged);
      this.gvContactList.ItemDoubleClick += new GVItemEventHandler(this.gvContactList_ItemDoubleClick);
      this.gvContactList.DoubleClick += new EventHandler(this.gvContactList_DoubleClick);
      this.gvContactList.SortItems += new GVColumnSortEventHandler(this.gvContactList_SortItems);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.gcDetail;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 238);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 35;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.collapsibleSplitter1.SplitterMoved += new SplitterEventHandler(this.collapsibleSplitter1_SplitterMoved);
      this.gcDetail.Controls.Add((Control) this.flowLayoutPanel3);
      this.gcDetail.Controls.Add((Control) this.tabControl1);
      this.gcDetail.Controls.Add((Control) this.picLinked);
      this.gcDetail.Dock = DockStyle.Bottom;
      this.gcDetail.HeaderForeColor = SystemColors.ControlText;
      this.gcDetail.Location = new Point(1, 245);
      this.gcDetail.Name = "gcDetail";
      this.gcDetail.Padding = new Padding(2, 2, 0, 0);
      this.gcDetail.Size = new Size(662, 435);
      this.gcDetail.TabIndex = 34;
      this.gcDetail.Text = "Contact Details";
      this.flowLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel3.BackColor = Color.Transparent;
      this.flowLayoutPanel3.Controls.Add((Control) this.siBtnSave);
      this.flowLayoutPanel3.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel3.Controls.Add((Control) this.btnGoto);
      this.flowLayoutPanel3.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel3.Location = new Point(529, 2);
      this.flowLayoutPanel3.Name = "flowLayoutPanel3";
      this.flowLayoutPanel3.Padding = new Padding(0, 0, 5, 0);
      this.flowLayoutPanel3.Size = new Size(134, 22);
      this.flowLayoutPanel3.TabIndex = 3;
      this.verticalSeparator1.Location = new Point(105, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 7;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.btnGoto.BackColor = SystemColors.Control;
      this.btnGoto.Enabled = false;
      this.btnGoto.Location = new Point(14, 0);
      this.btnGoto.Margin = new Padding(0);
      this.btnGoto.Name = "btnGoto";
      this.btnGoto.Size = new Size(88, 22);
      this.btnGoto.TabIndex = 6;
      this.btnGoto.Text = "Go To Contact";
      this.btnGoto.UseVisualStyleBackColor = true;
      this.btnGoto.Click += new EventHandler(this.btnGoto_Click);
      this.tabControl1.Controls.Add((Control) this.tpDetails);
      this.tabControl1.Controls.Add((Control) this.tabCategoryStandardFields);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(3, 28);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.Padding = new Point(8, 3);
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(658, 406);
      this.tabControl1.TabIndex = 1;
      this.tpDetails.Controls.Add((Control) this.groupContainer3);
      this.tpDetails.Controls.Add((Control) this.groupContainer2);
      this.tpDetails.Location = new Point(4, 23);
      this.tpDetails.Margin = new Padding(0);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(650, 379);
      this.tpDetails.TabIndex = 0;
      this.tpDetails.Text = "Details";
      this.tpDetails.UseVisualStyleBackColor = true;
      this.groupContainer3.Controls.Add((Control) this.comboBoxBizInfoLicAuthType);
      this.groupContainer3.Controls.Add((Control) this.comboBoxBizInfoLicState);
      this.groupContainer3.Controls.Add((Control) this.dpBizLicIssueDate);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizInfoLicAuthName);
      this.groupContainer3.Controls.Add((Control) this.lblLicenseNum);
      this.groupContainer3.Controls.Add((Control) this.txtBoxLicenseNumber);
      this.groupContainer3.Controls.Add((Control) this.label8);
      this.groupContainer3.Controls.Add((Control) this.label9);
      this.groupContainer3.Controls.Add((Control) this.label10);
      this.groupContainer3.Controls.Add((Control) this.label11);
      this.groupContainer3.Controls.Add((Control) this.txtBoxJobTitle);
      this.groupContainer3.Controls.Add((Control) this.lblTitle);
      this.groupContainer3.Controls.Add((Control) this.lblWebUrl);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizWebUrl);
      this.groupContainer3.Controls.Add((Control) this.txtBoxFees);
      this.groupContainer3.Controls.Add((Control) this.lblFees);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizState);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizZip);
      this.groupContainer3.Controls.Add((Control) this.lblBizZip);
      this.groupContainer3.Controls.Add((Control) this.lblBizState);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizCity);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizAddress2);
      this.groupContainer3.Controls.Add((Control) this.lblBizCity);
      this.groupContainer3.Controls.Add((Control) this.lblBizAddress2);
      this.groupContainer3.Controls.Add((Control) this.lblBizAddress1);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizAddress1);
      this.groupContainer3.Controls.Add((Control) this.cmbBoxCategoryID);
      this.groupContainer3.Controls.Add((Control) this.txtBoxCompanyName);
      this.groupContainer3.Controls.Add((Control) this.lblCategory);
      this.groupContainer3.Controls.Add((Control) this.lblCompany);
      this.groupContainer3.Dock = DockStyle.Right;
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(326, 2);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(322, 375);
      this.groupContainer3.TabIndex = 77;
      this.groupContainer3.Text = "Business Information";
      this.comboBoxBizInfoLicAuthType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxBizInfoLicAuthType.Enabled = false;
      this.comboBoxBizInfoLicAuthType.FormattingEnabled = true;
      this.comboBoxBizInfoLicAuthType.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Private",
        (object) "Public Federal",
        (object) "Public Local",
        (object) "Public State"
      });
      this.comboBoxBizInfoLicAuthType.Location = new Point(143, 275);
      this.comboBoxBizInfoLicAuthType.Name = "comboBoxBizInfoLicAuthType";
      this.comboBoxBizInfoLicAuthType.Size = new Size(121, 22);
      this.comboBoxBizInfoLicAuthType.TabIndex = 109;
      this.comboBoxBizInfoLicAuthType.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.comboBoxBizInfoLicState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxBizInfoLicState.Enabled = false;
      this.comboBoxBizInfoLicState.FormattingEnabled = true;
      this.comboBoxBizInfoLicState.Location = new Point(143, 298);
      this.comboBoxBizInfoLicState.Name = "comboBoxBizInfoLicState";
      this.comboBoxBizInfoLicState.Size = new Size(67, 22);
      this.comboBoxBizInfoLicState.TabIndex = 106;
      this.comboBoxBizInfoLicState.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.dpBizLicIssueDate.BackColor = SystemColors.Window;
      this.dpBizLicIssueDate.Enabled = false;
      this.dpBizLicIssueDate.Location = new Point(143, 321);
      this.dpBizLicIssueDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpBizLicIssueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpBizLicIssueDate.Name = "dpBizLicIssueDate";
      this.dpBizLicIssueDate.Size = new Size(123, 22);
      this.dpBizLicIssueDate.TabIndex = 108;
      this.dpBizLicIssueDate.Tag = (object) "763";
      this.dpBizLicIssueDate.ToolTip = "";
      this.dpBizLicIssueDate.Value = new DateTime(0L);
      this.dpBizLicIssueDate.ValueChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizInfoLicAuthName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizInfoLicAuthName.Location = new Point(143, 254);
      this.txtBoxBizInfoLicAuthName.MaxLength = 50;
      this.txtBoxBizInfoLicAuthName.Name = "txtBoxBizInfoLicAuthName";
      this.txtBoxBizInfoLicAuthName.ReadOnly = true;
      this.txtBoxBizInfoLicAuthName.Size = new Size(160, 20);
      this.txtBoxBizInfoLicAuthName.TabIndex = 102;
      this.txtBoxBizInfoLicAuthName.TextChanged += new EventHandler(this.dataChanged);
      this.lblLicenseNum.AutoSize = true;
      this.lblLicenseNum.Location = new Point(7, 236);
      this.lblLicenseNum.Name = "lblLicenseNum";
      this.lblLicenseNum.Size = new Size(102, 14);
      this.lblLicenseNum.TabIndex = 99;
      this.lblLicenseNum.Text = "Company License #";
      this.txtBoxLicenseNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxLicenseNumber.Location = new Point(143, 232);
      this.txtBoxLicenseNumber.MaxLength = 50;
      this.txtBoxLicenseNumber.Name = "txtBoxLicenseNumber";
      this.txtBoxLicenseNumber.ReadOnly = true;
      this.txtBoxLicenseNumber.Size = new Size(160, 20);
      this.txtBoxLicenseNumber.TabIndex = 100;
      this.txtBoxLicenseNumber.TextChanged += new EventHandler(this.dataChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(7, 324);
      this.label8.Name = "label8";
      this.label8.Size = new Size(78, 14);
      this.label8.TabIndex = 98;
      this.label8.Text = "Lic. Issue Date";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(7, 302);
      this.label9.Name = "label9";
      this.label9.Size = new Size(126, 14);
      this.label9.TabIndex = 97;
      this.label9.Text = "Lic. Authority State Code";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(7, 280);
      this.label10.Name = "label10";
      this.label10.Size = new Size(96, 14);
      this.label10.TabIndex = 96;
      this.label10.Text = "Lic. Authority Type";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 258);
      this.label11.Name = "label11";
      this.label11.Size = new Size(137, 14);
      this.label11.TabIndex = 95;
      this.label11.Text = "Lic. Issuing Authority Name";
      this.txtBoxJobTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxJobTitle.Location = new Point(143, 211);
      this.txtBoxJobTitle.MaxLength = 50;
      this.txtBoxJobTitle.Name = "txtBoxJobTitle";
      this.txtBoxJobTitle.ReadOnly = true;
      this.txtBoxJobTitle.Size = new Size(160, 20);
      this.txtBoxJobTitle.TabIndex = 98;
      this.txtBoxJobTitle.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxJobTitle.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(7, 214);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(46, 14);
      this.lblTitle.TabIndex = 92;
      this.lblTitle.Text = "Job Title";
      this.lblWebUrl.AutoSize = true;
      this.lblWebUrl.Location = new Point(7, 192);
      this.lblWebUrl.Name = "lblWebUrl";
      this.lblWebUrl.Size = new Size(52, 14);
      this.lblWebUrl.TabIndex = 90;
      this.lblWebUrl.Text = "Web URL";
      this.txtBoxBizWebUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizWebUrl.Location = new Point(143, 189);
      this.txtBoxBizWebUrl.MaxLength = 50;
      this.txtBoxBizWebUrl.Name = "txtBoxBizWebUrl";
      this.txtBoxBizWebUrl.ReadOnly = true;
      this.txtBoxBizWebUrl.Size = new Size(160, 20);
      this.txtBoxBizWebUrl.TabIndex = 96;
      this.txtBoxBizWebUrl.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizWebUrl.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxFees.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxFees.Location = new Point(143, 167);
      this.txtBoxFees.MaxLength = 8;
      this.txtBoxFees.Name = "txtBoxFees";
      this.txtBoxFees.ReadOnly = true;
      this.txtBoxFees.Size = new Size(160, 20);
      this.txtBoxFees.TabIndex = 94;
      this.txtBoxFees.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxFees.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblFees.AutoSize = true;
      this.lblFees.Location = new Point(7, 170);
      this.lblFees.Name = "lblFees";
      this.lblFees.Size = new Size(31, 14);
      this.lblFees.TabIndex = 88;
      this.lblFees.Text = "Fees";
      this.txtBoxBizState.CharacterCasing = CharacterCasing.Upper;
      this.txtBoxBizState.Location = new Point(143, 145);
      this.txtBoxBizState.MaxLength = 2;
      this.txtBoxBizState.Name = "txtBoxBizState";
      this.txtBoxBizState.ReadOnly = true;
      this.txtBoxBizState.Size = new Size(30, 20);
      this.txtBoxBizState.TabIndex = 90;
      this.txtBoxBizState.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizState.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxBizZip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizZip.Location = new Point(231, 145);
      this.txtBoxBizZip.MaxLength = 20;
      this.txtBoxBizZip.Name = "txtBoxBizZip";
      this.txtBoxBizZip.ReadOnly = true;
      this.txtBoxBizZip.Size = new Size(70, 20);
      this.txtBoxBizZip.TabIndex = 92;
      this.txtBoxBizZip.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizZip.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblBizZip.AutoSize = true;
      this.lblBizZip.Location = new Point(203, 148);
      this.lblBizZip.Name = "lblBizZip";
      this.lblBizZip.RightToLeft = RightToLeft.No;
      this.lblBizZip.Size = new Size(22, 14);
      this.lblBizZip.TabIndex = 84;
      this.lblBizZip.Text = "Zip";
      this.lblBizState.AutoSize = true;
      this.lblBizState.Location = new Point(7, 148);
      this.lblBizState.Name = "lblBizState";
      this.lblBizState.RightToLeft = RightToLeft.No;
      this.lblBizState.Size = new Size(32, 14);
      this.lblBizState.TabIndex = 82;
      this.lblBizState.Text = "State";
      this.txtBoxBizCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizCity.Location = new Point(143, 123);
      this.txtBoxBizCity.MaxLength = 50;
      this.txtBoxBizCity.Name = "txtBoxBizCity";
      this.txtBoxBizCity.ReadOnly = true;
      this.txtBoxBizCity.Size = new Size(160, 20);
      this.txtBoxBizCity.TabIndex = 88;
      this.txtBoxBizCity.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizCity.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxBizAddress2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizAddress2.Location = new Point(143, 101);
      this.txtBoxBizAddress2.MaxLength = 50;
      this.txtBoxBizAddress2.Name = "txtBoxBizAddress2";
      this.txtBoxBizAddress2.ReadOnly = true;
      this.txtBoxBizAddress2.Size = new Size(160, 20);
      this.txtBoxBizAddress2.TabIndex = 86;
      this.txtBoxBizAddress2.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizAddress2.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblBizCity.AutoSize = true;
      this.lblBizCity.Location = new Point(7, 126);
      this.lblBizCity.Name = "lblBizCity";
      this.lblBizCity.RightToLeft = RightToLeft.No;
      this.lblBizCity.Size = new Size(25, 14);
      this.lblBizCity.TabIndex = 80;
      this.lblBizCity.Text = "City";
      this.lblBizAddress2.AutoSize = true;
      this.lblBizAddress2.Location = new Point(7, 104);
      this.lblBizAddress2.Name = "lblBizAddress2";
      this.lblBizAddress2.Size = new Size(58, 14);
      this.lblBizAddress2.TabIndex = 78;
      this.lblBizAddress2.Text = "Address 2";
      this.lblBizAddress1.AutoSize = true;
      this.lblBizAddress1.Location = new Point(7, 82);
      this.lblBizAddress1.Name = "lblBizAddress1";
      this.lblBizAddress1.Size = new Size(58, 14);
      this.lblBizAddress1.TabIndex = 76;
      this.lblBizAddress1.Text = "Address 1";
      this.txtBoxBizAddress1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizAddress1.Location = new Point(143, 79);
      this.txtBoxBizAddress1.MaxLength = (int) byte.MaxValue;
      this.txtBoxBizAddress1.Name = "txtBoxBizAddress1";
      this.txtBoxBizAddress1.ReadOnly = true;
      this.txtBoxBizAddress1.Size = new Size(160, 20);
      this.txtBoxBizAddress1.TabIndex = 84;
      this.txtBoxBizAddress1.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizAddress1.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.cmbBoxCategoryID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxCategoryID.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCategoryID.Enabled = false;
      this.cmbBoxCategoryID.Location = new Point(143, 33);
      this.cmbBoxCategoryID.Name = "cmbBoxCategoryID";
      this.cmbBoxCategoryID.Size = new Size(160, 22);
      this.cmbBoxCategoryID.Sorted = true;
      this.cmbBoxCategoryID.TabIndex = 80;
      this.cmbBoxCategoryID.SelectedIndexChanged += new EventHandler(this.cmbBoxCategoryID_SelectedIndexChanged);
      this.txtBoxCompanyName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxCompanyName.Location = new Point(143, 57);
      this.txtBoxCompanyName.MaxLength = 64;
      this.txtBoxCompanyName.Name = "txtBoxCompanyName";
      this.txtBoxCompanyName.ReadOnly = true;
      this.txtBoxCompanyName.Size = new Size(160, 20);
      this.txtBoxCompanyName.TabIndex = 82;
      this.txtBoxCompanyName.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxCompanyName.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblCategory.AutoSize = true;
      this.lblCategory.Location = new Point(7, 36);
      this.lblCategory.Name = "lblCategory";
      this.lblCategory.Size = new Size(51, 14);
      this.lblCategory.TabIndex = 72;
      this.lblCategory.Text = "Category";
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(7, 60);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(52, 14);
      this.lblCompany.TabIndex = 74;
      this.lblCompany.Text = "Company";
      this.groupContainer2.Controls.Add((Control) this.comboBoxPersonalInfoLicAuthType);
      this.groupContainer2.Controls.Add((Control) this.comboBoxPersonalInfoLicState);
      this.groupContainer2.Controls.Add((Control) this.dpPersonalInfoLicIssueDate);
      this.groupContainer2.Controls.Add((Control) this.txtBoxPersonalInfoLicAuthNAme);
      this.groupContainer2.Controls.Add((Control) this.txtBoxPersonalInfoLicNo);
      this.groupContainer2.Controls.Add((Control) this.label7);
      this.groupContainer2.Controls.Add((Control) this.label6);
      this.groupContainer2.Controls.Add((Control) this.label5);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.label1);
      this.groupContainer2.Controls.Add((Control) this.chkPublic);
      this.groupContainer2.Controls.Add((Control) this.txtBoxLastName);
      this.groupContainer2.Controls.Add((Control) this.chkBoxNoSpam);
      this.groupContainer2.Controls.Add((Control) this.lblWorkEmail);
      this.groupContainer2.Controls.Add((Control) this.txtBoxBizEmail);
      this.groupContainer2.Controls.Add((Control) this.lblFaxNumber);
      this.groupContainer2.Controls.Add((Control) this.txtBoxSalutation);
      this.groupContainer2.Controls.Add((Control) this.lblCellPhone);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.txtBoxWorkPhone);
      this.groupContainer2.Controls.Add((Control) this.txtBoxPersonalEmail);
      this.groupContainer2.Controls.Add((Control) this.lblWorkPhone);
      this.groupContainer2.Controls.Add((Control) this.txtBoxFaxNumber);
      this.groupContainer2.Controls.Add((Control) this.lblHomeEmail);
      this.groupContainer2.Controls.Add((Control) this.txtBoxMobilePhone);
      this.groupContainer2.Controls.Add((Control) this.lblHomePhone);
      this.groupContainer2.Controls.Add((Control) this.txtBoxHomePhone);
      this.groupContainer2.Controls.Add((Control) this.txtBoxFirstName);
      this.groupContainer2.Controls.Add((Control) this.lblFirstName);
      this.groupContainer2.Controls.Add((Control) this.lblLastName);
      this.groupContainer2.Dock = DockStyle.Left;
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 2);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(322, 375);
      this.groupContainer2.TabIndex = 76;
      this.groupContainer2.Text = "Personal Information";
      this.comboBoxPersonalInfoLicAuthType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxPersonalInfoLicAuthType.Enabled = false;
      this.comboBoxPersonalInfoLicAuthType.FormattingEnabled = true;
      this.comboBoxPersonalInfoLicAuthType.Items.AddRange(new object[5]
      {
        (object) "",
        (object) "Private",
        (object) "Public Federal",
        (object) "Public Local",
        (object) "Public State"
      });
      this.comboBoxPersonalInfoLicAuthType.Location = new Point(147, 273);
      this.comboBoxPersonalInfoLicAuthType.Name = "comboBoxPersonalInfoLicAuthType";
      this.comboBoxPersonalInfoLicAuthType.Size = new Size(121, 22);
      this.comboBoxPersonalInfoLicAuthType.TabIndex = 79;
      this.comboBoxPersonalInfoLicAuthType.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.comboBoxPersonalInfoLicState.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxPersonalInfoLicState.Enabled = false;
      this.comboBoxPersonalInfoLicState.FormattingEnabled = true;
      this.comboBoxPersonalInfoLicState.Location = new Point(147, 295);
      this.comboBoxPersonalInfoLicState.Name = "comboBoxPersonalInfoLicState";
      this.comboBoxPersonalInfoLicState.Size = new Size(66, 22);
      this.comboBoxPersonalInfoLicState.TabIndex = 70;
      this.comboBoxPersonalInfoLicState.SelectedIndexChanged += new EventHandler(this.dataChanged);
      this.dpPersonalInfoLicIssueDate.BackColor = SystemColors.Window;
      this.dpPersonalInfoLicIssueDate.Enabled = false;
      this.dpPersonalInfoLicIssueDate.Location = new Point(147, 318);
      this.dpPersonalInfoLicIssueDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dpPersonalInfoLicIssueDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpPersonalInfoLicIssueDate.Name = "dpPersonalInfoLicIssueDate";
      this.dpPersonalInfoLicIssueDate.Size = new Size(123, 22);
      this.dpPersonalInfoLicIssueDate.TabIndex = 72;
      this.dpPersonalInfoLicIssueDate.Tag = (object) "763";
      this.dpPersonalInfoLicIssueDate.ToolTip = "";
      this.dpPersonalInfoLicIssueDate.Value = new DateTime(0L);
      this.dpPersonalInfoLicIssueDate.ValueChanged += new EventHandler(this.dataChanged);
      this.txtBoxPersonalInfoLicAuthNAme.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxPersonalInfoLicAuthNAme.Location = new Point(147, 252);
      this.txtBoxPersonalInfoLicAuthNAme.MaxLength = 50;
      this.txtBoxPersonalInfoLicAuthNAme.Name = "txtBoxPersonalInfoLicAuthNAme";
      this.txtBoxPersonalInfoLicAuthNAme.ReadOnly = true;
      this.txtBoxPersonalInfoLicAuthNAme.Size = new Size(146, 20);
      this.txtBoxPersonalInfoLicAuthNAme.TabIndex = 66;
      this.txtBoxPersonalInfoLicAuthNAme.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxPersonalInfoLicNo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxPersonalInfoLicNo.Location = new Point(147, 231);
      this.txtBoxPersonalInfoLicNo.MaxLength = 50;
      this.txtBoxPersonalInfoLicNo.Name = "txtBoxPersonalInfoLicNo";
      this.txtBoxPersonalInfoLicNo.ReadOnly = true;
      this.txtBoxPersonalInfoLicNo.Size = new Size(146, 20);
      this.txtBoxPersonalInfoLicNo.TabIndex = 64;
      this.txtBoxPersonalInfoLicNo.TextChanged += new EventHandler(this.dataChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 324);
      this.label7.Name = "label7";
      this.label7.Size = new Size(78, 14);
      this.label7.TabIndex = 69;
      this.label7.Text = "Lic. Issue Date";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 302);
      this.label6.Name = "label6";
      this.label6.Size = new Size(126, 14);
      this.label6.TabIndex = 68;
      this.label6.Text = "Lic. Authority State Code";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 280);
      this.label5.Name = "label5";
      this.label5.Size = new Size(96, 14);
      this.label5.TabIndex = 67;
      this.label5.Text = "Lic. Authority Type";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 258);
      this.label3.Name = "label3";
      this.label3.Size = new Size(137, 14);
      this.label3.TabIndex = 66;
      this.label3.Text = "Lic. Issuing Authority Name";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 234);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 14);
      this.label1.TabIndex = 65;
      this.label1.Text = "Contact License #";
      this.chkPublic.AutoSize = true;
      this.chkPublic.Enabled = false;
      this.chkPublic.Location = new Point(236, 346);
      this.chkPublic.Name = "chkPublic";
      this.chkPublic.Size = new Size(54, 18);
      this.chkPublic.TabIndex = 78;
      this.chkPublic.Text = "Public";
      this.chkPublic.UseVisualStyleBackColor = true;
      this.txtBoxLastName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxLastName.Location = new Point(147, 55);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.ReadOnly = true;
      this.txtBoxLastName.Size = new Size(146, 20);
      this.txtBoxLastName.TabIndex = 48;
      this.txtBoxLastName.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxLastName.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.chkBoxNoSpam.AutoSize = true;
      this.chkBoxNoSpam.Enabled = false;
      this.chkBoxNoSpam.Location = new Point(145, 346);
      this.chkBoxNoSpam.Name = "chkBoxNoSpam";
      this.chkBoxNoSpam.Size = new Size(85, 18);
      this.chkBoxNoSpam.TabIndex = 76;
      this.chkBoxNoSpam.Text = "Do Not Email";
      this.chkBoxNoSpam.CheckedChanged += new EventHandler(this.dataChanged);
      this.lblWorkEmail.AutoSize = true;
      this.lblWorkEmail.Location = new Point(7, 212);
      this.lblWorkEmail.Name = "lblWorkEmail";
      this.lblWorkEmail.Size = new Size(59, 14);
      this.lblWorkEmail.TabIndex = 61;
      this.lblWorkEmail.Text = "Work Email";
      this.txtBoxBizEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizEmail.Location = new Point(147, 209);
      this.txtBoxBizEmail.MaxLength = 50;
      this.txtBoxBizEmail.Name = "txtBoxBizEmail";
      this.txtBoxBizEmail.ReadOnly = true;
      this.txtBoxBizEmail.Size = new Size(146, 20);
      this.txtBoxBizEmail.TabIndex = 62;
      this.txtBoxBizEmail.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxBizEmail.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblFaxNumber.AutoSize = true;
      this.lblFaxNumber.Location = new Point(7, 168);
      this.lblFaxNumber.Name = "lblFaxNumber";
      this.lblFaxNumber.Size = new Size(65, 14);
      this.lblFaxNumber.TabIndex = 57;
      this.lblFaxNumber.Text = "Fax Number";
      this.txtBoxSalutation.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxSalutation.Location = new Point(147, 77);
      this.txtBoxSalutation.MaxLength = 84;
      this.txtBoxSalutation.Name = "txtBoxSalutation";
      this.txtBoxSalutation.ReadOnly = true;
      this.txtBoxSalutation.Size = new Size(146, 20);
      this.txtBoxSalutation.TabIndex = 50;
      this.txtBoxSalutation.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxSalutation.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblCellPhone.AutoSize = true;
      this.lblCellPhone.Location = new Point(6, 146);
      this.lblCellPhone.Name = "lblCellPhone";
      this.lblCellPhone.Size = new Size(57, 14);
      this.lblCellPhone.TabIndex = 55;
      this.lblCellPhone.Text = "Cell Phone";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 80);
      this.label2.Name = "label2";
      this.label2.Size = new Size(54, 14);
      this.label2.TabIndex = 49;
      this.label2.Text = "Salutation";
      this.txtBoxWorkPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxWorkPhone.Location = new Point(147, 121);
      this.txtBoxWorkPhone.MaxLength = 30;
      this.txtBoxWorkPhone.Name = "txtBoxWorkPhone";
      this.txtBoxWorkPhone.ReadOnly = true;
      this.txtBoxWorkPhone.Size = new Size(146, 20);
      this.txtBoxWorkPhone.TabIndex = 54;
      this.txtBoxWorkPhone.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxWorkPhone.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxPersonalEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxPersonalEmail.Location = new Point(147, 187);
      this.txtBoxPersonalEmail.MaxLength = 50;
      this.txtBoxPersonalEmail.Name = "txtBoxPersonalEmail";
      this.txtBoxPersonalEmail.ReadOnly = true;
      this.txtBoxPersonalEmail.Size = new Size(146, 20);
      this.txtBoxPersonalEmail.TabIndex = 60;
      this.txtBoxPersonalEmail.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxPersonalEmail.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblWorkPhone.AutoSize = true;
      this.lblWorkPhone.Location = new Point(7, 124);
      this.lblWorkPhone.Name = "lblWorkPhone";
      this.lblWorkPhone.Size = new Size(65, 14);
      this.lblWorkPhone.TabIndex = 53;
      this.lblWorkPhone.Text = "Work Phone";
      this.txtBoxFaxNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxFaxNumber.Location = new Point(147, 165);
      this.txtBoxFaxNumber.MaxLength = 30;
      this.txtBoxFaxNumber.Name = "txtBoxFaxNumber";
      this.txtBoxFaxNumber.ReadOnly = true;
      this.txtBoxFaxNumber.Size = new Size(146, 20);
      this.txtBoxFaxNumber.TabIndex = 58;
      this.txtBoxFaxNumber.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxFaxNumber.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblHomeEmail.AutoSize = true;
      this.lblHomeEmail.Location = new Point(7, 190);
      this.lblHomeEmail.Name = "lblHomeEmail";
      this.lblHomeEmail.Size = new Size(61, 14);
      this.lblHomeEmail.TabIndex = 59;
      this.lblHomeEmail.Text = "Home Email";
      this.txtBoxMobilePhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxMobilePhone.Location = new Point(147, 143);
      this.txtBoxMobilePhone.MaxLength = 30;
      this.txtBoxMobilePhone.Name = "txtBoxMobilePhone";
      this.txtBoxMobilePhone.ReadOnly = true;
      this.txtBoxMobilePhone.Size = new Size(146, 20);
      this.txtBoxMobilePhone.TabIndex = 56;
      this.txtBoxMobilePhone.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxMobilePhone.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblHomePhone.AutoSize = true;
      this.lblHomePhone.Location = new Point(7, 103);
      this.lblHomePhone.Name = "lblHomePhone";
      this.lblHomePhone.Size = new Size(67, 14);
      this.lblHomePhone.TabIndex = 51;
      this.lblHomePhone.Text = "Home Phone";
      this.txtBoxHomePhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxHomePhone.Location = new Point(147, 99);
      this.txtBoxHomePhone.MaxLength = 30;
      this.txtBoxHomePhone.Name = "txtBoxHomePhone";
      this.txtBoxHomePhone.ReadOnly = true;
      this.txtBoxHomePhone.Size = new Size(146, 20);
      this.txtBoxHomePhone.TabIndex = 52;
      this.txtBoxHomePhone.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxHomePhone.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxFirstName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxFirstName.Location = new Point(147, 33);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.ReadOnly = true;
      this.txtBoxFirstName.Size = new Size(146, 20);
      this.txtBoxFirstName.TabIndex = 46;
      this.txtBoxFirstName.TextChanged += new EventHandler(this.dataChanged);
      this.txtBoxFirstName.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblFirstName.AutoSize = true;
      this.lblFirstName.Location = new Point(7, 36);
      this.lblFirstName.Name = "lblFirstName";
      this.lblFirstName.Size = new Size(58, 14);
      this.lblFirstName.TabIndex = 45;
      this.lblFirstName.Text = "First Name";
      this.lblLastName.AutoSize = true;
      this.lblLastName.Location = new Point(7, 58);
      this.lblLastName.Name = "lblLastName";
      this.lblLastName.Size = new Size(58, 14);
      this.lblLastName.TabIndex = 47;
      this.lblLastName.Text = "Last Name";
      this.tabCategoryStandardFields.Location = new Point(4, 23);
      this.tabCategoryStandardFields.Name = "tabCategoryStandardFields";
      this.tabCategoryStandardFields.Size = new Size(650, 379);
      this.tabCategoryStandardFields.TabIndex = 2;
      this.tabCategoryStandardFields.Text = "Category Fields";
      this.tabCategoryStandardFields.UseVisualStyleBackColor = true;
      this.picLinked.BackColor = Color.Transparent;
      this.picLinked.Image = (Image) Resources.link;
      this.picLinked.InitialImage = (Image) componentResourceManager.GetObject("picLinked.InitialImage");
      this.picLinked.Location = new Point(97, 5);
      this.picLinked.Name = "picLinked";
      this.picLinked.Size = new Size(19, 15);
      this.picLinked.TabIndex = 1;
      this.picLinked.TabStop = false;
      this.panel1.Controls.Add((Control) this.chkLink);
      this.panel1.Controls.Add((Control) this.flowLayoutPanel2);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(1, 680);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(662, 37);
      this.panel1.TabIndex = 33;
      this.chkLink.AutoSize = true;
      this.chkLink.Location = new Point(7, 10);
      this.chkLink.Name = "chkLink";
      this.chkLink.Size = new Size(213, 18);
      this.chkLink.TabIndex = 8;
      this.chkLink.Text = "Link this loan with the selected contact";
      this.chkLink.UseVisualStyleBackColor = true;
      this.flowLayoutPanel2.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnNewLink);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnLink);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(339, 6);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Padding = new Padding(0, 0, 10, 0);
      this.flowLayoutPanel2.Size = new Size(324, 24);
      this.flowLayoutPanel2.TabIndex = 7;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(239, 0);
      this.btnCancel.Margin = new Padding(0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Close";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnNewLink.BackColor = SystemColors.Control;
      this.btnNewLink.Location = new Point(104, 0);
      this.btnNewLink.Margin = new Padding(0);
      this.btnNewLink.Name = "btnNewLink";
      this.btnNewLink.Size = new Size(135, 24);
      this.btnNewLink.TabIndex = 4;
      this.btnNewLink.Text = "Create New and Select";
      this.btnNewLink.UseVisualStyleBackColor = true;
      this.btnNewLink.Click += new EventHandler(this.btnNewLink_Click);
      this.btnLink.BackColor = SystemColors.Control;
      this.btnLink.Location = new Point(29, 0);
      this.btnLink.Margin = new Padding(0);
      this.btnLink.Name = "btnLink";
      this.btnLink.Size = new Size(75, 24);
      this.btnLink.TabIndex = 3;
      this.btnLink.Text = "Select";
      this.btnLink.UseVisualStyleBackColor = true;
      this.btnLink.Click += new EventHandler(this.btnLink_Click);
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDelete);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnDuplicate);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(565, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(91, 24);
      this.flowLayoutPanel1.TabIndex = 32;
      this.navContacts.BackColor = Color.Transparent;
      this.navContacts.Font = new Font("Arial", 8f);
      this.navContacts.ItemsPerPage = 100;
      this.navContacts.Location = new Point(0, 2);
      this.navContacts.Name = "navContacts";
      this.navContacts.NumberOfItems = 0;
      this.navContacts.Size = new Size(254, 24);
      this.navContacts.TabIndex = 26;
      this.navContacts.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContacts_PageChangedEvent);
      this.gradientPanel3.Borders = AnchorStyles.Left | AnchorStyles.Right;
      this.gradientPanel3.Controls.Add((Control) this.lblFilter);
      this.gradientPanel3.Controls.Add((Control) this.btnClear);
      this.gradientPanel3.Controls.Add((Control) this.label4);
      this.gradientPanel3.Dock = DockStyle.Top;
      this.gradientPanel3.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel3.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel3.Location = new Point(0, 0);
      this.gradientPanel3.Name = "gradientPanel3";
      this.gradientPanel3.Size = new Size(664, 30);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 7;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Location = new Point(34, 8);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(560, 14);
      this.lblFilter.TabIndex = 7;
      this.lblFilter.Text = "label1";
      this.btnClear.BackColor = SystemColors.Control;
      this.btnClear.Location = new Point(600, 4);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(56, 22);
      this.btnClear.TabIndex = 6;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(5, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(33, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Filter:";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(664, 748);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.gradientPanel3);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RxBusinessContact);
      this.ShowInTaskbar = false;
      this.Text = " Business Contacts";
      this.FormClosing += new FormClosingEventHandler(this.RxBusinessContact_FormClosing);
      ((ISupportInitialize) this.siBtnSave).EndInit();
      ((ISupportInitialize) this.btnDelete).EndInit();
      ((ISupportInitialize) this.btnDuplicate).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.gcDetail.ResumeLayout(false);
      this.flowLayoutPanel3.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      ((ISupportInitialize) this.picLinked).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.flowLayoutPanel2.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.ResumeLayout(false);
    }

    public enum ActionMode
    {
      SelectMode,
      LinkMode,
      ManageMode,
    }
  }
}
