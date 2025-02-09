// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.Paths
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.RemotingServices;
using System.IO;

#nullable disable
namespace Elli.Web.Host
{
  internal class Paths
  {
    internal static string ChromiumContextPath
    {
      get => Path.Combine(SystemSettings.TempFolderRoot, "Chromium");
    }

    internal static string ChromiumDefaultCookieStore
    {
      get => Path.Combine(Paths.ChromiumContextPath, "Default\\Network");
    }

    internal static string ChromiumDefaultCookieBackup
    {
      get => Path.Combine(SystemSettings.SettingsFolderRoot, "Chromium");
    }
  }
}
