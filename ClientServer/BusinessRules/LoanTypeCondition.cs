// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanTypeCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanTypeCondition : PredefinedCondition, IFieldComposition
  {
    private BizRule.LoanType type;

    public LoanTypeCondition(BizRule.LoanType type) => this.type = type;

    public override bool AppliesTo(IExecutionContext context)
    {
      return BizRule.GetLoanTypeEnum(string.Concat(context.Fields["1172"])) == this.type;
    }

    public string[] GetDependentFields()
    {
      return new string[1]{ "1172" };
    }
  }
}
