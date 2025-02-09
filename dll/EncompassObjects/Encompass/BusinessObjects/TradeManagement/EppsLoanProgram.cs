// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.EppsLoanProgram
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class EppsLoanProgram
  {
    public string programId;
    public string programName;

    public EppsLoanProgram(string programId, string programName)
    {
      this.programId = programId;
      this.programName = programName;
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
  }
}
