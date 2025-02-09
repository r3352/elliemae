// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignContactInfo
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
  public class CampaignContactInfo : IPropertyDictionary
  {
    public int ContactId;
    public DateTime CreatedDate;
    public CampaignActivityInfo[] ContactActivities;
    public Dictionary<string, object> ContactProperties = new Dictionary<string, object>();
    public bool IsNew;
    public bool IsDirty;
    public bool IsDeleted;

    public CampaignContactInfo(
      int contactId,
      DateTime createdDate,
      CampaignActivityInfo[] contactActivities)
    {
      this.ContactId = contactId;
      this.CreatedDate = createdDate;
      this.ContactActivities = contactActivities;
    }

    public object this[string propertyName]
    {
      get
      {
        switch (propertyName)
        {
          case "CampaignActivity.ContactId":
            return (object) this.ContactId;
          case "CampaignActivity.CreatedDate":
            return (object) this.CreatedDate;
          case "CampaignActivity.Activities":
            return (object) this.ContactActivities;
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
