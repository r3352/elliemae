// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ABILITYTOREPAY_P3CLASS
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
  public class _ABILITYTOREPAY_P3CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("QM_X163", JS.GetStr(loan, "QM.X163"));
      nameValueCollection.Add("QM_X164", JS.GetStr(loan, "QM.X164"));
      nameValueCollection.Add("QM_X165", JS.GetStr(loan, "QM.X165"));
      nameValueCollection.Add("QM_X166", JS.GetStr(loan, "QM.X166"));
      nameValueCollection.Add("QM_X167", JS.GetStr(loan, "QM.X167"));
      nameValueCollection.Add("QM_X168", JS.GetStr(loan, "QM.X168"));
      nameValueCollection.Add("QM_X169", JS.GetStr(loan, "QM.X169"));
      nameValueCollection.Add("QM_X170", JS.GetStr(loan, "QM.X170"));
      nameValueCollection.Add("QM_X171", JS.GetStr(loan, "QM.X171"));
      nameValueCollection.Add("QM_X172", JS.GetStr(loan, "QM.X172"));
      nameValueCollection.Add("QM_X173", JS.GetStr(loan, "QM.X173"));
      nameValueCollection.Add("QM_X174", JS.GetStr(loan, "QM.X174"));
      nameValueCollection.Add("QM_X175", JS.GetStr(loan, "QM.X175"));
      nameValueCollection.Add("QM_X176", JS.GetStr(loan, "QM.X176"));
      nameValueCollection.Add("QM_X177", JS.GetStr(loan, "QM.X177"));
      nameValueCollection.Add("QM_X178", JS.GetStr(loan, "QM.X178"));
      nameValueCollection.Add("QM_X179", JS.GetStr(loan, "QM.X179"));
      nameValueCollection.Add("QM_X180", JS.GetStr(loan, "QM.X180"));
      nameValueCollection.Add("QM_X181", JS.GetStr(loan, "QM.X181"));
      nameValueCollection.Add("QM_X182", JS.GetStr(loan, "QM.X182"));
      nameValueCollection.Add("QM_X183", JS.GetStr(loan, "QM.X183"));
      nameValueCollection.Add("QM_X184", JS.GetStr(loan, "QM.X184"));
      nameValueCollection.Add("QM_X185", JS.GetStr(loan, "QM.X185"));
      nameValueCollection.Add("QM_X186", JS.GetStr(loan, "QM.X186"));
      nameValueCollection.Add("QM_X187", JS.GetStr(loan, "QM.X187"));
      nameValueCollection.Add("QM_X188", JS.GetStr(loan, "QM.X188"));
      nameValueCollection.Add("QM_X189", JS.GetStr(loan, "QM.X189"));
      nameValueCollection.Add("QM_X190", JS.GetStr(loan, "QM.X190"));
      nameValueCollection.Add("QM_X155", JS.GetStr(loan, "QM.X155"));
      nameValueCollection.Add("QM_X156", JS.GetStr(loan, "QM.X156"));
      nameValueCollection.Add("QM_X157", JS.GetStr(loan, "QM.X157"));
      nameValueCollection.Add("QM_X158", JS.GetStr(loan, "QM.X158"));
      nameValueCollection.Add("QM_X159", JS.GetStr(loan, "QM.X159"));
      nameValueCollection.Add("QM_X160", JS.GetStr(loan, "QM.X160"));
      nameValueCollection.Add("QM_X161", JS.GetStr(loan, "QM.X161"));
      nameValueCollection.Add("QM_X162", JS.GetStr(loan, "QM.X162"));
      nameValueCollection.Add("QM_X153", JS.GetStr(loan, "QM.X153"));
      nameValueCollection.Add("QM_X154", JS.GetStr(loan, "QM.X154"));
      nameValueCollection.Add("QM_X120", JS.GetStr(loan, "QM.X120"));
      nameValueCollection.Add("QM_X121", JS.GetStr(loan, "QM.X121"));
      nameValueCollection.Add("QM_X122", JS.GetStr(loan, "QM.X122"));
      nameValueCollection.Add("S32DISC_X48", JS.GetStr(loan, "S32DISC.X48"));
      nameValueCollection.Add("QM_X123", JS.GetStr(loan, "QM.X123"));
      nameValueCollection.Add("S32DISC_X49_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "S32DISC.X49"), "does", false) == 0, "X"));
      nameValueCollection.Add("S32DISC_X49_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "S32DISC.X49"), "does not", false) == 0, "X"));
      nameValueCollection.Add("QM_X124_Y", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X124"), "does", false) == 0, "X"));
      nameValueCollection.Add("QM_X124_N", Jed.BF(Operators.CompareString(JS.GetStr(loan, "QM.X124"), "does not", false) == 0, "X"));
      nameValueCollection.Add("NEWHUD_X686", JS.GetStr(loan, "NEWHUD.X686"));
      nameValueCollection.Add("NEWHUD_X1067", Jed.BF(Operators.CompareString(JS.GetStr(loan, "NEWHUD.X1067"), "Y", false) == 0, "X"));
      nameValueCollection.Add("QM_X136", JS.GetStr(loan, "QM.X136"));
      nameValueCollection.Add("QM_X111", JS.GetStr(loan, "QM.X111"));
      nameValueCollection.Add("QM_X125", JS.GetStr(loan, "QM.X125"));
      nameValueCollection.Add("NEWHUD_X639", JS.GetStr(loan, "NEWHUD.X639"));
      nameValueCollection.Add("NEWHUD_X572", JS.GetStr(loan, "NEWHUD.X572"));
      nameValueCollection.Add("NEWHUD_X603", JS.GetStr(loan, "NEWHUD.X603"));
      nameValueCollection.Add("NEWHUD_X607", JS.GetStr(loan, "NEWHUD.X607"));
      nameValueCollection.Add("NEWHUD_X700", JS.GetStr(loan, "NEWHUD.X700"));
      nameValueCollection.Add("QM_X371", JS.GetStr(loan, "QM.X371"));
      nameValueCollection.Add("QM_X126", JS.GetStr(loan, "QM.X126"));
      nameValueCollection.Add("QM_X127", JS.GetStr(loan, "QM.X127"));
      nameValueCollection.Add("QM_X128", JS.GetStr(loan, "QM.X128"));
      nameValueCollection.Add("QM_X129", JS.GetStr(loan, "QM.X129"));
      nameValueCollection.Add("QM_X130", JS.GetStr(loan, "QM.X130"));
      nameValueCollection.Add("QM_X131", JS.GetStr(loan, "QM.X131"));
      nameValueCollection.Add("QM_X132", JS.GetStr(loan, "QM.X132"));
      nameValueCollection.Add("QM_X133", JS.GetStr(loan, "QM.X133"));
      nameValueCollection.Add("QM_X134", JS.GetStr(loan, "QM.X134"));
      return nameValueCollection;
    }
  }
}
