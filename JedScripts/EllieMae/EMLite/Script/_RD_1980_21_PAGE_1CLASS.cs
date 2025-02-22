﻿// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._RD_1980_21_PAGE_1CLASS
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
  public class _RD_1980_21_PAGE_1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str1);
      string str2 = JS.GetStr(loan, "1256");
      nameValueCollection.Add("1256", str2);
      string str3 = JS.GetStr(loan, "1262");
      nameValueCollection.Add("1262", str3);
      string str4 = JS.GetStr(loan, "USDA.X27");
      nameValueCollection.Add("USDA_X27", str4);
      string str5 = JS.GetStr(loan, "USDA.X29");
      nameValueCollection.Add("USDA_X29", str5);
      string str6 = JS.GetStr(loan, "USDA.X25");
      nameValueCollection.Add("USDA_X25", str6);
      string str7 = JS.GetStr(loan, "95");
      nameValueCollection.Add("95", str7);
      string str8 = JS.GetStr(loan, "1263");
      nameValueCollection.Add("1263", str8);
      string str9 = JS.GetStr(loan, "USDA.X28");
      nameValueCollection.Add("USDA_X28", str9);
      string str10 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305", str10);
      string str11 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str11);
      string str12 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str12);
      string str13 = JS.GetStr(loan, "1402");
      nameValueCollection.Add("1402", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("965_Y", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "N", false) == 0, "X", "");
      nameValueCollection.Add("965_N", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("466_Y", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "N", false) == 0, "X", "");
      nameValueCollection.Add("466_N", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X1"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X1_Y", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X1"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X1_N", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X2"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X2_Y", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X2"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X2_N", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Male", false) == 0, "X", "");
      nameValueCollection.Add("471_M", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Female", false) == 0, "X", "");
      nameValueCollection.Add("471_F", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1523_Y", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Not Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1523_N", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1524"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1524", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1525"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1525", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1526"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1526", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1527"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1527", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1528"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1528", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Married", false) == 0, "X", "");
      nameValueCollection.Add("52_M", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Separated", false) == 0, "X", "");
      nameValueCollection.Add("52_S", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Unmarried", false) == 0, "X", "");
      nameValueCollection.Add("52_U", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X10"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X10_Y", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X10"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X10_N", str35);
      string str36 = JS.GetStr(loan, "USDA.X11");
      nameValueCollection.Add("USDA_X11", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X3"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X3_Y", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X3"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X3_N", str38);
      string str39 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str39);
      string str40 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str40);
      string str41 = JS.GetStr(loan, "1403");
      nameValueCollection.Add("1403", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("985_Y", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "N", false) == 0, "X", "");
      nameValueCollection.Add("985_N", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("467_Y", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "N", false) == 0, "X", "");
      nameValueCollection.Add("467_N", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X4"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X4_Y", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X4"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X4_N", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X5"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X5_Y", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X5"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X5_N", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Male", false) == 0, "X", "");
      nameValueCollection.Add("478_M", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Female", false) == 0, "X", "");
      nameValueCollection.Add("478_F", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1531_Y", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Not Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1531_N", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1532"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1532", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1533"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1533", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1534"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1534", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1535"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1535", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1536"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1536", str58);
      string str59 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Married", false) == 0, "X", "");
      nameValueCollection.Add("84_M", str59);
      string str60 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Separated", false) == 0, "X", "");
      nameValueCollection.Add("84_S", str60);
      string str61 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Unmarried", false) == 0, "X", "");
      nameValueCollection.Add("84_U", str61);
      string str62 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X13"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X13_Y", str62);
      string str63 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X13"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X13_N", str63);
      string str64 = JS.GetStr(loan, "USDA.X14");
      nameValueCollection.Add("USDA_X14", str64);
      string str65 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X12"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X12", str65);
      string str66 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X15"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X15", str66);
      string str67 = JS.GetStr(loan, "USDA.X194");
      nameValueCollection.Add("USDA_X194", str67);
      string str68 = JS.GetStr(loan, "USDA.X195");
      nameValueCollection.Add("USDA_X195", str68);
      string str69 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X6"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X6_Y", str69);
      string str70 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X6"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X6_N", str70);
      string str71 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str71);
      string str72 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "12"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str72);
      string str73 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str73);
      string str74 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X7"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X7_Y", str74);
      string str75 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X7"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X7_N", str75);
      string str76 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X8"), "Guaranteed Loan", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X8_GUARANTEED", str76);
      string str77 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X8"), "Direct Loan", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X8_DIRECT", str77);
      string str78 = JS.GetStr(loan, "USDA.X9");
      nameValueCollection.Add("USDA_X9", str78);
      string str79 = JS.GetStr(loan, "USDA.X185");
      nameValueCollection.Add("USDA_X185", str79);
      string str80 = JS.GetStr(loan, "USDA.X16");
      nameValueCollection.Add("USDA_X16", str80);
      string str81 = JS.GetStr(loan, "USDA.X17");
      nameValueCollection.Add("USDA_X17", str81);
      string str82 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "740"), "", false) != 0, JS.GetStr(loan, "740") + " %", "");
      nameValueCollection.Add("740", str82);
      string str83 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "742"), "", false) != 0, JS.GetStr(loan, "742") + " %", "");
      nameValueCollection.Add("742", str83);
      string str84 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str84);
      string str85 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str85);
      string str86 = JS.GetStr(loan, "5");
      nameValueCollection.Add("5", str86);
      string str87 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X18"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X18_Y", str87);
      string str88 = JS.GetStr(loan, "761");
      nameValueCollection.Add("761", str88);
      string str89 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "2400"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("2400_Y", str89);
      string str90 = JS.GetStr(loan, "762");
      nameValueCollection.Add("762", str90);
      string str91 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X19"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X19_Y", str91);
      string str92 = JS.GetStr(loan, "USDA.X20");
      nameValueCollection.Add("USDA_X20", str92);
      string str93 = JS.GetStr(loan, "USDA.X198");
      nameValueCollection.Add("USDA_X198", str93);
      string str94 = JS.GetStr(loan, "USDA.X21");
      nameValueCollection.Add("USDA_X21", str94);
      string str95 = JS.GetStr(loan, "NEWHUD.X1585");
      nameValueCollection.Add("NEWHUD_X1585", str95);
      string str96 = JS.GetStr(loan, "USDA.X23");
      nameValueCollection.Add("USDA_X23", str96);
      string str97 = JS.GetStr(loan, "USDA.X24");
      nameValueCollection.Add("USDA_X24", str97);
      string str98 = JS.GetStr(loan, "1826");
      nameValueCollection.Add("1826", str98);
      string str99 = JS.GetStr(loan, "USDA.X26");
      nameValueCollection.Add("USDA_X26", str99);
      return nameValueCollection;
    }
  }
}
