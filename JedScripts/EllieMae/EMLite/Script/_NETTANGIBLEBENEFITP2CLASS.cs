// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._NETTANGIBLEBENEFITP2CLASS
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
  public class _NETTANGIBLEBENEFITP2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("NTB_X37", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X37"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X38", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X38"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X39", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X39"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X40", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X40"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X41", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X41"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X42", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X42"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X43", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X43"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X44", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X44"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X45", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X45"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X46", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X46"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X47", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X47"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X48", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X48"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X50", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X50"), "Y", false) == 0, "X"));
      nameValueCollection.Add("NTB_X23", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NTB.X23"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1264_A", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1264"), "", false) != 0, JS.GetStr(loan, "1264"), JS.GetStr(loan, "VEND.X293")));
      return nameValueCollection;
    }
  }
}
