﻿// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ITEMIZEDFEEWORKSHEETP2CLASS
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
  public class _ITEMIZEDFEEWORKSHEETP2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str1);
      string str2 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str2);
      string str3 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str3);
      string str4 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "2026"), "", false) != 0, JS.GetStr(loan, "2026"), JS.GetStr(loan, "317")) + " " + Jed.BF(Operators.CompareString(JS.GetStr(loan, "2026"), "", false) != 0, JS.GetStr(loan, "2027"), JS.GetStr(loan, "324"));
      nameValueCollection.Add("317_324", str5);
      string str6 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str6);
      string str7 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str7);
      string str8 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str8);
      string str9 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str9);
      string str10 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str10);
      string str11 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str11);
      string str12 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str12);
      string str13 = JS.GetStr(loan, "1401");
      nameValueCollection.Add("1401", str13);
      string str14 = JS.GetStr(loan, "3") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3"), "", false) != 0, " %");
      nameValueCollection.Add("3", str14);
      string str15 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str15);
      string str16 = JS.GetStr(loan, "1172");
      nameValueCollection.Add("1172", str16);
      string str17 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str17);
      string str18 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str18);
      string str19 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str19);
      string str20 = JS.GetStr(loan, "332");
      nameValueCollection.Add("332", str20);
      string str21 = JS.GetStr(loan, "333");
      nameValueCollection.Add("333", str21);
      string str22 = JS.GetStr(loan, "NEWHUD.X582") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1062"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1062"), "");
      nameValueCollection.Add("NEWHUD_X582", str22);
      string str23 = JS.GetStr(loan, "L259") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1063"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1063"), "");
      nameValueCollection.Add("L259", str23);
      string str24 = JS.GetStr(loan, "1666") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1064"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1064"), "");
      nameValueCollection.Add("1666", str24);
      string str25 = JS.GetStr(loan, "NEWHUD.X583") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1065"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1065"), "");
      nameValueCollection.Add("NEWHUD_X583", str25);
      string str26 = JS.GetStr(loan, "L248");
      nameValueCollection.Add("L248", str26);
      string str27 = JS.GetStr(loan, "L252");
      nameValueCollection.Add("L252", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1956"), "", false) != 0, "to " + JS.GetStr(loan, "1956"), "");
      nameValueCollection.Add("1956", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1500"), "", false) != 0, "to " + JS.GetStr(loan, "1500"), "");
      nameValueCollection.Add("1500", str29);
      string str30 = JS.GetStr(loan, "NEWHUD.X584") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1066"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1066"), "");
      nameValueCollection.Add("NEWHUD_X584", str30);
      string str31 = JS.GetStr(loan, "NEWHUD.X1586") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1587"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1587"), "");
      nameValueCollection.Add("NEWHUD_X1586", str31);
      string str32 = JS.GetStr(loan, "NEWHUD.X1594") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1595"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1595"), "");
      nameValueCollection.Add("NEWHUD_X1594", str32);
      string str33 = JS.GetStr(loan, "NEWHUD.X1716");
      nameValueCollection.Add("NEWHUD_X1716", str33);
      string str34 = JS.GetStr(loan, "1387");
      nameValueCollection.Add("1387", str34);
      string str35 = JS.GetStr(loan, "230");
      nameValueCollection.Add("230", str35);
      string str36 = JS.GetStr(loan, "1296");
      nameValueCollection.Add("1296", str36);
      string str37 = JS.GetStr(loan, "232");
      nameValueCollection.Add("232", str37);
      string str38 = JS.GetStr(loan, "1386");
      nameValueCollection.Add("1386", str38);
      string str39 = JS.GetStr(loan, "231");
      nameValueCollection.Add("231", str39);
      string str40 = JS.GetStr(loan, "L267");
      nameValueCollection.Add("L267", str40);
      string str41 = JS.GetStr(loan, "L268");
      nameValueCollection.Add("L268", str41);
      string str42 = JS.GetStr(loan, "1388");
      nameValueCollection.Add("1388", str42);
      string str43 = JS.GetStr(loan, "235");
      nameValueCollection.Add("235", str43);
      string str44 = JS.GetStr(loan, "1628") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X341"), "", false) != 0, " to " + JS.GetStr(loan, "VEND.X341"), "");
      nameValueCollection.Add("1628", str44);
      string str45 = JS.GetStr(loan, "1629");
      nameValueCollection.Add("1629", str45);
      string str46 = JS.GetStr(loan, "1630");
      nameValueCollection.Add("1630", str46);
      string str47 = JS.GetStr(loan, "660") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X350"), "", false) != 0, " to " + JS.GetStr(loan, "VEND.X350"), "");
      nameValueCollection.Add("660", str47);
      string str48 = JS.GetStr(loan, "340");
      nameValueCollection.Add("340", str48);
      string str49 = JS.GetStr(loan, "253");
      nameValueCollection.Add("253", str49);
      string str50 = JS.GetStr(loan, "661") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X359"), "", false) != 0, " to " + JS.GetStr(loan, "VEND.X359"), "");
      nameValueCollection.Add("661", str50);
      string str51 = JS.GetStr(loan, "341");
      nameValueCollection.Add("341", str51);
      string str52 = JS.GetStr(loan, "254");
      nameValueCollection.Add("254", str52);
      string str53 = JS.GetStr(loan, "558");
      nameValueCollection.Add("558", str53);
      string str54 = JS.GetStr(loan, "NEWHUD.X1706");
      nameValueCollection.Add("NEWHUD_X1706", str54);
      string str55 = JS.GetStr(loan, "NEWHUD.X1707");
      nameValueCollection.Add("NEWHUD_X1707", str55);
      string str56 = JS.GetStr(loan, "228");
      nameValueCollection.Add("228", str56);
      string str57 = JS.GetStr(loan, "229");
      nameValueCollection.Add("229", str57);
      string str58 = JS.GetStr(loan, "230");
      nameValueCollection.Add("230_a", str58);
      string str59 = JS.GetStr(loan, "1405");
      nameValueCollection.Add("1405", str59);
      string str60 = JS.GetStr(loan, "232");
      nameValueCollection.Add("232_a", str60);
      string str61 = JS.GetStr(loan, "233");
      nameValueCollection.Add("233", str61);
      string str62 = JS.GetStr(loan, "234");
      nameValueCollection.Add("234", str62);
      string str63 = JS.GetStr(loan, "912");
      nameValueCollection.Add("912", str63);
      string str64 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, JS.GetStr(loan, "136"), JS.GetStr(loan, "1092"));
      nameValueCollection.Add("136_a", str64);
      string str65 = JS.GetStr(loan, "137");
      nameValueCollection.Add("137_a", str65);
      string str66 = JS.GetStr(loan, "138");
      nameValueCollection.Add("138_a", str66);
      string str67 = JS.GetStr(loan, "1093");
      nameValueCollection.Add("1093_a", str67);
      string str68 = JS.GetStr(loan, "969");
      nameValueCollection.Add("969", str68);
      string str69 = JS.GetStr(loan, "1073");
      nameValueCollection.Add("1073", str69);
      string str70 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109_2", str70);
      string str71 = JS.GetStr(loan, "TNBPCC");
      nameValueCollection.Add("TNBPCC", str71);
      string str72 = JS.GetStr(loan, "1045");
      nameValueCollection.Add("1045", str72);
      string str73 = JS.GetStr(loan, "141");
      nameValueCollection.Add("141", str73);
      string str74 = JS.GetStr(loan, "1095");
      nameValueCollection.Add("1095", str74);
      string str75 = JS.GetStr(loan, "1115");
      nameValueCollection.Add("1115", str75);
      string str76 = JS.GetStr(loan, "1647");
      nameValueCollection.Add("1647", str76);
      string str77 = JS.GetStr(loan, "1845");
      nameValueCollection.Add("1845", str77);
      string str78 = JS.GetStr(loan, "140");
      nameValueCollection.Add("140", str78);
      string str79 = JS.GetStr(loan, "1851");
      nameValueCollection.Add("1851", str79);
      string str80 = JS.GetStr(loan, "1844");
      nameValueCollection.Add("1844", str80);
      string str81 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "Other", false) == 0, "Other", JS.GetStr(loan, "202")))))))));
      nameValueCollection.Add("202", str81);
      string str82 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "Other", false) == 0, "Other", JS.GetStr(loan, "1091")))))))));
      nameValueCollection.Add("1091", str82);
      string str83 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "Other", false) == 0, "Other", JS.GetStr(loan, "1106")))))))));
      nameValueCollection.Add("1106", str83);
      string str84 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "Other", false) == 0, "Other", JS.GetStr(loan, "1646")))))))));
      nameValueCollection.Add("1646", str84);
      string str85 = JS.GetStr(loan, "TNBPCC");
      nameValueCollection.Add("TNBPCC_a", str85);
      string str86 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "1st Lien", "2nd Lien");
      nameValueCollection.Add("CC_FROM", str86);
      string str87 = JS.GetStr(loan, "NEWHUD.X1149");
      nameValueCollection.Add("NEWHUD_X1149_a", str87);
      string str88 = JS.GetStr(loan, "142");
      nameValueCollection.Add("142", str88);
      string str89 = Jed.BF(Jed.S2N(JS.GetStr(loan, "142")) >= 0.0, "Cash from borrower", "Cash to borrower");
      nameValueCollection.Add("CASH_FROM_TO_BORROWER", str89);
      string str90 = JS.GetStr(loan, "NEWHUD.X1149");
      nameValueCollection.Add("NEWHUD_X1149", str90);
      string str91 = JS.GetStr(loan, "BORPCC");
      nameValueCollection.Add("BORPCC", str91);
      string str92 = JS.GetStr(loan, "TOTPCC");
      nameValueCollection.Add("TOTPCC", str92);
      string str93 = JS.GetStr(loan, "138");
      nameValueCollection.Add("TOTAL_PREPAID", str93);
      string str94 = JS.GetStr(loan, "137");
      nameValueCollection.Add("TOTAL_CC", str94);
      string str95 = JS.GetStr(loan, "334");
      nameValueCollection.Add("334", str95);
      string str96 = JS.GetStr(loan, "337");
      nameValueCollection.Add("337", str96);
      string str97 = JS.GetStr(loan, "642");
      nameValueCollection.Add("642", str97);
      string str98 = JS.GetStr(loan, "NEWHUD.X591");
      nameValueCollection.Add("NEWHUD_X591", str98);
      string str99 = JS.GetStr(loan, "1050");
      nameValueCollection.Add("1050", str99);
      string str100 = JS.GetStr(loan, "643");
      nameValueCollection.Add("643", str100);
      string str101 = JS.GetStr(loan, "L260");
      nameValueCollection.Add("L260", str101);
      string str102 = JS.GetStr(loan, "1667");
      nameValueCollection.Add("1667", str102);
      string str103 = JS.GetStr(loan, "NEWHUD.X592");
      nameValueCollection.Add("NEWHUD_X592", str103);
      string str104 = JS.GetStr(loan, "NEWHUD.X593");
      nameValueCollection.Add("NEWHUD_X593", str104);
      string str105 = JS.GetStr(loan, "NEWHUD.X1588");
      nameValueCollection.Add("NEWHUD_X1588", str105);
      string str106 = JS.GetStr(loan, "NEWHUD.X1596");
      nameValueCollection.Add("NEWHUD_X1596", str106);
      string str107 = JS.GetStr(loan, "656");
      nameValueCollection.Add("656", str107);
      string str108 = JS.GetStr(loan, "338");
      nameValueCollection.Add("338", str108);
      string str109 = JS.GetStr(loan, "655");
      nameValueCollection.Add("655", str109);
      string str110 = JS.GetStr(loan, "L269");
      nameValueCollection.Add("L269", str110);
      string str111 = JS.GetStr(loan, "657");
      nameValueCollection.Add("657", str111);
      string str112 = JS.GetStr(loan, "1631");
      nameValueCollection.Add("1631", str112);
      string str113 = JS.GetStr(loan, "658");
      nameValueCollection.Add("658", str113);
      string str114 = JS.GetStr(loan, "659");
      nameValueCollection.Add("659", str114);
      string str115 = JS.GetStr(loan, "NEWHUD.X1708");
      nameValueCollection.Add("NEWHUD_X1708", str115);
      string str116 = JS.GetStr(loan, "202");
      nameValueCollection.Add("CC_SUMMARY_1", str116);
      string str117 = JS.GetStr(loan, "1091");
      nameValueCollection.Add("CC_SUMMARY_2", str117);
      string str118 = JS.GetStr(loan, "1106");
      nameValueCollection.Add("CC_SUMMARY_3", str118);
      string str119 = JS.GetStr(loan, "1646");
      nameValueCollection.Add("CC_SUMMARY_4", str119);
      string str120 = JS.GetStr(loan, "141");
      nameValueCollection.Add("CCS_1", str120);
      string str121 = JS.GetStr(loan, "1095");
      nameValueCollection.Add("CCS_2", str121);
      string str122 = JS.GetStr(loan, "1115");
      nameValueCollection.Add("CCS_3", str122);
      string str123 = JS.GetStr(loan, "1647");
      nameValueCollection.Add("CCS_4", str123);
      return nameValueCollection;
    }
  }
}
