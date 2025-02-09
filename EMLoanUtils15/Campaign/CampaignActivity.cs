// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignActivity
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Campaign;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignActivity : BusinessBase, IDisposable
  {
    private CampaignActivityInfo activityInfo;
    private bool isSelected;
    private ActivityStatus pendingStatus;

    public int CampaignStepId => this.activityInfo.CampaignStepId;

    public int ContactId => this.activityInfo.ContactId;

    public DateTime CreatedDate => this.activityInfo.CreatedDate;

    public DateTime ScheduledDate
    {
      get => this.activityInfo.ScheduledDate;
      set
      {
        if (!(this.activityInfo.ScheduledDate != value))
          return;
        this.activityInfo.ScheduledDate = value;
        this.MarkDirty();
      }
    }

    public DateTime CompletedDate => this.activityInfo.CompletedDate;

    public ActivityStatus Status
    {
      get => this.activityInfo.Status;
      set
      {
        if (this.activityInfo.Status == value)
          return;
        this.activityInfo.Status = value;
        this.MarkDirty();
      }
    }

    public Dictionary<string, object> ContactProperties => this.activityInfo.ContactProperties;

    public bool IsSelected
    {
      get => this.isSelected;
      set => this.isSelected = value;
    }

    public ActivityStatus PendingStatus
    {
      get => this.pendingStatus;
      set => this.pendingStatus = value;
    }

    internal CampaignActivityInfo GetInfo()
    {
      this.activityInfo.IsNew = this.IsNew;
      this.activityInfo.IsDirty = this.IsDirty;
      this.activityInfo.IsDeleted = this.IsDeleted;
      return this.activityInfo;
    }

    public override string ToString()
    {
      return string.Format("CampaignActivity[{0},{1}]", (object) this.CampaignStepId, (object) this.ContactId);
    }

    public bool Equals(CampaignActivity obj)
    {
      return this.CampaignStepId.Equals(obj.CampaignStepId) && this.ContactId.Equals(obj.ContactId);
    }

    public override int GetHashCode()
    {
      return string.Format("{0}-{1}", (object) this.CampaignStepId, (object) this.ContactId).GetHashCode();
    }

    public static CampaignActivity NewCampaignActivity() => new CampaignActivity();

    public static CampaignActivity NewCampaignActivity(CampaignActivityInfo activityInfo)
    {
      return new CampaignActivity(activityInfo);
    }

    public static CampaignActivity GetCampaignActivity(CampaignActivityInfo activityInfo)
    {
      throw new NotSupportedException("");
    }

    private CampaignActivity()
    {
      this.MarkNew();
      this.MarkAsChild();
      this.activityInfo = new CampaignActivityInfo(0, 0, DateTime.Today, DateTime.MinValue, DateTime.MinValue, ActivityStatus.Expected);
    }

    private CampaignActivity(CampaignActivityInfo activityInfo)
    {
      this.MarkOld();
      this.MarkAsChild();
      this.activityInfo = activityInfo;
    }

    public void Dispose()
    {
    }
  }
}
