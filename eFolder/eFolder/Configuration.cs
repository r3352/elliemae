// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.Configuration
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System;
using System.Configuration;

#nullable disable
namespace EllieMae.EMLite.eFolder
{
  internal static class Configuration
  {
    public static string VaultServerUrl
    {
      get => ConfigurationManager.AppSettings[nameof (VaultServerUrl)].ToString();
    }

    public static int VaultBufferSize
    {
      get => Convert.ToInt32(ConfigurationManager.AppSettings[nameof (VaultBufferSize)].ToString());
    }

    public static string MediaTokenCreator
    {
      get => ConfigurationManager.AppSettings[nameof (MediaTokenCreator)].ToString();
    }

    public static string MediaTokenExpires
    {
      get => ConfigurationManager.AppSettings[nameof (MediaTokenExpires)].ToString();
    }

    public static string MediaFileExpires
    {
      get => ConfigurationManager.AppSettings[nameof (MediaFileExpires)].ToString();
    }
  }
}
