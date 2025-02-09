// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomCalculation
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  [Serializable]
  public class CustomCalculation
  {
    private string calcId;
    private string userCode;
    private string[] dependentFields;

    public CustomCalculation(string userCode)
      : this(Guid.NewGuid().ToString("N"), userCode)
    {
    }

    public CustomCalculation(string calcId, string userCode)
    {
      this.calcId = calcId;
      this.userCode = userCode;
    }

    public string ID => this.calcId;

    public string UserCode => this.userCode;

    public string[] DependentFields
    {
      get
      {
        if (this.dependentFields == null)
          this.dependentFields = FieldReplacementRegex.ParseDependentFields(this.userCode);
        return this.dependentFields;
      }
    }

    public string ToSourceCode()
    {
      return FieldReplacementRegex.Replace(this.userCode).Replace("\r", " ").Replace("\n", " ");
    }

    public override string ToString() => this.userCode;

    public override bool Equals(object obj)
    {
      CustomCalculation customCalculation = obj as CustomCalculation;
      return obj != null && customCalculation.userCode == this.userCode;
    }

    public override int GetHashCode() => this.userCode.GetHashCode();
  }
}
