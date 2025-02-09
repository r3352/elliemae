// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._VA_26_0286_LOAN_SUMMARY_P1CLASS
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
  public class _VA_26_0286_LOAN_SUMMARY_P1CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("1040", JS.GetStr(loan, "1040"));
      nameValueCollection.Add("VASUMMX32_VASUMMX33", JS.GetStr(loan, "VASUMM.X32") + " " + JS.GetStr(loan, "VASUMM.X33"));
      nameValueCollection.Add("VASUMMX34", JS.GetStr(loan, "VASUMM.X34"));
      nameValueCollection.Add("VASUMMX35_male", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X35"), "Male", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX35_female", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X35"), "Female", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X1"), "//", false) != 0, JS.GetStr(loan, "VASUMM.X1")));
      nameValueCollection.Add("VASUMMX2", JS.GetStr(loan, "VASUMM.X2"));
      nameValueCollection.Add("VASUMMX3", JS.GetStr(loan, "VASUMM.X3"));
      nameValueCollection.Add("954_army", Jed.BF(Operators.CompareString(JS.GetStr(loan, "954"), "Army", false) == 0, "X"));
      nameValueCollection.Add("954_navy", Jed.BF(Operators.CompareString(JS.GetStr(loan, "954"), "Navy", false) == 0, "X"));
      nameValueCollection.Add("954_airforce", Jed.BF(Operators.CompareString(JS.GetStr(loan, "954"), "AirForce", false) == 0, "X"));
      nameValueCollection.Add("954_marinecorps", Jed.BF(Operators.CompareString(JS.GetStr(loan, "954"), "Marine", false) == 0, "X"));
      nameValueCollection.Add("954_coastguard", Jed.BF(Operators.CompareString(JS.GetStr(loan, "954"), "CoastGuard", false) == 0, "X"));
      nameValueCollection.Add("954_other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "954"), "Other", false) == 0, "X"));
      nameValueCollection.Add("955_separated", Jed.BF(Operators.CompareString(JS.GetStr(loan, "955"), "SeparatedFromService", false) == 0, "X"));
      nameValueCollection.Add("955_inservice", Jed.BF(Operators.CompareString(JS.GetStr(loan, "955"), "InService", false) == 0, "X"));
      nameValueCollection.Add("934_yes", Jed.BF(Operators.CompareString(JS.GetStr(loan, "934"), "Y", false) == 0, "X"));
      nameValueCollection.Add("934_no", Jed.BF(Operators.CompareString(JS.GetStr(loan, "934"), "Y", false) != 0, "X"));
      nameValueCollection.Add("953_automatic", Jed.BF(Operators.CompareString(JS.GetStr(loan, "953"), "Automatic", false) == 0, "X"));
      nameValueCollection.Add("953_auto-IRRRL", Jed.BF(Operators.CompareString(JS.GetStr(loan, "953"), "Auto-IRRRL", false) == 0, "X"));
      nameValueCollection.Add("953_VA_prior_approval", Jed.BF(Operators.CompareString(JS.GetStr(loan, "953"), "VAPriorApproval", false) == 0, "X"));
      nameValueCollection.Add("956_home", Jed.BF(Operators.CompareString(JS.GetStr(loan, "956"), "Home", false) == 0, "X"));
      nameValueCollection.Add("956_manufactured", Jed.BF(Operators.CompareString(JS.GetStr(loan, "956"), "ManufacturedHome", false) == 0, "X"));
      nameValueCollection.Add("956_condo", Jed.BF(Operators.CompareString(JS.GetStr(loan, "956"), "Condominium", false) == 0, "X"));
      nameValueCollection.Add("956_alterations", Jed.BF(Operators.CompareString(JS.GetStr(loan, "956"), "Alterations/Improvements", false) == 0, "X"));
      nameValueCollection.Add("956_refinance", Jed.BF(Operators.CompareString(JS.GetStr(loan, "956"), "Refinance", false) == 0, "X"));
      nameValueCollection.Add("958_purchase", Jed.BF(Operators.CompareString(JS.GetStr(loan, "958"), "Purchase", false) == 0, "X"));
      nameValueCollection.Add("958_IRRRL", Jed.BF(Operators.CompareString(JS.GetStr(loan, "958"), "IRRRL", false) == 0, "X"));
      nameValueCollection.Add("958_cashoutrefi", Jed.BF(Operators.CompareString(JS.GetStr(loan, "958"), "CashOutRefi", false) == 0, "X"));
      nameValueCollection.Add("958_manufactured", Jed.BF(Operators.CompareString(JS.GetStr(loan, "958"), "ManufacturedHome", false) == 0, "X"));
      nameValueCollection.Add("958_REFI90%", Jed.BF(Operators.CompareString(JS.GetStr(loan, "958"), "RefinancingOver", false) == 0, "X"));
      nameValueCollection.Add("959_fixed", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "RegularFixed", false) == 0, "X"));
      nameValueCollection.Add("959_GPM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "GPM", false) == 0, "X"));
      nameValueCollection.Add("959_otherGPMS", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "OtherGPM", false) == 0, "X"));
      nameValueCollection.Add("959_GEM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "GEM", false) == 0, "X"));
      nameValueCollection.Add("959_Tempbuydown", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "TemporaryBuydown", false) == 0, "X"));
      nameValueCollection.Add("959_hybridarm", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "HybridARM", false) == 0, "X"));
      nameValueCollection.Add("959_arm", Jed.BF(Operators.CompareString(JS.GetStr(loan, "959"), "ARM", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX24_3/1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X24"), "3/1", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX24_5/1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X24"), "5/1", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX24_7/1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X24"), "7/1", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX24_10/1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X24"), "10/1", false) == 0, "X"));
      nameValueCollection.Add("957_sole", Jed.BF(Operators.CompareString(JS.GetStr(loan, "957"), "SoleOwnership", false) == 0, "X"));
      nameValueCollection.Add("957_joint2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "957"), "Joint - 2 Or More Veterans", false) == 0, "X"));
      nameValueCollection.Add("957_jointveteran", Jed.BF(Operators.CompareString(JS.GetStr(loan, "957"), "Joint - Veteran/Non-Veteran", false) == 0, "X"));
      nameValueCollection.Add("748", Jed.BF(Operators.CompareString(JS.GetStr(loan, "748"), "//", false) != 0, JS.GetStr(loan, "748")));
      nameValueCollection.Add("136", JS.GetStr(loan, "136"));
      nameValueCollection.Add("356", JS.GetStr(loan, "356"));
      nameValueCollection.Add("961", JS.GetStr(loan, "961"));
      nameValueCollection.Add("376_none", Jed.BF(Operators.CompareString(JS.GetStr(loan, "376"), "Y", false) == 0, "X"));
      nameValueCollection.Add("380_insulation", Jed.BF(Operators.CompareString(JS.GetStr(loan, "380"), "Y", false) == 0, "X"));
      nameValueCollection.Add("378_replacment", Jed.BF(Operators.CompareString(JS.GetStr(loan, "378"), "Y", false) == 0, "X"));
      nameValueCollection.Add("377_insulation_solar", Jed.BF(Operators.CompareString(JS.GetStr(loan, "377"), "Y", false) == 0, "X"));
      nameValueCollection.Add("379_addition", Jed.BF(Operators.CompareString(JS.GetStr(loan, "379"), "Y", false) == 0, "X"));
      nameValueCollection.Add("381_other", Jed.BF(Operators.CompareString(JS.GetStr(loan, "381"), "Y", false) == 0, "X"));
      nameValueCollection.Add("2", JS.GetStr(loan, "2"));
      nameValueCollection.Add("1041_neither", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) != 0 & Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) != 0, "X"));
      nameValueCollection.Add("1041_pud", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "PUD", false) == 0, "X"));
      nameValueCollection.Add("1041_condo", Jed.BF(Operators.CompareString(JS.GetStr(loan, "1041"), "Condominium", false) == 0, "X"));
      nameValueCollection.Add("962_IND", Jed.BF(Operators.CompareString(JS.GetStr(loan, "962"), "IND", false) == 0, "X"));
      nameValueCollection.Add("962_ONE", Jed.BF(Operators.CompareString(JS.GetStr(loan, "962"), "MCRV", false) == 0, "X"));
      nameValueCollection.Add("962_LAPP", Jed.BF(Operators.CompareString(JS.GetStr(loan, "962"), "LAPP", false) == 0, "X"));
      nameValueCollection.Add("962_MBL", Jed.BF(Operators.CompareString(JS.GetStr(loan, "962"), "MBL", false) == 0, "X"));
      nameValueCollection.Add("962_HUD", Jed.BF(Operators.CompareString(JS.GetStr(loan, "962"), "HUD", false) == 0, "X"));
      nameValueCollection.Add("962_PMC", Jed.BF(Operators.CompareString(JS.GetStr(loan, "962"), "PMC", false) == 0, "X"));
      nameValueCollection.Add("1523_1", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1523"), "Not Hispanic or Latino", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1531"), "Not Hispanic or Latino", false) == 0, "X"));
      nameValueCollection.Add("1523_2", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1523"), "Hispanic or Latino", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1531"), "Hispanic or Latino", false) == 0, "X"));
      nameValueCollection.Add("1524", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1524"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1532"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1525", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1525"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1533"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1526", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1526"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1534"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1527", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1527"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1535"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1528", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1528"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & Operators.CompareString(JS.GetStr(loan, "1536"), "Y", false) == 0, "X"));
      nameValueCollection.Add("1529_1530", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "Borrower", false) == 0 & (Operators.CompareString(JS.GetStr(loan, "1529"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1530"), "Y", false) == 0) | Operators.CompareString(JS.GetStr(loan, "VASUMM.X31"), "CoBorrower", false) == 0 & (Operators.CompareString(JS.GetStr(loan, "1537"), "Y", false) == 0 | Operators.CompareString(JS.GetStr(loan, "1538"), "Y", false) == 0), "X"));
      nameValueCollection.Add("VASUMMX50_FHAFIXED", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "FHA-Fixed", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_FHAARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "FHA-ARM/HARM", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_FIXED", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "Conventional-Fixed", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_ARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "Conventional-ARM/HARM", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_INTONLY", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "Conventional-Interest Only", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_VAFIXED", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "VA-Fixed", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_VAARM", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "VA-ARM/HARM", false) == 0, "X"));
      nameValueCollection.Add("VASUMMX50_OTHER", Jed.BF(Operators.CompareString(JS.GetStr(loan, "VASUMM.X50"), "Other", false) == 0, "X"));
      return nameValueCollection;
    }
  }
}
