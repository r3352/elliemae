// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._FAIRLENDINGCLASS
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
  public class _FAIRLENDINGCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "11");
      nameValueCollection.Add("11", str1);
      string str2 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("MAP2", str2);
      string str3 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str3);
      string str4 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str4);
      string str5 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("MAP1", str5);
      string str6 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str6);
      string str7 = JS.GetStr(loan, "363");
      nameValueCollection.Add("363", str7);
      string str8 = JS.GetStr(loan, "NOTICES.X20");
      nameValueCollection.Add("NOTICEX20", str8);
      string str9 = JS.GetStr(loan, "NOTICES.X21");
      nameValueCollection.Add("NOTICEX21", str9);
      string str10 = JS.GetStr(loan, "NOTICES.X22");
      nameValueCollection.Add("NOTICEX22", str10);
      string str11 = JS.GetStr(loan, "NOTICES.X23");
      nameValueCollection.Add("NOTICEX23", str11);
      string str12 = JS.GetStr(loan, "NOTICES.X24");
      nameValueCollection.Add("NOTICEX24", str12);
      string str13 = JS.GetStr(loan, "NOTICES.X25");
      nameValueCollection.Add("NOTICEX25", str13);
      string str14 = JS.GetStr(loan, "NOTICES.X26");
      nameValueCollection.Add("NOTICEX26", str14);
      string str15 = JS.GetStr(loan, "NOTICES.X27");
      nameValueCollection.Add("NOTICEX27", str15);
      string str16 = JS.GetStr(loan, "36") + "  " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str16);
      string str17 = JS.GetStr(loan, "68") + "  " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str17);
      return nameValueCollection;
    }
  }
}
