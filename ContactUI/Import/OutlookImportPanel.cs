// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.OutlookImportPanel
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Wizard;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using Outlook;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class OutlookImportPanel : ContactImportWizardItem
  {
    private const string className = "OutlookImportPanel";
    private static string sw = Tracing.SwContact;
    private bool AllowEmailAccess = true;
    private Panel panel2;
    private Label label3;
    private Label label2;
    private Label label1;
    private IContainer components;
    private bool bReplaceAll;
    private bool bCreateNewAll;

    public OutlookImportPanel(ContactImportWizardItem prevItem)
      : base(prevItem)
    {
      this.InitializeComponent();
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
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panel2.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.White;
      this.panel2.Controls.Add((Control) this.label3);
      this.panel2.Controls.Add((Control) this.label2);
      this.panel2.Controls.Add((Control) this.label1);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 60);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(496, 254);
      this.panel2.TabIndex = 13;
      this.label3.Location = new Point(38, 78);
      this.label3.Name = "label3";
      this.label3.Size = new Size(418, 28);
      this.label3.TabIndex = 15;
      this.label3.Text = "To begin the import process, click the Import button below.";
      this.label2.Location = new Point(38, 24);
      this.label2.Name = "label2";
      this.label2.Size = new Size(418, 48);
      this.label2.TabIndex = 14;
      this.label2.Text = "To import contacts, you will first be prompted to select the Outlook Contact folder from which the contact data will be drawn. Once selected, all contacts within that folder will be imported into Encompass.";
      this.label1.Location = new Point(38, 208);
      this.label1.Name = "label1";
      this.label1.Size = new Size(418, 22);
      this.label1.TabIndex = 13;
      this.label1.Text = "Warning: Importing contacts will cause all Outlook windows to be closed.";
      this.Controls.Add((Control) this.panel2);
      this.Header = "Import from Microsoft Outlook";
      this.Name = nameof (OutlookImportPanel);
      this.Subheader = "";
      this.Controls.SetChildIndex((Control) this.panel2, 0);
      this.panel2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public override string NextLabel => "&Import >";

    public override WizardItem Next()
    {
      return this.importOutlookContacts() ? WizardItem.Finished : (WizardItem) null;
    }

    private bool importOutlookContacts()
    {
      this.AllowEmailAccess = true;
      Cursor.Current = Cursors.WaitCursor;
      ImportContact importContact;
      try
      {
        importContact = new ImportContact();
      }
      catch
      {
        int num = (int) Utils.Dialog((IWin32Window) this.ParentForm, "Please check if Microsoft Outlook is installed properly. The action of importing contacts will be aborted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      try
      {
        if (!importContact.Initialized || !importContact.InitCDO())
          return false;
        Cursor.Current = Cursors.Default;
        MAPIFolder mapiFolder = importContact.SelectFolder();
        if (mapiFolder == null)
          return false;
        if (new ProgressDialog("Contact Import", new AsynchronousProcess(this.importContacts), (object) new object[2]
        {
          (object) importContact,
          (object) mapiFolder
        }, true).RunWait((IWin32Window) this) == DialogResult.OK)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Contacts were imported successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        return true;
      }
      catch (System.Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while importing contacts. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(OutlookImportPanel.sw, TraceLevel.Error, nameof (OutlookImportPanel), "Error in importing contacts: " + (object) ex);
        return false;
      }
      finally
      {
        importContact.Clear();
      }
    }

    private DialogResult importContacts(object state, IProgressFeedback feedback)
    {
      try
      {
        feedback.Status = "Reading Contacts from Outlook...";
        ImportContact olImport = (ImportContact) ((object[]) state)[0];
        MAPIFolder contactFolder = (MAPIFolder) ((object[]) state)[1];
        ContactItem[] contactItemArray = olImport.SelectContacts(contactFolder);
        feedback.Status = "Importing " + (object) contactItemArray.Length + " Contacts...";
        feedback.ResetCounter(contactItemArray.Length);
        for (int index = 0; index < contactItemArray.Length; ++index)
        {
          ContactItem olContact = contactItemArray[index];
          object obj;
          ContactImportDupOption contactImportDupOption;
          if (this.ImportParameters.ContactType == ContactType.BizPartner)
          {
            obj = (object) this.OutlookContactToBizPartnerInfo(olImport, olContact);
            contactImportDupOption = this.importBizPartner((BizPartnerInfo) obj, feedback);
          }
          else
          {
            obj = (object) this.OutlookContactToBorrowerInfo(olImport, olContact);
            contactImportDupOption = this.importBorrower((BorrowerInfo) obj, feedback);
          }
          feedback.Increment(1);
          if (contactImportDupOption == ContactImportDupOption.CreateNew || contactImportDupOption == ContactImportDupOption.ReplaceAll || contactImportDupOption == ContactImportDupOption.Replace)
            this.ImportParameters.WizardForm.OnContactImported(obj);
          if (feedback.Cancel || contactImportDupOption == ContactImportDupOption.Abort)
            return DialogResult.Cancel;
        }
        return DialogResult.OK;
      }
      catch (System.Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while importing contacts. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        Tracing.Log(OutlookImportPanel.sw, TraceLevel.Error, nameof (OutlookImportPanel), "Error in importing contacts: " + (object) ex);
        return DialogResult.No;
      }
    }

    private ContactImportDupOption importBizPartner(
      BizPartnerInfo contactInfo,
      IProgressFeedback feedback)
    {
      feedback.Details = "Saving " + contactInfo.FirstName + " " + contactInfo.LastName;
      ContactGroupInfo[] groupList = (ContactGroupInfo[]) null;
      if (this.ImportParameters.GroupList != null)
        groupList = this.ImportParameters.GroupList;
      ContactImportDupOption contactImportDupOption = ContactImportUtil.SaveBizPartnerContactInfo((IWin32Window) this.ParentForm, contactInfo, (ContactCustomField[]) null, (CustomFieldValue[]) null, (CustomFieldValue[]) null, groupList, this.bReplaceAll, ContactSource.Outlook);
      if (contactImportDupOption == ContactImportDupOption.ReplaceAll)
        this.bReplaceAll = true;
      return contactImportDupOption;
    }

    private ContactImportDupOption importBorrower(
      BorrowerInfo contactInfo,
      IProgressFeedback feedback)
    {
      feedback.Details = "Saving " + contactInfo.FirstName + " " + contactInfo.LastName;
      ContactImportDupOption contactImportDupOption = ContactImportUtil.SaveBorrowerContactInfo((IWin32Window) this.ParentForm, contactInfo, (Opportunity) null, (ContactCustomField[]) null, this.bReplaceAll, this.bCreateNewAll, ContactSource.Outlook);
      switch (contactImportDupOption)
      {
        case ContactImportDupOption.ReplaceAll:
          this.bReplaceAll = true;
          break;
        case ContactImportDupOption.CreateNewAll:
          this.bCreateNewAll = true;
          break;
      }
      return contactImportDupOption;
    }

    private BizPartnerInfo OutlookContactToBizPartnerInfo(
      ImportContact olImport,
      ContactItem olContact)
    {
      BizPartnerInfo bizPartnerInfo = new BizPartnerInfo();
      bizPartnerInfo.AccessLevel = this.ImportParameters.AccessLevel;
      bizPartnerInfo.OwnerID = this.ImportParameters.AccessLevel != ContactAccess.Public ? Session.UserID : "";
      Hashtable categoryNameToIdTable = new BizCategoryUtil(Session.SessionObjects).GetCategoryNameToIdTable();
      if (olContact.Categories == null || olContact.Categories == string.Empty)
      {
        bizPartnerInfo.CategoryID = (int) categoryNameToIdTable[(object) "No Category"];
      }
      else
      {
        string[] strArray = olContact.Categories.Split(',');
        int num1;
        int num2 = num1 = (int) categoryNameToIdTable[(object) "No Category"];
        bool flag = false;
        for (int index = 0; index < strArray.Length; ++index)
        {
          string key = strArray[index].Trim();
          if (categoryNameToIdTable[(object) key] != null)
          {
            if (!flag)
            {
              num2 = (int) categoryNameToIdTable[(object) key];
              flag = true;
            }
            else
            {
              num2 = (int) categoryNameToIdTable[(object) "No Category"];
              break;
            }
          }
        }
        bizPartnerInfo.CategoryID = num2;
      }
      bizPartnerInfo.FirstName = this.truncateString(olContact.FirstName, 50);
      bizPartnerInfo.LastName = this.truncateString(olContact.LastName, 50);
      bizPartnerInfo.Salutation = string.Empty;
      bizPartnerInfo.CompanyName = this.truncateString(olContact.CompanyName, 50);
      bizPartnerInfo.JobTitle = this.truncateString(olContact.JobTitle, 50);
      bizPartnerInfo.BizAddress.Street1 = this.truncateString(olContact.BusinessAddressStreet, (int) byte.MaxValue);
      bizPartnerInfo.BizAddress.Street2 = string.Empty;
      bizPartnerInfo.BizAddress.City = this.truncateString(olContact.BusinessAddressCity, 50);
      bizPartnerInfo.BizAddress.State = olContact.BusinessAddressState == null ? string.Empty : Utils.GetStateAbbr(olContact.BusinessAddressState);
      bizPartnerInfo.BizAddress.Zip = this.truncateString(olContact.BusinessAddressPostalCode, 20);
      bizPartnerInfo.BizWebUrl = this.truncateString(olContact.BusinessHomePage, 50);
      bizPartnerInfo.HomePhone = this.truncateString(olContact.HomeTelephoneNumber, 50);
      bizPartnerInfo.WorkPhone = this.truncateString(olContact.BusinessTelephoneNumber, 30);
      bizPartnerInfo.MobilePhone = this.truncateString(olContact.MobileTelephoneNumber, 30);
      bizPartnerInfo.FaxNumber = this.truncateString(olContact.BusinessFaxNumber, 30);
      if (this.AllowEmailAccess)
      {
        try
        {
          bizPartnerInfo.PersonalEmail = olContact.Email1Address != null ? (olContact.Email1Address.IndexOf("@") != -1 ? this.truncateString(olContact.Email1Address, 50) : this.truncateString(olImport.GetSMTPEmailAddress(olContact.Email1Address), 50)) : string.Empty;
          bizPartnerInfo.BizEmail = olContact.Email2Address != null ? (olContact.Email2Address.IndexOf("@") != -1 ? this.truncateString(olContact.Email2Address, 50) : this.truncateString(olImport.GetSMTPEmailAddress(olContact.Email2Address), 50)) : string.Empty;
        }
        catch
        {
          this.AllowEmailAccess = false;
        }
      }
      bizPartnerInfo.LicenseNumber = string.Empty;
      bizPartnerInfo.Fees = 0;
      bizPartnerInfo.Comment = string.Empty;
      bizPartnerInfo.CustField1 = string.Empty;
      bizPartnerInfo.CustField2 = string.Empty;
      bizPartnerInfo.CustField3 = string.Empty;
      bizPartnerInfo.CustField4 = string.Empty;
      bizPartnerInfo.PrimaryEmail = "B";
      bizPartnerInfo.PrimaryPhone = "W";
      bizPartnerInfo.NoSpam = false;
      return bizPartnerInfo;
    }

    private BorrowerInfo OutlookContactToBorrowerInfo(ImportContact olImport, ContactItem olContact)
    {
      BorrowerInfo borrowerInfo = new BorrowerInfo();
      borrowerInfo.FirstName = this.truncateString(olContact.FirstName, 50);
      borrowerInfo.LastName = this.truncateString(olContact.LastName, 50);
      borrowerInfo.Salutation = string.Empty;
      borrowerInfo.HomeAddress.Street1 = this.truncateString(olContact.HomeAddressStreet, (int) byte.MaxValue);
      borrowerInfo.HomeAddress.Street2 = string.Empty;
      borrowerInfo.HomeAddress.City = this.truncateString(olContact.HomeAddressCity, 50);
      borrowerInfo.HomeAddress.State = olContact.HomeAddressState == null ? string.Empty : Utils.GetStateAbbr(olContact.HomeAddressState);
      borrowerInfo.HomeAddress.Zip = this.truncateString(olContact.HomeAddressPostalCode, 20);
      borrowerInfo.BizAddress.Street1 = this.truncateString(olContact.BusinessAddressStreet, 50);
      borrowerInfo.BizAddress.Street2 = string.Empty;
      borrowerInfo.BizAddress.City = this.truncateString(olContact.BusinessAddressCity, 50);
      borrowerInfo.BizAddress.State = olContact.BusinessAddressState == null ? string.Empty : Utils.GetStateAbbr(olContact.BusinessAddressState);
      borrowerInfo.BizAddress.Zip = this.truncateString(olContact.BusinessAddressPostalCode, 20);
      borrowerInfo.BizWebUrl = this.truncateString(olContact.BusinessHomePage, 50);
      borrowerInfo.EmployerName = this.truncateString(olContact.CompanyName, 50);
      borrowerInfo.JobTitle = this.truncateString(olContact.JobTitle, 50);
      borrowerInfo.WorkPhone = this.truncateString(olContact.BusinessTelephoneNumber, 30);
      borrowerInfo.HomePhone = this.truncateString(olContact.HomeTelephoneNumber, 30);
      borrowerInfo.MobilePhone = this.truncateString(olContact.MobileTelephoneNumber, 50);
      borrowerInfo.FaxNumber = this.truncateString(olContact.BusinessFaxNumber, 30);
      if (this.AllowEmailAccess)
      {
        try
        {
          borrowerInfo.PersonalEmail = olContact.Email1Address != null ? (olContact.Email1Address.IndexOf("@") != -1 ? this.truncateString(olContact.Email1Address, 50) : this.truncateString(olImport.GetSMTPEmailAddress(olContact.Email1Address), 50)) : string.Empty;
          borrowerInfo.BizEmail = olContact.Email2Address != null ? (olContact.Email2Address.IndexOf("@") != -1 ? this.truncateString(olContact.Email2Address, 50) : this.truncateString(olImport.GetSMTPEmailAddress(olContact.Email2Address), 50)) : string.Empty;
        }
        catch
        {
          this.AllowEmailAccess = false;
        }
      }
      borrowerInfo.Birthdate = olContact.Birthday.CompareTo(new DateTime(2200, 1, 1, 10, 0, 0)) >= 0 ? DateTime.MinValue : olContact.Birthday;
      borrowerInfo.Married = olContact.Spouse != null && !(olContact.Spouse == string.Empty);
      borrowerInfo.SpouseContactID = 0;
      borrowerInfo.SpouseName = this.truncateString(olContact.Spouse, 50);
      borrowerInfo.Anniversary = olContact.Anniversary.CompareTo(new DateTime(2200, 1, 1, 10, 0, 0)) >= 0 ? DateTime.MinValue : olContact.Anniversary;
      borrowerInfo.OwnerID = Session.UserInfo.Userid;
      borrowerInfo.AccessLevel = this.ImportParameters.AccessLevel;
      borrowerInfo.CustField1 = string.Empty;
      borrowerInfo.CustField2 = string.Empty;
      borrowerInfo.CustField3 = string.Empty;
      borrowerInfo.CustField4 = string.Empty;
      borrowerInfo.PrimaryEmail = "B";
      borrowerInfo.PrimaryPhone = "W";
      borrowerInfo.NoSpam = false;
      borrowerInfo.PrimaryContact = true;
      return borrowerInfo;
    }

    private string truncateString(string value, int length)
    {
      if (value == null)
        return string.Empty;
      return value.Length > length ? value.Substring(0, length) : value;
    }
  }
}
