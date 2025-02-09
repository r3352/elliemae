// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_8923_RATE_REDUCTION_WORKSHEETCLASS
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
  public class _VA_26_8923_RATE_REDUCTION_WORKSHEETCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("26", JS.GetStr(loan, "26"));
      nameValueCollection.Add("VARRRWSX1", JS.GetStr(loan, "VARRRWS.X1"));
      nameValueCollection.Add("VARRRWSX2_1", JS.GetStr(loan, "VARRRWS.X2"));
      nameValueCollection.Add("VARRRWSX2_2", JS.GetStr(loan, "VARRRWS.X2"));
      nameValueCollection.Add("1010_1", JS.GetStr(loan, "VARRRWS.X9"));
      nameValueCollection.Add("1010_2", JS.GetStr(loan, "VARRRWS.X9"));
      nameValueCollection.Add("388", JS.GetStr(loan, "388"));
      nameValueCollection.Add("1107_1", JS.GetStr(loan, "1107"));
      nameValueCollection.Add("1107_2", JS.GetStr(loan, "1107"));
      nameValueCollection.Add("VARRRWSX3_1", JS.GetStr(loan, "VARRRWS.X3"));
      nameValueCollection.Add("VARRRWSX3_2", JS.GetStr(loan, "VARRRWS.X3"));
      nameValueCollection.Add("VARRRWSX4", JS.GetStr(loan, "VARRRWS.X4"));
      nameValueCollection.Add("VARRRWSX5_1", JS.GetStr(loan, "VARRRWS.X5"));
      nameValueCollection.Add("VARRRWSX5_2", JS.GetStr(loan, "VARRRWS.X5"));
      nameValueCollection.Add("VARRRWSX6", JS.GetStr(loan, "VARRRWS.X6"));
      nameValueCollection.Add("VARRRWSX7_1", JS.GetStr(loan, "VARRRWS.X7"));
      nameValueCollection.Add("VARRRWSX7_2", JS.GetStr(loan, "VARRRWS.X7"));
      nameValueCollection.Add("VARRRWSX8", JS.GetStr(loan, "VARRRWS.X8"));
      nameValueCollection.Add("VARRRWSX9", Jed.NF(Jed.S2N(JS.GetStr(loan, "VARRRWS.X8")) + Jed.S2N(JS.GetStr(loan, "VARRRWS.X7")), (byte) 18, 0));
      nameValueCollection.Add("VARRRWSX10", Jed.NF(Jed.S2N(JS.GetStr(loan, "VARRRWS.X8")) + Jed.S2N(JS.GetStr(loan, "VARRRWS.X7")) - Jed.S2N(JS.GetStr(loan, "VARRRWS.X3")), (byte) 18, 0));
      nameValueCollection.Add("VARRRWSX11", JS.GetStr(loan, "VARRRWS.X11"));
      nameValueCollection.Add("VARRRWSX12", Jed.NF(Jed.S2N(JS.GetStr(loan, "VARRRWS.X8")) + Jed.S2N(JS.GetStr(loan, "VARRRWS.X7")) - Jed.S2N(JS.GetStr(loan, "VARRRWS.X3")) - Jed.S2N(JS.GetStr(loan, "VARRRWS.X5")), (byte) 18, 0));
      nameValueCollection.Add("VARRRWSX13", JS.GetStr(loan, "VARRRWS.X13"));
      nameValueCollection.Add("363", Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363")));
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      return nameValueCollection;
    }
  }
}
