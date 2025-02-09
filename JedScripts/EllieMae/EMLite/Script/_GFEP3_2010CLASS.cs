// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFEP3_2010CLASS
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
  public class _GFEP3_2010CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("NEWHUD_X4_a", JS.GetStr(loan, "NEWHUD.X4"));
      nameValueCollection.Add("3_a", JS.GetStr(loan, "3"));
      nameValueCollection.Add("NEWHUD_X217", JS.GetStr(loan, "NEWHUD.X217"));
      nameValueCollection.Add("NEWHUD_X93_a", JS.GetStr(loan, "NEWHUD.X93"));
      nameValueCollection.Add("NEWHUD_X4_b", JS.GetStr(loan, "NEWHUD.X4"));
      nameValueCollection.Add("NEWHUD_X95", JS.GetStr(loan, "NEWHUD.X95"));
      nameValueCollection.Add("NEWHUD_X96", JS.GetStr(loan, "NEWHUD.X96"));
      nameValueCollection.Add("NEWHUD_X97", JS.GetStr(loan, "NEWHUD.X97"));
      nameValueCollection.Add("NEWHUD_X98", JS.GetStr(loan, "NEWHUD.X98"));
      nameValueCollection.Add("NEWHUD_X99", JS.GetStr(loan, "NEWHUD.X99"));
      nameValueCollection.Add("NEWHUD_X4_c", JS.GetStr(loan, "NEWHUD.X4"));
      nameValueCollection.Add("NEWHUD_X101", JS.GetStr(loan, "NEWHUD.X101"));
      nameValueCollection.Add("NEWHUD_X102", JS.GetStr(loan, "NEWHUD.X102"));
      nameValueCollection.Add("NEWHUD_X103", JS.GetStr(loan, "NEWHUD.X103"));
      nameValueCollection.Add("NEWHUD_X104", JS.GetStr(loan, "NEWHUD.X104"));
      nameValueCollection.Add("NEWHUD_X105", JS.GetStr(loan, "NEWHUD.X105"));
      nameValueCollection.Add("NEWHUD_X806", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X806"), "", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X302"), JS.GetStr(loan, "317")), JS.GetStr(loan, "NEWHUD.X806")), ""));
      nameValueCollection.Add("NEWHUD_X4_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "NEWHUD.X4"), "", false) != 0, "$ " + JS.GetStr(loan, "NEWHUD.X4"), ""));
      nameValueCollection.Add("1347", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1347"), "", false) != 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "1347"), "1", false) == 0, JS.GetStr(loan, "1347") + " Year", JS.GetStr(loan, "1347") + " Years"), ""));
      nameValueCollection.Add("3_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "3"), "", false) != 0, JS.GetStr(loan, "3") + " %", ""));
      nameValueCollection.Add("NEWHUD_X217_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "NEWHUD.X217"), "", false) != 0, "$ " + JS.GetStr(loan, "NEWHUD.X217"), ""));
      nameValueCollection.Add("432", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "432"), "", false) != 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "432"), "1", false) == 0, JS.GetStr(loan, "432") + " day", JS.GetStr(loan, "432") + " days"), ""));
      nameValueCollection.Add("NEWHUD_X5", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X5"), "Y", false) == 0, "Yes", "No"), ""));
      nameValueCollection.Add("NEWHUD_X6", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X6"), "Y", false) == 0, "Yes", "No"), ""));
      nameValueCollection.Add("NEWHUD_X8", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X8"), "Y", false) == 0, "Yes", "No"), ""));
      nameValueCollection.Add("675", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "Y", false) == 0, "Yes", "No"), ""));
      nameValueCollection.Add("1659", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "Yes", "No"), ""));
      nameValueCollection.Add("NEWHUD_X93_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X807"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "NEWHUD.X93"), "", false) != 0, "$ " + JS.GetStr(loan, "NEWHUD.X93"), ""));
      return nameValueCollection;
    }
  }
}
