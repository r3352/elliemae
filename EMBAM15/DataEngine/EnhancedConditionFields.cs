// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.EnhancedConditionFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class EnhancedConditionFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static EnhancedConditionFields()
    {
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalCount, "Number of External Enhanced Conditions", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalOpenCount, "Number of Open External Enhanced Conditions", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalType, "All External Enhanced Conditions", FieldFormat.STRING, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalOpenType, "All Open External Enhanced Conditions", FieldFormat.STRING, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.ExternalOpenTypeCount, "Number of Open External Conditions of specific condition type", FieldFormat.INTEGER, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.ExternalTypeCount, "Number of External Conditions of specific condition type", FieldFormat.INTEGER, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllConditionExternalOpenType, "All Open External Enhanced Conditions of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllConditionExternalType, "All External Enhanced Conditions of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllInternalType, "All Internal Enhanced Conditions", FieldFormat.STRING, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllInternalOpenType, "All Internal Open Enhanced Conditions", FieldFormat.STRING, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllConditionInternalType, "All Internal Enhanced Conditions of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllConditionInternalOpenType, "All Open Internal Enhanced Conditions of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.InternalTypeCount, "Number of Internal Conditions of specific condition type", FieldFormat.INTEGER, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.InternalOpenTypeCount, "Number of Open Internal Conditions of specific condition type", FieldFormat.INTEGER, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllInternalCount, "Number of Internal Enhanced Conditions of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllInternalOpenCount, "Number of Open Internal Enhanced Conditions of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllOpenCount, "Number of Open External and Internal Conditions of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllCount, "Number of External and Internal Conditions of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllOpenType, "All Open Enhanced Conditions of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllType, "All Enhanced Conditions of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalPulibshedCount, "Number of External Enhanced Conditions with an open Publish Date", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.ExternalTypeSatisfied, "All External Enhanced Conditions which are not open of a specific condition type", FieldFormat.STRING, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalSatisfied, "All External Enhanced Conditions which are not open of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.ExternalSatisfiedCount, "Number of External Enhanced Conditions which are not open of specific condition type", FieldFormat.INTEGER, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllExternalSatisfiedCount, "Number of External Enhanced Conditions which are not open of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.InternalTypeSatisfied, "All Internal Enhanced Conditions which are not open of specific condition type", FieldFormat.STRING, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllInternalTypeSatisfied, "All Internal Enhanced Conditions which are not open of all condition types", FieldFormat.STRING, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.InternalSatisfiedCount, "Number of Internal Enhanced Conditions which are not open of specific condition type", FieldFormat.INTEGER, true));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllInternalSatisfiedCount, "Number of Internal Enhanced Conditions which are not open of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllSatisfiedCount, "Number of Internal & External Enhanced Conditions which are not open of all condition types", FieldFormat.INTEGER, false));
      EnhancedConditionFields.All.Add((FieldDefinition) new EnhancedExternalConditionSummaryField(EnhancedExternalConditionProperty.AllSatisfied, "All Internal & External Enhanced Conditions which are not open of all condition types", FieldFormat.STRING, false));
    }
  }
}
