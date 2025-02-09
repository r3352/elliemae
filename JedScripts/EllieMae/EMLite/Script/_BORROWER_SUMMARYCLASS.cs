// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._BORROWER_SUMMARYCLASS
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
  public class _BORROWER_SUMMARYCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = JS.GetStr(loan, "1051");
      nameValueCollection.Add("1051", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1393"), "", false) == 0, "Active Loan", JS.GetStr(loan, "1393"));
      nameValueCollection.Add("1393", str3);
      string str4 = JS.GetStr(loan, "749");
      nameValueCollection.Add("749", str4);
      string str5 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str5);
      string str6 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65", str6);
      string str7 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str7);
      string str8 = JS.GetStr(loan, "FE0117");
      nameValueCollection.Add("FE0128", str8);
      string str9 = JS.GetStr(loan, "1490");
      nameValueCollection.Add("1490", str9);
      string str10 = JS.GetStr(loan, "1188");
      nameValueCollection.Add("1188", str10);
      string str11 = JS.GetStr(loan, "1240");
      nameValueCollection.Add("1240", str11);
      string str12 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str12);
      string str13 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97", str13);
      string str14 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str14);
      string str15 = JS.GetStr(loan, "FE0217");
      nameValueCollection.Add("FE0228", str15);
      string str16 = JS.GetStr(loan, "1480");
      nameValueCollection.Add("1480", str16);
      string str17 = JS.GetStr(loan, "1241");
      nameValueCollection.Add("1241", str17);
      string str18 = JS.GetStr(loan, "1268");
      nameValueCollection.Add("1268", str18);
      string str19 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str19);
      string str20 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str20);
      string str21 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str21);
      string str22 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "PrimaryResidence", false) == 0, "X");
      nameValueCollection.Add("190_PRIMARY", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "SecondHome", false) == 0, "X");
      nameValueCollection.Add("190_SENCOND_HOME", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "Investor", false) == 0, "X");
      nameValueCollection.Add("190_INVESTMENT", str25);
      string str26 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str26);
      string str27 = JS.GetStr(loan, "1771") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1771"), "", false) != 0, "%") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1335"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "1771"), "", false) != 0, " / ") + JS.GetStr(loan, "1335");
      nameValueCollection.Add("1335", str27);
      string str28 = Jed.NF(Jed.Num(JS.GetNum(loan, "356")), (byte) 18, 0);
      nameValueCollection.Add("356", str28);
      string str29 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("2", str29);
      string str30 = JS.GetStr(loan, "3") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3"), "", false) != 0, "%");
      nameValueCollection.Add("3", str30);
      string str31 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str31);
      string str32 = JS.GetStr(loan, "325");
      nameValueCollection.Add("325", str32);
      string str33 = JS.GetStr(loan, "5");
      nameValueCollection.Add("5", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "X");
      nameValueCollection.Add("420_1st", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "X");
      nameValueCollection.Add("420_2nd", str35);
      string str36 = JS.GetStr(loan, "1401");
      nameValueCollection.Add("1401", str36);
      string str37 = JS.GetStr(loan, "1785");
      nameValueCollection.Add("1785", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "761"), "//", false) == 0, "", JS.GetStr(loan, "761"));
      nameValueCollection.Add("761", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "762"), "//", false) == 0, "", JS.GetStr(loan, "762"));
      nameValueCollection.Add("762", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "X");
      nameValueCollection.Add("19_Purchase", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_Cash-Out_Refi", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_No_Cash-Out_Refi", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0, "X");
      nameValueCollection.Add("19_Construction", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, "X");
      nameValueCollection.Add("19_Construction_Perm", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Other", false) == 0, "X");
      nameValueCollection.Add("19_Other", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "Conventional", false) == 0, "X");
      nameValueCollection.Add("1172_Conventional", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "FHA", false) == 0, "X");
      nameValueCollection.Add("1172_FHA", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "VA", false) == 0, "X");
      nameValueCollection.Add("1172_VA", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "FarmersHomeAdministration", false) == 0, "X");
      nameValueCollection.Add("1172_FmHa", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "Other", false) == 0, "X");
      nameValueCollection.Add("1172_Other", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("608_Fixed_Rate", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "X");
      nameValueCollection.Add("608_GPM", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X");
      nameValueCollection.Add("608_ARM", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "X");
      nameValueCollection.Add("608_Other", str54);
      string str55 = JS.GetStr(loan, "1177");
      nameValueCollection.Add("1177", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1853"), "Y", false) == 0, "X");
      nameValueCollection.Add("1853", str56);
      string str57 = JS.GetStr(loan, "740");
      nameValueCollection.Add("740", str57);
      string str58 = JS.GetStr(loan, "742");
      nameValueCollection.Add("742", str58);
      string str59 = JS.GetStr(loan, "353");
      nameValueCollection.Add("353", str59);
      string str60 = JS.GetStr(loan, "976");
      nameValueCollection.Add("976", str60);
      string str61 = JS.GetStr(loan, "1389");
      nameValueCollection.Add("1389", str61);
      string str62 = JS.GetStr(loan, "1731");
      nameValueCollection.Add("1731", str62);
      string str63 = JS.GetStr(loan, "1733");
      nameValueCollection.Add("1733", str63);
      string str64 = "";
      nameValueCollection.Add("BORSUMX1", str64);
      string str65 = "";
      nameValueCollection.Add("BORSUMX2", str65);
      string str66 = "";
      nameValueCollection.Add("BORSUMX3", str66);
      string str67 = "";
      nameValueCollection.Add("BORSUMX4", str67);
      string str68 = "";
      nameValueCollection.Add("BORSUMX5", str68);
      string str69 = "";
      nameValueCollection.Add("BORSUMX6", str69);
      string str70 = "";
      nameValueCollection.Add("BORSUMX7", str70);
      string str71 = "";
      nameValueCollection.Add("BORSUMX8", str71);
      string str72 = "";
      nameValueCollection.Add("BORSUMX9", str72);
      string str73 = JS.GetStr(loan, "1822");
      nameValueCollection.Add("1822", str73);
      return nameValueCollection;
    }
  }
}
