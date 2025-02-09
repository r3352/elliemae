// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._HUD_56001_PROP_IMP_PG4CLASS
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
  public class _HUD_56001_PROP_IMP_PG4CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      string str1 = JS.GetStr(loan, "315");
      nameValueCollection.Add("315_A", str1);
      string str2 = JS.GetStr(loan, "319");
      nameValueCollection.Add("319_A", str2);
      string str3 = JS.GetStr(loan, "313") + Jed.BF(Operators.CompareString(JS.GetStr(loan, "321"), "", false) != 0, ", ") + JS.GetStr(loan, "321") + " " + JS.GetStr(loan, "323");
      nameValueCollection.Add("313_321_323_A", str3);
      string str4 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "FaceToFace", false) == 0, "X");
      nameValueCollection.Add("479_1FacetoFace", str4);
      string str5 = Jed.BF(Operators.CompareString(JS.GetStr(loan, "479"), "Telephone", false) == 0, "X");
      nameValueCollection.Add("479_3Telephone", str5);
      string str6 = JS.GetStr(loan, "65");
      nameValueCollection.Add("65_A", str6);
      string str7 = JS.GetStr(loan, "97");
      nameValueCollection.Add("97_A", str7);
      string str8 = JS.GetStr(loan, "1018");
      nameValueCollection.Add("1018", str8);
      string str9 = JS.GetStr(loan, "1144");
      nameValueCollection.Add("1144", str9);
      return nameValueCollection;
    }
  }
}
