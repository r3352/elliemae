// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BorrowerSummaryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BorrowerSummaryInfo : IPropertyDictionary
  {
    private Hashtable data;

    public BorrowerSummaryInfo(BorrowerInfo contact)
    {
      this.data = new Hashtable();
      this.ContactID = contact.ContactID;
      this.OwnerID = contact.OwnerID;
      this.LastName = contact.LastName;
      this.FirstName = contact.FirstName;
      this.HomePhone = contact.HomePhone;
      this.PersonalEmail = contact.PersonalEmail;
      this.NoSpam = contact.NoSpam;
      this.ContactType = contact.ContactType;
      this.Status = contact.Status;
      this.LastModified = contact.LastModified;
    }

    public BorrowerSummaryInfo(
      int contactId,
      string ownerId,
      string lastName,
      bool noSpam,
      string firstName,
      string homePhone,
      string personalEmail,
      BorrowerType contactType,
      string status,
      DateTime lastModified)
    {
      this.data = new Hashtable();
      this.ContactID = contactId;
      this.OwnerID = ownerId;
      this.LastName = lastName;
      this.FirstName = firstName;
      this.HomePhone = homePhone;
      this.PersonalEmail = personalEmail;
      this.NoSpam = noSpam;
      this.ContactType = contactType;
      this.Status = status;
      this.LastModified = lastModified;
    }

    public BorrowerSummaryInfo(Hashtable data) => this.data = data;

    public DateTime LastModified
    {
      get
      {
        return this.getField("Contact.LastModified") == null ? DateTime.MinValue : (DateTime) this.getField("Contact.LastModified");
      }
      set => this.setField("Contact.LastModified", (object) value);
    }

    public string Status
    {
      get
      {
        return this.getField("Contact.Status") == null ? "" : string.Concat(this.getField("Contact.Status"));
      }
      set => this.setField("Contact.Status", (object) value);
    }

    public BorrowerType ContactType
    {
      get
      {
        return this.getField("Contact.ContactType") == null ? BorrowerType.Blank : (BorrowerType) this.getField("Contact.ContactType");
      }
      set => this.setField("Contact.ContactType", (object) value);
    }

    public bool NoSpam
    {
      get
      {
        return this.getField("Contact.NoSpam") != null && string.Concat(this.getField("Contact.NoSpam")) == "true";
      }
      set => this.setField("Contact.NoSpam", (object) value);
    }

    public string PersonalEmail
    {
      get
      {
        return this.getField("Contact.PersonalEmail") == null ? "" : string.Concat(this.getField("Contact.PersonalEmail"));
      }
      set => this.setField("Contact.PersonalEmail", (object) value);
    }

    public string LastName
    {
      get
      {
        return this.getField("Contact.LastName") == null ? "" : string.Concat(this.getField("Contact.LastName"));
      }
      set => this.setField("Contact.LastName", (object) value);
    }

    public string FirstName
    {
      get
      {
        return this.getField("Contact.FirstName") == null ? "" : string.Concat(this.getField("Contact.FirstName"));
      }
      set => this.setField("Contact.FirstName", (object) value);
    }

    public string HomePhone
    {
      get
      {
        return this.getField("Contact.HomePhone") == null ? "" : string.Concat(this.getField("Contact.HomePhone"));
      }
      set => this.setField("Contact.HomePhone", (object) value);
    }

    public string OwnerID
    {
      get
      {
        return this.getField("Contact.OwnerID") == null ? "" : string.Concat(this.getField("Contact.OwnerID"));
      }
      set => this.setField("Contact.OwnerID", (object) value);
    }

    public int ContactID
    {
      get
      {
        return this.getField("Contact.ContactID") == null ? -1 : (int) this.getField("Contact.ContactID");
      }
      set => this.setField("Contact.ContactID", (object) value);
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
