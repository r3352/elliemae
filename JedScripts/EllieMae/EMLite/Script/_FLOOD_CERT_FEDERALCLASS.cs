// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FLOOD_CERT_FEDERALCLASS
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
  public class _FLOOD_CERT_FEDERALCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = "";
      nameValueCollection.Add("5850", str1);
      string str2 = JS.GetStr(loan, "315");
      nameValueCollection.Add("381", str2);
      string str3 = "";
      nameValueCollection.Add("5851", str3);
      string str4 = JS.GetStr(loan, "362");
      nameValueCollection.Add("18", str4);
      string str5 = JS.GetStr(loan, "326");
      nameValueCollection.Add("4011", str5);
      string str6 = JS.GetStr(loan, "324");
      nameValueCollection.Add("1509", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"), "");
      nameValueCollection.Add("1526", str7);
      string str8 = JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101", str8);
      string str9 = JS.GetStr(loan, "364");
      nameValueCollection.Add("1", str9);
      string str10 = JS.GetStr(loan, "11");
      nameValueCollection.Add("31", str10);
      string str11 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("32_33_34", str11);
      string str12 = JS.GetStr(loan, "13");
      nameValueCollection.Add("35", str12);
      string str13 = JS.GetStr(loan, "17");
      nameValueCollection.Add("1206", str13);
      string str14 = Jed.NF(Jed.Num(JS.GetNum(loan, "356")), (byte) 18, 0);
      nameValueCollection.Add("801", str14);
      string str15 = JS.GetStr(loan, "18");
      nameValueCollection.Add("37", str15);
      return nameValueCollection;
    }
  }
}
