// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._1084B_PARTNERSHIP_ETCCLASS
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
  public class _1084B_PARTNERSHIP_ETCCLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("FM1084X93", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X93"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X94", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X94"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X95", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X95"), "Y", false) == 0, "X"));
      nameValueCollection.Add("FM1084X96", JS.GetStr(loan, "FM1084.X96"));
      nameValueCollection.Add("FM1084X97", JS.GetStr(loan, "FM1084.X97"));
      nameValueCollection.Add("FM1084X98", JS.GetStr(loan, "FM1084.X98"));
      nameValueCollection.Add("FM1084X99", JS.GetStr(loan, "FM1084.X99"));
      nameValueCollection.Add("FM1084X100", JS.GetStr(loan, "FM1084.X100"));
      nameValueCollection.Add("FM1084X101", JS.GetStr(loan, "FM1084.X101"));
      nameValueCollection.Add("FM1084X102", JS.GetStr(loan, "FM1084.X102"));
      nameValueCollection.Add("FM1084X103", JS.GetStr(loan, "FM1084.X103"));
      nameValueCollection.Add("FM1084X104", JS.GetStr(loan, "FM1084.X104"));
      nameValueCollection.Add("FM1084X105", JS.GetStr(loan, "FM1084.X105"));
      nameValueCollection.Add("FM1084X106", JS.GetStr(loan, "FM1084.X106"));
      nameValueCollection.Add("FM1084X134", JS.GetStr(loan, "FM1084.X134"));
      nameValueCollection.Add("FM1084X135", JS.GetStr(loan, "FM1084.X135"));
      nameValueCollection.Add("FM1084X136", JS.GetStr(loan, "FM1084.X136"));
      nameValueCollection.Add("FM1084X137", JS.GetStr(loan, "FM1084.X137"));
      nameValueCollection.Add("FM1084X138", JS.GetStr(loan, "FM1084.X138"));
      nameValueCollection.Add("FM1084X139", JS.GetStr(loan, "FM1084.X139"));
      nameValueCollection.Add("FM1084X140", JS.GetStr(loan, "FM1084.X140"));
      nameValueCollection.Add("FM1084X141", JS.GetStr(loan, "FM1084.X141"));
      nameValueCollection.Add("FM1084X142", JS.GetStr(loan, "FM1084.X142"));
      nameValueCollection.Add("FM1084X143", JS.GetStr(loan, "FM1084.X143"));
      nameValueCollection.Add("FM1084X107", JS.GetStr(loan, "FM1084.X107"));
      nameValueCollection.Add("FM1084X108", JS.GetStr(loan, "FM1084.X108"));
      nameValueCollection.Add("FM1084X109", JS.GetStr(loan, "FM1084.X109"));
      nameValueCollection.Add("FM1084X110", JS.GetStr(loan, "FM1084.X110"));
      nameValueCollection.Add("FM1084X111", JS.GetStr(loan, "FM1084.X111"));
      nameValueCollection.Add("FM1084X112", JS.GetStr(loan, "FM1084.X112"));
      nameValueCollection.Add("FM1084X113", JS.GetStr(loan, "FM1084.X113"));
      nameValueCollection.Add("FM1084X114", JS.GetStr(loan, "FM1084.X114"));
      nameValueCollection.Add("FM1084X115", JS.GetStr(loan, "FM1084.X115"));
      nameValueCollection.Add("FM1084X144", JS.GetStr(loan, "FM1084.X144"));
      nameValueCollection.Add("FM1084X145", JS.GetStr(loan, "FM1084.X145"));
      nameValueCollection.Add("FM1084X146", JS.GetStr(loan, "FM1084.X146"));
      nameValueCollection.Add("FM1084X147", JS.GetStr(loan, "FM1084.X147"));
      nameValueCollection.Add("FM1084X148", JS.GetStr(loan, "FM1084.X148"));
      nameValueCollection.Add("FM1084X149", JS.GetStr(loan, "FM1084.X149"));
      nameValueCollection.Add("FM1084X150", JS.GetStr(loan, "FM1084.X150"));
      nameValueCollection.Add("FM1084X151", JS.GetStr(loan, "FM1084.X151"));
      nameValueCollection.Add("FM1084X116", JS.GetStr(loan, "FM1084.X116"));
      nameValueCollection.Add("FM1084X117", JS.GetStr(loan, "FM1084.X117"));
      nameValueCollection.Add("FM1084X118", JS.GetStr(loan, "FM1084.X118"));
      nameValueCollection.Add("FM1084X119", JS.GetStr(loan, "FM1084.X119"));
      nameValueCollection.Add("FM1084X120", JS.GetStr(loan, "FM1084.X120"));
      nameValueCollection.Add("FM1084X121", JS.GetStr(loan, "FM1084.X121"));
      nameValueCollection.Add("FM1084X122", JS.GetStr(loan, "FM1084.X122"));
      nameValueCollection.Add("FM1084X123", JS.GetStr(loan, "FM1084.X123"));
      nameValueCollection.Add("FM1084X124", JS.GetStr(loan, "FM1084.X124"));
      nameValueCollection.Add("FM1084X125", JS.GetStr(loan, "FM1084.X125"));
      nameValueCollection.Add("FM1084X126", JS.GetStr(loan, "FM1084.X126"));
      nameValueCollection.Add("FM1084X127", JS.GetStr(loan, "FM1084.X127"));
      nameValueCollection.Add("FM1084X128", JS.GetStr(loan, "FM1084.X128"));
      nameValueCollection.Add("FM1084X129", JS.GetStr(loan, "FM1084.X129"));
      nameValueCollection.Add("FM1084X130", JS.GetStr(loan, "FM1084.X130"));
      nameValueCollection.Add("FM1084X131", JS.GetStr(loan, "FM1084.X131"));
      nameValueCollection.Add("FM1084X132", JS.GetStr(loan, "FM1084.X132"));
      nameValueCollection.Add("FM1084X133", JS.GetStr(loan, "FM1084.X133"));
      nameValueCollection.Add("FM1084X152", JS.GetStr(loan, "FM1084.X152"));
      nameValueCollection.Add("FM1084X153", JS.GetStr(loan, "FM1084.X153"));
      nameValueCollection.Add("FM1084X154", JS.GetStr(loan, "FM1084.X154"));
      nameValueCollection.Add("FM1084X155", JS.GetStr(loan, "FM1084.X155"));
      nameValueCollection.Add("FM1084X156", JS.GetStr(loan, "FM1084.X156"));
      nameValueCollection.Add("FM1084X157", JS.GetStr(loan, "FM1084.X157"));
      nameValueCollection.Add("FM1084X158", JS.GetStr(loan, "FM1084.X158"));
      nameValueCollection.Add("FM1084X159", JS.GetStr(loan, "FM1084.X159"));
      nameValueCollection.Add("FM1084X160", JS.GetStr(loan, "FM1084.X160"));
      nameValueCollection.Add("FM1084X161", JS.GetStr(loan, "FM1084.X161"));
      nameValueCollection.Add("FM1084X162", JS.GetStr(loan, "FM1084.X162"));
      nameValueCollection.Add("FM1084X163", JS.GetStr(loan, "FM1084.X163"));
      nameValueCollection.Add("FM1084X164", JS.GetStr(loan, "FM1084.X164"));
      nameValueCollection.Add("FM1084X165", JS.GetStr(loan, "FM1084.X165"));
      nameValueCollection.Add("FM1084X166", JS.GetStr(loan, "FM1084.X166"));
      nameValueCollection.Add("FM1084X167", JS.GetStr(loan, "FM1084.X167"));
      nameValueCollection.Add("FM1084X168", JS.GetStr(loan, "FM1084.X168"));
      nameValueCollection.Add("FM1084X169", JS.GetStr(loan, "FM1084.X169"));
      nameValueCollection.Add("FM1084X170", JS.GetStr(loan, "FM1084.X170"));
      nameValueCollection.Add("FM1084X171", JS.GetStr(loan, "FM1084.X171"));
      nameValueCollection.Add("FM1084X172", JS.GetStr(loan, "FM1084.X172"));
      nameValueCollection.Add("FM1084X173", JS.GetStr(loan, "FM1084.X173"));
      nameValueCollection.Add("FM1084X174", JS.GetStr(loan, "FM1084.X174"));
      nameValueCollection.Add("FM1084X175", JS.GetStr(loan, "FM1084.X175"));
      nameValueCollection.Add("FM1084X176", JS.GetStr(loan, "FM1084.X176"));
      return nameValueCollection;
    }
  }
}
