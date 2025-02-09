// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PostClosingConditionFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class PostClosingConditionFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static PostClosingConditionFields()
    {
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.Title, "Post-Condition Title", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.Source, "Post-Condition Source", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.Description, "Post-Condition Description", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.AddedBy, "Post-Condition Added By", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateAdded, "Post-Condition Date Added", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.IsCleared, "Post-Condition Is Cleared", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.ClearedBy, "Post-Condition Cleared By", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateCleared, "Post-Condition Date Cleared", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.IsReceived, "Post-Condition Is Received", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.ReceivedBy, "Post-Condition Received By", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateReceived, "Post-Condition Date Received", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.IsRequested, "Post-Condition Is Requested", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.RequestedBy, "Post-Condition Requested By", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.RequestedFrom, "Post-Condition Requested From", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateRequested, "Post-Condition Date Requested", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.IsRerequested, "Post-Condition Is Rerequested", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.RerequestedBy, "Post-Condition Rerequested By", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateRerequested, "Post-Condition Date Rerequested", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.IsSent, "Post-Condition Is Sent", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.SentBy, "Post-Condition Sent By", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateSent, "Post-Condition Date Sent", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.Recipient, "Post-Condition Recipient", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.DateExpected, "Post-Condition Date Expected", FieldFormat.DATE));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.Status, "Post-Condition Status", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.Comments, "Post-Condition Comments", FieldFormat.STRING));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.PrintInternally, "Post-Condition Is Printed Internally", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionField(PostClosingConditionProperty.PrintExternally, "Post-Condition Is Printed Externally", FieldFormat.YN));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionSummaryField("PCC.ALL", "Post Closing Condition All"));
      PostClosingConditionFields.All.Add((FieldDefinition) new PostClosingConditionSummaryField("PCC.NOTCLEARED", "Post Closing Condition Not Cleared"));
    }
  }
}
