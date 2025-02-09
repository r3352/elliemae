// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.DisclosureTracking2015Log
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using Elli.ElliEnum;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Xml;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  public class DisclosureTracking2015Log : 
    DisclosureTrackingBase,
    IDisclosureTracking2015Log,
    IDisclosureTrackingLog
  {
    public static readonly List<string> disclosureSnapshotFields = new List<string>((IEnumerable<string>) new string[30]
    {
      "1868",
      "1873",
      "11",
      "12",
      "14",
      "15",
      "799",
      "KBYO.XD799",
      "3887",
      "1401",
      "2",
      "1206",
      "3142",
      "745",
      "4008",
      "4009",
      "3164",
      "3197",
      "3136",
      "1172",
      "696",
      "4461",
      "4000",
      "4001",
      "4002",
      "4003",
      "4004",
      "4005",
      "4006",
      "4007"
    });
    public static readonly List<string> DisclosedCDFields = new List<string>((IEnumerable<string>) new string[1418]
    {
      "CD2.XSTD",
      "CD2.XSTI",
      "CD2.XSTLC",
      "CD2.XSTJ",
      "CD3.X45",
      "LE2.X27",
      "1821",
      "356",
      "136",
      "LE1.X3",
      "1063",
      "364",
      "4063",
      "1040",
      "LE1.X2",
      "LE1.X5",
      "dd_LE1X4",
      "1172",
      "1172",
      "1172",
      "1172",
      "1172",
      "LE1.X4",
      "19",
      "SYS.X6",
      "I_LE1X5",
      "2",
      "NEWHUD.X6",
      "3",
      "KBYO.XD3",
      "NEWHUD.X5",
      "5",
      "NEWHUD.X8",
      "675",
      "1659",
      "l_X13",
      "LE1.X13",
      "LE1.X14",
      "l_X15",
      "LE1.X15",
      "LE1.X17",
      "l_X555",
      "NEWHUD.X555",
      "KBYO.NEWHUDXD555",
      "LE1.X18",
      "l_X18",
      "LE1.X16",
      "l_X16",
      "LE1.X10",
      "NEWHUD.X7",
      "LE1.X11",
      "LE1.X12",
      "l_X12",
      "l_X315",
      "RE88395.X315",
      "LE1.X27",
      "l_X27",
      "RE88395.X121",
      "NEWHUD.X348",
      "l_X26",
      "LE1.X26",
      "HUD51",
      "l_X19",
      "LE1.X19",
      "LE1.X20",
      "l_X21",
      "LE1.X21",
      "l_X22",
      "LE1.X22",
      "LE1.X25",
      "LE1.X26",
      "l_X25",
      "LE1.X24",
      "l_X24",
      "LE1.X23",
      "NEWHUD.X349",
      "NEWHUD.X350",
      "NEWHUD.X351",
      "NEWHUD.X78",
      "1868",
      "1416",
      "1417",
      "1418",
      "1419",
      "1873",
      "701",
      "702",
      "1249",
      "703",
      "1264",
      "1257",
      "1258",
      "4680",
      "4681",
      "URLA.X267",
      "URLA.X269",
      "1259",
      "1260",
      "748",
      "2553",
      "VEND.X655",
      "L72",
      "11",
      "12",
      "14",
      "15",
      "CD1.X1",
      "CD1.X2",
      "CD1.X3",
      "CD1.X4",
      "CD1.X5",
      "CD1.X6",
      "CD1.X7",
      "CD1.X8",
      "CD1.X9",
      "CD1.X10",
      "CD1.X11",
      "CD1.X12",
      "CD1.X13",
      "CD1.X14",
      "CD1.X15",
      "CD1.X16",
      "CD1.X17",
      "CD1.X18",
      "CD1.X19",
      "CD1.X20",
      "CD1.X21",
      "CD1.X22",
      "CD1.X23",
      "CD1.X24",
      "CD1.X25",
      "CD1.X26",
      "CD1.X27",
      "CD1.X28",
      "CD1.X29",
      "CD1.X30",
      "CD1.X31",
      "CD1.X32",
      "CD1.X33",
      "CD1.X34",
      "CD1.X35",
      "CD1.X36",
      "CD1.X37",
      "CD1.X38",
      "CD1.X39",
      "CD1.X40",
      "CD1.X41",
      "CD1.X52",
      "CD1.X53",
      "CD1.X54",
      "CD1.X55",
      "CD1.X56",
      "CD1.X57",
      "CD1.X58",
      "CD1.X59",
      "CD1.X60",
      "CD1.X61",
      "CD1.X62",
      "CD1.X63",
      "CD1.X64",
      "CD1.X65",
      "3977",
      "3288",
      "CD1.X42",
      "LE2.X28",
      "3287",
      "3291",
      "CD1.X43",
      "NEWHUD2.XPJTCOLUMNS",
      "CD1.X66",
      "CD1.X67",
      "CD1.X68",
      "CD1.X69",
      "NEWHUD2.XPJT",
      "CD1.XD1",
      "CD1.XD2",
      "CD1.XD3",
      "CD1.XD4",
      "CD1.XD8",
      "CD1.XD9",
      "CD1.XD13",
      "CD1.XD14",
      "CD1.XD17",
      "CD1.XD18",
      "CD1.XD22",
      "CD1.XD23",
      "CD1.XD26",
      "CD1.XD27",
      "CD1.XD31",
      "CD1.XD32",
      "CD1.XD35",
      "CD1.XD36",
      "CD1.XD40",
      "CD1.XD41",
      "PAYMENTTABLE.CUSTOMIZE",
      "LOANTERMTABLE.CUSTOMIZE",
      "PAYMENTTABLE.USEACTUALPAYMENTCHANGE",
      "4085",
      "CD1.X70",
      "LE1.X88",
      "LE1.X89",
      "CD1.X71",
      "LE1.X91",
      "LE1.XD27",
      "CD1.X72",
      "CONST.X1",
      "4113",
      "KBYO.XD4113",
      "L726",
      "1176",
      "1177",
      "LE1.X98",
      "LE1.X99",
      "CD2.XSTF",
      "CD2.XSTG",
      "CD2.XSTH",
      "CD2.XBCCAC",
      "CD2.XBCCBC",
      "CD2.XSCCAC",
      "CD2.XSCCBC",
      "CD2.XCCBO",
      "CD2.XSTLC",
      "CD2.XSTJ",
      "CD2.XSTE",
      "CD2.XSTE",
      "CD2.XSTF",
      "CD2.XSTG",
      "CD2.XSTI",
      "CD2.XOCAC",
      "CD2.XOCBC",
      "CD2.XSTH",
      "CD2.XSTB",
      "CD2.XSTC",
      "CD2.XSTA",
      "CD2.XSTB",
      "CD2.XSTC",
      "CD2.XSTD",
      "CD2.XLCAC",
      "CD2.XLCBC",
      "CD2.X2",
      "CD3.X45",
      "136",
      "L79",
      "CD3.X1",
      "L85",
      "L84",
      "L88",
      "L89",
      "CD3.X2",
      "CD3.X3",
      "CD3.X4",
      "CD3.X5",
      "L94",
      "L100",
      "L106",
      "L92",
      "L93",
      "L98",
      "L99",
      "L104",
      "L105",
      "L110",
      "L111",
      "L114",
      "L115",
      "L118",
      "L119",
      "L122",
      "L123",
      "CD3.X6",
      "CD3.X7",
      "CD3.X41",
      "L128",
      "2",
      "L132",
      "L135",
      "L134",
      "CD3.X13",
      "CD3.X14",
      "CD3.X17",
      "CD3.X16",
      "CD3.X15",
      "CD3.X18",
      "L158",
      "L164",
      "L170",
      "L156",
      "L157",
      "L162",
      "L163",
      "L168",
      "L169",
      "L174",
      "L175",
      "L178",
      "L179",
      "L182",
      "L183",
      "CD3.X42",
      "CD3.X108",
      "CD3.X9",
      "CD3.X10",
      "CD3.X11",
      "CD3.X12",
      "CD3.X19",
      "CD3.X20",
      "CD3.X21",
      "CD3.X22",
      "CD3.X23",
      "CD3.X48",
      "136",
      "L80",
      "L82",
      "L87",
      "L86",
      "L90",
      "L91",
      "CD3.X24",
      "CD3.X25",
      "CD3.X26",
      "CD3.X27",
      "L97",
      "L103",
      "L109",
      "L95",
      "L96",
      "L101",
      "L102",
      "L107",
      "L108",
      "L112",
      "L113",
      "L116",
      "L117",
      "L120",
      "L121",
      "L124",
      "L125",
      "CD3.X30",
      "CD3.X31",
      "CD3.X43",
      "L81",
      "CD3.X28",
      "CD3.X29",
      "L129",
      "CD3.X46",
      "L133",
      "L136",
      "L150",
      "L151",
      "CD3.X32",
      "L155",
      "L154",
      "CD3.X33",
      "L161",
      "L167",
      "L173",
      "L159",
      "L160",
      "L165",
      "L166",
      "L171",
      "L172",
      "L176",
      "L177",
      "L180",
      "L181",
      "L184",
      "L185",
      "CD3.X44",
      "L139",
      "L142",
      "L143",
      "L146",
      "L147",
      "CD3.X34",
      "CD3.X35",
      "CD3.X36",
      "CD3.X37",
      "CD3.X38",
      "CD3.X39",
      "CD3.X40",
      "CD3.X49",
      "CD3.X50",
      "CD3.X51",
      "CD3.X52",
      "CD3.X53",
      "CD3.X54",
      "CD3.X55",
      "CD3.X56",
      "CD3.X57",
      "CD3.X58",
      "CD3.X59",
      "CD3.X60",
      "CD3.X61",
      "CD3.X62",
      "CD3.X63",
      "CD3.X64",
      "CD3.X65",
      "CD3.X66",
      "CD3.X67",
      "CD3.X68",
      "CD3.X69",
      "CD3.X70",
      "CD3.X71",
      "CD3.X72",
      "CD3.X73",
      "CD3.X74",
      "CD3.X75",
      "CD3.X76",
      "CD3.X77",
      "CD3.X78",
      "CD3.X79",
      "CD3.X80",
      "CD3.X81",
      "CD3.X82",
      "CD3.X83",
      "CD3.X84",
      "CD3.X85",
      "CD3.X86",
      "CD3.X87",
      "CD3.X88",
      "CD3.X89",
      "CD3.X90",
      "CD3.X91",
      "CD3.X92",
      "CD3.X93",
      "CD3.X94",
      "CD3.X95",
      "CD3.X96",
      "CD3.X97",
      "CD3.X98",
      "CD3.X99",
      "CD3.X100",
      "CD3.X101",
      "CD3.X102",
      "CD3.X103",
      "CD3.X104",
      "CD3.X105",
      "CD3.X106",
      "CD3.X107",
      "CD3.X109",
      "CD3.X110",
      "CD3.X111",
      "CD3.X112",
      "CD3.X113",
      "CD3.X114",
      "CD3.X115",
      "CD3.X116",
      "CD3.X117",
      "CD3.X118",
      "CD3.X119",
      "CD3.X120",
      "CD3.X121",
      "CD3.X122",
      "CD3.X123",
      "CD3.X124",
      "CD3.X125",
      "CD3.X126",
      "CD3.X127",
      "CD3.X128",
      "CD3.X129",
      "CD3.X130",
      "CD3.X131",
      "CD3.X132",
      "CD3.X133",
      "CD3.X134",
      "LE2.X28",
      "HUD1P1.X54",
      "HUD1P1.X15",
      "HUD1P1.X39",
      "HUD1P1.X14",
      "HUD1P1.X38",
      "HUD1P1.X9",
      "HUD1P1.X72",
      "HUD1P1.X50",
      "HUD1P1.X42",
      "HUD1P1.X2",
      "HUD1P1.X69",
      "HUD1P1.X80",
      "HUD1P1.X73",
      "HUD1P1.X60",
      "HUD1P1.X53",
      "HUD1P1.X62",
      "HUD1P1.X57",
      "HUD1P1.X6",
      "HUD1P1.X43",
      "HUD1P1.X84",
      "HUD1P1.X48",
      "HUD1P1.X56",
      "HUD1P1.X37",
      "HUD1P1.X28",
      "HUD1P1.X44",
      "HUD1P1.X88",
      "HUD1P1.X85",
      "HUD1P1.X79",
      "HUD1P1.X23",
      "HUD1P1.X17",
      "HUD1P1.X41",
      "HUD1P1.X81",
      "HUD1P1.X87",
      "HUD1P1.X58",
      "HUD1P1.X26",
      "HUD1P1.X11",
      "HUD1P1.X27",
      "HUD1P1.X47",
      "HUD1P1.X71",
      "HUD1P1.X45",
      "HUD1P1.X82",
      "HUD1P1.X4",
      "HUD1P1.X10",
      "HUD1P1.X46",
      "HUD1P1.X64",
      "HUD1P1.X5",
      "HUD1P1.X76",
      "HUD1P1.X63",
      "HUD1P1.X86",
      "HUD1P1.X77",
      "HUD1P1.X78",
      "HUD1P1.X59",
      "HUD1P1.X83",
      "HUD1P1.X52",
      "HUD1P1.X12",
      "HUD1P1.X40",
      "HUD1P1.X8",
      "HUD1P1.X24",
      "HUD1P1.X25",
      "HUD1P1.X1",
      "HUD1P1.X61",
      "HUD1P1.X55",
      "HUD1P1.X70",
      "HUD1P1.X16",
      "HUD1P1.X74",
      "HUD1P1.X7",
      "HUD1P1.X75",
      "HUD1P1.X89",
      "HUD1P1.X90",
      "HUD1P1.X91",
      "HUD1P1.X92",
      "HUD1P1.X93",
      "HUD1P1.X94",
      "HUD1P1.X95",
      "HUD1P1.X96",
      "HUD1P1.X97",
      "HUD1P1.X98",
      "HUD1P1.X99",
      "HUD1P1.X100",
      "HUD1P1.X101",
      "HUD1P1.X102",
      "HUD1P1.X103",
      "CD3.X136",
      "CD3.X137",
      "CD3.X138",
      "L726",
      "CD3.X272",
      "CD3.X374",
      "CD3.X379",
      "CD3.X569",
      "CD3.X1429",
      "CD3.X1428",
      "CD3.X1426",
      "CD3.X1425",
      "CD3.X1424",
      "CD3.X1423",
      "CD3.X1421",
      "CD3.X312",
      "CD3.X977",
      "CD3.X996",
      "CD3.X293",
      "CD3.X564",
      "CD3.X371",
      "CD3.X230",
      "CD3.X386",
      "CD3.X239",
      "CD3.X284",
      "CD3.X278",
      "CD3.X468",
      "CD3.X993",
      "CD3.X206",
      "CD3.X202",
      "CD3.X571",
      "CD3.X176",
      "CD3.X1206",
      "CD3.X383",
      "CD3.X436",
      "CD3.X1204",
      "CD3.X203",
      "CD3.X1203",
      "CD3.X158",
      "CD3.X1488",
      "CD3.X1487",
      "CD3.X1486",
      "CD3.X1485",
      "CD3.X1482",
      "CD3.X1413",
      "CD3.X295",
      "CD3.X221",
      "CD3.X1201",
      "CD3.X349",
      "CD3.X1200",
      "CD3.X415",
      "CD3.X1041",
      "CD3.X433",
      "CD3.X980",
      "CD3.X238",
      "CD3.X780",
      "CD3.X592",
      "CD3.X315",
      "CD3.X1500",
      "CD3.X1016",
      "CD3.X269",
      "CD3.X1484",
      "CD3.X392",
      "CD3.X251",
      "CD3.X444",
      "CD3.X1024",
      "CD3.X1022",
      "CD3.X966",
      "CD3.X227",
      "CD3.X1020",
      "CD3.X369",
      "CD3.X359",
      "CD3.X182",
      "CD3.X188",
      "CD3.X1450",
      "CD3.X957",
      "CD3.X409",
      "CD3.X1479",
      "CD3.X170",
      "CD3.X1042",
      "CD3.X254",
      "CD3.X1077",
      "CD3.X1076",
      "CD3.X1074",
      "CD3.X1073",
      "CD3.X1070",
      "CD3.X999",
      "CD3.X384",
      "CD3.X978",
      "CD3.X334",
      "CD3.X275",
      "CD3.X1207",
      "CD3.X578",
      "CD3.X378",
      "CD3.X152",
      "CD3.X1222",
      "CD3.X975",
      "CD3.X145",
      "CD3.X1205",
      "CD3.X390",
      "CD3.X210",
      "CD3.X234",
      "CD3.X365",
      "CD3.X1473",
      "CD3.X1035",
      "CD3.X264",
      "CD3.X330",
      "CD3.X354",
      "CD3.X362",
      "CD3.X1463",
      "CD3.X167",
      "CD3.X414",
      "CD3.X1032",
      "CD3.X574",
      "CD3.X219",
      "CD3.X984",
      "CD3.X372",
      "CD3.X281",
      "CD3.X368",
      "CD3.X218",
      "CD3.X348",
      "CD3.X1085",
      "CD3.X430",
      "CD3.X396",
      "CD3.X229",
      "CD3.X216",
      "CD3.X324",
      "CD3.X149",
      "CD3.X988",
      "CD3.X1171",
      "CD3.X393",
      "CD3.X1427",
      "CD3.X406",
      "CD3.X139",
      "CD3.X424",
      "CD3.X231",
      "CD3.X1235",
      "CD3.X146",
      "CD3.X688",
      "CD3.X328",
      "CD3.X289",
      "CD3.X517",
      "CD3.X164",
      "CD3.X306",
      "CD3.X1210",
      "CD3.X370",
      "CD3.X223",
      "CD3.X224",
      "CD3.X586",
      "CD3.X161",
      "CD3.X469",
      "CD3.X1117",
      "CD3.X321",
      "CD3.X357",
      "CD3.X248",
      "CD3.X515",
      "CD3.X1414",
      "CD3.X1430",
      "CD3.X998",
      "CD3.X997",
      "CD3.X1466",
      "CD3.X185",
      "CD3.X173",
      "CD3.X192",
      "CD3.X427",
      "CD3.X1231",
      "CD3.X1480",
      "CD3.X266",
      "CD3.X1090",
      "CD3.X541",
      "CD3.X991",
      "CD3.X1461",
      "CD3.X1218",
      "CD3.X160",
      "CD3.X245",
      "CD3.X300",
      "CD3.X305",
      "CD3.X870",
      "CD3.X1326",
      "CD3.X260",
      "CD3.X297",
      "CD3.X979",
      "CD3.X143",
      "CD3.X1467",
      "CD3.X539",
      "CD3.X375",
      "CD3.X1439",
      "CD3.X1438",
      "CD3.X1437",
      "CD3.X1435",
      "CD3.X1434",
      "CD3.X967",
      "CD3.X1422",
      "CD3.X313",
      "CD3.X237",
      "CD3.X1465",
      "CD3.X410",
      "CD3.X1464",
      "CD3.X580",
      "CD3.X294",
      "CD3.X565",
      "CD3.X1462",
      "CD3.X1420",
      "CD3.X288",
      "CD3.X217",
      "CD3.X310",
      "CD3.X323",
      "CD3.X153",
      "CD3.X994",
      "CD3.X387",
      "CD3.X207",
      "CD3.X162",
      "CD3.X329",
      "CD3.X1221",
      "CD3.X1220",
      "CD3.X204",
      "CD3.X973",
      "CD3.X159",
      "CD3.X169",
      "CD3.X1498",
      "CD3.X1497",
      "CD3.X1496",
      "CD3.X1495",
      "CD3.X1494",
      "CD3.X1493",
      "CD3.X177",
      "CD3.X416",
      "CD3.X381",
      "CD3.X434",
      "CD3.X201",
      "CD3.X156",
      "CD3.X981",
      "CD3.X1049",
      "CD3.X1469",
      "CD3.X1468",
      "CD3.X1460",
      "CD3.X174",
      "CD3.X316",
      "CD3.X1029",
      "CD3.X1027",
      "CD3.X1045",
      "CD3.X431",
      "CD3.X1044",
      "CD3.X302",
      "CD3.X493",
      "CD3.X252",
      "CD3.X1325",
      "CD3.X1324",
      "CD3.X318",
      "CD3.X1015",
      "CD3.X287",
      "CD3.X171",
      "CD3.X1040",
      "CD3.X1455",
      "CD3.X1028",
      "CD3.X403",
      "CD3.X1079",
      "CD3.X470",
      "CD3.X205",
      "CD3.X446",
      "CD3.X1086",
      "CD3.X1084",
      "CD3.X1083",
      "CD3.X1081",
      "CD3.X1419",
      "CD3.X583",
      "CD3.X1025",
      "CD3.X779",
      "CD3.X154",
      "CD3.X276",
      "CD3.X962",
      "CD3.X422",
      "CD3.X491",
      "CD3.X142",
      "CD3.X193",
      "CD3.X1058",
      "CD3.X1057",
      "CD3.X1056",
      "CD3.X1054",
      "CD3.X1023",
      "CD3.X958",
      "CD3.X1236",
      "CD3.X1234",
      "CD3.X1412",
      "CD3.X236",
      "CD3.X426",
      "CD3.X199",
      "CD3.X184",
      "CD3.X599",
      "CD3.X273",
      "CD3.X391",
      "CD3.X211",
      "CD3.X347",
      "CD3.X250",
      "CD3.X445",
      "CD3.X1209",
      "CD3.X1208",
      "CD3.X1202",
      "CD3.X366",
      "CD3.X404",
      "CD3.X228",
      "CD3.X270",
      "CD3.X331",
      "CD3.X419",
      "CD3.X1067",
      "CD3.X576",
      "CD3.X285",
      "CD3.X141",
      "CD3.X363",
      "CD3.X970",
      "CD3.X963",
      "CD3.X337",
      "CD3.X567",
      "CD3.X972",
      "CD3.X575",
      "CD3.X985",
      "CD3.X190",
      "CD3.X282",
      "CD3.X1501",
      "CD3.X987",
      "CD3.X360",
      "CD3.X397",
      "CD3.X194",
      "CD3.X494",
      "CD3.X1490",
      "CD3.X572",
      "CD3.X335",
      "CD3.X296",
      "CD3.X579",
      "CD3.X394",
      "CD3.X407",
      "CD3.X180",
      "CD3.X214",
      "CD3.X340",
      "CD3.X147",
      "CD3.X317",
      "CD3.X165",
      "CD3.X307",
      "CD3.X577",
      "CD3.X332",
      "CD3.X189",
      "CD3.X441",
      "CD3.X144",
      "CD3.X581",
      "CD3.X304",
      "CD3.X277",
      "CD3.X467",
      "CD3.X401",
      "CD3.X1225",
      "CD3.X400",
      "CD3.X516",
      "CD3.X1446",
      "CD3.X570",
      "CD3.X240",
      "CD3.X213",
      "CD3.X301",
      "CD3.X196",
      "CD3.X778",
      "CD3.X961",
      "CD3.X222",
      "CD3.X421",
      "CD3.X352",
      "CD3.X428",
      "CD3.X582",
      "CD3.X1078",
      "CD3.X1080",
      "CD3.X598",
      "CD3.X990",
      "CD3.X380",
      "CD3.X168",
      "CD3.X1444",
      "CD3.X1019",
      "CD3.X1018",
      "CD3.X1017",
      "CD3.X595",
      "CD3.X246",
      "CD3.X291",
      "CD3.X1013",
      "CD3.X336",
      "CD3.X1012",
      "CD3.X1489",
      "CD3.X1055",
      "CD3.X195",
      "CD3.X1442",
      "CD3.X150",
      "CD3.X1010",
      "CD3.X600",
      "CD3.X243",
      "CD3.X1440",
      "CD3.X1053",
      "CD3.X518",
      "CD3.X263",
      "CD3.X1052",
      "CD3.X437",
      "CD3.X358",
      "CD3.X429",
      "CD3.X1051",
      "CD3.X376",
      "CD3.X1483",
      "CD3.X1449",
      "CD3.X1448",
      "CD3.X1447",
      "CD3.X1445",
      "CD3.X1443",
      "CD3.X1441",
      "CD3.X589",
      "CD3.X989",
      "CD3.X1481",
      "CD3.X411",
      "CD3.X443",
      "CD3.X1071",
      "CD3.X389",
      "CD3.X355",
      "CD3.X413",
      "CD3.X325",
      "CD3.X1492",
      "CD3.X566",
      "CD3.X373",
      "CD3.X868",
      "CD3.X1491",
      "CD3.X257",
      "CD3.X1418",
      "CD3.X1417",
      "CD3.X1416",
      "CD3.X1415",
      "CD3.X268",
      "CD3.X311",
      "CD3.X388",
      "CD3.X995",
      "CD3.X208",
      "CD3.X377",
      "CD3.X151",
      "CD3.X1199",
      "CD3.X563",
      "CD3.X1472",
      "CD3.X385",
      "CD3.X1197",
      "CD3.X361",
      "CD3.X233",
      "CD3.X542",
      "CD3.X992",
      "CD3.X181",
      "CD3.X209",
      "CD3.X439",
      "CD3.X442",
      "CD3.X178",
      "CD3.X435",
      "CD3.X157",
      "CD3.X956",
      "CD3.X1478",
      "CD3.X1477",
      "CD3.X1476",
      "CD3.X1475",
      "CD3.X1474",
      "CD3.X1471",
      "CD3.X1470",
      "CD3.X220",
      "CD3.X175",
      "CD3.X1037",
      "CD3.X1036",
      "CD3.X1034",
      "CD3.X1033",
      "CD3.X1031",
      "CD3.X1030",
      "CD3.X1048",
      "CD3.X432",
      "CD3.X253",
      "CD3.X314",
      "CD3.X690",
      "CD3.X965",
      "CD3.X187",
      "CD3.X1009",
      "CD3.X1008",
      "CD3.X1014",
      "CD3.X960",
      "CD3.X1499",
      "CD3.X172",
      "CD3.X1043",
      "CD3.X1228",
      "CD3.X438",
      "CD3.X440",
      "CD3.X1005",
      "CD3.X226",
      "CD3.X327",
      "CD3.X140",
      "CD3.X1003",
      "CD3.X1436",
      "CD3.X1002",
      "CD3.X1011",
      "CD3.X964",
      "CD3.X1001",
      "CD3.X1000",
      "CD3.X1433",
      "CD3.X183",
      "CD3.X968",
      "CD3.X1432",
      "CD3.X418",
      "CD3.X1431",
      "CD3.X1021",
      "CD3.X186",
      "CD3.X1069",
      "CD3.X1066",
      "CD3.X1065",
      "CD3.X1064",
      "CD3.X1063",
      "CD3.X1062",
      "CD3.X1061",
      "CD3.X1060",
      "CD3.X1004",
      "CD3.X1075",
      "CD3.X959",
      "CD3.X399",
      "CD3.X274",
      "CD3.X212",
      "CD3.X1007",
      "CD3.X179",
      "CD3.X333",
      "CD3.X1219",
      "CD3.X1217",
      "CD3.X1213",
      "CD3.X1212",
      "CD3.X309",
      "CD3.X1087",
      "CD3.X976",
      "CD3.X1006",
      "CD3.X271",
      "CD3.X351",
      "CD3.X346",
      "CD3.X286",
      "CD3.X974",
      "CD3.X689",
      "CD3.X1082",
      "CD3.X364",
      "CD3.X971",
      "CD3.X343",
      "CD3.X1026",
      "CD3.X283",
      "CD3.X1050",
      "CD3.X398",
      "CD3.X573",
      "CD3.X983",
      "CD3.X280",
      "CD3.X303",
      "CD3.X322",
      "CD3.X319",
      "CD3.X395",
      "CD3.X408",
      "CD3.X292",
      "CD3.X492",
      "CD3.X215",
      "CD3.X191",
      "CD3.X568",
      "CD3.X148",
      "CD3.X585",
      "CD3.X166",
      "CD3.X308",
      "CD3.X405",
      "CD3.X423",
      "CD3.X279",
      "CD3.X367",
      "CD3.X267",
      "CD3.X163",
      "CD3.X1216",
      "CD3.X1068",
      "CD3.X402",
      "CD3.X1215",
      "CD3.X420",
      "CD3.X869",
      "CD3.X1214",
      "CD3.X241",
      "CD3.X1211",
      "CD3.X1144",
      "CD3.X197",
      "CD3.X417",
      "CD3.X320",
      "CD3.X425",
      "CD3.X584",
      "CD3.X382",
      "CD3.X242",
      "CD3.X353",
      "CD3.X969",
      "CD3.X1039",
      "CD3.X986",
      "CD3.X1038",
      "CD3.X249",
      "CD3.X1198",
      "CD3.X1196",
      "CD3.X225",
      "CD3.X350",
      "CD3.X290",
      "CD3.X247",
      "CD3.X1059",
      "CD3.X155",
      "CD3.X265",
      "CD3.X540",
      "CD3.X326",
      "CD3.X1072",
      "CD3.X244",
      "CD3.X982",
      "CD3.X299",
      "CD3.X200",
      "CD3.X1459",
      "CD3.X1458",
      "CD3.X1457",
      "CD3.X1456",
      "CD3.X1454",
      "CD3.X1453",
      "CD3.X1452",
      "CD3.X1451",
      "CD3.X235",
      "CD3.X232",
      "CD3.X198",
      "CD3.X298",
      "CD3.X412",
      "CD3.X356",
      "CD3.X1507",
      "CD3.X1508",
      "CD3.X1509",
      "CD3.X1510",
      "CD3.X1511",
      "CD3.X1512",
      "CD3.X1513",
      "CD3.X1514",
      "CD3.X1515",
      "CD3.X1516",
      "CD3.X1517",
      "CD3.X1518",
      "CD3.X1519",
      "CD3.X1520",
      "CD3.X1521",
      "CD3.X1522",
      "CD3.X1523",
      "CD3.X1524",
      "CD3.X1525",
      "CD3.X1526",
      "CD3.X1527",
      "CD3.X1528",
      "CD3.X1529",
      "CD3.X1530",
      "CD3.X1531",
      "CD3.X1532",
      "CD3.X1533",
      "CD3.X1534",
      "CD3.X1535",
      "CD3.X1536",
      "CD3.X1537",
      "CD3.X1538",
      "CD3.X1539",
      "CD3.X1540",
      "CD3.X1541",
      "CD3.X1542",
      "CD3.X1505",
      "220",
      "LE2.X5",
      "LE2.X6",
      "LE2.X7",
      "LE2.X8",
      "LE2.X9",
      "LE2.X10",
      "LE2.X11",
      "LE2.X12",
      "LE2.X13",
      "LE2.X14",
      "LE2.X15",
      "LE2.X16",
      "LE2.X17",
      "LE2.X18",
      "LE2.X19",
      "LE2.X20",
      "LE2.X21",
      "LE2.X22",
      "LE2.X23",
      "LE2.X24",
      "COMPLIANCEVERSION.CD3X1505",
      "LE3.X12",
      "LE3.X12",
      "CD4.X1",
      "CD4.X1",
      "672",
      "674",
      "1719",
      "CD4.X2",
      "CD4.X2",
      "CD4.X2",
      "CD4.X3",
      "CD4.X3",
      "CD4.X3",
      "2831",
      "2832",
      "1854",
      "3876",
      "3877",
      "1603",
      "LE2.X96",
      "689",
      "KBYO.XD689",
      "3",
      "KBYO.XD3",
      "2625",
      "KBYO.XD2625",
      "1699",
      "KBYO.XD1699",
      "LE2.X99",
      "694",
      "697",
      "KBYO.XD697",
      "695",
      "KBYO.XD695",
      "LE2.X97",
      "LE2.X98",
      "I_1699",
      "1177",
      "NEWHUD.X6",
      "1712",
      "CD4.X25",
      "CD4.X26",
      "CD4.X27",
      "CD4.X28",
      "CD4.X30",
      "CD4.X33",
      "CD4.X34",
      "CD4.X23",
      "1628",
      "660",
      "661",
      "HUD66",
      "HUD67",
      "NEWHUD.X337",
      "NEWHUD.X339",
      "NEWHUD.X338",
      "NEWHUD.X1726",
      "CD4.X4",
      "CD4.X5",
      "CD4.X6",
      "NEWHUD.X343",
      "NEWHUD.X337",
      "NEWHUD.X339",
      "NEWHUD.X338",
      "NEWHUD.X1726",
      "CD4.X4",
      "CD4.X5",
      "CD4.X6",
      "NEWHUD.X343",
      "NEWHUD.X1719",
      "HUD24",
      "1550",
      "DISCLOSURE.X913",
      "CD4.X7",
      "CD4.X8",
      "DISCLOSURE.X914",
      "608",
      "CD4.X9",
      "CD4.X10",
      "423",
      "663",
      "CD4.X40",
      "CD4.X41",
      "CD4.X42",
      "CD4.X43",
      "CD4.X31",
      "CD4.X44",
      "CD4.X45",
      "CD4.X49",
      "CD4.X50",
      "1677",
      "CD4.X46",
      "CD4.X51",
      "NEWHUD.X1728",
      "CD5.X1",
      "1206",
      "948",
      "799",
      "LE3.X16",
      "KBYO.LE3XD16",
      "CD5.X6",
      "CD5.X7",
      "CD5.X8",
      "CD5.X9",
      "CD5.X10",
      "CD5.X11",
      "CD5.X12",
      "CD5.X13",
      "CD5.X14",
      "CD5.X15",
      "CD5.X16",
      "CD5.X17",
      "CD5.X18",
      "CD5.X19",
      "CD5.X20",
      "CD5.X21",
      "CD5.X22",
      "CD5.X23",
      "CD5.X24",
      "CD5.X25",
      "CD5.X26",
      "CD5.X27",
      "CD5.X28",
      "CD5.X29",
      "CD5.X30",
      "CD5.X31",
      "CD5.X32",
      "CD5.X33",
      "CD5.X34",
      "CD5.X35",
      "CD5.X36",
      "CD5.X37",
      "CD5.X38",
      "CD5.X39",
      "CD5.X40",
      "CD5.X41",
      "CD5.X42",
      "CD5.X43",
      "CD5.X44",
      "CD5.X45",
      "CD5.X46",
      "CD5.X47",
      "CD5.X48",
      "CD5.X49",
      "CD5.X50",
      "CD5.X51",
      "CD5.X52",
      "CD5.X53",
      "CD5.X54",
      "CD5.X55",
      "CD5.X56",
      "CD5.X57",
      "CD5.X58",
      "CD5.X59",
      "CD5.X60",
      "CD5.X61",
      "CD5.X62",
      "CD5.X63",
      "CD5.X64",
      "CD5.X65",
      "CD5.X66",
      "CD5.X67",
      "4673",
      "4674",
      "CD3.XH87",
      "CD3.XH88",
      "CD3.X89",
      "CD3.XH90",
      "CD3.XH93",
      "CD3.X94",
      "CD3.XH95",
      "CD3.XH96",
      "CD3.XH97",
      "CD3.XH98",
      "CD3.XH99",
      "CD3.XH100",
      "LE1.XD45",
      "LE1.XD46",
      "LE1.XD54",
      "LE1.XD55",
      "LE1.XD63",
      "LE1.XD64",
      "LE1.XD72",
      "LE1.XD73",
      "LE2.XSTD_DV",
      "LE2.XSTI_DV",
      "CD1.XD11",
      "CD1.XD12",
      "CD1.XD20",
      "CD1.XD21",
      "CD1.XD29",
      "CD1.XD30",
      "CD1.XD38",
      "CD1.XD39"
    });
    public static readonly List<string> DisclosedLEFields = new List<string>((IEnumerable<string>) new string[789]
    {
      "1257",
      "1258",
      "1259",
      "1260",
      "1264",
      "LE2.XSTD",
      "LE2.XSTI",
      "LE2.XLC",
      "LE2.XSTJ",
      "LE2.X25",
      "LE2.X27",
      "LE1.X1",
      "1868",
      "1416",
      "1417",
      "1418",
      "1419",
      "1873",
      "11",
      "12",
      "14",
      "15",
      "LE1.X3",
      "LE1.X4",
      "19",
      "SYS.X6",
      "364",
      "4063",
      "762",
      "1821",
      "356",
      "136",
      "LE1.X2",
      "LE1.X5",
      "2400",
      "761",
      "LE1.X28",
      "1172",
      "1063",
      "dd_LE1X4",
      "LE1.X8",
      "I_LE1X6",
      "dd_LE1X7",
      "I_LE1X28",
      "LE1.X6",
      "dd_LE1X9",
      "I_LE1X8",
      "LE1.X7",
      "LE1.X9",
      "I_LE1X5",
      "LE1.X33",
      "LE1.X34",
      "LE1.X37",
      "LE1.X36",
      "LE1.X35",
      "LE1.X38",
      "LE1.X39",
      "3164",
      "3972",
      "3197",
      "3973",
      "3974",
      "I_3973",
      "3976",
      "3975",
      "LE1.X29",
      "NEWHUD.X349",
      "NEWHUD.X350",
      "NEWHUD.X351",
      "NEWHUD.X78",
      "LE1.X30",
      "LE1.X31",
      "LE1.X32",
      "2",
      "NEWHUD.X6",
      "3",
      "KBYO.XD3",
      "NEWHUD.X5",
      "5",
      "NEWHUD.X8",
      "675",
      "1659",
      "l_X13",
      "LE1.X13",
      "LE1.X14",
      "l_X15",
      "LE1.X15",
      "LE1.X17",
      "l_X555",
      "NEWHUD.X555",
      "KBYO.NEWHUDXD555",
      "LE1.X18",
      "l_X18",
      "LE1.X16",
      "l_X16",
      "LE1.X10",
      "NEWHUD.X7",
      "LE1.X11",
      "LE1.X12",
      "l_X12",
      "l_X315",
      "RE88395.X315",
      "LE1.X27",
      "l_X27",
      "RE88395.X121",
      "NEWHUD.X348",
      "l_X26",
      "LE1.X26",
      "HUD51",
      "l_X19",
      "LE1.X19",
      "LE1.X20",
      "l_X21",
      "LE1.X21",
      "l_X22",
      "LE1.X22",
      "LE1.X23",
      "l_X24",
      "LE1.X24",
      "l_X25",
      "LE1.X25",
      "NEWHUD2.XPJT",
      "608",
      "CD1.X7",
      "CD1.X8",
      "CD1.X9",
      "CD1.X15",
      "CD1.X16",
      "CD1.X17",
      "CD1.X18",
      "CD1.X24",
      "CD1.X25",
      "CD1.X26",
      "CD1.X27",
      "CD1.X35",
      "CD1.X36",
      "3165",
      "3167",
      "3168",
      "3169",
      "LE1.X78",
      "LE1.X79",
      "LE1.X80",
      "LE1.X81",
      "LE1.X82",
      "LE1.X83",
      "LE1.X84",
      "LE1.X85",
      "LE1.X86",
      "LE1.X87",
      "CD1.X42",
      "CD1.X43",
      "3291",
      "LE1.X41",
      "LE1.X42",
      "LE1.X43",
      "LE1.X44",
      "LE1.X45",
      "LE1.X46",
      "LE1.X47",
      "LE1.X48",
      "LE1.X49",
      "LE1.X50",
      "LE1.X51",
      "LE1.X52",
      "LE1.X53",
      "LE1.X54",
      "LE1.X55",
      "LE1.X56",
      "LE1.X57",
      "LE1.X58",
      "LE1.X59",
      "LE1.X60",
      "LE1.X61",
      "LE1.X62",
      "LE1.X63",
      "LE1.X64",
      "LE1.X65",
      "LE1.X66",
      "LE1.X67",
      "LE1.X68",
      "LE1.X69",
      "LE1.X70",
      "LE1.X71",
      "LE1.X72",
      "LE1.X73",
      "LE1.X74",
      "LE1.X75",
      "3287",
      "3288",
      "NEWHUD2.XPJTCOLUMNS",
      "3152",
      "LE1.X77",
      "LE1.XD1",
      "LE1.XD2",
      "LE1.XD3",
      "LE1.XD4",
      "LE1.XD8",
      "LE1.XD9",
      "LE1.XD28",
      "LE1.XD29",
      "LE1.XD42",
      "LE1.XD43",
      "LE1.XD45",
      "LE1.XD46",
      "LE1.XD47",
      "LE1.XD48",
      "LE1.XD51",
      "LE1.XD52",
      "LE1.XD54",
      "LE1.XD55",
      "LE1.XD56",
      "LE1.XD57",
      "LE1.XD60",
      "LE1.XD61",
      "LE1.XD63",
      "LE1.XD64",
      "LE1.XD65",
      "LE1.XD66",
      "LE1.XD69",
      "LE1.XD70",
      "LE1.XD72",
      "LE1.XD73",
      "LE1.XD74",
      "LE1.XD75",
      "LE1.XG9",
      "PAYMENTTABLE.CUSTOMIZE",
      "LOANTERMTABLE.CUSTOMIZE",
      "PAYMENTTABLE.USEACTUALPAYMENTCHANGE",
      "LE1.X88",
      "LE1.X89",
      "4085",
      "LE1.X90",
      "LE1.X91",
      "LE1.XD27",
      "CONST.X1",
      "4113",
      "KBYO.XD4113",
      "L726",
      "1176",
      "1177",
      "URLA.X267",
      "URLA.X269",
      "LE1.X98",
      "LE1.X99",
      "LE2.XSTA",
      "LE2.XSTB",
      "LE2.XSTC",
      "LE2.XSTD",
      "1177",
      "NEWHUD.X6",
      "1712",
      "CD4.X25",
      "CD4.X26",
      "CD4.X27",
      "CD4.X28",
      "CD4.X30",
      "CD4.X33",
      "CD4.X34",
      "CD4.X23",
      "LE2.XSTE",
      "LE2.XSTF",
      "LE2.XSTG",
      "LE2.XSTH",
      "LE2.XSTI",
      "LE2.XSTJ",
      "LE2.XDI",
      "LE2.XLC",
      "I_LE2X4",
      "LE2.X4",
      "220",
      "LE2.X5",
      "LE2.X6",
      "LE2.X7",
      "LE2.X8",
      "LE2.X9",
      "LE2.X10",
      "LE2.X11",
      "LE2.X12",
      "LE2.X13",
      "LE2.X14",
      "LE2.X15",
      "LE2.X16",
      "LE2.X17",
      "LE2.X18",
      "LE2.X19",
      "LE2.X20",
      "LE2.X21",
      "LE2.X22",
      "LE2.X23",
      "LE2.X24",
      "LE2.X25",
      "NEWHUD2.X23",
      "LE2.X3",
      "L128",
      "LE2.X2",
      "LE2.X1",
      "LE2.XSTJ",
      "I_NEWHUD2X23",
      "LE2.X27",
      "2",
      "LE2.XSTJ",
      "LE2.X26",
      "LE2.X1",
      "I_1092",
      "1092",
      "LE2.X28",
      "LE2.X96",
      "689",
      "KBYO.XD689",
      "3",
      "KBYO.XD3",
      "2625",
      "KBYO.XD2625",
      "1699",
      "KBYO.XD1699",
      "LE2.X99",
      "694",
      "697",
      "KBYO.XD697",
      "695",
      "KBYO.XD695",
      "LE2.X97",
      "LE2.X98",
      "I_1699",
      "423",
      "LE2.X100",
      "LE2.X29",
      "LE2.X30",
      "CD4.X31",
      "LE2.X31",
      "LE2.X32",
      "1677",
      "CD4.X46",
      "CD3.X1502",
      "L84",
      "L85",
      "CD3.X139",
      "CD3.X140",
      "CD3.X141",
      "CD3.X142",
      "CD3.X143",
      "CD3.X144",
      "CD3.X145",
      "CD3.X146",
      "CD3.X147",
      "CD3.X148",
      "CD3.X149",
      "CD3.X150",
      "CD3.X151",
      "CD3.X152",
      "CD3.X153",
      "CD3.X154",
      "CD3.X155",
      "CD3.X156",
      "CD3.X157",
      "CD3.X158",
      "CD3.X159",
      "CD3.X160",
      "CD3.X161",
      "CD3.X162",
      "CD3.X163",
      "CD3.X164",
      "CD3.X165",
      "CD3.X166",
      "CD3.X167",
      "CD3.X168",
      "CD3.X169",
      "CD3.X170",
      "CD3.X171",
      "CD3.X172",
      "CD3.X173",
      "CD3.X174",
      "CD3.X175",
      "CD3.X176",
      "CD3.X177",
      "CD3.X178",
      "CD3.X179",
      "CD3.X180",
      "CD3.X181",
      "CD3.X182",
      "CD3.X183",
      "CD3.X184",
      "CD3.X185",
      "CD3.X186",
      "CD3.X187",
      "CD3.X188",
      "CD3.X189",
      "CD3.X190",
      "CD3.X191",
      "CD3.X192",
      "CD3.X193",
      "CD3.X194",
      "CD3.X195",
      "CD3.X196",
      "CD3.X197",
      "CD3.X198",
      "CD3.X199",
      "CD3.X200",
      "CD3.X201",
      "CD3.X202",
      "CD3.X203",
      "CD3.X204",
      "CD3.X205",
      "CD3.X206",
      "CD3.X207",
      "CD3.X208",
      "CD3.X209",
      "CD3.X210",
      "CD3.X211",
      "CD3.X212",
      "CD3.X213",
      "CD3.X214",
      "CD3.X215",
      "CD3.X216",
      "CD3.X217",
      "CD3.X218",
      "CD3.X219",
      "CD3.X220",
      "CD3.X221",
      "CD3.X222",
      "L88",
      "L89",
      "CD3.X2",
      "CD3.X3",
      "CD3.X4",
      "CD3.X5",
      "CD3.X223",
      "CD3.X224",
      "CD3.X225",
      "CD3.X226",
      "CD3.X227",
      "CD3.X228",
      "CD3.X229",
      "CD3.X230",
      "CD3.X231",
      "CD3.X232",
      "CD3.X233",
      "CD3.X234",
      "CD3.X235",
      "CD3.X236",
      "CD3.X237",
      "CD3.X238",
      "CD3.X239",
      "CD3.X240",
      "CD3.X241",
      "CD3.X242",
      "CD3.X243",
      "CD3.X244",
      "CD3.X245",
      "CD3.X246",
      "CD3.X247",
      "CD3.X248",
      "CD3.X249",
      "CD3.X250",
      "CD3.X251",
      "CD3.X252",
      "CD3.X253",
      "CD3.X254",
      "L92",
      "L93",
      "L94",
      "L98",
      "L99",
      "L100",
      "L100",
      "L104",
      "L105",
      "L106",
      "CD3.X257",
      "CD3.X260",
      "CD3.X263",
      "L110",
      "L111",
      "L114",
      "L115",
      "L118",
      "L119",
      "L122",
      "L123",
      "CD3.X264",
      "CD3.X265",
      "CD3.X266",
      "CD3.X267",
      "CD3.X268",
      "CD3.X269",
      "CD3.X270",
      "CD3.X271",
      "CD3.X272",
      "CD3.X273",
      "CD3.X274",
      "CD3.X275",
      "CD3.X276",
      "CD3.X277",
      "CD3.X278",
      "CD3.X1503",
      "L134",
      "L135",
      "CD3.X279",
      "CD3.X280",
      "CD3.X281",
      "CD3.X282",
      "CD3.X283",
      "CD3.X284",
      "CD3.X285",
      "CD3.X286",
      "CD3.X287",
      "CD3.X288",
      "CD3.X289",
      "CD3.X10",
      "CD3.X9",
      "CD3.X11",
      "CD3.X12",
      "CD3.X290",
      "CD3.X291",
      "CD3.X292",
      "CD3.X293",
      "CD3.X294",
      "CD3.X295",
      "CD3.X296",
      "CD3.X297",
      "CD3.X298",
      "CD3.X299",
      "CD3.X300",
      "CD3.X301",
      "CD3.X302",
      "CD3.X303",
      "CD3.X304",
      "CD3.X305",
      "CD3.X306",
      "CD3.X307",
      "CD3.X308",
      "CD3.X309",
      "CD3.X13",
      "CD3.X14",
      "CD3.X15",
      "CD3.X16",
      "CD3.X17",
      "CD3.X18",
      "CD3.X19",
      "CD3.X20",
      "CD3.X310",
      "CD3.X311",
      "CD3.X312",
      "CD3.X313",
      "CD3.X314",
      "CD3.X315",
      "CD3.X316",
      "CD3.X317",
      "CD3.X318",
      "CD3.X319",
      "CD3.X320",
      "CD3.X321",
      "CD3.X322",
      "CD3.X323",
      "CD3.X324",
      "CD3.X325",
      "CD3.X326",
      "CD3.X327",
      "CD3.X328",
      "CD3.X329",
      "CD3.X330",
      "CD3.X331",
      "CD3.X332",
      "CD3.X333",
      "CD3.X334",
      "CD3.X335",
      "CD3.X336",
      "CD3.X337",
      "L156",
      "L157",
      "L158",
      "CD3.X340",
      "L162",
      "L163",
      "L164",
      "CD3.X343,",
      "L168",
      "L169",
      "L170",
      "CD3.X346",
      "L174",
      "L175",
      "L178",
      "L179",
      "L182",
      "L183",
      "CD3.X347",
      "CD3.X348",
      "CD3.X349",
      "CD3.X350",
      "CD3.X351",
      "CD3.X352",
      "CD3.X353",
      "CD3.X354",
      "CD3.X355",
      "CD3.X356",
      "CD3.X357",
      "CD3.X358",
      "CD3.X359",
      "CD3.X360",
      "CD3.X361",
      "CD3.X362",
      "CD3.X363",
      "CD3.X364",
      "CD3.X365",
      "CD3.X366",
      "CD3.X367",
      "CD3.X368",
      "CD3.X369",
      "CD3.X370",
      "CD3.X371",
      "CD3.X372",
      "CD3.X373",
      "CD3.X374",
      "CD3.X375",
      "CD3.X376",
      "CD3.X377",
      "CD3.X378",
      "CD3.X379",
      "CD3.X380",
      "CD3.X381",
      "CD3.X382",
      "CD3.X383",
      "CD3.X384",
      "CD3.X385",
      "CD3.X386",
      "CD3.X387",
      "CD3.X388",
      "CD3.X389",
      "CD3.X390",
      "CD3.X391",
      "CD3.X392",
      "CD3.X393",
      "CD3.X394",
      "CD3.X395",
      "CD3.X396",
      "CD3.X397",
      "CD3.X398",
      "CD3.X399",
      "CD3.X400",
      "CD3.X401",
      "CD3.X402",
      "CD3.X403",
      "CD3.X404",
      "CD3.X405",
      "CD3.X406",
      "CD3.X407",
      "CD3.X408",
      "CD3.X409",
      "CD3.X410",
      "CD3.X411",
      "CD3.X412",
      "CD3.X413",
      "CD3.X414",
      "CD3.X415",
      "CD3.X416",
      "CD3.X417",
      "CD3.X418",
      "CD3.X419",
      "CD3.X420",
      "CD3.X421",
      "CD3.X422",
      "CD3.X423",
      "CD3.X424",
      "CD3.X425",
      "CD3.X426",
      "CD3.X427",
      "CD3.X428",
      "CD3.X429",
      "CD3.X430",
      "CD3.X431",
      "CD3.X432",
      "CD3.X433",
      "CD3.X434",
      "CD3.X435",
      "CD3.X436",
      "CD3.X437",
      "CD3.X438",
      "CD3.X439",
      "CD3.X440",
      "CD3.X441",
      "CD3.X442",
      "CD3.X1503",
      "CD3.X1504",
      "CD3.X1505",
      "CD3.X1506",
      "CD3.X1507",
      "CD3.X1508",
      "CD3.X1509",
      "CD3.X1510",
      "CD3.X1511",
      "CD3.X1512",
      "CD3.X1513",
      "CD3.X1514",
      "CD3.X1515",
      "CD3.X1516",
      "CD3.X1517",
      "CD3.X1518",
      "CD3.X1519",
      "CD3.X1520",
      "CD3.X1521",
      "CD3.X1522",
      "CD3.X1523",
      "CD3.X1524",
      "CD3.X1525",
      "CD3.X1526",
      "CD3.X1527",
      "CD3.X1528",
      "CD3.X1529",
      "CD3.X1530",
      "CD3.X1531",
      "CD3.X1532",
      "CD3.X1533",
      "CD3.X1534",
      "CD3.X1535",
      "CD3.X1536",
      "CD3.X1537",
      "CD3.X1538",
      "CD3.X1539",
      "CD3.X1540",
      "CD3.X1541",
      "4673",
      "4674",
      "LE3.X17",
      "LE3.X18",
      "799",
      "KBYO.XD799",
      "LE3.X16",
      "KBYO.LE3XD16",
      "I_LE3X1",
      "I_LE3X2",
      "I_LE3X3",
      "I_LE3X4",
      "I_LE3X5",
      "1264",
      "3244",
      "LE3.X1",
      "LE3.X2",
      "LE3.X3",
      "LE3.X4",
      "LE3.X5",
      "LE3.X11",
      "LE3.X11",
      "LE3.X12",
      "LE3.X13",
      "672",
      "674",
      "2831",
      "3877",
      "3876",
      "1719",
      "2832",
      "1854",
      "RESPA.X28",
      "RESPA.X6",
      "LE3.X15",
      "VEND.X293",
      "VEND.X527",
      "LE3.X6",
      "LE3.X7",
      "LE3.X8",
      "LE3.X9",
      "LE3.X10",
      "I_LE3X6",
      "I_LE3X7",
      "I_LE3X8",
      "I_LE3X9",
      "I_LE3X10",
      "LE3.X19",
      "LE3.X20",
      "LE3.X21",
      "LE3.X22",
      "LE3.X23",
      "CD3.XH87",
      "CD3.XH88",
      "CD3.X89",
      "CD3.XH90",
      "CD3.XH93",
      "CD3.X94",
      "CD3.XH95",
      "CD3.XH96",
      "CD3.XH97",
      "CD3.XH98",
      "CD3.XH99",
      "CD3.XH100",
      "LE2.XSTD_DV",
      "LE2.XSTI_DV",
      "CD3.X80"
    });
    public static readonly List<string> DisclosedItemizationFields = new List<string>((IEnumerable<string>) new string[699]
    {
      "NEWHUD2.X28",
      "NEWHUD2.X29",
      "NEWHUD2.X30",
      "NEWHUD2.X31",
      "388",
      "389",
      "1620",
      "NEWHUD.X223",
      "NEWHUD.X224",
      "NEWHUD.X1718",
      "154",
      "1627",
      "1838",
      "1841",
      "NEWHUD.X732",
      "NEWHUD.X1235",
      "NEWHUD.X1243",
      "NEWHUD.X1251",
      "NEWHUD.X1259",
      "NEWHUD.X1267",
      "NEWHUD.X1275",
      "NEWHUD.X1283",
      "NEWHUD.X750",
      "NEWHUD.X126",
      "NEWHUD.X127",
      "NEWHUD.X128",
      "NEWHUD.X129",
      "NEWHUD.X130",
      "369",
      "371",
      "348",
      "931",
      "1390",
      "NEWHUD.X1291",
      "NEWHUD.X1299",
      "NEWHUD.X1307",
      "NEWHUD.X1315",
      "NEWHUD.X1323",
      "NEWHUD.X1331",
      "NEWHUD.X1339",
      "NEWHUD.X1347",
      "NEWHUD.X1355",
      "NEWHUD.X1363",
      "NEWHUD.X1371",
      "NEWHUD.X1379",
      "NEWHUD.X1387",
      "NEWHUD.X1395",
      "NEWHUD.X1403",
      "NEWHUD.X1411",
      "410",
      "NEWHUD.X656",
      "NEWHUD.X794",
      "NEWHUD.X1166",
      "NEWHUD.X1141",
      "NEWHUD.X1225",
      "NEWHUD.X1142",
      "NEWHUD.X1167",
      "NEWHUD.X1168",
      "NEWHUD.X1143",
      "NEWHUD.X1226",
      "NEWHUD.X1144",
      "NEWHUD.X1169",
      "NEWHUD.X1170",
      "NEWHUD.X1172",
      "NEWHUD.X1171",
      "NEWHUD.X1146",
      "NEWHUD.X1145",
      "NEWHUD.X1147",
      "NEWHUD.X1148",
      "NEWHUD.X1173",
      "NEWHUD.X1174",
      "NEWHUD.X1067",
      "NEWHUD.X1150",
      "NEWHUD.X1227",
      "NEWHUD.X1153",
      "NEWHUD.X1154",
      "NEWHUD.X1165",
      "NEWHUD.X1158",
      "NEWHUD.X1162",
      "NEWHUD.X1157",
      "NEWHUD.X1161",
      "NEWHUD2.X7",
      "NEWHUD.X1068",
      "1401",
      "1785",
      "NEWHUD2.X24",
      "NEWHUD2.X25",
      "NEWHUD2.X26",
      "NEWHUD2.X27",
      "L211",
      "L213",
      "L217",
      "NEWHUD2.X5",
      "NEWHUD2.X6",
      "NEWHUD2.X35",
      "NEWHUD2.X34",
      "NEWHUD2.X33",
      "NEWHUD2.X32",
      "SYS.X8",
      "332",
      "333",
      "L244",
      "L245",
      "NEWHUD.X651",
      "NEWHUD.X597",
      "NEWHUD.X582",
      "L251",
      "230",
      "NEWHUD.X598",
      "L259",
      "NEWHUD.X1594",
      "NEWHUD.X1586",
      "NEWHUD.X584",
      "NEWHUD.X583",
      "1666",
      "NEWHUD.X599",
      "NEWHUD.X600",
      "NEWHUD.X601",
      "NEWHUD2.X4397",
      "NEWHUD2.X4399",
      "NEWHUD2.X4401",
      "NEWHUD2.X4403",
      "NEWHUD2.X4405",
      "NEWHUD2.X4407",
      "NEWHUD2.X4409",
      "NEWHUD2.X4411",
      "NEWHUD2.X4400",
      "NEWHUD2.X4402",
      "NEWHUD2.X4404",
      "NEWHUD2.X4406",
      "NEWHUD2.X4408",
      "NEWHUD2.X4410",
      "NEWHUD2.X4412",
      "NEWHUD2.X39",
      "NEWHUD2.X38",
      "NEWHUD2.X37",
      "NEWHUD2.X36",
      "NEWHUD.X1719",
      "NEWHUD.X349",
      "NEWHUD.X350",
      "NEWHUD.X351",
      "NEWHUD.X78",
      "1387",
      "230",
      "1296",
      "l_232",
      "232",
      "1386",
      "231",
      "L267",
      "L268",
      "1388",
      "235",
      "1628",
      "1629",
      "1630",
      "660",
      "340",
      "253",
      "661",
      "341",
      "254",
      "NEWHUD.X1706",
      "NEWHUD.X1707",
      "NEWHUD.X1713",
      "NEWHUD2.X43",
      "NEWHUD2.X42",
      "NEWHUD2.X41",
      "NEWHUD2.X40",
      "NEWHUD.X951",
      "NEWHUD.X957",
      "NEWHUD.X960",
      "NEWHUD.X969",
      "NEWHUD.X978",
      "NEWHUD.X987",
      "NEWHUD.X996",
      "NEWHUD.X1002",
      "NEWHUD.X993",
      "NEWHUD.X984",
      "NEWHUD.X975",
      "NEWHUD.X966",
      "NEWHUD.X1017",
      "NEWHUD.X107",
      "NEWHUD.X573",
      "646",
      "1634",
      "NEWHUD.X206",
      "NEWHUD.X640",
      "NEWHUD.X207",
      "NEWHUD.X641",
      "NEWHUD.X208",
      "NEWHUD.X1610",
      "NEWHUD.X1602",
      "1777",
      "1772",
      "1767",
      "1762",
      "NEWHUD.X209",
      "NEWHUD.X576",
      "NEWHUD.X577",
      "NEWHUD.X578",
      "NEWHUD.X579",
      "NEWHUD.X580",
      "NEWHUD.X581",
      "NEWHUD.X809",
      "NEWHUD.X811",
      "NEWHUD.X813",
      "NEWHUD.X815",
      "NEWHUD.X817",
      "NEWHUD2.X96",
      "NEWHUD2.X97",
      "NEWHUD2.X98",
      "NEWHUD2.X99",
      "NEWHUD2.X100",
      "NEWHUD2.X101",
      "NEWHUD2.X102",
      "NEWHUD2.X103",
      "NEWHUD2.X47",
      "NEWHUD2.X46",
      "NEWHUD2.X45",
      "NEWHUD2.X44",
      "1636",
      "1637",
      "1638",
      "373",
      "1640",
      "1643",
      "NEWHUD.X1618",
      "NEWHUD.X1625",
      "NEWHUD2.X48",
      "NEWHUD2.X49",
      "NEWHUD2.X50",
      "NEWHUD2.X51",
      "NEWHUD.X251",
      "NEWHUD.X603",
      "NEWHUD.X108",
      "NEWHUD.X109",
      "650",
      "651",
      "NEWHUD.X110",
      "NEWHUD.X111",
      "40",
      "43",
      "NEWHUD.X112",
      "1782",
      "1787",
      "NEWHUD.X115",
      "NEWHUD.X114",
      "NEWHUD.X113",
      "1792",
      "NEWHUD.X252",
      "NEWHUD.X1656",
      "NEWHUD.X1648",
      "NEWHUD.X1640",
      "NEWHUD.X1632",
      "NEWHUD.X253",
      "NEWHUD.X800",
      "NEWHUD.X277",
      "NEWHUD.X278",
      "NEWHUD2.X53",
      "NEWHUD2.X52",
      "NEWHUD.X277",
      "NEWHUD.X278",
      "NEWHUD.X1585",
      "136",
      "L726",
      "967",
      "968",
      "1092",
      "138",
      "137",
      "969",
      "1093",
      "1073",
      "140",
      "143",
      "141",
      "1095",
      "1647",
      "1845",
      "1851",
      "1852",
      "1045",
      "2",
      "1844",
      "142",
      "202",
      "1091",
      "1106",
      "1646",
      "228",
      "229",
      "230",
      "1405",
      "232",
      "233",
      "912",
      "234",
      "BORPCC",
      "143",
      "BKRPCC",
      "LENPCC",
      "OTHPCC",
      "TNBPCC",
      "TOTPCC",
      "NEWHUD2.X23",
      "708",
      "709",
      "1252",
      "710",
      "1254",
      "93",
      "711",
      "707",
      "VEND.X165",
      "VEND.X164",
      "VEND.X163",
      "VEND.X162",
      "VEND.X157",
      "VEND.X158",
      "VEND.X159",
      "VEND.X160",
      "VEND.X17",
      "VEND.X16",
      "VEND.X15",
      "VEND.X14",
      "VEND.X20",
      "VEND.X21",
      "VEND.X19",
      "VEND.X13",
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
      "NEWHUD.X1149",
      "1109",
      "1115",
      "NEWHUD2.X3335",
      "NEWHUD2.X4196",
      "NEWHUD2.X4229",
      "NEWHUD2.X4262",
      "NEWHUD2.X4295",
      "NEWHUD2.X4328",
      "NEWHUD2.X4361",
      "3292",
      "NEWHUD.X691",
      "558",
      "NEWHUD.X949",
      "NEWHUD2.X913",
      "647",
      "648",
      "NEWHUD2.X77",
      "NEWHUD2.X89",
      "NEWHUD2.X923",
      "NEWHUD2.X80",
      "NEWHUD2.X68",
      "NEWHUD2.X62",
      "NEWHUD2.X74",
      "NEWHUD2.X71",
      "NEWHUD2.X95",
      "NEWHUD2.X65",
      "NEWHUD2.X92",
      "NEWHUD2.X83",
      "NEWHUD2.X86",
      "NEWHUD2.X4427",
      "NEWHUD2.X4428",
      "NEWHUD2.X4398",
      "NEWHUD2.X4416",
      "NEWHUD2.X4415",
      "NEWHUD2.X4418",
      "NEWHUD2.X4417",
      "NEWHUD2.X4420",
      "NEWHUD2.X4419",
      "NEWHUD2.X4422",
      "NEWHUD2.X4421",
      "NEWHUD2.X4424",
      "NEWHUD2.X4423",
      "NEWHUD2.X4426",
      "NEWHUD2.X4425",
      "NEWHUD2.X133",
      "NEWHUD2.X134",
      "NEWHUD2.X135",
      "NEWHUD2.X136",
      "NEWHUD2.X137",
      "NEWHUD2.X126",
      "NEWHUD2.X124",
      "NEWHUD2.X125",
      "NEWHUD2.X138",
      "NEWHUD2.X129",
      "NEWHUD2.X127",
      "NEWHUD2.X128",
      "NEWHUD2.X140",
      "NEWHUD2.X139",
      "NEWHUD2.X132",
      "NEWHUD2.X130",
      "NEWHUD2.X131",
      "1172",
      "NEWHUD2.X4435",
      "NEWHUD2.X4436",
      "NEWHUD2.X4437",
      "NEWHUD2.X4438",
      "NEWHUD2.X4439",
      "NEWHUD2.X4440",
      "NEWHUD2.X4441",
      "NEWHUD2.X4442",
      "NEWHUD2.X4443",
      "NEWHUD2.X4444",
      "NEWHUD2.X4447",
      "NEWHUD2.X4480",
      "NEWHUD2.X4513",
      "NEWHUD2.X4546",
      "NEWHUD2.X4579",
      "NEWHUD2.X4610",
      "NEWHUD2.X4617",
      "NEWHUD2.X4624",
      "NEWHUD2.X4631",
      "NEWHUD2.X4638",
      "NEWHUD2.X4660",
      "NEWHUD2.X4683",
      "NEWHUD2.X4706",
      "NEWHUD2.X4729",
      "19",
      "NEWHUD2.X4760",
      "NEWHUD2.X4761",
      "NEWHUD2.X4762",
      "NEWHUD2.X4763",
      "4794",
      "4795",
      "4796",
      "4855",
      "4856",
      "NEWHUD2.X4769",
      "NEWHUD2.X4780",
      "NEWHUD2.X4781",
      "NEWHUD2.X4782",
      "NEWHUD2.X4783",
      "NEWHUD2.X4784",
      "NEWHUD2.X4785",
      "NEWHUD2.X319",
      "NEWHUD2.X352",
      "NEWHUD2.X385",
      "NEWHUD2.X418",
      "NEWHUD2.X451",
      "NEWHUD2.X484",
      "NEWHUD2.X517",
      "NEWHUD2.X550",
      "NEWHUD2.X583",
      "NEWHUD2.X616",
      "NEWHUD2.X649",
      "NEWHUD2.X682",
      "NEWHUD2.X715",
      "NEWHUD2.X748",
      "NEWHUD2.X781",
      "NEWHUD2.X814",
      "NEWHUD2.X847",
      "NEWHUD2.X880",
      "NEWHUD2.X946",
      "NEWHUD2.X979",
      "NEWHUD2.X1012",
      "NEWHUD2.X1045",
      "NEWHUD2.X1078",
      "NEWHUD2.X1111",
      "NEWHUD2.X1144",
      "NEWHUD2.X1177",
      "NEWHUD2.X1210",
      "NEWHUD2.X1243",
      "NEWHUD2.X1276",
      "NEWHUD2.X1309",
      "NEWHUD2.X1342",
      "NEWHUD2.X1375",
      "NEWHUD2.X1408",
      "NEWHUD2.X1441",
      "NEWHUD2.X1474",
      "NEWHUD2.X1507",
      "NEWHUD2.X1540",
      "NEWHUD2.X1573",
      "NEWHUD2.X1606",
      "NEWHUD2.X1639",
      "NEWHUD2.X1672",
      "NEWHUD2.X1705",
      "NEWHUD2.X1738",
      "NEWHUD2.X1771",
      "NEWHUD2.X1804",
      "NEWHUD2.X1837",
      "NEWHUD2.X1870",
      "NEWHUD2.X1903",
      "NEWHUD2.X1936",
      "NEWHUD2.X1969",
      "NEWHUD2.X2002",
      "NEWHUD2.X2035",
      "NEWHUD2.X2068",
      "NEWHUD2.X2101",
      "NEWHUD2.X2134",
      "NEWHUD2.X2860",
      "NEWHUD2.X2893",
      "NEWHUD2.X2926",
      "NEWHUD2.X2959",
      "NEWHUD2.X2992",
      "NEWHUD2.X3025",
      "NEWHUD2.X3058",
      "NEWHUD2.X3091",
      "NEWHUD2.X3124",
      "NEWHUD2.X3157",
      "NEWHUD2.X3190",
      "NEWHUD2.X3223",
      "NEWHUD2.X3256",
      "NEWHUD2.X3289",
      "NEWHUD2.X3322",
      "NEWHUD2.X3355",
      "NEWHUD2.X3388",
      "NEWHUD2.X3421",
      "NEWHUD2.X3454",
      "NEWHUD2.X3487",
      "NEWHUD2.X3520",
      "NEWHUD2.X3553",
      "NEWHUD2.X3586",
      "NEWHUD2.X3619",
      "NEWHUD2.X3652",
      "NEWHUD2.X3685",
      "NEWHUD2.X3718",
      "NEWHUD2.X3751",
      "NEWHUD2.X3784",
      "NEWHUD2.X3817",
      "NEWHUD2.X3850",
      "NEWHUD2.X3883",
      "NEWHUD2.X3916",
      "NEWHUD2.X3949",
      "NEWHUD2.X3982",
      "NEWHUD2.X4015",
      "NEWHUD2.X4048",
      "NEWHUD2.X4081",
      "NEWHUD2.X4114",
      "NEWHUD2.X4147",
      "NEWHUD2.X4180",
      "NEWHUD2.X4213",
      "NEWHUD2.X4246",
      "NEWHUD2.X4279",
      "NEWHUD2.X4312",
      "NEWHUD2.X4345",
      "NEWHUD2.X4378",
      "454",
      "L228",
      "1621",
      "367",
      "439",
      "155",
      "1625",
      "1839",
      "1842",
      "NEWHUD.X733",
      "NEWHUD.X1237",
      "NEWHUD.X1245",
      "NEWHUD.X1253",
      "NEWHUD.X1261",
      "NEWHUD.X1269",
      "NEWHUD.X1277",
      "NEWHUD.X1285",
      "NEWHUD2.X9",
      "641",
      "640",
      "336",
      "NEWHUD.X400",
      "NEWHUD.X136",
      "NEWHUD.X137",
      "NEWHUD.X138",
      "NEWHUD.X139",
      "NEWHUD.X140",
      "370",
      "372",
      "349",
      "932",
      "1009",
      "NEWHUD.X1293",
      "NEWHUD.X1301",
      "NEWHUD.X1309",
      "NEWHUD.X1317",
      "NEWHUD.X1325",
      "NEWHUD.X1333",
      "NEWHUD.X1341",
      "NEWHUD.X1349",
      "NEWHUD.X1357",
      "NEWHUD.X1365",
      "NEWHUD.X1373",
      "NEWHUD.X1381",
      "NEWHUD.X1389",
      "NEWHUD.X1397",
      "NEWHUD.X1405",
      "NEWHUD.X1413",
      "554",
      "NEWHUD.X657",
      "NEWHUD2.X2207",
      "NEWHUD2.X2207",
      "337",
      "1050",
      "NEWHUD.X571",
      "NEWHUD2.X2867",
      "NEWHUD2.X2867",
      "NEWHUD.X952",
      "NEWHUD2.X2900",
      "NEWHUD2.X2900",
      "NEWHUD.X961",
      "NEWHUD2.X2933",
      "NEWHUD2.X2933",
      "NEWHUD.X970",
      "NEWHUD2.X2966",
      "NEWHUD2.X2966",
      "NEWHUD.X979",
      "NEWHUD2.X2999",
      "NEWHUD2.X2999",
      "NEWHUD.X988",
      "NEWHUD2.X3032",
      "NEWHUD2.X3032",
      "NEWHUD.X997",
      "NEWHUD.X645",
      "NEWHUD2.X3065",
      "NEWHUD2.X3065",
      "NEWHUD2.X11",
      "NEWHUD2.X3098",
      "NEWHUD2.X3098",
      "NEWHUD2.X14",
      "NEWHUD2.X3131",
      "NEWHUD2.X3131",
      "NEWHUD.X808",
      "NEWHUD2.X3164",
      "NEWHUD2.X3164",
      "NEWHUD.X810",
      "NEWHUD2.X3197",
      "NEWHUD2.X3197",
      "NEWHUD.X812",
      "NEWHUD2.X3230",
      "NEWHUD2.X3230",
      "NEWHUD.X814",
      "NEWHUD2.X3263",
      "NEWHUD2.X3263",
      "NEWHUD.X816",
      "NEWHUD2.X3296",
      "NEWHUD2.X3296",
      "NEWHUD.X818",
      "NEWHUD2.X3362",
      "NEWHUD2.X3362",
      "NEWHUD.X639",
      "NEWHUD2.X3395",
      "NEWHUD2.X3395",
      "NEWHUD.X215",
      "NEWHUD2.X3428",
      "NEWHUD2.X3428",
      "NEWHUD.X216",
      "NEWHUD2.X3461",
      "NEWHUD2.X3461",
      "1763",
      "NEWHUD2.X3494",
      "NEWHUD2.X3494",
      "1768",
      "NEWHUD2.X3527",
      "NEWHUD2.X3527",
      "1773",
      "NEWHUD2.X3560",
      "NEWHUD2.X3560",
      "1778",
      "NEWHUD.X254",
      "644",
      "645",
      "41",
      "44",
      "1783",
      "1788",
      "1793",
      "NEWHUD.X731",
      "FV.X348",
      "FV.X366",
      "3171",
      "3172",
      "3173",
      "FV.X208",
      "FV.X210",
      "FV.X26",
      "FV.X52",
      "FV.X205",
      "FV.X206",
      "FV.X207",
      "FV.X24",
      "FV.X50",
      "FV.X208",
      "FV.X321",
      "FV.X396",
      "FV.X397"
    });
    public static readonly List<string> DiscloseCOCFields = new List<string>((IEnumerable<string>) new string[15]
    {
      "XCOC0001",
      "XCOC0003",
      "XCOC0004",
      "XCOC0005",
      "XCOC0006",
      "XCOC0007",
      "XCOC0008",
      "XCOC0009",
      "XCOC0010",
      "XCOC0011",
      "XCOC0012",
      "XCOC0013",
      "XCOC0014",
      "XCOC0015",
      "XCOC0098"
    });
    public static readonly List<string> DisclosedSSPLFields = new List<string>((IEnumerable<string>) new string[33]
    {
      "SP0001",
      "SP0002",
      "SP0003",
      "SP0004",
      "SP0005",
      "SP0006",
      "SP0007",
      "SP0010",
      "SP0012",
      "SP0013",
      "SP0014",
      "SP0015",
      "SP0016",
      "SP0017",
      "SP0018",
      "SP0019",
      "SP0020",
      "SP0021",
      "SP0022",
      "SP0023",
      "SP0024",
      "SP0025",
      "SP0026",
      "SP0027",
      "SP0028",
      "SP0029",
      "SP0030",
      "SP0031",
      "SP0032",
      "SP0033",
      "SP0034",
      "SP0035",
      "SP0036"
    });
    public static readonly List<string> DisclosedNBOFields = new List<string>((IEnumerable<string>) new string[24]
    {
      "NBOC0001",
      "NBOC0002",
      "NBOC0003",
      "NBOC0004",
      "NBOC0005",
      "NBOC0006",
      "NBOC0007",
      "NBOC0008",
      "NBOC0009",
      "NBOC0010",
      "NBOC0011",
      "NBOC0012",
      "NBOC0013",
      "NBOC0014",
      "NBOC0015",
      "NBOC0016",
      "NBOC0017",
      "NBOC0018",
      "NBOC0019",
      "NBOC0020",
      "NBOC0022",
      "NBOC0023",
      "NBOC0098",
      "NBOC0099"
    });
    public static readonly List<string> DisclosedVestingFields = new List<string>((IEnumerable<string>) new string[15]
    {
      "TR0001",
      "TR0002",
      "TR0003",
      "TR0004",
      "TR0005",
      "TR0006",
      "TR0007",
      "TR0008",
      "TR0009",
      "TR0010",
      "TR0011",
      "TR0012",
      "TR0013",
      "TR0014",
      "TR0099"
    });
    public static readonly string XmlType = "DisclosureTracking2015";
    private string disclosedByFullName = "";
    private string sentMethodOther;
    private bool useForUCDExport;
    private bool isDisclosed = true;
    private bool containCD;
    private bool containLE;
    private bool providerListSent;
    private bool providerListNoFeeSent;
    private DateTime receivedDate = DateTime.MinValue;
    private bool isLocked;
    private bool isDisclosedAPRLocked;
    private bool isDisclosedFinanceChargeLocked;
    private bool isDisclosedDailyInterestLocked;
    private bool isDisclosedReceivedDateLocked;
    private string lockedDisclosedByField = "";
    private DateTime lockedDisclosedDateField = DateTime.MinValue;
    private string lockedDisclosedAPRField = "";
    private string lockedDisclosedFinanceChargeField = "";
    private string disclosedDailyInterest = "";
    private string disclosedAPR = "";
    private string financeCharge = "";
    private string lockedDisclosedDailyInterestField = "";
    private bool intentToProceed;
    private string intentToProceedReceivedBy;
    private DateTime intentToProceedDate;
    private DisclosureTrackingBase.DisclosedMethod intentToProceedReceivedMethod;
    private string intentToProceedMethodOther;
    private string intentToProceedComment;
    private bool leReasonIsChangedCircumstanceSettlementCharges;
    private bool leReasonIsChangedCircumstanceEligibility;
    private bool leReasonIsRevisionsRequestedByConsumer;
    private bool leReasonIsInterestRateDependentCharges;
    private bool leReasonIsExpiration;
    private bool leReasonIsDelayedSettlementOnConstructionLoans;
    private bool leReasonIsOther;
    private bool cdReasonIsChangeInAPR;
    private bool cdReasonIsChangeInLoanProduct;
    private bool cdReasonIsPrepaymentPenaltyAdded;
    private bool cdReasonIsChangeInSettlementCharges;
    private bool cdReasonIs24HourAdvancePreview;
    private bool cdReasonIsToleranceCure;
    private bool cdReasonIsClericalErrorCorrection;
    private bool cdReasonIsChangedCircumstanceEligibility;
    private bool cdReasonIsRevisionsRequestedByConsumer;
    private bool cdReasonIsInterestRateDependentCharges;
    private bool cdReasonIsOther;
    private string leReasonOther;
    private string cdReasonOther;
    private string changeInCircumstance;
    private string changeInCircumstanceComments;
    private DisclosureTracking2015Log.DisclosureTypeEnum disclosureType;
    private DisclosureTrackingBase.DisclosedMethod borrowerDisclosedMethod;
    private string borrowerDisclosedMethodOther;
    private DateTime borrowerPresumedReceivedDate;
    private DateTime lockedBorrowerPresumedReceivedDate;
    private bool isBorrowerPresumedDateLocked;
    private DateTime borrowerActualReceivedDate;
    private DisclosureTrackingBase.DisclosedMethod coBorrowerDisclosedMethod;
    private string coBorrowerDisclosedMethodOther;
    private DateTime coBorrowerPresumedReceivedDate;
    private DateTime lockedCoBorrowerPresumedReceivedDate;
    private bool isCoBorrowerPresumedDateLocked;
    private DateTime coBorrowerActualReceivedDate;
    private bool isIntentReceivedByLocked;
    private string lockedIntentReceivedByField = "";
    private bool leDisclosedByBroker;
    private string edsRequestGuid = "";
    private string linkedGuid = "";
    private bool borrowerLoanLevelConsentMapForCC;
    private bool coBorrowerLoanLevelConsentMapForCC;
    private string ucd = "";
    private string udt = "";
    private LoanData loanData;
    private string borrowerFulfillmentMethodDescription = "";
    private string coBorrowerFulfillmentMethodDescription = "";
    private DateTime presumedFulfillmentDate;
    private DateTime actualFulfillmentDate;
    private Dictionary<string, string> attributeList = new Dictionary<string, string>();
    private Dictionary<string, INonBorrowerOwnerItem> nonBorrowerOwnerCollections = new Dictionary<string, INonBorrowerOwnerItem>();
    private DateTime changesReceivedDate;
    private DateTime revisedDueDate;
    protected List<DisclosedLoanItem> tempList = new List<DisclosedLoanItem>();
    private bool ucdCreationError;
    private List<string[]> itemizationFields = new List<string[]>();
    private static ConcurrentDictionary<string, Dictionary<string, string>> cachedDisclosureTrackingLoanSnapShots = new ConcurrentDictionary<string, Dictionary<string, string>>();
    protected Hashtable loanDataFromOtherLogs = new Hashtable();

    protected override bool ReadDateInUtc => true;

    public DisclosureTracking2015Log(
      DateTime date,
      LoanData loanData,
      bool containLE,
      bool containCD,
      bool manuallyCreated,
      bool containSafeHarbor,
      bool containsSettlementServicesProvider,
      bool containsSSP_NoFee = false)
      : base(date.ToUniversalTime(), loanData, manuallyCreated, containSafeHarbor, true)
    {
      if (containCD)
        this.containCD = true;
      if (containLE)
        this.containLE = true;
      if (containsSettlementServicesProvider)
        this.providerListSent = true;
      if (containsSSP_NoFee)
        this.providerListNoFeeSent = true;
      this.loanData = loanData;
      this.populateAttributes(loanData);
      this.udt = this.extractLoanData(loanData);
      this.setFormFields();
      this.createNBO();
    }

    public DisclosureTracking2015Log(LogList log, XmlElement e)
      : base(log, e, true)
    {
      AttributeReader attributeReader1 = new AttributeReader(e);
      this.loanData = log.Loan;
      this.containCD = this.convertStringToBool(attributeReader1.GetString("ContainCD"));
      this.containLE = this.convertStringToBool(attributeReader1.GetString("ContainLE"));
      this.providerListSent = this.convertStringToBool(attributeReader1.GetString(nameof (ProviderListSent)));
      this.providerListNoFeeSent = this.convertStringToBool(attributeReader1.GetString(nameof (ProviderListNoFeeSent)));
      this.sentMethodOther = attributeReader1.GetString(nameof (DisclosedMethodOther));
      this.useForUCDExport = this.convertStringToBool(attributeReader1.GetString(nameof (UseForUCDExport)));
      this.isDisclosed = this.convertStringToBool(attributeReader1.GetString(nameof (IsDisclosed)));
      this.isLocked = this.convertStringToBool(attributeReader1.GetString(nameof (IsLocked)));
      this.lockedDisclosedByField = attributeReader1.GetString(nameof (LockedDisclosedByField));
      this.lockedDisclosedDateField = attributeReader1.GetDate(nameof (LockedDisclosedDateField));
      this.disclosedByFullName = attributeReader1.GetString(nameof (DisclosedByFullName));
      try
      {
        this.disclosureType = (DisclosureTracking2015Log.DisclosureTypeEnum) Enum.Parse(typeof (DisclosureTracking2015Log.DisclosureTypeEnum), attributeReader1.GetString(nameof (DisclosureType)), true);
      }
      catch
      {
      }
      this.leReasonIsChangedCircumstanceSettlementCharges = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsChangedCircumstanceSettlementCharges)));
      this.leReasonIsChangedCircumstanceEligibility = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsChangedCircumstanceEligibility)));
      this.leReasonIsRevisionsRequestedByConsumer = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsRevisionsRequestedByConsumer)));
      this.leReasonIsInterestRateDependentCharges = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsInterestRateDependentCharges)));
      this.leReasonIsExpiration = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsExpiration)));
      this.leReasonIsDelayedSettlementOnConstructionLoans = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsDelayedSettlementOnConstructionLoans)));
      this.leReasonIsOther = this.convertStringToBool(attributeReader1.GetString(nameof (LEReasonIsOther)));
      this.leReasonOther = attributeReader1.GetString(nameof (LEReasonOther));
      this.cdReasonIsChangeInAPR = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsChangeInAPR)));
      this.cdReasonIsChangeInLoanProduct = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsChangeInLoanProduct)));
      this.cdReasonIsPrepaymentPenaltyAdded = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsPrepaymentPenaltyAdded)));
      this.cdReasonIsChangeInSettlementCharges = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsChangeInSettlementCharges)));
      this.cdReasonIs24HourAdvancePreview = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIs24HourAdvancePreview)));
      this.cdReasonIsToleranceCure = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsToleranceCure)));
      this.cdReasonIsClericalErrorCorrection = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsClericalErrorCorrection)));
      this.cdReasonIsChangedCircumstanceEligibility = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsChangedCircumstanceEligibility)));
      this.cdReasonIsRevisionsRequestedByConsumer = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsRevisionsRequestedByConsumer)));
      this.cdReasonIsInterestRateDependentCharges = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsInterestRateDependentCharges)));
      this.cdReasonIsOther = this.convertStringToBool(attributeReader1.GetString(nameof (CDReasonIsOther)));
      this.cdReasonOther = attributeReader1.GetString(nameof (CDReasonOther));
      this.changeInCircumstance = attributeReader1.GetString(nameof (ChangeInCircumstance));
      this.changeInCircumstanceComments = attributeReader1.GetString(nameof (ChangeInCircumstanceComments));
      this.intentToProceed = this.convertStringToBool(attributeReader1.GetString(nameof (IntentToProceed)));
      this.intentToProceedDate = attributeReader1.GetDate(nameof (IntentToProceedDate));
      this.intentToProceedReceivedBy = attributeReader1.GetString(nameof (IntentToProceedReceivedBy));
      try
      {
        this.intentToProceedReceivedMethod = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), attributeReader1.GetString(nameof (IntentToProceedReceivedMethod)), true);
      }
      catch
      {
      }
      this.intentToProceedMethodOther = attributeReader1.GetString(nameof (IntentToProceedReceivedMethodOther));
      this.intentToProceedComment = attributeReader1.GetString(nameof (IntentToProceedComments));
      this.isIntentReceivedByLocked = this.convertStringToBool(attributeReader1.GetString(nameof (IsIntentReceivedByLocked)));
      this.lockedIntentReceivedByField = attributeReader1.GetString(nameof (LockedIntentReceivedByField), false);
      try
      {
        this.borrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), attributeReader1.GetString(nameof (BorrowerDisclosedMethod)), true);
      }
      catch
      {
      }
      this.borrowerDisclosedMethodOther = attributeReader1.GetString(nameof (BorrowerDisclosedMethodOther));
      this.borrowerPresumedReceivedDate = attributeReader1.GetDate(nameof (BorrowerPresumedReceivedDate), DateTime.MinValue);
      this.lockedBorrowerPresumedReceivedDate = attributeReader1.GetDate(nameof (LockedBorrowerPresumedReceivedDate));
      this.isBorrowerPresumedDateLocked = this.convertStringToBool(attributeReader1.GetString(nameof (IsBorrowerPresumedDateLocked)));
      this.borrowerActualReceivedDate = attributeReader1.GetDate(nameof (BorrowerActualReceivedDate), DateTime.MinValue);
      try
      {
        this.coBorrowerDisclosedMethod = (DisclosureTrackingBase.DisclosedMethod) Enum.Parse(typeof (DisclosureTrackingBase.DisclosedMethod), attributeReader1.GetString(nameof (CoBorrowerDisclosedMethod)), true);
      }
      catch
      {
      }
      this.coBorrowerDisclosedMethodOther = attributeReader1.GetString(nameof (CoBorrowerDisclosedMethodOther));
      this.coBorrowerPresumedReceivedDate = attributeReader1.GetDate(nameof (CoBorrowerPresumedReceivedDate), DateTime.MinValue);
      this.lockedCoBorrowerPresumedReceivedDate = attributeReader1.GetDate(nameof (LockedCoBorrowerPresumedReceivedDate));
      this.isCoBorrowerPresumedDateLocked = this.convertStringToBool(attributeReader1.GetString(nameof (IsCoBorrowerPresumedDateLocked)));
      this.coBorrowerActualReceivedDate = attributeReader1.GetDate(nameof (CoBorrowerActualReceivedDate), DateTime.MinValue);
      this.leDisclosedByBroker = this.convertStringToBool(attributeReader1.GetString(nameof (LEDisclosedByBroker)));
      this.isDisclosedAPRLocked = this.convertStringToBool(attributeReader1.GetString(nameof (isDisclosedAPRLocked), false));
      this.isDisclosedFinanceChargeLocked = this.convertStringToBool(attributeReader1.GetString(nameof (isDisclosedFinanceChargeLocked), false));
      this.isDisclosedReceivedDateLocked = this.convertStringToBool(attributeReader1.GetString(nameof (isDisclosedReceivedDateLocked), false));
      this.lockedDisclosedAPRField = attributeReader1.GetString(nameof (lockedDisclosedAPRField), false);
      this.lockedDisclosedByField = attributeReader1.GetString(nameof (lockedDisclosedByField), false);
      this.lockedDisclosedFinanceChargeField = attributeReader1.GetString(nameof (lockedDisclosedFinanceChargeField), false);
      this.isDisclosedDailyInterestLocked = this.convertStringToBool(attributeReader1.GetString(nameof (isDisclosedDailyInterestLocked), false));
      this.lockedDisclosedDailyInterestField = attributeReader1.GetString(nameof (lockedDisclosedDailyInterestField), false);
      this.borrowerFulfillmentMethodDescription = attributeReader1.GetString(nameof (borrowerFulfillmentMethodDescription), false);
      this.coBorrowerFulfillmentMethodDescription = attributeReader1.GetString(nameof (coBorrowerFulfillmentMethodDescription), false);
      this.presumedFulfillmentDate = attributeReader1.GetDate(nameof (presumedFulfillmentDate), DateTime.MinValue);
      this.actualFulfillmentDate = attributeReader1.GetDate(nameof (actualFulfillmentDate), DateTime.MinValue);
      this.disclosedAPR = attributeReader1.GetString(nameof (DisclosedAPR));
      this.financeCharge = attributeReader1.GetString(nameof (FinanceCharge));
      this.disclosedDailyInterest = attributeReader1.GetString(nameof (DisclosedDailyInterest));
      this.edsRequestGuid = attributeReader1.GetString(nameof (EDSRequestGuid));
      this.borrowerLoanLevelConsentMapForCC = this.convertStringToBool(attributeReader1.GetString(nameof (BorrowerLoanLevelConsentMapForCC)));
      this.coBorrowerLoanLevelConsentMapForCC = this.convertStringToBool(attributeReader1.GetString(nameof (CoBorrowerLoanLevelConsentMapForCC)));
      this.linkedGuid = attributeReader1.GetString(nameof (LinkedGuid));
      this.ucdCreationError = this.convertStringToBool(attributeReader1.GetString(nameof (UCDCreationError)));
      XmlNodeList xmlNodeList = e.SelectNodes("Attributes/Attribute");
      for (int i = 0; i < xmlNodeList.Count; ++i)
      {
        AttributeReader attributeReader2 = new AttributeReader((XmlElement) xmlNodeList[i]);
        this.attributeList.Add(attributeReader2.GetString("Name"), attributeReader2.GetString("Value"));
      }
      this.PopulateNBOOobject();
      this.MarkAsClean();
    }

    private void PopulateNBOOobject()
    {
      IEnumerable<KeyValuePair<string, string>> source = this.attributeList.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (p => p.Key.ToLower() == "nbocount"));
      if (source == null || !source.Any<KeyValuePair<string, string>>())
        return;
      IEnumerable<KeyValuePair<string, string>> keyValuePairs = this.attributeList.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (p => p.Key.StartsWith("NBOC") && p.Key.ToLower() != "nbocount"));
      Dictionary<string, Dictionary<string, string>> dictionary1 = new Dictionary<string, Dictionary<string, string>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (KeyValuePair<string, string> keyValuePair in keyValuePairs)
      {
        string empty = string.Empty;
        string key1;
        if (keyValuePair.Key.Contains("_"))
          key1 = keyValuePair.Key.Split('_')[1];
        else
          key1 = keyValuePair.Key.Substring(keyValuePair.Key.Length - 2, 2);
        string key2 = keyValuePair.Key.Substring(0, 6);
        Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
        dictionary2.Add(key1, keyValuePair.Value);
        if (dictionary1.ContainsKey(key2))
          dictionary1[key2].Add(key1, keyValuePair.Value);
        else
          dictionary1.Add(key2, dictionary2);
      }
      foreach (KeyValuePair<string, Dictionary<string, string>> keyValuePair in dictionary1)
      {
        Dictionary<string, string> dictionary3 = keyValuePair.Value;
        string FName = dictionary3.ContainsKey("01") ? dictionary3["01"] : "";
        string MName = dictionary3.ContainsKey("02") ? dictionary3["02"] : "";
        string LName = dictionary3.ContainsKey("03") ? dictionary3["03"] : "";
        string Suffix = dictionary3.ContainsKey("04") ? dictionary3["04"] : "";
        string Address = dictionary3.ContainsKey("05") ? dictionary3["05"] : "";
        string City = dictionary3.ContainsKey("06") ? dictionary3["06"] : "";
        string State = dictionary3.ContainsKey("07") ? dictionary3["07"] : "";
        string Zip = dictionary3.ContainsKey("08") ? dictionary3["08"] : "";
        string VestingType = dictionary3.ContainsKey("09") ? dictionary3["09"] : "";
        string HomePhone = dictionary3.ContainsKey("10") ? dictionary3["10"] : "";
        string Email = dictionary3.ContainsKey("11") ? dictionary3["11"] : "";
        bool IsNoThirdPartyEmail = dictionary3.ContainsKey("12") && keyValuePair.Value["12"] == "T";
        string BusiPhone = dictionary3.ContainsKey("13") ? dictionary3["13"] : "";
        string Cell = dictionary3.ContainsKey("14") ? dictionary3["14"] : "";
        string Fax = dictionary3.ContainsKey("15") ? dictionary3["15"] : "";
        DateTime DOB = dictionary3.ContainsKey("16") ? Utils.ParseDate((object) dictionary3["16"]) : DateTime.MinValue;
        string InstanceId = keyValuePair.Key.Substring(keyValuePair.Key.Length - 2, 2);
        string TRGuid = dictionary3.ContainsKey("99") ? dictionary3["99"] : "";
        this.nonBorrowerOwnerCollections.Add(keyValuePair.Key, (INonBorrowerOwnerItem) new NonBorrowerOwnerItem(FName, MName, LName, Suffix, Address, City, State, Zip, VestingType, HomePhone, Email, IsNoThirdPartyEmail, BusiPhone, Cell, Fax, DOB, TRGuid, InstanceId, this)
        {
          DisclosedMethod = (dictionary3.ContainsKey("DisclosedMethod") ? Convert.ToInt32(dictionary3["DisclosedMethod"]) : -1),
          DisclosedMethodOther = (dictionary3.ContainsKey("DisclosedMethodOther") ? dictionary3["DisclosedMethodOther"] : ""),
          PresumedReceivedDate = (dictionary3.ContainsKey("PresumedReceivedDate") ? Utils.ParseDate((object) dictionary3["PresumedReceivedDate"]) : DateTime.MinValue),
          lockedPresumedReceivedDate = (dictionary3.ContainsKey("lockedPresumedReceivedDate") ? Utils.ParseDate((object) dictionary3["lockedPresumedReceivedDate"]) : DateTime.MinValue),
          isPresumedDateLocked = (keyValuePair.Value["isPresumedDateLocked"] == "Y"),
          ActualReceivedDate = (dictionary3.ContainsKey("ActualReceivedDate") ? Utils.ParseDate((object) dictionary3["ActualReceivedDate"]) : DateTime.MinValue),
          isBorrowerTypeLocked = (dictionary3.ContainsKey("isBorrowerTypeLocked") && dictionary3["isBorrowerTypeLocked"] == "Y"),
          BorrowerType = (dictionary3.ContainsKey("BorrowerType") ? dictionary3["BorrowerType"] : ""),
          LockedBorrowerType = (dictionary3.ContainsKey("LockedBorrowerType") ? dictionary3["LockedBorrowerType"] : ""),
          TRGuid = (dictionary3.ContainsKey("99") ? dictionary3["99"] : ""),
          eDisclosureNBOAuthenticatedDate = (dictionary3.ContainsKey("eDisclosureNBOAuthenticatedDate") ? this.ConvertToLoanTimeZone(Utils.ParseDate((object) dictionary3["eDisclosureNBOAuthenticatedDate"])) : DateTime.MinValue),
          eDisclosureNBOAuthenticatedIP = (dictionary3.ContainsKey("eDisclosureNBOAuthenticatedIP") ? dictionary3["eDisclosureNBOAuthenticatedIP"] : ""),
          eDisclosureNBOViewMessageDate = (dictionary3.ContainsKey("eDisclosureNBOViewMessageDate") ? this.ConvertToLoanTimeZone(Utils.ParseDate((object) dictionary3["eDisclosureNBOViewMessageDate"])) : DateTime.MinValue),
          eDisclosureNBORejectConsentDate = (dictionary3.ContainsKey("eDisclosureNBORejectConsentDate") ? this.ConvertToLoanTimeZone(Utils.ParseDate((object) dictionary3["eDisclosureNBORejectConsentDate"])) : DateTime.MinValue),
          eDisclosureNBORejectConsentIP = (dictionary3.ContainsKey("eDisclosureNBORejectConsentIP") ? dictionary3["eDisclosureNBORejectConsentIP"] : ""),
          eDisclosureNBOSignedDate = (dictionary3.ContainsKey("eDisclosureNBOSignedDate") ? this.ConvertToLoanTimeZone(Utils.ParseDate((object) dictionary3["eDisclosureNBOSignedDate"])) : DateTime.MinValue),
          eDisclosureNBOeSignedIP = (dictionary3.ContainsKey("eDisclosureNBOeSignedIP") ? dictionary3["eDisclosureNBOeSignedIP"] : ""),
          eDisclosureNBOLoanLevelConsent = (dictionary3.ContainsKey("eDisclosureNBOLoanLevelConsent") ? dictionary3["eDisclosureNBOLoanLevelConsent"] : ""),
          eDisclosureNBOAcceptConsentDate = (dictionary3.ContainsKey("eDisclosureNBOAcceptConsentDate") ? this.ConvertToLoanTimeZone(Utils.ParseDate((object) dictionary3["eDisclosureNBOAcceptConsentDate"])) : DateTime.MinValue),
          eDisclosureNBOAcceptConsentIP = (dictionary3.ContainsKey("eDisclosureNBOAcceptConsentIP") ? dictionary3["eDisclosureNBOAcceptConsentIP"] : ""),
          eDisclosureNBODocumentViewedDate = (dictionary3.ContainsKey("eDisclosureNBODocumentViewedDate") ? this.ConvertToLoanTimeZone(Utils.ParseDate((object) dictionary3["eDisclosureNBODocumentViewedDate"])) : DateTime.MinValue),
          eDisclosureNBOeSignatures = (dictionary3.ContainsKey("eDisclosureNBOeSignatures") && dictionary3["eDisclosureNBOeSignatures"] == "Y")
        });
      }
    }

    public override DateTime DisclosureCreatedDTTM => base.DisclosureCreatedDTTM.ToUniversalTime();

    public Dictionary<string, INonBorrowerOwnerItem> NonBorrowerOwnerCollections
    {
      get => this.nonBorrowerOwnerCollections;
    }

    public int NumberOfNonBorrowingOwnerContact
    {
      get => Utils.ParseInt((object) this.GetAttribute("NBOcount"), 0);
    }

    public int NumberOfVestingParties => Utils.ParseInt((object) this.GetAttribute("TRcount"), 0);

    public int NumberOfGoodFaithChangeOfCircumstance
    {
      get => Utils.ParseInt((object) this.GetAttribute("XCOCcount"), 0);
    }

    public bool FeeLevelDisclosuresIndicator
    {
      get => Utils.ParseBoolean((object) this.GetAttribute("XCOCFeeLevelIndicator"));
    }

    public DisclosureTracking2015Log.TrackingLogStatus Status
    {
      get => DisclosureTracking2015Log.TrackingLogStatus.Active;
      set
      {
      }
    }

    private void addNBOandTRFields()
    {
      if (!this.DisclosedForCD && !this.DisclosedForLE)
        return;
      string id = this.loanData.CurrentBorrowerPair.Id;
      int additionalVestingParties = this.loanData.GetNumberOfAdditionalVestingParties();
      int num = 0;
      List<string> stringList = new List<string>();
      for (int index = 1; index <= additionalVestingParties; ++index)
      {
        string str1 = "TR" + index.ToString("00");
        if (string.Compare(this.loanData.GetField(str1 + "05"), id, true) == 0)
        {
          ++num;
          string str2 = "TR" + num.ToString("00");
          foreach (string disclosedVestingField in DisclosureTracking2015Log.DisclosedVestingFields)
          {
            string str3 = disclosedVestingField.Substring(disclosedVestingField.Length - 2, 2);
            string field = this.loanData.GetField(str1 + str3);
            this.AddAttribute(str2 + str3, field);
            if (str3 == "99")
              stringList.Add(field);
          }
        }
      }
      this.AddAttribute("TRcount", num.ToString());
    }

    private void addCOCFields()
    {
      this.AddAttribute("XCOCChangedCircumstances", this.DisclosedForCD ? this.loanData.GetField("CD1.X61") : this.loanData.GetField("3168"));
      this.AddAttribute("XCOCFeeLevelIndicator", this.loanData.GetField("4461"));
      if (!(this.loanData.GetField("4461") == "Y") || !this.DisclosedForCD && !this.DisclosedForLE)
        return;
      int changeOfCircumstance = this.loanData.GetNumberOfGoodFaithChangeOfCircumstance();
      Dictionary<string, GFFVAlertTriggerField> gffAlertFieldList = changeOfCircumstance > 0 ? this.getGFFAlertFieldList() : (Dictionary<string, GFFVAlertTriggerField>) null;
      int num = 0;
      for (int index = 1; index <= changeOfCircumstance; ++index)
      {
        string str1 = "XCOC" + index.ToString("00");
        string field = this.loanData.GetField(str1 + "01");
        if (!string.IsNullOrEmpty(field))
        {
          foreach (string discloseCocField in DisclosureTracking2015Log.DiscloseCOCFields)
          {
            string str2 = discloseCocField.Substring(discloseCocField.Length - 2, 2);
            this.AddAttribute(str1 + str2, this.loanData.GetField(str1 + str2));
          }
          string str3;
          string str4 = str3 = "";
          if (gffAlertFieldList.ContainsKey(field))
          {
            GFFVAlertTriggerField alertTriggerField = gffAlertFieldList[field];
            if (alertTriggerField != null)
            {
              str3 = alertTriggerField.Description;
              str4 = alertTriggerField.ItemizationValue;
            }
          }
          this.AddAttribute(str1 + "_Description", str3);
          this.AddAttribute(str1 + "_Amount", str4);
          ++num;
        }
      }
      this.AddAttribute("XCOCcount", num.ToString());
    }

    private void setFormFields()
    {
      if (this.containLE)
      {
        this.AddAttribute("ChangesLEReceivedDate", this.loanData.GetField("3165"));
        this.AddAttribute("RevisedLEDueDate", this.loanData.GetField("3167"));
        this.addCOCFields();
        this.addNBOandTRFields();
        this.changeInCircumstance = this.loanData.GetField("3169");
        this.changeInCircumstanceComments = this.loanData.GetField("LE1.X86");
        this.leReasonIsChangedCircumstanceSettlementCharges = this.loanData.GetField("LE1.X78") == "Y";
        this.leReasonIsChangedCircumstanceEligibility = this.loanData.GetField("LE1.X79") == "Y";
        this.leReasonIsRevisionsRequestedByConsumer = this.loanData.GetField("LE1.X80") == "Y";
        this.leReasonIsInterestRateDependentCharges = this.loanData.GetField("LE1.X81") == "Y";
        this.leReasonIsExpiration = this.loanData.GetField("LE1.X82") == "Y";
        this.leReasonIsDelayedSettlementOnConstructionLoans = this.loanData.GetField("LE1.X83") == "Y";
        this.leReasonIsOther = this.loanData.GetField("LE1.X84") == "Y";
        this.leReasonOther = this.loanData.GetField("LE1.X85");
        this.loanData.SetField("3169", "");
        this.loanData.SetField("LE1.X90", "");
        this.loanData.SetField("LE1.X86", "");
        this.loanData.SetField("LE1.X78", "N");
        this.loanData.SetField("LE1.X79", "N");
        this.loanData.SetField("LE1.X80", "N");
        this.loanData.SetField("LE1.X81", "N");
        this.loanData.SetField("LE1.X82", "N");
        this.loanData.SetField("LE1.X83", "N");
        this.loanData.SetField("LE1.X84", "N");
        this.loanData.SetField("LE1.X85", "");
        this.loanData.SetField("3165", "//");
        this.loanData.SetField("3167", "//");
        this.loanData.SetField("3168", "N");
      }
      if (this.containCD)
      {
        this.AddAttribute("ChangesCDReceivedDate", this.loanData.GetField("CD1.X62"));
        this.AddAttribute("RevisedCDDueDate", this.loanData.GetField("CD1.X63"));
        this.addCOCFields();
        this.addNBOandTRFields();
        this.changeInCircumstance = this.loanData.GetField("CD1.X64");
        this.changeInCircumstanceComments = this.loanData.GetField("CD1.X65");
        this.cdReasonIsChangeInAPR = this.loanData.GetField("CD1.X52") == "Y";
        this.cdReasonIsChangeInLoanProduct = this.loanData.GetField("CD1.X53") == "Y";
        this.cdReasonIsPrepaymentPenaltyAdded = this.loanData.GetField("CD1.X54") == "Y";
        this.cdReasonIsChangeInSettlementCharges = this.loanData.GetField("CD1.X55") == "Y";
        this.cdReasonIs24HourAdvancePreview = this.loanData.GetField("CD1.X56") == "Y";
        this.cdReasonIsToleranceCure = this.loanData.GetField("CD1.X57") == "Y";
        this.cdReasonIsClericalErrorCorrection = this.loanData.GetField("CD1.X58") == "Y";
        this.cdReasonIsOther = this.loanData.GetField("CD1.X59") == "Y";
        this.cdReasonIsChangedCircumstanceEligibility = this.loanData.GetField("CD1.X68") == "Y";
        this.cdReasonIsRevisionsRequestedByConsumer = this.loanData.GetField("CD1.X66") == "Y";
        this.cdReasonIsInterestRateDependentCharges = this.loanData.GetField("CD1.X67") == "Y";
        this.cdReasonOther = this.loanData.GetField("CD1.X60");
        this.loanData.SetField("CD1.X64", "");
        this.loanData.SetField("CD1.X70", "");
        this.loanData.SetField("CD1.X65", "");
        this.loanData.SetField("CD1.X52", "N");
        this.loanData.SetField("CD1.X53", "N");
        this.loanData.SetField("CD1.X54", "N");
        this.loanData.SetField("CD1.X55", "N");
        this.loanData.SetField("CD1.X56", "N");
        this.loanData.SetField("CD1.X57", "N");
        this.loanData.SetField("CD1.X58", "N");
        this.loanData.SetField("CD1.X59", "N");
        this.loanData.SetField("CD1.X66", "N");
        this.loanData.SetField("CD1.X67", "N");
        this.loanData.SetField("CD1.X68", "N");
        this.loanData.SetField("CD1.X60", "");
        this.loanData.SetField("CD1.X62", "//");
        this.loanData.SetField("CD1.X63", "//");
        this.loanData.SetField("CD1.X61", "N");
      }
      if (!this.containCD)
        return;
      this.loanData.SetField("3171", "//");
      this.loanData.SetField("3172", "");
      this.loanData.SetField("3173", "");
    }

    private bool convertStringToBool(string input) => input == "Y" || input == "True";

    private string extractLoanData(LoanData loanData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string disclosureSnapshotField in DisclosureTracking2015Log.disclosureSnapshotFields)
      {
        string simpleField = loanData.GetSimpleField(disclosureSnapshotField);
        if (!(simpleField.Trim() == ""))
        {
          this.AddDisclosedLoanInfo(disclosureSnapshotField, simpleField);
          stringBuilder.Append(this.AddDisclosedFieldString(disclosureSnapshotField, simpleField));
        }
      }
      if (this.DisclosedForCD)
      {
        foreach (string disclosedCdField in DisclosureTracking2015Log.DisclosedCDFields)
        {
          if (!disclosedCdField.StartsWith("CD3.XH"))
          {
            string simpleField = loanData.GetSimpleField(disclosedCdField);
            if (!(simpleField.Trim() == "") || !(disclosedCdField != "L726"))
            {
              this.AddDisclosedLoanInfo(disclosedCdField, simpleField);
              stringBuilder.Append(this.AddDisclosedFieldString(disclosedCdField, simpleField));
            }
          }
        }
      }
      if (this.DisclosedForLE)
      {
        foreach (string disclosedLeField in DisclosureTracking2015Log.DisclosedLEFields)
        {
          if (!disclosedLeField.StartsWith("CD3.XH"))
          {
            string simpleField = loanData.GetSimpleField(disclosedLeField);
            if (!(simpleField.Trim() == "") || !(disclosedLeField != "L726"))
            {
              this.AddDisclosedLoanInfo(disclosedLeField, simpleField);
              stringBuilder.Append(this.AddDisclosedFieldString(disclosedLeField, simpleField));
            }
          }
        }
      }
      if (this.DisclosedForCD || this.DisclosedForLE)
      {
        foreach (string itemizationField in DisclosureTracking2015Log.DisclosedItemizationFields)
        {
          string simpleField = loanData.GetSimpleField(itemizationField);
          if (!(simpleField.Trim() == "") || !(itemizationField != "FV.X396") || !(itemizationField != "FV.X397"))
          {
            this.AddDisclosedLoanInfo(itemizationField, simpleField);
            stringBuilder.Append(this.AddDisclosedFieldString(itemizationField, simpleField));
          }
        }
        foreach (string[] itemizationField in this.itemizationFields)
        {
          for (int index = 2; index <= itemizationField.Length; ++index)
          {
            string simpleField = loanData.GetSimpleField(itemizationField[index]);
            if (!(simpleField.Trim() == ""))
            {
              this.AddDisclosedLoanInfo(itemizationField[index], simpleField);
              stringBuilder.Append(this.AddDisclosedFieldString(itemizationField[index], simpleField));
            }
          }
        }
      }
      if (this.DisclosedForSafeHarbor)
      {
        foreach (string disclosedSafeHarborField in DisclosureTrackingBase.GetDisclosedSafeHarborFields())
        {
          if (!DisclosureTracking2015Log.disclosureSnapshotFields.Contains(disclosedSafeHarborField))
          {
            this.AddDisclosedLoanInfo(disclosedSafeHarborField, loanData.GetSimpleField(disclosedSafeHarborField));
            stringBuilder.Append(this.AddDisclosedFieldString(disclosedSafeHarborField, loanData.GetSimpleField(disclosedSafeHarborField)));
            if (loanData.IsLocked(disclosedSafeHarborField) && !DisclosureTracking2015Log.disclosureSnapshotFields.Contains(disclosedSafeHarborField + "_LOCKED"))
            {
              this.AddDisclosedLoanInfo(disclosedSafeHarborField + "_LOCKED", loanData.GetFieldFromCal(disclosedSafeHarborField));
              stringBuilder.Append(this.AddDisclosedFieldString(disclosedSafeHarborField + "_LOCKED", loanData.GetFieldFromCal(disclosedSafeHarborField)));
            }
          }
        }
      }
      if (this.ProviderListSent || this.ProviderListNoFeeSent)
      {
        int serviceProviders = this.loanData.GetNumberOfSettlementServiceProviders();
        this.AddDisclosedLoanInfo("SSPLcount", serviceProviders.ToString());
        stringBuilder.Append(this.AddDisclosedFieldString("SSPLcount", serviceProviders.ToString()));
        for (int index = 1; index <= serviceProviders; ++index)
        {
          string str1 = index.ToString("00");
          foreach (string disclosedSsplField in DisclosureTracking2015Log.DisclosedSSPLFields)
          {
            string str2 = disclosedSsplField.Substring(disclosedSsplField.Length - 2, 2);
            this.AddDisclosedLoanInfo("SP" + str1 + str2, loanData.GetField("SP" + str1 + str2));
            stringBuilder.Append(this.AddDisclosedFieldString("SP" + str1 + str2, loanData.GetField("SP" + str1 + str2)));
          }
        }
      }
      this.AddDisclosedLoanInfo("edisclosureDisclosedMessage", "");
      stringBuilder.Append(this.AddDisclosedFieldString("edisclosureDisclosedMessage", ""));
      return stringBuilder.ToString();
    }

    private Dictionary<string, GFFVAlertTriggerField> getGFFAlertFieldList()
    {
      Dictionary<string, GFFVAlertTriggerField> gffAlertFieldList = new Dictionary<string, GFFVAlertTriggerField>();
      Hashtable varianceAlertDetails = this.loanData.Calculator.GetGFFVarianceAlertDetails();
      for (int index = 1; index <= 3; ++index)
      {
        string str;
        switch (index)
        {
          case 1:
            str = "Cannot Decrease";
            break;
          case 2:
            str = "Cannot Increase";
            break;
          default:
            str = "10% Variance";
            break;
        }
        string key = str;
        if (varianceAlertDetails.ContainsKey((object) key))
        {
          ArrayList arrayList = (ArrayList) varianceAlertDetails[(object) key];
          if (arrayList != null && arrayList.Count != 0)
          {
            foreach (GFFVAlertTriggerField alertTriggerField in arrayList)
            {
              if (!gffAlertFieldList.ContainsKey(alertTriggerField.FieldId))
                gffAlertFieldList.Add(alertTriggerField.FieldId, alertTriggerField);
            }
          }
        }
      }
      return gffAlertFieldList;
    }

    public void AppendFieldValues(Dictionary<string, string> fields)
    {
      StringBuilder stringBuilder = new StringBuilder(this.udt);
      foreach (KeyValuePair<string, string> field in fields)
      {
        this.AddDisclosedLoanInfo(field.Key, field.Value);
        stringBuilder.Append(this.AddDisclosedFieldString(field.Key, field.Value));
      }
      this.udt = stringBuilder.ToString();
    }

    public void SetItemizationFields(List<string[]> fields, LoanData loanData)
    {
      StringBuilder stringBuilder = new StringBuilder();
      this.itemizationFields = fields;
      foreach (string[] itemizationField in this.itemizationFields)
      {
        for (int index = 2; index < itemizationField.Length; ++index)
        {
          string simpleField = loanData.GetSimpleField(itemizationField[index]);
          if (!(simpleField.Trim() == ""))
          {
            if (!this.IsFieldExist(itemizationField[index]))
              stringBuilder.Append(this.AddDisclosedFieldString(itemizationField[index], simpleField));
            this.AddDisclosedLoanInfo(itemizationField[index], simpleField);
          }
        }
      }
      if ((this.DisclosedForLE || this.DisclosedForCD) && this.loanData.Calculator != null)
      {
        string fieldValue1 = this.loanData.Calculator.SetTotalLenderCredit((IDisclosureTracking2015Log) this);
        string fieldValue2 = this.loanData.Calculator.SetCannotIncrease((IDisclosureTracking2015Log) this);
        this.AddDisclosedLoanInfo("TotalLenderCredit", fieldValue1);
        stringBuilder.Append(this.AddDisclosedFieldString("TotalLenderCredit", fieldValue1));
        this.AddDisclosedLoanInfo("CannotIncrease", fieldValue2);
        stringBuilder.Append(this.AddDisclosedFieldString("CannotIncrease", fieldValue2));
      }
      this.udt += stringBuilder.ToString();
    }

    internal override void OnRecordAdd()
    {
      if (this.Log.Loan.Calculator == null)
        return;
      this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
    }

    public string GetCureLogComment()
    {
      string cureLogComment = "";
      if (this.containLE)
        cureLogComment = (this.leReasonIsChangedCircumstanceSettlementCharges ? "Changed Circumstance - Settlement Charges," : "") + (this.leReasonIsChangedCircumstanceEligibility ? "Changed Circumstance - Eligibility," : "") + (this.leReasonIsRevisionsRequestedByConsumer ? "Revisions requested by the Consumer," : "") + (this.leReasonIsInterestRateDependentCharges ? "Interest Rate dependent charges (Rate Lock)," : "") + (this.leReasonIsExpiration ? "Expiration (Intent to Proceed received after 10 business days)," : "") + (this.leReasonIsDelayedSettlementOnConstructionLoans ? "Delayed Settlement on Construction Loans," : "") + (this.leReasonIsOther ? "," + this.leReasonOther : "");
      if (this.containCD)
        cureLogComment = (this.cdReasonIsChangeInAPR ? "Change in APR," : "") + (this.cdReasonIsChangeInLoanProduct ? "Change in Loan Product," : "") + (this.cdReasonIsPrepaymentPenaltyAdded ? "Prepayment Penalty Added," : "") + (this.cdReasonIsChangeInSettlementCharges ? "Change in Settlement Charges," : "") + (this.cdReasonIsChangedCircumstanceEligibility ? "Changed Circumstance - Eligibility," : "") + (this.cdReasonIsRevisionsRequestedByConsumer ? "Revisions requested by the Consumer," : "") + (this.cdReasonIsInterestRateDependentCharges ? "Interest Rate dependent charges (Rate Lock)," : "") + (this.cdReasonIs24HourAdvancePreview ? "24-hour Advanced Preview," : "") + (this.cdReasonIsToleranceCure ? "Tolerance Cure," : "") + (this.cdReasonIsClericalErrorCorrection ? "Clerical Error Correction," : "") + (this.cdReasonIsOther ? "," + this.cdReasonOther : "");
      if (cureLogComment != "")
      {
        if (this.containLE)
          cureLogComment = "LE issued with Change of Circumstance containing the following reasons: " + cureLogComment.TrimEnd(',');
        else if (this.containCD)
          cureLogComment = "CD issued with Change of Circumstance containing the following reasons:" + cureLogComment.TrimEnd(',');
      }
      else if (this.containLE)
        cureLogComment = "LE issued with Change of Circumstance";
      else if (this.containCD)
        cureLogComment = "CD issued with Change of Circumstance";
      return cureLogComment;
    }

    public void CreateCureLog(
      double appliedCureAmount,
      string cureLogComment,
      Hashtable triggerFields,
      string resolvedById,
      string resolvedBy,
      bool newLog)
    {
      if (this.DisclosedForLE && this.DisclosureType != DisclosureTracking2015Log.DisclosureTypeEnum.Revised || appliedCureAmount == 0.0 || RegulationAlerts.GetGoodFaithFeeVarianceViolationAlert(this.loanData) != null)
        return;
      string fieldValue = DateTime.Now.ToString("MM/dd/yyyy");
      GoodFaithFeeVarianceCureLog rec = new GoodFaithFeeVarianceCureLog(Utils.ParseDate((object) fieldValue), resolvedById, resolvedBy, appliedCureAmount.ToString(), this.loanData.GetField("FV.X348"), cureLogComment, "Variance Cured", DateTime.Now);
      foreach (DictionaryEntry triggerField1 in triggerFields)
      {
        ArrayList arrayList = (ArrayList) triggerField1.Value;
        for (int index = 0; index < arrayList.Count; ++index)
        {
          GFFVAlertTriggerField triggerField2 = (GFFVAlertTriggerField) arrayList[index];
          rec.TriggerFieldList.Add(triggerField2);
        }
      }
      if (newLog)
      {
        DisclosedLoanItem disclosedLoanItem1 = new DisclosedLoanItem("3171", fieldValue);
        if (this.loanDataList.Contains(disclosedLoanItem1))
        {
          this.loanDataList.Remove(disclosedLoanItem1);
          this.loanDataList.Add(disclosedLoanItem1);
        }
        DisclosedLoanItem disclosedLoanItem2 = new DisclosedLoanItem("3172", cureLogComment);
        if (this.loanDataList.Contains(disclosedLoanItem2))
        {
          this.loanDataList.Remove(disclosedLoanItem2);
          this.loanDataList.Add(disclosedLoanItem2);
        }
        DisclosedLoanItem disclosedLoanItem3 = new DisclosedLoanItem("3173", resolvedById);
        if (this.loanDataList.Contains(disclosedLoanItem3))
        {
          this.loanDataList.Remove(disclosedLoanItem3);
          this.loanDataList.Add(disclosedLoanItem3);
        }
        DisclosedLoanItem disclosedLoanItem4 = new DisclosedLoanItem("FV.X366", appliedCureAmount.ToString());
        if (this.loanDataList.Contains(disclosedLoanItem4))
        {
          this.loanDataList.Remove(disclosedLoanItem4);
          this.loanDataList.Add(disclosedLoanItem4);
        }
      }
      this.loanData.GetLogList().AddRecord((LogRecordBase) rec);
    }

    public bool LEDisclosedByBroker
    {
      get => this.leDisclosedByBroker;
      set
      {
        this.leDisclosedByBroker = value;
        this.MarkAsDirty();
      }
    }

    public DisclosureTracking2015Log.DisclosureTypeEnum DisclosureType
    {
      get => this.disclosureType;
      set
      {
        this.disclosureType = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public void AdjustDisclosedType(DisclosureTracking2015Log.DisclosureTypeEnum type)
    {
      this.disclosureType = type;
      this.MarkAsDirty();
    }

    public string DisclosureTypeName
    {
      get
      {
        switch (this.disclosureType)
        {
          case DisclosureTracking2015Log.DisclosureTypeEnum.Initial:
            return "Initial";
          case DisclosureTracking2015Log.DisclosureTypeEnum.Revised:
            return "Revised";
          case DisclosureTracking2015Log.DisclosureTypeEnum.Final:
            return "Final";
          case DisclosureTracking2015Log.DisclosureTypeEnum.PostConsummation:
            return "Post Consummation";
          default:
            return "Initial";
        }
      }
    }

    public DateTime LockedDisclosedDateField
    {
      get => this.lockedDisclosedDateField;
      set
      {
        this.lockedDisclosedDateField = Utils.TruncateDate(value);
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public DateTime OriginalDisclosedDate
    {
      get => this.ConvertToLoanTimeZone(this.date);
      set
      {
        this.date = this.ConvertToUtc(value);
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public override DateTime DisclosedDate
    {
      get => this.IsLocked ? this.LockedDisclosedDateField : this.OriginalDisclosedDate.Date;
      set
      {
        if (!this.IsLocked)
          return;
        this.LockedDisclosedDateField = value;
      }
    }

    public DateTime DisclosedDateTime
    {
      get => !this.IsLocked ? this.OriginalDisclosedDate : this.LockedDisclosedDateField;
    }

    public bool ProviderListSent => this.providerListSent;

    public bool ProviderListNoFeeSent => this.providerListNoFeeSent;

    public override string DisclosedByFullName
    {
      get => this.disclosedByFullName;
      set
      {
        this.disclosedByFullName = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public string DisclosedByReportFullName
    {
      get
      {
        return this.IsDisclosedByLocked ? this.lockedDisclosedByField : this.disclosedByFullName + "(" + this.DisclosedBy + ")";
      }
    }

    public string LockedDisclosedByField
    {
      get => this.lockedDisclosedByField;
      set
      {
        this.lockedDisclosedByField = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public DisclosureTrackingBase.DisclosedMethod DisclosureMethod
    {
      get => this.method;
      set
      {
        this.method = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public string DisclosedMethodOther
    {
      get => this.sentMethodOther;
      set => this.sentMethodOther = value;
    }

    public bool UseForUCDExport
    {
      get => this.useForUCDExport;
      set => this.useForUCDExport = value;
    }

    public override string DisclosedMethodName
    {
      get => DisclosureTracking2015Log.GetDisclosedMethodName(this.method);
    }

    public DisclosureTracking2015Log.DisclosureTypeEnum DisclosureType2015 => this.DisclosureType;

    public DateTime DisclosedDate2015 => this.DisclosedDateTime;

    public DisclosureTrackingBase.DisclosedMethod BorrowerDisclosedMethod2015
    {
      get => this.BorrowerDisclosedMethod;
    }

    public DisclosureTrackingBase.DisclosedMethod CoBorrowerDisclosedMethod2015
    {
      get => this.CoBorrowerDisclosedMethod;
    }

    public DateTime BorrowerActualReceivedDate2015 => this.BorrowerActualReceivedDate;

    public DateTime CoBorrowerActualReceivedDate2015 => this.CoBorrowerActualReceivedDate;

    public string DisclosedMethodNameByType(DisclosureTrackingBase.DisclosedMethod type)
    {
      switch (type)
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
        case DisclosureTrackingBase.DisclosedMethod.Email:
          return "Email";
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          return "Phone";
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          return "eFolder eDisclosures";
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          return "Closing Docs Order";
        default:
          return "In Person";
      }
    }

    public override string GetDisclosedField(string fieldId)
    {
      return this.GetDisclosedField(fieldId, true);
    }

    public string GetDisclosedField(string fieldId, bool retrieveFromOtherLog)
    {
      if ((fieldId.StartsWith("XCOC") || fieldId.StartsWith("NBOC") || fieldId.StartsWith("NBOC")) && fieldId.Length >= 8 || fieldId.StartsWith("TR") && fieldId.Length >= 6)
        return this.GetAttribute(fieldId) ?? "";
      if (!this.IsLoanDataListExist && this.loanData.SnapshotProvider != null)
      {
        if (DisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots.Count > 20)
          DisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots.Clear();
        string key = this.loanData.GUID + "-" + this.Guid;
        Dictionary<string, string> dataFields;
        DisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots.TryGetValue(key, out dataFields);
        if (dataFields == null)
        {
          bool ucdExists = !this.ucdCreationError && (this.DisclosedForCD || this.DisclosedForLE);
          dataFields = DisclosureTracking2015Log.cachedDisclosureTrackingLoanSnapShots[key] = this.loanData.SnapshotProvider.GetLoanSnapshot(LogSnapshotType.DisclosureTracking, new System.Guid(this.Guid), ucdExists);
        }
        this.PopulateLoanDataList(dataFields);
      }
      string str = (this.loanDataFromOtherLogs == null || !this.loanDataFromOtherLogs.ContainsKey((object) fieldId) ? (!(fieldId == "FV.X396") && !(fieldId == "FV.X397") || this.loanDataUniqueList.ContainsKey((object) "FV.X396") || this.loanDataUniqueList.ContainsKey((object) "FV.X397") ? (string) this.loanDataUniqueList[(object) fieldId] : (fieldId == "FV.X396" ? (string) this.loanDataUniqueList[(object) "FV.X366"] : "")) : (string) this.loanDataFromOtherLogs[(object) fieldId]) ?? "";
      if (fieldId == "1868" && str == "" && this.loanData != null)
      {
        str = this.loanData.GetField("1868");
        if (str == "")
          str = this.FormatName(this.loanData.GetField("4000"), this.loanData.GetField("4001"), this.loanData.GetField("4002"), this.loanData.GetField("4003"));
      }
      if (fieldId == "1873" && str == "" && this.loanData != null && ((string) this.loanDataUniqueList[(object) "1868"] ?? "") == "")
      {
        str = this.loanData.GetField("1873");
        if (str == "")
          str = this.FormatName(this.loanData.GetField("4004"), this.loanData.GetField("4005"), this.loanData.GetField("4006"), this.loanData.GetField("4007"));
      }
      if (this.loanData != null)
      {
        FieldFormat format = fieldId == "SSPLcount" || fieldId == "TRcount" || fieldId == "NBOcount" ? FieldFormat.INTEGER : this.loanData.GetFormat(fieldId);
        if ((!(fieldId == "703") || !(this.loanData.GetField(this.loanData.foreignAddressIndictorLookupTable["703"]) == "Y")) && (!(fieldId == "1249") || !(this.loanData.GetField(this.loanData.foreignAddressIndictorLookupTable["1249"]) == "Y")) && (!(fieldId == "1419") || !(this.loanData.GetField(this.loanData.foreignAddressIndictorLookupTable["1419"]) == "Y")) && (!(fieldId == "1418") || !(this.loanData.GetField(this.loanData.foreignAddressIndictorLookupTable["1418"]) == "Y")))
          str = Utils.ApplyFieldFormatting(str, format);
      }
      return str ?? "";
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
        if (!this.IsAttachedToLog || this.Log.Loan.Calculator == null)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      }
    }

    public DateTime ChangesReceivedDate
    {
      get => this.changesReceivedDate;
      set => this.changesReceivedDate = value;
    }

    public DateTime RevisedDueDate
    {
      get => this.revisedDueDate;
      set => this.revisedDueDate = value;
    }

    public DateTime SetReceivedDateFromCalc
    {
      get => this.receivedDate;
      set
      {
        this.receivedDate = value;
        this.MarkAsDirty();
        if (!this.IsAttachedToLog || this.Log.Loan.Calculator == null)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      }
    }

    public override bool IsDisclosedReceivedDateLocked
    {
      get => this.isDisclosedReceivedDateLocked;
      set
      {
        this.isDisclosedReceivedDateLocked = value;
        this.lockedDisclosedReceivedDate = this.ReceivedDate;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public string DisclosedAPR
    {
      get => this.IsDisclosedAPRLocked ? this.lockedDisclosedAPRField : this.disclosedAPR;
      set
      {
        if (this.IsDisclosedAPRLocked)
          this.lockedDisclosedAPRField = value;
        if (!this.IsAttachedToLog || this.Log.Loan.Calculator == null)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      }
    }

    public string DisclosedAPRValue => this.disclosedAPR;

    public string FinanceCharge
    {
      get
      {
        return this.IsDisclosedFinanceChargeLocked ? this.lockedDisclosedFinanceChargeField : this.financeCharge;
      }
      set
      {
        if (this.IsDisclosedFinanceChargeLocked)
          this.lockedDisclosedFinanceChargeField = value;
        if (!this.IsAttachedToLog || this.Log.Loan.Calculator == null)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      }
    }

    public string FinanceChargeValue => this.financeCharge;

    public string DisclosedDailyInterest
    {
      get
      {
        return this.IsDisclosedDailyInterestLocked ? this.lockedDisclosedDailyInterestField : this.disclosedDailyInterest;
      }
      set
      {
        if (this.IsDisclosedDailyInterestLocked)
          this.lockedDisclosedDailyInterestField = value;
        if (!this.IsAttachedToLog || this.Log.Loan.Calculator == null)
          return;
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      }
    }

    public string DisclosedDailyInterestValue => this.disclosedDailyInterest;

    public override void populateAttributes(LoanData loandata)
    {
      base.populateAttributes(loandata);
      this.borrowerName = loandata.GetField("1868");
      if (loandata != null && loandata.Calculator != null)
      {
        if (this.borrowerName.Trim() == "")
        {
          loandata.Calculator.FormCalculation("UPDATEBORROWERVESTINGNAME");
          this.borrowerName = loandata.GetField("1868");
        }
        this.coBorrowerName = loandata.GetField("1873");
        if (this.coBorrowerName.Trim() == "")
        {
          loandata.Calculator.FormCalculation("UPDATECOBORROWERVESTINGNAME");
          this.coBorrowerName = loandata.GetField("1873");
        }
      }
      this.disclosedDailyInterest = loandata.GetField("334");
      this.disclosedAPR = loandata.GetField("799");
      this.financeCharge = loandata.GetField("1206");
      this.applicationDate = Utils.ParseDate((object) loandata.GetField("3142"), DateTime.MinValue);
      this.borrowerLoanLevelConsentMapForCC = true;
      this.coBorrowerLoanLevelConsentMapForCC = true;
    }

    private void createNBO()
    {
      if (!this.DisclosedForCD && !this.DisclosedForLE)
        return;
      int num1 = 0;
      int num2 = Utils.ParseInt((object) this.AttributeList["TRcount"]);
      string str1 = (string) null;
      List<string> stringList = new List<string>();
      for (int index = 1; index <= num2; ++index)
      {
        string str2 = "TR" + index.ToString("00");
        stringList.Add(this.AttributeList[str2 + "99"]);
      }
      int num3 = 0;
      if (stringList != null && stringList.Count > 0)
        num3 = this.loanData.GetNumberOfNonBorrowingOwnerContact();
      for (int index = 1; index <= num3; ++index)
      {
        string key = "NBOC" + index.ToString("00");
        string field = this.loanData.GetField(key + "98");
        if (stringList.Contains(field))
        {
          ++num1;
          str1 = "NBOC" + num1.ToString("00");
          this.nonBorrowerOwnerCollections.Add(key, (INonBorrowerOwnerItem) new NonBorrowerOwnerItem(this.loanData.GetField(key + "01"), this.loanData.GetField(key + "02"), this.loanData.GetField(key + "03"), this.loanData.GetField(key + "04"), this.loanData.GetField(key + "05"), this.loanData.GetField(key + "06"), this.loanData.GetField(key + "07"), this.loanData.GetField(key + "08"), this.loanData.GetField(key + "09"), this.loanData.GetField(key + "10"), this.loanData.GetField(key + "11"), Utils.ParseBoolean((object) this.loanData.GetField(key + "12")), this.loanData.GetField(key + "13"), this.loanData.GetField(key + "14"), this.loanData.GetField(key + "15"), Utils.ParseDate((object) this.loanData.GetField(key + "16")), this.loanData.GetField(key + "98"), index.ToString("00"), this)
          {
            BorrowerType = this.loanData.GetField(key + "09")
          });
        }
      }
    }

    public void ResetLoanDataFromOtherLogs() => this.loanDataFromOtherLogs.Clear();

    public void SetLoanDataFromOtherLogs(string id, string val)
    {
      if (this.loanDataFromOtherLogs.ContainsKey((object) id))
        this.loanDataFromOtherLogs[(object) id] = (object) val;
      else
        this.loanDataFromOtherLogs.Add((object) id, (object) val);
    }

    public string ReadLoanDataFromOtherLogs(string id)
    {
      return !this.loanDataFromOtherLogs.ContainsKey((object) id) ? (string) null : (string) this.loanDataFromOtherLogs[(object) id];
    }

    public bool IsDisclosed
    {
      get => this.isDisclosed;
      set
      {
        this.isDisclosed = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
        {
          if (this.DisclosedForLE || this.DisclosedForCD)
            this.Log.Loan.Calculator.UpdateDisclosureTypeForTimeline((IDisclosureTracking2015Log) this);
          this.Log.Loan.Calculator.CalculateLastDisclosedCDorLE((IDisclosureTracking2015Log) this);
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
          this.Log.Loan.Calculator.UpdateLogs();
        }
        this.MarkAsDirty();
      }
    }

    public bool IsLoanDataListExist => this.loanDataList.Count != 0;

    public void RemoveShapsnot()
    {
      this.loanDataList.Clear();
      this.loanDataUniqueList.Clear();
    }

    public bool DisclosedForCD => this.containCD;

    public bool DisclosedForLE => this.containLE;

    public override bool IsLocked
    {
      get => this.isLocked;
      set
      {
        this.isLocked = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public bool IsDisclosedAPRLocked
    {
      get => this.isDisclosedAPRLocked;
      set
      {
        this.isDisclosedAPRLocked = value;
        this.lockedDisclosedAPRField = this.DisclosedAPR;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public bool IsDisclosedFinanceChargeLocked
    {
      get => this.isDisclosedFinanceChargeLocked;
      set
      {
        this.isDisclosedFinanceChargeLocked = value;
        this.lockedDisclosedFinanceChargeField = this.FinanceCharge;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public bool IsDisclosedDailyInterestLocked
    {
      get => this.isDisclosedDailyInterestLocked;
      set
      {
        this.isDisclosedDailyInterestLocked = value;
        this.lockedDisclosedDailyInterestField = this.DisclosedDailyInterest;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public bool LEReasonAnyChecked
    {
      get
      {
        return this.LEReasonIsInterestRateDependentCharges || this.LEReasonIsChangedCircumstanceSettlementCharges || this.LEReasonIsChangedCircumstanceEligibility || this.LEReasonIsRevisionsRequestedByConsumer || this.LEReasonIsExpiration || this.LEReasonIsDelayedSettlementOnConstructionLoans;
      }
    }

    public bool LEReasonIsChangedCircumstanceSettlementCharges
    {
      get => this.leReasonIsChangedCircumstanceSettlementCharges;
      set => this.leReasonIsChangedCircumstanceSettlementCharges = value;
    }

    public bool LEReasonIsChangedCircumstanceEligibility
    {
      get => this.leReasonIsChangedCircumstanceEligibility;
      set => this.leReasonIsChangedCircumstanceEligibility = value;
    }

    public bool LEReasonIsRevisionsRequestedByConsumer
    {
      get => this.leReasonIsRevisionsRequestedByConsumer;
      set => this.leReasonIsRevisionsRequestedByConsumer = value;
    }

    public bool LEReasonIsInterestRateDependentCharges
    {
      get => this.leReasonIsInterestRateDependentCharges;
      set => this.leReasonIsInterestRateDependentCharges = value;
    }

    public bool LEReasonIsExpiration
    {
      get => this.leReasonIsExpiration;
      set => this.leReasonIsExpiration = value;
    }

    public bool LEReasonIsDelayedSettlementOnConstructionLoans
    {
      get => this.leReasonIsDelayedSettlementOnConstructionLoans;
      set => this.leReasonIsDelayedSettlementOnConstructionLoans = value;
    }

    public bool LEReasonIsOther
    {
      get => this.leReasonIsOther;
      set => this.leReasonIsOther = value;
    }

    public bool CDReasonIsChangeInAPR
    {
      get => this.cdReasonIsChangeInAPR;
      set => this.cdReasonIsChangeInAPR = value;
    }

    public bool CDReasonIsChangeInLoanProduct
    {
      get => this.cdReasonIsChangeInLoanProduct;
      set => this.cdReasonIsChangeInLoanProduct = value;
    }

    public bool CDReasonIsPrepaymentPenaltyAdded
    {
      get => this.cdReasonIsPrepaymentPenaltyAdded;
      set => this.cdReasonIsPrepaymentPenaltyAdded = value;
    }

    public bool CDReasonIsChangeInSettlementCharges
    {
      get => this.cdReasonIsChangeInSettlementCharges;
      set => this.cdReasonIsChangeInSettlementCharges = value;
    }

    public bool CDReasonIs24HourAdvancePreview
    {
      get => this.cdReasonIs24HourAdvancePreview;
      set => this.cdReasonIs24HourAdvancePreview = value;
    }

    public bool CDReasonIsToleranceCure
    {
      get => this.cdReasonIsToleranceCure;
      set => this.cdReasonIsToleranceCure = value;
    }

    public bool CDReasonIsClericalErrorCorrection
    {
      get => this.cdReasonIsClericalErrorCorrection;
      set => this.cdReasonIsClericalErrorCorrection = value;
    }

    public bool CDReasonIsChangedCircumstanceEligibility
    {
      get => this.cdReasonIsChangedCircumstanceEligibility;
      set => this.cdReasonIsChangedCircumstanceEligibility = value;
    }

    public bool CDReasonIsRevisionsRequestedByConsumer
    {
      get => this.cdReasonIsRevisionsRequestedByConsumer;
      set => this.cdReasonIsRevisionsRequestedByConsumer = value;
    }

    public bool CDReasonIsInterestRateDependentCharges
    {
      get => this.cdReasonIsInterestRateDependentCharges;
      set => this.cdReasonIsInterestRateDependentCharges = value;
    }

    public bool CDReasonIsOther
    {
      get => this.cdReasonIsOther;
      set => this.cdReasonIsOther = value;
    }

    public string LEReasonOther
    {
      get => this.leReasonOther;
      set => this.leReasonOther = value;
    }

    public string CDReasonOther
    {
      get => this.cdReasonOther;
      set => this.cdReasonOther = value;
    }

    public bool IsBorrowerTypeLocked
    {
      get => this.GetAttribute("isborrowerTypeLocked") == "True";
      set
      {
        if (!this.attributeList.ContainsKey("isborrowerTypeLocked"))
          this.attributeList.Add("isborrowerTypeLocked", value.ToString());
        else
          this.attributeList["isborrowerTypeLocked"] = value.ToString();
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public string LockedBorrowerType
    {
      get => this.GetAttribute("lockedBorrowerType");
      set
      {
        if (!this.attributeList.ContainsKey("lockedBorrowerType"))
          this.attributeList.Add("lockedBorrowerType", value.ToString());
        else
          this.attributeList["lockedBorrowerType"] = value.ToString();
      }
    }

    public bool IsCoBorrowerTypeLocked
    {
      get => this.GetAttribute("iscoborrowerTypeLocked") == "True";
      set
      {
        if (!this.attributeList.ContainsKey("iscoborrowerTypeLocked"))
          this.attributeList.Add("iscoborrowerTypeLocked", value.ToString());
        else
          this.attributeList["iscoborrowerTypeLocked"] = value.ToString();
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public string LockedCoBorrowerType
    {
      get => this.GetAttribute("lockedCoBorrowerType");
      set
      {
        if (!this.attributeList.ContainsKey("lockedCoBorrowerType"))
          this.attributeList.Add("lockedCoBorrowerType", value.ToString());
        else
          this.attributeList["lockedCoBorrowerType"] = value.ToString();
      }
    }

    public string ChangeInCircumstance
    {
      get => this.changeInCircumstance;
      set => this.changeInCircumstance = value;
    }

    public string ChangeInCircumstanceComments
    {
      get => this.changeInCircumstanceComments;
      set => this.changeInCircumstanceComments = value;
    }

    public bool IntentToProceed
    {
      get => this.intentToProceed;
      set
      {
        this.intentToProceed = value;
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    private void ValidateNBOCInstance(string nboInstance)
    {
      if (this.nonBorrowerOwnerCollections == null || this.nonBorrowerOwnerCollections.Count == 0)
        throw new Exception("There is no NBO fields exist for the current Instance " + nboInstance);
      if (!nboInstance.Contains("NBOC"))
        throw new Exception("Its not a valid NBO Key: " + nboInstance);
      Convert.ToInt32(nboInstance.Substring(nboInstance.Length - 2));
    }

    private void reCalcDisclosureTracking()
    {
      if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
        this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
      this.MarkAsDirty();
    }

    public void SetnboAttributeValue(
      string nboInstance,
      object value,
      DisclosureTracking2015Log.NBOUpdatableFields fld)
    {
      if (value == null)
        return;
      this.ValidateNBOCInstance(nboInstance);
      switch (fld)
      {
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod:
          this.nonBorrowerOwnerCollections[nboInstance].DisclosedMethod = Utils.ParseInt(value);
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther:
          this.nonBorrowerOwnerCollections[nboInstance].DisclosedMethodOther = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate:
          this.nonBorrowerOwnerCollections[nboInstance].PresumedReceivedDate = Utils.TruncateDate(Utils.ParseDate(value));
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate:
          this.nonBorrowerOwnerCollections[nboInstance].lockedPresumedReceivedDate = Utils.TruncateDate(Utils.ParseDate(value));
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked:
          this.nonBorrowerOwnerCollections[nboInstance].isPresumedDateLocked = Utils.ParseBoolean(value);
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate:
          this.nonBorrowerOwnerCollections[nboInstance].ActualReceivedDate = Utils.TruncateDate(Utils.ParseDate(value));
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked:
          this.nonBorrowerOwnerCollections[nboInstance].isBorrowerTypeLocked = Utils.ParseBoolean(value);
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.BorrowerType:
          this.nonBorrowerOwnerCollections[nboInstance].BorrowerType = value.ToString();
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.LockedBorrowerType:
          this.nonBorrowerOwnerCollections[nboInstance].LockedBorrowerType = value.ToString();
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAuthenticatedDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAuthenticatedIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOViewMessageDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOViewMessageDate = Utils.ParseDate(value);
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBORejectConsentDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBORejectConsentIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOSignedDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOSignedDate = Utils.ParseDate(value);
          this.reCalcDisclosureTracking();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignedIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOeSignedIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOLoanLevelConsent = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAcceptConsentDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAcceptConsentIP = value.ToString();
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBODocumentViewedDate = Utils.ParseDate(value);
          break;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignatures:
          this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOeSignatures = (string) value == "1";
          break;
      }
    }

    public string GetnboAttributeValue(
      string nboInstance,
      DisclosureTracking2015Log.NBOUpdatableFields flds)
    {
      this.ValidateNBOCInstance(nboInstance);
      switch (flds)
      {
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethod:
          return this.nonBorrowerOwnerCollections[nboInstance].DisclosedMethod.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.DisclosedMethodOther:
          return this.nonBorrowerOwnerCollections[nboInstance].DisclosedMethodOther;
        case DisclosureTracking2015Log.NBOUpdatableFields.PresumedReceivedDate:
          return this.nonBorrowerOwnerCollections[nboInstance].PresumedReceivedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.lockedPresumedReceivedDate:
          return this.nonBorrowerOwnerCollections[nboInstance].lockedPresumedReceivedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.isPresumedDateLocked:
          return !this.nonBorrowerOwnerCollections[nboInstance].isPresumedDateLocked ? "F" : "T";
        case DisclosureTracking2015Log.NBOUpdatableFields.ActualReceivedDate:
          return this.nonBorrowerOwnerCollections[nboInstance].ActualReceivedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.isBorrowerTypeLocked:
          return !this.nonBorrowerOwnerCollections[nboInstance].isBorrowerTypeLocked ? "F" : "T";
        case DisclosureTracking2015Log.NBOUpdatableFields.BorrowerType:
          return this.nonBorrowerOwnerCollections[nboInstance].BorrowerType;
        case DisclosureTracking2015Log.NBOUpdatableFields.LockedBorrowerType:
          return this.nonBorrowerOwnerCollections[nboInstance].LockedBorrowerType;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedDate:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAuthenticatedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAuthenticatedIP:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAuthenticatedIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOViewMessageDate:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOViewMessageDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentDate:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBORejectConsentDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBORejectConsentIP:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBORejectConsentIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOSignedDate:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOSignedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignedIP:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOeSignedIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOLoanLevelConsent:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOLoanLevelConsent.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentDate:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAcceptConsentDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOAcceptConsentIP:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOAcceptConsentIP;
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBODocumentViewedDate:
          return this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBODocumentViewedDate.ToString();
        case DisclosureTracking2015Log.NBOUpdatableFields.eDisclosureNBOeSignatures:
          return !this.nonBorrowerOwnerCollections[nboInstance].eDisclosureNBOeSignatures ? "0" : "1";
        default:
          return "";
      }
    }

    public Dictionary<string, INonBorrowerOwnerItem> GetAllnboItems()
    {
      return this.nonBorrowerOwnerCollections;
    }

    public DateTime IntentToProceedDate
    {
      get => this.intentToProceedDate;
      set
      {
        this.intentToProceedDate = Utils.TruncateDate(value);
        this.MarkAsDirty();
      }
    }

    public string IntentToProceedReceivedBy
    {
      get => this.intentToProceedReceivedBy;
      set
      {
        this.intentToProceedReceivedBy = value;
        this.MarkAsDirty();
      }
    }

    public string IntentToProceedReportReceivedBy
    {
      get
      {
        return this.IsIntentReceivedByLocked ? this.LockedIntentReceivedByField : this.IntentToProceedReceivedBy;
      }
    }

    public string LockedIntentReceivedByField
    {
      get => this.lockedIntentReceivedByField;
      set
      {
        this.lockedIntentReceivedByField = value;
        this.MarkAsDirty();
      }
    }

    public DisclosureTrackingBase.DisclosedMethod IntentToProceedReceivedMethod
    {
      get => this.intentToProceedReceivedMethod;
      set => this.intentToProceedReceivedMethod = value;
    }

    public string IntentToProceedReceivedMethodOther
    {
      get => this.intentToProceedMethodOther;
      set => this.intentToProceedMethodOther = value;
    }

    public string IntentToProceedComments
    {
      get => this.intentToProceedComment;
      set => this.intentToProceedComment = value;
    }

    public bool IsIntentReceivedByLocked
    {
      get => this.isIntentReceivedByLocked;
      set
      {
        this.isIntentReceivedByLocked = value;
        this.MarkAsDirty();
      }
    }

    public DisclosureTrackingBase.DisclosedMethod BorrowerDisclosedMethod
    {
      get => this.borrowerDisclosedMethod;
      set => this.borrowerDisclosedMethod = value;
    }

    public string BorrowerDisclosedMethodOther
    {
      get => this.borrowerDisclosedMethodOther;
      set => this.borrowerDisclosedMethodOther = value;
    }

    public bool IsBorrowerPresumedDateLocked
    {
      get => this.isBorrowerPresumedDateLocked;
      set => this.isBorrowerPresumedDateLocked = value;
    }

    public DateTime LockedBorrowerPresumedReceivedDate
    {
      get => this.lockedBorrowerPresumedReceivedDate;
      set => this.lockedBorrowerPresumedReceivedDate = Utils.TruncateDate(value);
    }

    public DateTime BorrowerPresumedReceivedDate
    {
      get => this.borrowerPresumedReceivedDate;
      set
      {
        if (!(value >= this.DisclosedDateTime))
          return;
        this.borrowerPresumedReceivedDate = Utils.TruncateDate(value);
      }
    }

    public DateTime BorrowerPresumedLockedReceivedDate
    {
      get
      {
        return this.IsBorrowerPresumedDateLocked ? this.lockedBorrowerPresumedReceivedDate : this.borrowerPresumedReceivedDate;
      }
    }

    public DateTime BorrowerActualReceivedDate
    {
      get => this.borrowerActualReceivedDate;
      set
      {
        this.borrowerActualReceivedDate = Utils.TruncateDate(value);
        if (this.IsAttachedToLog && this.Log.Loan.Calculator != null)
          this.Log.Loan.Calculator.CalculateLatestDisclosure2015((IDisclosureTracking2015Log) this);
        this.MarkAsDirty();
      }
    }

    public bool IsNboExist
    {
      get => this.nonBorrowerOwnerCollections != null && this.nonBorrowerOwnerCollections.Count > 0;
    }

    public DisclosureTrackingBase.DisclosedMethod CoBorrowerDisclosedMethod
    {
      get => this.coBorrowerDisclosedMethod;
      set => this.coBorrowerDisclosedMethod = value;
    }

    public string CoBorrowerDisclosedMethodOther
    {
      get => this.coBorrowerDisclosedMethodOther;
      set => this.coBorrowerDisclosedMethodOther = value;
    }

    public bool IsCoBorrowerPresumedDateLocked
    {
      get => this.isCoBorrowerPresumedDateLocked;
      set => this.isCoBorrowerPresumedDateLocked = value;
    }

    public DateTime LockedCoBorrowerPresumedReceivedDate
    {
      get => this.lockedCoBorrowerPresumedReceivedDate;
      set => this.lockedCoBorrowerPresumedReceivedDate = Utils.TruncateDate(value);
    }

    public DateTime CoBorrowerPresumedReceivedDate
    {
      get => this.coBorrowerPresumedReceivedDate;
      set
      {
        if (!(value >= this.DisclosedDateTime))
          return;
        this.coBorrowerPresumedReceivedDate = Utils.TruncateDate(value);
      }
    }

    public DateTime CoBorrowerPresumedLockedReceivedDate
    {
      get
      {
        return this.IsCoBorrowerPresumedDateLocked ? this.lockedCoBorrowerPresumedReceivedDate : this.coBorrowerPresumedReceivedDate;
      }
    }

    public DateTime CoBorrowerActualReceivedDate
    {
      get => this.coBorrowerActualReceivedDate;
      set => this.coBorrowerActualReceivedDate = Utils.TruncateDate(value);
    }

    public string BorrowerFulfillmentMethodDescription
    {
      get => this.borrowerFulfillmentMethodDescription;
      set => this.borrowerFulfillmentMethodDescription = value;
    }

    public string CoBorrowerFulfillmentMethodDescription
    {
      get => this.coBorrowerFulfillmentMethodDescription;
      set => this.coBorrowerFulfillmentMethodDescription = value;
    }

    public DateTime PresumedFulfillmentDate
    {
      get => this.presumedFulfillmentDate;
      set => this.presumedFulfillmentDate = value;
    }

    public DateTime ActualFulfillmentDate
    {
      get => this.actualFulfillmentDate;
      set => this.actualFulfillmentDate = value;
    }

    public string UCD
    {
      get => this.ucd;
      set => this.ucd = value;
    }

    public string UDT
    {
      get => this.udt;
      set => this.udt = value;
    }

    public string EDSRequestGuid
    {
      get => this.edsRequestGuid;
      set => this.edsRequestGuid = value;
    }

    public bool BorrowerLoanLevelConsentMapForCC
    {
      get => this.borrowerLoanLevelConsentMapForCC;
      set => this.borrowerLoanLevelConsentMapForCC = value;
    }

    public bool CoBorrowerLoanLevelConsentMapForCC
    {
      get => this.coBorrowerLoanLevelConsentMapForCC;
      set => this.coBorrowerLoanLevelConsentMapForCC = value;
    }

    public bool UCDCreationError
    {
      get => this.ucdCreationError;
      set => this.ucdCreationError = value;
    }

    public string LinkedGuid
    {
      get => this.linkedGuid;
      set => this.linkedGuid = value;
    }

    public override void MarkAsClean() => base.MarkAsClean();

    public override bool IsDirty() => base.IsDirty();

    public string GetAttribute(string name)
    {
      return this.attributeList.ContainsKey(name) ? this.attributeList[name] : string.Empty;
    }

    public void AddAttribute(string name, string value) => this.attributeList.Add(name, value);

    public int NumberOfAttributes() => this.attributeList.Count;

    public Dictionary<string, string> AttributeList => this.attributeList;

    public void resetBorrowerReceiveDate()
    {
      this.borrowerPresumedReceivedDate = DateTime.MinValue;
      this.borrowerActualReceivedDate = DateTime.MinValue;
    }

    public void resetCoBorrowerReceiveDate()
    {
      this.coBorrowerPresumedReceivedDate = DateTime.MinValue;
      this.coBorrowerActualReceivedDate = DateTime.MinValue;
    }

    public override void ToXml(XmlElement e)
    {
      base.ToXml(e);
      AttributeWriter attributeWriter = new AttributeWriter(e);
      attributeWriter.Write("Type", (object) DisclosureTracking2015Log.XmlType);
      attributeWriter.Write("ContainCD", (object) this.containCD);
      attributeWriter.Write("ContainLE", (object) this.containLE);
      attributeWriter.Write("LockedBorrowerType", (object) this.LockedBorrowerType);
      attributeWriter.Write("IsBorrowerTypeLocked", (object) this.IsBorrowerTypeLocked);
      attributeWriter.Write("LockedCoBorrowerType", (object) this.LockedCoBorrowerType);
      attributeWriter.Write("IsCoBorrowerTypeLocked", (object) this.IsCoBorrowerTypeLocked);
      attributeWriter.Write("ProviderListSent", (object) this.providerListSent);
      attributeWriter.Write("ProviderListNoFeeSent", (object) this.providerListNoFeeSent);
      attributeWriter.Write("DisclosedMethodOther", (object) this.sentMethodOther);
      attributeWriter.Write("UseForUCDExport", (object) this.useForUCDExport);
      attributeWriter.Write("IsDisclosed", (object) this.isDisclosed);
      attributeWriter.Write("IsLocked", (object) this.isLocked);
      attributeWriter.Write("LockedDisclosedByField", (object) this.lockedDisclosedByField);
      attributeWriter.Write("LockedDisclosedDateField", (object) this.lockedDisclosedDateField);
      attributeWriter.Write("DisclosedByFullName", (object) this.disclosedByFullName);
      attributeWriter.Write("DisclosureType", (object) this.disclosureType.ToString());
      attributeWriter.Write("LEReasonIsChangedCircumstanceSettlementCharges", (object) this.leReasonIsChangedCircumstanceSettlementCharges);
      attributeWriter.Write("LEReasonIsChangedCircumstanceEligibility", (object) this.leReasonIsChangedCircumstanceEligibility);
      attributeWriter.Write("LEReasonIsRevisionsRequestedByConsumer", (object) this.leReasonIsRevisionsRequestedByConsumer);
      attributeWriter.Write("LEReasonIsInterestRateDependentCharges", (object) this.leReasonIsInterestRateDependentCharges);
      attributeWriter.Write("LEReasonIsExpiration", (object) this.leReasonIsExpiration);
      attributeWriter.Write("LEReasonIsDelayedSettlementOnConstructionLoans", (object) this.leReasonIsDelayedSettlementOnConstructionLoans);
      attributeWriter.Write("LEReasonIsOther", (object) this.leReasonIsOther);
      attributeWriter.Write("LEReasonOther", (object) this.leReasonOther);
      attributeWriter.Write("CDReasonIsChangeInAPR", (object) this.cdReasonIsChangeInAPR);
      attributeWriter.Write("CDReasonIsChangeInLoanProduct", (object) this.cdReasonIsChangeInLoanProduct);
      attributeWriter.Write("CDReasonIsPrepaymentPenaltyAdded", (object) this.cdReasonIsPrepaymentPenaltyAdded);
      attributeWriter.Write("CDReasonIsChangeInSettlementCharges", (object) this.cdReasonIsChangeInSettlementCharges);
      attributeWriter.Write("CDReasonIs24HourAdvancePreview", (object) this.cdReasonIs24HourAdvancePreview);
      attributeWriter.Write("CDReasonIsToleranceCure", (object) this.cdReasonIsToleranceCure);
      attributeWriter.Write("CDReasonIsClericalErrorCorrection", (object) this.cdReasonIsClericalErrorCorrection);
      attributeWriter.Write("CDReasonIsChangedCircumstanceEligibility", (object) this.cdReasonIsChangedCircumstanceEligibility);
      attributeWriter.Write("CDReasonIsRevisionsRequestedByConsumer", (object) this.cdReasonIsRevisionsRequestedByConsumer);
      attributeWriter.Write("CDReasonIsInterestRateDependentCharges", (object) this.cdReasonIsInterestRateDependentCharges);
      attributeWriter.Write("CDReasonIsOther", (object) this.cdReasonIsOther);
      attributeWriter.Write("CDReasonOther", (object) this.cdReasonOther);
      attributeWriter.Write("ChangeInCircumstance", (object) this.changeInCircumstance);
      attributeWriter.Write("ChangeInCircumstanceComments", (object) this.changeInCircumstanceComments);
      attributeWriter.Write("IntentToProceed", (object) this.intentToProceed);
      attributeWriter.Write("IntentToProceedDate", (object) this.intentToProceedDate);
      attributeWriter.Write("IntentToProceedReceivedBy", (object) this.intentToProceedReceivedBy);
      attributeWriter.Write("IntentToProceedReceivedMethod", (object) this.intentToProceedReceivedMethod.ToString());
      attributeWriter.Write("IntentToProceedReceivedMethodOther", (object) this.intentToProceedMethodOther);
      attributeWriter.Write("IntentToProceedComments", (object) this.intentToProceedComment);
      attributeWriter.Write("LockedIntentReceivedByField", (object) this.lockedIntentReceivedByField);
      attributeWriter.Write("IsIntentReceivedByLocked", (object) this.isIntentReceivedByLocked);
      attributeWriter.Write("BorrowerDisclosedMethod", (object) this.borrowerDisclosedMethod.ToString());
      attributeWriter.Write("BorrowerDisclosedMethodOther", (object) this.borrowerDisclosedMethodOther);
      attributeWriter.Write("BorrowerPresumedReceivedDate", (object) this.borrowerPresumedReceivedDate);
      attributeWriter.Write("LockedBorrowerPresumedReceivedDate", (object) this.lockedBorrowerPresumedReceivedDate);
      attributeWriter.Write("IsBorrowerPresumedDateLocked", (object) this.isBorrowerPresumedDateLocked);
      attributeWriter.Write("BorrowerActualReceivedDate", (object) this.borrowerActualReceivedDate);
      attributeWriter.Write("CoBorrowerDisclosedMethod", (object) this.coBorrowerDisclosedMethod.ToString());
      attributeWriter.Write("CoBorrowerDisclosedMethodOther", (object) this.coBorrowerDisclosedMethodOther);
      attributeWriter.Write("CoBorrowerPresumedReceivedDate", (object) this.coBorrowerPresumedReceivedDate);
      attributeWriter.Write("LockedCoBorrowerPresumedReceivedDate", (object) this.lockedCoBorrowerPresumedReceivedDate);
      attributeWriter.Write("IsCoBorrowerPresumedDateLocked", (object) this.isCoBorrowerPresumedDateLocked);
      attributeWriter.Write("CoBorrowerActualReceivedDate", (object) this.coBorrowerActualReceivedDate);
      attributeWriter.Write("LEDisclosedByBroker", (object) this.leDisclosedByBroker);
      attributeWriter.Write("isDisclosedAPRLocked", (object) this.isDisclosedAPRLocked);
      attributeWriter.Write("isDisclosedFinanceChargeLocked", (object) this.isDisclosedFinanceChargeLocked);
      attributeWriter.Write("isDisclosedDailyInterestLocked", (object) this.isDisclosedDailyInterestLocked);
      attributeWriter.Write("isDisclosedReceivedDateLocked", (object) this.isDisclosedReceivedDateLocked);
      attributeWriter.Write("lockedDisclosedAPRField", (object) this.lockedDisclosedAPRField);
      attributeWriter.Write("lockedDisclosedByField", (object) this.lockedDisclosedByField);
      attributeWriter.Write("lockedDisclosedFinanceChargeField", (object) this.lockedDisclosedFinanceChargeField);
      attributeWriter.Write("lockedDisclosedDailyInterestField", (object) this.lockedDisclosedDailyInterestField);
      attributeWriter.Write("borrowerFulfillmentMethodDescription", (object) this.borrowerFulfillmentMethodDescription);
      attributeWriter.Write("coBorrowerFulfillmentMethodDescription", (object) this.coBorrowerFulfillmentMethodDescription);
      attributeWriter.Write("presumedFulfillmentDate", (object) this.presumedFulfillmentDate);
      attributeWriter.Write("actualFulfillmentDate", (object) this.actualFulfillmentDate);
      attributeWriter.Write("DisclosedAPR", (object) this.disclosedAPR);
      attributeWriter.Write("FinanceCharge", (object) this.financeCharge);
      attributeWriter.Write("DisclosedDailyInterest", (object) this.disclosedDailyInterest);
      attributeWriter.Write("DisclosedForCD", (object) this.DisclosedForCD);
      attributeWriter.Write("DisclosedForLE", (object) this.DisclosedForLE);
      attributeWriter.Write("DisclosureMethod", (object) this.DisclosureMethod);
      attributeWriter.Write("DisclosedMethodName", (object) this.DisclosedMethodName);
      attributeWriter.Write("EDSRequestGuid", (object) this.edsRequestGuid);
      attributeWriter.Write("BorrowerLoanLevelConsentMapForCC", (object) this.BorrowerLoanLevelConsentMapForCC);
      attributeWriter.Write("CoBorrowerLoanLevelConsentMapForCC", (object) this.CoBorrowerLoanLevelConsentMapForCC);
      attributeWriter.Write("LinkedGuid", (object) this.linkedGuid);
      attributeWriter.Write("UCDCreationError", (object) this.ucdCreationError);
      this.UpdateNboObjectToAttribute();
      XmlElement element1 = e.OwnerDocument.CreateElement("Attributes");
      foreach (KeyValuePair<string, string> attribute in this.attributeList)
      {
        XmlElement element2 = e.OwnerDocument.CreateElement("Attribute");
        element2.SetAttribute("Name", attribute.Key);
        element2.SetAttribute("Value", attribute.Value);
        element1.AppendChild((XmlNode) element2);
      }
      e.AppendChild((XmlNode) element1);
      if (!this.loanData.IncludeSnapshotInXML)
        return;
      XmlElement element3 = e.OwnerDocument.CreateElement("Fields");
      for (int index = 0; index < this.loanDataList.Count; ++index)
      {
        DisclosedLoanItem loanData = this.loanDataList[index];
        XmlElement element4 = e.OwnerDocument.CreateElement("Field");
        element4.SetAttribute("FieldID", loanData.FieldID);
        element4.SetAttribute("FieldValue", loanData.FieldValue);
        element3.AppendChild((XmlNode) element4);
      }
      e.AppendChild((XmlNode) element3);
    }

    private void UpdateNboObjectToAttribute()
    {
      foreach (KeyValuePair<string, INonBorrowerOwnerItem> borrowerOwnerCollection in this.nonBorrowerOwnerCollections)
      {
        string key = borrowerOwnerCollection.Key;
        INonBorrowerOwnerItem borrowerOwnerItem = borrowerOwnerCollection.Value;
        this.attributeList[key + "01"] = borrowerOwnerCollection.Value.FirstName;
        this.attributeList[key + "02"] = borrowerOwnerCollection.Value.MidName;
        this.attributeList[key + "03"] = borrowerOwnerCollection.Value.LastName;
        this.attributeList[key + "04"] = borrowerOwnerCollection.Value.Suffix;
        this.attributeList[key + "05"] = borrowerOwnerCollection.Value.Address;
        this.attributeList[key + "06"] = borrowerOwnerCollection.Value.City;
        this.attributeList[key + "07"] = borrowerOwnerCollection.Value.State;
        this.attributeList[key + "08"] = borrowerOwnerCollection.Value.Zip;
        this.attributeList[key + "09"] = borrowerOwnerCollection.Value.VestingType;
        this.attributeList[key + "10"] = borrowerOwnerCollection.Value.HomePhone;
        this.attributeList[key + "11"] = borrowerOwnerCollection.Value.Email;
        this.attributeList[key + "12"] = borrowerOwnerCollection.Value.IsNoThirdPartyEmail ? "Y" : "N";
        this.attributeList[key + "13"] = borrowerOwnerCollection.Value.BusiPhone;
        this.attributeList[key + "14"] = borrowerOwnerCollection.Value.Cell;
        this.attributeList[key + "15"] = borrowerOwnerCollection.Value.Fax;
        this.attributeList[key + "16"] = AttributeWriter.dateToString(borrowerOwnerCollection.Value.DOB);
        this.attributeList[key + "99"] = borrowerOwnerCollection.Value.TRGuid;
        this.attributeList[key + "_DisclosedMethod"] = borrowerOwnerCollection.Value.DisclosedMethod.ToString();
        this.attributeList[key + "_DisclosedMethodOther"] = borrowerOwnerCollection.Value.DisclosedMethodOther;
        this.attributeList[key + "_PresumedReceivedDate"] = AttributeWriter.dateToString(borrowerOwnerCollection.Value.PresumedReceivedDate);
        this.attributeList[key + "_lockedPresumedReceivedDate"] = AttributeWriter.dateToString(borrowerOwnerCollection.Value.lockedPresumedReceivedDate);
        this.attributeList[key + "_isPresumedDateLocked"] = borrowerOwnerCollection.Value.isPresumedDateLocked ? "Y" : "N";
        this.attributeList[key + "_ActualReceivedDate"] = AttributeWriter.dateToString(borrowerOwnerCollection.Value.ActualReceivedDate);
        this.attributeList[key + "_isBorrowerTypeLocked"] = borrowerOwnerCollection.Value.isBorrowerTypeLocked ? "Y" : "N";
        this.attributeList[key + "_BorrowerType"] = borrowerOwnerCollection.Value.BorrowerType;
        this.attributeList[key + "_LockedBorrowerType"] = borrowerOwnerCollection.Value.LockedBorrowerType;
        this.attributeList[key + "_eDisclosureNBOAuthenticatedDate"] = AttributeWriter.dateToString(this.ConvertToUtc(borrowerOwnerCollection.Value.eDisclosureNBOAuthenticatedDate));
        this.attributeList[key + "_eDisclosureNBOAuthenticatedIP"] = borrowerOwnerCollection.Value.eDisclosureNBOAuthenticatedIP;
        this.attributeList[key + "_eDisclosureNBOViewMessageDate"] = AttributeWriter.dateToString(this.ConvertToUtc(borrowerOwnerCollection.Value.eDisclosureNBOViewMessageDate));
        this.attributeList[key + "_eDisclosureNBORejectConsentDate"] = AttributeWriter.dateToString(this.ConvertToUtc(borrowerOwnerCollection.Value.eDisclosureNBORejectConsentDate));
        this.attributeList[key + "_eDisclosureNBORejectConsentIP"] = borrowerOwnerCollection.Value.eDisclosureNBORejectConsentIP;
        this.attributeList[key + "_eDisclosureNBOSignedDate"] = AttributeWriter.dateToString(this.ConvertToUtc(borrowerOwnerCollection.Value.eDisclosureNBOSignedDate));
        this.attributeList[key + "_eDisclosureNBOeSignedIP"] = borrowerOwnerCollection.Value.eDisclosureNBOeSignedIP;
        this.attributeList[key + "_eDisclosureNBOAcceptConsentDate"] = AttributeWriter.dateToString(this.ConvertToUtc(borrowerOwnerCollection.Value.eDisclosureNBOAcceptConsentDate));
        this.attributeList[key + "_eDisclosureNBOAcceptConsentIP"] = borrowerOwnerCollection.Value.eDisclosureNBOAcceptConsentIP;
        this.attributeList[key + "_eDisclosureNBODocumentViewedDate"] = AttributeWriter.dateToString(this.ConvertToUtc(borrowerOwnerCollection.Value.eDisclosureNBODocumentViewedDate));
        this.attributeList[key + "_eDisclosureNBOLoanLevelConsent"] = borrowerOwnerCollection.Value.eDisclosureNBOLoanLevelConsent;
        this.attributeList[key + "_eDisclosureNBOeSignatures"] = borrowerOwnerCollection.Value.eDisclosureNBOeSignatures ? "Y" : "N";
      }
      this.attributeList["NBOcount"] = this.nonBorrowerOwnerCollections.Count.ToString();
    }

    public int CompareDisclosedDate(IDisclosureTracking2015Log obj)
    {
      IDisclosureTracking2015Log disclosureTracking2015Log = (IDisclosureTracking2015Log) this;
      int num = DateTime.Compare(disclosureTracking2015Log.DisclosedDateTime, obj.DisclosedDateTime);
      if (num == 0)
        num = DateTime.Compare(disclosureTracking2015Log.Date, obj.Date);
      return num;
    }

    public LoanData LogLoanData => this.loanData;

    public static string GetDisclosedMethodName(DisclosureTrackingBase.DisclosedMethod method)
    {
      switch (method)
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
        case DisclosureTrackingBase.DisclosedMethod.Email:
          return "Email";
        case DisclosureTrackingBase.DisclosedMethod.Phone:
          return "Phone";
        case DisclosureTrackingBase.DisclosedMethod.Signature:
          return "eFolder eDisclosures";
        case DisclosureTrackingBase.DisclosedMethod.ClosingDocsOrder:
          return "Closing Docs Order";
        case DisclosureTrackingBase.DisclosedMethod.eClose:
          return "eClose";
        default:
          return "In Person";
      }
    }

    [Flags]
    public enum DisclosureTrackingType
    {
      None = 0,
      LE = 1,
      CD = 2,
      All = 255, // 0x000000FF
    }

    public enum DisclosureTypeEnum
    {
      None,
      Initial,
      Revised,
      Final,
      PostConsummation,
    }

    public enum TrackingLogStatus
    {
      Active,
      Pending,
    }

    public enum NBOUpdatableFields
    {
      DisclosedMethod,
      DisclosedMethodOther,
      PresumedReceivedDate,
      lockedPresumedReceivedDate,
      isPresumedDateLocked,
      ActualReceivedDate,
      isBorrowerTypeLocked,
      BorrowerType,
      LockedBorrowerType,
      eDisclosureNBOAuthenticatedDate,
      eDisclosureNBOAuthenticatedIP,
      eDisclosureNBOViewMessageDate,
      eDisclosureNBORejectConsentDate,
      eDisclosureNBORejectConsentIP,
      eDisclosureNBOSignedDate,
      eDisclosureNBOeSignedIP,
      eDisclosureNBOLoanLevelConsent,
      eDisclosureNBOAcceptConsentDate,
      eDisclosureNBOAcceptConsentIP,
      eDisclosureNBODocumentViewedDate,
      eDisclosureNBOCAuthenticatedDate,
      eDisclosureNBOCAuthenticatedIP,
      eDisclosureNBOCeSignedIP,
      eDisclosureNBOCRejectConsentDate,
      eDisclosureNBOCRejectConsentIP,
      eDisclosureNBOCSignedDate,
      eDisclosureNBOCViewMessageDate,
      eDisclosureNBOeSignatures,
    }
  }
}
