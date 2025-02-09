// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.Forms.Packages.ConfigurationManagerExtensions
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.InputEngine.Forms.Packages
{
  public static class ConfigurationManagerExtensions
  {
    public static string[] GetFilteredCustomDataObjectNames(
      this IConfigurationManager configurationManager)
    {
      string[] customDataObjectNames = configurationManager.GetCustomDataObjectNames();
      List<string> stringList = new List<string>();
      foreach (string filename in customDataObjectNames)
      {
        if (!ConfigurationManagerExtensions.IsOperatingSystemFile(filename))
          stringList.Add(filename);
      }
      return stringList.ToArray();
    }

    private static bool IsOperatingSystemFile(string filename)
    {
      return string.IsNullOrEmpty(filename) || filename.Equals("Thumbs.db", StringComparison.CurrentCultureIgnoreCase);
    }
  }
}
