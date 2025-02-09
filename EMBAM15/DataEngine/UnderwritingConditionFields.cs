// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.UnderwritingConditionFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class UnderwritingConditionFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static UnderwritingConditionFields()
    {
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.Title, "Condition Title", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.Category, "Condition Category", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.Description, "Condition Description", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.AddedBy, "Condition Added By", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.DateAdded, "Condition Date Added", FieldFormat.DATE));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.IsCleared, "Condition Is Cleared", FieldFormat.YN));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.ClearedBy, "Condition Cleared By", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.DateCleared, "Condition Date Cleared", FieldFormat.DATE));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.IsReceived, "Condition Is Received", FieldFormat.YN));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.ReceivedBy, "Condition Received By", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.DateReceived, "Condition Date Received", FieldFormat.DATE));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.IsReviewed, "Condition Is Reviewed", FieldFormat.YN));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.ReviewedBy, "Condition Reviewed By", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.DateReviewed, "Condition Date Reviewed", FieldFormat.DATE));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.IsWaived, "Condition Is Waived", FieldFormat.YN));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.WaivedBy, "Condition Waived By", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.DateWaived, "Condition Date Waived", FieldFormat.DATE));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.Status, "Condition Status", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.PriorTo, "Condition Prior To", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.Comments, "Condition Comments", FieldFormat.STRING));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.AllowToClear, "Condition Allow to Clear", FieldFormat.YN));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionField(UnderwritingConditionProperty.IsInternal, "Condition Is Internal", FieldFormat.YN));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.ALL", "Underwriting Conditions ALL"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.NOTCLEARED", "Underwriting Conditions Not Cleared"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.PTA", "Underwriting Conditions PTA"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.PTD", "Underwriting Conditions PTD"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.PTF", "Underwriting Conditions PTF"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.AC", "Underwriting Conditions AC"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.PTP", "Underwriting Conditions PTP"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.INTERNAL", "Underwriting Conditions ALL Internal Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionSummaryField("UWC.OPENINTERNAL", "Underwriting Conditions ALL Internal Open Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ALLCOUNT", "Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ADDEDCOUNT", "New Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.FULFILLEDCOUNT", "Fulfilled Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.RECEIVEDCOUNT", "Received Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.REVIEWEDCOUNT", "Reviewed Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.REJECTEDCOUNT", "Rejected Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.CLEAREDCOUNT", "Cleared Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.WAIVEDCOUNT", "Waived Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.OPENCOUNT", "Open Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.OPENEXTERNALCOUNT", "Open External Underwriting Conditions"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ALLPTACOUNT", "Number of all External/Internal Underwriting Conditions PTA - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ALLPTCPTFCOUNT", "Number of all External/Internal Underwriting Conditions PTC and PTF - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ALLPTPCOUNT", "Number of all External/Internal Underwriting Conditions PTP - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ALLPTDCOUNT", "Number of all External/Internal External Underwriting Conditions PTD - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ALLACCOUNT", "Number of all External/Internal External Underwriting Conditions AC - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.PTACOUNT", "Number of all External Underwriting Conditions PTA - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.PTCPTFCOUNT", "Number of all External Underwriting Conditions PTC and PTF - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.PTPCOUNT", "Number of all External Underwriting Conditions PTP - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.PTDCOUNT", "Number of all External External Underwriting Conditions PTD - Not Cleared/Not Waived"));
      UnderwritingConditionFields.All.Add((FieldDefinition) new UnderwritingConditionCountField("UWC.ACCOUNT", "Number of all External External Underwriting Conditions AC - Not Cleared/Not Waived"));
    }
  }
}
