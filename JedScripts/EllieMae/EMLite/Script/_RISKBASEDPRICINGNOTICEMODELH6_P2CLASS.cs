// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._RISKBASEDPRICINGNOTICEMODELH6_P2CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _RISKBASEDPRICINGNOTICEMODELH6_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "67");
      nameValueCollection.Add("67", str1);
      string str2 = JS.GetStr(loan, "DISCLOSURE.X1");
      nameValueCollection.Add("DISCLOSURE_X1", str2);
      string str3 = JS.GetStr(loan, "DISCLOSURE.X11");
      nameValueCollection.Add("DISCLOSURE_X11", str3);
      string str4 = JS.GetStr(loan, "DISCLOSURE.X9");
      nameValueCollection.Add("DISCLOSURE_X9", str4);
      string str5 = JS.GetStr(loan, "DISCLOSURE.X10");
      nameValueCollection.Add("DISCLOSURE_X10", str5);
      string str6 = JS.GetStr(loan, "DISCLOSURE.X13") + JS.GetStr(loan, "DISCLOSURE.X14") + JS.GetStr(loan, "DISCLOSURE.X15") + JS.GetStr(loan, "DISCLOSURE.X16") + JS.GetStr(loan, "DISCLOSURE.X173");
      nameValueCollection.Add("DISCLOSURE_X173", str6);
      return nameValueCollection;
    }
  }
}
