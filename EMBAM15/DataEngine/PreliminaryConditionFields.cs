// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PreliminaryConditionFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class PreliminaryConditionFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static PreliminaryConditionFields()
    {
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.Title, "Preliminary Condition Title", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.Category, "Preliminary Condition Category", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.Description, "Preliminary Condition Description", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.AddedBy, "Preliminary Condition Added By", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.DateAdded, "Preliminary Condition Date Added", FieldFormat.DATE));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.Fulfilled, "Preliminary Condition Is Fulfilled", FieldFormat.YN));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.FulfilledBy, "Preliminary Condition Fulfilled By", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.DateFulfilled, "Preliminary Condition Date Fulfilled", FieldFormat.DATE));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.Status, "Preliminary Condition Status", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.PriorTo, "Preliminary Condition Prior To", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionField(PreliminaryConditionProperty.Comments, "Preliminary Condition Comments", FieldFormat.STRING));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.ALL", "Preliminary Conditions ALL"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.ALLOPEN", "Preliminary Conditions Not Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.ALLFULFILLED", "Preliminary Conditions Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTA", "Preliminary Conditions PTA ALL"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTD", "Preliminary Conditions PTD ALL"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTF", "Preliminary Conditions PTF ALL"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.AC", "Preliminary Conditions AC ALL"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTP", "Preliminary Conditions PTP ALL"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTAOPEN", "Preliminary Conditions PTA Not Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTDOPEN", "Preliminary Conditions PTD Not Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTFOPEN", "Preliminary Conditions PTF Not Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.ACOPEN", "Preliminary Conditions AC Not Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTPOPEN", "Preliminary Conditions PTP Not Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTAFULFILLED", "Preliminary Conditions PTA Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTDFULFILLED", "Preliminary Conditions PTD Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTFFULFILLED", "Preliminary Conditions PTF Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.ACFULFILLED", "Preliminary Conditions AC Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionSummaryField("PRECON.PTPFULFILLED", "Preliminary Conditions PTP Fulfilled"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.ALLCOUNT", "Preliminary Conditions Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.OPENCOUNT", "Open Preliminary Conditions Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.FULFILLEDCOUNT", "Fulfilled Preliminary Conditions Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTACOUNT", "Preliminary Conditions PTA Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTDCOUNT", "Preliminary Conditions PTD Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTFCOUNT", "Preliminary Conditions PTF Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.ACCOUNT", "Preliminary Conditions AC Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTPCOUNT", "Preliminary Conditions PTP Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTAOPENCOUNT", "Open Preliminary Conditions PTA Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTDOPENCOUNT", "Open Preliminary Conditions PTD Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTFOPENCOUNT", "Open Preliminary Conditions PTF Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.ACOPENCOUNT", "Open Preliminary Conditions AC Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTPOPENCOUNT", "Open Preliminary Conditions PTP Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTAFULFILLEDCOUNT", "Fulfilled Preliminary Conditions PTA Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTDFULFILLEDCOUNT", "Fulfilled Preliminary Conditions PTD Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTFFULFILLEDCOUNT", "Fulfilled Preliminary Conditions PTF Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.ACFULFILLEDCOUNT", "Fulfilled Preliminary Conditions AC Count"));
      PreliminaryConditionFields.All.Add((FieldDefinition) new PreliminaryConditionCountField("PRECON.PTPFULFILLEDCOUNT", "Fulfilled Preliminary Conditions PTP Count"));
    }
  }
}
