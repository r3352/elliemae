// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Campaign.CampaignSummaryInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Campaign
{
  [Serializable]
  public class CampaignSummaryInfo
  {
    private CampaignSummaryInfo.EnumDictionary campaignDictionary;
    private CampaignSummaryInfo.EnumDictionary tasksDueDictionary;

    public int this[CampaignStatus campaignStatus]
    {
      get
      {
        return this.campaignDictionary[Enum.Format(typeof (CampaignStatus), (object) campaignStatus, "G")];
      }
      set
      {
        this.campaignDictionary[Enum.Format(typeof (CampaignStatus), (object) campaignStatus, "G")] = value;
      }
    }

    public int this[ActivityType activityType]
    {
      get
      {
        return this.tasksDueDictionary[Enum.Format(typeof (ActivityType), (object) activityType, "G")];
      }
      set
      {
        this.tasksDueDictionary[Enum.Format(typeof (ActivityType), (object) activityType, "G")] = value;
      }
    }

    public CampaignSummaryInfo()
    {
      this.campaignDictionary = new CampaignSummaryInfo.EnumDictionary(typeof (CampaignStatus));
      this.tasksDueDictionary = new CampaignSummaryInfo.EnumDictionary(typeof (ActivityType));
    }

    [Serializable]
    public class EnumDictionary : DictionaryBase
    {
      public EnumDictionary(Type enumType)
      {
        foreach (string name in Enum.GetNames(enumType))
          this.Add(name, 0);
      }

      public int this[string enumName]
      {
        get => (int) this.Dictionary[(object) enumName];
        set => this.Dictionary[(object) enumName] = (object) value;
      }

      private void Add(string enumName, int value)
      {
        this.Dictionary.Add((object) enumName, (object) value);
      }
    }
  }
}
