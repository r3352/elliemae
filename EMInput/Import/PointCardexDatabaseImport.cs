// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointCardexDatabaseImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.ContactUI.Import;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointCardexDatabaseImport
  {
    private ArrayList pointCardex = new ArrayList();
    private string source = string.Empty;
    private ContactAccess accessLevel;
    private ContactGroupInfo[] groupList;
    private PointImportDuplicateDialog dupDialog = new PointImportDuplicateDialog();
    private bool bReplaceAll;

    public event ContactEventHandler ContactCreated;

    public event ContactEventHandler ContactOverwritten;

    public PointCardexDatabaseImport(string source, ContactAccess accessLevel)
    {
      this.source = Path.Combine(source, "Pointcdx.mdb");
      this.accessLevel = accessLevel;
    }

    public PointCardexDatabaseImport(
      string source,
      ContactAccess accessLevel,
      ContactGroupInfo[] groupList)
    {
      this.source = Path.Combine(source, "Pointcdx.mdb");
      this.accessLevel = accessLevel;
      this.groupList = groupList;
    }

    public DialogResult ImportContacts(object state, IProgressFeedback feedback)
    {
      feedback.Status = "Opening Point CARDEX database...";
      try
      {
        if (!this.OpenDatabase())
          return DialogResult.Abort;
        Hashtable categoryNameToIdTable = new BizCategoryUtil(Session.SessionObjects).GetCategoryNameToIdTable();
        feedback.Status = "Importing " + (object) this.pointCardex.Count + " contacts...";
        feedback.ResetCounter(this.pointCardex.Count);
        int num1 = 0;
        for (int index = 0; index < this.pointCardex.Count; ++index)
        {
          if (feedback.Cancel)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Import cancelled. " + (object) num1 + " contact(s) successfully imported.");
            return DialogResult.Cancel;
          }
          string[] fields = this.pointCardex[index].ToString().Split('\u001F');
          string str = fields[0];
          feedback.Details = "Importing contact " + fields[1] + "...";
          if (!categoryNameToIdTable.Contains((object) str) && this.accessLevel == ContactAccess.Public)
          {
            BizCategory bizCategory = Session.ContactManager.AddBizCategory(str);
            categoryNameToIdTable.Add((object) bizCategory.Name, (object) bizCategory.CategoryID);
          }
          switch (this.AddNewContact(fields))
          {
            case ContactImportDupOption.ReplaceAll:
            case ContactImportDupOption.Replace:
            case ContactImportDupOption.CreateNew:
              ++num1;
              break;
            case ContactImportDupOption.Abort:
              return DialogResult.Cancel;
          }
          feedback.Increment(1);
        }
        int num3 = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "Successfully imported " + (object) num1 + " contact(s).");
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) feedback.ParentForm, "An error occurred during import: " + ex.Message + ".");
        return DialogResult.Abort;
      }
    }

    private ContactImportDupOption AddNewContact(string[] fields)
    {
      BizPartnerInfo bizPartnerInfo = new BizPartnerInfo(Session.UserID);
      bizPartnerInfo.AccessLevel = this.accessLevel;
      bizPartnerInfo.CategoryID = this.GetCategoryID(fields[0]);
      bizPartnerInfo.FirstName = this.ParseName(fields[1], "First");
      bizPartnerInfo.LastName = this.ParseName(fields[1], "Last");
      bizPartnerInfo.JobTitle = fields[2];
      bizPartnerInfo.CompanyName = fields[3];
      bizPartnerInfo.BizAddress.Street1 = fields[4];
      bizPartnerInfo.BizAddress.City = fields[5];
      bizPartnerInfo.BizAddress.State = fields[6];
      bizPartnerInfo.BizAddress.Zip = fields[7];
      bizPartnerInfo.WorkPhone = fields[8];
      bizPartnerInfo.FaxNumber = fields[9];
      bizPartnerInfo.MobilePhone = fields[10];
      bizPartnerInfo.HomePhone = fields[11];
      bizPartnerInfo.Comment = fields[12];
      bizPartnerInfo.PersonalEmail = fields[14];
      bizPartnerInfo.BizEmail = fields[13];
      if (this.accessLevel == ContactAccess.Public)
        bizPartnerInfo.OwnerID = "";
      ContactImportDupOption contactImportDupOption = ContactImportUtil.SaveBizPartnerContactInfo((IWin32Window) Form.ActiveForm, bizPartnerInfo, (ContactCustomField[]) null, (CustomFieldValue[]) null, (CustomFieldValue[]) null, this.groupList, this.bReplaceAll, ContactSource.Point);
      if (contactImportDupOption == ContactImportDupOption.ReplaceAll)
        this.bReplaceAll = true;
      if (contactImportDupOption == ContactImportDupOption.ReplaceAll || contactImportDupOption == ContactImportDupOption.Replace)
        this.OnContactOverwritten(bizPartnerInfo);
      else if (contactImportDupOption == ContactImportDupOption.CreateNew)
        this.OnContactCreated(bizPartnerInfo);
      return contactImportDupOption;
    }

    private int GetCategoryID(string catName)
    {
      BizCategoryUtil bizCategoryUtil = new BizCategoryUtil(Session.SessionObjects);
      Hashtable categoryNameToIdTable = bizCategoryUtil.GetCategoryNameToIdTable();
      return !categoryNameToIdTable.Contains((object) catName) ? bizCategoryUtil.GetNoCategoryId() : (int) Convert.ToInt16(categoryNameToIdTable[(object) catName]);
    }

    private string ParseName(string name, string indicator)
    {
      string empty = string.Empty;
      string name1;
      try
      {
        switch (indicator)
        {
          case "First":
            name1 = name.Substring(0, name.IndexOf(" "));
            break;
          case "Last":
            name1 = name.Substring(name.IndexOf(" ") + 1);
            break;
          default:
            name1 = string.Empty;
            break;
        }
      }
      catch
      {
        return string.Empty;
      }
      return name1;
    }

    private bool OpenDatabase()
    {
      OleDbConnection connection;
      try
      {
        connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.source + ";");
        connection.Open();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Unable to open database: " + ex.Source + "-" + ex.Message);
        return false;
      }
      string cmdText = "Select * FROM Cardex";
      try
      {
        OleDbDataReader oleDbDataReader = new OleDbCommand(cmdText, connection).ExecuteReader();
        if (!oleDbDataReader.HasRows)
        {
          int num = (int) MessageBox.Show("The database '" + this.source + "' does not have any records stored. Please setup your cardex database and then import.");
          return false;
        }
        while (oleDbDataReader.Read())
        {
          string str = string.Empty;
          for (int ordinal = 1; ordinal < oleDbDataReader.FieldCount; ++ordinal)
          {
            string empty = string.Empty;
            if (!oleDbDataReader.IsDBNull(ordinal))
              empty = oleDbDataReader.GetString(ordinal);
            str = str + empty + "\u001F";
          }
          this.pointCardex.Add((object) str);
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Unable to read records from table '" + this.source + "'." + ex.Message);
        return false;
      }
      connection.Close();
      return true;
    }

    protected void OnContactCreated(BizPartnerInfo contactInfo)
    {
      if (this.ContactCreated == null)
        return;
      this.ContactCreated((object) this, contactInfo);
    }

    protected void OnContactOverwritten(BizPartnerInfo contactInfo)
    {
      if (this.ContactOverwritten == null)
        return;
      this.ContactOverwritten((object) this, contactInfo);
    }
  }
}
