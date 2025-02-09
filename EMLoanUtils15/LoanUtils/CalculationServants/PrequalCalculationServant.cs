// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.PrequalCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class PrequalCalculationServant(ILoanModelProvider modelProvider) : CalculationServantBase(modelProvider)
  {
    public void CalculateMiddleFico(string id)
    {
      int middleFico = Utils.FindMiddleFico(new int[3]
      {
        Utils.GetFICONumber(this.Val("67")),
        Utils.GetFICONumber(this.Val("1450")),
        Utils.GetFICONumber(this.Val("1414"))
      });
      this.SetCurrentNum("2849", (double) middleFico);
      string str1 = this.Val("USDA.X12");
      if (str1 != "Y" && (middleFico > 0 || id == "67" || id == "1450" || id == "1414"))
        this.SetVal("USDA.X194", middleFico.ToString());
      if (str1 == "Y")
        this.SetVal("USDA.X194", "");
      middleFico = Utils.FindMiddleFico(new int[3]
      {
        Utils.GetFICONumber(this.Val("60")),
        Utils.GetFICONumber(this.Val("1452")),
        Utils.GetFICONumber(this.Val("1415"))
      });
      this.SetCurrentNum("2850", (double) middleFico);
      string str2 = this.Val("USDA.X15");
      if (str2 != "Y" && (middleFico > 0 || id == "60" || id == "1452" || id == "1415"))
        this.SetVal("USDA.X195", middleFico.ToString());
      if (!(str2 == "Y"))
        return;
      this.SetVal("USDA.X195", "");
    }
  }
}
