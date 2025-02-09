// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneTaskField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class MilestoneTaskField : VirtualField
  {
    public const string FieldPrefix = "Task.";
    private MilestoneTaskProperty property;
    private FieldOptionCollection options = FieldOptionCollection.Empty;

    public MilestoneTaskField(
      MilestoneTaskProperty property,
      string description,
      FieldFormat format)
      : base("Task." + property.ToString(), description, format, FieldInstanceSpecifierType.MilestoneTask)
    {
      this.property = property;
    }

    public MilestoneTaskField(MilestoneTaskField parent, string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
    }

    public MilestoneTaskField(
      MilestoneTaskProperty property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.MilestoneTaskFields;

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new MilestoneTaskField(this, string.Concat(instanceSpecifier));
    }

    protected override string Evaluate(LoanData loan)
    {
      MilestoneTaskLog taskLog = this.getTaskLog(loan);
      if (taskLog == null)
        return "";
      switch (this.property)
      {
        case MilestoneTaskProperty.Name:
          return taskLog.TaskName;
        case MilestoneTaskProperty.Milestone:
          return taskLog.Stage;
        case MilestoneTaskProperty.DateAdded:
          return this.FormatDate(taskLog.AddDate);
        case MilestoneTaskProperty.AddedBy:
          return taskLog.AddedBy;
        case MilestoneTaskProperty.DateCompleted:
          return this.FormatDate(taskLog.CompletedDate);
        case MilestoneTaskProperty.CompletedBy:
          return taskLog.CompletedBy;
        case MilestoneTaskProperty.Completed:
          return !taskLog.Completed ? "N" : "Y";
        case MilestoneTaskProperty.Required:
          return !taskLog.IsRequired ? "N" : "Y";
        case MilestoneTaskProperty.Description:
          return taskLog.TaskDescription;
        case MilestoneTaskProperty.ContactCount:
          return string.Concat((object) taskLog.ContactCount);
        case MilestoneTaskProperty.Comments:
          return taskLog.Comments;
        case MilestoneTaskProperty.Priority:
          return taskLog.TaskPriority;
        default:
          return "";
      }
    }

    private MilestoneTaskLog getTaskLog(LoanData loan)
    {
      foreach (MilestoneTaskLog taskLog in loan.GetLogList().GetAllRecordsOfType(typeof (MilestoneTaskLog)))
      {
        if (string.Compare(taskLog.TaskName, this.InstanceSpecifier.ToString(), true) == 0)
          return taskLog;
      }
      return (MilestoneTaskLog) null;
    }

    public override FieldOptionCollection Options => this.options;

    public static FieldFormat GetPropertyFormat(MilestoneTaskProperty property)
    {
      switch (property)
      {
        case MilestoneTaskProperty.DateAdded:
        case MilestoneTaskProperty.DateCompleted:
          return FieldFormat.DATE;
        case MilestoneTaskProperty.Completed:
        case MilestoneTaskProperty.Required:
          return FieldFormat.YN;
        case MilestoneTaskProperty.ContactCount:
          return FieldFormat.INTEGER;
        default:
          return FieldFormat.STRING;
      }
    }

    public MilestoneTaskProperty Property => this.property;
  }
}
