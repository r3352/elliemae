// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.MockSpecialFeatureCodeService
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class MockSpecialFeatureCodeService : ISpecialFeatureCodeManager
  {
    private IList<SpecialFeatureCodeDefinition> List { get; set; }

    public MockSpecialFeatureCodeService()
    {
      this.List = (IList<SpecialFeatureCodeDefinition>) Enumerable.Range(123, 200).Select<int, SpecialFeatureCodeDefinition>((Func<int, SpecialFeatureCodeDefinition>) (c => new SpecialFeatureCodeDefinition(c.ToString(), "Fannie Mae", string.Format("Description for '{0}'", (object) c), "Just a test"))).ToList<SpecialFeatureCodeDefinition>();
    }

    private SpecialFeatureCodeDefinition FindMatching(SpecialFeatureCodeDefinition toFind)
    {
      return this.List.FirstOrDefault<SpecialFeatureCodeDefinition>((Func<SpecialFeatureCodeDefinition, bool>) (def => def.Code == toFind.Code && def.Source == toFind.Source));
    }

    bool ISpecialFeatureCodeManager.Activate(SpecialFeatureCodeDefinition toActivate)
    {
      SpecialFeatureCodeDefinition matching = this.FindMatching(toActivate);
      bool flag = matching != null;
      if (flag)
        matching.Status = SpecialFeatureCodeDefinitionStatus.Active;
      return flag;
    }

    bool ISpecialFeatureCodeManager.Add(SpecialFeatureCodeDefinition toAdd)
    {
      bool flag = this.FindMatching(toAdd) == null;
      if (flag)
        this.List.Add(toAdd);
      return flag;
    }

    bool ISpecialFeatureCodeManager.Deactivate(SpecialFeatureCodeDefinition toDeactivate)
    {
      SpecialFeatureCodeDefinition matching = this.FindMatching(toDeactivate);
      bool flag = matching != null;
      if (flag)
        matching.Status = SpecialFeatureCodeDefinitionStatus.None;
      return flag;
    }

    bool ISpecialFeatureCodeManager.Delete(SpecialFeatureCodeDefinition toDelete)
    {
      SpecialFeatureCodeDefinition matching = this.FindMatching(toDelete);
      bool flag = matching != null;
      if (flag)
        this.List.Remove(matching);
      return flag;
    }

    IList<SpecialFeatureCodeDefinition> ISpecialFeatureCodeManager.GetAll() => this.List;

    IList<SpecialFeatureCodeDefinition> ISpecialFeatureCodeManager.GetActive()
    {
      return (IList<SpecialFeatureCodeDefinition>) this.List.Where<SpecialFeatureCodeDefinition>((Func<SpecialFeatureCodeDefinition, bool>) (sfcDef => sfcDef.Status == SpecialFeatureCodeDefinitionStatus.Active)).ToList<SpecialFeatureCodeDefinition>();
    }

    bool ISpecialFeatureCodeManager.Update(SpecialFeatureCodeDefinition toUpdate)
    {
      SpecialFeatureCodeDefinition matching = this.FindMatching(toUpdate);
      bool flag = matching != null;
      if (flag)
      {
        matching.Code = toUpdate.Code;
        matching.Comment = toUpdate.Comment;
        matching.Description = toUpdate.Description;
        matching.Source = toUpdate.Source;
        matching.Status = toUpdate.Status;
      }
      return flag;
    }

    bool ISpecialFeatureCodeManager.IsUsedinFieldTriggerRule(string sfcId)
    {
      throw new NotImplementedException();
    }
  }
}
