// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_1880_ELIG_CERT_P1CLASS
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
  public class _VA_26_1880_ELIG_CERT_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "VAELIG.X71") + "  " + JS.GetStr(loan, "VAELIG.X72");
      nameValueCollection.Add("VAELIGX71", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X1"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X1"));
      nameValueCollection.Add("VAELIGX1", str2);
      string str3 = JS.GetStr(loan, "VAELIG.X73");
      nameValueCollection.Add("VAELIGX73", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X4"), "", false) != 0, "X");
      nameValueCollection.Add("VA_X4A_YES", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X4"), "", false) == 0, "X");
      nameValueCollection.Add("VA_X4A_NO", str5);
      string str6 = JS.GetStr(loan, "VAELIG.X4");
      nameValueCollection.Add("VAELIGX4", str6);
      string str7 = JS.GetStr(loan, "VAELIG.X74");
      nameValueCollection.Add("VAELIGX74", str7);
      string str8 = JS.GetStr(loan, "VAELIG.X97");
      nameValueCollection.Add("VAELIGX97", str8);
      string str9 = JS.GetStr(loan, "VAELIG.X75");
      nameValueCollection.Add("VAELIGX75", str9);
      string str10 = JS.GetStr(loan, "VAELIG.X76") + " " + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X77"), "", false) != 0, ", ") + JS.GetStr(loan, "VAELIG.X77") + " " + JS.GetStr(loan, "VAELIG.X78");
      nameValueCollection.Add("VAELIGX76", str10);
      string str11 = JS.GetStr(loan, "VAELIG.X51");
      nameValueCollection.Add("VAELIGX51", str11);
      string str12 = JS.GetStr(loan, "VAELIG.X54") + " " + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X55"), "", false) != 0, ", ") + JS.GetStr(loan, "VAELIG.X55") + " " + JS.GetStr(loan, "VAELIG.X56");
      nameValueCollection.Add("VAELIGX52", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X22"), "Y", false) == 0, "X");
      nameValueCollection.Add("VAELIGX22_Yes", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X22"), "Y", false) != 0, "X");
      nameValueCollection.Add("VAELIGX22_No", str14);
      string str15 = JS.GetStr(loan, "VAELIG.X23");
      nameValueCollection.Add("VAELIGX23", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X2"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X2"));
      nameValueCollection.Add("VAELIGX2", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X3"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X3"));
      nameValueCollection.Add("VAELIGX3", str17);
      string str18 = JS.GetStr(loan, "VAELIG.X5");
      nameValueCollection.Add("VAELIGX5", str18);
      string str19 = JS.GetStr(loan, "VAELIG.X6");
      nameValueCollection.Add("VAELIGX6", str19);
      string str20 = JS.GetStr(loan, "VAELIG.X67");
      nameValueCollection.Add("VAELIGX67", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X7"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X7"));
      nameValueCollection.Add("VAELIGX7", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X8"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X8"));
      nameValueCollection.Add("VAELIGX8", str22);
      string str23 = JS.GetStr(loan, "VAELIG.X10");
      nameValueCollection.Add("VAELIGX10", str23);
      string str24 = JS.GetStr(loan, "VAELIG.X11");
      nameValueCollection.Add("VAELIGX11", str24);
      string str25 = JS.GetStr(loan, "VAELIG.X68");
      nameValueCollection.Add("VAELIGX68", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X12"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X12"));
      nameValueCollection.Add("VAELIGX12", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X13"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X13"));
      nameValueCollection.Add("VAELIGX13", str27);
      string str28 = JS.GetStr(loan, "VAELIG.X15");
      nameValueCollection.Add("VAELIGX15", str28);
      string str29 = JS.GetStr(loan, "VAELIG.X16");
      nameValueCollection.Add("VAELIGX16", str29);
      string str30 = JS.GetStr(loan, "VAELIG.X69");
      nameValueCollection.Add("VAELIGX69", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X17"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X17"));
      nameValueCollection.Add("VAELIGX17", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X18"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X18"));
      nameValueCollection.Add("VAELIGX18", str32);
      string str33 = JS.GetStr(loan, "VAELIG.X20");
      nameValueCollection.Add("VAELIGX20", str33);
      string str34 = JS.GetStr(loan, "VAELIG.X21");
      nameValueCollection.Add("VAELIGX21", str34);
      string str35 = JS.GetStr(loan, "VAELIG.X70");
      nameValueCollection.Add("VAELIGX70", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X99"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X99"));
      nameValueCollection.Add("VAELIGX99", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X100"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X100"));
      nameValueCollection.Add("VAELIGX100", str37);
      string str38 = JS.GetStr(loan, "VAELIG.X101");
      nameValueCollection.Add("VAELIGX101", str38);
      string str39 = JS.GetStr(loan, "VAELIG.X98");
      nameValueCollection.Add("VAELIGX98", str39);
      string str40 = JS.GetStr(loan, "VAELIG.X102");
      nameValueCollection.Add("VAELIGX102", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X104"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X104"));
      nameValueCollection.Add("VAELIGX104", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X105"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X105"));
      nameValueCollection.Add("VAELIGX105", str42);
      string str43 = JS.GetStr(loan, "VAELIG.X106");
      nameValueCollection.Add("VAELIGX106", str43);
      string str44 = JS.GetStr(loan, "VAELIG.X103");
      nameValueCollection.Add("VAELIGX103", str44);
      string str45 = JS.GetStr(loan, "VAELIG.X107");
      nameValueCollection.Add("VAELIGX107", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X109"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X109"));
      nameValueCollection.Add("VAELIGX109", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X110"), "//", false) != 0, JS.GetStr(loan, "VAELIG.X110"));
      nameValueCollection.Add("VAELIGX110", str47);
      string str48 = JS.GetStr(loan, "VAELIG.X111");
      nameValueCollection.Add("VAELIGX111", str48);
      string str49 = JS.GetStr(loan, "VAELIG.X108");
      nameValueCollection.Add("VAELIGX108", str49);
      string str50 = JS.GetStr(loan, "VAELIG.X112");
      nameValueCollection.Add("VAELIGX112", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAELIG.X113"), "Y", false) == 0, "X");
      nameValueCollection.Add("VAELIGX113", str51);
      return nameValueCollection;
    }
  }
}
