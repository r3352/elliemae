// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._STATEMENT_OF_DENIALCLASS
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
  public class _STATEMENT_OF_DENIALCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101_150_151", str2);
      string str3 = JS.GetStr(loan, "DENIAL.X82");
      nameValueCollection.Add("102", str3);
      string str4 = JS.GetStr(loan, "DENIAL.X83") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X84"), "", false) != 0, ", ") + JS.GetStr(loan, "DENIAL.X84") + " " + JS.GetStr(loan, "DENIAL.X85");
      nameValueCollection.Add("103_104_105", str4);
      string str5 = JS.GetStr(loan, "2");
      nameValueCollection.Add("11", str5);
      string str6 = JS.GetStr(loan, "3");
      nameValueCollection.Add("12", str6);
      string str7 = JS.GetStr(loan, "4");
      nameValueCollection.Add("13", str7);
      string str8 = JS.GetStr(loan, "DENIAL.X71");
      nameValueCollection.Add("DENIAL_X71", str8);
      string str9 = JS.GetStr(loan, "DENIAL.X72");
      nameValueCollection.Add("DENIAL_X72", str9);
      string str10 = JS.GetStr(loan, "DENIAL.X73");
      nameValueCollection.Add("DENIAL_X73", str10);
      string str11 = JS.GetStr(loan, "DENIAL.X74");
      nameValueCollection.Add("DENIAL_X74", str11);
      string str12 = JS.GetStr(loan, "DENIAL.X75");
      nameValueCollection.Add("DENIAL_X75", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X30"), "Y", false) == 0, "X");
      nameValueCollection.Add("3800", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X31"), "Y", false) == 0, "X");
      nameValueCollection.Add("3801", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X32"), "Y", false) == 0, "X");
      nameValueCollection.Add("3802", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X33"), "Y", false) == 0, "X");
      nameValueCollection.Add("3803", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X34"), "Y", false) == 0, "X");
      nameValueCollection.Add("3804", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X35"), "Y", false) == 0, "X");
      nameValueCollection.Add("3805", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X36"), "Y", false) == 0, "X");
      nameValueCollection.Add("3806", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X37"), "Y", false) == 0, "X");
      nameValueCollection.Add("3807", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X39"), "Y", false) == 0, "X");
      nameValueCollection.Add("3809", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X40"), "Y", false) == 0, "X");
      nameValueCollection.Add("3810", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X77"), "Y", false) == 0, "X");
      nameValueCollection.Add("DENIAL_X77", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X78"), "Y", false) == 0, "X");
      nameValueCollection.Add("DENIAL_X78", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X79"), "Y", false) == 0, "X");
      nameValueCollection.Add("DENIAL_X79", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X42"), "Y", false) == 0, "X");
      nameValueCollection.Add("3812", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X43"), "Y", false) == 0, "X");
      nameValueCollection.Add("3813", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X44"), "Y", false) == 0, "X");
      nameValueCollection.Add("3814", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X45"), "Y", false) == 0, "X");
      nameValueCollection.Add("3815", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X46"), "Y", false) == 0, "X");
      nameValueCollection.Add("3816", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X47"), "Y", false) == 0, "X");
      nameValueCollection.Add("3817", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X48"), "Y", false) == 0, "X");
      nameValueCollection.Add("3818", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X49"), "Y", false) == 0, "X");
      nameValueCollection.Add("3819", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X57"), "Y", false) == 0, "X");
      nameValueCollection.Add("3827", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X58"), "Y", false) == 0, "X");
      nameValueCollection.Add("3828", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X59"), "Y", false) == 0, "X");
      nameValueCollection.Add("3829", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X60"), "Y", false) == 0, "X");
      nameValueCollection.Add("3830", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X61"), "Y", false) == 0, "X");
      nameValueCollection.Add("3831", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X62"), "Y", false) == 0, "X");
      nameValueCollection.Add("3832", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X63"), "Y", false) == 0, "X");
      nameValueCollection.Add("3833", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X80"), "Y", false) == 0, "X");
      nameValueCollection.Add("DENIAL_X80", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X65"), "Y", false) == 0, "X");
      nameValueCollection.Add("3846", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X67"), "Y", false) == 0, "X");
      nameValueCollection.Add("3845", str43);
      string str44 = JS.GetStr(loan, "DENIAL.X66");
      nameValueCollection.Add("3847", str44);
      string str45 = JS.GetStr(loan, "DENIAL.X68");
      nameValueCollection.Add("3848", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X12"), "Y", false) == 0, "X");
      nameValueCollection.Add("3837", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X12"), "Y", false) == 0, JS.GetStr(loan, "624"));
      nameValueCollection.Add("3841", str47);
      string str48 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "DENIAL.X12"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "626") + "  " + JS.GetStr(loan, "627") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1245"), "", false) != 0, ", ") + JS.GetStr(loan, "1245") + " " + JS.GetStr(loan, "628"));
      nameValueCollection.Add("3842_3843", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X12"), "Y", false) == 0, JS.GetStr(loan, "629"));
      nameValueCollection.Add("3844", str49);
      return nameValueCollection;
    }
  }
}
