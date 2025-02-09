// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LicenseRequest
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Server
{
  internal class LicenseRequest
  {
    private string url;
    private Hashtable postData;
    private TimeSpan timeout = new TimeSpan(0, 0, 5);

    public LicenseRequest(string url, Hashtable postData)
    {
      this.url = url;
      this.postData = postData;
    }

    public TimeSpan Timeout
    {
      get => this.timeout;
      set => this.timeout = value;
    }

    public string Send()
    {
      byte[] requestBody = this.getRequestBody();
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.url);
      httpWebRequest.AllowAutoRedirect = true;
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/x-www-form-urlencoded";
      httpWebRequest.ContentLength = (long) requestBody.Length;
      httpWebRequest.Proxy = WebRequest.DefaultWebProxy;
      httpWebRequest.ReadWriteTimeout = (int) this.timeout.TotalMilliseconds;
      httpWebRequest.Timeout = (int) this.timeout.TotalMilliseconds;
      httpWebRequest.KeepAlive = false;
      Stream requestStream = httpWebRequest.GetRequestStream();
      requestStream.Write(requestBody, 0, requestBody.Length);
      requestStream.Close();
      using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
      {
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.ASCII))
          return streamReader.ReadToEnd();
      }
    }

    private byte[] getRequestBody()
    {
      if (this.postData == null)
        return new byte[0];
      StringBuilder stringBuilder = new StringBuilder();
      bool flag = true;
      foreach (DictionaryEntry dictionaryEntry in this.postData)
      {
        if (!flag)
          stringBuilder.Append("&");
        stringBuilder.Append(HttpUtility.UrlEncode(dictionaryEntry.Key.ToString()) + "=" + HttpUtility.UrlEncode(dictionaryEntry.Value.ToString()));
        flag = false;
      }
      return Encoding.ASCII.GetBytes(stringBuilder.ToString());
    }
  }
}
