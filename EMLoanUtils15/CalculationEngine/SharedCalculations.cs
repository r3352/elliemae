// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.SharedCalculations
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class SharedCalculations
  {
    public static void Execute(
      IHtmlInput loan,
      SharedCalculations.SharedCalculationType calcType,
      string triggerFieldID,
      string val)
    {
      if (calcType != SharedCalculations.SharedCalculationType.HELOC)
        return;
      SharedCalculations.executeHELOCSharedCalculations(loan, triggerFieldID, val);
    }

    private static void executeHELOCSharedCalculations(IHtmlInput loan, string id, string val)
    {
      if (id == "1172" || id == null)
      {
        int num = loan.GetField("1172") == "HELOC" ? 1 : 0;
      }
      if ((id == "4586" || id == null) && val != "Y")
        loan.SetField("4587", string.Empty);
      if (id == "4600" && val != "Y")
      {
        loan.SetField("4603", "N");
        loan.SetField("4604", "N");
        loan.SetField("4605", string.Empty);
        loan.SetField("4606", string.Empty);
      }
      if (id == "4603" && val != "Y")
        loan.SetField("4605", string.Empty);
      if (id == "4604" && val != "Y")
        loan.SetField("4606", string.Empty);
      if (id == "4612" && val != "Y")
        loan.SetField("4613", "N");
      if (id == "4557" && val != "Y")
      {
        loan.SetField("4614", string.Empty);
        loan.SetField("4615", string.Empty);
        loan.SetField("4616", "N");
        loan.SetField("4617", string.Empty);
        loan.SetField("4618", string.Empty);
        loan.SetField("4619", string.Empty);
        loan.SetField("4592", string.Empty);
        loan.SetField("4577", string.Empty);
        loan.SetField("4578", string.Empty);
        loan.SetField("4579", string.Empty);
        loan.SetField("4580", string.Empty);
        loan.SetField("4581", string.Empty);
        loan.SetField("SYS.X2", string.Empty);
        loan.SetField("4582", string.Empty);
        loan.SetField("4583", string.Empty);
        loan.SetField("4584", string.Empty);
        loan.SetField("4585", string.Empty);
      }
      if (id == "4616" && val != "Y")
        loan.SetField("4617", string.Empty);
      if (!(id == "4621") || !(val != "Y"))
        return;
      loan.SetField("4622", string.Empty);
    }

    public static bool IsInsuranceOrTax(string desc)
    {
      return SharedCalculations.IsInsuranceFee(desc) || SharedCalculations.IsTaxFee(desc);
    }

    public static bool IsInsuranceFee(string desc)
    {
      desc = desc.ToLower();
      return desc.IndexOf("insurance") > -1;
    }

    public static bool IsTaxFee(string desc)
    {
      desc = desc.ToLower();
      return desc.IndexOf("tax") > -1;
    }

    public static bool UseNewVAIRRRL(
      string f1172,
      string f958,
      string f1887,
      string f2553,
      string f748,
      string f763)
    {
      DateTime date = Utils.ParseDate((object) "08/08/2019");
      if (!(f1172 == "VA") || !(f958 == "IRRRL"))
        return false;
      if (DateTime.Compare(Utils.ParseDate((object) f1887), date) >= 0 || DateTime.Compare(Utils.ParseDate((object) f2553), date) >= 0 || DateTime.Compare(Utils.ParseDate((object) f748), date) >= 0 || DateTime.Compare(Utils.ParseDate((object) f763), date) >= 0)
        return true;
      return (f1887 == "" || f1887 == "//") && (f2553 == "" || f2553 == "//") && (f748 == "" || f748 == "//") && (f763 == "" || f763 == "//") && DateTime.Compare(DateTime.Today, date) >= 0;
    }

    public enum SharedCalculationType
    {
      HELOC,
    }
  }
}
