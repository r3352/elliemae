// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AutoRetrieveSettings
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class AutoRetrieveSettings : IXmlSerializable
  {
    private int autoRetrieveSettingsID;
    private bool access;
    private int? parentRetrieveSettingsId;
    private string shortDescription;
    private string milestoneId;

    public int AutoRetrieveSettingsID
    {
      get => this.autoRetrieveSettingsID;
      set => this.autoRetrieveSettingsID = value;
    }

    public bool Access
    {
      get => this.access;
      set => this.access = value;
    }

    public int? ParentRetrieveSettingsId
    {
      get => this.parentRetrieveSettingsId;
      set => this.parentRetrieveSettingsId = value;
    }

    public string ShortDescription
    {
      get => this.shortDescription;
      set => this.shortDescription = value;
    }

    public string MilestoneId
    {
      get => this.milestoneId;
      set => this.milestoneId = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("AutoRetrieveSettingsID", (object) this.autoRetrieveSettingsID);
      info.AddValue("Access", (object) this.access);
      info.AddValue("ParentRetrieveSettingsId", (object) this.parentRetrieveSettingsId);
      info.AddValue("ShortDescription", (object) this.shortDescription);
      info.AddValue("MilestoneId", (object) this.milestoneId);
    }
  }
}
