// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactGroupMemberCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class ContactGroupMemberCollection : BusinessCollectionBase
  {
    private Hashtable addedMemberIds = new Hashtable();
    private Hashtable deletedMemberIds = new Hashtable();

    public ContactGroupMember this[int index] => (ContactGroupMember) this.List[index];

    public void Add(ContactGroupMember contactGroupMember)
    {
      this.List.Add((object) contactGroupMember);
      this.addedMemberIds.Add((object) contactGroupMember.ContactId, (object) contactGroupMember);
      this.deletedMemberIds.Remove((object) contactGroupMember.ContactId);
    }

    public void Remove(ContactGroupMember contactGroupMember)
    {
      this.List.Remove((object) contactGroupMember);
      this.addedMemberIds.Remove((object) contactGroupMember.ContactId);
      this.deletedMemberIds.Add((object) contactGroupMember.ContactId, (object) contactGroupMember);
    }

    public ContactGroupMember Find(int contactId)
    {
      return this.addedMemberIds.Contains((object) contactId) ? (ContactGroupMember) this.addedMemberIds[(object) contactId] : (ContactGroupMember) null;
    }

    public void Sort(string fieldName, bool directionAscending)
    {
      ListSortDirection direction = !directionAscending ? ListSortDirection.Descending : ListSortDirection.Ascending;
      IBindingList bindingList = (IBindingList) this;
      PropertyDescriptor property = TypeDescriptor.GetProperties(typeof (EventInfo)).Find(fieldName, false);
      if (property == null)
        return;
      bindingList.ApplySort(property, direction);
    }

    public bool Contains(ContactGroupMember contactGroupMember)
    {
      return this.addedMemberIds.Contains((object) contactGroupMember.ContactId);
    }

    public bool ContainsDeleted(ContactGroupMember contactGroupMember)
    {
      return this.deletedMemberIds.Contains((object) contactGroupMember.ContactId);
    }

    public static ContactGroupMemberCollection NewContactGroupMemberCollection()
    {
      return new ContactGroupMemberCollection();
    }

    public static ContactGroupMemberCollection NewContactGroupMemberCollection(int capacity)
    {
      return new ContactGroupMemberCollection(capacity);
    }

    private ContactGroupMemberCollection()
      : this(16)
    {
    }

    private ContactGroupMemberCollection(int capacity)
    {
      this.InnerList.Capacity = capacity;
      this.MarkAsChild();
      this.AllowSort = true;
      this.Sort("LastName", true);
    }
  }
}
