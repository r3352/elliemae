// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ABILITYTOREPAY_P1CLASS
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
  public class _ABILITYTOREPAY_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("363", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str4);
      string str5 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str5);
      string str6 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str6);
      string str7 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str7);
      string str8 = JS.GetStr(loan, "QM.X23");
      nameValueCollection.Add("QM_X23", str8);
      string str9 = JS.GetStr(loan, "QM.X24");
      nameValueCollection.Add("QM_X24", str9);
      string str10 = JS.GetStr(loan, "QM.X25");
      nameValueCollection.Add("QM_X25", str10);
      string str11 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str11);
      string str12 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str12);
      string str13 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str13);
      string str14 = JS.GetStr(loan, "1014");
      nameValueCollection.Add("1014", str14);
      string str15 = JS.GetStr(loan, "745");
      nameValueCollection.Add("745", str15);
      string str16 = JS.GetStr(loan, "3293");
      nameValueCollection.Add("3293", str16);
      string str17 = JS.GetStr(loan, "2625");
      nameValueCollection.Add("2625", str17);
      string str18 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str18);
      string str19 = JS.GetStr(loan, "325");
      nameValueCollection.Add("325", str19);
      string str20 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str20);
      string str21 = JS.GetStr(loan, "356");
      nameValueCollection.Add("356", str21);
      string str22 = JS.GetStr(loan, "3331");
      nameValueCollection.Add("3331", str22);
      string str23 = JS.GetStr(loan, "2982");
      nameValueCollection.Add("2982", str23);
      string str24 = JS.GetStr(loan, "NEWHUD.X6");
      nameValueCollection.Add("NEWHUD_X6", str24);
      string str25 = JS.GetStr(loan, "1659");
      nameValueCollection.Add("1659", str25);
      string str26 = JS.GetStr(loan, "RE88395.X316");
      nameValueCollection.Add("RE88395_X316", str26);
      string str27 = JS.GetStr(loan, "RE88395.X315");
      nameValueCollection.Add("RE88395_X315", str27);
      string str28 = JS.GetStr(loan, "NTB.X16");
      nameValueCollection.Add("NTB_X16", str28);
      string str29 = JS.GetStr(loan, "799");
      nameValueCollection.Add("799", str29);
      string str30 = JS.GetStr(loan, "3121");
      nameValueCollection.Add("3121", str30);
      string str31 = JS.GetStr(loan, "1206");
      nameValueCollection.Add("1206", str31);
      string str32 = JS.GetStr(loan, "3246");
      nameValueCollection.Add("3246", str32);
      string str33 = JS.GetStr(loan, "3134");
      nameValueCollection.Add("3134", str33);
      string str34 = JS.GetStr(loan, "761");
      nameValueCollection.Add("761", str34);
      string str35 = JS.GetStr(loan, "3253");
      nameValueCollection.Add("3253", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X135"), "is", false) == 0, "X");
      nameValueCollection.Add("QM_X135_Y", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X135"), "is not", false) == 0, "X");
      nameValueCollection.Add("QM_X135_N", str37);
      string str38 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3_a", str38);
      string str39 = JS.GetStr(loan, "3290");
      nameValueCollection.Add("3290", str39);
      string str40 = JS.GetStr(loan, "740");
      nameValueCollection.Add("740", str40);
      string str41 = JS.GetStr(loan, "742");
      nameValueCollection.Add("742", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "", JS.GetStr(loan, "1827"));
      nameValueCollection.Add("1827", str42);
      string str43 = JS.GetStr(loan, "QM.X373");
      nameValueCollection.Add("QM_X373", str43);
      string str44 = JS.GetStr(loan, "QM.X115");
      nameValueCollection.Add("QM_X115", str44);
      string str45 = JS.GetStr(loan, "QM.X116");
      nameValueCollection.Add("QM_X116", str45);
      string str46 = JS.GetStr(loan, "3275");
      nameValueCollection.Add("3275", str46);
      string str47 = JS.GetStr(loan, "QM.X337");
      nameValueCollection.Add("QM_X337", str47);
      string str48 = JS.GetStr(loan, "QM.X118");
      nameValueCollection.Add("QM_X118", str48);
      string str49 = JS.GetStr(loan, "QM.X119");
      nameValueCollection.Add("QM_X119", str49);
      string str50 = JS.GetStr(loan, "1725");
      nameValueCollection.Add("1725", str50);
      string str51 = JS.GetStr(loan, "1726");
      nameValueCollection.Add("1726", str51);
      string str52 = JS.GetStr(loan, "1727");
      nameValueCollection.Add("1727", str52);
      string str53 = JS.GetStr(loan, "1728");
      nameValueCollection.Add("1728", str53);
      string str54 = JS.GetStr(loan, "1729");
      nameValueCollection.Add("1729", str54);
      string str55 = JS.GetStr(loan, "1730");
      nameValueCollection.Add("1730", str55);
      return nameValueCollection;
    }
  }
}
