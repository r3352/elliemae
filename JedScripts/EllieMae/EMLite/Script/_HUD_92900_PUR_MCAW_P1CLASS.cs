// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_92900_PUR_MCAW_P1CLASS
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
  public class _HUD_92900_PUR_MCAW_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("1039", JS.GetStr(loan, "1039"));
      nameValueCollection.Add("1067_Existing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1067"), "ExistingConstruction", false) == 0, " X"));
      nameValueCollection.Add("1067_Proposed", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1067"), "ProposedConstruction", false) == 0, " X"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("65", JS.GetStr(loan, "65"));
      nameValueCollection.Add("97", JS.GetStr(loan, "97"));
      nameValueCollection.Add("1109", JS.GetStr(loan, "1109"));
      nameValueCollection.Add("1160", JS.GetStr(loan, "969"));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("356", JS.GetStr(loan, "356"));
      nameValueCollection.Add("386", JS.GetStr(loan, "386"));
      nameValueCollection.Add("1131", JS.GetStr(loan, "1131"));
      nameValueCollection.Add("1132", JS.GetStr(loan, "1132"));
      nameValueCollection.Add("737", JS.GetStr(loan, "737"));
      nameValueCollection.Add("1347", JS.GetStr(loan, "1347"));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("964", JS.GetStr(loan, "964"));
      nameValueCollection.Add("MCAWPURX12", JS.GetStr(loan, "MCAWPUR.X12"));
      nameValueCollection.Add("1132_a", JS.GetStr(loan, "1132"));
      nameValueCollection.Add("MCAWPURX1", JS.GetStr(loan, "MCAWPUR.X1"));
      nameValueCollection.Add("MCAWPURX2", JS.GetStr(loan, "MCAWPUR.X2"));
      nameValueCollection.Add("MCAWPURX13", JS.GetStr(loan, "MCAWPUR.X13"));
      nameValueCollection.Add("MCAWPURX3", JS.GetStr(loan, "MCAWPUR.X3"));
      nameValueCollection.Add("1090", JS.GetStr(loan, "1090"));
      nameValueCollection.Add("MCAWPURX5", JS.GetStr(loan, "MCAWPUR.X5"));
      nameValueCollection.Add("MCAWPURX14", JS.GetStr(loan, "MCAWPUR.X14"));
      nameValueCollection.Add("1117", JS.GetStr(loan, "1117"));
      nameValueCollection.Add("61", JS.GetStr(loan, "61"));
      nameValueCollection.Add("1046", JS.GetStr(loan, "1046"));
      nameValueCollection.Add("MCAWPURX7", JS.GetStr(loan, "MCAWPUR.X7"));
      nameValueCollection.Add("3033", JS.GetStr(loan, "3033"));
      nameValueCollection.Add("1137", JS.GetStr(loan, "1137"));
      nameValueCollection.Add("MCAWPURX8", JS.GetStr(loan, "MCAWPUR.X8"));
      nameValueCollection.Add("201", JS.GetStr(loan, "201"));
      nameValueCollection.Add("MCAWPURX9", JS.GetStr(loan, "MCAWPUR.X9"));
      nameValueCollection.Add("220", JS.GetStr(loan, "220"));
      nameValueCollection.Add("1094", JS.GetStr(loan, "1094"));
      nameValueCollection.Add("MCAWPURX10", JS.GetStr(loan, "MCAWPUR.X10"));
      nameValueCollection.Add("1140", JS.GetStr(loan, "1140"));
      nameValueCollection.Add("MCAWPURX11", JS.GetStr(loan, "MCAWPUR.X11"));
      nameValueCollection.Add("101", JS.GetStr(loan, "101"));
      nameValueCollection.Add("936", JS.GetStr(loan, "936"));
      nameValueCollection.Add("110", JS.GetStr(loan, "110"));
      nameValueCollection.Add("938", JS.GetStr(loan, "938"));
      nameValueCollection.Add("906", JS.GetStr(loan, "906"));
      nameValueCollection.Add("1389", JS.GetStr(loan, "1761"));
      nameValueCollection.Add("382", JS.GetStr(loan, "382"));
      nameValueCollection.Add("733", JS.GetStr(loan, "733"));
      nameValueCollection.Add("272", JS.GetStr(loan, "272"));
      nameValueCollection.Add("1161", JS.GetStr(loan, "1161"));
      nameValueCollection.Add("1163", JS.GetStr(loan, "1163"));
      nameValueCollection.Add("1150", JS.GetStr(loan, "1150"));
      nameValueCollection.Add("465", JS.GetStr(loan, "1724"));
      nameValueCollection.Add("232", JS.GetStr(loan, "1728"));
      nameValueCollection.Add("233", JS.GetStr(loan, "1729"));
      nameValueCollection.Add("234", JS.GetStr(loan, "1730"));
      nameValueCollection.Add("229", JS.GetStr(loan, "1725"));
      nameValueCollection.Add("230", JS.GetStr(loan, "1726"));
      nameValueCollection.Add("FloodDescription", Jed.BF(Operators.CompareString(JS.GetStr(loan, "230"), JS.GetStr(loan, "1726"), false) != 0 & Jed.S2N(JS.GetStr(loan, "1726")) > 0.0, "/Flood Insurance", ""));
      nameValueCollection.Add("1405", JS.GetStr(loan, "1727"));
      nameValueCollection.Add("739", JS.GetStr(loan, "739"));
      nameValueCollection.Add("1150_a", JS.GetStr(loan, "1150"));
      nameValueCollection.Add("1187", JS.GetStr(loan, "MCAWPUR.X24"));
      nameValueCollection.Add("353", JS.GetStr(loan, "MCAWPUR.X21"));
      nameValueCollection.Add("740", JS.GetStr(loan, "MCAWPUR.X22"));
      nameValueCollection.Add("742", JS.GetStr(loan, "MCAWPUR.X23"));
      nameValueCollection.Add("1164", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1164"), "Accept", false) == 0, "A", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1164"), "Reject", false) == 0, "R", "")));
      nameValueCollection.Add("1165", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1165"), "Accept", false) == 0, "A", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1165"), "Reject", false) == 0, "R", "")));
      nameValueCollection.Add("1166", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1166"), "Accept", false) == 0, "A", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1166"), "Reject", false) == 0, "R", "")));
      nameValueCollection.Add("1167", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1167"), "Accept", false) == 0, "A", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1167"), "Reject", false) == 0, "R", "")));
      nameValueCollection.Add("1018", JS.GetStr(loan, "1018"));
      nameValueCollection.Add("940", JS.GetStr(loan, "940"));
      nameValueCollection.Add("1144", JS.GetStr(loan, "1144"));
      nameValueCollection.Add("942", JS.GetStr(loan, "942"));
      nameValueCollection.Add("136", JS.GetStr(loan, "136"));
      nameValueCollection.Add("1116", JS.GetStr(loan, "1116"));
      nameValueCollection.Add("135", JS.GetStr(loan, "135"));
      nameValueCollection.Add("3053", JS.GetStr(loan, "3053"));
      nameValueCollection.Add("980", JS.GetStr(loan, "980"));
      nameValueCollection.Add("1228", JS.GetStr(loan, "1228"));
      return nameValueCollection;
    }
  }
}
