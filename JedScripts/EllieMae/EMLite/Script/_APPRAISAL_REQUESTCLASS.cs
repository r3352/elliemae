// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._APPRAISAL_REQUESTCLASS
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
  public class _APPRAISAL_REQUESTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "617");
      nameValueCollection.Add("617", str1);
      string str2 = JS.GetStr(loan, "618");
      nameValueCollection.Add("618", str2);
      string str3 = JS.GetStr(loan, "619");
      nameValueCollection.Add("619", str3);
      string str4 = JS.GetStr(loan, "620") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1244"), "", false) != 0, ", ") + JS.GetStr(loan, "1244") + " " + JS.GetStr(loan, "621");
      nameValueCollection.Add("620_1244_621", str4);
      string str5 = JS.GetStr(loan, "622");
      nameValueCollection.Add("622", str5);
      string str6 = JS.GetStr(loan, "1246");
      nameValueCollection.Add("1246", str6);
      string str7 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str7);
      string str8 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str8);
      string str9 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str9);
      string str10 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str10);
      string str11 = JS.GetStr(loan, "326");
      nameValueCollection.Add("326", str11);
      string str12 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362", str12);
      string str13 = JS.GetStr(loan, "1408");
      nameValueCollection.Add("1408", str13);
      string str14 = JS.GetStr(loan, "1409");
      nameValueCollection.Add("1409", str14);
      string str15 = JS.GetStr(loan, "317");
      nameValueCollection.Add("317", str15);
      string str16 = JS.GetStr(loan, "1406");
      nameValueCollection.Add("1406", str16);
      string str17 = JS.GetStr(loan, "1407");
      nameValueCollection.Add("1407", str17);
      string str18 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str18);
      string str19 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str19);
      string str20 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str20);
      string str21 = JS.GetStr(loan, "66");
      nameValueCollection.Add("66", str21);
      string str22 = JS.GetStr(loan, "FE0117");
      nameValueCollection.Add("FE0117", str22);
      string str23 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str23);
      string str24 = JS.GetStr(loan, "FR0204");
      nameValueCollection.Add("FR0204", str24);
      string str25 = JS.GetStr(loan, "FR0206") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0207"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0207") + " " + JS.GetStr(loan, "FR0208");
      nameValueCollection.Add("FR0206_FR0207_FR0208", str25);
      string str26 = JS.GetStr(loan, "98");
      nameValueCollection.Add("98", str26);
      string str27 = JS.GetStr(loan, "FE0217");
      nameValueCollection.Add("FE0217", str27);
      string str28 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str28);
      string str29 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str29);
      string str30 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str30);
      string str31 = JS.GetStr(loan, "16");
      nameValueCollection.Add("16", str31);
      string str32 = JS.GetStr(loan, "18");
      nameValueCollection.Add("18", str32);
      string str33 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str33);
      string str34 = JS.GetStr(loan, "1821");
      nameValueCollection.Add("1821", str34);
      string str35 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Attached", false) == 0, "Attached") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "Condominium") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Cooperative", false) == 0, "Cooperative") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Detached", false) == 0, "Detached") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "HighRiseCondominium", false) == 0, "High Rise Condominium") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "ManufacturedHousing", false) == 0, "Manufactured Housing") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "PUD") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "DetachedCondo", false) == 0, "Detached Condo") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "ManufacturedHomeCondoPUDCoOp", false) == 0, "Mfd Home/Condo/PUD/Co-Op");
      nameValueCollection.Add("1041", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "PrimaryResidence", false) == 0, "Primary") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "SecondHome", false) == 0, "Secondary") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1811"), "Investor", false) == 0, "Investment");
      nameValueCollection.Add("1811", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "Other", false) == 0, JS.GetStr(loan, "1063"), JS.GetStr(loan, "1172"));
      nameValueCollection.Add("1172", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "FirstLien", false) == 0, "First") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "420"), "SecondLien", false) == 0, "Second");
      nameValueCollection.Add("420", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "Purchase") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionOnly", false) == 0, "Construction Only") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0, "Cash-Out Refinance") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "ConstructionToPermanent", false) == 0, "Construction To Permanent") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "No Cash-Out Refinance") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Other", false) == 0, "Other");
      nameValueCollection.Add("19", str40);
      string str41 = JS.GetStr(loan, "1401");
      nameValueCollection.Add("1401", str41);
      string str42 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str42);
      string str43 = JS.GetStr(loan, "1256");
      nameValueCollection.Add("1256", str43);
      string str44 = JS.GetStr(loan, "1262");
      nameValueCollection.Add("1262", str44);
      string str45 = JS.GetStr(loan, "95");
      nameValueCollection.Add("95", str45);
      string str46 = JS.GetStr(loan, "VEND.X144");
      nameValueCollection.Add("VENDX144", str46);
      string str47 = JS.GetStr(loan, "VEND.X150");
      nameValueCollection.Add("VENDX150", str47);
      string str48 = JS.GetStr(loan, "VEND.X151");
      nameValueCollection.Add("VENDX151", str48);
      string str49 = JS.GetStr(loan, "VEND.X152");
      nameValueCollection.Add("VENDX152", str49);
      string str50 = JS.GetStr(loan, "VEND.X133");
      nameValueCollection.Add("VENDX133", str50);
      string str51 = JS.GetStr(loan, "VEND.X139");
      nameValueCollection.Add("VENDX139", str51);
      string str52 = JS.GetStr(loan, "VEND.X140");
      nameValueCollection.Add("VENDX140", str52);
      string str53 = JS.GetStr(loan, "VEND.X141");
      nameValueCollection.Add("VENDX141", str53);
      string str54 = JS.GetStr(loan, "610");
      nameValueCollection.Add("610", str54);
      string str55 = JS.GetStr(loan, "611");
      nameValueCollection.Add("611", str55);
      string str56 = JS.GetStr(loan, "615");
      nameValueCollection.Add("615", str56);
      string str57 = JS.GetStr(loan, "87");
      nameValueCollection.Add("87", str57);
      string str58 = JS.GetStr(loan, "411");
      nameValueCollection.Add("411", str58);
      string str59 = JS.GetStr(loan, "416");
      nameValueCollection.Add("416", str59);
      string str60 = JS.GetStr(loan, "417");
      nameValueCollection.Add("417", str60);
      string str61 = JS.GetStr(loan, "88");
      nameValueCollection.Add("88", str61);
      string str62 = JS.GetStr(loan, "763");
      nameValueCollection.Add("763", str62);
      string str63 = JS.GetStr(loan, "REQUEST.X21");
      nameValueCollection.Add("REQUESTX21", str63);
      string str64 = JS.GetStr(loan, "1541");
      nameValueCollection.Add("1541", str64);
      string str65 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X22"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX22", str65);
      string str66 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X23"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX23", str66);
      string str67 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "REQUEST.X24"), "Y", false) == 0, "X");
      nameValueCollection.Add("REQUESTX24", str67);
      string str68 = JS.GetStr(loan, "REQUEST.X25");
      nameValueCollection.Add("REQUESTX25", str68);
      string str69 = JS.GetStr(loan, "REQUEST.X26");
      nameValueCollection.Add("REQUESTX26", str69);
      string str70 = JS.GetStr(loan, "REQUEST.X27");
      nameValueCollection.Add("REQUESTX27", str70);
      string str71 = JS.GetStr(loan, "REQUEST.X28");
      nameValueCollection.Add("REQUESTX28", str71);
      string str72 = JS.GetStr(loan, "17");
      nameValueCollection.Add("17", str72);
      string str73 = JS.GetStr(loan, "1824");
      nameValueCollection.Add("1824", str73);
      return nameValueCollection;
    }
  }
}
