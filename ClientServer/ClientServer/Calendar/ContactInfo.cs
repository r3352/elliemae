// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Calendar.ContactInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Calendar
{
  [Serializable]
  public class ContactInfo
  {
    private string contactName;
    private string contactID;
    private CategoryType contactType;

    public ContactInfo()
    {
      this.contactName = "";
      this.contactID = "";
      this.contactType = CategoryType.Borrower;
    }

    public ContactInfo(string ContactName, string ContactID)
    {
      this.contactID = ContactID;
      this.contactName = ContactName;
      this.SetContactTypeAsBorrower();
    }

    public ContactInfo(string ContactName, string ContactID, CategoryType ContactType)
    {
      this.contactID = ContactID;
      this.contactName = ContactName;
      this.contactType = ContactType;
    }

    public ContactInfo(string ContactID, CategoryType ContactType)
    {
      this.contactID = ContactID;
      this.contactName = "";
      this.contactType = ContactType;
    }

    public ContactInfo(string ContactID)
    {
      this.contactID = ContactID;
      this.contactName = "";
      this.SetContactTypeAsBorrower();
    }

    public void SetContactTypeAsBorrower() => this.contactType = CategoryType.Borrower;

    public void SetContactTypeAsBizPartner() => this.contactType = CategoryType.BizPartner;

    public string ContactName
    {
      get => this.contactName;
      set => this.contactName = value;
    }

    public string ContactID
    {
      get => this.contactID;
      set => this.contactID = value;
    }

    public CategoryType ContactType
    {
      get => this.contactType;
      set => this.contactType = value;
    }

    public string ContactDisplay
    {
      get
      {
        switch (this.contactType)
        {
          case CategoryType.Borrower:
            return this.contactName + "  ( Borrower Contact )";
          case CategoryType.BizPartner:
            return this.contactName + "  ( Business Contact )";
          default:
            return this.contactName;
        }
      }
    }

    public object this[string columnName]
    {
      get
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "contactname":
            return (object) this.contactName;
          case "contactid":
            return (object) this.contactID;
          case "contacttype":
            return (object) this.contactType;
          default:
            return (object) null;
        }
      }
    }
  }
}
