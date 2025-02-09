// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.CustomWebRequest
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.IO;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  public class CustomWebRequest : WebRequest
  {
    private HttpWebRequest webRequest;
    private MemoryStream memoryStream;

    public event ChunkHandler ChunkSent;

    public event ChunkHandler ChunkReceived;

    public CustomWebRequest(Uri uri)
    {
      this.webRequest = (HttpWebRequest) WebRequest.Create(uri);
      this.webRequest.KeepAlive = false;
      this.webRequest.Timeout = 900000;
      this.webRequest.AllowWriteStreamBuffering = false;
    }

    public override void Abort() => this.webRequest.Abort();

    public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
    {
      return this.webRequest.BeginGetRequestStream(callback, state);
    }

    public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
    {
      return this.webRequest.BeginGetResponse(callback, state);
    }

    public override string ConnectionGroupName
    {
      get => this.webRequest.ConnectionGroupName;
      set => this.webRequest.ConnectionGroupName = value;
    }

    public override long ContentLength
    {
      get => this.webRequest.ContentLength;
      set => this.webRequest.ContentLength = value;
    }

    public override string ContentType
    {
      get => this.webRequest.ContentType;
      set => this.webRequest.ContentType = value;
    }

    public override ICredentials Credentials
    {
      get => this.webRequest.Credentials;
      set => this.webRequest.Credentials = value;
    }

    public override Stream EndGetRequestStream(IAsyncResult asyncResult)
    {
      return this.webRequest.EndGetRequestStream(asyncResult);
    }

    public override WebResponse EndGetResponse(IAsyncResult asyncResult)
    {
      return this.webRequest.EndGetResponse(asyncResult);
    }

    public override Stream GetRequestStream()
    {
      this.memoryStream = new MemoryStream();
      return (Stream) this.memoryStream;
    }

    public override WebResponse GetResponse()
    {
      this.memoryStream = new MemoryStream(this.memoryStream.ToArray(), false);
      this.webRequest.ContentLength = this.memoryStream.Length;
      long length = this.memoryStream.Length;
      Stream requestStream = this.webRequest.GetRequestStream();
      try
      {
        byte[] buffer = new byte[4096];
        long transferredBytes = 0;
        for (int count = this.memoryStream.Read(buffer, 0, buffer.Length); count > 0; count = this.memoryStream.Read(buffer, 0, buffer.Length))
        {
          requestStream.Write(buffer, 0, count);
          transferredBytes += Convert.ToInt64(count);
          if (this.ChunkSent != null)
          {
            ChunkHandlerEventArgs e = new ChunkHandlerEventArgs(transferredBytes, length);
            this.ChunkSent((object) this, e);
            if (e.Abort)
              this.webRequest.Abort();
          }
        }
        CustomWebResponse response = new CustomWebResponse((WebRequest) this.webRequest);
        response.ChunkReceived += new ChunkHandler(this.webResponse_ChunkReceived);
        return (WebResponse) response;
      }
      finally
      {
        this.memoryStream.Close();
        requestStream.Close();
      }
    }

    private void webResponse_ChunkReceived(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkReceived == null)
        return;
      this.ChunkReceived((object) this, e);
    }

    public override WebHeaderCollection Headers
    {
      get => this.webRequest.Headers;
      set => this.webRequest.Headers = value;
    }

    public override string Method
    {
      get => this.webRequest.Method;
      set => this.webRequest.Method = value;
    }

    public override bool PreAuthenticate
    {
      get => this.webRequest.PreAuthenticate;
      set => this.webRequest.PreAuthenticate = value;
    }

    public override IWebProxy Proxy
    {
      get => this.webRequest.Proxy;
      set => this.webRequest.Proxy = value;
    }

    public override Uri RequestUri => this.webRequest.RequestUri;

    public override int Timeout
    {
      get => this.webRequest.Timeout;
      set => this.webRequest.Timeout = value;
    }
  }
}
