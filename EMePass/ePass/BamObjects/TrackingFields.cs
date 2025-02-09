// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.TrackingFields
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
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
