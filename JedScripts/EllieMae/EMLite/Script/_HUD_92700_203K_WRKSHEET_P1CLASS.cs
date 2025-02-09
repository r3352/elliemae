// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_92700_203K_WRKSHEET_P1CLASS
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
  public class _HUD_92700_203K_WRKSHEET_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str3);
      string str4 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str4);
      string str5 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str5);
      string str6 = JS.GetStr(loan, "16");
      nameValueCollection.Add("16", str6);
      string str7 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X1"), "Y", false) == 0, "X");
      nameValueCollection.Add("MAX23KX1_Yes", str7);
      string str8 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X1"), "N", false) == 0 | Operators.CompareString(JS.GetStr(loan, "MAX23K.X1"), "", false) == 0, "X");
      nameValueCollection.Add("MAX23KX1_No", str8);
      string str9 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Purchase", false) == 0, "X");
      nameValueCollection.Add("19", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "19"), "Cash-Out Refinance", false) == 0 | Operators.CompareString(JS.GetStr(loan, "19"), "NoCash-Out Refinance", false) == 0, "X");
      nameValueCollection.Add("1151", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X78"), "Y", false) == 0, "X");
      nameValueCollection.Add("MAX23KX78", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1518"), "//", false) != 0, JS.GetStr(loan, "1518"), "");
      nameValueCollection.Add("1518", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X69"), "OwnedOccupant", false) == 0, "X");
      nameValueCollection.Add("MAX23KX69_OwnerOcc", str13);
      string str14 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X69"), "Nonprofit", false) == 0, "X");
      nameValueCollection.Add("MAX23KX69_Nonprofit", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X69"), "GovernmentAgency", false) == 0, "X");
      nameValueCollection.Add("MAX23KX69_GovnAgncy", str15);
      string str16 = JS.GetStr(loan, "MAX23K.X40");
      nameValueCollection.Add("MAX23KX40", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MAX23K.X75"), "Y", false) == 0, "X");
      nameValueCollection.Add("MAX23KX75", str17);
      string str18 = JS.GetStr(loan, "MAX23K.X5");
      nameValueCollection.Add("MAX23KX5", str18);
      string str19 = JS.GetStr(loan, "MAX23K.X6");
      nameValueCollection.Add("MAX23KX6", str19);
      string str20 = JS.GetStr(loan, "MAX23K.X7");
      nameValueCollection.Add("MAX23KX7", str20);
      string str21 = JS.GetStr(loan, "1132");
      nameValueCollection.Add("1132", str21);
      string str22 = JS.GetStr(loan, "MAX23K.X8");
      nameValueCollection.Add("MAX23KX8", str22);
      string str23 = JS.GetStr(loan, "MAX23K.X70");
      nameValueCollection.Add("MAX23KX70", str23);
      string str24 = JS.GetStr(loan, "MAX23K.X9");
      nameValueCollection.Add("MAX23KX9", str24);
      string str25 = JS.GetStr(loan, "MAX23K.X10");
      nameValueCollection.Add("MAX23KX10", str25);
      string str26 = JS.GetStr(loan, "MAX23K.X11");
      nameValueCollection.Add("MAX23KX11", str26);
      string str27 = JS.GetStr(loan, "MAX23K.X12");
      nameValueCollection.Add("MAX23KX12", str27);
      string str28 = JS.GetStr(loan, "MAX23K.X13");
      nameValueCollection.Add("MAX23KX13", str28);
      string str29 = JS.GetStr(loan, "MAX23K.X14");
      nameValueCollection.Add("MAX23KX14", str29);
      string str30 = JS.GetStr(loan, "MAX23K.X15");
      nameValueCollection.Add("MAX23KX15", str30);
      string str31 = JS.GetStr(loan, "MAX23K.X16");
      nameValueCollection.Add("MAX23KX16", str31);
      string str32 = JS.GetStr(loan, "MAX23K.X63");
      nameValueCollection.Add("MAX23KX63", str32);
      string str33 = JS.GetStr(loan, "MAX23K.X17");
      nameValueCollection.Add("MAX23KX17", str33);
      string str34 = JS.GetStr(loan, "MAX23K.X18");
      nameValueCollection.Add("MAX23KX18", str34);
      string str35 = JS.GetStr(loan, "MAX23K.X19");
      nameValueCollection.Add("MAX23KX19", str35);
      string str36 = JS.GetStr(loan, "MAX23K.X77");
      nameValueCollection.Add("MAX23KX77", str36);
      string str37 = JS.GetStr(loan, "MAX23K.X20");
      nameValueCollection.Add("MAX23KX20", str37);
      string str38 = JS.GetStr(loan, "MAX23K.X21");
      nameValueCollection.Add("MAX23KX21", str38);
      string str39 = JS.GetStr(loan, "MAX23K.X22");
      nameValueCollection.Add("MAX23KX22", str39);
      string str40 = JS.GetStr(loan, "MAX23K.X23");
      nameValueCollection.Add("MAX23KX23", str40);
      string str41 = JS.GetStr(loan, "MAX23K.X24");
      nameValueCollection.Add("MAX23KX24", str41);
      string str42 = JS.GetStr(loan, "MAX23K.X25");
      nameValueCollection.Add("MAX23KX25", str42);
      string str43 = JS.GetStr(loan, "MAX23K.X44");
      nameValueCollection.Add("MAX23KX44", str43);
      string str44 = JS.GetStr(loan, "MAX23K.X26");
      nameValueCollection.Add("MAX23KX26", str44);
      string str45 = JS.GetStr(loan, "MAX23K.X27");
      nameValueCollection.Add("MAX23KX27", str45);
      string str46 = JS.GetStr(loan, "MAX23K.X28");
      nameValueCollection.Add("MAX23KX28", str46);
      string str47 = JS.GetStr(loan, "MAX23K.X29");
      nameValueCollection.Add("MAX23KX29", str47);
      string str48 = JS.GetStr(loan, "MAX23K.X30");
      nameValueCollection.Add("MAX23KX30", str48);
      string str49 = JS.GetStr(loan, "MAX23K.X61");
      nameValueCollection.Add("MAX23KX61", str49);
      string str50 = JS.GetStr(loan, "MAX23K.X32");
      nameValueCollection.Add("MAX23KX32", str50);
      string str51 = JS.GetStr(loan, "MAX23K.X33");
      nameValueCollection.Add("MAX23KX33", str51);
      string str52 = Jed.NF(Jed.S2N(JS.GetStr(loan, "MAX23K.X35")), (byte) 2, 0);
      nameValueCollection.Add("MAX23KX35", str52);
      string str53 = JS.GetStr(loan, "MAX23K.X36");
      nameValueCollection.Add("MAX23KX36", str53);
      string str54 = JS.GetStr(loan, "MAX23K.X37");
      nameValueCollection.Add("MAX23KX37", str54);
      string str55 = JS.GetStr(loan, "1134");
      nameValueCollection.Add("1134", str55);
      string str56 = JS.GetStr(loan, "MAX23K.X41");
      nameValueCollection.Add("MAX23KX41", str56);
      string str57 = JS.GetStr(loan, "MAX23K.X42");
      nameValueCollection.Add("MAX23KX42", str57);
      string str58 = JS.GetStr(loan, "MAX23K.X43");
      nameValueCollection.Add("MAX23KX43", str58);
      string str59 = JS.GetStr(loan, "MAX23K.X45");
      nameValueCollection.Add("MAX23KX45", str59);
      string str60 = JS.GetStr(loan, "MAX23K.X46");
      nameValueCollection.Add("MAX23KX46", str60);
      string str61 = JS.GetStr(loan, "MAX23K.X47");
      nameValueCollection.Add("MAX23KX47", str61);
      string str62 = JS.GetStr(loan, "MAX23K.X67");
      nameValueCollection.Add("MAX23KX67", str62);
      string str63 = JS.GetStr(loan, "1107");
      nameValueCollection.Add("1107", str63);
      string str64 = JS.GetStr(loan, "1045");
      nameValueCollection.Add("1045", str64);
      string str65 = JS.GetStr(loan, "MAX23K.X68");
      nameValueCollection.Add("MAX23KX68", str65);
      string str66 = JS.GetStr(loan, "3");
      nameValueCollection.Add("3", str66);
      string str67 = JS.GetStr(loan, "1061");
      nameValueCollection.Add("1061", str67);
      string str68 = JS.GetStr(loan, "980");
      nameValueCollection.Add("980", str68);
      string str69 = JS.GetStr(loan, "MAX23K.X79");
      nameValueCollection.Add("MAX23KX79", str69);
      return nameValueCollection;
    }
  }
}
