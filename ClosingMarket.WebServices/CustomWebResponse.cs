// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.CustomWebResponse
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System;
using System.IO;
using System.Net;

#nullable disable
namespace ClosingMarket.WebServices
{
  public class CustomWebResponse : WebResponse
  {
    private HttpWebRequest webRequest;
    private HttpWebResponse webResponse;

    public event ChunkHandler ChunkReceived;

    public event EventHandler ResponseComplete;

    public CustomWebResponse(WebRequest webRequest)
    {
      this.webRequest = (HttpWebRequest) webRequest;
      this.webResponse = (HttpWebResponse) webRequest.GetResponse();
    }

    public override void Close() => this.webResponse.Close();

    public override long ContentLength
    {
      get => this.webResponse.ContentLength;
      set => this.webResponse.ContentLength = value;
    }

    public override string ContentType
    {
      get => this.webResponse.ContentType;
      set => this.webResponse.ContentType = value;
    }

    public override Stream GetResponseStream()
    {
      MemoryStream responseStream1 = new MemoryStream();
      long contentLength = this.webResponse.ContentLength;
      Stream responseStream2 = this.webResponse.GetResponseStream();
      try
      {
        byte[] buffer = new byte[4096];
        long transferredBytes = 0;
        for (int count = responseStream2.Read(buffer, 0, buffer.Length); count > 0; count = responseStream2.Read(buffer, 0, buffer.Length))
        {
          responseStream1.Write(buffer, 0, count);
          transferredBytes += Convert.ToInt64(count);
          if (this.ChunkReceived != null)
          {
            ChunkHandlerEventArgs e = new ChunkHandlerEventArgs(transferredBytes, contentLength);
            this.ChunkReceived((object) this, e);
            if (e.Abort)
              this.webRequest.Abort();
          }
        }
        if (this.ResponseComplete != null)
          this.ResponseComplete((object) this, new EventArgs());
        return (Stream) responseStream1;
      }
      finally
      {
        responseStream2.Close();
      }
    }

    public override WebHeaderCollection Headers => this.webResponse.Headers;

    public override Uri ResponseUri => this.webResponse.ResponseUri;
  }
}
