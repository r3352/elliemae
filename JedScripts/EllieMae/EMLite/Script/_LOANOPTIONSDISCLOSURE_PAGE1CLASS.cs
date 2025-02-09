// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._LOANOPTIONSDISCLOSURE_PAGE1CLASS
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
  public class _LOANOPTIONSDISCLOSURE_PAGE1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str3);
      string str4 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str4);
      string str5 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str5);
      string str6 = JS.GetStr(loan, "1612");
      nameValueCollection.Add("1612", str6);
      string str7 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str7);
      string str8 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str8);
      string str9 = JS.GetStr(loan, "3237");
      nameValueCollection.Add("3237", str9);
      string str10 = JS.GetStr(loan, "3238");
      nameValueCollection.Add("3238", str10);
      string str11 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "68"), "", false) != 0 | Operators.CompareString(JS.GetStr(loan, "69"), "", false) != 0, " and ") + JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("36_37_a", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "Fixed Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "GPM - Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "OtherAmortizationType"))));
      nameValueCollection.Add("608", str12);
      string str13 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str13);
      string str14 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_a", str14);
      return nameValueCollection;
    }
  }
}
