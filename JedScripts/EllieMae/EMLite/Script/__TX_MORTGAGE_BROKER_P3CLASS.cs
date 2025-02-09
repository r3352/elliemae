// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__TX_MORTGAGE_BROKER_P3CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class __TX_MORTGAGE_BROKER_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("DISCLOSURE_X72", JS.GetStr(loan, "DISCLOSURE.X72"));
      nameValueCollection.Add("DISCLOSURE_X73", JS.GetStr(loan, "DISCLOSURE.X73"));
      nameValueCollection.Add("DISCLOSURE_X72_a", JS.GetStr(loan, "DISCLOSURE.X72"));
      nameValueCollection.Add("DISCLOSURE_X72_b", JS.GetStr(loan, "DISCLOSURE.X72"));
      nameValueCollection.Add("36_37_a", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69_a", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("DISCLOSURE_X72_c", JS.GetStr(loan, "DISCLOSURE.X72"));
      return nameValueCollection;
    }
  }
}
