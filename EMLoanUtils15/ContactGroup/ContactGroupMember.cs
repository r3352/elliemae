// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactGroupMember
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public abstract class ContactGroupMember : BusinessBase, IDisposable
  {
    private int groupId;
    private int contactId;
    private ContactType contactType;

    public int GroupId => this.groupId;

    public int ContactId => this.contactId;

    public ContactType ContactType => this.contactType;

    public abstract string LastName { get; }

    public abstract string FirstName { get; }

    public abstract string PhoneNumber { get; }

    public abstract string EmailAddress { get; }

    public abstract bool NoSpam { get; }

    public abstract DateTime DateAdded { get; }

    public override string ToString()
    {
      return string.Format("ContactGroupMember[{0}-{1}]", (object) this.GroupId, (object) this.ContactId);
    }

    public bool Equals(ContactGroupMember obj)
    {
      return this.ContactType.Equals((object) obj.ContactType) && this.ContactId.Equals(obj.ContactId);
    }

    public override int GetHashCode()
    {
      return string.Format("{0}-{1}", (object) this.GroupId, (object) this.ContactId).GetHashCode();
    }

    protected ContactGroupMember(int groupId, int contactId, ContactType contactType)
    {
      this.MarkAsChild();
      this.groupId = groupId;
      this.contactId = contactId;
      this.contactType = contactType;
    }

    public virtual void Dispose()
    {
    }

    [Serializable]
    private class Criteria
    {
      public int GroupId;
      public int ContactId;

      public Criteria(int groupId, int contactId)
      {
        this.GroupId = groupId;
        this.ContactId = contactId;
      }
    }
  }
}
