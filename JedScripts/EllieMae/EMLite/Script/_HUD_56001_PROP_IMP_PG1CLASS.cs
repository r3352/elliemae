// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_56001_PROP_IMP_PG1CLASS
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
  public class _HUD_56001_PROP_IMP_PG1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = " " + JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str1);
      string str2 = " " + JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str2);
      string str3 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "463"), "Y", false) == 0, "X");
      nameValueCollection.Add("463_Yes", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "463"), "N", false) == 0, "X");
      nameValueCollection.Add("463_No", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X1"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX1_Yes", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X1"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX1_No", str7);
      string str8 = JS.GetStr(loan, "CAPIAP.X59");
      nameValueCollection.Add("CAPIAPX59", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X2"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX2_Yes", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X2"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX2_No", str10);
      string str11 = JS.GetStr(loan, "CAPIAP.X3");
      nameValueCollection.Add("CAPIAPX3", str11);
      string str12 = JS.GetStr(loan, "CAPIAP.X4");
      nameValueCollection.Add("CAPIAPX4", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "169"), "Y", false) == 0, "X");
      nameValueCollection.Add("169_Yes", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "169"), "N", false) == 0, "X");
      nameValueCollection.Add("169_No", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "172"), "Y", false) == 0, "X");
      nameValueCollection.Add("172_Yes", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "172"), "N", false) == 0, "X");
      nameValueCollection.Add("172_No", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "265"), "Y", false) == 0, "X");
      nameValueCollection.Add("265_Yes", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "265"), "N", false) == 0, "X");
      nameValueCollection.Add("265_No", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "170"), "Y", false) == 0, "X");
      nameValueCollection.Add("1057_Yes", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "170"), "N", false) == 0, "X");
      nameValueCollection.Add("1057_No", str20);
      string str21 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str21);
      string str22 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str22);
      string str23 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str23);
      string str24 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str24);
      string str25 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str25);
      string str26 = JS.GetStr(loan, "FR0112") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0112"), "", false) != 0, "Y") + JS.GetStr(loan, "FR0124") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0124"), "", false) != 0, "M");
      nameValueCollection.Add("FR0112", str26);
      string str27 = JS.GetStr(loan, "FR0115");
      nameValueCollection.Add("FR0115", str27);
      string str28 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str28);
      string str29 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str29);
      string str30 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str30);
      string str31 = JS.GetStr(loan, "FR0204");
      nameValueCollection.Add("FR0204", str31);
      string str32 = JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0206_FR0207_FR0208", str32);
      string str33 = JS.GetStr(loan, "FR0212") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0212"), "", false) != 0, "Y") + JS.GetStr(loan, "FR0224") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0224"), "", false) != 0, "M");
      nameValueCollection.Add("FR0212", str33);
      string str34 = JS.GetStr(loan, "FR0215");
      nameValueCollection.Add("FR0215", str34);
      string str35 = JS.GetStr(loan, "FR0304");
      nameValueCollection.Add("FR0304", str35);
      string str36 = JS.GetStr(loan, "FR0306") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0307"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0307") + " " + JS.GetStr(loan, "FR0308");
      nameValueCollection.Add("FR0306_FR0307_FR0308", str36);
      string str37 = JS.GetStr(loan, "FR0312") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0312"), "", false) != 0, "Y") + JS.GetStr(loan, "FR0324") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0324"), "", false) != 0, "M");
      nameValueCollection.Add("FR0312", str37);
      string str38 = JS.GetStr(loan, "FR0315");
      nameValueCollection.Add("FR0315", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Married", false) == 0, "X");
      nameValueCollection.Add("52_Married", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Separated", false) == 0, "X");
      nameValueCollection.Add("52_Separated", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Unmarried", false) == 0, "X");
      nameValueCollection.Add("52_Unmarried", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Male", false) == 0, "X");
      nameValueCollection.Add("471_Male", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Female", false) == 0, "X");
      nameValueCollection.Add("471_Female", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1402"), "//", false) != 0, JS.GetStr(loan, "1402"), "");
      nameValueCollection.Add("CAPIAPX7", str44);
      string str45 = JS.GetStr(loan, "53");
      nameValueCollection.Add("53", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1523_a", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Not Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1523_b", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1524"), "Y", false) == 0, "X");
      nameValueCollection.Add("1524", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1525"), "Y", false) == 0, "X");
      nameValueCollection.Add("1525", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1526"), "Y", false) == 0, "X");
      nameValueCollection.Add("1526", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1527"), "Y", false) == 0, "X");
      nameValueCollection.Add("1527", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1528"), "Y", false) == 0, "X");
      nameValueCollection.Add("1528", str52);
      string str53 = JS.GetStr(loan, "CAPIAP.X9");
      nameValueCollection.Add("CAPIAPX9", str53);
      string str54 = JS.GetStr(loan, "CAPIAP.X10");
      nameValueCollection.Add("CAPIAPX10", str54);
      string str55 = JS.GetStr(loan, "CAPIAP.X11") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X12"), "", false) != 0, ", ") + JS.GetStr(loan, "CAPIAP.X12") + " " + JS.GetStr(loan, "CAPIAP.X13");
      nameValueCollection.Add("CAPIAPX11_CAPIAPX12_CAPIAPX13", str55);
      string str56 = JS.GetStr(loan, "CAPIAP.X14");
      nameValueCollection.Add("CAPIAPX14", str56);
      string str57 = JS.GetStr(loan, "CAPIAP.X15");
      nameValueCollection.Add("CAPIAPX15", str57);
      string str58 = JS.GetStr(loan, "FR0404");
      nameValueCollection.Add("FR0404", str58);
      string str59 = JS.GetStr(loan, "FR0406") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0407"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0407") + " " + JS.GetStr(loan, "FR0408");
      nameValueCollection.Add("FR0406_FR0407_FR0408", str59);
      string str60 = JS.GetStr(loan, "FR0412") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0412"), "", false) != 0, "Y") + JS.GetStr(loan, "FR0424") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0424"), "", false) != 0, "M");
      nameValueCollection.Add("FR0412", str60);
      string str61 = JS.GetStr(loan, "FR0415");
      nameValueCollection.Add("FR0415", str61);
      string str62 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Married", false) == 0, "X");
      nameValueCollection.Add("84_Married", str62);
      string str63 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Separated", false) == 0, "X");
      nameValueCollection.Add("84_Separated", str63);
      string str64 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Unmarried", false) == 0, "X");
      nameValueCollection.Add("84_Unmarried", str64);
      string str65 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Male", false) == 0, "X");
      nameValueCollection.Add("478_Male", str65);
      string str66 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Female", false) == 0, "X");
      nameValueCollection.Add("478_Female", str66);
      string str67 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1403"), "//", false) != 0, JS.GetStr(loan, "1403"), "");
      nameValueCollection.Add("CAPIAPX8", str67);
      string str68 = JS.GetStr(loan, "85");
      nameValueCollection.Add("85", str68);
      string str69 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1531_a", str69);
      string str70 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Not Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1531_b", str70);
      string str71 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1532"), "Y", false) == 0, "X");
      nameValueCollection.Add("1532", str71);
      string str72 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1533"), "Y", false) == 0, "X");
      nameValueCollection.Add("1533", str72);
      string str73 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1534"), "Y", false) == 0, "X");
      nameValueCollection.Add("1534", str73);
      string str74 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1535"), "Y", false) == 0, "X");
      nameValueCollection.Add("1535", str74);
      string str75 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1536"), "Y", false) == 0, "X");
      nameValueCollection.Add("1536", str75);
      string str76 = JS.GetStr(loan, "CAPIAP.X16");
      nameValueCollection.Add("CAPIAPX16", str76);
      string str77 = JS.GetStr(loan, "CAPIAP.X17");
      nameValueCollection.Add("CAPIAPX17", str77);
      string str78 = JS.GetStr(loan, "CAPIAP.X18") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X19"), "", false) != 0, ", ") + JS.GetStr(loan, "CAPIAP.X19") + " " + JS.GetStr(loan, "CAPIAP.X20");
      nameValueCollection.Add("CAPIAPX18_CAPIAPX19_CAPIAPX20", str78);
      string str79 = JS.GetStr(loan, "CAPIAP.X21");
      nameValueCollection.Add("CAPIAPX21", str79);
      string str80 = JS.GetStr(loan, "CAPIAP.X22");
      nameValueCollection.Add("CAPIAPX22", str80);
      return nameValueCollection;
    }
  }
}
