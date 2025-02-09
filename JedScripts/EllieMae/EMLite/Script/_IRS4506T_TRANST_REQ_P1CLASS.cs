// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._IRS4506T_TRANST_REQ_P1CLASS
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
  public class _IRS4506T_TRANST_REQ_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = JS.GetStr(loan, "IRS4506.X2") + " " + JS.GetStr(loan, "IRS4506.X3");
      nameValueCollection.Add("IRS4506X2_X3", str2);
      string str3 = JS.GetStr(loan, "IRS4506.X4");
      nameValueCollection.Add("IRS4506X4", str3);
      string str4 = JS.GetStr(loan, "IRS4506.X6") + " " + JS.GetStr(loan, "IRS4506.X7");
      nameValueCollection.Add("IRS4506X6_X7", str4);
      string str5 = JS.GetStr(loan, "IRS4506.X5");
      nameValueCollection.Add("IRS4506X5", str5);
      string str6 = JS.GetStr(loan, "IRS4506.X39") + " " + JS.GetStr(loan, "IRS4506.X40");
      nameValueCollection.Add("IRS4506X39_X40", str6);
      string str7 = JS.GetStr(loan, "IRS4506.X35") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X35"), "", false) != 0, " ") + JS.GetStr(loan, "IRS4506.X36") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X36"), "", false) != 0, ", ") + JS.GetStr(loan, "IRS4506.X37") + " " + JS.GetStr(loan, "IRS4506.X38");
      nameValueCollection.Add("IRS4506X35_X36_X37_X38", str7);
      string str8 = JS.GetStr(loan, "IRS4506.X41") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X41"), "", false) != 0, ", ") + JS.GetStr(loan, "IRS4506.X42") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X42"), "", false) != 0, ", ") + JS.GetStr(loan, "IRS4506.X43") + " " + JS.GetStr(loan, "IRS4506.X44");
      nameValueCollection.Add("IRS4506X41_X42_X43_X44", str8);
      string str9 = JS.GetStr(loan, "IRS4506.X8") + " " + JS.GetStr(loan, "IRS4506.X9");
      nameValueCollection.Add("IRS4506X8_X9", str9);
      string str10 = JS.GetStr(loan, "IRS4506.X10") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X10"), "", false) != 0, ", ") + JS.GetStr(loan, "IRS4506.X11") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X11"), "", false) != 0, ", ") + JS.GetStr(loan, "IRS4506.X12") + " " + JS.GetStr(loan, "IRS4506.X13");
      nameValueCollection.Add("IRS4506X10_X11_X12_X13", str10);
      string str11 = JS.GetStr(loan, "IRS4506.X45");
      nameValueCollection.Add("IRS4506X45", str11);
      string str12 = JS.GetStr(loan, "IRS4506.X24");
      nameValueCollection.Add("IRS4506X24", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X25"), "//", false) != 0, JS.GetStr(loan, "IRS4506.X25"), "");
      nameValueCollection.Add("IRS4506X25", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X26"), "//", false) != 0, JS.GetStr(loan, "IRS4506.X26"), "");
      nameValueCollection.Add("IRS4506X26", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X29"), "//", false) != 0, JS.GetStr(loan, "IRS4506.X29"), "");
      nameValueCollection.Add("IRS4506X29", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X30"), "//", false) != 0, JS.GetStr(loan, "IRS4506.X30"), "");
      nameValueCollection.Add("IRS4506X30", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X46"), "Y", false) == 0, "X");
      nameValueCollection.Add("IRS4506X46", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X47"), "Y", false) == 0, "X");
      nameValueCollection.Add("IRS4506X47", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X48"), "Y", false) == 0, "X");
      nameValueCollection.Add("IRS4506X48", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X49"), "Y", false) == 0, "X");
      nameValueCollection.Add("IRS4506X49", str20);
      string str21 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X50"), "Y", false) == 0, "X");
      nameValueCollection.Add("IRS4506X50", str21);
      string str22 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X64"), "Y", false) == 0, "X");
      nameValueCollection.Add("IRS4506X64", str22);
      string str23 = JS.GetStr(loan, "IRS4506.X28");
      nameValueCollection.Add("IRS4506X28", str23);
      string areaCode = Jed.GetAreaCode(JS.GetStr(loan, "IRS4506.X27"));
      nameValueCollection.Add("IRS4506X27_1", areaCode);
      string phoneNo = Jed.GetPhoneNo(JS.GetStr(loan, "IRS4506.X27"));
      nameValueCollection.Add("IRS4506X27_2", phoneNo);
      return nameValueCollection;
    }
  }
}
