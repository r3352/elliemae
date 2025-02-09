// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_1A__2010__P2CLASS
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
  public class _HUD_1A__2010__P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("NEWHUD_X571", JS.GetStr(loan, "NEWHUD.X571"));
      nameValueCollection.Add("NEWHUD_X645", JS.GetStr(loan, "NEWHUD.X645"));
      nameValueCollection.Add("NEWHUD_X572", JS.GetStr(loan, "NEWHUD.X572"));
      nameValueCollection.Add("NEWHUD_X639", JS.GetStr(loan, "NEWHUD.X639"));
      nameValueCollection.Add("646", JS.GetStr(loan, "646"));
      nameValueCollection.Add("1634", JS.GetStr(loan, "1634"));
      nameValueCollection.Add("NEWHUD_X206", JS.GetStr(loan, "NEWHUD.X206"));
      nameValueCollection.Add("NEWHUD_X207", JS.GetStr(loan, "NEWHUD.X207"));
      nameValueCollection.Add("NEWHUD_X640", JS.GetStr(loan, "NEWHUD.X640"));
      nameValueCollection.Add("NEWHUD_X641", JS.GetStr(loan, "NEWHUD.X641"));
      nameValueCollection.Add("NEWHUD_X215", JS.GetStr(loan, "NEWHUD.X215"));
      nameValueCollection.Add("NEWHUD_X215_$", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X215")) > 0.0, "$", ""));
      nameValueCollection.Add("NEWHUD_X216", JS.GetStr(loan, "NEWHUD.X216"));
      nameValueCollection.Add("NEWHUD_X216_$", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X216")) > 0.0, "$", ""));
      nameValueCollection.Add("1763", JS.GetStr(loan, "1763"));
      nameValueCollection.Add("1763_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1763")) > 0.0, "$", ""));
      nameValueCollection.Add("1768", JS.GetStr(loan, "1768"));
      nameValueCollection.Add("1768_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1768")) > 0.0, "$", ""));
      nameValueCollection.Add("1773", JS.GetStr(loan, "1773"));
      nameValueCollection.Add("1773_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1773")) > 0.0, "$", ""));
      nameValueCollection.Add("1778", JS.GetStr(loan, "1778"));
      nameValueCollection.Add("1778_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1778")) > 0.0, "$", ""));
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
      nameValueCollection.Add("NEWHUD_X607", JS.GetStr(loan, "NEWHUD.X607"));
      nameValueCollection.Add("390", JS.GetStr(loan, "390"));
      nameValueCollection.Add("390_$", Jed.BF(Jed.Num(JS.GetNum(loan, "390")) > 0.0, "$", ""));
      nameValueCollection.Add("NEWHUD_X700", Jed.BF(Jed.Num(JS.GetNum(loan, "NEWHUD.X700")) - Jed.Num(JS.GetNum(loan, "NEWHUD.X868")) - Jed.Num(JS.GetNum(loan, "NEWHUD.X869")) - Jed.Num(JS.GetNum(loan, "NEWHUD.X870")) != 0.0, Jed.NF(Jed.Num(JS.GetNum(loan, "NEWHUD.X700")) - Jed.Num(JS.GetNum(loan, "NEWHUD.X868")) - Jed.Num(JS.GetNum(loan, "NEWHUD.X869")) - Jed.Num(JS.GetNum(loan, "NEWHUD.X870")), (byte) 18, 0), ""));
      nameValueCollection.Add("647", JS.GetStr(loan, "647"));
      nameValueCollection.Add("647_$", Jed.BF(Jed.Num(JS.GetNum(loan, "647")) > 0.0, "$", ""));
      nameValueCollection.Add("648", JS.GetStr(loan, "648"));
      nameValueCollection.Add("648_$", Jed.BF(Jed.Num(JS.GetNum(loan, "648")) > 0.0, "$", ""));
      nameValueCollection.Add("374", JS.GetStr(loan, "374"));
      nameValueCollection.Add("374_$", Jed.BF(Jed.Num(JS.GetNum(loan, "374")) > 0.0, "$", ""));
      nameValueCollection.Add("1641", JS.GetStr(loan, "1641"));
      nameValueCollection.Add("1641_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1641")) > 0.0, "$", ""));
      nameValueCollection.Add("1644", JS.GetStr(loan, "1644"));
      nameValueCollection.Add("1644_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1644")) > 0.0, "$", ""));
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
      nameValueCollection.Add("NEWHUD_X947", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X947"), "", false) != 0, "to " + JS.GetStr(loan, "NEWHUD.X947"), ""));
      nameValueCollection.Add("NEWHUD_X251", JS.GetStr(loan, "NEWHUD.X251") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1085"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1085"), ""));
      nameValueCollection.Add("650", JS.GetStr(loan, "650") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1086"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1086"), ""));
      nameValueCollection.Add("651", JS.GetStr(loan, "651") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1087"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1087"), ""));
      nameValueCollection.Add("40", JS.GetStr(loan, "40") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1088"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1088"), ""));
      nameValueCollection.Add("43", JS.GetStr(loan, "43") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1089"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1089"), ""));
      nameValueCollection.Add("1782", JS.GetStr(loan, "1782") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1090"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1090"), ""));
      nameValueCollection.Add("1787", JS.GetStr(loan, "1787") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1091"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1091"), ""));
      nameValueCollection.Add("1792", JS.GetStr(loan, "1792") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1092"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1092"), ""));
      nameValueCollection.Add("NEWHUD_X252", JS.GetStr(loan, "NEWHUD.X252") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1093"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1093"), ""));
      nameValueCollection.Add("NEWHUD_X253", JS.GetStr(loan, "NEWHUD.X253") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1094"), "", false) != 0, " to " + JS.GetStr(loan, "NEWHUD.X1094"), ""));
      nameValueCollection.Add("NEWHUD_X603", JS.GetStr(loan, "NEWHUD.X603"));
      nameValueCollection.Add("NEWHUD_X254", JS.GetStr(loan, "NEWHUD.X254"));
      nameValueCollection.Add("644", JS.GetStr(loan, "644"));
      nameValueCollection.Add("645", JS.GetStr(loan, "645"));
      nameValueCollection.Add("645_$", Jed.BF(Jed.Num(JS.GetNum(loan, "645")) > 0.0, "$", ""));
      nameValueCollection.Add("41", JS.GetStr(loan, "41"));
      nameValueCollection.Add("41_$", Jed.BF(Jed.Num(JS.GetNum(loan, "41")) > 0.0, "$", ""));
      nameValueCollection.Add("44", JS.GetStr(loan, "44"));
      nameValueCollection.Add("44_$", Jed.BF(Jed.Num(JS.GetNum(loan, "44")) > 0.0, "$", ""));
      nameValueCollection.Add("1783", JS.GetStr(loan, "1783"));
      nameValueCollection.Add("1783_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1783")) > 0.0, "$", ""));
      nameValueCollection.Add("1788", JS.GetStr(loan, "1788"));
      nameValueCollection.Add("1788_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1788")) > 0.0, "$", ""));
      nameValueCollection.Add("1793", JS.GetStr(loan, "1793"));
      nameValueCollection.Add("1793_$", Jed.BF(Jed.Num(JS.GetNum(loan, "1793")) > 0.0, "$", ""));
      nameValueCollection.Add("NEWHUD_X255", JS.GetStr(loan, "NEWHUD.X255"));
      nameValueCollection.Add("NEWHUD_X256", JS.GetStr(loan, "NEWHUD.X256"));
      nameValueCollection.Add("NEWHUD_X277", JS.GetStr(loan, "NEWHUD.X277"));
      return nameValueCollection;
    }
  }
}
