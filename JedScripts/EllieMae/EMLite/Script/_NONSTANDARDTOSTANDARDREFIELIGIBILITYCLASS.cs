// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._NONSTANDARDTOSTANDARDREFIELIGIBILITYCLASS
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
  public class _NONSTANDARDTOSTANDARDREFIELIGIBILITYCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("363", JS.GetStr(loan, "363"));
      nameValueCollection.Add("364", JS.GetStr(loan, "364"));
      nameValueCollection.Add("QM_X1_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X1"), "May", false) == 0, "X"));
      nameValueCollection.Add("QM_X1_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X1"), "May Not", false) == 0, "X"));
      nameValueCollection.Add("QM_X2_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X2"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X2_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X2"), "N", false) == 0, "X"));
      nameValueCollection.Add("NTB_X30", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X30"), "Y", false) == 0, "Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X30"), "N", false) == 0, "No", "")));
      nameValueCollection.Add("QM_X3_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X3"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X3_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X3"), "N", false) == 0, "X"));
      nameValueCollection.Add("QM_X4", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X4"), "//", false) != 0, JS.GetStr(loan, "QM.X4")));
      nameValueCollection.Add("NTB_X3", JS.GetStr(loan, "NTB.X3"));
      nameValueCollection.Add("NTB_X7", JS.GetStr(loan, "NTB.X7"));
      nameValueCollection.Add("NTB_X2", JS.GetStr(loan, "NTB.X2"));
      nameValueCollection.Add("NTB_X20", JS.GetStr(loan, "NTB.X20"));
      nameValueCollection.Add("NTB_X5_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X5"), "AdjustableRate", false) == 0, "X"));
      nameValueCollection.Add("QM_X7", JS.GetStr(loan, "QM.X7"));
      nameValueCollection.Add("QM_X8", JS.GetStr(loan, "QM.X8"));
      nameValueCollection.Add("QM_X9", JS.GetStr(loan, "QM.X9"));
      nameValueCollection.Add("QM_X10", JS.GetStr(loan, "QM.X10"));
      nameValueCollection.Add("QM_X11", JS.GetStr(loan, "QM.X11"));
      nameValueCollection.Add("QM_X12", JS.GetStr(loan, "QM.X12"));
      nameValueCollection.Add("NTB_X12_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X12"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X13", JS.GetStr(loan, "QM.X13"));
      nameValueCollection.Add("QM_X14", JS.GetStr(loan, "QM.X14"));
      nameValueCollection.Add("QM_X15", JS.GetStr(loan, "QM.X15"));
      nameValueCollection.Add("QM_X16_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X16"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X16_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X16"), "N", false) == 0, "X"));
      nameValueCollection.Add("QM_X5", JS.GetStr(loan, "QM.X5"));
      nameValueCollection.Add("QM_X6_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X6"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X6_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X6"), "N", false) == 0, "X"));
      nameValueCollection.Add("4", JS.GetStr(loan, "4"));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("5", JS.GetStr(loan, "5"));
      nameValueCollection.Add("QM_X17_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X17"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X17_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X17"), "N", false) == 0, "X"));
      nameValueCollection.Add("1659_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1659_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "N", false) == 0, "X"));
      nameValueCollection.Add("NTB_X29", JS.GetStr(loan, "NTB.X29"));
      nameValueCollection.Add("QM_X18_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X18"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X18_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X18"), "N", false) == 0, "X"));
      nameValueCollection.Add("QM_X19", JS.GetStr(loan, "QM.X19"));
      nameValueCollection.Add("QM_X20_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X20"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X20_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X20"), "N", false) == 0, "X"));
      nameValueCollection.Add("NTB_X25_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X25"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X25_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X25"), "N", false) == 0, "X"));
      nameValueCollection.Add("QM_X21_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X21"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X21_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X21"), "N", false) == 0, "X"));
      nameValueCollection.Add("QM_X22_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X22"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X22_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X22"), "N", false) == 0, "X"));
      nameValueCollection.Add("315", Jed.BF(Operators.CompareString(JS.GetStr(loan, "315"), "", false) != 0, JS.GetStr(loan, "315"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X293"), "", false) != 0, JS.GetStr(loan, "VEND.X293"), "")));
      return nameValueCollection;
    }
  }
}
