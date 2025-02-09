// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_1003_ADDENDUM_P2CLASS
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
  public class _VA_1003_ADDENDUM_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "900"), "Y", false) == 0, "X");
      nameValueCollection.Add("900_Yes", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "900"), "N", false) == 0, "X");
      nameValueCollection.Add("900_No", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "933"), "Y", false) == 0, "X");
      nameValueCollection.Add("933_Yes", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "933"), "N", false) == 0, "X");
      nameValueCollection.Add("933_No", str4);
      string str5 = JS.GetStr(loan, "687");
      nameValueCollection.Add("687", str5);
      string str6 = JS.GetStr(loan, "744");
      nameValueCollection.Add("744", str6);
      string str7 = JS.GetStr(loan, "461") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1738"), "", false) != 0, ", ") + JS.GetStr(loan, "1738") + "  " + JS.GetStr(loan, "1739") + " " + JS.GetStr(loan, "1740");
      nameValueCollection.Add("461_1738_1739", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1016"), "Y", false) == 0, "X");
      nameValueCollection.Add("1016_Yes", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1016"), "N", false) == 0, "X");
      nameValueCollection.Add("1016_No", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1017"), "Y", false) == 0, "X");
      nameValueCollection.Add("1017_Yes", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1017"), "N", false) == 0, "X");
      nameValueCollection.Add("1017_No", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1398"), "Y", false) == 0, "X");
      nameValueCollection.Add("1398_Yes", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1398"), "N", false) == 0, "X");
      nameValueCollection.Add("1398_No", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "ActuallyOccupyPropertyAsMyHome", false) == 0, "X");
      nameValueCollection.Add("1065_a", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "SpouseOnActiveMilitaryDuty", false) == 0, "X");
      nameValueCollection.Add("1065_b", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "PreviouslyOccupiedTheProperty", false) == 0, "X");
      nameValueCollection.Add("1065_c", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1065"), "PreviouslyOccupiedWhileSpouseOnActiveMilitaryDuty", false) == 0, "X");
      nameValueCollection.Add("1065_d", str17);
      string str18 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1639"), "The reasonable value of the property as determined by V.A.", false) == 0, "X");
      nameValueCollection.Add("1639_ReasonValue", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1639"), "The statement of appraised value as determined by HUD / FHA", false) == 0, "X");
      nameValueCollection.Add("1639_Statement", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1399"), "AwareOfThisValuation", false) == 0, "X");
      nameValueCollection.Add("1399_a", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1399"), "NotAwareOfThisValuation", false) == 0, "X");
      nameValueCollection.Add("1399_b", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1400"), "Y", false) == 0, "X");
      nameValueCollection.Add("1400_Yes", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1400"), "N", false) == 0, "X");
      nameValueCollection.Add("1400_NotAppl", str24);
      return nameValueCollection;
    }
  }
}
