// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignCollection
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignCollection : BusinessCollectionBase
  {
    public EllieMae.EMLite.Campaign.Campaign this[int index] => (EllieMae.EMLite.Campaign.Campaign) this.List[index];

    public void Add(EllieMae.EMLite.Campaign.Campaign campaign) => this.List.Add((object) campaign);

    public void Remove(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.List.Remove((object) campaign);
    }

    public EllieMae.EMLite.Campaign.Campaign Find(int campaignId)
    {
      foreach (object obj in (IEnumerable) this.List)
      {
        if (campaignId == ((EllieMae.EMLite.Campaign.Campaign) obj).CampaignId)
          return (EllieMae.EMLite.Campaign.Campaign) obj;
      }
      return (EllieMae.EMLite.Campaign.Campaign) null;
    }

    public bool Contains(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      foreach (EllieMae.EMLite.Campaign.Campaign campaign1 in (IEnumerable) this.List)
      {
        if (campaign1.Equals(campaign))
          return true;
      }
      return false;
    }

    public bool ContainsDeleted(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      foreach (EllieMae.EMLite.Campaign.Campaign deleted in (CollectionBase) this.deletedList)
      {
        if (deleted.Equals(campaign))
          return true;
      }
      return false;
    }

    public void RefreshCampaignList()
    {
      foreach (EllieMae.EMLite.Campaign.Campaign campaign in (IEnumerable) this.List)
        campaign.Refresh();
    }

    public static CampaignCollection NewCampaignCollection() => new CampaignCollection();

    public static CampaignCollection NewCampaignCollection(int capacity)
    {
      return new CampaignCollection(capacity);
    }

    public static CampaignCollection GetCampaignCollection(
      CampaignCollectionCriteria criteria,
      SessionObjects sessionObjects)
    {
      CampaignInfo[] campaignsForUser = sessionObjects.CampaignManager.GetCampaignsForUser(criteria);
      CampaignCollection campaignCollection = new CampaignCollection(campaignsForUser == null ? 0 : campaignsForUser.Length);
      if (campaignsForUser != null)
      {
        foreach (CampaignInfo campaignInfo in campaignsForUser)
          campaignCollection.Add(EllieMae.EMLite.Campaign.Campaign.NewCampaign(campaignInfo, sessionObjects));
      }
      return campaignCollection;
    }

    private CampaignCollection()
    {
    }

    private CampaignCollection(int capacity)
    {
      if (16 > capacity)
        capacity = 16;
      this.InnerList.Capacity = capacity;
    }
  }
}
