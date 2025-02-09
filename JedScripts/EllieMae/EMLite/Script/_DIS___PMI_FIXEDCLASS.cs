// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._DIS___PMI_FIXEDCLASS
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
  public class _DIS___PMI_FIXEDCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str1);
      string str2 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str2);
      string str3 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str3);
      string str4 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "363"), "//", false) != 0, JS.GetStr(loan, "363"));
      nameValueCollection.Add("363", str5);
      string str6 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str6);
      string str7 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_", str7);
      string str8 = "";
      nameValueCollection.Add("109", str8);
      string str9 = "";
      nameValueCollection.Add("118", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3531"), "Y", false) == 0, "X");
      nameValueCollection.Add("3531", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "3532"), "Y", false) == 0, "X");
      nameValueCollection.Add("3532", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "325"), JS.GetStr(loan, "4"), false) != 0, "Because of your balloon feature, this date may not be reached before the loan matures.");
      nameValueCollection.Add("2", str12);
      string str13 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "325"), "", false) != 0 & Operators.CompareString(JS.GetStr(loan, "325"), JS.GetStr(loan, "4"), false) != 0, "Because of your balloon feature, this date may not be reached before the loan matures.");
      nameValueCollection.Add("2_a", str13);
      string str14 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37_a", str14);
      string str15 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69_a", str15);
      return nameValueCollection;
    }
  }
}
