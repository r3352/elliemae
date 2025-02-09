// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ANNUALREPAYMENTINCOMEWORKSHEETP2CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _ANNUALREPAYMENTINCOMEWORKSHEETP2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("USDA_X185", JS.GetStr(loan, "USDA.X185"));
      nameValueCollection.Add("USDA_X172", JS.GetStr(loan, "USDA.X172"));
      nameValueCollection.Add("USDA_X173", JS.GetStr(loan, "USDA.X173"));
      nameValueCollection.Add("USDA_X214", JS.GetStr(loan, "USDA.X214"));
      nameValueCollection.Add("USDA_X174", JS.GetStr(loan, "USDA.X174"));
      nameValueCollection.Add("USDA_X175", JS.GetStr(loan, "USDA.X175"));
      nameValueCollection.Add("USDA_X215", JS.GetStr(loan, "USDA.X215"));
      nameValueCollection.Add("USDA_X176", JS.GetStr(loan, "USDA.X176"));
      nameValueCollection.Add("USDA_X216", JS.GetStr(loan, "USDA.X216"));
      nameValueCollection.Add("USDA_X177", JS.GetStr(loan, "USDA.X177"));
      nameValueCollection.Add("14", JS.GetStr(loan, "14"));
      nameValueCollection.Add("13", JS.GetStr(loan, "13"));
      nameValueCollection.Add("USDA_X180", JS.GetStr(loan, "USDA.X180"));
      nameValueCollection.Add("USDA_X17", JS.GetStr(loan, "USDA.X17"));
      return nameValueCollection;
    }
  }
}
