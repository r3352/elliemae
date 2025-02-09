// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.LoanChannelCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class LoanChannelCondition : PredefinedCondition, IFieldComposition
  {
    private string channelOptions = string.Empty;

    public LoanChannelCondition(string channelOptions) => this.channelOptions = channelOptions;

    public override bool AppliesTo(IExecutionContext context)
    {
      return this.channelOptions.IndexOf(BizRule.GetChannelValue(string.Concat(context.Fields["2626"]))) > -1;
    }

    public string[] GetDependentFields()
    {
      return new string[1]{ "2626" };
    }
  }
}
