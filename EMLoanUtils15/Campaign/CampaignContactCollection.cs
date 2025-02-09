// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignContactCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using System;
using System.Collections;
using System.ComponentModel;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignContactCollection : BusinessCollectionBase
  {
    public CampaignContact this[int index] => (CampaignContact) this.List[index];

    public void Add(CampaignContact campaignContact) => this.List.Add((object) campaignContact);

    public void Remove(CampaignContact campaignContact)
    {
      CampaignContact campaignContact1 = this.Find(campaignContact.ContactId);
      if (campaignContact1 == null)
        return;
      this.List.Remove((object) campaignContact1);
    }

    public CampaignContact Find(int contactId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (contactId == ((CampaignContact) obj).ContactId)
          return (CampaignContact) obj;
      }
      return (CampaignContact) null;
    }

    public void Sort(string fieldName, bool directionAscending)
    {
      ListSortDirection direction = !directionAscending ? ListSortDirection.Descending : ListSortDirection.Ascending;
      IBindingList bindingList = (IBindingList) this;
      PropertyDescriptor property = TypeDescriptor.GetProperties(typeof (CampaignContact)).Find(fieldName, false);
      if (property == null)
        return;
      bindingList.ApplySort(property, direction);
    }

    public bool Contains(CampaignContact campaignContact)
    {
      foreach (CampaignContact campaignContact1 in (IEnumerable) this.List)
      {
        if (campaignContact1.Equals(campaignContact))
          return true;
      }
      return false;
    }

    public bool ContainsDeleted(CampaignContact campaignContact)
    {
      foreach (CampaignContact deleted in (CollectionBase) this.deletedList)
      {
        if (deleted.Equals(campaignContact))
          return true;
      }
      return false;
    }

    public static CampaignContactCollection NewCampaignContactCollection()
    {
      return new CampaignContactCollection();
    }

    public static CampaignContactCollection NewCampaignContactCollection(int capacity)
    {
      return new CampaignContactCollection(capacity);
    }

    private CampaignContactCollection()
      : this(16)
    {
    }

    private CampaignContactCollection(int capacity)
    {
      if (16 > capacity)
        capacity = 16;
      this.InnerList.Capacity = capacity;
      this.MarkAsChild();
      this.AllowSort = true;
    }
  }
}
