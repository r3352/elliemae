// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.ClosingCost
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.VersionInterface15;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class ClosingCost : FieldDataTemplate
  {
    public static readonly string[] TemplateFields;
    private static readonly List<string> template2015Fields;
    private static readonly Dictionary<string, string> mapTable = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private bool for2010gfe;
    private string respaVersion = "";
    private static string[,] maps = new string[10, 2]
    {
      {
        "contactund",
        "984"
      },
      {
        "contactesc",
        "611"
      },
      {
        "contacttit",
        "416"
      },
      {
        "contactapp",
        "618"
      },
      {
        "contactatt",
        "VEND.X117"
      },
      {
        "contacthoi",
        "VEND.X162"
      },
      {
        "contactmic",
        "707"
      },
      {
        "contactflo",
        "VEND.X13"
      },
      {
        "contactcre",
        "625"
      },
      {
        "contactdoc",
        "VEND.X195"
      }
    };
    private static string[] sameIds = new string[1038]
    {
      "1804",
      "1093",
      "230",
      "232",
      "1785",
      "388",
      "1619",
      "454",
      "559",
      "SYS.X136",
      "SYS.X17",
      "SYS.X65",
      "SYS.X252",
      "SYS.X251",
      "1061",
      "436",
      "560",
      "SYS.X137",
      "SYS.X18",
      "SYS.X66",
      "641",
      "581",
      "SYS.X231",
      "SYS.X19",
      "SYS.X67",
      "640",
      "580",
      "SYS.X232",
      "SYS.X20",
      "SYS.X68",
      "L704",
      "329",
      "557",
      "SYS.X138",
      "SYS.X21",
      "SYS.X69",
      "L227",
      "L228",
      "L229",
      "SYS.X139",
      "SYS.X116",
      "SYS.X126",
      "REGZGFE.X7",
      "L230",
      "L231",
      "SYS.X140",
      "SYS.X117",
      "SYS.X127",
      "389",
      "1620",
      "439",
      "572",
      "SYS.X147",
      "SYS.X28",
      "SYS.X76",
      "L224",
      "336",
      "565",
      "SYS.X141",
      "SYS.X22",
      "SYS.X70",
      "1812",
      "1621",
      "1622",
      "SYS.X233",
      "SYS.X201",
      "SYS.X202",
      "367",
      "569",
      "SYS.X142",
      "SYS.X23",
      "SYS.X71",
      "1813",
      "1623",
      "1624",
      "SYS.X234",
      "SYS.X203",
      "SYS.X204",
      "369",
      "370",
      "574",
      "SYS.X149",
      "SYS.X30",
      "SYS.X78",
      "371",
      "372",
      "575",
      "SYS.X150",
      "SYS.X31",
      "SYS.X79",
      "348",
      "349",
      "96",
      "SYS.X151",
      "SYS.X32",
      "SYS.X80",
      "931",
      "932",
      "1345",
      "SYS.X152",
      "SYS.X33",
      "SYS.X81",
      "1390",
      "1009",
      "6",
      "SYS.X153",
      "SYS.X34",
      "SYS.X82",
      "410",
      "554",
      "678",
      "SYS.X154",
      "SYS.X35",
      "SYS.X83",
      "1391",
      "81",
      "82",
      "SYS.X155",
      "SYS.X36",
      "SYS.X84",
      "154",
      "155",
      "200",
      "SYS.X156",
      "SYS.X37",
      "SYS.X85",
      "1627",
      "1625",
      "1626",
      "SYS.X243",
      "SYS.X215",
      "SYS.X216",
      "1671",
      "1672",
      "332",
      "333",
      "334",
      "561",
      "SYS.X157",
      "SYS.X4",
      "SYS.X86",
      "337",
      "562",
      "SYS.X158",
      "SYS.X38",
      "SYS.X87",
      "L251",
      "642",
      "578",
      "SYS.X159",
      "SYS.X39",
      "SYS.X88",
      "643",
      "579",
      "SYS.X160",
      "SYS.X40",
      "SYS.X89",
      "1050",
      "571",
      "SYS.X235",
      "SYS.X29",
      "SYS.X77",
      "L259",
      "L260",
      "L261",
      "SYS.X161",
      "SYS.X118",
      "SYS.X128",
      "1666",
      "1667",
      "1668",
      "SYS.X238",
      "SYS.X205",
      "SYS.X206",
      "1387",
      "656",
      "596",
      "SYS.X162",
      "SYS.X42",
      "SYS.X91",
      "1296",
      "338",
      "563",
      "SYS.X163",
      "SYS.X43",
      "SYS.X92",
      "L267",
      "L268",
      "L269",
      "L270",
      "SYS.X164",
      "SYS.X119",
      "SYS.X129",
      "1386",
      "231",
      "655",
      "595",
      "SYS.X165",
      "SYS.X44",
      "SYS.X93",
      "1388",
      "235",
      "657",
      "597",
      "SYS.X167",
      "SYS.X45",
      "SYS.X94",
      "1628",
      "1629",
      "1630",
      "1631",
      "1632",
      "SYS.X239",
      "SYS.X207",
      "SYS.X208",
      "660",
      "340",
      "253",
      "658",
      "598",
      "SYS.X168",
      "SYS.X46",
      "SYS.X95",
      "661",
      "341",
      "254",
      "659",
      "599",
      "SYS.X169",
      "SYS.X47",
      "SYS.X96",
      "387",
      "582",
      "SYS.X170",
      "SYS.X15",
      "SYS.X97",
      "396",
      "583",
      "SYS.X174",
      "SYS.X48",
      "SYS.X98",
      "391",
      "392",
      "584",
      "SYS.X175",
      "SYS.X49",
      "SYS.X99",
      "978",
      "1049",
      "SYS.X176",
      "SYS.X16",
      "SYS.X100",
      "385",
      "585",
      "SYS.X177",
      "SYS.X50",
      "SYS.X101",
      "652",
      "646",
      "592",
      "SYS.X181",
      "SYS.X52",
      "SYS.X103",
      "1633",
      "1634",
      "1635",
      "SYS.X240",
      "SYS.X209",
      "SYS.X210",
      "1762",
      "1763",
      "1764",
      "SYS.X244",
      "SYS.X217",
      "SYS.X218",
      "1767",
      "1768",
      "1769",
      "SYS.X245",
      "SYS.X219",
      "SYS.X220",
      "1772",
      "1773",
      "1774",
      "SYS.X246",
      "SYS.X221",
      "SYS.X222",
      "1777",
      "1778",
      "1779",
      "SYS.X247",
      "SYS.X223",
      "SYS.X224",
      "1673",
      "1674",
      "1636",
      "390",
      "587",
      "SYS.X182",
      "SYS.X53",
      "SYS.X104",
      "1637",
      "647",
      "593",
      "SYS.X183",
      "SYS.X54",
      "SYS.X105",
      "1638",
      "648",
      "594",
      "SYS.X184",
      "SYS.X55",
      "SYS.X106",
      "373",
      "374",
      "576",
      "SYS.X185",
      "SYS.X56",
      "SYS.X107",
      "1640",
      "1641",
      "1642",
      "SYS.X241",
      "SYS.X211",
      "SYS.X212",
      "1643",
      "1644",
      "1645",
      "SYS.X242",
      "SYS.X213",
      "SYS.X214",
      "REGZGFE.X15",
      "339",
      "564",
      "SYS.X187",
      "SYS.X58",
      "SYS.X109",
      "650",
      "644",
      "590",
      "SYS.X190",
      "SYS.X61",
      "SYS.X112",
      "651",
      "645",
      "591",
      "SYS.X191",
      "SYS.X62",
      "SYS.X113",
      "40",
      "41",
      "42",
      "SYS.X192",
      "SYS.X63",
      "SYS.X114",
      "43",
      "44",
      "55",
      "SYS.X193",
      "SYS.X64",
      "SYS.X115",
      "1782",
      "1783",
      "1784",
      "SYS.X248",
      "SYS.X225",
      "SYS.X226",
      "1787",
      "1788",
      "1789",
      "SYS.X249",
      "SYS.X227",
      "SYS.X228",
      "1792",
      "1793",
      "1794",
      "SYS.X250",
      "SYS.X229",
      "SYS.X230",
      "1675",
      "1676",
      "1750",
      "1322",
      "1757",
      "1199",
      "1751",
      "1752",
      "1806",
      "1807",
      "1209",
      "USEREGZMI",
      "1838",
      "1839",
      "1840",
      "1841",
      "1842",
      "1843",
      "1662",
      "1664",
      "1847",
      "1848",
      "1849",
      "1850",
      "375",
      "383",
      "577",
      "SYS.X186",
      "SYS.X57",
      "SYS.X108",
      "SYS.X371",
      "L725",
      "L209",
      "L210",
      "L211",
      "L212",
      "L213",
      "L214",
      "L217",
      "L215",
      "L216",
      "SYS.X393",
      "L218",
      "L219",
      "SYS.X398",
      "REGZGFE.X5",
      "REGZGFE.X6",
      "448",
      "449",
      "REGZGFE.X14",
      "1878",
      "L244",
      "L245",
      "L287",
      "L288",
      "L289",
      "SYS.X403",
      "SYS.X171",
      "SYS.X121",
      "SYS.X131",
      "SYS.X404",
      "L290",
      "L291",
      "L292",
      "SYS.X405",
      "SYS.X172",
      "SYS.X122",
      "SYS.X132",
      "SYS.X406",
      "L293",
      "L294",
      "L295",
      "SYS.X407",
      "SYS.X173",
      "SYS.X123",
      "SYS.X133",
      "SYS.X408",
      "1879",
      "1880",
      "1955",
      "1956",
      "1663",
      "1665",
      "2402",
      "2403",
      "2404",
      "2405",
      "2406",
      "2407",
      "2408",
      "2409",
      "2410",
      "SYS.X8",
      "1107",
      "1826",
      "1765",
      "1760",
      "1757",
      "1199",
      "1198",
      "1201",
      "1200",
      "1205",
      "1775",
      "1753",
      "SYS.X116",
      "SYS.X201",
      "SYS.X23",
      "SYS.X28",
      "SYS.X37",
      "SYS.X215",
      "SYS.X294",
      "SYS.X299",
      "SYS.X318",
      "SYS.X320",
      "SYS.X322",
      "SYS.X324",
      "SYS.X326",
      "SYS.X328",
      "SYS.X330",
      "SYS.X332",
      "VEND.X341",
      "VEND.X350",
      "VEND.X359",
      "NEWHUD.X12",
      "NEWHUD.X13",
      "NEWHUD.X14",
      "NEWHUD.X15",
      "NEWHUD.X16",
      "NEWHUD.X126",
      "NEWHUD.X127",
      "NEWHUD.X128",
      "NEWHUD.X129",
      "NEWHUD.X130",
      "NEWHUD.X609",
      "NEWHUD.X610",
      "NEWHUD.X611",
      "NEWHUD.X612",
      "NEWHUD.X19",
      "NEWHUD.X21",
      "NEWHUD.X23",
      "NEWHUD.X25",
      "NEWHUD.X27",
      "NEWHUD.X29",
      "NEWHUD.X31",
      "NEWHUD.X33",
      "NEWHUD.X35",
      "NEWHUD.X37",
      "NEWHUD.X400",
      "NEWHUD.X136",
      "NEWHUD.X137",
      "NEWHUD.X138",
      "NEWHUD.X139",
      "NEWHUD.X140",
      "NEWHUD.X147",
      "NEWHUD.X148",
      "NEWHUD.X149",
      "NEWHUD.X150",
      "NEWHUD.X151",
      "NEWHUD.X582",
      "NEWHUD.X583",
      "NEWHUD.X584",
      "NEWHUD.X622",
      "NEWHUD.X79",
      "NEWHUD.X585",
      "NEWHUD.X586",
      "NEWHUD.X587",
      "NEWHUD.X588",
      "NEWHUD.X589",
      "NEWHUD.X591",
      "NEWHUD.X592",
      "NEWHUD.X593",
      "NEWHUD.X594",
      "NEWHUD.X595",
      "NEWHUD.X596",
      "NEWHUD.X349",
      "NEWHUD.X350",
      "NEWHUD.X78",
      "NEWHUD.X351",
      "NEWHUD.X202",
      "NEWHUD.X203",
      "NEWHUD.X204",
      "NEWHUD.X205",
      "NEWHUD.X206",
      "NEWHUD.X207",
      "NEWHUD.X208",
      "NEWHUD.X209",
      "NEWHUD.X38",
      "NEWHUD.X210",
      "NEWHUD.X39",
      "NEWHUD.X211",
      "NEWHUD.X565",
      "NEWHUD.X566",
      "NEWHUD.X567",
      "NEWHUD.X568",
      "NEWHUD.X569",
      "NEWHUD.X570",
      "NEWHUD.X571",
      "NEWHUD.X645",
      "NEWHUD.X572",
      "NEWHUD.X639",
      "NEWHUD.X640",
      "NEWHUD.X641",
      "NEWHUD.X215",
      "NEWHUD.X216",
      "NEWHUD.X218",
      "NEWHUD.X219",
      "NEWHUD.X214",
      "NEWHUD.X604",
      "NEWHUD.X730",
      "NEWHUD.X605",
      "NEWHUD.X606",
      "NEWHUD.X607",
      "NEWHUD.X251",
      "NEWHUD.X252",
      "NEWHUD.X253",
      "NEWHUD.X40",
      "NEWHUD.X42",
      "NEWHUD.X44",
      "NEWHUD.X46",
      "NEWHUD.X48",
      "NEWHUD.X50",
      "NEWHUD.X52",
      "NEWHUD.X54",
      "NEWHUD.X56",
      "NEWHUD.X603",
      "NEWHUD.X254",
      "NEWHUD.X255",
      "NEWHUD.X256",
      "NEWHUD.X258",
      "NEWHUD.X259",
      "NEWHUD.X260",
      "NEWHUD.X353",
      "NEWHUD.X178",
      "NEWHUD.X179",
      "NEWHUD.X180",
      "NEWHUD.X181",
      "NEWHUD.X182",
      "NEWHUD.X183",
      "NEWHUD.X627",
      "NEWHUD.X628",
      "NEWHUD.X189",
      "NEWHUD.X190",
      "NEWHUD.X191",
      "NEWHUD.X192",
      "NEWHUD.X193",
      "NEWHUD.X597",
      "NEWHUD.X598",
      "NEWHUD.X599",
      "NEWHUD.X600",
      "NEWHUD.X601",
      "NEWHUD.X629",
      "NEWHUD.X623",
      "NEWHUD.X624",
      "NEWHUD.X630",
      "NEWHUD.X625",
      "NEWHUD.X626",
      "NEWHUD.X646",
      "NEWHUD.X573",
      "NEWHUD.X576",
      "NEWHUD.X577",
      "NEWHUD.X578",
      "NEWHUD.X579",
      "NEWHUD.X580",
      "NEWHUD.X581",
      "NEWHUD.X233",
      "NEWHUD.X234",
      "NEWHUD.X235",
      "NEWHUD.X236",
      "NEWHUD.X237",
      "NEWHUD.X238",
      "NEWHUD.X239",
      "NEWHUD.X240",
      "NEWHUD.X241",
      "NEWHUD.X242",
      "NEWHUD.X249",
      "NEWHUD.X108",
      "NEWHUD.X109",
      "NEWHUD.X110",
      "NEWHUD.X111",
      "NEWHUD.X112",
      "NEWHUD.X113",
      "NEWHUD.X114",
      "NEWHUD.X115",
      "NEWHUD.X270",
      "NEWHUD.X271",
      "NEWHUD.X272",
      "NEWHUD.X273",
      "NEWHUD.X274",
      "NEWHUD.X275",
      "NEWHUD.X276",
      "NEWHUD.X244",
      "NEWHUD.X245",
      "NEWHUD.X650",
      "NEWHUD.X651",
      "NEWHUD.X647",
      "NEWHUD.X648",
      "NEWHUD.X649",
      "NEWHUD.X107",
      "NEWHUD.X661",
      "NEWHUD.X662",
      "NEWHUD.X663",
      "NEWHUD.X664",
      "NEWHUD.X665",
      "NEWHUD.X666",
      "NEWHUD.X667",
      "NEWHUD.X668",
      "NEWHUD.X669",
      "NEWHUD.X670",
      "NEWHUD.X671",
      "NEWHUD.X656",
      "NEWHUD.X657",
      "NEWHUD.X658",
      "NEWHUD.X659",
      "NEWHUD.X660",
      "NEWHUD.X686",
      "NEWHUD.X687",
      "NEWHUD.X688",
      "NEWHUD.X689",
      "NEWHUD.X690",
      "NEWHUD.X691",
      "NEWHUD.X692",
      "NEWHUD.X693",
      "NEWHUD.X694",
      "NEWHUD.X695",
      "NEWHUD.X696",
      "NEWHUD.X697",
      "NEWHUD.X698",
      "NEWHUD.X699",
      "NEWHUD.X701",
      "NEWHUD.X702",
      "NEWHUD.X703",
      "NEWHUD.X704",
      "NEWHUD.X705",
      "NEWHUD.X706",
      "NEWHUD.X707",
      "NEWHUD.X709",
      "NEWHUD.X710",
      "NEWHUD.X711",
      "NEWHUD.X712",
      "NEWHUD.X713",
      "NEWHUD.X715",
      "NEWHUD.X399",
      "NEWHUD.X731",
      "NEWHUD.X724",
      "NEWHUD.X726",
      "NEWHUD.X727",
      "NEWHUD.X728",
      "NEWHUD.X729",
      "NEWHUD.X748",
      "NEWHUD.X742",
      "NEWHUD.X157",
      "NEWHUD.X158",
      "NEWHUD.X159",
      "NEWHUD.X160",
      "NEWHUD.X161",
      "NEWHUD.X162",
      "NEWHUD.X163",
      "NEWHUD.X164",
      "NEWHUD.X165",
      "NEWHUD.X743",
      "NEWHUD.X744",
      "NEWHUD.X745",
      "NEWHUD.X746",
      "NEWHUD.X747",
      "NEWHUD.X221",
      "NEWHUD.X222",
      "NEWHUD.X261",
      "NEWHUD.X262",
      "NEWHUD.X263",
      "NEWHUD.X264",
      "NEWHUD.X732",
      "NEWHUD.X733",
      "NEWHUD.X734",
      "NEWHUD.X735",
      "NEWHUD.X736",
      "NEWHUD.X737",
      "NEWHUD.X738",
      "NEWHUD.X739",
      "NEWHUD.X740",
      "NEWHUD.X741",
      "NEWHUD.X749",
      "NEWHUD.X750",
      "NEWHUD.X751",
      "NEWHUD.X752",
      "NEWHUD.X753",
      "NEWHUD.X754",
      "NEWHUD.X755",
      "NEWHUD.X756",
      "NEWHUD.X757",
      "NEWHUD.X758",
      "NEWHUD.X759",
      "NEWHUD.X760",
      "NEWHUD.X761",
      "NEWHUD.X762",
      "NEWHUD.X763",
      "NEWHUD.X764",
      "NEWHUD.X765",
      "NEWHUD.X766",
      "NEWHUD.X767",
      "NEWHUD.X768",
      "NEWHUD.X769",
      "NEWHUD.X770",
      "NEWHUD.X771",
      "NEWHUD.X772",
      "NEWHUD.X116",
      "NEWHUD.X117",
      "NEWHUD.X779",
      "NEWHUD.X780",
      "NEWHUD.X781",
      "NEWHUD.X782",
      "NEWHUD.X783",
      "NEWHUD.X784",
      "NEWHUD.X787",
      "NEWHUD.X788",
      "NEWHUD.X803",
      "NEWHUD.X804",
      "NEWHUD.X805",
      "NEWHUD.X808",
      "NEWHUD.X809",
      "NEWHUD.X810",
      "NEWHUD.X811",
      "NEWHUD.X812",
      "NEWHUD.X813",
      "NEWHUD.X814",
      "NEWHUD.X815",
      "NEWHUD.X816",
      "NEWHUD.X817",
      "NEWHUD.X818",
      "NEWHUD.X819",
      "NEWHUD.X820",
      "NEWHUD.X947",
      "NEWHUD.X951",
      "NEWHUD.X952",
      "NEWHUD.X953",
      "NEWHUD.X954",
      "NEWHUD.X955",
      "NEWHUD.X956",
      "NEWHUD.X957",
      "NEWHUD.X958",
      "NEWHUD.X959",
      "NEWHUD.X960",
      "NEWHUD.X961",
      "NEWHUD.X962",
      "NEWHUD.X963",
      "NEWHUD.X964",
      "NEWHUD.X965",
      "NEWHUD.X966",
      "NEWHUD.X967",
      "NEWHUD.X968",
      "NEWHUD.X969",
      "NEWHUD.X970",
      "NEWHUD.X971",
      "NEWHUD.X972",
      "NEWHUD.X973",
      "NEWHUD.X974",
      "NEWHUD.X975",
      "NEWHUD.X976",
      "NEWHUD.X977",
      "NEWHUD.X978",
      "NEWHUD.X979",
      "NEWHUD.X980",
      "NEWHUD.X981",
      "NEWHUD.X982",
      "NEWHUD.X983",
      "NEWHUD.X984",
      "NEWHUD.X985",
      "NEWHUD.X986",
      "NEWHUD.X987",
      "NEWHUD.X988",
      "NEWHUD.X989",
      "NEWHUD.X990",
      "NEWHUD.X991",
      "NEWHUD.X992",
      "NEWHUD.X993",
      "NEWHUD.X994",
      "NEWHUD.X995",
      "NEWHUD.X996",
      "NEWHUD.X997",
      "NEWHUD.X998",
      "NEWHUD.X999",
      "NEWHUD.X1000",
      "NEWHUD.X1001",
      "NEWHUD.X1002",
      "NEWHUD.X1003",
      "NEWHUD.X1004",
      "NEWHUD.X1005",
      "NEWHUD.X1006",
      "NEWHUD.X1007",
      "NEWHUD.X1008",
      "NEWHUD.X1009",
      "NEWHUD.X1010",
      "NEWHUD.X1011",
      "NEWHUD.X1012",
      "NEWHUD.X1013",
      "NEWHUD.X1014",
      "NEWHUD.X1015",
      "NEWHUD.X1016",
      "NEWHUD.X1017",
      "NEWHUD.X223",
      "NEWHUD.X224",
      "NEWHUD.X226",
      "NEWHUD.X227",
      "NEWHUD.X228",
      "NEWHUD.X229",
      "NEWHUD.X230",
      "NEWHUD.X231",
      "NEWHUD.X232",
      "NEWHUD.X1045",
      "NEWHUD.X1046",
      "NEWHUD.X1047",
      "NEWHUD.X1048",
      "NEWHUD.X1049",
      "NEWHUD.X1050",
      "NEWHUD.X1051",
      "NEWHUD.X1052",
      "NEWHUD.X1053",
      "NEWHUD.X1054",
      "NEWHUD.X1055",
      "NEWHUD.X1056",
      "NEWHUD.X1057",
      "NEWHUD.X1058",
      "NEWHUD.X1059",
      "NEWHUD.X1060",
      "NEWHUD.X1061",
      "NEWHUD.X1062",
      "NEWHUD.X1063",
      "NEWHUD.X1064",
      "NEWHUD.X1065",
      "NEWHUD.X1066",
      "NEWHUD.X1070",
      "NEWHUD.X1071",
      "NEWHUD.X1072",
      "NEWHUD.X1073",
      "NEWHUD.X1074",
      "NEWHUD.X1075",
      "NEWHUD.X1076",
      "NEWHUD.X1077",
      "NEWHUD.X1078",
      "NEWHUD.X1079",
      "NEWHUD.X1080",
      "NEWHUD.X1081",
      "NEWHUD.X1082",
      "NEWHUD.X1083",
      "NEWHUD.X1084",
      "NEWHUD.X1085",
      "NEWHUD.X1086",
      "NEWHUD.X1087",
      "NEWHUD.X1088",
      "NEWHUD.X1089",
      "NEWHUD.X1090",
      "NEWHUD.X1091",
      "NEWHUD.X1092",
      "NEWHUD.X1093",
      "NEWHUD.X1094",
      "NEWHUD.X714",
      "NEWHUD.X246",
      "NEWHUD.X247",
      "NEWHUD.X250",
      "NEWHUD.X1067",
      "NEWHUD.X225",
      "NEWHUD.X1139",
      "NEWHUD.X1140",
      "NEWHUD.X1141",
      "NEWHUD.X1142",
      "NEWHUD.X1143",
      "NEWHUD.X1144",
      "NEWHUD.X1145",
      "NEWHUD.X1146",
      "NEWHUD.X1147",
      "NEWHUD.X1148",
      "NEWHUD.X1149",
      "NEWHUD.X1150",
      "NEWHUD.X1151",
      "NEWHUD.X1152",
      "NEWHUD.X1153",
      "NEWHUD.X1154",
      "NEWHUD.X1155",
      "NEWHUD.X1156",
      "NEWHUD.X1157",
      "NEWHUD.X1158",
      "NEWHUD.X1159",
      "NEWHUD.X1160",
      "NEWHUD.X1161",
      "NEWHUD.X1162",
      "NEWHUD.X1163",
      "NEWHUD.X1164",
      "NEWHUD.X1165",
      "NEWHUD.X1166",
      "NEWHUD.X1167",
      "NEWHUD.X1168",
      "NEWHUD.X1169",
      "NEWHUD.X1170",
      "NEWHUD.X1171",
      "NEWHUD.X1172",
      "NEWHUD.X1173",
      "NEWHUD.X1174",
      "NEWHUD.X1175",
      "NEWHUD.X1176",
      "NEWHUD.X1177",
      "NEWHUD.X1178",
      "NEWHUD.X1179",
      "NEWHUD.X1180",
      "NEWHUD.X1181",
      "NEWHUD.X1182",
      "NEWHUD.X1183",
      "NEWHUD.X1184",
      "NEWHUD.X1185",
      "NEWHUD.X1186",
      "NEWHUD.X1187",
      "NEWHUD.X1188",
      "NEWHUD.X1189",
      "NEWHUD.X1190",
      "NEWHUD.X1191",
      "NEWHUD.X1192",
      "NEWHUD.X1193",
      "NEWHUD.X1194",
      "NEWHUD.X1195",
      "NEWHUD.X1196",
      "NEWHUD.X1197",
      "NEWHUD.X1198",
      "NEWHUD.X1199",
      "NEWHUD.X1200",
      "NEWHUD.X1201",
      "NEWHUD.X1202",
      "NEWHUD.X1203",
      "NEWHUD.X1204",
      "NEWHUD.X1205",
      "NEWHUD.X1206",
      "NEWHUD.X1207",
      "NEWHUD.X1208",
      "NEWHUD.X1209",
      "NEWHUD.X1210",
      "NEWHUD.X1211",
      "NEWHUD.X1212",
      "NEWHUD.X1213",
      "NEWHUD.X1214",
      "NEWHUD.X1215",
      "NEWHUD.X1216",
      "NEWHUD.X1225",
      "NEWHUD.X1226",
      "NEWHUD.X1227",
      "NEWHUD.X1228",
      "NEWHUD.X1229",
      "NEWHUD.X1230",
      "NEWHUD.X1068",
      "ESCROW_TABLE",
      "TITLE_TABLE",
      "2010TITLE_TABLE",
      "NEWHUD.X1718",
      "2978",
      "3531",
      "3532",
      "3533",
      "3625",
      "3262",
      "SYS.X11",
      "NEWHUD.X1724",
      "NEWHUD.X1725"
    };
    public static List<string> SharedFields = new List<string>((IEnumerable<string>) new string[441]
    {
      "L211",
      "L212",
      "L213",
      "L214",
      "L217",
      "L215",
      "L216",
      "L218",
      "L219",
      "L228",
      "1621",
      "367",
      "389",
      "1620",
      "388",
      "1847",
      "1663",
      "617",
      "624",
      "L224",
      "1500",
      "369",
      "410",
      "554",
      "678",
      "SYS.X35",
      "SYS.X286",
      "1391",
      "81",
      "82",
      "SYS.X36",
      "SYS.X288",
      "371",
      "348",
      "931",
      "1390",
      "641",
      "640",
      "336",
      "370",
      "372",
      "349",
      "932",
      "1009",
      "574",
      "575",
      "96",
      "1345",
      "6",
      "SYS.X20",
      "SYS.X22",
      "SYS.X30",
      "SYS.X31",
      "SYS.X32",
      "SYS.X33",
      "SYS.X34",
      "SYS.X256",
      "SYS.X258",
      "SYS.X268",
      "SYS.X276",
      "SYS.X278",
      "SYS.X280",
      "SYS.X282",
      "SYS.X284",
      "SYS.X19",
      "154",
      "155",
      "1627",
      "1625",
      "SYS.X261",
      "SYS.X269",
      "SYS.X271",
      "SYS.X265",
      "SYS.X289",
      "SYS.X291",
      "SYS.X296",
      "SYS.X301",
      "SYS.X255",
      "SYS.X257",
      "SYS.X267",
      "SYS.X275",
      "SYS.X277",
      "SYS.X279",
      "SYS.X281",
      "SYS.X283",
      "SYS.X285",
      "1838",
      "1839",
      "1841",
      "1842",
      "SYS.X262",
      "SYS.X270",
      "SYS.X272",
      "SYS.X266",
      "SYS.X290",
      "SYS.X292",
      "SYS.X297",
      "SYS.X302",
      "SYS.X231",
      "SYS.X232",
      "SYS.X141",
      "SYS.X149",
      "SYS.X150",
      "SYS.X151",
      "SYS.X152",
      "SYS.X153",
      "SYS.X154",
      "SYS.X116",
      "SYS.X201",
      "SYS.X23",
      "SYS.X28",
      "SYS.X37",
      "SYS.X215",
      "SYS.X294",
      "SYS.X299",
      "SYS.X252",
      "SYS.X251",
      "436",
      "1061",
      "SYS.X17",
      "559",
      "L229",
      "1622",
      "569",
      "572",
      "200",
      "1626",
      "1840",
      "1843",
      "581",
      "580",
      "565",
      "454",
      "SYS.X136",
      "SYS.X139",
      "SYS.X233",
      "SYS.X142",
      "SYS.X147",
      "SYS.X156",
      "SYS.X243",
      "SYS.X293",
      "SYS.X298",
      "SYS.X8",
      "332",
      "L244",
      "L245",
      "L248",
      "L252",
      "L251",
      "230",
      "1956",
      "1500",
      "L259",
      "1666",
      "337",
      "642",
      "1050",
      "643",
      "L260",
      "1668",
      "SYS.X4",
      "SYS.X38",
      "SYS.X304",
      "SYS.X306",
      "SYS.X308",
      "SYS.X312",
      "SYS.X310",
      "SYS.X314",
      "SYS.X316",
      "561",
      "337",
      "562",
      "578",
      "571",
      "579",
      "L261",
      "1667",
      "SYS.X159",
      "SYS.X39",
      "SYS.X29",
      "SYS.X160",
      "SYS.X40",
      "SYS.X161",
      "SYS.X118",
      "SYS.X238",
      "SYS.X205",
      "SYS.X303",
      "SYS.X305",
      "SYS.X307",
      "SYS.X311",
      "SYS.X309",
      "SYS.X313",
      "SYS.X315",
      "SYS.X157",
      "SYS.X158",
      "SYS.X235",
      "1628",
      "660",
      "661",
      "1387",
      "1296",
      "1386",
      "L267",
      "1388",
      "1629",
      "340",
      "341",
      "230",
      "232",
      "231",
      "L268",
      "235",
      "1630",
      "253",
      "254",
      "SYS.X42",
      "SYS.X43",
      "SYS.X44",
      "SYS.X119",
      "SYS.X45",
      "SYS.X207",
      "SYS.X46",
      "SYS.X47",
      "SYS.X317",
      "SYS.X319",
      "SYS.X323",
      "SYS.X321",
      "SYS.X325",
      "SYS.X327",
      "SYS.X329",
      "SYS.X331",
      "VEND.X341",
      "VEND.X350",
      "VEND.X359",
      "596",
      "563",
      "595",
      "L270",
      "597",
      "1632",
      "598",
      "599",
      "SYS.X318",
      "SYS.X320",
      "SYS.X322",
      "SYS.X324",
      "SYS.X326",
      "SYS.X328",
      "SYS.X330",
      "SYS.X332",
      "646",
      "1634",
      "1763",
      "1768",
      "1773",
      "1778",
      "1764",
      "1769",
      "1774",
      "1779",
      "SYS.X217",
      "SYS.X219",
      "SYS.X221",
      "SYS.X223",
      "SYS.X348",
      "SYS.X350",
      "SYS.X352",
      "SYS.X354",
      "1762",
      "1767",
      "1772",
      "1777",
      "SYS.X347",
      "SYS.X349",
      "SYS.X351",
      "SYS.X353",
      "SYS.X244",
      "SYS.X245",
      "SYS.X246",
      "SYS.X247",
      "1636",
      "1637",
      "1638",
      "373",
      "1640",
      "1643",
      "390",
      "647",
      "648",
      "374",
      "1641",
      "1644",
      "576",
      "1642",
      "1645",
      "SYS.X53",
      "SYS.X54",
      "SYS.X55",
      "SYS.X56",
      "SYS.X211",
      "SYS.X213",
      "587",
      "SYS.X355",
      "SYS.X357",
      "SYS.X359",
      "SYS.X361",
      "SYS.X363",
      "SYS.X365",
      "SYS.X182",
      "SYS.X183",
      "SYS.X184",
      "SYS.X185",
      "SYS.X241",
      "SYS.X242",
      "593",
      "594",
      "650",
      "651",
      "40",
      "43",
      "1782",
      "1787",
      "1792",
      "644",
      "645",
      "41",
      "44",
      "1783",
      "1788",
      "1793",
      "590",
      "591",
      "42",
      "55",
      "1784",
      "1789",
      "1794",
      "SYS.X61",
      "SYS.X375",
      "SYS.X377",
      "SYS.X379",
      "SYS.X381",
      "SYS.X383",
      "SYS.X385",
      "SYS.X387",
      "SYS.X229",
      "SYS.X62",
      "SYS.X63",
      "SYS.X64",
      "SYS.X225",
      "SYS.X227",
      "SYS.X374",
      "SYS.X376",
      "SYS.X378",
      "SYS.X380",
      "SYS.X382",
      "SYS.X384",
      "SYS.X386",
      "SYS.X190",
      "SYS.X191",
      "SYS.X192",
      "SYS.X193",
      "SYS.X248",
      "SYS.X249",
      "SYS.X250",
      "1107",
      "1765",
      "2978",
      "1209",
      "SYS.X11",
      "1199",
      "1198",
      "1201",
      "1200",
      "1205",
      "1775",
      "1753",
      "VAVOB.X72",
      "VASUMM.X49",
      "1757",
      "3531",
      "3532",
      "3533",
      "3625",
      "3262",
      "SYS.X11",
      "1322",
      "1750",
      "1752",
      "1751",
      "2402",
      "2403",
      "2404",
      "2405",
      "2406",
      "2407",
      "2408",
      "619",
      "620",
      "1244",
      "621",
      "1246",
      "618",
      "974",
      "622",
      "89",
      "626",
      "627",
      "1245",
      "628",
      "1247",
      "625",
      "629",
      "90",
      "708",
      "709",
      "1252",
      "710",
      "707",
      "711",
      "93",
      "1254",
      "VEND.X157",
      "VEND.X158",
      "VEND.X159",
      "VEND.X160",
      "VEND.X162",
      "VEND.X163",
      "VEND.X164",
      "VEND.X165",
      "VEND.X13",
      "VEND.X14",
      "VEND.X15",
      "VEND.X16",
      "VEND.X17",
      "VEND.X19",
      "VEND.X20",
      "VEND.X21",
      "ESCROW_TABLE",
      "TITLE_TABLE",
      "2010TITLE_TABLE"
    });
    public static string[] GFEAppraisal = new string[11]
    {
      "617",
      "621",
      "1244",
      "620",
      "619",
      "1246",
      "89",
      "622",
      "974",
      "618",
      "168"
    };
    public static string[] GFECredit = new string[10]
    {
      "624",
      "628",
      "1245",
      "627",
      "626",
      "1247",
      "90",
      "629",
      "625",
      "VEND.X169"
    };
    public static string[] GFEUnderWriting = new string[10]
    {
      "REGZGFE.X8",
      "VEND.X174",
      "VEND.X173",
      "VEND.X172",
      "VEND.X171",
      "VEND.X176",
      "1411",
      "1410",
      "984",
      "VEND.X177"
    };
    public static string[] GFEMortIns = new string[10]
    {
      "L248",
      "710",
      "1252",
      "709",
      "708",
      "1254",
      "93",
      "711",
      "707",
      "VEND.X167"
    };
    public static string[] GFEHazIns = new string[10]
    {
      "L252",
      "VEND.X160",
      "VEND.X159",
      "VEND.X158",
      "VEND.X157",
      "VEND.X165",
      "VEND.X164",
      "VEND.X163",
      "VEND.X162",
      "VEND.X166"
    };
    public static string[] GFEFloodIns = new string[10]
    {
      "1500",
      "VEND.X17",
      "VEND.X16",
      "VEND.X15",
      "VEND.X14",
      "VEND.X20",
      "VEND.X21",
      "VEND.X19",
      "VEND.X13",
      "VEND.X22"
    };
    public static string[] GFEEscrowFee = new string[10]
    {
      "610",
      "614",
      "1175",
      "613",
      "612",
      "1011",
      "87",
      "615",
      "611",
      "186"
    };
    public static string[] GFEDocFee = new string[10]
    {
      "395",
      "VEND.X193",
      "VEND.X192",
      "VEND.X191",
      "VEND.X190",
      "VEND.X198",
      "VEND.X197",
      "VEND.X196",
      "VEND.X195",
      "VEND.X199"
    };
    public static string[] GFEAttorneyFee = new string[10]
    {
      "56",
      "VEND.X115",
      "VEND.X114",
      "VEND.X113",
      "VEND.X112",
      "VEND.X120",
      "VEND.X119",
      "VEND.X118",
      "VEND.X117",
      "VEND.X121"
    };
    public static string[] GFETitleFee = new string[10]
    {
      "411",
      "414",
      "1174",
      "413",
      "412",
      "1243",
      "88",
      "417",
      "416",
      "187"
    };
    public static string[] GFESellAttorney = new string[10]
    {
      "VEND.X122",
      "VEND.X126",
      "VEND.X125",
      "VEND.X124",
      "VEND.X123",
      "VEND.X131",
      "VEND.X130",
      "VEND.X129",
      "VEND.X128",
      "VEND.X132"
    };
    public static string[] GFEBuyerAgent = new string[10]
    {
      "VEND.X133",
      "VEND.X137",
      "VEND.X136",
      "VEND.X135",
      "VEND.X134",
      "VEND.X142",
      "VEND.X141",
      "VEND.X140",
      "VEND.X139",
      "VEND.X143"
    };
    public static string[] GFESellerAgent = new string[10]
    {
      "VEND.X144",
      "VEND.X148",
      "VEND.X147",
      "VEND.X146",
      "VEND.X145",
      "VEND.X153",
      "VEND.X152",
      "VEND.X151",
      "VEND.X150",
      "VEND.X154"
    };
    public static string[] GFESeller = new string[9]
    {
      "638",
      "703",
      "1249",
      "702",
      "701",
      "1251",
      "VEND.X220",
      "92",
      "704"
    };
    public static string[] GFEBuilder = new string[10]
    {
      "713",
      "717",
      "1253",
      "716",
      "715",
      "1255",
      "94",
      "718",
      "714",
      "972"
    };
    public static string[] GFESurveyor = new string[10]
    {
      "VEND.X34",
      "VEND.X39",
      "VEND.X38",
      "VEND.X37",
      "VEND.X36",
      "VEND.X42",
      "VEND.X43",
      "VEND.X41",
      "VEND.X35",
      "VEND.X168"
    };
    public static string[] GFEServicing = new string[10]
    {
      "VEND.X178",
      "VEND.X182",
      "VEND.X181",
      "VEND.X180",
      "VEND.X179",
      "VEND.X187",
      "VEND.X186",
      "VEND.X185",
      "VEND.X184",
      "VEND.X188"
    };
    public static string[] GFEWarehouse = new string[10]
    {
      "VEND.X200",
      "VEND.X204",
      "VEND.X203",
      "VEND.X202",
      "VEND.X201",
      "VEND.X209",
      "VEND.X208",
      "VEND.X207",
      "VEND.X206",
      "VEND.X210"
    };
    public static string[] GFEPlanner = new string[10]
    {
      "VEND.X44",
      "VEND.X49",
      "VEND.X48",
      "VEND.X47",
      "VEND.X46",
      "VEND.X52",
      "VEND.X53",
      "VEND.X51",
      "VEND.X45",
      "VEND.X211"
    };
    public static string[] GFEInvestor = new string[12]
    {
      "VEND.X263",
      "VEND.X267",
      "VEND.X266",
      "VEND.X265",
      "VEND.X264",
      "VEND.X274",
      "VEND.X273",
      "VEND.X272",
      "VEND.X271",
      "VEND.X275",
      "VEND.X276",
      "VEND.X268"
    };
    public static string[] GFEAssignTo = new string[12]
    {
      "VEND.X278",
      "VEND.X282",
      "VEND.X281",
      "VEND.X280",
      "VEND.X279",
      "VEND.X283",
      "VEND.X289",
      "VEND.X288",
      "VEND.X287",
      "VEND.X286",
      "VEND.X290",
      "VEND.X291"
    };
    public static string[] GFEBroker = new string[14]
    {
      "VEND.X293",
      "VEND.X297",
      "VEND.X296",
      "VEND.X295",
      "VEND.X294",
      "VEND.X299",
      "VEND.X300",
      "VEND.X306",
      "VEND.X305",
      "VEND.X304",
      "VEND.X303",
      "VEND.X302",
      "VEND.X307",
      "VEND.X308"
    };
    public static string[] GFEPreparedBy = new string[10]
    {
      "VEND.X310",
      "VEND.X314",
      "VEND.X313",
      "VEND.X312",
      "VEND.X311",
      "VEND.X320",
      "VEND.X319",
      "VEND.X318",
      "VEND.X317",
      "VEND.X321"
    };
    public static string[] GFECustom1 = new string[10]
    {
      "VEND.X54",
      "VEND.X59",
      "VEND.X58",
      "VEND.X57",
      "VEND.X56",
      "VEND.X62",
      "VEND.X63",
      "VEND.X61",
      "VEND.X55",
      "VEND.X213"
    };
    public static string[] GFECustom2 = new string[10]
    {
      "VEND.X64",
      "VEND.X69",
      "VEND.X68",
      "VEND.X67",
      "VEND.X66",
      "VEND.X72",
      "VEND.X73",
      "VEND.X71",
      "VEND.X65",
      "VEND.X215"
    };
    public static string[] GFECustom3 = new string[10]
    {
      "VEND.X74",
      "VEND.X79",
      "VEND.X78",
      "VEND.X77",
      "VEND.X76",
      "VEND.X82",
      "VEND.X83",
      "VEND.X81",
      "VEND.X75",
      "VEND.X217"
    };
    public static string[] GFECustom4 = new string[10]
    {
      "VEND.X1",
      "VEND.X6",
      "VEND.X5",
      "VEND.X4",
      "VEND.X3",
      "VEND.X9",
      "VEND.X10",
      "VEND.X8",
      "VEND.X2",
      "VEND.X219"
    };

    public static string[] Template2015Fields => ClosingCost.template2015Fields.ToArray();

    public static string[] AllTemplateFields
    {
      get
      {
        List<string> stringList = new List<string>();
        stringList.AddRange((IEnumerable<string>) ClosingCost.TemplateFields);
        stringList.AddRange((IEnumerable<string>) ClosingCost.template2015Fields);
        return stringList.ToArray();
      }
    }

    static ClosingCost()
    {
      for (int index = 0; index <= ClosingCost.maps.GetUpperBound(0); ++index)
        ClosingCost.mapTable[ClosingCost.maps[index, 1]] = ClosingCost.maps[index, 0];
      foreach (string sameId in ClosingCost.sameIds)
        ClosingCost.mapTable[sameId.Trim()] = sameId.Trim();
      for (int index = 1235; index <= 1418; ++index)
        ClosingCost.mapTable["NEWHUD.X" + (object) index] = "NEWHUD.X" + (object) index;
      for (int index = 1525; index <= 1540; ++index)
        ClosingCost.mapTable["NEWHUD.X" + (object) index] = "NEWHUD.X" + (object) index;
      for (int index = 1586; index <= 1663; ++index)
        ClosingCost.mapTable["NEWHUD.X" + (object) index] = "NEWHUD.X" + (object) index;
      for (int index = 1705; index <= 1715; ++index)
        ClosingCost.mapTable["NEWHUD.X" + (object) index] = "NEWHUD.X" + (object) index;
      for (int index = 4610; index <= 4644; ++index)
        ClosingCost.mapTable["NEWHUD2.X" + (object) index] = "NEWHUD2.X" + (object) index;
      for (int index = 251; index <= 392; ++index)
        ClosingCost.mapTable["SYS.X" + index.ToString()] = "SYS.X" + index.ToString();
      ClosingCost.mapTable.Remove("SYS.X367");
      ClosingCost.mapTable.Remove("SYS.X368");
      ClosingCost.mapTable.Remove("SYS.X369");
      for (int index = 821; index <= 946; ++index)
        ClosingCost.mapTable["NEWHUD.X" + index.ToString()] = "NEWHUD.X" + index.ToString();
      for (int index = 1433; index <= 1478; ++index)
        ClosingCost.mapTable["NEWHUD.X" + index.ToString()] = "NEWHUD.X" + index.ToString();
      for (int index = 1664; index <= 1683; ++index)
        ClosingCost.mapTable["NEWHUD.X" + index.ToString()] = "NEWHUD.X" + index.ToString();
      for (int index = 1005; index <= 1016; ++index)
        ClosingCost.mapTable["NEWHUD.X" + index.ToString()] = "NEWHUD.X" + index.ToString();
      for (int index = 1; index <= 234; ++index)
        ClosingCost.mapTable["PTC.X" + index.ToString()] = "PTC.X" + index.ToString();
      for (int index = 247; index <= 315; ++index)
        ClosingCost.mapTable["PTC.X" + index.ToString()] = "PTC.X" + index.ToString();
      for (int index = 247; index <= 348; ++index)
        ClosingCost.mapTable["PTC.X" + index.ToString()] = "PTC.X" + index.ToString();
      for (int index = 1; index <= 234; ++index)
        ClosingCost.mapTable["POPT.X" + index.ToString()] = "POPT.X" + index.ToString();
      for (int index = 247; index <= 315; ++index)
        ClosingCost.mapTable["POPT.X" + index.ToString()] = "POPT.X" + index.ToString();
      for (int index = 247; index <= 348; ++index)
        ClosingCost.mapTable["POPT.X" + index.ToString()] = "POPT.X" + index.ToString();
      for (int index = 0; index < ClosingCost.GFEAppraisal.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEAppraisal[index]] = ClosingCost.GFEAppraisal[index];
      for (int index = 0; index < ClosingCost.GFECredit.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFECredit[index]] = ClosingCost.GFECredit[index];
      for (int index = 0; index < ClosingCost.GFEUnderWriting.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEUnderWriting[index]] = ClosingCost.GFEUnderWriting[index];
      for (int index = 0; index < ClosingCost.GFEMortIns.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEMortIns[index]] = ClosingCost.GFEMortIns[index];
      for (int index = 0; index < ClosingCost.GFEHazIns.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEHazIns[index]] = ClosingCost.GFEHazIns[index];
      for (int index = 0; index < ClosingCost.GFEFloodIns.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEFloodIns[index]] = ClosingCost.GFEFloodIns[index];
      for (int index = 0; index < ClosingCost.GFEEscrowFee.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEEscrowFee[index]] = ClosingCost.GFEEscrowFee[index];
      for (int index = 0; index < ClosingCost.GFEDocFee.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEDocFee[index]] = ClosingCost.GFEDocFee[index];
      for (int index = 0; index < ClosingCost.GFEAttorneyFee.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFEAttorneyFee[index]] = ClosingCost.GFEAttorneyFee[index];
      for (int index = 0; index < ClosingCost.GFETitleFee.Length; ++index)
        ClosingCost.mapTable[ClosingCost.GFETitleFee[index]] = ClosingCost.GFETitleFee[index];
      ClosingCost.mapTable["974"] = "974";
      ClosingCost.mapTable["NEWHUD.X1724"] = "NEWHUD.X1724";
      ClosingCost.mapTable["NEWHUD.X1725"] = "NEWHUD.X1725";
      ClosingCost.mapTable["NEWHUD.X794"] = "NEWHUD.X794";
      ClosingCost.mapTable["NEWHUD.X798"] = "NEWHUD.X798";
      ClosingCost.mapTable["NEWHUD.X799"] = "NEWHUD.X799";
      ClosingCost.mapTable["NEWHUD.X800"] = "NEWHUD.X800";
      ClosingCost.mapTable["NEWHUD.X801"] = "NEWHUD.X801";
      ClosingCost.mapTable["NEWHUD.X1719"] = "NEWHUD.X1719";
      ClosingCost.mapTable["NEWHUD.X76"] = "NEWHUD.X76";
      ClosingCost.template2015Fields = new List<string>();
      for (int index = 1; index <= 4785; ++index)
      {
        if ((index < 61 || index > 62) && (index < 64 || index > 65) && (index < 67 || index > 68) && (index < 165 || index > 200) && (index < 231 || index > 233) && (index < 264 || index > 266) && (index < 297 || index > 299) && (index < 304 || index > 305) && (index < 330 || index > 332) && (index < 337 || index > 338) && (index < 363 || index > 365) && (index < 370 || index > 371) && (index < 396 || index > 398) && (index < 403 || index > 404) && (index < 429 || index > 431) && (index < 436 || index > 437) && (index < 462 || index > 465) && (index < 469 || index > 481) && (index < 485 || index > 488) && (index < 495 || index > 497) && (index < 502 || index > 503) && (index < 528 || index > 530) && (index < 535 || index > 536) && (index < 561 || index > 563) && (index < 568 || index > 569) && (index < 594 || index > 596) && (index < 601 || index > 602) && (index < 627 || index > 629) && (index < 634 || index > 635) && (index < 660 || index > 662) && (index < 667 || index > 668) && (index < 693 || index > 695) && (index < 700 || index > 701) && (index < 726 || index > 728) && (index < 733 || index > 734) && (index < 759 || index > 761) && (index < 766 || index > 767) && (index < 792 || index > 794) && (index < 799 || index > 800) && (index < 825 || index > 827) && (index < 832 || index > 833) && (index < 858 || index > 860) && (index < 865 || index > 866) && (index < 891 || index > 894) && (index < 898 || index > 903) && (index < 905 || index > 910) && (index < 913 || index > 926) && (index < 931 || index > 932) && (index < 957 || index > 959) && (index < 964 || index > 965) && (index < 990 || index > 992) && (index < 997 || index > 998) && (index < 1023 || index > 1025) && (index < 1030 || index > 1031) && (index < 1056 || index > 1058) && (index < 1089 || index > 1091) && (index < 1096 || index > 1097) && (index < 1122 || index > 1124) && (index < 1129 || index > 1130) && (index < 1155 || index > 1157) && (index < 1162 || index > 1163) && (index < 1188 || index > 1190) && (index < 1195 || index > 1196) && (index < 1221 || index > 1223) && (index < 1228 || index > 1229) && (index < 1254 || index > 1256) && (index < 1261 || index > 1262) && (index < 1287 || index > 1289) && (index < 1294 || index > 1295) && (index < 1320 || index > 1322) && (index < 1327 || index > 1328) && (index < 1353 || index > 1355) && (index < 1360 || index > 1361) && (index < 1386 || index > 1388) && (index < 1393 || index > 1394) && (index < 1419 || index > 1421) && (index < 1426 || index > 1427) && (index < 1452 || index > 1454) && (index < 1459 || index > 1460) && (index < 1485 || index > 1487) && (index < 1492 || index > 1493) && (index < 1518 || index > 1520) && (index < 1525 || index > 1526) && (index < 1551 || index > 1553) && (index < 1558 || index > 1559) && (index < 1584 || index > 1586) && (index < 1591 || index > 1592) && (index < 1617 || index > 1619) && (index < 1624 || index > 1625) && (index < 1650 || index > 1652) && (index < 1657 || index > 1658) && (index < 1683 || index > 1685) && (index < 1690 || index > 1691) && (index < 1716 || index > 1718) && (index < 1723 || index > 1724) && (index < 1749 || index > 1751) && (index < 1756 || index > 1757) && (index < 1782 || index > 1784) && (index < 1789 || index > 1790) && (index < 1815 || index > 1817) && (index < 1822 || index > 1823) && (index < 1848 || index > 1850) && (index < 1855 || index > 1856) && (index < 1881 || index > 1883) && (index < 1888 || index > 1889) && (index < 1914 || index > 1916) && (index < 1921 || index > 1922) && (index < 1947 || index > 1949) && (index < 1954 || index > 1955) && (index < 1980 || index > 1982) && (index < 1987 || index > 1988) && (index < 2013 || index > 2015) && (index < 2020 || index > 2021) && (index < 2046 || index > 2048) && (index < 2053 || index > 2054) && (index < 2079 || index > 2081) && (index < 2086 || index > 2087) && (index < 2107 || index > 2108) && (index < 2112 || index > 2114) && (index < 2119 || index > 2120) && (index < 2140 || index > 2141) && (index < 2145 || index > 2147) && (index < 2152 || index > 2153) && (index < 2172 || index > 2174) && (index < 2178 || index > 2180) && (index < 2185 || index > 2186) && (index < 2211 || index > 2213) && (index < 2244 || index > 2246) && (index < 2251 || index > 2252) && (index < 2277 || index > 2279) && (index < 2284 || index > 2285) && (index < 2310 || index > 2312) && (index < 2343 || index > 2345) && (index < 2350 || index > 2351) && (index < 2376 || index > 2378) && (index < 2383 || index > 2384) && (index < 2409 || index > 2411) && (index < 2416 || index > 2417) && (index < 2442 || index > 2444) && (index < 2449 || index > 2450) && (index < 2475 || index > 2477) && (index < 2482 || index > 2483) && (index < 2508 || index > 2510) && (index < 2515 || index > 2516) && (index < 2541 || index > 2543) && (index < 2548 || index > 2549) && (index < 2555 || index > 2556) && (index < 2574 || index > 2576) && (index < 2581 || index > 2582) && (index < 2588 || index > 2589) && (index < 2607 || index > 2609) && (index < 2614 || index > 2615) && (index < 2621 || index > 2622) && (index < 2640 || index > 2642) && (index < 2647 || index > 2648) && (index < 2654 || index > 2655) && (index < 2673 || index > 2675) && (index < 2680 || index > 2681) && (index < 2687 || index > 2688) && (index < 2706 || index > 2708) && (index < 2713 || index > 2714) && (index < 2720 || index > 2721) && (index < 2739 || index > 2741) && (index < 2746 || index > 2747) && (index < 2753 || index > 2754) && (index < 2772 || index > 2774) && (index < 2779 || index > 2780) && (index < 2786 || index > 2787) && (index < 2805 || index > 2807) && (index < 2812 || index > 2813) && (index < 2819 || index > 2820) && (index < 2838 || index > 2840) && (index < 2845 || index > 2846) && (index < 2871 || index > 2873) && (index < 2878 || index > 2879) && (index < 2904 || index > 2906) && (index < 2911 || index > 2912) && (index < 2937 || index > 2939) && (index < 2944 || index > 2945) && (index < 2970 || index > 2972) && (index < 2977 || index > 2978) && (index < 3003 || index > 3005) && (index < 3010 || index > 3011) && (index < 3036 || index > 3038) && (index < 3069 || index > 3071) && (index < 3102 || index > 3104) && (index < 3135 || index > 3137) && (index < 3168 || index > 3170) && (index < 3201 || index > 3203) && (index < 3234 || index > 3236) && (index < 3267 || index > 3269) && (index < 3300 || index > 3302) && (index < 3307 || index > 3308) && (index < 3333 || index > 3334) && (index < 3340 || index > 3341) && (index < 3366 || index > 3368) && (index < 3373 || index > 3374) && (index < 3399 || index > 3401) && (index < 3406 || index > 3407) && (index < 3432 || index > 3434) && (index < 3439 || index > 3440) && (index < 3465 || index > 3467) && (index < 3472 || index > 3473) && (index < 3498 || index > 3500) && (index < 3505 || index > 3506) && (index < 3531 || index > 3533) && (index < 3538 || index > 3539) && (index < 3564 || index > 3566) && (index < 3571 || index > 3572) && (index < 3597 || index > 3599) && (index < 3604 || index > 3605) && (index < 3630 || index > 3632) && (index < 3663 || index > 3665) && (index < 3696 || index > 3698) && (index < 3701 || index > 3702) && (index < 3729 || index > 3731) && (index < 3734 || index > 3735) && (index < 3762 || index > 3764) && (index < 3767 || index > 3768) && (index < 3795 || index > 3797) && (index < 3800 || index > 3801) && (index < 3828 || index > 3830) && (index < 3833 || index > 3834) && (index < 3861 || index > 3863) && (index < 3894 || index > 3896) && (index < 3927 || index > 3929) && (index < 3934 || index > 3935) && (index < 3960 || index > 3962) && (index < 3967 || index > 3968) && (index < 3993 || index > 3995) && (index < 4000 || index > 4001) && (index < 4026 || index > 4028) && (index < 4033 || index > 4034) && (index < 4059 || index > 4061) && (index < 4066 || index > 4067) && (index < 4092 || index > 4094) && (index < 4099 || index > 4100) && (index < 4125 || index > 4127) && (index < 4132 || index > 4133) && (index < 4158 || index > 4160) && (index < 4165 || index > 4166) && (index < 4191 || index > 4193) && (index < 4198 || index > 4199) && (index < 4224 || index > 4226) && (index < 4231 || index > 4232) && (index < 4257 || index > 4259) && (index < 4264 || index > 4265) && (index < 4290 || index > 4292) && (index < 4297 || index > 4298) && (index < 4323 || index > 4325) && (index < 4330 || index > 4331) && (index < 4356 || index > 4358) && (index < 4363 || index > 4364) && (index < 4389 || index > 4391) && (index < 4449 || index > 4450) && (index < 4475 || index > 4477) && (index < 4482 || index > 4483) && (index < 4508 || index > 4510) && (index < 4515 || index > 4516) && (index < 4541 || index > 4543) && (index < 4548 || index > 4549) && (index < 4574 || index > 4576) && (index < 4581 || index > 4582) && (index < 4607 || index > 4609) && (index < 4650 || index > 4659) && (index < 4756 || index > 4759) && index != 71 && index != 74 && index != 77 && index != 80 && index != 83 && index != 86 && index != 89 && index != 92 && index != 95 && index != 206 && index != 225 && index != 239 && index != 258 && index != 291 && index != 272 && index != 467 && index != 1064 && index != 2084 && index != 2117 && index != 2187 && index != 2218 && index != 2238 && index != 2271 && index != 2304 && index != 2317 && index != 2337 && index != 2370 && index != 2403 && index != 2436 && index != 2447 && index != 2469 && index != 2480 && index != 2502 && index != 2513 && index != 2535 && index != 2553 && index != 2559 && index != 2562 && index != 2565 && index != 2568 && index != 2586 && index != 2592 && index != 2595 && index != 2598 && index != 2619 && index != 2625 && index != 2628 && index != 2631 && index != 2634 && index != 2652 && index != 2658 && index != 2661 && index != 2664 && index != 2667 && index != 2685 && index != 2691 && index != 2694 && index != 2697 && index != 2700 && index != 2718 && index != 2724 && index != 2727 && index != 2730 && index != 2733 && index != 2751 && index != 2757 && index != 2760 && index != 2763 && index != 2766 && index != 2784 && index != 2790 && index != 2793 && index != 2796 && index != 2799 && index != 2817 && index != 2823 && index != 2826 && index != 2829 && index != 3044 && index != 3077 && index != 3110 && index != 3143 && index != 3176 && index != 3209 && index != 3242 && index != 3275 && index != 3569 && index != 3602 && index != 3635 && index != 3638 && index != 3671 && index != 3704 && index != 3723 && index != 3737 && index != 3756 && index != 3770 && index != 3789 && index != 3803 && index != 3822 && index != 3836 && index != 3855 && index != 3866 && index != 3869 && index != 3899 && index != 3902 && index != 4396 && index != 4398 && index != 4668 && index != 4691 && index != 4714 && index != 4737 && index != 4676 && index != 4699 && index != 4722 && index != 4745 && index != 4465 && index != 4468 && index != 4646 && index != 4481 && index != 4647 && index != 4514 && index != 4648 && index != 4547 && index != 4649 && index != 4580 && index != 4666 && index != 4667 && index != 4689 && index != 4690 && index != 4712 && index != 4713 && index != 4735 && index != 4736 && index != 4197 && index != 4230 && index != 4263 && index != 4296 && index != 4329 && index != 4362 && index != 4448 && index != 4481 && index != 4768 && (index < 4770 || index >= 4780))
          ClosingCost.template2015Fields.Add("NEWHUD2.X" + (object) index);
      }
      ClosingCost.remove2015UnwantedFields(203, 4163, 33);
      ClosingCost.remove2015UnwantedFields(204, 4164, 33);
      ClosingCost.remove2015UnwantedFields(4429, 4434, 1);
      for (int index = 1; index <= 21; ++index)
        ClosingCost.template2015Fields.Add("POPT2.X" + (object) index);
      ClosingCost.template2015Fields.Add("4855");
      ClosingCost.template2015Fields.Add("4856");
      ClosingCost.TemplateFields = new string[ClosingCost.mapTable.Count];
      ClosingCost.mapTable.Keys.CopyTo(ClosingCost.TemplateFields, 0);
    }

    private static void remove2015UnwantedFields(int started, int end, int interval)
    {
      for (int index = started; index <= end; index += interval)
      {
        if (ClosingCost.template2015Fields.Contains("NEWHUD2.X" + (object) index))
          ClosingCost.template2015Fields.Remove("NEWHUD2.X" + (object) index);
      }
    }

    public ClosingCost()
    {
    }

    public ClosingCost(XmlSerializationInfo info)
      : base(info)
    {
      JedVersion jedVersion1 = new JedVersion(1, 0, 0);
      JedVersion jedVersion2;
      try
      {
        jedVersion2 = JedVersion.Parse(info.GetString("Version"));
      }
      catch
      {
        jedVersion2 = JedVersion.Parse(this.GetSimpleField("CCVersion"));
      }
      if (jedVersion2 < JedVersion.Parse("2.3.0"))
      {
        for (int index = 252; index <= 292; index += 2)
          this.dataMigrationForPTB("SYS.X" + index.ToString());
        this.dataMigrationForPTB("SYS.X297");
        for (int index = 302; index <= 366; index += 2)
          this.dataMigrationForPTB("SYS.X" + index.ToString());
        for (int index = 371; index <= 387; index += 2)
          this.dataMigrationForPTB("SYS.X" + index.ToString());
        this.dataMigrationForPTB("SYS.X392");
        this.dataMigrationForPTB("SYS.X404");
        this.dataMigrationForPTB("SYS.X406");
        this.dataMigrationForPTB("SYS.X408");
      }
      if (jedVersion2 < JedVersion.Parse("6.7.0"))
      {
        this.dataMigrationForToFields("154", "NEWHUD.X1045");
        this.dataMigrationForToFields("1627", "NEWHUD.X1046");
        this.dataMigrationForToFields("1838", "NEWHUD.X1047");
        this.dataMigrationForToFields("1841", "NEWHUD.X1048");
        this.dataMigrationForToFields("NEWHUD.X732", "NEWHUD.X1049");
        this.dataMigrationForToFields("NEWHUD.X126", "NEWHUD.X1050");
        this.dataMigrationForToFields("NEWHUD.X127", "NEWHUD.X1051");
        this.dataMigrationForToFields("NEWHUD.X128", "NEWHUD.X1052");
        this.dataMigrationForToFields("NEWHUD.X129", "NEWHUD.X1053");
        this.dataMigrationForToFields("NEWHUD.X130", "NEWHUD.X1054");
        this.dataMigrationForToFields("369", "NEWHUD.X1055");
        this.dataMigrationForToFields("371", "NEWHUD.X1056");
        this.dataMigrationForToFields("348", "NEWHUD.X1057");
        this.dataMigrationForToFields("931", "NEWHUD.X1058");
        this.dataMigrationForToFields("1390", "NEWHUD.X1059");
        this.dataMigrationForToFields("410", "NEWHUD.X1060");
        this.dataMigrationForToFields("NEWHUD.X656", "NEWHUD.X1061");
        this.dataMigrationForToFields("NEWHUD.X582", "NEWHUD.X1062");
        this.dataMigrationForToFields("L259", "NEWHUD.X1063");
        this.dataMigrationForToFields("1666", "NEWHUD.X1064");
        this.dataMigrationForToFields("NEWHUD.X583", "NEWHUD.X1065");
        this.dataMigrationForToFields("NEWHUD.X584", "NEWHUD.X1066");
        this.dataMigrationForToFields("1628", "VEND.X341");
        this.dataMigrationForToFields("660", "VEND.X350");
        this.dataMigrationForToFields("661", "VEND.X359");
        this.dataMigrationForToFields("NEWHUD.X951", "NEWHUD.X1070");
        this.dataMigrationForToFields("NEWHUD.X960", "NEWHUD.X1071");
        this.dataMigrationForToFields("NEWHUD.X969", "NEWHUD.X1072");
        this.dataMigrationForToFields("NEWHUD.X978", "NEWHUD.X1073");
        this.dataMigrationForToFields("NEWHUD.X987", "NEWHUD.X1074");
        this.dataMigrationForToFields("NEWHUD.X996", "NEWHUD.X1075");
        this.dataMigrationForToFields("NEWHUD.X208", "NEWHUD.X1076");
        this.dataMigrationForToFields("NEWHUD.X209", "NEWHUD.X1077");
        this.dataMigrationForToFields("1762", "NEWHUD.X1078");
        this.dataMigrationForToFields("1767", "NEWHUD.X1079");
        this.dataMigrationForToFields("1772", "NEWHUD.X1080");
        this.dataMigrationForToFields("1777", "NEWHUD.X1081");
        this.dataMigrationForToFields("373", "NEWHUD.X1082");
        this.dataMigrationForToFields("1640", "NEWHUD.X1083");
        this.dataMigrationForToFields("1643", "NEWHUD.X1084");
        this.dataMigrationForToFields("NEWHUD.X251", "NEWHUD.X1085");
        this.dataMigrationForToFields("650", "NEWHUD.X1086");
        this.dataMigrationForToFields("651", "NEWHUD.X1087");
        this.dataMigrationForToFields("40", "NEWHUD.X1088");
        this.dataMigrationForToFields("43", "NEWHUD.X1089");
        this.dataMigrationForToFields("1782", "NEWHUD.X1090");
        this.dataMigrationForToFields("1787", "NEWHUD.X1091");
        this.dataMigrationForToFields("1792", "NEWHUD.X1092");
        this.dataMigrationForToFields("NEWHUD.X252", "NEWHUD.X1093");
        this.dataMigrationForToFields("NEWHUD.X253", "NEWHUD.X1094");
      }
      if (base.TemplateName == "")
        base.TemplateName = this.GetSimpleField("1804");
      try
      {
        this.respaVersion = info.GetString("RESPAVERSION");
      }
      catch (Exception ex)
      {
      }
      if ((this.respaVersion ?? "") == "")
      {
        this.for2010gfe = this.GetSimpleField("FOR2010") == "Y" || info.GetBoolean("GFE2010", false);
        this.respaVersion = this.for2010gfe ? "2010" : "2009";
      }
      else
        this.for2010gfe = !(this.respaVersion == "2015") && !(this.respaVersion == "2009");
      if (this.FieldData.ContainsKey("FOR2010"))
        this.FieldData.Remove("FOR2010");
      if (!this.FieldData.ContainsKey("CCVersion"))
        return;
      this.FieldData.Remove("CCVersion");
    }

    public bool For2010GFE
    {
      get => this.for2010gfe;
      set
      {
        this.for2010gfe = value;
        this.MarkAsDirty();
      }
    }

    public string RESPAVersion
    {
      get => this.respaVersion;
      set
      {
        if (this.respaVersion == "2010" && value == "2015")
        {
          if (this.GetField("L211") != "")
          {
            this.SetField("NEWHUD2.X1", this.GetField("L211"));
            this.SetField("NEWHUD2.X201", this.GetField("L211"));
            this.SetField("NEWHUD2.X208", this.GetField("L211"));
            this.SetField("NEWHUD2.X211", this.GetField("L211"));
          }
          if (this.GetField("L213") != "")
          {
            this.SetField("NEWHUD2.X3", this.GetField("L213"));
            this.SetField("NEWHUD2.X234", this.GetField("L211"));
            this.SetField("NEWHUD2.X241", this.GetField("L211"));
            this.SetField("NEWHUD2.X244", this.GetField("L211"));
          }
          if (this.GetField("L215") != "")
          {
            this.AddLock("NEWHUD2.X5");
            this.SetField("NEWHUD2.X5", this.GetField("L215"));
            this.SetField("L215", "");
          }
          if (this.GetField("L216") != "")
          {
            this.AddLock("NEWHUD2.X6");
            this.SetField("NEWHUD2.X6", this.GetField("L216"));
            this.SetField("L216", "");
          }
          if (this.GetField("NEWHUD.X591") != "" || this.GetField("NEWHUD.X594") != "")
          {
            this.AddLock("NEWHUD.X591");
            this.SetField("NEWHUD2.X2247", this.GetField("231"));
            this.SetField("NEWHUD2.X2254", this.GetField("NEWHUD.X591"));
            this.SetField("NEWHUD2.X2255", this.GetField("NEWHUD.X591"));
            this.SetField("NEWHUD2.X2257", this.GetField("NEWHUD.X591"));
            this.SetField("NEWHUD2.X2258", this.GetField("NEWHUD.X594"));
            this.SetField("NEWHUD2.X2260", this.GetField("NEWHUD.X594"));
          }
          if (this.GetField("643") != "" || this.GetField("579") != "")
          {
            this.AddLock("643");
            this.SetField("NEWHUD2.X4400", (Utils.ParseDouble((object) this.GetField("643")) + Utils.ParseDouble((object) this.GetField("579"))).ToString());
            this.SetField("NEWHUD2.X2313", this.GetField("NEWHUD2.X4400"));
            this.SetField("NEWHUD2.X2320", this.GetField("643"));
            this.SetField("NEWHUD2.X2321", this.GetField("643"));
            this.SetField("NEWHUD2.X2323", this.GetField("643"));
            this.SetField("NEWHUD2.X2324", this.GetField("579"));
            this.SetField("NEWHUD2.X2326", this.GetField("579"));
          }
          if (this.GetField("L260") != "" || this.GetField("L261") != "")
          {
            this.AddLock("L260");
            this.SetField("NEWHUD2.X4402", (Utils.ParseDouble((object) this.GetField("L260")) + Utils.ParseDouble((object) this.GetField("L261"))).ToString());
            this.SetField("NEWHUD2.X2346", this.GetField("NEWHUD2.X4402"));
            this.SetField("NEWHUD2.X2353", this.GetField("L260"));
            this.SetField("NEWHUD2.X2354", this.GetField("L260"));
            this.SetField("NEWHUD2.X2356", this.GetField("L260"));
            this.SetField("NEWHUD2.X2357", this.GetField("L261"));
            this.SetField("NEWHUD2.X2359", this.GetField("L261"));
          }
          if (this.GetField("1667") != "" || this.GetField("1668") != "")
          {
            this.AddLock("1667");
            this.SetField("NEWHUD2.X4404", (Utils.ParseDouble((object) this.GetField("1667")) + Utils.ParseDouble((object) this.GetField("1668"))).ToString());
            this.SetField("NEWHUD2.X2379", this.GetField("NEWHUD2.X4404"));
            this.SetField("NEWHUD2.X2386", this.GetField("1667"));
            this.SetField("NEWHUD2.X2387", this.GetField("1667"));
            this.SetField("NEWHUD2.X2389", this.GetField("1667"));
            this.SetField("NEWHUD2.X2390", this.GetField("1668"));
            this.SetField("NEWHUD2.X2392", this.GetField("1668"));
          }
          if (this.GetField("NEWHUD.X592") != "" || this.GetField("NEWHUD.X595") != "")
          {
            this.AddLock("NEWHUD.X592");
            this.SetField("NEWHUD2.X4406", (Utils.ParseDouble((object) this.GetField("NEWHUD.X592")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X595"))).ToString());
            this.SetField("NEWHUD2.X2412", this.GetField("NEWHUD2.X4406"));
            this.SetField("NEWHUD2.X2419", this.GetField("NEWHUD.X592"));
            this.SetField("NEWHUD2.X2420", this.GetField("NEWHUD.X592"));
            this.SetField("NEWHUD2.X2422", this.GetField("NEWHUD.X592"));
            this.SetField("NEWHUD2.X2423", this.GetField("NEWHUD.X595"));
            this.SetField("NEWHUD2.X2425", this.GetField("NEWHUD.X595"));
          }
          if (this.GetField("NEWHUD.X593") != "" || this.GetField("NEWHUD.X596") != "")
          {
            this.AddLock("NEWHUD.X593");
            this.SetField("NEWHUD2.X4408", (Utils.ParseDouble((object) this.GetField("NEWHUD.X593")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X596"))).ToString());
            this.SetField("NEWHUD2.X2445", this.GetField("NEWHUD2.X4408"));
            this.SetField("NEWHUD2.X2452", this.GetField("NEWHUD.X592"));
            this.SetField("NEWHUD2.X2453", this.GetField("NEWHUD.X592"));
            this.SetField("NEWHUD2.X2455", this.GetField("NEWHUD.X592"));
            this.SetField("NEWHUD2.X2456", this.GetField("NEWHUD.X595"));
            this.SetField("NEWHUD2.X2458", this.GetField("NEWHUD.X595"));
          }
          if (this.GetField("NEWHUD.X1588") != "" || this.GetField("NEWHUD.X1589") != "")
          {
            this.AddLock("NEWHUD.X1588");
            this.SetField("NEWHUD2.X4410", (Utils.ParseDouble((object) this.GetField("NEWHUD.X1588")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X1589"))).ToString());
            this.SetField("NEWHUD2.X2478", this.GetField("NEWHUD2.X4410"));
            this.SetField("NEWHUD2.X2485", this.GetField("NEWHUD.X1588"));
            this.SetField("NEWHUD2.X2486", this.GetField("NEWHUD.X1588"));
            this.SetField("NEWHUD2.X2488", this.GetField("NEWHUD.X1588"));
            this.SetField("NEWHUD2.X2489", this.GetField("NEWHUD.X1589"));
            this.SetField("NEWHUD2.X2491", this.GetField("NEWHUD.X1589"));
          }
          if (this.GetField("NEWHUD.X1596") != "" || this.GetField("NEWHUD.X1597") != "")
          {
            this.AddLock("NEWHUD.X1596");
            this.SetField("NEWHUD2.X4412", (Utils.ParseDouble((object) this.GetField("NEWHUD.X1596")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X1597"))).ToString());
            this.SetField("NEWHUD2.X2511", this.GetField("NEWHUD2.X4412"));
            this.SetField("NEWHUD2.X2518", this.GetField("NEWHUD.X1596"));
            this.SetField("NEWHUD2.X2519", this.GetField("NEWHUD.X1596"));
            this.SetField("NEWHUD2.X2521", this.GetField("NEWHUD.X1596"));
            this.SetField("NEWHUD2.X2522", this.GetField("NEWHUD.X1597"));
            this.SetField("NEWHUD2.X2524", this.GetField("NEWHUD.X1597"));
          }
          double num = Utils.ParseDouble((object) this.GetField("NEWHUD.X952")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X961")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X970")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X979")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X988")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X997"));
          this.SetField("NEWHUD.X571", num != 0.0 ? num.ToString("N2") : "");
          num = Utils.ParseDouble((object) this.GetField("NEWHUD.X953")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X962")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X971")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X980")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X989")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X998"));
          this.SetField("NEWHUD.X798", num != 0.0 ? num.ToString("N2") : "");
          this.SetField("RecalculationRequired", "Y");
        }
        else if (this.respaVersion == "2015" && value != "2015")
        {
          this.SetField("L215", this.GetField("NEWHUD2.X5"));
          this.SetField("L216", this.GetField("NEWHUD2.X6"));
          this.RemoveLock("NEWHUD.X591");
          this.RemoveLock("643");
          this.RemoveLock("L260");
          this.RemoveLock("1667");
          this.RemoveLock("NEWHUD.X592");
          this.RemoveLock("NEWHUD.X593");
          this.RemoveLock("NEWHUD.X593");
          this.RemoveLock("NEWHUD.X1588");
          this.RemoveLock("NEWHUD.X1596");
          double num = Utils.ParseDouble((object) this.GetField("NEWHUD.X952")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X961")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X970")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X979")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X988")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X997")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X645")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X639")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X215")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X216")) + Utils.ParseDouble((object) this.GetField("1763")) + Utils.ParseDouble((object) this.GetField("1768")) + Utils.ParseDouble((object) this.GetField("1773")) + Utils.ParseDouble((object) this.GetField("1778"));
          this.SetField("NEWHUD.X571", num != 0.0 ? num.ToString("N2") : "");
          num = Utils.ParseDouble((object) this.GetField("NEWHUD.X953")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X962")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X971")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X980")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X989")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X998")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X782")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X784")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X218")) + Utils.ParseDouble((object) this.GetField("NEWHUD.X219")) + Utils.ParseDouble((object) this.GetField("1764")) + Utils.ParseDouble((object) this.GetField("1769")) + Utils.ParseDouble((object) this.GetField("1774")) + Utils.ParseDouble((object) this.GetField("1779"));
          this.SetField("NEWHUD.X798", num != 0.0 ? num.ToString("N2") : "");
          this.SetField("RecalculationRequired", "Y");
          for (int index = 0; index < ClosingCost.template2015Fields.Count; ++index)
            this.RemoveField(ClosingCost.template2015Fields[index]);
        }
        this.respaVersion = value;
        this.MarkAsDirty();
      }
    }

    public override string[] GetAllowedFieldIDs()
    {
      return this.respaVersion == "2015" ? ClosingCost.AllTemplateFields : ClosingCost.TemplateFields;
    }

    public override string GetSimpleField(string id)
    {
      return ClosingCost.mapTable.ContainsKey(id) ? base.GetSimpleField(ClosingCost.mapTable[id]) : base.GetSimpleField(id);
    }

    public override FieldDefinition GetFieldDefinition(string id)
    {
      return ClosingCost.mapTable.ContainsKey(id) ? base.GetFieldDefinition(ClosingCost.mapTable[id]) : base.GetFieldDefinition(id);
    }

    public override ITemplateSetting Duplicate() => base.Duplicate();

    private void dataMigrationForPTB(string id)
    {
      string simpleField = this.GetSimpleField(id);
      if (string.Compare(simpleField, "Y", true) == 0)
      {
        base.SetField(id, "Broker");
      }
      else
      {
        if (string.Compare(simpleField, "N", true) != 0)
          return;
        base.SetField(id, "");
      }
    }

    private void dataMigrationForToFields(string id, string toFieldID)
    {
      string simpleField = this.GetSimpleField(id);
      if (this.GetSimpleField(toFieldID) != string.Empty || simpleField == string.Empty)
        return;
      int length = simpleField.ToLower().IndexOf(" to:");
      if (length == -1)
        return;
      base.SetField(id, simpleField.Substring(0, length).Trim());
      base.SetField(toFieldID, simpleField.Substring(length + 4).Trim());
    }

    public override string TemplateName
    {
      get => base.TemplateName;
      set
      {
        base.TemplateName = value;
        this.SetCurrentField("1804", value);
      }
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Version", (object) "6.7.0");
      info.AddValue("GFE2010", (object) this.for2010gfe);
      info.AddValue("RESPAVERSION", (object) this.respaVersion);
    }

    public static explicit operator ClosingCost(BinaryObject obj)
    {
      return (ClosingCost) BinaryConvertibleObject.Parse(obj, typeof (ClosingCost));
    }

    public override void SetField(string id, string val)
    {
      base.SetField(id, val);
      switch (id)
      {
        case "617":
          this.initContactDetail("617", ClosingCost.GFEAppraisal);
          break;
        case "624":
          this.initContactDetail("624", ClosingCost.GFECredit);
          break;
        case "REGZGFE.X8":
          this.initContactDetail("REGZGFE.X8", ClosingCost.GFEUnderWriting);
          break;
        case "L248":
          this.initContactDetail("L248", ClosingCost.GFEMortIns);
          break;
        case "L252":
          this.initContactDetail("L252", ClosingCost.GFEHazIns);
          break;
        case "1500":
          this.initContactDetail("1500", ClosingCost.GFEFloodIns);
          break;
        case "610":
          this.initContactDetail("610", ClosingCost.GFEEscrowFee);
          break;
        case "395":
          this.initContactDetail("395", ClosingCost.GFEDocFee);
          break;
        case "56":
          this.initContactDetail("56", ClosingCost.GFEAttorneyFee);
          break;
        case "411":
          this.initContactDetail("411", ClosingCost.GFETitleFee);
          break;
      }
    }

    public override Hashtable GetProperties()
    {
      Hashtable properties = base.GetProperties();
      properties[(object) "For2010GFE"] = this.For2010GFE ? (object) "Yes" : (object) "No";
      properties[(object) "RESPAVERSION"] = (object) this.respaVersion;
      return properties;
    }

    public void RemoveContactDetail()
    {
      this.initContactDetail("617", ClosingCost.GFEAppraisal);
      this.initContactDetail("624", ClosingCost.GFECredit);
      this.initContactDetail("REGZGFE.X8", ClosingCost.GFEUnderWriting);
      this.initContactDetail("L248", ClosingCost.GFEMortIns);
      this.initContactDetail("L252", ClosingCost.GFEHazIns);
      this.initContactDetail("1500", ClosingCost.GFEFloodIns);
      this.initContactDetail("610", ClosingCost.GFEEscrowFee);
      this.initContactDetail("395", ClosingCost.GFEDocFee);
      this.initContactDetail("56", ClosingCost.GFEAttorneyFee);
      this.initContactDetail("411", ClosingCost.GFETitleFee);
    }

    private void initContactDetail(string checkID, string[] ids)
    {
      if ((this.GetSimpleField(checkID) ?? "") != "")
        return;
      for (int index = 1; index < ids.Length; ++index)
      {
        this.RemoveField(ids[index]);
        this.CleanField(ids[index]);
      }
      this.RemoveField(checkID);
      this.CleanField(checkID);
    }
  }
}
