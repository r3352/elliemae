// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Workflow.MilestoneTemplateCondition
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Workflow
{
  [Serializable]
  public class MilestoneTemplateCondition : BinaryConvertible<MilestoneTemplateCondition>
  {
    private MilestoneTemplateConditionType conditionType;
    private IXmlSerializable condition;

    public MilestoneTemplateCondition(MilestoneTemplateSimpleCondition condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      this.conditionType = MilestoneTemplateConditionType.Simple;
      this.condition = (IXmlSerializable) condition;
    }

    public MilestoneTemplateCondition(FieldFilterList condition)
    {
      if (condition == null)
        throw new ArgumentNullException(nameof (condition));
      this.conditionType = MilestoneTemplateConditionType.Advanced;
      this.condition = (IXmlSerializable) condition;
    }

    public MilestoneTemplateCondition(XmlSerializationInfo info)
    {
      this.conditionType = info.GetEnum<MilestoneTemplateConditionType>(nameof (ConditionType));
      if (this.conditionType == MilestoneTemplateConditionType.Advanced)
      {
        this.condition = (IXmlSerializable) info.GetValue<FieldFilterList>("Condition");
      }
      else
      {
        if (this.conditionType != MilestoneTemplateConditionType.Simple)
          return;
        this.condition = (IXmlSerializable) info.GetValue<MilestoneTemplateSimpleCondition>("Condition");
      }
    }

    public MilestoneTemplateConditionType ConditionType => this.conditionType;

    public FieldFilterList GetAdvancedCondition() => this.condition as FieldFilterList;

    public MilestoneTemplateSimpleCondition GetSimpleCondition()
    {
      return this.condition as MilestoneTemplateSimpleCondition;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("ConditionType", (object) this.conditionType);
      info.AddValue("Condition", (object) this.condition);
    }

    public override string ToString()
    {
      if (this.conditionType == MilestoneTemplateConditionType.Advanced)
        return this.GetAdvancedCondition().ToString(false);
      return this.conditionType == MilestoneTemplateConditionType.Simple ? this.GetSimpleCondition().ToString() : "";
    }
  }
}
