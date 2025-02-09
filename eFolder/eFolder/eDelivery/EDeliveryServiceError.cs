// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.EDeliveryServiceError
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.Runtime.Serialization;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class EDeliveryServiceError : HttpException
  {
    public EDeliveryServiceError(SerializationInfo info, StreamingContext context)
    {
      if (info == null)
        return;
      this.summary = info.GetString(nameof (summary));
      this.code = info.GetString(nameof (code));
      this.errors = (Error[]) info.GetValue(nameof (errors), typeof (Error[]));
      this.details = info.GetString(nameof (details));
    }

    public string GetDisplayErrorMessage() => "Package creation failed";

    public string GetDetailedErrorMessageForLogging()
    {
      string str = "";
      if (this.errors != null)
      {
        foreach (Error error in this.errors)
          str = str + Environment.NewLine + error.summary + Environment.NewLine + error.details;
      }
      return " Error details: " + this.summary + Environment.NewLine + str;
    }

    public string code { get; set; }

    public string summary { get; set; }

    public string details { get; set; }

    public Error[] errors { get; set; }
  }
}
