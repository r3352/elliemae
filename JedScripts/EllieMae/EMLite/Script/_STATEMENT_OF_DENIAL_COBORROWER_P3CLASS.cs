// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._STATEMENT_OF_DENIAL_COBORROWER_P3CLASS
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
  public class _STATEMENT_OF_DENIAL_COBORROWER_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X41"));
      nameValueCollection.Add("DISCLOSURE_X41", str1);
      string str2 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "DISCLOSURE.X42") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X42"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X43") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X43"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X44") + " " + JS.GetStr(loan, "DISCLOSURE.X45"));
      nameValueCollection.Add("DISCLOSURE_X42_X43_X44_X45", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X46"));
      nameValueCollection.Add("DISCLOSURE_X46", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X49"));
      nameValueCollection.Add("DISCLOSURE_X49", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X50"));
      nameValueCollection.Add("DISCLOSURE_X50", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X52"));
      nameValueCollection.Add("DISCLOSURE_X52", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, JS.GetStr(loan, "1415"));
      nameValueCollection.Add("1415", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X178"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X630"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X178", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X25"), "Y", false) == 0, "X");
      nameValueCollection.Add("3838", str9);
      string str10 = JS.GetStr(loan, "DENIAL.X91");
      nameValueCollection.Add("DENIAL_X91", str10);
      string str11 = JS.GetStr(loan, "DENIAL.X92") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X92"), "", false) != 0, ", ") + JS.GetStr(loan, "DENIAL.X93") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X94"), "", false) != 0, ", ") + JS.GetStr(loan, "DENIAL.X94") + " " + JS.GetStr(loan, "DENIAL.X95");
      nameValueCollection.Add("DENIAL_X92_DENIAL_X93_DENIAL_X94_DENIAL_X95", str11);
      string str12 = JS.GetStr(loan, "DENIAL.X96");
      nameValueCollection.Add("DENIAL_X96", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X26"), "Y", false) == 0, "X");
      nameValueCollection.Add("3790", str13);
      string str14 = JS.GetStr(loan, "ECOA_NAME");
      nameValueCollection.Add("ECOA_NAME1", str14);
      string str15 = JS.GetStr(loan, "ECOA_ADDR") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "ECOA_ADDR2"), "", false) != 0, " ") + JS.GetStr(loan, "ECOA_ADDR2") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "ECOA_CITY"), "", false) != 0, " ") + JS.GetStr(loan, "ECOA_CITY") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "ECOA_ST"), "", false) != 0, ", ") + JS.GetStr(loan, "ECOA_ST") + " " + JS.GetStr(loan, "ECOA_ZIP");
      nameValueCollection.Add("ECOA_ADDR1", str15);
      string str16 = JS.GetStr(loan, "ECOA_PHONE");
      nameValueCollection.Add("ECOA_PHONE1", str16);
      string str17 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str17);
      string str18 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str18);
      string str19 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X11"), "//", false) != 0, JS.GetStr(loan, "DENIAL.X11"), "");
      nameValueCollection.Add("DENIAL_X11", str20);
      string str21 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str21);
      string str22 = JS.GetStr(loan, "DENIAL.X70");
      nameValueCollection.Add("DENIAL_X70", str22);
      return nameValueCollection;
    }
  }
}
