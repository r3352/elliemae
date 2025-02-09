// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignActivityInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CampaignActivityInfo : IPropertyDictionary
  {
    public int CampaignStepId;
    public int ContactId;
    public DateTime CreatedDate;
    public DateTime ScheduledDate;
    public DateTime CompletedDate;
    public ActivityStatus Status;
    public Dictionary<string, object> ContactProperties = new Dictionary<string, object>();
    public bool IsNew;
    public bool IsDirty;
    public bool IsDeleted;

    public CampaignActivityInfo(
      int campaignStepId,
      int contactId,
      DateTime createdDate,
      DateTime scheduledDate,
      DateTime completedDate,
      ActivityStatus status)
    {
      this.CampaignStepId = campaignStepId;
      this.ContactId = contactId;
      this.CreatedDate = createdDate;
      this.ScheduledDate = scheduledDate;
      this.CompletedDate = completedDate;
      this.Status = status;
    }

    public object this[string propertyName]
    {
      get
      {
        switch (propertyName)
        {
          case "CampaignActivity.CampaignStepId":
            return (object) this.CampaignStepId;
          case "CampaignActivity.ContactId":
            return (object) this.ContactId;
          case "CampaignActivity.CreatedDate":
            return (object) this.CreatedDate;
          case "CampaignActivity.ScheduledDate":
            return (object) this.ScheduledDate;
          case "CampaignActivity.CompletedDate":
            return (object) this.CompletedDate;
          case "CampaignActivity.Status":
            return (object) this.Status;
          default:
            return this.ContactProperties != null ? this.ContactProperties[propertyName] : (object) null;
        }
      }
      set
      {
      }
    }
  }
}
