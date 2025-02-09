// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContactStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ContactStore
  {
    private const string className = "ContactStore�";

    private ContactStore()
    {
    }

    public static Contact CheckOut(int contactId, ContactType contactType)
    {
      ContactStore.ContactIdentity identifier = new ContactStore.ContactIdentity(contactId, contactType);
      ICacheLock<bool?> innerLock = ClientContext.GetCurrent().Cache.CheckOutWithNull<bool?>(nameof (ContactStore), identifier.ToString(), (object) identifier);
      try
      {
        return contactType == ContactType.BizPartner ? (Contact) new BizPartnerContact(innerLock) : (Contact) new BorrowerContact(innerLock);
      }
      catch (Exception ex)
      {
        innerLock.UndoCheckout();
        Err.Reraise(nameof (ContactStore), ex);
        return (Contact) null;
      }
    }

    public static Contact GetLatestVersion(int contactId, ContactType contactType)
    {
      ContactStore.ContactIdentity id = new ContactStore.ContactIdentity(contactId, contactType);
      try
      {
        return contactType == ContactType.BizPartner ? (Contact) new BizPartnerContact(id) : (Contact) new BorrowerContact(id);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (ContactStore), ex);
        return (Contact) null;
      }
    }

    public class ContactIdentity
    {
      private int contactId;
      private ContactType contactType;

      public ContactIdentity(int contactId, ContactType contactType)
      {
        this.contactId = contactId;
        this.contactType = contactType;
      }

      public int ContactID => this.contactId;

      public ContactType ContactType => this.contactType;

      public override int GetHashCode() => this.contactId;

      public override bool Equals(object obj)
      {
        ContactStore.ContactIdentity contactIdentity = obj as ContactStore.ContactIdentity;
        return obj != null && this.contactId == contactIdentity.contactId && this.contactType == contactIdentity.contactType;
      }

      public override string ToString()
      {
        return "(" + this.contactType.ToString() + "," + (object) this.contactId + ")";
      }
    }
  }
}
