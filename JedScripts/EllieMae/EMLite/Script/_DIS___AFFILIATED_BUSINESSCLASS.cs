// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._DIS___AFFILIATED_BUSINESSCLASS
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
  public class _DIS___AFFILIATED_BUSINESSCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101", str1);
      string str2 = JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69");
      nameValueCollection.Add("150_151", str2);
      string str3 = JS.GetStr(loan, "315");
      nameValueCollection.Add("381", str3);
      string str4 = JS.GetStr(loan, "319");
      nameValueCollection.Add("383", str4);
      string str5 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("384_385_386", str5);
      string str6 = JS.GetStr(loan, "11");
      nameValueCollection.Add("31", str6);
      string str7 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("32_33_34", str7);
      string str8 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str8);
      string str9 = JS.GetStr(loan, "315");
      nameValueCollection.Add("381_A", str9);
      string str10 = JS.GetStr(loan, "315");
      nameValueCollection.Add("381_AA", str10);
      string str11 = JS.GetStr(loan, "315");
      nameValueCollection.Add("381_AAA", str11);
      string str12 = JS.GetStr(loan, "AFF.X2");
      nameValueCollection.Add("AFFX2", str12);
      string str13 = JS.GetStr(loan, "AFF.X3");
      nameValueCollection.Add("AFFX3", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "AFF.X4"), "Y", false) == 0, "X");
      nameValueCollection.Add("AFFX4", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "AFF.X5"), "Y", false) == 0, "X");
      nameValueCollection.Add("AFFX5", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "AFF.X6"), "Y", false) == 0, "X");
      nameValueCollection.Add("AFFX6", str16);
      string str17 = JS.GetStr(loan, "AFF.X7");
      nameValueCollection.Add("11360", str17);
      string str18 = JS.GetStr(loan, "AFF.X9");
      nameValueCollection.Add("11362", str18);
      string str19 = JS.GetStr(loan, "AFF.X11");
      nameValueCollection.Add("11364", str19);
      string str20 = JS.GetStr(loan, "AFF.X13");
      nameValueCollection.Add("11366", str20);
      string str21 = JS.GetStr(loan, "AFF.X15");
      nameValueCollection.Add("11368", str21);
      string str22 = JS.GetStr(loan, "AFF.X17");
      nameValueCollection.Add("11370", str22);
      string str23 = JS.GetStr(loan, "AFF.X8");
      nameValueCollection.Add("11361", str23);
      string str24 = JS.GetStr(loan, "AFF.X10");
      nameValueCollection.Add("11363", str24);
      string str25 = JS.GetStr(loan, "AFF.X12");
      nameValueCollection.Add("11365", str25);
      string str26 = JS.GetStr(loan, "AFF.X14");
      nameValueCollection.Add("11367", str26);
      string str27 = JS.GetStr(loan, "AFF.X16");
      nameValueCollection.Add("11369", str27);
      string str28 = JS.GetStr(loan, "AFF.X18");
      nameValueCollection.Add("11371", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "AFF.X31"), "Y", false) == 0, "X");
      nameValueCollection.Add("AFFX31", str29);
      string str30 = JS.GetStr(loan, "AFF.X19");
      nameValueCollection.Add("11373", str30);
      string str31 = JS.GetStr(loan, "AFF.X21");
      nameValueCollection.Add("11375", str31);
      string str32 = JS.GetStr(loan, "AFF.X23");
      nameValueCollection.Add("11377", str32);
      string str33 = JS.GetStr(loan, "AFF.X25");
      nameValueCollection.Add("11379", str33);
      string str34 = JS.GetStr(loan, "AFF.X27");
      nameValueCollection.Add("11381", str34);
      string str35 = JS.GetStr(loan, "AFF.X29");
      nameValueCollection.Add("11383", str35);
      string str36 = JS.GetStr(loan, "AFF.X20");
      nameValueCollection.Add("11374", str36);
      string str37 = JS.GetStr(loan, "AFF.X22");
      nameValueCollection.Add("11376", str37);
      string str38 = JS.GetStr(loan, "AFF.X24");
      nameValueCollection.Add("11378", str38);
      string str39 = JS.GetStr(loan, "AFF.X26");
      nameValueCollection.Add("11380", str39);
      string str40 = JS.GetStr(loan, "AFF.X28");
      nameValueCollection.Add("11382", str40);
      string str41 = JS.GetStr(loan, "AFF.X30");
      nameValueCollection.Add("11384", str41);
      string str42 = "  " + JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101_A", str42);
      string str43 = "  " + JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69");
      nameValueCollection.Add("150_151_A", str43);
      return nameValueCollection;
    }
  }
}
