// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_1820_LOAN_DISBURSEMENT_CERT_P2CLASS
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
  public class _VA_26_1820_LOAN_DISBURSEMENT_CERT_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str1);
      string str2 = JS.GetStr(loan, "319") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "313"), "", false) != 0, ", ") + JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("319_313_321", str2);
      string str3 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "ActuallyOccupyPropertyAsMyHome", false) == 0, "X");
      nameValueCollection.Add("1065_1", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "SpouseOnActiveMilitaryDuty", false) == 0, "X");
      nameValueCollection.Add("1065_2", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "VeteranOnActiveMilitaryDuty", false) == 0, "X");
      nameValueCollection.Add("1065_3", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "PreviouslyOccupiedTheProperty", false) == 0, "X");
      nameValueCollection.Add("1065_4", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "PreviouslyOccupiedWhileSpouseOnActiveMilitaryDuty", false) == 0, "X");
      nameValueCollection.Add("1065_5", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "PreviouslyDependentOccupiedWhileVeteranOnActiveMilitaryDuty", false) == 0, "X");
      nameValueCollection.Add("1065_6", str9);
      string str10 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1399"), "AwareOfThisValuation", false) == 0, "X");
      nameValueCollection.Add("1399_1", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1399"), "NotAwareOfThisValuation", false) == 0, "X");
      nameValueCollection.Add("1399_2", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X22"), "N", false) == 0, "X");
      nameValueCollection.Add("VAELIG_X22", str13);
      return nameValueCollection;
    }
  }
}
