// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_0286_LOAN_SUMMARY_P2CLASS
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
  public class _VA_26_0286_LOAN_SUMMARY_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("986_Conventional", Jed.BF(Operators.CompareString(JS.GetStr(loan, "986"), "Conventional", false) == 0, "X"));
      nameValueCollection.Add("986_Singlewide", Jed.BF(Operators.CompareString(JS.GetStr(loan, "986"), "Singlewide", false) == 0, "X"));
      nameValueCollection.Add("986_Doublewide", Jed.BF(Operators.CompareString(JS.GetStr(loan, "986"), "Doublewide", false) == 0, "X"));
      nameValueCollection.Add("986_M/H", Jed.BF(Operators.CompareString(JS.GetStr(loan, "986"), "LotOnly", false) == 0, "X"));
      nameValueCollection.Add("986_Prefabricated", Jed.BF(Operators.CompareString(JS.GetStr(loan, "986"), "PrefabricatedHome", false) == 0, "X"));
      nameValueCollection.Add("986_Condo", Jed.BF(Operators.CompareString(JS.GetStr(loan, "986"), "Condominium", false) == 0, "X"));
      nameValueCollection.Add("988_Existing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "988"), "ExistingOrUsedHome", false) == 0, "X"));
      nameValueCollection.Add("988_Appraised", Jed.BF(Operators.CompareString(JS.GetStr(loan, "988"), "ProposedConstruction", false) == 0, "X"));
      nameValueCollection.Add("988_NewExisting", Jed.BF(Operators.CompareString(JS.GetStr(loan, "988"), "NewExisting-NeverOccupied", false) == 0, "X"));
      nameValueCollection.Add("988_Energy", Jed.BF(Operators.CompareString(JS.GetStr(loan, "988"), "EnergyImprovements", false) == 0, "X"));
      nameValueCollection.Add("16_Single", Jed.BF(Operators.CompareString(JS.GetStr(loan, "16"), "1", false) == 0, "X"));
      nameValueCollection.Add("16_TwoUnits", Jed.BF(Operators.CompareString(JS.GetStr(loan, "16"), "2", false) == 0, "X"));
      nameValueCollection.Add("16_ThreeUnits", Jed.BF(Operators.CompareString(JS.GetStr(loan, "16"), "3", false) == 0, "X"));
      nameValueCollection.Add("16_Fourormore", Jed.BF(Jed.S2N(JS.GetStr(loan, "16")) > 3.0, "X"));
      nameValueCollection.Add("989", JS.GetStr(loan, "989"));
      nameValueCollection.Add("963_Other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "963"), "Other", false) == 0, "X"));
      nameValueCollection.Add("963_Rentedspace", Jed.BF(Operators.CompareString(JS.GetStr(loan, "963"), "RentedSpace", false) == 0, "X"));
      nameValueCollection.Add("963_M/HVetown", Jed.BF(Operators.CompareString(JS.GetStr(loan, "963"), "VeteranOwnedLot", false) == 0, "X"));
      nameValueCollection.Add("963_OnPermanent", Jed.BF(Operators.CompareString(JS.GetStr(loan, "963"), "PermanentFoundation", false) == 0, "X"));
      nameValueCollection.Add("11", JS.GetStr(loan, "11"));
      nameValueCollection.Add("12", JS.GetStr(loan, "12"));
      nameValueCollection.Add("14", JS.GetStr(loan, "14"));
      nameValueCollection.Add("15", JS.GetStr(loan, "15"));
      nameValueCollection.Add("13", JS.GetStr(loan, "13"));
      nameValueCollection.Add("1059", JS.GetStr(loan, "1059"));
      nameValueCollection.Add("1060", JS.GetStr(loan, "1060"));
      nameValueCollection.Add("305", JS.GetStr(loan, "305"));
      nameValueCollection.Add("VASUMMX6", JS.GetStr(loan, "VASUMM.X6"));
      nameValueCollection.Add("VASUMMX7", JS.GetStr(loan, "VASUMM.X7"));
      nameValueCollection.Add("VASUMMX8", JS.GetStr(loan, "VASUMM.X8"));
      nameValueCollection.Add("VASUMMX9", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X9"), "//", false) != 0, JS.GetStr(loan, "VASUMM.X9")));
      nameValueCollection.Add("VASUMMX10", JS.GetStr(loan, "VASUMM.X10"));
      nameValueCollection.Add("VASUMMX11", JS.GetStr(loan, "VASUMM.X11"));
      nameValueCollection.Add("VASUMMX12", JS.GetStr(loan, "VASUMM.X12"));
      nameValueCollection.Add("VASUMMX13_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X13"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX13_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X13"), "No", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX14_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X4"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX14_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X4"), "No", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX17", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X17"), "01", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX18", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X17"), "02", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX19", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X17"), "03", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX20", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X17"), "04", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX25", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X17"), "05", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX21", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X21"), "1", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX22", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X21"), "2", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX23", JS.GetStr(loan, "VASUMM.X23"));
      nameValueCollection.Add("915", JS.GetStr(loan, "915"));
      nameValueCollection.Add("993", JS.GetStr(loan, "993"));
      nameValueCollection.Add("1325", JS.GetStr(loan, "1325"));
      nameValueCollection.Add("1340", JS.GetStr(loan, "1340"));
      nameValueCollection.Add("1341", JS.GetStr(loan, "1341"));
      nameValueCollection.Add("1006_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1006"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("1006_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1006"), "No", false) == 0, "X"));
      nameValueCollection.Add("1008", JS.GetStr(loan, "1008"));
      nameValueCollection.Add("1061", JS.GetStr(loan, "VASUMM.X47"));
      nameValueCollection.Add("331", JS.GetStr(loan, "VASUMM.X45"));
      nameValueCollection.Add("1010", JS.GetStr(loan, "VASUMM.X48"));
      nameValueCollection.Add("1093", JS.GetStr(loan, "VASUMM.X46"));
      nameValueCollection.Add("4", JS.GetStr(loan, "4"));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("990_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "990"), "Y", false) == 0, "X"));
      nameValueCollection.Add("990_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "990"), "N", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX14", JS.GetStr(loan, "VASUMM.X14"));
      nameValueCollection.Add("VASUMMX15", JS.GetStr(loan, "VASUMM.X15"));
      nameValueCollection.Add("VASUMMX16", JS.GetStr(loan, "VASUMM.X16"));
      nameValueCollection.Add("VASUMMX37", JS.GetStr(loan, "VASUMM.X37"));
      nameValueCollection.Add("VASUMMX38", JS.GetStr(loan, "VASUMM.X38"));
      nameValueCollection.Add("VASUMMX39", JS.GetStr(loan, "VASUMM.X39"));
      nameValueCollection.Add("VASUMMX40", JS.GetStr(loan, "VASUMM.X40"));
      nameValueCollection.Add("VASUMMX41", JS.GetStr(loan, "VASUMM.X41"));
      nameValueCollection.Add("VASUMMX42", JS.GetStr(loan, "VASUMM.X42"));
      nameValueCollection.Add("VASUMMX43", JS.GetStr(loan, "VASUMM.X43"));
      nameValueCollection.Add("VASUMMX44", JS.GetStr(loan, "VASUMM.X44"));
      return nameValueCollection;
    }
  }
}
