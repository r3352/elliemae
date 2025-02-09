// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._REGZ___TIL32_PG_1CLASS
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
  public class _REGZ___TIL32_PG_1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str4);
      string str5 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str5);
      string str6 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str6);
      string str7 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str7);
      string str8 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str8);
      string str9 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("315", str9);
      string str10 = JS.GetStr(loan, "1257");
      nameValueCollection.Add("319", str10);
      string str11 = JS.GetStr(loan, "1258") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1259"), "", false) != 0, ", ") + JS.GetStr(loan, "1259") + " " + JS.GetStr(loan, "1260");
      nameValueCollection.Add("313_321_323", str11);
      string str12 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str12);
      string str13 = JS.GetStr(loan, "799");
      nameValueCollection.Add("799", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "423"), "Biweekly", false) != 0, "X", "");
      nameValueCollection.Add("423_Monthly", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "423"), "Biweekly", false) == 0, "X", "");
      nameValueCollection.Add("423_Biweekly", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X", "");
      nameValueCollection.Add("608_Applicable", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) != 0, "X", "");
      nameValueCollection.Add("608_NotApplicable", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, JS.GetStr(loan, "2"), "");
      nameValueCollection.Add("S32DISC_X1", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, JS.GetStr(loan, "3"), "");
      nameValueCollection.Add("S32DISC_X2", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, JS.GetStr(loan, "247"), "");
      nameValueCollection.Add("S32DISC_X3", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, Jed.NF(Jed.Num(JS.GetNum(loan, "3")) + Jed.Num(JS.GetNum(loan, "247")), (byte) 2, 0), "");
      nameValueCollection.Add("S32DISC_X4", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, JS.GetStr(loan, "5"), "");
      nameValueCollection.Add("S32DISC_X5", str22);
      string str23 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1698"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1697"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("1698_Is", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1698"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "1697"), "Y", false) != 0, "X", "");
      nameValueCollection.Add("1698_IsNot", str25);
      return nameValueCollection;
    }
  }
}
