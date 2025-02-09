// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class MilestoneField : VirtualField
  {
    private const string FieldPrefix = "Log.MS";
    private MilestoneProperty property;
    private FieldOptionCollection options = FieldOptionCollection.Empty;

    public MilestoneField(MilestoneProperty property, string description, FieldFormat format)
      : base("Log.MS." + property.ToString(), description, format, FieldInstanceSpecifierType.Milestone)
    {
      this.property = property;
    }

    public MilestoneField(
      MilestoneProperty property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
      this.options = new FieldOptionCollection(options, true);
    }

    public MilestoneField(MilestoneField parent, string documentTitle)
      : base((VirtualField) parent, (object) documentTitle)
    {
      this.property = parent.property;
    }

    protected override FieldDefinition CreateInstanceFromSpecifier(object instanceSpecifier)
    {
      return (FieldDefinition) new MilestoneField(this, string.Concat(instanceSpecifier));
    }

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.MilestoneFields;

    protected override string Evaluate(LoanData loan)
    {
      MilestoneLog milestoneLog = this.getMilestoneLog(loan);
      if (milestoneLog == null)
        return "";
      switch (this.property)
      {
        case MilestoneProperty.Status:
          return !milestoneLog.Done ? "Expected" : "Achieved";
        case MilestoneProperty.Date:
          return !milestoneLog.Done ? "" : this.FormatDate(milestoneLog.Date);
        case MilestoneProperty.ExpectedDate:
          return this.FormatDate(milestoneLog.Date);
        case MilestoneProperty.Comments:
          return milestoneLog.Comments;
        case MilestoneProperty.Duration:
          return milestoneLog.Duration.ToString();
        case MilestoneProperty.DateTime:
          return !milestoneLog.Done ? "" : this.FormatDateTime(milestoneLog.Date);
        case MilestoneProperty.TPOConnectStatus:
          return milestoneLog.TPOConnectStatus;
        case MilestoneProperty.ConsumerStatus:
          return milestoneLog.ConsumerStatus;
        default:
          return "";
      }
    }

    public override FieldOptionCollection Options => this.options;

    private MilestoneLog getMilestoneLog(LoanData loan)
    {
      string stage = string.Concat(this.InstanceSpecifier);
      return loan.GetLogList().GetMilestoneByName(stage);
    }

    public MilestoneProperty Property => this.property;
  }
}
