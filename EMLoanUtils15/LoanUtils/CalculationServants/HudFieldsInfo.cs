// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.CalculationServants.HudFieldsInfo
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.CalculationEngine;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.CalculationServants
{
  public static class HudFieldsInfo
  {
    public static Hashtable BorHudFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
    public static Hashtable SelHudFields = CollectionsUtil.CreateCaseInsensitiveHashtable();
    public static string[] Line801PaidByFields = (string[]) null;

    static HudFieldsInfo()
    {
      foreach (string[] strArray in HUDGFE2010Fields.HUD2010ToGFE2010FIELDMAP)
      {
        HudFieldsInfo.BorHudFields.Add((object) strArray[1], (object) strArray);
        if (strArray[2] != string.Empty && !HudFieldsInfo.SelHudFields.ContainsKey((object) strArray[2]))
          HudFieldsInfo.SelHudFields.Add((object) strArray[2], (object) strArray);
      }
      HudFieldsInfo.Line801PaidByFields = new string[22]
      {
        "SYS.X251",
        "SYS.X261",
        "SYS.X269",
        "SYS.X271",
        "SYS.X265",
        "NEWHUD.X227",
        "SYS.X289",
        "SYS.X291",
        "SYS.X296",
        "SYS.X301",
        "NEWHUD.X748",
        "NEWHUD.X1239",
        "NEWHUD.X1247",
        "NEWHUD.X1255",
        "NEWHUD.X1263",
        "NEWHUD.X1271",
        "NEWHUD.X1279",
        "NEWHUD.X1287",
        "NEWHUD.X1175",
        "NEWHUD.X1179",
        "NEWHUD.X1183",
        "NEWHUD.X1187"
      };
    }
  }
}
