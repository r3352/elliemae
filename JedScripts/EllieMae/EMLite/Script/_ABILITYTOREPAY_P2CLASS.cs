// Decompiled with JetBrains decompiler
// Type: JedScripts.EllieMae.EMLite.Script._ABILITYTOREPAY_P2CLASS
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
  public class _ABILITYTOREPAY_P2CLASS
  {
    public NameValueCollection RunScript(LoanData loan)
    {
      NameValueCollection nameValueCollection = new NameValueCollection();
      string empty = string.Empty;
      nameValueCollection.Add("QM_X113", JS.GetStr(loan, "QM.X113"));
      nameValueCollection.Add("QM_X114", JS.GetStr(loan, "QM.X114"));
      nameValueCollection.Add("QM_X117", JS.GetStr(loan, "QM.X117"));
      nameValueCollection.Add("AUSF_X3", JS.GetStr(loan, "AUSF.X3"));
      nameValueCollection.Add("QM_X137", JS.GetStr(loan, "QM.X137"));
      nameValueCollection.Add("QM_X145", JS.GetStr(loan, "QM.X145"));
      nameValueCollection.Add("QM_X352", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X352")), (byte) 0, 0));
      nameValueCollection.Add("QM_X204", JS.GetStr(loan, "QM.X204"));
      nameValueCollection.Add("QM_X205", JS.GetStr(loan, "QM.X205"));
      nameValueCollection.Add("QM_X206", JS.GetStr(loan, "QM.X206"));
      nameValueCollection.Add("QM_X207", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X207")), (byte) 0, 0));
      nameValueCollection.Add("QM_X208", JS.GetStr(loan, "QM.X208"));
      nameValueCollection.Add("QM_X209", JS.GetStr(loan, "QM.X209"));
      nameValueCollection.Add("QM_X210", JS.GetStr(loan, "QM.X210"));
      nameValueCollection.Add("QM_X211", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X211")), (byte) 0, 0));
      nameValueCollection.Add("QM_X212", JS.GetStr(loan, "QM.X212"));
      nameValueCollection.Add("QM_X213", JS.GetStr(loan, "QM.X213"));
      nameValueCollection.Add("QM_X214", JS.GetStr(loan, "QM.X214"));
      nameValueCollection.Add("QM_X215", JS.GetStr(loan, "QM.X215"));
      nameValueCollection.Add("QM_X216", JS.GetStr(loan, "QM.X216"));
      nameValueCollection.Add("QM_X353", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X353")), (byte) 0, 0));
      nameValueCollection.Add("QM_X314", JS.GetStr(loan, "QM.X314"));
      nameValueCollection.Add("QM_X315", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X315")), (byte) 0, 0));
      nameValueCollection.Add("QM_X316", JS.GetStr(loan, "QM.X316"));
      nameValueCollection.Add("QM_X317", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X317")), (byte) 0, 0));
      nameValueCollection.Add("QM_X318", JS.GetStr(loan, "QM.X318"));
      nameValueCollection.Add("QM_X354", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X354")), (byte) 0, 0));
      nameValueCollection.Add("QM_X217", JS.GetStr(loan, "QM.X217"));
      nameValueCollection.Add("QM_X218", JS.GetStr(loan, "QM.X218"));
      nameValueCollection.Add("QM_X219", JS.GetStr(loan, "QM.X219"));
      nameValueCollection.Add("QM_X220", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X220")), (byte) 0, 0));
      nameValueCollection.Add("QM_X221", JS.GetStr(loan, "QM.X221"));
      nameValueCollection.Add("QM_X222", JS.GetStr(loan, "QM.X222"));
      nameValueCollection.Add("QM_X223", JS.GetStr(loan, "QM.X223"));
      nameValueCollection.Add("QM_X224", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X224")), (byte) 0, 0));
      nameValueCollection.Add("QM_X225", JS.GetStr(loan, "QM.X225"));
      nameValueCollection.Add("QM_X226", JS.GetStr(loan, "QM.X226"));
      nameValueCollection.Add("QM_X227", JS.GetStr(loan, "QM.X227"));
      nameValueCollection.Add("QM_X228", JS.GetStr(loan, "QM.X228"));
      nameValueCollection.Add("QM_X229", JS.GetStr(loan, "QM.X229"));
      nameValueCollection.Add("QM_X355", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X355")), (byte) 0, 0));
      nameValueCollection.Add("QM_X319", JS.GetStr(loan, "QM.X319"));
      nameValueCollection.Add("QM_X320", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X320")), (byte) 0, 0));
      nameValueCollection.Add("QM_X321", JS.GetStr(loan, "QM.X321"));
      nameValueCollection.Add("QM_X322", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X322")), (byte) 0, 0));
      nameValueCollection.Add("QM_X323", JS.GetStr(loan, "QM.X323"));
      nameValueCollection.Add("QM_X356", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X356")), (byte) 0, 0));
      nameValueCollection.Add("QM_X230", JS.GetStr(loan, "QM.X230"));
      nameValueCollection.Add("QM_X231", JS.GetStr(loan, "QM.X231"));
      nameValueCollection.Add("QM_X232", JS.GetStr(loan, "QM.X232"));
      nameValueCollection.Add("QM_X233", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X233")), (byte) 0, 0));
      nameValueCollection.Add("QM_X234", JS.GetStr(loan, "QM.X234"));
      nameValueCollection.Add("QM_X235", JS.GetStr(loan, "QM.X235"));
      nameValueCollection.Add("QM_X236", JS.GetStr(loan, "QM.X236"));
      nameValueCollection.Add("QM_X237", JS.GetStr(loan, "QM.X237"));
      nameValueCollection.Add("QM_X238", JS.GetStr(loan, "QM.X238"));
      nameValueCollection.Add("QM_X357", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X357")), (byte) 0, 0));
      nameValueCollection.Add("QM_X324", JS.GetStr(loan, "QM.X324"));
      nameValueCollection.Add("QM_X325", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X325")), (byte) 0, 0));
      nameValueCollection.Add("QM_X326", JS.GetStr(loan, "QM.X326"));
      nameValueCollection.Add("QM_X358", JS.GetStr(loan, "QM.X358"));
      nameValueCollection.Add("QM_X359", JS.GetStr(loan, "QM.X359"));
      nameValueCollection.Add("QM_X360", JS.GetStr(loan, "QM.X360"));
      nameValueCollection.Add("QM_X361", JS.GetStr(loan, "QM.X361"));
      nameValueCollection.Add("QM_X362", JS.GetStr(loan, "QM.X362"));
      nameValueCollection.Add("QM_X363", JS.GetStr(loan, "QM.X363"));
      nameValueCollection.Add("QM_X251", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X251")), (byte) 0, 0));
      nameValueCollection.Add("QM_X252", JS.GetStr(loan, "QM.X252"));
      nameValueCollection.Add("QM_X253", JS.GetStr(loan, "QM.X253"));
      nameValueCollection.Add("QM_X254", JS.GetStr(loan, "QM.X254"));
      nameValueCollection.Add("QM_X255", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X255")), (byte) 0, 0));
      nameValueCollection.Add("QM_X256", JS.GetStr(loan, "QM.X256"));
      nameValueCollection.Add("QM_X257", JS.GetStr(loan, "QM.X257"));
      nameValueCollection.Add("QM_X258", JS.GetStr(loan, "QM.X258"));
      nameValueCollection.Add("QM_X331", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X331")), (byte) 0, 0));
      nameValueCollection.Add("QM_X332", JS.GetStr(loan, "QM.X332"));
      nameValueCollection.Add("QM_X333", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X333")), (byte) 0, 0));
      nameValueCollection.Add("QM_X334", JS.GetStr(loan, "QM.X334"));
      nameValueCollection.Add("QM_X261", JS.GetStr(loan, "QM.X261"));
      nameValueCollection.Add("QM_X262", JS.GetStr(loan, "QM.X262"));
      nameValueCollection.Add("QM_X259", JS.GetStr(loan, "QM.X259"));
      nameValueCollection.Add("QM_X260", JS.GetStr(loan, "QM.X260"));
      nameValueCollection.Add("QM_X239", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X239")), (byte) 0, 0));
      nameValueCollection.Add("QM_X240", JS.GetStr(loan, "QM.X240"));
      nameValueCollection.Add("QM_X241", JS.GetStr(loan, "QM.X241"));
      nameValueCollection.Add("QM_X242", JS.GetStr(loan, "QM.X242"));
      nameValueCollection.Add("QM_X243", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X243")), (byte) 0, 0));
      nameValueCollection.Add("QM_X244", JS.GetStr(loan, "QM.X244"));
      nameValueCollection.Add("QM_X245", JS.GetStr(loan, "QM.X245"));
      nameValueCollection.Add("QM_X246", JS.GetStr(loan, "QM.X246"));
      nameValueCollection.Add("QM_X247", JS.GetStr(loan, "QM.X247"));
      nameValueCollection.Add("QM_X248", JS.GetStr(loan, "QM.X248"));
      nameValueCollection.Add("QM_X327", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X327")), (byte) 0, 0));
      nameValueCollection.Add("QM_X328", JS.GetStr(loan, "QM.X328"));
      nameValueCollection.Add("QM_X329", Jed.NF(Jed.Num(JS.GetNum(loan, "QM.X329")), (byte) 0, 0));
      nameValueCollection.Add("QM_X330", JS.GetStr(loan, "QM.X330"));
      nameValueCollection.Add("QM_X249", JS.GetStr(loan, "QM.X249"));
      nameValueCollection.Add("QM_X250", JS.GetStr(loan, "QM.X250"));
      nameValueCollection.Add("QM_X142", JS.GetStr(loan, "QM.X142"));
      nameValueCollection.Add("QM_X150", JS.GetStr(loan, "QM.X150"));
      nameValueCollection.Add("QM_X263", JS.GetStr(loan, "QM.X263"));
      nameValueCollection.Add("QM_X264", JS.GetStr(loan, "QM.X264"));
      nameValueCollection.Add("QM_X265", JS.GetStr(loan, "QM.X265"));
      nameValueCollection.Add("QM_X266", JS.GetStr(loan, "QM.X266"));
      nameValueCollection.Add("QM_X267", JS.GetStr(loan, "QM.X267"));
      nameValueCollection.Add("QM_X268", JS.GetStr(loan, "QM.X268"));
      nameValueCollection.Add("QM_X269", JS.GetStr(loan, "QM.X269"));
      nameValueCollection.Add("QM_X270", JS.GetStr(loan, "QM.X270"));
      nameValueCollection.Add("QM_X271", JS.GetStr(loan, "QM.X271"));
      nameValueCollection.Add("QM_X272", JS.GetStr(loan, "QM.X272"));
      nameValueCollection.Add("QM_X273", JS.GetStr(loan, "QM.X273"));
      nameValueCollection.Add("QM_X274", JS.GetStr(loan, "QM.X274"));
      nameValueCollection.Add("QM_X275", JS.GetStr(loan, "QM.X275"));
      nameValueCollection.Add("QM_X276", JS.GetStr(loan, "QM.X276"));
      nameValueCollection.Add("QM_X277", JS.GetStr(loan, "QM.X277"));
      nameValueCollection.Add("QM_X278", JS.GetStr(loan, "QM.X278"));
      nameValueCollection.Add("QM_X279", JS.GetStr(loan, "QM.X279"));
      nameValueCollection.Add("QM_X280", JS.GetStr(loan, "QM.X280"));
      nameValueCollection.Add("FM1084_X6", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X6")));
      nameValueCollection.Add("FM1084_X50", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X50")));
      nameValueCollection.Add("FM1084_X133", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X133")));
      nameValueCollection.Add("FM1084_X168", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X168")));
      nameValueCollection.Add("FM1084_X133_FM1084_X168_24", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X168"), "", false) != 0, Jed.NF((Jed.Num(JS.GetNum(loan, "FM1084.X133")) + Jed.Num(JS.GetNum(loan, "FM1084.X168"))) / 24.0, (byte) 18, 0), Jed.NF(Jed.Num(JS.GetNum(loan, "FM1084.X133")) / 12.0, (byte) 18, 0))));
      nameValueCollection.Add("FM1084_X176", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X176")));
      nameValueCollection.Add("FM1084_X6_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "FE0215"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X6")));
      nameValueCollection.Add("FM1084_X50_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "FE0215"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X50")));
      nameValueCollection.Add("FM1084_X133_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "FE0215"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X133")));
      nameValueCollection.Add("FM1084_X168_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "FE0215"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X168")));
      nameValueCollection.Add("FM1084_X133_FM1084_X168_24_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "FE0215"), "Y", false) == 0, Jed.BF(Operators.CompareString(JS.GetStr(loan, "FM1084.X168"), "", false) != 0, Jed.NF((Jed.Num(JS.GetNum(loan, "FM1084.X133")) + Jed.Num(JS.GetNum(loan, "FM1084.X168"))) / 24.0, (byte) 18, 0), Jed.NF(Jed.Num(JS.GetNum(loan, "FM1084.X133")) / 12.0, (byte) 18, 0))));
      nameValueCollection.Add("FM1084_X176_a", Jed.BF(Operators.CompareString(JS.GetStr(loan, "FE0115"), "Y", false) != 0 & Operators.CompareString(JS.GetStr(loan, "FE0215"), "Y", false) == 0, JS.GetStr(loan, "FM1084.X176")));
      return nameValueCollection;
    }
  }
}
