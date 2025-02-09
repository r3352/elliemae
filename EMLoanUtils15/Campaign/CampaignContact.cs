// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Campaign.CampaignContact
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.BizLayer;
using EllieMae.EMLite.ClientServer.Campaign;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Campaign
{
  [Serializable]
  public class CampaignContact : BusinessBase, IDisposable
  {
    private CampaignContactInfo contactInfo;
    private CampaignActivityCollection contactActivities;
    private bool isReinsert;

    public int ContactId => this.contactInfo.ContactId;

    public DateTime CreatedDate => this.contactInfo.CreatedDate;

    public int ContactActivityCount
    {
      get => this.contactActivities == null ? 0 : this.contactActivities.Count;
    }

    public CampaignActivityCollection ContactActivities => this.contactActivities;

    public bool IsReinsert
    {
      get => this.isReinsert;
      set
      {
        if (this.isReinsert == value)
          return;
        this.isReinsert = value;
        this.MarkDirty();
      }
    }

    public bool HasActivityStatus(ActivityStatus status)
    {
      if (this.contactActivities == null)
        return false;
      foreach (CampaignActivity contactActivity in (CollectionBase) this.contactActivities)
      {
        if (status == contactActivity.Status)
          return true;
      }
      return false;
    }

    internal CampaignContactInfo GetInfo()
    {
      this.contactInfo.IsNew = this.IsNew;
      this.contactInfo.IsDirty = this.IsDirty;
      this.contactInfo.IsDeleted = this.IsDeleted;
      return this.contactInfo;
    }

    public override string ToString()
    {
      return string.Format("CampaignContact[{0}]", (object) this.ContactId);
    }

    public bool Equals(CampaignContact obj) => this.ContactId.Equals(obj.ContactId);

    public override int GetHashCode() => this.ContactId.GetHashCode();

    public static CampaignContact NewCampaignContact() => new CampaignContact();

    public static CampaignContact NewCampaignContact(int contactId)
    {
      return new CampaignContact(contactId);
    }

    public static CampaignContact NewCampaignContact(CampaignContactInfo contactInfo)
    {
      return new CampaignContact(contactInfo);
    }

    public static CampaignContact GetCampaignContact(CampaignContactInfo contactInfo)
    {
      throw new NotSupportedException("");
    }

    private CampaignContact()
    {
      this.MarkNew();
      this.MarkAsChild();
      this.contactInfo = new CampaignContactInfo(0, DateTime.Today, (CampaignActivityInfo[]) null);
    }

    private CampaignContact(int contactId)
    {
      this.MarkNew();
      this.MarkAsChild();
      this.contactInfo = new CampaignContactInfo(contactId, DateTime.Today, (CampaignActivityInfo[]) null);
    }

    private CampaignContact(CampaignContactInfo contactInfo)
    {
      this.MarkOld();
      this.MarkAsChild();
      this.contactInfo = contactInfo;
      if (contactInfo.ContactActivities == null)
        return;
      this.contactActivities = CampaignActivityCollection.NewCampaignActivityCollection(contactInfo.ContactActivities.Length);
      foreach (CampaignActivityInfo contactActivity in contactInfo.ContactActivities)
        this.contactActivities.Add(CampaignActivity.NewCampaignActivity(contactActivity));
    }

    public void Dispose()
    {
    }
  }
}
