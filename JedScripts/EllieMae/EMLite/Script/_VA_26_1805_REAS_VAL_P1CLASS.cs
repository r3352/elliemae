﻿// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_1805_REAS_VAL_P1CLASS
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
  public class _VA_26_1805_REAS_VAL_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "VA", false) == 0, "X");
      nameValueCollection.Add("GAPPR1S1", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1172"), "VA", false) != 0, "X");
      nameValueCollection.Add("GAPPR1S2", str2);
      string str3 = JS.GetStr(loan, "1039");
      nameValueCollection.Add("1039", str3);
      string str4 = JS.GetStr(loan, "1040");
      nameValueCollection.Add("1040", str4);
      string str5 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str5);
      string str6 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str6);
      string str7 = JS.GetStr(loan, "13");
      nameValueCollection.Add("13", str7);
      string str8 = JS.GetStr(loan, "765");
      nameValueCollection.Add("765", str8);
      string str9 = JS.GetStr(loan, "766");
      nameValueCollection.Add("766", str9);
      string str10 = JS.GetStr(loan, "797");
      nameValueCollection.Add("797", str10);
      string str11 = JS.GetStr(loan, "798");
      nameValueCollection.Add("798", str11);
      string str12 = JS.GetStr(loan, "926");
      nameValueCollection.Add("926", str12);
      string str13 = JS.GetStr(loan, "927");
      nameValueCollection.Add("927", str13);
      string str14 = JS.GetStr(loan, "928");
      nameValueCollection.Add("928", str14);
      string str15 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "X");
      nameValueCollection.Add("1041_Condo", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "X");
      nameValueCollection.Add("1041_PUD", str16);
      string str17 = JS.GetStr(loan, "315");
      nameValueCollection.Add("GAPPRX1", str17);
      string str18 = JS.GetStr(loan, "362");
      nameValueCollection.Add("GAPPRX2", str18);
      string str19 = JS.GetStr(loan, "319");
      nameValueCollection.Add("GAPPRX3", str19);
      string str20 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("GAPPRX4", str20);
      string str21 = JS.GetStr(loan, "3347");
      nameValueCollection.Add("3347", str21);
      string str22 = JS.GetStr(loan, "1042");
      nameValueCollection.Add("1042", str22);
      string str23 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1043"), "", false) != 0, "X");
      nameValueCollection.Add("GAPPR1S31", str23);
      string str24 = " " + JS.GetStr(loan, "1043");
      nameValueCollection.Add("1043", str24);
      string str25 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "600"), "", false) != 0, "X");
      nameValueCollection.Add("GAPPR1S32", str25);
      string str26 = " " + JS.GetStr(loan, "600");
      nameValueCollection.Add("600", str26);
      string str27 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1096"), "Public", false) == 0, "X");
      nameValueCollection.Add("1096_Public", str27);
      string str28 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1096"), "Community", false) == 0, "X");
      nameValueCollection.Add("1096_Commun", str28);
      string str29 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1096"), "Individual", false) == 0, "X");
      nameValueCollection.Add("1096_Individ", str29);
      string str30 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1097"), "Public", false) == 0, "X");
      nameValueCollection.Add("1097_Public", str30);
      string str31 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1097"), "Community", false) == 0, "X");
      nameValueCollection.Add("1097_Commun", str31);
      string str32 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1097"), "Individual", false) == 0, " X");
      nameValueCollection.Add("1097_Individ", str32);
      string str33 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1098"), "Public", false) == 0, "X");
      nameValueCollection.Add("1098_Public", str33);
      string str34 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1098"), "Community", false) == 0, "X");
      nameValueCollection.Add("1098_Commun", str34);
      string str35 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1098"), "Individual", false) == 0, "X");
      nameValueCollection.Add("1098_Individ", str35);
      string str36 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1099"), "Public", false) == 0, "X");
      nameValueCollection.Add("1099_Public", str36);
      string str37 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1099"), "Community", false) == 0, "X");
      nameValueCollection.Add("1099_Commun", str37);
      string str38 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1099"), "Individual", false) == 0, "X");
      nameValueCollection.Add("1099_Individ", str38);
      string str39 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "204"), "Y", false) == 0, "X");
      nameValueCollection.Add("204", str39);
      string str40 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "209"), "Y", false) == 0, "X");
      nameValueCollection.Add("209", str40);
      string str41 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "301"), "Y", false) == 0, "X");
      nameValueCollection.Add("301", str41);
      string str42 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "302"), "Y", false) == 0, "X");
      nameValueCollection.Add("302", str42);
      string str43 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "308"), "Y", false) == 0, "X");
      nameValueCollection.Add("308", str43);
      string str44 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "309"), "Y", false) == 0, "X");
      nameValueCollection.Add("309", str44);
      string str45 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "316"), "Y", false) == 0, "X");
      nameValueCollection.Add("316", str45);
      string str46 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "318"), "Y", false) == 0, "X");
      nameValueCollection.Add("318", str46);
      string str47 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "320"), "", false) != 0, "X");
      nameValueCollection.Add("GAPPR1S38", str47);
      string str48 = JS.GetStr(loan, "320");
      nameValueCollection.Add("320", str48);
      string str49 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "601"), "Proposed", false) == 0, "X");
      nameValueCollection.Add("601_Proposed", str49);
      string str50 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "601"), "UnderConstruction", false) == 0, "X");
      nameValueCollection.Add("601_UnderCons", str50);
      string str51 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "601"), "Existing", false) == 0, "X");
      nameValueCollection.Add("601_Existing", str51);
      string str52 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "601"), "SubjectToAlterationImprovementRepairAndRehabilitation", false) == 0, "X");
      nameValueCollection.Add("601_Alterations", str52);
      string str53 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "602"), "Detached", false) == 0, "X");
      nameValueCollection.Add("602_Detached", str53);
      string str54 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "602"), "Semi-Detached", false) == 0, "X");
      nameValueCollection.Add("602_SemiDetach", str54);
      string str55 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "602"), "Row", false) == 0, "X");
      nameValueCollection.Add("602_Row", str55);
      string str56 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "602"), "Apt Unit", false) == 0, "X");
      nameValueCollection.Add("602_AptUnit", str56);
      string str57 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "603"), "Y", false) == 0, "X");
      nameValueCollection.Add("603_Yes", str57);
      string str58 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "603"), "Y", false) != 0, "X");
      nameValueCollection.Add("603_No", str58);
      string str59 = JS.GetStr(loan, "604");
      nameValueCollection.Add("604", str59);
      string str60 = JS.GetStr(loan, "16");
      nameValueCollection.Add("16", str60);
      string str61 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "720"), "Private", false) == 0, "X");
      nameValueCollection.Add("720_Private", str61);
      string str62 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "720"), "Public", false) == 0, "X");
      nameValueCollection.Add("720_Public", str62);
      string str63 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "721"), "Private", false) == 0, "X");
      nameValueCollection.Add("721_Private", str63);
      string str64 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "721"), "Public", false) == 0, "X");
      nameValueCollection.Add("721_Public", str64);
      string str65 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "722"), "Y", false) == 0, "X");
      nameValueCollection.Add("722_Yes", str65);
      string str66 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "722"), "Y", false) != 0, "X");
      nameValueCollection.Add("722_No", str66);
      string str67 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "722"), "Y", false) == 0, JS.GetStr(loan, "724"));
      nameValueCollection.Add("724", str67);
      string str68 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "722"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "725"), "//", false) != 0, JS.GetStr(loan, "725")));
      nameValueCollection.Add("725", str68);
      string str69 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "726"), "//", false) != 0, JS.GetStr(loan, "726"));
      nameValueCollection.Add("726", str69);
      string str70 = JS.GetStr(loan, "727");
      nameValueCollection.Add("727", str70);
      string str71 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "728"), "Occupied by owner", false) == 0, "X");
      nameValueCollection.Add("728_OccupOwner", str71);
      string str72 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "728"), "Never occupied", false) == 0, "X");
      nameValueCollection.Add("728_NeverOcc", str72);
      string str73 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "728"), "Vacant", false) == 0, "X");
      nameValueCollection.Add("728_Vacant", str73);
      string str74 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "728"), "Occupied by tenant", false) == 0, "X");
      nameValueCollection.Add("728_OccuTenant", str74);
      string str75 = JS.GetStr(loan, "729");
      nameValueCollection.Add("729", str75);
      string str76 = JS.GetStr(loan, "730");
      nameValueCollection.Add("730", str76);
      string str77 = JS.GetStr(loan, "731");
      nameValueCollection.Add("731", str77);
      string str78 = JS.GetStr(loan, "315");
      nameValueCollection.Add("638", str78);
      string str79 = JS.GetStr(loan, "324");
      nameValueCollection.Add("704", str79);
      string str80 = JS.GetStr(loan, "681");
      nameValueCollection.Add("681", str80);
      string str81 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "681"), "", false) != 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "686"), "Y", false) == 0, "X"));
      nameValueCollection.Add("686_AM", str81);
      string str82 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "681"), "", false) != 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "686"), "N", false) == 0, "X"));
      nameValueCollection.Add("686_PM", str82);
      string str83 = JS.GetStr(loan, "683");
      nameValueCollection.Add("683", str83);
      string str84 = JS.GetStr(loan, "1059");
      nameValueCollection.Add("1059", str84);
      string str85 = JS.GetStr(loan, "1060");
      nameValueCollection.Add("1060", str85);
      string str86 = JS.GetStr(loan, "352");
      nameValueCollection.Add("352", str86);
      string str87 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("GAPPR1S3", str87);
      string str88 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("GAPPR1S30", str88);
      string str89 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("GAPPR1S4", str89);
      string str90 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1020"), "FHA", false) == 0, "X");
      nameValueCollection.Add("1020_FHA", str90);
      string str91 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1020"), "VA", false) == 0, "X");
      nameValueCollection.Add("1020_VA", str91);
      string str92 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1020"), "None", false) == 0, "X");
      nameValueCollection.Add("1020_NONE", str92);
      string str93 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1021"), "First Submission", false) == 0, "X");
      nameValueCollection.Add("1021_First", str93);
      string str94 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1021"), "Repeat Case", false) == 0, "X");
      nameValueCollection.Add("1021_Repeat", str94);
      string str95 = JS.GetStr(loan, "1022");
      nameValueCollection.Add("1022", str95);
      string str96 = JS.GetStr(loan, "713");
      nameValueCollection.Add("713", str96);
      string str97 = JS.GetStr(loan, "715");
      nameValueCollection.Add("715", str97);
      string str98 = JS.GetStr(loan, "716") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1253"), "", false) != 0, ", ") + JS.GetStr(loan, "1253") + " " + JS.GetStr(loan, "717");
      nameValueCollection.Add("716_1253_717", str98);
      string str99 = JS.GetStr(loan, "3348");
      nameValueCollection.Add("3348", str99);
      string str100 = JS.GetStr(loan, "718");
      nameValueCollection.Add("718", str100);
      string str101 = JS.GetStr(loan, "1025");
      nameValueCollection.Add("1025", str101);
      string str102 = JS.GetStr(loan, "1026");
      nameValueCollection.Add("1026", str102);
      string str103 = JS.GetStr(loan, "GAPPR.X11") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "GAPPR.X12"), "", false) != 0, ", ") + JS.GetStr(loan, "GAPPR.X12") + " " + JS.GetStr(loan, "GAPPR.X13");
      nameValueCollection.Add("GAPPRX11", str103);
      string str104 = JS.GetStr(loan, "1027");
      nameValueCollection.Add("1027", str104);
      string str105 = JS.GetStr(loan, "3349");
      nameValueCollection.Add("3349", str105);
      string str106 = JS.GetStr(loan, "3350");
      nameValueCollection.Add("3350", str106);
      string str107 = JS.GetStr(loan, "3351") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "3352"), "", false) != 0, ", ") + JS.GetStr(loan, "3352") + " " + JS.GetStr(loan, "3353");
      nameValueCollection.Add("3351_3352_3353", str107);
      string str108 = JS.GetStr(loan, "3354");
      nameValueCollection.Add("3354", str108);
      string str109 = JS.GetStr(loan, "1031");
      nameValueCollection.Add("1031", str109);
      string str110 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1032"), "", false) != 0, "X");
      nameValueCollection.Add("GAPPR1S5", str110);
      string str111 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1032"), "", false) == 0, "X");
      nameValueCollection.Add("GAPPR1S7", str111);
      string str112 = JS.GetStr(loan, "1032");
      nameValueCollection.Add("1032", str112);
      string str113 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1033"), "99 Years", false) == 0, "X");
      nameValueCollection.Add("1033_99Yrs", str113);
      string str114 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1033"), "Renewable", false) == 0, "X");
      nameValueCollection.Add("1033_Renew", str114);
      string str115 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1033"), "HUD/VA approved", false) == 0, "X");
      nameValueCollection.Add("1033_HUD/VA", str115);
      string str116 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1034"), "//", false) != 0, JS.GetStr(loan, "1034"));
      nameValueCollection.Add("1034", str116);
      string str117 = JS.GetStr(loan, "1035");
      nameValueCollection.Add("1035", str117);
      string str118 = JS.GetStr(loan, "136");
      nameValueCollection.Add("136", str118);
      string str119 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1344"), "Y", false) == 0, "X");
      nameValueCollection.Add("1344_Yes", str119);
      string str120 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1344"), "Y", false) != 0, "X");
      nameValueCollection.Add("1344_No", str120);
      string str121 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2", str121);
      string str122 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1036"), "Y", false) == 0, "X");
      nameValueCollection.Add("1036_Yes", str122);
      string str123 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "1036"), "Y", false) != 0, "X");
      nameValueCollection.Add("1036_No", str123);
      string str124 = JS.GetStr(loan, "1037");
      nameValueCollection.Add("1037", str124);
      string str125 = JS.GetStr(loan, "GAPPR.X5");
      nameValueCollection.Add("GAPPRS5", str125);
      string str126 = JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str126);
      string str127 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("363", str127);
      string str128 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "415"), "//", false) != 0, JS.GetStr(loan, "415"));
      nameValueCollection.Add("415", str128);
      string str129 = JS.GetStr(loan, "618");
      nameValueCollection.Add("618", str129);
      return nameValueCollection;
    }
  }
}
