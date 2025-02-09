// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FACT_ACT_DISCLOSURE_P2CLASS
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
  public class _FACT_ACT_DISCLOSURE_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_1", str1);
      string str2 = JS.GetStr(loan, "DISCLOSURE.X21");
      nameValueCollection.Add("DISCLOSURE_X21", str2);
      string str3 = JS.GetStr(loan, "DISCLOSURE.X22");
      nameValueCollection.Add("DISCLOSURE_X22", str3);
      string str4 = JS.GetStr(loan, "DISCLOSURE.X23") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X24"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X24") + " " + JS.GetStr(loan, "DISCLOSURE.X25");
      nameValueCollection.Add("MAP2", str4);
      string str5 = JS.GetStr(loan, "DISCLOSURE.X26");
      nameValueCollection.Add("DISCLOSURE_X26", str5);
      string str6 = JS.GetStr(loan, "DISCLOSURE.X27");
      nameValueCollection.Add("DISCLOSURE_X27", str6);
      string str7 = JS.GetStr(loan, "DISCLOSURE.X28");
      nameValueCollection.Add("DISCLOSURE_X28", str7);
      string str8 = JS.GetStr(loan, "DISCLOSURE.X29");
      nameValueCollection.Add("DISCLOSURE_X29", str8);
      string str9 = JS.GetStr(loan, "DISCLOSURE.X30");
      nameValueCollection.Add("DISCLOSURE_X30", str9);
      string str10 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_2", str10);
      string str11 = JS.GetStr(loan, "1450");
      nameValueCollection.Add("1450", str11);
      string str12 = JS.GetStr(loan, "DISCLOSURE.X31");
      nameValueCollection.Add("DISCLOSURE_X31", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X174"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X174", str13);
      string str14 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_2", str14);
      string str15 = JS.GetStr(loan, "1452");
      nameValueCollection.Add("1452", str15);
      string str16 = JS.GetStr(loan, "DISCLOSURE.X32");
      nameValueCollection.Add("DISCLOSURE_X32", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X177"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X177", str17);
      string str18 = JS.GetStr(loan, "DISCLOSURE.X41");
      nameValueCollection.Add("DISCLOSURE_X41", str18);
      string str19 = JS.GetStr(loan, "DISCLOSURE.X42");
      nameValueCollection.Add("DISCLOSURE_X42", str19);
      string str20 = JS.GetStr(loan, "DISCLOSURE.X43") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X44"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X44") + " " + JS.GetStr(loan, "DISCLOSURE.X45");
      nameValueCollection.Add("MAP3", str20);
      string str21 = JS.GetStr(loan, "DISCLOSURE.X46");
      nameValueCollection.Add("DISCLOSURE_X46", str21);
      string str22 = JS.GetStr(loan, "DISCLOSURE.X47");
      nameValueCollection.Add("DISCLOSURE_X47", str22);
      string str23 = JS.GetStr(loan, "DISCLOSURE.X48");
      nameValueCollection.Add("DISCLOSURE_X48", str23);
      string str24 = JS.GetStr(loan, "DISCLOSURE.X49");
      nameValueCollection.Add("DISCLOSURE_X49", str24);
      string str25 = JS.GetStr(loan, "DISCLOSURE.X50");
      nameValueCollection.Add("DISCLOSURE_X50", str25);
      string str26 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_3", str26);
      string str27 = JS.GetStr(loan, "1414");
      nameValueCollection.Add("1414", str27);
      string str28 = JS.GetStr(loan, "DISCLOSURE.X51");
      nameValueCollection.Add("DISCLOSURE_X51", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X175"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X175", str29);
      string str30 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_3", str30);
      string str31 = JS.GetStr(loan, "1415");
      nameValueCollection.Add("1415", str31);
      string str32 = JS.GetStr(loan, "DISCLOSURE.X52");
      nameValueCollection.Add("DISCLOSURE_X52", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X178"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X178", str33);
      string str34 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_4", str34);
      string str35 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_4", str35);
      return nameValueCollection;
    }
  }
}
