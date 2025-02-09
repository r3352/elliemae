// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script.__NY_PREAPPLICATION_LENDER_DISCLOSURE_P3CLASS
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
  public class __NY_PREAPPLICATION_LENDER_DISCLOSURE_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_1", str1);
      string str2 = JS.GetStr(loan, "DISCLOSURE.X144");
      nameValueCollection.Add("DISCLOSURE_X144", str2);
      string str3 = JS.GetStr(loan, "DISCLOSURE.X143");
      nameValueCollection.Add("DISCLOSURE_X143", str3);
      string str4 = JS.GetStr(loan, "DISCLOSURE.X142");
      nameValueCollection.Add("DISCLOSURE_X142", str4);
      string str5 = JS.GetStr(loan, "DISCLOSURE.X153");
      nameValueCollection.Add("DISCLOSURE_X153", str5);
      string str6 = JS.GetStr(loan, "DISCLOSURE.X152");
      nameValueCollection.Add("DISCLOSURE_X152", str6);
      string str7 = JS.GetStr(loan, "DISCLOSURE.X151");
      nameValueCollection.Add("DISCLOSURE_X151", str7);
      string str8 = JS.GetStr(loan, "DISCLOSURE.X150");
      nameValueCollection.Add("DISCLOSURE_X150", str8);
      string str9 = JS.GetStr(loan, "DISCLOSURE.X149");
      nameValueCollection.Add("DISCLOSURE_X149_2", str9);
      string str10 = JS.GetStr(loan, "DISCLOSURE.X138");
      nameValueCollection.Add("DISCLOSURE_X138", str10);
      string str11 = JS.GetStr(loan, "DISCLOSURE.X137");
      nameValueCollection.Add("DISCLOSURE_X137", str11);
      string str12 = JS.GetStr(loan, "DISCLOSURE.X149");
      nameValueCollection.Add("DISCLOSURE_X149", str12);
      string str13 = JS.GetStr(loan, "DISCLOSURE.X148");
      nameValueCollection.Add("DISCLOSURE_X148", str13);
      string str14 = JS.GetStr(loan, "DISCLOSURE.X139") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X140"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X140") + " " + JS.GetStr(loan, "DISCLOSURE.X141");
      nameValueCollection.Add("MAP2", str14);
      string str15 = JS.GetStr(loan, "DISCLOSURE.X145") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X146"), "", false) != 0, ", ") + JS.GetStr(loan, "DISCLOSURE.X146") + " " + JS.GetStr(loan, "DISCLOSURE.X147");
      nameValueCollection.Add("MAP3", str15);
      string str16 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X180"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X180", str16);
      string str17 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "DISCLOSURE.X181"), "Y", false) == 0, "X", "");
      nameValueCollection.Add("DISCLOSURE_X181", str17);
      return nameValueCollection;
    }
  }
}
