// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.TrackingFields
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class TrackingFields
  {
    public List<DT2015TrackingIndicators> Indicators { get; set; }

    public string DisclosedMessage { get; set; }

    public DateTime PackageCreatedDate { get; set; }

    public string PackageId { get; set; }

    public string DocPackageId { get; set; }

    public TrackingFields(
      List<DT2015TrackingIndicators> indicators,
      string disclosedMessage,
      DateTime packageCreatedDate,
      string packageId,
      string docPackageId)
    {
      this.Indicators = indicators;
      this.DisclosedMessage = disclosedMessage;
      this.PackageCreatedDate = packageCreatedDate;
      this.PackageId = packageId;
      this.DocPackageId = docPackageId;
    }
  }
}
