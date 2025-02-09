// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.DocumentTrackingSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class DocumentTrackingSetup : 
    IEnumerable,
    IEnumerable<DocumentTemplate>,
    IXmlSerializable,
    IHashableContents
  {
    public Dictionary<string, DocumentTemplate> dictDocTrackByName = new Dictionary<string, DocumentTemplate>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private const string VERSIONKEY = "cs_ver_num";
    private const string DONOTCREATEINFODOCS = "DoNotCreateInfoDocs";
    private const string SAVECOPYINFODOCS = "SaveCopyInfoDocs";
    private const string APPLYTIMESTAMPTOINFODOCS = "ApplyTimeStampToInfoDocs";
    private const string USEBACKGROUNDCONVERSION = "UseBackgroundConversion";
    private const string IGNOREINTENDEDFOR = "IgnoreIntendedFor";
    private string version = "6.5.1";
    private bool doNotCreateInfoDocs;
    private bool saveCopyInfoDocs;
    private bool applyTimeStampToInfoDocs;
    private bool useBackgroundConversion;
    private bool ignoreIntendedFor;

    public DocumentTrackingSetup()
    {
    }

    int IHashableContents.GetContentsHashCode()
    {
      int a = ObjectArrayHelpers.GetAggregateHash((object) this.Version, (object) this.DoNotCreateInfoDocs, (object) this.SaveCopyInfoDocs, (object) this.ApplyTimeStampToInfoDocs, (object) this.UseBackgroundConversion, (object) this.IgnoreIntendedFor);
      foreach (KeyValuePair<string, DocumentTemplate> keyValuePair in this.dictDocTrackByName)
        a = ObjectArrayHelpers.AggregateHash(a, (object) keyValuePair.Value);
      return a;
    }

    public DocumentTrackingSetup(XmlSerializationInfo info)
    {
      this.version = info.GetString("cs_ver_num", "2.2.0");
      this.doNotCreateInfoDocs = info.GetBoolean(nameof (DoNotCreateInfoDocs), false);
      this.saveCopyInfoDocs = info.GetBoolean(nameof (SaveCopyInfoDocs), false);
      this.applyTimeStampToInfoDocs = info.GetBoolean(nameof (ApplyTimeStampToInfoDocs), false);
      this.useBackgroundConversion = info.GetBoolean(nameof (UseBackgroundConversion), false);
      this.ignoreIntendedFor = info.GetBoolean(nameof (IgnoreIntendedFor), false);
      foreach (string name in info)
      {
        if (name != "cs_ver_num" && name != nameof (DoNotCreateInfoDocs) && name != nameof (SaveCopyInfoDocs) && name != nameof (ApplyTimeStampToInfoDocs) && name != nameof (UseBackgroundConversion) && name != nameof (IgnoreIntendedFor))
          this.Add((DocumentTemplate) info.GetValue(name, typeof (DocumentTemplate)));
      }
    }

    public string Version
    {
      get => this.version;
      set => this.version = value;
    }

    public bool DoNotCreateInfoDocs
    {
      get => this.doNotCreateInfoDocs;
      set => this.doNotCreateInfoDocs = value;
    }

    public bool SaveCopyInfoDocs
    {
      get => this.saveCopyInfoDocs;
      set => this.saveCopyInfoDocs = value;
    }

    public bool ApplyTimeStampToInfoDocs
    {
      get => this.applyTimeStampToInfoDocs;
      set => this.applyTimeStampToInfoDocs = value;
    }

    public bool UseBackgroundConversion
    {
      get => this.useBackgroundConversion;
      set => this.useBackgroundConversion = value;
    }

    public bool IgnoreIntendedFor
    {
      get => this.ignoreIntendedFor;
      set => this.ignoreIntendedFor = value;
    }

    public void Add(DocumentTemplate template)
    {
      if (this.dictDocTrackByName.ContainsKey(template.Name))
        this.dictDocTrackByName[template.Name] = template;
      else
        this.dictDocTrackByName.Add(template.Name, template);
    }

    public void AddRange(IEnumerable<DocumentTemplate> templates)
    {
      foreach (DocumentTemplate template in templates)
        this.Add(template);
    }

    public DocumentTemplate this[string guid] => this.GetByID(guid);

    public DocumentTemplate GetByName(string name)
    {
      return string.IsNullOrEmpty(name) || !this.dictDocTrackByName.ContainsKey(name) ? (DocumentTemplate) null : this.dictDocTrackByName[name];
    }

    public DocumentTemplate GetByID(string guid)
    {
      foreach (DocumentTemplate byId in this.dictDocTrackByName.Values)
      {
        if (byId.Guid == guid)
          return byId;
      }
      return (DocumentTemplate) null;
    }

    public bool Contains(DocumentTemplate template)
    {
      return this.dictDocTrackByName.ContainsKey(template.Name);
    }

    public bool Contains(string name) => this.dictDocTrackByName.ContainsKey(name);

    public void Remove(DocumentTemplate template) => this.dictDocTrackByName.Remove(template.Name);

    public void Remove(string name) => this.dictDocTrackByName.Remove(name);

    public DocumentTemplate[] ToArray()
    {
      return new List<DocumentTemplate>((IEnumerable<DocumentTemplate>) this.dictDocTrackByName.Values).ToArray();
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("cs_ver_num", (object) this.version);
      info.AddValue("DoNotCreateInfoDocs", (object) this.doNotCreateInfoDocs);
      info.AddValue("SaveCopyInfoDocs", (object) this.saveCopyInfoDocs);
      info.AddValue("ApplyTimeStampToInfoDocs", (object) this.applyTimeStampToInfoDocs);
      info.AddValue("UseBackgroundConversion", (object) this.useBackgroundConversion);
      info.AddValue("IgnoreIntendedFor", (object) this.ignoreIntendedFor);
      int num = 0;
      foreach (DocumentTemplate documentTemplate in this.dictDocTrackByName.Values)
      {
        info.AddValue(num.ToString(), (object) documentTemplate);
        ++num;
      }
    }

    public object Clone()
    {
      XmlSerializer xmlSerializer = new XmlSerializer();
      return (object) (DocumentTrackingSetup) xmlSerializer.Deserialize(xmlSerializer.Serialize((object) this), this.GetType());
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) this.dictDocTrackByName.Values.GetEnumerator();
    }

    IEnumerator<DocumentTemplate> IEnumerable<DocumentTemplate>.GetEnumerator()
    {
      return (IEnumerator<DocumentTemplate>) this.dictDocTrackByName.Values.GetEnumerator();
    }
  }
}
