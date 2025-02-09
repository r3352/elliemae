// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ABILITYTOREPAY_P4CLASS
// Assembly: JedScripts, Version=19.3.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0C1FAB5C-E085-4229-8A3F-9EA3E2E6B3AA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\JedScripts.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.JedScript;
using System.Collections.Specialized;

#nullable disable
namespace JedScripts.EllieMae.EMLite.Script
{
  public class _ABILITYTOREPAY_P4CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("364", JS.GetStr(loan, "364"));
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("363", JS.GetStr(loan, "363"));
      nameValueCollection.Add("3", Jed.BF(Jed.Num(JS.GetNum(loan, "3")) > 0.0, JS.GetStr(loan, "3") + " %", ""));
      nameValueCollection.Add("5", Jed.BF(Jed.Num(JS.GetNum(loan, "5")) > 0.0, "$ " + JS.GetStr(loan, "5"), ""));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("949", JS.GetStr(loan, "949"));
      nameValueCollection.Add("1109_a", JS.GetStr(loan, "948"));
      return nameValueCollection;
    }
  }
}
