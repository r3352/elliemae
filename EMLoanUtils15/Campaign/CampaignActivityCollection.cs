// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignActivityCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignActivityCollection : BusinessCollectionBase
  {
    public CampaignActivity this[int index] => (CampaignActivity) this.List[index];

    public void Add(CampaignActivity campaignActivity) => this.List.Add((object) campaignActivity);

    public void Remove(CampaignActivity campaignActivity)
    {
      this.List.Remove((object) campaignActivity);
    }

    public CampaignActivity Find(int campaignStepId, int contactId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (campaignStepId == ((CampaignActivity) obj).CampaignStepId && contactId == ((CampaignActivity) obj).ContactId)
          return (CampaignActivity) obj;
      }
      return (CampaignActivity) null;
    }

    public bool Contains(CampaignActivity item)
    {
      foreach (CampaignActivity campaignActivity in (IEnumerable) this.List)
      {
        if (campaignActivity.Equals(item))
          return true;
      }
      return false;
    }

    public bool ContainsDeleted(CampaignActivity item)
    {
      foreach (CampaignActivity deleted in (CollectionBase) this.deletedList)
      {
        if (deleted.Equals(item))
          return true;
      }
      return false;
    }

    public static CampaignActivityCollection NewCampaignActivityCollection()
    {
      return new CampaignActivityCollection();
    }

    public static CampaignActivityCollection NewCampaignActivityCollection(int capacity)
    {
      return new CampaignActivityCollection(capacity);
    }

    private CampaignActivityCollection() => this.MarkAsChild();

    private CampaignActivityCollection(int capacity)
    {
      if (16 > capacity)
        capacity = 16;
      this.InnerList.Capacity = capacity;
      this.MarkAsChild();
    }
  }
}
