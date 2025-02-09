// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._CASH_TO_CLOSECLASS
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
  public class _CASH_TO_CLOSECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str1);
      string str2 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str2);
      string str3 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str3);
      string str4 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str4);
      string str5 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str5);
      string str6 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str6);
      string str7 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str7);
      string str8 = JS.GetStr(loan, "317");
      nameValueCollection.Add("317", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("363", str9);
      string str10 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str10);
      string str11 = JS.GetStr(loan, "967");
      nameValueCollection.Add("967", str11);
      string str12 = JS.GetStr(loan, "968");
      nameValueCollection.Add("968", str12);
      string str13 = JS.GetStr(loan, "1092");
      nameValueCollection.Add("1092", str13);
      string str14 = JS.GetStr(loan, "138");
      nameValueCollection.Add("138", str14);
      string str15 = JS.GetStr(loan, "137");
      nameValueCollection.Add("137", str15);
      string str16 = JS.GetStr(loan, "969");
      nameValueCollection.Add("969", str16);
      string str17 = JS.GetStr(loan, "1061");
      nameValueCollection.Add("1010", str17);
      string str18 = JS.GetStr(loan, "1093");
      nameValueCollection.Add("1093", str18);
      string str19 = JS.GetStr(loan, "1073");
      nameValueCollection.Add("1073", str19);
      string str20 = JS.GetStr(loan, "140");
      nameValueCollection.Add("140", str20);
      string str21 = JS.GetStr(loan, "143");
      nameValueCollection.Add("143", str21);
      string str22 = JS.GetStr(loan, "202");
      nameValueCollection.Add("202", str22);
      string str23 = JS.GetStr(loan, "1091");
      nameValueCollection.Add("1091", str23);
      string str24 = JS.GetStr(loan, "1106");
      nameValueCollection.Add("1106", str24);
      string str25 = JS.GetStr(loan, "141");
      nameValueCollection.Add("141", str25);
      string str26 = JS.GetStr(loan, "1095");
      nameValueCollection.Add("1095", str26);
      string str27 = JS.GetStr(loan, "1115");
      nameValueCollection.Add("1115", str27);
      string str28 = JS.GetStr(loan, "1646");
      nameValueCollection.Add("1646", str28);
      string str29 = JS.GetStr(loan, "1647");
      nameValueCollection.Add("1647", str29);
      string str30 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str30);
      string str31 = JS.GetStr(loan, "1107");
      nameValueCollection.Add("1107", str31);
      string str32 = JS.GetStr(loan, "1045");
      nameValueCollection.Add("1045", str32);
      string str33 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str33);
      string str34 = JS.GetStr(loan, "142");
      nameValueCollection.Add("142", str34);
      return nameValueCollection;
    }
  }
}
