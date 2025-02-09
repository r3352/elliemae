// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FLOOD_HAZARD_NOTICECLASS
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
  public class _FLOOD_HAZARD_NOTICECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str1);
      string str2 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str2);
      string str3 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str3);
      string str4 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str4);
      string str5 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str5);
      string str6 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str6);
      string str7 = JS.GetStr(loan, "1416");
      nameValueCollection.Add("1416", str7);
      string str8 = JS.GetStr(loan, "1417") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1418"), "", false) != 0, ", ") + JS.GetStr(loan, "1418") + " " + JS.GetStr(loan, "1419");
      nameValueCollection.Add("1417_1418_1419", str8);
      string str9 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str9);
      string str10 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str10);
      string str11 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X48"), "Y", false) == 0, "X");
      nameValueCollection.Add("NOTICES_X48", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X49"), "Y", false) == 0, "X");
      nameValueCollection.Add("NOTICES_X49", str13);
      string str14 = JS.GetStr(loan, "NOTICES.X50");
      nameValueCollection.Add("NOTICES_X50", str14);
      return nameValueCollection;
    }
  }
}
