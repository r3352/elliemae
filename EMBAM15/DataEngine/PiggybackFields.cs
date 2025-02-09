// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.PiggybackFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class PiggybackFields : IXmlSerializable, IFieldSearchable
  {
    private Hashtable fields;

    public PiggybackFields()
    {
    }

    public PiggybackFields(XmlSerializationInfo info)
    {
      this.fields = new Hashtable();
      foreach (object obj in info)
      {
        string key = obj.ToString();
        if (key != "")
          this.fields.Add((object) key, (object) "");
      }
    }

    public int Count => this.fields != null ? this.fields.Count : 0;

    public void ClearFields()
    {
      if (this.fields == null)
        return;
      this.fields.Clear();
    }

    public void AddField(string id)
    {
      if (this.fields == null)
        this.fields = new Hashtable();
      if (this.fields.ContainsKey((object) id.ToUpper()))
        return;
      this.fields.Add((object) id.ToUpper(), (object) "");
    }

    public void RemoveField(string id)
    {
      if (this.fields == null || !this.fields.ContainsKey((object) id.ToUpper()))
        return;
      this.fields.Remove((object) id.ToUpper());
    }

    public string[] GetSyncFields() => this.GetSyncFields((List<string>) null);

    public string[] GetSyncFields(List<string> urla2020Fields)
    {
      if (this.fields == null)
        return (string[]) null;
      List<string> stringList = new List<string>();
      foreach (DictionaryEntry field in this.fields)
      {
        if (urla2020Fields == null || urla2020Fields.Count <= 0 || !urla2020Fields.Contains(field.Key.ToString()))
          stringList.Add(field.Key.ToString());
      }
      return stringList.ToArray();
    }

    public bool IsSycnField(string id)
    {
      return this.fields != null && this.fields.ContainsKey((object) id.ToUpper());
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      if (this.fields == null)
        return;
      foreach (DictionaryEntry field in this.fields)
        info.AddValue(field.Key.ToString(), (object) "");
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      foreach (object key in (IEnumerable) this.fields.Keys)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.AffectsValueOf, key.ToString());
    }
  }
}
