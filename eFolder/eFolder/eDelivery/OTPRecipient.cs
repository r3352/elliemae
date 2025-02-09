// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.OTPRecipient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using Newtonsoft.Json;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class OTPRecipient : Recipient
  {
    [JsonIgnore]
    public string PartyId { get; set; }

    [JsonIgnore]
    public string PhoneNumber { get; set; }

    [JsonIgnore]
    public PhoneType PhoneType { get; set; }
  }
}
