// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._DISCLOSURE_NOTICESCLASS
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
  public class _DISCLOSURE_NOTICESCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str3);
      string str4 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X1"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X1", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "Investor", false) == 0, " do not", "do");
      nameValueCollection.Add("190_Disclosure", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "Investor", false) == 0, " investment property", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "SecondHome", false) == 0, " secondary residence", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "PrimaryResidence", false) == 0, " primary residence", "")));
      nameValueCollection.Add("1811", str7);
      string str8 = JS.GetStr(loan, "NOTICES.X2");
      nameValueCollection.Add("X2", str8);
      string str9 = JS.GetStr(loan, "NOTICES.X3") + "  " + JS.GetStr(loan, "NOTICES.X4");
      nameValueCollection.Add("X3_X4", str9);
      string str10 = JS.GetStr(loan, "NOTICES.X5") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X6"), "", false) != 0, ", ") + JS.GetStr(loan, "NOTICES.X6") + " " + JS.GetStr(loan, "NOTICES.X7");
      nameValueCollection.Add("X5_X6_X7", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X9"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X9", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X10"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X10", str12);
      string str13 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str13);
      string str14 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_a", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X12"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X12", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X13"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X13", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X14"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X14", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X15"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X15", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X16"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X16", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "L252"), "", false) != 0, "COMPANY", "");
      nameValueCollection.Add("COMPANY", str20);
      string str21 = JS.GetStr(loan, "L252");
      nameValueCollection.Add("L252", str21);
      string str22 = JS.GetStr(loan, "VEND.X157");
      nameValueCollection.Add("VENDX157", str22);
      string str23 = JS.GetStr(loan, "VEND.X158") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X159"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X159") + " " + JS.GetStr(loan, "VEND.X160");
      nameValueCollection.Add("VENDX158_159_160", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X162"), "", false) != 0, "AGENT ", "") + JS.GetStr(loan, "VEND.X162");
      nameValueCollection.Add("AGENT", str24);
      string str25 = JS.GetStr(loan, "VEND.X163");
      nameValueCollection.Add("VENDX163", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X17"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X17", str26);
      string str27 = JS.GetStr(loan, "NOTICES.X18");
      nameValueCollection.Add("X18", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X19"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("X19", str28);
      string str29 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str29);
      string str30 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str30);
      return nameValueCollection;
    }
  }
}
