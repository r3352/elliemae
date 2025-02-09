// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactGroupCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactGroup
{
  [Serializable]
  public class ContactGroupCollection : EllieMae.EMLite.BizLayer.ReadOnlyCollectionBase
  {
    public EllieMae.EMLite.ContactGroup.ContactGroup this[int index]
    {
      get => (EllieMae.EMLite.ContactGroup.ContactGroup) this.List[index];
    }

    public void Add(EllieMae.EMLite.ContactGroup.ContactGroup contactGroup)
    {
      this.locked = false;
      this.List.Add((object) contactGroup);
      this.locked = true;
    }

    public bool Contains(EllieMae.EMLite.ContactGroup.ContactGroup contactGroup)
    {
      foreach (EllieMae.EMLite.ContactGroup.ContactGroup contactGroup1 in (IEnumerable) this.List)
      {
        if (contactGroup1.Equals(contactGroup))
          return true;
      }
      return false;
    }

    public static ContactGroupCollection NewContactGroupCollection()
    {
      return new ContactGroupCollection();
    }

    public static ContactGroupCollection NewContactGroupCollection(int capacity)
    {
      return new ContactGroupCollection(capacity);
    }

    public static ContactGroupCollection GetContactGroupCollection(
      ContactGroupCollectionCriteria criteria,
      SessionObjects sessionObjects)
    {
      ContactGroupInfo[] contactGroupsForUser = sessionObjects.ContactGroupManager.GetContactGroupsForUser(criteria);
      int num = criteria.ContactType == ContactType.Borrower ? 2 : 1;
      ContactGroupCollection contactGroupCollection = new ContactGroupCollection(contactGroupsForUser.Length + num);
      ContactGroupInfo groupInfo1 = new ContactGroupInfo(PsuedoGroupId.AllContacts, sessionObjects.UserID, criteria.ContactType, ContactGroupType.ContactGroup, "All Contacts", string.Empty, DateTime.Today, (int[]) null);
      contactGroupCollection.Add(EllieMae.EMLite.ContactGroup.ContactGroup.NewContactGroup(groupInfo1, sessionObjects));
      if (criteria.ContactType == ContactType.Borrower)
      {
        ContactGroupInfo groupInfo2 = new ContactGroupInfo(PsuedoGroupId.MyContacts, sessionObjects.UserID, criteria.ContactType, ContactGroupType.ContactGroup, "My Contacts", string.Empty, DateTime.Today, (int[]) null);
        contactGroupCollection.Add(EllieMae.EMLite.ContactGroup.ContactGroup.NewContactGroup(groupInfo2, sessionObjects));
      }
      foreach (ContactGroupInfo groupInfo3 in contactGroupsForUser)
        contactGroupCollection.Add(EllieMae.EMLite.ContactGroup.ContactGroup.NewContactGroup(groupInfo3, sessionObjects));
      return contactGroupCollection;
    }

    private ContactGroupCollection()
      : this(16)
    {
    }

    private ContactGroupCollection(int capacity) => this.InnerList.Capacity = capacity;

    [Serializable]
    public class Criteria
    {
      public string UserId;
      public ContactType ContactType;

      public Criteria(string userId, ContactType contactType)
      {
        this.UserId = userId;
        this.ContactType = contactType;
      }
    }
  }
}
