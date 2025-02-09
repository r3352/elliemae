// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__TX_MULTIPLE_ROLE_DISCLOSURECLASS
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
  public class __TX_MULTIPLE_ROLE_DISCLOSURECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("DISCLOSURE_X72", JS.GetStr(loan, "DISCLOSURE.X72"));
      nameValueCollection.Add("DISCLOSURE_X99", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X99"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X100", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X100"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X101", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X101"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X102", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X102"), "Y", false) == 0, "X"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("36_37_a", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69_a", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("DISCLOSURE_X95", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X95"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X96", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X96"), "Seller", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X97", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X96"), "Buyer", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X98", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X96"), "BuyerAndSeller", false) == 0, "X"));
      return nameValueCollection;
    }
  }
}
