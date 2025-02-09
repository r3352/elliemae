// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFEP1_2010LCLASS
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
  public class _GFEP1_2010LCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X293"), JS.GetStr(loan, "315"));
      nameValueCollection.Add("315", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X302"), JS.GetStr(loan, "317"));
      nameValueCollection.Add("317", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X294"), JS.GetStr(loan, "319"));
      nameValueCollection.Add("319", str3);
      string str4 = Jed.BF((Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0 ? 1 : 0) != 0, JS.GetStr(loan, "VEND.X295") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X296"), "", false) != 0, ", ") + JS.GetStr(loan, "VEND.X296") + " " + JS.GetStr(loan, "VEND.X297"), JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323"));
      nameValueCollection.Add("313_321_323", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X303"), "", false) == 0, JS.GetStr(loan, "VEND.X304"), JS.GetStr(loan, "VEND.X303")), JS.GetStr(loan, "1406"));
      nameValueCollection.Add("1406", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3136"), "Y", false) == 0, JS.GetStr(loan, "VEND.X305"), JS.GetStr(loan, "1407"));
      nameValueCollection.Add("1407", str6);
      string str7 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str7);
      string str8 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str8);
      string str9 = JS.GetStr(loan, "3170");
      nameValueCollection.Add("3170", str9);
      string str10 = JS.GetStr(loan, "NEWHUD.X1") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X725"), "Y", false) != 0, " " + JS.GetStr(loan, "NEWHUD.X725"), "");
      nameValueCollection.Add("NEWHUD_X1", str10);
      string str11 = JS.GetStr(loan, "NEWHUD.X2");
      nameValueCollection.Add("NEWHUD_X2", str11);
      string str12 = JS.GetStr(loan, "NEWHUD.X719");
      nameValueCollection.Add("NEWHUD_X719", str12);
      string str13 = JS.GetStr(loan, "NEWHUD.X3");
      nameValueCollection.Add("NEWHUD_X3", str13);
      string str14 = JS.GetStr(loan, "NEWHUD.X4");
      nameValueCollection.Add("NEWHUD_X4", str14);
      string str15 = JS.GetStr(loan, "1347");
      nameValueCollection.Add("1347", str15);
      string str16 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str16);
      string str17 = JS.GetStr(loan, "NEWHUD.X217");
      nameValueCollection.Add("NEWHUD_X217", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X5"), "Y", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X5_Y", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X5"), "N", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X5_N", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X5"), "Y", false) == 0, JS.GetStr(loan, "NEWHUD.X555"), "");
      nameValueCollection.Add("NEWHUD_X555", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X5"), "Y", false) == 0, Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X556")) == 1.0, JS.GetStr(loan, "NEWHUD.X556") + " month", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X556")) > 1.0, JS.GetStr(loan, "NEWHUD.X556") + " months", "")), "");
      nameValueCollection.Add("NEWHUD_X556", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X6"), "Y", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X6_Y", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X6"), "N", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X6_N", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X6"), "Y", false) == 0, JS.GetStr(loan, "NEWHUD.X7"), "");
      nameValueCollection.Add("NEWHUD_X7", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X8"), "Y", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X8_Y", str25);
      string str26 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X8"), "N", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X8_N", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X8"), "Y", false) == 0, Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X9")) == 1.0, JS.GetStr(loan, "NEWHUD.X9") + " month", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X9")) > 1.0, JS.GetStr(loan, "NEWHUD.X9") + " months", "")), "");
      nameValueCollection.Add("NEWHUD_X9", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X8"), "Y", false) == 0, JS.GetStr(loan, "NEWHUD.X10"), "");
      nameValueCollection.Add("NEWHUD_X10", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X8"), "Y", false) == 0, JS.GetStr(loan, "NEWHUD.X11"), "");
      nameValueCollection.Add("NEWHUD_X11", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "Y", false) == 0, "X");
      nameValueCollection.Add("675_Y", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "N", false) == 0, "X");
      nameValueCollection.Add("675_N", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "Y", false) == 0, JS.GetStr(loan, "RE88395.X315"), "");
      nameValueCollection.Add("RE88395_X315", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "X");
      nameValueCollection.Add("1659_Y", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "N", false) == 0, "X");
      nameValueCollection.Add("1659_N", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, JS.GetStr(loan, "RE88395.X121"), "");
      nameValueCollection.Add("RE88395_X121", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, JS.GetStr(loan, "NEWHUD.X348"), "");
      nameValueCollection.Add("NEWHUD_X348", str36);
      string str37 = JS.GetStr(loan, "5");
      nameValueCollection.Add("5", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1550"), "Y", false) == 0, "X");
      nameValueCollection.Add("1550_Y", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1550"), "N", false) == 0, "X");
      nameValueCollection.Add("1550_N", str39);
      string str40 = JS.GetStr(loan, "NEWHUD.X16");
      nameValueCollection.Add("NEWHUD_X16", str40);
      string str41 = JS.GetStr(loan, "NEWHUD.X92");
      nameValueCollection.Add("NEWHUD_X92", str41);
      string str42 = JS.GetStr(loan, "NEWHUD.X93");
      nameValueCollection.Add("NEWHUD_X93", str42);
      return nameValueCollection;
    }
  }
}
