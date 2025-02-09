// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Mapping
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Logging;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.XPath;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class Mapping
  {
    private bool versionMigrationOccured;
    private const string className = "Mapping";
    private const string LIABILITY = "LIABILITY";
    private const int MAX_NUMBER_OF_FIELD_TO_TRACE = 12;
    private static readonly string sw = Tracing.SwDataEngine;
    public const string ContainsEntityIds = "ContainsEntityIdsFor231";
    private const string BORROWER = "BORROWER";
    private const string BORROWERS = "BORROWER[@_PrintPositionType=\"Borrower\"]";
    private const string COBORROWERS = "BORROWER[@_PrintPositionType=\"CoBorrower\"]";
    private const string ID_HOLD = "#";
    private const string IND_HOLD = "%";
    private const int BASE = 0;
    private const int TITLE_HOLDER = 1;
    private const int DOWN_PAYMENT = 2;
    private const int BRW = 3;
    private const int COBRW = 4;
    private const int CAT_COMMON = 0;
    private const int CAT_BRW = 1;
    private const int CAT_COBRW = 2;
    public const Decimal TZcomplianceVersion = 21.104M;
    private static string[][] emAttrs = new string[31][]
    {
      new string[2]
      {
        "1074",
        "LOAN_PURPOSE/@TotalConstructionValue"
      },
      new string[2]
      {
        "1109",
        "TRANSACTION_DETAIL/@LoanAmountExcludeFees"
      },
      new string[2]
      {
        "142",
        "TRANSACTION_DETAIL/@CashFromToBorrower"
      },
      new string[2]
      {
        "388",
        "EllieMae/CLOSING_COST[1]/LoanOriginationFee/@PercentageFee"
      },
      new string[2]
      {
        "1303",
        "EllieMae/TSUM/SELLER_INFO/ContactInfo/@Contact"
      },
      new string[2]
      {
        "411",
        "EllieMae/CLOSING_COST[1]/ContactInfo/@TitleCompanyName"
      },
      new string[2]
      {
        "VAELIG.X78",
        "EllieMae/FHA_VA_LOANS/VA_CERTIFICATE_ELIGIBILITY/VeteranAddresss/@PostalCode"
      },
      new string[2]
      {
        "RE88395.X215",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1111/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X216",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1111/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X217",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1112/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X218",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1112/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X219",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1113/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X220",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1113/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X221",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1114/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X222",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1114/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X225",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1306/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X226",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1306/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X227",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1307/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X228",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1307/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X229",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1308/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X230",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1308/@PaidToOthers"
      },
      new string[2]
      {
        "RE88395.X231",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1309/@PaidToBroker"
      },
      new string[2]
      {
        "RE88395.X232",
        "EllieMae/CLOSING_COST[1]/UserDefinedFee_1309/@PaidToOthers"
      },
      new string[2]
      {
        "1165",
        "EllieMae/MCAW/BORROWER_RATING/@AdequancyOfEffectiveIncome"
      },
      new string[2]{ "995", "MORTGAGE_TERMS/@ARMTypeDescription" },
      new string[2]
      {
        "248",
        "EllieMae/LOAN_SUBMISSION/@ARMDescription"
      },
      new string[2]{ "698", "EllieMae/@MaximumBalance" },
      new string[2]
      {
        "CASASRN.X79",
        "EllieMae/FREDDIE_MAC/@CashOutAmount"
      },
      new string[2]
      {
        "MAX23K.X3",
        "EllieMae/FHA_203K/@EscrowCommitment"
      },
      new string[2]{ "649", "EllieMae/ENDORSEMENT/@EscrowAmount" },
      new string[2]
      {
        "156",
        "EllieMae/ENDORSEMENT/@VeteransPreference"
      }
    };
    private static ArrayList constFields = new ArrayList();
    private static ArrayList brwFields = new ArrayList();
    private static ArrayList cobrwFields = new ArrayList();
    private static CryptoRandom rndm = new CryptoRandom();
    public static readonly string[] XPathsForEntityIds = new string[54]
    {
      nameof (BORROWER),
      "ASSET",
      "BORROWER/EllieMae/DEPOSIT",
      nameof (LIABILITY),
      nameof (REO_PROPERTY),
      "BORROWER/_RESIDENCE",
      "BORROWER/EllieMae/PAIR/TAX_4506/HISTORY",
      "BORROWER/EllieMae/PAIR/TAX_4506T/HISTORY",
      "BORROWER/EllieMae/PAIR/TAX4506T_OrderReports/OrderInformation",
      "EllieMae/LOAN_PROGRAM",
      "EllieMae/SettlementServiceProviders/ServiceProvider",
      "EllieMae/AffiliatedBusinessArrangements/Affiliate",
      "EllieMae/HomeCounselingProviders/HomeCounselingProvider",
      "EllieMae/NonVols/NonVol",
      "EllieMae/InvestorDeliveryLogs/InvestorDeliveryLog",
      "EllieMae/Disasters/Disaster",
      "EllieMae/EncompassToEncompassLogs/EncompassToEncompassLog",
      "LOAN_PRODUCT_DATA/HELOC/EllieMae/DrawPeriod/Draw",
      "LOAN_PRODUCT_DATA/HELOC/EllieMae/RepaymentPeriod/Repayment",
      "EllieMae/HUD1ES/Date",
      "EllieMae/HUD1ES/DueDate",
      "EllieMae/HUD1ES/Setup",
      "EllieMae/HUD1ES/Itemize",
      "EllieMae/TRUST_ACCOUNT/ITEM",
      "EllieMae/STATEDISCLOSURE/NEWYORK/Fees",
      "EllieMae/TQL/FraudOrderAlerts/Alert",
      "EllieMae/TQL/ComplianceOrders/Alert",
      "EllieMae/TQL/Documents/Document",
      "EllieMae/LOG/Record/Alert",
      "EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner",
      "BORROWER/EMPLOYER",
      "EllieMae/AlertChangeCircumstanceList/AlertChangeCircumstance",
      "LOAN_PRODUCT_DATA/HELOC/EllieMae/HistoricalIndexSetting/YearSetting",
      "EllieMae/TQL/GSETrackers/Tracker",
      "_CLOSING_DOCUMENTS/TRUSTEE",
      "BORROWER/ProvidedDocuments/PROVIDED_DOCUMENT",
      "BORROWER/URLA2020/OtherAssets/OTHER_ASSET",
      "BORROWER/URLA2020/OtherLiabilities/OTHER_LIABILITY",
      "BORROWER/OtherLiabilities/OTHER_LIABILITY",
      "BORROWER/URLA2020/GiftsGrants/GiftGrant",
      "BORROWER/URLA2020/OtherIncomeSources/OtherIncomeSource",
      "BORROWER/URLA2020/URLAAlternateNames/URLAAlternateName",
      "BORROWER/URLA2020/AdditionalLoans/Additional_Loan",
      "BORROWER/AdditionalLoans/Additional_Loan",
      "EllieMae/Correspondent/Valuations/Valuation",
      "EllieMae/Correspondent/EarlyChecks/EarlyCheck",
      "EllieMae/Correspondent/Riders/Rider",
      "EllieMae/Correspondent/Scenarios/Scenario",
      "EllieMae/Correspondent/Disclosures/Disclosure",
      "EllieMae/Correspondent/CorrOtherInsurances/CorrOtherInsurance",
      "EllieMae/PropertyValuation",
      "EllieMae/SpecialFeatureCode",
      "BORROWER/CORRESPONDENT/CreditReports/CreditReport",
      "BORROWER/EllieMae/PAIR/ATR_QM/AusTrackingHistory/History"
    };
    public static readonly string[] XPathsForBadEntityIds = new string[1]
    {
      "_CLOSING_DOCUMENTS/TRUSTEE"
    };
    private Hashtable fields = new Hashtable();
    private LogList logList;
    private string brwId;
    private string coBrwId;
    private string brwPredicate;
    private string coBrwPredicate;
    private string liabPath;
    private string otherliabilitypath;
    private string otherAssetPath;
    private string additionalLoanpath;
    private string providedDocumentPath;
    private BorrowerPair currentBorrowerPair;
    private XmlElement root;
    private XmlElement lockRoot;
    private XmlDocument xmldoc;
    private XmlElement brwElm;
    private XmlElement coBrwElm;
    [NonSerialized]
    private StringBuilder cachedXml = new StringBuilder();
    private ILoanSettings loanSettings;
    private bool ignoreValidationErrors;
    [NonSerialized]
    private Dictionary<string, XmlElement> cachedElements = new Dictionary<string, XmlElement>();
    private string systemId = "";
    private string originalLoanVersion = "";
    private Dictionary<string, bool> traceFields = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private bool traceFieldUpdate;
    private List<string> fieldsToTrace = new List<string>();
    private string traceFieldUpdatePath = "";
    private TextTraceListener listener;
    private string ddmOnDemandTriggerFields;
    private bool ddmIsRequired;
    private Dictionary<string, string> cachedOrgFields = new Dictionary<string, string>();
    private static string[][][] valueMappings = new string[3][][]
    {
      new string[6][]
      {
        new string[1]
        {
          "EllieMae/HUD_HOME_IMPROVEMENT/@PropertyToBeImproved"
        },
        new string[2]{ "Single family", "SingleFamily" },
        new string[2]{ "Multifamily", "MultiFamily" },
        new string[2]{ "Manufactured home", "ManufacturedHome" },
        new string[2]
        {
          "Historic residential",
          "HistoricResidential"
        },
        new string[2]
        {
          "Health care facility",
          "HealthcareFacility"
        }
      },
      new string[2][]
      {
        new string[1]{ "BORROWER/EllieMae/DEPOSIT/@Type1" },
        new string[2]
        {
          "SecuredBorrowedFundNotDeposited",
          "SecuredBorrowedFundsNotDeposited"
        }
      },
      new string[2][]
      {
        new string[1]{ "ASSET/@_Type" },
        new string[2]
        {
          "SecuredBorrowedFundNotDeposited",
          "SecuredBorrowedFundsNotDeposited"
        }
      }
    };
    private static string[] idPaths = new string[15]
    {
      "BORROWER/@BorrowerID",
      "BORROWER/@JointAssetBorrowerID",
      "LIABILITY/@_ID",
      "LIABILITY/@REO_ID",
      "LIABILITY/@BorrowerID",
      "REO_PROPERTY/@REO_ID",
      "REO_PROPERTY/@BorrowerID",
      "BORROWER/EllieMae/DEPOSIT/@ID",
      "BORROWER/_RESIDENCE/EllieMae/@_ID",
      "BORROWER/EMPLOYER/EllieMae/@_ID",
      "LOCK/FIELD/@BorrowerID",
      "ASSET/EllieMae/@AssetID",
      "ASSET/@BorrowerID",
      "EllieMae/LOG/Record/@PairId",
      "EllieMae/LOG/Record/@Id"
    };
    private static string[][] borAttr = new string[83][]
    {
      new string[2]
      {
        "300",
        "EllieMae/@CreditReportReferenceIdentifier"
      },
      new string[2]{ "1811", "EllieMae/@PropertyUsageType" },
      new string[2]
      {
        "307",
        "EllieMae/@IncomeOtherThanBorrowerUsed"
      },
      new string[2]
      {
        "35",
        "EllieMae/@IncomeOfBorrowersSpouseUsed"
      },
      new string[2]{ "901", "EllieMae/@TotalBase" },
      new string[2]{ "902", "EllieMae/@TotalOvertime" },
      new string[2]{ "903", "EllieMae/@TotalBonus" },
      new string[2]{ "904", "EllieMae/@TotalCommissions" },
      new string[2]{ "905", "EllieMae/@TotalDividendsInterest" },
      new string[2]{ "906", "EllieMae/@TotalNetRentalIncome" },
      new string[2]{ "907", "EllieMae/@TotalOther1" },
      new string[2]{ "908", "EllieMae/@TotalOther2" },
      new string[2]{ "736", "EllieMae/@BrwCoBrwTotalIncome" },
      new string[2]{ "144", "EllieMae/@PersonOtherIncome1" },
      new string[2]{ "145", "EllieMae/@DescOtherIncome1" },
      new string[2]{ "146", "EllieMae/@AmountOtherIncome1" },
      new string[2]{ "147", "EllieMae/@PersonOtherIncome2" },
      new string[2]{ "148", "EllieMae/@DescOtherIncome2" },
      new string[2]{ "149", "EllieMae/@AmountOtherIncome2" },
      new string[2]{ "150", "EllieMae/@PersonOtherIncome3" },
      new string[2]{ "151", "EllieMae/@DescOtherIncome3" },
      new string[2]{ "152", "EllieMae/@AmountOtherIncome3" },
      new string[2]{ "979", "EllieMae/@TotalDeposit" },
      new string[2]
      {
        "909",
        "EllieMae/@TotalMortgagesMonthlyPayment"
      },
      new string[2]{ "941", "EllieMae/@TotalMortgagesBalance" },
      new string[2]{ "350", "EllieMae/@TotalMonthlyPayment" },
      new string[2]{ "732", "EllieMae/@TotalAssets" },
      new string[2]{ "734", "EllieMae/@NetWorth" },
      new string[2]{ "206", "EllieMae/@AliasName1" },
      new string[2]{ "1734", "EllieMae/@CreditorName1" },
      new string[2]{ "1735", "EllieMae/@AccountNumber1" },
      new string[2]{ "203", "EllieMae/@AliasName2" },
      new string[2]{ "1736", "EllieMae/@CreditorName2" },
      new string[2]{ "1737", "EllieMae/@AccountNumber2" },
      new string[2]{ "1374", "EllieMae/@GrossIncomeForComortSet" },
      new string[2]
      {
        "1379",
        "EllieMae/@PrimaryResidenceComortSet"
      },
      new string[2]{ "1384", "EllieMae/@MonthlyExpenseComortSet" },
      new string[2]{ "740", "EllieMae/@TopRatio" },
      new string[2]{ "742", "EllieMae/@BottomRatio" },
      new string[2]{ "267", "EllieMae/@LiquidAssetsComortSet" },
      new string[2]
      {
        "268",
        "EllieMae/@PresentHousingExpComortSet"
      },
      new string[2]
      {
        "1539",
        "EllieMae/@DebtToHousingGapRatio_FRE"
      },
      new string[2]{ "273", "EllieMae/@BrwCoBrwGrossBaseIncome" },
      new string[2]
      {
        "1168",
        "EllieMae/@BrwCoBrwGrossOtherIncome"
      },
      new string[2]{ "1171", "EllieMae/@GrossPositiveCashFlow" },
      new string[2]{ "1389", "EllieMae/@TotalGrossMonthlyIncome" },
      new string[2]{ "1815", "EllieMae/TSUM/@UserDefinedIncome" },
      new string[2]{ "1816", "EllieMae/TSUM/@UserDefinedIncome" },
      new string[2]
      {
        "1810",
        "EllieMae/@TotalEarningFromEmpployment"
      },
      new string[2]
      {
        "1006",
        "EllieMae/VA_LOAN_SUMMARY/@SpouseIncomeConsider"
      },
      new string[2]
      {
        "1008",
        "EllieMae/VA_LOAN_SUMMARY/@SpouseIncomeAmount"
      },
      new string[2]{ "1094", "EllieMae/@AssetsAvailable" },
      new string[2]
      {
        "CAPIAP.X139",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[2]/@LienHolderName"
      },
      new string[2]
      {
        "CAPIAP.X140",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[2]/@YearAndMake"
      },
      new string[2]
      {
        "CAPIAP.X141",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[2]/@LoanAmount"
      },
      new string[2]
      {
        "CAPIAP.X142",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[2]/@PresentBalance"
      },
      new string[2]
      {
        "CAPIAP.X143",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[2]/@MonthlyPayment"
      },
      new string[2]
      {
        "CAPIAP.X134",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[1]/@LienHolderName"
      },
      new string[2]
      {
        "CAPIAP.X135",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[1]/@YearAndMake"
      },
      new string[2]
      {
        "CAPIAP.X136",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[1]/@LoanAmount"
      },
      new string[2]
      {
        "CAPIAP.X137",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[1]/@PresentBalance"
      },
      new string[2]
      {
        "CAPIAP.X138",
        "EllieMae/HUD_HOME_IMPROVEMENT/AUTOMOTIVE[1]/@MonthlyPayment"
      },
      new string[2]
      {
        "CAPIAP.X149",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[2]/@LienHolder"
      },
      new string[2]
      {
        "CAPIAP.X30",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[2]/@FHAInsured"
      },
      new string[2]
      {
        "CAPIAP.X29",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[2]/@LoanAmount"
      },
      new string[2]
      {
        "CAPIAP.X152",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[2]/@PresentBalance"
      },
      new string[2]
      {
        "CAPIAP.X153",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[2]/@MonthlyPayment"
      },
      new string[2]
      {
        "CAPIAP.X148",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[1]/@LienHolder"
      },
      new string[2]
      {
        "CAPIAP.X28",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[1]/@FHAInsured"
      },
      new string[2]
      {
        "CAPIAP.X27",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[1]/@LoanAmount"
      },
      new string[2]
      {
        "CAPIAP.X150",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[1]/@PresentBalance"
      },
      new string[2]
      {
        "CAPIAP.X151",
        "EllieMae/HUD_HOME_IMPROVEMENT/REAL_ESTATE[1]/@MonthlyPayment"
      },
      new string[2]
      {
        "CAPIAP.X39",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[11]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X38",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[10]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X37",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[9]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X36",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[8]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X35",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[7]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X34",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[6]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X33",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[5]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X32",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[4]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X31",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[3]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X26",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[2]/@Amount"
      },
      new string[2]
      {
        "CAPIAP.X25",
        "EllieMae/HUD_HOME_IMPROVEMENT/LOAN_AMOUNT[1]/@Amount"
      }
    };
    private static string[][] ellieAttr = new string[27][]
    {
      new string[2]
      {
        "1312",
        "EllieMae/FHA_VA_LOANS/@BrwCoBrwTotalTaxDeductions"
      },
      new string[2]
      {
        "1315",
        "EllieMae/FHA_VA_LOANS/@BrwCoBrwTotalNetTakeHomePay"
      },
      new string[2]
      {
        "1318",
        "EllieMae/FHA_VA_LOANS/@BrwCoBrwTotalOtherNetIncome"
      },
      new string[2]
      {
        "1321",
        "EllieMae/FHA_VA_LOANS/@BrwCoBrwTotalNetIncome"
      },
      new string[2]
      {
        "198",
        "EllieMae/FHA_VA_LOANS/@OtherItemsDeducted"
      },
      new string[2]
      {
        "1323",
        "EllieMae/FHA_VA_LOANS/@TotalNetEffectiveIncome"
      },
      new string[2]
      {
        "1349",
        "EllieMae/FHA_VA_LOANS/@TotalForEstimatedMonthlyShelterExpense"
      },
      new string[2]
      {
        "1340",
        "EllieMae/FHA_VA_LOANS/@BalanceAvailableFamilySupportGuideline"
      },
      new string[2]
      {
        "1325",
        "EllieMae/FHA_VA_LOANS/@BalanceAvailableFamilySupportAmount"
      },
      new string[2]
      {
        "1341",
        "EllieMae/FHA_VA_LOANS/@DebtIncomeRatio"
      },
      new string[2]
      {
        "1326",
        "EllieMae/FHA_VA_LOANS/@PastCreditRecord"
      },
      new string[2]
      {
        "1327",
        "EllieMae/FHA_VA_LOANS/@VACreditStandards"
      },
      new string[2]
      {
        "993",
        "EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY/@TotalMonthlyGrossIncome"
      },
      new string[2]
      {
        "CASASRN.X170",
        "EllieMae/FREDDIE_MAC/@BorrowerField1"
      },
      new string[2]
      {
        "CASASRN.X171",
        "EllieMae/FREDDIE_MAC/@BorrowerField2"
      },
      new string[2]
      {
        "CASASRN.X180",
        "EllieMae/FREDDIE_MAC/@CoBorrowerField1"
      },
      new string[2]
      {
        "CASASRN.X181",
        "EllieMae/FREDDIE_MAC/@CoBorrowerField2"
      },
      new string[2]
      {
        "MCAWPUR.X22",
        "EllieMae/MCAW/@MtgPaymentToIncome1"
      },
      new string[2]
      {
        "MCAWPUR.X23",
        "EllieMae/MCAW/@TotalFixedPaymentToIncome1"
      },
      new string[2]
      {
        "GMCAW.X7",
        "EllieMae/MCAW/@MtgPaymentToIncome2"
      },
      new string[2]
      {
        "GMCAW.X8",
        "EllieMae/MCAW/@TotalFixedPaymentToIncome2"
      },
      new string[2]
      {
        "VEND.X241",
        "EllieMae/SELLER/@Relationship"
      },
      new string[2]
      {
        "VEND.X242",
        "EllieMae/SELLER/@LineItemNumber"
      },
      new string[2]{ "L611", "EllieMae/HUD1ES/Servicer/@Contact" },
      new string[2]{ "MORNET.X66", "LOAN_FEATURES/@ProductName" },
      new string[2]
      {
        "VALA.X19",
        "EllieMae/FHA_VA_LOANS/VA_LOAN_ANALYSIS/@BorrowerOtherIncomeDeduction"
      },
      new string[2]
      {
        "CASASRN.X163",
        "EllieMae/FREDDIE_MAC/@OfferingIdentifier"
      }
    };
    private static Hashtable msMap = new Hashtable();
    private const string REO_PROPERTY = "REO_PROPERTY";
    private const string MORT_HEADER = "FM";
    private const string MORTGAGELIAB = "LIABILITY[@_Type=\"MortgageLoan\"]";
    private const string depPath = "EllieMae/DEPOSIT";
    private const string giftGrantPath = "URLA2020/GiftsGrants/GiftGrant";
    private const string otherIncomeSourcePath = "URLA2020/OtherIncomeSources/OtherIncomeSource";
    private const string depIdPathMISMO = "ASSET/EllieMae[@AssetID=";
    private const string LOCK_PATH = "LOCK/FIELD[@id=\"";
    private const string FIELD = "FIELD";
    private const string ID = "id";
    private const string VAL = "val";
    private const string CC_ROOT = "EllieMae/CLOSING_COST";
    private const string CC = "CLOSING_COST";
    private const string LP_ROOT = "EllieMae/LOAN_PROGRAM";
    private const string LP = "LOAN_PROGRAM";
    private const string OtherIncomePredicate = "[not(@IncomeType=\"Base\") and not(@IncomeType=\"Overtime\") and not(@IncomeType=\"Bonus\") and not(@IncomeType=\"Commissions\") and not(@IncomeType=\"DividendsInterest\") and not(@IncomeType=\"NetRentalIncome\") and not(@IncomeType=\"OtherTypesOfIncome\")]";
    private const string SERVICEPROVIDERPATH = "EllieMae/SettlementServiceProviders";
    private const string SERVICEPROVIDERNODE = "ServiceProvider";
    private const string tax4506TPath = "EllieMae/PAIR/TAX_4506T/HISTORY";
    private const string tax4506Path = "EllieMae/PAIR/TAX_4506/HISTORY";
    private const string GSETRACKERSRPATH = "EllieMae/TQL/GSETrackers";
    private const string GSETRACKERNODE = "Tracker";
    private const string DISASTERSRPATH = "EllieMae/Disasters";
    private const string DISASTERNODE = "Disaster";
    private static string AUSTrackingHistoryPath = "EllieMae/PAIR/ATR_QM/AusTrackingHistory";
    private static string verificationTimelinePath = "EllieMae/PAIR/ATR_QM/VerificationTimeline";
    private static string verificationDocumentPath = "EllieMae/PAIR/ATR_QM/VerificationDocuments";
    private const string HOMECOUNSELINGPATH = "EllieMae/HomeCounselingProviders";
    private const string HOMECOUNSELINGNODE = "HomeCounselingProvider";
    private const string NONVOLPATH = "EllieMae/NonVols";
    private const string NONVOLNODE = "NonVol";
    private const string AFFILIATEDBUSINESSPATH = "EllieMae/AffiliatedBusinessArrangements";
    private const string AFFILIATENODE = "Affiliate";
    private static string LINKSYNCNODE = "EllieMae/ConstructionLinkAndSync/LinkedSubset/Fields";
    private const string NONBORROWINGOWNERCONTACTPATH = "EllieMae/NonBorrowingOwnerContacts";
    private const string NONBORROWINGOWNERNODE = "NonBorrowingOwner";
    private const string URLAAlternateNamePath = "URLA2020/URLAAlternateNames";
    private const string URLAAlternateNameNode = "URLAAlternateName";
    private const string GOODFAITHCHANGECIRCUMSTANCEPATH = "EllieMae/AlertChangeCircumstanceList";
    private const string GOODFAITHCHANGECIRCUMSTANCENODE = "AlertChangeCircumstance";
    private static string EDS_NODE_REQUIREDDATA = "EllieMae/EDSRequiredData";
    private static string EDS_NODE_REQUIREDDATAERROR = "/Errors";
    private static string EDS_NODE_REQUIREDDATA_AMORTIZATION = "/PaymentSchedules/Amortization";
    private static string EDS_NODE_REQUIREDDATA_WORSTCASE = "/PaymentSchedules/WorstCase";
    private static string EDS_NODE_REQUIREDDATA_UCD_LE = "/UCD/LE";
    private static string EDS_NODE_REQUIREDDATA_UCD_CD = "/UCD/CD";
    private static string EDS_NODE_REQUIREDDATA_FUNDINGFEE = "/FundingFees";
    private static string EDS_NODE_REQUIREDDATA_HELOC = "/HELOC";
    private static string EDS_NODE_REQUIREDDATA_CONDITIONLETTER = "/ConditionLetterSetting";
    private static string EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET = "/BalancingWorksheet";
    private static string EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_DEBITS = "/Debits";
    private static string EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_CREDITS = "/Credits";
    private static string EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_PRINTABLEAMOUNTS = "/PrintableAmounts";
    private static string EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_OTHERS = "/Others";
    private Dictionary<string, string> eds_UCD_Dictionary;

    public event MappingFieldChangedEventHandler MappingFieldChanged;

    internal string DDMOnDemandTriggerFields
    {
      get => this.ddmOnDemandTriggerFields;
      set => this.ddmOnDemandTriggerFields = value;
    }

    internal bool DDMIsRequired
    {
      get => this.ddmIsRequired;
      set => this.ddmIsRequired = value;
    }

    public bool VersionMigrationOccured => this.versionMigrationOccured;

    internal Mapping()
    {
    }

    internal Mapping(XmlDocument xmldoc, ILoanSettings loanSettings, bool skipUpdateCachedXml)
    {
      this.loanSettings = loanSettings;
      this.systemId = loanSettings.SystemID;
      this.root = xmldoc.DocumentElement;
      this.xmldoc = xmldoc;
      this.migrateXml(loanSettings.MigrationData);
      if (!skipUpdateCachedXml)
        this.UpdateCachedXmlFromxmlDoc(this.xmldoc);
      this.loadTraceFields();
      this.ddmIsRequired = false;
    }

    private void UpdateCachedXmlFromxmlDoc(XmlDocument xmlDocument)
    {
      this.cachedXml.Clear();
      using (StringWriter stringWriter = new StringWriter(this.cachedXml))
      {
        StringWriter output = stringWriter;
        using (XmlWriter w = XmlWriter.Create((TextWriter) output, new XmlWriterSettings()
        {
          CheckCharacters = false
        }))
          xmlDocument.WriteTo(w);
      }
    }

    private void LoadXmlDocFromCachedXml(XmlDocument xmlDocument)
    {
      using (StringBuilderReader stringBuilderReader = new StringBuilderReader(this.cachedXml))
      {
        StringBuilderReader input = stringBuilderReader;
        using (XmlReader reader = XmlReader.Create((TextReader) input, new XmlReaderSettings()
        {
          CheckCharacters = false
        }))
          xmlDocument.Load(reader);
      }
    }

    private void migrateXml(LoanMigrationData migrationData)
    {
      this.originalLoanVersion = this.EMXMLVersionID;
      if (this.LoanVersionNumber <= 0)
        this.LoanVersionNumber = 1;
      this.setComplianceVersion();
      this.initializeNodes();
      this.cleanupXml();
      this.cleanupXml31();
      this.cleanupXml33();
      this.cleanupXml34();
      this.cleanupXml35();
      this.cleanupXml351();
      this.cleanupXml36();
      this.cleanupXml361(migrationData);
      this.cleanupXml362();
      this.cleanupXml363();
      this.cleanupXml364();
      this.cleanupXml365();
      this.cleanupXml366();
      this.cleanupXml40();
      this.cleanupXml60();
      this.cleanupXml62();
      this.cleanupXml6203();
      this.cleanupXml6206();
      this.cleanupXml6207();
      this.cleanupXml6210();
      this.cleanupXml65();
      this.cleanupXml67();
      this.cleanupXml6705();
      this.cleanupXml6800();
      this.cleanupXml6803();
      this.cleanupXml70();
      this.cleanupXml7003();
      this.cleanupXml7004();
      this.cleanupXml75();
      this.cleanupXml751();
      this.cleanupXml7512();
      this.cleanupXml7513();
      this.cleanupXml8000();
      this.cleanupXml8100();
      this.cleanupXml9000(migrationData);
      this.cleanupXml9002();
      this.cleanupXml9004();
      this.cleanupXml9005();
      this.cleanupXml9102();
      this.cleanupXml14000();
      this.cleanupXml14203();
      this.cleanupXml14206();
      this.cleanupXml15100();
      this.cleanupXml15145();
      this.cleanupXml15149();
      this.cleanupXml151491();
      this.cleanupXml15208();
      this.cleanupXml15209();
      this.cleanupXml16102();
      this.cleanupXml16103();
      this.cleanupXml16201();
      this.cleanupXml17100();
      this.cleanupXml17104();
      this.cleanupXml17200();
      this.cleanupXml17202();
      this.cleanupXml17206();
      this.cleanupXml17300();
      this.cleanupXml17303();
      this.cleanupXml17307();
      this.cleanupXml17400();
      this.cleanupXml18100();
      this.cleanupXml18101();
      this.cleanupXml18104();
      this.cleanupXml18105();
      this.cleanupXml18200();
      this.cleanupXml18202();
      this.cleanupXml18300();
      this.cleanupXml18307();
      this.cleanupXml18400();
      this.cleanupXml18401();
      this.cleanupXml18403();
      this.cleanupXml18404();
      this.cleanupXml19100();
      this.cleanupXml19200();
      this.cleanupXml19300();
      this.cleanupXml19305();
      this.cleanupXml19400();
      this.cleanupXml19403();
      this.cleanupXml19407();
      this.cleanupXml20100();
      this.cleanupXml20103();
      this.cleanupXml20105();
      this.cleanupXml201012();
      this.cleanupXml20200();
      this.cleanupXml20204();
      this.cleanupXml20207();
      this.cleanupXml21100();
      this.cleanupXml21101();
      this.cleanupXml21102();
      this.cleanupXml21104();
      this.cleanupXml21200();
      this.cleanupXml21201();
      this.cleanupXml21203();
      this.cleanupXml21204();
      this.cleanupXml21300();
      this.cleanupXml21402();
      this.cleanupXml22100();
      this.cleanupXml22200();
      this.cleanupXml22300();
      this.cleanupXml22303();
      this.cleanupXml22304();
      this.cleanupXml22306();
      this.cleanupXml23100();
      this.cleanupXml23300();
      this.cleanupXml23302();
      this.cleanupXml24100();
      this.cleanupXml24200();
      this.cleanupXml24201();
      this.cleanupXml24300();
    }

    private void setComplianceVersion()
    {
      try
      {
        this.setFieldAtXpath((XmlElement) this.root.SelectSingleNode("EllieMae/ComplianceVersion") ?? this.createPath("EllieMae/ComplianceVersion"), "_NewBuydownEnabled", "COMPLIANCEVERSION.NEWBUYDOWNENABLED", this.loanSettings == null || !this.loanSettings.EnableTempBuyDown ? "N" : "Y", this.currentBorrowerPair);
      }
      catch
      {
      }
    }

    private void initializeNodes()
    {
      this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
      if (this.lockRoot != null)
        return;
      this.lockRoot = this.xmldoc.CreateElement("LOCK");
      this.root.AppendChild((XmlNode) this.lockRoot);
    }

    private string generateNewId()
    {
      lock (Mapping.rndm)
        return "_" + Mapping.rndm.Next().ToString();
    }

    internal string OriginalLoanVersion => this.originalLoanVersion;

    internal Field this[string fieldId] => this.GetFieldObject(fieldId, (BorrowerPair) null);

    internal Field GetFieldObject(string fieldId, BorrowerPair borrowerPair)
    {
      if (this.fields.Count == 0)
      {
        this.fields = new Hashtable(StandardFields.CommonFields.Count + 15);
        this.preloadCommonFields();
      }
      Field field1 = (Field) this.fields[(object) fieldId.ToUpper()];
      if (field1 != null)
        return this.resolveFieldForBorrowerPair(field1, borrowerPair == null ? this.currentBorrowerPair : borrowerPair);
      FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
      BorrowerPair pair = this.resolveBorrowerPair(fieldPairInfo, borrowerPair);
      Field field2 = (Field) this.fields[(object) fieldPairInfo.FieldID.ToUpper()];
      if (field2 == null)
      {
        field2 = this.createField(fieldPairInfo.FieldID);
        if (field2 == null)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Warning, nameof (Mapping), "Field not defined in the map file: " + fieldId);
          return Field.Empty;
        }
        this.fields[(object) field2.Id.ToUpper()] = (object) field2;
      }
      return this.resolveFieldForBorrowerPair(field2, pair);
    }

    private Field resolveFieldForBorrowerPair(Field field, BorrowerPair pair)
    {
      if (field.Definition.Category == FieldCategory.Common || pair == null)
        return field;
      string predicate = (string) null;
      if (field.Definition.Category == FieldCategory.Borrower)
        predicate = this.createPredicateForBorrower(pair.Borrower.Id);
      else if (field.Definition.Category == FieldCategory.Coborrower)
        predicate = this.createPredicateForBorrower(pair.CoBorrower.Id);
      return new Field(field, predicate);
    }

    private BorrowerPair resolveBorrowerPair(FieldPairInfo fieldInfo, BorrowerPair selectedPair)
    {
      BorrowerPair borrowerPair = (BorrowerPair) null;
      if (fieldInfo.PairIndex > 0)
      {
        borrowerPair = this.GetBorrowerPairByVisibleIndex(fieldInfo.PairIndex);
        if (borrowerPair == null)
          return (BorrowerPair) null;
      }
      if (borrowerPair != null)
        return borrowerPair;
      return selectedPair != null ? selectedPair : this.currentBorrowerPair;
    }

    private void preloadCommonFields()
    {
      foreach (FieldDefinition commonField in StandardFields.CommonFields)
        this.fields[(object) commonField.FieldID.ToUpper()] = (object) this.createField(commonField);
    }

    private Field createField(string fieldId)
    {
      FieldSettings fieldConfig = (FieldSettings) null;
      if (this.loanSettings != null)
        fieldConfig = this.loanSettings.FieldSettings;
      FieldDefinition field = EncompassFields.GetField(fieldId, fieldConfig);
      return field == null ? (Field) null : this.createField(field);
    }

    private Field createField(FieldDefinition def)
    {
      FieldEvents fieldEvents = !def.IsInstance ? new FieldEvents() : this[def.ParentField.FieldID].Events;
      return new Field(def, fieldEvents);
    }

    private string createPredicateForBorrower(string borrowerId)
    {
      return "[@BorrowerID=\"" + borrowerId + "\"]";
    }

    private void cleanupXml()
    {
      if (!this.root.HasAttribute("ContainsEntityIdsFor231"))
        Mapping.AddMissingEntityIds(this.root);
      if (this.toDouble(this.EMXMLVersionID) >= 3.0)
        return;
      this.versionMigrationOccured = true;
      this.root.SetAttribute("EMXMLVersionID", "3.0");
      this.root.SetAttribute("xsi:noNamespaceSchemaLocation", "EMXML.xsd");
      this.fixEnumerationValues();
      this.addUnderscoreForIds();
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      this.SetBorrowerPair(borrowerPairs[0]);
      this.addMissingFields();
      foreach (XmlElement selectNode in this.root.SelectNodes("BORROWER/EllieMae/DEPOSIT"))
      {
        if (selectNode.HasAttribute("holderCity"))
        {
          selectNode.SetAttribute("HolderCity", selectNode.GetAttribute("holderCity"));
          selectNode.RemoveAttribute("holderCity");
        }
      }
      foreach (XmlElement selectNode in this.root.SelectNodes("BORROWER/VA_BORROWER/EllieMae"))
      {
        if (selectNode.HasAttribute("VAFederalTaxAmount"))
        {
          ((XmlElement) selectNode.ParentNode).SetAttribute("VAFederalTaxAmount", selectNode.GetAttribute("VAFederalTaxAmount"));
          selectNode.RemoveAttribute("VAFederalTaxAmount");
        }
        if (selectNode.HasAttribute("VAStateTaxAmount"))
        {
          ((XmlElement) selectNode.ParentNode).SetAttribute("VAStateTaxAmount", selectNode.GetAttribute("VAStateTaxAmount"));
          selectNode.RemoveAttribute("VAStateTaxAmount");
        }
        if (selectNode.HasAttribute("VASocialSecurityTaxAmount"))
        {
          ((XmlElement) selectNode.ParentNode).SetAttribute("VASocialSecurityTaxAmount", selectNode.GetAttribute("VASocialSecurityTaxAmount"));
          selectNode.RemoveAttribute("VASocialSecurityTaxAmount");
        }
      }
      foreach (string[] emAttr in Mapping.emAttrs)
      {
        XmlNode xmlNode = this.root.SelectSingleNode(emAttr[1]);
        if (xmlNode != null)
        {
          this.SetFieldAt(emAttr[0], xmlNode.Value);
          int num = emAttr[1].IndexOf('@');
          XmlElement oldChild = ((XmlAttribute) xmlNode).OwnerElement;
          oldChild.RemoveAttribute(emAttr[1].Substring(num + 1));
          XmlElement parentNode;
          for (; oldChild.Attributes.Count == 0 && oldChild.ChildNodes.Count == 0; oldChild = parentNode)
          {
            parentNode = (XmlElement) oldChild.ParentNode;
            oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
          }
        }
      }
      XmlNode xmlNode1 = this.root.SelectSingleNode("EllieMae/HUD_HOME_IMPROVEMENT");
      if (xmlNode1 != null)
      {
        xmlNode1.ParentNode.RemoveChild(xmlNode1);
        this.createPath("EllieMae/FHA_VA_LOANS").AppendChild(xmlNode1);
      }
      XmlNode oldChild1 = this.root.SelectSingleNode("FHA_VA_LOAN");
      if (oldChild1 != null)
      {
        this.createPath("GOVERNMENT_LOAN/FHA_VA_LOAN").SetAttribute("GovernmentMortgageCreditCertificateAmount", ((XmlElement) oldChild1).GetAttribute("GovernmentMortgageCreditCertificateAmount"));
        this.root.RemoveChild(oldChild1);
      }
      XmlNode xmlNode2 = this.root.SelectSingleNode("EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY/@Ethnicity");
      if (xmlNode2 is XmlAttribute)
        ((XmlAttribute) xmlNode2).OwnerElement.RemoveAttribute(xmlNode2.Name);
      XmlNode xmlNode3 = this.root.SelectSingleNode("EllieMae/VA_CERTIFICATE_OF_REASONABLE_VALUE");
      if (xmlNode3 != null)
      {
        xmlNode3.ParentNode.RemoveChild(xmlNode3);
        this.createPath("EllieMae/FHA_VA_LOANS").AppendChild(xmlNode3);
      }
      XmlElement xmlElement1 = (XmlElement) null;
      XmlElement xmlElement2 = (XmlElement) null;
      foreach (XmlNode selectNode in this.root.SelectNodes("BORROWER"))
      {
        XmlNodeList childNodes = selectNode.ChildNodes;
        for (int index = 0; index < childNodes.Count; ++index)
        {
          XmlNode xmlNode4 = childNodes.Item(index);
          if (xmlNode4.Name == "MCAW")
            xmlElement1 = (XmlElement) xmlNode4;
          if (xmlNode4.Name == "EllieMae")
            xmlElement2 = (XmlElement) xmlNode4;
        }
        if (xmlElement1 != null && xmlElement2 != null)
        {
          xmlElement1.ParentNode.RemoveChild((XmlNode) xmlElement1);
          XmlElement xmlElement3 = (XmlElement) xmlElement2.AppendChild((XmlNode) xmlElement1);
          XmlAttribute xmlAttribute1 = (XmlAttribute) this.root.SelectSingleNode("EllieMae/MCAW/@OtherDebtsAndObligations");
          if (xmlAttribute1 != null)
          {
            xmlElement3.SetAttribute("OtherDebtsAndObligations", xmlAttribute1.Value);
            xmlAttribute1.OwnerElement.RemoveAttribute(xmlAttribute1.Name);
          }
          XmlAttribute xmlAttribute2 = (XmlAttribute) this.root.SelectSingleNode("EllieMae/MCAW/@TotalMonthlyPayments");
          if (xmlAttribute2 != null)
          {
            xmlElement3.SetAttribute("TotalMonthlyPayments", xmlAttribute2.Value);
            xmlAttribute2.OwnerElement.RemoveAttribute(xmlAttribute2.Name);
          }
          xmlElement1 = (XmlElement) null;
          xmlElement2 = (XmlElement) null;
        }
      }
      this.moveSingleAttribute("EllieMae/MCAW", "TotalMortgagePayment", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/MCAW", "TotalMortgagePayment");
      this.moveSingleAttribute("EllieMae/MCAW", "TotalFixedPaymentForPurchase", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/MCAW", "TotalFixedPaymentForPurchase");
      this.moveSingleAttribute("EllieMae/MCAW", "TotalFixedPaymentForRefinance", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/MCAW", "TotalFixedPaymentForRefinance");
      this.moveSingleAttribute("EllieMae/SELF_EMPLOYED_INCOME/FORM_2106/Depreciation", "Year2", "EllieMae/SELF_EMPLOYED_INCOME/FORM_2106/Depreciation2", "Year2");
      string fieldAt1 = this.GetFieldAt("969");
      this.SetFieldAt("969", this.GetFieldAt("1160"));
      this.SetFieldAt("1160", fieldAt1);
      foreach (XmlAttribute selectNode in this.root.SelectNodes("BORROWER/VA_BORROWER/EllieMae/@OtherTaxDeductionAmount"))
      {
        ((XmlElement) selectNode.OwnerElement.ParentNode).SetAttribute("VALocalTaxAmount", selectNode.Value);
        selectNode.OwnerElement.RemoveAttribute(selectNode.Name);
      }
      foreach (XmlNode selectNode in this.root.SelectNodes("BORROWER/@MaritalStatusType"))
      {
        if (selectNode.Value == "Seperated")
          selectNode.Value = "Separated";
      }
      string[] strArray = new string[4]
      {
        "EllieMae/LOAN_SUBMISSION/@Taxes",
        "EllieMae/LOAN_SUBMISSION/@Hazard",
        "EllieMae/LOAN_SUBMISSION/@MMM_PMI",
        "EllieMae/LOAN_SUBMISSION/@Flood"
      };
      foreach (string xpath in strArray)
      {
        XmlNode xmlNode5 = this.root.SelectSingleNode(xpath);
        if (xmlNode5 != null)
        {
          XmlAttribute xmlAttribute = (XmlAttribute) xmlNode5;
          if (xmlAttribute.Value == "on")
            xmlAttribute.Value = "Y";
        }
      }
      foreach (XmlElement selectNode in this.root.SelectNodes("REO_PROPERTY"))
      {
        if (selectNode.HasAttribute("LiabilityID"))
          selectNode.RemoveAttribute("LiabilityID");
      }
      string fieldAt2 = this.GetFieldAt("1804");
      if (fieldAt2 != string.Empty)
        this.setFieldAtId("1805", fieldAt2, (BorrowerPair) null);
      string fieldAt3 = this.GetFieldAt("LOANSUB.X14");
      if (!(fieldAt3 != string.Empty) || !(fieldAt3.ToUpper() == "ON"))
        return;
      this.setFieldAtId("LOANSUB.X14", "Y", (BorrowerPair) null);
    }

    private void fixEnumerationValues()
    {
      foreach (string[][] valueMapping in Mapping.valueMappings)
      {
        foreach (XmlAttribute selectNode in this.root.SelectNodes(valueMapping[0][0]))
        {
          string str = selectNode.Value;
          for (int index = 1; index < valueMapping.Length; ++index)
          {
            if (str == valueMapping[index][0])
            {
              selectNode.Value = valueMapping[index][1];
              break;
            }
          }
        }
      }
    }

    private void addUnderscoreForIds()
    {
      foreach (string idPath in Mapping.idPaths)
      {
        foreach (XmlNode selectNode in this.root.SelectNodes(idPath))
        {
          if (!selectNode.Value.StartsWith("_"))
            selectNode.Value = "_" + selectNode.Value;
        }
      }
    }

    private void addMissingFields()
    {
      XmlElement xmlElement = (XmlElement) null;
      this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
      if (this.lockRoot == null)
      {
        this.lockRoot = this.xmldoc.CreateElement("LOCK");
        this.root.AppendChild((XmlNode) this.lockRoot);
      }
      int count1 = this.root.SelectNodes("TITLE_HOLDER").Count;
      for (int index = 0; index < 2 - count1; ++index)
        this.root.AppendChild((XmlNode) this.xmldoc.CreateElement("TITLE_HOLDER"));
      string[] strArray1 = new string[7]
      {
        "FirstMortgagePrincipalAndInterest",
        "OtherMortgageLoanPrincipalAndInterest",
        "HazardInsurance",
        "RealEstateTax",
        "MI",
        "HomeownersAssociationDuesAndCondominiumFees",
        "OtherHousingExpense"
      };
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (this.root.SelectSingleNode("PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"" + strArray1[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("PROPOSED_HOUSING_EXPENSE");
          element.SetAttribute("HousingExpenseType", strArray1[index]);
          this.root.AppendChild((XmlNode) element);
        }
      }
      string[] strArray2 = new string[8]
      {
        "GroundRent",
        "FirstMortgagePrincipalAndInterest",
        "OtherMortgageLoanPrincipalAndInterest",
        "HazardInsurance",
        "RealEstateTax",
        "MI",
        "HomeownersAssociationDuesAndCondominiumFees",
        "OtherHousingExpense"
      };
      XmlElement newChild1;
      if ((newChild1 = (XmlElement) this.brwElm.SelectSingleNode("EllieMae")) == null)
      {
        newChild1 = this.xmldoc.CreateElement("EllieMae");
        this.brwElm.AppendChild((XmlNode) newChild1);
      }
      XmlElement newChild2;
      if ((newChild2 = (XmlElement) newChild1.SelectSingleNode("PAIR")) == null)
      {
        newChild2 = this.xmldoc.CreateElement("PAIR");
        newChild1.AppendChild((XmlNode) newChild2);
      }
      if (this.GetBorrowerPairs().Length > 1 && (xmlElement = (XmlElement) newChild2.SelectSingleNode("TAX_4506")) == null)
      {
        XmlElement element = this.xmldoc.CreateElement("TAX_4506");
        element.SetAttribute("Irs4506C", "Y");
        element.SetAttribute("Irs4506CPrintVersion", "Oct2022");
        element.SetAttribute("FormVersion", "4506-COct2022");
        newChild2.AppendChild((XmlNode) element);
      }
      XmlElement newChild3;
      if ((newChild3 = (XmlElement) newChild2.SelectSingleNode("TSUM")) == null)
      {
        newChild3 = this.xmldoc.CreateElement("TSUM");
        newChild2.AppendChild((XmlNode) newChild3);
      }
      for (int index = 0; index < strArray2.Length; ++index)
      {
        if (newChild3.SelectSingleNode("PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"" + strArray2[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("PROPOSED_HOUSING_EXPENSE");
          element.SetAttribute("HousingExpenseType", strArray2[index]);
          newChild3.AppendChild((XmlNode) element);
        }
      }
      string[] strArray3 = new string[7]
      {
        "Base",
        "Overtime",
        "Bonus",
        "Commissions",
        "DividendsInterest",
        "NetRentalIncome",
        "OtherTypesOfIncome"
      };
      for (int index = 0; index < strArray3.Length; ++index)
      {
        if (this.brwElm.SelectSingleNode("CURRENT_INCOME[@IncomeType=\"" + strArray3[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("CURRENT_INCOME");
          element.SetAttribute("IncomeType", strArray3[index]);
          this.brwElm.AppendChild((XmlNode) element);
        }
      }
      for (int index = 0; index < strArray3.Length; ++index)
      {
        if (this.coBrwElm.SelectSingleNode("CURRENT_INCOME[@IncomeType=\"" + strArray3[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("CURRENT_INCOME");
          element.SetAttribute("IncomeType", strArray3[index]);
          this.coBrwElm.AppendChild((XmlNode) element);
        }
      }
      string[] strArray4 = new string[8]
      {
        "Rent",
        "FirstMortgagePrincipalAndInterest",
        "OtherMortgageLoanPrincipalAndInterest",
        "HazardInsurance",
        "RealEstateTax",
        "MI",
        "HomeownersAssociationDuesAndCondominiumFees",
        "OtherHousingExpense"
      };
      for (int index = 0; index < strArray4.Length; ++index)
      {
        if (this.brwElm.SelectSingleNode("PRESENT_HOUSING_EXPENSE[@HousingExpenseType=\"" + strArray4[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("PRESENT_HOUSING_EXPENSE");
          element.SetAttribute("HousingExpenseType", strArray4[index]);
          this.brwElm.AppendChild((XmlNode) element);
        }
      }
      string[] strArray5 = new string[4]
      {
        "TotalMonthlyIncomeNotIncludingNetRentalIncome",
        "TotalPresentHousingExpense",
        "SubtotalLiquidAssetsNotIncludingGift",
        "TotalLiabilitesBalance"
      };
      for (int index = 0; index < strArray5.Length; ++index)
      {
        if (this.brwElm.SelectSingleNode("SUMMARY[@_AmountType=\"" + strArray5[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("SUMMARY");
          element.SetAttribute("_AmountType", strArray5[index]);
          this.brwElm.AppendChild((XmlNode) element);
        }
      }
      if (this.coBrwElm.SelectSingleNode("SUMMARY[@_AmountType=\"" + strArray5[0] + "\"]") == null)
      {
        XmlElement element = this.xmldoc.CreateElement("SUMMARY");
        element.SetAttribute("_AmountType", strArray5[0]);
        this.coBrwElm.AppendChild((XmlNode) element);
      }
      string[] strArray6 = new string[3]
      {
        "LifeInsurance",
        "RetirementFund",
        "NetWorthOfBusinessOwned"
      };
      for (int index = 0; index < strArray6.Length; ++index)
      {
        if (this.root.SelectSingleNode("ASSET" + this.brwPredicate + "[@_Type=\"" + strArray6[index] + "\"]") == null)
        {
          XmlElement element = this.xmldoc.CreateElement("ASSET");
          Mapping.AddEntityId(element);
          element.SetAttribute("_Type", strArray6[index]);
          element.SetAttribute("BorrowerID", this.brwId);
          this.root.AppendChild((XmlNode) element);
        }
      }
      int count2 = this.root.SelectNodes("ASSET" + this.brwPredicate + "[@_Type=\"Stock\"]").Count;
      for (int index = 0; index < 4 - count2; ++index)
      {
        XmlElement element = this.xmldoc.CreateElement("ASSET");
        Mapping.AddEntityId(element);
        element.SetAttribute("_Type", "Stock");
        element.SetAttribute("BorrowerID", this.brwId);
        this.root.AppendChild((XmlNode) element);
      }
      int count3 = this.root.SelectNodes("ASSET" + this.brwPredicate + "[@_Type=\"Automobile\"]").Count;
      for (int index = 0; index < 3 - count3; ++index)
      {
        XmlElement element = this.xmldoc.CreateElement("ASSET");
        Mapping.AddEntityId(element);
        element.SetAttribute("_Type", "Automobile");
        element.SetAttribute("BorrowerID", this.brwId);
        this.root.AppendChild((XmlNode) element);
      }
      int count4 = this.root.SelectNodes("ASSET" + this.brwPredicate + "[@_Type=\"CashOnHand\"]").Count;
      for (int index = 0; index < 2 - count4; ++index)
      {
        XmlElement element = this.xmldoc.CreateElement("ASSET");
        Mapping.AddEntityId(element);
        element.SetAttribute("_Type", "CashOnHand");
        element.SetAttribute("BorrowerID", this.brwId);
        this.root.AppendChild((XmlNode) element);
      }
      int count5 = this.root.SelectNodes("ASSET" + this.brwPredicate + "[@_Type=\"OtherNonLiquidAssets\"]").Count;
      for (int index = 0; index < 6 - count5; ++index)
      {
        XmlElement element = this.xmldoc.CreateElement("ASSET");
        Mapping.AddEntityId(element);
        element.SetAttribute("_Type", "OtherNonLiquidAssets");
        element.SetAttribute("BorrowerID", this.brwId);
        this.root.AppendChild((XmlNode) element);
      }
      int count6 = this.root.SelectNodes("LIABILITY" + this.brwPredicate + "[@_Type=\"JobRelatedExpenses\"]").Count;
      for (int index = 0; index < 2 - count6; ++index)
      {
        XmlElement element = this.xmldoc.CreateElement("LIABILITY");
        Mapping.AddEntityId(element);
        element.SetAttribute("_Type", "JobRelatedExpenses");
        element.SetAttribute("BorrowerID", this.brwId);
        this.root.AppendChild((XmlNode) element);
      }
      if (this.root.SelectSingleNode("LIABILITY" + this.brwPredicate + "[@_Type=\"Alimony\"]") != null)
        return;
      XmlElement element1 = this.xmldoc.CreateElement("LIABILITY");
      Mapping.AddEntityId(element1);
      element1.SetAttribute("_Type", "Alimony");
      element1.SetAttribute("BorrowerID", this.brwId);
      this.root.AppendChild((XmlNode) element1);
    }

    private void cleanupXml31()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.1)
        return;
      this.versionMigrationOccured = true;
      this.EMXMLVersionID = "3.1";
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        this.SetBorrowerPair(borrowerPairs[index]);
        string str1 = "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/";
        string str2 = "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/";
        foreach (string[] strArray in Mapping.borAttr)
        {
          XmlNode oldNode = strArray[0] == "203" || strArray[0] == "1736" || strArray[0] == "1737" || strArray[0] == "1816" ? this.root.SelectSingleNode(str2 + strArray[1]) : this.root.SelectSingleNode(str1 + strArray[1]);
          if (oldNode != null)
          {
            this.SetFieldAt(strArray[0], oldNode.Value);
            this.removeNode(oldNode, strArray[1]);
          }
        }
        if (index == 0)
        {
          foreach (string[] strArray in Mapping.ellieAttr)
          {
            XmlNode oldNode = this.root.SelectSingleNode(strArray[1]);
            if (oldNode != null)
            {
              this.SetFieldAt(strArray[0], oldNode.Value);
              this.removeNode(oldNode, strArray[1]);
            }
          }
          this.moveNode31("EllieMae/TAX_4506", str1 + "EllieMae/PAIR");
          this.moveNode31("EllieMae/SELF_EMPLOYED_INCOME", str1 + "EllieMae/PAIR");
        }
        this.moveNode31(str1 + "EllieMae/REO_SECTION", str1 + "EllieMae/PAIR");
        this.moveNode31(str1 + "EllieMae/MCAW", str1 + "EllieMae/PAIR");
        this.moveNode31(str1 + "EllieMae/LOAN_SUBMISSION", str1 + "EllieMae/PAIR");
        this.swapBorrowerNodes("BORROWER[@BorrowerID=\"BORID\"]", "/VA_BORROWER", borrowerPairs[index]);
        XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/MCAW");
        if (xmlNodeList.Count > 1)
        {
          XmlElement xmlElement = (XmlElement) xmlNodeList[0];
          XmlNode oldChild = xmlNodeList[1];
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) oldChild.Attributes)
            xmlElement.SetAttribute(attribute.Name, attribute.Value);
          oldChild.ParentNode.RemoveChild(oldChild);
        }
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae");
        if (xmlElement1 != null)
        {
          if (xmlElement1.HasAttribute("BorrowerGenderCode"))
            xmlElement1.RemoveAttribute("BorrowerGenderCode");
          if (xmlElement1.HasAttribute("BorrowerRaceCode"))
            xmlElement1.RemoveAttribute("BorrowerRaceCode");
        }
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae");
        if (xmlElement2 != null)
        {
          if (xmlElement2.HasAttribute("CoborrowerGenderCode"))
            xmlElement2.RemoveAttribute("CoborrowerGenderCode");
          if (xmlElement2.HasAttribute("CoborrowerRaceCode"))
            xmlElement2.RemoveAttribute("CoborrowerRaceCode");
        }
      }
      switch (this.GetFieldAt("MS.STATUS"))
      {
        case "started":
          this.SetFieldAt("MS.STATUS", "Started");
          break;
        case "Processing":
          this.SetFieldAt("MS.STATUS", "Sent to processing");
          break;
        case "Docs Signed":
          this.SetFieldAt("MS.STATUS", "Doc signed");
          break;
      }
      this.fixMilestoneDates();
    }

    private void moveNode31(string oldPath, string newPath)
    {
      XmlNode xmlNode = this.root.SelectSingleNode(oldPath);
      if (xmlNode == null)
        return;
      xmlNode.ParentNode.RemoveChild(xmlNode);
      ((XmlElement) this.root.SelectSingleNode(newPath) ?? this.createPath(newPath)).AppendChild(xmlNode);
    }

    private void fixMilestoneDates()
    {
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/LOG");
      if (xmlElement == null)
        return;
      lock (Mapping.msMap)
      {
        if (Mapping.msMap.Count == 0)
        {
          Mapping.msMap.Add((object) "Started", (object) "MS.START");
          Mapping.msMap.Add((object) "Processing", (object) "MS.PROC");
          Mapping.msMap.Add((object) "submittal", (object) "MS.SUB");
          Mapping.msMap.Add((object) "Approval", (object) "MS.APP");
          Mapping.msMap.Add((object) "Docs Signing", (object) "MS.DOC");
          Mapping.msMap.Add((object) "Funding", (object) "MS.FUN");
          Mapping.msMap.Add((object) "Completion", (object) "MS.CLO");
        }
      }
      foreach (XmlElement selectNode in xmlElement.SelectNodes("Milestone"))
      {
        string attribute = selectNode.GetAttribute("Stage");
        string ms = (string) Mapping.msMap[(object) attribute];
        if (ms != null)
        {
          DateTime date = this.GetDate(selectNode, "Date");
          if (date.Date != DateTime.MaxValue.Date && date.Date != DateTime.MinValue.Date)
          {
            string val = date.ToString("M/d/yy");
            if (selectNode.GetAttribute("Done") == "Y")
            {
              if (this.GetFieldAt(ms) == this.GetFieldAt(ms + ".DUE"))
                this.SetFieldAt(ms + ".DUE", val);
              this.SetFieldAt(ms, val);
            }
            else
            {
              this.SetFieldAt(ms, "");
              this.SetFieldAt(ms + ".DUE", date.ToString("M/d/yy"));
            }
          }
        }
      }
    }

    private void cleanupXml33()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.3)
        return;
      this.versionMigrationOccured = true;
      string xpath = "EllieMae/CLOSING_COST[1]/AssumptionFee";
      string str = "EllieMae/CLOSING_COST[1]/UserDefinedFee_822";
      XmlNode xmlNode = this.root.SelectSingleNode(xpath);
      if (xmlNode != null)
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(str) ?? this.createPath(str);
        if (xmlElement != null)
        {
          bool flag = false;
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlNode.Attributes)
          {
            if (attribute.Name == "PaidToName")
            {
              if (attribute.Value != "" && attribute.Value != null)
                xmlElement.SetAttribute("Description", "Rate Lock Fee to: " + attribute.Value);
              else
                xmlElement.SetAttribute("Description", "Rate Lock Fee");
              flag = true;
            }
            else
              xmlElement.SetAttribute(attribute.Name, attribute.Value);
          }
          if (xmlNode.Attributes.Count > 0 && !flag)
            xmlElement.SetAttribute("Description", "Rate Lock Fee");
        }
        xmlNode.RemoveAll();
      }
      this.EMXMLVersionID = "3.3";
    }

    private void cleanupXml361(LoanMigrationData migrationData)
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.61)
        return;
      this.versionMigrationOccured = true;
      if (migrationData == null)
        throw new Exception("MigrationData is required to access loans with version < 3.61");
      XmlElement ellieMaeRoot = this.GetEllieMaeRoot();
      XmlElement nonSystemLogRoot = this.GetNonSystemLogRoot();
      XmlElement systemLogRoot = this.GetSystemLogRoot(this.systemId);
      XmlNodeList xmlNodeList = nonSystemLogRoot.SelectNodes(".//Milestone");
      Hashtable hashtable = new Hashtable();
      string str1 = "";
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      foreach (XmlElement oldChild in xmlNodeList)
      {
        oldChild.RemoveAttribute("SysID");
        string attribute = oldChild.GetAttribute("Stage");
        if (EllieMae.EMLite.Common.Milestone.IsCoreMilestone(attribute))
          str1 = attribute;
        else
          insensitiveHashtable[(object) attribute] = (object) str1;
        XmlElement xmlElement = (XmlElement) oldChild.CloneNode(true);
        string roleType = (string) null;
        if (this.migrateMilestoneElement361(xmlElement, ellieMaeRoot, migrationData, ref roleType))
        {
          systemLogRoot.AppendChild((XmlNode) xmlElement);
          if ((roleType ?? "") != "")
            hashtable[(object) roleType] = (object) true;
        }
        oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
      }
      foreach (string legacyPersona in LoanMigrationData.LegacyPersonas)
      {
        if (!hashtable.Contains((object) legacyPersona) && migrationData.RealWorldRoleMap.Contains((object) legacyPersona))
          this.migrateMilestoneFreeRole361(legacyPersona, systemLogRoot, ellieMaeRoot, migrationData);
      }
      foreach (XmlElement selectNode in nonSystemLogRoot.SelectNodes(".//Record[@Type='Document' or @Type='Fax']"))
      {
        if (selectNode.GetAttribute("AttachmentID") != null)
        {
          selectNode.SetAttribute("Guid", selectNode.GetAttribute("AttachmentID"));
          selectNode.RemoveAttribute("AttachmentID");
        }
        string str2 = selectNode.GetAttribute("Stage") ?? "";
        if (str2 != "" && this.logList.GetMilestoneByName(str2) == null)
        {
          string str3 = string.Concat(insensitiveHashtable[(object) str2]);
          if (str3 == "Started")
            str3 = "";
          selectNode.SetAttribute("Stage", str3);
        }
      }
      foreach (XmlNode selectNode in nonSystemLogRoot.SelectNodes(".//Record[@Type='Document' or @Type='Verif']"))
      {
        systemLogRoot.AppendChild(selectNode.CloneNode(true));
        selectNode.ParentNode.RemoveChild(selectNode);
      }
      string[] strArray = new string[3]
      {
        "Conv",
        "Log",
        "Insurance"
      };
      foreach (string str4 in strArray)
      {
        foreach (XmlElement selectNode in nonSystemLogRoot.SelectNodes("./Record[@Type='" + str4 + "']"))
        {
          DateTime date = this.GetDate(selectNode, "Due");
          if (date == DateTime.MinValue)
            date = this.GetDate(selectNode, "Date");
          bool flag1 = this.GetAttr(selectNode, "AlertLO") == "Y";
          bool flag2 = this.GetAttr(selectNode, "AlertLP") == "Y";
          bool flag3 = this.GetAttr(selectNode, "AlertCL") == "Y";
          DateTime followedUpDate = this.GetAttr(selectNode, "FollowedUp") == "Y" ? date : DateTime.MinValue;
          string attr = this.GetAttr(selectNode, "UserId");
          selectNode.RemoveAttribute("Due");
          selectNode.RemoveAttribute("AlertLO");
          selectNode.RemoveAttribute("AlertLP");
          selectNode.RemoveAttribute("AlertCL");
          selectNode.RemoveAttribute("FollowedUp");
          if (flag3 && migrationData.RealWorldRoleMap.Contains((object) "CL"))
            this.writeAlertElement(selectNode, this.systemId, new LogAlert(attr, ((LoanMigrationData.RoleData) migrationData.RealWorldRoleMap[(object) "CL"]).RoleID, date, followedUpDate));
          if (flag2 && migrationData.RealWorldRoleMap.Contains((object) "LP"))
            this.writeAlertElement(selectNode, this.systemId, new LogAlert(attr, ((LoanMigrationData.RoleData) migrationData.RealWorldRoleMap[(object) "LP"]).RoleID, date, followedUpDate));
          if (flag1 && migrationData.RealWorldRoleMap.Contains((object) "LO"))
            this.writeAlertElement(selectNode, this.systemId, new LogAlert(attr, ((LoanMigrationData.RoleData) migrationData.RealWorldRoleMap[(object) "LO"]).RoleID, date, followedUpDate));
        }
      }
      string str5 = migrationData == null ? Guid.Empty.ToString() : this.systemId;
      ((XmlElement) this.root.SelectSingleNode("EllieMae/FormList"))?.SetAttribute("SysID", str5);
      this.EMXMLVersionID = "3.61";
    }

    private void migrateMilestoneFreeRole361(
      string roleType,
      XmlElement systemNodeElm,
      XmlElement ellieMaeRoot,
      LoanMigrationData migrationData)
    {
      LoanMigrationData.RoleData realWorldRole = (LoanMigrationData.RoleData) migrationData.RealWorldRoleMap[(object) roleType];
      XmlElement element = ellieMaeRoot.OwnerDocument.CreateElement("Record");
      element.SetAttribute("Type", MilestoneFreeRoleLog.XmlType);
      element.SetAttribute("Guid", Guid.NewGuid().ToString());
      element.SetAttribute("RoleID", string.Concat((object) realWorldRole.RoleID));
      element.SetAttribute("RoleName", realWorldRole.RoleName);
      this.migrateLoanAssociateInfo(element, ellieMaeRoot, roleType);
      systemNodeElm.AppendChild((XmlNode) element);
    }

    private bool migrateMilestoneElement361(
      XmlElement e,
      XmlElement ellieMaeRoot,
      LoanMigrationData migrationData,
      ref string roleType)
    {
      string str = e.GetAttribute("Stage") ?? "";
      EllieMae.EMLite.Workflow.Milestone milestone1 = (EllieMae.EMLite.Workflow.Milestone) null;
      foreach (EllieMae.EMLite.Workflow.Milestone milestone2 in migrationData.MilestoneList)
      {
        if (milestone2.Name == str)
          milestone1 = milestone2;
      }
      if (milestone1 == null)
        return false;
      e.SetAttribute("Guid", Guid.NewGuid().ToString());
      e.SetAttribute("MilestoneID", milestone1.MilestoneID);
      LoanMigrationData.MilestoneData milestoneData = (LoanMigrationData.MilestoneData) migrationData.MilestoneDataMap[(object) milestone1.MilestoneID];
      if (milestoneData == null || milestoneData.AssociatedRole == null || milestoneData.AssociatedRole.RoleID < RoleInfo.FileStarter.ID)
        return true;
      e.SetAttribute("RoleID", string.Concat((object) milestoneData.AssociatedRole.RoleID));
      e.SetAttribute("RoleName", milestoneData.AssociatedRole.RoleName);
      roleType = milestoneData.AssociatedRole.RealWorldRoleType;
      this.migrateLoanAssociateInfo(e, ellieMaeRoot, roleType);
      return true;
    }

    private void migrateLoanAssociateInfo(
      XmlElement target,
      XmlElement ellieMaeRoot,
      string roleType)
    {
      switch (roleType)
      {
        case "LO":
          this.migrateLOLPLC((XmlNode) target, ellieMaeRoot.SelectSingleNode(".//LOAN_OFFICER"), "Officer");
          break;
        case "LP":
          this.migrateLOLPLC((XmlNode) target, ellieMaeRoot.SelectSingleNode(".//LOAN_PROCESSOR"), "Processor");
          break;
        case "CL":
          this.migrateLOLPLC((XmlNode) target, ellieMaeRoot.SelectSingleNode(".//LOAN_CLOSER"), "Closer");
          break;
      }
    }

    internal XmlElement GetEllieMaeRoot()
    {
      return (XmlElement) this.root.SelectSingleNode("EllieMae") ?? (XmlElement) this.root.AppendChild((XmlNode) this.xmldoc.CreateElement("EllieMae"));
    }

    internal XmlElement GetSystemLogRoot(string systemId)
    {
      XmlElement systemLogRoot = (XmlElement) this.root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + systemId + "']");
      if (systemLogRoot != null)
        return systemLogRoot;
      XmlElement element = this.xmldoc.CreateElement("SystemLog");
      element.SetAttribute("SysID", systemId);
      return (XmlElement) this.GetEllieMaeRoot().InsertBefore((XmlNode) element, (XmlNode) this.GetNonSystemLogRoot());
    }

    internal XmlElement GetNonSystemLogRoot()
    {
      return (XmlElement) this.root.SelectSingleNode("EllieMae/LOG") ?? (XmlElement) this.GetEllieMaeRoot().AppendChild((XmlNode) this.xmldoc.CreateElement("LOG"));
    }

    private void writeAlertElement(XmlElement parentNode, string systemId, LogAlert alert)
    {
      alert.SetSystemID(systemId);
      alert.ToXml(parentNode);
    }

    private void migrateLOLPLC(XmlNode newNode, XmlNode sourceNode, string type)
    {
      if (sourceNode == null)
        return;
      string name = "";
      string str1 = "";
      string str2 = "";
      string str3 = "";
      string str4 = "";
      string str5 = "";
      switch (type)
      {
        case "Officer":
          name = "OfficerID";
          break;
        case "Processor":
          name = "ProcessorID";
          break;
        case "Closer":
          name = "CloserID";
          break;
      }
      if (sourceNode.Attributes[name] != null)
        str2 = sourceNode.Attributes[name].Value;
      XmlNode xmlNode = sourceNode.SelectSingleNode(".//ContactInfo");
      if (xmlNode != null)
      {
        if (xmlNode.Attributes["Name"] != null)
          str1 = xmlNode.Attributes["Name"].Value;
        if (xmlNode.Attributes["Email"] != null)
          str3 = xmlNode.Attributes["Email"].Value;
        if (xmlNode.Attributes["Phone"] != null)
          str4 = xmlNode.Attributes["Phone"].Value;
        if (xmlNode.Attributes["Fax"] != null)
          str5 = xmlNode.Attributes["Fax"].Value;
      }
      if (newNode.Attributes["LoanAssociateID"] != null)
      {
        newNode.Attributes["LoanAssociateID"].Value = str2;
      }
      else
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("LoanAssociateID");
        attribute.Value = str2;
        newNode.Attributes.Append(attribute);
      }
      if (newNode.Attributes["LoanAssociateName"] != null)
      {
        newNode.Attributes["LoanAssociateName"].Value = str1;
      }
      else
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("LoanAssociateName");
        attribute.Value = str1;
        newNode.Attributes.Append(attribute);
      }
      if (newNode.Attributes["LoanAssociateEmail"] != null)
      {
        newNode.Attributes["LoanAssociateEmail"].Value = str3;
      }
      else
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("LoanAssociateEmail");
        attribute.Value = str3;
        newNode.Attributes.Append(attribute);
      }
      if (newNode.Attributes["LoanAssociatePhone"] != null)
      {
        newNode.Attributes["LoanAssociatePhone"].Value = str4;
      }
      else
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("LoanAssociatePhone");
        attribute.Value = str4;
        newNode.Attributes.Append(attribute);
      }
      if (newNode.Attributes["LoanAssociateFax"] != null)
      {
        newNode.Attributes["LoanAssociateFax"].Value = str5;
      }
      else
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("LoanAssociateFax");
        attribute.Value = str5;
        newNode.Attributes.Append(attribute);
      }
    }

    private void cleanupXml366()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.66)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY");
      if (xmlElement1 != null)
      {
        string attribute = xmlElement1.GetAttribute("CreditScore");
        if (attribute != string.Empty)
          xmlElement1.SetAttribute("CreditScore", string.Concat((object) Utils.ParseInt((object) attribute)));
      }
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("BORROWER/EllieMae/PAIR/FHA_VA_LOANS");
      if (xmlNodeList1 != null)
      {
        foreach (XmlElement xmlElement2 in xmlNodeList1)
        {
          if (xmlElement2.HasAttribute("VACreditStandards"))
          {
            string attribute = xmlElement2.GetAttribute("VACreditStandards");
            if (attribute.ToLower() == "yes")
              xmlElement2.SetAttribute("VACreditStandards", "Y");
            else if (attribute.ToLower() == "no")
              xmlElement2.SetAttribute("VACreditStandards", "N");
          }
        }
      }
      string str1 = string.Empty;
      XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement3 != null)
        str1 = xmlElement3.GetAttribute("MS_STATUSDATE") ?? "";
      XmlNodeList xmlNodeList2 = this.root.SelectNodes("EllieMae/SystemLog[@SysID='" + this.systemId + "']//Milestone");
      if (str1 == string.Empty)
      {
        if (xmlNodeList2 != null)
        {
          foreach (XmlElement xmlElement4 in xmlNodeList2)
          {
            if ((xmlElement4.GetAttribute("Done") ?? "") == "Y")
              str1 = xmlElement4.GetAttribute("Date");
            else
              break;
          }
        }
        if (str1 != string.Empty)
          ((XmlElement) this.root.SelectSingleNode("EllieMae"))?.SetAttribute("MS_STATUSDATE", str1);
      }
      if (xmlNodeList2 != null)
      {
        DateTime minValue1 = DateTime.MinValue;
        DateTime minValue2 = DateTime.MinValue;
        TimeSpan timeSpan;
        int days;
        for (int i = 0; i < xmlNodeList2.Count - 1; ++i)
        {
          XmlElement elm1 = (XmlElement) xmlNodeList2[i];
          XmlElement elm2 = (XmlElement) xmlNodeList2[i + 1];
          if (elm1 != null && elm2 != null)
          {
            DateTime date1 = Utils.ParseDate((object) this.GetAttr(elm1, "Date"));
            DateTime date2 = Utils.ParseDate((object) this.GetAttr(elm2, "Date"));
            if (date1 == DateTime.MinValue || date1 == DateTime.MaxValue || date2 == DateTime.MinValue || date2 == DateTime.MaxValue || (elm1.GetAttribute("Done") ?? "") != "Y" || (elm2.GetAttribute("Done") ?? "") != "Y")
            {
              elm1.SetAttribute("Duration", "-1");
            }
            else
            {
              timeSpan = date2.Subtract(date1);
              XmlElement xmlElement5 = elm1;
              days = timeSpan.Days;
              string str2 = days.ToString();
              xmlElement5.SetAttribute("Duration", str2);
            }
          }
        }
        XmlElement xmlElement6 = (XmlElement) this.root.SelectSingleNode("EllieMae");
        xmlElement6?.SetAttribute("MS_LOANDURATION", "");
        XmlElement elm3 = (XmlElement) xmlNodeList2[0];
        XmlElement elm4 = (XmlElement) xmlNodeList2[xmlNodeList2.Count - 1];
        if (elm3 != null && elm4 != null && this.GetAttr(elm3, "Done") == "Y" && this.GetAttr(elm4, "Done") == "Y")
        {
          DateTime date3 = Utils.ParseDate((object) this.GetAttr(elm3, "Date"));
          DateTime date4 = Utils.ParseDate((object) this.GetAttr(elm4, "Date"));
          if (date3 != DateTime.MinValue && date4 != DateTime.MinValue)
          {
            timeSpan = date4.Subtract(date3);
            XmlElement xmlElement7 = xmlElement6;
            days = timeSpan.Days;
            string str3 = days.ToString();
            xmlElement7.SetAttribute("MS_LOANDURATION", str3);
          }
        }
      }
      XmlElement elm5 = (XmlElement) this.root.SelectSingleNode("EllieMae/REGZ");
      if (elm5 != null)
      {
        if (this.GetAttr(elm5, "MonthsOfMIPrepaid") != string.Empty)
        {
          elm5.SetAttribute("MIPrepaidIndicator", "Y");
        }
        else
        {
          XmlElement elm6 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/IMPOUND_TABLE");
          if (elm6 != null && Utils.ParseDouble((object) this.GetAttr(elm6, "MortgInsPremRate")) > 0.0)
            elm5.SetAttribute("MonthsOfMIPrepaid", "12");
        }
      }
      XmlElement elm7 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/MortgageInsurancePremium");
      if (elm7 != null)
      {
        double num1 = 0.0;
        if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") == "VA")
        {
          XmlElement elm8 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/VAFundingFee");
          if (elm8 != null)
            num1 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.GetAttr(elm8, "BorPaidAmount")) + Utils.ParseDouble((object) this.GetAttr(elm8, "SellerPaidAmount")), 2);
        }
        else
          num1 = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.GetAttr(elm7, "BorPaidAmount")) + Utils.ParseDouble((object) this.GetAttr(elm7, "SellerPaidAmount")), 2);
        XmlElement elm9 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/IMPOUND_TABLE/BASE_TYPE[4]");
        if (elm9 != null)
        {
          double num2 = 0.0;
          switch (this.GetAttr(elm9, "Type"))
          {
            case "Purchase Price":
              num2 = Utils.ParseDouble((object) this.readNodeValue("TRANSACTION_DETAIL/@PurchasePriceAmount"));
              break;
            case "Appraisal Value":
              num2 = Utils.ParseDouble((object) this.readNodeValue("ADDITIONAL_CASE_DATA/TRANSMITTAL_DATA/@PropertyAppraisedValueAmount"));
              break;
            case "Base Loan Amount":
              num2 = Utils.ParseDouble((object) this.readNodeValue("MORTGAGE_TERMS/@BorrowerRequestedLoanAmount"));
              break;
            case "Loan Amount":
              num2 = Utils.ParseDouble((object) this.readNodeValue("MORTGAGE_TERMS/@BaseLoanAmount"));
              break;
          }
          int num3 = Utils.ParseInt((object) this.readNodeValue("EllieMae/REGZ/@MonthsOfMIPrepaid"));
          double num4 = Utils.ParseDouble((object) this.readNodeValue("EllieMae/CLOSING_COST[1]/IMPOUND_TABLE/@MortgInsPremRate"));
          if (num2 > 0.0 && num4 > 0.0)
          {
            double num5 = num3 != 12 ? Utils.ArithmeticRounding(num2 * num4 / 100.0 / 12.0 * (double) num3, 2) : Utils.ArithmeticRounding(num2 * num4 / 100.0, 2);
            if (num5 != num1)
            {
              XmlElement xmlElement8 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"337\"]") ?? this.createPath("LOCK/FIELD");
              double num6 = num5 - Utils.ParseDouble((object) this.GetAttr(elm7, "SellerPaidAmount"));
              xmlElement8.SetAttribute("id", "337");
              xmlElement8.SetAttribute("val", num6.ToString("N2"));
            }
          }
        }
      }
      if (this.toDouble(this.originalLoanVersion) < 3.55 || this.toDouble(this.originalLoanVersion) >= 3.6)
      {
        XmlElement elm10 = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (elm10 != null && (this.GetAttr(elm10, "MIPPaidInCashByBorrower") ?? "") == string.Empty)
          elm10.SetAttribute("MIPPaidInCashByBorrower", this.GetAttr(elm10, "MIPPaidInCash"));
      }
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (borrowerPairs != null && borrowerPairs.Length != 0)
      {
        this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae", "MinFICO", "EllieMae/RateLock/Request/LoanInfo", "MinFICO", true);
        this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[0].CoBorrower.Id + "\"]/EllieMae", "MinFICO", "EllieMae/RateLock/Request/LoanInfo", "MinFICO2", true);
        this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/PAIR", "PropertyUsageType", "EllieMae/RateLock/Request/LoanInfo", "PropertyUsageType", true);
        int num = 0;
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          ++num;
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]", "_FirstName", "EllieMae/RateLock/Request/LoanInfo", "_FirstName" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]", "_LastName", "EllieMae/RateLock/Request/LoanInfo", "_LastName" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]", "_SSN", "EllieMae/RateLock/Request/LoanInfo", "_SSN" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "ExperianScore", "EllieMae/RateLock/Request/LoanInfo", "ExperianScore" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "TransUnionScore", "EllieMae/RateLock/Request/LoanInfo", "TransUnionScore" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "EquifaxScore", "EllieMae/RateLock/Request/LoanInfo", "EquifaxScore" + num.ToString(), true);
          ++num;
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]", "_FirstName", "EllieMae/RateLock/Request/LoanInfo", "_FirstName" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]", "_LastName", "EllieMae/RateLock/Request/LoanInfo", "_LastName" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]", "_SSN", "EllieMae/RateLock/Request/LoanInfo", "_SSN" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "ExperianScore", "EllieMae/RateLock/Request/LoanInfo", "ExperianScore" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "TransUnionScore", "EllieMae/RateLock/Request/LoanInfo", "TransUnionScore" + num.ToString(), true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "EquifaxScore", "EllieMae/RateLock/Request/LoanInfo", "EquifaxScore" + num.ToString(), true);
        }
      }
      this.copySingleAttribute("EllieMae", "LoanProgram", "EllieMae/RateLock/Request/LoanInfo", "LoanProgram", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "LoanDocumentationType", "EllieMae/RateLock/Request/LoanInfo", "LoanDocumentationType", true);
      this.copySingleAttribute("EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY", "CreditScore", "EllieMae", "CreditScoreToUse", true);
      this.copySingleAttribute("PROPERTY", "_StreetAddress", "EllieMae/RateLock/Request/LoanInfo", "_StreetAddress", true);
      this.copySingleAttribute("PROPERTY", "_City", "EllieMae/RateLock/Request/LoanInfo", "_City", true);
      this.copySingleAttribute("PROPERTY", "_State", "EllieMae/RateLock/Request/LoanInfo", "_State", true);
      this.copySingleAttribute("PROPERTY", "_PostalCode", "EllieMae/RateLock/Request/LoanInfo", "_PostalCode", true);
      this.copySingleAttribute("PROPERTY", "_County", "EllieMae/RateLock/Request/LoanInfo", "_County", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "GSEPropertyType", "EllieMae/RateLock/Request/LoanInfo", "GSEPropertyType", true);
      this.copySingleAttribute("ADDITIONAL_CASE_DATA/TRANSMITTAL_DATA", "PropertyEstimatedValueAmount", "EllieMae/RateLock/Request/LoanInfo", "PropertyEstimatedValueAmount", true);
      this.copySingleAttribute("ADDITIONAL_CASE_DATA/TRANSMITTAL_DATA", "PropertyAppraisedValueAmount", "EllieMae/RateLock/Request/LoanInfo", "PropertyAppraisedValueAmount", true);
      this.copySingleAttribute("LOAN_PURPOSE", "_Type", "EllieMae/RateLock/Request/LoanInfo", "_Type", true);
      this.copySingleAttribute("MORTGAGE_TERMS", "MortgageType", "EllieMae/RateLock/Request/LoanInfo", "MortgageType", true);
      this.copySingleAttribute("MORTGAGE_TERMS", "LoanAmortizationType", "EllieMae/RateLock/Request/LoanInfo", "LoanAmortizationType", true);
      this.copySingleAttribute("EllieMae/REGZ/GPM", "Rate", "EllieMae/RateLock/Request/LoanInfo", "GPMRate", true);
      this.copySingleAttribute("EllieMae/REGZ/GPM", "Years", "EllieMae/RateLock/Request/LoanInfo", "GPMYears", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "FNMProductPlanIndentifier", "EllieMae/RateLock/Request/LoanInfo", "FNMProductPlanIndentifier", true);
      this.copySingleAttribute("MORTGAGE_TERMS", "OtherAmortizationTypeDescription", "EllieMae/RateLock/Request/LoanInfo", "OtherAmortizationTypeDescription", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "LienPriorityType", "EllieMae/RateLock/Request/LoanInfo", "LienPriorityType", true);
      this.copySingleAttribute("MORTGAGE_TERMS", "LoanAmortizationTermMonths", "EllieMae/RateLock/Request/LoanInfo", "LoanAmortizationTermMonths", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "BalloonLoanMaturityTermMonths", "EllieMae/RateLock/Request/LoanInfo", "BalloonLoanMaturityTermMonths", true);
      this.copySingleAttribute("EllieMae/RateLock", "ImpoundWavied", "EllieMae/RateLock/Request/LoanInfo", "ImpoundWavied", true);
      this.copySingleAttribute("EllieMae/RateLock", "ImpoundType", "EllieMae/RateLock/Request/LoanInfo", "ImpoundType", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "PrepaymentPenaltyIndicator", "EllieMae/RateLock/Request/LoanInfo", "PrepayPenalty", true);
      this.copySingleAttribute("EllieMae/RateLock", "PenaltyTerm", "EllieMae/RateLock/Request/LoanInfo", "PenaltyTerm", true);
      this.copySingleAttribute("MORTGAGE_TERMS", "BaseLoanAmount", "EllieMae/RateLock/Request/LoanInfo", "BorrowerRequestedLoanAmount", true);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "LoanScheduledClosingDate", "EllieMae/RateLock/Request/LoanInfo", "LoanScheduledClosingDate", true);
      this.copySingleAttribute("EllieMae/TemplateFiles", "LoanProgramFile", "EllieMae/RateLock/Request/LoanInfo", "LoanProgramFile", true);
      this.copySingleAttribute("EllieMae", "FirstSubordinateAmount", "EllieMae/RateLock/Request/LoanInfo", "FirstSubordinateAmount", true);
      this.copySingleAttribute("EllieMae", "SecondSubordinateAmount", "EllieMae/RateLock/Request/LoanInfo", "SecondSubordinateAmount", true);
      this.copySingleAttribute("EllieMae/TSUM", "UnpaidBalance", "EllieMae/RateLock/Request/LoanInfo", "OtherSubordinateAmount", true);
      this.copySingleAttribute("TRANSACTION_DETAIL", "PurchasePriceAmount", "EllieMae/RateLock/Request/LoanInfo", "PurchasePriceAmount", true);
      this.EMXMLVersionID = "3.66";
    }

    private string readNodeValue(string nodePath)
    {
      int num = nodePath.LastIndexOf('@');
      if (num == -1)
        return string.Empty;
      string xpath = nodePath.Substring(0, num - 1);
      string attr = nodePath.Substring(num + 1);
      XmlElement elm = (XmlElement) this.root.SelectSingleNode(xpath);
      return elm != null ? this.GetAttr(elm, attr) : string.Empty;
    }

    private void cleanupXml60()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.0)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/HMDA");
      if (xmlElement != null)
      {
        switch (xmlElement.GetAttribute("PropertyType"))
        {
          case "One-to-four Family":
            xmlElement.SetAttribute("PropertyType", "One-to-fourFamily");
            break;
          case "Manufactured Housing":
            xmlElement.SetAttribute("PropertyType", "ManufacturedHousing");
            break;
          case "Multifamily Dwelling":
            xmlElement.SetAttribute("PropertyType", "MultifamilyDwelling");
            break;
        }
      }
      this.EMXMLVersionID = "6.0";
    }

    private void cleanupXml62()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.2)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae", "DatePrepared", "EllieMae/REGZ", "TILDate", false);
      this.copySingleAttribute("EllieMae", "DatePrepared", "EllieMae/REGZ", "GFEDate", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/MortgageInspectionFee", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/ProcessingFee", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/UnderwritingFee", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/MortgageBrokerFee", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/UserDefinedFee_820", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/UserDefinedFee_821", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/UserDefinedFee_822", "PTB", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801PTB", "EllieMae/CLOSING_COST[1]/UserDefinedFee_823", "PTB", true);
      this.EMXMLVersionID = "6.2";
    }

    private void cleanupXml6203()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.203)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement1 != null && xmlElement1.GetAttribute("UseNewHUD") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/MortgageInspectionFee", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/ProcessingFee", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/UnderwritingFee", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/MortgageBrokerFee", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/UserDefinedFee_820", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/UserDefinedFee_821", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/UserDefinedFee_822", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801APR", "EllieMae/CLOSING_COST[1]/UserDefinedFee_823", "PFC", true);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanOriginationFee", "Percentage", "EllieMae/CLOSING_COST[1]/LoanDiscountFee", "Percentage", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanOriginationFee", "Amount", "EllieMae/CLOSING_COST[1]/LoanDiscountFee", "Amount", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanOriginationFee", "BorPaidAmount", "TRANSACTION_DETAIL", "BorrowerPaidDiscountPointsTotalAmount", false);
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanOriginationFee");
        if (xmlElement2 != null)
        {
          xmlElement2.SetAttribute("Percentage", "");
          xmlElement2.SetAttribute("Amount", "");
          xmlElement2.SetAttribute("BorPaidAmount", "");
        }
      }
      this.EMXMLVersionID = "6.203";
    }

    private void cleanupXml6206()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.206)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement != null && xmlElement.GetAttribute("UseNewHUD") == "Y")
        this.copySingleAttribute("EllieMae", "ProposedHousingExpenseTotal", "EllieMae/CLOSING_COST[1]/NewHud/HUD1PG3", "MonthlyAmountWithEscrow", true);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "Address", "EllieMae/GFE/MLDS", "Address", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "City", "EllieMae/GFE/MLDS", "City", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "State", "EllieMae/GFE/MLDS", "State", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "PostalCode", "EllieMae/GFE/MLDS", "PostalCode", false);
      this.EMXMLVersionID = "6.206";
    }

    private void cleanupXml6207()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.207)
        return;
      this.versionMigrationOccured = true;
      this.EMXMLVersionID = "6.207";
    }

    private void cleanupXml6210()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.21)
        return;
      this.versionMigrationOccured = true;
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/AppraisalFee/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/AppraisalFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line804", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/AppraisalFee", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line804", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/CreditReportFee/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/CreditReportFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line805", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/CreditReportFee", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line805", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/TaxServiceFee/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/TaxServiceFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line806", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/TaxServiceFee", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line806", "WholePOCPaidBy", false);
      }
      for (int index = 7; index <= 12; ++index)
      {
        if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section800/@Line8" + index.ToString("00") + "POC") == "Y")
        {
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line8" + index.ToString("00") + "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line8" + index.ToString("00"), "WholePOC", false);
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line8" + index.ToString("00") + "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line8" + index.ToString("00"), "WholePOCPaidBy", false);
        }
      }
      for (int index = 813; index <= 818; ++index)
      {
        if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefinedFee_" + (object) index + "/@POC") == "Y")
        {
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefinedFee_" + (object) index, "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line" + (object) index, "WholePOC", false);
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefinedFee_" + (object) index, "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line" + (object) index, "WholePOCPaidBy", false);
        }
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section800/@Line819POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line819BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line819", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line819PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line819", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/PrepaidInterest/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/PrepaidInterest", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line901", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/PrepaidInterest", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line901", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/MortgageInsurancePremium/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/MortgageInsurancePremium", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line902", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/MortgageInsurancePremium", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line902", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/HazardInsurancePremium/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/HazardInsurancePremium", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line903", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/HazardInsurancePremium", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line903", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section900/@Line904POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line904BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line904", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line904PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line904", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/VAFundingFee/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/VAFundingFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line905", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/VAFundingFee", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line905", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/FloodInsuranceReserv/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/FloodInsuranceReserv", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line906", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/FloodInsuranceReserv", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line906", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_906/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_906", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line907", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_906", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line907", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_907/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_907", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line908", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_907", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line908", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section900/@Line909POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line909BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line909", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line909PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line909", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section900/@Line910POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line910BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line910", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line910PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line910", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1100/@Line1102POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/SettlementClosingFee", "NewHUDBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1102", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1102PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1102", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1100/@Line1103POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/TitleExamination", "NewHUDBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1103", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1103PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1103", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1100/@Line1104POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1104BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1104", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1104PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1104", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1100/@Line1109POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1109BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1109", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1109PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1109", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1100/@Line1110POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1110BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1110", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1110PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1110", "WholePOCPaidBy", false);
      }
      for (int index = 1111; index <= 1114; ++index)
      {
        if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_" + (object) index + "/@POC") == "Y")
        {
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_" + (object) index, "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line" + (object) index, "WholePOC", false);
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_" + (object) index, "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line" + (object) index, "WholePOCPaidBy", false);
        }
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/RecordingFee/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/RecordingFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1202", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/RecordingFee", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1202", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1200/@Line1203POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line1203BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1203", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1200", "Line1203PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1203", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/City_CountyTax_Stamps/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/City_CountyTax_Stamps", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1204", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/City_CountyTax_Stamps", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1204", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/StateTax_Stamps/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/StateTax_Stamps", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1205", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/StateTax_Stamps", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1205", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_1204/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_1204", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1206", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_1204", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1206", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_1205/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_1205", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1207", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_1205", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1207", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_1206/@POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_1206", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1208", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_1206", "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1208", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1300/@Line1302POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1302BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1302", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1302PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1302", "WholePOCPaidBy", false);
      }
      for (int index = 1303; index <= 1309; ++index)
      {
        if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/UserDefined_" + (object) index + "/@POC") == "Y")
        {
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_" + (object) index, "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line" + (object) index, "WholePOC", false);
          this.copySingleAttribute("EllieMae/CLOSING_COST[1]/UserDefined_" + (object) index, "PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line" + (object) index, "WholePOCPaidBy", false);
        }
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1300/@Line1310POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1310BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1310", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1310PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1310", "WholePOCPaidBy", false);
      }
      if (this.GetFieldAtXpath("EllieMae/CLOSING_COST[1]/NewHud/Section1300/@Line1311POC") == "Y")
      {
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1311BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1311", "WholePOC", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1311PaidBy", "EllieMae/CLOSING_COST[1]/NewHud/Line1311", "WholePOCPaidBy", false);
      }
      this.EMXMLVersionID = "6.210";
    }

    private void cleanupXml65()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.5)
        return;
      this.versionMigrationOccured = true;
      Dictionary<string, Dictionary<string, string>> dictionary1 = new Dictionary<string, Dictionary<string, string>>();
      dictionary1.Add("DOWN_PAYMENT/@_Amount", new Dictionary<string, string>()
      {
        {
          "743",
          "EllieMae/LOAN_SUBMISSION/@AmountRequiredToClose"
        }
      });
      dictionary1.Add("TRANSACTION_DETAIL/@PurchasePriceAmount", new Dictionary<string, string>()
      {
        {
          "CASASRN.X109",
          "EllieMae/FREDDIE_MAC/@NetPurchasePrice"
        },
        {
          "3038",
          "EllieMae/RateLock/Request/LoanInfo/@PurchasePriceAmount"
        }
      });
      dictionary1.Add("MORTGAGE_TERMS/@BorrowerRequestedLoanAmount", new Dictionary<string, string>()
      {
        {
          "3043",
          "EllieMae/RateLock/Request/LoanInfo/@BaseLoanAmount"
        }
      });
      foreach (string key1 in dictionary1.Keys)
      {
        XmlAttribute xmlAttribute1 = (XmlAttribute) this.root.SelectSingleNode(key1);
        if (xmlAttribute1 != null)
        {
          string str = xmlAttribute1.Value;
          Dictionary<string, string> dictionary2 = dictionary1[key1];
          foreach (string key2 in dictionary2.Keys)
          {
            XmlAttribute xmlAttribute2 = (XmlAttribute) this.root.SelectSingleNode(dictionary2[key2]);
            if (xmlAttribute2 != null && !(xmlAttribute2.Value == str))
              this.createLockField(key2, str);
          }
        }
      }
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("/LOAN_APPLICATION/BORROWER/DEPENDENT");
      if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
      {
        foreach (XmlNode oldChild in xmlNodeList1)
          oldChild.ParentNode.RemoveChild(oldChild);
      }
      XmlNodeList xmlNodeList2 = this.root.SelectNodes("EllieMae/SystemLog/Record/ContactList");
      if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
      {
        foreach (XmlElement xmlElement in xmlNodeList2)
        {
          if ((xmlElement.GetAttribute("ContactGUID") ?? "") == string.Empty)
            xmlElement.SetAttribute("ContactGUID", Guid.NewGuid().ToString());
        }
      }
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/SettlementClosingFee", "NewHUDBorPaidAmount", "EllieMae/CLOSING_COST[1]", "EscrowTableFee", false);
      this.copySingleAttribute("EllieMae/GFE", "AgregateAdjustment", "EllieMae/CLOSING_COST[1]/NewHud/Section1000", "AggregateAdjust", false);
      double num1 = Utils.ToDouble(this.readNodeValue("EllieMae/HUD1ES/@EscrowPayment"));
      if (num1 != 0.0)
        this.setFieldAtId("NEWHUD.X950", num1.ToString("N2"), (BorrowerPair) null);
      double num2 = Utils.ToDouble(this.readNodeValue("EllieMae/CLOSING_COST[1]/PrepaidInterest/@BorPaidAmount")) + Utils.ToDouble(this.readNodeValue("EllieMae/CLOSING_COST[1]/PrepaidInterest/@SellerPaidAmount"));
      if (num2 != 0.0)
        this.setFieldAtId("NEWHUD.X949", num2.ToString("N2"), (BorrowerPair) null);
      this.EMXMLVersionID = "6.5";
    }

    private void cleanupXml67()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.7)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1200", "Line1201BorPaidTotal", "EllieMae/CLOSING_COST[1]/NewHud/HUD1PG3", "HUD1GovernmentRecordingCharge", false);
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_820", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801GPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_821", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801HPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_822", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801IPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_823", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801JPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801AdditionalDesc5", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801KPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line808", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line808PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line809", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line809PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line810", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line810PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line811", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line811PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line812", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line812PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_813", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line813PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_814", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line814PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_815", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line815PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_816", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line816PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_817", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line817PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefinedFee_818", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line818PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line819", "EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line819PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line904", "EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line904PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_906", "PaidToName", "EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line907PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_907", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line908PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line909", "EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line909PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line910", "EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line910PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1006", "Description", "EllieMae/HUD1ES_PAYTO1", "Name");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1007", "Description", "EllieMae/HUD1ES_PAYTO2", "Name");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1008", "Description", "EllieMae/HUD1ES_PAYTO3", "Name");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101aDescription", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101APaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101bDescription", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101BPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101cDescription", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101CPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101dDescription", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101DPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101eDescription", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101EPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101FDescription", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101FPaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1109", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1109PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1110", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1110PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1111", "PaidToName", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1111PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1112", "PaidToName", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1112PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1113", "PaidToName", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1113PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1114", "PaidToName", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1114PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1204", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1200", "Line1206PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1205", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1200", "Line1207PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1206", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1200", "Line1208PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1302", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1302PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1303", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1303PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1304", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1304PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1305", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1305PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1306", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1306PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1307", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1307PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1308", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1308PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/UserDefined_1309", "Description", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1309PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1310", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1310PaidToName");
      this.migrateFeeManagement("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1311", "EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1311PaidToName");
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/HUD1PG2/Line817", "FWBC", "EllieMae/CLOSING_COST[1]/UserDefinedFee_819", "FWBC", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/HUD1PG2/Line817", "FWSC", "EllieMae/CLOSING_COST[1]/UserDefinedFee_819", "FWSC", false);
      this.copySingleAttribute("EllieMae", "CombinedLTV", "EllieMae", "TLTV", false);
      this.EMXMLVersionID = "6.7";
    }

    private void migrateFeeManagement(
      string feeDescriptionPath,
      string feeDescAttribute,
      string newToFieldPath,
      string newToAttribute)
    {
      string str1 = this.readNodeValue(feeDescriptionPath + "/@" + feeDescAttribute);
      if (str1 == string.Empty)
        return;
      int length = str1.ToLower().IndexOf(" to:");
      if (length == -1)
        return;
      string str2 = str1.Substring(0, length).Trim();
      string str3 = str1.Substring(length + 4).Trim();
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode(newToFieldPath) ?? this.createPath(newToFieldPath);
      if (xmlElement1 == null)
        return;
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode(feeDescriptionPath);
      if (xmlElement2 == null)
        return;
      xmlElement1.SetAttribute(newToAttribute, str3);
      xmlElement2.SetAttribute(feeDescAttribute, str2);
    }

    private void cleanupXml6705()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.705)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "Name", "EllieMae/BROKER_LENDER/ContactInfo", "SSNCompanyAgentName", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "Address", "EllieMae/BROKER_LENDER/ContactInfo", "SSNCompanyAgentAddress", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "City", "EllieMae/BROKER_LENDER/ContactInfo", "SSNCompanyAgentCity", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "State", "EllieMae/BROKER_LENDER/ContactInfo", "SSNCompanyAgentState", false);
      this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "PostalCode", "EllieMae/BROKER_LENDER/ContactInfo", "SSNCompanyAgentZipCode", false);
      this.EMXMLVersionID = "6.705";
    }

    private void cleanupXml6800()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.8)
        return;
      this.versionMigrationOccured = true;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string[] strArray = new string[3]
      {
        "Experian",
        "TransUnion",
        "Equifax"
      };
      if (borrowerPairs != null && borrowerPairs.Length != 0)
      {
        foreach (BorrowerPair borrowerPair in borrowerPairs)
        {
          for (int index1 = 0; index1 < strArray.Length; ++index1)
          {
            for (int index2 = 1; index2 <= 2; ++index2)
            {
              string str = index2 == 1 ? borrowerPair.Borrower.Id : borrowerPair.CoBorrower.Id;
              XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + str + "\"]/EllieMae/STATEDISCLOSURE/CREDITINFO/" + strArray[index1] + "/Comments[5]");
              if (xmlElement1 != null)
              {
                string attribute1 = xmlElement1.GetAttribute("Description");
                if (!(attribute1 == string.Empty))
                {
                  XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + str + "\"]/EllieMae/STATEDISCLOSURE/CREDITINFO/" + strArray[index1] + "/Comments[4]");
                  if (xmlElement2 == null)
                    xmlElement2 = this.createPath("BORROWER[@BorrowerID=\"" + str + "\"]/EllieMae/STATEDISCLOSURE/CREDITINFO/" + strArray[index1] + "/Comments[4]");
                  if (xmlElement2 != null)
                  {
                    string attribute2 = xmlElement2.GetAttribute("Description");
                    xmlElement2.SetAttribute("Description", attribute2 != string.Empty ? attribute2 + " " + attribute1 : attribute1);
                    xmlElement1.SetAttribute("Description", "");
                  }
                }
              }
            }
          }
        }
      }
      this.EMXMLVersionID = "6.800";
    }

    private void cleanupXml6803()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 6.803)
        return;
      this.versionMigrationOccured = true;
      this.migratePOC("EllieMae/CLOSING_COST[1]/LoanOriginationFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC1", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount1", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt1", "Indicator1");
      this.migratePOC("EllieMae/CLOSING_COST[1]/MortgageInspectionFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC2", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount2", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt2", "Indicator2");
      this.migratePOC("EllieMae/CLOSING_COST[1]/ProcessingFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC3", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount3", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt3", "Indicator3");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UnderwritingFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC4", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount4", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt4", "Indicator4");
      this.migratePOC("EllieMae/CLOSING_COST[1]/MortgageBrokerFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC5", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount5", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt5", "Indicator5");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800/BrokerCompensation", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section800/BrokerCompensation", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount6", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt6", "Indicator6");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_820", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC6", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount7", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt7", "Indicator7");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_821", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC7", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount8", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt8", "Indicator8");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_822", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC8", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount9", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt9", "Indicator9");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_823", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC9", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount10", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt10", "Indicator10");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line801AdditionalFee5", "EllieMae/CLOSING_COST[1]/NewHud/Line801", "WholePOC10", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount11", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt11", "Indicator11");
      this.migratePOC("EllieMae/CLOSING_COST[1]/AppraisalFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line804", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount13", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt13", "Indicator13");
      this.migratePOC("EllieMae/CLOSING_COST[1]/CreditReportFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line805", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount14", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt14", "Indicator14");
      this.migratePOC("EllieMae/CLOSING_COST[1]/TaxServiceFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line806", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount15", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt15", "Indicator15");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line807BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line807", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount16", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt16", "Indicator16");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line808BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line808", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount17", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt17", "Indicator17");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line809BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line809", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount18", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt18", "Indicator18");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line810BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line810", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount19", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt19", "Indicator19");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line811BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line811", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount20", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt20", "Indicator20");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line812BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line812", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount21", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt21", "Indicator21");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_813", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line813", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount22", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt22", "Indicator22");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_814", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line814", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount23", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt23", "Indicator23");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_815", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line815", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount24", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt24", "Indicator24");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_816", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line816", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount25", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt25", "Indicator25");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_817", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line817", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount26", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt26", "Indicator26");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefinedFee_818", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line818", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount27", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt27", "Indicator27");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line819BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line819", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC800", "Amount28", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC800", "BorPaidAmt28", "Indicator28");
      this.migratePOC("EllieMae/CLOSING_COST[1]/PrepaidInterest", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line901", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount1", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt1", "Indicator1");
      this.migratePOC("EllieMae/CLOSING_COST[1]/MortgageInsurancePremium", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line902", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount2", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt2", "Indicator2");
      this.migratePOC("EllieMae/CLOSING_COST[1]/HazardInsurancePremium", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line903", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount3", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt3", "Indicator3");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line904BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line904", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount4", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt4", "Indicator4");
      this.migratePOC("EllieMae/CLOSING_COST[1]/VAFundingFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line905", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount5", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt5", "Indicator5");
      this.migratePOC("EllieMae/CLOSING_COST[1]/FloodInsuranceReserv", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line906", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount6", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt6", "Indicator6");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_906", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line907", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount7", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt7", "Indicator7");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_907", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line908", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount8", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt8", "Indicator8");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line909BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line909", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount9", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt9", "Indicator9");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section900", "Line910BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line910", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC900", "Amount10", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC900", "BorPaidAmt10", "Indicator10");
      this.migratePOC("EllieMae/CLOSING_COST[1]/HazardInsurance", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount1", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt1", "Indicator1");
      this.migratePOC("EllieMae/CLOSING_COST[1]/MortgageInsurance", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount2", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt2", "Indicator2");
      this.migratePOC("EllieMae/CLOSING_COST[1]/PropertyTaxes", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount3", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt3", "Indicator3");
      this.migratePOC("EllieMae/CLOSING_COST[1]/SchoolTaxes", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount4", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt4", "Indicator4");
      this.migratePOC("EllieMae/CLOSING_COST[1]/FloodInsurance", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount5", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt5", "Indicator5");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1006", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount6", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt6", "Indicator6");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1007", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount7", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt7", "Indicator7");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1008", "BorPaidAmount", (string) null, (string) null, "EllieMae/CLOSING_COST[1]/NewHud/PTC1000", "Amount8", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1000", "BorPaidAmt8", "Indicator8");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101aBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101aWholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount1", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt1", "Indicator1");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101bBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101bWholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount2", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt2", "Indicator2");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101cBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101cWholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount3", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt3", "Indicator3");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101dBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101dWholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount4", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt4", "Indicator4");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101eBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101eWholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount5", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt5", "Indicator5");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101FBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101fWholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount6", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt6", "Indicator6");
      this.migratePOC("EllieMae/CLOSING_COST[1]/SettlementClosingFee", "NewHUDBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1102", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount7", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt7", "Indicator7");
      this.migratePOC("EllieMae/CLOSING_COST[1]/TitleExamination", "NewHUDBorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1103", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount8", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt8", "Indicator8");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHUD/Section1100", "Line1104BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1104", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount9", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt9", "Indicator9");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHUD/Section1100", "Line1109BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1109", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount10", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt10", "Indicator10");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHUD/Section1100", "Line1110BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1110", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount11", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt11", "Indicator11");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1111", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1111", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount12", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt12", "Indicator12");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1112", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1112", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount13", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt13", "Indicator13");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1113", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1113", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount14", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt14", "Indicator14");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1114", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1114", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1100", "Amount15", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1100", "BorPaidAmt15", "Indicator15");
      this.migratePOC("EllieMae/CLOSING_COST[1]/RecordingFee", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1202", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount1", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt1", "Indicator1");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section800", "Line1203BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1203", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount2", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt2", "Indicator2");
      this.migratePOC("EllieMae/CLOSING_COST[1]/City_CountyTax_Stamps", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1204", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount3", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt3", "Indicator3");
      this.migratePOC("EllieMae/CLOSING_COST[1]/StateTax_Stamps", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1205", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount4", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt4", "Indicator4");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1204", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1206", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount5", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt5", "Indicator5");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1205", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1207", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount6", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt6", "Indicator6");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1206", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1208", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1200", "Amount7", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1200", "BorPaidAmt7", "Indicator7");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1302BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1302", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount1", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt1", "Indicator1");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1303", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1303", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount2", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt2", "Indicator2");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1304", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1304", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount3", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt3", "Indicator3");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1305", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1305", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount4", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt4", "Indicator4");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1306", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1306", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount5", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt5", "Indicator5");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1307", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1307", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount6", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt6", "Indicator6");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1308", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1308", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount7", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt7", "Indicator7");
      this.migratePOC("EllieMae/CLOSING_COST[1]/UserDefined_1309", "BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1309", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount8", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt8", "Indicator8");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1310BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1310", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount9", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt9", "Indicator9");
      this.migratePOC("EllieMae/CLOSING_COST[1]/NewHud/Section1300", "Line1311BorPaidAmount", "EllieMae/CLOSING_COST[1]/NewHud/Line1311", "WholePOC", "EllieMae/CLOSING_COST[1]/NewHud/PTC1300", "Amount10", "EllieMae/CLOSING_COST[1]/NewHud/POCPTC1300", "BorPaidAmt10", "Indicator10");
      this.EMXMLVersionID = "6.803";
    }

    private void migratePOC(
      string borAmtPath,
      string borAmtPathAttr,
      string pocAmtPath,
      string pocAmtPathAttr,
      string ptcAmtPath,
      string ptcAmtPathAttr,
      string borToPayPath,
      string borToPayPathAttr,
      string pocPTCIndicatorPathAttr)
    {
      try
      {
        XmlElement xmlElement1 = pocAmtPath != null ? (XmlElement) this.root.SelectSingleNode(pocAmtPath) : (XmlElement) null;
        double num1 = xmlElement1 != null ? Utils.ParseDouble((object) xmlElement1.GetAttribute(pocAmtPathAttr)) : 0.0;
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode(borAmtPath);
        double num2 = xmlElement2 != null ? Utils.ParseDouble((object) xmlElement2.GetAttribute(borAmtPathAttr)) : 0.0;
        if (num2 == 0.0)
          return;
        XmlElement xmlElement3 = ptcAmtPath != null ? (XmlElement) this.root.SelectSingleNode(ptcAmtPath) : (XmlElement) null;
        double num3 = xmlElement3 != null ? Utils.ParseDouble((object) xmlElement3.GetAttribute(ptcAmtPathAttr)) : 0.0;
        XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode(borToPayPath) ?? this.createPath(borToPayPath);
        if (xmlElement4 == null)
          return;
        double num4 = num2 - num1 - num3;
        xmlElement4.SetAttribute(borToPayPathAttr, num4 != 0.0 ? num4.ToString("N2") : "");
        xmlElement4.SetAttribute(pocPTCIndicatorPathAttr, num1 != 0.0 ? "Y" : "");
      }
      catch (Exception ex)
      {
      }
    }

    private void cleanupXml70()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.0)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/HMDA");
      if (xmlElement != null && xmlElement.GetAttribute("TypeOfPurchaser") == "Commercial Bank")
        xmlElement.SetAttribute("TypeOfPurchaser", "Private Securitization");
      this.copySingleAttribute("EllieMae/HUD1ES", "StartingBalance", "EllieMae/CLOSING_COST[1]/NewHud/Section1000", "Line1001BorPaidTotal", false);
      this.EMXMLVersionID = "7.0";
    }

    private void cleanupXml7003()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.003)
        return;
      this.versionMigrationOccured = true;
      try
      {
        double num1 = 0.0;
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud");
        if (xmlElement1 != null)
        {
          for (int index = 1; index <= 7; ++index)
          {
            if (xmlElement1.HasAttribute("HomeownerInsuranceCharge" + (object) index))
              num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("HomeownerInsuranceCharge" + (object) index));
          }
          xmlElement1.SetAttribute("HomeownerInsurance", num1.ToString("N2"));
          if (xmlElement1.HasAttribute("RequiredServicesAmount"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("RequiredServicesAmount"));
          if (xmlElement1.HasAttribute("TitleServiceAmount"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("TitleServiceAmount"));
          if (xmlElement1.HasAttribute("OwnerTitleInsuranceAmount"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("OwnerTitleInsuranceAmount"));
          if (xmlElement1.HasAttribute("ShopRequiredServicesAmount"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("ShopRequiredServicesAmount"));
          if (xmlElement1.HasAttribute("GFEGovernmentRecordingCharges"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("GFEGovernmentRecordingCharges"));
          if (xmlElement1.HasAttribute("TotalTransferTaxes"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("TotalTransferTaxes"));
          if (xmlElement1.HasAttribute("Line1001Fee"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("Line1001Fee"));
          if (xmlElement1.HasAttribute("DailyInterestCharges"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("DailyInterestCharges"));
          xmlElement1.SetAttribute("AllOtherServiceAmount", num1.ToString("N2"));
          if (xmlElement1.HasAttribute("AdjustedOriginationCharges"))
            num1 += Utils.ParseDouble((object) xmlElement1.GetAttribute("AdjustedOriginationCharges"));
          xmlElement1.SetAttribute("TotalSettlementCharges", num1.ToString("N2"));
          XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud/LowerSettlement");
          if (xmlElement2 != null && xmlElement2.HasAttribute("TotalSettlementCharges"))
          {
            double num2 = Utils.ParseDouble((object) xmlElement2.GetAttribute("TotalSettlementCharges"));
            if (num2 > 0.0)
            {
              double num3 = num1 - num2;
              xmlElement2.SetAttribute("ServiceChargeReducedAmount", num3 < 0.0 ? "" : num3.ToString("N2"));
            }
            else
              xmlElement2.SetAttribute("ServiceChargeReducedAmount", "");
          }
          XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud/LowerInterest");
          if (xmlElement3 != null)
          {
            if (xmlElement3.HasAttribute("TotalSettlementCharges"))
            {
              double num4 = Utils.ParseDouble((object) xmlElement3.GetAttribute("TotalSettlementCharges"));
              if (num4 > 0.0)
              {
                double num5 = num4 - num1;
                xmlElement3.SetAttribute("ServiceChargeIncreasedAmount", num5 < 0.0 ? "" : num5.ToString("N2"));
              }
              else
                xmlElement3.SetAttribute("ServiceChargeIncreasedAmount", "");
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.EMXMLVersionID = "7.003";
    }

    private void cleanupXml7004()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.004)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanDiscountFee", "Percentage", "EllieMae/FHA_VA_LOANS/VA_REFI_WORKSHEET", "DiscountPercentage", false);
      this.EMXMLVersionID = "7.004";
    }

    private void cleanupXml75()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.5)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement1 != null && xmlElement1.GetAttribute("MICompanyNameType") == "PMIC")
        xmlElement1.SetAttribute("MICompanyNameType", "RMIC");
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement2 != null)
      {
        string attribute = xmlElement2.GetAttribute("RelatedLoanInvestorType");
        if (attribute == "FRE" || attribute == "Seller")
          xmlElement2.SetAttribute("FreddieRelatedLoanInvestorType", attribute);
      }
      int num1 = (int) this.toDouble(this.GetFieldAtXpath("MORTGAGE_TERMS/@LoanAmortizationTermMonths"));
      int num2 = (int) this.toDouble(this.GetFieldAtXpath("LOAN_PRODUCT_DATA/LOAN_FEATURES/@BalloonLoanMaturityTermMonths"));
      DateTime date = Utils.ParseDate((object) this.GetFieldAtXpath("LOAN_PRODUCT_DATA/LOAN_FEATURES/@ScheduledFirstPaymentDate"));
      if (date != DateTime.MinValue && (num1 > 0 || num2 > 0))
      {
        try
        {
          DateTime dateTime = date.AddMonths((num2 != 0 ? num2 : num1) / 2 + 1);
          dateTime = new DateTime(dateTime.Year, dateTime.Month, 1);
          this.SetFieldAt("3548", dateTime.ToString("MM/dd/yyyy"));
        }
        catch (Exception ex)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot calculate midpoint date (3548) due to invalid loan term (4) or Due In Term (325). Error: " + ex.Message);
        }
      }
      this.EMXMLVersionID = "7.5";
    }

    private void cleanupXml751()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.51)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement != null)
      {
        string attribute = xmlElement.GetAttribute("InvestorFeatureIdentifier");
        xmlElement.SetAttribute("FreddieInvestorFeatureIdentifier", attribute);
      }
      this.EMXMLVersionID = "7.51";
    }

    private void cleanupXml7512()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.512)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement1 != null)
      {
        string str = xmlElement1.GetAttribute("RefinanceCashOutDeterminationType");
        if (str == "LimitedCashOut")
          str = "NoCashOut";
        xmlElement1.SetAttribute("FreddieRefinanceCashOutDeterminationType", str);
      }
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("MORTGAGE_TERMS");
      if (xmlElement2 != null)
      {
        string attribute = xmlElement2.GetAttribute("MortgageType");
        XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
        if (xmlElement3 != null)
        {
          if (attribute != "Other")
            xmlElement3.SetAttribute("FannnieMortgageType", attribute);
          else if (attribute != "HELOC")
            xmlElement3.SetAttribute("FreddieMortgageType", attribute);
        }
      }
      this.EMXMLVersionID = "7.512";
    }

    private void cleanupXml7513()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 7.513)
        return;
      this.versionMigrationOccured = true;
      XmlElement oldChild = (XmlElement) this.root.SelectSingleNode("/LOAN_APPLICATION/LOCK/FIELD[@id='GLOBAL.S2']");
      oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
      this.EMXMLVersionID = "7.513";
    }

    private void cleanupXml8000()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 8.0)
        return;
      this.versionMigrationOccured = true;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      XmlElement xmlElement1 = (XmlElement) null;
      if (borrowerPairs != null && borrowerPairs.Length != 0)
      {
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/LIABILITY");
        if (xmlElement2 != null)
        {
          string attribute = xmlElement2.GetAttribute("MonthsToExclude");
          if (attribute != string.Empty)
          {
            string empty = string.Empty;
            foreach (BorrowerPair borrowerPair in borrowerPairs)
            {
              XmlNodeList xmlNodeList = this.root.SelectNodes("LIABILITY[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"][not(@_Type=\"Alimony\") and not(@_Type=\"JobRelatedExpenses\")]");
              if (xmlNodeList != null && xmlNodeList.Count != 0)
              {
                for (int i = 0; i < xmlNodeList.Count; ++i)
                  ((XmlElement) xmlNodeList[i]).SetAttribute("_MonthsToExclude", attribute);
              }
            }
          }
        }
      }
      XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/REGZ");
      if (xmlElement3 != null)
      {
        string attribute = xmlElement3.GetAttribute("GFEChangedCirsumstanceItem");
        if (xmlElement3.GetAttribute("GFEChangedCirsumstanceItemCode") == string.Empty)
        {
          switch (attribute)
          {
            case "Additional borrower has been added to the loan or borrower has been dropped from the loan":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceCode", "AddiBor");
              break;
            case "Additional service (such as survey) is necessary based on title report":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "AddiService");
              break;
            case "Appraised value is different than estimated value":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "ApprasValDiff");
              break;
            case "Borrower income could not verified or was verified at different amount":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "IncomeNotVeri");
              break;
            case "Borrower taking title to the property has changed":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "PropertyTitle");
              break;
            case "Change in loan amount":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "ChangeLoanAmt");
              break;
            case "Loan type or loan program has changed":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "LoanTypeProgram");
              break;
            case "Other":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "Other");
              break;
            case "Recording fees are increased based on need to record additional unanticipated documents such as release of prior lien":
              xmlElement3.SetAttribute("GFEChangedCirsumstanceItemCode", "RecordingFee");
              break;
          }
        }
      }
      if (this.GetFieldAtXpath("_CLOSING_DOCUMENTS/EllieMae/@LoanIsLocked") == "Y")
      {
        XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("EllieMae/LOG");
        if (xmlElement4 != null)
        {
          XmlNodeList xmlNodeList = xmlElement4.SelectNodes("Record");
          string str = "";
          foreach (XmlElement xmlElement5 in xmlNodeList)
          {
            if (xmlElement5.GetAttribute("IsLockExtension") == "Y")
              str = this.getFieldfromGuid(xmlElement5.GetAttribute("Guid"), "3364");
          }
          if (!string.IsNullOrEmpty(str))
          {
            if ((XmlElement) this.root.SelectSingleNode("EllieMae/RateLock/BuySide/ExtendedLockExpires") == null)
              xmlElement1 = this.createPath("EllieMae/RateLock/BuySide/ExtendedLockExpires");
            ((XmlElement) this.root.SelectSingleNode("EllieMae/RateLock/BuySide")).SetAttribute("ExtendedLockExpires", str);
          }
        }
      }
      this.EMXMLVersionID = "8.000";
    }

    private void cleanupXml8100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 8.1)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute((string) null, "Y", "EllieMae/FHA_VA_LOANS/Lender", "UseDefaultLenderInfo", true);
      this.copySingleAttribute("EllieMae/LENDER_INVESTOR", "Name", "EllieMae/FHA_VA_LOANS/Lender", "Name", true);
      this.copySingleAttribute("EllieMae/LENDER_INVESTOR", "Address", "EllieMae/FHA_VA_LOANS/Lender", "Address", true);
      this.copySingleAttribute("EllieMae/LENDER_INVESTOR", "City", "EllieMae/FHA_VA_LOANS/Lender", "City", true);
      this.copySingleAttribute("EllieMae/LENDER_INVESTOR", "State", "EllieMae/FHA_VA_LOANS/Lender", "State", true);
      this.copySingleAttribute("EllieMae/LENDER_INVESTOR", "PostalCode", "EllieMae/FHA_VA_LOANS/Lender", "PostalCode", true);
      this.copySingleAttribute("EllieMae/LENDER_INVESTOR", "LenderNMLSLicense", "EllieMae/FHA_VA_LOANS/Lender", "NMLS", true);
      this.copySingleAttribute("EllieMae/IRS_1098", "TaxID", "EllieMae/FHA_VA_LOANS/Lender", "TaxID", true);
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement != null)
      {
        if (this.GetFieldAtXpath("EllieMae/ULDD/@SpecialFloodHazardArea") == "Y")
        {
          if (this.GetFieldAtXpath("EllieMae/REGZ/INSURANCE/@FloodInsurance") == "Y")
            xmlElement.SetAttribute("FannieFloodSpecialFeatureCode", "170");
        }
        else if (this.GetFieldAtXpath("EllieMae/REGZ/INSURANCE/@FloodInsurance") == "Y")
          xmlElement.SetAttribute("FannieFloodSpecialFeatureCode", "175");
        else
          xmlElement.SetAttribute("FannieFloodSpecialFeatureCode", "180");
      }
      this.EMXMLVersionID = "8.100";
    }

    private void cleanupXml9000(LoanMigrationData migrationData)
    {
      XmlElement systemLogRoot = this.GetSystemLogRoot(this.systemId);
      if ((XmlElement) systemLogRoot.SelectSingleNode("MilestoneTemplate") == null)
      {
        XmlElement element = systemLogRoot.OwnerDocument.CreateElement("MilestoneTemplate");
        systemLogRoot.AppendChild((XmlNode) element);
        element.SetAttribute("Guid", Guid.NewGuid().ToString());
        element.SetAttribute("Type", "MilestoneTemplateLog");
        element.SetAttribute("MilestoneTemplateID", migrationData.DefaultTemplate.TemplateID);
        element.SetAttribute("MilestoneTemplateName", migrationData.DefaultTemplate.Name);
        element.SetAttribute("IsTemplateLocked", "Y");
        element.SetAttribute("IsTemplateDatesLocked", "N");
      }
      if (this.toDouble(this.EMXMLVersionID) >= 9.0)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/HUD1ES", "StartingBalance", "EllieMae/CLOSING_COST[1]/Section1000", "BorrowerPaidTotalAmount", true);
      double num = 0.0;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/HazardInsurance");
      if (xmlElement1 != null && xmlElement1.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement1.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/MortgageInsurance");
      if (xmlElement2 != null && xmlElement2.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement2.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/PropertyTaxes");
      if (xmlElement3 != null && xmlElement3.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement3.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/SchoolTaxes");
      if (xmlElement4 != null && xmlElement4.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement4.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement5 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/FloodInsurance");
      if (xmlElement5 != null && xmlElement5.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement5.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement6 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/UserDefined_1006");
      if (xmlElement6 != null && xmlElement6.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement6.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement7 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/UserDefined_1007");
      if (xmlElement7 != null && xmlElement7.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement7.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement8 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/UserDefined_1008");
      if (xmlElement8 != null && xmlElement8.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement8.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement9 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/UserDefined_1010");
      if (xmlElement9 != null && xmlElement9.HasAttribute("SellerPaidAmount"))
        num += Utils.ParseDouble((object) xmlElement9.GetAttribute("SellerPaidAmount"));
      XmlElement xmlElement10 = (XmlElement) this.root.SelectSingleNode("EllieMae/HUD1ES");
      if (xmlElement10 != null && xmlElement10.HasAttribute("StartingBalance"))
        num += Utils.ParseDouble((object) xmlElement10.GetAttribute("StartingBalance"));
      else
        xmlElement10 = this.createPath("EllieMae/HUD1ES");
      xmlElement10?.SetAttribute("StartingBalance", num.ToString("N2"));
      this.copySingleAttribute("EllieMae/FHA_VA_LOANS/HUD92900LT", "LoanFor203K", "EllieMae/RateLock/Request/LoanInfo", "LoanFor203K", false);
      this.copySingleAttribute("EllieMae/FHA_VA_LOANS/FHA_203K/REFINANCE_CALCULATION", "TotalForLesserOfSumAsIs", "EllieMae/RateLock/Request/LoanInfo", "TotalForLesserOfSumAsIs", false);
      this.copySingleAttribute("EllieMae/FREDDIE_MAC", "HELOCActualBalance", "EllieMae/RateLock/Request/LoanInfo", "HELOCActualBalance", false);
      this.MigrateForUlddManufacturedHomeWideType();
      this.MigrateForFreddiePropertyFormType();
      this.MigrateForFanniePropertyFormType();
      this.MigrateForRefinanceCashOutAmount();
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (borrowerPairs != null && borrowerPairs.Length != 0)
      {
        foreach (BorrowerPair borrowerPair in borrowerPairs)
        {
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_StreetAddress", "BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "BorrowerAddress", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_City", "BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "BorrowerAddressCity", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_State", "BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "BorrowerAddressState", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_PostalCode", "BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "BorrowerAddressZipcode", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_StreetAddress", "BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "CoBorrowerAddress", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_City", "BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "CoBorrowerAddressCity", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_State", "BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "CoBorrowerAddressState", false);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/_RESIDENCE[@BorrowerResidencyType=\"Current\"]", "_PostalCode", "BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "CoBorrowerAddressZipcode", false);
          XmlElement xmlElement11 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL");
          if (xmlElement11 != null && (xmlElement11.GetAttribute("BorrowerAddress") != string.Empty || xmlElement11.GetAttribute("BorrowerAddressCity") != string.Empty || xmlElement11.GetAttribute("BorrowerAddressState") != string.Empty || xmlElement11.GetAttribute("BorrowerAddressZipcode") != string.Empty))
            this.copySingleAttribute((string) null, "Present Address", "BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "BorrowerAddressType", false);
          XmlElement xmlElement12 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL");
          if (xmlElement12 != null && (xmlElement12.GetAttribute("CoBorrowerAddress") != string.Empty || xmlElement12.GetAttribute("CoBorrowerAddressCity") != string.Empty || xmlElement12.GetAttribute("CoBorrowerAddressState") != string.Empty || xmlElement12.GetAttribute("CoBorrowerAddressZipcode") != string.Empty))
            this.copySingleAttribute((string) null, "Present Address", "BORROWER[@BorrowerID=\"" + borrowerPair.CoBorrower.Id + "\"]/EllieMae/PAIR/STATEMENT_DENIAL", "CoBorrowerAddressType", false);
        }
      }
      this.EMXMLVersionID = "9.000";
    }

    private void MigrateForUlddManufacturedHomeWideType()
    {
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/TSUM");
      if (xmlElement1 == null || !xmlElement1.HasAttribute("PropertyType"))
        return;
      switch (xmlElement1.GetAttribute("PropertyType"))
      {
        case "Manufactured Housing Single Wide":
          XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
          if (xmlElement2 == null || xmlElement2.HasAttribute("ManufacturedHomeWidthType"))
            break;
          xmlElement2.SetAttribute("ManufacturedHomeWidthType", "ManufacturedSingleWide");
          break;
        case "Manufactured Housing Multiwide":
          XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
          if (xmlElement3 == null || xmlElement3.HasAttribute("ManufacturedHomeWidthType"))
            break;
          xmlElement3.SetAttribute("ManufacturedHomeWidthType", "ManufacturedMultiwide");
          break;
      }
    }

    private void MigrateForFreddiePropertyFormType()
    {
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/TSUM");
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement1 == null || xmlElement2 == null || !xmlElement1.HasAttribute("PropertyFormType") || !xmlElement2.HasAttribute("FreddiePropertyFormType") || !(xmlElement2.GetAttribute("FreddiePropertyFormType") == string.Empty))
        return;
      xmlElement2.SetAttribute("FreddiePropertyFormType", xmlElement1.GetAttribute("PropertyFormType"));
    }

    private void MigrateForFanniePropertyFormType()
    {
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/TSUM");
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement1 == null || xmlElement2 == null || !xmlElement1.HasAttribute("PropertyFormType") || !xmlElement2.HasAttribute("FanniePropertyFormType") || !(xmlElement2.GetAttribute("FanniePropertyFormType") == string.Empty))
        return;
      string attribute = xmlElement1.GetAttribute("PropertyFormType");
      // ISSUE: reference to a compiler-generated method
      switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(attribute))
      {
        case 213836621:
          if (!(attribute == "Two To Four Unit Residential Appraisal"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 2000A / FRE 1072 = Two To Four Unit Residential Appraisal");
          break;
        case 1624383784:
          if (!(attribute == "One Unit Residential Appraisal Field Review Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 2000 / FRE1032 = One Unit Residential Appraisal Field Review Report");
          break;
        case 1945541913:
          if (!(attribute == "Small Residential Income Property Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 1025 / FRE 72 = Small Residential Income Property Appraisal Report");
          break;
        case 2168029724:
          if (!(attribute == "Exterior Only Inspection Individual Cooperative Interest Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 2095 = Exterior Only Inspection Individual Cooperative Interest Appraisal Report");
          break;
        case 2311104136:
          if (!(attribute == "Uniform Residential Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 1004 / FRE 70 = Uniform Residential Appraisal Report");
          break;
        case 2774529652:
          if (!(attribute == "Individual Cooperative Interest Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 2090 = Individual Cooperative Interest Appraisal Report");
          break;
        case 2959858427:
          if (!(attribute == "Individual Condominium Unit Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 1073 / FRE 465 = Individual Condominium Unit Appraisal Report");
          break;
        case 3059959148:
          if (!(attribute == "Exterior Only Inspection Residential Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 2055 / FRE 2055 = Exterior Only Inspection Residential Appraisal Report");
          break;
        case 3132666464:
          if (!(attribute == "Manufactured Home Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 1004C / FRE 70B = Manufactured Home Appraisal Report");
          break;
        case 3168986995:
          if (!(attribute == "Exterior Only Inspection Individual Condominium Unit Appraisal Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "FNM 1075 / FRE 466 = Exterior Only Inspection Individual Condominium Unit Appraisal Report");
          break;
        case 3262274919:
          if (!(attribute == "Desktop Underwriter Property Inspection Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "");
          break;
        case 3581629925:
          if (!(attribute == "Appraisal Update And Or Completion Report"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "");
          break;
        case 3852393734:
          if (!(attribute == "Loan Prospector Condition And Marketability"))
            break;
          xmlElement2.SetAttribute("FreddiePropertyFormType", "");
          break;
      }
    }

    public void MigrateForRefinanceCashOutAmount()
    {
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("TRANSACTION_DETAIL/EllieMae");
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlElement1 == null || xmlElement2 == null || !xmlElement1.HasAttribute("CashFromToBorrower") || !xmlElement2.HasAttribute("RefinanceCashOutAmount") || !(xmlElement2.GetAttribute("RefinanceCashOutAmount") == string.Empty))
        return;
      Decimal result;
      Decimal num = Decimal.TryParse(xmlElement1.GetAttribute("CashFromToBorrower"), out result) ? result : 0M;
      if (!(num < 0M))
        return;
      xmlElement2.SetAttribute("RefinanceCashOutAmount", (-num).ToString());
    }

    private void cleanupXml9002()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 9.002)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/MCAW/MORTGAGE_CALCULATION", "RequirementAdjustment", "EllieMae/MCAW/MORTGAGE_CALCULATION", "RepairsImprovementAmount", true);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section800/BrokerCompensation", "BorPaidAmount", "EllieMae/ATR_QM/Qualification", "LOBrokerCompensationAmount", true);
      this.EMXMLVersionID = "9.002";
    }

    private void cleanupXml9004()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 9.004)
        return;
      this.versionMigrationOccured = true;
      string str = "EllieMae";
      string xpath = str + "/SystemLog[@SysID='" + this.systemId + "']//Milestone";
      if (this.root.SelectSingleNode(str + "/@MS_START") != null)
        this.root.SelectSingleNode(str + "/@MS_START").Value = "";
      if (this.root.SelectSingleNode(str + "/@MS_PROC") != null)
        this.root.SelectSingleNode(str + "/@MS_PROC").Value = "";
      if (this.root.SelectSingleNode(str + "/@MS_SUB") != null)
        this.root.SelectSingleNode(str + "/@MS_SUB").Value = "";
      if (this.root.SelectSingleNode(str + "/@MS_APP") != null)
        this.root.SelectSingleNode(str + "/@MS_APP").Value = "";
      if (this.root.SelectSingleNode(str + "/@MS_DOC") != null)
        this.root.SelectSingleNode(str + "/@MS_DOC").Value = "";
      if (this.root.SelectSingleNode(str + "/@MS_FUN") != null)
        this.root.SelectSingleNode(str + "/@MS_FUN").Value = "";
      if (this.root.SelectSingleNode(str + "/@MS_CLO") != null)
        this.root.SelectSingleNode(str + "/@MS_CLO").Value = "";
      foreach (XmlElement selectNode in this.root.SelectNodes(xpath))
      {
        if ((selectNode.GetAttribute("Done") ?? "") == "Y")
        {
          switch (selectNode.GetAttribute("MilestoneID"))
          {
            case "1":
              if (this.root.SelectSingleNode(str + "/@MS_START") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_START").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            case "2":
              if (this.root.SelectSingleNode(str + "/@MS_PROC") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_PROC").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            case "3":
              if (this.root.SelectSingleNode(str + "/@MS_SUB") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_SUB").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            case "4":
              if (this.root.SelectSingleNode(str + "/@MS_APP") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_APP").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            case "5":
              if (this.root.SelectSingleNode(str + "/@MS_DOC") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_DOC").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            case "6":
              if (this.root.SelectSingleNode(str + "/@MS_FUN") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_FUN").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            case "7":
              if (this.root.SelectSingleNode(str + "/@MS_CLO") != null)
              {
                this.root.SelectSingleNode(str + "/@MS_CLO").Value = selectNode.GetAttribute("Date");
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      this.EMXMLVersionID = "9.004";
    }

    private void cleanupXml9005()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 9.005)
        return;
      this.versionMigrationOccured = true;
      this.EMXMLVersionID = "9.005";
    }

    private void cleanupXml14000()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 14.0)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/REGZ/InterestPaymentSummary/InitialPeriod", "MonthlyPayment", "EllieMae/ATR_QM/Qualification", "InitialRateMonthlyPayment", false);
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/PAIR", "TopRatio", "EllieMae/ATR_QM/Qualification", "InitialRateHousingRatio", false);
      this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/PAIR", "BottomRatio", "EllieMae/ATR_QM/Qualification", "InitialRateTotalDebtRatio", false);
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/DISCLOSURE_NOTICES/PrivacyPolicy");
      if (xmlElement != null)
      {
        string empty = string.Empty;
        for (int index = 1; index <= 7; ++index)
        {
          string strA = xmlElement.GetAttribute("LimitSharing" + (object) index);
          if (string.Compare(strA, "Yes - It is required/provides an Opt-Out", true) == 0)
            strA = "Yes";
          else if (string.Compare(strA, "No - Does not provide an Opt-Out", true) == 0)
            strA = "No";
          else if (string.Compare(strA, "No - We don't share", true) == 0)
            strA = "We Don't Share";
          if (!string.IsNullOrEmpty(strA))
            xmlElement.SetAttribute("LimitSharing" + (object) index, strA);
        }
      }
      this.EMXMLVersionID = "14.000";
    }

    private void migrateTPOInformationTool()
    {
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode("EllieMae/TPO");
      if (newChild == null)
      {
        newChild = this.root.OwnerDocument.CreateElement("TPO");
        this.root.SelectSingleNode("EllieMae").AppendChild((XmlNode) newChild);
      }
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CUSTOM_FIELDS");
      if (xmlElement1 == null)
        return;
      XmlElement xmlElement2 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.SITE']");
      if (xmlElement2 != null)
        newChild.SetAttribute("SITEID", xmlElement2.GetAttribute("FieldValue"));
      XmlElement xmlElement3 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.COMPANY']");
      if (xmlElement3 != null)
        newChild.SetAttribute("Company", xmlElement3.GetAttribute("FieldValue"));
      XmlElement xmlElement4 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.BRANCH']");
      if (xmlElement4 != null)
        newChild.SetAttribute("Branch", xmlElement4.GetAttribute("FieldValue"));
      XmlElement xmlElement5 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LO']");
      if (xmlElement5 != null)
        newChild.SetAttribute("LOID", xmlElement5.GetAttribute("FieldValue"));
      XmlElement xmlElement6 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LP']");
      if (xmlElement6 != null)
        newChild.SetAttribute("LPID", xmlElement6.GetAttribute("FieldValue"));
      string[] strArray1 = new string[8]
      {
        "RegisterDate",
        "SubmitDate",
        "FeeReviewStatus",
        "Archived",
        "ImportSource",
        "DocumentsReadyDate",
        "FeeReviewStatusDate",
        "FeeReviewComments"
      };
      for (int index = 0; index < strArray1.Length; ++index)
      {
        XmlElement xmlElement7 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO." + strArray1[index].ToUpper() + "']");
        if (xmlElement7 != null)
        {
          newChild.SetAttribute(strArray1[index], xmlElement7.GetAttribute("FieldValue"));
          if (strArray1[index] == "DocumentsReadyDate" && xmlElement7.GetAttribute("FieldValue") != "" && xmlElement7.GetAttribute("FieldValue") != "//")
            newChild.SetAttribute("UnderwriterReviewed", "Y");
        }
      }
      XmlElement xmlElement8 = (XmlElement) newChild.SelectSingleNode("Company") ?? this.createPath("EllieMae/TPO/Company");
      string[] strArray2 = new string[13]
      {
        "Name",
        "LegalName",
        "Address",
        "City",
        "State",
        "Zip",
        "Phone",
        "Fax",
        "Rating",
        "ManagerName",
        "ManagerEmail",
        "AEName",
        "AEUserName"
      };
      for (int index = 0; index < strArray2.Length; ++index)
      {
        XmlElement xmlElement9 = !(strArray2[index] == "MERSOriginatingID") ? (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.COMPANY" + strArray2[index].ToUpper() + "']") : (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO." + strArray2[index].ToUpper() + "']");
        if (xmlElement9 != null)
          xmlElement8.SetAttribute(strArray2[index], xmlElement9.GetAttribute("FieldValue"));
      }
      XmlElement xmlElement10 = (XmlElement) newChild.SelectSingleNode("Branch") ?? this.createPath("EllieMae/TPO/Branch");
      for (int index = 0; index < strArray2.Length; ++index)
      {
        XmlElement xmlElement11 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.BRANCH" + strArray2[index].ToUpper() + "']");
        if (xmlElement11 != null)
          xmlElement10.SetAttribute(strArray2[index], xmlElement11.GetAttribute("FieldValue"));
      }
      string[] strArray3 = new string[11]
      {
        "Name",
        "Email",
        "Status",
        "BusinessPhone",
        "BusinessFax",
        "CellPhone",
        "Address",
        "City",
        "State",
        "Zip",
        "Notes"
      };
      for (int index = 0; index < strArray3.Length; ++index)
      {
        XmlElement xmlElement12 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LO" + strArray3[index].ToUpper() + "']");
        if (xmlElement12 != null)
          newChild.SetAttribute("LO" + strArray3[index], xmlElement12.GetAttribute("FieldValue"));
      }
      for (int index = 0; index < strArray3.Length; ++index)
      {
        XmlElement xmlElement13 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LP" + strArray3[index].ToUpper() + "']");
        if (xmlElement13 != null)
          newChild.SetAttribute("LP" + strArray3[index], xmlElement13.GetAttribute("FieldValue"));
      }
    }

    private void reMigrateTPOInformationTool()
    {
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode("EllieMae/TPO");
      if (newChild == null)
      {
        newChild = this.root.OwnerDocument.CreateElement("TPO");
        this.root.SelectSingleNode("EllieMae").AppendChild((XmlNode) newChild);
      }
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CUSTOM_FIELDS");
      if (xmlElement1 == null)
        return;
      XmlElement xmlElement2 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.SITE']");
      if (newChild.GetAttribute("SITEID") == "" && xmlElement2 != null)
        newChild.SetAttribute("SITEID", xmlElement2.GetAttribute("FieldValue"));
      string[] strArray1 = new string[3]
      {
        "RegisterDate",
        "SubmitDate",
        "DocumentsReadyDate"
      };
      for (int index = 0; index < strArray1.Length; ++index)
      {
        XmlElement xmlElement3 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO." + strArray1[index].ToUpper() + "']");
        if (xmlElement3 != null && Utils.ParseDate((object) xmlElement3.GetAttribute("FieldValue"), DateTime.MinValue) > Utils.ParseDate((object) newChild.GetAttribute(strArray1[index]), DateTime.MinValue))
          newChild.SetAttribute(strArray1[index], xmlElement3.GetAttribute("FieldValue"));
      }
      string[] strArray2 = new string[13]
      {
        "Name",
        "LegalName",
        "Address",
        "City",
        "State",
        "Zip",
        "Phone",
        "Fax",
        "Rating",
        "ManagerName",
        "ManagerEmail",
        "AEName",
        "AEUserName"
      };
      XmlElement xmlElement4 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.COMPANY']");
      if (newChild.GetAttribute("Company") == "" && xmlElement4 != null)
      {
        newChild.SetAttribute("Company", xmlElement4.GetAttribute("FieldValue"));
        XmlElement xmlElement5 = (XmlElement) newChild.SelectSingleNode("Company") ?? this.createPath("EllieMae/TPO/Company");
        for (int index = 0; index < strArray2.Length; ++index)
        {
          XmlElement xmlElement6 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.COMPANY" + strArray2[index].ToUpper() + "']");
          if (xmlElement6 != null)
            xmlElement5.SetAttribute(strArray2[index], xmlElement6.GetAttribute("FieldValue"));
        }
      }
      XmlElement xmlElement7 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.COMPANY']");
      if (xmlElement7 == null || !(newChild.GetAttribute("Company") == xmlElement7.GetAttribute("FieldValue")))
        return;
      XmlElement xmlElement8 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.BRANCH']");
      XmlElement xmlElement9 = (XmlElement) newChild.SelectSingleNode("Branch");
      if (newChild.GetAttribute("Branch") == "" && xmlElement8 != null)
      {
        newChild.SetAttribute("Branch", xmlElement8.GetAttribute("FieldValue"));
        if (xmlElement9 == null)
          xmlElement9 = this.createPath("EllieMae/TPO/Branch");
        for (int index = 0; index < strArray2.Length; ++index)
        {
          XmlElement xmlElement10 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.BRANCH" + strArray2[index].ToUpper() + "']");
          if (xmlElement10 != null)
            xmlElement9.SetAttribute(strArray2[index], xmlElement10.GetAttribute("FieldValue"));
        }
      }
      XmlElement xmlElement11 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.BRANCH']");
      if (xmlElement11 == null || !(newChild.GetAttribute("Branch") == xmlElement11.GetAttribute("FieldValue")))
        return;
      string[] strArray3 = new string[11]
      {
        "Name",
        "Email",
        "Status",
        "BusinessPhone",
        "BusinessFax",
        "CellPhone",
        "Address",
        "City",
        "State",
        "Zip",
        "Notes"
      };
      XmlElement xmlElement12 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LO']");
      if (newChild.GetAttribute("LOID") == "" && xmlElement12 != null)
      {
        newChild.SetAttribute("LOID", xmlElement12.GetAttribute("FieldValue"));
        for (int index = 0; index < strArray3.Length; ++index)
        {
          XmlElement xmlElement13 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LO" + strArray3[index].ToUpper() + "']");
          if (xmlElement13 != null)
            newChild.SetAttribute("LO" + strArray3[index], xmlElement13.GetAttribute("FieldValue"));
        }
      }
      XmlElement xmlElement14 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LP']");
      if (!(newChild.GetAttribute("LPID") == "") || xmlElement14 == null)
        return;
      newChild.SetAttribute("LPID", xmlElement14.GetAttribute("FieldValue"));
      for (int index = 0; index < strArray3.Length; ++index)
      {
        XmlElement xmlElement15 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.TPO.LP" + strArray3[index].ToUpper() + "']");
        if (xmlElement15 != null)
          newChild.SetAttribute("LP" + strArray3[index], xmlElement15.GetAttribute("FieldValue"));
      }
    }

    private void cleanupXml14203()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 14.203)
        return;
      this.versionMigrationOccured = true;
      this.migrateTPOInformationTool();
      this.EMXMLVersionID = "14.203";
    }

    private void cleanupXml14206()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 14.206)
        return;
      this.versionMigrationOccured = true;
      this.reMigrateTPOInformationTool();
      this.EMXMLVersionID = "14.206";
    }

    private void cleanupXml9102()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 9.102)
        return;
      this.versionMigrationOccured = true;
      this.EMXMLVersionID = "9.102";
    }

    private void cleanupXml40()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 4.0)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/FHA_VA_LOANS/HUD92900LT");
      if (xmlElement1 != null)
      {
        switch (xmlElement1.GetAttribute("ScoredByTotal"))
        {
          case "Y":
            xmlElement1.SetAttribute("ScoredByTotal", "Yes");
            break;
          case "N":
            xmlElement1.SetAttribute("ScoredByTotal", "No");
            break;
        }
      }
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/FHA_VA_LOANS");
      if (xmlElement2 != null)
      {
        switch (xmlElement2.GetAttribute("UtilityIncluded"))
        {
          case "Y":
            xmlElement2.SetAttribute("UtilityIncluded", "Yes");
            break;
          case "N":
            xmlElement2.SetAttribute("UtilityIncluded", "No");
            break;
        }
      }
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (borrowerPairs != null && borrowerPairs.Length != 0)
      {
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]", "_FirstName", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "_FirstName", true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]", "_LastName", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "_LastName", true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]", "_FirstName", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "_FirstName", true);
          this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]", "_LastName", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "_LastName", true);
        }
      }
      Dictionary<string, string> inputFormNameMapping = LoanMigrationData.GetInputFormNameMapping();
      foreach (XmlNode selectNode1 in this.root.SelectNodes("EllieMae/FormList"))
      {
        foreach (XmlElement selectNode2 in selectNode1.SelectNodes("Form[@html]"))
        {
          string attribute = selectNode2.GetAttribute("html");
          if (inputFormNameMapping.ContainsKey(attribute))
            selectNode2.SetAttribute("html", inputFormNameMapping[attribute]);
        }
      }
      this.migrateGeneralLogToTask();
      foreach (XmlNode selectNode3 in this.root.SelectNodes("EllieMae/SystemLog"))
      {
        foreach (XmlElement selectNode4 in selectNode3.SelectNodes("Record"))
        {
          switch (selectNode4.GetAttribute("Type"))
          {
            case "Condition":
              this.migrateComments(selectNode4);
              continue;
            case "Document":
              this.migrateComments(selectNode4);
              this.migrateMLC(selectNode4);
              continue;
            case "PostClosingCondition":
              this.migrateComments(selectNode4);
              continue;
            case "SellCondition":
              this.migrateComments(selectNode4);
              continue;
            default:
              continue;
          }
        }
      }
      this.EMXMLVersionID = "4.0";
    }

    private void migrateGeneralLogToTask()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/LOG/Record[@Type=\"Log\"]");
      if (xmlNodeList == null | xmlNodeList.Count == 0)
        return;
      XmlNode xmlNode = this.root.SelectSingleNode("EllieMae/SystemLog[@SysID='" + this.systemId + "']");
      if (xmlNode == null)
        return;
      foreach (XmlNode newChild in xmlNodeList)
      {
        XmlElement xmlElement = (XmlElement) newChild;
        xmlElement.SetAttribute("Type", "Task");
        xmlElement.SetAttribute("TaskName", xmlElement.GetAttribute("Desc"));
        xmlElement.SetAttribute("Stage", "");
        xmlElement.SetAttribute("Priority", "Optional");
        xmlElement.SetAttribute("TaskDescription", "");
        xmlElement.SetAttribute("AddedBy", xmlElement.GetAttribute("UserId"));
        xmlElement.SetAttribute("Priority", "Normal");
        xmlNode.AppendChild(newChild);
      }
    }

    private void migrateComments(XmlElement recordElm)
    {
      string attribute = recordElm.GetAttribute("Comments");
      if (string.IsNullOrEmpty(attribute))
        return;
      XmlElement element1 = this.xmldoc.CreateElement("Comments");
      recordElm.AppendChild((XmlNode) element1);
      XmlElement element2 = this.xmldoc.CreateElement("Entry");
      element2.SetAttribute("Comments", attribute);
      element1.AppendChild((XmlNode) element2);
      recordElm.RemoveAttribute("Comments");
    }

    private void migrateMLC(XmlElement recordElm)
    {
      if (this.toDouble(this.originalLoanVersion) >= 3.6 || recordElm.GetAttribute("IsCondition") != "Y")
        return;
      XmlElement element1 = this.xmldoc.CreateElement("Record");
      element1.SetAttribute("Guid", Guid.NewGuid().ToString());
      element1.SetAttribute("Type", "Condition");
      element1.SetAttribute("Title", recordElm.GetAttribute("Title"));
      element1.SetAttribute("Description", recordElm.GetAttribute("ConditionInfo"));
      element1.SetAttribute("Source", recordElm.GetAttribute("ConditionSource"));
      element1.SetAttribute("PairId", recordElm.GetAttribute("PairId"));
      element1.SetAttribute("DateAdded", DateTime.Now.ToString("M/d/yyyy HH:mm"));
      element1.SetAttribute("DateCleared", recordElm.GetAttribute("ReceiveDate"));
      recordElm.ParentNode.AppendChild((XmlNode) element1);
      XmlNode newChild = recordElm.SelectSingleNode("Conditions");
      if (newChild == null)
      {
        newChild = (XmlNode) this.xmldoc.CreateElement("Conditions");
        recordElm.AppendChild(newChild);
      }
      XmlElement element2 = this.xmldoc.CreateElement("ref");
      element2.SetAttribute("id", element1.GetAttribute("Guid"));
      newChild.AppendChild((XmlNode) element2);
    }

    private void cleanupXml365()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.65)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.toDouble(this.originalLoanVersion) > 3.53)
        {
          bool flag = true;
          XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/RateLock");
          if (xmlElement1 != null && (xmlElement1.GetAttribute("RequestFullfilledDateTime") ?? "") != string.Empty)
            flag = false;
          if (flag)
          {
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            DateTime dateTime = DateTime.MinValue;
            DateTime minValue = DateTime.MinValue;
            XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/LOG/Record");
            if (xmlNodeList != null && xmlNodeList.Count > 0)
            {
              foreach (XmlElement xmlElement2 in xmlNodeList)
              {
                string lower = (xmlElement2.GetAttribute("RequestedStatus", "") ?? "").ToLower();
                if (lower == "old lock" || lower == "locked")
                {
                  string attribute = xmlElement2.GetAttribute("Guid");
                  if ((attribute ?? "") != string.Empty)
                  {
                    XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/RateLock/RequestLogs/Log[@GUID=\"" + attribute + "\"]/FIELD[@id=\"2592\"]");
                    if (xmlElement3 != null)
                    {
                      DateTime date = Utils.ParseDate((object) xmlElement3.GetAttribute("val"));
                      if (lower == "locked")
                      {
                        dateTime = date;
                        break;
                      }
                      if (date != DateTime.MinValue && date > dateTime)
                        dateTime = date;
                    }
                  }
                }
              }
            }
            if (dateTime != DateTime.MinValue)
              xmlElement1.SetAttribute("RequestFullfilledDateTime", dateTime.ToString("MM/dd/yyyy hh:mm:ss tt"));
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml365 data migration for Field 2592: " + ex.Message);
      }
      this.EMXMLVersionID = "3.65";
    }

    private void cleanupXml364()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.64)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.toDouble(this.originalLoanVersion) != 3.53)
        {
          BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
          for (int index = 0; index < borrowerPairs.Length; ++index)
          {
            this.SetBorrowerPair(borrowerPairs[index]);
            XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/VA_BORROWER");
            if (xmlElement1 != null)
            {
              string attribute = xmlElement1.GetAttribute("VALocalTaxAmount");
              if (attribute != string.Empty)
              {
                this.createPath("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/MCAW").SetAttribute("Other", attribute);
                if (index == 0)
                {
                  XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"1161\"]") ?? this.createPath("LOCK/FIELD");
                  xmlElement2.SetAttribute("id", "1161");
                  xmlElement2.SetAttribute("val", attribute);
                }
              }
            }
          }
          this.SetBorrowerPair(borrowerPairs[0]);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml364 data migration for Field 1161: " + ex.Message);
      }
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (xmlElement != null)
        {
          foreach (XmlElement selectNode in xmlElement.SelectNodes("LOG/Record"))
          {
            if (selectNode.GetAttribute("Type") == "Fax")
            {
              string attribute = selectNode.GetAttribute("Name");
              if (attribute == string.Empty)
                attribute = selectNode.GetAttribute("Phone");
              selectNode.SetAttribute("Type", "Download");
              selectNode.SetAttribute("DownloadID", selectNode.GetAttribute("AttachmentID"));
              selectNode.SetAttribute("Title", selectNode.GetAttribute("PageCount") + " page fax");
              selectNode.SetAttribute("Sender", attribute);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml364 data migration for Log Record: " + ex.Message);
      }
      this.EMXMLVersionID = "3.64";
    }

    private void cleanupXml363()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.63)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/TSUM");
      if (xmlElement1 != null)
      {
        try
        {
          if (xmlElement1.GetAttribute("PropertyType") == "2-4 units")
            xmlElement1.SetAttribute("PropertyType", "2-4 Units");
        }
        catch (Exception ex)
        {
        }
      }
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/LOAN_FEATURES");
      if (xmlElement2 != null)
      {
        try
        {
          if (xmlElement2.GetAttribute("GSEPropertyType") == "Attached selected")
            xmlElement2.SetAttribute("GSEPropertyType", "Attached");
        }
        catch (Exception ex)
        {
        }
      }
      switch (this.GetFieldAt("1067"))
      {
        case "Existing":
          this.SetFieldAt("1067", "ExistingConstruction");
          break;
        case "Proposed":
          this.SetFieldAt("1067", "ProposedConstruction");
          break;
      }
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      this.SetBorrowerPair(borrowerPairs[0]);
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary", "CreditReceivedDate", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary", "CreditReceivedDate");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary", "DateOfBankruptcy", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary", "DateOfBankruptcy");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary", "PriorForeclosure", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary", "PriorForeclosure");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary", "DateOfForeclosure", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary", "DateOfForeclosure");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian30Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian30Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian60Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian60Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian90Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian90Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian120Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/ExperianCredit", "Experian120Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/TransUnionCredit", "Experian30Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion30Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion60Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion60Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion90Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion90Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion120Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/TransUnionCredit", "TransUnion120Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Experian30Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax30Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax60Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax60Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax90Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax90Days");
      this.moveSingleAttribute("EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax120Days", "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae/UNDERWRITER/Summary/EquifaxCredit", "Equifax120Days");
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        this.SetBorrowerPair(borrowerPairs[index]);
        XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/DEPOSIT");
        if (xmlNodeList != null && xmlNodeList.Count > 0)
        {
          foreach (XmlElement xmlElement3 in xmlNodeList)
          {
            if (xmlElement3.GetAttribute("Type1") == "GiftsNotDeposit")
              xmlElement3.SetAttribute("Type1", "GiftsNotDeposited");
            if (xmlElement3.GetAttribute("Type2") == "GiftsNotDeposit")
              xmlElement3.SetAttribute("Type2", "GiftsNotDeposited");
            if (xmlElement3.GetAttribute("Type3") == "GiftsNotDeposit")
              xmlElement3.SetAttribute("Type3", "GiftsNotDeposited");
            if (xmlElement3.GetAttribute("Type4") == "GiftsNotDeposit")
              xmlElement3.SetAttribute("Type4", "GiftsNotDeposited");
          }
        }
      }
      this.SetBorrowerPair(borrowerPairs[0]);
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("ASSET");
      if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
      {
        foreach (XmlElement xmlElement4 in xmlNodeList1)
        {
          if (xmlElement4.GetAttribute("_Type") == "GiftsNotDeposit")
            xmlElement4.SetAttribute("_Type", "GiftsNotDeposited");
        }
      }
      XmlNodeList xmlNodeList2 = this.root.SelectNodes("EllieMae/RateLock/RequestLogs/Log");
      if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (XmlNode xmlNode in xmlNodeList2)
        {
          foreach (XmlElement selectNode in xmlNode.SelectNodes("FIELD"))
          {
            switch (selectNode.GetAttribute("id"))
            {
              case "VASUMM.X23":
                selectNode.SetAttribute("id", "1484");
                flag1 = true;
                break;
              case "1041":
                if (selectNode.GetAttribute("val") == "Attached selected")
                  selectNode.SetAttribute("val", "Attached");
                flag2 = true;
                break;
            }
            if (flag2 & flag1)
              break;
          }
        }
      }
      if (this.toDouble(this.EMXMLVersionID) != 3.53)
      {
        double num1 = Utils.ParseDouble((object) this.GetFieldAt("559"));
        double num2 = Utils.ParseDouble((object) this.GetFieldAt("454"));
        double num3 = num1 + num2;
        if (num3 != 0.0)
          this.SetFieldAt("2591", num3.ToString("N2"));
        else
          this.SetFieldAt("2591", "");
        double num4 = num3 + Utils.ParseDouble((object) this.GetFieldAt("439"));
        if (num4 != 0.0)
          this.SetFieldAt("330", num4.ToString("N2"));
        else
          this.SetFieldAt("330", "");
        if (num1 != 0.0)
        {
          double num5 = Utils.ParseDouble((object) this.GetFieldAt("433"));
          double num6 = Utils.ParseDouble((object) this.GetFieldAt("435"));
          double num7 = num5 + num1;
          if (num7 != 0.0)
            this.SetFieldAt("433", num7.ToString("N2"));
          else
            this.SetFieldAt("433", "");
          double num8 = num6 + num1;
          if (num8 != 0.0)
            this.SetFieldAt("435", num8.ToString("N2"));
          else
            this.SetFieldAt("435", "");
        }
      }
      this.EMXMLVersionID = "3.63";
    }

    private void cleanupXml362()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.62)
        return;
      this.versionMigrationOccured = true;
      string str = string.Empty;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("_CLOSING_DOCUMENTS/LOAN_DETAILS");
        if (xmlElement != null)
          str = xmlElement.GetAttribute("DisbursementDate");
      }
      catch (Exception ex)
      {
      }
      if (str == string.Empty)
      {
        try
        {
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST/PrepaidInterest");
          if (xmlElement != null)
          {
            string attribute = xmlElement.GetAttribute("DateFrom");
            if (attribute != string.Empty)
            {
              if (attribute != "//")
                ((XmlElement) this.root.SelectSingleNode("_CLOSING_DOCUMENTS/LOAN_DETAILS") ?? this.createPath("_CLOSING_DOCUMENTS/LOAN_DETAILS"))?.SetAttribute("DisbursementDate", attribute);
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/LoanOriginationFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/LoanDiscountFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/AppraisalFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/CreditReportFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/LendersInspectionFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/MortgageInspectionFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/AssumptionFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/MortgageBrokerFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/TaxServiceFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/ProcessingFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UnderwritingFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/WireTransfer");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_813");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_814");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_815");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_816");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_817");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_818");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_819");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_820");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_821");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_822");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefinedFee_823");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/PrepaidInterest");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/MortgageInsurancePremium");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/HazardInsurancePremium");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/FloodInsuranceReserv");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/VAFundingFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_906");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_907");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/HazardInsurance");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/MortgageInsurance");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/SchoolTaxes");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/PropertyTaxes");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/FloodInsurance");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1006");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1007");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1008");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/ClosingFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/DocPrepFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/NotaryFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/AttorneyFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/TitleInsurance");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1109");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1110");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1111");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1112");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1113");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1114");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/RecordingFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/City_CountyTax_Stamps");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/StateTax_Stamps");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1204");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1205");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1206");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/SurveyFee");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/PestInspection");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1303");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1304");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1305");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1306");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1307");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1308");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/UserDefined_1309");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/CountyPropertyTaxes");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/TitleSearch");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/TitleExamination");
      this.migratePaidToInVersion362("EllieMae/CLOSING_COST/TitleBinder");
      this.EMXMLVersionID = "3.62";
    }

    private void migratePaidToInVersion362(string xnode)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(xnode);
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        switch (xmlElement.GetAttribute("PTB"))
        {
          case "Y":
            xmlElement.SetAttribute("PTB", "Broker");
            continue;
          case "N":
            xmlElement.SetAttribute("PTB", "");
            continue;
          default:
            continue;
        }
      }
    }

    private void cleanupXml36()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.6)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CUSTOM_FIELDS");
      if (xmlElement1 != null)
      {
        XmlNode xmlNode = xmlElement1.FirstChild;
        int num = 0;
        for (; xmlNode != null; xmlNode = xmlNode.NextSibling)
        {
          if (xmlNode is XmlElement xmlElement2)
          {
            ++num;
            xmlElement2.SetAttribute("FieldID", "CUST" + num.ToString("#00") + "FV");
          }
        }
      }
      this.EMXMLVersionID = "3.6";
    }

    private void cleanupXml351()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.51)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/PREPAYMENT_PENALTY");
      if (xmlElement1 != null)
      {
        try
        {
          string attribute = xmlElement1.GetAttribute("_PeriodSequenceIdentifier");
          xmlElement1.RemoveAttribute("_PeriodSequenceIdentifier");
          if (this.toDouble(attribute) > 0.0)
            xmlElement1.SetAttribute("_TermMonths", attribute);
        }
        catch (Exception ex)
        {
        }
      }
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement2 != null)
      {
        foreach (XmlElement selectNode in xmlElement2.SelectNodes("LOG/Record"))
        {
          if (selectNode.GetAttribute("Type") == "Offer")
          {
            string attribute = selectNode.GetAttribute("Eligible");
            selectNode.SetAttribute("InLog", attribute);
          }
        }
      }
      this.EMXMLVersionID = "3.51";
    }

    private void cleanupXml35()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.5)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/LOAN_FEATURES");
      if (xmlElement1 != null)
      {
        bool flag = xmlElement1.GetAttribute("PaymentFrequencyType") == "Biweekly";
        DateTime minValue = DateTime.MinValue;
        string attribute = xmlElement1.GetAttribute("ScheduledFirstPaymentDate");
        if (attribute != string.Empty)
        {
          if (attribute != null)
          {
            try
            {
              minValue = DateTime.Parse(attribute);
            }
            catch (Exception ex)
            {
              minValue = DateTime.MinValue;
            }
          }
        }
        XmlNodeList xmlNodeList1 = this.root.SelectNodes("EllieMae/HUD1ES/Setup");
        if (xmlNodeList1 != null && minValue != DateTime.MinValue)
        {
          XmlNodeList xmlNodeList2 = this.root.SelectNodes("EllieMae/HUD1ES/DueDate");
          if (xmlNodeList2 != null)
          {
            foreach (XmlElement oldChild in xmlNodeList2)
              oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
          }
          for (int index = 1; index <= 4; ++index)
            this.createPath("EllieMae/HUD1ES/DueDate[" + index.ToString() + "]");
          for (int index = 1; index <= 8; ++index)
          {
            string name = string.Empty;
            switch (index)
            {
              case 1:
                name = "TaxDisb";
                break;
              case 2:
                name = "HazInsDisb";
                break;
              case 3:
                name = "MtgInsDisb";
                break;
              case 4:
                name = "FloodInsDisb";
                break;
              case 5:
                name = "SchoolTaxes";
                break;
              case 6:
                name = "UserDefined1";
                break;
              case 7:
                name = "UserDefined2";
                break;
              case 8:
                name = "UserDefined3";
                break;
            }
            int num = 0;
            DateTime dateTime = minValue;
            foreach (XmlElement xmlElement2 in xmlNodeList1)
            {
              if (xmlElement2.GetAttribute(name) != string.Empty)
              {
                ++num;
                if (num <= 4)
                  ((XmlElement) this.root.SelectSingleNode("EllieMae/HUD1ES/DueDate[" + (object) num + "]"))?.SetAttribute(name, dateTime.ToString("MM/dd/yyyy"));
                else
                  break;
              }
              dateTime = !flag ? dateTime.AddMonths(1) : dateTime.AddDays(14.0);
            }
          }
        }
      }
      XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/COMMITMENT_TERMS");
      if (xmlElement3 != null)
      {
        string attribute = xmlElement3.GetAttribute("TotalMonhlyExpense");
        try
        {
          xmlElement3.RemoveAttribute("TotalMonhlyExpense");
          xmlElement3.SetAttribute("TotalMonthlyExpense", attribute);
        }
        catch (Exception ex)
        {
        }
      }
      this.EMXMLVersionID = "3.5";
    }

    private void cleanupXml34()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 3.4)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement1 != null)
      {
        if (xmlElement1.GetAttribute("MS_STATUS") == "Closed")
          xmlElement1.SetAttribute("MS_STATUS", "Completed");
        foreach (XmlElement selectNode in xmlElement1.SelectNodes("LOG/Milestone"))
        {
          if (selectNode.GetAttribute("Stage") == "Closing")
            selectNode.SetAttribute("Stage", "Completion");
        }
        foreach (XmlElement selectNode in xmlElement1.SelectNodes("LOG/Record"))
        {
          if (selectNode.GetAttribute("Stage") == "Closing")
            selectNode.SetAttribute("Stage", "Completion");
          string attribute1 = selectNode.GetAttribute("Type");
          if (attribute1 == "Document" || attribute1 == "Verif")
          {
            selectNode.SetAttribute("eFolder", "Y");
            selectNode.SetAttribute("eFolder", selectNode.GetAttribute("eDisclosure"));
            XmlElement xmlElement2 = selectNode;
            DateTime dateTime = DateTime.MinValue;
            dateTime = dateTime.Date;
            string str = dateTime.ToString("M/d/yyyy hh:mm:ss tt");
            xmlElement2.SetAttribute("ArchiveDate", str);
            if (selectNode.GetAttribute("IsCondition") == "Y")
            {
              string attribute2 = selectNode.GetAttribute("Comments");
              if (attribute2 != string.Empty)
              {
                selectNode.SetAttribute("Comments", string.Empty);
                selectNode.SetAttribute("ConditionInfo", attribute2);
              }
            }
            string attribute3 = selectNode.GetAttribute("Title");
            if (attribute3 != null && !(attribute3 == string.Empty))
            {
              string lower1 = attribute3.ToLower();
              if (lower1 == "fraud")
                selectNode.SetAttribute("Title", "Fraud/Audit Services");
              string attribute4 = selectNode.GetAttribute("Company");
              if (attribute4 != null && !(attribute4 == string.Empty))
              {
                string lower2 = attribute4.ToLower();
                if (selectNode.GetAttribute("IsePASS").ToLower() == "y")
                {
                  switch (lower2)
                  {
                    case "affinity":
                      if (lower1 == "other services")
                      {
                        selectNode.SetAttribute("Title", "Fraud/Audit Services");
                        continue;
                      }
                      continue;
                    case "appintell":
                      switch (lower1)
                      {
                        case "other services":
                          selectNode.SetAttribute("Title", "Fraud/Audit Services");
                          continue;
                        case "flood certificate":
                          selectNode.SetAttribute("Title", "Other Services");
                          continue;
                        default:
                          continue;
                      }
                    case "countrywide":
                      if (lower1 == "lender submission")
                      {
                        selectNode.SetAttribute("EPASSSignature", "");
                        continue;
                      }
                      continue;
                    case "indymac":
                      if (lower1 == "underwriting")
                      {
                        selectNode.SetAttribute("Title", "Lender Submission");
                        selectNode.SetAttribute("EPASSSignature", "");
                        continue;
                      }
                      continue;
                    case "truapp":
                      if (lower1 == "fraud report")
                      {
                        selectNode.SetAttribute("Title", "Fraud/Audit Services");
                        continue;
                      }
                      continue;
                    default:
                      continue;
                  }
                }
              }
            }
          }
        }
        string attribute = xmlElement1.GetAttribute("DatePrepared");
        if (attribute != string.Empty)
          ((XmlElement) xmlElement1.SelectSingleNode("STATEDISCLOSURE/NEWYORK") ?? this.createPath("EllieMae/STATEDISCLOSURE/NEWYORK"))?.SetAttribute("ApplicationDate", attribute);
        XmlElement xmlElement3 = (XmlElement) xmlElement1.SelectSingleNode("STATEMENT_CREDIT_DENIAL");
        string str1;
        if (xmlElement3 != null)
        {
          str1 = xmlElement3.GetAttribute("AdverseActionDate");
          xmlElement3.RemoveAttribute("AdverseActionDate");
        }
        else
          str1 = string.Empty;
        xmlElement1.SetAttribute("AdverseActionDate", str1);
        if (str1 != string.Empty)
          ((XmlElement) xmlElement1.SelectSingleNode("STATEMENT_CREDIT_DENIAL") ?? this.createPath("EllieMae/STATEMENT_CREDIT_DENIAL"))?.SetAttribute("DenialDate", str1);
      }
      this.EMXMLVersionID = "3.4";
    }

    private void cleanupXml15100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 15.1)
        return;
      this.versionMigrationOccured = true;
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/REGZ/PAYMENT_SCHEDULE");
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/ULDD");
      if (xmlNodeList != null && xmlNodeList.Count >= 2 && xmlElement1 != null)
      {
        XmlElement xmlElement2 = (XmlElement) xmlNodeList[1];
        if (xmlElement2 != null)
          xmlElement1.SetAttribute("FirstRateChangePaymentEffectiveDate", xmlElement2.GetAttribute("PaymentDate"));
      }
      if (this.GetFieldAt("3969") == "")
      {
        if (this.GetFieldAt("NEWHUD.X354") == "Y")
          this.SetFieldAt("3969", "RESPA 2010 GFE and HUD-1");
        else
          this.SetFieldAt("3969", "Old GFE and HUD-1");
      }
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101eBorrowerSelects", "EllieMae/CLOSING_COST[1]/NewHud/Section1100", "Line1101fBorrowerSelects", false);
      this.UpdateBaseLTVRatioPercent();
      this.EMXMLVersionID = "15.100";
    }

    private void UpdateBaseLTVRatioPercent()
    {
      string fieldAt = this.GetFieldAt("19");
      Decimal num1 = Utils.ParseDecimal((object) this.GetFieldAt("1109"), 0M);
      Decimal num2 = Utils.ParseDecimal((object) this.GetFieldAt("356"), 0M);
      Decimal num3 = Utils.ParseDecimal((object) this.GetFieldAt("1821"), 0M);
      Decimal num4 = Utils.ParseDecimal((object) this.GetFieldAt("136"), 0M);
      Decimal num5 = 0M;
      if (num1 > 0M)
      {
        switch (fieldAt)
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
            this.SetFieldAt("ULDD.X186", (Decimal.Truncate(d) + 1M).ToString());
            this.SetFieldAt("4012", (Decimal.Truncate(d) + 1M).ToString());
          }
          else
          {
            this.SetFieldAt("ULDD.X186", Decimal.Truncate(d).ToString());
            this.SetFieldAt("4012", Decimal.Truncate(d).ToString());
          }
        }
        else
        {
          this.SetFieldAt("ULDD.X186", "");
          this.SetFieldAt("4012", "");
        }
      }
      else
      {
        this.SetFieldAt("ULDD.X186", "");
        this.SetFieldAt("4012", "");
      }
    }

    private void cleanupXml15145()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 15.145)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanEstimate2015");
      if (xmlElement1 != null && xmlElement1.HasAttribute("SignatureType") && xmlElement1.GetAttribute("SignatureType").Trim() == "")
        xmlElement1.SetAttribute("SignatureType", "AsApplicant");
      if (xmlElement1 != null && !xmlElement1.HasAttribute("SignatureType"))
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("SignatureType");
        attribute.Value = "AsApplicant";
        xmlElement1.Attributes.Append(attribute);
      }
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015");
      if (xmlElement2 != null && xmlElement2.HasAttribute("SignatureType") && xmlElement2.GetAttribute("SignatureType").Trim() == "")
        xmlElement2.SetAttribute("SignatureType", "AsApplicant");
      if (xmlElement2 != null && !xmlElement2.HasAttribute("SignatureType"))
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("SignatureType");
        attribute.Value = "AsApplicant";
        xmlElement2.Attributes.Append(attribute);
      }
      this.EMXMLVersionID = "15.145";
    }

    private void cleanupXml15149()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 15.149)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015");
      if (xmlElement1 != null && !xmlElement1.HasAttribute("ExcludeBorrowerClosingCosts"))
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("ExcludeBorrowerClosingCosts");
        attribute.Value = "Y";
        xmlElement1.Attributes.Append(attribute);
      }
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015");
      if (xmlElement2 != null && !xmlElement2.HasAttribute("IgnoreARMAdj"))
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("IgnoreARMAdj");
        attribute.Value = "Y";
        xmlElement2.Attributes.Append(attribute);
      }
      XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanEstimate2015");
      if (xmlElement3 != null && !xmlElement3.HasAttribute("UseActualDownPaymentAndClosingCostsFinancedIndicator"))
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("UseActualDownPaymentAndClosingCostsFinancedIndicator");
        attribute.Value = "Y";
        xmlElement3.Attributes.Append(attribute);
      }
      this.EMXMLVersionID = "15.149";
    }

    private void cleanupXml151491()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 15.1491)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement1 != null && xmlElement1.HasAttribute("UseNewForms2015") && xmlElement1.GetAttribute("UseNewForms2015") == "RESPA-TILA 2015 LE and CD")
      {
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud/Section800/LOCompensation");
        if (xmlElement2 != null && xmlElement2.GetAttribute("UseLOCompensationTool") != "Y")
          xmlElement2.SetAttribute("UseLOCompensationTool", "Y");
      }
      this.EMXMLVersionID = "15.1491";
    }

    private void cleanupXml15208()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 15.208)
        return;
      this.versionMigrationOccured = true;
      this.EMXMLVersionID = "15.208";
    }

    private void cleanupXml15209()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 15.209)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("MORTGAGE_TERMS", "AgencyCaseIdentifier", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "MICReference", false);
      DateTime dateTime = new DateTime(2016, 1, 1);
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/HMDA");
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement1 != null && xmlElement2 != null && xmlElement2.HasAttribute("AdverseActionDate"))
      {
        string attribute1 = xmlElement1.GetAttribute("ActionTaken");
        DateTime date = Utils.ParseDate((object) xmlElement2.GetAttribute("AdverseActionDate"));
        if (attribute1 == "" || attribute1 != "" && date >= dateTime)
        {
          XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/ATR_QM/Eligibility");
          if (xmlElement3 != null && xmlElement3.HasAttribute("ATRLoanType"))
          {
            string attribute2 = xmlElement3.GetAttribute("ATRLoanType");
            string empty = string.Empty;
            string str;
            switch (attribute2)
            {
              case "Qualified Mortgage":
                str = "QM";
                break;
              case "General ATR":
                str = "Non-QM";
                break;
              case "Exempt":
              case "Non-Standard to Standard Refinance":
                str = "Exempt";
                break;
              default:
                str = "";
                break;
            }
            if (!xmlElement1.HasAttribute("QMStatus"))
            {
              XmlAttribute attribute3 = this.xmldoc.CreateAttribute("QMStatus");
              attribute3.Value = str;
              xmlElement1.Attributes.Append(attribute3);
            }
            else
              xmlElement1.SetAttribute("QMStatus", str);
          }
        }
      }
      this.EMXMLVersionID = "15.209";
    }

    private void cleanupXml16102()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 16.102)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC1MIAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC1MIAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC1EstimatedEscrowAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC1EstimatedEscrowAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC2MIAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC2MIAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC2EstimatedEscrowAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC2EstimatedEscrowAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC3MIAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC3MIAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC3EstimatedEscrowAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC3EstimatedEscrowAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC4MIAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC4MIAmountUI", false);
      this.copySingleAttribute("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC4EstimatedEscrowAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "PPC4EstimatedEscrowAmountUI", false);
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanEstimate2015");
      if (xmlElement1 != null)
      {
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015") ?? this.createPath("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015");
        for (int index = 1; index <= 4; ++index)
        {
          if (xmlElement1.HasAttribute("PPC" + (object) index + "MIAmountUI") && (xmlElement1.GetAttribute("PPC" + (object) index + "MIAmountUI") == "0" || xmlElement1.GetAttribute("PPC" + (object) index + "MIAmountUI") == "-"))
            xmlElement2.SetAttribute("PPC" + (object) index + "MIAmountUI", xmlElement1.GetAttribute("PPC" + (object) index + "MIAmountUI"));
          if (xmlElement1.HasAttribute("PPC" + (object) index + "EstimatedEscrowAmountUI") && (xmlElement1.GetAttribute("PPC" + (object) index + "EstimatedEscrowAmountUI") == "0" || xmlElement1.GetAttribute("PPC" + (object) index + "EstimatedEscrowAmountUI") == "-"))
            xmlElement2.SetAttribute("PPC" + (object) index + "EstimatedEscrowAmountUI", xmlElement1.GetAttribute("PPC" + (object) index + "EstimatedEscrowAmountUI"));
        }
      }
      XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanEstimate2015");
      if (xmlElement3 != null && xmlElement3.HasAttribute("PrepaymentPenaltyPayOffDuringYear") && xmlElement3.GetAttribute("PrepaymentPenaltyPayOffDuringYear").Trim() != "")
      {
        XmlAttribute attribute1 = this.xmldoc.CreateAttribute("PrepaymentPenaltyPayOffInDateType");
        XmlAttribute attribute2 = this.xmldoc.CreateAttribute("PrepaymentPenaltyPayOffInFirstYear");
        int num = Utils.ParseInt((object) xmlElement3.GetAttribute("PrepaymentPenaltyPayOffDuringYear").Trim());
        if (num == 1)
        {
          attribute1.Value = "Year";
          attribute2.Value = "First";
        }
        else if (num > 1)
        {
          attribute1.Value = "Years";
          attribute2.Value = num.ToString();
        }
        if (attribute1.Value != "")
          xmlElement3.Attributes.Append(attribute2);
        if (attribute2.Value != "")
          xmlElement3.Attributes.Append(attribute1);
      }
      this.EMXMLVersionID = "16.102";
    }

    private void cleanupXml16103()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 16.103)
        return;
      this.versionMigrationOccured = true;
      this.EMXMLVersionID = "16.103";
    }

    private void cleanupXml16201()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 16.201)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
      if (xmlElement != null && xmlElement.HasAttribute("IsConstructionPhaseDisclosedSeparately"))
      {
        string str = "";
        if (xmlElement.HasAttribute("_Type"))
          str = xmlElement.GetAttribute("_Type");
        if (str != "ConstructionOnly")
          xmlElement.RemoveAttribute("IsConstructionPhaseDisclosedSeparately");
      }
      this.EMXMLVersionID = "16.201";
    }

    private void cleanupXml17100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.1)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("EllieMae/REGZ", "InterestOnlyMonths", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "InterestOnlyPaymentMonths", false);
      this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "ScheduledFirstPaymentDate", "EllieMae/HUD1ES", "EscrowFirstPaymentDate", false);
      XmlElement oldChild1 = (XmlElement) this.root.SelectSingleNode("EllieMae/GeneralContractor");
      if (oldChild1 != null)
      {
        XmlAttributeCollection attributes = oldChild1.Attributes;
        for (int index = 0; index < attributes.Count; ++index)
        {
          XmlNode xmlNode = attributes.Item(index);
          this.copySingleAttribute("EllieMae/GeneralContractor", xmlNode.Name, "EllieMae/GENERALCONTRACTOR", xmlNode.Name, false);
        }
        oldChild1.ParentNode?.RemoveChild((XmlNode) oldChild1);
      }
      XmlElement oldChild2 = (XmlElement) this.root.SelectSingleNode("EllieMae/Architect");
      if (oldChild2 != null)
      {
        XmlAttributeCollection attributes = oldChild2.Attributes;
        for (int index = 0; index < attributes.Count; ++index)
        {
          XmlNode xmlNode = attributes.Item(index);
          this.copySingleAttribute("EllieMae/Architect", xmlNode.Name, "EllieMae/ARCHITECT", xmlNode.Name, false);
        }
        oldChild2.ParentNode?.RemoveChild((XmlNode) oldChild2);
      }
      XmlElement oldChild3 = (XmlElement) this.root.SelectSingleNode("EllieMae/Builder");
      if (oldChild3 != null)
      {
        XmlAttributeCollection attributes = oldChild3.Attributes;
        for (int index = 0; index < attributes.Count; ++index)
        {
          XmlNode xmlNode = attributes.Item(index);
          this.copySingleAttribute("EllieMae/Builder", xmlNode.Name, "EllieMae/BUILDER", xmlNode.Name, false);
        }
        oldChild3.ParentNode?.RemoveChild((XmlNode) oldChild3);
      }
      this.EMXMLVersionID = "17.100";
    }

    private void cleanupXml17104()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.104)
        return;
      this.versionMigrationOccured = true;
      if (string.Equals(this.GetFieldAtXpath("EllieMae/@Channel"), "Correspondent", StringComparison.CurrentCultureIgnoreCase) && Utils.ParseDate((object) this.GetFieldAtXpath("EllieMae/RateLock/PurchaseAdvice/Correspondent/@Date"), DateTime.MinValue) != DateTime.MinValue)
      {
        if (Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/Correspondent/@OriginalPrincipalBalance"), 0M) == 0M && Utils.ParseDecimal((object) this.GetFieldAtXpath("MORTGAGE_TERMS/@BaseLoanAmount"), 0M) > 0M)
          this.copySingleAttribute("MORTGAGE_TERMS", "BaseLoanAmount", "EllieMae/Correspondent", "OriginalPrincipalBalance", false);
        if (Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/Correspondent/@UnpaidPrincipalBalance"), 0M) <= 0M && Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/RateLock/PurchaseAdvice/Correspondent/@CurrentPrincipal"), 0M) > 0M)
          this.copySingleAttribute("EllieMae/RateLock/PurchaseAdvice/Correspondent", "CurrentPrincipal", "EllieMae/Correspondent", "UnpaidPrincipalBalance", false);
        if (Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/InterimServicing/Summary/PurchaseAdvice/@Principle"), 0M) == 0M && Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/RateLock/PurchaseAdvice/Correspondent/@CurrentPrincipal"), 0M) > 0M)
          this.copySingleAttribute("EllieMae/RateLock/PurchaseAdvice/Correspondent", "CurrentPrincipal", "EllieMae/InterimServicing/Summary/PurchaseAdvice", "Principle", false);
        if (Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/Correspondent/@UnpaidPrincipalBalance"), 0M) > 0M && Utils.ParseDecimal((object) this.GetFieldAtXpath("EllieMae/ULDD/@UPBAmount"), 0M) == 0M)
          this.copySingleAttribute("EllieMae/Correspondent", "UnpaidPrincipalBalance", "EllieMae/ULDD", "UPBAmount", false);
      }
      this.EMXMLVersionID = "17.104";
    }

    private void cleanupXml17200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.2)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae");
      if (xmlElement1 != null && !xmlElement1.HasAttribute("Print2003Application"))
      {
        XmlAttribute attribute = this.xmldoc.CreateAttribute("Print2003Application");
        attribute.Value = "2009";
        xmlElement1.Attributes.Append(attribute);
      }
      try
      {
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/HMDA");
        XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"4180\"]");
        if ((xmlElement2 != null ? xmlElement2.GetAttribute("ActionTaken") : string.Empty) != string.Empty)
        {
          if (xmlElement3 == null)
          {
            double num = this.GetFieldAt("1116") == string.Empty ? 0.0 : Utils.ParseDouble((object) this.GetFieldAt("1116"));
            this.SetFieldAt("4180", num.ToString());
            this.createLockField("4180", num.ToString());
          }
        }
        else
        {
          double num1 = (this.GetFieldAt("356") == string.Empty ? 0.0 : (double) Convert.ToInt32(this.GetFieldAt("356"))) * 0.04;
          this.SetFieldAt("4180", num1.ToString());
          double num2 = (this.GetFieldAt("135") == string.Empty ? 0.0 : Utils.ParseDouble((object) this.GetFieldAt("135"))) - num1;
          if (num2 < 0.0)
            num2 = 0.0;
          this.SetFieldAt("3053", num2.ToString());
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "17.200";
    }

    private void cleanupXml17202()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.202)
        return;
      this.versionMigrationOccured = true;
      this.copySingleAttribute("PROPERTY", "_StreetAddress", "EllieMae/HMDA", "HmdaPropertyAddress", true);
      this.copySingleAttribute("PROPERTY", "_City", "EllieMae/HMDA", "HmdaPropertyCity", true);
      this.copySingleAttribute("PROPERTY", "_State", "EllieMae/HMDA", "HmdaPropertyState", true);
      this.EMXMLVersionID = "17.202";
    }

    private void cleanupXml17206()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.206)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"1341\"]") == null)
        {
          BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
          if (borrowerPairs != null)
          {
            if (borrowerPairs.Length != 0)
            {
              string fieldAtXpath = this.GetFieldAtXpath("BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/" + "EllieMae/PAIR/FHA_VA_LOANS/@DebtIncomeRatio");
              if (fieldAtXpath != string.Empty)
                this.createLockField("1341", fieldAtXpath);
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"QM.X376\"]");
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"QM.X116\"]");
        XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"QM.X119\"]");
        if (xmlElement1 == null && this.GetFieldAt("QM.X376") != string.Empty)
          this.createLockField("QM.X376", this.GetFieldAt("QM.X376"));
        if (xmlElement2 == null && this.GetFieldAt("QM.X116") != string.Empty)
          this.createLockField("QM.X116", this.GetFieldAt("QM.X116"));
        if (xmlElement3 == null)
        {
          if (this.GetFieldAt("QM.X119") != string.Empty)
            this.createLockField("QM.X119", this.GetFieldAt("QM.X119"));
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "17.206";
    }

    private void cleanupXml17300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.3)
        return;
      this.versionMigrationOccured = true;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER/GOVERNMENT_MONITORING");
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("BORROWER/EllieMae");
      if (xmlElement1 != null)
      {
        try
        {
          if (xmlElement1.GetAttribute("GenderType") == "No co-applicant")
          {
            foreach (XmlElement xmlElement2 in xmlNodeList1)
            {
              if (!string.IsNullOrEmpty(xmlElement2 != null ? xmlElement2.GetAttribute("NoCoApplicantEthnicityIndicator") : string.Empty))
              {
                xmlElement1.SetAttribute("NoCoApplicantEthnicityIndicator", "Y");
                break;
              }
            }
          }
        }
        catch
        {
        }
      }
      if (xmlNodeList1 != null)
      {
        try
        {
          foreach (XmlElement xmlElement3 in xmlNodeList1)
          {
            if ((xmlElement3 != null ? xmlElement3.GetAttribute("Ethnicity") : string.Empty) == "No co-applicant")
              xmlElement3.SetAttribute("NoCoApplicantSexIndicator", "Y");
          }
        }
        catch
        {
        }
      }
      try
      {
        XmlNodeList xmlNodeList2 = this.root.SelectNodes("BORROWER");
        string name = "";
        for (int i = 0; i < xmlNodeList2.Count; ++i)
        {
          string str1 = xmlNodeList2[i].Attributes["BorrowerID"].Value;
          XmlNode xmlNode = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + str1 + "\"]/GOVERNMENT_MONITORING/@GenderType");
          string str2 = "BORROWER[@BorrowerID=\"" + str1 + "\"]/GOVERNMENT_MONITORING";
          if (xmlNode != null)
          {
            switch (xmlNode.Value)
            {
              case "Female":
                name = "HmdaGendertypeFemaleIndicator";
                break;
              case "Male":
                name = "HmdaGendertypeMaleIndicator";
                break;
              case "InformationNotProvidedUnknown":
                name = "HmdaGendertypeDoNotWishIndicator";
                break;
              case "NotApplicable":
                name = "HmdaGendertypeNotApplicableIndicator";
                break;
            }
            ((XmlElement) this.root.SelectSingleNode(str2) ?? this.createPath(str2)).SetAttribute(name, "Y");
          }
        }
      }
      catch
      {
      }
      try
      {
        if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"MAX23K.X109\"]") == null && this.GetFieldAt("MAX23K.X109") != string.Empty)
          this.createLockField("MAX23K.X109", this.GetFieldAt("MAX23K.X109"));
        this.copySingleAttribute("EllieMae/FHA_VA_LOANS", "ExistingDebtAmount", "EllieMae/FHA_VA_LOANS/FHA_203K", "UnpaidPrincipalBalanceFirstLien", false);
        this.copySingleAttribute("EllieMae/FHA_VA_LOANS", "ExistingDebtAmount", "EllieMae/FHA_VA_LOANS/FHA_203K", "Existing203KDebtTotal", false);
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"MAX23K.X103\"]");
        XmlElement xmlElement5 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"MAX23K.X93\"]");
        if (xmlElement4 == null && this.GetFieldAt("MAX23K.X103") != string.Empty)
          this.createLockField("MAX23K.X103", this.GetFieldAt("MAX23K.X103"));
        if (xmlElement5 == null && this.GetFieldAt("MAX23K.X93") != string.Empty)
          this.createLockField("MAX23K.X93", this.GetFieldAt("MAX23K.X93"));
        this.copySingleAttribute("EllieMae/FHA_VA_LOANS/FHA_203K/REHABILITATION_COSTS", "IndependentConsultantFee", "EllieMae/FHA_VA_LOANS/FHA_203K", "InitialDrawAtClosingConsultantFees", false);
        this.copySingleAttribute("EllieMae/FHA_VA_LOANS/FHA_203K/REHABILITATION_COSTS", "ArchitecturalEngineeringFee", "EllieMae/FHA_VA_LOANS/FHA_203K", "InitialDrawAtClosingArchitecturalorEngineeringFees", false);
        this.copySingleAttribute("EllieMae/FHA_VA_LOANS/FHA_203K/REHABILITATION_COSTS", "PermitsAndOtherFee", "EllieMae/FHA_VA_LOANS/FHA_203K", "InitialDrawAtClosingPermitFees", false);
      }
      catch
      {
      }
      try
      {
        this.copySingleAttribute("TRANSACTION_DETAIL", "PurchasePriceAmount", "EllieMae/CLOSING_COST[1]", "DisclosedSalesPrice", false);
        if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"L726\"]") == null)
        {
          if (this.GetFieldAt("L726") != string.Empty)
            this.createLockField("L726", this.GetFieldAt("L726"));
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "17.300";
    }

    private void cleanupXml17303()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.303)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string xpath = "LOAN_PURPOSE/CONSTRUCTION_REFINANCE_DATA";
        string fieldId = "CASASRN.X79";
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(xpath);
        if (xmlElement != null)
        {
          string attribute = xmlElement.GetAttribute("FRECashOutAmount");
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + fieldId + "\"]") == null)
          {
            if (attribute != string.Empty)
              this.createLockField(fieldId, attribute);
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "17.303";
    }

    private void cleanupXml17307()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.307)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string xpath = "LOAN_PURPOSE/CONSTRUCTION_REFINANCE_DATA";
        string fieldId = "CASASRN.X79";
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(xpath);
        if (xmlElement != null)
        {
          string attribute = xmlElement.GetAttribute("FRECashOutAmount");
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + fieldId + "\"]") == null)
          {
            if (attribute != string.Empty)
              this.createLockField(fieldId, attribute);
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "17.307";
    }

    private void cleanupXml17400()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 17.4)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string xpath = "EllieMae/CLOSING_COST[1]/LoanEstimate2015";
        string name = "EstimatedTaxesInsuranceAssessments";
        string fieldId = "LE1.X29";
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/LOAN_FEATURES");
        if (xmlElement1 != null)
        {
          if (xmlElement1.GetAttribute("PaymentFrequencyType") == "Biweekly")
          {
            for (int index = 1; index <= 4; ++index)
            {
              switch (index)
              {
                case 2:
                case 3:
                  xpath = "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015";
                  fieldId = index == 2 ? "CD1.X3" : "CD4.X10";
                  name = index == 2 ? "EstimatedTaxesInsuranceAssessments" : "MonthlyEscrowPayment";
                  break;
                case 4:
                  xpath = "EllieMae/HUD1ES/BiWeekly";
                  name = "TotalBiweeklyPaymentToEscrow";
                  fieldId = "HUD65";
                  break;
              }
              XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode(xpath);
              if (xmlElement2 != null)
              {
                string attribute = xmlElement2.GetAttribute(name);
                if (!(attribute == "") && attribute != null && (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + fieldId + "\"]") == null)
                  this.createLockField(fieldId, attribute);
              }
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER");
        string name = "";
        for (int i = 0; i < xmlNodeList.Count; ++i)
        {
          string str1 = xmlNodeList[i].Attributes["BorrowerID"].Value;
          XmlNode xmlNode = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + str1 + "\"]/EllieMae/@Ethnicity");
          string str2 = "BORROWER[@BorrowerID=\"" + str1 + "\"]/EllieMae";
          if (xmlNode != null)
          {
            switch (xmlNode.Value)
            {
              case "Hispanic or Latino":
                name = "HmdaEthnicityHispanicLatinoIndicator";
                break;
              case "Not Hispanic or Latino":
                name = "HmdaEthnicityNotHispanicLatinoIndicator";
                break;
              case "Information not provided":
                name = "HmdaEthnicityDoNotWishIndicator";
                break;
              case "Not applicable":
                name = "HmdaEthnicityNotApplicableIndicator";
                break;
            }
            ((XmlElement) this.root.SelectSingleNode(str2) ?? this.createPath(str2)).SetAttribute(name, "Y");
          }
        }
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"HMDA.X32\"]");
        XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"HMDA.X36\"]");
        XmlElement xmlElement5 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"HMDA.X37\"]");
        if (xmlElement3 == null && this.GetFieldAt("HMDA.X32") != string.Empty)
          this.createLockField("HMDA.X32", this.GetFieldAt("HMDA.X32"));
        if (xmlElement4 == null && this.GetFieldAt("HMDA.X36") != string.Empty)
          this.createLockField("HMDA.X36", this.GetFieldAt("HMDA.X36"));
        if (xmlElement5 == null)
        {
          if (this.GetFieldAt("HMDA.X37") != string.Empty)
            this.createLockField("HMDA.X37", this.GetFieldAt("HMDA.X37"));
        }
      }
      catch
      {
      }
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER");
        for (int i = 0; i < xmlNodeList.Count; ++i)
        {
          string str = xmlNodeList[i].Attributes["BorrowerID"].Value;
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + str + "\"]/EllieMae");
          if (xmlElement != null)
          {
            string attribute1 = xmlElement.GetAttribute("_FirstName");
            string attribute2 = xmlElement.GetAttribute("_MiddleName");
            XmlNode xmlNode = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + str + "\"]/EllieMae/ULDD") ?? (XmlNode) this.createPath("BORROWER[@BorrowerID=\"" + str + "\"]/EllieMae/ULDD");
            if (xmlNode != null)
            {
              ((XmlElement) xmlNode).SetAttribute("Fannie_FirstName", attribute1);
              ((XmlElement) xmlNode).SetAttribute("Fannie_MiddleName", attribute2);
            }
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "17.400";
    }

    private void cleanupXml18100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.1)
        return;
      this.versionMigrationOccured = true;
      try
      {
        double num1 = 0.0;
        double num2 = 0.0;
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud");
        if (xmlElement1 != null)
          num1 = Utils.ParseDouble((object) xmlElement1.GetAttribute("TotalOfFinancedFees"));
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("TRANSACTION_DETAIL");
        if (xmlElement2 != null)
          num2 = Utils.ParseDouble((object) xmlElement2.GetAttribute("MIAndFundingFeeFinancedAmount"));
        if (num1 - num2 != 0.0)
          ((XmlElement) this.root.SelectSingleNode("EllieMae/USDA") ?? this.createPath("EllieMae/USDA")).SetAttribute("FinancedLoanClosingCosts", string.Concat((object) Utils.ArithmeticRounding(num1 - num2, 2)));
      }
      catch (Exception ex)
      {
      }
      this.EMXMLVersionID = "18.100";
    }

    private void cleanupXml18101()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.101)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"1008\"]");
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("BORROWER[1]/EllieMae/PAIR/FHA_VA_LOANS/VA_LOAN_SUMMARY");
        if (xmlElement2 != null && xmlElement1 == null)
        {
          string attribute = xmlElement2.GetAttribute("SpouseIncomeAmount");
          if (attribute != string.Empty)
            this.createLockField("1008", attribute);
        }
        if (Utils.ParseInt((object) this.GetFieldAt("3614")) <= 2016)
          this.lockFieldMigration("1191", "EllieMae/IRS_1098", "PointsPaid");
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.101";
    }

    private void cleanupXml18104()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.104)
        return;
      this.versionMigrationOccured = true;
      try
      {
        int count = this.root.SelectNodes("BORROWER").Count;
        ((XmlElement) this.root.SelectSingleNode("EllieMae"))?.SetAttribute("BorrowerPairCount", string.Concat((object) count));
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.104";
    }

    private void cleanupXml18105()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.105)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string str = "EllieMae/HMDA";
        string targetPath = "LOAN_PRODUCT_DATA/NMLS";
        this.copySingleAttribute(str, "PropertyType", targetPath, "NmlsPropertyType", false);
        string a = string.Empty;
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(str);
        if (xmlElement != null)
          a = xmlElement.GetAttribute("LienStatus");
        if (!string.Equals(a, "Not applicable"))
          this.copySingleAttribute(str, "LienStatus", targetPath, "NmlsLienStatus", false);
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.105";
    }

    private void cleanupXml18200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.2)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
        if (xmlElement != null)
        {
          string attribute = xmlElement.GetAttribute("_Type");
          if (attribute == "ConstructionToPermanent" || attribute == "ConstructionOnly")
          {
            this.lockFieldMigration("LE2.X2", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "DownPayment");
            this.lockFieldMigration("LE2.X3", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "FundsForBorrower");
            this.lockFieldMigration("CD3.X105", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "STDFinalDownPayment");
            this.lockFieldMigration("CD3.X107", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "STDFinalFundForBorrower");
            this.lockFieldMigration("LE1.X4", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "LoanPurpose");
          }
        }
        this.lockFieldMigration("VASUMM.X99", "EllieMae/FHA_VA_LOANS/VA_MANAGEMENT", "TotalProposedMonthlyPayment");
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/REGZ");
        if (xmlElement != null)
        {
          if (xmlElement.GetAttribute("BorrowerIntendToContinue") != "Y")
          {
            this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ClosingCostEstimateExpirationTime", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ClosingCostEstimateExpirationTimeUI", false);
            this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ClosingCostEstimateExpirationTimeZone", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ClosingCostEstimateExpirationTimeZoneUI", false);
            this.copySingleAttribute("EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ClosingCostEstimateExpirationDate", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ClosingCostEstimateExpirationDateUI", false);
          }
          this.copySingleAttribute("EllieMae/REGZ", "InterestOnlyIndicator", "EllieMae/HMDA", "HmdaInterestOnlyIndicator", false);
        }
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
        if (xmlElement != null)
        {
          string attribute = xmlElement.GetAttribute("_Type");
          if (!(attribute == "ConstructionToPermanent"))
          {
            if (!(attribute == "ConstructionOnly"))
              goto label_20;
          }
          this.lockFieldMigration("CONST.X59", "EllieMae/ConstructionManagement/LoanInfo", "AsCompletedAppraisedValue");
        }
      }
      catch
      {
      }
label_20:
      this.EMXMLVersionID = "18.200";
    }

    private void cleanupXml18202()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.202)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/REGZ");
        if (xmlElement1 != null)
        {
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement1.GetAttribute("FloorPercent")), "EllieMae/REGZ", "FloorPercentUI", false);
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement1.GetAttribute("_MaxLifeInterestCapPercent")), "EllieMae/REGZ", "_MaxLifeInterestCapPercentUI", false);
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement1.GetAttribute("APR")), "EllieMae/REGZ", "APRUI", false);
        }
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (xmlElement2 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement2.GetAttribute("InitialInterestRate")), "EllieMae", "InitialInterestRateUI", false);
        XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/RATE_ADJUSTMENT");
        if (xmlElement3 != null)
        {
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement3.GetAttribute("_Percent")), "LOAN_PRODUCT_DATA/RATE_ADJUSTMENT", "_PercentUI", false);
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement3.GetAttribute("_SubsequentCapPercent")), "LOAN_PRODUCT_DATA/RATE_ADJUSTMENT", "_SubsequentCapPercentUI", false);
        }
        XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/ARM");
        if (xmlElement4 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement4.GetAttribute("_IndexMarginPercent")), "LOAN_PRODUCT_DATA/ARM", "_IndexMarginPercentUI", false);
        XmlElement xmlElement5 = (XmlElement) this.root.SelectSingleNode("EllieMae/NewHud");
        if (xmlElement5 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement5.GetAttribute("_MaxLifeInterestCapPercent")), "EllieMae/NewHud", "_MaxLifeInterestCapPercentUI", false);
        XmlElement xmlElement6 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanEstimate2015");
        if (xmlElement6 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement6.GetAttribute("TotalInterestPercentage")), "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "TotalInterestPercentageUI", false);
        XmlElement xmlElement7 = (XmlElement) this.root.SelectSingleNode("MORTGAGE_TERMS");
        if (xmlElement7 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement7.GetAttribute("RequestedInterestRatePercent")), "MORTGAGE_TERMS", "RequestedInterestRatePercentUI", false);
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.202";
    }

    private void cleanupXml18300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.3)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (xmlElement1 != null)
        {
          string fieldAt1 = this.GetFieldAt("3632");
          string fieldAt2 = this.GetFieldAt("3633");
          string fieldAt3 = this.GetFieldAt("3634");
          string fieldAt4 = this.GetFieldAt("3635");
          string fieldAt5 = this.GetFieldAt("3636");
          string fieldAt6 = this.GetFieldAt("1262");
          if (fieldAt1 == string.Empty && fieldAt2 == string.Empty && fieldAt3 == string.Empty && fieldAt4 == string.Empty && fieldAt5 == string.Empty && fieldAt6 == string.Empty)
          {
            this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "Name", "EllieMae/FHA_VA_LOANS/Lender", "Name", false);
            this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "Address", "EllieMae/FHA_VA_LOANS/Lender", "Address", false);
            this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "City", "EllieMae/FHA_VA_LOANS/Lender", "City", false);
            this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "State", "EllieMae/FHA_VA_LOANS/Lender", "State", false);
            this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "PostalCode", "EllieMae/FHA_VA_LOANS/Lender", "PostalCode", false);
            this.copySingleAttribute("EllieMae/BROKER_LENDER/ContactInfo", "Phone", "EllieMae/LENDER_INVESTOR", "Phone", false);
          }
          this.copySingleAttribute("EllieMae/REGZ", "InterestOnlyIndicator", "EllieMae/HMDA", "HmdaInterestOnlyIndicator", false);
        }
        if (xmlElement1 != null)
        {
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement1.GetAttribute("FloorPercent")), "EllieMae/REGZ", "FloorPercentUI", false);
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement1.GetAttribute("_MaxLifeInterestCapPercent")), "EllieMae/REGZ", "_MaxLifeInterestCapPercentUI", false);
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement1.GetAttribute("APR")), "EllieMae/REGZ", "APRUI", false);
        }
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (xmlElement2 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement2.GetAttribute("InitialInterestRate")), "EllieMae", "InitialInterestRateUI", false);
        XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/RATE_ADJUSTMENT");
        if (xmlElement3 != null)
        {
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement3.GetAttribute("_Percent")), "LOAN_PRODUCT_DATA/RATE_ADJUSTMENT", "_PercentUI", false);
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement3.GetAttribute("_SubsequentCapPercent")), "LOAN_PRODUCT_DATA/RATE_ADJUSTMENT", "_SubsequentCapPercentUI", false);
        }
        XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/ARM");
        if (xmlElement4 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement4.GetAttribute("_IndexMarginPercent")), "LOAN_PRODUCT_DATA/ARM", "_IndexMarginPercentUI", false);
        XmlElement xmlElement5 = (XmlElement) this.root.SelectSingleNode("EllieMae/NewHud");
        if (xmlElement5 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement5.GetAttribute("_MaxLifeInterestCapPercent")), "EllieMae/NewHud", "_MaxLifeInterestCapPercentUI", false);
        XmlElement xmlElement6 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/LoanEstimate2015");
        if (xmlElement6 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement6.GetAttribute("TotalInterestPercentage")), "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "TotalInterestPercentageUI", false);
        XmlElement xmlElement7 = (XmlElement) this.root.SelectSingleNode("MORTGAGE_TERMS");
        if (xmlElement7 != null)
          this.copySingleAttribute(Utils.RemoveEndingZeros(xmlElement7.GetAttribute("RequestedInterestRatePercent")), "MORTGAGE_TERMS", "RequestedInterestRatePercentUI", false);
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud/FeeVariance");
        if (xmlElement != null)
        {
          if (Utils.ParseDouble((object) xmlElement.GetAttribute("ToleranceCureAppliedCureAmount")) >= 0.0)
          {
            this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/FeeVariance", "ToleranceCureAppliedCureAmount", "EllieMae/CLOSING_COST[1]/NewHud/FeeVariance", "CureAppliedToLenderCredit", false);
            this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud/FeeVariance", "ToleranceCureAppliedCureAmount", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "DiscloseLenderCredits", false);
          }
        }
      }
      catch
      {
      }
      try
      {
        for (int index = 1; index <= 3; ++index)
        {
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("ASSET[@_Type=\"Stock\"][" + (object) index + "]");
          string attribute = xmlElement.GetAttribute("_CashOrMarketValueAmount");
          if (attribute != "" && attribute.IndexOf(".") == -1)
            xmlElement.SetAttribute("_CashOrMarketValueAmount", attribute + ".00");
        }
      }
      catch
      {
      }
      bool flag = this.loanIsDisclosed();
      try
      {
        string a1 = string.Empty;
        string empty = string.Empty;
        string a2 = string.Empty;
        string a3 = string.Empty;
        XmlElement xmlElement8 = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
        if (xmlElement8 != null && xmlElement8.HasAttribute("_Type"))
          a1 = xmlElement8.GetAttribute("_Type");
        XmlElement xmlElement9 = (XmlElement) this.root.SelectSingleNode("EllieMae/REGZ");
        if (xmlElement9 != null && xmlElement9.HasAttribute("ConstructionLoanMethod"))
          a2 = xmlElement9.GetAttribute("ConstructionLoanMethod");
        XmlElement xmlElement10 = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]/NewHud");
        if (xmlElement10 != null && xmlElement10.HasAttribute("MonthlyPaymentRise"))
          a3 = xmlElement10.GetAttribute("MonthlyPaymentRise");
        if (flag)
        {
          if (!string.Equals(a2, "B"))
          {
            if (string.Equals(a1, "ConstructionOnly"))
            {
              if (string.Equals(a3, "N"))
                this.lockFieldMigration("NEWHUD.X8", "EllieMae/CLOSING_COST[1]/NewHud", "MonthlyPaymentRise");
              else if (string.Equals(a3, "Y"))
              {
                this.lockFieldMigration("NEWHUD.X8", "EllieMae/CLOSING_COST[1]/NewHud", "MonthlyPaymentRise");
                this.lockFieldMigration("LE1.X19", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsEveryYears", true);
                this.lockFieldMigration("LE1.X20", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsTermType", true);
                this.lockFieldMigration("LE1.X21", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsStartingInType", true);
                this.lockFieldMigration("LE1.X22", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAfterAdjustment", true);
                this.lockFieldMigration("LE1.X23", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPICanGoGoes", true);
                this.lockFieldMigration("LE1.X24", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "HighestMonthlyPI", true);
                this.lockFieldMigration("LE1.X25", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsInYear", true);
                this.lockFieldMigration("LE1.X26", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIInterestOnlyUntilYear", true);
                this.lockFieldMigration("LE1.X88", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustedInDateType", true);
                this.lockFieldMigration("LE1.X89", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIInterestOnlyDateType", true);
                this.lockFieldMigration("CD4.X23", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "InterestOnlyPayments", true);
                this.lockFieldMigration("CD4.X30", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "FirstChangeAmt", true);
                this.lockFieldMigration("CD4.X33", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "SubsequentChanges", true);
                this.lockFieldMigration("CD4.X34", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MaximumPaymentAmt", true);
                this.lockFieldMigration("CD4.X46", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "InterestOnlyPaymentMonths", true);
              }
            }
            else if (string.Equals(a1, "ConstructionToPermanent"))
            {
              if (string.Equals(a3, "Y"))
              {
                this.lockFieldMigration("LE1.X19", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsEveryYears", true);
                this.lockFieldMigration("LE1.X20", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsTermType", true);
                this.lockFieldMigration("LE1.X21", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustsStartingInType", true);
                this.lockFieldMigration("LE1.X22", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAfterAdjustment", true);
                this.lockFieldMigration("CD4.X23", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "InterestOnlyPayments", true);
                this.lockFieldMigration("CD4.X30", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "FirstChangeAmt", true);
                this.lockFieldMigration("CD4.X33", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "SubsequentChanges", true);
                this.lockFieldMigration("CD4.X34", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MaximumPaymentAmt", true);
                this.lockFieldMigration("CD4.X46", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "InterestOnlyPaymentMonths", true);
              }
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        string str1 = this.readNodeValue("EllieMae/REGZ/@ConstructionLoanMethod");
        string str2 = this.readNodeValue("LOAN_PURPOSE/@_Type");
        if (flag && str1 == "A" && str2.Contains("Construction"))
        {
          this.lockFieldMigration("CD4.X23", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "InterestOnlyPayments", true);
          this.lockFieldMigration("CD4.X30", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "FirstChangeAmt", true);
          this.lockFieldMigration("CD4.X33", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "SubsequentChanges", true);
          this.lockFieldMigration("CD4.X34", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MaximumPaymentAmt", true);
          this.lockFieldMigration("CD4.X46", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "InterestOnlyPaymentMonths", true);
        }
        string str3 = this.readNodeValue("EllieMae/ConstructionManagement/LoanInfo/@ConstructionPeriodIncludedInLoanTermFlag");
        if (str2 == "ConstructionToPermanent")
        {
          if (str3 == "Y")
          {
            BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
            for (int index = 0; index < borrowerPairs.Length; ++index)
            {
              this.SetBorrowerPair(borrowerPairs[index]);
              this.lockFieldMigration("1724", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"FirstMortgagePrincipalAndInterest\"]", "_PaymentAmount");
              this.lockFieldMigration("1725", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"OtherMortgageLoanPrincipalAndInterest\"]", "_PaymentAmount");
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement11 = (XmlElement) this.root.SelectSingleNode("EllieMae/COMMITMENT_TERMS");
        XmlElement xmlElement12 = (XmlElement) this.root.SelectSingleNode("PROPERTY");
        if (xmlElement11 != null)
        {
          string attribute1 = xmlElement11.GetAttribute("CommitmentExpired");
          string attribute2 = xmlElement12.GetAttribute("_State");
          if (!string.IsNullOrEmpty(attribute1))
          {
            if (flag)
            {
              if (!(attribute2 == "CA") && !(attribute2 == "CO") && !(attribute2 == "DC") && !(attribute2 == "FL") && !(attribute2 == "IL") && !(attribute2 == "MT") && !(attribute2 == "NJ"))
              {
                if (!(attribute2 == "NY"))
                  goto label_68;
              }
              this.copySingleAttribute("EllieMae/COMMITMENT_TERMS", "CommitmentExpired", "_CLOSING_DOCUMENTS/EllieMae/AdditionalDisclosure/CA", "CommitmentExpiredDate", false);
            }
          }
        }
      }
      catch
      {
      }
label_68:
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY");
        if (xmlElement != null)
        {
          double num1 = Utils.ParseDouble((object) xmlElement.GetAttribute("RecoupmentMonthlyDecreaseInPayment"));
          double num2 = Utils.ParseDouble((object) xmlElement.GetAttribute("RecoupmentTotalClosingCosts"));
          if (num1 > 0.0)
          {
            if (num2 > 0.0)
              xmlElement.SetAttribute("RecoupmentClosingCosts", string.Concat((object) Math.Ceiling(num2 / num1)));
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.300";
    }

    private bool loanIsDisclosed()
    {
      try
      {
        foreach (XmlNode selectNode1 in this.root.SelectNodes("EllieMae/LOG"))
        {
          foreach (XmlElement selectNode2 in selectNode1.SelectNodes("Record"))
          {
            string attribute = selectNode2.GetAttribute("Type");
            if (attribute == "DisclosureTracking2015" || attribute == "EnhancedDisclosureTracking2015")
              return true;
          }
        }
      }
      catch (Exception ex)
      {
      }
      return false;
    }

    private void cleanupXml18307()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.307)
        return;
      this.versionMigrationOccured = true;
      bool flag = this.loanIsDisclosed();
      try
      {
        if (flag)
        {
          this.lockFieldMigration("CD4.X8", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "EstimatedPropertyCosts");
          this.lockFieldMigration("CD4.X40", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "TotalDisbursed1YearConsummation");
          this.lockFieldMigration("CD4.X41", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "NonEscrowedPropertyCosts1YearConsummation");
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.307";
    }

    private void cleanupXml18400()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.4)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("GOVERNMENT_LOAN/FHA_LOAN");
        if (xmlElement != null)
        {
          if (xmlElement.GetAttribute("SectionOfActType") == "203")
            xmlElement.SetAttribute("SectionOfActType", "203H");
        }
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
        if (xmlElement != null)
        {
          if (xmlElement.GetAttribute("_Type") == "Cash-Out Refinance")
            this.lockFieldMigration("CASASRN.X79", "LOAN_PURPOSE/CONSTRUCTION_REFINANCE_DATA", "FRECashOutAmount");
        }
      }
      catch
      {
      }
      BorrowerPair[] borrowerPairs1 = this.GetBorrowerPairs();
      try
      {
        bool flag = false;
        if (borrowerPairs1.Length > 1)
        {
          string str = this.readNodeValue("BORROWER[@BorrowerID=\"" + borrowerPairs1[0].Borrower.Id + "\"]/EllieMae/PAIR/@PropertyUsageType");
          for (int index = 1; index < borrowerPairs1.Length; ++index)
          {
            if (this.readNodeValue("BORROWER[@BorrowerID=\"" + borrowerPairs1[index].Borrower.Id + "\"]/EllieMae/PAIR/@PropertyUsageType") != str)
            {
              flag = true;
              break;
            }
          }
          if (flag)
          {
            for (int index = 0; index < borrowerPairs1.Length; ++index)
              this.lockFieldMigration("1379", "BORROWER[@BorrowerID=\"" + borrowerPairs1[index].Borrower.Id + "\"]/EllieMae/PAIR", "PrimaryResidenceComortSet");
          }
        }
      }
      catch
      {
      }
      try
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
        if (xmlElement1 != null)
        {
          string attribute = xmlElement1.GetAttribute("_Type");
          if (attribute.IndexOf("Refinance") < 0)
          {
            if (attribute.IndexOf("Construction") < 0)
              goto label_31;
          }
          XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/CONSTRUCTION_REFINANCE_DATA");
          if (xmlElement2 != null)
          {
            if (xmlElement2.GetAttribute("PropertyExistingLienAmount") != "")
              this.lockFieldMigration("26", "EllieMae/CONSTRUCTION_REFINANCE_DATA", "PropertyExistingLienAmount");
          }
        }
      }
      catch
      {
      }
label_31:
      try
      {
        if (borrowerPairs1 != null)
        {
          if (borrowerPairs1.Length != 0)
          {
            this.SetBorrowerPair(borrowerPairs1[0]);
            int numberOfMortgages = this.GetNumberOfMortgages();
            int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
            if (exlcudingAlimonyJobExp > 0)
            {
              if (numberOfMortgages > 0)
              {
                Dictionary<string, List<int>> dictionary = (Dictionary<string, List<int>>) null;
                for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
                {
                  string fieldAt = this.GetFieldAt("FL" + index.ToString("00") + "25");
                  if (!(fieldAt == ""))
                  {
                    if (dictionary == null)
                      dictionary = new Dictionary<string, List<int>>();
                    if (!dictionary.ContainsKey(fieldAt))
                      dictionary.Add(fieldAt, new List<int>());
                    dictionary[fieldAt].Add(index);
                  }
                }
                if (dictionary != null)
                {
                  if (dictionary.Count > 0)
                  {
                    for (int index1 = 1; index1 <= numberOfMortgages; ++index1)
                    {
                      string fieldAt1 = this.GetFieldAt("FM" + index1.ToString("00") + "43");
                      if (!(fieldAt1 == "") && dictionary.ContainsKey(fieldAt1))
                      {
                        string fieldAt2 = this.GetFieldAt("FM" + index1.ToString("00") + "28");
                        if (!(fieldAt2 != "Y"))
                        {
                          List<int> intList = dictionary[fieldAt1];
                          if (intList != null && intList.Count > 0)
                          {
                            for (int index2 = 0; index2 < intList.Count; ++index2)
                              this.SetFieldAt("FL" + intList[index2].ToString("00") + "27", fieldAt2);
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
      }
      string str1 = this.readNodeValue("EllieMae/@LoanNumber");
      try
      {
        this.lockFieldMigration("428", "EllieMae", "SecondSubordinateAmount", str1 != "");
      }
      catch
      {
      }
      try
      {
        this.lockFieldMigration("427", "EllieMae", "FirstSubordinateAmount", str1 != "");
      }
      catch
      {
      }
      try
      {
        this.lockFieldMigration("CASASRN.X168", "EllieMae/FREDDIE_MAC", "HELOCCreditLimit", str1 != "");
      }
      catch
      {
      }
      try
      {
        this.lockFieldMigration("1732", "EllieMae/TSUM", "UnpaidBalance", str1 != "");
        this.lockFieldMigration("CASASRN.X167", "EllieMae/FREDDIE_MAC", "HELOCActualBalance", str1 != "");
      }
      catch
      {
      }
      try
      {
        string str2 = this.readNodeValue("LOAN_PRODUCT_DATA/LOAN_FEATURES/@LienPriorityType");
        if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") == "HELOC")
        {
          this.lockFieldMigration("5", "EllieMae", "MonthlyPayment");
          if (borrowerPairs1 == null)
            borrowerPairs1 = this.GetBorrowerPairs();
          for (int index = 0; index < borrowerPairs1.Length; ++index)
          {
            this.SetBorrowerPair(borrowerPairs1[index]);
            switch (str2)
            {
              case "FirstLien":
                this.lockFieldMigration("1724", "BORROWER[@BorrowerID=\"" + borrowerPairs1[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"FirstMortgagePrincipalAndInterest\"]", "_PaymentAmount");
                break;
              case "SecondLien":
                this.lockFieldMigration("1725", "BORROWER[@BorrowerID=\"" + borrowerPairs1[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"OtherMortgageLoanPrincipalAndInterest\"]", "_PaymentAmount");
                break;
            }
          }
        }
        switch (str2)
        {
          case "FirstLien":
            this.copySingleAttribute("1", "LOAN_PRODUCT_DATA/HELOC", "_HELOCLienPosition", true);
            break;
          case "SecondLien":
            this.copySingleAttribute("2", "LOAN_PRODUCT_DATA/HELOC", "_HELOCLienPosition", true);
            break;
        }
      }
      catch
      {
      }
      try
      {
        this.lockFieldMigration("976", "EllieMae", "CombinedLTV");
        this.lockFieldMigration("MORNET.X76", "EllieMae/FANNIE_MAE", "CLTV");
        this.lockFieldMigration("ULDD.FNM.MORNET.X76", "EllieMae/ULDD", "FannieCLTV");
        this.lockFieldMigration("1540", "EllieMae", "HCLTV_HTLTV");
        this.lockFieldMigration("MORNET.X77", "EllieMae/FANNIE_MAE", "HCLTV");
        this.lockFieldMigration("ULDD.FNM.MORNET.X77", "EllieMae/ULDD", "FannieHCLTV");
      }
      catch
      {
      }
      try
      {
        BorrowerPair[] borrowerPairs2 = this.GetBorrowerPairs();
        this.SetBorrowerPair(borrowerPairs2[0]);
        this.moveSingleAttribute("EllieMae/USDA", "AnnualIncome", "BORROWER[@BorrowerID=\"" + borrowerPairs2[0].Borrower.Id + "\"]/EllieMae/USDA", "AnnualIncome");
        this.moveSingleAttribute("EllieMae/USDA", "AdjustedAnnualIncome", "BORROWER[@BorrowerID=\"" + borrowerPairs2[0].Borrower.Id + "\"]/EllieMae/USDA", "AdjustedAnnualIncome");
        this.moveSingleAttribute("EllieMae/USDA/AnnualIncome", "BorrowerBaseIncome", "BORROWER[@BorrowerID=\"" + borrowerPairs2[0].Borrower.Id + "\"]/EllieMae/USDA", "BaseIncome");
        this.moveSingleAttribute("EllieMae/USDA/AnnualIncome", "CoborrowerBaseIncome", "BORROWER[@BorrowerID=\"" + borrowerPairs2[0].CoBorrower.Id + "\"]/EllieMae/USDA", "BaseIncome");
        this.moveSingleAttribute("EllieMae/USDA/AnnualIncome", "AdditionalIncomeFromPrimaryEmployment", "BORROWER[@BorrowerID=\"" + borrowerPairs2[0].Borrower.Id + "\"]/EllieMae/USDA", "AdditionalIncomeFromPrimaryEmployment");
        this.moveSingleAttribute("EllieMae/USDA/AnnualIncome", "AssetIncome", "BORROWER[@BorrowerID=\"" + borrowerPairs2[0].Borrower.Id + "\"]/EllieMae/USDA", "AssetIncome");
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.400";
    }

    private void cleanupXml18401()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.401)
        return;
      this.versionMigrationOccured = true;
      bool flag = this.loanIsDisclosed();
      try
      {
        string str1 = this.readNodeValue("EllieMae/REGZ/@ConstructionLoanMethod");
        string str2 = this.readNodeValue("LOAN_PURPOSE/@_Type");
        if (flag)
        {
          if (!(str2 == "ConstructionOnly"))
          {
            if (!(str2 == "ConstructionToPermanent"))
              goto label_10;
          }
          if (!(str1 == "A"))
          {
            if (!(str1 == "B"))
              goto label_10;
          }
          this.lockFieldMigration("4088", "EllieMae", "EstimatedConstructionInterest");
        }
      }
      catch
      {
      }
label_10:
      this.EMXMLVersionID = "18.401";
    }

    private void cleanupXml18403()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.403)
        return;
      this.versionMigrationOccured = true;
      string empty = string.Empty;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/HMDA");
        if (xmlElement != null)
        {
          string attribute = xmlElement.GetAttribute("PropertyValue");
          if (attribute != string.Empty)
            xmlElement.SetAttribute("PropertyValue", Utils.ArithmeticRounding(Utils.ToDouble(attribute), 0).ToString());
        }
      }
      catch
      {
      }
      try
      {
        if (this.readNodeValue("LOAN_PURPOSE/@_Type") == "ConstructionToPermanent")
        {
          if (this.readNodeValue("EllieMae/HUD1ES/@EscrowFirstPaymentDate") != this.readNodeValue("LOAN_PRODUCT_DATA/LOAN_FEATURES/@ScheduledFirstPaymentDate"))
          {
            if (Utils.ParseInt((object) this.readNodeValue("EllieMae/REGZ/CONSTRUCTION_LOANS/@ConstruccionPeriodMonths"), 0) >= 12)
              this.lockFieldMigration("CD4.X10", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "MonthlyEscrowPayment");
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.403";
    }

    private void cleanupXml18404()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 18.404)
        return;
      this.versionMigrationOccured = true;
      try
      {
        this.copySingleAttribute("LOAN_PRODUCT_DATA/LOAN_FEATURES", "BalloonIndicator", "EllieMae/HMDA", "BalloonIndicator", false);
        this.copySingleAttribute("EllieMae/CLOSING_COST[1]/NewHud", "LoanBalanceRise", "EllieMae/HMDA", "LoanBalanceRiseIndicator", false);
        this.copySingleAttribute("EllieMae/HMDA", "HmdaInterestOnlyIndicator", "EllieMae/HMDA", "Hmda2InterestOnlyIndicator", false);
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs.Length != 0)
        {
          for (int index = 0; index < borrowerPairs.Length; ++index)
          {
            string str1 = "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/EllieMae";
            this.copySingleAttribute(str1, "HmdaCreditScoreForDecisionMaking", str1, "Hmda2CreditScoreForDecisionMaking", false);
            this.copySingleAttribute(str1, "HmdaCreditScoringModel", str1, "Hmda2CreditScoringModel", false);
            string str2 = "BORROWER[@BorrowerID=\"" + borrowerPairs[0].CoBorrower.Id + "\"]/EllieMae";
            this.copySingleAttribute(str2, "HmdaCreditScoreForDecisionMaking", str2, "Hmda2CreditScoreForDecisionMaking", false);
            this.copySingleAttribute(str2, "HmdaCreditScoringModel", str2, "Hmda2CreditScoringModel", false);
          }
        }
      }
      catch
      {
      }
      bool flag = false;
      try
      {
        flag = this.loanIsDisclosed();
        if (((!(this.readNodeValue("LOAN_PURPOSE/@_Type") == "ConstructionOnly") ? 0 : (this.readNodeValue("EllieMae/REGZ/@ConstructionLoanMethod") == "A" ? 1 : 0)) & (flag ? 1 : 0)) != 0)
        {
          this.lockFieldMigration("LE1.X24", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "HighestMonthlyPI");
          this.lockFieldMigration("LE1.X88", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "MonthlyPIAdjustedInDateType");
          this.lockFieldMigration("CD4.X34", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "MaximumPaymentAmt");
        }
      }
      catch
      {
      }
      try
      {
        if (flag)
        {
          if (Utils.ParseDouble((object) this.readNodeValue("EllieMae/CLOSING_COST[1]/PrepaidInterest/@BorPaidAmount")) < 0.0)
            this.lockFieldMigration("LE3.X17", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "In5YearsTotalYouWillHavePaid");
        }
      }
      catch
      {
      }
      try
      {
        if (flag)
        {
          if (Utils.ParseDouble((object) this.readNodeValue("EllieMae/CLOSING_COST[1]/NewHud/Section900/Details/@Line901BorrowerAmountPaid2015")) < 0.0)
            this.lockFieldMigration("CD5.X1", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "TotalPayments");
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "18.404";
    }

    private void cleanupXml19100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.1)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PURPOSE");
        if (xmlElement != null)
        {
          string attribute = xmlElement.GetAttribute("_Type");
          if (attribute == "Purchase" && this.loanIsDisclosed())
          {
            Decimal num1 = Utils.ParseDecimal((object) this.GetFieldAt("2"), 0M);
            Decimal num2 = Utils.ParseDecimal((object) this.GetFieldAt("136"), 0M);
            bool flag = false;
            if (num1 > num2)
              flag = true;
            else if (this.LoanHasLiabilityToBePaidoff())
              flag = true;
            if (flag)
            {
              this.lockFieldMigration("LE2.X2", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "DownPayment");
              this.lockFieldMigration("LE2.X3", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "FundsForBorrower");
              this.lockFieldMigration("CD3.X105", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "STDFinalDownPayment");
              this.lockFieldMigration("CD3.X107", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "STDFinalFundForBorrower");
              this.lockFieldMigration("LE2.X29", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "ThirdPartyPaymentsNotOtherwiseDisclosed");
            }
          }
          if (attribute == "ConstructionToPermanent" && this.readNodeValue("EllieMae/HUD1ES/@EscrowFirstPaymentDate") != this.readNodeValue("LOAN_PRODUCT_DATA/LOAN_FEATURES/@ScheduledFirstPaymentDate") && Utils.ParseInt((object) this.readNodeValue("EllieMae/REGZ/CONSTRUCTION_LOANS/@ConstruccionPeriodMonths"), 0) >= 12)
            this.lockFieldMigration("CD4.X10", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "MonthlyEscrowPayment");
        }
        this.copySingleAttribute("LOAN_PRODUCT_DATA/HELOC", "_InitialAdvanceAmount", "EllieMae/RateLock/Request/LoanInfo", "InitialAdvanceAmount", false);
        this.copySingleAttribute("EllieMae/FREDDIE_MAC", "HELOCCreditLimit", "EllieMae/RateLock/Request/LoanInfo", "HELOCCreditLimit", false);
        this.copySingleAttribute("EllieMae", "TeaserRate", "EllieMae/RateLock/Request/LoanInfo", "TeaserRate", false);
        this.copySingleAttribute("EllieMae/REGZ/ARMDISCLOSURE", "DisclosureType", "EllieMae/RateLock/Request/LoanInfo", "DisclosureType", false);
        this.copySingleAttribute("LOAN_PRODUCT_DATA/ARM", "_IndexCurrentValuePercent", "EllieMae/RateLock/Request/LoanInfo", "IndexCurrentValuePercent", false);
        this.lockFieldMigration("VASUMM.X99", "EllieMae/FHA_VA_LOANS/VA_MANAGEMENT", "TotalProposedMonthlyPayment");
      }
      catch
      {
      }
      try
      {
        string str = this.readNodeValue("BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/EllieMae/PAIR/@PropertyUsageType");
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"3335\"]");
        if (str == "SecondHome")
        {
          if (xmlElement == null)
            ((XmlElement) this.root.SelectSingleNode("EllieMae")).SetAttribute("Occupancy", "OwnerOccupied");
        }
      }
      catch
      {
      }
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          if (this.readNodeValue("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/_ALIAS/@_UnparsedName") != string.Empty)
            this.lockFieldMigration("1869", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/_ALIAS", "_UnparsedName");
          if (this.readNodeValue("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/_ALIAS/@_UnparsedName") != string.Empty)
            this.lockFieldMigration("1874", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/_ALIAS", "_UnparsedName");
        }
      }
      catch
      {
      }
      try
      {
        string str1 = this.readNodeValue("LOAN_PURPOSE/@_Type");
        string str2 = this.readNodeValue("MORTGAGE_TERMS/@LoanAmortizationType");
        if (!string.IsNullOrEmpty(str1))
        {
          if (!string.IsNullOrEmpty(str2))
          {
            if (str1 == "ConstructionToPermanent")
            {
              if (this.loanIsDisclosed())
              {
                if (str2 == "AdjustableRate")
                  this.lockFieldMigration("LE1.X5", "EllieMae/CLOSING_COST[1]/LoanEstimate2015", "LoanProduct");
              }
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        if (Mapping.UseNewCompliance(18.3M, this.readNodeValue("EllieMae/ComplianceVersion/@_SavedLogVersion")))
        {
          if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") != "FarmersHomeAdministration")
          {
            if (!string.IsNullOrEmpty(this.readNodeValue("EllieMae/CLOSING_COST[1]/MortgageInsurancePremium/@BorPaidAmount")))
              this.lockFieldMigration("CD5.X1", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "TotalPayments");
          }
        }
      }
      catch
      {
      }
      try
      {
        string str = this.readNodeValue("PROPERTY/@_State");
        if (!string.IsNullOrEmpty(str))
        {
          if (this.loanIsDisclosed())
          {
            if (!(str == "KY") && !(str == "MD") && !(str == "VT") && !(str == "VA"))
            {
              if (!(str == "WI"))
                goto label_49;
            }
            this.copySingleAttribute("EllieMae/COMMITMENT_TERMS", "CommitmentExpired", "_CLOSING_DOCUMENTS/EllieMae/AdditionalDisclosure/CA", "CommitmentExpiredDate", false);
          }
        }
      }
      catch
      {
      }
label_49:
      try
      {
        if (((XmlElement) this.root.SelectSingleNode("EllieMae")).GetAttribute("Print2003Application") == "2020")
        {
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"1612\"]") == null)
            this.lockFieldMigration("1612", "INTERVIEWER_INFORMATION", "InterviewersName");
          string attribute = ((XmlElement) this.root.SelectSingleNode("EllieMae/BROKER_LENDER/ContactInfo")).GetAttribute("Address");
          if ((attribute ?? "") != string.Empty)
          {
            if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"319\"]") == null)
            {
              ((XmlElement) this.root.SelectSingleNode("URLA2020")).SetAttribute("OriginatorAddressLineText", attribute);
              this.lockFieldMigration("319", "EllieMae/BROKER_LENDER/ContactInfo", "Address");
            }
          }
        }
      }
      catch (Exception ex)
      {
        ex.ToString();
      }
      this.EMXMLVersionID = "19.100";
    }

    private void cleanupXml19200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.2)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"QM.X376\"]") == null)
        {
          BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
          if (borrowerPairs != null)
          {
            if (borrowerPairs.Length != 0)
            {
              if (this.GetFieldAtXpath("BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/" + "EllieMae/PAIR/@PropertyUsageType") == "PrimaryResidence")
              {
                if (this.readNodeValue("MORTGAGE_TERMS/@LoanAmortizationType") == "Fixed")
                {
                  string s = this.readNodeValue("EllieMae/REGZ/@InterestOnlyMonths");
                  if (!string.IsNullOrEmpty(s))
                  {
                    if (int.Parse(s) > 0)
                      this.lockFieldMigration("QM.X376", "EllieMae/ATR_QM/Qualification", "InitialRateTotalDebtRatio");
                  }
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      string str1 = this.readNodeValue("MORTGAGE_TERMS/@MortgageType");
      try
      {
        string str2 = this.readNodeValue("LOAN_PRODUCT_DATA/HELOC/@_HelocInitialPaymentBasis");
        if (str1 == "HELOC")
        {
          if (!string.IsNullOrEmpty(str2))
          {
            if (str2 != "Rate")
              this.copySingleAttribute("LOAN_PRODUCT_DATA/HELOC", "_HelocInitialPaymentBasis", "LOAN_PRODUCT_DATA/HELOC", "_HelocInitialPaymentBasisType", false);
          }
        }
      }
      catch
      {
      }
      try
      {
        string str3 = this.readNodeValue("LOAN_PRODUCT_DATA/HELOC/@_HelocPaymentBasis");
        if (str1 == "HELOC")
        {
          if (!string.IsNullOrEmpty(str3))
          {
            if (str3 != "Rate")
              this.copySingleAttribute("LOAN_PRODUCT_DATA/HELOC", "_HelocPaymentBasis", "LOAN_PRODUCT_DATA/HELOC", "_HelocPaymentBasisType", false);
          }
        }
      }
      catch
      {
      }
      bool flag = false;
      try
      {
        flag = this.loanIsDisclosed();
      }
      catch
      {
      }
      try
      {
        if (this.readNodeValue("EllieMae/ComplianceVersion/@_NewBuydownEnabled") == "Y")
        {
          string str4 = this.readNodeValue("EllieMae/FREDDIE_MAC/@BuydownContributor");
          if (!flag && str4 == "Seller")
          {
            this.copySingleAttribute("LOAN_PRODUCT_DATA/BUYDOWN[1]/EllieMae", "_TotalSubsidyAmount", "EllieMae/ATR_QM/Qualification", "BuydownSellerPaidSec32PointsFees", false);
            double num1 = Utils.ParseDouble((object) this.readNodeValue("EllieMae/SECTION32/@TotalPointsAndFees").Replace(",", ""));
            double num2 = Utils.ParseDouble((object) this.readNodeValue("EllieMae/ATR_QM/Qualification/@BuydownSellerPaidSec32PointsFees"));
            if (num2 != 0.0)
              this.copySingleAttribute((num1 + num2).ToString("N2").Replace(",", ""), "EllieMae/SECTION32", "TotalPointsAndFees", true);
          }
          if (flag)
            this.copySingleAttribute("Y", "EllieMae/ComplianceVersion", "_UseOldBuydownUIandLogic", true);
          if (!flag)
          {
            if (str4 != "Borrower")
            {
              for (int index = 1; index <= 6; ++index)
              {
                XmlElement xmlElement = (XmlElement) null;
                if (this.readNodeValue("LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]/@_IncreaseRatePercent") != "")
                {
                  this.copySingleAttribute("LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]", "_IncreaseRatePercent", "LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]", "_NonBorrowerIncreaseRatePercent", false);
                  xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]");
                  if (xmlElement != null && xmlElement.HasAttribute("_IncreaseRatePercent"))
                    xmlElement.RemoveAttribute("_IncreaseRatePercent");
                }
                if (this.readNodeValue("LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]/@_ChangeFrequencyMonths") != "")
                {
                  this.copySingleAttribute("LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]", "_ChangeFrequencyMonths", "LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]", "_NonBorrowerChangeFrequencyMonths", false);
                  if (xmlElement == null)
                    xmlElement = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/BUYDOWN[" + (object) index + "]");
                  if (xmlElement != null && xmlElement.HasAttribute("_ChangeFrequencyMonths"))
                    xmlElement.RemoveAttribute("_ChangeFrequencyMonths");
                }
              }
              if (this.lockRoot == null)
                this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
              if (this.lockRoot != null)
              {
                for (int index = 1269; index <= 1274; ++index)
                {
                  if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) index + "\"]") != null)
                    this.RemoveLockAt(string.Concat((object) index));
                  if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) (index + 344) + "\"]") != null)
                    this.RemoveLockAt(string.Concat((object) (index + 344)));
                }
              }
            }
          }
        }
      }
      catch
      {
      }
      try
      {
        if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") == "HELOC")
        {
          if (this.readNodeValue("EllieMae/@MaturityDate") != "")
          {
            if (Utils.ParseDate((object) this.readNodeValue("_CLOSING_DOCUMENTS/LOAN_DETAILS/@DocumentPreparationDate")) != DateTime.MinValue)
              this.lockFieldMigration("78", "EllieMae", "MaturityDate");
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "19.200";
    }

    private void cleanupXml19300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.3)
        return;
      this.versionMigrationOccured = true;
      try
      {
        this.SetFieldAt("4568", "Y");
        this.SetFieldAt("4559", "Y");
      }
      catch
      {
      }
      try
      {
        string str1 = this.readNodeValue("EllieMae/HMDA/@StateCode");
        string str2 = this.readNodeValue("EllieMae/HMDA/@CountyCode");
        string str3 = this.readNodeValue("EllieMae/HMDA/@CensusTrack");
        if (str1 == "NA" || str2 == "NA")
          this.copySingleAttribute("NA", "EllieMae/HMDA", "HMDACountyCode", true);
        if (!(str1 == "NA") && !(str2 == "NA"))
        {
          if (!(str3 == "NA"))
            goto label_11;
        }
        this.copySingleAttribute("NA", "EllieMae/HMDA", "HMDACensusTrack", true);
      }
      catch
      {
      }
label_11:
      try
      {
        string str = this.readNodeValue("EllieMae/HMDA/@ActionTaken");
        string sourceValue = this.readNodeValue("MORTGAGE_TERMS/@BaseLoanAmount");
        if (!(str == "Application approved but not accepted"))
        {
          if (!(str == "Preapproval request approved but not accepted"))
            goto label_16;
        }
        this.copySingleAttribute(sourceValue, "EllieMae/HMDA", "LoanAmount", true);
      }
      catch
      {
      }
label_16:
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        string targetPath1 = "BORROWER[@BorrowerID=\"" + borrowerPairs[0].Borrower.Id + "\"]/URLA2020";
        this.copySingleAttribute("EllieMae/HomeCounselingProviders/SelectedHomeCounselingProvider", "_AgencyName", targetPath1, "HomeCounselingAgencyName", false);
        this.copySingleAttribute("EllieMae/HomeCounselingProviders/SelectedHomeCounselingProvider", "_HomeCounselingCompletionDate", targetPath1, "HomeCounselingCompletionDate", false);
        string sourceValue1 = this.readNodeValue("URLA2020/@OwnershipConfirmationIndicator");
        if (sourceValue1 != "")
          this.copySingleAttribute(sourceValue1, targetPath1, "OwnershipConfirmationIndicator", false);
        string sourceValue2 = this.readNodeValue("URLA2020/@OwnershipFormatType");
        if (sourceValue2 != "")
          this.copySingleAttribute(sourceValue2, targetPath1, "OwnershipFormatType", false);
        string sourceValue3 = this.readNodeValue("URLA2020/@OwnershipPartyRoleIdentifier");
        if (sourceValue3 != "")
          this.copySingleAttribute(sourceValue3, targetPath1, "OwnershipPartyRoleIdentifier", false);
        string targetPath2 = "BORROWER[@BorrowerID=\"" + borrowerPairs[0].CoBorrower.Id + "\"]/URLA2020";
        string sourceValue4 = this.readNodeValue("URLA2020/@CounselingConfirmationIndicator");
        if (sourceValue4 != "")
          this.copySingleAttribute(sourceValue4, targetPath2, "OwnershipConfirmationIndicator", false);
        string sourceValue5 = this.readNodeValue("URLA2020/@CounselingFormatType");
        if (sourceValue5 != "")
          this.copySingleAttribute(sourceValue5, targetPath2, "OwnershipFormatType", false);
        string sourceValue6 = this.readNodeValue("URLA2020/@PartyRoleIdentifier");
        if (sourceValue6 != "")
          this.copySingleAttribute(sourceValue6, targetPath2, "OwnershipPartyRoleIdentifier", false);
        string sourceValue7 = this.readNodeValue("URLA2020/@HousingCounselingAgencyFullName");
        if (sourceValue7 != "")
          this.copySingleAttribute(sourceValue7, targetPath2, "HomeCounselingAgencyName", false);
        string sourceValue8 = this.readNodeValue("URLA2020/@CounselingCompletedDate");
        if (sourceValue8 != "")
          this.copySingleAttribute(sourceValue8, targetPath2, "HomeCounselingCompletionDate", false);
        string sourceValue9 = this.readNodeValue("URLA2020/@CoBorrAttendedSameCounselingIndicator");
        if (sourceValue9 != "")
          this.copySingleAttribute(sourceValue9, targetPath2, "CoBorrAttendedSameCounselingIndicator", false);
      }
      catch
      {
      }
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs != null)
        {
          for (int index = 0; index < borrowerPairs.Length; ++index)
          {
            this.SetBorrowerPair(borrowerPairs[index]);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor2", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "DataVerify", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor2", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor2", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor3", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor2", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor3", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor3", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor4", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor3", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor4", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor4", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor5", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor4", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor5", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor5", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor6", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor5", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor6", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor6", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor7", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor6", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor7", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor7", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor29", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor39", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor29", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor39", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor30", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor40", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor30", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor40", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor28", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor38", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor28", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor38", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor27", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor37", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor27", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor37", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor25", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor30", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor26", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor30", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor23", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor28", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor24", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor29", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor21", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor26", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor22", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor27", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor19", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor24", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor20", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor25", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor17", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor22", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor18", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor23", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor15", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor20", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor16", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor21", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor13", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor18", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor14", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor19", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor12", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor16", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor12", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor17", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor10", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor13", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor11", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor15", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor11", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor14", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor9", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor11", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor10", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor12", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor9", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor10", true);
            this.moveSingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae", "Vendor8", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae", "Vendor9", true);
          }
          this.SetBorrowerPair(borrowerPairs[0]);
        }
      }
      catch
      {
      }
      try
      {
        this.GetBorrowerPairs();
        string str = this.readNodeValue("LOAN_PRODUCT_DATA/BUYDOWN[1]/@_IncreaseRatePercent");
        if (this.readNodeValue("LOAN_PRODUCT_DATA/BUYDOWN[1]/@_NonBorrowerIncreaseRatePercent") == "" && str == "")
          this.SetFieldAt("ULDD.X181", "N");
        else
          this.SetFieldAt("ULDD.X181", "Y");
      }
      catch
      {
      }
      try
      {
        if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") == "HELOC")
        {
          if (this.readNodeValue("EllieMae/@MaturityDate") != "")
          {
            if (Utils.ParseDate((object) this.readNodeValue("_CLOSING_DOCUMENTS/LOAN_DETAILS/@DocumentPreparationDate")) != DateTime.MinValue)
              this.lockFieldMigration("78", "EllieMae", "MaturityDate");
          }
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "19.300";
    }

    private void cleanupXml19400()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.4)
        return;
      this.versionMigrationOccured = true;
      if (!this.loanIsDisclosed())
      {
        string str1 = this.readNodeValue("LOAN_PRODUCT_DATA/BUYDOWN[1]/@_IncreaseRatePercent");
        string str2 = this.readNodeValue("LOAN_PRODUCT_DATA/BUYDOWN[1]/@_NonBorrowerIncreaseRatePercent");
        if (!string.IsNullOrWhiteSpace(str1) || !string.IsNullOrWhiteSpace(str2))
        {
          this.SetFieldAt("425", "Y");
          this.SetFieldAt("ULDD.X181", "Y");
        }
      }
      try
      {
        switch (this.readNodeValue("LOAN_PRODUCT_DATA/LOAN_FEATURES/@LoanRepaymentType"))
        {
          case "NoNegativeAmortization":
            this.copySingleAttribute("none", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "NegativeAmortization", true);
            break;
          case "":
          case "ScheduledAmortization":
          case "InterestOnly":
            this.copySingleAttribute("", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "NegativeAmortization", true);
            break;
          case "PossibleNegativeAmortization":
            this.copySingleAttribute("potential", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "NegativeAmortization", true);
            this.SetFieldAt("URLA.X239", "Y");
            break;
          case "ScheduledNegativeAmortization":
            this.copySingleAttribute("scheduled", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "NegativeAmortization", true);
            this.SetFieldAt("URLA.X239", "Y");
            break;
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "19.400";
    }

    private void cleanupXml19403()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.403)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.readNodeValue("EllieMae/ULDD/@WareHouseLenderID") == "")
          this.moveSingleAttribute("EllieMae/ULDD", "WareHouseLenderId", "EllieMae/ULDD", "WareHouseLenderID");
      }
      catch (Exception ex)
      {
      }
      this.EMXMLVersionID = "19.403";
    }

    private void cleanupXml19407()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.407)
        return;
      this.versionMigrationOccured = true;
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs != null)
        {
          for (int index = 0; index < borrowerPairs.Length; ++index)
          {
            this.SetBorrowerPair(borrowerPairs[index]);
            this.PopulateLatestSubmissionAusTracking();
            XmlElement oldChild = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking/LatestSubmission");
            oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
          }
          this.SetBorrowerPair(borrowerPairs[0]);
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "19.407";
    }

    private void cleanupXml20100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.1)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (xmlElement != null)
        {
          if (xmlElement.GetAttribute("Print2003Application") == "2020" | xmlElement.GetAttribute("LINKGUID") != "")
            this.lockFieldMigration("140", "TRANSACTION_DETAIL", "SubordinateLienAmount");
        }
      }
      catch (Exception ex)
      {
      }
      this.EMXMLVersionID = "20.100";
    }

    private void cleanupXml19305()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 19.305)
        return;
      this.versionMigrationOccured = true;
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs != null)
        {
          for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
          {
            this.SetBorrowerPair(borrowerPairs[index1]);
            for (int index2 = 1; index2 <= this.GetNumberOfOtherLiabilities(); ++index2)
              this.copySingleAttribute("BORROWER[@BorrowerID=\"" + borrowerPairs[index1].Borrower.Id + "\"]/OtherLiabilities/OTHER_LIABILITY[" + index2.ToString() + "]", "HolderState", "BORROWER[@BorrowerID=\"" + borrowerPairs[index1].Borrower.Id + "\"]/URLA2020/OtherLiabilities/OTHER_LIABILITY[" + index2.ToString() + "]", "HolderState", true);
          }
          this.SetBorrowerPair(borrowerPairs[0]);
        }
      }
      catch
      {
      }
      this.EMXMLVersionID = "19.305";
    }

    private void cleanupXml20103()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.103)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") != "HELOC")
        {
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"5\"]") != null)
          {
            string str1 = this.readNodeValue("EllieMae/@MonthlyPayment");
            string str2 = "";
            XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae");
            if (xmlElement != null)
              str2 = xmlElement.GetAttribute("MonthlyPIPaymentAmountForLE1andCD1");
            if (Utils.ParseDecimal((object) str2) != Utils.ParseDecimal((object) str1))
              this.copySingleAttribute("EllieMae", "MonthlyPayment", "EllieMae", "MonthlyPIPaymentAmountForLE1andCD1", true);
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.EMXMLVersionID = "20.103";
    }

    private void cleanupXml20105()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.105)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (string.IsNullOrEmpty(this.GetFieldAt("CPA.RetainUserInputs")))
          this.SetFieldAt("CPA.RetainUserInputs", "Y");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20102 data migration for Field CPA.RetainUserInputs: " + ex.Message);
      }
      try
      {
        this.moveSingleAttribute("_CLOSING_DOCUMENTS/EllieMae/AdditionalDisclosure/PA", "AppraisalFeeamount", "_CLOSING_DOCUMENTS/EllieMae/AdditionalDisclosure/PA", "AppraisalFeeAmount", true);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20102 data migration for Field DISCLOSURE.X1188: " + ex.Message);
      }
      this.EMXMLVersionID = "20.105";
    }

    private void cleanupXml201012()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.112)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("LOAN_PRODUCT_DATA/HELOC");
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("EllieMae/FREDDIE_MAC");
        if (xmlElement2 != null)
        {
          if (xmlElement1 != null)
          {
            if (xmlElement1.GetAttribute("_HelocNewFinancingNotLinkedDrawAmount") == "0.00")
            {
              if (string.IsNullOrEmpty(xmlElement2.GetAttribute("HELOCActualBalance")))
                xmlElement2.SetAttribute("HELOCActualBalance", "0.00");
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml201012 data migration for Field : CASASRN.X167" + ex.Message);
      }
      try
      {
        this.lockFieldMigration("ULDD.X36", "EllieMae/ULDD", "InvestorFeatureIdentifier");
        this.lockFieldMigration("ULDD.X179", "EllieMae/ULDD", "FreddieInvestorFeatureIdentifier");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml201012 data migration for Field : ULDD.X36/ULDD.X179" + ex.Message);
      }
      this.EMXMLVersionID = "20.112";
    }

    private void cleanupXml20204()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.204)
        return;
      this.versionMigrationOccured = true;
      try
      {
        Mapping.RemoveInvalidCharFromAllRoles(this.root.OwnerDocument);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20202 data migration for Field : EDS.X6" + ex.Message);
      }
      try
      {
        this.copySingleAttribute("EllieMae/CONSTRUCTION_REFINANCE_DATA", "PropertyExistingLienAmount", "EllieMae/FHA_VA_LOANS/VA_MANAGEMENT", "RefinancePropertyExistingLienAmount", false);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20202 data migration for Field : VASUMM.X149" + ex.Message);
      }
      try
      {
        foreach (BorrowerPair borrowerPair in this.GetBorrowerPairs())
        {
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/EllieMae/PAIR/TAX_4506");
          if (xmlElement != null && !xmlElement.HasAttribute("Irs4506C"))
            xmlElement.SetAttribute("Irs4506C", "N");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20202 data migration for Field : VASUMM.X149" + ex.Message);
      }
      this.EMXMLVersionID = "20.204";
    }

    private void cleanupXml20207()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.207)
        return;
      this.versionMigrationOccured = true;
      try
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
          {
            "PROPERTY",
            "_PostalCode"
          },
          {
            "EllieMae/RateLock/SellSide/Investor",
            "_PostalCode"
          },
          {
            "EllieMae/RateLock/Comparison/Investor",
            "_PostalCode"
          },
          {
            "EllieMae/INVESTOR",
            "PostalCode"
          }
        };
        bool needsUpdate = false;
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(keyValuePair.Key);
          if (xmlElement != null)
          {
            string attribute = xmlElement.GetAttribute(keyValuePair.Value);
            if (attribute != null && attribute.IndexOf("-") == -1 && attribute.Length == 9)
            {
              string str = Utils.FormatInput(attribute, FieldFormat.ZIPCODE, ref needsUpdate);
              if (needsUpdate)
                xmlElement.SetAttribute(keyValuePair.Value, str);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20207 data migration for Field : 15, 2284, 3829, VEND.X267, " + ex.Message);
      }
      this.EMXMLVersionID = "20.207";
    }

    private void cleanupXml20200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 20.2)
        return;
      this.versionMigrationOccured = true;
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          string str1 = this.readNodeValue("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/@_EmploymentVerificationMessage");
          if (!str1.Equals("Y") && !str1.Equals("N"))
          {
            string str2 = this.readNodeValue("BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/@_EmploymentVerificationAvailable");
            if (str2.Equals("Y") || str2.Equals("N"))
            {
              XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID =\"" + borrowerPairs[index].CoBorrower.Id + "\"]");
              xmlElement.SetAttribute("_EmploymentVerificationAvailable", str1);
              xmlElement.SetAttribute("_EmploymentVerificationMessage", str2);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml20200 data migration for Field : MORNET.X141/MORNET.X143" + ex.Message);
      }
      this.EMXMLVersionID = "20.200";
    }

    private void cleanupXml21101()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.101)
        return;
      this.versionMigrationOccured = true;
      try
      {
        Dictionary<string, string> dictionary = new Dictionary<string, string>()
        {
          {
            "PROPERTY",
            "_PostalCode"
          },
          {
            "EllieMae/RateLock/SellSide/Investor",
            "_PostalCode"
          },
          {
            "EllieMae/RateLock/Comparison/Investor",
            "_PostalCode"
          },
          {
            "EllieMae/INVESTOR",
            "PostalCode"
          }
        };
        bool needsUpdate = false;
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(keyValuePair.Key);
          if (xmlElement != null)
          {
            string attribute = xmlElement.GetAttribute(keyValuePair.Value);
            if (attribute != null && attribute.IndexOf("-") == -1 && attribute.Length == 9)
            {
              string str = Utils.FormatInput(attribute, FieldFormat.ZIPCODE, ref needsUpdate);
              if (needsUpdate)
                xmlElement.SetAttribute(keyValuePair.Value, str);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21.101 data migration for Field : 15, 2284, 3829, VEND.X267, " + ex.Message);
      }
      this.EMXMLVersionID = "21.101";
    }

    private void cleanupXml21100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.1)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string sourceValue = this.readNodeValue("URLA2020/@PresentSupplementalPropertyInsuranceAmount");
        if (!string.IsNullOrEmpty(sourceValue))
        {
          string targetPath = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].Borrower.Id + "\"]/URLA2020";
          this.copySingleAttribute(sourceValue, targetPath, "PresentSupplementalPropertyInsuranceAmount", false);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21100 data migration for Field PresentSupplementalPropertyInsuranceAmount: " + ex.Message);
      }
      this.EMXMLVersionID = "21.100";
    }

    private void cleanupXml21102()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.102)
        return;
      this.versionMigrationOccured = true;
      try
      {
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Name", "EllieMae/APPOINTMENTOFDESIGNEE", "Name");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "DesigneeAcceptedDate", "EllieMae/APPOINTMENTOFDESIGNEE", "DesigneeAcceptedDate");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "ContactName", "EllieMae/APPOINTMENTOFDESIGNEE", "ContactName");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "ContactTitle", "EllieMae/APPOINTMENTOFDESIGNEE", "ContactTitle");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Phone", "EllieMae/APPOINTMENTOFDESIGNEE", "Phone");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Email", "EllieMae/APPOINTMENTOFDESIGNEE", "Email");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Fax", "EllieMae/APPOINTMENTOFDESIGNEE", "Fax");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21102 data migration for APPOINTMENTOFDESIGNEE fields " + ex.Message);
      }
      this.EMXMLVersionID = "21.102";
    }

    private void cleanupXml21104()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.104)
        return;
      this.versionMigrationOccured = true;
      try
      {
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Name", "EllieMae/APPOINTMENTOFDESIGNEE", "Name");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "DesigneeAcceptedDate", "EllieMae/APPOINTMENTOFDESIGNEE", "DesigneeAcceptedDate");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "ContactName", "EllieMae/APPOINTMENTOFDESIGNEE", "ContactName");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "ContactTitle", "EllieMae/APPOINTMENTOFDESIGNEE", "ContactTitle");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Phone", "EllieMae/APPOINTMENTOFDESIGNEE", "Phone");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Email", "EllieMae/APPOINTMENTOFDESIGNEE", "Email");
        this.moveSingleAttribute("EllieMae/AppointmentOfDesignee", "Fax", "EllieMae/APPOINTMENTOFDESIGNEE", "Fax");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21102 data migration for APPOINTMENTOFDESIGNEE fields " + ex.Message);
      }
      this.EMXMLVersionID = "21.104";
    }

    private void cleanupXml21200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.2)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string sourceValue1 = this.readNodeValue("EllieMae/ULDD/DECLARATION/EllieMae/@FreddieBorrowerAlienStatus");
        if (!string.IsNullOrEmpty(sourceValue1))
        {
          string targetPath = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].Borrower.Id + "\"]/DECLARATION/EllieMae";
          this.copySingleAttribute(sourceValue1, targetPath, "FreddieAlienStatus", false);
        }
        string sourceValue2 = this.readNodeValue("EllieMae/ULDD/DECLARATION/EllieMae/@FreddieCoBorrowerAlienStatus");
        if (!string.IsNullOrEmpty(sourceValue2))
        {
          string targetPath = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].CoBorrower.Id + "\"]/DECLARATION/EllieMae";
          this.copySingleAttribute(sourceValue2, targetPath, "FreddieAlienStatus", false);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21200 data migration : " + ex.Message);
      }
      this.EMXMLVersionID = "21.200";
    }

    private void cleanupXml21201()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.201)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string str = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].Borrower.Id + "\"]/";
        string targetPath = "EllieMae/FANNIE_MAE";
        string[] strArray = new string[5]
        {
          "_PIWMessage",
          "_ValueRepAndWarrantyMessage",
          "_AssetRepAndWarrantyMessage",
          "_ValueRepAndWarrantyAvailable",
          "_AssetRepAndWarrantyReliefAvailable"
        };
        for (int index = 0; index < strArray.Length; ++index)
        {
          string sourceValue = this.readNodeValue(str + "@" + strArray[index]);
          if (!string.IsNullOrEmpty(sourceValue))
            this.copySingleAttribute(sourceValue, targetPath, strArray[index], false);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21201 data migration : " + ex.Message);
      }
      this.EMXMLVersionID = "21.201";
    }

    private void cleanupXml21203()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.203)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae");
        if (xmlElement != null)
        {
          foreach (XmlElement selectNode in xmlElement.SelectNodes("LOG/Record[@Type=\"Export\"]"))
          {
            XmlNodeList source = selectNode.SelectNodes("ServiceType");
            if (source.Count > 1)
            {
              foreach (XmlNode oldChild in source.Cast<XmlNode>().GroupBy<XmlNode, string>((Func<XmlNode, string>) (e => e.Attributes["name"].InnerText.Trim())).SelectMany<IGrouping<string, XmlNode>, XmlNode>((Func<IGrouping<string, XmlNode>, IEnumerable<XmlNode>>) (m => m.Skip<XmlNode>(1))))
                selectNode.RemoveChild(oldChild);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21203 data migration for Loan Node Duplication : " + ex.Message);
      }
      this.EMXMLVersionID = "21.203";
    }

    private void cleanupXml21204()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.204)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.lockRoot == null)
          this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
        string[] strArray = new string[3]
        {
          "119",
          "120",
          "123"
        };
        if (this.lockRoot != null)
        {
          for (int index = 0; index < strArray.Length; ++index)
          {
            if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + strArray[index] + "\"]") != null)
              this.RemoveLockAt(strArray[index]);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21204 data migration for removing locks on 119, 120 and 123 : " + ex.Message);
      }
      this.EMXMLVersionID = "21.204";
    }

    private void cleanupXml21300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.3)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string str1 = this.readNodeValue("EllieMae/Shipping/@InvestorConnectDeliveryStatus");
        string str2 = this.readNodeValue("EllieMae/Shipping/@InvestorConnectDeliveryStatusDateTime");
        string str3 = this.readNodeValue("EllieMae/Shipping/@InvestorConnectDeliveredToCompany");
        string str4 = this.readNodeValue("EllieMae/Shipping/@InvestorConnectDeliveredToCategory");
        if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2) && string.IsNullOrEmpty(str3))
        {
          if (string.IsNullOrEmpty(str4))
            goto label_7;
        }
        if ((XmlElement) this.root.SelectSingleNode("EllieMae/InvestorDeliveryLogs") == null)
        {
          XmlElement path = this.createPath("EllieMae/InvestorDeliveryLogs");
          XmlElement element = this.xmldoc.CreateElement("InvestorDeliveryLog");
          Mapping.AddEntityId(element);
          element.SetAttribute("Status", str1);
          element.SetAttribute("StatusDate", str2);
          element.SetAttribute("CompanyDeliveredTo", str3);
          element.SetAttribute("CategoryDeliveredTo", str4);
          path.AppendChild((XmlNode) element);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21300 data migration for CBIZ-35432 : " + ex.Message);
      }
label_7:
      try
      {
        string str = this.readNodeValue("EllieMae/@MERSNumber");
        if (!string.IsNullOrEmpty(str))
        {
          this.copySingleAttribute("Y", "EllieMae", "Mom", false);
          if (str.Length >= 7)
            this.copySingleAttribute(str.Substring(0, 7), "EllieMae", "MersOrgId", false);
        }
        this.copySingleAttribute(this.loanSettings.LoanAmountRounding ? "Y" : "N", "EllieMae", "IsLoanAmountRounding", false);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21300 data migration : " + ex.Message);
      }
      try
      {
        XmlElement newChild = (XmlElement) this.root.SelectSingleNode("EllieMae/Aiq");
        if (newChild == null)
        {
          newChild = this.xmldoc.CreateElement("Aiq");
          this.root.SelectSingleNode("EllieMae").AppendChild((XmlNode) newChild);
        }
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("EllieMae/CUSTOM_FIELDS");
        if (xmlElement1 != null)
        {
          XmlElement xmlElement2 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.FOLDERID']");
          if (xmlElement2 != null)
            newChild.SetAttribute("FolderId", xmlElement2.GetAttribute("FieldValue"));
          XmlElement xmlElement3 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.THREADID']");
          if (xmlElement3 != null)
            newChild.SetAttribute("ThreadId", xmlElement3.GetAttribute("FieldValue"));
          XmlElement xmlElement4 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.CABINETID']");
          if (xmlElement4 != null)
            newChild.SetAttribute("CabinetId", xmlElement4.GetAttribute("FieldValue"));
          XmlElement xmlElement5 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.INFLIGHTLOANID']");
          if (xmlElement5 != null)
            newChild.SetAttribute("InFlightLoanId", xmlElement5.GetAttribute("FieldValue"));
          XmlElement xmlElement6 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.SEGMENTID']");
          if (xmlElement6 != null)
            newChild.SetAttribute("SegmentId", xmlElement6.GetAttribute("FieldValue"));
          XmlElement xmlElement7 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.LOANWASMIRROREDONPST']");
          if (xmlElement7 != null)
            newChild.SetAttribute("LoanWasMirroredOnPst", xmlElement7.GetAttribute("FieldValue"));
          XmlElement xmlElement8 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.LASTUPDATEBYLISINPST']");
          if (xmlElement8 != null)
            newChild.SetAttribute("LastUpdateByLisInPst", xmlElement8.GetAttribute("FieldValue"));
          XmlElement xmlElement9 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.PROPAGATEDATA']");
          if (xmlElement9 != null)
            newChild.SetAttribute("PropagateData", xmlElement9.GetAttribute("FieldValue"));
          XmlElement xmlElement10 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.DOCMIRRORINFLIGHT']");
          if (xmlElement10 != null)
            newChild.SetAttribute("DocMirrorInFlight", xmlElement10.GetAttribute("FieldValue"));
          XmlElement xmlElement11 = (XmlElement) xmlElement1.SelectSingleNode("FIELD[@FieldID='CX.DV.AIQSITEID']");
          if (xmlElement11 != null)
            newChild.SetAttribute("AiqSiteId", xmlElement11.GetAttribute("FieldValue"));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml21300 data migration for CBIZ-40277 : " + ex.Message);
      }
      this.EMXMLVersionID = "21.300";
    }

    private void cleanupXml21402()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 21.402)
        return;
      this.versionMigrationOccured = true;
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs != null)
        {
          if (borrowerPairs.Length != 0)
          {
            for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
            {
              this.SetBorrowerPair(borrowerPairs[index1]);
              int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
              if (exlcudingAlimonyJobExp != 0)
              {
                for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
                {
                  string fieldAt = this.GetFieldAt("FL" + index2.ToString("00") + "32");
                  if (!string.IsNullOrEmpty(fieldAt) && !(fieldAt == "Conventional") && !(fieldAt == "FHA") && !(fieldAt == "VA") && !(fieldAt == "FarmersHomeAdministration") && !(fieldAt == "Other"))
                    this.SetFieldAt("FL" + index2.ToString("00") + "32", "");
                }
              }
            }
            this.SetBorrowerPair(borrowerPairs[0]);
          }
        }
      }
      catch (Exception ex)
      {
      }
      this.EMXMLVersionID = "21.402";
    }

    private void cleanupXml22100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.1)
        return;
      this.versionMigrationOccured = true;
      string[,] strArray = new string[118, 5]
      {
        {
          "TQLGSE1603",
          "EllieMae/TQL/GSETrackers/Tracker[15]/@GSECloseByDate",
          "TQL.X111",
          "DuRecommendationDate",
          "L"
        },
        {
          "TQLGSE1604",
          "EllieMae/TQL/GSETrackers/Tracker[15]/@GSECloseByDate2",
          "TQL.X112",
          "FannieUcdpStatusDate",
          "L"
        },
        {
          "TQLGSE1703",
          "EllieMae/TQL/GSETrackers/Tracker[16]/@GSECloseByDate",
          "TQL.X113",
          "CuRiskScoreDate",
          "L"
        },
        {
          "TQLGSE1704",
          "EllieMae/TQL/GSETrackers/Tracker[16]/@GSECloseByDate2",
          "TQL.X114",
          "EcStatus1003Date",
          "L"
        },
        {
          "TQLGSE1803",
          "EllieMae/TQL/GSETrackers/Tracker[17]/@GSECloseByDate",
          "TQL.X115",
          "EcStatusUlddDate",
          "L"
        },
        {
          "TQLGSE1804",
          "EllieMae/TQL/GSETrackers/Tracker[17]/@GSECloseByDate2",
          "TQL.X116",
          "UcdCollectionDate",
          "L"
        },
        {
          "TQLGSE1903",
          "EllieMae/TQL/GSETrackers/Tracker[18]/@GSECloseByDate",
          "TQL.X117",
          "LpaRiskClassDate",
          "L"
        },
        {
          "TQLGSE1904",
          "EllieMae/TQL/GSETrackers/Tracker[18]/@GSECloseByDate2",
          "TQL.X118",
          "LpaPurchEligDate",
          "L"
        },
        {
          "TQLGSE2003",
          "EllieMae/TQL/GSETrackers/Tracker[19]/@GSECloseByDate",
          "TQL.X119",
          "DocumentationLevelDate",
          "L"
        },
        {
          "TQLGSE2004",
          "EllieMae/TQL/GSETrackers/Tracker[19]/@GSECloseByDate2",
          "TQL.X120",
          "FreddieUcdpStatusDate",
          "L"
        },
        {
          "TQLGSE2103",
          "EllieMae/TQL/GSETrackers/Tracker[20]/@GSECloseByDate",
          "TQL.X121",
          "CollRiskScoreDate",
          "L"
        },
        {
          "TQLGSE2104",
          "EllieMae/TQL/GSETrackers/Tracker[20]/@GSECloseByDate2",
          "TQL.X122",
          "LqaPurchEligDate",
          "L"
        },
        {
          "TQLGSE2203",
          "EllieMae/TQL/GSETrackers/Tracker[21]/@GSECloseByDate",
          "TQL.X123",
          "LqaRiskAssessmentDate",
          "L"
        },
        {
          "TQLGSE2204",
          "EllieMae/TQL/GSETrackers/Tracker[21]/@GSECloseByDate2",
          "TQL.X124",
          "LclaUcdReqDate",
          "L"
        },
        {
          "TQLGSE0303",
          "EllieMae/TQL/GSETrackers/Tracker[2]/@GSECloseByDate",
          "TQL.X127",
          "FannieAssetRepAndWarrantDate",
          "L"
        },
        {
          "TQLGSE1203",
          "EllieMae/TQL/GSETrackers/Tracker[11]/@GSECloseByDate",
          "TQL.X130",
          "FreddieApprWaiverOfferedDate",
          "L"
        },
        {
          "TQLGSE1303",
          "EllieMae/TQL/GSETrackers/Tracker[12]/@GSECloseByDate",
          "TQL.X131",
          "FreddieColRepAndWarrantDate",
          "L"
        },
        {
          "TQLGSE1403",
          "EllieMae/TQL/GSETrackers/Tracker[13]/@GSECloseByDate",
          "TQL.X132",
          "FreddieAssetRepAndWarrantDate",
          "L"
        },
        {
          "TQLGSE1601",
          "EllieMae/TQL/GSETrackers/Tracker[15]/@DUFindingsMessageID",
          "TQL.X142",
          "RwtFannieVoe1Description",
          "B"
        },
        {
          "TQLGSE1602",
          "EllieMae/TQL/GSETrackers/Tracker[15]/@DUFindingsMessageText",
          "TQL.X192",
          "RwtFannieVoe1Description",
          "C"
        },
        {
          "TQLGSE1701",
          "EllieMae/TQL/GSETrackers/Tracker[16]/@DUFindingsMessageID",
          "TQL.X143",
          "RwtFannieVoe2Description",
          "B"
        },
        {
          "TQLGSE1702",
          "EllieMae/TQL/GSETrackers/Tracker[16]/@DUFindingsMessageText",
          "TQL.X193",
          "RwtFannieVoe2Description",
          "C"
        },
        {
          "TQLGSE1801",
          "EllieMae/TQL/GSETrackers/Tracker[17]/@DUFindingsMessageID",
          "TQL.X144",
          "RwtFannieVoe3Description",
          "B"
        },
        {
          "TQLGSE1802",
          "EllieMae/TQL/GSETrackers/Tracker[17]/@DUFindingsMessageText",
          "TQL.X194",
          "RwtFannieVoe3Description",
          "C"
        },
        {
          "TQLGSE0403",
          "EllieMae/TQL/GSETrackers/Tracker[3]/@GSECloseByDate",
          "TQL.X147",
          "RwtFannieVoe1Date",
          "B"
        },
        {
          "TQLGSE0404",
          "EllieMae/TQL/GSETrackers/Tracker[3]/@GSECloseByDate2",
          "TQL.X197",
          "RwtFannieVoe1Date",
          "C"
        },
        {
          "TQLGSE2301",
          "EllieMae/TQL/GSETrackers/Tracker[22]/@DUFindingsMessageID",
          "TQL.X145",
          "RwtFannieVoe2Status",
          "B"
        },
        {
          "TQLGSE2302",
          "EllieMae/TQL/GSETrackers/Tracker[22]/@DUFindingsMessageText",
          "TQL.X195",
          "RwtFannieVoe2Status",
          "C"
        },
        {
          "TQLGSE2303",
          "EllieMae/TQL/GSETrackers/Tracker[22]/@GSECloseByDate",
          "TQL.X148",
          "RwtFannieVoe2Date",
          "B"
        },
        {
          "TQLGSE2304",
          "EllieMae/TQL/GSETrackers/Tracker[22]/@GSECloseByDate2",
          "TQL.X198",
          "RwtFannieVoe2Date",
          "C"
        },
        {
          "TQLGSE2401",
          "EllieMae/TQL/GSETrackers/Tracker[23]/@DUFindingsMessageID",
          "TQL.X146",
          "RwtFannieVoe3Status",
          "B"
        },
        {
          "TQLGSE2402",
          "EllieMae/TQL/GSETrackers/Tracker[23]/@DUFindingsMessageText",
          "TQL.X196",
          "RwtFannieVoe3Status",
          "C"
        },
        {
          "TQLGSE2403",
          "EllieMae/TQL/GSETrackers/Tracker[23]/@GSECloseByDate",
          "TQL.X149",
          "RwtFannieVoe3Date",
          "B"
        },
        {
          "TQLGSE2404",
          "EllieMae/TQL/GSETrackers/Tracker[23]/@GSECloseByDate2",
          "TQL.X199",
          "RwtFannieVoe3Date",
          "C"
        },
        {
          "TQLGSE3602",
          "EllieMae/TQL/GSETrackers/Tracker[35]/@DUFindingsMessageText",
          "TQL.X150",
          "RwtFannieVoe2Message",
          "B"
        },
        {
          "TQLGSE3702",
          "EllieMae/TQL/GSETrackers/Tracker[36]/@DUFindingsMessageText",
          "TQL.X200",
          "RwtFannieVoe2Message",
          "C"
        },
        {
          "TQLGSE3802",
          "EllieMae/TQL/GSETrackers/Tracker[37]/@DUFindingsMessageText",
          "TQL.X151",
          "RwtFannieVoe3Message",
          "B"
        },
        {
          "TQLGSE3902",
          "EllieMae/TQL/GSETrackers/Tracker[38]/@DUFindingsMessageText",
          "TQL.X201",
          "RwtFannieVoe3Message",
          "C"
        },
        {
          "TQLGSE1901",
          "EllieMae/TQL/GSETrackers/Tracker[18]/@DUFindingsMessageID",
          "TQL.X152",
          "RwtFreddieVoe1Description",
          "B"
        },
        {
          "TQLGSE1902",
          "EllieMae/TQL/GSETrackers/Tracker[18]/@DUFindingsMessageText",
          "TQL.X202",
          "RwtFreddieVoe1Description",
          "C"
        },
        {
          "TQLGSE2001",
          "EllieMae/TQL/GSETrackers/Tracker[19]/@DUFindingsMessageID",
          "TQL.X153",
          "RwtFreddieVoe2Description",
          "B"
        },
        {
          "TQLGSE2002",
          "EllieMae/TQL/GSETrackers/Tracker[19]/@DUFindingsMessageText",
          "TQL.X203",
          "RwtFreddieVoe2Description",
          "C"
        },
        {
          "TQLGSE2101",
          "EllieMae/TQL/GSETrackers/Tracker[20]/@DUFindingsMessageID",
          "TQL.X154",
          "RwtFreddieVoe3Description",
          "B"
        },
        {
          "TQLGSE2102",
          "EllieMae/TQL/GSETrackers/Tracker[20]/@DUFindingsMessageText",
          "TQL.X204",
          "RwtFreddieVoe3Description",
          "C"
        },
        {
          "TQLGSE2501",
          "EllieMae/TQL/GSETrackers/Tracker[24]/@DUFindingsMessageID",
          "TQL.X155",
          "RwtFreddieVoe1Status",
          "B"
        },
        {
          "TQLGSE2502",
          "EllieMae/TQL/GSETrackers/Tracker[24]/@DUFindingsMessageText",
          "TQL.X205",
          "RwtFreddieVoe1Status",
          "C"
        },
        {
          "TQLGSE2503",
          "EllieMae/TQL/GSETrackers/Tracker[24]/@GSECloseByDate",
          "TQL.X158",
          "RwtFreddieVoe1Date",
          "B"
        },
        {
          "TQLGSE2504",
          "EllieMae/TQL/GSETrackers/Tracker[24]/@GSECloseByDate2",
          "TQL.X208",
          "RwtFreddieVoe1Date",
          "C"
        },
        {
          "TQLGSE2601",
          "EllieMae/TQL/GSETrackers/Tracker[25]/@DUFindingsMessageID",
          "TQL.X156",
          "RwtFreddieVoe2Status",
          "B"
        },
        {
          "TQLGSE2602",
          "EllieMae/TQL/GSETrackers/Tracker[25]/@DUFindingsMessageText",
          "TQL.X206",
          "RwtFreddieVoe2Status",
          "C"
        },
        {
          "TQLGSE2603",
          "EllieMae/TQL/GSETrackers/Tracker[25]/@GSECloseByDate",
          "TQL.X159",
          "RwtFreddieVoe2Date",
          "B"
        },
        {
          "TQLGSE2604",
          "EllieMae/TQL/GSETrackers/Tracker[25]/@GSECloseByDate2",
          "TQL.X209",
          "RwtFreddieVoe2Date",
          "C"
        },
        {
          "TQLGSE2701",
          "EllieMae/TQL/GSETrackers/Tracker[26]/@DUFindingsMessageID",
          "TQL.X157",
          "RwtFreddieVoe3Status",
          "B"
        },
        {
          "TQLGSE2702",
          "EllieMae/TQL/GSETrackers/Tracker[26]/@DUFindingsMessageText",
          "TQL.X207",
          "RwtFreddieVoe3Status",
          "C"
        },
        {
          "TQLGSE2703",
          "EllieMae/TQL/GSETrackers/Tracker[26]/@GSECloseByDate",
          "TQL.X160",
          "RwtFreddieVoe3Date",
          "B"
        },
        {
          "TQLGSE2704",
          "EllieMae/TQL/GSETrackers/Tracker[26]/@GSECloseByDate2",
          "TQL.X210",
          "RwtFreddieVoe3Date",
          "C"
        },
        {
          "TQLGSE4002",
          "EllieMae/TQL/GSETrackers/Tracker[39]/@DUFindingsMessageText",
          "TQL.X161",
          "RwtFreddieVoe1Message",
          "B"
        },
        {
          "TQLGSE4102",
          "EllieMae/TQL/GSETrackers/Tracker[40]/@DUFindingsMessageText",
          "TQL.X211",
          "RwtFreddieVoe1Message",
          "C"
        },
        {
          "TQLGSE4202",
          "EllieMae/TQL/GSETrackers/Tracker[41]/@DUFindingsMessageText",
          "TQL.X162",
          "RwtFreddieVoe2Message",
          "B"
        },
        {
          "TQLGSE4302",
          "EllieMae/TQL/GSETrackers/Tracker[42]/@DUFindingsMessageText",
          "TQL.X212",
          "RwtFreddieVoe2Message",
          "C"
        },
        {
          "TQLGSE4402",
          "EllieMae/TQL/GSETrackers/Tracker[43]/@DUFindingsMessageText",
          "TQL.X163",
          "RwtFreddieVoe3Message",
          "B"
        },
        {
          "TQLGSE4502",
          "EllieMae/TQL/GSETrackers/Tracker[44]/@DUFindingsMessageText",
          "TQL.X213",
          "RwtFreddieVoe3Message",
          "C"
        },
        {
          "TQLGSE0503",
          "EllieMae/TQL/GSETrackers/Tracker[4]/@GSECloseByDate",
          "TQL.X164",
          "RwtFannieVoiBaseIncomeDate",
          "B"
        },
        {
          "TQLGSE0603",
          "EllieMae/TQL/GSETrackers/Tracker[5]/@GSECloseByDate",
          "TQL.X165",
          "RwtFannieVoiBonusDate",
          "B"
        },
        {
          "TQLGSE0703",
          "EllieMae/TQL/GSETrackers/Tracker[6]/@GSECloseByDate",
          "TQL.X166",
          "RwtFannieVoiOvertimeDate",
          "B"
        },
        {
          "TQLGSE0803",
          "EllieMae/TQL/GSETrackers/Tracker[7]/@GSECloseByDate",
          "TQL.X167",
          "RwtFannieVoiCommissionDate",
          "B"
        },
        {
          "TQLGSE0903",
          "EllieMae/TQL/GSETrackers/Tracker[8]/@GSECloseByDate",
          "TQL.X168",
          "RwtFannieVoiSocialSecurityDate",
          "B"
        },
        {
          "TQLGSE1003",
          "EllieMae/TQL/GSETrackers/Tracker[9]/@GSECloseByDate",
          "TQL.X169",
          "RwtFannieVoiRetirementDate",
          "B"
        },
        {
          "TQLGSE1103",
          "EllieMae/TQL/GSETrackers/Tracker[10]/@GSECloseByDate",
          "TQL.X170",
          "RwtFannieVoiSelfEmployedDate",
          "B"
        },
        {
          "TQLGSE0504",
          "EllieMae/TQL/GSETrackers/Tracker[4]/@GSECloseByDate2",
          "TQL.X214",
          "RwtFannieVoiBaseIncomeDate",
          "C"
        },
        {
          "TQLGSE0604",
          "EllieMae/TQL/GSETrackers/Tracker[5]/@GSECloseByDate2",
          "TQL.X215",
          "RwtFannieVoiBonusDate",
          "C"
        },
        {
          "TQLGSE0704",
          "EllieMae/TQL/GSETrackers/Tracker[6]/@GSECloseByDate2",
          "TQL.X216",
          "RwtFannieVoiOvertimeDate",
          "C"
        },
        {
          "TQLGSE0804",
          "EllieMae/TQL/GSETrackers/Tracker[7]/@GSECloseByDate2",
          "TQL.X217",
          "RwtFannieVoiCommissionDate",
          "C"
        },
        {
          "TQLGSE0904",
          "EllieMae/TQL/GSETrackers/Tracker[8]/@GSECloseByDate2",
          "TQL.X218",
          "RwtFannieVoiSocialSecurityDate",
          "C"
        },
        {
          "TQLGSE1004",
          "EllieMae/TQL/GSETrackers/Tracker[9]/@GSECloseByDate2",
          "TQL.X219",
          "RwtFannieVoiRetirementDate",
          "C"
        },
        {
          "TQLGSE1104",
          "EllieMae/TQL/GSETrackers/Tracker[10]/@GSECloseByDate2",
          "TQL.X220",
          "RwtFannieVoiSelfEmployedDate",
          "C"
        },
        {
          "TQLGSE2801",
          "EllieMae/TQL/GSETrackers/Tracker[27]/@DUFindingsMessageID",
          "TQL.X171",
          "RwtFreddieVoiPayrollStatus",
          "B"
        },
        {
          "TQLGSE2901",
          "EllieMae/TQL/GSETrackers/Tracker[28]/@DUFindingsMessageID",
          "TQL.X172",
          "RwtFreddieVoiPensionStatus",
          "B"
        },
        {
          "TQLGSE3001",
          "EllieMae/TQL/GSETrackers/Tracker[29]/@DUFindingsMessageID",
          "TQL.X173",
          "RwtFreddieVoiSocialSecurityStatus",
          "B"
        },
        {
          "TQLGSE3101",
          "EllieMae/TQL/GSETrackers/Tracker[30]/@DUFindingsMessageID",
          "TQL.X174",
          "RwtFreddieVoiVaBenefitsStatus",
          "B"
        },
        {
          "TQLGSE3201",
          "EllieMae/TQL/GSETrackers/Tracker[31]/@DUFindingsMessageID",
          "TQL.X175",
          "RwtFreddieVoiMilitaryStatus",
          "B"
        },
        {
          "TQLGSE3301",
          "EllieMae/TQL/GSETrackers/Tracker[32]/@DUFindingsMessageID",
          "TQL.X176",
          "RwtFreddieVoiChildSupportStatus",
          "B"
        },
        {
          "TQLGSE3401",
          "EllieMae/TQL/GSETrackers/Tracker[33]/@DUFindingsMessageID",
          "TQL.X177",
          "RwtFreddieVoiSelfEmployedStatus",
          "B"
        },
        {
          "TQLGSE2803",
          "EllieMae/TQL/GSETrackers/Tracker[27]/@GSECloseByDate",
          "TQL.X178",
          "RwtFreddieVoiPayrollDate",
          "B"
        },
        {
          "TQLGSE2903",
          "EllieMae/TQL/GSETrackers/Tracker[28]/@GSECloseByDate",
          "TQL.X179",
          "RwtFreddieVoiPensionDate",
          "B"
        },
        {
          "TQLGSE3003",
          "EllieMae/TQL/GSETrackers/Tracker[29]/@GSECloseByDate",
          "TQL.X180",
          "RwtFreddieVoiSocialSecurityDate",
          "B"
        },
        {
          "TQLGSE3103",
          "EllieMae/TQL/GSETrackers/Tracker[30]/@GSECloseByDate",
          "TQL.X181",
          "RwtFreddieVoiVaBenefitsDate",
          "B"
        },
        {
          "TQLGSE3203",
          "EllieMae/TQL/GSETrackers/Tracker[31]/@GSECloseByDate",
          "TQL.X182",
          "RwtFreddieVoiMilitaryDate",
          "B"
        },
        {
          "TQLGSE3303",
          "EllieMae/TQL/GSETrackers/Tracker[32]/@GSECloseByDate",
          "TQL.X183",
          "RwtFreddieVoiChildSupportDate",
          "B"
        },
        {
          "TQLGSE3403",
          "EllieMae/TQL/GSETrackers/Tracker[33]/@GSECloseByDate",
          "TQL.X184",
          "RwtFreddieVoiSelfEmployedDate",
          "B"
        },
        {
          "TQLGSE2802",
          "EllieMae/TQL/GSETrackers/Tracker[27]/@DUFindingsMessageText",
          "TQL.X221",
          "RwtFreddieVoiPayrollStatus",
          "C"
        },
        {
          "TQLGSE2902",
          "EllieMae/TQL/GSETrackers/Tracker[28]/@DUFindingsMessageText",
          "TQL.X222",
          "RwtFreddieVoiPensionStatus",
          "C"
        },
        {
          "TQLGSE3002",
          "EllieMae/TQL/GSETrackers/Tracker[29]/@DUFindingsMessageText",
          "TQL.X223",
          "RwtFreddieVoiSocialSecurityStatus",
          "C"
        },
        {
          "TQLGSE3102",
          "EllieMae/TQL/GSETrackers/Tracker[30]/@DUFindingsMessageText",
          "TQL.X224",
          "RwtFreddieVoiVaBenefitsStatus",
          "C"
        },
        {
          "TQLGSE3202",
          "EllieMae/TQL/GSETrackers/Tracker[31]/@DUFindingsMessageText",
          "TQL.X225",
          "RwtFreddieVoiMilitaryStatus",
          "C"
        },
        {
          "TQLGSE3302",
          "EllieMae/TQL/GSETrackers/Tracker[32]/@DUFindingsMessageText",
          "TQL.X226",
          "RwtFreddieVoiChildSupportStatus",
          "C"
        },
        {
          "TQLGSE3402",
          "EllieMae/TQL/GSETrackers/Tracker[33]/@DUFindingsMessageText",
          "TQL.X227",
          "RwtFreddieVoiSelfEmployedStatus",
          "C"
        },
        {
          "TQLGSE2804",
          "EllieMae/TQL/GSETrackers/Tracker[27]/@GSECloseByDate2",
          "TQL.X228",
          "RwtFreddieVoiPayrollDate",
          "C"
        },
        {
          "TQLGSE2904",
          "EllieMae/TQL/GSETrackers/Tracker[28]/@GSECloseByDate2",
          "TQL.X229",
          "RwtFreddieVoiPensionDate",
          "C"
        },
        {
          "TQLGSE3004",
          "EllieMae/TQL/GSETrackers/Tracker[29]/@GSECloseByDate2",
          "TQL.X230",
          "RwtFreddieVoiSocialSecurityDate",
          "C"
        },
        {
          "TQLGSE3104",
          "EllieMae/TQL/GSETrackers/Tracker[30]/@GSECloseByDate2",
          "TQL.X231",
          "RwtFreddieVoiVaBenefitsDate",
          "C"
        },
        {
          "TQLGSE3204",
          "EllieMae/TQL/GSETrackers/Tracker[31]/@GSECloseByDate2",
          "TQL.X232",
          "RwtFreddieVoiMilitaryDate",
          "C"
        },
        {
          "TQLGSE3304",
          "EllieMae/TQL/GSETrackers/Tracker[32]/@GSECloseByDate2",
          "TQL.X233",
          "RwtFreddieVoiChildSupportDate",
          "C"
        },
        {
          "TQLGSE3404",
          "EllieMae/TQL/GSETrackers/Tracker[33]/@GSECloseByDate2",
          "TQL.X234",
          "RwtFreddieVoiSelfEmployedDate",
          "C"
        },
        {
          "TQLGSE4602",
          "EllieMae/TQL/GSETrackers/Tracker[45]/@DUFindingsMessageText",
          "TQL.X185",
          "RwtFreddieVoiPayrollMessage",
          "B"
        },
        {
          "TQLGSE4702",
          "EllieMae/TQL/GSETrackers/Tracker[46]/@DUFindingsMessageText",
          "TQL.X186",
          "RwtFreddieVoiPensionMessage",
          "B"
        },
        {
          "TQLGSE4802",
          "EllieMae/TQL/GSETrackers/Tracker[47]/@DUFindingsMessageText",
          "TQL.X187",
          "RwtFreddieVoiSocialSecurityMessage",
          "B"
        },
        {
          "TQLGSE4902",
          "EllieMae/TQL/GSETrackers/Tracker[48]/@DUFindingsMessageText",
          "TQL.X188",
          "RwtFreddieVoiVaBenefitsMessage",
          "B"
        },
        {
          "TQLGSE5002",
          "EllieMae/TQL/GSETrackers/Tracker[49]/@DUFindingsMessageText",
          "TQL.X189",
          "RwtFreddieVoiMilitaryMessage",
          "B"
        },
        {
          "TQLGSE5102",
          "EllieMae/TQL/GSETrackers/Tracker[50]/@DUFindingsMessageText",
          "TQL.X190",
          "RwtFreddieVoiChildSupportMessage",
          "B"
        },
        {
          "TQLGSE5202",
          "EllieMae/TQL/GSETrackers/Tracker[51]/@DUFindingsMessageText",
          "TQL.X191",
          "RwtFreddieVoiSelfEmployedMessage",
          "B"
        },
        {
          "TQLGSE5402",
          "EllieMae/TQL/GSETrackers/Tracker[53]/@DUFindingsMessageText",
          "TQL.X235",
          "RwtFreddieVoiPayrollMessage",
          "C"
        },
        {
          "TQLGSE5502",
          "EllieMae/TQL/GSETrackers/Tracker[54]/@DUFindingsMessageText",
          "TQL.X236",
          "RwtFreddieVoiPensionMessage",
          "C"
        },
        {
          "TQLGSE5602",
          "EllieMae/TQL/GSETrackers/Tracker[55]/@DUFindingsMessageText",
          "TQL.X237",
          "RwtFreddieVoiSocialSecurityMessage",
          "C"
        },
        {
          "TQLGSE5702",
          "EllieMae/TQL/GSETrackers/Tracker[56]/@DUFindingsMessageText",
          "TQL.X238",
          "RwtFreddieVoiVaBenefitsMessage",
          "C"
        },
        {
          "TQLGSE5802",
          "EllieMae/TQL/GSETrackers/Tracker[57]/@DUFindingsMessageText",
          "TQL.X239",
          "RwtFreddieVoiMilitaryMessage",
          "C"
        },
        {
          "TQLGSE5902",
          "EllieMae/TQL/GSETrackers/Tracker[58]/@DUFindingsMessageText",
          "TQL.X240",
          "RwtFreddieVoiChildSupportMessage",
          "C"
        },
        {
          "TQLGSE6002",
          "EllieMae/TQL/GSETrackers/Tracker[59]/@DUFindingsMessageText",
          "TQL.X241",
          "RwtFreddieVoiSelfEmployedMessage",
          "C"
        }
      };
      try
      {
        string str1 = "EllieMae/TQL";
        string str2 = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].Borrower.Id + "\"]/EllieMae/TQL";
        string str3 = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].CoBorrower.Id + "\"]/EllieMae/TQL";
        for (int index = 0; index < strArray.GetLength(0); ++index)
        {
          string sourceValue = this.readNodeValue(strArray[index, 1]);
          if (!string.IsNullOrEmpty(sourceValue))
          {
            string targetPath = str1;
            switch (strArray[index, 4])
            {
              case "B":
                targetPath = str2;
                break;
              case "C":
                targetPath = str3;
                break;
            }
            this.copySingleAttribute(sourceValue, targetPath, strArray[index, 3], false);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Exception in cleanupXml22100 data migration for CBIZ-39970 : " + ex.Message);
      }
      this.EMXMLVersionID = "22.100";
    }

    private void cleanupXml22200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.2)
        return;
      this.versionMigrationOccured = true;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/TQL/GSETrackers/Tracker");
        if (xmlNodeList != null)
        {
          foreach (object obj in xmlNodeList)
          {
            if (obj is XmlElement xmlElement && string.IsNullOrEmpty(xmlElement.GetAttribute("_ID")))
            {
              string attribute = xmlElement.GetAttribute("ID");
              xmlElement.SetAttribute("_ID", string.IsNullOrEmpty(attribute) ? Guid.NewGuid().ToString() : attribute);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22200 data migration for CBIZ-44356: " + ex.Message);
      }
      this.EMXMLVersionID = "22.200";
    }

    private void cleanupXml22300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.3)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (!string.IsNullOrEmpty(this.readNodeValue("EllieMae/HMDA/@ActionTaken")))
        {
          BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
          string str1 = "EMPLOYER[@EmploymentCurrentIndicator=\"Y\"]";
          string str2 = "EMPLOYER[@EmploymentCurrentIndicator=\"N\"]";
          int num1 = 0;
          int num2 = 0;
          for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
          {
            string str3 = "BORROWER[@BorrowerID=\"" + borrowerPairs[index1].Borrower.Id + "\"]/";
            string str4 = "BORROWER[@BorrowerID=\"" + borrowerPairs[index1].CoBorrower.Id + "\"]/";
            string[] strArray = new string[6]
            {
              "]/EllieMae/@BasePay",
              "]/EllieMae/@Overtime",
              "]/EllieMae/@Overtime",
              "]/EllieMae/@Commissions",
              "]/@EmploymentCurrentMilitaryEntitlement",
              "]/EllieMae/@Other"
            };
            for (int index2 = 1; index2 <= this.GetNumberOfEmployers(true, borrowerPairs); ++index2)
            {
              if (this.readNodeValue(str3 + "EMPLOYER[" + index2.ToString() + "]/@EmploymentCurrentIndicator") == "Y")
                ++num1;
              else
                ++num2;
              Decimal num3 = 0M;
              foreach (string str5 in strArray)
              {
                string s = this.readNodeValue(str3 + "EMPLOYER[" + index2.ToString() + str5);
                num3 += string.IsNullOrEmpty(s) ? 0M : Decimal.Parse(s);
              }
              string s1 = this.readNodeValue(str3 + "EMPLOYER[" + index2.ToString() + "]/@IncomeEmploymentMonthlyAmount");
              Decimal num4 = string.IsNullOrEmpty(s1) ? 0M : Decimal.Parse(s1);
              if (num3 != num4)
              {
                this.lockFieldMigration("BE" + index2.ToString("00") + "12", str3 + "EMPLOYER[" + index2.ToString() + "]", "IncomeEmploymentMonthlyAmount");
                switch (num1)
                {
                  case 1:
                    this.lockFieldMigration("FE0112", str3 + str1 + "[1]", "IncomeEmploymentMonthlyAmount");
                    continue;
                  case 2:
                    this.lockFieldMigration("FE0312", str3 + str1 + "[2]", "IncomeEmploymentMonthlyAmount");
                    continue;
                  default:
                    if (num2 == 1)
                    {
                      this.lockFieldMigration("FE0512", str3 + str2 + "[1]", "IncomeEmploymentMonthlyAmount");
                      continue;
                    }
                    continue;
                }
              }
            }
            num2 = num1 = 0;
            for (int index3 = 1; index3 <= this.GetNumberOfEmployers(false, borrowerPairs); ++index3)
            {
              if (this.readNodeValue(str4 + "EMPLOYER[" + index3.ToString() + "]/@EmploymentCurrentIndicator") == "Y")
                ++num1;
              else
                ++num2;
              Decimal num5 = 0M;
              foreach (string str6 in strArray)
              {
                string s = this.readNodeValue(str4 + "EMPLOYER[" + index3.ToString() + str6);
                num5 += string.IsNullOrEmpty(s) ? 0M : Decimal.Parse(s);
              }
              string s2 = this.readNodeValue(str4 + "EMPLOYER[" + index3.ToString() + "]/@IncomeEmploymentMonthlyAmount");
              Decimal num6 = string.IsNullOrEmpty(s2) ? 0M : Decimal.Parse(s2);
              if (num5 != num6)
              {
                this.lockFieldMigration("CE" + index3.ToString("00") + "12", str4 + "EMPLOYER[" + index3.ToString() + "]", "IncomeEmploymentMonthlyAmount");
                switch (num1)
                {
                  case 1:
                    this.lockFieldMigration("FE0212", str4 + str1 + "[1]", "IncomeEmploymentMonthlyAmount");
                    continue;
                  case 2:
                    this.lockFieldMigration("FE0412", str4 + str1 + "[2]", "IncomeEmploymentMonthlyAmount");
                    continue;
                  default:
                    if (num2 == 1)
                    {
                      this.lockFieldMigration("FE0612", str4 + str2 + "[1]", "IncomeEmploymentMonthlyAmount");
                      continue;
                    }
                    continue;
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22300 data migration for CBIZ-43788: " + ex.Message);
      }
      this.EMXMLVersionID = "22.300";
    }

    private void cleanupXml22303()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.303)
        return;
      this.versionMigrationOccured = true;
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs != null)
        {
          if (borrowerPairs.Length != 0)
          {
            foreach (BorrowerPair pair in borrowerPairs)
            {
              if (this.GetFieldAtXpath("BORROWER[@BorrowerID=\"" + pair.Borrower.Id + "\"]/EllieMae/PAIR/TAX_4506/@Irs4506C") == "Y")
                this.SetFieldAt("IRS4506.X92", "Sep2020", pair);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22303 data migration for CBIZ-48443: " + ex.Message);
      }
      try
      {
        string sourceValue = this.readNodeValue("EllieMae/USDA/CaseNumber/@BorrowerID");
        if (!string.IsNullOrEmpty(sourceValue))
        {
          string targetPath = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].Borrower.Id + "\"]/EllieMae/USDA";
          this.copySingleAttribute(sourceValue, targetPath, "RhsBorrowerId", false);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22303 data migration for CBIZ-48530 : " + ex.Message);
      }
      this.EMXMLVersionID = "22.303";
    }

    private void cleanupXml22304()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.304)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.readNodeValue("MORTGAGE_TERMS/@MortgageType") == "HELOC")
        {
          if (!string.IsNullOrEmpty(this.readNodeValue("EllieMae/@TeaserRate")))
          {
            string str = this.readNodeValue("EllieMae/HMDA/@ActionTaken");
            if (!("Loan Originated" == str) && !("Application approved but not accepted" == str) && !("Loan purchased by your institution" == str))
            {
              if (!("Preapproval request approved but not accepted" == str))
                goto label_9;
            }
            this.lockFieldMigration("HMDA.X81", "EllieMae/HMDA", "InterestRate", true);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22304 data migration for CBIZ-49590 : " + ex.Message);
      }
label_9:
      this.EMXMLVersionID = "22.304";
    }

    private void cleanupXml22306()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.306)
        return;
      this.versionMigrationOccured = true;
      try
      {
        this.lockFieldMigration("L128", "_CLOSING_DOCUMENTS/RESPA_HUD_DETAIL/Line201", "_LineItemAmount", false);
        if (this.readNodeValue("TRANSACTION_DETAIL/EllieMae/@UseItemizedCredits") == "Y")
          this.lockFieldMigration("URLA.X151", "URLA2020", "TotalOtherAssetToLoan", true);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22306 data migration for CBIZ-49421: " + ex.Message);
      }
      this.EMXMLVersionID = "22.306";
    }

    private void cleanupXml23100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 23.1)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (this.lockRoot == null)
          this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
        if (this.lockRoot != null)
        {
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) 4673 + "\"]") != null)
            this.RemoveLockAt(string.Concat((object) 4673));
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) 4674 + "\"]") != null)
            this.RemoveLockAt(string.Concat((object) 4674));
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) 4676 + "\"]") != null)
            this.RemoveLockAt(string.Concat((object) 4676));
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) 4677 + "\"]") != null)
            this.RemoveLockAt(string.Concat((object) 4677));
          if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + (object) 4683 + "\"]") != null)
            this.RemoveLockAt(string.Concat((object) 4683));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23100 data migration for CBIZ-47517: " + ex.Message);
      }
      try
      {
        if (!string.IsNullOrEmpty(this.readNodeValue("PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"MI\"]/@_PaymentAmount")))
        {
          string str = this.readNodeValue("EllieMae/CLOSING_COST[1]/ClosingDisclosure2015/@EscrowIndicator");
          string sourceValue = "";
          switch (str)
          {
            case "Y":
              sourceValue = "Y";
              break;
            case "N":
              sourceValue = "N";
              string targetPath1 = "EllieMae/HUD1ES";
              double num = this.toDouble(this.readNodeValue(targetPath1 + "/@EscrowPaymentYearly"));
              this.copySingleAttribute("0.0", targetPath1, "EscrowPaymentYearly", true);
              this.copySingleAttribute((num + this.toDouble(this.readNodeValue(targetPath1 + "/@NonEscrowCostsYearly"))).ToString(), targetPath1, "NonEscrowCostsYearly", true);
              this.copySingleAttribute("0.0", "EllieMae/CLOSING_COST[1]/ClosingDisclosure2015", "MonthlyEscrowPayment", true);
              break;
          }
          if (sourceValue != "")
          {
            string targetPath2 = "EllieMae/CLOSING_COST[1]/NewHud/HUD1PG3";
            this.copySingleAttribute(sourceValue, targetPath2, "HasEscrowMortgageInsurancesIndicator", false);
            string targetPath3 = "EllieMae/CLOSING_COST[1]/NewHud/Section1000/Details";
            this.copySingleAttribute(sourceValue, targetPath3, "Line1003EscrowedIndicator2015", false);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23100 data migration for CBIZ-49377: " + ex.Message);
      }
      this.EMXMLVersionID = "23.100";
    }

    private void cleanupXml22302()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 22.302)
        return;
      try
      {
        string sourceValue = this.readNodeValue("EllieMae/USDA/CaseNumber/@BorrowerID");
        if (!string.IsNullOrEmpty(sourceValue))
        {
          string targetPath = "BORROWER[@BorrowerID=\"" + this.GetBorrowerPairs()[0].Borrower.Id + "\"]/EllieMae/USDA";
          this.copySingleAttribute(sourceValue, targetPath, "RhsBorrowerId", false);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml22302 data migration : " + ex.Message);
      }
      this.EMXMLVersionID = "22.302";
    }

    private void cleanupXml23300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 23.3)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string attribute1 = this.root.GetAttribute("TPOConnectStatusUpdated");
        string attribute2 = this.root.GetAttribute("TPOConnectStatus");
        if (!string.IsNullOrEmpty(attribute1))
        {
          if (string.IsNullOrEmpty(attribute2))
            this.root.SetAttribute("TPOConnectStatus", attribute1);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23300 data migration for CBIZ-52012: " + ex.Message);
      }
      try
      {
        if ("VA" == this.readNodeValue("MORTGAGE_TERMS/@MortgageType"))
        {
          if ("IRRRL" == this.readNodeValue("EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY/@LoanCode"))
          {
            if (this.loanIsDisclosed())
              this.lockFieldMigration("QM.X25", "EllieMae/ATR_QM/Eligibility", "IsEligibleForSafeHarbor");
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23300 data migration for CBIZ-33216: " + ex.Message);
      }
      try
      {
        if (!string.IsNullOrEmpty(this.GetFieldAt("934")))
          this.lockFieldMigration("934", "EllieMae", "FirstTimeHomebuyers");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23300 data migration for Field 934: " + ex.Message);
      }
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        string s1 = this.readNodeValue("EllieMae/USDA/AnnualIncome/@AdditionalMemberBaseIncome");
        Decimal num1 = string.IsNullOrEmpty(s1) ? 0M : Decimal.Parse(s1);
        string s2 = this.readNodeValue("EllieMae/USDA/AnnualIncome/@DependentDeduction");
        Decimal num2 = string.IsNullOrEmpty(s2) ? 0M : Decimal.Parse(s2);
        string s3 = this.readNodeValue("EllieMae/USDA/AnnualIncome/@AnnualChildCareExpenses");
        Decimal num3 = num2 + (string.IsNullOrEmpty(s3) ? 0M : Decimal.Parse(s3));
        string s4 = this.readNodeValue("EllieMae/USDA/AnnualIncome/@ElderlyHouseholdDeduction");
        Decimal num4 = num3 + (string.IsNullOrEmpty(s4) ? 0M : Decimal.Parse(s4));
        string s5 = this.readNodeValue("EllieMae/USDA/AnnualIncome/@DisabilityDeduction");
        Decimal num5 = num4 + (string.IsNullOrEmpty(s5) ? 0M : Decimal.Parse(s5));
        string s6 = this.readNodeValue("EllieMae/USDA/AnnualIncome/@MedicalExpenses");
        Decimal num6 = num5 + (string.IsNullOrEmpty(s6) ? 0M : Decimal.Parse(s6));
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          string str1 = "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/";
          string str2 = "BORROWER[@BorrowerID=\"" + borrowerPairs[index].CoBorrower.Id + "\"]/";
          string s7 = this.readNodeValue(str1 + "EllieMae/USDA/@BaseIncome");
          Decimal num7 = num1 + (string.IsNullOrEmpty(s7) ? 0M : Decimal.Parse(s7));
          string s8 = this.readNodeValue(str2 + "EllieMae/USDA/@BaseIncome");
          Decimal num8 = num7 + (string.IsNullOrEmpty(s8) ? 0M : Decimal.Parse(s8));
          string s9 = this.readNodeValue(str1 + "EllieMae/USDA/@AdditionalIncomeFromPrimaryEmployment");
          Decimal num9 = num8 + (string.IsNullOrEmpty(s9) ? 0M : Decimal.Parse(s9));
          string s10 = this.readNodeValue(str1 + "EllieMae/USDA/@AssetIncome");
          num1 = num9 + (string.IsNullOrEmpty(s10) ? 0M : Decimal.Parse(s10));
        }
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/USDA") ?? this.createPath("EllieMae/USDA");
        xmlElement.SetAttribute("TotalAnnualIncome", num1 == 0M ? "" : string.Concat((object) num1));
        Decimal num10 = num1 - num6;
        xmlElement.SetAttribute("TotalAdjustedAnnualIncome", num10 == 0M ? "" : string.Concat((object) num10));
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23300 data migration for CBIZ-54047: " + ex.Message);
      }
      this.EMXMLVersionID = "23.300";
    }

    private void cleanupXml23302()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 23.302)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (((XmlElement) this.root.SelectSingleNode("EllieMae")).GetAttribute("Print2003Application") == "2020")
        {
          if (this.readNodeValue("EllieMae/HMDA/@ActionTaken") != "")
          {
            BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
            for (int index = 0; index < borrowerPairs.Length; ++index)
            {
              this.lockFieldMigration("1726", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"HazardInsurance\"]", "_PaymentAmount", true);
              this.lockFieldMigration("4947", "BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM", "SupplementalInsuranceAmount", true);
              ((XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs[index].Borrower.Id + "\"]/EllieMae/PAIR/TSUM")).SetAttribute("SupplementalInsuranceAmount", "");
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml23302 data migration for CBIZ-55838/CBIZ-56424: " + ex.Message);
      }
      this.EMXMLVersionID = "23.302";
    }

    private void cleanupXml24100()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 24.1)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string str = this.readNodeValue("EllieMae/HMDA/@ActionTaken");
        if (!(str != ""))
        {
          if (str == "")
          {
            if (!this.loanIsDisclosed())
              goto label_8;
          }
          else
            goto label_8;
        }
        this.lockFieldMigration("137", "TRANSACTION_DETAIL", "EstimatedClosingCostsAmount", true);
        this.lockFieldMigration("1046", "EllieMae/FHA_VA_LOANS", "DiscountPoints", true);
        this.lockFieldMigration("1093", "TRANSACTION_DETAIL", "BorrowerPaidDiscountPointsTotalAmount", true);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24100 data migration for CBIZ-49475: " + ex.Message);
      }
label_8:
      try
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        foreach (BorrowerPair pair in borrowerPairs)
        {
          this.SetBorrowerPair(pair);
          XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pair.Borrower.Id + "\"]/EllieMae/PAIR/TAX_4506");
          string attribute1 = xmlElement?.GetAttribute("Irs4506C");
          string attribute2 = xmlElement?.GetAttribute("Irs4506CPrintVersion");
          string val = "";
          int numberOfTaX4506Ts = this.GetNumberOfTAX4506Ts(false);
          if (attribute1 == "Y" && attribute2 == "Sep2020")
            val = "4506-CSept2020";
          else if (attribute1 == "Y" && attribute2 == "Oct2022")
            val = "4506-COct2022";
          else if (attribute1 == "N")
            val = "4506-T";
          xmlElement?.SetAttribute("FormVersion", val);
          for (int index = 1; index <= numberOfTaX4506Ts; ++index)
            this.SetFieldAt("IR" + index.ToString("00") + "93", val, pair);
        }
        this.SetBorrowerPair(borrowerPairs[0]);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24100 data migration for CBIZ-55909: " + ex.Message);
      }
      this.EMXMLVersionID = "24.100";
    }

    private void cleanupXml24200()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 24.2)
        return;
      this.versionMigrationOccured = true;
      try
      {
        if (!string.IsNullOrEmpty(this.readNodeValue("EllieMae/HMDA/@ActionTaken")))
        {
          string str1 = this.readNodeValue("LOAN_PURPOSE/@_Type");
          double num = Utils.ParseDouble((object) this.readNodeValue("LOAN_PRODUCT_DATA/ARM/@_QualifyingRatePercent"));
          string str2 = this.readNodeValue("MORTGAGE_TERMS/@MortgageType");
          BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
          bool flag = true;
          foreach (BorrowerPair pair in borrowerPairs)
          {
            this.SetBorrowerPair(pair);
            string str3 = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]";
            this.lockFieldMigration("1724", str3 + "/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"FirstMortgagePrincipalAndInterest\"]", "_PaymentAmount", true);
            this.lockFieldMigration("1725", str3 + "/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"OtherMortgageLoanPrincipalAndInterest\"]", "_PaymentAmount", true);
            string str4 = this.readNodeValue(str3 + "/EllieMae/PAIR/@PropertyUsageType");
            if (flag && str2 == "HELOC" && num == 0.0 && str4 == "Investor" && str1 == "Purchase")
              this.lockFieldMigration("URLA.X81", "/URLA2020", "RentalEstimatedNetMonthlyRentAmount", true);
            if ((str2 == "HELOC" || str1 == "ConstructionToPermanent") && num == 0.0 && (str4 == "Investor" || str4 == "SecondHome"))
            {
              this.lockFieldMigration("462", str3 + "/EllieMae/PAIR/TSUM", "GrossNegativeCashFlow", true);
              this.lockFieldMigration("1169", str3 + "/EllieMae", "PositiveCashFlow", true);
              if (flag)
                this.lockFieldMigration("106", str3 + "/CURRENT_INCOME[@IncomeType=\"NetRentalIncome\"]", "_MonthlyTotalAmount", true);
            }
            flag = false;
          }
          this.SetBorrowerPair(borrowerPairs[0]);
          this.lockFieldMigration("NEWHUD2.X951", "EllieMae/CLOSING_COST[1]/NewHud/Section800/Details", "Line802eSec32PointsAndFees2015", true);
          this.lockFieldMigration("NEWHUD2.X984", "EllieMae/CLOSING_COST[1]/NewHud/Section800/Details", "Line802fSec32PointsAndFees2015", true);
          this.lockFieldMigration("NEWHUD2.X1017", "EllieMae/CLOSING_COST[1]/NewHud/Section800/Details", "Line802gSec32PointsAndFees2015", true);
          this.lockFieldMigration("NEWHUD2.X1050", "EllieMae/CLOSING_COST[1]/NewHud/Section800/Details", "Line802hSec32PointsAndFees2015", true);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24200 data migration for CBIZ-44660, CBIZ-53155, CBIZ-57838, CBIZ-55578 and CBIZ-46435: " + ex.Message);
      }
      try
      {
        FieldSettings fieldSettings = this.loanSettings.FieldSettings;
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/CUSTOM_FIELDS/FIELD");
        if (xmlNodeList != null)
        {
          foreach (object obj in xmlNodeList)
          {
            if (obj is XmlElement xmlElement)
            {
              string attribute = xmlElement.GetAttribute("FieldID");
              if (!string.IsNullOrWhiteSpace(attribute))
              {
                if (CustomFieldInfo.IsCustomFieldID(attribute))
                {
                  if (!string.Equals(attribute.ToUpper(), attribute))
                    xmlElement.SetAttribute("FieldID", attribute.ToUpper());
                }
                else if (LockRequestCustomField.IsLockRequestCustomField(attribute))
                {
                  string idForCustomField = LockRequestCustomField.GetBaseFieldIDForCustomField(attribute);
                  if (CustomFieldInfo.IsCustomFieldID(idForCustomField))
                  {
                    if (!string.Equals(attribute.ToUpper(), attribute))
                      xmlElement.SetAttribute("FieldID", attribute.ToUpper());
                  }
                  else
                  {
                    Field field = this[idForCustomField];
                    if (field != null && !string.IsNullOrWhiteSpace(field.Id))
                    {
                      string customFieldId = LockRequestCustomField.GenerateCustomFieldID(field.Id);
                      if (!string.Equals(attribute, customFieldId))
                        xmlElement.SetAttribute("FieldID", customFieldId);
                    }
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24200 data migration for EBSP-54435: " + ex.Message);
      }
      try
      {
        if (((XmlElement) this.root.SelectSingleNode("EllieMae")).GetAttribute("Print2003Application") == "2020")
        {
          if (this.readNodeValue("EllieMae/HMDA/@ActionTaken") != "")
          {
            foreach (BorrowerPair borrowerPair in this.GetBorrowerPairs())
              this.lockFieldMigration("URLA.X230", "BORROWER[@BorrowerID=\"" + borrowerPair.Borrower.Id + "\"]/URLA2020", "TotalAppliedToDownpayment", true);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24200 data migration for CBIZ-59784: " + ex.Message);
      }
      this.EMXMLVersionID = "24.200";
    }

    private void cleanupXml24201()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 24.201)
        return;
      this.versionMigrationOccured = true;
      string sourceValue = this.readNodeValue("EllieMae/HMDA/@RepurchasedActionDate");
      if (Utils.ParseDate((object) sourceValue) != DateTime.MinValue)
        this.copySingleAttribute(sourceValue, "EllieMae/HMDA", "RepurchasedActionDate2", false);
      this.EMXMLVersionID = "24.201";
    }

    private void cleanupXml24300()
    {
      if (this.toDouble(this.EMXMLVersionID) >= 24.3)
        return;
      this.versionMigrationOccured = true;
      try
      {
        string str1 = this.readNodeValue("AFFORDABLE_LENDING/@AffordableHousingAreaMedianIncomeYear");
        string str2 = this.readNodeValue("AFFORDABLE_LENDING/@HUDMedianIncomeAmount");
        string str3 = this.readNodeValue("AFFORDABLE_LENDING/@AffordableHousingAreaMedianIncome80Perc");
        string str4 = this.readNodeValue("AFFORDABLE_LENDING/@AffordableHousingAreaMedianIncome50Perc");
        if (string.IsNullOrWhiteSpace(str1))
        {
          if (string.IsNullOrWhiteSpace(str2) && string.IsNullOrWhiteSpace(str3))
          {
            if (string.IsNullOrWhiteSpace(str4))
              goto label_7;
          }
          ((XmlElement) this.root.SelectSingleNode("AFFORDABLE_LENDING") ?? this.createPath("AFFORDABLE_LENDING")).SetAttribute("AffordableHousingAreaMedianIncomeManualYear", "Y");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24300 data migration for CBIZ-60414: " + ex.Message);
      }
label_7:
      try
      {
        string str5 = this.readNodeValue("EllieMae/@EstimatedMfiYear");
        string str6 = this.readNodeValue("EllieMae/@ActualMfiHundredPercent");
        string str7 = this.readNodeValue("EllieMae/@EstimatedMfiHundredPercent");
        if (string.IsNullOrWhiteSpace(str5))
        {
          if (string.IsNullOrWhiteSpace(str6))
          {
            if (string.IsNullOrWhiteSpace(str7))
              goto label_13;
          }
          ((XmlElement) this.root.SelectSingleNode("AFFORDABLE_LENDING") ?? this.createPath("AFFORDABLE_LENDING")).SetAttribute("AffordableHousingMedianFamilyIncomeManualYear", "Y");
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24300 data migration for CBIZ-60414: " + ex.Message);
      }
label_13:
      bool flag = false;
      try
      {
        flag = !string.IsNullOrEmpty(this.readNodeValue("EllieMae/HMDA/@ActionTaken")) || this.loanIsDisclosed();
        if (flag)
        {
          string str8 = this.readNodeValue("LOAN_PURPOSE/@_Type");
          double num = Utils.ParseDouble((object) this.readNodeValue("LOAN_PRODUCT_DATA/ARM/@_QualifyingRatePercent"));
          if (str8 == "ConstructionOnly")
          {
            if (num > 0.0)
            {
              BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
              foreach (BorrowerPair pair in borrowerPairs)
              {
                this.SetBorrowerPair(pair);
                string str9 = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]";
                if (this.readNodeValue(str9 + "/EllieMae/PAIR/@PropertyUsageType") == "PrimaryResidence")
                {
                  this.lockFieldMigration("1724", str9 + "/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"FirstMortgagePrincipalAndInterest\"]", "_PaymentAmount", true);
                  this.lockFieldMigration("1725", str9 + "/EllieMae/PAIR/TSUM/PROPOSED_HOUSING_EXPENSE[@HousingExpenseType=\"OtherMortgageLoanPrincipalAndInterest\"]", "_PaymentAmount", true);
                }
              }
              this.SetBorrowerPair(borrowerPairs[0]);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24300 data migration for CBIZ-47514: " + ex.Message);
      }
      try
      {
        if (flag)
        {
          string str10 = this.readNodeValue("LOAN_PURPOSE/@_Type");
          string str11 = this.readNodeValue("EllieMae/REGZ/@ConstructionLoanMethod");
          string str12 = this.readNodeValue("_CLOSING_DOCUMENTS/LOAN_DETAILS/INTERIM_INTEREST/@_PerDiemCalculationMethodType");
          if (!(str10 == "ConstructionOnly"))
          {
            if (!(str10 == "ConstructionToPermanent"))
              goto label_34;
          }
          if (str11 == "B")
          {
            if (!(str12 == "365/360"))
            {
              if (!(str12 == "365/365"))
                goto label_34;
            }
            this.lockFieldMigration("4088", "EllieMae", "EstimatedConstructionInterest", true);
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "exception in cleanupXml24300 data migration for CBIZ-60160: " + ex.Message);
      }
label_34:
      string str13 = this.readNodeValue("EllieMae/@GUID");
      if (!string.IsNullOrEmpty(str13))
      {
        Dictionary<string, string> dictionary = this.loanSettings.LoanExternalFields != null ? this.loanSettings.LoanExternalFields(str13) : (Dictionary<string, string>) null;
        if (dictionary != null)
        {
          if (dictionary.ContainsKey("ANALYZER.X1") || dictionary.ContainsKey("ANALYZER.X2") || dictionary.ContainsKey("ANALYZER.X3"))
          {
            XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("_Analyzer/EllieMae/ElliAnalyzersResultSummary/Income") ?? this.createPath("_Analyzer/EllieMae/ElliAnalyzersResultSummary/Income");
            string str14;
            if (!xmlElement.HasAttribute("Eligible") && dictionary.TryGetValue("ANALYZER.X1", out str14))
              xmlElement.SetAttribute("Eligible", str14);
            if (!xmlElement.HasAttribute("ExceptionsCount") && dictionary.TryGetValue("ANALYZER.X2", out str14))
              xmlElement.SetAttribute("ExceptionsCount", str14);
            if (!xmlElement.HasAttribute("Status") && dictionary.TryGetValue("ANALYZER.X3", out str14))
              xmlElement.SetAttribute("Status", str14);
          }
          if (dictionary.ContainsKey("ANALYZER.X4") || dictionary.ContainsKey("ANALYZER.X5") || dictionary.ContainsKey("ANALYZER.X6"))
          {
            XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("_Analyzer/EllieMae/ElliAnalyzersResultSummary/Credit") ?? this.createPath("_Analyzer/EllieMae/ElliAnalyzersResultSummary/Credit");
            string str15;
            if (!xmlElement.HasAttribute("Eligible") && dictionary.TryGetValue("ANALYZER.X4", out str15))
              xmlElement.SetAttribute("Eligible", str15);
            if (!xmlElement.HasAttribute("ExceptionsCount") && dictionary.TryGetValue("ANALYZER.X5", out str15))
              xmlElement.SetAttribute("ExceptionsCount", str15);
            if (!xmlElement.HasAttribute("Status") && dictionary.TryGetValue("ANALYZER.X6", out str15))
              xmlElement.SetAttribute("Status", str15);
          }
        }
      }
      this.EMXMLVersionID = "24.300";
    }

    private int GetNumberOfEmployers(bool borrower, BorrowerPair[] pairs)
    {
      return borrower ? this.root.SelectNodes("BORROWER[@BorrowerID=\"" + pairs[0].Borrower.Id + "\"]/EMPLOYER").Count : this.root.SelectNodes("BORROWER[@BorrowerID=\"" + pairs[0].CoBorrower.Id + "\"]/EMPLOYER").Count;
    }

    internal bool LoanHasLiabilityToBePaidoff()
    {
      bool bePaidoff = false;
      int numberOfNonVols = this.GetNumberOfNonVols();
      for (int index = 1; index <= numberOfNonVols; ++index)
      {
        string str = "UNFL" + index.ToString("00");
        if (this.GetFieldAt(str + "05") == "Y" && !this.GetFieldAt(str + "02").Contains("exceeding legal limits P.O.C."))
        {
          bePaidoff = true;
          break;
        }
      }
      if (bePaidoff)
        return bePaidoff;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      BorrowerPair currentBorrowerPair = this.currentBorrowerPair;
      for (int index1 = 0; index1 < borrowerPairs.Length; ++index1)
      {
        this.SetBorrowerPair(borrowerPairs[index1]);
        int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
        for (int index2 = 1; index2 <= exlcudingAlimonyJobExp; ++index2)
        {
          if (this.GetFieldAt("FL" + index2.ToString("00") + "18") == "Y" && this.GetFieldAt("FL" + index2.ToString("00") + "63") == "Y")
          {
            bePaidoff = true;
            break;
          }
        }
        if (bePaidoff)
          break;
      }
      if (currentBorrowerPair != null && (this.currentBorrowerPair == null || currentBorrowerPair.Id != this.currentBorrowerPair.Id))
        this.SetBorrowerPair(currentBorrowerPair);
      return bePaidoff;
    }

    private void lockFieldMigration(string fieldID, string nodePath, string attribute)
    {
      this.lockFieldMigration(fieldID, nodePath, attribute, false);
    }

    private void lockFieldMigration(
      string fieldID,
      string nodePath,
      string attribute,
      bool alwaysLock)
    {
      if ((XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + fieldID + "\"]") != null)
        return;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(nodePath);
      string attribute1 = xmlElement != null ? xmlElement.GetAttribute(attribute) : "";
      if ((attribute1 ?? "") == string.Empty && !alwaysLock)
        return;
      this.createLockField(fieldID, attribute1);
    }

    private void copySingleAttribute(
      string sourceValue,
      string targetPath,
      string targetAttr,
      bool overwrite)
    {
      if (sourceValue == "")
        return;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(targetPath) ?? this.createPath(targetPath);
      if (xmlElement == null || !((xmlElement.GetAttribute(targetAttr) ?? "") == "" | overwrite))
        return;
      xmlElement.SetAttribute(targetAttr, sourceValue);
    }

    private void copySingleAttribute(
      string sourcePath,
      string sourceAttr,
      string targetPath,
      string targetAttr,
      bool overwrite)
    {
      string empty = string.Empty;
      string str;
      if (sourcePath != null)
      {
        XmlNode xmlNode = this.root.SelectSingleNode(sourcePath);
        if (xmlNode == null)
          return;
        str = ((XmlElement) xmlNode).GetAttribute(sourceAttr);
        if ((str ?? "") == "")
          return;
      }
      else
        str = sourceAttr;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(targetPath) ?? this.createPath(targetPath);
      if (xmlElement == null || !((xmlElement.GetAttribute(targetAttr) ?? "") == "" | overwrite))
        return;
      xmlElement.SetAttribute(targetAttr, str);
    }

    private void moveSingleAttribute(
      string oldPath,
      string oldAttr,
      string newPath,
      string newAttr)
    {
      this.moveSingleAttribute(oldPath, oldAttr, newPath, newAttr, false);
    }

    private void moveSingleAttribute(
      string oldPath,
      string oldAttr,
      string newPath,
      string newAttr,
      bool applyEmptySource)
    {
      XmlNode oldNode = this.root.SelectSingleNode(oldPath);
      if (!(oldNode != null | applyEmptySource))
        return;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(newPath) ?? this.createPath(newPath);
      if (xmlElement != null)
      {
        string str = string.Empty;
        if (oldNode != null)
          str = ((XmlElement) oldNode).GetAttribute(oldAttr);
        if (str == null)
          str = string.Empty;
        if (str != string.Empty | applyEmptySource)
          xmlElement.SetAttribute(newAttr, str);
      }
      if (oldNode != null)
        oldNode = this.root.SelectSingleNode(oldPath + "/@" + oldAttr);
      if (oldNode == null)
        return;
      this.removeNode(oldNode, oldPath + "/@" + oldAttr);
    }

    private void removeNode(XmlNode oldNode, string oldPath)
    {
      int num = oldPath.LastIndexOf('@');
      XmlElement oldChild = ((XmlAttribute) oldNode).OwnerElement;
      if (oldChild == null)
        return;
      string name = oldPath.Substring(num + 1);
      oldChild.RemoveAttribute(name);
      XmlElement parentNode;
      for (; oldChild.Attributes.Count == 0 && oldChild.ChildNodes.Count == 0; oldChild = parentNode)
      {
        parentNode = (XmlElement) oldChild.ParentNode;
        oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
      }
    }

    public bool IgnoreValidationErrors
    {
      get => this.ignoreValidationErrors;
      set => this.ignoreValidationErrors = value;
    }

    private XmlElement getBorrowerElement(string predicate)
    {
      return (XmlElement) this.root.SelectSingleNode("BORROWER" + predicate);
    }

    internal void SetBorrowerPair(BorrowerPair pair)
    {
      this.brwId = pair.Borrower.Id;
      this.coBrwId = pair.CoBorrower.Id;
      this.brwPredicate = "[@BorrowerID=\"" + this.brwId + "\"]";
      this.coBrwPredicate = "[@BorrowerID=\"" + this.coBrwId + "\"]";
      this.liabPath = "LIABILITY" + this.brwPredicate + "[not(@_Type=\"Alimony\") and not(@_Type=\"JobRelatedExpenses\")]";
      this.otherliabilitypath = "BORROWER" + this.brwPredicate + "/URLA2020/OtherLiabilities/OTHER_LIABILITY";
      this.additionalLoanpath = "BORROWER" + this.brwPredicate + "/URLA2020/AdditionalLoans/Additional_Loan";
      this.otherAssetPath = "BORROWER" + this.brwPredicate + "/URLA2020/OtherAssets/OTHER_ASSET";
      this.providedDocumentPath = "BORROWER" + this.brwPredicate + "/ProvidedDocuments/PROVIDED_DOCUMENT";
      this.brwElm = this.getBorrowerElement(this.brwPredicate);
      if ((this.coBrwId ?? "") != "")
        this.coBrwElm = this.getBorrowerElement(this.coBrwPredicate);
      this.currentBorrowerPair = pair;
    }

    internal BorrowerPair[] GetBorrowerPairs()
    {
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("BORROWER");
      BorrowerPair[] borrowerPairs;
      switch (xmlNodeList1.Count)
      {
        case 0:
          borrowerPairs = (BorrowerPair[]) null;
          break;
        case 1:
          borrowerPairs = new BorrowerPair[1]
          {
            new BorrowerPair(this.createBorrower((XmlElement) xmlNodeList1[0]), (Borrower) null)
          };
          break;
        default:
          XmlNodeList xmlNodeList2 = this.root.SelectNodes("BORROWER[@_PrintPositionType=\"Borrower\"]");
          int count = xmlNodeList2.Count;
          borrowerPairs = new BorrowerPair[count];
          for (int i = 0; i < count; ++i)
          {
            XmlNode node = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + xmlNodeList2[i].Attributes["JointAssetBorrowerID"].Value + "\"]");
            borrowerPairs[i] = new BorrowerPair(this.createBorrower((XmlElement) xmlNodeList2[i]), this.createBorrower((XmlElement) node));
          }
          break;
      }
      return borrowerPairs;
    }

    private Borrower createBorrower(XmlElement node)
    {
      return node == null ? (Borrower) null : new Borrower(node.GetAttribute("_FirstName"), node.GetAttribute("_LastName"), node.GetAttribute("BorrowerID"), node.GetAttribute("_eid"));
    }

    public BorrowerPair GetBorrowerPairByVisibleIndex(int index)
    {
      if (index < 1)
        throw new ArgumentException("Borrower Pair index must be >= 1");
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      if (borrowerPairs == null)
        return (BorrowerPair) null;
      return index > borrowerPairs.Length ? (BorrowerPair) null : borrowerPairs[index - 1];
    }

    public bool setEnableEnchancedConditionFlag()
    {
      return this.GetFieldAtXpath("EllieMae/@UseEnhancedConditionIndicator") == "Y";
    }

    public BorrowerPair CreateBorrowerPair()
    {
      XmlElement element1 = this.xmldoc.CreateElement("BORROWER");
      Mapping.AddEntityId(element1);
      XmlElement element2 = this.xmldoc.CreateElement("BORROWER");
      Mapping.AddEntityId(element2);
      string newId1 = this.generateNewId();
      string newId2 = this.generateNewId();
      this.setFieldAtXpath(element1, "BorrowerID", "BORPAIRID", newId1, this.currentBorrowerPair);
      element1.SetAttribute("JointAssetBorrowerID", newId2);
      element1.SetAttribute("_PrintPositionType", "Borrower");
      this.setFieldAtXpath(element2, "BorrowerID", "BORPAIRID", newId2, this.currentBorrowerPair);
      element2.SetAttribute("JointAssetBorrowerID", newId1);
      element2.SetAttribute("_PrintPositionType", "CoBorrower");
      XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER");
      XmlNode refChild = xmlNodeList[xmlNodeList.Count - 1];
      refChild.ParentNode.InsertAfter((XmlNode) element1, refChild);
      refChild.ParentNode.InsertAfter((XmlNode) element2, (XmlNode) element1);
      BorrowerPair pair = new BorrowerPair(this.createBorrower(element1), this.createBorrower(element2));
      string id = this.currentBorrowerPair.Id;
      this.SetBorrowerPair(pair);
      this.addMissingFields();
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (!(borrowerPairs[index].Id != id))
        {
          this.SetBorrowerPair(borrowerPairs[index]);
          break;
        }
      }
      return pair;
    }

    internal void ChangeLoanToSecond()
    {
      string fieldAt1 = this.GetFieldAt("1109");
      string fieldAt2 = this.GetFieldAt("3");
      string fieldAt3 = this.GetFieldAt("4");
      this.SetFieldAt("420", "SecondLien");
      this.SetFieldAt("427", fieldAt1);
      this.SetFieldAt("428", this.GetFieldAt("LOANAMT2"));
      this.SetFieldAt("1109", this.GetFieldAt("LOANAMT2"));
      this.SetFieldAt("3", this.GetFieldAt("INTRATE2"));
      this.SetFieldAt("KBYO.XD3", Utils.RemoveEndingZeros(this.GetFieldAt("3")));
      this.SetFieldAt("4", this.GetFieldAt("TERM2"));
      this.SetFieldAt("LOANAMT1", fieldAt1);
      this.SetFieldAt("INTRATE1", fieldAt2);
      this.SetFieldAt("TERM1", fieldAt3);
      this.RemoveLockAt("140");
      this.SetFieldAt("140", string.Empty);
      this.SetFieldAt("1845", fieldAt1);
      this.SetFieldAt("1401", string.Empty);
      this.SetFieldAt("2861", string.Empty);
      this.SetFieldAt("1785", string.Empty);
      this.SetFieldAt("1014", string.Empty);
      string empty = string.Empty;
      for (int index = 1; index <= 16; ++index)
      {
        string str = "TA" + index.ToString("00");
        this.SetFieldAt(str + "DS", string.Empty);
        this.SetFieldAt(str + "NO", string.Empty);
        this.SetFieldAt(str + "DT", string.Empty);
        this.SetFieldAt(str + "PC", string.Empty);
        this.SetFieldAt(str + "PA", string.Empty);
        this.SetFieldAt(str + "RC", string.Empty);
        this.SetFieldAt(str + "RA", string.Empty);
      }
      this.SetFieldAt("TATOTAL1", string.Empty);
      this.SetFieldAt("TATOTAL2", string.Empty);
      this.SetFieldAt("TABALANCE", string.Empty);
      double num = this.toDouble(this.GetFieldAt("304"));
      if (num != 0.0)
        this.SetFieldAt("1851", (num * -1.0).ToString("N2"));
      this.RemoveLockAt("143");
      string fieldAt4 = this.GetFieldAt("610");
      string fieldAt5 = this.GetFieldAt("411");
      string fieldAt6 = this.GetFieldAt("56");
      string fieldAt7 = this.GetFieldAt("L252");
      string fieldAt8 = this.GetFieldAt("L248");
      string fieldAt9 = this.GetFieldAt("1500");
      string fieldAt10 = this.GetFieldAt("624");
      string fieldAt11 = this.GetFieldAt("REGZGFE.X8");
      string fieldAt12 = this.GetFieldAt("395");
      XmlNode oldChild = this.root.SelectSingleNode("EllieMae/CLOSING_COST[1]");
      if (oldChild != null)
        this.root.SelectSingleNode("EllieMae").RemoveChild(oldChild);
      this.SetFieldAt("610", fieldAt4);
      this.SetFieldAt("411", fieldAt5);
      this.SetFieldAt("56", fieldAt6);
      this.SetFieldAt("L252", fieldAt7);
      this.SetFieldAt("L248", fieldAt8);
      this.SetFieldAt("1500", fieldAt9);
      this.SetFieldAt("624", fieldAt10);
      this.SetFieldAt("REGZGFE.X8", fieldAt11);
      this.SetFieldAt("395", fieldAt12);
    }

    public void RemoveBorrowerPair(BorrowerPair pair)
    {
      string[] strArray1 = new string[2]
      {
        pair.Borrower.Id,
        pair.CoBorrower.Id
      };
      string[] strArray2 = new string[4]
      {
        "BORROWER",
        "ASSET",
        "LIABILITY",
        "REO_PROPERTY"
      };
      foreach (string str1 in strArray1)
      {
        string str2 = "[@BorrowerID=\"" + str1 + "\"]";
        foreach (string str3 in strArray2)
        {
          foreach (XmlNode selectNode in this.root.SelectNodes(str3 + str2))
            this.root.RemoveChild(selectNode);
        }
      }
      foreach (XmlNode selectNode in this.lockRoot.SelectNodes("FIELD" + this.brwPredicate))
        this.lockRoot.RemoveChild(selectNode);
    }

    internal bool RemoveCoborrowers(BorrowerPair[] pairs)
    {
      bool flag1 = false;
      for (int index = 0; index < pairs.Length; ++index)
      {
        this.SetBorrowerPair(pairs[index]);
        try
        {
          XmlNode xmlNode = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index].CoBorrower.Id + "\"]");
          XmlNodeList childNodes = xmlNode.ChildNodes;
          for (int i = childNodes.Count - 1; i >= 0; --i)
            xmlNode.RemoveChild(childNodes[i]);
          XmlElement xmlElement1 = (XmlElement) xmlNode;
          if (xmlElement1 != null)
          {
            if (xmlElement1.HasAttribute("_HomeTelephoneNumber"))
              xmlElement1.RemoveAttribute("_HomeTelephoneNumber");
            if (xmlElement1.HasAttribute("MaritalStatusType"))
              xmlElement1.RemoveAttribute("MaritalStatusType");
            if (xmlElement1.HasAttribute("DependentCount"))
              xmlElement1.RemoveAttribute("DependentCount");
            if (xmlElement1.HasAttribute("SchoolingYears"))
              xmlElement1.RemoveAttribute("SchoolingYears");
            if (xmlElement1.HasAttribute("_AgeAtApplicationYears"))
              xmlElement1.RemoveAttribute("_AgeAtApplicationYears");
            if (xmlElement1.HasAttribute("_FirstName"))
              xmlElement1.RemoveAttribute("_FirstName");
            if (xmlElement1.HasAttribute("_LastName"))
              xmlElement1.RemoveAttribute("_LastName");
            if (xmlElement1.HasAttribute("_SSN"))
              xmlElement1.SetAttribute("_SSN", "");
            if (xmlElement1.HasAttribute("_BorrowerType"))
              xmlElement1.RemoveAttribute("_BorrowerType");
          }
          this.SetFieldAt("1602", "");
          XmlNodeList xmlNodeList1 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + pairs[index].Borrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          if (xmlNodeList1 != null)
          {
            foreach (XmlElement xmlElement2 in xmlNodeList1)
            {
              if (xmlElement2 != null && xmlElement2.HasAttribute("Type") && xmlElement2.GetAttribute("Type") == "C")
              {
                xmlElement2.SetAttribute("Type", "");
                xmlElement2.SetAttribute("Description", "");
                xmlElement2.SetAttribute("Amount", "");
              }
            }
          }
          XmlNodeList xmlNodeList2 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + pairs[index].Borrower.Id + "\"]/EllieMae/PAIR/ATR_QM/VerificationTimeline/Timeline");
          if (xmlNodeList2 != null)
          {
            foreach (XmlNode oldChild in xmlNodeList2)
            {
              foreach (XmlNode selectNode in oldChild.SelectNodes("FIELD[@BorrowerType=\"Coborrower\"]"))
                oldChild.ParentNode.RemoveChild(oldChild);
            }
          }
          string empty = string.Empty;
          for (int numberOfDeposits = this.GetNumberOfDeposits(); numberOfDeposits > 0; --numberOfDeposits)
          {
            switch (this.GetFieldAt("FD" + numberOfDeposits.ToString("00") + "24"))
            {
              case "CoBorrower":
                this.RemoveDepositAt(numberOfDeposits - 1);
                break;
              case "Both":
                this.SetFieldAt("DD" + numberOfDeposits.ToString("00") + "24", "Borrower");
                break;
            }
          }
          for (int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp(); exlcudingAlimonyJobExp > 0; --exlcudingAlimonyJobExp)
          {
            switch (this.GetFieldAt("FL" + exlcudingAlimonyJobExp.ToString("00") + "15"))
            {
              case "CoBorrower":
                this.RemoveLiabilityAt(exlcudingAlimonyJobExp - 1);
                break;
              case "Both":
                this.SetFieldAt("FL" + exlcudingAlimonyJobExp.ToString("00") + "15", "Borrower");
                break;
            }
          }
          string xpath = "BORROWER[@BorrowerID=\"" + pairs[index].Borrower.Id + "\"]/EllieMae/DEPOSIT";
          for (XmlNodeList xmlNodeList3 = this.root.SelectNodes(xpath); xmlNodeList3 != null && xmlNodeList3.Count > 0; xmlNodeList3 = this.root.SelectNodes(xpath))
          {
            bool flag2 = false;
            foreach (XmlElement oldChild in xmlNodeList3)
            {
              if (oldChild.HasAttribute("Person") && string.Compare(oldChild.GetAttribute("Person"), "CoBorrower", true) == 0)
              {
                oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
                flag2 = true;
              }
            }
            if (!flag2)
              break;
          }
          this.addMissingFields();
          flag1 = true;
        }
        catch (Exception ex)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove coborrower from borrower pair '" + pairs[index].CoBorrower.Id + "', Error: " + ex.Message);
        }
        this.SetBorrowerPair(pairs[0]);
      }
      return flag1;
    }

    internal void SwapBorrowers(
      int sourcePair,
      bool sourceFromCoborrower,
      int targetPair,
      bool targetToCoborrower)
    {
      BorrowerPair[] borrowerPairs1 = this.GetBorrowerPairs();
      if (sourcePair == targetPair)
      {
        if (sourceFromCoborrower == targetToCoborrower)
          return;
        this.SwapBorrowers(new BorrowerPair[1]
        {
          borrowerPairs1[sourcePair - 1]
        });
      }
      else
      {
        string id = this.currentBorrowerPair.Id;
        bool targetBorIsNew = false;
        if (borrowerPairs1.Length < targetPair)
        {
          for (int index = borrowerPairs1.Length + 1; index <= targetPair; ++index)
            this.CreateBorrowerPair();
          borrowerPairs1 = this.GetBorrowerPairs();
          targetBorIsNew = true;
        }
        int index1 = sourcePair - 1;
        int index2 = targetPair - 1;
        string fieldAt1 = this.GetFieldAt("106", borrowerPairs1[index1]);
        string fieldAt2 = this.GetFieldAt("115", borrowerPairs1[index1]);
        XmlNode xmlNode1 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index1].Borrower.Id) + "\"]");
        XmlNode refChild1 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (sourceFromCoborrower ? borrowerPairs1[index1].Borrower.Id : borrowerPairs1[index1].CoBorrower.Id) + "\"]");
        XmlNode xmlNode2 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (targetToCoborrower ? borrowerPairs1[index2].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id) + "\"]");
        XmlNode refChild2 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (targetToCoborrower ? borrowerPairs1[index2].Borrower.Id : borrowerPairs1[index2].CoBorrower.Id) + "\"]");
        this.root.RemoveChild(xmlNode1);
        this.root.RemoveChild(xmlNode2);
        if (sourceFromCoborrower)
          this.root.InsertAfter(xmlNode2, refChild1);
        else
          this.root.InsertBefore(xmlNode2, refChild1);
        if (targetToCoborrower)
          this.root.InsertAfter(xmlNode1, refChild2);
        else
          this.root.InsertBefore(xmlNode1, refChild2);
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index1].Borrower.Id) + "\"]/EllieMae/PAIR");
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (targetToCoborrower ? borrowerPairs1[index2].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id) + "\"]/EllieMae/PAIR");
        xmlElement1?.ParentNode.RemoveChild((XmlNode) xmlElement1);
        xmlElement2?.ParentNode.RemoveChild((XmlNode) xmlElement2);
        if (xmlElement2 != null)
        {
          XmlElement xmlElement3 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index1].Borrower.Id) + "\"]/EllieMae");
          if (xmlElement3 == null && xmlElement3 == null)
            xmlElement3 = this.createPath("BORROWER[@BorrowerID=\"" + (sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index1].Borrower.Id) + "\"]/EllieMae");
          xmlElement3.AppendChild((XmlNode) xmlElement2);
        }
        if (xmlElement1 != null)
          ((XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (targetToCoborrower ? borrowerPairs1[index2].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id) + "\"]/EllieMae") ?? this.createPath("BORROWER[@BorrowerID=\"" + (targetToCoborrower ? borrowerPairs1[index2].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id) + "\"]/EllieMae"))?.AppendChild((XmlNode) xmlElement1);
        string empty1 = string.Empty;
        if (!sourceFromCoborrower & targetToCoborrower || sourceFromCoborrower && !targetToCoborrower)
          this.moveNodesBackToPrimaryBorrowerAfterSwap(sourceFromCoborrower ? borrowerPairs1[index2].Borrower.Id : borrowerPairs1[index1].Borrower.Id, sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id, "SUMMARY", "", false, false);
        if (!sourceFromCoborrower & targetToCoborrower)
          this.moveNodesBackToPrimaryBorrowerAfterSwap(borrowerPairs1[index1].Borrower.Id, borrowerPairs1[index2].CoBorrower.Id, "PRESENT_HOUSING_EXPENSE", "", false, false);
        else if (sourceFromCoborrower && !targetToCoborrower)
          this.moveNodesBackToPrimaryBorrowerAfterSwap(borrowerPairs1[index2].Borrower.Id, borrowerPairs1[index1].CoBorrower.Id, "PRESENT_HOUSING_EXPENSE", "", false, false);
        if (!sourceFromCoborrower && !targetToCoborrower || sourceFromCoborrower & targetToCoborrower)
        {
          XmlNodeList xmlNodeList1 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/DEPOSIT");
          XmlNodeList xmlNodeList2 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/DEPOSIT");
          XmlElement xmlElement4 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae") ?? this.createPath("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae");
          XmlElement xmlElement5 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae") ?? this.createPath("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae");
          List<string> stringList = new List<string>();
          if (xmlNodeList1 != null)
          {
            foreach (XmlElement newChild in xmlNodeList1)
            {
              if (newChild.HasAttribute("Person") && (string.Compare(newChild.GetAttribute("Person"), "CoBorrower", true) == 0 || string.Compare(newChild.GetAttribute("Person"), "Both", true) == 0))
              {
                xmlElement5.AppendChild((XmlNode) newChild);
                if (newChild.HasAttribute("ID") && !stringList.Contains(newChild.GetAttribute("ID")))
                  stringList.Add(newChild.GetAttribute("ID"));
              }
            }
          }
          if (xmlNodeList2 != null)
          {
            foreach (XmlElement newChild in xmlNodeList2)
            {
              if (newChild.HasAttribute("Person") && (!newChild.HasAttribute("ID") || !stringList.Contains(newChild.GetAttribute("ID"))) && (string.Compare(newChild.GetAttribute("Person"), "CoBorrower", true) == 0 || string.Compare(newChild.GetAttribute("Person"), "Both", true) == 0))
                xmlElement4.AppendChild((XmlNode) newChild);
            }
          }
          XmlNodeList xmlNodeList3 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          XmlNodeList xmlNodeList4 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          string str1 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/PAIR/TSUM";
          XmlElement refChild3 = (XmlElement) this.root.SelectSingleNode(str1) ?? this.createPath(str1);
          string str2 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/PAIR/TSUM";
          XmlElement refChild4 = (XmlElement) this.root.SelectSingleNode(str2) ?? this.createPath(str2);
          int count = xmlNodeList4 != null ? xmlNodeList4.Count : 0;
          if (xmlNodeList3 != null)
          {
            foreach (XmlNode newChild in xmlNodeList3)
            {
              XmlElement xmlElement6 = (XmlElement) newChild;
              if (xmlElement6.HasAttribute("Type") && string.Compare(xmlElement6.GetAttribute("Type"), sourceFromCoborrower || targetToCoborrower ? "C" : "B", true) == 0)
                refChild4.ParentNode.InsertAfter(newChild, (XmlNode) refChild4);
            }
          }
          if (xmlNodeList4 != null)
          {
            for (int i = 0; i < count; ++i)
            {
              XmlElement newChild = (XmlElement) xmlNodeList4[i];
              if (newChild.HasAttribute("Type") && string.Compare(newChild.GetAttribute("Type"), sourceFromCoborrower || targetToCoborrower ? "C" : "B", true) == 0)
                refChild3.ParentNode.InsertAfter((XmlNode) newChild, (XmlNode) refChild3);
            }
          }
        }
        else if (!sourceFromCoborrower & targetToCoborrower)
        {
          XmlNodeList xmlNodeList5 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/DEPOSIT");
          XmlNodeList xmlNodeList6 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/DEPOSIT");
          string str3 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].CoBorrower.Id + "\"]/EllieMae";
          XmlElement xmlElement7 = (XmlElement) this.root.SelectSingleNode(str3) ?? this.createPath(str3);
          string str4 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae";
          XmlElement xmlElement8 = (XmlElement) this.root.SelectSingleNode(str4) ?? this.createPath(str4);
          if (xmlNodeList6 != null)
          {
            foreach (XmlElement newChild in xmlNodeList6)
            {
              if (newChild.HasAttribute("Person") && string.Compare(newChild.GetAttribute("Person"), "CoBorrower", true) == 0)
              {
                newChild.SetAttribute("Person", "Borrower");
                xmlElement7.AppendChild((XmlNode) newChild);
              }
            }
          }
          if (xmlNodeList5 != null)
          {
            foreach (XmlNode newChild in xmlNodeList5)
            {
              XmlElement xmlElement9 = (XmlElement) newChild;
              if (xmlElement9.HasAttribute("Person"))
              {
                if (string.Compare(xmlElement9.GetAttribute("Person"), "CoBorrower", true) == 0 || string.Compare(xmlElement9.GetAttribute("Person"), "Both", true) == 0)
                  xmlElement7.AppendChild(newChild);
                else if (string.Compare(xmlElement9.GetAttribute("Person"), "Borrower", true) == 0)
                {
                  xmlElement9.SetAttribute("Person", "CoBorrower");
                  xmlElement8.AppendChild(newChild);
                }
              }
            }
          }
          XmlNodeList xmlNodeList7 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].CoBorrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          XmlNodeList xmlNodeList8 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          string str5 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].CoBorrower.Id + "\"]/EllieMae/PAIR/TSUM";
          XmlElement refChild5 = (XmlElement) this.root.SelectSingleNode(str5) ?? this.createPath(str5);
          string str6 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/PAIR/TSUM";
          XmlElement refChild6 = (XmlElement) this.root.SelectSingleNode(str6) ?? this.createPath(str6);
          int count = xmlNodeList8 != null ? xmlNodeList8.Count : 0;
          if (xmlNodeList7 != null)
          {
            foreach (XmlElement newChild in xmlNodeList7)
            {
              if (newChild.HasAttribute("Type") && string.Compare(newChild.GetAttribute("Type"), "B", true) == 0)
              {
                newChild.SetAttribute("Type", "C");
                refChild6.ParentNode.InsertAfter((XmlNode) newChild, (XmlNode) refChild6);
              }
            }
          }
          if (xmlNodeList8 != null)
          {
            for (int i = 0; i < count; ++i)
            {
              XmlElement newChild = (XmlElement) xmlNodeList8[i];
              if (newChild.HasAttribute("Type") && string.Compare(newChild.GetAttribute("Type"), "C", true) == 0)
              {
                newChild.SetAttribute("Type", "B");
                refChild5.ParentNode.InsertAfter((XmlNode) newChild, (XmlNode) refChild5);
              }
            }
          }
        }
        else if (sourceFromCoborrower && !targetToCoborrower)
        {
          XmlNodeList xmlNodeList9 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/DEPOSIT");
          XmlNodeList xmlNodeList10 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae/DEPOSIT");
          string str7 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae";
          XmlElement xmlElement10 = (XmlElement) this.root.SelectSingleNode(str7) ?? this.createPath(str7);
          string str8 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].CoBorrower.Id + "\"]/EllieMae";
          XmlElement xmlElement11 = (XmlElement) this.root.SelectSingleNode(str8) ?? this.createPath(str8);
          if (xmlNodeList9 != null)
          {
            foreach (XmlElement newChild in xmlNodeList9)
            {
              if (newChild.HasAttribute("Person") && string.Compare(newChild.GetAttribute("Person"), "CoBorrower", true) == 0)
              {
                newChild.SetAttribute("Person", "Borrower");
                xmlElement11.AppendChild((XmlNode) newChild);
              }
            }
          }
          if (xmlNodeList10 != null)
          {
            foreach (XmlNode newChild in xmlNodeList10)
            {
              XmlElement xmlElement12 = (XmlElement) newChild;
              if (xmlElement12.HasAttribute("Person"))
              {
                if (string.Compare(xmlElement12.GetAttribute("Person"), "CoBorrower", true) == 0 || string.Compare(xmlElement12.GetAttribute("Person"), "Both", true) == 0)
                  xmlElement11.AppendChild(newChild);
                else if (string.Compare(xmlElement12.GetAttribute("Person"), "Borrower", true) == 0)
                {
                  xmlElement12.SetAttribute("Person", "CoBorrower");
                  xmlElement10.AppendChild(newChild);
                }
              }
            }
          }
          XmlNodeList xmlNodeList11 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].CoBorrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          XmlNodeList xmlNodeList12 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME");
          string str9 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].CoBorrower.Id + "\"]/EllieMae/PAIR/TSUM";
          XmlElement refChild7 = (XmlElement) this.root.SelectSingleNode(str9) ?? this.createPath(str9);
          string str10 = "BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae/PAIR/TSUM";
          XmlElement refChild8 = (XmlElement) this.root.SelectSingleNode(str10) ?? this.createPath(str10);
          int count = xmlNodeList12 != null ? xmlNodeList12.Count : 0;
          if (xmlNodeList11 != null)
          {
            foreach (XmlElement newChild in xmlNodeList11)
            {
              if (newChild.HasAttribute("Type") && string.Compare(newChild.GetAttribute("Type"), "B", true) == 0)
              {
                newChild.SetAttribute("Type", "C");
                refChild8.ParentNode.InsertAfter((XmlNode) newChild, (XmlNode) refChild8);
              }
            }
          }
          if (xmlNodeList12 != null)
          {
            for (int i = 0; i < count; ++i)
            {
              XmlElement newChild = (XmlElement) xmlNodeList12[i];
              if (newChild.HasAttribute("Type") && string.Compare(newChild.GetAttribute("Type"), "C", true) == 0)
              {
                newChild.SetAttribute("Type", "B");
                refChild7.ParentNode.InsertAfter((XmlNode) newChild, (XmlNode) refChild7);
              }
            }
          }
        }
        this.swapVOLIndicator("LIABILITY[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae", "LIABILITY[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae", "_ID", "Person", borrowerPairs1[index1].Borrower.Id, sourceFromCoborrower ? "CoBorrower" : "Borrower", borrowerPairs1[index2].Borrower.Id, targetToCoborrower ? "CoBorrower" : "Borrower");
        this.swapVOMIndicator(sourceFromCoborrower, borrowerPairs1[index1].Borrower.Id, targetToCoborrower, borrowerPairs1[index2].Borrower.Id);
        this.swapASSET1003Indicator(sourceFromCoborrower, borrowerPairs1[index1].Borrower.Id, targetToCoborrower, borrowerPairs1[index2].Borrower.Id);
        if (sourceFromCoborrower && !targetToCoborrower || !sourceFromCoborrower & targetToCoborrower)
        {
          if (sourceFromCoborrower)
          {
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].CoBorrower.Id + "\"]/_RESIDENCE/EllieMae", true);
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/_RESIDENCE/EllieMae", false);
          }
          else
          {
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/_RESIDENCE/EllieMae", false);
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].CoBorrower.Id + "\"]/_RESIDENCE/EllieMae", true);
          }
          if (sourceFromCoborrower)
          {
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].CoBorrower.Id + "\"]/EMPLOYER/EllieMae", true);
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EMPLOYER/EllieMae", false);
          }
          else
          {
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EMPLOYER/EllieMae", false);
            this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + borrowerPairs1[index2].CoBorrower.Id + "\"]/EMPLOYER/EllieMae", true);
          }
          if (sourceFromCoborrower)
          {
            this.swapIndicator("ASSET[@BorrowerID=\"" + borrowerPairs1[index1].CoBorrower.Id + "\"]/EllieMae", "Person");
            this.swapIndicator("ASSET[@BorrowerID=\"" + borrowerPairs1[index2].Borrower.Id + "\"]/EllieMae", "Person");
          }
          else
          {
            this.swapIndicator("ASSET[@BorrowerID=\"" + borrowerPairs1[index1].Borrower.Id + "\"]/EllieMae", "Person");
            this.swapIndicator("ASSET[@BorrowerID=\"" + borrowerPairs1[index2].CoBorrower.Id + "\"]/EllieMae", "Person");
          }
        }
        XmlElement xmlElement13 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index1].Borrower.Id) + "\"]");
        XmlElement xmlElement14 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + (targetToCoborrower ? borrowerPairs1[index2].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id) + "\"]");
        if (xmlElement13 != null)
        {
          xmlElement13.SetAttribute("BorrowerID", targetToCoborrower ? borrowerPairs1[index2].CoBorrower.Id : borrowerPairs1[index2].Borrower.Id);
          xmlElement13.SetAttribute("JointAssetBorrowerID", targetToCoborrower ? borrowerPairs1[index2].Borrower.Id : borrowerPairs1[index2].CoBorrower.Id);
          xmlElement13.SetAttribute("_PrintPositionType", targetToCoborrower ? "CoBorrower" : "Borrower");
        }
        if (xmlElement14 != null)
        {
          xmlElement14.SetAttribute("BorrowerID", sourceFromCoborrower ? borrowerPairs1[index1].CoBorrower.Id : borrowerPairs1[index1].Borrower.Id);
          xmlElement14.SetAttribute("JointAssetBorrowerID", sourceFromCoborrower ? borrowerPairs1[index1].Borrower.Id : borrowerPairs1[index1].CoBorrower.Id);
          xmlElement14.SetAttribute("_PrintPositionType", sourceFromCoborrower ? "CoBorrower" : "Borrower");
        }
        BorrowerPair[] borrowerPairs2 = this.GetBorrowerPairs();
        string empty2 = string.Empty;
        for (int index3 = 0; index3 < borrowerPairs2.Length; ++index3)
        {
          for (int index4 = 1; index4 <= 2; ++index4)
          {
            string str11 = index4 == 1 ? "IR" : "AR";
            int numberOfTaX4506Ts = this.GetNumberOfTAX4506Ts(index4 != 1);
            for (int index5 = 1; index5 <= numberOfTaX4506Ts; ++index5)
            {
              string str12 = index5.ToString("00");
              string fieldAt3 = this.GetFieldAt("IR" + str12 + "01", borrowerPairs2[index3]);
              if (fieldAt3 == "Both" || fieldAt3 == "Borrower")
              {
                this.SetFieldAt(str11 + str12 + "02", this.GetFieldAt("36", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "03", this.GetFieldAt("37", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "04", this.GetFieldAt("65", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "39", this.GetFieldAt("36", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "40", this.GetFieldAt("37", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "35", this.GetFieldAt("FR0104", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "36", this.GetFieldAt("FR0106", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "37", this.GetFieldAt("FR0107", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "38", this.GetFieldAt("FR0108", borrowerPairs2[index3]), borrowerPairs2[index3]);
              }
              if (fieldAt3 == "CoBorrower")
              {
                this.SetFieldAt(str11 + str12 + "02", this.GetFieldAt("68", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "03", this.GetFieldAt("69", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "04", this.GetFieldAt("97", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "39", this.GetFieldAt("68", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "40", this.GetFieldAt("69", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "35", this.GetFieldAt("FR0204", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "36", this.GetFieldAt("FR0206", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "37", this.GetFieldAt("FR0207", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "38", this.GetFieldAt("FR0208", borrowerPairs2[index3]), borrowerPairs2[index3]);
              }
              if (fieldAt3 == "Both")
              {
                this.SetFieldAt(str11 + str12 + "06", this.GetFieldAt("68", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "07", this.GetFieldAt("69", borrowerPairs2[index3]), borrowerPairs2[index3]);
                this.SetFieldAt(str11 + str12 + "05", this.GetFieldAt("97", borrowerPairs2[index3]), borrowerPairs2[index3]);
              }
              else if (fieldAt3 != "Other")
              {
                this.SetFieldAt(str11 + str12 + "06", "");
                this.SetFieldAt(str11 + str12 + "07", "");
                this.SetFieldAt(str11 + str12 + "05", "");
              }
            }
          }
        }
        this.swapFORM1084(borrowerPairs2[index1].Borrower.Id, sourceFromCoborrower, borrowerPairs2[index2].Borrower.Id, targetToCoborrower, targetBorIsNew);
        string str13 = "";
        string str14 = "";
        string belongToNodeName = "Person";
        for (int index6 = 1; index6 <= 5; ++index6)
        {
          switch (index6)
          {
            case 1:
              str13 = "GiftsGrants/GiftGrant";
              str14 = "GiftsGrants";
              break;
            case 2:
              str13 = "OtherIncomeSources/OtherIncomeSource";
              str14 = "OtherIncomeSources";
              break;
            case 3:
              str13 = "OtherLiabilities/OTHER_LIABILITY";
              str14 = "OtherLiabilities";
              belongToNodeName = "_BorrowerType";
              break;
            case 4:
              str13 = "OtherAssets/OTHER_ASSET";
              str14 = "OtherAssets";
              belongToNodeName = "_BorrowerType";
              break;
            case 5:
              str13 = "AdditionalLoans/Additional_Loan";
              str14 = "AdditionalLoans";
              belongToNodeName = "_BorrowerType";
              break;
          }
          if (sourceFromCoborrower & targetToCoborrower || !sourceFromCoborrower && !targetToCoborrower)
          {
            XmlNodeList sourceNodeList = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs2[index1].Borrower.Id + "\"]/URLA2020/" + str13);
            XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs2[index2].Borrower.Id + "\"]/URLA2020/" + str13);
            List<string> nodeEIDs = this.loadURLA2020VerifEIDs(xmlNodeList);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList, (List<string>) null, borrowerPairs2[index2].Borrower.Id, "URLA2020/" + str14, "CoBorrower", belongToNodeName, false);
            this.moveURLA2020VerifsFromPairToPair(xmlNodeList, nodeEIDs, borrowerPairs2[index1].Borrower.Id, "URLA2020/" + str14, "CoBorrower", belongToNodeName, false);
          }
          else if (!sourceFromCoborrower & targetToCoborrower)
          {
            XmlNodeList sourceNodeList1 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs2[index2].CoBorrower.Id + "\"]/URLA2020/" + str13);
            XmlNodeList sourceNodeList2 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs2[index2].Borrower.Id + "\"]/URLA2020/" + str13);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList1, (List<string>) null, borrowerPairs2[index1].Borrower.Id, "URLA2020/" + str14, "CoBorrower", belongToNodeName, false);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList1, (List<string>) null, borrowerPairs2[index1].Borrower.Id, "URLA2020/" + str14, "Both", belongToNodeName, false);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList2, (List<string>) null, borrowerPairs2[index1].Borrower.Id, "URLA2020/" + str14, "CoBorrower", belongToNodeName, true);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList1, (List<string>) null, borrowerPairs2[index2].Borrower.Id, "URLA2020/" + str14, "Borrower", belongToNodeName, true);
          }
          else if (sourceFromCoborrower && !targetToCoborrower)
          {
            XmlNodeList sourceNodeList3 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs2[index1].CoBorrower.Id + "\"]/URLA2020/" + str13);
            XmlNodeList sourceNodeList4 = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + borrowerPairs2[index1].Borrower.Id + "\"]/URLA2020/" + str13);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList3, (List<string>) null, borrowerPairs2[index2].Borrower.Id, "URLA2020/" + str14, "CoBorrower", belongToNodeName, false);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList3, (List<string>) null, borrowerPairs2[index2].Borrower.Id, "URLA2020/" + str14, "Both", belongToNodeName, false);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList4, (List<string>) null, borrowerPairs2[index2].Borrower.Id, "URLA2020/" + str14, "CoBorrower", belongToNodeName, true);
            this.moveURLA2020VerifsFromPairToPair(sourceNodeList3, (List<string>) null, borrowerPairs2[index1].Borrower.Id, "URLA2020/" + str14, "Borrower", belongToNodeName, true);
          }
          XmlElement oldChild1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs2[index1].CoBorrower.Id + "\"]/URLA2020/" + str14);
          oldChild1?.ParentNode.RemoveChild((XmlNode) oldChild1);
          XmlElement oldChild2 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs2[index2].CoBorrower.Id + "\"]/URLA2020/" + str14);
          oldChild2?.ParentNode.RemoveChild((XmlNode) oldChild2);
        }
        if (!sourceFromCoborrower & targetToCoborrower || sourceFromCoborrower && !targetToCoborrower)
        {
          string[] strArray1 = new string[12]
          {
            "OtherTotalIncome",
            "TotalAdditionalAssetsAmount",
            "TotalURLA2020AssetsAmount",
            "TotalAdditionalOtherAssetsAmount",
            "TotalOtherAssetsAmount",
            "TotalAdditionalLiabilitiesAmount",
            "TotalLiabilitiesAmount",
            "TotalAdditionalOtherLiabilitiesAmount",
            "TotalOtherLiabilitiesAmount",
            "TotalAdditionalLoansAmount",
            "TotalAppliedToDownpayment",
            "PresentSupplementalPropertyInsuranceAmount"
          };
          for (int index7 = 0; index7 < strArray1.Length; ++index7)
          {
            if (!sourceFromCoborrower & targetToCoborrower)
              this.moveNodesBackToPrimaryBorrowerAfterSwap(borrowerPairs2[index2].CoBorrower.Id, borrowerPairs2[index1].Borrower.Id, "URLA2020/@" + strArray1[index7], "URLA2020", true, false);
            else
              this.moveNodesBackToPrimaryBorrowerAfterSwap(borrowerPairs2[index1].CoBorrower.Id, borrowerPairs2[index2].Borrower.Id, "URLA2020/@" + strArray1[index7], "URLA2020", true, false);
          }
          string[] strArray2 = new string[15]
          {
            "SectionAExplanation",
            "SectionBExplanation",
            "SectionCExplanation",
            "SectionDExplanation",
            "SectionAExplanation",
            "SectionD2Explanation",
            "SectionEExplanation",
            "SectionFExplanation",
            "SectionGExplanation",
            "SectionHExplanation",
            "SectionIExplanation",
            "SectionJExplanation",
            "SectionKExplanation",
            "SectionLExplanation",
            "SectionMExplanation"
          };
          for (int index8 = 0; index8 < strArray2.Length; ++index8)
          {
            if (!sourceFromCoborrower & targetToCoborrower)
              this.moveNodesBackToPrimaryBorrowerAfterSwap(borrowerPairs2[index2].CoBorrower.Id, borrowerPairs2[index1].Borrower.Id, "DECLARATION/@" + strArray2[index8], "DECLARATION", true, false);
            else
              this.moveNodesBackToPrimaryBorrowerAfterSwap(borrowerPairs2[index1].CoBorrower.Id, borrowerPairs2[index2].Borrower.Id, "DECLARATION/@" + strArray2[index8], "DECLARATION", true, false);
          }
        }
        this.SetFieldAt("106", fieldAt1, borrowerPairs2[0]);
        this.SetFieldAt("115", fieldAt2, borrowerPairs2[0]);
        try
        {
          this.SetBorrowerPair(borrowerPairs2[index1]);
          this.PopulateLatestSubmissionAusTracking();
          XmlElement oldChild3 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs2[index1].CoBorrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking/LatestSubmission");
          oldChild3?.ParentNode.RemoveChild((XmlNode) oldChild3);
          this.SetBorrowerPair(borrowerPairs2[index2]);
          this.PopulateLatestSubmissionAusTracking();
          XmlElement oldChild4 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borrowerPairs2[index2].CoBorrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking/LatestSubmission");
          oldChild4?.ParentNode.RemoveChild((XmlNode) oldChild4);
        }
        catch
        {
        }
        int index9 = 0;
        BorrowerPair[] borrowerPairs3 = this.GetBorrowerPairs();
        for (int index10 = 0; index10 < borrowerPairs3.Length; ++index10)
        {
          this.SetBorrowerPair(borrowerPairs3[index10]);
          this.addMissingFields();
          XmlElement xmlElement15 = (XmlElement) this.brwElm.SelectSingleNode("SUMMARY[@_AmountType=\"TotalPresentHousingExpense\"]");
          if (xmlElement15 != null && !xmlElement15.HasAttribute("_Amount"))
            xmlElement15.SetAttribute("_Amount", "");
          XmlElement xmlElement16 = (XmlElement) this.coBrwElm.SelectSingleNode("SUMMARY[@_AmountType=\"TotalPresentHousingExpense\"]");
          if (xmlElement16 != null && xmlElement16.HasAttribute("_Amount"))
            xmlElement16.RemoveAttribute("_Amount");
          if (borrowerPairs3[index10].Id == id)
            index9 = index10;
        }
        this.SetBorrowerPair(borrowerPairs3[index9]);
      }
    }

    private void moveURLA2020VerifsFromPairToPair(
      XmlNodeList sourceNodeList,
      List<string> nodeEIDs,
      string targetBorID,
      string verifNodeName,
      string belongTo,
      string belongToNodeName,
      bool changeBelongTo)
    {
      if (sourceNodeList == null || sourceNodeList.Count == 0)
        return;
      XmlElement xmlElement1 = (XmlElement) null;
      foreach (XmlNode sourceNode in sourceNodeList)
      {
        XmlElement xmlElement2 = (XmlElement) sourceNode;
        if (xmlElement2.HasAttribute(belongToNodeName) && string.Compare(xmlElement2.GetAttribute(belongToNodeName), belongTo, true) == 0)
        {
          string attribute = xmlElement2.GetAttribute("_eid");
          if (nodeEIDs != null && !nodeEIDs.Contains(attribute))
            break;
          if (xmlElement1 == null)
          {
            string str = "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/" + verifNodeName;
            xmlElement1 = (XmlElement) this.root.SelectSingleNode(str) ?? this.createPath(str);
          }
          if (changeBelongTo)
            xmlElement2.SetAttribute(belongToNodeName, xmlElement2.GetAttribute(belongToNodeName) == "Borrower" ? "CoBorrower" : "Borrower");
          xmlElement1.AppendChild(sourceNode);
        }
      }
    }

    private List<string> loadURLA2020VerifEIDs(XmlNodeList nodeList)
    {
      if (nodeList == null || nodeList.Count == 0)
        return (List<string>) null;
      List<string> stringList = new List<string>();
      foreach (XmlElement node in nodeList)
      {
        if (node.HasAttribute("_eid"))
          stringList.Add(node.GetAttribute("_eid"));
      }
      return stringList;
    }

    private void swapFORM1084(
      string sourceBorID,
      bool sourceFromCoborrower,
      string targetBorID,
      bool targetToCoborrower,
      bool targetBorIsNew)
    {
      string[] strArray1 = new string[6]
      {
        "FORM_1040",
        "FORM_2106",
        "FORM_6252",
        "FORM_1065",
        "FORM_1120S",
        "FORM_1120"
      };
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      for (int index = 0; index < strArray1.Length; ++index)
      {
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/" + strArray1[index]);
        string str = "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/" + strArray1[index];
        if (xmlElement1 != null && xmlElement1.ChildNodes != null && xmlElement1.ChildNodes.Count > 0)
        {
          foreach (XmlElement childNode in xmlElement1.ChildNodes)
          {
            string attribute1 = childNode.GetAttribute(sourceFromCoborrower ? "Year2" : "Year1");
            XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode(str + "/" + childNode.Name) ?? this.createPath(str + "/" + childNode.Name);
            if (xmlElement2 != null)
            {
              string attribute2 = xmlElement2.GetAttribute(targetToCoborrower ? "Year2" : "Year1");
              xmlElement2.SetAttribute(targetToCoborrower ? "Year2" : "Year1", attribute1);
              childNode.SetAttribute(sourceFromCoborrower ? "Year2" : "Year1", attribute2);
            }
          }
        }
      }
      for (int index = 0; index < strArray1.Length; ++index)
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/" + strArray1[index]);
        string str = "BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/" + strArray1[index];
        if (xmlElement != null && xmlElement.ChildNodes != null && xmlElement.ChildNodes.Count > 0)
        {
          foreach (XmlElement childNode in xmlElement.ChildNodes)
          {
            string attribute3 = childNode.GetAttribute(targetToCoborrower ? "Year2" : "Year1");
            if ((XmlElement) this.root.SelectSingleNode(str + "/" + childNode.Name) == null)
            {
              XmlElement path = this.createPath(str + "/" + childNode.Name);
              if (path != null)
              {
                string attribute4 = path.GetAttribute(sourceFromCoborrower ? "Year2" : "Year1");
                path.SetAttribute(sourceFromCoborrower ? "Year2" : "Year1", attribute3);
                childNode.SetAttribute(targetToCoborrower ? "Year2" : "Year1", attribute4);
              }
            }
          }
        }
      }
      string sourcePath1 = "BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME";
      string targetPath1 = "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME";
      this.swapAttribute(sourcePath1, sourceFromCoborrower ? "Year2_FormA" : "Year1_FormA", targetPath1, targetToCoborrower ? "Year2_FormA" : "Year1_FormA");
      this.swapAttribute(sourcePath1, sourceFromCoborrower ? "Year2_FormB" : "Year1_FormB", targetPath1, targetToCoborrower ? "Year2_FormB" : "Year1_FormB");
      string[] strArray2 = new string[4]
      {
        "TotalFor1040",
        "Totals",
        "GrandTotal",
        "RecurringCapitalGains_Form4797"
      };
      for (int index = 0; index < strArray2.Length; ++index)
      {
        string sourcePath2 = "BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/" + strArray2[index];
        string targetPath2 = "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/" + strArray2[index];
        this.swapAttribute(sourcePath2, sourceFromCoborrower ? "Year2" : "Year1", targetPath2, targetToCoborrower ? "Year2" : "Year1");
      }
      if (!targetBorIsNew)
        return;
      this.copySingleAttribute("BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1040", "OtherDescription", "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1040", "OtherDescription", true);
      this.copySingleAttribute("BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1065", "OwnershipPercent", "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1065", "OwnershipPercent", true);
      this.copySingleAttribute("BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1120S", "OwnershipPercent", "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1120S", "OwnershipPercent", true);
      this.copySingleAttribute("BORROWER[@BorrowerID=\"" + sourceBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1120", "OwnershipPercent", "BORROWER[@BorrowerID=\"" + targetBorID + "\"]/EllieMae/PAIR/SELF_EMPLOYED_INCOME/FORM_1120", "OwnershipPercent", true);
    }

    private void swapAttribute(
      string sourcePath,
      string sourceAttribute,
      string targetPath,
      string targetAttribute)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode(sourcePath);
      if (xmlElement1 != null)
        str1 = xmlElement1.GetAttribute(sourceAttribute);
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode(targetPath);
      if (xmlElement2 != null)
        str2 = xmlElement2.GetAttribute(targetAttribute);
      if (str1 != "" && xmlElement2 == null)
        xmlElement2 = this.createPath(targetPath);
      xmlElement2?.SetAttribute(targetAttribute, str1);
      if (str2 != "" && xmlElement1 == null)
        xmlElement1 = this.createPath(sourcePath);
      xmlElement1?.SetAttribute(sourceAttribute, str2);
    }

    internal void SwapBorrowers(BorrowerPair[] pairs)
    {
      string empty1 = string.Empty;
      for (int index1 = 0; index1 < pairs.Length; ++index1)
      {
        string id = pairs[index1].Id;
        string fieldAt1 = this.GetFieldAt("106", pairs[index1]);
        string fieldAt2 = this.GetFieldAt("115", pairs[index1]);
        XmlNode refChild = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]");
        XmlNode xmlNode1 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]");
        this.root.RemoveChild(xmlNode1);
        this.root.InsertBefore(xmlNode1, refChild);
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]");
        XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]");
        if (xmlElement1 != null)
        {
          xmlElement1.SetAttribute("BorrowerID", pairs[index1].Borrower.Id);
          xmlElement1.SetAttribute("JointAssetBorrowerID", pairs[index1].CoBorrower.Id);
          xmlElement1.SetAttribute("_PrintPositionType", "Borrower");
        }
        if (xmlElement2 != null)
        {
          xmlElement2.SetAttribute("BorrowerID", pairs[index1].CoBorrower.Id);
          xmlElement2.SetAttribute("JointAssetBorrowerID", pairs[index1].Borrower.Id);
          xmlElement2.SetAttribute("_PrintPositionType", "CoBorrower");
        }
        XmlNode xmlNode2 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]/EllieMae/PAIR");
        XmlNode xmlNode3 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/PAIR");
        if (xmlNode2 != null)
        {
          xmlNode2.ParentNode.RemoveChild(xmlNode2);
          ((XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae") ?? this.createPath("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae"))?.AppendChild(xmlNode2);
        }
        if (xmlNode3 != null)
        {
          xmlNode3.ParentNode.RemoveChild(xmlNode3);
          ((XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]/EllieMae") ?? this.createPath("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]/EllieMae"))?.AppendChild(xmlNode3);
        }
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "SUMMARY", "", false, false);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "PRESENT_HOUSING_EXPENSE", "", false, false);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "EllieMae/DEPOSIT", "EllieMae", false, false);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "URLA2020/GiftsGrants/GiftGrant", "URLA2020/GiftsGrants", true, true);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "URLA2020/OtherIncomeSources/OtherIncomeSource", "URLA2020/OtherIncomeSources", true, true);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "URLA2020/OtherLiabilities/OTHER_LIABILITY", "URLA2020/OtherLiabilities", true, true);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "URLA2020/OtherAssets/OTHER_ASSET", "URLA2020/OtherAssets", true, true);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "URLA2020/AdditionalLoans/Additional_Loan", "URLA2020/AdditionalLoans", true, true);
        this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "ProvidedDocuments/PROVIDED_DOCUMENT", "ProvidedDocuments", true, true);
        string[] strArray1 = new string[12]
        {
          "OtherTotalIncome",
          "TotalAdditionalAssetsAmount",
          "TotalURLA2020AssetsAmount",
          "TotalAdditionalOtherAssetsAmount",
          "TotalOtherAssetsAmount",
          "TotalAdditionalLiabilitiesAmount",
          "TotalLiabilitiesAmount",
          "TotalAdditionalOtherLiabilitiesAmount",
          "TotalOtherLiabilitiesAmount",
          "TotalAdditionalLoansAmount",
          "TotalAppliedToDownpayment",
          "PresentSupplementalPropertyInsuranceAmount"
        };
        foreach (string str in strArray1)
          this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "URLA2020/@" + str, "URLA2020", true, false);
        string[] strArray2 = new string[15]
        {
          "SectionAExplanation",
          "SectionBExplanation",
          "SectionCExplanation",
          "SectionDExplanation",
          "SectionAExplanation",
          "SectionD2Explanation",
          "SectionEExplanation",
          "SectionFExplanation",
          "SectionGExplanation",
          "SectionHExplanation",
          "SectionIExplanation",
          "SectionJExplanation",
          "SectionKExplanation",
          "SectionLExplanation",
          "SectionMExplanation"
        };
        foreach (string str in strArray2)
          this.moveNodesBackToPrimaryBorrowerAfterSwap(pairs[index1].CoBorrower.Id, pairs[index1].Borrower.Id, "DECLARATION/@" + str, "DECLARATION", true, false);
        XmlElement oldChild1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking/LatestSubmission");
        if (oldChild1 != null)
        {
          XmlElement oldChild2 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking/LatestSubmission");
          oldChild2?.ParentNode.RemoveChild((XmlNode) oldChild2);
          ((XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking") ?? this.createPath("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/ATR_QM/AUSTracking")).AppendChild(oldChild1.CloneNode(true));
          oldChild1.ParentNode.RemoveChild((XmlNode) oldChild1);
        }
        this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/_RESIDENCE/EllieMae", true);
        this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]/_RESIDENCE/EllieMae", false);
        this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EMPLOYER/EllieMae", true);
        this.swapBelongIndicator("BORROWER[@BorrowerID=\"" + pairs[index1].CoBorrower.Id + "\"]/EMPLOYER/EllieMae", false);
        this.swapIndicator("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/DEPOSIT", "Person");
        this.swapIndicator("LIABILITY[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae", "Person");
        this.swapIndicator("ASSET[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae", "Person");
        this.swapIndicator("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/PAIR/OTHER_INCOME", "Type");
        this.swapIndicator("EllieMae/FHA_VA_LOANS/VA_LOAN_SUMMARY", "VeteranType");
        this.swapIndicator("EllieMae/FHA_VA_LOANS/VA_VERIFICATION_OF_BENEFIT", "VeteranType");
        this.swapIndicator("EllieMae/FHA_VA_LOANS/VA_CERTIFICATE_ELIGIBILITY", "VeteranType");
        string empty2 = string.Empty;
        for (int index2 = 1; index2 <= 2; ++index2)
        {
          string str1 = index2 == 1 ? "IR" : "AR";
          int numberOfTaX4506Ts = this.GetNumberOfTAX4506Ts(index2 != 1);
          for (int index3 = 1; index3 <= numberOfTaX4506Ts; ++index3)
          {
            string str2 = index3.ToString("00");
            string fieldAt3 = this.GetFieldAt(str1 + str2 + "01", pairs[index1]);
            if (fieldAt3 == "Both" || fieldAt3 == "Borrower")
            {
              this.SetFieldAt(str1 + str2 + "02", this.GetFieldAt("36", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "03", this.GetFieldAt("37", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "04", this.GetFieldAt("65", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "39", this.GetFieldAt("36", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "40", this.GetFieldAt("37", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "35", this.GetFieldAt("FR0104", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "36", this.GetFieldAt("FR0106", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "37", this.GetFieldAt("FR0107", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "38", this.GetFieldAt("FR0108", pairs[index1]), pairs[index1]);
            }
            if (fieldAt3 == "CoBorrower")
            {
              this.SetFieldAt(str1 + str2 + "02", this.GetFieldAt("68", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "03", this.GetFieldAt("69", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "04", this.GetFieldAt("97", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "39", this.GetFieldAt("68", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "40", this.GetFieldAt("69", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "35", this.GetFieldAt("FR0204", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "36", this.GetFieldAt("FR0206", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "37", this.GetFieldAt("FR0207", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "38", this.GetFieldAt("FR0208", pairs[index1]), pairs[index1]);
            }
            if (fieldAt3 == "Both")
            {
              this.SetFieldAt(str1 + str2 + "06", this.GetFieldAt("68", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "07", this.GetFieldAt("69", pairs[index1]), pairs[index1]);
              this.SetFieldAt(str1 + str2 + "05", this.GetFieldAt("97", pairs[index1]), pairs[index1]);
            }
            else if (fieldAt3 != "Other")
            {
              this.SetFieldAt(str1 + str2 + "06", "");
              this.SetFieldAt(str1 + str2 + "07", "");
              this.SetFieldAt(str1 + str2 + "05", "");
            }
          }
        }
        this.SetFieldAt("106", fieldAt1, pairs[index1]);
        this.SetFieldAt("115", fieldAt2, pairs[index1]);
        string fieldAt4 = this.GetFieldAt("31");
        this.SetFieldAt("31", this.GetFieldAt("1602", pairs[index1]), pairs[index1]);
        this.SetFieldAt("1602", fieldAt4, pairs[index1]);
        this.swapFORM1084(pairs[index1].Id, true, pairs[index1].Id, false, false);
        XmlNodeList xmlNodeList = this.root.SelectNodes("BORROWER[@BorrowerID=\"" + pairs[index1].Borrower.Id + "\"]/EllieMae/PAIR/ATR_QM/VerificationTimeline/Timeline");
        if (xmlNodeList != null)
        {
          foreach (XmlNode xmlNode4 in xmlNodeList)
          {
            XmlNode xmlNode5 = xmlNode4.SelectSingleNode("FIELD");
            if (xmlNode5 != null)
            {
              XmlElement xmlElement3 = (XmlElement) xmlNode5;
              if (xmlElement3.HasAttribute("BorrowerType"))
              {
                if (xmlElement3.GetAttribute("BorrowerType") == "Borrower")
                  xmlElement3.SetAttribute("BorrowerType", "Coborrower");
                else
                  xmlElement3.SetAttribute("BorrowerType", "Borrower");
              }
            }
          }
        }
      }
      this.SetBorrowerPair(pairs[0]);
    }

    private void moveNodesBackToPrimaryBorrowerAfterSwap(
      string cobID,
      string borID,
      string sourcePath,
      string targetPath,
      bool removeSourceNode,
      bool swapBelongTo)
    {
      string xpath = "BORROWER[@BorrowerID=\"" + cobID + "\"]/" + sourcePath;
      if (sourcePath.IndexOf("/@") > -1)
      {
        int length = sourcePath.LastIndexOf("/@");
        if (length == -1)
          return;
        string name = sourcePath.Substring(length + 2);
        string str1 = sourcePath.Substring(0, length);
        if (name.Trim() == "" || str1.Trim() == "")
          return;
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + cobID + "\"]/" + str1);
        if (xmlElement == null || !xmlElement.HasAttribute(name))
          return;
        string attribute = xmlElement.GetAttribute(name);
        if (!(attribute != ""))
          return;
        xmlElement.RemoveAttribute(name);
        string str2 = "BORROWER[@BorrowerID=\"" + borID + "\"]/" + str1;
        ((XmlElement) this.root.SelectSingleNode(str2) ?? this.createPath(str2)).SetAttribute(name, attribute);
      }
      else
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(xpath);
        if (xmlNodeList == null || xmlNodeList.Count <= 0)
          return;
        XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + borID + "\"]" + (targetPath != "" ? "/" + targetPath : "")) ?? this.createPath("BORROWER[@BorrowerID=\"" + borID + "\"]" + (targetPath != "" ? "/" + targetPath : ""));
        if (xmlElement1 != null)
        {
          foreach (XmlNode newChild in xmlNodeList)
          {
            if (sourcePath == "SUMMARY")
            {
              XmlElement xmlElement2 = (XmlElement) newChild;
              if (xmlElement2 != null && xmlElement2.HasAttribute("_AmountType") && xmlElement2.GetAttribute("_AmountType") == "TotalMonthlyIncomeNotIncludingNetRentalIncome")
                continue;
            }
            xmlElement1.AppendChild(newChild);
          }
        }
        if (removeSourceNode)
        {
          XmlElement oldChild = (XmlElement) this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + cobID + "\"]" + (targetPath != "" ? "/" + targetPath : ""));
          oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
        }
        if (!swapBelongTo)
          return;
        string nodeID = "Person";
        if (sourcePath == "URLA2020/OtherLiabilities/OTHER_LIABILITY" || sourcePath == "URLA2020/OtherAssets/OTHER_ASSET" || sourcePath == "URLA2020/AdditionalLoans/Additional_Loan" || sourcePath == "ProvidedDocuments/PROVIDED_DOCUMENT")
          nodeID = "_BorrowerType";
        this.swapIndicator("BORROWER[@BorrowerID=\"" + borID + "\"]/" + sourcePath, nodeID);
      }
    }

    private void swapBorrowerNodes(string upperXPath, string node, BorrowerPair pair)
    {
      string xpath1 = upperXPath.Replace("BORID", pair.Borrower.Id);
      XmlNodeList xmlNodeList1 = this.root.SelectNodes(xpath1 + node);
      XmlNode xmlNode1 = this.root.SelectSingleNode(xpath1);
      string xpath2 = upperXPath.Replace("BORID", pair.CoBorrower.Id);
      XmlNodeList xmlNodeList2 = this.root.SelectNodes(xpath2 + node);
      XmlNode xmlNode2 = this.root.SelectSingleNode(xpath2);
      if (xmlNode1 != null && xmlNodeList1 != null)
      {
        for (int i = xmlNodeList1.Count - 1; i >= 0; --i)
          xmlNode1.RemoveChild(xmlNodeList1[i]);
      }
      if (xmlNode1 != null && xmlNodeList2 != null)
      {
        for (int i = 0; i < xmlNodeList2.Count; ++i)
          xmlNode1.AppendChild(xmlNodeList2[i]);
      }
      if (xmlNode2 == null || xmlNodeList1 == null)
        return;
      for (int i = 0; i < xmlNodeList1.Count; ++i)
        xmlNode2.AppendChild(xmlNodeList1[i]);
    }

    private void swapBelongIndicator(string nodePath, bool isBorrower)
    {
      string str1 = "Borrower";
      string str2 = "CoBorrower";
      if (isBorrower)
      {
        str2 = "Borrower";
        str1 = "CoBorrower";
      }
      XmlNodeList xmlNodeList = this.root.SelectNodes(nodePath);
      if (xmlNodeList == null)
        return;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        if (xmlElement != null && xmlElement.HasAttribute("Person") && xmlElement.GetAttribute("Person") == str1)
          xmlElement.SetAttribute("Person", str2);
      }
    }

    internal int NewOtherAsset()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.otherAssetPath);
      this.createOtherAsset("URLAROA" + (xmlNodeList.Count + 1).ToString("00") + "00", (BorrowerPair) null);
      return xmlNodeList.Count;
    }

    internal int GetNumberOfOtherAssets() => this.root.SelectNodes(this.otherAssetPath).Count;

    private void swapVOLIndicator(
      string sourceNodePath,
      string targetNodePath,
      string nodeIDName,
      string belongID,
      string oldBorID,
      string oldIndicatorValue,
      string newBorID,
      string newIndicatorValue)
    {
      XmlNodeList xmlNodeList1 = this.root.SelectNodes(sourceNodePath);
      XmlNodeList xmlNodeList2 = this.root.SelectNodes(targetNodePath);
      if ((xmlNodeList1 == null || xmlNodeList1.Count == 0) && (xmlNodeList2 == null || xmlNodeList2.Count == 0))
        return;
      XmlNodeList xmlNodeList3 = this.root.SelectNodes("REO_PROPERTY[@BorrowerID=\"" + oldBorID + "\"]");
      XmlNodeList xmlNodeList4 = this.root.SelectNodes("REO_PROPERTY[@BorrowerID=\"" + newBorID + "\"]");
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (XmlElement xmlElement1 in xmlNodeList1)
      {
        if (xmlElement1 != null && xmlElement1.HasAttribute(belongID) && string.Compare(xmlElement1.GetAttribute(belongID), oldIndicatorValue, true) == 0)
        {
          xmlElement1.SetAttribute(belongID, newIndicatorValue);
          XmlElement parentNode = (XmlElement) xmlElement1.ParentNode;
          if (parentNode != null && parentNode.HasAttribute(nodeIDName))
            stringList1.Add(parentNode.GetAttribute(nodeIDName));
          if (parentNode != null && parentNode.HasAttribute("BorrowerID") && string.Compare(parentNode.GetAttribute("BorrowerID"), oldBorID, true) == 0)
          {
            parentNode.SetAttribute("BorrowerID", newBorID);
            if (parentNode.HasAttribute("REO_ID"))
            {
              string attribute = parentNode.GetAttribute("REO_ID");
              if (!(attribute == string.Empty) && !stringList2.Contains(attribute))
              {
                foreach (XmlElement xmlElement2 in xmlNodeList3)
                {
                  if (xmlElement2 != null && string.Compare(attribute, xmlElement2.GetAttribute("REO_ID"), true) == 0 && xmlElement2.HasAttribute("BorrowerID") && string.Compare(xmlElement2.GetAttribute("BorrowerID"), oldBorID, true) == 0)
                  {
                    xmlElement2.SetAttribute("BorrowerID", newBorID);
                    stringList2.Add(attribute);
                    break;
                  }
                }
              }
            }
          }
        }
      }
      foreach (XmlElement xmlElement3 in xmlNodeList2)
      {
        if (xmlElement3 != null && xmlElement3.HasAttribute(belongID))
        {
          XmlElement parentNode = (XmlElement) xmlElement3.ParentNode;
          if ((parentNode == null || !parentNode.HasAttribute(nodeIDName) || !stringList1.Contains(parentNode.GetAttribute(nodeIDName))) && string.Compare(xmlElement3.GetAttribute(belongID), newIndicatorValue, true) == 0)
          {
            xmlElement3.SetAttribute(belongID, oldIndicatorValue);
            if (parentNode != null && parentNode.HasAttribute("BorrowerID") && string.Compare(parentNode.GetAttribute("BorrowerID"), newBorID, true) == 0)
            {
              parentNode.SetAttribute("BorrowerID", oldBorID);
              if (parentNode.HasAttribute("REO_ID"))
              {
                string attribute = parentNode.GetAttribute("REO_ID");
                if (!(attribute == string.Empty) && !stringList2.Contains(attribute))
                {
                  foreach (XmlElement xmlElement4 in xmlNodeList4)
                  {
                    if (xmlElement4 != null && string.Compare(attribute, xmlElement4.GetAttribute("REO_ID"), true) == 0 && xmlElement4.HasAttribute("BorrowerID") && string.Compare(xmlElement4.GetAttribute("BorrowerID"), newBorID, true) == 0)
                    {
                      xmlElement4.SetAttribute("BorrowerID", oldBorID);
                      stringList2.Add(attribute);
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }
    }

    private void swapVOMIndicator(
      bool sourceFromCoborrower,
      string oldBorID,
      bool targetToCoborrower,
      string newBorID)
    {
      if (sourceFromCoborrower & targetToCoborrower)
        return;
      string empty1 = string.Empty;
      List<string> stringList = new List<string>();
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("LIABILITY");
      if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
      {
        foreach (XmlElement xmlElement in xmlNodeList1)
        {
          if (xmlElement.HasAttribute("REO_ID"))
          {
            string attribute = xmlElement.GetAttribute("REO_ID");
            if (!(attribute == string.Empty) && !stringList.Contains(attribute))
              stringList.Add(attribute);
          }
        }
      }
      string empty2 = string.Empty;
      XmlNodeList xmlNodeList2 = this.root.SelectNodes("REO_PROPERTY");
      if (xmlNodeList2 == null || xmlNodeList2.Count <= 0)
        return;
      foreach (XmlElement xmlElement in xmlNodeList2)
      {
        if (xmlElement.HasAttribute("REO_ID"))
        {
          string attribute1 = xmlElement.GetAttribute("REO_ID");
          if (!(attribute1 == string.Empty) && !stringList.Contains(attribute1) && xmlElement.HasAttribute("BorrowerID"))
          {
            string attribute2 = xmlElement.GetAttribute("BorrowerID");
            if (!sourceFromCoborrower && string.Compare(attribute2, oldBorID, true) == 0)
              xmlElement.SetAttribute("BorrowerID", newBorID);
            else if (!targetToCoborrower && string.Compare(attribute2, newBorID, true) == 0)
              xmlElement.SetAttribute("BorrowerID", oldBorID);
          }
        }
      }
    }

    private void swapASSET1003Indicator(
      bool sourceFromCoborrower,
      string oldBorID,
      bool targetToCoborrower,
      string newBorID)
    {
      if (sourceFromCoborrower | targetToCoborrower)
        return;
      string empty = string.Empty;
      XmlNodeList xmlNodeList = this.root.SelectNodes("ASSET");
      if (xmlNodeList == null || xmlNodeList.Count <= 0)
        return;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        if (xmlElement.HasAttribute("BorrowerID"))
        {
          string attribute = xmlElement.GetAttribute("BorrowerID");
          if (string.Compare(attribute, oldBorID, true) == 0)
            xmlElement.SetAttribute("BorrowerID", newBorID);
          else if (!targetToCoborrower && string.Compare(attribute, newBorID, true) == 0)
            xmlElement.SetAttribute("BorrowerID", oldBorID);
        }
      }
    }

    private void swapIndicator(string nodePath, string nodeID)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(nodePath);
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return;
      string str1 = "CoBorrower";
      string str2 = "Borrower";
      if (nodePath.EndsWith("EllieMae/PAIR/OTHER_INCOME"))
      {
        str1 = "C";
        str2 = "B";
      }
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        if (xmlElement != null && xmlElement.HasAttribute(nodeID))
        {
          string attribute = xmlElement.GetAttribute(nodeID);
          if (attribute == str1)
            xmlElement.SetAttribute(nodeID, str2);
          else if (attribute == str2)
            xmlElement.SetAttribute(nodeID, str1);
        }
      }
    }

    public void SwapBorrowerPairs(BorrowerPair[] pairs)
    {
      XmlNode xmlNode1 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[0].Borrower.Id + "\"]");
      XmlNode xmlNode2 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[0].CoBorrower.Id + "\"]");
      XmlNode xmlNode3 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[1].Borrower.Id + "\"]");
      XmlNode xmlNode4 = this.root.SelectSingleNode("BORROWER[@BorrowerID=\"" + pairs[1].CoBorrower.Id + "\"]");
      this.root.RemoveChild(xmlNode3);
      this.root.InsertBefore(xmlNode3, xmlNode1);
      this.root.RemoveChild(xmlNode1);
      this.root.InsertBefore(xmlNode1, xmlNode4);
      this.root.RemoveChild(xmlNode4);
      this.root.InsertAfter(xmlNode4, xmlNode3);
      this.root.RemoveChild(xmlNode2);
      this.root.InsertAfter(xmlNode2, xmlNode1);
    }

    private XmlElement createPath(string path)
    {
      string[] strArray = path.Split('/');
      string empty = string.Empty;
      string pathSoFar = string.Empty;
      int length = strArray.Length;
      int index = 0;
      XmlElement elm = this.root;
      while (index < length)
      {
        string xpath = strArray[index];
        XmlElement xmlElement = (XmlElement) elm.SelectSingleNode(xpath);
        if (xmlElement != null)
        {
          elm = xmlElement;
          ++index;
          pathSoFar = pathSoFar + xpath.Split('[')[0] + "/";
        }
        else
        {
          do
          {
            string path1 = strArray[index];
            elm = this.createAndAppendNewElements(elm, path1, pathSoFar);
            if (elm == null)
            {
              Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in createAndAppendNewElements(): " + path);
              return (XmlElement) null;
            }
            ++index;
            pathSoFar = pathSoFar + path1.Split('[')[0] + "/";
          }
          while (index < length);
        }
      }
      return elm;
    }

    private XmlElement createAndAppendNewElements(XmlElement elm, string path, string pathSoFar)
    {
      int num1 = path.IndexOf(']');
      XmlElement element;
      if (num1 != -1)
      {
        if (path.LastIndexOf(']') != num1)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot have multiple predicates in creating an element: " + path);
          return (XmlElement) null;
        }
        int length = path.IndexOf('[');
        string s = path.Substring(length + 1, num1 - length - 1);
        int num2;
        try
        {
          num2 = int.Parse(s);
        }
        catch (FormatException ex)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot create element (mapping is wrong or contains special predicate) path: " + path);
          return (XmlElement) null;
        }
        string str = path.Substring(0, length);
        XmlNodeList xmlNodeList = elm.SelectNodes(str);
        int count = xmlNodeList.Count;
        element = this.xmldoc.CreateElement(str);
        if (((IEnumerable<string>) Mapping.XPathsForEntityIds).Contains<string>(pathSoFar + str))
          Mapping.AddEntityId(element);
        if (count == 0)
          elm.AppendChild((XmlNode) element);
        else
          elm.InsertAfter((XmlNode) element, xmlNodeList[count - 1]);
        for (int index = count + 1; index < num2; ++index)
        {
          XmlElement refChild = element;
          element = this.xmldoc.CreateElement(str);
          if (((IEnumerable<string>) Mapping.XPathsForEntityIds).Contains<string>(pathSoFar + str))
            Mapping.AddEntityId(element);
          elm.InsertAfter((XmlNode) element, (XmlNode) refChild);
        }
      }
      else
      {
        element = this.xmldoc.CreateElement(path);
        if (((IEnumerable<string>) Mapping.XPathsForEntityIds).Contains<string>(pathSoFar + path))
          Mapping.AddEntityId(element);
        elm.AppendChild((XmlNode) element);
      }
      return element;
    }

    private void createVOL(string id, BorrowerPair pair)
    {
      int num1 = int.Parse(id.Substring(2, 2));
      if (id.Length > 6)
        num1 = int.Parse(id.Substring(2, 3));
      string xpath;
      string str1;
      if (pair == null)
      {
        xpath = this.liabPath;
        str1 = this.brwId;
      }
      else
      {
        xpath = this.liabPath.Replace(this.brwId, pair.Borrower.Id);
        str1 = pair.Borrower.Id;
      }
      XmlNodeList xmlNodeList = this.root.SelectNodes(xpath);
      int num2 = num1 - xmlNodeList.Count;
      XmlElement refChild = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        refChild = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      int count = xmlNodeList.Count;
      while (num2-- > 0)
      {
        XmlElement element1 = this.xmldoc.CreateElement("LIABILITY");
        Mapping.AddEntityId(element1);
        XmlElement element2 = this.xmldoc.CreateElement("EllieMae");
        string str2 = "FL" + (++count).ToString("00");
        this.setFieldAtXpath(element1, "_ID", str2 + "99", this.generateNewId(), this.currentBorrowerPair);
        this.setFieldAtXpath(element2, "PrintAttach", str2 + "36", "Y", this.currentBorrowerPair);
        this.setFieldAtXpath(element2, "NoLinkToDocTrack", str2 + "97", "Y", this.currentBorrowerPair);
        element1.SetAttribute("BorrowerID", str1);
        element1.AppendChild((XmlNode) element2);
        if (refChild != null)
          this.root.InsertAfter((XmlNode) element1, (XmlNode) refChild);
        else
          this.root.AppendChild((XmlNode) element1);
        refChild = element1;
      }
    }

    private void setDefaultVerfiSettings(string verifID)
    {
      this.SetFieldAt(verifID + "36", "Y");
      this.SetFieldAt(verifID + "97", "Y");
      if (this.GetFieldAt("1825") == "2020")
      {
        if (verifID.StartsWith("DD"))
          this.SetFieldAt(verifID + "52", "Y");
        else if (verifID.StartsWith("FM"))
          this.SetFieldAt(verifID + "52", "Y");
      }
      if (verifID.StartsWith("BE") || verifID.StartsWith("CE"))
      {
        this.SetFieldAt(verifID + "81", "Encompass");
      }
      else
      {
        if (!verifID.StartsWith("FM"))
          return;
        this.SetFieldAt(verifID + "60", "Encompass");
      }
    }

    private void createVOM(string id, BorrowerPair pair)
    {
      string xpath;
      string str;
      if (pair == null)
      {
        xpath = "REO_PROPERTY" + this.brwPredicate;
        str = this.brwId;
      }
      else
      {
        xpath = "REO_PROPERTY" + this.brwPredicate.Replace(this.brwId, pair.Borrower.Id);
        str = pair.Borrower.Id;
      }
      int num1 = int.Parse(id.Substring(2, 2));
      if (id.Length > 6)
        num1 = int.Parse(id.Substring(2, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes(xpath);
      int num2 = num1 - xmlNodeList.Count;
      XmlElement refChild = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        refChild = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      int num3 = xmlNodeList.Count;
      if (num3 == 0)
        num3 = 1;
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("REO_PROPERTY");
        Mapping.AddEntityId(element);
        element.SetAttribute("REO_ID", this.generateNewId());
        element.SetAttribute("BorrowerID", str);
        if (refChild != null)
          this.root.InsertAfter((XmlNode) element, (XmlNode) refChild);
        else
          this.root.AppendChild((XmlNode) element);
        refChild = element;
        this.setDefaultVerfiSettings("FM" + num3.ToString("00"));
        ++num3;
      }
    }

    private bool isElmPathExists(string elmPath)
    {
      int length = elmPath.IndexOf("/EllieMae");
      return length > 0 && this.root.SelectSingleNode(elmPath.Substring(0, length)) != null;
    }

    [NoTrace]
    internal bool SetFieldAt(string id, string val)
    {
      return this.SetFieldAt(id, val, (BorrowerPair) null);
    }

    [NoTrace]
    internal bool SetFieldAt(string id, string val, BorrowerPair pair)
    {
      if (Tracing.IsSwitchActive(Mapping.sw, TraceLevel.Info))
      {
        string msg = "... setfield( " + id + ", " + val + (pair == null ? string.Empty : ", " + pair.Borrower.LastName) + ")";
        if (this.traceFields.ContainsKey(id))
          msg = msg + Environment.NewLine + "  #Field Trace (" + id + "):" + Tracing.GetStackTrace();
        else if (Tracing.Debug)
          msg += Mapping.getStackFrameText();
        Tracing.Log(Mapping.sw, TraceLevel.Info, nameof (Mapping), msg);
      }
      string str = "";
      if (this.traceFieldUpdate && this.fieldsToTrace.Contains(id))
        str = this.GetFieldAt(id);
      if (!this.setFieldAtId(id, val, pair))
        return false;
      if (id.StartsWith("DD"))
      {
        string id1 = id.Substring(0, 4) + "35";
        if (id.Length > 6)
          id1 = id.Substring(0, 5) + "35";
        if (this.GetFieldAt(id1, pair) == string.Empty)
          this.setFieldAtId(id1, this.generateNewId(), pair);
      }
      else if (id.StartsWith("BR") || id.StartsWith("CR") && !id.StartsWith("CRED") || id.StartsWith("BE") || id.StartsWith("CE"))
      {
        string id2 = id.Substring(0, 4) + "99";
        if (id.Length > 6)
          id2 = id.Substring(0, 5) + "99";
        if (this.GetFieldAt(id2, pair) == string.Empty)
          this.setFieldAtId(id2, this.generateNewId(), pair);
        if (id.StartsWith("BR"))
          this.createLoanlordInfo(id, val);
      }
      else if (id.StartsWith("FR01") || id.StartsWith("FR03"))
        this.createLoanlordInfo(id, val);
      else if (id.StartsWith("USDA.X") && this.toDouble(id.Substring(6)) >= 108.0 && this.toDouble(id.Substring(6)) <= 119.0)
        this.createLoanlordInfo(id, val);
      else if (id == "2" && this.GetFieldAt("4062") == "Y")
        this.SetFieldAt("4062", "N");
      if (this.traceFieldUpdate && this.fieldsToTrace.Contains(id) && str != val)
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Trace Field Update ***** Field #" + id + " [FROM " + str + " TO " + val + "]");
        stringBuilder.AppendLine("Loan Number: " + this.GetFieldAt("364"));
        stringBuilder.AppendLine(Tracing.GetStackTrace());
        this.WriteToLog(nameof (Mapping), stringBuilder.ToString());
      }
      return true;
    }

    private void WriteToLog(string className, string message)
    {
      try
      {
        message = "[" + DateTime.Now.ToString("MM/dd/yy H:mm:ss.ffff") + "] INFO {" + Thread.CurrentThread.GetHashCode().ToString("000") + "} (" + className + "}: " + message;
        new UTF8Encoding(true).GetBytes(message);
        this.listener.Write(message);
      }
      catch (Exception ex)
      {
        Tracing.Log(true, TraceLevel.Info.ToString().ToUpper(), className, message);
      }
    }

    [NoTrace]
    private static string getStackFrameText() => Tracing.GetStackTrace(1);

    public void SetCreatedDateInUTC(string val)
    {
      ((XmlElement) this.root.SelectSingleNode("EllieMae"))?.SetAttribute("LoanCreatedDateUtc", val);
    }

    private bool setFieldAtId(string id, string val, BorrowerPair pair)
    {
      Field field = !((id ?? "") == "") ? this.GetFieldObject(id, pair) : throw new ArgumentException("Invalid or missing field ID specified");
      string xpath = field.Xpath;
      string empty = string.Empty;
      if (xpath == "")
        throw new InvalidOperationException("The specified field '" + id + "' is not defined and cannot be updated.");
      id = field.Id;
      string fieldAt = this.GetFieldAt(field.Id, pair);
      if (fieldAt == val)
        return false;
      int length = xpath.IndexOf("/@");
      if (length == -1)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "SetCurrentField(), no XPath defined (no attribute for id)!!! id: " + id);
        return false;
      }
      string str = xpath.Substring(0, length);
      string name = xpath.Substring(length + 2);
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(str);
      if (xmlElement == null)
      {
        if (val == string.Empty || val == null)
          return true;
        if (id.StartsWith("NBOC") && !this.isElmPathExists(str) && id.Length >= 8)
          this.createRepeatableNode(id, "EllieMae/NonBorrowingOwnerContacts", "NonBorrowingOwner", true);
        else if (id.StartsWith("XCOC") && !this.isElmPathExists(str) && id.Length >= 8)
          this.createRepeatableNode(id, "EllieMae/AlertChangeCircumstanceList", "AlertChangeCircumstance", true);
        else if (id.StartsWith("FL") && !this.isElmPathExists(str))
        {
          if (id.IndexOf(".X") == -1)
            this.createVOL(field.Id, pair);
        }
        else if (id.StartsWith("FR") && !this.isElmPathExists(str))
          this.createVOR(field.Id, pair);
        else if (!id.StartsWith("FEMA") && id.StartsWith("FE") && !this.isElmPathExists(str))
          this.createVOE(field.Id, pair);
        else if (id.StartsWith("FM") && !this.isElmPathExists(str) && id.Length == 6)
          this.createVOM(field.Id, pair);
        else if (id.StartsWith("SP") && !this.isElmPathExists(str) && id.Length == 6)
          this.createProvider(id);
        else if (id.StartsWith("HC") && !this.isElmPathExists(str) && id.Length == 6)
          this.createHomeCounselingProvider(id, (string) null);
        else if (id.StartsWith("AB") && !this.isElmPathExists(str) && id.Length == 6)
          this.createAffiliate(id);
        else if (EncompassFields.IsUserDefinedField(field.Id) && !this.isElmPathExists(str))
          this.createCustomField(field.Id);
        else if (id.StartsWith("IR") && !this.isElmPathExists(str) && (id.Length == 6 || id.Length == 8))
          this.createTAX4506T(false);
        else if (id.StartsWith("AR") && !this.isElmPathExists(str) && id.Length == 6)
          this.createTAX4506T(true);
        else if (id.StartsWith("UNFL") && !this.isElmPathExists(str) && id.Length >= 8)
          this.createRepeatableNode(id, "EllieMae/NonVols", "NonVol", false);
        else if (id.StartsWith("URLAROA") && !this.isElmPathExists(str) && id.Length >= 11)
          this.createOtherAsset(field.Id, pair);
        else if (id.StartsWith("URLAROL") && !this.isElmPathExists(str) && id.Length >= 11)
          this.createOtherLiability(field.Id, pair);
        else if (id.StartsWith("URLAROIS") && !this.isElmPathExists(str) && id.Length >= 12)
          this.createRepeatableNode(id, str, "OtherIncomeSource", true);
        else if (id.StartsWith("URLARGG") && !this.isElmPathExists(str) && id.Length >= 11)
          this.createRepeatableNode(id, str, "GiftGrant", true);
        else if (id.StartsWith("DOCPROV") && !this.isElmPathExists(str) && id.Length >= 11)
          this.createProvidedDocument(field.Id, pair);
        else if (id.StartsWith("DD") && id.Length >= 6)
          this.createRepeatableNode(id, str, "DEPOSIT", true);
        else if (id.StartsWith("FEMA") && id.Length >= 8)
          this.createRepeatableNode(id, str, "Disaster", true);
        if (!id.StartsWith("SP"))
          xmlElement = field.CachedElement;
        if (xmlElement == null || xmlElement.OwnerDocument != this.root.OwnerDocument)
          xmlElement = this.createPath(str);
        if (xmlElement == null)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Unable to set the field at " + id + " in borrower pair " + (object) pair);
          return false;
        }
        field.CachedElement = xmlElement;
      }
      if (string.IsNullOrEmpty(val) && xmlElement.HasAttribute(name))
        xmlElement.RemoveAttribute(name);
      else
        xmlElement.SetAttribute(name, val);
      field.CachedValue = val;
      this.onFieldChange(field.Id, pair, fieldAt, val);
      return true;
    }

    private bool setFieldAtXpath(
      XmlElement element,
      string attr,
      string fieldId,
      string val,
      BorrowerPair pair)
    {
      try
      {
        if (element == null)
          return false;
        string attribute = element.GetAttribute(attr);
        if (attribute == val)
          return false;
        if (string.IsNullOrEmpty(val) && element.HasAttribute(attr))
          element.RemoveAttribute(attr);
        else
          element.SetAttribute(attr, val);
        this.onFieldChange(fieldId, pair, attribute, val);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in setFieldAtXpath() for field" + fieldId + "', Error: " + ex.Message);
        return false;
      }
    }

    private XmlElement createAdditionalField(string path, string originalID)
    {
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode(path);
      if (newChild == null)
      {
        XmlNode path1 = (XmlNode) this.createPath("EllieMae/RateLock/Request/AdditionalFields");
        newChild = this.xmldoc.CreateElement("FIELD");
        path1.AppendChild((XmlNode) newChild);
        newChild.SetAttribute("id", originalID);
      }
      return newChild;
    }

    private void onFieldChange(
      string fieldId,
      BorrowerPair pair,
      string priorValue,
      string newValue,
      bool suppressNotifications = false)
    {
      try
      {
        if (!this.ddmIsRequired && this.ddmOnDemandTriggerFields != null && this.ddmOnDemandTriggerFields.IndexOf("|" + fieldId.ToUpper() + "|") > -1)
        {
          this.ddmIsRequired = true;
          Tracing.Log(Mapping.sw, nameof (Mapping), TraceLevel.Info, "DDM - DDM will be applied before loan saved due to the value is changed in field : " + fieldId + ". Prior Value: " + priorValue + ", New Value: " + newValue);
        }
        if (this.MappingFieldChanged == null)
          return;
        this.MappingFieldChanged((object) this, new MappingFieldChangedEventArgs(fieldId, pair, priorValue, newValue, suppressNotifications));
      }
      catch (CountyLimitException ex)
      {
        Tracing.Log(Mapping.sw, nameof (Mapping), TraceLevel.Error, "Failed County Limit Validation.  Error: " + ex.Message);
      }
      catch (GFEDaysToExpireException ex)
      {
        Tracing.Log(Mapping.sw, nameof (Mapping), TraceLevel.Error, "GFE Days to Expire Validation.  Error: " + ex.Message);
      }
      catch (ComplianceCalendarException ex)
      {
        Tracing.Log(Mapping.sw, nameof (Mapping), TraceLevel.Error, "Compliance Calendar Validation.  Error: " + ex.Message);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, nameof (Mapping), TraceLevel.Error, "Error in FieldChanged event. Field ID = " + fieldId + ", PriorValue = " + priorValue + ", NewValue = " + newValue + ".  Error: " + (object) ex);
      }
    }

    private void createCustomField(string fieldId)
    {
      if (string.IsNullOrEmpty(fieldId))
        return;
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode("EllieMae/CUSTOM_FIELDS");
      if (newChild == null)
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae");
        newChild = this.xmldoc.CreateElement("CUSTOM_FIELDS");
        XmlNode refChild = xmlElement.SelectSingleNode("CUSTOM_FIELDS");
        if (refChild != null)
          xmlElement.InsertAfter((XmlNode) newChild, refChild);
        else
          xmlElement.AppendChild((XmlNode) newChild);
      }
      ((XmlElement) newChild.AppendChild((XmlNode) newChild.OwnerDocument.CreateElement("FIELD"))).SetAttribute("FieldID", fieldId);
    }

    private void createLockField(string fieldId, string value)
    {
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode("LOCK/FIELD[@id=\"" + fieldId + "\"]");
      if (newChild == null)
      {
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("LOCK") ?? this.xmldoc.CreateElement("LOCK");
        newChild = this.xmldoc.CreateElement("FIELD");
        xmlElement.AppendChild((XmlNode) newChild);
      }
      newChild.SetAttribute("id", fieldId);
      newChild.SetAttribute("val", value);
    }

    internal string GetAdditionalField(string id)
    {
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/CUSTOM_FIELDS/FIELD[@FieldID='" + id + "']");
      return xmlElement == null ? string.Empty : xmlElement.GetAttribute("FieldValue");
    }

    internal bool SetAdditionalField(string id, string val)
    {
      if (string.IsNullOrEmpty(id))
        return false;
      string xpath = "EllieMae/CUSTOM_FIELDS/FIELD[@FieldID='" + id + "']";
      XmlNode oldChild = this.root.SelectSingleNode(xpath);
      XmlElement xmlElement = (XmlElement) oldChild;
      if (xmlElement == null && val == string.Empty)
        return true;
      if (xmlElement == null)
      {
        this.createCustomField(id);
        oldChild = this.root.SelectSingleNode(xpath);
        xmlElement = (XmlElement) oldChild;
      }
      if (xmlElement == null)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Warning, nameof (Mapping), "No XPath defined for id: " + id);
        return false;
      }
      string additionalField = this.GetAdditionalField(id);
      if (additionalField == val)
        return true;
      if (val == string.Empty)
        oldChild.ParentNode.RemoveChild(oldChild);
      else
        xmlElement.SetAttribute("FieldValue", val);
      this.onFieldChange(id, (BorrowerPair) null, additionalField, val);
      return true;
    }

    internal string GetFieldAt(string id, BorrowerPair pair)
    {
      Field fieldObject = this.GetFieldObject(id, pair);
      if (fieldObject.IsXPathFixed && fieldObject.CachedValue != null)
        return fieldObject.CachedValue;
      string xpath = fieldObject.Xpath;
      if (xpath == string.Empty)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Warning, nameof (Mapping), "No XPath defined for id: " + id + " for pair " + (object) pair);
        return string.Empty;
      }
      string fieldAt = fieldObject.IsXPathFixed ? this.GetFieldAtFixedXpath(fieldObject) : this.GetFieldAtXpath(xpath);
      if (fieldObject.IsXPathFixed)
        fieldObject.CachedValue = fieldAt;
      return fieldAt;
    }

    internal string GetFieldAt(string id) => this.GetFieldAt(id, (BorrowerPair) null);

    private string GetFieldAtFixedXpath(Field field)
    {
      if (this.cachedElements == null)
        this.cachedElements = new Dictionary<string, XmlElement>();
      XmlElement xmlElement1 = (XmlElement) null;
      if (field.XPathElement != null && this.cachedElements.TryGetValue(field.XPathElement, out xmlElement1))
        return xmlElement1.GetAttribute(field.XPathAttribute);
      try
      {
        if (!(this.root.SelectSingleNode(field.XPathElement != null ? field.XPathElement : field.Xpath) is XmlElement xmlElement2))
          return field.Xpath != null && this.root != null && field.Xpath[0] == '@' ? this.root.GetAttribute(field.Xpath.Substring(1)) : string.Empty;
      }
      catch (XPathException ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Warning, nameof (Mapping), "Invalid XPath: " + field.XPathElement);
        return string.Empty;
      }
      if (field.XPathElement != null && !this.cachedElements.ContainsKey(field.XPathElement))
        this.cachedElements.Add(field.XPathElement, xmlElement2);
      return xmlElement2.GetAttribute(field.XPathAttribute);
    }

    private string GetFieldAtXpath(string p)
    {
      XmlNode xmlNode;
      try
      {
        xmlNode = this.root.SelectSingleNode(p);
      }
      catch (XPathException ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Warning, nameof (Mapping), "Invalid XPath: " + p);
        return string.Empty;
      }
      if (xmlNode == null)
        return string.Empty;
      return xmlNode.Value;
    }

    internal int GetNumberOfLiabilitesExlcudingAlimonyJobExp()
    {
      return this.root.SelectNodes(this.liabPath).Count;
    }

    internal int RemoveLiabilityAt(int n)
    {
      try
      {
        string str1 = (n + 1).ToString("00");
        double num1 = this.toDouble(this.GetFieldAt("FL" + str1 + "13"));
        double num2 = this.toDouble(this.GetFieldAt("FL" + str1 + "11"));
        string fieldAt = this.GetFieldAt("FL" + str1 + "25");
        this.logList.RemoveVerif(this.GetFieldAt("FL" + str1 + "99"));
        XmlNode selectNode = this.root.SelectNodes(this.liabPath)[n];
        selectNode.ParentNode.RemoveChild(selectNode);
        int numberOfMortgages = this.GetNumberOfMortgages();
        int num3 = -1;
        for (int index = 1; index <= numberOfMortgages; ++index)
        {
          string str2 = "FM" + index.ToString("00");
          if (this.GetFieldAt(str2 + "43") == fieldAt)
          {
            double num4 = this.toDouble(this.GetFieldAt(str2 + "17")) - num1;
            double num5 = this.toDouble(this.GetFieldAt(str2 + "16")) - num2;
            this.SetFieldAt(str2 + "17", num4.ToString());
            this.SetFieldAt(str2 + "16", num5.ToString());
            num3 = index - 1;
          }
        }
        return num3;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove liability " + (object) n + ", e: " + ex.Message);
        return -2;
      }
    }

    internal int NewLiability()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.liabPath);
      this.createVOL("FL" + (xmlNodeList.Count + 1).ToString("00") + "00", (BorrowerPair) null);
      return xmlNodeList.Count;
    }

    internal void UpLiability(int i)
    {
      if (i == 0)
        return;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(this.liabPath);
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in MoveUpLiability, e: " + ex.Message);
      }
    }

    internal void UpVerification(string xmlPath, int i)
    {
      if (i == 0)
        return;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(xmlPath);
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in MoveUp " + xmlPath + ", e: " + ex.Message);
      }
    }

    internal void DownLiability(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(this.liabPath);
        if (i >= xmlNodeList.Count - 1)
          return;
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i + 1];
        newChild.ParentNode.InsertAfter(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in MoveUpLiability, e: " + ex.Message);
      }
    }

    internal void UpOtherAsset(int i)
    {
      if (i == 0)
        return;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(this.otherAssetPath);
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in MoveUpOtherAsset, e: " + ex.Message);
      }
    }

    internal void DownOtherAsset(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(this.otherAssetPath);
        if (i >= xmlNodeList.Count - 1)
          return;
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i + 1];
        newChild.ParentNode.InsertAfter(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in MoveDownOtherAsset, e: " + ex.Message);
      }
    }

    internal void DownVerification(string xmlPath, int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes(xmlPath);
        if (i >= xmlNodeList.Count - 1)
          return;
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i + 1];
        newChild.ParentNode.InsertAfter(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in MoveDown " + xmlPath + ", e: " + ex.Message);
      }
    }

    internal int NewAdditionalLoan()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.additionalLoanpath);
      this.createVOAL("URLARAL" + (xmlNodeList.Count + 1).ToString("00") + "00", (BorrowerPair) null);
      return xmlNodeList.Count;
    }

    private void createVOAL(string id, BorrowerPair pair)
    {
      int num1 = int.Parse(id.Substring(7, 2));
      if (id.Length > 11)
        num1 = int.Parse(id.Substring(7, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes((pair == null ? "BORROWER" + this.brwPredicate : "BORROWER" + this.brwPredicate.Replace(this.brwId, pair.Borrower.Id)) + "/URLA2020/AdditionalLoans/Additional_Loan");
      int num2 = num1 - xmlNodeList.Count;
      XmlElement xmlElement = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        xmlElement = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      int num3 = xmlNodeList.Count;
      if (num3 == 0)
        num3 = 1;
      XmlNode parent = this.brwElm.SelectSingleNode("URLA2020/AdditionalLoans");
      if (parent == null)
      {
        parent = this.CreateAdditionalLoanRoot();
        this.brwElm.AppendChild(parent.ParentNode);
      }
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("Additional_Loan");
        Mapping.AddEntityId(element);
        if (id.Length > 10)
        {
          string str = id.Substring(0, 9);
          this.setFieldAtXpath(element, "ID", str + "99", this.generateNewId(), this.currentBorrowerPair);
        }
        this.AppendSameTypeChild((XmlElement) parent, element, "Additional_Loan");
        ++num3;
      }
    }

    private XmlNode CreateAdditionalLoanRoot()
    {
      return (this.brwElm.SelectSingleNode("URLA2020") != null ? this.brwElm.SelectSingleNode("URLA2020") : (XmlNode) this.xmldoc.CreateElement("URLA2020")).AppendChild((XmlNode) this.xmldoc.CreateElement("AdditionalLoans"));
    }

    internal int GetNumberOfMortgages()
    {
      return this.root.SelectNodes("REO_PROPERTY" + this.brwPredicate).Count;
    }

    internal bool RemoveMortgageAt(int n)
    {
      try
      {
        int num1 = n + 1;
        string fieldAt1 = this.GetFieldAt("FM" + num1.ToString("00") + "43");
        string fieldAt2 = this.GetFieldAt("FM" + num1.ToString("00") + "28");
        this.logList.RemoveVerif(this.GetFieldAt("FM" + num1.ToString("00") + "43"));
        XmlNode selectNode = this.root.SelectNodes("REO_PROPERTY" + this.brwPredicate)[n];
        XmlNodeList childNodes = this.lockRoot.ChildNodes;
        for (int i = 0; i < childNodes.Count; ++i)
        {
          XmlElement oldChild = (XmlElement) childNodes[i];
          string attribute = oldChild.GetAttribute("id");
          if (attribute.StartsWith("FM"))
          {
            int num2 = int.Parse(attribute.Substring(2, 2)) - 1;
            string str = attribute.Substring(4, 2);
            if (attribute.Length > 6)
            {
              num2 = int.Parse(attribute.Substring(2, 3)) - 1;
              str = attribute.Substring(5, 2);
            }
            if (num2 == n)
            {
              this.RemoveLockCacheAt(attribute);
              this.lockRoot.RemoveChild((XmlNode) oldChild);
            }
            else if (num2 > n)
            {
              string id = "FM" + num2.ToString("00") + str;
              this.RemoveLockCacheAt(attribute);
              this.RemoveLockCacheAt(id);
              oldChild.SetAttribute("id", id);
            }
          }
        }
        selectNode.ParentNode.RemoveChild(selectNode);
        if (fieldAt1 != string.Empty)
        {
          int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
          for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
          {
            string fieldAt3 = this.GetFieldAt("FL" + index.ToString("00") + "25");
            if (fieldAt1 == fieldAt3)
            {
              this.SetFieldAt("FL" + index.ToString("00") + "25", string.Empty);
              if (fieldAt2 == "Y" && this.GetFieldAt("FL" + index.ToString("00") + "27") == "Y")
                this.SetFieldAt("FL" + index.ToString("00") + "27", string.Empty);
            }
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove VOM " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal int NewMortgage(string selectedVOL)
    {
      string newId = this.generateNewId();
      double num1 = 0.0;
      double num2 = 0.0;
      string val = "";
      bool flag = true;
      if (selectedVOL != string.Empty)
      {
        string[] source = selectedVOL.Split('|');
        if (((IEnumerable<string>) source).Count<string>() > 0)
          val = this.GetFieldAt("FL" + this.toDouble(source[0]).ToString("00") + "15");
        for (int index = 0; index < source.Length; ++index)
        {
          double num3 = this.toDouble(source[index]);
          num1 += this.toDouble(this.GetFieldAt("FL" + num3.ToString("00") + "13"));
          num2 += this.toDouble(this.GetFieldAt("FL" + num3.ToString("00") + "11"));
          string fieldAt1 = this.GetFieldAt("FL" + num3.ToString("00") + "15");
          string fieldAt2 = this.GetFieldAt("FL" + num3.ToString("00") + "08");
          if (fieldAt2 != "HELOC" && fieldAt2 != "MortgageLoan" || fieldAt1 != val)
            flag = false;
          this.SetFieldAt("FL" + num3.ToString("00") + "25", newId);
        }
      }
      XmlElement element = this.xmldoc.CreateElement("REO_PROPERTY");
      Mapping.AddEntityId(element);
      element.SetAttribute("BorrowerID", this.brwId);
      this.root.AppendChild((XmlNode) element);
      int count = this.root.SelectNodes("REO_PROPERTY" + this.brwPredicate).Count;
      this.setFieldAtXpath(element, "REO_ID", "FM" + count.ToString("00") + "43", newId, this.currentBorrowerPair);
      this.setFieldAtXpath(element, "_LienUPBAmount", "FM" + count.ToString("00") + "17", newId, this.currentBorrowerPair);
      this.setFieldAtXpath(element, "_LienInstallmentAmount", "FM" + count.ToString("00") + "16", newId, this.currentBorrowerPair);
      this.setDefaultVerfiSettings("FM" + count.ToString("00"));
      if (this.GetFieldAt("1825") == "2020" && flag && !string.IsNullOrEmpty(selectedVOL))
        this.SetFieldAt("FM" + count.ToString("00") + "46", val);
      return count - 1;
    }

    internal int AttachMortgage(string currentInd, string selectedVOL)
    {
      double num1 = 0.0;
      double num2 = 0.0;
      string fieldAt1 = this.GetFieldAt("FM" + currentInd + "43");
      string fieldAt2 = this.GetFieldAt("FM" + currentInd + "46");
      bool flag1 = true;
      bool flag2 = false;
      string val = "";
      int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      if (selectedVOL != string.Empty)
        selectedVOL = "|" + selectedVOL + "|";
      if (exlcudingAlimonyJobExp > 0 && selectedVOL != string.Empty)
      {
        for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
        {
          string str = "FL" + index.ToString("00");
          if (selectedVOL.IndexOf("|" + index.ToString() + "|") != -1)
          {
            val = this.GetFieldAt(str + "15");
            break;
          }
        }
      }
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        string fieldAt3 = this.GetFieldAt(str + "25");
        int num3 = selectedVOL.IndexOf("|" + index.ToString() + "|");
        string fieldAt4 = this.GetFieldAt(str + "15");
        if (this.GetFieldAt("1825") == "2020" && num3 != -1)
        {
          if (fieldAt2 != "")
          {
            if (fieldAt4 != fieldAt2)
              flag2 = true;
          }
          else if (val != fieldAt4)
            flag1 = false;
        }
        if (fieldAt3 == fieldAt1)
        {
          if (num3 == -1)
            this.SetFieldAt(str + "25", string.Empty);
        }
        else if (num3 != -1)
          this.SetFieldAt(str + "25", fieldAt1);
        if (num3 != -1)
        {
          num1 += this.toDouble(this.GetFieldAt(str + "13"));
          num2 += this.toDouble(this.GetFieldAt(str + "11"));
        }
      }
      if (flag1 && !string.IsNullOrEmpty(selectedVOL))
        this.SetFieldAt("FM" + currentInd + "46", val);
      if (flag2)
        this.SetFieldAt("FM" + currentInd + "46", "");
      this.SetFieldAt("FM" + currentInd + "17", num1.ToString());
      this.SetFieldAt("FM" + currentInd + "16", num2.ToString());
      return int.Parse(currentInd) - 1;
    }

    internal void UpMortgage(int n)
    {
      if (n == 0 || this.GetFieldAt("1825") == "2020" && n == 1 && this.GetFieldAt("FM0128") == "Y")
        return;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("REO_PROPERTY" + this.brwPredicate);
        XmlNode newChild = xmlNodeList[n];
        XmlNode refChild = xmlNodeList[n - 1];
        XmlNode parentNode = newChild.ParentNode;
        XmlNodeList childNodes = this.lockRoot.ChildNodes;
        for (int i = 0; i < childNodes.Count; ++i)
        {
          XmlElement xmlElement = (XmlElement) childNodes[i];
          string attribute = xmlElement.GetAttribute("id");
          if (attribute.StartsWith("FM"))
          {
            int num = int.Parse(attribute.Substring(2, 2)) - 1;
            string str = attribute.Substring(4, 2);
            if (attribute.Length > 6)
            {
              num = int.Parse(attribute.Substring(2, 3)) - 1;
              str = attribute.Substring(5, 2);
            }
            if (num == n)
            {
              string id = "FM" + num.ToString("00") + str;
              this.RemoveLockCacheAt(attribute);
              this.RemoveLockCacheAt(id);
              xmlElement.SetAttribute("id", id);
            }
            else if (num == n - 1)
            {
              string id = "FM" + (num + 2).ToString("00") + str;
              this.RemoveLockCacheAt(attribute);
              this.RemoveLockCacheAt(id);
              xmlElement.SetAttribute("id", id);
            }
          }
        }
        parentNode.InsertBefore(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpMortgage, e: " + ex.Message);
      }
    }

    internal void DownMortgage(int n)
    {
      try
      {
        if (this.GetFieldAt("1825") == "2020" && n == 0 && this.GetFieldAt("FM0128") == "Y")
          return;
        XmlNodeList xmlNodeList = this.root.SelectNodes("REO_PROPERTY" + this.brwPredicate);
        if (n >= xmlNodeList.Count - 1)
          return;
        XmlNode newChild = xmlNodeList[n];
        XmlNode refChild = xmlNodeList[n + 1];
        XmlNode parentNode = newChild.ParentNode;
        XmlNodeList childNodes = this.lockRoot.ChildNodes;
        for (int i = 0; i < childNodes.Count; ++i)
        {
          XmlElement xmlElement = (XmlElement) childNodes[i];
          string attribute = xmlElement.GetAttribute("id");
          if (attribute.StartsWith("FM"))
          {
            int num = int.Parse(attribute.Substring(2, 2)) - 1;
            string str = attribute.Substring(4, 2);
            if (attribute.Length > 6)
            {
              num = int.Parse(attribute.Substring(2, 3)) - 1;
              str = attribute.Substring(5, 2);
            }
            if (num == n)
            {
              string id = "FM" + (num + 2).ToString("00") + str;
              this.RemoveLockCacheAt(attribute);
              this.RemoveLockCacheAt(id);
              xmlElement.SetAttribute("id", id);
            }
            else if (num == n + 1)
            {
              string id = "FM" + num.ToString("00") + str;
              this.RemoveLockCacheAt(attribute);
              this.RemoveLockCacheAt(id);
              xmlElement.SetAttribute("id", id);
            }
          }
        }
        parentNode.InsertAfter(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownMortgage, e: " + ex.Message);
      }
    }

    internal int GetNumberOfAdditionalVestingParties()
    {
      return this.root.SelectNodes("_CLOSING_DOCUMENTS/TRUSTEE").Count;
    }

    internal bool RemoveVestingPartyAt(int n)
    {
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("_CLOSING_DOCUMENTS/TRUSTEE");
      if (xmlNodeList1.Count <= n)
        return false;
      XmlNode oldChild = xmlNodeList1[n];
      string attribute = ((XmlElement) xmlNodeList1[n]).GetAttribute("_NBORecordID");
      if (attribute != "")
      {
        XmlNodeList xmlNodeList2 = this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner");
        if (xmlNodeList2 == null || xmlNodeList2.Count == 0)
          return true;
        for (int i = 0; i < xmlNodeList2.Count; ++i)
        {
          XmlElement xmlElement = (XmlElement) xmlNodeList2[i];
          if (xmlElement.HasAttribute("_ID") && string.Compare(attribute, xmlElement.GetAttribute("_ID"), true) == 0)
          {
            xmlNodeList2[i].ParentNode.RemoveChild(xmlNodeList2[i]);
            break;
          }
        }
      }
      oldChild.ParentNode.RemoveChild(oldChild);
      return true;
    }

    internal int NewVestingParty()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("_CLOSING_DOCUMENTS/TRUSTEE");
      int num = xmlNodeList.Count + 1;
      XmlElement xmlElement = (XmlElement) ((XmlElement) this.root.SelectSingleNode("_CLOSING_DOCUMENTS") ?? (XmlElement) this.root.AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("_CLOSING_DOCUMENTS"))).AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("TRUSTEE"));
      Mapping.AddEntityId(xmlElement);
      string str = num.ToString("00");
      this.setFieldAtXpath(xmlElement, "Guid", "TR" + str + "10", this.generateNewId(), this.currentBorrowerPair);
      return xmlNodeList.Count;
    }

    internal int GetNumberOfDeposits() => this.brwElm.SelectNodes("EllieMae/DEPOSIT").Count;

    internal int GetNumberOfGiftsAndGrants()
    {
      return this.brwElm.SelectNodes("URLA2020/GiftsGrants/GiftGrant").Count;
    }

    internal int GetNumberOfOtherIncomeSources()
    {
      return this.brwElm.SelectNodes("URLA2020/OtherIncomeSources/OtherIncomeSource").Count;
    }

    internal bool RemoveDepositAt(int n)
    {
      try
      {
        XmlNode selectNode = this.brwElm.SelectNodes("EllieMae/DEPOSIT")[n];
        string fieldAt = this.GetFieldAt("DD" + (n + 1).ToString("00") + "35");
        this.logList.RemoveVerif(fieldAt);
        XmlNodeList xmlNodeList = this.root.SelectNodes("ASSET/EllieMae[@AssetID=\"" + fieldAt + "\"]");
        if (xmlNodeList.Count != 0)
        {
          for (int i = xmlNodeList.Count - 1; i >= 0; --i)
            this.root.RemoveChild(xmlNodeList[i].ParentNode);
        }
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Deposit " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveGiftGrantAt(int n)
    {
      try
      {
        XmlNode selectNode = this.brwElm.SelectNodes("URLA2020/GiftsGrants/GiftGrant")[n];
        this.logList.RemoveVerif(this.GetFieldAt("URLARGG" + (n + 1).ToString("00") + "01"));
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Gifts and Grants " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveOtherLiabilityAt(int n)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes(this.otherliabilitypath)[n];
        this.logList.RemoveVerif(this.GetFieldAt("URLAROL" + (n + 1).ToString("00") + "99"));
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Deposit " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveAdditionalLoanAt(int n)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes(this.additionalLoanpath)[n];
        this.logList.RemoveVerif(this.GetFieldAt("URLARAL" + (n + 1).ToString("00") + "99"));
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Additional Loan " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveOtherAssetAt(int n)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes(this.otherAssetPath)[n];
        this.logList.RemoveVerif(this.GetFieldAt("URLAROA" + (n + 1).ToString("00") + "99"));
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Other Assets " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveOtherIncomeSourceAt(int n)
    {
      try
      {
        XmlNode selectNode = this.brwElm.SelectNodes("URLA2020/OtherIncomeSources/OtherIncomeSource")[n];
        this.logList.RemoveVerif(this.GetFieldAt("URLAROIS" + (n + 1).ToString("00") + "01"));
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Other Incomes " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal int NewDeposit()
    {
      XmlNodeList xmlNodeList = this.brwElm.SelectNodes("EllieMae/DEPOSIT");
      int num = xmlNodeList.Count + 1;
      XmlNode parent = this.brwElm.SelectSingleNode("EllieMae");
      XmlElement element = this.xmldoc.CreateElement("DEPOSIT");
      Mapping.AddEntityId(element);
      this.AppendSameTypeChild((XmlElement) parent, element, "DEPOSIT");
      this.SetFieldAt("DD" + num.ToString("00") + "35", this.generateNewId());
      this.setDefaultVerfiSettings("DD" + num.ToString("00"));
      return xmlNodeList.Count;
    }

    internal int NewGiftGrant()
    {
      XmlNodeList xmlNodeList = this.brwElm.SelectNodes("URLA2020/GiftsGrants/GiftGrant");
      int num = xmlNodeList.Count + 1;
      XmlNode newChild = this.brwElm.SelectSingleNode("URLA2020");
      if (newChild == null)
      {
        newChild = (XmlNode) this.xmldoc.CreateElement("URLA2020");
        this.brwElm.AppendChild(newChild);
      }
      if (this.brwElm.SelectSingleNode("URLA2020/GiftsGrants") == null)
      {
        XmlNode element = (XmlNode) this.xmldoc.CreateElement("GiftsGrants");
        newChild.AppendChild(element);
      }
      XmlNode parent = this.brwElm.SelectSingleNode("URLA2020/GiftsGrants");
      XmlElement element1 = this.xmldoc.CreateElement("GiftGrant");
      Mapping.AddEntityId(element1);
      this.AppendSameTypeChild((XmlElement) parent, element1, "GiftGrant");
      this.SetFieldAt("URLARGG" + num.ToString("00") + "01", this.generateNewId());
      this.SetFieldAt("URLARGG" + num.ToString("00") + "04", "Y");
      return xmlNodeList.Count;
    }

    internal int NewOtherLiability()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.otherliabilitypath);
      this.createOtherLiability("URLAROL" + (xmlNodeList.Count + 1).ToString("00") + "00", (BorrowerPair) null);
      return xmlNodeList.Count;
    }

    internal int NewOtherIncomeSource()
    {
      XmlNodeList xmlNodeList = this.brwElm.SelectNodes("URLA2020/OtherIncomeSources/OtherIncomeSource");
      int num = xmlNodeList.Count + 1;
      XmlNode newChild = this.brwElm.SelectSingleNode("URLA2020");
      if (newChild == null)
      {
        newChild = (XmlNode) this.xmldoc.CreateElement("URLA2020");
        this.brwElm.AppendChild(newChild);
      }
      if (this.brwElm.SelectSingleNode("URLA2020/OtherIncomeSources") == null)
      {
        XmlNode element = (XmlNode) this.xmldoc.CreateElement("OtherIncomeSources");
        newChild.AppendChild(element);
      }
      XmlNode parent = this.brwElm.SelectSingleNode("URLA2020/OtherIncomeSources");
      XmlElement element1 = this.xmldoc.CreateElement("OtherIncomeSource");
      Mapping.AddEntityId(element1);
      this.AppendSameTypeChild((XmlElement) parent, element1, "OtherIncomeSource");
      this.SetFieldAt("URLAROIS" + num.ToString("00") + "01", this.generateNewId());
      this.SetFieldAt("URLAROIS" + num.ToString("00") + "04", "Y");
      this.SetFieldAt("URLAROIS" + num.ToString("00") + "23", "Encompass");
      return xmlNodeList.Count;
    }

    private void AppendSameTypeChild(XmlElement parent, XmlElement child, string type)
    {
      XmlNodeList xmlNodeList = parent.SelectNodes(type);
      if (xmlNodeList.Count == 0)
        parent.AppendChild((XmlNode) child);
      else
        parent.InsertAfter((XmlNode) child, xmlNodeList[xmlNodeList.Count - 1]);
    }

    private void createVORVOE(
      string id,
      string type,
      string attr,
      string current,
      string prior,
      string xpath,
      BorrowerPair pair)
    {
      int num1 = int.Parse(id.Substring(2, 2));
      bool flag = false;
      if (id.Length > 6)
        num1 = int.Parse(id.Substring(2, 3));
      string str1;
      XmlElement parent;
      if (num1 % 2 == 1)
      {
        str1 = "Borrower";
        parent = pair != null ? (XmlElement) this.root.SelectSingleNode("BORROWER" + this.brwPredicate.Replace(this.brwId, pair.Borrower.Id)) : this.brwElm;
      }
      else
      {
        str1 = "CoBorrower";
        parent = pair != null ? (XmlElement) this.root.SelectSingleNode("BORROWER" + this.coBrwPredicate.Replace(this.brwId, pair.CoBorrower.Id)) : this.coBrwElm;
      }
      string str2;
      if (type == "EMPLOYER")
      {
        flag = num1 == 3 || num1 == 4;
        str2 = !(str1 == "Borrower") ? "CE" : "BE";
      }
      else
        str2 = !(str1 == "Borrower") ? "CR" : "BR";
      if (((num1 == 1 ? 1 : (num1 == 2 ? 1 : 0)) | (flag ? 1 : 0)) != 0)
      {
        xpath = xpath.Replace("\"N\"", "\"Y\"");
        int num2 = parent.SelectNodes(xpath).Count + 1;
        if (id.StartsWith("FR01") || id.StartsWith("FR02"))
          num2 = parent.SelectNodes("_RESIDENCE[@BorrowerResidencyType=\"Current\"]").Count + 1;
        int num3 = flag ? 2 : 1;
        bool borrower = str1 == "Borrower";
        for (; num2 <= num3; ++num2)
        {
          int num4 = this.GetNumberOfEmployer(borrower) + 1;
          XmlElement element1 = this.xmldoc.CreateElement(type);
          Mapping.AddEntityId(element1);
          XmlElement element2 = this.xmldoc.CreateElement("EllieMae");
          element1.AppendChild((XmlNode) element2);
          element1.SetAttribute(attr, current);
          element2.SetAttribute("Person", str1);
          element2.SetAttribute("_ID", this.generateNewId());
          this.AppendSameTypeChild(parent, element1, type);
          if (type == "EMPLOYER")
            this.setDefaultVerfiSettings(str2 + num4.ToString("00"));
          else
            this.setDefaultVerfiSettings(str2 + num2.ToString("00"));
        }
      }
      else
      {
        int num5 = !(type == "EMPLOYER") || num1 != 5 && num1 != 6 ? (num1 - 1) / 2 : 1;
        int count = parent.SelectNodes(xpath).Count;
        bool borrower = str1 == "Borrower";
        while (count < num5)
        {
          int num6 = this.GetNumberOfEmployer(borrower) + 1;
          XmlElement element3 = this.xmldoc.CreateElement(type);
          Mapping.AddEntityId(element3);
          XmlElement element4 = this.xmldoc.CreateElement("EllieMae");
          element3.AppendChild((XmlNode) element4);
          element3.SetAttribute(attr, prior);
          element4.SetAttribute("Person", str1);
          element4.SetAttribute("_ID", this.generateNewId());
          this.AppendSameTypeChild(parent, element3, type);
          ++count;
          if (type == "EMPLOYER")
            this.setDefaultVerfiSettings(str2 + num6.ToString("00"));
          else
            this.setDefaultVerfiSettings(str2 + count.ToString("00"));
        }
      }
    }

    private void createLoanlordInfo(string id, string val)
    {
      string id1 = this.currentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      this.SetBorrowerPair(borrowerPairs[0]);
      try
      {
        if (id != null && id.StartsWith("USDA.X"))
        {
          int num = Utils.ParseInt((object) id.Substring(6));
          if (num >= 108 && num <= 113)
            this.setFieldAtId("FR0115", "Rent", borrowerPairs[0]);
          else if (num >= 114 && num <= 119)
            this.setFieldAtId("FR0315", "Rent", borrowerPairs[0]);
        }
        switch (id)
        {
          case "USDA.X108":
            this.setFieldAtId("BR0102", val, borrowerPairs[0]);
            break;
          case "USDA.X109":
            this.setFieldAtId("BR0105", val, borrowerPairs[0]);
            break;
          case "USDA.X110":
            this.setFieldAtId("BR0109", val, borrowerPairs[0]);
            break;
          case "USDA.X111":
            this.setFieldAtId("BR0110", val, borrowerPairs[0]);
            break;
          case "USDA.X112":
            this.setFieldAtId("BR0111", val, borrowerPairs[0]);
            break;
          case "USDA.X113":
            this.setFieldAtId("BR0118", val, borrowerPairs[0]);
            break;
          case "USDA.X114":
            this.setFieldAtId("BR0202", val, borrowerPairs[0]);
            break;
          case "USDA.X115":
            this.setFieldAtId("BR0205", val, borrowerPairs[0]);
            break;
          case "USDA.X116":
            this.setFieldAtId("BR0209", val, borrowerPairs[0]);
            break;
          case "USDA.X117":
            this.setFieldAtId("BR0210", val, borrowerPairs[0]);
            break;
          case "USDA.X118":
            this.setFieldAtId("BR0211", val, borrowerPairs[0]);
            break;
          case "USDA.X119":
            this.setFieldAtId("BR0218", val, borrowerPairs[0]);
            break;
        }
        for (int index = 108; index <= 119; ++index)
          this.setFieldAtId("USDA.X" + (object) index, "", borrowerPairs[0]);
        int numberOfResidence = this.GetNumberOfResidence(true);
        if (numberOfResidence == 0)
          return;
        bool flag1 = false;
        bool flag2 = false;
        for (int index = 1; index <= numberOfResidence; ++index)
        {
          if (this.GetFieldAt("BR" + index.ToString("00") + "15") == "Rent")
          {
            if (!flag1 && this.GetFieldAt("BR" + index.ToString("00") + "23") == "Current")
            {
              this.setFieldAtId("USDA.X108", this.GetFieldAt("BR" + index.ToString("00") + "02"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X109", this.GetFieldAt("BR" + index.ToString("00") + "05"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X110", this.GetFieldAt("BR" + index.ToString("00") + "09"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X111", this.GetFieldAt("BR" + index.ToString("00") + "10"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X112", this.GetFieldAt("BR" + index.ToString("00") + "11"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X113", this.GetFieldAt("BR" + index.ToString("00") + "18"), borrowerPairs[0]);
              flag1 = true;
            }
            if (!flag2 && this.GetFieldAt("BR" + index.ToString("00") + "23") == "Prior")
            {
              this.setFieldAtId("USDA.X114", this.GetFieldAt("BR" + index.ToString("00") + "02"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X115", this.GetFieldAt("BR" + index.ToString("00") + "05"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X116", this.GetFieldAt("BR" + index.ToString("00") + "09"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X117", this.GetFieldAt("BR" + index.ToString("00") + "10"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X118", this.GetFieldAt("BR" + index.ToString("00") + "11"), borrowerPairs[0]);
              this.setFieldAtId("USDA.X119", this.GetFieldAt("BR" + index.ToString("00") + "18"), borrowerPairs[0]);
              flag2 = true;
            }
          }
          if (flag1 & flag2)
            break;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot modify landlord information for USDA fields. Field ID: " + id + ", Error: " + ex.Message);
      }
      finally
      {
        if (this.currentBorrowerPair.Id != id1)
        {
          for (int index = 0; index < borrowerPairs.Length; ++index)
          {
            if (borrowerPairs[index].Id == id1)
            {
              this.SetBorrowerPair(borrowerPairs[index]);
              break;
            }
          }
        }
      }
    }

    private void createVOR(string id, BorrowerPair pair)
    {
      this.createVORVOE(id, "_RESIDENCE", "BorrowerResidencyType", "Current", "Prior", "_RESIDENCE[@BorrowerResidencyType=\"Prior\"]", pair);
    }

    private void createVOE(string id, BorrowerPair pair)
    {
      this.createVORVOE(id, "EMPLOYER", "EmploymentCurrentIndicator", "Y", "N", "EMPLOYER[@EmploymentCurrentIndicator=\"N\"]", pair);
    }

    private int getNumberOfVerification(bool borrower, string verName)
    {
      return borrower ? this.brwElm.SelectNodes(verName).Count : this.coBrwElm.SelectNodes(verName).Count;
    }

    private int getNumberOfPrevVerification(bool borrower, string verName)
    {
      int prevVerification = 0;
      if (borrower)
      {
        foreach (XmlElement selectNode in this.brwElm.SelectNodes(verName))
        {
          if ((selectNode.GetAttribute("EmploymentCurrentIndicator") ?? "") != "Y")
            ++prevVerification;
        }
      }
      else
      {
        foreach (XmlElement selectNode in this.coBrwElm.SelectNodes(verName))
        {
          if ((selectNode.GetAttribute("EmploymentCurrentIndicator") ?? "") != "Y")
            ++prevVerification;
        }
      }
      return prevVerification;
    }

    internal int GetNumberOfResidence(bool borrower)
    {
      return this.getNumberOfVerification(borrower, "_RESIDENCE");
    }

    private void RemoveVerificationAt(bool borrower, int i, string verName)
    {
      XmlNodeList xmlNodeList = borrower ? this.brwElm.SelectNodes(verName) : this.coBrwElm.SelectNodes(verName);
      try
      {
        XmlNode oldChild = xmlNodeList[i];
        XmlElement xmlElement = (XmlElement) oldChild.SelectSingleNode("EllieMae");
        if (xmlElement != null)
          this.logList.RemoveVerif(xmlElement.GetAttribute("_ID"));
        oldChild.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove verification " + (object) i + ", e: " + ex.Message);
      }
    }

    internal void RemoveResidenceAt(bool borrower, int i)
    {
      this.RemoveVerificationAt(borrower, i, "_RESIDENCE");
      this.createLoanlordInfo((string) null, (string) null);
    }

    private int NewVerification(
      bool borrower,
      bool current,
      string verName,
      string attr,
      string currentStr,
      string priorStr,
      string brwHeader,
      string coBrwHeader,
      string brwId)
    {
      XmlElement element = this.xmldoc.CreateElement(verName);
      Mapping.AddEntityId(element);
      if (current)
        element.SetAttribute(attr, currentStr);
      else
        element.SetAttribute(attr, priorStr);
      int count;
      if (borrower)
      {
        count = this.brwElm.SelectNodes(verName).Count;
        this.brwElm.AppendChild((XmlNode) element);
        string str1 = brwHeader;
        int num = count + 1;
        string str2 = num.ToString("00");
        string str3 = brwId;
        this.SetFieldAt(str1 + str2 + str3, "Borrower");
        string str4 = brwHeader;
        num = count + 1;
        string str5 = num.ToString("00");
        this.SetFieldAt(str4 + str5 + "99", this.generateNewId());
        string str6 = brwHeader;
        num = count + 1;
        string str7 = num.ToString("00");
        this.setDefaultVerfiSettings(str6 + str7);
      }
      else
      {
        count = this.coBrwElm.SelectNodes(verName).Count;
        this.coBrwElm.AppendChild((XmlNode) element);
        string str8 = coBrwHeader;
        int num = count + 1;
        string str9 = num.ToString("00");
        string str10 = brwId;
        this.SetFieldAt(str8 + str9 + str10, "CoBorrower");
        string str11 = coBrwHeader;
        num = count + 1;
        string str12 = num.ToString("00");
        this.SetFieldAt(str11 + str12 + "99", this.generateNewId());
        string str13 = coBrwHeader;
        num = count + 1;
        string str14 = num.ToString("00");
        this.setDefaultVerfiSettings(str13 + str14);
      }
      return count;
    }

    internal int NewResidence(bool borrower, bool current)
    {
      return this.NewVerification(borrower, current, "_RESIDENCE", "BorrowerResidencyType", "Current", "Prior", "BR", "CR", "13");
    }

    private int moveVerification(
      bool from,
      bool to,
      int index,
      string verName,
      string brwHeader,
      string coBrwHeader,
      string brwId)
    {
      XmlNodeList xmlNodeList = from ? this.brwElm.SelectNodes(verName) : this.coBrwElm.SelectNodes(verName);
      int num = 0;
      try
      {
        XmlNode oldChild = xmlNodeList[index - 1];
        XmlNode newChild = oldChild.ParentNode.RemoveChild(oldChild);
        if (to)
        {
          this.brwElm.AppendChild(newChild);
          num = this.brwElm.SelectNodes(verName).Count;
          this.SetFieldAt(brwHeader + num.ToString("00") + brwId, "Borrower");
        }
        else
        {
          this.coBrwElm.AppendChild(newChild);
          num = this.coBrwElm.SelectNodes(verName).Count;
          this.SetFieldAt(coBrwHeader + num.ToString("00") + brwId, "CoBorrower");
        }
        if (verName == "EMPLOYER")
          this.calculatePreviousGrossIncome();
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot move the Verification (" + from.ToString() + ", " + to.ToString() + ", " + (object) index + "), e" + ex.Message);
      }
      return num;
    }

    private void calculatePreviousGrossIncome()
    {
      string val1 = string.Empty;
      string val2 = string.Empty;
      int numberOfEmployer1 = this.GetNumberOfEmployer(true);
      for (int index = 1; index <= numberOfEmployer1; ++index)
      {
        if (this.GetFieldAt("BE" + index.ToString("00") + "09") == "N")
        {
          val1 = !(this.GetFieldAt("BE" + index.ToString("00") + "15") == "Y") ? this.GetFieldAt("BE" + index.ToString("00") + "12") : this.GetFieldAt("BE" + index.ToString("00") + "56");
          break;
        }
      }
      this.SetFieldAt("URLA.X245", val1);
      int numberOfEmployer2 = this.GetNumberOfEmployer(false);
      for (int index = 1; index <= numberOfEmployer2; ++index)
      {
        if (this.GetFieldAt("CE" + index.ToString("00") + "09") == "N")
        {
          val2 = !(this.GetFieldAt("CE" + index.ToString("00") + "15") == "Y") ? this.GetFieldAt("CE" + index.ToString("00") + "12") : this.GetFieldAt("CE" + index.ToString("00") + "56");
          break;
        }
      }
      this.SetFieldAt("URLA.X246", val2);
    }

    internal int MoveResidence(bool from, bool to, int index)
    {
      return this.moveVerification(from, to, index, "_RESIDENCE", "BR", "CR", "13");
    }

    internal int GetNumberOfEmployer(bool borrower)
    {
      return this.getNumberOfVerification(borrower, "EMPLOYER");
    }

    internal int GetNumberOfPrevEmployer(bool borrower)
    {
      return this.getNumberOfPrevVerification(borrower, "EMPLOYER");
    }

    internal void RemoveEmployerAt(bool borrower, int i)
    {
      this.RemoveVerificationAt(borrower, i, "EMPLOYER");
    }

    internal int NewEmployer(bool borrower, bool current)
    {
      return this.NewVerification(borrower, current, "EMPLOYER", "EmploymentCurrentIndicator", "Y", "N", "BE", "CE", "08");
    }

    internal int MoveEmployer(bool from, bool to, int index)
    {
      return this.moveVerification(from, to, index, "EMPLOYER", "BE", "CE", "08");
    }

    internal void CopyResidence()
    {
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      XmlNodeList xmlNodeList1 = this.coBrwElm.SelectNodes("_RESIDENCE");
      for (int i = 0; i < xmlNodeList1.Count; ++i)
      {
        XmlElement xmlElement1 = (XmlElement) xmlNodeList1[i];
        stringList1.Add(xmlElement1.GetAttribute("BorrowerResidencyBasisType"));
        XmlElement xmlElement2 = (XmlElement) xmlNodeList1[i].SelectSingleNode("EllieMae");
        if (xmlElement2 == null)
          stringList2.Add("");
        else
          stringList2.Add(xmlElement2.GetAttribute("Rent"));
        this.coBrwElm.RemoveChild(xmlNodeList1[i]);
      }
      XmlNodeList xmlNodeList2 = this.brwElm.SelectNodes("_RESIDENCE");
      if (xmlNodeList2.Count == 0)
        return;
      for (int index = 0; index < xmlNodeList2.Count; ++index)
      {
        XmlNode newChild = xmlNodeList2[index].CloneNode(true);
        XmlElement element1 = (XmlElement) newChild;
        element1.SetAttribute("_eid", Guid.NewGuid().ToString());
        int num = index + 1;
        XmlElement element2 = (XmlElement) newChild.SelectSingleNode("EllieMae");
        this.setFieldAtXpath(element2, "Person", "CR" + num.ToString("00") + "13", "CoBorrower", this.currentBorrowerPair);
        this.setFieldAtXpath(element2, "_ID", "CR" + num.ToString("00") + "99", this.generateNewId(), this.currentBorrowerPair);
        if (stringList1 != null && stringList1.Count > index)
          this.setFieldAtXpath(element1, "BorrowerResidencyBasisType", "CR" + num.ToString("00") + "15", stringList1[index], this.currentBorrowerPair);
        if (stringList2 != null && stringList2.Count > index)
          this.setFieldAtXpath(element2, "Rent", "CR" + num.ToString("00") + "16", stringList2[index], this.currentBorrowerPair);
        this.coBrwElm.AppendChild(newChild);
      }
    }

    private void UpVORVOE(
      bool borrower,
      bool current,
      int index,
      string currentStr,
      string priorStr,
      string predicate,
      string type)
    {
      XmlElement xmlElement = borrower ? this.brwElm : this.coBrwElm;
      string str = current ? currentStr : priorStr;
      try
      {
        XmlNodeList xmlNodeList = xmlElement.SelectNodes(predicate + str + "\"]");
        if (xmlNodeList.Count < 2)
          return;
        XmlNode selectNode = xmlElement.SelectNodes(type)[index];
        int i = 0;
        while (i < xmlNodeList.Count && selectNode != xmlNodeList[i])
          ++i;
        if (i == 0)
          return;
        XmlNode refChild = xmlNodeList[i - 1];
        selectNode.ParentNode.InsertBefore(selectNode, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpVORVOE, e: " + ex.Message);
      }
    }

    internal void UpResidence(bool borrower, bool current, int index)
    {
      this.UpVORVOE(borrower, current, index, "Current", "Prior", "_RESIDENCE[@BorrowerResidencyType=\"", "_RESIDENCE");
    }

    internal void UpEmployer(bool borrower, bool current, int index)
    {
      this.UpVORVOE(borrower, current, index, "Y", "N", "EMPLOYER[@EmploymentCurrentIndicator=\"", "EMPLOYER");
    }

    private void DownVORVOE(
      bool borrower,
      bool current,
      int index,
      string currentStr,
      string priorStr,
      string predicate,
      string type)
    {
      XmlElement xmlElement = borrower ? this.brwElm : this.coBrwElm;
      string str = current ? currentStr : priorStr;
      try
      {
        XmlNodeList xmlNodeList = xmlElement.SelectNodes(predicate + str + "\"]");
        if (xmlNodeList.Count < 2)
          return;
        XmlNode selectNode = xmlElement.SelectNodes(type)[index];
        int i = 0;
        while (i < xmlNodeList.Count && selectNode != xmlNodeList[i])
          ++i;
        if (i == xmlNodeList.Count - 1)
          return;
        XmlNode refChild = xmlNodeList[i + 1];
        selectNode.ParentNode.InsertAfter(selectNode, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownVORVOE, e: " + ex.Message);
      }
    }

    internal void DownResidence(bool borrower, bool current, int index)
    {
      this.DownVORVOE(borrower, current, index, "Current", "Prior", "_RESIDENCE[@BorrowerResidencyType=\"", "_RESIDENCE");
    }

    internal void DownEmployer(bool borrower, bool current, int index)
    {
      this.DownVORVOE(borrower, current, index, "Y", "N", "EMPLOYER[@EmploymentCurrentIndicator=\"", "EMPLOYER");
    }

    private bool IsBorrowerDependent(string id)
    {
      string xpath = this[id].Xpath;
      return xpath.StartsWith("ASSET") || xpath.StartsWith("LIABILITY") || xpath.StartsWith("REO_PROPERTY");
    }

    internal void SetOrgFieldAt(string id, string val)
    {
      Tracing.Log(Mapping.sw, TraceLevel.Info, nameof (Mapping), "*** setorgfield(original value of the locked field)[ " + id + ", " + val + "]");
      bool flag = this.IsBorrowerDependent(id);
      string str = flag ? "LOCK/FIELD[@id=\"" + id + "\"]" + this.brwPredicate : "LOCK/FIELD[@id=\"" + id + "\"]";
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode(str);
      if (newChild == null)
      {
        if (this.lockRoot == null)
        {
          this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
          if (this.lockRoot == null)
            this.root.AppendChild((XmlNode) this.xmldoc.CreateElement("LOCK"));
        }
        newChild = this.xmldoc.CreateElement("FIELD");
        this.lockRoot.AppendChild((XmlNode) newChild);
        newChild.SetAttribute(nameof (id), id);
        if (flag)
          newChild.SetAttribute("BorrowerID", this.brwId);
      }
      newChild.SetAttribute(nameof (val), val);
      this.cachedOrgFields[str] = val;
    }

    internal void SetOrgFieldAt(string id, string val, BorrowerPair pair)
    {
      bool flag = this.IsBorrowerDependent(id);
      string str = flag ? "LOCK/FIELD[@id=\"" + id + "\"]" + this.brwPredicate : "LOCK/FIELD[@id=\"" + id + "\"]";
      XmlElement newChild = (XmlElement) this.root.SelectSingleNode(str);
      if (newChild == null)
      {
        if (this.lockRoot == null)
        {
          this.lockRoot = (XmlElement) this.root.SelectSingleNode("LOCK");
          if (this.lockRoot == null)
            this.root.AppendChild((XmlNode) this.xmldoc.CreateElement("LOCK"));
        }
        newChild = this.xmldoc.CreateElement("FIELD");
        this.lockRoot.AppendChild((XmlNode) newChild);
        newChild.SetAttribute(nameof (id), id);
        if (flag)
          newChild.SetAttribute("BorrowerID", pair.Borrower.Id);
      }
      newChild.SetAttribute(nameof (val), val);
      this.cachedOrgFields[str] = val;
    }

    internal string GetOrgFieldAt(string id)
    {
      string str = this.IsBorrowerDependent(id) ? "LOCK/FIELD[@id=\"" + id + "\"]" + this.brwPredicate : "LOCK/FIELD[@id=\"" + id + "\"]";
      if (this.cachedOrgFields.ContainsKey(str))
        return this.cachedOrgFields[str];
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(str);
      string orgFieldAt = (string) null;
      if (xmlElement != null)
        orgFieldAt = xmlElement.GetAttribute("val");
      this.cachedOrgFields[str] = orgFieldAt;
      return orgFieldAt;
    }

    internal string GetOrgFieldAt(string id, BorrowerPair pair)
    {
      bool flag = this.IsBorrowerDependent(id);
      string str = flag ? "LOCK/FIELD[@id=\"" + id + "\"]" + this.brwPredicate : "LOCK/FIELD[@id=\"" + id + "\"]";
      if (flag)
        str = str.Replace(this.brwId, pair.Borrower.Id);
      if (this.cachedOrgFields.ContainsKey(str))
        return this.cachedOrgFields[str];
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(str);
      string orgFieldAt = (string) null;
      if (xmlElement != null)
        orgFieldAt = xmlElement.GetAttribute("val");
      this.cachedOrgFields[str] = orgFieldAt;
      return orgFieldAt;
    }

    internal void RemoveLockAt(string id)
    {
      string str = this.IsBorrowerDependent(id) ? "LOCK/FIELD[@id=\"" + id + "\"]" + this.brwPredicate : "LOCK/FIELD[@id=\"" + id + "\"]";
      this.cachedOrgFields.Remove(str);
      XmlNode oldChild = this.root.SelectSingleNode(str);
      if (oldChild == null)
        return;
      this.lockRoot.RemoveChild(oldChild);
    }

    internal bool RemoveLockCacheAt(string id)
    {
      return this.cachedOrgFields.Remove(this.IsBorrowerDependent(id) ? "LOCK/FIELD[@id=\"" + id + "\"]" + this.brwPredicate : "LOCK/FIELD[@id=\"" + id + "\"]");
    }

    internal LogList GetLogList(LoanData loan)
    {
      if (this.logList == null)
        this.logList = this.readLogList(loan);
      return this.logList;
    }

    internal void ReplaceSystemID(LoanData loan, string systemId)
    {
      if (this.systemId == systemId)
        return;
      this.systemId = systemId;
      this.logList = this.readLogList(loan, systemId);
    }

    internal LogList ResetLogList(LoanData loan)
    {
      return this.logList = new LogList(loan, this.loanSettings.MilestoneDateCalculator);
    }

    internal void RemoveDocTrackLink(string linkID)
    {
      string empty = string.Empty;
      int num = 0;
      string str1 = string.Empty;
      string str2 = string.Empty;
      for (int index1 = 1; index1 <= 7; ++index1)
      {
        switch (index1)
        {
          case 1:
            num = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
            str1 = "FL";
            str2 = "99";
            break;
          case 2:
            num = this.GetNumberOfDeposits();
            str1 = "DD";
            str2 = "35";
            break;
          case 3:
            num = this.GetNumberOfEmployer(true);
            str1 = "BE";
            str2 = "99";
            break;
          case 4:
            num = this.GetNumberOfEmployer(false);
            str1 = "CE";
            str2 = "99";
            break;
          case 5:
            num = this.GetNumberOfMortgages();
            str1 = "FM";
            str2 = "43";
            break;
          case 6:
            num = this.GetNumberOfResidence(true);
            str1 = "BR";
            str2 = "99";
            break;
          case 7:
            num = this.GetNumberOfResidence(false);
            str1 = "CR";
            str2 = "99";
            break;
        }
        for (int index2 = 1; index2 <= num; ++index2)
        {
          if (this.GetFieldAt(str1 + index2.ToString("00") + str2) == linkID)
          {
            this.SetFieldAt(str1 + index2.ToString("00") + "97", "Y");
            return;
          }
        }
      }
    }

    internal void ReplaceCachedXML() => this.ReplaceCachedXML((XmlDocument) null);

    internal void ReplaceCachedXML(XmlDocument newCachedXml)
    {
      if (newCachedXml == null)
        this.UpdateCachedXmlFromxmlDoc(this.xmldoc);
      else
        this.UpdateCachedXmlFromxmlDoc(newCachedXml);
      foreach (Field field in (IEnumerable) this.fields.Values)
      {
        field.CachedValue = (string) null;
        field.CachedElement = (XmlElement) null;
      }
      this.cachedElements = (Dictionary<string, XmlElement>) null;
    }

    internal XmlDocument MergeChanges(
      LoanContentAccess access,
      bool includeOperationalLog,
      bool updateCachedXmlForFullAccess = true)
    {
      this.saveLogList(includeOperationalLog);
      if (access != LoanContentAccess.FullAccess)
        return this.mergeChangesIntoCachedDocument(access);
      if (updateCachedXmlForFullAccess)
        this.UpdateCachedXmlFromxmlDoc(this.xmldoc);
      return this.xmldoc;
    }

    private XmlDocument mergeChangesIntoCachedDocument(LoanContentAccess access)
    {
      XmlDocument xmlDocument = new XmlDocument();
      this.LoadXmlDocFromCachedXml(xmlDocument);
      xmlDocument.DocumentElement.SetAttribute("LoanFileSequenceNumber", string.Concat((object) this.LoanVersionNumber));
      this.transferLogRecords(this.xmldoc, xmlDocument, SystemLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, StatusOnlineLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, HtmlEmailLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, EDMLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, PrintLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, ExportLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, CRMLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, DataTracLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, GetIndexLog.XmlType);
      if (access == LoanContentAccess.None)
      {
        this.UpdateCachedXmlFromxmlDoc(xmlDocument);
        return xmlDocument;
      }
      this.transferLogRecords(this.xmldoc, xmlDocument, DocumentLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, VerifLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, PreliminaryConditionLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, UnderwritingConditionLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, PostClosingConditionLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, SellConditionLog.XmlType);
      this.transferLogRecords(this.xmldoc, xmlDocument, DownloadLog.XmlType);
      if ((access & LoanContentAccess.ConversationLog) != LoanContentAccess.None)
        this.transferLogRecords(this.xmldoc, xmlDocument, ConversationLog.XmlType);
      if ((access & LoanContentAccess.Task) != LoanContentAccess.None)
        this.transferLogRecords(this.xmldoc, xmlDocument, MilestoneTaskLog.XmlType);
      if ((access & LoanContentAccess.ProfitManagement) != LoanContentAccess.None)
        this.transferNodes(this.xmldoc, xmlDocument, "LOAN_APPLICATION/EllieMae", "PROFIT_MANAGEMENT");
      if ((access & LoanContentAccess.LockRequest) != LoanContentAccess.None)
      {
        this.transferLogRecords(this.xmldoc, xmlDocument, LockRequestLog.XmlType);
        this.transferLogRecords(this.xmldoc, xmlDocument, LockConfirmLog.XmlType);
        this.transferLogRecords(this.xmldoc, xmlDocument, LockDenialLog.XmlType);
        this.transferLogRecords(this.xmldoc, xmlDocument, LockVoidLog.XmlType);
        this.transferNodes(this.xmldoc, xmlDocument, "LOAN_APPLICATION/EllieMae", "RateLock");
        this.transferNodes(this.xmldoc, xmlDocument, "LOAN_APPLICATION/EllieMae", "OptimalBlue");
      }
      if ((access & LoanContentAccess.DisclosureTracking) != LoanContentAccess.None || (access & LoanContentAccess.DisclosureTrackingViewOnly) != LoanContentAccess.None)
      {
        this.transferLogRecords(this.xmldoc, xmlDocument, DisclosureTrackingLog.XmlType);
        this.transferLogRecords(this.xmldoc, xmlDocument, DisclosureTracking2015Log.XmlType);
      }
      this.transferLogRecords(this.xmldoc, xmlDocument, GoodFaithFeeVarianceCureLog.XmlType);
      this.UpdateCachedXmlFromxmlDoc(xmlDocument);
      return xmlDocument;
    }

    private void transferNodes(
      XmlDocument source,
      XmlDocument target,
      string parentPath,
      string nodePath)
    {
      XmlElement xmlElement1 = (XmlElement) source.SelectSingleNode(parentPath);
      XmlElement xmlElement2 = (XmlElement) target.SelectSingleNode(parentPath);
      XmlNodeList xmlNodeList1 = xmlElement2.SelectNodes(nodePath);
      for (int i = 0; i < xmlNodeList1.Count; ++i)
        xmlElement2.RemoveChild(xmlNodeList1[i]);
      XmlNodeList xmlNodeList2 = xmlElement1.SelectNodes(nodePath);
      for (int i = 0; i < xmlNodeList2.Count; ++i)
        xmlElement2.AppendChild(target.ImportNode(xmlNodeList2[i], true));
    }

    private void transferLogRecords(XmlDocument source, XmlDocument target, string logType)
    {
      if (logType == DocumentLog.XmlType || logType == VerifLog.XmlType || logType == PreliminaryConditionLog.XmlType || logType == UnderwritingConditionLog.XmlType || logType == SellConditionLog.XmlType || logType == PostClosingConditionLog.XmlType || logType == PurchaseConditionLog.XmlType || logType == MilestoneTaskLog.XmlType)
        this.transferNodes(source, target, "LOAN_APPLICATION/EllieMae/SystemLog[@SysID='" + this.systemId + "']", "Record[@Type='" + logType + "']");
      else if (logType == CRMLog.XmlType)
        this.transferNodes(source, target, "LOAN_APPLICATION/EllieMae/SystemLog[@SysID='" + this.systemId + "']", "CRM");
      else
        this.transferNodes(source, target, "LOAN_APPLICATION/EllieMae/LOG", "Record[@Type='" + logType + "']");
    }

    private void saveLogList(bool includeOperationalLog)
    {
      if (this.logList != null)
      {
        XmlElement nonSystemLogRoot = this.GetNonSystemLogRoot();
        nonSystemLogRoot.ParentNode.RemoveChild((XmlNode) nonSystemLogRoot);
        XmlElement systemLogRoot = this.GetSystemLogRoot(this.systemId);
        systemLogRoot.ParentNode.RemoveChild((XmlNode) systemLogRoot);
        this.logList.ToXml(this.GetSystemLogRoot(this.systemId), this.GetNonSystemLogRoot(), includeOperationalLog);
      }
      this.root = this.xmldoc.DocumentElement;
    }

    private LogList readLogList(LoanData data)
    {
      return new LogList(data, this.systemId, this.GetSystemLogRoot(this.systemId), this.GetNonSystemLogRoot(), this.loanSettings.MilestoneDateCalculator);
    }

    private LogList readLogList(LoanData data, string systemId)
    {
      return new LogList(data, systemId, this.GetSystemLogRoot(systemId), this.GetNonSystemLogRoot(), this.loanSettings.MilestoneDateCalculator);
    }

    private string GetAttr(XmlElement elm, string attr) => elm.GetAttribute(attr) ?? string.Empty;

    private DateTime GetDate(XmlElement elm, string attr)
    {
      string attribute = elm.GetAttribute(attr);
      return attribute != null && !(attribute == string.Empty) ? DateTime.Parse(attribute) : DateTime.MinValue;
    }

    private int GetInt(XmlElement elm, string attr)
    {
      string attribute = elm.GetAttribute(attr);
      return attribute != null && !(attribute == string.Empty) ? int.Parse(attribute) : 0;
    }

    internal void CopyCurrentLoanScenarioToAlternative(int altNo)
    {
      XmlElement emRoot = (XmlElement) this.root.SelectSingleNode("EllieMae");
      XmlElement refChild = (XmlElement) this.root.SelectSingleNode("EllieMae/CLOSING_COST");
      XmlNode xmlNode = this.root.SelectSingleNode("EllieMae/CLOSING_COST[2]");
      this.SetFieldAt("1776", this.GetFieldAt("1093"));
      this.SetFieldAt("1780", this.GetFieldAt("230"));
      this.SetFieldAt("1781", this.GetFieldAt("232"));
      this.SetFieldAt("1805", this.GetFieldAt("1785"));
      if (refChild != null)
      {
        XmlElement newChild = (XmlElement) refChild.CloneNode(true);
        if (altNo == 1)
        {
          if (xmlNode != null)
            emRoot.RemoveChild(xmlNode);
          emRoot.InsertBefore((XmlNode) newChild, (XmlNode) refChild);
        }
        else
        {
          if (xmlNode == null)
          {
            xmlNode = (XmlNode) this.xmldoc.CreateElement("CLOSING_COST");
            emRoot.InsertAfter(xmlNode, (XmlNode) refChild);
          }
          else
          {
            XmlNode oldChild = this.root.SelectSingleNode("EllieMae/CLOSING_COST[3]");
            if (oldChild != null)
              emRoot.RemoveChild(oldChild);
          }
          emRoot.InsertAfter((XmlNode) newChild, xmlNode);
        }
      }
      this.createLoanProgramTrees(emRoot, altNo);
      foreach (string templateField in LoanProgram.TemplateFields)
      {
        string comparisonFieldId = LoanProgram.GetLoanComparisonFieldID(templateField, altNo);
        try
        {
          if (comparisonFieldId != "")
            this.SetFieldAt(comparisonFieldId, this.GetFieldAt(templateField));
        }
        catch (Exception ex)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "(CopyCurrentLoanScenarioToAlternative) loancompid = " + comparisonFieldId + "  id = " + templateField + "  Error: " + ex.Message);
        }
      }
    }

    internal void SwapLoanScenario(int altNo)
    {
      XmlElement emRoot = (XmlElement) this.root.SelectSingleNode("EllieMae");
      this.SetFieldAt("1776", this.GetFieldAt("1093"));
      this.SetFieldAt("1780", this.GetFieldAt("230"));
      this.SetFieldAt("1781", this.GetFieldAt("232"));
      this.SetFieldAt("1805", this.GetFieldAt("1785"));
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/CLOSING_COST");
      if (altNo == 1)
      {
        XmlElement element;
        if (xmlNodeList.Count == 1)
        {
          element = this.xmldoc.CreateElement("CLOSING_COST");
        }
        else
        {
          emRoot.RemoveChild(xmlNodeList[1]);
          element = (XmlElement) xmlNodeList[1];
        }
        emRoot.InsertBefore((XmlNode) element, xmlNodeList[0]);
      }
      else if (xmlNodeList.Count == 1)
      {
        XmlElement element1 = this.xmldoc.CreateElement("CLOSING_COST");
        emRoot.InsertBefore((XmlNode) element1, xmlNodeList[0]);
        XmlElement element2 = this.xmldoc.CreateElement("CLOSING_COST");
        emRoot.InsertBefore((XmlNode) element2, xmlNodeList[0]);
      }
      else if (xmlNodeList.Count == 2)
      {
        XmlElement element = this.xmldoc.CreateElement("CLOSING_COST");
        emRoot.InsertBefore((XmlNode) element, xmlNodeList[0]);
        emRoot.RemoveChild(xmlNodeList[0]);
        emRoot.InsertAfter(xmlNodeList[0], xmlNodeList[1]);
      }
      else
      {
        emRoot.RemoveChild(xmlNodeList[0]);
        emRoot.RemoveChild(xmlNodeList[2]);
        emRoot.InsertBefore(xmlNodeList[2], xmlNodeList[1]);
        emRoot.InsertAfter(xmlNodeList[0], xmlNodeList[1]);
      }
      this.SetFieldAt("1093", this.GetFieldAt("1776"));
      this.SetFieldAt("230", this.GetFieldAt("1780"));
      this.SetFieldAt("232", this.GetFieldAt("1781"));
      this.createLoanProgramTrees(emRoot, altNo);
      foreach (string allowedFieldId in new LoanProgram().GetAllowedFieldIDs())
      {
        string comparisonFieldId = LoanProgram.GetLoanComparisonFieldID(allowedFieldId, altNo);
        string fieldAt = this.GetFieldAt(comparisonFieldId);
        this.SetFieldAt(comparisonFieldId, this.GetFieldAt(allowedFieldId));
        this.SetFieldAt(allowedFieldId, fieldAt);
      }
    }

    private void createLoanProgramTrees(XmlElement emRoot, int altNo)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/LOAN_PROGRAM");
      if (altNo == 1)
      {
        if (xmlNodeList.Count != 0)
          return;
        XmlElement element = this.xmldoc.CreateElement("LOAN_PROGRAM");
        Mapping.AddEntityId(element);
        emRoot.AppendChild((XmlNode) element);
      }
      else if (xmlNodeList.Count == 0)
      {
        XmlElement element1 = this.xmldoc.CreateElement("LOAN_PROGRAM");
        Mapping.AddEntityId(element1);
        emRoot.AppendChild((XmlNode) element1);
        XmlElement element2 = this.xmldoc.CreateElement("LOAN_PROGRAM");
        Mapping.AddEntityId(element2);
        emRoot.InsertAfter((XmlNode) element2, (XmlNode) element1);
      }
      else
      {
        if (xmlNodeList.Count != 1)
          return;
        XmlElement element = this.xmldoc.CreateElement("LOAN_PROGRAM");
        Mapping.AddEntityId(element);
        emRoot.InsertAfter((XmlNode) element, xmlNodeList[0]);
      }
    }

    internal void ClearAlternative(int altNo)
    {
      this.root.SelectSingleNode("EllieMae/CLOSING_COST" + ("[" + (altNo + 1).ToString() + "]"))?.RemoveAll();
      this.root.SelectSingleNode("EllieMae/LOAN_PROGRAM" + ("[" + altNo.ToString() + "]"))?.RemoveAll();
    }

    private double toDouble(string val)
    {
      return val != null && !(val == string.Empty) ? double.Parse(val) : 0.0;
    }

    internal string[] GetFormListTemplate()
    {
      string[] formListTemplate1 = new string[0];
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/FormList[@SysID='" + this.systemId + "']/Form");
      if (xmlNodeList == null)
        return formListTemplate1;
      string[] formListTemplate2 = new string[xmlNodeList.Count];
      int index = -1;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        if (xmlElement != null && xmlElement.HasAttribute("html"))
        {
          string attribute = xmlElement.GetAttribute("html");
          if (attribute != string.Empty && attribute != null)
          {
            ++index;
            formListTemplate2[index] = attribute;
          }
        }
      }
      return formListTemplate2;
    }

    internal bool SetFormListTemplate(ArrayList newList)
    {
      this.root.SelectSingleNode("EllieMae/FormList[@SysID='" + this.systemId + "']")?.RemoveAll();
      if (newList == null)
        return true;
      XmlNode path = (XmlNode) this.createPath("EllieMae/FormList");
      XmlAttribute attribute = this.xmldoc.CreateAttribute("SysID");
      attribute.Value = this.systemId;
      path.Attributes.Append(attribute);
      if (path == null)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "(SetFormListTemplate) Can't create EllieMae/FormList node");
        return false;
      }
      bool flag = false;
      try
      {
        for (int index = 0; index < newList.Count; ++index)
        {
          string str = (string) newList[index];
          if (!(str == "-") || flag)
          {
            XmlElement element = this.xmldoc.CreateElement("Form");
            path.AppendChild((XmlNode) element);
            if (str.ToUpper() == "CREDIT DISCLOSURE")
              str = "FACT Act Disclosure";
            element.SetAttribute("html", str);
            if (!flag)
              flag = true;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "(SetFormListTemplate) Can't add form list to EllieMae/FormList node. Message: " + ex.Message);
        return false;
      }
      return true;
    }

    public void ClearOtherIncomeItems()
    {
      string xpath1 = this["101"].Xpath;
      string xpath2 = xpath1.Substring(0, xpath1.LastIndexOf("[@")) + "[not(@IncomeType=\"Base\") and not(@IncomeType=\"Overtime\") and not(@IncomeType=\"Bonus\") and not(@IncomeType=\"Commissions\") and not(@IncomeType=\"DividendsInterest\") and not(@IncomeType=\"NetRentalIncome\") and not(@IncomeType=\"OtherTypesOfIncome\")]";
      XmlNode xmlNode1 = this.root.SelectSingleNode(xpath2.Substring(0, xpath2.LastIndexOf("/CURRENT")));
      XmlNodeList xmlNodeList1 = this.root.SelectNodes(xpath2);
      for (int i = 0; i < xmlNodeList1.Count; ++i)
        xmlNode1.RemoveChild(xmlNodeList1[i]);
      string xpath3 = this["110"].Xpath;
      string xpath4 = xpath3.Substring(0, xpath3.LastIndexOf("[@")) + "[not(@IncomeType=\"Base\") and not(@IncomeType=\"Overtime\") and not(@IncomeType=\"Bonus\") and not(@IncomeType=\"Commissions\") and not(@IncomeType=\"DividendsInterest\") and not(@IncomeType=\"NetRentalIncome\") and not(@IncomeType=\"OtherTypesOfIncome\")]";
      XmlNode xmlNode2 = this.root.SelectSingleNode(xpath4.Substring(0, xpath4.LastIndexOf("/CURRENT")));
      XmlNodeList xmlNodeList2 = this.root.SelectNodes(xpath4);
      for (int i = 0; i < xmlNodeList2.Count; ++i)
        xmlNode2.RemoveChild(xmlNodeList2[i]);
    }

    public void AddOtherIncome(string desc, string amount, bool isForBorrower)
    {
      string str1 = !isForBorrower ? this["110"].Xpath : this["101"].Xpath;
      string str2 = str1.Substring(0, str1.LastIndexOf("[@")) + "[not(@IncomeType=\"Base\") and not(@IncomeType=\"Overtime\") and not(@IncomeType=\"Bonus\") and not(@IncomeType=\"Commissions\") and not(@IncomeType=\"DividendsInterest\") and not(@IncomeType=\"NetRentalIncome\") and not(@IncomeType=\"OtherTypesOfIncome\")]";
      this.root.SelectSingleNode(str2.Substring(0, str2.LastIndexOf("/CURRENT"))).AppendChild((XmlNode) this.createIncomeElement(desc, amount));
    }

    private XmlElement createIncomeElement(string desc, string amount)
    {
      XmlElement element = this.xmldoc.CreateElement("CURRENT_INCOME");
      element.SetAttribute("IncomeType", desc);
      element.SetAttribute("_MonthlyTotalAmount", amount);
      return element;
    }

    internal string EMXMLVersionID
    {
      get => this.root != null ? this.root.GetAttribute(nameof (EMXMLVersionID)) : "";
      set
      {
        if (value == null || !(value != ""))
          return;
        this.root.SetAttribute(nameof (EMXMLVersionID), value);
      }
    }

    internal string EMXMLVersionID_LDM
    {
      get => this.root != null ? this.root.GetAttribute(nameof (EMXMLVersionID_LDM)) : "";
      set
      {
        if (value == null || !(value != ""))
          return;
        this.root.SetAttribute(nameof (EMXMLVersionID_LDM), value);
      }
    }

    internal int LoanVersionNumber
    {
      get
      {
        if (this.root == null)
          return 0;
        string attribute = this.root.GetAttribute("LoanFileSequenceNumber");
        return !string.IsNullOrEmpty(attribute) ? int.Parse(attribute) : 0;
      }
      set
      {
        if (value < 0)
          return;
        this.root.SetAttribute("LoanFileSequenceNumber", value.ToString());
      }
    }

    internal bool TPOConnectStatus
    {
      get
      {
        if (this.root == null)
          return false;
        string attribute = this.root.GetAttribute(nameof (TPOConnectStatus));
        return attribute != null && !string.IsNullOrEmpty(attribute) && attribute == "Y";
      }
      set => this.root.SetAttribute(nameof (TPOConnectStatus), value ? "Y" : "N");
    }

    internal void RemoveLicenseNodes()
    {
      XmlElement xmlElement1 = (XmlElement) this.root.SelectSingleNode("_CLOSING_DOCUMENTS/EllieMae/StateLicense");
      xmlElement1?.RemoveAll();
      XmlElement xmlElement2 = (XmlElement) this.root.SelectSingleNode("_CLOSING_DOCUMENTS/EllieMae/LoanOfficerAllowed");
      xmlElement2?.RemoveAll();
      if (xmlElement1 == null && xmlElement2 == null)
        return;
      string[] states = Utils.GetStates();
      for (int index = 0; index < states.Length; ++index)
      {
        if (xmlElement2 != null)
        {
          Field fieldObject = this.GetFieldObject("LO.ALLOWED." + states[index], this.currentBorrowerPair);
          if (fieldObject != null && fieldObject.CachedValue != null)
            fieldObject.CachedValue = (string) null;
          else
            continue;
        }
        if (xmlElement1 != null)
        {
          Field fieldObject = this.GetFieldObject("LIC." + states[index], this.currentBorrowerPair);
          if (fieldObject != null && fieldObject.CachedValue != null)
            fieldObject.CachedValue = (string) null;
        }
      }
    }

    internal HelocRateTable GetDrawRepayPeriod()
    {
      HelocRateTable drawRepayPeriod = new HelocRateTable();
      string str = "DrawPeriod/Draw";
      string periodType = "Draw";
      for (int index = 1; index <= 2; ++index)
      {
        if (index == 2)
        {
          str = "RepaymentPeriod/Repayment";
          periodType = "Repayment";
        }
        XmlNodeList xmlNodeList = this.root.SelectNodes("LOAN_PRODUCT_DATA/HELOC/EllieMae/" + str);
        if (xmlNodeList != null)
        {
          try
          {
            foreach (XmlElement xmlElement in xmlNodeList)
            {
              string attribute1 = xmlElement.GetAttribute("Year");
              string attribute2 = xmlElement.GetAttribute("IndexRate");
              string attribute3 = xmlElement.GetAttribute("MarginRate");
              string attribute4 = xmlElement.GetAttribute("APR");
              string attribute5 = xmlElement.GetAttribute("MinimumMonthlyPayment");
              drawRepayPeriod.InsertYearRecord(attribute1, periodType, attribute2, attribute3, attribute4, attribute5);
            }
          }
          catch (Exception ex)
          {
            Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "GetDrawRepayPeriod: " + str + " invalid. Exception: " + ex.Message);
          }
        }
        else
          break;
      }
      return drawRepayPeriod;
    }

    internal int GetNumberOfHelocHistoricalIndices()
    {
      return this.root.SelectNodes("LOAN_PRODUCT_DATA/HELOC/EllieMae/HistoricalIndexSetting/YearSetting").Count;
    }

    internal bool SetDrawRepayPeriod(HelocRateTable helocTable)
    {
      try
      {
        this.root.SelectSingleNode("LOAN_PRODUCT_DATA/HELOC/EllieMae/DrawPeriod")?.RemoveAll();
        this.root.SelectSingleNode("LOAN_PRODUCT_DATA/HELOC/EllieMae/RepaymentPeriod")?.RemoveAll();
        int historicalIndices = this.GetNumberOfHelocHistoricalIndices();
        this.root.SelectSingleNode("LOAN_PRODUCT_DATA/HELOC/EllieMae/HistoricalIndexSetting")?.RemoveAll();
        if (historicalIndices > 0)
        {
          for (int index = 0; index < historicalIndices; ++index)
            this.clearRepeatableFieldCacheValues("HHI", index + 1, 1, 2, (List<int>) null, (List<int>) null, true);
        }
        if (helocTable != null && !helocTable.IsNewHELOC)
        {
          this.SetFieldAt("1889", "");
          this.SetFieldAt("1890", "");
        }
        this.SetFieldAt("1959", "");
        this.SetFieldAt("HHI.X1", "");
        this.SetFieldAt("HHI.X2", "");
        this.SetFieldAt("HHI.X3", "");
        this.SetFieldAt("HHI.X4", "");
        this.SetFieldAt("HHI.X5", "");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "SetDrawRepayPeriod: Can't remove draw/repayment section. Exception: " + ex.Message);
        return false;
      }
      if (helocTable != null)
      {
        string str1 = "DrawPeriod";
        string str2 = "Draw";
        string str3 = "HistoricalIndexSetting/YearSetting";
        int num1 = 0;
        int num2 = 0;
        this.SetFieldAt("HHI.X5", helocTable.DecimalsUseForIndex);
        if (helocTable.IsNewHELOC)
        {
          this.SetFieldAt("HHI.X1", helocTable.IndexDay.ToString());
          this.SetFieldAt("HHI.X2", helocTable.IndexMonth.ToString());
          this.SetFieldAt("HHI.X3", helocTable.DefaultHistoricMargin.ToString());
          this.SetFieldAt("HHI.X4", helocTable.UseAlternateSchedule ? "Y" : "N");
          this.SetFieldAt("1959", helocTable.IndexName.ToString());
          int num3 = 0;
          for (int i = 0; i < helocTable.Count; ++i)
          {
            HelocRateTable.YearRecord yearRecordAt = helocTable.GetYearRecordAt(i);
            ++num3;
            string str4 = "HHI" + num3.ToString("00");
            try
            {
              Mapping.AddEntityId(this.createPath("LOAN_PRODUCT_DATA/HELOC/EllieMae/" + str3 + "[" + num3.ToString() + "]"));
              this.SetFieldAt(str4 + "01", yearRecordAt.Year);
              this.SetFieldAt(str4 + "02", yearRecordAt.IndexRate);
            }
            catch (Exception ex)
            {
              Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "SetHistoricalTableIndex: Can't set Historical Index section. Exception: " + ex.Message);
              return false;
            }
          }
        }
        else
        {
          for (int index = 1; index <= 2; ++index)
          {
            if (index == 2)
            {
              str1 = "RepaymentPeriod";
              str2 = "Repayment";
            }
            int num4 = 0;
            string str5 = str1 == "RepaymentPeriod" ? "HTR" : "HTD";
            for (int i = 0; i < helocTable.Count; ++i)
            {
              HelocRateTable.YearRecord yearRecordAt = helocTable.GetYearRecordAt(i);
              if (!(yearRecordAt.PeriodType != str2))
              {
                ++num4;
                str5 += num4.ToString("00");
                try
                {
                  XmlElement path = this.createPath("LOAN_PRODUCT_DATA/HELOC/EllieMae/" + str1 + "/" + str2 + "[" + num4.ToString() + "]");
                  Mapping.AddEntityId(path);
                  this.setFieldAtXpath(path, "Year", str5 + "01", yearRecordAt.Year, this.currentBorrowerPair);
                  this.setFieldAtXpath(path, "IndexRate", str5 + "02", yearRecordAt.IndexRate, this.currentBorrowerPair);
                  this.setFieldAtXpath(path, "MarginRate", str5 + "03", yearRecordAt.MarginRate, this.currentBorrowerPair);
                  this.setFieldAtXpath(path, "APR", str5 + "04", yearRecordAt.APR, this.currentBorrowerPair);
                  this.setFieldAtXpath(path, "MinimumMonthlyPayment", str5 + "05", yearRecordAt.MinimumPayment, this.currentBorrowerPair);
                  if (index == 1)
                    num1 += 12;
                  else
                    num2 += 12;
                }
                catch (Exception ex)
                {
                  Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "SetDrawRepayPeriod: Can't set draw/repayment section. Exception: " + ex.Message);
                  return false;
                }
              }
            }
          }
          this.SetFieldAt("1889", num1.ToString());
          this.SetFieldAt("1890", num2.ToString());
        }
      }
      return true;
    }

    public void SetCreatedWithoutLoanNumber()
    {
      if (this.root.SelectSingleNode("EllieMae/CreatedWithoutLoanNumber") != null)
        return;
      XmlElement element1 = this.xmldoc.CreateElement("EllieMae");
      XmlElement element2 = this.xmldoc.CreateElement("CreatedWithoutLoanNumber");
      element1.AppendChild((XmlNode) element2);
      this.root.AppendChild((XmlNode) element1);
    }

    public void UnsetCreatedWithoutLoanNumber()
    {
      XmlNode oldChild = this.root.SelectSingleNode("EllieMae/CreatedWithoutLoanNumber");
      oldChild?.ParentNode.RemoveChild(oldChild);
    }

    public bool IsCreatedWithoutLoanNumber()
    {
      return this.root.SelectSingleNode("EllieMae/CreatedWithoutLoanNumber") != null;
    }

    public bool IsFieldDefined(string fieldId)
    {
      return this.fields.ContainsKey((object) fieldId.ToUpper());
    }

    [Obsolete]
    public string GetCurrentCachedXml() => this.cachedXml.ToString();

    public void ActivateAlert(string alertId, string userId, DateTime timestamp)
    {
      XmlElement alertElement = this.getAlertElement(alertId, true);
      alertElement.SetAttribute("ActivationUser", userId);
      alertElement.SetAttribute("ActivationDate", timestamp.ToString("M/d/yyyy h:mm tt"));
      alertElement.SetAttribute("DismissalDate", "");
      alertElement.SetAttribute("DismissalUser", "");
    }

    public void DeactivateAlert(string alertId)
    {
      XmlElement alertElement = this.getAlertElement(alertId, false);
      alertElement?.ParentNode.RemoveChild((XmlNode) alertElement);
    }

    public void DismissAlert(string alertId, string userId, DateTime timestamp)
    {
      XmlElement alertElement = this.getAlertElement(alertId, true);
      alertElement.SetAttribute("DismissalUser", userId);
      alertElement.SetAttribute("DismissalDate", timestamp.ToString("M/d/yyyy h:mm tt"));
    }

    public AlertStatus GetAlertStatus(string alertId)
    {
      XmlElement alertElement = this.getAlertElement(alertId, false);
      if (alertElement == null)
        return new AlertStatus(alertId, DateTime.MinValue, (string) null, DateTime.MinValue, (string) null);
      DateTime activationTime = DateTime.MinValue;
      DateTime dismissalTime = DateTime.MinValue;
      try
      {
        activationTime = DateTime.ParseExact(alertElement.GetAttribute("ActivationDate"), "M/d/yyyy h:mm tt", (IFormatProvider) null);
      }
      catch
      {
      }
      try
      {
        dismissalTime = DateTime.ParseExact(alertElement.GetAttribute("DismissalDate"), "M/d/yyyy h:mm tt", (IFormatProvider) null);
      }
      catch
      {
      }
      return new AlertStatus(alertId, activationTime, alertElement.GetAttribute("ActivationUser"), dismissalTime, alertElement.GetAttribute("DismissalUser"));
    }

    private XmlElement getAlertElement(string alertId, bool createIfMissing)
    {
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/Alerts[@SysID='" + this.systemId + "']");
      if (xmlElement == null)
      {
        xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae").AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("Alerts"));
        xmlElement.SetAttribute("SysID", this.systemId);
      }
      XmlElement alertElement = (XmlElement) xmlElement.SelectSingleNode("Alert[@AlertID='" + alertId + "']");
      if (alertElement == null & createIfMissing)
      {
        alertElement = (XmlElement) xmlElement.AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("Alert"));
        alertElement.SetAttribute("AlertID", alertId);
      }
      return alertElement;
    }

    public bool GetDocFieldOverrideFlag(string fieldId)
    {
      XmlElement settingsFieldElement = this.getDocSettingsFieldElement(fieldId, false);
      return settingsFieldElement != null && settingsFieldElement.GetAttribute("Override") == "1";
    }

    public void SetDocFieldOverrideFlag(string fieldId, bool ovride)
    {
      this.getDocSettingsFieldElement(fieldId, true).SetAttribute("Override", ovride ? "1" : "0");
    }

    private XmlElement getDocSettingsElement()
    {
      return (XmlElement) this.root.SelectSingleNode("EllieMae/DocSettings") ?? (XmlElement) this.root.SelectSingleNode("EllieMae").AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("DocSettings"));
    }

    private XmlElement getDocSettingsFieldElement(string fieldId, bool createIfMissing)
    {
      XmlElement docSettingsElement = this.getDocSettingsElement();
      XmlElement xmlElement = (XmlElement) docSettingsElement.SelectSingleNode("FieldSettings") ?? (XmlElement) docSettingsElement.AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("FieldSettings"));
      XmlElement settingsFieldElement = (XmlElement) xmlElement.SelectSingleNode("Field[@ID='" + fieldId + "']");
      if (settingsFieldElement == null & createIfMissing)
      {
        settingsFieldElement = (XmlElement) xmlElement.AppendChild((XmlNode) this.root.OwnerDocument.CreateElement("Field"));
        settingsFieldElement.SetAttribute("ID", fieldId);
      }
      return settingsFieldElement;
    }

    internal Hashtable GetLockRequestSnapshot(string loanLockGUID)
    {
      string str = "EllieMae/RateLock/RequestLogs/";
      XmlNode xmlNode;
      try
      {
        xmlNode = this.root.SelectSingleNode(str + "Log[@GUID='" + loanLockGUID + "']");
        if (xmlNode == null)
          return (Hashtable) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddLockRequestSnapshot: Can't remove " + str + "Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return (Hashtable) null;
      }
      Hashtable lockRequestSnapshot = new Hashtable();
      foreach (XmlElement selectNode in xmlNode.SelectNodes("FIELD"))
      {
        string attribute1 = selectNode.GetAttribute("id");
        string attribute2 = selectNode.GetAttribute("val");
        if (lockRequestSnapshot.ContainsKey((object) attribute1))
          lockRequestSnapshot[(object) attribute1] = (object) attribute2;
        else
          lockRequestSnapshot.Add((object) attribute1, (object) attribute2);
      }
      return lockRequestSnapshot;
    }

    internal void RemoveLockRequest()
    {
      string xpath = "EllieMae/RateLock";
      try
      {
        this.root.SelectSingleNode(xpath)?.RemoveAll();
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "RemoveLockRequest: Can't remove " + xpath + " node. Exception: " + ex.Message);
      }
    }

    internal bool AddLockRequestSnapshot(string loanLockGUID, Hashtable fieldValues)
    {
      string str1 = "EllieMae/RateLock/RequestLogs";
      try
      {
        this.root.SelectSingleNode(str1 + "/Log[@GUID='" + loanLockGUID + "']")?.RemoveAll();
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddLockRequestSnapshot: Can't remove " + str1 + "/Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return false;
      }
      XmlElement xmlElement1 = (XmlElement) (this.root.SelectSingleNode(str1) ?? (XmlNode) this.createPath(str1)).AppendChild((XmlNode) this.xmldoc.CreateElement("Log"));
      xmlElement1.SetAttribute("GUID", loanLockGUID);
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (DictionaryEntry fieldValue in fieldValues)
      {
        string str2 = fieldValue.Key.ToString();
        string str3 = fieldValue.Value.ToString();
        if (!(str3 == string.Empty))
        {
          XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
          xmlElement2.SetAttribute("id", str2);
          xmlElement2.SetAttribute("val", str3);
        }
      }
      return true;
    }

    internal bool ClearLockRequestSnapshot(string loanLockGUID)
    {
      string str = "EllieMae/RateLock/RequestLogs";
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(str);
      try
      {
        XmlNode oldChild = this.root.SelectSingleNode(str + "/Log[@GUID='" + loanLockGUID + "']");
        if (oldChild != null)
          xmlElement?.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddLockRequestSnapshot: Can't remove " + str + "/Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return false;
      }
      if (xmlElement == null)
        xmlElement = this.createPath(str);
      ((XmlElement) xmlElement.AppendChild((XmlNode) this.xmldoc.CreateElement("Log"))).SetAttribute("GUID", loanLockGUID);
      return true;
    }

    internal bool UpdateConfirmLockComments(
      string loanLockGUID,
      string newComments,
      bool onBuySide)
    {
      string str = "EllieMae/RateLock/RequestLogs";
      XmlNode xmlNode;
      try
      {
        xmlNode = this.root.SelectSingleNode(str + "/Log[@GUID='" + loanLockGUID + "']");
        if (xmlNode == null)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "UpdateConfirmLockComments: Can't find the " + str + "/Log[GUID='" + loanLockGUID + "'] node.");
          return false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "UpdateConfirmLockComments: Can't add new comments to " + str + "/Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return false;
      }
      XmlElement xmlElement = (XmlElement) null;
      foreach (XmlElement selectNode in xmlNode.SelectNodes("FIELD"))
      {
        string attribute = selectNode.GetAttribute("id");
        if (onBuySide && attribute == "2204" || !onBuySide && attribute == "2275")
        {
          xmlElement = selectNode;
          break;
        }
      }
      try
      {
        if (xmlElement == null)
        {
          xmlElement = (XmlElement) xmlNode.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
          if (onBuySide)
            xmlElement.SetAttribute("id", "2204");
          else
            xmlElement.SetAttribute("id", "2275");
        }
        xmlElement.SetAttribute("val", newComments);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "UpdateConfirmLockComments: Can't update comments to " + str + "/Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return false;
      }
      return true;
    }

    internal bool AddFieldToLockSnapshot(
      LoanData loan,
      string loanLockGUID,
      string fieldId,
      string newValue)
    {
      string str1 = "EllieMae/RateLock/RequestLogs";
      string xpath = str1 + "/Log[@GUID='" + loanLockGUID + "']";
      XmlNode xmlNode;
      try
      {
        xmlNode = this.root.SelectSingleNode(xpath);
        if (xmlNode == null)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "UpdateConfirmLockComments: Can't find the " + str1 + "/Log[GUID='" + loanLockGUID + "'] node.");
          return false;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "UpdateConfirmLockComments: Can't add new comments to " + str1 + "/Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return false;
      }
      XmlNodeList xmlNodeList = xmlNode.SelectNodes("FIELD");
      string str2 = "CLRFIELD." + fieldId;
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        if (xmlElement.GetAttribute("id") == str2)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddLockField: Can't add field to " + str1 + "/Log[GUID='" + loanLockGUID + "'] node. Field,  " + fieldId + ", already exists");
          return false;
        }
      }
      try
      {
        XmlElement xmlElement = (XmlElement) xmlNode.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
        xmlElement.SetAttribute("id", str2);
        xmlElement.SetAttribute("val", newValue);
        this.logList.GetLockRequest(loanLockGUID).ResetLockRequestSnapshot();
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddLockField: Can't add field to " + str1 + "/Log[GUID='" + loanLockGUID + "'] node. Exception: " + ex.Message);
        return false;
      }
      return true;
    }

    internal long GetBatchUpdateSequenceNum()
    {
      XmlElement batchUpdateElement = this.getBatchUpdateElement(false);
      if (batchUpdateElement == null)
        return -1;
      try
      {
        return long.Parse(batchUpdateElement.GetAttribute("SequenceNumber"));
      }
      catch
      {
        return -1;
      }
    }

    internal void SetBatchUpdateSequenceNum(long sequenceNumber)
    {
      this.getBatchUpdateElement(true).SetAttribute("SequenceNumber", sequenceNumber.ToString());
    }

    private XmlElement getBatchUpdateElement(bool createIfMissing)
    {
      XmlElement batchUpdateElement = (XmlElement) this.root.SelectSingleNode("EllieMae/BatchUpdate[@SysID='" + this.systemId + "']");
      if (batchUpdateElement == null)
      {
        if (!createIfMissing)
          return (XmlElement) null;
        batchUpdateElement = (XmlElement) this.root.SelectSingleNode("EllieMae").AppendChild((XmlNode) this.xmldoc.CreateElement("BatchUpdate"));
        batchUpdateElement.SetAttribute("SysID", this.systemId);
      }
      return batchUpdateElement;
    }

    internal string getFieldfromGuid(string Guid, string fieldID)
    {
      foreach (XmlElement selectNode in this.root.SelectSingleNode("EllieMae/RateLock/RequestLogs" + "/Log[@GUID='" + Guid + "']").SelectNodes("FIELD"))
      {
        if (selectNode.GetAttribute("id") == fieldID)
          return selectNode.GetAttribute("val");
      }
      return (string) null;
    }

    private void loadTraceFields()
    {
      try
      {
        string[] strArray = string.Concat(EnConfigurationSettings.GlobalSettings["TraceFields"]).Split(';');
        for (int index = 0; index < strArray.Length; ++index)
        {
          if (strArray[index].Trim() != "")
            this.traceFields[strArray[index].Trim()] = true;
        }
        string str1 = string.Concat(EnConfigurationSettings.GlobalSettings["TraceFieldUpdate"]);
        this.traceFieldUpdate = !string.IsNullOrEmpty(str1) && str1 == "1";
        if (!this.traceFieldUpdate)
          return;
        string str2 = string.Concat(EnConfigurationSettings.GlobalSettings["FieldsToTrace"]);
        if (!string.IsNullOrEmpty(str2))
        {
          int num = 0;
          string str3 = str2;
          char[] separator = new char[1]{ ';' };
          foreach (string str4 in str3.Split(separator, StringSplitOptions.RemoveEmptyEntries))
          {
            if (num < 12 && !this.fieldsToTrace.Contains(str4))
            {
              this.fieldsToTrace.Add(str4);
              ++num;
            }
          }
        }
        this.traceFieldUpdatePath = string.Concat(EnConfigurationSettings.GlobalSettings["TraceFieldUpdatePath"]);
        string fileName = "TraceFieldUpdate" + DateTime.Today.ToString("MMddyyyy") + ".log";
        this.listener = Trace.Listeners["TraceFieldUpdateListener"] as TextTraceListener;
        if (this.listener != null)
          return;
        this.listener = new TextTraceListener("TraceFieldUpdateListener", this.traceFieldUpdatePath, fileName);
        Trace.Listeners.Add((TraceListener) this.listener);
      }
      catch
      {
        this.listener = Trace.Listeners["TraceFieldUpdateListener"] as TextTraceListener;
      }
    }

    public void CLose()
    {
      if (this.listener == null)
        return;
      this.listener.Close();
      Trace.Listeners.Remove("TraceFieldUpdateListener");
    }

    internal void AddServicingTransaction(ServicingTransactionBase transactionLog)
    {
      XmlElement newlog1 = (XmlElement) this.root.SelectSingleNode("EllieMae/InterimServicing/TransactionLog/Transaction[@GUID=\"" + transactionLog.TransactionGUID + "\"]");
      if (newlog1 != null)
      {
        transactionLog.Add(newlog1, this.GetFieldAt("4912") == "FiveDecimals");
      }
      else
      {
        string str = "EllieMae/InterimServicing/TransactionLog";
        XmlElement newlog2 = (XmlElement) ((XmlElement) this.root.SelectSingleNode(str) ?? this.createPath(str)).AppendChild((XmlNode) this.xmldoc.CreateElement("Transaction"));
        transactionLog.Add(newlog2, this.GetFieldAt("4912") == "FiveDecimals");
      }
    }

    internal bool RemoveServicingTransaction(string transactionGUID)
    {
      XmlNode oldChild = this.root.SelectSingleNode("EllieMae/InterimServicing/TransactionLog/Transaction[@GUID=\"" + transactionGUID + "\"]");
      oldChild?.ParentNode.RemoveChild(oldChild);
      return true;
    }

    internal bool ClearServicingTransactions()
    {
      try
      {
        XmlNode oldChild = this.root.SelectSingleNode("EllieMae/InterimServicing");
        oldChild?.ParentNode.RemoveChild(oldChild);
        this.clearRepeatableFieldCacheValues("SERVICE.X", -1, 1, 143, new List<int>()
        {
          27,
          28,
          29,
          78,
          84
        }, new List<int>() { 99 }, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Can't clear Interim Servicing due to the following error: " + ex.Message);
        return false;
      }
    }

    internal ServicingTransactionBase[] GetServicingTransactions()
    {
      return this.GetServicingTransactions(false);
    }

    internal ServicingTransactionBase[] GetServicingTransactions(bool skipSchedule)
    {
      return Mapping.GetServicingTransactions(this.root.SelectNodes("EllieMae/InterimServicing/TransactionLog/Transaction"), skipSchedule);
    }

    public static ServicingTransactionBase[] GetServicingTransactions(
      XmlNodeList nodeList,
      bool skipSchedule)
    {
      if (nodeList == null)
        return (ServicingTransactionBase[]) null;
      ServicingTransactionBase transLog = (ServicingTransactionBase) null;
      ArrayList source = new ArrayList();
      foreach (XmlElement node in nodeList)
      {
        if (node.HasAttribute("Type"))
        {
          switch ((ServicingTransactionTypes) ServicingEnum.ToEnum(node.GetAttribute("Type"), typeof (ServicingTransactionTypes)))
          {
            case ServicingTransactionTypes.Payment:
              transLog = (ServicingTransactionBase) new PaymentTransactionLog(node);
              break;
            case ServicingTransactionTypes.PaymentReversal:
              transLog = (ServicingTransactionBase) new PaymentReversalLog(node);
              break;
            case ServicingTransactionTypes.EscrowDisbursement:
              transLog = (ServicingTransactionBase) new EscrowDisbursementLog(node);
              break;
            case ServicingTransactionTypes.EscrowInterest:
              transLog = (ServicingTransactionBase) new EscrowInterestLog(node);
              break;
            case ServicingTransactionTypes.Other:
              transLog = (ServicingTransactionBase) new OtherTransactionLog(node);
              break;
            case ServicingTransactionTypes.SchedulePayment:
              if (!skipSchedule)
              {
                transLog = (ServicingTransactionBase) new SchedulePaymentLog(node);
                break;
              }
              continue;
            case ServicingTransactionTypes.PurchaseAdvice:
              transLog = (ServicingTransactionBase) new LoanPurchaseLog(node);
              break;
            case ServicingTransactionTypes.PrincipalDisbursement:
              transLog = (ServicingTransactionBase) new PrincipalDisbursementLog(node);
              break;
            default:
              continue;
          }
          if (source.Cast<ServicingTransactionBase>().Where<ServicingTransactionBase>((Func<ServicingTransactionBase, bool>) (x => x.TransactionGUID.Equals(transLog.TransactionGUID))).Count<ServicingTransactionBase>() > 0)
            node.ParentNode.RemoveChild((XmlNode) node);
          else
            source.Add((object) transLog);
        }
      }
      return source.Count == 0 ? (ServicingTransactionBase[]) null : (ServicingTransactionBase[]) source.ToArray(typeof (ServicingTransactionBase));
    }

    internal bool StartInterimServicing(PaymentScheduleSnapshot paySchedule)
    {
      if (paySchedule.MonthlyPayments.Length <= 1)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Can't create loan snapshot for Intrim Servicing. Loan Information is invalid.");
        return false;
      }
      string xpath = "EllieMae/InterimServicing/LoanSnapshot";
      try
      {
        XmlNode oldChild = this.root.SelectSingleNode(xpath);
        oldChild?.ParentNode.RemoveChild(oldChild);
        XmlNode path = (XmlNode) this.createPath("EllieMae/InterimServicing/LoanSnapshot");
        paySchedule.ToXml(path, this.xmldoc);
        this.SetFieldAt("SERVICE.X8", "Current");
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Can't create loan snapshot for Intrim Servicing. Exception: " + ex.Message);
        return false;
      }
      return true;
    }

    internal PaymentScheduleSnapshot GetPaymentScheduleSnapshot()
    {
      XmlNode node = this.root.SelectSingleNode("EllieMae/InterimServicing/LoanSnapshot");
      return node == null ? (PaymentScheduleSnapshot) null : new PaymentScheduleSnapshot(node);
    }

    internal void ClearSettlementServiceProviders()
    {
      try
      {
        string fieldAt = this.GetFieldAt("SP.DATEISSUED");
        int serviceProviders = this.GetNumberOfSettlementServiceProviders();
        XmlNode oldChild = this.root.SelectSingleNode("EllieMae/SettlementServiceProviders");
        oldChild?.ParentNode.RemoveChild(oldChild);
        if (serviceProviders > 0)
        {
          for (int index = 0; index < serviceProviders; ++index)
            this.clearRepeatableFieldCacheValues("SP", index + 1, 1, 36, (List<int>) null, (List<int>) null, true);
        }
        this.GetFieldObject("SP.ADDITIONALINFO", this.currentBorrowerPair).CachedValue = (string) null;
        this.GetFieldObject("SP.DATEISSUED", this.currentBorrowerPair).CachedValue = (string) null;
        this.SetFieldAt("SP.DATEISSUED", fieldAt);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in ClearSettlementServiceProviders, e: " + ex.Message);
      }
    }

    internal int GetNumberOfSettlementServiceProviders()
    {
      return this.root.SelectNodes("EllieMae/SettlementServiceProviders/ServiceProvider").Count;
    }

    internal bool UpSettlementServiceProvider(int i)
    {
      if (i == 0)
        return true;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/SettlementServiceProviders/ServiceProvider");
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
        this.clearRepeatableFieldCacheValues("SP", i, 1, 36, (List<int>) null, (List<int>) null, true);
        this.clearRepeatableFieldCacheValues("SP", i + 1, 1, 36, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpSettlementServiceProvider, e: " + ex.Message);
        return false;
      }
    }

    internal bool DownSettlementServiceProvider(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/SettlementServiceProviders/ServiceProvider");
        if (i < xmlNodeList.Count - 1)
        {
          XmlNode newChild = xmlNodeList[i];
          XmlNode refChild = xmlNodeList[i + 1];
          newChild.ParentNode.InsertAfter(newChild, refChild);
        }
        this.clearRepeatableFieldCacheValues("SP", i + 1, 1, 36, (List<int>) null, (List<int>) null, true);
        this.clearRepeatableFieldCacheValues("SP", i + 2, 1, 36, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownSettlementServiceProvider, e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveSettlementServiceProviderAt(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/SettlementServiceProviders/ServiceProvider");
        if (xmlNodeList == null || xmlNodeList.Count == 0 || i >= xmlNodeList.Count)
          return true;
        XmlNode oldChild = xmlNodeList[i];
        oldChild.ParentNode.RemoveChild(oldChild);
        for (int index = 0; index < xmlNodeList.Count; ++index)
          this.clearRepeatableFieldCacheValues("SP", index + 1, 1, 36, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove settlement service provider " + (object) i + ", e: " + ex.Message);
        return false;
      }
    }

    internal int NewSettlementServiceProvider()
    {
      if ((XmlElement) this.root.SelectSingleNode("EllieMae/SettlementServiceProviders") == null)
        this.createPath("EllieMae/SettlementServiceProviders");
      int num = this.root.SelectNodes("EllieMae/SettlementServiceProviders/ServiceProvider").Count + 1;
      this.createProvider("SP" + num.ToString("00") + "00");
      return num;
    }

    private void createProvider(string id)
    {
      int num1 = int.Parse(id.Substring(2, 2));
      if (id.Length > 6)
        num1 = int.Parse(id.Substring(2, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/SettlementServiceProviders/ServiceProvider");
      int num2 = num1 - xmlNodeList.Count;
      XmlElement refChild = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        refChild = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/SettlementServiceProviders") ?? this.createPath("EllieMae/SettlementServiceProviders");
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("ServiceProvider");
        element.SetAttribute("_ID", this.generateNewId());
        Mapping.AddEntityId(element);
        if (refChild != null)
          xmlElement.InsertAfter((XmlNode) element, (XmlNode) refChild);
        else
          xmlElement.AppendChild((XmlNode) element);
        refChild = element;
      }
    }

    public void UpTAX4506T(int i, bool for4506Only)
    {
      if (i == 0)
        return;
      try
      {
        XmlNodeList xmlNodeList = this.brwElm.SelectNodes(for4506Only ? "EllieMae/PAIR/TAX_4506/HISTORY" : "EllieMae/PAIR/TAX_4506T/HISTORY");
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Can't move " + (for4506Only ? "TAX4506" : "TAX4506T") + " record, e: " + ex.Message);
      }
    }

    public void DownTAX4506T(int i, bool for4506Only)
    {
      try
      {
        XmlNodeList xmlNodeList = this.brwElm.SelectNodes(for4506Only ? "EllieMae/PAIR/TAX_4506/HISTORY" : "EllieMae/PAIR/TAX_4506T/HISTORY");
        if (i >= xmlNodeList.Count - 1)
          return;
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i + 1];
        newChild.ParentNode.InsertAfter(newChild, refChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Can't move " + (for4506Only ? "TAX4506" : "TAX4506T") + " record, e: " + ex.Message);
      }
    }

    internal int NewTAX4506T(bool for4506Only) => this.createTAX4506T(for4506Only);

    private int createTAX4506T(bool for4506Only)
    {
      int num = this.brwElm.SelectNodes(for4506Only ? "EllieMae/PAIR/TAX_4506/HISTORY" : "EllieMae/PAIR/TAX_4506T/HISTORY").Count + 1;
      this.SetFieldAt((for4506Only ? "AR" : "IR") + num.ToString("00") + "63", this.generateNewId());
      if (!for4506Only)
        this.SetFieldAt("IR" + num.ToString("00") + "93", "4506-COct2022");
      return this.brwElm.SelectNodes(for4506Only ? "EllieMae/PAIR/TAX_4506/HISTORY" : "EllieMae/PAIR/TAX_4506T/HISTORY").Count;
    }

    public int RemoveTAX4506TAt(int i, bool for4506Only)
    {
      try
      {
        XmlNode selectNode = this.brwElm.SelectNodes(for4506Only ? "EllieMae/PAIR/TAX_4506/HISTORY" : "EllieMae/PAIR/TAX_4506T/HISTORY")[i];
        selectNode.ParentNode.RemoveChild(selectNode);
        return 1;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove " + (for4506Only ? (object) "TAX4506 " : (object) "TAX4506-T ") + (object) i + ", e: " + ex.Message);
        return -1;
      }
    }

    internal int GetNumberOfTAX4506Ts(bool for4506Only)
    {
      return this.brwElm.SelectNodes(for4506Only ? "EllieMae/PAIR/TAX_4506/HISTORY" : "EllieMae/PAIR/TAX_4506T/HISTORY").Count;
    }

    internal int GetNumberOf4506TReports()
    {
      return this.brwElm.SelectNodes("EllieMae/PAIR/TAX4506T_OrderReports/OrderInformation").Count;
    }

    internal int New4506TReport()
    {
      int num = this.brwElm.SelectNodes("EllieMae/PAIR/TAX4506T_OrderReports/OrderInformation").Count + 1;
      XmlNode parent = this.brwElm.SelectSingleNode("EllieMae");
      XmlElement element = this.xmldoc.CreateElement("OrderInformation");
      Mapping.AddEntityId(element);
      this.AppendSameTypeChild((XmlElement) parent, element, "OrderInformation");
      this.SetFieldAt("TQL4506T" + num.ToString("00") + "99", this.generateNewId());
      return this.brwElm.SelectNodes("EllieMae/PAIR/TAX4506T_OrderReports/OrderInformation").Count;
    }

    internal int NewGSERepWarrantTracker()
    {
      int num;
      try
      {
        if ((XmlElement) this.root.SelectSingleNode("EllieMae/TQL/GSETrackers") == null)
          this.createPath("EllieMae/TQL/GSETrackers");
        num = this.root.SelectNodes("EllieMae/TQL/GSETrackers/Tracker").Count + 1;
        this.createRepeatableNode("TQLGSE" + num.ToString("00") + "00", "EllieMae/TQL/GSETrackers", "Tracker", true);
      }
      catch (Exception ex)
      {
        return -1;
      }
      return num;
    }

    internal int NewDisaster()
    {
      int num;
      try
      {
        if ((XmlElement) this.root.SelectSingleNode("EllieMae/Disasters") == null)
          this.createPath("EllieMae/Disasters");
        num = this.root.SelectNodes("EllieMae/Disasters/Disaster").Count + 1;
        this.createRepeatableNode("FEMA" + num.ToString("00") + "00", "EllieMae/Disasters", "Disaster", true);
      }
      catch (Exception ex)
      {
        return -1;
      }
      return num;
    }

    public int Remove4506TReportAt(int i)
    {
      try
      {
        XmlNode selectNode = this.brwElm.SelectNodes("EllieMae/PAIR/TAX4506T_OrderReports/OrderInformation")[i];
        selectNode.ParentNode.RemoveChild(selectNode);
        return 1;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove 4506C Report record, e: " + ex.Message);
        return -1;
      }
    }

    public int RemoveGSERepWarrantTrackerAt(int i)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes("EllieMae/TQL/GSETrackers/Tracker")[i];
        selectNode.ParentNode.RemoveChild(selectNode);
        return 1;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove GSE Rep & Warrant Tracker record, e: " + ex.Message);
        return -1;
      }
    }

    public bool RemoveDisasterAt(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/Disasters/Disaster");
        XmlNode oldChild = xmlNodeList[i];
        int count = xmlNodeList.Count;
        oldChild.ParentNode.RemoveChild(oldChild);
        for (int index = 0; index < count; ++index)
          this.clearRepeatableFieldCacheValues("FEMA", index + 1, 1, 17, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Disaster, e: " + ex.Message);
        return false;
      }
    }

    public bool RemoveDisasters()
    {
      try
      {
        this.root.SelectSingleNode("EllieMae/Disasters")?.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "RemoveDisasters: Can't remove Disasterss. Exception: " + ex.Message);
      }
      return false;
    }

    internal bool UpDisaster(int i)
    {
      if (i == 0)
        return true;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/Disasters/Disaster");
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
        this.clearRepeatableFieldCacheValues("FEMA", i, 1, 17, (List<int>) null, (List<int>) null, true);
        this.clearRepeatableFieldCacheValues("FEMA", i + 1, 1, 17, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpDisaster, e: " + ex.Message);
        return false;
      }
    }

    internal bool DownDisaster(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/Disasters/Disaster");
        if (i < xmlNodeList.Count - 1)
        {
          XmlNode newChild = xmlNodeList[i];
          XmlNode refChild = xmlNodeList[i + 1];
          newChild.ParentNode.InsertAfter(newChild, refChild);
        }
        this.clearRepeatableFieldCacheValues("FEMA", i + 1, 1, 17, (List<int>) null, (List<int>) null, true);
        this.clearRepeatableFieldCacheValues("FEMA", i + 2, 1, 17, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownHomeCounselingProvider, e: " + ex.Message);
        return false;
      }
    }

    public bool Remove4506TReports()
    {
      try
      {
        this.brwElm.SelectSingleNode("EllieMae/PAIR/TAX4506T_OrderReports")?.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Remove4506TReports: Can't remove all 4506C Reports. Exception: " + ex.Message);
      }
      return false;
    }

    public bool RemoveTQLFraudAlerts()
    {
      try
      {
        this.root.SelectSingleNode("EllieMae/TQL/FraudOrderAlerts")?.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "RemoveTQLFraudAlerts: Can't remove Fraud Alerts. Exception: " + ex.Message);
      }
      return false;
    }

    public bool RemoveTQLComplianceAlerts()
    {
      try
      {
        this.root.SelectSingleNode("EllieMae/TQL/ComplianceOrders")?.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "RemoveTQLComplianceAlerts: Can't remove Compliance Alerts. Exception: " + ex.Message);
      }
      return false;
    }

    public bool RemoveTQLDocDeliveryDates()
    {
      try
      {
        this.root.SelectSingleNode("EllieMae/TQL/Documents")?.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "RemoveTQLDocDeliveryDates: Can't remove document delivery dates. Exception: " + ex.Message);
      }
      return false;
    }

    public bool RemoveGSERepWarrantTrackers()
    {
      try
      {
        this.root.SelectSingleNode("EllieMae/TQL/GSETrackers")?.RemoveAll();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "RemoveGSERepWarrantTrackers: Can't remove GSE Rep & Warrant Tracker. Exception: " + ex.Message);
      }
      return false;
    }

    internal int GetNumberOfTQLDocDeliveryDates()
    {
      return this.root.SelectNodes("EllieMae/TQL/Documents/Document").Count;
    }

    internal int GetNumberOfComplianceAlerts()
    {
      return this.root.SelectNodes("EllieMae/TQL/ComplianceOrders/Alert").Count;
    }

    internal int GetNumberOfFraudAlerts()
    {
      return this.root.SelectNodes("EllieMae/TQL/FraudOrderAlerts/Alert").Count;
    }

    internal int GetNumberOfGSERepWarrantTrackers()
    {
      return this.root.SelectNodes("EllieMae/TQL/GSETrackers/Tracker").Count;
    }

    internal int GetNumberOfDisasters()
    {
      return this.root.SelectNodes("EllieMae/Disasters/Disaster").Count;
    }

    internal int GetNumberOfAUSTrackingHistory()
    {
      return this.root.SelectNodes("BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/" + Mapping.AUSTrackingHistoryPath + "/History").Count;
    }

    public AUSTrackingHistoryList GetAUSTrackingHistoryList()
    {
      return this.GetAUSTrackingHistoryList(this.currentBorrowerPair.Id);
    }

    public AUSTrackingHistoryList GetAUSTrackingHistoryList(string borPairID)
    {
      string str = "BORROWER[@BorrowerID=\"" + borPairID + "\"]/";
      AUSTrackingHistoryList trackingHistoryList = new AUSTrackingHistoryList();
      XmlNodeList xmlNodeList1 = this.root.SelectNodes(str + Mapping.AUSTrackingHistoryPath + "/History");
      if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
      {
        foreach (XmlNode xmlNode in xmlNodeList1)
        {
          XmlElement xmlElement1 = (XmlElement) xmlNode;
          if (xmlElement1.HasAttribute("GUID"))
          {
            AUSTrackingHistoryLog rec = new AUSTrackingHistoryLog(xmlElement1.GetAttribute("GUID"));
            if (xmlElement1.HasAttribute("CreatedOn"))
              rec.Date = Utils.ParseDate((object) xmlElement1.GetAttribute("CreatedOn"));
            XmlNodeList xmlNodeList2 = xmlNode.SelectNodes("FIELD");
            if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
            {
              foreach (XmlElement xmlElement2 in xmlNodeList2)
              {
                string attribute1 = xmlElement2.GetAttribute("id");
                string attribute2 = xmlElement2.GetAttribute("val");
                rec.SetField(attribute1, attribute2);
              }
            }
            trackingHistoryList.AddHistory(rec);
          }
        }
      }
      trackingHistoryList.Sort();
      return trackingHistoryList;
    }

    internal string AddAUSTrackingHistory(
      DateTime submittedDate,
      Hashtable trackingHistoryTable,
      bool forDU,
      string CreatedOn)
    {
      string historyID = Guid.NewGuid().ToString();
      if (trackingHistoryTable.ContainsKey((object) "GUID"))
        historyID = trackingHistoryTable[(object) "GUID"].ToString();
      AUSTrackingHistoryLog trackingHistory = new AUSTrackingHistoryLog(historyID);
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (DictionaryEntry dictionaryEntry in trackingHistoryTable)
      {
        string id = dictionaryEntry.Key.ToString();
        string val = dictionaryEntry.Value != null ? dictionaryEntry.Value.ToString() : "";
        switch (id)
        {
          case nameof (CreatedOn):
            trackingHistory.Date = Utils.ParseDate((object) val);
            continue;
          case "RecordType":
            trackingHistory.RecordType = val;
            continue;
          default:
            trackingHistory.DataValues.SetField(id, val);
            continue;
        }
      }
      if (string.IsNullOrEmpty(trackingHistory.RecordType))
        trackingHistory.RecordType = forDU ? "DU" : "LP";
      if (submittedDate != DateTime.MinValue)
        trackingHistory.Date = submittedDate;
      else if (trackingHistory.Date == DateTime.MinValue)
        trackingHistory.Date = DateTime.Now;
      if (!string.IsNullOrEmpty(CreatedOn))
        trackingHistory.CreatedOn = CreatedOn;
      return this.AddAUSTrackingHistory(trackingHistory) ? trackingHistory.HistoryID : (string) null;
    }

    internal bool AddAUSTrackingHistory(AUSTrackingHistoryLog trackingHistory)
    {
      string str = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      string xpath = str + Mapping.AUSTrackingHistoryPath + "/History[@GUID='" + trackingHistory.HistoryID + "']";
      try
      {
        XmlNode oldChild = this.root.SelectSingleNode(xpath);
        oldChild?.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddAUSTrackingHistory: Can't remove AUS Tracking snapshot '" + trackingHistory.HistoryID + "' node. Exception: " + ex.Message);
        return false;
      }
      XmlElement elm = (XmlElement) (this.root.SelectSingleNode(str + Mapping.AUSTrackingHistoryPath) ?? (XmlNode) this.createPath(str + Mapping.AUSTrackingHistoryPath)).AppendChild((XmlNode) this.xmldoc.CreateElement("History"));
      Mapping.AddEntityId(elm);
      elm.SetAttribute("GUID", trackingHistory.HistoryID);
      if (string.Equals(trackingHistory.RecordType, "Manual", StringComparison.CurrentCultureIgnoreCase))
        elm.SetAttribute("CreatedOn", trackingHistory.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'"));
      else if (!string.IsNullOrEmpty(trackingHistory.CreatedOn))
        elm.SetAttribute("CreatedOn", trackingHistory.CreatedOn);
      else
        elm.SetAttribute("CreatedOn", trackingHistory.Date.ToString("MM/dd/yyyy hh:mm:ss tt"));
      elm.SetAttribute("RecordType", trackingHistory.RecordType);
      string empty = string.Empty;
      foreach (string assignedFieldId in trackingHistory.DataValues.GetAssignedFieldIDs())
      {
        string field = trackingHistory.GetField(assignedFieldId);
        if (!(field == string.Empty))
        {
          XmlElement xmlElement = (XmlElement) elm.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
          xmlElement.SetAttribute("id", assignedFieldId);
          xmlElement.SetAttribute("val", field);
        }
      }
      this.PopulateLatestSubmissionAusTracking();
      return true;
    }

    internal bool UpdateAUSTrackingHistory(AUSTrackingHistoryLog trackingHistory)
    {
      return this.AddAUSTrackingHistory(trackingHistory);
    }

    internal void PopulateLatestSubmissionAusTracking()
    {
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      AUSTrackingHistoryList trackingHistoryList1 = (AUSTrackingHistoryList) null;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        AUSTrackingHistoryList trackingHistoryList2 = this.GetAUSTrackingHistoryList(borrowerPairs[index].Id);
        if (trackingHistoryList2 != null && trackingHistoryList2.HistoryCount != 0)
        {
          if (trackingHistoryList1 == null)
          {
            trackingHistoryList1 = trackingHistoryList2;
          }
          else
          {
            for (int i = 0; i < trackingHistoryList2.HistoryCount; ++i)
              trackingHistoryList1.AddHistory(trackingHistoryList2.GetHistoryAt(i));
          }
        }
      }
      trackingHistoryList1?.Sort();
      if (trackingHistoryList1 == null || trackingHistoryList1.HistoryCount == 0)
      {
        for (int index = 1; index <= 18; ++index)
          this.SetFieldAt("AUSF.X" + (object) index, "");
        this.SetFieldAt("4830", "");
        this.SetFieldAt("4752", "");
      }
      else
      {
        List<string> source1 = new List<string>()
        {
          "LQA",
          "EarlyCheck"
        };
        List<string> source2 = new List<string>()
        {
          "DU",
          "LP"
        };
        string fieldAt = this.GetFieldAt("AUSF.X71");
        AUSTrackingHistoryLog trackingHistoryLog1 = (AUSTrackingHistoryLog) null;
        AUSTrackingHistoryLog trackingHistoryLog2 = (AUSTrackingHistoryLog) null;
        for (int i = 0; i < trackingHistoryList1.HistoryCount; ++i)
        {
          if (!source1.Contains<string>(trackingHistoryList1.GetHistoryAt(i).DataValues.GetField("AUS.X1"), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase))
          {
            trackingHistoryLog2 = trackingHistoryList1.GetHistoryAt(i);
            break;
          }
        }
        if (string.IsNullOrEmpty(fieldAt))
        {
          trackingHistoryLog1 = trackingHistoryLog2;
        }
        else
        {
          for (int i = 0; i < trackingHistoryList1.HistoryCount; ++i)
          {
            if (source2.Contains<string>(trackingHistoryList1.GetHistoryAt(i).DataValues.GetField("AUS.X1"), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase) && trackingHistoryList1.GetHistoryAt(i).HistoryID == fieldAt)
            {
              trackingHistoryLog1 = trackingHistoryList1.GetHistoryAt(i);
              break;
            }
          }
          if (trackingHistoryLog1 == null)
          {
            trackingHistoryLog1 = trackingHistoryLog2;
            this.SetFieldAt("AUSF.X71", "");
          }
          else if (trackingHistoryLog1.HistoryID != trackingHistoryLog2.HistoryID)
          {
            if (trackingHistoryLog1.DataValues.GetField("AUS.X1") == trackingHistoryLog2.DataValues.GetField("AUS.X1"))
            {
              trackingHistoryLog1 = trackingHistoryLog2;
              this.SetFieldAt("AUSF.X71", trackingHistoryLog2.HistoryID);
            }
            else
            {
              for (int i = 0; i < trackingHistoryList1.HistoryCount; ++i)
              {
                if (trackingHistoryList1.GetHistoryAt(i).DataValues.GetField("AUS.X1") == trackingHistoryLog1.DataValues.GetField("AUS.X1"))
                {
                  trackingHistoryLog2 = trackingHistoryList1.GetHistoryAt(i);
                  break;
                }
              }
              if (trackingHistoryLog1.HistoryID != trackingHistoryLog2.HistoryID)
              {
                trackingHistoryLog1 = trackingHistoryLog2;
                this.SetFieldAt("AUSF.X71", trackingHistoryLog2.HistoryID);
              }
            }
          }
        }
        if (trackingHistoryLog1 != null)
        {
          this.SetFieldAt("AUSF.X1", trackingHistoryLog1.DataValues.GetField("AUS.X1"));
          this.SetFieldAt("AUSF.X2", trackingHistoryLog1.DataValues.GetField("AUS.X2"));
          this.SetFieldAt("AUSF.X3", trackingHistoryLog1.DataValues.GetField("AUS.X6"));
          this.SetFieldAt("AUSF.X4", trackingHistoryLog1.DataValues.GetField("AUS.X7"));
          this.SetFieldAt("AUSF.X5", trackingHistoryLog1.DataValues.GetField("AUS.X10"));
          this.SetFieldAt("AUSF.X6", trackingHistoryLog1.DataValues.GetField("AUS.X8"));
          this.SetFieldAt("AUSF.X7", trackingHistoryLog1.DataValues.GetField("AUS.X3"));
          this.SetFieldAt("AUSF.X8", trackingHistoryLog1.DataValues.GetField("AUS.X173"));
          this.SetFieldAt("AUSF.X9", trackingHistoryLog1.DataValues.GetField("AUS.X4"));
          this.SetFieldAt("AUSF.X10", trackingHistoryLog1.DataValues.GetField("AUS.X174"));
          this.SetFieldAt("AUSF.X11", trackingHistoryLog1.DataValues.GetField("AUS.X5"));
          this.SetFieldAt("AUSF.X12", trackingHistoryLog1.DataValues.GetField("AUS.X9"));
          this.SetFieldAt("AUSF.X13", trackingHistoryLog1.DataValues.GetField("AUS.X32"));
          this.SetFieldAt("AUSF.X14", trackingHistoryLog1.DataValues.GetField("AUS.X41"));
          this.SetFieldAt("AUSF.X15", trackingHistoryLog1.DataValues.GetField("AUS.X42"));
          this.SetFieldAt("AUSF.X16", trackingHistoryLog1.DataValues.GetField("AUS.X33"));
          this.SetFieldAt("AUSF.X17", trackingHistoryLog1.DataValues.GetField("AUS.X14"));
          this.SetFieldAt("AUSF.X18", trackingHistoryLog1.DataValues.GetField("AUS.X15"));
        }
        if (trackingHistoryLog2 == null || string.Compare(trackingHistoryLog2.DataValues.GetField("AUS.X1"), "DU", true) != 0)
          return;
        if (string.Compare(trackingHistoryLog2.DataValues.GetField("AUS.X199"), "not applicable", true) == 0)
        {
          this.SetFieldAt("4830", "Y");
          this.SetFieldAt("4752", "");
        }
        else
        {
          this.SetFieldAt("4830", "");
          this.SetFieldAt("4752", trackingHistoryLog2.DataValues.GetField("AUS.X199"));
        }
      }
    }

    public int GetNumberOfVerficiationTimelineLogs(VerificationTimelineType timelineType)
    {
      string str = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      if (timelineType == VerificationTimelineType.None)
        return this.root.SelectNodes(str + Mapping.verificationTimelinePath + "/Timeline").Count;
      return this.root.SelectNodes(str + Mapping.verificationTimelinePath + "/Timeline[@TimelineType ='" + timelineType.ToString() + "']").Count;
    }

    public VerificationTimelineList GetVerficiationTimelineLogs(
      VerificationTimelineType timelineType)
    {
      VerificationTimelineList verficiationTimelineLogs = new VerificationTimelineList();
      string str = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      XmlNodeList xmlNodeList;
      if (timelineType == VerificationTimelineType.None)
        xmlNodeList = this.root.SelectNodes(str + Mapping.verificationTimelinePath + "/Timeline");
      else
        xmlNodeList = this.root.SelectNodes(str + Mapping.verificationTimelinePath + "/Timeline[@TimelineType ='" + timelineType.ToString() + "']");
      if (xmlNodeList != null && xmlNodeList.Count > 0)
      {
        foreach (XmlNode e in xmlNodeList)
        {
          VerificationTimelineLog timeline = (VerificationTimelineLog) null;
          switch (timelineType)
          {
            case VerificationTimelineType.Employment:
              VerificationTimelineEmploymentLog timelineEmploymentLog = new VerificationTimelineEmploymentLog((XmlElement) e);
              timelineEmploymentLog.GetStatusFromXml((XmlElement) e);
              timeline = (VerificationTimelineLog) timelineEmploymentLog;
              break;
            case VerificationTimelineType.Income:
              VerificationTimelineIncomeLog timelineIncomeLog = new VerificationTimelineIncomeLog((XmlElement) e);
              timelineIncomeLog.GetStatusFromXml((XmlElement) e);
              timeline = (VerificationTimelineLog) timelineIncomeLog;
              break;
            case VerificationTimelineType.Asset:
              VerificationTimelineAssetLog timelineAssetLog = new VerificationTimelineAssetLog((XmlElement) e);
              timelineAssetLog.GetStatusFromXml((XmlElement) e);
              timeline = (VerificationTimelineLog) timelineAssetLog;
              break;
            case VerificationTimelineType.Obligation:
              VerificationTimelineObligationLog timelineObligationLog = new VerificationTimelineObligationLog((XmlElement) e);
              timelineObligationLog.GetStatusFromXml((XmlElement) e);
              timeline = (VerificationTimelineLog) timelineObligationLog;
              break;
          }
          if (timeline != null)
            verficiationTimelineLogs.AddTimeline(timeline);
        }
      }
      verficiationTimelineLogs.Sort();
      return verficiationTimelineLogs;
    }

    public bool AddVerificationTimelineLog(VerificationTimelineLog timelineLog)
    {
      string str = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      string xpath = str + Mapping.verificationTimelinePath + "/Timeline[@GUID='" + timelineLog.Guid + "']";
      try
      {
        this.root.SelectSingleNode(xpath)?.RemoveAll();
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddVerificationTimelineLog: Can't remove Verification Log '" + timelineLog.Guid + "' node. Exception: " + ex.Message);
        return false;
      }
      XmlElement xmlElement1 = (XmlElement) (this.root.SelectSingleNode(str + Mapping.verificationTimelinePath) ?? (XmlNode) this.createPath(str + Mapping.verificationTimelinePath)).AppendChild((XmlNode) this.xmldoc.CreateElement("Timeline"));
      xmlElement1.SetAttribute("GUID", timelineLog.Guid);
      xmlElement1.SetAttribute("CreatedOn", timelineLog.Date.ToString("MM/dd/yyyy hh:mm:ss tt"));
      xmlElement1.SetAttribute("TimelineType", timelineLog.TimelineType.ToString());
      XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
      xmlElement2.SetAttribute("BorrowerType", timelineLog.BorrowerType == LoanBorrowerType.Coborrower ? "Coborrower" : "Borrower");
      xmlElement2.SetAttribute("HowToComplete", timelineLog.HowCompleted);
      xmlElement2.SetAttribute("CompletedBy", timelineLog.CompletedBy);
      xmlElement2.SetAttribute("DateToComplete", timelineLog.DateCompleted == DateTime.MinValue ? "" : timelineLog.DateCompleted.ToString("MM/dd/yyyy"));
      xmlElement2.SetAttribute("ReviewedBy", timelineLog.ReviewedBy);
      xmlElement2.SetAttribute("DateToReview", timelineLog.DateReviewed == DateTime.MinValue ? "" : timelineLog.DateReviewed.ToString("MM/dd/yyyy"));
      xmlElement2.SetAttribute("eFolderAttached", timelineLog.EFolderAttached ? "Y" : "N");
      xmlElement2.SetAttribute("DateToUpload", timelineLog.DateUploaded == DateTime.MinValue ? "" : timelineLog.DateUploaded.ToString("MM/dd/yyyy"));
      XmlElement fieldXml = (XmlElement) xmlElement1.AppendChild((XmlNode) this.xmldoc.CreateElement("STATUS"));
      if (timelineLog.TimelineType == VerificationTimelineType.Employment)
        ((VerificationTimelineEmploymentLog) timelineLog).SetStatusToXml(fieldXml);
      else if (timelineLog.TimelineType == VerificationTimelineType.Income)
        ((VerificationTimelineIncomeLog) timelineLog).SetStatusToXml(fieldXml);
      else if (timelineLog.TimelineType == VerificationTimelineType.Asset)
        ((VerificationTimelineAssetLog) timelineLog).SetStatusToXml(fieldXml);
      else if (timelineLog.TimelineType == VerificationTimelineType.Obligation)
        ((VerificationTimelineObligationLog) timelineLog).SetStatusToXml(fieldXml);
      return true;
    }

    public bool UpdateVerificationTimelineLog(VerificationTimelineLog timelineLog)
    {
      return this.AddVerificationTimelineLog(timelineLog);
    }

    public int GetNumberOfVerficiationDocuments(VerificationTimelineType timelineType)
    {
      string str = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      if (timelineType == VerificationTimelineType.None)
        return this.root.SelectNodes(str + Mapping.verificationDocumentPath + "/Document").Count;
      return this.root.SelectNodes(str + Mapping.verificationDocumentPath + "/Document[@TimelineType ='" + timelineType.ToString() + "']").Count;
    }

    public VerificationDocumentList GetVerficiationDocuments(VerificationTimelineType timelineType)
    {
      VerificationDocumentList verficiationDocuments = new VerificationDocumentList();
      string str = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      XmlNodeList xmlNodeList;
      if (timelineType == VerificationTimelineType.None)
        xmlNodeList = this.root.SelectNodes(str + Mapping.verificationDocumentPath + "/Document");
      else
        xmlNodeList = this.root.SelectNodes(str + Mapping.verificationDocumentPath + "/Document[@TimelineType ='" + timelineType.ToString() + "']");
      if (xmlNodeList != null && xmlNodeList.Count > 0)
      {
        foreach (XmlElement e in xmlNodeList)
        {
          VerificationDocument document = new VerificationDocument(e);
          verficiationDocuments.AddDocument(document);
        }
      }
      verficiationDocuments.Sort();
      return verficiationDocuments;
    }

    public bool AddVerificationDocument(VerificationDocument documentLog)
    {
      string str1 = "BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/";
      string xpath = str1 + Mapping.verificationDocumentPath + "/Document[@GUID='" + documentLog.Guid + "']";
      try
      {
        this.root.SelectSingleNode(xpath)?.RemoveAll();
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "AddVerificationDocument: Can't remove Verification Document '" + documentLog.Guid + "' node. Exception: " + ex.Message);
        return false;
      }
      XmlElement xmlElement1 = (XmlElement) (this.root.SelectSingleNode(str1 + Mapping.verificationDocumentPath) ?? (XmlNode) this.createPath(str1 + Mapping.verificationDocumentPath)).AppendChild((XmlNode) this.xmldoc.CreateElement("Document"));
      xmlElement1.SetAttribute("GUID", documentLog.Guid);
      xmlElement1.SetAttribute("CreatedOn", documentLog.Date.ToString("MM/dd/yyyy hh:mm:ss tt"));
      xmlElement1.SetAttribute("TimelineType", documentLog.TimelineType.ToString());
      XmlElement xmlElement2 = (XmlElement) xmlElement1.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
      xmlElement2.SetAttribute("DocumentName", documentLog.DocName);
      XmlElement xmlElement3 = xmlElement2;
      DateTime dateTime;
      string str2;
      if (!(documentLog.CurrentDate == DateTime.MinValue))
      {
        dateTime = documentLog.CurrentDate;
        str2 = dateTime.ToString("MM/dd/yyyy");
      }
      else
        str2 = "";
      xmlElement3.SetAttribute("CurrentDate", str2);
      XmlElement xmlElement4 = xmlElement2;
      string str3;
      if (!(documentLog.ExpirationDate == DateTime.MinValue))
      {
        dateTime = documentLog.ExpirationDate;
        str3 = dateTime.ToString("MM/dd/yyyy");
      }
      else
        str3 = "";
      xmlElement4.SetAttribute("ExpirationDate", str3);
      return true;
    }

    public bool UpdateVerificationDocument(VerificationDocument documentLog)
    {
      return this.AddVerificationDocument(documentLog);
    }

    internal void ClearHomeCounselingProviders()
    {
      try
      {
        XmlNode oldChild = this.root.SelectSingleNode("EllieMae/HomeCounselingProviders");
        oldChild?.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in ClearHomeCounselingProviders, e: " + ex.Message);
      }
    }

    internal int GetNumberOfHomeCounselingProviders()
    {
      return this.root.SelectNodes("EllieMae/HomeCounselingProviders/HomeCounselingProvider").Count;
    }

    internal int[] GetSelectedHomeCounselingProviders()
    {
      List<int> intList = new List<int>();
      int counselingProviders = this.GetNumberOfHomeCounselingProviders();
      for (int index = 1; index <= counselingProviders; ++index)
      {
        if (this.GetFieldAt("HC" + index.ToString("00") + "01") == "Y")
          intList.Add(index);
      }
      return intList.ToArray();
    }

    internal void SetSelectedHomeCounselingProvider(int i, bool selected)
    {
      int counselingProviders = this.GetNumberOfHomeCounselingProviders();
      string empty = string.Empty;
      if (!selected)
      {
        this.SetFieldAt("HC" + i.ToString("00") + "14", "");
        this.SetFieldAt("HOC.X1", "");
      }
      else
      {
        for (int index = 1; index <= counselingProviders; ++index)
        {
          if (i == index)
            this.SetFieldAt("HC" + index.ToString("00") + "14", "Y");
          else if (this.GetFieldAt("HC" + index.ToString("00") + "14") == "Y")
            this.SetFieldAt("HC" + index.ToString("00") + "14", "");
        }
        if (!selected)
          return;
        for (int index = 2; index <= 15; ++index)
        {
          if (index != 14)
            this.SetFieldAt("HOC.X" + (object) index, this.GetFieldAt("HC" + i.ToString("00") + index.ToString("00")));
        }
        this.SetFieldAt("URLA.X153", "Y");
        this.SetFieldAt("HOC.X20", this.GetFieldAt("HC" + i.ToString("00") + "17"));
        this.SetFieldAt("URLA.X155", this.GetFieldAt("HC" + i.ToString("00") + "99"));
        this.SetFieldAt("URLA.X232", this.GetFieldAt("HOC.X2"));
        this.SetFieldAt("URLA.X233", this.GetFieldAt("HOC.X17"));
        XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/HomeCounselingProviders/HomeCounselingProvider[" + (object) i + "]");
        if (xmlElement == null || !(xmlElement.GetAttribute("_ID") != string.Empty))
          return;
        this.SetFieldAt("HOC.X1", xmlElement.GetAttribute("_ID"));
      }
    }

    internal bool UpHomeCounselingProvider(int i)
    {
      if (i == 0)
        return true;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/HomeCounselingProviders/HomeCounselingProvider");
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
        this.clearRepeatableFieldCacheValues("HC", i, 1, 17, (List<int>) null, new List<int>()
        {
          99
        }, true);
        this.clearRepeatableFieldCacheValues("HC", i + 1, 1, 17, (List<int>) null, new List<int>()
        {
          99
        }, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpHomeCounselingProvider, e: " + ex.Message);
        return false;
      }
    }

    internal bool DownHomeCounselingProvider(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/HomeCounselingProviders/HomeCounselingProvider");
        if (i < xmlNodeList.Count - 1)
        {
          XmlNode newChild = xmlNodeList[i];
          XmlNode refChild = xmlNodeList[i + 1];
          newChild.ParentNode.InsertAfter(newChild, refChild);
        }
        this.clearRepeatableFieldCacheValues("HC", i + 1, 1, 17, (List<int>) null, new List<int>()
        {
          99
        }, true);
        this.clearRepeatableFieldCacheValues("HC", i + 2, 1, 17, (List<int>) null, new List<int>()
        {
          99
        }, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownHomeCounselingProvider, e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveHomeCounselingProviderAt(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/HomeCounselingProviders/HomeCounselingProvider");
        if (xmlNodeList == null || xmlNodeList.Count == 0 || i >= xmlNodeList.Count)
          return true;
        XmlNode oldChild = xmlNodeList[i];
        oldChild.ParentNode.RemoveChild(oldChild);
        List<int> oddIDs = new List<int>() { 99 };
        for (int index = i; index < xmlNodeList.Count; ++index)
          this.clearRepeatableFieldCacheValues("HC", index + 1, 1, 17, (List<int>) null, oddIDs, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Home Counseling Provider " + (object) i + ", e: " + ex.Message);
        return false;
      }
    }

    internal int NewHomeCounselingProvider(string importSource)
    {
      if ((XmlElement) this.root.SelectSingleNode("EllieMae/HomeCounselingProviders") == null)
        this.createPath("EllieMae/HomeCounselingProviders");
      int num = this.root.SelectNodes("EllieMae/HomeCounselingProviders/HomeCounselingProvider").Count + 1;
      this.createHomeCounselingProvider("HC" + num.ToString("00") + "00", importSource);
      return num;
    }

    private void createHomeCounselingProvider(string id, string importSource)
    {
      int num1 = int.Parse(id.Substring(2, 2));
      if (id.Length > 6)
        num1 = int.Parse(id.Substring(2, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/HomeCounselingProviders/HomeCounselingProvider");
      int num2 = num1 - xmlNodeList.Count;
      int count = xmlNodeList.Count;
      XmlElement refChild = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        refChild = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/HomeCounselingProviders") ?? this.createPath("EllieMae/HomeCounselingProviders");
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("HomeCounselingProvider");
        Mapping.AddEntityId(element);
        element.SetAttribute("_ID", this.generateNewId());
        if (id.Length > 5)
        {
          string str = "HC" + (++count).ToString("00");
          this.setFieldAtXpath(element, "_SelectedIndicator", str + "01", "Y", this.currentBorrowerPair);
          this.setFieldAtXpath(element, "_AgencySource", str + "16", importSource == null ? "Manual" : "", this.currentBorrowerPair);
        }
        if (refChild != null)
          xmlElement.InsertAfter((XmlNode) element, (XmlNode) refChild);
        else
          xmlElement.AppendChild((XmlNode) element);
        refChild = element;
      }
    }

    internal void ClearNonVols()
    {
      try
      {
        int numberOfNonVols = this.GetNumberOfNonVols();
        if (numberOfNonVols > 0)
        {
          for (int index = 1; index <= numberOfNonVols; ++index)
            this.clearRepeatableFieldCacheValues("UNFL", index, 1, 9, (List<int>) null, (List<int>) null, true);
        }
        XmlNode oldChild = this.root.SelectSingleNode("EllieMae/NonVols");
        oldChild?.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in ClearNONVOL, e: " + ex.Message);
      }
    }

    private string isNonVolPOC(string desc) => desc.IndexOf("P.O.C.") > -1 ? "True" : "False";

    private string paidBy(string desc)
    {
      string str1 = string.Empty;
      int startIndex = desc.IndexOf("P.O.C.");
      if (startIndex > -1)
      {
        string[] source = desc.Substring(startIndex).Split('.');
        string str2 = source[((IEnumerable<string>) source).Count<string>() - 1];
        if (str2.IndexOf(")") > -1)
          str2 = str2.Substring(0, str2.Length - 1);
        str1 = str2.Trim();
      }
      return str1;
    }

    private string paidTo(string desc)
    {
      if (string.IsNullOrEmpty(desc))
        return desc;
      string str = string.Empty;
      int startIndex = desc.IndexOf("to ") + 3;
      int num = desc.IndexOf("(", startIndex);
      if (startIndex > -1)
        str = desc.Substring(startIndex, num - startIndex).Trim();
      return str;
    }

    internal void CreateNonVols(Dictionary<int, List<string>> nonVol)
    {
      foreach (KeyValuePair<int, List<string>> keyValuePair in nonVol)
      {
        int num = 0;
        int key = keyValuePair.Key;
        foreach (string str in keyValuePair.Value)
        {
          ++num;
          switch (num)
          {
            case 5:
              this.setFieldAtId("UNFL" + key.ToString("00") + num.ToString("00"), str == "True" ? "Y" : "N", (BorrowerPair) null);
              continue;
            case 6:
              this.setFieldAtId("UNFL" + key.ToString("00") + "09", this.paidTo(str), (BorrowerPair) null);
              this.setFieldAtId("UNFL" + key.ToString("00") + "08", str, (BorrowerPair) null);
              continue;
            default:
              this.setFieldAtId("UNFL" + key.ToString("00") + num.ToString("00"), str, (BorrowerPair) null);
              continue;
          }
        }
        this.setFieldAtId("UNFL" + key.ToString("00") + "06", this.isNonVolPOC(nonVol[key][1]) == "True" ? "Y" : "N", (BorrowerPair) null);
        string str1 = this.paidBy(nonVol[key][1]);
        this.setFieldAtId("UNFL" + key.ToString("00") + "07", string.IsNullOrEmpty(str1) ? nonVol[key][6] : str1, (BorrowerPair) null);
      }
    }

    internal int GetNumberOfNonVols() => this.root.SelectNodes("EllieMae/NonVols/NonVol").Count;

    internal int[] GetSelectedNonVol()
    {
      List<int> intList = new List<int>();
      int numberOfNonVols = this.GetNumberOfNonVols();
      for (int index = 1; index <= numberOfNonVols; ++index)
      {
        if (this.GetFieldAt("UNFL" + index.ToString("00") + "01") == "Y")
          intList.Add(index);
      }
      return intList.ToArray();
    }

    internal bool UpNonVol(int i)
    {
      if (i == 0)
        return true;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/NonVols/NonVol");
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpNONVOL, e: " + ex.Message);
        return false;
      }
    }

    internal bool DownNonVol(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/NonVols/NonVol");
        if (i < xmlNodeList.Count - 1)
        {
          XmlNode newChild = xmlNodeList[i];
          XmlNode refChild = xmlNodeList[i + 1];
          newChild.ParentNode.InsertAfter(newChild, refChild);
        }
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownNONVOL, e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveNonVolAt(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/NonVols/NonVol");
        if (xmlNodeList == null || xmlNodeList.Count == 0 || i >= xmlNodeList.Count)
          return true;
        XmlNode oldChild = xmlNodeList[i];
        oldChild.ParentNode.RemoveChild(oldChild);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove NONVOL " + (object) i + ", e: " + ex.Message);
        return false;
      }
    }

    internal int NewNonVol(string importSource)
    {
      if ((XmlElement) this.root.SelectSingleNode("EllieMae/NonVols") == null)
        this.createPath("EllieMae/NonVols");
      int num = this.root.SelectNodes("EllieMae/NonVols/NonVol").Count + 1;
      this.createRepeatableNode("UNFL" + num.ToString("00") + "00", "EllieMae/NonVols", "NonVol", false);
      return num;
    }

    internal void ClearAffiliates()
    {
      try
      {
        int numberOfAffiliates = this.GetNumberOfAffiliates();
        if (numberOfAffiliates > 0)
        {
          for (int index = 1; index <= numberOfAffiliates; ++index)
            this.clearRepeatableFieldCacheValues("AB", index, 1, 28, (List<int>) null, (List<int>) null, true);
        }
        XmlNode oldChild = this.root.SelectSingleNode("EllieMae/AffiliatedBusinessArrangements");
        oldChild?.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in ClearAffiliates, e: " + ex.Message);
      }
    }

    internal int GetNumberOfAffiliates()
    {
      return this.root.SelectNodes("EllieMae/AffiliatedBusinessArrangements/Affiliate").Count;
    }

    internal bool UpAffiliate(int i)
    {
      if (i == 0)
        return true;
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/AffiliatedBusinessArrangements/Affiliate");
        XmlNode newChild = xmlNodeList[i];
        XmlNode refChild = xmlNodeList[i - 1];
        newChild.ParentNode.InsertBefore(newChild, refChild);
        this.clearRepeatableFieldCacheValues("AB", i, 1, 28, (List<int>) null, (List<int>) null, true);
        this.clearRepeatableFieldCacheValues("AB", i + 1, 1, 28, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in UpAffiliate, e: " + ex.Message);
        return false;
      }
    }

    internal bool DownAffiliate(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/AffiliatedBusinessArrangements/Affiliate");
        if (i < xmlNodeList.Count - 1)
        {
          XmlNode newChild = xmlNodeList[i];
          XmlNode refChild = xmlNodeList[i + 1];
          newChild.ParentNode.InsertAfter(newChild, refChild);
        }
        this.clearRepeatableFieldCacheValues("AB", i + 1, 1, 28, (List<int>) null, (List<int>) null, true);
        this.clearRepeatableFieldCacheValues("AB", i + 2, 1, 28, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in DownAffiliate, e: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveAffiliateAt(int i)
    {
      try
      {
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/AffiliatedBusinessArrangements/Affiliate");
        if (xmlNodeList == null || xmlNodeList.Count == 0 || i >= xmlNodeList.Count)
          return true;
        XmlNode oldChild = xmlNodeList[i];
        oldChild.ParentNode.RemoveChild(oldChild);
        for (int index = i; index < xmlNodeList.Count; ++index)
          this.clearRepeatableFieldCacheValues("AB", index + 1, 1, 28, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Affiliate At " + (object) i + ", e: " + ex.Message);
        return false;
      }
    }

    private void clearRepeatableFieldCacheValues(
      string prefixID,
      int index,
      int fieldNoStart,
      int fieldNoEnd,
      List<int> skipIDs,
      List<int> oddIDs)
    {
      this.clearRepeatableFieldCacheValues(prefixID, index, fieldNoStart, fieldNoEnd, skipIDs, oddIDs, false);
    }

    private void clearRepeatableFieldCacheValues(
      string prefixID,
      int index,
      int fieldNoStart,
      int fieldNoEnd,
      List<int> skipIDs,
      List<int> oddIDs,
      bool clearElement)
    {
      for (int index1 = fieldNoStart; index1 <= fieldNoEnd; ++index1)
      {
        if (skipIDs == null || !skipIDs.Contains(index1))
        {
          Field field = index != -1 ? this.GetFieldObject(prefixID + index.ToString("00") + index1.ToString("00"), this.currentBorrowerPair) : this.GetFieldObject(prefixID + (object) index1, this.currentBorrowerPair);
          if (field != null)
          {
            field.CachedValue = (string) null;
            if (clearElement)
            {
              field.CachedElement = (XmlElement) null;
              if (this.cachedElements != null && field.XPathElement != null && this.cachedElements.ContainsKey(field.XPathElement))
                this.cachedElements.Remove(field.XPathElement);
            }
          }
        }
      }
      if (oddIDs == null)
        return;
      for (int index2 = 0; index2 < oddIDs.Count; ++index2)
      {
        Field field = index != -1 ? this.GetFieldObject(prefixID + index.ToString("00") + oddIDs[index2].ToString("00"), this.currentBorrowerPair) : this.GetFieldObject(prefixID + (object) oddIDs[index2], this.currentBorrowerPair);
        if (field != null)
        {
          field.CachedValue = (string) null;
          if (clearElement)
          {
            field.CachedElement = (XmlElement) null;
            if (this.cachedElements != null && field.XPathElement != null && this.cachedElements.ContainsKey(field.XPathElement))
              this.cachedElements.Remove(field.XPathElement);
          }
        }
      }
    }

    internal int NewAffiliate()
    {
      if ((XmlElement) this.root.SelectSingleNode("EllieMae/AffiliatedBusinessArrangements") == null)
        this.createPath("EllieMae/AffiliatedBusinessArrangements");
      int num = this.root.SelectNodes("EllieMae/AffiliatedBusinessArrangements/Affiliate").Count + 1;
      this.createAffiliate("AB" + num.ToString("00") + "00");
      return num;
    }

    private void createAffiliate(string id)
    {
      int num1 = int.Parse(id.Substring(2, 2));
      if (id.Length > 6)
        num1 = int.Parse(id.Substring(2, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/AffiliatedBusinessArrangements/Affiliate");
      int num2 = num1 - xmlNodeList.Count;
      XmlElement refChild = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        refChild = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode("EllieMae/AffiliatedBusinessArrangements") ?? this.createPath("EllieMae/AffiliatedBusinessArrangements");
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("Affiliate");
        element.SetAttribute("_ID", this.generateNewId());
        Mapping.AddEntityId(element);
        if (refChild != null)
          xmlElement.InsertAfter((XmlNode) element, (XmlNode) refChild);
        else
          xmlElement.AppendChild((XmlNode) element);
        refChild = element;
      }
    }

    internal bool CreateSubsetConstructionLinkedFields(string[] linkedFields, string[] linkedValues)
    {
      XmlElement path;
      try
      {
        XmlElement oldChild = (XmlElement) this.root.SelectSingleNode(Mapping.LINKSYNCNODE);
        oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
        path = this.createPath(Mapping.LINKSYNCNODE);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "CreateSubsetConstructionLinkedFields: Can't create subset fields for construction link and sync node. Exception: " + ex.Message);
        return false;
      }
      for (int index = 0; index < linkedFields.Length; ++index)
      {
        XmlElement xmlElement = (XmlElement) path.AppendChild((XmlNode) this.xmldoc.CreateElement("FIELD"));
        xmlElement.SetAttribute("id", linkedFields[index]);
        xmlElement.SetAttribute("val", linkedValues[index]);
      }
      return true;
    }

    internal List<KeyValuePair<string, string>> GetSubsetConstructionLinkedFieldValues()
    {
      XmlNode xmlNode = this.root.SelectSingleNode(Mapping.LINKSYNCNODE);
      if (xmlNode == null)
        return new List<KeyValuePair<string, string>>();
      XmlNodeList xmlNodeList = xmlNode.SelectNodes("FIELD");
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return new List<KeyValuePair<string, string>>();
      List<KeyValuePair<string, string>> linkedFieldValues = new List<KeyValuePair<string, string>>();
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        string attribute1 = xmlElement.GetAttribute("id");
        string attribute2 = xmlElement.GetAttribute("val");
        linkedFieldValues.Add(new KeyValuePair<string, string>(attribute1, attribute2));
      }
      return linkedFieldValues;
    }

    internal string GetSubsetConstructionLinkedFieldValue(string fieldID)
    {
      XmlNode xmlNode = this.root.SelectSingleNode(Mapping.LINKSYNCNODE);
      if (xmlNode == null)
        return "";
      XmlNodeList xmlNodeList = xmlNode.SelectNodes("FIELD");
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return "";
      fieldID = fieldID.Replace("LINKSYNC.", "");
      foreach (XmlElement xmlElement in xmlNodeList)
      {
        string attribute = xmlElement.GetAttribute("id");
        if (string.Compare(fieldID, attribute, true) == 0)
          return xmlElement.GetAttribute("val");
      }
      return "";
    }

    internal void RemoveSubsetConstructionLinkedFieldValues()
    {
      try
      {
        XmlElement oldChild = (XmlElement) this.root.SelectSingleNode(Mapping.LINKSYNCNODE);
        oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
      }
      catch (Exception ex)
      {
      }
    }

    private static void AddEntityId(XmlElement elm)
    {
      if (elm.HasAttribute("_eid"))
        return;
      elm.SetAttribute("_eid", Guid.NewGuid().ToString());
    }

    private static void AddMissingEntityIds(XmlElement rootelm)
    {
      foreach (string xpathsForBadEntityId in Mapping.XPathsForBadEntityIds)
      {
        foreach (XmlElement selectNode in rootelm.SelectNodes(xpathsForBadEntityId))
        {
          if (selectNode.HasAttribute("_eid") && !Guid.TryParse(selectNode.GetAttribute("_eid"), out Guid _))
            selectNode.RemoveAttribute("_eid");
        }
      }
      foreach (string xpathsForEntityId in Mapping.XPathsForEntityIds)
      {
        foreach (XmlElement selectNode in rootelm.SelectNodes(xpathsForEntityId))
          Mapping.AddEntityId(selectNode);
      }
      rootelm.SetAttribute("ContainsEntityIdsFor231", "Y");
    }

    public static bool AddMissingEntityIds(XmlDocument xmlDoc)
    {
      XmlElement rootelm = (XmlElement) xmlDoc.SelectSingleNode("LOAN_APPLICATION");
      if (rootelm.HasAttribute("ContainsEntityIdsFor231"))
        return false;
      Mapping.AddMissingEntityIds(rootelm);
      return true;
    }

    public static void RemoveInvalidCharFromAllRoles(XmlDocument xmlDoc)
    {
      XmlElement xmlElement = (XmlElement) xmlDoc.DocumentElement.SelectSingleNode("EllieMae/SettingsInfo");
      if (xmlElement == null)
        return;
      string attribute = xmlElement.GetAttribute("AllRoles");
      if (string.IsNullOrEmpty(attribute) || attribute.IndexOf("\0") < 0)
        return;
      string str = attribute.Replace("\0", ":");
      xmlElement.SetAttribute("AllRoles", str);
    }

    internal int GetNumberOfNonBorrowingOwnerContact()
    {
      return this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner").Count;
    }

    internal string GetEidOfNonBorrowingOwnerContactAt(int i)
    {
      return ((XmlElement) this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner")[i - 1]).GetAttribute("_eid");
    }

    internal int NewNonBorrowingOwnerContact()
    {
      try
      {
        if ((XmlElement) this.root.SelectSingleNode("EllieMae/NonBorrowingOwnerContacts") == null)
          this.createPath("EllieMae/NonBorrowingOwnerContacts");
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner");
        this.createRepeatableNode("NBOC" + (xmlNodeList.Count + 1).ToString("00") + "00", "EllieMae/NonBorrowingOwnerContacts", "NonBorrowingOwner", true);
        return xmlNodeList.Count;
      }
      catch (Exception ex)
      {
        return -1;
      }
    }

    internal bool RemoveNonBorrowingOwnerContactAt(int i)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner")[i - 1];
        XmlElement xmlElement1 = (XmlElement) selectNode;
        if (xmlElement1.HasAttribute("BorrowerVestingRecordID"))
        {
          string attribute = xmlElement1.GetAttribute("BorrowerVestingRecordID");
          if (attribute != "")
          {
            XmlNodeList xmlNodeList = this.root.SelectNodes("_CLOSING_DOCUMENTS/TRUSTEE");
            if (xmlNodeList != null && xmlNodeList.Count > 0)
            {
              for (int i1 = 0; i1 < xmlNodeList.Count; ++i1)
              {
                XmlElement xmlElement2 = (XmlElement) xmlNodeList[i1];
                if (xmlElement2.HasAttribute("Guid") && string.Compare(attribute, xmlElement2.GetAttribute("Guid"), true) == 0)
                {
                  xmlNodeList[i1].ParentNode.RemoveChild(xmlNodeList[i1]);
                  break;
                }
              }
            }
          }
        }
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner node. Error: " + ex.Message);
        return false;
      }
    }

    internal int GetNBOLinkedVesting(int VestingIndex)
    {
      XmlNodeList xmlNodeList1 = this.root.SelectNodes("_CLOSING_DOCUMENTS/TRUSTEE");
      if (xmlNodeList1.Count <= VestingIndex)
        return 0;
      XmlElement xmlElement1 = (XmlElement) xmlNodeList1[VestingIndex];
      if (xmlElement1 == null || !xmlElement1.HasAttribute("_NBORecordID"))
        return 0;
      string attribute = xmlElement1.GetAttribute("_NBORecordID");
      if (string.IsNullOrEmpty(attribute))
        return 0;
      XmlNodeList xmlNodeList2 = this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner");
      if (xmlNodeList2 == null || xmlNodeList2.Count == 0)
        return 0;
      for (int i = 0; i < xmlNodeList2.Count; ++i)
      {
        XmlElement xmlElement2 = (XmlElement) xmlNodeList2[i];
        if (xmlElement2.HasAttribute("_ID") && string.Compare(attribute, xmlElement2.GetAttribute("_ID"), true) == 0)
          return i + 1;
      }
      return 0;
    }

    internal int GetNBOLinkedVesting(string nboGUID)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/NonBorrowingOwnerContacts/NonBorrowingOwner");
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return 0;
      for (int i = 0; i < xmlNodeList.Count; ++i)
      {
        XmlElement xmlElement = (XmlElement) xmlNodeList[i];
        if (xmlElement.HasAttribute("_ID") && string.Compare(nboGUID, xmlElement.GetAttribute("_ID"), true) == 0)
          return i + 1;
      }
      return 0;
    }

    internal int GetVestingLinkedNBO(string vestingGUID)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("_CLOSING_DOCUMENTS/TRUSTEE");
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return 0;
      for (int i = 0; i < xmlNodeList.Count; ++i)
      {
        XmlElement xmlElement = (XmlElement) xmlNodeList[i];
        if (xmlElement.HasAttribute("Guid") && string.Compare(vestingGUID, xmlElement.GetAttribute("Guid"), true) == 0)
          return i + 1;
      }
      return 0;
    }

    private string alternateNamePath(bool borrower)
    {
      return "BORROWER[@BorrowerID=\"" + (borrower ? this.currentBorrowerPair.Borrower.Id : this.currentBorrowerPair.CoBorrower.Id) + "\"]/" + "URLA2020/URLAAlternateNames";
    }

    internal int GetNumberOfURLAAlternateNames(bool borrower)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.alternateNamePath(borrower) + "/URLAAlternateName");
      return xmlNodeList == null ? 0 : xmlNodeList.Count;
    }

    internal IList<URLAAlternateName> GetURLAAlternateNames(bool borrower)
    {
      IList<URLAAlternateName> urlaAlternateNames = (IList<URLAAlternateName>) new List<URLAAlternateName>();
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.alternateNamePath(borrower) + "/URLAAlternateName");
      if (xmlNodeList != null)
      {
        foreach (XmlElement xmlElement in xmlNodeList)
          urlaAlternateNames.Add(new URLAAlternateName(xmlElement.HasAttribute("_ID") ? xmlElement.Attributes["_ID"].Value : "", xmlElement.HasAttribute("FirstName") ? xmlElement.Attributes["FirstName"].Value : "", xmlElement.HasAttribute("MiddleName") ? xmlElement.Attributes["MiddleName"].Value : "", xmlElement.HasAttribute("LastName") ? xmlElement.Attributes["LastName"].Value : "", xmlElement.HasAttribute("Suffix") ? xmlElement.Attributes["Suffix"].Value : "", xmlElement.HasAttribute("FullName") ? xmlElement.Attributes["FullName"].Value : ""));
      }
      return urlaAlternateNames;
    }

    internal void UpdateURLAAlternateNames(bool borrower, IList<URLAAlternateName> akaNameList)
    {
      XmlNode oldChild = this.root.SelectSingleNode(this.alternateNamePath(borrower));
      if (oldChild != null)
      {
        try
        {
          oldChild.ParentNode.RemoveChild(oldChild);
        }
        catch (Exception ex)
        {
          Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove " + this.alternateNamePath(borrower) + " node. Error: " + ex.Message);
          return;
        }
      }
      if (akaNameList == null || akaNameList.Count == 0)
        return;
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(this.alternateNamePath(borrower)) ?? this.createPath(this.alternateNamePath(borrower));
      for (int index = 0; index < akaNameList.Count; ++index)
      {
        XmlElement element = this.xmldoc.CreateElement("URLAAlternateName");
        string str1 = (akaNameList[index].ID ?? "") != "" ? akaNameList[index].ID : Guid.NewGuid().ToString();
        element.SetAttribute("_eid", str1);
        element.SetAttribute("ID", this.generateNewId());
        if (str1.Length > 11)
        {
          string str2 = str1.Substring(0, 10);
          this.setFieldAtXpath(element, "FirstName", str2 + "01", akaNameList[index].FirstName, this.currentBorrowerPair);
          this.setFieldAtXpath(element, "MiddleName", str2 + "02", akaNameList[index].MiddleName, this.currentBorrowerPair);
          this.setFieldAtXpath(element, "LastName", str2 + "03", akaNameList[index].LastName, this.currentBorrowerPair);
          this.setFieldAtXpath(element, "Suffix", str2 + "04", akaNameList[index].Suffix, this.currentBorrowerPair);
          this.setFieldAtXpath(element, "FullName", str2 + "05", akaNameList[index].FullName, this.currentBorrowerPair);
        }
        xmlElement.AppendChild((XmlNode) element);
      }
    }

    internal int GetNumberOfGoodFaithChangeOfCircumstance()
    {
      return this.root.SelectNodes("EllieMae/AlertChangeCircumstanceList/AlertChangeCircumstance").Count;
    }

    internal int NewGoodFaithChangeOfCircumstance(string alertFieldID)
    {
      if ((XmlElement) this.root.SelectSingleNode("EllieMae/AlertChangeCircumstanceList") == null)
        this.createPath("EllieMae/AlertChangeCircumstanceList");
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/AlertChangeCircumstanceList/AlertChangeCircumstance");
      int num = xmlNodeList.Count + 1;
      this.createRepeatableNode("XCOC" + num.ToString("00") + "00", "EllieMae/AlertChangeCircumstanceList", "AlertChangeCircumstance", true);
      this.SetFieldAt("XCOC" + num.ToString("00") + "01", alertFieldID);
      return xmlNodeList.Count;
    }

    internal bool RemoveGoodFaithChangeOfCircumstance(int i)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes("EllieMae/AlertChangeCircumstanceList/AlertChangeCircumstance")[i - 1];
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove EllieMae/AlertChangeCircumstanceList/AlertChangeCircumstance node. Error: " + ex.Message);
        return false;
      }
    }

    internal bool RemoveAllGoodFaithChangeOfCircumstance()
    {
      XmlNode oldChild = this.root.SelectSingleNode("EllieMae/AlertChangeCircumstanceList");
      if (oldChild == null)
        return true;
      try
      {
        oldChild.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove EllieMae/AlertChangeCircumstanceList node. Error: " + ex.Message);
        return false;
      }
      return true;
    }

    private void createRepeatableNode(
      string id,
      string pathName,
      string nodeName,
      bool useGUIDforID)
    {
      int num1;
      if (id.StartsWith("URLARGG") && id.Length >= 11)
      {
        num1 = id.Length <= 11 ? int.Parse(id.Substring(7, 2)) : int.Parse(id.Substring(7, 3));
        int length = pathName.IndexOf("/" + nodeName + "[");
        if (length == -1)
          return;
        pathName = pathName.Substring(0, length);
      }
      else if (id.StartsWith("URLAROIS") && id.Length >= 12)
      {
        num1 = id.Length <= 12 ? int.Parse(id.Substring(8, 2)) : int.Parse(id.Substring(8, 3));
        int length = pathName.IndexOf("/" + nodeName + "[");
        if (length == -1)
          return;
        pathName = pathName.Substring(0, length);
      }
      else if (nodeName == "DEPOSIT" && id.StartsWith("DD") && id.Length >= 6)
      {
        num1 = id.Length <= 6 ? int.Parse(id.Substring(2, 2)) : int.Parse(id.Substring(2, 3));
        int length = pathName.IndexOf("/" + nodeName + "[");
        if (length == -1)
          return;
        pathName = pathName.Substring(0, length);
      }
      else if (id.StartsWith("TQLGSE"))
        num1 = id.Length <= 10 ? int.Parse(id.Substring(6, 2)) : int.Parse(id.Substring(7, 2));
      else if (id.StartsWith("URLA"))
        num1 = int.Parse(id.Substring(7, 2));
      else if (id.StartsWith("SFC"))
        num1 = int.Parse(id.Substring(3, 2));
      else if (id.StartsWith("FEMA"))
      {
        num1 = id.Length <= 8 ? int.Parse(id.Substring(4, 2)) : int.Parse(id.Substring(4, 3));
        int length = pathName.IndexOf("/" + nodeName + "[");
        if (length == -1)
          return;
        pathName = pathName.Substring(0, length);
      }
      else
        num1 = id.Length <= 8 ? int.Parse(id.Substring(4, 2)) : int.Parse(id.Substring(4, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes(pathName + "/" + nodeName);
      int num2 = num1 - xmlNodeList.Count;
      XmlElement refChild = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        refChild = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      XmlElement xmlElement = (XmlElement) this.root.SelectSingleNode(pathName) ?? this.createPath(pathName);
      while (num2-- > 0)
      {
        switch (nodeName)
        {
          case "DEPOSIT":
            this.NewDeposit();
            continue;
          case "OtherIncomeSource":
            this.NewOtherIncomeSource();
            continue;
          case "GiftGrant":
            this.NewGiftGrant();
            continue;
          default:
            XmlElement element = this.xmldoc.CreateElement(nodeName);
            Mapping.AddEntityId(element);
            element.SetAttribute("_ID", useGUIDforID ? Guid.NewGuid().ToString() : this.generateNewId());
            if (refChild != null)
              xmlElement.InsertAfter((XmlNode) element, (XmlNode) refChild);
            else
              xmlElement.AppendChild((XmlNode) element);
            refChild = element;
            continue;
        }
      }
    }

    public static bool UseNewCompliance(Decimal versionToRunNewLogic, string complianceVersion)
    {
      if (string.Compare(complianceVersion, "No Log", true) == 0 || string.IsNullOrEmpty(complianceVersion))
        return true;
      string[] strArray = complianceVersion.Split('.');
      if (strArray == null || strArray.Length == 0)
        return true;
      complianceVersion = strArray[0] + (strArray.Length > 1 ? "." + (object) Utils.ParseInt((object) strArray[1]) ?? "" : "") + (strArray.Length > 2 ? string.Concat((object) Utils.ParseInt((object) strArray[2])) : "");
      return Utils.ParseDecimal((object) complianceVersion) >= versionToRunNewLogic;
    }

    private void createOtherAsset(string id, BorrowerPair pair)
    {
      int num1 = int.Parse(id.Substring(7, 2));
      if (id.Length > 11)
        num1 = int.Parse(id.Substring(7, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes((pair == null ? "BORROWER" + this.brwPredicate : "BORROWER" + this.brwPredicate.Replace(this.brwId, pair.Borrower.Id)) + "/URLA2020/OtherAssets/OTHER_ASSET");
      int num2 = num1 - xmlNodeList.Count;
      XmlElement xmlElement = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        xmlElement = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      int num3 = xmlNodeList.Count;
      if (num3 == 0)
        num3 = 1;
      XmlNode newChild = this.brwElm.SelectSingleNode("URLA2020/OtherAssets");
      if (newChild == null)
      {
        newChild = this.brwElm.SelectSingleNode("URLA2020") != null ? this.brwElm.SelectSingleNode("URLA2020") : (XmlNode) this.xmldoc.CreateElement("URLA2020");
        newChild.AppendChild((XmlNode) this.xmldoc.CreateElement("OtherAssets"));
        this.brwElm.AppendChild(newChild);
      }
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("OTHER_ASSET");
        Mapping.AddEntityId(element);
        element.SetAttribute("ID", this.generateNewId());
        this.AppendSameTypeChild(newChild.Name == "OtherAssets" ? (XmlElement) newChild : (XmlElement) newChild.SelectSingleNode("OtherAssets"), element, "OTHER_ASSET");
        ++num3;
      }
    }

    private void createOtherLiability(string id, BorrowerPair pair)
    {
      int num1 = int.Parse(id.Substring(7, 2));
      if (id.Length > 11)
        num1 = int.Parse(id.Substring(7, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes((pair == null ? "BORROWER" + this.brwPredicate : "BORROWER" + this.brwPredicate.Replace(this.brwId, pair.Borrower.Id)) + "/URLA2020/OtherLiabilities/OTHER_LIABILITY");
      int num2 = num1 - xmlNodeList.Count;
      XmlElement xmlElement1;
      XmlElement xmlElement2;
      if (xmlNodeList.Count == 0)
        xmlElement2 = (XmlElement) null;
      else
        xmlElement1 = xmlElement2 = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      xmlElement1 = xmlElement2;
      if (xmlNodeList.Count == 0)
        ;
      XmlNode parent = this.brwElm.SelectSingleNode("URLA2020/OtherLiabilities");
      if (parent == null)
      {
        XmlNode xmlNode = this.brwElm.SelectSingleNode("URLA2020");
        parent = (xmlNode == null ? (XmlNode) this.xmldoc.CreateElement("URLA2020") : xmlNode).AppendChild((XmlNode) this.xmldoc.CreateElement("OtherLiabilities"));
        this.brwElm.AppendChild(parent.ParentNode);
      }
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("OTHER_LIABILITY");
        Mapping.AddEntityId(element);
        element.SetAttribute("ID", this.generateNewId());
        this.AppendSameTypeChild((XmlElement) parent, element, "OTHER_LIABILITY");
      }
    }

    internal int GetNumberOfOtherLiabilities()
    {
      return this.root.SelectNodes("BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/" + "/URLA2020/OtherLiabilities/OTHER_LIABILITY").Count;
    }

    internal void MoveMortgageToTop(int n)
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("REO_PROPERTY" + this.brwPredicate);
      XmlNode xmlNode = xmlNodeList[0];
      XmlNode newChild = xmlNodeList.Item(n - 1);
      XmlNodeList childNodes = this.lockRoot.ChildNodes;
      for (int i = 0; i < childNodes.Count; ++i)
      {
        XmlElement xmlElement = (XmlElement) childNodes[i];
        string attribute = xmlElement.GetAttribute("id");
        if (attribute.StartsWith("FM"))
        {
          int num = int.Parse(attribute.Substring(2, 2));
          string str = attribute.Substring(4, 2);
          if (attribute.Length > 6)
          {
            num = int.Parse(attribute.Substring(2, 3));
            str = attribute.Substring(5, 2);
          }
          if (num == n)
          {
            string id = "FM01" + str;
            this.RemoveLockCacheAt(attribute);
            this.RemoveLockCacheAt(id);
            xmlElement.SetAttribute("id", id);
          }
          else if (num < n)
          {
            string id = "FM" + (num + 1).ToString("00") + str;
            this.RemoveLockCacheAt(attribute);
            this.RemoveLockCacheAt(id);
            xmlElement.SetAttribute("id", id);
          }
        }
      }
      xmlNode.ParentNode.PrependChild(newChild);
    }

    internal int GetNumberOfAdditionalLoans()
    {
      return this.root.SelectNodes("BORROWER[@BorrowerID=\"" + this.currentBorrowerPair.Id + "\"]/" + "/URLA2020/AdditionalLoans/Additional_Loan").Count;
    }

    internal int NewProvidedDocument()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes(this.providedDocumentPath);
      this.createProvidedDocument("DOCPROV" + (xmlNodeList.Count + 1).ToString("00") + "00", (BorrowerPair) null);
      return xmlNodeList.Count;
    }

    internal int GetNumberOfProvidedDocuments()
    {
      return this.root.SelectNodes(this.providedDocumentPath).Count;
    }

    internal bool RemoveProvidedDocumentAt(int n)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes(this.providedDocumentPath)[n];
        this.logList.RemoveVerif(this.GetFieldAt("DOCPROV" + (n + 1).ToString("00") + "99"));
        selectNode.ParentNode.RemoveChild(selectNode);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove ProvidedDocument " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    private void createProvidedDocument(string id, BorrowerPair pair)
    {
      int num1 = int.Parse(id.Substring(7, 2));
      if (id.Length > 11)
        num1 = int.Parse(id.Substring(7, 3));
      XmlNodeList xmlNodeList = this.root.SelectNodes((pair == null ? "BORROWER" + this.brwPredicate : "BORROWER" + this.brwPredicate.Replace(this.brwId, pair.Borrower.Id)) + "/ProvidedDocuments/PROVIDED_DOCUMENT");
      int num2 = num1 - xmlNodeList.Count;
      XmlElement xmlElement = (XmlElement) null;
      if (xmlNodeList.Count != 0)
        xmlElement = (XmlElement) xmlNodeList[xmlNodeList.Count - 1];
      int num3 = xmlNodeList.Count;
      if (num3 == 0)
        num3 = 1;
      XmlNode xmlNode = this.brwElm.SelectSingleNode("ProvidedDocuments") ?? this.brwElm.AppendChild((XmlNode) this.xmldoc.CreateElement("ProvidedDocuments"));
      while (num2-- > 0)
      {
        XmlElement element = this.xmldoc.CreateElement("PROVIDED_DOCUMENT");
        Mapping.AddEntityId(element);
        element.SetAttribute("_ID", this.generateNewId());
        this.AppendSameTypeChild(xmlNode.Name == "ProvidedDocuments" ? (XmlElement) xmlNode : (XmlElement) this.brwElm.SelectSingleNode("ProvidedDocuments"), element, "PROVIDED_DOCUMENT");
        ++num3;
      }
    }

    internal int GetNumberOfSpecialFeatureCodes()
    {
      return this.root.SelectNodes("EllieMae/SpecialFeatureCode").Count;
    }

    internal int NewSpecialFeatureCode()
    {
      this.createRepeatableNode("SFC" + (this.root.SelectNodes("EllieMae/SpecialFeatureCode").Count + 1).ToString("00") + "01", "EllieMae", "SpecialFeatureCode", true);
      return this.root.SelectNodes("EllieMae/SpecialFeatureCode").Count;
    }

    internal void RemoveSpecialFeatureCodes()
    {
      XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/SpecialFeatureCode");
      if (xmlNodeList == null || xmlNodeList.Count == 0)
        return;
      for (int index = xmlNodeList.Count - 1; index >= 0; --index)
      {
        xmlNodeList[index].ParentNode.RemoveChild(xmlNodeList[index]);
        this.clearRepeatableFieldCacheValues("SFC", index, 1, 5, (List<int>) null, (List<int>) null, true);
      }
    }

    internal bool RemoveSpecialFeatureCodeAt(int n)
    {
      try
      {
        XmlNode selectNode = this.root.SelectNodes("EllieMae/SpecialFeatureCode")[n];
        selectNode.ParentNode.RemoveChild(selectNode);
        XmlNodeList xmlNodeList = this.root.SelectNodes("EllieMae/SpecialFeatureCode");
        for (int index = 0; index < xmlNodeList.Count; ++index)
          this.clearRepeatableFieldCacheValues("SFC", index + 1, 1, 5, (List<int>) null, (List<int>) null, true);
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Cannot remove Special Feature Code " + (object) n + ", e: " + ex.Message);
        return false;
      }
    }

    internal void SetEDSData(EDSDataType dataType, object data)
    {
      string nodeRequireddata = Mapping.EDS_NODE_REQUIREDDATA;
      PaymentScheduleSnapshot scheduleSnapshot = (PaymentScheduleSnapshot) null;
      List<FundingFee> fundingFeeList = (List<FundingFee>) null;
      Dictionary<string, object> balancingWorksheet = (Dictionary<string, object>) null;
      Dictionary<string, string> dictionary1 = (Dictionary<string, string>) null;
      string str1;
      switch (dataType)
      {
        case EDSDataType.PaymentSchedule_Amortization:
          scheduleSnapshot = (PaymentScheduleSnapshot) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_AMORTIZATION;
          break;
        case EDSDataType.PaymentSchedule_WorstCase:
          scheduleSnapshot = (PaymentScheduleSnapshot) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_WORSTCASE;
          break;
        case EDSDataType.UCD_LE:
          this.eds_UCD_Dictionary = (Dictionary<string, string>) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_UCD_LE;
          break;
        case EDSDataType.UCD_CD:
          this.eds_UCD_Dictionary = (Dictionary<string, string>) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_UCD_CD;
          break;
        case EDSDataType.FundingFee:
          fundingFeeList = (List<FundingFee>) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_FUNDINGFEE;
          break;
        case EDSDataType.BalancingWorksheet:
          balancingWorksheet = (Dictionary<string, object>) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET;
          break;
        case EDSDataType.HELOC:
          this.eds_UCD_Dictionary = (Dictionary<string, string>) data;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_HELOC;
          break;
        case EDSDataType.ConditionLetter:
          dictionary1 = data as Dictionary<string, string>;
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_CONDITIONLETTER;
          break;
        case EDSDataType.Error:
          str1 = nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATAERROR;
          break;
        default:
          return;
      }
      XmlElement path1;
      try
      {
        XmlElement oldChild = (XmlElement) this.root.SelectSingleNode(str1);
        oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
        path1 = this.createPath(str1);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "SetEDSData: Can't create node \"" + str1 + "\". Exception: " + ex.Message);
        return;
      }
      XmlNode targetXmlElement = (XmlNode) path1;
      switch (dataType)
      {
        case EDSDataType.PaymentSchedule_Amortization:
        case EDSDataType.PaymentSchedule_WorstCase:
          if (scheduleSnapshot == null)
            break;
          try
          {
            scheduleSnapshot.ToXml((XmlNode) path1, this.xmldoc);
            break;
          }
          catch (Exception ex)
          {
            Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Can't add payment schedule snapshot to loan.em. Exception: " + ex.Message);
            break;
          }
        case EDSDataType.UCD_LE:
        case EDSDataType.UCD_CD:
        case EDSDataType.HELOC:
          using (Dictionary<string, string>.Enumerator enumerator = ((Dictionary<string, string>) data).GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, string> current = enumerator.Current;
              XmlElement element = this.xmldoc.CreateElement("Field");
              targetXmlElement.AppendChild((XmlNode) element);
              element.SetAttribute("ID", current.Key);
              element.SetAttribute("Value", current.Value);
            }
            break;
          }
        case EDSDataType.FundingFee:
          if (fundingFeeList == null)
            break;
          using (List<FundingFee>.Enumerator enumerator = fundingFeeList.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              FundingFee current = enumerator.Current;
              XmlElement element = this.xmldoc.CreateElement("Fee");
              targetXmlElement.AppendChild((XmlNode) element);
              element.SetAttribute("LineID", current.LineID);
              element.SetAttribute("CDLineID", current.CDLineID);
              XmlElement xmlElement1 = element;
              double num = current.Amount;
              string str2 = num.ToString();
              xmlElement1.SetAttribute("Amount", str2);
              element.SetAttribute("BalanceChecked", current.BalanceChecked ? "Y" : "N");
              element.SetAttribute("FeeDescription", current.FeeDescription);
              XmlElement xmlElement2 = element;
              num = current.PACBroker2015;
              string str3 = num.ToString();
              xmlElement2.SetAttribute("PACBroker2015", str3);
              XmlElement xmlElement3 = element;
              num = current.PACLender2015;
              string str4 = num.ToString();
              xmlElement3.SetAttribute("PACLender2015", str4);
              XmlElement xmlElement4 = element;
              num = current.PACOther2015;
              string str5 = num.ToString();
              xmlElement4.SetAttribute("PACOther2015", str5);
              element.SetAttribute("PaidBy", current.PaidBy);
              element.SetAttribute("PaidTo", current.PaidTo);
              element.SetAttribute("Payee", current.Payee);
              XmlElement xmlElement5 = element;
              num = current.POCAmount;
              string str6 = num.ToString();
              xmlElement5.SetAttribute("POCAmount", str6);
              XmlElement xmlElement6 = element;
              num = current.POCBorrower2015;
              string str7 = num.ToString();
              xmlElement6.SetAttribute("POCBorrower2015", str7);
              XmlElement xmlElement7 = element;
              num = current.POCBroker2015;
              string str8 = num.ToString();
              xmlElement7.SetAttribute("POCBroker2015", str8);
              XmlElement xmlElement8 = element;
              num = current.POCLender2015;
              string str9 = num.ToString();
              xmlElement8.SetAttribute("POCLender2015", str9);
              XmlElement xmlElement9 = element;
              num = current.POCOther2015;
              string str10 = num.ToString();
              xmlElement9.SetAttribute("POCOther2015", str10);
              element.SetAttribute("POCPaidBy", current.POCPaidBy);
              XmlElement xmlElement10 = element;
              num = current.POCSeller2015;
              string str11 = num.ToString();
              xmlElement10.SetAttribute("POCSeller2015", str11);
              XmlElement xmlElement11 = element;
              num = current.PTCAmount;
              string str12 = num.ToString();
              xmlElement11.SetAttribute("PTCAmount", str12);
              element.SetAttribute("PTCPaidBy", current.PTCPaidBy);
            }
            break;
          }
        case EDSDataType.BalancingWorksheet:
          if (balancingWorksheet == null)
            break;
          try
          {
            this.addBalancingWorksheetItems(targetXmlElement, str1 + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_DEBITS, balancingWorksheet, "Debits");
          }
          catch (Exception ex)
          {
          }
          try
          {
            this.addBalancingWorksheetItems(targetXmlElement, str1 + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_CREDITS, balancingWorksheet, "Credits");
          }
          catch (Exception ex)
          {
          }
          try
          {
            bool flag = false;
            List<string> list = balancingWorksheet.Keys.ToList<string>();
            for (int index = 0; index < list.Count; ++index)
            {
              if (string.Compare(list[index], "Debits", true) != 0 && string.Compare(list[index], "Credits", true) != 0 && string.Compare(list[index], "Printable Amounts", true) != 0)
              {
                if (!flag)
                {
                  targetXmlElement = (XmlNode) this.createPath(str1 + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_OTHERS);
                  flag = true;
                }
                XmlElement element = this.xmldoc.CreateElement("Item");
                targetXmlElement.AppendChild((XmlNode) element);
                element.SetAttribute("ID", list[index]);
                element.SetAttribute("Value", balancingWorksheet[list[index]].ToString());
              }
            }
          }
          catch (Exception ex)
          {
          }
          if (!balancingWorksheet.ContainsKey("Printable Amounts"))
            break;
          try
          {
            Dictionary<string, string> dictionary2 = (Dictionary<string, string>) balancingWorksheet["Printable Amounts"];
            if (dictionary2 == null || dictionary2.Count <= 0)
              break;
            XmlNode path2 = (XmlNode) this.createPath(str1 + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_PRINTABLEAMOUNTS);
            List<string> list = dictionary2.Keys.ToList<string>();
            for (int index = 0; index < list.Count; ++index)
            {
              XmlElement element = this.xmldoc.CreateElement("Item");
              path2.AppendChild((XmlNode) element);
              element.SetAttribute("LineNumber", list[index]);
              element.SetAttribute("PrintableAmount", dictionary2[list[index]].ToString());
            }
            break;
          }
          catch (Exception ex)
          {
            break;
          }
        case EDSDataType.ConditionLetter:
          if (dictionary1 == null)
            break;
          using (Dictionary<string, string>.Enumerator enumerator = dictionary1.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, string> current = enumerator.Current;
              XmlElement element = this.xmldoc.CreateElement("ConditionLetter");
              element.SetAttribute("LetterName", current.Key);
              this.xmldoc.LoadXml(current.Value);
              element.AppendChild((XmlNode) this.xmldoc.DocumentElement);
              targetXmlElement.AppendChild((XmlNode) element);
            }
            break;
          }
        case EDSDataType.Error:
          string str13 = (string) data;
          if (str13 != "")
          {
            string str14 = str13;
            char[] chArray = new char[1]{ '|' };
            foreach (string str15 in str14.Split(chArray))
            {
              XmlElement element = this.xmldoc.CreateElement("Error");
              targetXmlElement.AppendChild((XmlNode) element);
              element.SetAttribute("Message", str15);
            }
            break;
          }
          XmlElement element1 = this.xmldoc.CreateElement("Error");
          targetXmlElement.AppendChild((XmlNode) element1);
          element1.SetAttribute("Message", "None");
          break;
      }
    }

    private void addBalancingWorksheetItems(
      XmlNode targetXmlElement,
      string nodePath,
      Dictionary<string, object> balancingWorksheet,
      string id)
    {
      if (balancingWorksheet == null || !balancingWorksheet.ContainsKey(id))
        return;
      List<string[]> strArrayList = (List<string[]>) balancingWorksheet[id];
      if (strArrayList == null || strArrayList.Count == 0)
        return;
      targetXmlElement = (XmlNode) this.createPath(nodePath);
      XmlNode xmlNode = targetXmlElement;
      for (int index1 = 0; index1 < strArrayList.Count; ++index1)
      {
        string[] strArray = strArrayList[index1];
        XmlElement element = this.xmldoc.CreateElement("Item");
        xmlNode.AppendChild((XmlNode) element);
        element.SetAttribute("ColTotal", string.Concat((object) strArray.Length));
        for (int index2 = 0; index2 < strArray.Length; ++index2)
          element.SetAttribute("Col" + (object) (index2 + 1), strArray[index2]);
      }
    }

    internal object GetEDSData(EDSDataType dataType)
    {
      string nodeRequireddata = Mapping.EDS_NODE_REQUIREDDATA;
      switch (dataType)
      {
        case EDSDataType.PaymentSchedule_Amortization:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_AMORTIZATION;
          break;
        case EDSDataType.PaymentSchedule_WorstCase:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_WORSTCASE;
          break;
        case EDSDataType.UCD_LE:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_UCD_LE;
          break;
        case EDSDataType.UCD_CD:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_UCD_CD;
          break;
        case EDSDataType.FundingFee:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_FUNDINGFEE;
          break;
        case EDSDataType.BalancingWorksheet:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET;
          break;
        case EDSDataType.HELOC:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_HELOC;
          break;
        case EDSDataType.ConditionLetter:
          nodeRequireddata += Mapping.EDS_NODE_REQUIREDDATA_CONDITIONLETTER;
          break;
      }
      if (dataType == EDSDataType.PaymentSchedule_Amortization || dataType == EDSDataType.PaymentSchedule_WorstCase)
      {
        XmlNode node = this.root.SelectSingleNode(nodeRequireddata);
        if (node != null)
          return (object) new PaymentScheduleSnapshot(node, false, false);
      }
      else
      {
        switch (dataType)
        {
          case EDSDataType.FundingFee:
            XmlNodeList xmlNodeList1 = this.root.SelectNodes(nodeRequireddata + "/Fee");
            if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
            {
              List<FundingFee> edsData = new List<FundingFee>();
              foreach (XmlElement xmlElement in xmlNodeList1)
                edsData.Add(new FundingFee()
                {
                  LineID = xmlElement.GetAttribute("LineID"),
                  CDLineID = xmlElement.GetAttribute("CDLineID"),
                  Amount = Utils.ParseDouble((object) xmlElement.GetAttribute("Amount")),
                  BalanceChecked = xmlElement.GetAttribute("BalanceChecked") == "Y",
                  FeeDescription = xmlElement.GetAttribute("FeeDescription"),
                  PACBroker2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("PACBroker2015")),
                  PACLender2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("PACLender2015")),
                  PACOther2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("PACOther2015")),
                  PaidBy = xmlElement.GetAttribute("PaidBy"),
                  PaidTo = xmlElement.GetAttribute("PaidTo"),
                  Payee = xmlElement.GetAttribute("Payee"),
                  POCAmount = Utils.ParseDouble((object) xmlElement.GetAttribute("POCAmount")),
                  POCBorrower2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("POCBorrower2015")),
                  POCBroker2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("POCBroker2015")),
                  POCLender2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("POCLender2015")),
                  POCOther2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("POCOther2015")),
                  POCPaidBy = xmlElement.GetAttribute("POCPaidBy"),
                  POCSeller2015 = Utils.ParseDouble((object) xmlElement.GetAttribute("POCSeller2015")),
                  PTCAmount = Utils.ParseDouble((object) xmlElement.GetAttribute("PTCAmount")),
                  PTCPaidBy = xmlElement.GetAttribute("PTCPaidBy")
                });
              if (edsData.Count > 0)
                return (object) edsData;
              break;
            }
            break;
          case EDSDataType.BalancingWorksheet:
            Dictionary<string, object> edsData1 = new Dictionary<string, object>();
            XmlNodeList sourceNodeList1 = this.root.SelectNodes(nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_DEBITS + "/Item");
            if (sourceNodeList1 != null && sourceNodeList1.Count > 0)
            {
              List<string[]> balancingWorksheetItems = this.getBalancingWorksheetItems(sourceNodeList1);
              if (balancingWorksheetItems != null && balancingWorksheetItems.Count > 0)
                edsData1.Add("Debits", (object) balancingWorksheetItems);
            }
            XmlNodeList sourceNodeList2 = this.root.SelectNodes(nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_CREDITS + "/Item");
            if (sourceNodeList2 != null && sourceNodeList2.Count > 0)
            {
              List<string[]> balancingWorksheetItems = this.getBalancingWorksheetItems(sourceNodeList2);
              if (balancingWorksheetItems != null && balancingWorksheetItems.Count > 0)
                edsData1.Add("Credits", (object) balancingWorksheetItems);
            }
            XmlNodeList xmlNodeList2 = this.root.SelectNodes(nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_PRINTABLEAMOUNTS + "/Item");
            if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
            {
              Dictionary<string, string> dictionary = new Dictionary<string, string>();
              foreach (XmlElement xmlElement in xmlNodeList2)
              {
                string attribute1 = xmlElement.GetAttribute("LineNumber");
                string attribute2 = xmlElement.GetAttribute("PrintableAmount");
                if (!dictionary.ContainsKey(attribute1))
                  dictionary.Add(attribute1, attribute2);
              }
              if (dictionary != null && dictionary.Count > 0)
                edsData1.Add("Printable Amounts", (object) dictionary);
            }
            XmlNodeList xmlNodeList3 = this.root.SelectNodes(nodeRequireddata + Mapping.EDS_NODE_REQUIREDDATA_BALANCINGWORKSHEET_OTHERS + "/Item");
            if (xmlNodeList3 != null && xmlNodeList3.Count > 0)
            {
              foreach (XmlElement xmlElement in xmlNodeList3)
              {
                string attribute3 = xmlElement.GetAttribute("ID");
                string attribute4 = xmlElement.GetAttribute("Value");
                edsData1.Add(attribute3, (object) attribute4);
              }
            }
            if (edsData1 != null && edsData1.Count > 0)
              return (object) edsData1;
            break;
          default:
            if (dataType == EDSDataType.UCD_LE || dataType == EDSDataType.UCD_CD || dataType == EDSDataType.HELOC)
            {
              XmlNodeList xmlNodeList4 = this.root.SelectNodes(nodeRequireddata + "/Field");
              if (xmlNodeList4 != null && xmlNodeList4.Count > 0)
              {
                Dictionary<string, string> edsData2 = new Dictionary<string, string>();
                foreach (XmlElement xmlElement in xmlNodeList4)
                {
                  string attribute5 = xmlElement.GetAttribute("ID");
                  string attribute6 = xmlElement.GetAttribute("Value");
                  if (!(attribute5 == ""))
                  {
                    if (!edsData2.ContainsKey(attribute6))
                      edsData2.Add(attribute5, attribute6);
                    else
                      edsData2[attribute5] = attribute6;
                  }
                }
                if (edsData2.Count > 0)
                  return (object) edsData2;
                break;
              }
              break;
            }
            if (dataType == EDSDataType.ConditionLetter)
            {
              XmlNodeList xmlNodeList5 = this.root.SelectNodes(nodeRequireddata + "/ConditionLetter");
              if (xmlNodeList5 != null && xmlNodeList5.Count > 0)
              {
                Dictionary<string, ConditionalLetterPrintOption> edsData3 = new Dictionary<string, ConditionalLetterPrintOption>();
                foreach (XmlElement xmlElement in xmlNodeList5)
                {
                  string attribute = xmlElement.GetAttribute("LetterName");
                  string outerXml = xmlElement.FirstChild.OuterXml;
                  if (!(attribute == ""))
                  {
                    if (!edsData3.ContainsKey(outerXml))
                      edsData3.Add(attribute, (ConditionalLetterPrintOption) new BinaryObject(outerXml, Encoding.Default));
                    else
                      edsData3[attribute] = (ConditionalLetterPrintOption) new BinaryObject(outerXml, Encoding.Default);
                  }
                }
                if (edsData3.Count > 0)
                  return (object) edsData3;
                break;
              }
              break;
            }
            break;
        }
      }
      return (object) null;
    }

    private List<string[]> getBalancingWorksheetItems(XmlNodeList sourceNodeList)
    {
      List<string[]> balancingWorksheetItems = new List<string[]>();
      List<string> stringList1 = new List<string>();
      foreach (XmlElement sourceNode in sourceNodeList)
      {
        int num = Utils.ParseInt((object) sourceNode.GetAttribute("ColTotal"), 0);
        if (num != 0)
        {
          List<string> stringList2 = new List<string>();
          for (int index = 0; index < num; ++index)
            stringList2.Add(sourceNode.GetAttribute("Col" + (object) (index + 1)));
          balancingWorksheetItems.Add(stringList2.ToArray());
        }
      }
      return balancingWorksheetItems;
    }

    internal void ClearEDSData()
    {
      try
      {
        XmlNode oldChild = this.root.SelectSingleNode(Mapping.EDS_NODE_REQUIREDDATA);
        oldChild?.ParentNode.RemoveChild(oldChild);
      }
      catch (Exception ex)
      {
        Tracing.Log(Mapping.sw, TraceLevel.Error, nameof (Mapping), "Error in ClearEDSData, e: " + ex.Message);
      }
    }
  }
}
