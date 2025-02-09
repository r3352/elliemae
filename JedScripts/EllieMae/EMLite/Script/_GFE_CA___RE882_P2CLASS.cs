// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFE_CA___RE882_P2CLASS
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
  public class _GFE_CA___RE882_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("RE88395X108", JS.GetStr(loan, "RE88395.X108"));
      nameValueCollection.Add("RE88395X193", JS.GetStr(loan, "RE88395.X193"));
      nameValueCollection.Add("RE88395X112", JS.GetStr(loan, "RE88395.X112"));
      nameValueCollection.Add("RE88395X113", JS.GetStr(loan, "RE88395.X113"));
      nameValueCollection.Add("RE88395X114", JS.GetStr(loan, "RE88395.X114"));
      nameValueCollection.Add("RE88395X115", JS.GetStr(loan, "RE88395.X115"));
      nameValueCollection.Add("RE882X64", JS.GetStr(loan, "RE882.X64"));
      nameValueCollection.Add("RE882X65", JS.GetStr(loan, "RE882.X65"));
      nameValueCollection.Add("RE88395X116", JS.GetStr(loan, "RE88395.X116"));
      nameValueCollection.Add("RE88395X117_ToYou", Jed.BF(Jed.S2N(JS.GetStr(loan, "RE88395.X118")) <= 0.0, "X"));
      nameValueCollection.Add("RE88395X117_ThatYouPay", Jed.BF(Jed.S2N(JS.GetStr(loan, "RE88395.X118")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X118", Jed.BF(Jed.S2N(JS.GetStr(loan, "RE88395.X118")) <= 0.0, Jed.NF(Jed.S2N(JS.GetStr(loan, "RE88395.X118")) * -1.0, (byte) 18, 0), JS.GetStr(loan, "RE88395.X118")));
      nameValueCollection.Add("3", JS.GetStr(loan, "3"));
      nameValueCollection.Add("608_Fixed", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "Fixed", false) == 0, "X"));
      nameValueCollection.Add("608_ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "608"), "AdjustableRate", false) == 0, "X"));
      nameValueCollection.Add("5", JS.GetStr(loan, "5"));
      nameValueCollection.Add("325", Jed.BF(Jed.S2N(JS.GetStr(loan, "325")) > 0.0, JS.GetStr(loan, "325"), JS.GetStr(loan, "4")));
      nameValueCollection.Add("1347", JS.GetStr(loan, "1347"));
      nameValueCollection.Add("1392", JS.GetStr(loan, "1392"));
      nameValueCollection.Add("1659_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1659_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) != 0, "X"));
      nameValueCollection.Add("RE88395X122", JS.GetStr(loan, "RE88395.X122"));
      nameValueCollection.Add("RE88395X121", JS.GetStr(loan, "RE88395.X121"));
      nameValueCollection.Add("RE88395X123_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X322"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "RE88395.X191"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X123_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X123"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X315", JS.GetStr(loan, "RE88395.X315"));
      nameValueCollection.Add("RE88395X316", JS.GetStr(loan, "RE88395.X316"));
      nameValueCollection.Add("RE88395X191_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X191"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X191_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X191"), "Y", false) != 0, "X"));
      nameValueCollection.Add("HUD24_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0, "X"));
      nameValueCollection.Add("HUD24_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0, "X"));
      nameValueCollection.Add("HUD24", JS.GetStr(loan, "HUD24"));
      nameValueCollection.Add("231_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & Jed.S2N(JS.GetStr(loan, "231")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0141"), "", false) != 0, "X"));
      nameValueCollection.Add("231_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & (Jed.S2N(JS.GetStr(loan, "231")) == 0.0 | Operators.CompareString(JS.GetStr(loan, "HUD0141"), "", false) == 0), "X"));
      nameValueCollection.Add("232_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & Jed.S2N(JS.GetStr(loan, "232")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0143"), "", false) != 0, "X"));
      nameValueCollection.Add("232_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & (Jed.S2N(JS.GetStr(loan, "232")) == 0.0 | Operators.CompareString(JS.GetStr(loan, "HUD0143"), "", false) == 0), "X"));
      nameValueCollection.Add("230_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & Jed.S2N(JS.GetStr(loan, "230")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0142"), "", false) != 0, "X"));
      nameValueCollection.Add("230_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & (Jed.S2N(JS.GetStr(loan, "230")) == 0.0 | Operators.CompareString(JS.GetStr(loan, "HUD0142"), "", false) == 0), "X"));
      nameValueCollection.Add("235_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & Jed.S2N(JS.GetStr(loan, "235")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0144"), "", false) != 0, "X"));
      nameValueCollection.Add("235_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & (Jed.S2N(JS.GetStr(loan, "235")) == 0.0 | Operators.CompareString(JS.GetStr(loan, "HUD0144"), "", false) == 0), "X"));
      nameValueCollection.Add("234_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & (Jed.S2N(JS.GetStr(loan, "L268")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0145"), "", false) != 0 | Jed.S2N(JS.GetStr(loan, "1630")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0146"), "", false) != 0 | Jed.S2N(JS.GetStr(loan, "253")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0147"), "", false) != 0 | Jed.S2N(JS.GetStr(loan, "254")) > 0.0 & Operators.CompareString(JS.GetStr(loan, "HUD0148"), "", false) != 0), "X"));
      nameValueCollection.Add("234_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0 & (Jed.S2N(JS.GetStr(loan, "L268")) == 0.0 & Jed.S2N(JS.GetStr(loan, "1630")) == 0.0 & Jed.S2N(JS.GetStr(loan, "253")) == 0.0 & Jed.S2N(JS.GetStr(loan, "254")) == 0.0 | Operators.CompareString(JS.GetStr(loan, "HUD0145"), "", false) == 0 & Operators.CompareString(JS.GetStr(loan, "HUD0146"), "", false) == 0 & Operators.CompareString(JS.GetStr(loan, "HUD0147"), "", false) == 0 & Operators.CompareString(JS.GetStr(loan, "HUD0148"), "", false) == 0), "X"));
      nameValueCollection.Add("RE88395X336", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0, JS.GetStr(loan, "RE88395.X336")));
      nameValueCollection.Add("RE88395X319_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X319")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X320_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X320")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X333_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X333")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X334_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X334")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X335_Y", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X335")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X319_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X319")) == 0.0, "X"));
      nameValueCollection.Add("RE88395X320_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X320")) == 0.0, "X"));
      nameValueCollection.Add("RE88395X333_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X333")) == 0.0, "X"));
      nameValueCollection.Add("RE88395X334_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X334")) == 0.0, "X"));
      nameValueCollection.Add("RE88395X335_N", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X335")) == 0.0, "X"));
      nameValueCollection.Add("RE88395X336a", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0, JS.GetStr(loan, "RE88395.X336")));
      nameValueCollection.Add("RE88395X128", JS.GetStr(loan, "RE88395.X128"));
      nameValueCollection.Add("RE88395X131", JS.GetStr(loan, "RE88395.X131"));
      nameValueCollection.Add("RE88395X134", JS.GetStr(loan, "RE88395.X134"));
      nameValueCollection.Add("RE88395X129", JS.GetStr(loan, "RE88395.X129"));
      nameValueCollection.Add("RE88395X132", JS.GetStr(loan, "RE88395.X132"));
      nameValueCollection.Add("RE88395X135", JS.GetStr(loan, "RE88395.X135"));
      nameValueCollection.Add("RE88395X130", JS.GetStr(loan, "RE88395.X130"));
      nameValueCollection.Add("RE88395X133", JS.GetStr(loan, "RE88395.X133"));
      nameValueCollection.Add("RE88395X136", JS.GetStr(loan, "RE88395.X136"));
      nameValueCollection.Add("RE88395X137", JS.GetStr(loan, "RE88395.X137"));
      nameValueCollection.Add("RE88395X140", JS.GetStr(loan, "RE88395.X140"));
      nameValueCollection.Add("RE88395X143", JS.GetStr(loan, "RE88395.X143"));
      nameValueCollection.Add("RE88395X138", JS.GetStr(loan, "RE88395.X138"));
      nameValueCollection.Add("RE88395X141", JS.GetStr(loan, "RE88395.X141"));
      nameValueCollection.Add("RE88395X144", JS.GetStr(loan, "RE88395.X144"));
      nameValueCollection.Add("RE88395X139", JS.GetStr(loan, "RE88395.X139"));
      nameValueCollection.Add("RE88395X142", JS.GetStr(loan, "RE88395.X142"));
      nameValueCollection.Add("RE88395X145", JS.GetStr(loan, "RE88395.X145"));
      return nameValueCollection;
    }
  }
}
