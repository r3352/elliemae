// Decompiled with JetBrains decompiler
// Type: ClosingMarket.WebServices.CustomWebRequest
// Assembly: ClosingMarket.WebServices, Version=1.0.2749.29102, Culture=neutral, PublicKeyToken=null
// MVID: 510652A0-EF36-486C-9EB6-CECE9FC11560
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClosingMarket.WebServices.dll

using System;
using System.IO;
using System.Net;

#nullable disable
namespace ClosingMarket.WebServices
{
  public class CustomWebRequest : WebRequest
  {
    private HttpWebRequest webRequest;
    private MemoryStream memoryStream;

    public event ChunkHandler ChunkSent;

    public event ChunkHandler ChunkReceived;

    public event EventHandler RequestComplete;

    public event EventHandler ResponseComplete;

    public CustomWebRequest(Uri uri)
    {
      this.webRequest = (HttpWebRequest) WebRequest.Create(uri);
      this.webRequest.KeepAlive = false;
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
      MemoryStream memoryStream = new MemoryStream(this.memoryStream.ToArray(), false);
      this.webRequest.ContentLength = memoryStream.Length;
      long length = memoryStream.Length;
      Stream requestStream = this.webRequest.GetRequestStream();
      try
      {
        byte[] buffer = new byte[4096];
        long transferredBytes = 0;
        for (int count = memoryStream.Read(buffer, 0, buffer.Length); count > 0; count = memoryStream.Read(buffer, 0, buffer.Length))
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
        if (this.RequestComplete != null)
          this.RequestComplete((object) this, new EventArgs());
        CustomWebResponse response = new CustomWebResponse((WebRequest) this.webRequest);
        response.ChunkReceived += new ChunkHandler(this.webResponse_ChunkReceived);
        response.ResponseComplete += new EventHandler(this.webResponse_ResponseComplete);
        return (WebResponse) response;
      }
      finally
      {
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

    private void webResponse_ResponseComplete(object sender, EventArgs e)
    {
      if (this.ResponseComplete == null)
        return;
      this.ResponseComplete((object) this, e);
    }
  }
}
