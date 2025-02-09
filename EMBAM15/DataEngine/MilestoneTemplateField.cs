// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneTemplateField
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Workflow;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class MilestoneTemplateField : VirtualField
  {
    private const string FieldPrefix = "Log.MT";
    private MilestoneTemplateProperty property;

    public MilestoneTemplateField(
      MilestoneTemplateProperty property,
      string description,
      FieldFormat format)
      : base("Log.MT." + property.ToString(), description, format, FieldInstanceSpecifierType.None)
    {
      this.property = property;
    }

    public MilestoneTemplateField(
      MilestoneTemplateProperty property,
      string description,
      FieldFormat format,
      string[] options)
      : this(property, description, format)
    {
    }

    public override bool AllowEdit => base.AllowEdit;

    public override VirtualFieldType VirtualFieldType => VirtualFieldType.MilestoneTemplateFields;

    protected override string Evaluate(LoanData loan)
    {
      MilestoneTemplate milestoneTemplate = loan.GetLogList().MilestoneTemplate;
      if (milestoneTemplate == null)
        return "";
      switch (this.property)
      {
        case MilestoneTemplateProperty.TemplateId:
          return milestoneTemplate.TemplateID;
        case MilestoneTemplateProperty.Name:
          return milestoneTemplate.Name;
        case MilestoneTemplateProperty.Active:
          return !milestoneTemplate.Active ? "N" : "Y";
        case MilestoneTemplateProperty.IsLocked:
          return !loan.GetLogList().MSLock ? "N" : "Y";
        case MilestoneTemplateProperty.IsDateLocked:
          return !loan.GetLogList().MSDateLock ? "N" : "Y";
        default:
          return "";
      }
    }

    public MilestoneTemplateProperty Property => this.property;
  }
}
