// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._DIS___RESPA_SERVICINGCLASS
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
  public class _DIS___RESPA_SERVICINGCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("363", JS.GetStr(loan, "363"));
      nameValueCollection.Add("RESPAX1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RESPA.X1"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RESPAX6", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RESPA.X6"), "Y", false) == 0, "X"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("RESPAX28", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RESPA.X28"), "Y", false) == 0, "X"));
      return nameValueCollection;
    }
  }
}
