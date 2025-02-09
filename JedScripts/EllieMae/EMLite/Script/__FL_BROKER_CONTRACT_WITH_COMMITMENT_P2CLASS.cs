// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__FL_BROKER_CONTRACT_WITH_COMMITMENT_P2CLASS
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
  public class __FL_BROKER_CONTRACT_WITH_COMMITMENT_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("FLGFE_X72", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLGFE.X72"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FLGFE_X73", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLGFE.X73"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FLGFE_X74", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLGFE.X74"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FLGFE_X75", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLGFE.X75"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FLGFE_X76", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLGFE.X76"), "Y", false) == 0, "X"));
      nameValueCollection.Add("DISCLOSURE_X171", JS.GetStr(loan, "DISCLOSURE.X171"));
      nameValueCollection.Add("317", JS.GetStr(loan, "317"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      return nameValueCollection;
    }
  }
}
