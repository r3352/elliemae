// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._PRIVACYPOLICYP2CLASS
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
  public class _PRIVACYPOLICYP2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str1);
      string str2 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315a", str2);
      string str3 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315b", str3);
      string str4 = JS.GetStr(loan, "NOTICES.X72");
      nameValueCollection.Add("NOTICES_X72", str4);
      string str5 = JS.GetStr(loan, "NOTICES.X73");
      nameValueCollection.Add("NOTICES_X73", str5);
      string str6 = JS.GetStr(loan, "NOTICES.X74");
      nameValueCollection.Add("NOTICES_X74", str6);
      string str7 = JS.GetStr(loan, "NOTICES.X75");
      nameValueCollection.Add("NOTICES_X75", str7);
      string str8 = JS.GetStr(loan, "NOTICES.X76");
      nameValueCollection.Add("NOTICES_X76", str8);
      string str9 = JS.GetStr(loan, "NOTICES.X77");
      nameValueCollection.Add("NOTICES_X77", str9);
      string str10 = JS.GetStr(loan, "NOTICES.X94");
      nameValueCollection.Add("NOTICES_X94", str10);
      string str11 = JS.GetStr(loan, "NOTICES.X78");
      nameValueCollection.Add("NOTICES_X78", str11);
      string str12 = JS.GetStr(loan, "NOTICES.X88");
      nameValueCollection.Add("NOTICES_X88", str12);
      string str13 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "NOTICES.X79"), "We have affiliates and share personal information with them", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "NOTICES.X80") + " " + JS.GetStr(loan, "NOTICES.X81") + " " + JS.GetStr(loan, "NOTICES.X82"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X79"), "We have affiliates but do not share personal information with them", false) == 0, JS.GetStr(loan, "315") + " does not share with our affiliates.", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X79"), "We do not have affiliates", false) == 0, JS.GetStr(loan, "315") + " has no affiliates.", "")));
      nameValueCollection.Add("NOTICES_X79", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X83"), "We share personal information with nonaffiliated third parties", false) == 0, JS.GetStr(loan, "NOTICES.X84"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X83"), "We do not share personal information with nonaffiliated third parties", false) == 0, JS.GetStr(loan, "315") + " does not share with nonaffiliates so they can market to you.", ""));
      nameValueCollection.Add("NOTICES_X83", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X85"), "We share personal information with joint marketing partners", false) == 0, JS.GetStr(loan, "NOTICES.X86"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X85"), "We do not engage in joint marketing", false) == 0, JS.GetStr(loan, "315") + " does not jointly market.", ""));
      nameValueCollection.Add("NOTICES_X85", str15);
      string str16 = JS.GetStr(loan, "NOTICES.X87");
      nameValueCollection.Add("NOTICES_X87", str16);
      return nameValueCollection;
    }
  }
}
