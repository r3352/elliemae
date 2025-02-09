// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.Campaign
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ContactGroup;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class Campaign : BusinessBase, IDisposable
  {
    private CampaignInfo campaignInfo;
    private CampaignStepCollection campaignSteps;
    private EllieMae.EMLite.ContactGroup.ContactGroup contactGroup;
    private ContactQuery addQuery;
    private ContactQuery deleteQuery;
    [NotUndoable]
    private SessionObjects sessionObjects;
    [NotUndoable]
    private CampaignContactCollection campaignContacts;

    public int CampaignId => this.campaignInfo.CampaignId;

    public string UserId => this.campaignInfo.UserId;

    public string CampaignName
    {
      get => this.campaignInfo.CampaignName;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.campaignInfo.CampaignName != value))
          return;
        this.campaignInfo.CampaignName = value;
        this.BrokenRules.Assert("CampaignNameRequired", "Campaign Name is a required field", this.campaignInfo.CampaignName.Length < 1);
        this.BrokenRules.Assert("CampaignNameLength", "Campaign Name exceeds 64 characters", this.campaignInfo.CampaignName.Length > 64);
        this.MarkDirty();
      }
    }

    public string CampaignDesc
    {
      get => this.campaignInfo.CampaignDesc;
      set
      {
        value = value != null ? value.Trim() : string.Empty;
        if (!(this.campaignInfo.CampaignDesc != value))
          return;
        this.campaignInfo.CampaignDesc = value;
        this.BrokenRules.Assert("CampaignDescriptionLength", "Campaign Description exceeds 250 characters", this.campaignInfo.CampaignDesc.Length > 250);
        this.MarkDirty();
      }
    }

    public CampaignType CampaignType
    {
      get => this.campaignInfo.CampaignType;
      set
      {
        if (this.campaignInfo.CampaignType == value)
          return;
        if (this.IsNew)
        {
          if ((CampaignType.Manual & value) != (CampaignType.Manual & this.campaignInfo.CampaignType))
          {
            this.addQuery = (ContactQuery) null;
            this.deleteQuery = (ContactQuery) null;
            this.contactGroup = (EllieMae.EMLite.ContactGroup.ContactGroup) null;
          }
          else if ((CampaignType.AutoDeleteQuery & value) != CampaignType.AutoDeleteQuery)
            this.deleteQuery = (ContactQuery) null;
        }
        this.campaignInfo.CampaignType = value;
        this.MarkDirty();
      }
    }

    public ContactType ContactType
    {
      get => this.campaignInfo.ContactType;
      set
      {
        if (this.campaignInfo.ContactType == value || !this.IsNew)
          return;
        this.campaignInfo.ContactType = value;
        this.addQuery = (ContactQuery) null;
        this.deleteQuery = (ContactQuery) null;
        this.contactGroup = (EllieMae.EMLite.ContactGroup.ContactGroup) null;
        this.MarkDirty();
      }
    }

    public CampaignStatus Status
    {
      get => this.campaignInfo.Status;
      set
      {
        if (this.campaignInfo.Status == value)
          return;
        this.campaignInfo.Status = value;
        this.MarkDirty();
      }
    }

    public CampaignFrequencyType FrequencyType
    {
      get => this.campaignInfo.FrequencyType;
      set
      {
        if (this.campaignInfo.FrequencyType == value)
          return;
        this.campaignInfo.FrequencyType = value;
        this.BrokenRules.Assert("CampaignFrequency", "Custom frequency interval can not be zero days", CampaignFrequencyType.Custom == this.campaignInfo.FrequencyType && this.campaignInfo.FrequencyInterval <= 0);
        this.MarkDirty();
      }
    }

    public int FrequencyInterval
    {
      get => this.campaignInfo.FrequencyInterval;
      set
      {
        if (this.campaignInfo.FrequencyInterval == value)
          return;
        this.campaignInfo.FrequencyInterval = value;
        this.BrokenRules.Assert("CampaignFrequency", "Custom frequency interval can not be zero days", CampaignFrequencyType.Custom == this.campaignInfo.FrequencyType && this.campaignInfo.FrequencyInterval <= 0);
        this.MarkDirty();
      }
    }

    public int AddQueryId => this.campaignInfo.AddQueryId;

    public ContactQuery AddQuery
    {
      get
      {
        if (this.addQuery == null)
        {
          if (this.campaignInfo.AddQueryId == 0)
          {
            this.addQuery = ContactQuery.NewContactQuery(this.ContactType, ContactQueryType.CampaignPredefinedQuery | ContactQueryType.CampaignAddQuery, this.sessionObjects);
            this.addQuery.QueryName = Guid.NewGuid().ToString();
          }
          else
            this.addQuery = ContactQuery.GetContactQuery(this.campaignInfo.AddQueryId, this.sessionObjects);
        }
        return this.addQuery;
      }
      set => this.addQuery = value;
    }

    public int DeleteQueryId => this.campaignInfo.DeleteQueryId;

    public ContactQuery DeleteQuery
    {
      get
      {
        if (this.deleteQuery == null)
        {
          if (this.campaignInfo.DeleteQueryId == 0)
          {
            this.deleteQuery = ContactQuery.NewContactQuery(this.ContactType, ContactQueryType.CampaignPredefinedQuery | ContactQueryType.CampaignDeleteQuery, this.sessionObjects);
            this.deleteQuery.QueryName = Guid.NewGuid().ToString();
          }
          else
            this.deleteQuery = ContactQuery.GetContactQuery(this.campaignInfo.DeleteQueryId, this.sessionObjects);
        }
        return this.deleteQuery;
      }
      set => this.deleteQuery = value;
    }

    public int ContactGroupId => this.campaignInfo.ContactGroupId;

    public int ContactGroupCount
    {
      get => this.contactGroup == null ? 0 : this.contactGroup.GroupMembers.Count;
    }

    [XmlIgnore]
    public EllieMae.EMLite.ContactGroup.ContactGroup ContactGroup
    {
      get
      {
        if (this.contactGroup == null)
        {
          if (this.campaignInfo.ContactGroupId == 0)
          {
            this.contactGroup = EllieMae.EMLite.ContactGroup.ContactGroup.NewContactGroup(this.ContactType, ContactGroupType.CampaignGroup, this.sessionObjects);
            this.contactGroup.GroupName = Guid.NewGuid().ToString();
          }
          else
            this.contactGroup = EllieMae.EMLite.ContactGroup.ContactGroup.GetContactGroup(this.campaignInfo.ContactGroupId, this.sessionObjects);
        }
        return this.contactGroup;
      }
      set
      {
        if (value != null || this.campaignInfo.ContactGroupId != 0 || this.contactGroup == value)
          return;
        this.contactGroup = (EllieMae.EMLite.ContactGroup.ContactGroup) null;
      }
    }

    public DateTime CreationTime => this.campaignInfo.CreationTime;

    public DateTime StartedTime => this.campaignInfo.StartedTime;

    public DateTime NextRefreshTime => this.campaignInfo.NextRefreshTime;

    public DateTime LastActivityDate
    {
      get
      {
        DateTime lastActivityDate = DateTime.MinValue;
        foreach (CampaignStep campaignStep in (CollectionBase) this.campaignSteps)
        {
          if (lastActivityDate < campaignStep.LastActivityDate)
            lastActivityDate = campaignStep.LastActivityDate;
        }
        return lastActivityDate;
      }
    }

    public int TasksDue => this.calculateTasksDue();

    public int CampaignStepCount => this.campaignSteps == null ? 0 : this.campaignSteps.Count;

    public CampaignStepCollection CampaignSteps
    {
      get
      {
        if (this.campaignSteps == null && this.IsNew)
          this.campaignSteps = CampaignStepCollection.NewCampaignStepCollection();
        return this.campaignSteps;
      }
      set
      {
        if (value != null || !this.IsNew || this.campaignSteps == value)
          return;
        this.campaignSteps = (CampaignStepCollection) null;
      }
    }

    [XmlIgnore]
    public CampaignContactCollection CampaignContacts
    {
      get
      {
        if (this.Status != CampaignStatus.Running && CampaignStatus.Stopped != this.Status)
          return (CampaignContactCollection) null;
        if (this.campaignContacts == null)
          this.getCampaignContacts();
        return this.campaignContacts;
      }
    }

    public void GetActivityForStep(CampaignActivityCollectionCriteria criteria)
    {
      this.campaignSteps.Find(criteria.CampaignStepId).GetActivity(criteria);
      this.campaignContacts = (CampaignContactCollection) null;
    }

    public void UpdateActivityForStep(
      CampaignActivityCollectionCriteria criteria,
      string activityNote)
    {
      this.campaignSteps.Find(criteria.CampaignStepId).UpdateActivity(criteria, activityNote);
      this.campaignContacts = (CampaignContactCollection) null;
    }

    public void UpdateContactList()
    {
      if (this.Status != CampaignStatus.Running && CampaignStatus.Stopped != this.Status || this.campaignContacts == null || !this.campaignContacts.IsDirty)
        return;
      int length = 0;
      int[] contactIds1 = new int[this.campaignContacts.DeletedCount];
      if (contactIds1.Length != 0)
      {
        ++length;
        int num = 0;
        foreach (CampaignContact deleted in (CollectionBase) this.campaignContacts.deletedList)
          contactIds1[num++] = deleted.ContactId;
      }
      int[] contactIds2 = new int[this.campaignContacts.AddedCount];
      if (contactIds2.Length != 0)
      {
        ++length;
        int num = 0;
        foreach (CampaignContact campaignContact in (CollectionBase) this.campaignContacts)
        {
          if (campaignContact.IsNew)
            contactIds2[num++] = campaignContact.ContactId;
        }
      }
      ArrayList arrayList = new ArrayList();
      foreach (CampaignContact campaignContact in (CollectionBase) this.campaignContacts)
      {
        if (campaignContact.IsReinsert)
          arrayList.Add((object) campaignContact.ContactId);
      }
      if (0 < arrayList.Count)
        ++length;
      CrudRequestParameter[] crudRequests = new CrudRequestParameter[length];
      int num1 = 0;
      if (contactIds1.Length != 0)
        crudRequests[num1++] = new CrudRequestParameter(CrudAction.Delete, contactIds1);
      if (contactIds2.Length != 0)
        crudRequests[num1++] = new CrudRequestParameter(CrudAction.Create, contactIds2);
      if (0 < arrayList.Count)
      {
        CrudRequestParameter[] requestParameterArray = crudRequests;
        int index = num1;
        int num2 = index + 1;
        CrudRequestParameter requestParameter = new CrudRequestParameter(CrudAction.Update, (int[]) arrayList.ToArray(typeof (int)));
        requestParameterArray[index] = requestParameter;
      }
      this.SetInfo(this.sessionObjects.CampaignManager.UpdateCampaignContacts(this.CampaignId, crudRequests));
    }

    public EllieMae.EMLite.Campaign.Campaign Save(string userId)
    {
      if (this.campaignInfo.UserId != userId)
      {
        this.campaignInfo.UserId = userId;
        Guid guid;
        if (this.addQuery != null)
        {
          this.addQuery.GetInfo().UserId = userId;
          ContactQuery addQuery = this.addQuery;
          guid = Guid.NewGuid();
          string str = guid.ToString();
          addQuery.QueryName = str;
        }
        if (this.DeleteQuery != null)
        {
          this.deleteQuery.GetInfo().UserId = userId;
          ContactQuery deleteQuery = this.deleteQuery;
          guid = Guid.NewGuid();
          string str = guid.ToString();
          deleteQuery.QueryName = str;
        }
        if (this.contactGroup != null)
        {
          this.contactGroup.GetInfo().UserId = userId;
          EllieMae.EMLite.ContactGroup.ContactGroup contactGroup = this.contactGroup;
          guid = Guid.NewGuid();
          string str = guid.ToString();
          contactGroup.GroupName = str;
        }
      }
      return (EllieMae.EMLite.Campaign.Campaign) this.Save();
    }

    public void Start()
    {
      if (CampaignStatus.NotStarted != this.Status && CampaignStatus.Stopped != this.Status)
        return;
      this.SetInfo(this.sessionObjects.CampaignManager.StartCampaign(this.CampaignId));
    }

    public void Stop()
    {
      if (this.Status != CampaignStatus.Running)
        return;
      this.SetInfo(this.sessionObjects.CampaignManager.StopCampaign(this.CampaignId));
    }

    public void RunQueries()
    {
      this.SetInfo(this.sessionObjects.CampaignManager.RunCampaignQueries(this.CampaignId));
    }

    public void Refresh()
    {
      this.SetInfo(this.sessionObjects.CampaignManager.GetCampaign(this.CampaignId));
    }

    public void ConvertRelativeStepOffsetToFixedStepInterval()
    {
      int num = 0;
      for (int index = 0; index < this.CampaignSteps.Count; ++index)
      {
        num += this.CampaignSteps[index].StepOffset;
        this.CampaignSteps[index].StepInterval = num;
      }
    }

    public void ConvertFixedStepIntervalToRelativeStepOffset()
    {
      int num = 0;
      for (int index = 0; index < this.CampaignSteps.Count; ++index)
      {
        this.CampaignSteps[index].StepOffset = this.CampaignSteps[index].StepInterval - num;
        num = this.CampaignSteps[index].StepInterval;
      }
    }

    public CampaignTemplate GetCampaignTemplate()
    {
      this.preTemplateCreation();
      return new CampaignTemplate(this.campaignInfo);
    }

    internal CampaignInfo GetInfo()
    {
      this.campaignInfo.IsNew = this.IsNew;
      this.campaignInfo.IsDirty = this.IsDirty;
      this.campaignInfo.IsDeleted = this.IsDeleted;
      return this.campaignInfo;
    }

    internal void SetInfo(CampaignInfo campaignInfo)
    {
      if (campaignInfo == null)
        return;
      this.campaignInfo = campaignInfo;
      this.campaignSteps = CampaignStepCollection.NewCampaignStepCollection();
      if (campaignInfo.CampaignSteps != null)
      {
        foreach (CampaignStepInfo campaignStep in campaignInfo.CampaignSteps)
          this.campaignSteps.Add(CampaignStep.NewCampaignStep(this, campaignStep, this.sessionObjects));
      }
      this.ConvertFixedStepIntervalToRelativeStepOffset();
      this.contactGroup = (EllieMae.EMLite.ContactGroup.ContactGroup) null;
      if ((ContactGroupInfo) null != campaignInfo.ContactGroupInfo)
        this.contactGroup = EllieMae.EMLite.ContactGroup.ContactGroup.NewContactGroup(campaignInfo.ContactGroupInfo, this.sessionObjects);
      this.addQuery = (ContactQuery) null;
      if (campaignInfo.AddQueryInfo != null)
        this.addQuery = ContactQuery.NewContactQuery(campaignInfo.AddQueryInfo);
      this.deleteQuery = (ContactQuery) null;
      if (campaignInfo.DeleteQueryInfo != null)
        this.deleteQuery = ContactQuery.NewContactQuery(campaignInfo.DeleteQueryInfo);
      this.campaignContacts = (CampaignContactCollection) null;
    }

    private void getCampaignContacts()
    {
      if (this.Status != CampaignStatus.Running && CampaignStatus.Stopped != this.Status)
        return;
      CampaignContactInfo[] campaignContacts = this.sessionObjects.CampaignManager.GetCampaignContacts(new CampaignContactCollectionCritera(this.CampaignId)
      {
        FieldList = new string[2]
        {
          "Contact.ContactID",
          "CampaignActivity.Activity"
        },
        SortFields = new SortField[1]
        {
          new SortField("Contact.ContactID", FieldSortOrder.Ascending)
        }
      });
      int length = campaignContacts != null ? campaignContacts.Length : 0;
      this.campaignContacts = CampaignContactCollection.NewCampaignContactCollection(length);
      for (int index = 0; index < length; ++index)
        this.campaignContacts.Add(CampaignContact.NewCampaignContact(campaignContacts[index]));
    }

    private int calculateTasksDue()
    {
      int tasksDue = 0;
      foreach (CampaignStep campaignStep in (CollectionBase) this.campaignSteps)
        tasksDue += campaignStep.TasksDueCount;
      return tasksDue;
    }

    private void preTemplateCreation()
    {
      this.campaignInfo.AddQueryInfo = (ContactQueryInfo) null;
      if (this.addQuery != null && !this.addQuery.IsDeleted)
        this.campaignInfo.AddQueryInfo = this.addQuery.GetInfo();
      this.campaignInfo.DeleteQueryInfo = (ContactQueryInfo) null;
      if (this.deleteQuery != null && !this.deleteQuery.IsDeleted)
        this.campaignInfo.DeleteQueryInfo = this.deleteQuery.GetInfo();
      this.ConvertRelativeStepOffsetToFixedStepInterval();
      List<CampaignStepInfo> campaignStepInfoList = new List<CampaignStepInfo>();
      foreach (CampaignStep campaignStep in (CollectionBase) this.CampaignSteps)
      {
        if (!campaignStep.IsDeleted)
          campaignStepInfoList.Add(campaignStep.GetInfo());
      }
      this.campaignInfo.CampaignSteps = campaignStepInfoList.ToArray();
    }

    public override string ToString() => string.Format("Campaign[{0}]", (object) this.CampaignId);

    public bool Equals(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      return this.CampaignId.Equals(campaign.CampaignId);
    }

    public new static bool Equals(object objA, object objB)
    {
      return objA is EllieMae.EMLite.Campaign.Campaign && objB is EllieMae.EMLite.Campaign.Campaign && ((EllieMae.EMLite.Campaign.Campaign) objA).Equals((EllieMae.EMLite.Campaign.Campaign) objB);
    }

    public override bool Equals(object obj) => obj is EllieMae.EMLite.Campaign.Campaign && this.Equals((EllieMae.EMLite.Campaign.Campaign) obj);

    public override int GetHashCode()
    {
      return string.Format("{0}-{1}", (object) this.CampaignId, (object) 0).GetHashCode();
    }

    public override bool IsValid
    {
      get
      {
        this.BrokenRules.Assert("CampaignSteps", "Campaign must contain at least one step", this.campaignSteps != null && this.campaignSteps.Count == 0);
        if (!base.IsValid)
          return false;
        return this.campaignSteps == null || this.campaignSteps.IsValid;
      }
    }

    public override bool IsDirty
    {
      get
      {
        if (base.IsDirty || this.campaignSteps != null && this.campaignSteps.IsDirty || this.contactGroup != null && this.contactGroup.IsDirty || this.addQuery != null && this.addQuery.IsDirty)
          return true;
        return this.deleteQuery != null && this.deleteQuery.IsDirty;
      }
    }

    public override BusinessBase Save()
    {
      if (this.IsDeleted)
      {
        if (!this.IsNew)
          this.sessionObjects.CampaignManager.DeleteCampaign(this.CampaignId);
        this.MarkNew();
      }
      else
      {
        this.ConvertRelativeStepOffsetToFixedStepInterval();
        if (this.IsDirty)
        {
          this.campaignInfo.IsNew = this.IsNew;
          this.campaignInfo.IsDirty = this.IsDirty;
          this.campaignInfo.IsDeleted = this.IsDeleted;
          this.campaignInfo.AddQueryInfo = (ContactQueryInfo) null;
          if (this.addQuery != null)
          {
            ContactQueryInfo info = this.addQuery.GetInfo();
            if (this.AddQueryId == 0 && info.IsNew)
            {
              if (string.Empty != info.XmlQueryString)
                this.campaignInfo.AddQueryInfo = info;
            }
            else if (this.AddQueryId != 0 && info.IsDirty)
            {
              if (string.Empty != info.XmlQueryString)
                this.campaignInfo.AddQueryInfo = info;
            }
            else if (this.AddQueryId != 0 && info.IsDeleted)
              this.campaignInfo.AddQueryInfo = info;
          }
          this.campaignInfo.DeleteQueryInfo = (ContactQueryInfo) null;
          if (this.deleteQuery != null)
          {
            ContactQueryInfo info = this.deleteQuery.GetInfo();
            if (this.DeleteQueryId == 0 && info.IsNew)
            {
              if (string.Empty != info.XmlQueryString)
                this.campaignInfo.DeleteQueryInfo = info;
            }
            else if (this.DeleteQueryId != 0 && info.IsDirty)
            {
              if (string.Empty != info.XmlQueryString)
                this.campaignInfo.DeleteQueryInfo = info;
            }
            else if (this.DeleteQueryId != 0 && info.IsDeleted)
              this.campaignInfo.DeleteQueryInfo = info;
          }
          this.campaignInfo.ContactGroupInfo = (ContactGroupInfo) null;
          if (this.contactGroup != null && CampaignStatus.NotStarted == this.Status)
          {
            ContactGroupInfo info = this.contactGroup.GetInfo();
            if (this.ContactGroupId == 0 && (CampaignType.Manual & this.CampaignType) == CampaignType.Manual)
            {
              if (info.IsNew)
                this.campaignInfo.ContactGroupInfo = info;
            }
            else if (this.ContactGroupId != 0 && (CampaignType.Manual & this.CampaignType) == CampaignType.Manual && info.IsDirty)
              this.campaignInfo.ContactGroupInfo = info;
          }
          this.campaignInfo.CampaignSteps = (CampaignStepInfo[]) null;
          this.campaignInfo.AddedSteps = new CampaignStepInfo[this.CampaignSteps.AddedCount];
          this.campaignInfo.UpdatedSteps = new CampaignStepInfo[this.CampaignSteps.UpdatedCount];
          this.campaignInfo.DeletedStepIds = new int[this.CampaignSteps.DeletedCount];
          int num1 = 0;
          int num2 = 0;
          foreach (CampaignStep campaignStep in (CollectionBase) this.CampaignSteps)
          {
            if (campaignStep.IsNew)
              this.campaignInfo.AddedSteps[num1++] = campaignStep.GetInfo();
            else if (campaignStep.IsDirty)
              this.campaignInfo.UpdatedSteps[num2++] = campaignStep.GetInfo();
          }
          int num3 = 0;
          foreach (CampaignStep deleted in (CollectionBase) this.CampaignSteps.deletedList)
            this.campaignInfo.DeletedStepIds[num3++] = deleted.CampaignStepId;
          this.SetInfo(this.sessionObjects.CampaignManager.SaveCampaign(this.campaignInfo));
          this.MarkOld();
        }
      }
      return (BusinessBase) this;
    }

    public static EllieMae.EMLite.Campaign.Campaign NewCampaign(SessionObjects sessionObjects)
    {
      return new EllieMae.EMLite.Campaign.Campaign(sessionObjects);
    }

    public static EllieMae.EMLite.Campaign.Campaign NewCampaign(
      CampaignInfo campaignInfo,
      SessionObjects sessionObjects)
    {
      return new EllieMae.EMLite.Campaign.Campaign(campaignInfo, sessionObjects);
    }

    public static EllieMae.EMLite.Campaign.Campaign NewCampaign(
      CampaignTemplate campaignTemplate,
      SessionObjects sessionObjects)
    {
      return new EllieMae.EMLite.Campaign.Campaign(campaignTemplate, sessionObjects);
    }

    public static EllieMae.EMLite.Campaign.Campaign GetCampaign(
      int campaignId,
      SessionObjects sessionObjects)
    {
      return new EllieMae.EMLite.Campaign.Campaign(sessionObjects.CampaignManager.GetCampaign(campaignId), sessionObjects);
    }

    public static void DeleteCampaign(int campaignId, SessionObjects sessionObjects)
    {
      if (0 >= campaignId)
        return;
      sessionObjects.CampaignManager.DeleteCampaign(campaignId);
    }

    public static void CopyCampaign(
      int oldCampaignId,
      bool copyContacts,
      string newCampaignName,
      string newCampaignDesc,
      SessionObjects sessionObjects)
    {
      if (0 >= oldCampaignId || newCampaignName == null || !(string.Empty != newCampaignName))
        return;
      sessionObjects.CampaignManager.CopyCampaign(oldCampaignId, copyContacts, newCampaignName, newCampaignDesc);
    }

    private Campaign(SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.campaignInfo = new CampaignInfo(0, this.sessionObjects.UserID, string.Empty, string.Empty, CampaignType.Manual, ContactType.Borrower, CampaignStatus.NotStarted, CampaignFrequencyType.Daily, 0, 0, 0, 0, DateTime.Today, DateTime.MinValue, DateTime.MaxValue, 0, (CampaignStepInfo[]) null, "4.0");
      this.BrokenRules.Assert("CampaignNameRequired", "Campaign Name is a required field", this.campaignInfo.CampaignName.Length < 1);
    }

    private Campaign(CampaignInfo campaignInfo, SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkOld();
      this.campaignInfo = campaignInfo;
      if (campaignInfo.CampaignSteps == null)
        return;
      this.campaignSteps = CampaignStepCollection.NewCampaignStepCollection(campaignInfo.CampaignSteps.Length);
      foreach (CampaignStepInfo campaignStep in campaignInfo.CampaignSteps)
        this.campaignSteps.Add(CampaignStep.NewCampaignStep(this, campaignStep, this.sessionObjects));
      this.campaignSteps.Sort("StepNumber", true);
      this.ConvertFixedStepIntervalToRelativeStepOffset();
    }

    private Campaign(CampaignTemplate campaignTemplate, SessionObjects sessionObjects)
    {
      this.sessionObjects = sessionObjects;
      this.MarkNew();
      this.campaignInfo = new CampaignInfo(0, this.sessionObjects.UserID, campaignTemplate.GetField(nameof (CampaignName)), campaignTemplate.GetField(nameof (CampaignDesc)), (CampaignType) Enum.Parse(typeof (CampaignType), campaignTemplate.GetField(nameof (CampaignType))), (ContactType) new ContactTypeNameProvider().GetValue(campaignTemplate.GetField(nameof (ContactType))), CampaignStatus.NotStarted, (CampaignFrequencyType) new CampaignFrequencyNameProvider().GetValue(campaignTemplate.GetField(nameof (FrequencyType))), int.Parse(campaignTemplate.GetField(nameof (FrequencyInterval))), 0, 0, 0, DateTime.Today, DateTime.MinValue, DateTime.MaxValue, 0, (CampaignStepInfo[]) null, campaignTemplate.TemplateVersion);
      this.BrokenRules.Assert("CampaignNameRequired", "Campaign Name is a required field", this.campaignInfo.CampaignName.Length < 1);
      if (CampaignType.AutoAddQuery == (CampaignType.AutoAddQuery & this.campaignInfo.CampaignType))
      {
        this.addQuery = ContactQuery.NewContactQuery(campaignTemplate.AddQueryTemplate, this.sessionObjects);
        this.campaignInfo.AddQueryInfo = this.addQuery.GetInfo();
      }
      if (CampaignType.AutoDeleteQuery == (CampaignType.AutoDeleteQuery & this.campaignInfo.CampaignType))
      {
        this.deleteQuery = ContactQuery.NewContactQuery(campaignTemplate.DeleteQueryTemplate, this.sessionObjects);
        this.campaignInfo.DeleteQueryInfo = this.deleteQuery.GetInfo();
      }
      CampaignTemplate.CampaignStepTemplateList campaignStepTemplates = campaignTemplate.CampaignStepTemplates;
      this.campaignSteps = CampaignStepCollection.NewCampaignStepCollection(campaignStepTemplates.Count);
      this.campaignInfo.CampaignSteps = new CampaignStepInfo[campaignStepTemplates.Count];
      int num = 0;
      foreach (CampaignTemplate.CampaignStepTemplate stepTemplate in (ArrayList) campaignStepTemplates)
      {
        CampaignStep campaignStep = CampaignStep.NewCampaignStep(this, stepTemplate, this.sessionObjects);
        this.campaignSteps.Add(campaignStep);
        this.campaignInfo.CampaignSteps[num++] = campaignStep.GetInfo();
      }
      this.campaignSteps.Sort("StepNumber", true);
      this.ConvertFixedStepIntervalToRelativeStepOffset();
    }

    public void Dispose()
    {
    }
  }
}
