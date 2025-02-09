// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1084A_CASH_ANALYSISCLASS
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
  public class _1084A_CASH_ANALYSISCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("FM1084X1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X1"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X2"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X3", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X3"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X4", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X4"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X5", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X5"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X6", JS.GetStr(loan, "FM1084.X6"));
      nameValueCollection.Add("FM1084X7", JS.GetStr(loan, "FM1084.X7"));
      nameValueCollection.Add("FM1084X8", JS.GetStr(loan, "FM1084.X8"));
      nameValueCollection.Add("FM1084X9", JS.GetStr(loan, "FM1084.X9"));
      nameValueCollection.Add("FM1084X10", JS.GetStr(loan, "FM1084.X10"));
      nameValueCollection.Add("FM1084X11", JS.GetStr(loan, "FM1084.X11"));
      nameValueCollection.Add("FM1084X12", JS.GetStr(loan, "FM1084.X12"));
      nameValueCollection.Add("FM1084X13", JS.GetStr(loan, "FM1084.X13"));
      nameValueCollection.Add("FM1084X14", JS.GetStr(loan, "FM1084.X14"));
      nameValueCollection.Add("FM1084X15", JS.GetStr(loan, "FM1084.X15"));
      nameValueCollection.Add("FM1084X16", JS.GetStr(loan, "FM1084.X16"));
      nameValueCollection.Add("FM1084X17", JS.GetStr(loan, "FM1084.X17"));
      nameValueCollection.Add("FM1084X18", JS.GetStr(loan, "FM1084.X18"));
      nameValueCollection.Add("FM1084X19", JS.GetStr(loan, "FM1084.X19"));
      nameValueCollection.Add("FM1084X50", JS.GetStr(loan, "FM1084.X50"));
      nameValueCollection.Add("FM1084X51", JS.GetStr(loan, "FM1084.X51"));
      nameValueCollection.Add("FM1084X52", JS.GetStr(loan, "FM1084.X52"));
      nameValueCollection.Add("FM1084X53", JS.GetStr(loan, "FM1084.X53"));
      nameValueCollection.Add("FM1084X54", JS.GetStr(loan, "FM1084.X54"));
      nameValueCollection.Add("FM1084X55", JS.GetStr(loan, "FM1084.X55"));
      nameValueCollection.Add("FM1084X56", JS.GetStr(loan, "FM1084.X56"));
      nameValueCollection.Add("FM1084X57", JS.GetStr(loan, "FM1084.X57"));
      nameValueCollection.Add("FM1084X58", JS.GetStr(loan, "FM1084.X58"));
      nameValueCollection.Add("FM1084X59", JS.GetStr(loan, "FM1084.X59"));
      nameValueCollection.Add("FM1084X60", JS.GetStr(loan, "FM1084.X60"));
      nameValueCollection.Add("FM1084X61", JS.GetStr(loan, "FM1084.X61"));
      nameValueCollection.Add("FM1084X62", JS.GetStr(loan, "FM1084.X62"));
      nameValueCollection.Add("FM1084X20", JS.GetStr(loan, "FM1084.X20"));
      nameValueCollection.Add("FM1084X63", JS.GetStr(loan, "FM1084.X63"));
      nameValueCollection.Add("FM1084X21", JS.GetStr(loan, "FM1084.X21"));
      nameValueCollection.Add("FM1084X22", JS.GetStr(loan, "FM1084.X22"));
      nameValueCollection.Add("FM1084X64", JS.GetStr(loan, "FM1084.X64"));
      nameValueCollection.Add("FM1084X65", JS.GetStr(loan, "FM1084.X65"));
      nameValueCollection.Add("FM1084X23", JS.GetStr(loan, "FM1084.X23"));
      nameValueCollection.Add("FM1084X24", JS.GetStr(loan, "FM1084.X24"));
      nameValueCollection.Add("FM1084X66", JS.GetStr(loan, "FM1084.X66"));
      nameValueCollection.Add("FM1084X67", JS.GetStr(loan, "FM1084.X67"));
      nameValueCollection.Add("FM1084X25", JS.GetStr(loan, "FM1084.X25"));
      nameValueCollection.Add("FM1084X26", JS.GetStr(loan, "FM1084.X26"));
      nameValueCollection.Add("FM1084X27", JS.GetStr(loan, "FM1084.X27"));
      nameValueCollection.Add("FM1084X28", JS.GetStr(loan, "FM1084.X28"));
      nameValueCollection.Add("FM1084X29", JS.GetStr(loan, "FM1084.X29"));
      nameValueCollection.Add("FM1084X30", JS.GetStr(loan, "FM1084.X30"));
      nameValueCollection.Add("FM1084X68", JS.GetStr(loan, "FM1084.X68"));
      nameValueCollection.Add("FM1084X69", JS.GetStr(loan, "FM1084.X69"));
      nameValueCollection.Add("FM1084X70", JS.GetStr(loan, "FM1084.X70"));
      nameValueCollection.Add("FM1084X71", JS.GetStr(loan, "FM1084.X71"));
      nameValueCollection.Add("FM1084X72", JS.GetStr(loan, "FM1084.X72"));
      nameValueCollection.Add("FM1084X73", JS.GetStr(loan, "FM1084.X73"));
      nameValueCollection.Add("FM1084X31", JS.GetStr(loan, "FM1084.X31"));
      nameValueCollection.Add("FM1084X74", JS.GetStr(loan, "FM1084.X74"));
      nameValueCollection.Add("FM1084X32", JS.GetStr(loan, "FM1084.X32"));
      nameValueCollection.Add("FM1084X75", JS.GetStr(loan, "FM1084.X75"));
      nameValueCollection.Add("FM1084X33", JS.GetStr(loan, "FM1084.X33"));
      nameValueCollection.Add("FM1084X76", JS.GetStr(loan, "FM1084.X76"));
      nameValueCollection.Add("FM1084X34", JS.GetStr(loan, "FM1084.X34"));
      nameValueCollection.Add("FM1084X35", JS.GetStr(loan, "FM1084.X35"));
      nameValueCollection.Add("FM1084X36", JS.GetStr(loan, "FM1084.X36"));
      nameValueCollection.Add("FM1084X37", JS.GetStr(loan, "FM1084.X37"));
      nameValueCollection.Add("FM1084X77", JS.GetStr(loan, "FM1084.X77"));
      nameValueCollection.Add("FM1084X78", JS.GetStr(loan, "FM1084.X78"));
      nameValueCollection.Add("FM1084X79", JS.GetStr(loan, "FM1084.X79"));
      nameValueCollection.Add("FM1084X80", JS.GetStr(loan, "FM1084.X80"));
      nameValueCollection.Add("FM1084X38", JS.GetStr(loan, "FM1084.X38"));
      nameValueCollection.Add("FM1084X39", JS.GetStr(loan, "FM1084.X39"));
      nameValueCollection.Add("FM1084X40", JS.GetStr(loan, "FM1084.X40"));
      nameValueCollection.Add("FM1084X41", JS.GetStr(loan, "FM1084.X41"));
      nameValueCollection.Add("FM1084X42", JS.GetStr(loan, "FM1084.X42"));
      nameValueCollection.Add("FM1084X43", JS.GetStr(loan, "FM1084.X43"));
      nameValueCollection.Add("FM1084X81", JS.GetStr(loan, "FM1084.X81"));
      nameValueCollection.Add("FM1084X82", JS.GetStr(loan, "FM1084.X82"));
      nameValueCollection.Add("FM1084X83", JS.GetStr(loan, "FM1084.X83"));
      nameValueCollection.Add("FM1084X84", JS.GetStr(loan, "FM1084.X84"));
      nameValueCollection.Add("FM1084X85", JS.GetStr(loan, "FM1084.X85"));
      nameValueCollection.Add("FM1084X86", JS.GetStr(loan, "FM1084.X86"));
      nameValueCollection.Add("FM1084X44", JS.GetStr(loan, "FM1084.X44"));
      nameValueCollection.Add("FM1084X45", JS.GetStr(loan, "FM1084.X45"));
      nameValueCollection.Add("FM1084X46", JS.GetStr(loan, "FM1084.X46"));
      nameValueCollection.Add("FM1084X87", JS.GetStr(loan, "FM1084.X87"));
      nameValueCollection.Add("FM1084X88", JS.GetStr(loan, "FM1084.X88"));
      nameValueCollection.Add("FM1084X89", JS.GetStr(loan, "FM1084.X89"));
      nameValueCollection.Add("FM1084X47", JS.GetStr(loan, "FM1084.X47"));
      nameValueCollection.Add("FM1084X48", JS.GetStr(loan, "FM1084.X48"));
      nameValueCollection.Add("FM1084X49", JS.GetStr(loan, "FM1084.X49"));
      nameValueCollection.Add("FM1084X90", JS.GetStr(loan, "FM1084.X90"));
      nameValueCollection.Add("FM1084X91", JS.GetStr(loan, "FM1084.X91"));
      nameValueCollection.Add("FM1084X92", JS.GetStr(loan, "FM1084.X92"));
      return nameValueCollection;
    }
  }
}
