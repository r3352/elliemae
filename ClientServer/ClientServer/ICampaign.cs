// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICampaign
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.Query;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICampaign
  {
    int[] GetCampaignQueryListForUser(string userId);

    CampaignInfo RunCampaignQueries(int campaignId);

    CampaignInfo[] GetCampaignsForUser(CampaignCollectionCriteria criteria);

    CampaignInfo GetCampaign(int campaignId);

    CampaignInfo GetCampaignFromStep(int campaignStepID);

    CampaignInfo SaveCampaign(CampaignInfo campaignInfo);

    void DeleteCampaign(int campaignId);

    void CopyCampaign(
      int oldCampaignId,
      bool copyContacts,
      string newCampaignName,
      string newCampaignDesc);

    CampaignInfo StartCampaign(int campaignId);

    CampaignInfo StopCampaign(int campaignId);

    CampaignContactInfo[] GetCampaignContacts(CampaignContactCollectionCritera criteria);

    CampaignInfo UpdateCampaignContacts(int campaignId, CrudRequestParameter[] crudRequests);

    CampaignTasksDueInfo[] GetCampaignsTasksDue();

    int GetTasksDueForUser(string userId);

    CampaignStepInfo GetCampaignStepActivity(CampaignActivityCollectionCriteria criteria);

    CampaignStepInfo UpdateCampaignStepActiviity(
      CampaignActivityCollectionCriteria criteria,
      ActivityUpdateParameter activityUpdateParameter);

    CampaignStepInfo GetCampaignStepInfo(int campaignStepID);

    ICursor OpenCampaignContactCursor(CampaignContactCollectionCritera criteria);

    ICursor OpenCampaignHistoryCursor(
      CampaignHistoryCollectionCritera criteria,
      SortField[] sortFields);
  }
}
