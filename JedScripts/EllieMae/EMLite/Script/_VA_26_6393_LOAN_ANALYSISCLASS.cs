// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_6393_LOAN_ANALYSISCLASS
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
  public class _VA_26_6393_LOAN_ANALYSISCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("1335", JS.GetStr(loan, "1335"));
      nameValueCollection.Add("38", JS.GetStr(loan, "38"));
      nameValueCollection.Add("FE0110", JS.GetStr(loan, "FE0110"));
      nameValueCollection.Add("FE0113", JS.GetStr(loan, "FE0113"));
      nameValueCollection.Add("915", JS.GetStr(loan, "915"));
      nameValueCollection.Add("737", JS.GetStr(loan, "737"));
      nameValueCollection.Add("1087_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1087"), "Yes", false) == 0, "X"));
      nameValueCollection.Add("1087_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1087"), "No", false) == 0, "X"));
      nameValueCollection.Add("70", JS.GetStr(loan, "70"));
      nameValueCollection.Add("FE0210", JS.GetStr(loan, "FE0210"));
      nameValueCollection.Add("FE0213", JS.GetStr(loan, "FE0213"));
      nameValueCollection.Add("54", JS.GetStr(loan, "54"));
      nameValueCollection.Add("1347", JS.GetStr(loan, "1347"));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("5", JS.GetStr(loan, "5"));
      nameValueCollection.Add("1405", JS.GetStr(loan, "1405"));
      nameValueCollection.Add("230", JS.GetStr(loan, "230"));
      nameValueCollection.Add("1346", JS.GetStr(loan, "1346"));
      nameValueCollection.Add("1147", Jed.NF(Jed.Num(JS.GetNum(loan, "1147")) + Jed.Num(JS.GetNum(loan, "1148")), (byte) 2, 0));
      nameValueCollection.Add("1348", JS.GetStr(loan, "1348"));
      nameValueCollection.Add("1349_2", JS.GetStr(loan, "1349"));
      nameValueCollection.Add("FL0102", JS.GetStr(loan, "FL0102"));
      nameValueCollection.Add("FL0202", JS.GetStr(loan, "FL0202"));
      nameValueCollection.Add("FL0302", JS.GetStr(loan, "FL0302"));
      nameValueCollection.Add("FL0402", JS.GetStr(loan, "FL0402"));
      nameValueCollection.Add("FL0502", JS.GetStr(loan, "FL0502"));
      nameValueCollection.Add("FL0602", JS.GetStr(loan, "FL0602"));
      nameValueCollection.Add("FL0702", JS.GetStr(loan, "FL0702"));
      nameValueCollection.Add("FL0111", JS.GetStr(loan, "FL0111"));
      nameValueCollection.Add("FL0211", JS.GetStr(loan, "FL0211"));
      nameValueCollection.Add("FL0311", JS.GetStr(loan, "FL0311"));
      nameValueCollection.Add("FL0411", JS.GetStr(loan, "FL0411"));
      nameValueCollection.Add("FL0511", JS.GetStr(loan, "FL0511"));
      nameValueCollection.Add("FL0611", JS.GetStr(loan, "FL0611"));
      nameValueCollection.Add("FL0711", JS.GetStr(loan, "FL0711"));
      nameValueCollection.Add("FL0113", JS.GetStr(loan, "FL0113"));
      nameValueCollection.Add("FL0213", JS.GetStr(loan, "FL0213"));
      nameValueCollection.Add("FL0313", JS.GetStr(loan, "FL0313"));
      nameValueCollection.Add("FL0413", JS.GetStr(loan, "FL0413"));
      nameValueCollection.Add("FL0513", JS.GetStr(loan, "FL0513"));
      nameValueCollection.Add("FL0613", JS.GetStr(loan, "FL0613"));
      nameValueCollection.Add("FL0713", JS.GetStr(loan, "FL0713"));
      nameValueCollection.Add("VALAX20", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X20"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0102"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX21", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X21"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0202"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX22", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X22"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0302"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX23", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X23"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0402"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX24", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X24"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0502"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX25", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X25"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0602"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX26", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X26"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "FL0702"), "", false) != 0, "X"));
      nameValueCollection.Add("VALAX27", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VALA.X27"), "Y", false) == 0, "X"));
      nameValueCollection.Add("256", Jed.NF(Jed.Num(JS.GetNum(loan, "256")) + Jed.Num(JS.GetNum(loan, "272")) + Jed.Num(JS.GetNum(loan, "1062")) + Jed.Num(JS.GetNum(loan, "VALA.X30")), (byte) 2, 0));
      nameValueCollection.Add("VALAX29", JS.GetStr(loan, "VALA.X29"));
      nameValueCollection.Add("733", JS.GetStr(loan, "733"));
      nameValueCollection.Add("1089", JS.GetStr(loan, "1089"));
      nameValueCollection.Add("1088", JS.GetStr(loan, "1088"));
      nameValueCollection.Add("1810", JS.GetStr(loan, "1810"));
      nameValueCollection.Add("1306", JS.GetStr(loan, "1306"));
      nameValueCollection.Add("1307", JS.GetStr(loan, "1307"));
      nameValueCollection.Add("1308", JS.GetStr(loan, "1308"));
      nameValueCollection.Add("1309", JS.GetStr(loan, "1309"));
      nameValueCollection.Add("1310", JS.GetStr(loan, "1310"));
      nameValueCollection.Add("1161", JS.GetStr(loan, "VALA.X19"));
      nameValueCollection.Add("1156", JS.GetStr(loan, "1156"));
      nameValueCollection.Add("1158", JS.GetStr(loan, "1158"));
      nameValueCollection.Add("1159", JS.GetStr(loan, "1159"));
      nameValueCollection.Add("1311", JS.GetStr(loan, "1311"));
      nameValueCollection.Add("1312", JS.GetStr(loan, "1312"));
      nameValueCollection.Add("1313", JS.GetStr(loan, "1313"));
      nameValueCollection.Add("1314", JS.GetStr(loan, "1314"));
      nameValueCollection.Add("1315", JS.GetStr(loan, "1315"));
      nameValueCollection.Add("1316", JS.GetStr(loan, "1316"));
      nameValueCollection.Add("1317", JS.GetStr(loan, "1317"));
      nameValueCollection.Add("1318", JS.GetStr(loan, "1318"));
      nameValueCollection.Add("1319", JS.GetStr(loan, "1319"));
      nameValueCollection.Add("1320", JS.GetStr(loan, "1320"));
      nameValueCollection.Add("1321", JS.GetStr(loan, "1321"));
      nameValueCollection.Add("198", JS.GetStr(loan, "198"));
      nameValueCollection.Add("1323", JS.GetStr(loan, "1323"));
      nameValueCollection.Add("1349_1", JS.GetStr(loan, "1349"));
      nameValueCollection.Add("1325", JS.GetStr(loan, "1325"));
      nameValueCollection.Add("1340", JS.GetStr(loan, "1340"));
      nameValueCollection.Add("1341", JS.GetStr(loan, "1341"));
      nameValueCollection.Add("1326_Satisfactory", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1326"), "Satisfactory", false) == 0, "X"));
      nameValueCollection.Add("1326_Unsatisfac", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1326"), "Unsatisfactory", false) == 0, "X"));
      nameValueCollection.Add("1327_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1327"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1327_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1327"), "N", false) == 0, "X"));
      nameValueCollection.Add("1018", JS.GetStr(loan, "1018"));
      nameValueCollection.Add("1144", JS.GetStr(loan, "1144"));
      nameValueCollection.Add("1216", JS.GetStr(loan, "VALA.X10"));
      nameValueCollection.Add("1217", JS.GetStr(loan, "VALA.X11"));
      nameValueCollection.Add("1218", JS.GetStr(loan, "VALA.X12"));
      nameValueCollection.Add("1219", JS.GetStr(loan, "VALA.X13"));
      nameValueCollection.Add("1220", JS.GetStr(loan, "VALA.X14"));
      nameValueCollection.Add("1221", JS.GetStr(loan, "VALA.X15"));
      nameValueCollection.Add("1222", JS.GetStr(loan, "VALA.X16"));
      nameValueCollection.Add("1223", JS.GetStr(loan, "VALA.X17"));
      nameValueCollection.Add("1224", JS.GetStr(loan, "VALA.X18"));
      return nameValueCollection;
    }
  }
}
