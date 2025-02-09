// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._PROFIT_MANAGEMENTCLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _PROFIT_MANAGEMENTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("364", JS.GetStr(loan, "364"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("PM01", JS.GetStr(loan, "PM01"));
      nameValueCollection.Add("PM02", JS.GetStr(loan, "PM02"));
      nameValueCollection.Add("PM03", JS.GetStr(loan, "PM03"));
      nameValueCollection.Add("PM04", JS.GetStr(loan, "PM04"));
      nameValueCollection.Add("PM05", JS.GetStr(loan, "PM05"));
      nameValueCollection.Add("PM06", JS.GetStr(loan, "PM06"));
      nameValueCollection.Add("PM07", JS.GetStr(loan, "PM07"));
      nameValueCollection.Add("PM08", JS.GetStr(loan, "PM08"));
      nameValueCollection.Add("PM09", JS.GetStr(loan, "PM09"));
      nameValueCollection.Add("PM10", JS.GetStr(loan, "PM10"));
      nameValueCollection.Add("PM11", JS.GetStr(loan, "PM11"));
      nameValueCollection.Add("PM12", JS.GetStr(loan, "PM12"));
      nameValueCollection.Add("PM14", JS.GetStr(loan, "PM14"));
      nameValueCollection.Add("PM15", JS.GetStr(loan, "PM15"));
      nameValueCollection.Add("PM16", JS.GetStr(loan, "PM16"));
      nameValueCollection.Add("PM18", JS.GetStr(loan, "PM18"));
      nameValueCollection.Add("PM19", JS.GetStr(loan, "PM19"));
      nameValueCollection.Add("PM20", JS.GetStr(loan, "PM20"));
      nameValueCollection.Add("PM22", JS.GetStr(loan, "PM22"));
      nameValueCollection.Add("PM27", JS.GetStr(loan, "PM27"));
      nameValueCollection.Add("PM23", JS.GetStr(loan, "PM23"));
      nameValueCollection.Add("PM24", JS.GetStr(loan, "PM24"));
      nameValueCollection.Add("PM26", JS.GetStr(loan, "PM26"));
      nameValueCollection.Add("PM13", JS.GetStr(loan, "PM13"));
      nameValueCollection.Add("PM17", JS.GetStr(loan, "PM17"));
      nameValueCollection.Add("PM21", JS.GetStr(loan, "PM21"));
      nameValueCollection.Add("PM25", JS.GetStr(loan, "PM25"));
      nameValueCollection.Add("PM28", JS.GetStr(loan, "PM28"));
      return nameValueCollection;
    }
  }
}
