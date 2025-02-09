// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003PG4__LETTER_CLASS
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
  public class _1003PG4__LETTER_CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str1);
      string str2 = JS.GetStr(loan, "1045");
      nameValueCollection.Add("1045", str2);
      string str3 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str3);
      string str4 = JS.GetStr(loan, "142");
      nameValueCollection.Add("142", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "463"), "Y", false) == 0, "X");
      nameValueCollection.Add("463_Yes", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "463"), "N", false) == 0, "X");
      nameValueCollection.Add("463_No", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "464"), "Y", false) == 0, "X");
      nameValueCollection.Add("464_Yes", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "464"), "N", false) == 0, "X");
      nameValueCollection.Add("464_No", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "173"), "Y", false) == 0, "X");
      nameValueCollection.Add("173_Yes", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "173"), "N", false) == 0, "X");
      nameValueCollection.Add("173_No", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "179"), "Y", false) == 0, "X");
      nameValueCollection.Add("179_Yes", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "179"), "N", false) == 0, "X");
      nameValueCollection.Add("179_No", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "174"), "Y", false) == 0, "X");
      nameValueCollection.Add("174_Yes", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "174"), "N", false) == 0, "X");
      nameValueCollection.Add("174_No", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "180"), "Y", false) == 0, "X");
      nameValueCollection.Add("180_Yes", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "180"), "N", false) == 0, "X");
      nameValueCollection.Add("180_No", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "171"), "Y", false) == 0, "X");
      nameValueCollection.Add("171_Yes", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "171"), "N", false) == 0, "X");
      nameValueCollection.Add("171_No", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "177"), "Y", false) == 0, "X");
      nameValueCollection.Add("177_Yes", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "177"), "N", false) == 0, "X");
      nameValueCollection.Add("177_No", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "Y", false) == 0, "X");
      nameValueCollection.Add("965_Yes", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "965"), "N", false) == 0, "X");
      nameValueCollection.Add("965_No", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "Y", false) == 0, "X");
      nameValueCollection.Add("985_Yes", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "985"), "N", false) == 0, "X");
      nameValueCollection.Add("985_No", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "Y", false) == 0, "X");
      nameValueCollection.Add("466_Yes", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "466"), "N", false) == 0, "X");
      nameValueCollection.Add("466_No", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "Y", false) == 0, "X");
      nameValueCollection.Add("467_Yes", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "467"), "N", false) == 0, "X");
      nameValueCollection.Add("467_No", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "418"), "Yes", false) == 0, "X");
      nameValueCollection.Add("418_Yes", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "418"), "No", false) == 0, "X");
      nameValueCollection.Add("418_No", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1343"), "Yes", false) == 0, "X");
      nameValueCollection.Add("1343_Yes", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1343"), "No", false) == 0, "X");
      nameValueCollection.Add("1343_No", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "403"), "Yes", false) == 0, "X");
      nameValueCollection.Add("403_Yes", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "403"), "No", false) == 0, "X");
      nameValueCollection.Add("403_No", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1108"), "Yes", false) == 0, "X");
      nameValueCollection.Add("1108_Yes", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1108"), "No", false) == 0, "X");
      nameValueCollection.Add("1108_No", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "981"), "PrimaryResidence", false) == 0, "PR") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "981"), "Investment", false) == 0, "IP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "981"), "SecondaryResidence", false) == 0, "SH");
      nameValueCollection.Add("981", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1015"), "PrimaryResidence", false) == 0, "PR") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1015"), "Investment", false) == 0, "IP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1015"), "SecondaryResidence", false) == 0, "SH");
      nameValueCollection.Add("1015", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1069"), "Sole", false) == 0, "S") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1069"), "JointWithSpouse", false) == 0, "SP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1069"), "JointWithOtherThanSpouse", false) == 0, "O");
      nameValueCollection.Add("1069", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1070"), "Sole", false) == 0, "S") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1070"), "JointWithSpouse", false) == 0, "SP") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1070"), "JointWithOtherThanSpouse", false) == 0, "O");
      nameValueCollection.Add("1070", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "188"), "Y", false) == 0, "X");
      nameValueCollection.Add("188", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1523_a", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Not Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1523_b", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1524"), "Y", false) == 0, "X");
      nameValueCollection.Add("1524", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1525"), "Y", false) == 0, "X");
      nameValueCollection.Add("1525", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1526"), "Y", false) == 0, "X");
      nameValueCollection.Add("1526", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1527"), "Y", false) == 0, "X");
      nameValueCollection.Add("1527", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1528"), "Y", false) == 0, "X");
      nameValueCollection.Add("1528", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Female", false) == 0, "X");
      nameValueCollection.Add("471_Female", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Male", false) == 0, "X");
      nameValueCollection.Add("471_Male", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "189"), "Y", false) == 0, "X");
      nameValueCollection.Add("189", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1531_a", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Not Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1531_b", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1532"), "Y", false) == 0, "X");
      nameValueCollection.Add("1532", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1533"), "Y", false) == 0, "X");
      nameValueCollection.Add("1533", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1534"), "Y", false) == 0, "X");
      nameValueCollection.Add("1534", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1535"), "Y", false) == 0, "X");
      nameValueCollection.Add("1535", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1536"), "Y", false) == 0, "X");
      nameValueCollection.Add("1536", str58);
      string str59 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Female", false) == 0, "X");
      nameValueCollection.Add("478_Female", str59);
      string str60 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Male", false) == 0, "X");
      nameValueCollection.Add("478_Male", str60);
      string str61 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "FaceToFace", false) == 0, "X");
      nameValueCollection.Add("479_Face2Face", str61);
      string str62 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Mail", false) == 0, "X");
      nameValueCollection.Add("479_Mail", str62);
      string str63 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Telephone", false) == 0, "X");
      nameValueCollection.Add("479_Telephone", str63);
      string str64 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Internet", false) == 0, "X");
      nameValueCollection.Add("479_Internet", str64);
      string str65 = JS.GetStr(loan, "1612");
      nameValueCollection.Add("1612", str65);
      string str66 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_A", str66);
      string str67 = JS.GetStr(loan, "3237");
      nameValueCollection.Add("3237", str67);
      string str68 = JS.GetStr(loan, "3238");
      nameValueCollection.Add("3238", str68);
      string str69 = JS.GetStr(loan, "1823");
      nameValueCollection.Add("1823", str69);
      string str70 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str70);
      string str71 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str71);
      string str72 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str72);
      string str73 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3261"), "//", false) == 0, "", JS.GetStr(loan, "3261"));
      nameValueCollection.Add("3261", str73);
      return nameValueCollection;
    }
  }
}
