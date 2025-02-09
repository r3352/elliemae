// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_8937_VERIF_OF_BENEFITSCLASS
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
  public class _VA_26_8937_VERIF_OF_BENEFITSCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("VARELDBX7", str1);
      string str2 = JS.GetStr(loan, "362");
      nameValueCollection.Add("VARELDBX2", str2);
      string str3 = JS.GetStr(loan, "319");
      nameValueCollection.Add("VARELDBX3", str3);
      string str4 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("VARELDBX4_VARELDBX8", str4);
      string str5 = JS.GetStr(loan, "VASUMM.X32") + " " + JS.GetStr(loan, "VASUMM.X33");
      nameValueCollection.Add("36_37", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X1"), "//", false) != 0, JS.GetStr(loan, "VASUMM.X1"));
      nameValueCollection.Add("VARELDBX1", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0, JS.GetStr(loan, "FR0104"), Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0, JS.GetStr(loan, "FR0204")));
      nameValueCollection.Add("FRO104", str7);
      string str8 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108"), Jed.BF((Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208")));
      nameValueCollection.Add("FRO106_FRO107", str8);
      string str9 = JS.GetStr(loan, "VAVOB.X66");
      nameValueCollection.Add("VARELDBX23", str9);
      string str10 = JS.GetStr(loan, "VASUMM.X34");
      nameValueCollection.Add("VARELDBX5", str10);
      string str11 = JS.GetStr(loan, "VAVOB.X67");
      nameValueCollection.Add("65", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X70"), "Do", false) == 0, "X");
      nameValueCollection.Add("VARELDBX10_Do", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X70"), "Do Not", false) == 0, "X");
      nameValueCollection.Add("VARELDBX10_Donot", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X71"), "Have", false) == 0, "X");
      nameValueCollection.Add("VAVOBX71_Do", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X71"), "Have Not", false) == 0, "X");
      nameValueCollection.Add("VAVOBX71_Donot", str15);
      return nameValueCollection;
    }
  }
}
