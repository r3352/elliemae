// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ChangeOfCircumstanceItems
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>
  /// Class to get the Change of Circumstances attribute from the current Disclosure Tracking log 2015
  /// </summary>
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

    /// <summary>The coc Collection based on the Instance / Order ID</summary>
    public List<COCCollection> COCCollections => this.cocCollections;

    /// <summary>COC changed Circumstances</summary>
    public string ChangedCircumstances => this.changedCircumstances;

    /// <summary>COC Fee Level Indicator</summary>
    public string FeeLevelIndicator => this.feeLevelIndicator;
  }
}
