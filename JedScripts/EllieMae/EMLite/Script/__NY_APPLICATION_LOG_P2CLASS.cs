// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__NY_APPLICATION_LOG_P2CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScript;
using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class __NY_APPLICATION_LOG_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "DISCLOSURE.X161");
      nameValueCollection.Add("DISCLOSURE_X161", str1);
      string str2 = JS.GetStr(loan, "NYFEES1309");
      nameValueCollection.Add("NYFEES1309", str2);
      string str3 = JS.GetStr(loan, "NYFEES1308");
      nameValueCollection.Add("NYFEES1308", str3);
      string str4 = JS.GetStr(loan, "NYFEES1307");
      nameValueCollection.Add("NYFEES1307", str4);
      string str5 = JS.GetStr(loan, "NYFEES1306");
      nameValueCollection.Add("NYFEES1306", str5);
      string str6 = JS.GetStr(loan, "NYFEES1302");
      nameValueCollection.Add("NYFEES1302", str6);
      string str7 = JS.GetStr(loan, "NYFEES1301");
      nameValueCollection.Add("NYFEES1301", str7);
      string str8 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str8);
      string str9 = JS.GetStr(loan, "DISCLOSURE.X154");
      nameValueCollection.Add("DISCLOSURE_X154", str9);
      string str10 = JS.GetStr(loan, "NYFEES1709");
      nameValueCollection.Add("NYFEES1709", str10);
      string str11 = JS.GetStr(loan, "NYFEES1708");
      nameValueCollection.Add("NYFEES1708", str11);
      string str12 = JS.GetStr(loan, "NYFEES1707");
      nameValueCollection.Add("NYFEES1707", str12);
      string str13 = JS.GetStr(loan, "NYFEES1706");
      nameValueCollection.Add("NYFEES1706", str13);
      string str14 = JS.GetStr(loan, "NYFEES1702");
      nameValueCollection.Add("NYFEES1702", str14);
      string str15 = JS.GetStr(loan, "NYFEES1701");
      nameValueCollection.Add("NYFEES1701", str15);
      string str16 = JS.GetStr(loan, "DISCLOSURE.X163");
      nameValueCollection.Add("DISCLOSURE_X163", str16);
      string str17 = JS.GetStr(loan, "NYFEES1608");
      nameValueCollection.Add("NYFEES1608", str17);
      string str18 = JS.GetStr(loan, "NYFEES1607");
      nameValueCollection.Add("NYFEES1607", str18);
      string str19 = JS.GetStr(loan, "DISCLOSURE.X160");
      nameValueCollection.Add("DISCLOSURE_X160", str19);
      string str20 = JS.GetStr(loan, "NYFEES1606");
      nameValueCollection.Add("NYFEES1606", str20);
      string str21 = JS.GetStr(loan, "NYFEES1409");
      nameValueCollection.Add("NYFEES1409", str21);
      string str22 = JS.GetStr(loan, "NYFEES1408");
      nameValueCollection.Add("NYFEES1408", str22);
      string str23 = JS.GetStr(loan, "NYFEES1407");
      nameValueCollection.Add("NYFEES1407", str23);
      string str24 = JS.GetStr(loan, "NYFEES1406");
      nameValueCollection.Add("NYFEES1406", str24);
      string str25 = JS.GetStr(loan, "NYFEES1402");
      nameValueCollection.Add("NYFEES1402", str25);
      string str26 = JS.GetStr(loan, "NYFEES1401");
      nameValueCollection.Add("NYFEES1401", str26);
      string str27 = JS.GetStr(loan, "DISCLOSURE.X159");
      nameValueCollection.Add("DISCLOSURE_X159", str27);
      string str28 = JS.GetStr(loan, "DISCLOSURE.X158");
      nameValueCollection.Add("DISCLOSURE_X158", str28);
      string str29 = JS.GetStr(loan, "DISCLOSURE.X157");
      nameValueCollection.Add("DISCLOSURE_X157", str29);
      string str30 = JS.GetStr(loan, "DISCLOSURE.X156");
      nameValueCollection.Add("DISCLOSURE_X156", str30);
      string str31 = JS.GetStr(loan, "DISCLOSURE.X155");
      nameValueCollection.Add("DISCLOSURE_X155", str31);
      string str32 = JS.GetStr(loan, "NYFEES1809");
      nameValueCollection.Add("NYFEES1809", str32);
      string str33 = JS.GetStr(loan, "NYFEES1808");
      nameValueCollection.Add("NYFEES1808", str33);
      string str34 = JS.GetStr(loan, "NYFEES1807");
      nameValueCollection.Add("NYFEES1807", str34);
      string str35 = JS.GetStr(loan, "NYFEES1806");
      nameValueCollection.Add("NYFEES1806", str35);
      string str36 = JS.GetStr(loan, "NYFEES1802");
      nameValueCollection.Add("NYFEES1802", str36);
      string str37 = JS.GetStr(loan, "NYFEES1801");
      nameValueCollection.Add("NYFEES1801", str37);
      string str38 = JS.GetStr(loan, "NYFEES1108");
      nameValueCollection.Add("NYFEES1108", str38);
      string str39 = JS.GetStr(loan, "NYFEES1107");
      nameValueCollection.Add("NYFEES1107", str39);
      string str40 = JS.GetStr(loan, "NYFEES1106");
      nameValueCollection.Add("NYFEES1106", str40);
      string str41 = JS.GetStr(loan, "NYFEES1102");
      nameValueCollection.Add("NYFEES1102", str41);
      string str42 = JS.GetStr(loan, "NYFEES1101");
      nameValueCollection.Add("NYFEES1101", str42);
      string str43 = JS.GetStr(loan, "NYFEES1509");
      nameValueCollection.Add("NYFEES1509", str43);
      string str44 = JS.GetStr(loan, "NYFEES1508");
      nameValueCollection.Add("NYFEES1508", str44);
      string str45 = JS.GetStr(loan, "NYFEES1507");
      nameValueCollection.Add("NYFEES1507", str45);
      string str46 = JS.GetStr(loan, "NYFEES1506");
      nameValueCollection.Add("NYFEES1506", str46);
      string str47 = JS.GetStr(loan, "NYFEES1502");
      nameValueCollection.Add("NYFEES1502", str47);
      string str48 = JS.GetStr(loan, "NYFEES1501");
      nameValueCollection.Add("NYFEES1501", str48);
      string str49 = JS.GetStr(loan, "NYFEES1208");
      nameValueCollection.Add("NYFEES1208", str49);
      string str50 = JS.GetStr(loan, "NYFEES1207");
      nameValueCollection.Add("NYFEES1207", str50);
      string str51 = JS.GetStr(loan, "NYFEES1206");
      nameValueCollection.Add("NYFEES1206", str51);
      string str52 = JS.GetStr(loan, "NYFEES1202");
      nameValueCollection.Add("NYFEES1202", str52);
      string str53 = JS.GetStr(loan, "NYFEES1201");
      nameValueCollection.Add("NYFEES1201", str53);
      string str54 = JS.GetStr(loan, "NYFEES1609");
      nameValueCollection.Add("NYFEES1609", str54);
      string str55 = JS.GetStr(loan, "NYFEES1602");
      nameValueCollection.Add("NYFEES1602", str55);
      string str56 = JS.GetStr(loan, "NYFEES1601");
      nameValueCollection.Add("NYFEES1601", str56);
      string str57 = JS.GetStr(loan, "DISCLOSURE.X162");
      nameValueCollection.Add("DISCLOSURE_X162", str57);
      string str58 = JS.GetStr(loan, "NYFEES1103") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1104"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1104") + " " + JS.GetStr(loan, "NYFEES1105");
      nameValueCollection.Add("MAP11", str58);
      string str59 = JS.GetStr(loan, "NYFEES1203") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1204"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1204") + " " + JS.GetStr(loan, "NYFEES1205");
      nameValueCollection.Add("MAP12", str59);
      string str60 = JS.GetStr(loan, "NYFEES1303") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1304"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1304") + " " + JS.GetStr(loan, "NYFEES1305");
      nameValueCollection.Add("MAP13", str60);
      string str61 = JS.GetStr(loan, "NYFEES1403") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1404"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1404") + " " + JS.GetStr(loan, "NYFEES1405");
      nameValueCollection.Add("MAP14", str61);
      string str62 = JS.GetStr(loan, "NYFEES1503") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1504"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1504") + " " + JS.GetStr(loan, "NYFEES1505");
      nameValueCollection.Add("MAP15", str62);
      string str63 = JS.GetStr(loan, "NYFEES1603") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1604"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1604") + " " + JS.GetStr(loan, "NYFEES1605");
      nameValueCollection.Add("MAP16", str63);
      string str64 = JS.GetStr(loan, "NYFEES1703") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1704"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1704") + " " + JS.GetStr(loan, "NYFEES1705");
      nameValueCollection.Add("MAP17", str64);
      string str65 = JS.GetStr(loan, "NYFEES1803") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1804"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1804") + " " + JS.GetStr(loan, "NYFEES1805");
      nameValueCollection.Add("MAP18", str65);
      return nameValueCollection;
    }
  }
}
