// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_92700_A_203K_BOR_ACKCLASS
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
  public class _HUD_92700_A_203K_BOR_ACKCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("MAX23K_X73_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X73"), "PayNetInterest", false) == 0, "X"));
      nameValueCollection.Add("MAX23K_X73_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X73"), "ApplyNetInterest", false) == 0, "X"));
      nameValueCollection.Add("MAX23K_X74_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X73"), "Other", false) == 0, "X"));
      nameValueCollection.Add("MAX23K_X74_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X73"), "Other", false) == 0, JS.GetStr(loan, "MAX23K.X74")));
      return nameValueCollection;
    }
  }
}
