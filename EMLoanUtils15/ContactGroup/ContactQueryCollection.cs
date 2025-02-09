// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactGroup.ContactQueryCollection
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
  public class ContactQueryCollection : EllieMae.EMLite.BizLayer.ReadOnlyCollectionBase
  {
    public ContactQuery this[int index] => (ContactQuery) this.List[index];

    public void Add(ContactQuery contactQuery)
    {
      this.locked = false;
      this.List.Add((object) contactQuery);
      this.locked = true;
    }

    public bool Contains(ContactQuery contactQuery)
    {
      foreach (ContactQuery contactQuery1 in (IEnumerable) this.List)
      {
        if (contactQuery1.Equals(contactQuery))
          return true;
      }
      return false;
    }

    public static ContactQueryCollection NewContactQueryCollection()
    {
      return new ContactQueryCollection();
    }

    public static ContactQueryCollection NewContactQueryCollection(int capacity)
    {
      return new ContactQueryCollection(capacity);
    }

    public static ContactQueryCollection GetContactQueryCollection(
      ContactQueryCollectionCriteria criteria,
      SessionObjects sessionObjects)
    {
      ContactQueryInfo[] contactQueries = sessionObjects.ContactGroupManager.GetContactQueries(criteria);
      ContactQueryCollection contactQueryCollection = new ContactQueryCollection(contactQueries.Length);
      foreach (ContactQueryInfo queryInfo in contactQueries)
        contactQueryCollection.Add(ContactQuery.NewContactQuery(queryInfo));
      return contactQueryCollection;
    }

    private ContactQueryCollection()
      : this(16)
    {
    }

    private ContactQueryCollection(int capacity) => this.InnerList.Capacity = capacity;

    [Serializable]
    public class Criteria
    {
      public string UserId;
      public ContactType ContactType;
      public ContactQueryType ContactQueryType;

      public Criteria(string userId, ContactType contactType, ContactQueryType contactQueryType)
      {
        this.UserId = userId;
        this.ContactType = contactType;
        this.ContactQueryType = contactQueryType;
      }
    }
  }
}
