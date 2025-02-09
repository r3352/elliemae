// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_1820_LOAN_DISBURSEMENT_CERT_P1CLASS
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
  public class _VA_26_1820_LOAN_DISBURSEMENT_CERT_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X51"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X51", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X52"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X52", str2);
      string str3 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str3);
      string str4 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str4);
      string str5 = JS.GetStr(loan, "1059");
      nameValueCollection.Add("1059", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "L770"), "//", false) != 0, JS.GetStr(loan, "L770"));
      nameValueCollection.Add("L770", str6);
      string str7 = JS.GetStr(loan, "VASUMM.X32") + " " + JS.GetStr(loan, "VASUMM.X33");
      nameValueCollection.Add("VASUMM_X32_VASUMM_X33", str7);
      string str8 = JS.GetStr(loan, "VASUMM.X34");
      nameValueCollection.Add("VASUMM_X34", str8);
      string str9 = JS.GetStr(loan, "VAVOB.X44");
      nameValueCollection.Add("VAVOB_X44", str9);
      string str10 = JS.GetStr(loan, "VAVOB.X45") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X46"), "", false) != 0, ", ") + JS.GetStr(loan, "VAVOB.X46") + " " + JS.GetStr(loan, "VAVOB.X47");
      nameValueCollection.Add("VAVOB_X45_VAVOB_X46_VAVOB_X47", str10);
      string str11 = JS.GetStr(loan, "CAPIAP.X9");
      nameValueCollection.Add("CAPIAPX9", str11);
      string str12 = JS.GetStr(loan, "CAPIAP.X10");
      nameValueCollection.Add("CAPIAPX10", str12);
      string str13 = JS.GetStr(loan, "CAPIAP.X11") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X12"), "", false) != 0, ", ") + JS.GetStr(loan, "CAPIAP.X12") + " " + JS.GetStr(loan, "CAPIAP.X13");
      nameValueCollection.Add("CAPIAPX11_CAPIAPX12_CAPIAPX13", str13);
      string str14 = JS.GetStr(loan, "CAPIAP.X15");
      nameValueCollection.Add("CAPIAPX15", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3642"), "Guaranty", false) == 0, "X");
      nameValueCollection.Add("3642_Guaranty", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3642"), "Insurance", false) == 0, "X");
      nameValueCollection.Add("3642_Insurance", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseHomePrevOccupied", false) == 0, "X");
      nameValueCollection.Add("28_PurchHmPrevOcc", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "FinanceImprovement", false) == 0, "X");
      nameValueCollection.Add("28_FinanImpExist", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseNewCondoUnit", false) == 0, "X");
      nameValueCollection.Add("28_PurchNewCondo", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseExistingCondoUnit", false) == 0, "X");
      nameValueCollection.Add("28_PurchExistCndo", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "Refinance", false) == 0, "X");
      nameValueCollection.Add("28_Refi", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseHomeNotPrevOccupied", false) == 0, "X");
      nameValueCollection.Add("28_PurchHmNotOcc", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseManufacturedHome", false) == 0, "X");
      nameValueCollection.Add("28_PurchPermMF", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "ConstructionHome", false) == 0, "X");
      nameValueCollection.Add("28_ConstrHome", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "PurchaseManufacturedHomeAndLot", false) == 0, "X");
      nameValueCollection.Add("28_PurchPermMF&Lot", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "RefinanceManufacturedHomeToBuyLot", false) == 0, "X");
      nameValueCollection.Add("28_RefiMF", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "28"), "RefinanceManufacturedHomeLotLoan", false) == 0, "X");
      nameValueCollection.Add("28_RefiMFLotLn", str27);
      string str28 = JS.GetStr(loan, "11") + Jed.BF(true, ", ") + JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15") + ", Lot#: " + JS.GetStr(loan, "2973") + ", Block#: " + JS.GetStr(loan, "2974") + ", Subdivision: " + JS.GetStr(loan, "1079");
      nameValueCollection.Add("11_12_14", str28);
      string str29 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str29);
      string str30 = JS.GetStr(loan, "5");
      nameValueCollection.Add("5", str30);
      string str31 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "L770"), "//", false) != 0, JS.GetStr(loan, "L770"));
      nameValueCollection.Add("L770_a", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "682"), "//", false) != 0, JS.GetStr(loan, "682"));
      nameValueCollection.Add("682", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "748"), "//", false) != 0, JS.GetStr(loan, "748"));
      nameValueCollection.Add("748", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "2553"), "//", false) != 0, JS.GetStr(loan, "2553"));
      nameValueCollection.Add("2553", str35);
      string str36 = JS.GetStr(loan, "1347");
      nameValueCollection.Add("1347", str36);
      string str37 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "78"), "//", false) != 0, JS.GetStr(loan, "78"));
      nameValueCollection.Add("78", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "X");
      nameValueCollection.Add("420_FirstReal", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "X");
      nameValueCollection.Add("420_2ndReal", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X53"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X53", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X54"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X54", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X55"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X55", str43);
      string str44 = JS.GetStr(loan, "VASUMM.X56");
      nameValueCollection.Add("VASUMM_X56", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1497"), "Veteran", false) == 0, "X");
      nameValueCollection.Add("1497_Veteran", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1497"), "VeteranAndSpouse", false) == 0, "X");
      nameValueCollection.Add("1497_Vet&Spouse", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1497"), "Other", false) == 0, "X");
      nameValueCollection.Add("1497_Other", str47);
      string str48 = JS.GetStr(loan, "1064");
      nameValueCollection.Add("1064", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1066"), "FeeSimple", false) == 0, "X");
      nameValueCollection.Add("1066_FeeSimp", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1066"), "Leasehold", false) == 0, "X");
      nameValueCollection.Add("1066_Leasehold", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X57"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X57", str51);
      string str52 = JS.GetStr(loan, "VASUMM.X58");
      nameValueCollection.Add("VASUMM_X58", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1034"), "//", false) != 0, JS.GetStr(loan, "1034"));
      nameValueCollection.Add("1034", str53);
      string str54 = JS.GetStr(loan, "1031");
      nameValueCollection.Add("1031", str54);
      string str55 = JS.GetStr(loan, "VEND.X445");
      nameValueCollection.Add("VEND_X445", str55);
      string str56 = JS.GetStr(loan, "HUD42");
      nameValueCollection.Add("HUD42", str56);
      string str57 = JS.GetStr(loan, "VEND.X446");
      nameValueCollection.Add("VEND_X446", str57);
      string str58 = JS.GetStr(loan, "HUD44");
      nameValueCollection.Add("HUD44", str58);
      string str59 = Jed.NF(12.0 * Jed.S2N(JS.GetStr(loan, "230")), (byte) 18, 0);
      nameValueCollection.Add("642", str59);
      string str60 = Jed.NF(12.0 * Jed.S2N(JS.GetStr(loan, "235")), (byte) 18, 0);
      nameValueCollection.Add("643", str60);
      string str61 = JS.GetStr(loan, "VASUMM.X59");
      nameValueCollection.Add("VASUMM_X59", str61);
      string str62 = JS.GetStr(loan, "VASUMM.X60");
      nameValueCollection.Add("VASUMM_X60", str62);
      string str63 = JS.GetStr(loan, "3643");
      nameValueCollection.Add("3643", str63);
      string str64 = JS.GetStr(loan, "VASUMM.X63");
      nameValueCollection.Add("VASUMM_X63", str64);
      string str65 = JS.GetStr(loan, "VASUMM.X64");
      nameValueCollection.Add("VASUMM_X64", str65);
      string str66 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X65"), "Escrow", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X65_Escrow", str66);
      string str67 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X65"), "Earmarked Account", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X65_Earmarked", str67);
      string str68 = JS.GetStr(loan, "VASUMM.X66");
      nameValueCollection.Add("VASUMM_X66", str68);
      string str69 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X67"), "Y", false) == 0, "X");
      nameValueCollection.Add("VASUMM_X67", str69);
      string str70 = JS.GetStr(loan, "322");
      nameValueCollection.Add("322", str70);
      string str71 = JS.GetStr(loan, "1795");
      nameValueCollection.Add("1795", str71);
      string str72 = JS.GetStr(loan, "1796");
      nameValueCollection.Add("1796", str72);
      string str73 = JS.GetStr(loan, "684");
      nameValueCollection.Add("684", str73);
      string str74 = JS.GetStr(loan, "1797");
      nameValueCollection.Add("1797", str74);
      string str75 = JS.GetStr(loan, "1798");
      nameValueCollection.Add("1798", str75);
      return nameValueCollection;
    }
  }
}
