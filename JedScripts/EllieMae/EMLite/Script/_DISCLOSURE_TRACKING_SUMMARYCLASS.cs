// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._DISCLOSURE_TRACKING_SUMMARYCLASS
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
  public class _DISCLOSURE_TRACKING_SUMMARYCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str4);
      string str5 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11a", str5);
      string str6 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("12_14_15", str6);
      string str7 = JS.GetStr(loan, "317");
      nameValueCollection.Add("317", str7);
      string str8 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362", str8);
      string str9 = JS.GetStr(loan, "3121");
      nameValueCollection.Add("3121", str9);
      string str10 = JS.GetStr(loan, "1401");
      nameValueCollection.Add("1401", str10);
      string str11 = JS.GetStr(loan, "2");
      nameValueCollection.Add("2a", str11);
      string str12 = JS.GetStr(loan, "1206");
      nameValueCollection.Add("1206", str12);
      string str13 = JS.GetStr(loan, "3142");
      nameValueCollection.Add("3142", str13);
      string str14 = JS.GetStr(loan, "3143");
      nameValueCollection.Add("3143", str14);
      string str15 = JS.GetStr(loan, "3145");
      nameValueCollection.Add("3145", str15);
      string str16 = JS.GetStr(loan, "3147");
      nameValueCollection.Add("3147", str16);
      string str17 = JS.GetStr(loan, "763");
      nameValueCollection.Add("763", str17);
      string str18 = JS.GetStr(loan, "3152");
      nameValueCollection.Add("3152", str18);
      string str19 = JS.GetStr(loan, "3153");
      nameValueCollection.Add("3153", str19);
      string str20 = JS.GetStr(loan, "3154");
      nameValueCollection.Add("3154", str20);
      string str21 = JS.GetStr(loan, "3155");
      nameValueCollection.Add("3155", str21);
      string str22 = JS.GetStr(loan, "3148");
      nameValueCollection.Add("3148", str22);
      string str23 = JS.GetStr(loan, "3149");
      nameValueCollection.Add("3149", str23);
      string str24 = JS.GetStr(loan, "3150");
      nameValueCollection.Add("3150", str24);
      string str25 = JS.GetStr(loan, "3151");
      nameValueCollection.Add("3151", str25);
      return nameValueCollection;
    }
  }
}
