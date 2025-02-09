// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Export.BamObjects.FulfillmentFields
// Assembly: EMExport, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D06A4C35-7634-4F74-B132-8DD78784C14A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMExport.dll

using EllieMae.EMLite.Export.BamEnums;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Export.BamObjects
{
  public class FulfillmentFields
  {
    private DateTime _processedDate;

    public bool IsManual { get; set; }

    public string Id { get; set; }

    public DisclosedMethodEnum DisclosedMethod { get; set; }

    public List<FulfillmentRecipient> Recipients { get; set; }

    public string OrderedBy { get; set; }

    public string TrackingNumber { get; set; }

    public DateTime ProcessedDate
    {
      get => this._processedDate;
      set => this._processedDate = value;
    }

    public FulfillmentFields(
      bool isManual,
      string id,
      DisclosedMethodEnum disclosedMethod,
      List<FulfillmentRecipient> recipients,
      string orderedBy,
      DateTime processedDate,
      string trackingNumber = "")
    {
      this.IsManual = isManual;
      this.Id = id;
      this.DisclosedMethod = disclosedMethod;
      this.Recipients = recipients;
      this.OrderedBy = orderedBy;
      this.ProcessedDate = processedDate;
      this.TrackingNumber = trackingNumber;
    }
  }
}
