// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.ULDDExportCalculationServant
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public class ULDDExportCalculationServant(ILoanModelProvider modelProvider) : 
    CalculationServantBase(modelProvider)
  {
    public void CalculateFannieMaeExportFields(string id, string val)
    {
      if (string.IsNullOrEmpty(id))
        return;
      string upper = id.ToUpper();
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(upper))
      {
        case 2439456:
          if (!(upper == "1149") || !string.IsNullOrEmpty(this.Val("ULDD.GNM.MortgageOriginator")))
            return;
          switch (val)
          {
            case "ThirdParty":
              this.SetVal("ULDD.GNM.MortgageOriginator", "Broker");
              return;
            case "Seller":
              this.SetVal("ULDD.GNM.MortgageOriginator", "Lender");
              return;
            case "Correspondent":
              this.SetVal("ULDD.GNM.MortgageOriginator", "Correspondent");
              return;
            default:
              return;
          }
        case 115504067:
          if (!(upper == "ULDD.X173") || !Utils.IsInt((object) this.Val("ULDD.X173")))
            return;
          if (Utils.ParseInt((object) this.Val("ULDD.X173")) < 0 || Utils.ParseInt((object) this.Val("ULDD.X173")) > 999)
          {
            this.SetVal("ULDD.X173", "");
            return;
          }
          this.SetVal("ULDD.X173", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
          return;
        case 215184043:
          if (!(upper == "ULDD.X119") || !(this.Val("ULDD.X119") != "Other"))
            return;
          this.SetVal("ULDD.X121", "");
          return;
        case 227960804:
          if (!(upper == "ULDD.FRE.DOWNPMT2TYPE") || !(val != "OtherTypeOfDownPayment"))
            return;
          this.SetVal("ULDD.FRE.DownPmt2TypeExpl", "");
          return;
        case 266502638:
          if (!(upper == "ULDD.X178"))
            return;
          this.UpdateFloodSpecialFeatureCode(val, this.Val("676"));
          return;
        case 283280257:
          int num1 = upper == "ULDD.X179" ? 1 : 0;
          return;
        case 292307274:
          if (!(upper == "ULDD.FNM.1859"))
            return;
          goto label_252;
        case 350953279:
          if (!(upper == "19"))
            return;
          switch (val)
          {
            case "Cash-Out Refinance":
              this.SetVal("ULDD.X18", "CashOut");
              this.SetVal("ULDD.X180", "CashOut");
              break;
            case "NoCash-Out Refinance":
              if (this.Val("1172") == "Conventional" || this.Val("1172") == "Other" || this.Val("1172") == "HELOC")
                this.SetVal("ULDD.X18", "LimitedCashOut");
              if (this.Val("1172") == "FHA" || this.Val("1172") == "VA" || this.Val("1172") == "FarmersHomeAdministration" || string.IsNullOrEmpty(this.Val("1172")))
                this.SetVal("ULDD.X18", "NoCashOut");
              this.SetVal("ULDD.X180", "NoCashOut");
              break;
            default:
              this.SetVal("ULDD.X18", "");
              this.SetVal("ULDD.X180", "");
              break;
          }
          this.UpdateBaseLTVRatioPercent();
          return;
        case 405301056:
          int num2 = upper == "430" ? 1 : 0;
          return;
        case 647630759:
          if (!(upper == "1012"))
            return;
          switch (val)
          {
            case "E_PUD":
              this.SetVal("ULDD.X142", "E");
              return;
            case "F_PUD":
              this.SetVal("ULDD.X142", "F");
              return;
            case "FannieMaeReviewPERSCoopProject":
              this.SetVal("ULDD.X142", "2");
              return;
            case "FullReviewCoopProject":
              this.SetVal("ULDD.X142", "1");
              return;
            case "G_NotInAProjectOrDevelopment":
              this.SetVal("ULDD.X142", "G");
              return;
            case "P_LimitedReviewNew":
              this.SetVal("ULDD.X142", "P");
              return;
            case "Q_LimitedReviewEst":
              this.SetVal("ULDD.X142", "Q");
              return;
            case "R_ExpeditedNew":
              this.SetVal("ULDD.X142", "R");
              return;
            case "S_ExpeditedEst":
              this.SetVal("ULDD.X142", "S");
              return;
            case "T_FannieMaeReview":
              this.SetVal("ULDD.X142", "T");
              return;
            case "U_FHAapproved":
              this.SetVal("ULDD.X142", "U");
              return;
            case "V_NoReviewSiteCondo":
            case "V_RefiPlus":
              this.SetVal("ULDD.X142", "V");
              return;
            default:
              this.SetVal("ULDD.X142", "");
              return;
          }
        case 669660000:
          if (!(upper == "232"))
            return;
          if (Utils.IsDecimal((object) val))
          {
            this.SetVal("ULDD.GNM.GovAnnlPrmAmt", (Utils.ParseDecimal((object) val) * 12M).ToString());
            return;
          }
          this.SetVal("ULDD.GNM.GovAnnlPrmAmt", "");
          return;
        case 749624177:
          if (!(upper == "ULDD.FRE.DOWNPMT2SOURCETYPE") || !(val != "Other"))
            return;
          this.SetVal("ULDD.FRE.DownPmt2SourceTypeExpl", "");
          return;
        case 792024936:
          if (!(upper == "467") || !(this.Val("467") != "N"))
            return;
          this.SetVal("ULDD.X148", "");
          this.SetVal("4710", "");
          return;
        case 800856262:
          if (!(upper == "1859"))
            return;
          goto label_252;
        case 808802555:
          if (!(upper == "466") || !(this.Val("466") != "N"))
            return;
          this.SetVal("ULDD.X123", "");
          this.SetVal("4709", "");
          return;
        case 868566362:
          if (!(upper == "ULDD.FRE.DOWNPMT3SOURCETYPE") || !(val != "Other"))
            return;
          this.SetVal("ULDD.FRE.DownPmt3SourceTypeExpl", "");
          return;
        case 900716976:
          if (!(upper == "ULDD.X11") || !(this.Val("ULDD.X11") != "Simple"))
            return;
          this.SetVal("ULDD.X55", "");
          return;
        case 1024742963:
          if (!(upper == "SERVICE.X32"))
            return;
          if (Utils.IsDate((object) this.Val("SERVICE.X32")))
          {
            this.SetVal("ULDD.X53", this.Val("SERVICE.X32"));
            return;
          }
          this.SetVal("ULDD.X53", "");
          return;
        case 1050435619:
          if (!(upper == "ULDD.X98") || !Utils.IsInt((object) this.Val("ULDD.X98")) || Utils.ParseInt((object) this.Val("ULDD.X98")) >= 0 && Utils.ParseInt((object) this.Val("ULDD.X98")) <= 999)
            return;
          this.SetVal("ULDD.X98", "");
          return;
        case 1076061558:
          if (!(upper == "SERVICE.X57"))
            return;
          if (Utils.IsDecimal((object) this.Val("SERVICE.X57")))
          {
            this.SetVal("ULDD.X1", this.Val("SERVICE.X57"));
            return;
          }
          this.SetVal("ULDD.X1", "");
          return;
        case 1096595161:
          if (!(upper == "356"))
            return;
          goto label_351;
        case 1196275137:
          if (!(upper == "338"))
            return;
          break;
        case 1272069539:
          if (!(upper == "934"))
            return;
          goto label_338;
        case 1276701438:
          if (!(upper == "SERVICE.X13"))
            return;
          break;
        case 1321563753:
          if (!(upper == "965") || !(this.Val("965") != "N"))
            return;
          this.SetVal("ULDD.X123", "");
          this.SetVal("4709", "");
          return;
        case 1327034295:
          if (!(upper == "SERVICE.X14"))
            return;
          break;
        case 1501788124:
          if (!(upper == "ULDD.FNM.X70"))
            return;
          goto label_239;
        case 1523042276:
          if (!(upper == "975"))
            return;
          goto label_236;
        case 1703413742:
          if (!(upper == "ULDD.FNM.X50"))
            return;
          goto label_238;
        case 1741391264:
          if (!(upper == "1264"))
            return;
          this.SetVal("ULDD.X184", val);
          return;
        case 1821004170:
          if (!(upper == "ULDD.FNM.X43"))
            return;
          goto label_237;
        case 1988490412:
          if (!(upper == "694"))
            return;
          this.SetVal("ULDD.X167", val);
          return;
        case 2000333201:
          if (!(upper == "136"))
            return;
          goto label_351;
        case 2005268031:
          if (!(upper == "695"))
            return;
          this.SetVal("ULDD.X169", val);
          return;
        case 2022045650:
          if (!(upper == "696"))
            return;
          this.SetVal("ULDD.X6", val);
          this.SetVal("ULDD.X59", val);
          return;
        case 2024944435:
          if (!(upper == "SYS.X319"))
            return;
          break;
        case 2031986902:
          if (!(upper == "1172"))
            return;
          if (this.Val("1172") != "Other")
          {
            this.SetVal("ULDD.X153", "");
            this.SetVal("ULDD.FNM.X1172", this.Val("1172"));
          }
          else
            this.SetVal("ULDD.FNM.X1172", "");
          if (this.Val("1172") != "HELOC")
            this.SetVal("ULDD.FRE.X1172", this.Val("1172"));
          else
            this.SetVal("ULDD.FRE.X1172", "");
          if (this.Val("19") == "Cash-Out Refinance")
          {
            this.SetVal("ULDD.X18", "CashOut");
            this.SetVal("ULDD.X180", "CashOut");
          }
          else if (this.Val("19") == "NoCash-Out Refinance")
          {
            if (this.Val("1172") == "Conventional" || this.Val("1172") == "Other" || this.Val("1172") == "HELOC")
              this.SetVal("ULDD.X18", "LimitedCashOut");
            if (this.Val("1172") == "FHA" || this.Val("1172") == "VA" || this.Val("1172") == "FarmersHomeAdministration" || string.IsNullOrEmpty(this.Val("1172")))
              this.SetVal("ULDD.X18", "NoCashOut");
            this.SetVal("ULDD.X180", "NoCashOut");
          }
          else
          {
            this.SetVal("ULDD.X18", "");
            this.SetVal("ULDD.X180", "");
          }
          switch (val)
          {
            case "FHA":
              this.SetVal("ULDD.GNM.X1172", "FHA");
              return;
            case "VA":
              this.SetVal("ULDD.GNM.X1172", "VA");
              return;
            case "FarmersHomeAdministration":
              this.SetVal("ULDD.GNM.X1172", "USDARuralDevelopment");
              return;
            default:
              this.SetVal("ULDD.GNM.X1172", "");
              return;
          }
        case 2038823269:
          if (!(upper == "697"))
            return;
          this.SetVal("ULDD.X61", val);
          return;
        case 2081586604:
          int num3 = upper == "ULDD.FNM.430" ? 1 : 0;
          return;
        case 2105305235:
          if (!(upper == "ULDD.FRE.DOWNPMT4SOURCETYPE") || !(val != "Other"))
            return;
          this.SetVal("ULDD.FRE.DownPmt4SourceTypeExpl", "");
          return;
        case 2144816681:
          if (!(upper == "HMDA.X15"))
            return;
          goto label_234;
        case 2278978275:
          if (!(upper == "ULDD.X102") || !(this.Val("ULDD.X102") != "Other"))
            return;
          this.SetVal("ULDD.X103", "");
          return;
        case 2284784020:
          if (!(upper == "1109"))
            return;
          goto label_351;
        case 2292753220:
          if (!(upper == "676"))
            return;
          this.UpdateFloodSpecialFeatureCode(this.Val("ULDD.X178"), val);
          return;
        case 2341331635:
          if (!(upper == "1041"))
            return;
          switch (val)
          {
            case "Attached":
              this.SetVal("ULDD.X177", "Attached");
              return;
            case "Detached":
              this.SetVal("ULDD.X177", "Detached");
              return;
            default:
              return;
          }
        case 2346088751:
          if (!(upper == "ULDD.X106") || !(this.Val("ULDD.X106") != "Other"))
            return;
          this.SetVal("ULDD.X107", "");
          return;
        case 2346235846:
          if (!(upper == "ULDD.X134") || !(this.Val("ULDD.X134") != "Other"))
            return;
          this.SetVal("ULDD.X135", "");
          this.SetVal("ULDD.X136", "");
          return;
        case 2347784130:
          if (!(upper == "34"))
            return;
          string str1 = "";
          string str2 = "";
          switch (val)
          {
            case "BridgeLoan":
              str1 = "BridgeLoan";
              str2 = "BridgeLoan";
              break;
            case "CashOnHand":
              str1 = "CashOnHand";
              str2 = "CashOnHand";
              break;
            case "CheckingSavings":
              str1 = "CheckingSavings";
              str2 = "CheckingSavings";
              break;
            case "DepositOnSalesContract":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "EquityOnPendingSale":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "EquityOnSoldProperty":
              str2 = "EquityOnSoldProperty";
              break;
            case "EquityOnSubjectProperty":
              str1 = "EquityOnSubjectProperty";
              str2 = "EquityOnSubjectProperty";
              break;
            case "FHAGiftSourceEmployer":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "FHAGiftSourceGovernmentAssistance":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "FHAGiftSourceNA":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "FHAGiftSourceNonprofitNonSellerFunded":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "FHAGiftSourceNonprofitSellerFunded":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "FHAGiftSourceRelative":
              str2 = "OtherTypeOfDownPayment";
              break;
            case "GiftFunds":
              str1 = "GiftFunds";
              str2 = "GiftFunds";
              break;
            case "LifeInsuranceCashValue":
              str1 = "LifeInsuranceCashValue";
              str2 = "LifeInsuranceCashValue";
              break;
            case "LotEquity":
              str1 = "LotEquity";
              str2 = "LotEquity";
              break;
            case "OtherTypeOfDownPayment":
              str1 = "Other";
              str2 = "OtherTypeOfDownPayment";
              break;
            case "RentWithOptionToPurchase":
              str1 = "RentWithOptionToPurchase";
              str2 = "RentWithOptionToPurchase";
              break;
            case "RetirementFunds":
              str1 = "RetirementFunds";
              str2 = "RetirementFunds";
              break;
            case "SaleOfChattel":
              str1 = "SaleOfChattel";
              str2 = "SaleOfChattel";
              break;
            case "SecuredBorrowedFunds":
              str2 = "SecuredBorrowedFunds";
              break;
            case "StocksAndBonds":
              str1 = "StocksAndBonds";
              str2 = "StocksAndBonds";
              break;
            case "SweatEquity":
              str1 = "SweatEquity";
              str2 = "SweatEquity";
              break;
            case "TradeEquity":
              str1 = "TradeEquity";
              str2 = "TradeEquity";
              break;
            case "TrustFunds":
              str1 = "TrustFunds";
              str2 = "TrustFunds";
              break;
            case "UnsecuredBorrowedFunds":
              str1 = "UnsecuredBorrowedFunds";
              str2 = "UnsecuredBorrowedFunds";
              break;
          }
          this.SetVal("ULDD.X106", str1);
          this.SetVal("ULDD.FRE.DownPmt3Type", str2);
          if (!(val != "OtherTypeOfDownPayment"))
            return;
          this.SetVal("ULDD.FRE.DownPmt3TypeExpl", "");
          return;
        case 2446754465:
          if (!(upper == "ULDD.X108") || !(this.Val("ULDD.X108") != "Other"))
            return;
          this.SetVal("ULDD.X109", "");
          return;
        case 2514159131:
          if (!(upper == "ULDD.X120") || !(this.Val("ULDD.X120") != "Other"))
            return;
          this.SetVal("ULDD.X122", "");
          return;
        case 2599032964:
          if (!(upper == "ULDD.X187"))
            return;
          if (val != "Other")
            this.SetVal("ULDD.X188", "");
          if (val == "Manufactured")
          {
            this.SetVal("603", "Y");
            return;
          }
          this.SetVal("603", "");
          return;
        case 2770305142:
          if (!(upper == "ULDD.FNM.4005"))
            return;
          goto label_249;
        case 2775311943:
          if (!(upper == "ULDD.FRE.DOWNPMT3TYPE") || !(val != "OtherTypeOfDownPayment"))
            return;
          this.SetVal("ULDD.FRE.DownPmt3TypeExpl", "");
          return;
        case 2787082761:
          if (!(upper == "ULDD.FNM.4004"))
            return;
          goto label_243;
        case 2837415618:
          if (!(upper == "ULDD.FNM.4001"))
            return;
          goto label_246;
        case 2854193237:
          if (!(upper == "ULDD.FNM.4000"))
            return;
          goto label_240;
        case 2872052334:
          if (!(upper == "403"))
            return;
          goto label_338;
        case 3069768554:
          if (!(upper == "2847") || !(this.Val("2847") != "C"))
            return;
          this.SetVal("ULDD.X152", "");
          return;
        case 3083347921:
          if (!(upper == "1821"))
            return;
          goto label_351;
        case 3130610112:
          if (!(upper == "ULDD.X86") || !Utils.IsInt((object) this.Val("ULDD.X86")) || Utils.ParseInt((object) this.Val("ULDD.X86")) >= 1 && Utils.ParseInt((object) this.Val("ULDD.X86")) <= 31)
            return;
          this.SetVal("ULDD.X86", "");
          return;
        case 3130885846:
          if (!(upper == "ULDD.GNM.DWNPYMNTFNDSTYPE") || !(val != "Other"))
            return;
          this.SetVal("ULDD.GNM.OtherDwnPymntFndsType", "");
          return;
        case 3171440836:
          if (!(upper == "MORNET.X77"))
            return;
          goto label_233;
        case 3181928707:
          if (!(upper == "ULDD.X29") || !(this.Val("ULDD.X29") != "Other"))
            return;
          this.SetVal("ULDD.X190", "");
          return;
        case 3188218455:
          if (!(upper == "MORNET.X76"))
            return;
          goto label_232;
        case 3204996074:
          if (!(upper == "MORNET.X75"))
            return;
          goto label_235;
        case 3266655445:
          if (!(upper == "ULDD.X70"))
            return;
          goto label_239;
        case 3307360144:
          if (!(upper == "ULDD.X208") || !(val != "Other"))
            return;
          this.SetVal("ULDD.X210", "");
          return;
        case 3347792069:
          if (!(upper == "ULDD.FNM.HMDA.X15"))
            return;
          goto label_234;
        case 3350690635:
          if (!(upper == "ULDD.X43"))
            return;
          goto label_237;
        case 3366482516:
          if (!(upper == "ULDD.X24") || !(this.Val("ULDD.X24") != "Other"))
            return;
          this.SetVal("ULDD.X25", "");
          return;
        case 3366629611:
          if (!(upper == "ULDD.X32") || !(val != "Other"))
            return;
          this.SetVal("ULDD.FRE.AVMModelNameExpl", "");
          return;
        case 3382274397:
          if (!(upper == "ULDD.X89") || !(this.Val("ULDD.X89") != "Other"))
            return;
          this.SetVal("ULDD.X90", "");
          return;
        case 3403562627:
          if (!(upper == "985") || !(this.Val("985") != "N"))
            return;
          this.SetVal("ULDD.X148", "");
          this.SetVal("4710", "");
          return;
        case 3420489784:
          if (!(upper == "4973"))
            return;
          goto label_338;
        case 3433740087:
          int num4 = upper == "ULDD.X36" ? 1 : 0;
          return;
        case 3444917368:
          if (!(upper == "364"))
            return;
          this.SetVal("ULDD.X21", val);
          return;
        case 3451356349:
          if (!(upper == "ULDD.X45") || !Utils.IsInt((object) this.Val("ULDD.X45")) || Utils.ParseInt((object) this.Val("ULDD.X45")) >= 1 && Utils.ParseInt((object) this.Val("ULDD.X45")) <= 31)
            return;
          this.SetVal("ULDD.X45", "");
          return;
        case 3451503444:
          if (!(upper == "ULDD.X51") || !(this.Val("ULDD.X51") != "Other"))
            return;
          this.SetVal("ULDD.X52", "");
          return;
        case 3468281063:
          if (!(upper == "ULDD.X50"))
            return;
          goto label_238;
        case 3478663961:
          if (!(upper == "ULDD.FRE.DOWNPAYMENTTYPE") || !(val != "OtherTypeOfDownPayment"))
            return;
          this.SetVal("ULDD.FRE.ExplanationofDownpayment", "");
          return;
        case 3487533608:
          if (!(upper == "ULDD.FNM.975"))
            return;
          goto label_236;
        case 3508838667:
          if (!(upper == "ULDD.X212") || !(val != "Third Party"))
            return;
          this.SetVal("ULDD.X214", "");
          return;
        case 3525616286:
          if (!(upper == "ULDD.X211") || !(val != "Third Party"))
            return;
          this.SetVal("ULDD.X213", "");
          return;
        case 3537933117:
          if (!(upper == "4974"))
            return;
          goto label_338;
        case 3550349406:
          if (!(upper == "4001"))
            return;
          goto label_246;
        case 3559024429:
          if (!(upper == "ULDD.X207") || !(val != "Other"))
            return;
          this.SetVal("ULDD.X209", "");
          return;
        case 3567127025:
          if (!(upper == "4000"))
            return;
          goto label_240;
        case 3605733003:
          if (!(upper == "1553"))
            return;
          switch (this.Val("1553"))
          {
            case "Manufactured Housing Single Wide":
            case "Manufactured Housing Multiwide":
              this.SetVal("ULDD.X144", "");
              this.SetVal("ULDD.X182", "");
              return;
            case "Condominium":
              this.SetVal("ULDD.X182", "Condominium");
              return;
            case "Cooperative":
              this.SetVal("ULDD.X182", "Cooperative");
              return;
            default:
              this.SetVal("ULDD.X182", "");
              return;
          }
        case 3614548598:
          if (!(upper == "ULDD.FRE.DOWNPMT4TYPE") || !(val != "OtherTypeOfDownPayment"))
            return;
          this.SetVal("ULDD.FRE.DownPmt4TypeExpl", "");
          return;
        case 3617459882:
          if (!(upper == "4005"))
            return;
          goto label_249;
        case 3634237501:
          if (!(upper == "4004"))
            return;
          goto label_243;
        case 3740101050:
          if (!(upper == "1543") || !(this.Val("1543") != "Other"))
            return;
          this.SetVal("ULDD.X149", "");
          this.SetVal("1556", "");
          return;
        case 4132588608:
          if (!(upper == "ULDD.FNM.MORNET.X77"))
            return;
          goto label_233;
        case 4149366227:
          if (!(upper == "ULDD.FNM.MORNET.X76"))
            return;
          goto label_232;
        case 4166143846:
          if (!(upper == "ULDD.FNM.MORNET.X75"))
            return;
          goto label_235;
        default:
          return;
      }
      if (Utils.ParseInt((object) this.Val("338")) <= 0)
        return;
      if (this.Val("SYS.X319") == "")
      {
        this.SetVal("ULDD.X49", "Borrower");
        return;
      }
      if (this.Val("SYS.X319") == "Lender")
      {
        this.SetVal("ULDD.X49", "Lender");
        return;
      }
      this.SetVal("ULDD.X49", "");
      return;
label_232:
      this.SetVal("ULDD.FNM.MORNET.X76", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
      return;
label_233:
      this.SetVal("ULDD.FNM.MORNET.X77", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
      return;
label_234:
      this.SetVal("ULDD.FNM.HMDA.X15", ULDDExportCalculation.ParseDecimal((object) val, 4, 2).ToString("#.00"));
      return;
label_235:
      this.SetVal("ULDD.FNM.MORNET.X75", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
      return;
label_236:
      this.SetVal("ULDD.FNM.975", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
      return;
label_237:
      this.SetVal("ULDD.FNM.X43", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
      return;
label_238:
      this.SetVal("ULDD.FNM.X50", ULDDExportCalculation.ParseDecimal((object) val, 3, 2).ToString("#.00"));
      return;
label_239:
      this.SetVal("ULDD.FNM.X70", string.Concat((object) ULDDExportCalculation.ParseNumeric3((object) val)));
      return;
label_240:
      if (val.Length > 25)
      {
        this.SetVal("ULDD.FNM.4000", val.Substring(0, 25));
        return;
      }
      this.SetVal("ULDD.FNM.4000", val);
      return;
label_243:
      if (val.Length > 25)
      {
        this.SetVal("ULDD.FNM.4004", val.Substring(0, 25));
        return;
      }
      this.SetVal("ULDD.FNM.4004", val);
      return;
label_246:
      if (val.Length > 1)
      {
        this.SetVal("ULDD.FNM.4001", val.Substring(0, 1));
        return;
      }
      this.SetVal("ULDD.FNM.4001", val);
      return;
label_249:
      if (val.Length > 1)
      {
        this.SetVal("ULDD.FNM.4005", val.Substring(0, 1));
        return;
      }
      this.SetVal("ULDD.FNM.4005", val);
      return;
label_252:
      if (val.Length > 35)
      {
        this.SetVal("ULDD.FNM.1859", val.Substring(0, 35));
        return;
      }
      this.SetVal("ULDD.FNM.1859", val);
      return;
label_338:
      if (this.Val("934") == "Y")
      {
        this.SetVal("ULDD.FNM.LoanProgramIdentifier", "LoanFirstTimeHomebuyer");
        return;
      }
      this.SetVal("ULDD.FNM.LoanProgramIdentifier", "");
      return;
label_351:
      this.UpdateBaseLTVRatioPercent();
    }

    private void UpdateFloodSpecialFeatureCode(string valUlddX178, string val676)
    {
      if (valUlddX178.ToUpper() == "Y")
      {
        if (val676.ToUpper() == "Y")
          this.SetVal("ULDD.FNM.FloodSpecialFeatureCode", "170");
        else
          this.SetVal("ULDD.FNM.FloodSpecialFeatureCode", "");
      }
      else if (val676.ToUpper() == "Y")
        this.SetVal("ULDD.FNM.FloodSpecialFeatureCode", "175");
      else
        this.SetVal("ULDD.FNM.FloodSpecialFeatureCode", "180");
    }

    private void UpdateBaseLTVRatioPercent()
    {
      string str = this.Val("19");
      Decimal num1 = Utils.ParseDecimal((object) this.Val("1109"), 0M);
      Decimal num2 = Utils.ParseDecimal((object) this.Val("356"), 0M);
      Decimal num3 = Utils.ParseDecimal((object) this.Val("1821"), 0M);
      Decimal num4 = Utils.ParseDecimal((object) this.Val("136"), 0M);
      Decimal num5 = 0M;
      if (num1 > 0M)
      {
        switch (str)
        {
          case "Purchase":
          case "ConstructionOnly":
          case "ConstructionToPermanent":
            if (num4 == num2 && num3 == num4 && num4 == 0M)
              num5 = 0M;
            else if (num4 == num2 && num4 == 0M)
              num5 = num3;
            else if (num2 == 0M)
              num2 = num3;
            else if (num4 == 0M)
              num5 = num2;
            if (num4 > 0M && num2 > 0M)
            {
              num5 = num4 < num2 ? num4 : num2;
              break;
            }
            if (num4 > 0M && num2 == 0M)
            {
              num5 = num4;
              break;
            }
            break;
          case "Cash-Out Refinance":
          case "NoCash-Out Refinance":
            num5 = num2 > 0M ? num2 : num3;
            break;
        }
        if (num5 > 0M)
        {
          Decimal d = num1 * 100M / num5;
          string[] strArray = d.ToString().Split('.');
          if (strArray.Length == 2 && strArray[1].IndexOf("00") != 0)
          {
            this.SetVal("ULDD.X186", (Decimal.Truncate(d) + 1M).ToString());
            this.SetVal("4012", (Decimal.Truncate(d) + 1M).ToString());
          }
          else
          {
            this.SetVal("ULDD.X186", Decimal.Truncate(d).ToString());
            this.SetVal("4012", Decimal.Truncate(d).ToString());
          }
        }
        else
        {
          this.SetVal("ULDD.X186", "");
          this.SetVal("4012", "");
        }
      }
      else
      {
        this.SetVal("ULDD.X186", "");
        this.SetVal("4012", "");
      }
    }
  }
}
