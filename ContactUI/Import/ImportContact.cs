// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.ImportContact
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using MAPI;
using Outlook;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class ImportContact
  {
    private NameSpace nameSpace;
    private Outlook.Application outlook;
    private static string className = nameof (ImportContact);
    private static string sw = Tracing.SwContact;
    private Session session;
    private Hashtable GALHashTable = new Hashtable();
    private bool CDO_INITIALIZED;
    private bool _Initialized;
    private object oFalse = (object) false;
    private object oTrue = (object) true;
    private object oMissing = (object) Missing.Value;

    public bool Initialized => this._Initialized;

    public ImportContact()
    {
      try
      {
        this.outlook = (Outlook.Application) new ApplicationClass();
        this.nameSpace = this.outlook.GetNamespace("MAPI");
        this.nameSpace.Logon((object) "", (object) "", this.oFalse, this.oTrue);
        this._Initialized = true;
      }
      catch
      {
        throw new OutlookNotInitializedException();
      }
    }

    public bool InitCDO()
    {
      if (this.CDO_INITIALIZED)
        return true;
      this.CDO_INITIALIZED = true;
      try
      {
        this.session = (Session) new SessionClass();
        this.session.Logon((object) "Default Outlook Profile", this.oMissing, this.oFalse, this.oFalse, this.oMissing, this.oMissing, this.oMissing);
        this.BuildGALHashTable();
      }
      catch (System.Exception ex)
      {
        Tracing.Log(ImportContact.sw, TraceLevel.Warning, ImportContact.className, "CDO Exception" + ex.Message + "\n" + ex.StackTrace);
        return new CDOHelpDialog().ShowDialog() == DialogResult.OK;
      }
      return true;
    }

    public void Clear()
    {
      try
      {
        if (this.nameSpace != null)
          this.nameSpace.Logoff();
        this.nameSpace = (NameSpace) null;
      }
      catch
      {
      }
      try
      {
        if (this.outlook != null)
          this.outlook.Quit();
        this.outlook = (Outlook.Application) null;
      }
      catch
      {
      }
      try
      {
        if (this.session != null)
          this.session.Logoff();
        this.session = (Session) null;
      }
      catch
      {
      }
    }

    public MAPIFolder GetDefaultFolder()
    {
      return this.nameSpace == null ? (MAPIFolder) null : this.nameSpace.GetDefaultFolder(OlDefaultFolders.olFolderContacts);
    }

    public MAPIFolder SelectFolder()
    {
      if (this.nameSpace == null)
        return (MAPIFolder) null;
      MAPIFolder mapiFolder;
      while ((mapiFolder = this.nameSpace.PickFolder()) != null && mapiFolder.DefaultItemType != OlItemType.olContactItem)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "The selected folder is not a contact folder. Please select a contact folder.", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
      }
      return mapiFolder;
    }

    public ContactItem[] SelectContacts(MAPIFolder contactFolder)
    {
      if (contactFolder == null)
        return (ContactItem[]) null;
      ArrayList arrayList = new ArrayList();
      Items items = contactFolder.Items;
      for (object obj = items.GetFirst(); obj != null; obj = items.GetNext())
      {
        if (obj is ContactItem)
          arrayList.Add(obj);
      }
      return (ContactItem[]) arrayList.ToArray(typeof (ContactItem));
    }

    private void BuildGALHashTable()
    {
      int Index1 = 805503006;
      int Index2 = 972947486;
      MAPI.AddressEntries addressEntries = (MAPI.AddressEntries) ((MAPI.AddressList) this.session.GetAddressList((object) CdoAddressListTypes.CdoAddressListGAL)).AddressEntries;
      MAPI.AddressEntry addressEntry = (MAPI.AddressEntry) addressEntries.GetFirst();
      while (addressEntry != null)
      {
        try
        {
          Fields fields = (Fields) addressEntry.Fields;
          this.GALHashTable.Add((object) ((Field) fields.get_Item((object) Index1, this.oMissing)).Value.ToString(), (object) ((Field) fields.get_Item((object) Index2, this.oMissing)).Value.ToString());
        }
        catch (System.Exception ex)
        {
          Tracing.Log(ImportContact.sw, TraceLevel.Warning, ImportContact.className, "Failed to build GAL Hashtable entry \"" + addressEntry.Name + "\": " + ex.Message + " " + ex.StackTrace);
        }
        finally
        {
          addressEntry = (MAPI.AddressEntry) addressEntries.GetNext();
        }
      }
    }

    public string GetSMTPEmailAddress(string emailID)
    {
      return emailID != "" && this.GALHashTable.ContainsKey((object) emailID) ? this.GALHashTable[(object) emailID].ToString() : "";
    }

    public static ArrayList GetContactFolderList()
    {
      Outlook.Application application = (Outlook.Application) new ApplicationClass();
      NameSpace nameSpace = application.GetNamespace("MAPI");
      nameSpace.Logon((object) "exchtest", (object) "net", (object) false, (object) true);
      ArrayList contactFolder = ImportContact.FindContactFolder(nameSpace.Folders, new ArrayList());
      nameSpace.Logoff();
      application.Quit();
      return contactFolder;
    }

    private static ArrayList FindContactFolder(Outlook.Folders root, ArrayList ContactFolderList)
    {
      if (root == null)
        return ContactFolderList;
      for (MAPIFolder mapiFolder = root.GetFirst(); mapiFolder != null; mapiFolder = root.GetNext())
      {
        if (mapiFolder.DefaultItemType.ToString() == "olContactItem")
          ContactFolderList.Add((object) mapiFolder);
        ContactFolderList = ImportContact.FindContactFolder(mapiFolder.Folders, ContactFolderList);
      }
      return ContactFolderList;
    }
  }
}
