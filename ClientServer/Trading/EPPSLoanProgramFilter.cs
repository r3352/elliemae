// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.EPPSLoanProgramFilter
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class EPPSLoanProgramFilter : TradeEntity, IXmlSerializable
  {
    private string programId = "";
    private string programName = "";

    public EPPSLoanProgramFilter()
    {
    }

    public EPPSLoanProgramFilter(string programId, string programName)
    {
      this.programId = programId;
      this.programName = programName;
    }

    public EPPSLoanProgramFilter(Guid id, string programId, string programName)
    {
      if (id != Guid.Empty)
        this.Id = id;
      this.programId = programId;
      this.programName = programName;
    }

    public EPPSLoanProgramFilter(EPPSLoanProgramFilter source)
    {
      if (source.Id != Guid.Empty)
        this.Id = source.Id;
      this.programId = source.programId;
      this.programName = source.programName;
    }

    public EPPSLoanProgramFilter(XmlSerializationInfo info)
    {
      try
      {
        Guid guid = info.GetValue<Guid>("guid");
        this.Id = guid == Guid.Empty ? Guid.NewGuid() : guid;
      }
      catch
      {
        this.Id = Guid.NewGuid();
      }
      this.programId = info.GetString("id");
      this.programName = info.GetString("name");
    }

    public string ProgramId
    {
      get => this.programId;
      set => this.programId = value;
    }

    public string ProgramName
    {
      get => this.programName;
      set => this.programName = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("guid", (object) this.Id);
      info.AddValue("id", (object) this.programId);
      info.AddValue("name", (object) this.programName);
    }
  }
}
