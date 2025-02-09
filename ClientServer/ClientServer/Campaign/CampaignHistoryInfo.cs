// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignHistoryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CampaignHistoryInfo
  {
    public int CampaignId;
    public int ContactId;
    public int StepNumber;
    public string StepName;
    public string ActivityType;
    public string ActivityStatus;
    public DateTime CompletedDate;
    public string ContactOwner;
    public int NoteId;
    public string LastName;
    public string FirstName;
    public bool IsNew;
    public bool IsDirty;
    public bool IsDeleted;

    public CampaignHistoryInfo()
    {
    }

    public CampaignHistoryInfo(
      int campaignId,
      int contactId,
      int stepNumber,
      string stepName,
      string activityType,
      string activityStatus,
      DateTime completedDate,
      string contactOwner,
      int noteId,
      string lastName,
      string firstName)
    {
      this.CampaignId = campaignId;
      this.ContactId = contactId;
      this.StepNumber = stepNumber;
      this.StepName = stepName;
      this.ActivityType = activityType;
      this.ActivityStatus = activityStatus;
      this.CompletedDate = completedDate;
      this.ContactOwner = contactOwner;
      this.NoteId = noteId;
      this.LastName = lastName;
      this.FirstName = firstName;
    }
  }
}
