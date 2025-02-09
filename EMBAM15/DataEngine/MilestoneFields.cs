// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class MilestoneFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static MilestoneFields()
    {
      MilestoneFields.All.Add((FieldDefinition) new CoreMilestoneField());
      MilestoneFields.All.Add((FieldDefinition) new CurrentMilestoneField());
      MilestoneFields.All.Add((FieldDefinition) new NextMilestoneField());
      MilestoneFields.All.Add((FieldDefinition) new NextMilestoneNameField());
      MilestoneFields.All.Add((FieldDefinition) new LastCompletedMilestoneField());
      MilestoneFields.All.Add((FieldDefinition) new LastCompletedMilestoneNameField());
      MilestoneFields.All.Add((FieldDefinition) new CurrentMilestoneTPOConnectStatusField());
      MilestoneFields.All.Add((FieldDefinition) new CurrentConsumerStatusField());
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.Status, "Milestone Status", FieldFormat.STRING, new string[2]
      {
        "Expected",
        "Achieved"
      }));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.Date, "Milestone Date", FieldFormat.DATE));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.DateTime, "Milestone Date/Time", FieldFormat.DATETIME));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.ExpectedDate, "Milestone Expected Completion Date", FieldFormat.DATE));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.Comments, "Milestone Comments", FieldFormat.STRING));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.Duration, "Milestone Duration", FieldFormat.INTEGER));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.TPOConnectStatus, "Milestone TPO Connect Status", FieldFormat.STRING));
      MilestoneFields.All.Add((FieldDefinition) new MilestoneField(MilestoneProperty.ConsumerStatus, "Milestone Consumer Status", FieldFormat.STRING));
    }
  }
}
