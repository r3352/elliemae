// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._SSA_89SOCIALSECURITYNUMBERVERIFICATIONP1CLASS
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
  public class _SSA_89SOCIALSECURITYNUMBERVERIFICATIONP1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "1402");
      nameValueCollection.Add("1402", str2);
      string str3 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str3);
      string str4 = JS.GetStr(loan, "3249");
      nameValueCollection.Add("3249", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3860"), "Y", false) == 0, "X");
      nameValueCollection.Add("3860", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3861"), "Y", false) == 0, "X");
      nameValueCollection.Add("3861", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3862"), "Y", false) == 0, "X");
      nameValueCollection.Add("3862", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3863"), "Y", false) == 0, "X");
      nameValueCollection.Add("3863", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3864"), "Y", false) == 0, "X");
      nameValueCollection.Add("3864", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3865"), "Y", false) == 0, "X");
      nameValueCollection.Add("3865", str10);
      string str11 = JS.GetStr(loan, "3537");
      nameValueCollection.Add("3537", str11);
      string str12 = JS.GetStr(loan, "3538") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3538"), "", false) != 0, ", ") + JS.GetStr(loan, "3539") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3540"), "", false) != 0, ", ") + JS.GetStr(loan, "3540") + " " + JS.GetStr(loan, "3541");
      nameValueCollection.Add("3538_3539_3540_3541", str12);
      string str13 = JS.GetStr(loan, "3297");
      nameValueCollection.Add("3297", str13);
      string str14 = JS.GetStr(loan, "3298") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3298"), "", false) != 0, ", ") + JS.GetStr(loan, "3299") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3300"), "", false) != 0, ", ") + JS.GetStr(loan, "3300") + " " + JS.GetStr(loan, "3301");
      nameValueCollection.Add("3298_3299_3300_3301", str14);
      string str15 = JS.GetStr(loan, "3250");
      nameValueCollection.Add("3250", str15);
      string str16 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str16);
      string str17 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str17);
      string str18 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str18);
      return nameValueCollection;
    }
  }
}
