// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignStepCollection
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
  public class CampaignStepCollection : BusinessCollectionBase
  {
    public CampaignStep this[int index] => (CampaignStep) this.List[index];

    public void Add(CampaignStep campaignStep) => this.List.Add((object) campaignStep);

    public void Remove(CampaignStep campaignStep) => this.List.Remove((object) campaignStep);

    public CampaignStep Find(int campaignStepId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (campaignStepId == ((CampaignStep) obj).CampaignStepId)
          return (CampaignStep) obj;
      }
      return (CampaignStep) null;
    }

    public void Sort(string fieldName, bool directionAscending)
    {
      ListSortDirection direction = !directionAscending ? ListSortDirection.Descending : ListSortDirection.Ascending;
      IBindingList bindingList = (IBindingList) this;
      PropertyDescriptor property = TypeDescriptor.GetProperties(typeof (CampaignStep)).Find(fieldName, false);
      if (property == null)
        return;
      bindingList.ApplySort(property, direction);
    }

    public bool Contains(CampaignStep campaignStep)
    {
      foreach (CampaignStep campaignStep1 in (IEnumerable) this.List)
      {
        if (campaignStep1.Equals(campaignStep))
          return true;
      }
      return false;
    }

    public bool ContainsDeleted(CampaignStep campaignStep)
    {
      foreach (CampaignStep deleted in (CollectionBase) this.deletedList)
      {
        if (deleted.Equals(campaignStep))
          return true;
      }
      return false;
    }

    public static CampaignStepCollection NewCampaignStepCollection()
    {
      return new CampaignStepCollection();
    }

    public static CampaignStepCollection NewCampaignStepCollection(int capacity)
    {
      return new CampaignStepCollection(capacity);
    }

    private CampaignStepCollection()
      : this(16)
    {
    }

    private CampaignStepCollection(int capacity)
    {
      if (16 > capacity)
        capacity = 16;
      this.InnerList.Capacity = capacity;
      this.MarkAsChild();
      this.AllowSort = true;
    }
  }
}
