// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_0551_DEBT_QUESTCLASS
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
  public class _VA_26_0551_DEBT_QUESTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("170_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1057"), "Y", false) == 0, "X"));
      nameValueCollection.Add("170_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1057"), "N", false) == 0, "X"));
      nameValueCollection.Add("463_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "463"), "Y", false) == 0, "X"));
      nameValueCollection.Add("463_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "463"), "N", false) == 0, "X"));
      return nameValueCollection;
    }
  }
}
