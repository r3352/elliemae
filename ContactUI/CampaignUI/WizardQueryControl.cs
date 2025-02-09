// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.WizardQueryControl
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class WizardQueryControl : UserControl
  {
    private const int SEARCH_PREDEFINED = 0;
    private const int SEARCH_SAVED = 1;
    private const int SEARCH_ADVANCED = 2;
    private string[] searchOptions = new string[3]
    {
      "Predefined Search",
      "Saved Search",
      "Advanced Search"
    };
    private WizardQueryControl.PredefinedQuery[] predefinedQueries = new WizardQueryControl.PredefinedQuery[10]
    {
      new WizardQueryControl.PredefinedQuery(true, true, true, true, ""),
      new WizardQueryControl.PredefinedQuery(true, false, true, false, "Imported or Newly Created Borrower Contacts"),
      new WizardQueryControl.PredefinedQuery(true, true, true, false, "Borrower Contact Type"),
      new WizardQueryControl.PredefinedQuery(true, true, true, false, "Borrower Contact Status"),
      new WizardQueryControl.PredefinedQuery(true, false, true, false, "Last Loan Originated"),
      new WizardQueryControl.PredefinedQuery(true, false, true, false, "Last Loan Closed"),
      new WizardQueryControl.PredefinedQuery(true, false, false, true, "Imported or Newly Created Business Contacts"),
      new WizardQueryControl.PredefinedQuery(true, true, false, true, "Business Contact Category"),
      new WizardQueryControl.PredefinedQuery(true, false, true, false, "Birthday"),
      new WizardQueryControl.PredefinedQuery(true, false, true, false, "Anniversary")
    };
    private string[] loanFields = new string[5]
    {
      "Completion Date",
      "Loan Amount",
      "Interest Rate",
      "Loan Type",
      "Loan Purpose"
    };
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
    private string[] borrowerTypes;
    private string[] borrowerStatuses;
    private string[] partnerCategories;
    private string[] birthdayConditions;
    private string[] anniversaryConditions;
    private const int pnlStartLocationX = 3;
    private const int pnlStartLocationY = 0;
    private int pnlLocationX = 3;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private EllieMae.EMLite.ContactGroup.ContactQuery campaignQuery;
    private QueryEditMode queryEditMode;
    private bool displayWizardOptions;
    private bool displayingPredefinedQuery;
    private string queryName = string.Empty;
    private CampaignFrequencyNameProvider frequencyNames = new CampaignFrequencyNameProvider();
    private Label label1;
    private BizCategoryUtil catUtil;
    private GroupBox grpFilters;
    private ComboBox cboPredefinedQuery;
    private System.ComponentModel.Container components;
    private Panel pnlComboSelect;
    private ComboBox cboComboValue;
    private Label lblComboDesc;
    private Panel pnlNumericSelect;
    private Label lblNumericDesc;
    private NumericUpDown nudNumericValue;
    private Label lblCompareDesc;
    private ComboBox cboCompareOperators;
    private TextBox txtCompareAmount;
    private Label lblCompareUnits;
    private Label lblPurposeDesc;
    private ComboBox cboPurposeValue;
    private Panel pnlCompareSelect;
    private Panel pnlPurposeSelect;
    private Label lblNumericUnits;
    private Panel pnlSearchOption;
    private Label lblAddOrRemove;
    private Button btnTest;
    private Panel pnlMain;
    private Label lblAdvancedFilters;
    private CheckBox chkPrimaryOnly;
    private Panel pnlPrimaryOnly;
    private FormattedLabel lblNote;
    private ComboBox cboSearchOption;
    private Label lblSearchOption;
    private StandardIconButton icnAdvancedQuery;
    private Label lblFilters;
    private Panel pnlPredefinedFilters;
    private Panel pnlAdvancedFilters;
    private ComboBox cboAdvancedSearch;

    public WizardQueryControl(
      QueryEditMode queryEditMode,
      bool displayWizardOptions,
      EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.queryEditMode = queryEditMode;
      this.displayWizardOptions = displayWizardOptions;
      this.campaign = campaign;
      Cursor.Current = Cursors.WaitCursor;
      this.InitializeComponent();
      this.populateControls();
      Cursor.Current = Cursors.Default;
    }

    protected void populateControls()
    {
      this.cboPredefinedQuery.Left = this.icnAdvancedQuery.Left;
      this.cboPredefinedQuery.Top = this.cboSearchOption.Top;
      this.cboAdvancedSearch.Location = this.cboPredefinedQuery.Location;
      this.pnlAdvancedFilters.Location = this.pnlPredefinedFilters.Location;
      this.cboSearchOption.Items.Clear();
      this.cboSearchOption.Items.AddRange((object[]) this.searchOptions);
      bool flag1 = this.queryEditMode == QueryEditMode.AddQuery;
      bool flag2 = this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower;
      foreach (WizardQueryControl.PredefinedQuery predefinedQuery in this.predefinedQueries)
      {
        if ((flag1 && predefinedQuery.IsAdd || !flag1 && predefinedQuery.IsDelete) && (flag2 && predefinedQuery.IsBorrower || !flag2 && predefinedQuery.IsPartner))
          this.cboPredefinedQuery.Items.Add((object) predefinedQuery.Name);
      }
      this.pnlSearchOption.Visible = this.displayWizardOptions;
      if (this.queryEditMode == QueryEditMode.AddQuery)
      {
        this.campaignQuery = this.campaign.AddQuery;
        this.lblAddOrRemove.Text = "Build a search for contacts to automatically add to the campaign.";
        if (this.campaign.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower)
        {
          this.chkPrimaryOnly.Checked = this.campaignQuery.PrimaryOnly;
          this.pnlPrimaryOnly.Visible = true;
          this.pnlMain.BringToFront();
        }
      }
      else
      {
        this.campaignQuery = this.campaign.DeleteQuery;
        this.lblAddOrRemove.Text = "Build a search to automatically remove contacts from the campaign.";
      }
      this.displayQuery();
    }

    private void displayQuery()
    {
      if (ContactQueryType.CampaignPredefinedQuery == (ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType))
      {
        this.cboSearchOption.SelectedIndex = 0;
        this.cboPredefinedQuery.Visible = true;
        this.cboAdvancedSearch.Visible = false;
        this.icnAdvancedQuery.Visible = false;
        this.pnlPredefinedFilters.Visible = true;
        this.pnlAdvancedFilters.Visible = false;
        this.cboPredefinedQuery.SelectedIndex = 0;
        if (!(string.Empty != this.campaignQuery.XmlQueryString))
          return;
        this.displayPredefinedQuery();
      }
      else
      {
        this.cboSearchOption.SelectedIndex = 2;
        this.cboPredefinedQuery.Visible = false;
        this.cboAdvancedSearch.Visible = false;
        this.icnAdvancedQuery.Visible = true;
        this.pnlAdvancedFilters.Visible = true;
        this.pnlPredefinedFilters.Visible = false;
        try
        {
          this.lblAdvancedFilters.Text = ((FieldFilterList) QueryXmlConverter.Deserialize(typeof (FieldFilterList), this.campaignQuery.XmlQueryString)).ToString(false);
          this.btnTest.Enabled = true;
        }
        catch
        {
        }
      }
    }

    private void displayPredefinedQuery()
    {
      this.displayingPredefinedQuery = true;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.campaignQuery.XmlQueryString);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string attribute = documentElement.GetAttribute("Name");
      this.cboPredefinedQuery.SelectedItem = (object) attribute;
      ArrayList parms = new ArrayList();
      for (int i = 0; i < documentElement.ChildNodes.Count; ++i)
        parms.Add((object) ((XmlElement) documentElement.ChildNodes[i]).GetAttribute("Value"));
      switch (attribute)
      {
        case "Anniversary":
          this.setBorrowerAnniversary(parms[0].ToString());
          break;
        case "Birthday":
          this.setBorrowerBirthday(parms[0].ToString());
          break;
        case "Borrower Contact Status":
          this.setBorrowerStatus(parms[0].ToString());
          break;
        case "Borrower Contact Type":
          this.setBorrowerType(parms[0].ToString());
          break;
        case "Business Contact Category":
          if (this.catUtil == null)
            this.catUtil = new BizCategoryUtil(Session.SessionObjects);
          this.setPartnerCategory(this.catUtil.CategoryIdToName(int.Parse(parms[0].ToString())));
          break;
        case "Imported or Newly Created Borrower Contacts":
          this.setNewContact(int.Parse(parms[0].ToString()));
          break;
        case "Imported or Newly Created Business Contacts":
          this.setNewContact(int.Parse(parms[0].ToString()));
          break;
        case "Last Loan Closed":
          this.setLastLoanClosed(parms);
          break;
        case "Last Loan Originated":
          this.setLastLoanOriginated(int.Parse(parms[0].ToString()));
          break;
      }
      this.displayingPredefinedQuery = false;
    }

    private void setNewContact(int days)
    {
      this.setNumericSelect((this.campaignQuery.ContactType == EllieMae.EMLite.ContactUI.ContactType.Borrower ? "Borrowers" : "Business contacts") + " imported or created in the previous", days, nameof (days));
      this.btnTest.Enabled = true;
    }

    private void setBorrowerBirthday(string condition)
    {
      if (this.birthdayConditions == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (string str in MonthWeekEnumUtil.GetDisplayNamesNoExactDate())
        {
          if (string.Empty != str)
            arrayList.Add((object) str);
        }
        this.birthdayConditions = (string[]) arrayList.ToArray(typeof (string));
      }
      this.setComboSelect(121, "Borrowers whose birthday is in the", this.birthdayConditions, condition);
      this.btnTest.Enabled = true;
    }

    private void setBorrowerAnniversary(string condition)
    {
      if (this.anniversaryConditions == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (string str in MonthWeekEnumUtil.GetDisplayNamesNoExactDate())
        {
          if (string.Empty != str)
            arrayList.Add((object) str);
        }
        this.anniversaryConditions = (string[]) arrayList.ToArray(typeof (string));
      }
      this.setComboSelect(121, "Borrowers whose anniversary is in the", this.anniversaryConditions, condition);
      this.btnTest.Enabled = true;
    }

    private void setBorrowerType(string borrowerType)
    {
      if (this.borrowerTypes == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (object displayName in BorrowerTypeEnumUtil.GetDisplayNames())
        {
          if (!(string.Empty == displayName.ToString()))
            arrayList.Add(displayName);
        }
        this.borrowerTypes = (string[]) arrayList.ToArray(typeof (string));
      }
      if (this.borrowerTypes.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are currently no borrower types defined for selection.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (string.Empty == borrowerType)
          borrowerType = this.borrowerTypes[0];
        this.setComboSelect(121, "Borrowers whose contact type is", this.borrowerTypes, borrowerType);
        this.btnTest.Enabled = true;
      }
    }

    private void setBorrowerStatus(string borrowerStatus)
    {
      if (this.borrowerStatuses == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (BorrowerStatusItem borrowerStatusItem in Session.ContactManager.GetBorrowerStatus().Items)
        {
          if (!(string.Empty == borrowerStatusItem.name))
            arrayList.Add((object) borrowerStatusItem.name);
        }
        this.borrowerStatuses = (string[]) arrayList.ToArray(typeof (string));
      }
      if (this.borrowerStatuses.Length == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "There are currently no borrower status codes defined for selection.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (string.Empty == borrowerStatus)
          borrowerStatus = this.borrowerStatuses[0];
        this.setComboSelect(402, "Borrowers whose contact status is", this.borrowerStatuses, borrowerStatus);
        this.btnTest.Enabled = true;
      }
    }

    private void setLastLoanOriginated(int days)
    {
      this.setNumericSelect("Borrowers whose last loan was originated in the previous", days, nameof (days));
      this.btnTest.Enabled = true;
    }

    private void setLastLoanClosed(ArrayList parms)
    {
      if (this.displayingPredefinedQuery)
        this.setComboSelect(121, "Borrowers whose last closed loan had a", this.loanFields, parms[0].ToString());
      switch (parms[0].ToString())
      {
        case "Completion Date":
          this.setNumericSelect("which occurred in the previous", 2 <= parms.Count ? int.Parse(parms[1].ToString()) : 1, "days");
          break;
        case "Loan Amount":
          this.setCompareSelect(string.Empty, 2 <= parms.Count ? parms[1].ToString() : this.operators[2], 3 <= parms.Count ? Decimal.Parse(parms[2].ToString()) : 0M, "dollars");
          break;
        case "Interest Rate":
          this.setCompareSelect(string.Empty, 2 <= parms.Count ? parms[1].ToString() : this.operators[2], 3 <= parms.Count ? Decimal.Parse(parms[2].ToString()) : 0M, "percent");
          break;
        case "Loan Type":
          this.setPurposeSelect(this.loanTypes, 2 <= parms.Count ? parms[1].ToString() : this.loanTypes[0]);
          break;
        case "Loan Purpose":
          this.setPurposeSelect(this.loanPurposes, 2 <= parms.Count ? parms[1].ToString() : this.loanPurposes[0]);
          break;
      }
      this.btnTest.Enabled = true;
    }

    private void setPartnerCategory(string partnerCategory)
    {
      if (this.partnerCategories == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (BizCategory bizCategory in Session.ContactManager.GetBizCategories())
        {
          if (!(string.Empty == bizCategory.Name))
            arrayList.Add((object) bizCategory.Name);
        }
        this.partnerCategories = (string[]) arrayList.ToArray(typeof (string));
      }
      this.setComboSelect(352, "Business contacts whose contact category is", this.partnerCategories, partnerCategory);
      this.btnTest.Enabled = true;
    }

    private void setNumericSelect(string description, int initialValue, string units)
    {
      this.lblNumericDesc.Text = description;
      this.nudNumericValue.Value = (Decimal) initialValue;
      this.lblNumericUnits.Text = units;
      this.nudNumericValue.Left = this.lblNumericDesc.Location.X + this.lblNumericDesc.Size.Width - 2;
      this.lblNumericUnits.Left = this.nudNumericValue.Location.X + this.nudNumericValue.Size.Width;
      this.nudNumericValue.BringToFront();
      this.pnlNumericSelect.Width = this.lblNumericUnits.Location.X + this.lblNumericUnits.Size.Width;
      this.pnlNumericSelect.Location = new Point(this.pnlLocationX, 0);
      this.pnlLocationX += this.pnlNumericSelect.Width;
      this.pnlNumericSelect.Visible = true;
    }

    private void setComboSelect(
      int comboWidth,
      string description,
      string[] selections,
      string selection)
    {
      this.lblComboDesc.Text = description;
      this.cboComboValue.Items.Clear();
      this.cboComboValue.Items.AddRange((object[]) selections);
      this.cboComboValue.SelectedItem = (object) selection;
      this.cboComboValue.DropDownWidth = this.calculateDropDownWidth(this.cboComboValue, selections);
      this.cboComboValue.Left = this.lblComboDesc.Location.X + this.lblComboDesc.Size.Width - 2;
      this.cboComboValue.Width = comboWidth;
      this.pnlComboSelect.Width = this.cboComboValue.Location.X + this.cboComboValue.Size.Width;
      this.pnlComboSelect.Location = new Point(this.pnlLocationX, 0);
      this.pnlLocationX += this.pnlComboSelect.Width;
      this.pnlComboSelect.Visible = true;
    }

    private int calculateDropDownWidth(ComboBox comboBox, string[] selections)
    {
      int val1 = 0;
      using (Graphics graphics = comboBox.CreateGraphics())
      {
        foreach (string selection in selections)
        {
          float width = graphics.MeasureString(selection, comboBox.Font).Width;
          if ((double) val1 < (double) width)
            val1 = (int) Math.Ceiling((double) width);
        }
      }
      if (comboBox.MaxDropDownItems < selections.Length)
        val1 += 25;
      return Math.Max(val1, comboBox.Width);
    }

    private void setCompareSelect(
      string description,
      string comparison,
      Decimal amount,
      string units)
    {
      this.lblCompareDesc.Text = description;
      this.cboCompareOperators.Items.Clear();
      this.cboCompareOperators.Items.AddRange((object[]) this.operators);
      this.cboCompareOperators.SelectedItem = (object) comparison;
      this.txtCompareAmount.Text = amount.ToString();
      this.lblCompareUnits.Text = units;
      this.cboCompareOperators.Left = this.lblCompareDesc.Location.X + this.lblCompareDesc.Size.Width;
      this.txtCompareAmount.Left = this.cboCompareOperators.Location.X + this.cboCompareOperators.Size.Width + 2;
      this.lblCompareUnits.Left = this.txtCompareAmount.Location.X + this.txtCompareAmount.Size.Width;
      this.pnlCompareSelect.Width = this.lblCompareUnits.Location.X + this.lblCompareUnits.Size.Width;
      this.pnlCompareSelect.Location = new Point(this.pnlLocationX, 0);
      this.pnlLocationX += this.pnlCompareSelect.Width;
      this.pnlCompareSelect.Visible = true;
    }

    private void setPurposeSelect(string[] selections, string selection)
    {
      this.cboPurposeValue.Items.Clear();
      this.cboPurposeValue.Items.AddRange((object[]) selections);
      this.cboPurposeValue.SelectedItem = (object) selection;
      this.pnlPurposeSelect.Width = this.cboPurposeValue.Location.X + this.cboPurposeValue.Size.Width;
      this.pnlPurposeSelect.Location = new Point(this.pnlLocationX, 0);
      this.pnlLocationX += this.pnlPurposeSelect.Width;
      this.pnlPurposeSelect.Visible = true;
    }

    private void cboSearchOption_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if (this.cboSearchOption.SelectedIndex == 0)
      {
        this.cboPredefinedQuery.SelectedIndex = 0;
        this.cboPredefinedQuery.Visible = true;
        this.cboAdvancedSearch.Visible = false;
        this.icnAdvancedQuery.Visible = false;
        this.pnlPredefinedFilters.Visible = true;
        this.pnlAdvancedFilters.Visible = false;
        this.btnTest.Enabled = false;
        this.campaignQuery.QueryType &= ~ContactQueryType.CampaignAdvancedQuery;
        this.campaignQuery.QueryType |= ContactQueryType.CampaignPredefinedQuery;
        this.campaignQuery.XmlQueryString = string.Empty;
        this.lblAdvancedFilters.Text = string.Empty;
      }
      else if (2 == this.cboSearchOption.SelectedIndex)
      {
        this.campaignQuery.QueryType &= ~ContactQueryType.CampaignPredefinedQuery;
        this.campaignQuery.QueryType |= ContactQueryType.CampaignAdvancedQuery;
        this.campaignQuery.XmlQueryString = string.Empty;
        this.cboPredefinedQuery.SelectedIndex = 0;
        this.cboPredefinedQuery.Visible = false;
        this.cboAdvancedSearch.Visible = false;
        this.icnAdvancedQuery.Visible = true;
        this.pnlAdvancedFilters.Visible = true;
        this.pnlPredefinedFilters.Visible = false;
        this.btnTest.Enabled = false;
        this.lblAdvancedFilters.Text = string.Empty;
        this.icnAdvancedQuery_Click(sender, e);
      }
      else
      {
        this.loadSavedViews();
        if (this.cboAdvancedSearch.Items.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "There are no Saved Searches available for selection", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.displayQuery();
        }
        else
        {
          this.cboPredefinedQuery.SelectedIndex = 0;
          this.cboPredefinedQuery.Visible = false;
          this.cboAdvancedSearch.Visible = true;
          this.icnAdvancedQuery.Visible = false;
          this.pnlAdvancedFilters.Visible = true;
          this.pnlPredefinedFilters.Visible = false;
          this.campaignQuery.BeginEdit();
          this.campaignQuery.QueryType = ContactQueryType.CampaignAdvancedQuery;
          if (this.queryEditMode == QueryEditMode.AddQuery)
            this.campaignQuery.QueryType |= ContactQueryType.CampaignAddQuery;
          else
            this.campaignQuery.QueryType |= ContactQueryType.CampaignDeleteQuery;
          this.campaignQuery.XmlQueryString = string.Empty;
          this.btnTest.Enabled = false;
        }
      }
    }

    private void loadSavedViews()
    {
      FileSystemEntry[] fileSystemEntryArray = this.campaign.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower ? Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.BizPartnerView, FileSystemEntry.PrivateRoot(Session.UserID)) : Session.ConfigurationManager.GetTemplateDirEntries(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, FileSystemEntry.PrivateRoot(Session.UserID));
      this.cboAdvancedSearch.Items.Clear();
      foreach (FileSystemEntry e in fileSystemEntryArray)
        this.cboAdvancedSearch.Items.Add((object) new FileSystemEntryListItem(e));
    }

    private void cboPredefinedQuery_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.queryName = string.Empty;
      this.pnlNumericSelect.Visible = false;
      this.pnlComboSelect.Visible = false;
      this.pnlCompareSelect.Visible = false;
      this.pnlPurposeSelect.Visible = false;
      this.pnlLocationX = 3;
      this.btnTest.Enabled = false;
      if (0 >= this.cboPredefinedQuery.SelectedIndex)
        return;
      this.queryName = this.cboPredefinedQuery.SelectedItem.ToString();
      if (this.displayingPredefinedQuery)
        return;
      string queryName = this.queryName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(queryName))
      {
        case 104821391:
          if (!(queryName == "Last Loan Closed"))
            break;
          ArrayList parms = new ArrayList();
          parms.Add((object) this.loanFields[0]);
          this.displayingPredefinedQuery = true;
          this.setLastLoanClosed(parms);
          this.displayingPredefinedQuery = false;
          break;
        case 1326173803:
          if (!(queryName == "Borrower Contact Status"))
            break;
          this.setBorrowerStatus(string.Empty);
          break;
        case 1339627841:
          if (!(queryName == "Business Contact Category"))
            break;
          this.setPartnerCategory("No Category");
          break;
        case 2926465976:
          if (!(queryName == "Imported or Newly Created Borrower Contacts"))
            break;
          this.setNewContact(1);
          break;
        case 2947482404:
          if (!(queryName == "Imported or Newly Created Business Contacts"))
            break;
          this.setNewContact(1);
          break;
        case 3148086241:
          if (!(queryName == "Borrower Contact Type"))
            break;
          this.setBorrowerType(string.Empty);
          break;
        case 3243292492:
          if (!(queryName == "Birthday"))
            break;
          this.setBorrowerBirthday(MonthWeekEnumUtil.ValueToName(MonthWeekEnum.CurrentWeek));
          break;
        case 3618076121:
          if (!(queryName == "Last Loan Originated"))
            break;
          this.setLastLoanOriginated(1);
          break;
        case 4053872457:
          if (!(queryName == "Anniversary"))
            break;
          this.setBorrowerAnniversary(MonthWeekEnumUtil.ValueToName(MonthWeekEnum.CurrentWeek));
          break;
      }
    }

    private void cboComboValue_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (-1 == this.cboComboValue.SelectedIndex || this.displayingPredefinedQuery || !("Last Loan Closed" == this.queryName))
        return;
      this.pnlNumericSelect.Visible = false;
      this.pnlCompareSelect.Visible = false;
      this.pnlPurposeSelect.Visible = false;
      this.pnlLocationX = this.pnlComboSelect.Right;
      this.setLastLoanClosed(new ArrayList()
      {
        (object) this.cboComboValue.SelectedItem.ToString()
      });
    }

    private void chkPrimaryOnly_CheckedChanged(object sender, EventArgs e)
    {
      if (this.campaign.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower || (ContactQueryType.CampaignAddQuery & this.campaignQuery.QueryType) != ContactQueryType.CampaignAddQuery)
        return;
      this.campaignQuery.PrimaryOnly = this.chkPrimaryOnly.Checked;
    }

    private void CampaignQueryControl_Leave(object sender, EventArgs e)
    {
      if (this.campaignQuery == null || (ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType) != ContactQueryType.CampaignPredefinedQuery)
        return;
      this.SavePredefinedQuery();
    }

    private void btnTest_Click(object sender, EventArgs e)
    {
      if (this.campaignQuery != null && (ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType) == ContactQueryType.CampaignPredefinedQuery)
      {
        this.SavePredefinedQuery();
        int num = (int) new ViewContactGroupDialog(this.campaignQuery).ShowDialog();
      }
      else if (this.campaignQuery.XmlQueryString == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please click the Advanced Query button and select the advanced search criteria first.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2 = (int) new ViewContactGroupDialog(this.campaignQuery).ShowDialog();
      }
    }

    private void SavePredefinedQuery()
    {
      if ((ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType) != ContactQueryType.CampaignPredefinedQuery)
        return;
      if (this.cboPredefinedQuery.SelectedIndex == 0)
      {
        this.campaignQuery.XmlQueryString = string.Empty;
      }
      else
      {
        if (!this.btnTest.Enabled)
          return;
        ArrayList arrayList = new ArrayList();
        switch (this.queryName)
        {
          case "Anniversary":
            arrayList.Add((object) this.cboComboValue.SelectedItem.ToString());
            break;
          case "Birthday":
            arrayList.Add((object) this.cboComboValue.SelectedItem.ToString());
            break;
          case "Borrower Contact Status":
            arrayList.Add((object) this.cboComboValue.SelectedItem.ToString());
            break;
          case "Borrower Contact Type":
            arrayList.Add((object) this.cboComboValue.SelectedItem.ToString());
            break;
          case "Business Contact Category":
            if (this.catUtil == null)
              this.catUtil = new BizCategoryUtil(Session.SessionObjects);
            int id = this.catUtil.CategoryNameToId(this.cboComboValue.SelectedItem.ToString());
            arrayList.Add((object) id.ToString());
            break;
          case "Imported or Newly Created Borrower Contacts":
            arrayList.Add((object) this.nudNumericValue.Value.ToString());
            break;
          case "Imported or Newly Created Business Contacts":
            arrayList.Add((object) this.nudNumericValue.Value.ToString());
            break;
          case "Last Loan Closed":
            string str = this.cboComboValue.SelectedItem.ToString();
            arrayList.Add((object) str);
            switch (str)
            {
              case "Completion Date":
                arrayList.Add((object) this.nudNumericValue.Value.ToString());
                break;
              case "Loan Amount":
                arrayList.Add((object) this.cboCompareOperators.SelectedItem.ToString());
                arrayList.Add((object) this.txtCompareAmount.Text);
                break;
              case "Interest Rate":
                arrayList.Add((object) this.cboCompareOperators.SelectedItem.ToString());
                arrayList.Add((object) this.txtCompareAmount.Text);
                break;
              case "Loan Type":
                arrayList.Add((object) this.cboPurposeValue.SelectedItem.ToString());
                break;
              case "Loan Purpose":
                arrayList.Add((object) this.cboPurposeValue.SelectedItem.ToString());
                break;
            }
            break;
          case "Last Loan Originated":
            arrayList.Add((object) this.nudNumericValue.Value.ToString());
            break;
        }
        XmlDocument xmlDocument = new XmlDocument();
        XmlElement element1 = xmlDocument.CreateElement("SqlStmt");
        element1.SetAttribute("Name", this.queryName);
        xmlDocument.PrependChild((XmlNode) element1);
        for (int index = 0; index < arrayList.Count; ++index)
        {
          string name = "Parm" + (object) (index + 1);
          XmlElement element2 = xmlDocument.CreateElement(name);
          element2.SetAttribute("Value", arrayList[index] as string);
          element1.AppendChild((XmlNode) element2);
        }
        StringBuilder sb = new StringBuilder();
        StringWriter writer = new StringWriter(sb);
        xmlDocument.Save((TextWriter) writer);
        this.campaignQuery.XmlQueryString = sb.ToString();
      }
    }

    private DialogResult showAdvancedQueryDlg()
    {
      FieldFilterList currentFilter = new FieldFilterList();
      try
      {
        if (this.campaignQuery.XmlQueryString != "")
          currentFilter = (FieldFilterList) QueryXmlConverter.Deserialize(typeof (FieldFilterList), this.campaignQuery.XmlQueryString);
      }
      catch
      {
      }
      ContactAdvSearchDialog contactAdvSearchDialog = this.campaign.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower ? new ContactAdvSearchDialog((ReportFieldDefs) BizPartnerLoanReportFieldDefs.GetFieldDefs(EllieMae.EMLite.ContactUI.ContactType.BizPartner), currentFilter) : new ContactAdvSearchDialog((ReportFieldDefs) BorrowerLoanReportFieldDefs.GetFieldDefs(), currentFilter);
      DialogResult dialogResult = contactAdvSearchDialog.ShowDialog();
      if (DialogResult.OK == dialogResult)
      {
        this.campaignQuery.XmlQueryString = QueryXmlConverter.Serialize((object) contactAdvSearchDialog.GetSelectedFilter());
        this.lblAdvancedFilters.Text = contactAdvSearchDialog.GetSelectedFilter().ToString(false);
        this.btnTest.Enabled = true;
      }
      return dialogResult;
    }

    private void icnAdvancedQuery_Click(object sender, EventArgs e)
    {
      this.campaignQuery.BeginEdit();
      this.campaignQuery.QueryType = ContactQueryType.CampaignAdvancedQuery;
      if (this.queryEditMode == QueryEditMode.AddQuery)
        this.campaignQuery.QueryType |= ContactQueryType.CampaignAddQuery;
      else
        this.campaignQuery.QueryType |= ContactQueryType.CampaignDeleteQuery;
      if (DialogResult.OK == this.showAdvancedQueryDlg())
        this.campaignQuery.ApplyEdit();
      else
        this.campaignQuery.CancelEdit();
    }

    private void cboAdvancedSearch_SelectionChangeCommitted(object sender, EventArgs e)
    {
      FileSystemEntry entry = ((FileSystemEntryListItem) this.cboAdvancedSearch.SelectedItem).Entry;
      ContactView contactView = this.campaignQuery.ContactType != EllieMae.EMLite.ContactUI.ContactType.Borrower ? (ContactView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.BizPartnerView, entry) : (ContactView) Session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.BorrowerContactView, entry);
      this.campaignQuery.XmlQueryString = "";
      this.lblAdvancedFilters.Text = "";
      this.btnTest.Enabled = false;
      if (contactView.Filter == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The selected view can not be selected because there is no filter configured for this view.");
        this.cboSearchOption.SelectedIndex = 1;
      }
      else
      {
        this.campaignQuery.XmlQueryString = QueryXmlConverter.Serialize((object) contactView.Filter);
        if (contactView.Filter != null)
          this.lblAdvancedFilters.Text = contactView.Filter.ToString(false);
        this.btnTest.Enabled = true;
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
      this.grpFilters = new GroupBox();
      this.pnlAdvancedFilters = new Panel();
      this.lblAdvancedFilters = new Label();
      this.pnlPredefinedFilters = new Panel();
      this.pnlNumericSelect = new Panel();
      this.lblNumericUnits = new Label();
      this.lblNumericDesc = new Label();
      this.nudNumericValue = new NumericUpDown();
      this.pnlComboSelect = new Panel();
      this.cboComboValue = new ComboBox();
      this.lblComboDesc = new Label();
      this.pnlPurposeSelect = new Panel();
      this.cboPurposeValue = new ComboBox();
      this.lblPurposeDesc = new Label();
      this.pnlCompareSelect = new Panel();
      this.lblCompareUnits = new Label();
      this.txtCompareAmount = new TextBox();
      this.cboCompareOperators = new ComboBox();
      this.lblCompareDesc = new Label();
      this.btnTest = new Button();
      this.cboPredefinedQuery = new ComboBox();
      this.pnlSearchOption = new Panel();
      this.cboAdvancedSearch = new ComboBox();
      this.icnAdvancedQuery = new StandardIconButton();
      this.lblSearchOption = new Label();
      this.cboSearchOption = new ComboBox();
      this.lblAddOrRemove = new Label();
      this.pnlMain = new Panel();
      this.lblFilters = new Label();
      this.pnlPrimaryOnly = new Panel();
      this.lblNote = new FormattedLabel();
      this.chkPrimaryOnly = new CheckBox();
      this.label1 = new Label();
      this.grpFilters.SuspendLayout();
      this.pnlAdvancedFilters.SuspendLayout();
      this.pnlPredefinedFilters.SuspendLayout();
      this.pnlNumericSelect.SuspendLayout();
      this.nudNumericValue.BeginInit();
      this.pnlComboSelect.SuspendLayout();
      this.pnlPurposeSelect.SuspendLayout();
      this.pnlCompareSelect.SuspendLayout();
      this.pnlSearchOption.SuspendLayout();
      ((ISupportInitialize) this.icnAdvancedQuery).BeginInit();
      this.pnlMain.SuspendLayout();
      this.pnlPrimaryOnly.SuspendLayout();
      this.SuspendLayout();
      this.grpFilters.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpFilters.Controls.Add((Control) this.pnlAdvancedFilters);
      this.grpFilters.Controls.Add((Control) this.pnlPredefinedFilters);
      this.grpFilters.Font = new Font("Arial", 8.25f);
      this.grpFilters.Location = new Point(36, 31);
      this.grpFilters.Name = "grpFilters";
      this.grpFilters.Size = new Size(595, 237);
      this.grpFilters.TabIndex = 18;
      this.grpFilters.TabStop = false;
      this.pnlAdvancedFilters.Controls.Add((Control) this.lblAdvancedFilters);
      this.pnlAdvancedFilters.Location = new Point(1, 132);
      this.pnlAdvancedFilters.Name = "pnlAdvancedFilters";
      this.pnlAdvancedFilters.Size = new Size(588, 92);
      this.pnlAdvancedFilters.TabIndex = 5;
      this.lblAdvancedFilters.Font = new Font("Arial", 8.25f);
      this.lblAdvancedFilters.Location = new Point(6, 8);
      this.lblAdvancedFilters.Name = "lblAdvancedFilters";
      this.lblAdvancedFilters.Size = new Size(576, 76);
      this.lblAdvancedFilters.TabIndex = 0;
      this.pnlPredefinedFilters.Controls.Add((Control) this.pnlNumericSelect);
      this.pnlPredefinedFilters.Controls.Add((Control) this.pnlComboSelect);
      this.pnlPredefinedFilters.Controls.Add((Control) this.pnlPurposeSelect);
      this.pnlPredefinedFilters.Controls.Add((Control) this.pnlCompareSelect);
      this.pnlPredefinedFilters.Font = new Font("Arial", 8.25f);
      this.pnlPredefinedFilters.Location = new Point(1, 8);
      this.pnlPredefinedFilters.Name = "pnlPredefinedFilters";
      this.pnlPredefinedFilters.Size = new Size(588, 115);
      this.pnlPredefinedFilters.TabIndex = 4;
      this.pnlNumericSelect.Controls.Add((Control) this.lblNumericUnits);
      this.pnlNumericSelect.Controls.Add((Control) this.lblNumericDesc);
      this.pnlNumericSelect.Controls.Add((Control) this.nudNumericValue);
      this.pnlNumericSelect.Location = new Point(0, 0);
      this.pnlNumericSelect.Name = "pnlNumericSelect";
      this.pnlNumericSelect.Size = new Size(437, 28);
      this.pnlNumericSelect.TabIndex = 0;
      this.lblNumericUnits.AutoSize = true;
      this.lblNumericUnits.Font = new Font("Arial", 8.25f);
      this.lblNumericUnits.Location = new Point(289, 8);
      this.lblNumericUnits.Name = "lblNumericUnits";
      this.lblNumericUnits.Size = new Size(31, 14);
      this.lblNumericUnits.TabIndex = 2;
      this.lblNumericUnits.Text = "days";
      this.lblNumericUnits.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNumericDesc.AutoSize = true;
      this.lblNumericDesc.Font = new Font("Arial", 8.25f);
      this.lblNumericDesc.Location = new Point(2, 8);
      this.lblNumericDesc.Name = "lblNumericDesc";
      this.lblNumericDesc.Size = new Size(234, 14);
      this.lblNumericDesc.TabIndex = 1;
      this.lblNumericDesc.Text = "Borrowers imported or created in the previous ";
      this.lblNumericDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.nudNumericValue.Font = new Font("Arial", 8.25f);
      this.nudNumericValue.Location = new Point(242, 5);
      this.nudNumericValue.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this.nudNumericValue.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.nudNumericValue.Name = "nudNumericValue";
      this.nudNumericValue.Size = new Size(46, 20);
      this.nudNumericValue.TabIndex = 0;
      this.nudNumericValue.TextAlign = HorizontalAlignment.Right;
      this.nudNumericValue.Value = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this.pnlComboSelect.Controls.Add((Control) this.cboComboValue);
      this.pnlComboSelect.Controls.Add((Control) this.lblComboDesc);
      this.pnlComboSelect.Location = new Point(0, 28);
      this.pnlComboSelect.Name = "pnlComboSelect";
      this.pnlComboSelect.Size = new Size(437, 28);
      this.pnlComboSelect.TabIndex = 1;
      this.cboComboValue.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboComboValue.Font = new Font("Arial", 8.25f);
      this.cboComboValue.Location = new Point(201, 4);
      this.cboComboValue.Name = "cboComboValue";
      this.cboComboValue.Size = new Size(121, 22);
      this.cboComboValue.TabIndex = 3;
      this.cboComboValue.SelectedIndexChanged += new EventHandler(this.cboComboValue_SelectedIndexChanged);
      this.lblComboDesc.AutoSize = true;
      this.lblComboDesc.Font = new Font("Arial", 8.25f);
      this.lblComboDesc.Location = new Point(2, 8);
      this.lblComboDesc.Name = "lblComboDesc";
      this.lblComboDesc.Size = new Size(171, 14);
      this.lblComboDesc.TabIndex = 2;
      this.lblComboDesc.Text = "Borrowers whose contact type is";
      this.lblComboDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlPurposeSelect.Controls.Add((Control) this.cboPurposeValue);
      this.pnlPurposeSelect.Controls.Add((Control) this.lblPurposeDesc);
      this.pnlPurposeSelect.Location = new Point(0, 84);
      this.pnlPurposeSelect.Name = "pnlPurposeSelect";
      this.pnlPurposeSelect.Size = new Size(189, 28);
      this.pnlPurposeSelect.TabIndex = 3;
      this.cboPurposeValue.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPurposeValue.Font = new Font("Arial", 8.25f);
      this.cboPurposeValue.Location = new Point(18, 4);
      this.cboPurposeValue.Name = "cboPurposeValue";
      this.cboPurposeValue.Size = new Size(164, 22);
      this.cboPurposeValue.TabIndex = 1;
      this.lblPurposeDesc.AutoSize = true;
      this.lblPurposeDesc.Font = new Font("Arial", 8.25f);
      this.lblPurposeDesc.Location = new Point(2, 8);
      this.lblPurposeDesc.Name = "lblPurposeDesc";
      this.lblPurposeDesc.Size = new Size(17, 14);
      this.lblPurposeDesc.TabIndex = 0;
      this.lblPurposeDesc.Text = "of";
      this.lblPurposeDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlCompareSelect.Controls.Add((Control) this.lblCompareUnits);
      this.pnlCompareSelect.Controls.Add((Control) this.txtCompareAmount);
      this.pnlCompareSelect.Controls.Add((Control) this.cboCompareOperators);
      this.pnlCompareSelect.Controls.Add((Control) this.lblCompareDesc);
      this.pnlCompareSelect.Location = new Point(0, 56);
      this.pnlCompareSelect.Name = "pnlCompareSelect";
      this.pnlCompareSelect.Size = new Size(437, 28);
      this.pnlCompareSelect.TabIndex = 2;
      this.lblCompareUnits.AutoSize = true;
      this.lblCompareUnits.Font = new Font("Arial", 8.25f);
      this.lblCompareUnits.Location = new Point(324, 8);
      this.lblCompareUnits.Name = "lblCompareUnits";
      this.lblCompareUnits.Size = new Size(39, 14);
      this.lblCompareUnits.TabIndex = 3;
      this.lblCompareUnits.Text = "dollars";
      this.lblCompareUnits.TextAlign = ContentAlignment.MiddleLeft;
      this.txtCompareAmount.Font = new Font("Arial", 8.25f);
      this.txtCompareAmount.Location = new Point(240, 5);
      this.txtCompareAmount.Name = "txtCompareAmount";
      this.txtCompareAmount.Size = new Size(80, 20);
      this.txtCompareAmount.TabIndex = 2;
      this.txtCompareAmount.Text = "999,999,999.99";
      this.txtCompareAmount.TextAlign = HorizontalAlignment.Right;
      this.cboCompareOperators.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCompareOperators.Font = new Font("Arial", 8.25f);
      this.cboCompareOperators.Items.AddRange(new object[3]
      {
        (object) "less than",
        (object) "equal to",
        (object) "greater than"
      });
      this.cboCompareOperators.Location = new Point(151, 4);
      this.cboCompareOperators.Name = "cboCompareOperators";
      this.cboCompareOperators.Size = new Size(85, 22);
      this.cboCompareOperators.TabIndex = 1;
      this.lblCompareDesc.AutoSize = true;
      this.lblCompareDesc.Font = new Font("Arial", 8.25f);
      this.lblCompareDesc.Location = new Point(2, 8);
      this.lblCompareDesc.Name = "lblCompareDesc";
      this.lblCompareDesc.Size = new Size(134, 14);
      this.lblCompareDesc.TabIndex = 0;
      this.lblCompareDesc.Text = "and whose loan amount is";
      this.lblCompareDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.btnTest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnTest.Enabled = false;
      this.btnTest.Location = new Point(556, 10);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new Size(75, 22);
      this.btnTest.TabIndex = 16;
      this.btnTest.Text = "Test";
      this.btnTest.Click += new EventHandler(this.btnTest_Click);
      this.cboPredefinedQuery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboPredefinedQuery.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPredefinedQuery.Font = new Font("Arial", 8.25f);
      this.cboPredefinedQuery.Location = new Point(412, 31);
      this.cboPredefinedQuery.Name = "cboPredefinedQuery";
      this.cboPredefinedQuery.Size = new Size(256, 22);
      this.cboPredefinedQuery.TabIndex = 0;
      this.cboPredefinedQuery.Visible = false;
      this.cboPredefinedQuery.SelectedIndexChanged += new EventHandler(this.cboPredefinedQuery_SelectedIndexChanged);
      this.pnlSearchOption.Controls.Add((Control) this.cboAdvancedSearch);
      this.pnlSearchOption.Controls.Add((Control) this.icnAdvancedQuery);
      this.pnlSearchOption.Controls.Add((Control) this.cboPredefinedQuery);
      this.pnlSearchOption.Controls.Add((Control) this.lblSearchOption);
      this.pnlSearchOption.Controls.Add((Control) this.cboSearchOption);
      this.pnlSearchOption.Controls.Add((Control) this.lblAddOrRemove);
      this.pnlSearchOption.Dock = DockStyle.Top;
      this.pnlSearchOption.Location = new Point(0, 0);
      this.pnlSearchOption.Name = "pnlSearchOption";
      this.pnlSearchOption.Size = new Size(671, 51);
      this.pnlSearchOption.TabIndex = 19;
      this.pnlSearchOption.Visible = false;
      this.cboAdvancedSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboAdvancedSearch.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboAdvancedSearch.Font = new Font("Arial", 8.25f);
      this.cboAdvancedSearch.Location = new Point(412, 3);
      this.cboAdvancedSearch.Name = "cboAdvancedSearch";
      this.cboAdvancedSearch.Size = new Size(256, 22);
      this.cboAdvancedSearch.TabIndex = 4;
      this.cboAdvancedSearch.Visible = false;
      this.cboAdvancedSearch.SelectionChangeCommitted += new EventHandler(this.cboAdvancedSearch_SelectionChangeCommitted);
      this.icnAdvancedQuery.BackColor = Color.Transparent;
      this.icnAdvancedQuery.Location = new Point(241, 33);
      this.icnAdvancedQuery.Name = "icnAdvancedQuery";
      this.icnAdvancedQuery.Size = new Size(16, 16);
      this.icnAdvancedQuery.StandardButtonType = StandardIconButton.ButtonType.SearchButton;
      this.icnAdvancedQuery.TabIndex = 3;
      this.icnAdvancedQuery.TabStop = false;
      this.icnAdvancedQuery.Visible = false;
      this.icnAdvancedQuery.Click += new EventHandler(this.icnAdvancedQuery_Click);
      this.lblSearchOption.AutoSize = true;
      this.lblSearchOption.Location = new Point(33, 35);
      this.lblSearchOption.Name = "lblSearchOption";
      this.lblSearchOption.Size = new Size(79, 14);
      this.lblSearchOption.TabIndex = 2;
      this.lblSearchOption.Text = "Search Option:";
      this.cboSearchOption.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSearchOption.FormattingEnabled = true;
      this.cboSearchOption.Items.AddRange(new object[3]
      {
        (object) "Predefined Search",
        (object) "Saved Search",
        (object) "Advanced Search"
      });
      this.cboSearchOption.Location = new Point(114, 31);
      this.cboSearchOption.Name = "cboSearchOption";
      this.cboSearchOption.Size = new Size(121, 22);
      this.cboSearchOption.TabIndex = 1;
      this.cboSearchOption.SelectionChangeCommitted += new EventHandler(this.cboSearchOption_SelectionChangeCommitted);
      this.lblAddOrRemove.AutoSize = true;
      this.lblAddOrRemove.Location = new Point(33, 10);
      this.lblAddOrRemove.Name = "lblAddOrRemove";
      this.lblAddOrRemove.Size = new Size(318, 14);
      this.lblAddOrRemove.TabIndex = 0;
      this.lblAddOrRemove.Text = "Build a search for contacts to automatically add to the campaign.";
      this.lblAddOrRemove.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlMain.Controls.Add((Control) this.lblFilters);
      this.pnlMain.Controls.Add((Control) this.btnTest);
      this.pnlMain.Controls.Add((Control) this.grpFilters);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 51);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(671, 298);
      this.pnlMain.TabIndex = 26;
      this.lblFilters.AutoSize = true;
      this.lblFilters.Location = new Point(33, 15);
      this.lblFilters.Name = "lblFilters";
      this.lblFilters.Size = new Size(36, 14);
      this.lblFilters.TabIndex = 26;
      this.lblFilters.Text = "Filters";
      this.lblFilters.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlPrimaryOnly.Controls.Add((Control) this.label1);
      this.pnlPrimaryOnly.Controls.Add((Control) this.lblNote);
      this.pnlPrimaryOnly.Controls.Add((Control) this.chkPrimaryOnly);
      this.pnlPrimaryOnly.Dock = DockStyle.Bottom;
      this.pnlPrimaryOnly.Location = new Point(0, 349);
      this.pnlPrimaryOnly.Name = "pnlPrimaryOnly";
      this.pnlPrimaryOnly.Size = new Size(671, 65);
      this.pnlPrimaryOnly.TabIndex = 27;
      this.pnlPrimaryOnly.Visible = false;
      this.lblNote.AutoSize = false;
      this.lblNote.Location = new Point(36, 25);
      this.lblNote.Name = "lblNote";
      this.lblNote.Size = new Size(33, 17);
      this.lblNote.TabIndex = 69;
      this.lblNote.Text = "<b>Note:</b>";
      this.chkPrimaryOnly.AutoSize = true;
      this.chkPrimaryOnly.Location = new Point(37, 6);
      this.chkPrimaryOnly.Name = "chkPrimaryOnly";
      this.chkPrimaryOnly.Size = new Size(156, 18);
      this.chkPrimaryOnly.TabIndex = 68;
      this.chkPrimaryOnly.Text = "Add primary contacts only.";
      this.chkPrimaryOnly.CheckedChanged += new EventHandler(this.chkPrimaryOnly_CheckedChanged);
      this.label1.Location = new Point(75, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(593, 28);
      this.label1.TabIndex = 70;
      this.label1.Text = "Contacts who have opted-out from phone calls, emails, mail and faxes will be automatically screened when you run  the campaign.";
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.pnlPrimaryOnly);
      this.Controls.Add((Control) this.pnlSearchOption);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Name = nameof (WizardQueryControl);
      this.Size = new Size(671, 414);
      this.Leave += new EventHandler(this.CampaignQueryControl_Leave);
      this.grpFilters.ResumeLayout(false);
      this.pnlAdvancedFilters.ResumeLayout(false);
      this.pnlPredefinedFilters.ResumeLayout(false);
      this.pnlNumericSelect.ResumeLayout(false);
      this.pnlNumericSelect.PerformLayout();
      this.nudNumericValue.EndInit();
      this.pnlComboSelect.ResumeLayout(false);
      this.pnlComboSelect.PerformLayout();
      this.pnlPurposeSelect.ResumeLayout(false);
      this.pnlPurposeSelect.PerformLayout();
      this.pnlCompareSelect.ResumeLayout(false);
      this.pnlCompareSelect.PerformLayout();
      this.pnlSearchOption.ResumeLayout(false);
      this.pnlSearchOption.PerformLayout();
      ((ISupportInitialize) this.icnAdvancedQuery).EndInit();
      this.pnlMain.ResumeLayout(false);
      this.pnlMain.PerformLayout();
      this.pnlPrimaryOnly.ResumeLayout(false);
      this.pnlPrimaryOnly.PerformLayout();
      this.ResumeLayout(false);
    }

    public class PredefinedQuery
    {
      public bool IsAdd;
      public bool IsDelete;
      public bool IsBorrower;
      public bool IsPartner;
      public string Name;

      public PredefinedQuery(
        bool isAdd,
        bool isDelete,
        bool isBorrower,
        bool isPartner,
        string name)
      {
        this.IsAdd = isAdd;
        this.IsDelete = isDelete;
        this.IsBorrower = isBorrower;
        this.IsPartner = isPartner;
        this.Name = name;
      }
    }
  }
}
