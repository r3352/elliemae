// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Contact.RxBorrowerContact
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.Properties;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Contact
{
  public class RxBorrowerContact : Form
  {
    private FeaturesAclManager aclMgr;
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private TableLayout borrowerTableLayout;
    private ICursor contactCursor;
    private BorrowerReportFieldDefs contactFieldDefs;
    private FieldFilterList advFilter;
    private ContactView currentView;
    private PageChangedEventArgs currentPagingArgument;
    private bool goToContact;
    private ContactInfo selectedContact;
    private static string sw = Tracing.SwContact;
    private bool isCoborrower;
    private BorrowerInfo borObj;
    private string currentAssContactGuid = "";
    private string mappingID = "";
    private string firstName = "";
    private string lastName = "";
    private string middleName = "";
    private string suffix = "";
    private string ssn = "";
    private string hPhone = "";
    private string wPhone = "";
    private string cPhone = "";
    private EllieMae.EMLite.ClientServer.Address hAddress;
    private DateTime birthday = DateTime.MinValue;
    private string referral = "";
    private bool married;
    private bool primaryContact;
    private string email = "";
    private string faxPhone = "";
    private bool forceOpOutLogic;
    private string loID = "";
    private bool forceClose;
    private string currentUserID = string.Empty;
    private IContainer components;
    private GradientPanel gradientPanel3;
    private Label label4;
    private GroupContainer groupContainer1;
    private PageListNavigator navContacts;
    private GridView gvContactList;
    private TabControl tabControl1;
    private TabPage tpDetails;
    private TabPage tpExtra;
    private TextBox txtSuffixName;
    private Label label3;
    private TextBox txtMiddleName;
    private Label label2;
    private Label lblSSN;
    private TextBox txtBoxSSN;
    private TextBox txtBoxSalutation;
    private Label label5;
    private Label lblWorkEmail;
    private TextBox txtBoxBizEmail;
    private TextBox txtBoxFaxNumber;
    private Label lblFaxNumber;
    private TextBox txtBoxMobilePhone;
    private Label lblCellPhone;
    private TextBox txtBoxWorkPhone;
    private Label lblWorkPhone;
    private TextBox txtBoxLastName;
    private Label lblLastName;
    private Label lblFirstName;
    private TextBox txtBoxFirstName;
    private Label lblHomeEmail;
    private TextBox txtBoxPersonalEmail;
    private TextBox txtBoxHomePhone;
    private Label lblHomePhone;
    private CheckBox chkBoxNoFax;
    private CheckBox chkBoxNoSpam;
    private CheckBox chkBoxNoCall;
    private TextBox txtBoxHomeState;
    private TextBox txtBoxHomeAddress2;
    private TextBox txtBoxHomeAddress1;
    private Label lblHomeZip;
    private Label lblHomeCity;
    private Label lblHomeState;
    private Label lblHomeAddress1;
    private TextBox txtBoxHomeZip;
    private Label lblHomeAddress2;
    private TextBox txtBoxHomeCity;
    private Button btnNewLink;
    private Button btnLink;
    private Button btnCancel;
    private Label lblBorrowerType;
    private ComboBox cmbBoxContactType;
    private Label lblBorrowerStatus;
    private ComboBox cmbBoxStatus;
    private TextBox txtBoxBizState;
    private TextBox txtBoxBizAddress2;
    private TextBox txtBoxBizAddress1;
    private Label lblBizZip;
    private Label lblBizCity;
    private Label lblBizState;
    private Label lblBizAddress1;
    private Label lblBizAddress2;
    private TextBox txtBoxBizCity;
    private Label lblCompany;
    private TextBox txtBoxEmployerName;
    private Label lblTitle;
    private TextBox txtBoxJobTitle;
    private TextBox txtBoxBizWebUrl;
    private Label lblWebUrl;
    private NoContextMenuTextBox txtBoxLeadTransID;
    private Label label6;
    private NoContextMenuTextBox txtBoxReferral;
    private CheckBox chkBoxAccess;
    private CheckBox chkBoxAccessPublic;
    private TextBox txtBoxIncome;
    private Label lblIncome;
    private Label lblReferral;
    private TextBox txtBoxOwner;
    private Label lblOwner;
    private Button btnClear;
    private PictureBox picLinked;
    private Button btnGoTo;
    private GroupContainer groupContainer2;
    private GroupContainer groupContainer3;
    private Label lblBirthday;
    private TextBox txtBoxBirthdate;
    private CheckBox chkBoxMarried;
    private Label lblSpouse;
    private TextBox txtBoxSpouseName;
    private TextBox txtBoxAnniversary;
    private Label lblAnniversary;
    private Label label7;
    private GroupContainer gcDetail;
    private Panel panel1;
    private CollapsibleSplitter collapsibleSplitter1;
    private GroupContainer groupContainer4;
    private GroupContainer groupContainer5;
    private TextBox txtBoxBizZip;
    private Label label1;
    private CheckBox chkPrimary;
    private Panel panel2;
    private Panel panel3;
    private BorderPanel borderPanel1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Label lblFilter;
    private Button btnImport;

    public RxBorrowerContact(bool isCoborrower, bool allowNavigate)
      : this(isCoborrower, allowNavigate, false)
    {
    }

    public RxBorrowerContact(bool isCoborrower, bool allowNavigate, bool forceOptoutLogic)
      : this(isCoborrower, allowNavigate, forceOptoutLogic, "")
    {
    }

    public RxBorrowerContact(
      bool isCoborrower,
      bool allowNavigate,
      bool forceOptoutLogic,
      string currentUserID)
    {
      this.currentUserID = currentUserID;
      this.InitializeComponent();
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.isCoborrower = isCoborrower;
      this.picLinked.Visible = false;
      this.forceOpOutLogic = forceOptoutLogic;
      this.init();
      if (!allowNavigate)
        this.btnGoTo.Enabled = false;
      if (this.currentUserID != string.Empty)
      {
        this.btnNewLink.Visible = this.btnLink.Visible = false;
        this.btnImport.Left = this.btnCancel.Left + this.btnImport.Width + 10;
        this.btnGoTo.Visible = false;
      }
      this.DialogResult = DialogResult.Cancel;
    }

    private void init()
    {
      LoanData loanData = Session.LoanData;
      this.loID = this.currentUserID != string.Empty ? this.currentUserID : loanData.GetSimpleField("LOID");
      this.enforceSecurity();
      this.initialGVContactList();
      ContactView view = new ContactView("DefaultView");
      view.Layout = this.getDefaultTableLayout();
      if (this.isCoborrower)
      {
        this.ssn = loanData.GetField("97");
        this.firstName = loanData.GetField("4004");
        this.lastName = loanData.GetField("4006");
      }
      else
      {
        this.ssn = loanData.GetField("65");
        this.firstName = loanData.GetField("4000");
        this.lastName = loanData.GetField("4002");
      }
      view.Filter = new FieldFilterList();
      if (this.ssn != "")
        view.Filter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "Contact.SSN", "Social Security", OperatorTypes.Equals, this.ssn), JointTokens.And);
      if (this.firstName != "")
        view.Filter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "Contact.FirstName", "First Name", OperatorTypes.Equals, this.firstName), JointTokens.And);
      if (this.lastName != "")
        view.Filter.Add(new FieldFilter(EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsString, "Contact.LastName", "Last Name", OperatorTypes.Equals, this.lastName), JointTokens.And);
      this.cmbBoxContactType.Items.AddRange(BorrowerTypeEnumUtil.GetDisplayNames());
      BorrowerStatus borrowerStatus = Session.ContactManager.GetBorrowerStatus();
      this.cmbBoxStatus.Items.Clear();
      this.cmbBoxStatus.Items.Add((object) new BorrowerStatusItem("", -1));
      this.cmbBoxStatus.Items.AddRange((object[]) borrowerStatus.Items);
      this.mappingID = !this.isCoborrower ? Session.LoanData.CurrentBorrowerPair.Borrower.Id : Session.LoanData.CurrentBorrowerPair.CoBorrower.Id;
      if (Session.LoanData.GetLogList().GetCRMMapping(this.mappingID) != null)
        this.currentAssContactGuid = Session.LoanData.GetLogList().GetCRMMapping(this.mappingID).ContactGuid;
      this.setCurrentView(view);
    }

    private void setCurrentView(ContactView view)
    {
      this.currentView = view;
      this.applyTableLayout(view.Layout);
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
        this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvContactList);
        this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      }
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) this.contactFieldDefs);
    }

    private GridViewLayoutManager createLayoutManager()
    {
      GridViewLayoutManager layoutManager = new GridViewLayoutManager(this.gvContactList, this.getFullTableLayout());
      layoutManager.LayoutChanged += new EventHandler(this.mngr_LayoutChanged);
      return layoutManager;
    }

    private void mngr_LayoutChanged(object sender, EventArgs e)
    {
    }

    private void enforceSecurity()
    {
      if (this.loID == "")
      {
        this.btnClear.Enabled = false;
        this.btnLink.Enabled = false;
        this.btnNewLink.Enabled = false;
        this.btnGoTo.Visible = false;
      }
      else
      {
        if (this.loID != Session.UserID)
        {
          this.btnClear.Visible = false;
          this.btnGoTo.Visible = false;
        }
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_CreateNew))
          this.btnNewLink.Enabled = false;
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Borrower_Access))
          this.btnGoTo.Visible = false;
        if (!this.forceOpOutLogic)
          return;
        if (this.aclMgr.GetUserApplicationRight(AclFeature.Cnt_Contacts_Update))
        {
          this.btnCancel.Visible = true;
        }
        else
        {
          this.btnCancel.Visible = false;
          this.ControlBox = false;
        }
      }
    }

    private void initialGVContactList()
    {
      this.contactFieldDefs = BorrowerReportFieldDefs.GetFieldDefs(true);
      this.gvLayoutManager = new GridViewLayoutManager(this.gvContactList, this.getFullTableLayout(), this.getDefaultTableLayout());
      if (this.borrowerTableLayout != null)
        this.gvLayoutManager.ApplyLayout(this.borrowerTableLayout, false);
      this.gvLayoutManager.LayoutChanged += new EventHandler(this.gvLayoutManager_LayoutChanged);
      this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvContactList);
      this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      this.gvFilterManager.CreateColumnFilters((ReportFieldDefs) BorrowerReportFieldDefs.GetFieldDefs(false));
      this.buildQuerySummary(true);
    }

    private TableLayout getFullTableLayout()
    {
      TableLayout fullTableLayout = new TableLayout();
      foreach (BorrowerReportFieldDef contactFieldDef in (ReportFieldDefContainer) this.contactFieldDefs)
      {
        if (fullTableLayout.GetColumnByID(contactFieldDef.CriterionFieldName) == null)
          fullTableLayout.AddColumn(new TableLayout.Column(contactFieldDef.CriterionFieldName, contactFieldDef.Name, contactFieldDef.Description, contactFieldDef.FieldType == EllieMae.EMLite.ClientServer.Reporting.FieldTypes.IsNumeric ? HorizontalAlignment.Right : HorizontalAlignment.Left, 100));
      }
      fullTableLayout.SortByDescription();
      return fullTableLayout;
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.populateContacts(this.generateCriterion());
    }

    private void gvLayoutManager_LayoutChanged(object sender, EventArgs e)
    {
      this.borrowerTableLayout = this.gvLayoutManager.GetCurrentLayout();
      this.populateContacts(this.generateCriterion());
    }

    private void buildQuerySummary(bool populateLabel)
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      this.lblFilter.Text = fieldFilterList.ToString(true);
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
      if (this.loID == "")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must assign a loan officer to the loan file before linking it to a contact.");
        this.DialogResult = DialogResult.Cancel;
        this.forceClose = true;
      }
      else if (!Session.OrganizationManager.UserExists(this.loID))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The assigned loan officer does not exist in Encompass anymore.  You must assign a new loan officer to the loan file before linking it to a contact.");
        this.DialogResult = DialogResult.Cancel;
        this.forceClose = true;
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        try
        {
          if (this.contactCursor != null)
            this.contactCursor.Dispose();
          this.buildQuerySummary(true);
          this.contactCursor = advCri == null || advCri.Length == 0 || advCri[0] == null || !(advCri[0].ToSQLClause() != "") ? Session.ContactManager.OpenBorrowerCursorForUser(this.loID, new QueryCriterion[0], RelatedLoanMatchType.None, this.generateSortFields(), this.generateFieldList(), true) : Session.ContactManager.OpenBorrowerCursorForUser(this.loID, advCri, RelatedLoanMatchType.None, this.generateSortFields(), this.generateFieldList(), true);
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
    }

    public bool ForcedClose => this.forceClose;

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
        BorrowerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(column.ColumnID);
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
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.SSN", "SSN", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.FirstName", "First Name", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.LastName", "Last Name", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.HomePhone", "Home Phone", HorizontalAlignment.Left, 90));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.PersonalEmail", "Home Email", HorizontalAlignment.Left, 95));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.HomeAddress1", "Address 1", HorizontalAlignment.Left, 91));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.HomeCity", "City", HorizontalAlignment.Left, 100));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.HomeState", "State", HorizontalAlignment.Left, 50));
      defaultTableLayout.AddColumn(new TableLayout.Column("Contact.FaxNumber", "FAX Number", HorizontalAlignment.Left, 87));
      return defaultTableLayout;
    }

    private void navContacts_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
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
        BorrowerReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(columnId);
        if (fieldByCriterionName != null && contactInfo[columnId] != null)
          obj = ReportFieldClientExtension.ToDisplayElement(fieldByCriterionName, columnId, (IPropertyDictionary) contactInfo, (EventHandler) null);
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private int getSelectedItemsCount() => this.gvContactList.SelectedItems.Count;

    public void RefreshList() => this.populateContacts(this.generateCriterion());

    private void gvContactList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        this.borObj = (BorrowerInfo) null;
        this.btnGoTo.Enabled = false;
      }
      else
      {
        this.borObj = Session.ContactManager.GetBorrower(((BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag).ContactID);
        this.txtBoxFirstName.Text = this.borObj.FirstName;
        this.txtBoxLastName.Text = this.borObj.LastName;
        this.txtMiddleName.Text = this.borObj.MiddleName;
        this.txtSuffixName.Text = this.borObj.SuffixName;
        this.txtBoxSalutation.Text = this.borObj.Salutation;
        this.txtBoxSSN.Text = this.borObj.SSN;
        this.txtBoxHomePhone.Text = this.borObj.HomePhone;
        this.txtBoxWorkPhone.Text = this.borObj.WorkPhone;
        this.txtBoxMobilePhone.Text = this.borObj.MobilePhone;
        this.txtBoxFaxNumber.Text = this.borObj.FaxNumber;
        this.txtBoxPersonalEmail.Text = this.borObj.PersonalEmail;
        this.txtBoxBizEmail.Text = this.borObj.BizEmail;
        this.chkBoxNoCall.Checked = this.borObj.NoCall;
        this.chkBoxNoSpam.Checked = this.borObj.NoSpam;
        this.chkBoxNoFax.Checked = this.borObj.NoFax;
        this.txtBoxHomeAddress1.Text = this.borObj.HomeAddress.Street1;
        this.txtBoxHomeCity.Text = this.borObj.HomeAddress.Street2;
        this.txtBoxHomeCity.Text = this.borObj.HomeAddress.City;
        this.txtBoxHomeState.Text = this.borObj.HomeAddress.State;
        this.txtBoxHomeZip.Text = this.borObj.HomeAddress.Zip;
        this.cmbBoxContactType.Text = BorrowerTypeEnumUtil.ValueToName(this.borObj.ContactType);
        bool flag = false;
        foreach (BorrowerStatusItem borrowerStatusItem in this.cmbBoxStatus.Items)
        {
          if (borrowerStatusItem.name == this.borObj.Status)
          {
            flag = true;
            break;
          }
        }
        if (flag)
        {
          this.cmbBoxStatus.Text = this.borObj.Status;
        }
        else
        {
          this.cmbBoxStatus.Items.Add((object) new BorrowerStatusItem(this.borObj.Status, -1));
          this.cmbBoxStatus.Text = this.borObj.Status;
        }
        this.txtBoxEmployerName.Text = this.borObj.EmployerName;
        this.txtBoxJobTitle.Text = this.borObj.JobTitle;
        this.txtBoxBizAddress1.Text = this.borObj.BizAddress.Street1;
        this.txtBoxBizAddress2.Text = this.borObj.BizAddress.Street2;
        this.txtBoxBizCity.Text = this.borObj.BizAddress.City;
        this.txtBoxBizState.Text = this.borObj.BizAddress.State;
        this.txtBoxBizZip.Text = this.borObj.BizAddress.Zip;
        if (this.borObj.Birthdate != DateTime.MinValue)
          this.txtBoxBirthdate.Text = this.borObj.Birthdate.ToShortDateString();
        else
          this.txtBoxBirthdate.Text = "";
        this.txtBoxReferral.Text = this.borObj.Referral;
        this.txtBoxLeadTransID.Text = this.borObj.LeadTxnID;
        if (this.borObj.Income > 0M)
          this.txtBoxIncome.Text = this.borObj.Income.ToString();
        else
          this.txtBoxIncome.Text = "";
        this.chkBoxMarried.Checked = this.borObj.Married;
        this.txtBoxSpouseName.Text = this.borObj.SpouseName;
        if (this.borObj.Anniversary != DateTime.MinValue)
          this.txtBoxAnniversary.Text = this.borObj.Anniversary.ToString("MM/dd");
        else
          this.txtBoxAnniversary.Text = "";
        this.txtBoxOwner.Text = this.borObj.OwnerID;
        if (this.borObj.AccessLevel == ContactAccess.Public)
        {
          this.chkBoxAccess.Visible = false;
          this.chkBoxAccessPublic.Visible = true;
        }
        else
        {
          this.chkBoxAccess.Visible = true;
          this.chkBoxAccessPublic.Visible = false;
        }
        this.chkPrimary.Checked = this.borObj.PrimaryContact;
        if (this.borObj.ContactGuid.ToString() == this.currentAssContactGuid)
          this.picLinked.Visible = true;
        else
          this.picLinked.Visible = false;
        this.btnGoTo.Enabled = true;
      }
    }

    private void btnLink_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a borrower contact to link.");
      }
      else
      {
        if (this.currentAssContactGuid != "" && this.currentAssContactGuid != this.borObj.ContactGuid.ToString() && Utils.Dialog((IWin32Window) this, "This loan is currently linked to a different borrower contact.  If you create a link to the selected contact, you will break the existing link.  Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
          return;
        if (this.ssn == "")
          this.ssn = this.borObj.SSN;
        else if (this.borObj.SSN != "" && this.borObj.SSN != this.ssn)
        {
          RxBorrowerSSN rxBorrowerSsn = new RxBorrowerSSN(this.isCoborrower, this.borObj, false, this.forceOpOutLogic);
          if (rxBorrowerSsn.ShowDialog((IWin32Window) this) == DialogResult.Cancel)
          {
            this.DialogResult = DialogResult.Cancel;
            return;
          }
          this.borObj = rxBorrowerSsn.BorrowerObj;
        }
        Session.LoanData.GetLogList().AddCRMMapping(this.mappingID, CRMLogType.BorrowerContact, this.borObj.ContactGuid.ToString(), this.isCoborrower ? CRMRoleType.Coborrower : CRMRoleType.Borrower);
        this.DialogResult = DialogResult.OK;
        RxBorrowerSync rxBorrowerSync = new RxBorrowerSync(this.isCoborrower, this.borObj, this.forceOpOutLogic, false);
        if (rxBorrowerSync.HasConflict)
        {
          int num2 = (int) rxBorrowerSync.ShowDialog((IWin32Window) this);
          this.borObj = rxBorrowerSync.BorrowerObj;
        }
        this.DialogResult = DialogResult.OK;
      }
    }

    public BorrowerInfo BorrowerObj => this.borObj;

    private void btnNewLink_Click(object sender, EventArgs e)
    {
      int contact = this.createContact();
      this.borObj = Session.ContactManager.GetBorrower(contact);
      Session.LoanData.GetLogList().AddCRMMapping(this.mappingID, CRMLogType.BorrowerContact, this.borObj.ContactGuid.ToString(), this.isCoborrower ? CRMRoleType.Coborrower : CRMRoleType.Borrower);
      if (this.loID == Session.UserID)
        Session.MainScreen.AddNewBorrowerToContactManagerList(contact);
      RxBorrowerSync rxBorrowerSync = new RxBorrowerSync(this.isCoborrower, this.borObj, this.forceOpOutLogic, false);
      this.DialogResult = DialogResult.OK;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
      this.currentView.Filter.Clear();
      this.gvFilterManager.ClearColumnFilters();
      this.populateContacts(this.generateCriterion());
    }

    private int createContact()
    {
      BorrowerInfo info = new BorrowerInfo();
      LoanData loanData = Session.LoanData;
      if (this.isCoborrower)
      {
        this.firstName = loanData.GetField("4004");
        this.lastName = loanData.GetField("4006");
        this.middleName = loanData.GetField("4005");
        this.suffix = loanData.GetField("4007");
        this.ssn = loanData.GetField("97");
        this.hPhone = loanData.GetField("98");
        this.wPhone = loanData.GetField("FE0217");
        this.cPhone = loanData.GetField("1480");
        this.faxPhone = loanData.GetField("1241");
        this.email = loanData.GetField("1268");
        this.hAddress = new EllieMae.EMLite.ClientServer.Address(loanData.GetField("FR0204"), "", loanData.GetField("FR0206"), loanData.GetField("FR0207"), loanData.GetField("FR0208"));
        if (loanData.GetField("1403") != "")
        {
          try
          {
            this.birthday = DateTime.Parse(loanData.GetField("1403"));
          }
          catch (Exception ex)
          {
          }
        }
        this.referral = loanData.GetField("1822");
        if (loanData.GetField("84") != "" && loanData.GetField("84") != "Unmarried")
          this.married = true;
      }
      else
      {
        this.firstName = loanData.GetField("4000");
        this.lastName = loanData.GetField("4002");
        this.middleName = loanData.GetField("4001");
        this.suffix = loanData.GetField("4003");
        this.ssn = loanData.GetField("65");
        this.hPhone = loanData.GetField("66");
        this.wPhone = loanData.GetField("FE0117");
        this.cPhone = loanData.GetField("1490");
        this.faxPhone = loanData.GetField("1188");
        this.email = loanData.GetField("1240");
        this.hAddress = new EllieMae.EMLite.ClientServer.Address(loanData.GetField("FR0104"), "", loanData.GetField("FR0106"), loanData.GetField("FR0107"), loanData.GetField("FR0108"));
        if (loanData.GetField("1402") != "")
        {
          try
          {
            this.birthday = DateTime.Parse(loanData.GetField("1402"));
          }
          catch (Exception ex)
          {
          }
        }
        this.referral = loanData.GetField("1822");
        if (loanData.GetField("52") != "" && loanData.GetField("52") != "Unmarried")
          this.married = true;
        this.primaryContact = true;
      }
      info.FirstName = this.firstName;
      info.MiddleName = this.middleName;
      info.LastName = this.lastName;
      info.SuffixName = this.suffix;
      info.SSN = this.ssn;
      info.OwnerID = this.loID;
      info.AccessLevel = ContactAccess.Private;
      info.HomeAddress.Street1 = this.hAddress.Street1;
      info.HomeAddress.Street2 = this.hAddress.Street2;
      info.HomeAddress.City = this.hAddress.City;
      info.HomeAddress.State = this.hAddress.State;
      info.HomeAddress.Zip = this.hAddress.Zip;
      info.WorkPhone = this.wPhone;
      info.HomePhone = this.hPhone;
      info.MobilePhone = this.cPhone;
      info.Birthdate = this.birthday;
      info.Married = this.married;
      info.Referral = this.referral;
      info.PrimaryContact = this.primaryContact;
      info.FaxNumber = this.faxPhone;
      info.PersonalEmail = this.email;
      int borrower = Session.ContactManager.CreateBorrower(info);
      List<ContactCustomField> contactCustomFieldList = new List<ContactCustomField>();
      foreach (CustomFieldMapping customFieldMapping in (CollectionBase) CustomFieldMappingCollection.GetCustomFieldMappingCollection(Session.SessionObjects, new CustomFieldMappingCollection.Criteria(CustomFieldsType.Borrower, false)))
      {
        string fieldValue = (string) null;
        try
        {
          fieldValue = Session.LoanData.GetField(customFieldMapping.LoanFieldId);
          Tracing.Log(RxBorrowerContact.sw, TraceLevel.Verbose, "Custom Field Mapping", string.Format("CustomFieldMapping: CategoryId='{0}', FieldNumber='{1}', FieldFormat='{2}', LoanFieldId='{3}', FieldValue='{4}'", (object) customFieldMapping.CategoryId, (object) customFieldMapping.FieldNumber, (object) customFieldMapping.FieldFormat, (object) customFieldMapping.LoanFieldId, (object) fieldValue));
        }
        catch (Exception ex)
        {
          Tracing.Log(RxBorrowerContact.sw, TraceLevel.Info, "Custom Field Mapping", string.Format("Loan Field ID '{2}', Value '{1}' to Business Contact 'Custom Field {0}' failed.", (object) customFieldMapping.FieldNumber.ToString(), fieldValue == null ? (object) "UNKNOWN" : (object) fieldValue, (object) customFieldMapping.LoanFieldId));
          fieldValue = (string) null;
        }
        if (fieldValue != null)
        {
          ContactCustomField contactCustomField = new ContactCustomField(borrower, customFieldMapping.FieldNumber, this.loID, fieldValue);
          contactCustomFieldList.Add(contactCustomField);
        }
      }
      if (contactCustomFieldList.Count != 0)
      {
        foreach (ContactCustomField contactCustomField in contactCustomFieldList)
        {
          contactCustomField.ContactID = borrower;
          contactCustomField.OwnerID = this.loID;
        }
        Session.ContactManager.UpdateCustomFieldsForContact(borrower, EllieMae.EMLite.ContactUI.ContactType.Borrower, contactCustomFieldList.ToArray());
      }
      return borrower;
    }

    private void btnGoTo_Click(object sender, EventArgs e)
    {
      this.goToContact = true;
      this.selectedContact = new ContactInfo(string.Concat((object) this.borObj.ContactID), CategoryType.Borrower);
      this.DialogResult = DialogResult.OK;
    }

    public bool GoToContact => this.goToContact;

    public ContactInfo SelectedContactInfo => this.selectedContact;

    private void RxBorrowerContact_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult == DialogResult.OK || this.btnCancel.Visible || this.forceClose)
        return;
      e.Cancel = true;
    }

    private void gvContactList_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!this.btnLink.Visible)
        return;
      this.btnLink.PerformClick();
    }

    private void btnImport_Click(object sender, EventArgs e)
    {
      if (this.gvContactList.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Please select a borrower contact to import.");
      }
      else
      {
        this.borObj = Session.ContactManager.GetBorrower(((BorrowerSummaryInfo) this.gvContactList.SelectedItems[0].Tag).ContactID);
        this.DialogResult = DialogResult.OK;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (RxBorrowerContact));
      this.groupContainer1 = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.gvContactList = new GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.gcDetail = new GroupContainer();
      this.panel2 = new Panel();
      this.tabControl1 = new TabControl();
      this.tpDetails = new TabPage();
      this.panel3 = new Panel();
      this.groupContainer2 = new GroupContainer();
      this.txtBoxHomeState = new TextBox();
      this.txtBoxFirstName = new TextBox();
      this.txtBoxHomeAddress2 = new TextBox();
      this.txtBoxHomeAddress1 = new TextBox();
      this.lblFirstName = new Label();
      this.lblHomeZip = new Label();
      this.lblSSN = new Label();
      this.lblHomeCity = new Label();
      this.txtSuffixName = new TextBox();
      this.lblHomeState = new Label();
      this.txtBoxSSN = new TextBox();
      this.lblHomeAddress1 = new Label();
      this.txtMiddleName = new TextBox();
      this.txtBoxHomeZip = new TextBox();
      this.txtBoxSalutation = new TextBox();
      this.lblHomeAddress2 = new Label();
      this.label3 = new Label();
      this.txtBoxHomeCity = new TextBox();
      this.label5 = new Label();
      this.label2 = new Label();
      this.lblLastName = new Label();
      this.txtBoxLastName = new TextBox();
      this.groupContainer3 = new GroupContainer();
      this.label7 = new Label();
      this.chkBoxMarried = new CheckBox();
      this.lblSpouse = new Label();
      this.txtBoxSpouseName = new TextBox();
      this.txtBoxAnniversary = new TextBox();
      this.lblAnniversary = new Label();
      this.lblBirthday = new Label();
      this.txtBoxBirthdate = new TextBox();
      this.lblWorkPhone = new Label();
      this.lblHomePhone = new Label();
      this.chkBoxNoFax = new CheckBox();
      this.lblWorkEmail = new Label();
      this.chkBoxNoSpam = new CheckBox();
      this.chkBoxNoCall = new CheckBox();
      this.txtBoxHomePhone = new TextBox();
      this.txtBoxBizEmail = new TextBox();
      this.txtBoxPersonalEmail = new TextBox();
      this.txtBoxFaxNumber = new TextBox();
      this.lblHomeEmail = new Label();
      this.lblFaxNumber = new Label();
      this.txtBoxWorkPhone = new TextBox();
      this.txtBoxMobilePhone = new TextBox();
      this.lblCellPhone = new Label();
      this.tpExtra = new TabPage();
      this.groupContainer5 = new GroupContainer();
      this.txtBoxBizZip = new TextBox();
      this.txtBoxBizState = new TextBox();
      this.lblCompany = new Label();
      this.txtBoxIncome = new TextBox();
      this.lblBizZip = new Label();
      this.lblIncome = new Label();
      this.txtBoxBizAddress2 = new TextBox();
      this.lblBizState = new Label();
      this.txtBoxJobTitle = new TextBox();
      this.txtBoxBizWebUrl = new TextBox();
      this.txtBoxBizAddress1 = new TextBox();
      this.lblWebUrl = new Label();
      this.lblTitle = new Label();
      this.txtBoxEmployerName = new TextBox();
      this.lblBizCity = new Label();
      this.txtBoxBizCity = new TextBox();
      this.lblBizAddress2 = new Label();
      this.lblBizAddress1 = new Label();
      this.groupContainer4 = new GroupContainer();
      this.label1 = new Label();
      this.chkPrimary = new CheckBox();
      this.lblBorrowerType = new Label();
      this.txtBoxLeadTransID = new NoContextMenuTextBox();
      this.chkBoxAccessPublic = new CheckBox();
      this.chkBoxAccess = new CheckBox();
      this.cmbBoxContactType = new ComboBox();
      this.label6 = new Label();
      this.lblBorrowerStatus = new Label();
      this.txtBoxReferral = new NoContextMenuTextBox();
      this.cmbBoxStatus = new ComboBox();
      this.txtBoxOwner = new TextBox();
      this.lblReferral = new Label();
      this.lblOwner = new Label();
      this.picLinked = new PictureBox();
      this.panel1 = new Panel();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnCancel = new Button();
      this.btnNewLink = new Button();
      this.btnLink = new Button();
      this.btnGoTo = new Button();
      this.navContacts = new PageListNavigator();
      this.gradientPanel3 = new GradientPanel();
      this.lblFilter = new Label();
      this.btnClear = new Button();
      this.label4 = new Label();
      this.btnImport = new Button();
      this.groupContainer1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.gcDetail.SuspendLayout();
      this.panel2.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.panel3.SuspendLayout();
      this.groupContainer2.SuspendLayout();
      this.groupContainer3.SuspendLayout();
      this.tpExtra.SuspendLayout();
      this.groupContainer5.SuspendLayout();
      this.groupContainer4.SuspendLayout();
      ((ISupportInitialize) this.picLinked).BeginInit();
      this.panel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.gradientPanel3.SuspendLayout();
      this.SuspendLayout();
      this.groupContainer1.Controls.Add((Control) this.borderPanel1);
      this.groupContainer1.Controls.Add((Control) this.collapsibleSplitter1);
      this.groupContainer1.Controls.Add((Control) this.gcDetail);
      this.groupContainer1.Controls.Add((Control) this.panel1);
      this.groupContainer1.Controls.Add((Control) this.navContacts);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 30);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(657, 631);
      this.groupContainer1.TabIndex = 6;
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.gvContactList);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(655, 218);
      this.borderPanel1.TabIndex = 31;
      this.gvContactList.AllowMultiselect = false;
      this.gvContactList.BorderStyle = BorderStyle.None;
      this.gvContactList.Dock = DockStyle.Fill;
      this.gvContactList.FilterVisible = true;
      this.gvContactList.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.gvContactList.Location = new Point(0, 0);
      this.gvContactList.Name = "gvContactList";
      this.gvContactList.Size = new Size(655, 217);
      this.gvContactList.TabIndex = 28;
      this.gvContactList.SelectedIndexChanged += new EventHandler(this.gvContactList_SelectedIndexChanged);
      this.gvContactList.ItemDoubleClick += new GVItemEventHandler(this.gvContactList_ItemDoubleClick);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.gcDetail;
      this.collapsibleSplitter1.Dock = DockStyle.Bottom;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(1, 244);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 30;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.gcDetail.AutoScroll = true;
      this.gcDetail.Controls.Add((Control) this.panel2);
      this.gcDetail.Controls.Add((Control) this.picLinked);
      this.gcDetail.Dock = DockStyle.Bottom;
      this.gcDetail.HeaderForeColor = SystemColors.ControlText;
      this.gcDetail.Location = new Point(1, 251);
      this.gcDetail.Name = "gcDetail";
      this.gcDetail.Size = new Size(655, 345);
      this.gcDetail.TabIndex = 29;
      this.gcDetail.Text = "Contact Details";
      this.panel2.AutoScroll = true;
      this.panel2.Controls.Add((Control) this.tabControl1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.panel2.Location = new Point(1, 26);
      this.panel2.Name = "panel2";
      this.panel2.Padding = new Padding(2, 2, 0, 0);
      this.panel2.Size = new Size(653, 318);
      this.panel2.TabIndex = 2;
      this.tabControl1.Controls.Add((Control) this.tpDetails);
      this.tabControl1.Controls.Add((Control) this.tpExtra);
      this.tabControl1.Dock = DockStyle.Fill;
      this.tabControl1.Location = new Point(2, 2);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.Padding = new Point(8, 3);
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new Size(651, 316);
      this.tabControl1.TabIndex = 1;
      this.tpDetails.Controls.Add((Control) this.panel3);
      this.tpDetails.Location = new Point(4, 23);
      this.tpDetails.Margin = new Padding(0);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(643, 289);
      this.tpDetails.TabIndex = 0;
      this.tpDetails.Text = "Details";
      this.tpDetails.UseVisualStyleBackColor = true;
      this.panel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel3.AutoScroll = true;
      this.panel3.Controls.Add((Control) this.groupContainer2);
      this.panel3.Controls.Add((Control) this.groupContainer3);
      this.panel3.Location = new Point(0, 2);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(639, 285);
      this.panel3.TabIndex = 73;
      this.groupContainer2.Controls.Add((Control) this.txtBoxHomeState);
      this.groupContainer2.Controls.Add((Control) this.txtBoxFirstName);
      this.groupContainer2.Controls.Add((Control) this.txtBoxHomeAddress2);
      this.groupContainer2.Controls.Add((Control) this.txtBoxHomeAddress1);
      this.groupContainer2.Controls.Add((Control) this.lblFirstName);
      this.groupContainer2.Controls.Add((Control) this.lblHomeZip);
      this.groupContainer2.Controls.Add((Control) this.lblSSN);
      this.groupContainer2.Controls.Add((Control) this.lblHomeCity);
      this.groupContainer2.Controls.Add((Control) this.txtSuffixName);
      this.groupContainer2.Controls.Add((Control) this.lblHomeState);
      this.groupContainer2.Controls.Add((Control) this.txtBoxSSN);
      this.groupContainer2.Controls.Add((Control) this.lblHomeAddress1);
      this.groupContainer2.Controls.Add((Control) this.txtMiddleName);
      this.groupContainer2.Controls.Add((Control) this.txtBoxHomeZip);
      this.groupContainer2.Controls.Add((Control) this.txtBoxSalutation);
      this.groupContainer2.Controls.Add((Control) this.lblHomeAddress2);
      this.groupContainer2.Controls.Add((Control) this.label3);
      this.groupContainer2.Controls.Add((Control) this.txtBoxHomeCity);
      this.groupContainer2.Controls.Add((Control) this.label5);
      this.groupContainer2.Controls.Add((Control) this.label2);
      this.groupContainer2.Controls.Add((Control) this.lblLastName);
      this.groupContainer2.Controls.Add((Control) this.txtBoxLastName);
      this.groupContainer2.Dock = DockStyle.Left;
      this.groupContainer2.Font = new Font("Arial", 8.25f);
      this.groupContainer2.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer2.Location = new Point(0, 0);
      this.groupContainer2.Name = "groupContainer2";
      this.groupContainer2.Size = new Size(318, 285);
      this.groupContainer2.TabIndex = 71;
      this.groupContainer2.Text = "Personal Information";
      this.txtBoxHomeState.Font = new Font("Arial", 8.25f);
      this.txtBoxHomeState.Location = new Point(70, 209);
      this.txtBoxHomeState.MaxLength = 2;
      this.txtBoxHomeState.Name = "txtBoxHomeState";
      this.txtBoxHomeState.ReadOnly = true;
      this.txtBoxHomeState.Size = new Size(28, 20);
      this.txtBoxHomeState.TabIndex = 7;
      this.txtBoxFirstName.Font = new Font("Arial", 8.25f);
      this.txtBoxFirstName.Location = new Point(70, 33);
      this.txtBoxFirstName.MaxLength = 50;
      this.txtBoxFirstName.Name = "txtBoxFirstName";
      this.txtBoxFirstName.ReadOnly = true;
      this.txtBoxFirstName.Size = new Size(233, 20);
      this.txtBoxFirstName.TabIndex = 36;
      this.txtBoxHomeAddress2.Font = new Font("Arial", 8.25f);
      this.txtBoxHomeAddress2.Location = new Point(70, 165);
      this.txtBoxHomeAddress2.MaxLength = 50;
      this.txtBoxHomeAddress2.Name = "txtBoxHomeAddress2";
      this.txtBoxHomeAddress2.ReadOnly = true;
      this.txtBoxHomeAddress2.Size = new Size(233, 20);
      this.txtBoxHomeAddress2.TabIndex = 3;
      this.txtBoxHomeAddress1.Font = new Font("Arial", 8.25f);
      this.txtBoxHomeAddress1.Location = new Point(70, 143);
      this.txtBoxHomeAddress1.MaxLength = (int) byte.MaxValue;
      this.txtBoxHomeAddress1.Name = "txtBoxHomeAddress1";
      this.txtBoxHomeAddress1.ReadOnly = true;
      this.txtBoxHomeAddress1.Size = new Size(233, 20);
      this.txtBoxHomeAddress1.TabIndex = 1;
      this.lblFirstName.AutoSize = true;
      this.lblFirstName.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblFirstName.Location = new Point(7, 36);
      this.lblFirstName.Name = "lblFirstName";
      this.lblFirstName.Size = new Size(58, 14);
      this.lblFirstName.TabIndex = 35;
      this.lblFirstName.Text = "First Name";
      this.lblHomeZip.AutoSize = true;
      this.lblHomeZip.Font = new Font("Arial", 8.25f);
      this.lblHomeZip.Location = new Point(110, 212);
      this.lblHomeZip.Name = "lblHomeZip";
      this.lblHomeZip.RightToLeft = RightToLeft.No;
      this.lblHomeZip.Size = new Size(22, 14);
      this.lblHomeZip.TabIndex = 8;
      this.lblHomeZip.Text = "Zip";
      this.lblSSN.AutoSize = true;
      this.lblSSN.Font = new Font("Arial", 8.25f);
      this.lblSSN.Location = new Point(7, 124);
      this.lblSSN.Name = "lblSSN";
      this.lblSSN.Size = new Size(28, 14);
      this.lblSSN.TabIndex = 43;
      this.lblSSN.Text = "SSN";
      this.lblHomeCity.AutoSize = true;
      this.lblHomeCity.Font = new Font("Arial", 8.25f);
      this.lblHomeCity.Location = new Point(7, 190);
      this.lblHomeCity.Name = "lblHomeCity";
      this.lblHomeCity.RightToLeft = RightToLeft.No;
      this.lblHomeCity.Size = new Size(25, 14);
      this.lblHomeCity.TabIndex = 4;
      this.lblHomeCity.Text = "City";
      this.txtSuffixName.Font = new Font("Arial", 8.25f);
      this.txtSuffixName.Location = new Point(227, 77);
      this.txtSuffixName.MaxLength = 50;
      this.txtSuffixName.Name = "txtSuffixName";
      this.txtSuffixName.ReadOnly = true;
      this.txtSuffixName.Size = new Size(76, 20);
      this.txtSuffixName.TabIndex = 41;
      this.lblHomeState.AutoSize = true;
      this.lblHomeState.Font = new Font("Arial", 8.25f);
      this.lblHomeState.Location = new Point(7, 213);
      this.lblHomeState.Name = "lblHomeState";
      this.lblHomeState.RightToLeft = RightToLeft.No;
      this.lblHomeState.Size = new Size(32, 14);
      this.lblHomeState.TabIndex = 6;
      this.lblHomeState.Text = "State";
      this.txtBoxSSN.Font = new Font("Arial", 8.25f);
      this.txtBoxSSN.Location = new Point(70, 121);
      this.txtBoxSSN.MaxLength = 12;
      this.txtBoxSSN.Name = "txtBoxSSN";
      this.txtBoxSSN.ReadOnly = true;
      this.txtBoxSSN.Size = new Size(233, 20);
      this.txtBoxSSN.TabIndex = 44;
      this.lblHomeAddress1.AutoSize = true;
      this.lblHomeAddress1.Font = new Font("Arial", 8.25f);
      this.lblHomeAddress1.Location = new Point(7, 146);
      this.lblHomeAddress1.Name = "lblHomeAddress1";
      this.lblHomeAddress1.Size = new Size(58, 14);
      this.lblHomeAddress1.TabIndex = 0;
      this.lblHomeAddress1.Text = "Address 1";
      this.txtMiddleName.Font = new Font("Arial", 8.25f);
      this.txtMiddleName.Location = new Point(70, 55);
      this.txtMiddleName.MaxLength = 50;
      this.txtMiddleName.Name = "txtMiddleName";
      this.txtMiddleName.ReadOnly = true;
      this.txtMiddleName.Size = new Size(233, 20);
      this.txtMiddleName.TabIndex = 38;
      this.txtBoxHomeZip.Font = new Font("Arial", 8.25f);
      this.txtBoxHomeZip.Location = new Point(134, 209);
      this.txtBoxHomeZip.MaxLength = 20;
      this.txtBoxHomeZip.Name = "txtBoxHomeZip";
      this.txtBoxHomeZip.ReadOnly = true;
      this.txtBoxHomeZip.Size = new Size(112, 20);
      this.txtBoxHomeZip.TabIndex = 9;
      this.txtBoxSalutation.Font = new Font("Arial", 8.25f);
      this.txtBoxSalutation.Location = new Point(70, 99);
      this.txtBoxSalutation.MaxLength = 84;
      this.txtBoxSalutation.Name = "txtBoxSalutation";
      this.txtBoxSalutation.ReadOnly = true;
      this.txtBoxSalutation.Size = new Size(233, 20);
      this.txtBoxSalutation.TabIndex = 42;
      this.lblHomeAddress2.AutoSize = true;
      this.lblHomeAddress2.Font = new Font("Arial", 8.25f);
      this.lblHomeAddress2.Location = new Point(7, 170);
      this.lblHomeAddress2.Name = "lblHomeAddress2";
      this.lblHomeAddress2.Size = new Size(58, 14);
      this.lblHomeAddress2.TabIndex = 2;
      this.lblHomeAddress2.Text = "Address 2";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f);
      this.label3.Location = new Point(185, 78);
      this.label3.Name = "label3";
      this.label3.Size = new Size(36, 14);
      this.label3.TabIndex = 69;
      this.label3.Text = "Suffix";
      this.txtBoxHomeCity.Font = new Font("Arial", 8.25f);
      this.txtBoxHomeCity.Location = new Point(70, 187);
      this.txtBoxHomeCity.MaxLength = 50;
      this.txtBoxHomeCity.Name = "txtBoxHomeCity";
      this.txtBoxHomeCity.ReadOnly = true;
      this.txtBoxHomeCity.Size = new Size(233, 20);
      this.txtBoxHomeCity.TabIndex = 5;
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f);
      this.label5.Location = new Point(7, 102);
      this.label5.Name = "label5";
      this.label5.Size = new Size(54, 14);
      this.label5.TabIndex = 40;
      this.label5.Text = "Salutation";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f);
      this.label2.Location = new Point(7, 58);
      this.label2.Name = "label2";
      this.label2.Size = new Size(37, 14);
      this.label2.TabIndex = 68;
      this.label2.Text = "Middle";
      this.lblLastName.AutoSize = true;
      this.lblLastName.Font = new Font("Arial", 8.25f);
      this.lblLastName.Location = new Point(7, 80);
      this.lblLastName.Name = "lblLastName";
      this.lblLastName.Size = new Size(58, 14);
      this.lblLastName.TabIndex = 37;
      this.lblLastName.Text = "Last Name";
      this.txtBoxLastName.Font = new Font("Arial", 8.25f);
      this.txtBoxLastName.Location = new Point(70, 77);
      this.txtBoxLastName.MaxLength = 50;
      this.txtBoxLastName.Name = "txtBoxLastName";
      this.txtBoxLastName.ReadOnly = true;
      this.txtBoxLastName.Size = new Size(109, 20);
      this.txtBoxLastName.TabIndex = 39;
      this.groupContainer3.Controls.Add((Control) this.label7);
      this.groupContainer3.Controls.Add((Control) this.chkBoxMarried);
      this.groupContainer3.Controls.Add((Control) this.lblSpouse);
      this.groupContainer3.Controls.Add((Control) this.txtBoxSpouseName);
      this.groupContainer3.Controls.Add((Control) this.txtBoxAnniversary);
      this.groupContainer3.Controls.Add((Control) this.lblAnniversary);
      this.groupContainer3.Controls.Add((Control) this.lblBirthday);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBirthdate);
      this.groupContainer3.Controls.Add((Control) this.lblWorkPhone);
      this.groupContainer3.Controls.Add((Control) this.lblHomePhone);
      this.groupContainer3.Controls.Add((Control) this.chkBoxNoFax);
      this.groupContainer3.Controls.Add((Control) this.lblWorkEmail);
      this.groupContainer3.Controls.Add((Control) this.chkBoxNoSpam);
      this.groupContainer3.Controls.Add((Control) this.chkBoxNoCall);
      this.groupContainer3.Controls.Add((Control) this.txtBoxHomePhone);
      this.groupContainer3.Controls.Add((Control) this.txtBoxBizEmail);
      this.groupContainer3.Controls.Add((Control) this.txtBoxPersonalEmail);
      this.groupContainer3.Controls.Add((Control) this.txtBoxFaxNumber);
      this.groupContainer3.Controls.Add((Control) this.lblHomeEmail);
      this.groupContainer3.Controls.Add((Control) this.lblFaxNumber);
      this.groupContainer3.Controls.Add((Control) this.txtBoxWorkPhone);
      this.groupContainer3.Controls.Add((Control) this.txtBoxMobilePhone);
      this.groupContainer3.Controls.Add((Control) this.lblCellPhone);
      this.groupContainer3.Dock = DockStyle.Right;
      this.groupContainer3.Font = new Font("Arial", 8.25f);
      this.groupContainer3.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer3.Location = new Point(321, 0);
      this.groupContainer3.Name = "groupContainer3";
      this.groupContainer3.Size = new Size(318, 285);
      this.groupContainer3.TabIndex = 72;
      this.groupContainer3.Text = "Contact and Campaign Information";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(7, 212);
      this.label7.Name = "label7";
      this.label7.Size = new Size(43, 14);
      this.label7.TabIndex = 72;
      this.label7.Text = "Married";
      this.chkBoxMarried.AutoSize = true;
      this.chkBoxMarried.Enabled = false;
      this.chkBoxMarried.Location = new Point(80, 213);
      this.chkBoxMarried.Name = "chkBoxMarried";
      this.chkBoxMarried.Size = new Size(15, 14);
      this.chkBoxMarried.TabIndex = 67;
      this.lblSpouse.AutoSize = true;
      this.lblSpouse.Location = new Point(7, 233);
      this.lblSpouse.Name = "lblSpouse";
      this.lblSpouse.Size = new Size(44, 14);
      this.lblSpouse.TabIndex = 68;
      this.lblSpouse.Text = "Spouse";
      this.txtBoxSpouseName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxSpouseName.Location = new Point(80, 230);
      this.txtBoxSpouseName.MaxLength = 50;
      this.txtBoxSpouseName.Name = "txtBoxSpouseName";
      this.txtBoxSpouseName.ReadOnly = true;
      this.txtBoxSpouseName.Size = new Size(208, 20);
      this.txtBoxSpouseName.TabIndex = 69;
      this.txtBoxAnniversary.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxAnniversary.Location = new Point(80, 252);
      this.txtBoxAnniversary.MaxLength = 10;
      this.txtBoxAnniversary.Name = "txtBoxAnniversary";
      this.txtBoxAnniversary.ReadOnly = true;
      this.txtBoxAnniversary.Size = new Size(208, 20);
      this.txtBoxAnniversary.TabIndex = 71;
      this.lblAnniversary.AutoSize = true;
      this.lblAnniversary.Location = new Point(7, (int) byte.MaxValue);
      this.lblAnniversary.Name = "lblAnniversary";
      this.lblAnniversary.Size = new Size(67, 14);
      this.lblAnniversary.TabIndex = 70;
      this.lblAnniversary.Text = "Anniversary";
      this.lblBirthday.AutoSize = true;
      this.lblBirthday.Location = new Point(7, 190);
      this.lblBirthday.Name = "lblBirthday";
      this.lblBirthday.Size = new Size(47, 14);
      this.lblBirthday.TabIndex = 65;
      this.lblBirthday.Text = "Birthday";
      this.txtBoxBirthdate.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBirthdate.Location = new Point(80, 187);
      this.txtBoxBirthdate.MaxLength = 10;
      this.txtBoxBirthdate.Name = "txtBoxBirthdate";
      this.txtBoxBirthdate.ReadOnly = true;
      this.txtBoxBirthdate.Size = new Size(208, 20);
      this.txtBoxBirthdate.TabIndex = 66;
      this.lblWorkPhone.AutoSize = true;
      this.lblWorkPhone.Location = new Point(7, 58);
      this.lblWorkPhone.Name = "lblWorkPhone";
      this.lblWorkPhone.Size = new Size(65, 14);
      this.lblWorkPhone.TabIndex = 47;
      this.lblWorkPhone.Text = "Work Phone";
      this.lblHomePhone.AutoSize = true;
      this.lblHomePhone.Location = new Point(7, 36);
      this.lblHomePhone.Name = "lblHomePhone";
      this.lblHomePhone.Size = new Size(67, 14);
      this.lblHomePhone.TabIndex = 45;
      this.lblHomePhone.Text = "Home Phone";
      this.chkBoxNoFax.AutoSize = true;
      this.chkBoxNoFax.Enabled = false;
      this.chkBoxNoFax.Location = new Point(162, 167);
      this.chkBoxNoFax.Name = "chkBoxNoFax";
      this.chkBoxNoFax.Size = new Size(76, 18);
      this.chkBoxNoFax.TabIndex = 64;
      this.chkBoxNoFax.Text = "Do not fax";
      this.lblWorkEmail.AutoSize = true;
      this.lblWorkEmail.Location = new Point(7, 146);
      this.lblWorkEmail.Name = "lblWorkEmail";
      this.lblWorkEmail.Size = new Size(59, 14);
      this.lblWorkEmail.TabIndex = 57;
      this.lblWorkEmail.Text = "Work Email";
      this.chkBoxNoSpam.AutoSize = true;
      this.chkBoxNoSpam.Enabled = false;
      this.chkBoxNoSpam.Location = new Point(80, 167);
      this.chkBoxNoSpam.Name = "chkBoxNoSpam";
      this.chkBoxNoSpam.Size = new Size(84, 18);
      this.chkBoxNoSpam.TabIndex = 62;
      this.chkBoxNoSpam.Text = "Do not email";
      this.chkBoxNoCall.AutoSize = true;
      this.chkBoxNoCall.Enabled = false;
      this.chkBoxNoCall.Location = new Point(8, 167);
      this.chkBoxNoCall.Name = "chkBoxNoCall";
      this.chkBoxNoCall.Size = new Size(76, 18);
      this.chkBoxNoCall.TabIndex = 60;
      this.chkBoxNoCall.Text = "Do not call";
      this.txtBoxHomePhone.Location = new Point(80, 33);
      this.txtBoxHomePhone.MaxLength = 30;
      this.txtBoxHomePhone.Name = "txtBoxHomePhone";
      this.txtBoxHomePhone.ReadOnly = true;
      this.txtBoxHomePhone.Size = new Size(206, 20);
      this.txtBoxHomePhone.TabIndex = 46;
      this.txtBoxBizEmail.Location = new Point(80, 143);
      this.txtBoxBizEmail.MaxLength = 50;
      this.txtBoxBizEmail.Name = "txtBoxBizEmail";
      this.txtBoxBizEmail.ReadOnly = true;
      this.txtBoxBizEmail.Size = new Size(206, 20);
      this.txtBoxBizEmail.TabIndex = 59;
      this.txtBoxPersonalEmail.Location = new Point(80, 121);
      this.txtBoxPersonalEmail.MaxLength = 50;
      this.txtBoxPersonalEmail.Name = "txtBoxPersonalEmail";
      this.txtBoxPersonalEmail.ReadOnly = true;
      this.txtBoxPersonalEmail.Size = new Size(206, 20);
      this.txtBoxPersonalEmail.TabIndex = 55;
      this.txtBoxFaxNumber.Location = new Point(80, 99);
      this.txtBoxFaxNumber.MaxLength = 30;
      this.txtBoxFaxNumber.Name = "txtBoxFaxNumber";
      this.txtBoxFaxNumber.ReadOnly = true;
      this.txtBoxFaxNumber.Size = new Size(206, 20);
      this.txtBoxFaxNumber.TabIndex = 52;
      this.lblHomeEmail.AutoSize = true;
      this.lblHomeEmail.Location = new Point(7, 124);
      this.lblHomeEmail.Name = "lblHomeEmail";
      this.lblHomeEmail.Size = new Size(61, 14);
      this.lblHomeEmail.TabIndex = 53;
      this.lblHomeEmail.Text = "Home Email";
      this.lblFaxNumber.AutoSize = true;
      this.lblFaxNumber.Location = new Point(7, 102);
      this.lblFaxNumber.Name = "lblFaxNumber";
      this.lblFaxNumber.Size = new Size(65, 14);
      this.lblFaxNumber.TabIndex = 51;
      this.lblFaxNumber.Text = "Fax Number";
      this.txtBoxWorkPhone.Location = new Point(80, 55);
      this.txtBoxWorkPhone.MaxLength = 30;
      this.txtBoxWorkPhone.Name = "txtBoxWorkPhone";
      this.txtBoxWorkPhone.ReadOnly = true;
      this.txtBoxWorkPhone.Size = new Size(206, 20);
      this.txtBoxWorkPhone.TabIndex = 48;
      this.txtBoxMobilePhone.Location = new Point(80, 77);
      this.txtBoxMobilePhone.MaxLength = 30;
      this.txtBoxMobilePhone.Name = "txtBoxMobilePhone";
      this.txtBoxMobilePhone.ReadOnly = true;
      this.txtBoxMobilePhone.Size = new Size(206, 20);
      this.txtBoxMobilePhone.TabIndex = 50;
      this.lblCellPhone.AutoSize = true;
      this.lblCellPhone.Location = new Point(7, 80);
      this.lblCellPhone.Name = "lblCellPhone";
      this.lblCellPhone.Size = new Size(57, 14);
      this.lblCellPhone.TabIndex = 49;
      this.lblCellPhone.Text = "Cell Phone";
      this.tpExtra.AutoScroll = true;
      this.tpExtra.Controls.Add((Control) this.groupContainer5);
      this.tpExtra.Controls.Add((Control) this.groupContainer4);
      this.tpExtra.Location = new Point(4, 23);
      this.tpExtra.Margin = new Padding(0);
      this.tpExtra.Name = "tpExtra";
      this.tpExtra.Padding = new Padding(0, 2, 2, 2);
      this.tpExtra.Size = new Size(643, 289);
      this.tpExtra.TabIndex = 1;
      this.tpExtra.Text = "Extra";
      this.tpExtra.UseVisualStyleBackColor = true;
      this.groupContainer5.Controls.Add((Control) this.txtBoxBizZip);
      this.groupContainer5.Controls.Add((Control) this.txtBoxBizState);
      this.groupContainer5.Controls.Add((Control) this.lblCompany);
      this.groupContainer5.Controls.Add((Control) this.txtBoxIncome);
      this.groupContainer5.Controls.Add((Control) this.lblBizZip);
      this.groupContainer5.Controls.Add((Control) this.lblIncome);
      this.groupContainer5.Controls.Add((Control) this.txtBoxBizAddress2);
      this.groupContainer5.Controls.Add((Control) this.lblBizState);
      this.groupContainer5.Controls.Add((Control) this.txtBoxJobTitle);
      this.groupContainer5.Controls.Add((Control) this.txtBoxBizWebUrl);
      this.groupContainer5.Controls.Add((Control) this.txtBoxBizAddress1);
      this.groupContainer5.Controls.Add((Control) this.lblWebUrl);
      this.groupContainer5.Controls.Add((Control) this.lblTitle);
      this.groupContainer5.Controls.Add((Control) this.txtBoxEmployerName);
      this.groupContainer5.Controls.Add((Control) this.lblBizCity);
      this.groupContainer5.Controls.Add((Control) this.txtBoxBizCity);
      this.groupContainer5.Controls.Add((Control) this.lblBizAddress2);
      this.groupContainer5.Controls.Add((Control) this.lblBizAddress1);
      this.groupContainer5.Dock = DockStyle.Right;
      this.groupContainer5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.groupContainer5.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer5.Location = new Point(323, 2);
      this.groupContainer5.Name = "groupContainer5";
      this.groupContainer5.Size = new Size(318, 285);
      this.groupContainer5.TabIndex = 23;
      this.groupContainer5.Text = "Business Information";
      this.txtBoxBizZip.Location = new Point(174, 123);
      this.txtBoxBizZip.MaxLength = 2;
      this.txtBoxBizZip.Name = "txtBoxBizZip";
      this.txtBoxBizZip.ReadOnly = true;
      this.txtBoxBizZip.Size = new Size(84, 20);
      this.txtBoxBizZip.TabIndex = 16;
      this.txtBoxBizState.Location = new Point(91, 123);
      this.txtBoxBizState.MaxLength = 2;
      this.txtBoxBizState.Name = "txtBoxBizState";
      this.txtBoxBizState.ReadOnly = true;
      this.txtBoxBizState.Size = new Size(28, 20);
      this.txtBoxBizState.TabIndex = 11;
      this.lblCompany.AutoSize = true;
      this.lblCompany.Location = new Point(7, 36);
      this.lblCompany.Name = "lblCompany";
      this.lblCompany.Size = new Size(52, 14);
      this.lblCompany.TabIndex = 0;
      this.lblCompany.Text = "Company";
      this.txtBoxIncome.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxIncome.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtBoxIncome.Location = new Point(91, 189);
      this.txtBoxIncome.MaxLength = 18;
      this.txtBoxIncome.Name = "txtBoxIncome";
      this.txtBoxIncome.ReadOnly = true;
      this.txtBoxIncome.Size = new Size(204, 20);
      this.txtBoxIncome.TabIndex = 8;
      this.lblBizZip.Location = new Point(125, 125);
      this.lblBizZip.Name = "lblBizZip";
      this.lblBizZip.RightToLeft = RightToLeft.No;
      this.lblBizZip.Size = new Size(52, 15);
      this.lblBizZip.TabIndex = 12;
      this.lblBizZip.Text = "Zip Code";
      this.lblIncome.AutoSize = true;
      this.lblIncome.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblIncome.Location = new Point(7, 192);
      this.lblIncome.Name = "lblIncome";
      this.lblIncome.Size = new Size(78, 14);
      this.lblIncome.TabIndex = 6;
      this.lblIncome.Text = "Annual Income";
      this.txtBoxBizAddress2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizAddress2.Location = new Point(91, 79);
      this.txtBoxBizAddress2.MaxLength = 50;
      this.txtBoxBizAddress2.Name = "txtBoxBizAddress2";
      this.txtBoxBizAddress2.ReadOnly = true;
      this.txtBoxBizAddress2.Size = new Size(204, 20);
      this.txtBoxBizAddress2.TabIndex = 7;
      this.lblBizState.AutoSize = true;
      this.lblBizState.Location = new Point(7, 126);
      this.lblBizState.Name = "lblBizState";
      this.lblBizState.RightToLeft = RightToLeft.No;
      this.lblBizState.Size = new Size(32, 14);
      this.lblBizState.TabIndex = 10;
      this.lblBizState.Text = "State";
      this.txtBoxJobTitle.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxJobTitle.Location = new Point(91, 167);
      this.txtBoxJobTitle.MaxLength = 50;
      this.txtBoxJobTitle.Name = "txtBoxJobTitle";
      this.txtBoxJobTitle.ReadOnly = true;
      this.txtBoxJobTitle.Size = new Size(204, 20);
      this.txtBoxJobTitle.TabIndex = 3;
      this.txtBoxBizWebUrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizWebUrl.Location = new Point(91, 145);
      this.txtBoxBizWebUrl.MaxLength = 50;
      this.txtBoxBizWebUrl.Name = "txtBoxBizWebUrl";
      this.txtBoxBizWebUrl.ReadOnly = true;
      this.txtBoxBizWebUrl.Size = new Size(204, 20);
      this.txtBoxBizWebUrl.TabIndex = 15;
      this.txtBoxBizAddress1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizAddress1.Location = new Point(91, 57);
      this.txtBoxBizAddress1.MaxLength = (int) byte.MaxValue;
      this.txtBoxBizAddress1.Name = "txtBoxBizAddress1";
      this.txtBoxBizAddress1.ReadOnly = true;
      this.txtBoxBizAddress1.Size = new Size(204, 20);
      this.txtBoxBizAddress1.TabIndex = 5;
      this.lblWebUrl.AutoSize = true;
      this.lblWebUrl.Location = new Point(7, 148);
      this.lblWebUrl.Name = "lblWebUrl";
      this.lblWebUrl.Size = new Size(52, 14);
      this.lblWebUrl.TabIndex = 14;
      this.lblWebUrl.Text = "Web URL";
      this.lblTitle.AutoSize = true;
      this.lblTitle.Location = new Point(7, 172);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new Size(46, 14);
      this.lblTitle.TabIndex = 2;
      this.lblTitle.Text = "Job Title";
      this.txtBoxEmployerName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxEmployerName.Location = new Point(91, 35);
      this.txtBoxEmployerName.MaxLength = 50;
      this.txtBoxEmployerName.Name = "txtBoxEmployerName";
      this.txtBoxEmployerName.ReadOnly = true;
      this.txtBoxEmployerName.Size = new Size(204, 20);
      this.txtBoxEmployerName.TabIndex = 1;
      this.lblBizCity.AutoSize = true;
      this.lblBizCity.Location = new Point(7, 104);
      this.lblBizCity.Name = "lblBizCity";
      this.lblBizCity.RightToLeft = RightToLeft.No;
      this.lblBizCity.Size = new Size(25, 14);
      this.lblBizCity.TabIndex = 8;
      this.lblBizCity.Text = "City";
      this.txtBoxBizCity.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxBizCity.Location = new Point(91, 101);
      this.txtBoxBizCity.MaxLength = 50;
      this.txtBoxBizCity.Name = "txtBoxBizCity";
      this.txtBoxBizCity.ReadOnly = true;
      this.txtBoxBizCity.Size = new Size(204, 20);
      this.txtBoxBizCity.TabIndex = 9;
      this.lblBizAddress2.AutoSize = true;
      this.lblBizAddress2.Location = new Point(7, 82);
      this.lblBizAddress2.Name = "lblBizAddress2";
      this.lblBizAddress2.Size = new Size(58, 14);
      this.lblBizAddress2.TabIndex = 6;
      this.lblBizAddress2.Text = "Address 2";
      this.lblBizAddress1.AutoSize = true;
      this.lblBizAddress1.Location = new Point(7, 60);
      this.lblBizAddress1.Name = "lblBizAddress1";
      this.lblBizAddress1.Size = new Size(58, 14);
      this.lblBizAddress1.TabIndex = 4;
      this.lblBizAddress1.Text = "Address 1";
      this.groupContainer4.Controls.Add((Control) this.label1);
      this.groupContainer4.Controls.Add((Control) this.chkPrimary);
      this.groupContainer4.Controls.Add((Control) this.lblBorrowerType);
      this.groupContainer4.Controls.Add((Control) this.txtBoxLeadTransID);
      this.groupContainer4.Controls.Add((Control) this.chkBoxAccessPublic);
      this.groupContainer4.Controls.Add((Control) this.chkBoxAccess);
      this.groupContainer4.Controls.Add((Control) this.cmbBoxContactType);
      this.groupContainer4.Controls.Add((Control) this.label6);
      this.groupContainer4.Controls.Add((Control) this.lblBorrowerStatus);
      this.groupContainer4.Controls.Add((Control) this.txtBoxReferral);
      this.groupContainer4.Controls.Add((Control) this.cmbBoxStatus);
      this.groupContainer4.Controls.Add((Control) this.txtBoxOwner);
      this.groupContainer4.Controls.Add((Control) this.lblReferral);
      this.groupContainer4.Controls.Add((Control) this.lblOwner);
      this.groupContainer4.Dock = DockStyle.Left;
      this.groupContainer4.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer4.Location = new Point(0, 2);
      this.groupContainer4.Name = "groupContainer4";
      this.groupContainer4.Size = new Size(318, 285);
      this.groupContainer4.TabIndex = 22;
      this.groupContainer4.Text = "Contact Properties";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(7, 126);
      this.label1.Name = "label1";
      this.label1.Size = new Size(83, 14);
      this.label1.TabIndex = 23;
      this.label1.Text = "Primary Contact";
      this.chkPrimary.Checked = true;
      this.chkPrimary.CheckState = CheckState.Checked;
      this.chkPrimary.Enabled = false;
      this.chkPrimary.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkPrimary.Location = new Point(93, (int) sbyte.MaxValue);
      this.chkPrimary.Name = "chkPrimary";
      this.chkPrimary.Size = new Size(19, 16);
      this.chkPrimary.TabIndex = 22;
      this.lblBorrowerType.AutoSize = true;
      this.lblBorrowerType.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblBorrowerType.Location = new Point(7, 36);
      this.lblBorrowerType.Name = "lblBorrowerType";
      this.lblBorrowerType.Size = new Size(70, 14);
      this.lblBorrowerType.TabIndex = 5;
      this.lblBorrowerType.Text = "Contact Type";
      this.txtBoxLeadTransID.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtBoxLeadTransID.Location = new Point(93, 103);
      this.txtBoxLeadTransID.Name = "txtBoxLeadTransID";
      this.txtBoxLeadTransID.ReadOnly = true;
      this.txtBoxLeadTransID.Size = new Size(207, 20);
      this.txtBoxLeadTransID.TabIndex = 21;
      this.chkBoxAccessPublic.Checked = true;
      this.chkBoxAccessPublic.CheckState = CheckState.Checked;
      this.chkBoxAccessPublic.Enabled = false;
      this.chkBoxAccessPublic.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkBoxAccessPublic.Location = new Point(93, 171);
      this.chkBoxAccessPublic.Name = "chkBoxAccessPublic";
      this.chkBoxAccessPublic.Size = new Size(76, 16);
      this.chkBoxAccessPublic.TabIndex = 17;
      this.chkBoxAccessPublic.Text = "Public";
      this.chkBoxAccess.Enabled = false;
      this.chkBoxAccess.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.chkBoxAccess.Location = new Point(93, 171);
      this.chkBoxAccess.Name = "chkBoxAccess";
      this.chkBoxAccess.Size = new Size(76, 16);
      this.chkBoxAccess.TabIndex = 16;
      this.chkBoxAccess.Text = "Public";
      this.cmbBoxContactType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxContactType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxContactType.Enabled = false;
      this.cmbBoxContactType.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbBoxContactType.Location = new Point(93, 33);
      this.cmbBoxContactType.Name = "cmbBoxContactType";
      this.cmbBoxContactType.Size = new Size(209, 22);
      this.cmbBoxContactType.TabIndex = 6;
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(7, 107);
      this.label6.Name = "label6";
      this.label6.Size = new Size(71, 14);
      this.label6.TabIndex = 20;
      this.label6.Text = "Lead Trans #";
      this.lblBorrowerStatus.AutoSize = true;
      this.lblBorrowerStatus.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblBorrowerStatus.Location = new Point(7, 62);
      this.lblBorrowerStatus.Name = "lblBorrowerStatus";
      this.lblBorrowerStatus.Size = new Size(38, 14);
      this.lblBorrowerStatus.TabIndex = 7;
      this.lblBorrowerStatus.Text = "Status";
      this.txtBoxReferral.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtBoxReferral.Location = new Point(93, 81);
      this.txtBoxReferral.Name = "txtBoxReferral";
      this.txtBoxReferral.ReadOnly = true;
      this.txtBoxReferral.Size = new Size(207, 20);
      this.txtBoxReferral.TabIndex = 5;
      this.cmbBoxStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxStatus.Enabled = false;
      this.cmbBoxStatus.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbBoxStatus.Location = new Point(93, 57);
      this.cmbBoxStatus.Name = "cmbBoxStatus";
      this.cmbBoxStatus.Size = new Size(209, 22);
      this.cmbBoxStatus.TabIndex = 8;
      this.txtBoxOwner.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxOwner.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtBoxOwner.Location = new Point(93, 145);
      this.txtBoxOwner.MaxLength = 50;
      this.txtBoxOwner.Name = "txtBoxOwner";
      this.txtBoxOwner.ReadOnly = true;
      this.txtBoxOwner.Size = new Size(209, 20);
      this.txtBoxOwner.TabIndex = 19;
      this.lblReferral.AutoSize = true;
      this.lblReferral.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblReferral.Location = new Point(7, 84);
      this.lblReferral.Name = "lblReferral";
      this.lblReferral.Size = new Size(46, 14);
      this.lblReferral.TabIndex = 3;
      this.lblReferral.Text = "Referral";
      this.lblOwner.AutoSize = true;
      this.lblOwner.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblOwner.Location = new Point(7, 148);
      this.lblOwner.Name = "lblOwner";
      this.lblOwner.Size = new Size(81, 14);
      this.lblOwner.TabIndex = 18;
      this.lblOwner.Text = "Contact Owner";
      this.picLinked.BackColor = Color.Transparent;
      this.picLinked.Image = (Image) Resources.link;
      this.picLinked.InitialImage = (Image) componentResourceManager.GetObject("picLinked.InitialImage");
      this.picLinked.Location = new Point(99, 6);
      this.picLinked.Name = "picLinked";
      this.picLinked.Size = new Size(19, 14);
      this.picLinked.TabIndex = 1;
      this.picLinked.TabStop = false;
      this.panel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.panel1.Controls.Add((Control) this.btnGoTo);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(1, 596);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(655, 34);
      this.panel1.TabIndex = 2;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnCancel);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnNewLink);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnLink);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnImport);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(263, 6);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Padding = new Padding(0, 0, 10, 0);
      this.flowLayoutPanel1.Size = new Size(392, 22);
      this.flowLayoutPanel1.TabIndex = 6;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(307, 0);
      this.btnCancel.Margin = new Padding(5, 0, 0, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnNewLink.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnNewLink.BackColor = SystemColors.Control;
      this.btnNewLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnNewLink.Location = new Point(173, 0);
      this.btnNewLink.Margin = new Padding(5, 0, 0, 0);
      this.btnNewLink.Name = "btnNewLink";
      this.btnNewLink.Size = new Size(129, 22);
      this.btnNewLink.TabIndex = 4;
      this.btnNewLink.Text = "Create New and Link";
      this.btnNewLink.UseVisualStyleBackColor = true;
      this.btnNewLink.Click += new EventHandler(this.btnNewLink_Click);
      this.btnLink.BackColor = SystemColors.Control;
      this.btnLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnLink.Location = new Point(93, 0);
      this.btnLink.Margin = new Padding(0);
      this.btnLink.Name = "btnLink";
      this.btnLink.Size = new Size(75, 22);
      this.btnLink.TabIndex = 3;
      this.btnLink.Text = "Link";
      this.btnLink.UseVisualStyleBackColor = true;
      this.btnLink.Click += new EventHandler(this.btnLink_Click);
      this.btnGoTo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnGoTo.BackColor = SystemColors.Control;
      this.btnGoTo.Enabled = false;
      this.btnGoTo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnGoTo.Location = new Point(5, 6);
      this.btnGoTo.Name = "btnGoTo";
      this.btnGoTo.Size = new Size(94, 22);
      this.btnGoTo.TabIndex = 5;
      this.btnGoTo.Text = "Go To Contact";
      this.btnGoTo.UseVisualStyleBackColor = true;
      this.btnGoTo.Click += new EventHandler(this.btnGoTo_Click);
      this.navContacts.BackColor = Color.Transparent;
      this.navContacts.Font = new Font("Arial", 8f);
      this.navContacts.ItemsPerPage = 100;
      this.navContacts.Location = new Point(0, 2);
      this.navContacts.Name = "navContacts";
      this.navContacts.NumberOfItems = 0;
      this.navContacts.Size = new Size(254, 22);
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
      this.gradientPanel3.Size = new Size(657, 30);
      this.gradientPanel3.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gradientPanel3.TabIndex = 5;
      this.lblFilter.AutoEllipsis = true;
      this.lblFilter.BackColor = Color.Transparent;
      this.lblFilter.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblFilter.Location = new Point(34, 8);
      this.lblFilter.Name = "lblFilter";
      this.lblFilter.Size = new Size(555, 18);
      this.lblFilter.TabIndex = 7;
      this.lblFilter.Text = "label8";
      this.btnClear.BackColor = SystemColors.Control;
      this.btnClear.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnClear.Location = new Point(595, 4);
      this.btnClear.Name = "btnClear";
      this.btnClear.Size = new Size(56, 22);
      this.btnClear.TabIndex = 6;
      this.btnClear.Text = "Clear";
      this.btnClear.UseVisualStyleBackColor = true;
      this.btnClear.Click += new EventHandler(this.btnClear_Click);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(5, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(33, 14);
      this.label4.TabIndex = 3;
      this.label4.Text = "Filter:";
      this.btnImport.BackColor = SystemColors.Control;
      this.btnImport.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnImport.Location = new Point(18, 0);
      this.btnImport.Margin = new Padding(0);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new Size(75, 22);
      this.btnImport.TabIndex = 5;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new EventHandler(this.btnImport_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(657, 661);
      this.Controls.Add((Control) this.groupContainer1);
      this.Controls.Add((Control) this.gradientPanel3);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RxBorrowerContact);
      this.Text = "Borrower Contacts";
      this.FormClosing += new FormClosingEventHandler(this.RxBorrowerContact_FormClosing);
      this.groupContainer1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.gcDetail.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.groupContainer2.ResumeLayout(false);
      this.groupContainer2.PerformLayout();
      this.groupContainer3.ResumeLayout(false);
      this.groupContainer3.PerformLayout();
      this.tpExtra.ResumeLayout(false);
      this.groupContainer5.ResumeLayout(false);
      this.groupContainer5.PerformLayout();
      this.groupContainer4.ResumeLayout(false);
      this.groupContainer4.PerformLayout();
      ((ISupportInitialize) this.picLinked).EndInit();
      this.panel1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.gradientPanel3.ResumeLayout(false);
      this.gradientPanel3.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
