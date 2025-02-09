// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.FieldStateCondition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public class FieldStateCondition : PredefinedCondition, IFieldComposition
  {
    private string fieldId;
    private FieldState requiredState;

    public FieldStateCondition(string fieldId, FieldState requiredState)
    {
      this.fieldId = fieldId;
      this.requiredState = requiredState;
    }

    public string FieldID => this.fieldId;

    public FieldState RequiredState => this.requiredState;

    public override bool AppliesTo(IExecutionContext icontext)
    {
      ExecutionContext executionContext = (ExecutionContext) icontext;
      bool flag = Utils.UnformatValue(executionContext.Loan.GetField(this.fieldId), executionContext.Loan.GetFormat(this.fieldId)) == "";
      return this.requiredState != FieldState.Empty ? !flag : flag;
    }

    public string[] GetDependentFields()
    {
      return new string[1]{ this.fieldId };
    }
  }
}
