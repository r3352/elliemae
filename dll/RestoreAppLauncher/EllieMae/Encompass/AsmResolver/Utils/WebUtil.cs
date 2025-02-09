// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.WebUtil
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using System;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  internal class WebUtil
  {
    internal static string HttpGet(string url, string guidPrefix)
    {
      if (guidPrefix != null)
        url = url + guidPrefix + Guid.NewGuid().ToString();
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      WebUtil.setProxyCredentials((WebRequest) httpWebRequest);
      httpWebRequest.Method = "GET";
      StringBuilder stringBuilder = new StringBuilder();
      HttpWebResponse httpWebResponse = (HttpWebResponse) null;
      Stream stream = (Stream) null;
      StreamReader streamReader = (StreamReader) null;
      try
      {
        httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        stream = httpWebResponse.GetResponseStream();
        streamReader = new StreamReader(stream);
        char[] buffer = new char[1024];
        for (int length = streamReader.Read(buffer, 0, buffer.Length); length > 0; length = streamReader.Read(buffer, 0, buffer.Length))
        {
          string str = new string(buffer, 0, length);
          stringBuilder.Append(str);
        }
      }
      finally
      {
        streamReader?.Close();
        stream?.Close();
        httpWebResponse?.Close();
      }
      return stringBuilder.ToString();
    }

    internal static byte[] GetFile(string url, long blockSize, long size, string progressBarTitle)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(WebUtil.myUrlEncode(url));
      WebUtil.setProxyCredentials((WebRequest) httpWebRequest);
      Stream responseStream = httpWebRequest.GetResponse().GetResponseStream();
      blockSize = blockSize > 0L ? blockSize : 10485760L;
      byte[] numArray1;
      if (size > 0L)
      {
        blockSize = Math.Min(blockSize, size);
        numArray1 = new byte[size];
      }
      else
        numArray1 = new byte[blockSize];
      byte[] numArray2 = new byte[blockSize];
      long length1 = 0;
      IProgressBar progressBar = (IProgressBar) null;
      if (progressBarTitle != null)
      {
        progressBar = (IProgressBar) new ProgressBarForm(progressBarTitle ?? "");
        progressBar.Minimum = 0;
        progressBar.Maximum = (int) size;
        progressBar.Value = 0;
        progressBar.ShowProgressBar();
      }
      long length2;
      while ((length2 = (long) responseStream.Read(numArray2, 0, numArray2.Length)) > 0L)
      {
        if (length1 + length2 > (long) numArray1.Length)
        {
          byte[] destinationArray = new byte[Math.Max((long) numArray1.Length + blockSize, (long) (numArray1.Length * 2))];
          Array.Copy((Array) numArray1, (Array) destinationArray, length1);
          numArray1 = destinationArray;
        }
        Array.Copy((Array) numArray2, 0L, (Array) numArray1, length1, length2);
        length1 += length2;
        if (progressBar != null)
        {
          try
          {
            progressBar.Value = (int) length1;
          }
          catch
          {
            try
            {
              progressBar.CloseProgressBar();
              progressBar = (IProgressBar) null;
            }
            catch
            {
            }
          }
        }
      }
      progressBar?.CloseProgressBar();
      responseStream?.Close();
      byte[] destinationArray1 = new byte[length1];
      Array.Copy((Array) numArray1, (Array) destinationArray1, length1);
      return destinationArray1;
    }

    private static void setProxyCredentials(WebRequest webRequest)
    {
      WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
      webRequest.Proxy = WebRequest.DefaultWebProxy;
      webRequest.Proxy.Credentials = (ICredentials) CredentialCache.DefaultNetworkCredentials;
    }

    private static string myUrlEncode(string url)
    {
      url = url.Replace("%", "%25");
      url = url.Replace("#", "%23");
      url = url.Replace('\\', '/');
      return url;
    }
  }
}
