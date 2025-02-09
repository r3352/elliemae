// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._PAYMENT_HISTORYCLASS
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
  public class _PAYMENT_HISTORYCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "SERVICE.X1");
      nameValueCollection.Add("SERVICE_X1", str1);
      string str2 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str2);
      string str3 = JS.GetStr(loan, "1257");
      nameValueCollection.Add("1257", str3);
      string str4 = JS.GetStr(loan, "1258") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1259"), "", false) != 0, ", ") + JS.GetStr(loan, "1259") + " " + JS.GetStr(loan, "1260");
      nameValueCollection.Add("1258_1259_1260", str4);
      string str5 = JS.GetStr(loan, "1262");
      nameValueCollection.Add("1262", str5);
      string str6 = JS.GetStr(loan, "1263");
      nameValueCollection.Add("1263", str6);
      string str7 = JS.GetStr(loan, "SERVICE.X8");
      nameValueCollection.Add("SERVICE_X8", str7);
      string str8 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str8);
      string str9 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str9);
      string str10 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11_11", str10);
      string str11 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str11);
      string str12 = JS.GetStr(loan, "SERVICE.X57");
      nameValueCollection.Add("SERVICE_X57", str12);
      string str13 = JS.GetStr(loan, "SERVICE.X81");
      nameValueCollection.Add("SERVICE_X81", str13);
      string str14 = JS.GetStr(loan, "SERVICE.X32");
      nameValueCollection.Add("SERVICE_X32", str14);
      string str15 = JS.GetStr(loan, "SERVICE.X14");
      nameValueCollection.Add("SERVICE_X14", str15);
      return nameValueCollection;
    }
  }
}
