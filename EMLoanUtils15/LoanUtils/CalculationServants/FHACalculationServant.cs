// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.FHACalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class FHACalculationServant(ILoanModelProvider modelProvider) : CalculationServantBase(modelProvider)
  {
    private const string ClassName = "FHACalculationServant�";

    public void CalculateMCAWRefinance(string id, string val)
    {
      if (!(id == "1134"))
        return;
      if (this.Val("202") == "" && this.Val("141") == "" || this.Val("202") == "MIPremiumRefund")
      {
        if (val == string.Empty)
          this.SetVal("202", "");
        else
          this.SetVal("202", "MIPremiumRefund");
        this.SetVal("141", val);
      }
      else if (this.Val("1091") == "" && this.Val("1095") == "" || this.Val("1091") == "MIPremiumRefund")
      {
        if (val == string.Empty)
          this.SetVal("1091", "");
        else
          this.SetVal("1091", "MIPremiumRefund");
        this.SetVal("1095", val);
      }
      else if (this.Val("1106") == "" && this.Val("1115") == "" || this.Val("1106") == "MIPremiumRefund")
      {
        if (val == string.Empty)
          this.SetVal("1106", "");
        else
          this.SetVal("1106", "MIPremiumRefund");
        this.SetVal("1115", val);
      }
      else
      {
        if ((!(this.Val("1646") == "") || !(this.Val("1647") == "")) && !(this.Val("1646") == "MIPremiumRefund"))
          return;
        if (val == string.Empty)
          this.SetVal("1646", "");
        else
          this.SetVal("1646", "MIPremiumRefund");
        this.SetVal("1647", val);
      }
    }
  }
}
