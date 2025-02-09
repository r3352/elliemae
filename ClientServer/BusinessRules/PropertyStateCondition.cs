// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PropertyStateCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class PropertyStateCondition : PredefinedCondition, IFieldComposition
  {
    private USPS.StateCode stateCode;

    public PropertyStateCondition(USPS.StateCode stateCode) => this.stateCode = stateCode;

    public override bool AppliesTo(IExecutionContext context)
    {
      string upper = string.Concat(context.Fields["14"]).Trim().ToUpper();
      USPS.StateCode stateCode = USPS.StateCode.Unknown;
      if (USPS.StateCodes.ContainsKey((object) upper))
        stateCode = (USPS.StateCode) USPS.StateCodes[(object) upper];
      return stateCode == this.stateCode;
    }

    public string[] GetDependentFields()
    {
      return new string[1]{ "14" };
    }
  }
}
