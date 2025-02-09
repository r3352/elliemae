// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__FL_COMMITMENTCLASS
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
  public class __FL_COMMITMENTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "3094");
      nameValueCollection.Add("3094", str1);
      string str2 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str4);
      string str5 = JS.GetStr(loan, "FLMTGCM.X1");
      nameValueCollection.Add("FLMTGCM_X1", str5);
      string str6 = JS.GetStr(loan, "FLMTGCM.X11");
      nameValueCollection.Add("FLMTGCM_X11", str6);
      string str7 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str7);
      string str8 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str8);
      string str9 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str9);
      string str10 = Jed.BF(Jed.S2N(JS.GetStr(loan, "2")) != 0.0, "$ " + JS.GetStr(loan, "2"), "");
      nameValueCollection.Add("2", str10);
      string str11 = JS.GetStr(loan, "FLMTGCM.X7");
      nameValueCollection.Add("FLMTGCM_X7", str11);
      string str12 = JS.GetStr(loan, "FLMTGCM.X6");
      nameValueCollection.Add("FLMTGCM_X6", str12);
      string str13 = JS.GetStr(loan, "FLMTGCM.X4");
      nameValueCollection.Add("FLMTGCM_X4", str13);
      string str14 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str14);
      string str15 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str15);
      string str16 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("MAP2", str16);
      string str17 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("MAP1", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X2"), "Are", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X2_2", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X2"), "AreNot", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X2", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X3"), "EstablishedByLender", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X3_2", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X3"), "DetermineAtClosing", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X3", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X8"), "N", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X8_2", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X8"), "Y", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X8", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X12"), "Y", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X12", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "FLMTGCM.X10"), "Y", false) == 0, "X");
      nameValueCollection.Add("FLMTGCM_X10", str25);
      return nameValueCollection;
    }
  }
}
