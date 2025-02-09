// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._DEBT_CONSOLIDATIONCLASS
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
  public class _DEBT_CONSOLIDATIONCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("363", Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363")));
      nameValueCollection.Add("1401", JS.GetStr(loan, "1401"));
      nameValueCollection.Add("1109", JS.GetStr(loan, "1109"));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("4", JS.GetStr(loan, "4"));
      nameValueCollection.Add("799", JS.GetStr(loan, "799"));
      nameValueCollection.Add("5", JS.GetStr(loan, "5"));
      nameValueCollection.Add("PREQUALX245", Jed.NF(Jed.S2N(JS.GetStr(loan, "PREQUAL.X245")), (byte) 18, 0));
      nameValueCollection.Add("733", Jed.NF(Jed.S2N(JS.GetStr(loan, "733")), (byte) 18, 0));
      nameValueCollection.Add("PREQUALX248", Jed.NF(Jed.S2N(JS.GetStr(loan, "PREQUAL.X248")), (byte) 18, 0));
      return nameValueCollection;
    }
  }
}
