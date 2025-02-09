// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.QueryResult
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class QueryResult
  {
    private QueryResult.ColumnNameCollection columns;
    private List<object[]> rows = new List<object[]>();

    public QueryResult(string[] columnNames)
    {
      this.columns = new QueryResult.ColumnNameCollection(columnNames);
    }

    public void AddRow(object[] data) => this.rows.Add(data);

    public void AddRow(DataRow row)
    {
      object[] data = new object[row.Table.Columns.Count];
      for (int columnIndex = 0; columnIndex < row.Table.Columns.Count; ++columnIndex)
        data[columnIndex] = !Convert.IsDBNull(row[columnIndex]) ? row[columnIndex] : (object) null;
      this.AddRow(data);
    }

    public int RecordCount => this.rows.Count;

    public QueryResult.ColumnNameCollection Columns => this.columns;

    public object this[int row, int column] => this.rows[row][column];

    public object this[int row, string columnName]
    {
      get => this.rows[row][this.columns.GetColumnIndex(columnName)];
    }

    public object[] GetRow(int index) => (object[]) this.rows[index].Clone();

    public object[][] GetRows(int startIndex, int count)
    {
      List<object[]> objArrayList = new List<object[]>();
      for (int index = startIndex; index < startIndex + count; ++index)
        objArrayList.Add((object[]) this.rows[index].Clone());
      return objArrayList.ToArray();
    }

    public DataTable ToDataTable()
    {
      DataTable dataTable = new DataTable();
      foreach (string column in (IEnumerable) this.Columns)
        dataTable.Columns.Add(column);
      for (int index = 0; index < this.RecordCount; ++index)
        dataTable.Rows.Add(this.rows[index]);
      return dataTable;
    }

    public QueryResult ExtractRows(int startIndex, int count)
    {
      QueryResult rows = new QueryResult(this.Columns.ToArray());
      for (int index = startIndex; index < startIndex + count; ++index)
        rows.AddRow((object[]) this.rows[index].Clone());
      return rows;
    }

    [Serializable]
    public class ColumnNameCollection : IEnumerable
    {
      private string[] names;
      [NonSerialized]
      private Dictionary<string, int> columnLookup;

      public ColumnNameCollection(string[] names) => this.names = names;

      public string this[int index] => this.names[index];

      public int Count => this.names.Length;

      public int GetColumnIndex(string name)
      {
        this.loadColumnLookup();
        return this.columnLookup.ContainsKey(name) ? this.columnLookup[name] : -1;
      }

      public string[] ToArray() => (string[]) this.names.Clone();

      IEnumerator IEnumerable.GetEnumerator() => this.names.GetEnumerator();

      private void loadColumnLookup()
      {
        if (this.columnLookup != null)
          return;
        this.columnLookup = new Dictionary<string, int>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        for (int index = 0; index < this.names.Length; ++index)
          this.columnLookup[this.names[index]] = index;
      }
    }
  }
}
