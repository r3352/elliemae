// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.MergeParamValues
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [Serializable]
  public class MergeParamValues
  {
    private Dictionary<string, string> _params = new Dictionary<string, string>();

    public MergeParamValues()
    {
    }

    public MergeParamValues(XmlElement paramsNode) => this.Load(paramsNode);

    public MergeParamValues(MergeParamValues other) => this.Load(other);

    public void Load(MergeParamValues other)
    {
      if (other == null)
        return;
      this.Load(other.ToXml());
    }

    public void Load(XmlElement paramsNode)
    {
      if (paramsNode == null)
        return;
      foreach (XmlElement selectNode in paramsNode.SelectNodes("pair"))
      {
        string attribute1 = selectNode.GetAttribute("key");
        string attribute2 = selectNode.GetAttribute("value");
        if (attribute1 != null)
          this._params[attribute1] = attribute2;
      }
    }

    public MergeParamValues Clone()
    {
      MergeParamValues mergeParamValues = new MergeParamValues();
      mergeParamValues.Load(this.ToXml());
      return mergeParamValues;
    }

    public XmlElement ToXml()
    {
      XmlElement xml = (XmlElement) null;
      if (this._params.Count > 0)
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.XmlResolver = (XmlResolver) null;
        xmlDocument.AppendChild((XmlNode) xmlDocument.CreateElement("MergeParams"));
        foreach (string key in this._params.Keys)
        {
          XmlElement xmlElement = (XmlElement) xmlDocument.DocumentElement.AppendChild((XmlNode) xmlDocument.CreateElement("pair"));
          xmlElement.SetAttribute("key", key);
          if (this._params[key] != null)
            xmlElement.SetAttribute("value", this._params[key]);
        }
        xml = xmlDocument.DocumentElement;
      }
      return xml;
    }

    public List<string> Keys
    {
      get
      {
        List<string> keys = new List<string>();
        keys.AddRange((IEnumerable<string>) this._params.Keys);
        return keys;
      }
    }

    public int Count => this._params.Count;

    public bool ContainsKey(string key) => this._params.ContainsKey(key);

    public bool Remove(string key) => this._params.Remove(key);

    public string this[string key]
    {
      get
      {
        string str = (string) null;
        if (key != null && this._params.ContainsKey(key))
          str = this._params[key];
        return str;
      }
      set => this._params[key] = value;
    }

    public void Add(MergeParamValues newValues)
    {
      if (newValues == null)
        return;
      foreach (string key in newValues.Keys)
        this._params[key] = newValues[key];
    }
  }
}
