// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BizPartnerSummaryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BizPartnerSummaryInfo : IPropertyDictionary
  {
    private Hashtable data;

    public BizPartnerSummaryInfo(BizPartnerInfo contact)
    {
      this.data = new Hashtable();
      this.ContactID = contact.ContactID;
      this.CompanyName = contact.CompanyName;
      this.LastName = contact.LastName;
      this.FirstName = contact.FirstName;
      this.WorkPhone = contact.WorkPhone;
      this.BizEmail = contact.BizEmail;
      this.NoSpam = contact.NoSpam;
      this.AccessLevel = contact.AccessLevel;
      this.CategoryID = contact.CategoryID;
      this.LastModified = contact.LastModified;
    }

    public BizPartnerSummaryInfo(
      int contactId,
      int categoryId,
      string companyName,
      string lastName,
      string firstName,
      string workPhone,
      string bizEmail,
      bool noSpam,
      ContactAccess accessLevel,
      DateTime lastModified)
    {
      this.data = new Hashtable();
      this.ContactID = contactId;
      this.CategoryID = categoryId;
      this.CompanyName = companyName;
      this.LastName = lastName;
      this.FirstName = firstName;
      this.WorkPhone = workPhone;
      this.BizEmail = bizEmail;
      this.NoSpam = noSpam;
      this.AccessLevel = accessLevel;
      this.LastModified = lastModified;
    }

    public BizPartnerSummaryInfo(Hashtable data) => this.data = data;

    public ContactAccess AccessLevel
    {
      get
      {
        return this.getField("Contact.AccessLevel") == null ? ContactAccess.Private : (ContactAccess) this.getField("Contact.AccessLevel");
      }
      set => this.setField("Contact.AccessLevel", (object) value);
    }

    public string WorkPhone
    {
      get
      {
        return this.getField("Contact.WorkPhone") == null ? "" : string.Concat(this.getField("Contact.WorkPhone"));
      }
      set => this.setField("Contact.WorkPhone", (object) value);
    }

    public string CompanyName
    {
      get
      {
        return this.getField("Contact.CompanyName") == null ? "" : string.Concat(this.getField("Contact.CompanyName"));
      }
      set => this.setField("Contact.CompanyName", (object) value);
    }

    public int CategoryID
    {
      get
      {
        return this.getField("Contact.CategoryID") == null ? -1 : (int) this.getField("Contact.CategoryID");
      }
      set => this.setField("Contact.CategoryID", (object) value);
    }

    public int ContactID
    {
      get
      {
        return this.getField("Contact.ContactID") == null ? -1 : (int) this.getField("Contact.ContactID");
      }
      set => this.setField("Contact.ContactID", (object) value);
    }

    public string FirstName
    {
      get
      {
        return this.getField("Contact.FirstName") == null ? "" : string.Concat(this.getField("Contact.FirstName"));
      }
      set => this.setField("Contact.FirstName", (object) value);
    }

    public string LastName
    {
      get
      {
        return this.getField("Contact.LastName") == null ? "" : string.Concat(this.getField("Contact.LastName"));
      }
      set => this.setField("Contact.LastName", (object) value);
    }

    public string BizEmail
    {
      get
      {
        return this.getField("Contact.BizEmail") == null ? "" : string.Concat(this.getField("Contact.BizEmail"));
      }
      set => this.setField("Contact.BizEmail", (object) value);
    }

    public bool NoSpam
    {
      get
      {
        return this.getField("Contact.NoSpam") != null && string.Concat(this.getField("Contact.NoSpam")) == "true";
      }
      set => this.setField("Contact.NoSpam", (object) value);
    }

    public DateTime LastModified
    {
      get
      {
        return this.getField("Contact.LastModified") == null ? DateTime.MinValue : (DateTime) this.getField("Contact.LastModified");
      }
      set => this.setField("Contact.LastModified", (object) value);
    }

    public object this[string propertyName]
    {
      get => this.getField(propertyName);
      set => throw new NotImplementedException();
    }

    private object getField(string fieldName)
    {
      if (this.data.ContainsKey((object) fieldName))
        return this.data[(object) fieldName];
      if (this.data.ContainsKey((object) fieldName.Replace(" ", "")))
        return this.data[(object) fieldName.Replace(" ", "")];
      return this.data.ContainsKey((object) ("Contact." + fieldName)) ? this.data[(object) ("Contact." + fieldName)] : (object) null;
    }

    private void setField(string fieldName, object value)
    {
      if (this.data.ContainsKey((object) fieldName))
        this.data[(object) fieldName] = value;
      else if (this.data.ContainsKey((object) ("Contact." + fieldName)))
        this.data[(object) ("Contact." + fieldName)] = value;
      else
        this.data.Add((object) fieldName, value);
    }
  }
}
