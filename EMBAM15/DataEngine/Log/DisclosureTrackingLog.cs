// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DisclosureTrackingLog
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DisclosureTrackingLog : 
    DisclosureTrackingBase,
    IDisclosureTracking2010Log,
    IDisclosureTrackingLog
  {
    private static readonly List<string> disclosureSnapshotFields = new List<string>((IEnumerable<string>) new string[13]
    {
      "36",
      "37",
      "68",
      "69",
      "11",
      "12",
      "14",
      "15",
      "799",
      "1401",
      "1109",
      "1206",
      "745"
    });
    public static readonly List<string> DisclosedGFEITEMIZATIONFIELDS = new List<string>((IEnumerable<string>) new string[228]
    {
      "1401",
      "1785",
      "L211",
      "L212",
      "L213",
      "L214",
      "L215",
      "L216",
      "L217",
      "L218",
      "L219",
      "NEWHUD.X12",
      "L228",
      "1621",
      "367",
      "389",
      "1620",
      "439",
      "NEWHUD.X13",
      "NEWHUD.X713",
      "1847",
      "1663",
      "NEWHUD.X715",
      "388",
      "1619",
      "3119",
      "NEWHUD.X15",
      "3",
      "NEWHUD.X688",
      "NEWHUD.X16",
      "617",
      "NEWHUD.X609",
      "624",
      "NEWHUD.X610",
      "L224",
      "NEWHUD.X611",
      "NEWHUD.X399",
      "NEWHUD.X612",
      "NEWHUD.X126",
      "NEWHUD.X662",
      "NEWHUD.X127",
      "NEWHUD.X663",
      "NEWHUD.X128",
      "NEWHUD.X664",
      "NEWHUD.X129",
      "NEWHUD.X665",
      "NEWHUD.X130",
      "NEWHUD.X666",
      "369",
      "NEWHUD.X667",
      "371",
      "NEWHUD.X668",
      "348",
      "NEWHUD.X669",
      "931",
      "NEWHUD.X670",
      "1390",
      "NEWHUD.X671",
      "410",
      "NEWHUD.X656",
      "1061",
      "436",
      "454",
      "NEWHUD.X702",
      "NEWHUD.X703",
      "NEWHUD.X704",
      "NEWHUD.X705",
      "NEWHUD.X706",
      "NEWHUD.X707",
      "NEWHUD.X726",
      "NEWHUD.X727",
      "NEWHUD.X728",
      "NEWHUD.X729",
      "NEWHUD.X769",
      "NEWHUD.X770",
      "NEWHUD.X771",
      "NEWHUD.X14",
      "NEWHUD.X714",
      "NEWHUD.X712",
      "NEWHUD.X718",
      "NEWHUD.X709",
      "NEWHUD.X710",
      "NEWHUD.X720",
      "SYS.X8",
      "332",
      "333",
      "NEWHUD.X701",
      "L244",
      "L245",
      "L248",
      "NEWHUD.X622",
      "L252",
      "NEWHUD.X650",
      "L251",
      "NEWHUD.X582",
      "NEWHUD.X585",
      "1956",
      "NEWHUD.X661",
      "1500",
      "NEWHUD.X586",
      "L259",
      "NEWHUD.X587",
      "1666",
      "NEWHUD.X588",
      "NEWHUD.X583",
      "NEWHUD.X589",
      "NEWHUD.X584",
      "1109",
      "1107",
      "1826",
      "1765",
      "1760",
      "1045",
      "2",
      "2978",
      "1209",
      "SYS.X11",
      "1757",
      "1199",
      "1198",
      "1201",
      "1200",
      "1205",
      "1775",
      "1753",
      "VAVOB.X72",
      "VASUMM.X49",
      "1751",
      "1752",
      "NEWHUD.X691",
      "NEWHUD.X349",
      "NEWHUD.X350",
      "NEWHUD.X351",
      "NEWHUD.X78",
      "1387",
      "230",
      "NEWHUD.X692",
      "1296",
      "232",
      "NEWHUD.X693",
      "1386",
      "231",
      "NEWHUD.X694",
      "L267",
      "L268",
      "NEWHUD.X695",
      "1388",
      "235",
      "NEWHUD.X696",
      "1628",
      "1629",
      "1630",
      "NEWHUD.X697",
      "660",
      "340",
      "253",
      "NEWHUD.X698",
      "661",
      "341",
      "254",
      "NEWHUD.X699",
      "558",
      "NEWHUD.X1709",
      "NEWHUD.X1707",
      "NEWHUD.X1706",
      "NEWHUD.X38",
      "NEWHUD.X202",
      "NEWHUD.X203",
      "NEWHUD.X210",
      "NEWHUD.X204",
      "NEWHUD.X39",
      "NEWHUD.X205",
      "NEWHUD.X211",
      "646",
      "1634",
      "NEWHUD.X206",
      "NEWHUD.X207",
      "NEWHUD.X208",
      "NEWHUD.X565",
      "NEWHUD.X209",
      "NEWHUD.X566",
      "1762",
      "NEWHUD.X567",
      "1767",
      "NEWHUD.X568",
      "1772",
      "NEWHUD.X569",
      "1777",
      "NEWHUD.X570",
      "2402",
      "2403",
      "2404",
      "2405",
      "2406",
      "2407",
      "2408",
      "NEWHUD.X214",
      "1636",
      "NEWHUD.X604",
      "NEWHUD.X731",
      "1637",
      "NEWHUD.X605",
      "1638",
      "NEWHUD.X606",
      "NEWHUD.X724",
      "373",
      "1640",
      "1643",
      "NEWHUD.X772",
      "NEWHUD.X40",
      "NEWHUD.X251",
      "NEWHUD.X42",
      "650",
      "NEWHUD.X44",
      "651",
      "NEWHUD.X46",
      "40",
      "NEWHUD.X48",
      "43",
      "NEWHUD.X50",
      "1782",
      "NEWHUD.X52",
      "1787",
      "NEWHUD.X54",
      "1792",
      "NEWHUD.X56",
      "NEWHUD.X252",
      "NEWHUD.X253"
    });
    public static readonly List<string> REGZTILFIELDS = new List<string>((IEnumerable<string>) new string[242]
    {
      "662",
      "3156",
      "3158",
      "3157",
      "3159",
      "3247",
      "1401",
      "1785",
      "799",
      "3121",
      "1206",
      "3246",
      "3122",
      "682",
      "1109",
      "2",
      "3",
      "1014",
      "4",
      "325",
      "1269",
      "1270",
      "1271",
      "1272",
      "1273",
      "1274",
      "1613",
      "1614",
      "1615",
      "1616",
      "1617",
      "1618",
      "1177",
      "1853",
      "SYS.X2",
      "423",
      "697",
      "696",
      "695",
      "694",
      "247",
      "2625",
      "689",
      "688",
      "1827",
      "1699",
      "SYS.X1",
      "1700",
      "2551",
      "2552",
      "2307",
      "691",
      "690",
      "1712",
      "1713",
      "698",
      "SYS.X6",
      "1962",
      "1176",
      "1677",
      "1266",
      "1267",
      "312",
      "763",
      "682",
      "1961",
      "1963",
      "1265",
      "1404",
      "1959",
      "1888",
      "1889",
      "1890",
      "1891",
      "1482",
      "1965",
      "1966",
      "1892",
      "1893",
      "1413",
      "1483",
      "1967",
      "1968",
      "1986",
      "1987",
      "1766",
      "1198",
      "1770",
      "1200",
      "1205",
      "1209",
      "2978",
      "1678",
      "GLOBAL.S1",
      "GLOBAL.S4",
      "GLOBAL.S7",
      "GLOBAL.S10",
      "GLOBAL.S13",
      "GLOBAL.S16",
      "GLOBAL.S19",
      "GLOBAL.S22",
      "GLOBAL.S25",
      "GLOBAL.S3",
      "GLOBAL.S6",
      "GLOBAL.S9",
      "GLOBAL.S12",
      "GLOBAL.S15",
      "GLOBAL.S18",
      "GLOBAL.S21",
      "GLOBAL.S24",
      "GLOBAL.S27",
      "1679",
      "1681",
      "1683",
      "1685",
      "1687",
      "1689",
      "1691",
      "1693",
      "1695",
      "GLOBAL.S2",
      "GLOBAL.S5",
      "GLOBAL.S8",
      "GLOBAL.S11",
      "GLOBAL.S14",
      "GLOBAL.S17",
      "GLOBAL.S20",
      "GLOBAL.S23",
      "GLOBAL.S26",
      "1680",
      "1682",
      "1684",
      "1686",
      "1688",
      "1690",
      "1692",
      "1694",
      "1696",
      "949",
      "1701",
      "1206",
      "948",
      "1207",
      "664",
      "663",
      "665",
      "666",
      "1702",
      "1698",
      "1242",
      "1697",
      "723",
      "676",
      "8",
      "1703",
      "1704",
      "1705",
      "1708",
      "1709",
      "1710",
      "1603",
      "671",
      "1707",
      "675",
      "670",
      "672",
      "674",
      "1719",
      "1854",
      "677",
      "680",
      "679",
      "REGZ_DAY",
      "REGZ_MONTH",
      "REGZ_YR",
      "2026",
      "2027",
      "3",
      "5",
      "3267",
      "3268",
      "3264",
      "3265",
      "3266",
      "696",
      "3",
      "5",
      "3264",
      "3265",
      "3266",
      "3267",
      "3268",
      "3274",
      "3275",
      "3285",
      "3278",
      "3279",
      "3280",
      "2625",
      "3286",
      "3283",
      "3284",
      "3289",
      "1177",
      "3281",
      "3282",
      "3269",
      "3270",
      "3271",
      "3272",
      "3273",
      "3276",
      "3277",
      "3278",
      "3279",
      "2625",
      "3281",
      "3282",
      "3283",
      "3284",
      "3289",
      "3287",
      "3288",
      "1677",
      "3287",
      "3288",
      "3291",
      "1613",
      "1679",
      "3285",
      "3286",
      "19",
      "608",
      "334",
      "3887",
      "SYS.X303",
      "4535",
      "4536",
      "4537",
      "4538",
      "4539",
      "4540"
    });
    public static readonly List<string> HUDGFEFIELDS = new List<string>((IEnumerable<string>) new string[204]
    {
      "3136",
      "3137",
      "3139",
      "3138",
      "3163",
      "3162",
      "3140",
      "3164",
      "761",
      "762",
      "3168",
      "3169",
      "3166",
      "3165",
      "3167",
      "1401",
      "1785",
      "364",
      "3170",
      "763",
      "682",
      "1109",
      "3",
      "4",
      "325",
      "2",
      "1014",
      "5",
      "1172",
      "1063",
      "19",
      "9",
      "608",
      "1267",
      "1266",
      "995",
      "994",
      "NEWHUD.X1",
      "NEWHUD.X2",
      "NEWHUD.X719",
      "NEWHUD.X3",
      "NEWHUD.X4",
      "1347",
      "NEWHUD.X217",
      "NEWHUD.X5",
      "NEWHUD.X555",
      "NEWHUD.X556",
      "NEWHUD.X6",
      "NEWHUD.X7",
      "NEWHUD.X8",
      "NEWHUD.X9",
      "NEWHUD.X10",
      "NEWHUD.X11",
      "675",
      "RE88395.X315",
      "1659",
      "RE88395.X121",
      "NEWHUD.X348",
      "HUD24",
      "1550",
      "NEWHUD.X12",
      "NEWHUD.X13",
      "NEWHUD.X14",
      "NEWHUD.X721",
      "NEWHUD.X722",
      "NEWHUD.X16",
      "NEWHUD.X17",
      "617",
      "NEWHUD.X609",
      "624",
      "NEWHUD.X610",
      "L224",
      "NEWHUD.X611",
      "NEWHUD.X399",
      "NEWHUD.X612",
      "L248",
      "NEWHUD.X622",
      "NEWHUD.X18",
      "NEWHUD.X19",
      "NEWHUD.X20",
      "NEWHUD.X21",
      "NEWHUD.X22",
      "NEWHUD.X23",
      "NEWHUD.X24",
      "NEWHUD.X25",
      "NEWHUD.X26",
      "NEWHUD.X27",
      "NEWHUD.X28",
      "NEWHUD.X29",
      "NEWHUD.X30",
      "NEWHUD.X31",
      "NEWHUD.X32",
      "NEWHUD.X33",
      "NEWHUD.X34",
      "NEWHUD.X35",
      "NEWHUD.X36",
      "NEWHUD.X37",
      "NEWHUD.X38",
      "NEWHUD.X401",
      "NEWHUD.X402",
      "NEWHUD.X403",
      "NEWHUD.X404",
      "NEWHUD.X405",
      "NEWHUD.X406",
      "NEWHUD.X407",
      "NEWHUD.X408",
      "NEWHUD.X409",
      "NEWHUD.X410",
      "NEWHUD.X411",
      "NEWHUD.X412",
      "NEWHUD.X413",
      "NEWHUD.X414",
      "NEWHUD.X559",
      "NEWHUD.X560",
      "NEWHUD.X561",
      "NEWHUD.X562",
      "NEWHUD.X642",
      "NEWHUD.X643",
      "NEWHUD.X999",
      "NEWHUD.X1000",
      "NEWHUD.X1002",
      "NEWHUD.X1003",
      "NEWHUD.X1005",
      "NEWHUD.X1006",
      "NEWHUD.X1008",
      "NEWHUD.X1009",
      "NEWHUD.X1011",
      "NEWHUD.X1012",
      "NEWHUD.X39",
      "NEWHUD.X40",
      "NEWHUD.X41",
      "NEWHUD.X42",
      "NEWHUD.X43",
      "NEWHUD.X44",
      "NEWHUD.X45",
      "NEWHUD.X46",
      "NEWHUD.X47",
      "NEWHUD.X48",
      "NEWHUD.X49",
      "NEWHUD.X50",
      "NEWHUD.X51",
      "NEWHUD.X52",
      "NEWHUD.X53",
      "NEWHUD.X54",
      "NEWHUD.X55",
      "NEWHUD.X56",
      "NEWHUD.X108",
      "NEWHUD.X109",
      "NEWHUD.X110",
      "NEWHUD.X111",
      "NEWHUD.X112",
      "NEWHUD.X113",
      "NEWHUD.X114",
      "NEWHUD.X115",
      "NEWHUD.X214",
      "NEWHUD.X76",
      "NEWHUD.X691",
      "NEWHUD.X349",
      "NEWHUD.X350",
      "NEWHUD.X351",
      "NEWHUD.X78",
      "NEWHUD.X701",
      "333",
      "332",
      "L244",
      "L245",
      "NEWHUD.X604",
      "NEWHUD.X724",
      "NEWHUD.X771",
      "NEWHUD.X772",
      "NEWHUD.X79",
      "NEWHUD.X80",
      "NEWHUD.X81",
      "NEWHUD.X82",
      "NEWHUD.X83",
      "NEWHUD.X84",
      "NEWHUD.X85",
      "NEWHUD.X86",
      "NEWHUD.X87",
      "NEWHUD.X88",
      "NEWHUD.X89",
      "NEWHUD.X90",
      "NEWHUD.X91",
      "NEWHUD.X653",
      "NEWHUD.X654",
      "NEWHUD.X119",
      "NEWHUD.X120",
      "NEWHUD.X121",
      "NEWHUD.X122",
      "NEWHUD.X123",
      "NEWHUD.X124",
      "NEWHUD.X655",
      "NEWHUD.X92",
      "NEWHUD.X93",
      "NEWHUD.X95",
      "NEWHUD.X96",
      "NEWHUD.X101",
      "NEWHUD.X102",
      "NEWHUD.X97",
      "NEWHUD.X103",
      "NEWHUD.X98",
      "NEWHUD.X99",
      "NEWHUD.X104",
      "NEWHUD.X105"
    });
    public static readonly string XmlType = "DisclosureTracking";
    private string disclosedByFullName = "";
    private string isDisclosed = "True";
    private string containGFE = "False";
    private string containTIL = "False";
    private DateTime receivedDate = DateTime.MinValue;
    private string isLocked = "False";
    private string isDisclosedAPRLocked = "False";
    private string isDisclosedFinanceChargeLocked = "False";
    private string isDisclosedDailyInterestLocked = "False";
    private string isDisclosedReceivedDateLocked = "False";
    private string lockedDisclosedByField = "";
    private string lockedDisclosedAPRField = "";
    private string lockedDisclosedFinanceChargeField = "";
    private string lockedDisclosedDailyInterestField = "";

    public DisclosureTrackingLog(
      DateTime date,
      LoanData loanData,
      bool containGFE,
      bool containTIL,
      bool manuallyCreated,
      bool containSafeHarbor)
      : base(date, loanData, manuallyCreated, containSafeHarbor, false)
    {
      if (containGFE)
        this.containGFE = "True";
      if (containTIL)
        this.containTIL = "True";
      this.extractLoanData(loanData);
    }

    public DisclosureTrackingLog(LogList log, XmlElement e)
      : base(log, e, false)
    {
      AttributeReader attributeReader = new AttributeReader(e);
      this.containGFE = attributeReader.GetString("ContainGFE");
      this.receivedDate = attributeReader.GetDate(nameof (ReceivedDate), DateTime.MinValue);
      this.isDisclosed = attributeReader.GetString(nameof (IsDisclosed));
      this.isLocked = attributeReader.GetString(nameof (IsLocked));
      this.containTIL = attributeReader.GetString("ContainTIL");
      this.isDisclosedAPRLocked = attributeReader.GetString(nameof (isDisclosedAPRLocked), false);
      this.disclosedByFullName = attributeReader.GetString(nameof (DisclosedByFullName));
      this.isDisclosedFinanceChargeLocked = attributeReader.GetString(nameof (isDisclosedFinanceChargeLocked), false);
      this.isDisclosedReceivedDateLocked = attributeReader.GetString(nameof (isDisclosedReceivedDateLocked), false);
      this.lockedDisclosedAPRField = attributeReader.GetString(nameof (lockedDisclosedAPRField), false);
      this.lockedDisclosedByField = attributeReader.GetString(nameof (lockedDisclosedByField), false);
      this.lockedDisclosedFinanceChargeField = attributeReader.GetString(nameof (lockedDisclosedFinanceChargeField), false);
      this.isDisclosedDailyInterestLocked = attributeReader.GetString(nameof (isDisclosedDailyInterestLocked), false);
      this.lockedDisclosedDailyInterestField = attributeReader.GetString(nameof (lockedDisclosedDailyInterestField), false);
      if (this.borrowerName == "")
        this.borrowerName = this.GetDisclosedField("36") + " " + this.GetDisclosedField("37");
      if (this.coBorrowerName == "")
        this.coBorrowerName = this.GetDisclosedField("68") + " " + this.GetDisclosedField("69");
      if (this.propertyAddress == "")
        this.propertyAddress = this.GetDisclosedField("11");
      if (this.propertyCity == "")
        this.propertyCity = this.GetDisclosedField("12");
      if (this.propertyState == "")
        this.propertyState = this.GetDisclosedField("14");
      if (this.propertyZip == "")
        this.propertyZip = this.GetDisclosedField("15");
      if (this.loanProgram == "")
        this.loanProgram = this.GetDisclosedField("1401");
      if (this.loanAmount == "")
        this.loanAmount = this.GetDisclosedField("2");
      if (this.applicationDate == DateTime.MinValue)
        this.applicationDate = Utils.ParseDate((object) this.GetDisclosedField("745"), DateTime.MinValue);
      this.MarkAsClean();
    }

    private void extractLoanData(LoanData loanData)
    {
      foreach (string disclosureSnapshotField in DisclosureTrackingLog.disclosureSnapshotFields)
      {
        if (!(disclosureSnapshotField == "799") || this.DisclosedForTIL)
          this.AddDisclosedLoanInfo(disclosureSnapshotField, loanData.GetField(disclosureSnapshotField));
      }
      if (this.DisclosedForSafeHarbor)
      {
        foreach (string disclosedSafeHarborField in DisclosureTrackingBase.GetDisclosedSafeHarborFields())
        {
          if (!DisclosureTrackingLog.disclosureSnapshotFields.Contains(disclosedSafeHarborField))
          {
            this.AddDisclosedLoanInfo(disclosedSafeHarborField, loanData.GetField(disclosedSafeHarborField));
            if (loanData.IsLocked(disclosedSafeHarborField) && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(disclosedSafeHarborField + "_LOCKED"))
              this.AddDisclosedLoanInfo(disclosedSafeHarborField + "_LOCKED", loanData.GetFieldFromCal(disclosedSafeHarborField));
          }
        }
      }
      if (this.DisclosedForGFE)
      {
        foreach (string str in DisclosureTrackingLog.DisclosedGFEITEMIZATIONFIELDS)
        {
          if (!DisclosureTrackingLog.disclosureSnapshotFields.Contains(str))
          {
            this.AddDisclosedLoanInfo(str, loanData.GetField(str));
            if (loanData.IsLocked(str) && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(str + "_LOCKED"))
              this.AddDisclosedLoanInfo(str + "_LOCKED", loanData.GetFieldFromCal(str));
          }
        }
        foreach (string str in DisclosureTrackingLog.HUDGFEFIELDS)
        {
          if (!DisclosureTrackingLog.DisclosedGFEITEMIZATIONFIELDS.Contains(str) && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(str))
          {
            this.AddDisclosedLoanInfo(str, loanData.GetField(str));
            if (loanData.IsLocked(str) && !DisclosureTrackingLog.DisclosedGFEITEMIZATIONFIELDS.Contains(str + "_LOCKED") && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(str + "_LOCKED"))
              this.AddDisclosedLoanInfo(str + "_LOCKED", loanData.GetFieldFromCal(str));
          }
        }
        try
        {
          foreach (string str in DisclosureTrackingLog.REGZTILFIELDS)
          {
            if (!DisclosureTrackingLog.DisclosedGFEITEMIZATIONFIELDS.Contains(str) && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(str) && !DisclosureTrackingLog.HUDGFEFIELDS.Contains(str))
            {
              this.AddDisclosedLoanInfo(str, loanData.GetField(str));
              if (loanData.IsLocked(str) && !DisclosureTrackingLog.DisclosedGFEITEMIZATIONFIELDS.Contains(str + "_LOCKED") && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(str + "_LOCKED"))
                this.AddDisclosedLoanInfo(str + "_LOCKED", loanData.GetFieldFromCal(str));
            }
          }
        }
        catch
        {
        }
      }
      else
      {
        try
        {
          foreach (string str in DisclosureTrackingLog.REGZTILFIELDS)
          {
            if (!DisclosureTrackingLog.disclosureSnapshotFields.Contains(str))
            {
              this.AddDisclosedLoanInfo(str, loanData.GetField(str));
              if (loanData.IsLocked(str) && !DisclosureTrackingLog.DisclosedGFEITEMIZATIONFIELDS.Contains(str + "_LOCKED") && !DisclosureTrackingLog.disclosureSnapshotFields.Contains(str + "_LOCKED"))
                this.AddDisclosedLoanInfo(str + "_LOCKED", loanData.GetFieldFromCal(str));
            }
          }
        }
        catch
        {
        }
      }
      this.AddDisclosedLoanInfo("edisclosureDisclosedMessage", "");
    }

    internal override void OnRecordAdd()
    {
      this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
      if (!this.DisclosedForGFE || !(this.Log.Loan.GetField("3168") == "Y"))
        return;
      if (this.Log.Loan.GetField("3166") != "")
        this.Comments = "Changed Circumstance - " + this.Log.Loan.GetField("3166");
      this.Log.Loan.SetField("3168", "N");
    }

    public override DateTime DisclosedDate
    {
      get => this.date;
      set
      {
        if (this.IsLocked)
          this.date = value;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public override string DisclosedByFullName
    {
      get => this.IsDisclosedByLocked ? this.lockedDisclosedByField : this.disclosedByFullName;
      set
      {
        if (this.IsDisclosedByLocked)
          this.lockedDisclosedByField = value;
        else
          this.disclosedByFullName = value;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public DisclosureTrackingBase.DisclosedMethod DisclosureMethod
    {
      get => this.method;
      set
      {
        this.method = value;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public override string DisclosedMethodName
    {
      get
      {
        switch (this.method)
        {
          case DisclosureTrackingBase.DisclosedMethod.ByMail:
            return "U.S. Mail";
          case DisclosureTrackingBase.DisclosedMethod.eDisclosure:
            return "eFolder eDisclosures";
          case DisclosureTrackingBase.DisclosedMethod.Fax:
            return "Fax";
          case DisclosureTrackingBase.DisclosedMethod.InPerson:
            return "In Person";
          case DisclosureTrackingBase.DisclosedMethod.Other:
            return "Other";
          default:
            return "U.S. Mail";
        }
      }
    }

    public override DateTime ReceivedDate
    {
      get
      {
        return this.IsDisclosedReceivedDateLocked ? this.lockedDisclosedReceivedDate : this.receivedDate;
      }
      set
      {
        if (this.IsDisclosedReceivedDateLocked)
          this.lockedDisclosedReceivedDate = value;
        else
          this.receivedDate = value;
        this.MarkAsDirty();
        if (!this.IsAttachedToLog)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
      }
    }

    public DateTime SetReceivedDateFromCalc
    {
      set
      {
        this.receivedDate = value;
        this.MarkAsDirty();
        if (!this.IsAttachedToLog)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
      }
    }

    public override bool IsDisclosedReceivedDateLocked
    {
      get => this.isDisclosedReceivedDateLocked == "True";
      set
      {
        this.isDisclosedReceivedDateLocked = !value ? "False" : "True";
        this.lockedDisclosedReceivedDate = this.ReceivedDate;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public string DisclosedAPR
    {
      get
      {
        return this.IsDisclosedAPRLocked ? this.lockedDisclosedAPRField : this.GetDisclosedField("799");
      }
      set
      {
        if (this.IsDisclosedAPRLocked)
          this.lockedDisclosedAPRField = value;
        if (!this.IsAttachedToLog)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
      }
    }

    public string FinanceCharge
    {
      get
      {
        return this.IsDisclosedFinanceChargeLocked ? this.lockedDisclosedFinanceChargeField : this.GetDisclosedField("1206");
      }
      set
      {
        if (this.IsDisclosedFinanceChargeLocked)
          this.lockedDisclosedFinanceChargeField = value;
        if (!this.IsAttachedToLog)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
      }
    }

    public string DisclosedDailyInterest
    {
      get
      {
        if (this.IsDisclosedDailyInterestLocked)
          return this.lockedDisclosedDailyInterestField;
        return string.Compare(this.GetDisclosedField("SYS.X303"), "Lender", true) == 0 ? string.Empty : this.GetDisclosedField("334");
      }
      set
      {
        if (this.IsDisclosedDailyInterestLocked)
          this.lockedDisclosedDailyInterestField = value;
        if (!this.IsAttachedToLog)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
      }
    }

    public bool IsDisclosed
    {
      get => this.isDisclosed == "True";
      set
      {
        this.isDisclosed = !value ? "False" : "True";
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public bool DisclosedForGFE => this.containGFE == "True";

    public bool DisclosedForTIL => this.containTIL == "True";

    public override bool IsLocked
    {
      get => this.isLocked == "True";
      set
      {
        if (value)
        {
          this.isLocked = "True";
          this.date = this.DisclosureCreatedDTTM;
        }
        else
        {
          this.isLocked = "False";
          this.date = DateTime.Parse(this.DisclosureCreatedDTTM.ToString("MM/dd/yyyy  HH:mm:ss"));
        }
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public bool IsDisclosedAPRLocked
    {
      get => this.isDisclosedAPRLocked == "True";
      set
      {
        this.isDisclosedAPRLocked = !value ? "False" : "True";
        this.lockedDisclosedAPRField = this.DisclosedAPR;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public bool IsDisclosedFinanceChargeLocked
    {
      get => this.isDisclosedFinanceChargeLocked == "True";
      set
      {
        this.isDisclosedFinanceChargeLocked = !value ? "False" : "True";
        this.lockedDisclosedFinanceChargeField = this.FinanceCharge;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public bool IsDisclosedDailyInterestLocked
    {
      get => this.isDisclosedDailyInterestLocked == "True";
      set
      {
        this.isDisclosedDailyInterestLocked = !value ? "False" : "True";
        this.lockedDisclosedDailyInterestField = this.DisclosedDailyInterest;
        if (this.IsAttachedToLog)
          this.Log.Loan.Calculator.CalculateLatestDisclosure(this);
        this.MarkAsDirty();
      }
    }

    public override void MarkAsClean() => base.MarkAsClean();

    public override bool IsDirty() => base.IsDirty();

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DisclosureTrackingLog.XmlType);
      if (this.receivedDate != DateTime.MinValue)
        attributeWriter.Write("ReceivedDate", (object) this.receivedDate.ToString("MM/dd/yyyy"));
      else
        attributeWriter.Write("ReceivedDate", (object) "");
      attributeWriter.Write("ContainGFE", (object) this.containGFE);
      attributeWriter.Write("ContainTIL", (object) this.containTIL);
      attributeWriter.Write("IsDisclosed", (object) this.isDisclosed);
      attributeWriter.Write("IsLocked", (object) this.isLocked);
      attributeWriter.Write("isDisclosedAPRLocked", (object) this.isDisclosedAPRLocked);
      attributeWriter.Write("isDisclosedFinanceChargeLocked", (object) this.isDisclosedFinanceChargeLocked);
      attributeWriter.Write("isDisclosedDailyInterestLocked", (object) this.isDisclosedDailyInterestLocked);
      attributeWriter.Write("isDisclosedReceivedDateLocked", (object) this.isDisclosedReceivedDateLocked);
      attributeWriter.Write("lockedDisclosedAPRField", (object) this.lockedDisclosedAPRField);
      attributeWriter.Write("lockedDisclosedByField", (object) this.lockedDisclosedByField);
      attributeWriter.Write("lockedDisclosedFinanceChargeField", (object) this.lockedDisclosedFinanceChargeField);
      attributeWriter.Write("lockedDisclosedDailyInterestField", (object) this.lockedDisclosedDailyInterestField);
      attributeWriter.Write("DisclosedByFullName", (object) this.disclosedByFullName);
      XmlElement element1 = e.OwnerDocument.CreateElement("Fields");
      for (int index = 0; index < this.loanDataList.Count; ++index)
      {
        DisclosedLoanItem loanData = this.loanDataList[index];
        XmlElement element2 = e.OwnerDocument.CreateElement("Field");
        element2.SetAttribute("FieldID", loanData.FieldID);
        element2.SetAttribute("FieldValue", loanData.FieldValue);
        element1.AppendChild((XmlNode) element2);
      }
      e.AppendChild((XmlNode) element1);
    }

    [Flags]
    public enum DisclosureTrackingType
    {
      None = 0,
      GFE = 1,
      TIL = 2,
      All = 255, // 0x000000FF
    }
  }
}
