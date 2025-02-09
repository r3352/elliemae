// Decompiled with JetBrains decompiler
// Type: Encompass.Export.HmdaFieldsExport
// Assembly: Export.Fannie32, Version=1.0.7572.19737, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 8E2848B0-2048-4927-92C6-BBAFEF09B5DF
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EMN\Export.Fannie32.dll

using Export.Fannie32;
using System;
using System.Collections.Generic;

#nullable disable
namespace Encompass.Export
{
  internal class HmdaFieldsExport
  {
    private static Encompass.Export.Fannie32 _fannie32;

    internal HmdaFieldsExport(Encompass.Export.Fannie32 fannie32)
    {
      HmdaFieldsExport._fannie32 = fannie32;
    }

    internal void BuildAdsForHmda(
      string borSsn,
      string coborSsn,
      string borFirstName,
      string coborFirstName)
    {
      this.BuildExportForHmda(new HmdaFacade(borFirstName, borSsn, coborFirstName, coborSsn, HmdaFieldsExport._fannie32).GetHmdaDataPointsDictionary());
    }

    internal void BuildExportForHmda(
      List<Tuple<string, string>> borrowerDataPointsDictionary)
    {
      foreach (Tuple<string, string> borrowerDataPoints in borrowerDataPointsDictionary)
      {
        if (!string.IsNullOrWhiteSpace(borrowerDataPoints.Item2))
        {
          HmdaFieldsExport._fannie32.AddSingleValue("ADS");
          HmdaFieldsExport._fannie32.AddValue(borrowerDataPoints.Item1, 35, false);
          HmdaFieldsExport._fannie32.AddValue(borrowerDataPoints.Item2, 50, false);
          HmdaFieldsExport._fannie32.AddSingleValue(Environment.NewLine);
        }
      }
    }
  }
}
