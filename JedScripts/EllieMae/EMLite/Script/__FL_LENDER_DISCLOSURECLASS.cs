// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__FL_LENDER_DISCLOSURECLASS
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
  public class __FL_LENDER_DISCLOSURECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.NF(Jed.S2N(JS.GetStr(loan, "DISCLOSURE.X67")) + Jed.S2N(JS.GetStr(loan, "DISCLOSURE.X69")) + Jed.S2N(JS.GetStr(loan, "DISCLOSURE.X70")) + Jed.S2N(JS.GetStr(loan, "FLMTGCM.X13")) + Jed.S2N(JS.GetStr(loan, "FLMTGCM.X15")) + Jed.S2N(JS.GetStr(loan, "FLMTGCM.X16")) + Jed.S2N(JS.GetStr(loan, "FLMTGCM.X18")) + Jed.S2N(JS.GetStr(loan, "FLMTGCM.X21")), (byte) 18, 0);
      nameValueCollection.Add("MAP2", str1);
      string str2 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("MAP1", str2);
      string str3 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str3);
      string str4 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str4);
      string str5 = JS.GetStr(loan, "DISCLOSURE.X67");
      nameValueCollection.Add("DISCLOSURE_X67", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X68"), "NonRefundable", false) == 0, "The fee is nonrefundable") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X68"), "Refundable", false) == 0, "The fee is refundable") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X68"), "NA", false) == 0, "No funds collected at or prior to funding");
      nameValueCollection.Add("DISCLOSURE_X68", str6);
      string str7 = JS.GetStr(loan, "DISCLOSURE.X69");
      nameValueCollection.Add("DISCLOSURE_X69", str7);
      string str8 = JS.GetStr(loan, "DISCLOSURE.X70");
      nameValueCollection.Add("DISCLOSURE_X70", str8);
      string str9 = JS.GetStr(loan, "FLMTGCM.X13");
      nameValueCollection.Add("FLMTGCM_X13", str9);
      string str10 = JS.GetStr(loan, "FLMTGCM.X14");
      nameValueCollection.Add("FLMTGCM_X14", str10);
      string str11 = JS.GetStr(loan, "FLMTGCM.X15");
      nameValueCollection.Add("FLMTGCM_X15", str11);
      string str12 = JS.GetStr(loan, "FLMTGCM.X16");
      nameValueCollection.Add("FLMTGCM_X16", str12);
      string str13 = JS.GetStr(loan, "FLMTGCM.X17");
      nameValueCollection.Add("FLMTGCM_X17", str13);
      string str14 = JS.GetStr(loan, "FLMTGCM.X18");
      nameValueCollection.Add("FLMTGCM_X18", str14);
      string str15 = JS.GetStr(loan, "FLMTGCM.X19");
      nameValueCollection.Add("FLMTGCM_X19", str15);
      string str16 = JS.GetStr(loan, "FLMTGCM.X20");
      nameValueCollection.Add("FLMTGCM_X20", str16);
      string str17 = JS.GetStr(loan, "FLMTGCM.X21");
      nameValueCollection.Add("FLMTGCM_X21", str17);
      string str18 = JS.GetStr(loan, "FLMTGCM.X22");
      nameValueCollection.Add("FLMTGCM_X22", str18);
      string str19 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str19);
      string str20 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str20);
      string str21 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str21);
      string str22 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_2", str22);
      string str23 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str23);
      string str24 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str24);
      string str25 = JS.GetStr(loan, "362");
      nameValueCollection.Add("F362", str25);
      string str26 = JS.GetStr(loan, "315");
      nameValueCollection.Add("F315", str26);
      string str27 = JS.GetStr(loan, "319") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "313"), "", false) != 0, ", ") + JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("F319_F313_321_323", str27);
      string str28 = JS.GetStr(loan, "324");
      nameValueCollection.Add("F324", str28);
      string str29 = JS.GetStr(loan, "FLGFE.X41");
      nameValueCollection.Add("FLGFE_X41", str29);
      return nameValueCollection;
    }
  }
}
