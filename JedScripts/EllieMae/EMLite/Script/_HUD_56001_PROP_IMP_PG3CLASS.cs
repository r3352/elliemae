// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_56001_PROP_IMP_PG3CLASS
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
  public class _HUD_56001_PROP_IMP_PG3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X60"), "SingleFamily", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX60_1Single", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X60"), "MultiFamily", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX60_2Multi", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X60"), "Nonresidential", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX60_3NonRes", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X60"), "ManufacturedHome", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX60_4MF", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X60"), "HistoricResidential", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX60_5Historic", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X60"), "HealthcareFacility", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX60_6Health", str6);
      string str7 = JS.GetStr(loan, "CAPIAP.X61");
      nameValueCollection.Add("CAPIAPX61", str7);
      string str8 = JS.GetStr(loan, "CAPIAP.X43");
      nameValueCollection.Add("CAPIAPX43", str8);
      string str9 = JS.GetStr(loan, "CAPIAP.X44");
      nameValueCollection.Add("CAPIAPX44", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X45"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX45_Yes", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X45"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX45_No", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X46"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX46_Yes", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X46"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX46_No", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X47"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX47_Yes", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X47"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX47_No", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X48"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX48_Yes", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X48"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX48_No", str17);
      string str18 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str18);
      string str19 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str19);
      string str20 = JS.GetStr(loan, "CAPIAP.X49");
      nameValueCollection.Add("CAPIAPX49", str20);
      string str21 = JS.GetStr(loan, "CAPIAP.X50");
      nameValueCollection.Add("CAPIAPX50", str21);
      string str22 = JS.GetStr(loan, "CAPIAP.X51") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X52"), "", false) != 0, ", ") + JS.GetStr(loan, "CAPIAP.X52") + " " + JS.GetStr(loan, "CAPIAP.X53");
      nameValueCollection.Add("CAPIAPX51_CAPIAPX52_CAPIAPX53", str22);
      string str23 = JS.GetStr(loan, "18");
      nameValueCollection.Add("18", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "24"), "//", false) != 0, JS.GetStr(loan, "24"), "");
      nameValueCollection.Add("24", str24);
      string str25 = Jed.NF(Jed.S2N(JS.GetStr(loan, "25")), (byte) 18, 0);
      nameValueCollection.Add("25", str25);
      string str26 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str26);
      string str27 = JS.GetStr(loan, "CAPIAP.X54");
      nameValueCollection.Add("CAPIAPX54", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X55"), "//", false) != 0, JS.GetStr(loan, "CAPIAP.X55"), "");
      nameValueCollection.Add("CAPIAPX55", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X56"), "Y", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX56_Yes", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X56"), "N", false) == 0, "X");
      nameValueCollection.Add("CAPIAPX56_No", str30);
      string str31 = JS.GetStr(loan, "205");
      nameValueCollection.Add("205", str31);
      string str32 = JS.GetStr(loan, "982");
      nameValueCollection.Add("982", str32);
      string str33 = JS.GetStr(loan, "CAPIAP.X144");
      nameValueCollection.Add("1024", str33);
      string str34 = JS.GetStr(loan, "CAPIAP.X145") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "CAPIAP.X146"), "", false) != 0, ", ") + JS.GetStr(loan, "CAPIAP.X146") + " " + JS.GetStr(loan, "CAPIAP.X147");
      nameValueCollection.Add("1025", str34);
      string str35 = JS.GetStr(loan, "29");
      nameValueCollection.Add("29", str35);
      string str36 = JS.GetStr(loan, "317");
      nameValueCollection.Add("317", str36);
      string str37 = JS.GetStr(loan, "982");
      nameValueCollection.Add("982_A", str37);
      string str38 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362", str38);
      string str39 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str39);
      string str40 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str40);
      string str41 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str41);
      return nameValueCollection;
    }
  }
}
