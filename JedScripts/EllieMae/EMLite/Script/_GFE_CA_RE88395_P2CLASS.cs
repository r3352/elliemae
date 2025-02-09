// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._GFE_CA_RE88395_P2CLASS
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
  public class _GFE_CA_RE88395_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("RE88395X108", JS.GetStr(loan, "RE88395.X108"));
      nameValueCollection.Add("RE88395X111", JS.GetStr(loan, "RE88395.X111"));
      nameValueCollection.Add("RE88395X112", JS.GetStr(loan, "RE88395.X112"));
      nameValueCollection.Add("RE88395X113", JS.GetStr(loan, "RE88395.X113"));
      nameValueCollection.Add("RE88395X114", JS.GetStr(loan, "RE88395.X114"));
      nameValueCollection.Add("RE88395X115", JS.GetStr(loan, "RE88395.X115"));
      nameValueCollection.Add("RE88395X116", JS.GetStr(loan, "RE88395.X116"));
      nameValueCollection.Add("RE88395X326_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Fixed Rate Loan", false) == 0, "X"));
      nameValueCollection.Add("RE88395X326_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, "X"));
      nameValueCollection.Add("RE88395X326_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, "X"));
      nameValueCollection.Add("RE88395X326_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, "X"));
      nameValueCollection.Add("4", JS.GetStr(loan, "4"));
      nameValueCollection.Add("4_Months", "X");
      nameValueCollection.Add("3_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Fixed Rate Loan", false) == 0, JS.GetStr(loan, "3"), ""));
      nameValueCollection.Add("5_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Fixed Rate Loan", false) == 0, JS.GetStr(loan, "5"), ""));
      nameValueCollection.Add("3_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "3"), ""));
      nameValueCollection.Add("696_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "696"), ""));
      nameValueCollection.Add("696_b1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "694"), ""));
      nameValueCollection.Add("5_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "5"), ""));
      nameValueCollection.Add("1827_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "1827"), ""));
      nameValueCollection.Add("2625_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "2625"), ""));
      nameValueCollection.Add("697_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "697"), ""));
      nameValueCollection.Add("RE88395X323_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X323"), ""));
      nameValueCollection.Add("RE88395X324_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X324"), ""));
      nameValueCollection.Add("RE88395X325_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X325"), ""));
      nameValueCollection.Add("RE88395X327_b", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Fixed Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X327"), ""));
      nameValueCollection.Add("3_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "3"), ""));
      nameValueCollection.Add("1827_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "1827"), ""));
      nameValueCollection.Add("5_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "5"), ""));
      nameValueCollection.Add("2625_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "2625"), ""));
      nameValueCollection.Add("697_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "697"), ""));
      nameValueCollection.Add("696_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "696"), ""));
      nameValueCollection.Add("RE88395X325_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X325"), ""));
      nameValueCollection.Add("RE88395X327_c", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X327"), ""));
      nameValueCollection.Add("3_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "3"), ""));
      nameValueCollection.Add("696_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "696"), ""));
      nameValueCollection.Add("696_e", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "696"), ""));
      nameValueCollection.Add("5_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "5"), ""));
      nameValueCollection.Add("1827_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "1827"), ""));
      nameValueCollection.Add("2625_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "2625"), ""));
      nameValueCollection.Add("697_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "697"), ""));
      nameValueCollection.Add("RE88395X324_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X324"), ""));
      nameValueCollection.Add("RE88395X325_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X325"), ""));
      nameValueCollection.Add("RE88395X327_d", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X326"), "Initial Adjustable Rate Loan", false) == 0, JS.GetStr(loan, "RE88395.X327"), ""));
      nameValueCollection.Add("MORNETX67_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "FullDocumentation", false) == 0, "X"));
      nameValueCollection.Add("MORNETX67_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "MORNET.X67"), "FullDocumentation", false) != 0, "X"));
      nameValueCollection.Add("RE88395X120_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "N", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1659"), "", false) == 0, "X"));
      nameValueCollection.Add("RE88395X120_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X121", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, JS.GetStr(loan, "RE88395.X121")));
      nameValueCollection.Add("RE88395X122", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X122"), "//", false) != 0 & Operators.CompareString(JS.GetStr(loan, "1659"), "Y", false) == 0, JS.GetStr(loan, "RE88395.X122")));
      nameValueCollection.Add("RE88395X322", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X322"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X123", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X123"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X124", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X124"), "Y", false) == 0, "X"));
      nameValueCollection.Add("RE88395X315", JS.GetStr(loan, "RE88395.X315"));
      nameValueCollection.Add("RE88395X126_Original", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X126"), "Original Balance", false) == 0, "X"));
      nameValueCollection.Add("RE88395X126_Unpaid", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X126"), "Unpaid Balance", false) == 0, "X"));
      nameValueCollection.Add("RE88395X127", JS.GetStr(loan, "RE88395.X127"));
      nameValueCollection.Add("HUD24", JS.GetStr(loan, "HUD24"));
      nameValueCollection.Add("HUD24_a", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) > 0.0, "X"));
      nameValueCollection.Add("HUD24_b", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0, "X"));
      nameValueCollection.Add("RE88395X319", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X319")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X320", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X320")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X333", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X333")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X334", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X334")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X335", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0 & Jed.S2N(JS.GetStr(loan, "RE88395.X335")) > 0.0, "X"));
      nameValueCollection.Add("RE88395X336", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0, JS.GetStr(loan, "RE88395.X336")));
      nameValueCollection.Add("RE88395X321", Jed.BF(Jed.S2N(JS.GetStr(loan, "HUD24")) <= 0.0, JS.GetStr(loan, "RE88395.X321")));
      nameValueCollection.Add("RE88395X155_No", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X128"), "", false) != 0, "", "X"));
      nameValueCollection.Add("RE88395X155_Yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "RE88395.X128"), "", false) != 0, "X"));
      nameValueCollection.Add("RE88395X128", JS.GetStr(loan, "RE88395.X128"));
      nameValueCollection.Add("RE88395X131", JS.GetStr(loan, "RE88395.X131"));
      nameValueCollection.Add("RE88395X134", JS.GetStr(loan, "RE88395.X134"));
      nameValueCollection.Add("RE88395X129", JS.GetStr(loan, "RE88395.X129"));
      nameValueCollection.Add("RE88395X132", JS.GetStr(loan, "RE88395.X132"));
      nameValueCollection.Add("RE88395X135", JS.GetStr(loan, "RE88395.X135"));
      nameValueCollection.Add("RE88395X130", JS.GetStr(loan, "RE88395.X130"));
      nameValueCollection.Add("RE88395X133", JS.GetStr(loan, "RE88395.X133"));
      nameValueCollection.Add("RE88395X136", JS.GetStr(loan, "RE88395.X136"));
      return nameValueCollection;
    }
  }
}
