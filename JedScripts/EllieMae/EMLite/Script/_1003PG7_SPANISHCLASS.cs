// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003PG7_SPANISHCLASS
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
  public class _1003PG7_SPANISHCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "188"), "Y", false) == 0, "X");
      nameValueCollection.Add("188", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1523_a", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1523"), "Not Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1523_b", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1524"), "Y", false) == 0, "X");
      nameValueCollection.Add("1524", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1525"), "Y", false) == 0, "X");
      nameValueCollection.Add("1525", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1526"), "Y", false) == 0, "X");
      nameValueCollection.Add("1526", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1527"), "Y", false) == 0, "X");
      nameValueCollection.Add("1527", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1528"), "Y", false) == 0, "X");
      nameValueCollection.Add("1528", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Female", false) == 0, "X");
      nameValueCollection.Add("471_Female", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "471"), "Male", false) == 0, "X");
      nameValueCollection.Add("471_Male", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "189"), "Y", false) == 0, "X");
      nameValueCollection.Add("189", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1531_a", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1531"), "Not Hispanic or Latino", false) == 0, "X");
      nameValueCollection.Add("1531_b", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1532"), "Y", false) == 0, "X");
      nameValueCollection.Add("1532", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1533"), "Y", false) == 0, "X");
      nameValueCollection.Add("1533", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1534"), "Y", false) == 0, "X");
      nameValueCollection.Add("1534", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1535"), "Y", false) == 0, "X");
      nameValueCollection.Add("1535", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1536"), "Y", false) == 0, "X");
      nameValueCollection.Add("1536", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Female", false) == 0, "X");
      nameValueCollection.Add("478_Female", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "478"), "Male", false) == 0, "X");
      nameValueCollection.Add("478_Male", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "FaceToFace", false) == 0, "X");
      nameValueCollection.Add("479_Face2Face", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Mail", false) == 0, "X");
      nameValueCollection.Add("479_Mail", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Telephone", false) == 0, "X");
      nameValueCollection.Add("479_Telephone", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Internet", false) == 0, "X");
      nameValueCollection.Add("479_Internet", str24);
      string str25 = JS.GetStr(loan, "1612");
      nameValueCollection.Add("1612", str25);
      string str26 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_A", str26);
      string str27 = JS.GetStr(loan, "3237");
      nameValueCollection.Add("3237", str27);
      string str28 = JS.GetStr(loan, "3238");
      nameValueCollection.Add("3238", str28);
      string str29 = JS.GetStr(loan, "1823");
      nameValueCollection.Add("1823", str29);
      string str30 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str30);
      string str31 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str31);
      string str32 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3261"), "//", false) == 0, "", JS.GetStr(loan, "3261"));
      nameValueCollection.Add("3261", str33);
      return nameValueCollection;
    }
  }
}
