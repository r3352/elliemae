// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._UNDERWRITINGCONDITIONSCLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _UNDERWRITINGCONDITIONSCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("363", JS.GetStr(loan, "363"));
      nameValueCollection.Add("364", JS.GetStr(loan, "364"));
      nameValueCollection.Add("4000_4002", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("4004_4006", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      return nameValueCollection;
    }
  }
}
