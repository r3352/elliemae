// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomFieldCalculator
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CustomFieldCalculator : FieldCalculator
  {
    private CustomFieldInfo fieldInfo;

    public CustomFieldCalculator(
      CustomFieldInfo fieldInfo,
      CustomCalculation calc,
      ICustomCalculationImpl impl)
      : base((FieldDefinition) new CustomField(fieldInfo), calc, impl)
    {
      this.fieldInfo = fieldInfo;
    }

    public CustomFieldCalculator(
      CustomFieldInfo fieldInfo,
      CustomCalculation calc,
      TypeIdentifier typeId,
      RuntimeContext context)
      : base((FieldDefinition) new CustomField(fieldInfo), calc, typeId, context)
    {
      this.fieldInfo = fieldInfo;
    }

    public CustomFieldInfo Field => this.fieldInfo;
  }
}
