// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LoanData
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.ElliEnum;
using Elli.Interface;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Serialization;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.InterimServicing;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Workflow;
using EllieMae.EMLite.Xml;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Schema;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public sealed class LoanData : IHtmlInput, ISerializable, ICloneable
  {
    private const string className = "LoanData";
    private bool refreshURLA2020 = true;
    private bool displaySwtichURLAPopup = true;
    private bool clearAIQIncomeAnalyzerAlert;
    private string aiqIncomeEpassMessageID;
    private static string sw = Tracing.SwDataEngine;
    private static readonly object nobj = (object) Missing.Value;
    private static readonly bool SkipMergeFieldsForFullAccess;
    private static XmlSchema schema = (XmlSchema) null;
    public EventHandler CountyLimitInvalidLoanAmount;
    public EventHandler RediscloseAPRRequired;
    public bool SkipCountyLimitCalculation;
    public EventHandler BorrowerPairCreated;
    private XmlDocument xmldoc;
    private XmlDocument edsLoanXml;
    public DateTime BaseLastModified = DateTime.MaxValue;
    private Mapping innerMap;
    private Hashtable dirtyTbl = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private object dataMgr;
    private ILoanCalculator calculator;
    private ILoanValidator validator;
    private ILoanTriggers triggers;
    private IPrintFormSelector printFormSelector;
    private IAlertMonitor alertMonitor;
    private ILoanHistoryMonitor loanHistoryMonitor;
    private ILoanSettings loanSettings;
    private ILoanAccessRules accessRules = (ILoanAccessRules) new FullRightsAccessRules();
    private LoanContentAccess contentAccess = LoanContentAccess.FullAccess;
    private ILoanSnapshotProvider snapshotProvider;
    private LoanData linkedData;
    private IStageLoanHistoryManager stageHistoryManager;
    private Hashtable dirtyTbl2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable lockTbl2 = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable userModifiedFieldsTbl3 = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private bool dirty;
    private bool mergeRequired;
    private BorrowerPair pair;
    private int currentBorPairIndex = 1;
    private BinaryObject remoteData;
    private bool isDraft;
    private bool isDraftLoanSubmitted;
    private LogRecordBase[] cachedDocumentLogsForReportingDatabase;
    private bool enableEnchancedConditions;
    public readonly Dictionary<string, string> foreignAddressIndictorLookupTable = new Dictionary<string, string>()
    {
      {
        "FR07",
        "29"
      },
      {
        "FR08",
        "29"
      },
      {
        "1418",
        "URLA.X267"
      },
      {
        "1419",
        "URLA.X267"
      },
      {
        "1521",
        "URLA.X268"
      },
      {
        "1522",
        "URLA.X268"
      },
      {
        "BR10",
        "39"
      },
      {
        "BR11",
        "39"
      },
      {
        "BR07",
        "29"
      },
      {
        "BR08",
        "29"
      },
      {
        "CR10",
        "39"
      },
      {
        "CR11",
        "39"
      },
      {
        "CR07",
        "29"
      },
      {
        "CR08",
        "29"
      },
      {
        "FE06",
        "80"
      },
      {
        "FE07",
        "80"
      },
      {
        "BE06",
        "80"
      },
      {
        "BE07",
        "80"
      },
      {
        "CE06",
        "80"
      },
      {
        "CE07",
        "80"
      },
      {
        "FM07",
        "58"
      },
      {
        "FM08",
        "58"
      },
      {
        "DD06",
        "39"
      },
      {
        "DD07",
        "39"
      },
      {
        "FL06",
        "67"
      },
      {
        "FL07",
        "67"
      },
      {
        "URLAROL09",
        "23"
      },
      {
        "URLAROL10",
        "23"
      },
      {
        "CAPIAP.X12",
        "CAPIAP.X62"
      },
      {
        "CAPIAP.X13",
        "CAPIAP.X62"
      },
      {
        "CAPIAP.X19",
        "CAPIAP.X63"
      },
      {
        "CAPIAP.X20",
        "CAPIAP.X63"
      },
      {
        "DENIAL.X84",
        "DENIAL.X97"
      },
      {
        "DENIAL.X85",
        "DENIAL.X97"
      },
      {
        "DENIAL.X89",
        "DENIAL.X99"
      },
      {
        "DENIAL.X90",
        "DENIAL.X99"
      },
      {
        "1292",
        "4678"
      },
      {
        "1305",
        "4678"
      },
      {
        "NBOC07",
        "22"
      },
      {
        "NBOC08",
        "22"
      },
      {
        "1249",
        "4680"
      },
      {
        "703",
        "4680"
      },
      {
        "VEND.X415",
        "VEND.X1047"
      },
      {
        "VEND.X416",
        "VEND.X1047"
      },
      {
        "Seller3.State",
        "Seller3.ForeignAddressIndicator"
      },
      {
        "Seller3.Zip",
        "Seller3.ForeignAddressIndicator"
      },
      {
        "Seller4.State",
        "Seller4.ForeignAddressIndicator"
      },
      {
        "Seller4.Zip",
        "Seller4.ForeignAddressIndicator"
      },
      {
        "4883",
        "4880"
      },
      {
        "4884",
        "4880"
      }
    };
    private BusinessRuleOnDemandEnum businessRuleTrigger;
    private ArrayList locked_COC_Fields = new ArrayList();
    private bool loanDisclosedClearGFE;
    private bool feeLevel_COC_LE_Warning = true;
    private bool feeLevel_COC_CD_Warning = true;
    private string fieldForGoTo = "";
    private Hashtable hiddenFields;
    private Hashtable editableFields;
    private Hashtable viewOnlyFields;
    private bool temporaryIgnoreRule;
    private bool dirtyFlagChangeEnabled = true;
    private string uuid;
    private bool isTemplate;
    private bool isAutoSaveFlag;
    private DateTime autoSaveDateTime;
    private string ulddExportType = "";
    private bool isInFindFieldForm;
    private bool isSingleFieldSelection;
    private bool buttonSelectionEnabled = true;
    private bool addLoanNumber = true;
    private static string[] attrs = new string[9]
    {
      "LOAN_APPLICATION/LOAN_PURPOSE/CONSTRUCTION_REFINANCE_DATA/@RefinanceImprovementsType",
      "LOAN_APPLICATION/EllieMae/GFE/MLDS/@PrepaymentPenaltyIndicator",
      "BORROWER/GOVERNMENT_MONITORING/@RaceNationalOriginType",
      "EllieMae/REGZ/@MidPointCancellation",
      "LIABILITY/@SubjectLoanResubordinationIndicator",
      "ADDITIONAL_CASE_DATA/TRANSMITTAL_DATA/@BelowMarketSubordinateFinancingIndicator",
      "LOAN_PURPOSE/CONSTRUCTION_REFINANCE_DATA/@GSERefinancePurposeType",
      "LOAN_PURPOSE/CONSTRUCTION_REFINANCE_DATA/@RefinanceImprovementsType",
      "EllieMae/FHA_VA_LOANS/@SpecialAssessments"
    };
    private static string[] elms = new string[4]
    {
      "EllieMae/HOUSING_EXPENSE",
      "LOAN_PRODUCT_DATA/LOAN_FEATURES",
      "REO_PROPERTY",
      "EllieMae/LOAN_PROGRAM"
    };
    private string validationErrors;
    private int errInd;
    private static Dictionary<string, string[]> eConsentBorrowerCoborrower_fieldIDs = new Dictionary<string, string[]>()
    {
      {
        "b1",
        new string[6]
        {
          "3984",
          "3985",
          "3986",
          "3987",
          "4989",
          "4956"
        }
      },
      {
        "c1",
        new string[6]
        {
          "3988",
          "3989",
          "3990",
          "3991",
          "4990",
          "4957"
        }
      },
      {
        "b2",
        new string[6]
        {
          "3992",
          "3993",
          "3994",
          "3995",
          "4991",
          "4958"
        }
      },
      {
        "c2",
        new string[6]
        {
          "3996",
          "3997",
          "3998",
          "3999",
          "4992",
          "4959"
        }
      },
      {
        "b3",
        new string[6]
        {
          "4023",
          "4024",
          "4025",
          "4026",
          "4993",
          "4960"
        }
      },
      {
        "c3",
        new string[6]
        {
          "4027",
          "4028",
          "4029",
          "4030",
          "4994",
          "4961"
        }
      },
      {
        "b4",
        new string[6]
        {
          "4031",
          "4032",
          "4033",
          "4034",
          "4995",
          "4962"
        }
      },
      {
        "c4",
        new string[6]
        {
          "4035",
          "4036",
          "4037",
          "4038",
          "4996",
          "4963"
        }
      },
      {
        "b5",
        new string[6]
        {
          "4039",
          "4040",
          "4041",
          "4042",
          "4997",
          "4964"
        }
      },
      {
        "c5",
        new string[6]
        {
          "4043",
          "4044",
          "4045",
          "4046",
          "4998",
          "4965"
        }
      },
      {
        "b6",
        new string[6]
        {
          "4047",
          "4048",
          "4049",
          "4050",
          "4999",
          "4966"
        }
      },
      {
        "c6",
        new string[6]
        {
          "4051",
          "4052",
          "4053",
          "4054",
          "5000",
          "4967"
        }
      }
    };
    private BorrowerPair[] pairs;
    private static string nil = string.Empty;
    private Hashtable findFieldTable = new Hashtable();
    private static string[] loanFlds = new string[36]
    {
      "136",
      "1335",
      "1771",
      "1109",
      "912",
      "137",
      "142",
      "140",
      "799",
      "PREQUAL.X7",
      "PREQUAL.X8",
      "356",
      "5",
      "740",
      "742",
      "353",
      "976",
      "1389",
      "1733",
      "PREQUAL.X270",
      "PREQUAL.X271",
      "PREQUAL.X272",
      "138",
      "1073",
      "PREQUAL.X273",
      "1844",
      "915",
      "PREQUAL.X275",
      "1107",
      "1045",
      "2",
      "462",
      "PREQUAL.X274",
      "2849",
      "1742",
      "PREQUAL.X202"
    };
    private static string[] alt1Flds = new string[36]
    {
      "PREQUAL.X33",
      "PREQUAL.X234",
      "PREQUAL.X233",
      "PREQUAL.X34",
      "PREQUAL.X235",
      "PREQUAL.X43",
      "PREQUAL.X44",
      "PREQUAL.X236",
      "PREQUAL.X48",
      "PREQUAL.X56",
      "PREQUAL.X57",
      "PREQUAL.X254",
      "PREQUAL.X256",
      "PREQUAL.X237",
      "PREQUAL.X238",
      "PREQUAL.X262",
      "PREQUAL.X264",
      "PREQUAL.X266",
      "PREQUAL.X268",
      "PREQUAL.X276",
      "PREQUAL.X278",
      "PREQUAL.X280",
      "PREQUAL.X282",
      "PREQUAL.X284",
      "PREQUAL.X286",
      "PREQUAL.X288",
      "PREQUAL.X290",
      "PREQUAL.X292",
      "PREQUAL.X295",
      "PREQUAL.X297",
      "PREQUAL.X299",
      "PREQUAL.X301",
      "PREQUAL.X303",
      "PREQUAL.X305",
      "PREQUAL.X258",
      "PREQUAL.X260"
    };
    private static string[] alt2Flds = new string[36]
    {
      "PREQUAL.X73",
      "PREQUAL.X240",
      "PREQUAL.X239",
      "PREQUAL.X74",
      "PREQUAL.X241",
      "PREQUAL.X83",
      "PREQUAL.X84",
      "PREQUAL.X242",
      "PREQUAL.X88",
      "PREQUAL.X96",
      "PREQUAL.X97",
      "PREQUAL.X255",
      "PREQUAL.X257",
      "PREQUAL.X243",
      "PREQUAL.X244",
      "PREQUAL.X263",
      "PREQUAL.X265",
      "PREQUAL.X267",
      "PREQUAL.X269",
      "PREQUAL.X277",
      "PREQUAL.X279",
      "PREQUAL.X281",
      "PREQUAL.X283",
      "PREQUAL.X285",
      "PREQUAL.X287",
      "PREQUAL.X289",
      "PREQUAL.X291",
      "PREQUAL.X293",
      "PREQUAL.X296",
      "PREQUAL.X298",
      "PREQUAL.X300",
      "PREQUAL.X302",
      "PREQUAL.X304",
      "PREQUAL.X306",
      "PREQUAL.X259",
      "PREQUAL.X261"
    };
    private static string[] addIds = new string[4]
    {
      "PM13",
      "PM17",
      "PM21",
      "PM25"
    };
    private static string[] perIds = new string[4]
    {
      "PM11",
      "PM15",
      "PM19",
      "PM23"
    };
    private static string[] typeIds = new string[4]
    {
      "PM12",
      "PM16",
      "PM20",
      "PM24"
    };
    private static string[] races = new string[7]
    {
      "American Indian or Alaska Native; ",
      "Asian; ",
      "Black or African American; ",
      "Native Hawaiian or Other Pacific Islander; ",
      "White; ",
      "Information not provided; ",
      "Not applicable; "
    };
    private Hashtable fieldModifiedByTemplate;
    private static HashSet<string> FieldsToBeExcluded = new HashSet<string>()
    {
      "136",
      "967",
      "968",
      "1092",
      "138",
      "137",
      "969",
      "1093",
      "1073",
      "3551",
      "140",
      "143",
      "202",
      "141",
      "1091",
      "1095",
      "1106",
      "1115",
      "1646",
      "1647",
      "1845",
      "1851",
      "1852",
      "1109",
      "1045",
      "2",
      "1844",
      "142",
      "19",
      "4084"
    };
    private string[] dbaNames;
    private bool vaLoanValidation = true;
    private bool includeSnapshotInXML;
    private string[] linkedStandardFields = new string[23]
    {
      "19",
      "1109",
      "4084",
      "1172",
      "420",
      "1888",
      "4487",
      "4488",
      "4489",
      "4490",
      "4493",
      "4494",
      "364",
      "2",
      "228",
      "229",
      "234",
      "QM.X337",
      "1264",
      "URLA.X210",
      "3",
      "URLA.X209",
      "5025"
    };
    private string[] linkedVirtualFields = new string[22]
    {
      "682",
      "78",
      "325",
      "608",
      "3",
      "2400",
      "697",
      "696",
      "695",
      "694",
      "247",
      "ARM.ApplyLfCpLow",
      "689",
      "688",
      "1827",
      "1699",
      "ARM.FlrBasis",
      "SYS.X1",
      "1700",
      "ARM.FlrVerbgTyp",
      "1959",
      "5"
    };
    private object ddmTrigger;
    private Dictionary<string, string> ddmOnDemandVirtualFields;

    public static event LoanDataEventHandler InstanceParsed;

    public event LoanDataEventHandler BorrowerPairChanged;

    public event LoanDataEventHandler LienPositionChanged;

    public event LoanDataEventHandler VestingChanged;

    public event LoanVerifOperationEventHandler VerificationsChanged;

    public event LoanDataModifiedEventHandler LoanDataModified;

    public event FieldChangedEventHandler FieldChanged;

    public event LogRecordEventHandler LogRecordAdded;

    public event LogRecordEventHandler LogRecordRemoved;

    public event LogRecordEventHandler LogRecordChanged;

    public event LogRecordEventHandler RateLockRequested;

    public event LogRecordEventHandler RateLockConfirmed;

    public event LogRecordEventHandler RateLockDenied;

    public event LogRecordEventHandler LockVoided;

    public event CancelableMilestoneEventHandler BeforeMilestoneCompleted;

    public event MilestoneEventHandler MilestoneCompleted;

    public event LockRequestFieldChangedEventHandler LockRequestFieldChanged;

    public event EventHandler BeforeTriggerRuleApplied;

    public event EventHandler TriggerRuleLoanTemplateChecked;

    public event LoanDataBeforeChangedEventHandler BeforeFieldChanged;

    public event EventHandler Closed;

    public event EventHandler FormVersionChanged;

    public event EventHandler Disclosure2015Created;

    public bool RefreshURLA2020Fields
    {
      get => this.refreshURLA2020;
      set => this.refreshURLA2020 = value;
    }

    public bool BatchAppliedSinceLastSave { get; set; }

    public bool DisplaySwitchURLAPopup
    {
      get => this.displaySwtichURLAPopup;
      set => this.displaySwtichURLAPopup = value;
    }

    public EventHandler Disclosure2015CreatedEventHandler { get; }

    public string XmlDocString
    {
      get
      {
        using (MemoryStream stream = StreamHelper.NewMemoryStream())
        {
          using (XmlWriter writer = XmlHelper.CreateWriter((Stream) stream))
            this.xmldoc.WriteTo(writer);
          return stream.ToString(Encoding.UTF8, true);
        }
      }
    }

    public XmlDocument XmlDocClone => (XmlDocument) this.xmldoc?.Clone();

    public XmlDocument EDSLoanXmlDocument => this.edsLoanXml;

    public string EDSLoanXmlString => this.edsLoanXml.OuterXml;

    public FieldChangeTracker FieldChangeTracker { get; set; } = new FieldChangeTracker();

    public bool EnableEnhancedConditions
    {
      get
      {
        return this.enableEnchancedConditions || "Y".Equals(this.GetField("ENHANCEDCOND.X1"), StringComparison.OrdinalIgnoreCase);
      }
    }

    public bool IsDraft
    {
      get => this.isDraft;
      set => this.isDraft = value;
    }

    public bool IsDraftLoanSubmitted
    {
      get => this.isDraftLoanSubmitted;
      set => this.isDraftLoanSubmitted = value;
    }

    public bool IgnoreFieldChangeTrackingForXDB
    {
      get => this.FieldChangeTracker.IgnoreFieldChangeTrackingForXDB;
    }

    public Dictionary<string, FieldChangeInfo> FieldChanges => this.FieldChangeTracker.FieldChanges;

    public BusinessRuleOnDemandEnum BusinessRuleTrigger
    {
      get => this.businessRuleTrigger;
      set => this.businessRuleTrigger = value;
    }

    public bool DefrdRetryAllowed { get; set; }

    public bool DefrdReSubmitAllowed { get; set; }

    public bool DefrdBorrowerActionRequired { get; set; }

    public bool DefrdLenderActionRequired { get; set; }

    public bool DefrdEncompassLevelActionRequired { get; set; }

    public bool DefrdLoanAppSubmitEventRequired { get; set; }

    public bool IsLinknSyncSecondaryLoan => this.LinkSyncType == LinkSyncType.ConstructionLinked;

    public ArrayList Locked_COC_Fields => this.locked_COC_Fields;

    public bool LoanDisclosedClearGFE
    {
      get => this.loanDisclosedClearGFE;
      set => this.loanDisclosedClearGFE = value;
    }

    public bool FeeLevel_COC_LE_Warning
    {
      get => this.feeLevel_COC_LE_Warning;
      set => this.feeLevel_COC_LE_Warning = value;
    }

    public bool FeeLevel_COC_CD_Warning
    {
      get => this.feeLevel_COC_CD_Warning;
      set => this.feeLevel_COC_CD_Warning = value;
    }

    public bool ClearAIQIncomeAnalyzerAlert
    {
      get => this.clearAIQIncomeAnalyzerAlert;
      set => this.clearAIQIncomeAnalyzerAlert = value;
    }

    public string AiqIncomeEpassMessageID
    {
      get => this.aiqIncomeEpassMessageID;
      set => this.aiqIncomeEpassMessageID = value;
    }

    static LoanData()
    {
      LoanData.SkipMergeFieldsForFullAccess = Utils.ParseBoolean((object) ConfigurationManager.AppSettings.Get("LoanData.SkipMergeFieldsForFullAccess"));
    }

    public LoanData(
      string xmlData,
      ILoanSettings loanSettings,
      bool generateNewGuid,
      bool enableEnchancedConditions = false)
    {
      if (string.IsNullOrEmpty(xmlData))
        throw new ArgumentException(nameof (xmlData), "XML value cannot be null or empty");
      this.loanSettings = loanSettings != null ? loanSettings : throw new ArgumentNullException(nameof (loanSettings));
      this.enableEnchancedConditions = enableEnchancedConditions;
      this.parseXml(xmlData, generateNewGuid);
      this.dirty = false;
    }

    public LoanData(string xmlData, ILoanSettings loanSettings)
      : this(xmlData, loanSettings, false)
    {
    }

    public LoanData(LoanData source, ILoanSettings loanSettings)
      : this(source, loanSettings, true)
    {
    }

    public LoanData(
      LoanData source,
      ILoanSettings loanSettings,
      bool generateNewGuid,
      bool replaceCachedXML = true,
      bool enableEnchancedConditions = false)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      this.loanSettings = loanSettings != null ? loanSettings : throw new ArgumentNullException(nameof (loanSettings));
      this.enableEnchancedConditions = enableEnchancedConditions;
      using (Stream stream = source.ToStream(source.ContentAccess, true, replaceCachedXML))
        this.parseXml(stream, generateNewGuid);
      if (source.FieldChangeTracker != null)
        this.FieldChangeTracker = source.FieldChangeTracker.Clone();
      this.dirty = false;
    }

    public LoanData(BinaryObject data, ILoanSettings loanSettings)
    {
      this.loanSettings = loanSettings;
      this.remoteData = data;
      if (loanSettings.MigrationData == null)
        return;
      this.parseXml(data.AsStream(), false);
    }

    public LoanData(XmlDocument xmlDoc, ILoanSettings loanSettings)
      : this(xmlDoc, loanSettings, false)
    {
    }

    public LoanData(XmlDocument xmlDoc, ILoanSettings loanSettings, bool generateNewGuid)
      : this(xmlDoc, loanSettings, generateNewGuid, false)
    {
    }

    public LoanData(
      XmlDocument xmlDoc,
      ILoanSettings loanSettings,
      bool generateNewGuid,
      bool cloneForCalc)
    {
      this.loanSettings = loanSettings;
      this.parseXml(xmlDoc, generateNewGuid, cloneForCalc);
      this.dirty = false;
    }

    private LoanData(SerializationInfo info, StreamingContext context)
    {
      this.remoteData = (BinaryObject) info.GetValue("xml", typeof (BinaryObject));
      this.loanSettings = (ILoanSettings) info.GetValue(nameof (loanSettings), typeof (ILoanSettings));
      this.BaseLastModified = (DateTime) info.GetValue(nameof (BaseLastModified), typeof (DateTime));
      this.autoSaveDateTime = (DateTime) info.GetValue(nameof (autoSaveDateTime), typeof (DateTime));
      this.isAutoSaveFlag = (bool) info.GetValue(nameof (isAutoSaveFlag), typeof (bool));
      string str = (string) info.GetValue("FieldChangeTrackerStr", typeof (string));
      if (string.IsNullOrEmpty(str))
        return;
      this.FieldChangeTracker = JsonConvert.DeserializeObject<FieldChangeTracker>(str);
    }

    private void updateDirtyTbl2(string fieldID, string val, BorrowerPair borrowerPair)
    {
      this.dirtyTbl2[(object) fieldID] = (object) new object[2]
      {
        (object) val,
        (object) borrowerPair
      };
    }

    private void updateUserModifiedFieldsDirty(
      string fieldID,
      string val,
      BorrowerPair borrowerPair)
    {
      this.userModifiedFieldsTbl3[(object) fieldID] = (object) new object[2]
      {
        (object) val,
        (object) borrowerPair
      };
    }

    public void ReplaceXml(Stream xmldata)
    {
      Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Verbose, "LoanData.ReplaceXml invoked");
      this.parseXml(xmldata, false);
      this.calculator = (ILoanCalculator) null;
      if (this.triggers != null)
        this.triggers.ResubscribeToFieldEvents();
      this.validator = (ILoanValidator) null;
      this.printFormSelector = (IPrintFormSelector) null;
      this.alertMonitor = (IAlertMonitor) null;
    }

    public void Close()
    {
      this.innerMap.CLose();
      if (this.Closed == null)
        return;
      this.Closed((object) null, (EventArgs) null);
    }

    private void parseXml(string xmldata, bool generateNewGuid)
    {
      using (Stream utf8StreamWithBom = xmldata.ToUtf8StreamWithBOM())
        this.parseXml(utf8StreamWithBom, generateNewGuid);
    }

    private void parseXml(Stream stream, bool generateNewGuid)
    {
      XmlDocument xmldocument = XmlHelper.NewXmlDocument();
      using (TextReader reader = XmlHelper.CreateReader(stream))
        xmldocument.Load(reader);
      this.parseXml(xmldocument, generateNewGuid, false);
    }

    private void parseXml(XmlDocument xmldocument, bool generateNewGuid, bool cloneForCalc)
    {
      if (xmldocument.SelectSingleNode("//EMLITE") != null)
      {
        string xml = xmldocument.OuterXml.Replace("<EMLITE", "<EllieMae").Replace("/EMLITE>", "/EllieMae>");
        this.xmldoc = XmlHelper.NewXmlDocument();
        this.xmldoc.LoadXml(xml);
      }
      else
        this.xmldoc = xmldocument;
      this.innerMap = new Mapping(this.xmldoc, this.loanSettings, cloneForCalc || LoanData.SkipMergeFieldsForFullAccess);
      BorrowerPair currentBorrowerPair = this.CurrentBorrowerPair;
      BorrowerPair[] borrowerPairs = this.innerMap.GetBorrowerPairs();
      this.pair = (BorrowerPair) null;
      this.SetBorrowerPair(borrowerPairs[0]);
      if (this.GUID == "" | generateNewGuid)
        this.GUID = LoanData.NewGuid();
      this.innerMap.MappingFieldChanged += new MappingFieldChangedEventHandler(this.innerMap_FieldChanged);
      LoanData.onInstanceParsed(this);
      if (currentBorrowerPair == null || this.CurrentBorrowerPair != null && !(currentBorrowerPair.Id != this.CurrentBorrowerPair.Id))
        return;
      this.SetBorrowerPair(currentBorrowerPair);
    }

    public void CurrentAPRChanged(object sender, EventArgs e)
    {
      if (this.RediscloseAPRRequired == null)
        return;
      this.RediscloseAPRRequired(sender, e);
    }

    public void ExceededLoanAmountEvent(object sender, EventArgs e)
    {
      if (this.CountyLimitInvalidLoanAmount == null)
        return;
      this.CountyLimitInvalidLoanAmount((object) null, (EventArgs) null);
    }

    public static string NewGuid() => "{" + (object) Guid.NewGuid() + "}";

    public DateTimeType MilestoneDateTimeType => this.loanSettings.MilestoneDateTimeType;

    public DateTimeType DocumentDateTimeType => this.loanSettings.DocumentDateTimeType;

    public string SystemID => this.loanSettings.SystemID;

    private Mapping map
    {
      get
      {
        this.Parse();
        return this.innerMap;
      }
    }

    public Mapping GetMap() => this.map;

    public string FieldForGoTo
    {
      get => this.fieldForGoTo;
      set => this.fieldForGoTo = value;
    }

    public CustomFieldsInfo CustomFields => this.loanSettings.FieldSettings.CustomFields;

    public void AttachExistingSettings(LoanData existingLoan)
    {
      this.contentAccess = existingLoan.ContentAccess;
      this.triggers = existingLoan.Triggers;
      this.printFormSelector = existingLoan.PrintFormSelector;
      this.accessRules = existingLoan.AccessRules;
      this.hiddenFields = existingLoan.HiddenFields;
      this.editableFields = existingLoan.EditableFields;
      this.viewOnlyFields = existingLoan.ViewOnlyFields;
      this.validator = existingLoan.Validator;
      this.snapshotProvider = existingLoan.snapshotProvider;
    }

    public void ReplaceCachedXML() => this.map.ReplaceCachedXML();

    public void AttachDataMgr(object dataMgr) => this.dataMgr = dataMgr;

    public void AttachCalculator(ILoanCalculator calculator) => this.calculator = calculator;

    public void AttachValidator(ILoanValidator validator) => this.validator = validator;

    public void AttachSnapshotProvider(ILoanSnapshotProvider provider)
    {
      this.snapshotProvider = provider;
    }

    [CLSCompliant(false)]
    public ILoanSnapshotProvider SnapshotProvider => this.snapshotProvider;

    public void AttachTriggers(ILoanTriggers triggers) => this.triggers = triggers;

    public void AttachAlertMonitor(IAlertMonitor monitor) => this.alertMonitor = monitor;

    public LoanData AttachStageLoanHistoryManager(IStageLoanHistoryManager historymanager)
    {
      this.stageHistoryManager = historymanager;
      return this;
    }

    public void AttachLoanHistoryMonitor(ILoanHistoryMonitor monitor)
    {
      this.loanHistoryMonitor = monitor;
    }

    public void AttachPrintFormSelector(IPrintFormSelector printFormSelector)
    {
      this.printFormSelector = printFormSelector;
    }

    public ILoanSettings Settings
    {
      get => this.loanSettings;
      set => this.loanSettings = value;
    }

    public LRAdditionalFields SecondaryAdditionalFields
    {
      get => this.loanSettings.FieldSettings.LockRequestAdditionalFields;
    }

    public void ApplyAccessRules(ILoanAccessRules accessRules)
    {
      this.accessRules = accessRules != null ? accessRules : throw new ArgumentNullException(nameof (accessRules));
    }

    public Hashtable HiddenFields => this.hiddenFields;

    public void AttachHiddenFields(Hashtable hiddenFields) => this.hiddenFields = hiddenFields;

    public void DetachHiddenFields()
    {
      if (this.hiddenFields == null)
        return;
      this.hiddenFields.Clear();
      this.hiddenFields = (Hashtable) null;
    }

    public Hashtable EditableFields => this.editableFields;

    public void AttachEditableFields(Hashtable editableFields)
    {
      this.editableFields = editableFields;
    }

    public void AttachLinkedEditableFields(Hashtable editableFields)
    {
      if (this.editableFields == null)
        return;
      string empty = string.Empty;
      foreach (DictionaryEntry editableField in editableFields)
      {
        string key = (string) editableField.Key;
        if (!this.editableFields.ContainsKey((object) key))
          this.editableFields.Remove((object) key);
      }
    }

    public bool IsFieldEditable(string id)
    {
      return !this.IsFieldReadOnly(id) && !this.IsFieldAccessRestricted(id);
    }

    public bool IsFieldAccessRestricted(string id)
    {
      return this.contentAccess != LoanContentAccess.FullAccess && ((this.contentAccess & LoanContentAccess.FormFields) == LoanContentAccess.None || this.editableFields == null || !this.editableFields.ContainsKey((object) id));
    }

    public bool IsRescindable()
    {
      bool flag = false;
      string field1 = this.GetField("3942");
      string field2 = this.GetField("CONST.X2");
      string field3 = this.GetField("19");
      if (field3 == LoanPurposeEnumUtil.ValueToNameInLoan(LoanPurpose.Construction) || field3 == LoanPurposeEnumUtil.ValueToNameInLoan(LoanPurpose.ConstructionPerm) || this.IsLinknSyncSecondaryLoan)
      {
        if (string.Compare(field1, "Y", true) != 0 && field2 == "Y")
          flag = true;
      }
      else
      {
        string field4 = this.GetField("1811");
        string field5 = this.GetField("16");
        int num = 0;
        if (!string.IsNullOrEmpty(field5))
          num = Convert.ToInt32(field5);
        if (string.Compare(field1, "Y", true) != 0 && string.Compare(field4, "PrimaryResidence", true) == 0 && num < 5 && (field3 == LoanPurposeEnumUtil.ValueToNameInLoan(LoanPurpose.CashOutRefi) || field3 == LoanPurposeEnumUtil.ValueToNameInLoan(LoanPurpose.NoCashOutRefi)))
          flag = true;
      }
      return flag;
    }

    public Hashtable ViewOnlyFields => this.viewOnlyFields;

    public void AttachReadOnlyFields(Hashtable viewOnlyFields)
    {
      this.viewOnlyFields = viewOnlyFields;
    }

    public bool TemporaryIgnoreRule
    {
      get => this.temporaryIgnoreRule;
      set => this.temporaryIgnoreRule = value;
    }

    public bool IsFieldReadOnly(string id)
    {
      return this.viewOnlyFields != null && this.viewOnlyFields.ContainsKey((object) id);
    }

    public ILoanCalculator Calculator => this.calculator;

    public ILoanValidator Validator => this.validator;

    public object DataMgr => this.dataMgr;

    public ILoanAccessRules AccessRules => this.accessRules;

    public ILoanTriggers Triggers => this.triggers;

    public IAlertMonitor AlertMonitor => this.alertMonitor;

    public IStageLoanHistoryManager StageHistoryManager => this.stageHistoryManager;

    public ILoanHistoryMonitor LoanHistoryMonitor => this.loanHistoryMonitor;

    public IPrintFormSelector PrintFormSelector => this.printFormSelector;

    public bool IgnoreValidationErrors
    {
      get => this.map.IgnoreValidationErrors;
      set => this.map.IgnoreValidationErrors = value;
    }

    public LoanContentAccess ContentAccess
    {
      get => this.contentAccess;
      set
      {
        this.contentAccess = value;
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Verbose, "Loan's ContentAccess level set to: " + (object) value);
      }
    }

    public string CurrentLoanVersion
    {
      get => this.map.EMXMLVersionID;
      set => this.map.EMXMLVersionID = value;
    }

    public string CurrentLoanVersion_LDM
    {
      get => this.map.EMXMLVersionID_LDM;
      set => this.map.EMXMLVersionID_LDM = value;
    }

    public int LoanVersionNumber
    {
      get => this.map.LoanVersionNumber;
      set => this.map.LoanVersionNumber = value;
    }

    public bool TPOConnectStatus
    {
      get => this.map.TPOConnectStatus;
      set => this.map.TPOConnectStatus = value;
    }

    public string OriginalLoanVersion => this.map.OriginalLoanVersion;

    public void Parse()
    {
      if (this.innerMap != null)
        return;
      this.parseXml(this.remoteData.AsStream(), false);
      this.remoteData.Dispose();
      this.remoteData = (BinaryObject) null;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      if (this.xmldoc == null)
        info.AddValue("xml", (object) this.remoteData);
      else
        info.AddValue("xml", (object) BinaryObject.Marshal(new BinaryObject(this.ToStream(true), false)));
      info.AddValue("loanSettings", (object) this.loanSettings);
      info.AddValue("FieldChangeTrackerStr", (object) JsonConvert.SerializeObject((object) this.FieldChangeTracker, Newtonsoft.Json.Formatting.None));
      info.AddValue("BaseLastModified", this.BaseLastModified);
      info.AddValue("autoSaveDateTime", this.autoSaveDateTime);
      info.AddValue("isAutoSaveFlag", this.isAutoSaveFlag);
    }

    public object Clone() => (object) new LoanData(this, this.loanSettings);

    public object CloneEDS() => (object) new LoanData(this.edsLoanXml, this.loanSettings, true);

    public LoanData Clone(bool generateLoanGuid)
    {
      this.IncludeSnapshotInXML = true;
      this.GetCalculationDTSnapshotForLoan();
      string xml = this.ToXml(true);
      this.IncludeSnapshotInXML = false;
      return new LoanData(xml, this.loanSettings, generateLoanGuid)
      {
        BaseLastModified = this.BaseLastModified
      };
    }

    public bool Dirty
    {
      get
      {
        if (!this.dirty)
          this.Dirty = this.GetLogList().IsDirty();
        return this.dirty;
      }
      set
      {
        if (!this.dirtyFlagChangeEnabled)
          return;
        if (!value)
        {
          this.GetLogList().MarkAsClean();
          this.dirtyTbl2.Clear();
        }
        this.dirty = value;
        if (!this.dirty || this.LoanDataModified == null)
          return;
        this.LoanDataModified((object) this, (EventArgs) null);
      }
    }

    public void DisableDirtyFlagChange() => this.dirtyFlagChangeEnabled = false;

    public void EnableDirtyFlagChange() => this.dirtyFlagChangeEnabled = true;

    public string GUID
    {
      get
      {
        if (string.IsNullOrEmpty(this.uuid))
        {
          this.uuid = this.map.GetFieldAt(nameof (GUID)).Trim();
          if (this.uuid == "")
            return "";
          if (this.uuid[0] != '{')
          {
            this.uuid = "{" + this.uuid + "}";
            this.map.SetFieldAt(nameof (GUID), this.uuid);
          }
        }
        return this.uuid;
      }
      set
      {
        this.map.SetFieldAt(nameof (GUID), value);
        this.uuid = (string) null;
      }
    }

    public string LinkGUID => this.map.GetFieldAt("LINKGUID");

    public LoanData LinkedData
    {
      get => this.linkedData;
      set
      {
        this.linkedData = value;
        if (value == null)
          this.map.SetFieldAt("LINKGUID", "");
        else if (value.GUID != this.LinkGUID)
          this.map.SetFieldAt("LINKGUID", value.GUID);
        if (this.linkedData == null || this.linkedData.pairs != null)
          return;
        this.linkedData.pairs = this.map.GetBorrowerPairs();
      }
    }

    public bool IsTemplate
    {
      get => this.isTemplate;
      set => this.isTemplate = value;
    }

    public bool IsAutoSaveFlag
    {
      get => this.isAutoSaveFlag;
      set => this.isAutoSaveFlag = value;
    }

    public DateTime AutoSaveDateTime
    {
      get => this.autoSaveDateTime;
      set => this.autoSaveDateTime = value;
    }

    public bool IsULDDExporting => this.ULDDExportType != "";

    public string ULDDExportType
    {
      get => this.ulddExportType;
      set => this.ulddExportType = value;
    }

    public bool IsInFindFieldForm
    {
      get => this.isInFindFieldForm;
      set => this.isInFindFieldForm = value;
    }

    public bool IsSingleFieldSelection
    {
      get => this.isSingleFieldSelection;
      set => this.isSingleFieldSelection = value;
    }

    public bool ButtonSelectionEnabled
    {
      get => this.buttonSelectionEnabled;
      set => this.buttonSelectionEnabled = value;
    }

    public bool IsCreatedWithoutLoanNumber() => this.map.IsCreatedWithoutLoanNumber();

    public void SetCreatedWithoutLoanNumber() => this.map.SetCreatedWithoutLoanNumber();

    public void UnsetCreatedWithoutLoanNumber() => this.map.UnsetCreatedWithoutLoanNumber();

    public void ActivateAlert(string alertId, string userId, DateTime timestamp)
    {
      this.map.ActivateAlert(alertId, userId, timestamp);
      this.Dirty = true;
    }

    public void DeactivateAlert(string alertGuid)
    {
      this.map.DeactivateAlert(alertGuid);
      this.Dirty = true;
    }

    public void DismissAlert(string alertGuid, string userId, DateTime timestamp)
    {
      this.map.DismissAlert(alertGuid, userId, timestamp);
      this.Dirty = true;
    }

    public AlertStatus GetAlertStatus(string alertGuid) => this.map.GetAlertStatus(alertGuid);

    public bool IsDocFieldOverridden(string fieldId)
    {
      return !this.IsFieldEmpty(fieldId) || this.map.GetDocFieldOverrideFlag(fieldId);
    }

    public bool GetDocFieldUserOverride(string fieldId)
    {
      return this.map.GetDocFieldOverrideFlag(fieldId);
    }

    public void SetDocFieldUserOverride(string fieldId, bool ovride)
    {
      if (this.map.GetDocFieldOverrideFlag(fieldId) == ovride)
        return;
      this.map.SetDocFieldOverrideFlag(fieldId, ovride);
      this.dirty = true;
    }

    public Hashtable PrepareLockRequestData()
    {
      Hashtable hashtable = new Hashtable();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < LockRequestLog.SnapshotFields.Count; ++index)
      {
        string snapshotField = LockRequestLog.SnapshotFields[index];
        string str;
        switch (snapshotField)
        {
          case "2592":
            continue;
          case "2144":
            str = string.IsNullOrEmpty(this.GetField("2144")) ? this.GetField("4456") : this.GetField("2144") + Environment.NewLine + Environment.NewLine + this.GetField("4456");
            break;
          default:
            str = this.GetField(snapshotField);
            break;
        }
        if (!(str == string.Empty))
        {
          if (hashtable.ContainsKey((object) snapshotField))
            hashtable[(object) snapshotField] = (object) str;
          else
            hashtable.Add((object) snapshotField, (object) str);
        }
      }
      for (int index = 0; index < LockRequestLog.RequestFields.Count; ++index)
      {
        string requestField = LockRequestLog.RequestFields[index];
        string field = this.GetField(requestField);
        if (!(field == string.Empty))
        {
          if (hashtable.ContainsKey((object) requestField))
            hashtable[(object) requestField] = (object) field;
          else
            hashtable.Add((object) requestField, (object) field);
        }
      }
      LRAdditionalFields additionalFields = this.Settings.FieldSettings.LockRequestAdditionalFields;
      if (additionalFields != null)
      {
        string[] fields1 = additionalFields.GetFields(false);
        if (fields1 != null && fields1.Length != 0)
        {
          for (int index = 0; index < fields1.Length; ++index)
          {
            string field = this.GetField(fields1[index]);
            if (!(field == string.Empty))
            {
              if (hashtable.ContainsKey((object) fields1[index]))
                hashtable[(object) fields1[index]] = (object) field;
              else
                hashtable.Add((object) fields1[index], (object) field);
            }
          }
        }
        string[] fields2 = additionalFields.GetFields(true);
        if (fields2 != null && fields2.Length != 0)
        {
          for (int index = 0; index < fields2.Length; ++index)
          {
            string customFieldId = LockRequestCustomField.GenerateCustomFieldID(fields2[index]);
            string field = this.GetField(customFieldId);
            if (!(field == string.Empty))
            {
              if (hashtable.ContainsKey((object) customFieldId))
                hashtable[(object) customFieldId] = (object) field;
              else
                hashtable.Add((object) customFieldId, (object) field);
            }
          }
        }
        AlertConfig alertConfig = this.Settings.AlertSetupData.GetAlertConfig(43);
        RegulationAlert definition = (RegulationAlert) alertConfig.Definition;
        if (alertConfig.TriggerFieldList != null)
        {
          foreach (string triggerField in alertConfig.TriggerFieldList)
          {
            if (!hashtable.ContainsKey((object) triggerField))
            {
              string field = this.GetField(triggerField);
              if (!(field == string.Empty))
                hashtable.Add((object) triggerField, (object) field);
            }
          }
        }
        hashtable.Add((object) "OPTIMAL.HISTORY", (object) this.GetField("OPTIMAL.HISTORY"));
      }
      for (int index = 2088; index <= 2144; ++index)
        this.SetField(index.ToString(), "");
      for (int index = 2414; index <= 2447; ++index)
        this.SetField(index.ToString(), "");
      for (int index = 2647; index <= 2687; ++index)
        this.SetField(index.ToString(), "");
      this.SetField("2848", "");
      this.SetField("3841", "NewLock");
      this.SetField("3847", "");
      this.SetField("3872", "");
      this.SetField("3874", "");
      this.SetField("4456", "");
      for (int index = 3454; index <= 3473; ++index)
        this.SetField(index.ToString(), "");
      for (int index = 4256; index < 4276; ++index)
        this.SetField(index.ToString(), "");
      for (int index = 4336; index < 4356; ++index)
        this.SetField(index.ToString(), "");
      this.SetField("3254", "");
      if (this.GetField("3965") != string.Empty)
      {
        hashtable[(object) "3911"] = (object) this.GetField("3965");
        this.SetField("3965", "");
      }
      if (this.GetField("4187") != string.Empty)
      {
        hashtable[(object) "3910"] = (object) this.GetField("4187");
        this.SetField("4187", "");
      }
      return hashtable;
    }

    public Hashtable PrepareLockCancellationData(Hashtable confirmedLockData)
    {
      Hashtable hashtable = new Hashtable();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      for (int index = 0; index < LockRequestLog.SnapshotFields.Count; ++index)
      {
        string snapshotField = LockRequestLog.SnapshotFields[index];
        if (!(snapshotField == "2592"))
        {
          string field = this.GetField(snapshotField);
          if (!(field == string.Empty))
          {
            if (hashtable.ContainsKey((object) snapshotField))
              hashtable[(object) snapshotField] = (object) field;
            else
              hashtable.Add((object) snapshotField, (object) field);
          }
        }
      }
      List<string> stringList = new List<string>();
      foreach (string requestField in LockRequestLog.RequestFields)
        stringList.Add(requestField);
      for (int index = 2088; index <= 2144; ++index)
        stringList.Add(index.ToString());
      for (int index = 2414; index <= 2447; ++index)
        stringList.Add(index.ToString());
      for (int index = 2647; index <= 2689; ++index)
        stringList.Add(index.ToString());
      for (int index = 3454; index <= 3473; ++index)
        stringList.Add(index.ToString());
      for (int index = 4256; index <= 4275; ++index)
        stringList.Add(index.ToString());
      for (int index = 4336; index <= 4355; ++index)
        stringList.Add(index.ToString());
      stringList.Add("2853");
      foreach (string key in stringList)
      {
        if (hashtable.ContainsKey((object) key))
          hashtable[(object) key] = (object) string.Empty;
      }
      string[] strArray = new string[8]
      {
        "3358",
        "3363",
        "3364",
        "3365",
        "3359",
        "3366",
        "3367",
        "3368"
      };
      foreach (string key in strArray)
      {
        if (confirmedLockData.ContainsKey((object) key))
        {
          if (hashtable.ContainsKey((object) key))
            hashtable[(object) key] = confirmedLockData[(object) key];
          else
            hashtable.Add((object) key, confirmedLockData[(object) key]);
        }
      }
      LRAdditionalFields additionalFields = this.Settings.FieldSettings.LockRequestAdditionalFields;
      if (additionalFields != null)
      {
        string[] fields1 = additionalFields.GetFields(false);
        if (fields1 != null && fields1.Length != 0)
        {
          for (int index = 0; index < fields1.Length; ++index)
          {
            string field = this.GetField(fields1[index]);
            if (!(field == string.Empty))
            {
              if (hashtable.ContainsKey((object) fields1[index]))
                hashtable[(object) fields1[index]] = (object) field;
              else
                hashtable.Add((object) fields1[index], (object) field);
            }
          }
        }
        string[] fields2 = additionalFields.GetFields(true);
        if (fields2 != null && fields2.Length != 0)
        {
          for (int index = 0; index < fields2.Length; ++index)
          {
            string customFieldId = LockRequestCustomField.GenerateCustomFieldID(fields2[index]);
            string field = this.GetField(customFieldId);
            if (!(field == string.Empty))
            {
              if (hashtable.ContainsKey((object) customFieldId))
                hashtable[(object) customFieldId] = (object) field;
              else
                hashtable.Add((object) customFieldId, (object) field);
            }
          }
        }
      }
      this.SetField("2848", "");
      return hashtable;
    }

    internal bool AddLockRequestSnapshot(string loanLockGUID, Hashtable fieldValues)
    {
      bool flag = this.map.AddLockRequestSnapshot(loanLockGUID, fieldValues);
      if (this.calculator != null)
        this.calculator.CalculateInvestorStatus();
      return flag;
    }

    internal bool ClearLockRequestSnapshot(string loanLockGUID)
    {
      return this.map.ClearLockRequestSnapshot(loanLockGUID);
    }

    internal Hashtable GetLockRequestSnapshot(string loanLockGUID)
    {
      return this.map.GetLockRequestSnapshot(loanLockGUID);
    }

    public bool UpdateConfirmLockComments(string loanLockGUID, string newComments, bool onBuySide)
    {
      return this.map.UpdateConfirmLockComments(loanLockGUID, newComments, onBuySide);
    }

    public bool AddLockField(string loanLockGUID, string fieldId, string newValue)
    {
      return this.map.AddFieldToLockSnapshot(this, loanLockGUID, fieldId, newValue);
    }

    public bool AddLoanNumber
    {
      get => this.addLoanNumber;
      set => this.addLoanNumber = value;
    }

    private void calculateULI()
    {
      if (this.Calculator == null)
        return;
      this.Calculator.FormCalculation("CALCULATEULI", (string) null, (string) null);
    }

    private void calculateloanIdentifier()
    {
      if (this.Calculator == null)
        return;
      this.Calculator.FormCalculation("CALCURLALOANIDENTIFIER", (string) null, (string) null);
    }

    public string LoanNumber
    {
      get
      {
        string fieldAt = this.map.GetFieldAt("364");
        if (!string.IsNullOrEmpty(fieldAt) && string.IsNullOrEmpty(this.GetField("HMDA.X28")))
          this.calculateULI();
        return fieldAt;
      }
      set
      {
        this.SetCurrentField("364", value);
        if (string.IsNullOrEmpty(this.GetField("HMDA.X28")))
          this.calculateULI();
        this.calculateloanIdentifier();
        if (this.validator != null)
          this.validator.Validate("364", value);
        if (this.GetField("1481") == "Y")
        {
          this.SetCurrentField("305", value);
          if (this.validator != null)
            this.validator.Validate("305", value);
        }
        this.SetCurrentField("ULDD.X21", value);
      }
    }

    public string EncompassVersion
    {
      get => this.map.GetFieldAt("SYS.X611");
      set => this.SetCurrentField("SYS.X611", value);
    }

    public string MersNumber
    {
      get => this.map.GetFieldAt("1051");
      set => this.SetCurrentField("1051", value ?? "");
    }

    public string MersOrgId
    {
      get => this.map.GetFieldAt("4722");
      set => this.SetCurrentFieldFromCal("4722", value);
    }

    public string Mom
    {
      get => this.map.GetFieldAt("4723");
      set => this.SetCurrentField("4723", value);
    }

    public Hashtable DirtyTable => this.dirtyTbl2;

    public Hashtable DirtyUserModifiedTable => this.userModifiedFieldsTbl3;

    public Hashtable LockTable => this.lockTbl2;

    public string[] GetRemovedLockedFieldIDs()
    {
      List<string> stringList = new List<string>();
      foreach (string key in (IEnumerable) this.lockTbl2.Keys)
      {
        if (string.Concat(this.lockTbl2[(object) key]) == "")
          stringList.Add(key);
      }
      return stringList.ToArray();
    }

    public string[] GetLockedFieldIDs()
    {
      List<string> stringList = new List<string>();
      foreach (string key in (IEnumerable) this.lockTbl2.Keys)
      {
        if (string.Concat(this.lockTbl2[(object) key]) == "locked")
          stringList.Add(key);
      }
      return stringList.ToArray();
    }

    public BorrowerPair CurrentBorrowerPair
    {
      get
      {
        this.Parse();
        return this.pair;
      }
    }

    public string PairId => this.CurrentBorrowerPair.Borrower.Id;

    public bool IsAdverse() => LoanStatusMap.IsAdverseStatus(this.GetField("1393"));

    public bool IsClonedLoan { get; set; }

    public FieldFormat GetFormat(string id)
    {
      if (string.IsNullOrEmpty(id))
        return FieldFormat.STRING;
      FieldFormat format = this.map[id].DataFormat;
      if (LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(id))
        format = !this.loanSettings.Use10DigitLockSecondaryTradeFields ? FieldFormat.DECIMAL_3 : FieldFormat.DECIMAL_10;
      if (Utils.GetIndexRateFields().Contains(id) && this.GetField("4912") != "FiveDecimals")
        format = FieldFormat.DECIMAL_3;
      if ((id.StartsWith("HHI") || id.StartsWith("HTR") || id.StartsWith("HTD")) && id.EndsWith("02") && this.GetField("HHI.X5") != "FiveDecimals")
        format = FieldFormat.DECIMAL_3;
      return format;
    }

    public FieldDefinition GetFieldDefinition(string id) => this.map[id].Definition;

    public bool IsFieldDefined(string fieldId) => this.map.IsFieldDefined(fieldId);

    public bool IsFieldEmpty(string id)
    {
      return Utils.UnformatValue(this.GetSimpleField(id), this.GetFormat(id)) == "";
    }

    public string ToXmlForSystemLogs()
    {
      return this.xmldoc == null ? this.remoteData.ToString(Encoding.Default) : this.map.MergeChanges(LoanContentAccess.None, true).OuterXml;
    }

    public string ToXml() => this.ToXml(true);

    public string ToXml(bool includeOperationalLog)
    {
      return this.ToXml(this.contentAccess, includeOperationalLog);
    }

    public string ToXml(LoanContentAccess access, bool includeOperationalLog)
    {
      return this.ToXml(access, includeOperationalLog, true);
    }

    public string ToXml(
      LoanContentAccess access,
      bool includeOperationalLog,
      bool replaceCachedXML)
    {
      using (MemoryStream stream = StreamHelper.NewMemoryStream())
      {
        this.WriteXml((Stream) stream, access, includeOperationalLog, replaceCachedXML);
        return stream.ToString(Encoding.Default, true);
      }
    }

    public Stream ToStream() => this.ToStream(true);

    public Stream ToStream(bool includeOperationalLog)
    {
      return this.ToStream(this.contentAccess, includeOperationalLog);
    }

    public Stream ToStream(LoanContentAccess access, bool includeOperationalLog)
    {
      return this.ToStream(access, includeOperationalLog, true);
    }

    public Stream ToStream(
      LoanContentAccess access,
      bool includeOperationalLog,
      bool replaceCachedXML)
    {
      MemoryStream stream = StreamHelper.NewMemoryStream();
      this.WriteXml((Stream) stream, access, includeOperationalLog, replaceCachedXML);
      stream.Seek(0L, SeekOrigin.Begin);
      return (Stream) stream;
    }

    public void WriteXml(
      Stream stream,
      LoanContentAccess access,
      bool includeOperationalLog,
      bool replaceCachedXML)
    {
      if (this.xmldoc == null)
      {
        this.remoteData.Write(stream);
      }
      else
      {
        using (XmlWriter writer = XmlHelper.CreateWriter(stream))
          this.ToXmlDocument(access, includeOperationalLog, replaceCachedXML).WriteTo(writer);
      }
    }

    public XmlDocument ToXmlDocument() => this.ToXmlDocument(this.ContentAccess, true, true);

    private XmlDocument ToXmlDocument(
      LoanContentAccess access,
      bool includeOperationalLog,
      bool replaceCachedXML)
    {
      if (this.xmldoc == null)
        this.parseXml(this.remoteData.AsStream(), false);
      if (!this.Dirty && !this.mergeRequired)
        return this.xmldoc;
      this.mergeRequired = true;
      XmlDocument xmlDocument = this.map.MergeChanges(access, includeOperationalLog, !LoanData.SkipMergeFieldsForFullAccess);
      if (LoanData.SkipMergeFieldsForFullAccess && access == LoanContentAccess.FullAccess)
      {
        this.Dirty = false;
        this.mergeRequired = false;
      }
      else if ((access & LoanContentAccess.FormFields) == LoanContentAccess.FormFields)
      {
        xmlDocument = this.mergeFieldChanges(xmlDocument);
        if (replaceCachedXML)
          this.map.ReplaceCachedXML(xmlDocument);
      }
      return xmlDocument;
    }

    public string ToDraftXml(bool includeOperationalLog = true)
    {
      return this.ToDraftXml(LoanContentAccess.FullAccess, true);
    }

    public string ToDraftXml(LoanContentAccess access, bool includeOperationalLog)
    {
      return this.ToDraftXml(access, includeOperationalLog, true);
    }

    public string ToDraftXml(
      LoanContentAccess access,
      bool includeOperationalLog,
      bool replaceCachedXML)
    {
      if (this.xmldoc == null)
        return this.remoteData.ToString(Encoding.Default);
      if (!this.Dirty && !this.mergeRequired)
        return this.xmldoc.OuterXml;
      this.mergeRequired = true;
      XmlDocument xmlDocument = this.map.MergeChanges(access, includeOperationalLog);
      if ((access & LoanContentAccess.FormFields) == LoanContentAccess.FormFields)
      {
        xmlDocument = this.mergeFieldChanges(xmlDocument);
        if (replaceCachedXML)
          this.map.ReplaceCachedXML(xmlDocument);
      }
      return xmlDocument.OuterXml;
    }

    public bool MergeRequired
    {
      get => this.mergeRequired;
      set => this.mergeRequired = value;
    }

    public LoanData MergeChanges() => new LoanData(this, this.loanSettings, false, false);

    private XmlDocument mergeFieldChanges(XmlDocument xmlDoc)
    {
      try
      {
        LoanData loanData = new LoanData(xmlDoc.CloneNode(true) as XmlDocument, this.loanSettings);
        BorrowerPair currentBorrowerPair = loanData.CurrentBorrowerPair;
        BorrowerPair[] borrowerPairs = loanData.GetBorrowerPairs();
        loanData.SetBorrowerPair(borrowerPairs[0]);
        foreach (DictionaryEntry dictionaryEntry in this.DirtyTable)
        {
          string str = dictionaryEntry.Key.ToString();
          if (!str.StartsWith("NBOC") && !str.StartsWith("TR") && !str.StartsWith("TQLGSE") && !this.IsFieldAccessRestricted(str))
          {
            object[] objArray = (object[]) dictionaryEntry.Value;
            string val = (string) objArray[0];
            BorrowerPair borrowerPair = (BorrowerPair) objArray[1];
            BorrowerPair pair = borrowerPairs[0];
            for (int index = 0; index < borrowerPairs.Length; ++index)
            {
              if (borrowerPair.Id == borrowerPairs[index].Id)
              {
                pair = borrowerPairs[index];
                break;
              }
            }
            if (!this.IsFieldAccessRestricted("LOCKBUTTON_" + str) && this.lockTbl2.ContainsKey((object) str))
            {
              if (string.Concat(this.lockTbl2[(object) str]) == "locked")
              {
                if (!loanData.IsLocked(str))
                  loanData.AddLock(str);
              }
              else if (string.Concat(this.lockTbl2[(object) str]) == "" && loanData.IsLocked(str))
                loanData.RemoveLock(str);
            }
            loanData.SetCurrentField(str, val, pair);
          }
        }
        if (currentBorrowerPair != null && (this.CurrentBorrowerPair == null || currentBorrowerPair.Id != this.CurrentBorrowerPair.Id))
          loanData.SetBorrowerPair(currentBorrowerPair);
        return loanData.xmldoc;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in merging field changes to new xml string. Error: " + ex.Message);
        return xmlDoc;
      }
    }

    private void removeMISMOElements(XmlElement elm)
    {
      ArrayList arrayList = new ArrayList();
      foreach (XmlElement childNode in elm.ChildNodes)
        arrayList.Add((object) childNode);
      foreach (XmlElement elm1 in arrayList)
      {
        if (elm1.Name != "EllieMae")
        {
          elm1.RemoveAllAttributes();
          this.removeMISMOElements(elm1);
        }
      }
      if (elm.ChildNodes.Count != 0)
        return;
      elm.ParentNode.RemoveChild((XmlNode) elm);
    }

    private void removeUsedAttributes(XmlElement elm, XmlElement completeElm)
    {
      foreach (XmlAttribute attribute in (XmlNamedNodeMap) elm.Attributes)
      {
        if (completeElm.HasAttribute(attribute.Name))
          completeElm.RemoveAttribute(attribute.Name);
      }
      foreach (XmlElement childNode in elm.ChildNodes)
      {
        XmlNode completeElm1 = completeElm.SelectSingleNode(childNode.Name);
        if (completeElm1 != null)
          this.removeUsedAttributes(childNode, (XmlElement) completeElm1);
      }
      if (completeElm.Attributes.Count != 0 || completeElm.ChildNodes.Count != 0)
        return;
      completeElm.ParentNode.RemoveChild((XmlNode) completeElm);
    }

    private void removeIllegalEmptyStrings(XmlElement root)
    {
      foreach (string attr in LoanData.attrs)
      {
        foreach (XmlNode selectNode in root.SelectNodes(attr))
        {
          if (selectNode.Value == string.Empty)
            ((XmlAttribute) selectNode).OwnerElement.RemoveAttribute(attr.Substring(attr.IndexOf('@') + 1));
        }
      }
      ArrayList arrayList = new ArrayList();
      foreach (string elm in LoanData.elms)
      {
        foreach (XmlElement selectNode in root.SelectNodes(elm))
        {
          arrayList.Clear();
          foreach (XmlAttribute attribute in (XmlNamedNodeMap) selectNode.Attributes)
          {
            if (attribute.Value == string.Empty)
              arrayList.Add((object) attribute.Name);
          }
          foreach (string name in arrayList)
            selectNode.RemoveAttribute(name);
        }
      }
    }

    private void rearrangeOrder(XmlNode sample, XmlNode data)
    {
      XmlNode refChild = (XmlNode) null;
      foreach (XmlNode childNode in sample.ChildNodes)
      {
        foreach (XmlNode selectNode in data.SelectNodes(childNode.Name))
        {
          if (refChild == null)
          {
            if (data.FirstChild != selectNode)
            {
              data.RemoveChild(selectNode);
              data.InsertBefore(selectNode, data.FirstChild);
            }
            refChild = selectNode;
          }
          else
          {
            data.RemoveChild(selectNode);
            data.InsertAfter(selectNode, refChild);
            refChild = selectNode;
          }
          this.rearrangeOrder(childNode, selectNode);
        }
      }
    }

    private void validateXml()
    {
      XmlReaderSettings settings = new XmlReaderSettings();
      settings.ValidationType = ValidationType.Schema;
      settings.Schemas.Add(LoanData.schema);
      settings.ValidationEventHandler += new ValidationEventHandler(this.validationHandler);
      XmlReader xmlReader = XmlReader.Create((XmlReader) new XmlTextReader((TextReader) new StringReader(this.xmldoc.DocumentElement.OuterXml)), settings);
      this.validationErrors = string.Empty;
      this.errInd = 1;
      do
        ;
      while (xmlReader.Read());
      if (!(this.validationErrors != string.Empty))
        return;
      Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "xml validation errors\r\n" + this.validationErrors);
    }

    private void validationHandler(object sender, ValidationEventArgs args)
    {
      this.validationErrors = this.validationErrors + "(" + (object) this.errInd++ + ") " + args.Message + Environment.NewLine;
    }

    public int GetPairIndex(string pId)
    {
      BorrowerPair[] borrowerPairs = this.map.GetBorrowerPairs();
      int pairIndex = -10;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Borrower.Id == pId)
        {
          pairIndex = index;
          break;
        }
      }
      return pairIndex;
    }

    public int GetPairIndexByEID(string eid)
    {
      BorrowerPair[] borrowerPairs = this.map.GetBorrowerPairs();
      int pairIndexByEid = -1;
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        if (borrowerPairs[index].Borrower.EID == eid)
        {
          pairIndexByEid = index;
          break;
        }
      }
      return pairIndexByEid;
    }

    public string GetPairID(int pairIndex)
    {
      BorrowerPair[] borrowerPairs = this.map.GetBorrowerPairs();
      if (pairIndex < 0 || pairIndex > borrowerPairs.Length - 1)
        throw new ArgumentOutOfRangeException(nameof (pairIndex));
      return borrowerPairs[pairIndex].Id;
    }

    public BorrowerPair[] GetBorrowerPairs() => this.map.GetBorrowerPairs();

    public BorrowerPair GetBorrowerPair(string pairID)
    {
      foreach (BorrowerPair borrowerPair in this.map.GetBorrowerPairs())
      {
        if (borrowerPair.Id == pairID)
          return borrowerPair;
      }
      return (BorrowerPair) null;
    }

    public BorrowerPair GetBorrowerPairByEID(string eid)
    {
      foreach (BorrowerPair borrowerPair in this.map.GetBorrowerPairs())
      {
        if (borrowerPair.Borrower.EID == eid)
          return borrowerPair;
      }
      return (BorrowerPair) null;
    }

    public BorrowerPair CreateBorrowerPair()
    {
      this.Dirty = true;
      BorrowerPair borrowerPair = this.map.CreateBorrowerPair();
      this.onBorrowerPairCreated(borrowerPair.Id);
      this.pairs = this.map.GetBorrowerPairs();
      this.SetField("4460", string.Concat((object) this.pairs.Length));
      return borrowerPair;
    }

    public void ChangeLoanToSecond()
    {
      this.Dirty = true;
      this.map.ChangeLoanToSecond();
      this.onLienPositionChanged();
    }

    public void RemoveLockRequest()
    {
      this.Dirty = true;
      this.map.RemoveLockRequest();
    }

    public void AddServicingTransaction(
      ServicingTransactionBase transactionLog,
      bool recalcServicing)
    {
      this.Dirty = true;
      this.map.AddServicingTransaction(transactionLog);
      if (!recalcServicing)
        return;
      this.Calculator.CalculateInterimServicing(true);
    }

    public void AddServicingTransaction(ServicingTransactionBase transactionLog)
    {
      this.AddServicingTransaction(transactionLog, true);
    }

    public void UpdateServicingTransaction(ServicingTransactionBase transactionLog)
    {
      this.AddServicingTransaction(transactionLog);
    }

    public bool RemoveServicingTransaction(ServicingTransactionBase transactionLog)
    {
      this.Dirty = true;
      foreach (ServicingTransactionBase servicingTransaction in this.GetServicingTransactions(true))
      {
        if (servicingTransaction is PaymentReversalLog && ((PaymentReversalLog) servicingTransaction).PaymentGUID == transactionLog.TransactionGUID)
          this.map.RemoveServicingTransaction(servicingTransaction.TransactionGUID);
      }
      this.map.RemoveServicingTransaction(transactionLog.TransactionGUID);
      this.reindexServicingTransactions();
      this.Calculator.CalculateInterimServicing(true);
      return true;
    }

    private void reindexServicingTransactions()
    {
      ServicingTransactionBase[] servicingTransactions = this.GetServicingTransactions(true);
      if (servicingTransactions == null)
        return;
      int num1 = 1;
      int num2 = 1;
      int num3 = 1;
      foreach (ServicingTransactionBase transactionLog in servicingTransactions)
      {
        bool flag = false;
        if (transactionLog is PaymentTransactionLog)
        {
          PaymentTransactionLog paymentTransactionLog = (PaymentTransactionLog) transactionLog;
          if (paymentTransactionLog.PaymentNo != num2)
          {
            paymentTransactionLog.PaymentNo = num2;
            flag = true;
          }
          ++num2;
        }
        else if (transactionLog is EscrowDisbursementLog)
        {
          EscrowDisbursementLog escrowDisbursementLog = (EscrowDisbursementLog) transactionLog;
          if (escrowDisbursementLog.DisbursementNo != num3)
          {
            escrowDisbursementLog.DisbursementNo = num3;
            flag = true;
          }
          ++num3;
        }
        if (transactionLog.TransactionNo != num1)
        {
          transactionLog.TransactionNo = num1;
          flag = true;
        }
        ++num1;
        if (flag)
          this.map.AddServicingTransaction(transactionLog);
      }
    }

    public ServicingTransactionBase[] GetServicingTransactions()
    {
      return this.GetServicingTransactions(false);
    }

    public ServicingTransactionBase[] GetServicingTransactions(bool skipSchedule)
    {
      return this.map.GetServicingTransactions(skipSchedule);
    }

    public bool StartInterimServicing()
    {
      this.RemoveInterimServicing();
      if (!this.map.StartInterimServicing(this.calculator.CalculateInterimServicingPaymentSchedule(true)))
        return false;
      this.SetField("SERVICE.X1", this.GetField("364"));
      this.SetField("SERVICE.X2", this.GetField("36"));
      this.SetField("SERVICE.X3", this.GetField("37"));
      this.SetField("SERVICE.X140", this.GetField("66"));
      this.SetField("SERVICE.X141", this.GetField("FE0117"));
      this.SetField("SERVICE.X142", this.GetField("1490"));
      this.SetField("SERVICE.X143", this.GetField("1240"));
      if (this.GetField("1811") == "PrimaryResidence")
      {
        this.SetField("SERVICE.X4", this.GetField("11"));
        this.SetField("SERVICE.X5", this.GetField("12"));
        this.SetField("SERVICE.X6", this.GetField("14"));
        this.SetField("SERVICE.X7", this.GetField("15"));
      }
      else if (this.GetField("1416") == "")
      {
        this.SetField("SERVICE.X4", this.GetField("FR0104"));
        this.SetField("SERVICE.X5", this.GetField("FR0106"));
        this.SetField("SERVICE.X6", this.GetField("FR0107"));
        this.SetField("SERVICE.X7", this.GetField("FR0108"));
      }
      else
      {
        this.SetField("SERVICE.X4", this.GetField("1416"));
        this.SetField("SERVICE.X5", this.GetField("1417"));
        this.SetField("SERVICE.X6", this.GetField("1418"));
        this.SetField("SERVICE.X7", this.GetField("1419"));
      }
      this.SetCurrentFieldFromCal("SERVICE.X9", "");
      this.SetCurrentFieldFromCal("SERVICE.X10", "");
      this.SetCurrentFieldFromCal("SERVICE.X11", "");
      this.SetCurrentFieldFromCal("SERVICE.X12", "");
      this.SetCurrentFieldFromCal("SERVICE.X14", "");
      this.SetCurrentFieldFromCal("SERVICE.X99", "");
      switch (this.GetField("2626"))
      {
        case "Correspondent":
          this.SetField("SERVICE.X144", this.GetField("3579"));
          break;
        case "Banked - Retail":
        case "Banked - Wholesale":
          this.SetField("SERVICE.X144", this.GetField("2"));
          break;
        default:
          this.SetField("SERVICE.X144", this.GetField("2"));
          break;
      }
      this.Calculator.AttachIntermServicingFieldHandlers();
      this.Calculator.CalculateInterimServicing(false);
      return true;
    }

    public bool RemoveInterimServicing()
    {
      try
      {
        for (int index = 13; index <= 22; ++index)
          this.RemoveCurrentLock("SERVICE.X" + index.ToString());
        this.RemoveCurrentLock("SERVICE.X25");
        for (int index = 58; index <= 73; ++index)
          this.RemoveCurrentLock("SERVICE.X" + index.ToString());
        this.map.ClearServicingTransactions();
        return true;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "The existing Interim Servicing can't be cleaned up due to the following error: " + ex.Message);
      }
      return false;
    }

    public PaymentScheduleSnapshot GetPaymentScheduleSnapshot()
    {
      return this.map.GetPaymentScheduleSnapshot();
    }

    public bool RemoveCoborrowers(BorrowerPair[] pairs)
    {
      this.Dirty = true;
      bool flag = this.map.RemoveCoborrowers(pairs);
      if (flag)
        this.onBorrowerPairChanged();
      return flag;
    }

    public void RemoveBorrowerPair(BorrowerPair pair)
    {
      this.Dirty = true;
      this.map.RemoveBorrowerPair(pair);
      if (this.dirtyTbl2 != null)
      {
        string id = pair.Id;
        foreach (string key in this.dirtyTbl2.Keys.Cast<string>().ToList<string>())
        {
          object[] objArray = (object[]) this.dirtyTbl2[(object) key];
          if (objArray != null && objArray.Length >= 2 && ((BorrowerPair) objArray[1]).Id == id)
            this.dirtyTbl2.Remove((object) key);
        }
      }
      this.pairs = this.map.GetBorrowerPairs();
      this.onBorrowerPairChanged();
    }

    private void eSignConsentSwapBorrowerCoBorrower(
      int sourcePair,
      bool sourceForCoborrower,
      int targetPair,
      bool targetForCoborrower)
    {
      string[] strArray1 = new string[6];
      string[] strArray2 = new string[6];
      string[] strArray3 = !sourceForCoborrower ? LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) sourcePair] : LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) sourcePair];
      string[] strArray4 = !targetForCoborrower ? LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) targetPair] : LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) targetPair];
      for (int index = 0; index < 6; ++index)
      {
        string field = this.GetField(strArray3[index]);
        this.SetField(strArray3[index], this.GetField(strArray4[index]));
        this.SetField(strArray4[index], field);
      }
      this.onBorrowerPairChanged();
    }

    public void eSignConsentSwapBorrowerPairs(int pair1, int pair2)
    {
      string[] strArray1 = new string[6];
      string[] strArray2 = new string[6];
      string[] strArray3 = new string[6];
      string[] strArray4 = new string[6];
      string[] coborrowerFieldId1 = LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) pair1];
      string[] coborrowerFieldId2 = LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) pair1];
      string[] coborrowerFieldId3 = LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) pair2];
      string[] coborrowerFieldId4 = LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) pair2];
      for (int index = 0; index < 6; ++index)
      {
        string field1 = this.GetField(coborrowerFieldId1[index]);
        this.SetField(coborrowerFieldId1[index], this.GetField(coborrowerFieldId3[index]));
        this.SetField(coborrowerFieldId3[index], field1);
        string field2 = this.GetField(coborrowerFieldId2[index]);
        this.SetField(coborrowerFieldId2[index], this.GetField(coborrowerFieldId4[index]));
        this.SetField(coborrowerFieldId4[index], field2);
      }
      this.onBorrowerPairChanged();
    }

    public void eSignConsentDeleteBorrowerPair(int pair, int numPairs)
    {
      string[] strArray1 = new string[6];
      string[] strArray2 = new string[6];
      string[] strArray3 = new string[6];
      string[] strArray4 = new string[6];
      for (int index1 = pair; index1 < numPairs; ++index1)
      {
        if (index1 < 6)
        {
          string[] coborrowerFieldId1 = LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) index1];
          string[] coborrowerFieldId2 = LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) index1];
          string[] coborrowerFieldId3 = LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) (index1 + 1)];
          string[] coborrowerFieldId4 = LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) (index1 + 1)];
          for (int index2 = 0; index2 < 6; ++index2)
          {
            this.SetField(coborrowerFieldId1[index2], this.GetField(coborrowerFieldId3[index2]));
            this.SetField(coborrowerFieldId2[index2], this.GetField(coborrowerFieldId4[index2]));
          }
        }
        if (index1 == 6)
        {
          string[] coborrowerFieldId5 = LoanData.eConsentBorrowerCoborrower_fieldIDs["b" + (object) 6];
          string[] coborrowerFieldId6 = LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) 6];
          for (int index3 = 0; index3 < 6; ++index3)
          {
            this.SetField(coborrowerFieldId5[index3], "");
            this.SetField(coborrowerFieldId6[index3], "");
          }
        }
      }
      this.onBorrowerPairChanged();
    }

    public void eSignConsentDeleteCoBorrower(int pair)
    {
      string[] coborrowerFieldId = LoanData.eConsentBorrowerCoborrower_fieldIDs["c" + (object) pair];
      for (int index = 0; index < 6; ++index)
        this.SetField(coborrowerFieldId[index], "");
      this.onBorrowerPairChanged();
    }

    public void SwapBorrowers(
      int sourcePair,
      bool sourceForCoborrower,
      int targetPair,
      bool targetForCoborrower)
    {
      if (!this.dirty)
        this.Dirty = true;
      this.map.SwapBorrowers(sourcePair, sourceForCoborrower, targetPair, targetForCoborrower);
      this.eSignConsentSwapBorrowerCoBorrower(sourcePair, sourceForCoborrower, targetPair, targetForCoborrower);
      this.onBorrowerPairChanged();
      this.calculator.FormCalculation("SWAPBORROWERS");
    }

    public void SwapBorrowers(BorrowerPair[] pairs)
    {
      this.Dirty = true;
      this.map.SwapBorrowers(pairs);
      int num = this.GetPairIndex(pairs[0].Id) + 1;
      if (num > 0)
        this.eSignConsentSwapBorrowerCoBorrower(num, true, num, false);
      this.onBorrowerPairChanged();
    }

    public void SwapBorrowerPairs(BorrowerPair[] pairs)
    {
      this.Dirty = true;
      int pair1 = this.GetPairIndex(pairs[0].Id) + 1;
      int pair2 = this.GetPairIndex(pairs[1].Id) + 1;
      this.map.SwapBorrowerPairs(pairs);
      this.pairs = this.map.GetBorrowerPairs();
      if (pair1 > 0 && pair2 > 0)
        this.eSignConsentSwapBorrowerPairs(pair1, pair2);
      this.onBorrowerPairChanged();
    }

    public void SetBorrowerPair(BorrowerPair pair)
    {
      if (this.pair?.Id == pair.Id)
        return;
      this.map.SetBorrowerPair(pair);
      this.pair = pair;
      this.currentBorPairIndex = this.GetPairIndex(this.pair.Id) + 1;
    }

    public int GetNumberOfBorrowerPairs()
    {
      this.pairs = this.map.GetBorrowerPairs();
      return this.pairs.Length;
    }

    public void SetBorrowerPair(int pairIndex)
    {
      if (this.pairs == null)
        this.pairs = this.map.GetBorrowerPairs();
      this.map.SetBorrowerPair(this.pairs[pairIndex]);
      this.pair = this.pairs[pairIndex];
      this.currentBorPairIndex = this.GetPairIndex(this.pair.Id) + 1;
    }

    public void RegisterFieldValueChangeEventHandler(string id, Routine fx)
    {
      Field field = this.map[id];
      if (field == Field.Empty)
        return;
      field.Events.CalProc += fx;
    }

    public void UnregisterFieldValueChangeEventHandler(string id, Routine fx)
    {
      Field field = this.map[id];
      if (field == Field.Empty)
        return;
      field.Events.CalProc -= fx;
    }

    public void RegisterCustomFieldValueChangeEventHandler(string id, Routine fx)
    {
      Field field = this.map[id];
      if (field == Field.Empty)
        return;
      field.Events.CustomCalProc -= fx;
      field.Events.CustomCalProc += fx;
    }

    public void UnregisterCustomFieldValueChangeEventHandler(string id, Routine fx)
    {
      Field field = this.map[id];
      if (field == Field.Empty)
        return;
      field.Events.CustomCalProc -= fx;
    }

    public void RegisterFieldValueValidationEventHandler(string id, Validation fx)
    {
      Field field = this.map[id];
      if (field == Field.Empty)
        return;
      field.Events.ValidationProc += fx;
    }

    public void UnregisterFieldValueValidationEventHandler(string id, Validation fx)
    {
      Field field = this.map[id];
      if (field == Field.Empty)
        return;
      field.Events.ValidationProc -= fx;
    }

    public string GetSimpleField(string id) => this.GetSimpleField(id, this.CurrentBorrowerPair);

    public string GetSimpleField(string id, int pairIndex)
    {
      return this.GetSimpleField(id, this.pairs[pairIndex]);
    }

    public string GetSimpleField(string id, BorrowerPair borrowerPair)
    {
      if (string.IsNullOrWhiteSpace(id))
        return string.Empty;
      FieldDefinition fieldDefinition = this.GetFieldDefinition(id);
      return this.GetFieldValueForFieldDefinition(id, borrowerPair, fieldDefinition, true);
    }

    private string GetFieldValueForFieldDefinition(
      string id,
      BorrowerPair borrowerPair,
      FieldDefinition fieldDef,
      bool formatFieldValue)
    {
      string val = !(fieldDef is PersistentField) ? fieldDef.GetValue(this) : this.map.GetFieldAt(id, borrowerPair);
      return formatFieldValue && (fieldDef.Format == FieldFormat.DATE || fieldDef.Format == FieldFormat.DATETIME) && val.Contains("-") ? this.FormatValue(val, fieldDef.Format) : val;
    }

    public string GetRawSimpleField(string id, BorrowerPair pair)
    {
      FieldDefinition fieldDefinition = this.GetFieldDefinition(id);
      return this.GetFieldValueForFieldDefinition(id, pair, fieldDefinition, false);
    }

    public string GetRawSimpleField(string id)
    {
      FieldDefinition fieldDefinition = this.GetFieldDefinition(id);
      return this.GetFieldValueForFieldDefinition(id, this.CurrentBorrowerPair, fieldDefinition, false);
    }

    public string GetSimpleFieldForReportingDB(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        return string.Empty;
      FieldDefinition fieldDefinition = this.GetFieldDefinition(id);
      if (!(fieldDefinition is DocumentTrackingField))
        return this.GetFieldValueForFieldDefinition(id, this.CurrentBorrowerPair, fieldDefinition, true);
      DocumentTrackingField documentTrackingField = (DocumentTrackingField) fieldDefinition;
      if (this.cachedDocumentLogsForReportingDatabase == null)
        this.cachedDocumentLogsForReportingDatabase = this.GetLogList().GetAllRecordsOfType(typeof (DocumentLog));
      return documentTrackingField.EvaluateUsingCachedDocumentLogs(this.cachedDocumentLogsForReportingDatabase);
    }

    public void TriggerCalculation(string id, string val)
    {
      this.TriggerCalculation(id, val, false);
    }

    public void TriggerCalculation(string id, string val, bool customOnly)
    {
      this.TriggerCalculation(id, val, customOnly, 0);
    }

    public void TriggerCalculation(string id, string val, bool customOnly, int pairIndex)
    {
      try
      {
        if (this.calculator != null)
        {
          this.calculator.InitializingCalculation((string) null, (string) null);
          if (this.Settings.FieldSettings.LockRequestAdditionalFields != null && this.Settings.FieldSettings.LockRequestAdditionalFields.IsAdditionalField(id, true))
            this.calculator.SpecialCalculation(CalculationActionID.CopyLoanToLockRequestAdditionalFields, id, val);
          if (this.Settings.HMDAInfo != null && this.Settings.HMDAInfo.HMDAIncome == id)
            this.calculator.FormCalculation("HMDAINCOME");
        }
        if (!customOnly)
          this.map[id].Events.RaiseCalProc(id, val);
        if (pairIndex > 0)
          this.map[id].Events.RaiseCustomCalProc(id + "#" + (object) pairIndex, val);
        else
          this.map[id].Events.RaiseCustomCalProc(id, val);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Exception in TriggerCalculation (" + id + ", " + val + "): " + ex.Message);
        if (ex is ComplianceCalendarException)
          throw ex;
        if (ex.InnerException is ComplianceCalendarException)
          throw ex.InnerException;
        if (ex is GFEDaysToExpireException)
          throw ex;
        if (ex.InnerException is GFEDaysToExpireException)
          throw ex.InnerException;
        if (ex is CountyLimitException)
          throw ex;
        if (ex.InnerException is CountyLimitException)
          throw ex.InnerException;
      }
    }

    public bool TriggerValidation(string id, string val)
    {
      try
      {
        return this.map[id].Events.PerformValidationProc(id, val);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Exception in TriggerValidation (" + id + ", " + val + "): " + ex.Message);
        throw ex;
      }
    }

    public string GetFieldFromCal(string id)
    {
      return this.IsLocked(id) ? this.GetOrgField(id) : this.GetField(id);
    }

    [NoTrace]
    public void SetFieldFromCal(string id, string val)
    {
      if (this.BeforeFieldChanged != null && !this.BeforeFieldChanged(id, val, this.GetSimpleField(id)))
        return;
      if (this.IsLocked(id))
      {
        this.Dirty = true;
        this.map.SetOrgFieldAt(id, val);
      }
      else
        this.SetField(id, val);
    }

    [NoTrace]
    public void SetCurrentFieldFromCal(string id, string val)
    {
      if (this.IsLocked(id))
      {
        this.Dirty = true;
        this.map.SetOrgFieldAt(id, val);
      }
      else
        this.SetCurrentField(id, val);
    }

    public string FormatValue(string val, FieldFormat format)
    {
      return Utils.ApplyFieldFormatting(val, format);
    }

    internal string GetAdditionalField(string id, FieldFormat format)
    {
      return this.FormatValue(this.map.GetAdditionalField(id), format);
    }

    internal bool SetAdditionalField(string id, string val) => this.map.SetAdditionalField(id, val);

    public string GetField(string id)
    {
      string field = this.FormatValue(this.GetSimpleField(id), this.GetFormat(id));
      if (this.hiddenFields != null && this.hiddenFields.ContainsKey((object) id))
        field = "*";
      return field;
    }

    public string GetField(string id, int borIndex)
    {
      if (this.pairs == null)
        this.pairs = this.GetBorrowerPairs();
      if (this.pairs == null || this.pairs.Length <= borIndex)
        return string.Empty;
      if (borIndex < 0)
        borIndex = 0;
      return this.GetField(id, this.pairs[borIndex]);
    }

    public string GetField(string id, BorrowerPair pair)
    {
      string field = this.FormatValue(this.GetSimpleField(id, pair), this.GetFormat(id));
      if (id == "VASUMM.X23" && field.IndexOf(",") >= 0)
        field = field.Replace(",", "");
      if (this.hiddenFields != null && this.hiddenFields.ContainsKey((object) id))
        field = "*";
      return field;
    }

    public double FltVal(string id)
    {
      string simpleField = this.GetSimpleField(id);
      try
      {
        return simpleField == LoanData.nil ? 0.0 : double.Parse(simpleField);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Routine: FltVal  id: " + id + " " + ex.Message);
        return 0.0;
      }
    }

    public object GetNativeField(string id)
    {
      return Utils.ConvertToNativeValue(this.GetSimpleField(id), this.GetFormat(id), (object) null);
    }

    [NoTrace]
    public void SetField(string id, string val) => this.SetField(id, val, false);

    [NoTrace]
    public void SetField(string id, string val, bool isUserModified)
    {
      this.SetField(id, val, false, isUserModified);
    }

    [NoTrace]
    public void SetField(string id, string val, bool applyLoanTemplate, bool isUserModified)
    {
      if ((this.validator == null || this.validator.Enabled) && this.IsFieldReadOnly(id))
        return;
      if (this.validator != null)
        this.validator.Validate(id, val);
      this.SetCurrentField(id, val, false, isUserModified);
      if (!applyLoanTemplate || this.TriggerRuleLoanTemplateChecked == null)
        return;
      this.TriggerRuleLoanTemplateChecked((object) new string[2]
      {
        id,
        val
      }, new EventArgs());
    }

    public void RemoveLicenseNodes() => this.map.RemoveLicenseNodes();

    public bool IsLocked(string id) => this.map.GetOrgFieldAt(id) != null;

    public void RemoveCurrentLock(string id)
    {
      this.Dirty = true;
      this.lockTbl2[(object) id] = (object) "";
      this.map.RemoveLockAt(id);
    }

    public void RemoveLock(string id)
    {
      this.Dirty = true;
      this.lockTbl2[(object) id] = (object) "";
      string orgFieldAt = this.map.GetOrgFieldAt(id);
      this.map.RemoveLockAt(id);
      this.SetField(id, orgFieldAt);
    }

    public string GetOrgField(string id)
    {
      string orgFieldAt = this.map.GetOrgFieldAt(id);
      return orgFieldAt == null ? string.Empty : this.FormatValue(orgFieldAt, this.GetFormat(id));
    }

    public void AddLock(string id)
    {
      this.Dirty = true;
      this.map.SetOrgFieldAt(id, this.map.GetFieldAt(id));
      this.lockTbl2[(object) id] = (object) "locked";
    }

    [NoTrace]
    public void SetCurrentField(string id, string val) => this.SetCurrentField(id, val, true);

    [NoTrace]
    public void SetCurrentField(string id, string val, bool customCalcsOnly)
    {
      this.SetCurrentField(id, val, this.CurrentBorrowerPair, customCalcsOnly);
    }

    public void SetCreatedDateInUTC(DateTime val)
    {
      this.map.SetCreatedDateInUTC(val.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
    }

    [NoTrace]
    public void SetCurrentField(string id, string val, BorrowerPair pair)
    {
      this.SetCurrentField(id, val, pair, true);
    }

    [NoTrace]
    public void SetCurrentField(string id, string val, bool customCalcsOnly, bool isUserModified)
    {
      this.SetCurrentField(id, val, this.CurrentBorrowerPair, customCalcsOnly, isUserModified);
    }

    [NoTrace]
    public void SetCurrentField(
      string id,
      string val,
      BorrowerPair pair,
      bool customCalcsOnly,
      bool isUserModified = false)
    {
      if (pair == null)
        pair = this.CurrentBorrowerPair;
      FieldDefinition fieldDefinition = this.GetFieldDefinition(id);
      if (fieldDefinition != null)
      {
        if (fieldDefinition != FieldDefinition.Empty)
        {
          try
          {
            if (Utils.GetIndexRateFields().Contains(id) && this.GetField("4912") != "FiveDecimals")
            {
              if (val == null)
                val = string.Empty;
              val = val.Trim();
              if (val != "")
                val = Utils.ParseDecimal((object) val, true).ToString("F3");
            }
            else
              val = fieldDefinition.ValidateFormat(val);
          }
          catch (FormatException ex)
          {
            string str = "The value '" + val + "' is invalid for field '" + id + "'. " + ex.Message;
            if (!this.IgnoreValidationErrors)
              throw new FieldValidationException(id, val, str);
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), str);
            return;
          }
          if (pair.Id == this.CurrentBorrowerPair.Id && !this.TriggerValidation(id, val))
            return;
          string fieldAt = this.map.GetFieldAt(id, pair);
          bool flag;
          if (fieldDefinition is PersistentField)
          {
            flag = this.map.SetFieldAt(id, val, pair);
          }
          else
          {
            fieldDefinition.SetValue(this, val);
            flag = true;
          }
          if (flag)
            flag = fieldAt != this.map.GetFieldAt(id);
          if (!flag)
            return;
          this.Dirty = true;
          this.updateDirtyTbl2(id, val, pair);
          if (isUserModified)
            this.updateUserModifiedFieldsDirty(id, val, pair);
          if (!(pair.Id == this.CurrentBorrowerPair.Id))
            return;
          this.dirtyTbl[(object) id] = LoanData.nobj;
          this.TriggerCalculation(id, val, customCalcsOnly, fieldDefinition.RequiresBorrowerPredicate ? this.currentBorPairIndex : 0);
          return;
        }
      }
      Tracing.Log(LoanData.sw, TraceLevel.Warning, nameof (LoanData), "Attempt to set value '" + val + "' for undefined field '" + id + "'");
      throw new InvalidOperationException("The specified field '" + id + "' is not defined and cannot be updated.");
    }

    public string ReformatFieldValue(FieldDefinition fieldDef, string originalValue)
    {
      return (fieldDef.Format == FieldFormat.STATE || fieldDef.Format == FieldFormat.ZIPCODE) && this.IsForeignIndicatorSelected(fieldDef.FieldID) ? originalValue : fieldDef.ValidateFormat(originalValue);
    }

    public bool IsForeignIndicatorSelected(string fieldId)
    {
      string empty = string.Empty;
      try
      {
        int index = 0;
        if (fieldId.IndexOf("#") > 0)
        {
          index = Utils.ParseInt((object) fieldId.Substring(fieldId.IndexOf("#") + 1, 1), 0) - 1;
          if (index > 0)
            fieldId = fieldId.Substring(0, fieldId.Length - 2);
        }
        string id;
        if (fieldId.StartsWith("FR") || fieldId.StartsWith("FE") && !fieldId.StartsWith("FEMA") || fieldId.StartsWith("BE") || fieldId.StartsWith("CE") || fieldId.StartsWith("FM") || fieldId.StartsWith("DD") || fieldId.StartsWith("FL") || fieldId.StartsWith("BR") || fieldId.StartsWith("CR"))
        {
          string key = fieldId.Substring(0, 2) + fieldId.Substring(fieldId.Length > 6 ? 5 : 4, 2);
          id = this.foreignAddressIndictorLookupTable.ContainsKey(key) ? fieldId.Substring(0, fieldId.Length > 6 ? 5 : 4) + this.foreignAddressIndictorLookupTable[key] : string.Empty;
        }
        else if (fieldId.StartsWith("URLAROL"))
        {
          string key = fieldId.Substring(0, 7) + fieldId.Substring(fieldId.Length > 11 ? 10 : 9, 2);
          id = this.foreignAddressIndictorLookupTable.ContainsKey(key) ? fieldId.Substring(0, fieldId.Length > 11 ? 10 : 9) + this.foreignAddressIndictorLookupTable[key] : string.Empty;
        }
        else if (fieldId.StartsWith("NBOC"))
        {
          string key = fieldId.Substring(0, 4) + fieldId.Substring(fieldId.Length > 8 ? 7 : 6, 2);
          id = this.foreignAddressIndictorLookupTable.ContainsKey(key) ? fieldId.Substring(0, fieldId.Length > 8 ? 7 : 6) + this.foreignAddressIndictorLookupTable[key] : string.Empty;
        }
        else
          id = this.foreignAddressIndictorLookupTable.ContainsKey(fieldId) ? this.foreignAddressIndictorLookupTable[fieldId] : string.Empty;
        if (string.IsNullOrEmpty(id))
          return false;
        if (index <= 0)
          return this.GetSimpleField(id) == "Y";
        if (this.pairs == null)
          this.GetNumberOfBorrowerPairs();
        return this.pairs != null && index < this.pairs.Length && this.GetSimpleField(id, this.pairs[index]) == "Y";
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    [NoTrace]
    public void SetCurrentFieldFromCal(string id, string val, BorrowerPair pair)
    {
      if (this.map.GetOrgFieldAt(id, pair) != null)
      {
        this.Dirty = true;
        this.map.SetOrgFieldAt(id, val, pair);
      }
      else
        this.SetCurrentField(id, val, pair, true);
    }

    public string[] SelectedFields
    {
      get
      {
        int num = 0;
        string[] selectedFields = new string[this.findFieldTable.Count];
        foreach (DictionaryEntry dictionaryEntry in this.findFieldTable)
          selectedFields[num++] = dictionaryEntry.Key.ToString();
        return selectedFields;
      }
    }

    public void ClearSelectedFields()
    {
      if (this.findFieldTable != null)
        this.findFieldTable.Clear();
      else
        this.findFieldTable = new Hashtable();
    }

    public LoanData.FindFieldTypes SelectedFieldType(string id)
    {
      return this.findFieldTable == null || !this.findFieldTable.ContainsKey((object) id) ? LoanData.FindFieldTypes.None : (LoanData.FindFieldTypes) this.findFieldTable[(object) id];
    }

    public void AddSelectedField(string id, LoanData.FindFieldTypes fieldType)
    {
      if (this.isInFindFieldForm && this.isSingleFieldSelection)
        this.findFieldTable.Clear();
      if (this.findFieldTable.ContainsKey((object) id))
        return;
      this.findFieldTable.Add((object) id, (object) fieldType);
    }

    public void AddSelectedField(string id)
    {
      this.AddSelectedField(id, LoanData.FindFieldTypes.NewSelect);
    }

    public bool RemoveSelectedField(string id)
    {
      if (this.findFieldTable.ContainsKey((object) id))
      {
        if ((LoanData.FindFieldTypes) this.findFieldTable[(object) id] == LoanData.FindFieldTypes.Existing)
          return false;
        this.findFieldTable.Remove((object) id);
      }
      return true;
    }

    public bool IsDirty(string id) => this.dirtyTbl[(object) id] != null;

    public void ClearDirtyTable() => this.dirtyTbl.Clear();

    public void CleanField(string id) => this.dirtyTbl.Remove((object) id);

    public int GetNumberOfAdditionalLoans() => this.map.GetNumberOfAdditionalLoans();

    public int GetNumberOfLiabilitesExlcudingAlimonyJobExp()
    {
      return this.map.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
    }

    private void updateMultiInstanceFieldsInDirtyTable(
      string prefixID,
      int originalIndex,
      int newIndex)
    {
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      foreach (DictionaryEntry dictionaryEntry in this.dirtyTbl2)
      {
        string key1 = dictionaryEntry.Key.ToString();
        if (key1.StartsWith(prefixID))
        {
          int num1 = Utils.ParseInt((object) key1.Substring(prefixID.Length, originalIndex > 99 ? 3 : 2));
          int num2 = Utils.ParseInt((object) key1.Substring(prefixID.Length, newIndex > 99 ? 3 : 2));
          if (num1 == originalIndex)
          {
            string str = key1.Substring(prefixID.Length + (originalIndex > 99 ? 3 : 2));
            string key2 = prefixID + newIndex.ToString("00") + str;
            insensitiveHashtable.Add((object) key2, (object) (object[]) this.dirtyTbl2[(object) key1]);
            stringList2.Add(key1);
            if (this.dirtyTbl.ContainsKey((object) key1))
            {
              stringList1.Add(key2);
              this.dirtyTbl.Remove((object) key1);
            }
          }
          else if (num2 == newIndex)
          {
            string str = key1.Substring(prefixID.Length + (newIndex > 99 ? 3 : 2));
            string key3 = prefixID + originalIndex.ToString("00") + str;
            insensitiveHashtable.Add((object) key3, (object) (object[]) this.dirtyTbl2[(object) key1]);
            stringList2.Add(key1);
            if (this.dirtyTbl.ContainsKey((object) key1))
            {
              stringList1.Add(key3);
              this.dirtyTbl.Remove((object) key1);
            }
          }
        }
      }
      for (int index = 0; index < stringList2.Count; ++index)
        this.dirtyTbl2.Remove((object) stringList2[index]);
      foreach (DictionaryEntry dictionaryEntry in insensitiveHashtable)
      {
        if (!this.dirtyTbl2.ContainsKey(dictionaryEntry.Key))
          this.dirtyTbl2.Add(dictionaryEntry.Key, dictionaryEntry.Value);
      }
      for (int index = 0; index < stringList1.Count; ++index)
      {
        if (!this.dirtyTbl.ContainsKey((object) stringList1[index]))
          this.dirtyTbl.Add((object) stringList1[index], LoanData.nobj);
      }
    }

    public int RemoveLiabilityAt(int i)
    {
      this.Dirty = true;
      int exlcudingAlimonyJobExp = this.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      int verifIndex = this.map.RemoveLiabilityAt(i);
      if (verifIndex >= 0)
        this.onVerificationsChanged(VerifType.Mortgage, verifIndex, VerifOperation.Detach);
      else if (verifIndex == -1)
      {
        this.onVerificationsChanged(VerifType.Liability, i, VerifOperation.Remove);
        for (int index = i; index < exlcudingAlimonyJobExp; ++index)
        {
          int num1 = index + 1;
          string mappingID = "FL" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
          int num2;
          string str = "FL" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
          CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
          if (crmMapping != null)
          {
            this.GetLogList().RemoveCRMMapping(crmMapping);
            if (num2 >= i + 1)
            {
              crmMapping.MappingID = str;
              this.GetLogList().AddCRMMapping(crmMapping);
            }
          }
        }
      }
      return verifIndex;
    }

    public int NewLiability()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewLiability();
      this.onVerificationsChanged(VerifType.Liability, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewAdditionalLoan()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewAdditionalLoan();
      this.onVerificationsChanged(VerifType.AdditionalLoan, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public void UpLiability(int i)
    {
      this.Dirty = true;
      this.map.UpLiability(i);
      int originalIndex = i + 1;
      string mappingID1 = "FL" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = "FL" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
      this.updateMultiInstanceFieldsInDirtyTable("FL", originalIndex, originalIndex - 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void UpOtherAsset(int i)
    {
      this.Dirty = true;
      this.map.UpOtherAsset(i);
      int originalIndex = i + 1;
      string mappingID1 = "URLAROA" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = "URLAROA" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
      this.updateMultiInstanceFieldsInDirtyTable("URLAROA", originalIndex, originalIndex - 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void UpDisaster(int i)
    {
      this.Dirty = true;
      this.map.UpDisaster(i);
      int originalIndex = i + 1;
      string mappingID1 = "FEMA" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = "FEMA" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
      this.updateMultiInstanceFieldsInDirtyTable("FEMA", originalIndex, originalIndex - 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 != null)
        logList.AddCRMMapping(crmMapping2);
      if (i != 1)
        return;
      this.TriggerCalculation("FEMA0106", (string) null);
    }

    public void DownOtherAsset(int i)
    {
      this.Dirty = true;
      this.map.DownOtherAsset(i);
      int originalIndex = i + 1;
      string mappingID1 = "URLAROA" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = "URLAROA" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
      this.updateMultiInstanceFieldsInDirtyTable("URLAROA", originalIndex, originalIndex + 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void DownDisaster(int i)
    {
      this.Dirty = true;
      this.map.DownDisaster(i);
      int originalIndex = i + 1;
      string mappingID1 = "FEMA" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = "FEMA" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
      this.updateMultiInstanceFieldsInDirtyTable("FEMA", originalIndex, originalIndex + 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 != null)
        logList.AddCRMMapping(crmMapping2);
      if (i != 0)
        return;
      this.TriggerCalculation("FEMA0106", (string) null);
    }

    public void DownLiability(int i)
    {
      this.Dirty = true;
      this.map.DownLiability(i);
      int originalIndex = i + 1;
      string mappingID1 = "FL" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = "FL" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
      this.updateMultiInstanceFieldsInDirtyTable("FL", originalIndex, originalIndex + 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void UpVerification(string header, int i)
    {
      this.Dirty = true;
      switch (header)
      {
        case "URLARGG":
          this.map.UpVerification("BORROWER/URLA2020/GiftsGrants/GiftGrant", i);
          break;
        case "URLAROIS":
          this.map.UpVerification("BORROWER/URLA2020/OtherIncomeSources/OtherIncomeSource", i);
          break;
        case "URLAROL":
          this.map.UpVerification("BORROWER/URLA2020/OtherLiabilities/OTHER_LIABILITY", i);
          break;
        case "URLARAL":
          this.map.UpVerification("BORROWER/URLA2020/AdditionalLoans/Additional_Loan", i);
          break;
        default:
          return;
      }
      int originalIndex = i + 1;
      string mappingID1 = header + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = header + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
      this.updateMultiInstanceFieldsInDirtyTable(header, originalIndex, originalIndex - 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void DownVerification(string header, int i)
    {
      this.Dirty = true;
      switch (header)
      {
        case "URLARGG":
          this.map.DownVerification("BORROWER/URLA2020/GiftsGrants/GiftGrant", i);
          break;
        case "URLAROIS":
          this.map.DownVerification("BORROWER/URLA2020/OtherIncomeSources/OtherIncomeSource", i);
          break;
        case "URLAROL":
          this.map.DownVerification("BORROWER/URLA2020/OtherLiabilities/OTHER_LIABILITY", i);
          break;
        case "URLARAL":
          this.map.DownVerification("BORROWER/URLA2020/AdditionalLoans/Additional_Loan", i);
          break;
        default:
          return;
      }
      int originalIndex = i + 1;
      string mappingID1 = header + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
      string mappingID2 = header + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
      this.updateMultiInstanceFieldsInDirtyTable(header, originalIndex, originalIndex + 1);
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public int GetNumberOfMortgages() => this.map.GetNumberOfMortgages();

    public int RemoveMortgageAt(int i)
    {
      this.Dirty = true;
      if (!this.map.RemoveMortgageAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.Mortgage, i, VerifOperation.Remove);
      return 1;
    }

    public void AttachMortgage(string currentInd, string selectedVOL)
    {
      this.Dirty = true;
      int verifIndex = this.map.AttachMortgage(currentInd, selectedVOL);
      if (verifIndex >= 0)
        this.onVerificationsChanged(VerifType.Mortgage, verifIndex, VerifOperation.Attach);
      this.dirtyTbl[(object) ("FM" + currentInd + "17")] = LoanData.nobj;
      this.dirtyTbl[(object) ("FM" + currentInd + "16")] = LoanData.nobj;
    }

    public int NewMortgage(string selectedVOL)
    {
      this.Dirty = true;
      int verifIndex = this.map.NewMortgage(selectedVOL);
      this.onVerificationsChanged(VerifType.Mortgage, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public void UpMortgage(int i)
    {
      this.Dirty = true;
      this.map.UpMortgage(i);
      this.updateMultiInstanceFieldsInDirtyTable("FM", i + 1, i);
    }

    public void MoveMortgageToTop(int i)
    {
      this.Dirty = true;
      this.map.MoveMortgageToTop(i);
      this.updateMultiInstanceFieldsInDirtyTable("FM", i, 1);
    }

    public void DownMortgage(int i)
    {
      this.Dirty = true;
      this.map.DownMortgage(i);
      this.updateMultiInstanceFieldsInDirtyTable("FM", i + 1, i + 2);
    }

    public int GetNumberOfDeposits() => this.map.GetNumberOfDeposits();

    public int GetNumberOfGiftsAndGrants() => this.map.GetNumberOfGiftsAndGrants();

    public int GetNumberOfOtherIncomeSources() => this.map.GetNumberOfOtherIncomeSources();

    public int GetNumberOfDisasters() => this.map.GetNumberOfDisasters();

    public int GetNumberOfAdditionalVestingParties()
    {
      return this.map.GetNumberOfAdditionalVestingParties();
    }

    public int GetNumberOfOtherAssets() => this.map.GetNumberOfOtherAssets();

    public VestingPartyFields[] GetVestingPartyFields(bool includeBorrowers)
    {
      List<VestingPartyFields> vestingPartyFieldsList = new List<VestingPartyFields>();
      if (includeBorrowers)
      {
        BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
        if (borrowerPairs != null)
        {
          for (int index = 0; index < borrowerPairs.Length; ++index)
          {
            VestingPartyFields borrowerFields = VestingPartyFields.GetBorrowerFields(borrowerPairs[index]);
            string simpleField1 = this.GetSimpleField(borrowerFields.NameField, borrowerFields.BorrowerPair);
            string simpleField2 = this.GetSimpleField(borrowerFields.TypeField, borrowerFields.BorrowerPair);
            if (simpleField1 == "")
            {
              string val = Utils.JoinName(this.GetSimpleField("36", borrowerPairs[index]), this.GetSimpleField("37", borrowerPairs[index])).Trim();
              if (!(val == ""))
                this.SetCurrentField(borrowerFields.NameField, val, borrowerFields.BorrowerPair);
              else
                continue;
            }
            if (simpleField2 == "")
            {
              string val = this.GetSimpleField("4008", borrowerPairs[index]);
              if (val == "")
                val = "Individual";
              this.SetCurrentField(borrowerFields.TypeField, val, borrowerFields.BorrowerPair);
            }
            vestingPartyFieldsList.Add(borrowerFields);
            VestingPartyFields coBorrowerFields = VestingPartyFields.GetCoBorrowerFields(borrowerPairs[index]);
            string simpleField3 = this.GetSimpleField(coBorrowerFields.NameField, coBorrowerFields.BorrowerPair);
            string simpleField4 = this.GetSimpleField(coBorrowerFields.TypeField, coBorrowerFields.BorrowerPair);
            if (simpleField3 == "")
            {
              string val = Utils.JoinName(this.GetSimpleField("68", borrowerPairs[index]), this.GetSimpleField("69", borrowerPairs[index])).Trim();
              if (!(val == ""))
                this.SetCurrentField(coBorrowerFields.NameField, val, coBorrowerFields.BorrowerPair);
              else
                continue;
            }
            if (simpleField4 == "")
            {
              string val = this.GetSimpleField("4009", borrowerPairs[index]);
              if (val == "")
                val = "Individual";
              this.SetCurrentField(coBorrowerFields.TypeField, val, coBorrowerFields.BorrowerPair);
            }
            vestingPartyFieldsList.Add(coBorrowerFields);
          }
        }
      }
      int additionalVestingParties = this.GetNumberOfAdditionalVestingParties();
      for (int trusteeIndex = 1; trusteeIndex <= additionalVestingParties; ++trusteeIndex)
        vestingPartyFieldsList.Add(VestingPartyFields.GetAdditionalVestingPartyInstanceFields(trusteeIndex));
      return vestingPartyFieldsList.ToArray();
    }

    public int RemoveDepositAt(int i)
    {
      this.Dirty = true;
      int numberOfDeposits = this.GetNumberOfDeposits();
      if (!this.map.RemoveDepositAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.Deposit, i, VerifOperation.Remove);
      for (int index = i; index < numberOfDeposits; ++index)
      {
        int num1 = index + 1;
        string mappingID = "DD" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "DD" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int RemoveGiftGrantAt(int i)
    {
      this.Dirty = true;
      int ofGiftsAndGrants = this.GetNumberOfGiftsAndGrants();
      if (!this.map.RemoveGiftGrantAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.GiftGrant, i, VerifOperation.Remove);
      for (int index = i; index < ofGiftsAndGrants; ++index)
      {
        int num1 = index + 1;
        string mappingID = "URLARGG" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "URLARGG" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int RemoveDisasterAt(int i)
    {
      this.Dirty = true;
      int numberOfDisasters = this.GetNumberOfDisasters();
      if (!this.map.RemoveDisasterAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.Disaster, i, VerifOperation.Remove);
      for (int index = i; index < numberOfDisasters; ++index)
      {
        int num1 = index + 1;
        string mappingID = "FEMA" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "FEMA" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int RemoveOtherAssetAt(int i)
    {
      this.Dirty = true;
      int numberOfOtherAssets = this.GetNumberOfOtherAssets();
      if (!this.map.RemoveOtherAssetAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.OtherAsset, i, VerifOperation.Remove);
      for (int index = i; index < numberOfOtherAssets; ++index)
      {
        int num1 = index + 1;
        string mappingID = "URLAROA" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "URLAROA" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int RemoveOtherIncomeSourceAt(int i)
    {
      this.Dirty = true;
      int otherIncomeSources = this.GetNumberOfOtherIncomeSources();
      if (!this.map.RemoveOtherIncomeSourceAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.OtherIncomeSource, i, VerifOperation.Remove);
      for (int index = i; index < otherIncomeSources; ++index)
      {
        int num1 = index + 1;
        string mappingID = "URLAROIS" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "URLAROIS" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int RemoveOtherLiabilityAt(int i)
    {
      this.Dirty = true;
      int ofOtherLiability = this.GetNumberOfOtherLiability();
      if (!this.map.RemoveOtherLiabilityAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.OtherLiability, i, VerifOperation.Remove);
      for (int index = i; index < ofOtherLiability; ++index)
      {
        int num1 = index + 1;
        string mappingID = "URLAROL" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "URLAROL" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int RemoveAdditionalLoanAt(int i)
    {
      this.Dirty = true;
      int ofAdditionalLoans = this.GetNumberOfAdditionalLoans();
      if (!this.map.RemoveAdditionalLoanAt(i))
        return -2;
      this.onVerificationsChanged(VerifType.AdditionalLoan, i, VerifOperation.Remove);
      for (int index = i; index < ofAdditionalLoans; ++index)
      {
        int num1 = index + 1;
        string mappingID = "URLARAL" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
        int num2;
        string str = "URLARAL" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
      return 1;
    }

    public int NewDeposit()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewDeposit();
      this.onVerificationsChanged(VerifType.Deposit, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewGiftGrant()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewGiftGrant();
      this.onVerificationsChanged(VerifType.GiftGrant, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewDisaster()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewDisaster();
      this.onVerificationsChanged(VerifType.Disaster, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewOtherIncomeSource()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewOtherIncomeSource();
      this.onVerificationsChanged(VerifType.OtherIncomeSource, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewOtherAsset()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewOtherAsset();
      this.onVerificationsChanged(VerifType.OtherAsset, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewOtherLiability()
    {
      this.Dirty = true;
      int verifIndex = this.map.NewOtherLiability();
      this.onVerificationsChanged(VerifType.OtherLiability, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int NewGSERepWarrantTracker()
    {
      this.Dirty = true;
      return this.map.NewGSERepWarrantTracker();
    }

    public int GetNumberOfResidence(bool borrower) => this.map.GetNumberOfResidence(borrower);

    public void RemoveResidenceAt(bool borrower, int i)
    {
      this.Dirty = true;
      int numberOfResidence = this.GetNumberOfResidence(borrower);
      this.map.RemoveResidenceAt(borrower, i);
      this.onVerificationsChanged(VerifType.Residence, i, VerifOperation.Remove);
      for (int index = i; index < numberOfResidence; ++index)
      {
        int num1 = index + 1;
        string mappingID;
        int num2;
        string str;
        if (borrower)
        {
          mappingID = "BR" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
          str = "BR" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        }
        else
        {
          mappingID = "CR" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
          str = "CR" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        }
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
    }

    public int NewResidence(bool borrower, bool current)
    {
      this.Dirty = true;
      int verifIndex = this.map.NewResidence(borrower, current);
      this.onVerificationsChanged(VerifType.Residence, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int MoveResidence(bool from, bool to, int index)
    {
      this.Dirty = true;
      return this.map.MoveResidence(from, to, index);
    }

    public void UpResidence(bool borrower, bool current, int index)
    {
      this.Dirty = true;
      this.map.UpResidence(borrower, current, index);
      int originalIndex = index + 1;
      string mappingID1;
      string mappingID2;
      if (borrower)
      {
        mappingID1 = "BR" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "BR" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
        this.updateMultiInstanceFieldsInDirtyTable("BR", originalIndex, originalIndex - 1);
      }
      else
      {
        mappingID1 = "CR" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "CR" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
        this.updateMultiInstanceFieldsInDirtyTable("CR", originalIndex, originalIndex - 1);
      }
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void DownResidence(bool borrower, bool current, int index)
    {
      this.Dirty = true;
      this.map.DownResidence(borrower, current, index);
      int originalIndex = index + 1;
      string mappingID1;
      string mappingID2;
      if (borrower)
      {
        mappingID1 = "BR" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "BR" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
        this.updateMultiInstanceFieldsInDirtyTable("BR", originalIndex, originalIndex + 1);
      }
      else
      {
        mappingID1 = "CR" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "CR" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
        this.updateMultiInstanceFieldsInDirtyTable("CR", originalIndex, originalIndex + 1);
      }
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public int GetNumberOfEmployer(bool borrower) => this.map.GetNumberOfEmployer(borrower);

    public int GetNumberOfPrevEmployer(bool borrower) => this.map.GetNumberOfPrevEmployer(borrower);

    public void RemoveEmployerAt(bool borrower, int i)
    {
      this.Dirty = true;
      int numberOfEmployer = this.GetNumberOfEmployer(borrower);
      this.map.RemoveEmployerAt(borrower, i);
      this.onVerificationsChanged(VerifType.Employer, i, VerifOperation.Remove);
      for (int index = i; index < numberOfEmployer; ++index)
      {
        int num1 = index + 1;
        string mappingID;
        int num2;
        string str;
        if (borrower)
        {
          mappingID = "BE" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
          str = "BE" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        }
        else
        {
          mappingID = "CE" + (num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
          str = "CE" + ((num2 = num1 - 1) < 10 ? "0" + (object) num2 : string.Concat((object) num2));
        }
        CRMLog crmMapping = this.GetLogList().GetCRMMapping(mappingID);
        if (crmMapping != null)
        {
          this.GetLogList().RemoveCRMMapping(crmMapping);
          if (num2 >= i + 1)
          {
            crmMapping.MappingID = str;
            this.GetLogList().AddCRMMapping(crmMapping);
          }
        }
      }
    }

    public int NewEmployer(bool borrower, bool current)
    {
      this.Dirty = true;
      int verifIndex = this.map.NewEmployer(borrower, current);
      this.onVerificationsChanged(VerifType.Employer, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int MoveEmployer(bool from, bool to, int index)
    {
      this.Dirty = true;
      int numberOfEmployer1 = this.GetNumberOfEmployer(true);
      int numberOfEmployer2 = this.GetNumberOfEmployer(false);
      int num = this.map.MoveEmployer(from, to, index);
      int numberOfEmployer3 = this.GetNumberOfEmployer(true);
      int numberOfEmployer4 = this.GetNumberOfEmployer(false);
      if (numberOfEmployer1 != numberOfEmployer3 || numberOfEmployer2 != numberOfEmployer4)
      {
        this.TriggerCalculation("BE0109", this.GetSimpleField("BE0109"));
        this.TriggerCalculation("CE0109", this.GetSimpleField("CE0109"));
      }
      return num;
    }

    public void UpEmployer(bool borrower, bool current, int index)
    {
      this.Dirty = true;
      this.map.UpEmployer(borrower, current, index);
      int originalIndex = index + 1;
      string mappingID1;
      string mappingID2;
      if (borrower)
      {
        mappingID1 = "BE" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "BE" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
        this.updateMultiInstanceFieldsInDirtyTable("BE", originalIndex, originalIndex - 1);
      }
      else
      {
        mappingID1 = "CE" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "CE" + (originalIndex - 1 < 10 ? "0" + (object) (originalIndex - 1) : string.Concat((object) (originalIndex - 1)));
        this.updateMultiInstanceFieldsInDirtyTable("CE", originalIndex, originalIndex - 1);
      }
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void DownEmployer(bool borrower, bool current, int index)
    {
      this.Dirty = true;
      this.map.DownEmployer(borrower, current, index);
      int originalIndex = index + 1;
      string mappingID1;
      string mappingID2;
      if (borrower)
      {
        mappingID1 = "BE" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "BE" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
        this.updateMultiInstanceFieldsInDirtyTable("BE", originalIndex, originalIndex + 1);
      }
      else
      {
        mappingID1 = "CE" + (originalIndex < 10 ? "0" + (object) originalIndex : string.Concat((object) originalIndex));
        mappingID2 = "CE" + (originalIndex + 1 < 10 ? "0" + (object) (originalIndex + 1) : string.Concat((object) (originalIndex + 1)));
        this.updateMultiInstanceFieldsInDirtyTable("CE", originalIndex, originalIndex + 1);
      }
      LogList logList = this.GetLogList();
      CRMLog crmMapping1 = logList.GetCRMMapping(mappingID1);
      CRMLog crmMapping2 = logList.GetCRMMapping(mappingID2);
      if (crmMapping1 != null)
      {
        logList.RemoveCRMMapping(crmMapping1);
        crmMapping1.MappingID = mappingID2;
      }
      if (crmMapping2 != null)
      {
        logList.RemoveCRMMapping(crmMapping2);
        crmMapping2.MappingID = mappingID1;
      }
      if (crmMapping1 != null)
        logList.AddCRMMapping(crmMapping1);
      if (crmMapping2 == null)
        return;
      logList.AddCRMMapping(crmMapping2);
    }

    public void CopyResidence()
    {
      this.Dirty = true;
      string[] strArray = new string[22]
      {
        "FR0212",
        "FR0224",
        "FR0204",
        "FR0206",
        "FR0207",
        "FR0208",
        "FR0229",
        "FR0226",
        "FR0225",
        "FR0227",
        "FR0230",
        "FR0412",
        "FR0424",
        "FR0404",
        "FR0406",
        "FR0407",
        "FR0408",
        "FR0429",
        "FR0426",
        "FR0425",
        "FR0427",
        "FR0430"
      };
      this.map.CopyResidence();
      for (int index = 0; index < strArray.Length; ++index)
      {
        string field = this.GetField(strArray[index]);
        this.dirtyTbl[(object) strArray[index]] = LoanData.nobj;
        this.dirtyTbl2[(object) strArray[index]] = (object) new object[2]
        {
          (object) field,
          (object) this.CurrentBorrowerPair
        };
      }
    }

    public bool RemoveAdditionalVestingPartyAt(int i)
    {
      if (!this.map.RemoveVestingPartyAt(i))
        return false;
      this.Dirty = true;
      this.onVestingChanged();
      return true;
    }

    public int NewAdditionalVestingParty()
    {
      this.Dirty = true;
      return this.map.NewVestingParty();
    }

    public void RemoveDocTrackLink(string linkID) => this.map.RemoveDocTrackLink(linkID);

    public LogList GetLogList() => this.map.GetLogList(this);

    public void ReplaceSystemID(string systemId)
    {
      if (string.IsNullOrEmpty(systemId))
        return;
      this.map.ReplaceSystemID(this, systemId);
    }

    public LogList ResetLogList()
    {
      this.Dirty = true;
      return this.map.ResetLogList(this);
    }

    public void CopyCurrentLoanScenarioToAlternative(int altNo)
    {
      this.Dirty = true;
      string[] strArray = altNo == 1 ? LoanData.alt1Flds : LoanData.alt2Flds;
      for (int index = 0; index < LoanData.loanFlds.Length; ++index)
        this.SetCurrentField(strArray[index], this.GetSimpleField(LoanData.loanFlds[index]));
      this.map.CopyCurrentLoanScenarioToAlternative(altNo);
    }

    public void SwapLoanScenario(int altNo)
    {
      this.Dirty = true;
      string[] strArray = altNo == 1 ? LoanData.alt1Flds : LoanData.alt2Flds;
      for (int index = 0; index < LoanData.loanFlds.Length; ++index)
      {
        string simpleField = this.GetSimpleField(LoanData.loanFlds[index]);
        this.SetCurrentField(LoanData.loanFlds[index], this.GetSimpleField(strArray[index]));
        this.SetCurrentField(strArray[index], simpleField);
      }
      this.map.SwapLoanScenario(altNo);
    }

    public int GetNumberOfOtherLiability() => this.map.GetNumberOfOtherLiabilities();

    public void ClearAlternative(int altNo)
    {
      this.Dirty = true;
      foreach (string id in altNo == 1 ? LoanData.alt1Flds : LoanData.alt2Flds)
        this.SetCurrentField(id, string.Empty);
      this.map.ClearAlternative(altNo);
    }

    public void CopyLoanProgramToLockRequest(LoanProgram lp, bool appendOnly)
    {
      if (lp == null)
        return;
      this.Dirty = true;
      this.IgnoreValidationErrors = true;
      bool flag = lp.IgnoreBusinessRules && this.temporaryIgnoreRule;
      for (int index = 0; index < LoanProgram.LockRequestFieldMap.Length / 2; ++index)
      {
        string val = lp.GetSimpleField(LoanProgram.LockRequestFieldMap[index, 0]) ?? string.Empty;
        if (!(val == string.Empty & appendOnly) && (flag || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) LoanProgram.LockRequestFieldMap[index, 1])))
          this.SetCurrentField(LoanProgram.LockRequestFieldMap[index, 1], val);
      }
      LRAdditionalFields additionalFields = this.Settings.FieldSettings.LockRequestAdditionalFields;
      if (additionalFields != null && additionalFields.GetFields(true) != null)
      {
        foreach (string templateField in LoanProgram.TemplateFields)
        {
          if (additionalFields.IsAdditionalField(templateField, true) && (flag || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) LockRequestCustomField.GenerateCustomFieldID(templateField))))
          {
            string val = lp.GetSimpleField(templateField) ?? string.Empty;
            if (!(val == string.Empty & appendOnly))
              this.SetCurrentField(LockRequestCustomField.GenerateCustomFieldID(templateField), val);
          }
        }
      }
      this.IgnoreValidationErrors = false;
    }

    public void CopyClosingCostToLockRequest(ClosingCost cc, bool appendOnly)
    {
      if (this.Settings.FieldSettings.LockRequestAdditionalFields == null || this.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true) == null || cc == null)
        return;
      this.Dirty = true;
      string empty = string.Empty;
      this.IgnoreValidationErrors = true;
      string[] strArray = cc.RESPAVersion == "2015" ? ClosingCost.AllTemplateFields : ClosingCost.TemplateFields;
      bool flag = cc.IgnoreBusinessRules && this.temporaryIgnoreRule;
      foreach (string str in strArray)
      {
        if (this.Settings.FieldSettings.LockRequestAdditionalFields.IsAdditionalField(str, true))
        {
          string val = cc.GetSimpleField(str) ?? string.Empty;
          if (!(val == string.Empty & appendOnly) && ((this.viewOnlyFields == null ? 1 : (!this.viewOnlyFields.ContainsKey((object) LockRequestCustomField.GenerateCustomFieldID(str)) ? 1 : 0)) | (flag ? 1 : 0)) != 0)
            this.SetCurrentField(LockRequestCustomField.GenerateCustomFieldID(str), val);
        }
      }
      this.IgnoreValidationErrors = false;
    }

    public void SelectLoanProgram(LoanProgram lp, HelocRateTable helocTable, bool appendLP)
    {
      if (lp == null)
        return;
      this.Dirty = true;
      this.IgnoreValidationErrors = true;
      string[] templateFields = LoanProgram.TemplateFields;
      bool flag1 = lp.IgnoreBusinessRules && this.temporaryIgnoreRule;
      string str1 = ",1269,1270,1271,1272,1273,1274,1613,1614,1615,1616,1617,1618,";
      string simpleField = lp.GetSimpleField("1172");
      if (flag1 || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) "1172"))
      {
        if (simpleField == "VA" && this.GetField("1172") != "VA")
        {
          this.SetCurrentField("NEWHUD.X1017", "Y");
          this.SetCurrentField("NEWHUD.X750", "Y");
        }
        if (simpleField != string.Empty || simpleField == "" && !appendLP)
          this.SetCurrentField("1172", simpleField);
      }
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      bool flag5 = false;
      foreach (string str2 in templateFields)
      {
        string val = lp.GetSimpleField(str2);
        string loanFieldId = lp.ToLoanFieldID(str2);
        switch (loanFieldId)
        {
          case "1172":
            continue;
          case "1785":
            if (!(val == "") && val != null)
            {
              int num = val.LastIndexOf("\\");
              if (num > -1)
              {
                val = val.Substring(num + 1);
                if (val.Length > 4 && val.ToUpper().Substring(val.Length - 4) == ".XML")
                {
                  val = val.Substring(0, val.Length - 4);
                  break;
                }
                break;
              }
              break;
            }
            continue;
        }
        if (val == null)
          val = string.Empty;
        if (!(loanFieldId == string.Empty) && ((lp.AlwaysApply(str2) ? 0 : (val == "" ? 1 : 0)) & (appendLP ? 1 : 0)) == 0 && (flag1 || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) loanFieldId)))
        {
          if (loanFieldId == "667" && val == "may")
            val = "May";
          else if (loanFieldId == "1699" && val != string.Empty)
            this.AddLock(loanFieldId);
          if (str1.IndexOf("," + loanFieldId + ",") > -1 && val != string.Empty)
            this.AddLock(loanFieldId);
          if (loanFieldId == "677" && val == "May" && this.Use2015RESPA)
            val = "May_SubjectToConditions";
          if (this.fieldModifiedByTemplate != null)
          {
            if (!this.fieldModifiedByTemplate.ContainsKey((object) str2))
              this.fieldModifiedByTemplate.Add((object) str2, (object) val);
            else
              this.fieldModifiedByTemplate[(object) str2] = (object) val;
          }
          if (!flag2 && (loanFieldId == "4464" || loanFieldId == "4465" || loanFieldId == "4466" || loanFieldId == "4467" || loanFieldId == "4468"))
            flag2 = true;
          else if (!flag3 && (loanFieldId == "4531" || loanFieldId == "4469" || loanFieldId == "4470" || loanFieldId == "4471"))
            flag3 = true;
          if (!flag4 && (loanFieldId == "4475" || loanFieldId == "4476" || loanFieldId == "4477" || loanFieldId == "4478" || loanFieldId == "4479"))
            flag4 = true;
          else if (!flag5 && (loanFieldId == "4530" || loanFieldId == "4480" || loanFieldId == "4481" || loanFieldId == "4482"))
            flag5 = true;
          if (loanFieldId == "4689" && !this.loanSettings.AllowHybridWithENoteClosing && val == "HybridWithENote")
            val = "";
          if (loanFieldId == "4746" && val == "")
          {
            if (this.GetField("4746") == "")
              val = "AmortizingPayment";
            else
              continue;
          }
          if (loanFieldId == "4747" && val == "")
          {
            if (this.GetField("4747") == "")
              val = "Actuarial";
            else
              continue;
          }
          this.SetCurrentField(loanFieldId, val);
          if (loanFieldId == "675")
          {
            this.SetCurrentField("RE88395.X322", val);
            if (val == "Y")
            {
              this.SetCurrentField("RE88395.X123", "");
              this.SetCurrentField("RE88395.X124", "");
            }
          }
        }
      }
      if (Utils.ParseInt((object) this.GetField("1177")) > 0)
        this.SetCurrentField("2982", "Y");
      else
        this.SetCurrentField("2982", "N");
      if (this.Calculator != null)
      {
        this.Calculator.FormCalculation("CLEARBORROWERBUYDOWN");
        this.Calculator.FormCalculation("4985");
      }
      this.setMIP((FieldDataTemplate) lp, appendLP);
      if (helocTable != null)
        this.SetDrawRepayPeriod(helocTable);
      this.vaLoanValidation = this.validateVALoan();
      this.calculator.CopyHUD2010ToGFE2010("NEWHUD.X645", false);
      this.calculator.CopyGFEToMLDS((string) null);
      this.calculator.FormCalculation("URLA.X133");
      this.calculator.ApplyLoanTypeOtherField();
      this.calculator.FormCalculation("KBYO.XD3");
      this.calculator.FormCalculation("USDA", (string) null, (string) null);
      this.calculator.FormCalculation("CALCAUTOMATEDDISCLOSURES");
      if (lp.GetField("1959") != "" && lp.GetField("666") == "")
        this.calculator.FormCalculation("1959");
      if (appendLP)
      {
        if (flag3)
          this.calculator.FormCalculation("4531");
        else if (flag2 & flag3 | flag2)
          this.calculator.FormCalculation("4468");
        if (flag5)
          this.calculator.FormCalculation("4530");
        else if (flag4 & flag5 | flag4)
          this.calculator.FormCalculation("4479");
      }
      this.IgnoreValidationErrors = false;
      this.updateEscrowFeeForVALoan();
    }

    private void setMIP(FieldDataTemplate template, bool appendData)
    {
      if (template.GetSimpleField("3531") == "Y" || template.GetSimpleField("3532") == "Y")
        this.SetCurrentField("3533", "");
      else if (template.GetSimpleField("3533") == "Y")
      {
        this.SetCurrentField("3531", "");
        this.SetCurrentField("3532", "");
      }
      switch (template)
      {
        case ClosingCost _:
        case LoanProgram _:
          if (appendData)
            break;
          if (template.GetSimpleField("1199") == "" || template.GetSimpleField("1198") == "")
          {
            this.SetCurrentField("232", "");
            this.SetCurrentField("1766", "");
          }
          if (!(template.GetSimpleField("1200") == "") && !(template.GetSimpleField("1201") == ""))
            break;
          this.SetCurrentField("1770", "");
          break;
      }
    }

    private void updateEscrowFeeForVALoan()
    {
      if (!this.Use2010RESPA && !this.Use2015RESPA || !(this.GetField("1172") == "VA") || !(this.GetField("NEWHUD.X234") == "Y") || Utils.ParseDouble((object) this.GetField("NEWHUD.X858")) <= Utils.ParseDouble((object) this.GetField("NEWHUD.X808")))
        return;
      this.SetCurrentField("NEWHUD.X858", this.GetField("NEWHUD.X808"));
    }

    public void SelectLoanProgram(LoanProgram lp, HelocRateTable helocTable)
    {
      this.SelectLoanProgram(lp, helocTable, false);
    }

    public void SelectClosingCostProgram(ClosingCost cc)
    {
      this.SelectClosingCostProgram(cc, false);
    }

    public void SelectClosingCostProgram(ClosingCost cc, bool appendCC)
    {
      if (cc == null)
        return;
      this.Dirty = true;
      if (cc.RESPAVersion == "2015")
        cc.SetField("REGZGFE.X8", "");
      this.initFileContacts((FormDataBase) cc);
      string empty = string.Empty;
      this.IgnoreValidationErrors = true;
      string[] source = cc.RESPAVersion == "2015" ? ClosingCost.AllTemplateFields : ClosingCost.TemplateFields;
      if (cc.RESPAVersion == "2015")
        source = ((IEnumerable<string>) source).Where<string>((Func<string, bool>) (w => !((IEnumerable<string>) ClosingCost.GFEUnderWriting).Contains<string>(w))).ToArray<string>();
      bool flag1 = cc.IgnoreBusinessRules && this.temporaryIgnoreRule;
      bool flag2 = true;
      if (Utils.ParseDouble((object) cc.GetSimpleField("NEWHUD.X645")) != 0.0)
      {
        for (int index = 808; index <= 818; ++index)
        {
          if (cc.GetSimpleField("NEWHUD.X" + (object) index) != string.Empty)
          {
            flag2 = false;
            break;
          }
        }
        if (flag2)
          cc.SetField("NEWHUD.X808", cc.GetSimpleField("NEWHUD.X645"));
      }
      bool flag3 = false;
      bool flag4 = this.GetField("1172") == "FarmersHomeAdministration";
      foreach (string str in source)
      {
        if (!(cc.RESPAVersion == "2009") || str.StartsWith("NEWHUD.X") || str.StartsWith("PTC.X") || str.StartsWith("POPT.X") || ClosingCost.SharedFields.Contains(str))
        {
          switch (str)
          {
            case "RecalculationRequired":
              continue;
            case "NEWHUD.X1139":
              if (cc.RESPAVersion == "2015" || this.Use2015RESPA)
                continue;
              break;
          }
          if (!((IEnumerable<string>) ClosingCost.GFETitleFee).Contains<string>(str) || !this.Use2015RESPA && !this.Use2010RESPA)
          {
            string val = cc.GetSimpleField(str) ?? string.Empty;
            if (((val == string.Empty ? 1 : (val == "//" ? 1 : 0)) & (appendCC ? 1 : 0)) == 0 && ((this.viewOnlyFields == null ? 1 : (!this.viewOnlyFields.ContainsKey((object) str) ? 1 : 0)) | (flag1 ? 1 : 0)) != 0)
            {
              if (cc.RESPAVersion != "2009")
              {
                if (val != string.Empty && cc.IsLocked(str) && !this.IsLocked(str))
                  this.AddLock(str);
                else if (!cc.IsLocked(str) && this.IsLocked(str))
                  this.RemoveLock(str);
              }
              if (str == "1804")
                this.SetCurrentField("1785", val);
              else if (!(str == "1205" & flag4))
              {
                if (str == "1107" & flag4)
                {
                  this.SetCurrentField("3560", val);
                  flag3 = true;
                }
                else
                {
                  if (this.fieldModifiedByTemplate != null)
                  {
                    if (!this.fieldModifiedByTemplate.ContainsKey((object) str))
                      this.fieldModifiedByTemplate.Add((object) str, (object) val);
                    else
                      this.fieldModifiedByTemplate[(object) str] = (object) val;
                  }
                  this.SetCurrentField(str, val);
                }
              }
              else
                continue;
              if (str == "NEWHUD.X808")
                this.SetField("ESCROW_TABLE", "");
              if (str == "231" && ((this.viewOnlyFields == null ? 1 : (!this.viewOnlyFields.ContainsKey((object) "1405") ? 1 : 0)) | (flag1 ? 1 : 0)) != 0)
                this.SetCurrentField("1405", val);
              if (str == "NEWHUD2.X4413" || str == "NEWHUD2.X4414" || str == "NEWHUD2.X4415" || str == "NEWHUD2.X4416" || str == "NEWHUD2.X4435" || str == "NEWHUD2.X4417" || str == "NEWHUD2.X4418" || str == "NEWHUD2.X4436" || str == "NEWHUD2.X4419" || str == "NEWHUD2.X4420" || str == "NEWHUD2.X4437" || str == "NEWHUD2.X4421" || str == "NEWHUD2.X4422" || str == "NEWHUD2.X4438" || str == "NEWHUD2.X4423" || str == "NEWHUD2.X4424" || str == "NEWHUD2.X4439" || str == "NEWHUD2.X4425" || str == "NEWHUD2.X4426" || str == "NEWHUD2.X4440")
                this.Calculator.FormCalculation("CALCFEEDETAILINDICATORS", str, val);
            }
          }
        }
      }
      if (flag4)
      {
        this.SetCurrentField("1775", "Y");
        this.SetCurrentField("1757", "Loan Amount");
      }
      if (this.Use2015RESPA)
        this.UpdateLenderObligatedFeeIndicators();
      this.setMIP((FieldDataTemplate) cc, appendCC);
      this.vaLoanValidation = this.validateVALoan();
      if (cc.GetSimpleField("1107") != string.Empty)
        this.Calculator.FormCalculation("MIP", "1107", cc.GetSimpleField("1107"));
      else if (!appendCC)
        this.Calculator.FormCalculation("MIP", (string) null, (string) null);
      this.recalculateLine1200();
      if (this.Use2010RESPA || this.Use2015RESPA)
      {
        this.updateEscrowFeeForVALoan();
        if (flag3)
          this.calculator.FormCalculation("USDAMIP", (string) null, (string) null);
        this.calculator.FormCalculation("MIP", (string) null, (string) null);
        if (this.Use2015RESPA)
          this.calculator.FormCalculation("VERIFYINGPOCPTCFIELDS", "TemplateApplied", (string) null);
        this.calculator.CopyHUD2010ToGFE2010((string) null, false);
        this.calculator.CopyGFEToMLDS((string) null);
        this.calculator.FormCalculation("NEWHUD2.X4768");
      }
      this.IgnoreValidationErrors = false;
    }

    private void initFileContacts(FormDataBase template)
    {
      this.initContactDetail(template, "617", ClosingCost.GFEAppraisal, true);
      this.initContactDetail(template, "624", ClosingCost.GFECredit, true);
      this.initContactDetail(template, "REGZGFE.X8", ClosingCost.GFEUnderWriting, true);
      this.initContactDetail(template, "L248", ClosingCost.GFEMortIns, true);
      this.initContactDetail(template, "L252", ClosingCost.GFEHazIns, true);
      this.initContactDetail(template, "1500", ClosingCost.GFEFloodIns, true);
      this.initContactDetail(template, "395", ClosingCost.GFEDocFee, true);
      this.initContactDetail(template, "56", ClosingCost.GFEAttorneyFee, true);
      if (!this.Use2010RESPA && !this.Use2015RESPA)
      {
        this.initContactDetail(template, "610", ClosingCost.GFEEscrowFee, true);
        this.initContactDetail(template, "411", ClosingCost.GFETitleFee, true);
      }
      if (!(template is DataTemplate))
        return;
      this.initContactDetail(template, "VEND.X122", ClosingCost.GFESellAttorney, false);
      this.initContactDetail(template, "VEND.X133", ClosingCost.GFEBuyerAgent, false);
      this.initContactDetail(template, "VEND.X144", ClosingCost.GFESellerAgent, false);
      this.initContactDetail(template, "638", ClosingCost.GFESeller, false);
      this.initContactDetail(template, "713", ClosingCost.GFEBuilder, false);
      this.initContactDetail(template, "VEND.X34", ClosingCost.GFESurveyor, false);
      this.initContactDetail(template, "VEND.X178", ClosingCost.GFEServicing, false);
      this.initContactDetail(template, "VEND.X200", ClosingCost.GFEWarehouse, false);
      this.initContactDetail(template, "VEND.X44", ClosingCost.GFEPlanner, false);
      this.initContactDetail(template, "VEND.X263", ClosingCost.GFEInvestor, false);
      this.initContactDetail(template, "VEND.X278", ClosingCost.GFEAssignTo, false);
      this.initContactDetail(template, "VEND.X293", ClosingCost.GFEBroker, false);
      this.initContactDetail(template, "VEND.X310", ClosingCost.GFEPreparedBy, false);
      this.initContactDetail(template, "VEND.X54", ClosingCost.GFECustom1, false);
      this.initContactDetail(template, "VEND.X64", ClosingCost.GFECustom2, false);
      this.initContactDetail(template, "VEND.X74", ClosingCost.GFECustom3, false);
      this.initContactDetail(template, "VEND.X1", ClosingCost.GFECustom4, false);
    }

    private void initContactDetail(
      FormDataBase template,
      string checkID,
      string[] ids,
      bool removedDetail)
    {
      if ((template.GetSimpleField(checkID) ?? "") != "")
      {
        for (int index = 1; index < ids.Length; ++index)
        {
          if (this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) ids[index]))
            this.SetCurrentField(ids[index], "");
        }
      }
      else
      {
        if (!removedDetail || !(template is ClosingCost))
          return;
        for (int index = 1; index < ids.Length; ++index)
        {
          if (this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) ids[index]))
            template.SetField(ids[index], "");
        }
      }
    }

    private string emptyToZero(string s) => s == string.Empty ? "0.00" : s;

    private double stringToFloat(string str)
    {
      double num;
      if (str != string.Empty)
      {
        try
        {
          num = (double) float.Parse(str);
        }
        catch (Exception ex)
        {
          num = 0.0;
        }
      }
      else
        num = 0.0;
      return num;
    }

    public void CleanUpMilestoneDates() => this.GetLogList().CleanUpMilestoneDates();

    private string getRaceString(int startId, int endId)
    {
      string empty = string.Empty;
      bool flag = false;
      int index = 0;
      int num = startId;
      while (index < 7)
      {
        if (this.GetField(num.ToString()) == "Y")
        {
          flag = true;
          empty += LoanData.races[index];
        }
        ++index;
        ++num;
      }
      return !flag ? string.Empty : empty.Substring(0, empty.Length - 2);
    }

    public DisclosureTrackingLog.DisclosureTrackingType GetRequiredDisclosures()
    {
      LoanChannel loanChannel = (LoanChannel) new LoanChannelNameProvider().GetValue(this.GetField("2626"));
      bool flag = this.GetField("3136") == "Y";
      switch (loanChannel)
      {
        case LoanChannel.BankedWholesale:
          return !flag ? DisclosureTrackingLog.DisclosureTrackingType.All : DisclosureTrackingLog.DisclosureTrackingType.TIL;
        case LoanChannel.Brokered:
          return DisclosureTrackingLog.DisclosureTrackingType.GFE;
        default:
          return DisclosureTrackingLog.DisclosureTrackingType.All;
      }
    }

    public bool ISLESectionCPopulated() => Utils.ToDouble(this.GetField("LE2.XSTC")) > 0.0;

    public PipelineInfo ToPipelineInfo() => this.ToPipelineInfo(this.alertMonitor);

    public PipelineInfo ToPipelineInfo(IAlertMonitor alertMonitor)
    {
      Hashtable info = new Hashtable();
      ArrayList arrayList1 = new ArrayList();
      BorrowerPair currentBorrowerPair = this.CurrentBorrowerPair;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      this.SetBorrowerPair(borrowerPairs[0]);
      info[(object) "Guid"] = (object) this.GUID;
      info[(object) "LinkGuid"] = (object) this.LinkGUID;
      info[(object) "LoanNumber"] = (object) this.GetField("364");
      info[(object) "LoanVersionNumber"] = (object) this.GetField("LOANFILESEQUENCENUMBER");
      info[(object) "BorrowerLastName"] = (object) this.GetField("37");
      info[(object) "BorrowerFirstName"] = (object) this.GetField("36");
      info[(object) "CoBorrowerFirstName"] = (object) this.GetField("68");
      info[(object) "CoBorrowerLastName"] = (object) this.GetField("69");
      info[(object) "Address1"] = (object) this.GetField("11");
      info[(object) "City"] = (object) this.GetField("12");
      info[(object) "State"] = (object) this.GetField("14");
      info[(object) "Zip"] = (object) this.GetField("15");
      info[(object) "loanOfficerId"] = (object) this.GetField("LOID");
      info[(object) "loanProcessorId"] = (object) this.GetField("LPID");
      info[(object) "LoanCloserId"] = (object) this.GetField("CLID");
      info[(object) "LoanProcessorName"] = (object) this.GetField("362");
      info[(object) "LoanOfficerName"] = (object) this.GetField("317");
      info[(object) "LoanCloserName"] = (object) this.GetField("1855");
      info[(object) "LoanAmount"] = (object) this.GetField("1109");
      info[(object) "TotalLoanAmount"] = (object) this.GetField("2");
      info[(object) "AppraisedValue"] = (object) this.GetField("356");
      info[(object) "EstimatedValue"] = (object) this.GetField("1821");
      info[(object) "DownPayment"] = (object) this.GetField("1335");
      info[(object) "LoanType"] = (object) this.GetField("1172");
      info[(object) "LoanPurpose"] = (object) this.GetField("19");
      info[(object) "LoanRate"] = (object) this.GetField("3");
      info[(object) "SecondMortgage"] = this.GetField("420") == "SecondLien" ? (object) "Y" : (object) "N";
      info[(object) "Term"] = (object) this.GetField("4");
      info[(object) "loanStatus"] = (object) string.Concat((object) (int) LoanStatusMap.GetLoanStatusEnum(this.GetField("1393")));
      info[(object) "loanDocTypeCode"] = (object) LoanDocTypeMap.GetLetterCode(this.GetField("MORNET.X67"));
      info[(object) "LTV"] = (object) this.GetField("353");
      info[(object) "CLTV"] = (object) this.GetField("976");
      info[(object) "OccupancyStatus"] = (object) this.GetField("1811");
      info[(object) "EscrowWaived"] = this.GetField("2293") == "Waived" ? (object) "Y" : (object) "N";
      info[(object) "PropertyType"] = (object) this.GetField("1041");
      info[(object) "DTITop"] = (object) this.GetField("740");
      info[(object) "DTIBottom"] = (object) this.GetField("742");
      info[(object) "TotalMonthlyIncome"] = (object) this.GetField("736");
      info[(object) "InvestorStatus"] = (object) this.GetField("2031");
      info[(object) "TradeNumber"] = (object) this.GetField("2032");
      info[(object) "TradeGuid"] = (object) this.GetField("2819");
      info[(object) "Amortization"] = (object) this.GetField("608");
      LoanChannel loanChannel = (LoanChannel) new LoanChannelNameProvider().GetValue(this.GetField("2626"));
      if (loanChannel == LoanChannel.None)
        info[(object) "loanChannel"] = (object) "";
      else
        info[(object) "loanChannel"] = (object) ((int) loanChannel).ToString();
      info[(object) "CorrespondentTradeNumber"] = (object) this.GetField("3915");
      info[(object) "CorrespondentTradeGuid"] = (object) this.GetField("3914");
      try
      {
        info[(object) "InvestorStatusDate"] = (object) Utils.ParseDate((object) this.GetField("2033"), true);
      }
      catch
      {
        info[(object) "InvestorStatusDate"] = (object) null;
      }
      if (this.GetField("608") == "AdjustableRate")
      {
        info[(object) "ARMMargin"] = (object) this.GetField("689");
        info[(object) "ARMLifeCap"] = (object) this.GetField("247");
        info[(object) "ARMFloorRate"] = (object) this.GetField("1699");
        info[(object) "ARMFirstRateAdjCap"] = (object) this.GetField("697");
        try
        {
          info[(object) "ARMAdjustmentDate"] = (object) Utils.ParseDate((object) this.GetField("3054"), true);
        }
        catch
        {
          info[(object) "ARMAdjustmentDate"] = (object) null;
        }
      }
      else
      {
        info[(object) "ARMMargin"] = (object) "0";
        info[(object) "ARMLifeCap"] = (object) "0";
        info[(object) "ARMFloorRate"] = (object) "0";
        info[(object) "ARMFirstRateAdjCap"] = (object) "0";
        info[(object) "ARMAdjustmentDate"] = (object) null;
      }
      try
      {
        info[(object) "CreditScore"] = (object) Utils.ParseInt((object) this.GetField("VASUMM.X23"), true);
      }
      catch
      {
        info[(object) "CreditScore"] = (object) null;
      }
      try
      {
        info[(object) "LoanProgram"] = (object) FileSystemEntry.Parse(this.GetField("1401")).Name;
      }
      catch
      {
        info[(object) "LoanProgram"] = (object) this.GetField("1401");
      }
      try
      {
        info[(object) "DatePurchased"] = (object) Utils.ParseDate((object) this.GetField("2370"), true);
      }
      catch
      {
        info[(object) "DatePurchased"] = (object) null;
      }
      LockRequestLog currentLockRequest1 = this.GetLogList().GetCurrentLockRequest();
      Hashtable hashtable1 = currentLockRequest1 == null ? new Hashtable() : currentLockRequest1.GetLockRequestSnapshot();
      info[(object) "NetBuyPrice"] = (object) string.Concat(hashtable1[(object) "2203"]);
      info[(object) "NetSellPrice"] = (object) string.Concat(hashtable1[(object) "2274"]);
      info[(object) "TotalBuyPrice"] = (object) string.Concat(hashtable1[(object) "2218"]);
      info[(object) "TotalSellPrice"] = (object) string.Concat(hashtable1[(object) "2295"]);
      info[(object) "NetProfit"] = (object) string.Concat(hashtable1[(object) "2028"]);
      info[(object) "LockDays"] = (object) this.GetField("432");
      string field = this.GetField("762");
      DateTime minValue = DateTime.MinValue;
      try
      {
        minValue = DateTime.Parse(field);
      }
      catch (Exception ex)
      {
      }
      if (minValue == DateTime.MinValue)
        info[(object) "LockExpirationDate"] = (object) null;
      else
        info[(object) "LockExpirationDate"] = (object) minValue.ToString("M/d/yy");
      if (string.Equals(this.GetField("2626"), "Correspondent", StringComparison.InvariantCultureIgnoreCase))
      {
        LockConfirmLog confirmForCurrentLock = this.GetLogList().GetMostRecentConfirmForCurrentLock();
        if (confirmForCurrentLock != null && confirmForCurrentLock.CommitmentTermEnabled)
          info[(object) "MostRecentCommitmentEnabled"] = (object) "1";
        else
          info[(object) "MostRecentCommitmentEnabled"] = (object) "0";
        DateTime date = Utils.ParseDate((object) this.GetField("4529"));
        if (date == DateTime.MinValue)
          info[(object) "CommitmentExpirationDate"] = (object) null;
        else
          info[(object) "CommitmentExpirationDate"] = (object) date.ToString("M/d/yy");
      }
      if (info.Contains((object) "MostRecentCommitmentEnabled") && info[(object) "MostRecentCommitmentEnabled"].ToString() == "1" && info.Contains((object) "CommitmentExpirationDate") && info[(object) "CommitmentExpirationDate"] != null)
        arrayList1.Add((object) new PipelineInfo.Alert(10, "", "expected", Utils.ParseDate((object) this.GetField("4529")), 10.ToString(), (string) null));
      else if (info.Contains((object) "LockExpirationDate") && info[(object) "LockExpirationDate"] != null)
        arrayList1.Add((object) new PipelineInfo.Alert(10, "", "expected", Utils.ParseDate((object) this.GetField("762")), 10.ToString(), (string) null));
      try
      {
        DateTime dateTime = DateTime.Parse(this.GetField("749"));
        info[(object) "ActionTakenDate"] = (object) dateTime.ToString("M/d/yy");
      }
      catch (Exception ex)
      {
        info[(object) "ActionTakenDate"] = (object) null;
      }
      info[(object) "ActionTaken"] = this.GetField("1393") == "" ? (object) "Active Loan" : (object) this.GetField("1393");
      info[(object) "CensusTract"] = (object) this.GetField("700");
      info[(object) "MSA"] = (object) this.GetField("699");
      info[(object) "BorrowerSex"] = (object) this.GetField("471");
      info[(object) "CoBorrowerSex"] = (object) this.GetField("478");
      info[(object) "BorrowerRace"] = (object) this.getRaceString(1524, 1530);
      info[(object) "CoBorrowerRace"] = (object) this.getRaceString(1532, 1538);
      info[(object) "BorrowerEthnicity"] = (object) this.GetField("1523");
      info[(object) "CoBorrowerEthnicity"] = (object) this.GetField("1531");
      info[(object) "RateIsLocked"] = this.GetField("2400") == "Y" ? (object) "Y" : (object) "N";
      info[(object) "LockRequested"] = (object) this.GetField("LOCKRATE.REQUESTED");
      info[(object) "LockRequestPending"] = (object) this.GetField("LOCKRATE.REQUESTPENDING");
      info[(object) "LockExtensionRequestPending"] = (object) this.GetField("LOCKRATE.EXTENSIONREQUESTPENDING");
      info[(object) "LockCancellationRequestPending"] = (object) this.GetField("LOCKRATE.CANCELLATIONREQUESTPENDING");
      info[(object) "LockIsCancelled"] = (object) this.GetField("LOCKRATE.ISCANCELLED");
      info[(object) "RelockRequestPending"] = (object) this.GetField("LOCKRATE.RELOCKREQUESTPENDING");
      if (Utils.ParseDate((object) this.GetField("SERVICE.X13")) != DateTime.MinValue)
      {
        if (this.GetField("SERVICE.X10") != this.GetField("SERVICE.X14"))
        {
          info[(object) "ISStatementDue"] = (object) this.GetField("SERVICE.X13");
          arrayList1.Add((object) new PipelineInfo.Alert(7, "", "expected", Utils.ParseDate((object) this.GetField("SERVICE.X13")), 7.ToString(), (string) null));
        }
      }
      else
        info[(object) "ISStatementDue"] = (object) null;
      if (Utils.ParseDate((object) this.GetField("SERVICE.X14")) != DateTime.MinValue)
        info[(object) "ISPaymentDue"] = (object) this.GetField("SERVICE.X14");
      else
        info[(object) "ISPaymentDue"] = (object) null;
      if (Utils.ParseDate((object) this.GetField("SERVICE.X15")) != DateTime.MinValue)
      {
        info[(object) "ISLatePaymentDate"] = (object) this.GetField("SERVICE.X15");
        arrayList1.Add((object) new PipelineInfo.Alert(6, "", "expected", Utils.ParseDate((object) this.GetField("SERVICE.X15")), 6.ToString(), (string) null));
      }
      else
        info[(object) "ISLatePaymentDate"] = (object) null;
      DateTime date1 = DateTime.MaxValue;
      for (int index = 59; index <= 73; index += 2)
      {
        DateTime date2 = Utils.ParseDate((object) this.GetField("SERVICE.X" + index.ToString()));
        if (date2 != DateTime.MinValue && date2 < date1)
          date1 = date2;
      }
      if (date1 != DateTime.MaxValue)
      {
        info[(object) "ISEscrowDue"] = (object) date1.ToString("MM/dd/yyyy");
        arrayList1.Add((object) new PipelineInfo.Alert(3, "", "expected", date1, 3.ToString(), (string) null));
      }
      else
        info[(object) "ISEscrowDue"] = (object) null;
      info[(object) "ISStatus"] = this.GetField("SERVICE.X8") == "" ? (object) "Not Servicing" : (object) this.GetField("SERVICE.X8");
      if (this.GetField("2827") == "Y")
      {
        DateTime date3 = Utils.ParseDate((object) this.GetField("2824"));
        if (date3 != DateTime.MinValue)
          info[(object) "RegistrationExpiredDate"] = (object) date3.ToString("MM/dd/yyyy");
        else
          info[(object) "RegistrationExpiredDate"] = (object) null;
      }
      else
        info[(object) "RegistrationExpiredDate"] = (object) null;
      LogList logList = this.GetLogList();
      info[(object) "RateLockStatus"] = (object) string.Empty;
      LockRequestLog confirmLockRequest = logList.GetLastNotConfirmLockRequest();
      Hashtable hashtable2 = (Hashtable) null;
      if (confirmLockRequest != null)
      {
        if (string.Compare(confirmLockRequest.RequestedStatus, RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested), true) == 0)
        {
          info[(object) "RateLockStatus"] = (object) RateLockEnum.GetRateLockStatusNum(RateLockRequestStatus.Requested);
          info[(object) "LockRequestDate"] = (object) (confirmLockRequest.Date.ToString("MM/dd/yyyy") + " " + confirmLockRequest.TimeRequested);
        }
        LockRequestLog lastConfirmedLock = logList.GetLastConfirmedLock();
        if (lastConfirmedLock != null)
          hashtable2 = lastConfirmedLock.GetLockRequestSnapshot();
      }
      else
      {
        LockRequestLog currentLockRequest2 = logList.GetCurrentLockRequest();
        if (currentLockRequest2 != null)
        {
          info[(object) "LockRequestDate"] = (object) (currentLockRequest2.Date.ToString("MM/dd/yyyy") + " " + currentLockRequest2.TimeRequested);
          if (info[(object) "LockExpirationDate"] == null)
          {
            info[(object) "RateLockStatus"] = (object) RateLockEnum.GetRateLockStatusNum(RateLockRequestStatus.Requested);
            if (currentLockRequest2.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied))
              info[(object) "RateLockStatus"] = (object) "";
            else if (currentLockRequest2.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
            {
              LockConfirmLog confirmLockLog = logList.GetConfirmLockLog();
              if (confirmLockLog != null && confirmLockLog.RequestGUID == currentLockRequest2.Guid || currentLockRequest2.IsFakeRequest)
                info[(object) "RateLockStatus"] = (object) "";
            }
          }
          else
          {
            LockConfirmLog confirmLockLog = logList.GetConfirmLockLog();
            if (confirmLockLog == null || confirmLockLog.RequestGUID != currentLockRequest2.Guid)
              info[(object) "RateLockStatus"] = (object) RateLockEnum.GetRateLockStatusNum(RateLockRequestStatus.Requested);
          }
          hashtable2 = currentLockRequest2.GetLockRequestSnapshot();
        }
      }
      info[(object) "BuySideCommitted"] = (object) "N";
      info[(object) "SellSideCommitted"] = (object) "N";
      if (hashtable2 != null)
      {
        if (hashtable2.ContainsKey((object) "2592") && hashtable2[(object) "2592"].ToString() != string.Empty)
          info[(object) "BuySideCommitted"] = (object) "Y";
        if (hashtable2.ContainsKey((object) "2286") && hashtable2[(object) "2286"].ToString() != string.Empty)
          info[(object) "SellSideCommitted"] = (object) "Y";
      }
      int numberOfMilestones = logList.GetNumberOfMilestones();
      PipelineInfo.MilestoneInfo[] milestones = new PipelineInfo.MilestoneInfo[numberOfMilestones];
      bool flag = true;
      DateTime dateStarted = DateTime.MinValue;
      DateTime dateTime1;
      for (int index = 0; index < numberOfMilestones; ++index)
      {
        MilestoneLog milestoneAt1 = logList.GetMilestoneAt(index);
        PipelineInfo.Alert[] pipelineAlerts = milestoneAt1.GetPipelineAlerts();
        if (pipelineAlerts != null && pipelineAlerts.Length != 0)
          arrayList1.AddRange((ICollection) pipelineAlerts);
        string milestoneId = milestoneAt1.MilestoneID;
        string stage = milestoneAt1.Stage;
        int roleId = milestoneAt1.RoleID;
        string guid = milestoneAt1.Guid;
        bool done = milestoneAt1.Done;
        bool reviewed = milestoneAt1.Reviewed;
        if (index == 0)
          dateStarted = milestoneAt1.Date;
        DateTime dateCompleted = milestoneAt1.Done ? milestoneAt1.Date : DateTime.MinValue;
        milestones[index] = new PipelineInfo.MilestoneInfo(milestoneId, stage, roleId, guid, done, reviewed, index, dateStarted, dateCompleted);
        dateStarted = dateCompleted;
        if (index != numberOfMilestones - 1)
        {
          MilestoneLog milestoneAt2 = logList.GetMilestoneAt(index + 1);
          if (milestoneAt1.Done && !milestoneAt2.Done)
          {
            flag = false;
            info[(object) "CurrentMilestoneName"] = (object) milestoneAt1.Stage;
            info[(object) "LastMilestoneSorted"] = (object) milestoneAt1.Stage;
            info[(object) "CurrentMilestoneID"] = (object) milestoneAt1.MilestoneID;
            Hashtable hashtable3 = info;
            dateTime1 = milestoneAt1.Date;
            DateTime date4 = dateTime1.Date;
            dateTime1 = DateTime.MaxValue;
            DateTime date5 = dateTime1.Date;
            string empty;
            if (!(date4 == date5))
            {
              dateTime1 = milestoneAt1.Date;
              empty = dateTime1.ToString("MM/dd/yyyy hh:mm tt");
            }
            else
              empty = string.Empty;
            hashtable3[(object) "CurrentMilestoneDate"] = (object) empty;
            info[(object) "NextMilestoneName"] = (object) milestoneAt2.Stage;
            info[(object) "NextMilestoneSorted"] = (object) milestoneAt2.Stage;
            info[(object) "NextMilestoneID"] = (object) milestoneAt2.MilestoneID;
            info[(object) "CurrentCoreMilestoneName"] = (object) milestoneAt1.Stage;
            dateTime1 = milestoneAt2.Date;
            DateTime date6 = dateTime1.Date;
            dateTime1 = DateTime.MaxValue;
            DateTime date7 = dateTime1.Date;
            if (date6 != date7)
            {
              Hashtable hashtable4 = info;
              dateTime1 = milestoneAt2.Date;
              string str = dateTime1.ToString("MM/dd/yyyy hh:mm tt");
              hashtable4[(object) "NextMilestoneDate"] = (object) str;
            }
            logList.GetMilestoneAt(0);
            if (index > 0)
            {
              MilestoneLog milestoneAt3 = logList.GetMilestoneAt(index - 1);
              dateTime1 = milestoneAt3.Date;
              DateTime date8 = dateTime1.Date;
              dateTime1 = DateTime.MaxValue;
              DateTime date9 = dateTime1.Date;
              if (date8 != date9)
              {
                Hashtable hashtable5 = info;
                dateTime1 = milestoneAt3.Date;
                string str = dateTime1.ToString("M/d/yy");
                hashtable5[(object) "PrevMilestoneGroupDate"] = (object) str;
              }
            }
          }
          string empty1 = string.Empty;
          if (milestoneAt1.Done)
          {
            dateTime1 = milestoneAt1.Date;
            DateTime date10 = dateTime1.Date;
            dateTime1 = DateTime.MaxValue;
            DateTime date11 = dateTime1.Date;
            if (date10 != date11)
            {
              dateTime1 = milestoneAt1.Date;
              empty1 = dateTime1.ToString("M/d/yyyy HH:mm:ss");
            }
          }
          if (empty1 != string.Empty)
          {
            if (milestoneAt1.Stage == "Started")
              info[(object) "DateFileOpened"] = (object) empty1;
            else if (milestoneAt1.Stage == "Processing")
              info[(object) "DateSentToProcessing"] = (object) empty1;
            else if (milestoneAt1.Stage == "Submittal")
              info[(object) "DateSubmittedToLender"] = (object) empty1;
            else if (milestoneAt1.Stage == "Approval")
              info[(object) "DateApprovalReceived"] = (object) empty1;
            else if (milestoneAt1.Stage == "Docs Signing")
              info[(object) "DateDocsSigned"] = (object) empty1;
            else if (milestoneAt1.Stage == "Funding")
              info[(object) "DateFunded"] = (object) empty1;
          }
        }
      }
      MilestoneLog milestoneAt = logList.GetMilestoneAt(logList.GetNumberOfMilestones() - 1);
      DateTime date12;
      if (milestoneAt != null)
      {
        DateTime date13 = milestoneAt.Date;
        dateTime1 = milestoneAt.Date;
        date12 = dateTime1.Date;
      }
      else
      {
        dateTime1 = DateTime.MaxValue;
        date12 = dateTime1.Date;
      }
      DateTime dateTime2 = date12;
      DateTime dateTime3 = dateTime2;
      dateTime1 = DateTime.MaxValue;
      DateTime date14 = dateTime1.Date;
      string str1 = dateTime3 == date14 ? string.Empty : dateTime2.ToString("M/d/yy");
      if (str1 != string.Empty)
      {
        if (milestoneAt.Done)
          info[(object) "DateCompleted"] = (object) str1;
        else
          info[(object) "DateOfEstimatedCompletion"] = (object) str1;
      }
      if (flag)
      {
        info[(object) "CurrentMilestoneName"] = (object) "Completion";
        MilestoneLog milestoneByName = logList.GetMilestoneByName("Completion");
        info[(object) "CurrentMilestoneID"] = milestoneByName != null ? (object) milestoneByName.MilestoneID : (object) string.Empty;
        info[(object) "CurrentCoreMilestoneName"] = (object) "Completion";
        info[(object) "NextMilestoneName"] = (object) string.Empty;
        info[(object) "NextMilestoneID"] = (object) null;
        info[(object) "NextCoreMilestoneName"] = (object) string.Empty;
      }
      info[(object) "Lender"] = (object) this.GetField("1264");
      info[(object) "Investor"] = (object) this.GetField("VEND.X263");
      info[(object) "Broker"] = (object) this.GetField("VEND.X293");
      info[(object) "CreditVendor"] = (object) this.GetField("624");
      info[(object) "UnderwriterVendor"] = (object) this.GetField("REGZGFE.X8");
      info[(object) "AppraisalVendor"] = (object) this.GetField("617");
      info[(object) "TitleVendor"] = (object) this.GetField("411");
      info[(object) "EscrowVendor"] = (object) this.GetField("610");
      info[(object) "FloodVendor"] = (object) this.GetField("1500");
      info[(object) "DocPrepVendor"] = (object) this.GetField("395");
      info[(object) "HazardInsuranceVendor"] = (object) this.GetField("L252");
      info[(object) "MortgageInsuranceVendor"] = (object) this.GetField("L248");
      info[(object) "ReferralSource"] = (object) this.GetField("1822");
      info[(object) "LoanSource"] = (object) this.GetField("2024");
      info[(object) "DateCreated"] = (object) this.GetField("2025");
      info[(object) "LoanOfficerProfit"] = (object) this.emptyToZero(this.GetField("PM18"));
      info[(object) "LoanBrokerProfit"] = (object) this.emptyToZero(this.GetField("PM28"));
      info[(object) "LoanProcessorProfit"] = (object) this.emptyToZero(this.GetField("PM22"));
      info[(object) "ManagerProfit"] = (object) this.emptyToZero(this.GetField("PM14"));
      info[(object) "OtherProfit"] = (object) this.emptyToZero(this.GetField("PM26"));
      double num1 = 0.0;
      for (int index = 0; index < LoanData.addIds.Length; ++index)
        num1 += this.stringToFloat(this.GetSimpleField(LoanData.addIds[index]));
      info[(object) "TotalAdditionalCommission"] = (object) num1.ToString("N2");
      double num2 = 0.0;
      double num3 = 0.0;
      for (int index = 0; index < LoanData.typeIds.Length; ++index)
      {
        double num4 = this.stringToFloat(this.GetSimpleField(LoanData.perIds[index]));
        if (num4 != 0.0)
        {
          switch (this.GetField(LoanData.typeIds[index]))
          {
            case "Loan":
              num2 += num4;
              continue;
            case "Profit":
              num3 += num4;
              continue;
            default:
              continue;
          }
        }
      }
      info[(object) "TotalCommissionByLoan"] = (object) num2.ToString("N2");
      info[(object) "TotalCommissionByProfit"] = (object) num3.ToString("N2");
      info[(object) "LOAdditionalCommission"] = (object) this.GetSimpleField("PM17");
      switch (this.GetSimpleField("PM16"))
      {
        case "Loan":
          info[(object) "LOCommissionByLoan"] = (object) this.GetSimpleField("PM15");
          info[(object) "LOCommissionByProfit"] = (object) string.Empty;
          break;
        case "Profit":
          info[(object) "LOCommissionByLoan"] = (object) string.Empty;
          info[(object) "LOCommissionByProfit"] = (object) this.GetSimpleField("PM15");
          break;
      }
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        PipelineInfo.Borrower borrower1 = new PipelineInfo.Borrower();
        borrower1.FirstName = this.GetSimpleField("36", borrowerPairs[index]);
        borrower1.LastName = this.GetSimpleField("37", borrowerPairs[index]);
        borrower1.HomePhone = this.GetSimpleField("66", borrowerPairs[index]);
        borrower1.WorkPhone = this.GetSimpleField("FE0117", borrowerPairs[index]);
        borrower1.CellPhone = this.GetSimpleField("1490", borrowerPairs[index]);
        borrower1.Email = this.GetSimpleField("1240", borrowerPairs[index]);
        borrower1.WorkEmail = this.GetSimpleField("1178", borrowerPairs[index]);
        borrower1.SSN = this.GetSimpleField("65", borrowerPairs[index]);
        borrower1.BorrowerType = LoanBorrowerType.Borrower;
        borrower1.PairIndex = index + 1;
        if (borrower1.SSN != "" || borrower1.LastName != "" || borrower1.FirstName != "" || borrower1.Email != "" || borrower1.WorkEmail != "" || borrower1.HomePhone != "" || borrower1.WorkPhone != "" || borrower1.CellPhone != "")
          arrayList2.Add((object) borrower1);
        PipelineInfo.Borrower borrower2 = new PipelineInfo.Borrower();
        borrower2.FirstName = this.GetSimpleField("68", borrowerPairs[index]);
        borrower2.LastName = this.GetSimpleField("69", borrowerPairs[index]);
        borrower2.HomePhone = this.GetSimpleField("98", borrowerPairs[index]);
        borrower2.WorkPhone = this.GetSimpleField("FE0217", borrowerPairs[index]);
        borrower2.CellPhone = this.GetSimpleField("1480", borrowerPairs[index]);
        borrower2.Email = this.GetSimpleField("1268", borrowerPairs[index]);
        borrower2.WorkEmail = this.GetSimpleField("1179", borrowerPairs[index]);
        borrower2.SSN = this.GetSimpleField("97", borrowerPairs[index]);
        borrower2.BorrowerType = LoanBorrowerType.Coborrower;
        borrower2.PairIndex = index + 1;
        if (borrower2.SSN != "" || borrower2.LastName != "" || borrower2.FirstName != "" || borrower2.Email != "" || borrower2.WorkEmail != "" || borrower2.HomePhone != "" || borrower2.WorkPhone != "" || borrower2.CellPhone != "")
          arrayList2.Add((object) borrower2);
      }
      info[(object) "TPOLOID"] = (object) this.GetField("TPO.X62");
      info[(object) "TPOLPID"] = (object) this.GetField("TPO.X75");
      info[(object) "TPOCompanyID"] = (object) this.GetField("TPO.X15");
      info[(object) "TPOBranchID"] = (object) this.GetField("TPO.X39");
      info[(object) "TPORegisterDate"] = (object) this.GetField("TPO.X3");
      info[(object) "TPOSubmitDate"] = (object) this.GetField("TPO.X4");
      info[(object) "TPOSiteID"] = (object) this.GetField("TPO.X1");
      info[(object) "TPOArchived"] = (object) this.GetField("TPO.X8");
      info[(object) "TPOLOName"] = (object) this.GetField("TPO.X61");
      info[(object) "TPOLPName"] = (object) this.GetField("TPO.X74");
      info[(object) "TPOCompanyName"] = (object) this.GetField("TPO.X14");
      info[(object) "TPOBranchName"] = (object) this.GetField("TPO.X38");
      if (!string.IsNullOrEmpty(this.GetField("TPO.X88")))
        info[(object) "TPOUnderwritingDelegated"] = this.GetField("TPO.X88") == "Y" ? (object) "Y" : (object) "N";
      info[(object) "UnderWriterApprovalDate"] = (object) this.GetField("2301");
      info[(object) "UnderWriterSuspendedDate"] = (object) this.GetField("2303");
      info[(object) "UnderWriterDeniedDate"] = (object) this.GetField("2987");
      info[(object) "UnderWriterDifferentApprovedDate"] = (object) this.GetField("2989");
      info[(object) "WithdrawnDate"] = (object) this.GetField("4120");
      int numberOfRecords = logList.GetNumberOfRecords();
      for (int i = 0; i < numberOfRecords; ++i)
      {
        PipelineInfo.Alert[] pipelineAlerts = logList.GetRecordAt(i).GetPipelineAlerts();
        if (pipelineAlerts != null)
          arrayList1.AddRange((ICollection) pipelineAlerts);
      }
      if (Utils.ParseDouble((object) this.GetField("2629")) != 0.0 && Utils.ParseDouble((object) this.GetField("2211")) != 0.0)
        arrayList1.Add((object) new PipelineInfo.Alert(8, "", "Not Reconciled", DateTime.Today, 8.ToString(), (string) null));
      DateTime date15 = DateTime.MinValue;
      DateTime dateTime4 = DateTime.MinValue;
      try
      {
        date15 = Utils.ParseDate((object) this.GetField("2012"), true);
        info[(object) "InvestorDeliveryDate"] = (object) date15;
      }
      catch
      {
        info[(object) "InvestorDeliveryDate"] = (object) null;
      }
      try
      {
        dateTime4 = Utils.ParseDate((object) this.GetField("2014"), true);
        info[(object) "DateShipped"] = (object) dateTime4;
      }
      catch
      {
        info[(object) "DateShipped"] = (object) null;
      }
      if (date15 != DateTime.MinValue && dateTime4 == DateTime.MinValue)
        arrayList1.Add((object) new PipelineInfo.Alert(12, "Shipping expected", "", date15, 12.ToString(), (string) null));
      PipelineInfo.Alert creditLimitRequired = LoanData.GetCreditLimitRequired(this);
      if (creditLimitRequired != null)
        arrayList1.Add((object) creditLimitRequired);
      PipelineInfo.Alert ifResubordinated = LoanData.GetLienPositionRequiredIfResubordinated(this);
      if (ifResubordinated != null)
        arrayList1.Add((object) ifResubordinated);
      PipelineInfo.Alert ifSubjectProperty = LoanData.GetLienPositionRequiredIfSubjectProperty(this);
      if (ifSubjectProperty != null)
        arrayList1.Add((object) ifSubjectProperty);
      PipelineInfo.Alert alert = LiborTransitionToSofrAlert.GetAlert(this);
      if (alert != null)
        arrayList1.Add((object) alert);
      arrayList1.AddRange((ICollection) RegulationAlerts.GetAlerts(this));
      if (alertMonitor != null)
        arrayList1.AddRange((ICollection) alertMonitor.GetAlerts(this));
      PipelineInfo.Alert[] alerts = new PipelineInfo.Alert[arrayList1.Count];
      arrayList1.CopyTo((Array) alerts);
      arrayList1.Clear();
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      for (int order = 0; order < allMilestones.Length; ++order)
      {
        MilestoneLog milestoneLog = allMilestones[order];
        if (milestoneLog.RoleID >= RoleInfo.FileStarter.ID && (milestoneLog.LoanAssociateID ?? "") != "")
          arrayList1.Add((object) milestoneLog.ToLoanAssociateInfo(milestoneLog.MilestoneID, order));
      }
      foreach (MilestoneFreeRoleLog milestoneFreeRole in logList.GetAllMilestoneFreeRoles())
      {
        if (milestoneFreeRole.RoleID > RoleInfo.FileStarter.ID && (milestoneFreeRole.LoanAssociateID ?? "") != "")
          arrayList1.Add((object) milestoneFreeRole.ToLoanAssociateInfo());
      }
      PipelineInfo.LoanAssociateInfo[] loanAssociates = new PipelineInfo.LoanAssociateInfo[arrayList1.Count];
      arrayList1.CopyTo((Array) loanAssociates);
      this.SetBorrowerPair(currentBorrowerPair);
      this.populateCurrentMileStoneIDtoAlerts(alerts, info[(object) "CurrentMilestoneID"].ToString());
      return new PipelineInfo(info, (PipelineInfo.Borrower[]) arrayList2.ToArray(typeof (PipelineInfo.Borrower)), alerts, loanAssociates, milestones);
    }

    private void populateCurrentMileStoneIDtoAlerts(
      PipelineInfo.Alert[] alerts,
      string CurrentMilestoneID)
    {
      foreach (PipelineInfo.Alert alert in alerts)
        alert.CurrentMileStoneID = CurrentMilestoneID;
    }

    public HelocRateTable GetDrawRepayPeriod() => this.map.GetDrawRepayPeriod();

    public MilestoneTemplate GetDefaultTemplate()
    {
      return this.loanSettings.MigrationData.DefaultTemplate;
    }

    public bool SetDrawRepayPeriod(HelocRateTable helocTable)
    {
      if (!this.map.SetDrawRepayPeriod(helocTable))
        return false;
      this.dirtyTbl[(object) "1889"] = LoanData.nobj;
      this.dirtyTbl2[(object) "1889"] = LoanData.nobj;
      this.dirtyTbl[(object) "1890"] = LoanData.nobj;
      this.dirtyTbl2[(object) "1890"] = LoanData.nobj;
      this.dirty = true;
      return true;
    }

    public string[] GetFormListTemplate() => this.map.GetFormListTemplate();

    public bool SetTaskSetTemplate(
      TaskSetTemplate taskTemplate,
      Hashtable taskIDSetup,
      List<EllieMae.EMLite.Workflow.Milestone> msSetup,
      UserInfo user)
    {
      if (taskTemplate == null)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Info, nameof (LoanData), "SetTaskSetTemplate: TaskSetTemplate is null.");
        return false;
      }
      if (taskTemplate.DocList == null)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Info, nameof (LoanData), "SetTaskSetTemplate: taskTemplate.DocList is null.");
        return false;
      }
      if (msSetup == null)
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "SetTaskSetTemplate: Can't find Log Instance file.");
      LogList logList = this.GetLogList();
      if (logList == null)
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "SetTaskSetTemplate: Can't find Log List.");
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      if (taskIDSetup != null)
      {
        foreach (DictionaryEntry dictionaryEntry in taskIDSetup)
        {
          MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) dictionaryEntry.Value;
          if (!insensitiveHashtable.ContainsKey((object) milestoneTaskDefinition.TaskName))
            insensitiveHashtable.Add((object) milestoneTaskDefinition.TaskName, (object) milestoneTaskDefinition);
        }
      }
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      bool flag = true;
      foreach (MilestoneLog milestoneLog in allMilestones)
      {
        ArrayList tasksByMilestone = taskTemplate.GetTasksByMilestone(milestoneLog.Stage);
        if (tasksByMilestone == null || tasksByMilestone.Count == 0)
        {
          if (!milestoneLog.Done & flag)
            flag = false;
        }
        else
        {
          foreach (string key in tasksByMilestone)
          {
            if (!((key ?? "") == string.Empty) && insensitiveHashtable != null && insensitiveHashtable.ContainsKey((object) key))
            {
              MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) insensitiveHashtable[(object) key];
              logList.AddRecord((LogRecordBase) new MilestoneTaskLog(user, milestoneTaskDefinition.TaskName, milestoneTaskDefinition.TaskDescription)
              {
                TaskGUID = milestoneTaskDefinition.TaskGUID,
                IsRequired = false,
                Stage = milestoneLog.Stage,
                DaysToComplete = (flag ? milestoneTaskDefinition.DaysToComplete : 0),
                DaysToCompleteFromSetting = (flag ? -1 : milestoneTaskDefinition.DaysToComplete),
                TaskPriority = milestoneTaskDefinition.TaskPriority.ToString()
              });
            }
          }
          if (!milestoneLog.Done & flag)
            flag = false;
        }
      }
      return true;
    }

    public bool SetSettlementServiceProviders(SettlementServiceTemplate providerTemplate)
    {
      if (providerTemplate == null)
        return false;
      string[] assignedFieldIds = providerTemplate.GetAssignedFieldIDs();
      if (assignedFieldIds.Length == 0)
        return false;
      this.Dirty = true;
      this.map.ClearSettlementServiceProviders();
      this.IgnoreValidationErrors = true;
      bool ignoreBusinessRules = providerTemplate.IgnoreBusinessRules;
      if (ignoreBusinessRules || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) "SP.ADDITIONALINFO"))
        this.SetCurrentField("SP.ADDITIONALINFO", providerTemplate.AdditionalInfo);
      foreach (string str in assignedFieldIds)
      {
        if (!(str == string.Empty) && (ignoreBusinessRules || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) str)))
        {
          try
          {
            this.SetCurrentField(str, providerTemplate.GetSimpleField(str));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in populating settlement service provider field '" + str + "'. Error: " + ex.Message);
          }
        }
      }
      this.IgnoreValidationErrors = false;
      return true;
    }

    public bool SetAffiliateTemplate(AffiliateTemplate affiliateTemplate)
    {
      if (affiliateTemplate == null)
        return false;
      string[] assignedFieldIds = affiliateTemplate.GetAssignedFieldIDs();
      if (assignedFieldIds.Length == 0)
        return false;
      this.Dirty = true;
      this.map.ClearAffiliates();
      this.IgnoreValidationErrors = true;
      bool ignoreBusinessRules = affiliateTemplate.IgnoreBusinessRules;
      foreach (string str in assignedFieldIds)
      {
        if (!(str == string.Empty) && (ignoreBusinessRules || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) str) && !this.viewOnlyFields.ContainsKey((object) ("AB00" + (str.Length > 6 ? str.Substring(5) : str.Substring(4))))))
        {
          try
          {
            this.SetCurrentField(str, affiliateTemplate.GetSimpleField(str));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in populating affiliate template field '" + str + "'. Error: " + ex.Message);
          }
        }
      }
      this.IgnoreValidationErrors = false;
      return true;
    }

    public bool SetFormListTemplate(FormTemplate formTemplate)
    {
      this.Dirty = true;
      if (formTemplate == null)
        return this.map.SetFormListTemplate((ArrayList) null);
      XmlStringTable existingForms = formTemplate.GetExistingForms();
      ArrayList newList = new ArrayList();
      int num = -1;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      while (true)
      {
        string currentFormID;
        do
        {
          do
          {
            ++num;
            if (existingForms.ContainsKey(num.ToString()))
              currentFormID = (string) existingForms[num.ToString()];
            else
              goto label_11;
          }
          while ((currentFormID ?? "") == string.Empty);
          string str = FormTemplate.RESPAFormNameSwap(currentFormID, this.GetField("3969"));
          if (currentFormID.ToLower() == "gfe - itemization" && this.Use2010RESPA && !newList.Contains((object) "2010 GFE"))
            newList.Add((object) "2010 GFE");
          if (str != null && (!newList.Contains((object) str) || str == "-"))
            newList.Add((object) str);
        }
        while (!(currentFormID.ToLower() == "hud-1 page 2") || !this.Use2010RESPA || newList.Contains((object) "2010 HUD-1 Page 3"));
        newList.Add((object) "2010 HUD-1 Page 3");
      }
label_11:
      if (newList.Count <= 0)
        return false;
      if (newList.Contains((object) "203k Max Mortgage WS"))
      {
        if (!newList.Contains((object) "FHA Management"))
        {
          for (int index = 0; index < newList.Count; ++index)
          {
            if (newList[index].ToString() == "203k Max Mortgage WS")
            {
              newList[index] = (object) "FHA Management";
              break;
            }
          }
        }
        else
          newList.Remove((object) "203k Max Mortgage WS");
      }
      return this.map.SetFormListTemplate(newList);
    }

    public void UpdateLenderObligatedFeeIndicators()
    {
      foreach (int key in Utils.LenderObligatedFee_IndicatorFields.Keys)
        this.UpdateLenderObligatedFeeIndicator(key);
    }

    public void UpdateLenderObligatedFeeIndicator(int feeLineNo)
    {
      string borrowerAmountFieldId = Utils.GetLenderObligatedBorrowerAmountFieldID(feeLineNo);
      string indicatorFieldId = Utils.GetLenderObligatedIndicatorFieldID(feeLineNo);
      if (Utils.ToDouble(this.GetSimpleField(borrowerAmountFieldId)) == 0.0 || !(this.GetSimpleField(indicatorFieldId) != "Y"))
        return;
      this.SetField(indicatorFieldId, "Y");
    }

    public bool SetDataTemplate(DataTemplate dataTemplate)
    {
      return this.setDataTemplate(dataTemplate, false);
    }

    private bool setDataTemplate(DataTemplate dataTemplate, bool fromLoanTemplate)
    {
      if (dataTemplate == null)
        return false;
      this.initFileContacts((FormDataBase) dataTemplate);
      string[] assignedFieldIds = dataTemplate.GetAssignedFieldIDs();
      this.IgnoreValidationErrors = true;
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList();
      List<string[]> fieldsToBeTriggered1 = new List<string[]>();
      Dictionary<string, string> eSignerFields = new Dictionary<string, string>();
      bool flag1;
      List<string[]> strArrayList;
      try
      {
        bool flag2 = dataTemplate.IgnoreBusinessRules && this.temporaryIgnoreRule;
        string simpleField1 = dataTemplate.GetSimpleField("3969");
        string field1 = this.GetField("3969");
        string field2 = this.GetField("1825");
        string simpleField2 = dataTemplate.GetSimpleField("1172");
        flag1 = dataTemplate.GetSimpleField("1969") == "Y" && dataTemplate.GetSimpleField("1969") != this.GetField("1969");
        if (simpleField2 != string.Empty && (flag2 || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) "1172")))
        {
          try
          {
            if (simpleField2 == "VA" && this.GetField("1172") != "VA")
            {
              this.SetCurrentField("NEWHUD.X1017", "Y");
              this.SetCurrentField("NEWHUD.X750", "Y");
            }
            this.SetCurrentField("1172", simpleField2);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Unexpected error while applying template field '1172': " + ex.Message + ". Field will be skipped.");
          }
        }
        bool flag3 = ((IEnumerable<string>) assignedFieldIds).Contains<string>("ULDD.FNM.X43") && !((IEnumerable<string>) assignedFieldIds).Contains<string>("ULDD.X43");
        foreach (string str in assignedFieldIds)
        {
          if (this.loanSettings != null && this.loanSettings.FieldSettings != null)
          {
            FieldDefinition field3 = EncompassFields.GetField(str, this.loanSettings.FieldSettings, false);
            if (field3 != null && field3.Format == FieldFormat.AUDIT)
              continue;
          }
          string val = dataTemplate.GetSimpleField(str);
          if (!(str == "1393") && !(str == "1172") && !(str == "HMDA.X27") && (!(str == "1825") || !(field2 == "2020")) && (!(str == "3164") || !(val == "N")) && (!(str == "3197") && !(str == "3972") && !(str == "3145") || !(val == "//")) && (!(str == "3969") || !(simpleField1 == "") && !(simpleField1 == field1)) && (!(str == "CD3.X87") && !(str == "CD3.X88") && !(str == "CD3.X89") && !(str == "CD3.X90") && !(str == "CD3.X91") && !(str == "CD3.X92") && !(str == "CD3.X93") && !(str == "CD3.X94") && !(str == "CD3.X95") && !(str == "CD3.X96") && !(str == "CD3.X97") && !(str == "CD3.X98") && !(str == "CD3.X99") && !(str == "CD3.X100") && !(str == "CD3.X101") && !(str == "CD3.X87") && !(str == "CD3.XH88") && !(str == "CD3.XH90") && !(str == "CD3.XH93") && !(str == "CD3.XH95") && !(str == "CD3.XH96") && !(str == "CD3.XH97") && !(str == "CD3.XH98") && !(str == "CD3.XH99") && !(str == "CD3.XH100") || dataTemplate.IsLocked(str)))
          {
            if (str.IndexOf("RESPA.X") == 0)
              arrayList1.Add((object) str);
            if (str.IndexOf("FICC.X") == 0)
              arrayList2.Add((object) str);
            if (field2 == "2009" && str == "4796")
              val = "";
            if (str == "4672" || str == "4802" || str == "4806" || str == "4809" || str == "4811" || str == "4814" || str == "4818" || str == "4824" || str == "4682" || str == "4804" || str == "4807" || str == "4810" || str == "4813" || str == "4816" || str == "4821" || str == "4827" || str == "4840" || str == "4831" || str == "4832" || str == "4833" || str == "4834" || str == "4835" || str == "4836" || str == "4837")
              eSignerFields.Add(str, val);
            if (flag2 || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) str))
            {
              try
              {
                if (dataTemplate.IsLocked(str))
                  this.AddLock(str);
                string upper = str.ToUpper();
                // ISSUE: reference to a compiler-generated method
                switch (\u003CPrivateImplementationDetails\u003E.ComputeStringHash(upper))
                {
                  case 46640787:
                    if (upper == "MS.STATUSDATE")
                      goto label_44;
                    else
                      break;
                  case 107658983:
                    if (upper == "MS.CLO")
                      goto label_44;
                    else
                      break;
                  case 1635131273:
                    if (upper == "MS.SUB.DUE")
                      goto label_44;
                    else
                      break;
                  case 1696882373:
                    if (upper == "MS.PROC.DUE")
                      goto label_44;
                    else
                      break;
                  case 1708047876:
                    if (upper == "MS.APP")
                      goto label_44;
                    else
                      break;
                  case 2342700610:
                    if (upper == "MS.FUN.DUE")
                      goto label_44;
                    else
                      break;
                  case 2470469305:
                    if (upper == "MS.PROC")
                      goto label_44;
                    else
                      break;
                  case 3031505671:
                    if (upper == "MS.CLO.DUE")
                      goto label_44;
                    else
                      break;
                  case 3338390999:
                    if (upper == "MS.START")
                      goto label_44;
                    else
                      break;
                  case 3341275769:
                    if (upper == "MS.DOC.DUE")
                      goto label_44;
                    else
                      break;
                  case 3500393629:
                    if (upper == "MS.DOC")
                      goto label_44;
                    else
                      break;
                  case 3714253254:
                    if (upper == "MS.FUN")
                      goto label_44;
                    else
                      break;
                  case 3766966285:
                    if (upper == "MS.SUB")
                      goto label_44;
                    else
                      break;
                  case 3801102188:
                    if (upper == "MS.APP.DUE")
                      goto label_44;
                    else
                      break;
                }
                if (this.fieldModifiedByTemplate != null)
                {
                  if (!this.fieldModifiedByTemplate.ContainsKey((object) str))
                    this.fieldModifiedByTemplate.Add((object) str, (object) val);
                  else
                    this.fieldModifiedByTemplate[(object) str] = (object) val;
                }
                this.SetCurrentField(str, val);
              }
              catch (Exception ex)
              {
                Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Unexpected error while applying template field '" + str + "': " + ex.Message + ". Field will be skipped.");
                continue;
              }
label_44:
              if (flag3 && str == "ULDD.FNM.X43")
                this.SetCurrentField("ULDD.X43", val);
              if (str == "RE88395.X322" && val == "Y")
                this.SetCurrentField("675", val);
              else if (str == "675" && val == "Y")
              {
                this.SetCurrentField("RE88395.X322", val);
                this.SetCurrentField("RE88395.X123", "");
                this.SetCurrentField("RE88395.X124", "");
              }
              else if (str == "URLA.X119" || str == "URLA.X238" || str == "4750" || str == "5006")
                fieldsToBeTriggered1.Add(new string[2]
                {
                  str,
                  val
                });
            }
          }
        }
        List<string[]> fieldsToBeTriggered2 = this.triggereSignerCalcs("4840", "4682", "4672", eSignerFields, fieldsToBeTriggered1);
        List<string[]> fieldsToBeTriggered3 = this.triggereSignerCalcs("4831", "4804", "4802", eSignerFields, fieldsToBeTriggered2);
        List<string[]> fieldsToBeTriggered4 = this.triggereSignerCalcs("4832", "4807", "4806", eSignerFields, fieldsToBeTriggered3);
        List<string[]> fieldsToBeTriggered5 = this.triggereSignerCalcs("4833", "4810", "4809", eSignerFields, fieldsToBeTriggered4);
        List<string[]> fieldsToBeTriggered6 = this.triggereSignerCalcs("4834", "4813", "4811", eSignerFields, fieldsToBeTriggered5);
        List<string[]> fieldsToBeTriggered7 = this.triggereSignerCalcs("4835", "4816", "4814", eSignerFields, fieldsToBeTriggered6);
        List<string[]> fieldsToBeTriggered8 = this.triggereSignerCalcs("4836", "4821", "4818", eSignerFields, fieldsToBeTriggered7);
        strArrayList = this.triggereSignerCalcs("4837", "4827", "4824", eSignerFields, fieldsToBeTriggered8);
        if (simpleField1 != "" && field1 != "" && simpleField1 != field1 && Utils.CheckIf2015RespaTila(simpleField1) && (this.viewOnlyFields == null || this.viewOnlyFields != null && !this.viewOnlyFields.ContainsKey((object) "3969")))
        {
          this.SetCurrentField("NEWHUD.X1139", "Y");
          this.SetCurrentField("NEWHUD.X713", "");
          this.Calculator.FormCalculation("SWITCHTO2015", (string) null, (string) null);
        }
        if (dataTemplate.GetSimpleField("2982") == "N" && this.GetSimpleField("2982") == "N" && (string.IsNullOrEmpty(dataTemplate.GetSimpleField("1177")) || dataTemplate.GetSimpleField("1177") == "0") && !string.IsNullOrEmpty(this.GetSimpleField("1177")) && this.GetSimpleField("1177") != "0")
          this.SetCurrentField("1177", "");
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("L81")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X443")))
          this.Calculator.FormCalculation("L81", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("L86")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X467")))
          this.Calculator.FormCalculation("L86", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("L90")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X491")))
          this.Calculator.FormCalculation("L90", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X24")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X515")))
          this.Calculator.FormCalculation("CD3.X24", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X26")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X539")))
          this.Calculator.FormCalculation("CD3.X26", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("L88")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X223")))
          this.Calculator.FormCalculation("L88", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X2")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X227")))
          this.Calculator.FormCalculation("CD3.X2", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X9")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X285")))
          this.Calculator.FormCalculation("CD3.X9", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X13")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X310")))
          this.Calculator.FormCalculation("CD3.X13", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X15")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X314")))
          this.Calculator.FormCalculation("CD3.X15", (string) null, (string) null);
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X17")) && string.IsNullOrEmpty(dataTemplate.GetSimpleField("CD3.X318")))
          this.Calculator.FormCalculation("CD3.X17", (string) null, (string) null);
        if (dataTemplate.GetField("1107") != string.Empty || dataTemplate.GetField("1109") != string.Empty)
          this.Calculator.FormCalculation("MIP", "1107", dataTemplate.GetField("1107"));
        if (this.GetField("14") == "CA")
        {
          if ((dataTemplate.GetField("390") != string.Empty || dataTemplate.GetField("587") != string.Empty) && dataTemplate.GetField("1636") == string.Empty && this.GetField("1636") != string.Empty)
          {
            this.SetCurrentField("1636", "");
            this.SetCurrentField("2402", "");
            this.SetCurrentField("2403", "");
            this.SetCurrentField("2404", "");
          }
          if ((dataTemplate.GetField("647") != string.Empty || dataTemplate.GetField("593") != string.Empty) && dataTemplate.GetField("1637") == string.Empty && this.GetField("1637") != string.Empty)
          {
            this.SetCurrentField("1637", "");
            this.SetCurrentField("2405", "");
            this.SetCurrentField("2406", "");
          }
          if ((dataTemplate.GetField("648") != string.Empty || dataTemplate.GetField("594") != string.Empty) && dataTemplate.GetField("1638") == string.Empty && this.GetField("1638") != string.Empty)
          {
            this.SetCurrentField("1638", "");
            this.SetCurrentField("2407", "");
            this.SetCurrentField("2408", "");
          }
        }
        if (this.Use2015RESPA)
          this.UpdateLenderObligatedFeeIndicators();
        this.recalculateLine1200();
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in loading data template. Error: " + ex.Message);
        throw new Exception("Unexpected error while applying data template", ex);
      }
      if (arrayList1.Count > 0)
      {
        string[] strArray = new string[3]{ "1", "6", "28" };
        foreach (string str in strArray)
        {
          string id = "RESPA.X" + str.Trim();
          if (!arrayList1.Contains((object) id))
            this.SetCurrentField(id, "");
        }
      }
      if (this.Calculator != null)
      {
        this.Calculator.FormCalculation("CLEARBORROWERBUYDOWN");
        if (strArrayList != null && strArrayList.Count > 0)
        {
          for (int index = 0; index < strArrayList.Count; ++index)
            this.Calculator.FormCalculation(strArrayList[index][0], strArrayList[index][0], strArrayList[index][1]);
        }
      }
      this.setMIP((FieldDataTemplate) dataTemplate, true);
      this.vaLoanValidation = this.validateVALoan();
      if (this.Use2015RESPA)
      {
        if (flag1)
          this.calculator.FormCalculation("1969");
        this.calculator.FormCalculation("VERIFYINGPOCPTCFIELDS", "TemplateApplied", (string) null);
        this.calculator.FormCalculation("REGZGFE_2010", (string) null, (string) null);
      }
      if (this.GetField("14") == "CA")
      {
        if (dataTemplate.GetSimpleField("559") != string.Empty)
          this.SetCurrentField("559", dataTemplate.GetSimpleField("559"));
        if (dataTemplate.GetSimpleField("NEWHUD.X715") != string.Empty && dataTemplate.GetSimpleField("NEWHUD.X1139") != "Y")
        {
          this.SetCurrentField("NEWHUD.X1139", "");
          this.SetCurrentField("NEWHUD.X715", dataTemplate.GetSimpleField("NEWHUD.X715"));
          string[] strArray = new string[5]
          {
            "1847",
            "NEWHUD.X734",
            "1061",
            "436",
            "NEWHUD.X788"
          };
          for (int index = 0; index < strArray.Length; ++index)
          {
            if (dataTemplate.GetSimpleField(strArray[index]) != string.Empty)
              this.SetCurrentField(strArray[index], dataTemplate.GetSimpleField(strArray[index]));
          }
        }
        this.calculator.CopyHUD2010ToGFE2010((string) null, false);
      }
      else
        this.calculator.CopyHUD2010ToGFE2010((string) null, false);
      this.calculator.FormCalculation("KBYO.XD3");
      this.calculator.FormCalculation("19");
      this.calculator.FormCalculation("URLA.X110");
      this.calculator.FormCalculation("FR0108");
      this.Calculator.FormCalculation("CALCAUTOMATEDDISCLOSURES");
      this.IgnoreValidationErrors = false;
      try
      {
        this.copyDescriptionAndAmountCd3(dataTemplate, 139, 222, dataTemplate.GetSimpleField("L84"), dataTemplate.GetSimpleField("L85"));
        this.copyDescriptionAndAmountCd3(dataTemplate, 231, 254, dataTemplate.GetSimpleField("CD3.X4"), dataTemplate.GetSimpleField("CD3.X5"));
        this.copyDescriptionAndAmountCd3(dataTemplate, 279, 284, dataTemplate.GetSimpleField("L134"), dataTemplate.GetSimpleField("L135"));
        this.copyDescriptionAndAmountCd3(dataTemplate, 290, 309, dataTemplate.GetSimpleField("CD3.X11"), dataTemplate.GetSimpleField("CD3.X12"));
        this.copyDescriptionAndAmountCd3(dataTemplate, 322, 337, dataTemplate.GetSimpleField("CD3.X19"), dataTemplate.GetSimpleField("CD3.X20"));
        this.copyDescriptionAndAmountCd3(dataTemplate, 353, 442, dataTemplate.GetSimpleField("L182"), dataTemplate.GetSimpleField("L183"));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in loading data template. Error: " + ex.Message);
        throw new Exception("Unexpected error while applying data template", ex);
      }
      return true;
    }

    public void setTaxTranscriptTemplate(IRS4506TFields template)
    {
      string str = "IR" + this.NewTAX4506T(false).ToString("00");
      foreach (string assignedFieldId in template.GetAssignedFieldIDs())
      {
        string field = template.GetField(assignedFieldId);
        this.SetField(str + assignedFieldId.Substring(4), field);
      }
    }

    private List<string[]> triggereSignerCalcs(
      string customizeIndicator,
      string userId,
      string lenderRepDropdown,
      Dictionary<string, string> eSignerFields,
      List<string[]> fieldsToBeTriggered)
    {
      if (eSignerFields.ContainsKey(customizeIndicator) && eSignerFields[customizeIndicator] == "N" || !eSignerFields.ContainsKey(customizeIndicator))
      {
        if (eSignerFields.ContainsKey(userId))
          fieldsToBeTriggered.Add(new string[2]
          {
            userId,
            eSignerFields[userId]
          });
        else if (eSignerFields.ContainsKey(lenderRepDropdown))
          fieldsToBeTriggered.Add(new string[2]
          {
            lenderRepDropdown,
            eSignerFields[lenderRepDropdown]
          });
      }
      return fieldsToBeTriggered;
    }

    private void copyDescriptionAndAmountCd3(
      DataTemplate dataTemplate,
      int startingFieldID,
      int endingFieldID,
      string copyFieldValue1,
      string copyFieldValue2)
    {
      bool flag = true;
      string str = "CD3.X";
      for (int index = startingFieldID; index <= endingFieldID; ++index)
      {
        if (!string.IsNullOrEmpty(dataTemplate.GetSimpleField(str + index.ToString())))
          flag = false;
      }
      if (!flag)
        return;
      this.SetCurrentField(str + startingFieldID.ToString(), copyFieldValue1);
      this.SetCurrentField(str + (startingFieldID + 1).ToString(), copyFieldValue2);
    }

    private void recalculateLine1200()
    {
      if (this.GetField("1636") != string.Empty)
      {
        this.Calculator.FormCalculation("UPDATECITYSTATEUSERFEES", "1636", this.GetField("1636"));
        this.calculator.CopyHUD2010ToGFE2010("390", false);
      }
      if (this.GetField("1637") != string.Empty)
      {
        this.Calculator.FormCalculation("UPDATECITYSTATEUSERFEES", "1637", this.GetField("1637"));
        this.calculator.CopyHUD2010ToGFE2010("647", false);
      }
      if (this.GetField("1638") != string.Empty)
      {
        this.Calculator.FormCalculation("UPDATECITYSTATEUSERFEES", "1638", this.GetField("1638"));
        this.calculator.CopyHUD2010ToGFE2010("648", false);
      }
      if (this.GetField("373") != string.Empty)
      {
        this.Calculator.FormCalculation("UPDATECITYSTATEUSERFEES", "373", this.GetField("373"));
        this.calculator.CopyHUD2010ToGFE2010("374", false);
      }
      if (this.GetField("1640") != string.Empty)
      {
        this.Calculator.FormCalculation("UPDATECITYSTATEUSERFEES", "1640", this.GetField("1640"));
        this.calculator.CopyHUD2010ToGFE2010("1641", false);
      }
      if (!(this.GetField("1643") != string.Empty))
        return;
      this.Calculator.FormCalculation("UPDATECITYSTATEUSERFEES", "1643", this.GetField("1643"));
      this.calculator.CopyHUD2010ToGFE2010("1644", false);
    }

    public void SetFundingTemplate(FundingTemplate fundingTemplate, bool append)
    {
      if (fundingTemplate == null)
        return;
      this.applyGenericTemplate((FieldDataTemplate) fundingTemplate, append);
    }

    public void SetPurchaseAdviceTemplate(PurchaseAdviceTemplate purchaseTemplate, bool append)
    {
      if (purchaseTemplate == null)
        return;
      this.applyGenericTemplate((FieldDataTemplate) purchaseTemplate, append);
    }

    private void applyGenericTemplate(FieldDataTemplate template, bool append)
    {
      this.Dirty = true;
      this.IgnoreValidationErrors = true;
      if (!append)
      {
        foreach (string allowedFieldId in template.GetAllowedFieldIDs())
          this.SetCurrentField(allowedFieldId, "");
      }
      bool flag = template.IgnoreBusinessRules && this.temporaryIgnoreRule;
      foreach (string str in (FormDataBase) template)
      {
        if (!(str == "DESCRIPTION") && !(str == "NAME") && !(str == string.Empty))
        {
          string val = template.GetSimpleField(str) ?? "";
          if (!(val == "" & append) && (flag || this.viewOnlyFields == null || !this.viewOnlyFields.ContainsKey((object) str)))
          {
            try
            {
              this.SetCurrentField(str, val);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in Setting Purchase Advice template. Field ID: " + str + " Error: " + ex.Message);
            }
          }
        }
      }
      this.IgnoreValidationErrors = false;
    }

    public bool SetLoanTemplate(LoanTemplateSet templateSet)
    {
      return this.SetLoanTemplate(templateSet, false);
    }

    public Hashtable FieldModifiedByTemplate
    {
      get => this.fieldModifiedByTemplate;
      set => this.fieldModifiedByTemplate = value;
    }

    public bool SetLoanTemplate(LoanTemplateSet templateSet, bool appendData)
    {
      this.fieldModifiedByTemplate = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.calculator.SetCalculationsToSkip(true, new string[1]
      {
        "NewHUDCalculation:CalculateProjectedPaymentTable"
      });
      this.SetFormListTemplate(templateSet.FormTemplate);
      this.setDataTemplate(templateSet.DataTemplate, true);
      this.SetSettlementServiceProviders(templateSet.ProviderTemplate);
      this.SetAffiliateTemplate(templateSet.AffiliateTemplate);
      this.SelectLoanProgram(templateSet.LoanProgram, templateSet.HELOCTable, appendData);
      if (templateSet.LoanProgramClosingCost != null)
        this.SelectClosingCostProgram(templateSet.LoanProgramClosingCost, appendData);
      this.SelectClosingCostProgram(templateSet.ClosingCost, appendData);
      if (this.GetField("1172") == "VA")
      {
        this.SetCurrentField("NEWHUD.X1017", "Y");
        this.SetCurrentField("NEWHUD.X750", "Y");
      }
      this.calculator.SetCalculationsToSkip(false, new string[1]
      {
        "NewHUDCalculation:CalculateProjectedPaymentTable"
      });
      this.calculator.CalculateAll();
      return true;
    }

    public void ClearOtherIncomeItems() => this.map.ClearOtherIncomeItems();

    public void AddOtherIncome(string desc, string amount, bool isForBorrower)
    {
      this.map.AddOtherIncome(desc, amount, isForBorrower);
    }

    private void onBorrowerPairChanged()
    {
      if (this.BorrowerPairChanged == null)
        return;
      this.BorrowerPairChanged(this);
    }

    private void onVestingChanged()
    {
      if (this.VestingChanged == null)
        return;
      this.VestingChanged(this);
    }

    private void onBorrowerPairCreated(string pairID)
    {
      if (this.BorrowerPairCreated == null)
        return;
      this.BorrowerPairCreated((object) pairID, new EventArgs());
    }

    private void onLienPositionChanged()
    {
      if (this.LienPositionChanged == null)
        return;
      this.LienPositionChanged(this);
    }

    private void onVerificationsChanged(VerifType type, int verifIndex, VerifOperation op)
    {
      if (this.VerificationsChanged == null)
        return;
      this.VerificationsChanged(this, type, verifIndex, op);
    }

    private static void onInstanceParsed(LoanData data)
    {
      LoanDataEventHandler dataEventHandler = (LoanDataEventHandler) null;
      lock (typeof (LoanData))
      {
        if (LoanData.InstanceParsed != null)
          dataEventHandler = (LoanDataEventHandler) LoanData.InstanceParsed.Clone();
      }
      if (dataEventHandler == null)
        return;
      dataEventHandler(data);
    }

    public void PrefixedCalculations()
    {
      if (this.toDouble(this.map.EMXMLVersionID) >= 3.2)
        return;
      if (this.calculator != null)
      {
        this.calculator.PrefixCalculations("3.1");
        if (this.dirty)
          this.Dirty = false;
      }
      this.map.EMXMLVersionID = "3.2";
    }

    private double toDouble(string val)
    {
      return val != null && !(val == string.Empty) ? double.Parse(val) : 0.0;
    }

    public void FixMilestoneIDs(Hashtable msGuidMapping)
    {
      LogList logList = this.GetLogList();
      int numberOfMilestones = logList.GetNumberOfMilestones();
      for (int i = 0; i < numberOfMilestones; ++i)
      {
        MilestoneLog milestoneAt = logList.GetMilestoneAt(i);
        if (!((milestoneAt.MilestoneID ?? "").Trim() == ""))
          break;
        milestoneAt.FixMilestoneID(msGuidMapping);
      }
    }

    private void innerMap_FieldChanged(object source, MappingFieldChangedEventArgs e)
    {
      try
      {
        if (e.SuppressNotification || this.FieldChanged == null)
          return;
        this.FieldChanged((object) this, (FieldChangedEventArgs) e);
      }
      catch (CountyLimitException ex)
      {
        throw ex;
      }
      catch (GFEDaysToExpireException ex)
      {
        throw ex;
      }
      catch (ComplianceCalendarException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Error in FieldChanged event. Field ID = " + e.FieldID + ", PriorValue = " + e.PriorValue + ", NewValue = " + e.NewValue + ".  Error: " + (object) ex);
      }
    }

    internal void NotifyLogRecordAdded(LogRecordBase record)
    {
      try
      {
        this.Dirty = true;
        if (this.LogRecordAdded != null)
          this.LogRecordAdded((object) this, new LogRecordEventArgs(record));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in NotifyLogRecordAdded: " + (object) ex);
      }
      try
      {
        LogRecordEventHandler recordEventHandler = (LogRecordEventHandler) null;
        switch (record)
        {
          case LockRequestLog _:
            recordEventHandler = this.RateLockRequested;
            break;
          case LockConfirmLog _:
            recordEventHandler = this.RateLockConfirmed;
            break;
          case LockDenialLog _:
            recordEventHandler = this.RateLockDenied;
            break;
          case LockVoidLog _:
            recordEventHandler = this.LockVoided;
            break;
        }
        if (recordEventHandler == null)
          return;
        recordEventHandler((object) this, new LogRecordEventArgs(record));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in custom log event handler: " + (object) ex);
      }
    }

    internal void NotifyLogRecordRemoved(LogRecordBase record)
    {
      try
      {
        this.Dirty = true;
        if (this.LogRecordRemoved == null)
          return;
        this.LogRecordRemoved((object) this, new LogRecordEventArgs(record));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in NotifyLogRecordRemoved: " + (object) ex);
      }
    }

    internal void NotifyLogRecordChanged(LogRecordBase record)
    {
      try
      {
        this.Dirty = true;
        if (this.LogRecordChanged == null)
          return;
        this.LogRecordChanged((object) this, new LogRecordEventArgs(record));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in NotifyLogRecordChanged: " + (object) ex);
      }
    }

    internal bool NotifyBeforeMilestoneCompleted(MilestoneLog ms)
    {
      try
      {
        CancelableMilestoneEventArgs e = new CancelableMilestoneEventArgs(ms);
        if (this.BeforeMilestoneCompleted != null)
          this.BeforeMilestoneCompleted((object) this, e);
        return !e.Cancel;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in NotifyBeforeMilestoneCompleted: " + (object) ex);
        return false;
      }
    }

    public void NotifyLockRequestFieldChanged(string sourcePage)
    {
      try
      {
        LockRequestFieldChangedEventArgs e = new LockRequestFieldChangedEventArgs(sourcePage);
        if (this.LockRequestFieldChanged == null)
          return;
        this.LockRequestFieldChanged((object) this, e);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in NotifyLockRequestFieldChanged: " + (object) ex);
      }
    }

    internal void NotifyMilestoneCompleted(MilestoneLog ms)
    {
      try
      {
        if (this.MilestoneCompleted == null)
          return;
        this.MilestoneCompleted((object) this, new MilestoneEventArgs(ms));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in NotifyMilestoneCompleted: " + (object) ex);
      }
    }

    public ArrayList SyncPiggyBackFiles(
      string[] ids,
      bool runPostSyncOnly,
      bool runAllCalculation,
      string id,
      string val,
      bool isLinkLoan)
    {
      ArrayList arrayList = new ArrayList();
      if (this.linkedData == null)
        return arrayList;
      bool flag1 = this.LinkSyncType == LinkSyncType.ConstructionPrimary || this.LinkSyncType == LinkSyncType.ConstructionLinked;
      if (!runPostSyncOnly)
      {
        BorrowerPair[] borrowerPairs1 = this.GetBorrowerPairs();
        BorrowerPair[] borrowerPairs2 = this.linkedData.GetBorrowerPairs();
        if (borrowerPairs1.Length > borrowerPairs2.Length)
        {
          for (; borrowerPairs1.Length > borrowerPairs2.Length; borrowerPairs2 = this.linkedData.GetBorrowerPairs())
            this.linkedData.CreateBorrowerPair();
        }
        else if (borrowerPairs2.Length > borrowerPairs1.Length)
        {
          for (; borrowerPairs2.Length > borrowerPairs1.Length; borrowerPairs2 = this.linkedData.GetBorrowerPairs())
            this.linkedData.RemoveBorrowerPair(borrowerPairs2[borrowerPairs2.Length - 1]);
        }
        int borrowingOwnerContact1 = this.GetNumberOfNonBorrowingOwnerContact();
        int borrowingOwnerContact2 = this.linkedData.GetNumberOfNonBorrowingOwnerContact();
        if (borrowingOwnerContact2 > borrowingOwnerContact1)
        {
          for (int index = borrowingOwnerContact2; index > 0; --index)
            this.linkedData.RemoveNonBorrowingOwnerContactAt(index - 1);
        }
        int additionalVestingParties1 = this.linkedData.GetNumberOfAdditionalVestingParties();
        int additionalVestingParties2 = this.GetNumberOfAdditionalVestingParties();
        if (additionalVestingParties1 > additionalVestingParties2)
        {
          for (int index = additionalVestingParties1; index > 0; --index)
            this.linkedData.RemoveAdditionalVestingPartyAt(index - 1);
        }
        for (int index1 = 1; index1 <= borrowingOwnerContact1; ++index1)
        {
          for (int index2 = 1; index2 <= 38; ++index2)
          {
            if (index2 != 29)
              this.linkedData.SetCurrentField("NBOC" + index1.ToString("00") + index2.ToString("00"), this.GetSimpleField("NBOC" + index1.ToString("00") + index2.ToString("00")));
          }
          this.linkedData.SetCurrentField("NBOC" + index1.ToString("00") + "98", this.GetSimpleField("NBOC" + index1.ToString("00") + "98"));
          this.linkedData.SetCurrentField("NBOC" + index1.ToString("00") + "99", this.GetSimpleField("NBOC" + index1.ToString("00") + "99"));
        }
        for (int index3 = 1; index3 <= additionalVestingParties2; ++index3)
        {
          for (int index4 = 1; index4 <= 14; ++index4)
            this.linkedData.SetCurrentField("TR" + index3.ToString("00") + index4.ToString("00"), this.GetSimpleField("TR" + index3.ToString("00") + index4.ToString("00")));
          this.linkedData.SetCurrentField("TR" + index3.ToString("00") + "99", this.GetSimpleField("TR" + index3.ToString("00") + "99"));
        }
        this.GetNumberOfAdditionalVestingParties();
        this.linkedData.GetNumberOfAdditionalVestingParties();
        for (int index5 = 0; index5 < ids.Length; ++index5)
        {
          if (!(ids[index5] == "364") && !(ids[index5] == "1051") && !(ids[index5] == "GUID") && !(ids[index5] == "420") && !(ids[index5] == "427") && !(ids[index5] == "428") && (!flag1 || !LoanData.FieldsToBeExcluded.Contains(ids[index5])) && (!(ids[index5] == "1851") || !this.linkedData.IsLocked("1851")))
          {
            switch (ids[index5])
            {
              case "URLARGG":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncURLARGG);
                continue;
              case "URLAROA":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncURLAROA);
                continue;
              case "URLAROIS":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncURLAROIS);
                continue;
              case "URLAROL":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncURLAROL);
                continue;
              case "VOD":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncVOD);
                continue;
              case "VOE":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncVOE);
                continue;
              case "VOLVOM":
                this.syncVOLVOM(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncVOL);
                this.syncVOLVOM(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncVOM);
                continue;
              case "VOR":
                this.syncVerification(borrowerPairs1, borrowerPairs2, SyncPiggybackOperation.SyncVOR);
                continue;
              default:
                try
                {
                  if (!this.linkedData.IsFieldReadOnly(ids[index5]))
                  {
                    for (int index6 = 0; index6 < borrowerPairs1.Length; ++index6)
                    {
                      FieldDefinition field = EncompassFields.GetField(ids[index5], this.loanSettings.FieldSettings);
                      if (field != null)
                      {
                        if (!field.FieldID.StartsWith("TR") && !field.FieldID.StartsWith("NBOC"))
                        {
                          this.syncField(ids[index5], borrowerPairs1[index6], borrowerPairs2[index6], field.Category);
                          if (field.Category == FieldCategory.Common)
                            break;
                        }
                      }
                      else
                        break;
                    }
                    continue;
                  }
                  continue;
                }
                catch (Exception ex)
                {
                  Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "(SyncPiggyBackFiles) ids[j] = " + ids[index5] + "  Error: " + ex.Message);
                  continue;
                }
            }
          }
        }
      }
      List<IDisclosureTracking2015Log> disclosureTracking2015LogList = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) this.GetLogList().GetAllIDisclosureTracking2015Log(false));
      List<IDisclosureTracking2015Log> source = new List<IDisclosureTracking2015Log>((IEnumerable<IDisclosureTracking2015Log>) this.linkedData.GetLogList().GetAllIDisclosureTracking2015Log(false));
      if (source != null && source.Count > 0)
      {
        foreach (IDisclosureTracking2015Log disclosureTracking2015Log1 in disclosureTracking2015LogList)
        {
          IDisclosureTracking2015Log log = disclosureTracking2015Log1;
          IDisclosureTracking2015Log disclosureTracking2015Log2 = source.FirstOrDefault<IDisclosureTracking2015Log>((Func<IDisclosureTracking2015Log, bool>) (x => x.LinkedGuid == log.Guid));
          if (disclosureTracking2015Log2 != null)
          {
            foreach (KeyValuePair<string, INonBorrowerOwnerItem> borrowerOwnerCollection in log.NonBorrowerOwnerCollections)
              disclosureTracking2015Log2.NonBorrowerOwnerCollections[borrowerOwnerCollection.Key] = borrowerOwnerCollection.Value;
          }
        }
      }
      if (runAllCalculation)
      {
        this.Calculator.CalculateAll();
        this.linkedData.Calculator.CalculateAll();
      }
      if (flag1)
        return arrayList;
      if (isLinkLoan && id == "420")
      {
        if (this.linkedData.GetSimpleField("420") == "SecondLien")
        {
          this.SetCurrentField("420", "FirstLien");
          this.SetCurrentField("4494", "1");
        }
        else
        {
          this.SetCurrentField("420", "SecondLien");
          this.SetCurrentField("4494", "2");
        }
      }
      bool flag2 = this.LinkSyncType == LinkSyncType.None || this.LinkSyncType == LinkSyncType.PiggybackPrimary || this.LinkSyncType == LinkSyncType.PiggybackLinked;
      bool loanIsHELOCorOther = this.Calculator != null && this.Calculator.IsPiggybackHELOC;
      if (((flag2 ? 0 : (id == "420" ? 1 : (id == "1172" ? 1 : 0))) & (loanIsHELOCorOther ? 1 : 0)) != 0)
      {
        this.SetField("428", "");
        this.linkedData.SetField("428", "");
      }
      if (this.GetSimpleField("420") == "SecondLien")
      {
        if (!flag2 && !loanIsHELOCorOther)
        {
          if (!isLinkLoan)
            this.linkedData.SetField("1109", this.GetSimpleField("427"));
          else
            this.SetField("1109", this.linkedData.GetSimpleField("428"));
        }
        this.linkedData.SetCurrentField("420", "FirstLien");
        this.linkedData.SetCurrentField("4494", "1");
        this.SetCurrentField("LOANAMT1", this.linkedData.GetSimpleField("1109"));
        this.SetCurrentField("INTRATE1", this.linkedData.GetSimpleField("3"));
        this.SetCurrentField("TERM1", this.linkedData.GetSimpleField("4"));
        this.SetCurrentField("228", this.linkedData.GetSimpleField("5"));
        if (!flag2)
        {
          if (loanIsHELOCorOther)
          {
            this.SetField("427", this.linkedData.GetSimpleField("1109"));
            this.linkedData.SetCurrentField("427", this.linkedData.GetSimpleField("1109"));
          }
          else
          {
            this.linkedData.SetCurrentField("428", this.GetSimpleField("1109"));
            this.SetField("427", this.linkedData.GetSimpleField("1109"));
          }
        }
        this.SetCurrentField("1845", this.linkedData.GetSimpleField("1109"));
        this.CalculateSubordinate(false, this, this.linkedData, loanIsHELOCorOther);
      }
      else
      {
        if (!flag2 && !loanIsHELOCorOther)
        {
          if (!isLinkLoan)
            this.linkedData.SetField("1109", this.GetSimpleField("428"));
          else
            this.SetField("1109", this.linkedData.GetSimpleField("427"));
        }
        if (this.GetSimpleField("420") == "")
        {
          this.SetCurrentField("420", "FirstLien");
          this.SetCurrentField("4494", "1");
        }
        this.linkedData.SetCurrentField("420", "SecondLien");
        this.linkedData.SetCurrentField("4494", "2");
        this.linkedData.SetCurrentField("LOANAMT1", this.GetSimpleField("1109"));
        this.linkedData.SetCurrentField("INTRATE1", this.GetSimpleField("3"));
        this.linkedData.SetCurrentField("TERM1", this.GetSimpleField("4"));
        this.linkedData.SetCurrentField("228", this.GetSimpleField("5"));
        if (!flag2)
        {
          if (loanIsHELOCorOther)
          {
            this.SetField("427", this.linkedData.GetSimpleField("1109"));
            this.linkedData.SetCurrentField("427", this.linkedData.GetSimpleField("1109"));
          }
          else
          {
            this.linkedData.SetCurrentField("427", this.GetSimpleField("1109"));
            this.SetField("428", this.linkedData.GetSimpleField("1109"));
          }
        }
        this.linkedData.SetCurrentField("1845", this.GetSimpleField("1109"));
        this.CalculateSubordinate(true, this, this.linkedData, loanIsHELOCorOther);
      }
      if (!this.linkedData.IsLocked("1851"))
      {
        double num = (Utils.ParseDouble((object) this.GetSimpleField("138")) + Utils.ParseDouble((object) this.GetSimpleField("137")) + Utils.ParseDouble((object) this.GetSimpleField("969")) + Utils.ParseDouble((object) this.GetSimpleField("1093")) - Utils.ParseDouble((object) this.GetSimpleField("1045")) - Utils.ParseDouble((object) this.GetSimpleField("143")) - Utils.ParseDouble((object) this.GetSimpleField("1852"))) * -1.0;
        if (num != 0.0)
          this.linkedData.SetCurrentField("1851", num.ToString("N2"));
        else
          this.linkedData.SetCurrentField("1851", "");
      }
      if (!this.IsLocked("1851"))
      {
        double num = (Utils.ParseDouble((object) this.linkedData.GetSimpleField("138")) + Utils.ParseDouble((object) this.linkedData.GetSimpleField("137")) + Utils.ParseDouble((object) this.linkedData.GetSimpleField("969")) + Utils.ParseDouble((object) this.linkedData.GetSimpleField("1093")) - Utils.ParseDouble((object) this.linkedData.GetSimpleField("1045")) - Utils.ParseDouble((object) this.linkedData.GetSimpleField("143")) - Utils.ParseDouble((object) this.linkedData.GetSimpleField("1852"))) * -1.0;
        if (num != 0.0)
          this.SetCurrentField("1851", num.ToString("N2"));
        else
          this.SetCurrentField("1851", "");
      }
      if (runAllCalculation)
      {
        this.Calculator.CalculateAll();
        this.linkedData.Calculator.CalculateAll();
      }
      else
      {
        this.Calculator.FormCalculation("GFE", id, val);
        this.linkedData.Calculator.FormCalculation("GFE", id, val);
      }
      return arrayList;
    }

    public void CalculateSubordinate(
      bool isFirstLien,
      LoanData loanData,
      LoanData loanLinkedData,
      bool loanIsHELOCorOther)
    {
      if (loanData == null || loanLinkedData == null)
        return;
      if (isFirstLien)
      {
        if (loanIsHELOCorOther)
          return;
        if (!loanLinkedData.IsLocked("140"))
          loanLinkedData.SetCurrentField("140", "");
        double num = loanData.Use2020URLA ? this.toDouble(loanData.GetSimpleField("URLA.X230")) : this.toDouble(loanData.GetSimpleField("428")) + this.toDouble(loanData.GetSimpleField("1732"));
        if (loanData.IsLocked("140"))
          return;
        loanData.SetCurrentField("140", num != 0.0 ? num.ToString("N2") : "");
      }
      else
      {
        if (loanIsHELOCorOther)
          return;
        if (!loanData.IsLocked("140"))
          loanData.SetCurrentField("140", "");
        double num = loanLinkedData.Use2020URLA ? this.toDouble(loanLinkedData.GetSimpleField("URLA.X230")) : this.toDouble(loanLinkedData.GetSimpleField("428")) + this.toDouble(this.linkedData.GetSimpleField("1732"));
        if (loanLinkedData.IsLocked("140"))
          return;
        loanLinkedData.SetCurrentField("140", num != 0.0 ? num.ToString("N2") : "");
      }
    }

    private void syncField(
      string id,
      BorrowerPair pair,
      BorrowerPair linkedPair,
      FieldCategory fieldCategory)
    {
      string simpleField1 = this.linkedData.GetSimpleField(id, linkedPair);
      string simpleField2 = this.GetSimpleField(id, pair);
      if (!(simpleField1 != simpleField2))
        return;
      if (id == "URLA.X195" || id == "URLA.X196")
        this.linkedData.UpdateURLAAlternateNames(id == "URLA.X195", this.GetURLAAlternames(id == "URLA.X195"));
      if (this.IsLocked(id))
        this.linkedData.AddLock(id);
      else if (this.linkedData.IsLocked(id))
        this.linkedData.RemoveLock(id);
      if (fieldCategory == FieldCategory.Common)
        this.linkedData.SetCurrentField(id, simpleField2);
      else
        this.linkedData.SetCurrentField(id, simpleField2, linkedPair);
    }

    private void syncVOLVOM(
      BorrowerPair[] borPairs,
      BorrowerPair[] linkBorPairs,
      SyncPiggybackOperation syncType)
    {
      string xpath = "LIABILITY";
      if (syncType == SyncPiggybackOperation.SyncVOM)
        xpath = "REO_PROPERTY";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      XmlElement refChild = (XmlElement) null;
      XmlNodeList xmlNodeList = this.linkedData.xmldoc.DocumentElement.SelectNodes(xpath);
      try
      {
        if (xmlNodeList != null)
        {
          foreach (XmlElement oldChild in xmlNodeList)
            oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
        }
        xmlNodeList = this.xmldoc.DocumentElement.SelectNodes(xpath);
        if (xmlNodeList == null)
          return;
        if (xmlNodeList.Count == 0)
          return;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "(syncVOLVOM) Trying to remove nodes: " + xpath + " Error: " + ex.Message);
      }
      try
      {
        IEnumerator enumerator = xmlNodeList.GetEnumerator();
        try
        {
label_32:
          while (enumerator.MoveNext())
          {
            XmlNode current = (XmlNode) enumerator.Current;
            string attribute = ((XmlElement) current).GetAttribute("BorrowerID");
            string str = string.Empty;
            int index = 0;
            while (true)
            {
              if (index < borPairs.Length && index < linkBorPairs.Length)
              {
                if (string.Compare(borPairs[index].Borrower.Id, attribute, true) == 0)
                  str = linkBorPairs[index].Borrower.Id;
                else if (string.Compare(borPairs[index].CoBorrower.Id, attribute, true) == 0)
                  str = linkBorPairs[index].Borrower.Id;
                if (str == string.Empty)
                  ++index;
                else
                  break;
              }
              else
                goto label_32;
            }
            XmlElement newChild = (XmlElement) this.linkedData.xmldoc.ImportNode(current, true);
            newChild.SetAttribute("BorrowerID", str);
            try
            {
              if (refChild != null)
                this.linkedData.xmldoc.DocumentElement.InsertAfter((XmlNode) newChild, (XmlNode) refChild);
              else
                this.linkedData.xmldoc.DocumentElement.AppendChild((XmlNode) newChild);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "Exception in Piggyback synchronize : " + ex.Message);
            }
            refChild = newChild;
          }
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "(syncVOLVOM) Trying to clone nodes: " + xpath + " Error: " + ex.Message);
      }
      this.Dirty = true;
      this.linkedData.Dirty = true;
    }

    private void syncVerification(
      BorrowerPair[] borPairs,
      BorrowerPair[] linkBorPairs,
      SyncPiggybackOperation syncType)
    {
      string str1 = "";
      string str2 = "";
      switch (syncType)
      {
        case SyncPiggybackOperation.SyncVOD:
          str1 = "EllieMae/DEPOSIT";
          str2 = "EllieMae";
          break;
        case SyncPiggybackOperation.SyncVOE:
          str1 = "EMPLOYER";
          break;
        case SyncPiggybackOperation.SyncVOR:
          str1 = "_RESIDENCE";
          break;
        case SyncPiggybackOperation.SyncURLARGG:
          str1 = "URLA2020/GiftsGrants/GiftGrant";
          str2 = "URLA2020/GiftsGrants";
          break;
        case SyncPiggybackOperation.SyncURLAROA:
          str1 = "URLA2020/OtherAssets/OTHER_ASSET";
          str2 = "URLA2020/OtherAssets";
          break;
        case SyncPiggybackOperation.SyncURLAROIS:
          str1 = "URLA2020/OtherIncomeSources/OtherIncomeSource";
          str2 = "URLA2020/OtherIncomeSources";
          break;
        case SyncPiggybackOperation.SyncURLAROL:
          str1 = "URLA2020/OtherLiabilities/OTHER_LIABILITY";
          str2 = "URLA2020/OtherLiabilities";
          break;
      }
      string empty = string.Empty;
      try
      {
        for (int index = 0; index < linkBorPairs.Length; ++index)
        {
          XmlNodeList xmlNodeList1 = this.linkedData.xmldoc.DocumentElement.SelectNodes("BORROWER[@BorrowerID=\"" + linkBorPairs[index].Borrower.Id + "\"]/" + str1);
          if (xmlNodeList1 != null && xmlNodeList1.Count > 0)
          {
            foreach (XmlElement oldChild in xmlNodeList1)
              oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
            this.linkedData.Dirty = true;
          }
          XmlNodeList xmlNodeList2 = this.linkedData.xmldoc.DocumentElement.SelectNodes("BORROWER[@BorrowerID=\"" + linkBorPairs[index].CoBorrower.Id + "\"]/" + str1);
          if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
          {
            foreach (XmlElement oldChild in xmlNodeList2)
              oldChild.ParentNode.RemoveChild((XmlNode) oldChild);
            this.linkedData.Dirty = true;
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "(syncVerification) Trying to clear nodes: " + syncType.ToString() + " Error: " + ex.Message);
      }
      try
      {
        for (int index1 = 0; index1 < borPairs.Length && index1 < linkBorPairs.Length; ++index1)
        {
          for (int index2 = 1; index2 <= 2; ++index2)
          {
            string str3 = index2 == 1 ? borPairs[index1].Borrower.Id : borPairs[index1].CoBorrower.Id;
            string str4 = index2 == 1 ? linkBorPairs[index1].Borrower.Id : linkBorPairs[index1].CoBorrower.Id;
            XmlNodeList xmlNodeList = this.xmldoc.DocumentElement.SelectNodes("BORROWER[@BorrowerID=\"" + str3 + "\"]/" + str1);
            if (xmlNodeList != null && xmlNodeList.Count != 0)
            {
              XmlNode xmlNode = this.linkedData.xmldoc.DocumentElement.SelectSingleNode("BORROWER[@BorrowerID=\"" + str4 + "\"]" + (str2 != "" ? "/" : "") + str2);
              if (xmlNode == null && (syncType == SyncPiggybackOperation.SyncURLARGG || syncType == SyncPiggybackOperation.SyncURLAROA || syncType == SyncPiggybackOperation.SyncURLAROIS || syncType == SyncPiggybackOperation.SyncURLAROL))
              {
                XmlElement xmlElement1 = (XmlElement) this.linkedData.xmldoc.DocumentElement.SelectSingleNode("BORROWER[@BorrowerID=\"" + str4 + "\"]");
                XmlElement xmlElement2 = (XmlElement) xmlElement1.SelectSingleNode("URLA2020") ?? this.linkedData.xmldoc.CreateElement("URLA2020");
                if ((XmlElement) xmlElement1.SelectSingleNode("/" + str2) == null)
                  xmlElement1.AppendChild(xmlElement2.AppendChild((XmlNode) this.linkedData.xmldoc.CreateElement(str2.Replace("URLA2020/", ""))).ParentNode);
                xmlNode = this.linkedData.xmldoc.DocumentElement.SelectSingleNode("BORROWER[@BorrowerID=\"" + str4 + "\"]" + (str2 != "" ? "/" : "") + str2);
              }
              foreach (XmlNode node in xmlNodeList)
              {
                XmlNode newChild = this.linkedData.xmldoc.ImportNode(node, true);
                xmlNode?.AppendChild(newChild);
              }
              this.Dirty = true;
              this.linkedData.Dirty = true;
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "(syncVerification) Trying to copy nodes: " + syncType.ToString() + " Error: " + ex.Message);
      }
    }

    internal void RefreshRequiredTaskRule(string taskName)
    {
      if (this.BeforeTriggerRuleApplied == null)
        return;
      this.BeforeTriggerRuleApplied((object) taskName, new EventArgs());
    }

    public bool Use2010RESPA => this.GetSimpleField("3969") == "RESPA 2010 GFE and HUD-1";

    public bool Use2015RESPA => Utils.CheckIf2015RespaTila(this.GetSimpleField("3969"));

    public bool Use2020URLA => Utils.CheckIfURLA2020(this.GetSimpleField("1825"));

    public LinkSyncType LinkSyncType
    {
      get
      {
        if (this.LinkedData == null)
          return LinkSyncType.None;
        if (this.GetSimpleField("19") == "ConstructionOnly" && this.GetSimpleField("4084") == "Y")
          return LinkSyncType.ConstructionPrimary;
        if (this.linkedData.GetSimpleField("19") == "ConstructionOnly" && this.linkedData.GetSimpleField("4084") == "Y")
          return LinkSyncType.ConstructionLinked;
        return this.GetSimpleField("420") == "SecondLien" ? LinkSyncType.PiggybackLinked : LinkSyncType.PiggybackPrimary;
      }
    }

    public eConsentTypes eConsentType
    {
      get
      {
        return string.Compare(this.GetField("4499"), "fullexternaleconsent", true) == 0 ? eConsentTypes.FullexternaleConsent : eConsentTypes.InternaleConsent;
      }
    }

    public long GetBatchUpdateSequenceNum() => this.map.GetBatchUpdateSequenceNum();

    public void SetBatchUpdateSequenceNum(long sequenceNumber)
    {
      this.map.SetBatchUpdateSequenceNum(sequenceNumber);
    }

    public string[] DBANames
    {
      get => this.dbaNames;
      set => this.dbaNames = value;
    }

    public bool VALoanValidation
    {
      get => this.vaLoanValidation || this.GetField("1172") != "VA";
      set => this.vaLoanValidation = value;
    }

    private bool validateVALoan()
    {
      if (!this.vaLoanValidation)
        return false;
      if (this.GetField("1172") != "VA" || this.Use2015RESPA)
        return true;
      for (int index = 809; index <= 818; ++index)
      {
        if (this.GetField("NEWHUD.X" + (object) index) != string.Empty)
          return false;
      }
      return true;
    }

    public string VALoanWarningMessage
    {
      get
      {
        return "When you apply a template with closing costs to a loan that will have a VA loan type, the itemized fields for line 1102 on the 2010 Itemization will not be copied from the template, and the itemized 1102 fields in the loan will be cleared.";
      }
    }

    public bool UpSettlementServiceProvider(int i)
    {
      if (!this.map.UpSettlementServiceProvider(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool DownSettlementServiceProvider(int i)
    {
      if (!this.map.DownSettlementServiceProvider(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool RemoveSettlementServiceProviderAt(int i)
    {
      if (!this.map.RemoveSettlementServiceProviderAt(i))
        return false;
      this.dirty = true;
      return true;
    }

    public int NewSettlementServiceProvider()
    {
      this.Dirty = true;
      return this.map.NewSettlementServiceProvider();
    }

    public int GetNumberOfSettlementServiceProviders()
    {
      return this.map.GetNumberOfSettlementServiceProviders();
    }

    public void UpTAX4506T(int i, bool for4506Only)
    {
      this.Dirty = true;
      this.map.UpTAX4506T(i, for4506Only);
    }

    public void DownTAX4506T(int i, bool for4506Only)
    {
      this.Dirty = true;
      this.map.DownTAX4506T(i, for4506Only);
    }

    public int NewTAX4506T(bool for4506Only)
    {
      this.Dirty = true;
      int verifIndex = this.map.NewTAX4506T(for4506Only);
      this.onVerificationsChanged(for4506Only ? VerifType.TAX4506 : VerifType.TAX4506T, verifIndex, VerifOperation.Add);
      return verifIndex;
    }

    public int RemoveTAX4506TAt(int i, bool for4506Only)
    {
      this.Dirty = true;
      return this.map.RemoveTAX4506TAt(i, for4506Only);
    }

    public int GetNumberOfTAX4506Ts(bool for4506Only) => this.map.GetNumberOfTAX4506Ts(for4506Only);

    public int GetNumberOf4506TReports() => this.map.GetNumberOf4506TReports();

    public int New4506TReport()
    {
      this.Dirty = true;
      return this.map.New4506TReport();
    }

    public int Remove4506TReportAt(int i)
    {
      this.Dirty = true;
      return this.map.Remove4506TReportAt(i);
    }

    public int RemoveGSERepWarrantTrackerAt(int i)
    {
      this.Dirty = true;
      return this.map.RemoveGSERepWarrantTrackerAt(i);
    }

    public bool Remove4506TReports()
    {
      this.Dirty = true;
      return this.map.Remove4506TReports();
    }

    public bool RemoveTQLFraudAlerts()
    {
      this.Dirty = true;
      return this.map.RemoveTQLFraudAlerts();
    }

    public bool RemoveTQLComplianceAlerts()
    {
      this.Dirty = true;
      return this.map.RemoveTQLComplianceAlerts();
    }

    public bool RemoveTQLDocDeliveryDates()
    {
      this.Dirty = true;
      return this.map.RemoveTQLDocDeliveryDates();
    }

    public bool RemoveGSERepWarrantTrackers()
    {
      this.Dirty = true;
      return this.map.RemoveGSERepWarrantTrackers();
    }

    public bool RemoveDisasters()
    {
      this.Dirty = true;
      return this.map.RemoveDisasters();
    }

    public int GetNumberOfTQLDocDeliveryDates() => this.map.GetNumberOfTQLDocDeliveryDates();

    public int GetNumberOfComplianceAlerts() => this.map.GetNumberOfComplianceAlerts();

    public int GetNumberOfFraudAlerts() => this.map.GetNumberOfFraudAlerts();

    public int GetNumberOfGSERepWarrantTrackers() => this.map.GetNumberOfGSERepWarrantTrackers();

    public bool IsValidValue(string fieldId) => this.IsValidValue(fieldId, this.GetField(fieldId));

    public bool IsValidValue(string fieldId, string val)
    {
      switch (fieldId)
      {
        case "ULDD.X179":
          string[] source = val.Trim().Split(' ');
          if (source.Length > 10 || ((IEnumerable<string>) source).Any<string>((Func<string, bool>) (code => code.Length != 3 || !code.IsAlphaNumeric())))
            return false;
          break;
        case "ULDD.RefinanceCashOutAmount":
          if (Utils.ParseDouble((object) val, 0.0) < 0.0)
            return false;
          break;
      }
      return true;
    }

    public List<FundingFee> GetFundingFees(bool hideZero)
    {
      return this.Calculator.GetFundingFees(hideZero);
    }

    public string[] GetAUSTrackingHistoryGUIDs(bool includeManualEntry)
    {
      List<string> stringList = new List<string>();
      AUSTrackingHistoryList trackingHistoryList = this.GetAUSTrackingHistoryList();
      if (trackingHistoryList != null)
      {
        trackingHistoryList.Sort();
        for (int i = 0; i < trackingHistoryList.HistoryCount; ++i)
        {
          AUSTrackingHistoryLog historyAt = trackingHistoryList.GetHistoryAt(i);
          if (string.Compare(historyAt.RecordType, "Manual", true) != 0 || includeManualEntry)
            stringList.Add(historyAt.HistoryID);
        }
      }
      return stringList.ToArray();
    }

    public int GetNumberOfAUSTrackingHistory() => this.GetNumberOfAUSTrackingHistory(false);

    public int GetNumberOfAUSTrackingHistory(bool checkAllPairs)
    {
      if (!checkAllPairs)
        return this.map.GetNumberOfAUSTrackingHistory();
      int index1 = -1;
      int ausTrackingHistory = 0;
      string id = this.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.SetBorrowerPair(borrowerPairs[index2]);
        ausTrackingHistory += this.map.GetNumberOfAUSTrackingHistory();
        if (id == borrowerPairs[index2].Id)
          index1 = index2;
      }
      this.SetBorrowerPair(borrowerPairs[index1]);
      return ausTrackingHistory;
    }

    public AUSTrackingHistoryList GetAUSTrackingHistoryList()
    {
      return this.GetAUSTrackingHistoryList(false);
    }

    public AUSTrackingHistoryList GetAUSTrackingHistoryList(bool checkAllPairs)
    {
      if (!checkAllPairs)
        return this.map.GetAUSTrackingHistoryList();
      AUSTrackingHistoryList trackingHistoryList1 = (AUSTrackingHistoryList) null;
      int index1 = -1;
      string id = this.CurrentBorrowerPair.Id;
      BorrowerPair[] borrowerPairs = this.GetBorrowerPairs();
      for (int index2 = 0; index2 < borrowerPairs.Length; ++index2)
      {
        this.SetBorrowerPair(borrowerPairs[index2]);
        AUSTrackingHistoryList trackingHistoryList2 = this.map.GetAUSTrackingHistoryList();
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
          if (id == borrowerPairs[index2].Id)
            index1 = index2;
        }
      }
      this.SetBorrowerPair(borrowerPairs[index1]);
      trackingHistoryList1.Sort();
      return trackingHistoryList1;
    }

    public bool CheckIfAUSTrackingForLP(AUSTrackingHistoryLog trackingLog)
    {
      return trackingLog.RecordType == "LP" || trackingLog.SubmissionType == "LP";
    }

    public string GetAUSTrackingHistory(string historyGUID)
    {
      AUSTrackingHistoryList trackingHistoryList = this.GetAUSTrackingHistoryList();
      for (int i = 0; i < trackingHistoryList.HistoryCount; ++i)
      {
        AUSTrackingHistoryLog historyAt = trackingHistoryList.GetHistoryAt(i);
        if (string.Compare(historyAt.HistoryID, historyGUID, true) == 0)
        {
          Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
          string[] assignedFieldIds = historyAt.DataValues.GetAssignedFieldIDs();
          for (int index = 0; index < assignedFieldIds.Length; ++index)
          {
            if (!insensitiveHashtable.ContainsKey((object) assignedFieldIds[index]))
              insensitiveHashtable.Add((object) assignedFieldIds[index], (object) historyAt.GetField(assignedFieldIds[index]));
          }
          if (!insensitiveHashtable.ContainsKey((object) "GUID"))
            insensitiveHashtable.Add((object) "GUID", (object) "");
          insensitiveHashtable[(object) "GUID"] = (object) historyAt.HistoryID;
          XmlDocument xmlDocument = new XmlDocument();
          xmlDocument.LoadXml("<Fields />");
          foreach (DictionaryEntry dictionaryEntry in insensitiveHashtable)
          {
            XmlElement xmlElement = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("Field"));
            xmlElement.SetAttribute("Name", dictionaryEntry.Key.ToString());
            xmlElement.SetAttribute("Value", dictionaryEntry.Value.ToString());
          }
          return xmlDocument.OuterXml;
        }
      }
      return (string) null;
    }

    public string ImportAUSTrackingHistory(
      string xmlString,
      string submissionDate,
      bool copyDefaultLoanDataToLog,
      bool forDU)
    {
      XmlDocument xmlDocument;
      try
      {
        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlString);
      }
      catch
      {
        throw new FormatException("ImportAUSTrackingHistory: Failed to load the XML string!");
      }
      Hashtable insensitiveHashtable = CollectionsUtil.CreateCaseInsensitiveHashtable();
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      foreach (XmlElement selectNode in xmlDocument.SelectNodes("//Field"))
      {
        string key = selectNode.GetAttribute("Name").Trim();
        if (!(key == string.Empty))
        {
          string str = selectNode.GetAttribute("Value").Trim();
          if (!insensitiveHashtable.ContainsKey((object) key))
            insensitiveHashtable.Add((object) key, (object) str);
        }
      }
      this.Dirty = true;
      string CreatedOn = string.Empty;
      if (this.SubmissionDateTimeInUTC(submissionDate))
        CreatedOn = submissionDate;
      DateTime submittedDate = Utils.ParseDate((object) submissionDate);
      if (submittedDate == DateTime.MinValue)
        submittedDate = DateTime.Now;
      if (!insensitiveHashtable.ContainsKey((object) "AUS.X999"))
        insensitiveHashtable.Add((object) "AUS.X999", (object) "Automation");
      else
        insensitiveHashtable[(object) "AUS.X999"] = (object) "Automation";
      if (!insensitiveHashtable.ContainsKey((object) "AUS.X1"))
        insensitiveHashtable.Add((object) "AUS.X1", forDU ? (object) "DU" : (object) "LP");
      if (insensitiveHashtable[(object) "AUS.X1"].ToString() == string.Empty)
        insensitiveHashtable[(object) "AUS.X1"] = forDU ? (object) "DU" : (object) "LP";
      if (!insensitiveHashtable.ContainsKey((object) "AUS.X3"))
        insensitiveHashtable.Add((object) "AUS.X3", (object) submittedDate.ToString("MM/dd/yyyy"));
      if (!insensitiveHashtable.ContainsKey((object) "AUS.X173"))
        insensitiveHashtable.Add((object) "AUS.X173", (object) submittedDate.ToString("hh:mm:ss tt"));
      if (copyDefaultLoanDataToLog)
      {
        AUSTrackingHistoryLog trackingHistoryLog = new AUSTrackingHistoryLog("");
        trackingHistoryLog.CopyLoanToLog(this, forDU);
        string[] assignedFieldIds = trackingHistoryLog.DataValues.GetAssignedFieldIDs();
        for (int index = 0; index < assignedFieldIds.Length; ++index)
        {
          if (!insensitiveHashtable.ContainsKey((object) assignedFieldIds[index]))
            insensitiveHashtable.Add((object) assignedFieldIds[index], (object) "");
          insensitiveHashtable[(object) assignedFieldIds[index]] = (object) trackingHistoryLog.DataValues.GetField(assignedFieldIds[index]);
        }
      }
      List<KeyValuePair<string, string>> fieldPriorValues = this.getAUSFieldPriorValues();
      this.setDiscrepancyAlertFields();
      string str1 = this.map.AddAUSTrackingHistory(submittedDate, insensitiveHashtable, forDU, CreatedOn);
      this.triggerAUSFRules(fieldPriorValues);
      return str1;
    }

    private bool SubmissionDateTimeInUTC(string Date)
    {
      try
      {
        string format = "yyyy-MM-ddTHH:mm:ssZ";
        return DateTime.TryParseExact(Date, format, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime _);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, " Unable to parse submissiondatetime  ,Error Message: " + ex.Message);
        return false;
      }
    }

    public bool AddAUSTrackingHistory(AUSTrackingHistoryLog trackingHistory)
    {
      List<KeyValuePair<string, string>> fieldPriorValues = this.getAUSFieldPriorValues();
      this.Dirty = true;
      this.setDiscrepancyAlertFields();
      bool flag = this.map.AddAUSTrackingHistory(trackingHistory);
      this.triggerAUSFRules(fieldPriorValues);
      return flag;
    }

    public bool UpdateAUSTrackingHistory(AUSTrackingHistoryLog trackingHistory)
    {
      List<KeyValuePair<string, string>> fieldPriorValues = this.getAUSFieldPriorValues();
      this.Dirty = true;
      bool flag = this.map.UpdateAUSTrackingHistory(trackingHistory);
      this.triggerAUSFRules(fieldPriorValues);
      return flag;
    }

    public void PopulateLatestSubmissionAusTracking()
    {
      List<KeyValuePair<string, string>> fieldPriorValues = this.getAUSFieldPriorValues();
      this.innerMap.PopulateLatestSubmissionAusTracking();
      this.triggerAUSFRules(fieldPriorValues);
    }

    private List<KeyValuePair<string, string>> getAUSFieldPriorValues()
    {
      List<KeyValuePair<string, string>> fieldPriorValues = new List<KeyValuePair<string, string>>();
      for (int index = 1; index <= 21; ++index)
        fieldPriorValues.Add(new KeyValuePair<string, string>("AUSF.X" + (object) index, this.GetSimpleField("AUSF.X" + (object) index)));
      return fieldPriorValues;
    }

    private void triggerAUSFRules(
      List<KeyValuePair<string, string>> ausFieldPriorValues)
    {
      foreach (KeyValuePair<string, string> ausFieldPriorValue in ausFieldPriorValues)
      {
        string simpleField = this.GetSimpleField(ausFieldPriorValue.Key);
        if (string.Compare(ausFieldPriorValue.Value, simpleField, true) != 0)
        {
          this.Dirty = true;
          this.updateDirtyTbl2(ausFieldPriorValue.Key, simpleField, this.CurrentBorrowerPair);
          this.TriggerCalculation(ausFieldPriorValue.Key, simpleField, true);
        }
      }
      ausFieldPriorValues.Clear();
    }

    private void setDiscrepancyAlertFields()
    {
      this.SetField("AUSF.X19", "");
      this.SetField("AUSF.X20", "");
      this.SetField("AUSF.X21", "");
      ((IEnumerable<string[]>) RegulationAlerts.GetAUSDataDiscrepancyAlertFields(false, this.GetField("1811") == "PrimaryResidence", Utils.ParseDouble((object) this.GetField("1014")) == 0.0)).ToList<string[]>().ForEach((Action<string[]>) (x => this.SetField(x[2], "")));
    }

    public int GetNumberOfVerficiationTimelineLogs(VerificationTimelineType timelineType)
    {
      return this.map.GetNumberOfVerficiationTimelineLogs(timelineType);
    }

    public VerificationTimelineList GetVerficiationTimelineLogs(
      VerificationTimelineType timelineType)
    {
      return this.map.GetVerficiationTimelineLogs(timelineType);
    }

    public bool AddVerificationTimelineLog(VerificationTimelineLog timelineLog)
    {
      this.Dirty = true;
      return this.map.AddVerificationTimelineLog(timelineLog);
    }

    public bool UpdateVerificationTimelineLog(VerificationTimelineLog timelineLog)
    {
      this.Dirty = true;
      return this.map.UpdateVerificationTimelineLog(timelineLog);
    }

    public int GetNumberOfVerficiationDocuments(VerificationTimelineType timelineType)
    {
      return this.map.GetNumberOfVerficiationDocuments(timelineType);
    }

    public VerificationDocumentList GetVerficiationDocuments(VerificationTimelineType timelineType)
    {
      return this.map.GetVerficiationDocuments(timelineType);
    }

    public bool AddVerificationDocument(VerificationDocument documentLog)
    {
      this.Dirty = true;
      return this.map.AddVerificationDocument(documentLog);
    }

    public bool UpdateVerificationDocument(VerificationDocument documentLog)
    {
      this.Dirty = true;
      return this.map.UpdateVerificationDocument(documentLog);
    }

    public bool UpHomeCounselingProvider(int i)
    {
      if (!this.map.UpHomeCounselingProvider(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool DownHomeCounselingProvider(int i)
    {
      if (!this.map.DownHomeCounselingProvider(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool RemoveHomeCounselingProviderAt(int i)
    {
      if (!this.map.RemoveHomeCounselingProviderAt(i))
        return false;
      this.dirty = true;
      return true;
    }

    public int NewHomeCounselingProvider() => this.NewHomeCounselingProvider((string) null);

    public int NewHomeCounselingProvider(string importSource)
    {
      this.Dirty = true;
      return this.map.NewHomeCounselingProvider(importSource);
    }

    public int GetNumberOfHomeCounselingProviders()
    {
      return this.map.GetNumberOfHomeCounselingProviders();
    }

    public int[] GetSelectedHomeCounselingProviders()
    {
      return this.map.GetSelectedHomeCounselingProviders();
    }

    public void SetSelectedHomeCounselingProvider(int i, bool selected)
    {
      this.Dirty = true;
      this.map.SetSelectedHomeCounselingProvider(i, selected);
    }

    public bool UpNonVol(int i)
    {
      if (!this.map.UpNonVol(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool DownNonVol(int i)
    {
      if (!this.map.DownNonVol(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool RemoveNonVolAt(int i)
    {
      if (!this.map.RemoveNonVolAt(i))
        return false;
      this.dirty = true;
      return true;
    }

    public int NewNonVol() => this.NewNonVol((string) null);

    public int NewNonVol(string importSource)
    {
      this.Dirty = true;
      return this.map.NewNonVol(importSource);
    }

    public int GetNumberOfNonVols() => this.map.GetNumberOfNonVols();

    public void CreateNonVols(Dictionary<int, List<string>> nonVol)
    {
      this.map.CreateNonVols(nonVol);
    }

    public void ClearNonVols() => this.map.ClearNonVols();

    public bool UpAffiliate(int i)
    {
      if (!this.map.UpAffiliate(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool DownAffiliate(int i)
    {
      if (!this.map.DownAffiliate(i))
        return false;
      this.dirty = true;
      return true;
    }

    public bool RemoveAffiliateAt(int i)
    {
      if (!this.map.RemoveAffiliateAt(i))
        return false;
      this.dirty = true;
      return true;
    }

    public int NewAffiliate()
    {
      this.Dirty = true;
      return this.map.NewAffiliate();
    }

    public int GetNumberOfAffiliates() => this.map.GetNumberOfAffiliates();

    public static string GuidToString(Guid guid) => guid.ToString("B");

    public static Guid StringToGuid(string guid) => Guid.Parse(guid);

    public Dictionary<string, Dictionary<string, string>> GetSnapshotDataForAllDisclosureTracking2015Logs()
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetLogList().GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, bool> snapshotGuids = new Dictionary<string, bool>();
      foreach (DisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        bool flag = !disclosureTracking2015Log.UCDCreationError && (disclosureTracking2015Log.DisclosedForCD || disclosureTracking2015Log.DisclosedForLE);
        snapshotGuids.Add(disclosureTracking2015Log.Guid, flag);
      }
      return this.SnapshotProvider.GetLoanSnapshots(LogSnapshotType.DisclosureTracking, snapshotGuids);
    }

    public Dictionary<string, Dictionary<string, string>> GetSnapshotDataForAllDisclosureTracking2015Logs(
      bool includedInTimelineOnly)
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetLogList().GetAllIDisclosureTracking2015Log(includedInTimelineOnly);
      Dictionary<string, bool> snapshotGuids = new Dictionary<string, bool>();
      foreach (DisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        bool flag = !disclosureTracking2015Log.UCDCreationError && (disclosureTracking2015Log.DisclosedForCD || disclosureTracking2015Log.DisclosedForLE);
        snapshotGuids.Add(disclosureTracking2015Log.Guid, flag);
      }
      return this.SnapshotProvider.GetLoanSnapshots(LogSnapshotType.DisclosureTracking, snapshotGuids);
    }

    public void GetSnapshotDataForAllDisclosureTracking2015LogsForLoan()
    {
      IDisclosureTracking2015Log[] idisclosureTracking2015Log = this.GetLogList().GetAllIDisclosureTracking2015Log(true);
      Dictionary<string, bool> snapshotGuids = new Dictionary<string, bool>();
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        bool flag = !disclosureTracking2015Log.UCDCreationError && (disclosureTracking2015Log.DisclosedForCD || disclosureTracking2015Log.DisclosedForLE);
        snapshotGuids.Add(disclosureTracking2015Log.Guid, flag);
      }
      Dictionary<string, Dictionary<string, string>> loanSnapshots = this.SnapshotProvider.GetLoanSnapshots(LogSnapshotType.DisclosureTracking, snapshotGuids);
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in idisclosureTracking2015Log)
      {
        if (snapshotGuids.ContainsKey(disclosureTracking2015Log.Guid))
        {
          if (!disclosureTracking2015Log.IsLoanDataListExist)
            disclosureTracking2015Log.PopulateLoanDataList(loanSnapshots[disclosureTracking2015Log.Guid]);
        }
        else if (disclosureTracking2015Log.IsLoanDataListExist)
          disclosureTracking2015Log.RemoveShapsnot();
      }
    }

    public void GetSnapshotDataForDisclosureTracking2015LogsForLoan(string guid)
    {
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.GetLogList().GetAllIDisclosureTracking2015Log(true))
      {
        if (disclosureTracking2015Log.Guid == guid && !disclosureTracking2015Log.IsLoanDataListExist)
        {
          bool ucdExists = !disclosureTracking2015Log.UCDCreationError && (disclosureTracking2015Log.DisclosedForLE || disclosureTracking2015Log.DisclosedForCD);
          Dictionary<string, string> loanSnapshot = this.SnapshotProvider.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, new Guid(guid), ucdExists);
          disclosureTracking2015Log.PopulateLoanDataList(loanSnapshot);
        }
      }
    }

    public void GetCalculationDTSnapshotForLoan()
    {
      string[] strArray = new string[10]
      {
        "FV.X356",
        "FV.X357",
        "FV.X358",
        "FV.X359",
        "FV.X360",
        "FV.X361",
        "FV.X362",
        "FV.X363",
        "FV.X364",
        "FV.X365"
      };
      List<string> stringList = new List<string>();
      foreach (string id in strArray)
      {
        string field = this.GetField(id);
        if (field != "" && !stringList.Contains(field))
          stringList.Add(field);
      }
      foreach (IDisclosureTracking2015Log disclosureTracking2015Log in this.GetLogList().GetAllIDisclosureTracking2015Log(false))
      {
        if (stringList.Contains(disclosureTracking2015Log.Guid))
        {
          if (!disclosureTracking2015Log.IsLoanDataListExist)
          {
            bool ucdExists = !disclosureTracking2015Log.UCDCreationError && (disclosureTracking2015Log.DisclosedForLE || disclosureTracking2015Log.DisclosedForCD);
            Dictionary<string, string> loanSnapshot = this.SnapshotProvider.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, Guid.Parse(disclosureTracking2015Log.Guid), ucdExists);
            disclosureTracking2015Log.PopulateLoanDataList(loanSnapshot);
          }
        }
        else if (disclosureTracking2015Log.IsLoanDataListExist)
          disclosureTracking2015Log.RemoveShapsnot();
      }
    }

    public bool IncludeSnapshotInXML
    {
      get => this.includeSnapshotInXML;
      set => this.includeSnapshotInXML = value;
    }

    public void ChangeFormVersion()
    {
      if (this.FormVersionChanged == null)
        return;
      this.FormVersionChanged((object) null, (EventArgs) null);
    }

    public void DisclosureTracking2015Created(DisclosureTracking2015Log log)
    {
      if (this.Disclosure2015Created == null)
        return;
      this.Disclosure2015Created((object) log, (EventArgs) null);
    }

    public static List<FundingFee> GetFundingFees(
      string fundingFeesFieldValue,
      string loanId = null,
      string correlationId = null,
      int loanVersionId = 0)
    {
      List<FundingFee> fundingFees = new List<FundingFee>();
      if (fundingFeesFieldValue == "" || fundingFeesFieldValue == null)
        return fundingFees;
      PropertyInfo[] properties = typeof (FundingFee).GetProperties();
      string[] strArray1 = fundingFeesFieldValue.Split('^');
      string str = string.Empty;
      Exception exception = (Exception) null;
      for (int index1 = 0; index1 < strArray1.Length; ++index1)
      {
        string[] strArray2 = strArray1[index1].Split('|');
        int index2 = 0;
        FundingFee fundingFee = new FundingFee();
        foreach (PropertyInfo propertyInfo in properties)
        {
          if (!(propertyInfo.Name == "Tag"))
          {
            if (!(propertyInfo.GetSetMethod() == (MethodInfo) null))
            {
              try
              {
                propertyInfo.SetValue((object) fundingFee, Convert.ChangeType((object) strArray2[index2], propertyInfo.PropertyType), (object[]) null);
              }
              catch (Exception ex)
              {
                str = str + Environment.NewLine + "fee section: " + strArray2[0] + "fee value: " + strArray2[index2] + ", index of col: " + (object) index2;
                exception = ex;
              }
              finally
              {
                ++index2;
              }
            }
          }
        }
        fundingFees.Add(fundingFee);
      }
      if (exception != null)
      {
        Tracing.Log(LoanData.sw, nameof (LoanData), TraceLevel.Error, "LoanId: " + loanId + " ,CorrelationId: " + correlationId + " , Loan Version: " + (object) loanVersionId + " ,Error Message: " + exception.Message + " ,Invalid Fees: " + str);
        throw exception;
      }
      return fundingFees;
    }

    private void populateLinkedFields(bool cleanUpField)
    {
      bool flag = cleanUpField || this.linkedData == null;
      for (int index = 0; index < this.linkedStandardFields.Length; ++index)
        this.SetField("LINK_" + this.linkedStandardFields[index], flag ? "" : this.linkedData.GetField(this.linkedStandardFields[index]));
    }

    public void CreateSubsetConstructionLinkedFields()
    {
      if (this.GetField("LINKGUID") == "" || this.linkedData == null || this.linkedData.GetField("LINKGUID") == "")
        return;
      this.populateLinkedFields(false);
      if (this.linkedData == null)
        return;
      this.Dirty = true;
      string[] linkedValues = new string[this.linkedVirtualFields.Length];
      for (int index = 0; index < this.linkedVirtualFields.Length; ++index)
        linkedValues[index] = this.linkedData.GetField(this.linkedVirtualFields[index]);
      this.map.CreateSubsetConstructionLinkedFields(this.linkedVirtualFields, linkedValues);
    }

    public List<KeyValuePair<string, string>> GetSubsetConstructionLinkedFieldValues()
    {
      return this.map.GetSubsetConstructionLinkedFieldValues();
    }

    public void RemoveSubsetConstructionLinkedFieldValues()
    {
      this.populateLinkedFields(true);
      this.map.RemoveSubsetConstructionLinkedFieldValues();
    }

    public void SetBusinessRuleTriggerEnum(
      BusinessRuleOnDemandEnum newBusinessRuleTrigger,
      bool include)
    {
      if (include)
        this.businessRuleTrigger |= newBusinessRuleTrigger;
      else
        this.businessRuleTrigger &= ~newBusinessRuleTrigger;
    }

    public object DDMTrigger
    {
      get => this.ddmTrigger;
      set => this.ddmTrigger = value;
    }

    public string DDMOnDemandTriggerFields
    {
      get => this.map.DDMOnDemandTriggerFields;
      set => this.map.DDMOnDemandTriggerFields = value;
    }

    public bool DDMIsRequired
    {
      get => this.map.DDMIsRequired;
      set => this.map.DDMIsRequired = value;
    }

    public Dictionary<string, string> DDMOnDemandVirtualFields
    {
      set => this.ddmOnDemandVirtualFields = value;
      get => this.ddmOnDemandVirtualFields;
    }

    public void ResetDDMVirtualFieldTable()
    {
      if (this.ddmOnDemandVirtualFields == null || this.ddmOnDemandVirtualFields.Count == 0)
        return;
      foreach (string str in new List<string>((IEnumerable<string>) this.ddmOnDemandVirtualFields.Keys))
        this.ddmOnDemandVirtualFields[str] = this.GetSimpleField(str);
    }

    public int GetNumberOfNonBorrowingOwnerContact()
    {
      return this.map.GetNumberOfNonBorrowingOwnerContact();
    }

    public string GetEidOfNonBorrowingOwnerContactAt(int i)
    {
      return this.map.GetEidOfNonBorrowingOwnerContactAt(i);
    }

    public int NewNonBorrowingOwnerContact()
    {
      this.Dirty = true;
      return this.map.NewNonBorrowingOwnerContact();
    }

    public bool RemoveNonBorrowingOwnerContactAt(int i)
    {
      bool flag = this.map.RemoveNonBorrowingOwnerContactAt(i);
      if (flag)
      {
        this.Dirty = true;
        this.TriggerCalculation("URLA.X136", (string) null);
      }
      return flag;
    }

    public int GetNBOLinkedVesting(int VestingIndex) => this.map.GetNBOLinkedVesting(VestingIndex);

    public int GetNBOLinkedVesting(string nboGUID) => this.map.GetNBOLinkedVesting(nboGUID);

    public int GetVestingLinkedNBO(string vestingGUID) => this.map.GetVestingLinkedNBO(vestingGUID);

    public List<int> GetNBOLinkedVestingIdxList()
    {
      List<int> linkedVestingIdxList = new List<int>();
      int additionalVestingParties = this.GetNumberOfAdditionalVestingParties();
      for (int index = 1; index <= additionalVestingParties; ++index)
      {
        int nboLinkedVesting = this.GetNBOLinkedVesting(this.GetField("TR" + index.ToString("00") + "99"));
        if (nboLinkedVesting != 0)
          linkedVestingIdxList.Add(nboLinkedVesting);
      }
      return linkedVestingIdxList;
    }

    public List<NonBorrowingOwner> GetNboByBorrowerPairId(string borrowPairId)
    {
      List<NonBorrowingOwner> byBorrowerPairId = new List<NonBorrowingOwner>();
      Dictionary<string, Tuple<string, int>> dictionary = new Dictionary<string, Tuple<string, int>>();
      VestingPartyFields[] vestingPartyFields = this.GetVestingPartyFields(false);
      for (int index = 1; index <= vestingPartyFields.Length; ++index)
      {
        string str = "TR" + (index <= 99 ? index.ToString("00") : index.ToString("000"));
        string simpleField1 = this.GetSimpleField(str + "05");
        string simpleField2 = this.GetSimpleField(str + "10");
        if (!dictionary.ContainsKey(simpleField2) && (borrowPairId == simpleField1 || string.IsNullOrEmpty(borrowPairId)))
          dictionary.Add(simpleField2, new Tuple<string, int>(simpleField1, index));
      }
      int borrowingOwnerContact = this.GetNumberOfNonBorrowingOwnerContact();
      for (int i = 1; i <= borrowingOwnerContact; ++i)
      {
        string str = "NBOC" + (i <= 99 ? i.ToString("00") : i.ToString("000"));
        NonBorrowingOwner nonBorrowingOwner = new NonBorrowingOwner();
        nonBorrowingOwner.BorrowerVestingRecordID = this.GetSimpleField(str + "99");
        if (!string.IsNullOrEmpty(nonBorrowingOwner.BorrowerVestingRecordID) && dictionary.ContainsKey(nonBorrowingOwner.BorrowerVestingRecordID))
        {
          nonBorrowingOwner.NboIndex = i;
          nonBorrowingOwner.FirstName = this.GetSimpleField(str + "01");
          nonBorrowingOwner.MiddleName = this.GetSimpleField(str + "02");
          nonBorrowingOwner.LastName = this.GetSimpleField(str + "03");
          nonBorrowingOwner.SuffixName = this.GetSimpleField(str + "04");
          nonBorrowingOwner.AddressStreet = this.GetSimpleField(str + "05");
          nonBorrowingOwner.AddressCity = this.GetSimpleField(str + "06");
          nonBorrowingOwner.AddressState = this.GetSimpleField(str + "07");
          nonBorrowingOwner.AddressPostalCode = this.GetSimpleField(str + "08");
          nonBorrowingOwner.BorrowerType = this.GetSimpleField(str + "09");
          nonBorrowingOwner.HomePhoneNumber = this.GetSimpleField(str + "10");
          nonBorrowingOwner.EmailAddress = this.GetSimpleField(str + "11");
          nonBorrowingOwner.No3rdPartyEmailIndicator = new bool?(Utils.ParseBoolean((object) this.GetSimpleField(str + "12")));
          nonBorrowingOwner.BusinessPhoneNumber = this.GetSimpleField(str + "13");
          nonBorrowingOwner.CellPhoneNumber = this.GetSimpleField(str + "14");
          nonBorrowingOwner.FaxNumber = this.GetSimpleField(str + "15");
          nonBorrowingOwner.DateOfBirth = new DateTime?(Utils.ParseDate((object) this.GetSimpleField(str + "16")));
          nonBorrowingOwner.eSignConsentNBOCStatus = this.GetSimpleField(str + "17");
          nonBorrowingOwner.eSignConsentNBOCDateAccepted = new DateTime?(Utils.ParseDate((object) this.GetSimpleField(str + "18")));
          nonBorrowingOwner.eSignConsentNBOCIPAddress = this.GetSimpleField(str + "19");
          nonBorrowingOwner.eSignConsentNBOCSource = this.GetSimpleField(str + "19");
          nonBorrowingOwner.NBOID = this.GetSimpleField(str + "98");
          nonBorrowingOwner.EID = this.GetEidOfNonBorrowingOwnerContactAt(i);
          nonBorrowingOwner.BorrowerVestingRecordID = this.GetSimpleField(str + "99");
          nonBorrowingOwner.VestingBorrowerPairId = dictionary[nonBorrowingOwner.BorrowerVestingRecordID].Item1;
          nonBorrowingOwner.NBOVestingIndex = dictionary[nonBorrowingOwner.BorrowerVestingRecordID].Item2;
          byBorrowerPairId.Add(nonBorrowingOwner);
        }
      }
      return byBorrowerPairId;
    }

    public int GetNumberOfURLAAlternateNames(bool borrower)
    {
      return this.map.GetNumberOfURLAAlternateNames(borrower);
    }

    public IList<URLAAlternateName> GetURLAAlternames(bool borrower)
    {
      return this.map.GetURLAAlternateNames(borrower);
    }

    public void UpdateURLAAlternateNames(bool borrower, IList<URLAAlternateName> akaNameList)
    {
      this.Dirty = true;
      this.map.UpdateURLAAlternateNames(borrower, akaNameList);
    }

    public int GetNumberOfGoodFaithChangeOfCircumstance()
    {
      return this.map.GetNumberOfGoodFaithChangeOfCircumstance();
    }

    public int NewGoodFaithChangeOfCircumstance(string keyFieldID)
    {
      this.Dirty = true;
      return this.map.NewGoodFaithChangeOfCircumstance(keyFieldID);
    }

    public bool RemoveGoodFaithChangeOfCircumstance(string alertFieldID)
    {
      bool flag = false;
      int circumstanceRecordIndex = this.GetGoodFaithChangeOfCircumstanceRecordIndex(alertFieldID);
      if (circumstanceRecordIndex > -1)
        flag = this.map.RemoveGoodFaithChangeOfCircumstance(circumstanceRecordIndex);
      if (flag)
        this.Dirty = true;
      return flag;
    }

    public bool RemoveGoodFaithChangeOfCircumstance(int i)
    {
      bool flag = this.map.RemoveGoodFaithChangeOfCircumstance(i);
      if (flag)
      {
        for (int index = 1; index < 15; ++index)
          this.TriggerCalculation("XCOC" + i.ToString("00") + index.ToString("00"), "");
      }
      return flag;
    }

    public bool RemoveAllGoodFaithChangeOfCircumstance()
    {
      int changeOfCircumstance = this.map.GetNumberOfGoodFaithChangeOfCircumstance();
      bool flag = this.map.RemoveAllGoodFaithChangeOfCircumstance();
      if (flag)
      {
        this.Dirty = true;
        for (int index1 = 1; index1 <= changeOfCircumstance; ++index1)
        {
          for (int index2 = 1; index2 < 15; ++index2)
            this.TriggerCalculation("XCOC" + index1.ToString("00") + index2.ToString("00"), "");
        }
      }
      return flag;
    }

    public int GetGoodFaithChangeOfCircumstanceRecordIndex(string alertFieldID)
    {
      int changeOfCircumstance = this.map.GetNumberOfGoodFaithChangeOfCircumstance();
      for (int circumstanceRecordIndex = 1; circumstanceRecordIndex <= changeOfCircumstance; ++circumstanceRecordIndex)
      {
        if (string.Compare(this.GetField("XCOC" + circumstanceRecordIndex.ToString("00") + "01"), alertFieldID, true) == 0)
          return circumstanceRecordIndex;
      }
      return -1;
    }

    public string GetGoodFaithChangeOfCircumstanceRecordFieldStr(string fieldIndex)
    {
      int changeOfCircumstance = this.map.GetNumberOfGoodFaithChangeOfCircumstance();
      string circumstanceRecordFieldStr = "";
      for (int index = 1; index <= changeOfCircumstance; ++index)
      {
        string field = this.GetField("XCOC" + index.ToString("00") + fieldIndex);
        if (field != "" && !circumstanceRecordFieldStr.Contains(Environment.NewLine + field + Environment.NewLine) && !circumstanceRecordFieldStr.StartsWith(field))
          circumstanceRecordFieldStr = circumstanceRecordFieldStr + field + Environment.NewLine;
      }
      return circumstanceRecordFieldStr;
    }

    public string[] GetGoodFaithChangeOfCircumstanceRecordField(string fieldIndex)
    {
      int changeOfCircumstance = this.map.GetNumberOfGoodFaithChangeOfCircumstance();
      string[] circumstanceRecordField = new string[changeOfCircumstance];
      for (int index = 1; index <= changeOfCircumstance; ++index)
        circumstanceRecordField[index - 1] = this.GetField("XCOC" + index.ToString("00") + fieldIndex);
      return circumstanceRecordField;
    }

    public void SyncAlertCoCRecords(Hashtable triggerFields)
    {
      if (triggerFields == null || triggerFields.Count == 0 || this.GetField("4461") != "Y")
      {
        if (this.map.GetNumberOfGoodFaithChangeOfCircumstance() <= 0)
          return;
        this.RemoveAllGoodFaithChangeOfCircumstance();
        if (this.calculator == null || !(this.GetField("4461") == "Y"))
          return;
        this.Locked_COC_Fields.Clear();
        this.calculator.CopyAlertCoCToLECDPage1(false);
        this.calculator.CopyAlertCoCToLECDPage1(true);
      }
      else
      {
        Dictionary<string, Dictionary<string, string>> dictionary1 = new Dictionary<string, Dictionary<string, string>>();
        foreach (DictionaryEntry triggerField in triggerFields)
        {
          ArrayList arrayList = (ArrayList) triggerField.Value;
          for (int index = 0; index < arrayList.Count; ++index)
          {
            GFFVAlertTriggerField alertTriggerField = (GFFVAlertTriggerField) arrayList[index];
            if (!(alertTriggerField.Description == "Cannot Decrease") && !(alertTriggerField.Description == "Cannot Increase") && !(alertTriggerField.Description == "10% Variance") && !dictionary1.ContainsKey(alertTriggerField.FieldId))
              dictionary1.Add(alertTriggerField.FieldId, new Dictionary<string, string>()
              {
                {
                  "XCOC0009",
                  alertTriggerField.VarianceLimit
                },
                {
                  "XCOC0010",
                  alertTriggerField.Description
                },
                {
                  "XCOC0011",
                  alertTriggerField.InitialLEValue
                },
                {
                  "XCOC0012",
                  alertTriggerField.Baseline
                },
                {
                  "XCOC0013",
                  alertTriggerField.DisclosedValue
                },
                {
                  "XCOC0014",
                  alertTriggerField.ItemizationValue
                },
                {
                  "XCOC0015",
                  alertTriggerField.VarianceValue
                }
              });
          }
        }
        string str1 = "XCOC";
        List<List<string>> stringListList = new List<List<string>>();
        List<string> stringList1 = new List<string>();
        List<int> intList1 = new List<int>();
        int changeOfCircumstance = this.map.GetNumberOfGoodFaithChangeOfCircumstance();
        for (int index1 = 1; index1 <= changeOfCircumstance; ++index1)
        {
          string field = this.GetField(str1 + index1.ToString("00") + "01");
          if (field == "" || !dictionary1.ContainsKey(field))
          {
            intList1.Add(index1);
          }
          else
          {
            List<string> stringList2 = new List<string>();
            Dictionary<string, string> dictionary2 = dictionary1.ContainsKey(field) ? dictionary1[field] : (Dictionary<string, string>) null;
            for (int index2 = 0; index2 < DisclosureTracking2015Log.DiscloseCOCFields.Count; ++index2)
            {
              string str2 = DisclosureTracking2015Log.DiscloseCOCFields[index2].Substring(DisclosureTracking2015Log.DiscloseCOCFields[index2].Length - 2, 2);
              if (Utils.ParseInt((object) str2) >= 10 && Utils.ParseInt((object) str2) < 98)
                this.SetCurrentField(str1 + index1.ToString("00") + str2, dictionary2.ContainsKey(str1 + "00" + str2) ? dictionary2[str1 + "00" + str2] : "");
              stringList2.Add(this.GetField(str1 + index1.ToString("00") + str2));
            }
            stringListList.Add(stringList2);
            stringList1.Add(field);
          }
        }
        string field1 = this.GetField("4462");
        IDisclosureTracking2015Log idisclosureTracking2015Log = this.GetLogList().GetLatestIDisclosureTracking2015Log(DisclosureTracking2015Log.DisclosureTrackingType.All);
        string str3 = !(field1 == "") || idisclosureTracking2015Log == null ? field1 : (idisclosureTracking2015Log.DisclosedForCD ? "CD" : "LE");
        if (stringList1.Count == dictionary1.Count)
        {
          if (changeOfCircumstance <= dictionary1.Count)
            return;
          if (intList1.Count > 0)
          {
            for (int index = intList1.Count - 1; index >= 0; --index)
              this.map.RemoveGoodFaithChangeOfCircumstance(intList1[index]);
          }
          this.calculator.CopyAlertCoCToLECDPage1(str3 == "CD");
        }
        else
        {
          bool complianceSetting = (bool) this.Settings.ComplianceSettings[(object) "Compliance.CountDay0ForRediscloseDueDate"];
          foreach (string key in dictionary1.Keys)
          {
            if (!stringList1.Contains(key))
            {
              List<string> stringList3 = new List<string>();
              for (int index = 0; index < DisclosureTracking2015Log.DiscloseCOCFields.Count; ++index)
              {
                DisclosureTracking2015Log.DiscloseCOCFields[index].Substring(DisclosureTracking2015Log.DiscloseCOCFields[index].Length - 2, 2);
                stringList3.Add(index == 0 ? key : "");
              }
              stringListList.Add(stringList3);
            }
          }
          this.RemoveAllGoodFaithChangeOfCircumstance();
          List<int> intList2 = new List<int>();
          for (int index3 = 0; index3 < stringListList.Count; ++index3)
          {
            List<string> stringList4 = stringListList[index3];
            int num = this.NewGoodFaithChangeOfCircumstance(stringList4[0]) + 1;
            for (int index4 = 0; index4 < DisclosureTracking2015Log.DiscloseCOCFields.Count; ++index4)
            {
              string str4 = DisclosureTracking2015Log.DiscloseCOCFields[index4].Substring(DisclosureTracking2015Log.DiscloseCOCFields[index4].Length - 2, 2);
              if (!(str4 == "01") && !(str4 == "98"))
              {
                if (str4 == "03" && (stringList4[index4] == "" || stringList4[index4] == "//"))
                {
                  List<string> stringList5 = stringList4;
                  int index5 = index4;
                  DateTime dateTime = DateTime.Today;
                  dateTime = dateTime.Date;
                  string str5 = dateTime.ToString("MM/dd/yyyy");
                  stringList5[index5] = str5;
                }
                else
                {
                  if (str4 == "04" && (stringList4[index4] == "" || stringList4[index4] == "//"))
                  {
                    intList2.Add(num);
                    continue;
                  }
                  if (Utils.ParseInt((object) str4) >= 9 && Utils.ParseInt((object) str4) < 98 && dictionary1.ContainsKey(this.GetField(str1 + num.ToString("00") + "01")))
                  {
                    Dictionary<string, string> dictionary3 = dictionary1[this.GetField(str1 + num.ToString("00") + "01")];
                    if (str4 == "09")
                    {
                      if (stringList4[index4] == "" && dictionary3.ContainsKey(str1 + "0009"))
                        stringList4[index4] = dictionary3[str1 + "0009"];
                    }
                    else
                      stringList4[index4] = dictionary3.ContainsKey(str1 + "00" + str4) ? dictionary3[str1 + "00" + str4] : "";
                  }
                }
                if (stringList4[index4] == "//")
                  stringList4[index4] = "";
                this.SetField(str1 + num.ToString("00") + str4, stringList4[index4]);
              }
            }
          }
          if (intList2.Count > 0)
          {
            for (int index = 0; index < intList2.Count; ++index)
              this.Calculator.CalculateRevisedDueDate(str1 + intList2[index].ToString("00") + "03", str1 + intList2[index].ToString("00") + "04", complianceSetting, true, false);
          }
          if (this.calculator == null)
            return;
          this.calculator.CopyAlertCoCToLECDPage1(str3 == "CD");
        }
      }
    }

    public void SetRequiredDataForEDS(
      Dictionary<string, string> conditionLetterForms = null,
      bool trimLoanXmlResponse = false)
    {
      string data1 = "";
      string str1 = "|";
      LoanData loanData = (LoanData) null;
      Mapping mapping = (Mapping) null;
      if (this.Calculator != null)
      {
        try
        {
          loanData = this.Calculator.CreateEDSCalculator();
          if (this.linkedData != null)
          {
            if (this.linkedData.Calculator != null)
            {
              LoanData edsCalculator = this.linkedData.Calculator.CreateEDSCalculator();
              loanData.LinkedData = edsCalculator;
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot create EDS Calculator from current loan due to error " + ex.Message);
          data1 = data1 + "Error: Cannot create EDS Calculator from current loan due to error " + ex.Message;
        }
        if (loanData != null)
        {
          loanData.AddLock("VASUMM.X138");
          mapping = loanData.GetMap();
          try
          {
            loanData.Calculator.CalculateProjectedPaymentTable(true);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate Projected Payment Table due to error " + ex.Message);
            data1 = data1 + "Error: Cannot generate Projected Payment Table due to error " + ex.Message;
          }
          try
          {
            mapping.SetEDSData(EDSDataType.PaymentSchedule_Amortization, (object) loanData.Calculator.GetPaymentSchedule(true));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate Amortization Payment Schedule due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot generate Amortization Payment Schedule due to error " + ex.Message;
          }
          try
          {
            mapping.SetEDSData(EDSDataType.PaymentSchedule_WorstCase, (object) loanData.Calculator.GetWorstCaseScenarioPaymentSchedule(true));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate Worst-case Scenario Payment Schedule due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot generate Worst-case Scenario Payment Schedule due to error " + ex.Message;
          }
          try
          {
            UCDXmlParser ucdXmlParser = new UCDXmlParser(loanData.Calculator.GetUCDXmlDocument(true, true));
            mapping.SetEDSData(EDSDataType.UCD_LE, (object) ucdXmlParser.ParseXml(true));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate UCD - LE due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot generate UCD - LE due to error " + ex.Message;
          }
          try
          {
            UCDXmlParser ucdXmlParser = new UCDXmlParser(loanData.Calculator.GetUCDXmlDocument(false, true));
            mapping.SetEDSData(EDSDataType.UCD_CD, (object) ucdXmlParser.ParseXml(false));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate UCD - CD due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot generate UCD - CD due to error " + ex.Message;
          }
          try
          {
            mapping.SetEDSData(EDSDataType.FundingFee, (object) loanData.Calculator.GetFundingFees(false));
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot perform Funding Fee calculation due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot perform Funding Fee calculation due to error " + ex.Message;
          }
          try
          {
            mapping.SetEDSData(EDSDataType.BalancingWorksheet, (object) loanData.Calculator.GetFundingBalancingWorksheet());
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot perform Balancing Worksheet calculation due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot perform Balancing Worksheet calculation due to error " + ex.Message;
          }
          try
          {
            Dictionary<string, string> data2 = loanData.Calculator.BuildHelocExampleSchedules();
            if (data2 != null)
              mapping.SetEDSData(EDSDataType.HELOC, (object) data2);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate HELOC due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot generate HELOC due to error " + ex.Message;
          }
          try
          {
            loanData.Calculator.FormCalculation("EDS.X5");
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot perform calculation for field EDS.X5 due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot perform calculation for field EDS.X5 due to error " + ex.Message;
          }
          try
          {
            loanData.Calculator.FormCalculation("LE1.XD8");
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot perform calculation for field LE1.XD8 due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot perform calculation for field LE1.XD8 due to error " + ex.Message;
          }
        }
      }
      else
        data1 = "Warning: Loan Calculator is not attached.";
      string str2 = "";
      if (this.Settings != null)
      {
        if (this.Settings.AllRoles != null)
        {
          try
          {
            foreach (RoleInfo allRole in this.Settings.AllRoles)
              str2 = str2 + (object) allRole.RoleID + "$" + allRole.Name + ":";
            goto label_45;
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot generate Role Information due to error " + ex.Message);
            data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot generate Role Information due to error " + ex.Message;
            goto label_45;
          }
        }
      }
      data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot read Roles from Encompass setting.";
label_45:
      try
      {
        int length = str2.LastIndexOf(":");
        loanData.SetCurrentField("EDS.X6", length > 0 ? str2.Substring(0, length) : "");
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot perform calculation for field EDS.X6 due to error " + ex.Message);
        data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot perform calculation for field EDS.X6 due to error " + ex.Message;
      }
      Dictionary<int, int> allUserFromLogs = this.getAllUserFromLogs();
      if (allUserFromLogs.Count > 0)
      {
        string str3 = "";
        Hashtable users = this.calculator.GetUsers(allUserFromLogs.Keys.ToArray<int>());
        foreach (object key in (IEnumerable) users.Keys)
        {
          string str4 = users[key].ToString();
          str3 = str3 + str4 + ":";
        }
        try
        {
          int length = str3.LastIndexOf(":");
          if (length > 0)
            loanData.SetCurrentField("EDS.X7", str3.Substring(0, length));
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot perform calculation for field EDS.X7 due to error " + ex.Message);
          data1 = data1 + (data1 != "" ? str1 : "") + "Error: Cannot perform calculation for field EDS.X7 due to error " + ex.Message;
        }
      }
      try
      {
        if (mapping != null)
        {
          if (conditionLetterForms != null)
            mapping.SetEDSData(EDSDataType.ConditionLetter, (object) conditionLetterForms);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot add ConditionLetter forms to EDS node due to error " + ex.Message);
      }
      try
      {
        mapping?.SetEDSData(EDSDataType.Error, (object) data1);
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot add error message to EDS node due to error " + ex.Message);
      }
      this.edsLoanXml = loanData.xmldoc;
      if (trimLoanXmlResponse)
      {
        try
        {
          this.RemoveOptimalBlueRecords();
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanData.sw, TraceLevel.Error, nameof (LoanData), "Error in function SetRequiredDataForEDS: Cannot remove OptimalBlue records due to the error " + ex.Message);
        }
      }
      try
      {
        loanData.Close();
      }
      catch (Exception ex)
      {
      }
    }

    private void RemoveOptimalBlueRecords()
    {
      XmlElement element = this.edsLoanXml.CreateElement("OptimalBlue");
      XmlNode oldChild = this.edsLoanXml.SelectSingleNode("LOAN_APPLICATION/EllieMae/OptimalBlue");
      oldChild?.ParentNode.ReplaceChild((XmlNode) element, oldChild);
      string[] strArray = new string[3]
      {
        "OPTIMAL.HISTORY",
        "OPTIMAL.REQUEST",
        "OPTIMAL.RESPONSE"
      };
      foreach (string str in strArray)
      {
        foreach (XmlNode selectNode in this.edsLoanXml.SelectNodes("LOAN_APPLICATION/EllieMae/RateLock/RequestLogs//*[@id='" + str + "']"))
        {
          XmlAttribute attribute = selectNode.Attributes["val"];
          if (attribute != null)
            selectNode.Attributes.Remove(attribute);
        }
      }
    }

    private Dictionary<int, int> getAllUserFromLogs()
    {
      Dictionary<int, int> allUserFromLogs = new Dictionary<int, int>();
      int num = 0;
      foreach (ConditionLog allCondition in this.GetLogList().GetAllConditions(ConditionType.Underwriting))
      {
        if (allCondition.ConditionType == ConditionType.Underwriting)
        {
          UnderwritingConditionLog underwritingConditionLog = (UnderwritingConditionLog) allCondition;
          if (!allUserFromLogs.ContainsKey(underwritingConditionLog.ForRoleID) && underwritingConditionLog.ForRoleID >= 0)
            allUserFromLogs.Add(underwritingConditionLog.ForRoleID, ++num);
        }
      }
      return allUserFromLogs;
    }

    public object GetEDSData(EDSDataType dataType) => this.map.GetEDSData(dataType);

    public void ClearEDSData()
    {
      this.map.ClearEDSData();
      this.SetCurrentField("EDS.X5", "");
      this.SetCurrentField("EDS.X6", "");
    }

    public static PipelineInfo.Alert GetCreditLimitRequired(LoanData loan)
    {
      PipelineInfo.Alert creditLimitRequired = (PipelineInfo.Alert) null;
      int exlcudingAlimonyJobExp = loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        Decimal num = Utils.ParseDecimal((object) loan.GetField(str + "31"), 0M);
        string field1 = loan.GetField(str + "08");
        string field2 = loan.GetField(str + "18");
        if (num == 0M && field1 == "HELOC" && field2 != "Y")
        {
          creditLimitRequired = new PipelineInfo.Alert(64, "", index.ToString(), DateTime.Today, (string) null, (string) null);
          break;
        }
      }
      return creditLimitRequired;
    }

    public static PipelineInfo.Alert GetLienPositionRequiredIfResubordinated(LoanData loan)
    {
      PipelineInfo.Alert ifResubordinated = (PipelineInfo.Alert) null;
      int exlcudingAlimonyJobExp = loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        string field1 = loan.GetField(str + "28");
        string field2 = loan.GetField(str + "29");
        string field3 = loan.GetField(str + "26");
        if ((string.IsNullOrEmpty(field1) || string.IsNullOrEmpty(field2)) && field3 == "Y")
        {
          ifResubordinated = new PipelineInfo.Alert(65, "", index.ToString(), DateTime.Today, (string) null, (string) null);
          break;
        }
      }
      return ifResubordinated;
    }

    public static PipelineInfo.Alert GetLienPositionRequiredIfSubjectProperty(LoanData loan)
    {
      PipelineInfo.Alert ifSubjectProperty = (PipelineInfo.Alert) null;
      int exlcudingAlimonyJobExp = loan.GetNumberOfLiabilitesExlcudingAlimonyJobExp();
      for (int index = 1; index <= exlcudingAlimonyJobExp; ++index)
      {
        string str = "FL" + index.ToString("00");
        string field1 = loan.GetField(str + "28");
        string field2 = loan.GetField(str + "29");
        string field3 = loan.GetField(str + "27");
        string field4 = loan.GetField(str + "18");
        if ((string.IsNullOrEmpty(field1) || string.IsNullOrEmpty(field2)) && field3 == "Y" && field4 != "Y")
        {
          ifSubjectProperty = new PipelineInfo.Alert(66, "", index.ToString(), DateTime.Today, (string) null, (string) null);
          break;
        }
      }
      return ifSubjectProperty;
    }

    public bool LoanHasLiabilityToBePaidoff() => this.map.LoanHasLiabilityToBePaidoff();

    public int GetNumberOfHelocHistoricalIndices() => this.map.GetNumberOfHelocHistoricalIndices();

    public int GetNumberOfSpecialFeatureCode() => this.map.GetNumberOfSpecialFeatureCodes();

    public int NewSpecialFeatureCode()
    {
      this.Dirty = true;
      return this.map.NewSpecialFeatureCode();
    }

    public void RemoveSpecialFeatureCodes()
    {
      this.Dirty = true;
      this.map.RemoveSpecialFeatureCodes();
    }

    public bool RemoveSpecialFeatureCodeAt(int n)
    {
      this.Dirty = true;
      return this.map.RemoveSpecialFeatureCodeAt(n);
    }

    public bool CompareTo(LoanData other, bool includeOperationalLog)
    {
      if (other == null)
        return false;
      using (Stream stream1 = this.ToStream(includeOperationalLog))
      {
        using (Stream stream2 = other.ToStream(includeOperationalLog))
        {
          if (stream1.Length != stream2.Length)
            return false;
          while (stream1.Position < stream1.Length)
          {
            if (stream1.ReadByte() != stream1.ReadByte())
              return false;
          }
        }
      }
      return true;
    }

    public string GetHomeCounselingUrl(
      string zipcode,
      string state,
      string city,
      out bool validGeo)
    {
      string str1 = "https://data.hud.gov/Housing_Counselor/";
      double num1 = 0.0;
      double num2 = 0.0;
      string field1 = this.GetField("HCSETTING.SERVICES");
      string str2 = this.GetField("HCSETTING.LANGUAGES");
      if (str2 == "")
        str2 = "ENG";
      string field2 = this.GetField("HCSETTING.DISTANCE");
      GeoCoordinate zipGeoCoordinate = ZipCodeUtils.GetZipGeoCoordinate(zipcode, state, city, "");
      if (zipGeoCoordinate != (GeoCoordinate) null)
      {
        num1 = zipGeoCoordinate.Latitude;
        num2 = zipGeoCoordinate.Longitude;
        validGeo = true;
      }
      else
        validGeo = false;
      return str1 + "searchByLocation?" + "Lat=" + HttpUtility.UrlEncode(num1.ToString()) + "&Long=" + HttpUtility.UrlEncode(num2.ToString()) + "&Distance=" + HttpUtility.UrlEncode(field2) + "&Services=" + field1 + "&Languages=" + str2;
    }

    public string GetProviderId(bool isGetFromLoan = true, Hashtable openedLockRequestSnapshot = null)
    {
      string field = Convert.ToString((openedLockRequestSnapshot ?? this.GetLogList().GetLastConfirmedLock()?.GetLockRequestSnapshot())?[(object) "5029"]);
      if (isGetFromLoan && string.IsNullOrWhiteSpace(field))
        field = this.GetField("5029");
      return field;
    }

    public enum FindFieldTypes
    {
      None,
      NewSelect,
      Existing,
    }
  }
}
