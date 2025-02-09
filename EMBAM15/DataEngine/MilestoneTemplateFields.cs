// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.MilestoneTemplateFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class MilestoneTemplateFields
  {
    public static readonly FieldDefinitionCollection All = new FieldDefinitionCollection();

    static MilestoneTemplateFields()
    {
      MilestoneTemplateFields.All.Add((FieldDefinition) new MilestoneTemplateField(MilestoneTemplateProperty.IsLocked, "MilestoneTemplate Lock Status", FieldFormat.YN));
      MilestoneTemplateFields.All.Add((FieldDefinition) new MilestoneTemplateField(MilestoneTemplateProperty.Name, "MilestoneTemplate Name", FieldFormat.STRING));
      MilestoneTemplateFields.All.Add((FieldDefinition) new MilestoneTemplateField(MilestoneTemplateProperty.TemplateId, "MilestoneTemplate ID", FieldFormat.INTEGER));
      MilestoneTemplateFields.All.Add((FieldDefinition) new MilestoneTemplateField(MilestoneTemplateProperty.IsDateLocked, "MilestoneTemplate Is Date Locked", FieldFormat.YN));
    }
  }
}
