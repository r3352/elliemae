// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._NETTANGIBLEBENEFITDISP1CLASS
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
  public class _NETTANGIBLEBENEFITDISP1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str3);
      string str4 = JS.GetStr(loan, "3142");
      nameValueCollection.Add("3142", str4);
      string str5 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str5);
      string str6 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str6);
      string str7 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "12"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1264"), "", false) != 0, JS.GetStr(loan, "1264"), JS.GetStr(loan, "VEND.X293"));
      nameValueCollection.Add("1264", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1264"), "", false) != 0, JS.GetStr(loan, "3032"), JS.GetStr(loan, "VEND.X300"));
      nameValueCollection.Add("3032", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1264"), "", false) != 0, JS.GetStr(loan, "3244"), JS.GetStr(loan, "VEND.X527"));
      nameValueCollection.Add("3244", str10);
      string str11 = JS.GetStr(loan, "1612");
      nameValueCollection.Add("1612", str11);
      string str12 = JS.GetStr(loan, "2306");
      nameValueCollection.Add("2306", str12);
      string str13 = JS.GetStr(loan, "3238");
      nameValueCollection.Add("3238", str13);
      string str14 = JS.GetStr(loan, "763");
      nameValueCollection.Add("763", str14);
      string str15 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str15);
      string str16 = JS.GetStr(loan, "325");
      nameValueCollection.Add("325", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("608_FIX", str17);
      string str18 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X");
      nameValueCollection.Add("608_ARM", str18);
      string str19 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "X");
      nameValueCollection.Add("1659", str19);
      string str20 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str20);
      string str21 = JS.GetStr(loan, "1827");
      nameValueCollection.Add("1827", str21);
      string str22 = JS.GetStr(loan, "2625");
      nameValueCollection.Add("2625", str22);
      string str23 = JS.GetStr(loan, "799");
      nameValueCollection.Add("799", str23);
      string str24 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1550"), "Y", false) == 0, "X");
      nameValueCollection.Add("1550_Y", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1550"), "Y", false) != 0, "X");
      nameValueCollection.Add("1550_N", str25);
      string str26 = JS.GetStr(loan, "5");
      nameValueCollection.Add("5", str26);
      string str27 = JS.GetStr(loan, "NEWHUD.X217");
      nameValueCollection.Add("NEWHUD_X217", str27);
      string str28 = JS.GetStr(loan, "912");
      nameValueCollection.Add("912", str28);
      string str29 = JS.GetStr(loan, "353");
      nameValueCollection.Add("353", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X357"), "Y", false) == 0, "X");
      nameValueCollection.Add("NEWHUD_X357_Y", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X357"), "Y", false) != 0, "X");
      nameValueCollection.Add("NEWHUD_X357_N", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "Y", false) == 0, "X");
      nameValueCollection.Add("675_Y", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "675"), "N", false) == 0, "X");
      nameValueCollection.Add("675_N", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X25"), "Y", false) == 0, "X");
      nameValueCollection.Add("NTB_X25_Y", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X25"), "N", false) == 0, "X");
      nameValueCollection.Add("NTB_X25_N", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "X");
      nameValueCollection.Add("1659_Y", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) != 0, "X");
      nameValueCollection.Add("1659_N", str37);
      string str38 = JS.GetStr(loan, "NTB.X1");
      nameValueCollection.Add("NTB_X1", str38);
      string str39 = JS.GetStr(loan, "NTB.X2");
      nameValueCollection.Add("NTB_X2", str39);
      string str40 = JS.GetStr(loan, "NTB.X3");
      nameValueCollection.Add("NTB_X3", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X5"), "Fixed", false) == 0, "X");
      nameValueCollection.Add("NTB_X5_FIX", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X5"), "AdjustableRate", false) == 0, "X");
      nameValueCollection.Add("NTB_X5_ARM", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X61"), "Y", false) == 0, "X");
      nameValueCollection.Add("NTB_X61", str43);
      string str44 = JS.GetStr(loan, "NTB.X7");
      nameValueCollection.Add("NTB_X7", str44);
      string str45 = JS.GetStr(loan, "NTB.X9");
      nameValueCollection.Add("NTB_X9", str45);
      string str46 = JS.GetStr(loan, "NTB.X10");
      nameValueCollection.Add("NTB_X10", str46);
      string str47 = JS.GetStr(loan, "NTB.X8");
      nameValueCollection.Add("NTB_X8", str47);
      string str48 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X38"), "Y", false) == 0, "X");
      nameValueCollection.Add("NTB_X38_Y", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X38"), "Y", false) != 0, "X");
      nameValueCollection.Add("NTB_X38_N", str49);
      string str50 = JS.GetStr(loan, "NTB.X20");
      nameValueCollection.Add("NTB_X20", str50);
      string str51 = JS.GetStr(loan, "NTB.X22");
      nameValueCollection.Add("NTB_X22", str51);
      string str52 = JS.GetStr(loan, "NTB.X21");
      nameValueCollection.Add("NTB_X21", str52);
      string str53 = JS.GetStr(loan, "NTB.X17");
      nameValueCollection.Add("NTB_X17", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X22"), "", false) != 0, "X");
      nameValueCollection.Add("NTB_X22_Y", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X22"), "", false) == 0, "X");
      nameValueCollection.Add("NTB_X22_N", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X13"), "Y", false) == 0, "X");
      nameValueCollection.Add("NTB_X13_Y", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X13"), "N", false) == 0, "X");
      nameValueCollection.Add("NTB_X13_N", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X12"), "Y", false) == 0, "X");
      nameValueCollection.Add("NTB_X12_Y", str58);
      string str59 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X12"), "N", false) == 0, "X");
      nameValueCollection.Add("NTB_X12_N", str59);
      string str60 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X61"), "Y", false) == 0, "X");
      nameValueCollection.Add("NTB_X61_Y", str60);
      string str61 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X61"), "Y", false) != 0, "X");
      nameValueCollection.Add("NTB_X61_N", str61);
      return nameValueCollection;
    }
  }
}
