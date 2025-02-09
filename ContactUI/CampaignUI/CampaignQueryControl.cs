// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignQueryControl
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignQueryControl : UserControl
  {
    private CampaignQueryControl.PredefinedQuery[] predefinedQueries = new CampaignQueryControl.PredefinedQuery[10]
    {
      new CampaignQueryControl.PredefinedQuery(true, true, true, true, ""),
      new CampaignQueryControl.PredefinedQuery(true, false, true, false, "Imported or Newly Created Borrower Contacts"),
      new CampaignQueryControl.PredefinedQuery(true, true, true, false, "Borrower Contact Type"),
      new CampaignQueryControl.PredefinedQuery(true, true, true, false, "Borrower Contact Status"),
      new CampaignQueryControl.PredefinedQuery(true, false, true, false, "Last Loan Originated"),
      new CampaignQueryControl.PredefinedQuery(true, false, true, false, "Last Loan Closed"),
      new CampaignQueryControl.PredefinedQuery(true, false, false, true, "Imported or Newly Created Business Contacts"),
      new CampaignQueryControl.PredefinedQuery(true, true, false, true, "Business Contact Category"),
      new CampaignQueryControl.PredefinedQuery(true, false, true, false, "Birthday"),
      new CampaignQueryControl.PredefinedQuery(true, false, true, false, "Anniversary")
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
    private const int pnlStartLocationY = 16;
    private int pnlLocationX = 3;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private EllieMae.EMLite.ContactGroup.ContactQuery campaignQuery;
    private QueryEditMode queryEditMode;
    private bool displayWizardOptions;
    private bool displayingPredefinedQuery;
    private string queryName = string.Empty;
    private CampaignFrequencyNameProvider frequencyNames = new CampaignFrequencyNameProvider();
    private BizCategoryUtil catUtil;
    private Sessions.Session session;
    private GroupBox grpParameters;
    private ComboBox cboPredefinedQueries;
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
    private Panel pnlAddOption;
    private Label lblFrequencyType;
    private Label lblDays;
    private Label lblSelectQueryType;
    private ComboBox cboFrequencyType;
    private Panel pnlDeleteOption;
    private CheckBox chkDeleteQuery;
    private Label lblFrequencyText;
    private Label lblFrequency;
    private RadioButton rbPredefinedQuery;
    private RadioButton rbAdvancedQuery;
    private Button btnPredefinedTest;
    private Button btnAdvancedQuery;
    private Button btnAdvancedTest;
    private GroupBox grpQueryResult;
    private Panel pnlMain;
    private NumericUpDown nudFrequencyDays;
    private Label lblAdvancedQueryDescription;
    private CheckBox chkPrimaryOnly;
    private Panel pnlPrimaryOnly;

    public CampaignQueryControl(
      Sessions.Session session,
      QueryEditMode queryEditMode,
      bool displayWizardOptions,
      EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.session = session;
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
      bool flag1 = this.queryEditMode == QueryEditMode.AddQuery;
      bool flag2 = this.campaign.ContactType == ContactType.Borrower;
      foreach (CampaignQueryControl.PredefinedQuery predefinedQuery in this.predefinedQueries)
      {
        if ((flag1 && predefinedQuery.IsAdd || !flag1 && predefinedQuery.IsDelete) && (flag2 && predefinedQuery.IsBorrower || !flag2 && predefinedQuery.IsPartner))
          this.cboPredefinedQueries.Items.Add((object) predefinedQuery.Name);
      }
      this.cboPredefinedQueries.SelectedIndex = 0;
      if (this.queryEditMode == QueryEditMode.AddQuery)
      {
        this.campaignQuery = this.campaign.AddQuery;
        this.cboFrequencyType.Items.Clear();
        this.cboFrequencyType.Items.AddRange((object[]) this.frequencyNames.GetNames());
        this.cboFrequencyType.SelectedIndex = this.cboFrequencyType.Items.IndexOf((object) this.frequencyNames.GetName((object) this.campaign.FrequencyType));
        if (CampaignFrequencyType.Custom == this.campaign.FrequencyType)
          this.nudFrequencyDays.Value = (Decimal) this.campaign.FrequencyInterval;
        if (CampaignFrequencyType.Custom == (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString()))
        {
          this.nudFrequencyDays.Visible = true;
          this.lblDays.Visible = true;
        }
        else
        {
          this.nudFrequencyDays.Visible = false;
          this.lblDays.Visible = false;
        }
        this.pnlAddOption.Visible = this.displayWizardOptions;
        if (this.campaign.ContactType == ContactType.Borrower)
        {
          this.chkPrimaryOnly.Checked = this.campaignQuery.PrimaryOnly;
          this.pnlPrimaryOnly.Visible = true;
        }
      }
      else
      {
        if ((CampaignType.AutoDeleteQuery & this.campaign.CampaignType) == CampaignType.AutoDeleteQuery)
        {
          this.chkDeleteQuery.Checked = true;
          this.campaignQuery = this.campaign.DeleteQuery;
        }
        else
        {
          this.chkDeleteQuery.Checked = false;
          this.rbPredefinedQuery.Enabled = false;
          this.rbAdvancedQuery.Enabled = false;
        }
        if (CampaignFrequencyType.Custom != this.campaign.FrequencyType)
          this.lblFrequency.Text = this.frequencyNames.GetName((object) this.campaign.FrequencyType);
        else
          this.lblFrequency.Text = "Every " + (object) this.campaign.FrequencyInterval + " days";
        this.pnlDeleteOption.Visible = this.displayWizardOptions;
      }
      if (this.campaignQuery == null || (ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType) == ContactQueryType.CampaignPredefinedQuery)
      {
        this.rbPredefinedQuery.Checked = true;
        this.cboPredefinedQueries.Enabled = this.queryEditMode == QueryEditMode.AddQuery || this.chkDeleteQuery.Checked;
        this.rbAdvancedQuery.Checked = false;
        this.btnAdvancedQuery.Enabled = false;
        this.btnAdvancedTest.Enabled = false;
        if (this.campaignQuery == null || !(string.Empty != this.campaignQuery.XmlQueryString))
          return;
        this.displayPredefinedQuery();
      }
      else
      {
        this.rbPredefinedQuery.Checked = false;
        this.cboPredefinedQueries.Enabled = false;
        this.rbAdvancedQuery.Checked = true;
        this.btnAdvancedQuery.Enabled = true;
        this.btnAdvancedTest.Enabled = true;
        try
        {
          EllieMae.EMLite.ClientServer.Contacts.ContactQuery query = (EllieMae.EMLite.ClientServer.Contacts.ContactQuery) QueryXmlConverter.Deserialize(typeof (EllieMae.EMLite.ClientServer.Contacts.ContactQuery), this.campaignQuery.XmlQueryString);
          ClientContactSearchUtil contactSearchUtil = new ClientContactSearchUtil(query.LoanMatchType, this.campaignQuery.ContactType);
          contactSearchUtil.FlushSearchObjectsToSql(query, this.campaignQuery.ContactType);
          StringBuilder stringBuilder = new StringBuilder(ClientContactSearchUtil.getSearchDescriptionHeading(query, this.campaignQuery.ContactType) + contactSearchUtil.Description);
          stringBuilder.Replace("Showing b", "B");
          this.lblAdvancedQueryDescription.Text = stringBuilder.ToString();
        }
        catch
        {
        }
      }
    }

    private void cboFrequencyType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (CampaignFrequencyType.Custom == (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString()))
      {
        this.nudFrequencyDays.Minimum = 1M;
        this.nudFrequencyDays.Visible = true;
        this.nudFrequencyDays.Enabled = true;
        this.lblDays.Visible = true;
      }
      else
      {
        this.nudFrequencyDays.Minimum = 0M;
        this.nudFrequencyDays.Value = 0M;
        this.nudFrequencyDays.Visible = false;
        this.lblDays.Visible = false;
      }
    }

    private void displayPredefinedQuery()
    {
      this.displayingPredefinedQuery = true;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(this.campaignQuery.XmlQueryString);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string attribute = documentElement.GetAttribute("Name");
      this.cboPredefinedQueries.SelectedItem = (object) attribute;
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
            this.catUtil = new BizCategoryUtil(this.session.SessionObjects);
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
      this.setNumericSelect((this.campaignQuery.ContactType == ContactType.Borrower ? "Borrowers" : "Business contacts") + " imported or created in the previous", days, nameof (days));
      this.btnPredefinedTest.Enabled = true;
    }

    private void setBorrowerBirthday(string condition)
    {
      if (this.birthdayConditions == null)
        this.birthdayConditions = MonthWeekEnumUtil.GetDisplayNamesNoExactDate();
      this.setComboSelect(121, "Borrowers whose birthday is in the", this.birthdayConditions, condition);
      this.btnPredefinedTest.Enabled = true;
    }

    private void setBorrowerAnniversary(string condition)
    {
      if (this.anniversaryConditions == null)
        this.anniversaryConditions = MonthWeekEnumUtil.GetDisplayNamesNoExactDate();
      this.setComboSelect(121, "Borrowers whose anniversary is in the", this.anniversaryConditions, condition);
      this.btnPredefinedTest.Enabled = true;
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
      this.setComboSelect(250, "Borrowers whose contact type is", this.borrowerTypes, borrowerType);
      this.btnPredefinedTest.Enabled = true;
    }

    private void setBorrowerStatus(string borrowerStatus)
    {
      if (this.borrowerStatuses == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (BorrowerStatusItem borrowerStatusItem in this.session.ContactManager.GetBorrowerStatus().Items)
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
        this.setComboSelect(250, "Borrowers whose contact status is", this.borrowerStatuses, borrowerStatus);
        this.btnPredefinedTest.Enabled = true;
      }
    }

    private void setLastLoanOriginated(int days)
    {
      this.setNumericSelect("Borrowers whose last loan was originated in the previous", days, nameof (days));
      this.btnPredefinedTest.Enabled = true;
    }

    private void setLastLoanClosed(ArrayList parms)
    {
      if (string.Empty == parms[0].ToString() || this.displayingPredefinedQuery)
      {
        this.setComboSelect(121, "Borrowers whose last closed loan had a", this.loanFields, parms[0].ToString());
        if (!this.displayingPredefinedQuery)
          return;
      }
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
      this.btnPredefinedTest.Enabled = true;
    }

    private void setPartnerCategory(string partnerCategory)
    {
      if (this.partnerCategories == null)
      {
        ArrayList arrayList = new ArrayList();
        foreach (BizCategory bizCategory in this.session.ContactManager.GetBizCategories())
        {
          if (!(string.Empty == bizCategory.Name))
            arrayList.Add((object) bizCategory.Name);
        }
        this.partnerCategories = (string[]) arrayList.ToArray(typeof (string));
      }
      this.setComboSelect(250, "Business contacts whose contact category is", this.partnerCategories, partnerCategory);
      this.btnPredefinedTest.Enabled = true;
    }

    private void setNumericSelect(string description, int initialValue, string units)
    {
      this.lblNumericDesc.Text = description;
      this.nudNumericValue.Value = (Decimal) initialValue;
      this.lblNumericUnits.Text = units;
      this.nudNumericValue.Left = this.lblNumericDesc.Location.X + this.lblNumericDesc.Size.Width - 8;
      this.lblNumericUnits.Left = this.nudNumericValue.Location.X + this.nudNumericValue.Size.Width;
      this.nudNumericValue.BringToFront();
      this.pnlNumericSelect.Width = this.lblNumericUnits.Location.X + this.lblNumericUnits.Size.Width;
      this.pnlNumericSelect.Location = new Point(this.pnlLocationX, 16);
      this.pnlLocationX += this.pnlNumericSelect.Width;
      this.pnlNumericSelect.Visible = true;
    }

    private void setComboSelect(
      int comboValueWidth,
      string description,
      string[] selections,
      string selection)
    {
      this.lblComboDesc.Text = description;
      this.cboComboValue.Items.Clear();
      this.cboComboValue.Items.AddRange((object[]) selections);
      this.cboComboValue.SelectedItem = (object) selection;
      this.cboComboValue.Left = this.lblComboDesc.Location.X + this.lblComboDesc.Size.Width - 8;
      this.cboComboValue.Width = comboValueWidth;
      this.pnlComboSelect.Width = this.cboComboValue.Location.X + this.cboComboValue.Size.Width;
      this.pnlComboSelect.Location = new Point(this.pnlLocationX, 16);
      this.pnlLocationX += this.pnlComboSelect.Width - 8;
      this.pnlComboSelect.Visible = true;
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
      this.cboCompareOperators.Left = this.lblCompareDesc.Location.X + this.lblCompareDesc.Size.Width - 2;
      this.txtCompareAmount.Left = this.cboCompareOperators.Location.X + this.cboCompareOperators.Size.Width + 2;
      this.lblCompareUnits.Left = this.txtCompareAmount.Location.X + this.txtCompareAmount.Size.Width;
      this.pnlCompareSelect.Width = this.lblCompareUnits.Location.X + this.lblCompareUnits.Size.Width;
      this.pnlCompareSelect.Location = new Point(this.pnlLocationX, 16);
      this.pnlLocationX += this.pnlCompareSelect.Width;
      this.pnlCompareSelect.Visible = true;
    }

    private void setPurposeSelect(string[] selections, string selection)
    {
      this.cboPurposeValue.Items.Clear();
      this.cboPurposeValue.Items.AddRange((object[]) selections);
      this.cboPurposeValue.SelectedItem = (object) selection;
      this.pnlPurposeSelect.Width = this.cboPurposeValue.Location.X + this.cboPurposeValue.Size.Width;
      this.pnlPurposeSelect.Location = new Point(this.pnlLocationX, 16);
      this.pnlLocationX += this.pnlPurposeSelect.Width;
      this.pnlPurposeSelect.Visible = true;
    }

    private void chkDeleteQuery_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkDeleteQuery.Checked)
      {
        this.campaign.CampaignType |= CampaignType.AutoDeleteQuery;
        this.campaign.CampaignType &= ~CampaignType.Manual;
        this.campaignQuery = this.campaign.DeleteQuery;
        this.rbPredefinedQuery.Enabled = true;
        this.rbPredefinedQuery.Checked = true;
        this.cboPredefinedQueries.SelectedIndex = 0;
        this.cboPredefinedQueries.Enabled = this.rbPredefinedQuery.Checked;
        this.rbAdvancedQuery.Enabled = true;
      }
      else
      {
        this.campaign.CampaignType &= ~CampaignType.AutoDeleteQuery;
        if (this.campaign.CampaignType == (CampaignType) 0)
          this.campaign.CampaignType = CampaignType.Manual;
        this.campaignQuery = (EllieMae.EMLite.ContactGroup.ContactQuery) null;
        this.rbPredefinedQuery.Enabled = false;
        this.rbPredefinedQuery.Checked = true;
        this.cboPredefinedQueries.SelectedIndex = 0;
        this.cboPredefinedQueries.Enabled = false;
        this.rbAdvancedQuery.Enabled = false;
        this.rbAdvancedQuery.Checked = false;
        this.btnAdvancedQuery.Enabled = false;
        this.btnAdvancedTest.Enabled = false;
        this.lblAdvancedQueryDescription.Text = string.Empty;
      }
    }

    private void rbSelectQuery_Click(object sender, EventArgs e)
    {
      if ((RadioButton) sender == this.rbPredefinedQuery && this.rbPredefinedQuery.Checked || (RadioButton) sender == this.rbAdvancedQuery && this.rbAdvancedQuery.Checked)
        return;
      this.rbPredefinedQuery.Checked = !this.rbPredefinedQuery.Checked;
      this.rbAdvancedQuery.Checked = !this.rbAdvancedQuery.Checked;
      if (this.rbPredefinedQuery.Checked)
      {
        this.campaignQuery.QueryType &= ~ContactQueryType.CampaignAdvancedQuery;
        this.campaignQuery.QueryType |= ContactQueryType.CampaignPredefinedQuery;
        this.campaignQuery.XmlQueryString = string.Empty;
        this.cboPredefinedQueries.Enabled = true;
        this.cboPredefinedQueries.SelectedIndex = 0;
        this.btnPredefinedTest.Enabled = false;
        this.btnAdvancedQuery.Enabled = false;
        this.btnAdvancedTest.Enabled = false;
        this.lblAdvancedQueryDescription.Text = string.Empty;
      }
      else
      {
        this.campaignQuery.QueryType &= ~ContactQueryType.CampaignPredefinedQuery;
        this.campaignQuery.QueryType |= ContactQueryType.CampaignAdvancedQuery;
        this.campaignQuery.XmlQueryString = string.Empty;
        this.cboPredefinedQueries.Enabled = false;
        this.cboPredefinedQueries.SelectedIndex = 0;
        this.btnPredefinedTest.Enabled = false;
        this.btnAdvancedQuery.Enabled = true;
        this.btnAdvancedTest.Enabled = true;
        this.lblAdvancedQueryDescription.Text = string.Empty;
      }
    }

    private void cboPredefinedQueries_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.queryName = string.Empty;
      this.pnlNumericSelect.Visible = false;
      this.pnlComboSelect.Visible = false;
      this.pnlCompareSelect.Visible = false;
      this.pnlPurposeSelect.Visible = false;
      this.pnlLocationX = 3;
      this.btnPredefinedTest.Enabled = false;
      if (0 >= this.cboPredefinedQueries.SelectedIndex)
        return;
      this.queryName = this.cboPredefinedQueries.SelectedItem.ToString();
      if (this.displayingPredefinedQuery)
        return;
      string queryName = this.queryName;
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(queryName))
      {
        case 104821391:
          if (!(queryName == "Last Loan Closed"))
            break;
          this.setLastLoanClosed(new ArrayList()
          {
            (object) string.Empty
          });
          break;
        case 1326173803:
          if (!(queryName == "Borrower Contact Status"))
            break;
          this.setBorrowerStatus("Cold Lead");
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
          this.setBorrowerType("Prospect");
          break;
        case 3243292492:
          if (!(queryName == "Birthday"))
            break;
          this.setBorrowerBirthday("Current Week");
          break;
        case 3618076121:
          if (!(queryName == "Last Loan Originated"))
            break;
          this.setLastLoanOriginated(1);
          break;
        case 4053872457:
          if (!(queryName == "Anniversary"))
            break;
          this.setBorrowerAnniversary("Current Week");
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
      if (this.campaign.ContactType != ContactType.Borrower || (ContactQueryType.CampaignAddQuery & this.campaignQuery.QueryType) != ContactQueryType.CampaignAddQuery)
        return;
      this.campaignQuery.PrimaryOnly = this.chkPrimaryOnly.Checked;
    }

    private void CampaignQueryControl_Leave(object sender, EventArgs e)
    {
      if (this.queryEditMode == QueryEditMode.AddQuery)
      {
        this.campaign.FrequencyType = (CampaignFrequencyType) this.frequencyNames.GetValue(this.cboFrequencyType.SelectedItem.ToString());
        this.campaign.FrequencyInterval = CampaignFrequencyType.Custom == this.campaign.FrequencyType ? (int) this.nudFrequencyDays.Value : 0;
      }
      else if (this.chkDeleteQuery.Checked)
      {
        this.campaign.CampaignType |= CampaignType.AutoDeleteQuery;
        this.campaign.CampaignType &= ~CampaignType.Manual;
      }
      else
      {
        this.campaign.CampaignType &= ~CampaignType.AutoDeleteQuery;
        if (this.campaign.CampaignType == (CampaignType) 0)
          this.campaign.CampaignType = CampaignType.Manual;
      }
      if (this.campaignQuery == null || (ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType) != ContactQueryType.CampaignPredefinedQuery)
        return;
      this.SavePredefinedQuery();
    }

    private void btnPredefinedTest_Click(object sender, EventArgs e)
    {
      this.SavePredefinedQuery();
      int num = (int) new ViewContactGroupDialog(this.campaignQuery).ShowDialog();
    }

    private void SavePredefinedQuery()
    {
      if ((ContactQueryType.CampaignPredefinedQuery & this.campaignQuery.QueryType) != ContactQueryType.CampaignPredefinedQuery)
        return;
      if (this.cboPredefinedQueries.SelectedIndex == 0)
      {
        this.campaignQuery.XmlQueryString = string.Empty;
      }
      else
      {
        if (!this.btnPredefinedTest.Enabled)
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
              this.catUtil = new BizCategoryUtil(this.session.SessionObjects);
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
      ContactAdvancedSearch contactAdvancedSearch = new ContactAdvancedSearch(this.session, this.campaignQuery.ContactType, this.queryEditMode == QueryEditMode.AddQuery ? ContactSearchContext.CampaignAdd : ContactSearchContext.CampaignDelete);
      try
      {
        EllieMae.EMLite.ClientServer.Contacts.ContactQuery query = (EllieMae.EMLite.ClientServer.Contacts.ContactQuery) QueryXmlConverter.Deserialize(typeof (EllieMae.EMLite.ClientServer.Contacts.ContactQuery), this.campaignQuery.XmlQueryString);
        contactAdvancedSearch.loadQuery(query);
      }
      catch
      {
      }
      DialogResult dialogResult = contactAdvancedSearch.ShowDialog();
      if (DialogResult.OK == dialogResult)
      {
        EllieMae.EMLite.ClientServer.Contacts.ContactQuery allSearchCriteria = contactAdvancedSearch.getAllSearchCriteria();
        this.campaignQuery.XmlQueryString = QueryXmlConverter.Serialize((object) allSearchCriteria);
        ClientContactSearchUtil contactSearchUtil = new ClientContactSearchUtil(allSearchCriteria.LoanMatchType, this.campaignQuery.ContactType);
        contactSearchUtil.FlushSearchObjectsToSql(allSearchCriteria, this.campaignQuery.ContactType);
        StringBuilder stringBuilder = new StringBuilder(ClientContactSearchUtil.getSearchDescriptionHeading(allSearchCriteria, this.campaignQuery.ContactType) + contactSearchUtil.Description);
        stringBuilder.Replace("Showing b", "B");
        this.lblAdvancedQueryDescription.Text = stringBuilder.ToString();
      }
      return dialogResult;
    }

    private void btnAdvanced_Click(object sender, EventArgs e)
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

    private void btnAdvancedTest_Click(object sender, EventArgs e)
    {
      if (this.campaignQuery.XmlQueryString == string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please click the Advanced Query button and select the advanced search criteria first.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        int num2 = (int) new ViewContactGroupDialog(this.campaignQuery).ShowDialog();
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
      this.grpParameters = new GroupBox();
      this.pnlPurposeSelect = new Panel();
      this.cboPurposeValue = new ComboBox();
      this.lblPurposeDesc = new Label();
      this.pnlCompareSelect = new Panel();
      this.lblCompareUnits = new Label();
      this.txtCompareAmount = new TextBox();
      this.cboCompareOperators = new ComboBox();
      this.lblCompareDesc = new Label();
      this.pnlComboSelect = new Panel();
      this.cboComboValue = new ComboBox();
      this.lblComboDesc = new Label();
      this.pnlNumericSelect = new Panel();
      this.lblNumericUnits = new Label();
      this.lblNumericDesc = new Label();
      this.nudNumericValue = new NumericUpDown();
      this.btnPredefinedTest = new Button();
      this.cboPredefinedQueries = new ComboBox();
      this.pnlAddOption = new Panel();
      this.lblDays = new Label();
      this.nudFrequencyDays = new NumericUpDown();
      this.cboFrequencyType = new ComboBox();
      this.lblFrequencyType = new Label();
      this.lblSelectQueryType = new Label();
      this.pnlDeleteOption = new Panel();
      this.lblFrequency = new Label();
      this.lblFrequencyText = new Label();
      this.chkDeleteQuery = new CheckBox();
      this.rbPredefinedQuery = new RadioButton();
      this.rbAdvancedQuery = new RadioButton();
      this.btnAdvancedQuery = new Button();
      this.btnAdvancedTest = new Button();
      this.grpQueryResult = new GroupBox();
      this.lblAdvancedQueryDescription = new Label();
      this.pnlMain = new Panel();
      this.pnlPrimaryOnly = new Panel();
      this.chkPrimaryOnly = new CheckBox();
      this.grpParameters.SuspendLayout();
      this.pnlPurposeSelect.SuspendLayout();
      this.pnlCompareSelect.SuspendLayout();
      this.pnlComboSelect.SuspendLayout();
      this.pnlNumericSelect.SuspendLayout();
      this.nudNumericValue.BeginInit();
      this.pnlAddOption.SuspendLayout();
      this.nudFrequencyDays.BeginInit();
      this.pnlDeleteOption.SuspendLayout();
      this.grpQueryResult.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.pnlPrimaryOnly.SuspendLayout();
      this.SuspendLayout();
      this.grpParameters.Controls.Add((Control) this.pnlPurposeSelect);
      this.grpParameters.Controls.Add((Control) this.pnlCompareSelect);
      this.grpParameters.Controls.Add((Control) this.pnlComboSelect);
      this.grpParameters.Controls.Add((Control) this.pnlNumericSelect);
      this.grpParameters.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpParameters.Location = new Point(40, 44);
      this.grpParameters.Name = "grpParameters";
      this.grpParameters.Size = new Size(591, 120);
      this.grpParameters.TabIndex = 18;
      this.grpParameters.TabStop = false;
      this.grpParameters.Text = "Query Options";
      this.pnlPurposeSelect.Controls.Add((Control) this.cboPurposeValue);
      this.pnlPurposeSelect.Controls.Add((Control) this.lblPurposeDesc);
      this.pnlPurposeSelect.Location = new Point(3, 100);
      this.pnlPurposeSelect.Name = "pnlPurposeSelect";
      this.pnlPurposeSelect.Size = new Size(189, 28);
      this.pnlPurposeSelect.TabIndex = 3;
      this.cboPurposeValue.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPurposeValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboPurposeValue.Location = new Point(22, 6);
      this.cboPurposeValue.Name = "cboPurposeValue";
      this.cboPurposeValue.Size = new Size(140, 21);
      this.cboPurposeValue.TabIndex = 1;
      this.lblPurposeDesc.AutoSize = true;
      this.lblPurposeDesc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPurposeDesc.Location = new Point(4, 8);
      this.lblPurposeDesc.Name = "lblPurposeDesc";
      this.lblPurposeDesc.Size = new Size(14, 16);
      this.lblPurposeDesc.TabIndex = 0;
      this.lblPurposeDesc.Text = "of";
      this.lblPurposeDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlCompareSelect.Controls.Add((Control) this.lblCompareUnits);
      this.pnlCompareSelect.Controls.Add((Control) this.txtCompareAmount);
      this.pnlCompareSelect.Controls.Add((Control) this.cboCompareOperators);
      this.pnlCompareSelect.Controls.Add((Control) this.lblCompareDesc);
      this.pnlCompareSelect.Location = new Point(3, 72);
      this.pnlCompareSelect.Name = "pnlCompareSelect";
      this.pnlCompareSelect.Size = new Size(437, 28);
      this.pnlCompareSelect.TabIndex = 2;
      this.lblCompareUnits.AutoSize = true;
      this.lblCompareUnits.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCompareUnits.Location = new Point(324, 8);
      this.lblCompareUnits.Name = "lblCompareUnits";
      this.lblCompareUnits.Size = new Size(38, 16);
      this.lblCompareUnits.TabIndex = 3;
      this.lblCompareUnits.Text = "dollars";
      this.lblCompareUnits.TextAlign = ContentAlignment.MiddleLeft;
      this.txtCompareAmount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtCompareAmount.Location = new Point(240, 6);
      this.txtCompareAmount.Name = "txtCompareAmount";
      this.txtCompareAmount.Size = new Size(80, 20);
      this.txtCompareAmount.TabIndex = 2;
      this.txtCompareAmount.Text = "999,999,999.99";
      this.txtCompareAmount.TextAlign = HorizontalAlignment.Right;
      this.cboCompareOperators.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboCompareOperators.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboCompareOperators.Items.AddRange(new object[3]
      {
        (object) "less than",
        (object) "equal to",
        (object) "greater than"
      });
      this.cboCompareOperators.Location = new Point(151, 6);
      this.cboCompareOperators.Name = "cboCompareOperators";
      this.cboCompareOperators.Size = new Size(85, 21);
      this.cboCompareOperators.TabIndex = 1;
      this.lblCompareDesc.AutoSize = true;
      this.lblCompareDesc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblCompareDesc.Location = new Point(4, 8);
      this.lblCompareDesc.Name = "lblCompareDesc";
      this.lblCompareDesc.Size = new Size(135, 16);
      this.lblCompareDesc.TabIndex = 0;
      this.lblCompareDesc.Text = "and whose loan amount is";
      this.lblCompareDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlComboSelect.Controls.Add((Control) this.cboComboValue);
      this.pnlComboSelect.Controls.Add((Control) this.lblComboDesc);
      this.pnlComboSelect.Location = new Point(3, 44);
      this.pnlComboSelect.Name = "pnlComboSelect";
      this.pnlComboSelect.Size = new Size(437, 28);
      this.pnlComboSelect.TabIndex = 1;
      this.cboComboValue.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboComboValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboComboValue.Location = new Point(201, 6);
      this.cboComboValue.Name = "cboComboValue";
      this.cboComboValue.Size = new Size(121, 21);
      this.cboComboValue.TabIndex = 3;
      this.cboComboValue.SelectedIndexChanged += new EventHandler(this.cboComboValue_SelectedIndexChanged);
      this.lblComboDesc.AutoSize = true;
      this.lblComboDesc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblComboDesc.Location = new Point(6, 8);
      this.lblComboDesc.Name = "lblComboDesc";
      this.lblComboDesc.Size = new Size(167, 16);
      this.lblComboDesc.TabIndex = 2;
      this.lblComboDesc.Text = "Borrowers whose contact type is";
      this.lblComboDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlNumericSelect.Controls.Add((Control) this.lblNumericUnits);
      this.pnlNumericSelect.Controls.Add((Control) this.lblNumericDesc);
      this.pnlNumericSelect.Controls.Add((Control) this.nudNumericValue);
      this.pnlNumericSelect.Location = new Point(3, 16);
      this.pnlNumericSelect.Name = "pnlNumericSelect";
      this.pnlNumericSelect.Size = new Size(437, 28);
      this.pnlNumericSelect.TabIndex = 0;
      this.lblNumericUnits.AutoSize = true;
      this.lblNumericUnits.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNumericUnits.Location = new Point(289, 8);
      this.lblNumericUnits.Name = "lblNumericUnits";
      this.lblNumericUnits.Size = new Size(28, 16);
      this.lblNumericUnits.TabIndex = 2;
      this.lblNumericUnits.Text = "days";
      this.lblNumericUnits.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNumericDesc.AutoSize = true;
      this.lblNumericDesc.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblNumericDesc.Location = new Point(6, 8);
      this.lblNumericDesc.Name = "lblNumericDesc";
      this.lblNumericDesc.Size = new Size(236, 16);
      this.lblNumericDesc.TabIndex = 1;
      this.lblNumericDesc.Text = "Borrowers imported or created in the previous ";
      this.lblNumericDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.nudNumericValue.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.nudNumericValue.Location = new Point(242, 6);
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
      this.btnPredefinedTest.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPredefinedTest.Enabled = false;
      this.btnPredefinedTest.Location = new Point(404, 13);
      this.btnPredefinedTest.Name = "btnPredefinedTest";
      this.btnPredefinedTest.TabIndex = 16;
      this.btnPredefinedTest.Text = "Test";
      this.btnPredefinedTest.Click += new EventHandler(this.btnPredefinedTest_Click);
      this.cboPredefinedQueries.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cboPredefinedQueries.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboPredefinedQueries.Enabled = false;
      this.cboPredefinedQueries.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cboPredefinedQueries.Location = new Point(140, 14);
      this.cboPredefinedQueries.Name = "cboPredefinedQueries";
      this.cboPredefinedQueries.Size = new Size(256, 21);
      this.cboPredefinedQueries.TabIndex = 0;
      this.cboPredefinedQueries.SelectedIndexChanged += new EventHandler(this.cboPredefinedQueries_SelectedIndexChanged);
      this.pnlAddOption.Controls.Add((Control) this.lblDays);
      this.pnlAddOption.Controls.Add((Control) this.nudFrequencyDays);
      this.pnlAddOption.Controls.Add((Control) this.cboFrequencyType);
      this.pnlAddOption.Controls.Add((Control) this.lblFrequencyType);
      this.pnlAddOption.Controls.Add((Control) this.lblSelectQueryType);
      this.pnlAddOption.Dock = DockStyle.Top;
      this.pnlAddOption.Location = new Point(0, 0);
      this.pnlAddOption.Name = "pnlAddOption";
      this.pnlAddOption.Size = new Size(671, 40);
      this.pnlAddOption.TabIndex = 19;
      this.pnlAddOption.Visible = false;
      this.lblDays.Location = new Point(464, 16);
      this.lblDays.Name = "lblDays";
      this.lblDays.Size = new Size(36, 23);
      this.lblDays.TabIndex = 4;
      this.lblDays.Text = "days";
      this.lblDays.TextAlign = ContentAlignment.MiddleLeft;
      this.nudFrequencyDays.Location = new Point(412, 16);
      this.nudFrequencyDays.Maximum = new Decimal(new int[4]
      {
        999,
        0,
        0,
        0
      });
      this.nudFrequencyDays.Name = "nudFrequencyDays";
      this.nudFrequencyDays.Size = new Size(48, 20);
      this.nudFrequencyDays.TabIndex = 3;
      this.nudFrequencyDays.Value = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.cboFrequencyType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboFrequencyType.Location = new Point(320, 16);
      this.cboFrequencyType.Name = "cboFrequencyType";
      this.cboFrequencyType.Size = new Size(84, 21);
      this.cboFrequencyType.TabIndex = 2;
      this.cboFrequencyType.SelectedIndexChanged += new EventHandler(this.cboFrequencyType_SelectedIndexChanged);
      this.lblFrequencyType.Location = new Point(240, 16);
      this.lblFrequencyType.Name = "lblFrequencyType";
      this.lblFrequencyType.Size = new Size(80, 23);
      this.lblFrequencyType.TabIndex = 1;
      this.lblFrequencyType.Text = "Run the query:";
      this.lblFrequencyType.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSelectQueryType.Location = new Point(7, 16);
      this.lblSelectQueryType.Name = "lblSelectQueryType";
      this.lblSelectQueryType.Size = new Size(233, 23);
      this.lblSelectQueryType.TabIndex = 0;
      this.lblSelectQueryType.Text = "Select a predefined query or create your own.";
      this.lblSelectQueryType.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlDeleteOption.Controls.Add((Control) this.lblFrequency);
      this.pnlDeleteOption.Controls.Add((Control) this.lblFrequencyText);
      this.pnlDeleteOption.Controls.Add((Control) this.chkDeleteQuery);
      this.pnlDeleteOption.Dock = DockStyle.Top;
      this.pnlDeleteOption.Location = new Point(0, 40);
      this.pnlDeleteOption.Name = "pnlDeleteOption";
      this.pnlDeleteOption.Size = new Size(671, 40);
      this.pnlDeleteOption.TabIndex = 20;
      this.pnlDeleteOption.Visible = false;
      this.lblFrequency.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFrequency.Location = new Point(392, 17);
      this.lblFrequency.Name = "lblFrequency";
      this.lblFrequency.Size = new Size(140, 23);
      this.lblFrequency.TabIndex = 2;
      this.lblFrequency.Text = "Monthly";
      this.lblFrequency.TextAlign = ContentAlignment.MiddleLeft;
      this.lblFrequencyText.Location = new Point(284, 17);
      this.lblFrequencyText.Name = "lblFrequencyText";
      this.lblFrequencyText.Size = new Size(112, 23);
      this.lblFrequencyText.TabIndex = 1;
      this.lblFrequencyText.Text = "The query will be run:";
      this.lblFrequencyText.TextAlign = ContentAlignment.MiddleLeft;
      this.chkDeleteQuery.Location = new Point(7, 16);
      this.chkDeleteQuery.Name = "chkDeleteQuery";
      this.chkDeleteQuery.Size = new Size(285, 24);
      this.chkDeleteQuery.TabIndex = 0;
      this.chkDeleteQuery.Text = "Use a query to remove contacts from the campaign.";
      this.chkDeleteQuery.CheckedChanged += new EventHandler(this.chkDeleteQuery_CheckedChanged);
      this.rbPredefinedQuery.AutoCheck = false;
      this.rbPredefinedQuery.Location = new Point(23, 12);
      this.rbPredefinedQuery.Name = "rbPredefinedQuery";
      this.rbPredefinedQuery.Size = new Size(125, 24);
      this.rbPredefinedQuery.TabIndex = 21;
      this.rbPredefinedQuery.Text = "Predefined Queries:";
      this.rbPredefinedQuery.Click += new EventHandler(this.rbSelectQuery_Click);
      this.rbAdvancedQuery.AutoCheck = false;
      this.rbAdvancedQuery.Location = new Point(23, 172);
      this.rbAdvancedQuery.Name = "rbAdvancedQuery";
      this.rbAdvancedQuery.Size = new Size(17, 24);
      this.rbAdvancedQuery.TabIndex = 22;
      this.rbAdvancedQuery.Click += new EventHandler(this.rbSelectQuery_Click);
      this.btnAdvancedQuery.Enabled = false;
      this.btnAdvancedQuery.Location = new Point(40, 173);
      this.btnAdvancedQuery.Name = "btnAdvancedQuery";
      this.btnAdvancedQuery.Size = new Size(108, 23);
      this.btnAdvancedQuery.TabIndex = 23;
      this.btnAdvancedQuery.Text = "Advanced Query";
      this.btnAdvancedQuery.Click += new EventHandler(this.btnAdvanced_Click);
      this.btnAdvancedTest.Enabled = false;
      this.btnAdvancedTest.Location = new Point(156, 173);
      this.btnAdvancedTest.Name = "btnAdvancedTest";
      this.btnAdvancedTest.TabIndex = 24;
      this.btnAdvancedTest.Text = "Test";
      this.btnAdvancedTest.Click += new EventHandler(this.btnAdvancedTest_Click);
      this.grpQueryResult.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpQueryResult.Controls.Add((Control) this.lblAdvancedQueryDescription);
      this.grpQueryResult.Location = new Point(40, 210);
      this.grpQueryResult.Name = "grpQueryResult";
      this.grpQueryResult.Size = new Size(591, 120);
      this.grpQueryResult.TabIndex = 25;
      this.grpQueryResult.TabStop = false;
      this.grpQueryResult.Text = "Query Result";
      this.lblAdvancedQueryDescription.Location = new Point(8, 20);
      this.lblAdvancedQueryDescription.Name = "lblAdvancedQueryDescription";
      this.lblAdvancedQueryDescription.Size = new Size(576, 92);
      this.lblAdvancedQueryDescription.TabIndex = 0;
      this.pnlMain.Controls.Add((Control) this.cboPredefinedQueries);
      this.pnlMain.Controls.Add((Control) this.btnAdvancedTest);
      this.pnlMain.Controls.Add((Control) this.btnAdvancedQuery);
      this.pnlMain.Controls.Add((Control) this.rbAdvancedQuery);
      this.pnlMain.Controls.Add((Control) this.grpQueryResult);
      this.pnlMain.Controls.Add((Control) this.rbPredefinedQuery);
      this.pnlMain.Controls.Add((Control) this.btnPredefinedTest);
      this.pnlMain.Controls.Add((Control) this.grpParameters);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 80);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(671, 410);
      this.pnlMain.TabIndex = 26;
      this.pnlPrimaryOnly.Controls.Add((Control) this.chkPrimaryOnly);
      this.pnlPrimaryOnly.Dock = DockStyle.Bottom;
      this.pnlPrimaryOnly.Location = new Point(0, 450);
      this.pnlPrimaryOnly.Name = "pnlPrimaryOnly";
      this.pnlPrimaryOnly.Size = new Size(671, 40);
      this.pnlPrimaryOnly.TabIndex = 27;
      this.pnlPrimaryOnly.Visible = false;
      this.chkPrimaryOnly.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkPrimaryOnly.Location = new Point(23, 8);
      this.chkPrimaryOnly.Name = "chkPrimaryOnly";
      this.chkPrimaryOnly.Size = new Size(233, 24);
      this.chkPrimaryOnly.TabIndex = 68;
      this.chkPrimaryOnly.Text = "Automatically add primary contacts only.";
      this.chkPrimaryOnly.CheckedChanged += new EventHandler(this.chkPrimaryOnly_CheckedChanged);
      this.Controls.Add((Control) this.pnlPrimaryOnly);
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.pnlDeleteOption);
      this.Controls.Add((Control) this.pnlAddOption);
      this.Name = nameof (CampaignQueryControl);
      this.Size = new Size(671, 490);
      this.Leave += new EventHandler(this.CampaignQueryControl_Leave);
      this.grpParameters.ResumeLayout(false);
      this.pnlPurposeSelect.ResumeLayout(false);
      this.pnlCompareSelect.ResumeLayout(false);
      this.pnlComboSelect.ResumeLayout(false);
      this.pnlNumericSelect.ResumeLayout(false);
      this.nudNumericValue.EndInit();
      this.pnlAddOption.ResumeLayout(false);
      this.nudFrequencyDays.EndInit();
      this.pnlDeleteOption.ResumeLayout(false);
      this.grpQueryResult.ResumeLayout(false);
      this.pnlMain.ResumeLayout(false);
      this.pnlPrimaryOnly.ResumeLayout(false);
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
