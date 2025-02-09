// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._TAX_AND_INSURANCECLASS
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
  public class _TAX_AND_INSURANCECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str2);
      string str3 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "12"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "748"), "//", false) != 0, JS.GetStr(loan, "748"), "");
      nameValueCollection.Add("748", str4);
      string str5 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str5);
      string str6 = JS.GetStr(loan, "1894");
      nameValueCollection.Add("1894", str6);
      string str7 = JS.GetStr(loan, "1051");
      nameValueCollection.Add("1051", str7);
      string str8 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str8);
      string str9 = JS.GetStr(loan, "1824");
      nameValueCollection.Add("1824", str9);
      string str10 = JS.GetStr(loan, "2973");
      nameValueCollection.Add("2973", str10);
      string str11 = JS.GetStr(loan, "2974");
      nameValueCollection.Add("2974", str11);
      string str12 = JS.GetStr(loan, "2975");
      nameValueCollection.Add("2975", str12);
      string str13 = JS.GetStr(loan, "VEND.X324");
      nameValueCollection.Add("X324", str13);
      string str14 = JS.GetStr(loan, "VEND.X323");
      nameValueCollection.Add("X323", str14);
      string str15 = JS.GetStr(loan, "VEND.X325") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X325"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X326") + " " + JS.GetStr(loan, "VEND.X327");
      nameValueCollection.Add("X325_X326_X327", str15);
      string str16 = JS.GetStr(loan, "VEND.X329");
      nameValueCollection.Add("X329", str16);
      string str17 = JS.GetStr(loan, "VEND.X437");
      nameValueCollection.Add("X437", str17);
      string str18 = JS.GetStr(loan, "VEND.X438");
      nameValueCollection.Add("X438", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X439"), "//", false) != 0, JS.GetStr(loan, "VEND.X439"), "");
      nameValueCollection.Add("X439", str19);
      string str20 = JS.GetStr(loan, "VEND.X440");
      nameValueCollection.Add("X440", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X441"), "//", false) != 0, JS.GetStr(loan, "VEND.X441"), "");
      nameValueCollection.Add("X441", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X442"), "//", false) != 0, JS.GetStr(loan, "VEND.X442"), "");
      nameValueCollection.Add("X442", str22);
      string str23 = JS.GetStr(loan, "VEND.X157");
      nameValueCollection.Add("X157", str23);
      string str24 = JS.GetStr(loan, "L252");
      nameValueCollection.Add("L252", str24);
      string str25 = JS.GetStr(loan, "VEND.X158") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X158"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X159") + " " + JS.GetStr(loan, "VEND.X160");
      nameValueCollection.Add("X158_X159_X160", str25);
      string str26 = JS.GetStr(loan, "VEND.X163");
      nameValueCollection.Add("X163", str26);
      string str27 = JS.GetStr(loan, "VEND.X162");
      nameValueCollection.Add("X162", str27);
      string str28 = JS.GetStr(loan, "VEND.X166");
      nameValueCollection.Add("X166", str28);
      string str29 = JS.GetStr(loan, "VEND.X443");
      nameValueCollection.Add("X443", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X444"), "//", false) != 0, JS.GetStr(loan, "VEND.X444"), "");
      nameValueCollection.Add("X444", str30);
      string str31 = JS.GetStr(loan, "VEND.X445");
      nameValueCollection.Add("X445", str31);
      string str32 = JS.GetStr(loan, "VEND.X14");
      nameValueCollection.Add("X14", str32);
      string str33 = JS.GetStr(loan, "1500");
      nameValueCollection.Add("1500", str33);
      string str34 = JS.GetStr(loan, "VEND.X15") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X15"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X16") + " " + JS.GetStr(loan, "VEND.X17");
      nameValueCollection.Add("X15_X16_X17", str34);
      string str35 = JS.GetStr(loan, "VEND.X19");
      nameValueCollection.Add("X19", str35);
      string str36 = JS.GetStr(loan, "VEND.X13");
      nameValueCollection.Add("X13", str36);
      string str37 = JS.GetStr(loan, "VEND.X22");
      nameValueCollection.Add("X22", str37);
      string str38 = JS.GetStr(loan, "VEND.X447");
      nameValueCollection.Add("X447", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X448"), "//", false) != 0, JS.GetStr(loan, "VEND.X448"), "");
      nameValueCollection.Add("X448", str39);
      string str40 = JS.GetStr(loan, "VEND.X446");
      nameValueCollection.Add("X446", str40);
      string str41 = JS.GetStr(loan, "VEND.X333");
      nameValueCollection.Add("X333", str41);
      string str42 = JS.GetStr(loan, "VEND.X332");
      nameValueCollection.Add("X332", str42);
      string str43 = JS.GetStr(loan, "VEND.X334") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X334"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X335") + " " + JS.GetStr(loan, "VEND.X336");
      nameValueCollection.Add("X334_X335_X336", str43);
      string str44 = JS.GetStr(loan, "VEND.X338");
      nameValueCollection.Add("X338", str44);
      string str45 = JS.GetStr(loan, "VEND.X449");
      nameValueCollection.Add("X449", str45);
      string str46 = JS.GetStr(loan, "VEND.X450");
      nameValueCollection.Add("X450", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X451"), "//", false) != 0, JS.GetStr(loan, "VEND.X451"), "");
      nameValueCollection.Add("X451", str47);
      string str48 = JS.GetStr(loan, "VEND.X452");
      nameValueCollection.Add("X452", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X453"), "//", false) != 0, JS.GetStr(loan, "VEND.X453"), "");
      nameValueCollection.Add("X453", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X454"), "//", false) != 0, JS.GetStr(loan, "VEND.X454"), "");
      nameValueCollection.Add("X454", str50);
      return nameValueCollection;
    }
  }
}
