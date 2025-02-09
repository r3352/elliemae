// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FACT_ACT_DISCLOSURE_P1CLASS
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
  public class _FACT_ACT_DISCLOSURE_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_1", str1);
      string str2 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str2);
      string str3 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str3);
      string str4 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str4);
      string str5 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("MAP4", str5);
      string str6 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str6);
      string str7 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str7);
      string str8 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("MAP5", str8);
      string str9 = JS.GetStr(loan, "DISCLOSURE.X1");
      nameValueCollection.Add("DISCLOSURE_X1", str9);
      string str10 = JS.GetStr(loan, "DISCLOSURE.X2");
      nameValueCollection.Add("DISCLOSURE_X2", str10);
      string str11 = JS.GetStr(loan, "DISCLOSURE.X3") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X4"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X4") + " " + JS.GetStr(loan, "DISCLOSURE.X5");
      nameValueCollection.Add("MAP1", str11);
      string str12 = JS.GetStr(loan, "DISCLOSURE.X6");
      nameValueCollection.Add("DISCLOSURE_X6", str12);
      string str13 = JS.GetStr(loan, "DISCLOSURE.X7");
      nameValueCollection.Add("DISCLOSURE_X7", str13);
      string str14 = JS.GetStr(loan, "DISCLOSURE.X8");
      nameValueCollection.Add("DISCLOSURE_X8", str14);
      string str15 = JS.GetStr(loan, "DISCLOSURE.X9");
      nameValueCollection.Add("DISCLOSURE_X9", str15);
      string str16 = JS.GetStr(loan, "DISCLOSURE.X10");
      nameValueCollection.Add("DISCLOSURE_X10", str16);
      string str17 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_1", str17);
      string str18 = JS.GetStr(loan, "67");
      nameValueCollection.Add("67", str18);
      string str19 = JS.GetStr(loan, "DISCLOSURE.X11");
      nameValueCollection.Add("DISCLOSURE_X11", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X173"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X173", str20);
      string str21 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_1", str21);
      string str22 = JS.GetStr(loan, "60");
      nameValueCollection.Add("60", str22);
      string str23 = JS.GetStr(loan, "DISCLOSURE.X12");
      nameValueCollection.Add("DISCLOSURE_X12", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X176"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X176", str24);
      return nameValueCollection;
    }
  }
}
