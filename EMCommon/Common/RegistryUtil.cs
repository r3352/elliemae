// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RegistryUtil
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class RegistryUtil
  {
    public static void CreateSubKey(RegistryKey rootKey, string subKeyPath)
    {
      using (RegistryKey registryKey = rootKey.OpenSubKey(subKeyPath))
      {
        if (registryKey != null)
          return;
      }
      rootKey.CreateSubKey(subKeyPath);
    }

    public static object GetRegistryValue(RegistryKey rootKey, string subKeyPath, string valName)
    {
      using (RegistryKey registryKey = rootKey.OpenSubKey(subKeyPath))
        return registryKey?.GetValue(valName);
    }

    public static void SetRegistryValue(
      RegistryKey rootKey,
      string subKeyPath,
      string valName,
      object value)
    {
      using (RegistryKey registryKey = rootKey.OpenSubKey(subKeyPath, true))
        registryKey?.SetValue(valName, value);
    }

    public static void SetRegistryValues(
      RegistryKey rootKey,
      string subKeyPath,
      Dictionary<string, object> values)
    {
      using (RegistryKey registryKey = rootKey.OpenSubKey(subKeyPath, true))
      {
        foreach (string key in values.Keys)
          registryKey.SetValue(key, values[key]);
      }
    }

    private static void getRegistryValues(
      RegistryKey rootKey,
      string subKeyPath,
      Dictionary<string, object> values)
    {
      using (RegistryKey registryKey = rootKey.OpenSubKey(subKeyPath))
      {
        if (registryKey == null)
          return;
        string[] array;
        if (values.Keys.Count == 0)
        {
          array = registryKey.GetValueNames();
        }
        else
        {
          array = new string[values.Keys.Count];
          values.Keys.CopyTo(array, 0);
        }
        foreach (string str in array)
          values[str] = (object) (string) registryKey.GetValue(str);
      }
    }

    public static Dictionary<string, object> GetRegistryValues(
      RegistryKey rootKey,
      string subKeyPath,
      params string[] valNames)
    {
      Dictionary<string, object> values = new Dictionary<string, object>();
      if (valNames != null)
      {
        foreach (string valName in valNames)
          values.Add(valName, (object) null);
      }
      RegistryUtil.getRegistryValues(rootKey, subKeyPath, values);
      return values;
    }

    public static string[] GetSubKeyNames(RegistryKey rootKey, string subKeyPath)
    {
      using (RegistryKey registryKey = rootKey.OpenSubKey(subKeyPath, false))
        return registryKey?.GetSubKeyNames();
    }
  }
}
