// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._MTG_LOAN_ORIG_AGREEMENTCLASS
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
  public class _MTG_LOAN_ORIG_AGREEMENTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str1);
      string str2 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_a", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "745"), "//", false) != 0, JS.GetStr(loan, "745"), "");
      nameValueCollection.Add("745", str3);
      string str4 = JS.GetStr(loan, "NOTICES.X29");
      nameValueCollection.Add("NOTICESX29", str4);
      string str5 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_b", str5);
      string str6 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str6);
      string str7 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str7);
      string str8 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str8);
      string str9 = JS.GetStr(loan, "326");
      nameValueCollection.Add("326", str9);
      string str10 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str10);
      string str11 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str11);
      string str12 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str12);
      string str13 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str13);
      string str14 = JS.GetStr(loan, "FR0204");
      nameValueCollection.Add("FR0204", str14);
      string str15 = JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0206_FR0207_FR0208", str15);
      return nameValueCollection;
    }
  }
}
