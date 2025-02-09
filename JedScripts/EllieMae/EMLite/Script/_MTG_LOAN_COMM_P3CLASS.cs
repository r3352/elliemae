// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._MTG_LOAN_COMM_P3CLASS
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
  public class _MTG_LOAN_COMM_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "3094");
      nameValueCollection.Add("3094", str1);
      string str2 = JS.GetStr(loan, "364");
      nameValueCollection.Add("901", str2);
      string str3 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("100_101", str3);
      string str4 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("150_151", str4);
      string str5 = JS.GetStr(loan, "363");
      nameValueCollection.Add("7354", str5);
      string str6 = JS.GetStr(loan, "11");
      nameValueCollection.Add("31", str6);
      string str7 = JS.GetStr(loan, "12") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "14"), "", false) != 0, ", ") + JS.GetStr(loan, "14") + " " + JS.GetStr(loan, "15");
      nameValueCollection.Add("32_33_34", str7);
      return nameValueCollection;
    }
  }
}
