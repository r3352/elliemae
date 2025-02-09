// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_928005B_APPR_VAL_P1CLASS
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
  public class _HUD_928005B_APPR_VAL_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1141"), "Y", false) == 0, " X");
      nameValueCollection.Add("1141", str1);
      string str2 = JS.GetStr(loan, "1039");
      nameValueCollection.Add("1039", str2);
      string str3 = JS.GetStr(loan, "984") + "  (" + JS.GetStr(loan, "3030") + ")";
      nameValueCollection.Add("984_980", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1365"), "Y", false) == 0, " X");
      nameValueCollection.Add("1365", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1038"), "//", false) != 0, JS.GetStr(loan, "1038"));
      nameValueCollection.Add("1038", str5);
      string str6 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str6);
      string str7 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str7);
      string str8 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str8);
      string str9 = JS.GetStr(loan, "13");
      nameValueCollection.Add("313", str9);
      string str10 = JS.GetStr(loan, "1059");
      nameValueCollection.Add("1059", str10);
      string str11 = JS.GetStr(loan, "1060");
      nameValueCollection.Add("1060", str11);
      string str12 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str12);
      string str13 = JS.GetStr(loan, "352");
      nameValueCollection.Add("352", str13);
      string str14 = JS.GetStr(loan, "3342");
      nameValueCollection.Add("3342", str14);
      string str15 = JS.GetStr(loan, "3343");
      nameValueCollection.Add("3343", str15);
      string str16 = JS.GetStr(loan, "3344") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3345"), "", false) != 0, ", ") + JS.GetStr(loan, "3345") + " " + JS.GetStr(loan, "3346");
      nameValueCollection.Add("3344_3345_3346", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1067"), "ExistingConstruction", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1067"), "NewConstruction", false) == 0, " X");
      nameValueCollection.Add("1067_Existing", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1067"), "ProposedConstruction", false) == 0, " X");
      nameValueCollection.Add("1067_Proposed", str18);
      string str19 = JS.GetStr(loan, "230");
      nameValueCollection.Add("230", str19);
      string str20 = JS.GetStr(loan, "1405");
      nameValueCollection.Add("1405", str20);
      string str21 = JS.GetStr(loan, "1068");
      nameValueCollection.Add("1068", str21);
      string str22 = JS.GetStr(loan, "1072");
      nameValueCollection.Add("1072", str22);
      string str23 = JS.GetStr(loan, "1071");
      nameValueCollection.Add("1071", str23);
      string str24 = JS.GetStr(loan, "234");
      nameValueCollection.Add("234", str24);
      string str25 = JS.GetStr(loan, "233");
      nameValueCollection.Add("233", str25);
      string str26 = JS.GetStr(loan, "1075");
      nameValueCollection.Add("1075", str26);
      string str27 = JS.GetStr(loan, "1082");
      nameValueCollection.Add("1082", str27);
      string str28 = JS.GetStr(loan, "1142");
      nameValueCollection.Add("1142", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1342"), "Y", false) == 0, " X");
      nameValueCollection.Add("1342_Is", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1342"), "Y", false) != 0, " X");
      nameValueCollection.Add("1342_Isnot", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "603"), "Y", false) == 0, " X");
      nameValueCollection.Add("603", str31);
      string str32 = JS.GetStr(loan, "1080");
      nameValueCollection.Add("1080", str32);
      string str33 = Jed.BF(Jed.S2N(JS.GetStr(loan, "1080")) > 0.0, " X");
      nameValueCollection.Add("1080_a", str33);
      string str34 = JS.GetStr(loan, "649");
      nameValueCollection.Add("649", str34);
      string str35 = Jed.BF(Jed.S2N(JS.GetStr(loan, "649")) > 0.0, " X");
      nameValueCollection.Add("649_a", str35);
      string str36 = JS.GetStr(loan, "966");
      nameValueCollection.Add("966", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "966"), "", false) != 0 | Operators.CompareString(JS.GetStr(loan, "1081"), "", false) != 0 | Operators.CompareString(JS.GetStr(loan, "1189"), "", false) != 0, " X");
      nameValueCollection.Add("966_a", str37);
      string str38 = JS.GetStr(loan, "1081");
      nameValueCollection.Add("1081", str38);
      string str39 = JS.GetStr(loan, "1189");
      nameValueCollection.Add("1189", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1190"), "", false) != 0, " X");
      nameValueCollection.Add("1190_a", str40);
      string str41 = JS.GetStr(loan, "1190");
      nameValueCollection.Add("1190", str41);
      return nameValueCollection;
    }
  }
}
