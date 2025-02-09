// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HOMEOWNERSHIP_COUNSELING_ORGANIZATION_P1CLASS
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
  public class _HOMEOWNERSHIP_COUNSELING_ORGANIZATION_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str2);
      string str3 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str3);
      string str4 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1264"), "", false) != 0, JS.GetStr(loan, "1264"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X293"), "", false) != 0, JS.GetStr(loan, "VEND.X293"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "315"), "", false) != 0, JS.GetStr(loan, "315"), "")));
      nameValueCollection.Add("315", str5);
      string str6 = JS.GetStr(loan, "HOC.X19");
      nameValueCollection.Add("HOC_X19", str6);
      return nameValueCollection;
    }
  }
}
