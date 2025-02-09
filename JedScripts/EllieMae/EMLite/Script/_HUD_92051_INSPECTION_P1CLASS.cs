// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_92051_INSPECTION_P1CLASS
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
  public class _HUD_92051_INSPECTION_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "713");
      nameValueCollection.Add("713", str1);
      string str2 = JS.GetStr(loan, "715");
      nameValueCollection.Add("715", str2);
      string str3 = JS.GetStr(loan, "716") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1253"), "", false) != 0, ", ") + JS.GetStr(loan, "1253") + " " + JS.GetStr(loan, "717");
      nameValueCollection.Add("702_1249_703", str3);
      string str4 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str4);
      string str5 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str5);
      string str6 = JS.GetStr(loan, "1257");
      nameValueCollection.Add("1257", str6);
      string str7 = JS.GetStr(loan, "1258") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1259"), "", false) != 0, ", ") + JS.GetStr(loan, "1259") + " " + JS.GetStr(loan, "1260");
      nameValueCollection.Add("1258_1259_1260", str7);
      string str8 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str8);
      string str9 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str9);
      return nameValueCollection;
    }
  }
}
