// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.CloserCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class CloserCalculationServant(ILoanModelProvider modelProvider) : CalculationServantBase(modelProvider)
  {
    public void CalculateCloserOthers(string id, string val)
    {
      if (!(id == "1172"))
        return;
      switch (val.ToLower())
      {
        case "conventional":
          this.SetVal("L83", "Uninsured");
          break;
        case "farmershomeadministration":
          this.SetVal("L83", "Rural Housing Service");
          break;
        case "fha":
          this.SetVal("L83", "FHA");
          break;
        case "va":
          this.SetVal("L83", "VA");
          break;
      }
    }
  }
}
