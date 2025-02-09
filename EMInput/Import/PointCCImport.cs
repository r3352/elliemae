// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Import.PointCCImport
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Import
{
  public class PointCCImport
  {
    private const string className = "PointCCImport";
    private static readonly string sw = Tracing.SwImportExport;
    private string filename;
    private Hashtable pointDict = new Hashtable();
    private Encoding encoding = Encoding.GetEncoding(1252);
    private ClosingCost cc = new ClosingCost();
    private static string[][] PointGFE = new string[52][]
    {
      new string[7]
      {
        "",
        "965",
        "1022",
        "1023",
        "1081",
        "8220",
        "8260"
      },
      new string[7]
      {
        "",
        "967",
        "968",
        "1024",
        "1082",
        "8221",
        "2065"
      },
      new string[7]{ "", "", "", "1005", "1068", "8222", "8262" },
      new string[7]{ "", "", "", "1006", "1069", "8223", "8263" },
      new string[7]{ "", "", "", "1000", "1140", "8224", "8274" },
      new string[7]
      {
        "",
        "969",
        "970",
        "1026",
        "1083",
        "8225",
        ""
      },
      new string[7]{ "", "", "", "1025", "1084", "8225", "8276" },
      new string[7]{ "", "", "", "5355", "1065", "8227", "8277" },
      new string[7]{ "", "", "", "5356", "1066", "8228", "8278" },
      new string[7]{ "", "", "", "5357", "1141", "8229", "8278" },
      new string[7]
      {
        "978",
        "",
        "",
        "1031",
        "1060",
        "8230",
        "8264"
      },
      new string[7]
      {
        "979",
        "",
        "",
        "1032",
        "1061",
        "8231",
        "8265"
      },
      new string[7]
      {
        "980",
        "",
        "",
        "1033",
        "1062",
        "8232",
        "8266"
      },
      new string[7]
      {
        "984",
        "",
        "",
        "1002",
        "1063",
        "8233",
        "8267"
      },
      new string[7]
      {
        "981",
        "",
        "",
        "1020",
        "2091",
        "1142",
        "2067"
      },
      new string[7]
      {
        "985",
        "",
        "",
        "1021",
        "2092",
        "1143",
        "2068"
      },
      new string[7]
      {
        "988",
        "",
        "",
        "1003",
        "2093",
        "1144",
        "2069"
      },
      new string[7]
      {
        "989",
        "",
        "",
        "1004",
        "2094",
        "1145",
        "2070"
      },
      new string[7]
      {
        "",
        "966",
        "973",
        "1027",
        "1085",
        "8249",
        "2071"
      },
      new string[7]{ "", "", "", "1028", "1086", "8250", "2072" },
      new string[7]
      {
        "",
        "404",
        "753",
        "1009",
        "1146",
        "8251",
        "2073"
      },
      new string[7]
      {
        "998",
        "",
        "",
        "5358",
        "1147",
        "8252",
        "2074"
      },
      new string[7]{ "", "", "", "5359", "1087", "8253", "" },
      new string[7]
      {
        "971",
        "",
        "",
        "1038",
        "1064",
        "8254",
        "2076"
      },
      new string[7]
      {
        "",
        "993",
        "753",
        "1047",
        "1149",
        "8255",
        "2077"
      },
      new string[7]
      {
        "",
        "997",
        "755",
        "1029",
        "1088",
        "8256",
        "2078"
      },
      new string[7]
      {
        "",
        "1034",
        "1035",
        "1011",
        "1150",
        "2095",
        "2079"
      },
      new string[7]
      {
        "",
        "995",
        "754",
        "999",
        "1151",
        "8257",
        "2080"
      },
      new string[7]
      {
        "",
        "1040",
        "1041",
        "1048",
        "1152",
        "2096",
        "2081"
      },
      new string[7]
      {
        "743",
        "1058",
        "1096",
        "1059",
        "1153",
        "2097",
        "2082"
      },
      new string[7]
      {
        "745",
        "1126",
        "1097",
        "1127",
        "1154",
        "2098",
        "2083"
      },
      new string[7]
      {
        "300",
        "",
        "",
        "1010",
        "1067",
        "8234",
        "8268"
      },
      new string[7]{ "", "", "", "1001", "1156", "8235", "8269" },
      new string[7]{ "", "", "", "1012", "1157", "8236", "8280" },
      new string[7]{ "", "", "", "1042", "1090", "8237", "8281" },
      new string[7]
      {
        "992",
        "",
        "",
        "1014",
        "1158",
        "8238",
        "8270"
      },
      new string[7]
      {
        "990",
        "",
        "",
        "1016",
        "1091",
        "8239",
        "8282"
      },
      new string[7]
      {
        "991",
        "",
        "",
        "1017",
        "1092",
        "8240",
        "8283"
      },
      new string[7]
      {
        "1128",
        "",
        "",
        "1129",
        "2099",
        "1159",
        "2085"
      },
      new string[7]
      {
        "1130",
        "",
        "",
        "1131",
        "2100",
        "1160",
        "2086"
      },
      new string[7]
      {
        "974",
        "",
        "",
        "1015",
        "1161",
        "8241",
        "8271"
      },
      new string[7]
      {
        "975",
        "",
        "",
        "1043",
        "1162",
        "8242",
        "8272"
      },
      new string[7]
      {
        "976",
        "",
        "",
        "1044",
        "1163",
        "8243",
        "8284"
      },
      new string[7]
      {
        "977",
        "",
        "",
        "1045",
        "1164",
        "8244",
        "8285"
      },
      new string[7]
      {
        "1132",
        "",
        "",
        "1133",
        "1165",
        "2101",
        "2087"
      },
      new string[7]
      {
        "1134",
        "",
        "",
        "1135",
        "1166",
        "2102",
        "2088"
      },
      new string[7]{ "", "", "", "1030", "1167", "8245", "8273" },
      new string[7]
      {
        "986",
        "",
        "",
        "1007",
        "1093",
        "8246",
        "8286"
      },
      new string[7]
      {
        "987",
        "",
        "",
        "1008",
        "1094",
        "8247",
        "8287"
      },
      new string[7]
      {
        "982",
        "",
        "",
        "1046",
        "1095",
        "8248",
        "8288"
      },
      new string[7]
      {
        "1136",
        "",
        "",
        "1137",
        "2103",
        "1168",
        "2089"
      },
      new string[7]
      {
        "1138",
        "",
        "",
        "1139",
        "2104",
        "1169",
        "2090"
      }
    };
    private static string[][] EmGFE = new string[52][]
    {
      new string[8]
      {
        "",
        "388",
        "1619",
        "454",
        "559",
        "SYS.X136",
        "SYS.X17",
        "SYS.X65"
      },
      new string[8]
      {
        "",
        "1061",
        "436",
        "1776",
        "560",
        "SYS.X137",
        "SYS.X18",
        "SYS.X66"
      },
      new string[8]
      {
        "617",
        "",
        "",
        "641",
        "581",
        "SYS.X231",
        "SYS.X19",
        "SYS.X67"
      },
      new string[8]
      {
        "624",
        "",
        "",
        "640",
        "580",
        "SYS.X232",
        "SYS.X20",
        "SYS.X68"
      },
      new string[8]
      {
        "L704",
        "",
        "",
        "329",
        "557",
        "SYS.X138",
        "SYS.X21",
        "SYS.X69"
      },
      new string[8]
      {
        "",
        "389",
        "1620",
        "439",
        "572",
        "SYS.X147",
        "SYS.X28",
        "SYS.X76"
      },
      new string[8]
      {
        "L224",
        "",
        "",
        "336",
        "565",
        "SYS.X141",
        "SYS.X22",
        "SYS.X70"
      },
      new string[8]
      {
        "1812",
        "",
        "",
        "1621",
        "1622",
        "SYS.X233",
        "SYS.X201",
        "SYS.X202"
      },
      new string[8]
      {
        "REGZGFE.X8",
        "",
        "",
        "367",
        "569",
        "SYS.X142",
        "SYS.X23",
        "SYS.X71"
      },
      new string[8]
      {
        "1813",
        "",
        "",
        "1623",
        "1624",
        "SYS.X234",
        "SYS.X203",
        "SYS.X204"
      },
      new string[8]
      {
        "369",
        "",
        "",
        "370",
        "574",
        "SYS.X149",
        "SYS.X30",
        "SYS.X78"
      },
      new string[8]
      {
        "371",
        "",
        "",
        "372",
        "575",
        "SYS.X150",
        "SYS.X31",
        "SYS.X79"
      },
      new string[8]
      {
        "348",
        "",
        "",
        "349",
        "96",
        "SYS.X151",
        "SYS.X32",
        "SYS.X80"
      },
      new string[8]
      {
        "931",
        "",
        "",
        "932",
        "1345",
        "SYS.X152",
        "SYS.X33",
        "SYS.X81"
      },
      new string[8]
      {
        "1390",
        "",
        "",
        "1009",
        "6",
        "SYS.X153",
        "SYS.X34",
        "SYS.X82"
      },
      new string[8]
      {
        "410",
        "",
        "",
        "554",
        "678",
        "SYS.X154",
        "SYS.X35",
        "SYS.X83"
      },
      new string[8]
      {
        "1391",
        "",
        "",
        "81",
        "82",
        "SYS.X155",
        "SYS.X36",
        "SYS.X84"
      },
      new string[8]
      {
        "154",
        "",
        "",
        "155",
        "200",
        "SYS.X156",
        "SYS.X37",
        "SYS.X85"
      },
      new string[8]
      {
        "",
        "332",
        "333",
        "334",
        "561",
        "SYS.X157",
        "SYS.X4",
        "SYS.X86"
      },
      new string[8]
      {
        "L248",
        "",
        "",
        "337",
        "562",
        "SYS.X158",
        "SYS.X38",
        "SYS.X87"
      },
      new string[8]
      {
        "L252",
        "L251",
        "1780",
        "642",
        "578",
        "SYS.X159",
        "SYS.X39",
        "SYS.X88"
      },
      new string[8]
      {
        "1666",
        "",
        "",
        "1667",
        "1668",
        "SYS.X238",
        "SYS.X205",
        "SYS.X206"
      },
      new string[8]
      {
        "",
        "",
        "",
        "1050",
        "571",
        "SYS.X235",
        "SYS.X29",
        "SYS.X77"
      },
      new string[8]
      {
        "L259",
        "",
        "",
        "L260",
        "L261",
        "SYS.X161",
        "SYS.X118",
        "SYS.X128"
      },
      new string[8]
      {
        "",
        "1387",
        "1780",
        "656",
        "596",
        "SYS.X162",
        "SYS.X42",
        "SYS.X91"
      },
      new string[8]
      {
        "",
        "1296",
        "1781",
        "338",
        "563",
        "SYS.X163",
        "SYS.X43",
        "SYS.X92"
      },
      new string[8]
      {
        "",
        "L267",
        "L268",
        "L269",
        "L270",
        "SYS.X164",
        "SYS.X119",
        "SYS.X129"
      },
      new string[8]
      {
        "",
        "1386",
        "231",
        "655",
        "595",
        "SYS.X165",
        "SYS.X44",
        "SYS.X93"
      },
      new string[8]
      {
        "",
        "1388",
        "235",
        "657",
        "597",
        "SYS.X167",
        "SYS.X45",
        "SYS.X94"
      },
      new string[8]
      {
        "1628",
        "1629",
        "1630",
        "1631",
        "1632",
        "SYS.X239",
        "SYS.X207",
        "SYS.X208"
      },
      new string[8]
      {
        "660",
        "340",
        "253",
        "658",
        "598",
        "SYS.X168",
        "SYS.X46",
        "SYS.X95"
      },
      new string[8]
      {
        "610",
        "",
        "",
        "387",
        "582",
        "SYS.X170",
        "SYS.X15",
        "SYS.X97"
      },
      new string[8]
      {
        "395",
        "",
        "",
        "396",
        "583",
        "SYS.X174",
        "SYS.X48",
        "SYS.X98"
      },
      new string[8]
      {
        "391",
        "",
        "",
        "392",
        "584",
        "SYS.X175",
        "SYS.X49",
        "SYS.X99"
      },
      new string[8]
      {
        "56",
        "",
        "",
        "978",
        "1049",
        "SYS.X176",
        "SYS.X16",
        "SYS.X100"
      },
      new string[8]
      {
        "411",
        "",
        "",
        "385",
        "585",
        "SYS.X177",
        "SYS.X50",
        "SYS.X101"
      },
      new string[8]
      {
        "652",
        "",
        "",
        "646",
        "592",
        "SYS.X181",
        "SYS.X52",
        "SYS.X103"
      },
      new string[8]
      {
        "1633",
        "",
        "",
        "1634",
        "1635",
        "SYS.X240",
        "SYS.X209",
        "SYS.X210"
      },
      new string[8]
      {
        "1762",
        "",
        "",
        "1763",
        "1764",
        "SYS.X244",
        "SYS.X217",
        "SYS.X218"
      },
      new string[8]
      {
        "1767",
        "",
        "",
        "1768",
        "1769",
        "SYS.X245",
        "SYS.X219",
        "SYS.X220"
      },
      new string[8]
      {
        "1636",
        "",
        "",
        "390",
        "587",
        "SYS.X182",
        "SYS.X53",
        "SYS.X104"
      },
      new string[8]
      {
        "1637",
        "",
        "",
        "647",
        "593",
        "SYS.X183",
        "SYS.X54",
        "SYS.X105"
      },
      new string[8]
      {
        "1638",
        "",
        "",
        "648",
        "594",
        "SYS.X184",
        "SYS.X55",
        "SYS.X106"
      },
      new string[8]
      {
        "373",
        "",
        "",
        "374",
        "576",
        "SYS.X185",
        "SYS.X56",
        "SYS.X107"
      },
      new string[8]
      {
        "1640",
        "",
        "",
        "1641",
        "1642",
        "SYS.X241",
        "SYS.X211",
        "SYS.X212"
      },
      new string[8]
      {
        "1643",
        "",
        "",
        "1644",
        "1645",
        "SYS.X242",
        "SYS.X213",
        "SYS.X214"
      },
      new string[8]
      {
        "REGZGFE.X15",
        "",
        "",
        "339",
        "564",
        "SYS.X187",
        "SYS.X58",
        "SYS.X109"
      },
      new string[8]
      {
        "650",
        "",
        "",
        "644",
        "590",
        "SYS.X190",
        "SYS.X61",
        "SYS.X112"
      },
      new string[8]
      {
        "651",
        "",
        "",
        "645",
        "591",
        "SYS.X191",
        "SYS.X62",
        "SYS.X113"
      },
      new string[8]
      {
        "40",
        "",
        "",
        "41",
        "42",
        "SYS.X192",
        "SYS.X63",
        "SYS.X114"
      },
      new string[8]
      {
        "43",
        "",
        "",
        "44",
        "55",
        "SYS.X193",
        "SYS.X64",
        "SYS.X115"
      },
      new string[8]
      {
        "1782",
        "",
        "",
        "1783",
        "1784",
        "SYS.X248",
        "SYS.X225",
        "SYS.X226"
      }
    };

    public int ImportFile(IWin32Window owner, string filename, string folder, bool isPublic)
    {
      TemplateSettingsType type = TemplateSettingsType.ClosingCost;
      this.filename = filename;
      if (!this.LoadFile())
        return 0;
      this.ImportFields();
      string str = this.cc.GetSimpleField("1804").Trim();
      if (str == string.Empty)
      {
        Tracing.Log(PointCCImport.sw, TraceLevel.Error, nameof (PointCCImport), "The Closing Cost Scenario  '" + filename + "'  does not have the right format.\r\n");
        return 0;
      }
      this.cc.Description = "Scenario Imported from Point: " + (object) DateTime.Now + "\r\nFilename: " + filename + "\r\nScenario Name: " + str;
      if (str.Length > 200)
        str = str.Substring(0, 200).Trim();
      FileSystemEntry fileSystemEntry = new FileSystemEntry(SystemUtil.CombinePath(folder, str), FileSystemEntry.Types.File, isPublic ? (string) null : Session.UserID);
      if (Session.ConfigurationManager.TemplateSettingsObjectExists(type, fileSystemEntry))
      {
        if (Utils.Dialog(owner, "The Closing Cost Scenario '" + this.filename + " - " + str + "' already exist. Do you want to overwrite it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        {
          Tracing.Log(PointCCImport.sw, TraceLevel.Error, nameof (PointCCImport), "The Closing Cost Scenario  '" + filename + "' was not imported because a scenerio with the same name already exists. The user chose not to overwrite the existing scenario.\r\n");
          return 0;
        }
      }
      Session.ConfigurationManager.SaveTemplateSettings(type, fileSystemEntry, (BinaryObject) (BinaryConvertibleObject) this.cc);
      return 1;
    }

    private void ImportFields()
    {
      for (int index = 0; index < PointCCImport.PointGFE.Length; ++index)
      {
        this.SetField(PointCCImport.EmGFE[index][0], this.GetField(PointCCImport.PointGFE[index][0]));
        this.SetField(PointCCImport.EmGFE[index][1], this.GetField(PointCCImport.PointGFE[index][1]));
        this.SetField(PointCCImport.EmGFE[index][2], this.GetField(PointCCImport.PointGFE[index][2]));
        this.SetPOCField(PointCCImport.EmGFE[index][5], this.GetField(PointCCImport.PointGFE[index][3]));
        this.SetField(PointCCImport.EmGFE[index][6], this.GetField(PointCCImport.PointGFE[index][4]));
        this.SetField(PointCCImport.EmGFE[index][7], this.GetField(PointCCImport.PointGFE[index][6]));
        if (this.GetField(PointCCImport.PointGFE[index][5]) == "X")
          this.SetField(PointCCImport.EmGFE[index][4], this.GetField(PointCCImport.PointGFE[index][3]));
        else
          this.SetField(PointCCImport.EmGFE[index][3], this.GetField(PointCCImport.PointGFE[index][3]));
      }
      this.SetField("1804", this.GetField("7404"));
      this.SetField("GFE1", this.GetField("5029"));
      this.SetField("GFE2", this.GetField("5030"));
      this.SetField("GFE3", this.GetField("5031"));
      double field = this.CalculateField("554,81,155");
      this.SetField("1671", field.ToString());
      field = this.CalculateField("678,82,200");
      this.SetField("1672", field.ToString());
      field = this.CalculateField("1763,1768");
      this.SetField("1673", field.ToString());
      field = this.CalculateField("1764,1769");
      this.SetField("1674", field.ToString());
      field = this.CalculateField("44,1783");
      this.SetField("1675", field.ToString());
      field = this.CalculateField("55,1784");
      this.SetField("1676", field.ToString());
    }

    private void SetField(string field, string val)
    {
      if (val == "X")
        val = "Y";
      if (val == string.Empty)
        return;
      this.cc.SetCurrentField(field, val);
    }

    private void SetPOCField(string field, string val)
    {
      val = !val.StartsWith("(") ? "N" : "Y";
      this.cc.SetCurrentField(field, val);
    }

    private double CalculateField(string list)
    {
      double field = 0.0;
      string str = list;
      char[] chArray = new char[1]{ ',' };
      foreach (object obj in str.Split(chArray))
        field += this.DoubleVal(this.GetEMField(obj.ToString()));
      return field;
    }

    private string GetField(string id)
    {
      string field = string.Empty;
      if (this.pointDict.ContainsKey((object) id))
      {
        field = this.pointDict[(object) id].ToString().Trim();
        if (field == null || field == "")
          field = string.Empty;
      }
      return field;
    }

    private string GetEMField(string id) => this.cc.GetSimpleField(id);

    private double DoubleVal(string val)
    {
      if (val != null)
      {
        if (!(val == string.Empty))
        {
          try
          {
            return double.Parse(val);
          }
          catch (Exception ex)
          {
            return 0.0;
          }
        }
      }
      return 0.0;
    }

    private bool LoadFile()
    {
      FileStream fileStream;
      try
      {
        fileStream = File.Open(this.filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
      }
      catch (Exception ex)
      {
        Tracing.Log(PointCCImport.sw, TraceLevel.Error, nameof (PointCCImport), "Unable to open file  '" + this.filename + "' Make sure your file is a Closing Cost template or it is not corrupted. Exception: " + ex.Message + "\r\n");
        return false;
      }
      int length = (int) fileStream.Length;
      byte[] buffer = new byte[length];
      fileStream.Read(buffer, 0, length);
      fileStream.Close();
      BinaryReader binaryReader = new BinaryReader((Stream) new MemoryStream(buffer));
      this.pointDict.Clear();
      while (true)
      {
        try
        {
          string key = ((ushort) binaryReader.ReadInt16()).ToString();
          int count = (int) binaryReader.ReadByte();
          if (count == (int) byte.MaxValue)
            count = (int) binaryReader.ReadInt16();
          string str = this.encoding.GetString(binaryReader.ReadBytes(count));
          this.pointDict[(object) key] = (object) str;
        }
        catch (EndOfStreamException ex)
        {
          binaryReader.Close();
          break;
        }
        catch (Exception ex)
        {
          Tracing.Log(PointCCImport.sw, TraceLevel.Error, nameof (PointCCImport), "Problem loading the Closing Cost Scenario  '" + this.filename + "' Make sure your file is a Closing Cost template or it is not corrupted. Exception: " + ex.Message + "\r\n");
          return false;
        }
      }
      return true;
    }
  }
}
