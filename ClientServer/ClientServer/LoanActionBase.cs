// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanActionBase
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
  public class LoanActionBase : IXmlSerializable
  {
    public readonly string LoanActionID;

    public LoanActionBase(string loanActionID) => this.LoanActionID = (loanActionID ?? "").Trim();

    public LoanActionBase(XmlSerializationInfo info)
    {
      if (info is PackageSerializationInfo)
        this.LoanActionID = ((PackageSerializationInfo) info).NameToMilestoneID(info.GetString("LoanAction"));
      else
        this.LoanActionID = info.GetString(nameof (LoanActionID));
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      if (info is PackageSerializationInfo)
        info.AddValue("LoanAction", (object) ((PackageSerializationInfo) info).MilestoneIDToName(this.LoanActionID));
      else
        info.AddValue("LoanActionID", (object) this.LoanActionID);
    }
  }
}
