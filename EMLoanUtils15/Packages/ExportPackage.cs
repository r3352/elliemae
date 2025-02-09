// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Packages.ExportPackage
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Packages
{
  public class ExportPackage : IDisposable
  {
    private string tempDir = ExportPackage.createTempDir();
    private XmlDocument manifest;
    private ExportPackage.PackageForms forms;
    private ExportPackage.PackageAssemblies assemblies;
    private ExportPackage.PackageFields fields;
    private ExportPackage.PackagePlugins plugins;
    private ExportPackage.PackageCustomDataObjects customDataObjects;

    public ExportPackage()
    {
      this.manifest = new XmlDocument();
      this.manifest.LoadXml("<package/>");
      this.forms = new ExportPackage.PackageForms(this);
      this.assemblies = new ExportPackage.PackageAssemblies(this);
      this.fields = new ExportPackage.PackageFields(this);
      this.plugins = new ExportPackage.PackagePlugins(this);
      this.customDataObjects = new ExportPackage.PackageCustomDataObjects(this);
    }

    public ExportPackage(string path)
    {
      try
      {
        FileCompressor.Instance.Unzip(path, this.tempDir);
        this.manifest = new XmlDocument();
        this.manifest.Load(this.getManifestPath());
        this.forms = new ExportPackage.PackageForms(this);
        this.assemblies = new ExportPackage.PackageAssemblies(this);
        this.fields = new ExportPackage.PackageFields(this);
        this.plugins = new ExportPackage.PackagePlugins(this);
        this.customDataObjects = new ExportPackage.PackageCustomDataObjects(this);
      }
      catch (Exception ex)
      {
        throw new FormatException("The specified form package is invalid", ex);
      }
    }

    public ExportPackage.PackageForms Forms => this.forms;

    public ExportPackage.PackageAssemblies Assemblies => this.assemblies;

    public ExportPackage.PackageFields Fields => this.fields;

    public ExportPackage.PackagePlugins Plugins => this.plugins;

    public ExportPackage.PackageCustomDataObjects CustomDataObjects => this.customDataObjects;

    public bool HasItem
    {
      get
      {
        return this.Forms.Count > 0 || this.Assemblies.Count > 0 || this.Fields.Count > 0 || this.Plugins.Count > 0 || this.customDataObjects.Count > 0;
      }
    }

    public void Save(string path)
    {
      this.manifest.Save(this.getManifestPath());
      FileCompressor.Instance.ZipDirectory(this.tempDir, path);
    }

    private string getManifestPath() => Path.Combine(this.tempDir, "manifest.xml");

    private static string createTempDir()
    {
      string path = Path.Combine(SystemSettings.TempFolderRoot, "Packages\\" + Guid.NewGuid().ToString());
      Directory.CreateDirectory(path);
      return path;
    }

    public void Dispose()
    {
      try
      {
        Directory.Delete(this.tempDir, true);
      }
      catch
      {
      }
    }

    private class XmlNodeCollection : IEnumerable
    {
      private XmlDocument xml;
      private XmlElement rootElement;
      private string itemName;

      public XmlNodeCollection(XmlDocument xml, string rootName, string itemName)
      {
        this.xml = xml;
        this.itemName = itemName;
        this.rootElement = (XmlElement) xml.DocumentElement.SelectSingleNode(rootName);
        if (this.rootElement != null)
          return;
        this.rootElement = (XmlElement) xml.DocumentElement.AppendChild((XmlNode) xml.CreateElement(rootName));
      }

      public XmlElement Get(int index)
      {
        return (XmlElement) this.rootElement.SelectNodes(this.itemName)[index];
      }

      public XmlElement Get(string attributeName, string attributeValue)
      {
        foreach (XmlElement selectNode in this.rootElement.SelectNodes(this.itemName))
        {
          if ((selectNode.GetAttribute(attributeName) ?? "") == attributeValue)
            return selectNode;
        }
        return (XmlElement) null;
      }

      public XmlElement Create(string attributeName, string attributeValue)
      {
        XmlElement xmlElement1 = this.Get(attributeName, attributeValue);
        if (xmlElement1 != null)
          return xmlElement1;
        XmlElement xmlElement2 = (XmlElement) this.rootElement.AppendChild((XmlNode) this.xml.CreateElement(this.itemName));
        xmlElement2.SetAttribute(attributeName, attributeValue);
        return xmlElement2;
      }

      public void Remove(string attributeName, string attributeValue)
      {
        XmlElement oldChild = this.Get(attributeName, attributeValue);
        if (oldChild == null)
          return;
        this.rootElement.RemoveChild((XmlNode) oldChild);
      }

      public void Clear() => this.rootElement.RemoveAll();

      public IEnumerator GetEnumerator()
      {
        return this.rootElement.SelectNodes(this.itemName).GetEnumerator();
      }
    }

    public class PackageForms : IEnumerable
    {
      private ExportPackage pkg;
      private ArrayList forms = new ArrayList();
      private ExportPackage.XmlNodeCollection xmlData;

      internal PackageForms(ExportPackage pkg)
      {
        this.pkg = pkg;
        this.xmlData = new ExportPackage.XmlNodeCollection(pkg.manifest, "FormList", "Form");
        foreach (XmlElement xmlElement in this.xmlData)
          this.forms.Add((object) new InputFormInfo(xmlElement.GetAttribute("id"), xmlElement.GetAttribute("mname"), InputFormType.Custom));
      }

      public void Add(InputFormInfo form, BinaryObject formData)
      {
        string path2 = FileSystem.EncodeFilename(form.Name, false) + ".emfrm";
        formData.Write(Path.Combine(this.pkg.tempDir, path2));
        if (!this.forms.Contains((object) form))
          this.forms.Add((object) form);
        XmlElement xmlElement = this.xmlData.Create("id", form.FormID);
        xmlElement.SetAttribute("mname", form.MnemonicName);
        xmlElement.SetAttribute("fname", path2);
      }

      public void Clear()
      {
        foreach (string file in Directory.GetFiles(this.pkg.tempDir, "*.emfrm"))
          File.Delete(file);
        this.forms.Clear();
        this.xmlData.Clear();
      }

      public BinaryObject Extract(InputFormInfo formInfo)
      {
        XmlElement xmlElement = this.xmlData.Get("id", formInfo.FormID);
        if (xmlElement == null)
          return (BinaryObject) null;
        string path = Path.Combine(this.pkg.tempDir, xmlElement.GetAttribute("fname"));
        return !File.Exists(path) ? (BinaryObject) null : new BinaryObject(path);
      }

      public InputFormInfo this[int index] => this.forms[index] as InputFormInfo;

      public int Count => this.forms.Count;

      public IEnumerator GetEnumerator() => this.forms.GetEnumerator();
    }

    public class PackageAssemblies : IEnumerable
    {
      private ExportPackage pkg;
      private ArrayList assemblies = new ArrayList();
      private ExportPackage.XmlNodeCollection xmlData;

      internal PackageAssemblies(ExportPackage pkg)
      {
        this.pkg = pkg;
        this.xmlData = new ExportPackage.XmlNodeCollection(pkg.manifest, "AssemblyList", "Assembly");
        foreach (XmlElement xmlElement in this.xmlData)
          this.assemblies.Add((object) xmlElement.GetAttribute("name"));
      }

      public void Add(string name, BinaryObject assemblyData)
      {
        string path2 = name + ".dll";
        string str = Path.Combine(this.pkg.tempDir, path2);
        assemblyData.Write(str);
        if (!this.assemblies.Contains((object) name))
          this.assemblies.Add((object) name);
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(str);
        XmlElement xmlElement = this.xmlData.Create(nameof (name), name);
        xmlElement.SetAttribute("fname", path2);
        xmlElement.SetAttribute("version", versionInfo.FileVersion);
      }

      public void Clear()
      {
        foreach (string file in Directory.GetFiles(this.pkg.tempDir, "*.dll"))
          File.Delete(file);
        this.assemblies.Clear();
        this.xmlData.Clear();
      }

      public Version VerionOf(string name)
      {
        XmlElement xmlElement = this.xmlData.Get(nameof (name), name);
        return xmlElement == null ? (Version) null : new Version(xmlElement.GetAttribute("version"));
      }

      public BinaryObject Extract(string name)
      {
        XmlElement xmlElement = this.xmlData.Get(nameof (name), name);
        if (xmlElement == null)
          return (BinaryObject) null;
        string path = Path.Combine(this.pkg.tempDir, xmlElement.GetAttribute("fname"));
        return !File.Exists(path) ? (BinaryObject) null : new BinaryObject(path);
      }

      public string this[int index] => this.assemblies[index] as string;

      public int Count => this.assemblies.Count;

      public IEnumerator GetEnumerator() => this.assemblies.GetEnumerator();
    }

    public class PackagePlugins : IEnumerable
    {
      private const string packageFolder = "Plugins�";
      private ExportPackage pkg;
      private ArrayList plugins = new ArrayList();
      private ExportPackage.XmlNodeCollection xmlData;

      internal PackagePlugins(ExportPackage pkg)
      {
        this.pkg = pkg;
        this.xmlData = new ExportPackage.XmlNodeCollection(pkg.manifest, "PluginList", "Plugin");
        foreach (XmlElement xmlElement in this.xmlData)
          this.plugins.Add((object) xmlElement.GetAttribute("name"));
      }

      public void Add(string name, BinaryObject assemblyData)
      {
        string str1 = name + ".dll";
        string str2 = Path.Combine(this.pkg.tempDir, "Plugins\\" + str1);
        Directory.CreateDirectory(Path.GetDirectoryName(str2));
        assemblyData.Write(str2);
        if (!this.plugins.Contains((object) name))
          this.plugins.Add((object) name);
        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(str2);
        XmlElement xmlElement = this.xmlData.Create(nameof (name), name);
        xmlElement.SetAttribute("fname", str1);
        xmlElement.SetAttribute("version", versionInfo.FileVersion);
      }

      public void Clear()
      {
        string path = Path.Combine(this.pkg.tempDir, "Plugins");
        if (Directory.Exists(path))
        {
          foreach (string file in Directory.GetFiles(path, "*.dll"))
            File.Delete(file);
        }
        this.plugins.Clear();
        this.xmlData.Clear();
      }

      public Version VerionOf(string name)
      {
        XmlElement xmlElement = this.xmlData.Get(nameof (name), name);
        return xmlElement == null ? (Version) null : new Version(xmlElement.GetAttribute("version"));
      }

      public BinaryObject Extract(string name)
      {
        XmlElement xmlElement = this.xmlData.Get(nameof (name), name);
        if (xmlElement == null)
          return (BinaryObject) null;
        string path = Path.Combine(this.pkg.tempDir, "Plugins\\" + xmlElement.GetAttribute("fname"));
        return !File.Exists(path) ? (BinaryObject) null : new BinaryObject(path);
      }

      public string this[int index] => this.plugins[index] as string;

      public int Count => this.plugins.Count;

      public IEnumerator GetEnumerator() => this.plugins.GetEnumerator();
    }

    public class PackageFields : IEnumerable
    {
      private ExportPackage pkg;
      private ArrayList fields = new ArrayList();
      private CustomFieldsInfo fieldsInfo;

      internal PackageFields(ExportPackage pkg)
      {
        this.pkg = pkg;
        this.loadFromManifest();
      }

      public void Add(CustomFieldInfo field)
      {
        if (this.fields.Contains((object) field))
          this.fields.Remove((object) field);
        this.fields.Add((object) field);
        this.fieldsInfo.Add(field);
        this.writeFieldsToManifest();
      }

      public void Clear()
      {
        this.fields.Clear();
        this.fieldsInfo = new CustomFieldsInfo(true);
        this.writeFieldsToManifest();
      }

      public CustomFieldInfo this[int index] => this.fields[index] as CustomFieldInfo;

      public int Count => this.fields.Count;

      public bool Contains(CustomFieldInfo field) => this.fields.Contains((object) field);

      public IEnumerator GetEnumerator() => this.fields.GetEnumerator();

      private void loadFromManifest()
      {
        this.fieldsInfo = new CustomFieldsInfo(((XmlElement) this.pkg.manifest.DocumentElement.SelectSingleNode("CustomFieldList") ?? (XmlElement) this.pkg.manifest.DocumentElement.AppendChild((XmlNode) this.pkg.manifest.CreateElement("CustomFieldList"))).OuterXml);
        foreach (CustomFieldInfo customFieldInfo in this.fieldsInfo)
        {
          if (!customFieldInfo.IsEmpty())
            this.fields.Add((object) customFieldInfo);
        }
      }

      private void writeFieldsToManifest()
      {
        XmlElement oldChild = (XmlElement) this.pkg.manifest.DocumentElement.SelectSingleNode("CustomFieldList");
        oldChild?.ParentNode.RemoveChild((XmlNode) oldChild);
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(this.fieldsInfo.ToString());
        this.pkg.manifest.DocumentElement.AppendChild(this.pkg.manifest.ImportNode((XmlNode) xmlDocument.DocumentElement, true));
      }
    }

    public class PackageCustomDataObjects : IEnumerable
    {
      private const string packageFolder = "CustomData�";
      private ExportPackage pkg;
      private ArrayList customDataObjects = new ArrayList();
      private ExportPackage.XmlNodeCollection xmlData;

      internal PackageCustomDataObjects(ExportPackage pkg)
      {
        this.pkg = pkg;
        this.xmlData = new ExportPackage.XmlNodeCollection(pkg.manifest, "CustomDataObjectList", "CustomDataObject");
        foreach (XmlElement xmlElement in this.xmlData)
          this.customDataObjects.Add((object) xmlElement.GetAttribute("name"));
      }

      public void Add(string name, BinaryObject assemblyData)
      {
        string path = Path.Combine(this.pkg.tempDir, "CustomData\\" + name);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        assemblyData.Write(path);
        if (!this.customDataObjects.Contains((object) name))
          this.customDataObjects.Add((object) name);
        this.xmlData.Create(nameof (name), name).SetAttribute("fname", name);
      }

      public void Clear()
      {
        string path = Path.Combine(this.pkg.tempDir, "CustomData");
        if (Directory.Exists(path))
        {
          foreach (string file in Directory.GetFiles(path, "*.*"))
            File.Delete(file);
        }
        this.customDataObjects.Clear();
        this.xmlData.Clear();
      }

      public Version VerionOf(string name)
      {
        XmlElement xmlElement = this.xmlData.Get(nameof (name), name);
        return xmlElement == null ? (Version) null : new Version(xmlElement.GetAttribute("version"));
      }

      public BinaryObject Extract(string name)
      {
        XmlElement xmlElement = this.xmlData.Get(nameof (name), name);
        if (xmlElement == null)
          return (BinaryObject) null;
        string path = Path.Combine(this.pkg.tempDir, "CustomData\\" + xmlElement.GetAttribute("fname"));
        return !File.Exists(path) ? (BinaryObject) null : new BinaryObject(path);
      }

      public string this[int index] => this.customDataObjects[index] as string;

      public int Count => this.customDataObjects.Count;

      public IEnumerator GetEnumerator() => this.customDataObjects.GetEnumerator();
    }
  }
}
