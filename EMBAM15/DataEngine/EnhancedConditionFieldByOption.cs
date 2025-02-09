// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EnhancedConditionFieldByOption
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class EnhancedConditionFieldByOption
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static EnhancedConditionFieldByOption()
    {
      EnhancedConditionFieldByOption.All.Add((FieldDefinition) new EnhancedConditionByOptionField(EnhancedConditionByOptionProperty.COP, "Category Options", FieldFormat.STRING));
      EnhancedConditionFieldByOption.All.Add((FieldDefinition) new EnhancedConditionByOptionField(EnhancedConditionByOptionProperty.PTO, "Prior To", FieldFormat.STRING));
      EnhancedConditionFieldByOption.All.Add((FieldDefinition) new EnhancedConditionByOptionField(EnhancedConditionByOptionProperty.SOP, "Source Options", FieldFormat.STRING));
      EnhancedConditionFieldByOption.All.Add((FieldDefinition) new EnhancedConditionByOptionField(EnhancedConditionByOptionProperty.ROP, "Recipient Options", FieldFormat.STRING));
      EnhancedConditionFieldByOption.All.Add((FieldDefinition) new EnhancedConditionByOptionField(EnhancedConditionByOptionProperty.TOP, "Tracking Options", FieldFormat.STRING));
      EnhancedConditionFieldByOption.All.Add((FieldDefinition) new EnhancedConditionByOptionField(EnhancedConditionByOptionProperty.TOW, "Tracking Owners", FieldFormat.STRING));
    }
  }
}
