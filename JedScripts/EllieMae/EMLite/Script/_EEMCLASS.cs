// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._EEMCLASS
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
  public class _EEMCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str2);
      string str3 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str3);
      string str4 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str4);
      string str5 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str5);
      string str6 = JS.GetStr(loan, "MAX23K.X40");
      nameValueCollection.Add("MAX23K_X40", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X75"), "Y", false) == 0, "X");
      nameValueCollection.Add("MAX23K_X75", str7);
      string str8 = JS.GetStr(loan, "EEM.X63");
      nameValueCollection.Add("EEM_X63", str8);
      string str9 = JS.GetStr(loan, "EEM.X64");
      nameValueCollection.Add("EEM_X64", str9);
      string str10 = JS.GetStr(loan, "EEM.X65");
      nameValueCollection.Add("EEM_X65", str10);
      string str11 = JS.GetStr(loan, "EEM.X66");
      nameValueCollection.Add("EEM_X66", str11);
      string str12 = JS.GetStr(loan, "EEM.X67");
      nameValueCollection.Add("EEM_X67", str12);
      string str13 = JS.GetStr(loan, "EEM.X68");
      nameValueCollection.Add("EEM_X68", str13);
      string str14 = JS.GetStr(loan, "EEM.X69");
      nameValueCollection.Add("EEM_X69", str14);
      string str15 = JS.GetStr(loan, "EEM.X70");
      nameValueCollection.Add("EEM_X70", str15);
      string str16 = JS.GetStr(loan, "EEM.X71");
      nameValueCollection.Add("EEM_X71", str16);
      string str17 = JS.GetStr(loan, "EEM.X72");
      nameValueCollection.Add("EEM_X72", str17);
      string str18 = JS.GetStr(loan, "EEM.X73");
      nameValueCollection.Add("EEM_X73", str18);
      string str19 = JS.GetStr(loan, "EEM.X74");
      nameValueCollection.Add("EEM_X74", str19);
      string str20 = JS.GetStr(loan, "EEM.X75");
      nameValueCollection.Add("EEM_X75", str20);
      string str21 = JS.GetStr(loan, "EEM.X76");
      nameValueCollection.Add("EEM_X76", str21);
      string str22 = JS.GetStr(loan, "EEM.X74");
      nameValueCollection.Add("EEM_X74_a", str22);
      string str23 = JS.GetStr(loan, "EEM.X77");
      nameValueCollection.Add("EEM_X77", str23);
      string str24 = JS.GetStr(loan, "EEM.X78");
      nameValueCollection.Add("EEM_X78", str24);
      string str25 = JS.GetStr(loan, "EEM.X79");
      nameValueCollection.Add("EEM_X79", str25);
      string str26 = JS.GetStr(loan, "EEM.X75");
      nameValueCollection.Add("EEM_X75_a", str26);
      string str27 = JS.GetStr(loan, "EEM.X81");
      nameValueCollection.Add("EEM_X81", str27);
      string str28 = JS.GetStr(loan, "EEM.X82");
      nameValueCollection.Add("EEM_X82", str28);
      string str29 = JS.GetStr(loan, "EEM.X83");
      nameValueCollection.Add("EEM_X83", str29);
      string str30 = JS.GetStr(loan, "EEM.X84");
      nameValueCollection.Add("EEM_X84", str30);
      string str31 = JS.GetStr(loan, "EEM.X85");
      nameValueCollection.Add("EEM_X85", str31);
      string str32 = JS.GetStr(loan, "EEM.X86");
      nameValueCollection.Add("EEM_X86", str32);
      string str33 = JS.GetStr(loan, "EEM.X87");
      nameValueCollection.Add("EEM_X87", str33);
      string str34 = JS.GetStr(loan, "1742");
      nameValueCollection.Add("1742", str34);
      string str35 = JS.GetStr(loan, "EEM.X88");
      nameValueCollection.Add("EEM_X88", str35);
      string str36 = JS.GetStr(loan, "EEM.X89");
      nameValueCollection.Add("EEM_X89", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "EEM.X90"), "Y", false) == 0, "X");
      nameValueCollection.Add("EEM_X90_Y", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "EEM.X90"), "Y", false) != 0, "X");
      nameValueCollection.Add("EEM_X90_N", str38);
      string str39 = JS.GetStr(loan, "1216");
      nameValueCollection.Add("1216", str39);
      return nameValueCollection;
    }
  }
}
