// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.FieldMilestonePair
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public class FieldMilestonePair : MilestoneBase
  {
    private readonly string _fieldId;

    public FieldMilestonePair(string fieldId, string milestoneId)
      : base(milestoneId)
    {
      this._fieldId = fieldId;
    }

    public string FieldId => this._fieldId;
  }
}
