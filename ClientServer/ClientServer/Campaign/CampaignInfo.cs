// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ContactUI;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CampaignInfo : BusinessInfoBase
  {
    public int CampaignId;
    public string UserId;
    public string CampaignName;
    public string CampaignDesc;
    public CampaignType CampaignType;
    public ContactType ContactType;
    public CampaignStatus Status;
    public CampaignFrequencyType FrequencyType;
    public int FrequencyInterval;
    public int AddQueryId;
    public int DeleteQueryId;
    public int ContactGroupId;
    public DateTime CreationTime;
    public DateTime StartedTime;
    public DateTime NextRefreshTime;
    public int TotalContacts;
    public string CampaignVersion;
    [NotUndoable]
    public CampaignStepInfo[] CampaignSteps;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;
    [NotUndoable]
    public CampaignStepInfo[] AddedSteps;
    [NotUndoable]
    public CampaignStepInfo[] UpdatedSteps;
    [NotUndoable]
    public int[] DeletedStepIds;
    [NotUndoable]
    public ContactQueryInfo AddQueryInfo;
    [NotUndoable]
    public ContactQueryInfo DeleteQueryInfo;
    [NotUndoable]
    public ContactGroupInfo ContactGroupInfo;

    public CampaignInfo()
    {
    }

    public CampaignInfo(
      int campaignId,
      string userId,
      string campaignName,
      string campaignDesc,
      CampaignType campaignType,
      ContactType contactType,
      CampaignStatus status,
      CampaignFrequencyType frequencyType,
      int frequencyInterval,
      int addQueryId,
      int deleteQueryId,
      int contactGroupId,
      DateTime creationTime,
      DateTime startedTime,
      DateTime nextRefreshTime,
      int totalContacts,
      CampaignStepInfo[] campaignSteps,
      string campaignVersion)
    {
      this.CampaignId = campaignId;
      this.UserId = userId;
      this.CampaignName = campaignName;
      this.CampaignDesc = campaignDesc;
      this.CampaignType = campaignType;
      this.ContactType = contactType;
      this.Status = status;
      this.FrequencyType = frequencyType;
      this.FrequencyInterval = frequencyInterval;
      this.AddQueryId = addQueryId;
      this.DeleteQueryId = deleteQueryId;
      this.ContactGroupId = contactGroupId;
      this.CreationTime = creationTime;
      this.StartedTime = startedTime;
      this.NextRefreshTime = nextRefreshTime;
      this.TotalContacts = totalContacts;
      this.CampaignSteps = campaignSteps;
      this.CampaignVersion = campaignVersion;
    }
  }
}
