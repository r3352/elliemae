// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CampaignData
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Campaign;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CampaignData
  {
    private static CampaignData campaignData;
    private string userId;
    private CampaignCollection campaigns;
    private EllieMae.EMLite.Campaign.Campaign activeCampaign;
    private CampaignStep activeCampaignStep;
    private EllieMae.EMLite.Campaign.Campaign wizardCampaign;
    private bool isNewCampaign;
    private EllieMae.EMLite.ContactGroup.ContactGroup wizardGroup;
    private FileSystemEntry templateSourceEntry;
    private FileSystemEntry templateTargetEntry;
    private CampaignCollectionCriteria campaignCollectionCriteria;

    private CampaignData()
    {
      string userId = Session.UserID;
      ContactType[] contactTypes = new ContactType[2]
      {
        ContactType.Borrower,
        ContactType.BizPartner
      };
      CampaignStatus[] campaignStatuses = new CampaignStatus[3]
      {
        CampaignStatus.NotStarted,
        CampaignStatus.Running,
        CampaignStatus.Stopped
      };
      ActivityType[] activityTypes = new ActivityType[5]
      {
        ActivityType.PhoneCall,
        ActivityType.Email,
        ActivityType.Letter,
        ActivityType.Reminder,
        ActivityType.Fax
      };
      ActivityStatus[] activityStatuses = new ActivityStatus[1]
      {
        ActivityStatus.Expected
      };
      DateTime[] activityDateRange = new DateTime[2];
      activityDateRange[0] = new DateTime(2000, 1, 1, 0, 0, 0);
      DateTime today = DateTime.Today;
      int year = today.Year;
      today = DateTime.Today;
      int month = today.Month;
      today = DateTime.Today;
      int day = today.Day;
      activityDateRange[1] = new DateTime(year, month, day, 23, 59, 59);
      this.campaignCollectionCriteria = new CampaignCollectionCriteria(userId, contactTypes, campaignStatuses, activityTypes, activityStatuses, activityDateRange);
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.LoadCampaignData();
    }

    public static CampaignData GetCampaignData()
    {
      if (CampaignData.campaignData == null)
        CampaignData.campaignData = new CampaignData();
      return CampaignData.campaignData;
    }

    public string UserId => this.userId;

    public CampaignCollection Campaigns => this.campaigns;

    public CampaignCollectionCriteria CampaignCollectionCriteria => this.campaignCollectionCriteria;

    public EllieMae.EMLite.Campaign.Campaign ActiveCampaign
    {
      get => this.activeCampaign;
      set
      {
        this.activeCampaign = value;
        this.activeCampaignStep = (CampaignStep) null;
        if (this.activeCampaign == null || this.activeCampaign.CampaignSteps == null || 0 >= this.activeCampaign.CampaignSteps.Count)
          return;
        this.activeCampaignStep = this.activeCampaign.CampaignSteps[0];
      }
    }

    public CampaignStep ActiveCampaignStep
    {
      get => this.activeCampaignStep;
      set
      {
        this.activeCampaignStep = value;
        this.activeCampaign = this.campaigns.Find(this.activeCampaignStep.CampaignId);
      }
    }

    public EllieMae.EMLite.Campaign.Campaign WizardCampaign
    {
      get => this.wizardCampaign;
      set => this.wizardCampaign = value;
    }

    public bool IsNewCampaign
    {
      get => this.isNewCampaign;
      set => this.isNewCampaign = value;
    }

    public EllieMae.EMLite.ContactGroup.ContactGroup WizardContactGroup
    {
      get => this.wizardGroup;
      set => this.wizardGroup = value;
    }

    public FileSystemEntry TemplateSourceEntry
    {
      get => this.templateSourceEntry;
      set => this.templateSourceEntry = value;
    }

    public FileSystemEntry TemplateTargetEntry
    {
      get => this.templateTargetEntry;
      set => this.templateTargetEntry = value;
    }

    public void LoadCampaignData()
    {
      this.campaigns = CampaignCollection.GetCampaignCollection(this.campaignCollectionCriteria, Session.SessionObjects);
      this.OnCampaignDataChangedEvent(EventArgs.Empty);
    }

    public void GetActivity(CampaignStep campaignStep)
    {
      this.activeCampaign.GetActivityForStep(new CampaignActivityCollectionCriteria(campaignStep.CampaignStepId, this.campaigns.Find(campaignStep.CampaignId).ContactType, this.campaignCollectionCriteria.ActivityStatuses, this.campaignCollectionCriteria.ActivityDateRange));
    }

    public void UpdateActivity(CampaignStep campaignStep, string activityNote)
    {
      this.activeCampaign.UpdateActivityForStep(new CampaignActivityCollectionCriteria(campaignStep.CampaignStepId, this.campaigns.Find(campaignStep.CampaignId).ContactType, this.campaignCollectionCriteria.ActivityStatuses, this.campaignCollectionCriteria.ActivityDateRange), activityNote);
      this.OnActivityUpdatedEvent(EventArgs.Empty);
    }

    public void UpdateContactList(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      campaign.UpdateContactList();
      this.OnActivityUpdatedEvent(EventArgs.Empty);
    }

    public void UpdateTotalTasksDue(int totalTasksDue)
    {
      this.OnTasksDueChangedEvent(new TasksDueEventArgs(totalTasksDue));
    }

    private int getTasksDueForUser() => Session.CampaignManager.GetTasksDueForUser(Session.UserID);

    public event ActivityUpdatedEventHandler ActivityUpdatedEvent;

    protected virtual void OnActivityUpdatedEvent(EventArgs e)
    {
      if (this.ActivityUpdatedEvent == null)
        return;
      this.ActivityUpdatedEvent((object) this, e);
    }

    public event CampaignDataChangedEventHandler CampaignDataChangedEvent;

    protected virtual void OnCampaignDataChangedEvent(EventArgs e)
    {
      if (this.CampaignDataChangedEvent == null)
        return;
      this.CampaignDataChangedEvent((object) this, e);
    }

    public event TasksDueChangedEventHandler TasksDueChangedEvent;

    protected virtual void OnTasksDueChangedEvent(TasksDueEventArgs e)
    {
      if (this.TasksDueChangedEvent == null)
        return;
      this.TasksDueChangedEvent((object) this, e);
    }
  }
}
