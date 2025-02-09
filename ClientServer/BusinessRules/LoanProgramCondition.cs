// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanProgramCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanProgramCondition : PredefinedCondition, IFieldComposition
  {
    private string loanProgramName = string.Empty;

    public LoanProgramCondition(string loanProgramName) => this.loanProgramName = loanProgramName;

    public override bool AppliesTo(IExecutionContext context)
    {
      return string.Concat(context.Fields["1401"]) == this.loanProgramName;
    }

    public string[] GetDependentFields()
    {
      return new string[1]{ "1401" };
    }
  }
}
