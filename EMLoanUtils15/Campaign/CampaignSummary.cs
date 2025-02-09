// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignSummary
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignSummary : ReadOnlyBase
  {
    private CampaignSummaryInfo summaryInfo;

    public int this[CampaignStatus campaignStatus]
    {
      get => this.summaryInfo[campaignStatus];
      set => this.summaryInfo[campaignStatus] = value;
    }

    public int this[ActivityType activityType]
    {
      get => this.summaryInfo[activityType];
      set => this.summaryInfo[activityType] = value;
    }

    public static CampaignSummary GetCampaignSummary(CampaignCollectionCriteria criteria)
    {
      return (CampaignSummary) null;
    }

    private CampaignSummary(CampaignSummaryInfo summaryInfo) => this.summaryInfo = summaryInfo;

    [Serializable]
    private class Criteria
    {
      public string UserId;
      public ContactType[] ContactTypes;
      public CampaignStatus[] CampaignStatuses;
      public ActivityType[] ActivityTypes;
      public ActivityStatus[] ActivityStatuses;
      public DateTime[] ActivityDateRange;

      public Criteria(
        string userId,
        ContactType[] contactTypes,
        CampaignStatus[] campaignStatuses,
        ActivityType[] activityTypes,
        ActivityStatus[] activityStatuses,
        DateTime[] activityDateRange)
      {
        this.UserId = userId;
        this.ContactTypes = contactTypes;
        this.CampaignStatuses = campaignStatuses;
        this.ActivityTypes = activityTypes;
        this.ActivityStatuses = activityStatuses;
        this.ActivityDateRange = activityDateRange;
      }
    }
  }
}
