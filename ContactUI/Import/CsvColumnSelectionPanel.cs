// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvColumnSelectionPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.ContactUI.CustomFields;
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
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvColumnSelectionPanel : ContactImportWizardItem
  {
    private const string className = "CsvColumnSelectionPanel";
    private static readonly string sw = Tracing.SwContact;
    private int maxRowsToDisplay = 100;
    private CsvImportParameters csvParams;
    private Panel panel2;
    private Label label2;
    private ColumnAssignmentListView lvwData;
    private IContainer components;
    private Label lblListHeader;
    private CheckBox chkShowAll;
    private Label lblShowAll;
    private Label label1;
    private Hashtable cateNameToIdTbl;
    private bool bReplaceAll;
    private bool bCreateNewAll;
    private List<KeyValuePair<string, string>> parentOrgs;
    private List<KeyValuePair<string, int>> parentOrgIDs;
    private List<KeyValuePair<string, string>> parentNewOrgs;
    private List<KeyValuePair<string, int>> parentNewOrgIDs;
    private List<long> allTPOIDs;
    private List<string> orgNames;
    private List<string> orgNewNames;
    private List<string> orgPathes;
    private List<string> orgNewPathes;
    private Hashtable allManagers;
    private ExternalOriginatorManagementData contactParentOrg;
    private List<UserInfo[]> allInternalUsers;
    private UserInfo[] allSalesReps;
    private UserInfo[] allNonSalesReps;
    private List<ExternalSettingValue> statusList;
    private Dictionary<string, List<ExternalSettingValue>> allSettings;
    private List<string> uniqueExistingLoginEmails;
    private List<string> uniqueNewLoginEmails;

    public CsvColumnSelectionPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.cateNameToIdTbl = new BizCategoryUtil(Session.SessionObjects).GetCategoryNameToIdTable();
      this.csvParams = (CsvImportParameters) this.ImportParameters.ImportOptions;
      this.InitializeComponent();
      this.displayParsedData();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.panel2 = new Panel();
      this.lblShowAll = new Label();
      this.chkShowAll = new CheckBox();
      this.lblListHeader = new Label();
      this.label2 = new Label();
      this.lvwData = new ColumnAssignmentListView();
      this.label1 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Controls.Add((Control) this.lblShowAll);
      this.panel2.Controls.Add((Control) this.chkShowAll);
      this.panel2.Controls.Add((Control) this.lblListHeader);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.lvwData);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 10;
      this.lblShowAll.Location = new Point(344, 63);
      this.lblShowAll.Name = "lblShowAll";
      this.lblShowAll.Size = new Size(94, 16);
      this.lblShowAll.TabIndex = 11;
      this.lblShowAll.Text = "Show All:";
      this.lblShowAll.TextAlign = ContentAlignment.MiddleRight;
      this.lblShowAll.Visible = false;
      this.chkShowAll.Location = new Point(440, 63);
      this.chkShowAll.Name = "chkShowAll";
      this.chkShowAll.Size = new Size(14, 13);
      this.chkShowAll.TabIndex = 10;
      this.chkShowAll.Visible = false;
      this.chkShowAll.Click += new EventHandler(this.chkShowAll_Click);
      this.lblListHeader.Location = new Point(38, 61);
      this.lblListHeader.Name = "lblListHeader";
      this.lblListHeader.Size = new Size(280, 16);
      this.lblListHeader.TabIndex = 9;
      this.lblListHeader.Text = "Data to Import:";
      this.lblListHeader.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(38, 13);
      this.label2.Name = "label2";
      this.label2.Size = new Size(422, 43);
      this.label2.TabIndex = 3;
      this.label2.Text = "Click each column header to map the column to an Encompass field. When you map a column, the data in the grid will be reformatted to reflect the values that will be imported. To view the original values, select (Unassigned) in the column header.";
      this.lvwData.AutoArrange = false;
      this.lvwData.GridLines = true;
      this.lvwData.Location = new Point(38, 79);
      this.lvwData.MultiSelect = false;
      this.lvwData.Name = "lvwData";
      this.lvwData.Size = new Size(416, 144);
      this.lvwData.TabIndex = 2;
      this.lvwData.View = View.Details;
      this.lvwData.ColumnTextChanged += new ColumnClickEventHandler(this.onColumnTextChanged);
      this.label1.Location = new Point(38, 225);
      this.label1.Name = "label1";
      this.label1.Size = new Size(414, 20);
      this.label1.TabIndex = 12;
      this.label1.Text = "When all columns have been assigned, click Import.";
      this.Controls.Add((Control) this.panel2);
      this.Header = "Data Mapping";
      this.Name = nameof (CsvColumnSelectionPanel);
      this.Subheader = "Map the imported data to fields in Encompass";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void displayParsedData()
    {
      Cursor.Current = Cursors.WaitCursor;
      this.lvwData.BeginUpdate();
      try
      {
        this.lvwData.Clear();
        string[] headerRow = this.csvParams.GetHeaderRow();
        CsvImportColumn[] availableColumns = this.csvParams.GetAllAvailableColumns();
        for (int index = 0; index < this.csvParams.ColumnCount; ++index)
          this.lvwData.Columns.Add(new ColumnHeader()
          {
            Text = this.ImportParameters.ContactType != EllieMae.EMLite.ContactUI.ContactType.TPO && this.ImportParameters.ContactType != EllieMae.EMLite.ContactUI.ContactType.TPOCompany || headerRow == null || index >= headerRow.Length ? "(Unassigned)" : this.populateDefaultHeaderForTPOImport(headerRow[index], availableColumns, this.ImportParameters.ContactType),
            Width = this.lvwData.ClientSize.Width / 2
          });
        for (int index = 0; index < this.maxRowsToDisplay && index < this.csvParams.RowCount; ++index)
          this.lvwData.Items.Add(new ListViewItem(this.csvParams.GetRow(index)));
        float num = 0.0f;
        using (Graphics graphics = this.lvwData.CreateGraphics())
        {
          this.lvwData.ColumnHeaderOptions.Add((object) CsvImportColumn.Unassigned);
          num = graphics.MeasureString(CsvImportColumn.Unassigned.ToString(), this.lvwData.Font).Width;
          for (int index = 0; index < availableColumns.Length; ++index)
          {
            this.lvwData.ColumnHeaderOptions.Add((object) availableColumns[index]);
            float width = graphics.MeasureString(availableColumns[index].ToString(), this.lvwData.Font).Width;
            if ((double) num < (double) width)
              num = width;
          }
        }
        this.lvwData.DropdownListWidth = Convert.ToInt32(num);
        if (this.csvParams.Columns != null)
        {
          for (int index = 0; index < this.csvParams.Columns.Length && index < this.csvParams.ColumnCount; ++index)
          {
            this.lvwData.Columns[index].Text = this.csvParams.Columns[index].Description;
            this.updateValuesInColumn(index);
          }
        }
        this.lblListHeader.Text = "Data to Import:";
        if (this.maxRowsToDisplay >= this.csvParams.RowCount)
          return;
        Label lblListHeader = this.lblListHeader;
        lblListHeader.Text = lblListHeader.Text + " (first " + (object) this.maxRowsToDisplay + " records only)";
      }
      finally
      {
        this.lvwData.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private void onColumnTextChanged(object sender, ColumnClickEventArgs e)
    {
      this.csvParams.AutoAssignColumns = false;
      this.updateValuesInColumn(e.Column);
    }

    private void updateValuesInColumn(int columnIndex)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.lvwData.BeginUpdate();
      try
      {
        CsvImportColumn columnByDescription = this.getColumnByDescription(this.lvwData.Columns[columnIndex].Text);
        for (int index = 0; index < this.lvwData.Items.Count; ++index)
        {
          if (this.lvwData.Items[index].SubItems.Count > columnIndex)
          {
            string[] row = this.csvParams.GetRow(index);
            string str1 = columnIndex < row.Length ? row[columnIndex] : "";
            string str2 = columnByDescription == null ? str1 : columnByDescription.FormatValue(str1);
            this.lvwData.Items[index].SubItems[columnIndex].Text = str2;
          }
        }
      }
      finally
      {
        this.lvwData.EndUpdate();
        Cursor.Current = Cursors.Default;
      }
    }

    private CsvImportColumn getColumnByDescription(string name)
    {
      if (name == CsvImportColumn.Unassigned.Description)
        return CsvImportColumn.Unassigned;
      CsvImportColumn[] availableColumns = this.csvParams.GetAllAvailableColumns();
      for (int index = 0; index < availableColumns.Length; ++index)
      {
        if (availableColumns[index].Description == name)
          return availableColumns[index];
      }
      return (CsvImportColumn) null;
    }

    public override WizardItem Back()
    {
      this.csvParams.Columns = this.getColumnMapping();
      return base.Back();
    }

    public override string NextLabel => "&Import >";

    public override WizardItem Next()
    {
      string[] duplicateColumns = this.getDuplicateColumns();
      DialogResult dialogResult = DialogResult.OK;
      if (duplicateColumns.Length != 0)
      {
        if (duplicateColumns.Length == 1)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You have assigned the \"" + duplicateColumns[0] + "\" field to more than one column. Each Encompass field can be assigned to only one data column.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You have assigned the following fields to multiple columns:" + Environment.NewLine + Environment.NewLine + string.Join(Environment.NewLine, duplicateColumns) + Environment.NewLine + Environment.NewLine + "Each Encompass field can be assigned to only one data column.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return (WizardItem) null;
      }
      if (!this.hasAssignedColumn())
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "You must assign at least one column to an Encompass data field by clicking the column's header and selecting the appropriate field name.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return (WizardItem) null;
      }
      if (this.hasUnassignedColumns() && Utils.Dialog((IWin32Window) this.ParentForm, "One or more columns have not been assigned to an Encompass data field. If you proceed, data within those columns will not be imported.", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.Cancel)
        return (WizardItem) null;
      if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPO || this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
      {
        List<object[]> invalidItems = this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPO ? this.hasInvalidTPOContactSourceRecord() : this.hasInvalidTPOOrganizationSourceRecord();
        if (invalidItems != null && invalidItems.Count > 0)
        {
          using (TPOInvalidRecordListForm invalidRecordListForm = new TPOInvalidRecordListForm(this.lvwData.Columns, invalidItems, this.lvwData.Items.Count == invalidItems.Count, this.ImportParameters.ContactType))
          {
            if (invalidRecordListForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return (WizardItem) null;
          }
          List<int> intList = new List<int>();
          for (int index = 0; index < invalidItems.Count; ++index)
          {
            ListViewItem listViewItem = (ListViewItem) invalidItems[index][0];
            intList.Add(listViewItem.Index);
          }
          invalidItems.Clear();
          this.csvParams.InvalidRows = intList;
        }
      }
      dialogResult = new ProgressDialog("Importing " + (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany ? " companies or branches" : "Contacts"), new AsynchronousProcess(this.importContactData), (object) null, true).ShowDialog((IWin32Window) this.ParentForm);
      return WizardItem.Finished;
    }

    private string[] getDuplicateColumns()
    {
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < this.lvwData.Columns.Count; ++index)
      {
        CsvImportColumn columnByDescription = this.getColumnByDescription(this.lvwData.Columns[index].Text);
        if (columnByDescription != CsvImportColumn.Unassigned)
        {
          if (arrayList2.Contains((object) columnByDescription))
          {
            if (!arrayList1.Contains((object) columnByDescription))
              arrayList1.Add((object) columnByDescription);
          }
          else
            arrayList2.Add((object) columnByDescription);
        }
      }
      string[] duplicateColumns = new string[arrayList1.Count];
      for (int index = 0; index < arrayList1.Count; ++index)
        duplicateColumns[index] = ((CsvImportColumn) arrayList1[index]).Description;
      return duplicateColumns;
    }

    private bool hasAssignedColumn()
    {
      for (int index = 0; index < this.lvwData.Columns.Count; ++index)
      {
        if (this.getColumnByDescription(this.lvwData.Columns[index].Text) != CsvImportColumn.Unassigned)
          return true;
      }
      return false;
    }

    private bool hasUnassignedColumns()
    {
      for (int index = 0; index < this.lvwData.Columns.Count; ++index)
      {
        if (this.getColumnByDescription(this.lvwData.Columns[index].Text) == CsvImportColumn.Unassigned)
          return true;
      }
      return false;
    }

    private CsvImportColumn[] getColumnMapping()
    {
      CsvImportColumn[] columnMapping = new CsvImportColumn[this.lvwData.Columns.Count];
      for (int index = 0; index < this.lvwData.Columns.Count; ++index)
        columnMapping[index] = this.getColumnByDescription(this.lvwData.Columns[index].Text);
      return columnMapping;
    }

    private DialogResult importContactData(object state, IProgressFeedback feedback)
    {
      int num1 = 0;
      int num2 = 0;
      try
      {
        feedback.Status = "Preparing to import...";
        CsvImportColumn[] columnMapping = this.getColumnMapping();
        feedback.ResetCounter(this.csvParams.RowCount);
        feedback.Status = "Importing " + (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany ? " companies or branches..." : "Contacts...");
        for (int index1 = 0; index1 < this.csvParams.RowCount; ++index1)
        {
          feedback.Increment(1);
          if (this.csvParams.InvalidRows == null || this.csvParams.InvalidRows.Count <= 0 || !this.csvParams.InvalidRows.Contains(index1))
          {
            string[] row = this.csvParams.GetRow(index1);
            Hashtable newContactInfo = this.createNewContactInfo();
            ArrayList arrayList1 = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            ArrayList arrayList3 = new ArrayList();
            string tpoParentName = string.Empty;
            bool tpoIsBranch = false;
            for (int index2 = 0; index2 < columnMapping.Length && index2 < row.Length; ++index2)
            {
              if (columnMapping[index2] is CvsCategoryFieldImportColumn)
              {
                CvsCategoryFieldImportColumn fieldImportColumn = (CvsCategoryFieldImportColumn) columnMapping[index2];
                CustomFieldValue customFieldValue = CustomFieldValue.NewCustomFieldValue(fieldImportColumn.FieldId, -1, fieldImportColumn.FieldFormat);
                customFieldValue.FieldValue = fieldImportColumn.ToPropertyValue(row[index2]);
                if (CustomFieldsType.BizCategoryStandard == fieldImportColumn.CustomFieldsType)
                  arrayList2.Add((object) customFieldValue);
                else
                  arrayList3.Add((object) customFieldValue);
              }
              else if (columnMapping[index2] is CvsCustomFieldImportColumn)
              {
                CvsCustomFieldImportColumn fieldImportColumn = (CvsCustomFieldImportColumn) columnMapping[index2];
                arrayList1.Add((object) new ContactCustomField(-1, fieldImportColumn.FieldID, "", fieldImportColumn.ToPropertyValue(row[index2])));
              }
              else if (columnMapping[index2] != CsvImportColumn.Unassigned)
              {
                IPropertyDictionary dict = (IPropertyDictionary) newContactInfo[(object) columnMapping[index2].PropertySource];
                if (columnMapping[index2].PropertySource == "PersonalInfoLicense")
                {
                  try
                  {
                    dict = (IPropertyDictionary) (newContactInfo[(object) "Contact"] as BizPartnerInfo).PersonalInfoLicense;
                  }
                  catch
                  {
                    continue;
                  }
                }
                else if (columnMapping[index2].PropertySource == "BizContactLicense")
                {
                  try
                  {
                    dict = (IPropertyDictionary) (newContactInfo[(object) "Contact"] as BizPartnerInfo).BizContactLicense;
                  }
                  catch
                  {
                    continue;
                  }
                }
                if (this.ImportParameters.ContactType != EllieMae.EMLite.ContactUI.ContactType.TPO)
                {
                  if (this.ImportParameters.ContactType != EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
                  {
                    try
                    {
                      dict[columnMapping[index2].PropertyName] = (object) columnMapping[index2].ToPropertyValue(row[index2]);
                      continue;
                    }
                    catch
                    {
                      continue;
                    }
                  }
                }
                try
                {
                  if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPO)
                  {
                    this.populateTPOContactFieldValue(dict, columnMapping[index2].PropertyName, columnMapping[index2], columnMapping[index2].ToPropertyValue(row[index2]), this.ImportParameters.ContactType);
                  }
                  else
                  {
                    this.populateTPOOrganizationFieldValue(dict, columnMapping[index2].PropertyName, columnMapping[index2], columnMapping[index2].ToPropertyValue(row[index2]), this.ImportParameters.ContactType);
                    if (string.Compare(columnMapping[index2].PropertyName, "OrganizationType", true) == 0)
                    {
                      if (string.Compare(columnMapping[index2].ToPropertyValue(row[index2]), "branch", true) == 0)
                        tpoIsBranch = true;
                    }
                    else if (string.Compare(columnMapping[index2].PropertyName, "parentorganizationname", true) == 0)
                      tpoParentName = columnMapping[index2].ToPropertyValue(row[index2]);
                  }
                }
                catch
                {
                }
              }
            }
            if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
            {
              if (!this.populateTPOAdditionalInfo(newContactInfo, tpoParentName, tpoIsBranch))
                continue;
            }
            else if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPO && !this.populateTPOContactAdditionalInfo(newContactInfo))
              continue;
            ContactImportDupOption contactImportDupOption = this.saveContactInfo(newContactInfo, (ContactCustomField[]) arrayList1.ToArray(typeof (ContactCustomField)), (CustomFieldValue[]) arrayList2.ToArray(typeof (CustomFieldValue)), (CustomFieldValue[]) arrayList3.ToArray(typeof (CustomFieldValue)));
            if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
              this.addParentOrganizationInfo(newContactInfo, tpoParentName, tpoIsBranch);
            ++num1;
            if (contactImportDupOption == ContactImportDupOption.Skip || contactImportDupOption == ContactImportDupOption.Abort)
              ++num2;
            else
              this.ImportParameters.WizardForm.OnContactImported(newContactInfo[(object) "Contact"]);
            feedback.Details = "Completed " + (object) num1 + " of " + (object) this.csvParams.RowCount;
            if (feedback.Cancel || contactImportDupOption == ContactImportDupOption.Abort)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Operation cancelled after importing " + (object) (num1 - num2) + (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany ? (object) " companies or branches." : (object) " contacts."), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              return DialogResult.Cancel;
            }
          }
        }
        int num4 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Successfully imported " + (object) (num1 - num2) + (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany ? (object) " companies or branches." : (object) " contacts."), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num5 = (int) Utils.Dialog((IWin32Window) this.ParentForm, "An error has occurred while importing " + (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany ? "companies or branches: " : "contacts: ") + ex.Message + ".", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return DialogResult.Abort;
      }
    }

    private Hashtable createNewContactInfo()
    {
      Hashtable newContactInfo = new Hashtable();
      if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPO)
        newContactInfo.Add((object) "Contact", (object) new ExternalUserInfo()
        {
          ExternalOrgID = this.ImportParameters.TPOExternalOrgID
        });
      else if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
      {
        ExternalOriginatorManagementData originatorManagementData = new ExternalOriginatorManagementData();
        newContactInfo.Add((object) "Contact", (object) originatorManagementData);
      }
      else if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.BizPartner)
      {
        BizPartnerInfo bizPartnerInfo = new BizPartnerInfo(Session.UserID);
        bizPartnerInfo.CategoryID = (int) this.cateNameToIdTbl[(object) "No Category"];
        bizPartnerInfo.AccessLevel = this.ImportParameters.AccessLevel;
        if (this.ImportParameters.AccessLevel == ContactAccess.Public)
        {
          bizPartnerInfo.OwnerID = "";
          newContactInfo.Add((object) "GroupList", (object) this.ImportParameters.GroupList);
        }
        newContactInfo.Add((object) "Contact", (object) bizPartnerInfo);
      }
      else
      {
        newContactInfo.Add((object) "Contact", (object) new BorrowerInfo(Session.UserID)
        {
          AccessLevel = this.ImportParameters.AccessLevel
        });
        newContactInfo.Add((object) "Opportunity", (object) new Opportunity());
      }
      return newContactInfo;
    }

    private ContactImportDupOption saveContactInfo(
      Hashtable dataObjects,
      ContactCustomField[] customFields,
      CustomFieldValue[] standardCategoryFields,
      CustomFieldValue[] customCategoryFields)
    {
      ContactImportDupOption contactImportDupOption;
      if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPO)
      {
        contactImportDupOption = ContactImportUtil.SaveTPOExternalUser((IWin32Window) this.Parent, (ExternalUserInfo) dataObjects[(object) "Contact"], this.ImportParameters.AllContactIds, this.bReplaceAll, this.bCreateNewAll, ContactSource.CSV);
        switch (contactImportDupOption)
        {
          case ContactImportDupOption.ReplaceAll:
            this.bReplaceAll = true;
            break;
          case ContactImportDupOption.CreateNewAll:
            this.bCreateNewAll = true;
            break;
        }
      }
      else if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.TPOCompany)
      {
        contactImportDupOption = ContactImportUtil.SaveTPOOrganization((IWin32Window) this.Parent, (ExternalOriginatorManagementData) dataObjects[(object) "Contact"], this.bReplaceAll, this.bCreateNewAll, ContactSource.CSV);
        switch (contactImportDupOption)
        {
          case ContactImportDupOption.ReplaceAll:
            this.bReplaceAll = true;
            break;
          case ContactImportDupOption.CreateNewAll:
            this.bCreateNewAll = true;
            break;
        }
      }
      else if (this.ImportParameters.ContactType == EllieMae.EMLite.ContactUI.ContactType.BizPartner)
      {
        BizPartnerInfo dataObject = (BizPartnerInfo) dataObjects[(object) "Contact"];
        ContactGroupInfo[] groupList = (ContactGroupInfo[]) null;
        if (dataObjects.ContainsKey((object) "GroupList"))
          groupList = (ContactGroupInfo[]) dataObjects[(object) "GroupList"];
        contactImportDupOption = ContactImportUtil.SaveBizPartnerContactInfo((IWin32Window) this.Parent, dataObject, customFields, standardCategoryFields, customCategoryFields, groupList, this.bReplaceAll, ContactSource.CSV);
        if (contactImportDupOption == ContactImportDupOption.ReplaceAll)
          this.bReplaceAll = true;
      }
      else
      {
        contactImportDupOption = ContactImportUtil.SaveBorrowerContactInfo((IWin32Window) this.Parent, (BorrowerInfo) dataObjects[(object) "Contact"], (Opportunity) dataObjects[(object) "Opportunity"], customFields, this.bReplaceAll, this.bCreateNewAll, ContactSource.CSV);
        switch (contactImportDupOption)
        {
          case ContactImportDupOption.ReplaceAll:
            this.bReplaceAll = true;
            break;
          case ContactImportDupOption.CreateNewAll:
            this.bCreateNewAll = true;
            break;
        }
      }
      return contactImportDupOption;
    }

    private void chkShowAll_Click(object sender, EventArgs e)
    {
      this.csvParams.Columns = this.getColumnMapping();
      this.maxRowsToDisplay = !this.chkShowAll.Checked ? 100 : int.MaxValue;
      this.displayParsedData();
    }

    private List<object[]> hasInvalidTPOOrganizationSourceRecord()
    {
      if (this.parentOrgs == null && this.orgNames == null && this.orgNewPathes == null && this.parentOrgIDs == null && this.allTPOIDs == null)
      {
        List<object> organizationNames = Session.ConfigurationManager.GetAllExternalOrganizationNames();
        if (organizationNames != null && organizationNames.Count > 0)
        {
          for (int index = 0; index < organizationNames.Count; ++index)
          {
            if (organizationNames[index] is List<KeyValuePair<string, string>>)
              this.parentOrgs = (List<KeyValuePair<string, string>>) organizationNames[index];
            else if (organizationNames[index] is List<KeyValuePair<string, int>>)
              this.parentOrgIDs = (List<KeyValuePair<string, int>>) organizationNames[index];
            else if (organizationNames[index] is List<string>)
            {
              List<string> stringList = (List<string>) organizationNames[index];
              if (stringList != null && stringList.Count > 0 && stringList[0].StartsWith("third party originators"))
                this.orgNewPathes = (List<string>) organizationNames[index];
              else
                this.orgNames = (List<string>) organizationNames[index];
            }
            else if (organizationNames[index] is List<long>)
              this.allTPOIDs = (List<long>) organizationNames[index];
          }
        }
        if (this.parentNewOrgs == null)
          this.parentNewOrgs = new List<KeyValuePair<string, string>>();
        if (this.parentNewOrgIDs == null)
          this.parentNewOrgIDs = new List<KeyValuePair<string, int>>();
        if (this.orgNewPathes == null)
          this.orgNewPathes = new List<string>();
        if (this.orgNewNames == null)
          this.orgNewNames = new List<string>();
        if (this.allTPOIDs == null)
          this.allTPOIDs = new List<long>();
        if (this.allManagers == null)
          this.allManagers = new Hashtable();
      }
      string val = string.Empty;
      string empty1 = string.Empty;
      List<object[]> objArrayList = new List<object[]>();
      string empty2 = string.Empty;
      string parentOrgName = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string str1 = string.Empty;
      string val1 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      string empty7 = string.Empty;
      string empty8 = string.Empty;
      string empty9 = string.Empty;
      for (int index1 = 0; index1 < this.lvwData.Items.Count; ++index1)
      {
        string empty10;
        string str2 = empty10 = string.Empty;
        string valueToCheck1 = empty10;
        string valueToCheck2 = empty10;
        string strB = empty10;
        string strA1 = empty10;
        string strA2 = empty10;
        string str3 = empty10;
        parentOrgName = empty10;
        string str4;
        string str5 = str4 = str3;
        int num;
        bool flag1 = (num = 0) != 0;
        bool flag2 = num != 0;
        bool flag3 = num != 0;
        bool flag4 = num != 0;
        bool flag5 = num != 0;
        bool flag6 = num != 0;
        bool flag7 = num != 0;
        bool flag8 = num != 0;
        string[] strArray = new string[8]
        {
          "Organization Name",
          "Channel Type",
          "Company Legal Name",
          "Address",
          "City",
          "State",
          "Zip",
          "Phone Number"
        };
        for (int index2 = 0; index2 < this.lvwData.Columns.Count; ++index2)
        {
          string str6 = this.lvwData.Items[index1].SubItems.Count > index2 ? this.lvwData.Items[index1].SubItems[index2].Text.Trim() : string.Empty;
          if (this.lvwData.Columns[index2].Text == "Use Parent Info (Co. Details)" && str6.ToLower().IndexOf("y") > -1)
            flag5 = true;
          else if (this.lvwData.Columns[index2].Text == "Use Parent Info (Rate Sheet)" && str6.ToLower().IndexOf("y") > -1)
            flag4 = true;
          else if (this.lvwData.Columns[index2].Text == "Use Parent Info (Pricing)" && str6.ToLower().IndexOf("y") > -1)
            flag3 = true;
          else if (this.lvwData.Columns[index2].Text == "Use Parent Info (Status)" && str6.ToLower().IndexOf("y") > -1)
            flag2 = true;
          else if (this.lvwData.Columns[index2].Text == "Use Parent Info (Business Info)" && str6.ToLower().IndexOf("y") > -1)
            flag1 = true;
          switch (this.lvwData.Columns[index2].Text)
          {
            case "Address":
              strArray[3] = string.Empty;
              break;
            case "Channel Type":
              strArray[1] = string.Empty;
              break;
            case "City":
              strArray[4] = string.Empty;
              break;
            case "Company Legal Name":
              strArray[2] = string.Empty;
              break;
            case "Organization Name":
              strArray[0] = string.Empty;
              break;
            case "Phone Number":
              strArray[7] = string.Empty;
              break;
            case "State":
              strArray[5] = string.Empty;
              break;
            case "Zip":
              strArray[6] = string.Empty;
              break;
          }
        }
        for (int index3 = 0; index3 < strArray.Length; ++index3)
        {
          if (!(strArray[index3] == string.Empty) && !(index3 > 0 & flag5))
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + strArray[index3] + " is required!";
          }
        }
        for (int index4 = 0; index4 < this.lvwData.Columns.Count; ++index4)
        {
          string str7 = this.lvwData.Items[index1].SubItems.Count > index4 ? this.lvwData.Items[index1].SubItems[index4].Text.Trim() : string.Empty;
          switch (this.lvwData.Columns[index4].Text)
          {
            case "Add to Watchlist":
            case "Can Close in Own Name":
            case "Can Found in Own Name":
            case "DU Sponsored":
            case "Disable Login":
            case "Incorporated":
            case "Use Parent Info (Business Info)":
            case "Use Parent Info (Co. Details)":
            case "Use Parent Info (Pricing)":
            case "Use Parent Info (Rate Sheet)":
            case "Use Parent Info (Status)":
            case "Use SSN Format":
              if (str7 != string.Empty && string.Compare(str7, "Yes", true) != 0 && string.Compare(str7, "Y", true) != 0 && string.Compare(str7, "N", true) != 0 && string.Compare(str7, "No", true) != 0)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid! Available options: Yes/No/Y/N.";
                break;
              }
              if (str7 != string.Empty && this.lvwData.Columns[index4].Text == "Use SSN Format" && str7.ToLower().IndexOf("y") > -1)
              {
                flag7 = true;
                break;
              }
              break;
            case "Address":
            case "City":
            case "Company Legal Name":
            case "Organization Name":
            case "State":
            case "Zip":
              if (str7 == string.Empty)
              {
                if (!flag5 || this.lvwData.Columns[index4].Text == "Organization Name")
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " cannot be blank!";
                  break;
                }
                break;
              }
              if (this.lvwData.Columns[index4].Text == "State")
              {
                if (str7.Length != 2 || Utils.GetFullStateName(str7).Length <= 2)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + "State is invalid! Available options: AL,AK,AZ...WY,VI,GU,PR.";
                  break;
                }
                break;
              }
              if (this.lvwData.Columns[index4].Text == "Organization Name")
              {
                str4 = str7;
                break;
              }
              if (this.lvwData.Columns[index4].Text == "Zip")
              {
                bool needsUpdate = true;
                if (Utils.FormatInput(str7, FieldFormat.ZIPCODE, ref needsUpdate) != str7)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + "Zip is invalid! Correct format: xxxxx-xxxx or xxxxx";
                  break;
                }
                break;
              }
              break;
            case "Application Date":
            case "Approved Date":
            case "Current Status Date":
            case "Date of Incorporated":
            case "E&O Expiration Date":
            case "Financials Last Update":
              if (str7 != string.Empty)
              {
                DateTime date = Utils.ParseDate((object) str7);
                if (date == DateTime.MinValue)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid! Correct Format: MM/DD/YYYY.";
                  break;
                }
                if (date < DateTime.Parse("01/01/1900") || date > DateTime.Parse("01/01/2079"))
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " must be between 01/01/1900 and 01/01/2079!";
                  break;
                }
                break;
              }
              break;
            case "Channel Type":
              if (!flag5 && string.Compare(str7, "Broker", true) != 0 && string.Compare(str7, "Correspondent", true) != 0 && string.Compare(str7, "Both", true) != 0)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + (str7 == string.Empty ? " cannot be blank!" : " is invalid!") + " Available options: Broker, Correspondent, Both.";
                break;
              }
              break;
            case "Company Net Worth":
              if (str7 != string.Empty)
              {
                if (Utils.ParseDecimal((object) str7).ToString("") != str7)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid!";
                  break;
                }
                if (str7.Length > 17)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " cannot exceed 17 digits!";
                  break;
                }
                break;
              }
              break;
            case "Company Rating":
              if (str7 != string.Empty && this.getTPOStatus(str7, 2) == -1)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid!";
                break;
              }
              break;
            case "Current Status":
              if (str7 != string.Empty && this.getTPOStatus(str7, 1) == -1)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid!";
                break;
              }
              break;
            case "Default DBA Name":
              if (str7 != string.Empty && str7.Length > 100)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " cannot exceed 100 characters!";
                break;
              }
              break;
            case "E&O Company":
            case "E&O Policy":
            case "Financials Period":
            case "MERS Originating Org ID":
              if (str7 != string.Empty && str7.Length > 50)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " cannot exceed 50 characters!";
                break;
              }
              break;
            case "Email":
            case "Lock Info Email":
            case "Rate Sheet Email":
              if (str7 != string.Empty && !Utils.ValidateEmail(str7))
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid!";
                break;
              }
              break;
            case "Fax Number":
            case "Lock Info Fax Number":
            case "Phone Number":
            case "Rate Sheet Fax Number":
              if (str7 != string.Empty)
              {
                bool needsUpdate = true;
                if (Utils.FormatInput(str7, FieldFormat.PHONE, ref needsUpdate).Length < 12)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " format is invalid! Correct format: xxx-xxx-xxxx xxxx";
                  break;
                }
                break;
              }
              if (!flag5 && this.lvwData.Columns[index4].Text == "Phone Number" && str7 == string.Empty)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " cannot be blank!";
                break;
              }
              break;
            case "ICE PPE Comp. Model":
              if (str7 != string.Empty && string.Compare(str7, "Borrower Only", true) != 0 && string.Compare(str7, "Creditor Only", true) != 0 && string.Compare(str7, "Borrower or Creditor", true) != 0)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid! Available options: Borrower Only/Creditor Only/Borrower or Creditor.";
                break;
              }
              break;
            case "Manager Name":
              val1 = str7;
              break;
            case "Organization ID":
              if (str7 != string.Empty)
              {
                if (str7.Length > 25)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " exceeds maximum length (25 digits)!";
                  break;
                }
                for (int index5 = 0; index5 < str7.Length; ++index5)
                {
                  if (!char.IsDigit(str7[index5]))
                  {
                    flag8 = true;
                    str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " must be numeric format!";
                    break;
                  }
                }
                break;
              }
              break;
            case "Organization Type":
              if (string.Compare(str7, "Company", true) != 0 && string.Compare(str7, "Branch", true) != 0)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid! Available options: Company, Branch.";
                break;
              }
              strA2 = str7;
              break;
            case "Other Entity Description":
              str1 = str7;
              break;
            case "Parent Organization Name":
              parentOrgName = str7;
              break;
            case "Price Group":
              if (str7 != string.Empty)
              {
                if (str7.Length > 64)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " cannot exceed 64 characters!";
                  break;
                }
                if (this.getTPOStatus(str7, 3) == -1)
                {
                  flag8 = true;
                  str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid!";
                  break;
                }
                break;
              }
              break;
            case "Primary Sales Rep. ID":
            case "Primary Sales Rep. Name":
              if (str7 != string.Empty)
              {
                if (this.lvwData.Columns[index4].Text == "Primary Sales Rep. ID")
                  valueToCheck2 = str7;
                else
                  valueToCheck1 = str7;
                val = this.getTPOSalesRepID(str7, string.Compare(this.lvwData.Columns[index4].Text, "Primary Sales Rep. Name", true) == 0);
                if (this.lvwData.Columns[index4].Text == "Primary Sales Rep. ID")
                {
                  strA1 = val;
                  break;
                }
                strB = val;
                break;
              }
              break;
            case "State of Incorporated":
              if (str7 != string.Empty && (str7.Length != 2 || Utils.GetFullStateName(str7).Length <= 2))
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid! Available options: AL,AK,AZ...WY,VI,GU,PR.";
                break;
              }
              break;
            case "Tax ID":
              if (str7 != string.Empty)
              {
                str5 = str7;
                break;
              }
              break;
            case "Type of Entity":
              if (str7 != string.Empty && string.Compare(str7, "Individual", true) != 0 && string.Compare(str7, "Sole Proprietorship", true) != 0 && string.Compare(str7, "Partnership", true) != 0 && string.Compare(str7, "Partnership", true) != 0 && string.Compare(str7, "Corporation", true) != 0 && string.Compare(str7, "Limited Liability Company", true) != 0 && string.Compare(str7, "Other", true) != 0 && string.Compare(str7, "Other (please specify)", true) != 0)
              {
                flag8 = true;
                str2 = str2 + (str2 != string.Empty ? "," : "") + this.lvwData.Columns[index4].Text + " is invalid! Available options: Individual/Sole Proprietorship/Partnership/Corporation/Limited Liability Company/Other.";
                break;
              }
              if (str7 != string.Empty && (string.Compare(str7, "Other", true) == 0 || string.Compare(str7, "Other (please specify)", true) == 0))
              {
                flag6 = true;
                break;
              }
              break;
          }
        }
        if (str4 != string.Empty)
        {
          if (string.Compare(strA2, "branch", true) == 0)
          {
            val = "third party originators\\" + parentOrgName.ToLower() + "\\" + str4.ToLower();
            if (this.orgPathes != null && this.orgPathes.FindIndex((Predicate<string>) (x => x == val)) > -1 || this.orgNewPathes != null && this.orgNewPathes.FindIndex((Predicate<string>) (x => x == val)) > -1)
            {
              flag8 = true;
              str2 = str2 + (str2 != string.Empty ? "," : "") + "The Organization Name has been used!";
            }
          }
          else if (this.orgNames.Contains(str4.ToLower()) || this.orgNewNames.Contains(str4.ToLower()))
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + "The Organization Name has been used!";
          }
        }
        if (!string.IsNullOrEmpty(strA1) && !string.IsNullOrEmpty(strB) && string.Compare(strA1, strB, true) != 0)
        {
          flag8 = true;
          str2 = str2 + (str2 != string.Empty ? "," : "") + "The Primary Sales Rep. ID and Name don't match!";
        }
        else if (string.IsNullOrEmpty(strA1) || string.IsNullOrEmpty(strB))
        {
          if (!string.IsNullOrEmpty(valueToCheck2) && string.IsNullOrEmpty(strA1))
          {
            flag8 = true;
            str2 = string.IsNullOrEmpty(this.getNonTPOSalesRepID(valueToCheck2, false)) ? str2 + (str2 != string.Empty ? "," : "") + "The Primary Sales Rep. ID is not found!" : str2 + (str2 != string.Empty ? "," : "") + "The Primary Sales Rep. ID does not have the Sales Rep's right!";
          }
          if (!string.IsNullOrEmpty(valueToCheck1) && string.IsNullOrEmpty(strB))
          {
            flag8 = true;
            str2 = string.IsNullOrEmpty(this.getNonTPOSalesRepID(valueToCheck1, true)) ? str2 + (str2 != string.Empty ? "," : "") + "The Primary Sales Rep. Name is not found!" : str2 + (str2 != string.Empty ? "," : "") + "The Primary Sales Rep. Name does not have the Sales Rep's right!";
          }
          if (string.IsNullOrEmpty(valueToCheck2) && string.IsNullOrEmpty(valueToCheck1) && string.Compare(strA2, "branch", true) != 0)
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + "The Primary Sales Rep. ID cannot be blank!";
          }
        }
        if (str5 != string.Empty && str5.Replace("-", "").Length < 9)
        {
          flag8 = true;
          str2 = str2 + (str2 != string.Empty ? "," : "") + "The Tax ID is invalid! Correct Format: " + (flag7 ? "XXX-XX-XXXX" : "XX-XXXXXXX") + ".";
        }
        if (string.Compare(strA2, "branch", true) == 0)
        {
          if (parentOrgName == string.Empty)
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + "The parent organization name for branch cannot be blank!";
          }
          else if (this.parentOrgs.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key.Equals(parentOrgName.ToLower()))) == -1 && this.parentNewOrgs.FindIndex((Predicate<KeyValuePair<string, string>>) (x => x.Key.Equals(parentOrgName.ToLower()))) == -1)
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + "The parent organization name is not available!";
          }
          else if (val1 != string.Empty && this.getManagerID(val1, parentOrgName) == null)
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + "The Manager Name is not available!";
          }
        }
        else if (parentOrgName != string.Empty)
        {
          flag8 = true;
          str2 = str2 + (str2 != string.Empty ? "," : "") + "The parent organization name is only available to Branch!";
        }
        else
        {
          if (val1 != string.Empty)
          {
            flag8 = true;
            str2 = str2 + (str2 != string.Empty ? "," : "") + "The Manager Name is invalid!";
          }
          if (flag5 | flag4 | flag3 | flag2 | flag1)
          {
            flag8 = true;
            if (flag5)
              str2 = str2 + (str2 != string.Empty ? "," : "") + "The 'Use Parent Info (Co. Details)' field should be blank for company import!";
            if (flag4)
              str2 = str2 + (str2 != string.Empty ? "," : "") + "The 'Use Parent Info (Rate Sheet)' field should be blank for company import!";
            if (flag3)
              str2 = str2 + (str2 != string.Empty ? "," : "") + "The 'Use Parent Info (Pricing)' field should be blank for company import!";
            if (flag2)
              str2 = str2 + (str2 != string.Empty ? "," : "") + "The 'Use Parent Info (Status)' field should be blank for company import!";
            if (flag1)
              str2 = str2 + (str2 != string.Empty ? "," : "") + "The 'Use Parent Info (Business Info)' field should be blank for company import!";
          }
        }
        if (flag6 && str1 == string.Empty || !flag6 && str1 != string.Empty)
        {
          flag8 = true;
          str2 = !flag6 || !(str1 == string.Empty) ? str2 + (str2 != string.Empty ? "," : "") + "The Other Entity Description for Type of Entity must be blank!" : str2 + (str2 != string.Empty ? "," : "") + "The Other Entity Description for Type of Entity cannot be blank!";
        }
        if (flag8)
        {
          objArrayList.Add(new object[2]
          {
            (object) this.lvwData.Items[index1],
            (object) str2
          });
        }
        else
        {
          if (string.Compare(strA2, "branch", true) != 0)
            this.parentNewOrgs.Add(new KeyValuePair<string, string>(str4.ToLower(), "Third Party Originators\\" + str4));
          else
            this.orgNewPathes.Add("third party originators\\" + parentOrgName.ToLower() + "\\" + str4.ToLower());
          this.orgNewNames.Add(str4.ToLower());
        }
      }
      return objArrayList;
    }

    private void populateTPOOrganizationFieldValue(
      IPropertyDictionary dict,
      string propertyName,
      CsvImportColumn col,
      string defVal,
      EllieMae.EMLite.ContactUI.ContactType importType)
    {
      string empty = string.Empty;
      try
      {
        switch (propertyName.ToLower())
        {
          case "address":
            dict[propertyName] = (object) defVal;
            break;
          case "addtowatchlist":
            dict[propertyName] = (object) (bool) (defVal.ToLower() == "y" ? 1 : (defVal.ToLower() == "yes" ? 1 : 0));
            break;
          case "applicationdate":
          case "approveddate":
          case "currentstatusdate":
          case "dateofincorporation":
          case "eoexpirationdate":
          case "financialslastupdate":
            if (!(defVal != string.Empty))
              break;
            dict[propertyName] = (object) Utils.ParseDate((object) defVal).ToString("MM/dd/yyyy");
            break;
          case "cancloseinownname":
          case "canfundinownname":
          case "dusponsored":
            dict[propertyName] = (object) (defVal.ToLower() == "y" || defVal.ToLower() == "yes" ? 1 : (defVal.ToLower() == "n" || defVal.ToLower() == "no" ? 2 : 0));
            break;
          case "city":
            dict[propertyName] = (object) defVal;
            break;
          case "companydbaname":
            if (!((defVal ?? "") != string.Empty))
              break;
            dict[propertyName] = (object) defVal;
            break;
          case "companylegalname":
            dict[propertyName] = (object) defVal;
            break;
          case "companynetworth":
            if (!(defVal != string.Empty))
              break;
            Decimal num = Utils.ParseDecimal((object) defVal);
            bool needsUpdate1 = true;
            dict[propertyName] = (object) Utils.FormatInput(num.ToString(), FieldFormat.INTEGER, ref needsUpdate1);
            break;
          case "companyrating":
            dict[propertyName] = (object) this.getTPOStatus(defVal, 2);
            break;
          case "currentstatus":
            dict[propertyName] = (object) this.getTPOStatus(defVal, 1);
            break;
          case "disabledlogin":
          case "incorporated":
          case "useparentinfoforapprovalstatus":
          case "useparentinfoforbusinessinfo":
          case "useparentinfoforcompanydetails":
          case "useparentinfoforepps":
          case "useparentinfoforratelock":
          case "usessnformat":
            dict[propertyName] = (object) (bool) (defVal.ToLower() == "y" ? 1 : (defVal.ToLower() == "yes" ? 1 : 0));
            break;
          case "email":
          case "emailforlockinfo":
          case "emailforratesheet":
          case "eocompany":
          case "eopolicynumber":
          case "eppsusername":
          case "financialsperiod":
          case "managername":
          case "mersoriginatingorgid":
          case "nmlsid":
          case "otherentitydescription":
          case "taxid":
          case "website":
            dict[propertyName] = (object) defVal;
            break;
          case "entitytype":
            defVal = string.Compare(defVal, "both", true) != 0 ? (string.Compare(defVal, "broker", true) != 0 ? (string.Compare(defVal, "correspondent", true) != 0 ? col.ToPropertyValue(defVal) : "Correspondent") : "Broker") : "Both";
            dict[propertyName] = (object) defVal;
            break;
          case "eppscompmodel":
            if (string.Compare(defVal, "Borrower Only", true) == 0)
            {
              dict[propertyName] = (object) "0";
              break;
            }
            if (string.Compare(defVal, "Creditor Only", true) == 0)
            {
              dict[propertyName] = (object) "1";
              break;
            }
            if (string.Compare(defVal, "Borrower or Creditor", true) != 0)
              break;
            dict[propertyName] = (object) "2";
            break;
          case "eppspricegroup":
            dict[propertyName] = (object) this.getTPOStatus(defVal, 3);
            break;
          case "faxforlockinfo":
          case "faxforratesheet":
          case "faxnumber":
          case "phonenumber":
            if (!(defVal != string.Empty))
              break;
            bool needsUpdate2 = true;
            dict[propertyName] = (object) Utils.FormatInput(defVal, FieldFormat.PHONE, ref needsUpdate2);
            break;
          case "organizationname":
            dict[propertyName] = (object) defVal;
            dict["HierarchyPath"] = (object) ("Third Party Originators\\" + defVal);
            break;
          case "organizationtype":
            dict[propertyName] = (object) (ExternalOriginatorOrgType) (defVal == "Branch" ? 2 : 0);
            break;
          case "orgid":
            if (!(defVal != string.Empty))
              break;
            if (defVal.Trim().Length > 25)
              defVal = defVal.Substring(0, 25);
            dict[propertyName] = (object) defVal;
            break;
          case "ownername":
            dict[propertyName] = (object) defVal;
            break;
          case "parentorganizationname":
            break;
          case "primarysalesrepname":
            if (string.IsNullOrEmpty(defVal))
              break;
            dict[propertyName] = (object) this.getTPOSalesRepID(defVal, true);
            break;
          case "primarysalesrepuserid":
            dict[propertyName] = (object) defVal;
            break;
          case "state":
            dict[propertyName] = (object) defVal;
            break;
          case "stateincorp":
            if (!(defVal != string.Empty))
              break;
            dict[propertyName] = (object) defVal.ToUpper();
            break;
          case "typeofentity":
            if (string.Compare(defVal, "Individual", true) == 0)
            {
              dict[propertyName] = (object) 1;
              break;
            }
            if (string.Compare(defVal, "Sole Proprietorship", true) == 0)
            {
              dict[propertyName] = (object) 2;
              break;
            }
            if (string.Compare(defVal, "Partnership", true) == 0)
            {
              dict[propertyName] = (object) 3;
              break;
            }
            if (string.Compare(defVal, "Corporation", true) == 0)
            {
              dict[propertyName] = (object) 4;
              break;
            }
            if (string.Compare(defVal, "Limited Liability Company", true) == 0)
            {
              dict[propertyName] = (object) 5;
              break;
            }
            if (string.Compare(defVal, "Other", true) != 0 && string.Compare(defVal, "Other (please specify)", true) != 0)
              break;
            dict[propertyName] = (object) 6;
            break;
          case "zip":
            if (!(defVal != string.Empty))
              break;
            bool needsUpdate3 = true;
            dict[propertyName] = (object) Utils.FormatInput(defVal, FieldFormat.ZIPCODE, ref needsUpdate3);
            break;
          default:
            dict[propertyName] = (object) col.ToPropertyValue(defVal);
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CsvColumnSelectionPanel.sw, nameof (CsvColumnSelectionPanel), TraceLevel.Error, ex.ToString());
      }
    }

    private bool populateTPOAdditionalInfo(
      Hashtable dataObjects,
      string tpoParentName,
      bool tpoIsBranch)
    {
      ExternalOriginatorManagementData dataObject = (ExternalOriginatorManagementData) dataObjects[(object) "Contact"];
      dataObject.ExternalID = Utils.NewTpoID(this.allTPOIDs).ToString();
      dataObject.OldExternalID = string.Empty;
      dataObject.contactType = ExternalOriginatorContactType.FreeEntry;
      if (!tpoIsBranch)
      {
        dataObject.Depth = 1;
        dataObject.UseParentInfo = dataObject.UseParentInfoForApprovalStatus = dataObject.UseParentInfoForBusinessInfo = dataObject.UseParentInfoForCompanyDetails = dataObject.UseParentInfoForEPPS = dataObject.UseParentInfoForRateLock = false;
        return true;
      }
      dataObject.Depth = 2;
      KeyValuePair<string, int> keyValuePair1 = this.parentOrgIDs.Find((Predicate<KeyValuePair<string, int>>) (x => x.Key.Equals(tpoParentName.ToLower())));
      if (keyValuePair1.Key == null)
        keyValuePair1 = this.parentNewOrgIDs.Find((Predicate<KeyValuePair<string, int>>) (x => x.Key.Equals(tpoParentName.ToLower())));
      if (keyValuePair1.Key == null)
        return false;
      KeyValuePair<string, string> keyValuePair2 = this.parentOrgs.Find((Predicate<KeyValuePair<string, string>>) (x => x.Key.Equals(tpoParentName.ToLower())));
      if (keyValuePair2.Key == null)
        keyValuePair2 = this.parentNewOrgs.Find((Predicate<KeyValuePair<string, string>>) (x => x.Key.Equals(tpoParentName.ToLower())));
      if (keyValuePair2.Key == null)
        return false;
      dataObject.Parent = keyValuePair1.Value;
      dataObject.HierarchyPath = keyValuePair2.Value + "\\" + dataObject.OrganizationName;
      ExternalOriginatorManagementData externalOrganization = Session.ConfigurationManager.GetExternalOrganization(false, dataObject.Parent);
      if (string.IsNullOrEmpty(dataObject.PrimarySalesRepUserId))
      {
        externalOrganization = Session.ConfigurationManager.GetExternalOrganization(false, dataObject.Parent);
        dataObject.PrimarySalesRepUserId = externalOrganization.PrimarySalesRepUserId;
      }
      if (dataObject.UseParentInfo || dataObject.UseParentInfoForApprovalStatus || dataObject.UseParentInfoForBusinessInfo || dataObject.UseParentInfoForCompanyDetails || dataObject.UseParentInfoForEPPS || dataObject.UseParentInfoForRateLock)
      {
        if (externalOrganization == null)
          externalOrganization = Session.ConfigurationManager.GetExternalOrganization(false, dataObject.Parent);
        if (externalOrganization != null)
        {
          if (dataObject.UseParentInfoForCompanyDetails)
          {
            dataObject.entityType = externalOrganization.entityType;
            dataObject.OrgID = externalOrganization.OrgID;
            dataObject.OwnerName = externalOrganization.OwnerName;
            dataObject.CompanyLegalName = externalOrganization.CompanyLegalName;
            dataObject.CompanyDBAName = externalOrganization.CompanyDBAName;
            dataObject.Address = externalOrganization.Address;
            dataObject.City = externalOrganization.City;
            dataObject.State = externalOrganization.State;
            dataObject.Zip = externalOrganization.Zip;
            dataObject.PhoneNumber = externalOrganization.PhoneNumber;
            dataObject.FaxNumber = externalOrganization.FaxNumber;
            dataObject.Email = externalOrganization.Email;
            dataObject.Website = externalOrganization.Website;
            dataObject.Manager = externalOrganization.Manager;
          }
          if (dataObject.UseParentInfoForRateLock)
          {
            dataObject.EmailForRateSheet = externalOrganization.EmailForRateSheet;
            dataObject.FaxForRateSheet = externalOrganization.FaxForRateSheet;
            dataObject.EmailForLockInfo = externalOrganization.EmailForLockInfo;
            dataObject.FaxForLockInfo = externalOrganization.FaxForLockInfo;
          }
          if (dataObject.UseParentInfoForEPPS)
          {
            dataObject.EPPSUserName = externalOrganization.EPPSUserName;
            dataObject.EPPSCompModel = externalOrganization.EPPSCompModel;
            dataObject.EPPSPriceGroup = externalOrganization.EPPSPriceGroup;
            dataObject.EPPSRateSheet = externalOrganization.EPPSRateSheet;
          }
          if (dataObject.UseParentInfoForApprovalStatus)
          {
            dataObject.CurrentStatus = externalOrganization.CurrentStatus;
            dataObject.AddToWatchlist = externalOrganization.AddToWatchlist;
            dataObject.CurrentStatusDate = externalOrganization.CurrentStatusDate;
            dataObject.ApprovedDate = externalOrganization.ApprovedDate;
            dataObject.ApplicationDate = externalOrganization.ApplicationDate;
            dataObject.CompanyRating = externalOrganization.CompanyRating;
          }
          if (dataObject.UseParentInfoForBusinessInfo)
          {
            dataObject.Incorporated = externalOrganization.Incorporated;
            dataObject.StateIncorp = externalOrganization.StateIncorp;
            dataObject.TypeOfEntity = externalOrganization.TypeOfEntity;
            dataObject.OtherEntityDescription = externalOrganization.OtherEntityDescription;
            dataObject.TaxID = externalOrganization.TaxID;
            dataObject.UseSSNFormat = externalOrganization.UseSSNFormat;
            dataObject.NmlsId = externalOrganization.NmlsId;
            dataObject.FinancialsPeriod = externalOrganization.FinancialsPeriod;
            dataObject.FinancialsLastUpdate = externalOrganization.FinancialsLastUpdate;
            dataObject.CompanyNetWorth = externalOrganization.CompanyNetWorth;
            dataObject.EOExpirationDate = externalOrganization.EOExpirationDate;
            dataObject.EOCompany = externalOrganization.EOCompany;
            dataObject.EOPolicyNumber = externalOrganization.EOPolicyNumber;
            dataObject.MERSOriginatingORGID = externalOrganization.MERSOriginatingORGID;
            dataObject.DUSponsored = externalOrganization.DUSponsored;
            dataObject.CanCloseInOwnName = externalOrganization.CanCloseInOwnName;
            dataObject.CanFundInOwnName = externalOrganization.CanFundInOwnName;
          }
        }
      }
      if ((dataObject.Manager ?? "") != string.Empty)
        dataObject.Manager = this.getManagerID(dataObject.Manager, tpoParentName);
      return true;
    }

    private void addParentOrganizationInfo(
      Hashtable dataObjects,
      string tpoParentName,
      bool tpoIsBranch)
    {
      ExternalOriginatorManagementData dataObject = (ExternalOriginatorManagementData) dataObjects[(object) "Contact"];
      if (!tpoIsBranch)
      {
        if (this.parentNewOrgIDs == null)
          this.parentNewOrgIDs = new List<KeyValuePair<string, int>>();
        if (this.parentNewOrgs == null)
          this.parentNewOrgs = new List<KeyValuePair<string, string>>();
        this.parentNewOrgIDs.Add(new KeyValuePair<string, int>(dataObject.OrganizationName.ToLower(), dataObject.oid));
        this.parentNewOrgs.Add(new KeyValuePair<string, string>(dataObject.OrganizationName.ToLower(), dataObject.HierarchyPath));
      }
      if (this.orgNewNames.Contains(dataObject.OrganizationName.ToLower()))
        return;
      this.orgNewNames.Add(dataObject.OrganizationName.ToLower());
    }

    private bool populateTPOContactAdditionalInfo(Hashtable dataObjects)
    {
      ExternalUserInfo dataObject = (ExternalUserInfo) dataObjects[(object) "Contact"];
      if ((UserInfo) dataObject == (UserInfo) null)
        return false;
      if (string.IsNullOrEmpty(dataObject.SalesRepID))
      {
        if (this.contactParentOrg == null)
          this.contactParentOrg = Session.ConfigurationManager.GetExternalOrganization(false, dataObject.ExternalOrgID);
        dataObject.SalesRepID = this.contactParentOrg != null ? this.contactParentOrg.PrimarySalesRepUserId : "";
      }
      if (dataObject.UseCompanyAddress)
      {
        if (this.contactParentOrg == null)
          this.contactParentOrg = Session.ConfigurationManager.GetExternalOrganization(false, dataObject.ExternalOrgID);
        if (this.contactParentOrg != null)
        {
          dataObject.Address = this.contactParentOrg.Address;
          dataObject.City = this.contactParentOrg.City;
          dataObject.State = this.contactParentOrg.State;
          dataObject.Zipcode = this.contactParentOrg.Zip;
        }
      }
      if (dataObject.UseParentInfoForRateLock)
      {
        if (this.contactParentOrg == null)
          this.contactParentOrg = Session.ConfigurationManager.GetExternalOrganization(false, dataObject.ExternalOrgID);
        if (this.contactParentOrg != null)
        {
          dataObject.EmailForRateSheet = this.contactParentOrg.EmailForRateSheet;
          dataObject.FaxForRateSheet = this.contactParentOrg.FaxForRateSheet;
          dataObject.EmailForLockInfo = this.contactParentOrg.EmailForLockInfo;
          dataObject.FaxForLockInfo = this.contactParentOrg.FaxForLockInfo;
        }
      }
      return true;
    }

    private List<object[]> hasInvalidTPOContactSourceRecord()
    {
      string empty1 = string.Empty;
      List<object[]> objArrayList = new List<object[]>();
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      string empty6 = string.Empty;
      string empty7 = string.Empty;
      bool flag1 = Session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      this.uniqueNewLoginEmails = new List<string>();
      for (int index1 = 0; index1 < this.lvwData.Items.Count; ++index1)
      {
        string empty8;
        string strB = empty8 = string.Empty;
        string strA = empty8;
        string valueToCheck1 = empty8;
        string valueToCheck2 = empty8;
        string str1 = empty8;
        int num;
        bool flag2 = (num = 0) != 0;
        bool flag3 = num != 0;
        bool flag4 = num != 0;
        bool flag5 = num != 0;
        bool flag6 = num != 0;
        bool flag7 = num != 0;
        bool flag8 = num != 0;
        bool flag9 = num != 0;
        bool flag10 = num != 0;
        for (int index2 = 0; index2 < this.lvwData.Columns.Count; ++index2)
        {
          string str2 = this.lvwData.Items[index1].SubItems.Count > index2 ? this.lvwData.Items[index1].SubItems[index2].Text.Trim() : string.Empty;
          if (string.Compare(this.lvwData.Columns[index2].Text, "Use Company Address", true) == 0)
            flag3 = str2.ToLower() == "y" || str2.ToLower() == "yes";
          else if (string.Compare(this.lvwData.Columns[index2].Text, "Use Parent Info (Rate Sheet and Lock)", true) == 0)
            flag2 = str2.ToLower() == "y" || str2.ToLower() == "yes";
        }
        for (int index3 = 0; index3 < this.lvwData.Columns.Count; ++index3)
        {
          string str3 = this.lvwData.Items[index1].SubItems.Count > index3 ? this.lvwData.Items[index1].SubItems[index3].Text.Trim() : string.Empty;
          switch (this.lvwData.Columns[index3].Text)
          {
            case "Address":
            case "City":
              if (str3 != string.Empty & flag3)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " should be blank if contact is using company address!";
                break;
              }
              break;
            case "Approval Date":
            case "Approval Status Date":
              if (str3 != string.Empty)
              {
                DateTime date = Utils.ParseDate((object) str3);
                if (date == DateTime.MinValue)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Correct Format: MM/DD/YYYY.";
                  break;
                }
                if (date < DateTime.Parse("01/01/1900") || date > DateTime.Parse("01/01/2079"))
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " must be between 01/01/1900 and 01/01/2079!";
                  break;
                }
                break;
              }
              break;
            case "Cell Phone":
            case "Fax Number":
              if (!this.validPhoneNumbers(str3))
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Correct format: xxx-xxx-xxxx";
                break;
              }
              break;
            case "Current Status":
              if (str3 != string.Empty && this.getTPOStatus(str3, 0) == -1)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + "Current Status is invalid!";
                break;
              }
              break;
            case "Disable Login":
            case "NMLS Current":
            case "Use Company Address":
            case "Use Parent Info (Rate Sheet and Lock)":
            case "Watch List":
              if (str3 != string.Empty && string.Compare(str3, "Yes", true) != 0 && string.Compare(str3, "Y", true) != 0 && string.Compare(str3, "N", true) != 0 && string.Compare(str3, "No", true) != 0)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Available options: Yes/No/Y/N.";
                break;
              }
              break;
            case "Email":
            case "Login Email":
              if (str3 == string.Empty || !Utils.ValidateEmail(str3))
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + (str3 == string.Empty ? " cannot be blank!" : " is invalid!");
              }
              else if (this.lvwData.Columns[index3].Text == "Login Email" && this.getUniqueLoginEmail(str3) == null)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + "Login Email is duplicated!";
              }
              if (this.lvwData.Columns[index3].Text == "Login Email")
                flag9 = true;
              if (this.lvwData.Columns[index3].Text == "Email")
              {
                flag8 = true;
                break;
              }
              break;
            case "First Name":
            case "Last Name":
              if (str3 == string.Empty)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " cannot be blank!";
              }
              if (this.lvwData.Columns[index3].Text == "First Name")
                flag5 = true;
              if (this.lvwData.Columns[index3].Text == "Last Name")
              {
                flag4 = true;
                break;
              }
              break;
            case "Lock Info Email":
            case "Rate Sheet Email":
              if (str3 != string.Empty)
              {
                if (flag2)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " should be blank when using parent info!";
                  break;
                }
                if (!Utils.ValidateEmail(str3))
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid!";
                  break;
                }
                break;
              }
              break;
            case "Lock Info Fax":
            case "Rate Sheet Fax":
              if (str3 != string.Empty)
              {
                if (flag2)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " should be blank when using parent info!";
                  break;
                }
                if (!this.validPhoneNumbers(str3))
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Correct format: xxx-xxx-xxxx";
                  break;
                }
                break;
              }
              break;
            case "NMLS Originator ID":
              if (str3 != string.Empty)
              {
                if (str3.Length > 12)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " exceeds maximum length (12 digits)!";
                  break;
                }
                for (int index4 = 0; index4 < str3.Length; ++index4)
                {
                  if (!char.IsDigit(str3[index4]))
                  {
                    flag10 = true;
                    str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " must be numeric format!";
                    break;
                  }
                }
                break;
              }
              break;
            case "Personas":
              if (str3 == string.Empty)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + "Persona name cannot be blank!";
              }
              else
              {
                string[] strArray = str3.Split(',');
                Persona[] allPersonas = Session.PersonaManager.GetAllPersonas(new PersonaType[2]
                {
                  PersonaType.External,
                  PersonaType.BothInternalExternal
                });
                foreach (string str4 in strArray)
                {
                  bool flag11 = false;
                  foreach (Persona persona in allPersonas)
                  {
                    if (string.Compare(str4.Trim(), persona.Name.Trim(), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                      flag11 = true;
                      break;
                    }
                  }
                  if (!flag11)
                  {
                    flag10 = true;
                    str1 = str1 + (str1 != string.Empty ? "," : "") + "Persona name '" + str4 + "' is invalid!";
                  }
                }
              }
              flag6 = true;
              break;
            case "Phone":
              if (!this.validPhoneNumbers(str3))
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Correct format: xxx-xxx-xxxx xxxx";
                break;
              }
              break;
            case "Role":
              if (str3 == string.Empty)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + "Role name cannot be blank!";
              }
              else if (str3.IndexOf(",") > -1)
              {
                string str5 = str3;
                char[] chArray = new char[1]{ ',' };
                foreach (string str6 in str5.Split(chArray))
                {
                  if (this.getTPORoleID(str6.Trim()) == ExternalUserInfo.ContactRoles.None)
                  {
                    flag10 = true;
                    str1 = str1 + (str1 != string.Empty ? "," : "") + "Role name '" + str6 + "' is invalid! Available Options:Loan Officer,Loan Processor,Manager,Administrator.";
                  }
                }
              }
              else if (this.getTPORoleID(str3) == ExternalUserInfo.ContactRoles.None)
              {
                flag10 = true;
                str1 = str1 + (str1 != string.Empty ? "," : "") + "Role name '" + str3 + "' is invalid! Available Options:Loan Officer,Loan Processor,Manager,Administrator.";
              }
              flag7 = true;
              break;
            case "SSN":
              if (str3 != string.Empty)
              {
                if (str3.Replace("-", "").Length != 9)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Correct Format: XXX-XX-XXXX.";
                  break;
                }
                bool needsUpdate = true;
                if (Utils.FormatInput(str3, FieldFormat.SSN, ref needsUpdate) != str3)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " is invalid! Correct Format: XXX-XX-XXXX.";
                  break;
                }
                break;
              }
              break;
            case "Sales Rep Name":
            case "Sales Rep User ID":
              if (str3 != string.Empty)
              {
                if (this.lvwData.Columns[index3].Text == "Sales Rep User ID")
                  valueToCheck2 = str3;
                else
                  valueToCheck1 = str3;
                string tpoSalesRepId = this.getTPOSalesRepID(str3, string.Compare(this.lvwData.Columns[index3].Text, "Sales Rep Name", true) == 0);
                if (tpoSalesRepId != null)
                {
                  if (this.lvwData.Columns[index3].Text == "Sales Rep User ID")
                  {
                    strA = tpoSalesRepId;
                    break;
                  }
                  strB = tpoSalesRepId;
                  break;
                }
                break;
              }
              break;
            case "State":
              if (str3 != string.Empty)
              {
                if (flag3)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " should be blank if contact is using company address!";
                  break;
                }
                if (str3.Length != 2 || Utils.GetFullStateName(str3).Length <= 2)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + "State is invalid! Available options: AL,AK,AZ...WY,VI,GU,PR.";
                  break;
                }
                break;
              }
              break;
            case "Zip":
              if (str3 != string.Empty)
              {
                if (flag3)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + this.lvwData.Columns[index3].Text + " should be blank if contact is using company address!";
                  break;
                }
                bool needsUpdate = true;
                if (Utils.FormatInput(str3, FieldFormat.ZIPCODE, ref needsUpdate) != str3)
                {
                  flag10 = true;
                  str1 = str1 + (str1 != string.Empty ? "," : "") + "Zip is invalid! Correct format: xxxxx-xxxx or xxxxx";
                  break;
                }
                break;
              }
              break;
          }
        }
        if (!flag5)
        {
          flag10 = true;
          str1 = str1 + (str1 != string.Empty ? "," : "") + "First Name is required!";
        }
        if (!flag4)
        {
          flag10 = true;
          str1 = str1 + (str1 != string.Empty ? "," : "") + "Last Name is required!";
        }
        if (!flag9)
        {
          flag10 = true;
          str1 = str1 + (str1 != string.Empty ? "," : "") + "Login Email is required!";
        }
        if (!flag8)
        {
          flag10 = true;
          str1 = str1 + (str1 != string.Empty ? "," : "") + "Email is required!";
        }
        if (!flag1)
        {
          if (!flag7)
          {
            flag10 = true;
            str1 = str1 + (str1 != string.Empty ? "," : "") + "Role is required!";
          }
        }
        else if (!flag6)
        {
          flag10 = true;
          str1 = str1 + (str1 != string.Empty ? "," : "") + "Persona is required!";
        }
        if (strA != string.Empty && strB != string.Empty && string.Compare(strA, strB, true) != 0)
        {
          flag10 = true;
          str1 = str1 + (str1 != string.Empty ? "," : "") + "Sales Rep User ID and Sales Rep Name don't match!";
        }
        else if (strA == string.Empty || strB == string.Empty)
        {
          if (valueToCheck2 != string.Empty && strA == string.Empty)
          {
            flag10 = true;
            str1 = string.IsNullOrEmpty(this.getNonTPOSalesRepID(valueToCheck2, false)) ? str1 + (str1 != string.Empty ? "," : "") + "Sales Rep User ID is not found!" : str1 + (str1 != string.Empty ? "," : "") + "Sales Rep User ID does not have the Sales Rep's right!";
          }
          if (valueToCheck1 != string.Empty && strB == string.Empty)
          {
            flag10 = true;
            str1 = string.IsNullOrEmpty(this.getNonTPOSalesRepID(valueToCheck1, true)) ? str1 + (str1 != string.Empty ? "," : "") + "Sales Rep Name is not found!" : str1 + (str1 != string.Empty ? "," : "") + "Sales Rep Name does not have the Sales Rep's right!";
          }
        }
        if (flag10)
          objArrayList.Add(new object[2]
          {
            (object) this.lvwData.Items[index1],
            (object) str1
          });
      }
      return objArrayList;
    }

    private bool validPhoneNumbers(string numbers)
    {
      if (numbers == string.Empty)
        return true;
      string str1 = numbers;
      numbers = numbers.Replace("-", "").Replace(" ", "");
      if (numbers.Length < 10 || numbers.Length > 14)
        return false;
      for (int index = 0; index < numbers.Length; ++index)
      {
        if (!char.IsDigit(numbers[index]))
          return false;
      }
      bool needsUpdate = true;
      string str2 = Utils.FormatInput(numbers, FieldFormat.PHONE, ref needsUpdate);
      if (str1.IndexOf("-") == -1)
        str2 = str2.Replace("-", "");
      if (str1.IndexOf(" ") == -1)
        str2 = str2.Replace(" ", "");
      return !(str2 != str1);
    }

    private void populateTPOContactFieldValue(
      IPropertyDictionary dict,
      string propertyName,
      CsvImportColumn col,
      string defVal,
      EllieMae.EMLite.ContactUI.ContactType importType)
    {
      string empty = string.Empty;
      switch (propertyName.ToLower())
      {
        case "approval_status":
          int tpoStatus = this.getTPOStatus(defVal, 0);
          if (tpoStatus == -1)
            break;
          dict[propertyName] = (object) tpoStatus;
          break;
        case "approval_status_watchlist":
        case "disabledlogin":
        case "inheritparentratesheet":
        case "nmlscurrent":
        case "usecompanyaddress":
          dict[propertyName] = (object) (bool) (defVal.ToLower() == "y" ? 1 : (defVal.ToLower() == "yes" ? 1 : 0));
          break;
        case "cell_phone":
        case "fax":
        case "lock_info_fax":
        case "phone":
        case "rate_sheet_fax":
          if (string.IsNullOrEmpty(defVal))
            break;
          bool needsUpdate = true;
          dict[propertyName] = (object) Utils.FormatInput(defVal, FieldFormat.PHONE, ref needsUpdate);
          break;
        case "roles":
          if (string.IsNullOrEmpty(defVal))
            break;
          if (defVal.IndexOf(',') > -1)
          {
            int num = 0;
            string str1 = defVal;
            char[] chArray = new char[1]{ ',' };
            foreach (string str2 in str1.Split(chArray))
              num = (int) ((ExternalUserInfo.ContactRoles) num | this.getTPORoleID(str2.Trim()));
            dict[propertyName] = (object) num;
            break;
          }
          if (this.getTPORoleID(defVal) == ExternalUserInfo.ContactRoles.None)
            break;
          dict[propertyName] = (object) (int) this.getTPORoleID(defVal);
          break;
        case "sales_rep_name":
          if (string.IsNullOrEmpty(defVal))
            break;
          string tpoSalesRepId1 = this.getTPOSalesRepID(defVal, true);
          if (tpoSalesRepId1 == null)
            break;
          dict["Sales_rep_userid"] = (object) tpoSalesRepId1;
          break;
        case "sales_rep_userid":
          if (string.IsNullOrEmpty(defVal))
            break;
          string tpoSalesRepId2 = this.getTPOSalesRepID(defVal, false);
          if (tpoSalesRepId2 == null)
            break;
          dict["Sales_rep_userid"] = (object) tpoSalesRepId2;
          break;
        case "state":
          dict[propertyName] = (object) defVal.ToUpper();
          break;
        case "userpersonas":
          if (string.IsNullOrEmpty(defVal))
            break;
          ExternalUserInfo externalUserInfo = (ExternalUserInfo) dict;
          string[] strArray = defVal.Split(',');
          Persona[] personaArray = new Persona[strArray.Length];
          for (int index = 0; index < strArray.Length; ++index)
            personaArray[index] = Session.PersonaManager.GetPersonaByName(strArray[index].Trim());
          externalUserInfo.UserPersonas = personaArray;
          break;
        default:
          dict[propertyName] = (object) col.ToPropertyValue(defVal);
          break;
      }
    }

    private string getTPOSalesRepID(string valueToCheck, bool useNameToCheck)
    {
      this.getAllInternalUsers();
      return this.getInternalUserID(this.allSalesReps, valueToCheck, useNameToCheck);
    }

    private string getNonTPOSalesRepID(string valueToCheck, bool useNameToCheck)
    {
      this.getAllInternalUsers();
      return this.getInternalUserID(this.allNonSalesReps, valueToCheck, useNameToCheck);
    }

    private void getAllInternalUsers()
    {
      if (this.allInternalUsers != null)
        return;
      this.allInternalUsers = Session.SessionObjects.ConfigurationManager.GetAllSalesRepUsers(Session.UserID);
      if (this.allInternalUsers == null || this.allInternalUsers.Count <= 0)
        return;
      if (this.allInternalUsers.Count >= 1)
        this.allSalesReps = this.allInternalUsers[0];
      if (this.allInternalUsers.Count < 2)
        return;
      this.allNonSalesReps = this.allInternalUsers[1];
    }

    private string getInternalUserID(UserInfo[] users, string valueToCheck, bool useNameToCheck)
    {
      if (users != null)
      {
        for (int index = 0; index < users.Length; ++index)
        {
          if (useNameToCheck)
          {
            if (string.Compare(users[index].FullName, valueToCheck, true) == 0)
              return users[index].Userid;
          }
          else if (string.Compare(users[index].Userid, valueToCheck, true) == 0)
            return users[index].Userid;
        }
      }
      return (string) null;
    }

    private ExternalUserInfo.ContactRoles getTPORoleID(string roleName)
    {
      if (string.Compare(roleName, "Loan Officer", true) == 0 || string.Compare(roleName, "LO", true) == 0)
        return ExternalUserInfo.ContactRoles.LoanOfficer;
      if (string.Compare(roleName, "Loan Processor", true) == 0 || string.Compare(roleName, "LP", true) == 0)
        return ExternalUserInfo.ContactRoles.LoanProcessor;
      if (string.Compare(roleName, "Manager", true) == 0)
        return ExternalUserInfo.ContactRoles.Manager;
      return string.Compare(roleName, "Administrator", true) == 0 || string.Compare(roleName, "Admin", true) == 0 ? ExternalUserInfo.ContactRoles.Administrator : ExternalUserInfo.ContactRoles.None;
    }

    private int getTPOStatus(string val, int checkType)
    {
      if (checkType > 0)
      {
        if (this.allSettings == null)
          this.allSettings = Session.SessionObjects.ConfigurationManager.GetExternalOrgSettings();
        if (this.allSettings != null)
        {
          if (checkType == 1 && this.allSettings.ContainsKey("Current Company Status"))
            this.statusList = this.allSettings["Current Company Status"];
          else if (checkType == 2 && this.allSettings.ContainsKey("Company Rating"))
            this.statusList = this.allSettings["Company Rating"];
          else if (checkType == 3 && this.allSettings.ContainsKey("Price Group"))
            this.statusList = this.allSettings["Price Group"];
          else if (checkType == 4 && this.allSettings.ContainsKey("ICE PPE Rate Sheet"))
            this.statusList = this.allSettings["ICE PPE Rate Sheet"];
        }
      }
      else if (this.statusList == null)
      {
        Dictionary<string, List<ExternalSettingValue>> externalOrgSettings = Session.SessionObjects.ConfigurationManager.GetExternalOrgSettings();
        if (externalOrgSettings.ContainsKey("Current Contact Status"))
          this.statusList = externalOrgSettings["Current Contact Status"];
      }
      if (this.statusList == null)
        return -1;
      if (checkType == 3)
      {
        for (int index = 0; index < this.statusList.Count; ++index)
        {
          if (string.Compare(val, this.statusList[index].settingCode + "-" + this.statusList[index].settingValue, true) == 0)
            return this.statusList[index].settingId;
        }
        for (int index = 0; index < this.statusList.Count; ++index)
        {
          if (string.Compare(val, this.statusList[index].settingCode, true) == 0)
            return this.statusList[index].settingId;
        }
        return -1;
      }
      for (int index = 0; index < this.statusList.Count; ++index)
      {
        if (string.Compare(val, this.statusList[index].settingValue, true) == 0)
          return this.statusList[index].settingId;
      }
      return -1;
    }

    private string getUniqueLoginEmail(string val)
    {
      if (this.uniqueExistingLoginEmails == null)
        this.uniqueExistingLoginEmails = Session.SessionObjects.ConfigurationManager.GetAllLoginEmailID("");
      if (this.uniqueExistingLoginEmails != null && this.uniqueExistingLoginEmails.Contains(val) || this.uniqueNewLoginEmails.Contains(val))
        return (string) null;
      this.uniqueNewLoginEmails.Add(val);
      return val;
    }

    private string getManagerID(string val, string parentOrgName)
    {
      string empty = string.Empty;
      KeyValuePair<string, int> keyValuePair = this.parentOrgIDs.Find((Predicate<KeyValuePair<string, int>>) (x => x.Key.Equals(parentOrgName.ToLower())));
      if (keyValuePair.Key == null)
        return (string) null;
      if (!this.allManagers.ContainsKey((object) keyValuePair.Value))
        this.allManagers.Add((object) keyValuePair.Value, (object) Session.ConfigurationManager.GetAllCompanyManagers(keyValuePair.Value));
      List<ExternalUserInfo> allManager = (List<ExternalUserInfo>) this.allManagers[(object) keyValuePair.Value];
      if (allManager == null)
        return (string) null;
      val = val.Replace(" ", "");
      foreach (ExternalUserInfo externalUserInfo in allManager)
      {
        if (string.Compare(externalUserInfo.FirstName + externalUserInfo.MiddleName + externalUserInfo.LastName + externalUserInfo.Suffix, val, true) == 0)
          return externalUserInfo.ExternalUserID;
      }
      return (string) null;
    }

    private string populateDefaultHeaderForTPOImport(
      string headerName,
      CsvImportColumn[] tpoDefaultHeaderNames,
      EllieMae.EMLite.ContactUI.ContactType importType)
    {
      string str = "(Unassigned)";
      for (int index = 0; index < tpoDefaultHeaderNames.Length; ++index)
      {
        if (string.Compare(headerName, tpoDefaultHeaderNames[index].Description, true) == 0)
          return tpoDefaultHeaderNames[index].Description;
      }
      return str;
    }
  }
}
