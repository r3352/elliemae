// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.MediaServer.MediaServerException
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery.MediaServer
{
  public class MediaServerException : Exception
  {
    public MediaServerException(MediaServerError error, HttpStatusCode statuscode)
    {
      this.Error = error;
      this.StatusCode = statuscode;
    }

    public MediaServerError Error { get; set; }

    public HttpStatusCode StatusCode { get; set; }
  }
}
