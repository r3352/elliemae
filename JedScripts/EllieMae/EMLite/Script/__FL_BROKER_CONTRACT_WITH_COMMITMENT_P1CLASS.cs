// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__FL_BROKER_CONTRACT_WITH_COMMITMENT_P1CLASS
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
  public class __FL_BROKER_CONTRACT_WITH_COMMITMENT_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str1);
      string str2 = JS.GetStr(loan, "FLGFE.X41");
      nameValueCollection.Add("FLGFE_X41", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str4);
      string str5 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str5);
      string str6 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("MAP1", str6);
      string str7 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str7);
      string str8 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str8);
      string str9 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str9);
      string str10 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str10);
      string str11 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str11);
      string str12 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str12);
      string str13 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str13);
      string str14 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str14);
      string str15 = JS.GetStr(loan, "325");
      nameValueCollection.Add("325", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("608_Fixed", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "X");
      nameValueCollection.Add("608_GPM", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X");
      nameValueCollection.Add("608_ARM", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "X");
      nameValueCollection.Add("608_Other", str19);
      string str20 = JS.GetStr(loan, "995");
      nameValueCollection.Add("995", str20);
      string str21 = JS.GetStr(loan, "994");
      nameValueCollection.Add("994", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "X");
      nameValueCollection.Add("420_First", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "X");
      nameValueCollection.Add("420_Second", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "690"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "691"), "", false) != 0, "X");
      nameValueCollection.Add("690_Y", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "690"), "", false) == 0 | Operators.CompareString(JS.GetStr(loan, "691"), "", false) == 0, "X");
      nameValueCollection.Add("690_N", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "Y", false) == 0, "X");
      nameValueCollection.Add("675_Y", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "N", false) == 0 | Operators.CompareString(JS.GetStr(loan, "675"), "", false) == 0, "X");
      nameValueCollection.Add("675_N", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) != 0 & Jed.Num(JS.GetNum(loan, "325")) < Jed.Num(JS.GetNum(loan, "4")), "X");
      nameValueCollection.Add("325_Y", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) == 0 | Jed.Num(JS.GetNum(loan, "325")) == Jed.Num(JS.GetNum(loan, "4")), "X");
      nameValueCollection.Add("325_N", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) != 0 & Jed.Num(JS.GetNum(loan, "325")) < Jed.Num(JS.GetNum(loan, "4")), JS.GetStr(loan, "325"));
      nameValueCollection.Add("325_A", str30);
      string str31 = JS.GetStr(loan, "2625");
      nameValueCollection.Add("2625", str31);
      string str32 = JS.GetStr(loan, "688");
      nameValueCollection.Add("688", str32);
      string str33 = JS.GetStr(loan, "689");
      nameValueCollection.Add("689", str33);
      string str34 = JS.GetStr(loan, "DISCLOSURE.X168");
      nameValueCollection.Add("DISCLOSURE_X168", str34);
      string str35 = JS.GetStr(loan, "696");
      nameValueCollection.Add("696", str35);
      string str36 = JS.GetStr(loan, "694");
      nameValueCollection.Add("694", str36);
      string str37 = JS.GetStr(loan, "697");
      nameValueCollection.Add("697", str37);
      string str38 = JS.GetStr(loan, "695");
      nameValueCollection.Add("695", str38);
      string str39 = Jed.NF(Jed.Num(JS.GetNum(loan, "RE88395.X152")), (byte) 18, 0);
      nameValueCollection.Add("RE88395_X152", str39);
      string str40 = Jed.NF(Jed.Num(JS.GetNum(loan, "FLGFE.X77")), (byte) 18, 0);
      nameValueCollection.Add("FLGFE_X77", str40);
      string str41 = Jed.NF(Jed.Num(JS.GetNum(loan, "FLGFE.X53")), (byte) 18, 0);
      nameValueCollection.Add("FLGFE_X53", str41);
      string str42 = Jed.NF(Jed.Num(JS.GetNum(loan, "FLGFE.X54")), (byte) 18, 0);
      nameValueCollection.Add("FLGFE_X54", str42);
      string str43 = JS.GetStr(loan, "DISCLOSURE.X170");
      nameValueCollection.Add("DISCLOSURE_X170", str43);
      string str44 = Jed.BF(Jed.Num(JS.GetNum(loan, "142")) < 0.0, "to", "from");
      nameValueCollection.Add("FROMTO", str44);
      string str45 = Jed.BF(Jed.Num(JS.GetNum(loan, "142")) < 0.0, Jed.NF(Jed.Num(JS.GetNum(loan, "142")) * -1.0, (byte) 18, 0), Jed.NF(Jed.Num(JS.GetNum(loan, "142")), (byte) 18, 0));
      nameValueCollection.Add("142", str45);
      return nameValueCollection;
    }
  }
}
