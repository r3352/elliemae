// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.BamObjects.FulfillmentFields
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ePass.BamEnums;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ePass.BamObjects
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
