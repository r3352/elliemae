// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_1003_ADDENDUM_P3CLASS
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
  public class _VA_1003_ADDENDUM_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1711"), "HUD / FHA", false) == 0, "X");
      nameValueCollection.Add("1711_HUD/FHA_PG3", str1);
      string str2 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040_PG3", str2);
      string str3 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305_PG3", str3);
      string str4 = JS.GetStr(loan, "1039");
      nameValueCollection.Add("1039_PG3", str4);
      string str5 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_PG3", str5);
      string str6 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_PG3", str6);
      string str7 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104_PG3", str7);
      string str8 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108_PG3", str8);
      string str9 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2_PG3", str9);
      string str10 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3_PG3", str10);
      string str11 = JS.GetStr(loan, "1347");
      nameValueCollection.Add("1347_PG3", str11);
      string str12 = JS.GetStr(loan, "1392");
      nameValueCollection.Add("1392_PG3", str12);
      string str13 = JS.GetStr(loan, "1093");
      nameValueCollection.Add("1093_PG3", str13);
      string str14 = JS.GetStr(loan, "969");
      nameValueCollection.Add("1160_PG3", str14);
      string str15 = JS.GetStr(loan, "232");
      nameValueCollection.Add("232_PG3", str15);
      string str16 = JS.GetStr(loan, "409");
      nameValueCollection.Add("409_PG3", str16);
      string str17 = JS.GetStr(loan, "1059");
      nameValueCollection.Add("1059_PG3", str17);
      string str18 = JS.GetStr(loan, "1060");
      nameValueCollection.Add("1060_PG3", str18);
      string str19 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11_PG3", str19);
      string str20 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15_PG3", str20);
      string str21 = JS.GetStr(loan, "765");
      nameValueCollection.Add("765_PG3", str21);
      string str22 = JS.GetStr(loan, "3632");
      nameValueCollection.Add("3632", str22);
      string str23 = JS.GetStr(loan, "3633");
      nameValueCollection.Add("3633", str23);
      string str24 = JS.GetStr(loan, "3634") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3635"), "", false) != 0, ", ") + JS.GetStr(loan, "3635") + " " + JS.GetStr(loan, "3636");
      nameValueCollection.Add("3634_3635_3636", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3637"), "", false) != 0, "NMLS ID: " + JS.GetStr(loan, "3637"));
      nameValueCollection.Add("3637", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3638"), "", false) != 0, "Tax ID: " + JS.GetStr(loan, "3638"));
      nameValueCollection.Add("3638", str26);
      string str27 = JS.GetStr(loan, "1111");
      nameValueCollection.Add("1111_PG3", str27);
      string str28 = JS.GetStr(loan, "1113");
      nameValueCollection.Add("1113_PG3", str28);
      string str29 = JS.GetStr(loan, "1114") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1743"), "", false) != 0, ", ") + JS.GetStr(loan, "1743") + " " + JS.GetStr(loan, "1744");
      nameValueCollection.Add("1114_PG3", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3640"), "", false) != 0, "NMLS ID: " + JS.GetStr(loan, "3640"));
      nameValueCollection.Add("3640", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3641"), "", false) != 0, "Tax ID: " + JS.GetStr(loan, "3641"));
      nameValueCollection.Add("3641", str31);
      string str32 = JS.GetStr(loan, "1262");
      nameValueCollection.Add("324_PG3", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3195"), "Y", false) == 0, JS.GetStr(loan, "984"), "");
      nameValueCollection.Add("984", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3195"), "Y", false) == 0, JS.GetStr(loan, "3030"), "");
      nameValueCollection.Add("3030", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "153"), "Does", false) == 0, "X");
      nameValueCollection.Add("153_Do", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "153"), "Does not", false) == 0, "X");
      nameValueCollection.Add("153_Donot", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3175"), "Approved", false) == 0, "X");
      nameValueCollection.Add("3175a", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3175"), "Modified And Approved", false) == 0, "X");
      nameValueCollection.Add("3175b", str38);
      string str39 = JS.GetStr(loan, "3656");
      nameValueCollection.Add("3656", str39);
      string str40 = JS.GetStr(loan, "3657");
      nameValueCollection.Add("3657", str40);
      string str41 = JS.GetStr(loan, "3658");
      nameValueCollection.Add("3658", str41);
      string str42 = JS.GetStr(loan, "3176");
      nameValueCollection.Add("3176", str42);
      string str43 = JS.GetStr(loan, "3177");
      nameValueCollection.Add("3177", str43);
      string str44 = JS.GetStr(loan, "3178");
      nameValueCollection.Add("3178", str44);
      string str45 = JS.GetStr(loan, "3179");
      nameValueCollection.Add("3179", str45);
      string str46 = JS.GetStr(loan, "3180");
      nameValueCollection.Add("3180", str46);
      string str47 = JS.GetStr(loan, "3181");
      nameValueCollection.Add("3181", str47);
      string str48 = JS.GetStr(loan, "3182");
      nameValueCollection.Add("3182", str48);
      string str49 = JS.GetStr(loan, "3183");
      nameValueCollection.Add("3183", str49);
      string str50 = JS.GetStr(loan, "3184");
      nameValueCollection.Add("3184", str50);
      string str51 = JS.GetStr(loan, "3196");
      nameValueCollection.Add("3196", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3185"), "Y", false) == 0, "X");
      nameValueCollection.Add("3185", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3186"), "Y", false) == 0, "X");
      nameValueCollection.Add("3186", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3187"), "Y", false) == 0, "X");
      nameValueCollection.Add("3187", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3188"), "Y", false) == 0, "X");
      nameValueCollection.Add("3188", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3189"), "Y", false) == 0, "X");
      nameValueCollection.Add("3189", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3190"), "Y", false) == 0, "X");
      nameValueCollection.Add("3190", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3191"), "Y", false) == 0, "X");
      nameValueCollection.Add("3191", str58);
      string str59 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3193"), "Y", false) == 0, "X");
      nameValueCollection.Add("3193", str59);
      string str60 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3195"), "Y", false) == 0, "X");
      nameValueCollection.Add("3195", str60);
      string str61 = JS.GetStr(loan, "3192");
      nameValueCollection.Add("3192", str61);
      string str62 = JS.GetStr(loan, "3194");
      nameValueCollection.Add("3194", str62);
      return nameValueCollection;
    }
  }
}
