// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ANNUALREPAYMENTINCOMEWORKSHEETP3CLASS
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
  public class _ANNUALREPAYMENTINCOMEWORKSHEETP3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37"));
      nameValueCollection.Add("68_69", JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69"));
      nameValueCollection.Add("USDA_X182", JS.GetStr(loan, "USDA.X182"));
      nameValueCollection.Add("USDA_X193", JS.GetStr(loan, "USDA.X193"));
      nameValueCollection.Add("USDA_X202", JS.GetStr(loan, "USDA.X202"));
      nameValueCollection.Add("USDA_X203", JS.GetStr(loan, "USDA.X203"));
      nameValueCollection.Add("USDA_X207", JS.GetStr(loan, "USDA.X207"));
      nameValueCollection.Add("USDA_X183", JS.GetStr(loan, "USDA.X183"));
      nameValueCollection.Add("USDA_X200", JS.GetStr(loan, "USDA.X200"));
      nameValueCollection.Add("USDA_X204", JS.GetStr(loan, "USDA.X204"));
      nameValueCollection.Add("USDA_X205", JS.GetStr(loan, "USDA.X205"));
      nameValueCollection.Add("USDA_X208", JS.GetStr(loan, "USDA.X208"));
      nameValueCollection.Add("USDA_X201", JS.GetStr(loan, "USDA.X201"));
      nameValueCollection.Add("USDA_X206", JS.GetStr(loan, "USDA.X206"));
      nameValueCollection.Add("USDA_X184", JS.GetStr(loan, "USDA.X184"));
      nameValueCollection.Add("USDA_X184_a", JS.GetStr(loan, "USDA.X184"));
      nameValueCollection.Add("USDA_X196_X197", JS.GetStr(loan, "USDA.X196") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "USDA.X196"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "USDA.X197"), "", false) != 0, " / ", "") + JS.GetStr(loan, "USDA.X197"));
      nameValueCollection.Add("1264", JS.GetStr(loan, "1264"));
      return nameValueCollection;
    }
  }
}
