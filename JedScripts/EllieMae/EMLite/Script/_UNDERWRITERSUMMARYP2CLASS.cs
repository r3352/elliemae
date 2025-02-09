// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._UNDERWRITERSUMMARYP2CLASS
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
  public class _UNDERWRITERSUMMARYP2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("REGZGFE_X8", JS.GetStr(loan, "REGZGFE.X8"));
      nameValueCollection.Add("984", JS.GetStr(loan, "984"));
      nameValueCollection.Add("2298", JS.GetStr(loan, "2298"));
      nameValueCollection.Add("2299", JS.GetStr(loan, "2299"));
      nameValueCollection.Add("2300", JS.GetStr(loan, "2300"));
      nameValueCollection.Add("2301", JS.GetStr(loan, "2301"));
      nameValueCollection.Add("2302", JS.GetStr(loan, "2302"));
      nameValueCollection.Add("2303", JS.GetStr(loan, "2303"));
      nameValueCollection.Add("2304", JS.GetStr(loan, "2304"));
      nameValueCollection.Add("2305", JS.GetStr(loan, "2305"));
      nameValueCollection.Add("VEND_X177", JS.GetStr(loan, "VEND.X177"));
      nameValueCollection.Add("L248", JS.GetStr(loan, "L248"));
      nameValueCollection.Add("2308", JS.GetStr(loan, "2308"));
      nameValueCollection.Add("2309", JS.GetStr(loan, "2309"));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("2310", JS.GetStr(loan, "2310"));
      nameValueCollection.Add("356", JS.GetStr(loan, "356"));
      nameValueCollection.Add("1393", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1393"), "", false) == 0, "Active Loan", JS.GetStr(loan, "1393")));
      nameValueCollection.Add("749", JS.GetStr(loan, "749"));
      nameValueCollection.Add("2312", JS.GetStr(loan, "2312"));
      nameValueCollection.Add("1544", JS.GetStr(loan, "1544"));
      nameValueCollection.Add("2314", JS.GetStr(loan, "2314"));
      nameValueCollection.Add("2315", JS.GetStr(loan, "2315"));
      nameValueCollection.Add("2316", JS.GetStr(loan, "2316"));
      nameValueCollection.Add("2313", JS.GetStr(loan, "2313"));
      nameValueCollection.Add("2317", JS.GetStr(loan, "2317"));
      nameValueCollection.Add("2318", JS.GetStr(loan, "2318"));
      return nameValueCollection;
    }
  }
}
