// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._INSURANCE_EVIDENCE_HAZARDCLASS
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
  public class _INSURANCE_EVIDENCE_HAZARDCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "L252");
      nameValueCollection.Add("VENDX156", str1);
      string str2 = JS.GetStr(loan, "VEND.X162");
      nameValueCollection.Add("VENDX162", str2);
      string str3 = JS.GetStr(loan, "VEND.X157");
      nameValueCollection.Add("VENDX157", str3);
      string str4 = JS.GetStr(loan, "VEND.X158") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X158"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X159") + " " + JS.GetStr(loan, "VEND.X160");
      nameValueCollection.Add("VENDX158_VENDX159_VENDX160", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X163"), "", false) != 0, "(P) ") + JS.GetStr(loan, "VEND.X163") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X163"), "", false) != 0, " / ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X165"), "", false) != 0, "(F) ") + JS.GetStr(loan, "VEND.X165");
      nameValueCollection.Add("VENDX718_VENDX1255", str5);
      string str6 = JS.GetStr(loan, "VEND.X164");
      nameValueCollection.Add("VENDX164", str6);
      string str7 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362", str7);
      string str8 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str8);
      string str9 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str9);
      string str10 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "324"), "", false) != 0, "(P) ") + JS.GetStr(loan, "324") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, " / ") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, "(F) ") + JS.GetStr(loan, "326");
      nameValueCollection.Add("324_326", str11);
      string str12 = "";
      nameValueCollection.Add("57", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"), "");
      nameValueCollection.Add("363", str13);
      string str14 = JS.GetStr(loan, "305");
      nameValueCollection.Add("305", str14);
      string str15 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str15);
      string str16 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str16);
      string str17 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107", str17);
      string str18 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str18);
      string str19 = JS.GetStr(loan, "FE0117");
      nameValueCollection.Add("FE0117", str19);
      string str20 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str20);
      string str21 = JS.GetStr(loan, "FR0204");
      nameValueCollection.Add("FR0204", str21);
      string str22 = JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0206_FR0207", str22);
      string str23 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str23);
      string str24 = JS.GetStr(loan, "FE0217");
      nameValueCollection.Add("FE0217", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Detached", false) == 0, "X");
      nameValueCollection.Add("1041_DETACHED", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Attached", false) == 0, "X");
      nameValueCollection.Add("1041_ATTACHED", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "X");
      nameValueCollection.Add("1041_CONDO", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "X");
      nameValueCollection.Add("1041_PUD", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Cooperative", false) == 0, "X");
      nameValueCollection.Add("1041_CO-OP", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "ManufacturedHousing", false) == 0, "X");
      nameValueCollection.Add("1041_MFG", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "X");
      nameValueCollection.Add("19_PURCHASE", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_CASH_OUT", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("19_NOCASH_OUT", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "X");
      nameValueCollection.Add("420_1ST", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "X");
      nameValueCollection.Add("420_2ND", str35);
      string str36 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str36);
      string str37 = JS.GetStr(loan, "REQUEST.X1");
      nameValueCollection.Add("5891", str37);
      string str38 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str38);
      string str39 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str39);
      string str40 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str40);
      string str41 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str41);
      string str42 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str42);
      string str43 = JS.GetStr(loan, "1824");
      nameValueCollection.Add("1824", str43);
      string str44 = JS.GetStr(loan, "VEND.X488");
      nameValueCollection.Add("VEND_X488", str44);
      string str45 = JS.GetStr(loan, "VEND.X489");
      nameValueCollection.Add("VEND_X489", str45);
      string str46 = JS.GetStr(loan, "VEND.X490");
      nameValueCollection.Add("VEND_X490", str46);
      string str47 = JS.GetStr(loan, "VEND.X491") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X492"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X492") + " " + JS.GetStr(loan, "VEND.X493");
      nameValueCollection.Add("VEND_X491_X492_X493", str47);
      string str48 = JS.GetStr(loan, "VEND.X494");
      nameValueCollection.Add("VEND_X494", str48);
      string str49 = JS.GetStr(loan, "VEND.X495");
      nameValueCollection.Add("VEND_X495", str49);
      string str50 = JS.GetStr(loan, "763");
      nameValueCollection.Add("763", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X2"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX2_Y", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X2"), "N", false) == 0, "X");
      nameValueCollection.Add("REQUESTX2_N", str52);
      string str53 = JS.GetStr(loan, "REQUEST.X3");
      nameValueCollection.Add("REQUESTX3", str53);
      string str54 = JS.GetStr(loan, "REQUEST.X4");
      nameValueCollection.Add("REQUESTX4", str54);
      string str55 = JS.GetStr(loan, "REQUEST.X5");
      nameValueCollection.Add("REQUESTX5", str55);
      return nameValueCollection;
    }
  }
}
