// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.PartnerAPI.PapiSettings
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using Microsoft.Win32;
using System;

#nullable disable
namespace EllieMae.EMLite.ePass.PartnerAPI
{
  public static class PapiSettings
  {
    private const string PAPIRoot = "Software\\Ellie Mae\\Encompass\\PAPI";
    private static bool isDebug = string.Concat(PapiSettings.readValue(nameof (Debug))) == "1";

    public static bool Debug => PapiSettings.isDebug;

    private static object readValue(string key)
    {
      string keyPath;
      string keyValueName;
      PapiSettings.parseKey(key, out keyPath, out keyValueName);
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass\\PAPI" + keyPath))
        return registryKey?.GetValue(keyValueName);
    }

    private static void writeValue(string key, object value)
    {
      string keyPath;
      string keyValueName;
      PapiSettings.parseKey(key, out keyPath, out keyValueName);
      using (RegistryKey subKey = Registry.LocalMachine.CreateSubKey("Software\\Ellie Mae\\Encompass\\PAPI" + keyPath))
      {
        if (subKey == null)
          throw new ApplicationException("Unable to create registry key Software\\Ellie Mae\\Encompass\\PAPI" + keyPath);
        if (value == null)
          subKey.DeleteValue(keyValueName);
        else
          subKey.SetValue(keyValueName, value);
      }
    }

    private static void parseKey(string key, out string keyPath, out string keyValueName)
    {
      keyPath = "";
      keyValueName = key;
      int length = key.LastIndexOf('\\');
      if (length < 0)
        return;
      if (length == key.Length - 1)
      {
        keyPath = key;
        keyValueName = "";
      }
      else
      {
        keyPath = key.Substring(0, length);
        keyValueName = key.Substring(length + 1, key.Length - length - 1);
      }
      if (keyPath.StartsWith("\\"))
        return;
      keyPath = "\\" + keyPath;
    }
  }
}
