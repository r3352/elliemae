// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._MTG_LOAN_COMM_P1CLASS
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
  public class _MTG_LOAN_COMM_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "3094");
      nameValueCollection.Add("3094", str1);
      string str2 = JS.GetStr(loan, "364");
      nameValueCollection.Add("901", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("150_151", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "second mortgage", "first mortgage");
      nameValueCollection.Add("420", str5);
      string str6 = JS.GetStr(loan, "363");
      nameValueCollection.Add("7354", str6);
      string str7 = JS.GetStr(loan, "11");
      nameValueCollection.Add("31", str7);
      string str8 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("32_33_34", str8);
      string str9 = JS.GetStr(loan, "2");
      nameValueCollection.Add("11", str9);
      string str10 = JS.GetStr(loan, "3");
      nameValueCollection.Add("12", str10);
      string str11 = JS.GetStr(loan, "353");
      nameValueCollection.Add("540", str11);
      string str12 = JS.GetStr(loan, "4") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) != 0, " / ") + JS.GetStr(loan, "325");
      nameValueCollection.Add("13_3190", str12);
      string str13 = JS.GetStr(loan, "1072");
      nameValueCollection.Add("7369", str13);
      string str14 = JS.GetStr(loan, "976");
      nameValueCollection.Add("541", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("608_1", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "X");
      nameValueCollection.Add("608_2", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, " X");
      nameValueCollection.Add("608_3", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "X");
      nameValueCollection.Add("608_4", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, JS.GetStr(loan, "4"));
      nameValueCollection.Add("4", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, JS.GetStr(loan, "5"));
      nameValueCollection.Add("5", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, JS.GetStr(loan, "994"));
      nameValueCollection.Add("994", str21);
      string str22 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101_2", str22);
      string str23 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("150_151_2", str23);
      string str24 = JS.GetStr(loan, "NOTICES.X31");
      nameValueCollection.Add("NOTICESX31", str24);
      string str25 = JS.GetStr(loan, "NOTICES.X32");
      nameValueCollection.Add("NOTICESX32", str25);
      string str26 = JS.GetStr(loan, "NOTICES.X33");
      nameValueCollection.Add("NOTICESX33", str26);
      string str27 = JS.GetStr(loan, "NOTICES.X34") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NOTICES.X35"), "", false) != 0, ", ") + JS.GetStr(loan, "NOTICES.X35") + " " + JS.GetStr(loan, "NOTICES.X36");
      nameValueCollection.Add("NOTICESX34_35_36", str27);
      string str28 = JS.GetStr(loan, "NOTICES.X37");
      nameValueCollection.Add("NOTICESX37", str28);
      string str29 = JS.GetStr(loan, "NOTICES.X47");
      nameValueCollection.Add("NOTICESX47", str29);
      return nameValueCollection;
    }
  }
}
