// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD1PG2_2010CLASS
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
  public class _HUD1PG2_2010CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("L725", JS.GetStr(loan, "L725"));
      nameValueCollection.Add("L209", JS.GetStr(loan, "L209"));
      nameValueCollection.Add("L210", JS.GetStr(loan, "L210"));
      nameValueCollection.Add("L211", JS.GetStr(loan, "L211"));
      nameValueCollection.Add("L212", JS.GetStr(loan, "L212"));
      nameValueCollection.Add("L213", JS.GetStr(loan, "L213"));
      nameValueCollection.Add("L214", JS.GetStr(loan, "L214"));
      nameValueCollection.Add("L217", JS.GetStr(loan, "L217"));
      nameValueCollection.Add("L215", JS.GetStr(loan, "L215"));
      nameValueCollection.Add("L218", JS.GetStr(loan, "L218"));
      nameValueCollection.Add("NEWHUD_X795", JS.GetStr(loan, "NEWHUD.X795"));
      nameValueCollection.Add("NEWHUD_X797", JS.GetStr(loan, "NEWHUD.X797"));
      nameValueCollection.Add("NEWHUD_X796", JS.GetStr(loan, "NEWHUD.X796"));
      nameValueCollection.Add("617", JS.GetStr(loan, "617"));
      nameValueCollection.Add("624", JS.GetStr(loan, "624"));
      nameValueCollection.Add("L224", JS.GetStr(loan, "L224"));
      nameValueCollection.Add("NEWHUD_X399", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X399"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X399"), ""));
      nameValueCollection.Add("NEWHUD_X126", JS.GetStr(loan, "NEWHUD.X126") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1050"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1050"), ""));
      nameValueCollection.Add("NEWHUD_X127", JS.GetStr(loan, "NEWHUD.X127") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1051"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1051"), ""));
      nameValueCollection.Add("NEWHUD_X128", JS.GetStr(loan, "NEWHUD.X128") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1052"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1052"), ""));
      nameValueCollection.Add("NEWHUD_X129", JS.GetStr(loan, "NEWHUD.X129") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1053"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1053"), ""));
      nameValueCollection.Add("NEWHUD_X130", JS.GetStr(loan, "NEWHUD.X130") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1054"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1054"), ""));
      nameValueCollection.Add("369", JS.GetStr(loan, "369") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1055"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1055"), ""));
      nameValueCollection.Add("371", JS.GetStr(loan, "371") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1056"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1056"), ""));
      nameValueCollection.Add("348", JS.GetStr(loan, "348") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1057"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1057"), ""));
      nameValueCollection.Add("931", JS.GetStr(loan, "931") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1058"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1058"), ""));
      nameValueCollection.Add("1390", JS.GetStr(loan, "1390") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1059"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1059"), ""));
      nameValueCollection.Add("NEWHUD_X1291", JS.GetStr(loan, "NEWHUD.X1291") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1292"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1292"), ""));
      nameValueCollection.Add("NEWHUD_X1299", JS.GetStr(loan, "NEWHUD.X1299") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1300"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1300"), ""));
      nameValueCollection.Add("L244", JS.GetStr(loan, "L244"));
      nameValueCollection.Add("L245", JS.GetStr(loan, "L245"));
      nameValueCollection.Add("333", JS.GetStr(loan, "333"));
      nameValueCollection.Add("1209", JS.GetStr(loan, "1209"));
      nameValueCollection.Add("L248", JS.GetStr(loan, "L248"));
      nameValueCollection.Add("L251", JS.GetStr(loan, "L251"));
      nameValueCollection.Add("L252", JS.GetStr(loan, "L252"));
      nameValueCollection.Add("NEWHUD_X582", JS.GetStr(loan, "NEWHUD.X582") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1062"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1062"), ""));
      nameValueCollection.Add("L259", JS.GetStr(loan, "L259") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1063"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1063"), ""));
      nameValueCollection.Add("1666", JS.GetStr(loan, "1666") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1064"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1064"), ""));
      nameValueCollection.Add("NEWHUD_X583", JS.GetStr(loan, "NEWHUD.X583") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1065"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1065"), ""));
      nameValueCollection.Add("NEWHUD_X584", JS.GetStr(loan, "NEWHUD.X584") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1066"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1066"), ""));
      nameValueCollection.Add("1956", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1956"), "", false) != 0, "to " + JS.GetStr(loan, "1956")));
      nameValueCollection.Add("1500", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1500"), "", false) != 0, "to " + JS.GetStr(loan, "1500")));
      nameValueCollection.Add("NEWHUD_X1716", JS.GetStr(loan, "NEWHUD.X1716"));
      nameValueCollection.Add("1387", JS.GetStr(loan, "1387"));
      nameValueCollection.Add("230", JS.GetStr(loan, "230"));
      nameValueCollection.Add("656", JS.GetStr(loan, "656"));
      nameValueCollection.Add("1296", JS.GetStr(loan, "1296"));
      nameValueCollection.Add("232", JS.GetStr(loan, "232"));
      nameValueCollection.Add("338", JS.GetStr(loan, "338"));
      nameValueCollection.Add("1386", JS.GetStr(loan, "1386"));
      nameValueCollection.Add("231", JS.GetStr(loan, "231"));
      nameValueCollection.Add("655", JS.GetStr(loan, "655"));
      nameValueCollection.Add("L267", JS.GetStr(loan, "L267"));
      nameValueCollection.Add("L268", JS.GetStr(loan, "L268"));
      nameValueCollection.Add("L269", JS.GetStr(loan, "L269"));
      nameValueCollection.Add("1388", JS.GetStr(loan, "1388"));
      nameValueCollection.Add("235", JS.GetStr(loan, "235"));
      nameValueCollection.Add("657", JS.GetStr(loan, "657"));
      nameValueCollection.Add("1628", JS.GetStr(loan, "1628") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X341"), "", false) != 0, " to " + JS.GetStr(loan, "VEND.X341"), ""));
      nameValueCollection.Add("1629", JS.GetStr(loan, "1629"));
      nameValueCollection.Add("1630", JS.GetStr(loan, "1630"));
      nameValueCollection.Add("1631", JS.GetStr(loan, "1631"));
      nameValueCollection.Add("660", JS.GetStr(loan, "660") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X350"), "", false) != 0, " to " + JS.GetStr(loan, "VEND.X350"), ""));
      nameValueCollection.Add("340", JS.GetStr(loan, "340"));
      nameValueCollection.Add("253", JS.GetStr(loan, "253"));
      nameValueCollection.Add("658", JS.GetStr(loan, "658"));
      nameValueCollection.Add("661", JS.GetStr(loan, "661") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "VEND.X359"), "", false) != 0, " to " + JS.GetStr(loan, "VEND.X359"), ""));
      nameValueCollection.Add("341", JS.GetStr(loan, "341"));
      nameValueCollection.Add("254", JS.GetStr(loan, "254"));
      nameValueCollection.Add("659", JS.GetStr(loan, "659"));
      nameValueCollection.Add("NEWHUD_X1706", JS.GetStr(loan, "NEWHUD.X1706"));
      nameValueCollection.Add("NEWHUD_X1707", JS.GetStr(loan, "NEWHUD.X1707"));
      nameValueCollection.Add("NEWHUD_X1708", JS.GetStr(loan, "NEWHUD.X1708"));
      nameValueCollection.Add("558", JS.GetStr(loan, "558"));
      nameValueCollection.Add("NEWHUD_X775", JS.GetStr(loan, "NEWHUD.X775"));
      nameValueCollection.Add("NEWHUD_X708", JS.GetStr(loan, "NEWHUD.X708"));
      nameValueCollection.Add("646", JS.GetStr(loan, "646"));
      nameValueCollection.Add("1634", JS.GetStr(loan, "1634"));
      nameValueCollection.Add("NEWHUD_X206", JS.GetStr(loan, "NEWHUD.X206"));
      nameValueCollection.Add("NEWHUD_X207", JS.GetStr(loan, "NEWHUD.X207"));
      nameValueCollection.Add("NEWHUD_X208", JS.GetStr(loan, "NEWHUD.X208") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1076"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1076"), ""));
      nameValueCollection.Add("NEWHUD_X209", JS.GetStr(loan, "NEWHUD.X209") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1077"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1077"), ""));
      nameValueCollection.Add("1762", JS.GetStr(loan, "1762") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1078"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1078"), ""));
      nameValueCollection.Add("1767", JS.GetStr(loan, "1767") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1079"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1079"), ""));
      nameValueCollection.Add("1772", JS.GetStr(loan, "1772") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1080"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1080"), ""));
      nameValueCollection.Add("1777", JS.GetStr(loan, "1777") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1081"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1081"), ""));
      nameValueCollection.Add("NEWHUD_X202", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X202"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X202"), ""));
      nameValueCollection.Add("NEWHUD_X203", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X203"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X203"), ""));
      nameValueCollection.Add("NEWHUD_X204", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X204"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X204"), ""));
      nameValueCollection.Add("NEWHUD_X205", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X205"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X205"), ""));
      nameValueCollection.Add("NEWHUD_X640", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X640")) != 0.0, "$" + JS.GetStr(loan, "NEWHUD.X640"), ""));
      nameValueCollection.Add("NEWHUD_X641", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X641")) != 0.0, "$" + JS.GetStr(loan, "NEWHUD.X641"), ""));
      nameValueCollection.Add("NEWHUD_X776", JS.GetStr(loan, "NEWHUD.X776"));
      nameValueCollection.Add("390_$", Jed.BF(Jed.Num(JS.GetNum(loan, "390")) > 0.0, "$", ""));
      nameValueCollection.Add("NEWHUD_X778", JS.GetStr(loan, "NEWHUD.X778"));
      nameValueCollection.Add("NEWHUD_X947", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X947"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X947"), ""));
      nameValueCollection.Add("2402", JS.GetStr(loan, "2402"));
      nameValueCollection.Add("2403", JS.GetStr(loan, "2403"));
      nameValueCollection.Add("2404", JS.GetStr(loan, "2404"));
      nameValueCollection.Add("2405", JS.GetStr(loan, "2405"));
      nameValueCollection.Add("2406", JS.GetStr(loan, "2406"));
      nameValueCollection.Add("2407", JS.GetStr(loan, "2407"));
      nameValueCollection.Add("2408", JS.GetStr(loan, "2408"));
      nameValueCollection.Add("373", JS.GetStr(loan, "373") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1082"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1082"), ""));
      nameValueCollection.Add("1640", JS.GetStr(loan, "1640") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1083"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1083"), ""));
      nameValueCollection.Add("1643", JS.GetStr(loan, "1643") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1084"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1084"), ""));
      nameValueCollection.Add("NEWHUD_X251", JS.GetStr(loan, "NEWHUD.X251") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1085"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1085"), ""));
      nameValueCollection.Add("650", JS.GetStr(loan, "650") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1086"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1086"), ""));
      nameValueCollection.Add("651", JS.GetStr(loan, "651") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1087"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1087"), ""));
      nameValueCollection.Add("40", JS.GetStr(loan, "40") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1088"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1088"), ""));
      nameValueCollection.Add("43", JS.GetStr(loan, "43") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1089"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1089"), ""));
      nameValueCollection.Add("1782", JS.GetStr(loan, "1782") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1090"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1090"), ""));
      nameValueCollection.Add("1787", JS.GetStr(loan, "1787") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1091"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1091"), ""));
      nameValueCollection.Add("1792", JS.GetStr(loan, "1792") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1092"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1092"), ""));
      nameValueCollection.Add("NEWHUD_X777", JS.GetStr(loan, "NEWHUD.X777"));
      nameValueCollection.Add("NEWHUD_X773", JS.GetStr(loan, "NEWHUD.X773"));
      nameValueCollection.Add("L216", JS.GetStr(loan, "L216"));
      nameValueCollection.Add("L219", JS.GetStr(loan, "L219"));
      nameValueCollection.Add("NEWHUD_X147", JS.GetStr(loan, "NEWHUD.X147"));
      nameValueCollection.Add("NEWHUD_X148", JS.GetStr(loan, "NEWHUD.X148"));
      nameValueCollection.Add("NEWHUD_X149", JS.GetStr(loan, "NEWHUD.X149"));
      nameValueCollection.Add("NEWHUD_X150", JS.GetStr(loan, "NEWHUD.X150"));
      nameValueCollection.Add("NEWHUD_X151", JS.GetStr(loan, "NEWHUD.X151"));
      nameValueCollection.Add("574", JS.GetStr(loan, "574"));
      nameValueCollection.Add("575", JS.GetStr(loan, "575"));
      nameValueCollection.Add("96", JS.GetStr(loan, "96"));
      nameValueCollection.Add("1345", JS.GetStr(loan, "1345"));
      nameValueCollection.Add("6", JS.GetStr(loan, "6"));
      nameValueCollection.Add("678", JS.GetStr(loan, "678"));
      nameValueCollection.Add("NEWHUD_X658", JS.GetStr(loan, "NEWHUD.X658"));
      nameValueCollection.Add("561", JS.GetStr(loan, "561"));
      nameValueCollection.Add("562", JS.GetStr(loan, "562"));
      nameValueCollection.Add("578", JS.GetStr(loan, "578"));
      nameValueCollection.Add("NEWHUD_X594", JS.GetStr(loan, "NEWHUD.X594"));
      nameValueCollection.Add("571", JS.GetStr(loan, "571"));
      nameValueCollection.Add("579", JS.GetStr(loan, "579"));
      nameValueCollection.Add("L261", JS.GetStr(loan, "L261"));
      nameValueCollection.Add("1668", JS.GetStr(loan, "1668"));
      nameValueCollection.Add("NEWHUD_X595", JS.GetStr(loan, "NEWHUD.X595"));
      nameValueCollection.Add("NEWHUD_X596", JS.GetStr(loan, "NEWHUD.X596"));
      nameValueCollection.Add("NEWHUD_X218", JS.GetStr(loan, "NEWHUD.X218"));
      nameValueCollection.Add("NEWHUD_X219", JS.GetStr(loan, "NEWHUD.X219"));
      nameValueCollection.Add("1764", JS.GetStr(loan, "1764"));
      nameValueCollection.Add("1769", JS.GetStr(loan, "1769"));
      nameValueCollection.Add("1774", JS.GetStr(loan, "1774"));
      nameValueCollection.Add("1779", JS.GetStr(loan, "1779"));
      nameValueCollection.Add("587", JS.GetStr(loan, "587"));
      nameValueCollection.Add("576", JS.GetStr(loan, "576"));
      nameValueCollection.Add("1642", JS.GetStr(loan, "1642"));
      nameValueCollection.Add("1645", JS.GetStr(loan, "1645"));
      nameValueCollection.Add("593", JS.GetStr(loan, "593"));
      nameValueCollection.Add("594", JS.GetStr(loan, "594"));
      nameValueCollection.Add("NEWHUD_X258", JS.GetStr(loan, "NEWHUD.X258"));
      nameValueCollection.Add("590", JS.GetStr(loan, "590"));
      nameValueCollection.Add("591", JS.GetStr(loan, "591"));
      nameValueCollection.Add("42", JS.GetStr(loan, "42"));
      nameValueCollection.Add("55", JS.GetStr(loan, "55"));
      nameValueCollection.Add("1784", JS.GetStr(loan, "1784"));
      nameValueCollection.Add("1789", JS.GetStr(loan, "1789"));
      nameValueCollection.Add("1794", JS.GetStr(loan, "1794"));
      nameValueCollection.Add("NEWHUD_X774", JS.GetStr(loan, "NEWHUD.X774"));
      return nameValueCollection;
    }
  }
}
