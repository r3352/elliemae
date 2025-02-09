// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ESTIMATEDTRANSACTIONSUMMARYCLASS
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
  public class _ESTIMATEDTRANSACTIONSUMMARYCLASS
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
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "FarmersHomeAdministration", false) == 0, "USDA-RHS", JS.GetStr(loan, "1172"));
      nameValueCollection.Add("1172", str5);
      string str6 = JS.GetStr(loan, "1401");
      nameValueCollection.Add("1401", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "4"), "", false) != 0, JS.GetStr(loan, "4") + " months", "@");
      nameValueCollection.Add("4", str7);
      string str8 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X293"), JS.GetStr(loan, "315"));
      nameValueCollection.Add("315", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X302"), JS.GetStr(loan, "317"));
      nameValueCollection.Add("317", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X294"), JS.GetStr(loan, "319"));
      nameValueCollection.Add("319", str11);
      string str12 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "VEND.X295") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X296"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X296") + " " + JS.GetStr(loan, "VEND.X297"), JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323"));
      nameValueCollection.Add("313_321_323", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3"), "", false) != 0, JS.GetStr(loan, "3") + " %", "");
      nameValueCollection.Add("3", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "Fixed Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "GraduatedPaymentMortgage", false) == 0, "GPM - Rate", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "OtherAmortizationType", false) == 0, "Other", ""))));
      nameValueCollection.Add("608", str14);
      string str15 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str15);
      string str16 = JS.GetStr(loan, "228");
      nameValueCollection.Add("228", str16);
      string str17 = JS.GetStr(loan, "229");
      nameValueCollection.Add("229", str17);
      string str18 = JS.GetStr(loan, "230");
      nameValueCollection.Add("230", str18);
      string str19 = JS.GetStr(loan, "1405");
      nameValueCollection.Add("1405", str19);
      string str20 = JS.GetStr(loan, "232");
      nameValueCollection.Add("232", str20);
      string str21 = JS.GetStr(loan, "233");
      nameValueCollection.Add("233", str21);
      string str22 = JS.GetStr(loan, "234");
      nameValueCollection.Add("234", str22);
      string str23 = JS.GetStr(loan, "912");
      nameValueCollection.Add("912", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0, JS.GetStr(loan, "1092"), JS.GetStr(loan, "136"));
      nameValueCollection.Add("136", str24);
      string str25 = JS.GetStr(loan, "137");
      nameValueCollection.Add("137", str25);
      string str26 = JS.GetStr(loan, "138");
      nameValueCollection.Add("138", str26);
      string str27 = JS.GetStr(loan, "1093");
      nameValueCollection.Add("1093", str27);
      string str28 = JS.GetStr(loan, "969");
      nameValueCollection.Add("969", str28);
      string str29 = JS.GetStr(loan, "1073");
      nameValueCollection.Add("1073", str29);
      string str30 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109_a", str30);
      string str31 = JS.GetStr(loan, "TNBPCC");
      nameValueCollection.Add("TNBPCC", str31);
      string str32 = JS.GetStr(loan, "1045");
      nameValueCollection.Add("1045", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "202"), "Other", false) == 0, "Other", JS.GetStr(loan, "202")))))))));
      nameValueCollection.Add("202", str33);
      string str34 = JS.GetStr(loan, "141");
      nameValueCollection.Add("141", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1091"), "Other", false) == 0, "Other", JS.GetStr(loan, "1091")))))))));
      nameValueCollection.Add("1091", str35);
      string str36 = JS.GetStr(loan, "1095");
      nameValueCollection.Add("1095", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1106"), "Other", false) == 0, "Other", JS.GetStr(loan, "1106")))))))));
      nameValueCollection.Add("1106", str37);
      string str38 = JS.GetStr(loan, "1115");
      nameValueCollection.Add("1115", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "RelocationFunds", false) == 0, "Relocation Funds", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "EmployerAssistedHousing", false) == 0, "Employer Assisted Housing", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "LeasePurchaseFund", false) == 0, "Lease Purchase Fund", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "CashDepositOnSalesContract", false) == 0, "Cash Deposit On Sales Contract", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "SellerCredit", false) == 0, "Seller Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "LenderCredit", false) == 0, "Lender Credit", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "BorrowerPaidFees", false) == 0, "Borrower Paid Fees", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1646"), "Other", false) == 0, "Other", JS.GetStr(loan, "1646")))))))));
      nameValueCollection.Add("1646", str39);
      string str40 = JS.GetStr(loan, "1647");
      nameValueCollection.Add("1647", str40);
      string str41 = JS.GetStr(loan, "1845");
      nameValueCollection.Add("1845", str41);
      string str42 = JS.GetStr(loan, "140");
      nameValueCollection.Add("140", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "1st Lien", "2nd Lien");
      nameValueCollection.Add("CC_FROM", str43);
      string str44 = JS.GetStr(loan, "1851");
      nameValueCollection.Add("1851", str44);
      string str45 = JS.GetStr(loan, "1852");
      nameValueCollection.Add("1852", str45);
      string str46 = JS.GetStr(loan, "1844");
      nameValueCollection.Add("1844", str46);
      string str47 = Jed.BF(Jed.S2N(JS.GetStr(loan, "142")) >= 0.0, "from", "to");
      nameValueCollection.Add("CASH_FROM_TO", str47);
      string str48 = JS.GetStr(loan, "142");
      nameValueCollection.Add("142", str48);
      string str49 = JS.GetStr(loan, "BORPCC");
      nameValueCollection.Add("BORPCC", str49);
      string str50 = JS.GetStr(loan, "TNBPCC");
      nameValueCollection.Add("TNBPCC_a", str50);
      string str51 = JS.GetStr(loan, "TOTPCC");
      nameValueCollection.Add("TOTPCC", str51);
      string str52 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str52);
      string str53 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str53);
      return nameValueCollection;
    }
  }
}
