// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._RD_1980_19CLASS
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
  public class _RD_1980_19CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "USDA.X120");
      nameValueCollection.Add("USDA_X120", str1);
      string str2 = JS.GetStr(loan, "USDA.X121");
      nameValueCollection.Add("USDA_X121", str2);
      string str3 = JS.GetStr(loan, "USDA.X122");
      nameValueCollection.Add("USDA_X122", str3);
      string str4 = JS.GetStr(loan, "36") + " " + JS.GetStr(loan, "37");
      nameValueCollection.Add("36_37", str4);
      string str5 = JS.GetStr(loan, "FR0104");
      nameValueCollection.Add("FR0104", str5);
      string str6 = JS.GetStr(loan, "FR0106") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "FR0107"), "", false) != 0, ", ") + JS.GetStr(loan, "FR0107") + " " + JS.GetStr(loan, "FR0108");
      nameValueCollection.Add("FR0106_FR0107_FR0108", str6);
      string str7 = JS.GetStr(loan, "USDA.X123");
      nameValueCollection.Add("USDA_X123", str7);
      string str8 = JS.GetStr(loan, "USDA.X186");
      nameValueCollection.Add("USDA_X186", str8);
      string str9 = JS.GetStr(loan, "USDA.X124");
      nameValueCollection.Add("USDA_X124", str9);
      string str10 = JS.GetStr(loan, "USDA.X125");
      nameValueCollection.Add("USDA_X125", str10);
      string str11 = JS.GetStr(loan, "USDA.X126");
      nameValueCollection.Add("USDA_X126", str11);
      string str12 = JS.GetStr(loan, "USDA.X127");
      nameValueCollection.Add("USDA_X127", str12);
      string str13 = JS.GetStr(loan, "1264");
      nameValueCollection.Add("1264", str13);
      string str14 = JS.GetStr(loan, "1257");
      nameValueCollection.Add("1257", str14);
      string str15 = JS.GetStr(loan, "1258") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "1259"), "", false) != 0, ", ") + JS.GetStr(loan, "1259") + " " + JS.GetStr(loan, "1260");
      nameValueCollection.Add("1258_1259_1260", str15);
      string str16 = JS.GetStr(loan, "USDA.X128");
      nameValueCollection.Add("USDA_X128", str16);
      string str17 = JS.GetStr(loan, "USDA.X166");
      nameValueCollection.Add("USDA_X166", str17);
      string str18 = JS.GetStr(loan, "USDA.X129");
      nameValueCollection.Add("USDA_X129", str18);
      string str19 = JS.GetStr(loan, "USDA.X130");
      nameValueCollection.Add("USDA_X130", str19);
      string str20 = JS.GetStr(loan, "USDA.X131");
      nameValueCollection.Add("USDA_X131", str20);
      string str21 = JS.GetStr(loan, "1826");
      nameValueCollection.Add("1826", str21);
      string str22 = JS.GetStr(loan, "USDA.X133");
      nameValueCollection.Add("USDA_X133", str22);
      string str23 = JS.GetStr(loan, "USDA.X134");
      nameValueCollection.Add("USDA_X134", str23);
      string str24 = JS.GetStr(loan, "748");
      nameValueCollection.Add("748", str24);
      string str25 = JS.GetStr(loan, "78");
      nameValueCollection.Add("78", str25);
      string str26 = JS.GetStr(loan, "USDA.X135");
      nameValueCollection.Add("USDA_X135", str26);
      string str27 = JS.GetStr(loan, "USDA.X199");
      nameValueCollection.Add("USDA_X199", str27);
      string str28 = JS.GetStr(loan, "USDA.X137");
      nameValueCollection.Add("USDA_X137", str28);
      string str29 = JS.GetStr(loan, "USDA.X138");
      nameValueCollection.Add("USDA_X138", str29);
      string str30 = JS.GetStr(loan, "USDA.X139");
      nameValueCollection.Add("USDA_X139", str30);
      string str31 = JS.GetStr(loan, "USDA.X140");
      nameValueCollection.Add("USDA_X140", str31);
      string str32 = JS.GetStr(loan, "USDA.X141");
      nameValueCollection.Add("USDA_X141", str32);
      string str33 = JS.GetStr(loan, "USDA.X142");
      nameValueCollection.Add("USDA_X142", str33);
      string str34 = JS.GetStr(loan, "SYS.X2");
      nameValueCollection.Add("SYS_X2", str34);
      string str35 = JS.GetStr(loan, "USDA.X143");
      nameValueCollection.Add("USDA_X143", str35);
      string str36 = JS.GetStr(loan, "USDA.X144");
      nameValueCollection.Add("USDA_X144", str36);
      string str37 = JS.GetStr(loan, "USDA.X145");
      nameValueCollection.Add("USDA_X145", str37);
      string str38 = JS.GetStr(loan, "USDA.X146");
      nameValueCollection.Add("USDA_X146", str38);
      string str39 = JS.GetStr(loan, "USDA.X147");
      nameValueCollection.Add("USDA_X147", str39);
      string str40 = JS.GetStr(loan, "USDA.X148");
      nameValueCollection.Add("USDA_X148", str40);
      string str41 = JS.GetStr(loan, "USDA.X149");
      nameValueCollection.Add("USDA_X149", str41);
      return nameValueCollection;
    }
  }
}
