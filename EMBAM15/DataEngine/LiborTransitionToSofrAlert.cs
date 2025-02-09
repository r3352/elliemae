// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LiborTransitionToSofrAlert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public static class LiborTransitionToSofrAlert
  {
    public static IList<string> TriggerFields = (IList<string>) new List<string>()
    {
      "1959",
      "745",
      "3142"
    };
    public static string Name = "LIBOR Index";
    public static string TriggerDescription = "Loan Index Type (1959) and either Origination Date (745) or Application Date (3142)";
    public static string Message = "LIBOR index is not supported by the GSEs for applications taken after September 30, 2020. Please confirm with secondary marketing if this loan should be originated on LIBOR.";

    public static PipelineInfo.Alert GetAlert(LoanData loan)
    {
      PipelineInfo.Alert alert = (PipelineInfo.Alert) null;
      string field = loan.GetField("1959");
      string str = "LIBOR";
      DateTime date1 = Utils.ParseDate((object) loan.GetSimpleField("745"));
      DateTime date2 = Utils.ParseDate((object) loan.GetSimpleField("3142"));
      DateTime dateTime = new DateTime(2020, 10, 1);
      bool flag1 = field.IndexOf(str, StringComparison.OrdinalIgnoreCase) > -1;
      bool flag2 = dateTime <= date1;
      bool flag3 = dateTime <= date2;
      if (flag1 && flag2 | flag3)
        alert = new PipelineInfo.Alert(68, "", "", DateTime.Today, (string) null, (string) null);
      return alert;
    }
  }
}
