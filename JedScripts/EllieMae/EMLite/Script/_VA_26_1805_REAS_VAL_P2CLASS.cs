// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_1805_REAS_VAL_P2CLASS
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
  public class _VA_26_1805_REAS_VAL_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str1);
      string str2 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str2);
      string str3 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str3);
      string str4 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str4);
      string str5 = JS.GetStr(loan, "765");
      nameValueCollection.Add("765", str5);
      string str6 = JS.GetStr(loan, "766");
      nameValueCollection.Add("766", str6);
      string str7 = JS.GetStr(loan, "797");
      nameValueCollection.Add("797", str7);
      string str8 = JS.GetStr(loan, "798");
      nameValueCollection.Add("798", str8);
      string str9 = JS.GetStr(loan, "926");
      nameValueCollection.Add("926", str9);
      string str10 = JS.GetStr(loan, "927");
      nameValueCollection.Add("927", str10);
      string str11 = JS.GetStr(loan, "928");
      nameValueCollection.Add("928", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "X");
      nameValueCollection.Add("1041_Condo", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "X");
      nameValueCollection.Add("1041_PUD", str13);
      string str14 = JS.GetStr(loan, "315");
      nameValueCollection.Add("GAPPRX1", str14);
      string str15 = JS.GetStr(loan, "362");
      nameValueCollection.Add("GAPPRX2", str15);
      string str16 = JS.GetStr(loan, "319");
      nameValueCollection.Add("GAPPRX3", str16);
      string str17 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("GAPPRX4", str17);
      string str18 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("CRV1S3", str18);
      string str19 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("CRV1S30", str19);
      string str20 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("CRVS4", str20);
      return nameValueCollection;
    }
  }
}
