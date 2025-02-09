// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.HMDACalculation
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  internal class HMDACalculation : CalculationBase
  {
    private const string className = "HMDACalculation�";
    private static readonly string sw = Tracing.SwDataEngine;
    internal Routine CalcAll;
    internal Routine CalcLoanType;
    internal Routine CalcLoanAmount;
    internal Routine CalcIncome;
    internal Routine CalcBusinessOrCommercialPurpose;
    internal Routine CalcApplicationDate;
    internal Routine CalcManufacturedProperty;
    internal Routine CalcRiskAssessment;
    internal Routine CalcOtherCreditScoringModel;
    internal Routine CalcTotalLoanCosts;
    internal Routine CalcTotalPointsAndFees;
    internal Routine CalcOriginationCharges;
    internal Routine CalcDiscountPoints;
    internal Routine CalcLenderCredits;
    internal Routine CalcInterestRate;
    internal Routine CalcPrepaymentPenaltyPeriod;
    internal Routine CalcLoanTerm;
    internal Routine CalcIntroRatePeriod;
    internal Routine CalcHmdaAge;
    internal Routine CalcTypeOfPurchaser;
    internal Routine CalcHOEPAStatus;
    internal Routine CalcCreditScoreForDecisionMaking;
    internal Routine CalcCreditScoringModel;
    internal Routine CalcAllLoanCostsBy2015Indicator;
    internal Routine CalcAllLoanCostsByActionTaken;
    internal Routine UpdateHMDATransmittalSheetDetails;
    internal Routine CalcHmdaSyncaddressfields;
    internal Routine CalcReasonForDenial;
    internal Routine CalcHmdaRateSpread;
    internal Routine CalcNocoapplicant;
    internal Routine CalcGender;
    internal Routine CalcRepurchasedReportingYear;
    internal Routine CalcRepurchasedLoanAmount;
    internal Routine CalcRepurchasedTypeOfPurchaser;
    internal Routine CalcRepurchasedActiontaken;
    internal Routine CalcHmdaRepurchasedActionDate;
    internal Routine CalcEthnicity;
    internal Routine CalcDoNotWish;
    internal Routine CalcSortRaceCategory;
    internal Routine CalcSortEthnicityCategory;
    internal Routine CalcHMDAReporting;
    internal Routine CalcHMDARace;
    internal Routine CalcHMDAEthnicity;
    internal Routine CalcHMDASex;
    internal Routine CalcAllHMDASettingsFunc;
    internal Routine CalcHmdaNmlsLoanOriginatorId;
    internal Routine CalcHmdaLoanPurpose;
    internal Routine CalcHmdaInterestOnlyIndicator;
    internal Routine CalcHmdaOriginationCharges;
    internal Routine CalcHmdaCDRequired;
    internal Routine CalcHMDACountyCensusTrackCode;
    internal Routine CalcHMDAAusRecommendation;
    internal Routine CalcDefaultFields;
    internal Routine CalcHMDAfields;
    internal Routine CalcNewHmdaInterestOnlyIndicator;
    internal Routine CopyHmdaValues;
    internal Routine CalcImportEthnicitySortingWithPairs;
    internal Routine SortEthnicityCategoryWithPairs;
    internal Routine CalcRaceImportSortingWithPairs;
    internal Routine SortRaceCategoryWithPairs;
    internal Routine CalcCountyCode;
    internal Routine CalculateMSACode;
    private string hmdaApplicationDateID = "745";
    private Dictionary<int, int> borrEthnicityList = new Dictionary<int, int>()
    {
      {
        4210,
        1
      },
      {
        4211,
        2
      },
      {
        4243,
        3
      },
      {
        4212,
        4
      },
      {
        4147,
        10
      },
      {
        4144,
        11
      },
      {
        4145,
        12
      },
      {
        4146,
        13
      }
    };
    private Dictionary<int, int> coBorrEthnicityList = new Dictionary<int, int>()
    {
      {
        4213,
        1
      },
      {
        4214,
        2
      },
      {
        4246,
        3
      },
      {
        4215,
        4
      },
      {
        4162,
        10
      },
      {
        4159,
        11
      },
      {
        4160,
        12
      },
      {
        4161,
        13
      }
    };
    private Dictionary<int, int> borrRaceList = new Dictionary<int, int>();
    private Dictionary<int, int> coBorrRaceList = new Dictionary<int, int>();
    private SortedList<int, int> borrEthnicitySortedList = new SortedList<int, int>();
    private SortedList<int, int> coBorrEthnicitySortedList = new SortedList<int, int>();
    private SortedList<int, int> borrRaceSortedList = new SortedList<int, int>();
    private SortedList<int, int> coBorrRaceSortedList = new SortedList<int, int>();
    private Dictionary<int, string> raceCodeDescriptionMapList = new Dictionary<int, string>()
    {
      {
        1,
        "American Indian or Alaska Native"
      },
      {
        2,
        "Asian"
      },
      {
        3,
        "Black or African American"
      },
      {
        4,
        "Native Hawaiian or Other Pacific Islander"
      },
      {
        5,
        "White"
      },
      {
        6,
        "Information Not Provided"
      },
      {
        7,
        "Not applicable"
      },
      {
        21,
        "Asian Indian"
      },
      {
        22,
        "Chinese"
      },
      {
        23,
        "Filipino"
      },
      {
        24,
        "Japanese"
      },
      {
        25,
        "Korean"
      },
      {
        26,
        "Vietnamese"
      },
      {
        19,
        "Other Asian"
      },
      {
        41,
        "Native Hawaiian"
      },
      {
        42,
        "Guamanian or Chamorro"
      },
      {
        43,
        "Samoan"
      },
      {
        20,
        "Other Pacific Islander"
      }
    };
    private Dictionary<int, string> ethnicityCodeDescriptionMapList = new Dictionary<int, string>()
    {
      {
        1,
        "Hispanic Or Latino"
      },
      {
        2,
        "Not Hispanic Or Latino"
      },
      {
        3,
        "Information Not Provided"
      },
      {
        4,
        "Not Applicable"
      },
      {
        5,
        "No CoApplicant"
      },
      {
        11,
        "Mexican"
      },
      {
        12,
        "Puerto Rican"
      },
      {
        13,
        "Cuban"
      },
      {
        10,
        "Other Hispanic or Latino"
      }
    };
    private readonly List<string> _default1111Fields = new List<string>()
    {
      "HMDA.X116",
      "HMDA.X117",
      "HMDA.X118",
      "HMDA.X119",
      "HMDA.X114",
      "HMDA.X120",
      "HMDA.X115",
      "HMDA.X38",
      "HMDA.X39",
      "HMDA.X40",
      "HMDA.X42",
      "HMDA.X56",
      "HMDA.X57",
      "HMDA.X58",
      "HMDA.X44",
      "HMDA.X50"
    };
    private readonly List<string> _defaultExemptFields = new List<string>()
    {
      "HMDA.X88",
      "HMDA.X89",
      "HMDA.X87",
      "HMDA.X15",
      "HMDA.X77",
      "HMDA.X78",
      "HMDA.X79",
      "HMDA.X35",
      "HMDA.X80",
      "HMDA.X81",
      "HMDA.X82",
      "HMDA.X56",
      "HMDA.X36",
      "HMDA.X37",
      "HMDA.X83",
      "HMDA.X84",
      "HMDA.X85",
      "HMDA.X41",
      "HMDA.X86"
    };

    internal HMDACalculation(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
      : base(l, calObjs)
    {
      this.CalcLoanType = this.RoutineX(new Routine(this.calculateHmdaLoanType));
      this.CalcLoanAmount = this.RoutineX(new Routine(this.calculateHmdaLoanAmount));
      this.CalcIncome = this.RoutineX(new Routine(this.calculateHmdaIncome));
      this.CalcBusinessOrCommercialPurpose = this.RoutineX(new Routine(this.calculateHmdaBusinessOrCommercialPurpose));
      this.CalcApplicationDate = this.RoutineX(new Routine(this.calculateHmdaApplicationDate));
      this.CalcManufacturedProperty = this.RoutineX(new Routine(this.calculateHmdaManufacturedProperty));
      this.CalcRiskAssessment = this.RoutineX(new Routine(this.calculateHmdaRiskAssessment));
      this.CalcTotalLoanCosts = this.RoutineX(new Routine(this.calculateHmdaTotalLoanCosts));
      this.CalcTotalPointsAndFees = this.RoutineX(new Routine(this.calculateHmdaTotalPointsAndFees));
      this.CalcOriginationCharges = this.RoutineX(new Routine(this.calculateHmdaOriginationCharges));
      this.CalcDiscountPoints = this.RoutineX(new Routine(this.calculateHmdaDiscountPoints));
      this.CalcLenderCredits = this.RoutineX(new Routine(this.calculateHmdaLenderCredits));
      this.CalcInterestRate = this.RoutineX(new Routine(this.calculateHmdaInterestRate));
      this.CalcPrepaymentPenaltyPeriod = this.RoutineX(new Routine(this.calculateHmdaPrepaymentPenaltyPeriod));
      this.CalcLoanTerm = this.RoutineX(new Routine(this.calculateHmdaLoanTerm));
      this.CalcIntroRatePeriod = this.RoutineX(new Routine(this.calculateHmdaIntroRatePeriod));
      this.CalcHmdaAge = this.RoutineX(new Routine(this.calculateHmdaAge));
      this.CalcTypeOfPurchaser = this.RoutineX(new Routine(this.calculateTypeOfPurchaser));
      this.CalcHOEPAStatus = this.RoutineX(new Routine(this.calculateHmdaHOEPAStatus));
      this.CalcCreditScoreForDecisionMaking = this.RoutineX(new Routine(this.calculateHmdaCreditScoreForDecision));
      this.CalcCreditScoringModel = this.RoutineX(new Routine(this.calculateHmdaCreditScoringModel));
      this.CalcAllLoanCostsBy2015Indicator = this.RoutineX(new Routine(this.calculateHmdaAllLoanCostsBy2015Indicator));
      this.CalcAllLoanCostsByActionTaken = this.RoutineX(new Routine(this.calculateHmdaAllLoanCostsByActionTaken));
      this.CalcReasonForDenial = this.RoutineX(new Routine(this.calculateReasonForDenial));
      this.CalcNocoapplicant = this.RoutineX(new Routine(this.calculateNocoapplicant));
      this.CalcHmdaRateSpread = this.RoutineX(new Routine(this.calculateHmdaRateSpread));
      this.CalcOtherCreditScoringModel = this.RoutineX(new Routine(this.calculateHmdaOtherCreditScoringModel));
      this.UpdateHMDATransmittalSheetDetails = this.RoutineX(new Routine(this.updateHMDATransmittalSheetDetails));
      this.CalcHmdaSyncaddressfields = this.RoutineX(new Routine(this.calculateHmdaSyncaddressfields));
      this.CalcGender = this.RoutineX(new Routine(this.calculateGender));
      this.CalcRepurchasedReportingYear = this.RoutineX(new Routine(this.calculateRepurchasedReportingYear));
      this.CalcRepurchasedLoanAmount = this.RoutineX(new Routine(this.calculateHmdaRepurchasedLoanAmount));
      this.CalcRepurchasedTypeOfPurchaser = this.RoutineX(new Routine(this.calculateRepurchasedTypeOfPurchaser));
      this.CalcRepurchasedActiontaken = this.RoutineX(new Routine(this.calculateRepurchasedActiontaken));
      this.CalcHmdaRepurchasedActionDate = this.RoutineX(new Routine(this.calculateHmdaRepurchasedActionDate));
      this.CalcEthnicity = this.RoutineX(new Routine(this.calculateEthnicity));
      this.CalcDoNotWish = this.RoutineX(new Routine(this.calculateDoNotWish));
      this.CalcSortRaceCategory = this.RoutineX(new Routine(this.sortRaceCategory));
      this.CalcSortEthnicityCategory = this.RoutineX(new Routine(this.sortEthnicityCategory));
      this.CalcHMDAReporting = this.RoutineX(new Routine(this.calculateHMDAReporting));
      this.CalcAllHMDASettingsFunc = this.RoutineX(new Routine(this.calculateAllHMDASettingsFunc));
      this.CalcHMDARace = this.RoutineX(new Routine(this.calculateHmdaRace));
      this.CalcHMDAEthnicity = this.RoutineX(new Routine(this.calculateHmdaEthnicity));
      this.CalcHMDASex = this.RoutineX(new Routine(this.calculateHmdaSex));
      this.CalcHmdaNmlsLoanOriginatorId = this.RoutineX(new Routine(this.calculateHmdaNmlsLoanOriginatorId));
      this.CalcHmdaLoanPurpose = this.RoutineX(new Routine(this.calculateHmdaLoanPurpose));
      this.CalcHmdaInterestOnlyIndicator = this.RoutineX(new Routine(this.calculateHmdaInterestOnlyIndicator));
      this.CalcNewHmdaInterestOnlyIndicator = this.RoutineX(new Routine(this.calculateNewHmdaInterestOnlyIndicator));
      this.CalcHmdaOriginationCharges = this.RoutineX(new Routine(this.calculateHmdaOriginationCharges));
      this.CalcHmdaCDRequired = this.RoutineX(new Routine(this.calculateHmdaCDRequired));
      this.CalcHMDACountyCensusTrackCode = this.RoutineX(new Routine(this.calculateHMDACountyCensusTrackCode));
      this.CalcHMDAAusRecommendation = this.RoutineX(new Routine(this.calculateHMDAAusRecommendation));
      this.CalcDefaultFields = this.RoutineX(new Routine(this.setDefaultHMDAExceptValues));
      this.CalcHMDAfields = this.RoutineX(new Routine(this.calculateHMDAfields));
      this.CopyHmdaValues = this.RoutineX(new Routine(this.copyHmdaValues));
      this.CalcAll = this.RoutineX(new Routine(this.calculateAll));
      this.CalcImportEthnicitySortingWithPairs = this.RoutineX(new Routine(this.calculateImportEthnicitySortingWithPairs));
      this.SortEthnicityCategoryWithPairs = this.RoutineX(new Routine(this.sortEthnicityCategoryWithPairs));
      this.CalcRaceImportSortingWithPairs = this.RoutineX(new Routine(this.calculateRaceImportSortingWithPairs));
      this.SortRaceCategoryWithPairs = this.RoutineX(new Routine(this.sortRaceCategoryWithPairs));
      this.CalcCountyCode = this.RoutineX(new Routine(this.calculateCountyCode));
      this.CalculateMSACode = this.RoutineX(new Routine(this.calculateMSACode));
      this.addFieldHandlers();
      this.setSortingValuesForRace();
    }

    private void setDefault1111Value()
    {
      foreach (string default1111Field in this._default1111Fields)
      {
        this.SetVal(default1111Field, "Exempt");
        this.AddLock(default1111Field);
      }
    }

    private void setDefaultExemptValue()
    {
      foreach (string defaultExemptField in this._defaultExemptFields)
      {
        this.SetVal(defaultExemptField, "Exempt");
        this.AddLock(defaultExemptField);
      }
    }

    private void setDefaultHMDAExceptValues(string id, string val)
    {
      if (this.loan.Settings.HMDAInfo == null || !this.loan.Settings.HMDAInfo.HMDANuli || !(this.Val("HMDA.X113") == "Y"))
        return;
      this.setDefault1111Value();
      this.setDefaultExemptValue();
    }

    private void copyHmdaValues(string id, string val)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 963857944:
          if (!(id == "1659"))
            break;
          this.SetVal("HMDA.X114", val);
          break;
        case 1492769932:
          if (!(id == "4177") || !(this.Val("HMDA.X118") != "Exempt"))
            break;
          this.SetVal("HMDA.X118", val);
          break;
        case 1526325170:
          if (!(id == "4175") || !(this.Val("HMDA.X117") != "Exempt"))
            break;
          this.SetVal("HMDA.X117", val);
          break;
        case 1543102789:
          if (!(id == "4174") || !(this.Val("HMDA.X116") != "Exempt"))
            break;
          this.SetVal("HMDA.X116", val);
          break;
        case 1610213265:
          if (!(id == "4178") || !(this.Val("HMDA.X119") != "Exempt"))
            break;
          this.SetVal("HMDA.X119", val);
          break;
        case 2233833150:
          if (!(id == "NEWHUD.X6"))
            break;
          this.SetVal("HMDA.X115", val);
          break;
        case 3556208321:
          if (!(id == "HMDA.X109"))
            break;
          this.SetVal("HMDA.X120", val);
          break;
      }
    }

    private void calculateHMDAfields(string id, string val)
    {
      if (this.Val("HMDA.X113") != "Y")
        return;
      this.calculateHmdaDenialReasons(id, val);
      this.calculateHmdaAllLoanCostsByActionTaken(id, val);
      this.calculateHmdaAge(id, val);
      this.calculateHmdaLoanAmount(id, val);
      this.calculateHmdaIncome(id, val);
      this.calculateHmdaManufacturedProperty(id, val);
      this.calculateHmdaTotalPointsAndFees(id, val);
      this.calculateHmdaLenderCredits(id, val);
      this.calculateHmdaLoanTerm(id, val);
      this.calculateHmdaNmlsLoanOriginatorId(id, val);
      this.calculateHMDACountyCensusTrackCode(id, val);
      this.calObjs.Cal.CalcLTV(id, val);
    }

    private void setSortingValuesForRace()
    {
      int num1 = 1;
      for (int index = 1524; index <= 1530; ++index)
        this.borrRaceList.Add(index == 1529 ? 4244 : index, num1++);
      int num2 = 19;
      Dictionary<int, int> borrRaceList1 = this.borrRaceList;
      int num3 = num2;
      int num4 = num3 + 1;
      borrRaceList1.Add(4154, num3);
      Dictionary<int, int> borrRaceList2 = this.borrRaceList;
      int num5 = num4;
      int num6 = num5 + 1;
      borrRaceList2.Add(4158, num5);
      for (int key = 4148; key < 4158; ++key)
      {
        if (key != 4154)
        {
          if (key == 4155)
            num6 = 41;
          this.borrRaceList.Add(key, num6++);
        }
      }
      int num7 = 1;
      for (int index = 1532; index <= 1538; ++index)
        this.coBorrRaceList.Add(index == 1537 ? 4247 : index, num7++);
      int num8 = 19;
      Dictionary<int, int> coBorrRaceList1 = this.coBorrRaceList;
      int num9 = num8;
      int num10 = num9 + 1;
      coBorrRaceList1.Add(4169, num9);
      Dictionary<int, int> coBorrRaceList2 = this.coBorrRaceList;
      int num11 = num10;
      int num12 = num11 + 1;
      coBorrRaceList2.Add(4173, num11);
      for (int key = 4163; key < 4173; ++key)
      {
        if (key != 4169)
        {
          if (key == 4170)
            num12 = 41;
          this.coBorrRaceList.Add(key, num12++);
        }
      }
    }

    private void addFieldHandlers()
    {
      Routine routine1 = this.RoutineX(new Routine(this.calculateReasonForDenial));
      this.AddFieldHandler("HMDA.X21", routine1);
      this.AddFieldHandler("HMDA.X22", routine1);
      this.AddFieldHandler("HMDA.X23", routine1);
      this.AddFieldHandler("HMDA.X33", routine1);
      Routine routine2 = this.RoutineX(new Routine(this.calculateNocoapplicant));
      this.AddFieldHandler("1531", routine2 + this.CalcEthnicity);
      this.AddFieldHandler("4188", routine2 + new Routine(this.sortEthnicityCategory));
      Routine routine3 = this.RoutineX(new Routine(this.calculateGender));
      this.AddFieldHandler("4195", routine3 + new Routine(this.calculateDoNotWish));
      this.AddFieldHandler("4199", routine3 + new Routine(this.calculateDoNotWish));
      Routine routine4 = this.RoutineX(new Routine(this.calculateHmdaRiskAssessment));
      this.AddFieldHandler("HMDA.X44", routine4);
      this.AddFieldHandler("HMDA.X45", routine4);
      this.AddFieldHandler("HMDA.X46", routine4);
      this.AddFieldHandler("HMDA.X47", routine4);
      this.AddFieldHandler("HMDA.X48", routine4);
      this.AddFieldHandler("HMDA.X50", routine4);
      this.AddFieldHandler("HMDA.X51", routine4);
      this.AddFieldHandler("HMDA.X52", routine4);
      this.AddFieldHandler("HMDA.X53", routine4);
      this.AddFieldHandler("HMDA.X54", routine4);
      this.AddFieldHandler("CD2.XSTD", this.RoutineX(new Routine(this.calculateHmdaTotalLoanCosts)));
      Routine routine5 = this.RoutineX(new Routine(this.calculateHmdaAge));
      this.AddFieldHandler("38", routine5);
      this.AddFieldHandler("70", routine5);
      this.AddFieldHandler("3840", routine5 + this.CalcCreditScoreForDecisionMaking + this.CalcCreditScoringModel + this.CalcSortEthnicityCategory + this.CalcSortRaceCategory + this.CalcHMDARace + this.CalcHMDASex + this.CalcHMDAEthnicity + this.CalcSortEthnicityCategory + this.CalcSortRaceCategory);
      Routine routine6 = this.RoutineX(new Routine(this.calculateHmdaInterestRate));
      this.AddFieldHandler("4113", routine6 + this.calObjs.NewHud2015Cal.CalcLECDDisplayFields);
      this.AddFieldHandler("HMDA.X81", routine6);
      this.AddFieldHandler("1397", this.RoutineX(new Routine(this.calculateTypeOfPurchaser)));
      this.AddFieldHandler("S32DISC.X51", this.RoutineX(new Routine(this.calculateHmdaHOEPAStatus)));
      Routine routine7 = this.RoutineX(new Routine(this.calculateHmdaDebtToIncomeRatio));
      this.AddFieldHandler("742", this.RoutineX(routine7));
      this.AddFieldHandler("HMDA.X97", this.RoutineX(routine7));
      Routine routine8 = this.RoutineX(new Routine(this.calculateHmdaCLTV));
      this.AddFieldHandler("976", this.RoutineX(routine8));
      this.AddFieldHandler("HMDA.X98", this.RoutineX(routine8));
      Routine routine9 = this.RoutineX(new Routine(this.calculateHmdaNmlsLoanOriginatorId));
      this.AddFieldHandler("3238", routine9);
      this.AddFieldHandler("745", routine9 + this.CalcHmdaLoanPurpose);
      this.AddFieldHandler("1393", this.RoutineX(new Routine(this.calculateHmdaDenialReasons)));
      Routine routine10 = this.RoutineX(new Routine(this.calculateHmdaCreditScoringModel));
      this.AddFieldHandler("4174", routine10 + new Routine(this.copyHmdaValues));
      this.AddFieldHandler("4177", routine10 + new Routine(this.copyHmdaValues));
      Routine routine11 = this.RoutineX(new Routine(this.calculateHmdaRepurchasedLoanAmount));
      this.AddFieldHandler("ULDD.X1", routine11);
      this.AddFieldHandler("3312", routine11);
      this.AddFieldHandler("3312", this.RoutineX(new Routine(this.calculateRepurchasedTypeOfPurchaser)) + this.CalcRepurchasedReportingYear + this.CalcRepurchasedActiontaken + this.CalcHmdaRepurchasedActionDate + new Routine(this.calculateHmdaRepurchasedActionDate2));
      this.AddFieldHandler("HMDA.X96", this.RoutineX(new Routine(this.calculateHmdaRepurchasedActionDate2)));
      this.AddFieldHandler("HMDA.X92", this.RoutineX(new Routine(this.calculateRepurchasedReportingYear)));
      Routine routine12 = this.RoutineX(new Routine(this.calculateEthnicity));
      this.AddFieldHandler("4205", routine12 + new Routine(this.calculateDoNotWish) + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4206", routine12 + new Routine(this.calculateDoNotWish) + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4210", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4211", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4212", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4213", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4214", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4215", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4144", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4145", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4146", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4147", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4159", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4160", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4161", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4162", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4243", routine12 + new Routine(this.sortEthnicityCategory));
      this.AddFieldHandler("4246", routine12 + new Routine(this.sortEthnicityCategory));
      Routine routine13 = this.RoutineX(new Routine(this.calculateHMDACountyCensusTrackCode));
      this.AddFieldHandler("1395", routine13);
      this.AddFieldHandler("1396", routine13);
      this.AddFieldHandler("700", routine13);
      Routine routine14 = this.RoutineX(new Routine(this.calculateDoNotWish));
      this.AddFieldHandler("188", routine14);
      this.AddFieldHandler("189", routine14);
      this.AddFieldHandler("NEWHUD.X796", this.CalcOriginationCharges);
      this.AddFieldHandler("HMDA.X121", this.CalcDiscountPoints + this.CalcLenderCredits + this.CalcHmdaOriginationCharges);
      this.AddFieldHandler("NEWHUD2.X935", this.CalcDiscountPoints);
      this.AddFieldHandler("NEWHUD2.X936", this.CalcDiscountPoints);
      Routine routine15 = this.RoutineX(new Routine(this.copyHmdaValues));
      this.AddFieldHandler("1659", routine15);
      this.AddFieldHandler("NEWHUD.X6", routine15);
      this.AddFieldHandler("HMDA.X109", routine15);
      Routine calcLoanTerm = this.CalcLoanTerm;
      this.AddFieldHandler("LE1.X2", calcLoanTerm);
      this.AddFieldHandler("LE1.X3", calcLoanTerm);
    }

    private void calculateAll(string id, string val)
    {
      this.calculateHmdaLoanType(id, val);
      this.calculateHmdaIncome(id, val);
      this.calculateHmdaLoanAmount(id, val);
      this.calculateHmdaBusinessOrCommercialPurpose(id, val);
      this.calculateHmdaDiscountPoints(id, val);
      this.calculateHmdaCreditScoreForDecision(id, val);
      this.calculateHmdaLoanTerm(id, val);
      this.calculateHmdaAllLoanCostsBy2015Indicator(id, val);
      this.calculateHmdaAllLoanCostsByActionTaken(id, val);
      this.calculateHmdaAge(id, val);
      this.calculateHmdaSyncaddressfields(id, val);
      this.calculateHmdaInterestOnlyIndicator(id, val);
      this.calculateNewHmdaInterestOnlyIndicator(id, val);
      this.calculateHMDACountyCensusTrackCode(id, val);
    }

    private void calculateHmdaAllLoanCostsBy2015Indicator(string id, string val)
    {
      this.calculateHmdaTotalLoanCosts(id, val);
      this.calculateHmdaTotalPointsAndFees(id, val);
      this.calculateHmdaOriginationCharges(id, val);
      this.calculateHmdaDiscountPoints(id, val);
      this.calculateHmdaLenderCredits(id, val);
    }

    private void calculateHmdaAllLoanCostsByActionTaken(string id, string val)
    {
      this.calculateHmdaApplicationDate(id, val);
      this.calculateHmdaInterestRate(id, val);
      this.calculateHmdaPrepaymentPenaltyPeriod(id, val);
      this.calculateTypeOfPurchaser(id, val);
      this.calculateHmdaHOEPAStatus(id, val);
      this.calculateHmdaCreditScoreForDecision(id, val);
      this.calculateHmdaCreditScoringModel(id, val);
      this.calculateReasonForDenial(id, val);
      this.calculateHmdaDebtToIncomeRatio(id, val);
      this.calculateHmdaCLTV(id, val);
      this.calculateHmdaIntroRatePeriod(id, val);
      this.calculateHmdaSubmissionOfApplication(id, val);
      this.calculateHmdaRiskAssessment(id, val);
      this.calculateHmdaEthnicity(id, val);
      this.calculateHmdaRace(id, val);
      this.calculateHmdaSex(id, val);
      this.calculateHmdaRateSpread(id, val);
      this.calculateHmdaTotalLoanCosts(id, val);
      this.calculateHmdaOriginationCharges(id, val);
      this.calculateHmdaDiscountPoints(id, val);
      this.calculateHMDACountyCensusTrackCode(id, val);
      this.calculateHmdaLenderCredits(id, val);
    }

    private void calculateAllHMDASettingsFunc(string id, string val)
    {
      this.calculateHmdaIncome(id, val);
      this.calculateHmdaAge(id, val);
      this.calculateHmdaDebtToIncomeRatio(id, val);
      this.calculateHmdaCLTV(id, val);
      this.calculateHmdaEthnicity(id, val);
      this.calculateHmdaRace(id, val);
      this.calculateHmdaSex(id, val);
    }

    internal void UpdateHMDA2018Profile()
    {
      this.updateHMDATransmittalSheetDetails((string) null, (string) null);
      this.calObjs.D1003Cal.CalcUli((string) null, (string) null);
      if (this.loan.Settings != null && this.loan.Settings.HMDAInfo != null && (this.loan.Settings.HMDAInfo.HMDAApplicationDate ?? "") != "")
        this.calObjs.D1003Cal.CopyCitizenshipAndAge(this.loan.Settings.HMDAInfo.HMDAApplicationDate, this.Val(this.loan.Settings.HMDAInfo.HMDAApplicationDate));
      this.calculateAllHMDASettingsFunc((string) null, (string) null);
      this.calculateHmdaApplicationDate(this.hmdaApplicationDateID, (string) null);
      this.calObjs.D1003URLA2020Cal.CalcURLALoanIdentifier((string) null, (string) null);
    }

    private void calculateHmdaLoanType(string id, string val)
    {
      switch (this.Val("1172"))
      {
        case "Other":
          string str = this.Val("NMLS.X10");
          if (str == "HECM-Standard" || str == "HECM-Saver")
          {
            this.SetVal("HMDA.X30", "FHA");
            break;
          }
          this.SetVal("HMDA.X30", "Conventional");
          break;
        case "HELOC":
        case "Conventional":
          this.SetVal("HMDA.X30", "Conventional");
          break;
        case "VA":
          this.SetVal("HMDA.X30", "VA");
          break;
        case "FHA":
          this.SetVal("HMDA.X30", "FHA");
          break;
        case "FarmersHomeAdministration":
          this.SetVal("HMDA.X30", "USDA-RHS or FSA");
          break;
      }
    }

    private void calculateHmdaLoanAmount(string id, string val)
    {
      string str1 = this.Val("1393");
      double num1 = this.FltVal("NMLS.X11");
      double num2 = this.FltVal("2");
      string str2 = this.Val("4458");
      if (str1 == "Loan purchased by your institution")
        this.SetCurrentNum("HMDA.X31", this.FltVal("ULDD.X1"));
      else if (string.IsNullOrEmpty(this.Val("4457")))
      {
        if ((string.IsNullOrEmpty(str1) || str1 == "Active Loan") && num2 != 0.0 || str1 == "Loan Originated" || str1 == "Application approved but not accepted" || str1 == "Preapproval request approved but not accepted")
          this.SetCurrentNum("HMDA.X31", num2);
        else
          this.SetCurrentNum("HMDA.X31", num1);
      }
      else
      {
        switch (str2)
        {
          case "Pending":
            if (!string.IsNullOrEmpty(str1) && !(str1 == "Active Loan") || num2 == 0.0)
              break;
            this.SetCurrentNum("HMDA.X31", num2);
            break;
          case "Declined":
          case "Withdrawn":
            if (!(str1 == "Application approved but not accepted") && !(str1 == "Application denied") && !(str1 == "Application withdrawn") && !(str1 == "File Closed for incompleteness") && !(str1 == "Preapproval request denied by financial institution") && !(str1 == "Preapproval request approved but not accepted"))
              break;
            this.SetCurrentNum("HMDA.X31", num2);
            break;
          case "Accepted":
            if (!(str1 == "Loan Originated") && !(str1 == "Application denied") && !(str1 == "Application withdrawn") && !(str1 == "File Closed for incompleteness"))
              break;
            this.SetCurrentNum("HMDA.X31", num2);
            break;
        }
      }
    }

    private void sortRaceCategoryWithPairs(string id, string val)
    {
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.sortRaceCategory(id, val);
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void sortRaceCategory(string id, string val)
    {
      Utils.ParseInt((object) id);
      if (id == "1524" || id == "1525" || id == "1526" || id == "1527" || id == "1528" || id == "1530" || id == "4143" || id == "4148" || id == "4149" || id == "4150" || id == "4151" || id == "4152" || id == "4153" || id == "4154" || id == "4155" || id == "4156" || id == "4157" || id == "4158" || id == "4244" || id == "1393")
      {
        int num = 4216;
        string val1 = "";
        this.borrRaceSortedList.Clear();
        switch (id)
        {
          case "1530":
            if (this.Val(id) == "Y")
            {
              this.borrRaceSortedList.Add(this.borrRaceList[1530], 1530);
              break;
            }
            break;
          case "4244":
            if (this.Val("4244") == "Y")
            {
              this.borrRaceSortedList.Add(this.borrRaceList[4244], 4244);
              break;
            }
            break;
          default:
            for (int key = 1524; key <= 4158; ++key)
            {
              if (this.Val(key.ToString()) == "Y")
                this.borrRaceSortedList.Add(this.borrRaceList[key], key);
              if (key == 1528)
                key = 4147;
            }
            break;
        }
        foreach (KeyValuePair<int, int> borrRaceSorted in this.borrRaceSortedList)
        {
          if (num <= 4220)
          {
            string val2 = borrRaceSorted.Key != 19 ? (borrRaceSorted.Key != 20 ? borrRaceSorted.Key.ToString() : "44") : "27";
            val1 = val1 + this.raceCodeDescriptionMapList[borrRaceSorted.Key] + ",";
            this.SetVal(num.ToString(), val2);
            ++num;
          }
          else
            break;
        }
        if (num <= 4220)
        {
          for (int index = num; index <= 4220; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val1.Length > 0 && val1.IndexOf(",") > 0)
          val1 = val1.Substring(0, val1.Length - 1);
        this.SetVal("4237", val1);
      }
      if (id == "1532" || id == "1533" || id == "1534" || id == "1535" || id == "1536" || id == "1538" || id == "4131" || id == "4163" || id == "4164" || id == "4165" || id == "4166" || id == "4167" || id == "4168" || id == "4169" || id == "4170" || id == "4171" || id == "4172" || id == "4173" || id == "4247" || id == "1393")
      {
        int num = 4226;
        string val3 = "";
        this.coBorrRaceSortedList.Clear();
        switch (id)
        {
          case "1538":
            if (this.Val(id) == "Y")
            {
              this.coBorrRaceSortedList.Add(this.coBorrRaceList[1538], 1538);
              break;
            }
            break;
          case "4247":
            if (this.Val("4247") == "Y")
            {
              this.coBorrRaceSortedList.Add(this.coBorrRaceList[4247], 4247);
              break;
            }
            break;
          default:
            for (int key = 1532; key <= 4173; ++key)
            {
              if (this.Val(key.ToString()) == "Y")
                this.coBorrRaceSortedList.Add(this.coBorrRaceList[key], key);
              if (key == 1536)
                key = 4162;
            }
            break;
        }
        foreach (KeyValuePair<int, int> coBorrRaceSorted in this.coBorrRaceSortedList)
        {
          if (num <= 4230)
          {
            string val4 = coBorrRaceSorted.Key != 19 ? (coBorrRaceSorted.Key != 20 ? coBorrRaceSorted.Key.ToString() : "44") : "27";
            val3 = val3 + this.raceCodeDescriptionMapList[coBorrRaceSorted.Key] + ",";
            this.SetVal(num.ToString(), val4);
            ++num;
          }
          else
            break;
        }
        if (num <= 4230)
        {
          for (int index = num; index <= 4230; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val3.Length > 0 && val3.IndexOf(",") > 0)
          val3 = val3.Substring(0, val3.Length - 1);
        this.SetVal("4239", val3);
      }
      if (id == "3840" || id == "3174")
      {
        for (int index = 4227; index <= 4230; ++index)
          this.SetVal(index.ToString(), "");
        this.coBorrRaceSortedList.Clear();
        if (id == "3840" ? this.Val("3840") == "Y" : this.Val("3174") == "Y")
        {
          this.SetVal("4226", "8");
          this.SetVal("4239", "No CoApplicant");
        }
        else
        {
          this.SetVal("4226", "");
          this.SetVal("4239", "");
        }
      }
      if (!(id == "IMPORT") && !(id == "4252") && !(id == "4253") && !(id == "1393") && !(id == "3840"))
        return;
      this.calculateRaceImportSorting(id, val);
    }

    private void calculateRaceImportSorting(string id, string val)
    {
      Utils.ParseInt((object) id);
      int num1 = 4216;
      string val1 = "";
      if (id == "4252" || id == "IMPORT" || id == "1393")
      {
        this.borrRaceSortedList.Clear();
        if (this.Val("1530") == "Y")
          this.borrRaceSortedList.Add(this.borrRaceList[1530], 1530);
        else if (this.Val("4244") == "Y")
        {
          this.borrRaceSortedList.Add(this.borrRaceList[4244], 4244);
        }
        else
        {
          for (int key = 1524; key <= 4158; ++key)
          {
            if (this.Val(key.ToString()) == "Y")
              this.borrRaceSortedList.Add(this.borrRaceList[key], key);
            if (key == 1528)
              key = 4147;
          }
        }
        foreach (KeyValuePair<int, int> borrRaceSorted in this.borrRaceSortedList)
        {
          if (num1 <= 4220)
          {
            string val2 = borrRaceSorted.Key != 19 ? (borrRaceSorted.Key != 20 ? borrRaceSorted.Key.ToString() : "44") : "27";
            val1 = val1 + this.raceCodeDescriptionMapList[borrRaceSorted.Key] + ",";
            this.SetVal(num1.ToString(), val2);
            ++num1;
          }
          else
            break;
        }
        if (num1 <= 4220)
        {
          for (int index = num1; index <= 4220; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val1.Length > 0 && val1.IndexOf(",") > 0)
          val1 = val1.Substring(0, val1.Length - 1);
        this.SetVal("4237", val1);
      }
      if (!(id == "4253") && !(id == "IMPORT") && !(id == "1393") && !(id == "3840"))
        return;
      int num2 = 4226;
      string val3 = "";
      this.coBorrRaceSortedList.Clear();
      if (this.Val("3174") == "Y")
      {
        for (int index = 4227; index <= 4230; ++index)
          this.SetVal(index.ToString(), "");
        this.SetVal("4226", "8");
        this.SetVal("4239", "No CoApplicant");
      }
      else
      {
        if (this.Val("1538") == "Y")
          this.coBorrRaceSortedList.Add(this.coBorrRaceList[1538], 1538);
        else if (this.Val("4247") == "Y")
        {
          this.coBorrRaceSortedList.Add(this.coBorrRaceList[4247], 4247);
        }
        else
        {
          for (int key = 1532; key <= 4173; ++key)
          {
            if (this.Val(key.ToString()) == "Y")
              this.coBorrRaceSortedList.Add(this.coBorrRaceList[key], key);
            if (key == 1536)
              key = 4162;
          }
        }
        foreach (KeyValuePair<int, int> coBorrRaceSorted in this.coBorrRaceSortedList)
        {
          if (num2 <= 4230)
          {
            string val4 = coBorrRaceSorted.Key != 19 ? (coBorrRaceSorted.Key != 20 ? coBorrRaceSorted.Key.ToString() : "44") : "27";
            val3 = val3 + this.raceCodeDescriptionMapList[coBorrRaceSorted.Key] + ",";
            this.SetVal(num2.ToString(), val4);
            ++num2;
          }
          else
            break;
        }
        if (num2 <= 4230)
        {
          for (int index = num2; index <= 4230; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val3.Length > 0 && val3.IndexOf(",") > 0)
          val3 = val3.Substring(0, val3.Length - 1);
        this.SetVal("4239", val3);
      }
    }

    private void calculateRaceImportSortingWithPairs(string id, string val)
    {
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.calculateRaceImportSorting(id, val);
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateHMDAReporting(string id, string val)
    {
    }

    private void sortEthnicityCategoryWithPairs(string id, string val)
    {
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.sortEthnicityCategory(id, val);
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void sortEthnicityCategory(string id, string val)
    {
      Utils.ParseInt((object) id);
      if (id == "4210" || id == "4211" || id == "4212" || id == "4243" || id == "4143" || id == "4144" || id == "4145" || id == "4146" || id == "4147" || id == "1393")
      {
        int num = 4221;
        this.borrEthnicitySortedList.Clear();
        switch (id)
        {
          case "4212":
            if (this.Val(id) == "Y")
            {
              this.borrEthnicitySortedList.Add(this.borrEthnicityList[4212], 4212);
              break;
            }
            break;
          case "4243":
            if (this.Val("4243") == "Y")
            {
              this.borrEthnicitySortedList.Add(this.borrEthnicityList[4243], 4243);
              break;
            }
            break;
          default:
            for (int key = 4144; key <= 4212; ++key)
            {
              if (this.Val(key.ToString()) == "Y")
                this.borrEthnicitySortedList.Add(this.borrEthnicityList[key], key);
              if (key == 4147)
                key = 4209;
            }
            break;
        }
        string val1 = "";
        foreach (KeyValuePair<int, int> borrEthnicitySorted in this.borrEthnicitySortedList)
        {
          if (num <= 4225)
          {
            val1 = val1 + this.ethnicityCodeDescriptionMapList[borrEthnicitySorted.Key] + ",";
            this.SetVal(num.ToString(), borrEthnicitySorted.Key == 10 ? "14" : borrEthnicitySorted.Key.ToString());
            ++num;
          }
          else
            break;
        }
        if (num <= 4225)
        {
          for (int index = num; index <= 4225; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val1.Length > 0 && val1.IndexOf(",") > 0)
          val1 = val1.Substring(0, val1.Length - 1);
        this.SetVal("4236", val1);
      }
      if (id == "4213" || id == "4214" || id == "4215" || id == "4246" || id == "4131" || id == "4159" || id == "4160" || id == "4161" || id == "4162" || id == "1393")
      {
        int num = 4231;
        string val2 = "";
        this.coBorrEthnicitySortedList.Clear();
        switch (id)
        {
          case "4215":
            if (this.Val(id) == "Y")
            {
              this.coBorrEthnicitySortedList.Add(this.coBorrEthnicityList[4215], 4215);
              break;
            }
            break;
          case "4246":
            if (this.Val("4246") == "Y")
            {
              this.coBorrEthnicitySortedList.Add(this.coBorrEthnicityList[4246], 4246);
              break;
            }
            break;
          default:
            for (int key = 4159; key <= 4215; ++key)
            {
              if (this.Val(key.ToString()) == "Y")
                this.coBorrEthnicitySortedList.Add(this.coBorrEthnicityList[key], key);
              if (key == 4162)
                key = 4212;
            }
            break;
        }
        foreach (KeyValuePair<int, int> borrEthnicitySorted in this.coBorrEthnicitySortedList)
        {
          if (num <= 4235)
          {
            val2 = val2 + this.ethnicityCodeDescriptionMapList[borrEthnicitySorted.Key] + ",";
            this.SetVal(num.ToString(), borrEthnicitySorted.Key == 10 ? "14" : borrEthnicitySorted.Key.ToString());
            ++num;
          }
          else
            break;
        }
        if (num <= 4235)
        {
          for (int index = num; index <= 4235; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val2.Length > 0 && val2.IndexOf(",") > 0)
          val2 = val2.Substring(0, val2.Length - 1);
        this.SetVal("4238", val2);
      }
      if (id == "3840" || id == "4188")
      {
        for (int index = 4232; index <= 4235; ++index)
          this.SetVal(index.ToString(), "");
        this.coBorrEthnicitySortedList.Clear();
        if (id == "3840" ? this.Val("3840") == "Y" : this.Val("4188") == "Y")
        {
          this.SetVal("4231", "5");
          this.SetVal("4238", "No CoApplicant");
        }
        else
        {
          this.SetVal("4231", "");
          this.SetVal("4238", "");
        }
      }
      if (!(id == "IMPORT") && !(id == "4205") && !(id == "4206") && !(id == "1393") && !(id == "3840"))
        return;
      this.calculateImportEthnicitySorting(id, val);
    }

    private void calculateImportEthnicitySorting(string id, string val)
    {
      Utils.ParseInt((object) id);
      int num1 = 4221;
      if (id == "4205" || id == "IMPORT" || id == "1393")
      {
        this.borrEthnicitySortedList.Clear();
        if (this.Val("4212") == "Y")
          this.borrEthnicitySortedList.Add(this.borrEthnicityList[4212], 4212);
        else if (this.Val("4243") == "Y")
        {
          this.borrEthnicitySortedList.Add(this.borrEthnicityList[4243], 4243);
        }
        else
        {
          for (int key = 4144; key <= 4212; ++key)
          {
            if (this.Val(key.ToString()) == "Y")
              this.borrEthnicitySortedList.Add(this.borrEthnicityList[key], key);
            if (key == 4147)
              key = 4209;
          }
        }
        string val1 = "";
        foreach (KeyValuePair<int, int> borrEthnicitySorted in this.borrEthnicitySortedList)
        {
          if (num1 <= 4225)
          {
            val1 = val1 + this.ethnicityCodeDescriptionMapList[borrEthnicitySorted.Key] + ",";
            this.SetVal(num1.ToString(), borrEthnicitySorted.Key == 10 ? "14" : borrEthnicitySorted.Key.ToString());
            ++num1;
          }
          else
            break;
        }
        if (num1 <= 4225)
        {
          for (int index = num1; index <= 4225; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val1.Length > 0 && val1.IndexOf(",") > 0)
          val1 = val1.Substring(0, val1.Length - 1);
        this.SetVal("4236", val1);
      }
      if (!(id == "4206") && !(id == "IMPORT") && !(id == "1393") && !(id == "3840"))
        return;
      int num2 = 4231;
      string val2 = "";
      this.coBorrEthnicitySortedList.Clear();
      if (this.Val("4188") == "Y")
      {
        for (int index = 4232; index <= 4235; ++index)
          this.SetVal(index.ToString(), "");
        this.SetVal("4231", "5");
        this.SetVal("4238", "No CoApplicant");
      }
      else
      {
        if (this.Val("4215") == "Y")
          this.coBorrEthnicitySortedList.Add(this.coBorrEthnicityList[4215], 4215);
        else if (this.Val("4246") == "Y")
        {
          this.coBorrEthnicitySortedList.Add(this.coBorrEthnicityList[4246], 4246);
        }
        else
        {
          for (int key = 4159; key <= 4215; ++key)
          {
            if (this.Val(key.ToString()) == "Y")
              this.coBorrEthnicitySortedList.Add(this.coBorrEthnicityList[key], key);
            if (key == 4162)
              key = 4212;
          }
        }
        foreach (KeyValuePair<int, int> borrEthnicitySorted in this.coBorrEthnicitySortedList)
        {
          if (num2 <= 4235)
          {
            val2 = val2 + this.ethnicityCodeDescriptionMapList[borrEthnicitySorted.Key] + ",";
            this.SetVal(num2.ToString(), borrEthnicitySorted.Key == 10 ? "14" : borrEthnicitySorted.Key.ToString());
            ++num2;
          }
          else
            break;
        }
        if (num2 <= 4235)
        {
          for (int index = num2; index <= 4235; ++index)
            this.SetVal(index.ToString(), "");
        }
        if (val2.Length > 0 && val2.IndexOf(",") > 0)
          val2 = val2.Substring(0, val2.Length - 1);
        this.SetVal("4238", val2);
      }
    }

    private void calculateImportEthnicitySortingWithPairs(string id, string val)
    {
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.calculateImportEthnicitySorting(id, val);
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateRepurchasedReportingYear(string id, string val)
    {
      DateTime date = Utils.ParseDate((object) this.Val("3312"));
      if (date == DateTime.MinValue)
        this.SetVal("HMDA.X92", string.Empty);
      else
        this.SetVal("HMDA.X92", date.Year.ToString());
    }

    private void calculateHmdaRepurchasedLoanAmount(string id, string val)
    {
      if (!(Utils.ParseDate((object) this.Val("3312")) != DateTime.MinValue))
        return;
      this.SetCurrentNum("HMDA.X93", this.FltVal("ULDD.X1"));
    }

    private void calculateRepurchasedTypeOfPurchaser(string id, string val)
    {
      if (!(Utils.ParseDate((object) this.Val("3312")) != DateTime.MinValue))
        return;
      this.SetVal("HMDA.X94", "Loan was not originated");
    }

    private void calculateRepurchasedActiontaken(string id, string val)
    {
      if (!(Utils.ParseDate((object) this.Val("3312")) != DateTime.MinValue))
        return;
      this.SetVal("HMDA.X95", "Loan purchased by your institution");
    }

    private void calculateHmdaRepurchasedActionDate(string id, string val)
    {
      if (!(Utils.ParseDate((object) this.Val("3312")) != DateTime.MinValue))
        return;
      this.SetVal("HMDA.X96", this.Val("3312"));
    }

    private void calculateHmdaRepurchasedActionDate2(string id, string val)
    {
      if (Utils.ParseDate((object) this.Val("HMDA.X96")) != DateTime.MinValue)
        this.SetVal("HMDA.X122", this.Val("HMDA.X96"));
      else
        this.SetVal("HMDA.X122", "");
    }

    private void calculateHmdaIncome(string id, string val)
    {
      bool flag = this.Val("4181") == "Y";
      string str = this.Val("1393");
      if (this.IntVal("HMDA.X27") <= 2017)
      {
        if (str != "Loan purchased by your institution" && this.Val("HMDA.X11") != "MultifamilyDwelling")
          this.SetVal("HMDA.X32", this.getHmdaIncome("1389", 12, 2017));
        else
          this.SetVal("HMDA.X32", "NA");
      }
      else if (flag || this.Val("HMDA.X99") == "Y")
        this.SetVal("HMDA.X32", "NA");
      else if (str != "Loan purchased by your institution")
      {
        this.SetVal("HMDA.X32", this.loan.Settings.HMDAInfo == null || this.loan.Settings.HMDAInfo.HMDAIncome == null ? "" : this.getHmdaIncome(this.loan.Settings.HMDAInfo.HMDAIncome, 12, 2018));
      }
      else
      {
        if (this.loan.Settings.HMDAInfo == null)
          return;
        this.SetVal("HMDA.X32", !this.loan.Settings.HMDAInfo.HMDAReportIncome || this.loan.Settings.HMDAInfo.HMDAIncome == null ? "NA" : this.getHmdaIncome(this.loan.Settings.HMDAInfo.HMDAIncome, 12, 2018));
      }
    }

    private void calculateHmdaBusinessOrCommercialPurpose(string id, string val)
    {
      this.SetVal("HMDA.X58", this.Val("QM.X110") == "Y" ? "Primarily for a business or commercial purpose" : "Not primarily for a business or commercial purpose");
    }

    private void calculateHmdaTotalLoanCosts(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
        return;
      string str = this.Val("1393");
      if (this.Val("HMDA.X56") != "Reverse mortgage" && this.Val("HMDA.X57") != "Open-end line of credit" && this.Val("HMDA.X58") != "Primarily for a business or commercial purpose" && this.Val("QM.X103") != "Y" && (string.IsNullOrEmpty(str) || str == "Loan Originated" || str == "Loan purchased by your institution") && this.UseNew2015GFEHUD)
        this.SetVal("HMDA.X77", this.FltVal("CD2.XSTD") == 0.0 ? "0" : this.Val("CD2.XSTD"));
      else
        this.SetVal("HMDA.X77", "NA");
    }

    private void calculateHmdaTotalPointsAndFees(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
        return;
      string str = this.Val("1393");
      if (this.Val("HMDA.X56") != "Reverse mortgage" && this.Val("HMDA.X57") != "Open-end line of credit" && this.Val("HMDA.X58") != "Primarily for a business or commercial purpose" && this.Val("QM.X103") != "Y" && !this.UseNew2015GFEHUD && (string.IsNullOrEmpty(str) || str == "Loan Originated"))
        this.SetVal("HMDA.X78", this.FltVal("S32DISC.X48") == 0.0 ? "0" : this.Val("S32DISC.X48"));
      else
        this.SetVal("HMDA.X78", "NA");
    }

    private void calculateHmdaOriginationCharges(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
        return;
      string str = this.Val("1393");
      if (this.Val("HMDA.X56") != "Reverse mortgage" && this.Val("HMDA.X57") != "Open-end line of credit" && this.Val("HMDA.X58") != "Primarily for a business or commercial purpose" && (string.IsNullOrEmpty(str) || str == "Loan Originated" || str == "Loan purchased by your institution") && this.Val("HMDA.X121") == "Y")
      {
        if (this.UseNew2015GFEHUD)
          this.SetVal("HMDA.X79", this.FltVal("CD2.XSTA") == 0.0 ? "0" : this.Val("CD2.XSTA"));
        else
          this.SetVal("HMDA.X79", this.FltVal("NEWHUD.X796") == 0.0 ? "0" : this.Val("NEWHUD.X796"));
      }
      else
        this.SetVal("HMDA.X79", "NA");
    }

    private void calculateHmdaDiscountPoints(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
        return;
      string str1 = this.Val("1393");
      string str2 = this.Val("HMDA.X121");
      string str3 = this.Val("HMDA.X56");
      string str4 = this.Val("HMDA.X57");
      string str5 = this.Val("HMDA.X58");
      if (str2 == "N" || !string.IsNullOrEmpty(str1) && str1 != "Active Loan" && str1 != "Loan purchased by your institution" && str1 != "Loan Originated" || str3 == "Reverse mortgage" || str4 == "Open-end line of credit" || str5 == "Primarily for a business or commercial purpose")
        this.SetVal("HMDA.X35", "NA");
      else if (this.UseNew2015GFEHUD)
      {
        if (this.FltVal("NEWHUD2.X927") == 0.0 || string.IsNullOrEmpty(this.Val("NEWHUD2.X927")))
          this.SetVal("HMDA.X35", "");
        else
          this.SetVal("HMDA.X35", this.Val("NEWHUD2.X927"));
      }
      else
      {
        if (!this.UseNewGFEHUD)
          return;
        if (this.FltVal("1093") == 0.0 || string.IsNullOrEmpty(this.Val("1093")))
          this.SetVal("HMDA.X35", "");
        else
          this.SetVal("HMDA.X35", this.Val("1093"));
      }
    }

    private void calculateHmdaCDRequired(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
        return;
      if (this.Val("HMDA.X56") == "Reverse mortgage" || this.Val("HMDA.X57") == "Open-end line of credit" || this.Val("HMDA.X58") == "Primarily for a business or commercial purpose")
        this.SetVal("HMDA.X121", "N");
      else
        this.SetVal("HMDA.X121", "Y");
    }

    private void calculateHmdaLenderCredits(string id, string val)
    {
      string str1 = this.Val("1393");
      string str2 = this.Val("3969");
      bool flag = string.IsNullOrEmpty(str1) || str1 == "Loan Originated" || str1 == "Loan purchased by your institution";
      if (this.Val("HMDA.X56") == "Reverse mortgage" || this.Val("HMDA.X57") == "Open-end line of credit" || this.Val("HMDA.X58") == "Primarily for a business or commercial purpose" || this.Val("HMDA.X121") == "N" || str2 == "RESPA 2010 GFE and HUD-1" || str2 == "Old GFE and HUD-1" || !flag)
      {
        this.SetVal("HMDA.X80", "NA");
      }
      else
      {
        double num = this.FltVal("CD2.XSTLC");
        this.SetVal("HMDA.X80", num == 0.0 ? "" : num.ToString());
      }
    }

    private void calculateHmdaInterestRate(string id, string val)
    {
      string str1 = this.Val("1393");
      if (str1 == "Loan Originated" || str1 == "Application approved but not accepted" || str1 == "Loan purchased by your institution" || str1 == "Preapproval request approved but not accepted")
      {
        string str2 = this.Val("1172");
        string val1 = "";
        if (str2 == "HELOC")
          val1 = this.Val("1482");
        if (val1 != "")
          this.SetVal("HMDA.X81", val1);
        else
          this.SetVal("HMDA.X81", this.Val("3"));
      }
      else
      {
        if (string.IsNullOrEmpty(str1) || !(str1 != "Active Loan"))
          return;
        this.SetVal("HMDA.X81", "NA");
      }
    }

    private void calculateHmdaPrepaymentPenaltyPeriod(string id, string val)
    {
      string str1 = this.Val("1393");
      bool flag1 = this.Val("HMDA.X56") == "Reverse mortgage";
      string str2 = this.Val("RE88395.X316");
      bool flag2 = this.Val("HMDA.X58") == "Primarily for a business or commercial purpose";
      if (!string.IsNullOrEmpty(str2) && str1 != "Loan purchased by your institution" && !flag1 && !flag2)
      {
        this.SetVal("HMDA.X82", this.Val("RE88395.X316"));
      }
      else
      {
        if (((string.IsNullOrEmpty(str2) ? 1 : (str1 == "Loan purchased by your institution" ? 1 : 0)) | (flag1 ? 1 : 0) | (flag2 ? 1 : 0)) == 0)
          return;
        this.SetVal("HMDA.X82", "NA");
      }
    }

    private void calculateHmdaLoanTerm(string id, string val)
    {
      if (this.Val("HMDA.X56") == "Reverse mortgage")
        this.SetVal("HMDA.X83", "NA");
      else if (this.Val("423") == "Biweekly")
      {
        string str1 = this.Val("LE1.X2");
        string str2 = this.Val("LE1.X3");
        if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
          this.SetVal("HMDA.X83", "");
        else
          this.SetVal("HMDA.X83", (Utils.ParseInt((object) str1, 0) * 12 + Utils.ParseInt((object) str2, 0)).ToString());
      }
      else if (this.Val("19") == "ConstructionToPermanent" && this.Val("CONST.X1") != "Y")
      {
        string str3 = this.Val("325");
        string str4 = this.Val("1176");
        if (string.IsNullOrEmpty(str3) && string.IsNullOrEmpty(str4))
          this.SetVal("HMDA.X83", string.Empty);
        else
          this.SetVal("HMDA.X83", (Utils.ParseInt((object) str3, 0) + Utils.ParseInt((object) str4, 0)).ToString());
      }
      else
        this.SetVal("HMDA.X83", this.Val("325"));
    }

    private void calculateHmdaIntroRatePeriod(string id, string val)
    {
      string str1 = this.Val("4492");
      string str2 = this.Val("1172");
      string str3 = this.Val("608");
      bool flag1 = str3 == "Fixed";
      bool flag2 = str3 == "AdjustableRate";
      string str4 = this.Val("696");
      bool flag3 = string.IsNullOrEmpty(str4);
      int num = this.IntVal("1176");
      string str5 = this.Val("19");
      string val1 = (string) null;
      if (str5 == "ConstructionToPermanent")
      {
        if (this.Val("1677") == this.Val("3"))
        {
          if (flag2)
            val1 = (num + Utils.ParseInt((object) str4, 0)).ToString();
          else if (flag1)
            val1 = "NA";
        }
        else
          val1 = num.ToString();
      }
      else if (str2 == "HELOC")
      {
        if (string.IsNullOrEmpty(str1))
        {
          if (flag1)
            val1 = "NA";
          else if (flag2)
            val1 = !flag3 ? str4 : "";
        }
        else if (flag2 | flag1)
          val1 = str1;
      }
      else if (flag2)
        val1 = str4;
      else if (flag1)
        val1 = "NA";
      if (val1 == null)
        return;
      this.SetVal("HMDA.X84", val1);
    }

    private void calculateReasonForDenial(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
      {
        if (!(this.Val("HMDA.X21") == "Not applicable"))
          return;
        this.SetVal("HMDA.X21", "");
      }
      else
      {
        if (this.IsHmdaActionValid("HMDA.X21"))
          this.SetVal("HMDA.X21", "Not applicable");
        if (!(this.Val("HMDA.X21") != "Other") || !(this.Val("HMDA.X22") != "Other") || !(this.Val("HMDA.X23") != "Other") || !(this.Val("HMDA.X33") != "Other"))
          return;
        this.SetVal("HMDA.X34", "");
      }
    }

    private void calculateNocoapplicant(string id, string val)
    {
      switch (id)
      {
        case "478":
          if (val == "No co-applicant")
          {
            this.SetVal("4197", "N");
            this.SetVal("4198", "N");
            this.SetVal("4199", "N");
            this.SetVal("4200", "N");
            this.SetVal("4189", "Y");
            this.SetVal("4248", "N");
            break;
          }
          this.SetVal("4189", "");
          break;
        case "1531":
          if (val == "No co-applicant")
          {
            this.SetVal("4188", "Y");
            this.SetVal("4213", "N");
            this.SetVal("4214", "N");
            this.SetVal("4215", "N");
            this.SetVal("4206", "N");
            this.SetVal("4246", "N");
            this.SetVal("4136", "");
            for (int index = 4159; index <= 4162; ++index)
              this.SetVal(string.Concat((object) index), "");
            break;
          }
          this.SetVal("4188", "");
          break;
        case "4189":
          if (val == "Y")
          {
            this.SetVal("478", "No co-applicant");
            this.SetVal("4197", "N");
            this.SetVal("4198", "N");
            this.SetVal("4199", "N");
            this.SetVal("4200", "N");
            this.SetVal("4248", "N");
            this.SetVal("188", "N");
            break;
          }
          if (!(this.Val("478") == "No co-applicant"))
            break;
          this.SetVal("478", "");
          break;
        case "4188":
          if (val == "Y")
          {
            this.SetVal("1531", "No co-applicant");
            for (int index = 4159; index <= 4162; ++index)
              this.SetVal(string.Concat((object) index), "");
            this.SetVal("4136", "");
            this.SetVal("4213", "N");
            this.SetVal("4214", "N");
            this.SetVal("4215", "N");
            this.SetVal("4206", "N");
            this.SetVal("4246", "N");
            this.SetVal("188", "N");
            break;
          }
          if (!(this.Val("1531") == "No co-applicant"))
            break;
          this.SetVal("1531", "");
          break;
        case "3174":
          if (!(val == "Y"))
            break;
          for (int index = 1532; index <= 1538; ++index)
            this.SetVal(string.Concat((object) index), "");
          for (int index = 4163; index <= 4173; ++index)
            this.SetVal(string.Concat((object) index), "");
          this.SetVal("4139", "");
          this.SetVal("4141", "");
          this.SetVal("4137", "");
          this.SetVal("4247", "");
          this.SetVal("4253", "");
          this.SetVal("188", "N");
          break;
      }
    }

    private void calculateGender(string id, string val)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 1662811788:
          if (!(id == "4199"))
            return;
          goto label_66;
        case 1679589407:
          if (!(id == "4198"))
            return;
          goto label_66;
        case 1729922264:
          if (!(id == "4195"))
            return;
          goto label_39;
        case 1746699883:
          if (!(id == "4194"))
            return;
          goto label_39;
        case 1763477502:
          if (!(id == "4197"))
            return;
          goto label_66;
        case 1780255121:
          if (!(id == "4196"))
            return;
          goto label_39;
        case 1786322784:
          if (!(id == "4245"))
            return;
          goto label_39;
        case 1830587978:
          if (!(id == "4193"))
            return;
          goto label_39;
        case 2004431831:
          if (!(id == "4248"))
            return;
          goto label_66;
        case 2821866572:
          if (!(id == "478"))
            return;
          break;
        case 2972865143:
          if (!(id == "471"))
            return;
          break;
        case 4151275515:
          if (!(id == "4200"))
            return;
          goto label_66;
        default:
          return;
      }
      if (id == "471")
      {
        switch (val)
        {
          case "Female":
            this.SetVal("4193", "Y");
            this.SetVal("4194", "");
            this.SetVal("4196", "");
            this.SetVal("4245", "");
            return;
          case "Male":
            this.SetVal("4193", "");
            this.SetVal("4194", "Y");
            this.SetVal("4196", "");
            this.SetVal("4245", "");
            return;
          case "NotApplicable":
            this.SetVal("4193", "");
            this.SetVal("4194", "");
            this.SetVal("4195", "");
            this.SetVal("4196", "Y");
            this.SetVal("4245", "");
            return;
          case "InformationNotProvidedUnknown":
            this.SetVal("4193", "");
            this.SetVal("4194", "");
            this.SetVal("4196", "");
            this.SetVal("4245", "Y");
            return;
          default:
            this.SetVal("4193", "");
            this.SetVal("4194", "");
            this.SetVal("4196", "");
            this.SetVal("4245", "");
            return;
        }
      }
      else
      {
        switch (val)
        {
          case "Female":
            this.SetVal("4197", "Y");
            this.SetVal("4198", "");
            this.SetVal("4200", "");
            this.SetVal("4189", "");
            this.SetVal("4248", "");
            return;
          case "Male":
            this.SetVal("4197", "");
            this.SetVal("4198", "Y");
            this.SetVal("4200", "");
            this.SetVal("4189", "");
            this.SetVal("4248", "");
            return;
          case "NotApplicable":
            this.SetVal("4197", "");
            this.SetVal("4198", "");
            this.SetVal("4199", "");
            this.SetVal("4200", "Y");
            this.SetVal("4189", "");
            this.SetVal("4248", "");
            return;
          case "InformationNotProvidedUnknown":
            this.SetVal("4197", "");
            this.SetVal("4198", "");
            this.SetVal("4200", "");
            this.SetVal("4189", "");
            this.SetVal("4248", "Y");
            return;
          default:
            this.SetVal("4197", "");
            this.SetVal("4198", "");
            this.SetVal("4200", "");
            this.SetVal("4248", "");
            return;
        }
      }
label_39:
      if (id == "4245")
      {
        if (val == "Y")
        {
          this.SetVal("4193", "");
          this.SetVal("4194", "");
          this.SetVal("4196", "");
          this.SetVal("471", "InformationNotProvidedUnknown");
          return;
        }
        if (!(this.Val("471") == "InformationNotProvidedUnknown"))
          return;
        this.SetVal("471", "");
        return;
      }
      if (id == "4195")
      {
        if (!(this.Val("4195") == "Y"))
          return;
        this.SetVal("4196", "N");
        return;
      }
      int num1 = 0;
      string str1 = "";
      bool flag1 = false;
      Dictionary<int, string> dictionary1 = new Dictionary<int, string>()
      {
        {
          4193,
          this.Val("4193")
        },
        {
          4194,
          this.Val("4194")
        }
      };
      foreach (KeyValuePair<int, string> keyValuePair in dictionary1)
      {
        if (keyValuePair.Value == "Y")
        {
          this.SetVal("4196", "N");
          flag1 = true;
          break;
        }
      }
      if (flag1 || this.Val("4196") == "Y")
        this.SetVal("4245", "N");
      for (int key = 4193; key <= 4196; ++key)
      {
        if ((key == 4196 ? this.Val(key.ToString()) : (dictionary1.ContainsKey(key) ? dictionary1[key] : "N")) == "Y")
        {
          ++num1;
          if (num1 <= 1)
          {
            switch (key)
            {
              case 4193:
                str1 = "Female";
                continue;
              case 4194:
                str1 = "Male";
                continue;
              case 4196:
                str1 = "NotApplicable";
                continue;
              default:
                continue;
            }
          }
          else
            break;
        }
      }
      this.SetVal("471", num1 == 1 ? str1 : "");
      return;
label_66:
      if (id == "4248")
      {
        if (val == "Y")
        {
          this.SetVal("4197", "");
          this.SetVal("4198", "");
          this.SetVal("4200", "");
          this.SetVal("4189", "");
          this.SetVal("478", "InformationNotProvidedUnknown");
        }
        else
        {
          if (!(this.Val("478") == "InformationNotProvidedUnknown"))
            return;
          this.SetVal("478", "");
        }
      }
      else if (id == "4199")
      {
        if (!(this.Val("4199") == "Y"))
          return;
        this.SetVal("4189", "N");
        this.SetVal("4200", "N");
      }
      else
      {
        int num2 = 0;
        string str2 = "";
        bool flag2 = false;
        Dictionary<int, string> dictionary2 = new Dictionary<int, string>()
        {
          {
            4197,
            this.Val("4197")
          },
          {
            4198,
            this.Val("4198")
          }
        };
        foreach (KeyValuePair<int, string> keyValuePair in dictionary2)
        {
          if (keyValuePair.Value == "Y")
          {
            this.SetVal("4200", "N");
            flag2 = true;
            break;
          }
        }
        if (flag2 || this.Val("4200") == "Y")
        {
          this.SetVal("4189", "N");
          this.SetVal("4248", "N");
        }
        for (int key = 4197; key <= 4200; ++key)
        {
          if ((key == 4200 ? this.Val(key.ToString()) : (dictionary2.ContainsKey(key) ? dictionary2[key] : "N")) == "Y")
          {
            ++num2;
            if (num2 <= 1)
            {
              switch (key)
              {
                case 4197:
                  str2 = "Female";
                  continue;
                case 4198:
                  str2 = "Male";
                  continue;
                case 4200:
                  str2 = "NotApplicable";
                  continue;
                default:
                  continue;
              }
            }
            else
              break;
          }
        }
        this.SetVal("478", num2 == 1 ? str2 : "");
      }
    }

    private void calculateEthnicity(string id, string val)
    {
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 1375179504:
          if (!(id == "4160"))
            return;
          goto label_111;
        case 1391957123:
          if (!(id == "4161"))
            return;
          goto label_111;
        case 1408734742:
          if (!(id == "4162"))
            return;
          goto label_111;
        case 1835816998:
          if (!(id == "4214"))
            return;
          goto label_84;
        case 1836655641:
          if (!(id == "4246"))
            return;
          goto label_84;
        case 1852594617:
          if (!(id == "4215"))
            return;
          goto label_84;
        case 1869372236:
          if (!(id == "4212"))
            return;
          goto label_55;
        case 1886149855:
          if (!(id == "4213"))
            return;
          goto label_84;
        case 1886988498:
          if (!(id == "4243"))
            return;
          goto label_55;
        case 1902927474:
          if (!(id == "4210"))
            return;
          goto label_55;
        case 1919705093:
          if (!(id == "4211"))
            return;
          goto label_55;
        case 3571192027:
          if (!(id == "1531"))
            return;
          break;
        case 3604894360:
          if (!(id == "1523"))
            return;
          break;
        case 3623674640:
          if (!(id == "4146"))
            return;
          goto label_82;
        case 3640452259:
          if (!(id == "4147"))
            return;
          goto label_82;
        case 3657229878:
          if (!(id == "4144"))
            return;
          goto label_82;
        case 3674007497:
          if (!(id == "4145"))
            return;
          goto label_82;
        case 3674154592:
          if (!(id == "4159"))
            return;
          goto label_111;
        case 4201608372:
          if (!(id == "4205"))
            return;
          goto label_55;
        case 4251941229:
          if (!(id == "4206"))
            return;
          goto label_84;
        default:
          return;
      }
      if (id == "1523")
      {
        switch (val)
        {
          case "Hispanic or Latino":
            this.SetVal("4210", "Y");
            this.SetVal("4211", "");
            this.SetVal("4212", "");
            this.SetVal("4243", "");
            return;
          case "Not Hispanic or Latino":
            this.SetVal("4210", "");
            this.SetVal("4211", "Y");
            this.SetVal("4212", "");
            this.SetVal("4243", "");
            return;
          case "Information not provided":
            this.SetVal("4210", "");
            this.SetVal("4211", "");
            this.SetVal("4212", "");
            this.SetVal("4243", "Y");
            return;
          case "Not applicable":
            this.SetVal("4210", "");
            this.SetVal("4205", "");
            this.SetVal("4211", "");
            this.SetVal("4212", "Y");
            this.SetVal("4243", "");
            return;
          default:
            this.SetVal("4210", "");
            this.SetVal("4211", "");
            this.SetVal("4212", "");
            this.SetVal("4243", "");
            return;
        }
      }
      else
      {
        switch (val)
        {
          case "Hispanic or Latino":
            this.SetVal("4213", "Y");
            this.SetVal("4214", "");
            this.SetVal("4215", "");
            this.SetVal("4246", "");
            this.SetVal("4188", "");
            return;
          case "Not Hispanic or Latino":
            this.SetVal("4213", "");
            this.SetVal("4214", "Y");
            this.SetVal("4215", "");
            this.SetVal("4246", "");
            this.SetVal("4188", "");
            return;
          case "Information not provided":
            this.SetVal("4213", "");
            this.SetVal("4214", "");
            this.SetVal("4215", "");
            this.SetVal("4246", "Y");
            this.SetVal("4188", "");
            return;
          case "Not applicable":
            this.SetVal("4213", "");
            this.SetVal("4206", "");
            this.SetVal("4214", "");
            this.SetVal("4215", "Y");
            this.SetVal("4246", "");
            this.SetVal("4188", "");
            return;
          default:
            this.SetVal("4213", "");
            this.SetVal("4214", "");
            this.SetVal("4215", "");
            this.SetVal("4246", "");
            return;
        }
      }
label_55:
      if (id == "4243")
      {
        if (this.Val("4243") == "Y")
        {
          this.SetVal("4210", "");
          this.SetVal("4211", "");
          this.SetVal("4212", "");
          this.SetVal("4144", "");
          this.SetVal("4145", "");
          this.SetVal("4146", "");
          this.SetVal("4147", "");
          this.SetVal("4125", "");
          this.SetVal("1523", "Information not provided");
          return;
        }
        if (!(this.Val("1523") == "Information not provided"))
          return;
        this.SetVal("1523", "");
        return;
      }
      if (id == "4205")
      {
        if (!(this.Val("4205") == "Y"))
          return;
        this.SetVal("4212", "");
        return;
      }
      int num1 = 0;
      string str1 = "";
      bool flag1 = false;
      Dictionary<int, string> dictionary1 = new Dictionary<int, string>()
      {
        {
          4210,
          this.Val("4210")
        },
        {
          4211,
          this.Val("4211")
        }
      };
      foreach (KeyValuePair<int, string> keyValuePair in dictionary1)
      {
        if (keyValuePair.Value == "Y")
        {
          this.SetVal("4212", "N");
          flag1 = true;
          break;
        }
      }
      if (flag1 || this.Val("4212") == "Y")
        this.SetVal("4243", "N");
      for (int key = 4210; key <= 4212; ++key)
      {
        if ((key == 4212 ? this.Val(key.ToString()) : (dictionary1.ContainsKey(key) ? dictionary1[key] : "N")) == "Y")
        {
          ++num1;
          if (num1 <= 1)
          {
            switch (key)
            {
              case 4210:
                str1 = "Hispanic or Latino";
                continue;
              case 4211:
                str1 = "Not Hispanic or Latino";
                continue;
              case 4212:
                str1 = "Not applicable";
                continue;
              default:
                continue;
            }
          }
          else
            break;
        }
      }
      this.SetVal("1523", num1 == 1 ? str1 : "");
      return;
label_82:
      if (!(val == "Y"))
        return;
      this.SetVal("4212", "N");
      this.SetVal("4243", "N");
      return;
label_84:
      if (id == "4246")
      {
        if (this.Val("4246") == "Y")
        {
          this.SetVal("4213", "");
          this.SetVal("4214", "");
          this.SetVal("4215", "");
          this.SetVal("4188", "");
          this.SetVal("4159", "");
          this.SetVal("4160", "");
          this.SetVal("4161", "");
          this.SetVal("4162", "");
          this.SetVal("4136", "");
          this.SetVal("1531", "Information not provided");
          return;
        }
        if (!(this.Val("1531") == "Information not provided"))
          return;
        this.SetVal("1531", "");
        return;
      }
      if (id == "4206")
      {
        if (!(this.Val("4206") == "Y"))
          return;
        this.SetVal("4215", "");
        this.SetVal("4188", "");
        return;
      }
      int num2 = 0;
      string str2 = "";
      bool flag2 = false;
      Dictionary<int, string> dictionary2 = new Dictionary<int, string>()
      {
        {
          4213,
          this.Val("4213")
        },
        {
          4214,
          this.Val("4214")
        }
      };
      foreach (KeyValuePair<int, string> keyValuePair in dictionary2)
      {
        if (keyValuePair.Value == "Y")
        {
          this.SetVal("4215", "N");
          flag2 = true;
          break;
        }
      }
      if (flag2 || this.Val("4215") == "Y")
      {
        this.SetVal("4188", "N");
        this.SetVal("4246", "N");
      }
      for (int key = 4213; key <= 4215; ++key)
      {
        if ((key == 4215 ? this.Val(key.ToString()) : (dictionary2.ContainsKey(key) ? dictionary2[key] : "N")) == "Y")
        {
          ++num2;
          if (num2 <= 1)
          {
            switch (key)
            {
              case 4213:
                str2 = "Hispanic or Latino";
                continue;
              case 4214:
                str2 = "Not Hispanic or Latino";
                continue;
              case 4215:
                str2 = "Not applicable";
                continue;
              default:
                continue;
            }
          }
          else
            break;
        }
      }
      this.SetVal("1531", num2 == 1 ? str2 : "");
      return;
label_111:
      if (!(val == "Y"))
        return;
      this.SetVal("4215", "N");
      this.SetVal("4188", "N");
      this.SetVal("4246", "N");
    }

    private void calculateDoNotWish(string id, string val)
    {
      if (id == "4205" || id == "4143")
      {
        if (this.Val("4205") == "Y" && this.Val("4143") == "FaceToFace")
        {
          for (int index = 4144; index <= 4147; ++index)
            this.SetVal(index.ToString(), "");
          this.SetVal("4125", "");
        }
      }
      else if ((id == "4206" || id == "4131") && this.Val("4206") == "Y" && this.Val("4131") == "FaceToFace")
      {
        for (int index = 4159; index <= 4162; ++index)
          this.SetVal(index.ToString(), "");
        this.SetVal("4136", "");
      }
      if (id == "4252" || id == "4143")
      {
        if (this.Val("4252") == "Y" && this.Val("4143") == "FaceToFace")
        {
          for (int index = 4148; index <= 4158; ++index)
            this.SetVal(index.ToString(), "");
          this.SetVal("4126", "");
          this.SetVal("4128", "");
          this.SetVal("4130", "");
        }
      }
      else if ((id == "4253" || id == "4131") && this.Val("4253") == "Y" && this.Val("4131") == "FaceToFace")
      {
        for (int index = 4163; index <= 4173; ++index)
          this.SetVal(index.ToString(), "");
        this.SetVal("4137", "");
        this.SetVal("4141", "");
        this.SetVal("4139", "");
      }
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(id))
      {
        case 1662811788:
          if (!(id == "4199"))
            return;
          goto label_44;
        case 1729922264:
          if (!(id == "4195"))
            return;
          break;
        case 1735842832:
          if (!(id == "4252"))
            return;
          break;
        case 1752620451:
          if (!(id == "4253"))
            return;
          goto label_44;
        case 4011823100:
          if (!(id == "188"))
            return;
          if (val == "Y")
          {
            this.SetVal("4205", "Y");
            this.SetVal("4252", "Y");
            this.SetVal("4195", "Y");
            this.SetVal("4212", "");
            this.SetVal("1530", "");
            this.SetVal("4196", "");
            return;
          }
          if (this.Val("4205") == "Y")
            this.SetVal("4205", "");
          if (this.Val("4252") == "Y")
            this.SetVal("4252", "");
          if (!(this.Val("4195") == "Y"))
            return;
          this.SetVal("4195", "");
          return;
        case 4028600719:
          if (!(id == "189"))
            return;
          if (val == "Y")
          {
            this.SetVal("4206", "Y");
            this.SetVal("4253", "Y");
            this.SetVal("4199", "Y");
            this.SetVal("4215", "");
            this.SetVal("4188", "");
            this.SetVal("1538", "");
            this.SetVal("3174", "");
            this.SetVal("4200", "");
            this.SetVal("4189", "");
            return;
          }
          if (this.Val("4206") == "Y")
            this.SetVal("4206", "");
          if (this.Val("4253") == "Y")
            this.SetVal("4253", "");
          if (!(this.Val("4199") == "Y"))
            return;
          this.SetVal("4199", "");
          return;
        case 4201608372:
          if (!(id == "4205"))
            return;
          break;
        case 4251941229:
          if (!(id == "4206"))
            return;
          goto label_44;
        default:
          return;
      }
      if (this.Val("4205") == "Y" && this.Val("4252") == "Y" && this.Val("4195") == "Y")
      {
        this.SetVal("188", "Y");
        return;
      }
      if (!(this.Val("188") == "Y"))
        return;
      this.SetVal("188", "");
      return;
label_44:
      if (this.Val("4206") == "Y" && this.Val("4253") == "Y" && this.Val("4199") == "Y")
      {
        this.SetVal("189", "Y");
      }
      else
      {
        if (!(this.Val("189") == "Y"))
          return;
        this.SetVal("189", "");
      }
    }

    private void calculateHmdaManufacturedProperty(string id, string val)
    {
      if (this.IsHmdaActionValid("HMDA.X39"))
        this.SetVal("HMDA.X39", "Not applicable");
      if (!this.IsHmdaActionValid("HMDA.X40"))
        return;
      this.SetVal("HMDA.X40", "Not applicable");
    }

    public bool IsHmdaActionValid(string id)
    {
      bool flag = false;
      string str = this.Val("1393");
      switch (id)
      {
        case "HMDA.X21":
          if (str == "Loan Originated" || str == "Application approved but not accepted" || str == "Application withdrawn" || str == "File Closed for incompleteness" || str == "Loan purchased by your institution" || str == "Preapproval request approved but not accepted")
          {
            flag = true;
            break;
          }
          break;
        case "HMDA.X39":
        case "HMDA.X40":
          if (this.Val("ULDD.X172") == "Site Built")
          {
            flag = true;
            break;
          }
          break;
        case "HMDA.X42":
        case "HMDA.X44":
        case "HMDA.X50":
          if (str == "Loan purchased by your institution")
          {
            flag = true;
            break;
          }
          break;
        case "HMDA.X43":
          if (str == "Application denied" || str == "Application withdrawn" || str == "File Closed for incompleteness" || str == "Loan purchased by your institution" || str == "Preapproval request denied by financial institution")
          {
            flag = true;
            break;
          }
          break;
      }
      return flag;
    }

    private void calculateHmdaRiskAssessment(string id, string val)
    {
      this.Val("1393");
      if (this.IsHmdaActionValid("HMDA.X44"))
      {
        this.SetVal("HMDA.X44", "Not applicable");
        this.SetVal("HMDA.X50", "Not applicable");
      }
      if (!(this.Val("HMDA.X50") != "Other") || !(this.Val("HMDA.X51") != "Other") || !(this.Val("HMDA.X52") != "Other") || !(this.Val("HMDA.X53") != "Other") || !(this.Val("HMDA.X54") != "Other"))
        return;
      this.SetVal("HMDA.X55", "");
    }

    internal void AddInitialApplicationDateTrigger(string hmdaApplicationDateID)
    {
      if (!(hmdaApplicationDateID != ""))
        return;
      this.hmdaApplicationDateID = hmdaApplicationDateID;
      this.AddFieldHandler(hmdaApplicationDateID, new Routine(this.calculateHmdaApplicationDate) + this.calObjs.D1003Cal.CopyCitizenshipAndAge + this.calObjs.Cal.CalcVAAccount + new Routine(this.calculateHmdaAge));
    }

    internal void AddInitialHMDAIncomeTrigger(string hmdaIncomeID)
    {
      if (!(hmdaIncomeID != ""))
        return;
      this.AddFieldHandler(hmdaIncomeID, new Routine(this.calculateHmdaIncome));
    }

    private void calculateHmdaApplicationDate(string id, string val)
    {
      if (this.Val("1393") != "Loan purchased by your institution")
      {
        if (!(id == this.hmdaApplicationDateID) && !(this.Val("HMDA.X29") == "") && (!(id == "1393") || !(this.Val("HMDA.X29") == "NA")))
          return;
        this.SetVal("HMDA.X29", Utils.ReformatEuropeanDate(this.Val(this.hmdaApplicationDateID)));
      }
      else
        this.SetVal("HMDA.X29", "NA");
    }

    private void updateHMDATransmittalSheetDetails(string id, string val)
    {
      if (this.loan == null || this.loan.Settings == null || this.loan.Settings.HMDAInfo == null)
        return;
      this.SetVal("HMDA.X71", this.loan.Settings.HMDAInfo.HMDARespondentID);
      this.SetVal("HMDA.X69", this.loan.Settings.HMDAInfo.HMDARespondentTaxID);
      switch (this.loan.Settings.HMDAInfo.HMDARespondentAgency)
      {
        case "1":
          this.SetVal("HMDA.X68", "OCC");
          break;
        case "2":
          this.SetVal("HMDA.X68", "FRS");
          break;
        case "3":
          this.SetVal("HMDA.X68", "FDIC");
          break;
        case "5":
          this.SetVal("HMDA.X68", "NCUA");
          break;
        case "7":
          this.SetVal("HMDA.X68", "HUD");
          break;
        case "9":
          this.SetVal("HMDA.X68", "CFPB");
          break;
        default:
          this.SetVal("HMDA.X68", "");
          break;
      }
      this.SetVal("HMDA.X70", this.loan.Settings.HMDAInfo.HMDALEI);
      this.SetVal("HMDA.X59", this.loan.Settings.HMDAInfo.HMDACompanyName);
      this.SetVal("HMDA.X60", this.loan.Settings.HMDAInfo.HMDAContactName);
      this.SetVal("HMDA.X63", this.loan.Settings.HMDAInfo.HMDAContactAddressLine1);
      this.SetVal("HMDA.X64", this.loan.Settings.HMDAInfo.HMDAContactCity);
      this.SetVal("HMDA.X65", this.loan.Settings.HMDAInfo.HMDAContactState);
      this.SetVal("HMDA.X66", this.loan.Settings.HMDAInfo.HMDAContactZipCode);
      this.SetVal("HMDA.X61", this.loan.Settings.HMDAInfo.HMDAContactPhone);
      this.SetVal("HMDA.X67", this.loan.Settings.HMDAInfo.HMDAContactFax);
      this.SetVal("HMDA.X62", this.loan.Settings.HMDAInfo.HMDAContactEmail);
      this.SetVal("HMDA.X72", this.loan.Settings.HMDAInfo.HMDAParentName);
      this.SetVal("HMDA.X73", this.loan.Settings.HMDAInfo.HMDAParentAddressLine1);
      this.SetVal("HMDA.X74", this.loan.Settings.HMDAInfo.HMDAParentCity);
      this.SetVal("HMDA.X75", this.loan.Settings.HMDAInfo.HMDAParentState);
      this.SetVal("HMDA.X76", this.loan.Settings.HMDAInfo.HMDAParentZipCode);
    }

    private void calculateHmdaCreditScoreForDecision(string id, string val)
    {
      if (this.IntVal("HMDA.X27") <= 2017)
      {
        if (this.IsLocked("4174"))
          this.RemoveCurrentLock("4174");
        if (this.IsLocked("HMDA.X116"))
          this.RemoveCurrentLock("HMDA.X116");
        if (this.IsLocked("4177"))
          this.RemoveCurrentLock("4177");
        if (!this.IsLocked("HMDA.X118"))
          return;
        this.RemoveCurrentLock("HMDA.X118");
      }
      else
      {
        string str = this.Val("1393");
        bool flag1 = true;
        int index1 = 0;
        string id1 = this.loan.CurrentBorrowerPair.Id;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        bool flag2 = str == "Application withdrawn" || str == "File Closed for incompleteness" || str == "Loan purchased by your institution";
        for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index2]);
          if (flag2)
          {
            this.SetVal("4174", "8888");
            if (this.Val("HMDA.X116") != "Exempt")
              this.SetVal("HMDA.X116", "8888");
            this.SetVal("4177", "8888");
            if (this.Val("HMDA.X118") != "Exempt")
              this.SetVal("HMDA.X118", "8888");
            flag1 = false;
          }
          else
          {
            if (this.Val("3840") == "Y")
            {
              this.SetVal("4177", "9999");
              this.SetVal("HMDA.X118", "9999");
              flag1 = false;
            }
            if (this.loan.Settings.HMDAInfo != null && this.Val("HMDA.X113") == "Y")
            {
              this.SetVal("HMDA.X116", "Exempt");
              this.SetVal("HMDA.X118", "Exempt");
            }
          }
          if (flag1)
          {
            if (this.IsLocked("4174"))
              this.RemoveLock("4174");
            if (this.IsLocked("HMDA.X116"))
              this.RemoveLock("HMDA.X116");
            if (this.IsLocked("4177"))
              this.RemoveLock("4177");
            if (this.IsLocked("HMDA.X118"))
              this.RemoveLock("HMDA.X118");
          }
          if (id1 == borrowerPairs[index2].Id)
            index1 = index2;
        }
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
      }
    }

    private void calculateHmdaCreditScoringModel(string id, string val)
    {
      if (this.IntVal("HMDA.X27") <= 2017)
      {
        if (this.IsLocked("4175"))
          this.RemoveCurrentLock("4175");
        if (this.IsLocked("HMDA.X117"))
          this.RemoveCurrentLock("HMDA.X117");
        if (this.IsLocked("4178"))
          this.RemoveCurrentLock("4178");
        if (!this.IsLocked("HMDA.X119"))
          return;
        this.RemoveCurrentLock("HMDA.X119");
      }
      else
      {
        string str1 = this.Val("1393");
        bool flag1 = true;
        int index1 = 0;
        string id1 = this.loan.CurrentBorrowerPair.Id;
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        bool flag2 = str1 == "Application withdrawn" || str1 == "File Closed for incompleteness" || str1 == "Loan purchased by your institution";
        string str2 = this.Val("HMDA.X117");
        string str3 = this.Val("HMDA.X119");
        for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
        {
          this.loan.SetBorrowerPair(borrowerPairs[index2]);
          if (flag2)
          {
            this.SetVal("4175", "Not applicable");
            if (this.Val("HMDA.X117") != "Exempt")
              this.SetVal("HMDA.X117", "Not applicable");
            this.SetVal("4178", "Not applicable");
            if (this.Val("HMDA.X119") != "Exempt")
              this.SetVal("HMDA.X119", "Not applicable");
            flag1 = false;
          }
          else
          {
            if (this.Val("3840") == "Y")
            {
              this.SetVal("4178", "No co-applicant");
              this.SetVal("HMDA.X119", "No co-applicant");
              flag1 = false;
            }
            if (this.loan.Settings.HMDAInfo != null && this.Val("HMDA.X113") == "Y")
            {
              this.SetVal("HMDA.X117", "Exempt");
              this.SetVal("HMDA.X119", "Exempt");
            }
          }
          if (str2 != "Other credit scoring model")
            this.SetVal("4176", "");
          if (str3 != "Other credit scoring model")
            this.SetVal("4179", "");
          if (id != null)
            this.copyHmdaValues(id, this.Val(id));
          if (flag1)
          {
            if (this.IsLocked("4175"))
              this.RemoveLock("4175");
            if (this.IsLocked("HMDA.X117"))
              this.RemoveLock("HMDA.X117");
            if (this.IsLocked("4178"))
              this.RemoveLock("4178");
            if (this.IsLocked("HMDA.X119"))
              this.RemoveLock("HMDA.X119");
          }
          if (id1 == borrowerPairs[index2].Id)
            index1 = index2;
        }
        this.loan.SetBorrowerPair(borrowerPairs[index1]);
      }
    }

    private void calculateHmdaOtherCreditScoringModel(string id, string val)
    {
      if (this.Val("HMDA.X117") != "Other credit scoring model")
        this.SetVal("4176", "");
      if (!(this.Val("HMDA.X119") != "Other credit scoring model"))
        return;
      this.SetVal("4179", "");
    }

    private void calculateHmdaAge(string id, string val)
    {
      string str1 = this.Val("1393");
      string str2 = this.Val("3840");
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        if (str1 != "Loan purchased by your institution")
        {
          this.SetVal("4183", this.Val("38"));
          this.SetVal("4184", str2 == "Y" ? "9999" : this.Val("70"));
        }
        else
        {
          if (this.loan.Settings.HMDAInfo != null && this.loan.Settings.HMDAInfo.HMDAReportAgeOfBorrower)
            this.SetVal("4183", this.Val("38"));
          else
            this.SetVal("4183", "8888");
          if (str2 == "Y")
            this.SetVal("4184", "9999");
          else if (str1 == "Loan purchased by your institution")
          {
            if (this.loan.Settings.HMDAInfo != null && this.loan.Settings.HMDAInfo.HMDAReportAgeOfCoBorrower)
              this.SetVal("4184", this.Val("70"));
            else
              this.SetVal("4184", "8888");
          }
        }
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateTypeOfPurchaser(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
      {
        if (!(this.Val("1397") == "Credit union, mortgage company, or finance company"))
          return;
        this.SetVal("1397", "");
      }
      else
      {
        string str = this.Val("1393");
        if (!(str == "Application approved but not accepted") && !(str == "Application denied") && !(str == "Application withdrawn") && !(str == "File Closed for incompleteness") && !(str == "Preapproval request denied by financial institution") && !(str == "Preapproval request approved but not accepted"))
          return;
        this.SetVal("1397", "Loan was not originated");
      }
    }

    private void calculateHmdaHOEPAStatus(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
      {
        if (!(this.Val("HMDA.X13") == "Not applicable"))
          return;
        this.SetVal("HMDA.X13", "");
      }
      else
      {
        string str1 = this.Val("1393");
        string str2 = this.Val("S32DISC.X51");
        string str3 = this.Val("1811", this.GetBorrowerPairs()[0]);
        string str4 = this.Val("HMDA.X56");
        string str5 = this.Val("19");
        if (str3 == "SecondHome" || str3 == "Investor" || str1 != "" && str1 != "Loan Originated" && str1 != "Loan purchased by your institution" || str4 == "Reverse mortgage" || str5 == "ConstructionOnly")
        {
          this.SetVal("HMDA.X13", "Not applicable");
        }
        else
        {
          if (!(str4 == "") && !(str4 == "Not a reverse mortgage") && !(str4 == "Exempt") || !(str5 == "") && !(str5 == "Purchase") && !(str5 == "Cash-Out Refinance") && !(str5 == "NoCash-Out Refinance") && !(str5 == "ConstructionToPermanent") && !(str5 == "Other"))
            return;
          switch (str2)
          {
            case "does":
              this.SetVal("HMDA.X13", "HOEPA Loan");
              break;
            case "does not":
              this.SetVal("HMDA.X13", "Not a HOEPA Loan");
              break;
            case "":
              this.SetVal("HMDA.X13", "");
              break;
          }
        }
      }
    }

    private void calculateHmdaDebtToIncomeRatio(string id, string val)
    {
      string str = this.Val("1393");
      if ((str == "Loan Originated" || str == "Application approved but not accepted" || str == "Application denied" || str == "Preapproval request denied by financial institution" || str == "Preapproval request approved but not accepted") && this.Val("HMDA.X97") != "Y")
      {
        this.SetVal("HMDA.X36", this.loan.Settings.HMDAInfo == null || this.loan.Settings.HMDAInfo.HMDADTI == null ? "" : this.getFirstBorrowerPairValue(this.loan.Settings.HMDAInfo.HMDADTI));
      }
      else
      {
        if (string.IsNullOrEmpty(str) || !(str != "Active Loan"))
          return;
        this.SetVal("HMDA.X36", "NA");
      }
    }

    private void calculateHmdaCLTV(string id, string val)
    {
      string str = this.Val("1393");
      if ((str == "Loan Originated" || str == "Application approved but not accepted" || str == "Application denied" || str == "Preapproval request denied by financial institution" || str == "Preapproval request approved but not accepted") && this.Val("HMDA.X98") != "Y")
      {
        this.SetVal("HMDA.X37", this.loan.Settings.HMDAInfo == null || this.loan.Settings.HMDAInfo.HMDACLTV == null ? "" : this.getFirstBorrowerPairValue(this.loan.Settings.HMDAInfo.HMDACLTV));
      }
      else
      {
        if (string.IsNullOrEmpty(str) || !(str != "Active Loan"))
          return;
        this.SetVal("HMDA.X37", "NA");
      }
    }

    private void calculateHmdaSubmissionOfApplication(string id, string val)
    {
      if (!this.IsHmdaActionValid("HMDA.X42"))
        return;
      this.SetVal("HMDA.X42", "Not applicable");
    }

    private void calculateHmdaNmlsLoanOriginatorId(string id, string val)
    {
      string val1 = this.Val("3238");
      string str = this.Val("1393");
      DateTime date = Utils.ParseDate((object) this.Val("745"));
      if (str == "Loan purchased by your institution" && date < Utils.ParseDate((object) "01/01/2018"))
        this.SetVal("HMDA.X86", "NA");
      else
        this.SetVal("HMDA.X86", val1);
    }

    public bool IsNMLSLoanOriginatorIDCalculated()
    {
      string str1 = this.Val("3238");
      string str2 = this.Val("1393");
      DateTime date = Utils.ParseDate((object) this.Val("745"));
      return str2 == "Loan purchased by your institution" && date < Utils.ParseDate((object) "01/01/2018") || str1 != string.Empty;
    }

    private void calculateHmdaEthnicity(string id, string val)
    {
      if (id == "3840" && val == "Y" || this.Val("1393") != "Loan purchased by your institution" || this.loan.Settings.HMDAInfo == null || this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo)
        return;
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.SetVal("1523", "Not applicable");
        this.SetVal("1531", "Not applicable");
        for (int index3 = 4; index3 <= 7; ++index3)
          this.SetVal("414" + index3.ToString(), "");
        this.SetVal("4210", "");
        this.SetVal("4211", "");
        this.SetVal("4205", "");
        this.SetVal("4125", "");
        for (int index4 = 59; index4 <= 62; ++index4)
          this.SetVal("41" + index4.ToString(), "");
        this.SetVal("4213", "");
        this.SetVal("4214", "");
        this.SetVal("4206", "");
        this.SetVal("4136", "");
        this.SetVal("4188", "");
        this.SetVal("4243", "");
        this.SetVal("4246", "");
        this.SetVal("4212", "Y");
        this.SetVal("4215", "Y");
        this.SetVal("4236", "Not Applicable");
        this.SetVal("4238", "Not Applicable");
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateHmdaRace(string id, string val)
    {
      if (id == "3840" && val == "Y" || this.Val("1393") != "Loan purchased by your institution" || this.loan.Settings.HMDAInfo == null || this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo)
        return;
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        for (int index3 = 4; index3 <= 8; ++index3)
          this.SetVal("152" + index3.ToString(), "");
        for (int index4 = 48; index4 <= 58; ++index4)
          this.SetVal("41" + index4.ToString(), "");
        this.SetVal("4126", "");
        this.SetVal("4128", "");
        this.SetVal("4130", "");
        for (int index5 = 2; index5 <= 6; ++index5)
          this.SetVal("153" + index5.ToString(), "");
        for (int index6 = 63; index6 <= 73; ++index6)
          this.SetVal("41" + index6.ToString(), "");
        this.SetVal("4137", "");
        this.SetVal("4139", "");
        this.SetVal("4141", "");
        this.SetVal("4252", "");
        this.SetVal("4253", "");
        this.SetVal("4244", "");
        this.SetVal("4247", "");
        this.SetVal("3174", "");
        this.SetVal("1530", "Y");
        this.SetVal("1538", "Y");
        this.SetVal("4237", "Not Applicable");
        this.SetVal("4239", "Not Applicable");
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateHmdaSex(string id, string val)
    {
      if (id == "3840" && val == "Y" || this.Val("1393") != "Loan purchased by your institution" || this.loan.Settings.HMDAInfo == null || this.loan.Settings.HMDAInfo.HMDAShowDemographicInfo)
        return;
      int index1 = 0;
      string id1 = this.loan.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.loan.SetBorrowerPair(borrowerPairs[index2]);
        this.SetVal("4193", "");
        this.SetVal("4194", "");
        this.SetVal("4195", "");
        this.SetVal("4245", "");
        this.SetVal("4196", "Y");
        this.SetVal("4197", "");
        this.SetVal("4198", "");
        this.SetVal("4199", "");
        this.SetVal("4248", "");
        this.SetVal("4189", "");
        this.SetVal("4200", "Y");
        this.SetVal("471", "NotApplicable");
        this.SetVal("478", "NotApplicable");
        if (id1 == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.loan.SetBorrowerPair(borrowerPairs[index1]);
    }

    private void calculateHmdaRateSpread(string id, string val)
    {
      if (this.IntVal("HMDA.X27") < 2018)
      {
        if (this.Val("HMDA.X15").LastIndexOf(".") != 3)
          return;
        this.SetVal("HMDA.X15", Utils.RemoveEndingZeros(this.Val("HMDA.X15")));
      }
      else
      {
        string str = this.Val("1393");
        if (str == "Loan purchased by your institution" || str == "Application denied" || str == "Application withdrawn" || str == "File Closed for incompleteness" || str == "Preapproval request denied by financial institution" || this.Val("HMDA.X56") == "Reverse mortgage")
        {
          this.SetVal("HMDA.X15", "NA");
        }
        else
        {
          string s = this.Val("HMDA.X15");
          Decimal result;
          if (this.loan.Settings.HMDAInfo != null && Decimal.TryParse(s, out result) && this.loan.Settings.HMDAInfo.HMDADisplayRateSpreadTo3Decimals)
          {
            this.SetVal("HMDA.X15", Utils.RemoveEndingZeros(Utils.ArithmeticRounding(result, 3).ToString()));
          }
          else
          {
            if (!(s == "NA"))
              return;
            this.SetVal("HMDA.X15", "");
          }
        }
      }
    }

    private void calculateHmdaSyncaddressfields(string id, string val)
    {
      if (id == "HMDA.X27" && this.IntVal("HMDA.X27") > 2017 && this.Val("HMDA.X91") != "Y" && this.Val("HMDA.X88") == "" && this.Val("HMDA.X89") == "" && this.Val("HMDA.X90") == "" && this.Val("HMDA.X87") == "")
        this.SetVal("HMDA.X91", "Y");
      if (!(this.Val("HMDA.X91") == "Y"))
        return;
      string str = this.Val("1393");
      string val1 = this.Val("11");
      if ((string.IsNullOrEmpty(val1) || val1.ToUpper() == "TBD") && !(str == "Loan Originated") && !(str == "Loan purchased by your institution"))
        this.SetVal("HMDA.X88", "NA");
      else
        this.SetVal("HMDA.X88", val1);
      this.SetVal("HMDA.X89", this.Val("12"));
      this.SetVal("HMDA.X90", this.Val("14"));
      this.SetVal("HMDA.X87", this.Val("15"));
    }

    private void calculateHmdaLoanPurpose(string id, string val)
    {
      if (this.IntVal("HMDA.X27") <= 2017)
        return;
      string str = this.Val("1393");
      DateTime date = Utils.ParseDate((object) this.Val("745"));
      if (!(str == "Loan purchased by your institution") || !(date < Utils.ParseDate((object) "01/01/2018")))
        return;
      this.SetVal("HMDA.X107", "Not applicable");
    }

    private void calculateHmdaInterestOnlyIndicator(string id, string val)
    {
      string str = this.Val("19");
      if (str != "ConstructionToPermanent" && str != "ConstructionOnly" && this.IntVal("1177") > 0)
      {
        this.SetVal("HMDA.X109", "Y");
      }
      else
      {
        switch (str)
        {
          case "ConstructionOnly":
            this.SetVal("HMDA.X109", "Y");
            return;
          case "ConstructionToPermanent":
            if (this.IntVal("1176") > 0)
            {
              this.SetVal("HMDA.X109", "Y");
              return;
            }
            break;
        }
        this.SetVal("HMDA.X109", "N");
      }
    }

    private void calculateNewHmdaInterestOnlyIndicator(string id, string val)
    {
      string str = this.Val("19");
      if (str != "ConstructionToPermanent" && str != "ConstructionOnly" && this.IntVal("1177") > 0)
      {
        this.SetVal("HMDA.X120", "Y");
      }
      else
      {
        switch (str)
        {
          case "ConstructionOnly":
            this.SetVal("HMDA.X120", "Y");
            return;
          case "ConstructionToPermanent":
            if (this.IntVal("1176") > 0)
            {
              this.SetVal("HMDA.X120", "Y");
              return;
            }
            break;
        }
        this.SetVal("HMDA.X120", "N");
      }
    }

    private void calculateHMDACountyCensusTrackCode(string id, string val)
    {
      string str1 = this.Val("1395");
      string val1 = this.Val("1396");
      this.Val("1393");
      bool flag = val1.Length >= 3 && val1.Length < 5 && str1.Length == 2;
      if (val1 == "NA" || str1 == "NA")
        this.SetVal("HMDA.X111", "NA");
      else if (val1.Length == 5)
        this.SetVal("HMDA.X111", val1);
      else if (!flag)
        this.SetVal("HMDA.X111", string.Empty);
      else
        this.SetVal("HMDA.X111", str1 + val1);
      string str2 = this.Val("700").Replace(".", "");
      int result = 0;
      if (str2 == "NA" || val1 == "NA" || str1 == "NA")
        this.SetVal("HMDA.X112", "NA");
      else if (str2.Length == 11)
        this.SetVal("HMDA.X112", str2);
      else if (!flag || str2.Length != 6 && str2.Length != 11 || !int.TryParse(str2, out result))
        this.SetVal("HMDA.X112", string.Empty);
      else
        this.SetVal("HMDA.X112", str1 + val1 + str2);
    }

    private void calculateHMDAAusRecommendation(string id, string val)
    {
      if (this.IntVal("HMDA.X27") >= 2019)
        return;
      this.verifyAusRecommendation("HMDA.X50");
      this.verifyAusRecommendation("HMDA.X51");
      this.verifyAusRecommendation("HMDA.X52");
      this.verifyAusRecommendation("HMDA.X53");
      this.verifyAusRecommendation("HMDA.X54");
    }

    private void verifyAusRecommendation(string id)
    {
      string str = this.Val(id);
      if (!(str == "Accept/Eligible") && !(str == "Accept/Ineligible") && !(str == "Accept/Unable to Determine") && !(str == "Refer with Caution/Eligible") && !(str == "Refer with Caution/Ineligible") && !(str == "Refer/Unable to Determine") && !(str == "Refer with Caution/Unable to Determine"))
        return;
      this.SetVal(id, "");
    }

    private void calculateHmdaDenialReasons(string id, string val)
    {
      if (this.IsHmdaActionValidForDenialReasons())
        return;
      this.SetVal("HMDA.X21", "Not applicable");
      this.SetVal("HMDA.X22", "");
      this.SetVal("HMDA.X23", "");
      this.SetVal("HMDA.X33", "");
      this.SetVal("HMDA.X34", "");
    }

    public bool IsHmdaActionValidForDenialReasons()
    {
      string str = this.Val("1393");
      return str == "Application denied" || str == "Preapproval request denied by financial institution" || str == "";
    }

    internal void UpdateEBSHiddenFields()
    {
      if (this.loan == null || this.loan.Settings == null || this.loan.Settings.HMDAInfo == null)
        return;
      this.SetVal("HMDA.X101", (this.loan.Settings.HMDAInfo.HMDADTI ?? "") == "" ? "" : this.getFirstBorrowerPairValue(this.loan.Settings.HMDAInfo.HMDADTI));
      this.SetVal("HMDA.X102", (this.loan.Settings.HMDAInfo.HMDACLTV ?? "") == "" ? "" : this.getFirstBorrowerPairValue(this.loan.Settings.HMDAInfo.HMDACLTV));
      this.SetVal("HMDA.X103", (this.loan.Settings.HMDAInfo.HMDAIncome ?? "") == "" ? "" : this.getFirstBorrowerPairValue(this.loan.Settings.HMDAInfo.HMDAIncome));
      this.SetVal("HMDA.X104", (this.loan.Settings.HMDAInfo.HMDAApplicationDate ?? "") == "" ? "" : this.Val(this.loan.Settings.HMDAInfo.HMDAApplicationDate));
    }

    private void calculateCountyCode(string id, string val)
    {
      string fips = ZipCodeUtils.GetFIPS(this.Val("14"), this.Val("13"));
      if (string.IsNullOrEmpty(fips))
        return;
      this.SetVal("1396", fips.Substring(2));
    }

    private void calculateMSACode(string id, string val)
    {
      string msaCode = ZipCodeUtils.GetMSACode(this.Val("14"), this.Val("13"));
      if (string.IsNullOrEmpty(msaCode))
        this.SetVal("699", string.Empty);
      else
        this.SetVal("699", msaCode);
    }

    private string getFirstBorrowerPairValue(string id)
    {
      if (string.IsNullOrEmpty(id))
        return "";
      BorrowerPair pair = (BorrowerPair) null;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (borrowerPairs != null && borrowerPairs.Length != 0)
        pair = borrowerPairs[0];
      return pair == null ? this.Val(id) : this.Val(id, pair);
    }

    private string getHmdaIncome(string id, int factor, int year)
    {
      string borrowerPairValue = this.getFirstBorrowerPairValue(id);
      int num = year > 2017 ? Decimal.ToInt32((Utils.ParseDecimal((object) borrowerPairValue, 0.0M) - Utils.ParseDecimal((object) this.Val("HMDA.X110"), 0.0M)) * (Decimal) factor) : Decimal.ToInt32(Utils.ParseDecimal((object) borrowerPairValue, 0.0M) * (Decimal) factor);
      return Convert.ToString((num >= 0 ? (num % 1000 >= 500 ? num + 1000 - num % 1000 : num - num % 1000) : (num % 1000 <= -500 ? num - 1000 - num % 1000 : num - num % 1000)) / 1000);
    }

    internal void SetDefaultHMDACalculation()
    {
      if (this.IntVal("HMDA.X27") <= 2017)
        return;
      this.CalcTotalLoanCosts((string) null, (string) null);
      this.CalcTotalPointsAndFees((string) null, (string) null);
      this.CalcOriginationCharges((string) null, (string) null);
      this.CalcDiscountPoints((string) null, (string) null);
      this.CalcReasonForDenial((string) null, (string) null);
      this.CalcTypeOfPurchaser((string) null, (string) null);
      this.CalcHOEPAStatus((string) null, (string) null);
      this.CalcHmdaRateSpread((string) null, (string) null);
      this.CalcHmdaSyncaddressfields((string) null, (string) null);
    }
  }
}
