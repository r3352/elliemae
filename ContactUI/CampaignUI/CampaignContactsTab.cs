// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignContactsTab
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Calendar;
using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignContactsTab : Form
  {
    private CampaignData campaignData;
    private ReportFieldDefs contactFieldDefs;
    private bool isListUpdatePending;
    private ICursor contactCursor;
    private SortField[] sortFields = new SortField[1]
    {
      new SortField("CreatedDate", FieldSortOrder.Descending)
    };
    private GridViewLayoutManager gvLayoutManager;
    private GridViewReportFilterManager gvFilterManager;
    private TableLayout borrowerTableLayout;
    private FlowLayoutPanel flowLayoutPanel1;
    private ToolTip toolTip1;
    private ContextMenuStrip ctxContacts;
    private ToolStripSeparator toolStripMenuItem1;
    private ToolStripMenuItem tsmiView;
    private ToolStripSeparator toolStripMenuItem2;
    private ToolStripMenuItem tsmiSelectAll;
    private ToolStripMenuItem tsmiAddContacts;
    private ToolStripMenuItem tsmiRemove;
    private ToolStripMenuItem tsmiAddtoGroup;
    private TableLayout bizPartnerTableLayout;
    private string[] operators = new string[3]
    {
      "less than",
      "equal to",
      "greater than"
    };
    private string[] loanTypes = new string[6]
    {
      "Conventional",
      "FHA",
      "VA",
      "USDA-RHS",
      "HELOC",
      "Other"
    };
    private string[] loanPurposes = new string[6]
    {
      "Purchase",
      "Cash Out Refi",
      "No Cash Out Refi",
      "Construction",
      "Construction - Permanent",
      "Other"
    };
    private IContainer components;
    private Button btnAddToGroup;
    private Button btnModify;
    private GradientPanel pnlManageContacts;
    private Label lblManageContacts;
    private GradientPanel pnlAddContacts;
    private Label lblAddQuery;
    private GradientPanel pnlRemoveContacts;
    private Label lblRemoveQuery;
    private TableContainer pnlContactTable;
    private PageListNavigator navContacts;
    private StandardIconButton icnAddContacts;
    private StandardIconButton icnRemoveContacts;
    private VerticalSeparator verticalSeparator1;
    private GridView gvContacts;
    private ImageList imgList;

    public event CampaignDetailForm.UpdateTaskCount UpdateTaskCount;

    public CampaignContactsTab()
    {
      this.InitializeComponent();
      this.campaignData = CampaignData.GetCampaignData();
    }

    public void PopulateControls()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      this.lblAddQuery.Text = this.buildQuerySummary(true);
      this.lblRemoveQuery.Text = this.buildQuerySummary(false);
      this.btnModify.Enabled = CampaignStatus.NotStarted != activeCampaign.Status;
      this.icnAddContacts.Enabled = CampaignStatus.NotStarted != activeCampaign.Status;
      this.icnRemoveContacts.Enabled = false;
      this.btnAddToGroup.Enabled = false;
      if (activeCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        this.resetContactFieldDefs(EllieMae.EMLite.ContactUI.ContactType.Borrower);
        this.gvLayoutManager = this.createGVLayoutMgr(EllieMae.EMLite.ContactUI.ContactType.Borrower);
        if (this.borrowerTableLayout != null)
          this.gvLayoutManager.ApplyLayout(this.borrowerTableLayout, false);
        else
          this.gvLayoutManager.ApplyLayout(this.getBorrowerTableLayout(false), false);
      }
      else
      {
        this.resetContactFieldDefs(EllieMae.EMLite.ContactUI.ContactType.BizPartner);
        this.gvLayoutManager = this.createGVLayoutMgr(EllieMae.EMLite.ContactUI.ContactType.BizPartner);
        if (this.bizPartnerTableLayout != null)
          this.gvLayoutManager.ApplyLayout(this.bizPartnerTableLayout, false);
        else
          this.gvLayoutManager.ApplyLayout(this.getBizPartnerTableLayout(false), false);
      }
      this.gvLayoutManager.LayoutChanged += new EventHandler(this.gvLayoutManager_LayoutChanged);
      this.gvFilterManager = new GridViewReportFilterManager(Session.DefaultInstance, this.gvContacts);
      this.gvFilterManager.FilterChanged += new EventHandler(this.gvFilterManager_FilterChanged);
      this.gvFilterManager.CreateColumnFilters(this.contactFieldDefs);
      this.getCampaignContacts();
      if (this.UpdateTaskCount == null)
        return;
      this.UpdateTaskCount();
    }

    private void resetContactFieldDefs(EllieMae.EMLite.ContactUI.ContactType type)
    {
      if (type == EllieMae.EMLite.ContactUI.ContactType.Borrower)
      {
        this.contactFieldDefs = (ReportFieldDefs) BorrowerReportFieldDefs.GetFieldDefs(true);
        this.contactFieldDefs.Add((ReportFieldDef) new BorrowerReportFieldDef("Borrower", "CreatedDate", "Created Date", "Date added to campaign", FieldFormat.DATE, "CampaignActivity.CreatedDate"));
      }
      else
      {
        this.contactFieldDefs = (ReportFieldDefs) BizPartnerReportFieldDefs.GetFieldDefs(true, EllieMae.EMLite.ContactUI.ContactType.BizPartner);
        this.contactFieldDefs.Add((ReportFieldDef) new BizPartnerReportFieldDef("BizPartner", "CreatedDate", "Created Date", "Date added to campaign", FieldFormat.DATE, "CampaignActivity.CreatedDate"));
      }
    }

    private GridViewLayoutManager createGVLayoutMgr(EllieMae.EMLite.ContactUI.ContactType type)
    {
      if (this.gvLayoutManager == null)
        return type == EllieMae.EMLite.ContactUI.ContactType.Borrower ? new GridViewLayoutManager(this.gvContacts, this.getBorrowerTableLayout(true), this.getBorrowerTableLayout(false)) : new GridViewLayoutManager(this.gvContacts, this.getBizPartnerTableLayout(true), this.getBizPartnerTableLayout(false));
      this.gvLayoutManager.AllColumns = type != EllieMae.EMLite.ContactUI.ContactType.Borrower ? this.getBizPartnerTableLayout(true) : this.getBorrowerTableLayout(true);
      this.resetContactFieldDefs(type);
      this.gvFilterManager.ReleaseFilterControls();
      this.gvFilterManager.CreateColumnFilters(this.contactFieldDefs);
      return this.gvLayoutManager;
    }

    private string buildQuerySummary(bool addQuery)
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(addQuery ? "Add Contacts: " : "Remove Contacts: ");
      if (addQuery && (CampaignType.AutoAddQuery & activeCampaign.CampaignType) != CampaignType.AutoAddQuery || !addQuery && (CampaignType.AutoDeleteQuery & activeCampaign.CampaignType) != CampaignType.AutoDeleteQuery)
      {
        stringBuilder.Append("Manual");
      }
      else
      {
        stringBuilder.Append("Automatic   Run Search: ");
        if (CampaignFrequencyType.Custom == activeCampaign.FrequencyType)
          stringBuilder.Append("Every " + (object) activeCampaign.FrequencyInterval + " days");
        else
          stringBuilder.Append(new CampaignFrequencyNameProvider().GetName((object) activeCampaign.FrequencyType));
        if (DateTime.MinValue != activeCampaign.LastActivityDate)
          stringBuilder.Append("   Last Update: " + activeCampaign.LastActivityDate.ToShortDateString());
        EllieMae.EMLite.ContactGroup.ContactQuery query = addQuery ? activeCampaign.AddQuery : activeCampaign.DeleteQuery;
        if ((query.QueryType & ContactQueryType.CampaignAdvancedQuery) == ContactQueryType.CampaignAdvancedQuery)
        {
          FieldFilterList fieldFilterList = (FieldFilterList) QueryXmlConverter.Deserialize(typeof (FieldFilterList), query.XmlQueryString);
          stringBuilder.Append("     Filters:" + fieldFilterList.ToString(false));
        }
        else
          stringBuilder.Append("     Filters:" + this.getPredefinedQueryDescription(query));
      }
      return stringBuilder.ToString();
    }

    private string getPredefinedQueryDescription(EllieMae.EMLite.ContactGroup.ContactQuery query)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(query.XmlQueryString);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string attribute = documentElement.GetAttribute("Name");
      ArrayList parms = new ArrayList();
      for (int i = 0; i < documentElement.ChildNodes.Count; ++i)
        parms.Add((object) ((XmlElement) documentElement.ChildNodes[i]).GetAttribute("Value"));
      switch (attribute)
      {
        case "Anniversary":
          return this.getBorrowerAnniversary(parms[0].ToString());
        case "Birthday":
          return "Borrowers whose birthday is in the " + parms[0].ToString();
        case "Borrower Contact Status":
          return this.getBorrowerStatus(parms[0].ToString());
        case "Borrower Contact Type":
          return this.getBorrowerType(parms[0].ToString());
        case "Business Contact Category":
          return this.getPartnerCategory(new BizCategoryUtil(Session.SessionObjects).CategoryIdToName(int.Parse(parms[0].ToString())));
        case "Imported or Newly Created Borrower Contacts":
          return this.getNewContactDesc(query, int.Parse(parms[0].ToString()));
        case "Imported or Newly Created Business Contacts":
          return this.getNewContactDesc(query, int.Parse(parms[0].ToString()));
        case "Last Loan Closed":
          return this.getLastLoanClosed(parms);
        case "Last Loan Originated":
          return "Borrowers whose last loan was originated in the previous " + parms[0].ToString() + " days.";
        default:
          return "";
      }
    }

    private string getLastLoanClosed(ArrayList parms)
    {
      string lastLoanClosed = "Borrowers whose last closed loan had a " + parms[0].ToString();
      switch (parms[0].ToString())
      {
        case "Completion Date":
          lastLoanClosed = lastLoanClosed + " which occurred in the previous " + (object) (2 <= parms.Count ? int.Parse(parms[1].ToString()) : 1) + " days";
          break;
        case "Loan Amount":
          lastLoanClosed = lastLoanClosed + " " + (2 <= parms.Count ? (object) parms[1].ToString() : (object) this.operators[2]) + " " + (object) (3 <= parms.Count ? Decimal.Parse(parms[2].ToString()) : 0M) + " dollars";
          break;
        case "Interest Rate":
          lastLoanClosed = lastLoanClosed + " " + (2 <= parms.Count ? (object) parms[1].ToString() : (object) this.operators[2]) + " " + (object) (3 <= parms.Count ? Decimal.Parse(parms[2].ToString()) : 0M) + " percent";
          break;
        case "Loan Type":
          lastLoanClosed = lastLoanClosed + " of " + (2 <= parms.Count ? parms[1].ToString() : this.loanTypes[0]);
          break;
        case "Loan Purpose":
          lastLoanClosed = lastLoanClosed + " of " + (2 <= parms.Count ? parms[1].ToString() : this.loanPurposes[0]);
          break;
      }
      return lastLoanClosed;
    }

    private string getPartnerCategory(string partnerCategory)
    {
      List<string> stringList = new List<string>();
      foreach (BizCategory bizCategory in Session.ContactManager.GetBizCategories())
      {
        if (!(string.Empty == bizCategory.Name))
          stringList.Add(bizCategory.Name);
      }
      return "Business contacts whose contact category is " + partnerCategory;
    }

    private string getNewContactDesc(EllieMae.EMLite.ContactGroup.ContactQuery query, int days)
    {
      return (query.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "Borrowers" : "Business contacts") + " imported or created in the previous " + (object) days + " days";
    }

    private string getBorrowerAnniversary(string condition)
    {
      List<string> stringList = new List<string>();
      foreach (string str in MonthWeekEnumUtil.GetDisplayNamesNoExactDate())
      {
        if (string.Empty != str)
          stringList.Add(str);
      }
      return "Borrowers whose anniversary is in the " + condition;
    }

    private string getBorrowerType(string borrowerType)
    {
      List<string> stringList = new List<string>();
      foreach (object displayName in BorrowerTypeEnumUtil.GetDisplayNames())
      {
        if (!(string.Empty == displayName.ToString()))
          stringList.Add(displayName.ToString());
      }
      if (0 < stringList.Count)
        return "Borrowers whose contact type is " + borrowerType;
      int num = (int) Utils.Dialog((IWin32Window) this, "There are currently no borrower types defined for selection.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return "";
    }

    private string getBorrowerStatus(string borrowerStatus)
    {
      List<string> stringList = new List<string>();
      foreach (BorrowerStatusItem borrowerStatusItem in Session.ContactManager.GetBorrowerStatus().Items)
      {
        if (!(string.Empty == borrowerStatusItem.name))
          stringList.Add(borrowerStatusItem.name);
      }
      if (0 < stringList.Count)
        return "Borrowers whose contact status is " + borrowerStatus;
      int num = (int) Utils.Dialog((IWin32Window) this, "There are currently no borrower status codes defined for selection.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      return "";
    }

    private TableLayout getBizPartnerTableLayout(bool fullLayout)
    {
      TableLayout partnerTableLayout = new TableLayout();
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.LastName", "Last Name", HorizontalAlignment.Left, 100));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.FirstName", "First Name", HorizontalAlignment.Left, 100));
      partnerTableLayout.AddColumn(new TableLayout.Column("CampaignActivity.CreatedDate", "Date Added", HorizontalAlignment.Left, 85));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.WorkPhone", "Work Phone", HorizontalAlignment.Left, 90));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.BizEmail", "Work Email", HorizontalAlignment.Left, 95));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.BizAddress1", "Address 1", HorizontalAlignment.Left, 91));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.BizCity", "City", HorizontalAlignment.Left, 100));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.BizState", "State", HorizontalAlignment.Left, 50));
      partnerTableLayout.AddColumn(new TableLayout.Column("Contact.FaxNumber", "Fax Number", HorizontalAlignment.Left, 87));
      partnerTableLayout.AddColumn(new TableLayout.Column("contactgroupcount.GroupCount", "Groups", HorizontalAlignment.Left, 50));
      partnerTableLayout.AddColumn(new TableLayout.Column("CampaignActivity.Activity", "Activity", HorizontalAlignment.Center, 50));
      if (fullLayout)
      {
        partnerTableLayout.AddColumn(new TableLayout.Column("Contact.BizAddress2", "Address 2", HorizontalAlignment.Left, 91));
        partnerTableLayout.AddColumn(new TableLayout.Column("Contact.BizZip", "Zip Code", HorizontalAlignment.Left, 100));
        partnerTableLayout.AddColumn(new TableLayout.Column("Contact.DoNotSpam", "Do not email", HorizontalAlignment.Left, 100));
      }
      return partnerTableLayout;
    }

    private TableLayout getBorrowerTableLayout(bool fullLayout)
    {
      TableLayout borrowerTableLayout = new TableLayout();
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.LastName", "Last Name", HorizontalAlignment.Left, 100));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.FirstName", "First Name", HorizontalAlignment.Left, 100));
      borrowerTableLayout.AddColumn(new TableLayout.Column("CampaignActivity.CreatedDate", "Date Added", HorizontalAlignment.Left, 85));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.HomePhone", "Home Phone", HorizontalAlignment.Left, 90));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.PersonalEmail", "Home Email", HorizontalAlignment.Left, 95));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.HomeAddress1", "Address 1", HorizontalAlignment.Left, 91));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.HomeCity", "City", HorizontalAlignment.Left, 100));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.HomeState", "State", HorizontalAlignment.Left, 50));
      borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.FaxNumber", "Fax Number", HorizontalAlignment.Left, 87));
      borrowerTableLayout.AddColumn(new TableLayout.Column("contactgroupcount.GroupCount", "Groups", HorizontalAlignment.Left, 50));
      borrowerTableLayout.AddColumn(new TableLayout.Column("CampaignActivity.Activity", "Activity", HorizontalAlignment.Center, 50));
      if (fullLayout)
      {
        borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.HomeAddress2", "Address 2", HorizontalAlignment.Left, 91));
        borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.HomeZip", "Zip Code", HorizontalAlignment.Left, 100));
        borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.DoNotSpam", "Do not email", HorizontalAlignment.Left, 100));
        borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.DoNotCall", "Do not call", HorizontalAlignment.Left, 100));
        borrowerTableLayout.AddColumn(new TableLayout.Column("Contact.DoNotFax", "Do not fax", HorizontalAlignment.Left, 100));
      }
      return borrowerTableLayout;
    }

    private void getCampaignContacts()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      Cursor.Current = Cursors.WaitCursor;
      try
      {
        if (this.contactCursor != null)
          this.contactCursor.Dispose();
        this.contactCursor = Session.CampaignManager.OpenCampaignContactCursor(new CampaignContactCollectionCritera(activeCampaign.CampaignId)
        {
          FieldList = this.generateFieldList(),
          FilterCriteria = this.generateFilterCriteria(),
          SortFields = this.generateSortFields()
        });
        this.navContacts.NumberOfItems = this.contactCursor.GetItemCount();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error loading Campaign Contacts: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private string[] generateFieldList()
    {
      List<string> stringList = new List<string>();
      foreach (TableLayout.Column column in this.gvLayoutManager.GetCurrentLayout())
      {
        ReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(column.ColumnID);
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

    private QueryCriterion generateFilterCriteria()
    {
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      QueryCriterion filterCriteria = (QueryCriterion) new OrdinalValueCriterion("CampaignActivity.Status", (object) 3, OrdinalMatchType.Equals);
      QueryCriterion queryCriterion = this.gvFilterManager.ToQueryCriterion();
      if (queryCriterion != null)
        filterCriteria = filterCriteria.And(queryCriterion);
      return filterCriteria;
    }

    private SortField[] generateSortFields()
    {
      Dictionary<int, SortField> dictionary = new Dictionary<int, SortField>();
      foreach (GVColumn column in this.gvContacts.Columns)
      {
        if (column.SortOrder != SortOrder.None)
        {
          TableLayout.Column tag = (TableLayout.Column) column.Tag;
          if (!(tag.ColumnID == "CampaignActivity.Activity"))
            dictionary[column.SortPriority] = new SortField(tag.ColumnID, SortOrder.Ascending == column.SortOrder ? FieldSortOrder.Ascending : FieldSortOrder.Descending);
        }
      }
      List<SortField> sortFieldList = new List<SortField>();
      if (this.sortFields != null && this.sortFields.Length != 0)
      {
        sortFieldList.Add(this.sortFields[0]);
        if (dictionary.ContainsKey(0) && sortFieldList[0].Term.FieldName != dictionary[0].Term.FieldName)
          sortFieldList.Add(dictionary[0]);
        if (sortFieldList.Count < 2 && dictionary.ContainsKey(1) && sortFieldList[0].Term.FieldName != dictionary[1].Term.FieldName)
          sortFieldList.Add(dictionary[1]);
      }
      else
      {
        if (dictionary.ContainsKey(1))
          sortFieldList.Add(dictionary[1]);
        if (dictionary.ContainsKey(0))
          sortFieldList.Add(dictionary[0]);
      }
      return sortFieldList.ToArray();
    }

    private void displayCampaignContacts(int itemIndex, int itemCount)
    {
      this.Cursor = Cursors.WaitCursor;
      this.gvContacts.BeginUpdate();
      this.icnRemoveContacts.Enabled = false;
      this.btnAddToGroup.Enabled = false;
      try
      {
        int contactId = 1 == this.gvContacts.SelectedItems.Count ? ((CampaignContactInfo) this.gvContacts.SelectedItems[0].Tag).ContactId : 0;
        this.gvContacts.Items.Clear();
        if (-1 == itemIndex || itemCount == 0)
          return;
        object[] items = this.contactCursor.GetItems(itemIndex, itemCount);
        if (items.Length == 0)
          return;
        GVItem gvItem1 = (GVItem) null;
        foreach (object obj in items)
        {
          if (obj is CampaignContactInfo contactInfo)
          {
            GVItem gvItem2 = this.createGVItem(contactInfo);
            if (gvItem1 == null)
              gvItem1 = gvItem2;
            if (contactId == ((CampaignContactInfo) gvItem2.Tag).ContactId)
              gvItem1 = gvItem2;
            this.gvContacts.Items.Add(gvItem2);
          }
        }
        if (gvItem1 == null)
          return;
        gvItem1.Selected = true;
        this.gvContacts.EnsureVisible(gvItem1.Index);
      }
      finally
      {
        this.icnRemoveContacts.Enabled = 0 < this.gvContacts.SelectedItems.Count;
        this.btnAddToGroup.Enabled = 0 < this.gvContacts.SelectedItems.Count;
        this.gvContacts.EndUpdate();
        this.Cursor = Cursors.Default;
      }
    }

    private GVItem createGVItem(CampaignContactInfo contactInfo)
    {
      GVItem gvItem = new GVItem();
      gvItem.Tag = (object) contactInfo;
      for (int index = 0; index < this.gvContacts.Columns.Count; ++index)
      {
        string columnId = ((TableLayout.Column) this.gvContacts.Columns[index].Tag).ColumnID;
        object obj = (object) null;
        if ("CampaignActivity.Activity" == columnId)
        {
          if (contactInfo.ContactActivities.Length != 0)
          {
            ImageLink imageLink = new ImageLink((Element) null, (Image) Resources.campaign_activity, (Image) Resources.campaign_activity_over, new EventHandler(this.lnkActivity_Click));
            imageLink.Tag = (object) contactInfo;
            obj = (object) imageLink;
          }
        }
        else if ("contactgroupcount.GroupCount" == columnId)
        {
          ContactGroupsLink contactGroupsLink = new ContactGroupsLink((int) contactInfo["contactgroupcount.GroupCount"], new EventHandler(this.lnkGroups_Click));
          contactGroupsLink.Tag = (object) contactInfo;
          obj = (object) contactGroupsLink;
        }
        else
        {
          ReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(columnId);
          obj = (object) string.Concat(contactInfo[columnId]);
          if (fieldByCriterionName != null)
            obj = (object) fieldByCriterionName.ToDisplayValue(obj.ToString());
        }
        gvItem.SubItems[index].Value = obj;
      }
      return gvItem;
    }

    private void lnkGroups_Click(object sender, EventArgs e)
    {
      if (!(sender is ContactGroupsLink contactGroupsLink) || !(contactGroupsLink.Tag is CampaignContactInfo tag))
        return;
      Point position = Cursor.Position;
      this.createGroupsPopup(tag)?.Show(position);
    }

    private void lnkActivity_Click(object sender, EventArgs e)
    {
      if (!(sender is ImageLink imageLink) || !(imageLink.Tag is CampaignContactInfo tag))
        return;
      Point position = Cursor.Position;
      this.createActivityPopup(tag)?.Show(position);
    }

    private ContextMenuStrip createGroupsPopup(CampaignContactInfo contactInfo)
    {
      if (this.campaignData.ActiveCampaign == null)
        return (ContextMenuStrip) null;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      ContextMenuStrip groupsPopup = new ContextMenuStrip();
      groupsPopup.ShowImageMargin = false;
      ObjectWithImage dataSource = new ObjectWithImage("Contact Groups", (Image) Resources.campaign_activity);
      groupsPopup.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) dataSource, ToolStripMenuItemEx.ToolStripItemType.Header));
      ContactGroupInfo[] groupsForContact = Session.ContactGroupManager.GetContactGroupsForContact(activeCampaign.ContactType, contactInfo.ContactId);
      int num = 1;
      foreach (ContactGroupInfo contactGroupInfo in groupsForContact)
        groupsPopup.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) string.Format("{0}. {1}", (object) num++, (object) contactGroupInfo.GroupName), ToolStripMenuItemEx.ToolStripItemType.Label));
      return groupsPopup;
    }

    private ContextMenuStrip createActivityPopup(CampaignContactInfo contactInfo)
    {
      if (this.campaignData.ActiveCampaign == null)
        return (ContextMenuStrip) null;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      ContextMenuStrip activityPopup = new ContextMenuStrip();
      activityPopup.ShowImageMargin = false;
      ObjectWithImage dataSource = new ObjectWithImage("Campaign Activity", (Image) Resources.campaign_activity);
      activityPopup.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) dataSource, ToolStripMenuItemEx.ToolStripItemType.Header));
      int num = 1;
      foreach (CampaignActivityInfo contactActivity in contactInfo.ContactActivities)
      {
        string stepName = activeCampaign.CampaignSteps.Find(contactActivity.CampaignStepId).StepName;
        string name = new ActivityStatusNameProvider().GetName((object) contactActivity.Status);
        string str = ActivityStatus.Expected == contactActivity.Status ? contactActivity.ScheduledDate.ToShortDateString() : contactActivity.CompletedDate.ToShortDateString();
        activityPopup.Items.Add((ToolStripItem) new ToolStripMenuItemEx((object) string.Format("{0}. {1} - {2} on {3}", (object) num++, (object) stepName, (object) name, (object) str), ToolStripMenuItemEx.ToolStripItemType.Label));
      }
      return activityPopup;
    }

    private void addContactsToCampaign()
    {
      if (this.campaignData.ActiveCampaign == null)
        return;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      if (CampaignStatus.NotStarted == activeCampaign.Status)
        return;
      using (CampaignAddContactsDlg campaignAddContactsDlg = new CampaignAddContactsDlg(this.campaignData.ActiveCampaign))
      {
        this.campaignData.ActiveCampaign.BeginEdit();
        if (DialogResult.OK == campaignAddContactsDlg.ShowDialog((IWin32Window) this))
        {
          this.campaignData.ActiveCampaign.ApplyEdit();
          this.campaignData.UpdateContactList(activeCampaign);
          this.PopulateControls();
        }
        else
          this.campaignData.ActiveCampaign.CancelEdit();
      }
    }

    private void removeContactsFromCampaign()
    {
      if (0 >= this.gvContacts.SelectedItems.Count || this.campaignData.ActiveCampaign == null)
        return;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      if (CampaignStatus.NotStarted == activeCampaign.Status || DialogResult.No == Utils.Dialog((IWin32Window) this, "Are you sure you want to remove the selected contacts from this campaign?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
        return;
      activeCampaign.BeginEdit();
      foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
      {
        CampaignContact campaignContact = CampaignContact.NewCampaignContact((CampaignContactInfo) selectedItem.Tag);
        if (activeCampaign.CampaignContacts.Contains(campaignContact))
          activeCampaign.CampaignContacts.Remove(campaignContact);
      }
      activeCampaign.ApplyEdit();
      this.campaignData.UpdateContactList(activeCampaign);
      this.PopulateControls();
    }

    private void addContactsToGroup()
    {
      if (0 >= this.gvContacts.SelectedItems.Count)
        return;
      int[] newContactIds = new int[this.gvContacts.SelectedItems.Count];
      int num = 0;
      foreach (GVItem selectedItem in this.gvContacts.SelectedItems)
        newContactIds[num++] = ((CampaignContactInfo) selectedItem.Tag).ContactId;
      using (ContactGroupSelectionDlg groupSelectionDlg = new ContactGroupSelectionDlg(this.campaignData.ActiveCampaign.ContactType, true))
      {
        if (DialogResult.Cancel == groupSelectionDlg.ShowDialog((IWin32Window) this))
          return;
        if (groupSelectionDlg.SelectedGroups != null)
        {
          foreach (ContactGroupInfo selectedGroup in groupSelectionDlg.SelectedGroups)
            Session.ContactGroupManager.AddContactsToGroup(newContactIds, selectedGroup.GroupId, selectedGroup.ContactType);
        }
      }
      this.PopulateControls();
    }

    private void onPostUpdateToUIThread(object state)
    {
      this.BeginInvoke((Delegate) new MethodInvoker(this.afterSelectedIndexChanged));
    }

    private void afterSelectedIndexChanged()
    {
      this.isListUpdatePending = false;
      this.icnRemoveContacts.Enabled = false;
      this.btnAddToGroup.Enabled = false;
      if (this.campaignData.ActiveCampaign == null || CampaignStatus.NotStarted == this.campaignData.ActiveCampaign.Status)
        return;
      this.icnRemoveContacts.Enabled = 0 < this.gvContacts.SelectedItems.Count;
      this.btnAddToGroup.Enabled = 0 < this.gvContacts.SelectedItems.Count;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void gvLayoutManager_LayoutChanged(object sender, EventArgs e)
    {
      if (this.campaignData.ActiveCampaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
        this.borrowerTableLayout = this.gvLayoutManager.GetCurrentLayout();
      else
        this.bizPartnerTableLayout = this.gvLayoutManager.GetCurrentLayout();
      if (this.gvFilterManager != null)
        this.gvFilterManager.CreateColumnFilters(this.contactFieldDefs);
      this.getCampaignContacts();
    }

    private void gvFilterManager_FilterChanged(object sender, EventArgs e)
    {
      this.getCampaignContacts();
    }

    private void navContacts_PageChangedEvent(object sender, PageChangedEventArgs e)
    {
      this.displayCampaignContacts(e.ItemIndex, e.ItemCount);
    }

    private void gvContacts_SortItems(object source, GVColumnSortEventArgs e)
    {
      if (this.campaignData.ActiveCampaign == null)
        return;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      SortField sortFieldForColumn = this.getSortFieldForColumn((TableLayout.Column) this.gvContacts.Columns[e.Column].Tag, e.SortOrder);
      if (sortFieldForColumn != null)
        this.sortFields = new SortField[1]
        {
          sortFieldForColumn
        };
      else
        this.sortFields = new SortField[0];
      this.getCampaignContacts();
    }

    private void gvContacts_SelectedIndexCommitted(object sender, EventArgs e)
    {
      if (this.isListUpdatePending)
        return;
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.onPostUpdateToUIThread));
      this.isListUpdatePending = true;
    }

    private void lnkPhoneNumber_Click(object sender, EventArgs e)
    {
      if (!(sender is ImageLink imageLink))
        return;
      int contactId = -1;
      try
      {
        contactId = Convert.ToInt32(imageLink.Tag);
      }
      catch
      {
      }
      if (-1 == contactId || this.campaignData.ActiveCampaign == null)
        return;
      using (ContactNotesDialog contactNotesDialog = new ContactNotesDialog(this.campaignData.ActiveCampaign.ContactType, contactId))
      {
        int num = (int) contactNotesDialog.ShowDialog();
      }
    }

    private void lnkEmailAddress_Click(object sender, EventArgs e)
    {
      if (!(sender is ImageLink imageLink))
        return;
      string[] strArray = imageLink.Tag.ToString().Split('|');
      int contactId = -1;
      try
      {
        contactId = Convert.ToInt32(strArray[0]);
      }
      catch
      {
      }
      if (-1 == contactId)
        return;
      string str = strArray[1];
      if (string.Empty == str || this.campaignData.ActiveCampaign == null)
        return;
      EllieMae.EMLite.Campaign.Campaign activeCampaign = this.campaignData.ActiveCampaign;
      using (ContactNotesDialog contactNotesDialog = new ContactNotesDialog(activeCampaign.ContactType, contactId))
      {
        int newNote = contactNotesDialog.CreateNewNote();
        SystemUtil.ShellExecute("mailto:" + str);
        ContactUtils.addEmailHistory(new int[1]{ contactId }, activeCampaign.ContactType, newNote);
        int num = (int) contactNotesDialog.ShowDialog();
      }
    }

    private void btnModify_Click(object sender, EventArgs e)
    {
      using (MaintainQueriesDialog maintainQueriesDialog = new MaintainQueriesDialog(this.campaignData.ActiveCampaign))
      {
        if (DialogResult.OK != maintainQueriesDialog.ShowDialog())
          return;
        this.PopulateControls();
      }
    }

    private void icnAddContacts_Click(object sender, EventArgs e) => this.addContactsToCampaign();

    private void icnRemoveContacts_Click(object sender, EventArgs e)
    {
      this.removeContactsFromCampaign();
    }

    private void btnAddToGroup_Click(object sender, EventArgs e)
    {
      this.addContactsToGroup();
      this.gvFilterManager.RefreshFilterContent();
    }

    private void mnuAddContacts_Click(object sender, EventArgs e) => this.addContactsToCampaign();

    private void mnuSelectAll_Click(object sender, EventArgs e)
    {
      this.gvContacts.BeginUpdate();
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gvContacts.Items)
        gvItem.Selected = true;
      this.gvContacts.EndUpdate();
    }

    private void mnuRemoveFromCampaign_Click(object sender, EventArgs e)
    {
      this.removeContactsFromCampaign();
    }

    private void mnuAddToGroup_Click(object sender, EventArgs e) => this.addContactsToGroup();

    private SortField[] getCurrentSortFields()
    {
      List<SortField> sortFieldList = new List<SortField>();
      foreach (GVColumn column in this.gvContacts.Columns)
      {
        if (column.SortOrder != SortOrder.None)
        {
          SortField sortFieldForColumn = this.getSortFieldForColumn((TableLayout.Column) column.Tag, column.SortOrder);
          if (sortFieldForColumn != null)
            sortFieldList.Add(sortFieldForColumn);
        }
      }
      return sortFieldList.ToArray();
    }

    private SortField getSortFieldForColumn(TableLayout.Column colInfo, SortOrder sortOrder)
    {
      ReportFieldDef fieldByCriterionName = this.contactFieldDefs.GetFieldByCriterionName(colInfo.ColumnID);
      return fieldByCriterionName != null ? new SortField(colInfo.ColumnID, sortOrder == SortOrder.Ascending ? FieldSortOrder.Ascending : FieldSortOrder.Descending, fieldByCriterionName.DataConversion) : (SortField) null;
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (CampaignContactsTab));
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      this.imgList = new ImageList(this.components);
      this.pnlContactTable = new TableContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.btnAddToGroup = new Button();
      this.verticalSeparator1 = new VerticalSeparator();
      this.icnRemoveContacts = new StandardIconButton();
      this.icnAddContacts = new StandardIconButton();
      this.gvContacts = new GridView();
      this.ctxContacts = new ContextMenuStrip(this.components);
      this.tsmiAddContacts = new ToolStripMenuItem();
      this.toolStripMenuItem1 = new ToolStripSeparator();
      this.tsmiView = new ToolStripMenuItem();
      this.toolStripMenuItem2 = new ToolStripSeparator();
      this.tsmiSelectAll = new ToolStripMenuItem();
      this.tsmiRemove = new ToolStripMenuItem();
      this.tsmiAddtoGroup = new ToolStripMenuItem();
      this.navContacts = new PageListNavigator();
      this.pnlRemoveContacts = new GradientPanel();
      this.lblRemoveQuery = new Label();
      this.pnlAddContacts = new GradientPanel();
      this.lblAddQuery = new Label();
      this.pnlManageContacts = new GradientPanel();
      this.btnModify = new Button();
      this.lblManageContacts = new Label();
      this.toolTip1 = new ToolTip(this.components);
      this.pnlContactTable.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.icnRemoveContacts).BeginInit();
      ((ISupportInitialize) this.icnAddContacts).BeginInit();
      this.ctxContacts.SuspendLayout();
      this.pnlRemoveContacts.SuspendLayout();
      this.pnlAddContacts.SuspendLayout();
      this.pnlManageContacts.SuspendLayout();
      this.SuspendLayout();
      this.imgList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imgList.ImageStream");
      this.imgList.TransparentColor = Color.Transparent;
      this.imgList.Images.SetKeyName(0, "Phone");
      this.imgList.Images.SetKeyName(1, "PhoneMouseOver");
      this.imgList.Images.SetKeyName(2, "Email");
      this.imgList.Images.SetKeyName(3, "EmailMouseOver");
      this.pnlContactTable.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlContactTable.Controls.Add((Control) this.flowLayoutPanel1);
      this.pnlContactTable.Controls.Add((Control) this.gvContacts);
      this.pnlContactTable.Controls.Add((Control) this.navContacts);
      this.pnlContactTable.Dock = DockStyle.Fill;
      this.pnlContactTable.Location = new Point(0, 76);
      this.pnlContactTable.Name = "pnlContactTable";
      this.pnlContactTable.Size = new Size(1000, 346);
      this.pnlContactTable.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.pnlContactTable.TabIndex = 73;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnAddToGroup);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.icnRemoveContacts);
      this.flowLayoutPanel1.Controls.Add((Control) this.icnAddContacts);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(844, 1);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(151, 22);
      this.flowLayoutPanel1.TabIndex = 23;
      this.btnAddToGroup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnAddToGroup.BackColor = SystemColors.Control;
      this.btnAddToGroup.Enabled = false;
      this.btnAddToGroup.Location = new Point(63, 0);
      this.btnAddToGroup.Margin = new Padding(0);
      this.btnAddToGroup.Name = "btnAddToGroup";
      this.btnAddToGroup.Padding = new Padding(2, 0, 0, 0);
      this.btnAddToGroup.Size = new Size(88, 22);
      this.btnAddToGroup.TabIndex = 18;
      this.btnAddToGroup.Text = "Add to Group";
      this.btnAddToGroup.UseVisualStyleBackColor = true;
      this.btnAddToGroup.Click += new EventHandler(this.btnAddToGroup_Click);
      this.verticalSeparator1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.verticalSeparator1.Location = new Point(57, 3);
      this.verticalSeparator1.Margin = new Padding(3, 3, 4, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 19;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.icnRemoveContacts.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnRemoveContacts.BackColor = Color.Transparent;
      this.icnRemoveContacts.Location = new Point(35, 3);
      this.icnRemoveContacts.Name = "icnRemoveContacts";
      this.icnRemoveContacts.Size = new Size(16, 16);
      this.icnRemoveContacts.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.icnRemoveContacts.TabIndex = 20;
      this.icnRemoveContacts.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.icnRemoveContacts, "Delete Contact");
      this.icnRemoveContacts.Click += new EventHandler(this.icnRemoveContacts_Click);
      this.icnAddContacts.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.icnAddContacts.BackColor = Color.Transparent;
      this.icnAddContacts.Location = new Point(13, 3);
      this.icnAddContacts.Name = "icnAddContacts";
      this.icnAddContacts.Size = new Size(16, 16);
      this.icnAddContacts.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.icnAddContacts.TabIndex = 21;
      this.icnAddContacts.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.icnAddContacts, "New Contact");
      this.icnAddContacts.Click += new EventHandler(this.icnAddContacts_Click);
      this.gvContacts.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "LastName";
      gvColumn1.Text = "Last Name";
      gvColumn1.Width = 100;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "FirstName";
      gvColumn2.Text = "First Name";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "DateAdded";
      gvColumn3.SortMethod = GVSortMethod.Date;
      gvColumn3.Text = "Date Added";
      gvColumn3.Width = 85;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "PhoneNumber";
      gvColumn4.Text = "Home Phone";
      gvColumn4.Width = 90;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "EmailAddress";
      gvColumn5.Text = "Home Email";
      gvColumn5.Width = 95;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Address";
      gvColumn6.SpringToFit = true;
      gvColumn6.Text = "Address";
      gvColumn6.Width = 191;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "City";
      gvColumn7.Text = "City";
      gvColumn7.Width = 100;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "State";
      gvColumn8.Text = "State";
      gvColumn8.Width = 50;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "FaxNumber";
      gvColumn9.Text = "Fax Number";
      gvColumn9.Width = 87;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Activity";
      gvColumn10.Text = "Activity";
      gvColumn10.Width = 100;
      this.gvContacts.Columns.AddRange(new GVColumn[10]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.gvContacts.ContextMenuStrip = this.ctxContacts;
      this.gvContacts.Dock = DockStyle.Fill;
      this.gvContacts.FilterVisible = true;
      this.gvContacts.Location = new Point(1, 25);
      this.gvContacts.Name = "gvContacts";
      this.gvContacts.Size = new Size(998, 320);
      this.gvContacts.SortOption = GVSortOption.Owner;
      this.gvContacts.TabIndex = 22;
      this.gvContacts.SelectedIndexCommitted += new EventHandler(this.gvContacts_SelectedIndexCommitted);
      this.gvContacts.SortItems += new GVColumnSortEventHandler(this.gvContacts_SortItems);
      this.ctxContacts.Items.AddRange(new ToolStripItem[7]
      {
        (ToolStripItem) this.tsmiAddContacts,
        (ToolStripItem) this.toolStripMenuItem1,
        (ToolStripItem) this.tsmiView,
        (ToolStripItem) this.toolStripMenuItem2,
        (ToolStripItem) this.tsmiSelectAll,
        (ToolStripItem) this.tsmiRemove,
        (ToolStripItem) this.tsmiAddtoGroup
      });
      this.ctxContacts.Name = "contextMenuStrip1";
      this.ctxContacts.Size = new Size(200, 148);
      this.ctxContacts.Opening += new CancelEventHandler(this.ctxContacts_Opening);
      this.tsmiAddContacts.Name = "tsmiAddContacts";
      this.tsmiAddContacts.Size = new Size(199, 22);
      this.tsmiAddContacts.Text = "Add Contacts";
      this.tsmiAddContacts.Click += new EventHandler(this.mnuAddContacts_Click);
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new Size(196, 6);
      this.tsmiView.Name = "tsmiView";
      this.tsmiView.Size = new Size(199, 22);
      this.tsmiView.Text = "View";
      this.tsmiView.Click += new EventHandler(this.mnuContactsView_Click);
      this.toolStripMenuItem2.Name = "toolStripMenuItem2";
      this.toolStripMenuItem2.Size = new Size(196, 6);
      this.tsmiSelectAll.Name = "tsmiSelectAll";
      this.tsmiSelectAll.Size = new Size(199, 22);
      this.tsmiSelectAll.Text = "Select All";
      this.tsmiSelectAll.Click += new EventHandler(this.mnuSelectAll_Click);
      this.tsmiRemove.Name = "tsmiRemove";
      this.tsmiRemove.Size = new Size(199, 22);
      this.tsmiRemove.Text = "Remove from Campaign";
      this.tsmiRemove.Click += new EventHandler(this.mnuRemoveFromCampaign_Click);
      this.tsmiAddtoGroup.Name = "tsmiAddtoGroup";
      this.tsmiAddtoGroup.Size = new Size(199, 22);
      this.tsmiAddtoGroup.Text = "Add to Group";
      this.tsmiAddtoGroup.Click += new EventHandler(this.mnuAddToGroup_Click);
      this.navContacts.BackColor = Color.Transparent;
      this.navContacts.Font = new Font("Arial", 8f);
      this.navContacts.ItemsPerPage = 100;
      this.navContacts.Location = new Point(0, 1);
      this.navContacts.Name = "navContacts";
      this.navContacts.NumberOfItems = 0;
      this.navContacts.Size = new Size(254, 22);
      this.navContacts.TabIndex = 0;
      this.navContacts.PageChangedEvent += new PageListNavigator.PageChangedEventHandler(this.navContacts_PageChangedEvent);
      this.pnlRemoveContacts.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlRemoveContacts.Controls.Add((Control) this.lblRemoveQuery);
      this.pnlRemoveContacts.Dock = DockStyle.Top;
      this.pnlRemoveContacts.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlRemoveContacts.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlRemoveContacts.Location = new Point(0, 51);
      this.pnlRemoveContacts.Name = "pnlRemoveContacts";
      this.pnlRemoveContacts.Size = new Size(1000, 25);
      this.pnlRemoveContacts.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlRemoveContacts.TabIndex = 72;
      this.lblRemoveQuery.AutoSize = true;
      this.lblRemoveQuery.BackColor = Color.Transparent;
      this.lblRemoveQuery.Location = new Point(10, 5);
      this.lblRemoveQuery.Name = "lblRemoveQuery";
      this.lblRemoveQuery.Size = new Size(132, 14);
      this.lblRemoveQuery.TabIndex = 0;
      this.lblRemoveQuery.Text = "Remove Contacts: Manual";
      this.pnlAddContacts.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlAddContacts.Controls.Add((Control) this.lblAddQuery);
      this.pnlAddContacts.Dock = DockStyle.Top;
      this.pnlAddContacts.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.pnlAddContacts.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.pnlAddContacts.Location = new Point(0, 26);
      this.pnlAddContacts.Name = "pnlAddContacts";
      this.pnlAddContacts.Size = new Size(1000, 25);
      this.pnlAddContacts.Style = GradientPanel.PanelStyle.TableFooter;
      this.pnlAddContacts.TabIndex = 71;
      this.lblAddQuery.AutoSize = true;
      this.lblAddQuery.BackColor = Color.Transparent;
      this.lblAddQuery.Location = new Point(10, 5);
      this.lblAddQuery.Name = "lblAddQuery";
      this.lblAddQuery.Size = new Size(396, 14);
      this.lblAddQuery.TabIndex = 0;
      this.lblAddQuery.Text = "Add Contacts: Automatic   Run Search: Daily   Last Updated: 10/14/2008   Filters:";
      this.pnlManageContacts.Controls.Add((Control) this.btnModify);
      this.pnlManageContacts.Controls.Add((Control) this.lblManageContacts);
      this.pnlManageContacts.Dock = DockStyle.Top;
      this.pnlManageContacts.Location = new Point(0, 0);
      this.pnlManageContacts.Name = "pnlManageContacts";
      this.pnlManageContacts.Size = new Size(1000, 26);
      this.pnlManageContacts.Style = GradientPanel.PanelStyle.TableHeader;
      this.pnlManageContacts.TabIndex = 70;
      this.btnModify.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnModify.Location = new Point(940, 2);
      this.btnModify.Name = "btnModify";
      this.btnModify.Size = new Size(55, 22);
      this.btnModify.TabIndex = 19;
      this.btnModify.Text = "Modify";
      this.btnModify.Click += new EventHandler(this.btnModify_Click);
      this.lblManageContacts.AutoSize = true;
      this.lblManageContacts.BackColor = Color.Transparent;
      this.lblManageContacts.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblManageContacts.Location = new Point(10, 6);
      this.lblManageContacts.Name = "lblManageContacts";
      this.lblManageContacts.Size = new Size(102, 14);
      this.lblManageContacts.TabIndex = 0;
      this.lblManageContacts.Text = "Manage Contacts";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(1000, 422);
      this.ControlBox = false;
      this.Controls.Add((Control) this.pnlContactTable);
      this.Controls.Add((Control) this.pnlRemoveContacts);
      this.Controls.Add((Control) this.pnlAddContacts);
      this.Controls.Add((Control) this.pnlManageContacts);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CampaignContactsTab);
      this.pnlContactTable.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.icnRemoveContacts).EndInit();
      ((ISupportInitialize) this.icnAddContacts).EndInit();
      this.ctxContacts.ResumeLayout(false);
      this.pnlRemoveContacts.ResumeLayout(false);
      this.pnlRemoveContacts.PerformLayout();
      this.pnlAddContacts.ResumeLayout(false);
      this.pnlAddContacts.PerformLayout();
      this.pnlManageContacts.ResumeLayout(false);
      this.pnlManageContacts.PerformLayout();
      this.ResumeLayout(false);
    }

    private void mnuContactsView_Click(object sender, EventArgs e)
    {
      if (1 != this.gvContacts.SelectedItems.Count)
        return;
      Cursor.Current = Cursors.WaitCursor;
      CampaignContactInfo tag = (CampaignContactInfo) this.gvContacts.SelectedItems[0].Tag;
      AttendeeInfo attendeeInfo = new AttendeeInfo();
      attendeeInfo.AssignInfo(tag.ContactId, this.campaignData.ActiveCampaign.ContactType);
      Cursor.Current = Cursors.Default;
      int num = (int) attendeeInfo.ShowDialog();
      if (!attendeeInfo.GoToContact)
        return;
      Session.MainScreen.NavigateToContact(attendeeInfo.SelectedContact);
    }

    private void ctxContacts_Opening(object sender, CancelEventArgs e)
    {
      this.tsmiAddContacts.Enabled = true;
      this.tsmiAddtoGroup.Enabled = true;
      this.tsmiRemove.Enabled = true;
      this.tsmiSelectAll.Enabled = true;
      this.tsmiView.Enabled = true;
      if (this.gvContacts.SelectedItems.Count == 0)
      {
        this.tsmiView.Enabled = false;
        this.tsmiRemove.Enabled = false;
        this.tsmiAddtoGroup.Enabled = false;
      }
      else
      {
        if (this.gvContacts.SelectedItems.Count <= 1)
          return;
        this.tsmiView.Enabled = false;
      }
    }

    public class ListViewColumnSorter : IComparer
    {
      private int ColumnToSort;
      private SortOrder OrderOfSort;
      private CaseInsensitiveComparer ObjectCompare;

      public ListViewColumnSorter()
      {
        this.ColumnToSort = 0;
        this.OrderOfSort = SortOrder.None;
        this.ObjectCompare = new CaseInsensitiveComparer();
      }

      public int Compare(object x, object y)
      {
        int num = this.ObjectCompare.Compare((object) ((ListViewItem) x).SubItems[this.ColumnToSort].Text, (object) ((ListViewItem) y).SubItems[this.ColumnToSort].Text);
        if (this.OrderOfSort == SortOrder.Ascending)
          return num;
        return this.OrderOfSort == SortOrder.Descending ? -num : 0;
      }

      public int SortColumn
      {
        set => this.ColumnToSort = value;
        get => this.ColumnToSort;
      }

      public SortOrder Order
      {
        set => this.OrderOfSort = value;
        get => this.OrderOfSort;
      }
    }
  }
}
