// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_54114_DIRECT_ENDCLASS
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
  public class _HUD_54114_DIRECT_ENDCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str2);
      string str3 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str3);
      string str4 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str4);
      string str5 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str5);
      string str6 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str6);
      string str7 = JS.GetStr(loan, "618");
      nameValueCollection.Add("618", str7);
      string str8 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str8);
      string str9 = JS.GetStr(loan, "984");
      nameValueCollection.Add("984", str9);
      string str10 = JS.GetStr(loan, "980");
      nameValueCollection.Add("980", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("1038", str11);
      return nameValueCollection;
    }
  }
}
