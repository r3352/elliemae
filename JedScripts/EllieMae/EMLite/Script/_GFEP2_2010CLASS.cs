// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFEP2_2010CLASS
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
  public class _GFEP2_2010CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("NEWHUD_X12", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X12"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X12")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X14_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X14"), "Origination Charge", false) == 0, "X"));
      nameValueCollection.Add("NEWHUD_X14_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X14"), "Settlement Reduced", false) == 0, "X"));
      nameValueCollection.Add("NEWHUD_X14_3", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X14"), "Settlement Increased", false) == 0, "X"));
      nameValueCollection.Add("3a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X14"), "Origination Charge", false) == 0, JS.GetStr(loan, "3")));
      nameValueCollection.Add("3b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X14"), "Settlement Reduced", false) == 0, JS.GetStr(loan, "3")));
      nameValueCollection.Add("3c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X14"), "Settlement Increased", false) == 0, JS.GetStr(loan, "3")));
      nameValueCollection.Add("NEWHUD_X13", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X13"), "", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X12"), "0.00", false) == 0 | Operators.CompareString(JS.GetStr(loan, "NEWHUD.X12"), "", false) == 0, "", "0.00"), Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X13")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X721", JS.GetStr(loan, "NEWHUD.X721"));
      nameValueCollection.Add("NEWHUD_X722", JS.GetStr(loan, "NEWHUD.X722"));
      nameValueCollection.Add("NEWHUD_X16", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X16"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X16")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X17", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X17"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X17")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X38", JS.GetStr(loan, "NEWHUD.X38"));
      nameValueCollection.Add("NEWHUD_X401", JS.GetStr(loan, "NEWHUD.X401"));
      nameValueCollection.Add("NEWHUD_X402", JS.GetStr(loan, "NEWHUD.X402"));
      nameValueCollection.Add("NEWHUD_X403", JS.GetStr(loan, "NEWHUD.X403"));
      nameValueCollection.Add("NEWHUD_X404", JS.GetStr(loan, "NEWHUD.X404"));
      nameValueCollection.Add("NEWHUD_X405", JS.GetStr(loan, "NEWHUD.X405"));
      nameValueCollection.Add("NEWHUD_X406", JS.GetStr(loan, "NEWHUD.X406"));
      nameValueCollection.Add("NEWHUD_X407", JS.GetStr(loan, "NEWHUD.X407"));
      nameValueCollection.Add("NEWHUD_X408", JS.GetStr(loan, "NEWHUD.X408"));
      nameValueCollection.Add("NEWHUD_X409", JS.GetStr(loan, "NEWHUD.X409"));
      nameValueCollection.Add("NEWHUD_X410", JS.GetStr(loan, "NEWHUD.X410"));
      nameValueCollection.Add("NEWHUD_X411", JS.GetStr(loan, "NEWHUD.X411"));
      nameValueCollection.Add("NEWHUD_X412", JS.GetStr(loan, "NEWHUD.X412"));
      nameValueCollection.Add("NEWHUD_X413", JS.GetStr(loan, "NEWHUD.X413"));
      nameValueCollection.Add("NEWHUD_X414", JS.GetStr(loan, "NEWHUD.X414"));
      nameValueCollection.Add("NEWHUD_X559", JS.GetStr(loan, "NEWHUD.X559"));
      nameValueCollection.Add("NEWHUD_X560", JS.GetStr(loan, "NEWHUD.X560"));
      nameValueCollection.Add("NEWHUD_X561", JS.GetStr(loan, "NEWHUD.X561"));
      nameValueCollection.Add("NEWHUD_X562", JS.GetStr(loan, "NEWHUD.X562"));
      nameValueCollection.Add("NEWHUD_X642", JS.GetStr(loan, "NEWHUD.X642"));
      nameValueCollection.Add("NEWHUD_X643", JS.GetStr(loan, "NEWHUD.X643"));
      nameValueCollection.Add("NEWHUD_X39", JS.GetStr(loan, "NEWHUD.X39"));
      nameValueCollection.Add("NEWHUD_X40", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X40"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X40")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X41", JS.GetStr(loan, "NEWHUD.X41"));
      nameValueCollection.Add("NEWHUD_X42", JS.GetStr(loan, "NEWHUD.X42"));
      nameValueCollection.Add("NEWHUD_X43", JS.GetStr(loan, "NEWHUD.X43"));
      nameValueCollection.Add("NEWHUD_X44", JS.GetStr(loan, "NEWHUD.X44"));
      nameValueCollection.Add("NEWHUD_X45", JS.GetStr(loan, "NEWHUD.X45"));
      nameValueCollection.Add("NEWHUD_X46", JS.GetStr(loan, "NEWHUD.X46"));
      nameValueCollection.Add("NEWHUD_X47", JS.GetStr(loan, "NEWHUD.X47"));
      nameValueCollection.Add("NEWHUD_X48", JS.GetStr(loan, "NEWHUD.X48"));
      nameValueCollection.Add("NEWHUD_X49", JS.GetStr(loan, "NEWHUD.X49"));
      nameValueCollection.Add("NEWHUD_X50", JS.GetStr(loan, "NEWHUD.X50"));
      nameValueCollection.Add("NEWHUD_X51", JS.GetStr(loan, "NEWHUD.X51"));
      nameValueCollection.Add("NEWHUD_X52", JS.GetStr(loan, "NEWHUD.X52"));
      nameValueCollection.Add("NEWHUD_X53", JS.GetStr(loan, "NEWHUD.X53"));
      nameValueCollection.Add("NEWHUD_X54", JS.GetStr(loan, "NEWHUD.X54"));
      nameValueCollection.Add("NEWHUD_X55", JS.GetStr(loan, "NEWHUD.X55"));
      nameValueCollection.Add("NEWHUD_X56", JS.GetStr(loan, "NEWHUD.X56"));
      nameValueCollection.Add("NEWHUD_X214", JS.GetStr(loan, "NEWHUD.X214"));
      nameValueCollection.Add("NEWHUD_X76", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X76"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X76")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X691", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X691"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X691")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X349", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X349"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NEWHUD_X350", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X350"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NEWHUD_X351", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X351"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NEWHUD_X78", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X351"), "Y", false) == 0, JS.GetStr(loan, "NEWHUD.X78"), ""));
      nameValueCollection.Add("NEWHUD_X701", JS.GetStr(loan, "NEWHUD.X701"));
      nameValueCollection.Add("333", JS.GetStr(loan, "333"));
      nameValueCollection.Add("332", JS.GetStr(loan, "332"));
      nameValueCollection.Add("L244", JS.GetStr(loan, "L244"));
      nameValueCollection.Add("NEWHUD_X79", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X79"), "", false) == 0, "0.00", Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X79")), (byte) 18, 0)));
      nameValueCollection.Add("NEWHUD_X80", JS.GetStr(loan, "NEWHUD.X80"));
      nameValueCollection.Add("NEWHUD_X81", JS.GetStr(loan, "NEWHUD.X81"));
      nameValueCollection.Add("NEWHUD_X82", JS.GetStr(loan, "NEWHUD.X82"));
      nameValueCollection.Add("NEWHUD_X83", JS.GetStr(loan, "NEWHUD.X83"));
      nameValueCollection.Add("NEWHUD_X84", JS.GetStr(loan, "NEWHUD.X84"));
      nameValueCollection.Add("NEWHUD_X85", JS.GetStr(loan, "NEWHUD.X85"));
      nameValueCollection.Add("NEWHUD_X86", JS.GetStr(loan, "NEWHUD.X86"));
      nameValueCollection.Add("NEWHUD_X87", JS.GetStr(loan, "NEWHUD.X87"));
      nameValueCollection.Add("NEWHUD_X88", JS.GetStr(loan, "NEWHUD.X88"));
      nameValueCollection.Add("NEWHUD_X89", JS.GetStr(loan, "NEWHUD.X89"));
      nameValueCollection.Add("NEWHUD_X90", JS.GetStr(loan, "NEWHUD.X90"));
      nameValueCollection.Add("NEWHUD_X91", JS.GetStr(loan, "NEWHUD.X91"));
      nameValueCollection.Add("NEWHUD_X653", JS.GetStr(loan, "NEWHUD.X653"));
      nameValueCollection.Add("NEWHUD_X654", JS.GetStr(loan, "NEWHUD.X654"));
      nameValueCollection.Add("NEWHUD_X92", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X92")) == 0.0, "0.00", JS.GetStr(loan, "NEWHUD.X92")));
      nameValueCollection.Add("NEWHUD_X93", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X93")) == 0.0, "0.00", JS.GetStr(loan, "NEWHUD.X93")));
      return nameValueCollection;
    }
  }
}
