// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneTaskFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class MilestoneTaskFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static MilestoneTaskFields()
    {
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Name, "Task Name", FieldFormat.STRING));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Milestone, "Task Milestone", FieldFormat.STRING));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Description, "Task Description", FieldFormat.STRING));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Required, "Task Required Flag", FieldFormat.YN));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.DateAdded, "Date Task Added", FieldFormat.DATE));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.AddedBy, "Task Creator", FieldFormat.STRING));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.DateCompleted, "Date Task Completed", FieldFormat.DATE));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.CompletedBy, "User Who Completed Task", FieldFormat.STRING));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Completed, "Task Completed", FieldFormat.YN));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.ContactCount, "Number of Task Contacts", FieldFormat.INTEGER));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Comments, "Task Comments", FieldFormat.STRING));
      MilestoneTaskFields.All.Add((FieldDefinition) new MilestoneTaskField(MilestoneTaskProperty.Priority, "Task Priority", FieldFormat.STRING, new string[3]
      {
        "Low",
        "Normal",
        "High"
      }));
    }
  }
}
