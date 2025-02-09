// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.RegistryAccessor
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class RegistryAccessor
  {
    private RegistryKey rootKey;
    private string rootPath;

    public RegistryAccessor(RegistryKey rootKey)
      : this(rootKey, (string) null)
    {
    }

    public RegistryAccessor(RegistryKey rootKey, string rootPath)
    {
      this.rootKey = rootKey;
      this.rootPath = rootPath;
      if (string.IsNullOrEmpty(rootPath))
        return;
      while (rootPath.EndsWith("\\"))
        rootPath = !(rootPath == "\\") ? rootPath.Substring(0, rootPath.Length - 1) : "";
    }

    public RegistryKey RootKey => this.rootKey;

    public string RootPath => this.rootPath;

    public bool Exists()
    {
      using (RegistryKey registryKey = this.rootKey.OpenSubKey(this.rootPath))
        return registryKey != null;
    }

    public object ReadValue(string valuePath) => this.ReadValue(valuePath, (object) null);

    public object ReadValue(string valuePath, object defaultValue)
    {
      string keyPath;
      string keyValueName;
      this.parseKey(valuePath, out keyPath, out keyValueName);
      RegistryKey registryKey = this.rootKey.OpenSubKey(this.rootPath + keyPath);
      if (registryKey == null)
        return defaultValue;
      try
      {
        return registryKey.GetValue(keyValueName) ?? defaultValue;
      }
      finally
      {
        registryKey.Close();
      }
    }

    public void WriteValue(string valuePath, object value)
    {
      string keyPath;
      string keyValueName;
      this.parseKey(valuePath, out keyPath, out keyValueName);
      RegistryKey subKey = this.rootKey.CreateSubKey(this.rootPath + keyPath);
      if (subKey == null)
        throw new ApplicationException("Unable to create registry key " + this.rootPath + keyPath);
      try
      {
        if (value == null)
          subKey.DeleteValue(keyValueName);
        else
          subKey.SetValue(keyValueName, value);
      }
      finally
      {
        subKey.Close();
      }
    }

    private void parseKey(string key, out string keyPath, out string keyValueName)
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
