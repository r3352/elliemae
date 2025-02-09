// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._INSURANCE_EVIDENCECLASS
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
  public class _INSURANCE_EVIDENCECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362", str1);
      string str2 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str2);
      string str3 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str3);
      string str4 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str4);
      string str5 = JS.GetStr(loan, "324") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "324"), "", false) != 0, " (P) ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, "/ ") + JS.GetStr(loan, "326") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, " (F)");
      nameValueCollection.Add("324_326", str5);
      string str6 = "";
      nameValueCollection.Add("57", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"), "");
      nameValueCollection.Add("363", str7);
      string str8 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305", str8);
      string str9 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str9);
      string str10 = JS.GetStr(loan, "FR0104") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0106"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0104_FR0106_FR0107", str10);
      string str11 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str11);
      string str12 = JS.GetStr(loan, "FE0117");
      nameValueCollection.Add("FE0117", str12);
      string str13 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str13);
      string str14 = JS.GetStr(loan, "FR0204") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0206"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0204_FR0206_FR0207", str14);
      string str15 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str15);
      string str16 = JS.GetStr(loan, "FE0217");
      nameValueCollection.Add("FE0217", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Detached", false) == 0, "  X");
      nameValueCollection.Add("1041_DETACHED", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Attached", false) == 0, "  X");
      nameValueCollection.Add("1041_ATTACHED", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "  X");
      nameValueCollection.Add("1041_CONDO", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "  X");
      nameValueCollection.Add("1041_PUD", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Cooperative", false) == 0, "  X");
      nameValueCollection.Add("1041_CO-OP", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "  X");
      nameValueCollection.Add("19_PURCHASE", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0, "  X");
      nameValueCollection.Add("19_CASH_OUT", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "  X");
      nameValueCollection.Add("19_NOCASH_OUT", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "  X");
      nameValueCollection.Add("420_1ST", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "  X");
      nameValueCollection.Add("420_2ND", str26);
      string str27 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str27);
      string str28 = "";
      nameValueCollection.Add("5891", str28);
      string str29 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str29);
      string str30 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str30);
      string str31 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str31);
      string str32 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str32);
      string str33 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str33);
      string str34 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362_a", str34);
      string str35 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_a", str35);
      string str36 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319_a", str36);
      string str37 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323_a", str37);
      string str38 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324_a", str38);
      string str39 = JS.GetStr(loan, "326");
      nameValueCollection.Add("326_a", str39);
      string str40 = JS.GetStr(loan, "763");
      nameValueCollection.Add("763", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "676"), "Y", false) == 0, "  X");
      nameValueCollection.Add("676", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1242"), "Y", false) == 0, "  X");
      nameValueCollection.Add("1242", str42);
      return nameValueCollection;
    }
  }
}
