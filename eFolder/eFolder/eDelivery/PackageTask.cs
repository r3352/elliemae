// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.PackageTask
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;
using System;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class PackageTask : EDeliveryRequest
  {
    [JsonIgnore]
    public string PackageId { get; set; }

    [JsonIgnore]
    public string RecipientId { get; set; }

    [JsonIgnore]
    public string TaskId { get; set; }

    public DateTime? ViewedDate { get; set; }

    public string ViewedIpAddress { get; set; }

    public DateTime? CompletedDate { get; set; }

    public string CompletedIpAddress { get; set; }
  }
}
