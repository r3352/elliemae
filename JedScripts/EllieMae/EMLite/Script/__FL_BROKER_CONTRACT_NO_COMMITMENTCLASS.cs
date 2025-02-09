// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__FL_BROKER_CONTRACT_NO_COMMITMENTCLASS
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
  public class __FL_BROKER_CONTRACT_NO_COMMITMENTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "11") + "\r\n" + JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("MAP2", str3);
      string str4 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str4);
      string str5 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("MAP1", str5);
      string str6 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str6);
      string str7 = JS.GetStr(loan, "1824");
      nameValueCollection.Add("1824", str7);
      string str8 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str8);
      string str9 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str9);
      string str10 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str10);
      string str11 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str11);
      string str12 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("608_1", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "X");
      nameValueCollection.Add("608_3", str14);
      string str15 = JS.GetStr(loan, "995");
      nameValueCollection.Add("995", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X");
      nameValueCollection.Add("608_2", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "X");
      nameValueCollection.Add("608_4", str17);
      string str18 = JS.GetStr(loan, "994");
      nameValueCollection.Add("994", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "X");
      nameValueCollection.Add("420_1", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "X");
      nameValueCollection.Add("420_2", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "Y", false) == 0, "X");
      nameValueCollection.Add("675_1", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) != 0 & Jed.Num(JS.GetNum(loan, "325")) < Jed.Num(JS.GetNum(loan, "4")), "X");
      nameValueCollection.Add("675_2", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "690"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "691"), "", false) != 0, "X");
      nameValueCollection.Add("675_3", str23);
      string str24 = JS.GetStr(loan, "689");
      nameValueCollection.Add("689", str24);
      string str25 = JS.GetStr(loan, "688");
      nameValueCollection.Add("688", str25);
      string str26 = JS.GetStr(loan, "DISCLOSURE.X168");
      nameValueCollection.Add("DISCLOSUREX168", str26);
      string str27 = JS.GetStr(loan, "694");
      nameValueCollection.Add("694", str27);
      string str28 = JS.GetStr(loan, "697");
      nameValueCollection.Add("697", str28);
      string str29 = JS.GetStr(loan, "695");
      nameValueCollection.Add("695", str29);
      string str30 = JS.GetStr(loan, "247");
      nameValueCollection.Add("247", str30);
      string str31 = JS.GetStr(loan, "DISCLOSURE.X169");
      nameValueCollection.Add("DISCLOSUREX169", str31);
      string str32 = JS.GetStr(loan, "FLGFE.X38");
      nameValueCollection.Add("FLGFE_X38", str32);
      string str33 = JS.GetStr(loan, "DISCLOSURE.X67");
      nameValueCollection.Add("DISCLOSUREX67", str33);
      string str34 = JS.GetStr(loan, "FLGFE.X54");
      nameValueCollection.Add("FLGFE_X54", str34);
      string str35 = JS.GetStr(loan, "DISCLOSURE.X170");
      nameValueCollection.Add("DISCLOSUREX170", str35);
      string str36 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str36);
      string str37 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str37);
      string str38 = JS.GetStr(loan, "DISCLOSURE.X171");
      nameValueCollection.Add("DISCLOSUREX171", str38);
      return nameValueCollection;
    }
  }
}
