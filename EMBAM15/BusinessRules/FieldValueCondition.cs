// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.FieldValueCondition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public class FieldValueCondition : PredefinedCondition
  {
    private string fieldId;
    private string fieldValue;

    public FieldValueCondition(string fieldId, string fieldValue)
    {
      this.fieldId = fieldId;
      this.fieldValue = fieldValue;
    }

    public string FieldID => this.fieldId;

    public string FieldValue => this.fieldValue;

    public override bool AppliesTo(IExecutionContext icontext)
    {
      return string.Compare(((ExecutionContext) icontext).Loan.GetField(this.fieldId), this.fieldValue, true) == 0;
    }
  }
}
