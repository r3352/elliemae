// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003_P4CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _1003_P4CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("305", JS.GetStr(loan, "305"));
      nameValueCollection.Add("1003p4", "");
      return nameValueCollection;
    }
  }
}
