// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ChangeOfCircumstanceItems
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class ChangeOfCircumstanceItems : IChangeOfCircumstanceItems
  {
    private List<COCCollection> cocCollections = new List<COCCollection>();
    private string changedCircumstances = string.Empty;
    private string feeLevelIndicator = string.Empty;

    internal ChangeOfCircumstanceItems(
      List<COCCollection> cocCollection,
      string cocChangedCircumstances,
      string cocFeeLevelIndicator)
    {
      this.cocCollections = cocCollection;
      this.changedCircumstances = cocChangedCircumstances;
      this.feeLevelIndicator = cocFeeLevelIndicator;
    }

    public List<COCCollection> COCCollections => this.cocCollections;

    public string ChangedCircumstances => this.changedCircumstances;

    public string FeeLevelIndicator => this.feeLevelIndicator;
  }
}
