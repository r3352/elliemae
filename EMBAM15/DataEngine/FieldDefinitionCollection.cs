// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.FieldDefinitionCollection
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class FieldDefinitionCollection : IEnumerable
  {
    private ArrayList fieldList = new ArrayList();
    private Hashtable fieldDict = CollectionsUtil.CreateCaseInsensitiveHashtable();

    internal FieldDefinitionCollection()
    {
    }

    internal void Add(FieldDefinition field)
    {
      if (this.fieldDict.Contains((object) field.FieldID))
        throw new Exception("Field already exists with ID '" + field.FieldID + "'");
      this.fieldDict[(object) field.FieldID] = (object) field;
      this.fieldList.Add((object) field);
    }

    public FieldDefinition this[string fieldId]
    {
      get => (FieldDefinition) this.fieldDict[(object) fieldId];
    }

    public bool Contains(string fieldId) => this.fieldDict.Contains((object) fieldId);

    public int Count => this.fieldDict.Count;

    public IEnumerator GetEnumerator() => this.fieldList.GetEnumerator();

    public FieldDefinition[] ToSortedList()
    {
      ArrayList arrayList = new ArrayList((ICollection) this.fieldList);
      arrayList.Sort();
      return (FieldDefinition[]) arrayList.ToArray(typeof (FieldDefinition));
    }
  }
}
