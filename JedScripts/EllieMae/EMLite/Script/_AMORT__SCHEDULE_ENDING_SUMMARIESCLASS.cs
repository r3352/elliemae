// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._AMORT__SCHEDULE_ENDING_SUMMARIESCLASS
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
  public class _AMORT__SCHEDULE_ENDING_SUMMARIESCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "68"), "", false) != 0, ", ") + JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str2);
      string str3 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str3);
      string str4 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str4);
      string str5 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str5);
      string str6 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str6);
      string str7 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1677"), "", false) != 0, JS.GetStr(loan, "1677"), JS.GetStr(loan, "3"));
      nameValueCollection.Add("3", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1176"), "", false) != 0, JS.GetStr(loan, "1176"), JS.GetStr(loan, "4"));
      nameValueCollection.Add("4", str9);
      return nameValueCollection;
    }
  }
}
