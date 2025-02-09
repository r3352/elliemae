// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1003PG1_SPANISHCLASS
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
  public class _1003PG1_SPANISHCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str1);
      string str2 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str2);
      string str3 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "307"), "Y", false) == 0, "X");
      nameValueCollection.Add("307", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "35"), "Y", false) == 0, "X");
      nameValueCollection.Add("35", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "307"), "Y", false) == 0, "X");
      nameValueCollection.Add("307_1", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "35"), "Y", false) == 0, "X");
      nameValueCollection.Add("35_1", str7);
      string str8 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "VA", false) == 0, "X");
      nameValueCollection.Add("1172_VA", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "FHA", false) == 0, "X");
      nameValueCollection.Add("1172_FHA", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "Other", false) == 0, "X");
      nameValueCollection.Add("1172_Oth", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "Conventional", false) == 0, "X");
      nameValueCollection.Add("1172_Conv", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "FarmersHomeAdministration", false) == 0, "X");
      nameValueCollection.Add("1172_FmHA", str13);
      string str14 = JS.GetStr(loan, "1063");
      nameValueCollection.Add("1063", str14);
      string str15 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str15);
      string str16 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str16);
      string str17 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str17);
      string str18 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("608_Fixed", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "X");
      nameValueCollection.Add("608_GPM", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X");
      nameValueCollection.Add("608_ARM", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "X");
      nameValueCollection.Add("608_Other", str22);
      string str23 = JS.GetStr(loan, "995");
      nameValueCollection.Add("995", str23);
      string str24 = JS.GetStr(loan, "994");
      nameValueCollection.Add("994", str24);
      string str25 = JS.GetStr(loan, "11") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "12"), "", false) != 0, ", ") + JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "13"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15") + " County: " + JS.GetStr(loan, "13");
      nameValueCollection.Add("11_12", str25);
      string str26 = JS.GetStr(loan, "18");
      nameValueCollection.Add("18", str26);
      string str27 = JS.GetStr(loan, "16");
      nameValueCollection.Add("16", str27);
      string str28 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str28);
      string str29 = JS.GetStr(loan, "1824");
      nameValueCollection.Add("1824", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0, "X");
      nameValueCollection.Add("19_Const", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_Ref", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, "X");
      nameValueCollection.Add("19_ConstP", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "X");
      nameValueCollection.Add("19_Pur", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Other", false) == 0, "X");
      nameValueCollection.Add("19_Other", str34);
      string str35 = JS.GetStr(loan, "9");
      nameValueCollection.Add("9", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "Investor", false) == 0, "X");
      nameValueCollection.Add("190_Inv", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "PrimaryResidence", false) == 0, "X");
      nameValueCollection.Add("190_Prim", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "SecondHome", false) == 0, "X");
      nameValueCollection.Add("190_Sec", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, JS.GetStr(loan, "20"));
      nameValueCollection.Add("20", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, JS.GetStr(loan, "21"));
      nameValueCollection.Add("21", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, JS.GetStr(loan, "10"));
      nameValueCollection.Add("10", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, JS.GetStr(loan, "22"));
      nameValueCollection.Add("22", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, JS.GetStr(loan, "23"));
      nameValueCollection.Add("23", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, JS.GetStr(loan, "1074"));
      nameValueCollection.Add("1074", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, JS.GetStr(loan, "26"));
      nameValueCollection.Add("26", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, JS.GetStr(loan, "25"));
      nameValueCollection.Add("25", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, JS.GetStr(loan, "24"));
      nameValueCollection.Add("24", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "CashOutDebtConsolidation", false) == 0, "Cash-out Debt Consolidation", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "CashOutHomeImprovement", false) == 0, "Cash-Out Home Improvement", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "CashOutLimited", false) == 0, "Cash-Out Limited", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "CashOutOther", false) == 0, "Cash-Out Other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "ChangeInRateTerm", false) == 0, "No Cash-Out Rate/Term", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "NoCashOutOther", false) == 0, "No Cash-Out Other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "NoCashOutFHAStreamlinedRefinance", false) == 0, "No Cash-Out FHA Streamlined", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "NoCashOutFREOwnedRefinance", false) == 0, "No Cash-Out Freddie Owned", Jed.BF(Operators.CompareString(JS.GetStr(loan, "299"), "NoCashOutStreamlinedRefinance", false) == 0, "No Cash-Out Streamlined", JS.GetStr(loan, "299"))))))))));
      nameValueCollection.Add("299_27", str48);
      string str49 = JS.GetStr(loan, "205");
      nameValueCollection.Add("205", str49);
      string str50 = JS.GetStr(loan, "29");
      nameValueCollection.Add("29", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "30"), "Made", false) == 0, "X");
      nameValueCollection.Add("30_made", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "30"), "ToBeMade", false) == 0, "X");
      nameValueCollection.Add("30_2bemade", str52);
      string str53 = JS.GetStr(loan, "31") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1602"), "", false) != 0, ", ") + JS.GetStr(loan, "1602");
      nameValueCollection.Add("31", str53);
      string str54 = JS.GetStr(loan, "33");
      nameValueCollection.Add("33", str54);
      string str55 = JS.GetStr(loan, "34");
      nameValueCollection.Add("34", str55);
      string str56 = JS.GetStr(loan, "191");
      nameValueCollection.Add("191", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1066"), "FeeSimple", false) == 0, "X");
      nameValueCollection.Add("1066_Simple", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1066"), "Leasehold", false) == 0, "X");
      nameValueCollection.Add("1066_Leasehold", str58);
      string str59 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1034"), "//", false) != 0, JS.GetStr(loan, "1034"), "");
      nameValueCollection.Add("1034", str59);
      return nameValueCollection;
    }
  }
}
