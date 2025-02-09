// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ABILITYTOREPAY_P5CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _ABILITYTOREPAY_P5CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("FL0102", JS.GetStr(loan, "FL0102"));
      nameValueCollection.Add("FL0202", JS.GetStr(loan, "FL0202"));
      nameValueCollection.Add("FL0302", JS.GetStr(loan, "FL0302"));
      nameValueCollection.Add("FL0402", JS.GetStr(loan, "FL0402"));
      nameValueCollection.Add("FL0502", JS.GetStr(loan, "FL0502"));
      nameValueCollection.Add("FL0602", JS.GetStr(loan, "FL0602"));
      nameValueCollection.Add("FL0702", JS.GetStr(loan, "FL0702"));
      nameValueCollection.Add("FL0802", JS.GetStr(loan, "FL0802"));
      nameValueCollection.Add("FL0902", JS.GetStr(loan, "FL0902"));
      nameValueCollection.Add("FL1002", JS.GetStr(loan, "FL1002"));
      nameValueCollection.Add("FL1102", JS.GetStr(loan, "FL1102"));
      nameValueCollection.Add("FL1202", JS.GetStr(loan, "FL1202"));
      nameValueCollection.Add("FL1302", JS.GetStr(loan, "FL1302"));
      nameValueCollection.Add("FL1402", JS.GetStr(loan, "FL1402"));
      nameValueCollection.Add("FL1502", JS.GetStr(loan, "FL1502"));
      nameValueCollection.Add("FL1602", JS.GetStr(loan, "FL1602"));
      nameValueCollection.Add("FL1702", JS.GetStr(loan, "FL1702"));
      nameValueCollection.Add("FL1802", JS.GetStr(loan, "FL1802"));
      nameValueCollection.Add("FL1902", JS.GetStr(loan, "FL1902"));
      nameValueCollection.Add("FL2002", JS.GetStr(loan, "FL2002"));
      nameValueCollection.Add("FL2102", JS.GetStr(loan, "FL2102"));
      nameValueCollection.Add("FL2202", JS.GetStr(loan, "FL2202"));
      nameValueCollection.Add("FL2302", JS.GetStr(loan, "FL2302"));
      nameValueCollection.Add("FL2402", JS.GetStr(loan, "FL2402"));
      nameValueCollection.Add("FL2502", JS.GetStr(loan, "FL2502"));
      nameValueCollection.Add("FL2602", JS.GetStr(loan, "FL2602"));
      nameValueCollection.Add("FL2702", JS.GetStr(loan, "FL2702"));
      nameValueCollection.Add("FL0108", JS.GetStr(loan, "FL0108"));
      nameValueCollection.Add("FL0208", JS.GetStr(loan, "FL0208"));
      nameValueCollection.Add("FL0308", JS.GetStr(loan, "FL0308"));
      nameValueCollection.Add("FL0408", JS.GetStr(loan, "FL0408"));
      nameValueCollection.Add("FL0508", JS.GetStr(loan, "FL0508"));
      nameValueCollection.Add("FL0608", JS.GetStr(loan, "FL0608"));
      nameValueCollection.Add("FL0708", JS.GetStr(loan, "FL0708"));
      nameValueCollection.Add("FL0808", JS.GetStr(loan, "FL0808"));
      nameValueCollection.Add("FL0908", JS.GetStr(loan, "FL0908"));
      nameValueCollection.Add("FL1008", JS.GetStr(loan, "FL1008"));
      nameValueCollection.Add("FL1108", JS.GetStr(loan, "FL1108"));
      nameValueCollection.Add("FL1208", JS.GetStr(loan, "FL1208"));
      nameValueCollection.Add("FL1308", JS.GetStr(loan, "FL1308"));
      nameValueCollection.Add("FL1408", JS.GetStr(loan, "FL1408"));
      nameValueCollection.Add("FL1508", JS.GetStr(loan, "FL1508"));
      nameValueCollection.Add("FL1608", JS.GetStr(loan, "FL1608"));
      nameValueCollection.Add("FL1708", JS.GetStr(loan, "FL1708"));
      nameValueCollection.Add("FL1808", JS.GetStr(loan, "FL1808"));
      nameValueCollection.Add("FL1908", JS.GetStr(loan, "FL1908"));
      nameValueCollection.Add("FL2008", JS.GetStr(loan, "FL2008"));
      nameValueCollection.Add("FL2108", JS.GetStr(loan, "FL2108"));
      nameValueCollection.Add("FL2208", JS.GetStr(loan, "FL2208"));
      nameValueCollection.Add("FL2308", JS.GetStr(loan, "FL2308"));
      nameValueCollection.Add("FL2408", JS.GetStr(loan, "FL2408"));
      nameValueCollection.Add("FL2508", JS.GetStr(loan, "FL2508"));
      nameValueCollection.Add("FL2608", JS.GetStr(loan, "FL2608"));
      nameValueCollection.Add("FL2708", JS.GetStr(loan, "FL2708"));
      nameValueCollection.Add("FL0111", JS.GetStr(loan, "FL0111"));
      nameValueCollection.Add("FL0211", JS.GetStr(loan, "FL0211"));
      nameValueCollection.Add("FL0311", JS.GetStr(loan, "FL0311"));
      nameValueCollection.Add("FL0411", JS.GetStr(loan, "FL0411"));
      nameValueCollection.Add("FL0511", JS.GetStr(loan, "FL0511"));
      nameValueCollection.Add("FL0611", JS.GetStr(loan, "FL0611"));
      nameValueCollection.Add("FL0711", JS.GetStr(loan, "FL0711"));
      nameValueCollection.Add("FL0811", JS.GetStr(loan, "FL0811"));
      nameValueCollection.Add("FL0911", JS.GetStr(loan, "FL0911"));
      nameValueCollection.Add("FL1011", JS.GetStr(loan, "FL1011"));
      nameValueCollection.Add("FL1111", JS.GetStr(loan, "FL1111"));
      nameValueCollection.Add("FL1211", JS.GetStr(loan, "FL1211"));
      nameValueCollection.Add("FL1311", JS.GetStr(loan, "FL1311"));
      nameValueCollection.Add("FL1411", JS.GetStr(loan, "FL1411"));
      nameValueCollection.Add("FL1511", JS.GetStr(loan, "FL1511"));
      nameValueCollection.Add("FL1611", JS.GetStr(loan, "FL1611"));
      nameValueCollection.Add("FL1711", JS.GetStr(loan, "FL1711"));
      nameValueCollection.Add("FL1811", JS.GetStr(loan, "FL1811"));
      nameValueCollection.Add("FL1911", JS.GetStr(loan, "FL1911"));
      nameValueCollection.Add("FL2011", JS.GetStr(loan, "FL2011"));
      nameValueCollection.Add("FL2111", JS.GetStr(loan, "FL2111"));
      nameValueCollection.Add("FL2211", JS.GetStr(loan, "FL2211"));
      nameValueCollection.Add("FL2311", JS.GetStr(loan, "FL2311"));
      nameValueCollection.Add("FL2411", JS.GetStr(loan, "FL2411"));
      nameValueCollection.Add("FL2511", JS.GetStr(loan, "FL2511"));
      nameValueCollection.Add("FL2611", JS.GetStr(loan, "FL2611"));
      nameValueCollection.Add("FL2711", JS.GetStr(loan, "FL2711"));
      nameValueCollection.Add("FL0112", JS.GetStr(loan, "FL0112"));
      nameValueCollection.Add("FL0212", JS.GetStr(loan, "FL0212"));
      nameValueCollection.Add("FL0312", JS.GetStr(loan, "FL0312"));
      nameValueCollection.Add("FL0412", JS.GetStr(loan, "FL0412"));
      nameValueCollection.Add("FL0512", JS.GetStr(loan, "FL0512"));
      nameValueCollection.Add("FL0612", JS.GetStr(loan, "FL0612"));
      nameValueCollection.Add("FL0712", JS.GetStr(loan, "FL0712"));
      nameValueCollection.Add("FL0812", JS.GetStr(loan, "FL0812"));
      nameValueCollection.Add("FL0912", JS.GetStr(loan, "FL0912"));
      nameValueCollection.Add("FL1012", JS.GetStr(loan, "FL1012"));
      nameValueCollection.Add("FL1112", JS.GetStr(loan, "FL1112"));
      nameValueCollection.Add("FL1212", JS.GetStr(loan, "FL1212"));
      nameValueCollection.Add("FL1312", JS.GetStr(loan, "FL1312"));
      nameValueCollection.Add("FL1412", JS.GetStr(loan, "FL1412"));
      nameValueCollection.Add("FL1512", JS.GetStr(loan, "FL1512"));
      nameValueCollection.Add("FL1612", JS.GetStr(loan, "FL1612"));
      nameValueCollection.Add("FL1712", JS.GetStr(loan, "FL1712"));
      nameValueCollection.Add("FL1812", JS.GetStr(loan, "FL1812"));
      nameValueCollection.Add("FL1912", JS.GetStr(loan, "FL1912"));
      nameValueCollection.Add("FL2012", JS.GetStr(loan, "FL2012"));
      nameValueCollection.Add("FL2112", JS.GetStr(loan, "FL2112"));
      nameValueCollection.Add("FL2212", JS.GetStr(loan, "FL2212"));
      nameValueCollection.Add("FL2312", JS.GetStr(loan, "FL2312"));
      nameValueCollection.Add("FL2412", JS.GetStr(loan, "FL2412"));
      nameValueCollection.Add("FL2512", JS.GetStr(loan, "FL2512"));
      nameValueCollection.Add("FL2612", JS.GetStr(loan, "FL2612"));
      nameValueCollection.Add("FL2712", JS.GetStr(loan, "FL2712"));
      nameValueCollection.Add("FL0117", JS.GetStr(loan, "FL0117"));
      nameValueCollection.Add("FL0217", JS.GetStr(loan, "FL0217"));
      nameValueCollection.Add("FL0317", JS.GetStr(loan, "FL0317"));
      nameValueCollection.Add("FL0417", JS.GetStr(loan, "FL0417"));
      nameValueCollection.Add("FL0517", JS.GetStr(loan, "FL0517"));
      nameValueCollection.Add("FL0617", JS.GetStr(loan, "FL0617"));
      nameValueCollection.Add("FL0717", JS.GetStr(loan, "FL0717"));
      nameValueCollection.Add("FL0817", JS.GetStr(loan, "FL0817"));
      nameValueCollection.Add("FL0917", JS.GetStr(loan, "FL0917"));
      nameValueCollection.Add("FL1017", JS.GetStr(loan, "FL1017"));
      nameValueCollection.Add("FL1117", JS.GetStr(loan, "FL1117"));
      nameValueCollection.Add("FL1217", JS.GetStr(loan, "FL1217"));
      nameValueCollection.Add("FL1317", JS.GetStr(loan, "FL1317"));
      nameValueCollection.Add("FL1417", JS.GetStr(loan, "FL1417"));
      nameValueCollection.Add("FL1517", JS.GetStr(loan, "FL1517"));
      nameValueCollection.Add("FL1617", JS.GetStr(loan, "FL1617"));
      nameValueCollection.Add("FL1717", JS.GetStr(loan, "FL1717"));
      nameValueCollection.Add("FL1817", JS.GetStr(loan, "FL1817"));
      nameValueCollection.Add("FL1917", JS.GetStr(loan, "FL1917"));
      nameValueCollection.Add("FL2017", JS.GetStr(loan, "FL2017"));
      nameValueCollection.Add("FL2117", JS.GetStr(loan, "FL2117"));
      nameValueCollection.Add("FL2217", JS.GetStr(loan, "FL2217"));
      nameValueCollection.Add("FL2317", JS.GetStr(loan, "FL2317"));
      nameValueCollection.Add("FL2417", JS.GetStr(loan, "FL2417"));
      nameValueCollection.Add("FL2517", JS.GetStr(loan, "FL2517"));
      nameValueCollection.Add("FL2617", JS.GetStr(loan, "FL2617"));
      nameValueCollection.Add("FL2717", JS.GetStr(loan, "FL2717"));
      nameValueCollection.Add("271", JS.GetStr(loan, "271"));
      nameValueCollection.Add("272", JS.GetStr(loan, "272"));
      nameValueCollection.Add("1835", JS.GetStr(loan, "1835"));
      nameValueCollection.Add("255", JS.GetStr(loan, "255"));
      nameValueCollection.Add("256", JS.GetStr(loan, "256"));
      nameValueCollection.Add("1836", JS.GetStr(loan, "1836"));
      nameValueCollection.Add("1058", JS.GetStr(loan, "1058"));
      nameValueCollection.Add("1062", JS.GetStr(loan, "1062"));
      nameValueCollection.Add("1837", JS.GetStr(loan, "1837"));
      nameValueCollection.Add("QM_X191", JS.GetStr(loan, "QM.X191"));
      nameValueCollection.Add("QM_X192", JS.GetStr(loan, "QM.X192"));
      nameValueCollection.Add("QM_X193", JS.GetStr(loan, "QM.X193"));
      nameValueCollection.Add("QM_X194", JS.GetStr(loan, "QM.X194"));
      nameValueCollection.Add("QM_X195", JS.GetStr(loan, "QM.X195"));
      nameValueCollection.Add("QM_X196", JS.GetStr(loan, "QM.X196"));
      nameValueCollection.Add("QM_X197", JS.GetStr(loan, "QM.X197"));
      nameValueCollection.Add("QM_X198", JS.GetStr(loan, "QM.X198"));
      nameValueCollection.Add("QM_X199", JS.GetStr(loan, "QM.X199"));
      nameValueCollection.Add("QM_X200", JS.GetStr(loan, "QM.X200"));
      nameValueCollection.Add("QM_X201", JS.GetStr(loan, "QM.X201"));
      nameValueCollection.Add("QM_X202", JS.GetStr(loan, "QM.X202"));
      return nameValueCollection;
    }
  }
}
