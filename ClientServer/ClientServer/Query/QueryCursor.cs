// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Query.QueryCursor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Query
{
  [Serializable]
  public class QueryCursor : ICursor, IDisposable
  {
    private QueryResult.ColumnNameCollection columnNames;
    private ICursor dataCursor;

    public QueryCursor(QueryResult.ColumnNameCollection columnNames, ICursor dataCursor)
    {
      this.columnNames = columnNames;
      this.dataCursor = dataCursor;
    }

    public QueryResult.ColumnNameCollection Columns => this.columnNames;

    public object[] GetRow(int index, bool isExternalOrganization)
    {
      return (object[]) this.dataCursor.GetItem(index, isExternalOrganization);
    }

    public object[][] GetRows(int startIndex, int count, bool isExternalOrganization)
    {
      return (object[][]) this.dataCursor.GetItems(startIndex, count, isExternalOrganization);
    }

    public int GetRowCount() => this.dataCursor.GetItemCount();

    int ICursor.GetItemCount() => this.dataCursor.GetItemCount();

    int ICursor.GetItemCount(int sqlRead) => this.dataCursor.GetItemCount(sqlRead);

    object ICursor.GetItem(int index, bool isExternalOrganization)
    {
      return this.dataCursor.GetItem(index, isExternalOrganization);
    }

    object[] ICursor.GetItems(int startIndex, int count, bool isExternalOrganization)
    {
      return this.dataCursor.GetItems(startIndex, count, isExternalOrganization, 0);
    }

    object[] ICursor.GetItems(int startIndex, int count, bool isExternalOrganization, int sqlRead)
    {
      return this.dataCursor.GetItems(startIndex, count, isExternalOrganization, sqlRead);
    }

    object ICursor.GetItem(int index) => this.dataCursor.GetItem(index, false);

    object[] ICursor.GetItems(int startIndex, int count)
    {
      return this.dataCursor.GetItems(startIndex, count, false);
    }

    public void Dispose()
    {
      if (this.dataCursor == null)
        return;
      this.dataCursor.Dispose();
      this.dataCursor = (ICursor) null;
    }
  }
}
