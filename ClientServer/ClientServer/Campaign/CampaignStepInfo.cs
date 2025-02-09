// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignStepInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BizLayer;
using EllieMae.EMLite.ClientServer.TaskList;
using System;
using System.Drawing;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CampaignStepInfo : BusinessInfoBase
  {
    public int CampaignStepId;
    public int CampaignId;
    public int StepNumber;
    public string StepName;
    public string StepDesc;
    public int StepInterval;
    public ActivityType ActivityType;
    public string ActivityUserId;
    public string DocumentId;
    public string Subject;
    public string Comments;
    public TaskPriority TaskPriority;
    public Color BarColor;
    public DateTime LastActivityDate;
    public int TasksDueCount;
    [NotUndoable]
    public CampaignActivityInfo[] CampaignActivities;
    [NotUndoable]
    public bool IsNew;
    [NotUndoable]
    public bool IsDirty;
    [NotUndoable]
    public bool IsDeleted;
    [NotUndoable]
    public CampaignActivityInfo[] AddedActivities;
    [NotUndoable]
    public CampaignActivityInfo[] UpdatedActivities;
    [NotUndoable]
    public int[] DeletedActivityIds;

    public CampaignStepInfo()
    {
    }

    public CampaignStepInfo(
      int campaignStepId,
      int campaignId,
      int stepNumber,
      string stepName,
      string stepDesc,
      int stepInterval,
      ActivityType activityType,
      string activityUserId,
      string documentId,
      string subject,
      string comments,
      TaskPriority taskPriority,
      Color barColor,
      DateTime lastActivityDate,
      int tasksDueCount,
      CampaignActivityInfo[] campaignActivities)
    {
      this.CampaignStepId = campaignStepId;
      this.CampaignId = campaignId;
      this.StepNumber = stepNumber;
      this.StepName = stepName;
      this.StepDesc = stepDesc;
      this.StepInterval = stepInterval;
      this.ActivityType = activityType;
      this.ActivityUserId = activityUserId;
      this.DocumentId = documentId;
      this.Subject = subject;
      this.Comments = comments;
      this.TaskPriority = taskPriority;
      this.BarColor = barColor;
      this.LastActivityDate = lastActivityDate;
      this.TasksDueCount = tasksDueCount;
      this.CampaignActivities = campaignActivities;
    }
  }
}
