// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003PG2_SPANISHCLASS
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
  public class _1003PG2_SPANISHCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str2);
      string str3 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str3);
      string str4 = JS.GetStr(loan, "39");
      nameValueCollection.Add("39", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Married", false) == 0, "X");
      nameValueCollection.Add("52_Married", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Separated", false) == 0, "X");
      nameValueCollection.Add("52_Separated", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Unmarried", false) == 0, "X");
      nameValueCollection.Add("52_Unmarried", str7);
      string str8 = JS.GetStr(loan, "53");
      nameValueCollection.Add("53", str8);
      string str9 = JS.GetStr(loan, "54");
      nameValueCollection.Add("54", str9);
      string str10 = JS.GetStr(loan, "1402");
      nameValueCollection.Add("38", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0115"), "Own", false) == 0, "X");
      nameValueCollection.Add("FR0115_Own", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0115"), "Rent", false) == 0, "X");
      nameValueCollection.Add("FR0115_Rent", str12);
      string str13 = JS.GetStr(loan, "FR0112") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0112"), "", false) != 0, "Y") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0112"), "", false) == 0, "") + JS.GetStr(loan, "FR0124") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0124"), "", false) != 0, "M") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0124"), "", false) == 0, "");
      nameValueCollection.Add("FR0112_FR0124", str13);
      string str14 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str14);
      string str15 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str15);
      string str16 = JS.GetStr(loan, "1416");
      nameValueCollection.Add("1416", str16);
      string str17 = JS.GetStr(loan, "1417") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1418"), "", false) != 0, ",") + JS.GetStr(loan, "1418") + " " + JS.GetStr(loan, "1419");
      nameValueCollection.Add("1417_1418_1419", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0315"), "Own", false) == 0, "X");
      nameValueCollection.Add("FR0315_Own", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0315"), "Rent", false) == 0, "X");
      nameValueCollection.Add("FR0315_Rent", str19);
      string str20 = JS.GetStr(loan, "FR0312") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0312"), "", false) != 0, "Y") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0312"), "", false) == 0, "") + JS.GetStr(loan, "FR0324") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0324"), "", false) != 0, "M") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0324"), "", false) == 0, "");
      nameValueCollection.Add("FR0312", str20);
      string str21 = JS.GetStr(loan, "FR0304");
      nameValueCollection.Add("FR0304", str21);
      string str22 = JS.GetStr(loan, "FR0306") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0307"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0307") + " " + JS.GetStr(loan, "FR0308");
      nameValueCollection.Add("FR0306_FR0307_FR0308", str22);
      string str23 = JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str23);
      string str24 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str24);
      string str25 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str25);
      string str26 = JS.GetStr(loan, "71");
      nameValueCollection.Add("71", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Married", false) == 0, "X");
      nameValueCollection.Add("84_Married", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Separated", false) == 0, "X");
      nameValueCollection.Add("84_Separated", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Unmarried", false) == 0, "X");
      nameValueCollection.Add("84_Unmarried", str29);
      string str30 = JS.GetStr(loan, "85");
      nameValueCollection.Add("85", str30);
      string str31 = JS.GetStr(loan, "86");
      nameValueCollection.Add("86", str31);
      string str32 = JS.GetStr(loan, "1403");
      nameValueCollection.Add("70", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0215"), "Own", false) == 0, "X");
      nameValueCollection.Add("FR0215_Own", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0215"), "Rent", false) == 0, "X");
      nameValueCollection.Add("FR0215_Rent", str34);
      string str35 = JS.GetStr(loan, "FR0212") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0212"), "", false) != 0, "Y") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0212"), "", false) == 0, "") + JS.GetStr(loan, "FR0224") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0224"), "", false) != 0, "M") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0224"), "", false) == 0, "");
      nameValueCollection.Add("FR0212_FR0224", str35);
      string str36 = JS.GetStr(loan, "FR0204");
      nameValueCollection.Add("FR0204", str36);
      string str37 = JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0206_FR0207_FR0208", str37);
      string str38 = JS.GetStr(loan, "1519");
      nameValueCollection.Add("1519", str38);
      string str39 = JS.GetStr(loan, "1520") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1521"), "", false) != 0, ", ") + JS.GetStr(loan, "1521") + " " + JS.GetStr(loan, "1522");
      nameValueCollection.Add("1520_1521_1522", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0415"), "Own", false) == 0, "X");
      nameValueCollection.Add("FR0415_Own", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0415"), "Rent", false) == 0, "X");
      nameValueCollection.Add("FR0415_Rent", str41);
      string str42 = JS.GetStr(loan, "FR0412") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0412"), "", false) != 0, "Y") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0412"), "", false) == 0, "") + JS.GetStr(loan, "FR0424") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0424"), "", false) != 0, "M") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0424"), "", false) == 0, "");
      nameValueCollection.Add("FR0412", str42);
      string str43 = JS.GetStr(loan, "FR0404");
      nameValueCollection.Add("FR0404", str43);
      string str44 = JS.GetStr(loan, "FR0406") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0407"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0407") + " " + JS.GetStr(loan, "FR0408");
      nameValueCollection.Add("FR0406_FR0407_FR0408", str44);
      return nameValueCollection;
    }
  }
}
