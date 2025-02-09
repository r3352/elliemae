// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CalculationBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.CalculationServants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public abstract class CalculationBase : ILoanModelProvider
  {
    private const string className = "CalculationBase�";
    private static readonly string sw = Tracing.SwDataEngine;
    protected LoanData loan;
    protected EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs;
    protected static string nil = string.Empty;
    protected const string BORROWER = "Borrower�";
    protected const string COBORROWER = "CoBorrower�";
    protected const string BOTH = "Both�";
    protected static Hashtable BORHUDFIELDS = CollectionsUtil.CreateCaseInsensitiveHashtable();
    protected static Hashtable SELHUDFIELDS = CollectionsUtil.CreateCaseInsensitiveHashtable();
    protected static string[] line801PaidByFields = (string[]) null;
    private readonly LoanDisclosedChecker _loanDisclosedChecker;
    protected Stopwatch Stopwatch;
    protected static string[] BorrowerFields = new string[68]
    {
      "454",
      "1093",
      "641",
      "640",
      "329",
      "L228",
      "L230",
      "439",
      "336",
      "1621",
      "367",
      "1623",
      "370",
      "372",
      "349",
      "932",
      "1009",
      "334",
      "337",
      "642",
      "643",
      "1050",
      "L260",
      "1667",
      "656",
      "338",
      "L269",
      "655",
      "657",
      "1631",
      "658",
      "659",
      "387",
      "396",
      "392",
      "978",
      "385",
      "646",
      "1634",
      "390",
      "647",
      "648",
      "374",
      "1641",
      "1644",
      "339",
      "644",
      "645",
      "41",
      "554",
      "81",
      "155",
      "1625",
      "1763",
      "1768",
      "1773",
      "1778",
      "44",
      "1783",
      "1788",
      "1793",
      "1839",
      "1842",
      "1849",
      "383",
      "L288",
      "L291",
      "L294"
    };
    protected static string[] PaidByChecks = new string[68]
    {
      "251",
      "253",
      "255",
      "257",
      "259",
      "261",
      "263",
      "265",
      "267",
      "269",
      "271",
      "273",
      "275",
      "277",
      "279",
      "281",
      "283",
      "303",
      "305",
      "307",
      "309",
      "311",
      "313",
      "315",
      "317",
      "319",
      "321",
      "323",
      "325",
      "327",
      "329",
      "331",
      "333",
      "335",
      "337",
      "339",
      "341",
      "343",
      "345",
      "355",
      "357",
      "359",
      "361",
      "363",
      "365",
      "372",
      "374",
      "376",
      "378",
      "285",
      "287",
      "289",
      "291",
      "347",
      "349",
      "351",
      "353",
      "380",
      "382",
      "384",
      "386",
      "296",
      "301",
      "391",
      "370",
      "403",
      "405",
      "407"
    };
    protected static string[] PTBChecks = new string[68]
    {
      "252",
      "254",
      "256",
      "258",
      "260",
      "262",
      "264",
      "266",
      "268",
      "270",
      "272",
      "274",
      "276",
      "278",
      "280",
      "282",
      "284",
      "304",
      "306",
      "308",
      "310",
      "312",
      "314",
      "316",
      "318",
      "320",
      "322",
      "324",
      "326",
      "328",
      "330",
      "332",
      "334",
      "336",
      "338",
      "340",
      "342",
      "344",
      "346",
      "356",
      "358",
      "360",
      "362",
      "364",
      "366",
      "373",
      "375",
      "377",
      "379",
      "286",
      "288",
      "290",
      "292",
      "348",
      "350",
      "352",
      "354",
      "381",
      "383",
      "385",
      "387",
      "297",
      "302",
      "392",
      "371",
      "404",
      "406",
      "408"
    };
    protected static string[] PFCChecks = new string[68]
    {
      "17",
      "18",
      "19",
      "20",
      "21",
      "116",
      "117",
      "28",
      "22",
      "201",
      "23",
      "203",
      "30",
      "31",
      "32",
      "33",
      "34",
      "4",
      "38",
      "39",
      "40",
      "29",
      "118",
      "205",
      "42",
      "43",
      "119",
      "44",
      "45",
      "207",
      "46",
      "47",
      "15",
      "48",
      "49",
      "16",
      "50",
      "52",
      "209",
      "53",
      "54",
      "55",
      "56",
      "211",
      "213",
      "58",
      "61",
      "62",
      "63",
      "35",
      "36",
      "37",
      "215",
      "217",
      "219",
      "221",
      "223",
      "64",
      "225",
      "227",
      "229",
      "294",
      "299",
      "389",
      "57",
      "121",
      "122",
      "123"
    };
    protected static string[] FHAChecks = new string[68]
    {
      "65",
      "66",
      "67",
      "68",
      "69",
      "126",
      "127",
      "76",
      "70",
      "202",
      "71",
      "204",
      "78",
      "79",
      "80",
      "81",
      "82",
      "86",
      "87",
      "88",
      "89",
      "77",
      "128",
      "206",
      "91",
      "92",
      "129",
      "93",
      "94",
      "208",
      "95",
      "96",
      "97",
      "98",
      "99",
      "100",
      "101",
      "103",
      "210",
      "104",
      "105",
      "106",
      "107",
      "212",
      "214",
      "109",
      "112",
      "113",
      "114",
      "83",
      "84",
      "85",
      "216",
      "218",
      "220",
      "222",
      "224",
      "115",
      "226",
      "228",
      "230",
      "295",
      "300",
      "390",
      "108",
      "131",
      "132",
      "133"
    };
    protected static string[] POCChecks = new string[68]
    {
      "136",
      "137",
      "231",
      "232",
      "138",
      "139",
      "140",
      "147",
      "141",
      "233",
      "142",
      "234",
      "149",
      "150",
      "151",
      "152",
      "153",
      "157",
      "158",
      "159",
      "160",
      "235",
      "161",
      "238",
      "162",
      "163",
      "164",
      "165",
      "167",
      "239",
      "168",
      "169",
      "170",
      "174",
      "175",
      "176",
      "177",
      "181",
      "240",
      "182",
      "183",
      "184",
      "185",
      "241",
      "242",
      "187",
      "190",
      "191",
      "192",
      "154",
      "155",
      "156",
      "243",
      "244",
      "245",
      "246",
      "247",
      "193",
      "248",
      "249",
      "250",
      "293",
      "298",
      "388",
      "186",
      "171",
      "172",
      "173"
    };
    protected static string[] SellerFields = new string[68]
    {
      "559",
      "560",
      "581",
      "580",
      "557",
      "L229",
      "L231",
      "572",
      "565",
      "1622",
      "569",
      "1624",
      "574",
      "575",
      "96",
      "1345",
      "6",
      "561",
      "562",
      "578",
      "579",
      "571",
      "L261",
      "1668",
      "596",
      "563",
      "L270",
      "595",
      "597",
      "1632",
      "598",
      "599",
      "582",
      "583",
      "584",
      "1049",
      "585",
      "592",
      "1635",
      "587",
      "593",
      "594",
      "576",
      "1642",
      "1645",
      "564",
      "590",
      "591",
      "42",
      "678",
      "82",
      "200",
      "1626",
      "1764",
      "1769",
      "1774",
      "1779",
      "55",
      "1784",
      "1789",
      "1794",
      "1840",
      "1843",
      "1850",
      "577",
      "L289",
      "L292",
      "L295"
    };
    protected static string[] PaidToOthers = new string[68]
    {
      "4",
      "7",
      "10",
      "13",
      "16",
      "165",
      "167",
      "19",
      "22",
      "25",
      "28",
      "31",
      "2",
      "8",
      "14",
      "20",
      "26",
      "40",
      "43",
      "46",
      "239",
      "52",
      "57",
      "49",
      "146",
      "147",
      "148",
      "41",
      "47",
      "160",
      "62",
      "71",
      "74",
      "77",
      "80",
      "161",
      "83",
      "88",
      "75",
      "91",
      "94",
      "89",
      "99",
      "100",
      "169",
      "102",
      "171",
      "173",
      "175",
      "205",
      "207",
      "209",
      "211",
      "215",
      "217",
      "219",
      "221",
      "225",
      "227",
      "229",
      "231",
      "235",
      "237",
      "194",
      "241",
      "243",
      "245",
      "247"
    };
    protected static string[] PaidToBroker = new string[68]
    {
      "3",
      "6",
      "9",
      "12",
      "15",
      "166",
      "168",
      "18",
      "21",
      "24",
      "27",
      "30",
      "5",
      "11",
      "17",
      "156",
      "157",
      "39",
      "42",
      "45",
      "240",
      "51",
      "56",
      "48",
      "60",
      "63",
      "66",
      "44",
      "159",
      "59",
      "65",
      "70",
      "73",
      "76",
      "79",
      "72",
      "82",
      "87",
      "78",
      "90",
      "93",
      "163",
      "98",
      "164",
      "170",
      "101",
      "172",
      "174",
      "176",
      "206",
      "208",
      "210",
      "212",
      "216",
      "218",
      "220",
      "222",
      "226",
      "228",
      "230",
      "232",
      "236",
      "238",
      "195",
      "242",
      "244",
      "246",
      "248"
    };
    protected static string[] SEC32BorFields = new string[54]
    {
      "454",
      "1093",
      "641",
      "640",
      "329",
      "L228",
      "L230",
      "439",
      "336",
      "1621",
      "367",
      "1623",
      "370",
      "372",
      "349",
      "932",
      "1009",
      "387",
      "396",
      "392",
      "978",
      "385",
      "S32DISC.X148",
      "S32DISC.X149",
      "390",
      "647",
      "648",
      "374",
      "1641",
      "1644",
      "339",
      "644",
      "645",
      "41",
      "554",
      "81",
      "155",
      "1625",
      "1839",
      "1842",
      "1763",
      "1768",
      "1773",
      "1778",
      "44",
      "1783",
      "1788",
      "1793",
      "L288",
      "L291",
      "L294",
      "337",
      "338",
      "NEWHUD.X1708"
    };
    protected static string[] SEC32FirstChecks = new string[53]
    {
      "3",
      "4",
      "26",
      "28",
      "7",
      "128",
      "130",
      "15",
      "8",
      "102",
      "9",
      "104",
      "17",
      "18",
      "19",
      "20",
      "21",
      "29",
      "30",
      "31",
      "132",
      "33",
      "35",
      "106",
      "36",
      "37",
      "38",
      "39",
      "108",
      "110",
      "41",
      "44",
      "45",
      "46",
      "22",
      "23",
      "24",
      "126",
      "134",
      "136",
      "112",
      "114",
      "116",
      "118",
      "47",
      "120",
      "122",
      "124",
      "138",
      "140",
      "142",
      "144",
      "146"
    };
    protected static string[] SEC32SecondChecks = new string[53]
    {
      "54",
      "55",
      "76",
      "77",
      "58",
      "129",
      "131",
      "66",
      "59",
      "103",
      "60",
      "105",
      "68",
      "69",
      "70",
      "71",
      "72",
      "78",
      "79",
      "80",
      "133",
      "82",
      "84",
      "107",
      "85",
      "86",
      "87",
      "88",
      "109",
      "111",
      "90",
      "93",
      "94",
      "95",
      "73",
      "74",
      "75",
      "127",
      "135",
      "137",
      "113",
      "115",
      "117",
      "119",
      "96",
      "121",
      "123",
      "125",
      "139",
      "141",
      "143",
      "145",
      "147"
    };
    protected static string PREPAIDLIST = "|334|337|642|1849|643|1050|L260|1667|656|338|L269|655|657|1631|658|659|";
    protected internal static Hashtable SyncFields = new Hashtable();
    private ICalculationObjects _calculationObjects;

    protected BorrowerPair[] GetBorrowerPairs() => this.loan.GetBorrowerPairs();

    protected bool IsPrimaryPair()
    {
      return this.loan.CurrentBorrowerPair.Borrower.Id == this.loan.GetBorrowerPairs()[0].Borrower.Id;
    }

    public bool IsLocked(string id) => this.loan.IsLocked(id);

    public void AddLock(string id) => this.loan.AddLock(id);

    public void RemoveLock(string id) => this.loan.RemoveLock(id);

    [NoTrace]
    protected string Val(string id)
    {
      return id == CalculationBase.nil ? "" : this.loan.GetSimpleField(id);
    }

    [NoTrace]
    protected string ValOrg(string id)
    {
      return id == CalculationBase.nil ? "" : this.loan.GetOrgField(id);
    }

    [NoTrace]
    protected string Val(string id, BorrowerPair pair)
    {
      return id == CalculationBase.nil ? "" : this.loan.GetSimpleField(id, pair);
    }

    [NoTrace]
    protected Routine RoutineX(Routine routine)
    {
      if (!EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic)
        return new Routine(routine.Invoke);
      string methodName = routine.Target.ToString() + "." + routine.Method.Name;
      return new Routine(((Routine) ((fieldId, val) => this.LogStartCalc("", methodName)) + routine + (Routine) ((fieldId, val) => this.LogEndCalc("", methodName))).Invoke);
    }

    [NoTrace]
    protected Validation ValidationX(Validation validation)
    {
      if (!EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic)
        return new Validation(validation.Invoke);
      string methodName = validation.Target.ToString() + "." + validation.Method.Name;
      return new Validation(((Validation) ((fieldId, val) => this.LogStartValidation("", methodName)) + validation + (Validation) ((fieldId, val) => this.LogEndValidation("", methodName))).Invoke);
    }

    public void LogStartCalc(string id, string method)
    {
      if (this.calObjs == null)
        return;
      this.Stopwatch = new Stopwatch();
      this.Stopwatch.Start();
    }

    public void LogEndCalc(string id, string method)
    {
      if (this.calObjs == null)
        return;
      this.Stopwatch.Stop();
      this.calObjs.IncreaseTriggerCounter(method, this.Stopwatch.ElapsedMilliseconds);
    }

    public bool LogStartValidation(string id, string method)
    {
      if (this.calObjs != null)
      {
        this.Stopwatch = new Stopwatch();
        this.Stopwatch.Start();
      }
      return true;
    }

    public bool LogEndValidation(string id, string method)
    {
      if (this.calObjs != null)
      {
        this.Stopwatch.Stop();
        this.calObjs.IncreaseTriggerCounter(method, this.Stopwatch.ElapsedMilliseconds);
      }
      return true;
    }

    [NoTrace]
    public double Flt(string val)
    {
      try
      {
        return val == CalculationBase.nil ? 0.0 : double.Parse(val);
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: Flt  val: " + val + " " + ex.Message);
        return 0.0;
      }
    }

    [NoTrace]
    public double FltVal(string id, bool getUnlockedValue)
    {
      string orgField = this.loan.GetOrgField(id);
      try
      {
        return orgField == CalculationBase.nil ? 0.0 : double.Parse(orgField);
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: FltVal  id: " + id + " " + ex.Message);
        return 0.0;
      }
    }

    [NoTrace]
    public double FltVal(string id)
    {
      string simpleField = this.loan.GetSimpleField(id);
      try
      {
        return simpleField == CalculationBase.nil ? 0.0 : double.Parse(simpleField);
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: FltVal  id: " + id + " " + ex.Message);
        return 0.0;
      }
    }

    [NoTrace]
    protected double FltVal(string id, BorrowerPair pair)
    {
      string simpleField = this.loan.GetSimpleField(id, pair);
      try
      {
        return simpleField == CalculationBase.nil ? 0.0 : double.Parse(simpleField);
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: FltVal(id, pair)  id: " + id + " " + ex.Message);
        return 0.0;
      }
    }

    [NoTrace]
    protected int IntVal(string id)
    {
      string simpleField = this.loan.GetSimpleField(id);
      try
      {
        return simpleField == CalculationBase.nil ? 0 : Convert.ToInt32(double.Parse(simpleField));
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: IntVal  id: " + id + " " + ex.Message);
        return 0;
      }
    }

    [NoTrace]
    protected int IntVal(string id, BorrowerPair pair)
    {
      string simpleField = this.loan.GetSimpleField(id, pair);
      try
      {
        return simpleField == CalculationBase.nil ? 0 : Convert.ToInt32(double.Parse(simpleField));
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: IntVal(id, pair)  id: " + id + " " + ex.Message);
        return 0;
      }
    }

    [NoTrace]
    protected bool BoolVal(string id) => Utils.ParseBoolean((object) this.loan.GetSimpleField(id));

    [NoTrace]
    protected int Int(string val)
    {
      try
      {
        return val == CalculationBase.nil ? 0 : Convert.ToInt32(double.Parse(val));
      }
      catch (Exception ex)
      {
        Tracing.Log(CalculationBase.sw, TraceLevel.Error, nameof (CalculationBase), "Routine: Int  val: " + val + " " + ex.Message);
        return 0;
      }
    }

    [NoTrace]
    protected void RemoveCurrentLock(string id)
    {
      if (!this.loan.IsLocked(id))
        return;
      this.loan.RemoveCurrentLock(id);
    }

    [NoTrace]
    protected void SetVal(string id, string val) => this.loan.SetCurrentFieldFromCal(id, val);

    [NoTrace]
    protected void SetVal(string id, string val, BorrowerPair pair)
    {
      this.loan.SetCurrentFieldFromCal(id, val, pair);
    }

    [NoTrace]
    protected void SetNum(string id, double num)
    {
      if (num == 0.0)
        this.loan.SetFieldFromCal(id, CalculationBase.nil);
      else
        this.loan.SetFieldFromCal(id, num.ToString());
    }

    [NoTrace]
    protected string GetFieldFromCal(string id) => this.loan.GetFieldFromCal(id);

    protected void TriggerCalculation(string id) => this.loan.TriggerCalculation(id, this.Val(id));

    [NoTrace]
    public void SetCurrentNum(string id, double num) => this.SetCurrentNum(id, num, false);

    [NoTrace]
    public void SetCurrentNum(string id, double num, bool showZero)
    {
      if (num == 0.0 && !showZero)
        this.loan.SetCurrentFieldFromCal(id, CalculationBase.nil);
      else
        this.loan.SetCurrentFieldFromCal(id, num.ToString());
    }

    [NoTrace]
    public void SetCurrentNumOnly(string id, double num, bool showZero = false)
    {
      throw new NotImplementedException();
    }

    [NoTrace]
    public void SetValOnly(string fieldId, string fieldValue)
    {
      throw new NotImplementedException();
    }

    public void SetValInteractive(string id, string val) => throw new NotImplementedException();

    [NoTrace]
    protected void SetCurrentNum(string id, double num, BorrowerPair pair, bool showZero = false)
    {
      if (num == 0.0 && !showZero)
        this.loan.SetCurrentFieldFromCal(id, CalculationBase.nil, pair);
      else
        this.loan.SetCurrentFieldFromCal(id, num.ToString(), pair);
    }

    protected internal double GetNonAPRAmount(string aprID, string paidByID)
    {
      if (!HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID))
        return 0.0;
      string[] strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      string str1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POC];
      double nonAprAmount1 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
      if (str1 == "NEWHUD.X820" && this.Val("NEWHUD.X715") == "Include Origination Credit")
        return this.Val(aprID) != "Y" ? this.FltVal("1663") * -1.0 : 0.0;
      if (this.Val(aprID) != "Y")
        return nonAprAmount1;
      double num1 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORTOPAY]);
      double num2 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
      string str2 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
      double num3 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
      string str3 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
      if (nonAprAmount1 == 0.0)
        return 0.0;
      double nonAprAmount2 = nonAprAmount1 - num1;
      switch (str2)
      {
        case "":
        case "Other":
          nonAprAmount2 -= num2;
          break;
        case "Broker":
          if (this.calObjs.NewHudCal.UsingTableFunded)
            break;
          goto case "";
      }
      switch (str3)
      {
        case "":
        case "Other":
          nonAprAmount2 -= num3;
          break;
        case "Broker":
          if (this.calObjs.NewHudCal.UsingTableFunded)
            break;
          goto case "";
      }
      return nonAprAmount2;
    }

    protected internal double GetAPRAmount(string aprID, string paidByID)
    {
      if (!HUDGFE2010Fields.PAIDBYPOPTFIELDS.ContainsKey((object) paidByID) || this.Val(aprID) != "Y")
        return 0.0;
      string[] strArray = (string[]) HUDGFE2010Fields.PAIDBYPOPTFIELDS[(object) paidByID];
      string str1 = strArray[HUDGFE2010Fields.PTCPOCINDEX_POC];
      double aprAmount = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
      this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
      double num1 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCAMT]);
      string str2 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_POCPAIDBY]);
      double num2 = this.FltVal(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCAMT]);
      string str3 = this.Val(strArray[HUDGFE2010Fields.PTCPOCINDEX_PTCPAIDBY]);
      if (str1 == "NEWHUD.X820" && this.Val("NEWHUD.X715") == "Include Origination Credit")
      {
        double num3 = this.FltVal("1663") * -1.0;
        return this.Val("NEWHUD.X749") == string.Empty ? num3 : 0.0;
      }
      if (aprAmount <= 0.0)
        return 0.0;
      switch (str2)
      {
        case "Lender":
          aprAmount -= num1;
          break;
        case "Broker":
          if (!this.calObjs.NewHudCal.UsingTableFunded)
            break;
          goto case "Lender";
      }
      switch (str3)
      {
        case "Lender":
          aprAmount -= num2;
          break;
        case "Broker":
          if (!this.calObjs.NewHudCal.UsingTableFunded)
            break;
          goto case "Lender";
      }
      return aprAmount;
    }

    protected void AddFieldHandler(string fieldId, Routine routine)
    {
      this.loan.RegisterFieldValueChangeEventHandler(fieldId, routine);
    }

    protected void AddFieldValidation(string fieldId, Validation validation)
    {
      this.loan.RegisterFieldValueValidationEventHandler(fieldId, validation);
    }

    protected CalculationBase(LoanData l, EllieMae.EMLite.CalculationEngine.CalculationObjects calObjs)
    {
      this.loan = l;
      this.calObjs = calObjs;
      this._loanDisclosedChecker = new LoanDisclosedChecker((ILoanModelProvider) this);
    }

    protected string GetVerifBlockNo(string id) => this.GetVerifBlockNo(id, 2);

    protected string GetVerifBlockNo(string id, int prefixLength)
    {
      if (id == string.Empty)
        return string.Empty;
      int num = 4 + prefixLength;
      if (id.Length < num)
        return string.Empty;
      string verifBlockNo = id.Substring(prefixLength, 2);
      if (id.Length > num)
        verifBlockNo = id.Substring(prefixLength, 3);
      return verifBlockNo;
    }

    protected string GetVerifBlock(string id)
    {
      if (id == string.Empty || id.Length < 6)
        return string.Empty;
      string verifBlock = id.Substring(0, 4);
      if (id.Length > 6)
        verifBlock = id.Substring(0, 5);
      return verifBlock;
    }

    protected string GetVerifFieldID(string id)
    {
      if (id == string.Empty || id.Length < 6)
        return string.Empty;
      string verifFieldId = id.Substring(4, 2);
      if (id.Length > 6)
        verifFieldId = id.Substring(5, 2);
      return verifFieldId;
    }

    protected double ToDouble(string val)
    {
      if (!(val == string.Empty))
      {
        if (val != null)
        {
          try
          {
            return double.Parse(val);
          }
          catch
          {
            return 0.0;
          }
        }
      }
      return 0.0;
    }

    protected int ToInt(string val)
    {
      try
      {
        return Convert.ToInt32(val == string.Empty || val == null ? 0.0 : double.Parse(val));
      }
      catch
      {
        return 0;
      }
    }

    protected double EMRounding(double val, int decimals)
    {
      double num1 = val;
      try
      {
        Decimal num2 = (Decimal) val;
        Decimal num3 = (Decimal) Math.Pow(10.0, (double) decimals);
        num1 = (double) (Decimal.Floor(Math.Abs(num2) * num3 + 0.5M) / num3 * (Decimal) Math.Sign(num2));
      }
      catch
      {
      }
      return num1;
    }

    protected double Truncate(double val, int precision)
    {
      return Math.Truncate((double) ((Decimal) val * (Decimal) Math.Pow(10.0, (double) precision))) / Math.Pow(10.0, (double) precision);
    }

    protected internal static void CreateSyncFieldList()
    {
      lock (CalculationBase.SyncFields.SyncRoot)
      {
        if (CalculationBase.SyncFields.Count > 0)
          return;
        for (int index = 0; index < CalculationBase.PTBChecks.Length; ++index)
        {
          string str = "|" + CalculationBase.BorrowerFields[index] + "|" + CalculationBase.SellerFields[index] + "|RE88395.X" + CalculationBase.PaidToOthers[index] + "|RE88395.X" + CalculationBase.PaidToBroker[index] + "|SYS.X" + CalculationBase.PaidByChecks[index] + "|SYS.X" + CalculationBase.PTBChecks[index];
          if (!CalculationBase.SyncFields.ContainsKey((object) ("SYS.X" + CalculationBase.PTBChecks[index])))
            CalculationBase.SyncFields.Add((object) ("SYS.X" + CalculationBase.PTBChecks[index]), (object) ("PTBCheck" + str));
          if (!CalculationBase.SyncFields.ContainsKey((object) ("SYS.X" + CalculationBase.PaidByChecks[index])))
            CalculationBase.SyncFields.Add((object) ("SYS.X" + CalculationBase.PaidByChecks[index]), (object) ("PaidBy" + str));
          if (!CalculationBase.SyncFields.ContainsKey((object) CalculationBase.BorrowerFields[index]))
          {
            CalculationBase.SyncFields.Add((object) CalculationBase.BorrowerFields[index], (object) ("B" + str));
            if (CalculationBase.BorrowerFields[index] == "454")
            {
              CalculationBase.SyncFields.Add((object) "388", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "1619", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "1093")
            {
              CalculationBase.SyncFields.Add((object) "1061", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "436", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "439")
            {
              CalculationBase.SyncFields.Add((object) "389", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "1620", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "334")
              CalculationBase.SyncFields.Add((object) "332", (object) ("B" + str));
            else if (CalculationBase.BorrowerFields[index] == "642")
              CalculationBase.SyncFields.Add((object) "L251", (object) ("B" + str));
            else if (CalculationBase.BorrowerFields[index] == "656")
            {
              CalculationBase.SyncFields.Add((object) "1387", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "230", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "338")
            {
              CalculationBase.SyncFields.Add((object) "1296", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "232", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "L269")
            {
              CalculationBase.SyncFields.Add((object) "L267", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "L268", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "655")
            {
              CalculationBase.SyncFields.Add((object) "1386", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "231", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "657")
            {
              CalculationBase.SyncFields.Add((object) "1388", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "235", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "1631")
            {
              CalculationBase.SyncFields.Add((object) "1629", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "1630", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "658")
            {
              CalculationBase.SyncFields.Add((object) "340", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "253", (object) ("B" + str));
            }
            else if (CalculationBase.BorrowerFields[index] == "659")
            {
              CalculationBase.SyncFields.Add((object) "341", (object) ("B" + str));
              CalculationBase.SyncFields.Add((object) "254", (object) ("B" + str));
            }
          }
          if (!CalculationBase.SyncFields.ContainsKey((object) CalculationBase.SellerFields[index]))
            CalculationBase.SyncFields.Add((object) CalculationBase.SellerFields[index], (object) ("S" + str));
          if (!CalculationBase.SyncFields.ContainsKey((object) ("RE88395.X" + CalculationBase.PaidToOthers[index])))
            CalculationBase.SyncFields.Add((object) ("RE88395.X" + CalculationBase.PaidToOthers[index]), (object) ("PTO" + str));
          if (!CalculationBase.SyncFields.ContainsKey((object) ("RE88395.X" + CalculationBase.PaidToBroker[index])))
            CalculationBase.SyncFields.Add((object) ("RE88395.X" + CalculationBase.PaidToBroker[index]), (object) ("PTB" + str));
        }
        string str1 = "|NEWHUD.X215|NEWHUD.X218|RE882.X59|RE882.X58|NEWHUD.X221|NEWHUD.X244";
        CalculationBase.SyncFields.Add((object) "NEWHUD.X215", (object) ("B" + str1));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X221", (object) ("PaidBy" + str1));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X244", (object) ("PTBCheck" + str1));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X218", (object) ("S" + str1));
        CalculationBase.SyncFields.Add((object) "RE882.X59", (object) ("PTO" + str1));
        CalculationBase.SyncFields.Add((object) "RE882.X58", (object) ("PTB" + str1));
        string str2 = "|NEWHUD.X639|NEWHUD.X784|RE882.X12|RE882.X11|NEWHUD.X745|NEWHUD.X805";
        CalculationBase.SyncFields.Add((object) "NEWHUD.X639", (object) ("B" + str2));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X745", (object) ("PaidBy" + str2));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X805", (object) ("PTBCheck" + str2));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X784", (object) ("S" + str2));
        CalculationBase.SyncFields.Add((object) "RE882.X12", (object) ("PTO" + str2));
        CalculationBase.SyncFields.Add((object) "RE882.X11", (object) ("PTB" + str2));
        string str3 = "|NEWHUD.X645|NEWHUD.X782|RE882.X57|RE882.X56|NEWHUD.X743|NEWHUD.X803";
        CalculationBase.SyncFields.Add((object) "NEWHUD.X645", (object) ("B" + str3));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X743", (object) ("PaidBy" + str3));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X803", (object) ("PTBCheck" + str3));
        CalculationBase.SyncFields.Add((object) "NEWHUD.X782", (object) ("S" + str3));
        CalculationBase.SyncFields.Add((object) "RE882.X57", (object) ("PTO" + str3));
        CalculationBase.SyncFields.Add((object) "RE882.X56", (object) ("PTB" + str3));
      }
    }

    public bool checkIfSimpleInterest()
    {
      return this.Val("4749") == "Y" && this.Val("423") != "Biweekly" && this.Val("19") != "ConstructionOnly" && this.Val("1172") != "HELOC";
    }

    public string findConstInterestType()
    {
      string constInterestType = this.Val("1962");
      if (constInterestType == "")
        constInterestType = "360/360";
      return constInterestType;
    }

    public DateTime findFirstPaymentDate()
    {
      DateTime minValue = DateTime.MinValue;
      int num1 = this.IntVal("1176");
      int num2 = this.IntVal("4");
      int num3 = this.Val("CONST.X1") != "Y" ? num2 + num1 : num2;
      DateTime firstPaymentDate;
      if (this.Val("19") == "ConstructionToPermanent" && num3 != num2)
      {
        firstPaymentDate = Utils.ParseDate((object) this.Val("1963"));
        DateTime dateTime = Utils.ParseDate((object) this.Val("763"));
        if (firstPaymentDate == DateTime.MinValue)
        {
          if (dateTime == DateTime.MinValue)
            dateTime = DateTime.Today;
          firstPaymentDate = dateTime.AddMonths(num1 + 1);
        }
      }
      else
      {
        firstPaymentDate = Utils.ParseDate((object) this.Val("682"));
        if (firstPaymentDate == DateTime.MinValue)
          firstPaymentDate = DateTime.Now;
      }
      return firstPaymentDate;
    }

    public bool findSimpleInterestUse366ForLeapYear() => this.Val("4748") == "Y";

    public DateTime findClosingDateToUse()
    {
      string str = this.Val("1887");
      if (str == "" || str == "//")
        str = this.Val("748");
      if (str == "" || str == "//")
        str = this.Val("763");
      return Utils.ParseDate((object) str);
    }

    public bool IsLoanDisclosed()
    {
      return this._loanDisclosedChecker.IsLoanDisclosed(this.calObjs.ManualSyncItemization);
    }

    public bool UseNewGFEHUD
    {
      get
      {
        return this.loan.GetField("NEWHUD.X354") == "Y" || this.loan.IsTemplate || this.loan.GetField("3969") == "RESPA 2010 GFE and HUD-1";
      }
    }

    public bool UseNew2015GFEHUD => Utils.CheckIf2015RespaTila(this.loan.GetField("3969"));

    public bool USEURLA2020 => Utils.CheckIfURLA2020(this.loan.GetField("1825"));

    public bool UseNoPayment(double total)
    {
      string field = this.loan.GetField("3");
      return !string.IsNullOrEmpty(field) && this.Flt(field) == 0.0 && total == 0.0;
    }

    public bool UseLinkedLoanNoPayment(double total)
    {
      if (this.loan.LinkedData == null)
        return false;
      string field = this.loan.LinkedData.GetField("3");
      return !string.IsNullOrEmpty(field) && this.Flt(field) == 0.0 && total == 0.0;
    }

    internal bool IsSyncGFERequired
    {
      get
      {
        return (this.UseNewGFEHUD || this.UseNew2015GFEHUD) && (!this.IsLoanDisclosed() || this.IsLoanDisclosed() && !this.calObjs.StopSyncItemization);
      }
    }

    Hashtable ILoanModelProvider.BorHudFields => CalculationBase.BORHUDFIELDS;

    Hashtable ILoanModelProvider.SelHudFields => CalculationBase.SELHUDFIELDS;

    bool ILoanModelProvider.IsPrimaryPair() => this.IsPrimaryPair();

    int ILoanModelProvider.GetBorrowerPairsCount()
    {
      return ((IEnumerable<BorrowerPair>) this.GetBorrowerPairs()).Count<BorrowerPair>();
    }

    string ILoanModelProvider.Val(string fieldId) => this.Val(fieldId);

    string ILoanModelProvider.Val(string fieldId, int borrowerPairIndex)
    {
      BorrowerPair borrowerPair = this.loan.GetBorrowerPair(this.loan.GetPairID(borrowerPairIndex));
      return this.Val(fieldId, borrowerPair);
    }

    void ILoanModelProvider.SetVal(string fieldId, string fieldValue)
    {
      this.SetVal(fieldId, fieldValue);
    }

    void ILoanModelProvider.SetVal(string fieldId, string fieldValue, int borrowerPairIndex)
    {
      BorrowerPair borrowerPair = this.loan.GetBorrowerPair(this.loan.GetPairID(borrowerPairIndex));
      this.SetVal(fieldId, fieldValue, borrowerPair);
    }

    public ICalculationObjects CalculationObjects
    {
      get => this._calculationObjects ?? (ICalculationObjects) this.calObjs;
    }

    public void CalculationObjectsSetter(ICalculationObjects calculationObjects)
    {
      this._calculationObjects = calculationObjects;
    }

    public void TriggerCalculation(string fieldid, string fieldValue)
    {
      this.loan.TriggerCalculation(fieldid, fieldValue);
    }

    public virtual void FormCal(string id, string val)
    {
    }

    public string HMDAApplicationDate
    {
      get
      {
        return this.loan.Settings != null && this.loan.Settings.HMDAInfo != null ? this.loan.Settings.HMDAInfo.HMDAApplicationDate : (string) null;
      }
    }
  }
}
