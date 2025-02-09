// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._STATEMENT_OF_DENIAL_COBORROWER_P2CLASS
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
  public class _STATEMENT_OF_DENIAL_COBORROWER_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DENIAL.X76"), "Y", false) == 0, "X");
      nameValueCollection.Add("DENIAL_X76", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X1"));
      nameValueCollection.Add("DISCLOSURE_X1", str2);
      string str3 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "DISCLOSURE.X2") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X2"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X3") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X3"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X4") + " " + JS.GetStr(loan, "DISCLOSURE.X5"));
      nameValueCollection.Add("DISCLOSURE_X2_X3_X4_X5", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X6"));
      nameValueCollection.Add("DISCLOSURE_X6", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X9"));
      nameValueCollection.Add("DISCLOSURE_X9", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X10"));
      nameValueCollection.Add("DISCLOSURE_X10", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X12"));
      nameValueCollection.Add("DISCLOSURE_X12", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, JS.GetStr(loan, "60"));
      nameValueCollection.Add("60", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X176"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X626"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X176", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X21"));
      nameValueCollection.Add("DISCLOSURE_X21", str10);
      string str11 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "DISCLOSURE.X22") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X22"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X23") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X23"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X24") + " " + JS.GetStr(loan, "DISCLOSURE.X25"));
      nameValueCollection.Add("DISCLOSURE_X22_X23_X24_X25", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X26"));
      nameValueCollection.Add("DISCLOSURE_X26", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X29"));
      nameValueCollection.Add("DISCLOSURE_X29", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X30"));
      nameValueCollection.Add("DISCLOSURE_X30", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, JS.GetStr(loan, "DISCLOSURE.X32"));
      nameValueCollection.Add("DISCLOSURE_X32", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, JS.GetStr(loan, "1452"));
      nameValueCollection.Add("1452", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X177"), "Y", false) == 0 & Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X628"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X177", str17);
      return nameValueCollection;
    }
  }
}
