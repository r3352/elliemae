// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DocEngine.DocEngineFieldData
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DocEngine
{
  public class DocEngineFieldData : DocEngineEntity
  {
    private Dictionary<string, string> fieldValues = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private Dictionary<string, string> settingsValues = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    public DocEngineFieldData(XmlDocument responseXml)
      : this((XmlElement) responseXml.DocumentElement.SelectSingleNode("//OverrideFields"))
    {
    }

    internal DocEngineFieldData(XmlElement xml)
      : base(xml)
    {
      foreach (XmlElement selectNode in xml.SelectNodes(".//Field"))
      {
        this.fieldValues[selectNode.GetAttribute("EncompassId")] = selectNode.GetAttribute("value");
        this.settingsValues[selectNode.GetAttribute("EncompassId")] = selectNode.GetAttribute("noLoanValue");
      }
    }

    public string GetField(string encfieldId)
    {
      return this.fieldValues.ContainsKey(encfieldId) ? this.fieldValues[encfieldId] : "";
    }

    public string GetSettingsValue(string encfieldId)
    {
      return this.settingsValues.ContainsKey(encfieldId) ? this.settingsValues[encfieldId] : "";
    }

    public void SetSettingsValue(string encfieldId, string value)
    {
      this.settingsValues[encfieldId] = value;
    }

    public bool ContainsField(string encfieldId) => this.fieldValues.ContainsKey(encfieldId);

    public Dictionary<string, string> ToDictionary()
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (string key in this.fieldValues.Keys)
        dictionary[key] = this.fieldValues[key];
      return dictionary;
    }
  }
}
