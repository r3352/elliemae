// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ADDENDUMHUDSETTLEMENTSTATEMENTCLASS
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
  public class _ADDENDUMHUDSETTLEMENTSTATEMENTCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "68"), "", false) != 0 | Operators.CompareString(JS.GetStr(loan, "69"), "", false) != 0, ", ") + JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("36_37_68_69", str1);
      string str2 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str2);
      string str3 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str3);
      string str4 = JS.GetStr(loan, "638");
      nameValueCollection.Add("638", str4);
      string str5 = "BORROWER";
      nameValueCollection.Add("BorrowerPaid", str5);
      string str6 = "SELLER";
      nameValueCollection.Add("SellerPaid", str6);
      string str7 = "PTC";
      nameValueCollection.Add("PTCPaid", str7);
      string str8 = "POC";
      nameValueCollection.Add("POCPaid", str8);
      return nameValueCollection;
    }
  }
}
