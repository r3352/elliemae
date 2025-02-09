// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.NMLSCalculations
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public static class NMLSCalculations
  {
    public static string CalculateNMLSLoanType(
      string field19,
      string field1172,
      string field420,
      string field16)
    {
      if (field1172 == "HELOC")
        return "HELOC";
      if (Utils.ParseInt((object) field16, 1) > 4)
        return "";
      if (field19 == "ConstructionOnly" || field19 == "ConstructionToPermanent")
        return "Construction";
      switch (field420)
      {
        case "SecondLien":
          return "ClosedEndSecond";
        case "FirstLien":
          return "ResidentialFirst";
        default:
          return "";
      }
    }

    public static string CalculateNMLSResidentialMortgageType(
      string field1172,
      string field608,
      string field3331)
    {
      bool flag1 = field1172 == "VA" || field1172 == "FHA" || field1172 == "FarmersHomeAdministration";
      bool flag2 = field608 == "Fixed" || field608 == "GraduatedPaymentMortgage";
      bool flag3 = field608 == "AdjustableRate";
      if (field1172 == "" || field608 == "" || field1172 == "HELOC")
        return "";
      if (flag1 & flag2)
        return "GovtFixed";
      if (flag1 & flag3)
        return "GovtARM";
      if (flag1)
        return "";
      if (field1172 == "Other" & flag2)
        return "OtherFixed";
      if (field1172 == "Other" & flag3)
        return "OtherARM";
      if (field3331 == "Conforming" & flag2)
        return "PrimeConformingFixed";
      if (field3331 == "Conforming" & flag3)
        return "PrimeConformingARM";
      if (field3331 == "Jumbo" & flag2)
        return "PrimeJumboFixed";
      return field3331 == "Jumbo" & flag3 ? "PrimeJumboARM" : "";
    }

    public static string CalculateNMLSDocType(string fieldMORNETX67, string fieldCASASRNX144)
    {
      if (fieldMORNETX67 != "")
        return fieldMORNETX67 == "FullDocumentation" || fieldMORNETX67 == "(F) Full Documentation" ? "Full" : "Alt";
      if (!(fieldCASASRNX144 != ""))
        return "";
      return fieldCASASRNX144 == "Z" ? "Full" : "Alt";
    }

    public static string CalculateNMLSARMType(string field608)
    {
      switch (field608)
      {
        case "":
          return "";
        case "AdjustableRate":
          return "";
        default:
          return "N";
      }
    }

    public static string CalculateNMLSSecondLienType(string field428, string field1732)
    {
      return Utils.ParseDecimal((object) field428, 0M) > 0M || Utils.ParseDecimal((object) field1732, 0M) > 0M ? "Y" : "N";
    }

    public static string CalculateNMLSRefiPurpose(string field19, string field299)
    {
      if (field19 == "NoCash-Out Refinance" || field19 == "Cash-Out Refinance")
      {
        switch (field299)
        {
          case "CashOutDebtConsolidation":
          case "CashOutHomeImprovement":
          case "CashOutLimited":
          case "CashOutOriginalLender":
          case "CashOutOther":
            return "RefiCashOut";
          case "ChangeInRateTerm":
          case "NoCashOutFHAStreamlinedRefinance":
          case "NoCashOutFREOwnedRefinance":
          case "NoCashOutOriginalLender":
          case "NoCashOutOther":
          case "NoCashOutStreamlinedRefinance":
            return "RefiRateTerm";
          default:
            return "";
        }
      }
      else
        return field19 != "" ? "Purchase" : "";
    }

    public static string CalculateLenderChannel(string field2626)
    {
      switch (field2626)
      {
        case "Banked - Retail":
        case "Brokered":
          return "Retail";
        case "Banked - Wholesale":
          return "Broker";
        case "Correspondent":
          return "Correspondent";
        default:
          return "";
      }
    }

    public static string CalculateOccupancyType(string field1811)
    {
      if (field1811 == "PrimaryResidence" || field1811 == "SecondHome")
        return "OwnerOccupied";
      return field1811 != "" ? "NonOwnerOccupied" : "";
    }

    public static string CalculatePMIIndicator(string field1766, string field1045)
    {
      return Utils.ParseDecimal((object) field1766, 0M) > 0M || Utils.ParseDecimal((object) field1045, 0M) > 0M ? "Y" : "N";
    }

    public static AmortizationType GetNMLSAmortizationType(string field608)
    {
      switch (field608)
      {
        case "Fixed":
        case "GraduatedPaymentMortgage":
          return AmortizationType.FixedRate;
        case "AdjustableRate":
          return AmortizationType.ARM;
        default:
          return AmortizationType.Blank;
      }
    }
  }
}
