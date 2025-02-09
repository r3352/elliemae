// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._LOANOPTIONSDISCLOSURE_PAGE2CLASS
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
  public class _LOANOPTIONSDISCLOSURE_PAGE2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("DISCLOSUREX688", JS.GetStr(loan, "DISCLOSURE.X688"));
      nameValueCollection.Add("DISCLOSUREX689", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "Fixed", false) == 0, "Fixed Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "GraduatedPaymentMortgage", false) == 0, "GPM - Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "AdjustableRate", false) == 0, "ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "OtherAmortizationType", false) == 0, "OtherAmortizationType")))));
      nameValueCollection.Add("DISCLOSUREX690", JS.GetStr(loan, "DISCLOSURE.X690") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X690"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX691", JS.GetStr(loan, "DISCLOSURE.X691") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X691"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX692", JS.GetStr(loan, "DISCLOSURE.X692") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X692"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX693", JS.GetStr(loan, "DISCLOSURE.X693") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X693"), "", false) != 0, "%"));
      nameValueCollection.Add("DISCLOSUREX694", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X694"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X694"));
      nameValueCollection.Add("DISCLOSUREX978", JS.GetStr(loan, "DISCLOSURE.X978") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X978"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX979", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X979"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X979"));
      nameValueCollection.Add("DISCLOSUREX980", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X980"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X980"));
      nameValueCollection.Add("DISCLOSUREX696", JS.GetStr(loan, "DISCLOSURE.X696"));
      nameValueCollection.Add("DISCLOSUREX697", JS.GetStr(loan, "DISCLOSURE.X697"));
      nameValueCollection.Add("DISCLOSUREX698", JS.GetStr(loan, "DISCLOSURE.X698"));
      nameValueCollection.Add("DISCLOSUREX699", JS.GetStr(loan, "DISCLOSURE.X699"));
      nameValueCollection.Add("DISCLOSUREX700", JS.GetStr(loan, "DISCLOSURE.X700"));
      nameValueCollection.Add("DISCLOSUREX701", JS.GetStr(loan, "DISCLOSURE.X701"));
      nameValueCollection.Add("DISCLOSUREX689_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "Fixed", false) == 0, "Fixed Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "GraduatedPaymentMortgage", false) == 0, "GPM - Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "AdjustableRate", false) == 0, "ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "OtherAmortizationType", false) == 0, "OtherAmortizationType")))));
      nameValueCollection.Add("DISCLOSUREX702", JS.GetStr(loan, "DISCLOSURE.X702") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X702"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX703", JS.GetStr(loan, "DISCLOSURE.X703") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X703"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX704", JS.GetStr(loan, "DISCLOSURE.X704") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X704"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX705", JS.GetStr(loan, "DISCLOSURE.X705") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X705"), "", false) != 0, "%"));
      nameValueCollection.Add("DISCLOSUREX706", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X706"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X706"));
      nameValueCollection.Add("DISCLOSUREX982", JS.GetStr(loan, "DISCLOSURE.X982") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X982"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX983", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X983"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X983"));
      nameValueCollection.Add("DISCLOSUREX984", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X984"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X984"));
      nameValueCollection.Add("DISCLOSUREX708", JS.GetStr(loan, "DISCLOSURE.X708"));
      nameValueCollection.Add("DISCLOSUREX689_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "Fixed", false) == 0, "Fixed Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "GraduatedPaymentMortgage", false) == 0, "GPM - Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "AdjustableRate", false) == 0, "ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "OtherAmortizationType", false) == 0, "OtherAmortizationType")))));
      nameValueCollection.Add("DISCLOSUREX709", JS.GetStr(loan, "DISCLOSURE.X709") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X709"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX710", JS.GetStr(loan, "DISCLOSURE.X710") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X710"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX711", JS.GetStr(loan, "DISCLOSURE.X711") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X711"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX712", JS.GetStr(loan, "DISCLOSURE.X712") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X712"), "", false) != 0, "%"));
      nameValueCollection.Add("DISCLOSUREX713", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X713"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X713"));
      nameValueCollection.Add("DISCLOSUREX986", JS.GetStr(loan, "DISCLOSURE.X986") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X986"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX987", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X987"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X987"));
      nameValueCollection.Add("DISCLOSUREX988", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X988"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X988"));
      nameValueCollection.Add("DISCLOSUREX715", JS.GetStr(loan, "DISCLOSURE.X715"));
      nameValueCollection.Add("DISCLOSUREX716", JS.GetStr(loan, "DISCLOSURE.X716"));
      nameValueCollection.Add("DISCLOSUREX717", JS.GetStr(loan, "DISCLOSURE.X717"));
      nameValueCollection.Add("DISCLOSUREX718", JS.GetStr(loan, "DISCLOSURE.X718"));
      nameValueCollection.Add("DISCLOSUREX719", JS.GetStr(loan, "DISCLOSURE.X719"));
      nameValueCollection.Add("DISCLOSUREX720", JS.GetStr(loan, "DISCLOSURE.X720"));
      nameValueCollection.Add("DISCLOSUREX689_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "Fixed", false) == 0, "Fixed Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "GraduatedPaymentMortgage", false) == 0, "GPM - Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "AdjustableRate", false) == 0, "ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X689"), "OtherAmortizationType", false) == 0, "OtherAmortizationType")))));
      nameValueCollection.Add("DISCLOSUREX721", JS.GetStr(loan, "DISCLOSURE.X721") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X721"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX722", JS.GetStr(loan, "DISCLOSURE.X722") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X722"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX723", JS.GetStr(loan, "DISCLOSURE.X723") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X723"), "", false) != 0, " months"));
      nameValueCollection.Add("DISCLOSUREX724", JS.GetStr(loan, "DISCLOSURE.X724") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X724"), "", false) != 0, "%"));
      nameValueCollection.Add("DISCLOSUREX725", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X725"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X725"));
      nameValueCollection.Add("DISCLOSUREX990", JS.GetStr(loan, "DISCLOSURE.X990") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X990"), "", false) != 0, " %"));
      nameValueCollection.Add("DISCLOSUREX991", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X991"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X991"));
      nameValueCollection.Add("DISCLOSUREX992", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X992"), "", false) != 0, "$") + JS.GetStr(loan, "DISCLOSURE.X992"));
      nameValueCollection.Add("DISCLOSUREX727", JS.GetStr(loan, "DISCLOSURE.X727"));
      nameValueCollection.Add("DISCLOSUREX728", JS.GetStr(loan, "DISCLOSURE.X728"));
      nameValueCollection.Add("DISCLOSUREX729", JS.GetStr(loan, "DISCLOSURE.X729"));
      nameValueCollection.Add("DISCLOSUREX730", JS.GetStr(loan, "DISCLOSURE.X730"));
      nameValueCollection.Add("DISCLOSUREX731", JS.GetStr(loan, "DISCLOSURE.X731"));
      nameValueCollection.Add("DISCLOSUREX732_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X732"), "Loan Option 1", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSUREX732_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X732"), "Loan Option 2", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSUREX732_3", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X732"), "Loan Option 3", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSUREX732_4", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X732"), "Loan Option 4", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSUREX732_Other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X732"), "Other Option", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSUREX733", JS.GetStr(loan, "DISCLOSURE.X733"));
      nameValueCollection.Add("DISCLOSUREX734", JS.GetStr(loan, "DISCLOSURE.X734"));
      return nameValueCollection;
    }
  }
}
