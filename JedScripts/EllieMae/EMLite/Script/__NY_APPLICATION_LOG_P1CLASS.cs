// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__NY_APPLICATION_LOG_P1CLASS
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
  public class __NY_APPLICATION_LOG_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "NYFEES0709");
      nameValueCollection.Add("NYFEES0709", str1);
      string str2 = JS.GetStr(loan, "NYFEES0708");
      nameValueCollection.Add("NYFEES0708", str2);
      string str3 = JS.GetStr(loan, "NYFEES0707");
      nameValueCollection.Add("NYFEES0707", str3);
      string str4 = JS.GetStr(loan, "NYFEES0706");
      nameValueCollection.Add("NYFEES0706", str4);
      string str5 = JS.GetStr(loan, "NYFEES0702");
      nameValueCollection.Add("NYFEES0702", str5);
      string str6 = JS.GetStr(loan, "NYFEES0701");
      nameValueCollection.Add("NYFEES0701", str6);
      string str7 = JS.GetStr(loan, "1822");
      nameValueCollection.Add("1822", str7);
      string str8 = JS.GetStr(loan, "1834");
      nameValueCollection.Add("1834", str8);
      string str9 = JS.GetStr(loan, "1830");
      nameValueCollection.Add("1830", str9);
      string str10 = JS.GetStr(loan, "NYFEES1008");
      nameValueCollection.Add("NYFEES1008", str10);
      string str11 = JS.GetStr(loan, "NYFEES1007");
      nameValueCollection.Add("NYFEES1007", str11);
      string str12 = JS.GetStr(loan, "NYFEES1006");
      nameValueCollection.Add("NYFEES1006", str12);
      string str13 = JS.GetStr(loan, "NYFEES1002");
      nameValueCollection.Add("NYFEES1002", str13);
      string str14 = JS.GetStr(loan, "NYFEES1001");
      nameValueCollection.Add("NYFEES1001", str14);
      string str15 = JS.GetStr(loan, "NYFEES0406");
      nameValueCollection.Add("NYFEES0406", str15);
      string str16 = JS.GetStr(loan, "NYFEES0402");
      nameValueCollection.Add("NYFEES0402", str16);
      string str17 = JS.GetStr(loan, "NYFEES0401");
      nameValueCollection.Add("NYFEES0401", str17);
      string str18 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str18);
      string str19 = JS.GetStr(loan, "DISCLOSURE.X172");
      nameValueCollection.Add("DISCLOSURE_X172", str19);
      string str20 = JS.GetStr(loan, "NYFEES0809");
      nameValueCollection.Add("NYFEES0809", str20);
      string str21 = JS.GetStr(loan, "NYFEES0808");
      nameValueCollection.Add("NYFEES0808", str21);
      string str22 = JS.GetStr(loan, "NYFEES0807");
      nameValueCollection.Add("NYFEES0807", str22);
      string str23 = JS.GetStr(loan, "NYFEES0806");
      nameValueCollection.Add("NYFEES0806", str23);
      string str24 = JS.GetStr(loan, "NYFEES0802");
      nameValueCollection.Add("NYFEES0802", str24);
      string str25 = JS.GetStr(loan, "NYFEES0801");
      nameValueCollection.Add("NYFEES0801", str25);
      string str26 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str26);
      string str27 = JS.GetStr(loan, "NYFEES0108");
      nameValueCollection.Add("NYFEES0108", str27);
      string str28 = JS.GetStr(loan, "NYFEES0107");
      nameValueCollection.Add("NYFEES0107", str28);
      string str29 = JS.GetStr(loan, "NYFEES0106");
      nameValueCollection.Add("NYFEES0106", str29);
      string str30 = JS.GetStr(loan, "NYFEES0102");
      nameValueCollection.Add("NYFEES0102", str30);
      string str31 = JS.GetStr(loan, "NYFEES0101");
      nameValueCollection.Add("NYFEES0101", str31);
      string str32 = JS.GetStr(loan, "NYFEES0509");
      nameValueCollection.Add("NYFEES0509", str32);
      string str33 = JS.GetStr(loan, "NYFEES0508");
      nameValueCollection.Add("NYFEES0508", str33);
      string str34 = JS.GetStr(loan, "NYFEES0507");
      nameValueCollection.Add("NYFEES0507", str34);
      string str35 = JS.GetStr(loan, "NYFEES0506");
      nameValueCollection.Add("NYFEES0506", str35);
      string str36 = JS.GetStr(loan, "NYFEES0502");
      nameValueCollection.Add("NYFEES0502", str36);
      string str37 = JS.GetStr(loan, "NYFEES0501");
      nameValueCollection.Add("NYFEES0501", str37);
      string str38 = JS.GetStr(loan, "NYFEES0909");
      nameValueCollection.Add("NYFEES0909", str38);
      string str39 = JS.GetStr(loan, "NYFEES0908");
      nameValueCollection.Add("NYFEES0908", str39);
      string str40 = JS.GetStr(loan, "NYFEES0907");
      nameValueCollection.Add("NYFEES0907", str40);
      string str41 = JS.GetStr(loan, "NYFEES0906");
      nameValueCollection.Add("NYFEES0906", str41);
      string str42 = JS.GetStr(loan, "NYFEES0902");
      nameValueCollection.Add("NYFEES0902", str42);
      string str43 = JS.GetStr(loan, "NYFEES0901");
      nameValueCollection.Add("NYFEES0901", str43);
      string str44 = JS.GetStr(loan, "NYFEES0408");
      nameValueCollection.Add("NYFEES0408", str44);
      string str45 = JS.GetStr(loan, "NYFEES0407");
      nameValueCollection.Add("NYFEES0407", str45);
      string str46 = JS.GetStr(loan, "NYFEES1009");
      nameValueCollection.Add("NYFEES1009", str46);
      string str47 = JS.GetStr(loan, "NYFEES0208");
      nameValueCollection.Add("NYFEES0208", str47);
      string str48 = JS.GetStr(loan, "NYFEES0207");
      nameValueCollection.Add("NYFEES0207", str48);
      string str49 = JS.GetStr(loan, "NYFEES0206");
      nameValueCollection.Add("NYFEES0206", str49);
      string str50 = JS.GetStr(loan, "NYFEES0202");
      nameValueCollection.Add("NYFEES0202", str50);
      string str51 = JS.GetStr(loan, "NYFEES0201");
      nameValueCollection.Add("NYFEES0201", str51);
      string str52 = JS.GetStr(loan, "NYFEES0609");
      nameValueCollection.Add("NYFEES0609", str52);
      string str53 = JS.GetStr(loan, "NYFEES0608");
      nameValueCollection.Add("NYFEES0608", str53);
      string str54 = JS.GetStr(loan, "NYFEES0607");
      nameValueCollection.Add("NYFEES0607", str54);
      string str55 = JS.GetStr(loan, "NYFEES0606");
      nameValueCollection.Add("NYFEES0606", str55);
      string str56 = JS.GetStr(loan, "NYFEES0602");
      nameValueCollection.Add("NYFEES0602", str56);
      string str57 = JS.GetStr(loan, "NYFEES0601");
      nameValueCollection.Add("NYFEES0601", str57);
      string str58 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str58);
      string str59 = JS.GetStr(loan, "NYFEES0308");
      nameValueCollection.Add("NYFEES0308", str59);
      string str60 = JS.GetStr(loan, "NYFEES0307");
      nameValueCollection.Add("NYFEES0307", str60);
      string str61 = JS.GetStr(loan, "NYFEES0306");
      nameValueCollection.Add("NYFEES0306", str61);
      string str62 = JS.GetStr(loan, "NYFEES0302");
      nameValueCollection.Add("NYFEES0302", str62);
      string str63 = JS.GetStr(loan, "NYFEES0301");
      nameValueCollection.Add("NYFEES0301", str63);
      string str64 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1819"), "Y", false) == 0, JS.GetStr(loan, "FR0104"), JS.GetStr(loan, "1416"));
      nameValueCollection.Add("MAP1", str64);
      string str65 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "1819"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108"), JS.GetStr(loan, "1417") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1418"), "", false) != 0, ", ") + JS.GetStr(loan, "1418") + " " + JS.GetStr(loan, "1419"));
      nameValueCollection.Add("MAP2", str65);
      string str66 = JS.GetStr(loan, "11");
      nameValueCollection.Add("MAP3", str66);
      string str67 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("MAP4", str67);
      string str68 = JS.GetStr(loan, "NYFEES0103") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0104"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0104") + " " + JS.GetStr(loan, "NYFEES0105");
      nameValueCollection.Add("MAP01", str68);
      string str69 = JS.GetStr(loan, "NYFEES0203") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0204"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0204") + " " + JS.GetStr(loan, "NYFEES0205");
      nameValueCollection.Add("MAP02", str69);
      string str70 = JS.GetStr(loan, "NYFEES0303") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0304"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0304") + " " + JS.GetStr(loan, "NYFEES0305");
      nameValueCollection.Add("MAP03", str70);
      string str71 = JS.GetStr(loan, "NYFEES0403") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0404"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0404") + " " + JS.GetStr(loan, "NYFEES0405");
      nameValueCollection.Add("MAP04", str71);
      string str72 = JS.GetStr(loan, "NYFEES0503") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0504"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0504") + " " + JS.GetStr(loan, "NYFEES0505");
      nameValueCollection.Add("MAP05", str72);
      string str73 = JS.GetStr(loan, "NYFEES0603") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0604"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0604") + " " + JS.GetStr(loan, "NYFEES0605");
      nameValueCollection.Add("MAP06", str73);
      string str74 = JS.GetStr(loan, "NYFEES0703") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0704"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0704") + " " + JS.GetStr(loan, "NYFEES0705");
      nameValueCollection.Add("MAP07", str74);
      string str75 = JS.GetStr(loan, "NYFEES0803") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0804"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0804") + " " + JS.GetStr(loan, "NYFEES0805");
      nameValueCollection.Add("MAP08", str75);
      string str76 = JS.GetStr(loan, "NYFEES0903") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES0904"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES0904") + " " + JS.GetStr(loan, "NYFEES0905");
      nameValueCollection.Add("MAP09", str76);
      string str77 = JS.GetStr(loan, "NYFEES1003") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NYFEES1004"), "", false) != 0, ", ") + JS.GetStr(loan, "NYFEES1004") + " " + JS.GetStr(loan, "NYFEES1005");
      nameValueCollection.Add("MAP10", str77);
      string str78 = JS.GetStr(loan, "1831") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1832"), "", false) != 0, ", ") + JS.GetStr(loan, "1832") + " " + JS.GetStr(loan, "1833");
      nameValueCollection.Add("MAP5", str78);
      return nameValueCollection;
    }
  }
}
