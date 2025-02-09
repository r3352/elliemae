// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.EPPSLoanProgram
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class EPPSLoanProgram
  {
    private string programId = "";
    private string programName = "";

    public EPPSLoanProgram(string programId, string programName)
    {
      this.programId = programId;
      this.programName = programName;
    }

    public string ProgramID => this.programId;

    public string ProgramName => this.programName;

    public override bool Equals(object obj)
    {
      return obj != null && !(this.GetType() != obj.GetType()) && this.programId == ((EPPSLoanProgram) obj).programId;
    }

    public override int GetHashCode() => this.programId.GetHashCode();
  }
}
