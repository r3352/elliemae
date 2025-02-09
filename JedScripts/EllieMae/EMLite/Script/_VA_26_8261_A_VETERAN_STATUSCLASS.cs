// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_8261_A_VETERAN_STATUSCLASS
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
  public class _VA_26_8261_A_VETERAN_STATUSCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "VAVOB.X52");
      nameValueCollection.Add("VAVOBX52", str1);
      string str2 = JS.GetStr(loan, "VAVOB.X53");
      nameValueCollection.Add("VAVOBX53", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X55"), "//", false) == 0, "", JS.GetStr(loan, "VAVOB.X55"));
      nameValueCollection.Add("VAVOBX55", str3);
      string str4 = JS.GetStr(loan, "VAVOB.X44");
      nameValueCollection.Add("VAVOBX44", str4);
      string str5 = JS.GetStr(loan, "VAVOB.X45") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X45"), "", false) != 0, ", ") + JS.GetStr(loan, "VAVOB.X46") + " " + JS.GetStr(loan, "VAVOB.X47");
      nameValueCollection.Add("VAVOBX45", str5);
      string str6 = JS.GetStr(loan, "VAELIG.X2");
      nameValueCollection.Add("VAELIGX2", str6);
      string str7 = JS.GetStr(loan, "VAELIG.X3");
      nameValueCollection.Add("VAELIGX3", str7);
      string str8 = JS.GetStr(loan, "VAELIG.X4");
      nameValueCollection.Add("VAELIGX4", str8);
      string str9 = JS.GetStr(loan, "VAELIG.X5");
      nameValueCollection.Add("VAELIGX5", str9);
      string str10 = JS.GetStr(loan, "VAELIG.X6");
      nameValueCollection.Add("VAELIGX6", str10);
      string str11 = JS.GetStr(loan, "VAELIG.X7");
      nameValueCollection.Add("VAELIGX7", str11);
      string str12 = JS.GetStr(loan, "VAELIG.X8");
      nameValueCollection.Add("VAELIGX8", str12);
      string str13 = JS.GetStr(loan, "VAELIG.X9");
      nameValueCollection.Add("VAELIGX9", str13);
      string str14 = JS.GetStr(loan, "VAELIG.X10");
      nameValueCollection.Add("VAELIGX10", str14);
      string str15 = JS.GetStr(loan, "VAELIG.X11");
      nameValueCollection.Add("VAELIGX11", str15);
      string str16 = JS.GetStr(loan, "VAELIG.S2");
      nameValueCollection.Add("VAELIGS2", str16);
      string str17 = JS.GetStr(loan, "VAELIG.S3");
      nameValueCollection.Add("VAELIGS3", str17);
      string str18 = JS.GetStr(loan, "VAELIG.X23");
      nameValueCollection.Add("VAELIGX23", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X68"), "Y", false) == 0, "X");
      nameValueCollection.Add("VAVOBX68_Yes", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X68"), "N", false) == 0, "X");
      nameValueCollection.Add("VAVOBX68_No", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X69"), "Y", false) == 0, "X");
      nameValueCollection.Add("VAVOBX69_Yes", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X69"), "N", false) == 0, "X");
      nameValueCollection.Add("VAVOBX69_No", str22);
      string str23 = JS.GetStr(loan, "VAVOB.X1");
      nameValueCollection.Add("VAVOBX1", str23);
      string str24 = JS.GetStr(loan, "VAVOB.X3");
      nameValueCollection.Add("VAVOBX3", str24);
      string str25 = JS.GetStr(loan, "VAVOB.X4") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VAVOB.X4"), "", false) != 0, ", ") + JS.GetStr(loan, "VAVOB.X5") + " " + JS.GetStr(loan, "VAVOB.X6");
      nameValueCollection.Add("VAVOBX4", str25);
      string str26 = JS.GetStr(loan, "315");
      nameValueCollection.Add("VAVOBX1_1", str26);
      string str27 = JS.GetStr(loan, "319");
      nameValueCollection.Add("VAVOBX3_1", str27);
      string str28 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("VAVOBX4_1", str28);
      return nameValueCollection;
    }
  }
}
