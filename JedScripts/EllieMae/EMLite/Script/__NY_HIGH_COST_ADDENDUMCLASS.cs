// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__NY_HIGH_COST_ADDENDUMCLASS
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
  public class __NY_HIGH_COST_ADDENDUMCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str3);
      string str4 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str4);
      string str5 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str5);
      string str6 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str6);
      string str7 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str7);
      string str8 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1819"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1416"), "", false) == 0, JS.GetStr(loan, "FR0104"), JS.GetStr(loan, "1416")) + "\r\n" + Jed.BF((Operators.CompareString(JS.GetStr(loan, "1819"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1416"), "", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108"), JS.GetStr(loan, "1417") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1418"), "", false) != 0, ", ") + JS.GetStr(loan, "1418") + " " + JS.GetStr(loan, "1419"));
      nameValueCollection.Add("MAP1", str9);
      string str10 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("MAP2", str10);
      return nameValueCollection;
    }
  }
}
