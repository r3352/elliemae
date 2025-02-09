// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.UserPipelineViewColumnCollection
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class UserPipelineViewColumnCollection : IEnumerable<UserPipelineViewColumn>, IEnumerable
  {
    private List<UserPipelineViewColumn> columns = new List<UserPipelineViewColumn>();

    public int Count => this.columns.Count;

    public void Clear() => this.columns.Clear();

    public UserPipelineViewColumn this[int index] => this.columns[index];

    public UserPipelineViewColumn Add(string columnDBName, int sortPriority)
    {
      return this.Add(columnDBName, SortOrder.None, sortPriority, -1, "");
    }

    public UserPipelineViewColumn Add(string columnDBName, SortOrder sortOrder, int sortPriority)
    {
      return this.Add(columnDBName, sortOrder, sortPriority, -1, "");
    }

    public UserPipelineViewColumn Add(
      string columnDBName,
      SortOrder sortOrder,
      int sortPriority,
      int width,
      string alignment = "�")
    {
      UserPipelineViewColumn pipelineViewColumn = new UserPipelineViewColumn(columnDBName);
      pipelineViewColumn.Width = width;
      pipelineViewColumn.Alignment = alignment;
      pipelineViewColumn.SortOrder = sortOrder;
      pipelineViewColumn.SortPriority = sortPriority;
      this.columns.Add(pipelineViewColumn);
      this.resetOrderIndex();
      return pipelineViewColumn;
    }

    public void Add(UserPipelineViewColumn newColumn)
    {
      if (newColumn.OrderIndex >= 0 && newColumn.OrderIndex < this.columns.Count)
        this.columns.Insert(newColumn.OrderIndex, newColumn);
      else
        this.columns.Add(newColumn);
      this.resetOrderIndex();
    }

    public void AddRange(UserPipelineViewColumn[] newColumns)
    {
      foreach (UserPipelineViewColumn pipelineViewColumn in (IEnumerable<UserPipelineViewColumn>) new List<UserPipelineViewColumn>((IEnumerable<UserPipelineViewColumn>) newColumns).OrderBy<UserPipelineViewColumn, int>((Func<UserPipelineViewColumn, int>) (o => o.orderIndex)))
      {
        this.columns.Add(pipelineViewColumn);
        this.resetOrderIndex();
      }
    }

    public void Remove(string columnDBName)
    {
      for (int index = this.columns.Count - 1; index >= 0; --index)
      {
        if (this.columns[index].ColumnDBName == columnDBName)
          this.columns.RemoveAt(index);
      }
      this.resetOrderIndex();
    }

    public void RemoveAt(int index)
    {
      this.columns.RemoveAt(index);
      this.resetOrderIndex();
    }

    public void Move(string columnDBName, int targetIndex)
    {
      for (int index = 0; index < this.columns.Count; ++index)
      {
        if (this.columns[index].ColumnDBName == columnDBName)
        {
          UserPipelineViewColumn column = this.columns[index];
          this.columns.RemoveAt(index);
          this.columns.Insert(targetIndex, column);
          this.resetOrderIndex();
          break;
        }
      }
    }

    public void MoveUp(string columnDBName)
    {
      for (int index = 1; index < this.columns.Count; ++index)
      {
        if (this.columns[index].ColumnDBName == columnDBName)
        {
          UserPipelineViewColumn column = this.columns[index];
          this.columns.RemoveAt(index);
          this.columns.Insert(index - 1, column);
          this.resetOrderIndex();
          break;
        }
      }
    }

    public void MoveDown(string columnDBName)
    {
      for (int index = 0; index < this.columns.Count - 1; ++index)
      {
        if (this.columns[index].ColumnDBName == columnDBName)
        {
          UserPipelineViewColumn column = this.columns[index];
          this.columns.RemoveAt(index);
          this.columns.Insert(index + 1, column);
          this.resetOrderIndex();
          break;
        }
      }
    }

    public void SetSortOrder(string dbColumnName, SortOrder sortOrder)
    {
      foreach (UserPipelineViewColumn column in this.columns)
        column.SetSortOrder(column.ColumnDBName == dbColumnName ? sortOrder : SortOrder.None);
    }

    public void SetSortOrder(int index, SortOrder sortOrder)
    {
      for (int index1 = 0; index1 < this.columns.Count; ++index1)
        this.columns[index1].SetSortOrder(index1 == index ? sortOrder : SortOrder.None);
    }

    public string[] GetFieldDBNames()
    {
      List<string> stringList = new List<string>();
      foreach (UserPipelineViewColumn column in this.columns)
        stringList.Add(column.ColumnDBName);
      return stringList.ToArray();
    }

    public IEnumerator<UserPipelineViewColumn> GetEnumerator()
    {
      return (IEnumerator<UserPipelineViewColumn>) this.columns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.columns.GetEnumerator();

    private void resetOrderIndex()
    {
      for (int index = 0; index < this.columns.Count; ++index)
        this.columns[index].SetOrderIndex(index);
    }
  }
}
