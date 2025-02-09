// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._TITLE_COMMITMENT_REQUESTCLASS
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
  public class _TITLE_COMMITMENT_REQUESTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "416");
      nameValueCollection.Add("416", str1);
      string str2 = JS.GetStr(loan, "411");
      nameValueCollection.Add("411", str2);
      string str3 = JS.GetStr(loan, "412");
      nameValueCollection.Add("412", str3);
      string str4 = JS.GetStr(loan, "413") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1174"), "", false) != 0, ", ") + JS.GetStr(loan, "1174") + " " + JS.GetStr(loan, "414");
      nameValueCollection.Add("413_1174_414", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "417"), "", false) != 0, "(P) ") + JS.GetStr(loan, "417") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1243"), "", false) != 0, " / ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1243"), "", false) != 0, "(F) ") + JS.GetStr(loan, "1243");
      nameValueCollection.Add("417_1243", str5);
      string str6 = JS.GetStr(loan, "88");
      nameValueCollection.Add("88", str6);
      string str7 = JS.GetStr(loan, "317");
      nameValueCollection.Add("317", str7);
      string str8 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str8);
      string str9 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str9);
      string str10 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "324"), "", false) != 0, "(P) ") + JS.GetStr(loan, "324") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, " / ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, "(F) ") + JS.GetStr(loan, "326");
      nameValueCollection.Add("324_326", str11);
      string str12 = "";
      nameValueCollection.Add("57", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("363", str13);
      string str14 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305", str14);
      string str15 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str15);
      string str16 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str16);
      string str17 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107", str17);
      string str18 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str18);
      string str19 = JS.GetStr(loan, "FE0117");
      nameValueCollection.Add("FE0128", str19);
      string str20 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str20);
      string str21 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str21);
      string str22 = JS.GetStr(loan, "FR0204");
      nameValueCollection.Add("FR0204", str22);
      string str23 = JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0206_FR0207", str23);
      string str24 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str24);
      string str25 = JS.GetStr(loan, "FE0217");
      nameValueCollection.Add("FE0228", str25);
      string str26 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "PrimaryResidence", false) == 0, "X");
      nameValueCollection.Add("190_PRIMARY", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "SecondHome", false) == 0, "X");
      nameValueCollection.Add("190_SECOND_HOME", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "Investor", false) == 0, "X");
      nameValueCollection.Add("190_INVESTMENT", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "X");
      nameValueCollection.Add("19_PURCHASE", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_CASH_OUT", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_NOCASH_OUT", str32);
      string str33 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str33);
      string str34 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str34);
      string str35 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str35);
      string str36 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str36);
      string str37 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str37);
      string str38 = JS.GetStr(loan, "1824");
      nameValueCollection.Add("1824", str38);
      string str39 = JS.GetStr(loan, "638") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "704"), "", false) != 0, "  /  ") + JS.GetStr(loan, "704");
      nameValueCollection.Add("638_704", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Detached", false) == 0, "X");
      nameValueCollection.Add("1041_DETACHED", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Attached", false) == 0, "X");
      nameValueCollection.Add("1041_ATTACHED", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "X");
      nameValueCollection.Add("1041_CONDO", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "X");
      nameValueCollection.Add("1041_PUD", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Cooperative", false) == 0, "X");
      nameValueCollection.Add("1041_CO-OP", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "ManufacturedHousing", false) == 0, "X");
      nameValueCollection.Add("1041_MFG", str45);
      string str46 = JS.GetStr(loan, "VEND.X144");
      nameValueCollection.Add("VENDX144", str46);
      string str47 = JS.GetStr(loan, "VEND.X145") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X146"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X146") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X147"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X147") + " " + JS.GetStr(loan, "VEND.X148");
      nameValueCollection.Add("VENDX145_VENDX148", str47);
      string str48 = JS.GetStr(loan, "VEND.X151");
      nameValueCollection.Add("VENDX151", str48);
      string str49 = JS.GetStr(loan, "1256");
      nameValueCollection.Add("1256", str49);
      string str50 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str50);
      string str51 = JS.GetStr(loan, "1257");
      nameValueCollection.Add("1257", str51);
      string str52 = JS.GetStr(loan, "1258") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1259"), "", false) != 0, ", ") + JS.GetStr(loan, "1259") + " " + JS.GetStr(loan, "1260");
      nameValueCollection.Add("1258_1259_1260", str52);
      string str53 = JS.GetStr(loan, "1262") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1262"), "", false) != 0, " (P) ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1263"), "", false) != 0, "/ ") + JS.GetStr(loan, "1263") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1263"), "", false) != 0, " (F)");
      nameValueCollection.Add("1262", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X11"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX11", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X12"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX12", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X13"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX13", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X14"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX14", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X15"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX15", str58);
      string str59 = JS.GetStr(loan, "REQUEST.X16");
      nameValueCollection.Add("REQUESTX16", str59);
      string str60 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "763"), "//", false) != 0, JS.GetStr(loan, "763"));
      nameValueCollection.Add("763", str60);
      string str61 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X17"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX17_Y", str61);
      string str62 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X17"), "N", false) == 0, "X");
      nameValueCollection.Add("REQUESTX17_N", str62);
      string str63 = JS.GetStr(loan, "REQUEST.X18");
      nameValueCollection.Add("REQUESTX18", str63);
      string str64 = JS.GetStr(loan, "REQUEST.X19");
      nameValueCollection.Add("REQUESTX19", str64);
      string str65 = JS.GetStr(loan, "REQUEST.X20");
      nameValueCollection.Add("REQUESTX20", str65);
      return nameValueCollection;
    }
  }
}
