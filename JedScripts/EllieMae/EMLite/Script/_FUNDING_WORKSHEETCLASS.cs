// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FUNDING_WORKSHEETCLASS
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
  public class _FUNDING_WORKSHEETCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str4);
      string str5 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str5);
      string str6 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str6);
      string str7 = JS.GetStr(loan, "VEND.X263");
      nameValueCollection.Add("VEND_X263", str7);
      string str8 = JS.GetStr(loan, "996");
      nameValueCollection.Add("996", str8);
      string str9 = JS.GetStr(loan, "352");
      nameValueCollection.Add("352", str9);
      string str10 = JS.GetStr(loan, "1991");
      nameValueCollection.Add("1991", str10);
      string str11 = JS.GetStr(loan, "1992");
      nameValueCollection.Add("1992", str11);
      string str12 = JS.GetStr(loan, "1993");
      nameValueCollection.Add("1993", str12);
      string str13 = JS.GetStr(loan, "1994");
      nameValueCollection.Add("1994", str13);
      string str14 = JS.GetStr(loan, "1995");
      nameValueCollection.Add("1995", str14);
      string str15 = JS.GetStr(loan, "1996");
      nameValueCollection.Add("1996", str15);
      string str16 = JS.GetStr(loan, "VEND.X200");
      nameValueCollection.Add("VEND_X200", str16);
      string str17 = JS.GetStr(loan, "2011");
      nameValueCollection.Add("2011", str17);
      string str18 = JS.GetStr(loan, "1997");
      nameValueCollection.Add("1997", str18);
      string str19 = JS.GetStr(loan, "1998");
      nameValueCollection.Add("1998", str19);
      string str20 = JS.GetStr(loan, "1999");
      nameValueCollection.Add("1999", str20);
      string str21 = JS.GetStr(loan, "2000");
      nameValueCollection.Add("2000", str21);
      string str22 = JS.GetStr(loan, "186");
      nameValueCollection.Add("186", str22);
      string str23 = JS.GetStr(loan, "610");
      nameValueCollection.Add("610", str23);
      string str24 = JS.GetStr(loan, "612");
      nameValueCollection.Add("612", str24);
      string str25 = JS.GetStr(loan, "613");
      nameValueCollection.Add("613", str25);
      string str26 = JS.GetStr(loan, "1175") + " " + JS.GetStr(loan, "614");
      nameValueCollection.Add("1175_614", str26);
      string str27 = JS.GetStr(loan, "611");
      nameValueCollection.Add("611", str27);
      string str28 = JS.GetStr(loan, "615");
      nameValueCollection.Add("615", str28);
      string str29 = JS.GetStr(loan, "1011");
      nameValueCollection.Add("1011", str29);
      string str30 = JS.GetStr(loan, "VEND.X396");
      nameValueCollection.Add("VEND_X396", str30);
      string str31 = JS.GetStr(loan, "VEND.X397");
      nameValueCollection.Add("VEND_X397", str31);
      string str32 = JS.GetStr(loan, "2004");
      nameValueCollection.Add("2004", str32);
      string str33 = JS.GetStr(loan, "2002");
      nameValueCollection.Add("2002", str33);
      string str34 = JS.GetStr(loan, "187");
      nameValueCollection.Add("187", str34);
      string str35 = JS.GetStr(loan, "411");
      nameValueCollection.Add("411", str35);
      string str36 = JS.GetStr(loan, "412");
      nameValueCollection.Add("412", str36);
      string str37 = JS.GetStr(loan, "413");
      nameValueCollection.Add("413", str37);
      string str38 = JS.GetStr(loan, "1174") + " " + JS.GetStr(loan, "414");
      nameValueCollection.Add("1174_414", str38);
      string str39 = JS.GetStr(loan, "416");
      nameValueCollection.Add("416", str39);
      string str40 = JS.GetStr(loan, "417");
      nameValueCollection.Add("417", str40);
      string str41 = JS.GetStr(loan, "1243");
      nameValueCollection.Add("1243", str41);
      string str42 = JS.GetStr(loan, "VEND.X398");
      nameValueCollection.Add("VEND_X398", str42);
      string str43 = JS.GetStr(loan, "VEND.X399");
      nameValueCollection.Add("VEND_X399", str43);
      string str44 = JS.GetStr(loan, "2007");
      nameValueCollection.Add("2007", str44);
      string str45 = JS.GetStr(loan, "2003");
      nameValueCollection.Add("2003", str45);
      string str46 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str46);
      string str47 = JS.GetStr(loan, "2005");
      nameValueCollection.Add("2005", str47);
      string str48 = JS.GetStr(loan, "1989");
      nameValueCollection.Add("1989", str48);
      string str49 = JS.GetStr(loan, "1990");
      nameValueCollection.Add("1990", str49);
      string str50 = JS.GetStr(loan, "4083");
      nameValueCollection.Add("4083", str50);
      return nameValueCollection;
    }
  }
}
