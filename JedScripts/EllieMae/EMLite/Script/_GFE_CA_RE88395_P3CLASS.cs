// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFE_CA_RE88395_P3CLASS
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
  public class _GFE_CA_RE88395_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "RE88395.X137");
      nameValueCollection.Add("RE88395X137", str1);
      string str2 = JS.GetStr(loan, "RE88395.X140");
      nameValueCollection.Add("RE88395X140", str2);
      string str3 = JS.GetStr(loan, "RE88395.X143");
      nameValueCollection.Add("RE88395X143", str3);
      string str4 = JS.GetStr(loan, "RE88395.X138");
      nameValueCollection.Add("RE88395X138", str4);
      string str5 = JS.GetStr(loan, "RE88395.X141");
      nameValueCollection.Add("RE88395X141", str5);
      string str6 = JS.GetStr(loan, "RE88395.X144");
      nameValueCollection.Add("RE88395X144", str6);
      string str7 = JS.GetStr(loan, "RE88395.X139");
      nameValueCollection.Add("RE88395X139", str7);
      string str8 = JS.GetStr(loan, "RE88395.X142");
      nameValueCollection.Add("RE88395X142", str8);
      string str9 = JS.GetStr(loan, "RE88395.X145");
      nameValueCollection.Add("RE88395X145", str9);
      string str10 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "May", false) == 0, "X");
      nameValueCollection.Add("RE88395X149_May", str10);
      string str11 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "Will", false) == 0, "X");
      nameValueCollection.Add("RE88395X149_Will", str11);
      string str12 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X149"), "WillNot", false) == 0, "X");
      nameValueCollection.Add("RE88395X149_Willnot", str12);
      string str13 = JS.GetStr(loan, "RE88395.X150") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X182"), "", false) != 0, " / ") + JS.GetStr(loan, "RE88395.X182");
      nameValueCollection.Add("RE88395X150", str13);
      string str14 = JS.GetStr(loan, "RE88395.X151") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X183"), "", false) != 0, " / ") + JS.GetStr(loan, "RE88395.X183");
      nameValueCollection.Add("RE88395X151", str14);
      string str15 = JS.GetStr(loan, "319") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "319"), "", false) != 0, ", ") + JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("319_313_a", str15);
      string str16 = JS.GetStr(loan, "RE88395.X182");
      nameValueCollection.Add("RE88395X182", str16);
      string str17 = JS.GetStr(loan, "RE88395.X183");
      nameValueCollection.Add("RE88395X183", str17);
      string str18 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str18);
      string str19 = JS.GetStr(loan, "68") + " " + JS.GetStr(loan, "69");
      nameValueCollection.Add("68_69", str19);
      return nameValueCollection;
    }
  }
}
