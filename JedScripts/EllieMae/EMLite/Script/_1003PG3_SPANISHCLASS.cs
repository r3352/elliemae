// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003PG3_SPANISHCLASS
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
  public class _1003PG3_SPANISHCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("315", JS.GetStr(loan, "315"));
      nameValueCollection.Add("101", JS.GetStr(loan, "101"));
      nameValueCollection.Add("102", JS.GetStr(loan, "102"));
      nameValueCollection.Add("103", JS.GetStr(loan, "103"));
      nameValueCollection.Add("104", JS.GetStr(loan, "104"));
      nameValueCollection.Add("105", JS.GetStr(loan, "105"));
      nameValueCollection.Add("106", JS.GetStr(loan, "106"));
      nameValueCollection.Add("107", JS.GetStr(loan, "107"));
      nameValueCollection.Add("108", JS.GetStr(loan, "108"));
      nameValueCollection.Add("910", JS.GetStr(loan, "910"));
      nameValueCollection.Add("110", JS.GetStr(loan, "110"));
      nameValueCollection.Add("111", JS.GetStr(loan, "111"));
      nameValueCollection.Add("112", JS.GetStr(loan, "112"));
      nameValueCollection.Add("113", JS.GetStr(loan, "113"));
      nameValueCollection.Add("114", JS.GetStr(loan, "114"));
      nameValueCollection.Add("115", JS.GetStr(loan, "115"));
      nameValueCollection.Add("116", JS.GetStr(loan, "116"));
      nameValueCollection.Add("117", JS.GetStr(loan, "117"));
      nameValueCollection.Add("911", JS.GetStr(loan, "911"));
      nameValueCollection.Add("901", JS.GetStr(loan, "901"));
      nameValueCollection.Add("902", JS.GetStr(loan, "902"));
      nameValueCollection.Add("903", JS.GetStr(loan, "903"));
      nameValueCollection.Add("904", JS.GetStr(loan, "904"));
      nameValueCollection.Add("905", JS.GetStr(loan, "905"));
      nameValueCollection.Add("906", JS.GetStr(loan, "906"));
      nameValueCollection.Add("907", JS.GetStr(loan, "907"));
      nameValueCollection.Add("908", JS.GetStr(loan, "908"));
      nameValueCollection.Add("736", JS.GetStr(loan, "736"));
      nameValueCollection.Add("119", JS.GetStr(loan, "119"));
      nameValueCollection.Add("120", JS.GetStr(loan, "120"));
      nameValueCollection.Add("121", JS.GetStr(loan, "121"));
      nameValueCollection.Add("122", JS.GetStr(loan, "122"));
      nameValueCollection.Add("123", JS.GetStr(loan, "123"));
      nameValueCollection.Add("124", JS.GetStr(loan, "124"));
      nameValueCollection.Add("125", JS.GetStr(loan, "125"));
      nameValueCollection.Add("126", JS.GetStr(loan, "126"));
      nameValueCollection.Add("737", JS.GetStr(loan, "737"));
      nameValueCollection.Add("dum3", "");
      nameValueCollection.Add("228", JS.GetStr(loan, "228"));
      nameValueCollection.Add("229", JS.GetStr(loan, "229"));
      nameValueCollection.Add("230", JS.GetStr(loan, "230"));
      nameValueCollection.Add("1405", JS.GetStr(loan, "1405"));
      nameValueCollection.Add("232", JS.GetStr(loan, "232"));
      nameValueCollection.Add("233", JS.GetStr(loan, "233"));
      nameValueCollection.Add("234", JS.GetStr(loan, "234"));
      nameValueCollection.Add("912", JS.GetStr(loan, "912"));
      nameValueCollection.Add("144", JS.GetStr(loan, "144"));
      nameValueCollection.Add("146", JS.GetStr(loan, "146"));
      nameValueCollection.Add("147", JS.GetStr(loan, "147"));
      nameValueCollection.Add("149", JS.GetStr(loan, "149"));
      nameValueCollection.Add("150", JS.GetStr(loan, "150"));
      nameValueCollection.Add("152", JS.GetStr(loan, "152"));
      nameValueCollection.Add("145", Jed.BF(Operators.CompareString(JS.GetStr(loan, "145"), "Unemployment", false) == 0, "Unemployment/Welfare", Jed.BF(Operators.CompareString(JS.GetStr(loan, "145"), "Pension", false) == 0, "Pension/Retirement Income", Jed.BF(Operators.CompareString(JS.GetStr(loan, "145"), "SocialSecurity", false) == 0, "Social Security/Disability Income", Jed.BF(Operators.CompareString(JS.GetStr(loan, "145"), "Trust", false) == 0, "Trust Income", JS.GetStr(loan, "145"))))));
      nameValueCollection.Add("148", Jed.BF(Operators.CompareString(JS.GetStr(loan, "148"), "Unemployment", false) == 0, "Unemployment/Welfare", Jed.BF(Operators.CompareString(JS.GetStr(loan, "148"), "Pension", false) == 0, "Pension/Retirement Income", Jed.BF(Operators.CompareString(JS.GetStr(loan, "148"), "SocialSecurity", false) == 0, "Social Security/Disability Income", Jed.BF(Operators.CompareString(JS.GetStr(loan, "148"), "Trust", false) == 0, "Trust Income", JS.GetStr(loan, "148"))))));
      nameValueCollection.Add("151", Jed.BF(Operators.CompareString(JS.GetStr(loan, "151"), "Unemployment", false) == 0, "Unemployment/Welfare", Jed.BF(Operators.CompareString(JS.GetStr(loan, "151"), "Pension", false) == 0, "Pension/Retirement Income", Jed.BF(Operators.CompareString(JS.GetStr(loan, "151"), "SocialSecurity", false) == 0, "Social Security/Disability Income", Jed.BF(Operators.CompareString(JS.GetStr(loan, "151"), "Trust", false) == 0, "Trust Income", JS.GetStr(loan, "151"))))));
      nameValueCollection.Add("181_Joint", Jed.BF(Operators.CompareString(JS.GetStr(loan, "181"), "Jointly", false) == 0, "X"));
      nameValueCollection.Add("181_NotJoint", Jed.BF(Operators.CompareString(JS.GetStr(loan, "181"), "NotJointly", false) == 0, "X"));
      nameValueCollection.Add("182", JS.GetStr(loan, "182"));
      nameValueCollection.Add("183", Jed.BF(Operators.CompareString(JS.GetStr(loan, "183"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "183"), "0.00", false) != 0, Jed.NF(Jed.S2N(JS.GetStr(loan, "183")), (byte) 18, 0)));
      nameValueCollection.Add("1715", JS.GetStr(loan, "1715"));
      nameValueCollection.Add("1716", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1716"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "1716"), "0.00", false) != 0, Jed.NF(Jed.S2N(JS.GetStr(loan, "1716")), (byte) 18, 0)));
      nameValueCollection.Add("36_37", JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37"));
      return nameValueCollection;
    }
  }
}
