// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFE_CA___RE882_P3CLASS
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
  public class _GFE_CA___RE882_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "may", false) == 0, "X");
      nameValueCollection.Add("RE88395X149_MAY", str1);
      string str2 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "will", false) == 0, "X");
      nameValueCollection.Add("RE88395X149_WILL", str2);
      string str3 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "will not", false) == 0, "X");
      nameValueCollection.Add("RE88395X149_WILLNOT", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "NoDocumentation", false) == 0 | Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "LimitedDocumentation", false) == 0, "X");
      nameValueCollection.Add("MORNETX67_Y", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "NoDocumentation", false) != 0 & Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "LimitedDocumentation", false) != 0, "X");
      nameValueCollection.Add("MORNETX67_N", str5);
      string str6 = JS.GetStr(loan, "RE88395.X150");
      nameValueCollection.Add("RE88395X150", str6);
      string str7 = JS.GetStr(loan, "RE88395.X182");
      nameValueCollection.Add("RE88395X182", str7);
      string str8 = JS.GetStr(loan, "3237");
      nameValueCollection.Add("3237", str8);
      string str9 = JS.GetStr(loan, "1612");
      nameValueCollection.Add("1612", str9);
      string str10 = JS.GetStr(loan, "2306");
      nameValueCollection.Add("2306", str10);
      string str11 = JS.GetStr(loan, "3238");
      nameValueCollection.Add("3238", str11);
      string str12 = JS.GetStr(loan, "RE88395.X338");
      nameValueCollection.Add("RE88395X338", str12);
      string str13 = JS.GetStr(loan, "RE88395.X339") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X340"), "", false) != 0, ", ") + JS.GetStr(loan, "RE88395.X340") + " " + JS.GetStr(loan, "RE88395.X341");
      nameValueCollection.Add("RE88395X339_X340_X341", str13);
      return nameValueCollection;
    }
  }
}
