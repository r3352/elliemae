// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.FieldMilestonePair
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class FieldMilestonePair : MilestoneBase
  {
    public readonly string FieldID;

    public FieldMilestonePair(string fieldID, string milestoneID)
      : base(milestoneID)
    {
      this.FieldID = fieldID;
    }

    public FieldMilestonePair(XmlSerializationInfo info)
      : base(info)
    {
      this.FieldID = info.GetString(nameof (FieldID));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("FieldID", (object) this.FieldID);
    }
  }
}
