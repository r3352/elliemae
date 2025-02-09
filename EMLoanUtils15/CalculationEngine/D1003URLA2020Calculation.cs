// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.D1003URLA2020Calculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class D1003URLA2020Calculation : CalculationBase
  {
    private const string className = "D1003URLA2020Calculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcVOOATotalOtherAssets;
    internal Routine CalcVOOADoesNotApply;
    internal Routine CalcVOLDoesNotApply;
    internal Routine CalcVOOLDoesNotApply;
    internal Routine CalcVOMDoesNotApply;
    internal Routine CalcVOGGDoesNotApply;
    internal Routine CalcPriorResidenceDoesNotApply;
    internal Routine CalcCopyCurrentToMailingAddress;
    internal Routine CalcCopyCitizenship;
    internal Routine CalculateProductDescription;
    internal Routine CalcURLALoanIdentifier;
    internal Routine UpdateMarriedStatus;
    internal Routine ClearOtherLangDescription;
    internal Routine UpdateMilitaryService;
    internal Routine CalcVODTotalDeposits;
    internal Routine CalcOccupancyDisplayField;
    internal Routine ClearBankruptcyTypes;
    internal Routine ConcatenateURLAAddresses;
    internal Routine ConcatenateNames;
    internal Routine ClearConstructionLoanTypes;
    internal Routine CalcTitleInWhichPropertyHeld;
    internal Routine CalcBorrowerCount;
    internal Routine CalcNonSubjectPropertyDebtsToBePaidOffAmount;
    internal Routine CalcEstimatedClosingCostsAmount;
    internal Routine CalcTotalNewMortgageLoans;
    internal Routine CalcTotalGiftGrants;
    internal Routine CalcTotalOtherAssets;
    internal Routine CalcOtherCredits;
    internal Routine CalcTotalCredits;
    internal Routine CalcCreditType;
    internal Routine UpdateOtherRelationship;
    internal Routine UpdateCountry;
    internal Routine UpdateBorrowerAliasName;
    internal Routine CalcLandOtherType;
    internal Routine CalcEstimatedNetMonthlyRent;
    internal Routine CreateLinkedVoal;
    internal Routine UpdateLinkedVoal;
    internal Routine RemovelinkedVoal;
    internal Routine CalcAll;
    internal Routine CalcURLALoanPurposeMapping;
    internal Routine CopyStreetAddressTo2009;
    internal Routine CalcProposedSupplementalPropertyInsurance;
    internal Routine CalcVOEDoesNotApply;
    internal Routine CalcVOOIDoesNotApply;
    internal Routine CopyATRQMIncome;
    internal Routine CalculateRefinanceType;
    internal Routine CalcConstructionLoanIndicator;
    internal Routine CalcBackEndRatio;
    internal Routine CalcOtherLiabilitiesType;
    internal Routine CalcNegativeAmortizationIndicator;
    internal Routine CopySelfEmployedToBaseIncome;
    internal Routine CalcPropertyLeaseholdExpirationDate;
    internal Routine CalcArmProg;
    internal Routine CalcLockRequestPropertyAddress;
    internal Routine ClearCountryofCitizenship;
    internal Routine ClearDeclarationFields;
    internal Routine ClearForeignAddressFields;
    internal Routine ClearDenialSubjectAddressFields;
    internal Routine CalcPresentHousingExpenseRent;
    internal Routine CalcTotalMortgageAndTaxes;
    internal Routine CalcRent;
    internal Routine CalcAverageRepresentativeCreditScore;
    internal Routine CalcUrlaItemizedCredits;
    internal Routine CalcFirstTimeHomeBuyer;
    internal Routine CalcAccessoryDwellingUnit;
    internal Routine CalcCreditReportIndicators;
    internal Routine CopyBorrowerUrlaCounselingFormat;
    internal Routine CopyCoBorrowerUrlaCounselingFormat;
    protected static string[] ConstructionFields = new string[22]
    {
      "105",
      "114",
      "146",
      "149",
      "152",
      "1605",
      "1607",
      "1609",
      "210",
      "212",
      "213",
      "215",
      "217",
      "222",
      "1718",
      "224",
      "1053",
      "1055",
      "272",
      "256",
      "1062",
      "LINKGUID"
    };
    private readonly Dictionary<int, string> aggBorrIncomeFields = new Dictionary<int, string>()
    {
      {
        19,
        "101"
      },
      {
        20,
        "102"
      },
      {
        21,
        "103"
      },
      {
        22,
        "104"
      },
      {
        23,
        "107"
      }
    };
    private readonly Dictionary<int, string> aggCoBorrIncomeFields = new Dictionary<int, string>()
    {
      {
        19,
        "110"
      },
      {
        20,
        "111"
      },
      {
        21,
        "112"
      },
      {
        22,
        "113"
      },
      {
        23,
        "116"
      }
    };
    private readonly Dictionary<string, string> vodOtherAssetsMapping = new Dictionary<string, string>()
    {
      {
        "24",
        "01"
      },
      {
        "98",
        "19"
      },
      {
        "02",
        "05"
      },
      {
        "03",
        "06"
      },
      {
        "04",
        "07"
      },
      {
        "05",
        "08"
      },
      {
        "06",
        "09"
      },
      {
        "07",
        "10"
      },
      {
        "26",
        "11"
      },
      {
        "27",
        "12"
      },
      {
        "28",
        "13"
      },
      {
        "37",
        "14"
      },
      {
        "44",
        "16"
      },
      {
        "45",
        "17"
      },
      {
        "38",
        "15"
      },
      {
        "64",
        "20"
      },
      {
        "36",
        "18"
      }
    };
    internal Routine CalcAggregateIncome;
    internal Routine CalcGiftGrantSourceOtherDesc;
    internal Routine CalcRepaymentTypecode;
    internal Routine ClearVOMFutureProposeUsageTypeOtherDesc;
    internal Routine ClearPurchaseCreditSourceType;
    internal Routine CalcMilitaryEntitlementIncome;
    internal Routine CalcSupplementalInsuranceAmount;
    private static string[,] addressSets = new string[11, 5]
    {
      {
        "319",
        "URLA.X188",
        "URLA.X189",
        "URLA.X190",
        "Y"
      },
      {
        "FR0004",
        "FR0026",
        "FR0025",
        "FR0027",
        "N"
      },
      {
        "BR0004",
        "BR0026",
        "BR0025",
        "BR0027",
        "N"
      },
      {
        "CR0004",
        "CR0026",
        "CR0025",
        "CR0027",
        "N"
      },
      {
        "1416",
        "URLA.X197",
        "URLA.X7",
        "URLA.X8",
        "Y"
      },
      {
        "1519",
        "URLA.X198",
        "URLA.X9",
        "URLA.X10",
        "Y"
      },
      {
        "FE0004",
        "FE0060",
        "FE0058",
        "FE0059",
        "N"
      },
      {
        "BE0004",
        "BE0060",
        "BE0058",
        "BE0059",
        "N"
      },
      {
        "CE0004",
        "CE0060",
        "CE0058",
        "CE0059",
        "N"
      },
      {
        "11",
        "URLA.X73",
        "URLA.X74",
        "URLA.X75",
        "Y"
      },
      {
        "FM0004",
        "FM0050",
        "FM0047",
        "FM0048",
        "N"
      }
    };
    private static Dictionary<string, string> titleHeldMapping = new Dictionary<string, string>()
    {
      {
        "sole ownership",
        "Individual"
      },
      {
        "single man",
        "Individual"
      },
      {
        "single woman",
        "Individual"
      },
      {
        "married man",
        "Individual"
      },
      {
        "married woman",
        "Individual"
      },
      {
        "unmarried man",
        "Individual"
      },
      {
        "unmarried woman",
        "Individual"
      },
      {
        "as her sole and separate property",
        "Individual"
      },
      {
        "as his sole and separate property",
        "Individual"
      },
      {
        "life estate",
        "LifeEstate"
      },
      {
        "tenancy in common",
        "TenantsInCommon"
      },
      {
        "all as tenants in common",
        "TenantsInCommon"
      },
      {
        "as tenants in common",
        "TenantsInCommon"
      },
      {
        "husband and wife as tenants in common",
        "TenantsInCommon"
      },
      {
        "tenants in common",
        "TenantsInCommon"
      },
      {
        "each as to an undivided one half interest",
        "TenantsInCommon"
      },
      {
        "each as to an undivided one third interest",
        "TenantsInCommon"
      },
      {
        "each as to an undivided one fourth interest",
        "TenantsInCommon"
      },
      {
        "joint tenancy with right of survivorship",
        "JointTenantsWithRightOfSurvivorship"
      },
      {
        "husband and wife as joint tenants with right of survivorship",
        "JointTenantsWithRightOfSurvivorship"
      },
      {
        "as joint tenants with right of survivorship",
        "JointTenantsWithRightOfSurvivorship"
      },
      {
        "tenancy by the entirety",
        "TenantsByTheEntirety"
      },
      {
        "tenancy by entirety",
        "TenantsByTheEntirety"
      },
      {
        "as tenancy by entirety",
        "TenantsByTheEntirety"
      }
    };
    private readonly Dictionary<string, string[]> foreignAddressFieldsDict = new Dictionary<string, string[]>()
    {
      {
        "FR",
        new string[10]
        {
          "04",
          "26",
          "06",
          "07",
          "08",
          "28",
          "30",
          "25",
          "27",
          "09"
        }
      },
      {
        "URLA.X267",
        new string[10]
        {
          "1416",
          "URLA.X197",
          "1417",
          "1418",
          "1419",
          "URLA.X11",
          "URLA.X269",
          "URLA.X7",
          "URLA.X8",
          ""
        }
      },
      {
        "URLA.X268",
        new string[10]
        {
          "1519",
          "URLA.X198",
          "1520",
          "1521",
          "1522",
          "URLA.X12",
          "URLA.X270",
          "URLA.X9",
          "URLA.X10",
          ""
        }
      },
      {
        "BR39",
        new string[10]
        {
          "",
          "05",
          "09",
          "10",
          "11",
          "",
          "40",
          "",
          "",
          ""
        }
      },
      {
        "BR29",
        new string[10]
        {
          "04",
          "26",
          "06",
          "07",
          "08",
          "28",
          "30",
          "25",
          "27",
          "22"
        }
      },
      {
        "CR39",
        new string[10]
        {
          "",
          "05",
          "09",
          "10",
          "11",
          "",
          "40",
          "",
          "",
          ""
        }
      },
      {
        "CR29",
        new string[10]
        {
          "04",
          "26",
          "06",
          "07",
          "08",
          "28",
          "30",
          "25",
          "27",
          "22"
        }
      },
      {
        "FE",
        new string[10]
        {
          "04",
          "60",
          "05",
          "06",
          "07",
          "61",
          "79",
          "58",
          "59",
          ""
        }
      },
      {
        "FE0580",
        new string[10]
        {
          "FE0504",
          "FE0560",
          "FE0505",
          "FE0506",
          "FE0507",
          "",
          "FE0579",
          "FE0558",
          "FE0559",
          ""
        }
      },
      {
        "FE0680",
        new string[10]
        {
          "FE0604",
          "FE0660",
          "FE0605",
          "FE0606",
          "FE0607",
          "",
          "FE0679",
          "FE0658",
          "FE0659",
          ""
        }
      },
      {
        "FE0507",
        new string[10]
        {
          "FE0504",
          "FE0560",
          "FE0505",
          "FE0506",
          "FE0507",
          "",
          "FE0579",
          "FE0558",
          "FE0559",
          ""
        }
      },
      {
        "FE0607",
        new string[10]
        {
          "FE0604",
          "FE0660",
          "FE0605",
          "FE0606",
          "FE0607",
          "",
          "FE0679",
          "FE0658",
          "FE0659",
          ""
        }
      },
      {
        "BE",
        new string[10]
        {
          "04",
          "60",
          "05",
          "06",
          "07",
          "61",
          "79",
          "58",
          "59",
          ""
        }
      },
      {
        "CE",
        new string[10]
        {
          "04",
          "60",
          "05",
          "06",
          "07",
          "61",
          "79",
          "58",
          "59",
          ""
        }
      },
      {
        "FM",
        new string[10]
        {
          "04",
          "50",
          "06",
          "07",
          "08",
          "51",
          "57",
          "47",
          "48",
          ""
        }
      },
      {
        "DD",
        new string[10]
        {
          "",
          "04",
          "05",
          "06",
          "07",
          "",
          "40",
          "",
          "",
          ""
        }
      },
      {
        "FL",
        new string[10]
        {
          "",
          "04",
          "05",
          "06",
          "07",
          "",
          "68",
          "",
          "",
          ""
        }
      },
      {
        "URLAROL",
        new string[10]
        {
          "",
          "07",
          "08",
          "09",
          "10",
          "",
          "22",
          "",
          "",
          ""
        }
      },
      {
        "CAPIAP.X62",
        new string[10]
        {
          "",
          "CAPIAP.X10",
          "CAPIAP.X11",
          "CAPIAP.X12",
          "CAPIAP.X13",
          "",
          "CAPIAP.X64",
          "",
          "",
          ""
        }
      },
      {
        "CAPIAP.X63",
        new string[10]
        {
          "",
          "CAPIAP.X17",
          "CAPIAP.X18",
          "CAPIAP.X19",
          "CAPIAP.X20",
          "",
          "CAPIAP.X65",
          "",
          "",
          ""
        }
      },
      {
        "DENIAL.X97",
        new string[10]
        {
          "",
          "DENIAL.X82",
          "DENIAL.X83",
          "DENIAL.X84",
          "DENIAL.X85",
          "",
          "DENIAL.X98",
          "",
          "",
          ""
        }
      },
      {
        "DENIAL.X99",
        new string[10]
        {
          "",
          "DENIAL.X87",
          "DENIAL.X88",
          "DENIAL.X89",
          "DENIAL.X90",
          "",
          "DENIAL.X100",
          "",
          "",
          ""
        }
      },
      {
        "4678",
        new string[10]
        {
          "",
          "1302",
          "1304",
          "1292",
          "1305",
          "",
          "4679",
          "",
          "",
          ""
        }
      },
      {
        "NBOC",
        new string[10]
        {
          "",
          "05",
          "06",
          "07",
          "08",
          "",
          "23",
          "",
          "",
          ""
        }
      },
      {
        "4680",
        new string[10]
        {
          "",
          "701",
          "702",
          "1249",
          "703",
          "",
          "4681",
          "",
          "",
          ""
        }
      },
      {
        "VEND.X1047",
        new string[10]
        {
          "",
          "VEND.X413",
          "VEND.X414",
          "VEND.X415",
          "VEND.X416",
          "",
          "VEND.X1048",
          "",
          "",
          ""
        }
      },
      {
        "Seller3.ForeignAddressIndicator",
        new string[10]
        {
          "",
          "Seller3.Addr",
          "Seller3.City",
          "Seller3.State",
          "Seller3.Zip",
          "",
          "Seller3.Country",
          "",
          "",
          ""
        }
      },
      {
        "Seller4.ForeignAddressIndicator",
        new string[10]
        {
          "",
          "Seller4.Addr",
          "Seller4.City",
          "Seller4.State",
          "Seller4.Zip",
          "",
          "Seller4.Country",
          "",
          "",
          ""
        }
      },
      {
        "4880",
        new string[10]
        {
          "",
          "4881",
          "4882",
          "4883",
          "4884",
          "",
          "4893",
          "",
          "",
          ""
        }
      }
    };

    internal D1003URLA2020Calculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcURLALoanPurposeMapping = this.RoutineX(new Routine(this.calculateURLALoanPurposeMapping));
      this.CalcCopyCurrentToMailingAddress = this.RoutineX(new Routine(this.copyCurrentToMailingAddress));
      this.CalcCopyCitizenship = this.RoutineX(new Routine(this.copyCitizenship));
      this.CalcURLALoanIdentifier = this.RoutineX(new Routine(this.calculateURLALoanIdentifier));
      this.UpdateMarriedStatus = this.RoutineX(new Routine(this.updateMarriedStatus));
      this.ClearOtherLangDescription = this.RoutineX(new Routine(this.clearOtherLanguageDescription));
      this.UpdateMilitaryService = this.RoutineX(new Routine(this.updateMilitaryService));
      this.CalcVOOATotalOtherAssets = this.RoutineX(new Routine(this.calculateVOOATotalAssets));
      this.CalcVOOADoesNotApply = this.RoutineX(new Routine(this.calculateVOOADoesNotApply));
      this.CalcVOLDoesNotApply = this.RoutineX(new Routine(this.calculateVOLDoesNotApply));
      this.CalcVOOLDoesNotApply = this.RoutineX(new Routine(this.calculateVOOLDoesNotApply));
      this.CalcVOMDoesNotApply = this.RoutineX(new Routine(this.calculateVOMDoesNotApply));
      this.CalcVOGGDoesNotApply = this.RoutineX(new Routine(this.calculateVOGGDoesNotApply));
      this.CalcPriorResidenceDoesNotApply = this.RoutineX(new Routine(this.calculatePriorAddressDoesNotApply));
      this.CalculateProductDescription = this.RoutineX(new Routine(this.calculateProductDescription));
      this.CalcVODTotalDeposits = this.RoutineX(new Routine(this.calculateVODTotalDeposits));
      this.CalcOccupancyDisplayField = this.RoutineX(new Routine(this.calculateOccupancyDisplayField));
      this.ClearBankruptcyTypes = this.RoutineX(new Routine(this.clearBankruptcyTypes));
      this.ConcatenateURLAAddresses = this.RoutineX(new Routine(this.concatenateURLAAddresses));
      this.ConcatenateNames = this.RoutineX(new Routine(this.concatenateNames));
      this.ClearConstructionLoanTypes = this.RoutineX(new Routine(this.clearConstructionLoanTypes));
      this.CalcTitleInWhichPropertyHeld = this.RoutineX(new Routine(this.calculateTitleInWhichPropertyHeld));
      this.CalcBorrowerCount = this.RoutineX(new Routine(this.calculateBorrowerCount));
      this.CalcNonSubjectPropertyDebtsToBePaidOffAmount = this.RoutineX(new Routine(this.calculateNonSubjectPropertyDebtsToBePaidOffAmount));
      this.CalcEstimatedClosingCostsAmount = this.RoutineX(new Routine(this.calculateEstimatedClosingCostsAmount));
      this.CalcTotalNewMortgageLoans = this.RoutineX(new Routine(this.calculateTotalNewMortgageLoans));
      this.CalcTotalGiftGrants = this.RoutineX(new Routine(this.calculateTotalGiftGrants));
      this.CalcTotalOtherAssets = this.RoutineX(new Routine(this.calculateTotalOtherAssets));
      this.CalcOtherCredits = this.RoutineX(new Routine(this.calculateOtherCredits));
      this.CalcTotalCredits = this.RoutineX(new Routine(this.calculateTotalCredits));
      this.CalcCreditType = this.RoutineX(new Routine(this.calculateCreditType));
      this.CopyStreetAddressTo2009 = this.RoutineX(new Routine(this.copyStreetAddressTo2009));
      this.UpdateOtherRelationship = this.RoutineX(new Routine(this.updateOtherRelationship));
      this.UpdateCountry = this.RoutineX(new Routine(this.updateCountry));
      this.UpdateBorrowerAliasName = this.RoutineX(new Routine(this.updateBorrowerAliasName));
      this.CalcLandOtherType = this.RoutineX(new Routine(this.calculateLandOtherType));
      this.CalcEstimatedNetMonthlyRent = this.RoutineX(new Routine(this.calculateEstimatedNetMonthlyRent));
      this.CalcProposedSupplementalPropertyInsurance = this.RoutineX(new Routine(this.calculateProposedSupplementalPropertyInsurance));
      this.ClearCountryofCitizenship = this.RoutineX(new Routine(this.clearCountryofCitizenship));
      this.ClearDeclarationFields = this.RoutineX(new Routine(this.clearDeclarationFields));
      this.CalcAggregateIncome = this.RoutineX(new Routine(this.calculateAggregateIncome));
      this.CalculateRefinanceType = this.RoutineX(new Routine(this.calculateRefinanceType));
      this.CreateLinkedVoal = this.RoutineX(new Routine(this.createLinkedVoal));
      this.UpdateLinkedVoal = this.RoutineX(new Routine(this.updateLinkedVoal));
      this.RemovelinkedVoal = this.RoutineX(new Routine(this.removelinkedVoal));
      this.CopyATRQMIncome = this.RoutineX(new Routine(this.copyATRQMIncome));
      this.CalcAll = this.RoutineX(new Routine(this.calculateAll));
      this.CalcVOEDoesNotApply = this.RoutineX(new Routine(this.calculateVOEDoesNotApply));
      this.CalcVOOIDoesNotApply = this.RoutineX(new Routine(this.calculateVOOIDoesNotApply));
      this.CalcGiftGrantSourceOtherDesc = this.RoutineX(new Routine(this.calculateGiftGrantSourceOtherDesc));
      this.CalcConstructionLoanIndicator = this.RoutineX(new Routine(this.calculateConstructionLoanIndicator));
      this.CalcRepaymentTypecode = this.RoutineX(new Routine(this.calculateRepaymentTypecode));
      this.CalcBackEndRatio = this.RoutineX(new Routine(this.calculateBackEndRatio));
      this.CalcOtherLiabilitiesType = this.RoutineX(new Routine(this.calculateOtherLiabilitiesType));
      this.CalcNegativeAmortizationIndicator = this.RoutineX(new Routine(this.calculateNegativeAmortizationIndicator));
      this.CopySelfEmployedToBaseIncome = this.RoutineX(new Routine(this.copySelfEmployedToBaseIncome));
      this.CalcPropertyLeaseholdExpirationDate = this.RoutineX(new Routine(this.calculatePropertyLeaseholdExpirationDate));
      this.CalcArmProg = this.RoutineX(new Routine(this.calculateArmProg));
      this.CalcLockRequestPropertyAddress = this.RoutineX(new Routine(this.calculateLockRequestPropertyAddress));
      this.ClearVOMFutureProposeUsageTypeOtherDesc = this.RoutineX(new Routine(this.clearVOMFutureProposeUsageTypeOtherDesc));
      this.ClearPurchaseCreditSourceType = this.RoutineX(new Routine(this.clearPurchaseCreditSourceType));
      this.ClearForeignAddressFields = this.RoutineX(new Routine(this.clearAllForeignAddressBlocks));
      this.ClearDenialSubjectAddressFields = this.RoutineX(new Routine(this.clearDenialSubjectAddressFields));
      this.CalcMilitaryEntitlementIncome = this.RoutineX(new Routine(this.calculateMilitaryEntitlementIncome));
      this.CalcRent = this.RoutineX(new Routine(this.calculateRent));
      this.CalcPresentHousingExpenseRent = this.RoutineX(new Routine(this.calculatePresentHousingExpenseRent));
      this.CalcTotalMortgageAndTaxes = this.RoutineX(new Routine(this.calculateTotalMortgageAndTaxes));
      this.CalcAverageRepresentativeCreditScore = this.RoutineX(new Routine(this.calculateAverageRepresentativeCreditScore));
      this.CalcUrlaItemizedCredits = this.RoutineX(new Routine(this.calculateUrlaItemizedCredits));
      this.CalcSupplementalInsuranceAmount = this.RoutineX(new Routine(this.calculateSupplementalInsuranceAmount));
      this.CalcFirstTimeHomeBuyer = this.RoutineX(new Routine(this.calculateFirstTimeHomeBuyer));
      this.CalcAccessoryDwellingUnit = this.RoutineX(new Routine(this.calculateAccessoryDwellingUnit));
      this.CalcCreditReportIndicators = this.RoutineX(new Routine(this.calculateCreditReportIndicators));
      this.CopyBorrowerUrlaCounselingFormat = this.RoutineX(new Routine(this.copyBorrowerUrlaCounselingFormat));
      this.CopyCoBorrowerUrlaCounselingFormat = this.RoutineX(new Routine(this.copyCoBorrowerUrlaCounselingFormat));
      this.addFieldHandlers();
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.copyCurrentToMailingAddress));
      this.AddFieldHandler("FR0128", routine1);
      this.AddFieldHandler("FR0228", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.copyCitizenship));
      this.AddFieldHandler("URLA.X1", routine2);
      this.AddFieldHandler("URLA.X2", routine2);
      this.AddFieldHandler("965", routine2);
      this.AddFieldHandler("985", routine2);
      this.AddFieldHandler("466", routine2);
      this.AddFieldHandler("467", routine2);
      Routine routine3 = this.RoutineX(new Routine(this.calculateURLALoanIdentifier));
      this.AddFieldHandler("2573", routine3);
      this.AddFieldHandler("URLA.X119", routine3);
      this.AddFieldHandler("URLA.X120", routine3);
      this.AddFieldHandler("352", routine3);
      this.AddFieldHandler("364", routine3);
      this.AddFieldHandler("URLA.X238", routine3);
      Routine routine4 = this.RoutineX(new Routine(this.updateOtherRelationship));
      this.AddFieldHandler("URLA.X113", routine4);
      this.AddFieldHandler("URLA.X114", routine4);
      Routine routine5 = this.RoutineX(new Routine(this.updateMilitaryService));
      this.AddFieldHandler("URLA.X13", routine5);
      this.AddFieldHandler("URLA.X14", routine5);
      Routine routine6 = this.RoutineX(new Routine(this.clearOtherLanguageDescription));
      this.AddFieldHandler("URLA.X21", routine6);
      this.AddFieldHandler("URLA.X22", routine6);
      Routine routine7 = this.RoutineX(new Routine(this.calculateVOOATotalAssets));
      this.AddFieldHandler("URLAROA0001", routine7);
      this.AddFieldHandler("URLAROA0003", routine7);
      Routine routine8 = this.RoutineX(this.calObjs.D1003Cal.CalcTotalOtherAssets);
      this.AddFieldHandler("URLAROA0004", routine8);
      this.AddFieldHandler("URLAROA0002", routine8);
      this.AddFieldHandler("URLAROA0003", routine8);
      this.AddFieldHandler("URLARGG0020", routine8);
      this.AddFieldHandler("URLARGG0021", routine8);
      this.AddFieldHandler("URLAROA0001", this.RoutineX(new Routine(this.calculateVOOADoesNotApply)));
      this.AddFieldHandler("FL0015", this.RoutineX(new Routine(this.calculateVOLDoesNotApply)));
      this.AddFieldHandler("URLAROL0001", this.RoutineX(new Routine(this.calculateVOOLDoesNotApply)));
      Routine routine9 = this.RoutineX(new Routine(this.calculateVOMDoesNotApply));
      this.AddFieldHandler("FM0046", routine9);
      this.AddFieldHandler("URLA.X110", routine9);
      this.AddFieldHandler("URLARGG0002", this.RoutineX(new Routine(this.calculateVOGGDoesNotApply)));
      Routine routine10 = this.RoutineX(new Routine(this.calculatePriorAddressDoesNotApply));
      this.AddFieldHandler("BR0013", routine10);
      this.AddFieldHandler("BR0023", routine10);
      this.AddFieldHandler("CR0013", routine10);
      this.AddFieldHandler("CR0023", routine10);
      this.AddFieldHandler("FR0304", routine10);
      this.AddFieldHandler("FR0306", routine10);
      this.AddFieldHandler("FR0307", routine10);
      this.AddFieldHandler("FR0308", routine10);
      this.AddFieldHandler("FR0312", routine10);
      this.AddFieldHandler("FR0315", routine10);
      this.AddFieldHandler("FR0324", routine10);
      this.AddFieldHandler("FR0325", routine10);
      this.AddFieldHandler("FR0326", routine10);
      this.AddFieldHandler("FR0327", routine10);
      this.AddFieldHandler("FR0328", routine10);
      this.AddFieldHandler("FR0398", routine10);
      this.AddFieldHandler("FR0399", routine10);
      this.AddFieldHandler("FR0316", routine10);
      this.AddFieldHandler("FR0404", routine10);
      this.AddFieldHandler("FR0406", routine10);
      this.AddFieldHandler("FR0407", routine10);
      this.AddFieldHandler("FR0408", routine10);
      this.AddFieldHandler("FR0412", routine10);
      this.AddFieldHandler("FR0415", routine10);
      this.AddFieldHandler("FR0424", routine10);
      this.AddFieldHandler("FR0425", routine10);
      this.AddFieldHandler("FR0426", routine10);
      this.AddFieldHandler("FR0427", routine10);
      this.AddFieldHandler("FR0428", routine10);
      this.AddFieldHandler("FR0498", routine10);
      this.AddFieldHandler("FR0499", routine10);
      this.AddFieldHandler("FR0416", routine10);
      Routine routine11 = this.RoutineX(new Routine(this.updateBorrowerAliasName));
      this.AddFieldHandler("URLABAKA0001", routine11);
      this.AddFieldHandler("URLACAKA0001", routine11);
      Routine routine12 = this.RoutineX(new Routine(this.calculateVODTotalDeposits));
      this.AddFieldHandler("DD0048", routine12);
      this.AddFieldHandler("DD0049", routine12);
      this.AddFieldHandler("DD0050", routine12);
      this.AddFieldHandler("DD0051", routine12);
      Routine routine13 = this.RoutineX(new Routine(this.calculateOccupancyDisplayField));
      this.AddFieldHandler("1811", routine13);
      this.AddFieldHandler("1172", routine13);
      this.AddFieldHandler("URLA.X76", routine13);
      Routine routine14 = this.RoutineX(new Routine(this.clearBankruptcyTypes));
      this.AddFieldHandler("265", routine14);
      this.AddFieldHandler("266", routine14);
      Routine routine15 = this.RoutineX(new Routine(this.concatenateURLAAddresses));
      this.AddFieldHandler("URLA.X188", routine15);
      this.AddFieldHandler("URLA.X189", routine15);
      this.AddFieldHandler("URLA.X190", routine15);
      this.AddFieldHandler("319", routine15);
      Routine routine16 = this.RoutineX(new Routine(this.concatenateNames));
      this.AddFieldHandler("URLA.X170", routine16);
      this.AddFieldHandler("URLA.X171", routine16);
      this.AddFieldHandler("URLA.X172", routine16);
      this.AddFieldHandler("URLA.X173", routine16);
      this.AddFieldHandler("1612", routine16);
      Routine routine17 = this.RoutineX(new Routine(this.clearConstructionLoanTypes));
      this.AddFieldHandler("4084", routine17);
      this.AddFieldHandler("URLA.X133", this.CalcConstructionLoanIndicator + routine17);
      this.AddFieldHandler("33", this.RoutineX(new Routine(this.calculateMannerTitleHeld)));
      this.AddFieldHandler("URLA.X138", this.RoutineX(new Routine(this.calculateMannerTitleHeldOtherDesc)));
      Routine routine18 = this.RoutineX(new Routine(this.calculateOwnerShipShare));
      this.AddFieldHandler("FE0115", routine18);
      this.AddFieldHandler("FE0215", routine18);
      this.AddFieldHandler("FE0315", routine18);
      this.AddFieldHandler("FE0415", routine18);
      this.AddFieldHandler("FE0515", routine18);
      this.AddFieldHandler("FE0615", routine18);
      this.AddFieldHandler("BE0015", routine18);
      this.AddFieldHandler("CE0015", routine18);
      this.AddFieldHandler("URLARGG0021", this.RoutineX(new Routine(this.calculateTotalGiftGrants)));
      this.AddFieldHandler("URLA.X150", this.RoutineX(new Routine(this.calculateOtherCredits)));
      this.AddFieldHandler("URLA.X149", this.RoutineX(new Routine(this.calculateTotalCredits)));
      Routine routine19 = this.RoutineX(new Routine(this.updateCountry));
      this.AddFieldHandler("FR0108", routine19);
      this.AddFieldHandler("FR0208", routine19);
      this.AddFieldHandler("FR0308", routine19);
      this.AddFieldHandler("FR0408", routine19);
      this.AddFieldHandler("1419", routine19);
      this.AddFieldHandler("1522", routine19);
      this.AddFieldHandler("BR0008", routine19);
      this.AddFieldHandler("CR0008", routine19);
      this.AddFieldHandler("BR0011", routine19);
      this.AddFieldHandler("CR0011", routine19);
      this.AddFieldHandler("FE0107", routine19);
      this.AddFieldHandler("FE0207", routine19);
      this.AddFieldHandler("FE0307", routine19);
      this.AddFieldHandler("FE0407", routine19);
      this.AddFieldHandler("FE0507", routine19);
      this.AddFieldHandler("FE0607", routine19);
      this.AddFieldHandler("BE0007", routine19);
      this.AddFieldHandler("CE0007", routine19);
      this.AddFieldHandler("FM0008", routine19);
      this.AddFieldHandler("FL0007", routine19);
      this.AddFieldHandler("DD0007", routine19);
      this.AddFieldHandler("URLAROL0010", routine19);
      this.AddFieldHandler("CAPIAP.X13", routine19);
      this.AddFieldHandler("DENIAL.X90", routine19);
      this.AddFieldHandler("DENIAL.X85", routine19);
      this.AddFieldHandler("1305", routine19);
      this.AddFieldHandler("NBOC0008", routine19);
      this.AddFieldHandler("703", routine19);
      this.AddFieldHandler("VEND.X416", routine19);
      this.AddFieldHandler("Seller3.Zip", routine19);
      this.AddFieldHandler("Seller4.Zip", routine19);
      this.AddFieldHandler("4884", routine19);
      this.AddFieldHandler("URLA.X141", this.RoutineX(new Routine(this.calculateLandOtherType)));
      Routine routine20 = this.RoutineX(new Routine(this.calculateEstimatedNetMonthlyRent));
      this.AddFieldHandler("1005", routine20);
      this.AddFieldHandler("1487", routine20);
      this.AddFieldHandler("912", routine20);
      this.AddFieldHandler("URLA.X166", this.RoutineX(new Routine(this.calculateGovtRefTypeOtherDesc)));
      Routine routine21 = this.RoutineX(new Routine(this.calculateLoanRepaymentType));
      this.AddFieldHandler("URLA.X239", routine21);
      this.AddFieldHandler("CD4.X2", routine21);
      Routine routine22 = this.RoutineX(new Routine(this.calculateAggregateIncome));
      this.AddFieldHandler("BE0019", routine22);
      this.AddFieldHandler("CE0019", routine22);
      this.AddFieldHandler("BE0020", routine22);
      this.AddFieldHandler("CE0020", routine22);
      this.AddFieldHandler("BE0021", routine22);
      this.AddFieldHandler("CE0021", routine22);
      this.AddFieldHandler("BE0022", routine22);
      this.AddFieldHandler("CE0022", routine22);
      this.AddFieldHandler("BE0023", routine22);
      this.AddFieldHandler("CE0023", routine22);
      for (int index1 = 19; index1 <= 53; ++index1)
      {
        if (index1 == 24)
          index1 = 53;
        for (int index2 = 1; index2 <= 4; ++index2)
          this.AddFieldHandler("FE" + index2.ToString("00") + index1.ToString(), routine22);
      }
      this.AddFieldHandler("424", this.RoutineX(new Routine(this.calculateNegativeAmortizationIndicator)));
      Routine routine23 = this.RoutineX(new Routine(this.calculateVOEDoesNotApply));
      for (int index3 = 1; index3 <= 4; ++index3)
      {
        for (int index4 = 2; index4 <= 60; ++index4)
        {
          if ((index4 <= 2 || index4 >= 5) && (index4 <= 7 || index4 >= 10) && index4 != 11 && (index4 <= 12 || index4 >= 15) && index4 != 18 && (index4 <= 23 || index4 >= 51) && index4 != 57)
            this.AddFieldHandler("FE" + index3.ToString("00") + index4.ToString("00"), routine23);
        }
      }
      for (int index = 5; index < 7; ++index)
      {
        this.AddFieldHandler("FE" + index.ToString("00") + "02", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "60", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "58", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "59", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "05", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "06", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "07", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "17", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "10", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "51", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "14", routine23);
        this.AddFieldHandler("FE" + index.ToString("00") + "15", routine23);
      }
      this.AddFieldHandler("URLAROIS0002", this.CalcVOOIDoesNotApply);
      this.AddFieldHandler("URLARGG0019", this.RoutineX(new Routine(this.calculateGiftGrantSourceOtherDesc)));
      Routine routine24 = this.RoutineX(new Routine(this.calculateRepaymentTypecode));
      this.AddFieldHandler("424", routine24);
      this.AddFieldHandler("URLA.X239", routine24);
      Routine routine25 = this.RoutineX(new Routine(this.copySelfEmployedToBaseIncome));
      this.AddFieldHandler("FE0115", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome);
      this.AddFieldHandler("FE0215", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome);
      this.AddFieldHandler("FE0315", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome);
      this.AddFieldHandler("FE0415", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome);
      this.AddFieldHandler("FE0515", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("FE0615", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome + this.calObjs.VERIFCal.CalcPreviousGrossIncome);
      this.AddFieldHandler("BE0015", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome);
      this.AddFieldHandler("CE0015", routine25 + this.calObjs.VERIFCal.CalcMonthlyIncome);
      this.AddFieldHandler("1066", this.RoutineX(this.CalcPropertyLeaseholdExpirationDate));
      Routine routine26 = this.RoutineX(new Routine(this.calculateHomeOwnershipSubData));
      this.AddFieldHandler("URLA.X153", routine26);
      this.AddFieldHandler("URLA.X159", routine26);
      Routine routine27 = this.RoutineX(new Routine(this.calculateHomeEducationSubData));
      this.AddFieldHandler("URLA.X299", routine27);
      this.AddFieldHandler("URLA.X300", routine27);
      Routine routine28 = this.RoutineX(new Routine(this.calculateLockRequestPropertyAddress));
      this.AddFieldHandler("4516", routine28);
      this.AddFieldHandler("4517", routine28);
      this.AddFieldHandler("4518", routine28);
      this.AddFieldHandler("FM0055", this.RoutineX(new Routine(this.clearVOMFutureProposeUsageTypeOtherDesc)));
      Routine routine29 = this.RoutineX(new Routine(this.clearPurchaseCreditSourceType));
      this.AddFieldHandler("202", routine29);
      this.AddFieldHandler("1091", routine29);
      this.AddFieldHandler("1106", routine29);
      this.AddFieldHandler("1646", routine29);
      Routine routine30 = this.RoutineX(this.ClearCountryofCitizenship);
      this.AddFieldHandler("URLA.X1", routine30);
      this.AddFieldHandler("URLA.X2", routine30);
      Routine routine31 = this.RoutineX(new Routine(this.clearDeclarationFields));
      this.AddFieldHandler("418", routine31);
      this.AddFieldHandler("1343", routine31);
      Routine routine32 = this.RoutineX(new Routine(this.clearAllForeignAddressBlocks));
      this.AddFieldHandler("FR0129", routine32);
      this.AddFieldHandler("FR0229", routine32);
      this.AddFieldHandler("FR0329", routine32);
      this.AddFieldHandler("FR0429", routine32);
      this.AddFieldHandler("URLA.X267", routine32);
      this.AddFieldHandler("URLA.X268", routine32);
      this.AddFieldHandler("BR0029", routine32);
      this.AddFieldHandler("CR0029", routine32);
      this.AddFieldHandler("BR0039", routine32);
      this.AddFieldHandler("CR0039", routine32);
      this.AddFieldHandler("FE0180", routine32);
      this.AddFieldHandler("FE0280", routine32);
      this.AddFieldHandler("FE0380", routine32);
      this.AddFieldHandler("FE0480", routine32);
      this.AddFieldHandler("FE0580", routine32);
      this.AddFieldHandler("FE0680", routine32);
      this.AddFieldHandler("BE0080", routine32);
      this.AddFieldHandler("CE0080", routine32);
      this.AddFieldHandler("CAPIAP.X62", routine32);
      this.AddFieldHandler("CAPIAP.X63", routine32);
      this.AddFieldHandler("URLAROL0023", routine32);
      this.AddFieldHandler("DD0039", routine32);
      this.AddFieldHandler("FM0058", routine32);
      this.AddFieldHandler("FL0067", routine32);
      this.AddFieldHandler("4678", routine32);
      this.AddFieldHandler("DENIAL.X97", routine32);
      this.AddFieldHandler("DENIAL.X99", routine32);
      this.AddFieldHandler("NBOC0022", routine32);
      this.AddFieldHandler("4680", routine32);
      this.AddFieldHandler("VEND.X1047", routine32);
      this.AddFieldHandler("Seller3.ForeignAddressIndicator", routine32);
      this.AddFieldHandler("Seller4.ForeignAddressIndicator", routine32);
      this.AddFieldHandler("4880", routine32);
      Routine routine33 = this.RoutineX(new Routine(this.clearDenialSubjectAddressFields));
      this.AddFieldHandler("DENIAL.X81", routine33);
      this.AddFieldHandler("DENIAL.X86", routine33);
      this.AddFieldHandler("4004", this.RoutineX(new Routine(this.calculateCountryCode)));
      this.AddFieldHandler("URLA.X210", this.RoutineX(this.UpdateLinkedVoal));
      this.AddFieldHandler("URLA.X136", this.RoutineX(new Routine(this.calculateTitleInWhichPropertyHeld)));
      Routine routine34 = this.RoutineX(new Routine(this.calculateUrlaItemizedCredits));
      this.AddFieldHandler("4796", routine34);
      this.AddFieldHandler("1825", routine34);
      Routine routine35 = this.RoutineX(new Routine(this.calculateAverageRepresentativeCreditScore));
      this.AddFieldHandler("4752", routine35);
      this.AddFieldHandler("4830", routine35);
      this.AddFieldHandler("URLA.X309", this.RoutineX(new Routine(this.calculateAccessoryDwellingUnit)));
      this.AddFieldHandler("4947", this.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcHousingExp + this.calObjs.D1003Cal.CalcLoansubAndTSUM);
      this.AddFieldHandler("1726", this.calObjs.D1003Cal.CalcHousingExp + this.CalcSupplementalInsuranceAmount + this.calObjs.D1003Cal.CalcLoansubAndTSUM);
      Routine routine36 = this.RoutineX(new Routine(this.calculateCreditReportIndicators));
      this.AddFieldHandler("4750", routine36);
      this.AddFieldHandler("5006", routine36);
      Routine routine37 = this.RoutineX(new Routine(this.copyBorrowerUrlaCounselingFormat));
      this.AddFieldHandler("URLA.X301", routine37);
      this.AddFieldHandler("URLA.X154", routine37);
      Routine routine38 = this.RoutineX(new Routine(this.copyCoBorrowerUrlaCounselingFormat));
      this.AddFieldHandler("URLA.X302", routine38);
      this.AddFieldHandler("URLA.X160", routine38);
    }

    internal void SwitchToURLA2020(string id, string val)
    {
      if (id == "1825" && val.Equals("2020"))
      {
        this.calculateLoanRepaymentType(id, val);
        this.calculateRepaymentTypecode(id, val);
        this.copyVomPropertyType(id, val);
        this.copyVodBalanceFrom2009To2020();
        if (this.loan.RefreshURLA2020Fields)
        {
          this.recalculateConstructionLoanStaticFields();
          this.CopyConstructionLoanRepeatableDataFields();
        }
        this.copyAggregateIncomeTo2020();
        this.linkFinanceRecToVoal();
        this.calculateURLALoanIdentifier(id, val);
        this.calObjs.VERIFCal.CalcAppliedToDownpayment(id, val);
        this.calObjs.VERIFCal.CalSubFin(id, val);
      }
      else
      {
        int index1 = 0;
        string id1 = this.loan.CurrentBorrowerPair.Id;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index2]);
          this.calObjs.D1003Cal.CalcTotalOtherAssets(id, val);
          if (id1 == borrowerPairs[index2].Id)
            index1 = index2;
        }
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
        this.calObjs.VERIFCal.CalSubFin(id, val);
      }
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      for (int index = 1; index <= numberOfMortgages; ++index)
        this.calObjs.VERIFCal.CalculateVOM("FM" + index.ToString("00") + "20", "IMPORT");
      this.calculateProposedSupplementalPropertyInsurance(id, val);
      this.calObjs.D1003URLA2020Cal.CalcSupplementalInsuranceAmount(id, val);
      this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount(id, val);
      this.calObjs.D1003Cal.CalcHousingExp(id, val);
      this.concatenateURLAAddresses(id, val);
      this.concatenateNames(id, val);
      this.calculateMannerTitleHeld(id, val);
      this.calculateTitleInWhichPropertyHeld(id, val);
      this.includeVerificationsOnAUSExport(id, val);
      this.recalculateVOMOwnedBy(id, val);
      this.syncEmploymentStartDate(id, val);
      this.calculateRefinanceType(id, val);
      this.calculateLockRequestPropertyAddress(id, val);
      this.calObjs.Cal.CalcDisclosureReadyDate(id, val);
      this.calObjs.Cal.CalcAtAppDisclosureDate(id, val);
      this.calObjs.Cal.CalcAtLockDisclosureDate(id, val);
      this.calObjs.Cal.CalcChangeCircumstanceRequirementsDate(id, val);
      if (!(id == "1825"))
        return;
      this.calculateLoanRepaymentType(id, val);
    }

    private void linkFinanceRecToVoal()
    {
      int ofAdditionalLoans = this.loan.GetNumberOfAdditionalLoans();
      for (int index = 1; index <= ofAdditionalLoans && ofAdditionalLoans > 0; ++index)
        this.loan.RemoveAdditionalLoanAt(index - 1);
      string val1 = this.Val("4487");
      string val2 = this.Val("4488");
      string val3 = this.Val("4489");
      string val4 = this.Val("4490");
      int num = 1;
      if (!string.IsNullOrEmpty(val1))
      {
        this.SetVal("URLARAL" + num.ToString("00") + "16", "Mortgage");
        this.SetVal("URLARAL" + num.ToString("00") + "17", "1");
        this.SetVal("URLARAL" + num.ToString("00") + "20", val1);
        ++num;
      }
      if (!string.IsNullOrEmpty(val2))
      {
        this.SetVal("URLARAL" + num.ToString("00") + "16", "Mortgage");
        this.SetVal("URLARAL" + num.ToString("00") + "17", "2");
        this.SetVal("URLARAL" + num.ToString("00") + "20", val2);
        ++num;
      }
      if (!string.IsNullOrEmpty(val3) || !string.IsNullOrEmpty(val4))
      {
        this.SetVal("URLARAL" + num.ToString("00") + "16", "HELOC");
        this.SetVal("URLARAL" + num.ToString("00") + "21", val3);
        this.SetVal("URLARAL" + num.ToString("00") + "20", val4);
      }
      this.calObjs.VERIFCal.CalcAdditionalLoansAmount((string) null, (string) null);
    }

    private void recalculateConstructionLoanStaticFields()
    {
      string val1 = string.Empty;
      string val2 = string.Empty;
      string val3 = string.Empty;
      switch (this.Val("19"))
      {
        case "ConstructionOnly":
          if (this.Val("1964") == "Y" && this.Val("Constr.Refi") != "Y" && this.Val("4084") == "Y")
          {
            val1 = "Purchase";
            val2 = "Y";
            val3 = "OneClosing";
          }
          else if (this.Val("1964") != "Y" && this.Val("Constr.Refi") == "Y" && this.Val("4084") == "Y")
          {
            val1 = "Refinance";
            val2 = "Y";
            val3 = "OneClosing";
          }
          else if (this.Val("1964") != "Y" && this.Val("Constr.Refi") != "Y" && this.Val("4084") == "Y")
          {
            val1 = "Refinance";
            val2 = "Y";
            val3 = "OneClosing";
          }
          else if (this.Val("1964") == "Y" && this.Val("Constr.Refi") != "Y" && this.Val("4084") != "Y")
          {
            val1 = "Other";
            val2 = "N";
          }
          else if (this.Val("1964") != "Y" && this.Val("4084") != "Y")
          {
            val1 = "Other";
            val2 = "N";
          }
          this.SetVal("URLA.X133", val2);
          this.SetVal("URLA.X134", val3);
          break;
        case "ConstructionToPermanent":
          if (this.Val("1964") == "Y" && this.Val("Constr.Refi") != "Y")
          {
            val1 = "Purchase";
            val2 = "Y";
            val3 = "OneClosing";
          }
          else if (this.Val("1964") != "Y" && this.Val("Constr.Refi") == "Y")
          {
            val1 = "Refinance";
            val2 = "Y";
            val3 = "OneClosing";
          }
          else if (this.Val("1964") != "Y" && this.Val("Constr.Refi") != "Y")
          {
            val1 = "Refinance";
            val2 = "Y";
            val3 = "OneClosing";
          }
          this.SetVal("URLA.X133", val2);
          this.SetVal("URLA.X134", val3);
          break;
        case "Purchase":
          val1 = "Purchase";
          break;
        case "NoCash-Out Refinance":
          val1 = "Refinance";
          break;
        case "Cash-Out Refinance":
          val1 = "Refinance";
          break;
        case "Other":
          val1 = "Other";
          break;
      }
      this.SetVal("URLA.X71", val1);
    }

    private void calculateURLALoanPurposeMapping(string id, string val)
    {
      string empty = string.Empty;
      string str1 = this.Val("1964");
      string str2 = this.Val("Constr.Refi");
      string str3 = this.Val("19");
      string val1 = string.Empty;
      string val2;
      switch (str3)
      {
        case "Purchase":
          val2 = "Purchase";
          break;
        case "Cash-Out Refinance":
        case "NoCash-Out Refinance":
          val2 = "Refinance";
          break;
        case "ConstructionOnly":
          if (str2 == "Y")
          {
            val2 = "Refinance";
            break;
          }
          if (str1 == "Y")
          {
            val2 = "Purchase";
            break;
          }
          val2 = "Other";
          val1 = "Construction Only";
          break;
        case "ConstructionToPermanent":
          val2 = !(str2 == "Y") ? (!(str1 == "Y") ? "Refinance" : "Purchase") : "Refinance";
          break;
        case "Other":
          val2 = "Other";
          val1 = this.Val("9");
          break;
        default:
          val2 = string.Empty;
          break;
      }
      this.SetVal("URLA.X71", val2);
      this.SetVal("9", val1);
    }

    private void CopyConstructionLoanRepeatableDataFields()
    {
      int index1 = 0;
      double num1 = 0.0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int num2 = 1;
        int num3 = 1;
        int num4 = 8;
        int num5 = 48;
        int num6 = 24;
        int numberOfDeposits = this.loan.GetNumberOfDeposits();
        int num7;
        if (numberOfDeposits > 0)
        {
          num3 = this.CreateOtherAssetsRecords(numberOfDeposits);
          num7 = numberOfDeposits + 1;
        }
        else
          num7 = 1;
        int num8 = 1;
        for (int index3 = 0; index3 < D1003URLA2020Calculation.ConstructionFields.Length; ++index3)
        {
          string constructionField = D1003URLA2020Calculation.ConstructionFields[index3];
          string id2 = "URLAROIS" + num2.ToString("00") + "22";
          string id3 = "URLAROIS" + num2.ToString("00") + "02";
          string id4 = "URLAROIS" + num2.ToString("00") + "18";
          string id5 = "URLAROIS" + num2.ToString("00") + "19";
          string id6 = "URLAROA" + num3.ToString("00") + "01";
          string id7 = "URLAROA" + num3.ToString("00") + "02";
          string id8 = "URLAROA" + num3.ToString("00") + "03";
          string id9 = "URLAROA" + num3.ToString("00") + "04";
          string id10 = "URLAROL" + num8.ToString("00") + "01";
          string id11 = "URLAROL" + num8.ToString("00") + "02";
          string id12 = "URLAROL" + num8.ToString("00") + "04";
          string id13 = "URLAROL" + num8.ToString("00") + "03";
          string id14 = "URLAROL" + num8.ToString("00") + "19";
          string id15 = "DD" + num7.ToString("00") + num4.ToString("00");
          string id16 = "DD" + num7.ToString("00") + num5.ToString("00");
          string id17 = "DD" + num7.ToString("00") + "02";
          string id18 = "DD" + num7.ToString("00") + num6.ToString("00");
          switch (constructionField)
          {
            case "105":
              if (this.FltVal(constructionField) > 0.0)
              {
                this.SetCurrentNum(id2, this.FltVal(constructionField));
                this.SetVal(id3, "Borrower");
                this.SetVal(id4, "DividendsInterest");
                ++num2;
                break;
              }
              break;
            case "1053":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("1052"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "1055":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("1054"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "1062":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id10, "Both");
                this.SetVal(id11, "Other");
                this.SetVal(id12, this.Val("1058"));
                this.SetCurrentNum(id13, this.FltVal(constructionField));
                this.SetVal(id14, this.Val("1837"));
                ++num8;
                break;
              }
              break;
            case "114":
              if (this.FltVal(constructionField) > 0.0)
              {
                this.SetCurrentNum(id2, this.FltVal(constructionField));
                this.SetVal(id3, "CoBorrower");
                this.SetVal(id4, "DividendsInterest");
                ++num2;
                break;
              }
              break;
            case "146":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("144") == "B")
                  this.SetVal(id3, "Borrower");
                else if (this.Val("144") == "C")
                  this.SetVal(id3, "CoBorrower");
                else if (string.IsNullOrEmpty(this.Val("144")))
                  this.SetVal(id3, "");
                if (!string.IsNullOrEmpty(this.Val("145")))
                {
                  this.SetVal(id4, this.ConvertEnumerationTo2020(this.Val("145")));
                  this.SetVal(id5, this.ConvertDescriptionForEnumerationFor2020(this.Val("145")));
                }
                this.SetCurrentNum(id2, this.FltVal(constructionField));
                ++num2;
                break;
              }
              break;
            case "149":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("147") == "B")
                  this.SetVal(id3, "Borrower");
                else if (this.Val("147") == "C")
                  this.SetVal(id3, "CoBorrower");
                else if (string.IsNullOrEmpty(this.Val("147")))
                  this.SetVal(id3, "");
                if (!string.IsNullOrEmpty(this.Val("148")))
                {
                  this.SetVal(id4, this.ConvertEnumerationTo2020(this.Val("148")));
                  this.SetVal(id5, this.ConvertDescriptionForEnumerationFor2020(this.Val("148")));
                }
                this.SetCurrentNum(id2, this.FltVal(constructionField));
                ++num2;
                break;
              }
              break;
            case "152":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("150") == "B")
                  this.SetVal(id3, "Borrower");
                else if (this.Val("150") == "C")
                  this.SetVal(id3, "CoBorrower");
                else if (string.IsNullOrEmpty(this.Val("150")))
                  this.SetVal(id3, "");
                if (!string.IsNullOrEmpty(this.Val("151")))
                {
                  this.SetVal(id4, this.ConvertEnumerationTo2020(this.Val("151")));
                  this.SetVal(id5, this.ConvertDescriptionForEnumerationFor2020(this.Val("151")));
                }
                this.SetCurrentNum(id2, this.FltVal(constructionField));
                ++num2;
                break;
              }
              break;
            case "1605":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("1604"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "1607":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("1606"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "1609":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("1608"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "1718":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("1717"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "210":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id18, "Both");
                this.SetVal(id15, "LifeInsurance");
                this.SetCurrentNum(id16, this.FltVal(constructionField));
                this.SetVal(id17, "Converted From 2009");
                ++num7;
                break;
              }
              break;
            case "212":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id18, "Both");
                this.SetVal(id15, "RetirementFund");
                this.SetVal(id17, "Converted From 2009");
                this.SetCurrentNum(id16, this.FltVal(constructionField));
                ++num7;
                break;
              }
              break;
            case "213":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, "Net Worth of Business Owned");
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "215":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("214"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "217":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("216"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "222":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("221"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "224":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id6, "Both");
                this.SetVal(id7, "Other");
                this.SetVal(id9, this.Val("223"));
                this.SetCurrentNum(id8, this.FltVal(constructionField));
                ++num3;
                break;
              }
              break;
            case "256":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id10, "Both");
                this.SetVal(id11, "JobRelatedExpenses");
                this.SetCurrentNum(id13, this.FltVal(constructionField));
                this.SetVal(id14, this.Val("1836"));
                ++num8;
                break;
              }
              break;
            case "272":
              if (this.FltVal(constructionField) > 0.0)
              {
                if (this.Val("181") == "Jointly")
                  this.SetVal(id10, "Both");
                this.SetVal(id11, "Other");
                this.SetVal(id12, this.Val("271"));
                this.SetCurrentNum(id13, this.FltVal(constructionField));
                this.SetVal(id14, this.Val("1835"));
                ++num8;
                break;
              }
              break;
          }
        }
        double subTotal = 0.0;
        this.recalculateVerifications((string) null, (string) null, out subTotal);
        num1 += subTotal;
        this.calculateVODTotalDeposits((string) null, (string) null);
        this.calculateVOOATotalAssets((string) null, (string) null);
        this.calObjs.VERIFCal.CalcOtherLiabilityMonthlyIncome((string) null, (string) null);
        this.calObjs.VERIFCal.CalcOtherIncome((string) null, (string) null);
        this.calObjs.D1003Cal.CalcOtherIncome((string) null, (string) null);
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.SetCurrentNum("1547", num1);
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void copyVomPropertyType(string id, string val)
    {
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        for (int index3 = 1; index3 <= numberOfMortgages; ++index3)
        {
          string str1 = "FM" + index3.ToString("00");
          string str2 = this.Val(str1 + "41");
          this.SetVal(str1 + "55", str2 == "Investor" ? "Investment" : str2);
        }
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private int CreateOtherAssetsRecords(int numberOfDeposits)
    {
      int otherAssetsRecords = 1;
      List<KeyValuePair<string, string>> keyValuePairList = (List<KeyValuePair<string, string>>) null;
      for (int index1 = 1; index1 <= numberOfDeposits; ++index1)
      {
        string str1 = index1.ToString("00");
        int num1 = 8;
        int num2 = 11;
        int num3 = 48;
        keyValuePairList?.Clear();
        bool flag1 = false;
        int num4;
        for (int index2 = 4; index2 < 20; index2 += 4)
        {
          string id1 = "DD" + str1 + num1.ToString("00");
          string id2 = "DD" + str1 + num2.ToString("00");
          string str2 = str1;
          num4 = num1 + 1;
          string str3 = num4.ToString("00");
          string id3 = "DD" + str2 + str3;
          string str4 = str1;
          num4 = num1 + 2;
          string str5 = num4.ToString("00");
          string id4 = "DD" + str4 + str5;
          string str6 = this.Val(id1);
          string str7 = this.ConvertDepositEnumerationTo2020(str6);
          string id5 = "DD" + str1 + num3.ToString("00");
          if (string.IsNullOrEmpty(str7) && this.FltVal(id2) > 0.0)
          {
            flag1 = true;
            if (keyValuePairList == null || keyValuePairList.Count == 0)
            {
              keyValuePairList = new List<KeyValuePair<string, string>>();
              foreach (KeyValuePair<string, string> keyValuePair in this.vodOtherAssetsMapping)
                keyValuePairList.Add(new KeyValuePair<string, string>(keyValuePair.Value, this.Val("DD" + str1 + keyValuePair.Key)));
            }
            string str8 = otherAssetsRecords.ToString("00");
            string id6 = "URLAROA" + str8 + "02";
            string id7 = "URLAROA" + str8 + "03";
            string id8 = "URLAROA" + str8 + "04";
            if (str6 == "CashOnHand")
            {
              this.SetVal(id6, str6);
            }
            else
            {
              this.SetVal(id6, "Other");
              this.SetVal(id8, this.GetDepositEnumerationToDesc2020(str6));
            }
            if (keyValuePairList != null)
            {
              foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList)
                this.SetVal("URLAROA" + otherAssetsRecords.ToString("00") + keyValuePair.Key, keyValuePair.Value);
            }
            this.SetCurrentNum(id7, this.FltVal(id2));
            ++otherAssetsRecords;
            this.SetVal(id1, "");
            this.SetVal(id2, "");
            this.SetVal(id3, "");
            this.SetVal(id4, "");
            this.SetVal(id5, "");
          }
          num1 += 4;
          num2 += 4;
          ++num3;
        }
        if (flag1)
        {
          List<string[]> strArrayList = new List<string[]>();
          int num5 = 8;
          int num6 = 48;
          for (int index3 = 4; index3 < 20; index3 += 4)
          {
            string id9 = "DD" + str1 + num5.ToString("00");
            string str9 = str1;
            num4 = num5 + 1;
            string str10 = num4.ToString("00");
            string id10 = "DD" + str9 + str10;
            string str11 = str1;
            num4 = num5 + 2;
            string str12 = num4.ToString("00");
            string id11 = "DD" + str11 + str12;
            string str13 = str1;
            num4 = num5 + 3;
            string str14 = num4.ToString("00");
            string id12 = "DD" + str13 + str14;
            string id13 = "DD" + str1 + num6.ToString("00");
            strArrayList.Add(new string[5]
            {
              this.Val(id9),
              this.Val(id10),
              this.Val(id11),
              this.Val(id12),
              this.Val(id13)
            });
            bool flag2 = true;
            for (int index4 = 0; index4 < strArrayList[strArrayList.Count - 1].Length; ++index4)
            {
              if (!string.IsNullOrEmpty(strArrayList[strArrayList.Count - 1][index4]))
              {
                flag2 = false;
                break;
              }
            }
            if (flag2)
            {
              strArrayList.RemoveAt(strArrayList.Count - 1);
            }
            else
            {
              this.SetVal(id9, "");
              this.SetVal(id10, "");
              this.SetVal(id11, "");
              this.SetVal(id12, "");
              this.SetVal(id13, "");
            }
            num5 += 4;
            ++num6;
          }
          if (strArrayList != null && strArrayList.Count > 0)
          {
            int num7 = 8;
            int num8 = 48;
            for (int index5 = 0; index5 < strArrayList.Count; ++index5)
            {
              string id14 = "DD" + str1 + num7.ToString("00");
              string str15 = str1;
              num4 = num7 + 1;
              string str16 = num4.ToString("00");
              string id15 = "DD" + str15 + str16;
              string str17 = str1;
              num4 = num7 + 2;
              string str18 = num4.ToString("00");
              string id16 = "DD" + str17 + str18;
              string str19 = str1;
              num4 = num7 + 3;
              string str20 = num4.ToString("00");
              string id17 = "DD" + str19 + str20;
              string id18 = "DD" + str1 + num8.ToString("00");
              this.SetVal(id14, strArrayList[index5][0]);
              this.SetVal(id15, strArrayList[index5][1]);
              this.SetVal(id16, strArrayList[index5][2]);
              this.SetVal(id17, strArrayList[index5][3]);
              this.SetVal(id18, strArrayList[index5][4]);
              num7 += 4;
              ++num8;
            }
          }
        }
      }
      return otherAssetsRecords;
    }

    private void copyVodBalanceFrom2009To2020()
    {
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        for (int index3 = 1; index3 <= numberOfDeposits; ++index3)
        {
          int num1 = 8;
          int num2 = 11;
          int num3 = 48;
          for (int index4 = 1; index4 <= 4; ++index4)
          {
            string id2 = "DD" + index3.ToString("00") + num2.ToString("00");
            string id3 = "DD" + index3.ToString("00") + num3.ToString("00");
            if (!string.IsNullOrEmpty(this.Val(id2)))
              this.SetCurrentNum(id3, this.FltVal(id2));
            num1 += 4;
            num2 += 4;
            ++num3;
          }
        }
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private string ConvertDepositEnumerationTo2020(string key)
    {
      string str = string.Empty;
      switch (key)
      {
        case "Bond":
          str = "Bond";
          break;
        case "BridgeLoanNotDeposited":
          str = "Bridge Loan Not Deposited";
          break;
        case "CertificateOfDepositTimeDeposit":
          str = "Certificate Of Deposit";
          break;
        case "CheckingAccount":
          str = "Checking Account";
          break;
        case "IndividualDevelopmentAccount":
          str = "Individual Development Account";
          break;
        case "LifeInsurance":
          str = "Life Insurance";
          break;
        case "MoneyMarketFund":
          str = "Money Market Fund";
          break;
        case "MutualFund":
          str = "Mutual Fund";
          break;
        case "Other":
          str = "Other";
          break;
        case "RetirementFund":
          str = "Retirement Fund";
          break;
        case "SavingsAccount":
          str = "Savings Account";
          break;
        case "Stock":
          str = "Stock";
          break;
        case "StockOptions":
          str = "Stock Options";
          break;
        case "TrustAccount":
          str = "Trust Account";
          break;
      }
      return str;
    }

    private string GetDepositEnumerationToDesc2020(string key)
    {
      string enumerationToDesc2020 = string.Empty;
      switch (key)
      {
        case "Bond":
          enumerationToDesc2020 = "Bond";
          break;
        case "BridgeLoanNotDeposited":
          enumerationToDesc2020 = "Bridge Loan Not Deposited";
          break;
        case "CashDepositOnSalesContract":
          enumerationToDesc2020 = "Cash Deposit On Sales Contract";
          break;
        case "CashOnHand":
          enumerationToDesc2020 = "Cash On Hand";
          break;
        case "CertificateOfDepositTimeDeposit":
          enumerationToDesc2020 = "Certificate Of Deposit";
          break;
        case "CheckingAccount":
          enumerationToDesc2020 = "Checking Account";
          break;
        case "GiftOfEquity":
          enumerationToDesc2020 = "Gift Of Equity";
          break;
        case "GiftsNotDeposited":
          enumerationToDesc2020 = "Gifts Not Deposited";
          break;
        case "GiftsTotal":
          enumerationToDesc2020 = "Gifts Total";
          break;
        case "IndividualDevelopmentAccount":
          enumerationToDesc2020 = "Individual Development Account";
          break;
        case "LifeInsurance":
          enumerationToDesc2020 = "Life Insurance";
          break;
        case "MoneyMarketFund":
          enumerationToDesc2020 = "Money Market Fund";
          break;
        case "MutualFund":
          enumerationToDesc2020 = "Mutual Fund";
          break;
        case "NetEquity":
          enumerationToDesc2020 = "Net Equity";
          break;
        case "NetWorthOfBusinessOwned":
          enumerationToDesc2020 = "Net Worth Of Business Owned";
          break;
        case "Other":
          enumerationToDesc2020 = "Other";
          break;
        case "OtherLiquidAssets":
          enumerationToDesc2020 = "Other Liquid Assets";
          break;
        case "OtherNonLiquidAssets":
          enumerationToDesc2020 = "Other Non Liquid Assets";
          break;
        case "PendingNetSaleProceedsFromRealEstateAssets":
          enumerationToDesc2020 = "Net Proceeds From Real Estate Assets";
          break;
        case "RetirementFund":
          enumerationToDesc2020 = "Retirement Fund";
          break;
        case "SavingsAccount":
          enumerationToDesc2020 = "Savings Account";
          break;
        case "SecuredBorrowedFundsNotDeposited":
          enumerationToDesc2020 = "Secured Borrowed Funds Not Deposited";
          break;
        case "Stock":
          enumerationToDesc2020 = "Stock";
          break;
        case "StockOptions":
          enumerationToDesc2020 = "Stock Options";
          break;
        case "TrustAccount":
          enumerationToDesc2020 = "Trust Account";
          break;
      }
      return enumerationToDesc2020;
    }

    private string ConvertEnumerationTo2020(string key)
    {
      switch (key)
      {
        case "AccessoryUnitIncome":
          return "AccessoryUnitIncome";
        case "AlimonyChildSupport":
        case "EmploymentRelatedAssets":
        case "ForeignIncome":
        case "MilitaryBasePay":
        case "MilitaryClothesAllowance":
        case "MilitaryCombatPay":
        case "MilitaryFlightPay":
        case "MilitaryHazardPay":
        case "MilitaryOverseasPay":
        case "MilitaryPropPay":
        case "MilitaryQuartersAllowance":
        case "MilitaryRationsAllowance":
        case "MilitaryVariableHousingAllowance":
        case "OtherIncome":
        case "SeasonalIncome":
          return "Other";
        case "AutomobileExpenseAccount":
          return "AutomobileAllowance";
        case "CapitalGains":
          return "CapitalGains";
        case "FNMBoarderIncome":
          return "BoarderIncome";
        case "FNMGovernmentMortgageCreditCertificate":
          return "MortgageCreditCertificate";
        case "FosterCare":
          return "FosterCare";
        case "MortgageDifferential":
          return "MortgageDifferential";
        case "Non-borrowerHouseholdIncome":
          return "NonBorrowerHouseholdIncome";
        case "NotesReceivableInstallment":
          return "NotesReceivableInstallment";
        case "Pension":
          return "Pension";
        case "RoyaltyPayment":
          return "Royalties";
        case "Section8":
          return "PublicAssistance";
        case "SocialSecurity":
          return "SocialSecurity";
        case "TemporaryLeave":
          return "TemporaryLeave";
        case "TipIncome":
          return "TipIncome";
        case "Trust":
          return "Trust";
        case "Unemployment":
          return "Unemployment";
        case "VABenefitsNonEducational":
          return "VABenefitsNonEducational";
        default:
          return "";
      }
    }

    private string ConvertDescriptionForEnumerationFor2020(string key)
    {
      string str = "";
      switch (key)
      {
        case "AlimonyChildSupport":
          str = "AlimonyChildSupport";
          break;
        case "EmploymentRelatedAssets":
          str = "EmploymentRelatedAssets";
          break;
        case "ForeignIncome":
          str = "ForeignIncome";
          break;
        case "MilitaryBasePay":
          str = "MilitaryBasePay";
          break;
        case "MilitaryClothesAllowance":
          str = "MilitaryClothesAllowance";
          break;
        case "MilitaryCombatPay":
          str = "MilitaryCombatPay";
          break;
        case "MilitaryFlightPay":
          str = "MilitaryFlightPay";
          break;
        case "MilitaryHazardPay":
          str = "MilitaryHazardPay";
          break;
        case "MilitaryOverseasPay":
          str = "MilitaryOverseasPay";
          break;
        case "MilitaryPropPay":
          str = "MilitaryPropPay";
          break;
        case "MilitaryQuartersAllowance":
          str = "MilitaryQuartersAllowance";
          break;
        case "MilitaryRationsAllowance":
          str = "MilitaryRationsAllowance";
          break;
        case "MilitaryVariableHousingAllowance":
          str = "MilitaryVariableHousingAllowance";
          break;
        case "SeasonalIncome":
          str = "SeasonalIncome";
          break;
      }
      return str;
    }

    private void copyAggregateIncomeTo2020()
    {
      int index1 = 0;
      string id = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
        bool flag1 = false;
        if (numberOfEmployer1 == 0)
        {
          foreach (KeyValuePair<int, string> aggBorrIncomeField in this.aggBorrIncomeFields)
          {
            string val = this.Val(aggBorrIncomeField.Value);
            if (!string.IsNullOrEmpty(val))
            {
              this.SetVal("BE01" + (object) aggBorrIncomeField.Key, val);
              flag1 = true;
            }
          }
          if (flag1)
          {
            this.SetVal("BE0109", "Y");
            this.SetVal("BE0108", "Borrower");
            this.calObjs.VERIFCal.CalcMonthlyIncome("BE0112", (string) null);
          }
        }
        int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
        bool flag2 = false;
        if (numberOfEmployer2 == 0)
        {
          foreach (KeyValuePair<int, string> coBorrIncomeField in this.aggCoBorrIncomeFields)
          {
            string val = this.Val(coBorrIncomeField.Value);
            if (!string.IsNullOrEmpty(val))
            {
              this.SetVal("CE01" + (object) coBorrIncomeField.Key, val);
              flag2 = true;
            }
          }
          if (flag2)
          {
            this.SetVal("CE0109", "Y");
            this.SetVal("CE0108", "CoBorrower");
            this.calObjs.VERIFCal.CalcMonthlyIncome("CE0112", (string) null);
          }
        }
        this.calculateAggregateIncome((string) null, (string) null);
        if (id == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateVOOATotalAssets(string id, string val)
    {
      int numberOfOtherAssets = this.loan.GetNumberOfOtherAssets();
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      for (int index = 1; index <= numberOfOtherAssets; ++index)
      {
        string str = "URLAROA" + index.ToString("00");
        double num5 = this.FltVal(str + "03");
        switch (this.Val(str + "01"))
        {
          case "Both":
            num1 += num5;
            num2 += num5;
            break;
          case "Borrower":
            num1 += num5;
            break;
          case "CoBorrower":
            num2 += num5;
            break;
        }
        num3 += num5;
        if (index > 3)
          num4 += num5;
      }
      this.SetCurrentNum("URLA.X57", num1);
      this.SetCurrentNum("URLA.X58", num2);
      this.SetCurrentNum("URLA.X53", num4);
      this.SetCurrentNum("URLA.X54", num3);
      this.calObjs.D1003Cal.CalcTotalOtherAssets(id, val);
    }

    private void calculateVOOADoesNotApply(string id, string val)
    {
      this.calculateDoNotApply(this.loan.GetNumberOfOtherAssets(), "URLAROA", "01", "URLA.X51", "URLA.X52");
    }

    private void calculateVOLDoesNotApply(string id, string val)
    {
      this.calculateDoNotApply(this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp(), "FL", "15", "URLA.X59", "URLA.X60");
    }

    private void calculateVOOLDoesNotApply(string id, string val)
    {
      this.calculateDoNotApply(this.loan.GetNumberOfOtherLiability(), "URLAROL", "01", "URLA.X63", "URLA.X64");
    }

    private void calculateVOMDoesNotApply(string id, string val)
    {
      this.calculateDoNotApply(this.loan.GetNumberOfMortgages(), "FM", "46", "URLA.X69", "URLA.X110");
    }

    private void calculateVOGGDoesNotApply(string id, string val)
    {
      this.calculateDoNotApply(this.loan.GetNumberOfGiftsAndGrants(), "URLARGG", "02", "URLA.X82", "URLA.X83");
    }

    private void calculatePriorAddressDoesNotApply(string id, string val)
    {
      if (this.checkPriorAddressExists(true))
        this.SetVal("URLA.X265", "N");
      if (!this.checkPriorAddressExists(false))
        return;
      this.SetVal("URLA.X266", "N");
    }

    private bool checkPriorAddressExists(bool isBorrower)
    {
      string str = isBorrower ? "BR" : "CR";
      int numberOfResidence = this.loan.GetNumberOfResidence(isBorrower);
      for (int index = 1; index <= numberOfResidence; ++index)
      {
        if (this.Val(str + index.ToString("00") + "23") == "Prior")
          return true;
      }
      return false;
    }

    private void calculateDoNotApply(
      int recCount,
      string prefixID,
      string borTypeID,
      string borDoNotApplyID,
      string cobDoNotApplyID)
    {
      string val1 = (string) null;
      string val2 = (string) null;
      for (int index = 1; index <= recCount; ++index)
      {
        string str = this.Val(prefixID + index.ToString("00") + borTypeID);
        if (str == "Both")
        {
          val1 = "N";
          val2 = "N";
        }
        else if (str == "Borrower" && val1 == null)
          val1 = "N";
        else if (str == "CoBorrower" && val2 == null)
          val2 = "N";
        if (val1 != null && val2 != null)
          break;
      }
      if (val1 != null)
        this.SetVal(borDoNotApplyID, val1);
      if (val2 == null)
        return;
      this.SetVal(cobDoNotApplyID, val2);
    }

    public void UpdateCurrentMailingAddress()
    {
      this.copyCurrentToMailingAddress((string) null, (string) null);
    }

    private void copyCurrentToMailingAddress(string id, string val)
    {
      if (this.Val("1819") == "Y")
      {
        this.SetVal("1416", this.Val("FR0104"));
        this.SetVal("URLA.X7", this.Val("FR0125"));
        this.SetVal("URLA.X8", this.Val("FR0127"));
        this.SetVal("1417", this.Val("FR0106"));
        this.SetVal("1418", this.Val("FR0107"));
        this.SetVal("1419", this.Val("FR0108"));
        this.SetVal("URLA.X11", this.Val("FR0128"));
        this.SetVal("URLA.X197", this.Val("FR0126"));
      }
      if (!(this.Val("1820") == "Y"))
        return;
      this.SetVal("1519", this.Val("FR0204"));
      this.SetVal("URLA.X9", this.Val("FR0225"));
      this.SetVal("URLA.X10", this.Val("FR0227"));
      this.SetVal("1520", this.Val("FR0206"));
      this.SetVal("1521", this.Val("FR0207"));
      this.SetVal("1522", this.Val("FR0208"));
      this.SetVal("URLA.X12", this.Val("FR0228"));
      this.SetVal("URLA.X198", this.Val("FR0226"));
    }

    private void calculateURLALoanIdentifier(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      if (id == "URLA.X238" && val == "Y")
        this.SetVal("URLA.X119", "N");
      else if (id == "URLA.X119" && val == "Y")
        this.SetVal("URLA.X238", "N");
      if (this.Val("URLA.X238") == "Y")
      {
        if (this.Val("2573") == "UseInvestorNumber")
          this.SetVal("URLA.X120", this.Val("352") + "/" + this.Val("HMDA.X28"));
        else
          this.SetVal("URLA.X120", this.Val("364") + "/" + this.Val("HMDA.X28"));
      }
      else if (this.Val("URLA.X119") == "Y")
        this.SetVal("URLA.X120", this.Val("HMDA.X28"));
      else if (this.Val("2573") == "UseInvestorNumber")
        this.SetVal("URLA.X120", this.Val("352"));
      else
        this.SetVal("URLA.X120", this.Val("364"));
    }

    private void copyCitizenship(string id, string val)
    {
      switch (id)
      {
        case "URLA.X1":
          if (val == "USCitizen")
          {
            this.SetVal("965", "Y");
            this.SetVal("466", "N");
          }
          else
          {
            this.SetVal("965", "");
            this.SetVal("467", "");
          }
          if (val == "PermanentResidentAlien")
          {
            this.SetVal("466", "Y");
            this.SetVal("965", "N");
          }
          if (val == "NonPermanentResidentAlien")
          {
            this.SetVal("965", "N");
            this.SetVal("466", "N");
            break;
          }
          if (val != "NonPermanentResidentAlien" && val == "")
          {
            this.SetVal("965", "");
            this.SetVal("466", "");
            break;
          }
          break;
        case "965":
          if (val == "Y")
          {
            this.SetVal("URLA.X1", "USCitizen");
            break;
          }
          break;
        case "URLA.X2":
          if (val == "USCitizen")
          {
            this.SetVal("985", "Y");
            this.SetVal("467", "N");
          }
          else
          {
            this.SetVal("985", "");
            this.SetVal("467", "");
          }
          if (val == "PermanentResidentAlien")
          {
            this.SetVal("467", "Y");
            this.SetVal("985", "N");
          }
          if (val == "NonPermanentResidentAlien")
          {
            this.SetVal("985", "N");
            this.SetVal("467", "N");
            break;
          }
          if (val != "NonPermanentResidentAlien" && val == "")
          {
            this.SetVal("985", "");
            this.SetVal("467", "");
            break;
          }
          break;
        case "985":
          if (val == "Y")
          {
            this.SetVal("URLA.X2", "USCitizen");
            break;
          }
          break;
        case "466":
          if (val == "Y")
          {
            this.SetVal("URLA.X1", "PermanentResidentAlien");
            break;
          }
          break;
        case "467":
          if (val == "Y")
          {
            this.SetVal("URLA.X2", "PermanentResidentAlien");
            break;
          }
          break;
      }
      if (this.Val("965") == "N" && this.Val("985") == "N" && this.Val("466") == "N" && this.Val("467") == "N")
      {
        this.SetVal("URLA.X1", "NonPermanentResidentAlien");
        this.SetVal("URLA.X2", "NonPermanentResidentAlien");
      }
      if (this.Val("965") == "N" && this.Val("466") == "N" && this.Val("985") == "" && this.Val("467") == "")
        this.SetVal("URLA.X1", "NonPermanentResidentAlien");
      if (this.Val("985") == "N" && this.Val("467") == "N" && this.Val("965") == "" && this.Val("466") == "")
        this.SetVal("URLA.X2", "NonPermanentResidentAlien");
      if ((!(this.Val("965") == "N") || !(this.Val("985") == "N") || !(this.Val("466") == "") || !(this.Val("467") == "")) && (!(this.Val("965") == "") || !(this.Val("985") == "") || !(this.Val("466") == "N") || !(this.Val("467") == "N")) && (!(this.Val("965") == "") || !(this.Val("985") == "") || !(this.Val("466") == "") || !(this.Val("467") == "")))
        return;
      this.SetVal("URLA.X1", "");
      this.SetVal("URLA.X2", "");
    }

    private void updateMarriedStatus(string id, string val)
    {
      if (id == "52" && this.Val("52") != "Unmarried")
      {
        this.loan.SetField("URLA.X111", string.Empty);
        this.loan.SetField("URLA.X113", string.Empty);
        this.loan.SetField("URLA.X117", string.Empty);
      }
      if (!(id == "84") || !(this.Val("84") != "Unmarried"))
        return;
      this.loan.SetField("URLA.X112", string.Empty);
      this.loan.SetField("URLA.X114", string.Empty);
      this.loan.SetField("URLA.X118", string.Empty);
    }

    private void updateOtherRelationship(string id, string val)
    {
      if (this.Val("URLA.X113") != "Other")
        this.SetVal("URLA.X115", string.Empty);
      if (!(this.Val("URLA.X114") != "Other"))
        return;
      this.SetVal("URLA.X116", string.Empty);
    }

    private void updateMilitaryService(string id, string val)
    {
      if (this.Val("URLA.X13") != "Y")
      {
        this.loan.SetField("URLA.X123", string.Empty);
        this.loan.SetField("URLA.X124", string.Empty);
        this.loan.SetField("URLA.X125", string.Empty);
        this.loan.SetField("URLA.X17", string.Empty);
      }
      if (!(this.Val("URLA.X14") != "Y"))
        return;
      this.loan.SetField("URLA.X126", string.Empty);
      this.loan.SetField("URLA.X127", string.Empty);
      this.loan.SetField("URLA.X128", string.Empty);
      this.loan.SetField("URLA.X18", string.Empty);
    }

    private void clearOtherLanguageDescription(string id, string val)
    {
      if (this.Val("URLA.X21") != "OtherIndicator")
        this.loan.SetField("URLA.X35", string.Empty);
      if (!(this.Val("URLA.X22") != "OtherIndicator"))
        return;
      this.loan.SetField("URLA.X36", string.Empty);
    }

    private void calculateOwnerShipShare(string id, string val)
    {
      if (id.StartsWith("BE") || id.StartsWith("CE"))
      {
        string verifBlock = this.GetVerifBlock(id);
        this.setOwnershipField(val, verifBlock + "55");
      }
      else
      {
        switch (id)
        {
          case "FE0515":
          case "FE0615":
            bool borrower = id.Substring(2, 2) == "05";
            int numberOfEmployer = this.loan.GetNumberOfEmployer(borrower);
            string str = borrower ? "BE" : "CE";
            for (int index = 1; index <= numberOfEmployer; ++index)
            {
              if (this.Val(str + index.ToString("00") + "09") == "N")
              {
                this.setOwnershipField(val, str + index.ToString("00") + "55");
                break;
              }
            }
            break;
          case "FE0115":
            this.setOwnershipField(val, "FE0155");
            break;
          case "FE0215":
            this.setOwnershipField(val, "FE0255");
            break;
          case "FE0315":
            this.setOwnershipField(val, "FE0355");
            break;
          case "FE0415":
            this.setOwnershipField(val, "FE0455");
            break;
        }
      }
    }

    private void calculateProposedSupplementalPropertyInsurance(string id, string val)
    {
      if (!this.USEURLA2020)
      {
        this.SetVal("URLA.X144", "");
      }
      else
      {
        double num = this.Val("1801") != "Y" ? this.FltVal("235") : 0.0;
        if (this.Val("1802") != "Y" && SharedCalculations.IsInsuranceFee(this.Val("1628")))
          num += this.FltVal("1630");
        if (this.Val("1803") != "Y" && SharedCalculations.IsInsuranceFee(this.Val("660")))
          num += this.FltVal("253");
        if (this.Val("1804") != "Y" && SharedCalculations.IsInsuranceFee(this.Val("661")))
          num += this.FltVal("254");
        this.SetCurrentNum("URLA.X144", num);
      }
    }

    private void setOwnershipField(string fValue, string dID)
    {
      if (!(fValue != "Y"))
        return;
      this.SetVal(dID, string.Empty);
    }

    private void updateBorrowerAliasName(string id, string val)
    {
      string str;
      switch (id)
      {
        case "URLABAKA0101":
          str = "URLA.X195";
          break;
        case "URLACAKA0101":
          str = "URLA.X196";
          break;
        default:
          str = (string) null;
          break;
      }
      string id1 = str;
      if (id1 == null && (id == "URLA.X195" || id == "URLA.X196"))
        id1 = id;
      if (id1 == null)
        return;
      IList<URLAAlternateName> urlaAlternames = this.loan.GetURLAAlternames(id1 == "URLA.X195");
      if (urlaAlternames == null || urlaAlternames.Count == 0)
      {
        this.SetVal(id1, "");
      }
      else
      {
        string val1 = "";
        for (int index = 0; index < urlaAlternames.Count; ++index)
        {
          if (!(urlaAlternames[index].FullName == ""))
            val1 = val1 + (!(val1 != "") || !(urlaAlternames[index].FullName != "") ? "" : "; ") + urlaAlternames[index].FullName;
        }
        this.SetVal(id1, val1);
        switch (id1)
        {
          case "URLA.X195":
            this.SetVal("1869", val1);
            break;
          case "URLA.X196":
            this.SetVal("1874", val1);
            break;
        }
      }
    }

    private void calculateVODTotalDeposits(string id, string val)
    {
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      double num4 = 0.0;
      for (int index = 1; index <= numberOfDeposits; ++index)
      {
        string str = "DD" + index.ToString("00");
        double num5 = this.FltVal(str + "48") + this.FltVal(str + "49") + this.FltVal(str + "50") + this.FltVal(str + "51");
        switch (this.Val(str + "24"))
        {
          case "Both":
            num1 += num5;
            num2 += num5;
            break;
          case "Borrower":
            num1 += num5;
            break;
          case "CoBorrower":
            num2 += num5;
            break;
        }
        num3 += num5;
        if (index > 5)
          num4 += num5;
      }
      this.SetCurrentNum("URLA.X55", num1);
      this.SetCurrentNum("URLA.X56", num2);
      this.SetCurrentNum("URLA.X49", num4);
      this.SetCurrentNum("URLA.X50", num3);
    }

    private void calculateOccupancyDisplayField(string id, string val)
    {
      string str1 = this.Val("1811");
      string str2 = this.Val("1172");
      bool flag = this.Val("URLA.X76") == "Y";
      switch (str1)
      {
        case "PrimaryResidence":
          this.SetVal("URLA.X108", "Primary");
          break;
        case "SecondHome":
          this.SetVal("URLA.X108", flag ? "FHA Secondary Residence" : "SecondHome");
          break;
        case "Investor":
          this.SetVal("URLA.X108", "Investment");
          break;
        default:
          this.SetVal("URLA.X108", "");
          break;
      }
      if (!(str2 != "FHA"))
        return;
      this.SetVal("URLA.X76", "N");
    }

    private void clearBankruptcyTypes(string id, string val)
    {
      if (id == "265" && this.Val("265") != "Y")
      {
        this.SetVal("URLA.X174", "N");
        this.SetVal("URLA.X175", "N");
        this.SetVal("URLA.X176", "N");
        this.SetVal("URLA.X177", "N");
      }
      if (!(id == "266") || !(this.Val("266") != "Y"))
        return;
      this.SetVal("URLA.X178", "N");
      this.SetVal("URLA.X179", "N");
      this.SetVal("URLA.X180", "N");
      this.SetVal("URLA.X181", "N");
    }

    private void concatenateURLAAddresses(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      if (id == "1825")
      {
        int pairIndex = 0;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        string id1 = this.loan.CurrentBorrowerPair.Id;
        for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index1]);
          for (int index2 = 0; index2 < D1003URLA2020Calculation.addressSets.GetLength(0); ++index2)
            this.copybackToStreetAddressOnToggleToURLA2020(D1003URLA2020Calculation.addressSets[index2, 0], D1003URLA2020Calculation.addressSets[index2, 1], D1003URLA2020Calculation.addressSets[index2, 4], D1003URLA2020Calculation.addressSets[index2, 2], D1003URLA2020Calculation.addressSets[index2, 3]);
          if (id1 == borrowerPairs[index1].Id)
            pairIndex = index1;
        }
        this.loan.SetBorrowerPair(pairIndex);
      }
      else
      {
        bool flag = id.StartsWith("FR") || id.StartsWith("BR") || id.StartsWith("CR") && !id.StartsWith("CRED") || id.StartsWith("FE") || id.StartsWith("BE") || id.StartsWith("CE") || id.StartsWith("FM");
        for (int index = 0; index < D1003URLA2020Calculation.addressSets.GetLength(0); ++index)
        {
          string longAddressFieldID = "";
          string streetAddressFieldID = "";
          string unitTypeFieldID = "";
          string unitNoFieldID = "";
          if (D1003URLA2020Calculation.addressSets[index, 4] == "Y")
          {
            if (!flag)
            {
              longAddressFieldID = D1003URLA2020Calculation.addressSets[index, 0];
              streetAddressFieldID = D1003URLA2020Calculation.addressSets[index, 1];
              unitTypeFieldID = D1003URLA2020Calculation.addressSets[index, 2];
              unitNoFieldID = D1003URLA2020Calculation.addressSets[index, 3];
            }
          }
          else if (flag)
          {
            longAddressFieldID = D1003URLA2020Calculation.getActualFieldId(D1003URLA2020Calculation.addressSets[index, 0], id);
            streetAddressFieldID = D1003URLA2020Calculation.getActualFieldId(D1003URLA2020Calculation.addressSets[index, 1], id);
            unitTypeFieldID = D1003URLA2020Calculation.getActualFieldId(D1003URLA2020Calculation.addressSets[index, 2], id);
            unitNoFieldID = D1003URLA2020Calculation.getActualFieldId(D1003URLA2020Calculation.addressSets[index, 3], id);
          }
          if (!(longAddressFieldID == string.Empty))
          {
            if (id == longAddressFieldID)
            {
              this.copybackToStreetAddress(longAddressFieldID, streetAddressFieldID, unitTypeFieldID, unitNoFieldID);
              break;
            }
            if (id == streetAddressFieldID || id == unitTypeFieldID || id == unitNoFieldID)
            {
              this.concatenateAddress(longAddressFieldID, streetAddressFieldID, unitTypeFieldID, unitNoFieldID);
              break;
            }
          }
        }
      }
    }

    private static string getActualFieldId(string generalID, string id)
    {
      string str = id.Length == 6 ? id.Substring(2, 2) : id.Substring(2, 3);
      return generalID.Substring(0, 2) + str + generalID.Substring(4, 2);
    }

    private void concatenateAddress(
      string longAddressFieldID,
      string streetAddressFieldID,
      string unitTypeFieldID,
      string unitNoFieldID)
    {
      if (longAddressFieldID == null || longAddressFieldID == string.Empty || streetAddressFieldID == null || streetAddressFieldID == string.Empty || unitTypeFieldID == null || unitTypeFieldID == string.Empty || unitNoFieldID == null || unitNoFieldID == string.Empty)
        return;
      string str1 = this.Val(streetAddressFieldID);
      string str2 = this.Val(unitTypeFieldID);
      string str3 = this.Val(unitNoFieldID);
      string str4 = str1;
      string str5 = str4 + (!(str4 != "") || !(str2 != "") ? "" : " ") + str2;
      string val = str5 + (!(str5 != "") || !(str3 != "") ? "" : " ") + str3;
      if (longAddressFieldID == "319")
      {
        string str6 = this.Val(longAddressFieldID);
        string str7 = this.Val("URLA.X188") + str2 + str3;
        if (string.Compare(str6.Replace(" ", ""), str7.Replace(" ", ""), true) == 0)
          return;
        this.SetVal(longAddressFieldID, val);
      }
      else
        this.SetVal(longAddressFieldID, val);
    }

    private void copybackToStreetAddress(
      string longAddressFieldID,
      string streetAddressFieldID,
      string unitTypeFieldID,
      string unitNoFieldID)
    {
      if (longAddressFieldID == null || longAddressFieldID == string.Empty || streetAddressFieldID == null || streetAddressFieldID == string.Empty || unitTypeFieldID == null || unitTypeFieldID == string.Empty || unitNoFieldID == null || unitNoFieldID == string.Empty)
        return;
      if (longAddressFieldID == "319")
      {
        string str1 = this.Val(longAddressFieldID);
        string str2 = this.Val(unitTypeFieldID);
        string str3 = this.Val(unitNoFieldID);
        string str4 = this.Val("URLA.X188") + str2 + str3;
        if (string.Compare(str1.Replace(" ", ""), str4.Replace(" ", ""), true) != 0)
          this.SetVal(streetAddressFieldID, this.Val(longAddressFieldID));
      }
      else
        this.SetVal(streetAddressFieldID, this.Val(longAddressFieldID));
      if (this.USEURLA2020 && longAddressFieldID == "319")
        return;
      this.SetVal(unitTypeFieldID, string.Empty);
      this.SetVal(unitNoFieldID, string.Empty);
    }

    private void copybackToStreetAddressOnToggleToURLA2020(
      string longAddressFieldID,
      string streetAddressFieldID,
      string isActualID,
      string unitTypeFieldID,
      string unitNoFieldID)
    {
      if (longAddressFieldID == null || longAddressFieldID == string.Empty || streetAddressFieldID == null || streetAddressFieldID == string.Empty)
        return;
      if (isActualID == "Y")
      {
        if (longAddressFieldID == "319")
        {
          string str1 = this.Val(unitTypeFieldID);
          string str2 = this.Val(unitNoFieldID);
          if (str1 != string.Empty || str2 != string.Empty)
          {
            string val = this.Val(longAddressFieldID);
            string str3 = this.Val("URLA.X188") + str1 + str2;
            if (string.Compare(val.Replace(" ", ""), str3.Replace(" ", ""), true) == 0)
              return;
            this.SetVal("URLA.X188", val);
            this.SetVal("URLA.X189", "");
            this.SetVal("URLA.X190", "");
          }
          else
            this.SetVal("URLA.X188", this.Val(longAddressFieldID));
        }
        else
          this.SetVal(streetAddressFieldID, this.Val(longAddressFieldID));
      }
      else
      {
        string str4 = longAddressFieldID.Substring(0, 2);
        int num = 0;
        switch (str4)
        {
          case "BR":
            num = this.loan.GetNumberOfResidence(true);
            break;
          case "CR":
            num = this.loan.GetNumberOfResidence(false);
            break;
          case "FR":
            num = 4;
            break;
          case "BE":
            num = this.loan.GetNumberOfEmployer(true);
            break;
          case "CE":
            num = this.loan.GetNumberOfEmployer(false);
            break;
          case "FE":
            num = 6;
            break;
          case "FM":
            num = this.loan.GetNumberOfMortgages();
            break;
        }
        for (int index = 1; index <= num; ++index)
        {
          string str5 = str4 + index.ToString("00");
          string id = str5 + longAddressFieldID.Substring(4, 2);
          this.SetVal(str5 + streetAddressFieldID.Substring(4, 2), this.Val(id));
        }
      }
    }

    internal void MISMOImport(string id, string val)
    {
      this.calObjs.D1003URLA2020Cal.UpdateBorrowerAliasName("URLA.X195", this.Val("URLA.X195"));
      this.calObjs.D1003URLA2020Cal.UpdateBorrowerAliasName("URLA.X196", this.Val("URLA.X196"));
      this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("ULDD.X187", this.Val("ULDD.X187"));
      this.calObjs.D1003URLA2020Cal.ConcatenateNames((string) null, (string) null);
      this.calObjs.VERIFCal.CalcOtherIncome((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcVODTotalDeposits((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcVOOATotalOtherAssets((string) null, (string) null);
      this.calObjs.VERIFCal.CalcOtherLiabilityMonthlyIncome((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalGiftGrants((string) null, (string) null);
      this.calObjs.VERIFCal.CalcAdditionalLoansAmount((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.copyStreetAddressTo2009((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalOtherAssets((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcNonSubjectPropertyDebtsToBePaidOffAmount((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcEstimatedNetMonthlyRent((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier((string) null, (string) null);
      this.calObjs.MLDSCal.CalcMLDSScenarios("675", this.Val("675"));
      this.calObjs.D1003Cal.PopulateOtherFields("675", this.Val("675"));
      this.calObjs.VACal.CalcMaximumSellerContribution((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcBorrowerCount((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcCreditType((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcLockRequestPropertyAddress((string) null, (string) null);
      this.calObjs.Cal.CalcPropertyEstateType((string) null, (string) null);
      if (!this.CalculationObjects.SkipLockRequestSync)
        this.calObjs.D1003Cal.PopulateOtherFields("IMPORT", (string) null);
      this.calObjs.ULDDExpCal.CalculateMISMOImportField();
      this.calObjs.PrequalCal.CalcLockRequestLoan((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTitleInWhichPropertyHeld((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcOccupancyDisplayField((string) null, (string) null);
      this.calObjs.Cal.CalcOthers("14", this.Val("14"));
      this.calObjs.HMDACal.CalcEthnicity("4243", (string) null);
      this.calObjs.HMDACal.CalcEthnicity("4246", (string) null);
      this.calObjs.Cal.PopulateSubjectPropertyAddress((string) null, (string) null);
      this.calObjs.HMDACal.CalcHmdaSyncaddressfields("HMDA.X27", (string) null);
      this.calObjs.HMDACal.CalcRepurchasedReportingYear((string) null, (string) null);
      this.calObjs.HMDACal.CalcHmdaCDRequired((string) null, (string) null);
      this.calObjs.ULDDExpCal.calculateFannieMaeExportFields("1172", (string) null);
      this.calObjs.NewHudCal.CalculateProjectedPaymentTable(true);
      this.calObjs.ToolCal.SetLateDaysEndDefaultValueFromImport(this.calObjs.ExternalLateFeeSettings);
      this.calObjs.D1003URLA2020Cal.CalcRepaymentTypecode((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcNegativeAmortizationIndicator((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalculateRefinanceType((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcPresentHousingExpenseRent((string) null, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcTotalMortgageAndTaxes((string) null, (string) null);
      this.calObjs.VERIFCal.CalcAppliedToDownpayment((string) null, (string) null);
      this.calObjs.VERIFCal.CalSubFin((string) null, (string) null);
      this.calObjs.VERIFCal.CalcPaceLoanAmounts((string) null, (string) null);
      this.calObjs.FHACal.CalcCAWRefi("1134", this.Val("1134"));
      for (int index = 1; index <= this.loan.GetNumberOfMortgages(); ++index)
      {
        string id1 = "FM" + index.ToString("00") + "08";
        this.calObjs.D1003URLA2020Cal.updateCountry(id1, this.Val(id1));
      }
      this.calObjs.D1003URLA2020Cal.updateCountry("1419", this.Val("1419"));
      this.calObjs.D1003URLA2020Cal.updateCountry("1522", this.Val("1522"));
      int index1 = 0;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string id2 = this.loan.CurrentBorrowerPair.Id;
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.calObjs.HMDACal.CalcGender("4193", (string) null);
        this.calObjs.HMDACal.CalcGender("4197", (string) null);
        this.calObjs.D1003Cal.CopyCitizenshipAndAge("1402", this.Val("1402"));
        this.calObjs.D1003Cal.CopyCitizenshipAndAge("1403", this.Val("1403"));
        this.copyCitizenship("URLA.X1", this.Val("URLA.X1"));
        this.copyCitizenship("URLA.X2", this.Val("URLA.X2"));
        this.calObjs.D1003Cal.CopyCitizenshipAndAge("965", this.Val("965"));
        this.calObjs.D1003Cal.CopyCitizenshipAndAge("985", this.Val("985"));
        this.includeVerificationsOnAUSExport("1825", "2020");
        this.calObjs.D1003URLA2020Cal.CopySelfEmployedToBaseIncome("IMPORT", (string) null);
        this.calObjs.D1003URLA2020Cal.CalcAggregateIncome((string) null, (string) null);
        this.calObjs.VERIFCal.CalcMaintenanceExpenseAmount((string) null, (string) null);
        this.calObjs.D1003Cal.CalcOtherIncome("108", this.Val("108"));
        for (int index3 = 1; index3 <= this.loan.GetNumberOfEmployer(true); ++index3)
        {
          string id3 = "BE" + index3.ToString("00") + "07";
          this.calObjs.D1003URLA2020Cal.updateCountry(id3, this.Val(id3));
        }
        for (int index4 = 1; index4 <= this.loan.GetNumberOfEmployer(false); ++index4)
        {
          string id4 = "CE" + index4.ToString("00") + "07";
          this.calObjs.D1003URLA2020Cal.updateCountry(id4, this.Val(id4));
        }
        for (int index5 = 1; index5 <= this.loan.GetNumberOfResidence(true); ++index5)
        {
          string id5 = "BR" + index5.ToString("00") + "08";
          this.calObjs.D1003URLA2020Cal.updateCountry(id5, this.Val(id5));
        }
        for (int index6 = 1; index6 <= this.loan.GetNumberOfResidence(false); ++index6)
        {
          string id6 = "CR" + index6.ToString("00") + "08";
          this.calObjs.D1003URLA2020Cal.updateCountry(id6, this.Val(id6));
        }
        this.calObjs.Cal.UpdateAccountName("URLA.X73", "");
        if (id2 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
      if (borrowerPairs.Length > 1)
      {
        string field = this.loan.GetField("740");
        this.loan.SetField("740", "");
        this.loan.SetField("740", field);
      }
      this.calObjs.D1003URLA2020Cal.CalcAggregateIncome((string) null, (string) null);
      this.calObjs.PrequalCal.CalcLoanComparison((string) null, (string) null);
    }

    private void copyStreetAddressTo2009(string id, string val)
    {
      for (int index1 = 0; index1 < D1003URLA2020Calculation.addressSets.GetLength(0); ++index1)
      {
        string addressSet1 = D1003URLA2020Calculation.addressSets[index1, 1];
        string addressSet2 = D1003URLA2020Calculation.addressSets[index1, 0];
        string addressSet3 = D1003URLA2020Calculation.addressSets[index1, 4];
        if (addressSet1 == null || addressSet1 == string.Empty || addressSet2 == null || addressSet2 == string.Empty)
          break;
        if (addressSet3 == "Y")
        {
          this.concatenateAddress(addressSet2, addressSet1, D1003URLA2020Calculation.addressSets[index1, 2], D1003URLA2020Calculation.addressSets[index1, 3]);
        }
        else
        {
          string str1 = addressSet1.Substring(0, 2);
          int num = 0;
          switch (str1)
          {
            case "BR":
              num = this.loan.GetNumberOfResidence(true);
              break;
            case "CR":
              num = this.loan.GetNumberOfResidence(false);
              break;
            case "FR":
              num = 4;
              break;
            case "BE":
              num = this.loan.GetNumberOfEmployer(true);
              break;
            case "CE":
              num = this.loan.GetNumberOfEmployer(false);
              break;
            case "FE":
              num = 6;
              break;
            case "FM":
              num = this.loan.GetNumberOfMortgages();
              break;
          }
          for (int index2 = 1; index2 <= num; ++index2)
          {
            string str2 = str1 + index2.ToString("00");
            string streetAddressFieldID = str2 + addressSet1.Substring(4, 2);
            string longAddressFieldID = str2 + addressSet2.Substring(4, 2);
            string unitTypeFieldID = str2 + D1003URLA2020Calculation.addressSets[index1, 2].Substring(4, 2);
            string unitNoFieldID = str2 + D1003URLA2020Calculation.addressSets[index1, 3].Substring(4, 2);
            this.concatenateAddress(longAddressFieldID, streetAddressFieldID, unitTypeFieldID, unitNoFieldID);
          }
        }
      }
    }

    private void concatenateNames(string id, string val)
    {
      if (this.Val("1825") == "2020")
      {
        string str1 = this.Val("URLA.X170");
        string str2 = this.Val("URLA.X171");
        string str3 = this.Val("URLA.X172");
        string str4 = this.Val("URLA.X173");
        string str5 = str1;
        string str6 = str5 + (!(str5 != "") || !(str2 != "") ? "" : " ") + str2;
        string str7 = str6 + (!(str6 != "") || !(str3 != "") ? "" : " ") + str3;
        string str8 = str7 + (!(str7 != "") || !(str4 != "") ? "" : " ") + str4;
        if (id == "1825" && !this.IsLocked("1612"))
        {
          if (!(this.Val("1612") != "") || string.Compare(str8, this.Val("1612"), true) == 0)
            return;
          this.AddLock("1612");
        }
        else
          this.SetVal("1612", str8);
      }
      else
      {
        if (!this.IsLocked("1612"))
          return;
        this.RemoveCurrentLock("1612");
      }
    }

    private void includeVerificationsOnAUSExport(string id, string val)
    {
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      if (!(id == "1825") || !(val == "2020"))
        return;
      for (int index = 1; index <= numberOfDeposits; ++index)
      {
        if (this.Val("DD" + index.ToString("00") + "52") == "")
          this.SetVal("DD" + index.ToString("00") + "52", "Y");
      }
      for (int index = 1; index <= numberOfMortgages; ++index)
      {
        if (this.Val("FM" + index.ToString("00") + "52") == "")
          this.SetVal("FM" + index.ToString("00") + "52", "Y");
      }
    }

    private void recalculateVerifications(string id, string val, out double subTotal)
    {
      subTotal = 0.0;
      int numberOfDeposits = this.loan.GetNumberOfDeposits();
      for (int index = 1; index <= numberOfDeposits; ++index)
      {
        string id1 = "DD" + index.ToString("00") + "34";
        if (string.IsNullOrEmpty(this.Val("DD" + index.ToString("00") + "09")))
          this.calObjs.VERIFCal.CalculateDeposits(id1, (string) null);
        else
          this.calObjs.VERIFCal.CalculateDepositsForTogglingURLA2020(id1, (string) null);
        subTotal += this.FltVal(id1);
      }
      this.SetCurrentNum("979", subTotal);
    }

    private void clearConstructionLoanTypes(string id, string val)
    {
      if (this.Val("1825") != "2020" || !(this.Val("URLA.X133") != "Y"))
        return;
      this.SetVal("URLA.X134", string.Empty);
    }

    private void calculateMannerTitleHeld(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      string val1 = this.Val("33");
      string lower = val1.ToLower();
      if (val1 == "")
      {
        this.SetVal("URLA.X138", "");
        this.SetVal("URLA.X139", "");
      }
      else if (D1003URLA2020Calculation.titleHeldMapping.ContainsKey(lower))
      {
        this.SetVal("URLA.X138", D1003URLA2020Calculation.titleHeldMapping[lower]);
        this.SetVal("URLA.X139", "");
      }
      else
      {
        this.SetVal("URLA.X138", "Other");
        this.SetVal("URLA.X139", val1);
      }
    }

    private void calculateMannerTitleHeldOtherDesc(string id, string val)
    {
      if (this.Val("1825") != "2020" || !"URLA.X138".Equals(id) || "Other".Equals(this.Val("URLA.X138")))
        return;
      if (this.IsLocked("URLA.X139"))
        this.RemoveLock("URLA.X139");
      this.SetVal("URLA.X139", "");
    }

    private void calculateTitleInWhichPropertyHeld(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      List<string> stringList = new List<string>();
      foreach (VestingPartyFields vestingPartyField in this.loan.GetVestingPartyFields(true))
      {
        string simpleField1 = this.loan.GetSimpleField(vestingPartyField.TypeField, vestingPartyField.BorrowerPair);
        if (!"Co-signer".Equals(simpleField1) && !"Non Title Spouse".Equals(simpleField1) && !"Officer".Equals(simpleField1) && !"Settlor".Equals(simpleField1))
        {
          string simpleField2 = this.loan.GetSimpleField(vestingPartyField.NameField, vestingPartyField.BorrowerPair);
          stringList.Add(simpleField2);
        }
      }
      int count = stringList.Count;
      string val1 = "";
      for (int index = 0; index < count; ++index)
      {
        val1 += stringList[index];
        if (index < count - 2)
          val1 += ", ";
        else if (index == count - 2)
          val1 = count != 2 ? val1 + ", and " : val1 + " and ";
      }
      this.SetVal("URLA.X136", val1);
    }

    private void calculateBorrowerCount(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      int num = 0;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (!string.IsNullOrEmpty(borrowerPairs[index].Borrower.LastName))
          ++num;
        if (!string.IsNullOrEmpty(borrowerPairs[index].CoBorrower.LastName))
          ++num;
      }
      if (this.IsLocked("URLA.X194"))
        return;
      this.SetVal("URLA.X194", num.ToString());
    }

    private void recalculateVOMOwnedBy(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      int numberOfMortgages = this.loan.GetNumberOfMortgages();
      int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      List<int> intList = new List<int>();
      for (int index1 = 1; index1 <= numberOfMortgages; ++index1)
      {
        string str1 = this.Val("FM" + index1.ToString("00") + "43");
        bool flag = true;
        string val1 = "";
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          string str2 = this.Val("FL" + index2.ToString("00") + "25");
          string str3 = this.Val("FL" + index2.ToString("00") + "08");
          if (str2 == str1 && (str3 == "HELOC" || str3 == "MortgageLoan"))
            intList.Add(index2);
        }
        if (intList.Count > 0)
        {
          val1 = this.Val("FL" + intList[0].ToString("00") + "15");
          foreach (int num in intList)
          {
            if (this.Val("FL" + num.ToString("00") + "15") != val1)
              flag = false;
          }
        }
        if (flag)
          this.SetVal("FM" + index1.ToString("00") + "46", val1);
        intList.Clear();
      }
    }

    private void syncEmploymentStartDate(string id, string val)
    {
      bool useurlA2020 = this.USEURLA2020;
      int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
      {
        if (useurlA2020)
          this.SetVal("BE" + index.ToString("00") + "51", this.Val("BE" + index.ToString("00") + "11"));
      }
      int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
      {
        if (useurlA2020)
          this.SetVal("CE" + index.ToString("00") + "51", this.Val("CE" + index.ToString("00") + "11"));
      }
    }

    private void calculateNonSubjectPropertyDebtsToBePaidOffAmount(string id, string val)
    {
      int index1 = 0;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string id1 = this.loan.CurrentBorrowerPair.Id;
      double num = 0.0;
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int exlcudingAlimonyJobExp = this.loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index3 = 1; index3 <= exlcudingAlimonyJobExp; ++index3)
        {
          string str1 = this.Val("FL" + index3.ToString("00") + "18");
          string str2 = this.Val("FL" + index3.ToString("00") + "08");
          string str3 = this.Val("FL" + index3.ToString("00") + "27");
          string str4 = this.Val("FL" + index3.ToString("00") + "63");
          if ((!(str2 == "HELOC") && !(str2 == "MortgageLoan") || !(str3 == "Y")) && str1 == "Y" && str4 == "Y" && this.Val("FL" + index3.ToString("00") + "17") != "Y")
            num += this.FltVal("FL" + index3.ToString("00") + "16");
        }
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
      this.SetCurrentNum("URLA.X145", num);
    }

    private void calculateEstimatedClosingCostsAmount(string id, string val)
    {
      this.SetCurrentNum("URLA.X146", this.FltVal("137") + this.FltVal("138") + this.FltVal("969"));
    }

    private void calculateTotalNewMortgageLoans(string id, string val)
    {
      double num = this.FltVal("URLA.X230");
      this.SetCurrentNum("URLA.X148", !(this.Val("1172") == "HELOC") ? num + this.FltVal("2") : num + this.FltVal("1888"));
    }

    private void calculateOtherCredits(string id, string val)
    {
      double num = this.FltVal("1852") + this.FltVal("URLA.X151") + this.FltVal("141") + this.FltVal("1095") + this.FltVal("1115") + this.FltVal("1647") + this.FltVal("1851");
      if (this.Val("4796") == "Y")
        num += this.FltVal("4794") + this.FltVal("4795") + this.FltVal("1134") + this.FltVal("L128");
      this.SetCurrentNum("URLA.X149", num);
    }

    private void calculateTotalGiftGrants(string id, string val)
    {
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double num = 0.0;
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int ofGiftsAndGrants = this.loan.GetNumberOfGiftsAndGrants();
        for (int index3 = 1; index3 <= ofGiftsAndGrants; ++index3)
          num += this.FltVal("URLARGG" + index3.ToString("00") + "21");
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
      this.SetCurrentNum("URLA.X150", num);
    }

    private void calculateTotalOtherAssets(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      double num1 = 0.0;
      double num2 = 0.0;
      List<string> stringList = new List<string>()
      {
        "EmployerAssistedHousing",
        "LotEquity",
        "RelocationFunds",
        "LeasePurchaseCredit",
        "SweatEquity",
        "TradeEquityFromPropertySwap"
      };
      string str1 = this.Val("4796");
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        int numberOfOtherAssets = this.loan.GetNumberOfOtherAssets();
        for (int index3 = 1; index3 <= numberOfOtherAssets; ++index3)
        {
          string str2 = this.Val("URLAROA" + index3.ToString("00") + "02");
          double num3 = this.FltVal("URLAROA" + index3.ToString("00") + "03");
          if (str2 == "EarnestMoney")
          {
            if (str1 == "Y")
              num2 += num3;
            else
              num1 += num3;
          }
          else if (stringList.Contains(str2))
            num1 += num3;
        }
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
      this.SetCurrentNum("URLA.X151", num1);
      this.SetCurrentNum("L128", num2);
      if (id == null || !id.EndsWith("02") || !id.StartsWith("URLAROA") || !(val != "Other"))
        return;
      this.SetVal(id.Substring(0, id.Length - 2) + "04", string.Empty);
    }

    private void calculateOtherLiabilitiesType(string id, string val)
    {
      if (!id.EndsWith("02") || !id.StartsWith("URLAROL") || !(val != "Other"))
        return;
      this.SetVal(id.Substring(0, id.Length - 2) + "04", string.Empty);
    }

    private void calculateTotalCredits(string id, string val)
    {
      this.SetCurrentNum("URLA.X152", this.FltVal("143") + this.FltVal("URLA.X149"));
    }

    private void calculateCreditType(string id, string val)
    {
      string str = (string) null;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (borrowerPairs.Length > 1)
      {
        str = this.loan.CurrentBorrowerPair.Id;
        this.loan.SetBorrowerPair(borrowerPairs[0]);
      }
      int num = this.IntVal("URLA.X194");
      this.SetVal("URLA.X234", num > 1 ? "Jointly" : (num == 0 ? "" : "NotJointly"));
      if (str == null)
        return;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Id == str)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index]);
          break;
        }
      }
    }

    private void updateCountry(string id, string val)
    {
      string fieldIdPrefix = "";
      bool isForeignIndicatorSelected = this.loan.IsForeignIndicatorSelected(id);
      if (id.Length >= 6 && (id.StartsWith("FR") || id.StartsWith("BR") || id.StartsWith("CR") && !id.StartsWith("CRED")) && id.EndsWith("08"))
        this.validatezipcode(val, id.Substring(0, id.Length == 6 ? 4 : 5) + "28", id.Substring(0, id.Length == 6 ? 4 : 5) + "30", isForeignIndicatorSelected, fieldIdPrefix);
      else if (id.Length >= 6 && (id.StartsWith("BR") || id.StartsWith("CR") && !id.StartsWith("CRED")) && id.EndsWith("11"))
      {
        this.validatezipcode(val, (string) null, id.Substring(0, id.Length == 6 ? 4 : 5) + "40", isForeignIndicatorSelected, fieldIdPrefix);
      }
      else
      {
        string key;
        if (id.StartsWith("FE05") || id.StartsWith("FE06"))
          key = id;
        else if (id.StartsWith("FE") || id.StartsWith("BE") || id.StartsWith("CE") || id.StartsWith("FM") || id.StartsWith("DD") || id.StartsWith("FL"))
        {
          key = id.Substring(0, 2);
          fieldIdPrefix = key + id.Substring(2, 2);
        }
        else if (id.StartsWith("URLAROL"))
        {
          key = id.Substring(0, 7);
          fieldIdPrefix = key + id.Substring(7, 2);
        }
        else if (id.StartsWith("NBOC"))
        {
          key = id.Substring(0, 4);
          fieldIdPrefix = key + id.Substring(4, 2);
        }
        else
        {
          if (!this.loan.foreignAddressIndictorLookupTable.ContainsKey(id))
            return;
          key = this.loan.foreignAddressIndictorLookupTable[id];
        }
        this.validatezipcode(val, this.foreignAddressFieldsDict[key][5], this.foreignAddressFieldsDict[key][6], isForeignIndicatorSelected, fieldIdPrefix);
      }
    }

    private void validatezipcode(
      string zipcode,
      string countryfieldId,
      string country,
      bool isForeignIndicatorSelected,
      string fieldIdPrefix)
    {
      if (isForeignIndicatorSelected)
        return;
      ZipCodeInfo zipInfoAt = ZipCodeUtils.GetZipInfoAt(zipcode);
      if ((zipcode == string.Empty || zipInfoAt == null) && countryfieldId != null)
      {
        if (!string.IsNullOrEmpty(countryfieldId))
          this.SetVal(fieldIdPrefix + countryfieldId, string.Empty);
        if (string.IsNullOrEmpty(country))
          return;
        this.SetVal(fieldIdPrefix + country, string.Empty);
      }
      else
      {
        if (!string.IsNullOrEmpty(countryfieldId))
          this.SetVal(fieldIdPrefix + countryfieldId, "US");
        if (string.IsNullOrEmpty(country))
          return;
        this.SetVal(fieldIdPrefix + country, "US");
      }
    }

    private void calculateLandOtherType(string id, string val)
    {
      if (!(this.Val("URLA.X141") != "Other"))
        return;
      this.SetVal("URLA.X142", "");
    }

    private void calculateRent(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      string id1 = id.Replace("15", "16");
      if (!(this.Val(id) != "Rent"))
        return;
      this.SetVal(id1, "");
    }

    private void calculateAggregateIncome(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      if (id == null)
      {
        this.calculateAllEmployerIncome();
      }
      else
      {
        int num1 = Utils.ParseInt((object) id.Substring(2, 2), 0);
        string str1 = id.Substring(0, 2);
        if (!(str1 == "BE") && !(str1 == "CE") && !(str1 == "FE"))
          return;
        int num2;
        switch (str1)
        {
          case "BE":
            num2 = 1;
            break;
          case "FE":
            num2 = num1 % 2 != 0 ? 1 : 0;
            break;
          default:
            num2 = 0;
            break;
        }
        bool borrower = num2 != 0;
        string s = id.Substring(id.Length - 2);
        // ISSUE: reference to a compiler-generated method
        switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(s))
        {
          case 350953279:
            if (!(s == "19"))
              return;
            this.SetNum(borrower ? "101" : "110", this.employerIncomeTotal(borrower, D1003URLA2020Calculation.Employer.BasePay));
            return;
          case 418063755:
            if (!(s == "15"))
              return;
            break;
          case 418210850:
            if (!(s == "09"))
              return;
            int numberOfEmployer = this.loan.GetNumberOfEmployer(borrower);
            string str2 = borrower ? "BE" : "CE";
            double num3 = 0.0;
            double num4 = 0.0;
            double num5 = 0.0;
            double num6 = 0.0;
            double num7 = 0.0;
            for (int index = 1; index <= numberOfEmployer; ++index)
            {
              string str3 = str2 + index.ToString("00");
              if (!(this.Val(str3 + "09") != "Y"))
              {
                num3 += this.FltVal(str3 + "19");
                num4 += this.FltVal(str3 + "20");
                num5 += this.FltVal(str3 + "21");
                num6 += this.FltVal(str3 + "22");
                num7 += this.FltVal(str3 + "23");
              }
            }
            this.SetNum(borrower ? "101" : "110", num3);
            this.SetNum(borrower ? "102" : "111", num4);
            this.SetNum(borrower ? "103" : "112", num5);
            this.SetNum(borrower ? "104" : "113", num6);
            this.SetNum(borrower ? "107" : "116", num7);
            return;
          case 2313243154:
            if (!(s == "56"))
              return;
            break;
          case 2364708844:
            if (!(s == "21"))
              return;
            this.SetNum(borrower ? "103" : "112", this.employerIncomeTotal(borrower, D1003URLA2020Calculation.Employer.Bonus));
            return;
          case 2381486463:
            if (!(s == "20"))
              return;
            this.SetNum(borrower ? "102" : "111", this.employerIncomeTotal(borrower, D1003URLA2020Calculation.Employer.Overtime));
            return;
          case 2398264082:
            if (!(s == "23"))
              return;
            this.SetNum(borrower ? "107" : "116", this.employerIncomeTotal(borrower, D1003URLA2020Calculation.Employer.Other));
            return;
          case 2415041701:
            if (!(s == "22"))
              return;
            this.SetNum(borrower ? "104" : "113", this.employerIncomeTotal(borrower, D1003URLA2020Calculation.Employer.Commissions));
            return;
          default:
            return;
        }
        this.SetNum(borrower ? "101" : "110", this.employerIncomeTotal(borrower, D1003URLA2020Calculation.Employer.BasePay));
      }
    }

    private double employerIncomeTotal(bool borrower, D1003URLA2020Calculation.Employer e)
    {
      int numberOfEmployer = this.loan.GetNumberOfEmployer(borrower);
      double num = 0.0;
      string str1 = borrower ? "BE" : "CE";
      string str2 = ((int) e).ToString();
      for (int index = 1; index <= numberOfEmployer; ++index)
      {
        string str3 = str1 + index.ToString("00");
        if (this.Val(str3 + "09") == "Y")
          num += this.FltVal(str3 + str2);
      }
      return num;
    }

    private void calculateAllEmployerIncome()
    {
      this.SetNum("101", this.employerIncomeTotal(true, D1003URLA2020Calculation.Employer.BasePay));
      this.SetNum("110", this.employerIncomeTotal(false, D1003URLA2020Calculation.Employer.BasePay));
      this.SetNum("102", this.employerIncomeTotal(true, D1003URLA2020Calculation.Employer.Overtime));
      this.SetNum("111", this.employerIncomeTotal(false, D1003URLA2020Calculation.Employer.Overtime));
      this.SetNum("103", this.employerIncomeTotal(true, D1003URLA2020Calculation.Employer.Bonus));
      this.SetNum("112", this.employerIncomeTotal(false, D1003URLA2020Calculation.Employer.Bonus));
      this.SetNum("104", this.employerIncomeTotal(true, D1003URLA2020Calculation.Employer.Commissions));
      this.SetNum("113", this.employerIncomeTotal(false, D1003URLA2020Calculation.Employer.Commissions));
      this.SetNum("107", this.employerIncomeTotal(true, D1003URLA2020Calculation.Employer.Other));
      this.SetNum("116", this.employerIncomeTotal(false, D1003URLA2020Calculation.Employer.Other));
    }

    private void calculateEstimatedNetMonthlyRent(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      string str = this.Val("19");
      string propertyType = this.Val("1811", this.GetBorrowerPairs()[0]);
      int num = this.IntVal("16");
      if (str == "Purchase")
      {
        switch (propertyType)
        {
          case "Investor":
            this.SetCurrentNum("URLA.X81", Utils.ArithmeticRounding(this.calObjs.D1003Cal.FindCashflowAmount(propertyType), 2));
            return;
          case "PrimaryResidence":
            if (num <= 1 || num >= 5)
              break;
            goto case "Investor";
        }
      }
      this.SetCurrentNum("URLA.X81", 0.0);
    }

    private void calculateGovtRefTypeOtherDesc(string id, string val)
    {
      if (!(this.Val("URLA.X166") != "Other"))
        return;
      this.SetVal("URLA.X167", string.Empty);
    }

    private void calculateLoanRepaymentType(string id, string val)
    {
      switch (id)
      {
        case "CD4.X2":
          string str1 = this.Val("CD4.X2");
          this.SetVal("URLA.X239", !(str1 != "none") || !(str1 != "") ? "N" : "Y");
          switch (str1)
          {
            case "scheduled":
              this.SetVal("424", "ScheduledNegativeAmortization");
              return;
            case "potential":
              this.SetVal("424", "PossibleNegativeAmortization");
              return;
            default:
              this.SetVal("424", this.USEURLA2020 || str1 == "" ? string.Empty : "NoNegativeAmortization");
              return;
          }
        case "URLA.X239":
          if (this.Val("URLA.X239") != "Y")
          {
            this.SetVal("424", string.Empty);
            this.SetVal("CD4.X2", "none");
            break;
          }
          if (!(this.Val("424") == "") && this.Val("424").Length >= 28)
            break;
          this.SetVal("424", "PossibleNegativeAmortization");
          break;
        case "1825":
          string str2 = this.Val("CD4.X2");
          string str3 = this.Val("424");
          if (this.USEURLA2020)
          {
            if (str3 != "PossibleNegativeAmortization" && str3 != "ScheduledNegativeAmortization")
            {
              this.SetVal("424", "");
              this.SetVal("URLA.X239", "");
              if (!(str2 != "none"))
                break;
              this.SetVal("CD4.X2", "");
              break;
            }
            if (!(this.Val("URLA.X239") != "Y"))
              break;
            this.SetVal("URLA.X239", "Y");
            break;
          }
          if (!(str2 == "none") || !(str3 != "NoNegativeAmortization"))
            break;
          this.SetVal("424", "NoNegativeAmortization");
          break;
      }
    }

    private void createLinkedVoal(string id, string val)
    {
      if (!this.USEURLA2020 || this.loan.LinkedData == null || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary && this.loan.LinkedData.LinkSyncType == LinkSyncType.ConstructionLinked || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked && this.loan.LinkedData.LinkSyncType == LinkSyncType.ConstructionPrimary)
        return;
      LoanData primary = (LoanData) null;
      if (this.loan.LinkSyncType == LinkSyncType.None || this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary)
        primary = this.loan;
      else if (this.loan.LinkSyncType == LinkSyncType.PiggybackLinked || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked)
        primary = this.loan.LinkedData;
      BorrowerPair currentBorrowerPair1 = primary.CurrentBorrowerPair;
      primary.SetBorrowerPair(0);
      BorrowerPair currentBorrowerPair2 = primary.LinkedData.CurrentBorrowerPair;
      primary.LinkedData.SetBorrowerPair(0);
      int index = primary.GetNumberOfAdditionalLoans() + 1;
      int num = primary.LinkedData.GetNumberOfAdditionalLoans() + 1;
      string str = "URLARAL" + index.ToString("00");
      this.createUpdateVoalRecords(primary, true, index);
      this.createUpdateVoalRecords(primary, false, id == "LINKEXISTING" ? num : 1);
      primary.SetBorrowerPair(currentBorrowerPair1);
      primary.LinkedData.SetBorrowerPair(currentBorrowerPair2);
    }

    private void createUpdateVoalRecords(LoanData primary, bool isUpdatePrimaryLoan, int index)
    {
      if (primary == null || primary.LinkedData == null)
        return;
      string str = "URLARAL" + index.ToString("00");
      LoanData loanData1;
      LoanData loanData2;
      if (isUpdatePrimaryLoan)
      {
        loanData1 = primary;
        loanData2 = primary.LinkedData;
      }
      else
      {
        loanData1 = primary.LinkedData;
        loanData2 = primary;
      }
      bool flag = loanData2.GetField("1172") == "HELOC";
      loanData1.SetField(str + "01", "Both");
      loanData1.SetField(str + "20", loanData2.GetField("2"));
      if (!flag)
        loanData1.SetField(str + "22", loanData2.GetField("1109"));
      else
        loanData1.SetField(str + "22", loanData2.GetField("4493"));
      string field = loanData2.GetField("4494");
      if ((string.IsNullOrEmpty(field) ? 0 : Utils.ParseInt((object) field)) > 1)
        loanData1.SetField(str + "18", loanData2.GetField("229"));
      else
        loanData1.SetField(str + "18", loanData2.GetField("228"));
      if (flag)
        loanData1.SetField(str + "34", loanData2.GetField("5025"));
      loanData1.SetField(str + "25", "Y");
      loanData1.SetField(str + "19", loanData2.GetField("QM.X337"));
      loanData1.SetField(str + "16", flag ? "HELOC" : "Mortgage");
      loanData1.SetField(str + "21", loanData2.GetField("1888"));
      loanData1.SetField(str + "17", field);
      loanData1.SetField(str + "02", loanData2.GetField("1264"));
      loanData1.SetField(str + "23", loanData2.GetField("URLA.X209"));
      loanData1.SetField(str + "24", loanData2.GetField("URLA.X210"));
    }

    private void updateLinkedVoal(string id, string val)
    {
      if (!this.USEURLA2020 || this.loan.LinkedData == null || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary && this.loan.LinkedData.LinkSyncType == LinkSyncType.ConstructionLinked || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked && this.loan.LinkedData.LinkSyncType == LinkSyncType.ConstructionPrimary)
        return;
      LoanData primary = (LoanData) null;
      LoanData loanData = (LoanData) null;
      bool flag = false;
      if (this.loan.LinkSyncType == LinkSyncType.None || this.loan.LinkSyncType == LinkSyncType.PiggybackPrimary || this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary)
      {
        primary = this.loan;
        loanData = this.loan.LinkedData;
      }
      else if (this.loan.LinkSyncType == LinkSyncType.PiggybackLinked || this.loan.LinkSyncType == LinkSyncType.ConstructionLinked)
      {
        primary = this.loan.LinkedData;
        loanData = this.loan;
        flag = true;
      }
      BorrowerPair currentBorrowerPair1 = primary.CurrentBorrowerPair;
      primary.SetBorrowerPair(0);
      BorrowerPair currentBorrowerPair2 = loanData.CurrentBorrowerPair;
      loanData.SetBorrowerPair(0);
      int ofAdditionalLoans = primary.GetNumberOfAdditionalLoans();
      string str = "URLARAL";
      for (int index1 = 1; index1 <= ofAdditionalLoans; ++index1)
      {
        if (primary.GetField(str + index1.ToString("00") + "25") == "Y")
        {
          int index2 = index1;
          if (flag)
          {
            index2 = 0;
            for (int index3 = 1; index3 <= loanData.GetNumberOfAdditionalLoans(); ++index3)
            {
              if (loanData.GetField(str + index3.ToString("00") + "25") == "Y")
              {
                index2 = index3;
                break;
              }
            }
          }
          if (index2 > 0)
          {
            this.createUpdateVoalRecords(primary, !flag, index2);
            break;
          }
          break;
        }
      }
      primary.SetBorrowerPair(currentBorrowerPair1);
      loanData.SetBorrowerPair(currentBorrowerPair2);
    }

    private void removelinkedVoal(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      if (this.Val("420") == "FirstLien")
      {
        this.removeVoal(this.loan);
        if (this.loan == null || this.loan.LinkedData == null)
          return;
        this.removeVoal(this.loan.LinkedData);
      }
      else
      {
        if (this.loan != null && this.loan.LinkedData != null)
          this.removeVoal(this.loan.LinkedData);
        this.removeVoal(this.loan);
      }
    }

    private void removeVoal(LoanData loandata)
    {
      if (loandata == null)
        return;
      BorrowerPair currentBorrowerPair = loandata.CurrentBorrowerPair;
      loandata.SetBorrowerPair(0);
      for (int ofAdditionalLoans = loandata.GetNumberOfAdditionalLoans(); ofAdditionalLoans > 0; --ofAdditionalLoans)
      {
        if (loandata.GetField("URLARAL" + ofAdditionalLoans.ToString("00") + "25") == "Y")
          loandata.RemoveAdditionalLoanAt(ofAdditionalLoans - 1);
      }
      loandata.SetBorrowerPair(currentBorrowerPair);
    }

    private void calculateRefinanceType(string id, string val)
    {
      string str = this.Val("19");
      if (!(str == "Purchase") && !(str == "Other") && !string.IsNullOrEmpty(str))
        return;
      this.SetVal("URLA.X165", string.Empty);
    }

    private void calculateAll(string id, string val) => this.clearConstructionLoanTypes(id, val);

    private void calculateVOEDoesNotApply(string id, string val)
    {
      if (!id.StartsWith("FE") && (!id.StartsWith("BE") && !id.StartsWith("CE") || !id.EndsWith("09")))
        return;
      bool borrower = id.StartsWith("FE") ? id.Substring(2, 2) == "01" || id.Substring(2, 2) == "03" || id.Substring(2, 2) == "05" : id.StartsWith("BE");
      string str1 = borrower ? "BE" : "CE";
      int numberOfEmployer = this.loan.GetNumberOfEmployer(borrower);
      int num = 0;
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      bool flag6 = false;
      for (int index = 1; index <= numberOfEmployer; ++index)
      {
        string str2 = this.Val(str1 + index.ToString("00") + "08");
        switch (this.Val(str1 + index.ToString("00") + "09"))
        {
          case "N":
            if (str2 == "Borrower" && !flag1)
            {
              this.SetVal("URLA.X203", "N");
              flag1 = true;
              break;
            }
            if (str2 == "CoBorrower" && !flag2)
            {
              this.SetVal("URLA.X204", "N");
              flag2 = true;
              break;
            }
            break;
          case "Y":
            ++num;
            if (num == 1)
            {
              if (str2 == "Borrower" && !flag3)
              {
                this.SetVal("URLA.X199", "N");
                flag3 = true;
                break;
              }
              if (str2 == "CoBorrower" && !flag4)
              {
                this.SetVal("URLA.X200", "N");
                flag4 = true;
                break;
              }
              break;
            }
            if (num > 1)
            {
              if (str2 == "Borrower" && !flag5)
              {
                this.SetVal("URLA.X201", "N");
                flag5 = true;
                break;
              }
              if (str2 == "CoBorrower" && !flag6)
              {
                this.SetVal("URLA.X202", "N");
                flag6 = true;
                break;
              }
              break;
            }
            break;
        }
      }
    }

    private void calculateVOOIDoesNotApply(string id, string val)
    {
      this.calculateDoNotApply(this.loan.GetNumberOfOtherIncomeSources(), "URLAROIS", "02", "URLA.X40", "URLA.X41");
    }

    private void copyATRQMIncome(string id, string val)
    {
      Dictionary<string, double> incomeList1 = new Dictionary<string, double>()
      {
        {
          "QM.X137",
          0.0
        },
        {
          "QM.X138",
          0.0
        },
        {
          "QM.X139",
          0.0
        },
        {
          "QM.X140",
          0.0
        },
        {
          "QM.X142",
          0.0
        },
        {
          "QM.X143",
          0.0
        },
        {
          "QM.X281",
          0.0
        },
        {
          "QM.X282",
          0.0
        },
        {
          "QM.X283",
          0.0
        },
        {
          "QM.X284",
          0.0
        },
        {
          "QM.X285",
          0.0
        },
        {
          "QM.X286",
          0.0
        },
        {
          "QM.X295",
          0.0
        },
        {
          "QM.X296",
          0.0
        },
        {
          "QM.X297",
          0.0
        },
        {
          "QM.X298",
          0.0
        },
        {
          "QM.X299",
          0.0
        },
        {
          "QM.X307",
          0.0
        }
      };
      Dictionary<string, double> incomeList2 = new Dictionary<string, double>()
      {
        {
          "QM.X145",
          0.0
        },
        {
          "QM.X146",
          0.0
        },
        {
          "QM.X147",
          0.0
        },
        {
          "QM.X148",
          0.0
        },
        {
          "QM.X149",
          0.0
        },
        {
          "QM.X150",
          0.0
        },
        {
          "QM.X151",
          0.0
        },
        {
          "QM.X288",
          0.0
        },
        {
          "QM.X289",
          0.0
        },
        {
          "QM.X290",
          0.0
        },
        {
          "QM.X291",
          0.0
        },
        {
          "QM.X292",
          0.0
        },
        {
          "QM.X293",
          0.0
        },
        {
          "QM.X301",
          0.0
        },
        {
          "QM.X302",
          0.0
        },
        {
          "QM.X303",
          0.0
        },
        {
          "QM.X304",
          0.0
        },
        {
          "QM.X305",
          0.0
        }
      };
      string empty = string.Empty;
      this.copyIncome(incomeList1, true, ref empty);
      this.copyIncome(incomeList2, false, ref empty);
      if (empty.Length <= 0)
        return;
      this.SetVal("QM.X307", empty.Substring(0, empty.Length - 1));
    }

    private void copyIncome(
      Dictionary<string, double> incomeList,
      bool isborrower,
      ref string f307desc)
    {
      int numberOfEmployer = this.loan.GetNumberOfEmployer(isborrower);
      string str1 = isborrower ? "BE" : "CE";
      for (int index = 1; index <= numberOfEmployer; ++index)
      {
        string str2 = str1 + index.ToString("00");
        if (this.Val(str2 + "09") == "Y")
        {
          string str3 = this.Val(str2 + "63");
          if (isborrower)
          {
            if (str3 != "Y")
            {
              incomeList["QM.X137"] += this.FltVal(str2 + "19");
              incomeList["QM.X142"] += this.FltVal(str2 + "23");
            }
            else
            {
              incomeList["QM.X281"] += this.FltVal(str2 + "19");
              incomeList["QM.X299"] += this.FltVal(str2 + "23");
            }
            incomeList["QM.X138"] += this.FltVal(str2 + "20");
            incomeList["QM.X139"] += this.FltVal(str2 + "21");
            incomeList["QM.X140"] += this.FltVal(str2 + "22");
            incomeList["QM.X282"] += this.FltVal(str2 + "77");
            incomeList["QM.X283"] += this.FltVal(str2 + "65");
            incomeList["QM.X284"] += this.FltVal(str2 + "66");
            incomeList["QM.X285"] += this.FltVal(str2 + "67");
            incomeList["QM.X286"] += this.FltVal(str2 + "68");
            incomeList["QM.X295"] += this.FltVal(str2 + "69");
            incomeList["QM.X296"] += this.FltVal(str2 + "70");
            incomeList["QM.X297"] += this.FltVal(str2 + "71");
            incomeList["QM.X298"] += this.FltVal(str2 + "72");
          }
          else
          {
            if (str3 != "Y")
            {
              incomeList["QM.X145"] += this.FltVal(str2 + "19");
              incomeList["QM.X150"] += this.FltVal(str2 + "23");
            }
            else
            {
              incomeList["QM.X288"] += this.FltVal(str2 + "19");
              incomeList["QM.X305"] += this.FltVal(str2 + "23");
            }
            incomeList["QM.X146"] += this.FltVal(str2 + "20");
            incomeList["QM.X147"] += this.FltVal(str2 + "21");
            incomeList["QM.X148"] += this.FltVal(str2 + "22");
            incomeList["QM.X289"] += this.FltVal(str2 + "77");
            incomeList["QM.X290"] += this.FltVal(str2 + "65");
            incomeList["QM.X291"] += this.FltVal(str2 + "66");
            incomeList["QM.X292"] += this.FltVal(str2 + "67");
            incomeList["QM.X293"] += this.FltVal(str2 + "68");
            incomeList["QM.X301"] += this.FltVal(str2 + "69");
            incomeList["QM.X302"] += this.FltVal(str2 + "70");
            incomeList["QM.X303"] += this.FltVal(str2 + "71");
            incomeList["QM.X304"] += this.FltVal(str2 + "72");
          }
          string str4 = this.Val(str2 + "74");
          if (!string.IsNullOrEmpty(str4) && str3 == "Y")
            f307desc = f307desc + str4 + ";";
        }
      }
      foreach (KeyValuePair<string, double> income in incomeList)
        this.SetCurrentNum(income.Key, income.Value);
      if (isborrower)
        this.SetCurrentNum("QM.X143", this.FltVal("URLA.X42"));
      else
        this.SetCurrentNum("QM.X151", this.FltVal("URLA.X43"));
    }

    private void calculateGiftGrantSourceOtherDesc(string id, string val)
    {
      if (this.Val("1825") != "2020" || id == null || !id.EndsWith("19") || !id.StartsWith("URLARGG") || !(val != "Other"))
        return;
      this.SetVal(id.Substring(0, id.Length - 2) + "22", string.Empty);
    }

    private void calculateConstructionLoanIndicator(string id, string val)
    {
      if (this.Val("1825") != "2020" || !(this.Val("19") == "ConstructionToPermanent"))
        return;
      this.SetVal("URLA.X133", "Y");
    }

    private void calculateRepaymentTypecode(string id, string val)
    {
      string str = this.Val("424");
      switch (str)
      {
        case "PossibleNegativeAmortization":
          this.SetVal("CD4.X2", "potential");
          break;
        case "ScheduledNegativeAmortization":
          this.SetVal("CD4.X2", "scheduled");
          break;
        default:
          if (str == "NoNegativeAmortization" || str == "" && this.Val("URLA.X239") == "Y")
          {
            this.SetVal("CD4.X2", "none");
            break;
          }
          if (!(str == "") && !(str == "ScheduledAmortization") && !(str == "InterestOnly") && !(this.Val("URLA.X239") != "Y"))
            break;
          if (id == "1825" && this.USEURLA2020)
          {
            if (!(str == "") || !(this.Val("CD4.X2") != "none"))
              break;
            this.SetVal("CD4.X2", "");
            break;
          }
          if ((id == "424" || id == "URLA.X239") && str == "" && this.USEURLA2020)
          {
            this.SetVal("CD4.X2", "none");
            break;
          }
          this.SetVal("CD4.X2", "");
          break;
      }
    }

    private void calculateBackEndRatio(string id, string val)
    {
      if (this.Val("1825") != "2020")
        return;
      int ofOtherLiability = this.loan.GetNumberOfOtherLiability();
      double num1 = 0.0;
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 1; index <= ofOtherLiability; ++index)
      {
        string str = "URLAROL" + index.ToString("00");
        switch (this.Val(str + "02"))
        {
          case "ChildSupport":
          case "Alimony":
            num1 += this.FltVal(str + "03");
            break;
          case "JobRelatedExpenses":
            num2 += this.FltVal(str + "03");
            break;
          default:
            num3 += this.FltVal(str + "03");
            break;
        }
      }
      this.SetCurrentNum("272", num1);
      this.SetCurrentNum("256", num2);
      this.SetCurrentNum("1062", num3);
    }

    private void calculateNegativeAmortizationIndicator(string id, string val)
    {
      string str = this.Val("424");
      if (str == "PossibleNegativeAmortization" || str == "ScheduledNegativeAmortization")
      {
        this.SetVal("URLA.X239", "Y");
      }
      else
      {
        if (!(id == "424") || !(str == ""))
          return;
        this.SetVal("URLA.X239", "N");
        this.SetVal("CD4.X2", "none");
      }
    }

    private void calculatePropertyLeaseholdExpirationDate(string id, string val)
    {
      if (!(this.Val("1066") != "Leasehold"))
        return;
      this.SetVal("1034", "");
    }

    private void copySelfEmployedToBaseIncome(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      string verifBlock = this.GetVerifBlock(id);
      if (id.EndsWith("15") && this.Val(verifBlock + "15") == "N")
      {
        if (verifBlock == "FE05" || verifBlock == "FE06")
        {
          bool borrower = verifBlock.Substring(2) == "05";
          int numberOfEmployer = this.loan.GetNumberOfEmployer(borrower);
          string str = borrower ? "BE" : "CE";
          for (int index = 1; index <= numberOfEmployer; ++index)
          {
            if (this.Val(str + index.ToString("00") + "09") == "N")
            {
              if (this.Val(str + index.ToString("00") + "56") != "")
                this.SetVal(str + index.ToString("00") + "19", string.Empty);
              this.SetVal(str + index.ToString("00") + "56", string.Empty);
              break;
            }
          }
        }
        else
        {
          if (this.Val(verifBlock + "56") != "")
            this.SetVal(verifBlock + "19", string.Empty);
          this.SetVal(verifBlock + "56", string.Empty);
        }
      }
      if (id.EndsWith("56"))
      {
        this.SetVal(verifBlock + "19", this.Val(verifBlock + "56"));
        if (this.Val(verifBlock + "15") == "Y" && this.FltVal(verifBlock + "56") != 0.0)
        {
          for (int index = 65; index <= 72; ++index)
            this.SetVal(verifBlock + (object) index, "");
          this.SetVal(verifBlock + "77", "");
          this.SetVal(verifBlock + "53", "");
        }
      }
      if (!(id == "IMPORT"))
        return;
      int numberOfEmployer1 = this.loan.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
      {
        string val1 = this.Val("BE" + index.ToString("00") + "56");
        if (this.Val("BE" + index.ToString("00") + "15") == "Y" && val1 != "")
          this.SetVal("BE" + index.ToString("00") + "19", val1);
      }
      int numberOfEmployer2 = this.loan.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
      {
        string val2 = this.Val("CE" + index.ToString("00") + "56");
        if (this.Val("CE" + index.ToString("00") + "15") == "Y" && val2 != "")
          this.SetVal("CE" + index.ToString("00") + "19", val2);
      }
    }

    private void calculateArmProg(string id, string val)
    {
      if (!(this.Val("608") != "AdjustableRate") || !(this.Val("1172") != "HELOC"))
        return;
      this.SetVal("995", "");
    }

    private void calculateProductDescription(string id, string val)
    {
      if (!this.USEURLA2020 || !(this.Val("URLA.X242") != "Y"))
        return;
      this.SetVal("URLA.X143", "");
    }

    private void calculateHomeOwnershipSubData(string id, string val)
    {
      if (!this.USEURLA2020 && !this.loan.IsTemplate)
        return;
      if (id == "URLA.X153" && val != "Y")
      {
        this.SetVal("URLA.X154", "");
        this.SetVal("URLA.X155", "");
        this.SetVal("URLA.X232", "");
        this.SetVal("URLA.X233", "");
      }
      else
      {
        if (!(id == "URLA.X159") || !(val != "Y"))
          return;
        this.SetVal("URLA.X160", "");
        this.SetVal("URLA.X161", "");
        this.SetVal("URLA.X244", "");
        this.SetVal("URLA.X243", "");
        this.SetVal("URLA.X215", "");
      }
    }

    private void calculateHomeEducationSubData(string id, string val)
    {
      if (!this.USEURLA2020 && !this.loan.IsTemplate)
        return;
      if (id == "URLA.X299" && val != "Y")
      {
        this.SetVal("URLA.X301", "");
        this.SetVal("URLA.X303", "");
        this.SetVal("URLA.X305", "");
        this.SetVal("URLA.X307", "");
        this.copyBorrowerUrlaCounselingFormat("URLA.X301", "");
      }
      else
      {
        if (!(id == "URLA.X300") || !(val != "Y"))
          return;
        this.SetVal("URLA.X302", "");
        this.SetVal("URLA.X304", "");
        this.SetVal("URLA.X306", "");
        this.SetVal("URLA.X308", "");
        this.copyCoBorrowerUrlaCounselingFormat("URLA.X302", "");
      }
    }

    private void calculateLockRequestPropertyAddress(string id, string val)
    {
      switch (id)
      {
        case "1825":
          if (this.Val("2942") == "")
          {
            if (this.USEURLA2020)
            {
              this.SetVal("4516", this.Val("URLA.X73"));
              this.SetVal("4517", this.Val("URLA.X74"));
              this.SetVal("4518", this.Val("URLA.X75"));
              this.concatenateAddress("2942", "4516", "4517", "4518");
            }
            else if (this.Val("2942") == "")
              this.SetVal("2942", this.Val("11"));
          }
          this.SetVal("4516", this.Val("2942"));
          if (!this.USEURLA2020)
          {
            this.SetVal("4517", "");
            this.SetVal("4518", "");
            this.SetVal("URLA.X74", "");
            this.SetVal("URLA.X75", "");
            break;
          }
          break;
        case "URLA.X73":
          this.SetVal("4516", this.Val("URLA.X73"));
          break;
        case "URLA.X74":
          this.SetVal("4517", this.Val("URLA.X74"));
          break;
        case "URLA.X75":
          this.SetVal("4518", this.Val("URLA.X75"));
          break;
        default:
          if (id == "11" && !this.USEURLA2020)
          {
            this.SetVal("4516", this.Val("11"));
            this.SetVal("4517", "");
            this.SetVal("4518", "");
            break;
          }
          break;
      }
      if (!(id == "URLA.X73") && !(id == "URLA.X74") && !(id == "URLA.X75") && !(id == "4516") && !(id == "4517") && !(id == "4518") || !this.USEURLA2020)
        return;
      this.concatenateAddress("2942", "4516", "4517", "4518");
    }

    private void clearVOMFutureProposeUsageTypeOtherDesc(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      string str1 = id.Substring(0, 2);
      string str2 = id.Substring(id.Length - 2, 2);
      if (!(str1 == "FM") || !(str2 == "55") || !(val != "Other"))
        return;
      this.SetVal(id.Substring(0, id.Length - 2) + "56", "");
    }

    private void clearPurchaseCreditSourceType(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      if (id == "202" && val == string.Empty)
        this.SetVal("4667", "");
      else if (id == "1091" && val == string.Empty)
        this.SetVal("4668", "");
      else if (id == "1106" && val == string.Empty)
      {
        this.SetVal("4669", "");
      }
      else
      {
        if (!(id == "1646") || !(val == string.Empty))
          return;
        this.SetVal("4670", "");
      }
    }

    internal double GetVOALLienAmount(bool isFirstLien, bool useQualifyingAmount = false)
    {
      if (!this.USEURLA2020)
        return 0.0;
      BorrowerPair pair = (BorrowerPair) null;
      BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
      if (borrowerPairs.Length > 1)
      {
        string id = this.loan.CurrentBorrowerPair.Id;
        for (int index = 1; index < borrowerPairs.Length; ++index)
        {
          if (id == borrowerPairs[index].Id)
          {
            pair = borrowerPairs[index];
            this.loan.SetBorrowerPair(borrowerPairs[0]);
            break;
          }
        }
      }
      int ofAdditionalLoans = this.loan.GetNumberOfAdditionalLoans();
      int num1 = 0;
      double voalLienAmount = 0.0;
      double num2 = 0.0;
      for (int index = 1; index <= ofAdditionalLoans; ++index)
      {
        string str = "URLARAL" + index.ToString("00");
        string field = this.loan.GetField(str + "17");
        if (isFirstLien)
        {
          if (!(field != "1"))
          {
            ++num1;
            if (num1 == 1)
            {
              if (useQualifyingAmount)
                num2 = this.FltVal(str + "34");
              voalLienAmount = !useQualifyingAmount || num2 <= 0.0 ? this.FltVal(str + "18") : num2;
            }
            else
            {
              voalLienAmount = 0.0;
              break;
            }
          }
        }
        else if (!(field == "1") && !(field == string.Empty))
        {
          if (useQualifyingAmount)
            num2 = this.FltVal(str + "34");
          if (useQualifyingAmount && num2 > 0.0)
            voalLienAmount += num2;
          else
            voalLienAmount += this.FltVal(str + "18");
        }
      }
      if (pair != null && pair.Id != this.loan.CurrentBorrowerPair.Id)
        this.loan.SetBorrowerPair(pair);
      return voalLienAmount;
    }

    private void clearCountryofCitizenship(string id, string val)
    {
      if (string.IsNullOrEmpty(this.Val("URLA.X1")) || this.Val("URLA.X1") != "NonPermanentResidentAlien")
        this.loan.SetField("URLA.X263", string.Empty);
      if (!string.IsNullOrEmpty(this.Val("URLA.X2")) && !(this.Val("URLA.X2") != "NonPermanentResidentAlien"))
        return;
      this.loan.SetField("URLA.X264", string.Empty);
    }

    private void clearDeclarationFields(string id, string val)
    {
      if (!(val == "No") && !(val == ""))
        return;
      switch (id)
      {
        case "418":
          this.SetVal("403", string.Empty);
          this.SetVal("981", string.Empty);
          this.SetVal("1069", string.Empty);
          break;
        case "1343":
          this.SetVal("1108", string.Empty);
          this.SetVal("1015", string.Empty);
          this.SetVal("1070", string.Empty);
          break;
      }
    }

    private void clearAllForeignAddressBlocks(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      string fieldIdPrefix = "";
      string key = id;
      if ((id.StartsWith("FR") || id.StartsWith("FE") || id.StartsWith("BE") || id.StartsWith("CE") || id.StartsWith("FM") && !id.EndsWith("28") || id.StartsWith("DD") || id.StartsWith("FL")) && !id.StartsWith("FE05") && !id.StartsWith("FE06"))
      {
        key = id.Substring(0, 2);
        fieldIdPrefix = key + id.Substring(2, 2);
      }
      else if (id.StartsWith("URLAROL"))
      {
        key = id.Substring(0, 7);
        fieldIdPrefix = key + id.Substring(7, 2);
      }
      else if (id.StartsWith("NBOC"))
      {
        key = id.Substring(0, 4);
        fieldIdPrefix = key + id.Substring(4, 2);
      }
      else if (id.StartsWith("BR") || id.StartsWith("CR"))
      {
        key = id.Substring(0, 2) + id.Substring(4, 2);
        fieldIdPrefix = id.Substring(0, 4);
      }
      else if (id.StartsWith("FM") && id.EndsWith("28"))
      {
        if (!(this.Val(id) == "Y"))
          return;
        this.SetVal("FM" + id.Substring(2, 2) + "58", string.Empty);
        this.SetVal("FM" + id.Substring(2, 2) + "57", "US");
        this.SetVal("FM" + id.Substring(2, 2) + "51", "US");
        return;
      }
      try
      {
        this.clearForeignAddressFields(val, fieldIdPrefix, this.foreignAddressFieldsDict[key][0], this.foreignAddressFieldsDict[key][1], this.foreignAddressFieldsDict[key][2], this.foreignAddressFieldsDict[key][3], this.foreignAddressFieldsDict[key][4], this.foreignAddressFieldsDict[key][5], this.foreignAddressFieldsDict[key][6], this.foreignAddressFieldsDict[key][7], this.foreignAddressFieldsDict[key][8], this.foreignAddressFieldsDict[key][9]);
      }
      catch
      {
      }
      switch (id)
      {
        case "URLA.X267":
          this.SetVal("ULDD.X27", this.Val("URLA.X267", this.GetBorrowerPairs()[0]) == "N" ? "US" : string.Empty);
          break;
        case "URLA.X268":
          this.SetVal("ULDD.X155", !(this.Val("URLA.X268", this.GetBorrowerPairs()[0]) != "Y") || string.IsNullOrEmpty(this.Val("4004", this.GetBorrowerPairs()[0])) ? string.Empty : "US");
          break;
      }
    }

    private void calculateCountryCode(string id, string val)
    {
      if (!(id == "4004"))
        return;
      this.SetVal("ULDD.X155", !(this.Val("URLA.X268", this.GetBorrowerPairs()[0]) != "Y") || string.IsNullOrEmpty(this.Val("4004")) ? string.Empty : "US");
    }

    private void clearForeignAddressFields(
      string isForeign,
      string fieldIdPrefix,
      string oldStreetAdr,
      string streetAdr,
      string city,
      string state,
      string zip,
      string countryCode,
      string country,
      string unitType,
      string unitNumber,
      string county)
    {
      if (isForeign == "N")
      {
        if (!string.IsNullOrEmpty(oldStreetAdr))
          this.SetVal(fieldIdPrefix + oldStreetAdr, string.Empty);
        this.SetVal(fieldIdPrefix + streetAdr, string.Empty);
        this.SetVal(fieldIdPrefix + city, string.Empty);
        this.SetVal(fieldIdPrefix + state, string.Empty);
        this.SetVal(fieldIdPrefix + zip, string.Empty);
        if (!string.IsNullOrEmpty(countryCode))
          this.SetVal(fieldIdPrefix + countryCode, string.Empty);
        this.SetVal(fieldIdPrefix + country, "US");
        if (!string.IsNullOrEmpty(unitType))
          this.SetVal(fieldIdPrefix + unitType, string.Empty);
        if (!string.IsNullOrEmpty(unitNumber))
          this.SetVal(fieldIdPrefix + unitNumber, string.Empty);
        if (string.IsNullOrEmpty(county))
          return;
        this.SetVal(fieldIdPrefix + county, string.Empty);
      }
      else
      {
        if (string.IsNullOrEmpty(countryCode))
          return;
        this.SetVal(fieldIdPrefix + countryCode, string.Empty);
      }
    }

    private void clearDenialSubjectAddressFields(string id, string val)
    {
      if (!this.USEURLA2020)
        return;
      if (id == "DENIAL.X81" && val == "Subject Address" && this.Val("DENIAL.X97") == "Y")
      {
        this.clearDenialAddresses("DENIAL.X97", this.foreignAddressFieldsDict["DENIAL.X97"][1], this.foreignAddressFieldsDict["DENIAL.X97"][2], this.foreignAddressFieldsDict["DENIAL.X97"][3], this.foreignAddressFieldsDict["DENIAL.X97"][4], this.foreignAddressFieldsDict["DENIAL.X97"][6]);
      }
      else
      {
        if (!(id == "DENIAL.X86") || !(val == "Subject Address") || !(this.Val("DENIAL.X99") == "Y"))
          return;
        this.clearDenialAddresses("DENIAL.X99", this.foreignAddressFieldsDict["DENIAL.X99"][1], this.foreignAddressFieldsDict["DENIAL.X99"][2], this.foreignAddressFieldsDict["DENIAL.X99"][3], this.foreignAddressFieldsDict["DENIAL.X99"][4], this.foreignAddressFieldsDict["DENIAL.X99"][6]);
      }
    }

    private void clearDenialAddresses(
      string foreignIndicator,
      string address,
      string city,
      string zip,
      string state,
      string country)
    {
      this.SetVal(foreignIndicator, "N");
      this.SetVal(address, string.Empty);
      this.SetVal(city, string.Empty);
      this.SetVal(zip, string.Empty);
      this.SetVal(state, string.Empty);
      this.SetVal(country, "US");
    }

    private void calculateMilitaryEntitlementIncome(string id, string val)
    {
      string str = id.Substring(0, id.Length - 2);
      double num1 = 0.0;
      for (int index = 65; index < 73; ++index)
        num1 += this.FltVal(str + (object) index);
      double num2 = num1 + this.FltVal(str + "77");
      this.SetCurrentNum(str + "53", num2);
      this.SetVal(str + "63", num2 > 0.0 ? "Y" : "N");
    }

    private void calculatePresentHousingExpenseRent(string id, string val)
    {
    }

    private void calculateTotalMortgageAndTaxes(string id, string val)
    {
    }

    private void calculateAverageRepresentativeCreditScore(string id, string val)
    {
      string str;
      switch (id)
      {
        case "4752":
          str = "4830";
          break;
        case "4830":
          str = "4752";
          break;
        default:
          str = "";
          break;
      }
      string id1 = str;
      if (id1 == "")
        return;
      if (this.IsLocked(id) && !this.IsLocked(id1))
        this.AddLock(id1);
      else if (!this.IsLocked(id) && this.IsLocked(id1))
      {
        this.RemoveCurrentLock(id1);
        if (this.loan.GetNumberOfAUSTrackingHistory(true) > 0)
        {
          AUSTrackingHistoryList trackingHistoryList = this.loan.GetAUSTrackingHistoryList(true);
          AUSTrackingHistoryLog trackingHistoryLog = (AUSTrackingHistoryLog) null;
          for (int i = 0; i < trackingHistoryList.HistoryCount; ++i)
          {
            if (string.Compare(trackingHistoryList.GetHistoryAt(i).DataValues.GetField("AUS.X1"), "DU", true) == 0)
            {
              trackingHistoryLog = trackingHistoryList.GetHistoryAt(i);
              break;
            }
          }
          if (trackingHistoryLog == null)
          {
            this.SetVal("4830", "");
            this.SetVal("4752", "");
            return;
          }
          if (string.Compare(trackingHistoryLog.DataValues.GetField("AUS.X199"), "not applicable", true) == 0)
          {
            this.SetVal("4830", "Y");
            this.SetVal("4752", "");
            return;
          }
          this.SetVal("4830", "N");
          this.SetVal("4752", trackingHistoryLog.DataValues.GetField("AUS.X199"));
          return;
        }
        this.SetVal("4830", "");
        this.SetVal("4752", "");
        return;
      }
      if (id == "4752" && this.IsLocked("4830"))
      {
        this.RemoveCurrentLock("4830");
        this.SetVal("4830", string.IsNullOrEmpty(this.Val("4752")) ? "Y" : "N");
        this.AddLock("4830");
      }
      else
      {
        if (!(id == "4830") || !(this.Val("4830") == "Y") || !this.IsLocked("4752"))
          return;
        this.RemoveCurrentLock("4752");
        this.SetVal("4752", "");
        this.AddLock("4752");
      }
    }

    private void calculateUrlaItemizedCredits(string id, string val)
    {
      if (!(this.Val("1825") == "2009") || this.loan.IsTemplate)
        return;
      this.SetVal("4796", "");
    }

    private void calculateSupplementalInsuranceAmount(string id, string val)
    {
      string val1 = "";
      if (this.USEURLA2020)
      {
        switch (this.Val("1811"))
        {
          case "PrimaryResidence":
            val1 = this.Val("URLA.X144");
            break;
          case "SecondHome":
          case "Investor":
            val1 = this.Val("URLA.X212");
            break;
        }
      }
      this.SetVal("4947", val1);
    }

    private void calculateFirstTimeHomeBuyer(string id, string val)
    {
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string val1 = "";
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (this.Val("4973", borrowerPairs[index]) == "Y" || this.Val("4974", borrowerPairs[index]) == "Y")
        {
          val1 = "Y";
          break;
        }
        if (this.Val("4973", borrowerPairs[index]) == "N" || this.Val("4974", borrowerPairs[index]) == "N")
          val1 = "N";
      }
      this.SetVal("934", val1);
      this.SetVal("3528", this.IsLocked("934") ? this.Val("934") : val1);
    }

    private void calculateAccessoryDwellingUnit(string id, string val)
    {
      if (!(this.Val("URLA.X309") != "Y"))
        return;
      this.SetVal("URLA.X310", "");
      this.SetVal("URLA.X311", "");
      this.SetVal("URLA.X312", "");
      this.SetVal("URLA.X313", "");
      this.SetVal("URLA.X314", "");
    }

    private void calculateCreditReportIndicators(string id, string val)
    {
      if (id == "4750" && val == "Y" && this.Val("5006") == "Y")
      {
        this.SetVal("5006", "");
      }
      else
      {
        if (!(id == "5006") || !(val == "Y") || !(this.Val("4750") == "Y"))
          return;
        this.SetVal("4750", "");
      }
    }

    private void copyBorrowerUrlaCounselingFormat(string id, string val)
    {
      string str1 = this.Val("URLA.X301");
      string str2 = this.Val("URLA.X154");
      if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
        this.SetVal("ULDD.X207", "");
      else if (!string.IsNullOrEmpty(str1))
        this.SetVal("ULDD.X207", str1 == "InPerson" ? "Classroom" : "Home Study");
      else if (!string.IsNullOrEmpty(str2) && str2 != "InPerson" && str2 != "WebBased")
        this.SetVal("ULDD.X207", "Individual");
      this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("ULDD.X207", this.Val("ULDD.X207"));
    }

    private void copyCoBorrowerUrlaCounselingFormat(string id, string val)
    {
      string str1 = this.Val("URLA.X302");
      string str2 = this.Val("URLA.X160");
      if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
        this.SetVal("ULDD.X208", "");
      else if (!string.IsNullOrEmpty(str1))
        this.SetVal("ULDD.X208", str1 == "InPerson" ? "Classroom" : "Home Study");
      else if (!string.IsNullOrEmpty(str2) && str2 != "InPerson" && str2 != "WebBased")
        this.SetVal("ULDD.X208", "Individual");
      this.calObjs.ULDDExpCal.CalcFannieMaeExportFields("ULDD.X208", this.Val("ULDD.X208"));
    }

    private enum Employer
    {
      BasePay = 19, // 0x00000013
      Overtime = 20, // 0x00000014
      Bonus = 21, // 0x00000015
      Commissions = 22, // 0x00000016
      Other = 23, // 0x00000017
      MilitaryEntitlements = 53, // 0x00000035
    }
  }
}
