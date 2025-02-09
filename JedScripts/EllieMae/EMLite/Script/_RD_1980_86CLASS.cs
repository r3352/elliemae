// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._RD_1980_86CLASS
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
  public class _RD_1980_86CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "USDA.X34");
      nameValueCollection.Add("USDA_X34", str1);
      string str2 = JS.GetStr(loan, "USDA.X35");
      nameValueCollection.Add("USDA_X35", str2);
      string str3 = JS.GetStr(loan, "USDA.X36");
      nameValueCollection.Add("USDA_X36", str3);
      string str4 = JS.GetStr(loan, "USDA.X37") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X38"), "", false) != 0, ", ") + JS.GetStr(loan, "USDA.X38") + " " + JS.GetStr(loan, "USDA.X39");
      nameValueCollection.Add("USDA_X37_X38_X39", str4);
      string str5 = JS.GetStr(loan, "USDA.X40");
      nameValueCollection.Add("USDA_X40", str5);
      string areaCode1 = Jed.GetAreaCode(JS.GetStr(loan, "USDA.X41"));
      nameValueCollection.Add("USDA_X41_AREACODE", areaCode1);
      string phoneNoWithoutExt = Jed.GetPhoneNoWithoutExt(JS.GetStr(loan, "USDA.X41"));
      nameValueCollection.Add("USDA_X41", phoneNoWithoutExt);
      string phoneExt = Jed.GetPhoneExt(JS.GetStr(loan, "USDA.X41"));
      nameValueCollection.Add("USDA_X41_EXT", phoneExt);
      string areaCode2 = Jed.GetAreaCode(JS.GetStr(loan, "USDA.X42"));
      nameValueCollection.Add("USDA_X42_AREACODE", areaCode2);
      string phoneNo = Jed.GetPhoneNo(JS.GetStr(loan, "USDA.X42"));
      nameValueCollection.Add("USDA_X42", phoneNo);
      string str6 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str6);
      string str7 = JS.GetStr(loan, "USDA.X25");
      nameValueCollection.Add("USDA_X25", str7);
      string str8 = JS.GetStr(loan, "USDA.X33");
      nameValueCollection.Add("USDA_X33", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X3"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X6"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X3_X6", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X7"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X7_Y", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X8"), "Guaranteed Loan", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X8_GUARANTEED", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X8"), "Direct Loan", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X8_DIRECT", str12);
      string str13 = JS.GetStr(loan, "USDA.X9");
      nameValueCollection.Add("USDA_X9", str13);
      string str14 = JS.GetStr(loan, "USDA.X185");
      nameValueCollection.Add("USDA_X185", str14);
      string str15 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str15);
      string str16 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str16);
      string str17 = JS.GetStr(loan, "1402");
      nameValueCollection.Add("1402", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("965_Y", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "N", false) == 0, "X", "");
      nameValueCollection.Add("965_N", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("466_Y", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "N", false) == 0, "X", "");
      nameValueCollection.Add("466_N", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X1"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X1_Y", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X1"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X1_N", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X2"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X2_Y", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X2"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X2_N", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Male", false) == 0, "X", "");
      nameValueCollection.Add("471_M", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Female", false) == 0, "X", "");
      nameValueCollection.Add("471_F", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1523_Y", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Not Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1523_N", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1524"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1524", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1525"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1525", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1526"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1526", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1527"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1527", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1528"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1528", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Married", false) == 0, "X", "");
      nameValueCollection.Add("52_M", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Separated", false) == 0, "X", "");
      nameValueCollection.Add("52_S", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "52"), "Unmarried", false) == 0, "X", "");
      nameValueCollection.Add("52_U", str37);
      string str38 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str38);
      string str39 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str39);
      string str40 = JS.GetStr(loan, "1403");
      nameValueCollection.Add("1403", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("985_Y", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "N", false) == 0, "X", "");
      nameValueCollection.Add("985_N", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("467_Y", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "N", false) == 0, "X", "");
      nameValueCollection.Add("467_N", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X4"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X4_Y", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X4"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X4_N", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X5"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X5_Y", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X5"), "N", false) == 0, "X", "");
      nameValueCollection.Add("USDA_X5_N", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Male", false) == 0, "X", "");
      nameValueCollection.Add("478_M", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Female", false) == 0, "X", "");
      nameValueCollection.Add("478_F", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1531_Y", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Not Hispanic or Latino", false) == 0, "X", "");
      nameValueCollection.Add("1531_N", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1532"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1532", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1533"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1533", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1534"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1534", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1535"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1535", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1536"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1536", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Married", false) == 0, "X", "");
      nameValueCollection.Add("84_M", str58);
      string str59 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Separated", false) == 0, "X", "");
      nameValueCollection.Add("84_S", str59);
      string str60 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "84"), "Unmarried", false) == 0, "X", "");
      nameValueCollection.Add("84_U", str60);
      string str61 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str61);
      string str62 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "12"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str62);
      string str63 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str63);
      return nameValueCollection;
    }
  }
}
