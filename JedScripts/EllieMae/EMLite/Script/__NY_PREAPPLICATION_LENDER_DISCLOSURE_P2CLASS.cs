// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__NY_PREAPPLICATION_LENDER_DISCLOSURE_P2CLASS
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
  public class __NY_PREAPPLICATION_LENDER_DISCLOSURE_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_1", str1);
      string str2 = JS.GetStr(loan, "DISCLOSURE.X129");
      nameValueCollection.Add("DISCLOSURE_X129", str2);
      string str3 = JS.GetStr(loan, "DISCLOSURE.X128");
      nameValueCollection.Add("DISCLOSURE_X128", str3);
      string str4 = JS.GetStr(loan, "DISCLOSURE.X127");
      nameValueCollection.Add("DISCLOSURE_X127", str4);
      string str5 = JS.GetStr(loan, "DISCLOSURE.X126");
      nameValueCollection.Add("DISCLOSURE_X126", str5);
      string str6 = JS.GetStr(loan, "DISCLOSURE.X125");
      nameValueCollection.Add("DISCLOSURE_X125", str6);
      string str7 = JS.GetStr(loan, "DISCLOSURE.X124");
      nameValueCollection.Add("DISCLOSURE_X124", str7);
      string str8 = JS.GetStr(loan, "DISCLOSURE.X123");
      nameValueCollection.Add("DISCLOSURE_X123", str8);
      string str9 = JS.GetStr(loan, "DISCLOSURE.X122");
      nameValueCollection.Add("DISCLOSURE_X122", str9);
      string str10 = JS.GetStr(loan, "DISCLOSURE.X136");
      nameValueCollection.Add("DISCLOSURE_X136", str10);
      string str11 = JS.GetStr(loan, "DISCLOSURE.X132");
      nameValueCollection.Add("DISCLOSURE_X132", str11);
      string str12 = JS.GetStr(loan, "DISCLOSURE.X131");
      nameValueCollection.Add("DISCLOSURE_X131", str12);
      string str13 = JS.GetStr(loan, "DISCLOSURE.X130");
      nameValueCollection.Add("DISCLOSURE_X130", str13);
      string str14 = JS.GetStr(loan, "DISCLOSURE.X133") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X134"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X134") + " " + JS.GetStr(loan, "DISCLOSURE.X135");
      nameValueCollection.Add("MAP1", str14);
      string str15 = JS.GetStr(loan, "DISCLOSURE.X164");
      nameValueCollection.Add("DISCLOSURE_X164", str15);
      string str16 = JS.GetStr(loan, "DISCLOSURE.X165");
      nameValueCollection.Add("DISCLOSURE_X165", str16);
      string str17 = JS.GetStr(loan, "DISCLOSURE.X166");
      nameValueCollection.Add("DISCLOSURE_X166", str17);
      string str18 = JS.GetStr(loan, "DISCLOSURE.X167");
      nameValueCollection.Add("DISCLOSURE_X167", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X179"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X179", str19);
      return nameValueCollection;
    }
  }
}
