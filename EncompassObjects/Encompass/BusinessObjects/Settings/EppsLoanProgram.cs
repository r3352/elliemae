// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Settings.EppsLoanProgram
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Settings
{
  /// <summary>EppsLoanProgram Class</summary>
  public class EppsLoanProgram
  {
    /// <summary>ProgramId</summary>
    public string programId;
    /// <summary>ProgramName</summary>
    public string programName;

    /// <summary>Constructor</summary>
    /// <param name="programId"></param>
    /// <param name="programName"></param>
    public EppsLoanProgram(string programId, string programName)
    {
      this.programId = programId;
      this.programName = programName;
    }

    /// <summary>Gets or Sets the Program Id</summary>
    public string ProgramId
    {
      get => this.programId;
      set => this.programId = value;
    }

    /// <summary>Gets or Sets the Program Name</summary>
    public string ProgramName
    {
      get => this.programName;
      set => this.programName = value;
    }
  }
}
