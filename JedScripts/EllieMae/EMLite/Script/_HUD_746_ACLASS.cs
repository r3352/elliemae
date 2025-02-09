// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_746_ACLASS
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
  public class _HUD_746_ACLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("DRW223KX1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X1"), "on", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X2"), "on", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX3_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X3"), "Will examine at next inspection", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX3_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X3"), "Do not conceal until re-inspected", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX4", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X4"), "on", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX5", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X5"), "on", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX6", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X6"), "on", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX7_Draw", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X7"), "Draw Inspection", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX7_Contingency", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X7"), "Contingency Reserve Inspection", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX7_Final", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X7"), "Final Inspection", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX7_Change", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X7"), "Change Order", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX7_Other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X7"), "Other (Explain)", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX8", JS.GetStr(loan, "DRW223K.X8"));
      nameValueCollection.Add("DRW223KX9", "  " + JS.GetStr(loan, "DRW223K.X9"));
      nameValueCollection.Add("DRW223KX10", "  " + JS.GetStr(loan, "DRW223K.X10"));
      nameValueCollection.Add("DRW223KX11", "  " + JS.GetStr(loan, "DRW223K.X11"));
      nameValueCollection.Add("DRW223KX12", "  " + JS.GetStr(loan, "DRW223K.X12"));
      nameValueCollection.Add("DRW223KX13", "  " + JS.GetStr(loan, "DRW223K.X13"));
      nameValueCollection.Add("DRW223KX14", "  " + JS.GetStr(loan, "DRW223K.X14"));
      nameValueCollection.Add("DRW223KX15", "  " + JS.GetStr(loan, "DRW223K.X15"));
      nameValueCollection.Add("DRW223KX16", "  " + JS.GetStr(loan, "DRW223K.X16"));
      nameValueCollection.Add("DRW223KX17", "  " + JS.GetStr(loan, "DRW223K.X17"));
      nameValueCollection.Add("DRW223KX18", "  " + JS.GetStr(loan, "DRW223K.X18"));
      nameValueCollection.Add("DRW223KX19", "  " + JS.GetStr(loan, "DRW223K.X19"));
      nameValueCollection.Add("DRW223KX20", "  " + JS.GetStr(loan, "DRW223K.X20"));
      nameValueCollection.Add("DRW223KX21", "  " + JS.GetStr(loan, "DRW223K.X21"));
      nameValueCollection.Add("DRW223KX22", "  " + JS.GetStr(loan, "DRW223K.X22"));
      nameValueCollection.Add("DRW223KX23", "  " + JS.GetStr(loan, "DRW223K.X23"));
      nameValueCollection.Add("DRW223KX24", "  " + JS.GetStr(loan, "DRW223K.X24"));
      nameValueCollection.Add("DRW223KX25", "  " + JS.GetStr(loan, "DRW223K.X25"));
      nameValueCollection.Add("DRW223KX26", "  " + JS.GetStr(loan, "DRW223K.X26"));
      nameValueCollection.Add("DRW223KX27", "  " + JS.GetStr(loan, "DRW223K.X27"));
      nameValueCollection.Add("DRW223KX28", "  " + JS.GetStr(loan, "DRW223K.X28"));
      nameValueCollection.Add("DRW223KX29", "  " + JS.GetStr(loan, "DRW223K.X29"));
      nameValueCollection.Add("DRW223KX30", "  " + JS.GetStr(loan, "DRW223K.X30"));
      nameValueCollection.Add("DRW223KX31", "  " + JS.GetStr(loan, "DRW223K.X31"));
      nameValueCollection.Add("DRW223KX32", "  " + JS.GetStr(loan, "DRW223K.X32"));
      nameValueCollection.Add("DRW223KX33", "  " + JS.GetStr(loan, "DRW223K.X33"));
      nameValueCollection.Add("DRW223KX34", "  " + JS.GetStr(loan, "DRW223K.X34"));
      nameValueCollection.Add("DRW223KX35", "  " + JS.GetStr(loan, "DRW223K.X35"));
      nameValueCollection.Add("DRW223KX36", "  " + JS.GetStr(loan, "DRW223K.X36"));
      nameValueCollection.Add("DRW223KX37", "  " + JS.GetStr(loan, "DRW223K.X37"));
      nameValueCollection.Add("DRW223KX38", "  " + JS.GetStr(loan, "DRW223K.X38"));
      nameValueCollection.Add("DRW223KX39", "  " + JS.GetStr(loan, "DRW223K.X39"));
      nameValueCollection.Add("DRW223KX40", "  " + JS.GetStr(loan, "DRW223K.X40"));
      nameValueCollection.Add("DRW223KX41_Consultant", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X41"), "Consultant", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX41_Fee", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X41"), "Fee Inspector", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX41_DEStaff", Jed.BF(Operators.CompareString(JS.GetStr(loan, "DRW223K.X41"), "DE Staff Inspector", false) == 0, "   X"));
      nameValueCollection.Add("DRW223KX42", JS.GetStr(loan, "DRW223K.X42"));
      nameValueCollection.Add("DRW223KX43", JS.GetStr(loan, "DRW223K.X43"));
      return nameValueCollection;
    }
  }
}
