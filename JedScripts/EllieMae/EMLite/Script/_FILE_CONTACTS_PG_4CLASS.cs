// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FILE_CONTACTS_PG_4CLASS
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
  public class _FILE_CONTACTS_PG_4CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str1);
      string str2 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "12"), "", false) != 0, ",") + " " + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("363", str3);
      string str4 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str4);
      string str5 = JS.GetStr(loan, "VEND.X86");
      nameValueCollection.Add("VENDX86", str5);
      string str6 = JS.GetStr(loan, "VEND.X74");
      nameValueCollection.Add("VENDX74", str6);
      string str7 = JS.GetStr(loan, "VEND.X76");
      nameValueCollection.Add("VENDX76", str7);
      string str8 = JS.GetStr(loan, "VEND.X77") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X77"), "", false) != 0, ",") + " " + JS.GetStr(loan, "VEND.X78") + " " + JS.GetStr(loan, "VEND.X79");
      nameValueCollection.Add("VENDX77_VENDX78_VENDX79", str8);
      string str9 = JS.GetStr(loan, "VEND.X80");
      nameValueCollection.Add("VENDX80", str9);
      string str10 = JS.GetStr(loan, "VEND.X75");
      nameValueCollection.Add("VENDX75", str10);
      string str11 = JS.GetStr(loan, "VEND.X81");
      nameValueCollection.Add("VENDX81", str11);
      string str12 = JS.GetStr(loan, "VEND.X83");
      nameValueCollection.Add("VENDX83", str12);
      string str13 = JS.GetStr(loan, "VEND.X82");
      nameValueCollection.Add("VENDX82", str13);
      string str14 = JS.GetStr(loan, "VEND.X217");
      nameValueCollection.Add("VENDX217", str14);
      string str15 = JS.GetStr(loan, "VEND.X11");
      nameValueCollection.Add("VENDX11", str15);
      string str16 = JS.GetStr(loan, "VEND.X1");
      nameValueCollection.Add("VENDX1", str16);
      string str17 = JS.GetStr(loan, "VEND.X3");
      nameValueCollection.Add("VENDX3", str17);
      string str18 = JS.GetStr(loan, "VEND.X4") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X4"), "", false) != 0, ",") + " " + JS.GetStr(loan, "VEND.X5") + " " + JS.GetStr(loan, "VEND.X6");
      nameValueCollection.Add("VENDX4_VENDX5_VENDX6", str18);
      string str19 = JS.GetStr(loan, "VEND.X7");
      nameValueCollection.Add("VENDX7", str19);
      string str20 = JS.GetStr(loan, "VEND.X2");
      nameValueCollection.Add("VENDX2", str20);
      string str21 = JS.GetStr(loan, "VEND.X8");
      nameValueCollection.Add("VENDX8", str21);
      string str22 = JS.GetStr(loan, "VEND.X10");
      nameValueCollection.Add("VENDX10", str22);
      string str23 = JS.GetStr(loan, "VEND.X9");
      nameValueCollection.Add("VENDX9", str23);
      string str24 = JS.GetStr(loan, "VEND.X219");
      nameValueCollection.Add("VENDX219", str24);
      return nameValueCollection;
    }
  }
}
