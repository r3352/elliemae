// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EnAppSettings
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class EnAppSettings : ICollection, IEnumerable
  {
    private Hashtable appSettings = new Hashtable();
    private Hashtable clientSettings = new Hashtable();
    private string filePath;

    internal EnAppSettings() => this.filePath = "";

    internal EnAppSettings(string configFile)
    {
      if (File.Exists(configFile))
        this.load(configFile);
      this.filePath = configFile;
    }

    public string ConfigurationFile => this.filePath;

    public string this[string key] => (string) this.appSettings[(object) key];

    public bool HasKey(string key) => this.appSettings.ContainsKey((object) key);

    private void load(string configFile)
    {
      if (!new FileInfo(configFile).Exists)
        return;
      try
      {
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(configFile);
        this.parseAppSettings(xmldoc);
      }
      catch (XmlException ex)
      {
        throw new Exception("Error parsing application configuration file", (Exception) ex);
      }
    }

    private void parseAppSettings(XmlDocument xmldoc)
    {
      foreach (XmlElement xmlElement1 in xmldoc.DocumentElement.GetElementsByTagName("appSettings"))
      {
        foreach (XmlElement xmlElement2 in xmlElement1.GetElementsByTagName("add"))
        {
          if ((xmlElement2.GetAttribute("key") ?? "") != "")
          {
            string key = xmlElement2.GetAttribute("key") ?? "";
            this.appSettings[(object) key] = (object) (xmlElement2.GetAttribute("value") ?? "");
            if (key.StartsWith("$Client:", StringComparison.OrdinalIgnoreCase))
              this.clientSettings[(object) key.Split(':')[1]] = (object) (xmlElement2.GetAttribute("value") ?? "");
          }
        }
      }
    }

    public Hashtable ClientSettings => this.clientSettings;

    public bool IsSynchronized => false;

    public int Count => this.appSettings.Count;

    public void CopyTo(Array array, int index) => this.appSettings.CopyTo(array, index);

    public object SyncRoot => this.appSettings.SyncRoot;

    public IEnumerator GetEnumerator() => (IEnumerator) this.appSettings.GetEnumerator();

    public string ScannerUI
    {
      get
      {
        string str = this[nameof (ScannerUI)];
        return str == null || str.Trim() == "" ? "default" : str.Trim().ToLower();
      }
    }

    public bool DisableCrossAppDomainRemoting
    {
      get
      {
        if ((this[nameof (DisableCrossAppDomainRemoting)] ?? "") == "1")
          return true;
        return !((this[nameof (DisableCrossAppDomainRemoting)] ?? "") == "0") && EnConfigurationSettings.GlobalSettings.DisableCrossAppDomainRemoting;
      }
    }
  }
}
