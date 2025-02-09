// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignStep
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.TaskList;
using System;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignStep : BusinessBase, IDisposable
  {
    [NotUndoable]
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private CampaignStepInfo stepInfo;
    [NotUndoable]
    private CampaignActivityCollection campaignActivities;
    private int stepOffset;
    private bool isDueItemsOnly = true;
    [NotUndoable]
    private SessionObjects sessionObjects;

    public int CampaignStepId => this.stepInfo.CampaignStepId;

    public int CampaignId => this.stepInfo.CampaignId;

    public int StepNumber
    {
      get => this.stepInfo.StepNumber;
      set
      {
        if (this.stepInfo.StepNumber == value)
          return;
        this.stepInfo.StepNumber = value;
        this.MarkDirty();
      }
    }

    public string StepName
    {
      get => this.stepInfo.StepName;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.stepInfo.StepName != value))
          return;
        if (string.Compare(value, this.stepInfo.StepName, true) != 0 && this.campaign != null)
        {
          bool isBroken = false;
          foreach (CampaignStep campaignStep in (CollectionBase) this.campaign.CampaignSteps)
          {
            if (string.Compare(campaignStep.StepName, value, true) == 0)
            {
              isBroken = true;
              break;
            }
          }
          this.BrokenRules.Assert("CampaignStepNameAlreadyExists", "Campaign Step Name already exists in this campaign", isBroken);
        }
        this.stepInfo.StepName = value;
        this.BrokenRules.Assert("CampaignStepNameRequired", "Campaign Step Name is a required field", this.stepInfo.StepName.Length < 1);
        this.BrokenRules.Assert("CampaignStepNameLength", "Campaign Step Name exceeds 64 characters", this.stepInfo.StepName.Length > 64);
        this.MarkDirty();
      }
    }

    public string StepDesc
    {
      get => this.stepInfo.StepDesc;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.stepInfo.StepDesc != value))
          return;
        this.stepInfo.StepDesc = value;
        this.BrokenRules.Assert("CampaignStepDescriptionLength", "Campaign Step Description exceeds 250 characters", this.stepInfo.StepDesc.Length > 250);
        this.MarkDirty();
      }
    }

    public int StepOffset
    {
      get => this.stepOffset;
      set
      {
        if (this.stepOffset == value)
          return;
        this.stepOffset = value;
        this.BrokenRules.Assert("CampaignStepOffsetTooSmall", "Campaign Step Interval cannot be negitive", this.stepInfo.StepInterval < 0);
      }
    }

    public ActivityType ActivityType
    {
      get => this.stepInfo.ActivityType;
      set
      {
        if (this.stepInfo.ActivityType == value)
          return;
        this.stepInfo.ActivityType = value;
        if (this.stepInfo.ActivityType == ActivityType.Email || ActivityType.Fax == this.stepInfo.ActivityType || ActivityType.Letter == this.stepInfo.ActivityType)
          this.BrokenRules.Assert("DocumentRequired", "Please select a Document for this activity", this.stepInfo.DocumentId.Length < 1);
        else
          this.BrokenRules.Assert("DocumentRequired", "Please select a Document for this activity", false);
        this.MarkDirty();
      }
    }

    public string ActivityUserId
    {
      get => this.stepInfo.ActivityUserId;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.stepInfo.ActivityUserId != value))
          return;
        this.stepInfo.ActivityUserId = value;
        this.BrokenRules.Assert("CampaignStepUserIdLength", "Campaign Step UserId exceeds 16 characters", this.stepInfo.Subject.Length > 16);
        this.MarkDirty();
      }
    }

    public string DocumentId
    {
      get => this.stepInfo.DocumentId;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.stepInfo.DocumentId != value))
          return;
        this.stepInfo.DocumentId = value;
        if (this.stepInfo.ActivityType == ActivityType.Email || ActivityType.Letter == this.stepInfo.ActivityType)
          this.BrokenRules.Assert("DocumentRequired", "Please select a Document for this activity", this.stepInfo.DocumentId.Length < 1);
        else
          this.BrokenRules.Assert("DocumentRequired", "Please select a Document for this activity", false);
        this.BrokenRules.Assert("CampaignStepDocumentIdentifierLength", "Campaign Step Document Identifier exceeds 255 characters", this.stepInfo.DocumentId.Length > (int) byte.MaxValue);
        this.MarkDirty();
      }
    }

    public string Subject
    {
      get => this.stepInfo.Subject;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.stepInfo.Subject != value))
          return;
        this.stepInfo.Subject = value;
        this.BrokenRules.Assert("CampaignStepSubjectLength", "Campaign Step Subject exceeds 255 characters", this.stepInfo.Subject.Length > (int) byte.MaxValue);
        this.MarkDirty();
      }
    }

    public string Comments
    {
      get => this.stepInfo.Comments;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.stepInfo.Comments != value))
          return;
        this.stepInfo.Comments = value;
        this.BrokenRules.Assert("CampaignStepCommentsLength", "Campaign Step Comments exceeds 255 characters", this.stepInfo.Comments.Length > (int) byte.MaxValue);
        this.MarkDirty();
      }
    }

    public TaskPriority TaskPriority
    {
      get => this.stepInfo.TaskPriority;
      set
      {
        if (this.stepInfo.TaskPriority == value)
          return;
        this.stepInfo.TaskPriority = value;
        this.MarkDirty();
      }
    }

    public Color BarColor
    {
      get => this.stepInfo.BarColor;
      set
      {
        if (!(this.stepInfo.BarColor != value))
          return;
        this.stepInfo.BarColor = value;
        this.MarkDirty();
      }
    }

    public int TasksDueCount
    {
      get => this.stepInfo.TasksDueCount;
      set => this.stepInfo.TasksDueCount = value;
    }

    public DateTime LastActivityDate
    {
      get => this.stepInfo.LastActivityDate;
      set => this.stepInfo.LastActivityDate = value;
    }

    [XmlIgnore]
    public CampaignActivityCollection CampaignActivities
    {
      get => this.campaignActivities;
      set => this.campaignActivities = value;
    }

    public bool IsDueItemsOnly
    {
      get => this.isDueItemsOnly;
      set => this.isDueItemsOnly = value;
    }

    internal int StepInterval
    {
      get => this.stepInfo.StepInterval;
      set
      {
        if (this.stepInfo.StepInterval == value)
          return;
        this.stepInfo.StepInterval = value;
        this.MarkDirty();
      }
    }

    internal void GetActivity(CampaignActivityCollectionCriteria criteria)
    {
      if (!this.isDueItemsOnly)
        criteria.ActivityDateRange = (DateTime[]) null;
      this.SetInfo(this.sessionObjects.CampaignManager.GetCampaignStepActivity(criteria));
    }

    internal void UpdateActivity(CampaignActivityCollectionCriteria criteria, string activityNote)
    {
      int length = 0;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      ArrayList arrayList3 = new ArrayList();
      foreach (CampaignActivity campaignActivity in (CollectionBase) this.campaignActivities)
      {
        if (campaignActivity.IsSelected)
        {
          switch (campaignActivity.PendingStatus)
          {
            case ActivityStatus.Completed:
              if (arrayList2.Count == 0)
                ++length;
              arrayList2.Add((object) campaignActivity.ContactId);
              continue;
            case ActivityStatus.OptedOut:
              if (arrayList3.Count == 0)
                ++length;
              arrayList3.Add((object) campaignActivity.ContactId);
              continue;
            case ActivityStatus.Removed:
              if (arrayList1.Count == 0)
                ++length;
              arrayList1.Add((object) campaignActivity.ContactId);
              continue;
            default:
              continue;
          }
        }
      }
      if (length == 0)
        return;
      ActivityUpdateParameter activityUpdateParameter = new ActivityUpdateParameter(activityNote, new ActivityStatusParameter[length]);
      int num1 = 0;
      if (0 < arrayList1.Count)
        activityUpdateParameter.ActivityStatusParameters[num1++] = new ActivityStatusParameter(ActivityStatus.Removed, (int[]) arrayList1.ToArray(typeof (int)));
      if (0 < arrayList2.Count)
        activityUpdateParameter.ActivityStatusParameters[num1++] = new ActivityStatusParameter(ActivityStatus.Completed, (int[]) arrayList2.ToArray(typeof (int)));
      if (0 < arrayList3.Count)
      {
        ActivityStatusParameter[] statusParameters = activityUpdateParameter.ActivityStatusParameters;
        int index = num1;
        int num2 = index + 1;
        ActivityStatusParameter activityStatusParameter = new ActivityStatusParameter(ActivityStatus.OptedOut, (int[]) arrayList3.ToArray(typeof (int)));
        statusParameters[index] = activityStatusParameter;
      }
      if (!this.isDueItemsOnly)
        criteria.ActivityDateRange = (DateTime[]) null;
      this.SetInfo(this.sessionObjects.CampaignManager.UpdateCampaignStepActiviity(criteria, activityUpdateParameter));
    }

    internal void InvalidateActivity()
    {
      this.campaignActivities = (CampaignActivityCollection) null;
    }

    internal CampaignStepInfo GetInfo()
    {
      this.stepInfo.IsNew = this.IsNew;
      this.stepInfo.IsDirty = this.IsDirty;
      this.stepInfo.IsDeleted = this.IsDeleted;
      return this.stepInfo;
    }

    internal void SetInfo(CampaignStepInfo stepInfo)
    {
      if (stepInfo == null)
        return;
      this.stepInfo = stepInfo;
      this.campaignActivities = CampaignActivityCollection.NewCampaignActivityCollection();
      if (stepInfo.CampaignActivities == null)
        return;
      foreach (CampaignActivityInfo campaignActivity in stepInfo.CampaignActivities)
        this.campaignActivities.Add(CampaignActivity.NewCampaignActivity(campaignActivity));
    }

    public override string ToString()
    {
      return string.Format("CampaignStep[{0}]", (object) this.CampaignStepId);
    }

    public bool Equals(CampaignStep obj) => this.CampaignStepId.Equals(obj.CampaignStepId);

    public override int GetHashCode()
    {
      return string.Format("{0}-{1}", (object) this.CampaignId, (object) this.CampaignStepId).GetHashCode();
    }

    public override bool IsValid
    {
      get
      {
        if (!base.IsValid)
          return false;
        return this.campaignActivities == null || this.campaignActivities.IsValid;
      }
    }

    public override bool IsDirty
    {
      get
      {
        if (base.IsDirty)
          return true;
        return this.campaignActivities != null && this.campaignActivities.IsDirty;
      }
    }

    public static CampaignStep NewCampaignStep(EllieMae.EMLite.Campaign.Campaign campaign, SessionObjects sessionObjects)
    {
      return new CampaignStep(campaign, sessionObjects);
    }

    public static CampaignStep NewCampaignStep(
      EllieMae.EMLite.Campaign.Campaign campaign,
      CampaignStepInfo stepInfo,
      SessionObjects sessionObjects)
    {
      return new CampaignStep(campaign, stepInfo, sessionObjects);
    }

    public static CampaignStep NewCampaignStep(
      EllieMae.EMLite.Campaign.Campaign campaign,
      CampaignTemplate.CampaignStepTemplate stepTemplate,
      SessionObjects sessionObjects)
    {
      return new CampaignStep(campaign, stepTemplate, sessionObjects);
    }

    private CampaignStep(SessionObjects sessionObjects)
      : this((EllieMae.EMLite.Campaign.Campaign) null, sessionObjects)
    {
    }

    private CampaignStep(EllieMae.EMLite.Campaign.Campaign campaign, SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.MarkAsChild();
      this.campaign = campaign;
      this.stepInfo = new CampaignStepInfo(0, 0, 0, string.Empty, string.Empty, 0, ActivityType.Email, string.Empty, string.Empty, string.Empty, string.Empty, TaskPriority.Normal, Color.Empty, DateTime.MinValue, 0, (CampaignActivityInfo[]) null);
      this.campaignActivities = CampaignActivityCollection.NewCampaignActivityCollection();
      this.BrokenRules.Assert("CampaignStepNameRequired", "Campaign Step Name is a required field", this.stepInfo.StepName.Length < 1);
      this.BrokenRules.Assert("DocumentRequired", "Please select a Document for this activity", (this.stepInfo.ActivityType == ActivityType.Email || ActivityType.Letter == this.stepInfo.ActivityType) && this.stepInfo.DocumentId.Length < 1);
    }

    private CampaignStep(
      EllieMae.EMLite.Campaign.Campaign campaign,
      CampaignStepInfo stepInfo,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkOld();
      this.MarkAsChild();
      this.campaign = campaign;
      this.stepInfo = stepInfo;
      if (stepInfo.CampaignActivities == null)
        return;
      this.campaignActivities = CampaignActivityCollection.NewCampaignActivityCollection(stepInfo.CampaignActivities.Length);
      foreach (CampaignActivityInfo campaignActivity in stepInfo.CampaignActivities)
        this.campaignActivities.Add(CampaignActivity.NewCampaignActivity(campaignActivity));
    }

    private CampaignStep(
      EllieMae.EMLite.Campaign.Campaign campaign,
      CampaignTemplate.CampaignStepTemplate stepTemplate,
      SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.MarkAsChild();
      this.campaign = campaign;
      this.stepInfo = new CampaignStepInfo(0, 0, int.Parse(stepTemplate.GetField(nameof (StepNumber))), stepTemplate.GetField(nameof (StepName)), stepTemplate.GetField(nameof (StepDesc)), int.Parse(stepTemplate.GetField(nameof (StepInterval))), (ActivityType) new ActivityTypeNameProvider().GetValue(stepTemplate.GetField(nameof (ActivityType))), stepTemplate.GetField("UserId"), stepTemplate.GetField(nameof (DocumentId)), stepTemplate.GetField(nameof (Subject)), stepTemplate.GetField(nameof (Comments)), (TaskPriority) new TaskPriorityNameProvider().GetValue(stepTemplate.GetField(nameof (TaskPriority))), Color.FromArgb(int.Parse(stepTemplate.GetField(nameof (BarColor)))), DateTime.MinValue, 0, (CampaignActivityInfo[]) null);
      this.campaignActivities = CampaignActivityCollection.NewCampaignActivityCollection();
      this.BrokenRules.Assert("CampaignStepNameRequired", "Campaign Step Name is a required field", this.stepInfo.StepName.Length < 1);
      this.BrokenRules.Assert("DocumentRequired", "Please select a Document for this activity", (this.stepInfo.ActivityType == ActivityType.Email || ActivityType.Letter == this.stepInfo.ActivityType) && this.stepInfo.DocumentId.Length < 1);
    }

    public void Dispose()
    {
    }
  }
}
