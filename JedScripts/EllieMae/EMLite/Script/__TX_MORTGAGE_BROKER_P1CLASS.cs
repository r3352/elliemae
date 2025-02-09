// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__TX_MORTGAGE_BROKER_P1CLASS
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
  public class __TX_MORTGAGE_BROKER_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("VEND_X302", JS.GetStr(loan, "VEND.X302"));
      nameValueCollection.Add("VEND_X300", JS.GetStr(loan, "VEND.X300"));
      nameValueCollection.Add("DISCLOSURE_X74", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X74"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X75", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X75"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X76", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X76"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X79", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X79"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X80", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X80"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.x908"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X77", JS.GetStr(loan, "DISCLOSURE.X77"));
      nameValueCollection.Add("DISCLOSURE_X78", JS.GetStr(loan, "DISCLOSURE.X78"));
      nameValueCollection.Add("DISCLOSURE_X909_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X80"), "Y", false) == 0, "Current wholesale options available to us") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X80"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X908"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X909")));
      nameValueCollection.Add("DISCLOSURE_X909_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X80"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X908"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X909")) + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X908"), "Y", false) != 0, ""));
      return nameValueCollection;
    }
  }
}
