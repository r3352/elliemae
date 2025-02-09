// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VOECLASS
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
  public class _VOECLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "364");
      nameValueCollection.Add("FE0131", str1);
      string str2 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str2);
      string str3 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str3);
      string str4 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "324"), "", false) != 0, "Phone  ") + JS.GetStr(loan, "324");
      nameValueCollection.Add("324", str5);
      string str6 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "326"), "", false) != 0, "Fax  ") + JS.GetStr(loan, "326");
      nameValueCollection.Add("326", str6);
      string str7 = JS.GetStr(loan, "362");
      nameValueCollection.Add("362", str7);
      string str8 = "";
      nameValueCollection.Add("363", str8);
      string str9 = JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str9);
      return nameValueCollection;
    }
  }
}
