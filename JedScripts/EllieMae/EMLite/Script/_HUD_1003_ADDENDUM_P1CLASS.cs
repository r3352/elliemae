// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_1003_ADDENDUM_P1CLASS
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
  public class _HUD_1003_ADDENDUM_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1711"), "HUD / FHA", false) == 0, "X");
      nameValueCollection.Add("1711_HUD/FHA", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1711"), "V.A.", false) == 0, "X");
      nameValueCollection.Add("1711_VA", str2);
      string str3 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str3);
      string str4 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305", str4);
      string str5 = JS.GetStr(loan, "1039");
      nameValueCollection.Add("1039", str5);
      string str6 = JS.GetStr(loan, "1093");
      nameValueCollection.Add("1093", str6);
      string str7 = JS.GetStr(loan, "969");
      nameValueCollection.Add("1160", str7);
      string str8 = JS.GetStr(loan, "232");
      nameValueCollection.Add("232", str8);
      string str9 = "   " + JS.GetStr(loan, "409");
      nameValueCollection.Add("409", str9);
      string str10 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str10);
      string str11 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str11);
      string str12 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str12);
      string str13 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str13);
      string str14 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str14);
      string str15 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str15);
      string str16 = JS.GetStr(loan, "1347");
      nameValueCollection.Add("1347", str16);
      string str17 = JS.GetStr(loan, "1392");
      nameValueCollection.Add("1392", str17);
      string str18 = JS.GetStr(loan, "1059");
      nameValueCollection.Add("1059", str18);
      string str19 = JS.GetStr(loan, "1060");
      nameValueCollection.Add("1060", str19);
      string str20 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str20);
      string str21 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str21);
      string str22 = JS.GetStr(loan, "765");
      nameValueCollection.Add("765", str22);
      string str23 = JS.GetStr(loan, "3632");
      nameValueCollection.Add("3632", str23);
      string str24 = JS.GetStr(loan, "3633");
      nameValueCollection.Add("3633", str24);
      string str25 = JS.GetStr(loan, "3634") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3635"), "", false) != 0, ", ") + JS.GetStr(loan, "3635") + " " + JS.GetStr(loan, "3636");
      nameValueCollection.Add("3634_3635_3636", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3637"), "", false) != 0, "NMLS ID: " + JS.GetStr(loan, "3637") + "    ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3638"), "", false) != 0, "Tax ID: " + JS.GetStr(loan, "3638"));
      nameValueCollection.Add("3637_3638", str26);
      string str27 = JS.GetStr(loan, "1111");
      nameValueCollection.Add("1111", str27);
      string str28 = JS.GetStr(loan, "1113");
      nameValueCollection.Add("1113", str28);
      string str29 = JS.GetStr(loan, "1114") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1743"), "", false) != 0, ", ") + JS.GetStr(loan, "1743") + " " + JS.GetStr(loan, "1744");
      nameValueCollection.Add("1114", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3640"), "", false) != 0, "NMLS ID: " + JS.GetStr(loan, "3640") + "    ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3641"), "", false) != 0, "Tax ID: " + JS.GetStr(loan, "3641"));
      nameValueCollection.Add("3640_3641", str30);
      string str31 = JS.GetStr(loan, "1262");
      nameValueCollection.Add("324", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "934"), "Y", false) == 0, "X");
      nameValueCollection.Add("934_Yes", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "934"), "N", false) == 0, "X");
      nameValueCollection.Add("934_No", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1497"), "Veteran", false) == 0, "X");
      nameValueCollection.Add("1497_Veteran", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1497"), "VeteranAndSpouse", false) == 0, "X");
      nameValueCollection.Add("1497_VeteranSpouse", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1497"), "Other", false) == 0, "X");
      nameValueCollection.Add("1497_Other", str36);
      string str37 = JS.GetStr(loan, "1064");
      nameValueCollection.Add("1064", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseHomePrevOccupied", false) == 0, "X");
      nameValueCollection.Add("28_PurchExist", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "FinanceImprovement", false) == 0, "X");
      nameValueCollection.Add("28_FinanceImpr", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "Refinance", false) == 0, "X");
      nameValueCollection.Add("28_Refi", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseNewCondoUnit", false) == 0, "X");
      nameValueCollection.Add("28_PurchNewCondo", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseExistingCondoUnit", false) == 0, "X");
      nameValueCollection.Add("28_PurchExistCondo", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseHomeNotPrevOccupied", false) == 0, "X");
      nameValueCollection.Add("28_PurchExistHome", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "ConstructionHome", false) == 0, "X");
      nameValueCollection.Add("28_ConstrctHome", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "FinanceCoOpPurchase", false) == 0, "X");
      nameValueCollection.Add("28_FinanceCoop", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseManufacturedHome", false) == 0, "X");
      nameValueCollection.Add("28_PurchPermMF", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseManufacturedHomeAndLot", false) == 0, "X");
      nameValueCollection.Add("28_PurchPermMF&Lot", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "RefinanceManufacturedHomeToBuyLot", false) == 0, "X");
      nameValueCollection.Add("28_RefiPermMFBuyLot", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "RefinanceManufacturedHomeLotLoan", false) == 0, "X");
      nameValueCollection.Add("28_RefiPermMFHomeLot", str49);
      string str50 = JS.GetStr(loan, "322");
      nameValueCollection.Add("322", str50);
      string str51 = JS.GetStr(loan, "1795");
      nameValueCollection.Add("1795", str51);
      string str52 = JS.GetStr(loan, "1796");
      nameValueCollection.Add("1796", str52);
      string str53 = JS.GetStr(loan, "684");
      nameValueCollection.Add("684", str53);
      string str54 = JS.GetStr(loan, "1797");
      nameValueCollection.Add("1797", str54);
      string str55 = JS.GetStr(loan, "1798");
      nameValueCollection.Add("1798", str55);
      return nameValueCollection;
    }
  }
}
