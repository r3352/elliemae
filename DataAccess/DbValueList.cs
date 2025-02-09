// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbValueList
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using System;
using System.Collections;
using System.Collections.Specialized;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbValueList : IEnumerable
  {
    private ArrayList values = new ArrayList();
    private Hashtable valueMap = CollectionsUtil.CreateCaseInsensitiveHashtable();

    public DbValueList()
    {
    }

    public DbValueList(DbValue[] values)
    {
      for (int index = 0; index < values.Length; ++index)
        this.Add(values[index]);
    }

    public DbValueList(DbValueList values)
    {
      foreach (DbValue dbValue in values)
        this.Add(dbValue);
    }

    public DbValueList(Hashtable values)
    {
      foreach (DictionaryEntry dictionaryEntry in values)
        this.Add(dictionaryEntry.Key.ToString(), dictionaryEntry.Value);
    }

    public DbValue this[int index]
    {
      get => (DbValue) this.values[index];
      set => this.values[index] = (object) value;
    }

    public DbValue Add(DbValue value)
    {
      if (this.Contains(value.ColumnName))
        throw new ArgumentException("Value for column already in collection: column = " + value.ColumnName);
      this.values.Add((object) value);
      this.valueMap.Add((object) value.ColumnName, (object) value);
      return value;
    }

    public DbValue Add(string columnName, object value) => this.Add(new DbValue(columnName, value));

    public DbValue Add(string columnName, object value, IDbEncoder encoder)
    {
      return this.Add(new DbValue(columnName, value, encoder));
    }

    public void Add(DbValueList values)
    {
      foreach (DbValue dbValue in values)
        this.Add(dbValue);
    }

    public int Count => this.values.Count;

    public bool Contains(string columnName) => this.valueMap.Contains((object) columnName);

    public void Remove(string columnName)
    {
      DbValue dbValue = (DbValue) this.valueMap[(object) columnName];
      if (dbValue == null)
        return;
      this.values.Remove((object) dbValue);
      this.valueMap.Remove((object) columnName);
    }

    public void Clear()
    {
      this.values.Clear();
      this.valueMap.Clear();
    }

    public DbValueList Clone() => new DbValueList(this);

    public IEnumerator GetEnumerator() => this.values.GetEnumerator();
  }
}
