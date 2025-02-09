// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._IRS8821_TAX_AUTH_P1CLASS
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
  public class _IRS8821_TAX_AUTH_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = "  " + JS.GetStr(loan, "364");
      nameValueCollection.Add("364", str1);
      string str2 = JS.GetStr(loan, "IRS4506.X2") + " " + JS.GetStr(loan, "IRS4506.X3");
      nameValueCollection.Add("IRS4506X2_IRS4506X3", str2);
      string str3 = JS.GetStr(loan, "IRS4506.X6") + " " + JS.GetStr(loan, "IRS4506.X7");
      nameValueCollection.Add("IRS4506X6_IRS4506X7", str3);
      string str4 = JS.GetStr(loan, "IRS4506.X35");
      nameValueCollection.Add("IRS4506X35", str4);
      string str5 = JS.GetStr(loan, "IRS4506.X36") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "IRS4506.X37"), "", false) != 0, ", ") + JS.GetStr(loan, "IRS4506.X37") + " " + JS.GetStr(loan, "IRS4506.X38");
      nameValueCollection.Add("IRS4506X36_IRS4506X37_IRS4506X38", str5);
      string str6 = JS.GetStr(loan, "IRS4506.X4");
      nameValueCollection.Add("IRS4506X4", str6);
      string str7 = JS.GetStr(loan, "IRS4506.X5");
      nameValueCollection.Add("IRS4506X5", str7);
      string str8 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315", str8);
      string str9 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319", str9);
      string str10 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323", str10);
      string str11 = JS.GetStr(loan, "FE0117");
      nameValueCollection.Add("FE0117", str11);
      return nameValueCollection;
    }
  }
}
