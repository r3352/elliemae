// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.DocMilestonePair
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public class DocMilestonePair : MilestoneBase
  {
    private readonly string _docGuid;
    private readonly bool _attachedRequired;

    public string Title { get; set; }

    public DocMilestonePair(string docGuid, string milestoneId, bool attachedRequired)
      : base(milestoneId)
    {
      this._docGuid = docGuid;
      this._attachedRequired = attachedRequired;
    }

    public string DocGUID => this._docGuid;

    public bool AttachmentRequired => this._attachedRequired;
  }
}
