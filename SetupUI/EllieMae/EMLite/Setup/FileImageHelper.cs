// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.FileImageHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.RemotingServices;
using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Script.Serialization;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class FileImageHelper
  {
    private const long minSizeInByte = 3072;
    private const long maxSizeInByte = 2000000;

    public static string GetToken()
    {
      string requestUriString = Session.SessionObjects.ConfigurationManager.getOAPIURL() + "/oauth2/v1/token";
      string str1 = string.Empty;
      try
      {
        string str2 = Session.ServerIdentity.InstanceName + "_" + Session.SessionObjects.SessionID;
        string str3 = "smartclient";
        string str4 = "nJWEYegDJk1iRIWvK9BL9Nfk6o5t7criMvCUgVDqHqNP9TgbL6tHhmgjr5St7Sgm";
        string str5 = "urn:elli:params:oauth:grant-type:encompass-bearer";
        WebRequest webRequest = WebRequest.Create(requestUriString);
        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        byte[] bytes = Encoding.UTF8.GetBytes("client_id=" + str3 + "&client_secret=" + str4 + "&grant_type=" + str5 + "&session=" + str2 + "&scope=ccap cc");
        webRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = webRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        string end;
        using (StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
          end = streamReader.ReadToEnd();
        object obj1 = new JavaScriptSerializer().DeserializeObject(end);
        // ISSUE: reference to a compiler-generated field
        if (FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (FileImageHelper)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, string, object>>.Create(Binder.GetIndex(CSharpBinderFlags.None, typeof (FileImageHelper), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__0.Target((CallSite) FileImageHelper.\u003C\u003Eo__5.\u003C\u003Ep__0, obj1, "access_token");
        str1 = target((CallSite) p1, obj2);
      }
      catch
      {
      }
      return "Bearer " + str1;
    }

    public static string UploadImage(Image img)
    {
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        img.Save((Stream) memoryStream, ImageFormat.Jpeg);
        array = memoryStream.ToArray();
      }
      string token = FileImageHelper.GetToken();
      string data = new JavaScriptSerializer().Serialize((object) new FileImageHelper.Asset());
      string address = Session.SessionObjects.ConfigurationManager.getOAPIURL() + "/content/v1/assets";
      FileImageHelper.Response response1 = (FileImageHelper.Response) null;
      using (WebClient webClient = new WebClient())
      {
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        webClient.Headers[HttpRequestHeader.Authorization] = token;
        response1 = new JavaScriptSerializer().Deserialize<FileImageHelper.Response>(webClient.UploadString(address, data));
      }
      string url = response1.url;
      string id = response1.id;
      using (WebClient webClient = new WebClient())
        webClient.UploadData(url, "PUT", array);
      FileImageHelper.Response response2 = (FileImageHelper.Response) null;
      using (WebClient webClient = new WebClient())
      {
        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
        webClient.Headers[HttpRequestHeader.Authorization] = token;
        response2 = new JavaScriptSerializer().Deserialize<FileImageHelper.Response>(webClient.DownloadString(address + "/" + id));
      }
      return response2.url;
    }

    public static byte[] Base64UrlDecode(string arg)
    {
      string s = arg.Replace('-', '+').Replace('_', '/');
      switch (s.Length % 4)
      {
        case 0:
          return Convert.FromBase64String(s);
        case 2:
          s += "==";
          goto case 0;
        case 3:
          s += "=";
          goto case 0;
        default:
          throw new Exception("Illegal base64url string!");
      }
    }

    public static void deleteImage(string ID)
    {
      string address = Session.SessionObjects.ConfigurationManager.getOAPIURL() + "/content/v1/assets/" + ID;
      using (WebClient webClient = new WebClient())
        webClient.UploadString(address, "DELETE", "");
    }

    public static Bitmap ByteToImage(byte[] blob)
    {
      MemoryStream memoryStream = new MemoryStream();
      byte[] buffer = blob;
      memoryStream.Write(buffer, 0, Convert.ToInt32(buffer.Length));
      Bitmap image = new Bitmap((Stream) memoryStream, false);
      memoryStream.Dispose();
      return image;
    }

    public static Bitmap RetrieveImage(string url)
    {
      using (WebClient webClient = new WebClient())
        return FileImageHelper.ByteToImage(webClient.DownloadData(url));
    }

    public static bool ValidPhoto(string filename)
    {
      long length = new FileInfo(filename).Length;
      return length > 3072L && length < 2000000L;
    }

    public static bool ValidDimensions(int width, int height) => width >= 300 || height >= 300;

    public static Image FixedSize(Image imgPhoto, int Width, int Height)
    {
      int width1 = imgPhoto.Width;
      int height1 = imgPhoto.Height;
      int x1 = 0;
      int y1 = 0;
      int x2 = 0;
      int y2 = 0;
      float num1 = (float) Width / (float) width1;
      float num2 = (float) Height / (float) height1;
      float num3;
      if ((double) num2 < (double) num1)
      {
        num3 = num2;
        x2 = (int) Convert.ToInt16((float) (((double) Width - (double) width1 * (double) num3) / 2.0));
      }
      else
      {
        num3 = num1;
        y2 = (int) Convert.ToInt16((float) (((double) Height - (double) height1 * (double) num3) / 2.0));
      }
      int width2 = (int) ((double) width1 * (double) num3);
      int height2 = (int) ((double) height1 * (double) num3);
      Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
      bitmap.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
      Graphics graphics = Graphics.FromImage((Image) bitmap);
      graphics.Clear(Color.Transparent);
      graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
      graphics.DrawImage(imgPhoto, new Rectangle(x2, y2, width2, height2), new Rectangle(x1, y1, width1, height1), GraphicsUnit.Pixel);
      graphics.Dispose();
      return (Image) bitmap;
    }

    private class Asset
    {
    }

    private class AssetDimensionDTO
    {
    }

    public class Response
    {
      public string id { get; set; }

      public string url { get; set; }
    }
  }
}
