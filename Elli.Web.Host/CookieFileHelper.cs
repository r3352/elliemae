// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.CookieFileHelper
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using DotNetBrowser.Cookies;
using EllieMae.EMLite.Common;
using Encompass.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Elli.Web.Host
{
  public class CookieFileHelper
  {
    private const string COOKIE_FILE_NAME = "Cookies";
    private const string className = "CookieFileHelper";
    private static string backupPath = Paths.ChromiumDefaultCookieBackup;
    private static readonly string sw = Tracing.SwThinThick;

    internal static void WriteToBackup(IEnumerable<Cookie> cookies)
    {
      try
      {
        if (cookies == null)
          return;
        byte[] content = DataProtectionAPI.EncryptStream(Encoding.ASCII.GetBytes(CookieBuilder.Jsonify(cookies)), DataProtectionScope.CurrentUser);
        FileHelper.WriteBytes(CookieFileHelper.backupPath, "Cookies", content, true);
      }
      catch (Exception ex)
      {
        Tracing.Log(CookieFileHelper.sw, TraceLevel.Warning, nameof (CookieFileHelper), "Cookie file write failed: " + ex.Message);
      }
    }

    internal static IEnumerable<Cookie> ReadFromBackup()
    {
      IEnumerable<Cookie> cookies = (IEnumerable<Cookie>) null;
      try
      {
        byte[] buffer = FileHelper.ReadBytes(CookieFileHelper.backupPath, "Cookies");
        if (buffer != null)
        {
          if (buffer.Length != 0)
            cookies = CookieBuilder.BuildAll(Encoding.ASCII.GetString(DataProtectionAPI.DecryptStream(buffer, DataProtectionScope.CurrentUser)));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CookieFileHelper.sw, TraceLevel.Warning, nameof (CookieFileHelper), "Cookie file read failed: " + ex.Message);
      }
      return cookies;
    }
  }
}
