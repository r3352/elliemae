// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__NY_PREAPPLICATION_BROKER_DISCLOSURE_P1CLASS
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
  public class __NY_PREAPPLICATION_BROKER_DISCLOSURE_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_1", str1);
      string str2 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str2);
      string str3 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str3);
      string str4 = JS.GetStr(loan, "DISCLOSURE.X98");
      nameValueCollection.Add("DISCLOSURE_X98", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X97"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X97", str5);
      string str6 = JS.GetStr(loan, "DISCLOSURE.X121");
      nameValueCollection.Add("DISCLOSURE_X121", str6);
      string str7 = JS.GetStr(loan, "DISCLOSURE.X120");
      nameValueCollection.Add("DISCLOSURE_X120", str7);
      string str8 = JS.GetStr(loan, "DISCLOSURE.X119");
      nameValueCollection.Add("DISCLOSURE_X119", str8);
      string str9 = JS.GetStr(loan, "DISCLOSURE.X118");
      nameValueCollection.Add("DISCLOSURE_X118", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X117"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X117", str10);
      string str11 = JS.GetStr(loan, "DISCLOSURE.X116");
      nameValueCollection.Add("DISCLOSURE_X116", str11);
      string str12 = JS.GetStr(loan, "DISCLOSURE.X115");
      nameValueCollection.Add("DISCLOSURE_X115", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X114"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X114", str13);
      string str14 = JS.GetStr(loan, "DISCLOSURE.X113");
      nameValueCollection.Add("DISCLOSURE_X113", str14);
      string str15 = JS.GetStr(loan, "DISCLOSURE.X112");
      nameValueCollection.Add("DISCLOSURE_X112", str15);
      string str16 = JS.GetStr(loan, "DISCLOSURE.X111");
      nameValueCollection.Add("DISCLOSURE_X111", str16);
      string str17 = JS.GetStr(loan, "DISCLOSURE.X110");
      nameValueCollection.Add("DISCLOSURE_X110", str17);
      string str18 = JS.GetStr(loan, "326");
      nameValueCollection.Add("326", str18);
      string str19 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str19);
      string str20 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X109"), "Y", false) == 0, "X");
      nameValueCollection.Add("DISCLOSURE_X109", str20);
      string str21 = JS.GetStr(loan, "DISCLOSURE.X108");
      nameValueCollection.Add("DISCLOSURE_X108", str21);
      string str22 = JS.GetStr(loan, "DISCLOSURE.X107");
      nameValueCollection.Add("DISCLOSURE_X107", str22);
      string str23 = JS.GetStr(loan, "DISCLOSURE.X106");
      nameValueCollection.Add("DISCLOSURE_X106", str23);
      string str24 = JS.GetStr(loan, "DISCLOSURE.X105");
      nameValueCollection.Add("DISCLOSURE_X105", str24);
      string str25 = JS.GetStr(loan, "DISCLOSURE.X104");
      nameValueCollection.Add("DISCLOSURE_X104", str25);
      string str26 = JS.GetStr(loan, "DISCLOSURE.X103");
      nameValueCollection.Add("DISCLOSURE_X103", str26);
      string str27 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("MAP11", str27);
      return nameValueCollection;
    }
  }
}
