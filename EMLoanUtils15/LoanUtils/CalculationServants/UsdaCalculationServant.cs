// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.UsdaCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class UsdaCalculationServant(ILoanModelProvider modelProvider) : CalculationServantBase(modelProvider)
  {
    public void ClearMipFields(string id, string val)
    {
      this.SetVal("1766", "");
      this.SetVal("1760", "");
      this.SetVal("1107", "");
      this.SetVal("1199", "");
      this.SetVal("1198", "");
      this.SetVal("1205", "");
      this.SetVal("NEWHUD.X1707", "");
      this.SetVal("232", "");
      this.SetVal("1200", "");
      this.SetVal("1201", "");
      this.SetVal("NEWHUD.X1706", "");
      this.SetVal("1765", "");
      this.SetVal("3531", "");
      this.SetVal("3532", "");
      this.SetVal("3533", "");
      this.SetVal("3265", "");
      for (int index = 3560; index <= 3566; ++index)
        this.SetVal(string.Concat((object) index), "");
      string str = this.Val("1172");
      if (str != "FarmersHomeAdministration")
      {
        this.SetVal("HUD.YearlyUSDAFee", "");
        this.SetVal("HUD50", "");
        this.SetVal("3551", "");
      }
      else if (str == "FarmersHomeAdministration")
        this.SetVal("HUD43", "");
      if (!(str == "FHA"))
        return;
      this.SetVal("1209", "");
      this.SetVal("2978", "");
    }
  }
}
