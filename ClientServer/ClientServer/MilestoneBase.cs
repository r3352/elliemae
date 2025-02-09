// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MilestoneBase
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Packages;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class MilestoneBase : IXmlSerializable
  {
    public readonly string MilestoneID;

    public MilestoneBase(string milestoneID) => this.MilestoneID = (milestoneID ?? "").Trim();

    public MilestoneBase(XmlSerializationInfo info)
    {
      if (info is PackageSerializationInfo)
        this.MilestoneID = ((PackageSerializationInfo) info).NameToMilestoneID(info.GetString("Milestone"));
      else
        this.MilestoneID = info.GetString(nameof (MilestoneID));
    }

    public bool IsCoreMilestone => this.CoreMilestoneID > 0;

    public string CustomMilestoneGuid => !this.IsCoreMilestone ? this.MilestoneID : "";

    public int CoreMilestoneID
    {
      get
      {
        try
        {
          return Convert.ToInt32(this.MilestoneID);
        }
        catch
        {
          return -1;
        }
      }
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      if (info is PackageSerializationInfo)
        info.AddValue("Milestone", (object) ((PackageSerializationInfo) info).MilestoneIDToName(this.MilestoneID));
      else
        info.AddValue("MilestoneID", (object) this.MilestoneID);
    }
  }
}
