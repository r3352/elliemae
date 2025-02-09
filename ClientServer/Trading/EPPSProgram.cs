// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.EPPSProgram
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class EPPSProgram : IXmlSerializable
  {
    public string Id;
    public string Name;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("Id", (object) this.Id);
      info.AddValue("Name", (object) this.Name);
    }

    public string ToXML() => new XmlSerializer().Serialize((object) this);

    public static EPPSProgram Parse(string xml)
    {
      return (xml ?? "") == "" ? (EPPSProgram) null : (EPPSProgram) new XmlSerializer().Deserialize(xml, typeof (EPPSProgram));
    }

    public EPPSProgram(EPPSProgram source)
    {
      this.Id = source.Id;
      this.Name = source.Name;
    }

    public EPPSProgram(XmlSerializationInfo info)
    {
      this.Id = info.GetString(nameof (Id));
      this.Name = info.GetString(nameof (Name));
    }

    public EPPSProgram()
    {
    }
  }
}
