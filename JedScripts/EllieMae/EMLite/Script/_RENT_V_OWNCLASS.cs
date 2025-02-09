// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._RENT_V_OWNCLASS
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
  public class _RENT_V_OWNCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "PREQUAL.X328");
      nameValueCollection.Add("PREQUAL_X328", str1);
      string str2 = JS.GetStr(loan, "PREQUAL.X216");
      nameValueCollection.Add("PREQUAL_X216", str2);
      string str3 = JS.GetStr(loan, "PREQUAL.X228");
      nameValueCollection.Add("PREQUAL_X228", str3);
      string str4 = JS.GetStr(loan, "PREQUAL.X335");
      nameValueCollection.Add("PREQUAL_X335_4", str4);
      string str5 = JS.GetStr(loan, "1771");
      nameValueCollection.Add("1771", str5);
      string str6 = JS.GetStr(loan, "1405");
      nameValueCollection.Add("1405", str6);
      string str7 = JS.GetStr(loan, "1335");
      nameValueCollection.Add("1335", str7);
      string str8 = JS.GetStr(loan, "230");
      nameValueCollection.Add("230", str8);
      string str9 = JS.GetStr(loan, "PREQUAL.X107");
      nameValueCollection.Add("PREQUAL_X107", str9);
      string str10 = JS.GetStr(loan, "PREQUAL.X327");
      nameValueCollection.Add("PREQUAL_X327", str10);
      string str11 = JS.GetStr(loan, "PREQUAL.X326");
      nameValueCollection.Add("PREQUAL_X326", str11);
      string str12 = JS.GetStr(loan, "PREQUAL.X325");
      nameValueCollection.Add("PREQUAL_X325", str12);
      string str13 = JS.GetStr(loan, "PREQUAL.X324");
      nameValueCollection.Add("PREQUAL_X324", str13);
      string str14 = JS.GetStr(loan, "PREQUAL.X323");
      nameValueCollection.Add("PREQUAL_X323", str14);
      string str15 = JS.GetStr(loan, "PREQUAL.X322");
      nameValueCollection.Add("PREQUAL_X322", str15);
      string str16 = JS.GetStr(loan, "PREQUAL.X321");
      nameValueCollection.Add("PREQUAL_X321", str16);
      string str17 = JS.GetStr(loan, "PREQUAL.X320");
      nameValueCollection.Add("PREQUAL_X320", str17);
      string str18 = JS.GetStr(loan, "1405");
      nameValueCollection.Add("1405_2", str18);
      string str19 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str19);
      string str20 = JS.GetStr(loan, "230");
      nameValueCollection.Add("230_2", str20);
      string str21 = JS.GetStr(loan, "304");
      nameValueCollection.Add("304", str21);
      string str22 = JS.GetStr(loan, "1612");
      nameValueCollection.Add("1612", str22);
      string str23 = JS.GetStr(loan, "PREQUAL.X316");
      nameValueCollection.Add("PREQUAL_X316", str23);
      string str24 = JS.GetStr(loan, "5");
      nameValueCollection.Add("5", str24);
      string str25 = JS.GetStr(loan, "4");
      nameValueCollection.Add("4", str25);
      string str26 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str26);
      string str27 = JS.GetStr(loan, "PREQUAL.X319");
      nameValueCollection.Add("PREQUAL_X319", str27);
      string str28 = JS.GetStr(loan, "PREQUAL.X318");
      nameValueCollection.Add("PREQUAL_X318", str28);
      string str29 = JS.GetStr(loan, "PREQUAL.X317");
      nameValueCollection.Add("PREQUAL_X317", str29);
      string str30 = JS.GetStr(loan, "PREQUAL.X319");
      nameValueCollection.Add("PREQUAL_X319_2", str30);
      string str31 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str31);
      string str32 = JS.GetStr(loan, "PREQUAL.X223");
      nameValueCollection.Add("PREQUAL_X223", str32);
      string str33 = JS.GetStr(loan, "1109");
      nameValueCollection.Add("1109", str33);
      string str34 = JS.GetStr(loan, "PREQUAL.X322");
      nameValueCollection.Add("PREQUAL_X322_2", str34);
      string str35 = JS.GetStr(loan, "PREQUAL.X332");
      nameValueCollection.Add("PREQUAL_X332", str35);
      string str36 = JS.GetStr(loan, "PREQUAL.X331");
      nameValueCollection.Add("PREQUAL_X331", str36);
      string str37 = JS.GetStr(loan, "PREQUAL.X330");
      nameValueCollection.Add("PREQUAL_X330", str37);
      string str38 = JS.GetStr(loan, "PREQUAL.X335");
      nameValueCollection.Add("PREQUAL_X335", str38);
      string str39 = JS.GetStr(loan, "PREQUAL.X334");
      nameValueCollection.Add("PREQUAL_X334", str39);
      string str40 = JS.GetStr(loan, "PREQUAL.X333");
      nameValueCollection.Add("PREQUAL_X333", str40);
      string str41 = JS.GetStr(loan, "119");
      nameValueCollection.Add("119", str41);
      string str42 = JS.GetStr(loan, "119");
      nameValueCollection.Add("119_2", str42);
      string str43 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str43);
      string str44 = JS.GetStr(loan, "PREQUAL.X335");
      nameValueCollection.Add("PREQUAL_X335_5", str44);
      string str45 = JS.GetStr(loan, "PREQUAL.X335");
      nameValueCollection.Add("PREQUAL_X335_3", str45);
      string str46 = JS.GetStr(loan, "PREQUAL.X335");
      nameValueCollection.Add("PREQUAL_X335_2", str46);
      string str47 = JS.GetStr(loan, "PREQUAL.X329");
      nameValueCollection.Add("PREQUAL_X329", str47);
      string str48 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str48);
      string str49 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str49);
      string str50 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str50);
      return nameValueCollection;
    }
  }
}
