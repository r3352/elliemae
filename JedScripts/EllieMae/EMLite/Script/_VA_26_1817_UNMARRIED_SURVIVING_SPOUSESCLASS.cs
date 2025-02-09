// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_1817_UNMARRIED_SURVIVING_SPOUSESCLASS
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
  public class _VA_26_1817_UNMARRIED_SURVIVING_SPOUSESCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("VAELIG_X5", JS.GetStr(loan, "VAELIG.X5"));
      nameValueCollection.Add("VAELIG_X6", JS.GetStr(loan, "VAELIG.X6"));
      nameValueCollection.Add("VAELIG_X2_X3", JS.GetStr(loan, "VAELIG.X2") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X2"), "", false) != 0, " - ") + JS.GetStr(loan, "VAELIG.X3"));
      nameValueCollection.Add("VAVOB_X66", JS.GetStr(loan, "VAVOB.X66"));
      nameValueCollection.Add("VAVOB_X67", JS.GetStr(loan, "VAVOB.X67"));
      nameValueCollection.Add("FE0217", JS.GetStr(loan, "FE0217"));
      nameValueCollection.Add("1403", JS.GetStr(loan, "1403"));
      return nameValueCollection;
    }
  }
}
